using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Utilizadores;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Consulta, criação, atualização e bloqueio de contas de funcionário (EP-004 a
/// EP-008). Só são consideradas as contas com registo em <c>Funcionario</c>; as
/// contas de Cliente são geridas em EP-011 a EP-014.
/// </summary>
public sealed class UtilizadoresService : IUtilizadoresService
{
    private const string MensagemNaoEncontrado = "A conta de funcionário indicada não existe.";
    private const string MensagemEmailRegistado = "Já existe uma conta registada com este e-mail.";
    private const string MensagemEmailUsado = "O e-mail indicado já é usado por outra conta.";

    // Violação de índice único no SQL Server (UQ_Utilizador_Email).
    private const int ErroChaveDuplicada = 2601;
    private const int ErroRestricaoUnica = 2627;

    private static readonly IReadOnlyList<string> Estados = [EstadosUtilizador.Ativo, EstadosUtilizador.Bloqueado];

    private readonly ClinicaDbContext _db;
    private readonly IPasswordHasher<Utilizador> _passwordHasher;
    private readonly TimeProvider _relogio;

    public UtilizadoresService(
        ClinicaDbContext db,
        IPasswordHasher<Utilizador> passwordHasher,
        TimeProvider relogio)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _relogio = relogio;
    }

    public async Task<PagedResultDto<UtilizadorListItemDto>> ListarAsync(UtilizadorFiltroQuery filtro)
    {
        ArgumentNullException.ThrowIfNull(filtro);

        var perfil = NormalizarFiltro(filtro.Perfil, Perfis.Todos, "perfil");
        var estado = NormalizarFiltro(filtro.Estado, Estados, "estado");

        var consulta =
            from u in _db.Utilizadores.AsNoTracking()
            join f in _db.Funcionarios.AsNoTracking() on u.Id equals f.UtilizadorId
            // Garante que uma conta de Cliente nunca aparece, mesmo com dados incoerentes.
            where u.Perfil != Perfis.Cliente
            select new { Utilizador = u, Funcionario = f };

        if (perfil is not null)
        {
            consulta = consulta.Where(x => x.Utilizador.Perfil == perfil);
        }

        if (estado is not null)
        {
            consulta = consulta.Where(x => x.Utilizador.Estado == estado);
        }

        var termo = filtro.Pesquisa?.Trim();
        if (!string.IsNullOrEmpty(termo))
        {
            consulta = consulta.Where(x =>
                x.Funcionario.Nome.Contains(termo) || x.Utilizador.Email.Contains(termo));
        }

        var total = await consulta.CountAsync();

        // Em long: com uma página muito alta, (pagina - 1) * tamanho ultrapassava o limite
        // de int e dava um salto negativo (erro 500). Depois do fim, a página fica vazia.
        var salto = (long)(filtro.Pagina - 1) * filtro.Tamanho;

        List<UtilizadorListItemDto> itens = [];

        if (salto < total)
        {
            // Ordenação estável (critério 5): nome e, em caso de empate, identificador.
            itens = await consulta
                .OrderBy(x => x.Funcionario.Nome)
                .ThenBy(x => x.Utilizador.Id)
                .Skip((int)salto)
                .Take(filtro.Tamanho)
                .Select(x => new UtilizadorListItemDto
                {
                    Id = x.Utilizador.Id,
                    Nome = x.Funcionario.Nome,
                    Email = x.Utilizador.Email,
                    Perfil = x.Utilizador.Perfil,
                    Estado = x.Utilizador.Estado,
                    PodeRegistarApoio = x.Funcionario.PodeRegistarApoio
                })
                .ToListAsync();
        }

        return new PagedResultDto<UtilizadorListItemDto>
        {
            Itens = itens,
            Pagina = filtro.Pagina,
            Tamanho = filtro.Tamanho,
            Total = total
        };
    }

    public async Task<UtilizadorDto> ObterAsync(int id)
    {
        var dados = await (
                from u in _db.Utilizadores.AsNoTracking()
                join f in _db.Funcionarios.AsNoTracking() on u.Id equals f.UtilizadorId
                where u.Id == id && u.Perfil != Perfis.Cliente
                select new { Utilizador = u, Funcionario = f })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemNaoEncontrado);

        return new UtilizadorDto
        {
            Id = dados.Utilizador.Id,
            Nome = dados.Funcionario.Nome,
            Email = dados.Utilizador.Email,
            Telefone = dados.Funcionario.Telefone,
            Perfil = dados.Utilizador.Perfil,
            Estado = dados.Utilizador.Estado,
            PodeRegistarApoio = dados.Funcionario.PodeRegistarApoio,
            CriadoEm = DateTime.SpecifyKind(dados.Utilizador.DataCriacao, DateTimeKind.Utc)
        };
    }

    public async Task<UtilizadorListItemDto> CriarAsync(CreateUtilizadorDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        var perfil = NormalizarFiltro(pedido.Perfil, Perfis.Todos, "perfil")
            ?? throw new ArgumentException("O perfil é obrigatório.");

        // Regras de negócio do contrato (422), verificadas antes da unicidade do e-mail.
        if (perfil == Perfis.Cliente)
        {
            throw new UnprocessableEntityException(
                "O perfil Cliente não pode ser atribuído neste endpoint; as contas de Cliente são criadas em POST /clientes.");
        }

        GarantirApoioCoerente(perfil, pedido.PodeRegistarApoio);

        var email = pedido.Email.Trim().ToLowerInvariant();

        if (await _db.Utilizadores.AnyAsync(u => u.Email == email))
        {
            throw new InvalidOperationException(MensagemEmailRegistado);
        }

        var utilizador = new Utilizador
        {
            Email = email,
            Perfil = perfil,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = Agora()
        };
        utilizador.PasswordHash = _passwordHasher.HashPassword(utilizador, pedido.PasswordInicial);

        var funcionario = new Funcionario
        {
            Nome = pedido.Nome.Trim(),
            Telefone = string.IsNullOrWhiteSpace(pedido.Telefone) ? null : pedido.Telefone.Trim(),
            PodeRegistarApoio = pedido.PodeRegistarApoio
        };

        // Utilizador e Funcionario são criados juntos ou não é criado nenhum.
        await using var transacao = await _db.Database.BeginTransactionAsync();

        _db.Utilizadores.Add(utilizador);
        await GravarAsync();

        funcionario.UtilizadorId = utilizador.Id;
        _db.Funcionarios.Add(funcionario);
        await GravarAsync();

        await transacao.CommitAsync();

        return new UtilizadorListItemDto
        {
            Id = utilizador.Id,
            Nome = funcionario.Nome,
            Email = utilizador.Email,
            Perfil = utilizador.Perfil,
            Estado = utilizador.Estado,
            PodeRegistarApoio = funcionario.PodeRegistarApoio
        };
    }

    public async Task<UtilizadorDto> AtualizarAsync(int id, int administradorId, UpdateUtilizadorDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        var perfil = NormalizarFiltro(pedido.Perfil, Perfis.Todos, "perfil")
            ?? throw new ArgumentException("O perfil é obrigatório.");

        // Em EP-007 o contrato prevê 400 para o perfil Cliente (em EP-006 é 422).
        if (perfil == Perfis.Cliente)
        {
            throw new ArgumentException(
                "O perfil Cliente não pode ser atribuído a uma conta de funcionário; as contas de Cliente são geridas em /clientes.");
        }

        var dados = await (
                from u in _db.Utilizadores
                join f in _db.Funcionarios on u.Id equals f.UtilizadorId
                where u.Id == id && u.Perfil != Perfis.Cliente
                select new { Utilizador = u, Funcionario = f })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemNaoEncontrado);

        // Regras de negócio do contrato (422), verificadas antes da unicidade do e-mail.
        if (id == administradorId && perfil != Perfis.Administrador)
        {
            throw new UnprocessableEntityException(
                "O Administrador não pode remover o próprio perfil de Administrador.");
        }

        // Verificada em cada atualização, incluindo na mudança de perfil (critério 3):
        // um Auxiliar que muda de perfil tem de deixar de ter a autorização.
        GarantirApoioCoerente(perfil, pedido.PodeRegistarApoio);

        var email = pedido.Email.Trim().ToLowerInvariant();

        // O próprio e-mail não conta como conflito.
        if (await _db.Utilizadores.AnyAsync(u => u.Email == email && u.Id != id))
        {
            throw new InvalidOperationException(MensagemEmailUsado);
        }

        dados.Utilizador.Email = email;
        dados.Utilizador.Perfil = perfil;
        dados.Funcionario.Nome = pedido.Nome.Trim();
        dados.Funcionario.Telefone = string.IsNullOrWhiteSpace(pedido.Telefone) ? null : pedido.Telefone.Trim();
        dados.Funcionario.PodeRegistarApoio = pedido.PodeRegistarApoio;

        // Um único SaveChanges: Utilizador e Funcionario mudam na mesma transação.
        // Um token emitido com o perfil antigo deixa de ser aceite no pedido seguinte (EP-003).
        await GravarAsync(MensagemEmailUsado);

        return new UtilizadorDto
        {
            Id = dados.Utilizador.Id,
            Nome = dados.Funcionario.Nome,
            Email = dados.Utilizador.Email,
            Telefone = dados.Funcionario.Telefone,
            Perfil = dados.Utilizador.Perfil,
            Estado = dados.Utilizador.Estado,
            PodeRegistarApoio = dados.Funcionario.PodeRegistarApoio,
            CriadoEm = DateTime.SpecifyKind(dados.Utilizador.DataCriacao, DateTimeKind.Utc)
        };
    }

    public async Task BloquearAsync(int id, int administradorId, AlterarEstadoUtilizadorDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        // EP-008 só bloqueia; o desbloqueio não faz parte do contrato.
        if (!string.Equals(pedido.Estado?.Trim(), EstadosUtilizador.Bloqueado, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                $"O estado '{pedido.Estado?.Trim()}' é inválido. Neste endpoint só é aceite {EstadosUtilizador.Bloqueado}.");
        }

        var utilizador = await _db.Utilizadores.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new KeyNotFoundException(MensagemNaoEncontrado);

        // Ao contrário de EP-005 e EP-007, uma conta de Cliente existe e dá 422 (contrato de EP-008).
        if (utilizador.Perfil == Perfis.Cliente)
        {
            throw new UnprocessableEntityException(
                "As contas de Cliente não podem ser bloqueadas neste endpoint; só as contas de funcionário.");
        }

        if (id == administradorId)
        {
            throw new UnprocessableEntityException("O Administrador não pode bloquear a própria conta.");
        }

        if (utilizador.Estado == EstadosUtilizador.Bloqueado)
        {
            throw new UnprocessableEntityException("A conta já está bloqueada.");
        }

        utilizador.Estado = EstadosUtilizador.Bloqueado;

        // Efeito imediato: EP-001 passa a devolver 403 e os tokens já emitidos são
        // rejeitados com 401 no pedido seguinte, porque cada pedido confirma o estado da conta.
        await _db.SaveChangesAsync();
    }

    /// <summary>A autorização de registo de apoio só existe em contas de Auxiliar Veterinário (422).</summary>
    private static void GarantirApoioCoerente(string perfil, bool podeRegistarApoio)
    {
        if (podeRegistarApoio && perfil != Perfis.AuxiliarVeterinario)
        {
            throw new UnprocessableEntityException(
                "A autorização podeRegistarApoio só pode ser concedida ao perfil AuxiliarVeterinario.");
        }
    }

    /// <summary>
    /// Devolve o valor da enumeração com a grafia oficial (sem distinguir maiúsculas),
    /// null se não foi indicado, ou 400 se não pertence à enumeração (critério 5).
    /// </summary>
    private static string? NormalizarFiltro(string? valor, IReadOnlyList<string> permitidos, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        var texto = valor.Trim();

        return permitidos.FirstOrDefault(p => string.Equals(p, texto, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"O {campo} '{texto}' é inválido. Valores permitidos: {string.Join(", ", permitidos)}.");
    }

    /// <summary>
    /// Grava as alterações; um pedido simultâneo com o mesmo e-mail, que passou a
    /// verificação anterior, é travado pelo índice único e devolve 409.
    /// </summary>
    private async Task GravarAsync(string mensagemConflito = MensagemEmailRegistado)
    {
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException
        {
            Number: ErroChaveDuplicada or ErroRestricaoUnica
        })
        {
            throw new InvalidOperationException(mensagemConflito, ex);
        }
    }

    private DateTime Agora()
    {
        // datetime2(0): sem frações de segundo.
        var agora = _relogio.GetUtcNow().UtcDateTime;
        return new DateTime(agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
    }
}

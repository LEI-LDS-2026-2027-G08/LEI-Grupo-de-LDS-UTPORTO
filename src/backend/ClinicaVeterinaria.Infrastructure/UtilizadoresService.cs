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
/// Consulta e criação de contas de funcionário (EP-004 a EP-006). Só são
/// consideradas as contas com registo em <c>Funcionario</c>; as contas de
/// Cliente são geridas em EP-011 a EP-014.
/// </summary>
public sealed class UtilizadoresService : IUtilizadoresService
{
    private const string MensagemNaoEncontrado = "A conta de funcionário indicada não existe.";
    private const string MensagemEmailRegistado = "Já existe uma conta registada com este e-mail.";

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

        // Ordenação estável (critério 5): nome e, em caso de empate, identificador.
        var itens = await consulta
            .OrderBy(x => x.Funcionario.Nome)
            .ThenBy(x => x.Utilizador.Id)
            .Skip((filtro.Pagina - 1) * filtro.Tamanho)
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
                where u.Id == id
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

        if (pedido.PodeRegistarApoio && perfil != Perfis.AuxiliarVeterinario)
        {
            throw new UnprocessableEntityException(
                "A autorização podeRegistarApoio só pode ser concedida ao perfil AuxiliarVeterinario.");
        }

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
    private async Task GravarAsync()
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
            throw new InvalidOperationException(MensagemEmailRegistado, ex);
        }
    }

    private DateTime Agora()
    {
        // datetime2(0): sem frações de segundo.
        var agora = _relogio.GetUtcNow().UtcDateTime;
        return new DateTime(agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
    }
}

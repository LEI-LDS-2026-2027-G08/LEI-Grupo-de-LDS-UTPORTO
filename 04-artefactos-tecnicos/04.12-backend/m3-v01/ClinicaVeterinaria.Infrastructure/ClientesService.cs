using ClinicaVeterinaria.Application.DTOs.Clientes;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Gestão das contas de Cliente (EP-011 a EP-014). O e-mail do Cliente é o da
/// conta <c>Utilizador</c> associada, que é a fonte única do dado.
/// </summary>
public sealed class ClientesService : IClientesService
{
    private const string MensagemNaoEncontrado = "O cliente indicado não existe.";
    private const string MensagemEmailRegistado = "Já existe uma conta registada com este e-mail.";
    private const string MensagemNifRegistado = "Já existe um cliente registado com este NIF.";

    // Violação de índice único no SQL Server (UQ_Utilizador_Email, UQ_Cliente_NIF).
    private const int ErroChaveDuplicada = 2601;
    private const int ErroRestricaoUnica = 2627;

    private readonly ClinicaDbContext _db;
    private readonly IPasswordHasher<Utilizador> _passwordHasher;
    private readonly TimeProvider _relogio;

    public ClientesService(
        ClinicaDbContext db,
        IPasswordHasher<Utilizador> passwordHasher,
        TimeProvider relogio)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _relogio = relogio;
    }

    public async Task<PagedResultDto<ClienteListItemDto>> PesquisarAsync(ClienteFiltroQuery filtro)
    {
        ArgumentNullException.ThrowIfNull(filtro);

        var consulta =
            from c in _db.Clientes.AsNoTracking()
            join u in _db.Utilizadores.AsNoTracking() on c.UtilizadorId equals u.Id
            select new { Cliente = c, u.Email };

        var termo = filtro.Pesquisa?.Trim();
        if (!string.IsNullOrEmpty(termo))
        {
            consulta = consulta.Where(x =>
                x.Cliente.Nome.Contains(termo)
                || (x.Cliente.NIF != null && x.Cliente.NIF.Contains(termo))
                || x.Cliente.Telefone.Contains(termo)
                || x.Email.Contains(termo));
        }

        var total = await consulta.CountAsync();

        var itens = await consulta
            .OrderBy(x => x.Cliente.Nome)
            .ThenBy(x => x.Cliente.Id)
            .Skip((filtro.Pagina - 1) * filtro.Tamanho)
            .Take(filtro.Tamanho)
            .Select(x => new ClienteListItemDto
            {
                Id = x.Cliente.Id,
                Nome = x.Cliente.Nome,
                Nif = x.Cliente.NIF,
                Telefone = x.Cliente.Telefone,
                Email = x.Email,
                NumeroAnimais = _db.Pacientes.Count(p => p.ClienteId == x.Cliente.Id)
            })
            .ToListAsync();

        return new PagedResultDto<ClienteListItemDto>
        {
            Itens = itens,
            Pagina = filtro.Pagina,
            Tamanho = filtro.Tamanho,
            Total = total
        };
    }

    public async Task<ClienteDto> ObterAsync(int id)
    {
        var dados = await (
                from c in _db.Clientes.AsNoTracking()
                join u in _db.Utilizadores.AsNoTracking() on c.UtilizadorId equals u.Id
                where c.Id == id
                select new { Cliente = c, Utilizador = u })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemNaoEncontrado);

        return await CriarClienteDtoAsync(dados.Cliente, dados.Utilizador);
    }

    public async Task<ClienteDto> CriarAsync(CreateClienteDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        var email = NormalizarEmail(pedido.Email);
        var nif = NormalizarNif(pedido.Nif);

        await GarantirEmailLivreAsync(email, utilizadorAtualId: null);
        await GarantirNifLivreAsync(nif, clienteAtualId: null);

        var agora = Agora();

        var utilizador = new Utilizador
        {
            Email = email,
            Perfil = Perfis.Cliente,
            Estado = EstadosUtilizador.Ativo,
            DataCriacao = agora
        };
        utilizador.PasswordHash = _passwordHasher.HashPassword(utilizador, pedido.PasswordInicial);

        var cliente = new Cliente
        {
            Nome = pedido.Nome.Trim(),
            NIF = nif,
            Telefone = pedido.Telefone.Trim(),
            Morada = NormalizarOpcional(pedido.Morada),
            DataRegisto = agora
        };

        // Utilizador e Cliente são criados juntos ou não é criado nenhum.
        await using var transacao = await _db.Database.BeginTransactionAsync();

        _db.Utilizadores.Add(utilizador);
        await GravarAsync();

        cliente.UtilizadorId = utilizador.Id;
        _db.Clientes.Add(cliente);
        await GravarAsync();

        await transacao.CommitAsync();

        return await CriarClienteDtoAsync(cliente, utilizador);
    }

    public async Task<ClienteDto> AtualizarAsync(int id, UpdateClienteDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException(MensagemNaoEncontrado);

        var utilizador = await _db.Utilizadores.FirstAsync(u => u.Id == cliente.UtilizadorId);

        var email = NormalizarEmail(pedido.Email);
        var nif = NormalizarNif(pedido.Nif);

        await GarantirEmailLivreAsync(email, utilizador.Id);
        await GarantirNifLivreAsync(nif, cliente.Id);

        cliente.Nome = pedido.Nome.Trim();
        cliente.NIF = nif;
        cliente.Telefone = pedido.Telefone.Trim();
        cliente.Morada = NormalizarOpcional(pedido.Morada);
        utilizador.Email = email;

        // Um único SaveChanges: Cliente e e-mail da conta mudam na mesma transação.
        await GravarAsync();

        return await CriarClienteDtoAsync(cliente, utilizador);
    }

    private async Task GarantirEmailLivreAsync(string email, int? utilizadorAtualId)
    {
        // O e-mail é único em todas as contas (UQ_Utilizador_Email), incluindo as dos funcionários.
        var emUso = await _db.Utilizadores
            .AnyAsync(u => u.Email == email && u.Id != utilizadorAtualId);

        if (emUso)
        {
            throw new InvalidOperationException(MensagemEmailRegistado);
        }
    }

    private async Task GarantirNifLivreAsync(string? nif, int? clienteAtualId)
    {
        if (nif is null)
        {
            return;
        }

        var emUso = await _db.Clientes
            .AnyAsync(c => c.NIF == nif && c.Id != clienteAtualId);

        if (emUso)
        {
            throw new InvalidOperationException(MensagemNifRegistado);
        }
    }

    /// <summary>
    /// Grava as alterações; um pedido simultâneo com o mesmo e-mail ou NIF, que
    /// passou as verificações anteriores, é travado pelos índices únicos e devolve 409.
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
        } sql)
        {
            var mensagem = sql.Message.Contains("UQ_Cliente_NIF", StringComparison.Ordinal)
                ? MensagemNifRegistado
                : MensagemEmailRegistado;

            throw new InvalidOperationException(mensagem, ex);
        }
    }

    private async Task<ClienteDto> CriarClienteDtoAsync(Cliente cliente, Utilizador utilizador)
    {
        var animais = await (
                from p in _db.Pacientes.AsNoTracking()
                join e in _db.Especies.AsNoTracking() on p.EspecieId equals e.Id
                where p.ClienteId == cliente.Id
                orderby p.Nome, p.Id
                select new AnimalRefDto { Id = p.Id, Nome = p.Nome, Especie = e.Nome })
            .ToListAsync();

        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Nif = cliente.NIF,
            Telefone = cliente.Telefone,
            Email = utilizador.Email,
            Morada = cliente.Morada,
            UtilizadorId = utilizador.Id,
            EstadoConta = utilizador.Estado,
            Animais = animais
        };
    }

    private DateTime Agora()
    {
        // datetime2(0): sem frações de segundo.
        var agora = _relogio.GetUtcNow().UtcDateTime;
        return new DateTime(agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
    }

    private static string NormalizarEmail(string email) => email.Trim().ToLowerInvariant();

    private static string? NormalizarNif(string? nif) => NormalizarOpcional(nif);

    private static string? NormalizarOpcional(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}

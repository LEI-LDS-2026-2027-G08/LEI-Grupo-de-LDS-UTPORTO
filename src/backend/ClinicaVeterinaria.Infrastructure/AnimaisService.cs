using ClinicaVeterinaria.Application.DTOs.Animais;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

public sealed class AnimaisService : IAnimaisService
{
    private const string MensagemAnimalNaoEncontrado =
        "O animal indicado não existe.";

    private const string MensagemClienteNaoEncontrado =
        "O cliente indicado não existe.";

    private readonly ClinicaDbContext _db;
    private readonly TimeProvider _relogio;

    public AnimaisService(
        ClinicaDbContext db,
        TimeProvider relogio)
    {
        _db = db;
        _relogio = relogio;
    }

public async Task<PagedResultDto<AnimalListItemDto>> PesquisarAsync(
    AnimalFiltroQuery filtro)
{
    ArgumentNullException.ThrowIfNull(filtro);

    var consulta =
        from p in _db.Pacientes.AsNoTracking()
        join c in _db.Clientes.AsNoTracking()
            on p.ClienteId equals c.Id
        join e in _db.Especies.AsNoTracking()
            on p.EspecieId equals e.Id
        select new
        {
            Paciente = p,
            Cliente = c,
            Especie = e.Nome,
            Raca = _db.Racas
                .Where(r => r.Id == p.RacaId && r.EspecieId == p.EspecieId)
                .Select(r => r.Nome)
                .FirstOrDefault()
        };

    if (filtro.ClienteId.HasValue)
    {
        consulta = consulta.Where(x =>
            x.Paciente.ClienteId == filtro.ClienteId.Value);
    }

    var pesquisa = filtro.Pesquisa?.Trim();

    if (!string.IsNullOrEmpty(pesquisa))
    {
        consulta = consulta.Where(x =>
            x.Paciente.Nome.Contains(pesquisa)
            || x.Cliente.Nome.Contains(pesquisa));
    }

    var especie = filtro.Especie?.Trim();

    if (!string.IsNullOrEmpty(especie))
    {
        consulta = consulta.Where(x =>
            x.Especie == especie);
    }

    var total = await consulta.CountAsync();

    var dados = await consulta
        .OrderBy(x => x.Paciente.Nome)
        .ThenBy(x => x.Paciente.Id)
        .Skip((filtro.Pagina - 1) * filtro.Tamanho)
        .Take(filtro.Tamanho)
        .Select(x => new
        {
            x.Paciente.Id,
            x.Paciente.Nome,
            x.Especie,
            x.Raca,
            x.Paciente.DataNascimento,
            x.Paciente.Peso,
            ClienteId = x.Cliente.Id,
            ClienteNome = x.Cliente.Nome
        })
        .ToListAsync();

    var itens = dados
        .Select(x => new AnimalListItemDto
        {
            Id = x.Id,
            Nome = x.Nome,
            Especie = x.Especie,
            Raca = x.Raca,
            IdadeAnos = CalcularIdade(x.DataNascimento),
            Peso = x.Peso,
            Cliente = new ClienteResumoDto
            {
                Id = x.ClienteId,
                Nome = x.ClienteNome
            }
        })
        .ToList();

    return new PagedResultDto<AnimalListItemDto>
    {
        Itens = itens,
        Pagina = filtro.Pagina,
        Tamanho = filtro.Tamanho,
        Total = total
    };
}

public async Task<AnimalDto> ObterAsync(int id)
{
    var dados = await (
        from p in _db.Pacientes.AsNoTracking()
        join c in _db.Clientes.AsNoTracking()
            on p.ClienteId equals c.Id
        join e in _db.Especies.AsNoTracking()
            on p.EspecieId equals e.Id
        where p.Id == id
        select new
        {
            Paciente = p,
            Cliente = c,
            Especie = e.Nome,
            Raca = _db.Racas
                .Where(r => r.Id == p.RacaId && r.EspecieId == p.EspecieId)
                .Select(r => r.Nome)
                .FirstOrDefault()
        })
        .FirstOrDefaultAsync()
        ?? throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);

    return new AnimalDto
    {
        Id = dados.Paciente.Id,
        Nome = dados.Paciente.Nome,
        Especie = dados.Especie,
        Raca = dados.Raca,
        Sexo = dados.Paciente.Sexo,
        DataNascimento = dados.Paciente.DataNascimento,
        IdadeAnos = CalcularIdade(dados.Paciente.DataNascimento),
        Peso = dados.Paciente.Peso,
        Cliente = new ClienteDetalheDto
        {
            Id = dados.Cliente.Id,
            Nome = dados.Cliente.Nome,
            Telefone = dados.Cliente.Telefone
        },
        AtualizadoEm = dados.Paciente.DataUltimaAlteracao
    };
}

public async Task<AnimalDto> CriarAsync(CreateAnimalDto pedido)
{
    ArgumentNullException.ThrowIfNull(pedido);

    ValidarDadosAdministrativos(pedido);

    var clienteExiste = await _db.Clientes
        .AnyAsync(c => c.Id == pedido.ClienteId);

    if (!clienteExiste)
    {
        throw new KeyNotFoundException(MensagemClienteNaoEncontrado);
    }

    var especie = await _db.Especies
        .FirstOrDefaultAsync(e => e.Nome == pedido.Especie.Trim())
        ?? throw new ArgumentException("A espécie indicada não existe.");

    int? racaId = null;

    if (!string.IsNullOrWhiteSpace(pedido.Raca))
    {
        var raca = await _db.Racas
            .FirstOrDefaultAsync(r =>
                r.EspecieId == especie.Id
                && r.Nome == pedido.Raca.Trim())
            ?? throw new ArgumentException(
                "A raça indicada não existe para a espécie selecionada.");

        racaId = raca.Id;
    }

    var agora = Agora();

    var paciente = new Paciente
    {
        ClienteId = pedido.ClienteId,
        Nome = pedido.Nome.Trim(),
        EspecieId = especie.Id,
        RacaId = racaId,
        Sexo = pedido.Sexo,
        DataNascimento = pedido.DataNascimento,
        Peso = pedido.Peso,
        DataRegisto = agora
    };

    _db.Pacientes.Add(paciente);
    await _db.SaveChangesAsync();

    return await ObterAsync(paciente.Id);
}

public async Task<AnimalDto> AtualizarAsync(
    int id,
    UpdateAnimalDto pedido)
{
    ArgumentNullException.ThrowIfNull(pedido);

    var paciente = await _db.Pacientes
        .FirstOrDefaultAsync(p => p.Id == id)
        ?? throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);

    ValidarDadosAdministrativos(pedido);

    var clienteExiste = await _db.Clientes
        .AnyAsync(c => c.Id == pedido.ClienteId);

    if (!clienteExiste)
    {
        throw new KeyNotFoundException(MensagemClienteNaoEncontrado);
    }

    var especie = await _db.Especies
        .FirstOrDefaultAsync(e => e.Nome == pedido.Especie.Trim())
        ?? throw new ArgumentException("A espécie indicada não existe.");

    int? racaId = null;

    if (!string.IsNullOrWhiteSpace(pedido.Raca))
    {
        var raca = await _db.Racas
            .FirstOrDefaultAsync(r =>
                r.EspecieId == especie.Id
                && r.Nome == pedido.Raca.Trim())
            ?? throw new ArgumentException(
                "A raça indicada não existe para a espécie selecionada.");

        racaId = raca.Id;
    }

    paciente.ClienteId = pedido.ClienteId;
    paciente.Nome = pedido.Nome.Trim();
    paciente.EspecieId = especie.Id;
    paciente.RacaId = racaId;
    paciente.Sexo = pedido.Sexo;
    paciente.DataNascimento = pedido.DataNascimento;
    paciente.Peso = pedido.Peso;
    paciente.DataUltimaAlteracao = Agora();

    await _db.SaveChangesAsync();

    return await ObterAsync(paciente.Id);
}
    private int? CalcularIdade(DateOnly? dataNascimento)
{
    if (!dataNascimento.HasValue)
    {
        return null;
    }

    var hoje = DateOnly.FromDateTime(_relogio.GetUtcNow().UtcDateTime);
    var nascimento = dataNascimento.Value;

    var idade = hoje.Year - nascimento.Year;

    if (nascimento > hoje.AddYears(-idade))
    {
        idade--;
    }

    return idade;
}
private void ValidarDadosAdministrativos(UpdateAnimalDto pedido)
{
    if (pedido.Peso.HasValue && pedido.Peso.Value <= 0)
    {
        throw new ArgumentException("O peso deve ser superior a 0.");
    }

    if (pedido.DataNascimento.HasValue)
    {
        var hoje = DateOnly.FromDateTime(
            _relogio.GetUtcNow().UtcDateTime);

        if (pedido.DataNascimento.Value > hoje)
        {
            throw new ArgumentException(
                "A data de nascimento não pode ser futura.");
        }
    }
}

private DateTime Agora()
{
    var agora = _relogio.GetUtcNow().UtcDateTime;

    return new DateTime(
        agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond),
        DateTimeKind.Utc);
}
}

using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Vacinacoes;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Vacinacao;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Boletim de vacinas e desparasitações (F006; I082): registo e atualização pelo Veterinário
/// (EP-043, RF056), boletim no web (EP-042) e boletim do Cliente na aplicação móvel (EP-044). A data
/// prevista da próxima administração é sempre calculada pelo servidor (RF045,
/// <see cref="CalculadoraProximaAdministracao"/>), como a coluna calculada
/// <c>RegistoProfilaxia.DataProximaAdministracao</c>.
/// </summary>
/// <remarks>
/// «Hoje» é o dia em hora de Lisboa (R-FUSO): conta para recusar datas de administração futuras e
/// para escolher as próximas administrações (só as que ainda não passaram; PA-11 da I028).
/// </remarks>
public sealed class VacinacoesService : IVacinacoesService
{
    /// <summary>EP-042, EP-043, EP-044 e RF056 — animal inexistente (404).</summary>
    public const string MensagemAnimalNaoEncontrado = "O animal indicado não existe.";

    /// <summary>EP-044 — animal de outro Cliente (403).</summary>
    public const string MensagemAnimalDeOutroCliente = "O animal indicado não pertence ao Cliente autenticado.";

    /// <summary>RF056 — registo inexistente ou de outro animal (404).</summary>
    public const string MensagemAdministracaoNaoEncontrada =
        "A vacinação ou desparasitação indicada não existe neste animal.";

    /// <summary>RF056 — registo feito por outro Veterinário (403).</summary>
    public const string MensagemAdministracaoDeOutroVeterinario =
        "A vacinação ou desparasitação foi registada por outro Veterinário.";

    /// <summary>EP-043 e RF056 — data de administração futura (400).</summary>
    public const string MensagemDataFutura = "A data de administração não pode ser futura.";

    private readonly ClinicaDbContext _db;
    private readonly TimeProvider _relogio;

    public VacinacoesService(ClinicaDbContext db, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(relogio);

        _db = db;
        _relogio = relogio;
    }

    public async Task<BoletimDto> ObterBoletimAsync(int animalId, BoletimQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        // (1) Tipo → 400 (UT-F006-020). A mesma regra da validação automática do pedido, para o
        // serviço também a aplicar quando é chamado diretamente.
        var tipo = string.IsNullOrWhiteSpace(query.Tipo) ? null : query.Tipo.Trim();

        if (tipo is not null && !TiposAdministracao.Todos.Contains(tipo, StringComparer.Ordinal))
        {
            throw new ArgumentException(TiposAdministracao.MensagemTipoInvalido);
        }

        // (2) Animal existe → 404.
        var animal = await (
                from p in _db.Pacientes.AsNoTracking()
                join e in _db.Especies.AsNoTracking() on p.EspecieId equals e.Id
                where p.Id == animalId
                select new AnimalRefDto { Id = p.Id, Nome = p.Nome, Especie = e.Nome })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);

        // (3) Administrações do animal (UT-F006-018) e, com tipo, só as desse tipo (UT-F006-019).
        var administracoes = await ObterAdministracoesAsync(animalId, tipo);

        return new BoletimDto
        {
            Animal = animal,
            Administracoes = administracoes,
            Proximas = Proximas(administracoes)
        };
    }

    public async Task<AdministracaoDto> RegistarAsync(int animalId, CreateAdministracaoDto pedido, int veterinarioId)
    {
        ArgumentNullException.ThrowIfNull(pedido);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(veterinarioId);

        // (1) Campos, data futura e intervalo → 400 (UT-F006-010 a 016).
        Validar(pedido);

        // (2) Animal existe → 404 (UT-F006-017).
        if (!await _db.Pacientes.AsNoTracking().AnyAsync(p => p.Id == animalId))
        {
            throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);
        }

        // (3) Grava o registo; a próxima administração é calculada (coluna calculada no SQL Server e
        // CalculadoraProximaAdministracao na resposta).
        var registo = new RegistoProfilaxia
        {
            PacienteId = animalId,
            VeterinarioId = veterinarioId
        };
        Copiar(pedido, registo);
        registo.DataRegisto = AgoraAoSegundo();

        _db.RegistosProfilaxias.Add(registo);
        await _db.SaveChangesAsync();

        return await ObterDtoAsync(registo.Id);
    }

    public async Task<AdministracaoDto> AtualizarAsync(
        int animalId,
        int administracaoId,
        UpdateAdministracaoDto pedido,
        int veterinarioId)
    {
        ArgumentNullException.ThrowIfNull(pedido);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(veterinarioId);

        // (1) Mesmas regras de EP-043 → 400.
        Validar(pedido);

        // (2) Animal existe → 404.
        if (!await _db.Pacientes.AsNoTracking().AnyAsync(p => p.Id == animalId))
        {
            throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);
        }

        // (3) O registo existe e é deste animal → 404 (o registo não muda de animal).
        var registo = await _db.RegistosProfilaxias
                .FirstOrDefaultAsync(r => r.Id == administracaoId && r.PacienteId == animalId)
            ?? throw new KeyNotFoundException(MensagemAdministracaoNaoEncontrada);

        // (4) Só o Veterinário que fez o registo o altera → 403 (como em EP-032, I077). A tabela
        // não guarda o autor das alterações, por isso o Veterinário do registo nunca muda.
        if (registo.VeterinarioId != veterinarioId)
        {
            throw new ForbiddenException(MensagemAdministracaoDeOutroVeterinario);
        }

        // (5) Substituição completa; a próxima administração é recalculada (critério 4).
        Copiar(pedido, registo);
        await _db.SaveChangesAsync();

        return await ObterDtoAsync(registo.Id);
    }

    public async Task<MeuBoletimDto> ObterBoletimDoClienteAsync(int clienteId, int animalId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(clienteId);

        // (1) Animal existe → 404; (2) é do Cliente do token → 403, sem devolver nada do animal
        // (RNF002; UT-F006-023; IT-TRV-15). Mesma ordem de EP-020.
        var donoId = await _db.Pacientes.AsNoTracking()
                .Where(p => p.Id == animalId)
                .Select(p => (int?)p.ClienteId)
                .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(MensagemAnimalNaoEncontrado);

        if (donoId != clienteId)
        {
            throw new ForbiddenException(MensagemAnimalDeOutroCliente);
        }

        // (3) O mesmo histórico e as mesmas próximas do boletim web (UT-F006-022), com menos campos.
        var administracoes = await ObterAdministracoesAsync(animalId, tipo: null);

        return new MeuBoletimDto
        {
            Historico = administracoes
                .Select(a => new HistoricoAdministracaoDto
                {
                    Tipo = a.Tipo,
                    Produto = a.Produto,
                    DataAdministracao = a.DataAdministracao
                })
                .ToList(),
            Proximas = Proximas(administracoes)
        };
    }

    /// <summary>
    /// Administrações do animal com o nome do Veterinário, da mais recente para a mais antiga (e,
    /// no mesmo dia, da última registada para a primeira).
    /// </summary>
    private async Task<List<AdministracaoDto>> ObterAdministracoesAsync(int animalId, string? tipo)
    {
        var registos = _db.RegistosProfilaxias.AsNoTracking().Where(r => r.PacienteId == animalId);

        if (tipo is not null)
        {
            registos = registos.Where(r => r.Tipo == tipo);
        }

        var linhas = await (
                from r in registos
                join v in _db.Funcionarios.AsNoTracking() on r.VeterinarioId equals v.UtilizadorId
                orderby r.DataAdministracao descending, r.Id descending
                select new { Registo = r, Veterinario = v.Nome })
            .ToListAsync();

        return linhas.Select(l => ParaDto(l.Registo, l.Veterinario)).ToList();
    }

    /// <summary>
    /// Próximas administrações que ainda não passaram (data prevista igual ou posterior a hoje), da
    /// mais próxima para a mais distante (UT-F006-018 e 021; PA-11 da I028).
    /// </summary>
    private List<ProximaAdministracaoDto> Proximas(List<AdministracaoDto> administracoes)
    {
        var hoje = Hoje();

        return administracoes
            .Where(a => a.DataProximaPrevista is DateOnly data && data >= hoje)
            .OrderBy(a => a.DataProximaPrevista)
            .ThenBy(a => a.Id)
            .Select(a => new ProximaAdministracaoDto
            {
                Tipo = a.Tipo,
                Produto = a.Produto,
                DataProximaPrevista = a.DataProximaPrevista!.Value
            })
            .ToList();
    }

    private async Task<AdministracaoDto> ObterDtoAsync(int id)
    {
        var linha = await (
                from r in _db.RegistosProfilaxias.AsNoTracking()
                join v in _db.Funcionarios.AsNoTracking() on r.VeterinarioId equals v.UtilizadorId
                where r.Id == id
                select new { Registo = r, Veterinario = v.Nome })
            .FirstAsync();

        return ParaDto(linha.Registo, linha.Veterinario);
    }

    private static AdministracaoDto ParaDto(RegistoProfilaxia registo, string veterinario) => new()
    {
        Id = registo.Id,
        Tipo = registo.Tipo,
        Produto = registo.Produto,
        Lote = registo.Lote,
        DataAdministracao = registo.DataAdministracao,
        IntervaloDias = registo.IntervaloDias,
        DataProximaPrevista = CalculadoraProximaAdministracao.Calcular(registo.DataAdministracao, registo.IntervaloDias),
        Veterinario = veterinario
    };

    // A coluna calculada DataProximaAdministracao não é escrita: o SQL Server calcula-a.
    private static void Copiar(UpdateAdministracaoDto pedido, RegistoProfilaxia registo)
    {
        registo.Tipo = pedido.Tipo.Trim();
        registo.Produto = pedido.Produto.Trim();
        registo.Lote = pedido.Lote.Trim();
        registo.DataAdministracao = pedido.DataAdministracao!.Value;
        registo.IntervaloDias = (short?)pedido.IntervaloDias;
    }

    /// <summary>
    /// Regras das anotações do pedido (tipo, produto, lote, data e intervalo) e data não futura.
    /// </summary>
    private void Validar(UpdateAdministracaoDto pedido)
    {
        var erros = new List<ValidationResult>();
        Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true);

        var mensagens = erros
            .Select(e => e.ErrorMessage ?? string.Empty)
            .Where(m => m.Length > 0)
            .ToList();

        // Hoje é aceite (UT-F006-012); amanhã não (UT-F006-011).
        if (pedido.DataAdministracao is DateOnly data && data > Hoje())
        {
            mensagens.Add(MensagemDataFutura);
        }

        if (mensagens.Count > 0)
        {
            throw new ArgumentException(string.Join(" ", mensagens));
        }
    }

    private DateOnly Hoje() =>
        DateOnly.FromDateTime(FusoHorarioClinica.ParaLocal(_relogio.GetUtcNow().UtcDateTime, FusoHorarioClinica.Obter()));

    // Sem segundos fracionários, como na coluna datetime2(0).
    private DateTime AgoraAoSegundo()
    {
        var agora = _relogio.GetUtcNow().UtcDateTime;

        return new DateTime(agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
    }
}

using System.Globalization;
using ClinicaVeterinaria.Application.DTOs.Clinica;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Validacao;
using ClinicaVeterinaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Dados gerais, configurações e horário de funcionamento da clínica (EP-009, EP-010).
/// O horário gravado aqui é a fonte única usada no cálculo das disponibilidades (I071).
/// </summary>
public sealed class ClinicaService : IClinicaService
{
    private const byte IdClinica = 1;
    private const string ChaveDuracaoConsulta = "DuracaoPadraoConsultaMinutos";
    private const string DescricaoDuracaoConsulta = "Duração por omissão de uma consulta, em minutos (EP-009 e EP-010).";
    private const int DuracaoConsultaPorOmissao = 30;

    private readonly ClinicaDbContext _db;
    private readonly TimeProvider _relogio;

    public ClinicaService(ClinicaDbContext db, TimeProvider relogio)
    {
        _db = db;
        _relogio = relogio;
    }

    public async Task<ClinicaDto> ObterAsync()
    {
        var clinica = await _db.Clinica.AsNoTracking().FirstOrDefaultAsync(c => c.Id == IdClinica)
            ?? new Clinica { Id = IdClinica };

        var duracao = await _db.ConfiguracaoSistema.AsNoTracking()
            .Where(c => c.Chave == ChaveDuracaoConsulta)
            .Select(c => c.Valor)
            .FirstOrDefaultAsync();

        var horario = await _db.HorariosFuncionamento.AsNoTracking()
            .Where(h => h.Ativo)
            .OrderBy(h => h.DiaSemana)
            .ThenBy(h => h.HoraAbertura)
            .Select(h => new PeriodoValidado(h.DiaSemana, h.HoraAbertura, h.HoraFecho))
            .ToListAsync();

        return CriarDto(clinica, LerDuracao(duracao), horario);
    }

    public async Task<ClinicaDto> AtualizarAsync(UpdateClinicaDto pedido)
    {
        ArgumentNullException.ThrowIfNull(pedido);

        // Valida tudo antes de alterar qualquer entidade: com um período inválido
        // não se grava nada e o horário anterior mantém-se (critério 4).
        var horario = ValidadorHorario.Validar(pedido.HorarioFuncionamento ?? []);
        var agora = Agora();

        var clinica = await _db.Clinica.FirstOrDefaultAsync(c => c.Id == IdClinica);
        if (clinica is null)
        {
            clinica = new Clinica { Id = IdClinica };
            _db.Clinica.Add(clinica);
        }

        clinica.Nome = pedido.Nome.Trim();
        clinica.NIF = pedido.Nif.Trim();
        clinica.Morada = pedido.Morada.Trim();
        clinica.Telefone = pedido.Telefone.Trim();
        clinica.Email = pedido.Email.Trim();
        clinica.DataUltimaAlteracao = agora;

        var duracao = await _db.ConfiguracaoSistema.FirstOrDefaultAsync(c => c.Chave == ChaveDuracaoConsulta);
        if (duracao is null)
        {
            duracao = new ConfiguracaoSistema { Chave = ChaveDuracaoConsulta, Descricao = DescricaoDuracaoConsulta };
            _db.ConfiguracaoSistema.Add(duracao);
        }

        duracao.Valor = pedido.DuracaoConsultaMinutos.ToString(CultureInfo.InvariantCulture);
        duracao.DataUltimaAlteracao = agora;

        // O horário enviado substitui o anterior por completo.
        _db.HorariosFuncionamento.RemoveRange(await _db.HorariosFuncionamento.ToListAsync());
        _db.HorariosFuncionamento.AddRange(horario.Select(p => new HorarioFuncionamento
        {
            DiaSemana = p.DiaSemana,
            HoraAbertura = p.Abertura,
            HoraFecho = p.Fecho,
            Ativo = true
        }));

        // Um único SaveChanges: dados, configuração e horário na mesma transação.
        await _db.SaveChangesAsync();

        return CriarDto(clinica, pedido.DuracaoConsultaMinutos, horario);
    }

    private static ClinicaDto CriarDto(Clinica clinica, int duracaoConsultaMinutos, IReadOnlyList<PeriodoValidado> horario) =>
        new()
        {
            Nome = clinica.Nome,
            Nif = clinica.NIF,
            Morada = clinica.Morada,
            Telefone = clinica.Telefone,
            Email = clinica.Email,
            DuracaoConsultaMinutos = duracaoConsultaMinutos,
            HorarioFuncionamento = horario
                .Select(p => new PeriodoHorarioDto
                {
                    DiaSemana = ValidadorHorario.NomeDia(p.DiaSemana),
                    Abertura = ValidadorHorario.FormatarHora(p.Abertura),
                    Fecho = ValidadorHorario.FormatarHora(p.Fecho)
                })
                .ToList()
        };

    private static int LerDuracao(string? valor) =>
        int.TryParse(valor, NumberStyles.Integer, CultureInfo.InvariantCulture, out var minutos)
            ? minutos
            : DuracaoConsultaPorOmissao;

    private DateTime Agora()
    {
        // datetime2(0): sem frações de segundo.
        var agora = _relogio.GetUtcNow().UtcDateTime;
        return new DateTime(agora.Ticks - (agora.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
    }
}

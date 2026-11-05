using System.Globalization;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Veterinarios;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Application.Validacao;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Veterinários e disponibilidade (EP-021, EP-022). Um Veterinário é um <c>Utilizador</c> com o
/// perfil Veterinario, a conta ativa e o registo em <c>Funcionario</c> com o mesmo identificador.
/// O cálculo dos horários livres está em <see cref="CalculadoraDisponibilidade"/>; este serviço só
/// lê os dados (horário da clínica, duração das consultas e marcações) e prepara a resposta.
/// </summary>
public sealed class VeterinariosService : IVeterinariosService
{
    private const string MensagemNaoEncontrado = "O veterinário indicado não existe ou não está ativo.";
    private const string FormatoHora = "HH:mm";

    private readonly ClinicaDbContext _db;
    private readonly TimeProvider _relogio;

    public VeterinariosService(ClinicaDbContext db, TimeProvider relogio)
    {
        _db = db;
        _relogio = relogio;
    }

    public async Task<IReadOnlyList<VeterinarioRefDto>> ListarAsync()
    {
        return await (
                from u in _db.Utilizadores.AsNoTracking()
                join f in _db.Funcionarios.AsNoTracking() on u.Id equals f.UtilizadorId
                where u.Perfil == Perfis.Veterinario && u.Estado == EstadosUtilizador.Ativo
                orderby f.Nome, u.Id
                select new VeterinarioRefDto { Id = u.Id, Nome = f.Nome })
            .ToListAsync();
    }

    public async Task<DisponibilidadeDto> ObterDisponibilidadeAsync(int veterinarioId, DisponibilidadeQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        // Mesmas regras da validação automática do pedido (400), para o serviço também as aplicar
        // quando é chamado diretamente. O período é validado antes de procurar o Veterinário.
        var erro = ValidadorPeriodoDisponibilidade.ObterErro(query.DataInicio, query.DataFim);
        if (erro is not null)
        {
            throw new ArgumentException(erro);
        }

        var dataInicio = query.DataInicio!.Value;
        var dataFim = ValidadorPeriodoDisponibilidade.DataFimEfetiva(dataInicio, query.DataFim);

        var existe = await (
                from u in _db.Utilizadores.AsNoTracking()
                join f in _db.Funcionarios.AsNoTracking() on u.Id equals f.UtilizadorId
                where u.Id == veterinarioId
                      && u.Perfil == Perfis.Veterinario
                      && u.Estado == EstadosUtilizador.Ativo
                select u.Id)
            .AnyAsync();

        if (!existe)
        {
            throw new KeyNotFoundException(MensagemNaoEncontrado);
        }

        var duracaoMinutos = DuracaoConsulta.Ler(await _db.ConfiguracaoSistema.AsNoTracking()
            .Where(c => c.Chave == DuracaoConsulta.Chave)
            .Select(c => c.Valor)
            .FirstOrDefaultAsync());

        var horario = await _db.HorariosFuncionamento.AsNoTracking()
            .Where(h => h.Ativo)
            .Select(h => new PeriodoValidado(h.DiaSemana, h.HoraAbertura, h.HoraFecho))
            .ToListAsync();

        var fuso = FusoHorarioClinica.Obter();

        // Marcações que tocam o período em hora local: da meia-noite do primeiro dia à
        // meia-noite seguinte ao último. Uma marcação que começou antes e ainda decorre conta.
        var inicioJanela = InicioDoDiaUtc(dataInicio, fuso);
        var fimJanela = InicioDoDiaUtc(dataFim.AddDays(1), fuso);

        var marcacoes = await _db.Marcacoes.AsNoTracking()
            .Where(m => m.VeterinarioId == veterinarioId
                        && m.Estado != CalculadoraDisponibilidade.EstadoCancelada
                        && m.DataHoraInicio < fimJanela
                        && m.DataHoraInicio.AddMinutes(m.DuracaoMinutos) > inicioJanela)
            .Select(m => new MarcacaoAgendada(m.DataHoraInicio, m.DuracaoMinutos, m.Estado))
            .ToListAsync();

        var dias = CalculadoraDisponibilidade.Calcular(
            dataInicio,
            dataFim,
            horario,
            duracaoMinutos,
            marcacoes,
            _relogio.GetUtcNow().UtcDateTime,
            fuso);

        return new DisponibilidadeDto
        {
            VeterinarioId = veterinarioId,
            DuracaoConsultaMinutos = duracaoMinutos,
            Dias = dias
                .Select(dia => new DiaDisponivelDto
                {
                    Data = dia.Data,
                    HorariosLivres = dia.HorariosLivres
                        .Select(hora => hora.ToString(FormatoHora, CultureInfo.InvariantCulture))
                        .ToList()
                })
                .ToList()
        };
    }

    // A meia-noite existe sempre em Lisboa (as mudanças de hora são à 01:00 UTC).
    private static DateTime InicioDoDiaUtc(DateOnly data, TimeZoneInfo fuso) =>
        FusoHorarioClinica.ParaUtc(data, TimeOnly.MinValue, fuso)
        ?? DateTime.SpecifyKind(data.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
}

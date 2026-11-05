namespace ClinicaVeterinaria.Application.DTOs.Clinica;

public class ClinicaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Nif { get; set; } = string.Empty;
    public string Morada { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int DuracaoConsultaMinutos { get; set; }

    /// <summary>Períodos ordenados por dia e hora de abertura; um dia sem períodos está fechado.</summary>
    public IReadOnlyList<PeriodoHorarioDto> HorarioFuncionamento { get; set; } = [];
}

namespace ClinicaVeterinaria.Application.Models.Clinica;

public class DadosClinica
{
    public string Nome { get; set; } = string.Empty;

    public string Nif { get; set; } = string.Empty;

    public string Morada { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int DuracaoConsultaMinutos { get; set; }

    public IReadOnlyList<PeriodoHorario> HorarioFuncionamento { get; set; } = [];
}

namespace ClinicaVeterinaria.Application.Models.Veterinarios;

public class DisponibilidadeVeterinario
{
    public int VeterinarioId { get; set; }

    public int DuracaoConsultaMinutos { get; set; }

    public IReadOnlyList<DiaDisponivel> Dias { get; set; } = [];
}

namespace ClinicaVeterinaria.Domain.Entities;

public class HorarioFuncionamento
{
    public int Id { get; set; }
    public byte DiaSemana { get; set; }
    public TimeOnly HoraAbertura { get; set; }
    public TimeOnly HoraFecho { get; set; }
    public bool Ativo { get; set; }
}
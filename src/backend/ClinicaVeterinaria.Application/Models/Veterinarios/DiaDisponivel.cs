namespace ClinicaVeterinaria.Application.Models.Veterinarios;

public sealed record DiaDisponivel(DateOnly Data, IReadOnlyList<TimeOnly> HorariosLivres);

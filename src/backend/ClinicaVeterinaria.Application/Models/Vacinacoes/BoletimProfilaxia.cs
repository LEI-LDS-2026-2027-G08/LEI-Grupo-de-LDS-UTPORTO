using ClinicaVeterinaria.Application.Models.Comum;

namespace ClinicaVeterinaria.Application.Models.Vacinacoes;

public class BoletimProfilaxia
{
    public ReferenciaAnimal Animal { get; set; } = new();

    public IReadOnlyList<AdministracaoRegistada> Administracoes { get; set; } = [];

    public IReadOnlyList<AdministracaoPrevista> Proximas { get; set; } = [];
}

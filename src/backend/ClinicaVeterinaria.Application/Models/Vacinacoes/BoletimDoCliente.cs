namespace ClinicaVeterinaria.Application.Models.Vacinacoes;

public class BoletimDoCliente
{
    public IReadOnlyList<AdministracaoAnterior> Historico { get; set; } = [];

    public IReadOnlyList<AdministracaoPrevista> Proximas { get; set; } = [];
}

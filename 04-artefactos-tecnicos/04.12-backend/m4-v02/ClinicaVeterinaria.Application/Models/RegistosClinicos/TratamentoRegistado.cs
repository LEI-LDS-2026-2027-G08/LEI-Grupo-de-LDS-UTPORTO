namespace ClinicaVeterinaria.Application.Models.RegistosClinicos;

public class TratamentoRegistado
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateOnly DataAplicacao { get; set; }
}

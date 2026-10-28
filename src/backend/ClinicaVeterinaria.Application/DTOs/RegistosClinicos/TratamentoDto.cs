namespace ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

/// <summary>Tratamento de um registo clínico (resposta de EP-031 e EP-032).</summary>
public class TratamentoDto
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateOnly DataAplicacao { get; set; }
}

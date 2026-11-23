namespace ClinicaVeterinaria.Api.DTOs.Consultas;

/// <summary>Resposta de EP-028 (PATCH /consultas/{id}/chegada).</summary>
public class ChegadaDto
{
    public int Id { get; set; }

    /// <summary>Hora do servidor, em UTC; nunca vem do pedido.</summary>
    public DateTime ChegadaEm { get; set; }
}

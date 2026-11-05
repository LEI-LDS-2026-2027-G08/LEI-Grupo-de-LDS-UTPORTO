namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>Resposta de EP-028 (PATCH /consultas/{id}/chegada).</summary>
public class ChegadaDto
{
    public int Id { get; set; }

    /// <summary>Hora do servidor, em UTC; nunca vem do pedido.</summary>
    public DateTime ChegadaEm { get; set; }
}

/// <summary>Resposta de EP-028 com a nova versão da marcação (<c>ETag</c>).</summary>
public sealed record ChegadaComVersao(ChegadaDto Chegada, string ETag);

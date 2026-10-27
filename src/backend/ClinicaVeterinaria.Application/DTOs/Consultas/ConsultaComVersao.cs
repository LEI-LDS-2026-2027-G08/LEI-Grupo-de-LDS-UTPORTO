namespace ClinicaVeterinaria.Application.DTOs.Consultas;

/// <summary>
/// Resultado de EP-024 no serviço: a marcação e a sua versão como <c>ETag</c>. Não é serializado;
/// o controlador devolve <see cref="Consulta"/> no corpo e <see cref="ETag"/> no cabeçalho
/// (Documentação de API v02, secção 5.1).
/// </summary>
public sealed record ConsultaComVersao(ConsultaDto Consulta, string ETag);

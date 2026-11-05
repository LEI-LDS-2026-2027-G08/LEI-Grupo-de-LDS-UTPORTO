namespace ClinicaVeterinaria.Application.DTOs.Vacinacoes;

/// <summary>
/// Corpo de EP-043 (POST /animais/{id}/vacinacoes): tipo, produto, lote, data de administração e,
/// quando aplicável, o intervalo até à próxima administração. O animal vem da rota.
/// </summary>
public class CreateAdministracaoDto : UpdateAdministracaoDto
{
}

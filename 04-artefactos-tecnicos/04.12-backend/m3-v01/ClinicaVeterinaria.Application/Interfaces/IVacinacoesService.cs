using ClinicaVeterinaria.Application.DTOs.Vacinacoes;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>
/// Boletim de vacinas e desparasitações (F006; I082). Registo, atualização e boletim no web
/// (EP-042, EP-043 e RF056): só o Veterinário. Boletim na aplicação móvel (EP-044): só o Cliente,
/// e só dos seus próprios animais.
/// </summary>
public interface IVacinacoesService
{
    /// <summary>
    /// EP-042 — boletim completo do animal: as administrações (da mais recente para a mais antiga)
    /// e as próximas previstas a partir de hoje (da mais próxima para a mais distante), opcionalmente
    /// só de um tipo. Tipo inválido: 400; animal inexistente: 404.
    /// </summary>
    Task<BoletimDto> ObterBoletimAsync(int animalId, BoletimQuery query);

    /// <summary>
    /// EP-043 — regista uma vacinação ou desparasitação do animal, feita pelo Veterinário
    /// autenticado. Com intervalo, calcula <c>dataProximaPrevista</c>; sem intervalo, fica nula.
    /// Dados em falta ou inválidos, data futura ou intervalo não superior a 0: 400; animal
    /// inexistente: 404.
    /// </summary>
    /// <param name="animalId">Animal da rota.</param>
    /// <param name="pedido">Tipo, produto, lote, data de administração e intervalo opcional.</param>
    /// <param name="veterinarioId">Veterinário autenticado (claim <c>veterinarioId</c>).</param>
    Task<AdministracaoDto> RegistarAsync(int animalId, CreateAdministracaoDto pedido, int veterinarioId);

    /// <summary>
    /// RF056 (I082, critério 4) — atualiza uma vacinação ou desparasitação do animal e recalcula
    /// <c>dataProximaPrevista</c>. Mesmas regras de EP-043 (400); animal inexistente, ou registo
    /// inexistente ou de outro animal: 404; registo feito por outro Veterinário: 403.
    /// </summary>
    Task<AdministracaoDto> AtualizarAsync(int animalId, int administracaoId, UpdateAdministracaoDto pedido, int veterinarioId);

    /// <summary>
    /// EP-044 — boletim de um animal do Cliente autenticado: o histórico e as próximas previstas a
    /// partir de hoje. Animal inexistente: 404; animal de outro Cliente: 403, sem nenhum dado dele.
    /// </summary>
    /// <param name="clienteId">Cliente autenticado (claim <c>clienteId</c>; nunca do pedido).</param>
    /// <param name="animalId">Animal da rota.</param>
    Task<MeuBoletimDto> ObterBoletimDoClienteAsync(int clienteId, int animalId);
}

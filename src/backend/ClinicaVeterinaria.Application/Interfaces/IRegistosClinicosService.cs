using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>
/// Registo clínico das consultas. Criação e alteração (EP-031, EP-032): só o Veterinário da
/// consulta. Leitura do registo e do histórico do paciente (EP-033, EP-034): Veterinário e
/// Auxiliar Veterinário, só de leitura.
/// </summary>
public interface IRegistosClinicosService
{
    /// <summary>
    /// EP-031 — cria o registo clínico da consulta, com os tratamentos e as prescrições, numa só
    /// gravação. Pedido inválido ou sem elemento clínico: 400; consulta inexistente: 404; consulta
    /// de outro Veterinário: 403; consulta Pendente ou Cancelada: 422; consulta que já tem
    /// registo: 409. O estado da marcação não muda.
    /// </summary>
    /// <param name="pedido">Consulta, notas, diagnóstico, tratamentos e prescrições.</param>
    /// <param name="veterinarioId">Veterinário autenticado (claim <c>veterinarioId</c>).</param>
    Task<RegistoClinicoDto> CriarAsync(CreateRegistoClinicoDto pedido, int veterinarioId);

    /// <summary>
    /// EP-032 — atualiza o registo: as listas substituem as anteriores (um elemento existente
    /// mantém-se indicando o seu <c>id</c>) e ficam registados <c>AtualizadoEm</c> e o autor. A
    /// consulta e o paciente não mudam. Pedido inválido ou <c>id</c> de outro registo: 400;
    /// registo inexistente: 404; registo de outro Veterinário: 403.
    /// </summary>
    Task<RegistoClinicoDto> AtualizarAsync(int id, UpdateRegistoClinicoDto pedido, int veterinarioId);

    /// <summary>
    /// EP-033 — devolve o registo clínico completo (notas, diagnóstico, tratamentos e prescrições),
    /// no formato da resposta de EP-031. Registo inexistente: 404.
    /// </summary>
    Task<RegistoClinicoDto> ObterAsync(int id);

    /// <summary>
    /// EP-034 — devolve o animal e os resumos dos seus registos clínicos, do mais recente para o
    /// mais antigo (pela data e hora da consulta), opcionalmente entre <c>dataInicio</c> e
    /// <c>dataFim</c> (dias em hora de Lisboa, os dois incluídos). Intervalo inválido: 400; animal
    /// inexistente: 404. Um animal sem registos tem a lista vazia.
    /// </summary>
    Task<HistoricoClinicoDto> ObterHistoricoAsync(int animalId, HistoricoClinicoQuery query);
}

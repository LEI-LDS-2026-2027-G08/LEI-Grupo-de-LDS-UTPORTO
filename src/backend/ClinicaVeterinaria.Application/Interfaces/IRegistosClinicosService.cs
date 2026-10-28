using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;

namespace ClinicaVeterinaria.Application.Interfaces;

/// <summary>Registo clínico das consultas (EP-031, EP-032). Só o Veterinário da consulta.</summary>
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
}

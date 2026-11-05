using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Seguranca;

/// <summary>
/// Base dos controladores das rotas <c>/me/*</c> (aplicação móvel do Cliente).
/// Aplica a política de propriedade e expõe o identificador do Cliente lido do
/// token, que deve ser o único usado para filtrar os dados (RNF002).
/// </summary>
/// <example>
/// <code>
/// [Route("me/animais")]
/// public sealed class MeusAnimaisController : ClienteAutenticadoControllerBase
/// {
///     [HttpGet]
///     public Task&lt;...&gt; Listar() => _servico.ListarDoClienteAsync(ClienteIdAutenticado);
/// }
/// </code>
/// </example>
[ApiController]
[Authorize(Policy = PoliticasAutorizacao.ClienteProprietario)]
public abstract class ClienteAutenticadoControllerBase : ControllerBase
{
    protected int ClienteIdAutenticado =>
        User.ObterClienteId()
        ?? throw new ForbiddenException("O utilizador autenticado não está associado a um Cliente.");
}

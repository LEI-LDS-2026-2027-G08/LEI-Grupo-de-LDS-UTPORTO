using ClinicaVeterinaria.Api.DTOs.Clientes;
using ClinicaVeterinaria.Application.Clientes;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Api.Controllers;

[ApiController]
[Route("clientes")]
[Produces("application/json")]
public sealed class ClientesController : ControllerBase
{
    private const string PerfisConsulta = Perfis.Administrador + "," + Perfis.Rececionista + "," + Perfis.Veterinario;
    private const string PerfisGestao = Perfis.Administrador + "," + Perfis.Rececionista;

    private readonly IClientesService _clientesService;

    public ClientesController(IClientesService clientesService)
    {
        _clientesService = clientesService;
    }

    /// <summary>EP-011 — Pesquisar clientes (Administrador, Rececionista, Veterinário).</summary>
    /// <response code="200">Página de clientes que correspondem à pesquisa, com o número de animais de cada um.</response>
    /// <response code="400">Parâmetros de paginação inválidos.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    [HttpGet]
    [Authorize(Roles = PerfisConsulta)]
    [ProducesResponseType<PagedResultDto<ClienteListItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResultDto<ClienteListItemDto>>> Pesquisar([FromQuery] ClienteFiltroQuery filtro)
    {
        var pagina = await _clientesService.PesquisarAsync(new PesquisaClientes(filtro.Pesquisa, filtro.Pagina, filtro.Tamanho));

        return Ok(new PagedResultDto<ClienteListItemDto>
        {
            Itens = pagina.Itens.Select(item => new ClienteListItemDto
            {
                Id = item.Id,
                Nome = item.Nome,
                Nif = item.Nif,
                Telefone = item.Telefone,
                Email = item.Email,
                NumeroAnimais = item.NumeroAnimais,
            }).ToArray(),
            Pagina = pagina.Pagina,
            Tamanho = pagina.Tamanho,
            Total = pagina.Total,
        });
    }

    /// <summary>EP-012 — Consultar cliente (Administrador, Rececionista, Veterinário).</summary>
    /// <response code="200">Ficha do cliente com os animais associados.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Cliente inexistente.</response>
    [HttpGet("{id:int}")]
    [Authorize(Roles = PerfisConsulta)]
    [ProducesResponseType<ClienteDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> Obter(int id)
    {
        return Ok(ParaDto(await _clientesService.ObterAsync(id)));
    }

    /// <summary>EP-013 — Criar conta de Cliente (Administrador, Rececionista).</summary>
    /// <response code="201">Cliente e conta de acesso criados; Location indica a ficha.</response>
    /// <response code="400">Campos em falta, NIF ou e-mail inválido, ou palavra-passe inicial fraca.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="409">NIF ou e-mail já registado.</response>
    [HttpPost]
    [Authorize(Roles = PerfisGestao)]
    [ProducesResponseType<ClienteDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClienteDto>> Criar(CreateClienteDto pedido)
    {
        var comando = new NovoCliente(ParaAlteracao(pedido), pedido.PasswordInicial);
        var cliente = ParaDto(await _clientesService.CriarAsync(comando));
        return CreatedAtAction(nameof(Obter), new { id = cliente.Id }, cliente);
    }

    /// <summary>EP-014 — Atualizar conta de Cliente (Administrador, Rececionista).</summary>
    /// <response code="200">Cliente atualizado.</response>
    /// <response code="400">Campos em falta ou inválidos.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="403">Perfil sem permissão.</response>
    /// <response code="404">Cliente inexistente.</response>
    /// <response code="409">NIF ou e-mail já usado por outro cliente ou conta.</response>
    [HttpPut("{id:int}")]
    [Authorize(Roles = PerfisGestao)]
    [ProducesResponseType<ClienteDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClienteDto>> Atualizar(int id, UpdateClienteDto pedido)
    {
        return Ok(ParaDto(await _clientesService.AtualizarAsync(id, ParaAlteracao(pedido))));
    }

    private static AlteracaoCliente ParaAlteracao(UpdateClienteDto pedido) =>
        new(pedido.Nome, pedido.Nif, pedido.Telefone, pedido.Email, pedido.Morada);

    private static ClienteDto ParaDto(FichaCliente ficha) => new()
    {
        Id = ficha.Cliente.Id,
        Nome = ficha.Cliente.Nome,
        Nif = ficha.Cliente.NIF,
        Telefone = ficha.Cliente.Telefone,
        Email = ficha.Email,
        Morada = ficha.Cliente.Morada,
        UtilizadorId = ficha.Cliente.UtilizadorId,
        EstadoConta = ficha.EstadoConta,
        Animais = ficha.Animais.Select(animal => new AnimalRefDto
        {
            Id = animal.Id,
            Nome = animal.Nome,
            Especie = animal.Especie,
        }).ToArray(),
    };
}

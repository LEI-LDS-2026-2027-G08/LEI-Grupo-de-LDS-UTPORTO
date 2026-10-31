using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Clientes;

/// <summary>
/// I092 — GET /clientes (EP-011), GET /clientes/{id} (EP-012), POST /clientes (EP-013) e
/// PUT /clientes/{id} (EP-014), casos IT-EP011-01 a 05, IT-EP012-01 e 02, IT-EP013-01 a 06 e
/// IT-EP014-01 a 07 da Especificação de Testes de Integração (I030), mais os 401 e 403
/// previstos no contrato da API v02.
/// Clientes da I061: João Costa (NIF 210000015, animais Bobi e Mia), Maria Silva
/// (NIF 210000023, animal Kiko) e Rui Santos (sem NIF, animal Luna).
/// </summary>
public sealed class ClientesEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
  private const string EmailJoao = "cliente.joao@clinica-g08.test";
  private const string EmailMaria = "cliente.maria@clinica-g08.test";
  private const string EmailRui = "cliente.rui@clinica-g08.test";
  private const string EmailVetAna = "vet.ana@clinica-g08.test";
  private const string NifMaria = "210000023";

  public ClientesEndpointsTests(IntegracaoFixture fixture)
      : base(fixture)
  {
  }

  // ----- EP-011 -----

  [Fact(DisplayName = "IT-EP011-01 — existem Clientes: 200 com envelope paginado")]
  public async Task ListarDevolveEnvelopePaginado()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.GetAsync("/clientes");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;
    var itens = raiz.GetProperty("itens").EnumerateArray().ToList();

    Assert.Equal(1, raiz.GetProperty("pagina").GetInt32());
    Assert.Equal(20, raiz.GetProperty("tamanho").GetInt32());
    Assert.Equal(await ContarClientesAsync(), raiz.GetProperty("total").GetInt32());
    Assert.Equal(raiz.GetProperty("total").GetInt32(), itens.Count);

    var joao = itens.Single(i => i.GetProperty("email").GetString() == EmailJoao);
    Assert.Equal("210000015", joao.GetProperty("nif").GetString());
    Assert.Equal(2, joao.GetProperty("numeroAnimais").GetInt32());
  }

  [Fact(DisplayName = "IT-EP011-02 — pesquisa por parte do nome: 200 só com os Clientes correspondentes")]
  public async Task PesquisaPorNome()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    Assert.Equal(EmailMaria, string.Join('|', await PesquisarEmailsAsync(cliente, "Silva")));
    Assert.Equal(EmailJoao, string.Join('|', await PesquisarEmailsAsync(cliente, "costa")));
  }

  [Theory(DisplayName = "IT-EP011-03 — pesquisa por NIF, telefone ou e-mail: 200 com o Cliente correto")]
  [InlineData(NifMaria, EmailMaria)]
  [InlineData("960000003", EmailRui)]
  [InlineData("cliente.joao", EmailJoao)]
  public async Task PesquisaPorNifTelefoneOuEmail(string termo, string emailEsperado)
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Veterinario");

    Assert.Equal(emailEsperado, string.Join('|', await PesquisarEmailsAsync(cliente, termo)));
  }

  [Fact(DisplayName = "IT-EP011-04 — termo inexistente: 200 com a lista vazia e total 0")]
  public async Task PesquisaSemResultados()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.GetAsync("/clientes?pesquisa=termo-que-nao-existe");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

    Assert.Equal(0, documento.RootElement.GetProperty("itens").GetArrayLength());
    Assert.Equal(0, documento.RootElement.GetProperty("total").GetInt32());
  }

  [Fact(DisplayName = "IT-EP011-05 — várias páginas: a paginação é consistente (sem repetições nem faltas, total estável)")]
  public async Task PaginacaoConsistente()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    var total = await ContarClientesAsync();
    var vistos = new List<int>();

    for (var pagina = 1; pagina <= total; pagina++)
    {
      using var resposta = await cliente.GetAsync($"/clientes?pagina={pagina}&tamanho=1");

      Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

      using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
      var raiz = documento.RootElement;

      Assert.Equal(pagina, raiz.GetProperty("pagina").GetInt32());
      Assert.Equal(1, raiz.GetProperty("tamanho").GetInt32());
      Assert.Equal(total, raiz.GetProperty("total").GetInt32());

      vistos.AddRange(raiz.GetProperty("itens").EnumerateArray().Select(i => i.GetProperty("id").GetInt32()));
    }

    using var depoisDoFim = await cliente.GetAsync($"/clientes?pagina={total + 1}&tamanho=1");
    using var fim = JsonDocument.Parse(await depoisDoFim.Content.ReadAsStringAsync());

    Assert.Equal(total, vistos.Count);
    Assert.Equal(total, vistos.Distinct().Count());
    Assert.Equal(0, fim.RootElement.GetProperty("itens").GetArrayLength());
  }

  // ----- EP-012 -----

  [Fact(DisplayName = "IT-EP012-01 — Cliente existente com animais: 200 com os dados do Cliente e os animais associados")]
  public async Task ObterClienteComAnimais()
  {
    var id = await ObterClienteIdAsync(EmailJoao);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Veterinario");

    using var resposta = await cliente.GetAsync($"/clientes/{id}");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;

    Assert.Equal(id, raiz.GetProperty("id").GetInt32());
    Assert.Equal(EmailJoao, raiz.GetProperty("email").GetString());
    Assert.Equal("210000015", raiz.GetProperty("nif").GetString());
    Assert.Equal("Ativo", raiz.GetProperty("estadoConta").GetString());
    Assert.Equal(
        "Bobi|Mia",
        string.Join('|', raiz.GetProperty("animais").EnumerateArray().Select(a => a.GetProperty("nome").GetString()).Order(StringComparer.Ordinal)));
  }

  [Fact(DisplayName = "IT-EP012-02 — identificador inexistente devolve 404")]
  public async Task ObterInexistenteDevolve404()
  {
    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.GetAsync($"/clientes/{await ObterClienteIdInexistenteAsync()}");

    await AssertErroAsync(resposta, HttpStatusCode.NotFound);
  }

  // ----- EP-013 -----

  [Fact(DisplayName = "IT-EP013-01 — NIF e e-mail livres: 201 com Location e o Cliente e o Utilizador Cliente ficam persistidos")]
  public async Task CriarClienteValido()
  {
    var email = DadosTeste.EmailUnico("cliente.novo");
    var nif = NifLivre();

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PostAsJsonAsync("/clientes", PedidoCriacao(email, nif));

    Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());
    var raiz = documento.RootElement;
    var id = raiz.GetProperty("id").GetInt32();

    Assert.NotNull(resposta.Headers.Location);
    Assert.EndsWith($"/clientes/{id}", resposta.Headers.Location!.ToString(), StringComparison.Ordinal);
    Assert.Equal(email, raiz.GetProperty("email").GetString());
    Assert.Equal(nif, raiz.GetProperty("nif").GetString());
    Assert.False(raiz.TryGetProperty("passwordHash", out _));

    var guardado = await LerClienteAsync(id);

    Assert.Equal("Ana Pereira", guardado.Nome);
    Assert.Equal(nif, guardado.Nif);
    Assert.Equal(email, guardado.Email);
    Assert.Equal("Cliente", guardado.Perfil);
    Assert.Equal("Ativo", guardado.Estado);
  }

  [Theory(DisplayName = "IT-EP013-02 — campo obrigatório em falta devolve 400 e nada é persistido")]
  [InlineData("nome")]
  [InlineData("telefone")]
  [InlineData("email")]
  [InlineData("passwordInicial")]
  public async Task CampoObrigatorioEmFaltaDevolve400(string campo)
  {
    var email = DadosTeste.EmailUnico("cliente.falta");
    var pedido = PedidoCriacao(email, NifLivre());
    pedido[campo] = null;
    var antes = await ContarClientesAsync();

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PostAsJsonAsync("/clientes", pedido);

    await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    Assert.Equal(antes, await ContarClientesAsync());
    Assert.Equal(0, await ContarContasAsync(email));
  }

  [Theory(DisplayName = "IT-EP013-03 — NIF inválido devolve 400 e nada é persistido")]
  [InlineData("123456780")]
  [InlineData("12345678")]
  [InlineData("12345678A")]
  public async Task NifInvalidoDevolve400(string nif)
  {
    var email = DadosTeste.EmailUnico("cliente.nif");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PostAsJsonAsync("/clientes", PedidoCriacao(email, nif));

    await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    Assert.Equal(0, await ContarContasAsync(email));
  }

  [Fact(DisplayName = "IT-EP013-04 — e-mail inválido devolve 400 e nada é persistido")]
  public async Task EmailInvalidoDevolve400()
  {
    var antes = await ContarClientesAsync();

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PostAsJsonAsync("/clientes", PedidoCriacao("ana.pereira.sem.arroba", NifLivre()));

    await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    Assert.Equal(antes, await ContarClientesAsync());
  }

  [Fact(DisplayName = "IT-EP013-05 — NIF já registado devolve 409 sem duplicação")]
  public async Task NifRepetidoDevolve409()
  {
    var email = DadosTeste.EmailUnico("cliente.nif.repetido");
    var antes = await ContarClientesAsync();

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PostAsJsonAsync("/clientes", PedidoCriacao(email, NifMaria));

    await AssertErroAsync(resposta, HttpStatusCode.Conflict);
    Assert.Equal(antes, await ContarClientesAsync());
    Assert.Equal(0, await ContarContasAsync(email));
    Assert.Equal(1, await ContarClientesComNifAsync(NifMaria));
  }

  [Theory(DisplayName = "IT-EP013-06 — e-mail já registado (de um Cliente ou de um funcionário) devolve 409 sem duplicação")]
  [InlineData(EmailMaria)]
  [InlineData(EmailVetAna)]
  public async Task EmailRepetidoDevolve409(string email)
  {
    var nif = NifLivre();
    var antes = await ContarClientesAsync();

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.PostAsJsonAsync("/clientes", PedidoCriacao(email.ToUpperInvariant(), nif));

    await AssertErroAsync(resposta, HttpStatusCode.Conflict);
    Assert.Equal(antes, await ContarClientesAsync());
    Assert.Equal(1, await ContarContasAsync(email));
    Assert.Equal(0, await ContarClientesComNifAsync(nif));
  }

  // ----- EP-014 -----

  [Fact(DisplayName = "IT-EP014-01 — Cliente existente: 200 e as alterações ficam persistidas")]
  public async Task AtualizarClienteExistente()
  {
    var id = await ObterClienteIdAsync(EmailJoao);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PutAsJsonAsync($"/clientes/{id}", new
    {
      nome = "João Costa",
      nif = "210000015",
      telefone = "961234000",
      email = EmailJoao,
      morada = "Av. Central, 22, Porto"
    });

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    var guardado = await LerClienteAsync(id);

    Assert.Equal("João Costa", guardado.Nome);
    Assert.Equal("961234000", guardado.Telefone);
    Assert.Equal("Av. Central, 22, Porto", guardado.Morada);
    Assert.Equal(EmailJoao, guardado.Email);
  }

  [Fact(DisplayName = "IT-EP014-02 — novo e-mail válido: o Cliente e a conta Utilizador passam a usar o novo e-mail")]
  public async Task AtualizarEmailAtualizaUtilizador()
  {
    var id = await ObterClienteIdAsync(EmailJoao);
    var antes = await LerClienteAsync(id);
    var novoEmail = DadosTeste.EmailUnico("joao.novo");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Administrador");

    using var resposta = await cliente.PutAsJsonAsync($"/clientes/{id}", PedidoAtualizacao(antes, email: novoEmail));

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

    Assert.Equal(novoEmail, documento.RootElement.GetProperty("email").GetString());
    Assert.Equal(novoEmail, (await LerClienteAsync(id)).Email);
    Assert.Equal(antes.UtilizadorId, await ObterUtilizadorIdAsync(novoEmail));
    Assert.Equal(0, await ContarContasAsync(EmailJoao));
  }

  [Theory(DisplayName = "IT-EP014-03 — dados inválidos devolvem 400 sem alterações")]
  [InlineData("nome", "")]
  [InlineData("nif", "123456780")]
  [InlineData("telefone", "abc")]
  [InlineData("email", "joao.sem.arroba")]
  public async Task DadosInvalidosDevolvem400(string campo, string valor)
  {
    var id = await ObterClienteIdAsync(EmailJoao);
    var antes = await LerClienteAsync(id);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PutAsJsonAsync($"/clientes/{id}", PedidoAtualizacao(antes, campo, valor));

    await AssertErroAsync(resposta, HttpStatusCode.BadRequest);
    Assert.Equal(antes, await LerClienteAsync(id));
  }

  [Fact(DisplayName = "IT-EP014-04 — Cliente inexistente devolve 404")]
  public async Task AtualizarInexistenteDevolve404()
  {
    var antes = await LerClienteAsync(await ObterClienteIdAsync(EmailJoao));

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PutAsJsonAsync(
        $"/clientes/{await ObterClienteIdInexistenteAsync()}",
        PedidoAtualizacao(antes, email: DadosTeste.EmailUnico("inexistente")));

    await AssertErroAsync(resposta, HttpStatusCode.NotFound);
  }

  [Fact(DisplayName = "IT-EP014-05 — NIF de outro Cliente devolve 409 sem alterações")]
  public async Task NifDeOutroClienteDevolve409()
  {
    var id = await ObterClienteIdAsync(EmailJoao);
    var antes = await LerClienteAsync(id);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PutAsJsonAsync($"/clientes/{id}", PedidoAtualizacao(antes, "nif", NifMaria));

    await AssertErroAsync(resposta, HttpStatusCode.Conflict);
    Assert.Equal(antes, await LerClienteAsync(id));
  }

  [Fact(DisplayName = "IT-EP014-06 — e-mail de outro Cliente devolve 409 sem alterações")]
  public async Task EmailDeOutroClienteDevolve409()
  {
    var id = await ObterClienteIdAsync(EmailJoao);
    var antes = await LerClienteAsync(id);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    using var resposta = await cliente.PutAsJsonAsync($"/clientes/{id}", PedidoAtualizacao(antes, email: EmailMaria));

    await AssertErroAsync(resposta, HttpStatusCode.Conflict);
    Assert.Equal(antes, await LerClienteAsync(id));
  }

  [Fact(DisplayName = "IT-EP014-07 — conflito durante a operação: dois pedidos simultâneos com o mesmo e-mail; um 200, um 409 e o perdedor mantém Cliente e Utilizador")]
  public async Task ConflitoSimultaneoEAtomico()
  {
    var idJoao = await ObterClienteIdAsync(EmailJoao);
    var idRui = await ObterClienteIdAsync(EmailRui);
    var antesJoao = await LerClienteAsync(idJoao);
    var antesRui = await LerClienteAsync(idRui);
    var emailDisputado = DadosTeste.EmailUnico("disputado");

    using var clienteA = await Fixture.CriarClienteAutenticadoAsync("Rececionista");
    using var clienteB = await Fixture.CriarClienteAutenticadoAsync("Rececionista");

    var tarefaJoao = clienteA.PutAsJsonAsync($"/clientes/{idJoao}", PedidoAtualizacao(antesJoao, "nome", "João Alterado", emailDisputado));
    var tarefaRui = clienteB.PutAsJsonAsync($"/clientes/{idRui}", PedidoAtualizacao(antesRui, "nome", "Rui Alterado", emailDisputado));

    using var respostaJoao = await tarefaJoao;
    using var respostaRui = await tarefaRui;

    var codigos = new[] { respostaJoao.StatusCode, respostaRui.StatusCode };

    Assert.Equal(1, codigos.Count(c => c == HttpStatusCode.OK));
    Assert.Equal(1, codigos.Count(c => c == HttpStatusCode.Conflict));
    Assert.Equal(1, await ContarContasAsync(emailDisputado));

    var (perdedorId, perdedorAntes) = respostaJoao.StatusCode == HttpStatusCode.Conflict
        ? (idJoao, antesJoao)
        : (idRui, antesRui);

    // Nem o nome do Cliente nem o e-mail da conta Utilizador do perdedor foram alterados.
    Assert.Equal(perdedorAntes, await LerClienteAsync(perdedorId));
  }

  // ----- 401 e 403 (contrato da API v02) -----

  [Theory(DisplayName = "I092 — EP-011 e EP-012: Auxiliar Veterinário e Cliente recebem 403")]
  [InlineData("AuxiliarVeterinario")]
  [InlineData("Cliente")]
  public async Task ConsultaSemPermissaoDevolve403(string perfil)
  {
    var id = await ObterClienteIdAsync(EmailJoao);

    using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

    using var pesquisar = await cliente.GetAsync("/clientes");
    using var obter = await cliente.GetAsync($"/clientes/{id}");

    await AssertErroAsync(pesquisar, HttpStatusCode.Forbidden);
    await AssertErroAsync(obter, HttpStatusCode.Forbidden);
  }

  [Theory(DisplayName = "I092 — EP-013 e EP-014: Veterinário, Auxiliar Veterinário e Cliente recebem 403 e nada é alterado")]
  [InlineData("Veterinario")]
  [InlineData("AuxiliarVeterinario")]
  [InlineData("Cliente")]
  public async Task GestaoSemPermissaoDevolve403(string perfil)
  {
    var id = await ObterClienteIdAsync(EmailJoao);
    var antes = await LerClienteAsync(id);
    var email = DadosTeste.EmailUnico("sem.permissao");

    using var cliente = await Fixture.CriarClienteAutenticadoAsync(perfil);

    using var criar = await cliente.PostAsJsonAsync("/clientes", PedidoCriacao(email, NifLivre()));
    using var atualizar = await cliente.PutAsJsonAsync($"/clientes/{id}", PedidoAtualizacao(antes, "nome", "Sem permissão"));

    await AssertErroAsync(criar, HttpStatusCode.Forbidden);
    await AssertErroAsync(atualizar, HttpStatusCode.Forbidden);
    Assert.Equal(0, await ContarContasAsync(email));
    Assert.Equal(antes, await LerClienteAsync(id));
  }

  [Fact(DisplayName = "I092 — EP-011 a EP-014: sem token devolvem 401 e nada é alterado")]
  public async Task SemTokenDevolve401()
  {
    var id = await ObterClienteIdAsync(EmailJoao);
    var antes = await LerClienteAsync(id);
    var email = DadosTeste.EmailUnico("sem.token");

    using var cliente = Fixture.Factory.CreateClient();

    using var pesquisar = await cliente.GetAsync("/clientes");
    using var obter = await cliente.GetAsync($"/clientes/{id}");
    using var criar = await cliente.PostAsJsonAsync("/clientes", PedidoCriacao(email, NifLivre()));
    using var atualizar = await cliente.PutAsJsonAsync($"/clientes/{id}", PedidoAtualizacao(antes, "nome", "Sem token"));

    await AssertErroAsync(pesquisar, HttpStatusCode.Unauthorized);
    await AssertErroAsync(obter, HttpStatusCode.Unauthorized);
    await AssertErroAsync(criar, HttpStatusCode.Unauthorized);
    await AssertErroAsync(atualizar, HttpStatusCode.Unauthorized);
    Assert.Equal(0, await ContarContasAsync(email));
    Assert.Equal(antes, await LerClienteAsync(id));
  }

  // ----- Auxiliares -----

  private static Dictionary<string, object?> PedidoCriacao(string email, string? nif) => new()
  {
    ["nome"] = "Ana Pereira",
    ["nif"] = nif,
    ["telefone"] = "961234567",
    ["email"] = email,
    ["morada"] = "Rua Nova, 5, Maia",
    ["passwordInicial"] = "Temp#2026x"
  };

  private static Dictionary<string, object?> PedidoAtualizacao(
      ClienteGuardado atual,
      string? campo = null,
      string? valor = null,
      string? email = null)
  {
    var pedido = new Dictionary<string, object?>
    {
      ["nome"] = atual.Nome,
      ["nif"] = atual.Nif,
      ["telefone"] = atual.Telefone,
      ["email"] = email ?? atual.Email,
      ["morada"] = atual.Morada
    };

    if (campo is not null)
    {
      pedido[campo] = valor;
    }

    return pedido;
  }

  /// <summary>NIF com dígito de controlo válido e ainda não usado (prefixo 29, fora dos dados da I061).</summary>
  private static string NifLivre()
  {
    var corpo = "29" + RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6", CultureInfo.InvariantCulture);
    var soma = 0;

    for (var i = 0; i < 8; i++)
    {
      soma += (corpo[i] - '0') * (9 - i);
    }

    var resto = soma % 11;
    var controlo = resto < 2 ? 0 : 11 - resto;

    return corpo + controlo.ToString(CultureInfo.InvariantCulture);
  }

  private static async Task<IReadOnlyList<string?>> PesquisarEmailsAsync(HttpClient cliente, string termo)
  {
    using var resposta = await cliente.GetAsync($"/clientes?pesquisa={Uri.EscapeDataString(termo)}");

    Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

    using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

    return documento.RootElement.GetProperty("itens").EnumerateArray()
        .Select(i => i.GetProperty("email").GetString())
        .ToList();
  }

  /// <summary>Formato único de erro da infraestrutura (I058): <c>code</c> igual ao código HTTP.</summary>
  private static async Task AssertErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado)
  {
    Assert.Equal(esperado, resposta.StatusCode);

    var corpo = await resposta.Content.ReadAsStringAsync();
    Assert.False(string.IsNullOrWhiteSpace(corpo), $"A resposta {(int)esperado} não tem corpo.");

    using var documento = JsonDocument.Parse(corpo);
    Assert.Equal((int)esperado, documento.RootElement.GetProperty("code").GetInt32());
  }

  private async Task<int> ObterClienteIdAsync(string email)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await (
            from c in db.Clientes.AsNoTracking()
            join u in db.Utilizadores.AsNoTracking() on c.UtilizadorId equals u.Id
            where u.Email == email
            select c.Id)
        .SingleAsync();
  }

  private async Task<int> ObterClienteIdInexistenteAsync()
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return (await db.Clientes.AsNoTracking().MaxAsync(c => (int?)c.Id) ?? 0) + 1000;
  }

  private async Task<int> ObterUtilizadorIdAsync(string email)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();
  }

  private async Task<int> ContarClientesAsync()
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await db.Clientes.AsNoTracking().CountAsync();
  }

  private async Task<int> ContarClientesComNifAsync(string nif)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await db.Clientes.AsNoTracking().CountAsync(c => c.NIF == nif);
  }

  private async Task<int> ContarContasAsync(string email)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    // A collation da base (SQL Server) não distingue maiúsculas de minúsculas.
    return await db.Utilizadores.AsNoTracking().CountAsync(u => u.Email == email);
  }

  private async Task<ClienteGuardado> LerClienteAsync(int id)
  {
    using var scope = Fixture.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

    return await (
            from c in db.Clientes.AsNoTracking()
            join u in db.Utilizadores.AsNoTracking() on c.UtilizadorId equals u.Id
            where c.Id == id
            select new ClienteGuardado(c.Nome, c.NIF, c.Telefone, c.Morada, u.Email, u.Perfil, u.Estado, u.Id))
        .SingleAsync();
  }

  private sealed record ClienteGuardado(
      string Nome,
      string? Nif,
      string Telefone,
      string? Morada,
      string Email,
      string Perfil,
      string Estado,
      int UtilizadorId);
}

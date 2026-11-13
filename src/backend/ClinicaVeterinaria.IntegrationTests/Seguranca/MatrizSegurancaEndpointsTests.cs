using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using ClinicaVeterinaria.Api.Seguranca;
using ClinicaVeterinaria.Application.Interfaces;
using ClinicaVeterinaria.Application.Seguranca;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ClinicaVeterinaria.IntegrationTests.Seguranca;

/// <summary>
/// I096 — matriz automática de segurança sobre todas as operações ativas da API (I030, secção
/// 14.1): para cada operação de <see cref="MatrizPerfis"/>, cada perfil autorizado passa a
/// autorização, cada perfil não autorizado recebe 403 e o pedido anónimo recebe 401 (IT-TRV-01 e
/// IT-TRV-05); os tokens expirados, adulterados e revogados recebem 401 em todas as operações
/// protegidas (IT-TRV-02 a 04); e os cenários nomeados IT-TRV-06 a 10.
/// </summary>
/// <remarks>
/// Cada pedido usa um token novo, emitido pelo serviço de JWT da API para a conta de demonstração
/// do perfil (I061), porque o EP-002 revoga o token com que é chamado. A base é reposta antes de
/// cada teste. Os erros 401 e 403 são verificados no formato único da I058.
/// </remarks>
public sealed partial class MatrizSegurancaEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    private static readonly Dictionary<string, string> EmailPorPerfil = new()
    {
        [Perfis.Administrador] = "admin@clinica-g08.test",
        [Perfis.Rececionista] = "rececao@clinica-g08.test",
        [Perfis.Veterinario] = "vet.ana@clinica-g08.test",
        [Perfis.AuxiliarVeterinario] = "aux.carla@clinica-g08.test",
        [Perfis.Cliente] = "cliente.joao@clinica-g08.test",
    };

    public MatrizSegurancaEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "I096 (critério 3) — a tabela de perfis cobre exatamente as rotas da API: nenhuma operação fica fora da matriz")]
    public void TabelaCobreTodasAsRotasDaApi()
    {
        var provider = Fixture.Factory.Services.GetRequiredService<IActionDescriptorCollectionProvider>();

        var daApi = provider.ActionDescriptors.Items
            .Where(a => a.AttributeRouteInfo?.Template is not null)
            .SelectMany(a => a.ActionConstraints!.OfType<HttpMethodActionConstraint>().SelectMany(c => c.HttpMethods)
                .Select(metodo => $"{metodo} {ParametroDeRota().Replace(a.AttributeRouteInfo!.Template!.Trim('/'), MatrizPerfis.Inexistente)}"))
            .Order(StringComparer.Ordinal)
            .ToList();

        var daTabela = MatrizPerfis.Operacoes
            .Select(o => $"{o.Metodo.Method} {o.Rota}")
            .Order(StringComparer.Ordinal)
            .ToList();

        Assert.Equal(daTabela, daApi);
    }

    [Theory(DisplayName = "IT-TRV-01 e IT-TRV-05 — matriz endpoint × perfil: autorizado passa, não autorizado recebe 403, anónimo recebe 401")]
    [MemberData(nameof(MatrizPerfis.Codigos), MemberType = typeof(MatrizPerfis))]
    public async Task MatrizEndpointPerfil(string codigo)
    {
        var operacao = MatrizPerfis.Obter(codigo);
        var falhas = new List<string>();

        foreach (var perfil in MatrizPerfis.TodosOsPerfis)
        {
            using var cliente = await ClienteDoPerfilAsync(perfil);
            using var resposta = await EnviarAsync(cliente, operacao);

            if (operacao.Permite(perfil))
            {
                if (resposta.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden || (int)resposta.StatusCode >= 500)
                {
                    falhas.Add($"{perfil}: esperado acesso, obtido {(int)resposta.StatusCode}");
                }
            }
            else
            {
                await VerificarErroAsync(resposta, HttpStatusCode.Forbidden, perfil, falhas);
            }
        }

        using var anonimo = Fixture.Factory.CreateClient();
        using var respostaAnonima = await EnviarAsync(anonimo, operacao);
        if (operacao.Publica)
        {
            if (respostaAnonima.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                falhas.Add($"Anónimo: {operacao.Codigo} é público, obtido {(int)respostaAnonima.StatusCode}");
            }
        }
        else
        {
            await VerificarErroAsync(respostaAnonima, HttpStatusCode.Unauthorized, "Anónimo", falhas);
        }

        Assert.True(falhas.Count == 0, $"{operacao}: {string.Join("; ", falhas)}");
    }

    [Fact(DisplayName = "IT-TRV-02 — JWT expirado (assinado com a chave certa) devolve 401 em todas as operações protegidas")]
    public async Task TokenExpiradoDevolve401()
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var opcoes = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>();

        // Emitido há 9 horas com a validade configurada de 8 horas: expirou há 1 hora.
        var geradorNoPassado = new JwtTokenService(opcoes, new RelogioFixo(DateTimeOffset.UtcNow.AddHours(-9)));
        var utilizador = await authService.ObterAutenticadoAsync(await ObterUtilizadorIdAsync(EmailPorPerfil[Perfis.Administrador]));
        var expirado = geradorNoPassado.GerarToken(utilizador);
        Assert.True(expirado.ExpiraEm < DateTime.UtcNow);

        await TodasAsProtegidasDevolvem401Async(expirado.Token);
    }

    [Theory(DisplayName = "IT-TRV-03 — JWT adulterado (perfil alterado ou assinatura trocada) devolve 401 em todas as operações protegidas")]
    [InlineData("perfil")]
    [InlineData("assinatura")]
    public async Task TokenAdulteradoDevolve401(string adulteracao)
    {
        var token = await TokenDeAsync(EmailPorPerfil[Perfis.Cliente]);
        var partes = token.Split('.');

        var adulterado = adulteracao == "perfil"
            ? $"{partes[0]}.{TrocarPerfil(partes[1], Perfis.Administrador)}.{partes[2]}"
            : $"{partes[0]}.{partes[1]}.{TrocarUltimoCaracter(partes[2])}";

        await TodasAsProtegidasDevolvem401Async(adulterado);
    }

    [Fact(DisplayName = "IT-TRV-04 — JWT revogado pelo fim de sessão (EP-002) devolve 401 em todas as operações protegidas")]
    public async Task TokenRevogadoDevolve401()
    {
        var token = await TokenDeAsync(EmailPorPerfil[Perfis.Administrador]);

        using (var cliente = ClienteComToken(token))
        using (var logout = await cliente.PostAsync("/auth/logout", null))
        {
            Assert.Equal(HttpStatusCode.NoContent, logout.StatusCode);
        }

        await TodasAsProtegidasDevolvem401Async(token);
    }

    [Fact(DisplayName = "IT-TRV-06 — o Cliente recebe 403 em todas as rotas fora de /auth/* e /me/*")]
    public async Task ClienteForaDaSuaAreaRecebe403()
    {
        var operacoes = MatrizPerfis.Operacoes
            .Where(o => !o.Rota.StartsWith("auth/", StringComparison.Ordinal) && !o.Rota.StartsWith("me/", StringComparison.Ordinal))
            .ToList();
        Assert.Equal(31, operacoes.Count);

        await PerfilRecebe403Async(Perfis.Cliente, operacoes);
    }

    [Fact(DisplayName = "IT-TRV-07 — o Administrador recebe 403 no registo clínico (EP-031 a EP-034)")]
    public Task AdministradorNoRegistoClinicoRecebe403() =>
        PerfilRecebe403Async(Perfis.Administrador, Operacoes("EP-031", "EP-032", "EP-033", "EP-034"));

    [Fact(DisplayName = "IT-TRV-08 — a Rececionista recebe 403 ao criar e ao alterar o registo clínico (EP-031, EP-032)")]
    public Task RececionistaNoRegistoClinicoRecebe403() =>
        PerfilRecebe403Async(Perfis.Rececionista, Operacoes("EP-031", "EP-032"));

    [Fact(DisplayName = "IT-TRV-09 — o Auxiliar recebe 403 ao registar ou alterar paciente e registo clínico (EP-017, EP-018, EP-031, EP-032)")]
    public Task AuxiliarNaoAlteraPacienteNemRegisto() =>
        PerfilRecebe403Async(Perfis.AuxiliarVeterinario, Operacoes("EP-017", "EP-018", "EP-031", "EP-032"));

    [Fact(DisplayName = "IT-TRV-10 — o Veterinário recebe 403 ao criar, alterar, mudar o estado ou registar a chegada da marcação (EP-025 a EP-028)")]
    public Task VeterinarioNaoAlteraMarcacao() =>
        PerfilRecebe403Async(Perfis.Veterinario, Operacoes("EP-025", "EP-026", "EP-027", "EP-028"));

    // ----- Auxiliares -----
    [GeneratedRegex(@"\{[^}]+\}")]
    private static partial Regex ParametroDeRota();

    private static List<MatrizPerfis.Operacao> Operacoes(params string[] codigos) =>
        codigos.Select(MatrizPerfis.Obter).ToList();

    private async Task PerfilRecebe403Async(string perfil, IReadOnlyList<MatrizPerfis.Operacao> operacoes)
    {
        var falhas = new List<string>();

        foreach (var operacao in operacoes)
        {
            Assert.False(operacao.Permite(perfil), $"{operacao} permite {perfil} na tabela.");

            using var cliente = await ClienteDoPerfilAsync(perfil);
            using var resposta = await EnviarAsync(cliente, operacao);
            await VerificarErroAsync(resposta, HttpStatusCode.Forbidden, operacao.ToString(), falhas);
        }

        Assert.True(falhas.Count == 0, string.Join("; ", falhas));
    }

    private async Task TodasAsProtegidasDevolvem401Async(string token)
    {
        var falhas = new List<string>();

        foreach (var operacao in MatrizPerfis.Operacoes.Where(o => !o.Publica))
        {
            using var cliente = ClienteComToken(token);
            using var resposta = await EnviarAsync(cliente, operacao);
            await VerificarErroAsync(resposta, HttpStatusCode.Unauthorized, operacao.ToString(), falhas);
        }

        Assert.True(falhas.Count == 0, string.Join("; ", falhas));
    }

    private static async Task<HttpResponseMessage> EnviarAsync(HttpClient cliente, MatrizPerfis.Operacao operacao)
    {
        using var pedido = new HttpRequestMessage(operacao.Metodo, "/" + operacao.Rota);
        if (operacao.TemCorpo)
        {
            pedido.Content = new StringContent("{}", Encoding.UTF8, "application/json");
        }

        return await cliente.SendAsync(pedido);
    }

    /// <summary>O código esperado, no formato de erro da I058 (<c>code</c>, <c>message</c>, <c>detail</c>).</summary>
    private static async Task VerificarErroAsync(HttpResponseMessage resposta, HttpStatusCode esperado, string quem, List<string> falhas)
    {
        if (resposta.StatusCode != esperado)
        {
            falhas.Add($"{quem}: esperado {(int)esperado}, obtido {(int)resposta.StatusCode}");
            return;
        }

        var corpo = await resposta.Content.ReadAsStringAsync();
        try
        {
            using var documento = JsonDocument.Parse(corpo);
            var raiz = documento.RootElement;
            if (raiz.GetProperty("code").GetInt32() != (int)esperado
                || string.IsNullOrWhiteSpace(raiz.GetProperty("message").GetString())
                || string.IsNullOrWhiteSpace(raiz.GetProperty("detail").GetString()))
            {
                falhas.Add($"{quem}: {(int)esperado} fora do formato da I058");
            }
        }
        catch (Exception erro) when (erro is JsonException or KeyNotFoundException or InvalidOperationException)
        {
            falhas.Add($"{quem}: {(int)esperado} sem corpo no formato da I058");
        }
    }

    private async Task<HttpClient> ClienteDoPerfilAsync(string perfil) =>
        ClienteComToken(await TokenDeAsync(EmailPorPerfil[perfil]));

    private HttpClient ClienteComToken(string token)
    {
        var cliente = Fixture.Factory.CreateClient();
        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return cliente;
    }

    private async Task<string> TokenDeAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();

        var utilizador = await authService.ObterAutenticadoAsync(await ObterUtilizadorIdAsync(email));

        return jwtTokenService.GerarToken(utilizador).Token;
    }

    private async Task<int> ObterUtilizadorIdAsync(string email)
    {
        using var scope = Fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();

        return await db.Utilizadores.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).SingleAsync();
    }

    /// <summary>Muda o claim <c>role</c> do payload, mantendo a assinatura original.</summary>
    private static string TrocarPerfil(string payloadBase64Url, string perfil)
    {
        var json = Encoding.UTF8.GetString(DescodificarBase64Url(payloadBase64Url));
        var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)!;
        claims["role"] = JsonSerializer.SerializeToElement(perfil);

        return CodificarBase64Url(JsonSerializer.SerializeToUtf8Bytes(claims));
    }

    private static string TrocarUltimoCaracter(string assinatura) =>
        assinatura[..^2] + (assinatura[^2] == 'A' ? 'B' : 'A') + assinatura[^1];

    private static byte[] DescodificarBase64Url(string parte)
    {
        var base64 = parte.Replace('-', '+').Replace('_', '/');
        return Convert.FromBase64String(base64.PadRight(base64.Length + ((4 - (base64.Length % 4)) % 4), '='));
    }

    private static string CodificarBase64Url(byte[] bytes) =>
        Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private sealed class RelogioFixo(DateTimeOffset agora) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => agora;
    }
}

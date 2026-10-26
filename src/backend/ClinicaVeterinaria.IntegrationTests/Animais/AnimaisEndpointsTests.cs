using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaVeterinaria.Infrastructure;
using ClinicaVeterinaria.IntegrationTests.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicaVeterinaria.IntegrationTests.Animais;

public sealed class AnimaisEndpointsTests :
    TesteIntegracaoBase,
    IClassFixture<IntegracaoFixture>
{
    public AnimaisEndpointsTests(IntegracaoFixture fixture)
        : base(fixture)
    {
    }

    [Fact(DisplayName = "IT-EP015 — pesquisar pacientes devolve resultados")]
    public async Task PesquisarPacientesDevolveResultados()
    {
        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync("Rececionista");

        using var resposta =
            await cliente.GetAsync(
                "/animais?pesquisa=Bobi&pagina=1&tamanho=100");

        Assert.Equal(
            HttpStatusCode.OK,
            resposta.StatusCode);

        using var documento =
            JsonDocument.Parse(
                await resposta.Content.ReadAsStringAsync());

        var itens =
            documento.RootElement.GetProperty("itens");

        Assert.Contains(
            itens.EnumerateArray(),
            item =>
                item.GetProperty("nome").GetString() == "Bobi");
    }

    [Fact(DisplayName = "IT-EP015 — perfil Cliente não pode pesquisar pacientes")]
    public async Task ClienteNaoPodePesquisarPacientes()
    {
        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync("Cliente");

        using var resposta =
            await cliente.GetAsync("/animais");

        Assert.Equal(
            HttpStatusCode.Forbidden,
            resposta.StatusCode);
    }

    [Fact(DisplayName = "IT-EP016 — Auxiliar consulta apenas ficha administrativa")]
    public async Task AuxiliarPodeConsultarFichaAdministrativa()
    {
        var animalId =
            await ObterAnimalIdAsync("Bobi");

        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "AuxiliarVeterinario");

        using var resposta =
            await cliente.GetAsync(
                $"/animais/{animalId}");

        Assert.Equal(
            HttpStatusCode.OK,
            resposta.StatusCode);

        using var documento =
            JsonDocument.Parse(
                await resposta.Content.ReadAsStringAsync());

        var raiz =
            documento.RootElement;

        Assert.Equal(
            "Bobi",
            raiz.GetProperty("nome").GetString());

        Assert.False(
            raiz.TryGetProperty(
                "historicoClinico",
                out _));

        Assert.False(
            raiz.TryGetProperty(
                "diagnostico",
                out _));

        Assert.False(
            raiz.TryGetProperty(
                "notasClinicas",
                out _));
    }

    [Fact(DisplayName = "IT-EP016 — animal inexistente devolve 404")]
    public async Task AnimalInexistenteDevolve404()
    {
        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "Rececionista");

        using var resposta =
            await cliente.GetAsync(
                "/animais/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            resposta.StatusCode);
    }

    [Fact(DisplayName = "IT-EP017 — registar paciente válido devolve 201")]
    public async Task RegistarPacienteValido()
    {
        var clienteId =
            await ObterClienteIdAsync(
                "cliente.joao@clinica-g08.test");

        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "Rececionista");

        var pedido = new
        {
            nome = "Thor I069",
            especie = "Cão",
            raca = "Labrador Retriever",
            sexo = "Macho",
            dataNascimento = "2022-05-10",
            peso = 25.40m,
            clienteId
        };

        using var resposta =
            await cliente.PostAsJsonAsync(
                "/animais",
                pedido);

        Assert.Equal(
            HttpStatusCode.Created,
            resposta.StatusCode);

        Assert.NotNull(
            resposta.Headers.Location);

        using var documento =
            JsonDocument.Parse(
                await resposta.Content.ReadAsStringAsync());

        Assert.Equal(
            "Thor I069",
            documento.RootElement
                .GetProperty("nome")
                .GetString());
    }

    [Theory(DisplayName = "IT-EP017 — peso não positivo devolve 400")]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task PesoNaoPositivoDevolve400(
        decimal peso)
    {
        var clienteId =
            await ObterClienteIdAsync(
                "cliente.joao@clinica-g08.test");

        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "Veterinario");

        var pedido = new
        {
            nome = "Peso Inválido",
            especie = "Cão",
            sexo = "Macho",
            peso,
            clienteId
        };

        using var resposta =
            await cliente.PostAsJsonAsync(
                "/animais",
                pedido);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            resposta.StatusCode);
    }

    [Fact(DisplayName = "IT-EP017 — data de nascimento futura devolve 400")]
    public async Task DataNascimentoFuturaDevolve400()
    {
        var clienteId =
            await ObterClienteIdAsync(
                "cliente.joao@clinica-g08.test");

        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "Rececionista");

        var pedido = new
        {
            nome = "Animal Futuro",
            especie = "Cão",
            sexo = "Macho",
            dataNascimento = "2030-01-01",
            peso = 10m,
            clienteId
        };

        using var resposta =
            await cliente.PostAsJsonAsync(
                "/animais",
                pedido);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            resposta.StatusCode);
    }

    [Fact(DisplayName = "IT-EP017 — Cliente inexistente devolve 404")]
    public async Task CriarComClienteInexistenteDevolve404()
    {
        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "Rececionista");

        var pedido = new
        {
            nome = "Sem Dono",
            especie = "Cão",
            sexo = "Macho",
            peso = 10m,
            clienteId = 999999
        };

        using var resposta =
            await cliente.PostAsJsonAsync(
                "/animais",
                pedido);

        Assert.Equal(
            HttpStatusCode.NotFound,
            resposta.StatusCode);
    }

    [Fact(DisplayName = "IT-EP018 — atualizar paciente persiste alterações")]
    public async Task AtualizarPacienteValido()
    {
        var animalId =
            await ObterAnimalIdAsync("Bobi");

        var clienteId =
            await ObterClienteIdAsync(
                "cliente.joao@clinica-g08.test");

        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "Veterinario");

        var pedido = new
        {
            nome = "Bobi Atualizado",
            especie = "Cão",
            raca = "Labrador Retriever",
            sexo = "Macho",
            dataNascimento = "2022-03-10",
            peso = 29.10m,
            clienteId
        };

        using var resposta =
            await cliente.PutAsJsonAsync(
                $"/animais/{animalId}",
                pedido);

        Assert.Equal(
            HttpStatusCode.OK,
            resposta.StatusCode);

        using var consulta =
            await cliente.GetAsync(
                $"/animais/{animalId}");

        Assert.Equal(
            HttpStatusCode.OK,
            consulta.StatusCode);

        using var documento =
            JsonDocument.Parse(
                await consulta.Content.ReadAsStringAsync());

        Assert.Equal(
            "Bobi Atualizado",
            documento.RootElement
                .GetProperty("nome")
                .GetString());

        Assert.Equal(
            29.10m,
            documento.RootElement
                .GetProperty("peso")
                .GetDecimal());
    }

    [Fact(DisplayName = "IT-EP018 — Cliente inexistente devolve 404 sem alterar paciente")]
    public async Task AtualizarParaClienteInexistenteMantemEstadoAnterior()
    {
        var animalId =
            await ObterAnimalIdAsync("Bobi");

        using var cliente =
            await Fixture.CriarClienteAutenticadoAsync(
                "Rececionista");

        var pedido = new
        {
            nome = "Nome Que Não Deve Ser Gravado",
            especie = "Cão",
            sexo = "Macho",
            peso = 20m,
            clienteId = 999999
        };

        using var resposta =
            await cliente.PutAsJsonAsync(
                $"/animais/{animalId}",
                pedido);

        Assert.Equal(
            HttpStatusCode.NotFound,
            resposta.StatusCode);

        using var consulta =
            await cliente.GetAsync(
                $"/animais/{animalId}");

        Assert.Equal(
            HttpStatusCode.OK,
            consulta.StatusCode);

        using var documento =
            JsonDocument.Parse(
                await consulta.Content.ReadAsStringAsync());

        Assert.Equal(
            "Bobi",
            documento.RootElement
                .GetProperty("nome")
                .GetString());
    }

    private async Task<int> ObterAnimalIdAsync(
        string nome)
    {
        using var scope =
            Fixture.Factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ClinicaDbContext>();

        return await db.Pacientes
            .AsNoTracking()
            .Where(p => p.Nome == nome)
            .Select(p => p.Id)
            .SingleAsync();
    }

    private async Task<int> ObterClienteIdAsync(
        string email)
    {
        using var scope =
            Fixture.Factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ClinicaDbContext>();

        return await (
            from c in db.Clientes.AsNoTracking()
            join u in db.Utilizadores.AsNoTracking()
                on c.UtilizadorId equals u.Id
            where u.Email == email
            select c.Id)
            .SingleAsync();
    }
}

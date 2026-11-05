using System.Reflection;
using ClinicaVeterinaria.Application.DTOs.RegistosClinicos;
using ClinicaVeterinaria.Application.Interfaces;

namespace ClinicaVeterinaria.UnitTests.RegistosClinicos;

/// <summary>
/// I088 — projeção do registo clínico autorizada ao Auxiliar Veterinário (descrição e critério 5 da
/// I088 no Backlog de Projeto v03). Fixa os campos de cada DTO de EP-033 e EP-034, para um campo novo
/// não entrar sem ser revisto (como a I072 fez para a agenda, em <c>ProjecaoConsultaTests</c>), e
/// confirma que a leitura não depende do perfil.
/// </summary>
/// <remarks>
/// Pelo contrato v02 (EP-033), pelo UC13 e pelo PA-08 da I028, o Auxiliar Veterinário lê o mesmo
/// registo completo que o Veterinário. Que essa projeção não lhe concede operações de edição é
/// verificado no controlador (<c>RegistosClinicosControllerTests</c>: EP-031 e EP-032 exigem a
/// política Veterinário e o Auxiliar só tem ações GET). O ponto em aberto 4 da Documentação de API
/// v02 (projeção reduzida para o Auxiliar) continua por decidir; se o grupo a aprovar, estes testes
/// passam a falhar até a nova projeção ser implementada e testada.
/// </remarks>
public class ProjecaoRegistoClinicoTests
{
    // Campos com o detalhe clínico do registo (Modelo de Dados v02) que o resumo do histórico não
    // pode ter.
    private static readonly string[] DetalheClinico =
    [
        "NotasClinicas",
        "Tratamentos",
        "Prescricoes",
        "Descricao",
        "Medicamento",
        "Dosagem",
        "Frequencia",
        "DuracaoDias"
    ];

    [Fact(DisplayName = "I088 (critério 5) — o resumo do histórico (EP-034) só tem os campos do resumo")]
    public void ResumoSoComOsCamposDoResumo()
    {
        Assert.Equal(
            "ConsultaId,Data,Diagnostico,Id,NumeroPrescricoes,NumeroTratamentos,Veterinario",
            Campos(typeof(RegistoClinicoResumoDto)));
        Assert.Equal("Animal,Registos", Campos(typeof(HistoricoClinicoDto)));
    }

    [Fact(DisplayName = "I088 (critério 5) — o resumo do histórico (EP-034) não expõe as notas clínicas nem o detalhe dos tratamentos e prescrições")]
    public void ResumoSemDetalheClinico()
    {
        Assert.Empty(NomesDosCampos(typeof(RegistoClinicoResumoDto)).Intersect(DetalheClinico, StringComparer.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "I088 (critério 5, PA-08) — o registo de EP-033 tem os campos do contrato v02")]
    public void RegistoComOsCamposDoContrato()
    {
        Assert.Equal(
            "AnimalId,AtualizadoEm,AtualizadoPor,ConsultaId,CriadoEm,Diagnostico,Id,NotasClinicas,Prescricoes,Tratamentos,Veterinario",
            Campos(typeof(RegistoClinicoDto)));
        Assert.Equal("DataAplicacao,Descricao,Id", Campos(typeof(TratamentoDto)));
        Assert.Equal("Dosagem,DuracaoDias,Frequencia,Id,Medicamento", Campos(typeof(PrescricaoDto)));
    }

    [Fact(DisplayName = "I088 (critério 5, PA-08) — a leitura não depende do perfil: o serviço não recebe o utilizador em EP-033 nem em EP-034")]
    public void LeituraIgualParaVeterinarioEAuxiliar()
    {
        var servico = typeof(IRegistosClinicosService);

        Assert.Equal(
            new[] { typeof(int) },
            Parametros(servico, nameof(IRegistosClinicosService.ObterAsync)));
        Assert.Equal(
            new[] { typeof(int), typeof(HistoricoClinicoQuery) },
            Parametros(servico, nameof(IRegistosClinicosService.ObterHistoricoAsync)));
    }

    /// <summary>Nomes das propriedades públicas, por ordem alfabética e separados por vírgulas.</summary>
    private static string Campos(Type tipo) =>
        string.Join(',', NomesDosCampos(tipo));

    private static IEnumerable<string> NomesDosCampos(Type tipo) =>
        tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .Order(StringComparer.Ordinal);

    private static Type[] Parametros(Type tipo, string metodo) =>
        tipo.GetMethod(metodo)!
            .GetParameters()
            .Select(p => p.ParameterType)
            .ToArray();
}

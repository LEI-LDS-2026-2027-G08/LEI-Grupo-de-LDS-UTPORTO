using System.Reflection;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Consultas;

namespace ClinicaVeterinaria.UnitTests.Consultas;

/// <summary>
/// I072 — critério 5: a resposta ao Auxiliar Veterinário (a mesma de EP-023 e EP-024 para os três
/// perfis) inclui as notas de preparação e não tem dados clínicos. Fixa os campos de cada DTO da
/// resposta, para um campo novo não entrar sem ser revisto.
/// </summary>
public class ProjecaoConsultaTests
{
    // Campos do registo clínico, tratamentos e prescrições (Modelo de Dados v02) que nunca podem
    // aparecer na agenda nem na marcação.
    private static readonly string[] CamposClinicos =
    [
        "NotasClinicas",
        "Diagnostico",
        "Tratamentos",
        "Prescricoes",
        "RegistoClinico",
        "Medicamento",
        "Dosagem",
        "Peso"
    ];

    [Fact(DisplayName = "I072 — ConsultaDto (EP-024): só dados administrativos e as notas de preparação")]
    public void ConsultaDtoSemDadosClinicos()
    {
        Assert.Equal("Animal,ChegadaEm,Cliente,DataHora,DuracaoMinutos,Estado,Id,Motivo,NotasPreparacao,Veterinario", Campos(typeof(ConsultaDto)));
    }

    [Fact(DisplayName = "I072 — ConsultaListItemDto (EP-023): só dados administrativos")]
    public void ConsultaListItemDtoSemDadosClinicos()
    {
        Assert.Equal("Animal,ChegadaEm,Cliente,DataHora,DuracaoMinutos,Estado,Id,Motivo,Veterinario", Campos(typeof(ConsultaListItemDto)));
    }

    [Fact(DisplayName = "I072 — NotaPreparacaoDto e as referências ao animal, ao proprietário e ao Veterinário")]
    public void DtosAninhadosSemDadosClinicos()
    {
        Assert.Equal("Autor,ConsultaId,CriadaEm,Id,Texto", Campos(typeof(NotaPreparacaoDto)));
        Assert.Equal("Especie,Id,Nome", Campos(typeof(AnimalRefDto)));
        Assert.Equal("Id,Nome,Telefone", Campos(typeof(ClienteRefDto)));
        Assert.Equal("Id,Nome", Campos(typeof(VeterinarioRefDto)));
    }

    [Theory(DisplayName = "I072 — nenhum DTO da agenda tem um campo clínico")]
    [InlineData(typeof(ConsultaDto))]
    [InlineData(typeof(ConsultaListItemDto))]
    [InlineData(typeof(NotaPreparacaoDto))]
    [InlineData(typeof(AnimalRefDto))]
    [InlineData(typeof(ClienteRefDto))]
    [InlineData(typeof(VeterinarioRefDto))]
    public void SemCamposClinicos(Type dto)
    {
        Assert.Empty(NomesDosCampos(dto).Intersect(CamposClinicos, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>Nomes das propriedades públicas, por ordem alfabética e separados por vírgulas.</summary>
    private static string Campos(Type tipo) =>
        string.Join(',', NomesDosCampos(tipo));

    private static IEnumerable<string> NomesDosCampos(Type tipo) =>
        tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .Order(StringComparer.Ordinal);
}

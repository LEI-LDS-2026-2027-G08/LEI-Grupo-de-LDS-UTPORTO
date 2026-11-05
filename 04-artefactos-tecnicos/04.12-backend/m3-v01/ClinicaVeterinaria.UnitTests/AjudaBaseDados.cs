using System.Globalization;
using ClinicaVeterinaria.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.UnitTests;

/// <summary>
/// <see cref="ClinicaDbContext"/> com o fornecedor EF Core InMemory, como prevê a
/// Especificação de Testes Unitários (I028, secção 4.2). Não aplica as restrições do
/// SQL Server (chaves estrangeiras, UNIQUE, CHECK); essas ficam para os testes de integração.
/// </summary>
internal static class AjudaBaseDados
{
    /// <summary>Contexto sobre uma base em memória nova, isolada dos outros testes.</summary>
    public static ClinicaDbContext CriarContexto() =>
        CriarContexto(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

    /// <summary>
    /// Contexto sobre a base em memória <paramref name="nomeBase"/>. Dois contextos com o
    /// mesmo nome partilham os dados, como dois pedidos (ou dois arranques da API) sobre a
    /// mesma base de dados.
    /// </summary>
    public static ClinicaDbContext CriarContexto(string nomeBase) =>
        new(new DbContextOptionsBuilder<ClinicaDbContext>()
            .UseInMemoryDatabase(nomeBase)
            .Options);
}

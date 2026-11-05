using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// Bloqueio exclusivo da agenda de um Veterinário durante a transação atual
/// (<c>sp_getapplock</c> do SQL Server), para que a verificação «está livre?» e a gravação da
/// marcação sejam feitas por um pedido de cada vez (I073, critério 2).
/// </summary>
/// <remarks>
/// Sem o bloqueio, dois pedidos simultâneos para a mesma vaga leriam ambos a agenda vazia e
/// gravariam as duas marcações: o <c>RowVersion</c> só protege alterações a uma linha que já
/// existe, não a inserção de duas linhas novas. Com o bloqueio, o segundo pedido espera que o
/// primeiro termine, volta a ler a agenda (já com a primeira marcação) e recebe 422.
/// O bloqueio é libertado no fim da transação (commit ou rollback). As operações que alteram a
/// data, a hora ou o Veterinário de uma marcação (EP-026, I074) devem usar o mesmo bloqueio.
/// Na base de dados em memória dos testes unitários não há transações nem bloqueios: o método
/// não faz nada.
/// </remarks>
internal static class BloqueioAgendaVeterinario
{
    /// <summary>Número do erro lançado quando o bloqueio não é obtido a tempo.</summary>
    public const int NumeroErroBloqueioNaoObtido = 51073;

    /// <summary>Tempo máximo de espera pelo bloqueio, em milissegundos.</summary>
    public const int EsperaMaximaMilissegundos = 15000;

    private const string MensagemOcupada =
        "A agenda do Veterinário está a ser alterada por outro pedido. Tente novamente.";

    /// <summary>
    /// Obtém o bloqueio da agenda de <paramref name="veterinarioId"/> até ao fim da transação
    /// atual, que tem de estar aberta. Se não o obtiver a tempo: 409.
    /// </summary>
    public static async Task AdquirirAsync(ClinicaDbContext db, int veterinarioId)
    {
        ArgumentNullException.ThrowIfNull(db);

        if (!db.Database.IsRelational())
        {
            return;
        }

        if (db.Database.CurrentTransaction is null)
        {
            throw new InvalidOperationException("O bloqueio da agenda exige uma transação aberta.");
        }

        var recurso = "agenda-veterinario-" + veterinarioId.ToString(System.Globalization.CultureInfo.InvariantCulture);

        try
        {
            await db.Database.ExecuteSqlInterpolatedAsync($@"
DECLARE @resultado INT;
EXEC @resultado = sp_getapplock
    @Resource = {recurso},
    @LockMode = 'Exclusive',
    @LockOwner = 'Transaction',
    @LockTimeout = {EsperaMaximaMilissegundos};
IF @resultado < 0
    THROW 51073, N'Bloqueio da agenda não obtido.', 1;");
        }
        catch (SqlException erro) when (erro.Number == NumeroErroBloqueioNaoObtido)
        {
            // Mapeado para 409 pelo middleware (InvalidOperationException).
            throw new InvalidOperationException(MensagemOcupada, erro);
        }
    }
}

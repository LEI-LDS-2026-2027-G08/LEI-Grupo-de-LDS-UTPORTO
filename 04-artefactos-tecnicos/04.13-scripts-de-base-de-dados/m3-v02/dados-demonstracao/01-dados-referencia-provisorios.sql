/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I061 — Escrever os scripts de dados iniciais e de demonstração
   Ficheiro       : dados-demonstracao/01-dados-referencia-provisorios.sql
   Versão         : m3-v02
   Finalidade     : APOIO PROVISÓRIO. Insere, apenas se ainda não existirem, as
                    linhas de referência de que os dados de demonstração dependem:
                    Clinica (Id = 1), ConfiguracaoSistema, Especie e Raca.
                    Pelo Modelo de Dados v02 (ponto 9.3, opção a), estas linhas
                    pertencem ao HasData das migrações da I062. Este script só se
                    usa numa base criada pelos scripts m3-v02 (que não as têm),
                    enquanto as migrações da I062 não existirem.
   Quando NÃO usar: numa base criada pelas migrações da I062 — o HasData já criou
                    estas linhas e o script não faz nada (verifica pelo nome).
   Base de dados  : A da ligação (sqlcmd -d); o script não fixa o nome.
   Ordem          : 1 de 2 — depois dos scripts 00 a 06 da m3-v02.
   Reexecução     : Idempotente: cada linha só é inserida se não existir
                    (Clinica pelo Id, ConfiguracaoSistema pela Chave, Especie
                    pelo Nome e Raca pelo par Especie + Nome).
   Dados          : Fictícios. Os valores da clínica seguem o exemplo de EP-009
                    da Documentação da API v02, com um NIF fictício válido
                    (dígito de controlo correto). Sem dados pessoais nem credenciais.
   ============================================================================= */

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
SET QUOTED_IDENTIFIER ON;
GO

/* A base de dados é indicada na ligação (sqlcmd -d <base> ou executar-dados.ps1
   -BaseDados), sem nome fixo. Proteção: nunca correr numa base de sistema. */
DECLARE @BaseAtual SYSNAME = DB_NAME();
IF @BaseAtual IN (N'master', N'model', N'msdb', N'tempdb')
BEGIN
    RAISERROR(N'[01] Base de dados não indicada (ligação em %s). Executar com sqlcmd -d <base> ou executar-dados.ps1 -BaseDados <base>.', 16, 1, @BaseAtual) WITH NOWAIT;
    SET NOEXEC ON;
END;
GO

DECLARE @Data DATETIME2(0) = '2026-10-25T00:00:00';   -- data fixa (DI-10): nunca a hora atual
DECLARE @N INT;

BEGIN TRANSACTION;

/* Clinica — linha única (CK_Clinica_LinhaUnica) */
INSERT INTO dbo.Clinica (Id, Nome, NIF, Morada, Telefone, Email, DataUltimaAlteracao)
SELECT 1, N'Clínica Veterinária G08', '501234560', N'Rua do Exemplo, 100, 4470-000 Maia', N'229000000', N'geral@clinica-g08.test', @Data
WHERE NOT EXISTS (SELECT 1 FROM dbo.Clinica WHERE Id = 1);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[01] Clinica: ', @N, N' linha(s) inserida(s).');

/* ConfiguracaoSistema */
INSERT INTO dbo.ConfiguracaoSistema (Chave, Valor, Descricao, DataUltimaAlteracao)
SELECT v.Chave, v.Valor, v.Descricao, @Data
FROM (VALUES
    (N'DuracaoPadraoConsultaMinutos', N'30', N'Duração por omissão de uma consulta, em minutos (EP-009 e EP-010).')
) AS v (Chave, Valor, Descricao)
WHERE NOT EXISTS (SELECT 1 FROM dbo.ConfiguracaoSistema AS c WHERE c.Chave = v.Chave);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[01] ConfiguracaoSistema: ', @N, N' linha(s) inserida(s).');

/* Especie */
INSERT INTO dbo.Especie (Nome)
SELECT v.Nome
FROM (VALUES (N'Cão'), (N'Gato')) AS v (Nome)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Especie AS e WHERE e.Nome = v.Nome);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[01] Especie: ', @N, N' linha(s) inserida(s).');

/* Raca (por espécie) */
INSERT INTO dbo.Raca (EspecieId, Nome)
SELECT e.Id, v.Raca
FROM (VALUES
    (N'Cão',  N'Labrador Retriever'),
    (N'Cão',  N'Pastor Alemão'),
    (N'Cão',  N'Sem raça definida'),
    (N'Gato', N'Europeu Comum'),
    (N'Gato', N'Siamês')
) AS v (Especie, Raca)
JOIN dbo.Especie AS e ON e.Nome = v.Especie
WHERE NOT EXISTS (SELECT 1 FROM dbo.Raca AS r WHERE r.EspecieId = e.Id AND r.Nome = v.Raca);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[01] Raca: ', @N, N' linha(s) inserida(s).');

COMMIT TRANSACTION;
PRINT N'[01] Dados de referência provisórios concluídos.';
GO

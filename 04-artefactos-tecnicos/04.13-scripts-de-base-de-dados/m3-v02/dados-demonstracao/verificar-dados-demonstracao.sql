/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I061 — Escrever os scripts de dados iniciais e de demonstração
   Ficheiro       : dados-demonstracao/verificar-dados-demonstracao.sql
   Versão         : m3-v02
   Finalidade     : Confirmar, só por leitura, que os dados de demonstração cobrem
                    os critérios de aceitação 2 e 3 da I061 e são coerentes entre
                    si (verificações 2 e 3 do Revisor). Termina com erro se
                    alguma verificação falhar.
   Base de dados  : A da ligação (sqlcmd -d); o script não fixa o nome.
   Execução       : Depois de 02-dados-demonstracao.sql.
   ============================================================================= */

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* A base de dados é indicada na ligação (sqlcmd -d <base> ou executar-dados.ps1
   -BaseDados), sem nome fixo. Proteção: nunca correr numa base de sistema. */
IF DB_NAME() IN (N'master', N'model', N'msdb', N'tempdb')
BEGIN
    RAISERROR(N'[VER] Base de dados não indicada (ligação em %s). Executar com sqlcmd -d <base> ou executar-dados.ps1 -BaseDados <base>.', 16, 1, DB_NAME()) WITH NOWAIT;
    SET NOEXEC ON;
END;
GO

DECLARE @R TABLE (Ordem INT IDENTITY(1,1), Codigo NVARCHAR(5), Verificacao NVARCHAR(200), Esperado NVARCHAR(100), Obtido NVARCHAR(100), Resultado NVARCHAR(10));
DECLARE @Dominio NVARCHAR(30) = N'%@clinica-g08.test';
DECLARE @V INT;

/* Critério 2 — contas */
INSERT INTO @R (Codigo, Verificacao, Esperado, Obtido, Resultado)
SELECT N'D01', N'Conta ativa no perfil ' + p.Perfil, N'>= 1', CAST(COUNT(u.Id) AS NVARCHAR(10)),
       CASE WHEN COUNT(u.Id) >= 1 THEN N'OK' ELSE N'FALHOU' END
FROM (VALUES (N'Administrador'), (N'Rececionista'), (N'Veterinario'), (N'AuxiliarVeterinario'), (N'Cliente')) AS p (Perfil)
LEFT JOIN dbo.Utilizador AS u ON u.Perfil = p.Perfil AND u.Estado = N'Ativo' AND u.Email LIKE @Dominio
GROUP BY p.Perfil;

SET @V = (SELECT COUNT(*) FROM dbo.Utilizador WHERE Estado = N'Bloqueado' AND Email LIKE @Dominio);
INSERT INTO @R VALUES (N'D02', N'Conta bloqueada', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.Funcionario AS f JOIN dbo.Utilizador AS u ON u.Id = f.UtilizadorId
          WHERE u.Perfil = N'AuxiliarVeterinario' AND u.Estado = N'Ativo' AND f.PodeRegistarApoio = 1);
INSERT INTO @R VALUES (N'D03', N'Auxiliar Veterinário ativo COM autorização de registo de apoio', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.Funcionario AS f JOIN dbo.Utilizador AS u ON u.Id = f.UtilizadorId
          WHERE u.Perfil = N'AuxiliarVeterinario' AND u.Estado = N'Ativo' AND f.PodeRegistarApoio = 0);
INSERT INTO @R VALUES (N'D04', N'Auxiliar Veterinário ativo SEM autorização de registo de apoio', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.Utilizador AS u
          WHERE u.Perfil <> N'Cliente' AND u.Email LIKE @Dominio
            AND NOT EXISTS (SELECT 1 FROM dbo.Funcionario AS f WHERE f.UtilizadorId = u.Id));
INSERT INTO @R VALUES (N'D05', N'Contas internas sem linha em Funcionario', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.Utilizador AS u
          WHERE u.Perfil = N'Cliente' AND u.Email LIKE @Dominio
            AND NOT EXISTS (SELECT 1 FROM dbo.Cliente AS c WHERE c.UtilizadorId = u.Id));
INSERT INTO @R VALUES (N'D06', N'Contas de Cliente sem linha em Cliente', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

/* Critério 4 — formato do hash (PasswordHasher v3: 0x01, HMACSHA512, 100 000 iterações) */
SET @V = (SELECT COUNT(*) FROM dbo.Utilizador WHERE Email LIKE @Dominio AND PasswordHash NOT LIKE N'AQAAAAIAAYag%');
INSERT INTO @R VALUES (N'D07', N'Contas de demonstração com hash fora do formato PasswordHasher v3', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

/* Critério 3 — dados de demonstração */
SET @V = (SELECT COUNT(*) FROM (SELECT p.ClienteId FROM dbo.Paciente AS p GROUP BY p.ClienteId HAVING COUNT(*) > 1) AS x);
INSERT INTO @R VALUES (N'D08', N'Clientes com mais de um animal', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

INSERT INTO @R (Codigo, Verificacao, Esperado, Obtido, Resultado)
SELECT N'D09', N'Marcações no estado ' + e.Estado, N'>= 1', CAST(COUNT(m.Id) AS NVARCHAR(10)),
       CASE WHEN COUNT(m.Id) >= 1 THEN N'OK' ELSE N'FALHOU' END
FROM (VALUES (N'Pendente'), (N'Confirmada'), (N'Realizada'), (N'Cancelada')) AS e (Estado)
LEFT JOIN dbo.Marcacao AS m ON m.Estado = e.Estado
GROUP BY e.Estado;

SET @V = (SELECT COUNT(*) FROM dbo.RegistoClinico AS r
          WHERE EXISTS (SELECT 1 FROM dbo.Tratamento AS t WHERE t.RegistoClinicoId = r.Id)
            AND EXISTS (SELECT 1 FROM dbo.Prescricao AS p WHERE p.RegistoClinicoId = r.Id));
INSERT INTO @R VALUES (N'D10', N'Registo clínico com tratamento e prescrição', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM (SELECT PacienteId FROM dbo.RegistoProfilaxia GROUP BY PacienteId
          HAVING SUM(CASE WHEN Tipo = N'Vacina' THEN 1 ELSE 0 END) >= 1 AND SUM(CASE WHEN Tipo = N'Desparasitacao' THEN 1 ELSE 0 END) >= 1) AS x);
INSERT INTO @R VALUES (N'D11', N'Paciente com boletim de vacinação e de desparasitação', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.Marcacao WHERE DataHoraChegada IS NOT NULL);
INSERT INTO @R VALUES (N'D12', N'Marcação com registo de chegada', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.InformacaoApoio);
INSERT INTO @R VALUES (N'D13', N'Informação de apoio registada', N'>= 1', CAST(@V AS NVARCHAR(10)), CASE WHEN @V >= 1 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(DISTINCT DiaSemana) FROM dbo.HorarioFuncionamento WHERE Ativo = 1);
INSERT INTO @R VALUES (N'D14', N'Dias com horário de funcionamento por omissão', N'6', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 6 THEN N'OK' ELSE N'FALHOU' END);

/* Coerência referencial e de negócio (verificação 2 do Revisor) */
SET @V = (SELECT COUNT(*) FROM dbo.RegistoClinico AS r JOIN dbo.Marcacao AS m ON m.Id = r.MarcacaoId WHERE m.Estado <> N'Realizada');
INSERT INTO @R VALUES (N'D15', N'Registos clínicos em consultas não realizadas', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.RegistoClinico AS r JOIN dbo.Marcacao AS m ON m.Id = r.MarcacaoId WHERE r.VeterinarioId <> m.VeterinarioId);
INSERT INTO @R VALUES (N'D16', N'Registos clínicos de outro Veterinário que não o da consulta', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.RegistoProfilaxia AS p JOIN dbo.Marcacao AS m ON m.Id = p.MarcacaoId WHERE p.PacienteId <> m.PacienteId);
INSERT INTO @R VALUES (N'D17', N'Profilaxias ligadas a consulta de outro paciente', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.Marcacao AS a JOIN dbo.Marcacao AS b
          ON a.VeterinarioId = b.VeterinarioId AND a.Id < b.Id
         AND a.Estado <> N'Cancelada' AND b.Estado <> N'Cancelada'
         AND a.DataHoraInicio < DATEADD(MINUTE, b.DuracaoMinutos, b.DataHoraInicio)
         AND b.DataHoraInicio < DATEADD(MINUTE, a.DuracaoMinutos, a.DataHoraInicio));
INSERT INTO @R VALUES (N'D18', N'Marcações sobrepostas do mesmo Veterinário (RF021)', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.Marcacao AS m
          JOIN dbo.Utilizador AS u ON u.Id = m.VeterinarioId
          WHERE u.Perfil <> N'Veterinario');
INSERT INTO @R VALUES (N'D19', N'Marcações atribuídas a quem não é Veterinário', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

SET @V = (SELECT COUNT(*) FROM dbo.InformacaoApoio AS i JOIN dbo.Funcionario AS f ON f.UtilizadorId = i.AuxiliarId
          JOIN dbo.Utilizador AS u ON u.Id = f.UtilizadorId
          WHERE u.Perfil <> N'AuxiliarVeterinario' OR f.PodeRegistarApoio = 0);
INSERT INTO @R VALUES (N'D20', N'Informação de apoio registada por quem não tem autorização', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

/* Verificação 3 do Revisor — sem dados pessoais reais */
SET @V = (SELECT COUNT(*) FROM dbo.Utilizador WHERE Email NOT LIKE @Dominio);
INSERT INTO @R VALUES (N'D21', N'Contas fora do domínio reservado .test (demonstração)', N'0', CAST(@V AS NVARCHAR(10)), CASE WHEN @V = 0 THEN N'OK' ELSE N'FALHOU' END);

/* Resultado */
DECLARE @Total INT = (SELECT COUNT(*) FROM @R);
DECLARE @Falhas INT = (SELECT COUNT(*) FROM @R WHERE Resultado <> N'OK');

SELECT Codigo AS [Código], Verificacao AS [Verificação], Esperado, Obtido, Resultado FROM @R ORDER BY Ordem;

SELECT N'Utilizador' AS Tabela, COUNT(*) AS Linhas FROM dbo.Utilizador UNION ALL
SELECT N'Funcionario', COUNT(*) FROM dbo.Funcionario UNION ALL
SELECT N'Cliente', COUNT(*) FROM dbo.Cliente UNION ALL
SELECT N'Paciente', COUNT(*) FROM dbo.Paciente UNION ALL
SELECT N'HorarioFuncionamento', COUNT(*) FROM dbo.HorarioFuncionamento UNION ALL
SELECT N'Marcacao', COUNT(*) FROM dbo.Marcacao UNION ALL
SELECT N'InformacaoApoio', COUNT(*) FROM dbo.InformacaoApoio UNION ALL
SELECT N'RegistoClinico', COUNT(*) FROM dbo.RegistoClinico UNION ALL
SELECT N'Tratamento', COUNT(*) FROM dbo.Tratamento UNION ALL
SELECT N'Prescricao', COUNT(*) FROM dbo.Prescricao UNION ALL
SELECT N'RegistoProfilaxia', COUNT(*) FROM dbo.RegistoProfilaxia;

PRINT CONCAT(N'Verificação dos dados de demonstração: ', @Total, N' verificações, ', @Total - @Falhas, N' OK, ', @Falhas, N' falhadas.');
IF @Falhas > 0
    RAISERROR(N'%d verificação(ões) dos dados de demonstração falharam. Ver a coluna Obtido.', 16, 1, @Falhas);
ELSE
    PRINT N'Resultado: os dados de demonstração cobrem os critérios 2 e 3 da I061 e são coerentes.';
GO

/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I061 — Escrever os scripts de dados iniciais e de demonstração
   Ficheiro       : dados-demonstracao/02-dados-demonstracao.sql
   Versão         : m3-v02
   Finalidade     : Inserir os dados iniciais e de demonstração necessários ao
                    funcionamento e à avaliação do sistema, coerentes entre si:
                    - contas de teste por perfil, incluindo a conta inicial do
                      Administrador, uma conta bloqueada e dois Auxiliares
                      Veterinários (com e sem autorização de registo de apoio);
                    - horário de funcionamento por omissão da clínica;
                    - clientes (um deles com mais de um animal) e animais;
                    - marcações nos quatro estados, com registo de chegada e
                      informação de apoio;
                    - um registo clínico com tratamento e prescrição;
                    - boletim de vacinação e desparasitação.
   Pré-requisitos : Esquema m3-v02 e dados de referência (Clinica, Especie, Raca e
                    ConfiguracaoSistema), criados pelo HasData das migrações da
                    I062 ou, até lá, por 01-dados-referencia-provisorios.sql.
                    Modelo de Dados v02, ponto 9.3 (opção a).
   Ordem          : 2 de 2.
   Reexecução     : Idempotente. Cada linha só é inserida se ainda não existir,
                    procurada pela chave natural (e-mail do utilizador, conta do
                    Cliente, Cliente + nome do animal, Veterinário + data e hora
                    da marcação, consulta do registo clínico, etc.). Uma segunda
                    execução regista «0 linha(s) inserida(s)» em todas as tabelas.
   Credenciais    : As palavras-passe NÃO constam deste ficheiro. Só existem os
                    hashes no formato do PasswordHasher do ASP.NET Core Identity
                    (v3: PBKDF2-HMAC-SHA512, 100 000 iterações, sal de 128 bits),
                    como define o Modelo de Dados v02 (7.1). As palavras-passe das
                    contas temporárias de avaliação estão em
                    /projeto/00-indice-e-submissao/m3-credenciais-avaliacao-v01.md.
   Dados          : Fictícios. E-mails no domínio reservado .test (RFC 2606),
                    NIF e telefones fictícios; nenhum dado pessoal real.
   Datas          : Fixas e em UTC, para que a reexecução nunca duplique dados.
                    Marcações passadas (outubro de 2026) e futuras (novembro de
                    2026), dentro do horário de funcionamento.
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

USE ClinicaVeterinaria;
GO

/* --- Pré-condição: dados de referência (I062 / HasData ou script 01) ------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Clinica WHERE Id = 1)
   OR NOT EXISTS (SELECT 1 FROM dbo.Especie WHERE Nome = N'Cão')
   OR NOT EXISTS (SELECT 1 FROM dbo.Especie WHERE Nome = N'Gato')
BEGIN
    RAISERROR(N'[02] Faltam dados de referência (Clinica ou as espécies Cão e Gato). Aplicar as migrações da I062 ou executar 01-dados-referencia-provisorios.sql antes deste script.', 16, 1);
    RETURN;
END;

DECLARE @N INT;

BEGIN TRANSACTION;

/* --- 1. Utilizadores (contas de teste por perfil) --------------------------- */
DECLARE @Utilizadores TABLE (Email NVARCHAR(256) NOT NULL, PasswordHash NVARCHAR(256) NOT NULL, Perfil NVARCHAR(25) NOT NULL, Estado NVARCHAR(10) NOT NULL);
INSERT INTO @Utilizadores (Email, PasswordHash, Perfil, Estado) VALUES
    (N'admin@clinica-g08.test', N'AQAAAAIAAYagAAAAEBfeSvy2D6XizwBrH4sDQCrKsBDCmYjJycD2PpxGWTrp8kdajPL0nijNVTsrFxKNdg==', N'Administrador', N'Ativo'),
    (N'rececao@clinica-g08.test', N'AQAAAAIAAYagAAAAEGThZkIWQdLB3uqanBT2B/reGl2Xw2iRSScQ4qVi66/rdMlMJxLqu2KJ9TCXs7oEgw==', N'Rececionista', N'Ativo'),
    (N'rececao.bloqueada@clinica-g08.test', N'AQAAAAIAAYagAAAAEIbVw3jBORVi9dtjVCtMVCTUksvsk0qfSlfceEnisQSUi3p2VEBKjQ4JsBysuSoqtA==', N'Rececionista', N'Bloqueado'),
    (N'vet.ana@clinica-g08.test', N'AQAAAAIAAYagAAAAELFSKDMXKig8adF88cGJ0c50TR1TWDFHczluQRKP0wGo4bdzY0SPBh1gQO8C6EYZkg==', N'Veterinario', N'Ativo'),
    (N'vet.bruno@clinica-g08.test', N'AQAAAAIAAYagAAAAEOzgYVqrcWAcV3fgyMF9lgY9YH1S9sllWk9NThxe1zN3Oqm4JncK9tkVEqpKZYXhFA==', N'Veterinario', N'Ativo'),
    (N'aux.carla@clinica-g08.test', N'AQAAAAIAAYagAAAAEMx5iSzsBaEFfS2wcKpo1UY2mzRAOzFCfl6vZU5P9AY8G5gNGCXW0SS22Ry7CCm/Aw==', N'AuxiliarVeterinario', N'Ativo'),
    (N'aux.diogo@clinica-g08.test', N'AQAAAAIAAYagAAAAECTwTucAYcQKXYc428lBBRYoC6cMLRK+xBKbzw7DwU9uEJxOI66JL+B1XqieXZpfhA==', N'AuxiliarVeterinario', N'Ativo'),
    (N'cliente.joao@clinica-g08.test', N'AQAAAAIAAYagAAAAEHVPBt3XElAZUzjzf+xhJX7sf9/ao/Zq7PArAxEXJ8HX0C+LxyAU23Cm2mgcKKIK9Q==', N'Cliente', N'Ativo'),
    (N'cliente.maria@clinica-g08.test', N'AQAAAAIAAYagAAAAEJv4q0p9jMIXe2YWX7diBowk+CTdeumpPQIXal2rV02tpYuzQMGXRWTYqwQtc0AgBg==', N'Cliente', N'Ativo'),
    (N'cliente.rui@clinica-g08.test', N'AQAAAAIAAYagAAAAEMxhAOeNm4BCV7sFQ/oI2hRiag2Yupw1QPcJPyGv26U69h9XMKoTC+Iax+RGuyoPRw==', N'Cliente', N'Ativo');

INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil, Estado)
SELECT v.Email, v.PasswordHash, v.Perfil, v.Estado
FROM @Utilizadores AS v
WHERE NOT EXISTS (SELECT 1 FROM dbo.Utilizador AS u WHERE u.Email = v.Email);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] Utilizador: ', @N, N' linha(s) inserida(s).');

/* --- 2. Funcionários (perfis internos) ------------------------------------- */
INSERT INTO dbo.Funcionario (UtilizadorId, Nome, Telefone, PodeRegistarApoio)
SELECT u.Id, v.Nome, v.Telefone, v.PodeRegistarApoio
FROM (VALUES
    (N'admin@clinica-g08.test',             N'Administração (demonstração)',       N'910000001', CAST(0 AS BIT)),
    (N'rececao@clinica-g08.test',           N'Rita Rececionista (demonstração)',   N'910000002', CAST(0 AS BIT)),
    (N'rececao.bloqueada@clinica-g08.test', N'Rui Bloqueado (demonstração)',       N'910000003', CAST(0 AS BIT)),
    (N'vet.ana@clinica-g08.test',           N'Dra. Ana Veterinária (demonstração)', N'910000004', CAST(0 AS BIT)),
    (N'vet.bruno@clinica-g08.test',         N'Dr. Bruno Veterinário (demonstração)', N'910000005', CAST(0 AS BIT)),
    (N'aux.carla@clinica-g08.test',         N'Carla Auxiliar (com apoio)',         N'910000006', CAST(1 AS BIT)),
    (N'aux.diogo@clinica-g08.test',         N'Diogo Auxiliar (sem apoio)',         N'910000007', CAST(0 AS BIT))
) AS v (Email, Nome, Telefone, PodeRegistarApoio)
JOIN dbo.Utilizador AS u ON u.Email = v.Email
WHERE NOT EXISTS (SELECT 1 FROM dbo.Funcionario AS f WHERE f.UtilizadorId = u.Id);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] Funcionario: ', @N, N' linha(s) inserida(s).');

/* --- 3. Horário de funcionamento por omissão (1 = segunda-feira) ------------ */
INSERT INTO dbo.HorarioFuncionamento (DiaSemana, HoraAbertura, HoraFecho)
SELECT v.DiaSemana, v.HoraAbertura, v.HoraFecho
FROM (VALUES
    (CAST(1 AS TINYINT), CAST('09:00' AS TIME(0)), CAST('13:00' AS TIME(0))), (CAST(1 AS TINYINT), CAST('14:00' AS TIME(0)), CAST('19:00' AS TIME(0))),
    (CAST(2 AS TINYINT), CAST('09:00' AS TIME(0)), CAST('13:00' AS TIME(0))), (CAST(2 AS TINYINT), CAST('14:00' AS TIME(0)), CAST('19:00' AS TIME(0))),
    (CAST(3 AS TINYINT), CAST('09:00' AS TIME(0)), CAST('13:00' AS TIME(0))), (CAST(3 AS TINYINT), CAST('14:00' AS TIME(0)), CAST('19:00' AS TIME(0))),
    (CAST(4 AS TINYINT), CAST('09:00' AS TIME(0)), CAST('13:00' AS TIME(0))), (CAST(4 AS TINYINT), CAST('14:00' AS TIME(0)), CAST('19:00' AS TIME(0))),
    (CAST(5 AS TINYINT), CAST('09:00' AS TIME(0)), CAST('13:00' AS TIME(0))), (CAST(5 AS TINYINT), CAST('14:00' AS TIME(0)), CAST('19:00' AS TIME(0))),
    (CAST(6 AS TINYINT), CAST('09:00' AS TIME(0)), CAST('13:00' AS TIME(0)))
) AS v (DiaSemana, HoraAbertura, HoraFecho)
WHERE NOT EXISTS (SELECT 1 FROM dbo.HorarioFuncionamento AS h WHERE h.DiaSemana = v.DiaSemana AND h.HoraAbertura = v.HoraAbertura);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] HorarioFuncionamento: ', @N, N' linha(s) inserida(s).');

/* --- 4. Clientes ------------------------------------------------------------ */
INSERT INTO dbo.Cliente (UtilizadorId, Nome, NIF, Telefone, Morada)
SELECT u.Id, v.Nome, v.NIF, v.Telefone, v.Morada
FROM (VALUES
    (N'cliente.joao@clinica-g08.test',  N'João Costa (demonstração)',  '210000015', N'960000001', N'Rua Fictícia, 1, 4470-001 Maia'),
    (N'cliente.maria@clinica-g08.test', N'Maria Silva (demonstração)', '210000023', N'960000002', N'Rua Fictícia, 2, 4470-002 Maia'),
    (N'cliente.rui@clinica-g08.test',   N'Rui Santos (demonstração)',  NULL,        N'960000003', NULL)
) AS v (Email, Nome, NIF, Telefone, Morada)
JOIN dbo.Utilizador AS u ON u.Email = v.Email
WHERE NOT EXISTS (SELECT 1 FROM dbo.Cliente AS c WHERE c.UtilizadorId = u.Id);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] Cliente: ', @N, N' linha(s) inserida(s).');

/* --- 5. Pacientes (o Cliente João tem dois animais) -------------------------- */
INSERT INTO dbo.Paciente (ClienteId, Nome, EspecieId, RacaId, Sexo, DataNascimento, Peso)
SELECT c.Id, v.Nome, e.Id, r.Id, v.Sexo, v.DataNascimento, v.Peso
FROM (VALUES
    (N'cliente.joao@clinica-g08.test',  N'Bobi', N'Cão',  N'Labrador Retriever', N'Macho', CAST('2022-03-10' AS DATE), CAST(28.40 AS DECIMAL(6,2))),
    (N'cliente.joao@clinica-g08.test',  N'Mia',  N'Gato', N'Europeu Comum',      N'Femea', CAST('2023-06-01' AS DATE), CAST(4.20 AS DECIMAL(6,2))),
    (N'cliente.maria@clinica-g08.test', N'Kiko', N'Cão',  N'Sem raça definida',  N'Macho', CAST('2021-11-20' AS DATE), CAST(12.80 AS DECIMAL(6,2))),
    (N'cliente.rui@clinica-g08.test',   N'Luna', N'Gato', N'Siamês',             N'Femea', NULL,                       NULL)
) AS v (ClienteEmail, Nome, Especie, Raca, Sexo, DataNascimento, Peso)
JOIN dbo.Utilizador AS u ON u.Email = v.ClienteEmail
JOIN dbo.Cliente    AS c ON c.UtilizadorId = u.Id
JOIN dbo.Especie    AS e ON e.Nome = v.Especie
LEFT JOIN dbo.Raca  AS r ON r.EspecieId = e.Id AND r.Nome = v.Raca   -- raça em falta no HasData: o animal fica sem raça (RacaId NULL)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Paciente AS p WHERE p.ClienteId = c.Id AND p.Nome = v.Nome);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] Paciente: ', @N, N' linha(s) inserida(s).');

/* --- 6. Marcações nos quatro estados (datas e horas em UTC) ----------------- */
DECLARE @Marcacoes TABLE
(
    Ref NVARCHAR(3) NOT NULL, ClienteEmail NVARCHAR(256) NOT NULL, Paciente NVARCHAR(80) NOT NULL,
    VeterinarioEmail NVARCHAR(256) NOT NULL, DataHoraInicio DATETIME2(0) NOT NULL, DuracaoMinutos SMALLINT NOT NULL,
    Motivo NVARCHAR(250) NULL, Estado NVARCHAR(12) NOT NULL, DataHoraChegada DATETIME2(0) NULL,
    CriadaPorEmail NVARCHAR(256) NOT NULL, DataCriacao DATETIME2(0) NOT NULL, DataUltimaAlteracao DATETIME2(0) NULL
);
INSERT INTO @Marcacoes VALUES
    (N'M1', N'cliente.joao@clinica-g08.test',  N'Bobi', N'vet.ana@clinica-g08.test',   '2026-10-14T09:30:00', 30, N'Comichão no ouvido esquerdo',  N'Realizada',  '2026-10-14T09:22:00', N'rececao@clinica-g08.test', '2026-10-10T15:00:00', '2026-10-14T10:05:00'),
    (N'M2', N'cliente.joao@clinica-g08.test',  N'Mia',  N'vet.bruno@clinica-g08.test', '2026-10-20T10:00:00', 30, N'Consulta de rotina',           N'Cancelada',  NULL,                  N'rececao@clinica-g08.test', '2026-10-15T11:00:00', '2026-10-19T09:00:00'),
    (N'M3', N'cliente.maria@clinica-g08.test', N'Kiko', N'vet.ana@clinica-g08.test',   '2026-11-03T09:00:00', 30, N'Vacinação anual',              N'Confirmada', NULL,                  N'rececao@clinica-g08.test', '2026-10-24T10:00:00', '2026-10-24T10:30:00'),
    (N'M4', N'cliente.rui@clinica-g08.test',   N'Luna', N'vet.bruno@clinica-g08.test', '2026-11-04T14:30:00', 30, N'Primeira consulta',            N'Pendente',   NULL,                  N'rececao@clinica-g08.test', '2026-10-25T09:00:00', NULL),
    (N'M5', N'cliente.joao@clinica-g08.test',  N'Bobi', N'vet.ana@clinica-g08.test',   '2026-11-05T11:00:00', 30, N'Reavaliação do ouvido',        N'Pendente',   NULL,                  N'rececao@clinica-g08.test', '2026-10-25T09:15:00', NULL);

DECLARE @MarcacoesResolvidas TABLE (Ref NVARCHAR(3) NOT NULL PRIMARY KEY, PacienteId INT NOT NULL, VeterinarioId INT NOT NULL, CriadaPorId INT NOT NULL, DataHoraInicio DATETIME2(0) NOT NULL);
INSERT INTO @MarcacoesResolvidas (Ref, PacienteId, VeterinarioId, CriadaPorId, DataHoraInicio)
SELECT m.Ref, p.Id, uv.Id, ur.Id, m.DataHoraInicio
FROM @Marcacoes AS m
JOIN dbo.Utilizador AS uc ON uc.Email = m.ClienteEmail
JOIN dbo.Cliente    AS c  ON c.UtilizadorId = uc.Id
JOIN dbo.Paciente   AS p  ON p.ClienteId = c.Id AND p.Nome = m.Paciente
JOIN dbo.Utilizador AS uv ON uv.Email = m.VeterinarioEmail
JOIN dbo.Utilizador AS ur ON ur.Email = m.CriadaPorEmail;

IF (SELECT COUNT(*) FROM @MarcacoesResolvidas) <> (SELECT COUNT(*) FROM @Marcacoes)
    THROW 50002, N'[02] Não foi possível resolver todas as marcações (paciente, Veterinário ou Rececionista em falta).', 1;

INSERT INTO dbo.Marcacao (PacienteId, VeterinarioId, DataHoraInicio, DuracaoMinutos, Motivo, Estado, DataHoraChegada, CriadaPorId, DataCriacao, DataUltimaAlteracao)
SELECT r.PacienteId, r.VeterinarioId, m.DataHoraInicio, m.DuracaoMinutos, m.Motivo, m.Estado, m.DataHoraChegada, r.CriadaPorId, m.DataCriacao, m.DataUltimaAlteracao
FROM @Marcacoes AS m
JOIN @MarcacoesResolvidas AS r ON r.Ref = m.Ref
WHERE NOT EXISTS (SELECT 1 FROM dbo.Marcacao AS x
                  WHERE x.PacienteId = r.PacienteId AND x.VeterinarioId = r.VeterinarioId AND x.DataHoraInicio = r.DataHoraInicio);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] Marcacao: ', @N, N' linha(s) inserida(s).');

DECLARE @M1 INT = (SELECT x.Id FROM dbo.Marcacao AS x JOIN @MarcacoesResolvidas AS r
                   ON x.PacienteId = r.PacienteId AND x.VeterinarioId = r.VeterinarioId AND x.DataHoraInicio = r.DataHoraInicio
                   WHERE r.Ref = N'M1');
DECLARE @VetAna INT = (SELECT Id FROM dbo.Utilizador WHERE Email = N'vet.ana@clinica-g08.test');
DECLARE @AuxCarla INT = (SELECT Id FROM dbo.Utilizador WHERE Email = N'aux.carla@clinica-g08.test');
DECLARE @Bobi INT = (SELECT p.Id FROM dbo.Paciente AS p JOIN dbo.Cliente AS c ON c.Id = p.ClienteId
                     JOIN dbo.Utilizador AS u ON u.Id = c.UtilizadorId WHERE u.Email = N'cliente.joao@clinica-g08.test' AND p.Nome = N'Bobi');
DECLARE @Kiko INT = (SELECT p.Id FROM dbo.Paciente AS p JOIN dbo.Cliente AS c ON c.Id = p.ClienteId
                     JOIN dbo.Utilizador AS u ON u.Id = c.UtilizadorId WHERE u.Email = N'cliente.maria@clinica-g08.test' AND p.Nome = N'Kiko');

/* --- 7. Informação de apoio (Auxiliar com autorização, RF027) --------------- */
INSERT INTO dbo.InformacaoApoio (MarcacaoId, AuxiliarId, Descricao, DataRegisto)
SELECT v.MarcacaoId, @AuxCarla, v.Descricao, v.DataRegisto
FROM (VALUES
    (@M1, N'Sala 2 preparada; tutor refere que o animal coça o ouvido há uma semana.', CAST('2026-10-14T09:25:00' AS DATETIME2(0)))
) AS v (MarcacaoId, Descricao, DataRegisto)
WHERE NOT EXISTS (SELECT 1 FROM dbo.InformacaoApoio AS i WHERE i.MarcacaoId = v.MarcacaoId AND i.Descricao = v.Descricao);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] InformacaoApoio: ', @N, N' linha(s) inserida(s).');

/* --- 8. Registo clínico da consulta realizada (M1), com tratamento e prescrição */
INSERT INTO dbo.RegistoClinico (MarcacaoId, VeterinarioId, NotasClinicas, Diagnostico, CriadoEm)
SELECT @M1, @VetAna,
       N'Canal auditivo esquerdo eritematoso, com cerúmen escuro. Sem alterações no ouvido direito.',
       N'Otite externa', CAST('2026-10-14T10:00:00' AS DATETIME2(0))
WHERE NOT EXISTS (SELECT 1 FROM dbo.RegistoClinico WHERE MarcacaoId = @M1);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] RegistoClinico: ', @N, N' linha(s) inserida(s).');

DECLARE @Reg1 INT = (SELECT Id FROM dbo.RegistoClinico WHERE MarcacaoId = @M1);

INSERT INTO dbo.Tratamento (RegistoClinicoId, Descricao, DataAplicacao)
SELECT @Reg1, N'Limpeza auricular com solução fisiológica', CAST('2026-10-14' AS DATE)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Tratamento WHERE RegistoClinicoId = @Reg1 AND Descricao = N'Limpeza auricular com solução fisiológica');
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] Tratamento: ', @N, N' linha(s) inserida(s).');

INSERT INTO dbo.Prescricao (RegistoClinicoId, Medicamento, Dosagem, Frequencia, DuracaoDias)
SELECT @Reg1, N'Gotas auriculares (fictício)', N'3 gotas no ouvido esquerdo', N'2 vezes por dia', 7
WHERE NOT EXISTS (SELECT 1 FROM dbo.Prescricao WHERE RegistoClinicoId = @Reg1 AND Medicamento = N'Gotas auriculares (fictício)');
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] Prescricao: ', @N, N' linha(s) inserida(s).');

/* --- 9. Boletim de vacinação e desparasitação (RF043 a RF047) --------------- */
INSERT INTO dbo.RegistoProfilaxia (PacienteId, VeterinarioId, MarcacaoId, Tipo, Produto, Lote, DataAdministracao, IntervaloDias, DataRegisto)
SELECT v.PacienteId, @VetAna, v.MarcacaoId, v.Tipo, v.Produto, v.Lote, v.DataAdministracao, v.IntervaloDias, v.DataRegisto
FROM (VALUES
    (@Bobi, CAST(NULL AS INT), N'Vacina',         N'Vacina antirrábica (fictícia)',     N'RB-2291', CAST('2025-10-14' AS DATE), CAST(365 AS SMALLINT), CAST('2025-10-14T10:00:00' AS DATETIME2(0))),
    (@Bobi, @M1,               N'Vacina',         N'Vacina polivalente (fictícia)',     N'PV-1187', CAST('2026-10-14' AS DATE), CAST(365 AS SMALLINT), CAST('2026-10-14T10:10:00' AS DATETIME2(0))),
    (@Bobi, @M1,               N'Desparasitacao', N'Desparasitante interno (fictício)', N'DP-0042', CAST('2026-10-14' AS DATE), CAST(90 AS SMALLINT),  CAST('2026-10-14T10:12:00' AS DATETIME2(0))),
    (@Kiko, CAST(NULL AS INT), N'Desparasitacao', N'Desparasitante externo (fictício)', N'DE-0310', CAST('2026-08-01' AS DATE), NULL,                  CAST('2026-08-01T15:00:00' AS DATETIME2(0)))
) AS v (PacienteId, MarcacaoId, Tipo, Produto, Lote, DataAdministracao, IntervaloDias, DataRegisto)
WHERE NOT EXISTS (SELECT 1 FROM dbo.RegistoProfilaxia AS x
                  WHERE x.PacienteId = v.PacienteId AND x.Tipo = v.Tipo AND x.Produto = v.Produto AND x.DataAdministracao = v.DataAdministracao);
SET @N = @@ROWCOUNT;
PRINT CONCAT(N'[02] RegistoProfilaxia: ', @N, N' linha(s) inserida(s).');

COMMIT TRANSACTION;
PRINT N'[02] Dados de demonstração concluídos.';
GO

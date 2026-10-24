/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : por atribuir (a versão m3-v01 destes scripts é a I060, concluída)
   Ficheiro       : verificacao/verificar-esquema.sql
   Versão         : m3-v02
   Finalidade     : Confirmar, depois de executar os scripts 00 a 06, que a base de
                    dados ClinicaVeterinaria contém exatamente o esquema previsto:
                    16 tabelas, 99 colunas (nome, ordem, tipo, nulidade e coluna
                    calculada persistida), 68 restrições (PK, FK, UNIQUE, CHECK e
                    DEFAULT), 19 índices, todas as FKs com NO ACTION e nenhuma
                    restrição desativada ou não fidedigna.
   Origem         : A lista de colunas foi extraída do dicionário de dados do Modelo
                    de Dados v02 (rascunho, secção 7), e não dos scripts de criação, para
                    que a verificação compare a base de dados com o modelo (ainda em rascunho).
   Execução       : Depois de 06-indices.sql. Só lê o catálogo do SQL Server; não
                    cria, altera nem apaga objetos ou dados.
   Resultado      : Resumo por categoria e lista das divergências. Termina com erro
                    (severidade 16) se existir alguma divergência.
   ============================================================================= */

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

USE ClinicaVeterinaria;
GO

DECLARE @MostrarTudo BIT = 0;  -- 1 = listar também as verificações com resultado OK

DECLARE @ColunasEsperadas TABLE
(
    Tabela    NVARCHAR(128) COLLATE DATABASE_DEFAULT NOT NULL,
    Coluna    NVARCHAR(128) COLLATE DATABASE_DEFAULT NOT NULL,
    Ordem     INT           NOT NULL,
    Tipo      NVARCHAR(50)  COLLATE DATABASE_DEFAULT NOT NULL,
    Nulo      BIT           NOT NULL,
    Calculada BIT           NOT NULL
);
INSERT INTO @ColunasEsperadas (Tabela, Coluna, Ordem, Tipo, Nulo, Calculada) VALUES
    (N'Utilizador', N'Id', 1, N'INT', 0, 0),
    (N'Utilizador', N'Email', 2, N'NVARCHAR(256)', 0, 0),
    (N'Utilizador', N'PasswordHash', 3, N'NVARCHAR(256)', 0, 0),
    (N'Utilizador', N'Perfil', 4, N'NVARCHAR(25)', 0, 0),
    (N'Utilizador', N'Estado', 5, N'NVARCHAR(10)', 0, 0),
    (N'Utilizador', N'DataCriacao', 6, N'DATETIME2(0)', 0, 0),
    (N'Utilizador', N'DataUltimoAcesso', 7, N'DATETIME2(0)', 1, 0),
    (N'TokenRevogado', N'Jti', 1, N'NVARCHAR(64)', 0, 0),
    (N'TokenRevogado', N'UtilizadorId', 2, N'INT', 0, 0),
    (N'TokenRevogado', N'DataExpiracao', 3, N'DATETIME2(0)', 0, 0),
    (N'TokenRevogado', N'DataRevogacao', 4, N'DATETIME2(0)', 0, 0),
    (N'Funcionario', N'UtilizadorId', 1, N'INT', 0, 0),
    (N'Funcionario', N'Nome', 2, N'NVARCHAR(150)', 0, 0),
    (N'Funcionario', N'Telefone', 3, N'NVARCHAR(20)', 1, 0),
    (N'Funcionario', N'PodeRegistarApoio', 4, N'BIT', 0, 0),
    (N'Clinica', N'Id', 1, N'TINYINT', 0, 0),
    (N'Clinica', N'Nome', 2, N'NVARCHAR(150)', 0, 0),
    (N'Clinica', N'NIF', 3, N'CHAR(9)', 0, 0),
    (N'Clinica', N'Morada', 4, N'NVARCHAR(250)', 0, 0),
    (N'Clinica', N'Telefone', 5, N'NVARCHAR(20)', 0, 0),
    (N'Clinica', N'Email', 6, N'NVARCHAR(256)', 0, 0),
    (N'Clinica', N'DataUltimaAlteracao', 7, N'DATETIME2(0)', 0, 0),
    (N'ConfiguracaoSistema', N'Chave', 1, N'NVARCHAR(100)', 0, 0),
    (N'ConfiguracaoSistema', N'Valor', 2, N'NVARCHAR(500)', 0, 0),
    (N'ConfiguracaoSistema', N'Descricao', 3, N'NVARCHAR(250)', 1, 0),
    (N'ConfiguracaoSistema', N'DataUltimaAlteracao', 4, N'DATETIME2(0)', 0, 0),
    (N'Cliente', N'Id', 1, N'INT', 0, 0),
    (N'Cliente', N'UtilizadorId', 2, N'INT', 0, 0),
    (N'Cliente', N'Nome', 3, N'NVARCHAR(150)', 0, 0),
    (N'Cliente', N'NIF', 4, N'CHAR(9)', 1, 0),
    (N'Cliente', N'Telefone', 5, N'NVARCHAR(20)', 0, 0),
    (N'Cliente', N'Morada', 6, N'NVARCHAR(250)', 1, 0),
    (N'Cliente', N'DataRegisto', 7, N'DATETIME2(0)', 0, 0),
    (N'Especie', N'Id', 1, N'INT', 0, 0),
    (N'Especie', N'Nome', 2, N'NVARCHAR(50)', 0, 0),
    (N'Raca', N'Id', 1, N'INT', 0, 0),
    (N'Raca', N'EspecieId', 2, N'INT', 0, 0),
    (N'Raca', N'Nome', 3, N'NVARCHAR(80)', 0, 0),
    (N'Paciente', N'Id', 1, N'INT', 0, 0),
    (N'Paciente', N'ClienteId', 2, N'INT', 0, 0),
    (N'Paciente', N'Nome', 3, N'NVARCHAR(80)', 0, 0),
    (N'Paciente', N'EspecieId', 4, N'INT', 0, 0),
    (N'Paciente', N'RacaId', 5, N'INT', 1, 0),
    (N'Paciente', N'Sexo', 6, N'NVARCHAR(12)', 0, 0),
    (N'Paciente', N'DataNascimento', 7, N'DATE', 1, 0),
    (N'Paciente', N'Peso', 8, N'DECIMAL(6,2)', 1, 0),
    (N'Paciente', N'DataRegisto', 9, N'DATETIME2(0)', 0, 0),
    (N'Paciente', N'DataUltimaAlteracao', 10, N'DATETIME2(0)', 1, 0),
    (N'HorarioFuncionamento', N'Id', 1, N'INT', 0, 0),
    (N'HorarioFuncionamento', N'DiaSemana', 2, N'TINYINT', 0, 0),
    (N'HorarioFuncionamento', N'HoraAbertura', 3, N'TIME(0)', 0, 0),
    (N'HorarioFuncionamento', N'HoraFecho', 4, N'TIME(0)', 0, 0),
    (N'HorarioFuncionamento', N'Ativo', 5, N'BIT', 0, 0),
    (N'Marcacao', N'Id', 1, N'INT', 0, 0),
    (N'Marcacao', N'PacienteId', 2, N'INT', 0, 0),
    (N'Marcacao', N'VeterinarioId', 3, N'INT', 0, 0),
    (N'Marcacao', N'DataHoraInicio', 4, N'DATETIME2(0)', 0, 0),
    (N'Marcacao', N'DuracaoMinutos', 5, N'SMALLINT', 0, 0),
    (N'Marcacao', N'Motivo', 6, N'NVARCHAR(250)', 1, 0),
    (N'Marcacao', N'Estado', 7, N'NVARCHAR(12)', 0, 0),
    (N'Marcacao', N'DataHoraChegada', 8, N'DATETIME2(0)', 1, 0),
    (N'Marcacao', N'CriadaPorId', 9, N'INT', 0, 0),
    (N'Marcacao', N'DataCriacao', 10, N'DATETIME2(0)', 0, 0),
    (N'Marcacao', N'DataUltimaAlteracao', 11, N'DATETIME2(0)', 1, 0),
    (N'Marcacao', N'RowVersion', 12, N'TIMESTAMP', 0, 0),
    (N'InformacaoApoio', N'Id', 1, N'INT', 0, 0),
    (N'InformacaoApoio', N'MarcacaoId', 2, N'INT', 0, 0),
    (N'InformacaoApoio', N'AuxiliarId', 3, N'INT', 0, 0),
    (N'InformacaoApoio', N'Descricao', 4, N'NVARCHAR(1000)', 0, 0),
    (N'InformacaoApoio', N'DataRegisto', 5, N'DATETIME2(0)', 0, 0),
    (N'RegistoClinico', N'Id', 1, N'INT', 0, 0),
    (N'RegistoClinico', N'MarcacaoId', 2, N'INT', 0, 0),
    (N'RegistoClinico', N'VeterinarioId', 3, N'INT', 0, 0),
    (N'RegistoClinico', N'NotasClinicas', 4, N'NVARCHAR(4000)', 1, 0),
    (N'RegistoClinico', N'Diagnostico', 5, N'NVARCHAR(1000)', 1, 0),
    (N'RegistoClinico', N'CriadoEm', 6, N'DATETIME2(0)', 0, 0),
    (N'RegistoClinico', N'AtualizadoEm', 7, N'DATETIME2(0)', 1, 0),
    (N'RegistoClinico', N'AtualizadoPorId', 8, N'INT', 1, 0),
    (N'Tratamento', N'Id', 1, N'INT', 0, 0),
    (N'Tratamento', N'RegistoClinicoId', 2, N'INT', 0, 0),
    (N'Tratamento', N'Descricao', 3, N'NVARCHAR(1000)', 0, 0),
    (N'Tratamento', N'DataAplicacao', 4, N'DATE', 0, 0),
    (N'Prescricao', N'Id', 1, N'INT', 0, 0),
    (N'Prescricao', N'RegistoClinicoId', 2, N'INT', 0, 0),
    (N'Prescricao', N'Medicamento', 3, N'NVARCHAR(150)', 0, 0),
    (N'Prescricao', N'Dosagem', 4, N'NVARCHAR(100)', 0, 0),
    (N'Prescricao', N'Frequencia', 5, N'NVARCHAR(100)', 0, 0),
    (N'Prescricao', N'DuracaoDias', 6, N'SMALLINT', 1, 0),
    (N'RegistoProfilaxia', N'Id', 1, N'INT', 0, 0),
    (N'RegistoProfilaxia', N'PacienteId', 2, N'INT', 0, 0),
    (N'RegistoProfilaxia', N'VeterinarioId', 3, N'INT', 0, 0),
    (N'RegistoProfilaxia', N'MarcacaoId', 4, N'INT', 1, 0),
    (N'RegistoProfilaxia', N'Tipo', 5, N'NVARCHAR(15)', 0, 0),
    (N'RegistoProfilaxia', N'Produto', 6, N'NVARCHAR(150)', 0, 0),
    (N'RegistoProfilaxia', N'Lote', 7, N'NVARCHAR(50)', 0, 0),
    (N'RegistoProfilaxia', N'DataAdministracao', 8, N'DATE', 0, 0),
    (N'RegistoProfilaxia', N'IntervaloDias', 9, N'SMALLINT', 1, 0),
    (N'RegistoProfilaxia', N'DataProximaAdministracao', 10, N'DATE', 1, 1),
    (N'RegistoProfilaxia', N'DataRegisto', 11, N'DATETIME2(0)', 0, 0);

DECLARE @ObjetosEsperados TABLE
(
    Tipo   VARCHAR(2) COLLATE DATABASE_DEFAULT NOT NULL,   -- PK, F, UQ, C, D
    Tabela NVARCHAR(128) COLLATE DATABASE_DEFAULT NOT NULL,
    Nome   NVARCHAR(128) COLLATE DATABASE_DEFAULT NOT NULL
);
INSERT INTO @ObjetosEsperados (Tipo, Tabela, Nome) VALUES
    (N'PK', N'Utilizador', N'PK_Utilizador'),
    (N'PK', N'TokenRevogado', N'PK_TokenRevogado'),
    (N'PK', N'Funcionario', N'PK_Funcionario'),
    (N'PK', N'Clinica', N'PK_Clinica'),
    (N'PK', N'ConfiguracaoSistema', N'PK_ConfiguracaoSistema'),
    (N'PK', N'Cliente', N'PK_Cliente'),
    (N'PK', N'Especie', N'PK_Especie'),
    (N'PK', N'Raca', N'PK_Raca'),
    (N'PK', N'Paciente', N'PK_Paciente'),
    (N'PK', N'HorarioFuncionamento', N'PK_HorarioFuncionamento'),
    (N'PK', N'Marcacao', N'PK_Marcacao'),
    (N'PK', N'InformacaoApoio', N'PK_InformacaoApoio'),
    (N'PK', N'RegistoClinico', N'PK_RegistoClinico'),
    (N'PK', N'Tratamento', N'PK_Tratamento'),
    (N'PK', N'Prescricao', N'PK_Prescricao'),
    (N'PK', N'RegistoProfilaxia', N'PK_RegistoProfilaxia'),
    (N'F', N'TokenRevogado', N'FK_TokenRevogado_Utilizador_UtilizadorId'),
    (N'F', N'Funcionario', N'FK_Funcionario_Utilizador_UtilizadorId'),
    (N'F', N'Cliente', N'FK_Cliente_Utilizador_UtilizadorId'),
    (N'F', N'Raca', N'FK_Raca_Especie_EspecieId'),
    (N'F', N'Paciente', N'FK_Paciente_Cliente_ClienteId'),
    (N'F', N'Paciente', N'FK_Paciente_Especie_EspecieId'),
    (N'F', N'Paciente', N'FK_Paciente_Raca'),
    (N'F', N'Marcacao', N'FK_Marcacao_Paciente_PacienteId'),
    (N'F', N'Marcacao', N'FK_Marcacao_Funcionario_VeterinarioId'),
    (N'F', N'Marcacao', N'FK_Marcacao_Funcionario_CriadaPorId'),
    (N'F', N'InformacaoApoio', N'FK_InformacaoApoio_Marcacao_MarcacaoId'),
    (N'F', N'InformacaoApoio', N'FK_InformacaoApoio_Funcionario_AuxiliarId'),
    (N'F', N'RegistoClinico', N'FK_RegistoClinico_Marcacao_MarcacaoId'),
    (N'F', N'RegistoClinico', N'FK_RegistoClinico_Funcionario_VeterinarioId'),
    (N'F', N'RegistoClinico', N'FK_RegistoClinico_Funcionario_AtualizadoPorId'),
    (N'F', N'Tratamento', N'FK_Tratamento_RegistoClinico_RegistoClinicoId'),
    (N'F', N'Prescricao', N'FK_Prescricao_RegistoClinico_RegistoClinicoId'),
    (N'F', N'RegistoProfilaxia', N'FK_RegistoProfilaxia_Paciente_PacienteId'),
    (N'F', N'RegistoProfilaxia', N'FK_RegistoProfilaxia_Funcionario_VeterinarioId'),
    (N'F', N'RegistoProfilaxia', N'FK_RegistoProfilaxia_Marcacao_MarcacaoId'),
    (N'UQ', N'Utilizador', N'UQ_Utilizador_Email'),
    (N'UQ', N'Cliente', N'UQ_Cliente_UtilizadorId'),
    (N'UQ', N'Especie', N'UQ_Especie_Nome'),
    (N'UQ', N'Raca', N'UQ_Raca_Especie_Nome'),
    (N'UQ', N'Raca', N'UQ_Raca_Especie_Id'),
    (N'UQ', N'RegistoClinico', N'UQ_RegistoClinico_Marcacao'),
    (N'C', N'Utilizador', N'CK_Utilizador_Perfil'),
    (N'C', N'Utilizador', N'CK_Utilizador_Estado'),
    (N'C', N'Clinica', N'CK_Clinica_LinhaUnica'),
    (N'C', N'Paciente', N'CK_Paciente_Sexo'),
    (N'C', N'Paciente', N'CK_Paciente_Peso'),
    (N'C', N'HorarioFuncionamento', N'CK_Horario_DiaSemana'),
    (N'C', N'HorarioFuncionamento', N'CK_Horario_Intervalo'),
    (N'C', N'Marcacao', N'CK_Marcacao_Estado'),
    (N'C', N'Marcacao', N'CK_Marcacao_Duracao'),
    (N'C', N'RegistoClinico', N'CK_RegistoClinico_Atualizacao'),
    (N'C', N'Prescricao', N'CK_Prescricao_Duracao'),
    (N'C', N'RegistoProfilaxia', N'CK_Profilaxia_Tipo'),
    (N'C', N'RegistoProfilaxia', N'CK_Profilaxia_Intervalo'),
    (N'D', N'Utilizador', N'DF_Utilizador_Estado'),
    (N'D', N'Utilizador', N'DF_Utilizador_DataCriacao'),
    (N'D', N'TokenRevogado', N'DF_TokenRevogado_DataRevogacao'),
    (N'D', N'Funcionario', N'DF_Funcionario_PodeRegistarApoio'),
    (N'D', N'Cliente', N'DF_Cliente_DataRegisto'),
    (N'D', N'Paciente', N'DF_Paciente_Sexo'),
    (N'D', N'Paciente', N'DF_Paciente_DataRegisto'),
    (N'D', N'HorarioFuncionamento', N'DF_HorarioFuncionamento_Ativo'),
    (N'D', N'Marcacao', N'DF_Marcacao_Estado'),
    (N'D', N'Marcacao', N'DF_Marcacao_DataCriacao'),
    (N'D', N'InformacaoApoio', N'DF_InformacaoApoio_DataRegisto'),
    (N'D', N'RegistoClinico', N'DF_RegistoClinico_CriadoEm'),
    (N'D', N'RegistoProfilaxia', N'DF_RegistoProfilaxia_DataRegisto');

DECLARE @IndicesEsperados TABLE
(
    Tabela   NVARCHAR(128) COLLATE DATABASE_DEFAULT NOT NULL,
    Nome     NVARCHAR(128) COLLATE DATABASE_DEFAULT NOT NULL,
    Unico    BIT            NOT NULL,
    Filtrado BIT            NOT NULL,
    Colunas  NVARCHAR(400)  COLLATE DATABASE_DEFAULT NOT NULL
);
INSERT INTO @IndicesEsperados (Tabela, Nome, Unico, Filtrado, Colunas) VALUES
    (N'Cliente', N'UQ_Cliente_NIF', 1, 1, N'NIF'),
    (N'TokenRevogado', N'IX_TokenRevogado_Utilizador', 0, 0, N'UtilizadorId,DataExpiracao'),
    (N'Paciente', N'IX_Paciente_ClienteId', 0, 0, N'ClienteId'),
    (N'Marcacao', N'IX_Marcacao_Veterinario_Data', 0, 0, N'VeterinarioId,DataHoraInicio'),
    (N'Marcacao', N'IX_Marcacao_Paciente_Data', 0, 0, N'PacienteId,DataHoraInicio'),
    (N'Marcacao', N'IX_Marcacao_Data', 0, 0, N'DataHoraInicio'),
    (N'Tratamento', N'IX_Tratamento_Registo', 0, 0, N'RegistoClinicoId'),
    (N'Prescricao', N'IX_Prescricao_Registo', 0, 0, N'RegistoClinicoId'),
    (N'RegistoProfilaxia', N'IX_Profilaxia_Paciente_Data', 0, 0, N'PacienteId,DataAdministracao'),
    (N'Paciente', N'IX_Paciente_EspecieId_RacaId', 0, 0, N'EspecieId,RacaId'),
    (N'Marcacao', N'IX_Marcacao_CriadaPorId', 0, 0, N'CriadaPorId'),
    (N'InformacaoApoio', N'IX_InformacaoApoio_MarcacaoId', 0, 0, N'MarcacaoId'),
    (N'InformacaoApoio', N'IX_InformacaoApoio_AuxiliarId', 0, 0, N'AuxiliarId'),
    (N'RegistoClinico', N'IX_RegistoClinico_VeterinarioId', 0, 0, N'VeterinarioId'),
    (N'RegistoClinico', N'IX_RegistoClinico_AtualizadoPorId', 0, 0, N'AtualizadoPorId'),
    (N'RegistoProfilaxia', N'IX_RegistoProfilaxia_VeterinarioId', 0, 0, N'VeterinarioId'),
    (N'RegistoProfilaxia', N'IX_RegistoProfilaxia_MarcacaoId', 0, 0, N'MarcacaoId'),
    (N'Cliente', N'IX_Cliente_Nome', 0, 0, N'Nome'),
    (N'Paciente', N'IX_Paciente_Nome', 0, 0, N'Nome');

DECLARE @R TABLE
(
    Ordem       INT IDENTITY(1,1) NOT NULL,
    Verificacao NVARCHAR(60)  NOT NULL,
    Objeto      NVARCHAR(300) NOT NULL,
    Esperado    NVARCHAR(300) NOT NULL,
    Encontrado  NVARCHAR(300) NOT NULL,
    Resultado   NVARCHAR(10)  NOT NULL
);

/* 1. Tabelas previstas e tabelas não previstas -------------------------------- */
INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'1. Tabela', N'dbo.' + e.Tabela, N'existe',
       CASE WHEN t.object_id IS NULL THEN N'em falta' ELSE N'existe' END,
       CASE WHEN t.object_id IS NULL THEN N'DIVERGE' ELSE N'OK' END
FROM (SELECT DISTINCT Tabela FROM @ColunasEsperadas) AS e
LEFT JOIN sys.tables AS t
       ON t.name = e.Tabela COLLATE DATABASE_DEFAULT AND t.schema_id = SCHEMA_ID(N'dbo');

INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'1. Tabela não prevista', SCHEMA_NAME(t.schema_id) + N'.' + t.name, N'não existe', N'existe', N'DIVERGE'
FROM sys.tables AS t
WHERE t.is_ms_shipped = 0
  AND NOT EXISTS (SELECT 1 FROM @ColunasEsperadas AS e
                  WHERE e.Tabela = t.name COLLATE DATABASE_DEFAULT AND t.schema_id = SCHEMA_ID(N'dbo'));

/* 2. Colunas: ordem, tipo, nulidade e coluna calculada persistida ------------- */
/* (o tipo rowversion de Marcacao.RowVersion aparece em sys.types com o nome «timestamp») */
WITH Reais AS
(
    SELECT t.name AS Tabela,
           c.name AS Coluna,
           ROW_NUMBER() OVER (PARTITION BY t.object_id ORDER BY c.column_id) AS Ordem,
           CASE
               WHEN ty.name IN (N'nvarchar', N'nchar')
                   THEN UPPER(ty.name) + N'(' + CASE WHEN c.max_length = -1 THEN N'MAX' ELSE CAST(c.max_length / 2 AS NVARCHAR(10)) END + N')'
               WHEN ty.name IN (N'varchar', N'char', N'varbinary', N'binary')
                   THEN UPPER(ty.name) + N'(' + CASE WHEN c.max_length = -1 THEN N'MAX' ELSE CAST(c.max_length AS NVARCHAR(10)) END + N')'
               WHEN ty.name IN (N'decimal', N'numeric')
                   THEN UPPER(ty.name) + N'(' + CAST(c.precision AS NVARCHAR(5)) + N',' + CAST(c.scale AS NVARCHAR(5)) + N')'
               WHEN ty.name IN (N'datetime2', N'time', N'datetimeoffset')
                   THEN UPPER(ty.name) + N'(' + CAST(c.scale AS NVARCHAR(5)) + N')'
               ELSE UPPER(ty.name)
           END AS Tipo,
           c.is_nullable AS Nulo,
           c.is_computed AS Calculada,
           ISNULL(cc.is_persisted, 0) AS Persistida
    FROM sys.columns AS c
    JOIN sys.tables  AS t  ON t.object_id = c.object_id AND t.schema_id = SCHEMA_ID(N'dbo')
    JOIN sys.types   AS ty ON ty.user_type_id = c.user_type_id
    LEFT JOIN sys.computed_columns AS cc ON cc.object_id = c.object_id AND cc.column_id = c.column_id
)
INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'2. Coluna',
       e.Tabela + N'.' + e.Coluna,
       CONCAT(N'#', e.Ordem, N' ', e.Tipo, CASE WHEN e.Nulo = 1 THEN N' NULL' ELSE N' NOT NULL' END,
              CASE WHEN e.Calculada = 1 THEN N' calculada persistida' ELSE N'' END),
       CASE WHEN r.Coluna IS NULL THEN N'em falta'
            ELSE CONCAT(N'#', r.Ordem, N' ', r.Tipo, CASE WHEN r.Nulo = 1 THEN N' NULL' ELSE N' NOT NULL' END,
                        CASE WHEN r.Calculada = 1 THEN CASE WHEN r.Persistida = 1 THEN N' calculada persistida' ELSE N' calculada NÃO persistida' END ELSE N'' END)
       END,
       CASE WHEN r.Coluna IS NOT NULL
             AND r.Ordem = e.Ordem
             AND r.Tipo = e.Tipo COLLATE DATABASE_DEFAULT
             AND r.Nulo = e.Nulo
             AND r.Calculada = e.Calculada
             AND (e.Calculada = 0 OR r.Persistida = 1)
            THEN N'OK' ELSE N'DIVERGE' END
FROM @ColunasEsperadas AS e
LEFT JOIN Reais AS r
       ON r.Tabela = e.Tabela COLLATE DATABASE_DEFAULT
      AND r.Coluna = e.Coluna COLLATE DATABASE_DEFAULT;

INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'2. Coluna não prevista', t.name + N'.' + c.name, N'não existe', N'existe', N'DIVERGE'
FROM sys.columns AS c
JOIN sys.tables  AS t ON t.object_id = c.object_id AND t.schema_id = SCHEMA_ID(N'dbo')
WHERE EXISTS (SELECT 1 FROM @ColunasEsperadas AS e WHERE e.Tabela = t.name COLLATE DATABASE_DEFAULT)
  AND NOT EXISTS (SELECT 1 FROM @ColunasEsperadas AS e
                  WHERE e.Tabela = t.name COLLATE DATABASE_DEFAULT AND e.Coluna = c.name COLLATE DATABASE_DEFAULT);

/* 3. Restrições PK, FK, UNIQUE, CHECK e DEFAULT -------------------------------- */
INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'3. Restrição ' + e.Tipo,
       e.Tabela + N'.' + e.Nome,
       N'existe',
       CASE WHEN o.object_id IS NULL THEN N'em falta' ELSE N'existe' END,
       CASE WHEN o.object_id IS NULL THEN N'DIVERGE' ELSE N'OK' END
FROM @ObjetosEsperados AS e
LEFT JOIN sys.objects AS o
       ON o.name = e.Nome COLLATE DATABASE_DEFAULT
      AND RTRIM(o.type) = e.Tipo COLLATE DATABASE_DEFAULT
      AND o.parent_object_id = OBJECT_ID(N'dbo.' + e.Tabela);

INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'3. Restrição não prevista', OBJECT_NAME(o.parent_object_id) + N'.' + o.name, N'não existe', RTRIM(o.type), N'DIVERGE'
FROM sys.objects AS o
WHERE o.type IN ('PK', 'F', 'UQ', 'C', 'D')
  AND o.schema_id = SCHEMA_ID(N'dbo')
  AND NOT EXISTS (SELECT 1 FROM @ObjetosEsperados AS e WHERE e.Nome = o.name COLLATE DATABASE_DEFAULT);

/* 4. Regras das restrições: FK com NO ACTION, ativas e fidedignas -------------- */
INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'4. Regra da FK',
       OBJECT_NAME(fk.parent_object_id) + N'.' + fk.name,
       N'ON DELETE/UPDATE NO ACTION, ativa e fidedigna',
       CONCAT(N'DELETE ', fk.delete_referential_action_desc, N', UPDATE ', fk.update_referential_action_desc,
              CASE WHEN fk.is_disabled = 1 THEN N', DESATIVADA' ELSE N'' END,
              CASE WHEN fk.is_not_trusted = 1 THEN N', NÃO FIDEDIGNA' ELSE N'' END),
       CASE WHEN fk.delete_referential_action = 0 AND fk.update_referential_action = 0
             AND fk.is_disabled = 0 AND fk.is_not_trusted = 0 THEN N'OK' ELSE N'DIVERGE' END
FROM sys.foreign_keys AS fk
WHERE fk.schema_id = SCHEMA_ID(N'dbo');

INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'4. Regra do CHECK',
       OBJECT_NAME(ck.parent_object_id) + N'.' + ck.name,
       N'ativa e fidedigna',
       CONCAT(CASE WHEN ck.is_disabled = 1 THEN N'DESATIVADA ' ELSE N'ativa ' END,
              CASE WHEN ck.is_not_trusted = 1 THEN N'NÃO FIDEDIGNA' ELSE N'fidedigna' END),
       CASE WHEN ck.is_disabled = 0 AND ck.is_not_trusted = 0 THEN N'OK' ELSE N'DIVERGE' END
FROM sys.check_constraints AS ck
WHERE ck.schema_id = SCHEMA_ID(N'dbo');

/* 5. Índices: tabela, unicidade, filtro e colunas-chave por ordem ------------- */
WITH Reais AS
(
    SELECT OBJECT_NAME(i.object_id) AS Tabela,
           i.name AS Nome,
           i.is_unique AS Unico,
           i.has_filter AS Filtrado,
           STRING_AGG(c.name, N',') WITHIN GROUP (ORDER BY ic.key_ordinal) AS Colunas
    FROM sys.indexes       AS i
    JOIN sys.tables        AS t  ON t.object_id = i.object_id AND t.schema_id = SCHEMA_ID(N'dbo')
    JOIN sys.index_columns AS ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
    JOIN sys.columns       AS c  ON c.object_id = ic.object_id AND c.column_id = ic.column_id
    WHERE i.is_primary_key = 0 AND i.is_unique_constraint = 0 AND i.type > 0
    GROUP BY i.object_id, i.name, i.is_unique, i.has_filter
)
INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'5. Índice',
       e.Tabela + N'.' + e.Nome,
       CONCAT(CASE WHEN e.Unico = 1 THEN N'UNIQUE ' ELSE N'' END, CASE WHEN e.Filtrado = 1 THEN N'filtrado ' ELSE N'' END, N'(', e.Colunas, N')'),
       CASE WHEN r.Nome IS NULL THEN N'em falta'
            ELSE CONCAT(r.Tabela, N' ', CASE WHEN r.Unico = 1 THEN N'UNIQUE ' ELSE N'' END,
                        CASE WHEN r.Filtrado = 1 THEN N'filtrado ' ELSE N'' END, N'(', r.Colunas, N')') END,
       CASE WHEN r.Nome IS NOT NULL
             AND r.Tabela = e.Tabela COLLATE DATABASE_DEFAULT
             AND r.Unico = e.Unico AND r.Filtrado = e.Filtrado
             AND r.Colunas = e.Colunas COLLATE DATABASE_DEFAULT
            THEN N'OK' ELSE N'DIVERGE' END
FROM @IndicesEsperados AS e
LEFT JOIN Reais AS r ON r.Nome = e.Nome COLLATE DATABASE_DEFAULT;

INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'5. Índice não previsto', OBJECT_NAME(i.object_id) + N'.' + i.name, N'não existe', N'existe', N'DIVERGE'
FROM sys.indexes AS i
JOIN sys.tables  AS t ON t.object_id = i.object_id AND t.schema_id = SCHEMA_ID(N'dbo')
WHERE i.is_primary_key = 0 AND i.is_unique_constraint = 0 AND i.type > 0
  AND NOT EXISTS (SELECT 1 FROM @IndicesEsperados AS e WHERE e.Nome = i.name COLLATE DATABASE_DEFAULT);

/* 6. Filtros dos índices únicos filtrados (Modelo de Dados, secção 6.4) -------- */
INSERT INTO @R (Verificacao, Objeto, Esperado, Encontrado, Resultado)
SELECT N'6. Filtro do índice',
       ISNULL(OBJECT_NAME(i.object_id) + N'.' + i.name, e.Nome),
       e.Filtro,
       ISNULL(i.filter_definition, N'sem filtro'),
       CASE WHEN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ISNULL(i.filter_definition, N''), N'[', N''), N']', N''), N'(', N''), N')', N''), N' ', N'')
               = REPLACE(e.Filtro, N' ', N'') COLLATE DATABASE_DEFAULT
            THEN N'OK' ELSE N'DIVERGE' END
FROM (VALUES (N'UQ_Cliente_NIF', N'NIF IS NOT NULL')) AS e (Nome, Filtro)
LEFT JOIN sys.indexes AS i ON i.name = e.Nome COLLATE DATABASE_DEFAULT;

/* Resultado -------------------------------------------------------------------- */
DECLARE @Total INT = (SELECT COUNT(*) FROM @R);
DECLARE @Divergencias INT = (SELECT COUNT(*) FROM @R WHERE Resultado <> N'OK');

SELECT Verificacao AS [Verificação],
       COUNT(*) AS [Verificações],
       SUM(CASE WHEN Resultado = N'OK' THEN 1 ELSE 0 END) AS [OK],
       SUM(CASE WHEN Resultado <> N'OK' THEN 1 ELSE 0 END) AS [Divergências]
FROM @R
GROUP BY Verificacao
ORDER BY Verificacao;

SELECT Verificacao AS [Verificação], Objeto, Esperado, Encontrado, Resultado
FROM @R
WHERE @MostrarTudo = 1 OR Resultado <> N'OK'
ORDER BY Ordem;

PRINT CONCAT(N'Verificação do esquema ClinicaVeterinaria: ', @Total, N' verificações, ', @Divergencias, N' divergência(s).');

IF @Divergencias > 0
    RAISERROR(N'O esquema não corresponde ao previsto: %d divergência(s). Ver a lista acima.', 16, 1, @Divergencias);
ELSE
    PRINT N'Resultado: o esquema corresponde integralmente ao previsto.';
GO

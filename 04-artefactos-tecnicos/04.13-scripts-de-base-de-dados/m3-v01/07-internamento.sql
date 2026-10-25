/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I060 — Escrever o script de criação do esquema da base de dados
   Ficheiro       : 07-internamento.sql
   Versão         : m3-v01
   Finalidade     : Criar as tabelas de internamento (F007 / ARC-F007): Alojamento,
                    Internamento e HistoricoEstadoInternamento, incluindo os índices
                    únicos filtrados que impedem mais de um internamento ativo por
                    paciente e por alojamento (D12).
   Ordem          : 8 de 9 — executar depois de 06-contas-e-pagamentos.sql.
   Pré-requisitos : Tabelas dbo.Funcionario (01) e dbo.Paciente (02).
   Fonte          : Modelo de Dados v01 (I016), secções 5.1, 5.2, 6.3, 6.4 e
                    7.20 a 7.22; decisão D12.
   Reexecução     : Cada tabela e índice só é criado se ainda não existir; uma
                    segunda execução não falha nem duplica objetos.
   Dados          : Não contém dados pessoais, credenciais nem palavras-passe.
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

IF OBJECT_ID(N'dbo.Alojamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Alojamento
    (
        Id        INT            IDENTITY(1,1) NOT NULL,
        Codigo    NVARCHAR(20)   NOT NULL,
        Descricao NVARCHAR(150)  NULL,
        Ativo     BIT            NOT NULL CONSTRAINT DF_Alojamento_Ativo DEFAULT (1),

        CONSTRAINT PK_Alojamento        PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Alojamento_Codigo UNIQUE (Codigo)
    );
    PRINT N'[07] Tabela dbo.Alojamento criada.';
END
ELSE
    PRINT N'[07] Tabela dbo.Alojamento já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Internamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Internamento
    (
        Id                       INT            IDENTITY(1,1) NOT NULL,
        PacienteId               INT            NOT NULL,
        VeterinarioResponsavelId INT            NOT NULL,
        AlojamentoId             INT            NULL,
        Motivo                   NVARCHAR(500)  NOT NULL,
        EstadoClinico            NVARCHAR(15)   NOT NULL CONSTRAINT DF_Internamento_EstadoClinico DEFAULT (N'EmObservacao'),
        DataEntrada              DATETIME2(0)   NOT NULL CONSTRAINT DF_Internamento_DataEntrada   DEFAULT (SYSUTCDATETIME()),
        DataAlta                 DATETIME2(0)   NULL,
        AltaRegistadaPorId       INT            NULL,

        CONSTRAINT PK_Internamento PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Internamento_Paciente_PacienteId
            FOREIGN KEY (PacienteId) REFERENCES dbo.Paciente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Internamento_Funcionario_VeterinarioResponsavelId
            FOREIGN KEY (VeterinarioResponsavelId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Internamento_Alojamento_AlojamentoId
            FOREIGN KEY (AlojamentoId) REFERENCES dbo.Alojamento (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Internamento_Funcionario_AltaRegistadaPorId
            FOREIGN KEY (AltaRegistadaPorId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_Internamento_Estado CHECK (EstadoClinico IN (N'EmObservacao', N'Recobro', N'AltaMedica')),
        CONSTRAINT CK_Internamento_Alta CHECK (
               (EstadoClinico = N'AltaMedica' AND DataAlta IS NOT NULL)
            OR (EstadoClinico <> N'AltaMedica' AND DataAlta IS NULL)),
        CONSTRAINT CK_Internamento_Datas CHECK (DataAlta IS NULL OR DataAlta >= DataEntrada)
    );
    PRINT N'[07] Tabela dbo.Internamento criada.';
END
ELSE
    PRINT N'[07] Tabela dbo.Internamento já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.Internamento') AND name = N'UQ_Internamento_PacienteAtivo')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UQ_Internamento_PacienteAtivo
        ON dbo.Internamento (PacienteId)
        WHERE DataAlta IS NULL;
    PRINT N'[07] Índice único filtrado UQ_Internamento_PacienteAtivo criado.';
END
ELSE
    PRINT N'[07] Índice UQ_Internamento_PacienteAtivo já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.Internamento') AND name = N'UQ_Internamento_AlojamentoAtivo')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UQ_Internamento_AlojamentoAtivo
        ON dbo.Internamento (AlojamentoId)
        WHERE DataAlta IS NULL AND AlojamentoId IS NOT NULL;
    PRINT N'[07] Índice único filtrado UQ_Internamento_AlojamentoAtivo criado.';
END
ELSE
    PRINT N'[07] Índice UQ_Internamento_AlojamentoAtivo já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.HistoricoEstadoInternamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HistoricoEstadoInternamento
    (
        Id             INT            IDENTITY(1,1) NOT NULL,
        InternamentoId INT            NOT NULL,
        EstadoClinico  NVARCHAR(15)   NOT NULL,
        DataHora       DATETIME2(0)   NOT NULL CONSTRAINT DF_HistoricoEstadoInternamento_DataHora DEFAULT (SYSUTCDATETIME()),
        VeterinarioId  INT            NOT NULL,
        Observacao     NVARCHAR(500)  NULL,

        CONSTRAINT PK_HistoricoEstadoInternamento PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_HistoricoEstadoInternamento_Internamento_InternamentoId
            FOREIGN KEY (InternamentoId) REFERENCES dbo.Internamento (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_HistoricoEstadoInternamento_Funcionario_VeterinarioId
            FOREIGN KEY (VeterinarioId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_HistoricoInternamento_Estado CHECK (EstadoClinico IN (N'EmObservacao', N'Recobro', N'AltaMedica'))
    );
    PRINT N'[07] Tabela dbo.HistoricoEstadoInternamento criada.';
END
ELSE
    PRINT N'[07] Tabela dbo.HistoricoEstadoInternamento já existe — sem alterações.';
GO

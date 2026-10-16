/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I060 — Escrever o script de criação do esquema da base de dados
   Ficheiro       : 03-agenda.sql
   Versão         : m3-v01
   Finalidade     : Criar as tabelas da agenda (F003 / ARC-F003):
                    HorarioFuncionamento, Marcacao e InformacaoApoio. A
                    disponibilidade não é persistida; é calculada (D17).
   Ordem          : 4 de 9 — executar depois de 02-clientes-e-pacientes.sql.
   Pré-requisitos : Tabelas dbo.Funcionario (script 01) e dbo.Paciente (script 02).
   Fonte          : Modelo de Dados v01 (I016), secções 5.1, 5.2, 6.3 e 7.10 a 7.12;
                    decisões D06 e D17.
   Reexecução     : Cada tabela só é criada se ainda não existir; uma segunda
                    execução não falha nem duplica objetos.
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

IF OBJECT_ID(N'dbo.HorarioFuncionamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HorarioFuncionamento
    (
        Id           INT      IDENTITY(1,1) NOT NULL,
        DiaSemana    TINYINT  NOT NULL,
        HoraAbertura TIME(0)  NOT NULL,
        HoraFecho    TIME(0)  NOT NULL,
        Ativo        BIT      NOT NULL CONSTRAINT DF_HorarioFuncionamento_Ativo DEFAULT (1),

        CONSTRAINT PK_HorarioFuncionamento PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT CK_Horario_DiaSemana    CHECK (DiaSemana BETWEEN 1 AND 7),
        CONSTRAINT CK_Horario_Intervalo    CHECK (HoraFecho > HoraAbertura)
    );
    PRINT N'[03] Tabela dbo.HorarioFuncionamento criada.';
END
ELSE
    PRINT N'[03] Tabela dbo.HorarioFuncionamento já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Marcacao', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Marcacao
    (
        Id                  INT            IDENTITY(1,1) NOT NULL,
        PacienteId          INT            NOT NULL,
        VeterinarioId       INT            NOT NULL,
        DataHoraInicio      DATETIME2(0)   NOT NULL,
        DuracaoMinutos      SMALLINT       NOT NULL,
        Motivo              NVARCHAR(250)  NULL,
        Estado              NVARCHAR(12)   NOT NULL CONSTRAINT DF_Marcacao_Estado      DEFAULT (N'Pendente'),
        DataHoraChegada     DATETIME2(0)   NULL,
        CriadaPorId         INT            NOT NULL,
        DataCriacao         DATETIME2(0)   NOT NULL CONSTRAINT DF_Marcacao_DataCriacao DEFAULT (SYSUTCDATETIME()),
        DataUltimaAlteracao DATETIME2(0)   NULL,

        CONSTRAINT PK_Marcacao PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Marcacao_Paciente_PacienteId
            FOREIGN KEY (PacienteId) REFERENCES dbo.Paciente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Marcacao_Funcionario_VeterinarioId
            FOREIGN KEY (VeterinarioId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Marcacao_Funcionario_CriadaPorId
            FOREIGN KEY (CriadaPorId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_Marcacao_Estado  CHECK (Estado IN (N'Pendente', N'Confirmada', N'Realizada', N'Cancelada')),
        CONSTRAINT CK_Marcacao_Duracao CHECK (DuracaoMinutos > 0)
    );
    PRINT N'[03] Tabela dbo.Marcacao criada.';
END
ELSE
    PRINT N'[03] Tabela dbo.Marcacao já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.InformacaoApoio', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.InformacaoApoio
    (
        Id          INT             IDENTITY(1,1) NOT NULL,
        MarcacaoId  INT             NOT NULL,
        AuxiliarId  INT             NOT NULL,
        Descricao   NVARCHAR(1000)  NOT NULL,
        DataRegisto DATETIME2(0)    NOT NULL CONSTRAINT DF_InformacaoApoio_DataRegisto DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_InformacaoApoio PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_InformacaoApoio_Marcacao_MarcacaoId
            FOREIGN KEY (MarcacaoId) REFERENCES dbo.Marcacao (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_InformacaoApoio_Funcionario_AuxiliarId
            FOREIGN KEY (AuxiliarId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION
    );
    PRINT N'[03] Tabela dbo.InformacaoApoio criada.';
END
ELSE
    PRINT N'[03] Tabela dbo.InformacaoApoio já existe — sem alterações.';
GO

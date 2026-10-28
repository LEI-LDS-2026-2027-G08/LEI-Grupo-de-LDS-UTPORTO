/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : por atribuir (a versão m3-v01 destes scripts é a I060, concluída)
   Ficheiro       : 04-registo-clinico.sql
   Versão         : m3-v02
   Finalidade     : Criar as tabelas do registo clínico (F004 / ARC-F004):
                    RegistoClinico, Tratamento e Prescricao. Cada consulta tem no
                    máximo um registo clínico e o paciente obtém-se através da
                    consulta (D07); por isso nenhuma destas tabelas guarda PacienteId.
   Ordem          : 5 de 7 — executar depois de 03-agenda.sql.
   Pré-requisitos : Tabelas dbo.Funcionario (script 01) e dbo.Marcacao (script 03).
   Fonte          : Modelo de Dados v02 (rascunho), secções 5.1, 5.2, 6.3, 6.4 e
                    7.13 a 7.15; decisão D07; EP-031/EP-032 da Documentação da API.
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

IF OBJECT_ID(N'dbo.RegistoClinico', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RegistoClinico
    (
        Id              INT             IDENTITY(1,1) NOT NULL,
        MarcacaoId      INT             NOT NULL,
        VeterinarioId   INT             NOT NULL,
        NotasClinicas   NVARCHAR(4000)  NULL,
        Diagnostico     NVARCHAR(1000)  NULL,
        CriadoEm        DATETIME2(0)    NOT NULL CONSTRAINT DF_RegistoClinico_CriadoEm DEFAULT (SYSUTCDATETIME()),
        AtualizadoEm    DATETIME2(0)    NULL,
        AtualizadoPorId INT             NULL,

        CONSTRAINT PK_RegistoClinico          PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_RegistoClinico_Marcacao UNIQUE (MarcacaoId),
        CONSTRAINT FK_RegistoClinico_Marcacao_MarcacaoId
            FOREIGN KEY (MarcacaoId) REFERENCES dbo.Marcacao (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_RegistoClinico_Funcionario_VeterinarioId
            FOREIGN KEY (VeterinarioId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_RegistoClinico_Funcionario_AtualizadoPorId
            FOREIGN KEY (AtualizadoPorId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_RegistoClinico_Atualizacao CHECK (
               (AtualizadoEm IS NULL AND AtualizadoPorId IS NULL)
            OR (AtualizadoEm IS NOT NULL AND AtualizadoPorId IS NOT NULL))
    );
    PRINT N'[04] Tabela dbo.RegistoClinico criada.';
END
ELSE
    PRINT N'[04] Tabela dbo.RegistoClinico já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Tratamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tratamento
    (
        Id               INT             IDENTITY(1,1) NOT NULL,
        RegistoClinicoId INT             NOT NULL,
        Descricao        NVARCHAR(1000)  NOT NULL,
        DataAplicacao    DATE            NOT NULL,

        CONSTRAINT PK_Tratamento PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Tratamento_RegistoClinico_RegistoClinicoId
            FOREIGN KEY (RegistoClinicoId) REFERENCES dbo.RegistoClinico (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION
    );
    PRINT N'[04] Tabela dbo.Tratamento criada.';
END
ELSE
    PRINT N'[04] Tabela dbo.Tratamento já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Prescricao', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Prescricao
    (
        Id               INT            IDENTITY(1,1) NOT NULL,
        RegistoClinicoId INT            NOT NULL,
        Medicamento      NVARCHAR(150)  NOT NULL,
        Dosagem          NVARCHAR(100)  NOT NULL,
        Frequencia       NVARCHAR(100)  NOT NULL,
        DuracaoDias      SMALLINT       NULL,

        CONSTRAINT PK_Prescricao PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Prescricao_RegistoClinico_RegistoClinicoId
            FOREIGN KEY (RegistoClinicoId) REFERENCES dbo.RegistoClinico (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_Prescricao_Duracao CHECK (DuracaoDias IS NULL OR DuracaoDias > 0)
    );
    PRINT N'[04] Tabela dbo.Prescricao criada.';
END
ELSE
    PRINT N'[04] Tabela dbo.Prescricao já existe — sem alterações.';
GO

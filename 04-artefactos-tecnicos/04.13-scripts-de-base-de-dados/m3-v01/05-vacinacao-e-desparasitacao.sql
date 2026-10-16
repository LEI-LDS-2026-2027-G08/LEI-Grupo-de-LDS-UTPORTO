/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I060 — Escrever o script de criação do esquema da base de dados
   Ficheiro       : 05-vacinacao-e-desparasitacao.sql
   Versão         : m3-v01
   Finalidade     : Criar a tabela RegistoProfilaxia (F006 / ARC-F006), única para
                    vacinações e desparasitações, distinguidas por Tipo (D08). A data
                    da próxima administração é uma coluna calculada e persistida
                    pelo SQL Server (RF045, secção 6.6).
   Ordem          : 6 de 9 — executar depois de 04-registo-clinico.sql.
   Pré-requisitos : Tabelas dbo.Funcionario (01), dbo.Paciente (02) e
                    dbo.Marcacao (03).
   Fonte          : Modelo de Dados v01 (I016), secções 5.1, 5.2, 6.3, 6.6 e 7.16;
                    decisão D08.
   Reexecução     : A tabela só é criada se ainda não existir; uma segunda
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

IF OBJECT_ID(N'dbo.RegistoProfilaxia', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RegistoProfilaxia
    (
        Id                       INT            IDENTITY(1,1) NOT NULL,
        PacienteId               INT            NOT NULL,
        VeterinarioId            INT            NOT NULL,
        MarcacaoId               INT            NULL,
        Tipo                     NVARCHAR(15)   NOT NULL,
        Produto                  NVARCHAR(150)  NOT NULL,
        Lote                     NVARCHAR(50)   NOT NULL,
        DataAdministracao        DATE           NOT NULL,
        IntervaloDias            SMALLINT       NULL,
        DataProximaAdministracao AS (
            CASE
                WHEN IntervaloDias IS NULL THEN NULL
                ELSE DATEADD(DAY, IntervaloDias, DataAdministracao)
            END
        ) PERSISTED,
        DataRegisto              DATETIME2(0)   NOT NULL CONSTRAINT DF_RegistoProfilaxia_DataRegisto DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_RegistoProfilaxia PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_RegistoProfilaxia_Paciente_PacienteId
            FOREIGN KEY (PacienteId) REFERENCES dbo.Paciente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_RegistoProfilaxia_Funcionario_VeterinarioId
            FOREIGN KEY (VeterinarioId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_RegistoProfilaxia_Marcacao_MarcacaoId
            FOREIGN KEY (MarcacaoId) REFERENCES dbo.Marcacao (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_Profilaxia_Tipo      CHECK (Tipo IN (N'Vacina', N'Desparasitacao')),
        CONSTRAINT CK_Profilaxia_Intervalo CHECK (IntervaloDias IS NULL OR IntervaloDias > 0)
    );
    PRINT N'[05] Tabela dbo.RegistoProfilaxia criada.';
END
ELSE
    PRINT N'[05] Tabela dbo.RegistoProfilaxia já existe — sem alterações.';
GO

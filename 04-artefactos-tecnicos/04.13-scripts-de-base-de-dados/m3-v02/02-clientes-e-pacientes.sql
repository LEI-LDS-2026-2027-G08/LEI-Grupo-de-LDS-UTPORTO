/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : por atribuir (a versão m3-v01 destes scripts é a I060, concluída)
   Ficheiro       : 02-clientes-e-pacientes.sql
   Versão         : m3-v02
   Finalidade     : Criar as tabelas de clientes e pacientes (F001/F002 —
                    ARC-F001/ARC-F002): Cliente, Especie, Raca e Paciente, incluindo
                    o índice único filtrado do NIF do Cliente e a chave estrangeira
                    composta que garante a coerência entre espécie e raça (D05).
   Ordem          : 3 de 7 — executar depois de 01-acesso-e-configuracao.sql.
   Pré-requisitos : Tabela dbo.Utilizador (script 01).
   Fonte          : Modelo de Dados v02 (rascunho), secções 5.1, 5.2, 6.3, 6.4 e
                    7.6 a 7.9.
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

IF OBJECT_ID(N'dbo.Cliente', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cliente
    (
        Id           INT            IDENTITY(1,1) NOT NULL,
        UtilizadorId INT            NOT NULL,
        Nome         NVARCHAR(150)  NOT NULL,
        NIF          CHAR(9)        NULL,
        Telefone     NVARCHAR(20)   NOT NULL,
        Morada       NVARCHAR(250)  NULL,
        DataRegisto  DATETIME2(0)   NOT NULL CONSTRAINT DF_Cliente_DataRegisto DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_Cliente              PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Cliente_UtilizadorId UNIQUE (UtilizadorId),
        CONSTRAINT FK_Cliente_Utilizador_UtilizadorId
            FOREIGN KEY (UtilizadorId) REFERENCES dbo.Utilizador (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION
    );
    PRINT N'[02] Tabela dbo.Cliente criada.';
END
ELSE
    PRINT N'[02] Tabela dbo.Cliente já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.Cliente') AND name = N'UQ_Cliente_NIF')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UQ_Cliente_NIF
        ON dbo.Cliente (NIF)
        WHERE NIF IS NOT NULL;
    PRINT N'[02] Índice único filtrado UQ_Cliente_NIF criado.';
END
ELSE
    PRINT N'[02] Índice UQ_Cliente_NIF já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Especie', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Especie
    (
        Id   INT           IDENTITY(1,1) NOT NULL,
        Nome NVARCHAR(50)  NOT NULL,

        CONSTRAINT PK_Especie      PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Especie_Nome UNIQUE (Nome)
    );
    PRINT N'[02] Tabela dbo.Especie criada.';
END
ELSE
    PRINT N'[02] Tabela dbo.Especie já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Raca', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Raca
    (
        Id        INT           IDENTITY(1,1) NOT NULL,
        EspecieId INT           NOT NULL,
        Nome      NVARCHAR(80)  NOT NULL,

        CONSTRAINT PK_Raca              PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Raca_Especie_Nome UNIQUE (EspecieId, Nome),
        CONSTRAINT UQ_Raca_Especie_Id   UNIQUE (EspecieId, Id),
        CONSTRAINT FK_Raca_Especie_EspecieId
            FOREIGN KEY (EspecieId) REFERENCES dbo.Especie (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION
    );
    PRINT N'[02] Tabela dbo.Raca criada.';
END
ELSE
    PRINT N'[02] Tabela dbo.Raca já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Paciente', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Paciente
    (
        Id                  INT            IDENTITY(1,1) NOT NULL,
        ClienteId           INT            NOT NULL,
        Nome                NVARCHAR(80)   NOT NULL,
        EspecieId           INT            NOT NULL,
        RacaId              INT            NULL,
        Sexo                NVARCHAR(12)   NOT NULL CONSTRAINT DF_Paciente_Sexo        DEFAULT (N'Desconhecido'),
        DataNascimento      DATE           NULL,
        Peso                DECIMAL(6,2)   NULL,
        DataRegisto         DATETIME2(0)   NOT NULL CONSTRAINT DF_Paciente_DataRegisto DEFAULT (SYSUTCDATETIME()),
        DataUltimaAlteracao DATETIME2(0)   NULL,

        CONSTRAINT PK_Paciente      PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Paciente_Cliente_ClienteId
            FOREIGN KEY (ClienteId) REFERENCES dbo.Cliente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Paciente_Especie_EspecieId
            FOREIGN KEY (EspecieId) REFERENCES dbo.Especie (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Paciente_Raca
            FOREIGN KEY (EspecieId, RacaId) REFERENCES dbo.Raca (EspecieId, Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_Paciente_Sexo CHECK (Sexo IN (N'Macho', N'Femea', N'Desconhecido')),
        CONSTRAINT CK_Paciente_Peso CHECK (Peso IS NULL OR Peso > 0)
    );
    PRINT N'[02] Tabela dbo.Paciente criada.';
END
ELSE
    PRINT N'[02] Tabela dbo.Paciente já existe — sem alterações.';
GO

/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I060 — Escrever o script de criação do esquema da base de dados
   Ficheiro       : 01-acesso-e-configuracao.sql
   Versão         : m3-v01
   Finalidade     : Criar as tabelas de acesso e configuração (F001 / ARC-F001):
                    Utilizador, TokenRevogado, Funcionario, Clinica e
                    ConfiguracaoSistema, com chaves primárias, chaves estrangeiras,
                    restrições UNIQUE e CHECK e valores por omissão.
   Ordem          : 2 de 9 — executar depois de 00-criar-base-de-dados.sql.
   Pré-requisitos : Base de dados ClinicaVeterinaria criada pelo script 00.
   Fonte          : Modelo de Dados v01 (I016), secções 5.1, 5.2, 6.1, 6.3, 6.4,
                    6.7 e 7.1 a 7.5.
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

IF OBJECT_ID(N'dbo.Utilizador', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Utilizador
    (
        Id               INT            IDENTITY(1,1) NOT NULL,
        Email            NVARCHAR(256)  NOT NULL,
        PasswordHash     NVARCHAR(256)  NOT NULL,
        Perfil           NVARCHAR(25)   NOT NULL,
        Estado           NVARCHAR(10)   NOT NULL CONSTRAINT DF_Utilizador_Estado      DEFAULT (N'Ativo'),
        DataCriacao      DATETIME2(0)   NOT NULL CONSTRAINT DF_Utilizador_DataCriacao DEFAULT (SYSUTCDATETIME()),
        DataUltimoAcesso DATETIME2(0)   NULL,

        CONSTRAINT PK_Utilizador        PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Utilizador_Email  UNIQUE (Email),
        CONSTRAINT CK_Utilizador_Perfil CHECK (Perfil IN (N'Administrador', N'Rececionista', N'Veterinario', N'AuxiliarVeterinario', N'Cliente')),
        CONSTRAINT CK_Utilizador_Estado CHECK (Estado IN (N'Ativo', N'Bloqueado'))
    );
    PRINT N'[01] Tabela dbo.Utilizador criada.';
END
ELSE
    PRINT N'[01] Tabela dbo.Utilizador já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.TokenRevogado', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TokenRevogado
    (
        Jti           NVARCHAR(64)  NOT NULL,
        UtilizadorId  INT           NOT NULL,
        DataExpiracao DATETIME2(0)  NOT NULL,
        DataRevogacao DATETIME2(0)  NOT NULL CONSTRAINT DF_TokenRevogado_DataRevogacao DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_TokenRevogado PRIMARY KEY CLUSTERED (Jti),
        CONSTRAINT FK_TokenRevogado_Utilizador_UtilizadorId
            FOREIGN KEY (UtilizadorId) REFERENCES dbo.Utilizador (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION
    );
    PRINT N'[01] Tabela dbo.TokenRevogado criada.';
END
ELSE
    PRINT N'[01] Tabela dbo.TokenRevogado já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Funcionario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Funcionario
    (
        UtilizadorId      INT            NOT NULL,
        Nome              NVARCHAR(150)  NOT NULL,
        Telefone          NVARCHAR(20)   NULL,
        PodeRegistarApoio BIT            NOT NULL CONSTRAINT DF_Funcionario_PodeRegistarApoio DEFAULT (0),

        CONSTRAINT PK_Funcionario PRIMARY KEY CLUSTERED (UtilizadorId),
        CONSTRAINT FK_Funcionario_Utilizador_UtilizadorId
            FOREIGN KEY (UtilizadorId) REFERENCES dbo.Utilizador (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION
    );
    PRINT N'[01] Tabela dbo.Funcionario criada.';
END
ELSE
    PRINT N'[01] Tabela dbo.Funcionario já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Clinica', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clinica
    (
        Id                  TINYINT        NOT NULL,
        Nome                NVARCHAR(150)  NOT NULL,
        NIF                 CHAR(9)        NOT NULL,
        Morada              NVARCHAR(250)  NOT NULL,
        Telefone            NVARCHAR(20)   NOT NULL,
        Email               NVARCHAR(256)  NOT NULL,
        DataUltimaAlteracao DATETIME2(0)   NOT NULL,

        CONSTRAINT PK_Clinica            PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT CK_Clinica_LinhaUnica CHECK (Id = 1)
    );
    PRINT N'[01] Tabela dbo.Clinica criada.';
END
ELSE
    PRINT N'[01] Tabela dbo.Clinica já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.ConfiguracaoSistema', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ConfiguracaoSistema
    (
        Chave               NVARCHAR(100)  NOT NULL,
        Valor               NVARCHAR(500)  NOT NULL,
        Descricao           NVARCHAR(250)  NULL,
        DataUltimaAlteracao DATETIME2(0)   NOT NULL,

        CONSTRAINT PK_ConfiguracaoSistema PRIMARY KEY CLUSTERED (Chave)
    );
    PRINT N'[01] Tabela dbo.ConfiguracaoSistema criada.';
END
ELSE
    PRINT N'[01] Tabela dbo.ConfiguracaoSistema já existe — sem alterações.';
GO

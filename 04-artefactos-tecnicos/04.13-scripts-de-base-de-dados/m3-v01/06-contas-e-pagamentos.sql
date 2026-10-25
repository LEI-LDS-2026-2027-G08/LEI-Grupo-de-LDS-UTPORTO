/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I060 — Escrever o script de criação do esquema da base de dados
   Ficheiro       : 06-contas-e-pagamentos.sql
   Versão         : m3-v01
   Finalidade     : Criar as tabelas de contas e pagamentos (F005 / ARC-F005):
                    ContaCliente, Pagamento e HistoricoEstadoPagamento. O valor pago
                    e o valor em dívida não são armazenados; são calculados a partir
                    de Pagamento (secção 6.8).
   Ordem          : 7 de 9 — executar depois de 05-vacinacao-e-desparasitacao.sql.
   Pré-requisitos : Tabelas dbo.Funcionario (01), dbo.Cliente e dbo.Paciente (02),
                    dbo.Marcacao (03) e dbo.Tratamento (04).
   Fonte          : Modelo de Dados v01 (I016), secções 5.1, 5.2, 6.3 e 7.17 a 7.19;
                    decisões D09, D10 e D11.
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

IF OBJECT_ID(N'dbo.ContaCliente', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ContaCliente
    (
        Id              INT            IDENTITY(1,1) NOT NULL,
        ClienteId       INT            NOT NULL,
        PacienteId      INT            NOT NULL,
        MarcacaoId      INT            NULL,
        TratamentoId    INT            NULL,
        Descricao       NVARCHAR(250)  NOT NULL,
        Valor           DECIMAL(10,2)  NOT NULL,
        EstadoPagamento NVARCHAR(10)   NOT NULL CONSTRAINT DF_ContaCliente_EstadoPagamento DEFAULT (N'Pendente'),
        DataRegisto     DATETIME2(0)   NOT NULL CONSTRAINT DF_ContaCliente_DataRegisto     DEFAULT (SYSUTCDATETIME()),
        RegistadoPorId  INT            NOT NULL,
        EncerradaEm     DATETIME2(0)   NULL,
        FechadaPorId    INT            NULL,

        CONSTRAINT PK_ContaCliente PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_ContaCliente_Cliente_ClienteId
            FOREIGN KEY (ClienteId) REFERENCES dbo.Cliente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_ContaCliente_Paciente_PacienteId
            FOREIGN KEY (PacienteId) REFERENCES dbo.Paciente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_ContaCliente_Marcacao_MarcacaoId
            FOREIGN KEY (MarcacaoId) REFERENCES dbo.Marcacao (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_ContaCliente_Tratamento_TratamentoId
            FOREIGN KEY (TratamentoId) REFERENCES dbo.Tratamento (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_ContaCliente_Funcionario_RegistadoPorId
            FOREIGN KEY (RegistadoPorId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_ContaCliente_Funcionario_FechadaPorId
            FOREIGN KEY (FechadaPorId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_Conta_Valor           CHECK (Valor > 0),
        CONSTRAINT CK_Conta_EstadoPagamento CHECK (EstadoPagamento IN (N'Pendente', N'Pago')),
        CONSTRAINT CK_Conta_Fecho CHECK (
               (EncerradaEm IS NULL AND FechadaPorId IS NULL)
            OR (EncerradaEm IS NOT NULL AND FechadaPorId IS NOT NULL AND EstadoPagamento = N'Pago'))
    );
    PRINT N'[06] Tabela dbo.ContaCliente criada.';
END
ELSE
    PRINT N'[06] Tabela dbo.ContaCliente já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.Pagamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pagamento
    (
        Id              INT            IDENTITY(1,1) NOT NULL,
        ContaId         INT            NOT NULL,
        Valor           DECIMAL(10,2)  NOT NULL,
        MetodoPagamento NVARCHAR(20)   NOT NULL,
        DataPagamento   DATETIME2(0)   NOT NULL CONSTRAINT DF_Pagamento_DataPagamento DEFAULT (SYSUTCDATETIME()),
        RegistadoPorId  INT            NOT NULL,

        CONSTRAINT PK_Pagamento PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_Pagamento_ContaCliente_ContaId
            FOREIGN KEY (ContaId) REFERENCES dbo.ContaCliente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_Pagamento_Funcionario_RegistadoPorId
            FOREIGN KEY (RegistadoPorId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_Pagamento_Valor  CHECK (Valor > 0),
        CONSTRAINT CK_Pagamento_Metodo CHECK (MetodoPagamento IN (N'Numerario', N'Multibanco', N'Transferencia'))
    );
    PRINT N'[06] Tabela dbo.Pagamento criada.';
END
ELSE
    PRINT N'[06] Tabela dbo.Pagamento já existe — sem alterações.';
GO

IF OBJECT_ID(N'dbo.HistoricoEstadoPagamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HistoricoEstadoPagamento
    (
        Id             INT            IDENTITY(1,1) NOT NULL,
        ContaId        INT            NOT NULL,
        EstadoAnterior NVARCHAR(10)   NOT NULL,
        EstadoNovo     NVARCHAR(10)   NOT NULL,
        Observacao     NVARCHAR(500)  NOT NULL,
        DataAlteracao  DATETIME2(0)   NOT NULL CONSTRAINT DF_HistoricoEstadoPagamento_DataAlteracao DEFAULT (SYSUTCDATETIME()),
        AlteradoPorId  INT            NOT NULL,

        CONSTRAINT PK_HistoricoEstadoPagamento PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_HistoricoEstadoPagamento_ContaCliente_ContaId
            FOREIGN KEY (ContaId) REFERENCES dbo.ContaCliente (Id)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_HistoricoEstadoPagamento_Funcionario_AlteradoPorId
            FOREIGN KEY (AlteradoPorId) REFERENCES dbo.Funcionario (UtilizadorId)
            ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_HistoricoPagamento_Estados CHECK (
                EstadoAnterior IN (N'Pendente', N'Pago')
            AND EstadoNovo     IN (N'Pendente', N'Pago')
            AND EstadoAnterior <> EstadoNovo)
    );
    PRINT N'[06] Tabela dbo.HistoricoEstadoPagamento criada.';
END
ELSE
    PRINT N'[06] Tabela dbo.HistoricoEstadoPagamento já existe — sem alterações.';
GO

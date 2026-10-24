/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : por atribuir (a versão m3-v01 destes scripts é a I060, concluída)
   Ficheiro       : 06-indices.sql
   Versão         : m3-v02
   Finalidade     : Criar os índices não únicos do esquema:
                    A) os 8 índices de desempenho do Modelo de Dados v02 (secção 6.5);
                    B) os índices das chaves estrangeiras que não ficam cobertas por
                       outra chave ou índice — os que o EF Core cria por convenção —,
                       com o nome IX_<Tabela>_<Colunas>;
                    C) os índices de pesquisa de clientes e animais por nome (EP-011
                       e EP-015 da Documentação da API v02).
                    Os índices únicos (UQ_*) são criados com as respetivas tabelas.
   Ordem          : 7 de 7 — último script; executar depois de
                    05-vacinacao-e-desparasitacao.sql.
   Pré-requisitos : Todas as tabelas criadas pelos scripts 01 a 05.
   Fonte          : Modelo de Dados v02 (rascunho), secções 5.2, 6.1 e 6.5;
                    Documentação da API v02 (rascunho), EP-011 e EP-015.
   Reexecução     : Cada índice só é criado se ainda não existir; uma segunda
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

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.TokenRevogado') AND name = N'IX_TokenRevogado_Utilizador')
BEGIN
    CREATE NONCLUSTERED INDEX IX_TokenRevogado_Utilizador ON dbo.TokenRevogado (UtilizadorId, DataExpiracao);
    PRINT N'[06] Índice IX_TokenRevogado_Utilizador criado.';
END
ELSE PRINT N'[06] Índice IX_TokenRevogado_Utilizador já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Paciente') AND name = N'IX_Paciente_ClienteId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Paciente_ClienteId ON dbo.Paciente (ClienteId);
    PRINT N'[06] Índice IX_Paciente_ClienteId criado.';
END
ELSE PRINT N'[06] Índice IX_Paciente_ClienteId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Marcacao') AND name = N'IX_Marcacao_Veterinario_Data')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Marcacao_Veterinario_Data ON dbo.Marcacao (VeterinarioId, DataHoraInicio);
    PRINT N'[06] Índice IX_Marcacao_Veterinario_Data criado.';
END
ELSE PRINT N'[06] Índice IX_Marcacao_Veterinario_Data já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Marcacao') AND name = N'IX_Marcacao_Paciente_Data')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Marcacao_Paciente_Data ON dbo.Marcacao (PacienteId, DataHoraInicio);
    PRINT N'[06] Índice IX_Marcacao_Paciente_Data criado.';
END
ELSE PRINT N'[06] Índice IX_Marcacao_Paciente_Data já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Marcacao') AND name = N'IX_Marcacao_Data')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Marcacao_Data ON dbo.Marcacao (DataHoraInicio);
    PRINT N'[06] Índice IX_Marcacao_Data criado.';
END
ELSE PRINT N'[06] Índice IX_Marcacao_Data já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Tratamento') AND name = N'IX_Tratamento_Registo')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Tratamento_Registo ON dbo.Tratamento (RegistoClinicoId);
    PRINT N'[06] Índice IX_Tratamento_Registo criado.';
END
ELSE PRINT N'[06] Índice IX_Tratamento_Registo já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Prescricao') AND name = N'IX_Prescricao_Registo')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Prescricao_Registo ON dbo.Prescricao (RegistoClinicoId);
    PRINT N'[06] Índice IX_Prescricao_Registo criado.';
END
ELSE PRINT N'[06] Índice IX_Prescricao_Registo já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.RegistoProfilaxia') AND name = N'IX_Profilaxia_Paciente_Data')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Profilaxia_Paciente_Data ON dbo.RegistoProfilaxia (PacienteId, DataAdministracao);
    PRINT N'[06] Índice IX_Profilaxia_Paciente_Data criado.';
END
ELSE PRINT N'[06] Índice IX_Profilaxia_Paciente_Data já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Paciente') AND name = N'IX_Paciente_EspecieId_RacaId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Paciente_EspecieId_RacaId ON dbo.Paciente (EspecieId, RacaId);
    PRINT N'[06] Índice IX_Paciente_EspecieId_RacaId criado.';
END
ELSE PRINT N'[06] Índice IX_Paciente_EspecieId_RacaId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Marcacao') AND name = N'IX_Marcacao_CriadaPorId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Marcacao_CriadaPorId ON dbo.Marcacao (CriadaPorId);
    PRINT N'[06] Índice IX_Marcacao_CriadaPorId criado.';
END
ELSE PRINT N'[06] Índice IX_Marcacao_CriadaPorId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.InformacaoApoio') AND name = N'IX_InformacaoApoio_MarcacaoId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_InformacaoApoio_MarcacaoId ON dbo.InformacaoApoio (MarcacaoId);
    PRINT N'[06] Índice IX_InformacaoApoio_MarcacaoId criado.';
END
ELSE PRINT N'[06] Índice IX_InformacaoApoio_MarcacaoId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.InformacaoApoio') AND name = N'IX_InformacaoApoio_AuxiliarId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_InformacaoApoio_AuxiliarId ON dbo.InformacaoApoio (AuxiliarId);
    PRINT N'[06] Índice IX_InformacaoApoio_AuxiliarId criado.';
END
ELSE PRINT N'[06] Índice IX_InformacaoApoio_AuxiliarId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.RegistoClinico') AND name = N'IX_RegistoClinico_VeterinarioId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_RegistoClinico_VeterinarioId ON dbo.RegistoClinico (VeterinarioId);
    PRINT N'[06] Índice IX_RegistoClinico_VeterinarioId criado.';
END
ELSE PRINT N'[06] Índice IX_RegistoClinico_VeterinarioId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.RegistoClinico') AND name = N'IX_RegistoClinico_AtualizadoPorId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_RegistoClinico_AtualizadoPorId ON dbo.RegistoClinico (AtualizadoPorId);
    PRINT N'[06] Índice IX_RegistoClinico_AtualizadoPorId criado.';
END
ELSE PRINT N'[06] Índice IX_RegistoClinico_AtualizadoPorId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.RegistoProfilaxia') AND name = N'IX_RegistoProfilaxia_VeterinarioId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_RegistoProfilaxia_VeterinarioId ON dbo.RegistoProfilaxia (VeterinarioId);
    PRINT N'[06] Índice IX_RegistoProfilaxia_VeterinarioId criado.';
END
ELSE PRINT N'[06] Índice IX_RegistoProfilaxia_VeterinarioId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.RegistoProfilaxia') AND name = N'IX_RegistoProfilaxia_MarcacaoId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_RegistoProfilaxia_MarcacaoId ON dbo.RegistoProfilaxia (MarcacaoId);
    PRINT N'[06] Índice IX_RegistoProfilaxia_MarcacaoId criado.';
END
ELSE PRINT N'[06] Índice IX_RegistoProfilaxia_MarcacaoId já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Cliente') AND name = N'IX_Cliente_Nome')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Cliente_Nome ON dbo.Cliente (Nome);
    PRINT N'[06] Índice IX_Cliente_Nome criado.';
END
ELSE PRINT N'[06] Índice IX_Cliente_Nome já existe — sem alterações.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Paciente') AND name = N'IX_Paciente_Nome')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Paciente_Nome ON dbo.Paciente (Nome);
    PRINT N'[06] Índice IX_Paciente_Nome criado.';
END
ELSE PRINT N'[06] Índice IX_Paciente_Nome já existe — sem alterações.';
GO

PRINT N'[06] Esquema da base de dados ClinicaVeterinaria concluído.';
GO

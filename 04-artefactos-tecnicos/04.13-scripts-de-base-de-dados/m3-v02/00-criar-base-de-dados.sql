/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : por atribuir (a versão m3-v01 destes scripts é a I060, concluída)
   Ficheiro       : 00-criar-base-de-dados.sql
   Versão         : m3-v02
   Finalidade     : Criar a base de dados ClinicaVeterinaria (identificador físico
                    da base de dados clinica-veterinaria — Modelo de Dados, secção 6.1)
                    com a collation Latin1_General_100_CI_AS (insensível a maiúsculas
                    e sensível a acentos), igual em todas as instâncias (DI-08,
                    proposta a confirmar pelo Revisor).
   Ordem          : 1 de 7 — primeiro script a executar.
   Pré-requisitos : Instância Microsoft SQL Server 2025 (alvo do projeto) acessível;
                    login com permissão CREATE DATABASE (ou base já criada pelo DBA).
   Fonte          : Modelo de Dados v02 (rascunho), secção 6.1; Documentação da
                    Arquitetura v02 (rascunho), ARC-DB.
   Reexecução     : A base de dados só é criada se ainda não existir; uma segunda
                    execução não altera nada.
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

USE master;
GO

IF DB_ID(N'ClinicaVeterinaria') IS NULL
BEGIN
    CREATE DATABASE ClinicaVeterinaria COLLATE Latin1_General_100_CI_AS;
    PRINT N'[00] Base de dados ClinicaVeterinaria criada.';
END
ELSE
    PRINT N'[00] Base de dados ClinicaVeterinaria já existe — sem alterações.';
GO

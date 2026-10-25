/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I060 — Escrever o script de criação do esquema da base de dados
   Ficheiro       : 00-criar-base-de-dados.sql
   Versão         : m3-v01
   Finalidade     : Criar a base de dados ClinicaVeterinaria (identificador físico
                    da base de dados clinica-veterinaria — Modelo de Dados, secção 6.1).
   Ordem          : 1 de 9 — primeiro script a executar.
   Pré-requisitos : Instância Microsoft SQL Server 2025 (alvo do projeto) acessível;
                    login com permissão CREATE DATABASE (ou base já criada pelo DBA).
   Fonte          : Modelo de Dados v01 (I016), secção 6.1; Documentação da
                    Arquitetura v01 (I019), ARC-DB.
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
    CREATE DATABASE ClinicaVeterinaria;
    PRINT N'[00] Base de dados ClinicaVeterinaria criada.';
END
ELSE
    PRINT N'[00] Base de dados ClinicaVeterinaria já existe — sem alterações.';
GO

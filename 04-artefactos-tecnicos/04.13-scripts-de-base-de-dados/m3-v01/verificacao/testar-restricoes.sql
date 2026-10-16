/* =============================================================================
   Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
   Issue          : I060 — Escrever o script de criação do esquema da base de dados
   Ficheiro       : verificacao/testar-restricoes.sql
   Versão         : m3-v01
   Finalidade     : Apoiar a verificação das chaves estrangeiras, restrições UNIQUE
                    e CHECK, valores por omissão e coluna calculada através de
                    inserções válidas (devem ser aceites) e inválidas (devem ser
                    rejeitadas pela restrição indicada).
   Execução       : Depois dos scripts 00 a 08, numa base de dados SEM DADOS (antes
                    dos scripts de dados iniciais da I061). Recusa-se a correr se
                    encontrar linhas em qualquer tabela.
   Efeitos        : Tudo decorre numa única transação, anulada no fim (ROLLBACK). No
                    fim, os contadores IDENTITY consumidos são repostos, para que o
                    primeiro registo real de cada tabela volte a receber Id = 1.
   Dados          : Exclusivamente fictícios (domínio reservado .invalid, NIF
                    genérico 999999990); sem palavras-passe reais.
   Resultado      : Tabela com cada teste e o resultado; termina com erro
                    (severidade 16) se algum teste falhar.
   ============================================================================= */

SET NOCOUNT ON;
SET XACT_ABORT OFF;   -- necessário: os testes negativos provocam erros que têm de ser apanhados sem anular a transação
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

/* --- Pré-condição: base de dados sem dados ---------------------------------- */
DECLARE @Linhas BIGINT =
(
    SELECT ISNULL(SUM(p.rows), 0)
    FROM sys.partitions AS p
    JOIN sys.tables     AS t ON t.object_id = p.object_id
    WHERE t.schema_id = SCHEMA_ID(N'dbo') AND p.index_id IN (0, 1)
);
IF @Linhas > 0
BEGIN
    RAISERROR(N'Testes não executados: a base de dados contém %I64d linha(s). Executar numa base de dados acabada de criar pelos scripts 00 a 08.', 16, 1, @Linhas);
    RETURN;
END;

DECLARE @R TABLE
(
    Ordem     INT IDENTITY(1,1) NOT NULL,
    Teste     NVARCHAR(10)   NOT NULL,
    Descricao NVARCHAR(200)  NOT NULL,
    Esperado  NVARCHAR(200)  NOT NULL,
    Obtido    NVARCHAR(2048) NOT NULL,
    Resultado NVARCHAR(10)   NOT NULL
);

DECLARE @T NVARCHAR(10), @D NVARCHAR(200), @E NVARCHAR(200);

DECLARE @UVet INT, @URec INT, @UAux INT, @UCli1 INT, @UCli2 INT, @UCli3 INT,
        @Cli1 INT, @Cli2 INT, @Cli3 INT,
        @EspA INT, @EspB INT, @RacaA INT, @RacaB INT,
        @Pac1 INT, @Pac2 INT,
        @Marc1 INT, @Marc2 INT,
        @Reg1 INT, @Trat1 INT,
        @Prof1 INT, @Prof2 INT,
        @Conta1 INT, @Conta2 INT,
        @Aloj1 INT, @Aloj2 INT, @Int1 INT;

BEGIN TRY
    BEGIN TRANSACTION;

    /* =========================================================================
       A) Inserções válidas — devem ser aceites (falha aqui interrompe os testes)
       ========================================================================= */
    INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'vet.i060@exemplo.invalid', N'hash-ficticio', N'Veterinario');
    SET @UVet = SCOPE_IDENTITY();
    INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'rec.i060@exemplo.invalid', N'hash-ficticio', N'Rececionista');
    SET @URec = SCOPE_IDENTITY();
    INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'aux.i060@exemplo.invalid', N'hash-ficticio', N'AuxiliarVeterinario');
    SET @UAux = SCOPE_IDENTITY();
    INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'cliente1.i060@exemplo.invalid', N'hash-ficticio', N'Cliente');
    SET @UCli1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'cliente2.i060@exemplo.invalid', N'hash-ficticio', N'Cliente');
    SET @UCli2 = SCOPE_IDENTITY();
    INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'cliente3.i060@exemplo.invalid', N'hash-ficticio', N'Cliente');
    SET @UCli3 = SCOPE_IDENTITY();

    INSERT INTO dbo.TokenRevogado (Jti, UtilizadorId, DataExpiracao) VALUES (N'jti-teste-i060', @UCli1, '2026-12-31T23:59:59');

    INSERT INTO dbo.Funcionario (UtilizadorId, Nome) VALUES (@UVet, N'Veterinário Teste');
    INSERT INTO dbo.Funcionario (UtilizadorId, Nome) VALUES (@URec, N'Rececionista Teste');
    INSERT INTO dbo.Funcionario (UtilizadorId, Nome, PodeRegistarApoio) VALUES (@UAux, N'Auxiliar Teste', 1);

    INSERT INTO dbo.Clinica (Id, Nome, NIF, Morada, Telefone, Email, DataUltimaAlteracao)
    VALUES (1, N'Clínica Teste', '999999990', N'Rua Fictícia, 1', N'200000000', N'clinica.i060@exemplo.invalid', SYSUTCDATETIME());
    INSERT INTO dbo.ConfiguracaoSistema (Chave, Valor, DataUltimaAlteracao) VALUES (N'DuracaoPadraoConsultaMinutos', N'30', SYSUTCDATETIME());

    INSERT INTO dbo.Cliente (UtilizadorId, Nome, NIF, Telefone) VALUES (@UCli1, N'Cliente Teste 1', '999999990', N'900000001');
    SET @Cli1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Cliente (UtilizadorId, Nome, Telefone) VALUES (@UCli2, N'Cliente Teste 2', N'900000002');  -- NIF NULL
    SET @Cli2 = SCOPE_IDENTITY();
    INSERT INTO dbo.Cliente (UtilizadorId, Nome, Telefone) VALUES (@UCli3, N'Cliente Teste 3', N'900000003');  -- NIF NULL
    SET @Cli3 = SCOPE_IDENTITY();

    INSERT INTO dbo.Especie (Nome) VALUES (N'Espécie Teste A');  SET @EspA = SCOPE_IDENTITY();
    INSERT INTO dbo.Especie (Nome) VALUES (N'Espécie Teste B');  SET @EspB = SCOPE_IDENTITY();
    INSERT INTO dbo.Raca (EspecieId, Nome) VALUES (@EspA, N'Raça Teste A');  SET @RacaA = SCOPE_IDENTITY();
    INSERT INTO dbo.Raca (EspecieId, Nome) VALUES (@EspB, N'Raça Teste B');  SET @RacaB = SCOPE_IDENTITY();

    INSERT INTO dbo.Paciente (ClienteId, Nome, EspecieId, RacaId, Sexo, Peso) VALUES (@Cli1, N'Animal Teste 1', @EspA, @RacaA, N'Macho', 12.50);
    SET @Pac1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Paciente (ClienteId, Nome, EspecieId) VALUES (@Cli1, N'Animal Teste 2', @EspA);  -- sem raça, Sexo por omissão
    SET @Pac2 = SCOPE_IDENTITY();

    INSERT INTO dbo.HorarioFuncionamento (DiaSemana, HoraAbertura, HoraFecho) VALUES (1, '09:00', '13:00');
    INSERT INTO dbo.HorarioFuncionamento (DiaSemana, HoraAbertura, HoraFecho) VALUES (1, '14:00', '19:00');  -- vários períodos no mesmo dia

    INSERT INTO dbo.Marcacao (PacienteId, VeterinarioId, DataHoraInicio, DuracaoMinutos, CriadaPorId)
    VALUES (@Pac1, @UVet, '2026-10-20T09:00:00', 30, @URec);
    SET @Marc1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Marcacao (PacienteId, VeterinarioId, DataHoraInicio, DuracaoMinutos, Estado, CriadaPorId)
    VALUES (@Pac2, @UVet, '2026-10-20T09:30:00', 30, N'Confirmada', @URec);
    SET @Marc2 = SCOPE_IDENTITY();

    INSERT INTO dbo.InformacaoApoio (MarcacaoId, AuxiliarId, Descricao) VALUES (@Marc1, @UAux, N'Sala preparada (teste).');

    INSERT INTO dbo.RegistoClinico (MarcacaoId, VeterinarioId, Diagnostico) VALUES (@Marc1, @UVet, N'Diagnóstico de teste');
    SET @Reg1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Tratamento (RegistoClinicoId, Descricao, DataAplicacao) VALUES (@Reg1, N'Tratamento de teste', '2026-10-20');
    SET @Trat1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Prescricao (RegistoClinicoId, Medicamento, Dosagem, Frequencia, DuracaoDias) VALUES (@Reg1, N'Medicamento X', N'3 gotas', N'2 vezes por dia', 7);

    INSERT INTO dbo.RegistoProfilaxia (PacienteId, VeterinarioId, MarcacaoId, Tipo, Produto, Lote, DataAdministracao, IntervaloDias)
    VALUES (@Pac1, @UVet, @Marc1, N'Vacina', N'Vacina de teste', N'LOTE-T1', '2026-10-01', 30);
    SET @Prof1 = SCOPE_IDENTITY();
    INSERT INTO dbo.RegistoProfilaxia (PacienteId, VeterinarioId, Tipo, Produto, Lote, DataAdministracao)
    VALUES (@Pac1, @UVet, N'Desparasitacao', N'Desparasitante de teste', N'LOTE-T2', '2026-10-01');
    SET @Prof2 = SCOPE_IDENTITY();

    INSERT INTO dbo.ContaCliente (ClienteId, PacienteId, MarcacaoId, TratamentoId, Descricao, Valor, RegistadoPorId)
    VALUES (@Cli1, @Pac1, @Marc1, @Trat1, N'Consulta de teste', 45.00, @URec);
    SET @Conta1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Pagamento (ContaId, Valor, MetodoPagamento, RegistadoPorId) VALUES (@Conta1, 20.00, N'Numerario', @URec);
    INSERT INTO dbo.HistoricoEstadoPagamento (ContaId, EstadoAnterior, EstadoNovo, Observacao, AlteradoPorId)
    VALUES (@Conta1, N'Pendente', N'Pago', N'Correção de teste', @URec);
    INSERT INTO dbo.ContaCliente (ClienteId, PacienteId, Descricao, Valor, EstadoPagamento, RegistadoPorId, EncerradaEm, FechadaPorId)
    VALUES (@Cli1, @Pac2, N'Conta paga e encerrada (teste)', 10.00, N'Pago', @URec, SYSUTCDATETIME(), @URec);
    SET @Conta2 = SCOPE_IDENTITY();

    INSERT INTO dbo.Alojamento (Codigo) VALUES (N'TESTE-01');  SET @Aloj1 = SCOPE_IDENTITY();
    INSERT INTO dbo.Alojamento (Codigo) VALUES (N'TESTE-02');  SET @Aloj2 = SCOPE_IDENTITY();
    INSERT INTO dbo.Internamento (PacienteId, VeterinarioResponsavelId, AlojamentoId, Motivo, DataEntrada)
    VALUES (@Pac1, @UVet, @Aloj1, N'Internamento de teste', '2026-10-10T10:00:00');
    SET @Int1 = SCOPE_IDENTITY();
    INSERT INTO dbo.HistoricoEstadoInternamento (InternamentoId, EstadoClinico, VeterinarioId) VALUES (@Int1, N'EmObservacao', @UVet);

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    VALUES (N'V01', N'Inserção de um registo válido em cada uma das 22 tabelas', N'Aceites', N'Aceites', N'OK');

    /* =========================================================================
       B) Valores por omissão, coluna calculada e unicidade filtrada
       ========================================================================= */
    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P01', N'Utilizador.Estado por omissão (DF_Utilizador_Estado)', N'Ativo', Estado, CASE WHEN Estado = N'Ativo' THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.Utilizador WHERE Id = @UVet;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P02', N'Paciente.Sexo por omissão (DF_Paciente_Sexo)', N'Desconhecido', Sexo, CASE WHEN Sexo = N'Desconhecido' THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.Paciente WHERE Id = @Pac2;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P03', N'Marcacao.Estado por omissão (DF_Marcacao_Estado)', N'Pendente', Estado, CASE WHEN Estado = N'Pendente' THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.Marcacao WHERE Id = @Marc1;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P04', N'ContaCliente.EstadoPagamento por omissão (DF_ContaCliente_EstadoPagamento)', N'Pendente', EstadoPagamento,
           CASE WHEN EstadoPagamento = N'Pendente' THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.ContaCliente WHERE Id = @Conta1;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P05', N'Datas de registo preenchidas por omissão em UTC (SYSUTCDATETIME)', N'Preenchidas',
           CASE WHEN DataCriacao IS NOT NULL THEN N'Preenchidas' ELSE N'NULL' END,
           CASE WHEN DataCriacao IS NOT NULL AND ABS(DATEDIFF(MINUTE, DataCriacao, SYSUTCDATETIME())) <= 5 THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.Utilizador WHERE Id = @UVet;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P06', N'DataProximaAdministracao = 2026-10-01 + 30 dias (RF045)', N'2026-10-31',
           ISNULL(CONVERT(NVARCHAR(10), DataProximaAdministracao, 23), N'NULL'),
           CASE WHEN DataProximaAdministracao = '2026-10-31' THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.RegistoProfilaxia WHERE Id = @Prof1;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P07', N'Sem IntervaloDias, DataProximaAdministracao fica NULL', N'NULL',
           ISNULL(CONVERT(NVARCHAR(10), DataProximaAdministracao, 23), N'NULL'),
           CASE WHEN DataProximaAdministracao IS NULL THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.RegistoProfilaxia WHERE Id = @Prof2;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P08', N'Vários Clientes sem NIF aceites (UQ_Cliente_NIF filtrado)', N'2',
           CAST(COUNT(*) AS NVARCHAR(10)), CASE WHEN COUNT(*) = 2 THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.Cliente WHERE NIF IS NULL;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P09', N'Paciente sem raça aceite (FK composta não se aplica a RacaId NULL)', N'RacaId NULL',
           CASE WHEN RacaId IS NULL THEN N'RacaId NULL' ELSE N'RacaId preenchido' END,
           CASE WHEN RacaId IS NULL THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.Paciente WHERE Id = @Pac2;

    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    SELECT N'P10', N'Conta paga e encerrada aceite (CK_Conta_Fecho)', N'Pago + EncerradaEm',
           CONCAT(EstadoPagamento, CASE WHEN EncerradaEm IS NOT NULL THEN N' + EncerradaEm' ELSE N'' END),
           CASE WHEN EstadoPagamento = N'Pago' AND EncerradaEm IS NOT NULL THEN N'OK' ELSE N'FALHOU' END
    FROM dbo.ContaCliente WHERE Id = @Conta2;

    /* =========================================================================
       C) Inserções e operações inválidas — devem ser rejeitadas pela restrição
          indicada em @E (o nome da restrição consta da mensagem de erro).
       ========================================================================= */

    -- Utilizador / acesso
    SELECT @T = N'N01', @D = N'E-mail de utilizador repetido', @E = N'UQ_Utilizador_Email';
    BEGIN TRY
        INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'vet.i060@exemplo.invalid', N'hash-ficticio', N'Veterinario');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N02', @D = N'Perfil inexistente', @E = N'CK_Utilizador_Perfil';
    BEGIN TRY
        INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (N'n02.i060@exemplo.invalid', N'hash-ficticio', N'Gestor');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N03', @D = N'Estado de utilizador inexistente', @E = N'CK_Utilizador_Estado';
    BEGIN TRY
        INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil, Estado) VALUES (N'n03.i060@exemplo.invalid', N'hash-ficticio', N'Cliente', N'Suspenso');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N04', @D = N'E-mail de utilizador nulo (NOT NULL)', @E = N'Erro 515 (NOT NULL)';
    BEGIN TRY
        INSERT INTO dbo.Utilizador (Email, PasswordHash, Perfil) VALUES (NULL, N'hash-ficticio', N'Cliente');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_NUMBER() = 515 THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N05', @D = N'Token revogado de utilizador inexistente', @E = N'FK_TokenRevogado_Utilizador_UtilizadorId';
    BEGIN TRY
        INSERT INTO dbo.TokenRevogado (Jti, UtilizadorId, DataExpiracao) VALUES (N'jti-n05', -1, '2026-12-31T23:59:59');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N06', @D = N'Jti repetido', @E = N'PK_TokenRevogado';
    BEGIN TRY
        INSERT INTO dbo.TokenRevogado (Jti, UtilizadorId, DataExpiracao) VALUES (N'jti-teste-i060', @UCli1, '2026-12-31T23:59:59');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N07', @D = N'Funcionário sem utilizador', @E = N'FK_Funcionario_Utilizador_UtilizadorId';
    BEGIN TRY
        INSERT INTO dbo.Funcionario (UtilizadorId, Nome) VALUES (-1, N'Sem conta');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N08', @D = N'Segunda linha de Clinica (Id = 2)', @E = N'CK_Clinica_LinhaUnica';
    BEGIN TRY
        INSERT INTO dbo.Clinica (Id, Nome, NIF, Morada, Telefone, Email, DataUltimaAlteracao)
        VALUES (2, N'Outra', '999999990', N'Rua', N'200000000', N'outra.i060@exemplo.invalid', SYSUTCDATETIME());
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- Clientes e pacientes
    SELECT @T = N'N09', @D = N'Dois Clientes com a mesma conta de utilizador', @E = N'UQ_Cliente_UtilizadorId';
    BEGIN TRY
        INSERT INTO dbo.Cliente (UtilizadorId, Nome, Telefone) VALUES (@UCli1, N'Duplicado', N'900000009');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N10', @D = N'NIF de Cliente repetido', @E = N'UQ_Cliente_NIF';
    BEGIN TRY
        UPDATE dbo.Cliente SET NIF = '999999990' WHERE Id = @Cli2;
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N11', @D = N'Cliente sem utilizador', @E = N'FK_Cliente_Utilizador_UtilizadorId';
    BEGIN TRY
        INSERT INTO dbo.Cliente (UtilizadorId, Nome, Telefone) VALUES (-1, N'Sem conta', N'900000011');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N12', @D = N'Espécie repetida', @E = N'UQ_Especie_Nome';
    BEGIN TRY
        INSERT INTO dbo.Especie (Nome) VALUES (N'Espécie Teste A');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N13', @D = N'Raça repetida na mesma espécie', @E = N'UQ_Raca_Especie_Nome';
    BEGIN TRY
        INSERT INTO dbo.Raca (EspecieId, Nome) VALUES (@EspA, N'Raça Teste A');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N14', @D = N'Raça de outra espécie (D05)', @E = N'FK_Paciente_Raca';
    BEGIN TRY
        INSERT INTO dbo.Paciente (ClienteId, Nome, EspecieId, RacaId) VALUES (@Cli1, N'Incoerente', @EspA, @RacaB);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N15', @D = N'Peso igual a zero', @E = N'CK_Paciente_Peso';
    BEGIN TRY
        INSERT INTO dbo.Paciente (ClienteId, Nome, EspecieId, Peso) VALUES (@Cli1, N'Peso zero', @EspA, 0);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N16', @D = N'Sexo fora da lista', @E = N'CK_Paciente_Sexo';
    BEGIN TRY
        INSERT INTO dbo.Paciente (ClienteId, Nome, EspecieId, Sexo) VALUES (@Cli1, N'Sexo inválido', @EspA, N'M');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- Agenda
    SELECT @T = N'N17', @D = N'Dia da semana 8', @E = N'CK_Horario_DiaSemana';
    BEGIN TRY
        INSERT INTO dbo.HorarioFuncionamento (DiaSemana, HoraAbertura, HoraFecho) VALUES (8, '09:00', '13:00');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N18', @D = N'Hora de fecho igual à de abertura', @E = N'CK_Horario_Intervalo';
    BEGIN TRY
        INSERT INTO dbo.HorarioFuncionamento (DiaSemana, HoraAbertura, HoraFecho) VALUES (2, '13:00', '13:00');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N19', @D = N'Estado de marcação inexistente', @E = N'CK_Marcacao_Estado';
    BEGIN TRY
        INSERT INTO dbo.Marcacao (PacienteId, VeterinarioId, DataHoraInicio, DuracaoMinutos, Estado, CriadaPorId)
        VALUES (@Pac1, @UVet, '2026-10-21T09:00:00', 30, N'Adiada', @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N20', @D = N'Duração da consulta igual a zero', @E = N'CK_Marcacao_Duracao';
    BEGIN TRY
        INSERT INTO dbo.Marcacao (PacienteId, VeterinarioId, DataHoraInicio, DuracaoMinutos, CriadaPorId)
        VALUES (@Pac1, @UVet, '2026-10-21T10:00:00', 0, @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N21', @D = N'Veterinário que não é funcionário', @E = N'FK_Marcacao_Funcionario_VeterinarioId';
    BEGIN TRY
        INSERT INTO dbo.Marcacao (PacienteId, VeterinarioId, DataHoraInicio, DuracaoMinutos, CriadaPorId)
        VALUES (@Pac1, @UCli1, '2026-10-21T11:00:00', 30, @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N22', @D = N'Informação de apoio para consulta inexistente', @E = N'FK_InformacaoApoio_Marcacao_MarcacaoId';
    BEGIN TRY
        INSERT INTO dbo.InformacaoApoio (MarcacaoId, AuxiliarId, Descricao) VALUES (-1, @UAux, N'Sem consulta');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- Registo clínico
    SELECT @T = N'N23', @D = N'Segundo registo clínico para a mesma consulta (EP-031, 409)', @E = N'UQ_RegistoClinico_Marcacao';
    BEGIN TRY
        INSERT INTO dbo.RegistoClinico (MarcacaoId, VeterinarioId, Diagnostico) VALUES (@Marc1, @UVet, N'Duplicado');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N24', @D = N'AtualizadoEm sem AtualizadoPorId', @E = N'CK_RegistoClinico_Atualizacao';
    BEGIN TRY
        INSERT INTO dbo.RegistoClinico (MarcacaoId, VeterinarioId, Diagnostico, AtualizadoEm) VALUES (@Marc2, @UVet, N'Teste', SYSUTCDATETIME());
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N25', @D = N'Duração da prescrição igual a zero', @E = N'CK_Prescricao_Duracao';
    BEGIN TRY
        INSERT INTO dbo.Prescricao (RegistoClinicoId, Medicamento, Dosagem, Frequencia, DuracaoDias) VALUES (@Reg1, N'X', N'1', N'1', 0);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- Vacinação e desparasitação
    SELECT @T = N'N26', @D = N'Tipo de profilaxia inexistente', @E = N'CK_Profilaxia_Tipo';
    BEGIN TRY
        INSERT INTO dbo.RegistoProfilaxia (PacienteId, VeterinarioId, Tipo, Produto, Lote, DataAdministracao)
        VALUES (@Pac1, @UVet, N'Antibiotico', N'X', N'L', '2026-10-01');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N27', @D = N'Intervalo em dias igual a zero', @E = N'CK_Profilaxia_Intervalo';
    BEGIN TRY
        INSERT INTO dbo.RegistoProfilaxia (PacienteId, VeterinarioId, Tipo, Produto, Lote, DataAdministracao, IntervaloDias)
        VALUES (@Pac1, @UVet, N'Vacina', N'X', N'L', '2026-10-01', 0);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- Contas e pagamentos
    SELECT @T = N'N28', @D = N'Valor da conta igual a zero', @E = N'CK_Conta_Valor';
    BEGIN TRY
        INSERT INTO dbo.ContaCliente (ClienteId, PacienteId, Descricao, Valor, RegistadoPorId) VALUES (@Cli1, @Pac1, N'Zero', 0, @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N29', @D = N'Estado de pagamento inexistente', @E = N'CK_Conta_EstadoPagamento';
    BEGIN TRY
        INSERT INTO dbo.ContaCliente (ClienteId, PacienteId, Descricao, Valor, EstadoPagamento, RegistadoPorId) VALUES (@Cli1, @Pac1, N'Parcial', 5, N'Parcial', @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N30', @D = N'Encerrar conta ainda pendente', @E = N'CK_Conta_Fecho';
    BEGIN TRY
        UPDATE dbo.ContaCliente SET EncerradaEm = SYSUTCDATETIME(), FechadaPorId = @URec WHERE Id = @Conta1;
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N31', @D = N'Conta associada a tratamento inexistente', @E = N'FK_ContaCliente_Tratamento_TratamentoId';
    BEGIN TRY
        INSERT INTO dbo.ContaCliente (ClienteId, PacienteId, TratamentoId, Descricao, Valor, RegistadoPorId) VALUES (@Cli1, @Pac1, -1, N'Sem tratamento', 5, @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N32', @D = N'Pagamento negativo', @E = N'CK_Pagamento_Valor';
    BEGIN TRY
        INSERT INTO dbo.Pagamento (ContaId, Valor, MetodoPagamento, RegistadoPorId) VALUES (@Conta1, -5, N'Numerario', @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N33', @D = N'Método de pagamento fora da lista', @E = N'CK_Pagamento_Metodo';
    BEGIN TRY
        INSERT INTO dbo.Pagamento (ContaId, Valor, MetodoPagamento, RegistadoPorId) VALUES (@Conta1, 5, N'MBWay', @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N34', @D = N'Histórico com estado anterior igual ao novo', @E = N'CK_HistoricoPagamento_Estados';
    BEGIN TRY
        INSERT INTO dbo.HistoricoEstadoPagamento (ContaId, EstadoAnterior, EstadoNovo, Observacao, AlteradoPorId)
        VALUES (@Conta1, N'Pendente', N'Pendente', N'Sem alteração', @URec);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- Internamento
    SELECT @T = N'N35', @D = N'Código de alojamento repetido', @E = N'UQ_Alojamento_Codigo';
    BEGIN TRY
        INSERT INTO dbo.Alojamento (Codigo) VALUES (N'TESTE-01');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N36', @D = N'Estado clínico inexistente', @E = N'CK_Internamento_Estado';
    BEGIN TRY
        INSERT INTO dbo.Internamento (PacienteId, VeterinarioResponsavelId, Motivo, EstadoClinico) VALUES (@Pac2, @UVet, N'Teste', N'Critico');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N37', @D = N'Alta médica sem data de alta', @E = N'CK_Internamento_Alta';
    BEGIN TRY
        INSERT INTO dbo.Internamento (PacienteId, VeterinarioResponsavelId, Motivo, EstadoClinico) VALUES (@Pac2, @UVet, N'Teste', N'AltaMedica');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N38', @D = N'Data de alta anterior à entrada', @E = N'CK_Internamento_Datas';
    BEGIN TRY
        INSERT INTO dbo.Internamento (PacienteId, VeterinarioResponsavelId, Motivo, EstadoClinico, DataEntrada, DataAlta, AltaRegistadaPorId)
        VALUES (@Pac2, @UVet, N'Teste', N'AltaMedica', '2026-10-10T10:00:00', '2026-10-09T10:00:00', @UVet);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N39', @D = N'Segundo internamento ativo do mesmo paciente', @E = N'UQ_Internamento_PacienteAtivo';
    BEGIN TRY
        INSERT INTO dbo.Internamento (PacienteId, VeterinarioResponsavelId, AlojamentoId, Motivo) VALUES (@Pac1, @UVet, @Aloj2, N'Duplicado');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N40', @D = N'Alojamento já ocupado por internamento ativo', @E = N'UQ_Internamento_AlojamentoAtivo';
    BEGIN TRY
        INSERT INTO dbo.Internamento (PacienteId, VeterinarioResponsavelId, AlojamentoId, Motivo) VALUES (@Pac2, @UVet, @Aloj1, N'Ocupado');
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N41', @D = N'Estado clínico inexistente no histórico', @E = N'CK_HistoricoInternamento_Estado';
    BEGIN TRY
        INSERT INTO dbo.HistoricoEstadoInternamento (InternamentoId, EstadoClinico, VeterinarioId) VALUES (@Int1, N'Critico', @UVet);
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_MESSAGE() LIKE N'%' + @E + N'%' THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- Eliminação sem cascata (D13): não se apagam dados referenciados.
    SELECT @T = N'N42', @D = N'Apagar paciente com consultas, contas e internamento', @E = N'Erro 547 (NO ACTION)';
    BEGIN TRY
        DELETE FROM dbo.Paciente WHERE Id = @Pac1;
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_NUMBER() = 547 THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    SELECT @T = N'N43', @D = N'Apagar utilizador de um Veterinário com atos registados', @E = N'Erro 547 (NO ACTION)';
    BEGIN TRY
        DELETE FROM dbo.Utilizador WHERE Id = @UVet;
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_NUMBER() = 547 THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    /* --- Depois da alta, o paciente e o alojamento podem voltar a ser usados -- */
    UPDATE dbo.Internamento
    SET EstadoClinico = N'AltaMedica', DataAlta = '2026-10-12T10:00:00', AltaRegistadaPorId = @UVet
    WHERE Id = @Int1;

    SELECT @T = N'P11', @D = N'Novo internamento do mesmo paciente e alojamento após a alta', @E = N'Aceite';
    BEGIN TRY
        INSERT INTO dbo.Internamento (PacienteId, VeterinarioResponsavelId, AlojamentoId, Motivo) VALUES (@Pac1, @UVet, @Aloj1, N'Reinternamento');
        INSERT INTO @R VALUES (@T, @D, @E, N'Aceite', N'OK');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), N'FALHOU');
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    -- A coluna calculada não pode ser escrita; SQL dinâmico para o erro de compilação ser apanhado.
    SELECT @T = N'N44', @D = N'Escrita manual de DataProximaAdministracao (coluna calculada)', @E = N'Erro 271 (coluna calculada)';
    BEGIN TRY
        EXEC sys.sp_executesql
            N'UPDATE dbo.RegistoProfilaxia SET DataProximaAdministracao = ''2027-01-01'' WHERE Id = @Id;',
            N'@Id INT', @Id = @Prof1;
        INSERT INTO @R VALUES (@T, @D, @E, N'Operação aceite', N'FALHOU');
    END TRY
    BEGIN CATCH
        INSERT INTO @R VALUES (@T, @D, @E, ERROR_MESSAGE(), CASE WHEN ERROR_NUMBER() = 271 THEN N'OK' ELSE N'FALHOU' END);
    END CATCH;
    IF XACT_STATE() = -1 THROW 50001, N'Transação inválida após um teste negativo.', 1;

    ROLLBACK TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    VALUES (N'ERRO', N'Erro inesperado — testes interrompidos (linha ' + CAST(ERROR_LINE() AS NVARCHAR(10)) + N')', N'Sem erro', ERROR_MESSAGE(), N'FALHOU');
END CATCH;

/* --- Repor os contadores IDENTITY consumidos pela transação anulada ------- */
IF NOT EXISTS (SELECT 1 FROM sys.partitions AS p JOIN sys.tables AS t ON t.object_id = p.object_id
               WHERE t.schema_id = SCHEMA_ID(N'dbo') AND p.index_id IN (0, 1) AND p.rows > 0)
BEGIN
    DECLARE @Reseed NVARCHAR(MAX) = N'';
    SELECT @Reseed += N'DBCC CHECKIDENT (N''dbo.' + t.name + N''', RESEED, 0) WITH NO_INFOMSGS;' + NCHAR(10)
    FROM sys.identity_columns AS ic
    JOIN sys.tables AS t ON t.object_id = ic.object_id
    WHERE t.schema_id = SCHEMA_ID(N'dbo') AND ic.last_value IS NOT NULL;
    EXEC sys.sp_executesql @Reseed;
    PRINT N'Contadores IDENTITY repostos; a base de dados ficou sem dados.';
END
ELSE
    INSERT INTO @R (Teste, Descricao, Esperado, Obtido, Resultado)
    VALUES (N'FIM', N'A base de dados devia ficar sem dados após o ROLLBACK', N'0 linhas', N'Existem linhas', N'FALHOU');

/* --- Resultado --------------------------------------------------------------- */
DECLARE @Total INT = (SELECT COUNT(*) FROM @R);
DECLARE @Falhas INT = (SELECT COUNT(*) FROM @R WHERE Resultado <> N'OK');

SELECT Teste, Descricao AS [Descrição], Esperado, Obtido, Resultado
FROM @R
ORDER BY Ordem;

PRINT CONCAT(N'Testes de restrições: ', @Total, N' executados, ', @Total - @Falhas, N' OK, ', @Falhas, N' falhados.');

IF @Falhas > 0
    RAISERROR(N'%d teste(s) de restrições falharam. Ver a coluna Obtido.', 16, 1, @Falhas);
ELSE
    PRINT N'Resultado: todas as restrições se comportam como previsto no Modelo de Dados.';
GO

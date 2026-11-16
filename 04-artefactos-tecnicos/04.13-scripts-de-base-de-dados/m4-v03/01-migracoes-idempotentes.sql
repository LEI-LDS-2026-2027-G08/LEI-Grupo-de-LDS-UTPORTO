-- A coluna calculada persistida de RegistoProfilaxia requer esta opção.
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Clinica] (
        [Id] tinyint NOT NULL,
        [Nome] nvarchar(150) NOT NULL,
        [NIF] char(9) NOT NULL,
        [Morada] nvarchar(250) NOT NULL,
        [Telefone] nvarchar(20) NOT NULL,
        [Email] nvarchar(256) NOT NULL,
        [DataUltimaAlteracao] datetime2(0) NOT NULL,
        CONSTRAINT [PK_Clinica] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Clinica_LinhaUnica] CHECK ([Id] = 1)
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [ConfiguracaoSistema] (
        [Chave] nvarchar(100) NOT NULL,
        [Valor] nvarchar(500) NOT NULL,
        [Descricao] nvarchar(250) NULL,
        [DataUltimaAlteracao] datetime2(0) NOT NULL,
        CONSTRAINT [PK_ConfiguracaoSistema] PRIMARY KEY ([Chave])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Especie] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Especie] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [HorarioFuncionamento] (
        [Id] int NOT NULL IDENTITY,
        [DiaSemana] tinyint NOT NULL,
        [HoraAbertura] time(0) NOT NULL,
        [HoraFecho] time(0) NOT NULL,
        [Ativo] bit NOT NULL CONSTRAINT [DF_HorarioFuncionamento_Ativo] DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_HorarioFuncionamento] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Horario_DiaSemana] CHECK ([DiaSemana] BETWEEN 1 AND 7),
        CONSTRAINT [CK_Horario_Intervalo] CHECK ([HoraFecho] > [HoraAbertura])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Utilizador] (
        [Id] int NOT NULL IDENTITY,
        [Email] nvarchar(256) NOT NULL,
        [PasswordHash] nvarchar(256) NOT NULL,
        [Perfil] nvarchar(25) NOT NULL,
        [Estado] nvarchar(10) NOT NULL CONSTRAINT [DF_Utilizador_Estado] DEFAULT N'Ativo',
        [DataCriacao] datetime2(0) NOT NULL CONSTRAINT [DF_Utilizador_DataCriacao] DEFAULT (SYSUTCDATETIME()),
        [DataUltimoAcesso] datetime2(0) NULL,
        CONSTRAINT [PK_Utilizador] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Utilizador_Estado] CHECK ([Estado] IN ('Ativo','Bloqueado')),
        CONSTRAINT [CK_Utilizador_Perfil] CHECK ([Perfil] IN ('Administrador','Rececionista','Veterinario','AuxiliarVeterinario','Cliente'))
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Raca] (
        [Id] int NOT NULL IDENTITY,
        [EspecieId] int NOT NULL,
        [Nome] nvarchar(80) NOT NULL,
        CONSTRAINT [PK_Raca] PRIMARY KEY ([Id]),
        CONSTRAINT [UQ_Raca_Especie_Id] UNIQUE ([EspecieId], [Id]),
        CONSTRAINT [FK_Raca_Especie_EspecieId] FOREIGN KEY ([EspecieId]) REFERENCES [Especie] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Cliente] (
        [Id] int NOT NULL IDENTITY,
        [UtilizadorId] int NOT NULL,
        [Nome] nvarchar(150) NOT NULL,
        [NIF] char(9) NULL,
        [Telefone] nvarchar(20) NOT NULL,
        [Morada] nvarchar(250) NULL,
        [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_Cliente_DataRegisto] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Cliente] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cliente_Utilizador_UtilizadorId] FOREIGN KEY ([UtilizadorId]) REFERENCES [Utilizador] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Funcionario] (
        [UtilizadorId] int NOT NULL,
        [Nome] nvarchar(150) NOT NULL,
        [Telefone] nvarchar(20) NULL,
        [PodeRegistarApoio] bit NOT NULL CONSTRAINT [DF_Funcionario_PodeRegistarApoio] DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK_Funcionario] PRIMARY KEY ([UtilizadorId]),
        CONSTRAINT [FK_Funcionario_Utilizador_UtilizadorId] FOREIGN KEY ([UtilizadorId]) REFERENCES [Utilizador] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [TokenRevogado] (
        [Jti] nvarchar(64) NOT NULL,
        [UtilizadorId] int NOT NULL,
        [DataExpiracao] datetime2(0) NOT NULL,
        [DataRevogacao] datetime2(0) NOT NULL CONSTRAINT [DF_TokenRevogado_DataRevogacao] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_TokenRevogado] PRIMARY KEY ([Jti]),
        CONSTRAINT [FK_TokenRevogado_Utilizador_UtilizadorId] FOREIGN KEY ([UtilizadorId]) REFERENCES [Utilizador] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Paciente] (
        [Id] int NOT NULL IDENTITY,
        [ClienteId] int NOT NULL,
        [Nome] nvarchar(80) NOT NULL,
        [EspecieId] int NOT NULL,
        [RacaId] int NULL,
        [Sexo] nvarchar(12) NOT NULL CONSTRAINT [DF_Paciente_Sexo] DEFAULT N'Desconhecido',
        [DataNascimento] date NULL,
        [Peso] decimal(6,2) NULL,
        [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_Paciente_DataRegisto] DEFAULT (SYSUTCDATETIME()),
        [DataUltimaAlteracao] datetime2(0) NULL,
        CONSTRAINT [PK_Paciente] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Paciente_Peso] CHECK ([Peso] IS NULL OR [Peso] > 0),
        CONSTRAINT [CK_Paciente_Sexo] CHECK ([Sexo] IN ('Macho','Femea','Desconhecido')),
        CONSTRAINT [FK_Paciente_Cliente_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Cliente] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Paciente_Especie_EspecieId] FOREIGN KEY ([EspecieId]) REFERENCES [Especie] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Paciente_Raca] FOREIGN KEY ([EspecieId], [RacaId]) REFERENCES [Raca] ([EspecieId], [Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Marcacao] (
        [Id] int NOT NULL IDENTITY,
        [PacienteId] int NOT NULL,
        [VeterinarioId] int NOT NULL,
        [DataHoraInicio] datetime2(0) NOT NULL,
        [DuracaoMinutos] smallint NOT NULL,
        [Motivo] nvarchar(250) NULL,
        [Estado] nvarchar(12) NOT NULL CONSTRAINT [DF_Marcacao_Estado] DEFAULT N'Pendente',
        [DataHoraChegada] datetime2(0) NULL,
        [CriadaPorId] int NOT NULL,
        [DataCriacao] datetime2(0) NOT NULL CONSTRAINT [DF_Marcacao_DataCriacao] DEFAULT (SYSUTCDATETIME()),
        [DataUltimaAlteracao] datetime2(0) NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Marcacao] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Marcacao_Duracao] CHECK ([DuracaoMinutos] > 0),
        CONSTRAINT [CK_Marcacao_Estado] CHECK ([Estado] IN ('Pendente','Confirmada','Realizada','Cancelada')),
        CONSTRAINT [FK_Marcacao_Funcionario_CriadaPorId] FOREIGN KEY ([CriadaPorId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Marcacao_Funcionario_VeterinarioId] FOREIGN KEY ([VeterinarioId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Marcacao_Paciente_PacienteId] FOREIGN KEY ([PacienteId]) REFERENCES [Paciente] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [InformacaoApoio] (
        [Id] int NOT NULL IDENTITY,
        [MarcacaoId] int NOT NULL,
        [AuxiliarId] int NOT NULL,
        [Descricao] nvarchar(1000) NOT NULL,
        [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_InformacaoApoio_DataRegisto] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_InformacaoApoio] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_InformacaoApoio_Funcionario_AuxiliarId] FOREIGN KEY ([AuxiliarId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InformacaoApoio_Marcacao_MarcacaoId] FOREIGN KEY ([MarcacaoId]) REFERENCES [Marcacao] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [RegistoClinico] (
        [Id] int NOT NULL IDENTITY,
        [MarcacaoId] int NOT NULL,
        [VeterinarioId] int NOT NULL,
        [NotasClinicas] nvarchar(4000) NULL,
        [Diagnostico] nvarchar(1000) NULL,
        [CriadoEm] datetime2(0) NOT NULL CONSTRAINT [DF_RegistoClinico_CriadoEm] DEFAULT (SYSUTCDATETIME()),
        [AtualizadoEm] datetime2(0) NULL,
        [AtualizadoPorId] int NULL,
        CONSTRAINT [PK_RegistoClinico] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_RegistoClinico_Atualizacao] CHECK (([AtualizadoEm] IS NULL AND [AtualizadoPorId] IS NULL) OR ([AtualizadoEm] IS NOT NULL AND [AtualizadoPorId] IS NOT NULL)),
        CONSTRAINT [FK_RegistoClinico_Funcionario_AtualizadoPorId] FOREIGN KEY ([AtualizadoPorId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RegistoClinico_Funcionario_VeterinarioId] FOREIGN KEY ([VeterinarioId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RegistoClinico_Marcacao_MarcacaoId] FOREIGN KEY ([MarcacaoId]) REFERENCES [Marcacao] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [RegistoProfilaxia] (
        [Id] int NOT NULL IDENTITY,
        [PacienteId] int NOT NULL,
        [VeterinarioId] int NOT NULL,
        [MarcacaoId] int NULL,
        [Tipo] nvarchar(15) NOT NULL,
        [Produto] nvarchar(150) NOT NULL,
        [Lote] nvarchar(50) NOT NULL,
        [DataAdministracao] date NOT NULL,
        [IntervaloDias] smallint NULL,
        [DataProximaAdministracao] AS CASE WHEN [IntervaloDias] IS NULL THEN NULL ELSE DATEADD(DAY, [IntervaloDias], [DataAdministracao]) END PERSISTED,
        [DataRegisto] datetime2(0) NOT NULL CONSTRAINT [DF_RegistoProfilaxia_DataRegisto] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_RegistoProfilaxia] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Profilaxia_Intervalo] CHECK ([IntervaloDias] IS NULL OR [IntervaloDias] > 0),
        CONSTRAINT [CK_Profilaxia_Tipo] CHECK ([Tipo] IN ('Vacina','Desparasitacao')),
        CONSTRAINT [FK_RegistoProfilaxia_Funcionario_VeterinarioId] FOREIGN KEY ([VeterinarioId]) REFERENCES [Funcionario] ([UtilizadorId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RegistoProfilaxia_Marcacao_MarcacaoId] FOREIGN KEY ([MarcacaoId]) REFERENCES [Marcacao] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RegistoProfilaxia_Paciente_PacienteId] FOREIGN KEY ([PacienteId]) REFERENCES [Paciente] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Prescricao] (
        [Id] int NOT NULL IDENTITY,
        [RegistoClinicoId] int NOT NULL,
        [Medicamento] nvarchar(150) NOT NULL,
        [Dosagem] nvarchar(100) NOT NULL,
        [Frequencia] nvarchar(100) NOT NULL,
        [DuracaoDias] smallint NULL,
        CONSTRAINT [PK_Prescricao] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Prescricao_Duracao] CHECK ([DuracaoDias] IS NULL OR [DuracaoDias] > 0),
        CONSTRAINT [FK_Prescricao_RegistoClinico_RegistoClinicoId] FOREIGN KEY ([RegistoClinicoId]) REFERENCES [RegistoClinico] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE TABLE [Tratamento] (
        [Id] int NOT NULL IDENTITY,
        [RegistoClinicoId] int NOT NULL,
        [Descricao] nvarchar(1000) NOT NULL,
        [DataAplicacao] date NOT NULL,
        CONSTRAINT [PK_Tratamento] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tratamento_RegistoClinico_RegistoClinicoId] FOREIGN KEY ([RegistoClinicoId]) REFERENCES [RegistoClinico] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Cliente_Nome] ON [Cliente] ([Nome]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UQ_Cliente_NIF] ON [Cliente] ([NIF]) WHERE [NIF] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [UQ_Cliente_UtilizadorId] ON [Cliente] ([UtilizadorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [UQ_Especie_Nome] ON [Especie] ([Nome]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_InformacaoApoio_AuxiliarId] ON [InformacaoApoio] ([AuxiliarId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_InformacaoApoio_MarcacaoId] ON [InformacaoApoio] ([MarcacaoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Marcacao_CriadaPorId] ON [Marcacao] ([CriadaPorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Marcacao_Data] ON [Marcacao] ([DataHoraInicio]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Marcacao_Paciente_Data] ON [Marcacao] ([PacienteId], [DataHoraInicio]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Marcacao_Veterinario_Data] ON [Marcacao] ([VeterinarioId], [DataHoraInicio]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Paciente_ClienteId] ON [Paciente] ([ClienteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Paciente_EspecieId_RacaId] ON [Paciente] ([EspecieId], [RacaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Paciente_Nome] ON [Paciente] ([Nome]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Prescricao_Registo] ON [Prescricao] ([RegistoClinicoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [UQ_Raca_Especie_Nome] ON [Raca] ([EspecieId], [Nome]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_RegistoClinico_AtualizadoPorId] ON [RegistoClinico] ([AtualizadoPorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_RegistoClinico_VeterinarioId] ON [RegistoClinico] ([VeterinarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [UQ_RegistoClinico_Marcacao] ON [RegistoClinico] ([MarcacaoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Profilaxia_Paciente_Data] ON [RegistoProfilaxia] ([PacienteId], [DataAdministracao]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_RegistoProfilaxia_MarcacaoId] ON [RegistoProfilaxia] ([MarcacaoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_RegistoProfilaxia_VeterinarioId] ON [RegistoProfilaxia] ([VeterinarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_TokenRevogado_Utilizador] ON [TokenRevogado] ([UtilizadorId], [DataExpiracao]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE INDEX [IX_Tratamento_Registo] ON [Tratamento] ([RegistoClinicoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [UQ_Utilizador_Email] ON [Utilizador] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025165126_Inicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261025165126_Inicial', N'10.0.13');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025184243_I062DadosReferencia'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataUltimaAlteracao', N'Email', N'Morada', N'NIF', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clinica]'))
        SET IDENTITY_INSERT [Clinica] ON;
    EXEC(N'INSERT INTO [Clinica] ([Id], [DataUltimaAlteracao], [Email], [Morada], [NIF], [Nome], [Telefone])
    VALUES (CAST(1 AS tinyint), ''2026-10-25T00:00:00'', N''geral@clinica-g08.test'', N''Rua do Exemplo, 100, 4470-000 Maia'', ''501234560'', N''Clínica Veterinária G08'', N''229000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DataUltimaAlteracao', N'Email', N'Morada', N'NIF', N'Nome', N'Telefone') AND [object_id] = OBJECT_ID(N'[Clinica]'))
        SET IDENTITY_INSERT [Clinica] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025184243_I062DadosReferencia'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Chave', N'DataUltimaAlteracao', N'Descricao', N'Valor') AND [object_id] = OBJECT_ID(N'[ConfiguracaoSistema]'))
        SET IDENTITY_INSERT [ConfiguracaoSistema] ON;
    EXEC(N'INSERT INTO [ConfiguracaoSistema] ([Chave], [DataUltimaAlteracao], [Descricao], [Valor])
    VALUES (N''DuracaoPadraoConsultaMinutos'', ''2026-10-25T00:00:00'', N''Duração por omissão de uma consulta, em minutos (EP-009 e EP-010).'', N''30'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Chave', N'DataUltimaAlteracao', N'Descricao', N'Valor') AND [object_id] = OBJECT_ID(N'[ConfiguracaoSistema]'))
        SET IDENTITY_INSERT [ConfiguracaoSistema] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025184243_I062DadosReferencia'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Especie]'))
        SET IDENTITY_INSERT [Especie] ON;
    EXEC(N'INSERT INTO [Especie] ([Id], [Nome])
    VALUES (1, N''Cão''),
    (2, N''Gato'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Especie]'))
        SET IDENTITY_INSERT [Especie] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025184243_I062DadosReferencia'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'EspecieId', N'Nome') AND [object_id] = OBJECT_ID(N'[Raca]'))
        SET IDENTITY_INSERT [Raca] ON;
    EXEC(N'INSERT INTO [Raca] ([Id], [EspecieId], [Nome])
    VALUES (1, 1, N''Labrador Retriever''),
    (2, 1, N''Pastor Alemão''),
    (3, 1, N''Sem raça definida''),
    (4, 2, N''Europeu Comum''),
    (5, 2, N''Siamês'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'EspecieId', N'Nome') AND [object_id] = OBJECT_ID(N'[Raca]'))
        SET IDENTITY_INSERT [Raca] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261025184243_I062DadosReferencia'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261025184243_I062DadosReferencia', N'10.0.13');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261113092251_ObservacaoEMotivoBloqueio'
)
BEGIN
    ALTER TABLE [Utilizador] ADD [MotivoBloqueio] nvarchar(500) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261113092251_ObservacaoEMotivoBloqueio'
)
BEGIN
    ALTER TABLE [Marcacao] ADD [Observacao] nvarchar(500) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261113092251_ObservacaoEMotivoBloqueio'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261113092251_ObservacaoEMotivoBloqueio', N'10.0.13');
END;

COMMIT;
GO


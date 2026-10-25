using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clinica",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NIF = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinica", x => x.Id);
                    table.CheckConstraint("CK_Clinica_LinhaUnica", "[Id] = 1");
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoSistema",
                columns: table => new
                {
                    Chave = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoSistema", x => x.Chave);
                });

            migrationBuilder.CreateTable(
                name: "Especie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorarioFuncionamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaSemana = table.Column<byte>(type: "tinyint", nullable: false),
                    HoraAbertura = table.Column<TimeOnly>(type: "time(0)", nullable: false),
                    HoraFecho = table.Column<TimeOnly>(type: "time(0)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                        .Annotation("Relational:DefaultConstraintName", "DF_HorarioFuncionamento_Ativo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioFuncionamento", x => x.Id);
                    table.CheckConstraint("CK_Horario_DiaSemana", "[DiaSemana] BETWEEN 1 AND 7");
                    table.CheckConstraint("CK_Horario_Intervalo", "[HoraFecho] > [HoraAbertura]");
                });

            migrationBuilder.CreateTable(
                name: "Utilizador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Perfil = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "Ativo")
                        .Annotation("Relational:DefaultConstraintName", "DF_Utilizador_Estado"),
                    DataCriacao = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Utilizador_DataCriacao"),
                    DataUltimoAcesso = table.Column<DateTime>(type: "datetime2(0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador", x => x.Id);
                    table.CheckConstraint("CK_Utilizador_Estado", "[Estado] IN ('Ativo','Bloqueado')");
                    table.CheckConstraint("CK_Utilizador_Perfil", "[Perfil] IN ('Administrador','Rececionista','Veterinario','AuxiliarVeterinario','Cliente')");
                });

            migrationBuilder.CreateTable(
                name: "Raca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EspecieId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raca", x => x.Id);
                    table.UniqueConstraint("UQ_Raca_Especie_Id", x => new { x.EspecieId, x.Id });
                    table.ForeignKey(
                        name: "FK_Raca_Especie_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NIF = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DataRegisto = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Cliente_DataRegisto")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cliente_Utilizador_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Funcionario",
                columns: table => new
                {
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PodeRegistarApoio = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("Relational:DefaultConstraintName", "DF_Funcionario_PodeRegistarApoio")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.UtilizadorId);
                    table.ForeignKey(
                        name: "FK_Funcionario_Utilizador_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TokenRevogado",
                columns: table => new
                {
                    Jti = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    DataRevogacao = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_TokenRevogado_DataRevogacao")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenRevogado", x => x.Jti);
                    table.ForeignKey(
                        name: "FK_TokenRevogado_Utilizador_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EspecieId = table.Column<int>(type: "int", nullable: false),
                    RacaId = table.Column<int>(type: "int", nullable: true),
                    Sexo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false, defaultValue: "Desconhecido")
                        .Annotation("Relational:DefaultConstraintName", "DF_Paciente_Sexo"),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: true),
                    Peso = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    DataRegisto = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Paciente_DataRegisto"),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2(0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.Id);
                    table.CheckConstraint("CK_Paciente_Peso", "[Peso] IS NULL OR [Peso] > 0");
                    table.CheckConstraint("CK_Paciente_Sexo", "[Sexo] IN ('Macho','Femea','Desconhecido')");
                    table.ForeignKey(
                        name: "FK_Paciente_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paciente_Especie_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paciente_Raca",
                        columns: x => new { x.EspecieId, x.RacaId },
                        principalTable: "Raca",
                        principalColumns: new[] { "EspecieId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Marcacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    VeterinarioId = table.Column<int>(type: "int", nullable: false),
                    DataHoraInicio = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    DuracaoMinutos = table.Column<short>(type: "smallint", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false, defaultValue: "Pendente")
                        .Annotation("Relational:DefaultConstraintName", "DF_Marcacao_Estado"),
                    DataHoraChegada = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    CriadaPorId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Marcacao_DataCriacao"),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcacao", x => x.Id);
                    table.CheckConstraint("CK_Marcacao_Duracao", "[DuracaoMinutos] > 0");
                    table.CheckConstraint("CK_Marcacao_Estado", "[Estado] IN ('Pendente','Confirmada','Realizada','Cancelada')");
                    table.ForeignKey(
                        name: "FK_Marcacao_Funcionario_CriadaPorId",
                        column: x => x.CriadaPorId,
                        principalTable: "Funcionario",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Marcacao_Funcionario_VeterinarioId",
                        column: x => x.VeterinarioId,
                        principalTable: "Funcionario",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Marcacao_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InformacaoApoio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarcacaoId = table.Column<int>(type: "int", nullable: false),
                    AuxiliarId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataRegisto = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_InformacaoApoio_DataRegisto")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformacaoApoio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InformacaoApoio_Funcionario_AuxiliarId",
                        column: x => x.AuxiliarId,
                        principalTable: "Funcionario",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InformacaoApoio_Marcacao_MarcacaoId",
                        column: x => x.MarcacaoId,
                        principalTable: "Marcacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistoClinico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarcacaoId = table.Column<int>(type: "int", nullable: false),
                    VeterinarioId = table.Column<int>(type: "int", nullable: false),
                    NotasClinicas = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Diagnostico = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_RegistoClinico_CriadoEm"),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    AtualizadoPorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistoClinico", x => x.Id);
                    table.CheckConstraint("CK_RegistoClinico_Atualizacao", "([AtualizadoEm] IS NULL AND [AtualizadoPorId] IS NULL) OR ([AtualizadoEm] IS NOT NULL AND [AtualizadoPorId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_RegistoClinico_Funcionario_AtualizadoPorId",
                        column: x => x.AtualizadoPorId,
                        principalTable: "Funcionario",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistoClinico_Funcionario_VeterinarioId",
                        column: x => x.VeterinarioId,
                        principalTable: "Funcionario",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistoClinico_Marcacao_MarcacaoId",
                        column: x => x.MarcacaoId,
                        principalTable: "Marcacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistoProfilaxia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    VeterinarioId = table.Column<int>(type: "int", nullable: false),
                    MarcacaoId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Produto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Lote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataAdministracao = table.Column<DateOnly>(type: "date", nullable: false),
                    IntervaloDias = table.Column<short>(type: "smallint", nullable: true),
                    DataProximaAdministracao = table.Column<DateOnly>(type: "date", nullable: true, computedColumnSql: "CASE WHEN [IntervaloDias] IS NULL THEN NULL ELSE DATEADD(DAY, [IntervaloDias], [DataAdministracao]) END", stored: true),
                    DataRegisto = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                        .Annotation("Relational:DefaultConstraintName", "DF_RegistoProfilaxia_DataRegisto")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistoProfilaxia", x => x.Id);
                    table.CheckConstraint("CK_Profilaxia_Intervalo", "[IntervaloDias] IS NULL OR [IntervaloDias] > 0");
                    table.CheckConstraint("CK_Profilaxia_Tipo", "[Tipo] IN ('Vacina','Desparasitacao')");
                    table.ForeignKey(
                        name: "FK_RegistoProfilaxia_Funcionario_VeterinarioId",
                        column: x => x.VeterinarioId,
                        principalTable: "Funcionario",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistoProfilaxia_Marcacao_MarcacaoId",
                        column: x => x.MarcacaoId,
                        principalTable: "Marcacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistoProfilaxia_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescricao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistoClinicoId = table.Column<int>(type: "int", nullable: false),
                    Medicamento = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Dosagem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Frequencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DuracaoDias = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescricao", x => x.Id);
                    table.CheckConstraint("CK_Prescricao_Duracao", "[DuracaoDias] IS NULL OR [DuracaoDias] > 0");
                    table.ForeignKey(
                        name: "FK_Prescricao_RegistoClinico_RegistoClinicoId",
                        column: x => x.RegistoClinicoId,
                        principalTable: "RegistoClinico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tratamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistoClinicoId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataAplicacao = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tratamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tratamento_RegistoClinico_RegistoClinicoId",
                        column: x => x.RegistoClinicoId,
                        principalTable: "RegistoClinico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Nome",
                table: "Cliente",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "UQ_Cliente_NIF",
                table: "Cliente",
                column: "NIF",
                unique: true,
                filter: "[NIF] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Cliente_UtilizadorId",
                table: "Cliente",
                column: "UtilizadorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Especie_Nome",
                table: "Especie",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InformacaoApoio_AuxiliarId",
                table: "InformacaoApoio",
                column: "AuxiliarId");

            migrationBuilder.CreateIndex(
                name: "IX_InformacaoApoio_MarcacaoId",
                table: "InformacaoApoio",
                column: "MarcacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcacao_CriadaPorId",
                table: "Marcacao",
                column: "CriadaPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Marcacao_Data",
                table: "Marcacao",
                column: "DataHoraInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Marcacao_Paciente_Data",
                table: "Marcacao",
                columns: new[] { "PacienteId", "DataHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Marcacao_Veterinario_Data",
                table: "Marcacao",
                columns: new[] { "VeterinarioId", "DataHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_ClienteId",
                table: "Paciente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_EspecieId_RacaId",
                table: "Paciente",
                columns: new[] { "EspecieId", "RacaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_Nome",
                table: "Paciente",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Prescricao_Registo",
                table: "Prescricao",
                column: "RegistoClinicoId");

            migrationBuilder.CreateIndex(
                name: "UQ_Raca_Especie_Nome",
                table: "Raca",
                columns: new[] { "EspecieId", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistoClinico_AtualizadoPorId",
                table: "RegistoClinico",
                column: "AtualizadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistoClinico_VeterinarioId",
                table: "RegistoClinico",
                column: "VeterinarioId");

            migrationBuilder.CreateIndex(
                name: "UQ_RegistoClinico_Marcacao",
                table: "RegistoClinico",
                column: "MarcacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profilaxia_Paciente_Data",
                table: "RegistoProfilaxia",
                columns: new[] { "PacienteId", "DataAdministracao" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistoProfilaxia_MarcacaoId",
                table: "RegistoProfilaxia",
                column: "MarcacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistoProfilaxia_VeterinarioId",
                table: "RegistoProfilaxia",
                column: "VeterinarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenRevogado_Utilizador",
                table: "TokenRevogado",
                columns: new[] { "UtilizadorId", "DataExpiracao" });

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_Registo",
                table: "Tratamento",
                column: "RegistoClinicoId");

            migrationBuilder.CreateIndex(
                name: "UQ_Utilizador_Email",
                table: "Utilizador",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clinica");

            migrationBuilder.DropTable(
                name: "ConfiguracaoSistema");

            migrationBuilder.DropTable(
                name: "HorarioFuncionamento");

            migrationBuilder.DropTable(
                name: "InformacaoApoio");

            migrationBuilder.DropTable(
                name: "Prescricao");

            migrationBuilder.DropTable(
                name: "RegistoProfilaxia");

            migrationBuilder.DropTable(
                name: "TokenRevogado");

            migrationBuilder.DropTable(
                name: "Tratamento");

            migrationBuilder.DropTable(
                name: "RegistoClinico");

            migrationBuilder.DropTable(
                name: "Marcacao");

            migrationBuilder.DropTable(
                name: "Funcionario");

            migrationBuilder.DropTable(
                name: "Paciente");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Raca");

            migrationBuilder.DropTable(
                name: "Utilizador");

            migrationBuilder.DropTable(
                name: "Especie");
        }
    }
}

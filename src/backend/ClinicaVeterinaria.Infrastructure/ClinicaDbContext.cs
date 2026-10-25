using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Domain.Entities;
namespace ClinicaVeterinaria.Infrastructure;

public class ClinicaDbContext : DbContext
{
    public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Clinica> Clinica { get; set; }
    public DbSet<ConfiguracaoSistema> ConfiguracaoSistema { get; set; }
    public DbSet<Especie> Especies { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<HorarioFuncionamento> HorariosFuncionamento { get; set; }
    public DbSet<InformacaoApoio> InformacoesApoio { get; set; }
    public DbSet<Marcacao> Marcacoes { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Prescricao> Prescricoes { get; set; }
    public DbSet<Raca> Racas { get; set; }
    public DbSet<RegistoClinico> RegistosClinicos { get; set; }
    public DbSet<RegistoProfilaxia> RegistosProfilaxias { get; set; }
    public DbSet<TokenRevogado> TokensRevogados { get; set; }
    public DbSet<Tratamento> Tratamentos { get; set; }
    public DbSet<Utilizador> Utilizadores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 9.4 (DI-08)
        modelBuilder.UseCollation("Latin1_General_100_CI_AS");

        // 7.1 Utilizador
        modelBuilder.Entity<Utilizador>(e =>
        {
            e.ToTable("Utilizador", t =>
            {
                t.HasCheckConstraint("CK_Utilizador_Perfil",
                    "[Perfil] IN ('Administrador','Rececionista','Veterinario','AuxiliarVeterinario','Cliente')");
                t.HasCheckConstraint("CK_Utilizador_Estado", "[Estado] IN ('Ativo','Bloqueado')");
            });
            e.HasKey(x => x.Id).HasName("PK_Utilizador");
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();
            e.Property(x => x.Perfil).HasConversion<string>().HasMaxLength(25).IsRequired();
            e.Property(x => x.Estado).HasConversion<string>().HasMaxLength(10).IsRequired()
                .HasDefaultValue("Ativo", "DF_Utilizador_Estado");
            e.Property(x => x.DataCriacao).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_Utilizador_DataCriacao");
            e.Property(x => x.DataUltimoAcesso).HasColumnType("datetime2(0)");
            e.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UQ_Utilizador_Email");
        });

        // 7.2 TokenRevogado
        modelBuilder.Entity<TokenRevogado>(e =>
        {
            e.ToTable("TokenRevogado");
            e.HasKey(x => x.Jti).HasName("PK_TokenRevogado");
            e.Property(x => x.Jti).HasMaxLength(64);
            e.Property(x => x.DataExpiracao).HasColumnType("datetime2(0)");
            e.Property(x => x.DataRevogacao).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_TokenRevogado_DataRevogacao");
            e.HasOne<Utilizador>().WithMany().HasForeignKey(x => x.UtilizadorId)
                .HasConstraintName("FK_TokenRevogado_Utilizador_UtilizadorId");
            e.HasIndex(x => new { x.UtilizadorId, x.DataExpiracao })
                .HasDatabaseName("IX_TokenRevogado_Utilizador");
        });

        // 7.3 Funcionario (PK = FK para Utilizador)
        modelBuilder.Entity<Funcionario>(e =>
        {
            e.ToTable("Funcionario");
            e.HasKey(x => x.UtilizadorId).HasName("PK_Funcionario");
            e.Property(x => x.UtilizadorId).ValueGeneratedNever();
            e.Property(x => x.Nome).HasMaxLength(150).IsRequired();
            e.Property(x => x.Telefone).HasMaxLength(20);
            e.Property(x => x.PodeRegistarApoio)
                .HasDefaultValue(false, "DF_Funcionario_PodeRegistarApoio");
            e.HasOne<Utilizador>().WithOne().HasForeignKey<Funcionario>(x => x.UtilizadorId)
                .HasConstraintName("FK_Funcionario_Utilizador_UtilizadorId");
        });

        // 7.4 Clinica (linha única)
        modelBuilder.Entity<Clinica>(e =>
        {
            e.ToTable("Clinica", t => t.HasCheckConstraint("CK_Clinica_LinhaUnica", "[Id] = 1"));
            e.HasKey(x => x.Id).HasName("PK_Clinica");
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedNever();
            e.Property(x => x.Nome).HasMaxLength(150).IsRequired();
            e.Property(x => x.NIF).HasMaxLength(9).IsFixedLength().IsUnicode(false).IsRequired();
            e.Property(x => x.Morada).HasMaxLength(250).IsRequired();
            e.Property(x => x.Telefone).HasMaxLength(20).IsRequired();
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.DataUltimaAlteracao).HasColumnType("datetime2(0)"); // sem DF (DI-10)
        });

        // 7.5 ConfiguracaoSistema (chave natural)
        modelBuilder.Entity<ConfiguracaoSistema>(e =>
        {
            e.ToTable("ConfiguracaoSistema");
            e.HasKey(x => x.Chave).HasName("PK_ConfiguracaoSistema");
            e.Property(x => x.Chave).HasMaxLength(100);
            e.Property(x => x.Valor).HasMaxLength(500).IsRequired();
            e.Property(x => x.Descricao).HasMaxLength(250);
            e.Property(x => x.DataUltimaAlteracao).HasColumnType("datetime2(0)"); // sem DF (DI-10)
        });

        // 7.6 Cliente
        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("Cliente");
            e.HasKey(x => x.Id).HasName("PK_Cliente");
            e.Property(x => x.Nome).HasMaxLength(150).IsRequired();
            e.Property(x => x.NIF).HasMaxLength(9).IsFixedLength().IsUnicode(false);
            e.Property(x => x.Telefone).HasMaxLength(20).IsRequired();
            e.Property(x => x.Morada).HasMaxLength(250);
            e.Property(x => x.DataRegisto).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_Cliente_DataRegisto");
            e.HasOne<Utilizador>().WithOne().HasForeignKey<Cliente>(x => x.UtilizadorId)
                .HasConstraintName("FK_Cliente_Utilizador_UtilizadorId");
            e.HasIndex(x => x.UtilizadorId).IsUnique().HasDatabaseName("UQ_Cliente_UtilizadorId");
            e.HasIndex(x => x.NIF).IsUnique().HasFilter("[NIF] IS NOT NULL")
                .HasDatabaseName("UQ_Cliente_NIF");
            e.HasIndex(x => x.Nome).HasDatabaseName("IX_Cliente_Nome");
        });

        // 7.7 Especie
        modelBuilder.Entity<Especie>(e =>
        {
            e.ToTable("Especie");
            e.HasKey(x => x.Id).HasName("PK_Especie");
            e.Property(x => x.Nome).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Nome).IsUnique().HasDatabaseName("UQ_Especie_Nome");
        });

        // 7.8 Raca
        modelBuilder.Entity<Raca>(e =>
        {
            e.ToTable("Raca");
            e.HasKey(x => x.Id).HasName("PK_Raca");
            e.Property(x => x.Nome).HasMaxLength(80).IsRequired();
            // Chave alternativa para a FK composta de Paciente
            e.HasAlternateKey(x => new { x.EspecieId, x.Id }).HasName("UQ_Raca_Especie_Id");
            e.HasIndex(x => new { x.EspecieId, x.Nome }).IsUnique().HasDatabaseName("UQ_Raca_Especie_Nome");
            e.HasOne<Especie>().WithMany().HasForeignKey(x => x.EspecieId)
                .HasConstraintName("FK_Raca_Especie_EspecieId");
        });

        // 7.9 Paciente
        modelBuilder.Entity<Paciente>(e =>
        {
            e.ToTable("Paciente", t =>
            {
                t.HasCheckConstraint("CK_Paciente_Sexo", "[Sexo] IN ('Macho','Femea','Desconhecido')");
                t.HasCheckConstraint("CK_Paciente_Peso", "[Peso] IS NULL OR [Peso] > 0");
            });
            e.HasKey(x => x.Id).HasName("PK_Paciente");
            e.Property(x => x.Nome).HasMaxLength(80).IsRequired();
            e.Property(x => x.Sexo).HasConversion<string>().HasMaxLength(12).IsRequired()
                .HasDefaultValue("Desconhecido", "DF_Paciente_Sexo");
            e.Property(x => x.DataNascimento).HasColumnType("date");
            e.Property(x => x.Peso).HasPrecision(6, 2);
            e.Property(x => x.DataRegisto).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_Paciente_DataRegisto");
            e.Property(x => x.DataUltimaAlteracao).HasColumnType("datetime2(0)");
            e.HasOne<Cliente>().WithMany().HasForeignKey(x => x.ClienteId)
                .HasConstraintName("FK_Paciente_Cliente_ClienteId");
            e.HasOne<Especie>().WithMany().HasForeignKey(x => x.EspecieId)
                .HasConstraintName("FK_Paciente_Especie_EspecieId");
            // FK composta (EspecieId, RacaId) → Raca(EspecieId, Id)
            e.HasOne<Raca>().WithMany()
                .HasForeignKey(x => new { x.EspecieId, x.RacaId })
                .HasPrincipalKey(r => new { r.EspecieId, r.Id })
                .IsRequired(false)
                .HasConstraintName("FK_Paciente_Raca");
            e.HasIndex(x => x.ClienteId).HasDatabaseName("IX_Paciente_ClienteId");
            e.HasIndex(x => new { x.EspecieId, x.RacaId }).HasDatabaseName("IX_Paciente_EspecieId_RacaId");
            e.HasIndex(x => x.Nome).HasDatabaseName("IX_Paciente_Nome");
        });

        // 7.10 HorarioFuncionamento (DiaSemana é byte 1..7)
        modelBuilder.Entity<HorarioFuncionamento>(e =>
        {
            e.ToTable("HorarioFuncionamento", t =>
            {
                t.HasCheckConstraint("CK_Horario_DiaSemana", "[DiaSemana] BETWEEN 1 AND 7");
                t.HasCheckConstraint("CK_Horario_Intervalo", "[HoraFecho] > [HoraAbertura]");
            });
            e.HasKey(x => x.Id).HasName("PK_HorarioFuncionamento");
            e.Property(x => x.HoraAbertura).HasColumnType("time(0)");
            e.Property(x => x.HoraFecho).HasColumnType("time(0)");
            e.Property(x => x.Ativo).HasDefaultValue(true, "DF_HorarioFuncionamento_Ativo");
        });

        // 7.11 Marcacao
        modelBuilder.Entity<Marcacao>(e =>
        {
            e.ToTable("Marcacao", t =>
            {
                t.HasCheckConstraint("CK_Marcacao_Estado",
                    "[Estado] IN ('Pendente','Confirmada','Realizada','Cancelada')");
                t.HasCheckConstraint("CK_Marcacao_Duracao", "[DuracaoMinutos] > 0");
            });
            e.HasKey(x => x.Id).HasName("PK_Marcacao");
            e.Property(x => x.DataHoraInicio).HasColumnType("datetime2(0)");
            e.Property(x => x.Motivo).HasMaxLength(250);
            e.Property(x => x.Estado).HasConversion<string>().HasMaxLength(12).IsRequired()
                .HasDefaultValue("Pendente", "DF_Marcacao_Estado");
            e.Property(x => x.DataHoraChegada).HasColumnType("datetime2(0)");
            e.Property(x => x.DataCriacao).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_Marcacao_DataCriacao");
            e.Property(x => x.DataUltimaAlteracao).HasColumnType("datetime2(0)");
            e.Property(x => x.RowVersion).IsRowVersion(); // 9.2
            e.HasOne<Paciente>().WithMany().HasForeignKey(x => x.PacienteId)
                .HasConstraintName("FK_Marcacao_Paciente_PacienteId");
            e.HasOne<Funcionario>().WithMany().HasForeignKey(x => x.VeterinarioId)
                .HasConstraintName("FK_Marcacao_Funcionario_VeterinarioId");
            e.HasOne<Funcionario>().WithMany().HasForeignKey(x => x.CriadaPorId)
                .HasConstraintName("FK_Marcacao_Funcionario_CriadaPorId");
            e.HasIndex(x => new { x.VeterinarioId, x.DataHoraInicio }).HasDatabaseName("IX_Marcacao_Veterinario_Data");
            e.HasIndex(x => new { x.PacienteId, x.DataHoraInicio }).HasDatabaseName("IX_Marcacao_Paciente_Data");
            e.HasIndex(x => x.DataHoraInicio).HasDatabaseName("IX_Marcacao_Data");
            e.HasIndex(x => x.CriadaPorId).HasDatabaseName("IX_Marcacao_CriadaPorId");
        });

        // 7.12 InformacaoApoio
        modelBuilder.Entity<InformacaoApoio>(e =>
        {
            e.ToTable("InformacaoApoio");
            e.HasKey(x => x.Id).HasName("PK_InformacaoApoio");
            e.Property(x => x.Descricao).HasMaxLength(1000).IsRequired();
            e.Property(x => x.DataRegisto).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_InformacaoApoio_DataRegisto");
            e.HasOne<Marcacao>().WithMany().HasForeignKey(x => x.MarcacaoId)
                .HasConstraintName("FK_InformacaoApoio_Marcacao_MarcacaoId");
            e.HasOne<Funcionario>().WithMany().HasForeignKey(x => x.AuxiliarId)
                .HasConstraintName("FK_InformacaoApoio_Funcionario_AuxiliarId");
            e.HasIndex(x => x.MarcacaoId).HasDatabaseName("IX_InformacaoApoio_MarcacaoId");
            e.HasIndex(x => x.AuxiliarId).HasDatabaseName("IX_InformacaoApoio_AuxiliarId");
        });

        // 7.13 RegistoClinico
        modelBuilder.Entity<RegistoClinico>(e =>
        {
            e.ToTable("RegistoClinico", t => t.HasCheckConstraint("CK_RegistoClinico_Atualizacao",
                "([AtualizadoEm] IS NULL AND [AtualizadoPorId] IS NULL) OR ([AtualizadoEm] IS NOT NULL AND [AtualizadoPorId] IS NOT NULL)"));
            e.HasKey(x => x.Id).HasName("PK_RegistoClinico");
            e.Property(x => x.NotasClinicas).HasMaxLength(4000);
            e.Property(x => x.Diagnostico).HasMaxLength(1000);
            e.Property(x => x.CriadoEm).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_RegistoClinico_CriadoEm");
            e.Property(x => x.AtualizadoEm).HasColumnType("datetime2(0)");
            e.HasOne<Marcacao>().WithOne().HasForeignKey<RegistoClinico>(x => x.MarcacaoId)
                .HasConstraintName("FK_RegistoClinico_Marcacao_MarcacaoId");
            e.HasOne<Funcionario>().WithMany().HasForeignKey(x => x.VeterinarioId)
                .HasConstraintName("FK_RegistoClinico_Funcionario_VeterinarioId");
            e.HasOne<Funcionario>().WithMany().HasForeignKey(x => x.AtualizadoPorId)
                .IsRequired(false)
                .HasConstraintName("FK_RegistoClinico_Funcionario_AtualizadoPorId");
            e.HasIndex(x => x.MarcacaoId).IsUnique().HasDatabaseName("UQ_RegistoClinico_Marcacao");
            e.HasIndex(x => x.VeterinarioId).HasDatabaseName("IX_RegistoClinico_VeterinarioId");
            e.HasIndex(x => x.AtualizadoPorId).HasDatabaseName("IX_RegistoClinico_AtualizadoPorId");
        });

        // 7.14 Tratamento
        modelBuilder.Entity<Tratamento>(e =>
        {
            e.ToTable("Tratamento");
            e.HasKey(x => x.Id).HasName("PK_Tratamento");
            e.Property(x => x.Descricao).HasMaxLength(1000).IsRequired();
            e.Property(x => x.DataAplicacao).HasColumnType("date");
            e.HasOne<RegistoClinico>().WithMany().HasForeignKey(x => x.RegistoClinicoId)
                .HasConstraintName("FK_Tratamento_RegistoClinico_RegistoClinicoId");
            e.HasIndex(x => x.RegistoClinicoId).HasDatabaseName("IX_Tratamento_Registo");
        });

        // 7.15 Prescricao
        modelBuilder.Entity<Prescricao>(e =>
        {
            e.ToTable("Prescricao", t => t.HasCheckConstraint("CK_Prescricao_Duracao",
                "[DuracaoDias] IS NULL OR [DuracaoDias] > 0"));
            e.HasKey(x => x.Id).HasName("PK_Prescricao");
            e.Property(x => x.Medicamento).HasMaxLength(150).IsRequired();
            e.Property(x => x.Dosagem).HasMaxLength(100).IsRequired();
            e.Property(x => x.Frequencia).HasMaxLength(100).IsRequired();
            e.HasOne<RegistoClinico>().WithMany().HasForeignKey(x => x.RegistoClinicoId)
                .HasConstraintName("FK_Prescricao_RegistoClinico_RegistoClinicoId");
            e.HasIndex(x => x.RegistoClinicoId).HasDatabaseName("IX_Prescricao_Registo");
        });

        // 7.16 RegistoProfilaxia
        modelBuilder.Entity<RegistoProfilaxia>(e =>
        {
            e.ToTable("RegistoProfilaxia", t =>
            {
                t.HasCheckConstraint("CK_Profilaxia_Tipo", "[Tipo] IN ('Vacina','Desparasitacao')");
                t.HasCheckConstraint("CK_Profilaxia_Intervalo", "[IntervaloDias] IS NULL OR [IntervaloDias] > 0");
            });
            e.HasKey(x => x.Id).HasName("PK_RegistoProfilaxia");
            e.Property(x => x.Tipo).HasConversion<string>().HasMaxLength(15).IsRequired();
            e.Property(x => x.Produto).HasMaxLength(150).IsRequired();
            e.Property(x => x.Lote).HasMaxLength(50).IsRequired();
            e.Property(x => x.DataAdministracao).HasColumnType("date");
            e.Property(x => x.DataProximaAdministracao).HasColumnType("date")
                .HasComputedColumnSql(
                    "CASE WHEN [IntervaloDias] IS NULL THEN NULL " +
                    "ELSE DATEADD(DAY, [IntervaloDias], [DataAdministracao]) END",
                    stored: true);
            e.Property(x => x.DataRegisto).HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()", "DF_RegistoProfilaxia_DataRegisto");
            e.HasOne<Paciente>().WithMany().HasForeignKey(x => x.PacienteId)
                .HasConstraintName("FK_RegistoProfilaxia_Paciente_PacienteId");
            e.HasOne<Funcionario>().WithMany().HasForeignKey(x => x.VeterinarioId)
                .HasConstraintName("FK_RegistoProfilaxia_Funcionario_VeterinarioId");
            e.HasOne<Marcacao>().WithMany().HasForeignKey(x => x.MarcacaoId)
                .IsRequired(false)
                .HasConstraintName("FK_RegistoProfilaxia_Marcacao_MarcacaoId");
            e.HasIndex(x => new { x.PacienteId, x.DataAdministracao }).HasDatabaseName("IX_Profilaxia_Paciente_Data");
            e.HasIndex(x => x.VeterinarioId).HasDatabaseName("IX_RegistoProfilaxia_VeterinarioId");
            e.HasIndex(x => x.MarcacaoId).HasDatabaseName("IX_RegistoProfilaxia_MarcacaoId");
        });

        // 6.1: NO ACTION em todas as chaves estrangeiras (DeleteBehavior.Restrict, 6.7)
        foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }
}
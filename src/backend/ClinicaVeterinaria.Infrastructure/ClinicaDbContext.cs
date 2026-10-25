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
        modelBuilder.Entity<Funcionario>().HasKey(f => f.UtilizadorId);
        modelBuilder.Entity<TokenRevogado>().HasKey(x => x.Jti);
        modelBuilder.Entity<ConfiguracaoSistema>().HasKey(x => x.Chave);
    }
}

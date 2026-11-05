using System.ComponentModel.DataAnnotations;
using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.DTOs.Comum;
using ClinicaVeterinaria.Application.DTOs.Consultas;
using ClinicaVeterinaria.Application.Exceptions;
using ClinicaVeterinaria.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infrastructure;

/// <summary>
/// EP-030 — marcações futuras e histórico administrativo do Cliente (I076; RF028, RF029). Só lê a
/// marcação, o animal, a espécie e o nome do Veterinário; nunca lê o motivo, as notas de preparação
/// nem o registo clínico. O Cliente vem sempre do token (RNF002).
/// </summary>
public sealed class MinhasConsultasService : IMinhasConsultasService
{
    /// <summary>EP-030 — <c>animalId</c> que não é de um animal do Cliente autenticado (403).</summary>
    public const string MensagemAnimalDeOutroCliente = "O animal indicado não pertence ao Cliente autenticado.";

    private readonly ClinicaDbContext _db;
    private readonly TimeProvider _relogio;

    public MinhasConsultasService(ClinicaDbContext db, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(relogio);

        _db = db;
        _relogio = relogio;
    }

    public async Task<IReadOnlyList<MinhaConsultaDto>> ListarAsync(int clienteId, MinhasConsultasQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        // Mesmas regras da validação automática do pedido (400), para o serviço também as aplicar
        // quando é chamado diretamente.
        Validar(query);

        var tipo = TiposMinhasConsultas.Normalizar(query.Tipo)!;

        // Um animal de outro Cliente dá 403. Um animal que não existe também: o contrato de EP-030
        // não prevê 404 e assim a resposta não revela se o animal existe (RNF002).
        if (query.AnimalId is int animalId
            && !await _db.Pacientes.AsNoTracking().AnyAsync(p => p.Id == animalId && p.ClienteId == clienteId))
        {
            throw new ForbiddenException(MensagemAnimalDeOutroCliente);
        }

        var agora = _relogio.GetUtcNow().UtcDateTime;

        var linhas =
            from m in _db.Marcacoes.AsNoTracking()
            join p in _db.Pacientes.AsNoTracking() on m.PacienteId equals p.Id
            join e in _db.Especies.AsNoTracking() on p.EspecieId equals e.Id
            join f in _db.Funcionarios.AsNoTracking() on m.VeterinarioId equals f.UtilizadorId
            where p.ClienteId == clienteId
            select new
            {
                m.Id,
                m.DataHoraInicio,
                m.Estado,
                AnimalId = p.Id,
                AnimalNome = p.Nome,
                Especie = e.Nome,
                VeterinarioNome = f.Nome
            };

        if (query.AnimalId is int filtroAnimal)
        {
            linhas = linhas.Where(l => l.AnimalId == filtroAnimal);
        }

        // A mesma regra de TiposMinhasConsultas.EFutura, escrita aqui para ser traduzida em SQL.
        linhas = tipo == TiposMinhasConsultas.Futuras
            ? linhas
                .Where(l => l.DataHoraInicio >= agora
                    && (l.Estado == EstadosConsulta.Pendente || l.Estado == EstadosConsulta.Confirmada))
                .OrderBy(l => l.DataHoraInicio)
                .ThenBy(l => l.Id)
            : linhas
                .Where(l => l.DataHoraInicio < agora
                    || (l.Estado != EstadosConsulta.Pendente && l.Estado != EstadosConsulta.Confirmada))
                .OrderByDescending(l => l.DataHoraInicio)
                .ThenByDescending(l => l.Id);

        var resultado = await linhas.ToListAsync();

        return resultado
            .Select(l => new MinhaConsultaDto
            {
                Id = l.Id,
                // As colunas datetime2 guardam UTC (Modelo de Dados, D15), mas são lidas sem fuso.
                DataHora = DateTime.SpecifyKind(l.DataHoraInicio, DateTimeKind.Utc),
                Estado = l.Estado,
                Animal = new AnimalRefDto { Id = l.AnimalId, Nome = l.AnimalNome, Especie = l.Especie },
                Veterinario = l.VeterinarioNome
            })
            .ToList();
    }

    private static void Validar(object pedido)
    {
        var erros = new List<ValidationResult>();

        if (!Validator.TryValidateObject(pedido, new ValidationContext(pedido), erros, validateAllProperties: true))
        {
            throw new ArgumentException(string.Join(" ", erros.Select(e => e.ErrorMessage)));
        }
    }
}

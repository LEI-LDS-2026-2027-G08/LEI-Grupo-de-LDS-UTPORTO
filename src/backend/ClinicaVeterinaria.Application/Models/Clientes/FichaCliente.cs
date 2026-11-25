using ClinicaVeterinaria.Domain.Entities;

namespace ClinicaVeterinaria.Application.Models.Clientes;

public sealed record FichaCliente(Cliente Cliente, string Email, string EstadoConta, IReadOnlyList<AnimalDoCliente> Animais);

namespace ClinicaVeterinaria.Domain.Entities;

public class ConfiguracaoSistema
{
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataUltimaAlteracao { get; set; }
}
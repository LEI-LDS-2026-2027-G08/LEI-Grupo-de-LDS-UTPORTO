using ClinicaVeterinaria.Application.Seguranca;

namespace ClinicaVeterinaria.IntegrationTests.Seguranca;

/// <summary>
/// I096 — tabela de perfis autorizados por endpoint (critério 3), de onde a matriz de segurança é
/// gerada. Conferida com a Documentação de API v02 (linha «Perfis» de cada endpoint) e com a matriz
/// endpoint × perfil da I030 (secção 14.1): os 37 endpoints ativos e a atualização do boletim
/// (RF056, I082), que a API v02 ainda não descreve.
/// </summary>
/// <remarks>
/// Os identificadores nas rotas são de recursos que não existem (999999): o pedido de um perfil
/// autorizado passa a autorização e termina na validação ou no 404, sem alterar dados. O que a
/// matriz verifica é só a decisão de autenticação e de autorização.
/// </remarks>
public static class MatrizPerfis
{
    public const string Inexistente = "999999";

    /// <summary>Uma operação da API e os perfis que a podem usar.</summary>
    /// <param name="Codigo">Identificador na Documentação de API (EP-xxx) ou requisito (RF056).</param>
    /// <param name="Metodo">Método HTTP.</param>
    /// <param name="Rota">Rota pedida, com os identificadores já preenchidos.</param>
    /// <param name="Publica">Não requer JWT (só EP-001).</param>
    /// <param name="Autorizados">Perfis com acesso; os restantes recebem 403.</param>
    public sealed record Operacao(string Codigo, HttpMethod Metodo, string Rota, bool Publica, params string[] Autorizados)
    {
        public bool TemCorpo => Metodo == HttpMethod.Post || Metodo == HttpMethod.Put || Metodo == HttpMethod.Patch;

        public bool Permite(string perfil) => Publica || Autorizados.Contains(perfil);

        public override string ToString() => $"{Codigo} {Metodo} /{Rota}";
    }

    public static readonly IReadOnlyList<string> PerfisInternos =
        [Perfis.Administrador, Perfis.Rececionista, Perfis.Veterinario, Perfis.AuxiliarVeterinario];

    public static readonly IReadOnlyList<string> TodosOsPerfis = [.. PerfisInternos, Perfis.Cliente];

    private const string A = Perfis.Administrador;
    private const string R = Perfis.Rececionista;
    private const string V = Perfis.Veterinario;
    private const string X = Perfis.AuxiliarVeterinario;
    private const string C = Perfis.Cliente;
    private const string Id = Inexistente;

    public static readonly IReadOnlyList<Operacao> Operacoes =
    [

        // F001 — Autenticação, utilizadores, clínica e Clientes
        new("EP-001", HttpMethod.Post, "auth/login", true),
        new("EP-002", HttpMethod.Post, "auth/logout", false, A, R, V, X, C),
        new("EP-003", HttpMethod.Get, "auth/me", false, A, R, V, X, C),
        new("EP-004", HttpMethod.Get, "utilizadores", false, A),
        new("EP-005", HttpMethod.Get, $"utilizadores/{Id}", false, A),
        new("EP-006", HttpMethod.Post, "utilizadores", false, A),
        new("EP-007", HttpMethod.Put, $"utilizadores/{Id}", false, A),
        new("EP-008", HttpMethod.Patch, $"utilizadores/{Id}/estado", false, A),
        new("EP-009", HttpMethod.Get, "clinica", false, A),
        new("EP-010", HttpMethod.Put, "clinica", false, A),
        new("EP-011", HttpMethod.Get, "clientes", false, A, R, V),
        new("EP-012", HttpMethod.Get, $"clientes/{Id}", false, A, R, V),
        new("EP-013", HttpMethod.Post, "clientes", false, A, R),
        new("EP-014", HttpMethod.Put, $"clientes/{Id}", false, A, R),

        // F002 — Pacientes
        new("EP-015", HttpMethod.Get, "animais", false, R, V),
        new("EP-016", HttpMethod.Get, $"animais/{Id}", false, R, V, X),
        new("EP-017", HttpMethod.Post, "animais", false, R, V),
        new("EP-018", HttpMethod.Put, $"animais/{Id}", false, R, V),
        new("EP-019", HttpMethod.Get, "me/animais", false, C),
        new("EP-020", HttpMethod.Get, $"me/animais/{Id}", false, C),

        // F003 — Agendamento de consultas
        new("EP-021", HttpMethod.Get, "veterinarios", false, R),
        new("EP-022", HttpMethod.Get, $"veterinarios/{Id}/disponibilidade", false, R),
        new("EP-023", HttpMethod.Get, "consultas", false, R, V, X),
        new("EP-024", HttpMethod.Get, $"consultas/{Id}", false, R, V, X),
        new("EP-025", HttpMethod.Post, "consultas", false, R),
        new("EP-026", HttpMethod.Put, $"consultas/{Id}", false, R),
        new("EP-027", HttpMethod.Patch, $"consultas/{Id}/estado", false, R),
        new("EP-028", HttpMethod.Patch, $"consultas/{Id}/chegada", false, R),
        new("EP-029", HttpMethod.Post, $"consultas/{Id}/notas-preparacao", false, X),
        new("EP-030", HttpMethod.Get, "me/consultas", false, C),

        // F004 — Registo clínico
        new("EP-031", HttpMethod.Post, "registos-clinicos", false, V),
        new("EP-032", HttpMethod.Put, $"registos-clinicos/{Id}", false, V),
        new("EP-033", HttpMethod.Get, $"registos-clinicos/{Id}", false, V, X),
        new("EP-034", HttpMethod.Get, $"animais/{Id}/historico-clinico", false, V, X),

        // F006 — Boletim de vacinas e desparasitações
        new("EP-042", HttpMethod.Get, $"animais/{Id}/vacinacoes", false, V),
        new("EP-043", HttpMethod.Post, $"animais/{Id}/vacinacoes", false, V),
        new("RF056", HttpMethod.Put, $"animais/{Id}/vacinacoes/{Id}", false, V),
        new("EP-044", HttpMethod.Get, $"me/animais/{Id}/vacinacoes", false, C),
    ];

    public static Operacao Obter(string codigo) => Operacoes.Single(o => o.Codigo == codigo);

    /// <summary>Uma linha por operação, para os testes parametrizados.</summary>
    public static TheoryData<string> Codigos()
    {
        var dados = new TheoryData<string>();
        foreach (var operacao in Operacoes)
        {
            dados.Add(operacao.Codigo);
        }

        return dados;
    }
}

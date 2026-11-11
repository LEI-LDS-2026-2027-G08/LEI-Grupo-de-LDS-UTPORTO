import { act, render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { MemoryRouter } from "react-router-dom";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { ApiClient, ApiRequestError } from "../core/api";
import { SoPara } from "../components/SoPara";
import { AuthProvider } from "../core/auth/AuthContext";
import { TokenStorage } from "../core/auth/TokenStorage";
import type { Perfil } from "../models/auth";
import { resposta, servicoFalso, sessaoDe } from "../test/auxiliares";
import { AppRoutes } from "./AppRoutes";

function montar(
  caminho: string,
  servico = servicoFalso(),
  cliente?: ApiClient,
) {
  const clienteHttp = cliente ?? new ApiClient("http://api.teste", vi.fn());
  render(
    <MemoryRouter initialEntries={[caminho]}>
      <AuthProvider servico={servico} cliente={clienteHttp}>
        <AppRoutes />
      </AuthProvider>
    </MemoryRouter>,
  );
  return { servico, cliente: clienteHttp };
}

async function iniciarSessao(
  perfil: Perfil,
  servico: ReturnType<typeof servicoFalso>,
) {
  servico.login.mockResolvedValue(sessaoDe(perfil));
  const utilizador = userEvent.setup();
  await utilizador.type(screen.getByLabelText("Email"), "maria@clinica.pt");
  await utilizador.type(screen.getByLabelText("Palavra-passe"), "segredo");
  await utilizador.click(screen.getByRole("button", { name: "Entrar" }));
  return utilizador;
}

describe("rotas protegidas", () => {
  beforeEach(() => sessionStorage.clear());

  it("sem sessão, qualquer rota protegida leva ao login", () => {
    montar("/clientes");
    expect(
      screen.getByRole("heading", { name: "Iniciar sessão" }),
    ).toBeInTheDocument();
  });

  it("após o login mostra a página inicial e o menu do perfil", async () => {
    const { servico } = montar("/login");
    await iniciarSessao("Rececionista", servico);

    expect(
      await screen.findByRole("heading", { name: "Agenda" }),
    ).toBeInTheDocument();
    const menu = screen.getByRole("navigation", { name: "Menu principal" });
    expect(menu).toHaveTextContent("Pacientes");
    expect(menu).toHaveTextContent("Clientes");
    expect(menu).not.toHaveTextContent("Funcionários");
    expect(screen.getByText(/Maria Teste/)).toBeInTheDocument();
  });

  it("volta à página pedida depois de iniciar sessão", async () => {
    const { servico } = montar("/clientes");
    await iniciarSessao("Administrador", servico);
    expect(
      await screen.findByRole("heading", { name: "Clientes" }),
    ).toBeInTheDocument();
  });

  it.each([
    ["Rececionista", "/funcionarios/novo"],
    ["Veterinario", "/funcionarios"],
    ["AuxiliarVeterinario", "/funcionarios/5"],
  ] as const)(
    "I109: %s não acede a %s por endereço direto nem vê Funcionários no menu",
    async (perfil, caminho) => {
      const { servico } = montar(caminho);
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Acesso negado" }),
      ).toBeInTheDocument();
      expect(
        screen.getByRole("navigation", { name: "Menu principal" }),
      ).not.toHaveTextContent("Funcionários");
    },
  );

  it.each(["Rececionista", "Veterinario", "AuxiliarVeterinario"] as const)(
    "I110: %s não acede a /clinica por endereço direto nem vê Clínica e horários no menu",
    async (perfil) => {
      const { servico } = montar("/clinica");
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Acesso negado" }),
      ).toBeInTheDocument();
      expect(
        screen.getByRole("navigation", { name: "Menu principal" }),
      ).not.toHaveTextContent("Clínica e horários");
      expect(
        screen.queryByLabelText("Nome da clínica"),
      ).not.toBeInTheDocument();
    },
  );

  it("I110: o Administrador vê o ecrã Clínica e horários e a entrada no menu", async () => {
    const { servico } = montar("/clinica");
    await iniciarSessao("Administrador", servico);
    expect(
      await screen.findByRole("heading", { name: "Clínica e horários" }),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("navigation", { name: "Menu principal" }),
    ).toHaveTextContent("Clínica e horários");
  });

  it("perfil sem permissão numa rota vai para 'Acesso negado'", async () => {
    const { servico } = montar("/funcionarios");
    await iniciarSessao("Rececionista", servico);
    expect(
      await screen.findByRole("heading", { name: "Acesso negado" }),
    ).toBeInTheDocument();
  });

  it("mostra o erro do backend quando as credenciais são inválidas", async () => {
    const servico = servicoFalso();
    const cliente = new ApiClient(
      "http://api.teste",
      vi
        .fn()
        .mockResolvedValue(
          resposta(401, { code: 1, message: "Credenciais inválidas." }),
        ),
    );
    servico.login.mockImplementation(() =>
      cliente.post("/auth/login", {}, { publico: true }),
    );
    montar("/login", servico, cliente);

    const utilizador = userEvent.setup();
    await utilizador.type(screen.getByLabelText("Email"), "a@b.pt");
    await utilizador.type(screen.getByLabelText("Palavra-passe"), "errada");
    await utilizador.click(screen.getByRole("button", { name: "Entrar" }));

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Credenciais inválidas.",
    );
    expect(
      screen.getByRole("heading", { name: "Iniciar sessão" }),
    ).toBeInTheDocument();
  });

  it("recusa Clientes e termina a sessão que o backend abriu", async () => {
    const servico = servicoFalso();
    montar("/login", servico);
    await iniciarSessao("Cliente", servico);

    expect(await screen.findByRole("alert")).toHaveTextContent(
      /aplicação móvel/,
    );
    expect(servico.logout).toHaveBeenCalledTimes(1);
    expect(sessionStorage.length).toBe(0);
  });

  it("terminar sessão volta ao login e limpa o armazenamento", async () => {
    const servico = servicoFalso();
    montar("/login", servico);
    const utilizador = await iniciarSessao("Veterinario", servico);
    await screen.findByRole("heading", { name: "Agenda" });

    await utilizador.click(
      screen.getByRole("button", { name: "Terminar sessão" }),
    );

    expect(
      await screen.findByRole("heading", { name: "Iniciar sessão" }),
    ).toBeInTheDocument();
    expect(servico.logout).toHaveBeenCalled();
    expect(sessionStorage.length).toBe(0);
  });

  it("um 401 em qualquer pedido leva de volta ao login", async () => {
    const servico = servicoFalso();
    const cliente = new ApiClient(
      "http://api.teste",
      vi
        .fn()
        .mockResolvedValue(
          resposta(401, { code: 1, message: "Token expirado." }),
        ),
    );
    montar("/login", servico, cliente);
    await iniciarSessao("Rececionista", servico);
    await screen.findByRole("heading", { name: "Agenda" });

    await act(async () => {
      await cliente.get("/qualquer-coisa").catch(() => undefined);
    });

    expect(
      await screen.findByRole("heading", { name: "Iniciar sessão" }),
    ).toBeInTheDocument();
    // O utilizador percebe porque voltou ao login (critério 4 da I108).
    expect(screen.getByRole("status")).toHaveTextContent(
      "A sua sessão terminou",
    );
    expect(sessionStorage.length).toBe(0);
  });

  it("depois de terminar sessão, o token já não é enviado nos pedidos", async () => {
    const servico = servicoFalso();
    const fetchFalso = vi
      .fn()
      .mockImplementation(() => Promise.resolve(resposta(200, {})));
    const cliente = new ApiClient("http://api.teste", fetchFalso);
    montar("/login", servico, cliente);
    const utilizador = await iniciarSessao("Rececionista", servico);
    await screen.findByRole("heading", { name: "Agenda" });
    await cliente.get("/antes");
    expect(fetchFalso.mock.calls[0][1].headers.Authorization).toBe(
      "Bearer token-secreto-de-teste",
    );

    await utilizador.click(
      screen.getByRole("button", { name: "Terminar sessão" }),
    );
    await screen.findByRole("heading", { name: "Iniciar sessão" });
    await cliente.get("/depois");

    expect(fetchFalso.mock.calls[1][1].headers.Authorization).toBeUndefined();
    // Sem aviso: sair por vontade própria não é "sessão terminada" por expiração.
    expect(screen.queryByText(/A sua sessão terminou/)).not.toBeInTheDocument();
  });

  it("falha de ligação no login mostra a mensagem e não bloqueia o ecrã", async () => {
    const servico = servicoFalso();
    servico.login.mockRejectedValue(new ApiRequestError(0, "rede"));
    montar("/login", servico);

    const utilizador = userEvent.setup();
    await utilizador.type(screen.getByLabelText("Email"), "a@b.pt");
    await utilizador.type(screen.getByLabelText("Palavra-passe"), "x");
    await utilizador.click(screen.getByRole("button", { name: "Entrar" }));

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor",
    );
    expect(screen.getByRole("button", { name: "Entrar" })).toBeEnabled();
  });

  it("SoPara mostra o conteúdo só a quem pode executar a ação", async () => {
    TokenStorage.guardar(sessaoDe("Veterinario"));
    const servico = servicoFalso();
    servico.me.mockResolvedValue(sessaoDe("Veterinario").utilizador);
    render(
      <MemoryRouter>
        <AuthProvider
          servico={servico}
          cliente={new ApiClient("http://api.teste", vi.fn())}
        >
          <SoPara acao="registarVacinacao">
            <p>Botão de vacinação</p>
          </SoPara>
          <SoPara acao="gerirMarcacoes" alternativa={<p>Sem permissão</p>}>
            <p>Botão de marcações</p>
          </SoPara>
        </AuthProvider>
      </MemoryRouter>,
    );

    expect(await screen.findByText("Botão de vacinação")).toBeInTheDocument();
    expect(screen.queryByText("Botão de marcações")).not.toBeInTheDocument();
    expect(screen.getByText("Sem permissão")).toBeInTheDocument();
  });

  it("restaura uma sessão guardada depois de a confirmar com o backend", async () => {
    const sessao = sessaoDe("AuxiliarVeterinario");
    TokenStorage.guardar(sessao);
    const servico = servicoFalso();
    servico.me.mockResolvedValue(sessao.utilizador);

    montar("/preparacao", servico);

    expect(screen.getByRole("status")).toHaveTextContent(
      "A verificar a sessão",
    );
    expect(
      await screen.findByRole("heading", { name: "Preparação de consultas" }),
    ).toBeInTheDocument();
    expect(servico.me).toHaveBeenCalled();
  });

  it("descarta a sessão guardada se o backend a recusar", async () => {
    TokenStorage.guardar(sessaoDe("Rececionista"));
    const servico = servicoFalso();
    servico.me.mockRejectedValue(new Error("401"));

    montar("/agenda", servico);

    expect(
      await screen.findByRole("heading", { name: "Iniciar sessão" }),
    ).toBeInTheDocument();
    await waitFor(() => expect(sessionStorage.length).toBe(0));
  });

  it("rotas desconhecidas mostram 'Página não encontrada' a quem tem sessão", async () => {
    const { servico } = montar("/nao-existe");
    await iniciarSessao("Rececionista", servico);
    expect(
      await screen.findByRole("heading", { name: "Página não encontrada" }),
    ).toBeInTheDocument();
  });
});

describe("I111 — acesso às páginas de Clientes", () => {
  beforeEach(() => sessionStorage.clear());

  it.each([
    ["Veterinario", "/clientes"],
    ["Veterinario", "/clientes/5"],
    ["AuxiliarVeterinario", "/clientes/novo"],
  ] as const)(
    "%s não acede a %s por endereço direto nem vê Clientes no menu",
    async (perfil, caminho) => {
      const { servico } = montar(caminho);
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Acesso negado" }),
      ).toBeInTheDocument();
      expect(
        screen.getByRole("navigation", { name: "Menu principal" }),
      ).not.toHaveTextContent("Clientes");
    },
  );

  it.each(["Administrador", "Rececionista"] as const)(
    "%s abre o formulário de novo cliente",
    async (perfil) => {
      const { servico } = montar("/clientes/novo");
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Novo cliente" }),
      ).toBeInTheDocument();
    },
  );
});

describe("I112 — acesso às páginas de pacientes", () => {
  beforeEach(() => sessionStorage.clear());

  it.each([
    ["AuxiliarVeterinario", "/pacientes"],
    ["AuxiliarVeterinario", "/pacientes/novo"],
    ["Administrador", "/pacientes/3"],
  ] as const)(
    "%s não acede a %s por endereço direto",
    async (perfil, caminho) => {
      const { servico } = montar(caminho);
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Acesso negado" }),
      ).toBeInTheDocument();
    },
  );

  it("o Auxiliar abre a ficha do paciente, mas não vê Pacientes no menu", async () => {
    const { servico } = montar("/pacientes/3");
    await iniciarSessao("AuxiliarVeterinario", servico);
    expect(
      await screen.findByRole("heading", { name: "Ficha do paciente" }),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("navigation", { name: "Menu principal" }),
    ).not.toHaveTextContent("Pacientes");
  });

  it.each(["Rececionista", "Veterinario"] as const)(
    "%s abre o formulário de novo paciente",
    async (perfil) => {
      const { servico } = montar("/pacientes/novo");
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Novo paciente" }),
      ).toBeInTheDocument();
    },
  );
});

describe("I113 — acesso às páginas de marcações", () => {
  beforeEach(() => sessionStorage.clear());

  it.each([
    ["Veterinario", "/agenda/nova"],
    ["Veterinario", "/agenda/41/editar"],
    ["AuxiliarVeterinario", "/agenda/41"],
    ["Administrador", "/agenda/nova"],
  ] as const)(
    "%s não acede a %s por endereço direto",
    async (perfil, caminho) => {
      const { servico } = montar(caminho);
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Acesso negado" }),
      ).toBeInTheDocument();
    },
  );

  it("a Rececionista abre o formulário de nova marcação", async () => {
    const { servico } = montar("/agenda/nova");
    await iniciarSessao("Rececionista", servico);
    expect(
      await screen.findByRole("heading", { name: "Nova marcação" }),
    ).toBeInTheDocument();
  });

  it("o Veterinário abre o detalhe de uma marcação", async () => {
    const { servico } = montar("/agenda/41");
    await iniciarSessao("Veterinario", servico);
    expect(
      await screen.findByRole("heading", { name: "Detalhe da marcação" }),
    ).toBeInTheDocument();
  });
});

describe("I114 — acesso à agenda e à preparação de consultas", () => {
  beforeEach(() => sessionStorage.clear());

  it.each([
    ["AuxiliarVeterinario", "/agenda"],
    ["Administrador", "/agenda"],
    ["Rececionista", "/preparacao"],
    ["Veterinario", "/preparacao"],
  ] as const)(
    "%s não acede a %s por endereço direto",
    async (perfil, caminho) => {
      const { servico } = montar(caminho);
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Acesso negado" }),
      ).toBeInTheDocument();
    },
  );
});

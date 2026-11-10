import { render, screen, waitFor, within } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { describe, expect, it, vi } from "vitest";
import { ApiRequestError } from "../../core/api";
import {
  AuthContext,
  type AuthContextValue,
} from "../../core/auth/contextoAuth";
import { perfilPodeExecutar } from "../../core/auth/permissoes";
import type { PerfilWeb } from "../../models/auth";
import type { ClienteDetalhe } from "../../models/clientes";
import type { ServicoClientes } from "../../services/ClientesService";
import { ClienteFormPage } from "./ClienteFormPage";
import { ClientesPage } from "./ClientesPage";

const carlos: ClienteDetalhe = {
  id: 7,
  nome: "Carlos Sousa",
  nif: "123456789",
  telefone: "912345678",
  email: "carlos@email.pt",
  morada: "Rua A, 1",
  utilizadorId: 30,
  estadoConta: "Ativo",
  animais: [{ id: 3, nome: "Kiko", especie: "Cão" }],
};
const rita = {
  id: 8,
  nome: "Rita Ferreira",
  nif: null,
  telefone: "919876543",
  email: "rita@email.pt",
  numeroAnimais: 0,
};

function servicoFalso(): {
  [K in keyof ServicoClientes]: ReturnType<typeof vi.fn>;
} {
  return {
    listar: vi.fn().mockResolvedValue({
      itens: [{ ...carlos, numeroAnimais: 1 }, rita],
      pagina: 1,
      tamanho: 20,
      total: 2,
    }),
    obter: vi.fn().mockResolvedValue(carlos),
    criar: vi.fn().mockResolvedValue(carlos),
    atualizar: vi.fn().mockResolvedValue(carlos),
  };
}

/** Sessão falsa: só o necessário para o SoPara decidir o que mostrar. */
function sessaoDe(perfil: PerfilWeb): AuthContextValue {
  return {
    sessao: null,
    utilizador: { id: 1, nome: "Maria", email: "maria@clinica.pt", perfil },
    aRestaurar: false,
    login: vi.fn(),
    logout: vi.fn(),
    temPerfil: (perfis) => perfis.includes(perfil),
    pode: (acao) => perfilPodeExecutar(perfil, acao),
    sessaoTerminada: false,
  };
}

function montar(
  caminho: string,
  servico = servicoFalso(),
  perfil: PerfilWeb = "Rececionista",
) {
  const s = servico as unknown as ServicoClientes;
  render(
    <AuthContext.Provider value={sessaoDe(perfil)}>
      <MemoryRouter initialEntries={[caminho]}>
        <Routes>
          <Route path="/clientes" element={<ClientesPage servico={s} />} />
          <Route
            path="/clientes/novo"
            element={<ClienteFormPage servico={s} />}
          />
          <Route
            path="/clientes/:id"
            element={<ClienteFormPage servico={s} />}
          />
        </Routes>
      </MemoryRouter>
    </AuthContext.Provider>,
  );
  return servico;
}

describe("W04 — lista de Clientes", () => {
  it("carrega a lista com NIF, telefone, e-mail e número de animais", async () => {
    const servico = montar("/clientes");
    expect(screen.getByRole("status")).toHaveTextContent("A carregar");
    const linhaCarlos = (await screen.findByText("Carlos Sousa")).closest(
      "tr",
    )!;
    expect(linhaCarlos).toHaveTextContent("123456789");
    expect(linhaCarlos).toHaveTextContent("carlos@email.pt");
    const linhaRita = screen.getByText("Rita Ferreira").closest("tr")!;
    expect(linhaRita).toHaveTextContent("—");
    expect(
      within(linhaRita).getByRole("link", {
        name: "Ver / editar Rita Ferreira",
      }),
    ).toHaveAttribute("href", "/clientes/8");
    expect(screen.getByRole("link", { name: "Novo cliente" })).toHaveAttribute(
      "href",
      "/clientes/novo",
    );
    expect(servico.listar).toHaveBeenCalledWith({ pagina: 1, tamanho: 20 });
  });

  it("pesquisa pelo texto escrito", async () => {
    const servico = montar("/clientes");
    await screen.findByText("Carlos Sousa");
    const u = userEvent.setup();
    await u.type(
      screen.getByLabelText("Pesquisar (nome, NIF, telefone ou e-mail)"),
      " sousa ",
    );
    await u.click(screen.getByRole("button", { name: "Pesquisar" }));
    await waitFor(() =>
      expect(servico.listar).toHaveBeenLastCalledWith({
        pesquisa: "sousa",
        pagina: 1,
        tamanho: 20,
      }),
    );
  });

  it("indica quando a pesquisa não encontra clientes", async () => {
    const servico = servicoFalso();
    montar("/clientes", servico);
    await screen.findByText("Carlos Sousa");
    servico.listar.mockResolvedValue({
      itens: [],
      pagina: 1,
      tamanho: 20,
      total: 0,
    });
    const u = userEvent.setup();
    await u.type(
      screen.getByLabelText("Pesquisar (nome, NIF, telefone ou e-mail)"),
      "zzz",
    );
    await u.click(screen.getByRole("button", { name: "Pesquisar" }));
    expect(
      await screen.findByText("Nenhum cliente corresponde à pesquisa."),
    ).toBeInTheDocument();
  });

  it("muda de página", async () => {
    const servico = servicoFalso();
    servico.listar.mockResolvedValue({
      itens: [rita],
      pagina: 1,
      tamanho: 20,
      total: 45,
    });
    montar("/clientes", servico);
    expect(await screen.findByText(/Página 1 de 3/)).toBeInTheDocument();
    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Seguinte" }));
    await waitFor(() =>
      expect(servico.listar).toHaveBeenLastCalledWith({
        pagina: 2,
        tamanho: 20,
      }),
    );
  });

  it("mostra o erro quando a lista falha", async () => {
    const servico = servicoFalso();
    servico.listar.mockRejectedValue(new ApiRequestError(0, "rede"));
    montar("/clientes", servico);
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor",
    );
  });
});

describe("W05 — registar Cliente", () => {
  it("regista o Cliente com a conta de acesso e abre a ficha", async () => {
    const servico = montar("/clientes/novo");
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("Nome"), "Carlos Sousa");
    await u.type(screen.getByLabelText("NIF (opcional)"), "123456789");
    await u.type(screen.getByLabelText("Telefone"), "912345678");
    await u.type(screen.getByLabelText("Morada (opcional)"), "Rua A, 1");
    await u.type(screen.getByLabelText("E-mail"), "carlos@email.pt");
    await u.type(screen.getByLabelText("Palavra-passe inicial"), "Temp#2026");
    await u.click(screen.getByRole("button", { name: "Registar cliente" }));

    expect(servico.criar).toHaveBeenCalledWith({
      nome: "Carlos Sousa",
      nif: "123456789",
      telefone: "912345678",
      email: "carlos@email.pt",
      morada: "Rua A, 1",
      passwordInicial: "Temp#2026",
    });
    expect(
      await screen.findByText(
        "Cliente Carlos Sousa registado, com conta de acesso para carlos@email.pt.",
      ),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("heading", { name: "Ficha de cliente" }),
    ).toBeInTheDocument();
    expect(servico.obter).toHaveBeenCalledWith(7);
    expect(screen.queryByLabelText("Palavra-passe inicial")).toBeNull();
  });

  it("valida os campos antes de enviar", async () => {
    const servico = montar("/clientes/novo");
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("NIF (opcional)"), "123456780");
    await u.type(screen.getByLabelText("Palavra-passe inicial"), "curta");
    await u.click(screen.getByRole("button", { name: "Registar cliente" }));

    expect(screen.getByText("O nome é obrigatório.")).toBeInTheDocument();
    expect(screen.getByText(/O NIF é inválido/)).toBeInTheDocument();
    expect(screen.getByText("O telefone é obrigatório.")).toBeInTheDocument();
    expect(screen.getByText("O e-mail é obrigatório.")).toBeInTheDocument();
    expect(
      screen.getByText(
        "A palavra-passe inicial deve ter entre 8 e 128 caracteres.",
      ),
    ).toBeInTheDocument();
    expect(screen.getByLabelText("Nome")).toHaveAttribute(
      "aria-invalid",
      "true",
    );
    expect(servico.criar).not.toHaveBeenCalled();
  });

  it("mostra o conflito da API (e-mail ou NIF já registado) e limpa a palavra-passe", async () => {
    const servico = servicoFalso();
    servico.criar.mockRejectedValue(
      new ApiRequestError(
        409,
        "conflito",
        "Já existe uma conta com o e-mail indicado.",
      ),
    );
    montar("/clientes/novo", servico);
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("Nome"), "Rita Ferreira");
    await u.type(screen.getByLabelText("Telefone"), "919876543");
    await u.type(screen.getByLabelText("E-mail"), "carlos@email.pt");
    await u.type(screen.getByLabelText("Palavra-passe inicial"), "Temp#2026");
    await u.click(screen.getByRole("button", { name: "Registar cliente" }));

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Já existe uma conta com o e-mail indicado.",
    );
    expect(screen.getByLabelText("Nome")).toHaveValue("Rita Ferreira");
    expect(screen.getByLabelText("Palavra-passe inicial")).toHaveValue("");
    expect(
      screen.getByRole("button", { name: "Registar cliente" }),
    ).toBeEnabled();
  });
});

describe("W05 — ficha e edição do Cliente", () => {
  it("separa os dados do Cliente da conta de acesso", async () => {
    montar("/clientes/7");
    const dados = await screen.findByRole("group", {
      name: "Dados do cliente",
    });
    expect(within(dados).getByLabelText("Nome")).toHaveValue("Carlos Sousa");
    expect(within(dados).getByLabelText("NIF (opcional)")).toHaveValue(
      "123456789",
    );
    const conta = screen.getByRole("group", {
      name: "Conta de acesso (aplicação móvel)",
    });
    expect(within(conta).getByLabelText("E-mail")).toHaveValue(
      "carlos@email.pt",
    );
    expect(conta).toHaveTextContent("Estado da conta: Ativo");
    expect(screen.queryByLabelText("Palavra-passe inicial")).toBeNull();
  });

  it("guarda as alterações (EP-014) e mostra os novos valores", async () => {
    const servico = servicoFalso();
    servico.atualizar.mockResolvedValue({ ...carlos, telefone: "930000000" });
    montar("/clientes/7", servico);
    const telefone = await screen.findByLabelText("Telefone");
    const u = userEvent.setup();
    await u.clear(telefone);
    await u.type(telefone, "930000000");
    await u.click(screen.getByRole("button", { name: "Guardar alterações" }));

    expect(servico.atualizar).toHaveBeenCalledWith(7, {
      nome: "Carlos Sousa",
      nif: "123456789",
      telefone: "930000000",
      email: "carlos@email.pt",
      morada: "Rua A, 1",
    });
    expect(await screen.findByRole("status")).toHaveTextContent(
      "Alterações guardadas.",
    );
    expect(screen.getByLabelText("Telefone")).toHaveValue("930000000");
  });

  it("mostra o erro de validação da API (400) junto ao formulário", async () => {
    const servico = servicoFalso();
    servico.atualizar.mockRejectedValue(
      new ApiRequestError(400, "validacao", "O e-mail é inválido."),
    );
    montar("/clientes/7", servico);
    await screen.findByLabelText("Telefone");
    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Guardar alterações" }));
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "O e-mail é inválido.",
    );
  });

  it("lista os animais; a Rececionista tem ligação para a ficha do paciente", async () => {
    montar("/clientes/7");
    const animais = await screen.findByRole("region", {
      name: "Animais associados",
    });
    expect(animais).toHaveTextContent("Kiko · Cão");
    expect(
      within(animais).getByRole("link", { name: "Ver paciente" }),
    ).toHaveAttribute("href", "/pacientes/3");
  });

  it("o Administrador vê os animais sem ligação para a ficha do paciente", async () => {
    montar("/clientes/7", servicoFalso(), "Administrador");
    const animais = await screen.findByRole("region", {
      name: "Animais associados",
    });
    expect(animais).toHaveTextContent("Kiko");
    expect(within(animais).queryByRole("link")).toBeNull();
  });

  it("cliente inexistente (404) mostra a mensagem e a ligação para a lista", async () => {
    const servico = servicoFalso();
    servico.obter.mockRejectedValue(new ApiRequestError(404, "naoEncontrado"));
    montar("/clientes/99", servico);
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "O registo pedido não foi encontrado.",
    );
    expect(
      screen.getByRole("link", { name: "Voltar à lista" }),
    ).toHaveAttribute("href", "/clientes");
  });

  it("um endereço com id inválido não chama a API", async () => {
    const servico = montar("/clientes/abc");
    expect(screen.getByRole("alert")).toHaveTextContent(
      "O registo pedido não foi encontrado.",
    );
    expect(servico.obter).not.toHaveBeenCalled();
  });
});

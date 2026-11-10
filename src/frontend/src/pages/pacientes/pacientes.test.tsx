import {
  fireEvent,
  render,
  screen,
  waitFor,
  within,
} from "@testing-library/react";
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
import type { PacienteDetalhe } from "../../models/pacientes";
import type { ServicoPacientes } from "../../services/PacientesService";
import { PacienteDetalhePage } from "./PacienteDetalhePage";
import { PacientesPage } from "./PacientesPage";

const bobi: PacienteDetalhe = {
  id: 3,
  nome: "Bobi",
  especie: "Cão",
  raca: "Labrador Retriever",
  sexo: "Macho",
  dataNascimento: "2022-05-10",
  idadeAnos: 4,
  peso: 28.4,
  cliente: { id: 7, nome: "João Costa", telefone: "960000001" },
  atualizadoEm: "2026-10-05T10:12:00Z",
};
const joao = {
  id: 7,
  nome: "João Costa",
  nif: "210000015",
  telefone: "960000001",
  email: "cliente.joao@clinica-g08.test",
  numeroAnimais: 2,
};

function servicoFalso(): {
  [K in keyof ServicoPacientes]: ReturnType<typeof vi.fn>;
} {
  return {
    listar: vi.fn().mockResolvedValue({
      itens: [bobi],
      pagina: 1,
      tamanho: 20,
      total: 1,
    }),
    obter: vi.fn().mockResolvedValue(bobi),
    criar: vi.fn().mockResolvedValue(bobi),
    atualizar: vi.fn().mockResolvedValue(bobi),
  };
}

function clientesFalso() {
  return {
    listar: vi.fn().mockResolvedValue({
      itens: [joao],
      pagina: 1,
      tamanho: 10,
      total: 1,
    }),
  };
}

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
  entrada: string | { pathname: string; state: unknown },
  {
    perfil = "Rececionista" as PerfilWeb,
    servico = servicoFalso(),
    clientes = clientesFalso(),
  } = {},
) {
  const s = servico as unknown as ServicoPacientes;
  render(
    <AuthContext.Provider value={sessaoDe(perfil)}>
      <MemoryRouter initialEntries={[entrada]}>
        <Routes>
          <Route
            path="/pacientes"
            element={<PacientesPage servico={s} servicoClientes={clientes} />}
          />
          <Route
            path="/pacientes/novo"
            element={
              <PacienteDetalhePage servico={s} servicoClientes={clientes} />
            }
          />
          <Route
            path="/pacientes/:id"
            element={
              <PacienteDetalhePage servico={s} servicoClientes={clientes} />
            }
          />
        </Routes>
      </MemoryRouter>
    </AuthContext.Provider>,
  );
  return { servico, clientes };
}

describe("W07 — lista e pesquisa de pacientes", () => {
  it("carrega a lista com espécie, raça, idade, peso e proprietário", async () => {
    const { servico } = montar("/pacientes");
    const linha = (await screen.findByText("Bobi")).closest("tr")!;
    expect(linha).toHaveTextContent("Cão");
    expect(linha).toHaveTextContent("Labrador Retriever");
    expect(linha).toHaveTextContent("4 anos");
    expect(linha).toHaveTextContent("28,4 kg");
    expect(linha).toHaveTextContent("João Costa");
    expect(
      within(linha).getByRole("link", { name: "Ver ficha de Bobi" }),
    ).toHaveAttribute("href", "/pacientes/3");
    expect(servico.listar).toHaveBeenCalledWith({ pagina: 1, tamanho: 20 });
  });

  it.each(["Rececionista", "Veterinario"] as const)(
    "%s vê o botão «Novo paciente»",
    async (perfil) => {
      montar("/pacientes", { perfil });
      await screen.findByText("Bobi");
      expect(
        screen.getByRole("link", { name: "Novo paciente" }),
      ).toHaveAttribute("href", "/pacientes/novo");
    },
  );

  it("pesquisa pelo nome e pela espécie", async () => {
    const { servico } = montar("/pacientes");
    await screen.findByText("Bobi");
    const u = userEvent.setup();
    await u.type(
      screen.getByLabelText("Pesquisar (nome do animal ou do proprietário)"),
      "bobi",
    );
    await u.selectOptions(screen.getByLabelText("Espécie"), "Cão");
    await u.click(screen.getByRole("button", { name: "Pesquisar" }));
    await waitFor(() =>
      expect(servico.listar).toHaveBeenLastCalledWith({
        pesquisa: "bobi",
        especie: "Cão",
        pagina: 1,
        tamanho: 20,
      }),
    );
  });

  it("pesquisa por proprietário (RF017) e filtra a lista pelo Cliente escolhido", async () => {
    const { servico, clientes } = montar("/pacientes", {
      perfil: "Veterinario",
    });
    await screen.findByText("Bobi");
    const u = userEvent.setup();
    await u.type(
      screen.getByLabelText("Proprietário (nome, NIF, telefone ou e-mail)"),
      "Costa",
    );
    await u.click(
      screen.getByRole("button", { name: "Procurar proprietário" }),
    );
    expect(clientes.listar).toHaveBeenCalledWith({
      pesquisa: "Costa",
      pagina: 1,
      tamanho: 10,
    });
    await u.click(
      await screen.findByRole("button", { name: "Selecionar João Costa" }),
    );
    await waitFor(() =>
      expect(servico.listar).toHaveBeenLastCalledWith({
        clienteId: 7,
        pagina: 1,
        tamanho: 20,
      }),
    );
    expect(
      screen.getByRole("button", { name: "Remover filtro" }),
    ).toBeInTheDocument();
  });

  it("indica quando nenhum Cliente corresponde à pesquisa de proprietário", async () => {
    const clientes = clientesFalso();
    clientes.listar.mockResolvedValue({
      itens: [],
      pagina: 1,
      tamanho: 10,
      total: 0,
    });
    montar("/pacientes", { clientes });
    await screen.findByText("Bobi");
    const u = userEvent.setup();
    const campo = screen.getByLabelText(
      "Proprietário (nome, NIF, telefone ou e-mail)",
    );
    await u.type(campo, "zzz{Enter}");
    expect(
      await screen.findByText("Nenhum cliente encontrado."),
    ).toBeInTheDocument();
  });

  it("mostra o erro quando a lista falha", async () => {
    const servico = servicoFalso();
    servico.listar.mockRejectedValue(new ApiRequestError(0, "rede"));
    montar("/pacientes", { servico });
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor",
    );
  });
});

describe("W08 — registar paciente", () => {
  it("regista o paciente associado ao proprietário escolhido em W07", async () => {
    const { servico } = montar({
      pathname: "/pacientes/novo",
      state: { proprietario: { id: 7, nome: "João Costa" } },
    });
    expect(screen.getByText("João Costa")).toBeInTheDocument();
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("Nome do animal"), "Bobi");
    await u.type(screen.getByLabelText("Espécie"), "Cão");
    await u.type(
      screen.getByLabelText("Raça (opcional)"),
      "Labrador Retriever",
    );
    await u.selectOptions(screen.getByLabelText("Sexo"), "Macho");
    fireEvent.change(screen.getByLabelText("Data de nascimento (opcional)"), {
      target: { value: "2022-05-10" },
    });
    fireEvent.change(screen.getByLabelText("Peso em kg (opcional)"), {
      target: { value: "28.4" },
    });
    await u.click(
      screen.getByRole("button", { name: "Guardar dados administrativos" }),
    );

    expect(servico.criar).toHaveBeenCalledWith({
      nome: "Bobi",
      especie: "Cão",
      raca: "Labrador Retriever",
      sexo: "Macho",
      dataNascimento: "2022-05-10",
      peso: 28.4,
      clienteId: 7,
    });
    expect(
      await screen.findByText("Paciente Bobi registado para João Costa."),
    ).toBeInTheDocument();
    expect(servico.obter).toHaveBeenCalledWith(3);
  });

  it("escolhe o proprietário dentro do formulário", async () => {
    const { servico } = montar("/pacientes/novo", { perfil: "Veterinario" });
    const u = userEvent.setup();
    await u.type(
      screen.getByLabelText("Proprietário (nome, NIF, telefone ou e-mail)"),
      "joão{Enter}",
    );
    await u.click(
      await screen.findByRole("button", { name: "Selecionar João Costa" }),
    );
    await u.type(screen.getByLabelText("Nome do animal"), "Mia");
    await u.type(screen.getByLabelText("Espécie"), "Gato");
    await u.click(
      screen.getByRole("button", { name: "Guardar dados administrativos" }),
    );
    expect(servico.criar).toHaveBeenCalledWith(
      expect.objectContaining({ nome: "Mia", especie: "Gato", clienteId: 7 }),
    );
  });

  it("valida os campos e exige o proprietário antes de enviar", async () => {
    const { servico } = montar("/pacientes/novo");
    fireEvent.change(screen.getByLabelText("Peso em kg (opcional)"), {
      target: { value: "0" },
    });
    await userEvent
      .setup()
      .click(
        screen.getByRole("button", { name: "Guardar dados administrativos" }),
      );
    expect(
      screen.getByText("Escolha o proprietário (Cliente) do paciente."),
    ).toBeInTheDocument();
    expect(screen.getByText("O nome é obrigatório.")).toBeInTheDocument();
    expect(screen.getByText("A espécie é obrigatória.")).toBeInTheDocument();
    expect(
      screen.getByText("O peso deve estar entre 0,01 e 9999,99 kg."),
    ).toBeInTheDocument();
    expect(servico.criar).not.toHaveBeenCalled();
  });

  it("mostra o erro da API (400) sem bloquear o formulário", async () => {
    const servico = servicoFalso();
    servico.criar.mockRejectedValue(
      new ApiRequestError(
        400,
        "validacao",
        "A raça indicada não existe para a espécie selecionada.",
      ),
    );
    montar(
      {
        pathname: "/pacientes/novo",
        state: { proprietario: { id: 7, nome: "João Costa" } },
      },
      { servico },
    );
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("Nome do animal"), "Rex");
    await u.type(screen.getByLabelText("Espécie"), "Cão");
    await u.type(screen.getByLabelText("Raça (opcional)"), "Poodle");
    await u.click(
      screen.getByRole("button", { name: "Guardar dados administrativos" }),
    );
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "A raça indicada não existe para a espécie selecionada.",
    );
    expect(screen.getByLabelText("Raça (opcional)")).toHaveValue("Poodle");
    expect(
      screen.getByRole("button", { name: "Guardar dados administrativos" }),
    ).toBeEnabled();
  });
});

describe("W08 — ficha do paciente por perfil", () => {
  it("Veterinário edita os dados administrativos e vê a área clínica", async () => {
    const servico = servicoFalso();
    servico.atualizar.mockResolvedValue({ ...bobi, peso: 29.1 });
    montar("/pacientes/3", { perfil: "Veterinario", servico });
    expect(await screen.findByLabelText("Nome do animal")).toHaveValue("Bobi");
    expect(screen.getByLabelText("Peso em kg (opcional)")).toHaveValue(28.4);
    fireEvent.change(screen.getByLabelText("Peso em kg (opcional)"), {
      target: { value: "29.1" },
    });
    await userEvent
      .setup()
      .click(
        screen.getByRole("button", { name: "Guardar dados administrativos" }),
      );

    expect(servico.atualizar).toHaveBeenCalledWith(3, {
      nome: "Bobi",
      especie: "Cão",
      raca: "Labrador Retriever",
      sexo: "Macho",
      dataNascimento: "2022-05-10",
      peso: 29.1,
      clienteId: 7,
    });
    expect(await screen.findByRole("status")).toHaveTextContent(
      "Alterações guardadas.",
    );
    const clinica = screen.getByRole("navigation", { name: "Área clínica" });
    expect(
      within(clinica).getByRole("link", { name: "Registo clínico" }),
    ).toHaveAttribute("href", "/pacientes/3/registo");
    expect(
      within(clinica).getByRole("link", { name: "Vacinação" }),
    ).toBeInTheDocument();
    expect(screen.queryByRole("link", { name: "Ver cliente" })).toBeNull();
  });

  it("Rececionista edita, sem informação nem ligações clínicas", async () => {
    montar("/pacientes/3");
    expect(await screen.findByLabelText("Nome do animal")).toHaveValue("Bobi");
    expect(
      screen.getByRole("button", { name: "Guardar dados administrativos" }),
    ).toBeInTheDocument();
    expect(
      screen.queryByRole("navigation", { name: "Área clínica" }),
    ).toBeNull();
    expect(screen.getByRole("link", { name: "Ver cliente" })).toHaveAttribute(
      "href",
      "/clientes/7",
    );
  });

  it("Auxiliar consulta a ficha só em leitura, sem ações de edição", async () => {
    const { servico } = montar("/pacientes/3", {
      perfil: "AuxiliarVeterinario",
    });
    const ficha = await screen.findByLabelText("Dados administrativos");
    expect(ficha).toHaveTextContent("Bobi");
    expect(ficha).toHaveTextContent("João Costa · 960000001");
    expect(ficha).toHaveTextContent("10/05/2022");
    expect(ficha).toHaveTextContent("28,4 kg");
    expect(screen.queryByRole("textbox")).toBeNull();
    expect(screen.queryByRole("button")).toBeNull();
    expect(
      screen.getByRole("link", { name: "Registo clínico" }),
    ).toBeInTheDocument();
    expect(screen.queryByRole("link", { name: "Vacinação" })).toBeNull();
    expect(servico.atualizar).not.toHaveBeenCalled();
  });

  it("paciente inexistente (404) mostra a mensagem", async () => {
    const servico = servicoFalso();
    servico.obter.mockRejectedValue(new ApiRequestError(404, "naoEncontrado"));
    montar("/pacientes/99", { servico });
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "O registo pedido não foi encontrado.",
    );
  });

  it("um endereço com id inválido não chama a API", () => {
    const { servico } = montar("/pacientes/abc");
    expect(screen.getByRole("alert")).toHaveTextContent(
      "O registo pedido não foi encontrado.",
    );
    expect(servico.obter).not.toHaveBeenCalled();
  });
});

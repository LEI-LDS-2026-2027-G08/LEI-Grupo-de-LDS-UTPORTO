import { render, screen, waitFor, within } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { describe, expect, it, vi } from "vitest";
import { ApiRequestError } from "../../core/api";
import type { FuncionarioDetalhe } from "../../models/utilizadores";
import type { ServicoUtilizadores } from "../../services/UtilizadoresService";
import { FuncionarioFormPage } from "./FuncionarioFormPage";
import { FuncionariosPage } from "./FuncionariosPage";

const ana: FuncionarioDetalhe = {
  id: 5,
  nome: "Ana Reis",
  email: "ana@clinica.pt",
  telefone: "912345678",
  perfil: "Veterinario",
  estado: "Ativo",
  motivoBloqueio: null,
  podeRegistarApoio: false,
  criadoEm: "2026-10-01T09:00:00Z",
};
const rui = {
  id: 9,
  nome: "Rui Lopes",
  email: "rui@clinica.pt",
  perfil: "AuxiliarVeterinario" as const,
  estado: "Ativo" as const,
  podeRegistarApoio: true,
};

function servicoFalso(): {
  [K in keyof ServicoUtilizadores]: ReturnType<typeof vi.fn>;
} {
  return {
    listar: vi.fn().mockResolvedValue({
      itens: [ana, rui],
      pagina: 1,
      tamanho: 20,
      total: 2,
    }),
    obter: vi.fn().mockResolvedValue(ana),
    criar: vi.fn().mockResolvedValue(rui),
    atualizar: vi.fn().mockResolvedValue(ana),
    bloquear: vi.fn().mockResolvedValue(undefined),
  };
}

function montar(caminho: string, servico = servicoFalso()) {
  const s = servico as unknown as ServicoUtilizadores;
  render(
    <MemoryRouter initialEntries={[caminho]}>
      <Routes>
        <Route
          path="/funcionarios"
          element={<FuncionariosPage servico={s} />}
        />
        <Route
          path="/funcionarios/novo"
          element={<FuncionarioFormPage servico={s} />}
        />
        <Route
          path="/funcionarios/:id"
          element={<FuncionarioFormPage servico={s} />}
        />
      </Routes>
    </MemoryRouter>,
  );
  return servico;
}

describe("W02 — lista de funcionários", () => {
  it("carrega a lista e mostra perfil, estado e autorização de apoio", async () => {
    const servico = montar("/funcionarios");
    expect(screen.getByRole("status")).toHaveTextContent("A carregar");
    const linhaRui = (await screen.findByText("Rui Lopes")).closest("tr")!;
    expect(linhaRui).toHaveTextContent("Auxiliar Veterinário");
    expect(linhaRui).toHaveTextContent("Autorizado");
    const linhaAna = screen.getByText("Ana Reis").closest("tr")!;
    expect(linhaAna).toHaveTextContent("Veterinário");
    expect(linhaAna).toHaveTextContent("—");
    expect(servico.listar).toHaveBeenCalledWith({ pagina: 1, tamanho: 20 });
  });

  it("pesquisa com os filtros escolhidos", async () => {
    const servico = montar("/funcionarios");
    await screen.findByText("Ana Reis");
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("Pesquisar (nome ou e-mail)"), "ana");
    await u.selectOptions(screen.getByLabelText("Perfil"), "Veterinario");
    await u.selectOptions(screen.getByLabelText("Estado"), "Ativo");
    await u.click(screen.getByRole("button", { name: "Pesquisar" }));
    await waitFor(() =>
      expect(servico.listar).toHaveBeenLastCalledWith({
        pesquisa: "ana",
        perfil: "Veterinario",
        estado: "Ativo",
        pagina: 1,
        tamanho: 20,
      }),
    );
  });

  it("mostra o erro da API quando a lista falha", async () => {
    const servico = servicoFalso();
    servico.listar.mockRejectedValue(new ApiRequestError(0, "rede"));
    montar("/funcionarios", servico);
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor",
    );
  });

  it("bloqueia uma conta depois de confirmar, com motivo", async () => {
    const servico = montar("/funcionarios");
    const linha = (await screen.findByText("Ana Reis")).closest("tr")!;
    const u = userEvent.setup();
    await u.click(within(linha).getByRole("button", { name: "Bloquear" }));
    await u.type(screen.getByLabelText("Motivo (opcional)"), "Saiu.");
    await u.click(screen.getByRole("button", { name: "Confirmar bloqueio" }));
    await waitFor(() =>
      expect(servico.bloquear).toHaveBeenCalledWith(5, "Saiu."),
    );
    expect(
      await screen.findByText("A conta de Ana Reis foi bloqueada."),
    ).toBeInTheDocument();
    expect(servico.listar).toHaveBeenCalledTimes(2);
  });

  it("mostra o erro do bloqueio (422) sem fechar a confirmação", async () => {
    const servico = servicoFalso();
    servico.bloquear.mockRejectedValue(
      new ApiRequestError(
        422,
        "regraNegocio",
        "O Administrador não pode bloquear a própria conta.",
      ),
    );
    montar("/funcionarios", servico);
    const linha = (await screen.findByText("Ana Reis")).closest("tr")!;
    const u = userEvent.setup();
    await u.click(within(linha).getByRole("button", { name: "Bloquear" }));
    await u.click(screen.getByRole("button", { name: "Confirmar bloqueio" }));
    expect(await screen.findByRole("alert")).toHaveTextContent("própria conta");
    expect(
      screen.getByRole("button", { name: "Confirmar bloqueio" }),
    ).toBeEnabled();
  });
});

describe("W03 — criar e editar funcionário", () => {
  it("ao criar pede a palavra-passe inicial e só mostra a autorização ao Auxiliar", async () => {
    montar("/funcionarios/novo");
    expect(screen.getByLabelText("Palavra-passe inicial")).toHaveAttribute(
      "type",
      "password",
    );
    expect(
      screen.queryByLabelText(/registar informação não clínica/),
    ).not.toBeInTheDocument();
    const u = userEvent.setup();
    await u.selectOptions(
      screen.getByLabelText("Perfil"),
      "AuxiliarVeterinario",
    );
    expect(
      screen.getByLabelText(/registar informação não clínica/),
    ).toBeInTheDocument();
    await u.selectOptions(screen.getByLabelText("Perfil"), "Rececionista");
    expect(
      screen.queryByLabelText(/registar informação não clínica/),
    ).not.toBeInTheDocument();
  });

  it("cria a conta, envia os dados e volta à lista com aviso", async () => {
    const servico = montar("/funcionarios/novo");
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("Nome"), "Rui Lopes");
    await u.type(screen.getByLabelText("E-mail"), "rui@clinica.pt");
    await u.type(screen.getByLabelText("Palavra-passe inicial"), "Temp#2026");
    await u.selectOptions(
      screen.getByLabelText("Perfil"),
      "AuxiliarVeterinario",
    );
    await u.click(screen.getByLabelText(/registar informação não clínica/));
    await u.click(screen.getByRole("button", { name: "Guardar" }));
    await waitFor(() =>
      expect(servico.criar).toHaveBeenCalledWith({
        nome: "Rui Lopes",
        email: "rui@clinica.pt",
        telefone: "",
        perfil: "AuxiliarVeterinario",
        podeRegistarApoio: true,
        passwordInicial: "Temp#2026",
      }),
    );
    expect(
      await screen.findByText("Conta de Rui Lopes criada."),
    ).toBeInTheDocument();
  });

  it("um erro ao criar (409) mostra o detail e limpa a palavra-passe", async () => {
    const servico = servicoFalso();
    servico.criar.mockRejectedValue(
      new ApiRequestError(409, "conflito", "O e-mail já está registado."),
    );
    montar("/funcionarios/novo", servico);
    const u = userEvent.setup();
    await u.type(screen.getByLabelText("Nome"), "Rui");
    await u.type(screen.getByLabelText("E-mail"), "rui@clinica.pt");
    await u.type(screen.getByLabelText("Palavra-passe inicial"), "Temp#2026");
    await u.click(screen.getByRole("button", { name: "Guardar" }));
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "O e-mail já está registado.",
    );
    expect(screen.getByLabelText("Palavra-passe inicial")).toHaveValue("");
    expect(screen.getByLabelText("Nome")).toHaveValue("Rui");
  });

  it("ao editar carrega a conta, não pede palavra-passe e guarda", async () => {
    const servico = montar("/funcionarios/5");
    expect(await screen.findByLabelText("Nome")).toHaveValue("Ana Reis");
    expect(
      screen.queryByLabelText("Palavra-passe inicial"),
    ).not.toBeInTheDocument();
    expect(screen.getByText(/Estado:/)).toHaveTextContent("Ativo");
    const u = userEvent.setup();
    await u.selectOptions(screen.getByLabelText("Perfil"), "Rececionista");
    await u.click(screen.getByRole("button", { name: "Guardar" }));
    await waitFor(() =>
      expect(servico.atualizar).toHaveBeenCalledWith(5, {
        nome: "Ana Reis",
        email: "ana@clinica.pt",
        telefone: "912345678",
        perfil: "Rececionista",
        podeRegistarApoio: false,
      }),
    );
    expect(
      await screen.findByText("Alterações guardadas."),
    ).toBeInTheDocument();
  });

  it("ao mudar de Auxiliar para outro perfil a autorização deixa de valer", async () => {
    const servico = servicoFalso();
    servico.obter.mockResolvedValue({
      ...rui,
      telefone: null,
      criadoEm: "2026-10-01T09:00:00Z",
    });
    montar("/funcionarios/9", servico);
    expect(
      await screen.findByLabelText(/registar informação não clínica/),
    ).toBeChecked();
    const u = userEvent.setup();
    await u.selectOptions(screen.getByLabelText("Perfil"), "Veterinario");
    await u.selectOptions(
      screen.getByLabelText("Perfil"),
      "AuxiliarVeterinario",
    );
    expect(
      screen.getByLabelText(/registar informação não clínica/),
    ).not.toBeChecked();
  });

  it("conta inexistente (404): mostra a mensagem e volta à lista", async () => {
    const servico = servicoFalso();
    servico.obter.mockRejectedValue(
      new ApiRequestError(404, "naoEncontrado", "Utilizador não encontrado."),
    );
    montar("/funcionarios/99", servico);
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Utilizador não encontrado.",
    );
    expect(
      screen.getByRole("link", { name: "Voltar à lista" }),
    ).toBeInTheDocument();
  });

  it("numa conta bloqueada mostra o motivo do bloqueio", async () => {
    const servico = servicoFalso();
    servico.obter.mockResolvedValue({
      ...ana,
      estado: "Bloqueado",
      motivoBloqueio: "Saiu da clínica.",
    });
    montar("/funcionarios/5", servico);
    expect(
      await screen.findByText(/Motivo do bloqueio: Saiu da clínica\./),
    ).toBeInTheDocument();
  });

  it("bloqueia a conta a partir da ficha e deixa de mostrar o botão", async () => {
    const servico = montar("/funcionarios/5");
    await screen.findByLabelText("Nome");
    servico.obter.mockResolvedValue({ ...ana, estado: "Bloqueado" });
    const u = userEvent.setup();
    await u.click(screen.getByRole("button", { name: "Bloquear conta" }));
    await u.click(screen.getByRole("button", { name: "Confirmar bloqueio" }));
    await waitFor(() => expect(servico.bloquear).toHaveBeenCalledWith(5, ""));
    expect(await screen.findByText("Conta bloqueada.")).toBeInTheDocument();
    expect(screen.getByText(/Estado:/)).toHaveTextContent("Bloqueado");
    expect(
      screen.queryByRole("button", { name: "Bloquear conta" }),
    ).not.toBeInTheDocument();
  });
});

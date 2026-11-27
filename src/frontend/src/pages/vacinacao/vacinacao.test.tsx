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
import type { Administracao, Boletim } from "../../models/vacinacoes";
import type { ServicoVacinacoes } from "../../services/VacinacoesService";
import { VacinacaoPage } from "./VacinacaoPage";

const NOBIVAC: Administracao = {
  id: 21,
  tipo: "Vacina",
  produto: "Nobivac",
  lote: "L-2025-07",
  dataAdministracao: "2025-11-10",
  intervaloDias: 365,
  dataProximaPrevista: "2026-11-10",
  veterinario: "Dra. Ana",
};

const MILBEMAX: Administracao = {
  id: 22,
  tipo: "Desparasitacao",
  produto: "Milbemax",
  lote: "M-88",
  dataAdministracao: "2026-10-01",
  intervaloDias: null,
  dataProximaPrevista: null,
  veterinario: "Dr. Paulo",
};

const BOLETIM: Boletim = {
  animal: { id: 3, nome: "Bobi", especie: "Cão" },
  administracoes: [MILBEMAX, NOBIVAC],
  proximas: [
    { tipo: "Vacina", produto: "Nobivac", dataProximaPrevista: "2026-11-10" },
  ],
};

function servicoFalso(boletim: Boletim = BOLETIM): {
  [K in keyof ServicoVacinacoes]: ReturnType<typeof vi.fn>;
} {
  return {
    obterBoletim: vi.fn().mockResolvedValue(boletim),
    registar: vi.fn(),
  };
}

function sessaoDe(perfil: PerfilWeb): AuthContextValue {
  return {
    sessao: null,
    utilizador: { id: 5, nome: "Dra. Ana", email: "ana@clinica.pt", perfil },
    aRestaurar: false,
    login: vi.fn(),
    logout: vi.fn(),
    temPerfil: (perfis) => perfis.includes(perfil),
    pode: (acao) => perfilPodeExecutar(perfil, acao),
    sessaoTerminada: false,
  };
}

function montar({
  perfil = "Veterinario" as PerfilWeb,
  servico = servicoFalso(),
} = {}) {
  render(
    <AuthContext.Provider value={sessaoDe(perfil)}>
      <MemoryRouter initialEntries={["/pacientes/3/vacinacao"]}>
        <Routes>
          <Route
            path="/pacientes/:id/vacinacao"
            element={
              <VacinacaoPage
                servico={servico as unknown as ServicoVacinacoes}
              />
            }
          />
        </Routes>
      </MemoryRouter>
    </AuthContext.Provider>,
  );
  return servico;
}

function seccao(nome: string) {
  return screen.getByRole("region", { name: nome });
}

describe("W16 — boletim do paciente (I118)", () => {
  it("mantém próximas administrações com o mesmo tipo, produto e data sem chaves duplicadas", async () => {
    const primeira = BOLETIM.proximas[0];
    const segunda = { ...primeira };
    const erroConsola = vi.spyOn(console, "error").mockImplementation(() => {});
    try {
      montar({
        servico: servicoFalso({
          ...BOLETIM,
          proximas: [primeira, segunda],
        }),
      });
      const proximas = await screen.findByRole("region", {
        name: "Próximas administrações",
      });
      expect(within(proximas).getAllByRole("row")).toHaveLength(3);
      expect(erroConsola.mock.calls.flat().join(" ")).not.toMatch(/same key/);
    } finally {
      erroConsola.mockRestore();
    }
  });

  it("mostra o boletim com tipo, produto, lote, data, intervalo e próxima administração (EP-042)", async () => {
    const servico = montar();
    expect(screen.getByRole("status")).toHaveTextContent("A carregar…");

    expect(await screen.findByText(/Paciente:/)).toHaveTextContent(
      "Bobi · Cão",
    );
    expect(servico.obterBoletim).toHaveBeenCalledWith(3, undefined);

    const linhas = within(seccao("Boletim")).getAllByRole("row");
    expect(linhas).toHaveLength(3);
    expect(linhas[1]).toHaveTextContent("01/10/2026");
    expect(linhas[1]).toHaveTextContent("Desparasitação");
    expect(linhas[1]).toHaveTextContent("Milbemax");
    expect(linhas[1]).toHaveTextContent("M-88");
    expect(linhas[2]).toHaveTextContent("Nobivac");
    expect(linhas[2]).toHaveTextContent("365 dias");
    expect(linhas[2]).toHaveTextContent("10/11/2026");

    expect(
      within(seccao("Próximas administrações")).getByRole("table"),
    ).toHaveTextContent("10/11/2026");
  });

  it("paciente sem registos: mostra as mensagens próprias", async () => {
    montar({
      servico: servicoFalso({
        animal: BOLETIM.animal,
        administracoes: [],
        proximas: [],
      }),
    });
    expect(
      await screen.findByText(
        "Este paciente ainda não tem vacinações nem desparasitações registadas.",
      ),
    ).toBeInTheDocument();
    expect(
      screen.getByText("Sem próximas administrações previstas."),
    ).toBeInTheDocument();
  });

  it("filtra o boletim por tipo (EP-042, parâmetro tipo)", async () => {
    const servico = montar();
    await screen.findByText(/Paciente:/);
    await userEvent
      .setup()
      .selectOptions(screen.getByLabelText("Mostrar"), "Vacina");
    await waitFor(() =>
      expect(servico.obterBoletim).toHaveBeenLastCalledWith(3, "Vacina"),
    );
  });

  it("falha de ligação: mostra a mensagem e permite tentar novamente", async () => {
    const servico = servicoFalso();
    servico.obterBoletim
      .mockReset()
      .mockRejectedValueOnce(new ApiRequestError(0, "rede"))
      .mockResolvedValueOnce(BOLETIM);
    montar({ servico });
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor. Verifique a ligação.",
    );
    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Tentar novamente" }));
    expect(await screen.findByText(/Paciente:/)).toBeInTheDocument();
    expect(screen.queryByRole("alert")).not.toBeInTheDocument();
  });
});

describe("W16 — registar vacinação e desparasitação (I118)", () => {
  it("regista uma vacina com intervalo e mostra a próxima administração calculada pela API (EP-043)", async () => {
    const servico = montar();
    servico.registar.mockResolvedValue({
      ...NOBIVAC,
      id: 30,
      lote: "L-2026-11",
      dataAdministracao: "2026-11-10",
      dataProximaPrevista: "2027-11-10",
    });
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Registar administração" }),
    );
    await u.type(screen.getByLabelText("Produto"), "Nobivac");
    await u.type(screen.getByLabelText("Lote"), "L-2026-11");
    fireEvent.change(screen.getByLabelText("Data de administração"), {
      target: { value: "2026-11-10" },
    });
    await u.type(
      screen.getByLabelText("Intervalo até à próxima, em dias (opcional)"),
      "365",
    );
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    await waitFor(() =>
      expect(servico.registar).toHaveBeenCalledWith(3, {
        tipo: "Vacina",
        produto: "Nobivac",
        lote: "L-2026-11",
        dataAdministracao: "2026-11-10",
        intervaloDias: 365,
      }),
    );
    expect(
      await screen.findByText(/Vacina Nobivac registada/),
    ).toHaveTextContent("Próxima administração prevista: 10/11/2027.");
    // O boletim é lido de novo para mostrar o que ficou gravado.
    expect(servico.obterBoletim).toHaveBeenCalledTimes(2);
  });

  it("regista uma desparasitação sem intervalo: sem próxima administração", async () => {
    const servico = montar();
    servico.registar.mockResolvedValue({ ...MILBEMAX, id: 31 });
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Registar administração" }),
    );
    await u.selectOptions(screen.getByLabelText("Tipo"), "Desparasitacao");
    await u.type(screen.getByLabelText("Produto"), "Milbemax");
    await u.type(screen.getByLabelText("Lote"), "M-88");
    fireEvent.change(screen.getByLabelText("Data de administração"), {
      target: { value: "2026-10-01" },
    });
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    await waitFor(() =>
      expect(servico.registar).toHaveBeenCalledWith(
        3,
        expect.objectContaining({
          tipo: "Desparasitacao",
          intervaloDias: null,
        }),
      ),
    );
    expect(
      await screen.findByText(/Sem próxima administração prevista/),
    ).toBeInTheDocument();
  });

  it("valida os campos antes de enviar", async () => {
    const servico = montar();
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Registar administração" }),
    );
    await u.type(
      screen.getByLabelText("Intervalo até à próxima, em dias (opcional)"),
      "0",
    );
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    expect(screen.getByText("O produto é obrigatório.")).toBeInTheDocument();
    expect(screen.getByText("O lote é obrigatório.")).toBeInTheDocument();
    expect(
      screen.getByText(/O intervalo, quando indicado, deve ser/),
    ).toBeInTheDocument();
    expect(servico.registar).not.toHaveBeenCalled();
  });

  it("mostra o erro da API no formulário (ex.: data futura, 400) sem perder os dados", async () => {
    const servico = montar();
    servico.registar.mockRejectedValue(
      new ApiRequestError(
        400,
        "validacao",
        "A data de administração não pode ser futura.",
      ),
    );
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Registar administração" }),
    );
    await u.type(screen.getByLabelText("Produto"), "Nobivac");
    await u.type(screen.getByLabelText("Lote"), "L1");
    fireEvent.change(screen.getByLabelText("Data de administração"), {
      target: { value: "2026-11-10" },
    });
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    const formulario = screen.getByRole("form", {
      name: "Registar vacinação ou desparasitação",
    });
    expect(await within(formulario).findByRole("alert")).toHaveTextContent(
      "A data de administração não pode ser futura.",
    );
    expect(screen.getByLabelText("Produto")).toHaveValue("Nobivac");
  });
});

describe("W16 — âmbito da I118 (RF056 excluído, deliberação 5)", () => {
  it("o boletim não tem ação para alterar administrações", async () => {
    montar();
    await screen.findByText(/Paciente:/);
    expect(
      screen.queryByRole("button", { name: /^Alterar/ }),
    ).not.toBeInTheDocument();
    expect(
      within(seccao("Boletim")).queryByRole("columnheader", {
        name: "Ações",
      }),
    ).not.toBeInTheDocument();
  });

  it("Cancelar fecha o formulário sem enviar nada", async () => {
    const servico = montar();
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Registar administração" }),
    );
    await u.click(screen.getByRole("button", { name: "Cancelar" }));
    expect(screen.queryByLabelText("Produto")).not.toBeInTheDocument();
    expect(servico.registar).not.toHaveBeenCalled();
  });
});

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
import type {
  HistoricoClinico,
  RegistoClinico,
} from "../../models/registosClinicos";
import { HistoricoClinicoPage } from "./HistoricoClinicoPage";
import { MENSAGEM_FIM_ANTERIOR } from "./periodoHistorico";

const HISTORICO: HistoricoClinico = {
  animal: { id: 3, nome: "Bobi", especie: "Cão" },
  registos: [
    {
      id: 9,
      consultaId: 41,
      data: "2026-11-20",
      veterinario: "Dra. Ana",
      diagnostico: "Otite externa",
      numeroTratamentos: 1,
      numeroPrescricoes: 1,
    },
    {
      id: 5,
      consultaId: 30,
      data: "2026-10-02",
      veterinario: "Dr. Paulo",
      diagnostico: null,
      numeroTratamentos: 0,
      numeroPrescricoes: 0,
    },
  ],
};

const REGISTO: RegistoClinico = {
  id: 9,
  consultaId: 41,
  animalId: 3,
  veterinario: "Dra. Ana",
  criadoEm: "2026-11-20T10:40:00Z",
  notasClinicas: "Cerúmen abundante no ouvido esquerdo.",
  diagnostico: "Otite externa",
  tratamentos: [
    { id: 1, descricao: "Limpeza auricular", dataAplicacao: "2026-11-20" },
  ],
  prescricoes: [
    {
      id: 2,
      medicamento: "Otomax",
      dosagem: "5 gotas",
      frequencia: "2x por dia",
      duracaoDias: 7,
    },
  ],
  atualizadoEm: "2026-11-20T11:00:00Z",
  atualizadoPor: "Dra. Ana",
};

function servicoFalso(historico: HistoricoClinico = HISTORICO) {
  return {
    obterHistorico: vi.fn().mockResolvedValue(historico),
    obter: vi.fn().mockResolvedValue(REGISTO),
  };
}

function sessaoDe(perfil: PerfilWeb): AuthContextValue {
  return {
    sessao: null,
    utilizador: { id: 5, nome: "Utilizador", email: "u@clinica.pt", perfil },
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
      <MemoryRouter initialEntries={["/pacientes/3/registo"]}>
        <Routes>
          <Route
            path="/pacientes/:id/registo"
            element={<HistoricoClinicoPage servico={servico} />}
          />
        </Routes>
      </MemoryRouter>
    </AuthContext.Provider>,
  );
  return servico;
}

async function abrirRegisto(u: ReturnType<typeof userEvent.setup>) {
  await u.click(
    await screen.findByRole("button", { name: "Ver registo de 20/11/2026" }),
  );
  return screen.findByRole("region", { name: "Registo de 20/11/2026" });
}

describe("W13 — histórico clínico do Veterinário (I116)", () => {
  it("mostra o estado de carregamento e depois a lista de registos do paciente (EP-034)", async () => {
    const servico = montar();
    expect(screen.getByRole("status")).toHaveTextContent("A carregar…");

    const tabela = await screen.findByRole("table");
    expect(servico.obterHistorico).toHaveBeenCalledWith(3, {});
    expect(screen.getByText(/Paciente:/)).toHaveTextContent("Bobi · Cão");
    const linhas = within(tabela).getAllByRole("row");
    expect(linhas).toHaveLength(3);
    expect(linhas[1]).toHaveTextContent("20/11/2026");
    expect(linhas[1]).toHaveTextContent("Dra. Ana");
    expect(linhas[1]).toHaveTextContent("Otite externa");
    expect(linhas[2]).toHaveTextContent("Dr. Paulo");
    expect(linhas[2]).toHaveTextContent("—");
  });

  it("abre o registo completo: notas, diagnóstico, tratamentos e prescrições (EP-033)", async () => {
    const servico = montar();
    const u = userEvent.setup();
    const detalhe = await abrirRegisto(u);

    expect(servico.obter).toHaveBeenCalledWith(9);
    expect(
      await within(detalhe).findByText("Cerúmen abundante no ouvido esquerdo."),
    ).toBeInTheDocument();
    expect(within(detalhe).getByText("Otite externa")).toBeInTheDocument();
    expect(
      within(detalhe).getByRole("list", { name: "Tratamentos" }),
    ).toHaveTextContent("Limpeza auricular — aplicado em 20/11/2026");
    expect(
      within(detalhe).getByRole("list", { name: "Prescrições" }),
    ).toHaveTextContent("Otomax — 5 gotas, 2x por dia, durante 7 dias");
    expect(within(detalhe).getByText(/Registado por/)).toHaveTextContent(
      "Última alteração",
    );
  });

  it("o Veterinário tem a ação «Alterar registo», que abre o registo da consulta (I115)", async () => {
    montar();
    const u = userEvent.setup();
    const detalhe = await abrirRegisto(u);
    expect(
      await within(detalhe).findByRole("link", { name: "Alterar registo" }),
    ).toHaveAttribute("href", "/agenda/41/registo-clinico");
  });

  it("paciente sem registos: mostra a mensagem própria", async () => {
    montar({
      servico: servicoFalso({ animal: HISTORICO.animal, registos: [] }),
    });
    expect(
      await screen.findByText("Este paciente ainda não tem registos clínicos."),
    ).toBeInTheDocument();
    expect(screen.queryByRole("table")).not.toBeInTheDocument();
  });
});

describe("W13 — histórico clínico do Auxiliar Veterinário (I116)", () => {
  it("vê o mesmo registo, só de leitura, sem botões de criação ou alteração", async () => {
    montar({ perfil: "AuxiliarVeterinario" });
    const u = userEvent.setup();
    expect(await screen.findByText(/Vista só de leitura/)).toBeInTheDocument();

    const detalhe = await abrirRegisto(u);
    expect(
      await within(detalhe).findByText("Cerúmen abundante no ouvido esquerdo."),
    ).toBeInTheDocument();
    expect(
      screen.queryByRole("link", { name: /Alterar|Criar|Editar/ }),
    ).not.toBeInTheDocument();
    expect(
      screen.queryByRole("button", { name: /Alterar|Criar|Editar|Guardar/ }),
    ).not.toBeInTheDocument();
  });
});

describe("W13 — filtro por período (I116)", () => {
  it("pede à API só o período escolhido e mostra a mensagem própria quando não há registos", async () => {
    const servico = montar();
    await screen.findByRole("table");
    servico.obterHistorico.mockResolvedValueOnce({
      animal: HISTORICO.animal,
      registos: [],
    });

    fireEvent.change(screen.getByLabelText("Desde"), {
      target: { value: "2026-01-01" },
    });
    fireEvent.change(screen.getByLabelText("Até"), {
      target: { value: "2026-03-31" },
    });
    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Filtrar" }));

    await waitFor(() =>
      expect(servico.obterHistorico).toHaveBeenLastCalledWith(3, {
        dataInicio: "2026-01-01",
        dataFim: "2026-03-31",
      }),
    );
    expect(
      await screen.findByText(
        "Não há registos clínicos deste paciente no período escolhido.",
      ),
    ).toBeInTheDocument();
  });

  it("valida o período antes de pedir: a data de fim não pode ser anterior à de início", async () => {
    const servico = montar();
    await screen.findByRole("table");
    fireEvent.change(screen.getByLabelText("Desde"), {
      target: { value: "2026-11-20" },
    });
    fireEvent.change(screen.getByLabelText("Até"), {
      target: { value: "2026-11-01" },
    });
    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Filtrar" }));

    expect(screen.getByText(MENSAGEM_FIM_ANTERIOR)).toBeInTheDocument();
    expect(servico.obterHistorico).toHaveBeenCalledTimes(1);
  });
});

describe("W13 — erros e falha de ligação (I116)", () => {
  it("falha de ligação: mostra a mensagem e permite tentar novamente", async () => {
    const servico = servicoFalso();
    servico.obterHistorico
      .mockReset()
      .mockRejectedValueOnce(new ApiRequestError(0, "rede"))
      .mockResolvedValueOnce(HISTORICO);
    montar({ servico });

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor. Verifique a ligação.",
    );
    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Tentar novamente" }));

    expect(await screen.findByRole("table")).toBeInTheDocument();
    expect(screen.queryByRole("alert")).not.toBeInTheDocument();
    expect(servico.obterHistorico).toHaveBeenCalledTimes(2);
  });

  it("403 da API: mostra a mensagem da API sem dados clínicos", async () => {
    const servico = servicoFalso();
    servico.obterHistorico
      .mockReset()
      .mockRejectedValue(
        new ApiRequestError(
          403,
          "semPermissao",
          "Não tem permissão para realizar esta operação.",
        ),
      );
    montar({ servico });
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não tem permissão para realizar esta operação.",
    );
    expect(screen.queryByRole("table")).not.toBeInTheDocument();
  });

  it("erro ao abrir um registo: mostra o erro no detalhe e permite tentar novamente", async () => {
    const servico = servicoFalso();
    servico.obter
      .mockReset()
      .mockRejectedValueOnce(new ApiRequestError(500, "servidor"))
      .mockResolvedValueOnce(REGISTO);
    montar({ servico });
    const u = userEvent.setup();
    const detalhe = await abrirRegisto(u);

    expect(await within(detalhe).findByRole("alert")).toHaveTextContent(
      "Ocorreu um erro no servidor. Tente novamente mais tarde.",
    );
    await u.click(
      within(detalhe).getByRole("button", { name: "Tentar novamente" }),
    );
    expect(
      await within(detalhe).findByText("Cerúmen abundante no ouvido esquerdo."),
    ).toBeInTheDocument();
  });
});

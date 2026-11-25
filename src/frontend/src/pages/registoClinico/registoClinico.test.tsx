import { act, render, screen, waitFor, within } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { RouterProvider, createMemoryRouter } from "react-router-dom";
import { afterEach, describe, expect, it, vi } from "vitest";
import { ApiRequestError } from "../../core/api";
import {
  AuthContext,
  type AuthContextValue,
} from "../../core/auth/contextoAuth";
import { perfilPodeExecutar } from "../../core/auth/permissoes";
import type { PerfilWeb } from "../../models/auth";
import type { EstadoConsulta, MarcacaoDetalhe } from "../../models/agenda";
import type { RegistoClinico } from "../../models/registosClinicos";
import type { ServicoAgenda } from "../../services/AgendaService";
import type { ServicoRegistosClinicos } from "../../services/RegistosClinicosService";
import { MarcacaoDetalhePage } from "../agenda/MarcacaoDetalhePage";
import { MENSAGEM_SEM_ELEMENTO_CLINICO } from "./validacaoRegistoClinico";
import { RegistoClinicoPage } from "./RegistoClinicoPage";

function consultaEm(estado: EstadoConsulta): MarcacaoDetalhe {
  return {
    id: 41,
    dataHora: "2026-11-20T10:00:00Z",
    duracaoMinutos: 30,
    estado,
    motivo: "Comichão nos ouvidos",
    animal: { id: 3, nome: "Bobi", especie: "Cão" },
    cliente: { id: 7, nome: "João Costa" },
    veterinario: { id: 5, nome: "Dra. Ana" },
    chegadaEm: null,
    notasPreparacao: [],
    observacao: null,
  };
}

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
  atualizadoEm: null,
  atualizadoPor: null,
};

function servicosFalsos(
  estado: EstadoConsulta = "Realizada",
  existente: RegistoClinico | null = null,
) {
  const agenda = {
    obterConsulta: vi.fn().mockResolvedValue(consultaEm(estado)),
  };
  const registos: {
    [K in keyof ServicoRegistosClinicos]: ReturnType<typeof vi.fn>;
  } = {
    criar: vi.fn().mockImplementation(async (dados) => ({
      ...REGISTO,
      notasClinicas: dados.notasClinicas,
      diagnostico: dados.diagnostico,
      tratamentos: dados.tratamentos.map((t: object, i: number) => ({
        id: 100 + i,
        ...t,
      })),
      prescricoes: dados.prescricoes.map((p: object, i: number) => ({
        id: 200 + i,
        ...p,
      })),
    })),
    atualizar: vi.fn().mockResolvedValue({
      ...REGISTO,
      atualizadoEm: "2026-11-20T11:00:00Z",
      atualizadoPor: "Dra. Ana",
    }),
    obter: vi.fn().mockResolvedValue(REGISTO),
    obterHistorico: vi.fn(),
    obterDaConsulta: vi.fn().mockResolvedValue(existente),
  };
  return { agenda, registos };
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

/**
 * Monta as páginas num data router em memória (como a aplicação, que usa
 * createBrowserRouter), para o bloqueio da navegação (useBlocker) funcionar.
 * `anteriores` são entradas do histórico antes de `caminho` (para testar o
 * botão «Retroceder»).
 */
function montar(
  caminho: string,
  {
    perfil = "Veterinario" as PerfilWeb,
    servicos = servicosFalsos(),
    anteriores = [] as string[],
  } = {},
) {
  const entradas = [...anteriores, caminho];
  const router = createMemoryRouter(
    [
      {
        path: "/agenda/:id/registo-clinico",
        element: (
          <RegistoClinicoPage
            servico={servicos.registos as unknown as ServicoRegistosClinicos}
            servicoAgenda={servicos.agenda}
          />
        ),
      },
      {
        path: "/agenda/:id",
        element: (
          <MarcacaoDetalhePage
            servico={servicos.agenda as unknown as ServicoAgenda}
          />
        ),
      },
    ],
    { initialEntries: entradas, initialIndex: entradas.length - 1 },
  );
  render(
    <AuthContext.Provider value={sessaoDe(perfil)}>
      <RouterProvider router={router} />
    </AuthContext.Provider>,
  );
  return Object.assign(servicos, { router });
}

const CAMINHO = "/agenda/41/registo-clinico";

afterEach(() => {
  vi.restoreAllMocks();
});

describe("W13 — criar o registo clínico da consulta (I115)", () => {
  it("procura o registo da consulta pelo paciente e, sem registo, mostra o formulário de criação", async () => {
    const { registos } = montar(CAMINHO);
    expect(
      await screen.findByRole("button", { name: "Criar registo clínico" }),
    ).toBeInTheDocument();
    expect(registos.obterDaConsulta).toHaveBeenCalledWith(3, 41);
    expect(
      screen.getByText(/ainda não tem registo clínico/),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("group", { name: "Tratamentos" }),
    ).toHaveTextContent("Sem tratamentos.");
  });

  it("valida antes de confirmar: sem nenhum elemento clínico não envia nada", async () => {
    const { registos } = montar(CAMINHO);
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Criar registo clínico" }),
    );
    expect(
      await screen.findByText(MENSAGEM_SEM_ELEMENTO_CLINICO),
    ).toBeInTheDocument();
    expect(registos.criar).not.toHaveBeenCalled();
  });

  it("valida os campos obrigatórios de tratamentos e prescrições", async () => {
    const { registos } = montar(CAMINHO);
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Adicionar tratamento" }),
    );
    await u.click(screen.getByRole("button", { name: "Adicionar prescrição" }));
    await u.click(
      screen.getByRole("button", { name: "Criar registo clínico" }),
    );

    expect(
      screen.getByText("A descrição do tratamento é obrigatória."),
    ).toBeInTheDocument();
    expect(
      screen.getByText("O medicamento da prescrição é obrigatório."),
    ).toBeInTheDocument();
    expect(
      screen.getByLabelText("Medicamento da prescrição 1"),
    ).toHaveAttribute("aria-invalid", "true");
    expect(registos.criar).not.toHaveBeenCalled();
  });

  it("cria o registo com notas, diagnóstico, tratamento e prescrição (EP-031)", async () => {
    const { registos } = montar(CAMINHO);
    const u = userEvent.setup();
    await u.type(
      await screen.findByLabelText("Notas clínicas"),
      "Ouvido inflamado.",
    );
    await u.type(screen.getByLabelText("Diagnóstico"), "Otite externa");

    await u.click(screen.getByRole("button", { name: "Adicionar tratamento" }));
    // A data de aplicação começa no dia da consulta.
    expect(
      screen.getByLabelText("Data de aplicação do tratamento 1"),
    ).toHaveValue("2026-11-20");
    await u.type(
      screen.getByLabelText("Descrição do tratamento 1"),
      "Limpeza auricular",
    );

    await u.click(screen.getByRole("button", { name: "Adicionar prescrição" }));
    await u.type(
      screen.getByLabelText("Medicamento da prescrição 1"),
      "Otomax",
    );
    await u.type(screen.getByLabelText("Dosagem da prescrição 1"), "5 gotas");
    await u.type(
      screen.getByLabelText("Frequência da prescrição 1"),
      "2x por dia",
    );
    await u.type(
      screen.getByLabelText("Duração em dias da prescrição 1 (opcional)"),
      "7",
    );

    await u.click(
      screen.getByRole("button", { name: "Criar registo clínico" }),
    );

    await waitFor(() => expect(registos.criar).toHaveBeenCalledTimes(1));
    expect(registos.criar).toHaveBeenCalledWith({
      consultaId: 41,
      notasClinicas: "Ouvido inflamado.",
      diagnostico: "Otite externa",
      tratamentos: [
        { descricao: "Limpeza auricular", dataAplicacao: "2026-11-20" },
      ],
      prescricoes: [
        {
          medicamento: "Otomax",
          dosagem: "5 gotas",
          frequencia: "2x por dia",
          duracaoDias: 7,
        },
      ],
    });
    expect(
      await screen.findByText("Registo clínico criado para esta consulta."),
    ).toBeInTheDocument();
    // Depois de criado, o mesmo ecrã passa a alterar o registo.
    expect(
      screen.getByRole("button", { name: "Guardar alterações" }),
    ).toBeInTheDocument();
    expect(
      screen.queryByText("Tem alterações por guardar."),
    ).not.toBeInTheDocument();
  });

  it("mostra o erro da API no contexto do formulário (ex.: 403 de outro Veterinário)", async () => {
    const servicos = servicosFalsos();
    servicos.registos.criar.mockRejectedValue(
      new ApiRequestError(
        403,
        "semPermissao",
        "A consulta pertence a outro Veterinário.",
      ),
    );
    montar(CAMINHO, { servicos });
    const u = userEvent.setup();
    await u.type(await screen.findByLabelText("Diagnóstico"), "Otite");
    await u.click(
      screen.getByRole("button", { name: "Criar registo clínico" }),
    );

    const formulario = screen.getByRole("form", {
      name: "Registo clínico da consulta",
    });
    expect(await within(formulario).findByRole("alert")).toHaveTextContent(
      "A consulta pertence a outro Veterinário.",
    );
    // O que foi escrito não se perde.
    expect(screen.getByLabelText("Diagnóstico")).toHaveValue("Otite");
  });

  it("409: a consulta já tinha registo — carrega o que está guardado e avisa", async () => {
    const servicos = servicosFalsos();
    servicos.registos.criar.mockRejectedValue(
      new ApiRequestError(
        409,
        "conflito",
        "A consulta já tem registo clínico.",
      ),
    );
    servicos.registos.obterDaConsulta
      .mockResolvedValueOnce(null)
      .mockResolvedValueOnce(REGISTO);
    montar(CAMINHO, { servicos });
    const u = userEvent.setup();
    await u.type(await screen.findByLabelText("Diagnóstico"), "Otite");
    await u.click(
      screen.getByRole("button", { name: "Criar registo clínico" }),
    );

    expect(
      await screen.findByText(/Esta consulta já tinha registo clínico/),
    ).toBeInTheDocument();
    expect(screen.getByLabelText("Diagnóstico")).toHaveValue("Otite externa");
    expect(
      screen.getByRole("button", { name: "Guardar alterações" }),
    ).toBeInTheDocument();
  });
});

describe("W13 — atualizar o registo clínico (I115)", () => {
  it("carrega o registo existente e guarda as alterações mantendo os ids (EP-032)", async () => {
    const servicos = servicosFalsos("Realizada", REGISTO);
    montar(CAMINHO, { servicos });
    const u = userEvent.setup();

    const diagnostico = await screen.findByLabelText("Diagnóstico");
    expect(diagnostico).toHaveValue("Otite externa");
    expect(screen.getByText(/Registado por/)).toHaveTextContent("Dra. Ana");

    await u.clear(diagnostico);
    await u.type(diagnostico, "Otite externa bilateral");
    await u.click(screen.getByRole("button", { name: "Remover tratamento 1" }));
    expect(screen.getByText("Tem alterações por guardar.")).toBeInTheDocument();

    await u.click(screen.getByRole("button", { name: "Guardar alterações" }));

    await waitFor(() => expect(servicos.registos.atualizar).toHaveBeenCalled());
    expect(servicos.registos.atualizar).toHaveBeenCalledWith(9, {
      notasClinicas: "Cerúmen abundante no ouvido esquerdo.",
      diagnostico: "Otite externa bilateral",
      tratamentos: [],
      prescricoes: [
        {
          id: 2,
          medicamento: "Otomax",
          dosagem: "5 gotas",
          frequencia: "2x por dia",
          duracaoDias: 7,
        },
      ],
    });
    expect(servicos.registos.criar).not.toHaveBeenCalled();
    expect(
      await screen.findByText("Registo clínico atualizado."),
    ).toBeInTheDocument();
  });

  it("valida a duração da prescrição (inteiro entre 1 e 32767)", async () => {
    const servicos = servicosFalsos("Realizada", REGISTO);
    montar(CAMINHO, { servicos });
    const u = userEvent.setup();
    const duracao = await screen.findByLabelText(
      "Duração em dias da prescrição 1 (opcional)",
    );
    await u.clear(duracao);
    await u.type(duracao, "0");
    await u.click(screen.getByRole("button", { name: "Guardar alterações" }));
    expect(
      screen.getByText(/deve ser um número inteiro de dias entre 1 e 32767/),
    ).toBeInTheDocument();
    expect(servicos.registos.atualizar).not.toHaveBeenCalled();
  });
});

describe("W13 — alterações por guardar (I115, critério 6)", () => {
  it("pede confirmação antes de sair e fica no ecrã se o utilizador cancelar", async () => {
    const confirmar = vi.spyOn(window, "confirm").mockReturnValue(false);
    montar(CAMINHO);
    const u = userEvent.setup();
    await u.type(await screen.findByLabelText("Notas clínicas"), "Rascunho");
    await u.click(screen.getByRole("link", { name: "Voltar à marcação" }));

    await waitFor(() => expect(confirmar).toHaveBeenCalledTimes(1));
    expect(
      screen.getByRole("heading", { name: "Registo clínico da consulta" }),
    ).toBeInTheDocument();
    expect(screen.getByLabelText("Notas clínicas")).toHaveValue("Rascunho");
  });

  it("sai se o utilizador confirmar", async () => {
    vi.spyOn(window, "confirm").mockReturnValue(true);
    montar(CAMINHO);
    const u = userEvent.setup();
    await u.type(await screen.findByLabelText("Notas clínicas"), "Rascunho");
    await u.click(screen.getByRole("link", { name: "Voltar à marcação" }));
    expect(
      await screen.findByRole("heading", { name: "Detalhe da marcação" }),
    ).toBeInTheDocument();
  });

  it("botão «Retroceder» do navegador: pede confirmação e, ao cancelar, mantém os dados (D1 da revisão)", async () => {
    const confirmar = vi.spyOn(window, "confirm").mockReturnValue(false);
    const { router } = montar(CAMINHO, { anteriores: ["/agenda/41"] });
    const u = userEvent.setup();
    await u.type(await screen.findByLabelText("Notas clínicas"), "Rascunho");

    await act(async () => {
      await router.navigate(-1);
    });

    await waitFor(() =>
      expect(confirmar).toHaveBeenCalledWith(
        "Tem alterações por guardar. Se sair agora, perde-as. Quer mesmo sair?",
      ),
    );
    expect(router.state.location.pathname).toBe(CAMINHO);
    expect(
      screen.getByRole("heading", { name: "Registo clínico da consulta" }),
    ).toBeInTheDocument();
    expect(screen.getByLabelText("Notas clínicas")).toHaveValue("Rascunho");
  });

  it("botão «Retroceder» do navegador: sai se o utilizador confirmar", async () => {
    vi.spyOn(window, "confirm").mockReturnValue(true);
    const { router } = montar(CAMINHO, { anteriores: ["/agenda/41"] });
    const u = userEvent.setup();
    await u.type(await screen.findByLabelText("Notas clínicas"), "Rascunho");

    await act(async () => {
      await router.navigate(-1);
    });

    expect(
      await screen.findByRole("heading", { name: "Detalhe da marcação" }),
    ).toBeInTheDocument();
    expect(router.state.location.pathname).toBe("/agenda/41");
  });

  it("botão «Retroceder» sem alterações: sai sem perguntar", async () => {
    const confirmar = vi.spyOn(window, "confirm");
    const { router } = montar(CAMINHO, { anteriores: ["/agenda/41"] });
    await screen.findByLabelText("Notas clínicas");

    await act(async () => {
      await router.navigate(-1);
    });

    expect(
      await screen.findByRole("heading", { name: "Detalhe da marcação" }),
    ).toBeInTheDocument();
    expect(confirmar).not.toHaveBeenCalled();
  });

  it("sem alterações sai sem perguntar", async () => {
    const confirmar = vi.spyOn(window, "confirm");
    montar(CAMINHO);
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("link", { name: "Voltar à marcação" }),
    );
    expect(
      await screen.findByRole("heading", { name: "Detalhe da marcação" }),
    ).toBeInTheDocument();
    expect(confirmar).not.toHaveBeenCalled();
  });
});

describe("W13 — estado da consulta e acesso (I115)", () => {
  it.each(["Pendente", "Cancelada"] as const)(
    "consulta %s: não mostra o formulário nem procura o registo",
    async (estado) => {
      const servicos = servicosFalsos(estado);
      montar(CAMINHO, { servicos });
      expect(
        await screen.findByText(/Só é possível fazer o registo clínico/),
      ).toBeInTheDocument();
      expect(
        screen.queryByRole("form", { name: "Registo clínico da consulta" }),
      ).not.toBeInTheDocument();
      expect(servicos.registos.obterDaConsulta).not.toHaveBeenCalled();
    },
  );

  it("o Veterinário vê «Abrir registo clínico» no detalhe de uma consulta Realizada", async () => {
    montar("/agenda/41", { perfil: "Veterinario" });
    expect(
      await screen.findByRole("link", { name: "Abrir registo clínico" }),
    ).toHaveAttribute("href", "/agenda/41/registo-clinico");
  });

  it("a Rececionista não vê a ação de registo clínico", async () => {
    montar("/agenda/41", { perfil: "Rececionista" });
    expect(
      await screen.findByRole("heading", { name: "Detalhe da marcação" }),
    ).toBeInTheDocument();
    await screen.findByLabelText("Dados da marcação");
    expect(
      screen.queryByRole("link", { name: "Abrir registo clínico" }),
    ).not.toBeInTheDocument();
  });

  it("consulta Pendente: o Veterinário não tem a ligação no detalhe", async () => {
    montar("/agenda/41", {
      perfil: "Veterinario",
      servicos: servicosFalsos("Pendente"),
    });
    expect(
      await screen.findByText(/só está disponível em consultas Confirmada ou/),
    ).toBeInTheDocument();
    expect(
      screen.queryByRole("link", { name: "Abrir registo clínico" }),
    ).not.toBeInTheDocument();
  });
});

import { render, screen, waitFor, within } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { MemoryRouter } from "react-router-dom";
import { describe, expect, it, vi } from "vitest";
import { ApiRequestError } from "../../core/api";
import {
  AuthContext,
  type AuthContextValue,
} from "../../core/auth/contextoAuth";
import { perfilPodeExecutar } from "../../core/auth/permissoes";
import { hojeEmLisboa, somarDias } from "../../core/horaLisboa";
import type { PerfilWeb } from "../../models/auth";
import type { MarcacaoDetalhe, MarcacaoResumo } from "../../models/agenda";
import type { PacienteDetalhe } from "../../models/pacientes";
import type { ServicoAgenda } from "../../services/AgendaService";
import { AgendaPage } from "./AgendaPage";
import { PreparacaoConsultaPage } from "./PreparacaoConsultaPage";

const bobi: MarcacaoResumo = {
  id: 41,
  dataHora: "2026-11-20T10:00:00Z",
  duracaoMinutos: 30,
  estado: "Confirmada",
  motivo: "Vacina",
  animal: { id: 3, nome: "Bobi", especie: "Cão" },
  cliente: { id: 7, nome: "João Costa" },
  veterinario: { id: 5, nome: "Dra. Ana" },
  chegadaEm: "2026-11-20T09:52:00Z",
};
const mia: MarcacaoResumo = {
  ...bobi,
  id: 42,
  dataHora: "2026-11-20T10:30:00Z",
  estado: "Pendente",
  motivo: null,
  animal: { id: 8, nome: "Mia", especie: "Gato" },
  veterinario: { id: 6, nome: "Dr. Bruno" },
  chegadaEm: null,
};
const detalheBobi: MarcacaoDetalhe = {
  ...bobi,
  notasPreparacao: [
    {
      id: 1,
      texto: "Sala 2 preparada.",
      autor: "Carla",
      criadaEm: "2026-11-20T09:40:00Z",
    },
  ],
  observacao: null,
};
const fichaBobi: PacienteDetalhe = {
  id: 3,
  nome: "Bobi",
  especie: "Cão",
  raca: "Labrador Retriever",
  sexo: "Macho",
  dataNascimento: "2022-05-10",
  idadeAnos: 4,
  peso: 28.4,
  cliente: { id: 7, nome: "João Costa", telefone: "960000001" },
  atualizadoEm: null,
};

function servicoFalso(): {
  [K in keyof ServicoAgenda]: ReturnType<typeof vi.fn>;
} {
  return {
    listarVeterinarios: vi.fn().mockResolvedValue([
      { id: 5, nome: "Dra. Ana" },
      { id: 6, nome: "Dr. Bruno" },
    ]),
    obterDisponibilidade: vi.fn(),
    listarConsultas: vi.fn().mockResolvedValue({
      itens: [bobi, mia],
      pagina: 1,
      tamanho: 100,
      total: 2,
    }),
    obterConsulta: vi.fn().mockResolvedValue(detalheBobi),
    criar: vi.fn(),
    atualizar: vi.fn(),
    alterarEstado: vi.fn(),
    registarChegada: vi.fn(),
    registarNotaPreparacao: vi.fn().mockResolvedValue({
      id: 2,
      consultaId: 41,
      texto: "Tutor pediu corte de unhas.",
      autor: "Carla",
      criadaEm: "2026-11-20T09:45:00Z",
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

function montarAgenda(perfil: PerfilWeb, servico = servicoFalso()) {
  render(
    <AuthContext.Provider value={sessaoDe(perfil)}>
      <MemoryRouter>
        <AgendaPage servico={servico as unknown as ServicoAgenda} />
      </MemoryRouter>
    </AuthContext.Provider>,
  );
  return servico;
}

function montarPreparacao(
  servico = servicoFalso(),
  pacientes = { obter: vi.fn().mockResolvedValue(fichaBobi) },
) {
  render(
    <AuthContext.Provider value={sessaoDe("AuxiliarVeterinario")}>
      <MemoryRouter>
        <PreparacaoConsultaPage
          servico={servico as unknown as ServicoAgenda}
          servicoPacientes={pacientes}
        />
      </MemoryRouter>
    </AuthContext.Provider>,
  );
  return { servico, pacientes };
}

describe("W09 — agenda", () => {
  it("a Rececionista vê a agenda do dia com hora, paciente, Veterinário, estado e chegada", async () => {
    const servico = montarAgenda("Rececionista");
    const linha = (await screen.findByText("Bobi")).closest("tr")!;
    expect(linha).toHaveTextContent("10:00");
    expect(linha).toHaveTextContent("João Costa");
    expect(linha).toHaveTextContent("Dra. Ana");
    expect(linha).toHaveTextContent("Confirmada");
    expect(linha).toHaveTextContent("09:52");
    expect(
      within(linha).getByRole("link", { name: /Ver marcação de Bobi/ }),
    ).toHaveAttribute("href", "/agenda/41");
    expect(screen.getByRole("link", { name: "Nova marcação" })).toBeVisible();
    const hoje = hojeEmLisboa();
    expect(servico.listarConsultas).toHaveBeenCalledWith({
      dataInicio: hoje,
      dataFim: hoje,
      pagina: 1,
      tamanho: 100,
    });
  });

  it("filtra por Veterinário e muda de dia", async () => {
    const servico = montarAgenda("Rececionista");
    await screen.findByText("Bobi");
    const u = userEvent.setup();
    await u.selectOptions(
      screen.getByLabelText("Veterinário"),
      await screen.findByRole("option", { name: "Dr. Bruno" }),
    );
    await waitFor(() =>
      expect(servico.listarConsultas).toHaveBeenLastCalledWith(
        expect.objectContaining({ veterinarioId: 6 }),
      ),
    );
    await u.click(screen.getByRole("button", { name: "Dia seguinte" }));
    const amanha = somarDias(hojeEmLisboa(), 1);
    await waitFor(() =>
      expect(servico.listarConsultas).toHaveBeenLastCalledWith(
        expect.objectContaining({
          dataInicio: amanha,
          dataFim: amanha,
          veterinarioId: 6,
        }),
      ),
    );
  });

  it("o Veterinário vê só as suas consultas, sem filtro de Veterinário nem «Nova marcação»", async () => {
    const servico = montarAgenda("Veterinario");
    await screen.findByText("Bobi");
    expect(
      screen.getByText("Mostra só as consultas que lhe estão atribuídas."),
    ).toBeInTheDocument();
    expect(screen.queryByLabelText("Veterinário")).toBeNull();
    expect(screen.queryByRole("link", { name: "Nova marcação" })).toBeNull();
    expect(servico.listarVeterinarios).not.toHaveBeenCalled();
  });

  it("mostra o erro quando a agenda falha", async () => {
    const servico = servicoFalso();
    servico.listarConsultas.mockRejectedValue(new ApiRequestError(0, "rede"));
    montarAgenda("Rececionista", servico);
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor",
    );
  });
});

describe("W12 — preparação de consultas", () => {
  it("lista as consultas do dia e mostra a informação de preparação da escolhida", async () => {
    const { servico, pacientes } = montarPreparacao();
    const linha = (await screen.findByText("Bobi")).closest("tr")!;
    await userEvent
      .setup()
      .click(within(linha).getByRole("button", { name: /Selecionar/ }));

    expect(servico.obterConsulta).toHaveBeenCalledWith(41);
    expect(pacientes.obter).toHaveBeenCalledWith(3);
    const info = await screen.findByLabelText("Informação de preparação");
    expect(info).toHaveTextContent("20/11/2026, 10:00");
    expect(info).toHaveTextContent("Dra. Ana");
    expect(info).toHaveTextContent("Vacina");
    expect(info).toHaveTextContent("Labrador Retriever");
    expect(info).toHaveTextContent("28,4 kg");
    expect(info).toHaveTextContent("João Costa · 960000001");
    expect(
      screen.getByRole("list", { name: "Notas de apoio" }),
    ).toHaveTextContent("Sala 2 preparada. — Carla");
  });

  it("regista uma nota de apoio e junta-a à lista", async () => {
    const { servico } = montarPreparacao();
    const u = userEvent.setup();
    const linha = (await screen.findByText("Bobi")).closest("tr")!;
    await u.click(within(linha).getByRole("button", { name: /Selecionar/ }));
    await u.type(
      await screen.findByLabelText("Nova nota de apoio (não clínica)"),
      "Tutor pediu corte de unhas.",
    );
    await u.click(
      screen.getByRole("button", { name: "Guardar informação de apoio" }),
    );
    expect(servico.registarNotaPreparacao).toHaveBeenCalledWith(
      41,
      "Tutor pediu corte de unhas.",
    );
    expect(
      await screen.findByText("Informação de apoio guardada."),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("list", { name: "Notas de apoio" }),
    ).toHaveTextContent("Tutor pediu corte de unhas. — Carla");
    expect(
      screen.getByLabelText("Nova nota de apoio (não clínica)"),
    ).toHaveValue("");
  });

  it("não envia uma nota vazia", async () => {
    const { servico } = montarPreparacao();
    const u = userEvent.setup();
    const linha = (await screen.findByText("Bobi")).closest("tr")!;
    await u.click(within(linha).getByRole("button", { name: /Selecionar/ }));
    await u.click(
      await screen.findByRole("button", {
        name: "Guardar informação de apoio",
      }),
    );
    expect(
      screen.getByText("Escreva a informação de apoio."),
    ).toBeInTheDocument();
    expect(servico.registarNotaPreparacao).not.toHaveBeenCalled();
  });

  it("sem autorização de apoio (403) mostra a mensagem e deixa de oferecer o registo", async () => {
    const servico = servicoFalso();
    servico.registarNotaPreparacao.mockRejectedValue(
      new ApiRequestError(
        403,
        "semPermissao",
        "O Auxiliar Veterinário não tem autorização para registar informação de apoio.",
      ),
    );
    montarPreparacao(servico);
    const u = userEvent.setup();
    const linha = (await screen.findByText("Bobi")).closest("tr")!;
    await u.click(within(linha).getByRole("button", { name: /Selecionar/ }));
    await u.type(
      await screen.findByLabelText("Nova nota de apoio (não clínica)"),
      "Nota",
    );
    await u.click(
      screen.getByRole("button", { name: "Guardar informação de apoio" }),
    );
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "não tem autorização para registar informação de apoio",
    );
    expect(
      screen.getByText(/A sua conta não tem autorização/),
    ).toBeInTheDocument();
    expect(
      screen.queryByLabelText("Nova nota de apoio (não clínica)"),
    ).toBeNull();
  });

  it("não mostra ações de agenda nem de edição clínica", async () => {
    montarPreparacao();
    await screen.findByText("Bobi");
    expect(screen.queryByRole("link", { name: "Nova marcação" })).toBeNull();
    expect(
      screen.queryByRole("button", { name: "Registar chegada" }),
    ).toBeNull();
  });
});

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
import { hojeEmLisboa, somarDias } from "../../core/horaLisboa";
import type { PerfilWeb } from "../../models/auth";
import type { EstadoConsulta, MarcacaoDetalhe } from "../../models/agenda";
import type { ServicoAgenda } from "../../services/AgendaService";
import { MarcacaoDetalhePage } from "./MarcacaoDetalhePage";
import { MarcacaoFormPage } from "./MarcacaoFormPage";

function marcacaoEm(estado: EstadoConsulta): MarcacaoDetalhe {
  return {
    id: 41,
    dataHora: "2026-11-20T10:00:00Z",
    duracaoMinutos: 30,
    estado,
    motivo: "Vacina",
    animal: { id: 3, nome: "Bobi", especie: "Cão" },
    cliente: { id: 7, nome: "João Costa" },
    veterinario: { id: 5, nome: "Dra. Ana" },
    chegadaEm: null,
    notasPreparacao: [],
  };
}

const disponibilidade = {
  veterinarioId: 5,
  duracaoConsultaMinutos: 30,
  dias: [
    { data: "2026-11-20", horariosLivres: ["09:30", "10:30"] },
    { data: "2026-11-21", horariosLivres: [] },
  ],
};

function servicoFalso(estado: EstadoConsulta = "Pendente"): {
  [K in keyof ServicoAgenda]: ReturnType<typeof vi.fn>;
} {
  return {
    listarVeterinarios: vi.fn().mockResolvedValue([
      { id: 5, nome: "Dra. Ana" },
      { id: 6, nome: "Dr. Paulo" },
    ]),
    obterDisponibilidade: vi.fn().mockResolvedValue(disponibilidade),
    obterConsulta: vi.fn().mockResolvedValue(marcacaoEm(estado)),
    criar: vi.fn().mockResolvedValue(marcacaoEm("Pendente")),
    atualizar: vi.fn().mockResolvedValue(marcacaoEm(estado)),
    alterarEstado: vi.fn().mockResolvedValue(undefined),
  };
}

function sessaoDe(perfil: PerfilWeb): AuthContextValue {
  return {
    sessao: null,
    utilizador: { id: 1, nome: "Rita", email: "rita@clinica.pt", perfil },
    aRestaurar: false,
    login: vi.fn(),
    logout: vi.fn(),
    temPerfil: (perfis) => perfis.includes(perfil),
    pode: (acao) => perfilPodeExecutar(perfil, acao),
    sessaoTerminada: false,
  };
}

const PACIENTE = { id: 3, nome: "Bobi", cliente: "João Costa" };

function montar(
  entrada: string | { pathname: string; state: unknown },
  {
    perfil = "Rececionista" as PerfilWeb,
    servico = servicoFalso(),
    pacientes = { listar: vi.fn() },
  } = {},
) {
  const s = servico as unknown as ServicoAgenda;
  render(
    <AuthContext.Provider value={sessaoDe(perfil)}>
      <MemoryRouter initialEntries={[entrada]}>
        <Routes>
          <Route
            path="/agenda/nova"
            element={
              <MarcacaoFormPage servico={s} servicoPacientes={pacientes} />
            }
          />
          <Route
            path="/agenda/:id/editar"
            element={
              <MarcacaoFormPage servico={s} servicoPacientes={pacientes} />
            }
          />
          <Route
            path="/agenda/:id"
            element={<MarcacaoDetalhePage servico={s} />}
          />
        </Routes>
      </MemoryRouter>
    </AuthContext.Provider>,
  );
  return { servico, pacientes };
}

const novaComPaciente = {
  pathname: "/agenda/nova",
  state: { paciente: PACIENTE },
};

async function escolherVeterinarioEHorario(
  u: ReturnType<typeof userEvent.setup>,
  hora = "10:30",
) {
  await u.selectOptions(
    await screen.findByLabelText("Veterinário"),
    await screen.findByRole("option", { name: "Dra. Ana" }),
  );
  await u.click(
    screen.getByRole("button", { name: "Consultar disponibilidade" }),
  );
  await u.click(
    await screen.findByRole("button", { name: new RegExp(`às ${hora}$`) }),
  );
}

describe("W10 — nova marcação", () => {
  it("consulta a disponibilidade do Veterinário e mostra só os horários livres", async () => {
    const { servico } = montar(novaComPaciente);
    const u = userEvent.setup();
    await u.selectOptions(
      await screen.findByLabelText("Veterinário"),
      await screen.findByRole("option", { name: "Dra. Ana" }),
    );
    await u.click(
      screen.getByRole("button", { name: "Consultar disponibilidade" }),
    );
    const hoje = hojeEmLisboa();
    expect(servico.obterDisponibilidade).toHaveBeenCalledWith(
      5,
      hoje,
      somarDias(hoje, 6),
    );
    const livres = await screen.findByRole("group", {
      name: "Horários livres",
    });
    expect(within(livres).getAllByRole("button")).toHaveLength(2);
    expect(livres).toHaveTextContent("Sem horários livres.");
    expect(livres).toHaveTextContent("30 minutos");
  });

  it("cria a marcação no horário escolhido e abre o detalhe no estado Pendente", async () => {
    const { servico } = montar(novaComPaciente);
    const u = userEvent.setup();
    expect(screen.getByText("Bobi")).toBeInTheDocument();
    await escolherVeterinarioEHorario(u);
    expect(screen.getByText(/às 10:30/, { selector: "strong" })).toBeVisible();
    await u.type(screen.getByLabelText("Motivo (opcional)"), "Vacina");
    await u.click(screen.getByRole("button", { name: "Criar marcação" }));

    expect(servico.criar).toHaveBeenCalledWith({
      animalId: 3,
      veterinarioId: 5,
      dataHora: "2026-11-20T10:30:00.000Z",
      motivo: "Vacina",
    });
    expect(
      await screen.findByText("Marcação criada no estado Pendente."),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("heading", { name: "Detalhe da marcação" }),
    ).toBeInTheDocument();
  });

  it("não deixa criar sem paciente, Veterinário e horário livre", async () => {
    const { servico } = montar("/agenda/nova");
    await userEvent
      .setup()
      .click(await screen.findByRole("button", { name: "Criar marcação" }));
    expect(screen.getByText("Escolha o paciente.")).toBeInTheDocument();
    expect(screen.getByText("Escolha o Veterinário.")).toBeInTheDocument();
    expect(
      screen.getByText(
        "Consulte a disponibilidade e escolha um dos horários livres.",
      ),
    ).toBeInTheDocument();
    expect(servico.criar).not.toHaveBeenCalled();
  });

  it("mudar de Veterinário obriga a escolher de novo o horário (critério 3)", async () => {
    montar(novaComPaciente);
    const u = userEvent.setup();
    await escolherVeterinarioEHorario(u);
    await u.selectOptions(screen.getByLabelText("Veterinário"), "6");
    expect(screen.queryByRole("group", { name: "Horários livres" })).toBeNull();
    expect(screen.getByText("nenhum")).toBeInTheDocument();
  });

  it("se a vaga deixou de estar livre, mostra a resposta e atualiza a disponibilidade (critério 5)", async () => {
    const servico = servicoFalso();
    servico.criar.mockRejectedValue(
      new ApiRequestError(
        422,
        "regraNegocio",
        "O veterinário não está disponível nesse horário.",
      ),
    );
    montar(novaComPaciente, { servico });
    const u = userEvent.setup();
    await escolherVeterinarioEHorario(u);
    await u.click(screen.getByRole("button", { name: "Criar marcação" }));

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "O veterinário não está disponível nesse horário.",
    );
    await waitFor(() =>
      expect(servico.obterDisponibilidade).toHaveBeenCalledTimes(2),
    );
    expect(
      await screen.findByText(
        "A disponibilidade foi atualizada. Escolha outro horário livre.",
      ),
    ).toBeInTheDocument();
    expect(screen.getByText("nenhum")).toBeInTheDocument();
  });

  it("pesquisa o paciente quando não vem escolhido", async () => {
    const pacientes = {
      listar: vi.fn().mockResolvedValue({
        itens: [
          {
            id: 3,
            nome: "Bobi",
            especie: "Cão",
            raca: null,
            idadeAnos: 4,
            peso: 28.4,
            cliente: { id: 7, nome: "João Costa" },
          },
        ],
        pagina: 1,
        tamanho: 10,
        total: 1,
      }),
    };
    montar("/agenda/nova", { pacientes });
    const u = userEvent.setup();
    await u.type(
      screen.getByLabelText("Paciente (nome do animal ou do proprietário)"),
      "bobi{Enter}",
    );
    await u.click(
      await screen.findByRole("button", {
        name: "Selecionar Bobi (João Costa)",
      }),
    );
    expect(pacientes.listar).toHaveBeenCalledWith({
      pesquisa: "bobi",
      pagina: 1,
      tamanho: 10,
    });
    expect(screen.getByText("Bobi")).toBeInTheDocument();
  });
});

describe("W10 — editar marcação", () => {
  it("mantém o horário atual quando só muda o motivo", async () => {
    const { servico } = montar("/agenda/41/editar");
    const veterinario = await screen.findByLabelText("Veterinário");
    await waitFor(() => expect(veterinario).toHaveValue("5"));
    expect(screen.getByText("(horário atual)", { exact: false })).toBeVisible();
    const u = userEvent.setup();
    const motivo = screen.getByLabelText("Motivo (opcional)");
    await u.clear(motivo);
    await u.type(motivo, "Revisão");
    await u.click(screen.getByRole("button", { name: "Guardar alterações" }));
    expect(servico.atualizar).toHaveBeenCalledWith(41, {
      veterinarioId: 5,
      dataHora: "2026-11-20T10:00:00.000Z",
      motivo: "Revisão",
    });
  });

  it("um 412 volta a ler a marcação e avisa que mudou entretanto", async () => {
    const servico = servicoFalso();
    servico.atualizar.mockRejectedValue(
      new ApiRequestError(412, "versaoDesatualizada"),
    );
    montar("/agenda/41/editar", { servico });
    await screen.findByLabelText("Veterinário");
    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Guardar alterações" }));
    expect(
      await screen.findByText(/foi alterada por outra pessoa/),
    ).toBeInTheDocument();
    expect(servico.obterConsulta).toHaveBeenCalledTimes(2);
  });

  it("uma marcação Cancelada já não pode ser editada", async () => {
    montar("/agenda/41/editar", { servico: servicoFalso("Cancelada") });
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Esta marcação está Cancelada e já não pode ser alterada.",
    );
    expect(
      screen.queryByRole("button", { name: "Guardar alterações" }),
    ).toBeNull();
  });
});

describe("W11 — detalhe e estado da marcação", () => {
  it("Pendente: só oferece Confirmar e Cancelar e pede confirmação", async () => {
    const servico = servicoFalso("Pendente");
    servico.obterConsulta
      .mockResolvedValueOnce(marcacaoEm("Pendente"))
      .mockResolvedValueOnce(marcacaoEm("Confirmada"));
    montar("/agenda/41", { servico });
    const dados = await screen.findByLabelText("Dados da marcação");
    expect(dados).toHaveTextContent("Bobi · Cão");
    expect(dados).toHaveTextContent("João Costa");
    expect(dados).toHaveTextContent("20/11/2026, 10:00");
    expect(
      screen.queryByRole("button", { name: "Marcar como realizada" }),
    ).toBeNull();

    const u = userEvent.setup();
    await u.click(screen.getByRole("button", { name: "Confirmar marcação" }));
    expect(servico.alterarEstado).not.toHaveBeenCalled();
    const confirmacao = screen.getByRole("group", { name: "Confirmação" });
    expect(confirmacao).toHaveTextContent("de Pendente para Confirmada");
    await u.click(
      within(confirmacao).getByRole("button", { name: "Confirmar" }),
    );

    expect(servico.alterarEstado).toHaveBeenCalledWith(41, {
      estado: "Confirmada",
      observacao: "",
    });
    expect(
      await screen.findByText("Estado alterado para Confirmada."),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: "Marcar como realizada" }),
    ).toBeInTheDocument();
    expect(
      screen.queryByRole("button", { name: "Confirmar marcação" }),
    ).toBeNull();
  });

  it("cancela com o motivo indicado", async () => {
    const servico = servicoFalso("Confirmada");
    montar("/agenda/41", { servico });
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Cancelar marcação" }),
    );
    await u.type(
      screen.getByLabelText("Motivo do cancelamento (opcional)"),
      "Pedido do tutor.",
    );
    await u.click(screen.getByRole("button", { name: "Confirmar" }));
    expect(servico.alterarEstado).toHaveBeenCalledWith(41, {
      estado: "Cancelada",
      observacao: "Pedido do tutor.",
    });
  });

  it.each(["Realizada", "Cancelada"] as const)(
    "%s é final: sem transições nem edição",
    async (estado) => {
      montar("/agenda/41", { servico: servicoFalso(estado) });
      expect(await screen.findByText(/é um estado final/)).toBeInTheDocument();
      expect(screen.queryByRole("button")).toBeNull();
      expect(
        screen.queryByRole("link", { name: "Editar marcação" }),
      ).toBeNull();
    },
  );

  it("um 412 ao mudar o estado mostra os dados atuais e o aviso", async () => {
    const servico = servicoFalso("Pendente");
    servico.alterarEstado.mockRejectedValue(
      new ApiRequestError(412, "versaoDesatualizada"),
    );
    servico.obterConsulta
      .mockResolvedValueOnce(marcacaoEm("Pendente"))
      .mockResolvedValueOnce(marcacaoEm("Cancelada"));
    montar("/agenda/41", { servico });
    const u = userEvent.setup();
    await u.click(
      await screen.findByRole("button", { name: "Confirmar marcação" }),
    );
    await u.click(screen.getByRole("button", { name: "Confirmar" }));
    expect(
      await screen.findByText(/foi alterada por outra pessoa/),
    ).toBeInTheDocument();
    expect(screen.getByLabelText("Dados da marcação")).toHaveTextContent(
      "Cancelada",
    );
    expect(screen.getByRole("alert")).toHaveTextContent(
      "Este registo foi alterado entretanto",
    );
  });

  it("o Veterinário vê a marcação sem ações e com a ligação para a ficha", async () => {
    montar("/agenda/41", { perfil: "Veterinario" });
    expect(
      await screen.findByRole("link", { name: "Abrir ficha do paciente" }),
    ).toHaveAttribute("href", "/pacientes/3");
    expect(screen.queryByRole("heading", { name: "Ações" })).toBeNull();
    expect(screen.queryByRole("button")).toBeNull();
  });
});

import {
  fireEvent,
  render,
  screen,
  waitFor,
  within,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { ApiRequestError } from "../../core/api";
import type { ClinicaConfig } from "../../models/clinica";
import type { ServicoClinica } from "../../services/ClinicaService";
import { ClinicaConfiguracaoPage } from "./ClinicaConfiguracaoPage";

const clinica: ClinicaConfig = {
  nome: "Clínica Veterinária G08",
  nif: "501234567",
  morada: "Rua do Exemplo, 100, 4470-000 Maia",
  telefone: "229000000",
  email: "geral@clinica-g08.pt",
  duracaoConsultaMinutos: 30,
  horarioFuncionamento: [
    { diaSemana: "Segunda", abertura: "09:00", fecho: "13:00" },
    { diaSemana: "Segunda", abertura: "14:00", fecho: "19:00" },
    { diaSemana: "Terca", abertura: "09:00", fecho: "19:00" },
    { diaSemana: "Sabado", abertura: "09:00", fecho: "13:00" },
  ],
};

function servicoFalso(): {
  [K in keyof ServicoClinica]: ReturnType<typeof vi.fn>;
} {
  return {
    obter: vi.fn().mockResolvedValue(clinica),
    atualizar: vi
      .fn()
      .mockImplementation((dados: ClinicaConfig) => Promise.resolve(dados)),
  };
}

async function montar(servico = servicoFalso()) {
  render(
    <ClinicaConfiguracaoPage servico={servico as unknown as ServicoClinica} />,
  );
  await screen.findByLabelText("Nome da clínica");
  return servico;
}

function hora(rotulo: string, valor: string) {
  fireEvent.change(screen.getByLabelText(rotulo), { target: { value: valor } });
}

function guardar() {
  return userEvent
    .setup()
    .click(screen.getByRole("button", { name: "Guardar" }));
}

describe("W06 — clínica e horários: carregar", () => {
  it("mostra o estado de carregamento e depois os dados e o horário", async () => {
    const servico = servicoFalso();
    render(
      <ClinicaConfiguracaoPage
        servico={servico as unknown as ServicoClinica}
      />,
    );
    expect(screen.getByRole("status")).toHaveTextContent("A carregar");

    expect(await screen.findByLabelText("Nome da clínica")).toHaveValue(
      "Clínica Veterinária G08",
    );
    expect(screen.getByLabelText("NIF")).toHaveValue("501234567");
    expect(screen.getByLabelText("Morada")).toHaveValue(
      "Rua do Exemplo, 100, 4470-000 Maia",
    );
    expect(screen.getByLabelText("Telefone")).toHaveValue("229000000");
    expect(screen.getByLabelText("E-mail")).toHaveValue("geral@clinica-g08.pt");
    expect(screen.getByLabelText("Duração da consulta (minutos)")).toHaveValue(
      30,
    );
    expect(servico.obter).toHaveBeenCalledTimes(1);

    expect(screen.getByLabelText("Segunda, período 1 — abertura")).toHaveValue(
      "09:00",
    );
    expect(screen.getByLabelText("Segunda, período 1 — fecho")).toHaveValue(
      "13:00",
    );
    expect(screen.getByLabelText("Segunda, período 2 — abertura")).toHaveValue(
      "14:00",
    );
    expect(screen.getByLabelText("Segunda, período 2 — fecho")).toHaveValue(
      "19:00",
    );
    expect(screen.getByLabelText("Sábado, período 1 — fecho")).toHaveValue(
      "13:00",
    );
  });

  it("os dias sem períodos aparecem como encerrados", async () => {
    await montar();
    const domingo = screen.getByRole("heading", {
      name: "Domingo",
    }).parentElement!;
    expect(within(domingo).getByText("Encerrado")).toBeInTheDocument();
    const quarta = screen.getByRole("heading", {
      name: "Quarta",
    }).parentElement!;
    expect(within(quarta).getByText("Encerrado")).toBeInTheDocument();
    const segunda = screen.getByRole("heading", {
      name: "Segunda",
    }).parentElement!;
    expect(within(segunda).queryByText("Encerrado")).not.toBeInTheDocument();
  });

  it("se o carregamento falha mostra o erro e permite tentar de novo", async () => {
    const servico = servicoFalso();
    servico.obter
      .mockRejectedValueOnce(new ApiRequestError(0, "rede"))
      .mockResolvedValueOnce(clinica);
    render(
      <ClinicaConfiguracaoPage
        servico={servico as unknown as ServicoClinica}
      />,
    );

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor",
    );
    expect(screen.queryByLabelText("Nome da clínica")).not.toBeInTheDocument();

    await userEvent
      .setup()
      .click(screen.getByRole("button", { name: "Tentar novamente" }));

    expect(await screen.findByLabelText("Nome da clínica")).toHaveValue(
      "Clínica Veterinária G08",
    );
    expect(servico.obter).toHaveBeenCalledTimes(2);
  });

  it("um 403 ao carregar mostra a mensagem de falta de permissão", async () => {
    const servico = servicoFalso();
    servico.obter.mockRejectedValue(new ApiRequestError(403, "semPermissao"));
    render(
      <ClinicaConfiguracaoPage
        servico={servico as unknown as ServicoClinica}
      />,
    );
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não tem permissão",
    );
  });
});

describe("W06 — clínica e horários: guardar", () => {
  it("envia os dados aparados e o horário completo, e mostra o que a API devolveu", async () => {
    const servico = await montar();
    const u = userEvent.setup();
    const nome = screen.getByLabelText("Nome da clínica");
    await u.clear(nome);
    await u.type(nome, "  Clínica G08 Maia  ");
    const duracao = screen.getByLabelText("Duração da consulta (minutos)");
    await u.clear(duracao);
    await u.type(duracao, "45");

    // Quarta passa a abrir num período novo e a Segunda perde o 2.º período.
    await u.click(
      screen.getByRole("button", { name: "Adicionar período a Quarta" }),
    );
    hora("Quarta, período 1 — abertura", "10:00");
    hora("Quarta, período 1 — fecho", "12:30");
    await u.click(
      screen.getByRole("button", { name: "Remover Segunda, período 2" }),
    );

    // A API devolve o horário já ordenado e com a hora normalizada.
    servico.atualizar.mockImplementation((dados: ClinicaConfig) =>
      Promise.resolve({ ...dados, nome: "Clínica G08 Maia (gravado)" }),
    );
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    await waitFor(() => expect(servico.atualizar).toHaveBeenCalledTimes(1));
    expect(servico.atualizar).toHaveBeenCalledWith({
      nome: "Clínica G08 Maia",
      nif: "501234567",
      morada: "Rua do Exemplo, 100, 4470-000 Maia",
      telefone: "229000000",
      email: "geral@clinica-g08.pt",
      duracaoConsultaMinutos: 45,
      horarioFuncionamento: [
        { diaSemana: "Segunda", abertura: "09:00", fecho: "13:00" },
        { diaSemana: "Terca", abertura: "09:00", fecho: "19:00" },
        { diaSemana: "Quarta", abertura: "10:00", fecho: "12:30" },
        { diaSemana: "Sabado", abertura: "09:00", fecho: "13:00" },
      ],
    });

    // Critério 4: depois de gravar, o ecrã mostra o que ficou persistido.
    expect(await screen.findByRole("status")).toHaveTextContent(
      "Alterações guardadas.",
    );
    expect(screen.getByLabelText("Nome da clínica")).toHaveValue(
      "Clínica G08 Maia (gravado)",
    );
    expect(screen.getByLabelText("Quarta, período 1 — abertura")).toHaveValue(
      "10:00",
    );
    expect(screen.getByLabelText("Quarta, período 1 — fecho")).toHaveValue(
      "12:30",
    );
    expect(
      screen.queryByLabelText("Segunda, período 2 — abertura"),
    ).not.toBeInTheDocument();
  });

  it("aceita períodos seguidos (13:00–14:00 e 14:00–19:00)", async () => {
    const servico = await montar();
    const u = userEvent.setup();
    await u.click(
      screen.getByRole("button", { name: "Adicionar período a Domingo" }),
    );
    await u.click(
      screen.getByRole("button", { name: "Adicionar período a Domingo" }),
    );
    hora("Domingo, período 1 — abertura", "13:00");
    hora("Domingo, período 1 — fecho", "14:00");
    hora("Domingo, período 2 — abertura", "14:00");
    hora("Domingo, período 2 — fecho", "19:00");
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    await waitFor(() => expect(servico.atualizar).toHaveBeenCalledTimes(1));
    expect(screen.queryByRole("alert")).not.toBeInTheDocument();
  });

  it("remover todos os períodos de um dia deixa-o encerrado e envia o horário sem ele", async () => {
    const servico = await montar();
    const u = userEvent.setup();
    await u.click(
      screen.getByRole("button", { name: "Remover Sábado, período 1" }),
    );
    const sabado = screen.getByRole("heading", {
      name: "Sábado",
    }).parentElement!;
    expect(within(sabado).getByText("Encerrado")).toBeInTheDocument();

    await u.click(screen.getByRole("button", { name: "Guardar" }));
    await waitFor(() => expect(servico.atualizar).toHaveBeenCalledTimes(1));
    const enviado = servico.atualizar.mock.calls[0][0] as ClinicaConfig;
    expect(
      enviado.horarioFuncionamento.some((p) => p.diaSemana === "Sabado"),
    ).toBe(false);
  });

  it("critério 2: períodos sobrepostos no mesmo dia são rejeitados antes de ir à API", async () => {
    const servico = await montar();
    hora("Segunda, período 2 — abertura", "12:00");
    await guardar();

    const alerta = await screen.findByRole("alert");
    expect(alerta).toHaveTextContent(
      "Segunda: os períodos 09:00–13:00 e 12:00–19:00 sobrepõem-se.",
    );
    expect(servico.atualizar).not.toHaveBeenCalled();
    // O que o utilizador escreveu não se perde.
    expect(screen.getByLabelText("Segunda, período 2 — abertura")).toHaveValue(
      "12:00",
    );
  });

  it.each([
    ["igual à", "09:00"],
    ["anterior à", "08:00"],
  ])(
    "critério 2: fecho %s abertura é rejeitado antes de ir à API",
    async (_descricao, fecho) => {
      const servico = await montar();
      hora("Terça, período 1 — fecho", fecho);
      await guardar();

      expect(await screen.findByRole("alert")).toHaveTextContent(
        "Terça, período 1: o fecho tem de ser posterior à abertura.",
      );
      expect(servico.atualizar).not.toHaveBeenCalled();
    },
  );

  it("um período novo por preencher é rejeitado e indica qual", async () => {
    const servico = await montar();
    await userEvent
      .setup()
      .click(
        screen.getByRole("button", { name: "Adicionar período a Quinta" }),
      );
    await guardar();

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Quinta, período 1: indique a hora de abertura e a de fecho.",
    );
    expect(servico.atualizar).not.toHaveBeenCalled();
  });

  it("rejeita campos obrigatórios vazios e duração fora de 5–240", async () => {
    const servico = await montar();
    const u = userEvent.setup();
    await u.clear(screen.getByLabelText("Nome da clínica"));
    await u.clear(screen.getByLabelText("NIF"));
    await u.clear(screen.getByLabelText("Duração da consulta (minutos)"));
    await u.type(screen.getByLabelText("Duração da consulta (minutos)"), "3");
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    const alerta = await screen.findByRole("alert");
    expect(alerta).toHaveTextContent("Indique o nome da clínica.");
    expect(alerta).toHaveTextContent("Indique o NIF.");
    expect(alerta).toHaveTextContent("entre 5 e 240");
    expect(servico.atualizar).not.toHaveBeenCalled();
  });

  it("mostra o erro 400 da API e mantém os dados no formulário", async () => {
    const servico = servicoFalso();
    servico.atualizar.mockRejectedValue(
      new ApiRequestError(400, "validacao", "O NIF é inválido.", {
        code: 400,
        message: "Pedido inválido.",
        detail: "O NIF é inválido.",
      }),
    );
    await montar(servico);
    const u = userEvent.setup();
    const nif = screen.getByLabelText("NIF");
    await u.clear(nif);
    await u.type(nif, "123456789");
    await u.click(screen.getByRole("button", { name: "Guardar" }));

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "O NIF é inválido.",
    );
    expect(screen.getByLabelText("NIF")).toHaveValue("123456789");
    expect(screen.queryByText("Alterações guardadas.")).not.toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Guardar" })).toBeEnabled();
  });

  it("falha de ligação ao guardar mostra a mensagem e deixa tentar de novo", async () => {
    const servico = servicoFalso();
    servico.atualizar
      .mockRejectedValueOnce(new ApiRequestError(0, "rede"))
      .mockImplementation((dados: ClinicaConfig) => Promise.resolve(dados));
    await montar(servico);
    const u = userEvent.setup();
    await u.click(screen.getByRole("button", { name: "Guardar" }));
    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Não foi possível contactar o servidor",
    );

    await u.click(screen.getByRole("button", { name: "Guardar" }));
    expect(await screen.findByRole("status")).toHaveTextContent(
      "Alterações guardadas.",
    );
    expect(screen.queryByRole("alert")).not.toBeInTheDocument();
  });

  it("enquanto guarda desativa os botões", async () => {
    const servico = servicoFalso();
    let concluir: (c: ClinicaConfig) => void = () => undefined;
    servico.atualizar.mockReturnValue(
      new Promise<ClinicaConfig>((resolver) => {
        concluir = resolver;
      }),
    );
    await montar(servico);
    await guardar();

    const botao = await screen.findByRole("button", { name: "A guardar…" });
    expect(botao).toBeDisabled();
    expect(screen.getByRole("button", { name: "Cancelar" })).toBeDisabled();

    concluir(clinica);
    expect(await screen.findByRole("status")).toHaveTextContent(
      "Alterações guardadas.",
    );
  });

  it("Cancelar repõe o que está gravado e limpa as mensagens", async () => {
    const servico = await montar();
    const u = userEvent.setup();
    await u.clear(screen.getByLabelText("Morada"));
    await u.type(screen.getByLabelText("Morada"), "Outra rua");
    await u.click(
      screen.getByRole("button", { name: "Adicionar período a Domingo" }),
    );
    await u.click(screen.getByRole("button", { name: "Guardar" }));
    expect(await screen.findByRole("alert")).toBeInTheDocument();

    await u.click(screen.getByRole("button", { name: "Cancelar" }));

    expect(screen.getByLabelText("Morada")).toHaveValue(
      "Rua do Exemplo, 100, 4470-000 Maia",
    );
    expect(
      screen.queryByLabelText("Domingo, período 1 — abertura"),
    ).not.toBeInTheDocument();
    expect(screen.queryByRole("alert")).not.toBeInTheDocument();
    expect(servico.atualizar).not.toHaveBeenCalled();
  });
});

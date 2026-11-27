import { expect, test } from "@playwright/test";
import { CONTAS } from "./apoio/contas";
import {
  dataLisboa,
  dataPt,
  diaDeMarcacao,
  literal,
  sufixo,
} from "./apoio/dados";
import { pedidosA, registarPedidos, respostaDe } from "./apoio/pedidos";
import {
  confirmarMarcacaoAberta,
  criarMarcacaoDeTeste,
  prepararPaciente,
} from "./apoio/procedimentos";
import { entrarComo, terminarSessao } from "./apoio/sessao";
import { PaginaMarcacao } from "./paginas/PaginaMarcacao";
import { PaginaPreparacao } from "./paginas/PaginaPreparacao";

// I128, secção 9 — F003: preparação da consulta e informação não clínica de
// apoio do Auxiliar Veterinário (W11, W12). Automação: I130.

const ANA = CONTAS.vetAna;
const BRUNO = CONTAS.vetBruno;
const NOTA_NO_PEDIDO = /^\/consultas\/\d+\/notas-preparacao$/;

test("ST-WEB-F003-15 — Consultar a informação de preparação da consulta", async ({
  page,
}) => {
  const suf = sufixo("F003-15");
  const dia = diaDeMarcacao(15);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const marcacao = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await confirmarMarcacaoAberta(page);
  await terminarSessao(page);

  await test.step("1. PC-01 com aux.carla@: «Preparação de consultas»", async () => {
    await entrarComo.auxiliar(page);
    await expect(page).toHaveURL(/\/preparacao$/);
  });

  const preparacao = new PaginaPreparacao(page);
  await test.step("2. selecionar a consulta de teste em {D}", async () => {
    await preparacao.irParaDia(dia);
    await preparacao.selecionarConsulta(paciente.nome, marcacao.hora);
  });

  await test.step("informação de preparação, sem informação clínica", async () => {
    await expect(preparacao.selecionada).toBeVisible();
    await expect(preparacao.campo("Consulta")).toHaveText(
      `${dataPt(dia)}, ${marcacao.hora} · 30 minutos · Confirmada`,
    );
    await expect(preparacao.campo("Veterinário")).toHaveText(ANA.nome);
    await expect(preparacao.campo("Motivo")).toHaveText(marcacao.motivo);
    await expect(preparacao.campo("Chegada")).toHaveText("Não registada");
    await expect(preparacao.campo("Paciente")).toHaveText(
      `${paciente.nome} · Cão (Labrador Retriever) · Macho`,
    );
    await expect(preparacao.campo("Idade e peso")).toHaveText(
      /^\d+ anos? · 10,5 kg · nascido a 01\/01\/2023$/,
    );
    await expect(preparacao.campo("Proprietário")).toHaveText(
      `${cliente.nome} · ${cliente.telefone}`,
    );

    // Nenhuma informação clínica nem ações de agenda.
    await expect(preparacao.selecionada).not.toContainText(
      /Diagnóstico|Notas clínicas|Tratamento|Prescriç/,
    );
    for (const nome of [
      "Registar chegada",
      "Confirmar marcação",
      "Cancelar marcação",
      "Marcar como realizada",
    ]) {
      await expect(page.getByRole("button", { name: nome })).toHaveCount(0);
    }
    await expect(
      page.getByRole("link", { name: "Editar marcação" }),
    ).toHaveCount(0);
  });
});

test("ST-WEB-F003-16 — Registar informação não clínica de apoio com autorização", async ({
  page,
}) => {
  test.slow();
  const suf = sufixo("F003-16");
  const dia = diaDeMarcacao(16);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const marcacao = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: BRUNO,
    data: dia,
  });
  await confirmarMarcacaoAberta(page);
  await terminarSessao(page);
  const preparacao = new PaginaPreparacao(page);
  const texto = `Sala preparada ${suf}`;

  await test.step("1. PC-01 com aux.carla@ e selecionar a consulta", async () => {
    await entrarComo.auxiliar(page);
    await preparacao.irParaDia(dia);
    await preparacao.selecionarConsulta(paciente.nome, marcacao.hora);
    await expect(preparacao.semNotas).toBeVisible();
  });

  await test.step("2. guardar sem texto", async () => {
    const pedidos = registarPedidos(page);
    await preparacao.guardarNota.click();
    await expect(preparacao.nota).toHaveAccessibleDescription(
      "Escreva a informação de apoio.",
    );
    expect(pedidosA(pedidos, "POST", NOTA_NO_PEDIDO)).toHaveLength(0);
  });

  await test.step("3. guardar a nota", async () => {
    await preparacao.nota.fill(texto);
    await preparacao.guardarNota.click();
    await expect(preparacao.aviso).toHaveText("Informação de apoio guardada.");
    await expect(preparacao.notas.getByRole("listitem")).toHaveText([
      new RegExp(
        `^${literal(texto)} — ${literal(CONTAS.auxCarla.nome)}, ${literal(dataPt(dataLisboa(0)))}, \\d{2}:\\d{2}$`,
      ),
    ]);
    await expect(preparacao.nota).toHaveValue("");
  });

  await test.step("4. a Rececionista vê a nota no detalhe", async () => {
    await terminarSessao(page);
    await entrarComo.rececionista(page);
    const detalhe = new PaginaMarcacao(page);
    await detalhe.abrir(marcacao.id);
    await expect(detalhe.notasPreparacao).toContainText(
      `${texto} — ${CONTAS.auxCarla.nome}`,
    );
    // A nota não altera a marcação.
    await expect(detalhe.campo("Estado")).toHaveText("Confirmada");
    await expect(detalhe.campo("Motivo")).toHaveText(marcacao.motivo);
  });
});

test("ST-WEB-F003-17 — Recusar a nota de apoio sem autorização ou numa marcação cancelada", async ({
  page,
}) => {
  test.slow();
  const suf = sufixo("F003-17");
  const dia = diaDeMarcacao(17);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const confirmada = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await confirmarMarcacaoAberta(page);
  const cancelada = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await new PaginaMarcacao(page).mudarEstado("Cancelar marcação", "Cancelada");
  await terminarSessao(page);
  const preparacao = new PaginaPreparacao(page);
  const texto = `Nota ${suf}`;

  await test.step("1. aux.diogo@ (sem autorização): 403", async () => {
    await entrarComo.auxiliarSemApoio(page);
    await preparacao.irParaDia(dia);
    await preparacao.selecionarConsulta(paciente.nome, confirmada.hora);
    await preparacao.nota.fill(texto);
    const resposta = respostaDe(page, "POST", NOTA_NO_PEDIDO);
    await preparacao.guardarNota.click();
    expect((await resposta).status()).toBe(403);
    await expect(preparacao.erroNota).toContainText(
      "O Auxiliar Veterinário autenticado não tem autorização para registar informação de apoio.",
    );
    await expect(preparacao.semAutorizacao).toBeVisible();
    await expect(preparacao.formularioNota).toHaveCount(0);
    await expect(preparacao.guardarNota).toHaveCount(0);
  });

  await test.step("2. aux.carla@ numa marcação Cancelada: 422", async () => {
    await terminarSessao(page);
    await entrarComo.auxiliar(page);
    await preparacao.irParaDia(dia);
    await preparacao.selecionarConsulta(paciente.nome, cancelada.hora);
    await expect(preparacao.campo("Consulta")).toContainText("Cancelada");
    await preparacao.nota.fill(texto);
    const resposta = respostaDe(page, "POST", NOTA_NO_PEDIDO);
    await preparacao.guardarNota.click();
    expect((await resposta).status()).toBe(422);
    await expect(preparacao.formularioNota.getByRole("alert")).toContainText(
      "Não é possível registar notas de preparação numa marcação Cancelada.",
    );
    // O texto escrito não se perde.
    await expect(preparacao.nota).toHaveValue(texto);
    await expect(preparacao.formularioNota).toBeVisible();
  });

  await test.step("3. a primeira marcação continua sem a nota", async () => {
    await preparacao.selecionarConsulta(paciente.nome, confirmada.hora);
    await expect(preparacao.campo("Consulta")).toContainText("Confirmada");
    await expect(preparacao.semNotas).toBeVisible();
    await expect(preparacao.selecionada).not.toContainText(texto);
  });
});

import { expect, test, type Page } from "@playwright/test";
import { URL_API } from "./apoio/ambiente";
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
  idDoPaciente,
  prepararPaciente,
  type MarcacaoDeTeste,
} from "./apoio/procedimentos";
import { idDoBobi } from "./apoio/respostas";
import { entrarComo, iniciarSessao, terminarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaAgenda } from "./paginas/PaginaAgenda";
import { PaginaHistoricoClinico } from "./paginas/PaginaHistoricoClinico";
import { PaginaMarcacao } from "./paginas/PaginaMarcacao";
import { PaginaPaciente } from "./paginas/PaginaPaciente";
import { PaginaPacientes } from "./paginas/PaginaPacientes";
import { PaginaRegistoClinico } from "./paginas/PaginaRegistoClinico";

// I128, secção 10 — F004: registo clínico e histórico clínico do paciente
// (W08, W13). Automação: I130. Rótulos da implementação (I115, I116),
// como prevê a secção 6 da I128.

const ANA = CONTAS.vetAna;

/** Registo clínico da consulta do Bobi de 2026-10-14 (I128, secção 5.3). */
const BOBI = {
  data: "14/10/2026",
  notas:
    "Canal auditivo esquerdo eritematoso, com cerúmen escuro. Sem alterações no ouvido direito.",
  diagnostico: "Otite externa",
  tratamento:
    "Limpeza auricular com solução fisiológica — aplicado em 14/10/2026",
  prescricao: "Gotas auriculares (fictício)",
};

/** Mensagem local de W13 (validacaoRegistoClinico.ts), igual à da API. */
const SEM_ELEMENTO_CLINICO =
  "O registo clínico tem de ter pelo menos um elemento clínico: notas clínicas, diagnóstico, um tratamento ou uma prescrição.";

/** Mensagem do período do histórico (periodoHistorico.ts), igual à da API. */
const FIM_ANTERIOR_AO_INICIO =
  "A data de fim (dataFim) não pode ser anterior à data de início (dataInicio).";

const HISTORICO = /^\/animais\/\d+\/historico-clinico$/;
const REGISTOS = /^\/registos-clinicos(\/\d+)?$/;

interface DadosRegisto {
  notas: string;
  diagnostico: string;
  tratamento: string;
  medicamento: string;
  dosagem: string;
  frequencia: string;
  duracao: string;
}

/** Os quatro elementos clínicos de ST-WEB-F004-01. */
function dadosDoRegisto(suf: string): DadosRegisto {
  return {
    notas: `Notas ${suf}`,
    diagnostico: `Diagnóstico ${suf}`,
    tratamento: `Tratamento ${suf}`,
    medicamento: `Medicamento ${suf}`,
    dosagem: "1 comprimido",
    frequencia: "2 vezes por dia",
    duracao: "5",
  };
}

/**
 * Pré-condição comum: paciente de teste e marcação com a Dra. Ana em {D},
 * confirmada (PC-03 a PC-06). Termina a sessão da Rececionista (PC-02).
 */
async function prepararConsultaConfirmada(
  page: Page,
  suf: string,
  desvio: number,
) {
  const dia = diaDeMarcacao(desvio);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const marcacao = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await confirmarMarcacaoAberta(page);
  return { dia, cliente, paciente, marcacao };
}

/** Abre o registo clínico (W13) a partir do detalhe da marcação. */
async function abrirRegistoDaConsulta(
  page: Page,
  marcacao: MarcacaoDeTeste,
): Promise<PaginaRegistoClinico> {
  const detalhe = new PaginaMarcacao(page);
  await detalhe.abrir(marcacao.id);
  await detalhe.abrirRegistoClinico.click();
  const registo = new PaginaRegistoClinico(page);
  await registo.esperar();
  return registo;
}

/** Preenche notas, diagnóstico, um tratamento ({hoje}) e uma prescrição. */
async function preencherRegisto(
  registo: PaginaRegistoClinico,
  dados: DadosRegisto,
): Promise<void> {
  await registo.notas.fill(dados.notas);
  await registo.diagnostico.fill(dados.diagnostico);
  await registo.adicionarTratamento.click();
  await registo.descricaoTratamento(1).fill(dados.tratamento);
  await registo.dataTratamento(1).fill(dataLisboa(0));
  await registo.adicionarPrescricao.click();
  await registo.medicamento(1).fill(dados.medicamento);
  await registo.dosagem(1).fill(dados.dosagem);
  await registo.frequencia(1).fill(dados.frequencia);
  await registo.duracao(1).fill(dados.duracao);
}

/** «Registado por <Veterinário> em dd/mm/aaaa, HH:MM» (hoje). */
function autoriaDeHoje(veterinario: string): RegExp {
  return new RegExp(
    `^Registado por ${literal(veterinario)} em ${literal(dataPt(dataLisboa(0)))}, \\d{2}:\\d{2}`,
  );
}

test("ST-WEB-F004-01 — Registar notas clínicas, diagnóstico, tratamento e prescrição numa consulta", async ({
  page,
}) => {
  test.slow();
  const suf = sufixo("F004-01");
  const { dia, paciente, marcacao } = await prepararConsultaConfirmada(
    page,
    suf,
    18,
  );
  await terminarSessao(page);
  const dados = dadosDoRegisto(suf);
  const historico = new PaginaHistoricoClinico(page);
  const hoje = dataPt(dataLisboa(0));

  await test.step("1. ficha do paciente → «Registo clínico»", async () => {
    await entrarComo.veterinario(page);
    await new LayoutApp(page).irPara("Pacientes");
    const lista = new PaginaPacientes(page);
    await lista.esperarLista();
    await lista.pesquisar(paciente.nome);
    await lista.verFicha(paciente.nome).click();
    const ficha = new PaginaPaciente(page);
    await ficha.areaClinica
      .getByRole("link", { name: "Registo clínico", exact: true })
      .click();
    await historico.esperarLista();
    await expect(historico.vazia).toHaveText(
      "Este paciente ainda não tem registos clínicos.",
    );
    await expect(historico.criarNaAgenda).toBeVisible();
  });

  const registo = new PaginaRegistoClinico(page);
  await test.step("2. consulta de {D} (na Agenda) e os quatro elementos", async () => {
    await new LayoutApp(page).irPara("Agenda");
    const agenda = new PaginaAgenda(page);
    await agenda.irParaDia(dia);
    await agenda.verMarcacao(paciente.nome, marcacao.hora).click();
    await new PaginaMarcacao(page).abrirRegistoClinico.click();
    await registo.esperar();
    await expect(registo.semRegisto).toBeVisible();
    await preencherRegisto(registo, dados);
  });

  await test.step("3. gravar, recarregar e voltar a abrir", async () => {
    const resposta = respostaDe(page, "POST", /^\/registos-clinicos$/);
    await registo.criar.click();
    expect((await resposta).status()).toBe(201);
    await expect(registo.aviso).toHaveText(
      "Registo clínico criado para esta consulta.",
    );
    await expect(registo.autoria).toHaveText(autoriaDeHoje(ANA.nome));

    await page.reload();
    await registo.esperar();
    await expect(registo.autoria).toHaveText(autoriaDeHoje(ANA.nome));
    await expect(registo.campo("Paciente")).toHaveText(
      `${paciente.nome} · Cão`,
    );
    await expect(registo.campo("Data e hora")).toHaveText(
      `${dataPt(dia)}, ${marcacao.hora}`,
    );
    // O estado da marcação não muda.
    await expect(registo.campo("Estado")).toHaveText("Confirmada");
    await expect(registo.notas).toHaveValue(dados.notas);
    await expect(registo.diagnostico).toHaveValue(dados.diagnostico);
    await expect(registo.descricaoTratamento(1)).toHaveValue(dados.tratamento);
    await expect(registo.dataTratamento(1)).toHaveValue(dataLisboa(0));
    await expect(registo.medicamento(1)).toHaveValue(dados.medicamento);
    await expect(registo.dosagem(1)).toHaveValue(dados.dosagem);
    await expect(registo.frequencia(1)).toHaveValue(dados.frequencia);
    await expect(registo.duracao(1)).toHaveValue(dados.duracao);
    await expect(registo.guardarAlteracoes).toBeVisible();

    await historico.abrir(idDoPaciente(paciente));
    await expect(historico.linha(dataPt(dia)).locator("td")).toHaveText([
      dataPt(dia),
      ANA.nome,
      dados.diagnostico,
      "1",
      "1",
      "Ver registo",
    ]);
    await historico.verRegisto(dataPt(dia));
    await expect(historico.autoria).toHaveText(autoriaDeHoje(ANA.nome));
    await expect(historico.campo("Notas clínicas")).toHaveText(dados.notas);
    await expect(historico.campo("Diagnóstico")).toHaveText(dados.diagnostico);
    await expect(historico.tratamentos.getByRole("listitem")).toHaveText([
      `${dados.tratamento} — aplicado em ${hoje}`,
    ]);
    await expect(historico.prescricoes.getByRole("listitem")).toHaveText([
      `${dados.medicamento} — ${dados.dosagem}, ${dados.frequencia}, durante 5 dias`,
    ]);
  });
});

test("ST-WEB-F004-02 — Recusar registos clínicos inválidos", async ({
  page,
}) => {
  test.slow();
  const suf = sufixo("F004-02");
  const { dia, cliente, paciente, marcacao } = await prepararConsultaConfirmada(
    page,
    suf,
    19,
  );
  const pendente = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await terminarSessao(page);
  await entrarComo.veterinario(page);
  const pedidos = registarPedidos(page);
  const historico = new PaginaHistoricoClinico(page);

  await test.step("1. «Registo clínico» do paciente de teste", async () => {
    await historico.abrir(idDoPaciente(paciente));
    await expect(historico.vazia).toHaveText(
      "Este paciente ainda não tem registos clínicos.",
    );
  });

  const registo = await abrirRegistoDaConsulta(page, marcacao);

  await test.step("2a. registo sem nenhum elemento clínico", async () => {
    await registo.criar.click();
    await expect(registo.erros).toHaveText([SEM_ELEMENTO_CLINICO]);
    expect(pedidosA(pedidos, "POST", "/registos-clinicos")).toHaveLength(0);
  });

  await test.step("2b. prescrição sem dosagem", async () => {
    await registo.adicionarPrescricao.click();
    await registo.medicamento(1).fill(`Medicamento ${suf}`);
    await registo.frequencia(1).fill("2 vezes por dia");
    await registo.criar.click();
    await expect(registo.dosagem(1)).toHaveAccessibleDescription(
      "A dosagem da prescrição é obrigatória.",
    );
    await expect(registo.erros).toHaveCount(0);
    expect(pedidosA(pedidos, "POST", "/registos-clinicos")).toHaveLength(0);
    // O formulário não fica bloqueado.
    await expect(registo.criar).toBeEnabled();
    await expect(registo.dosagem(1)).toBeEditable();
  });

  await test.step("3. registo válido e segundo registo da mesma consulta", async () => {
    await registo.dosagem(1).fill("1 comprimido");
    const resposta = respostaDe(page, "POST", /^\/registos-clinicos$/);
    await registo.criar.click();
    expect((await resposta).status()).toBe(201);
    await expect(registo.aviso).toHaveText(
      "Registo clínico criado para esta consulta.",
    );
    // A aplicação não oferece um segundo registo da mesma consulta: o
    // registo aberto passa a ser alterado.
    await page.reload();
    await registo.esperar();
    await expect(registo.autoria).toHaveText(autoriaDeHoje(ANA.nome));
    await expect(registo.guardarAlteracoes).toBeVisible();
    await expect(registo.criar).toHaveCount(0);
  });

  await test.step("4. a consulta Pendente não recebe registo clínico", async () => {
    const detalhe = new PaginaMarcacao(page);
    await detalhe.abrir(pendente.id);
    await expect(detalhe.campo("Estado")).toHaveText("Pendente");
    await expect(detalhe.registoClinico).toContainText(
      "O registo clínico só está disponível em consultas Confirmada ou Realizada.",
    );
    await expect(detalhe.abrirRegistoClinico).toHaveCount(0);

    await page.goto(`/agenda/${pendente.id}/registo-clinico`);
    await registo.esperar();
    await expect(registo.estadoNaoAdmite).toHaveText(
      "Só é possível fazer o registo clínico de uma consulta Confirmada ou Realizada. Esta consulta está Pendente.",
    );
    await expect(registo.formulario).toHaveCount(0);
    // Só foi gravado o registo (c).
    expect(pedidosA(pedidos, "POST", "/registos-clinicos")).toHaveLength(1);
  });

  await test.step("o histórico tem um único registo", async () => {
    await historico.abrir(idDoPaciente(paciente));
    await expect(historico.linhas).toHaveCount(1);
    await expect(historico.linha(dataPt(dia))).toBeVisible();
  });
});

test("ST-WEB-F004-03 — Atualizar um registo clínico", async ({ page }) => {
  test.slow();
  const suf = sufixo("F004-03");
  const { dia, paciente, marcacao } = await prepararConsultaConfirmada(
    page,
    suf,
    20,
  );
  await terminarSessao(page);
  await entrarComo.veterinario(page);
  const dados = dadosDoRegisto(suf);
  const novoDiagnostico = `Diagnóstico alterado ${suf}`;
  const segundoTratamento = `Tratamento 2 ${suf}`;

  const registo = await abrirRegistoDaConsulta(page, marcacao);
  await preencherRegisto(registo, dados);
  await registo.criar.click();
  await expect(registo.aviso).toHaveText(
    "Registo clínico criado para esta consulta.",
  );

  await test.step("1. novo diagnóstico e segundo tratamento", async () => {
    await registo.diagnostico.fill(novoDiagnostico);
    await registo.adicionarTratamento.click();
    await registo.descricaoTratamento(2).fill(segundoTratamento);
    await registo.dataTratamento(2).fill(dataLisboa(0));
  });

  await test.step("2. gravar, recarregar e abrir o histórico", async () => {
    const resposta = respostaDe(page, "PUT", /^\/registos-clinicos\/\d+$/);
    await registo.guardarAlteracoes.click();
    expect((await resposta).status()).toBe(200);
    await expect(registo.aviso).toHaveText("Registo clínico atualizado.");
    const hoje = literal(dataPt(dataLisboa(0)));
    const autoria = new RegExp(
      `^Registado por ${literal(ANA.nome)} em ${hoje}, \\d{2}:\\d{2} · Última alteração: ${hoje}, \\d{2}:\\d{2} por ${literal(ANA.nome)}$`,
    );
    await expect(registo.autoria).toHaveText(autoria);

    await page.reload();
    await registo.esperar();
    await expect(registo.autoria).toHaveText(autoria);
    // Continua associado à mesma consulta e ao mesmo paciente.
    await expect(registo.campo("Paciente")).toHaveText(
      `${paciente.nome} · Cão`,
    );
    await expect(registo.campo("Data e hora")).toHaveText(
      `${dataPt(dia)}, ${marcacao.hora}`,
    );
    await expect(registo.diagnostico).toHaveValue(novoDiagnostico);
    await expect(registo.notas).toHaveValue(dados.notas);
    await expect(registo.descricaoTratamento(1)).toHaveValue(dados.tratamento);
    await expect(registo.descricaoTratamento(2)).toHaveValue(segundoTratamento);
    await expect(registo.medicamento(1)).toHaveValue(dados.medicamento);
    await expect(registo.dosagem(1)).toHaveValue(dados.dosagem);

    const historico = new PaginaHistoricoClinico(page);
    await historico.abrir(idDoPaciente(paciente));
    await expect(historico.linhas).toHaveCount(1);
    await expect(historico.linha(dataPt(dia)).locator("td")).toHaveText([
      dataPt(dia),
      ANA.nome,
      novoDiagnostico,
      "2",
      "1",
      "Ver registo",
    ]);
  });
});

test("ST-WEB-F004-04 — Não permitir alterar o registo clínico de outro Veterinário", async ({
  page,
  request,
}) => {
  const bobi = await idDoBobi(request);
  await entrarComo.outroVeterinario(page);
  const historico = new PaginaHistoricoClinico(page);
  const pedidos = registarPedidos(page);

  await test.step("1. o Dr. Bruno lê o registo de 2026-10-14", async () => {
    await page.goto(`/pacientes/${bobi}`);
    await new PaginaPaciente(page).areaClinica
      .getByRole("link", { name: "Registo clínico", exact: true })
      .click();
    await historico.esperarLista();
    await historico.verRegisto(BOBI.data);
    await expect(historico.campo("Notas clínicas")).toHaveText(BOBI.notas);
    await expect(historico.campo("Diagnóstico")).toHaveText(BOBI.diagnostico);
    await expect(historico.tratamentos).toContainText(BOBI.tratamento);
  });

  await test.step("2. tentar alterá-lo: 403", async () => {
    // A ligação «Alterar registo» aparece a qualquer Veterinário, mas o W13
    // da consulta da Dra. Ana é recusado pela API (403) e não abre o
    // formulário: nada pode ser gravado.
    const resposta = respostaDe(page, "GET", /^\/consultas\/\d+$/);
    await historico.alterarRegisto.click();
    expect((await resposta).status()).toBe(403);
    const registo = new PaginaRegistoClinico(page);
    await registo.esperar();
    await expect(page.locator("main").getByRole("alert")).toHaveText(
      "A marcação não está atribuída ao Veterinário autenticado.",
    );
    await expect(registo.formulario).toHaveCount(0);
    await expect(page.locator("main textarea")).toHaveCount(0);
    expect(pedidosA(pedidos, "PUT", REGISTOS)).toHaveLength(0);
    expect(pedidosA(pedidos, "POST", REGISTOS)).toHaveLength(0);
  });

  await test.step("o registo mantém «Otite externa»", async () => {
    await historico.abrir(bobi);
    await historico.verRegisto(BOBI.data);
    await expect(historico.campo("Diagnóstico")).toHaveText(BOBI.diagnostico);
  });
});

test("ST-WEB-F004-05 — Consultar o histórico clínico do paciente", async ({
  page,
  request,
}) => {
  const bobi = await idDoBobi(request);
  await entrarComo.veterinario(page);
  const historico = new PaginaHistoricoClinico(page);

  await test.step("1. ficha do Bobi → «Registo clínico»", async () => {
    await page.goto(`/pacientes/${bobi}`);
    await new PaginaPaciente(page).areaClinica
      .getByRole("link", { name: "Registo clínico", exact: true })
      .click();
    await historico.esperarLista();
    await expect(historico.linha(BOBI.data).locator("td")).toHaveText([
      BOBI.data,
      ANA.nome,
      BOBI.diagnostico,
      "1",
      "1",
      "Ver registo",
    ]);
    // Do mais recente para o mais antigo.
    const datas = (
      await historico.linhas.locator("td:first-child").allTextContents()
    ).map((d) => d.trim().split("/").reverse().join("-"));
    expect(datas).toEqual([...datas].sort().reverse());
  });

  await test.step("2. filtrar de 2026-10-01 a 2026-10-31", async () => {
    const resposta = page.waitForResponse(
      (r) =>
        r.url().startsWith(`${URL_API}/animais/${bobi}/historico-clinico?`) &&
        new URL(r.url()).searchParams.get("dataInicio") === "2026-10-01" &&
        new URL(r.url()).searchParams.get("dataFim") === "2026-10-31",
    );
    await historico.desde.fill("2026-10-01");
    await historico.ate.fill("2026-10-31");
    await historico.filtrar.click();
    expect((await resposta).status()).toBe(200);
    await historico.esperarLista();
    await expect(historico.linha(BOBI.data)).toBeVisible();
    for (const data of await historico.linhas
      .locator("td:first-child")
      .allTextContents()) {
      expect(data.trim()).toMatch(/^\d{2}\/10\/2026$/);
    }
  });

  await test.step("3. abrir o registo de 2026-10-14", async () => {
    await historico.verRegisto(BOBI.data);
    await expect(
      historico.detalhe.getByRole("heading", {
        name: `Registo de ${BOBI.data}`,
      }),
    ).toBeVisible();
    await expect(historico.campo("Notas clínicas")).toHaveText(BOBI.notas);
    await expect(historico.campo("Diagnóstico")).toHaveText(BOBI.diagnostico);
    await expect(historico.tratamentos.getByRole("listitem")).toHaveText([
      BOBI.tratamento,
    ]);
  });
});

test("ST-WEB-F004-06 — Recusar um intervalo de datas inválido no histórico", async ({
  page,
  request,
}) => {
  const bobi = await idDoBobi(request);
  await entrarComo.veterinario(page);
  const historico = new PaginaHistoricoClinico(page);

  await test.step("1. histórico clínico do Bobi", async () => {
    await historico.abrir(bobi);
    await expect(historico.linha(BOBI.data)).toBeVisible();
  });

  await test.step("2. data de fim anterior à de início", async () => {
    const pedidos = registarPedidos(page);
    await historico.desde.fill("2026-10-31");
    await historico.ate.fill("2026-10-01");
    await historico.filtrar.click();
    // O W13 valida o intervalo antes do pedido (mensagem local, a mesma da API).
    await expect(historico.ate).toHaveAccessibleDescription(
      FIM_ANTERIOR_AO_INICIO,
    );
    expect(pedidosA(pedidos, "GET", HISTORICO)).toHaveLength(0);
    // A lista anterior mantém-se e o ecrã não fica bloqueado.
    await expect(historico.linha(BOBI.data)).toBeVisible();
    await expect(page.locator("main").getByRole("alert")).toHaveCount(0);
    await expect(historico.filtrar).toBeEnabled();

    await historico.ate.fill("2026-10-31");
    await historico.desde.fill("2026-10-01");
    await historico.filtrar.click();
    await expect(historico.ate).not.toHaveAttribute("aria-invalid", "true");
    await historico.esperarLista();
    await expect(historico.linha(BOBI.data)).toBeVisible();
  });
});

test("ST-WEB-F004-07 — O Auxiliar Veterinário consulta só a informação clínica autorizada", async ({
  page,
  request,
}) => {
  // Resultado esperado da I128 v02 (PR #293), alinhado com a deliberação 5 do
  // Sprint Review do Sprint 07 (m4-s07-20261122-sprint-review-ata-v01.pdf): o
  // Auxiliar deixa de receber as notas clínicas e o diagnóstico (EP-033 e
  // EP-034) e mantém o resto do registo em leitura (tratamento e prescrição).
  const bobi = await idDoBobi(request);
  await iniciarSessao(page, CONTAS.auxCarla);
  const historico = new PaginaHistoricoClinico(page);

  await test.step("1. ficha do Bobi → «Registo clínico»", async () => {
    await page.goto(`/pacientes/${bobi}`);
    const ficha = new PaginaPaciente(page);
    await expect(ficha.areaClinica.getByRole("link")).toHaveText([
      "Registo clínico",
    ]);
    await expect(page.getByRole("link", { name: "Vacinação" })).toHaveCount(0);

    const resposta = respostaDe(page, "GET", HISTORICO);
    await ficha.areaClinica
      .getByRole("link", { name: "Registo clínico", exact: true })
      .click();
    const corpo = (await (await resposta).json()) as {
      registos: Record<string, unknown>[];
    };
    for (const linha of corpo.registos) {
      expect(linha).not.toHaveProperty("diagnostico");
    }

    await historico.esperarLista();
    await expect(historico.soLeitura).toBeVisible();
    await expect(historico.cabecalhos).toHaveText([
      "Data da consulta",
      "Veterinário",
      "Tratamentos",
      "Prescrições",
      "Ações",
    ]);
    await expect(historico.linha(BOBI.data).locator("td")).toHaveText([
      BOBI.data,
      ANA.nome,
      "1",
      "1",
      "Ver registo",
    ]);
  });

  await test.step("2. abrir o registo de 2026-10-14, só de leitura", async () => {
    const resposta = respostaDe(page, "GET", /^\/registos-clinicos\/\d+$/);
    await historico.verRegisto(BOBI.data);
    const registo = (await (await resposta).json()) as Record<string, unknown>;
    expect(registo).not.toHaveProperty("notasClinicas");
    expect(registo).not.toHaveProperty("diagnostico");

    await expect(historico.detalheDados.locator("dt")).toHaveText([
      "Tratamentos",
      "Prescrições",
    ]);
    await expect(historico.tratamentos.getByRole("listitem")).toHaveText([
      BOBI.tratamento,
    ]);
    await expect(historico.prescricoes).toContainText(BOBI.prescricao);
    const conteudo = page.locator("main");
    await expect(conteudo).not.toContainText(BOBI.diagnostico);
    await expect(conteudo).not.toContainText("Canal auditivo");
    // Sem ações para criar ou alterar notas, diagnósticos, tratamentos ou prescrições.
    await expect(historico.alterarRegisto).toHaveCount(0);
    await expect(conteudo.locator("textarea")).toHaveCount(0);
    await expect(
      conteudo.getByRole("button", {
        name: /Criar registo|Guardar alterações|Adicionar/,
      }),
    ).toHaveCount(0);
  });
});

import { expect, test, type Browser, type Page } from "@playwright/test";
import { CONTAS } from "./apoio/contas";
import {
  dataLisboa,
  dataPt,
  diaDaSemana,
  diaDeMarcacao,
  horaLisboa,
  somarDias,
  sufixo,
} from "./apoio/dados";
import { pedidosA, registarPedidos, respostaDe } from "./apoio/pedidos";
import {
  confirmarMarcacaoAberta,
  criarMarcacaoDeTeste,
  prepararPaciente,
} from "./apoio/procedimentos";
import { entrarComo, terminarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaAcessoNegado } from "./paginas/PaginaAvisos";
import { PaginaAgenda } from "./paginas/PaginaAgenda";
import { PaginaMarcacao } from "./paginas/PaginaMarcacao";
import { PaginaMarcacaoForm } from "./paginas/PaginaMarcacaoForm";

// I128, secção 9 — F003: disponibilidade, marcações, estados, agenda e
// chegada (W09, W10, W11). Automação: I130.

const ANA = CONTAS.vetAna;
const BRUNO = CONTAS.vetBruno;

/** Inícios de consulta de um dia útil (09:00–13:00 e 14:00–19:00, 30 min). */
const HORARIOS_DIA_UTIL = [
  ...["09", "10", "11", "12"].flatMap((h) => [`${h}:00`, `${h}:30`]),
  ...["14", "15", "16", "17", "18"].flatMap((h) => [`${h}:00`, `${h}:30`]),
];

/** Abre «Nova marcação» a partir do menu «Agenda». */
async function abrirNovaMarcacao(page: Page): Promise<PaginaMarcacaoForm> {
  await new LayoutApp(page).irPara("Agenda");
  const agenda = new PaginaAgenda(page);
  await agenda.titulo.waitFor();
  await agenda.novaMarcacao.click();
  const formulario = new PaginaMarcacaoForm(page);
  await formulario.pesquisaPaciente.waitFor();
  return formulario;
}

/** Abre a agenda do dia indicado a partir do menu «Agenda». */
async function abrirAgendaDoDia(
  page: Page,
  data: string,
): Promise<PaginaAgenda> {
  await new LayoutApp(page).irPara("Agenda");
  const agenda = new PaginaAgenda(page);
  await agenda.irParaDia(data);
  return agenda;
}

/** Segunda janela anónima do navegador, com a sessão da Rececionista. */
async function janelaDaRececionista(browser: Browser): Promise<Page> {
  const contexto = await browser.newContext();
  const pagina = await contexto.newPage();
  await entrarComo.rececionista(pagina);
  return pagina;
}

test("ST-WEB-F003-03 — Consultar a disponibilidade de um Veterinário", async ({
  page,
}) => {
  const suf = sufixo("F003-03");
  const dia = diaDeMarcacao(3);
  const { cliente, paciente } = await prepararPaciente(page, suf);

  const formulario = await abrirNovaMarcacao(page);
  await formulario.escolherPaciente(paciente.nome, cliente.nome);
  await formulario.consultarDisponibilidade(ANA.nome, dia);

  await expect(formulario.horarios).toContainText(
    "Duração de cada consulta: 30 minutos.",
  );
  const horas = await formulario.horasLivres(dia);
  // Só inícios dos dois períodos, de 30 em 30 minutos: nada às 13:00–13:30
  // nem a partir das 19:00.
  expect(horas.length).toBeGreaterThan(0);
  for (const hora of horas) expect(HORARIOS_DIA_UTIL).toContain(hora);
  expect(horas).not.toContain("13:00");
  expect(horas).not.toContain("13:30");
  expect(horas.filter((h) => h >= "19:00")).toHaveLength(0);
  expect(horas.some((h) => h < "13:00")).toBe(true);
  expect(horas.some((h) => h >= "14:00")).toBe(true);

  // A disponibilidade mostra 7 dias a partir de {D}: o domingo não tem horários.
  const domingo = [0, 1, 2, 3, 4, 5, 6]
    .map((n) => somarDias(dia, n))
    .find((d) => diaDaSemana(d) === 0);
  expect(domingo).toBeDefined();
  await expect(formulario.dia(domingo as string)).toContainText(
    "Sem horários livres.",
  );
  await expect(
    formulario.dia(domingo as string).getByRole("button"),
  ).toHaveCount(0);
});

test("ST-WEB-F003-04 — A disponibilidade exclui as marcações não canceladas", async ({
  page,
}) => {
  const suf = sufixo("F003-04");
  const dia = diaDeMarcacao(4);
  const { cliente, paciente } = await prepararPaciente(page, suf);

  const marcacao = await test.step("1. PC-05 às 10:00 com o Dr. Bruno", () =>
    criarMarcacaoDeTeste(page, suf, cliente, paciente, {
      veterinario: BRUNO,
      data: dia,
      hora: "10:00",
    }));

  await test.step("2. o horário ocupado não aparece", async () => {
    const formulario = await abrirNovaMarcacao(page);
    await formulario.escolherPaciente(paciente.nome, cliente.nome);
    await formulario.consultarDisponibilidade(BRUNO.nome, dia);
    await expect(formulario.dia(dia).getByRole("button").first()).toBeVisible();
    await expect(formulario.horario(dia, "10:00")).toHaveCount(0);
  });

  await test.step("3. cancelar a marcação", async () => {
    const detalhe = new PaginaMarcacao(page);
    await detalhe.abrir(marcacao.id);
    await detalhe.mudarEstado("Cancelar marcação", "Cancelada");
  });

  await test.step("4. o horário volta a aparecer", async () => {
    const formulario = await abrirNovaMarcacao(page);
    await formulario.escolherPaciente(paciente.nome, cliente.nome);
    await formulario.consultarDisponibilidade(BRUNO.nome, dia);
    await expect(formulario.horario(dia, "10:00")).toBeVisible();
  });
});

test("ST-WEB-F003-05 — Criar uma marcação", async ({ page }) => {
  const suf = sufixo("F003-05");
  const dia = diaDeMarcacao(5);
  const { cliente, paciente } = await prepararPaciente(page, suf);

  const marcacao = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });

  await test.step("1. detalhe da marcação criada", async () => {
    const detalhe = new PaginaMarcacao(page);
    await expect(detalhe.campo("Paciente")).toContainText(
      `${paciente.nome} · Cão`,
    );
    await expect(detalhe.campo("Cliente")).toHaveText(cliente.nome);
    await expect(detalhe.campo("Veterinário")).toHaveText(ANA.nome);
    await expect(detalhe.campo("Data e hora")).toHaveText(
      `${dataPt(dia)}, ${marcacao.hora} · 30 minutos`,
    );
    await expect(detalhe.campo("Motivo")).toHaveText(marcacao.motivo);
    await expect(detalhe.campo("Estado")).toHaveText("Pendente");
  });

  await test.step("2. a marcação aparece na agenda de {D}", async () => {
    const agenda = await abrirAgendaDoDia(page, dia);
    const linha = agenda.linhasCom(paciente.nome);
    await expect(linha).toHaveCount(1);
    await expect(linha.locator("td")).toHaveText([
      marcacao.hora,
      paciente.nome,
      cliente.nome,
      ANA.nome,
      marcacao.motivo,
      "Pendente",
      "—",
      "Ver",
    ]);
  });
});

test("ST-WEB-F003-06 — Recusar uma marcação incompleta ou numa vaga ocupada", async ({
  page,
  browser,
}) => {
  test.slow();
  const suf = sufixo("F003-06");
  const dia = diaDeMarcacao(6);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const pedidos = registarPedidos(page);
  const formulario = await abrirNovaMarcacao(page);

  await test.step("1. criar sem preencher nada", async () => {
    await formulario.criar.click();
    await expect(page.getByText("Escolha o paciente.")).toBeVisible();
    await expect(page.getByText("Escolha o Veterinário.")).toBeVisible();
    await expect(
      page.getByText(
        "Consulte a disponibilidade e escolha um dos horários livres.",
      ),
    ).toBeVisible();
    expect(pedidosA(pedidos, "POST", "/consultas")).toHaveLength(0);
  });

  await formulario.escolherPaciente(paciente.nome, cliente.nome);
  await formulario.consultarDisponibilidade(BRUNO.nome, dia);
  const hora = await formulario.escolherHorario(dia);

  await test.step("3. a segunda janela ocupa o mesmo horário", async () => {
    const janela = await janelaDaRececionista(browser);
    try {
      await criarMarcacaoDeTeste(janela, suf, cliente, paciente, {
        veterinario: BRUNO,
        data: dia,
        hora,
      });
    } finally {
      await janela.context().close();
    }
  });

  await test.step("4. criar na primeira janela: vaga ocupada", async () => {
    const resposta = respostaDe(page, "POST", /^\/consultas$/);
    await formulario.criar.click();
    expect((await resposta).status()).toBe(422);
    await expect(formulario.erro).toContainText(
      "O veterinário já tem uma marcação que se sobrepõe a esse horário.",
    );
    await expect(formulario.aviso).toHaveText(
      "A disponibilidade foi atualizada. Escolha outro horário livre.",
    );
    await expect(formulario.horario(dia, hora)).toHaveCount(0);
    await expect(formulario.horarioEscolhido).toContainText("nenhum");
    await expect(page).toHaveURL(/\/agenda\/nova$/);
  });

  await test.step("uma única marcação nesse horário", async () => {
    const agenda = await abrirAgendaDoDia(page, dia);
    await agenda.escolherVeterinario(BRUNO.nome);
    const linhas = agenda.linhasCom(paciente.nome);
    await expect(linhas).toHaveCount(1);
    await expect(linhas.locator("td").first()).toHaveText(hora);
  });
});

test("ST-WEB-F003-07 — Alterar uma marcação", async ({ page }) => {
  test.slow();
  const suf = sufixo("F003-07");
  const dia = diaDeMarcacao(7);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const marcacao = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  const detalhe = new PaginaMarcacao(page);
  const formulario = new PaginaMarcacaoForm(page);
  const novoMotivo = `Motivo alterado ${suf}`;

  await test.step("1–2. alterar só o motivo", async () => {
    await detalhe.editar.click();
    await expect(formulario.titulo).toHaveText("Editar marcação");
    await expect(formulario.horarioEscolhido).toContainText(
      `${dataPt(dia)} às ${marcacao.hora} (horário atual)`,
    );
    await formulario.motivo.fill(novoMotivo);
    await formulario.guardarAlteracoes.click();
    await expect(detalhe.aviso).toHaveText("Marcação alterada.");
    await expect(detalhe.campo("Motivo")).toHaveText(novoMotivo);
    await expect(detalhe.campo("Data e hora")).toHaveText(
      `${dataPt(dia)}, ${marcacao.hora} · 30 minutos`,
    );
  });

  const novaHora =
    await test.step("3. mudar para o segundo horário livre", async () => {
      await detalhe.editar.click();
      await formulario.consultarDisponibilidade(null, dia);
      const livres = await formulario.horasLivres(dia);
      expect(livres.length).toBeGreaterThan(1);
      const escolhida = await formulario.escolherHorario(dia, livres[1]);
      expect(escolhida).not.toBe(marcacao.hora);
      await expect(formulario.horarioEscolhido).not.toContainText(
        "(horário atual)",
      );
      await formulario.guardarAlteracoes.click();
      await expect(detalhe.aviso).toHaveText("Marcação alterada.");
      await expect(detalhe.campo("Data e hora")).toHaveText(
        `${dataPt(dia)}, ${escolhida} · 30 minutos`,
      );
      return escolhida;
    });

  await test.step("4. recarregar o detalhe e a agenda", async () => {
    await page.reload();
    await expect(detalhe.campo("Motivo")).toHaveText(novoMotivo);
    await expect(detalhe.campo("Data e hora")).toHaveText(
      `${dataPt(dia)}, ${novaHora} · 30 minutos`,
    );
    await expect(detalhe.campo("Estado")).toHaveText("Pendente");

    const agenda = await abrirAgendaDoDia(page, dia);
    const linha = agenda.linhasCom(paciente.nome);
    await expect(linha).toHaveCount(1);
    await expect(linha.locator("td")).toHaveText([
      novaHora,
      paciente.nome,
      cliente.nome,
      ANA.nome,
      novoMotivo,
      "Pendente",
      "—",
      "Ver",
    ]);
  });
});

test("ST-WEB-F003-08 — Recusar a alteração concorrente ou de uma marcação final", async ({
  page,
  browser,
}) => {
  test.slow();
  const suf = sufixo("F003-08");
  const dia = diaDeMarcacao(8);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const marcacao = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: BRUNO,
    data: dia,
  });
  const janela = await janelaDaRececionista(browser);
  try {
    const formularioA = new PaginaMarcacaoForm(page);
    const formularioB = new PaginaMarcacaoForm(janela);
    const detalheB = new PaginaMarcacao(janela);

    await test.step("1. «Editar marcação» nas duas janelas", async () => {
      await formularioA.abrirEdicao(marcacao.id);
      await formularioB.abrirEdicao(marcacao.id);
      await expect(formularioA.motivo).toHaveValue(marcacao.motivo);
      await expect(formularioB.motivo).toHaveValue(marcacao.motivo);
    });

    await test.step("2. a janela B guarda primeiro", async () => {
      await formularioB.motivo.fill(`Janela B ${suf}`);
      await formularioB.guardarAlteracoes.click();
      await expect(detalheB.aviso).toHaveText("Marcação alterada.");
    });

    await test.step("3. a janela A guarda depois: 412", async () => {
      const resposta = respostaDe(
        page,
        "PUT",
        new RegExp(`^/consultas/${marcacao.id}$`),
      );
      await formularioA.motivo.fill(`Janela A ${suf}`);
      await formularioA.guardarAlteracoes.click();
      expect((await resposta).status()).toBe(412);
      await expect(formularioA.aviso).toHaveText(
        "A marcação foi alterada por outra pessoa. Os dados foram atualizados: reveja-os e guarde de novo, se ainda fizer sentido.",
      );
      await expect(formularioA.motivo).toHaveValue(`Janela B ${suf}`);
      await expect(page).toHaveURL(/\/editar$/);
    });

    await test.step("4. a janela B cancela a marcação", async () => {
      await detalheB.mudarEstado("Cancelar marcação", "Cancelada");
      await expect(detalheB.campo("Estado")).toHaveText("Cancelada");
      await expect(detalheB.campo("Motivo")).toHaveText(`Janela B ${suf}`);
      await expect(detalheB.editar).toHaveCount(0);
      await expect(detalheB.botoesAcoes).toHaveCount(0);
      await expect(detalheB.acoes).toContainText(
        "Cancelada é um estado final: a marcação já não pode ser alterada.",
      );
    });

    await test.step("5. /agenda/{id}/editar da marcação cancelada", async () => {
      await formularioA.abrirEdicao(marcacao.id);
      await expect(formularioA.naoAlteravel).toHaveText(
        "Esta marcação está Cancelada e já não pode ser alterada.",
      );
      await expect(formularioA.guardarAlteracoes).toHaveCount(0);
      await expect(formularioA.motivo).toHaveCount(0);
    });
  } finally {
    await janela.context().close();
  }
});

test("ST-WEB-F003-09 — Atualizar o estado da marcação segundo as transições permitidas", async ({
  page,
}) => {
  const suf = sufixo("F003-09");
  const dia = diaDeMarcacao(9);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  const detalhe = new PaginaMarcacao(page);
  const observacao = `Confirmada por telefone ${suf}`;

  await test.step("1. Pendente: só Confirmar e Cancelar", async () => {
    await expect(detalhe.botoesAcoes).toHaveText([
      "Confirmar marcação",
      "Cancelar marcação",
    ]);
  });

  await test.step("2. confirmar com observação", async () => {
    await detalhe.transicao("Confirmar marcação").click();
    await expect(detalhe.confirmacao).toContainText(
      "Mudar o estado de Pendente para Confirmada?",
    );
    await detalhe.confirmacao
      .getByLabel("Observação (opcional)")
      .fill(observacao);
    await detalhe.confirmacao
      .getByRole("button", { name: "Confirmar", exact: true })
      .click();
    await expect(detalhe.aviso).toHaveText("Estado alterado para Confirmada.");
    await expect(detalhe.campo("Estado")).toHaveText("Confirmada");
    await expect(detalhe.campo("Observação do estado")).toHaveText(observacao);
  });

  await test.step("3. marcar como realizada", async () => {
    await detalhe.mudarEstado("Marcar como realizada", "Realizada");
  });

  await test.step("4. recarregar: estado final", async () => {
    await page.reload();
    await expect(detalhe.campo("Estado")).toHaveText("Realizada");
    await expect(detalhe.botoesAcoes).toHaveCount(0);
    await expect(detalhe.editar).toHaveCount(0);
    await expect(detalhe.acoes).toContainText(
      "Realizada é um estado final: a marcação já não pode ser alterada.",
    );
  });
});

test("ST-WEB-F003-10 — Não oferecer transições de estado não previstas", async ({
  page,
}) => {
  const suf = sufixo("F003-10");
  const dia = diaDeMarcacao(10);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: BRUNO,
    data: dia,
  });
  const detalhe = new PaginaMarcacao(page);
  const motivoCancelamento = `Pedido do tutor ${suf}`;

  await test.step("1. Pendente não passa a Realizada", async () => {
    await expect(detalhe.botoesAcoes).toHaveText([
      "Confirmar marcação",
      "Cancelar marcação",
    ]);
    await expect(detalhe.transicao("Marcar como realizada")).toHaveCount(0);
  });

  await test.step("2. cancelar com motivo", async () => {
    await detalhe.transicao("Cancelar marcação").click();
    await detalhe.confirmacao
      .getByLabel("Motivo do cancelamento (opcional)")
      .fill(motivoCancelamento);
    await detalhe.confirmacao
      .getByRole("button", { name: "Confirmar", exact: true })
      .click();
    await expect(detalhe.aviso).toHaveText("Estado alterado para Cancelada.");
  });

  await test.step("3. recarregar: Cancelada, sem ações", async () => {
    await page.reload();
    await expect(detalhe.campo("Estado")).toHaveText("Cancelada");
    await expect(detalhe.campo("Observação do estado")).toHaveText(
      motivoCancelamento,
    );
    await expect(detalhe.botoesAcoes).toHaveCount(0);
    await expect(detalhe.editar).toHaveCount(0);
    await expect(detalhe.registarChegada).toHaveCount(0);
  });
});

test("ST-WEB-F003-11 — Consultar a agenda da clínica (Rececionista)", async ({
  page,
}) => {
  test.slow();
  const suf = sufixo("F003-11");
  const dia = diaDeMarcacao(11);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const daAna = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await confirmarMarcacaoAberta(page);
  const doBruno = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: BRUNO,
    data: dia,
  });
  let agenda!: PaginaAgenda;
  const nossas = () => agenda.linhasCom(paciente.nome);

  await test.step("1. agenda de {D} com as duas marcações", async () => {
    agenda = await abrirAgendaDoDia(page, dia);
    await expect(agenda.novaMarcacao).toBeVisible();
    await expect(nossas()).toHaveCount(2);
    await expect(
      nossas().filter({ hasText: ANA.nome }).locator("td"),
    ).toHaveText([
      daAna.hora,
      paciente.nome,
      cliente.nome,
      ANA.nome,
      daAna.motivo,
      "Confirmada",
      "—",
      "Ver",
    ]);
    await expect(
      nossas().filter({ hasText: BRUNO.nome }).locator("td"),
    ).toHaveText([
      doBruno.hora,
      paciente.nome,
      cliente.nome,
      BRUNO.nome,
      doBruno.motivo,
      "Pendente",
      "—",
      "Ver",
    ]);
  });

  await test.step("2. filtro Veterinário = Dra. Ana", async () => {
    await agenda.escolherVeterinario(ANA.nome);
    await expect(nossas()).toHaveCount(1);
    await expect(nossas()).toContainText(ANA.nome);
  });

  await test.step("3. Veterinário Todos e estado Pendente", async () => {
    await agenda.escolherVeterinario("Todos");
    await agenda.escolherEstado("Pendente");
    await expect(nossas()).toHaveCount(1);
    await expect(nossas()).toContainText(BRUNO.nome);
  });

  await test.step("filtro sem resultados", async () => {
    // Em {D} nenhum cenário deixa marcações Realizadas.
    await agenda.escolherEstado("Realizada");
    await expect(agenda.vazia).toBeVisible();
    await agenda.escolherEstado("Pendente");
    await expect(nossas()).toHaveCount(1);
  });

  await test.step("4. dia seguinte e dia anterior, com os filtros", async () => {
    await agenda.aposNovaLista(() => agenda.diaSeguinte.click());
    await expect(agenda.diaMostrado).toContainText(dataPt(somarDias(dia, 1)));
    await expect(nossas()).toHaveCount(0);
    await agenda.aposNovaLista(() => agenda.diaAnterior.click());
    await expect(agenda.diaMostrado).toContainText(dataPt(dia));
    await expect(agenda.estado).toHaveValue("Pendente");
    await expect(nossas()).toHaveCount(1);
    await expect(nossas()).toContainText(BRUNO.nome);
  });
});

test("ST-WEB-F003-12 — O Veterinário vê só as consultas que lhe estão atribuídas", async ({
  page,
}) => {
  test.slow();
  const suf = sufixo("F003-12");
  const dia = diaDeMarcacao(12);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const daAna = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: BRUNO,
    data: dia,
  });
  await terminarSessao(page);
  await entrarComo.veterinario(page);
  const agenda = new PaginaAgenda(page);

  await test.step("1. agenda da Dra. Ana em {D}", async () => {
    await expect(page).toHaveURL(/\/agenda$/);
    await agenda.esperarLista();
    await expect(agenda.soAtribuidas).toBeVisible();
    await expect(agenda.veterinario).toHaveCount(0);
    await expect(agenda.novaMarcacao).toHaveCount(0);
    await agenda.irParaDia(dia);
    const nossas = agenda.linhasCom(paciente.nome);
    await expect(nossas).toHaveCount(1);
    await expect(nossas).toContainText(ANA.nome);
    await expect(nossas.filter({ hasText: BRUNO.nome })).toHaveCount(0);
  });

  await test.step("2. detalhe sem «Ações» e com a ficha do paciente", async () => {
    await agenda.verMarcacao(paciente.nome, daAna.hora).click();
    const detalhe = new PaginaMarcacao(page);
    await detalhe.dados.waitFor();
    await expect(detalhe.campo("Veterinário")).toHaveText(ANA.nome);
    await expect(detalhe.acoes).toHaveCount(0);
    await expect(page.getByRole("heading", { name: "Ações" })).toHaveCount(0);
    await expect(detalhe.abrirFicha).toBeVisible();
  });

  await test.step("3. /agenda/nova", async () => {
    await page.goto("/agenda/nova");
    await expect(page).toHaveURL(/\/acesso-negado$/);
    await expect(new PaginaAcessoNegado(page).titulo).toBeVisible();
  });
});

test("ST-WEB-F003-13 — Registar a chegada à consulta", async ({ page }) => {
  const suf = sufixo("F003-13");
  const dia = diaDeMarcacao(13);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: ANA,
    data: dia,
  });
  await confirmarMarcacaoAberta(page);
  const detalhe = new PaginaMarcacao(page);

  const chegada = await test.step("1. registar a chegada", async () => {
    const antes = horaLisboa();
    await detalhe.registarChegada.click();
    await expect(detalhe.aviso).toHaveText(
      /^Chegada registada às \d{2}:\d{2}\. O estado da marcação não muda\.$/,
    );
    const depois = horaLisboa();
    const hora = /às (\d{2}:\d{2})/.exec(
      (await detalhe.aviso.textContent()) ?? "",
    )?.[1];
    // A hora é a do servidor, em Lisboa: a de agora (ou a do minuto anterior).
    expect([antes, depois]).toContain(hora);
    await expect(detalhe.campo("Estado")).toHaveText("Confirmada");
    await expect(detalhe.registarChegada).toHaveCount(0);
    return hora as string;
  });

  await test.step("2. recarregar o detalhe e a agenda de {D}", async () => {
    await page.reload();
    await expect(detalhe.campo("Chegada")).toHaveText(
      `${dataPt(dataLisboa(0))}, ${chegada}`,
    );
    await expect(detalhe.campo("Estado")).toHaveText("Confirmada");
    const agenda = await abrirAgendaDoDia(page, dia);
    const linha = agenda.linhasCom(paciente.nome);
    await expect(linha).toHaveCount(1);
    // Coluna «Chegada» (a 7.ª: Hora, Paciente, Cliente, Veterinário, Motivo, Estado, Chegada).
    await expect(linha.locator("td").nth(6)).toHaveText(chegada);
  });
});

test("ST-WEB-F003-14 — Não registar a chegada fora de uma marcação Confirmada nem duas vezes", async ({
  page,
  browser,
}) => {
  test.slow();
  const suf = sufixo("F003-14");
  const dia = diaDeMarcacao(14);
  const { cliente, paciente } = await prepararPaciente(page, suf);
  const marcacao = await criarMarcacaoDeTeste(page, suf, cliente, paciente, {
    veterinario: BRUNO,
    data: dia,
  });
  const detalheA = new PaginaMarcacao(page);

  await test.step("1. Pendente: sem «Registar chegada»", async () => {
    await expect(detalheA.campo("Estado")).toHaveText("Pendente");
    await expect(detalheA.registarChegada).toHaveCount(0);
  });

  await test.step("2. PC-06", async () => {
    await confirmarMarcacaoAberta(page);
    await expect(detalheA.registarChegada).toBeVisible();
  });

  const janela = await janelaDaRececionista(browser);
  try {
    await test.step("3. chegada na janela B e depois na janela A", async () => {
      const detalheB = new PaginaMarcacao(janela);
      await detalheA.abrir(marcacao.id);
      await detalheB.abrir(marcacao.id);
      await expect(detalheA.registarChegada).toBeVisible();

      await detalheB.registarChegada.click();
      await expect(detalheB.aviso).toHaveText(/^Chegada registada às /);
      const chegadaB = (
        (await detalheB.campo("Chegada").textContent()) ?? ""
      ).trim();
      expect(chegadaB).toMatch(/^\d{2}\/\d{2}\/\d{4}, \d{2}:\d{2}$/);

      const resposta = respostaDe(
        page,
        "PATCH",
        new RegExp(`^/consultas/${marcacao.id}/chegada$`),
      );
      await detalheA.registarChegada.click();
      // A janela A envia o ETag que leu antes da chegada da janela B: a API
      // recusa com 412 (alteração concorrente). Sem ETag seria 422 («A
      // chegada desta marcação já foi registada.»); a I128 aceita os dois.
      expect((await resposta).status()).toBe(412);
      await expect(detalheA.erro).toBeVisible();
      await expect(detalheA.avisoConflito).toHaveText(
        "A marcação foi alterada por outra pessoa. Os dados abaixo já estão atualizados: confirme se a operação ainda faz sentido e repita-a.",
      );
      await expect(detalheA.campo("Chegada")).toHaveText(chegadaB);
      await expect(detalheA.registarChegada).toHaveCount(0);

      // A chegada registada na janela B não muda.
      await janela.reload();
      await expect(detalheB.campo("Chegada")).toHaveText(chegadaB);
      await expect(detalheB.campo("Estado")).toHaveText("Confirmada");
    });
  } finally {
    await janela.context().close();
  }
});

import { expect, test, type Page } from "@playwright/test";
import { telefoneNovo } from "./apoio/dados";
import { pedidosA, registarPedidos, respostaDe } from "./apoio/pedidos";
import { entrarComo } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaClinica, type Dia } from "./paginas/PaginaClinica";

// I128, secções 7 e 9 — dados, configurações e horário da clínica (W06).
// Automação: I129. Os cenários que alteram a configuração repõem-na sempre no
// fim e este ficheiro corre sozinho, depois dos restantes (projeto
// «sistema-exclusivo»; I128, secção 5.5, regra 3).

const HORARIO_DEMONSTRACAO: Record<Dia, string[][]> = {
  Segunda: [
    ["09:00", "13:00"],
    ["14:00", "19:00"],
  ],
  Terça: [
    ["09:00", "13:00"],
    ["14:00", "19:00"],
  ],
  Quarta: [
    ["09:00", "13:00"],
    ["14:00", "19:00"],
  ],
  Quinta: [
    ["09:00", "13:00"],
    ["14:00", "19:00"],
  ],
  Sexta: [
    ["09:00", "13:00"],
    ["14:00", "19:00"],
  ],
  Sábado: [["09:00", "13:00"]],
  Domingo: [],
};

async function abrirClinica(page: Page): Promise<PaginaClinica> {
  const clinica = new PaginaClinica(page);
  await entrarComo.administrador(page);
  await new LayoutApp(page).irPara("Clínica e horários");
  await clinica.esperarFormulario();
  return clinica;
}

/** Confirma que o horário no ecrã é o dos dados de demonstração. */
async function confirmarHorarioDemonstracao(clinica: PaginaClinica) {
  for (const [dia, periodos] of Object.entries(HORARIO_DEMONSTRACAO) as [
    Dia,
    string[][],
  ][]) {
    await expect(clinica.horasDoDia(dia), dia).toHaveCount(periodos.length * 2);
    for (const [i, [abertura, fecho]] of periodos.entries()) {
      await expect(clinica.hora(dia, i + 1, "abertura")).toHaveValue(abertura);
      await expect(clinica.hora(dia, i + 1, "fecho")).toHaveValue(fecho);
    }
    if (periodos.length === 0) {
      await expect(clinica.dia(dia)).toContainText("Encerrado");
    }
  }
}

/**
 * Limpeza do horário: repõe o dos dados de demonstração (segunda a sexta
 * 09:00–13:00 e 14:00–19:00, sábado 09:00–13:00, domingo encerrado).
 */
async function reporHorario(page: Page, clinica: PaginaClinica) {
  await page.reload();
  await clinica.esperarFormulario();
  for (const [dia, periodos] of Object.entries(HORARIO_DEMONSTRACAO) as [
    Dia,
    string[][],
  ][]) {
    while ((await clinica.horasDoDia(dia).count()) > periodos.length * 2) {
      await clinica.removerPeriodo(dia, 1).click();
    }
    while ((await clinica.horasDoDia(dia).count()) < periodos.length * 2) {
      await clinica.adicionarPeriodo(dia).click();
    }
    for (const [i, [abertura, fecho]] of periodos.entries()) {
      await clinica.hora(dia, i + 1, "abertura").fill(abertura);
      await clinica.hora(dia, i + 1, "fecho").fill(fecho);
    }
  }
  await clinica.guardar.click();
  await expect(clinica.aviso).toHaveText("Alterações guardadas.");
}

test("ST-WEB-F001-19 — Consultar os dados gerais e as configurações da clínica", async ({
  page,
}) => {
  const clinica = await abrirClinica(page);
  await expect(clinica.nome).toHaveValue("Clínica Veterinária G08");
  await expect(clinica.nif).toHaveValue("501234560");
  await expect(clinica.morada).not.toHaveValue("");
  await expect(clinica.telefone).not.toHaveValue("");
  await expect(clinica.email).not.toHaveValue("");
  await expect(clinica.duracao).toHaveValue("30");
  await confirmarHorarioDemonstracao(clinica);
});

test("ST-WEB-F001-20 — Atualizar os dados gerais e as configurações da clínica (exclusivo)", async ({
  page,
}) => {
  const clinica = await abrirClinica(page);
  const telefoneOriginal = await clinica.telefone.inputValue();
  const duracaoOriginal = await clinica.duracao.inputValue();
  const novoTelefone = telefoneNovo();

  try {
    await clinica.telefone.fill(novoTelefone);
    await clinica.duracao.fill("45");
    await clinica.guardar.click();
    await expect(clinica.aviso).toHaveText("Alterações guardadas.");

    await page.reload();
    await clinica.esperarFormulario();
    await expect(clinica.telefone).toHaveValue(novoTelefone);
    await expect(clinica.duracao).toHaveValue("45");
  } finally {
    await test.step("4. limpeza: repor telefone e duração", async () => {
      await page.reload();
      await clinica.esperarFormulario();
      await clinica.telefone.fill(telefoneOriginal);
      await clinica.duracao.fill(duracaoOriginal);
      await clinica.guardar.click();
      await expect(clinica.aviso).toHaveText("Alterações guardadas.");
      await expect(clinica.telefone).toHaveValue(telefoneOriginal);
      await expect(clinica.duracao).toHaveValue(duracaoOriginal);
    });
  }
});

test("ST-WEB-F001-21 — Recusar dados gerais inválidos da clínica (exclusivo)", async ({
  page,
}) => {
  const clinica = await abrirClinica(page);
  const pedidos = registarPedidos(page);
  const nifOriginal = await clinica.nif.inputValue();
  const nomeOriginal = await clinica.nome.inputValue();
  const duracaoOriginal = await clinica.duracao.inputValue();

  await test.step("2. nome vazio", async () => {
    await clinica.nome.fill("");
    await clinica.guardar.click();
    await expect(clinica.errosValidacao).toContainText(
      "Indique o nome da clínica.",
    );
    await clinica.cancelar.click();
  });

  await test.step("3. duração 3", async () => {
    await clinica.duracao.fill("3");
    await clinica.guardar.click();
    await expect(clinica.errosValidacao).toContainText(
      "A duração da consulta deve ser um número inteiro de minutos entre 5 e 240.",
    );
    await clinica.cancelar.click();
    expect(pedidosA(pedidos, "PUT", "/clinica")).toHaveLength(0);
  });

  await test.step("4. NIF inválido (400)", async () => {
    await clinica.nif.fill("123456788");
    const resposta = respostaDe(page, "PUT", /^\/clinica$/);
    await clinica.guardar.click();
    expect((await resposta).status()).toBe(400);
    await expect(clinica.erroApi).toBeVisible();
    await expect(clinica.nif).toHaveValue("123456788");
  });

  await test.step("5. nada foi gravado", async () => {
    await clinica.cancelar.click();
    await page.reload();
    await clinica.esperarFormulario();
    await expect(clinica.nif).toHaveValue(nifOriginal);
    await expect(clinica.nome).toHaveValue(nomeOriginal);
    await expect(clinica.duracao).toHaveValue(duracaoOriginal);
  });
});

test("ST-WEB-F003-01 — Gerir o horário de funcionamento (exclusivo)", async ({
  page,
}) => {
  const clinica = await abrirClinica(page);
  await confirmarHorarioDemonstracao(clinica);

  try {
    await clinica.hora("Segunda", 1, "fecho").fill("14:00");
    await clinica.removerPeriodo("Sábado", 1).click();
    await clinica.adicionarPeriodo("Domingo").click();
    await clinica.hora("Domingo", 1, "abertura").fill("10:00");
    await clinica.hora("Domingo", 1, "fecho").fill("12:00");
    await clinica.guardar.click();
    await expect(clinica.aviso).toHaveText("Alterações guardadas.");

    await page.reload();
    await clinica.esperarFormulario();
    await expect(clinica.hora("Segunda", 1, "abertura")).toHaveValue("09:00");
    await expect(clinica.hora("Segunda", 1, "fecho")).toHaveValue("14:00");
    await expect(clinica.hora("Segunda", 2, "abertura")).toHaveValue("14:00");
    await expect(clinica.hora("Segunda", 2, "fecho")).toHaveValue("19:00");
    await expect(clinica.dia("Sábado")).toContainText("Encerrado");
    await expect(clinica.hora("Domingo", 1, "abertura")).toHaveValue("10:00");
    await expect(clinica.hora("Domingo", 1, "fecho")).toHaveValue("12:00");
  } finally {
    await test.step("6. limpeza: repor o horário original", async () => {
      await reporHorario(page, clinica);
      await page.reload();
      await clinica.esperarFormulario();
      await confirmarHorarioDemonstracao(clinica);
    });
  }
});

test("ST-WEB-F003-02 — Recusar períodos sobrepostos ou com fecho não posterior à abertura (exclusivo)", async ({
  page,
}) => {
  const clinica = await abrirClinica(page);
  const pedidos = registarPedidos(page);

  await test.step("2. período sobreposto à segunda-feira", async () => {
    await clinica.adicionarPeriodo("Segunda").click();
    await clinica.hora("Segunda", 3, "abertura").fill("12:00");
    await clinica.hora("Segunda", 3, "fecho").fill("15:00");
    await clinica.guardar.click();
    await expect(clinica.errosValidacao).toContainText(
      "Segunda: os períodos 09:00–13:00 e 12:00–15:00 sobrepõem-se.",
    );
    await expect(clinica.errosValidacao).toContainText(
      "Segunda: os períodos 12:00–15:00 e 14:00–19:00 sobrepõem-se.",
    );
    await clinica.cancelar.click();
  });

  await test.step("3. fecho igual à abertura", async () => {
    await clinica.hora("Terça", 2, "fecho").fill("14:00");
    await clinica.guardar.click();
    await expect(clinica.errosValidacao).toContainText(
      "Terça, período 2: o fecho tem de ser posterior à abertura.",
    );
    await clinica.cancelar.click();
  });

  await test.step("4. período sem horas", async () => {
    await clinica.adicionarPeriodo("Quinta").click();
    await clinica.guardar.click();
    await expect(clinica.errosValidacao).toContainText(
      "Quinta, período 3: indique a hora de abertura e a de fecho.",
    );
  });

  await test.step("5. nada foi gravado", async () => {
    expect(pedidosA(pedidos, "PUT", "/clinica")).toHaveLength(0);
    await clinica.cancelar.click();
    await page.reload();
    await clinica.esperarFormulario();
    await confirmarHorarioDemonstracao(clinica);
  });
});

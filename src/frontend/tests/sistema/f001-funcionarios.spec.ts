import { expect, test, type Page } from "@playwright/test";
import { CONTAS } from "./apoio/contas";
import { emailDe, senhaGerada, sufixo, telefoneNovo } from "./apoio/dados";
import { MENSAGEM_BLOQUEADA } from "./apoio/mensagens";
import { linhaEmAlgumaPagina } from "./apoio/paginacao";
import { pedidosA, registarPedidos, respostaDe } from "./apoio/pedidos";
import {
  entrarComo,
  iniciarSessao,
  terminarSessao,
  type ContaCriada,
} from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaFuncionarioForm } from "./paginas/PaginaFuncionarioForm";
import { PaginaFuncionarios } from "./paginas/PaginaFuncionarios";
import { PaginaLogin } from "./paginas/PaginaLogin";

// I128, secção 7 — F001: contas de funcionário (W02, W03). Automação: I129.

const APOIO = "Pode registar informação não clínica de apoio";

/** Cria uma conta pelo ecrã «Novo funcionário» (como em ST-WEB-F001-07). */
async function criarFuncionario(
  page: Page,
  nome: string,
  suf: string,
  perfil: string,
): Promise<ContaCriada> {
  const lista = new PaginaFuncionarios(page);
  await new LayoutApp(page).irPara("Funcionários");
  await lista.novoFuncionario.click();
  const ficha = new PaginaFuncionarioForm(page);
  const conta = { nome, email: emailDe(suf), senha: senhaGerada(), perfil };
  await ficha.preencher({ ...conta, telefone: telefoneNovo(), apoio: false });
  await ficha.guardar.click();
  await expect(lista.aviso).toHaveText(`Conta de ${nome} criada.`);
  return conta;
}

test("ST-WEB-F001-06 — Listar e pesquisar funcionários", async ({ page }) => {
  const lista = new PaginaFuncionarios(page);
  await entrarComo.administrador(page);
  await new LayoutApp(page).irPara("Funcionários");
  await lista.esperarLista();

  await test.step("1. colunas da lista", async () => {
    await expect(lista.tabela.locator("thead th")).toHaveText([
      "Nome",
      "E-mail",
      "Perfil",
      "Estado",
      "Registo de apoio",
      "Ações",
    ]);
    // A Administração é a primeira por ordem alfabética: está sempre na 1.ª página.
    await expect(lista.linha(CONTAS.admin.email)).toContainText("—");
  });

  await test.step("2. pesquisa «Carla»", async () => {
    await lista.pesquisar("Carla");
    await expect(lista.linhas).toHaveCount(1);
    await expect(lista.linhas).toContainText(CONTAS.auxCarla.nome);
    await expect(lista.linhas).toContainText("Autorizado");
  });

  await test.step("3. filtro Perfil = Auxiliar Veterinário", async () => {
    await lista.pesquisar("", "Auxiliar Veterinário");
    await expect(
      lista.linhas.filter({ hasNotText: "Auxiliar Veterinário" }),
    ).toHaveCount(0);
    await expect(
      await linhaEmAlgumaPagina(page, lista.linhas, CONTAS.auxCarla.nome),
    ).toContainText("Autorizado");
    await expect(
      await linhaEmAlgumaPagina(page, lista.linhas, CONTAS.auxDiogo.nome),
    ).toContainText("Não autorizado");
  });

  await test.step("4. filtro Estado = Bloqueado", async () => {
    await lista.pesquisar("", "Todos", "Bloqueado");
    await expect(lista.linhas.filter({ hasNotText: "Bloqueado" })).toHaveCount(
      0,
    );
    await expect(
      await linhaEmAlgumaPagina(
        page,
        lista.linhas,
        CONTAS.rececaoBloqueada.nome,
      ),
    ).toHaveCount(1);
  });
});

test("ST-WEB-F001-07 — Criar uma conta de funcionário e atribuir-lhe um perfil", async ({
  page,
}) => {
  const suf = sufixo("F001-07");
  const nome = `Funcionário ${suf}`;
  const lista = new PaginaFuncionarios(page);
  await entrarComo.administrador(page);

  const conta = await criarFuncionario(page, nome, suf, "Rececionista");
  await expect(page).toHaveURL(/\/funcionarios$/);

  await lista.pesquisar(nome);
  await expect(lista.linhas).toHaveCount(1);
  await expect(lista.linhas).toContainText("Rececionista");
  await expect(lista.linhas).toContainText("Ativo");

  await terminarSessao(page);
  await iniciarSessao(page, conta);
  await expect(page).toHaveURL(/\/agenda$/);
  await expect(new LayoutApp(page).entradasMenu).toHaveText([
    "Agenda",
    "Pacientes",
    "Clientes",
  ]);
});

test("ST-WEB-F001-08 — Recusar a criação de funcionário com dados inválidos", async ({
  page,
}) => {
  const suf = sufixo("F001-08");
  const nome = `Funcionário ${suf}`;
  const ficha = new PaginaFuncionarioForm(page);
  const pedidos = registarPedidos(page);
  await entrarComo.administrador(page);
  await new LayoutApp(page).irPara("Funcionários");
  await new PaginaFuncionarios(page).novoFuncionario.click();
  await ficha.nome.waitFor();

  await test.step("2. formulário vazio", async () => {
    await ficha.guardar.click();
    for (const campo of [ficha.nome, ficha.email, ficha.palavraPasse]) {
      expect(
        await campo.evaluate((e: HTMLInputElement) => e.validationMessage),
      ).not.toBe("");
    }
    expect(pedidosA(pedidos, "POST", "/utilizadores")).toHaveLength(0);
  });

  await test.step("3. palavra-passe fraca (400)", async () => {
    await ficha.preencher({
      nome,
      email: emailDe(suf),
      telefone: telefoneNovo(),
      perfil: "Rececionista",
      senha: "fraca123",
    });
    const resposta = respostaDe(page, "POST", /^\/utilizadores$/);
    await ficha.guardar.click();
    expect((await resposta).status()).toBe(400);
    await expect(ficha.erro).toBeVisible();
    await expect(page).toHaveURL(/\/funcionarios\/novo$/);
    await expect(ficha.nome).toHaveValue(nome);
    await expect(ficha.palavraPasse).toHaveValue("");
  });

  await test.step("4. e-mail já registado (409)", async () => {
    await ficha.palavraPasse.fill(senhaGerada());
    await ficha.email.fill(CONTAS.rececao.email);
    const resposta = respostaDe(page, "POST", /^\/utilizadores$/);
    await ficha.guardar.click();
    expect((await resposta).status()).toBe(409);
    await expect(ficha.erro).toBeVisible();
    await expect(page).toHaveURL(/\/funcionarios\/novo$/);
    await expect(ficha.nome).toHaveValue(nome);
    await expect(ficha.palavraPasse).toHaveValue("");
  });

  await test.step("5. nenhuma conta criada", async () => {
    const lista = new PaginaFuncionarios(page);
    await lista.abrir();
    await lista.pesquisar(nome);
    await expect(lista.listaVazia).toBeVisible();
  });
});

test("ST-WEB-F001-09 — Atualizar uma conta de funcionário e a autorização de apoio", async ({
  page,
}) => {
  const suf = sufixo("F001-09");
  const nome = `Auxiliar ${suf}`;
  const novoTelefone = telefoneNovo();
  const lista = new PaginaFuncionarios(page);
  const ficha = new PaginaFuncionarioForm(page);
  await entrarComo.administrador(page);

  await criarFuncionario(page, nome, suf, "Auxiliar Veterinário");

  await test.step("2. sem autorização na lista", async () => {
    await lista.pesquisar(nome);
    await expect(lista.linhas).toContainText("Não autorizado");
  });

  await test.step("3. alterar telefone e autorização", async () => {
    await lista.abrirFicha(nome);
    await ficha.telefone.fill(novoTelefone);
    await ficha.apoio.check();
    await ficha.guardar.click();
    await expect(ficha.aviso).toHaveText("Alterações guardadas.");
  });

  await test.step("4. lista e ficha depois de recarregar", async () => {
    await new LayoutApp(page).irPara("Funcionários");
    await lista.pesquisar(nome);
    await expect(lista.linhas).toContainText("Autorizado");
    await expect(lista.linhas).not.toContainText("Não autorizado");
    await page.reload();
    await lista.pesquisar(nome);
    await lista.abrirFicha(nome);
    await expect(ficha.telefone).toHaveValue(novoTelefone);
    await expect(ficha.apoio).toBeChecked();
  });

  await test.step("5. mudar para Veterinário", async () => {
    await ficha.perfil.selectOption({ label: "Veterinário" });
    await expect(page.getByLabel(APOIO)).toHaveCount(0);
    await ficha.guardar.click();
    await expect(ficha.aviso).toHaveText("Alterações guardadas.");
    await new LayoutApp(page).irPara("Funcionários");
    await lista.pesquisar(nome);
    await expect(lista.linhas).toContainText("Veterinário");
    await expect(lista.linhas.locator("td").nth(4)).toHaveText("—");
  });
});

test("ST-WEB-F001-10 — Mostrar a autorização de apoio só ao Auxiliar Veterinário", async ({
  page,
}) => {
  const suf = sufixo("F001-10");
  const ficha = new PaginaFuncionarioForm(page);
  await entrarComo.administrador(page);
  await new LayoutApp(page).irPara("Funcionários");
  await new PaginaFuncionarios(page).novoFuncionario.click();
  await ficha.nome.waitFor();

  for (const perfil of ["Administrador", "Rececionista", "Veterinário"]) {
    await ficha.perfil.selectOption({ label: perfil });
    await expect(ficha.apoio, perfil).toHaveCount(0);
  }
  await ficha.perfil.selectOption({ label: "Auxiliar Veterinário" });
  await expect(ficha.apoio).toBeVisible();

  await ficha.apoio.check();
  await ficha.perfil.selectOption({ label: "Rececionista" });
  await expect(ficha.apoio).toHaveCount(0);

  // O pedido EP-006 é intercetado para confirmar que não leva a autorização
  // e é cancelado antes de chegar à API: o cenário não grava nada.
  let corpo = null as Record<string, unknown> | null;
  await page.route(/\/utilizadores$/, async (rota) => {
    if (rota.request().method() !== "POST") return rota.continue();
    corpo = rota.request().postDataJSON() as Record<string, unknown>;
    await rota.abort();
  });
  await ficha.nome.fill(`Funcionário ${suf}`);
  await ficha.email.fill(emailDe(suf));
  await ficha.palavraPasse.fill(senhaGerada());
  await ficha.guardar.click();
  await expect.poll(() => corpo).not.toBeNull();
  // O frontend só envia a autorização para o Auxiliar Veterinário.
  expect(corpo).toMatchObject({ perfil: "Rececionista" });
  expect(corpo?.podeRegistarApoio ?? false).toBe(false);
});

test("ST-WEB-F001-11 — Bloquear uma conta de funcionário", async ({ page }) => {
  const suf = sufixo("F001-11");
  const nome = `Bloquear ${suf}`;
  const motivo = `Teste ${suf}`;
  const lista = new PaginaFuncionarios(page);
  const ficha = new PaginaFuncionarioForm(page);
  await entrarComo.administrador(page);
  const conta = await criarFuncionario(page, nome, suf, "Rececionista");

  await test.step("2-3. bloquear com motivo", async () => {
    await lista.pesquisar(nome);
    await lista.bloquear(nome, motivo);
    await expect(lista.aviso).toHaveText(`A conta de ${nome} foi bloqueada.`);
    await expect(lista.linha(nome)).toContainText("Bloqueado");
    await expect(
      lista.linha(nome).getByRole("button", { name: "Bloquear" }),
    ).toHaveCount(0);
  });

  await test.step("4. ficha da conta bloqueada", async () => {
    await lista.abrirFicha(nome);
    await expect(ficha.detalheConta).toContainText("Estado: Bloqueado");
    await expect(ficha.detalheConta).toContainText(
      `Motivo do bloqueio: ${motivo}`,
    );
  });

  await test.step("5. a conta bloqueada não entra", async () => {
    await terminarSessao(page);
    const login = new PaginaLogin(page);
    await login.preencherEEntrar(conta.email, conta.senha);
    await expect(login.erro).toHaveText(MENSAGEM_BLOQUEADA);
    await expect(page).toHaveURL(/\/login$/);
  });
});

test("ST-WEB-F001-12 — Recusar o bloqueio da própria conta do Administrador", async ({
  page,
}) => {
  const lista = new PaginaFuncionarios(page);
  await entrarComo.administrador(page);
  await new LayoutApp(page).irPara("Funcionários");
  await lista.esperarLista();
  await lista.pesquisar(CONTAS.admin.email);

  const resposta = respostaDe(page, "PATCH", /^\/utilizadores\/\d+\/estado$/);
  await lista.bloquear(CONTAS.admin.email, "");
  expect((await resposta).status()).toBe(422);
  await expect(lista.confirmacao.getByRole("alert")).toBeVisible();
  await expect(lista.aviso).toHaveCount(0);

  await page.reload();
  await lista.esperarLista();
  await lista.pesquisar(CONTAS.admin.email);
  await expect(lista.linha(CONTAS.admin.email)).toContainText("Ativo");
  await expect(new LayoutApp(page).cabecalho).toContainText(CONTAS.admin.nome);
});

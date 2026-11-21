import { expect, test } from "@playwright/test";
import { CONTAS } from "./apoio/contas";
import {
  emailDe,
  nifValido,
  senhaGerada,
  sufixo,
  telefoneNovo,
} from "./apoio/dados";
import { pedidosA, registarPedidos, respostaDe } from "./apoio/pedidos";
import { criarClienteDeTeste } from "./apoio/procedimentos";
import { entrarComo, terminarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaClienteForm } from "./paginas/PaginaClienteForm";
import { PaginaClientes } from "./paginas/PaginaClientes";

// I128, secção 7 — F001: Clientes e contas de acesso (W04, W05). Automação: I129.

test("ST-WEB-F001-13 — Registar um Cliente com conta de acesso (Rececionista)", async ({
  page,
}) => {
  const suf = sufixo("F001-13");
  const dados = {
    nome: `Cliente ${suf}`,
    telefone: telefoneNovo(),
    email: emailDe(suf),
    senha: senhaGerada(),
  };
  const ficha = new PaginaClienteForm(page);
  const lista = new PaginaClientes(page);
  await entrarComo.rececionista(page);
  await new LayoutApp(page).irPara("Clientes");
  await lista.novoCliente.click();

  await expect(ficha.dadosCliente).toBeVisible();
  await expect(ficha.contaAcesso).toBeVisible();
  await expect(
    ficha.contaAcesso.getByLabel("Palavra-passe inicial"),
  ).toBeVisible();

  await ficha.preencher(dados);
  await ficha.registar.click();
  await expect(page).toHaveURL(/\/clientes\/\d+$/);
  await expect(
    page.getByRole("heading", { name: "Ficha de cliente" }),
  ).toBeVisible();
  await expect(ficha.aviso).toHaveText(
    `Cliente ${dados.nome} registado, com conta de acesso para ${dados.email}.`,
  );
  await expect(ficha.estadoConta).toHaveText("Estado da conta: Ativo");
  await expect(ficha.semAnimais).toBeVisible();
  await expect(ficha.palavraPasse).toHaveCount(0);

  await ficha.voltarLista.click();
  await lista.pesquisar(dados.nome);
  await expect(lista.linhas).toHaveCount(1);
  await expect(lista.linhas.locator("td")).toHaveText([
    dados.nome,
    "—",
    dados.telefone,
    dados.email,
    "0",
    "Ver / editar",
  ]);
});

test("ST-WEB-F001-14 — Registar um Cliente com NIF e morada (Administrador)", async ({
  page,
}) => {
  const suf = sufixo("F001-14");
  const dados = {
    nome: `Cliente ${suf}`,
    telefone: telefoneNovo(),
    email: emailDe(suf),
    senha: senhaGerada(),
    nif: nifValido(),
    morada: `Rua ${suf}, 1, 4470-000 Maia`,
  };
  const ficha = new PaginaClienteForm(page);
  await entrarComo.administrador(page);
  await new LayoutApp(page).irPara("Clientes");
  await new PaginaClientes(page).novoCliente.click();

  await ficha.preencher(dados);
  await ficha.registar.click();
  await expect(ficha.aviso).toHaveText(
    `Cliente ${dados.nome} registado, com conta de acesso para ${dados.email}.`,
  );
  await expect(ficha.nif).toHaveValue(dados.nif);
  await expect(ficha.morada).toHaveValue(dados.morada);
  await expect(ficha.semAnimais).toBeVisible();
  await expect(ficha.verPaciente).toHaveCount(0);
});

test("ST-WEB-F001-15 — Recusar o registo de um Cliente com dados inválidos", async ({
  page,
}) => {
  const suf = sufixo("F001-15");
  const validos = {
    nome: `Cliente ${suf}`,
    telefone: telefoneNovo(),
    email: emailDe(suf),
    senha: senhaGerada(),
  };
  const ficha = new PaginaClienteForm(page);
  const lista = new PaginaClientes(page);
  const pedidos = registarPedidos(page);
  await entrarComo.rececionista(page);
  await new LayoutApp(page).irPara("Clientes");
  await lista.novoCliente.click();
  await ficha.nome.waitFor();

  await test.step("2. formulário vazio", async () => {
    await ficha.registar.click();
    await expect(ficha.nome).toHaveAccessibleDescription(
      "O nome é obrigatório.",
    );
    await expect(ficha.telefone).toHaveAccessibleDescription(
      "O telefone é obrigatório.",
    );
    await expect(ficha.email).toHaveAccessibleDescription(
      "O e-mail é obrigatório.",
    );
    await expect(ficha.palavraPasse).toHaveAccessibleDescription(
      "A palavra-passe inicial é obrigatória.",
    );
  });

  await test.step("3. NIF inválido", async () => {
    await ficha.preencher({ ...validos, nif: "123456788" });
    await ficha.registar.click();
    await expect(ficha.nif).toHaveAccessibleDescription(
      "O NIF é inválido: deve ter 9 dígitos e um dígito de controlo correto.",
    );
    await ficha.nif.fill("");
  });

  await test.step("4. telefone inválido", async () => {
    await ficha.telefone.fill("12");
    await ficha.registar.click();
    await expect(ficha.telefone).toHaveAccessibleDescription(
      "O telefone deve ter entre 9 e 20 caracteres: dígitos, espaços e, opcionalmente, o indicativo + no início.",
    );
    await ficha.telefone.fill(validos.telefone);
  });

  await test.step("5. palavra-passe fraca", async () => {
    await ficha.palavraPasse.fill("fraca123");
    await ficha.registar.click();
    await expect(ficha.palavraPasse).toHaveAccessibleDescription(
      "A palavra-passe deve ter entre 8 e 128 caracteres, com pelo menos uma letra minúscula, uma letra maiúscula, um algarismo e um carácter de pontuação ou símbolo.",
    );
    await ficha.palavraPasse.fill(validos.senha);
    expect(pedidosA(pedidos, "POST", "/clientes")).toHaveLength(0);
  });

  await test.step("6. e-mail já registado (409)", async () => {
    await ficha.email.fill(CONTAS.clienteJoao.email);
    const resposta = respostaDe(page, "POST", /^\/clientes$/);
    await ficha.registar.click();
    expect((await resposta).status()).toBe(409);
    await expect(ficha.erro).toBeVisible();
    await expect(ficha.palavraPasse).toHaveValue("");
  });

  await test.step("7. nenhum Cliente criado", async () => {
    await lista.abrir();
    await lista.pesquisar(validos.nome);
    await expect(lista.semResultados).toBeVisible();
  });
});

test("ST-WEB-F001-16 — Atualizar os dados de um Cliente", async ({ page }) => {
  const suf = sufixo("F001-16");
  const cliente = await criarClienteDeTeste(page, suf);
  const novoTelefone = telefoneNovo();
  const novaMorada = `Rua ${suf}, 2, 4470-000 Maia`;
  const ficha = new PaginaClienteForm(page);
  const lista = new PaginaClientes(page);

  await ficha.telefone.fill(novoTelefone);
  await ficha.morada.fill(novaMorada);
  await ficha.guardarAlteracoes.click();
  await expect(ficha.aviso).toHaveText("Alterações guardadas.");
  await expect(
    ficha.contaAcesso.getByLabel("Palavra-passe inicial"),
  ).toHaveCount(0);

  await page.reload();
  await expect(ficha.telefone).toHaveValue(novoTelefone);
  await expect(ficha.morada).toHaveValue(novaMorada);

  await ficha.voltarLista.click();
  await lista.pesquisar(cliente.nome);
  await expect(lista.linha(cliente.nome)).toContainText(novoTelefone);
});

test("ST-WEB-F001-17 — Recusar a atualização de um Cliente com dados inválidos", async ({
  page,
}) => {
  const suf = sufixo("F001-17");
  const cliente = await criarClienteDeTeste(page, suf);
  await terminarSessao(page);
  await entrarComo.administrador(page);

  const ficha = new PaginaClienteForm(page);
  const pedidos = registarPedidos(page);
  await ficha.abrirFicha(cliente.id);
  await expect(ficha.nome).toHaveValue(cliente.nome);

  await test.step("1. nome vazio", async () => {
    await ficha.nome.fill("");
    await ficha.guardarAlteracoes.click();
    await expect(ficha.nome).toHaveAccessibleDescription(
      "O nome é obrigatório.",
    );
    expect(pedidosA(pedidos, "PUT", `/clientes/${cliente.id}`)).toHaveLength(0);
  });

  await test.step("2. e-mail já registado (409)", async () => {
    await ficha.nome.fill(cliente.nome);
    await ficha.email.fill("cliente.maria@clinica-g08.test");
    const resposta = respostaDe(page, "PUT", /^\/clientes\/\d+$/);
    await ficha.guardarAlteracoes.click();
    expect((await resposta).status()).toBe(409);
    await expect(ficha.erro).toBeVisible();
  });

  await test.step("3. nada foi gravado", async () => {
    await page.reload();
    await expect(ficha.nome).toHaveValue(cliente.nome);
    await expect(ficha.email).toHaveValue(cliente.email);
  });
});

test("ST-WEB-F001-18 — Pesquisar Clientes", async ({ page }) => {
  const suf = sufixo("F001-18");
  const lista = new PaginaClientes(page);
  await entrarComo.rececionista(page);
  await new LayoutApp(page).irPara("Clientes");
  await lista.esperarLista();

  await lista.pesquisar("joão");
  await expect(lista.linhas).toHaveCount(1);
  await expect(lista.linhas.locator("td").first()).toHaveText(
    CONTAS.clienteJoao.nome,
  );
  await expect(lista.linhas.locator("td").nth(4)).toHaveText("2");

  await lista.pesquisar("960000002");
  await expect(lista.linhas).toHaveCount(1);
  await expect(lista.linhas.locator("td").first()).toHaveText(
    "Maria Silva (demonstração)",
  );

  await lista.pesquisar(`zzz${suf}`);
  await expect(lista.semResultados).toBeVisible();
});

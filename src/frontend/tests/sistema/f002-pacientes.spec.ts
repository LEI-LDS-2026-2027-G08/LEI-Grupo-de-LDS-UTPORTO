import { expect, test } from "@playwright/test";
import { CONTAS } from "./apoio/contas";
import { dataLisboa, sufixo } from "./apoio/dados";
import { linhaEmAlgumaPagina } from "./apoio/paginacao";
import { pedidosA, registarPedidos, respostaDe } from "./apoio/pedidos";
import {
  criarClienteDeTeste,
  criarPacienteDeTeste,
} from "./apoio/procedimentos";
import { entrarComo, terminarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaPaciente } from "./paginas/PaginaPaciente";
import { PaginaPacientes } from "./paginas/PaginaPacientes";

// I128, secção 8 — F002: pacientes (W07, W08). Automação: I129.

test("ST-WEB-F002-01 — Registar um paciente associado a um Cliente (Rececionista)", async ({
  page,
}) => {
  const suf = sufixo("F002-01");
  const cliente = await criarClienteDeTeste(page, suf);
  const paciente = await criarPacienteDeTeste(page, suf, cliente);
  const ficha = new PaginaPaciente(page);

  const confirmarFicha = async () => {
    await expect(ficha.titulo).toHaveText("Ficha do paciente");
    await expect(ficha.nome).toHaveValue(paciente.nome);
    await expect(ficha.especie).toHaveValue("Cão");
    await expect(ficha.raca).toHaveValue("Labrador Retriever");
    await expect(ficha.sexo).toHaveValue("Macho");
    await expect(ficha.dataNascimento).toHaveValue("2023-01-01");
    await expect(ficha.peso).toHaveValue("10.5");
    await expect(ficha.detalhe).toContainText(/Idade: \d+ anos?/);
    await expect(ficha.detalhe).toContainText(
      `Telefone do proprietário: ${cliente.telefone}`,
    );
    await expect(ficha.proprietario.escolhido).toHaveText(cliente.nome);
  };

  await confirmarFicha();
  await page.reload();
  await confirmarFicha();
});

test("ST-WEB-F002-02 — Registar um paciente a partir da pesquisa por proprietário (Veterinário)", async ({
  page,
}) => {
  const suf = sufixo("F002-02");
  const cliente = await criarClienteDeTeste(page, suf);
  await terminarSessao(page);
  await entrarComo.veterinario(page);

  const lista = new PaginaPacientes(page);
  const ficha = new PaginaPaciente(page);
  const nome = `Gato ${suf}`;
  await new LayoutApp(page).irPara("Pacientes");
  await lista.esperarLista();
  await lista.proprietario.escolher(cliente.nome, cliente.nome);

  await test.step("3. novo paciente com o proprietário já escolhido", async () => {
    await lista.novoPaciente.click();
    await expect(ficha.proprietario.escolhido).toHaveText(cliente.nome);
    await ficha.preencher({
      nome,
      especie: "Gato",
      raca: "Siamês",
      sexo: "Femea",
    });
    await ficha.guardar.click();
    await expect(ficha.aviso).toHaveText(
      `Paciente ${nome} registado para ${cliente.nome}.`,
    );
    await expect(ficha.dataNascimento).toHaveValue("");
    await expect(ficha.peso).toHaveValue("");
    await expect(ficha.areaClinica.getByRole("link")).toHaveText([
      "Registo clínico",
      "Vacinação",
    ]);
  });

  await test.step("4. pesquisa pelo proprietário", async () => {
    await lista.abrir();
    await lista.proprietario.escolher(cliente.nome, cliente.nome);
    await lista.esperarLista();
    await expect(lista.linhas).toHaveCount(1);
    await expect(lista.linhas).toContainText(nome);
  });
});

test("ST-WEB-F002-03 — Recusar o registo de um paciente com dados inválidos", async ({
  page,
}) => {
  const suf = sufixo("F002-03");
  const cliente = await criarClienteDeTeste(page, suf);
  const lista = new PaginaPacientes(page);
  const ficha = new PaginaPaciente(page);
  const pedidos = registarPedidos(page);
  const nome = `Animal ${suf}`;
  await new LayoutApp(page).irPara("Pacientes");
  await lista.novoPaciente.click();
  await ficha.nome.waitFor();

  await test.step("1. formulário vazio", async () => {
    await ficha.guardar.click();
    await expect(
      page.getByText("Escolha o proprietário (Cliente) do paciente."),
    ).toBeVisible();
    await expect(ficha.nome).toHaveAccessibleDescription(
      "O nome é obrigatório.",
    );
    await expect(ficha.especie).toHaveAccessibleDescription(
      "A espécie é obrigatória.",
    );
  });

  await test.step("2. data futura e peso 0", async () => {
    await ficha.proprietario.escolher(cliente.nome, cliente.nome);
    await ficha.nome.fill(nome);
    await ficha.especie.fill("Cão");
    await ficha.dataNascimento.fill(dataLisboa(1));
    await ficha.guardar.click();
    await expect(ficha.dataNascimento).toHaveAccessibleDescription(
      "A data de nascimento não pode ser futura.",
    );
    await ficha.dataNascimento.fill("");
    await ficha.peso.fill("0");
    await ficha.guardar.click();
    await expect(ficha.peso).toHaveAccessibleDescription(
      "O peso deve estar entre 0,01 e 9999,99 kg.",
    );
    await ficha.peso.fill("");
    expect(pedidosA(pedidos, "POST", "/animais")).toHaveLength(0);
  });

  await test.step("3. espécie e raça inexistentes (400)", async () => {
    await ficha.especie.fill("Coelho");
    let resposta = respostaDe(page, "POST", /^\/animais$/);
    await ficha.guardar.click();
    expect((await resposta).status()).toBe(400);
    await expect(ficha.erro).toContainText("A espécie indicada não existe.");

    await ficha.especie.fill("Cão");
    await ficha.raca.fill("Poodle");
    resposta = respostaDe(page, "POST", /^\/animais$/);
    await ficha.guardar.click();
    expect((await resposta).status()).toBe(400);
    await expect(ficha.erro).toContainText(
      "A raça indicada não existe para a espécie selecionada.",
    );
    await expect(ficha.nome).toHaveValue(nome);
    await expect(ficha.nome).toBeEditable();
  });

  await test.step("4. nenhum paciente criado", async () => {
    await lista.abrir();
    await lista.pesquisar(nome);
    await expect(lista.semResultados).toBeVisible();
  });
});

test("ST-WEB-F002-04 — Consultar a ficha administrativa do paciente", async ({
  page,
}) => {
  const lista = new PaginaPacientes(page);
  const ficha = new PaginaPaciente(page);
  await entrarComo.rececionista(page);
  await new LayoutApp(page).irPara("Pacientes");
  await lista.esperarLista();
  await lista.pesquisar("Bobi");
  await lista.verFicha("Bobi").click();

  const confirmarBobi = async () => {
    await expect(ficha.nome).toHaveValue("Bobi");
    await expect(ficha.especie).toHaveValue("Cão");
    await expect(ficha.raca).toHaveValue("Labrador Retriever");
    await expect(ficha.sexo).toHaveValue("Macho");
    await expect(ficha.dataNascimento).toHaveValue("2022-03-10");
    await expect(ficha.peso).toHaveValue("28.4");
    await expect(ficha.detalhe).toContainText(/Idade: \d+ anos?/);
    await expect(ficha.proprietario.escolhido).toHaveText(
      CONTAS.clienteJoao.nome,
    );
    await expect(ficha.detalhe).toContainText(
      "Telefone do proprietário: 960000001",
    );
    await expect(ficha.verCliente).toBeVisible();
    await expect(ficha.novaMarcacao).toBeVisible();
    await expect(ficha.areaClinica).toHaveCount(0);
  };

  await confirmarBobi();
  const caminho = new URL(page.url()).pathname;
  await page.reload();
  await confirmarBobi();

  await terminarSessao(page);
  await entrarComo.veterinario(page);
  await ficha.abrir(caminho);
  await expect(ficha.nome).toHaveValue("Bobi");
  await expect(ficha.areaClinica.getByRole("link")).toHaveText([
    "Registo clínico",
    "Vacinação",
  ]);
  await expect(ficha.novaMarcacao).toHaveCount(0);
});

test("ST-WEB-F002-05 — Abrir a ficha de um paciente inexistente", async ({
  page,
}) => {
  const ficha = new PaginaPaciente(page);
  const pedidos = registarPedidos(page);
  await entrarComo.rececionista(page);

  await test.step("1. /pacientes/999999 (404 da API)", async () => {
    const resposta = respostaDe(page, "GET", /^\/animais\/999999$/);
    await page.goto("/pacientes/999999");
    expect((await resposta).status()).toBe(404);
    await expect(ficha.erro).toBeVisible();
    await expect(ficha.voltarListaPacientes).toBeVisible();
    await expect(ficha.formulario).toHaveCount(0);
  });

  await test.step("2. /pacientes/abc (endereço inválido)", async () => {
    await page.goto("/pacientes/abc");
    await expect(ficha.erro).toHaveText("O registo pedido não foi encontrado.");
    await expect(ficha.voltarListaPacientes).toBeVisible();
    expect(pedidosA(pedidos, "GET", "/animais/abc")).toHaveLength(0);
  });
});

test("ST-WEB-F002-06 — Atualizar os dados administrativos do paciente", async ({
  page,
}) => {
  const suf = sufixo("F002-06");
  const cliente = await criarClienteDeTeste(page, suf);
  const paciente = await criarPacienteDeTeste(page, suf, cliente);
  await terminarSessao(page);
  await entrarComo.veterinario(page);

  const ficha = new PaginaPaciente(page);
  await ficha.abrir(paciente.caminho);
  await ficha.peso.fill("12.3");
  await ficha.raca.fill("Pastor Alemão");
  await ficha.guardar.click();
  await expect(ficha.aviso).toHaveText("Alterações guardadas.");
  await expect(ficha.detalhe).toContainText("Última atualização:");
  await expect(ficha.detalhe).not.toContainText("Última atualização: —");

  await page.reload();
  await expect(ficha.peso).toHaveValue("12.3");
  await expect(ficha.raca).toHaveValue("Pastor Alemão");

  const lista = new PaginaPacientes(page);
  await lista.abrir();
  await lista.pesquisar(paciente.nome);
  await expect(lista.linha(paciente.nome)).toContainText("12,3 kg");
  await expect(lista.linha(paciente.nome)).toContainText("Pastor Alemão");
});

test("ST-WEB-F002-07 — Recusar a atualização do paciente com dados inválidos", async ({
  page,
}) => {
  const suf = sufixo("F002-07");
  const cliente = await criarClienteDeTeste(page, suf);
  const paciente = await criarPacienteDeTeste(page, suf, cliente);
  const ficha = new PaginaPaciente(page);
  const pedidos = registarPedidos(page);

  await ficha.nome.fill("");
  await ficha.guardar.click();
  await expect(ficha.nome).toHaveAccessibleDescription("O nome é obrigatório.");

  await ficha.nome.fill(paciente.nome);
  await ficha.peso.fill("10000");
  await ficha.guardar.click();
  await expect(ficha.peso).toHaveAccessibleDescription(
    "O peso deve estar entre 0,01 e 9999,99 kg.",
  );
  expect(pedidosA(pedidos, "PUT", /^\/animais\/\d+$/)).toHaveLength(0);

  await page.reload();
  await expect(ficha.nome).toHaveValue(paciente.nome);
  await expect(ficha.peso).toHaveValue("10.5");
});

test("ST-WEB-F002-08 — Pesquisar os pacientes de um proprietário", async ({
  page,
}) => {
  const lista = new PaginaPacientes(page);
  await entrarComo.rececionista(page);
  await new LayoutApp(page).irPara("Pacientes");
  await lista.esperarLista();

  await lista.proprietario.escolher("João", CONTAS.clienteJoao.nome);
  await lista.esperarLista();
  await expect(lista.linhas.locator("td:first-child")).toHaveText([
    "Bobi",
    "Mia",
  ]);
  await expect(lista.paginacao).toContainText("2 pacientes");
  for (const linha of await lista.linhas.all()) {
    await expect(linha.locator("td")).toHaveCount(7);
    await expect(linha).toContainText(CONTAS.clienteJoao.nome);
  }

  await lista.proprietario.limpar("Remover filtro").click();
  await lista.esperarLista();
  await expect(lista.paginacao).not.toContainText("· 2 pacientes");
  for (const nome of ["Kiko", "Luna"]) {
    await expect(
      await linhaEmAlgumaPagina(page, lista.linhas, nome),
      nome,
    ).toHaveCount(1);
  }
});

test("ST-WEB-F002-09 — Pesquisa por proprietário sem resultados", async ({
  page,
}) => {
  const suf = sufixo("F002-09");
  const cliente = await criarClienteDeTeste(page, suf);
  await terminarSessao(page);
  await entrarComo.veterinario(page);

  const lista = new PaginaPacientes(page);
  await new LayoutApp(page).irPara("Pacientes");
  await lista.esperarLista();
  const antes = await lista.linhas.count();

  await lista.proprietario.pesquisar(`zzz${suf}`);
  await expect(lista.proprietario.semResultados).toBeVisible();
  await expect(lista.linhas).toHaveCount(antes);

  await lista.proprietario.escolher(cliente.nome, cliente.nome);
  await lista.esperarLista();
  await expect(lista.semResultados).toBeVisible();
  await expect(lista.linhas).toHaveCount(0);
});

import { expect, test } from "@playwright/test";
import { CONTAS } from "./apoio/contas";
import { emailDe, senhaGerada, sufixo, telefoneNovo } from "./apoio/dados";
import { respostaDe } from "./apoio/pedidos";
import {
  MENSAGEM_SEM_LIGACAO,
  cortarLigacao,
  reporLigacao,
  reterPedidos,
} from "./apoio/rede";
import { iniciarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaClienteForm } from "./paginas/PaginaClienteForm";
import { PaginaClientes } from "./paginas/PaginaClientes";
import { PaginaClinica } from "./paginas/PaginaClinica";
import { PaginaFuncionarios } from "./paginas/PaginaFuncionarios";

// I128, secção 12 — backend indisponível, erros da API e estados de
// carregamento. Automação: I132.

/** Sinais de detalhes técnicos que nunca devem chegar ao utilizador. */
const DETALHE_TECNICO =
  /[{}<>]|Exception|stack|System\.|Microsoft\.|\bat \w+\./i;

test("ST-WEB-TRV-07 — Backend indisponível ao abrir uma página", async ({
  page,
}) => {
  const layout = new LayoutApp(page);
  const clinica = new PaginaClinica(page);
  const aCarregar = page.getByRole("status").filter({ hasText: "A carregar…" });
  const tentarNovamente = page.getByRole("button", {
    name: "Tentar novamente",
  });
  const cabecalho = `${CONTAS.admin.nome} · ${CONTAS.admin.perfil}`;

  await iniciarSessao(page, CONTAS.admin);
  await cortarLigacao(page);

  await test.step("«Clínica e horários» sem ligação", async () => {
    await layout.irPara("Clínica e horários");
    await expect(page.getByRole("alert")).toHaveText(MENSAGEM_SEM_LIGACAO);
    await expect(tentarNovamente).toBeVisible();
    await expect(aCarregar).toHaveCount(0);
    await expect(layout.cabecalho).toContainText(cabecalho);
  });

  await test.step("«Funcionários» sem ligação", async () => {
    await layout.irPara("Funcionários");
    await expect(page.getByRole("alert")).toHaveText(MENSAGEM_SEM_LIGACAO);
    await expect(aCarregar).toHaveCount(0);
    await expect(layout.cabecalho).toContainText(cabecalho);
  });

  await test.step("repor a ligação e «Tentar novamente»", async () => {
    await layout.irPara("Clínica e horários");
    await expect(tentarNovamente).toBeVisible();
    await reporLigacao(page);
    await tentarNovamente.click();
    await expect(clinica.nome).toBeVisible();
    await expect(clinica.nome).not.toHaveValue("");
    await expect(page.getByRole("alert")).toHaveCount(0);
    await expect(page).toHaveURL(/\/clinica$/);
  });
});

test("ST-WEB-TRV-08 — Backend indisponível ao guardar um formulário", async ({
  page,
}) => {
  const suf = sufixo("TRV-08");
  const ficha = new PaginaClienteForm(page);
  const cliente = {
    nome: `Cliente ${suf}`,
    telefone: telefoneNovo(),
    email: emailDe(suf),
  };

  await iniciarSessao(page, CONTAS.rececao);
  await new LayoutApp(page).irPara("Clientes");
  await new PaginaClientes(page).novoCliente.click();
  await ficha.preencher({ ...cliente, senha: senhaGerada() });

  await test.step("«Registar cliente» sem ligação", async () => {
    await cortarLigacao(page);
    await ficha.registar.click();
    await expect(ficha.erro).toHaveText(MENSAGEM_SEM_LIGACAO);
    await expect(ficha.nome).toHaveValue(cliente.nome);
    await expect(ficha.telefone).toHaveValue(cliente.telefone);
    await expect(ficha.email).toHaveValue(cliente.email);
    // A palavra-passe é limpa por segurança depois de cada envio.
    await expect(ficha.palavraPasse).toHaveValue("");
    await expect(ficha.registar).toBeEnabled();
    await expect(page).toHaveURL(/\/clientes\/novo$/);
  });

  await test.step("repor a ligação e registar de novo", async () => {
    await reporLigacao(page);
    await ficha.palavraPasse.fill(senhaGerada());
    await ficha.registar.click();
    await expect(ficha.aviso).toHaveText(
      `Cliente ${cliente.nome} registado, com conta de acesso para ${cliente.email}.`,
    );
    await expect(page).toHaveURL(/\/clientes\/\d+$/);
  });
});

test("ST-WEB-TRV-09 — Mostrar os erros da API sem detalhes técnicos", async ({
  page,
}) => {
  const casos = [
    {
      rota: "/clientes/999999",
      api: /^\/clientes\/999999$/,
      regresso: { nome: "Voltar à lista", destino: "/clientes" },
    },
    {
      rota: "/pacientes/999999",
      api: /^\/animais\/999999$/,
      regresso: { nome: "Voltar à lista de pacientes", destino: "/pacientes" },
    },
    {
      rota: "/agenda/999999",
      api: /^\/consultas\/999999$/,
      regresso: { nome: "Voltar à agenda", destino: "/agenda" },
    },
  ];

  await iniciarSessao(page, CONTAS.rececao);
  for (const { rota, api, regresso } of casos) {
    await test.step(rota, async () => {
      const resposta = respostaDe(page, "GET", api);
      await page.goto(rota);
      const recebida = await resposta;
      expect(recebida.status()).toBe(404);

      // O frontend mostra o `detail` da API; sem ele, o `message` ou a
      // mensagem por omissão (ApiClient.ts).
      const corpo = (await recebida.json().catch(() => null)) as {
        message?: string;
        detail?: string | null;
      } | null;
      const esperado =
        corpo?.detail?.trim() ||
        corpo?.message ||
        "O registo pedido não foi encontrado.";

      const conteudo = page.locator("main");
      const erro = conteudo.getByRole("alert");
      await expect(erro).toHaveText(esperado);
      expect(await erro.innerText()).not.toMatch(DETALHE_TECNICO);
      expect(await conteudo.innerText()).not.toMatch(DETALHE_TECNICO);
      await expect(
        conteudo.getByRole("link", { name: regresso.nome, exact: true }),
      ).toHaveAttribute("href", regresso.destino);
    });
  }
});

test("ST-WEB-TRV-10 — Estados de carregamento e botões inativos durante os pedidos", async ({
  page,
}) => {
  const layout = new LayoutApp(page);
  const funcionarios = new PaginaFuncionarios(page);
  const clinica = new PaginaClinica(page);
  const aCarregar = page
    .locator("main")
    .getByRole("status")
    .filter({ hasText: "A carregar…" });

  await iniciarSessao(page, CONTAS.admin);

  await test.step("«Funcionários» enquanto espera pela lista", async () => {
    const lista = await reterPedidos(page, "GET", /^\/utilizadores$/);
    await layout.irPara("Funcionários");
    await lista.primeiro;
    await expect(aCarregar).toBeVisible();
    await lista.libertar();
    await funcionarios.esperarLista();
    await expect(funcionarios.linhas.first()).toBeVisible();
  });

  await test.step("«Clínica e horários» enquanto espera pelos dados", async () => {
    const dados = await reterPedidos(page, "GET", /^\/clinica$/);
    await layout.irPara("Clínica e horários");
    await dados.primeiro;
    await expect(aCarregar).toBeVisible();
    await dados.libertar();
    await clinica.esperarFormulario();
  });

  await test.step("«Guardar» sem alterações enquanto espera pela resposta", async () => {
    const gravacao = await reterPedidos(page, "PUT", /^\/clinica$/);
    await clinica.guardar.click();
    await gravacao.primeiro;
    await expect(
      page.getByRole("button", { name: "A guardar…", exact: true }),
    ).toBeDisabled();
    await expect(clinica.cancelar).toBeDisabled();
    await gravacao.libertar();
    await expect(clinica.aviso).toHaveText("Alterações guardadas.");
    await expect(clinica.guardar).toBeEnabled();
    await expect(clinica.cancelar).toBeEnabled();
  });
});

import { expect, test } from "@playwright/test";
import { URL_API } from "./apoio/ambiente";
import { CONTAS, senhaDe } from "./apoio/contas";
import { senhaGerada } from "./apoio/dados";
import {
  sessaoGuardada,
  substituirToken,
  tokenDaSessao,
} from "./apoio/navegador";
import { respostaDe } from "./apoio/pedidos";
import { iniciarSessao, terminarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaClienteForm } from "./paginas/PaginaClienteForm";
import { PaginaClientes } from "./paginas/PaginaClientes";
import { PaginaFuncionarios } from "./paginas/PaginaFuncionarios";
import { PaginaLogin } from "./paginas/PaginaLogin";
import { PaginaPacientes } from "./paginas/PaginaPacientes";

// I128, secções 7 e 12 — fim de sessão, sessão mantida ao recarregar,
// sessão revogada ou inválida e dados sensíveis. Automação: I132.

test("ST-WEB-F001-04 — Terminar sessão e não voltar a páginas protegidas", async ({
  page,
}) => {
  const layout = new LayoutApp(page);
  const login = new PaginaLogin(page);
  const clientes = new PaginaClientes(page);

  await iniciarSessao(page, CONTAS.rececao);
  await layout.irPara("Clientes");
  await clientes.esperarLista();

  const fimDeSessao = respostaDe(page, "POST", /^\/auth\/logout$/);
  await terminarSessao(page);
  // O pedido de fim de sessão (EP-002) foi enviado e aceite pelo backend.
  expect((await fimDeSessao).ok()).toBe(true);
  await expect(layout.cabecalho).toHaveCount(0);

  await test.step("botão «Retroceder» do navegador", async () => {
    await page.goBack();
    await expect(page).toHaveURL(/\/login$/);
    await expect(login.titulo).toBeVisible();
    await expect(clientes.titulo).toHaveCount(0);
  });

  await test.step("abrir /clientes diretamente", async () => {
    await page.goto("/clientes");
    await expect(page).toHaveURL(/\/login$/);
    await expect(login.titulo).toBeVisible();
    await expect(clientes.titulo).toHaveCount(0);
  });

  await test.step("novo início de sessão", async () => {
    await iniciarSessao(page, CONTAS.rececao);
  });
});

test("ST-WEB-F001-05 — Manter a sessão ao navegar e ao recarregar", async ({
  page,
}) => {
  const layout = new LayoutApp(page);
  const login = new PaginaLogin(page);
  const pacientes = new PaginaPacientes(page);
  const cabecalho = `${CONTAS.vetAna.nome} · ${CONTAS.vetAna.perfil}`;

  await iniciarSessao(page, CONTAS.vetAna);
  for (const destino of ["Pacientes", "Agenda", "Pacientes"]) {
    await test.step(destino, async () => {
      await layout.irPara(destino);
      await expect(
        page.getByRole("heading", { name: destino, level: 1 }),
      ).toBeVisible();
      await expect(layout.cabecalho).toContainText(cabecalho);
      await expect(login.titulo).toHaveCount(0);
    });
  }

  await test.step("recarregar a página (F5)", async () => {
    const confirmacao = respostaDe(page, "GET", /^\/auth\/me$/);
    await page.reload();
    // A sessão é confirmada no backend (EP-003).
    expect((await confirmacao).status()).toBe(200);
    await expect(page).toHaveURL(/\/pacientes$/);
    await pacientes.esperarLista();
    await expect(pacientes.linhas.first()).toBeVisible();
    await expect(layout.cabecalho).toContainText(cabecalho);
    await expect(login.titulo).toHaveCount(0);
  });
});

test("ST-WEB-TRV-05 — Sessão revogada durante a utilização", async ({
  page,
}) => {
  const layout = new LayoutApp(page);
  const pacientes = new PaginaPacientes(page);

  await iniciarSessao(page, CONTAS.rececao);
  await layout.irPara("Clientes");
  await new PaginaClientes(page).esperarLista();

  await test.step("revogar o token fora do ecrã (EP-002)", async () => {
    const token = await tokenDaSessao(page);
    const revogacao = await page.request.post(`${URL_API}/auth/logout`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    expect(revogacao.ok()).toBe(true);
  });

  const lista = respostaDe(page, "GET", /^\/animais$/);
  await layout.irPara("Pacientes");
  expect((await lista).status()).toBe(401);
  await expect(page).toHaveURL(/\/login$/);
  await expect(
    page.getByText("A sua sessão terminou. Inicie sessão novamente."),
  ).toBeVisible();
  await expect(pacientes.linhas).toHaveCount(0);
  expect(await sessaoGuardada(page)).toBeNull();
});

test("ST-WEB-TRV-06 — Sessão inválida ao recarregar", async ({ page }) => {
  const funcionarios = new PaginaFuncionarios(page);
  const login = new PaginaLogin(page);

  await iniciarSessao(page, CONTAS.admin);
  await new LayoutApp(page).irPara("Funcionários");
  await funcionarios.esperarLista();

  // Token inválido no armazenamento da sessão, como um token expirado.
  await substituirToken(page, "token-invalido-trv-06");

  // A confirmação da sessão (EP-003) só segue para o backend depois de se
  // ver o estado «A verificar a sessão…».
  let deixarSeguir!: () => void;
  const podeSeguir = new Promise<void>((r) => (deixarSeguir = r));
  await page.route(`${URL_API}/auth/me`, async (route) => {
    await podeSeguir;
    await route.fallback();
  });
  const confirmacao = respostaDe(page, "GET", /^\/auth\/me$/);

  await page.reload();
  await expect(
    page.getByRole("status").filter({ hasText: "A verificar a sessão…" }),
  ).toBeVisible();
  deixarSeguir();

  expect((await confirmacao).status()).toBe(401);
  await expect(page).toHaveURL(/\/login$/);
  await expect(login.titulo).toBeVisible();
  await expect(funcionarios.titulo).toHaveCount(0);
  expect(await sessaoGuardada(page)).toBeNull();
});

test("ST-WEB-TRV-11 — Palavras-passe e tokens não ficam expostos", async ({
  page,
}) => {
  const login = new PaginaLogin(page);
  const layout = new LayoutApp(page);
  const senha = senhaDe(CONTAS.rececao);
  const senhaInicial = senhaGerada();

  const consola: string[] = [];
  page.on("console", (mensagem) => consola.push(mensagem.text()));
  const enderecos: string[] = [];
  page.on("framenavigated", (frame) => {
    if (frame === page.mainFrame()) enderecos.push(frame.url());
  });

  await test.step("iniciar sessão", async () => {
    await login.abrir();
    await expect(login.palavraPasse).toHaveAttribute("type", "password");
    await login.preencherEEntrar(CONTAS.rececao.email, senha);
    await expect(layout.terminarSessao).toBeVisible();
  });
  const token = await tokenDaSessao(page);

  await test.step("palavra-passe inicial de um Cliente novo", async () => {
    await layout.irPara("Clientes");
    await new PaginaClientes(page).novoCliente.click();
    const ficha = new PaginaClienteForm(page);
    await expect(ficha.palavraPasse).toHaveAttribute("type", "password");
    await ficha.palavraPasse.fill(senhaInicial);
    enderecos.push(page.url());
  });

  await terminarSessao(page);
  enderecos.push(page.url());

  // As verificações contam ocorrências sem as escrever no relatório, para
  // que uma falha não exponha o próprio segredo.
  const segredos = [token, senha, senhaInicial].flatMap((s) => [
    s,
    encodeURIComponent(s),
  ]);
  const temSegredo = (texto: string) => segredos.some((s) => texto.includes(s));
  expect(
    enderecos.filter(temSegredo).length,
    "endereços com o token ou uma palavra-passe",
  ).toBe(0);
  expect(
    consola.filter(temSegredo).length,
    "mensagens da consola com o token ou uma palavra-passe",
  ).toBe(0);
  expect(await sessaoGuardada(page)).toBeNull();
});

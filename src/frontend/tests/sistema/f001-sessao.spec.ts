import { expect, test } from "@playwright/test";
import { CONTAS, senhaDe } from "./apoio/contas";
import { sufixo } from "./apoio/dados";
import { MENSAGEM_BLOQUEADA, MENSAGEM_CREDENCIAIS } from "./apoio/mensagens";
import { iniciarSessao, terminarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import { PaginaLogin } from "./paginas/PaginaLogin";

// I128, secção 7 — F001: início de sessão (W01). Automação: I129.

test("ST-WEB-F001-01 — Iniciar sessão com cada perfil interno", async ({
  page,
}) => {
  const esperado = [
    {
      conta: CONTAS.admin,
      inicial: "/clientes",
      menu: ["Clientes", "Funcionários", "Clínica e horários"],
    },
    {
      conta: CONTAS.rececao,
      inicial: "/agenda",
      menu: ["Agenda", "Pacientes", "Clientes"],
    },
    { conta: CONTAS.vetAna, inicial: "/agenda", menu: ["Agenda", "Pacientes"] },
    {
      conta: CONTAS.auxCarla,
      inicial: "/preparacao",
      menu: ["Preparação de consultas"],
    },
  ];
  const layout = new LayoutApp(page);

  for (const { conta, inicial, menu } of esperado) {
    await test.step(conta.email, async () => {
      await iniciarSessao(page, conta);
      await expect(page).toHaveURL(new RegExp(`${inicial}$`));
      await expect(layout.cabecalho).toContainText(
        `${conta.nome} · ${conta.perfil}`,
      );
      await expect(layout.entradasMenu).toHaveText(menu);
      await terminarSessao(page);
    });
  }
});

test("ST-WEB-F001-02 — Recusar credenciais inválidas", async ({ page }) => {
  const suf = sufixo("F001-02");
  const senhaErrada = `Errada#${suf}`;
  const login = new PaginaLogin(page);
  const layout = new LayoutApp(page);
  await login.abrir();

  for (const email of [
    CONTAS.rececao.email,
    `naoexiste.${suf}@clinica-g08.test`.toLowerCase(),
  ]) {
    await test.step(email, async () => {
      await login.preencherEEntrar(email, senhaErrada);
      await expect(login.erro).toHaveText(MENSAGEM_CREDENCIAIS);
      await expect(page).toHaveURL(/\/login$/);
      await expect(layout.terminarSessao).toHaveCount(0);
      await expect(login.palavraPasse).toHaveValue("");
      await expect(login.palavraPasse).toBeEditable();
    });
  }

  await page.goto("/agenda");
  await expect(page).toHaveURL(/\/login$/);
  await expect(login.titulo).toBeVisible();
});

test("ST-WEB-F001-03 — Recusar o início de sessão de uma conta bloqueada", async ({
  page,
}) => {
  const login = new PaginaLogin(page);
  await login.abrir();
  await login.preencherEEntrar(
    CONTAS.rececaoBloqueada.email,
    senhaDe(CONTAS.rececaoBloqueada),
  );
  await expect(login.erro).toHaveText(MENSAGEM_BLOQUEADA);
  await expect(page).toHaveURL(/\/login$/);
  await expect(new LayoutApp(page).menu).toHaveCount(0);
});

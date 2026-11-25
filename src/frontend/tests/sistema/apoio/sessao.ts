import { expect, type Page } from "@playwright/test";
import { LayoutApp } from "../paginas/LayoutApp";
import { PaginaLogin } from "../paginas/PaginaLogin";
import { CONTAS, senhaDe, type Conta } from "./contas";

/** Credenciais de uma conta criada pelo próprio cenário. */
export interface ContaCriada {
  email: string;
  senha: string;
  nome: string;
  perfil: string;
}

/**
 * PC-01 — Iniciar sessão como <conta>. Espera que o cabeçalho mostre o nome
 * e o perfil e que «Terminar sessão» esteja visível.
 */
export async function iniciarSessao(
  page: Page,
  conta: Conta | ContaCriada,
): Promise<void> {
  const senha = "senha" in conta ? conta.senha : senhaDe(conta);
  const login = new PaginaLogin(page);
  await login.abrir();
  await login.preencherEEntrar(conta.email, senha);
  const layout = new LayoutApp(page);
  await expect(layout.cabecalho).toContainText(
    `${conta.nome} · ${conta.perfil}`,
  );
  await expect(layout.terminarSessao).toBeVisible();
}

/** PC-02 — Terminar sessão e esperar pelo ecrã «Iniciar sessão». */
export async function terminarSessao(page: Page): Promise<void> {
  await new LayoutApp(page).terminarSessao.click();
  await expect(new PaginaLogin(page).titulo).toBeVisible();
}

/** Ajudante de autenticação por perfil (critério 4 da I129). */
export const entrarComo = {
  administrador: (page: Page) => iniciarSessao(page, CONTAS.admin),
  rececionista: (page: Page) => iniciarSessao(page, CONTAS.rececao),
  veterinario: (page: Page) => iniciarSessao(page, CONTAS.vetAna),
  auxiliar: (page: Page) => iniciarSessao(page, CONTAS.auxCarla),
};

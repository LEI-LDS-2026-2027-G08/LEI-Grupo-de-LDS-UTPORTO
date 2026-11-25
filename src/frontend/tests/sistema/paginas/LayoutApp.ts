import type { Locator, Page } from "@playwright/test";

/** Estrutura comum às páginas autenticadas: cabeçalho e menu por perfil. */
export class LayoutApp {
  readonly cabecalho: Locator;
  readonly menu: Locator;
  readonly terminarSessao: Locator;

  constructor(page: Page) {
    this.cabecalho = page.locator(".app-utilizador");
    this.menu = page.getByRole("navigation", { name: "Menu principal" });
    this.terminarSessao = page.getByRole("button", { name: "Terminar sessão" });
  }

  /** Entradas do menu lateral, pela ordem em que aparecem. */
  get entradasMenu(): Locator {
    return this.menu.getByRole("link");
  }

  async irPara(rotulo: string): Promise<void> {
    await this.menu.getByRole("link", { name: rotulo, exact: true }).click();
  }
}

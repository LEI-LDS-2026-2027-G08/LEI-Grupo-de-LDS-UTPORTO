import type { Locator, Page } from "@playwright/test";

/** W01 — Início de sessão. */
export class PaginaLogin {
  readonly titulo: Locator;
  readonly email: Locator;
  readonly palavraPasse: Locator;
  readonly entrar: Locator;
  /** Faixa de erro do formulário (FormErro). */
  readonly erro: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", { name: "Iniciar sessão" });
    this.email = page.getByLabel("Email", { exact: true });
    this.palavraPasse = page.getByLabel("Palavra-passe", { exact: true });
    this.entrar = page.getByRole("button", { name: "Entrar" });
    this.erro = page.getByRole("alert");
  }

  /**
   * PC-01, passo 1: abre /login numa navegação nova. Passar primeiro por
   * about:blank garante que não fica o estado de navegação da sessão
   * anterior (o «de» que o ProtectedRoute guarda ao redirecionar).
   */
  async abrir(): Promise<void> {
    await this.page.goto("about:blank");
    await this.page.goto("/login");
    await this.email.waitFor();
  }

  async preencherEEntrar(email: string, senha: string): Promise<void> {
    await this.email.fill(email);
    await this.palavraPasse.fill(senha);
    await this.entrar.click();
  }
}

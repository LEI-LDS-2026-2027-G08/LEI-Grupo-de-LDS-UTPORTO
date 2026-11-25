import type { Locator, Page } from "@playwright/test";

/** W04 — Lista e pesquisa de Clientes. */
export class PaginaClientes {
  readonly titulo: Locator;
  readonly novoCliente: Locator;
  readonly pesquisa: Locator;
  readonly botaoPesquisar: Locator;
  readonly linhas: Locator;
  readonly semResultados: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", { name: "Clientes", level: 1 });
    this.novoCliente = page.getByRole("link", { name: "Novo cliente" });
    this.pesquisa = page.getByLabel(
      "Pesquisar (nome, NIF, telefone ou e-mail)",
    );
    this.botaoPesquisar = page.getByRole("button", { name: "Pesquisar" });
    this.linhas = page.locator("table.tabela tbody tr");
    this.semResultados = page.getByText(
      "Nenhum cliente corresponde à pesquisa.",
    );
  }

  async abrir(): Promise<void> {
    await this.page.goto("/clientes");
    await this.esperarLista();
  }

  async esperarLista(): Promise<void> {
    await this.titulo.waitFor();
    await this.page
      .getByRole("status")
      .filter({ hasText: "A carregar…" })
      .waitFor({ state: "detached" });
  }

  async pesquisar(texto: string): Promise<void> {
    await this.pesquisa.fill(texto);
    await this.botaoPesquisar.click();
    await this.esperarLista();
  }

  linha(texto: string): Locator {
    return this.linhas.filter({ hasText: texto });
  }
}

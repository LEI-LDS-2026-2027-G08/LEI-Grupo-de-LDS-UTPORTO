import type { Locator, Page } from "@playwright/test";

/** Pesquisa e escolha do proprietário, em W07 e W08. */
export class SeletorProprietario {
  readonly campo: Locator;
  readonly procurar: Locator;
  readonly semResultados: Locator;
  /** Nome do proprietário escolhido (em negrito). */
  readonly escolhido: Locator;

  constructor(
    private readonly page: Page,
    contexto: Locator | Page = page,
  ) {
    this.campo = contexto.getByLabel(
      "Proprietário (nome, NIF, telefone ou e-mail)",
    );
    this.procurar = contexto.getByRole("button", {
      name: "Procurar proprietário",
    });
    this.semResultados = contexto.getByText("Nenhum cliente encontrado.");
    this.escolhido = contexto
      .locator(".campo")
      .filter({ hasText: /^Proprietário/ })
      .locator("strong");
  }

  async pesquisar(texto: string): Promise<void> {
    await this.campo.fill(texto);
    await this.procurar.click();
    await this.page
      .getByRole("button", { name: "A procurar…" })
      .waitFor({ state: "detached" });
  }

  /** Pesquisa e carrega em «Selecionar <nome>». */
  async escolher(texto: string, nome: string): Promise<void> {
    await this.pesquisar(texto);
    await this.page
      .getByRole("button", { name: `Selecionar ${nome}`, exact: true })
      .click();
  }

  /** Botão que limpa a escolha («Remover filtro» na lista, «Alterar» na ficha). */
  limpar(rotulo: string): Locator {
    return this.page.getByRole("button", { name: rotulo, exact: true });
  }
}

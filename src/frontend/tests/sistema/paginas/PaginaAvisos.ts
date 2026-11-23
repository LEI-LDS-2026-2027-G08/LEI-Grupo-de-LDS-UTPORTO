import type { Locator, Page } from "@playwright/test";

/** «Acesso negado» (AcessoNegadoPage.tsx), mostrado em /acesso-negado. */
export class PaginaAcessoNegado {
  readonly titulo: Locator;
  readonly explicacao: Locator;
  readonly voltarInicio: Locator;

  constructor(page: Page) {
    this.titulo = page.getByRole("heading", {
      name: "Acesso negado",
      level: 1,
    });
    this.explicacao = page.getByText(
      "O seu perfil não tem permissão para ver esta página.",
    );
    this.voltarInicio = page.getByRole("link", { name: "Voltar ao início" });
  }
}

/** «Página não encontrada» (NaoEncontradaPage.tsx). */
export class PaginaNaoEncontrada {
  readonly titulo: Locator;
  readonly explicacao: Locator;
  readonly voltarInicio: Locator;

  constructor(page: Page) {
    this.titulo = page.getByRole("heading", {
      name: "Página não encontrada",
      level: 1,
    });
    this.explicacao = page.getByText("O endereço que abriu não existe.");
    this.voltarInicio = page.getByRole("link", { name: "Voltar ao início" });
  }
}

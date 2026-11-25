import type { Locator, Page } from "@playwright/test";
import { SeletorProprietario } from "./SeletorProprietario";

/** W07 — Lista e pesquisa de pacientes. */
export class PaginaPacientes {
  readonly titulo: Locator;
  readonly novoPaciente: Locator;
  readonly proprietario: SeletorProprietario;
  readonly pesquisa: Locator;
  readonly botaoPesquisar: Locator;
  readonly linhas: Locator;
  readonly paginacao: Locator;
  readonly semResultados: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", { name: "Pacientes", level: 1 });
    this.novoPaciente = page.getByRole("link", { name: "Novo paciente" });
    this.proprietario = new SeletorProprietario(page);
    this.pesquisa = page.getByLabel(
      "Pesquisar (nome do animal ou do proprietário)",
    );
    this.botaoPesquisar = page.getByRole("button", {
      name: "Pesquisar",
      exact: true,
    });
    this.linhas = page.locator("table.tabela tbody tr");
    this.paginacao = page.getByRole("navigation", { name: "Páginas" });
    this.semResultados = page.getByText(
      "Nenhum paciente corresponde à pesquisa.",
    );
  }

  async abrir(): Promise<void> {
    await this.page.goto("/pacientes");
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

  verFicha(nome: string): Locator {
    return this.page.getByRole("link", {
      name: `Ver ficha de ${nome}`,
      exact: true,
    });
  }
}

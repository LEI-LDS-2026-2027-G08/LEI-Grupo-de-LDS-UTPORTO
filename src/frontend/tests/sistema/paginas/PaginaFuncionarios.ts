import type { Locator, Page } from "@playwright/test";

/** W02 — Lista de funcionários. */
export class PaginaFuncionarios {
  readonly titulo: Locator;
  readonly novoFuncionario: Locator;
  readonly pesquisa: Locator;
  readonly perfil: Locator;
  readonly estado: Locator;
  readonly botaoPesquisar: Locator;
  /** Mensagem de sucesso (ex.: «Conta de … criada.»). */
  readonly aviso: Locator;
  readonly tabela: Locator;
  readonly linhas: Locator;
  readonly listaVazia: Locator;
  /** Janela «Confirmar bloqueio». */
  readonly confirmacao: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", { name: "Funcionários", level: 1 });
    this.novoFuncionario = page.getByRole("link", { name: "Novo funcionário" });
    this.pesquisa = page.getByLabel("Pesquisar (nome ou e-mail)");
    this.perfil = page.getByLabel("Perfil", { exact: true });
    this.estado = page.getByLabel("Estado", { exact: true });
    this.botaoPesquisar = page.getByRole("button", { name: "Pesquisar" });
    this.aviso = page.locator("p.sucesso");
    this.tabela = page.locator("table.tabela");
    this.linhas = this.tabela.locator("tbody tr");
    this.listaVazia = page.getByText("Não há funcionários com estes filtros.");
    this.confirmacao = page.getByRole("form", { name: "Confirmar bloqueio" });
  }

  async abrir(): Promise<void> {
    await this.page.goto("/funcionarios");
    await this.esperarLista();
  }

  /** Espera que a lista deixe de mostrar «A carregar…». */
  async esperarLista(): Promise<void> {
    await this.titulo.waitFor();
    await this.page
      .getByRole("status")
      .filter({ hasText: "A carregar…" })
      .waitFor({ state: "detached" });
  }

  async pesquisar(
    texto: string,
    perfil = "Todos",
    estado = "Todos",
  ): Promise<void> {
    await this.pesquisa.fill(texto);
    await this.perfil.selectOption({ label: perfil });
    await this.estado.selectOption({ label: estado });
    await this.botaoPesquisar.click();
    await this.esperarLista();
  }

  linha(texto: string): Locator {
    return this.linhas.filter({ hasText: texto });
  }

  async abrirFicha(texto: string): Promise<void> {
    await this.linha(texto).getByRole("link", { name: "Ver / editar" }).click();
  }

  async bloquear(texto: string, motivo: string): Promise<void> {
    await this.linha(texto).getByRole("button", { name: "Bloquear" }).click();
    await this.confirmacao.getByLabel("Motivo (opcional)").fill(motivo);
    await this.confirmacao
      .getByRole("button", { name: "Confirmar bloqueio" })
      .click();
  }
}

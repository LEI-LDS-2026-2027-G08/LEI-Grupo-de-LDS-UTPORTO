import { expect, type Locator, type Page } from "@playwright/test";

/** W13 — Histórico clínico do paciente, do Veterinário e do Auxiliar (EP-033, EP-034). */
export class PaginaHistoricoClinico {
  readonly titulo: Locator;
  /** Texto da vista só de leitura do Auxiliar Veterinário. */
  readonly soLeitura: Locator;
  /** Indicação ao Veterinário de que o registo se cria na Agenda. */
  readonly criarNaAgenda: Locator;
  readonly desde: Locator;
  readonly ate: Locator;
  readonly filtrar: Locator;
  readonly limparFiltro: Locator;
  readonly tabela: Locator;
  readonly cabecalhos: Locator;
  readonly linhas: Locator;
  readonly vazia: Locator;
  /** Secção do registo aberto com «Ver registo». */
  readonly detalhe: Locator;
  /** «Registado por … em …» do registo aberto. */
  readonly autoria: Locator;
  /** Lista «Registo clínico» dentro do registo aberto. */
  readonly detalheDados: Locator;
  readonly tratamentos: Locator;
  readonly prescricoes: Locator;
  readonly alterarRegisto: Locator;
  private readonly aCarregar: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", {
      name: "Histórico clínico",
      level: 1,
    });
    this.soLeitura = page.getByText(
      "Vista só de leitura: pode consultar a informação clínica autorizada, mas não criar nem alterar registos.",
    );
    this.criarNaAgenda = page.getByText(
      "Para criar o registo de uma consulta, abra a consulta na Agenda.",
    );
    const filtro = page.getByRole("form", { name: "Filtrar por período" });
    this.desde = filtro.getByLabel("Desde", { exact: true });
    this.ate = filtro.getByLabel("Até", { exact: true });
    this.filtrar = filtro.getByRole("button", { name: "Filtrar" });
    this.limparFiltro = filtro.getByRole("button", { name: "Limpar filtro" });
    this.tabela = page.locator("table.tabela");
    this.cabecalhos = this.tabela.locator("thead th");
    this.linhas = this.tabela.locator("tbody tr");
    this.vazia = page.locator("main p.tabela-vazia");
    this.detalhe = page.locator("section.detalhe-registo");
    this.autoria = this.detalhe.locator("p.detalhe-conta");
    this.detalheDados = this.detalhe.locator(
      'dl[aria-label="Registo clínico"]',
    );
    this.tratamentos = this.detalhe.getByRole("list", { name: "Tratamentos" });
    this.prescricoes = this.detalhe.getByRole("list", { name: "Prescrições" });
    this.alterarRegisto = page.getByRole("link", { name: "Alterar registo" });
    this.aCarregar = page
      .getByRole("status")
      .filter({ hasText: "A carregar…" });
  }

  async abrir(animalId: string | number): Promise<void> {
    await this.page.goto(`/pacientes/${animalId}/registo`);
    await this.esperarLista();
  }

  /** Espera que a lista do histórico (e o registo aberto) tenham chegado. */
  async esperarLista(): Promise<void> {
    await this.titulo.waitFor();
    await expect(this.aCarregar).toHaveCount(0);
    await expect(this.tabela.or(this.vazia)).toBeVisible();
  }

  /** Linha do histórico de uma consulta, pela data (dd/mm/aaaa). */
  linha(dataPt: string): Locator {
    return this.linhas.filter({ hasText: dataPt });
  }

  /** Carrega em «Ver registo de <data>» e espera pelo registo. */
  async verRegisto(dataPt: string): Promise<void> {
    await this.page
      .getByRole("button", { name: `Ver registo de ${dataPt}`, exact: true })
      .click();
    await expect(this.detalhe).toBeVisible();
    await expect(this.aCarregar).toHaveCount(0);
    await expect(this.detalheDados).toBeVisible();
  }

  /** Valor (dd) de um campo do registo aberto, pelo rótulo (dt). */
  campo(rotulo: string): Locator {
    return this.detalheDados.locator(
      `xpath=./dt[normalize-space(.)="${rotulo}"]/following-sibling::dd[1]`,
    );
  }
}

import { expect, type Locator, type Page } from "@playwright/test";
import { URL_API } from "../apoio/ambiente";

/** W12 — Preparação de consultas, do Auxiliar Veterinário (EP-023, EP-024, EP-016, EP-029). */
export class PaginaPreparacao {
  readonly titulo: Locator;
  readonly dia: Locator;
  readonly tabela: Locator;
  readonly vazia: Locator;
  /** Secção «Consulta selecionada». */
  readonly selecionada: Locator;
  /** Lista «Informação de preparação». */
  readonly informacao: Locator;
  readonly notas: Locator;
  readonly semNotas: Locator;
  /** Formulário «Informação de apoio não clínica». */
  readonly formularioNota: Locator;
  readonly nota: Locator;
  readonly guardarNota: Locator;
  readonly aviso: Locator;
  /** Faixa de erro da API ao guardar a nota (FormErro). */
  readonly erroNota: Locator;
  readonly semAutorizacao: Locator;
  private readonly aCarregar: Locator;
  private readonly aAbrir: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", {
      name: "Preparação de consultas",
      level: 1,
    });
    this.dia = page
      .getByRole("group", { name: "Filtros" })
      .getByLabel("Dia", { exact: true });
    this.tabela = page.locator("table.tabela");
    this.vazia = page.getByText(
      "Não há consultas neste dia com estes filtros.",
    );
    this.selecionada = page.getByRole("region", {
      name: "Consulta selecionada",
    });
    this.informacao = page.locator('dl[aria-label="Informação de preparação"]');
    this.notas = page.getByRole("list", { name: "Notas de apoio" });
    this.semNotas = page.getByText("Ainda não há notas de apoio.");
    this.formularioNota = page.getByRole("form", {
      name: "Informação de apoio não clínica",
    });
    this.nota = page.getByLabel("Nova nota de apoio (não clínica)");
    this.guardarNota = page.getByRole("button", {
      name: "Guardar informação de apoio",
    });
    this.aviso = this.selecionada.locator("p.sucesso");
    this.erroNota = this.selecionada.getByRole("alert");
    this.semAutorizacao = page.getByText(
      "A sua conta não tem autorização para registar informação de apoio. Esta autorização é dada pelo Administrador, na conta de funcionário.",
    );
    this.aCarregar = page
      .getByRole("status")
      .filter({ hasText: "A carregar…" });
    this.aAbrir = page.getByText("A abrir a consulta…");
  }

  async esperarLista(): Promise<void> {
    await this.titulo.waitFor();
    await expect(this.aCarregar).toHaveCount(0);
    await expect(this.tabela.or(this.vazia)).toBeVisible();
  }

  /** Muda o dia no campo «Dia» e espera pela lista desse dia (EP-023). */
  async irParaDia(data: string): Promise<void> {
    await this.esperarLista();
    const resposta = this.page.waitForResponse(
      (r) =>
        r.request().method() === "GET" &&
        r.url().startsWith(`${URL_API}/consultas?`) &&
        new URL(r.url()).searchParams.get("dataInicio") === data,
    );
    await this.dia.fill(data);
    await resposta;
    await this.esperarLista();
  }

  /**
   * Carrega em «Selecionar a consulta de <animal> às <hora>» e espera pela
   * informação de preparação dessa consulta.
   */
  async selecionarConsulta(animal: string, hora: string): Promise<void> {
    await this.page
      .getByRole("button", {
        name: `Selecionar a consulta de ${animal} às ${hora}`,
        exact: true,
      })
      .click();
    await expect(this.aAbrir).toHaveCount(0);
    await expect(this.campo("Paciente")).toContainText(animal);
    await expect(this.campo("Consulta")).toContainText(`, ${hora} ·`);
  }

  /** Valor (dd) de um campo da informação de preparação, pelo rótulo (dt). */
  campo(rotulo: string): Locator {
    return this.informacao.locator(
      `xpath=./dt[normalize-space(.)="${rotulo}"]/following-sibling::dd[1]`,
    );
  }
}

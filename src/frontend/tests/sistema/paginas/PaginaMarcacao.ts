import { expect, type Locator, type Page } from "@playwright/test";

/** W11 — Detalhe da marcação: dados, ações de estado e chegada (EP-024, EP-027, EP-028). */
export class PaginaMarcacao {
  readonly titulo: Locator;
  /** Mensagem de sucesso (ex.: «Estado alterado para Confirmada.»). */
  readonly aviso: Locator;
  /** Aviso de alteração concorrente (412). */
  readonly avisoConflito: Locator;
  /** Faixa de erro da API (FormErro). */
  readonly erro: Locator;
  readonly dados: Locator;
  /** Secção «Ações» (só Rececionista). */
  readonly acoes: Locator;
  readonly editar: Locator;
  readonly registarChegada: Locator;
  /** Grupo «Confirmação» da mudança de estado. */
  readonly confirmacao: Locator;
  readonly abrirFicha: Locator;
  /** Secção «Registo clínico» (só Veterinário). */
  readonly registoClinico: Locator;
  readonly abrirRegistoClinico: Locator;
  readonly notasPreparacao: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", {
      name: "Detalhe da marcação",
      level: 1,
    });
    this.aviso = page.locator("main p.sucesso");
    this.avisoConflito = page.locator("main p.aviso");
    this.erro = page.locator("main").getByRole("alert");
    this.dados = page.locator('dl[aria-label="Dados da marcação"]');
    this.acoes = page.getByRole("region", { name: "Ações", exact: true });
    this.editar = page.getByRole("link", { name: "Editar marcação" });
    this.registarChegada = page.getByRole("button", {
      name: "Registar chegada",
    });
    this.confirmacao = page.getByRole("group", { name: "Confirmação" });
    this.abrirFicha = page.getByRole("link", {
      name: "Abrir ficha do paciente",
    });
    this.registoClinico = page.getByRole("region", {
      name: "Registo clínico",
      exact: true,
    });
    this.abrirRegistoClinico = page.getByRole("link", {
      name: "Abrir registo clínico",
    });
    this.notasPreparacao = page.getByRole("region", {
      name: "Notas de preparação",
    });
  }

  async abrir(id: string): Promise<void> {
    await this.page.goto(`/agenda/${id}`);
    await this.dados.waitFor();
  }

  /** Valor (dd) de um campo dos dados da marcação, pelo rótulo (dt). */
  campo(rotulo: string): Locator {
    return this.dados.locator(
      `xpath=./dt[normalize-space(.)="${rotulo}"]/following-sibling::dd[1]`,
    );
  }

  /** Botões de estado e de chegada oferecidos na secção «Ações». */
  get botoesAcoes(): Locator {
    return this.acoes.getByRole("button");
  }

  /** Botão de uma transição de estado (ex.: «Confirmar marcação»). */
  transicao(rotulo: string): Locator {
    return this.acoes.getByRole("button", { name: rotulo, exact: true });
  }

  /**
   * Carrega no botão da transição, escreve a observação (se houver) e
   * confirma; espera por «Estado alterado para <estado>.».
   */
  async mudarEstado(
    rotulo: string,
    estado: string,
    observacao = "",
  ): Promise<void> {
    await this.transicao(rotulo).click();
    await expect(this.confirmacao).toBeVisible();
    if (observacao) {
      await this.confirmacao.getByRole("textbox").fill(observacao);
    }
    await this.confirmacao
      .getByRole("button", { name: "Confirmar", exact: true })
      .click();
    await expect(this.aviso).toHaveText(`Estado alterado para ${estado}.`);
  }
}

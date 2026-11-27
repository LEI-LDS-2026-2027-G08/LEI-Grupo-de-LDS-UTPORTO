import type { Locator, Page } from "@playwright/test";

/** W13 — Registo clínico de uma consulta, do Veterinário (EP-031 a EP-033). */
export class PaginaRegistoClinico {
  readonly titulo: Locator;
  readonly formulario: Locator;
  readonly dadosConsulta: Locator;
  /** «Registado por … em …», com a última alteração quando existe. */
  readonly autoria: Locator;
  /** Texto mostrado enquanto a consulta não tem registo clínico. */
  readonly semRegisto: Locator;
  readonly notas: Locator;
  readonly diagnostico: Locator;
  readonly adicionarTratamento: Locator;
  readonly adicionarPrescricao: Locator;
  readonly criar: Locator;
  readonly guardarAlteracoes: Locator;
  readonly aviso: Locator;
  /** Erros do formulário: a validação geral e a faixa de erro da API. */
  readonly erros: Locator;
  /** Aviso de uma consulta que não admite registo clínico. */
  readonly estadoNaoAdmite: Locator;
  private readonly aCarregar: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", {
      name: "Registo clínico da consulta",
      level: 1,
    });
    this.formulario = page.getByRole("form", {
      name: "Registo clínico da consulta",
    });
    this.dadosConsulta = page.locator('dl[aria-label="Dados da consulta"]');
    this.autoria = page
      .locator("p.detalhe-conta")
      .filter({ hasText: /^Registado por/ });
    this.semRegisto = page.getByText(
      "Esta consulta ainda não tem registo clínico.",
    );
    this.notas = this.formulario.getByLabel("Notas clínicas", { exact: true });
    this.diagnostico = this.formulario.getByLabel("Diagnóstico", {
      exact: true,
    });
    this.adicionarTratamento = this.formulario.getByRole("button", {
      name: "Adicionar tratamento",
    });
    this.adicionarPrescricao = this.formulario.getByRole("button", {
      name: "Adicionar prescrição",
    });
    this.criar = this.formulario.getByRole("button", {
      name: "Criar registo clínico",
    });
    this.guardarAlteracoes = this.formulario.getByRole("button", {
      name: "Guardar alterações",
    });
    this.aviso = page.locator("main p.sucesso");
    this.erros = this.formulario.getByRole("alert");
    this.estadoNaoAdmite = page
      .locator("main p.aviso")
      .filter({ hasText: "Só é possível fazer o registo clínico" });
    this.aCarregar = page
      .getByRole("status")
      .filter({ hasText: "A carregar…" });
  }

  /** Espera que a consulta e o registo (se existir) tenham chegado. */
  async esperar(): Promise<void> {
    await this.titulo.waitFor();
    await this.aCarregar.waitFor({ state: "detached" });
    await this.dadosConsulta.or(this.page.locator("main .form-erro")).waitFor();
  }

  /** Valor (dd) de um campo dos dados da consulta, pelo rótulo (dt). */
  campo(rotulo: string): Locator {
    return this.dadosConsulta.locator(
      `xpath=./dt[normalize-space(.)="${rotulo}"]/following-sibling::dd[1]`,
    );
  }

  descricaoTratamento(n: number): Locator {
    return this.formulario.getByLabel(`Descrição do tratamento ${n}`);
  }

  dataTratamento(n: number): Locator {
    return this.formulario.getByLabel(`Data de aplicação do tratamento ${n}`);
  }

  medicamento(n: number): Locator {
    return this.formulario.getByLabel(`Medicamento da prescrição ${n}`);
  }

  dosagem(n: number): Locator {
    return this.formulario.getByLabel(`Dosagem da prescrição ${n}`);
  }

  frequencia(n: number): Locator {
    return this.formulario.getByLabel(`Frequência da prescrição ${n}`);
  }

  duracao(n: number): Locator {
    return this.formulario.getByLabel(
      `Duração em dias da prescrição ${n} (opcional)`,
    );
  }
}

import type { Locator, Page } from "@playwright/test";

export interface DadosFuncionario {
  nome: string;
  email: string;
  telefone?: string;
  /** Rótulo do perfil (ex.: «Auxiliar Veterinário»). */
  perfil: string;
  senha: string;
  apoio?: boolean;
}

/** W03 — Criar / editar funcionário. */
export class PaginaFuncionarioForm {
  readonly nome: Locator;
  readonly email: Locator;
  readonly telefone: Locator;
  readonly palavraPasse: Locator;
  readonly perfil: Locator;
  readonly apoio: Locator;
  readonly guardar: Locator;
  readonly aviso: Locator;
  readonly erro: Locator;
  /** Linha com o estado, a data de criação e o motivo do bloqueio. */
  readonly detalheConta: Locator;

  constructor(private readonly page: Page) {
    const formulario = page.locator("form.formulario");
    this.nome = formulario.getByLabel("Nome", { exact: true });
    this.email = formulario.getByLabel("E-mail", { exact: true });
    this.telefone = formulario.getByLabel("Telefone (opcional)");
    this.palavraPasse = formulario.getByLabel("Palavra-passe inicial");
    this.perfil = formulario.getByLabel("Perfil", { exact: true });
    this.apoio = formulario.getByLabel(
      "Pode registar informação não clínica de apoio",
    );
    this.guardar = formulario.getByRole("button", { name: "Guardar" });
    this.aviso = page.locator("p.sucesso");
    this.erro = formulario.getByRole("alert");
    this.detalheConta = page.locator("p.detalhe-conta");
  }

  async abrirNovo(): Promise<void> {
    await this.page.goto("/funcionarios/novo");
    await this.nome.waitFor();
  }

  async preencher(dados: DadosFuncionario): Promise<void> {
    await this.nome.fill(dados.nome);
    await this.email.fill(dados.email);
    await this.telefone.fill(dados.telefone ?? "");
    await this.palavraPasse.fill(dados.senha);
    await this.perfil.selectOption({ label: dados.perfil });
    if (dados.apoio !== undefined && (await this.apoio.count()) > 0) {
      await this.apoio.setChecked(dados.apoio);
    }
  }
}

import type { Locator, Page } from "@playwright/test";

export interface DadosCliente {
  nome: string;
  telefone: string;
  email: string;
  senha: string;
  nif?: string;
  morada?: string;
}

/** W05 — Criar / consultar / editar Cliente. */
export class PaginaClienteForm {
  readonly dadosCliente: Locator;
  readonly contaAcesso: Locator;
  readonly nome: Locator;
  readonly nif: Locator;
  readonly telefone: Locator;
  readonly morada: Locator;
  readonly email: Locator;
  readonly palavraPasse: Locator;
  readonly registar: Locator;
  readonly guardarAlteracoes: Locator;
  readonly voltarLista: Locator;
  readonly aviso: Locator;
  /** Faixa de erro da API (FormErro). */
  readonly erro: Locator;
  readonly estadoConta: Locator;
  readonly semAnimais: Locator;
  readonly verPaciente: Locator;

  constructor(private readonly page: Page) {
    this.dadosCliente = page.getByRole("group", { name: "Dados do cliente" });
    this.contaAcesso = page.getByRole("group", {
      name: "Conta de acesso (aplicação móvel)",
    });
    this.nome = this.dadosCliente.getByLabel("Nome", { exact: true });
    this.nif = this.dadosCliente.getByLabel("NIF (opcional)");
    this.telefone = this.dadosCliente.getByLabel("Telefone", { exact: true });
    this.morada = this.dadosCliente.getByLabel("Morada (opcional)");
    this.email = this.contaAcesso.getByLabel("E-mail", { exact: true });
    this.palavraPasse = this.contaAcesso.getByLabel("Palavra-passe inicial");
    this.registar = page.getByRole("button", { name: "Registar cliente" });
    this.guardarAlteracoes = page.getByRole("button", {
      name: "Guardar alterações",
    });
    this.voltarLista = page.getByRole("link", { name: "Voltar à lista" });
    this.aviso = page.locator("p.sucesso");
    this.erro = page.locator("form.formulario").getByRole("alert");
    this.estadoConta = this.contaAcesso.locator("p.detalhe-conta");
    this.semAnimais = page.getByText("Este cliente não tem animais.");
    this.verPaciente = page.getByRole("link", { name: "Ver paciente" });
  }

  async abrirNovo(): Promise<void> {
    await this.page.goto("/clientes/novo");
    await this.nome.waitFor();
  }

  async abrirFicha(id: string): Promise<void> {
    await this.page.goto(`/clientes/${id}`);
    await this.nome.waitFor();
  }

  async preencher(dados: DadosCliente): Promise<void> {
    await this.nome.fill(dados.nome);
    await this.nif.fill(dados.nif ?? "");
    await this.telefone.fill(dados.telefone);
    await this.morada.fill(dados.morada ?? "");
    await this.email.fill(dados.email);
    await this.palavraPasse.fill(dados.senha);
  }
}

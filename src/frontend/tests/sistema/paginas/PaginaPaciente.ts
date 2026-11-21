import type { Locator, Page } from "@playwright/test";
import { SeletorProprietario } from "./SeletorProprietario";

export interface DadosPaciente {
  nome: string;
  especie: string;
  raca?: string;
  /** Valor do sexo: Macho, Femea ou Desconhecido. */
  sexo: string;
  /** aaaa-mm-dd */
  dataNascimento?: string;
  /** Peso em kg, com ponto decimal (o campo é numérico). */
  peso?: string;
}

/** W08 — Ficha administrativa do paciente (registo, consulta e edição). */
export class PaginaPaciente {
  readonly titulo: Locator;
  readonly formulario: Locator;
  readonly proprietario: SeletorProprietario;
  readonly nome: Locator;
  readonly especie: Locator;
  readonly raca: Locator;
  readonly sexo: Locator;
  readonly dataNascimento: Locator;
  readonly peso: Locator;
  readonly guardar: Locator;
  readonly aviso: Locator;
  /** Faixa de erro da API (FormErro) ou de registo inexistente. */
  readonly erro: Locator;
  /** Linha com a idade, a última atualização e o telefone do proprietário. */
  readonly detalhe: Locator;
  readonly areaClinica: Locator;
  readonly verCliente: Locator;
  readonly novaMarcacao: Locator;
  readonly voltarListaPacientes: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", { level: 1 });
    this.formulario = page.locator("form.formulario");
    this.proprietario = new SeletorProprietario(page, this.formulario);
    this.nome = this.formulario.getByLabel("Nome do animal");
    this.especie = this.formulario.getByLabel("Espécie", { exact: true });
    this.raca = this.formulario.getByLabel("Raça (opcional)");
    this.sexo = this.formulario.getByLabel("Sexo", { exact: true });
    this.dataNascimento = this.formulario.getByLabel(
      "Data de nascimento (opcional)",
    );
    this.peso = this.formulario.getByLabel("Peso em kg (opcional)");
    this.guardar = page.getByRole("button", {
      name: "Guardar dados administrativos",
    });
    this.aviso = page.locator("p.sucesso");
    this.erro = page.getByRole("alert");
    this.detalhe = page.locator("p.detalhe-conta");
    this.areaClinica = page.getByRole("navigation", { name: "Área clínica" });
    this.verCliente = page.getByRole("link", { name: "Ver cliente" });
    this.novaMarcacao = page.getByRole("link", { name: "Nova marcação" });
    this.voltarListaPacientes = page.getByRole("link", {
      name: "Voltar à lista de pacientes",
    });
  }

  async abrirNovo(): Promise<void> {
    await this.page.goto("/pacientes/novo");
    await this.nome.waitFor();
  }

  async abrir(caminho: string): Promise<void> {
    await this.page.goto(caminho);
    await this.nome.waitFor();
  }

  async preencher(dados: DadosPaciente): Promise<void> {
    await this.nome.fill(dados.nome);
    await this.especie.fill(dados.especie);
    await this.raca.fill(dados.raca ?? "");
    await this.sexo.selectOption(dados.sexo);
    await this.dataNascimento.fill(dados.dataNascimento ?? "");
    await this.peso.fill(dados.peso ?? "");
  }
}

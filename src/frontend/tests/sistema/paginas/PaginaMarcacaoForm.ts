import { expect, type Locator, type Page } from "@playwright/test";
import { dataPt, literal } from "../apoio/dados";
import { respostaDe } from "../apoio/pedidos";

/** W10 — Nova marcação e Editar marcação (EP-021, EP-022, EP-025, EP-026). */
export class PaginaMarcacaoForm {
  readonly titulo: Locator;
  readonly formulario: Locator;
  readonly pesquisaPaciente: Locator;
  readonly procurarPaciente: Locator;
  readonly veterinario: Locator;
  readonly aPartirDe: Locator;
  readonly consultar: Locator;
  /** Grupo «Horários livres», com um bloco por dia. */
  readonly horarios: Locator;
  /** Linha «Horário escolhido: …», com «(horário atual)» na edição. */
  readonly horarioEscolhido: Locator;
  readonly motivo: Locator;
  readonly criar: Locator;
  readonly guardarAlteracoes: Locator;
  /** Faixa de erro da API (FormErro) dentro do formulário. */
  readonly erro: Locator;
  /** Aviso depois de uma recusa (disponibilidade ou marcação atualizada). */
  readonly aviso: Locator;
  /** Mensagem de uma marcação Realizada ou Cancelada aberta para editar. */
  readonly naoAlteravel: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", { level: 1 });
    this.formulario = page.locator("form.formulario");
    this.pesquisaPaciente = this.formulario.getByLabel(
      "Paciente (nome do animal ou do proprietário)",
    );
    this.procurarPaciente = this.formulario.getByRole("button", {
      name: "Procurar paciente",
    });
    this.veterinario = this.formulario.getByLabel("Veterinário", {
      exact: true,
    });
    this.aPartirDe = this.formulario.getByLabel("A partir de");
    this.consultar = this.formulario.getByRole("button", {
      name: "Consultar disponibilidade",
    });
    this.horarios = this.formulario.getByRole("group", {
      name: "Horários livres",
    });
    this.horarioEscolhido = this.formulario
      .locator("p")
      .filter({ hasText: /^Horário escolhido:/ });
    this.motivo = this.formulario.getByLabel("Motivo (opcional)");
    this.criar = this.formulario.getByRole("button", {
      name: "Criar marcação",
    });
    this.guardarAlteracoes = this.formulario.getByRole("button", {
      name: "Guardar alterações",
    });
    this.erro = this.formulario.getByRole("alert");
    this.aviso = page.locator("p.aviso");
    this.naoAlteravel = page
      .getByRole("alert")
      .filter({ hasText: "já não pode ser alterada" });
  }

  /** Abre `/agenda/{id}/editar` e espera pelo formulário ou pela recusa. */
  async abrirEdicao(id: string): Promise<void> {
    await this.page.goto(`/agenda/${id}/editar`);
    await this.motivo.or(this.naoAlteravel).waitFor();
  }

  /** PC-05, passo 2: procura o paciente e carrega em «Selecionar …». */
  async escolherPaciente(animal: string, cliente: string): Promise<void> {
    await this.pesquisaPaciente.fill(animal);
    await this.procurarPaciente.click();
    await this.page
      .getByRole("button", {
        name: `Selecionar ${animal} (${cliente})`,
        exact: true,
      })
      .click();
    await expect(
      this.formulario.locator("strong", { hasText: animal }),
    ).toBeVisible();
  }

  /**
   * PC-05, passo 3: escolhe o Veterinário (se indicado), escreve o dia em
   * «A partir de» e espera pela resposta da disponibilidade (EP-022).
   */
  async consultarDisponibilidade(
    veterinario: string | null,
    data: string,
  ): Promise<void> {
    if (veterinario !== null) {
      await this.veterinario.selectOption({ label: veterinario });
    }
    await this.aPartirDe.fill(data);
    const resposta = respostaDe(
      this.page,
      "GET",
      /^\/veterinarios\/\d+\/disponibilidade$/,
    );
    await this.consultar.click();
    expect((await resposta).status()).toBe(200);
    await expect(this.consultar).toBeEnabled();
    await expect(this.horarios).toBeVisible();
  }

  /** Bloco de um dia em «Horários livres». */
  dia(data: string): Locator {
    return this.horarios.locator(".campo").filter({ hasText: dataPt(data) });
  }

  /** Botão de um horário livre (ex.: «seg., 30/11/2026 às 10:00»). */
  horario(data: string, hora: string): Locator {
    return this.horarios.getByRole("button", {
      name: new RegExp(`${literal(dataPt(data))} às ${literal(hora)}$`),
    });
  }

  /** Horas livres de um dia, pela ordem em que aparecem. */
  async horasLivres(data: string): Promise<string[]> {
    const textos = await this.dia(data).getByRole("button").allTextContents();
    return textos.map((t) => t.trim());
  }

  /**
   * PC-05, passo 4: escolhe o horário indicado ou, por omissão, o primeiro
   * livre do dia. Devolve a hora escolhida.
   */
  async escolherHorario(data: string, hora?: string): Promise<string> {
    const botao = hora
      ? this.horario(data, hora)
      : this.dia(data).getByRole("button").first();
    await expect(botao).toBeVisible();
    const escolhida = ((await botao.textContent()) ?? "").trim();
    await botao.click();
    await expect(botao).toHaveAttribute("aria-pressed", "true");
    await expect(this.horarioEscolhido).toContainText(
      `${dataPt(data)} às ${escolhida}`,
    );
    return escolhida;
  }
}

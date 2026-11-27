import { expect, type Locator, type Page } from "@playwright/test";
import { URL_API } from "../apoio/ambiente";
import { dataPt } from "../apoio/dados";

/** W09 — Agenda: as marcações de um dia, com filtros (EP-023). */
export class PaginaAgenda {
  readonly titulo: Locator;
  readonly novaMarcacao: Locator;
  /** Texto mostrado ao Veterinário em vez do filtro «Veterinário». */
  readonly soAtribuidas: Locator;
  readonly dia: Locator;
  readonly veterinario: Locator;
  readonly estado: Locator;
  readonly diaAnterior: Locator;
  readonly diaSeguinte: Locator;
  /** Dia mostrado ao lado dos botões (ex.: «seg., 30/11/2026»). */
  readonly diaMostrado: Locator;
  readonly tabela: Locator;
  readonly linhas: Locator;
  readonly vazia: Locator;
  private readonly aCarregar: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", { name: "Agenda", level: 1 });
    this.novaMarcacao = page.getByRole("link", { name: "Nova marcação" });
    this.soAtribuidas = page.getByText(
      "Mostra só as consultas que lhe estão atribuídas.",
    );
    const filtros = page.getByRole("group", { name: "Filtros da agenda" });
    this.dia = filtros.getByLabel("Dia", { exact: true });
    this.veterinario = filtros.getByLabel("Veterinário", { exact: true });
    this.estado = filtros.getByLabel("Estado", { exact: true });
    const dias = page.getByRole("navigation", { name: "Dias" });
    this.diaAnterior = dias.getByRole("button", { name: "Dia anterior" });
    this.diaSeguinte = dias.getByRole("button", { name: "Dia seguinte" });
    this.diaMostrado = dias.locator("strong");
    this.tabela = page.locator("table.tabela");
    this.linhas = this.tabela.locator("tbody tr");
    this.vazia = page.getByText(
      "Não há marcações neste dia com estes filtros.",
    );
    this.aCarregar = page
      .getByRole("status")
      .filter({ hasText: "A carregar…" });
  }

  /** Espera que a agenda mostre a resposta ao pedido mais recente. */
  async esperarLista(): Promise<void> {
    await this.titulo.waitFor();
    await expect(this.aCarregar).toHaveCount(0);
    await expect(this.tabela.or(this.vazia)).toBeVisible();
  }

  /**
   * Executa uma ação que muda o dia ou um filtro e espera pela nova lista
   * (EP-023), sem pausas fixas.
   */
  async aposNovaLista(acao: () => Promise<void>): Promise<void> {
    const resposta = this.page.waitForResponse(
      (r) =>
        r.request().method() === "GET" &&
        r.url().startsWith(`${URL_API}/consultas?`),
    );
    await acao();
    await resposta;
    await this.esperarLista();
  }

  /** Muda o dia no campo «Dia». */
  async irParaDia(data: string): Promise<void> {
    await this.esperarLista();
    await this.aposNovaLista(() => this.dia.fill(data));
    await expect(this.diaMostrado).toContainText(dataPt(data));
  }

  /** Filtro «Veterinário»: um nome ou «Todos». */
  async escolherVeterinario(rotulo: string): Promise<void> {
    await this.escolher(this.veterinario, rotulo);
  }

  /** Filtro «Estado»: um estado ou «Todos». */
  async escolherEstado(rotulo: string): Promise<void> {
    await this.escolher(this.estado, rotulo);
  }

  /** Escolhe a opção e, se o filtro mudou, espera pela nova lista. */
  private async escolher(lista: Locator, rotulo: string): Promise<void> {
    const atual = await lista.evaluate(
      (s: HTMLSelectElement) => s.selectedOptions[0]?.textContent ?? "",
    );
    if (atual === rotulo) return;
    await this.aposNovaLista(async () => {
      await lista.selectOption({ label: rotulo });
    });
  }

  /** Linhas da agenda com este texto (ex.: o nome do paciente de teste). */
  linhasCom(texto: string): Locator {
    return this.linhas.filter({ hasText: texto });
  }

  /** Ligação «Ver» da marcação de um paciente a uma hora. */
  verMarcacao(animal: string, hora: string): Locator {
    return this.page.getByRole("link", {
      name: `Ver marcação de ${animal} às ${hora}`,
      exact: true,
    });
  }
}

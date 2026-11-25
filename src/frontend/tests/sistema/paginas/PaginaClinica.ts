import type { Locator, Page } from "@playwright/test";

/** Dias da semana como aparecem no ecrã. */
export const DIAS = [
  "Segunda",
  "Terça",
  "Quarta",
  "Quinta",
  "Sexta",
  "Sábado",
  "Domingo",
] as const;
export type Dia = (typeof DIAS)[number];

/** W06 — Dados gerais, configurações e horário da clínica. */
export class PaginaClinica {
  readonly titulo: Locator;
  readonly nome: Locator;
  readonly nif: Locator;
  readonly morada: Locator;
  readonly telefone: Locator;
  readonly email: Locator;
  readonly duracao: Locator;
  readonly guardar: Locator;
  readonly cancelar: Locator;
  readonly aviso: Locator;
  /** Lista «Corrija os seguintes pontos antes de guardar:» (validação local). */
  readonly errosValidacao: Locator;
  /** Faixa de erro da API (FormErro). */
  readonly erroApi: Locator;

  constructor(private readonly page: Page) {
    this.titulo = page.getByRole("heading", {
      name: "Clínica e horários",
      level: 1,
    });
    const gerais = page.getByRole("group", { name: "Dados gerais" });
    this.nome = gerais.getByLabel("Nome da clínica");
    this.nif = gerais.getByLabel("NIF", { exact: true });
    this.morada = gerais.getByLabel("Morada", { exact: true });
    this.telefone = gerais.getByLabel("Telefone", { exact: true });
    this.email = gerais.getByLabel("E-mail", { exact: true });
    this.duracao = gerais.getByLabel("Duração da consulta (minutos)");
    this.guardar = page.getByRole("button", { name: "Guardar", exact: true });
    this.cancelar = page.getByRole("button", { name: "Cancelar", exact: true });
    this.aviso = page.locator("p.sucesso");
    this.errosValidacao = page
      .getByRole("alert")
      .filter({ hasText: "Corrija os seguintes pontos antes de guardar:" });
    this.erroApi = page
      .getByRole("alert")
      .filter({ hasNotText: "Corrija os seguintes pontos antes de guardar:" });
  }

  async abrir(): Promise<void> {
    await this.page.goto("/clinica");
    await this.esperarFormulario();
  }

  /** Espera que o «A carregar…» dê lugar ao formulário. */
  async esperarFormulario(): Promise<void> {
    await this.titulo.waitFor();
    await this.nome.waitFor();
  }

  /** Bloco de um dia no «Horário de funcionamento». */
  dia(dia: Dia): Locator {
    return this.page.locator(".horario-dia").filter({
      has: this.page.getByRole("heading", { name: dia, exact: true }),
    });
  }

  hora(dia: Dia, periodo: number, campo: "abertura" | "fecho"): Locator {
    return this.page.getByLabel(`${dia}, período ${periodo} — ${campo}`, {
      exact: true,
    });
  }

  removerPeriodo(dia: Dia, periodo: number): Locator {
    return this.page.getByRole("button", {
      name: `Remover ${dia}, período ${periodo}`,
      exact: true,
    });
  }

  adicionarPeriodo(dia: Dia): Locator {
    return this.page.getByRole("button", {
      name: `Adicionar período a ${dia}`,
      exact: true,
    });
  }

  /** Inputs de abertura/fecho de um dia (dois por período). */
  horasDoDia(dia: Dia): Locator {
    return this.dia(dia).locator('input[type="time"]');
  }
}

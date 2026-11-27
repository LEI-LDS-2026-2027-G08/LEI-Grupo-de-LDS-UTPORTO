import { expect, type Page } from "@playwright/test";
import { LayoutApp } from "../paginas/LayoutApp";
import { PaginaAgenda } from "../paginas/PaginaAgenda";
import { PaginaClienteForm } from "../paginas/PaginaClienteForm";
import { PaginaClientes } from "../paginas/PaginaClientes";
import { PaginaMarcacao } from "../paginas/PaginaMarcacao";
import { PaginaMarcacaoForm } from "../paginas/PaginaMarcacaoForm";
import { PaginaPaciente } from "../paginas/PaginaPaciente";
import { PaginaPacientes } from "../paginas/PaginaPacientes";
import type { Conta } from "./contas";
import { emailDe, senhaGerada, telefoneNovo } from "./dados";
import { entrarComo } from "./sessao";

export interface ClienteDeTeste {
  id: string;
  nome: string;
  telefone: string;
  email: string;
}

export interface PacienteDeTeste {
  /** Caminho da ficha (ex.: «/pacientes/42»). */
  caminho: string;
  nome: string;
}

export interface MarcacaoDeTeste {
  id: string;
  /** Dia em Lisboa (aaaa-mm-dd). */
  data: string;
  /** Hora de Lisboa (HH:mm). */
  hora: string;
  motivo: string;
  veterinario: Conta;
}

/**
 * PC-03 — Criar um Cliente de teste (Rececionista). Deixa a sessão da
 * Rececionista aberta e a ficha do Cliente no ecrã.
 */
export async function criarClienteDeTeste(
  page: Page,
  suf: string,
): Promise<ClienteDeTeste> {
  await entrarComo.rececionista(page);
  await new LayoutApp(page).irPara("Clientes");
  await new PaginaClientes(page).novoCliente.click();

  const ficha = new PaginaClienteForm(page);
  const cliente = {
    nome: `Cliente ${suf}`,
    telefone: telefoneNovo(),
    email: emailDe(suf),
  };
  await ficha.preencher({ ...cliente, senha: senhaGerada() });
  await ficha.registar.click();
  await expect(ficha.aviso).toHaveText(
    `Cliente ${cliente.nome} registado, com conta de acesso para ${cliente.email}.`,
  );
  const id = /\/clientes\/(\d+)$/.exec(page.url())?.[1];
  if (!id)
    throw new Error(`Endereço inesperado depois do registo: ${page.url()}`);
  return { id, ...cliente };
}

/**
 * PC-04 — Criar um paciente de teste (Rececionista), com a sessão da
 * Rececionista aberta. Deixa a ficha do paciente no ecrã.
 */
export async function criarPacienteDeTeste(
  page: Page,
  suf: string,
  cliente: ClienteDeTeste,
): Promise<PacienteDeTeste> {
  await new LayoutApp(page).irPara("Pacientes");
  await new PaginaPacientes(page).novoPaciente.click();

  const ficha = new PaginaPaciente(page);
  await ficha.proprietario.escolher(cliente.nome, cliente.nome);
  const nome = `Animal ${suf}`;
  await ficha.preencher({
    nome,
    especie: "Cão",
    raca: "Labrador Retriever",
    sexo: "Macho",
    dataNascimento: "2023-01-01",
    peso: "10.5",
  });
  await ficha.guardar.click();
  await expect(ficha.aviso).toHaveText(
    `Paciente ${nome} registado para ${cliente.nome}.`,
  );
  const caminho = new URL(page.url()).pathname;
  expect(caminho).toMatch(/^\/pacientes\/\d+$/);
  return { caminho, nome };
}

/** PC-03 e PC-04 — Cliente e paciente de teste, com a sessão da Rececionista. */
export async function prepararPaciente(
  page: Page,
  suf: string,
): Promise<{ cliente: ClienteDeTeste; paciente: PacienteDeTeste }> {
  const cliente = await criarClienteDeTeste(page, suf);
  const paciente = await criarPacienteDeTeste(page, suf, cliente);
  return { cliente, paciente };
}

/** Id do paciente a partir do caminho da ficha (ex.: «/pacientes/42» → «42»). */
export function idDoPaciente(paciente: PacienteDeTeste): string {
  return paciente.caminho.split("/").pop() ?? "";
}

/**
 * PC-05 — Criar uma marcação de teste (Rececionista), com a sessão da
 * Rececionista aberta: menu «Agenda» → «Nova marcação», paciente de teste,
 * Veterinário, disponibilidade a partir de {D}, o horário indicado (por
 * omissão, o primeiro livre de {D}) e «Motivo {sufixo}». Deixa o detalhe da
 * marcação no ecrã.
 */
export async function criarMarcacaoDeTeste(
  page: Page,
  suf: string,
  cliente: ClienteDeTeste,
  paciente: PacienteDeTeste,
  opcoes: { veterinario: Conta; data: string; hora?: string },
): Promise<MarcacaoDeTeste> {
  await new LayoutApp(page).irPara("Agenda");
  const agenda = new PaginaAgenda(page);
  await agenda.titulo.waitFor();
  await agenda.novaMarcacao.click();

  const formulario = new PaginaMarcacaoForm(page);
  await formulario.escolherPaciente(paciente.nome, cliente.nome);
  await formulario.consultarDisponibilidade(
    opcoes.veterinario.nome,
    opcoes.data,
  );
  const hora = await formulario.escolherHorario(opcoes.data, opcoes.hora);
  const motivo = `Motivo ${suf}`;
  await formulario.motivo.fill(motivo);
  await formulario.criar.click();

  await expect(new PaginaMarcacao(page).aviso).toHaveText(
    "Marcação criada no estado Pendente.",
  );
  const id = /\/agenda\/(\d+)$/.exec(page.url())?.[1];
  if (!id) {
    throw new Error(`Endereço inesperado depois da marcação: ${page.url()}`);
  }
  return {
    id,
    data: opcoes.data,
    hora,
    motivo,
    veterinario: opcoes.veterinario,
  };
}

/**
 * PC-06 — Confirmar a marcação aberta (Rececionista): «Confirmar marcação»,
 * «Confirmar» na «Confirmação» e «Estado alterado para Confirmada.».
 */
export async function confirmarMarcacaoAberta(page: Page): Promise<void> {
  await new PaginaMarcacao(page).mudarEstado(
    "Confirmar marcação",
    "Confirmada",
  );
}

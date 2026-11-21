import { expect, type Page } from "@playwright/test";
import { LayoutApp } from "../paginas/LayoutApp";
import { PaginaClienteForm } from "../paginas/PaginaClienteForm";
import { PaginaClientes } from "../paginas/PaginaClientes";
import { PaginaPaciente } from "../paginas/PaginaPaciente";
import { PaginaPacientes } from "../paginas/PaginaPacientes";
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

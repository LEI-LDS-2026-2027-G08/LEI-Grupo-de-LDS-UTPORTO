import type { PacienteFormState } from "../../models/pacientes";

/**
 * Validação local do formulário de W08, com as regras e mensagens da API
 * (UpdateAnimalDto e AnimaisService). A API continua a ser quem valida.
 */

export type ErrosPaciente = Partial<Record<keyof PacienteFormState, string>>;

/** Data de hoje (hora local) no formato aaaa-mm-dd. */
export function hojeIso(agora: Date = new Date()): string {
  const mes = String(agora.getMonth() + 1).padStart(2, "0");
  const dia = String(agora.getDate()).padStart(2, "0");
  return `${agora.getFullYear()}-${mes}-${dia}`;
}

/** Converte o peso escrito (aceita vírgula) num número; vazio dá null. */
export function lerPeso(texto: string): number | null {
  const limpo = texto.trim().replace(",", ".");
  if (limpo === "") return null;
  return Number(limpo);
}

export function validarPaciente(
  formulario: PacienteFormState,
  hoje: string = hojeIso(),
): ErrosPaciente {
  const erros: ErrosPaciente = {};
  const nome = formulario.nome.trim();
  const especie = formulario.especie.trim();
  const raca = formulario.raca.trim();

  if (nome === "") erros.nome = "O nome é obrigatório.";
  else if (nome.length > 80)
    erros.nome = "O nome não pode ter mais de 80 caracteres.";

  if (especie === "") erros.especie = "A espécie é obrigatória.";
  else if (especie.length > 50)
    erros.especie = "A espécie não pode ter mais de 50 caracteres.";

  if (raca.length > 80) {
    erros.raca = "A raça não pode ter mais de 80 caracteres.";
  }

  if (formulario.dataNascimento !== "" && formulario.dataNascimento > hoje) {
    erros.dataNascimento = "A data de nascimento não pode ser futura.";
  }

  const peso = lerPeso(formulario.peso);
  if (peso !== null && (Number.isNaN(peso) || peso < 0.01 || peso > 9999.99)) {
    erros.peso = "O peso deve estar entre 0,01 e 9999,99 kg.";
  }

  if (formulario.proprietario === null) {
    erros.proprietario = "Escolha o proprietário (Cliente) do paciente.";
  }

  return erros;
}

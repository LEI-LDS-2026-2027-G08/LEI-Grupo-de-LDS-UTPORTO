import type { ClienteFormState } from "../../models/clientes";

/**
 * Validação local do formulário de W05, com as mesmas regras e mensagens da
 * API (CreateClienteDto e UpdateClienteDto). Só serve para avisar antes de
 * enviar: a API continua a ser quem valida (Modelo de Classes Web, §14).
 */

export type ErrosCliente = Partial<Record<keyof ClienteFormState, string>>;

const TELEFONE = /^(?=.{9,20}$)\+?[0-9 ]+$/;
// Verificação básica (texto@texto.texto); a validação completa é a da API.
const EMAIL = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

/** NIF português: 9 dígitos e dígito de controlo (módulo 11), como na API. */
export function nifValido(nif: string): boolean {
  if (!/^\d{9}$/.test(nif)) return false;
  let soma = 0;
  for (let i = 0; i < 8; i++) {
    soma += Number(nif[i]) * (9 - i);
  }
  const resto = soma % 11;
  const controlo = resto < 2 ? 0 : 11 - resto;
  return Number(nif[8]) === controlo;
}

/** Devolve os erros por campo (objeto vazio se o formulário estiver válido). */
export function validarCliente(
  formulario: ClienteFormState,
  aCriar: boolean,
): ErrosCliente {
  const erros: ErrosCliente = {};
  const nome = formulario.nome.trim();
  const nif = formulario.nif.trim();
  const telefone = formulario.telefone.trim();
  const email = formulario.email.trim();
  const morada = formulario.morada.trim();

  if (nome === "") erros.nome = "O nome é obrigatório.";
  else if (nome.length > 150)
    erros.nome = "O nome não pode ter mais de 150 caracteres.";

  if (nif !== "" && !nifValido(nif)) {
    erros.nif =
      "O NIF é inválido: deve ter 9 dígitos e um dígito de controlo correto.";
  }

  if (telefone === "") erros.telefone = "O telefone é obrigatório.";
  else if (!TELEFONE.test(telefone)) {
    erros.telefone =
      "O telefone deve ter entre 9 e 20 caracteres: dígitos, espaços e, opcionalmente, o indicativo + no início.";
  }

  if (email === "") erros.email = "O e-mail é obrigatório.";
  else if (email.length > 256)
    erros.email = "O e-mail não pode ter mais de 256 caracteres.";
  else if (!EMAIL.test(email)) erros.email = "O e-mail é inválido.";

  if (morada.length > 250) {
    erros.morada = "A morada não pode ter mais de 250 caracteres.";
  }

  if (aCriar) {
    const tamanho = formulario.passwordInicial.length;
    if (tamanho === 0) {
      erros.passwordInicial = "A palavra-passe inicial é obrigatória.";
    } else if (tamanho < 8 || tamanho > 128) {
      erros.passwordInicial =
        "A palavra-passe inicial deve ter entre 8 e 128 caracteres.";
    }
  }

  return erros;
}

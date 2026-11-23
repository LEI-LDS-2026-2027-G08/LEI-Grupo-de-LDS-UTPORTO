import type {
  AdministracaoForm,
  AdministracaoRequest,
} from "../../models/vacinacoes";

/**
 * Validação local do formulário de W16, só dos campos (obrigatórios,
 * tamanhos e formato), com as mensagens da API (CreateAdministracaoDto).
 * As regras de negócio (data não futura, cálculo da próxima administração)
 * ficam na API: os erros dela aparecem no formulário.
 */

export const MAX_PRODUTO = 150;
export const MAX_LOTE = 50;
/** A coluna do intervalo é smallint (CK_Profilaxia_Intervalo). */
export const MAX_INTERVALO_DIAS = 32767;

export type ErrosAdministracao = Partial<
  Record<keyof AdministracaoForm, string>
>;

export function formularioVazio(dataAdministracao = ""): AdministracaoForm {
  return {
    tipo: "Vacina",
    produto: "",
    lote: "",
    dataAdministracao,
    intervaloDias: "",
  };
}

/** Intervalo escrito no campo; vazio dá null; texto inválido dá NaN. */
export function lerIntervalo(texto: string): number | null {
  const limpo = texto.trim();
  if (limpo === "") return null;
  return /^\d+$/.test(limpo) ? Number(limpo) : Number.NaN;
}

export function validarAdministracao(
  form: AdministracaoForm,
): ErrosAdministracao {
  const erros: ErrosAdministracao = {};
  const produto = form.produto.trim();
  const lote = form.lote.trim();

  if (produto === "") erros.produto = "O produto é obrigatório.";
  else if (produto.length > MAX_PRODUTO)
    erros.produto = "O produto não pode ter mais de 150 caracteres.";

  if (lote === "") erros.lote = "O lote é obrigatório.";
  else if (lote.length > MAX_LOTE)
    erros.lote = "O lote não pode ter mais de 50 caracteres.";

  if (!/^\d{4}-\d{2}-\d{2}$/.test(form.dataAdministracao)) {
    erros.dataAdministracao = "A data de administração é obrigatória.";
  }

  const intervalo = lerIntervalo(form.intervaloDias);
  if (
    intervalo !== null &&
    (Number.isNaN(intervalo) || intervalo < 1 || intervalo > MAX_INTERVALO_DIAS)
  ) {
    erros.intervaloDias =
      "O intervalo, quando indicado, deve ser um número inteiro de dias entre 1 e 32767.";
  }
  return erros;
}

export function pedidoDoFormulario(
  form: AdministracaoForm,
): AdministracaoRequest {
  return {
    tipo: form.tipo,
    produto: form.produto,
    lote: form.lote,
    dataAdministracao: form.dataAdministracao,
    intervaloDias: lerIntervalo(form.intervaloDias),
  };
}

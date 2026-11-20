/**
 * Validação local do período do histórico clínico (EP-034), com as regras e
 * as mensagens da API (HistoricoClinicoQuery → ValidadorFiltroConsultas).
 * A API continua a ser quem valida.
 */

/** Mensagens iguais às da API (ValidadorPeriodoDisponibilidade). */
export const MENSAGEM_FIM_ANTERIOR =
  "A data de fim (dataFim) não pode ser anterior à data de início (dataInicio).";
export const MENSAGEM_FORA_DOS_LIMITES =
  "As datas têm de estar entre 2000-01-01 e 9998-12-31.";

const DATA_MINIMA = "2000-01-01";
const DATA_MAXIMA = "9998-12-31";

/** Valida o período do filtro antes de o pedir à API (EP-034, 400). */
export function validarPeriodo(inicio: string, fim: string): string | null {
  for (const data of [inicio, fim]) {
    if (data !== "" && (data < DATA_MINIMA || data > DATA_MAXIMA)) {
      return MENSAGEM_FORA_DOS_LIMITES;
    }
  }
  if (inicio !== "" && fim !== "" && fim < inicio) {
    return MENSAGEM_FIM_ANTERIOR;
  }
  return null;
}

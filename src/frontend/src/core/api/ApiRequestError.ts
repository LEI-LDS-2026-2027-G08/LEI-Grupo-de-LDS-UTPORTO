import type { ApiErro } from "../../models/common";

/**
 * Tipo de problema, já traduzido a partir do código HTTP.
 * Os formulários e páginas decidem o que fazer olhando para a categoria
 * (ex.: "validacao" -> mostrar mensagem junto ao formulário).
 */
export type CategoriaErro =
  | "validacao" // 400
  | "naoAutenticado" // 401
  | "semPermissao" // 403
  | "naoEncontrado" // 404
  | "conflito" // 409
  | "versaoDesatualizada" // 412 (If-Match / ETag)
  | "regraNegocio" // 422
  | "servidor" // 5xx
  | "rede" // sem resposta do servidor
  | "configuracao" // falta VITE_API_URL
  | "desconhecido";

const MENSAGENS_POR_OMISSAO: Record<CategoriaErro, string> = {
  validacao: "Os dados enviados não são válidos.",
  naoAutenticado: "A sessão terminou. Inicie sessão novamente.",
  semPermissao: "Não tem permissão para realizar esta operação.",
  naoEncontrado: "O registo pedido não foi encontrado.",
  conflito: "A operação entra em conflito com o estado atual dos dados.",
  versaoDesatualizada:
    "Este registo foi alterado entretanto. Recarregue a página e tente novamente.",
  regraNegocio: "A operação não é permitida pelas regras da clínica.",
  servidor: "Ocorreu um erro no servidor. Tente novamente mais tarde.",
  rede: "Não foi possível contactar o servidor. Verifique a ligação.",
  configuracao: "A aplicação não está configurada (falta VITE_API_URL).",
  desconhecido: "Ocorreu um erro inesperado.",
};

export function categoriaDeStatus(status: number): CategoriaErro {
  switch (status) {
    case 400:
      return "validacao";
    case 401:
      return "naoAutenticado";
    case 403:
      return "semPermissao";
    case 404:
      return "naoEncontrado";
    case 409:
      return "conflito";
    case 412:
      return "versaoDesatualizada";
    case 422:
      return "regraNegocio";
    default:
      return status >= 500 ? "servidor" : "desconhecido";
  }
}

/**
 * O único tipo de erro que o ApiClient lança. `message` já é um texto
 * pronto para mostrar ao utilizador.
 */
export class ApiRequestError extends Error {
  readonly status: number;
  readonly categoria: CategoriaErro;
  /** Corpo de erro original do backend, se existir (nunca para 5xx). */
  readonly erro: ApiErro | null;

  constructor(
    status: number,
    categoria: CategoriaErro,
    message?: string,
    erro: ApiErro | null = null,
  ) {
    super(message ?? MENSAGENS_POR_OMISSAO[categoria]);
    this.name = "ApiRequestError";
    this.status = status;
    this.categoria = categoria;
    this.erro = erro;
  }
}

/** Texto seguro para mostrar ao utilizador, venha o erro de onde vier. */
export function mensagemDeErro(erro: unknown): string {
  if (erro instanceof ApiRequestError) return erro.message;
  return MENSAGENS_POR_OMISSAO.desconhecido;
}

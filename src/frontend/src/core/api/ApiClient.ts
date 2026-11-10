import type { ApiErro } from "../../models/common";
import {
  ApiRequestError,
  categoriaDeStatus,
  type CategoriaErro,
} from "./ApiRequestError";

export type Metodo = "GET" | "POST" | "PUT" | "PATCH" | "DELETE";
type ValorQuery = string | number | boolean | null | undefined;

export interface PedidoOpcoes {
  /** Parâmetros de query string. Valores null/undefined são ignorados. */
  query?: Record<string, ValorQuery>;
  /** Cabeçalhos extra (ex.: { "If-Match": etag } nas marcações). */
  headers?: Record<string, string>;
  /**
   * Pedido público (ex.: login): não envia o token e um 401 não é tratado
   * como "sessão expirada" (é só credenciais erradas).
   */
  publico?: boolean;
  signal?: AbortSignal;
}

/**
 * Resposta com os cabeçalhos, para quem precisa de ler, por exemplo, o
 * `ETag` das marcações (Modelo de Classes Web, §9.2).
 */
export interface RespostaComCabecalhos<T> {
  dados: T;
  cabecalhos: Headers;
}

function eApiErro(valor: unknown): valor is ApiErro {
  if (typeof valor !== "object" || valor === null) return false;
  const { message, detail } = valor as { message?: unknown; detail?: unknown };
  return (
    typeof message === "string" &&
    (detail === undefined || detail === null || typeof detail === "string")
  );
}

/**
 * Único ponto de acesso HTTP da aplicação. As páginas nunca chamam `fetch`
 * diretamente: usam um Service, que usa este cliente.
 *
 * - Junta o token (Bearer) a todos os pedidos, exceto os `publico`.
 * - Em 401 avisa quem se registou com `definirAoNaoAutorizado`
 *   (o AuthContext, que volta ao login).
 * - Converte qualquer falha num `ApiRequestError` com categoria e mensagem.
 */
/** Um pedido sem resposta durante este tempo falha (não deixa o ecrã preso). */
export const TEMPO_LIMITE_PEDIDO_MS = 30_000;

export class ApiClient {
  private readonly baseUrl: string;
  private readonly fetchFn: typeof fetch;
  private readonly timeoutMs: number;
  private token: string | null = null;
  private aoNaoAutorizado: (() => void) | null = null;

  constructor(
    baseUrl: string,
    fetchFn?: typeof fetch,
    timeoutMs: number = TEMPO_LIMITE_PEDIDO_MS,
  ) {
    this.baseUrl = baseUrl;
    this.fetchFn = fetchFn ?? ((input, init) => fetch(input, init));
    this.timeoutMs = timeoutMs;
  }

  setToken(token: string | null): void {
    this.token = token;
  }

  definirAoNaoAutorizado(callback: (() => void) | null): void {
    this.aoNaoAutorizado = callback;
  }

  get<T>(caminho: string, opcoes?: PedidoOpcoes): Promise<T> {
    return this.pedirDados<T>("GET", caminho, undefined, opcoes);
  }

  post<T>(caminho: string, corpo?: unknown, opcoes?: PedidoOpcoes): Promise<T> {
    return this.pedirDados<T>("POST", caminho, corpo, opcoes);
  }

  put<T>(caminho: string, corpo?: unknown, opcoes?: PedidoOpcoes): Promise<T> {
    return this.pedirDados<T>("PUT", caminho, corpo, opcoes);
  }

  patch<T>(
    caminho: string,
    corpo?: unknown,
    opcoes?: PedidoOpcoes,
  ): Promise<T> {
    return this.pedirDados<T>("PATCH", caminho, corpo, opcoes);
  }

  delete<T>(caminho: string, opcoes?: PedidoOpcoes): Promise<T> {
    return this.pedirDados<T>("DELETE", caminho, undefined, opcoes);
  }

  /** Como os métodos acima, mas devolve também os cabeçalhos da resposta. */
  pedirComCabecalhos<T>(
    metodo: Metodo,
    caminho: string,
    corpo?: unknown,
    opcoes?: PedidoOpcoes,
  ): Promise<RespostaComCabecalhos<T>> {
    return this.pedir<T>(metodo, caminho, corpo, opcoes);
  }

  private async pedirDados<T>(
    metodo: Metodo,
    caminho: string,
    corpo?: unknown,
    opcoes?: PedidoOpcoes,
  ): Promise<T> {
    return (await this.pedir<T>(metodo, caminho, corpo, opcoes)).dados;
  }

  private construirUrl(caminho: string, query?: PedidoOpcoes["query"]): string {
    const parametros = new URLSearchParams();
    for (const [chave, valor] of Object.entries(query ?? {})) {
      if (valor !== null && valor !== undefined) {
        parametros.set(chave, String(valor));
      }
    }
    const texto = parametros.toString();
    const barra = caminho.startsWith("/") ? "" : "/";
    return `${this.baseUrl}${barra}${caminho}${texto ? `?${texto}` : ""}`;
  }

  private async pedir<T>(
    metodo: Metodo,
    caminho: string,
    corpo?: unknown,
    opcoes: PedidoOpcoes = {},
  ): Promise<RespostaComCabecalhos<T>> {
    if (!this.baseUrl) {
      throw new ApiRequestError(0, "configuracao");
    }

    const cabecalhos: Record<string, string> = { Accept: "application/json" };
    if (corpo !== undefined) cabecalhos["Content-Type"] = "application/json";
    if (this.token && !opcoes.publico) {
      cabecalhos["Authorization"] = `Bearer ${this.token}`;
    }
    Object.assign(cabecalhos, opcoes.headers);

    const limite = AbortSignal.timeout(this.timeoutMs);
    const sinal = opcoes.signal
      ? AbortSignal.any([limite, opcoes.signal])
      : limite;

    let resposta: Response;
    try {
      resposta = await this.fetchFn(this.construirUrl(caminho, opcoes.query), {
        method: metodo,
        headers: cabecalhos,
        body: corpo === undefined ? undefined : JSON.stringify(corpo),
        signal: sinal,
      });
    } catch (e) {
      if (e instanceof DOMException && e.name === "TimeoutError") {
        throw new ApiRequestError(
          0,
          "rede",
          "O servidor demorou demasiado a responder. Tente novamente.",
        );
      }
      // Pedido cancelado de propósito: deixamos passar tal como veio.
      if (e instanceof DOMException && e.name === "AbortError") throw e;
      throw new ApiRequestError(0, "rede");
    }

    if (resposta.ok) {
      return {
        dados: (await this.lerCorpoSucesso(resposta)) as T,
        cabecalhos: resposta.headers,
      };
    }

    // 401 num pedido que levava token = a sessão expirou ou foi revogada.
    // (Sem token não havia sessão para terminar.)
    if (resposta.status === 401 && !opcoes.publico && this.token) {
      this.aoNaoAutorizado?.();
    }
    throw await this.construirErro(resposta);
  }

  private async lerCorpoSucesso(resposta: Response): Promise<unknown> {
    if (resposta.status === 204) return undefined;
    const texto = await resposta.text();
    if (texto === "") return undefined;
    try {
      return JSON.parse(texto);
    } catch {
      throw new ApiRequestError(resposta.status, "desconhecido");
    }
  }

  private async construirErro(resposta: Response): Promise<ApiRequestError> {
    const categoria: CategoriaErro = categoriaDeStatus(resposta.status);

    // Nos erros 5xx nunca mostramos o texto do backend: pode ter detalhes
    // técnicos que não interessam (nem devem chegar) ao utilizador.
    if (categoria === "servidor") {
      return new ApiRequestError(resposta.status, categoria);
    }

    let corpo: unknown = null;
    try {
      corpo = await resposta.json();
    } catch {
      // Corpo vazio ou que não é JSON: usamos a mensagem por omissão.
    }
    if (eApiErro(corpo)) {
      // API v03 §5: `message` é fixo por código (ex.: «Pedido inválido.») e o
      // texto específico vem em `detail` (ex.: as mensagens dos campos num 400).
      // Mostramos o `detail` quando existe; senão o `message`.
      const detail = corpo.detail?.trim();
      return new ApiRequestError(
        resposta.status,
        categoria,
        detail ? detail : corpo.message,
        corpo,
      );
    }
    return new ApiRequestError(resposta.status, categoria);
  }
}

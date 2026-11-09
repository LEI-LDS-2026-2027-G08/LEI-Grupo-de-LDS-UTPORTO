import { apiClient, ApiRequestError, type ApiClient } from "../core/api";
import type {
  CredenciaisLogin,
  SessaoAutenticada,
  UtilizadorAutenticado,
} from "../models/auth";

/**
 * Mensagens do login (RF002). Nunca dizem qual dos campos está errado,
 * para não revelar se um email existe.
 */
function traduzirErroLogin(erro: unknown): unknown {
  if (!(erro instanceof ApiRequestError)) return erro;
  switch (erro.categoria) {
    case "naoAutenticado": // 401 — credenciais incorretas
      return new ApiRequestError(
        erro.status,
        erro.categoria,
        "Email ou palavra-passe incorretos.",
        erro.erro,
      );
    case "semPermissao": // 403 — conta bloqueada
      return new ApiRequestError(
        erro.status,
        erro.categoria,
        "Esta conta está bloqueada. Contacte o administrador da clínica.",
        erro.erro,
      );
    case "validacao": // 400 — campos em falta
      return new ApiRequestError(
        erro.status,
        erro.categoria,
        "Indique o email e a palavra-passe.",
        erro.erro,
      );
    default:
      return erro;
  }
}

/** Chamadas ao backend para autenticação (EP-001, EP-002, EP-003). */
export class AuthService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-001 — público: não leva token e um 401 significa "credenciais erradas". */
  async login(credenciais: CredenciaisLogin): Promise<SessaoAutenticada> {
    try {
      return await this.api.post<SessaoAutenticada>(
        "/auth/login",
        credenciais,
        { publico: true },
      );
    } catch (erro) {
      throw traduzirErroLogin(erro);
    }
  }

  /** EP-002 — invalida o token no servidor (204). */
  async logout(): Promise<void> {
    await this.api.post<void>("/auth/logout");
  }

  /** EP-003 — confirma que o token ainda é válido e devolve o utilizador. */
  me(): Promise<UtilizadorAutenticado> {
    return this.api.get<UtilizadorAutenticado>("/auth/me");
  }
}

export const authService = new AuthService();

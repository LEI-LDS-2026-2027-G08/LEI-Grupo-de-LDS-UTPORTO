import { apiClient, type ApiClient } from "../core/api";
import type {
  CredenciaisLogin,
  SessaoAutenticada,
  UtilizadorAutenticado,
} from "../models/auth";

/** Chamadas ao backend para autenticação (EP-001, EP-002, EP-003). */
export class AuthService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-001 — público: não leva token e um 401 significa "credenciais erradas". */
  login(credenciais: CredenciaisLogin): Promise<SessaoAutenticada> {
    return this.api.post<SessaoAutenticada>("/auth/login", credenciais, {
      publico: true,
    });
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

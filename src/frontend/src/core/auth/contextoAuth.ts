import { createContext } from "react";
import type {
  CredenciaisLogin,
  PerfilWeb,
  SessaoAutenticada,
  UtilizadorAutenticado,
} from "../../models/auth";

export interface AuthContextValue {
  sessao: SessaoAutenticada | null;
  utilizador: UtilizadorAutenticado | null;
  /** true enquanto se confirma com o backend uma sessão guardada (EP-003). */
  aRestaurar: boolean;
  /** Faz login. Lança ApiRequestError se falhar ou se o perfil não for web. */
  login: (credenciais: CredenciaisLogin) => Promise<UtilizadorAutenticado>;
  /** Termina a sessão no servidor (best effort) e limpa-a localmente. */
  logout: () => Promise<void>;
  /** O utilizador atual tem um destes perfis? */
  temPerfil: (perfis: readonly PerfilWeb[]) => boolean;
}

export const AuthContext = createContext<AuthContextValue | null>(null);

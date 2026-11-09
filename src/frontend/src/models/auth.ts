/**
 * Perfis que podem usar a aplicação web. O perfil "Cliente" existe no
 * backend, mas só usa a aplicação móvel (RNF003).
 */
export const PERFIS_WEB = [
  "Administrador",
  "Rececionista",
  "Veterinario",
  "AuxiliarVeterinario",
] as const;

export type PerfilWeb = (typeof PERFIS_WEB)[number];
export type Perfil = PerfilWeb | "Cliente";

/** Indica se o perfil pode entrar na aplicação web. */
export function ePerfilWeb(perfil: string): perfil is PerfilWeb {
  return (PERFIS_WEB as readonly string[]).includes(perfil);
}

export interface CredenciaisLogin {
  email: string;
  password: string;
}

export interface UtilizadorAutenticado {
  id: string | number;
  nome: string;
  email: string;
  perfil: Perfil;
  /** Só preenchido quando o perfil é Cliente. */
  clienteId?: string | number | null;
}

export interface SessaoAutenticada {
  token: string;
  /** Data/hora de expiração do token, em ISO 8601. */
  expiraEm: string;
  utilizador: UtilizadorAutenticado;
}

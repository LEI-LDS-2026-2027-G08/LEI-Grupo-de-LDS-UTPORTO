import { PERFIS_WEB, type PerfilWeb } from "./auth";

/**
 * Perfis que se podem atribuir a uma conta de funcionário (EP-006/EP-007).
 * O perfil Cliente não é aceite nestes endpoints (Modelo de Classes Web, §10.1).
 */
export type PerfilInterno = PerfilWeb;
export const PERFIS_INTERNOS: readonly PerfilInterno[] = PERFIS_WEB;

export const ESTADOS_UTILIZADOR = ["Ativo", "Bloqueado"] as const;
export type EstadoUtilizador = (typeof ESTADOS_UTILIZADOR)[number];

/** Nome de cada perfil para mostrar ao utilizador. */
export const ROTULO_PERFIL: Record<PerfilInterno, string> = {
  Administrador: "Administrador",
  Rececionista: "Rececionista",
  Veterinario: "Veterinário",
  AuxiliarVeterinario: "Auxiliar Veterinário",
};

/** Só o Auxiliar Veterinário pode ter a autorização de registo de apoio. */
export function perfilTemAutorizacaoApoio(perfil: PerfilInterno): boolean {
  return perfil === "AuxiliarVeterinario";
}

/** Item da lista de funcionários (EP-004) e resposta da criação (EP-006, 201). */
export interface FuncionarioResumo {
  id: number;
  nome: string;
  email: string;
  perfil: PerfilInterno;
  estado: EstadoUtilizador;
  podeRegistarApoio: boolean;
}

/** Conta de funcionário (EP-005 e resposta de EP-007). Nunca traz a palavra-passe. */
export interface FuncionarioDetalhe extends FuncionarioResumo {
  telefone: string | null;
  /** Motivo do bloqueio (EP-008), ou null numa conta ativa ou bloqueada sem motivo. */
  motivoBloqueio: string | null;
  /** Data de criação, em ISO 8601 (UTC). */
  criadoEm: string;
}

/** Filtros da lista (EP-004). */
export interface UtilizadorFiltros {
  perfil?: PerfilInterno;
  estado?: EstadoUtilizador;
  pesquisa?: string;
  pagina: number;
  tamanho: number;
}

/** Corpo de EP-007. `podeRegistarApoio` só é enviado para o Auxiliar Veterinário. */
export interface AtualizarFuncionarioRequest {
  nome: string;
  email: string;
  telefone?: string | null;
  perfil: PerfilInterno;
  podeRegistarApoio?: boolean;
}

/** Corpo de EP-006: os campos de EP-007 e a palavra-passe inicial. */
export interface CriarFuncionarioRequest extends AtualizarFuncionarioRequest {
  passwordInicial: string;
}

/** Corpo de EP-008. O único estado aceite é Bloqueado (não há desbloqueio). */
export interface AtualizarEstadoUtilizadorRequest {
  estado: "Bloqueado";
  motivo?: string;
}

/** Estado do formulário W03 (criar e editar). */
export interface FuncionarioFormState {
  nome: string;
  email: string;
  telefone: string;
  perfil: PerfilInterno;
  podeRegistarApoio: boolean;
  /** Só ao criar; nunca é guardada fora do formulário. */
  passwordInicial: string;
}

/**
 * Tipos da vacinação e desparasitação no web (EP-042, EP-043 e a atualização
 * do RF056), iguais aos DTOs da API (BoletimDto, AdministracaoDto,
 * ProximaAdministracaoDto e Create/UpdateAdministracaoDto). As datas são dias
 * (aaaa-mm-dd). A próxima administração é sempre calculada pelo servidor
 * (RF045): o frontend nunca a calcula nem a envia.
 */

export const TIPOS_ADMINISTRACAO = ["Vacina", "Desparasitacao"] as const;
export type TipoAdministracao = (typeof TIPOS_ADMINISTRACAO)[number];

export const ROTULO_TIPO_ADMINISTRACAO: Record<TipoAdministracao, string> = {
  Vacina: "Vacina",
  Desparasitacao: "Desparasitação",
};

/** Vacinação ou desparasitação registada (EP-042 e resposta de EP-043). */
export interface Administracao {
  id: number;
  tipo: TipoAdministracao;
  produto: string;
  lote: string;
  dataAdministracao: string;
  intervaloDias: number | null;
  /** dataAdministracao + intervaloDias, calculada pela API; null sem intervalo. */
  dataProximaPrevista: string | null;
  /** Nome do Veterinário que registou. */
  veterinario: string;
}

/** Próxima administração prevista a partir de hoje (EP-042). */
export interface ProximaAdministracao {
  tipo: TipoAdministracao;
  produto: string;
  dataProximaPrevista: string;
}

/** Resposta de EP-042. */
export interface Boletim {
  animal: { id: number; nome: string; especie: string | null };
  /** Da mais recente para a mais antiga. */
  administracoes: Administracao[];
  /** Da mais próxima para a mais distante. */
  proximas: ProximaAdministracao[];
}

/** Corpo de EP-043 e da atualização (RF056). */
export interface AdministracaoRequest {
  tipo: TipoAdministracao;
  produto: string;
  lote: string;
  dataAdministracao: string;
  intervaloDias: number | null;
}

/** Estado do formulário (texto, como vem dos campos). */
export interface AdministracaoForm {
  tipo: TipoAdministracao;
  produto: string;
  lote: string;
  dataAdministracao: string;
  /** Vazio = sem próxima administração. */
  intervaloDias: string;
}

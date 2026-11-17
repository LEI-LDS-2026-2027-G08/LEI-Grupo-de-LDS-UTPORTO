/**
 * Tipos da agenda (EP-021 a EP-027), iguais aos DTOs da API
 * (VeterinarioRefDto, DisponibilidadeDto, ConsultaDto, CreateConsultaDto,
 * UpdateConsultaDto e UpdateEstadoConsultaDto). As datas e horas das
 * marcações vêm em UTC (ISO 8601); a disponibilidade vem em hora de Lisboa.
 * A versão da marcação não é um campo: viaja no cabeçalho ETag (§9.2).
 */

export const ESTADOS_CONSULTA = [
  "Pendente",
  "Confirmada",
  "Realizada",
  "Cancelada",
] as const;
export type EstadoConsulta = (typeof ESTADOS_CONSULTA)[number];

/** Estados que a Rececionista pode escolher (Pendente só existe ao criar). */
export type EstadoMarcacaoAtualizavel = Exclude<EstadoConsulta, "Pendente">;

/**
 * Transições permitidas (RF023 e Documentação de API, §8). Serve só para a
 * interface mostrar os botões certos: a API valida e recusa (422) as outras.
 */
export const TRANSICOES_ESTADO: Readonly<
  Record<EstadoConsulta, readonly EstadoMarcacaoAtualizavel[]>
> = {
  Pendente: ["Confirmada", "Cancelada"],
  Confirmada: ["Realizada", "Cancelada"],
  Realizada: [],
  Cancelada: [],
};

/** Só as marcações Pendente ou Confirmada podem ser alteradas (EP-026). */
export function marcacaoAlteravel(estado: EstadoConsulta): boolean {
  return estado === "Pendente" || estado === "Confirmada";
}

export interface VeterinarioResumo {
  id: number;
  nome: string;
}

export interface DiaDisponibilidade {
  /** Dia em Lisboa (aaaa-mm-dd). */
  data: string;
  /** Inícios livres em hora de Lisboa (HH:mm), por ordem crescente. */
  horariosLivres: string[];
}

/** Resposta de EP-022. */
export interface Disponibilidade {
  veterinarioId: number;
  duracaoConsultaMinutos: number;
  dias: DiaDisponibilidade[];
}

export interface AnimalMarcacao {
  id: number;
  nome: string;
  especie: string | null;
}

export interface ClienteMarcacao {
  id: number;
  nome: string;
}

/** Nota não clínica de apoio registada por um Auxiliar (EP-024 e EP-029). */
export interface NotaPreparacao {
  id: number;
  /** Só vem na resposta de EP-029. */
  consultaId?: number | null;
  texto: string;
  autor: string;
  criadaEm: string;
}

/** Marcação na agenda (EP-023). */
export interface MarcacaoResumo {
  id: number;
  /** Início da consulta em UTC (ISO 8601). */
  dataHora: string;
  duracaoMinutos: number;
  estado: EstadoConsulta;
  motivo: string | null;
  animal: AnimalMarcacao;
  cliente: ClienteMarcacao;
  veterinario: VeterinarioResumo;
  /** Hora de chegada (UTC) registada na receção (EP-028), ou null. */
  chegadaEm: string | null;
}

/** Marcação (EP-024, e resposta de EP-025 e EP-026). */
export interface MarcacaoDetalhe extends MarcacaoResumo {
  notasPreparacao: NotaPreparacao[];
  /** Observação da última alteração de estado (EP-027), ou null. */
  observacao: string | null;
}

/** Parâmetros de EP-023: dias de Lisboa (aaaa-mm-dd), ambos incluídos. */
export interface AgendaFiltros {
  dataInicio?: string;
  dataFim?: string;
  /** Ignorado pela API quando o perfil é Veterinário. */
  veterinarioId?: number;
  animalId?: number;
  estado?: EstadoConsulta;
  pagina: number;
  tamanho: number;
}

/** Resposta de EP-028. */
export interface ChegadaConsulta {
  id: number;
  chegadaEm: string;
}

/** Corpo de EP-025 (POST /consultas). */
export interface CriarMarcacaoRequest {
  animalId: number;
  veterinarioId: number;
  /** Início em UTC (ISO 8601). */
  dataHora: string;
  motivo: string | null;
}

/** Corpo de EP-026 (PUT /consultas/{id}): o paciente não muda. */
export type AlterarMarcacaoRequest = Omit<CriarMarcacaoRequest, "animalId">;

/** Corpo de EP-027 (PATCH /consultas/{id}/estado). */
export interface EstadoMarcacaoForm {
  estado: EstadoMarcacaoAtualizavel;
  observacao: string | null;
}

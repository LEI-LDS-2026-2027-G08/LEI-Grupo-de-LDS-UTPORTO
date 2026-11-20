/**
 * Tipos do registo clínico (EP-031 a EP-034), iguais aos DTOs da API
 * (RegistoClinicoDto, CreateRegistoClinicoDto, UpdateRegistoClinicoDto,
 * TratamentoDto, PrescricaoDto e HistoricoClinicoDto). As datas de criação e
 * alteração vêm em UTC (ISO 8601); a data de aplicação de um tratamento e a
 * data da consulta no histórico são dias (aaaa-mm-dd).
 */

/** Só os registos de consultas Confirmada ou Realizada (EP-031, 422). */
export const ESTADOS_COM_REGISTO_CLINICO = ["Confirmada", "Realizada"] as const;

export interface Tratamento {
  id: number;
  descricao: string;
  /** Dia de aplicação (aaaa-mm-dd). */
  dataAplicacao: string;
}

export interface Prescricao {
  id: number;
  medicamento: string;
  dosagem: string;
  frequencia: string;
  duracaoDias: number | null;
}

/** Registo clínico de uma consulta (EP-031, EP-032 e EP-033). */
export interface RegistoClinico {
  id: number;
  consultaId: number;
  animalId: number;
  /** Nome do Veterinário que criou o registo. */
  veterinario: string;
  criadoEm: string;
  notasClinicas: string | null;
  diagnostico: string | null;
  tratamentos: Tratamento[];
  prescricoes: Prescricao[];
  atualizadoEm: string | null;
  atualizadoPor: string | null;
}

/** Tratamento enviado em EP-031/EP-032: com `id`, mantém um existente. */
export interface TratamentoRequest {
  id?: number;
  descricao: string;
  dataAplicacao: string;
}

/** Prescrição enviada em EP-031/EP-032: com `id`, mantém uma existente. */
export interface PrescricaoRequest {
  id?: number;
  medicamento: string;
  dosagem: string;
  frequencia: string;
  duracaoDias: number | null;
}

/**
 * Corpo de EP-032 (PUT /registos-clinicos/{id}). As listas substituem as
 * anteriores: o que não vier é removido.
 */
export interface AtualizarRegistoClinicoRequest {
  notasClinicas: string | null;
  diagnostico: string | null;
  tratamentos: TratamentoRequest[];
  prescricoes: PrescricaoRequest[];
}

/** Corpo de EP-031 (POST /registos-clinicos). */
export interface CriarRegistoClinicoRequest extends AtualizarRegistoClinicoRequest {
  consultaId: number;
}

/** Linha do histórico clínico de um paciente (EP-034). */
export interface RegistoClinicoResumo {
  id: number;
  consultaId: number;
  /** Dia da consulta em Lisboa (aaaa-mm-dd). */
  data: string;
  veterinario: string;
  diagnostico: string | null;
  numeroTratamentos: number;
  numeroPrescricoes: number;
}

/** Resposta de EP-034. */
export interface HistoricoClinico {
  animal: { id: number; nome: string; especie: string | null };
  registos: RegistoClinicoResumo[];
}

/** Período opcional de EP-034: dias de Lisboa, os dois incluídos. */
export interface HistoricoClinicoFiltros {
  dataInicio?: string;
  dataFim?: string;
}

/* ---- Estado do formulário de W13 (texto, como vem dos campos) ---- */

export interface TratamentoForm {
  /** Chave local da linha (para o React), mesmo antes de ter id na API. */
  chave: string;
  id?: number;
  descricao: string;
  dataAplicacao: string;
}

export interface PrescricaoForm {
  chave: string;
  id?: number;
  medicamento: string;
  dosagem: string;
  frequencia: string;
  /** Texto do campo; vazio = sem duração. */
  duracaoDias: string;
}

export interface RegistoClinicoForm {
  notasClinicas: string;
  diagnostico: string;
  tratamentos: TratamentoForm[];
  prescricoes: PrescricaoForm[];
}

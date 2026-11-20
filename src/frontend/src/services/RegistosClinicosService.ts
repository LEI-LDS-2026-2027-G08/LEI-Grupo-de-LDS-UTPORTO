import { apiClient, type ApiClient } from "../core/api";
import type {
  AtualizarRegistoClinicoRequest,
  CriarRegistoClinicoRequest,
  HistoricoClinico,
  HistoricoClinicoFiltros,
  PrescricaoRequest,
  RegistoClinico,
  TratamentoRequest,
} from "../models/registosClinicos";

/** Texto opcional aparado; vazio passa a null. */
function textoOpcional(texto: string | null): string | null {
  const limpo = texto?.trim() ?? "";
  return limpo === "" ? null : limpo;
}

function corpoTratamento(t: TratamentoRequest): TratamentoRequest {
  return {
    ...(t.id ? { id: t.id } : {}),
    descricao: t.descricao.trim(),
    dataAplicacao: t.dataAplicacao,
  };
}

function corpoPrescricao(p: PrescricaoRequest): PrescricaoRequest {
  return {
    ...(p.id ? { id: p.id } : {}),
    medicamento: p.medicamento.trim(),
    dosagem: p.dosagem.trim(),
    frequencia: p.frequencia.trim(),
    duracaoDias: p.duracaoDias,
  };
}

/** Corpo de EP-031/EP-032: textos aparados e opcionais vazios a null. */
function corpoRegisto(
  dados: AtualizarRegistoClinicoRequest,
): AtualizarRegistoClinicoRequest {
  return {
    notasClinicas: textoOpcional(dados.notasClinicas),
    diagnostico: textoOpcional(dados.diagnostico),
    tratamentos: dados.tratamentos.map(corpoTratamento),
    prescricoes: dados.prescricoes.map(corpoPrescricao),
  };
}

/** Chamadas ao backend para o registo clínico (EP-031 a EP-034). */
export class RegistosClinicosService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-031 — cria o registo clínico da consulta (201). Só Veterinário. */
  criar(dados: CriarRegistoClinicoRequest): Promise<RegistoClinico> {
    return this.api.post<RegistoClinico>("/registos-clinicos", {
      consultaId: dados.consultaId,
      ...corpoRegisto(dados),
    });
  }

  /**
   * EP-032 — atualiza o registo. As listas substituem as anteriores: os
   * elementos existentes mantêm-se pelo `id`. Só Veterinário.
   */
  atualizar(
    id: number,
    dados: AtualizarRegistoClinicoRequest,
  ): Promise<RegistoClinico> {
    return this.api.put<RegistoClinico>(
      `/registos-clinicos/${id}`,
      corpoRegisto(dados),
    );
  }

  /** EP-033 — registo completo (Veterinário e Auxiliar Veterinário). */
  obter(id: number): Promise<RegistoClinico> {
    return this.api.get<RegistoClinico>(`/registos-clinicos/${id}`);
  }

  /** EP-034 — histórico clínico do paciente, do mais recente para o mais antigo. */
  obterHistorico(
    animalId: number,
    filtros: HistoricoClinicoFiltros = {},
  ): Promise<HistoricoClinico> {
    return this.api.get<HistoricoClinico>(
      `/animais/${animalId}/historico-clinico`,
      {
        query: {
          dataInicio: filtros.dataInicio || undefined,
          dataFim: filtros.dataFim || undefined,
        },
      },
    );
  }

  /**
   * Registo clínico de uma consulta, ou null se ainda não tem. A API não liga
   * a consulta ao registo (ConsultaDto não tem registoClinicoId), por isso
   * procura-se no histórico do paciente (EP-034) e lê-se o registo (EP-033).
   */
  async obterDaConsulta(
    animalId: number,
    consultaId: number,
  ): Promise<RegistoClinico | null> {
    const historico = await this.obterHistorico(animalId);
    const linha = historico.registos.find((r) => r.consultaId === consultaId);
    return linha ? this.obter(linha.id) : null;
  }
}

export type ServicoRegistosClinicos = Pick<
  RegistosClinicosService,
  "criar" | "atualizar" | "obter" | "obterHistorico" | "obterDaConsulta"
>;

export const registosClinicosService = new RegistosClinicosService();

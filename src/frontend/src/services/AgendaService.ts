import {
  apiClient,
  type ApiClient,
  type RespostaComCabecalhos,
} from "../core/api";
import type {
  AlterarMarcacaoRequest,
  CriarMarcacaoRequest,
  Disponibilidade,
  EstadoMarcacaoForm,
  MarcacaoDetalhe,
  VeterinarioResumo,
} from "../models/agenda";

/** Texto opcional aparado (motivo, observação); vazio passa a null. */
function textoOpcional(motivo: string | null): string | null {
  const limpo = motivo?.trim() ?? "";
  return limpo === "" ? null : limpo;
}

/**
 * Chamadas ao backend para a agenda (EP-021 a EP-027).
 *
 * Guarda em memória o último ETag de cada marcação (EP-024 a EP-027) e envia-o
 * em If-Match ao alterar (Modelo de Classes Web, §9.2). Uma resposta 412
 * significa que a marcação mudou entretanto: a página volta a lê-la.
 */
export class AgendaService {
  private readonly api: ApiClient;
  private readonly versoes = new Map<number, string>();

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  private guardarVersao(id: number, resposta: RespostaComCabecalhos<unknown>) {
    const etag = resposta.cabecalhos.get("ETag");
    if (etag) this.versoes.set(id, etag);
  }

  private cabecalhosVersao(id: number): Record<string, string> | undefined {
    const etag = this.versoes.get(id);
    return etag ? { "If-Match": etag } : undefined;
  }

  /** EP-021 — Veterinários ativos. */
  listarVeterinarios(): Promise<VeterinarioResumo[]> {
    return this.api.get<VeterinarioResumo[]>("/veterinarios");
  }

  /** EP-022 — horários livres de um Veterinário entre dois dias (Lisboa). */
  obterDisponibilidade(
    veterinarioId: number,
    dataInicio: string,
    dataFim?: string,
  ): Promise<Disponibilidade> {
    return this.api.get<Disponibilidade>(
      `/veterinarios/${veterinarioId}/disponibilidade`,
      { query: { dataInicio, dataFim } },
    );
  }

  /** EP-024 — marcação, guardando a versão (ETag). */
  async obterConsulta(id: number): Promise<MarcacaoDetalhe> {
    const resposta = await this.api.pedirComCabecalhos<MarcacaoDetalhe>(
      "GET",
      `/consultas/${id}`,
    );
    this.guardarVersao(id, resposta);
    return resposta.dados;
  }

  /** EP-025 — cria a marcação no estado Pendente (201). */
  async criar(dados: CriarMarcacaoRequest): Promise<MarcacaoDetalhe> {
    const resposta = await this.api.pedirComCabecalhos<MarcacaoDetalhe>(
      "POST",
      "/consultas",
      { ...dados, motivo: textoOpcional(dados.motivo) },
    );
    this.guardarVersao(resposta.dados.id, resposta);
    return resposta.dados;
  }

  /** EP-026 — altera Veterinário, data e hora ou motivo, com If-Match. */
  async atualizar(
    id: number,
    dados: AlterarMarcacaoRequest,
  ): Promise<MarcacaoDetalhe> {
    const resposta = await this.api.pedirComCabecalhos<MarcacaoDetalhe>(
      "PUT",
      `/consultas/${id}`,
      { ...dados, motivo: textoOpcional(dados.motivo) },
      { headers: this.cabecalhosVersao(id) },
    );
    this.guardarVersao(id, resposta);
    return resposta.dados;
  }

  /** EP-027 — muda o estado (204), com If-Match. */
  async alterarEstado(id: number, dados: EstadoMarcacaoForm): Promise<void> {
    const resposta = await this.api.pedirComCabecalhos<void>(
      "PATCH",
      `/consultas/${id}/estado`,
      { estado: dados.estado, observacao: textoOpcional(dados.observacao) },
      { headers: this.cabecalhosVersao(id) },
    );
    this.guardarVersao(id, resposta);
  }
}

export type ServicoAgenda = Pick<
  AgendaService,
  | "listarVeterinarios"
  | "obterDisponibilidade"
  | "obterConsulta"
  | "criar"
  | "atualizar"
  | "alterarEstado"
>;

export const agendaService = new AgendaService();

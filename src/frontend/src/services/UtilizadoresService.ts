import { apiClient, type ApiClient } from "../core/api";
import type { PagedResult } from "../models/common";
import {
  perfilTemAutorizacaoApoio,
  type AtualizarEstadoUtilizadorRequest,
  type AtualizarFuncionarioRequest,
  type CriarFuncionarioRequest,
  type FuncionarioDetalhe,
  type FuncionarioResumo,
  type UtilizadorFiltros,
} from "../models/utilizadores";

/**
 * Prepara o corpo de EP-006/EP-007: o telefone vazio vai como null e a
 * autorização de apoio só é enviada quando o perfil é Auxiliar Veterinário
 * (para os outros perfis a API responderia 422).
 */
function corpoFuncionario<T extends AtualizarFuncionarioRequest>(dados: T): T {
  const { podeRegistarApoio, telefone, ...resto } = dados;
  const telefoneLimpo = telefone?.trim() ? telefone.trim() : null;
  return {
    ...resto,
    telefone: telefoneLimpo,
    ...(perfilTemAutorizacaoApoio(dados.perfil)
      ? { podeRegistarApoio: podeRegistarApoio ?? false }
      : {}),
  } as T;
}

/** Chamadas ao backend para as contas de funcionários (EP-004 a EP-008). */
export class UtilizadoresService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-004 — lista paginada, com filtros opcionais. */
  listar(filtros: UtilizadorFiltros): Promise<PagedResult<FuncionarioResumo>> {
    return this.api.get<PagedResult<FuncionarioResumo>>("/utilizadores", {
      query: {
        perfil: filtros.perfil,
        estado: filtros.estado,
        pesquisa: filtros.pesquisa?.trim() || undefined,
        pagina: filtros.pagina,
        tamanho: filtros.tamanho,
      },
    });
  }

  /** EP-005 — detalhe de uma conta. */
  obter(id: number): Promise<FuncionarioDetalhe> {
    return this.api.get<FuncionarioDetalhe>(`/utilizadores/${id}`);
  }

  /** EP-006 — cria a conta (resposta 201 com os campos da lista). */
  criar(dados: CriarFuncionarioRequest): Promise<FuncionarioResumo> {
    return this.api.post<FuncionarioResumo>(
      "/utilizadores",
      corpoFuncionario(dados),
    );
  }

  /** EP-007 — atualiza os dados e o perfil; não envia palavra-passe. */
  atualizar(
    id: number,
    dados: AtualizarFuncionarioRequest,
  ): Promise<FuncionarioDetalhe> {
    return this.api.put<FuncionarioDetalhe>(
      `/utilizadores/${id}`,
      corpoFuncionario(dados),
    );
  }

  /** EP-008 — bloqueia a conta (204). O motivo é opcional. */
  async bloquear(id: number, motivo?: string): Promise<void> {
    const corpo: AtualizarEstadoUtilizadorRequest = { estado: "Bloqueado" };
    if (motivo?.trim()) corpo.motivo = motivo.trim();
    await this.api.patch<void>(`/utilizadores/${id}/estado`, corpo);
  }
}

export type ServicoUtilizadores = Pick<
  UtilizadoresService,
  "listar" | "obter" | "criar" | "atualizar" | "bloquear"
>;

export const utilizadoresService = new UtilizadoresService();

import { apiClient, type ApiClient } from "../core/api";
import type {
  Administracao,
  AdministracaoRequest,
  Boletim,
  TipoAdministracao,
} from "../models/vacinacoes";

/** Corpo de EP-043/RF056: textos aparados. A próxima prevista nunca é enviada. */
function corpoAdministracao(dados: AdministracaoRequest): AdministracaoRequest {
  return {
    tipo: dados.tipo,
    produto: dados.produto.trim(),
    lote: dados.lote.trim(),
    dataAdministracao: dados.dataAdministracao,
    intervaloDias: dados.intervaloDias,
  };
}

/**
 * Chamadas ao backend para o boletim de vacinação e desparasitação no web
 * (EP-042, EP-043 e a atualização do RF056). Só o perfil Veterinário.
 */
export class VacinacoesService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-042 — boletim do paciente; `tipo` filtra as duas listas. */
  obterBoletim(animalId: number, tipo?: TipoAdministracao): Promise<Boletim> {
    return this.api.get<Boletim>(`/animais/${animalId}/vacinacoes`, {
      query: { tipo: tipo || undefined },
    });
  }

  /** EP-043 — regista a administração (201) com a próxima prevista calculada. */
  registar(
    animalId: number,
    dados: AdministracaoRequest,
  ): Promise<Administracao> {
    return this.api.post<Administracao>(
      `/animais/${animalId}/vacinacoes`,
      corpoAdministracao(dados),
    );
  }

  /**
   * RF056 (PUT /animais/{id}/vacinacoes/{administracaoId}) — substitui os
   * dados e devolve a próxima prevista recalculada pela API.
   */
  atualizar(
    animalId: number,
    administracaoId: number,
    dados: AdministracaoRequest,
  ): Promise<Administracao> {
    return this.api.put<Administracao>(
      `/animais/${animalId}/vacinacoes/${administracaoId}`,
      corpoAdministracao(dados),
    );
  }
}

export type ServicoVacinacoes = Pick<
  VacinacoesService,
  "obterBoletim" | "registar" | "atualizar"
>;

export const vacinacoesService = new VacinacoesService();

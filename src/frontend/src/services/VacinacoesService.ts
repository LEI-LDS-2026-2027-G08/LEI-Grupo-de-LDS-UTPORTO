import { apiClient, type ApiClient } from "../core/api";
import type {
  Administracao,
  AdministracaoRequest,
  Boletim,
  TipoAdministracao,
} from "../models/vacinacoes";

/** Corpo de EP-043: textos aparados. A próxima prevista nunca é enviada. */
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
 * (EP-042 e EP-043). Só o perfil Veterinário. A alteração de administrações
 * (RF056) ficou excluída da I118 (deliberação 5 do Backlog Refinement de
 * 2026-11-13).
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
}

export type ServicoVacinacoes = Pick<
  VacinacoesService,
  "obterBoletim" | "registar"
>;

export const vacinacoesService = new VacinacoesService();

import { apiClient, type ApiClient } from "../core/api";
import type { PagedResult } from "../models/common";
import type {
  PacienteDetalhe,
  PacienteFiltros,
  PacienteRequest,
  PacienteResumo,
} from "../models/pacientes";

/** Corpo de EP-017 e EP-018: textos aparados e opcionais vazios a null. */
function corpoPaciente(dados: PacienteRequest): PacienteRequest {
  const raca = dados.raca?.trim() ?? "";
  return {
    nome: dados.nome.trim(),
    especie: dados.especie.trim(),
    raca: raca === "" ? null : raca,
    sexo: dados.sexo,
    dataNascimento: dados.dataNascimento || null,
    peso: dados.peso,
    clienteId: dados.clienteId,
  };
}

/** Chamadas ao backend para os pacientes (EP-015 a EP-018). */
export class PacientesService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-015 — lista paginada; proprietário, pesquisa e espécie são opcionais. */
  listar(filtros: PacienteFiltros): Promise<PagedResult<PacienteResumo>> {
    return this.api.get<PagedResult<PacienteResumo>>("/animais", {
      query: {
        clienteId: filtros.clienteId,
        pesquisa: filtros.pesquisa?.trim() || undefined,
        especie: filtros.especie || undefined,
        pagina: filtros.pagina,
        tamanho: filtros.tamanho,
      },
    });
  }

  /** EP-016 — ficha administrativa (sem informação clínica). */
  obter(id: number): Promise<PacienteDetalhe> {
    return this.api.get<PacienteDetalhe>(`/animais/${id}`);
  }

  /** EP-017 — regista o paciente associado a um Cliente existente (201). */
  criar(dados: PacienteRequest): Promise<PacienteDetalhe> {
    return this.api.post<PacienteDetalhe>("/animais", corpoPaciente(dados));
  }

  /** EP-018 — atualiza os dados administrativos. */
  atualizar(id: number, dados: PacienteRequest): Promise<PacienteDetalhe> {
    return this.api.put<PacienteDetalhe>(
      `/animais/${id}`,
      corpoPaciente(dados),
    );
  }
}

export type ServicoPacientes = Pick<
  PacientesService,
  "listar" | "obter" | "criar" | "atualizar"
>;

export const pacientesService = new PacientesService();

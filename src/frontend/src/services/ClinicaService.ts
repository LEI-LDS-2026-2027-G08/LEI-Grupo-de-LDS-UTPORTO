import { apiClient, type ApiClient } from "../core/api";
import type { ClinicaConfig } from "../models/clinica";

/** Chamadas ao backend para os dados, a configuração e o horário da clínica (EP-009 e EP-010). */
export class ClinicaService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-009 — dados gerais, duração das consultas e horário (só Administrador). */
  obter(): Promise<ClinicaConfig> {
    return this.api.get<ClinicaConfig>("/clinica");
  }

  /**
   * EP-010 — grava os dados; o horário enviado substitui o anterior.
   * Devolve o que ficou gravado, no formato de EP-009.
   */
  atualizar(dados: ClinicaConfig): Promise<ClinicaConfig> {
    return this.api.put<ClinicaConfig>("/clinica", dados);
  }
}

export type ServicoClinica = Pick<ClinicaService, "obter" | "atualizar">;

export const clinicaService = new ClinicaService();

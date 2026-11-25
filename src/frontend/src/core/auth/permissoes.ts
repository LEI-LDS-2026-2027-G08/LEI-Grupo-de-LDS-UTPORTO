import type { Perfil, PerfilWeb } from "../../models/auth";

const ADMIN: PerfilWeb = "Administrador";
const REC: PerfilWeb = "Rececionista";
const VET: PerfilWeb = "Veterinario";
const AUX: PerfilWeb = "AuxiliarVeterinario";

/**
 * Ações que a interface restringe por perfil (RF005), tiradas da matriz de
 * acesso do Modelo de Classes Frontend Web (§12 e §13).
 *
 * Quem implementa um ecrã usa estas ações para mostrar/esconder botões e
 * campos de escrita. É só interface: a API continua a ser quem autoriza.
 * Para uma ação nova, acrescente-a aqui.
 */
export const PERMISSOES = {
  /** W02/W03: criar, editar, atribuir perfil e bloquear contas de funcionários. */
  gerirFuncionarios: [ADMIN],
  /** W04/W05: criar, editar e consultar Clientes. */
  gerirClientes: [ADMIN, REC],
  /** W06: dados da clínica, configuração e horários. */
  configurarClinica: [ADMIN],
  /** W07/W08: criar e editar pacientes (escrita administrativa). */
  gerirPacientes: [REC, VET],
  /** W08: ver a ficha do paciente (o Auxiliar só lê). */
  consultarFichaPaciente: [REC, VET, AUX],
  /** W09/W10/W11: criar/alterar marcações, estado e registo de chegada. */
  gerirMarcacoes: [REC],
  /** W13: escrever no registo clínico. */
  registarClinico: [VET],
  /** W13: ler o registo clínico (o Auxiliar só lê o que for autorizado). */
  consultarRegistoClinico: [VET, AUX],
  /** W16: vacinação e desparasitação. */
  registarVacinacao: [VET],
  /** W12: apoio não clínico à consulta. */
  registarApoioNaoClinico: [AUX],
} as const satisfies Record<string, readonly PerfilWeb[]>;

export type Acao = keyof typeof PERMISSOES;

/** Pode este perfil executar esta ação? (O perfil Cliente nunca pode.) */
export function perfilPodeExecutar(perfil: Perfil, acao: Acao): boolean {
  return (PERMISSOES[acao] as readonly string[]).includes(perfil);
}

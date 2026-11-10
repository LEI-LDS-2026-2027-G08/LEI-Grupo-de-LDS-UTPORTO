import type { ComponentType } from "react";
import type { PerfilWeb } from "../models/auth";
import { FuncionarioFormPage } from "../pages/funcionarios/FuncionarioFormPage";
import { FuncionariosPage } from "../pages/funcionarios/FuncionariosPage";

export interface RotaApp {
  /** Código do ecrã no Modelo de Classes (W02, W04, ...). */
  ecra: string;
  titulo: string;
  caminho: string;
  perfis: readonly PerfilWeb[];
  /**
   * Página real. Enquanto não existir, mostra-se o marcador "em construção".
   * Quem implementa o ecrã só tem de preencher este campo.
   */
  pagina?: ComponentType;
}

const ADMIN: PerfilWeb = "Administrador";
const REC: PerfilWeb = "Rececionista";
const VET: PerfilWeb = "Veterinario";
const AUX: PerfilWeb = "AuxiliarVeterinario";

/** Todas as rotas protegidas da aplicação (matriz de acessos, §12). */
export const ROTAS: readonly RotaApp[] = [
  {
    ecra: "W02",
    titulo: "Funcionários",
    caminho: "/funcionarios",
    perfis: [ADMIN],
    pagina: FuncionariosPage,
  },
  {
    ecra: "W03",
    titulo: "Novo funcionário",
    caminho: "/funcionarios/novo",
    perfis: [ADMIN],
    pagina: FuncionarioFormPage,
  },
  {
    ecra: "W03",
    titulo: "Ficha de funcionário",
    caminho: "/funcionarios/:id",
    perfis: [ADMIN],
    pagina: FuncionarioFormPage,
  },
  {
    ecra: "W04",
    titulo: "Clientes",
    caminho: "/clientes",
    perfis: [ADMIN, REC],
  },
  {
    ecra: "W05",
    titulo: "Ficha de cliente",
    caminho: "/clientes/:id",
    perfis: [ADMIN, REC],
  },
  {
    ecra: "W06",
    titulo: "Clínica e horários",
    caminho: "/clinica",
    perfis: [ADMIN],
  },
  {
    ecra: "W07",
    titulo: "Pacientes",
    caminho: "/pacientes",
    perfis: [REC, VET],
  },
  {
    ecra: "W08",
    titulo: "Ficha de paciente",
    caminho: "/pacientes/:id",
    perfis: [REC, VET, AUX],
  },
  { ecra: "W09", titulo: "Agenda", caminho: "/agenda", perfis: [REC, VET] },
  {
    ecra: "W10",
    titulo: "Criar/editar marcação",
    caminho: "/agenda/nova",
    perfis: [REC],
  },
  {
    ecra: "W10",
    titulo: "Editar marcação",
    caminho: "/agenda/:id/editar",
    perfis: [REC],
  },
  {
    ecra: "W11",
    titulo: "Detalhe da marcação",
    caminho: "/agenda/:id",
    perfis: [REC, VET],
  },
  {
    ecra: "W12",
    titulo: "Preparação de consultas",
    caminho: "/preparacao",
    perfis: [AUX],
  },
  {
    ecra: "W13",
    titulo: "Registo clínico",
    caminho: "/pacientes/:id/registo",
    perfis: [VET, AUX],
  },
  {
    ecra: "W16",
    titulo: "Vacinação",
    caminho: "/pacientes/:id/vacinacao",
    perfis: [VET],
  },
];

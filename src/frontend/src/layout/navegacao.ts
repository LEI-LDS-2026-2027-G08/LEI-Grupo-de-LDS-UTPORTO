import type { Perfil, PerfilWeb } from "../models/auth";

export interface ItemMenu {
  rotulo: string;
  caminho: string;
  perfis: readonly PerfilWeb[];
}

/**
 * Itens do menu lateral e quem os vê (matriz de acessos do Modelo de
 * Classes Frontend Web, §12). Para mostrar um novo item a um perfil,
 * basta acrescentar o perfil aqui — e na rota correspondente em `rotas.ts`.
 */
export const ITENS_MENU: readonly ItemMenu[] = [
  {
    rotulo: "Agenda",
    caminho: "/agenda",
    perfis: ["Rececionista", "Veterinario"],
  },
  {
    rotulo: "Preparação de consultas",
    caminho: "/preparacao",
    perfis: ["AuxiliarVeterinario"],
  },
  {
    rotulo: "Pacientes",
    caminho: "/pacientes",
    perfis: ["Rececionista", "Veterinario"],
  },
  {
    rotulo: "Clientes",
    caminho: "/clientes",
    perfis: ["Administrador", "Rececionista"],
  },
  {
    rotulo: "Funcionários",
    caminho: "/funcionarios",
    perfis: ["Administrador"],
  },
  {
    rotulo: "Clínica e horários",
    caminho: "/clinica",
    perfis: ["Administrador"],
  },
];

/** Itens do menu que o perfil indicado pode ver. */
export function itensMenuPara(perfil: Perfil): ItemMenu[] {
  return ITENS_MENU.filter((item) =>
    (item.perfis as readonly string[]).includes(perfil),
  );
}

/** Primeira página a mostrar depois do login (o 1.º item do menu do perfil). */
export function rotaInicial(perfil: Perfil): string {
  return itensMenuPara(perfil)[0]?.caminho ?? "/acesso-negado";
}

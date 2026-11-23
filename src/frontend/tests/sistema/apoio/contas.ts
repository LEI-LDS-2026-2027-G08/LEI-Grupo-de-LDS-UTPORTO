/**
 * Contas de demonstração (I128, secção 5.2). As palavras-passe nunca ficam
 * no repositório: são lidas de variáveis de ambiente (localmente) ou de
 * secrets do GitHub (na pipeline).
 */
export interface Conta {
  email: string;
  /** Nome apresentado no cabeçalho. */
  nome: string;
  /** Perfil como aparece no cabeçalho (ex.: «Auxiliar veterinário»). */
  perfil: string;
  /** Variável de ambiente com a palavra-passe. */
  variavel: string;
}

export const CONTAS = {
  admin: {
    email: "admin@clinica-g08.test",
    nome: "Administração (demonstração)",
    perfil: "Administrador",
    variavel: "E2E_SENHA_ADMIN",
  },
  rececao: {
    email: "rececao@clinica-g08.test",
    nome: "Rita Rececionista (demonstração)",
    perfil: "Rececionista",
    variavel: "E2E_SENHA_RECECAO",
  },
  rececaoBloqueada: {
    email: "rececao.bloqueada@clinica-g08.test",
    nome: "Rui Bloqueado (demonstração)",
    perfil: "Rececionista",
    variavel: "E2E_SENHA_RECECAO_BLOQUEADA",
  },
  vetAna: {
    email: "vet.ana@clinica-g08.test",
    nome: "Dra. Ana Veterinária (demonstração)",
    perfil: "Veterinário",
    variavel: "E2E_SENHA_VET_ANA",
  },
  vetBruno: {
    email: "vet.bruno@clinica-g08.test",
    nome: "Dr. Bruno Veterinário (demonstração)",
    perfil: "Veterinário",
    variavel: "E2E_SENHA_VET_BRUNO",
  },
  auxCarla: {
    email: "aux.carla@clinica-g08.test",
    nome: "Carla Auxiliar (com apoio)",
    perfil: "Auxiliar veterinário",
    variavel: "E2E_SENHA_AUX_CARLA",
  },
  auxDiogo: {
    email: "aux.diogo@clinica-g08.test",
    nome: "Diogo Auxiliar (sem apoio)",
    perfil: "Auxiliar veterinário",
    variavel: "E2E_SENHA_AUX_DIOGO",
  },
  clienteJoao: {
    email: "cliente.joao@clinica-g08.test",
    nome: "João Costa (demonstração)",
    perfil: "Cliente",
    variavel: "E2E_SENHA_CLIENTE_JOAO",
  },
} as const satisfies Record<string, Conta>;

/** Variáveis de ambiente das contas usadas pelos testes da I129. */
export const VARIAVEIS_I129 = [
  CONTAS.admin,
  CONTAS.rececao,
  CONTAS.rececaoBloqueada,
  CONTAS.vetAna,
  CONTAS.auxCarla,
].map((c) => c.variavel);

/** Palavra-passe de uma conta de demonstração, lida da variável de ambiente. */
export function senhaDe(conta: Conta): string {
  const valor = process.env[conta.variavel];
  if (!valor) {
    throw new Error(
      `Falta a variável de ambiente ${conta.variavel} com a palavra-passe de ${conta.email}.`,
    );
  }
  return valor;
}

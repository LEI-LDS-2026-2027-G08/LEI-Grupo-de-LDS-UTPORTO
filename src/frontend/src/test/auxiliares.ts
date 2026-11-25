import { vi } from "vitest";
import type { Perfil, SessaoAutenticada } from "../models/auth";

/** Resposta JSON falsa, para simular o backend nos testes. */
export function resposta(status: number, corpo?: unknown): Response {
  return new Response(corpo === undefined ? null : JSON.stringify(corpo), {
    status,
    headers: { "Content-Type": "application/json" },
  });
}

/** Sessão válida (expira daqui a 1 hora) para o perfil indicado. */
export function sessaoDe(perfil: Perfil): SessaoAutenticada {
  return {
    token: "token-secreto-de-teste",
    expiraEm: new Date(Date.now() + 60 * 60 * 1000).toISOString(),
    utilizador: {
      id: "1",
      nome: "Maria Teste",
      email: "maria@clinica.pt",
      perfil,
    },
  };
}

/** Serviço de autenticação falso (substitui o AuthService real). */
export function servicoFalso() {
  return {
    login: vi.fn(),
    logout: vi.fn().mockResolvedValue(undefined),
    me: vi.fn(),
  };
}

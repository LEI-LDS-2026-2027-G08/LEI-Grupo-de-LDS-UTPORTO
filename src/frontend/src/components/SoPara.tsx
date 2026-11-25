import type { ReactNode } from "react";
import { useAuth } from "../core/auth/useAuth";
import type { Acao } from "../core/auth/permissoes";

interface SoParaProps {
  /** Ação (ver `core/auth/permissoes.ts`) que o utilizador tem de poder executar. */
  acao: Acao;
  children: ReactNode;
  /** O que mostrar a quem não pode. Por omissão, nada. */
  alternativa?: ReactNode;
}

/**
 * Mostra o conteúdo só a quem pode executar a ação (RF005).
 *
 * @example
 * <SoPara acao="gerirMarcacoes">
 *   <button onClick={novaMarcacao}>Nova marcação</button>
 * </SoPara>
 *
 * Esconder um botão não protege nada: a API rejeita sempre o que o perfil
 * não pode fazer. Isto serve para a interface não mostrar o que não funciona.
 */
export function SoPara({ acao, children, alternativa = null }: SoParaProps) {
  const { pode } = useAuth();
  return <>{pode(acao) ? children : alternativa}</>;
}

import { useContext, useEffect } from "react";
import { UNSAFE_DataRouterContext, useBlocker } from "react-router-dom";
import {
  MENSAGEM_ALTERACOES_POR_GUARDAR,
  useAvisoAoFecharSeparador,
} from "./useAvisoAlteracoes";

interface AvisoAlteracoesProps {
  /** Há alterações por guardar? */
  ativo: boolean;
  mensagem?: string;
}

/**
 * Evita perder alterações por guardar num formulário (I115, critério 6).
 *
 * Enquanto `ativo` for verdadeiro:
 * - qualquer navegação dentro da aplicação — ligações, menu e o botão
 *   «Retroceder» ou «Avançar» do navegador — pede confirmação; se o
 *   utilizador cancelar, fica no ecrã com os dados como estavam;
 * - fechar ou recarregar o separador mostra o aviso do navegador.
 *
 * O bloqueio usa o useBlocker do React Router, que só existe num data router
 * (createBrowserRouter na aplicação; createMemoryRouter nos testes). Fora de
 * um data router, fica só o aviso ao fechar o separador.
 */
export function AvisoAlteracoes({
  ativo,
  mensagem = MENSAGEM_ALTERACOES_POR_GUARDAR,
}: AvisoAlteracoesProps) {
  useAvisoAoFecharSeparador(ativo);
  const emDataRouter = useContext(UNSAFE_DataRouterContext) !== null;
  return emDataRouter ? (
    <BloqueioNavegacao ativo={ativo} mensagem={mensagem} />
  ) : null;
}

function BloqueioNavegacao({
  ativo,
  mensagem,
}: {
  ativo: boolean;
  mensagem: string;
}) {
  const bloqueio = useBlocker(
    ({ currentLocation, nextLocation }) =>
      ativo &&
      (currentLocation.pathname !== nextLocation.pathname ||
        currentLocation.search !== nextLocation.search),
  );

  useEffect(() => {
    if (bloqueio.state !== "blocked") return;
    if (window.confirm(mensagem)) bloqueio.proceed();
    else bloqueio.reset();
  }, [bloqueio, mensagem]);

  return null;
}

import { useEffect } from "react";

export const MENSAGEM_ALTERACOES_POR_GUARDAR =
  "Tem alterações por guardar. Se sair agora, perde-as. Quer mesmo sair?";

/**
 * Evita perder alterações por guardar num formulário (I115, critério 6).
 *
 * Enquanto `ativo` for verdadeiro:
 * - fechar ou recarregar o separador mostra o aviso do browser;
 * - clicar numa ligação da aplicação (menu, «Voltar», ...) pede confirmação;
 *   se o utilizador cancelar, a navegação não acontece.
 *
 * A aplicação usa BrowserRouter (sem data router), por isso o `useBlocker`
 * do React Router não está disponível; intercetamos o clique antes de chegar
 * ao `<Link>`. O botão «retroceder» do browser não é intercetado.
 */
export function useAvisoAlteracoes(
  ativo: boolean,
  mensagem: string = MENSAGEM_ALTERACOES_POR_GUARDAR,
): void {
  useEffect(() => {
    if (!ativo) return;

    function antesDeSair(evento: BeforeUnloadEvent) {
      evento.preventDefault();
      // Necessário em alguns browsers para mostrarem o aviso.
      evento.returnValue = "";
    }

    function aoClicar(evento: MouseEvent) {
      if (
        evento.defaultPrevented ||
        evento.button !== 0 ||
        evento.metaKey ||
        evento.ctrlKey ||
        evento.shiftKey ||
        evento.altKey
      ) {
        return;
      }
      const alvo = evento.target instanceof Element ? evento.target : null;
      const ligacao = alvo?.closest("a[href]");
      if (!(ligacao instanceof HTMLAnchorElement)) return;
      if (ligacao.target === "_blank") return;
      if (!window.confirm(mensagem)) {
        evento.preventDefault();
        evento.stopPropagation();
      }
    }

    window.addEventListener("beforeunload", antesDeSair);
    // Fase de captura: corre antes do onClick do <Link> do React Router.
    document.addEventListener("click", aoClicar, true);
    return () => {
      window.removeEventListener("beforeunload", antesDeSair);
      document.removeEventListener("click", aoClicar, true);
    };
  }, [ativo, mensagem]);
}

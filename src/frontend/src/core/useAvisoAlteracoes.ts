import { useEffect } from "react";

export const MENSAGEM_ALTERACOES_POR_GUARDAR =
  "Tem alterações por guardar. Se sair agora, perde-as. Quer mesmo sair?";

/**
 * Enquanto `ativo` for verdadeiro, fechar ou recarregar o separador mostra o
 * aviso do navegador (I115, critério 6). A navegação dentro da aplicação
 * (ligações e botão «Retroceder») é tratada pelo componente AvisoAlteracoes.
 */
export function useAvisoAoFecharSeparador(ativo: boolean): void {
  useEffect(() => {
    if (!ativo) return;

    function antesDeSair(evento: BeforeUnloadEvent) {
      evento.preventDefault();
      // Necessário em alguns navegadores para mostrarem o aviso.
      evento.returnValue = "";
    }

    window.addEventListener("beforeunload", antesDeSair);
    return () => window.removeEventListener("beforeunload", antesDeSair);
  }, [ativo]);
}

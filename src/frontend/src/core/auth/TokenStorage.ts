import { ePerfilWeb, type SessaoAutenticada } from "../../models/auth";

/**
 * Onde a sessão fica guardada no browser.
 *
 * Decisão: sessionStorage (e não localStorage). A sessão morre quando o
 * separador/browser fecha, não é partilhada entre separadores e não há
 * refresh token. Tudo está em try/catch porque o storage pode estar
 * bloqueado (modo privado, políticas do browser).
 */
const CHAVE = "clinica.sessao";

function eSessaoValida(valor: unknown): valor is SessaoAutenticada {
  if (typeof valor !== "object" || valor === null) return false;
  const s = valor as Partial<SessaoAutenticada>;
  return (
    typeof s.token === "string" &&
    s.token !== "" &&
    typeof s.expiraEm === "string" &&
    typeof s.utilizador === "object" &&
    s.utilizador !== null &&
    typeof s.utilizador.perfil === "string" &&
    ePerfilWeb(s.utilizador.perfil)
  );
}

export const TokenStorage = {
  guardar(sessao: SessaoAutenticada): void {
    try {
      sessionStorage.setItem(CHAVE, JSON.stringify(sessao));
    } catch {
      // Sem storage a sessão só dura enquanto a página não for recarregada.
    }
  },

  /** Devolve a sessão guardada, ou null se não existir, estiver corrompida ou expirada. */
  obter(): SessaoAutenticada | null {
    try {
      const texto = sessionStorage.getItem(CHAVE);
      if (texto === null) return null;
      const valor: unknown = JSON.parse(texto);
      if (!eSessaoValida(valor)) {
        TokenStorage.limpar();
        return null;
      }
      const expiraEm = Date.parse(valor.expiraEm);
      if (Number.isNaN(expiraEm) || expiraEm <= Date.now()) {
        TokenStorage.limpar();
        return null;
      }
      return valor;
    } catch {
      TokenStorage.limpar();
      return null;
    }
  },

  limpar(): void {
    try {
      sessionStorage.removeItem(CHAVE);
    } catch {
      // Nada a fazer.
    }
  },
};

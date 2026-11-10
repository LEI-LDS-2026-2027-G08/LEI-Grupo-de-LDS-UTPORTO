import {
  useCallback,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";
import { apiClient, ApiRequestError, type ApiClient } from "../api";
import {
  ePerfilWeb,
  type CredenciaisLogin,
  type PerfilWeb,
  type SessaoAutenticada,
  type UtilizadorAutenticado,
} from "../../models/auth";
import { authService, type AuthService } from "../../services/AuthService";
import { AuthContext, type AuthContextValue } from "./contextoAuth";
import { perfilPodeExecutar, type Acao } from "./permissoes";
import { TokenStorage } from "./TokenStorage";

interface AuthProviderProps {
  children: ReactNode;
  /** Só para testes: permite trocar o serviço e o cliente HTTP. */
  servico?: Pick<AuthService, "login" | "logout" | "me">;
  cliente?: ApiClient;
}

/**
 * Guarda a sessão e liga-a ao ApiClient.
 *
 * - Ao arrancar, lê a sessão guardada e confirma-a com `GET /auth/me`.
 * - Só aceita perfis web: um Cliente que faça login válido é terminado
 *   logo (EP-002) e recebe um erro (RNF003).
 * - Se o backend responder 401 a qualquer pedido com token, a sessão é limpa,
 *   `sessaoTerminada` fica true e o ProtectedRoute leva o utilizador ao login.
 */
export function AuthProvider({
  children,
  servico = authService,
  cliente = apiClient,
}: AuthProviderProps) {
  const [sessaoInicial] = useState(() => TokenStorage.obter());
  const [sessao, setSessao] = useState<SessaoAutenticada | null>(sessaoInicial);
  const [aRestaurar, setARestaurar] = useState(sessaoInicial !== null);
  const [sessaoTerminada, setSessaoTerminada] = useState(false);

  const terminarSessaoLocal = useCallback(() => {
    cliente.setToken(null);
    TokenStorage.limpar();
    setSessao(null);
  }, [cliente]);

  // 401 vindo de qualquer pedido com token -> a sessão expirou ou foi revogada.
  // Limpamos tudo e deixamos um aviso para o ecrã de login mostrar.
  useEffect(() => {
    cliente.definirAoNaoAutorizado(() => {
      terminarSessaoLocal();
      setSessaoTerminada(true);
    });
    return () => cliente.definirAoNaoAutorizado(null);
  }, [cliente, terminarSessaoLocal]);

  // Restaurar sessão guardada: o token só vale depois de o backend confirmar.
  useEffect(() => {
    if (sessaoInicial === null) return;
    let cancelado = false;
    cliente.setToken(sessaoInicial.token);

    servico
      .me()
      .then((utilizador) => {
        if (cancelado) return;
        if (!ePerfilWeb(utilizador.perfil)) {
          terminarSessaoLocal();
          return;
        }
        const atualizada = { ...sessaoInicial, utilizador };
        TokenStorage.guardar(atualizada);
        setSessao(atualizada);
      })
      .catch(() => {
        if (!cancelado) terminarSessaoLocal();
      })
      .finally(() => {
        if (!cancelado) setARestaurar(false);
      });

    return () => {
      cancelado = true;
    };
  }, [sessaoInicial, cliente, servico, terminarSessaoLocal]);

  const login = useCallback(
    async (credenciais: CredenciaisLogin): Promise<UtilizadorAutenticado> => {
      const nova = await servico.login(credenciais);

      if (!ePerfilWeb(nova.utilizador.perfil)) {
        // O backend aceitou o login, mas esta app não é para Clientes.
        // Usamos o token só para o invalidar logo a seguir.
        cliente.setToken(nova.token);
        try {
          await servico.logout();
        } catch {
          // Se falhar, o token expira sozinho; o utilizador é recusado na mesma.
        } finally {
          cliente.setToken(null);
        }
        throw new ApiRequestError(
          403,
          "semPermissao",
          "Os Clientes não têm acesso à aplicação web. Utilize a aplicação móvel.",
        );
      }

      cliente.setToken(nova.token);
      TokenStorage.guardar(nova);
      setSessaoTerminada(false);
      setSessao(nova);
      return nova.utilizador;
    },
    [servico, cliente],
  );

  const logout = useCallback(async () => {
    try {
      await servico.logout();
    } catch {
      // Mesmo que o servidor falhe, a sessão local termina.
    } finally {
      terminarSessaoLocal();
    }
  }, [servico, terminarSessaoLocal]);

  const valor = useMemo<AuthContextValue>(() => {
    const utilizador = sessao?.utilizador ?? null;
    return {
      sessao,
      utilizador,
      aRestaurar,
      login,
      logout,
      temPerfil: (perfis: readonly PerfilWeb[]) =>
        utilizador !== null &&
        (perfis as readonly string[]).includes(utilizador.perfil),
      pode: (acao: Acao) =>
        utilizador !== null && perfilPodeExecutar(utilizador.perfil, acao),
      sessaoTerminada,
    };
  }, [sessao, aRestaurar, sessaoTerminada, login, logout]);

  return <AuthContext.Provider value={valor}>{children}</AuthContext.Provider>;
}

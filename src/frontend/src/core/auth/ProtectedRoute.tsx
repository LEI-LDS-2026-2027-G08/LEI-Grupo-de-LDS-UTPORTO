import type { ReactNode } from "react";
import { Navigate, Outlet, useLocation } from "react-router-dom";
import type { PerfilWeb } from "../../models/auth";
import { useAuth } from "./useAuth";

interface ProtectedRouteProps {
  /** Perfis autorizados. Sem esta prop basta ter sessão iniciada. */
  perfis?: readonly PerfilWeb[];
  children?: ReactNode;
}

/**
 * Guarda de rotas.
 *  - A confirmar a sessão guardada -> mensagem de espera.
 *  - Sem sessão -> /login (guarda a página pedida para voltar depois).
 *  - Com sessão mas perfil não autorizado -> /acesso-negado.
 *  - Caso contrário mostra `children` (ou as rotas filhas, via <Outlet />).
 */
export function ProtectedRoute({ perfis, children }: ProtectedRouteProps) {
  const { utilizador, aRestaurar, temPerfil } = useAuth();
  const localizacao = useLocation();

  if (aRestaurar) {
    return <p role="status">A verificar a sessão…</p>;
  }
  if (utilizador === null) {
    return (
      <Navigate
        to="/login"
        replace
        state={{ de: localizacao.pathname + localizacao.search }}
      />
    );
  }
  if (perfis && !temPerfil(perfis)) {
    return <Navigate to="/acesso-negado" replace />;
  }
  return children ?? <Outlet />;
}

import { Navigate, Route, Routes } from "react-router-dom";
import { ProtectedRoute } from "../core/auth/ProtectedRoute";
import { useAuth } from "../core/auth/useAuth";
import { AppLayout } from "../layout/AppLayout";
import { rotaInicial } from "../layout/navegacao";
import { AcessoNegadoPage } from "../pages/AcessoNegadoPage";
import { EmConstrucaoPage } from "../pages/EmConstrucaoPage";
import { NaoEncontradaPage } from "../pages/NaoEncontradaPage";
import { LoginPage } from "../pages/auth/LoginPage";
import { ROTAS } from "./rotas";

function PaginaInicial() {
  const { utilizador } = useAuth();
  return <Navigate to={rotaInicial(utilizador!.perfil)} replace />;
}

/**
 * Tabela de rotas da aplicação.
 *  /login           -> pública
 *  tudo o resto     -> exige sessão, dentro do AppLayout (menu + cabeçalho)
 *  cada ROTA        -> exige também um dos perfis indicados em `rotas.ts`
 */
export function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route
        element={
          <ProtectedRoute>
            <AppLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<PaginaInicial />} />

        {ROTAS.map((rota) => {
          const Pagina = rota.pagina;
          return (
            <Route
              key={rota.caminho}
              path={rota.caminho}
              element={
                <ProtectedRoute perfis={rota.perfis}>
                  {Pagina ? (
                    <Pagina />
                  ) : (
                    <EmConstrucaoPage ecra={rota.ecra} titulo={rota.titulo} />
                  )}
                </ProtectedRoute>
              }
            />
          );
        })}

        <Route path="/acesso-negado" element={<AcessoNegadoPage />} />
        <Route path="*" element={<NaoEncontradaPage />} />
      </Route>
    </Routes>
  );
}

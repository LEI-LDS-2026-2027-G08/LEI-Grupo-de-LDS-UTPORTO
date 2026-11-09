import { useState } from "react";
import { NavLink, Outlet } from "react-router-dom";
import { useAuth } from "../core/auth/useAuth";
import { itensMenuPara } from "./navegacao";

const ROTULOS_PERFIL = {
  Administrador: "Administrador",
  Rececionista: "Rececionista",
  Veterinario: "Veterinário",
  AuxiliarVeterinario: "Auxiliar veterinário",
  Cliente: "Cliente",
} as const;

/** Estrutura comum às páginas autenticadas: cabeçalho, menu por perfil e conteúdo. */
export function AppLayout() {
  const { utilizador, logout } = useAuth();
  const [aTerminar, setATerminar] = useState(false);

  if (utilizador === null) return null; // o ProtectedRoute já tratou deste caso

  async function terminarSessao() {
    setATerminar(true);
    await logout();
  }

  return (
    <div className="app-layout">
      <header className="app-cabecalho">
        <strong>Clínica Veterinária</strong>
        <div className="app-utilizador">
          <span>
            {utilizador.nome} · {ROTULOS_PERFIL[utilizador.perfil]}
          </span>
          <button type="button" onClick={terminarSessao} disabled={aTerminar}>
            Terminar sessão
          </button>
        </div>
      </header>

      <nav className="app-menu" aria-label="Menu principal">
        <ul>
          {itensMenuPara(utilizador.perfil).map((item) => (
            <li key={item.caminho}>
              <NavLink to={item.caminho}>{item.rotulo}</NavLink>
            </li>
          ))}
        </ul>
      </nav>

      <main className="app-conteudo">
        <Outlet />
      </main>
    </div>
  );
}

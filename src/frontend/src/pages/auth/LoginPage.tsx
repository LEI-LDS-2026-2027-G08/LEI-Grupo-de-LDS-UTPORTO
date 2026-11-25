import { useState, type FormEvent } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import { useAuth } from "../../core/auth/useAuth";
import { rotaInicial } from "../../layout/navegacao";

/** W01 — Início de sessão. */
export function LoginPage() {
  const { utilizador, aRestaurar, sessaoTerminada, login } = useAuth();
  const localizacao = useLocation();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [erro, setErro] = useState<unknown>(null);
  const [aSubmeter, setASubmeter] = useState(false);

  if (aRestaurar) {
    return <p role="status">A verificar a sessão…</p>;
  }

  // Já há sessão (acabou de entrar, ou abriu /login com sessão ativa):
  // vai para a página que queria abrir, ou para a inicial do seu perfil.
  if (utilizador !== null) {
    const de = (localizacao.state as { de?: string } | null)?.de;
    return <Navigate to={de ?? rotaInicial(utilizador.perfil)} replace />;
  }

  async function submeter(evento: FormEvent) {
    evento.preventDefault();
    setErro(null);
    setASubmeter(true);
    try {
      await login({ email: email.trim(), password });
    } catch (e) {
      setErro(e);
      setPassword("");
    } finally {
      setASubmeter(false);
    }
  }

  return (
    <main className="login">
      <form onSubmit={submeter} className="login-cartao">
        <h1>Iniciar sessão</h1>
        {sessaoTerminada && erro === null ? (
          <p role="status" className="aviso">
            A sua sessão terminou. Inicie sessão novamente.
          </p>
        ) : null}
        <FormErro erro={erro} />
        <FormField
          rotulo="Email"
          type="email"
          autoComplete="username"
          required
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <FormField
          rotulo="Palavra-passe"
          type="password"
          autoComplete="current-password"
          required
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button type="submit" disabled={aSubmeter}>
          {aSubmeter ? "A entrar…" : "Entrar"}
        </button>
      </form>
    </main>
  );
}

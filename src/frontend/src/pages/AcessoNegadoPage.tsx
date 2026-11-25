import { Link } from "react-router-dom";

export function AcessoNegadoPage() {
  return (
    <section>
      <h1>Acesso negado</h1>
      <p>O seu perfil não tem permissão para ver esta página.</p>
      <Link to="/">Voltar ao início</Link>
    </section>
  );
}

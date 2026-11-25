import { Link } from "react-router-dom";

export function NaoEncontradaPage() {
  return (
    <section>
      <h1>Página não encontrada</h1>
      <p>O endereço que abriu não existe.</p>
      <Link to="/">Voltar ao início</Link>
    </section>
  );
}

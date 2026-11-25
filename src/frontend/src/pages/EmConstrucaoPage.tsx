interface EmConstrucaoPageProps {
  ecra: string;
  titulo: string;
}

/** Marcador de um ecrã ainda por implementar. Ver `routes/rotas.ts`. */
export function EmConstrucaoPage({ ecra, titulo }: EmConstrucaoPageProps) {
  return (
    <section>
      <h1>{titulo}</h1>
      <p>Este ecrã ({ecra}) ainda não foi implementado.</p>
    </section>
  );
}

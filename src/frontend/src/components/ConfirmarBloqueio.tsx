import { useState, type FormEvent } from "react";
import { FormErro } from "./FormErro";
import { FormField } from "./FormField";

interface ConfirmarBloqueioProps {
  /** Nome da conta a bloquear, para a pergunta de confirmação. */
  nome: string;
  /** Faz o pedido (EP-008). Se falhar, o erro é mostrado aqui. */
  aoConfirmar: (motivo: string) => Promise<void>;
  aoCancelar: () => void;
}

/**
 * Confirmação do bloqueio de uma conta (EP-008), com motivo opcional.
 * Não há desbloqueio: a ação é definitiva na interface.
 */
export function ConfirmarBloqueio({
  nome,
  aoConfirmar,
  aoCancelar,
}: ConfirmarBloqueioProps) {
  const [motivo, setMotivo] = useState("");
  const [erro, setErro] = useState<unknown>(null);
  const [aEnviar, setAEnviar] = useState(false);

  async function confirmar(evento: FormEvent) {
    evento.preventDefault();
    setErro(null);
    setAEnviar(true);
    try {
      await aoConfirmar(motivo);
    } catch (e) {
      setErro(e);
      setAEnviar(false);
    }
  }

  return (
    <form
      className="confirmacao"
      onSubmit={confirmar}
      aria-label="Confirmar bloqueio"
    >
      <p>
        Bloquear a conta de <strong>{nome}</strong>? A conta deixa de poder
        iniciar sessão e não há opção para a desbloquear.
      </p>
      <FormErro erro={erro} />
      <FormField
        rotulo="Motivo (opcional)"
        value={motivo}
        maxLength={500}
        onChange={(e) => setMotivo(e.target.value)}
      />
      <div className="acoes">
        <button type="submit" className="perigo" disabled={aEnviar}>
          {aEnviar ? "A bloquear…" : "Confirmar bloqueio"}
        </button>
        <button
          type="button"
          className="secundario"
          onClick={aoCancelar}
          disabled={aEnviar}
        >
          Cancelar
        </button>
      </div>
    </form>
  );
}

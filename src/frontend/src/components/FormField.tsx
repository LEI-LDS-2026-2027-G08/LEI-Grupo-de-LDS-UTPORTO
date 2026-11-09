import { useId, type InputHTMLAttributes } from "react";

interface FormFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  rotulo: string;
  /** Mensagem de erro deste campo (ex.: vinda de um ApiRequestError). */
  erro?: string | null;
}

/**
 * Campo de formulário: etiqueta + input + mensagem de erro, já ligados
 * entre si para leitores de ecrã. Aceita todas as props de um <input>.
 *
 * @example
 * <FormField
 *   rotulo="Email"
 *   type="email"
 *   value={email}
 *   onChange={(e) => setEmail(e.target.value)}
 *   erro={erroDoEmail}
 * />
 */
export function FormField({ rotulo, erro, id, ...resto }: FormFieldProps) {
  const idGerado = useId();
  const idCampo = id ?? idGerado;
  const idErro = `${idCampo}-erro`;

  return (
    <div className="campo">
      <label htmlFor={idCampo}>{rotulo}</label>
      <input
        id={idCampo}
        aria-invalid={erro ? true : undefined}
        aria-describedby={erro ? idErro : undefined}
        {...resto}
      />
      {erro ? (
        <p id={idErro} className="campo-erro">
          {erro}
        </p>
      ) : null}
    </div>
  );
}

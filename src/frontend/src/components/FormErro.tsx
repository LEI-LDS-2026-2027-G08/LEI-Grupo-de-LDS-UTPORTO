import { mensagemDeErro } from "../core/api";

interface FormErroProps {
  /** O erro apanhado no `catch` (normalmente um ApiRequestError) ou null. */
  erro: unknown;
}

/**
 * Faixa de erro de um formulário. Não mostra nada se não houver erro.
 * Funciona com qualquer erro: se não for um ApiRequestError mostra uma
 * mensagem genérica, sem detalhes técnicos.
 *
 * @example
 * try {
 *   await clienteService.criar(dados);
 * } catch (e) {
 *   setErro(e);
 * }
 * ...
 * <FormErro erro={erro} />
 */
export function FormErro({ erro }: FormErroProps) {
  if (erro === null || erro === undefined) return null;
  return (
    <div role="alert" className="form-erro">
      {mensagemDeErro(erro)}
    </div>
  );
}

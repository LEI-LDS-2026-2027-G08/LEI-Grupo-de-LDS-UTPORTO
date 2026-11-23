import { URL_API } from "./ambiente";
import { VARIAVEIS_I129 } from "./contas";

/**
 * Antes de qualquer teste: confirma que as palavras-passe das contas de
 * demonstração estão definidas e que o backend responde. Falha logo, com
 * uma mensagem clara, em vez de falhar 30 testes pelo mesmo motivo.
 */
export default async function verificarAmbiente(): Promise<void> {
  const emFalta = VARIAVEIS_I129.filter((nome) => !process.env[nome]);
  if (emFalta.length > 0) {
    throw new Error(
      `Faltam as variáveis de ambiente com as palavras-passe das contas de demonstração: ${emFalta.join(", ")}.`,
    );
  }
  try {
    const resposta = await fetch(`${URL_API}/auth/me`);
    if (resposta.status !== 401) {
      throw new Error(`resposta inesperada (${resposta.status})`);
    }
  } catch (erro) {
    throw new Error(
      `O backend não responde em ${URL_API} (${String(erro)}). Arranque a API com a base de dados de demonstração antes dos testes.`,
      { cause: erro },
    );
  }
}

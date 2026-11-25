import type { Page } from "@playwright/test";

/**
 * Acesso ao armazenamento da sessão do navegador (TokenStorage.ts), usado
 * pelos cenários de sessão da I132 (ST-WEB-TRV-04, -05, -06 e -11).
 */
export const CHAVE_SESSAO = "clinica.sessao";

/** Texto guardado na chave da sessão, ou null se não houver sessão. */
export function sessaoGuardada(page: Page): Promise<string | null> {
  return page.evaluate((chave) => sessionStorage.getItem(chave), CHAVE_SESSAO);
}

/** Token da sessão guardada (falha se não houver sessão). */
export async function tokenDaSessao(page: Page): Promise<string> {
  const texto = await sessaoGuardada(page);
  if (texto === null) {
    throw new Error("Não há sessão guardada no navegador.");
  }
  return (JSON.parse(texto) as { token: string }).token;
}

/**
 * Troca o token guardado por outro valor, mantendo o resto da sessão
 * (incluindo a data de expiração), como acontece com um token expirado ou
 * revogado que ainda está no navegador.
 */
export async function substituirToken(
  page: Page,
  token: string,
): Promise<void> {
  await page.evaluate(
    ([chave, novo]) => {
      const texto = sessionStorage.getItem(chave);
      if (texto === null) throw new Error("Não há sessão guardada.");
      const sessao = JSON.parse(texto) as { token: string };
      sessao.token = novo;
      sessionStorage.setItem(chave, JSON.stringify(sessao));
    },
    [CHAVE_SESSAO, token] as const,
  );
}

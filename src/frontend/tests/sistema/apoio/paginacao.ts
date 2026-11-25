import { expect, type Locator, type Page } from "@playwright/test";

/**
 * Procura uma linha com este texto na lista, a partir da página atual e
 * avançando com «Seguinte». As listas têm 20 linhas por página e estão
 * ordenadas por nome, por isso os dados criados por execuções anteriores
 * (até à reposição da base) podem empurrar um registo para outra página.
 */
export async function linhaEmAlgumaPagina(
  page: Page,
  linhas: Locator,
  texto: string,
): Promise<Locator> {
  const paginacao = page.getByRole("navigation", { name: "Páginas" });
  const seguinte = paginacao.getByRole("button", { name: "Seguinte" });
  for (let i = 0; i < 100; i++) {
    const linha = linhas.filter({ hasText: texto });
    if ((await linha.count()) > 0) return linha;
    if ((await seguinte.count()) === 0 || (await seguinte.isDisabled())) break;
    const atual = Number(/Página (\d+)/.exec(await paginacao.innerText())?.[1]);
    await seguinte.click();
    await expect(paginacao).toContainText(`Página ${atual + 1} de`);
  }
  return linhas.filter({ hasText: texto });
}

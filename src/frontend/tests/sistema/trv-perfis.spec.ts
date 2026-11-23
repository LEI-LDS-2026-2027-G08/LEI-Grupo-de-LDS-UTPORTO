import { expect, test, type Page } from "@playwright/test";
import { CONTAS, senhaDe, type Conta } from "./apoio/contas";
import { sufixo } from "./apoio/dados";
import { sessaoGuardada } from "./apoio/navegador";
import { idDoBobi, registarRespostas } from "./apoio/respostas";
import { iniciarSessao, terminarSessao } from "./apoio/sessao";
import { LayoutApp } from "./paginas/LayoutApp";
import {
  PaginaAcessoNegado,
  PaginaNaoEncontrada,
} from "./paginas/PaginaAvisos";
import { PaginaLogin } from "./paginas/PaginaLogin";
import { PaginaPaciente } from "./paginas/PaginaPaciente";

// I128, secções 8, 10, 11 e 12 — permissões por perfil: menu, rotas por
// endereço direto e acesso sem sessão. Automação: I132.

/** Menu e página inicial de cada perfil (tabela de ST-WEB-F001-01). */
const PERFIS: readonly { conta: Conta; inicial: string; menu: string[] }[] = [
  {
    conta: CONTAS.admin,
    inicial: "/clientes",
    menu: ["Clientes", "Funcionários", "Clínica e horários"],
  },
  {
    conta: CONTAS.rececao,
    inicial: "/agenda",
    menu: ["Agenda", "Pacientes", "Clientes"],
  },
  { conta: CONTAS.vetAna, inicial: "/agenda", menu: ["Agenda", "Pacientes"] },
  {
    conta: CONTAS.auxCarla,
    inicial: "/preparacao",
    menu: ["Preparação de consultas"],
  },
];

/** Abre a rota pelo endereço e confirma o «Acesso negado» sem perder a sessão. */
async function esperarAcessoNegado(
  page: Page,
  conta: Conta,
  rota: string,
): Promise<void> {
  const negado = new PaginaAcessoNegado(page);
  await page.goto(rota);
  await expect(page).toHaveURL(/\/acesso-negado$/);
  await expect(negado.titulo).toBeVisible();
  await expect(negado.explicacao).toBeVisible();
  await expect(negado.voltarInicio).toBeVisible();
  await expect(new LayoutApp(page).cabecalho).toContainText(
    `${conta.nome} · ${conta.perfil}`,
  );
}

test("ST-WEB-TRV-01 — Menu de cada perfil sem entradas de outros perfis", async ({
  page,
}) => {
  const layout = new LayoutApp(page);
  for (const { conta, menu } of PERFIS) {
    await test.step(conta.email, async () => {
      await iniciarSessao(page, conta);
      // Exatamente estas entradas, por esta ordem, e nenhuma outra.
      await expect(layout.entradasMenu).toHaveText(menu);
      await terminarSessao(page);
    });
  }
});

test("ST-WEB-TRV-02 — Recusar o acesso por endereço direto a rotas de outro perfil", async ({
  page,
}) => {
  // 4 perfis e 22 endereços abertos um a um: mais tempo do que o habitual.
  test.slow();
  const rotasRecusadas: Record<string, string[]> = {
    [CONTAS.admin.email]: [
      "/agenda",
      "/agenda/nova",
      "/pacientes",
      "/pacientes/1",
      "/preparacao",
    ],
    [CONTAS.rececao.email]: [
      "/funcionarios",
      "/funcionarios/novo",
      "/clinica",
      "/preparacao",
    ],
    [CONTAS.vetAna.email]: [
      "/clientes",
      "/clientes/novo",
      "/funcionarios",
      "/clinica",
      "/agenda/nova",
      "/preparacao",
    ],
    [CONTAS.auxCarla.email]: [
      "/agenda",
      "/agenda/1",
      "/pacientes",
      "/pacientes/novo",
      "/clientes",
      "/funcionarios",
      "/clinica",
    ],
  };
  const respostas = registarRespostas(page);
  const negado = new PaginaAcessoNegado(page);

  for (const { conta, inicial } of PERFIS) {
    await test.step(conta.email, async () => {
      await iniciarSessao(page, conta);
      for (const rota of rotasRecusadas[conta.email]) {
        await test.step(rota, async () => {
          // Espera que terminem os pedidos da página anterior (por exemplo,
          // a lista que abre depois do login), para só contar os desta rota.
          await page.waitForLoadState("networkidle");
          respostas.length = 0;
          await esperarAcessoNegado(page, conta, rota);
          // Ao abrir o endereço só se confirma a sessão (EP-003): a página
          // recusada não chegou a pedir os seus dados ao backend.
          expect(respostas.filter((r) => r.caminho !== "/auth/me")).toEqual([]);
        });
      }
      await negado.voltarInicio.click();
      await expect(page).toHaveURL(new RegExp(`${inicial}$`));
      await terminarSessao(page);
    });
  }
});

test("ST-WEB-TRV-03 — Rotas protegidas sem sessão levam ao início de sessão", async ({
  page,
}) => {
  const login = new PaginaLogin(page);
  const layout = new LayoutApp(page);
  for (const rota of [
    "/funcionarios",
    "/clientes/1",
    "/agenda",
    "/preparacao",
  ]) {
    await test.step(rota, async () => {
      await page.goto(rota);
      await expect(page).toHaveURL(/\/login$/);
      await expect(login.titulo).toBeVisible();
      await expect(layout.menu).toHaveCount(0);
      await expect(page.locator("table")).toHaveCount(0);
    });
  }
});

test("ST-WEB-TRV-04 — O Cliente não usa a aplicação web", async ({ page }) => {
  const login = new PaginaLogin(page);
  await login.abrir();
  await login.preencherEEntrar(
    CONTAS.clienteJoao.email,
    senhaDe(CONTAS.clienteJoao),
  );
  await expect(login.erro).toHaveText(
    "Os Clientes não têm acesso à aplicação web. Utilize a aplicação móvel.",
  );
  await expect(page).toHaveURL(/\/login$/);
  expect(await sessaoGuardada(page)).toBeNull();

  await page.goto("/clientes");
  await expect(page).toHaveURL(/\/login$/);
  await expect(login.titulo).toBeVisible();
});

test("ST-WEB-TRV-12 — Endereço inexistente", async ({ page }) => {
  const suf = sufixo("TRV-12");
  const naoEncontrada = new PaginaNaoEncontrada(page);
  await iniciarSessao(page, CONTAS.vetAna);
  await page.goto(`/nao-existe-${suf}`);
  await expect(naoEncontrada.titulo).toBeVisible();
  await expect(naoEncontrada.explicacao).toBeVisible();
  await naoEncontrada.voltarInicio.click();
  await expect(page).toHaveURL(/\/agenda$/);
});

test("ST-WEB-F002-10 — Ficha do paciente só em leitura para o Auxiliar Veterinário", async ({
  page,
  request,
}) => {
  const bobi = await idDoBobi(request);
  await iniciarSessao(page, CONTAS.auxCarla);

  await test.step("ficha do Bobi só de leitura", async () => {
    await page.goto(`/pacientes/${bobi}`);
    const dados = page.locator('dl[aria-label="Dados administrativos"]');
    await expect(dados).toContainText("Bobi");
    await expect(dados).toContainText(CONTAS.clienteJoao.nome);
    const conteudo = page.locator("main");
    await expect(conteudo.locator("input, select, textarea")).toHaveCount(0);
    await expect(
      page.getByRole("button", { name: "Guardar dados administrativos" }),
    ).toHaveCount(0);
    await expect(
      page.getByRole("link", { name: "Registo clínico", exact: true }),
    ).toBeVisible();
    await expect(
      page.getByRole("link", { name: "Vacinação", exact: true }),
    ).toHaveCount(0);
    await expect(
      page.getByRole("link", { name: "Nova marcação", exact: true }),
    ).toHaveCount(0);
  });

  for (const rota of ["/pacientes", "/pacientes/novo"]) {
    await test.step(rota, async () => {
      await esperarAcessoNegado(page, CONTAS.auxCarla, rota);
    });
  }
});

test("ST-WEB-F004-08 — Recusar o acesso ao registo clínico a quem não o pode ver", async ({
  page,
  request,
}) => {
  const bobi = await idDoBobi(request);
  for (const conta of [CONTAS.rececao, CONTAS.admin]) {
    await test.step(conta.email, async () => {
      await iniciarSessao(page, conta);
      await esperarAcessoNegado(page, conta, `/pacientes/${bobi}/registo`);
      // Nenhuma informação clínica (o Bobi tem «Otite externa», secção 5.3).
      await expect(page.locator("main")).not.toContainText("Otite externa");
      await expect(page.locator("main")).not.toContainText("Histórico clínico");
      await terminarSessao(page);
    });
  }
});

test("ST-WEB-F006-05 — Recusar o boletim web a quem não é Veterinário", async ({
  page,
  request,
}) => {
  const bobi = await idDoBobi(request);
  for (const conta of [CONTAS.rececao, CONTAS.auxCarla]) {
    await test.step(conta.email, async () => {
      await iniciarSessao(page, conta);
      await page.goto(`/pacientes/${bobi}`);
      await expect(
        page.getByRole("heading", { name: "Ficha do paciente", level: 1 }),
      ).toBeVisible();
      // A Rececionista vê a ficha editável; o Auxiliar, só de leitura.
      if (conta === CONTAS.rececao) {
        await expect(new PaginaPaciente(page).nome).toHaveValue("Bobi");
      } else {
        await expect(
          page.locator('dl[aria-label="Dados administrativos"]'),
        ).toContainText("Bobi");
      }
      await expect(
        page.getByRole("link", { name: "Vacinação", exact: true }),
      ).toHaveCount(0);

      await esperarAcessoNegado(page, conta, `/pacientes/${bobi}/vacinacao`);
      await terminarSessao(page);
    });
  }
});

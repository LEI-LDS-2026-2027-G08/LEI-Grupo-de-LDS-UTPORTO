import type { APIRequestContext, Page, Response } from "@playwright/test";
import { URL_API } from "./ambiente";
import { CONTAS, senhaDe } from "./contas";

/** Resposta do backend recebida pela página. */
export interface RespostaApi {
  metodo: string;
  /** Caminho sem o endereço do backend nem a query (ex.: «/clientes»). */
  caminho: string;
  status: number;
}

/**
 * Regista as respostas do backend a partir deste momento. Serve para
 * confirmar que uma página recusada não chegou a pedir os seus dados.
 */
export function registarRespostas(page: Page): RespostaApi[] {
  const respostas: RespostaApi[] = [];
  page.on("response", (resposta: Response) => {
    const url = resposta.url();
    if (!url.startsWith(URL_API)) return;
    respostas.push({
      metodo: resposta.request().method(),
      caminho: new URL(url).pathname,
      status: resposta.status(),
    });
  });
  return respostas;
}

interface PacienteDaLista {
  id: number;
  nome: string;
  cliente: { nome: string };
}

/**
 * Id do Bobi do João Costa nos dados de demonstração (I128, secção 5.3).
 * Lido da API com a conta da Rececionista (EP-001, EP-015), porque depende
 * da base; a sessão usada é terminada no fim (EP-002).
 */
export async function idDoBobi(request: APIRequestContext): Promise<number> {
  const login = await request.post(`${URL_API}/auth/login`, {
    data: {
      email: CONTAS.rececao.email,
      password: senhaDe(CONTAS.rececao),
    },
  });
  if (!login.ok()) {
    throw new Error(`Início de sessão na API falhou (${login.status()}).`);
  }
  const { token } = (await login.json()) as { token: string };
  const headers = { Authorization: `Bearer ${token}` };
  try {
    const resposta = await request.get(`${URL_API}/animais`, {
      params: { pesquisa: "Bobi" },
      headers,
    });
    if (!resposta.ok()) {
      throw new Error(`Pesquisa de pacientes falhou (${resposta.status()}).`);
    }
    const pagina = (await resposta.json()) as { itens: PacienteDaLista[] };
    const bobi = pagina.itens.find(
      (a) => a.nome === "Bobi" && a.cliente.nome === CONTAS.clienteJoao.nome,
    );
    if (!bobi) {
      throw new Error(
        "O Bobi do João Costa não existe na base: carregue os dados de demonstração (I061).",
      );
    }
    return bobi.id;
  } finally {
    await request.post(`${URL_API}/auth/logout`, { headers });
  }
}

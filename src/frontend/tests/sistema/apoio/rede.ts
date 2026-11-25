import type { Page, Route } from "@playwright/test";
import { URL_API } from "./ambiente";

/**
 * Controlo da ligação ao backend nos cenários de rede da I132
 * (ST-WEB-TRV-07, -08 e -10). A API nunca é simulada: os pedidos ou falham
 * como se o backend estivesse desligado, ou seguem para o backend real,
 * só mais tarde.
 */
const TODOS_OS_PEDIDOS = `${URL_API}/**`;

/** Mensagem do frontend quando o backend não responde (ApiRequestError.ts). */
export const MENSAGEM_SEM_LIGACAO =
  "Não foi possível contactar o servidor. Verifique a ligação.";

async function recusar(route: Route): Promise<void> {
  await route.abort("connectionrefused");
}

/** Corta a ligação: todos os pedidos ao backend falham sem resposta. */
export async function cortarLigacao(page: Page): Promise<void> {
  await page.route(TODOS_OS_PEDIDOS, recusar);
}

/** Repõe a ligação ao backend. */
export async function reporLigacao(page: Page): Promise<void> {
  await page.unroute(TODOS_OS_PEDIDOS, recusar);
}

/** Pedidos retidos até `libertar()` (ver `reterPedidos`). */
export interface PedidosRetidos {
  /** Resolve quando chegar o primeiro pedido retido. */
  readonly primeiro: Promise<void>;
  /** Deixa seguir para o backend os pedidos retidos e os seguintes. */
  libertar(): Promise<void>;
}

/**
 * Atrasa os pedidos com este método e caminho até `libertar()`, para se
 * poder ver o estado da página enquanto espera pela resposta (sem pausas
 * fixas: o teste decide quando a resposta pode chegar).
 */
export async function reterPedidos(
  page: Page,
  metodo: string,
  caminho: RegExp,
): Promise<PedidosRetidos> {
  let abrir!: () => void;
  const portao = new Promise<void>((resolver) => (abrir = resolver));
  let chegou!: () => void;
  const primeiro = new Promise<void>((resolver) => (chegou = resolver));

  const reter = async (route: Route): Promise<void> => {
    const pedido = route.request();
    if (
      pedido.method() === metodo &&
      caminho.test(new URL(pedido.url()).pathname)
    ) {
      chegou();
      await portao;
    }
    await route.fallback();
  };
  await page.route(TODOS_OS_PEDIDOS, reter);

  return {
    primeiro,
    async libertar() {
      abrir();
      await page.unroute(TODOS_OS_PEDIDOS, reter);
    },
  };
}

import type { Page, Request } from "@playwright/test";
import { URL_API } from "./ambiente";

/** Pedido feito pela página ao backend. */
export interface PedidoApi {
  metodo: string;
  /** Caminho sem o endereço do backend nem a query (ex.: «/clientes»). */
  caminho: string;
}

/**
 * Regista os pedidos feitos ao backend a partir deste momento.
 * Serve para confirmar que uma validação local não envia nada à API.
 */
export function registarPedidos(page: Page): PedidoApi[] {
  const pedidos: PedidoApi[] = [];
  page.on("request", (pedido: Request) => {
    const url = pedido.url();
    if (!url.startsWith(URL_API)) return;
    pedidos.push({
      metodo: pedido.method(),
      caminho: new URL(url).pathname,
    });
  });
  return pedidos;
}

/** Pedidos registados com este método e caminho (string exata ou expressão). */
export function pedidosA(
  pedidos: readonly PedidoApi[],
  metodo: string,
  caminho: string | RegExp,
): PedidoApi[] {
  return pedidos.filter(
    (p) =>
      p.metodo === metodo &&
      (typeof caminho === "string"
        ? p.caminho === caminho
        : caminho.test(p.caminho)),
  );
}

/** Espera pela resposta do backend a um pedido com este método e caminho. */
export function respostaDe(page: Page, metodo: string, caminho: RegExp) {
  return page.waitForResponse(
    (r) =>
      r.url().startsWith(URL_API) &&
      r.request().method() === metodo &&
      caminho.test(new URL(r.url()).pathname),
  );
}

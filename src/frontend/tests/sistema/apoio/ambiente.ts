/**
 * Endereços usados pelos testes de sistema (I128, secção 5.1).
 * Por omissão: frontend em `npm run dev` e backend real em localhost:5159.
 */
export const URL_WEB = (
  process.env.E2E_WEB_URL ?? "http://localhost:5173"
).replace(/\/+$/, "");

export const URL_API = (
  process.env.E2E_API_URL ?? "http://localhost:5159"
).replace(/\/+$/, "");

/**
 * Lê o URL do backend da variável de ambiente VITE_API_URL e remove as
 * barras finais, para que `${url}/auth/login` fique sempre bem formado.
 * Devolve "" se a variável não estiver definida (o ApiClient avisa).
 */
export function lerApiUrl(
  valor: string | undefined = import.meta.env.VITE_API_URL,
): string {
  return (valor ?? "").trim().replace(/\/+$/, "");
}

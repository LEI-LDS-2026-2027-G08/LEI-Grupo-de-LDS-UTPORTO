/** Estrutura de erro devolvida pelo backend (API v03). */
export interface ApiErro {
  code: number;
  message: string;
  detail?: string | null;
}

/** Resposta paginada devolvida pelas listagens do backend. */
export interface PagedResult<T> {
  itens: T[];
  pagina: number;
  tamanho: number;
  total: number;
}

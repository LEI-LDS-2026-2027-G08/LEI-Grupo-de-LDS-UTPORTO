import type { ReactNode } from "react";
import { mensagemDeErro } from "../core/api";

export interface ColunaTabela<T> {
  cabecalho: string;
  /** Devolve o conteúdo da célula para uma linha. */
  celula: (linha: T) => ReactNode;
}

interface DataTableProps<T> {
  colunas: readonly ColunaTabela<T>[];
  linhas: readonly T[];
  /** Identificador único da linha (para o React). */
  chaveLinha: (linha: T) => string | number;
  aCarregar?: boolean;
  /** Erro do pedido que alimentou a tabela (ApiRequestError ou outro). */
  erro?: unknown;
  /** Texto quando a lista está vazia. */
  mensagemVazia?: string;
}

/**
 * Tabela genérica com os três estados que toda a listagem precisa:
 * a carregar, com erro e vazia.
 *
 * @example
 * <DataTable
 *   colunas={[
 *     { cabecalho: "Nome", celula: (c) => c.nome },
 *     { cabecalho: "Email", celula: (c) => c.email },
 *   ]}
 *   linhas={clientes}
 *   chaveLinha={(c) => c.id}
 *   aCarregar={aCarregar}
 *   erro={erro}
 *   mensagemVazia="Ainda não há clientes."
 * />
 */
export function DataTable<T>({
  colunas,
  linhas,
  chaveLinha,
  aCarregar = false,
  erro = null,
  mensagemVazia = "Sem resultados.",
}: DataTableProps<T>) {
  if (aCarregar) {
    return <p role="status">A carregar…</p>;
  }
  if (erro !== null && erro !== undefined) {
    return (
      <div role="alert" className="form-erro">
        {mensagemDeErro(erro)}
      </div>
    );
  }
  if (linhas.length === 0) {
    return <p className="tabela-vazia">{mensagemVazia}</p>;
  }

  return (
    <table className="tabela">
      <thead>
        <tr>
          {colunas.map((coluna, indice) => (
            <th key={indice} scope="col">
              {coluna.cabecalho}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {linhas.map((linha) => (
          <tr key={chaveLinha(linha)}>
            {colunas.map((coluna, indice) => (
              <td key={indice}>{coluna.celula(linha)}</td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
}

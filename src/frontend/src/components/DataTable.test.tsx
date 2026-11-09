import { render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import { ApiRequestError } from "../core/api";
import { DataTable } from "./DataTable";

const colunas = [
  { cabecalho: "Nome", celula: (l: { nome: string }) => l.nome },
  { cabecalho: "Nome", celula: (l: { nome: string }) => l.nome.toUpperCase() },
];

describe("DataTable", () => {
  it("mostra 'A carregar…' enquanto carrega", () => {
    render(
      <DataTable
        colunas={colunas}
        linhas={[]}
        chaveLinha={() => 1}
        aCarregar
      />,
    );
    expect(screen.getByRole("status")).toHaveTextContent("A carregar");
  });

  it("mostra o erro de um pedido falhado", () => {
    render(
      <DataTable
        colunas={colunas}
        linhas={[]}
        chaveLinha={() => 1}
        erro={new ApiRequestError(0, "rede")}
      />,
    );
    expect(screen.getByRole("alert")).toHaveTextContent("contactar o servidor");
  });

  it("mostra a mensagem de lista vazia", () => {
    render(
      <DataTable
        colunas={colunas}
        linhas={[]}
        chaveLinha={() => 1}
        mensagemVazia="Ainda não há clientes."
      />,
    );
    expect(screen.getByText("Ainda não há clientes.")).toBeInTheDocument();
  });

  it("aceita colunas com o mesmo cabeçalho sem chaves repetidas", () => {
    const erros = vi.spyOn(console, "error").mockImplementation(() => {});
    render(
      <DataTable
        colunas={colunas}
        linhas={[{ nome: "Rex" }]}
        chaveLinha={(l) => l.nome}
      />,
    );
    expect(screen.getAllByRole("columnheader")).toHaveLength(2);
    expect(screen.getByText("REX")).toBeInTheDocument();
    expect(erros).not.toHaveBeenCalled();
    erros.mockRestore();
  });
});

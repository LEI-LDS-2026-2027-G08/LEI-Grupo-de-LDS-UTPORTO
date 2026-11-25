import { useEffect, useState, type FormEvent } from "react";
import { Link } from "react-router-dom";
import { DataTable, type ColunaTabela } from "../../components/DataTable";
import { SoPara } from "../../components/SoPara";
import type { ClienteFiltros, ClienteResumo } from "../../models/clientes";
import type { PagedResult } from "../../models/common";
import {
  clientesService,
  type ServicoClientes,
} from "../../services/ClientesService";

const TAMANHO_PAGINA = 20;

interface ClientesPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoClientes;
}

interface Resultado {
  filtros: ClienteFiltros;
  dados: PagedResult<ClienteResumo> | null;
  erro: unknown;
}

/** W04 — Lista e pesquisa de Clientes (EP-011). Administrador e Rececionista. */
export function ClientesPage({ servico = clientesService }: ClientesPageProps) {
  const [pesquisa, setPesquisa] = useState("");
  const [filtros, setFiltros] = useState<ClienteFiltros>({
    pagina: 1,
    tamanho: TAMANHO_PAGINA,
  });
  const [resultado, setResultado] = useState<Resultado | null>(null);

  useEffect(() => {
    let cancelado = false;
    servico
      .listar(filtros)
      .then((dados) => {
        if (!cancelado) setResultado({ filtros, dados, erro: null });
      })
      .catch((erro: unknown) => {
        if (!cancelado) setResultado({ filtros, dados: null, erro });
      });
    return () => {
      cancelado = true;
    };
  }, [servico, filtros]);

  const aCarregar = resultado === null || resultado.filtros !== filtros;

  function pesquisar(evento: FormEvent) {
    evento.preventDefault();
    setFiltros({
      pesquisa: pesquisa.trim() || undefined,
      pagina: 1,
      tamanho: TAMANHO_PAGINA,
    });
  }

  function irParaPagina(pagina: number) {
    setFiltros((atuais) => ({ ...atuais, pagina }));
  }

  const colunas: ColunaTabela<ClienteResumo>[] = [
    { cabecalho: "Nome", celula: (c) => c.nome },
    { cabecalho: "NIF", celula: (c) => c.nif ?? "—" },
    { cabecalho: "Telefone", celula: (c) => c.telefone },
    { cabecalho: "E-mail", celula: (c) => c.email },
    { cabecalho: "Animais", celula: (c) => c.numeroAnimais },
    {
      cabecalho: "Ações",
      celula: (c) => (
        <Link to={`/clientes/${c.id}`} aria-label={`Ver / editar ${c.nome}`}>
          Ver / editar
        </Link>
      ),
    },
  ];

  const dados = aCarregar ? null : (resultado?.dados ?? null);
  const totalPaginas = dados
    ? Math.max(1, Math.ceil(dados.total / dados.tamanho))
    : 1;

  return (
    <section>
      <div className="titulo-pagina">
        <h1>Clientes</h1>
        <SoPara acao="gerirClientes">
          <Link to="/clientes/novo" className="botao">
            Novo cliente
          </Link>
        </SoPara>
      </div>

      <form className="filtros" onSubmit={pesquisar} aria-label="Pesquisa">
        <div className="campo">
          <label htmlFor="pesquisa-clientes">
            Pesquisar (nome, NIF, telefone ou e-mail)
          </label>
          <input
            id="pesquisa-clientes"
            type="search"
            value={pesquisa}
            maxLength={100}
            onChange={(e) => setPesquisa(e.target.value)}
          />
        </div>
        <button type="submit">Pesquisar</button>
      </form>

      <DataTable
        colunas={colunas}
        linhas={dados?.itens ?? []}
        chaveLinha={(c) => c.id}
        aCarregar={aCarregar}
        erro={aCarregar ? null : resultado?.erro}
        mensagemVazia={
          filtros.pesquisa
            ? "Nenhum cliente corresponde à pesquisa."
            : "Ainda não há clientes registados."
        }
      />

      {dados && dados.total > 0 ? (
        <nav className="paginacao" aria-label="Páginas">
          <button
            type="button"
            className="secundario"
            disabled={filtros.pagina <= 1}
            onClick={() => irParaPagina(filtros.pagina - 1)}
          >
            Anterior
          </button>
          <span>
            Página {dados.pagina} de {totalPaginas} · {dados.total}{" "}
            {dados.total === 1 ? "cliente" : "clientes"}
          </span>
          <button
            type="button"
            className="secundario"
            disabled={filtros.pagina >= totalPaginas}
            onClick={() => irParaPagina(filtros.pagina + 1)}
          >
            Seguinte
          </button>
        </nav>
      ) : null}
    </section>
  );
}

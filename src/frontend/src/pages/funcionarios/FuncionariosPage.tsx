import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useLocation } from "react-router-dom";
import { ConfirmarBloqueio } from "../../components/ConfirmarBloqueio";
import { DataTable, type ColunaTabela } from "../../components/DataTable";
import type { PagedResult } from "../../models/common";
import {
  ESTADOS_UTILIZADOR,
  PERFIS_INTERNOS,
  ROTULO_PERFIL,
  type EstadoUtilizador,
  type FuncionarioResumo,
  type PerfilInterno,
  type UtilizadorFiltros,
} from "../../models/utilizadores";
import {
  utilizadoresService,
  type ServicoUtilizadores,
} from "../../services/UtilizadoresService";

const TAMANHO_PAGINA = 20;

interface FuncionariosPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoUtilizadores;
}

interface Resultado {
  filtros: UtilizadorFiltros;
  dados: PagedResult<FuncionarioResumo> | null;
  erro: unknown;
}

/** W02 — Lista de funcionários (EP-004 e EP-008). Só para o Administrador. */
export function FuncionariosPage({
  servico = utilizadoresService,
}: FuncionariosPageProps) {
  const localizacao = useLocation();
  const avisoInicial =
    (localizacao.state as { aviso?: string } | null)?.aviso ?? null;

  const [pesquisa, setPesquisa] = useState("");
  const [perfil, setPerfil] = useState<PerfilInterno | "">("");
  const [estado, setEstado] = useState<EstadoUtilizador | "">("");
  const [filtros, setFiltros] = useState<UtilizadorFiltros>({
    pagina: 1,
    tamanho: TAMANHO_PAGINA,
  });
  const [resultado, setResultado] = useState<Resultado | null>(null);
  const [recarregar, setRecarregar] = useState(0);
  const [aBloquear, setABloquear] = useState<FuncionarioResumo | null>(null);
  const [aviso, setAviso] = useState<string | null>(avisoInicial);

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
  }, [servico, filtros, recarregar]);

  const aCarregar = resultado === null || resultado.filtros !== filtros;

  function pesquisar(evento: FormEvent) {
    evento.preventDefault();
    setAviso(null);
    setFiltros({
      pesquisa: pesquisa.trim() || undefined,
      perfil: perfil || undefined,
      estado: estado || undefined,
      pagina: 1,
      tamanho: TAMANHO_PAGINA,
    });
  }

  function irParaPagina(pagina: number) {
    setFiltros((atuais) => ({ ...atuais, pagina }));
  }

  const confirmarBloqueio = useCallback(
    async (motivo: string) => {
      if (aBloquear === null) return;
      await servico.bloquear(aBloquear.id, motivo);
      setAviso(`A conta de ${aBloquear.nome} foi bloqueada.`);
      setABloquear(null);
      setRecarregar((n) => n + 1);
    },
    [aBloquear, servico],
  );

  const colunas: ColunaTabela<FuncionarioResumo>[] = [
    { cabecalho: "Nome", celula: (f) => f.nome },
    { cabecalho: "E-mail", celula: (f) => f.email },
    { cabecalho: "Perfil", celula: (f) => ROTULO_PERFIL[f.perfil] ?? f.perfil },
    { cabecalho: "Estado", celula: (f) => f.estado },
    {
      cabecalho: "Registo de apoio",
      celula: (f) =>
        f.perfil === "AuxiliarVeterinario"
          ? f.podeRegistarApoio
            ? "Autorizado"
            : "Não autorizado"
          : "—",
    },
    {
      cabecalho: "Ações",
      celula: (f) => (
        <div className="acoes-linha">
          <Link to={`/funcionarios/${f.id}`}>Ver / editar</Link>
          {f.estado === "Ativo" ? (
            <button
              type="button"
              className="ligacao perigo-texto"
              onClick={() => {
                setAviso(null);
                setABloquear(f);
              }}
            >
              Bloquear
            </button>
          ) : null}
        </div>
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
        <h1>Funcionários</h1>
        <Link to="/funcionarios/novo" className="botao">
          Novo funcionário
        </Link>
      </div>

      <form className="filtros" onSubmit={pesquisar} aria-label="Filtros">
        <div className="campo">
          <label htmlFor="filtro-pesquisa">Pesquisar (nome ou e-mail)</label>
          <input
            id="filtro-pesquisa"
            value={pesquisa}
            maxLength={100}
            onChange={(e) => setPesquisa(e.target.value)}
          />
        </div>
        <div className="campo">
          <label htmlFor="filtro-perfil">Perfil</label>
          <select
            id="filtro-perfil"
            value={perfil}
            onChange={(e) => setPerfil(e.target.value as PerfilInterno | "")}
          >
            <option value="">Todos</option>
            {PERFIS_INTERNOS.map((p) => (
              <option key={p} value={p}>
                {ROTULO_PERFIL[p]}
              </option>
            ))}
          </select>
        </div>
        <div className="campo">
          <label htmlFor="filtro-estado">Estado</label>
          <select
            id="filtro-estado"
            value={estado}
            onChange={(e) => setEstado(e.target.value as EstadoUtilizador | "")}
          >
            <option value="">Todos</option>
            {ESTADOS_UTILIZADOR.map((e) => (
              <option key={e} value={e}>
                {e}
              </option>
            ))}
          </select>
        </div>
        <button type="submit">Pesquisar</button>
      </form>

      {aviso ? (
        <p role="status" className="sucesso">
          {aviso}
        </p>
      ) : null}

      {aBloquear ? (
        <ConfirmarBloqueio
          key={aBloquear.id}
          nome={aBloquear.nome}
          aoConfirmar={confirmarBloqueio}
          aoCancelar={() => setABloquear(null)}
        />
      ) : null}

      <DataTable
        colunas={colunas}
        linhas={dados?.itens ?? []}
        chaveLinha={(f) => f.id}
        aCarregar={aCarregar}
        erro={aCarregar ? null : resultado?.erro}
        mensagemVazia="Não há funcionários com estes filtros."
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
            {dados.total === 1 ? "funcionário" : "funcionários"}
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

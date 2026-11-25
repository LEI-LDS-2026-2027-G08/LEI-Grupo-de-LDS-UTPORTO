import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { DataTable, type ColunaTabela } from "../../components/DataTable";
import { FormErro } from "../../components/FormErro";
import { SoPara } from "../../components/SoPara";
import { useAuth } from "../../core/auth/useAuth";
import {
  formatarDiaCurto,
  hojeEmLisboa,
  somarDias,
  utcParaLisboa,
} from "../../core/horaLisboa";
import {
  ESTADOS_CONSULTA,
  type AgendaFiltros,
  type EstadoConsulta,
  type MarcacaoResumo,
  type VeterinarioResumo,
} from "../../models/agenda";
import type { PagedResult } from "../../models/common";
import {
  agendaService,
  type ServicoAgenda,
} from "../../services/AgendaService";

/** Uma página por dia; a API aceita até 100 por página. */
const TAMANHO_PAGINA = 100;

interface AgendaPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoAgenda;
}

interface Resultado {
  filtros: AgendaFiltros;
  dados: PagedResult<MarcacaoResumo> | null;
  erro: unknown;
}

/**
 * W09 — Agenda (EP-023). A Rececionista vê a agenda da clínica, com filtro
 * por Veterinário; o Veterinário vê só as consultas que lhe estão atribuídas
 * (a API filtra pelo token). Um dia de cada vez.
 */
export function AgendaPage({ servico = agendaService }: AgendaPageProps) {
  const { pode } = useAuth();
  const eRececionista = pode("gerirMarcacoes");

  const [dia, setDia] = useState(() => hojeEmLisboa());
  const [veterinarioId, setVeterinarioId] = useState("");
  const [estado, setEstado] = useState<EstadoConsulta | "">("");
  const [pagina, setPagina] = useState(1);
  const [veterinarios, setVeterinarios] = useState<VeterinarioResumo[]>([]);
  const [erroVeterinarios, setErroVeterinarios] = useState<unknown>(null);
  const [resultado, setResultado] = useState<Resultado | null>(null);

  // A lista de Veterinários (EP-021) só existe para a Rececionista.
  useEffect(() => {
    if (!eRececionista) return;
    let cancelado = false;
    servico
      .listarVeterinarios()
      .then((lista) => {
        if (!cancelado) setVeterinarios(lista);
      })
      .catch((e: unknown) => {
        if (!cancelado) setErroVeterinarios(e);
      });
    return () => {
      cancelado = true;
    };
  }, [servico, eRececionista]);

  const filtros: AgendaFiltros = {
    dataInicio: dia,
    dataFim: dia,
    veterinarioId:
      eRececionista && veterinarioId ? Number(veterinarioId) : undefined,
    estado: estado || undefined,
    pagina,
    tamanho: TAMANHO_PAGINA,
  };
  const chave = JSON.stringify(filtros);

  useEffect(() => {
    let cancelado = false;
    const pedido: AgendaFiltros = JSON.parse(chave);
    servico
      .listarConsultas(pedido)
      .then((dados) => {
        if (!cancelado) setResultado({ filtros: pedido, dados, erro: null });
      })
      .catch((erro: unknown) => {
        if (!cancelado) setResultado({ filtros: pedido, dados: null, erro });
      });
    return () => {
      cancelado = true;
    };
  }, [servico, chave]);

  const aCarregar =
    resultado === null || JSON.stringify(resultado.filtros) !== chave;

  function mudarDia(novo: string) {
    setDia(novo);
    setPagina(1);
  }

  const colunas: ColunaTabela<MarcacaoResumo>[] = [
    { cabecalho: "Hora", celula: (m) => utcParaLisboa(m.dataHora).hora },
    { cabecalho: "Paciente", celula: (m) => m.animal.nome },
    { cabecalho: "Cliente", celula: (m) => m.cliente.nome },
    { cabecalho: "Veterinário", celula: (m) => m.veterinario.nome },
    { cabecalho: "Motivo", celula: (m) => m.motivo ?? "—" },
    { cabecalho: "Estado", celula: (m) => m.estado },
    {
      cabecalho: "Chegada",
      celula: (m) => (m.chegadaEm ? utcParaLisboa(m.chegadaEm).hora : "—"),
    },
    {
      cabecalho: "Ações",
      celula: (m) => (
        <Link
          to={`/agenda/${m.id}`}
          aria-label={`Ver marcação de ${m.animal.nome} às ${utcParaLisboa(m.dataHora).hora}`}
        >
          Ver
        </Link>
      ),
    },
  ];

  const dados = aCarregar ? null : (resultado?.dados ?? null);
  const totalPaginas = dados
    ? Math.max(1, Math.ceil(dados.total / dados.tamanho))
    : 1;
  const hoje = hojeEmLisboa();

  return (
    <section>
      <div className="titulo-pagina">
        <h1>Agenda</h1>
        <SoPara acao="gerirMarcacoes">
          <Link to="/agenda/nova" className="botao">
            Nova marcação
          </Link>
        </SoPara>
      </div>

      {!eRececionista ? (
        <p className="detalhe-conta">
          Mostra só as consultas que lhe estão atribuídas.
        </p>
      ) : null}

      <div className="filtros" role="group" aria-label="Filtros da agenda">
        <div className="campo">
          <label htmlFor="agenda-dia">Dia</label>
          <input
            id="agenda-dia"
            type="date"
            value={dia}
            onChange={(e) => {
              if (e.target.value) mudarDia(e.target.value);
            }}
          />
        </div>
        {eRececionista ? (
          <div className="campo">
            <label htmlFor="agenda-veterinario">Veterinário</label>
            <select
              id="agenda-veterinario"
              value={veterinarioId}
              onChange={(e) => {
                setVeterinarioId(e.target.value);
                setPagina(1);
              }}
            >
              <option value="">Todos</option>
              {veterinarios.map((v) => (
                <option key={v.id} value={v.id}>
                  {v.nome}
                </option>
              ))}
            </select>
          </div>
        ) : null}
        <div className="campo">
          <label htmlFor="agenda-estado">Estado</label>
          <select
            id="agenda-estado"
            value={estado}
            onChange={(e) => {
              setEstado(e.target.value as EstadoConsulta | "");
              setPagina(1);
            }}
          >
            <option value="">Todos</option>
            {ESTADOS_CONSULTA.map((e) => (
              <option key={e} value={e}>
                {e}
              </option>
            ))}
          </select>
        </div>
      </div>
      {erroVeterinarios ? <FormErro erro={erroVeterinarios} /> : null}

      <nav className="acoes" aria-label="Dias" style={{ marginBottom: "1rem" }}>
        <button
          type="button"
          className="secundario"
          onClick={() => mudarDia(somarDias(dia, -1))}
        >
          Dia anterior
        </button>
        <button
          type="button"
          className="secundario"
          disabled={dia === hoje}
          onClick={() => mudarDia(hoje)}
        >
          Hoje
        </button>
        <button
          type="button"
          className="secundario"
          onClick={() => mudarDia(somarDias(dia, 1))}
        >
          Dia seguinte
        </button>
        <strong>{formatarDiaCurto(dia)}</strong>
      </nav>

      <DataTable
        colunas={colunas}
        linhas={dados?.itens ?? []}
        chaveLinha={(m) => m.id}
        aCarregar={aCarregar}
        erro={aCarregar ? null : resultado?.erro}
        mensagemVazia="Não há marcações neste dia com estes filtros."
      />

      {dados && dados.total > dados.tamanho ? (
        <nav className="paginacao" aria-label="Páginas">
          <button
            type="button"
            className="secundario"
            disabled={pagina <= 1}
            onClick={() => setPagina(pagina - 1)}
          >
            Anterior
          </button>
          <span>
            Página {dados.pagina} de {totalPaginas}
          </span>
          <button
            type="button"
            className="secundario"
            disabled={pagina >= totalPaginas}
            onClick={() => setPagina(pagina + 1)}
          >
            Seguinte
          </button>
        </nav>
      ) : null}
      {dados && dados.total > 0 ? (
        <p className="detalhe-conta">
          {dados.total} {dados.total === 1 ? "marcação" : "marcações"} neste
          dia.
        </p>
      ) : null}
    </section>
  );
}

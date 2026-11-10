import { useEffect, useState, type FormEvent } from "react";
import { Link } from "react-router-dom";
import { DataTable, type ColunaTabela } from "../../components/DataTable";
import { SoPara } from "../../components/SoPara";
import type { PagedResult } from "../../models/common";
import {
  ESPECIES,
  type PacienteFiltros,
  type PacienteResumo,
  type ProprietarioResumo,
} from "../../models/pacientes";
import type { ServicoClientes } from "../../services/ClientesService";
import {
  pacientesService,
  type ServicoPacientes,
} from "../../services/PacientesService";
import { formatarIdade, formatarPeso } from "./formatacao";
import { SeletorProprietario } from "./SeletorProprietario";

const TAMANHO_PAGINA = 20;

interface PacientesPageProps {
  /** Só para testes: permitem trocar os serviços. */
  servico?: ServicoPacientes;
  servicoClientes?: Pick<ServicoClientes, "listar">;
}

interface Resultado {
  filtros: PacienteFiltros;
  dados: PagedResult<PacienteResumo> | null;
  erro: unknown;
}

/**
 * W07 — Lista e pesquisa de pacientes (EP-015), com pesquisa por
 * proprietário (EP-011 em modo de consulta). Rececionista e Veterinário.
 */
export function PacientesPage({
  servico = pacientesService,
  servicoClientes,
}: PacientesPageProps) {
  const [pesquisa, setPesquisa] = useState("");
  const [especie, setEspecie] = useState("");
  const [proprietario, setProprietario] = useState<ProprietarioResumo | null>(
    null,
  );
  const [filtros, setFiltros] = useState<PacienteFiltros>({
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
      clienteId: proprietario?.id,
      pesquisa: pesquisa.trim() || undefined,
      especie: especie || undefined,
      pagina: 1,
      tamanho: TAMANHO_PAGINA,
    });
  }

  // Escolher ou remover o proprietário filtra logo a lista.
  function alterarProprietario(novo: ProprietarioResumo | null) {
    setProprietario(novo);
    setFiltros((atuais) => ({
      ...atuais,
      clienteId: novo?.id,
      pagina: 1,
    }));
  }

  function irParaPagina(pagina: number) {
    setFiltros((atuais) => ({ ...atuais, pagina }));
  }

  const colunas: ColunaTabela<PacienteResumo>[] = [
    { cabecalho: "Nome", celula: (p) => p.nome },
    { cabecalho: "Espécie", celula: (p) => p.especie },
    { cabecalho: "Raça", celula: (p) => p.raca ?? "—" },
    { cabecalho: "Idade", celula: (p) => formatarIdade(p.idadeAnos) },
    { cabecalho: "Peso", celula: (p) => formatarPeso(p.peso) },
    { cabecalho: "Proprietário", celula: (p) => p.cliente.nome },
    {
      cabecalho: "Ações",
      celula: (p) => (
        <Link to={`/pacientes/${p.id}`} aria-label={`Ver ficha de ${p.nome}`}>
          Ver ficha
        </Link>
      ),
    },
  ];

  const dados = aCarregar ? null : (resultado?.dados ?? null);
  const totalPaginas = dados
    ? Math.max(1, Math.ceil(dados.total / dados.tamanho))
    : 1;
  const comFiltros = Boolean(
    filtros.clienteId || filtros.pesquisa || filtros.especie,
  );

  return (
    <section>
      <div className="titulo-pagina">
        <h1>Pacientes</h1>
        <SoPara acao="gerirPacientes">
          <Link
            to="/pacientes/novo"
            state={proprietario ? { proprietario } : undefined}
            className="botao"
          >
            Novo paciente
          </Link>
        </SoPara>
      </div>

      <SeletorProprietario
        selecionado={proprietario}
        aoSelecionar={alterarProprietario}
        rotuloLimpar="Remover filtro"
        servico={servicoClientes}
      />

      <form className="filtros" onSubmit={pesquisar} aria-label="Pesquisa">
        <div className="campo">
          <label htmlFor="pesquisa-pacientes">
            Pesquisar (nome do animal ou do proprietário)
          </label>
          <input
            id="pesquisa-pacientes"
            type="search"
            value={pesquisa}
            maxLength={100}
            onChange={(e) => setPesquisa(e.target.value)}
          />
        </div>
        <div className="campo">
          <label htmlFor="filtro-especie">Espécie</label>
          <select
            id="filtro-especie"
            value={especie}
            onChange={(e) => setEspecie(e.target.value)}
          >
            <option value="">Todas</option>
            {ESPECIES.map((e) => (
              <option key={e} value={e}>
                {e}
              </option>
            ))}
          </select>
        </div>
        <button type="submit">Pesquisar</button>
      </form>

      <DataTable
        colunas={colunas}
        linhas={dados?.itens ?? []}
        chaveLinha={(p) => p.id}
        aCarregar={aCarregar}
        erro={aCarregar ? null : resultado?.erro}
        mensagemVazia={
          comFiltros
            ? "Nenhum paciente corresponde à pesquisa."
            : "Ainda não há pacientes registados."
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
            {dados.total === 1 ? "paciente" : "pacientes"}
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

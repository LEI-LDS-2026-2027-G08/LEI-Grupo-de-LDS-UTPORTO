import { useEffect, useId, useState, type FormEvent } from "react";
import { DataTable, type ColunaTabela } from "../../components/DataTable";
import { FormErro } from "../../components/FormErro";
import { ApiRequestError } from "../../core/api";
import {
  formatarDataHoraLisboa,
  formatarDiaCurto,
  hojeEmLisboa,
  somarDias,
  utcParaLisboa,
} from "../../core/horaLisboa";
import {
  ESTADOS_CONSULTA,
  type AgendaFiltros,
  type EstadoConsulta,
  type MarcacaoDetalhe,
  type MarcacaoResumo,
} from "../../models/agenda";
import type { PagedResult } from "../../models/common";
import { ROTULO_SEXO, type PacienteDetalhe } from "../../models/pacientes";
import {
  agendaService,
  type ServicoAgenda,
} from "../../services/AgendaService";
import {
  pacientesService,
  type ServicoPacientes,
} from "../../services/PacientesService";
import {
  formatarData,
  formatarIdade,
  formatarPeso,
} from "../pacientes/formatacao";

const TAMANHO_PAGINA = 100;
const MAXIMO_NOTA = 1000;

interface PreparacaoConsultaPageProps {
  /** Só para testes: permitem trocar os serviços. */
  servico?: ServicoAgenda;
  servicoPacientes?: Pick<ServicoPacientes, "obter">;
}

interface Resultado {
  chave: string;
  dados: PagedResult<MarcacaoResumo> | null;
  erro: unknown;
}

interface Selecao {
  marcacao: MarcacaoDetalhe;
  /** Ficha administrativa (EP-016); null se não for possível lê-la. */
  paciente: PacienteDetalhe | null;
}

/**
 * W12 — Preparação de consultas (EP-023, EP-024, EP-016 e EP-029). Só para o
 * Auxiliar Veterinário: lista as consultas de um dia, mostra a informação de
 * preparação da escolhida e regista notas não clínicas de apoio quando a
 * conta tem essa autorização (a API recusa com 403 quando não tem).
 */
export function PreparacaoConsultaPage({
  servico = agendaService,
  servicoPacientes = pacientesService,
}: PreparacaoConsultaPageProps) {
  const idCampos = useId();
  const [dia, setDia] = useState(() => hojeEmLisboa());
  const [estado, setEstado] = useState<EstadoConsulta | "">("");
  const [resultado, setResultado] = useState<Resultado | null>(null);
  const [selecao, setSelecao] = useState<Selecao | null>(null);
  const [aAbrir, setAAbrir] = useState(false);
  const [erroSelecao, setErroSelecao] = useState<unknown>(null);
  const [texto, setTexto] = useState("");
  const [erroNota, setErroNota] = useState<unknown>(null);
  const [erroTexto, setErroTexto] = useState<string | null>(null);
  const [aGuardar, setAGuardar] = useState(false);
  const [aviso, setAviso] = useState<string | null>(null);
  const [semAutorizacao, setSemAutorizacao] = useState(false);

  const chave = JSON.stringify({ dia, estado });

  useEffect(() => {
    let cancelado = false;
    const { dia: d, estado: e } = JSON.parse(chave) as {
      dia: string;
      estado: EstadoConsulta | "";
    };
    const filtros: AgendaFiltros = {
      dataInicio: d,
      dataFim: d,
      estado: e || undefined,
      pagina: 1,
      tamanho: TAMANHO_PAGINA,
    };
    servico
      .listarConsultas(filtros)
      .then((dados) => {
        if (!cancelado) setResultado({ chave, dados, erro: null });
      })
      .catch((erro: unknown) => {
        if (!cancelado) setResultado({ chave, dados: null, erro });
      });
    return () => {
      cancelado = true;
    };
  }, [servico, chave]);

  const aCarregar = resultado === null || resultado.chave !== chave;

  async function selecionar(id: number) {
    setAAbrir(true);
    setErroSelecao(null);
    setErroNota(null);
    setErroTexto(null);
    setAviso(null);
    setTexto("");
    try {
      const marcacao = await servico.obterConsulta(id);
      let paciente: PacienteDetalhe | null = null;
      try {
        paciente = await servicoPacientes.obter(marcacao.animal.id);
      } catch {
        // A marcação continua útil sem a ficha; mostra-se o que houver.
      }
      setSelecao({ marcacao, paciente });
    } catch (e) {
      setSelecao(null);
      setErroSelecao(e);
    } finally {
      setAAbrir(false);
    }
  }

  async function guardarNota(evento: FormEvent) {
    evento.preventDefault();
    if (selecao === null) return;
    setErroNota(null);
    setAviso(null);
    const limpo = texto.trim();
    if (limpo === "") {
      setErroTexto("Escreva a informação de apoio.");
      return;
    }
    if (limpo.length > MAXIMO_NOTA) {
      setErroTexto("O texto da nota não pode ter mais de 1000 caracteres.");
      return;
    }
    setErroTexto(null);
    setAGuardar(true);
    try {
      const nota = await servico.registarNotaPreparacao(
        selecao.marcacao.id,
        limpo,
      );
      setSelecao((atual) =>
        atual
          ? {
              ...atual,
              marcacao: {
                ...atual.marcacao,
                notasPreparacao: [...atual.marcacao.notasPreparacao, nota],
              },
            }
          : atual,
      );
      setTexto("");
      setAviso("Informação de apoio guardada.");
    } catch (e) {
      setErroNota(e);
      // 403: a conta não tem a autorização de registo de apoio (critério 4).
      if (e instanceof ApiRequestError && e.categoria === "semPermissao") {
        setSemAutorizacao(true);
      }
    } finally {
      setAGuardar(false);
    }
  }

  const colunas: ColunaTabela<MarcacaoResumo>[] = [
    { cabecalho: "Hora", celula: (m) => utcParaLisboa(m.dataHora).hora },
    { cabecalho: "Paciente", celula: (m) => m.animal.nome },
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
        <button
          type="button"
          className="ligacao"
          aria-label={`Selecionar a consulta de ${m.animal.nome} às ${utcParaLisboa(m.dataHora).hora}`}
          disabled={aAbrir}
          onClick={() => void selecionar(m.id)}
        >
          Selecionar
        </button>
      ),
    },
  ];

  const dados = aCarregar ? null : (resultado?.dados ?? null);
  const marcacao = selecao?.marcacao ?? null;
  const paciente = selecao?.paciente ?? null;

  return (
    <section>
      <h1>Preparação de consultas</h1>

      <div className="filtros" role="group" aria-label="Filtros">
        <div className="campo">
          <label htmlFor={`${idCampos}-dia`}>Dia</label>
          <input
            id={`${idCampos}-dia`}
            type="date"
            value={dia}
            onChange={(e) => {
              if (e.target.value) setDia(e.target.value);
            }}
          />
        </div>
        <div className="campo">
          <label htmlFor={`${idCampos}-estado`}>Estado</label>
          <select
            id={`${idCampos}-estado`}
            value={estado}
            onChange={(e) => setEstado(e.target.value as EstadoConsulta | "")}
          >
            <option value="">Todos</option>
            {ESTADOS_CONSULTA.map((e) => (
              <option key={e} value={e}>
                {e}
              </option>
            ))}
          </select>
        </div>
        <button
          type="button"
          className="secundario"
          onClick={() => setDia(somarDias(dia, -1))}
        >
          Dia anterior
        </button>
        <button
          type="button"
          className="secundario"
          onClick={() => setDia(somarDias(dia, 1))}
        >
          Dia seguinte
        </button>
      </div>
      <p>
        <strong>{formatarDiaCurto(dia)}</strong>
      </p>

      <DataTable
        colunas={colunas}
        linhas={dados?.itens ?? []}
        chaveLinha={(m) => m.id}
        aCarregar={aCarregar}
        erro={aCarregar ? null : resultado?.erro}
        mensagemVazia="Não há consultas neste dia com estes filtros."
      />

      {aAbrir ? <p role="status">A abrir a consulta…</p> : null}
      {erroSelecao ? <FormErro erro={erroSelecao} /> : null}

      {marcacao ? (
        <section aria-labelledby={`${idCampos}-selecionada`}>
          <h2 id={`${idCampos}-selecionada`}>Consulta selecionada</h2>
          <dl className="formulario" aria-label="Informação de preparação">
            <dt>Consulta</dt>
            <dd>
              {formatarDataHoraLisboa(marcacao.dataHora)} ·{" "}
              {marcacao.duracaoMinutos} minutos · {marcacao.estado}
            </dd>
            <dt>Veterinário</dt>
            <dd>{marcacao.veterinario.nome}</dd>
            <dt>Motivo</dt>
            <dd>{marcacao.motivo ?? "—"}</dd>
            <dt>Chegada</dt>
            <dd>
              {marcacao.chegadaEm
                ? formatarDataHoraLisboa(marcacao.chegadaEm)
                : "Não registada"}
            </dd>
            <dt>Paciente</dt>
            <dd>
              {marcacao.animal.nome}
              {paciente
                ? ` · ${paciente.especie}${paciente.raca ? ` (${paciente.raca})` : ""} · ${ROTULO_SEXO[paciente.sexo] ?? paciente.sexo}`
                : marcacao.animal.especie
                  ? ` · ${marcacao.animal.especie}`
                  : ""}
            </dd>
            {paciente ? (
              <>
                <dt>Idade e peso</dt>
                <dd>
                  {formatarIdade(paciente.idadeAnos)} ·{" "}
                  {formatarPeso(paciente.peso)} · nascido a{" "}
                  {formatarData(paciente.dataNascimento)}
                </dd>
              </>
            ) : null}
            <dt>Proprietário</dt>
            <dd>
              {marcacao.cliente.nome}
              {paciente?.cliente.telefone
                ? ` · ${paciente.cliente.telefone}`
                : ""}
            </dd>
          </dl>

          <h3>Notas de apoio</h3>
          {marcacao.notasPreparacao.length === 0 ? (
            <p className="tabela-vazia">Ainda não há notas de apoio.</p>
          ) : (
            <ul aria-label="Notas de apoio">
              {marcacao.notasPreparacao.map((nota) => (
                <li key={nota.id}>
                  {nota.texto} — {nota.autor},{" "}
                  {formatarDataHoraLisboa(nota.criadaEm)}
                </li>
              ))}
            </ul>
          )}

          {aviso ? (
            <p role="status" className="sucesso">
              {aviso}
            </p>
          ) : null}

          {semAutorizacao ? (
            <>
              <FormErro erro={erroNota} />
              <p className="detalhe-conta">
                A sua conta não tem autorização para registar informação de
                apoio. Esta autorização é dada pelo Administrador, na conta de
                funcionário.
              </p>
            </>
          ) : (
            <form
              className="formulario"
              onSubmit={guardarNota}
              aria-label="Informação de apoio não clínica"
              noValidate
            >
              <FormErro erro={erroNota} />
              <div className="campo">
                <label htmlFor={`${idCampos}-nota`}>
                  Nova nota de apoio (não clínica)
                </label>
                <textarea
                  id={`${idCampos}-nota`}
                  rows={3}
                  maxLength={MAXIMO_NOTA}
                  style={{ font: "inherit", padding: "0.55rem 0.7rem" }}
                  value={texto}
                  aria-invalid={erroTexto ? true : undefined}
                  aria-describedby={
                    erroTexto ? `${idCampos}-nota-erro` : undefined
                  }
                  onChange={(e) => {
                    setTexto(e.target.value);
                    setErroTexto(null);
                  }}
                />
                {erroTexto ? (
                  <p id={`${idCampos}-nota-erro`} className="campo-erro">
                    {erroTexto}
                  </p>
                ) : null}
              </div>
              <button type="submit" disabled={aGuardar}>
                {aGuardar ? "A guardar…" : "Guardar informação de apoio"}
              </button>
            </form>
          )}
        </section>
      ) : null}
    </section>
  );
}

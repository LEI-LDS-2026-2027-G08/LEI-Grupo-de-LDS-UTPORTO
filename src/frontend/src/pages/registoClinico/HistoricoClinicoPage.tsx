import {
  useEffect,
  useId,
  useState,
  type FormEvent,
  type ReactNode,
} from "react";
import { Link, useParams } from "react-router-dom";
import { DataTable, type ColunaTabela } from "../../components/DataTable";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import { useAuth } from "../../core/auth/useAuth";
import { SoPara } from "../../components/SoPara";
import { formatarDataHoraLisboa } from "../../core/horaLisboa";
import type {
  HistoricoClinico,
  HistoricoClinicoFiltros,
  RegistoClinico,
  RegistoClinicoResumo,
} from "../../models/registosClinicos";
import {
  registosClinicosService,
  type ServicoRegistosClinicos,
} from "../../services/RegistosClinicosService";
import { formatarData } from "../pacientes/formatacao";
import { validarPeriodo } from "./periodoHistorico";

interface HistoricoClinicoPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: Pick<ServicoRegistosClinicos, "obterHistorico" | "obter">;
}

interface ResultadoHistorico {
  chave: string;
  dados: HistoricoClinico | null;
  erro: unknown;
}

interface ResultadoRegisto {
  chave: string;
  dados: RegistoClinico | null;
  erro: unknown;
}

/**
 * W13 — Histórico clínico do paciente (RF035, RF036; EP-034 e EP-033).
 * Veterinário e Auxiliar Veterinário: a rota está protegida por perfil e a
 * API recusa (403) os outros perfis.
 *
 * O Auxiliar vê apenas tratamentos e prescrições; só o
 * Veterinário tem a ação «Alterar registo» (W13 da consulta, I115). O
 * Auxiliar tem uma vista só de leitura, sem botões de criação ou alteração.
 *
 * A chave obriga a montar a página de novo quando o :id muda.
 */
export function HistoricoClinicoPage(props: HistoricoClinicoPageProps) {
  const { id } = useParams();
  return <HistoricoDoPaciente key={id} idTexto={id} {...props} />;
}

interface HistoricoDoPacienteProps extends HistoricoClinicoPageProps {
  idTexto: string | undefined;
}

function HistoricoDoPaciente({
  idTexto,
  servico = registosClinicosService,
}: HistoricoDoPacienteProps) {
  const idCampos = useId();
  const { temPerfil } = useAuth();
  const animalId = Number(idTexto);
  const idInvalido = !Number.isInteger(animalId) || animalId <= 0;

  // Campos do filtro (o que está escrito) e filtro aplicado (o que foi pedido).
  const [inicio, setInicio] = useState("");
  const [fim, setFim] = useState("");
  const [erroPeriodo, setErroPeriodo] = useState<string | null>(null);
  const [filtros, setFiltros] = useState<HistoricoClinicoFiltros>({});
  const [tentativa, setTentativa] = useState(0);
  const [resultado, setResultado] = useState<ResultadoHistorico | null>(null);

  const [selecionado, setSelecionado] = useState<number | null>(null);
  const [tentativaRegisto, setTentativaRegisto] = useState(0);
  const [registo, setRegisto] = useState<ResultadoRegisto | null>(null);

  const chave = JSON.stringify([animalId, filtros, tentativa]);
  const chaveRegisto = JSON.stringify([selecionado, tentativaRegisto]);

  useEffect(() => {
    if (idInvalido) return;
    let cancelado = false;
    const [, pedido] = JSON.parse(chave) as [number, HistoricoClinicoFiltros];
    servico
      .obterHistorico(animalId, pedido)
      .then((dados) => {
        if (!cancelado) setResultado({ chave, dados, erro: null });
      })
      .catch((erro: unknown) => {
        if (!cancelado) setResultado({ chave, dados: null, erro });
      });
    return () => {
      cancelado = true;
    };
  }, [servico, animalId, idInvalido, chave]);

  useEffect(() => {
    if (selecionado === null) return;
    let cancelado = false;
    servico
      .obter(selecionado)
      .then((dados) => {
        if (!cancelado) {
          setRegisto({ chave: chaveRegisto, dados, erro: null });
        }
      })
      .catch((erro: unknown) => {
        if (!cancelado) {
          setRegisto({ chave: chaveRegisto, dados: null, erro });
        }
      });
    return () => {
      cancelado = true;
    };
  }, [servico, selecionado, chaveRegisto]);

  function filtrar(evento: FormEvent) {
    evento.preventDefault();
    const erro = validarPeriodo(inicio, fim);
    setErroPeriodo(erro);
    if (erro) return;
    setSelecionado(null);
    setFiltros({
      dataInicio: inicio || undefined,
      dataFim: fim || undefined,
    });
  }

  function limparFiltro() {
    setInicio("");
    setFim("");
    setErroPeriodo(null);
    setSelecionado(null);
    setFiltros({});
  }

  const titulo = "Histórico clínico";
  const voltar = (
    <Link to={`/pacientes/${animalId}`} className="botao secundario">
      Voltar à ficha do paciente
    </Link>
  );

  if (idInvalido) {
    return (
      <section>
        <h1>{titulo}</h1>
        <div role="alert" className="form-erro">
          O registo pedido não foi encontrado.
        </div>
      </section>
    );
  }

  const aCarregar = resultado?.chave !== chave;
  const historico = aCarregar ? null : (resultado?.dados ?? null);
  const erroHistorico = aCarregar ? null : (resultado?.erro ?? null);
  const comFiltro = Boolean(filtros.dataInicio || filtros.dataFim);

  const colunas: ColunaTabela<RegistoClinicoResumo>[] = [
    { cabecalho: "Data da consulta", celula: (r) => formatarData(r.data) },
    { cabecalho: "Veterinário", celula: (r) => r.veterinario },
    ...(temPerfil(["Veterinario"])
      ? [
          {
            cabecalho: "Diagnóstico",
            celula: (r: RegistoClinicoResumo) => r.diagnostico ?? "—",
          },
        ]
      : []),
    { cabecalho: "Tratamentos", celula: (r) => r.numeroTratamentos },
    { cabecalho: "Prescrições", celula: (r) => r.numeroPrescricoes },
    {
      cabecalho: "Ações",
      celula: (r) => (
        <button
          type="button"
          className="ligacao"
          aria-pressed={selecionado === r.id}
          aria-label={`Ver registo de ${formatarData(r.data)}`}
          onClick={() => {
            setSelecionado(r.id);
            setTentativaRegisto((n) => n + 1);
          }}
        >
          Ver registo
        </button>
      ),
    },
  ];

  const linhaSelecionada =
    historico?.registos.find((r) => r.id === selecionado) ?? null;

  return (
    <section>
      <h1>{titulo}</h1>

      {historico ? (
        <p className="detalhe-conta">
          Paciente: <strong>{historico.animal.nome}</strong>
          {historico.animal.especie ? ` · ${historico.animal.especie}` : ""}
        </p>
      ) : null}

      <SoPara
        acao="registarClinico"
        alternativa={
          <p className="detalhe-conta">
            Vista só de leitura: pode consultar a informação clínica autorizada,
            mas não criar nem alterar registos.
          </p>
        }
      >
        <p className="detalhe-conta">
          Para criar o registo de uma consulta, abra a consulta na Agenda.
        </p>
      </SoPara>

      <form
        className="filtros"
        onSubmit={filtrar}
        aria-label="Filtrar por período"
        noValidate
      >
        <FormField
          id={`${idCampos}-inicio`}
          rotulo="Desde"
          type="date"
          value={inicio}
          onChange={(e) => setInicio(e.target.value)}
        />
        <FormField
          id={`${idCampos}-fim`}
          rotulo="Até"
          type="date"
          value={fim}
          onChange={(e) => setFim(e.target.value)}
          erro={erroPeriodo}
        />
        <div className="acoes">
          <button type="submit">Filtrar</button>
          {comFiltro || inicio || fim ? (
            <button type="button" className="secundario" onClick={limparFiltro}>
              Limpar filtro
            </button>
          ) : null}
        </div>
      </form>

      <DataTable
        colunas={colunas}
        linhas={historico?.registos ?? []}
        chaveLinha={(r) => r.id}
        aCarregar={aCarregar}
        erro={erroHistorico}
        mensagemVazia={
          comFiltro
            ? "Não há registos clínicos deste paciente no período escolhido."
            : "Este paciente ainda não tem registos clínicos."
        }
      />
      {erroHistorico ? (
        <div className="acoes">
          <button
            type="button"
            className="secundario"
            onClick={() => setTentativa((n) => n + 1)}
          >
            Tentar novamente
          </button>
        </div>
      ) : null}

      {selecionado !== null ? (
        <DetalheRegisto
          resultado={registo?.chave === chaveRegisto ? registo : null}
          dataConsulta={linhaSelecionada?.data ?? null}
          aoTentarNovamente={() => setTentativaRegisto((n) => n + 1)}
          aoFechar={() => setSelecionado(null)}
        />
      ) : null}

      <div className="acoes">{voltar}</div>
    </section>
  );
}

interface DetalheRegistoProps {
  /** null enquanto carrega. */
  resultado: ResultadoRegisto | null;
  dataConsulta: string | null;
  aoTentarNovamente: () => void;
  aoFechar: () => void;
}

/** Registo clínico completo (EP-033), só de leitura. */
function DetalheRegisto({
  resultado,
  dataConsulta,
  aoTentarNovamente,
  aoFechar,
}: DetalheRegistoProps) {
  const idTitulo = useId();
  const titulo = dataConsulta
    ? `Registo de ${formatarData(dataConsulta)}`
    : "Registo clínico";

  let conteudo: ReactNode;
  if (resultado === null) {
    conteudo = <p role="status">A carregar…</p>;
  } else if (resultado.dados === null) {
    conteudo = (
      <>
        <FormErro erro={resultado.erro} />
        <div className="acoes">
          <button
            type="button"
            className="secundario"
            onClick={aoTentarNovamente}
          >
            Tentar novamente
          </button>
        </div>
      </>
    );
  } else {
    const r = resultado.dados;
    conteudo = (
      <>
        <p className="detalhe-conta">
          Registado por <strong>{r.veterinario}</strong> em{" "}
          {formatarDataHoraLisboa(r.criadoEm)}
          {r.atualizadoEm
            ? ` · Última alteração: ${formatarDataHoraLisboa(r.atualizadoEm)}${
                r.atualizadoPor ? ` por ${r.atualizadoPor}` : ""
              }`
            : ""}
        </p>
        <dl
          className="formulario formulario-largo"
          aria-label="Registo clínico"
        >
          <SoPara acao="registarClinico">
            <dt>Notas clínicas</dt>
            <dd className="texto-clinico">{r.notasClinicas ?? "—"}</dd>
            <dt>Diagnóstico</dt>
            <dd className="texto-clinico">{r.diagnostico ?? "—"}</dd>
          </SoPara>
          <dt>Tratamentos</dt>
          <dd>
            {r.tratamentos.length === 0 ? (
              "Sem tratamentos."
            ) : (
              <ul aria-label="Tratamentos">
                {r.tratamentos.map((t) => (
                  <li key={t.id}>
                    <span className="texto-clinico">{t.descricao}</span> —
                    aplicado em {formatarData(t.dataAplicacao)}
                  </li>
                ))}
              </ul>
            )}
          </dd>
          <dt>Prescrições</dt>
          <dd>
            {r.prescricoes.length === 0 ? (
              "Sem prescrições."
            ) : (
              <ul aria-label="Prescrições">
                {r.prescricoes.map((p) => (
                  <li key={p.id}>
                    <strong>{p.medicamento}</strong> — {p.dosagem},{" "}
                    {p.frequencia}
                    {p.duracaoDias !== null
                      ? `, durante ${p.duracaoDias} ${
                          p.duracaoDias === 1 ? "dia" : "dias"
                        }`
                      : ""}
                  </li>
                ))}
              </ul>
            )}
          </dd>
        </dl>
        <SoPara acao="registarClinico">
          <div className="acoes">
            <Link
              to={`/agenda/${r.consultaId}/registo-clinico`}
              className="botao"
            >
              Alterar registo
            </Link>
          </div>
        </SoPara>
      </>
    );
  }

  return (
    <section aria-labelledby={idTitulo} className="detalhe-registo">
      <h2 id={idTitulo}>{titulo}</h2>
      {conteudo}
      <div className="acoes">
        <button type="button" className="secundario" onClick={aoFechar}>
          Fechar registo
        </button>
      </div>
    </section>
  );
}

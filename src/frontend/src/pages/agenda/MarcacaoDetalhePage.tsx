import { useEffect, useId, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import { FormErro } from "../../components/FormErro";
import { SoPara } from "../../components/SoPara";
import { ApiRequestError } from "../../core/api";
import { formatarDataHoraLisboa, utcParaLisboa } from "../../core/horaLisboa";
import {
  TRANSICOES_ESTADO,
  marcacaoAlteravel,
  type EstadoMarcacaoAtualizavel,
  type MarcacaoDetalhe,
} from "../../models/agenda";
import {
  agendaService,
  type ServicoAgenda,
} from "../../services/AgendaService";

interface MarcacaoDetalhePageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoAgenda;
}

/** Texto do botão de cada transição (RF023). */
const ROTULO_TRANSICAO: Record<EstadoMarcacaoAtualizavel, string> = {
  Confirmada: "Confirmar marcação",
  Realizada: "Marcar como realizada",
  Cancelada: "Cancelar marcação",
};

/**
 * W11 — Detalhe da marcação (EP-024, EP-027 e EP-028). Rececionista e
 * Veterinário. Só a Rececionista vê as ações: editar, registar a chegada e
 * mudar o estado, apenas pelas transições permitidas, com confirmação.
 *
 * A chave obriga a montar a página de novo quando o :id muda.
 */
export function MarcacaoDetalhePage(props: MarcacaoDetalhePageProps) {
  const { id } = useParams();
  return <DetalheMarcacao key={id} idTexto={id} {...props} />;
}

interface DetalheMarcacaoProps extends MarcacaoDetalhePageProps {
  idTexto: string | undefined;
}

function DetalheMarcacao({
  idTexto,
  servico = agendaService,
}: DetalheMarcacaoProps) {
  const localizacao = useLocation();
  const idCampos = useId();
  const id = Number(idTexto);
  const idInvalido = !Number.isInteger(id) || id <= 0;

  const [marcacao, setMarcacao] = useState<MarcacaoDetalhe | null>(null);
  const [erroCarregar, setErroCarregar] = useState<unknown>(null);
  const [aviso, setAviso] = useState<string | null>(
    () => (localizacao.state as { aviso?: string } | null)?.aviso ?? null,
  );
  const [avisoConflito, setAvisoConflito] = useState<string | null>(null);
  const [transicao, setTransicao] = useState<EstadoMarcacaoAtualizavel | null>(
    null,
  );
  const [observacao, setObservacao] = useState("");
  const [erro, setErro] = useState<unknown>(null);
  const [aGuardar, setAGuardar] = useState(false);

  useEffect(() => {
    if (idInvalido) return;
    let cancelado = false;
    servico
      .obterConsulta(id)
      .then((dados) => {
        if (!cancelado) setMarcacao(dados);
      })
      .catch((e: unknown) => {
        if (!cancelado) setErroCarregar(e);
      });
    return () => {
      cancelado = true;
    };
  }, [servico, id, idInvalido]);

  function escolherTransicao(estado: EstadoMarcacaoAtualizavel) {
    setTransicao(estado);
    setObservacao("");
    setErro(null);
    setAviso(null);
    setAvisoConflito(null);
  }

  async function confirmarTransicao() {
    if (transicao === null) return;
    setAGuardar(true);
    setErro(null);
    try {
      await servico.alterarEstado(id, {
        estado: transicao,
        observacao,
      });
      setMarcacao(await servico.obterConsulta(id));
      setAviso(`Estado alterado para ${transicao}.`);
      setTransicao(null);
    } catch (e) {
      await tratarRecusa(e);
    } finally {
      setAGuardar(false);
    }
  }

  /** EP-028 — a chegada não muda o estado da marcação (critério 1 da I114). */
  async function registarChegada() {
    setAGuardar(true);
    setErro(null);
    setAviso(null);
    setAvisoConflito(null);
    setTransicao(null);
    try {
      const chegada = await servico.registarChegada(id);
      setMarcacao((atual) =>
        atual ? { ...atual, chegadaEm: chegada.chegadaEm } : atual,
      );
      setAviso(
        `Chegada registada às ${utcParaLisboa(chegada.chegadaEm).hora}. O estado da marcação não muda.`,
      );
    } catch (e) {
      await tratarRecusa(e);
    } finally {
      setAGuardar(false);
    }
  }

  /**
   * A marcação mudou entretanto (412) ou a operação já não é válida (422):
   * mostra-se o erro e o estado atual, e o utilizador decide (§9.2).
   */
  async function tratarRecusa(e: unknown) {
    setErro(e);
    if (
      !(e instanceof ApiRequestError) ||
      (e.categoria !== "versaoDesatualizada" && e.categoria !== "regraNegocio")
    ) {
      return;
    }
    try {
      setMarcacao(await servico.obterConsulta(id));
      setTransicao(null);
      if (e.categoria === "versaoDesatualizada") {
        setAvisoConflito(
          "A marcação foi alterada por outra pessoa. Os dados abaixo já estão atualizados: confirme se a operação ainda faz sentido e repita-a.",
        );
      }
    } catch {
      // O erro original já está à vista.
    }
  }

  const titulo = "Detalhe da marcação";

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

  if (marcacao === null) {
    return (
      <section>
        <h1>{titulo}</h1>
        {erroCarregar ? (
          <FormErro erro={erroCarregar} />
        ) : (
          <p role="status">A carregar…</p>
        )}
      </section>
    );
  }

  const transicoes = TRANSICOES_ESTADO[marcacao.estado] ?? [];

  return (
    <section>
      <h1>{titulo}</h1>
      {aviso ? (
        <p role="status" className="sucesso">
          {aviso}
        </p>
      ) : null}
      {avisoConflito ? (
        <p role="status" className="aviso">
          {avisoConflito}
        </p>
      ) : null}
      {erro && transicao === null ? <FormErro erro={erro} /> : null}

      <dl className="formulario" aria-label="Dados da marcação">
        <dt>Paciente</dt>
        <dd>
          {marcacao.animal.nome}
          {marcacao.animal.especie ? ` · ${marcacao.animal.especie}` : ""}{" "}
          <SoPara acao="consultarFichaPaciente">
            <Link to={`/pacientes/${marcacao.animal.id}`}>
              Abrir ficha do paciente
            </Link>
          </SoPara>
        </dd>
        <dt>Cliente</dt>
        <dd>{marcacao.cliente.nome}</dd>
        <dt>Veterinário</dt>
        <dd>{marcacao.veterinario.nome}</dd>
        <dt>Data e hora</dt>
        <dd>
          {formatarDataHoraLisboa(marcacao.dataHora)} ·{" "}
          {marcacao.duracaoMinutos} minutos
        </dd>
        <dt>Motivo</dt>
        <dd>{marcacao.motivo ?? "—"}</dd>
        <dt>Estado</dt>
        <dd>
          <strong>{marcacao.estado}</strong>
        </dd>
        <dt>Observação do estado</dt>
        <dd>{marcacao.observacao ?? "—"}</dd>
        <dt>Chegada</dt>
        <dd>
          {marcacao.chegadaEm
            ? formatarDataHoraLisboa(marcacao.chegadaEm)
            : "Não registada"}
        </dd>
      </dl>

      <SoPara acao="gerirMarcacoes">
        <section aria-labelledby={`${idCampos}-acoes`}>
          <h2 id={`${idCampos}-acoes`}>Ações</h2>
          {transicoes.length === 0 ? (
            <p className="detalhe-conta">
              {marcacao.estado} é um estado final: a marcação já não pode ser
              alterada.
            </p>
          ) : (
            <div className="acoes">
              {marcacaoAlteravel(marcacao.estado) ? (
                <Link
                  to={`/agenda/${marcacao.id}/editar`}
                  className="botao secundario"
                >
                  Editar marcação
                </Link>
              ) : null}
              {marcacao.estado === "Confirmada" && !marcacao.chegadaEm ? (
                <button
                  type="button"
                  disabled={aGuardar}
                  onClick={() => void registarChegada()}
                >
                  Registar chegada
                </button>
              ) : null}
              {transicoes.map((estado) => (
                <button
                  key={estado}
                  type="button"
                  className={estado === "Cancelada" ? "perigo" : undefined}
                  disabled={aGuardar}
                  onClick={() => escolherTransicao(estado)}
                >
                  {ROTULO_TRANSICAO[estado]}
                </button>
              ))}
            </div>
          )}

          {transicao ? (
            <div className="confirmacao" role="group" aria-label="Confirmação">
              <p>
                Mudar o estado de <strong>{marcacao.estado}</strong> para{" "}
                <strong>{transicao}</strong>?
              </p>
              <FormErro erro={erro} />
              <div className="campo">
                <label htmlFor={`${idCampos}-obs`}>
                  {transicao === "Cancelada"
                    ? "Motivo do cancelamento (opcional)"
                    : "Observação (opcional)"}
                </label>
                <input
                  id={`${idCampos}-obs`}
                  maxLength={250}
                  value={observacao}
                  onChange={(e) => setObservacao(e.target.value)}
                />
              </div>
              <div className="acoes">
                <button
                  type="button"
                  className="secundario"
                  disabled={aGuardar}
                  onClick={() => {
                    setTransicao(null);
                    setErro(null);
                  }}
                >
                  Voltar
                </button>
                <button
                  type="button"
                  className={transicao === "Cancelada" ? "perigo" : undefined}
                  disabled={aGuardar}
                  onClick={() => void confirmarTransicao()}
                >
                  {aGuardar ? "A guardar…" : "Confirmar"}
                </button>
              </div>
            </div>
          ) : null}
        </section>
      </SoPara>

      {marcacao.notasPreparacao.length > 0 ? (
        <section aria-labelledby={`${idCampos}-notas`}>
          <h2 id={`${idCampos}-notas`}>Notas de preparação</h2>
          <ul>
            {marcacao.notasPreparacao.map((nota) => (
              <li key={nota.id}>
                {nota.texto} — {nota.autor},{" "}
                {formatarDataHoraLisboa(nota.criadaEm)}
              </li>
            ))}
          </ul>
        </section>
      ) : null}
    </section>
  );
}

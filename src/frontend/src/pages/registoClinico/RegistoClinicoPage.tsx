import {
  useCallback,
  useEffect,
  useId,
  useState,
  type FormEvent,
  type ReactNode,
} from "react";
import { Link, useParams } from "react-router-dom";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import { ApiRequestError } from "../../core/api";
import { formatarDataHoraLisboa, utcParaLisboa } from "../../core/horaLisboa";
import { useAvisoAlteracoes } from "../../core/useAvisoAlteracoes";
import type { MarcacaoDetalhe } from "../../models/agenda";
import {
  ESTADOS_COM_REGISTO_CLINICO,
  type PrescricaoForm,
  type RegistoClinico,
  type RegistoClinicoForm,
  type TratamentoForm,
} from "../../models/registosClinicos";
import {
  agendaService,
  type ServicoAgenda,
} from "../../services/AgendaService";
import {
  registosClinicosService,
  type ServicoRegistosClinicos,
} from "../../services/RegistosClinicosService";
import {
  MAX_DESCRICAO_TRATAMENTO,
  MAX_DIAGNOSTICO,
  MAX_DOSAGEM,
  MAX_DURACAO_DIAS,
  MAX_FREQUENCIA,
  MAX_MEDICAMENTO,
  MAX_NOTAS,
  assinaturaFormulario,
  formularioDeRegisto,
  formularioVazio,
  pedidoDoFormulario,
  prescricaoVazia,
  temErros,
  tratamentoVazio,
  validarRegistoClinico,
  type ErrosRegistoClinico,
} from "./validacaoRegistoClinico";

interface RegistoClinicoPageProps {
  /** Só para testes: permitem trocar os serviços. */
  servico?: ServicoRegistosClinicos;
  servicoAgenda?: Pick<ServicoAgenda, "obterConsulta">;
}

const SEM_ERROS: ErrosRegistoClinico = { tratamentos: {}, prescricoes: {} };

function estadoAdmiteRegisto(estado: string): boolean {
  return (ESTADOS_COM_REGISTO_CLINICO as readonly string[]).includes(estado);
}

/**
 * W13 — Registo clínico da consulta (RF030–RF034; EP-031 a EP-033). Só o
 * Veterinário: a rota está protegida por perfil e a API recusa (403) os
 * outros perfis e o Veterinário que não é o da consulta.
 *
 * Se a consulta ainda não tem registo, o formulário cria-o (EP-031); se já
 * tem, carrega-o e altera-o (EP-032), mantendo a consulta e o paciente.
 *
 * A chave obriga a montar a página de novo quando o :id muda.
 */
export function RegistoClinicoPage(props: RegistoClinicoPageProps) {
  const { id } = useParams();
  return <RegistoDaConsulta key={id} idTexto={id} {...props} />;
}

interface RegistoDaConsultaProps extends RegistoClinicoPageProps {
  idTexto: string | undefined;
}

function RegistoDaConsulta({
  idTexto,
  servico = registosClinicosService,
  servicoAgenda = agendaService,
}: RegistoDaConsultaProps) {
  const idCampos = useId();
  const consultaId = Number(idTexto);
  const idInvalido = !Number.isInteger(consultaId) || consultaId <= 0;

  const [consulta, setConsulta] = useState<MarcacaoDetalhe | null>(null);
  const [registo, setRegisto] = useState<RegistoClinico | null>(null);
  const [carregado, setCarregado] = useState(false);
  const [erroCarregar, setErroCarregar] = useState<unknown>(null);

  const [formulario, setFormulario] =
    useState<RegistoClinicoForm>(formularioVazio);
  const [assinaturaGuardada, setAssinaturaGuardada] = useState(() =>
    assinaturaFormulario(formularioVazio()),
  );
  const [errosCampos, setErrosCampos] =
    useState<ErrosRegistoClinico>(SEM_ERROS);
  const [tentouGuardar, setTentouGuardar] = useState(false);
  const [erro, setErro] = useState<unknown>(null);
  const [aviso, setAviso] = useState<string | null>(null);
  const [aGuardar, setAGuardar] = useState(false);

  const comAlteracoes =
    carregado && assinaturaFormulario(formulario) !== assinaturaGuardada;
  useAvisoAlteracoes(comAlteracoes);

  /** Mostra o registo vindo da API e marca-o como "tudo guardado". */
  const aplicarRegisto = useCallback((dados: RegistoClinico | null) => {
    const form = dados ? formularioDeRegisto(dados) : formularioVazio();
    setRegisto(dados);
    setFormulario(form);
    setAssinaturaGuardada(assinaturaFormulario(form));
    setErrosCampos(SEM_ERROS);
    setTentouGuardar(false);
  }, []);

  useEffect(() => {
    if (idInvalido) return;
    let cancelado = false;
    (async () => {
      try {
        const dadosConsulta = await servicoAgenda.obterConsulta(consultaId);
        if (cancelado) return;
        setConsulta(dadosConsulta);
        if (estadoAdmiteRegisto(dadosConsulta.estado)) {
          const existente = await servico.obterDaConsulta(
            dadosConsulta.animal.id,
            consultaId,
          );
          if (cancelado) return;
          aplicarRegisto(existente);
        }
        setCarregado(true);
      } catch (e) {
        if (!cancelado) setErroCarregar(e);
      }
    })();
    return () => {
      cancelado = true;
    };
  }, [servico, servicoAgenda, consultaId, idInvalido, aplicarRegisto]);

  function alterarFormulario(novo: RegistoClinicoForm) {
    setFormulario(novo);
    setAviso(null);
    // Depois da primeira tentativa, os erros acompanham o que se escreve.
    if (tentouGuardar) setErrosCampos(validarRegistoClinico(novo));
  }

  function alterarTratamento(chave: string, campos: Partial<TratamentoForm>) {
    alterarFormulario({
      ...formulario,
      tratamentos: formulario.tratamentos.map((t) =>
        t.chave === chave ? { ...t, ...campos } : t,
      ),
    });
  }

  function alterarPrescricao(chave: string, campos: Partial<PrescricaoForm>) {
    alterarFormulario({
      ...formulario,
      prescricoes: formulario.prescricoes.map((p) =>
        p.chave === chave ? { ...p, ...campos } : p,
      ),
    });
  }

  async function guardar(evento: FormEvent) {
    evento.preventDefault();
    setErro(null);
    setAviso(null);
    setTentouGuardar(true);
    const erros = validarRegistoClinico(formulario);
    setErrosCampos(erros);
    if (temErros(erros)) return;

    setAGuardar(true);
    try {
      const pedido = pedidoDoFormulario(formulario);
      const guardado = registo
        ? await servico.atualizar(registo.id, pedido)
        : await servico.criar({ consultaId, ...pedido });
      aplicarRegisto(guardado);
      setAviso(
        registo
          ? "Registo clínico atualizado."
          : "Registo clínico criado para esta consulta.",
      );
    } catch (e) {
      setErro(e);
      // 409: outra sessão criou o registo entretanto. Mostra-se o que está
      // guardado; as alterações deste ecrã ficam por aplicar.
      if (
        !registo &&
        consulta &&
        e instanceof ApiRequestError &&
        e.categoria === "conflito"
      ) {
        try {
          const existente = await servico.obterDaConsulta(
            consulta.animal.id,
            consultaId,
          );
          if (existente) {
            aplicarRegisto(existente);
            setAviso(
              "Esta consulta já tinha registo clínico. Os dados abaixo são os que estão guardados: volte a fazer as suas alterações e guarde.",
            );
          }
        } catch {
          // O erro original já está à vista.
        }
      }
    } finally {
      setAGuardar(false);
    }
  }

  const titulo = "Registo clínico da consulta";
  const voltar = (
    <Link to={`/agenda/${consultaId}`} className="botao secundario">
      Voltar à marcação
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

  if (consulta === null || (!carregado && !erroCarregar)) {
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

  const dadosConsulta = (
    <dl className="formulario" aria-label="Dados da consulta">
      <dt>Paciente</dt>
      <dd>
        {consulta.animal.nome}
        {consulta.animal.especie ? ` · ${consulta.animal.especie}` : ""}
      </dd>
      <dt>Cliente</dt>
      <dd>{consulta.cliente.nome}</dd>
      <dt>Veterinário</dt>
      <dd>{consulta.veterinario.nome}</dd>
      <dt>Data e hora</dt>
      <dd>{formatarDataHoraLisboa(consulta.dataHora)}</dd>
      <dt>Estado</dt>
      <dd>
        <strong>{consulta.estado}</strong>
      </dd>
    </dl>
  );

  if (!estadoAdmiteRegisto(consulta.estado)) {
    return (
      <section>
        <h1>{titulo}</h1>
        {dadosConsulta}
        <p role="status" className="aviso">
          Só é possível fazer o registo clínico de uma consulta Confirmada ou
          Realizada. Esta consulta está {consulta.estado}.
        </p>
        <div className="acoes">{voltar}</div>
      </section>
    );
  }

  if (erroCarregar) {
    return (
      <section>
        <h1>{titulo}</h1>
        {dadosConsulta}
        <FormErro erro={erroCarregar} />
        <div className="acoes">{voltar}</div>
      </section>
    );
  }

  const dataConsulta = utcParaLisboa(consulta.dataHora).data;

  return (
    <section>
      <h1>{titulo}</h1>
      {aviso ? (
        <p role="status" className="sucesso">
          {aviso}
        </p>
      ) : null}
      {dadosConsulta}

      {registo ? (
        <p className="detalhe-conta">
          Registado por <strong>{registo.veterinario}</strong> em{" "}
          {formatarDataHoraLisboa(registo.criadoEm)}
          {registo.atualizadoEm
            ? ` · Última alteração: ${formatarDataHoraLisboa(registo.atualizadoEm)}${
                registo.atualizadoPor ? ` por ${registo.atualizadoPor}` : ""
              }`
            : ""}
        </p>
      ) : (
        <p className="detalhe-conta">
          Esta consulta ainda não tem registo clínico. Preencha pelo menos um
          elemento: notas, diagnóstico, um tratamento ou uma prescrição.
        </p>
      )}

      <form
        className="formulario formulario-largo"
        onSubmit={guardar}
        aria-label={titulo}
        noValidate
      >
        <FormErro erro={erro} />
        {errosCampos.geral ? (
          <div role="alert" className="form-erro">
            {errosCampos.geral}
          </div>
        ) : null}

        <AreaTexto
          id={`${idCampos}-notas`}
          rotulo="Notas clínicas"
          maximo={MAX_NOTAS}
          valor={formulario.notasClinicas}
          erro={errosCampos.notasClinicas}
          aoAlterar={(v) =>
            alterarFormulario({ ...formulario, notasClinicas: v })
          }
          linhas={6}
        />
        <AreaTexto
          id={`${idCampos}-diagnostico`}
          rotulo="Diagnóstico"
          maximo={MAX_DIAGNOSTICO}
          valor={formulario.diagnostico}
          erro={errosCampos.diagnostico}
          aoAlterar={(v) =>
            alterarFormulario({ ...formulario, diagnostico: v })
          }
          linhas={3}
        />

        <Grupo titulo="Tratamentos">
          {formulario.tratamentos.length === 0 ? (
            <p className="detalhe-conta">Sem tratamentos.</p>
          ) : null}
          {formulario.tratamentos.map((t, i) => {
            const n = i + 1;
            const e = errosCampos.tratamentos[t.chave] ?? {};
            return (
              <fieldset key={t.chave} className="elemento-clinico">
                <legend>Tratamento {n}</legend>
                <AreaTexto
                  id={`${idCampos}-t-${t.chave}`}
                  rotulo={`Descrição do tratamento ${n}`}
                  maximo={MAX_DESCRICAO_TRATAMENTO}
                  valor={t.descricao}
                  erro={e.descricao}
                  aoAlterar={(v) =>
                    alterarTratamento(t.chave, { descricao: v })
                  }
                  linhas={2}
                />
                <FormField
                  rotulo={`Data de aplicação do tratamento ${n}`}
                  type="date"
                  required
                  value={t.dataAplicacao}
                  onChange={(ev) =>
                    alterarTratamento(t.chave, {
                      dataAplicacao: ev.target.value,
                    })
                  }
                  erro={e.dataAplicacao}
                />
                <button
                  type="button"
                  className="ligacao perigo-texto"
                  onClick={() =>
                    alterarFormulario({
                      ...formulario,
                      tratamentos: formulario.tratamentos.filter(
                        (x) => x.chave !== t.chave,
                      ),
                    })
                  }
                >
                  Remover tratamento {n}
                </button>
              </fieldset>
            );
          })}
          <button
            type="button"
            className="secundario"
            onClick={() =>
              alterarFormulario({
                ...formulario,
                tratamentos: [
                  ...formulario.tratamentos,
                  tratamentoVazio(dataConsulta),
                ],
              })
            }
          >
            Adicionar tratamento
          </button>
        </Grupo>

        <Grupo titulo="Prescrições">
          {formulario.prescricoes.length === 0 ? (
            <p className="detalhe-conta">Sem prescrições.</p>
          ) : null}
          {formulario.prescricoes.map((p, i) => {
            const n = i + 1;
            const e = errosCampos.prescricoes[p.chave] ?? {};
            return (
              <fieldset key={p.chave} className="elemento-clinico">
                <legend>Prescrição {n}</legend>
                <FormField
                  rotulo={`Medicamento da prescrição ${n}`}
                  required
                  maxLength={MAX_MEDICAMENTO}
                  value={p.medicamento}
                  onChange={(ev) =>
                    alterarPrescricao(p.chave, { medicamento: ev.target.value })
                  }
                  erro={e.medicamento}
                />
                <FormField
                  rotulo={`Dosagem da prescrição ${n}`}
                  required
                  maxLength={MAX_DOSAGEM}
                  value={p.dosagem}
                  onChange={(ev) =>
                    alterarPrescricao(p.chave, { dosagem: ev.target.value })
                  }
                  erro={e.dosagem}
                />
                <FormField
                  rotulo={`Frequência da prescrição ${n}`}
                  required
                  maxLength={MAX_FREQUENCIA}
                  value={p.frequencia}
                  onChange={(ev) =>
                    alterarPrescricao(p.chave, { frequencia: ev.target.value })
                  }
                  erro={e.frequencia}
                />
                <FormField
                  rotulo={`Duração em dias da prescrição ${n} (opcional)`}
                  type="number"
                  inputMode="numeric"
                  min={1}
                  max={MAX_DURACAO_DIAS}
                  step={1}
                  value={p.duracaoDias}
                  onChange={(ev) =>
                    alterarPrescricao(p.chave, { duracaoDias: ev.target.value })
                  }
                  erro={e.duracaoDias}
                />
                <button
                  type="button"
                  className="ligacao perigo-texto"
                  onClick={() =>
                    alterarFormulario({
                      ...formulario,
                      prescricoes: formulario.prescricoes.filter(
                        (x) => x.chave !== p.chave,
                      ),
                    })
                  }
                >
                  Remover prescrição {n}
                </button>
              </fieldset>
            );
          })}
          <button
            type="button"
            className="secundario"
            onClick={() =>
              alterarFormulario({
                ...formulario,
                prescricoes: [...formulario.prescricoes, prescricaoVazia()],
              })
            }
          >
            Adicionar prescrição
          </button>
        </Grupo>

        {registo ? (
          <p className="detalhe-conta">
            Os tratamentos e as prescrições que remover são apagados do registo
            quando guardar.
          </p>
        ) : null}
        {comAlteracoes ? (
          <p role="status" className="aviso">
            Tem alterações por guardar.
          </p>
        ) : null}

        <div className="acoes">
          {voltar}
          <button type="submit" disabled={aGuardar}>
            {aGuardar
              ? "A guardar…"
              : registo
                ? "Guardar alterações"
                : "Criar registo clínico"}
          </button>
        </div>
      </form>
    </section>
  );
}

interface AreaTextoProps {
  id: string;
  rotulo: string;
  maximo: number;
  valor: string;
  erro?: string;
  aoAlterar: (valor: string) => void;
  linhas: number;
}

/** Campo de texto longo com etiqueta, contador e erro ligados (como FormField). */
function AreaTexto({
  id,
  rotulo,
  maximo,
  valor,
  erro,
  aoAlterar,
  linhas,
}: AreaTextoProps) {
  const idErro = `${id}-erro`;
  return (
    <div className="campo">
      <label htmlFor={id}>{rotulo}</label>
      <textarea
        id={id}
        rows={linhas}
        maxLength={maximo}
        value={valor}
        aria-invalid={erro ? true : undefined}
        aria-describedby={erro ? idErro : undefined}
        onChange={(e) => aoAlterar(e.target.value)}
      />
      <small className="contador">
        {valor.length}/{maximo}
      </small>
      {erro ? (
        <p id={idErro} className="campo-erro">
          {erro}
        </p>
      ) : null}
    </div>
  );
}

function Grupo({ titulo, children }: { titulo: string; children: ReactNode }) {
  return (
    <fieldset className="grupo">
      <legend>{titulo}</legend>
      {children}
    </fieldset>
  );
}

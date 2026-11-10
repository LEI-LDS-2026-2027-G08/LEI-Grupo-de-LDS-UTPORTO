import { useEffect, useId, useState, type FormEvent } from "react";
import { Link, useLocation, useNavigate, useParams } from "react-router-dom";
import { FormErro } from "../../components/FormErro";
import { ApiRequestError } from "../../core/api";
import {
  formatarDiaCurto,
  hojeEmLisboa,
  lisboaParaUtc,
  somarDias,
  utcParaLisboa,
} from "../../core/horaLisboa";
import {
  marcacaoAlteravel,
  type Disponibilidade,
  type MarcacaoDetalhe,
  type VeterinarioResumo,
} from "../../models/agenda";
import type { ServicoPacientes } from "../../services/PacientesService";
import {
  agendaService,
  type ServicoAgenda,
} from "../../services/AgendaService";
import { SeletorPaciente, type PacienteEscolhido } from "./SeletorPaciente";

/** Dias mostrados de cada vez na disponibilidade (EP-022 aceita até 31). */
const DIAS_DISPONIBILIDADE = 7;

interface MarcacaoFormPageProps {
  /** Só para testes: permitem trocar os serviços. */
  servico?: ServicoAgenda;
  servicoPacientes?: Pick<ServicoPacientes, "listar">;
}

interface Horario {
  /** Dia em Lisboa (aaaa-mm-dd). */
  data: string;
  /** Hora de Lisboa (HH:mm). */
  hora: string;
}

interface ErrosMarcacao {
  paciente?: string;
  veterinario?: string;
  horario?: string;
  motivo?: string;
}

/**
 * W10 — Criar / editar marcação (EP-021, EP-022, EP-025 e EP-026). Só para a
 * Rececionista. O horário só pode ser escolhido entre os livres devolvidos
 * pela API; mudar de Veterinário ou de período obriga a escolher de novo.
 *
 * A chave obriga a montar a página de novo quando o :id muda.
 */
export function MarcacaoFormPage(props: MarcacaoFormPageProps) {
  const { id } = useParams();
  return <FormularioMarcacao key={id ?? "nova"} idTexto={id} {...props} />;
}

interface FormularioMarcacaoProps extends MarcacaoFormPageProps {
  idTexto: string | undefined;
}

function FormularioMarcacao({
  idTexto,
  servico = agendaService,
  servicoPacientes,
}: FormularioMarcacaoProps) {
  const navegar = useNavigate();
  const localizacao = useLocation();
  const idCampos = useId();
  const id = idTexto === undefined ? null : Number(idTexto);
  const aCriar = id === null;
  const idInvalido = id !== null && (!Number.isInteger(id) || id <= 0);

  const [paciente, setPaciente] = useState<PacienteEscolhido | null>(
    () =>
      (localizacao.state as { paciente?: PacienteEscolhido } | null)
        ?.paciente ?? null,
  );
  const [veterinarios, setVeterinarios] = useState<VeterinarioResumo[]>([]);
  const [erroVeterinarios, setErroVeterinarios] = useState<unknown>(null);
  const [veterinarioId, setVeterinarioId] = useState("");
  const [inicio, setInicio] = useState(() => hojeEmLisboa());
  const [disponibilidade, setDisponibilidade] =
    useState<Disponibilidade | null>(null);
  const [aConsultar, setAConsultar] = useState(false);
  const [erroDisponibilidade, setErroDisponibilidade] = useState<unknown>(null);
  const [horario, setHorario] = useState<Horario | null>(null);
  const [motivo, setMotivo] = useState("");
  const [marcacao, setMarcacao] = useState<MarcacaoDetalhe | null>(null);
  const [erroCarregar, setErroCarregar] = useState<unknown>(null);
  const [erro, setErro] = useState<unknown>(null);
  const [aviso, setAviso] = useState<string | null>(null);
  const [erros, setErros] = useState<ErrosMarcacao>({});
  const [aGuardar, setAGuardar] = useState(false);

  useEffect(() => {
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
  }, [servico]);

  // Volta a mostrar a marcação tal como está agora no servidor (depois de 412).
  const preencherCom = (dados: MarcacaoDetalhe) => {
    const atual = utcParaLisboa(dados.dataHora);
    setMarcacao(dados);
    setVeterinarioId(String(dados.veterinario.id));
    setInicio(atual.data);
    setHorario(atual);
    setMotivo(dados.motivo ?? "");
    setDisponibilidade(null);
  };

  useEffect(() => {
    if (id === null || idInvalido) return;
    let cancelado = false;
    servico
      .obterConsulta(id)
      .then((dados) => {
        if (cancelado) return;
        const atual = utcParaLisboa(dados.dataHora);
        setMarcacao(dados);
        setVeterinarioId(String(dados.veterinario.id));
        setInicio(atual.data);
        setHorario(atual);
        setMotivo(dados.motivo ?? "");
      })
      .catch((e: unknown) => {
        if (!cancelado) setErroCarregar(e);
      });
    return () => {
      cancelado = true;
    };
  }, [servico, id, idInvalido]);

  async function consultarDisponibilidade(vetId = veterinarioId) {
    setErroDisponibilidade(null);
    if (vetId === "") {
      setErros((atuais) => ({
        ...atuais,
        veterinario: "Escolha o Veterinário.",
      }));
      return;
    }
    setAConsultar(true);
    try {
      setDisponibilidade(
        await servico.obterDisponibilidade(
          Number(vetId),
          inicio,
          somarDias(inicio, DIAS_DISPONIBILIDADE - 1),
        ),
      );
    } catch (e) {
      setDisponibilidade(null);
      setErroDisponibilidade(e);
    } finally {
      setAConsultar(false);
    }
  }

  // Mudar de Veterinário ou de período invalida o horário escolhido (critério 3).
  function alterarVeterinario(valor: string) {
    setVeterinarioId(valor);
    setHorario(null);
    setDisponibilidade(null);
    setErros((atuais) => ({
      ...atuais,
      veterinario: undefined,
      horario: undefined,
    }));
  }

  // Mudar só o período mostrado não muda o horário já escolhido.
  function alterarInicio(valor: string) {
    setInicio(valor);
    setDisponibilidade(null);
  }

  function escolherHorario(novo: Horario) {
    setHorario(novo);
    setErros((atuais) => ({ ...atuais, horario: undefined }));
  }

  function validar(): ErrosMarcacao {
    const novos: ErrosMarcacao = {};
    if (aCriar && paciente === null) novos.paciente = "Escolha o paciente.";
    if (veterinarioId === "") novos.veterinario = "Escolha o Veterinário.";
    if (horario === null) {
      novos.horario =
        "Consulte a disponibilidade e escolha um dos horários livres.";
    }
    if (motivo.trim().length > 250) {
      novos.motivo = "O motivo não pode ter mais de 250 caracteres.";
    }
    return novos;
  }

  async function guardar(evento: FormEvent) {
    evento.preventDefault();
    setErro(null);
    setAviso(null);
    const novos = validar();
    setErros(novos);
    if (Object.keys(novos).length > 0 || horario === null) return;
    if (aCriar && paciente === null) return;

    const dados = {
      veterinarioId: Number(veterinarioId),
      dataHora: lisboaParaUtc(horario.data, horario.hora),
      motivo,
    };
    setAGuardar(true);
    try {
      const gravada =
        aCriar && paciente
          ? await servico.criar({ ...dados, animalId: paciente.id })
          : await servico.atualizar(id as number, dados);
      navegar(`/agenda/${gravada.id}`, {
        replace: true,
        state: {
          aviso: aCriar
            ? "Marcação criada no estado Pendente."
            : "Marcação alterada.",
        },
      });
    } catch (e) {
      setErro(e);
      await tratarFalha(e);
    } finally {
      setAGuardar(false);
    }
  }

  /** Depois de uma recusa da API, mostra os dados atuais (critério 5). */
  async function tratarFalha(e: unknown) {
    if (!(e instanceof ApiRequestError)) return;
    if (e.categoria === "versaoDesatualizada" && id !== null) {
      try {
        preencherCom(await servico.obterConsulta(id));
        setAviso(
          "A marcação foi alterada por outra pessoa. Os dados foram atualizados: reveja-os e guarde de novo, se ainda fizer sentido.",
        );
      } catch {
        // O erro original já está à vista.
      }
      return;
    }
    if (e.categoria === "regraNegocio" || e.categoria === "conflito") {
      setHorario(null);
      await consultarDisponibilidade();
      setAviso(
        "A disponibilidade foi atualizada. Escolha outro horário livre.",
      );
    }
  }

  const titulo = aCriar ? "Nova marcação" : "Editar marcação";

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

  if (!aCriar && marcacao === null) {
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

  if (marcacao && !marcacaoAlteravel(marcacao.estado)) {
    return (
      <section>
        <h1>{titulo}</h1>
        <p role="alert" className="form-erro">
          Esta marcação está {marcacao.estado} e já não pode ser alterada.
        </p>
        <Link to={`/agenda/${marcacao.id}`}>Voltar ao detalhe da marcação</Link>
      </section>
    );
  }

  const horarioAtual = marcacao ? utcParaLisboa(marcacao.dataHora) : null;
  const voltarPara = marcacao ? `/agenda/${marcacao.id}` : "/agenda";

  return (
    <section>
      <h1>{titulo}</h1>
      {aviso ? (
        <p role="status" className="aviso">
          {aviso}
        </p>
      ) : null}

      <form
        className="formulario"
        onSubmit={guardar}
        aria-label={titulo}
        noValidate
      >
        <FormErro erro={erro} />

        {aCriar ? (
          <SeletorPaciente
            selecionado={paciente}
            aoSelecionar={(p) => {
              setPaciente(p);
              setErros((atuais) => ({ ...atuais, paciente: undefined }));
            }}
            erro={erros.paciente}
            servico={servicoPacientes}
          />
        ) : (
          <p>
            Paciente: <strong>{marcacao?.animal.nome}</strong> ·{" "}
            {marcacao?.cliente.nome}
          </p>
        )}

        <div className="campo">
          <label htmlFor={`${idCampos}-vet`}>Veterinário</label>
          <select
            id={`${idCampos}-vet`}
            value={veterinarioId}
            aria-invalid={erros.veterinario ? true : undefined}
            onChange={(e) => alterarVeterinario(e.target.value)}
          >
            <option value="">Escolha…</option>
            {veterinarios.map((v) => (
              <option key={v.id} value={v.id}>
                {v.nome}
              </option>
            ))}
          </select>
          {erros.veterinario ? (
            <p className="campo-erro">{erros.veterinario}</p>
          ) : null}
          {erroVeterinarios ? <FormErro erro={erroVeterinarios} /> : null}
        </div>

        <fieldset>
          <legend>Disponibilidade</legend>
          <div className="filtros">
            <div className="campo">
              <label htmlFor={`${idCampos}-inicio`}>A partir de</label>
              <input
                id={`${idCampos}-inicio`}
                type="date"
                min={hojeEmLisboa()}
                value={inicio}
                onChange={(e) => alterarInicio(e.target.value)}
              />
            </div>
            <button
              type="button"
              className="secundario"
              disabled={aConsultar}
              onClick={() => void consultarDisponibilidade()}
            >
              {aConsultar ? "A consultar…" : "Consultar disponibilidade"}
            </button>
          </div>

          {erroDisponibilidade ? <FormErro erro={erroDisponibilidade} /> : null}

          {disponibilidade ? (
            <div aria-label="Horários livres" role="group">
              <p className="detalhe-conta">
                Duração de cada consulta:{" "}
                {disponibilidade.duracaoConsultaMinutos} minutos.
              </p>
              {disponibilidade.dias.map((dia) => (
                <div key={dia.data} className="campo">
                  <span>{formatarDiaCurto(dia.data)}</span>
                  {dia.horariosLivres.length === 0 ? (
                    <span className="tabela-vazia">Sem horários livres.</span>
                  ) : (
                    <div
                      className="acoes"
                      // Os horários passam para a linha seguinte em vez de
                      // alargarem o formulário.
                      style={{ flexWrap: "wrap" }}
                    >
                      {dia.horariosLivres.map((hora) => {
                        const escolhido =
                          horario?.data === dia.data && horario.hora === hora;
                        return (
                          <button
                            key={hora}
                            type="button"
                            className={escolhido ? undefined : "secundario"}
                            aria-pressed={escolhido}
                            aria-label={`${formatarDiaCurto(dia.data)} às ${hora}`}
                            onClick={() =>
                              escolherHorario({ data: dia.data, hora })
                            }
                          >
                            {hora}
                          </button>
                        );
                      })}
                    </div>
                  )}
                </div>
              ))}
            </div>
          ) : null}

          <p>
            Horário escolhido:{" "}
            <strong>
              {horario
                ? `${formatarDiaCurto(horario.data)} às ${horario.hora}`
                : "nenhum"}
            </strong>
            {horario &&
            horarioAtual &&
            horario.data === horarioAtual.data &&
            horario.hora === horarioAtual.hora
              ? " (horário atual)"
              : ""}
          </p>
          {erros.horario ? <p className="campo-erro">{erros.horario}</p> : null}
        </fieldset>

        <div className="campo">
          <label htmlFor={`${idCampos}-motivo`}>Motivo (opcional)</label>
          <input
            id={`${idCampos}-motivo`}
            maxLength={250}
            value={motivo}
            aria-invalid={erros.motivo ? true : undefined}
            onChange={(e) => setMotivo(e.target.value)}
          />
          {erros.motivo ? <p className="campo-erro">{erros.motivo}</p> : null}
        </div>

        <p className="detalhe-conta">
          Estado:{" "}
          <strong>
            {marcacao ? marcacao.estado : "Pendente (estado inicial)"}
          </strong>
        </p>

        <div className="acoes">
          <Link to={voltarPara} className="botao secundario">
            Cancelar
          </Link>
          <button type="submit" disabled={aGuardar}>
            {aGuardar
              ? "A guardar…"
              : aCriar
                ? "Criar marcação"
                : "Guardar alterações"}
          </button>
        </div>
      </form>
    </section>
  );
}

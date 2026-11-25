import { useEffect, useRef, useState, type FormEvent } from "react";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import {
  DIAS_SEMANA,
  DURACAO_MAXIMA_MINUTOS,
  DURACAO_MINIMA_MINUTOS,
  ROTULO_DIA,
  validarHorario,
  type ClinicaConfig,
  type DiaSemana,
  type PeriodoFormulario,
} from "../../models/clinica";
import {
  clinicaService,
  type ServicoClinica,
} from "../../services/ClinicaService";

interface ClinicaConfiguracaoPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoClinica;
}

interface FormularioClinica {
  nome: string;
  nif: string;
  morada: string;
  telefone: string;
  email: string;
  duracao: string;
  periodos: PeriodoFormulario[];
}

function formularioDe(
  config: ClinicaConfig,
  novaChave: () => number,
): FormularioClinica {
  return {
    nome: config.nome,
    nif: config.nif,
    morada: config.morada,
    telefone: config.telefone,
    email: config.email,
    duracao: String(config.duracaoConsultaMinutos),
    periodos: config.horarioFuncionamento.map((p) => ({
      chave: novaChave(),
      diaSemana: p.diaSemana,
      abertura: p.abertura,
      fecho: p.fecho,
    })),
  };
}

/** Problemas dos campos de texto e da duração, antes de qualquer pedido à API. */
function validarDados(f: FormularioClinica): string[] {
  const erros: string[] = [];
  if (!f.nome.trim()) erros.push("Indique o nome da clínica.");
  if (!f.nif.trim()) erros.push("Indique o NIF.");
  if (!f.morada.trim()) erros.push("Indique a morada.");
  if (!f.telefone.trim()) erros.push("Indique o telefone.");
  if (!f.email.trim()) erros.push("Indique o e-mail.");
  const duracao = Number(f.duracao);
  if (
    f.duracao.trim() === "" ||
    !Number.isInteger(duracao) ||
    duracao < DURACAO_MINIMA_MINUTOS ||
    duracao > DURACAO_MAXIMA_MINUTOS
  ) {
    erros.push(
      `A duração da consulta deve ser um número inteiro de minutos entre ${DURACAO_MINIMA_MINUTOS} e ${DURACAO_MAXIMA_MINUTOS}.`,
    );
  }
  return erros;
}

/** Pedido de EP-010: texto aparado e períodos por dia da semana e hora de abertura. */
function pedidoDe(f: FormularioClinica): ClinicaConfig {
  const ordemDia = (d: DiaSemana) => DIAS_SEMANA.indexOf(d);
  return {
    nome: f.nome.trim(),
    nif: f.nif.trim(),
    morada: f.morada.trim(),
    telefone: f.telefone.trim(),
    email: f.email.trim(),
    duracaoConsultaMinutos: Number(f.duracao),
    horarioFuncionamento: [...f.periodos]
      .sort(
        (a, b) =>
          ordemDia(a.diaSemana) - ordemDia(b.diaSemana) ||
          a.abertura.localeCompare(b.abertura),
      )
      .map(({ diaSemana, abertura, fecho }) => ({
        diaSemana,
        abertura,
        fecho,
      })),
  };
}

/**
 * W06 — Clínica, configurações e horários (EP-009 e EP-010; RF012, RF013 e
 * RF019). Só para o Administrador (a rota e o menu já o garantem; a API
 * recusa qualquer outro perfil com 403).
 *
 * Os horários são validados aqui antes de seguirem para a API; depois de
 * gravar, o ecrã mostra o que a API devolveu, ou seja, o que ficou persistido.
 */
export function ClinicaConfiguracaoPage({
  servico = clinicaService,
}: ClinicaConfiguracaoPageProps) {
  const contador = useRef(0);
  const novaChave = () => ++contador.current;

  const [config, setConfig] = useState<ClinicaConfig | null>(null);
  const [formulario, setFormulario] = useState<FormularioClinica | null>(null);
  const [erroCarregar, setErroCarregar] = useState<unknown>(null);
  const [recarregar, setRecarregar] = useState(0);
  const [errosValidacao, setErrosValidacao] = useState<string[]>([]);
  const [erro, setErro] = useState<unknown>(null);
  const [aviso, setAviso] = useState<string | null>(null);
  const [aGuardar, setAGuardar] = useState(false);

  useEffect(() => {
    let cancelado = false;
    servico
      .obter()
      .then((dados) => {
        if (cancelado) return;
        setErroCarregar(null);
        setConfig(dados);
        setFormulario(formularioDe(dados, () => ++contador.current));
      })
      .catch((e: unknown) => {
        if (!cancelado) setErroCarregar(e);
      });
    return () => {
      cancelado = true;
    };
  }, [servico, recarregar]);

  function limparMensagens() {
    setErrosValidacao([]);
    setErro(null);
    setAviso(null);
  }

  function alterar<K extends Exclude<keyof FormularioClinica, "periodos">>(
    campo: K,
    valor: FormularioClinica[K],
  ) {
    setFormulario((atual) => (atual ? { ...atual, [campo]: valor } : atual));
  }

  function alterarPeriodo(
    chave: number,
    campo: "abertura" | "fecho",
    valor: string,
  ) {
    setFormulario((atual) =>
      atual
        ? {
            ...atual,
            periodos: atual.periodos.map((p) =>
              p.chave === chave ? { ...p, [campo]: valor } : p,
            ),
          }
        : atual,
    );
  }

  function adicionarPeriodo(dia: DiaSemana) {
    const chave = novaChave();
    setFormulario((atual) =>
      atual
        ? {
            ...atual,
            periodos: [
              ...atual.periodos,
              { chave, diaSemana: dia, abertura: "", fecho: "" },
            ],
          }
        : atual,
    );
  }

  function removerPeriodo(chave: number) {
    setFormulario((atual) =>
      atual
        ? {
            ...atual,
            periodos: atual.periodos.filter((p) => p.chave !== chave),
          }
        : atual,
    );
  }

  function cancelar() {
    if (!config) return;
    limparMensagens();
    setFormulario(formularioDe(config, novaChave));
  }

  async function guardar(evento: FormEvent) {
    evento.preventDefault();
    if (!formulario) return;
    limparMensagens();

    const problemas = [
      ...validarDados(formulario),
      ...validarHorario(formulario.periodos),
    ];
    if (problemas.length > 0) {
      setErrosValidacao(problemas);
      return;
    }

    setAGuardar(true);
    try {
      const gravado = await servico.atualizar(pedidoDe(formulario));
      setConfig(gravado);
      setFormulario(formularioDe(gravado, novaChave));
      setAviso("Alterações guardadas.");
    } catch (e) {
      setErro(e);
    } finally {
      setAGuardar(false);
    }
  }

  if (formulario === null) {
    return (
      <section>
        <h1>Clínica e horários</h1>
        {erroCarregar ? (
          <>
            <FormErro erro={erroCarregar} />
            <button
              type="button"
              onClick={() => {
                setErroCarregar(null);
                setRecarregar((n) => n + 1);
              }}
            >
              Tentar novamente
            </button>
          </>
        ) : (
          <p role="status">A carregar…</p>
        )}
      </section>
    );
  }

  return (
    <section>
      <h1>Clínica e horários</h1>

      {aviso ? (
        <p role="status" className="sucesso">
          {aviso}
        </p>
      ) : null}

      <form
        className="formulario formulario-largo"
        onSubmit={guardar}
        aria-label="Configuração da clínica"
        noValidate
      >
        {errosValidacao.length > 0 ? (
          <div role="alert" className="form-erro">
            <p>Corrija os seguintes pontos antes de guardar:</p>
            <ul>
              {errosValidacao.map((mensagem) => (
                <li key={mensagem}>{mensagem}</li>
              ))}
            </ul>
          </div>
        ) : null}
        <FormErro erro={erro} />

        <fieldset className="grupo">
          <legend>Dados gerais</legend>
          <FormField
            rotulo="Nome da clínica"
            maxLength={150}
            value={formulario.nome}
            onChange={(e) => alterar("nome", e.target.value)}
          />
          <FormField
            rotulo="NIF"
            inputMode="numeric"
            maxLength={9}
            value={formulario.nif}
            onChange={(e) => alterar("nif", e.target.value)}
          />
          <FormField
            rotulo="Morada"
            maxLength={250}
            value={formulario.morada}
            onChange={(e) => alterar("morada", e.target.value)}
          />
          <FormField
            rotulo="Telefone"
            type="tel"
            maxLength={20}
            value={formulario.telefone}
            onChange={(e) => alterar("telefone", e.target.value)}
          />
          <FormField
            rotulo="E-mail"
            type="email"
            maxLength={256}
            value={formulario.email}
            onChange={(e) => alterar("email", e.target.value)}
          />
          <FormField
            rotulo="Duração da consulta (minutos)"
            type="number"
            min={DURACAO_MINIMA_MINUTOS}
            max={DURACAO_MAXIMA_MINUTOS}
            value={formulario.duracao}
            onChange={(e) => alterar("duracao", e.target.value)}
          />
        </fieldset>

        <fieldset className="grupo">
          <legend>Horário de funcionamento</legend>
          {DIAS_SEMANA.map((dia) => {
            const doDia = formulario.periodos.filter(
              (p) => p.diaSemana === dia,
            );
            return (
              <div key={dia} className="horario-dia">
                <h2 className="horario-dia-nome">{ROTULO_DIA[dia]}</h2>
                <div className="horario-periodos">
                  {doDia.length === 0 ? (
                    <p className="horario-encerrado">Encerrado</p>
                  ) : (
                    doDia.map((p, indice) => {
                      const local = `${ROTULO_DIA[dia]}, período ${indice + 1}`;
                      return (
                        <div key={p.chave} className="horario-periodo">
                          <input
                            type="time"
                            aria-label={`${local} — abertura`}
                            value={p.abertura}
                            onChange={(e) =>
                              alterarPeriodo(
                                p.chave,
                                "abertura",
                                e.target.value,
                              )
                            }
                          />
                          <span aria-hidden="true">–</span>
                          <input
                            type="time"
                            aria-label={`${local} — fecho`}
                            value={p.fecho}
                            onChange={(e) =>
                              alterarPeriodo(p.chave, "fecho", e.target.value)
                            }
                          />
                          <button
                            type="button"
                            className="ligacao perigo-texto"
                            aria-label={`Remover ${local}`}
                            onClick={() => removerPeriodo(p.chave)}
                          >
                            Remover
                          </button>
                        </div>
                      );
                    })
                  )}
                  <button
                    type="button"
                    className="ligacao"
                    aria-label={`Adicionar período a ${ROTULO_DIA[dia]}`}
                    onClick={() => adicionarPeriodo(dia)}
                  >
                    + Adicionar período
                  </button>
                </div>
              </div>
            );
          })}
        </fieldset>

        <div className="acoes">
          <button
            type="button"
            className="ligacao"
            onClick={cancelar}
            disabled={aGuardar}
          >
            Cancelar
          </button>
          <button type="submit" disabled={aGuardar}>
            {aGuardar ? "A guardar…" : "Guardar"}
          </button>
        </div>
      </form>
    </section>
  );
}

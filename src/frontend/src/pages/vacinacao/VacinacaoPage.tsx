import { useEffect, useId, useState, type FormEvent } from "react";
import { Link, useParams } from "react-router-dom";
import { DataTable, type ColunaTabela } from "../../components/DataTable";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import { SoPara } from "../../components/SoPara";
import { hojeEmLisboa } from "../../core/horaLisboa";
import {
  ROTULO_TIPO_ADMINISTRACAO,
  TIPOS_ADMINISTRACAO,
  type Administracao,
  type AdministracaoForm,
  type Boletim,
  type ProximaAdministracao,
  type TipoAdministracao,
} from "../../models/vacinacoes";
import {
  vacinacoesService,
  type ServicoVacinacoes,
} from "../../services/VacinacoesService";
import { formatarData } from "../pacientes/formatacao";
import {
  MAX_INTERVALO_DIAS,
  MAX_LOTE,
  MAX_PRODUTO,
  formularioVazio,
  pedidoDoFormulario,
  validarAdministracao,
  type ErrosAdministracao,
} from "./validacaoAdministracao";

interface VacinacaoPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoVacinacoes;
}

interface ResultadoBoletim {
  chave: string;
  dados: Boletim | null;
  erro: unknown;
}

/** Texto da próxima administração devolvida pela API (RF045). */
function textoProxima(dataProximaPrevista: string | null): string {
  return dataProximaPrevista
    ? `Próxima administração prevista: ${formatarData(dataProximaPrevista)}.`
    : "Sem próxima administração prevista (não foi indicado intervalo).";
}

/**
 * W16 — Boletim de vacinação e desparasitação do paciente (RF043–RF046;
 * EP-042 e EP-043). Só o Veterinário: a rota está protegida por perfil e a
 * API recusa (403) os outros perfis.
 *
 * A próxima administração é sempre a que a API calcula e devolve: o
 * frontend não a calcula. Depois de registar, mostra-se a data devolvida e o
 * boletim é lido de novo.
 *
 * A alteração de administrações (RF056) não faz parte desta tarefa: o RF056
 * ficou excluído da I118 (deliberação 5 do Backlog Refinement de
 * 2026-11-13), até ser especificado e aprovado.
 *
 * A chave obriga a montar a página de novo quando o :id muda.
 */
export function VacinacaoPage(props: VacinacaoPageProps) {
  const { id } = useParams();
  return <BoletimDoPaciente key={id} idTexto={id} {...props} />;
}

interface BoletimDoPacienteProps extends VacinacaoPageProps {
  idTexto: string | undefined;
}

function BoletimDoPaciente({
  idTexto,
  servico = vacinacoesService,
}: BoletimDoPacienteProps) {
  const idCampos = useId();
  const animalId = Number(idTexto);
  const idInvalido = !Number.isInteger(animalId) || animalId <= 0;

  const [tipo, setTipo] = useState<TipoAdministracao | "">("");
  const [versao, setVersao] = useState(0);
  const [resultado, setResultado] = useState<ResultadoBoletim | null>(null);

  const [formularioAberto, setFormularioAberto] = useState(false);
  const [formulario, setFormulario] =
    useState<AdministracaoForm>(formularioVazio);
  const [errosCampos, setErrosCampos] = useState<ErrosAdministracao>({});
  const [erro, setErro] = useState<unknown>(null);
  const [aGuardar, setAGuardar] = useState(false);
  const [aviso, setAviso] = useState<string | null>(null);

  const chave = JSON.stringify([animalId, tipo, versao]);

  useEffect(() => {
    if (idInvalido) return;
    let cancelado = false;
    const [, filtro] = JSON.parse(chave) as [number, TipoAdministracao | ""];
    servico
      .obterBoletim(animalId, filtro || undefined)
      .then((dados) => {
        if (!cancelado) setResultado({ chave, dados, erro: null });
      })
      .catch((e: unknown) => {
        if (!cancelado) setResultado({ chave, dados: null, erro: e });
      });
    return () => {
      cancelado = true;
    };
  }, [servico, animalId, idInvalido, chave]);

  function abrirNova() {
    setFormularioAberto(true);
    setFormulario(formularioVazio(hojeEmLisboa()));
    setErrosCampos({});
    setErro(null);
    setAviso(null);
  }

  function fecharFormulario() {
    setFormularioAberto(false);
    setErrosCampos({});
    setErro(null);
  }

  function alterar<K extends keyof AdministracaoForm>(
    campo: K,
    valor: AdministracaoForm[K],
  ) {
    setFormulario((atual) => ({ ...atual, [campo]: valor }));
  }

  async function guardar(evento: FormEvent) {
    evento.preventDefault();
    if (!formularioAberto) return;
    setErro(null);
    const erros = validarAdministracao(formulario);
    setErrosCampos(erros);
    if (Object.keys(erros).length > 0) return;

    setAGuardar(true);
    try {
      const pedido = pedidoDoFormulario(formulario);
      const guardada = await servico.registar(animalId, pedido);
      const rotulo = ROTULO_TIPO_ADMINISTRACAO[guardada.tipo] ?? guardada.tipo;
      setAviso(
        `${rotulo} ${guardada.produto} registada (administrada em ${formatarData(guardada.dataAdministracao)}). ${textoProxima(
          guardada.dataProximaPrevista,
        )}`,
      );
      setFormularioAberto(false);
      setVersao((v) => v + 1);
    } catch (e) {
      setErro(e);
    } finally {
      setAGuardar(false);
    }
  }

  const titulo = "Vacinação e desparasitação";

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
  const boletim = aCarregar ? null : (resultado?.dados ?? null);
  const erroBoletim = aCarregar ? null : (resultado?.erro ?? null);

  const colunasProximas: ColunaTabela<ProximaAdministracao>[] = [
    {
      cabecalho: "Data prevista",
      celula: (p) => formatarData(p.dataProximaPrevista),
    },
    {
      cabecalho: "Tipo",
      celula: (p) => ROTULO_TIPO_ADMINISTRACAO[p.tipo] ?? p.tipo,
    },
    { cabecalho: "Produto", celula: (p) => p.produto },
  ];

  const colunasHistorico: ColunaTabela<Administracao>[] = [
    {
      cabecalho: "Data",
      celula: (a) => formatarData(a.dataAdministracao),
    },
    {
      cabecalho: "Tipo",
      celula: (a) => ROTULO_TIPO_ADMINISTRACAO[a.tipo] ?? a.tipo,
    },
    { cabecalho: "Produto", celula: (a) => a.produto },
    { cabecalho: "Lote", celula: (a) => a.lote },
    {
      cabecalho: "Intervalo",
      celula: (a) =>
        a.intervaloDias === null ? "—" : `${a.intervaloDias} dias`,
    },
    {
      cabecalho: "Próxima prevista",
      celula: (a) => formatarData(a.dataProximaPrevista),
    },
    { cabecalho: "Veterinário", celula: (a) => a.veterinario },
  ];

  const tituloFormulario = "Registar vacinação ou desparasitação";

  return (
    <section>
      <h1>{titulo}</h1>
      {boletim ? (
        <p className="detalhe-conta">
          Paciente: <strong>{boletim.animal.nome}</strong>
          {boletim.animal.especie ? ` · ${boletim.animal.especie}` : ""}
        </p>
      ) : null}

      {aviso ? (
        <p role="status" className="sucesso">
          {aviso}
        </p>
      ) : null}

      <SoPara acao="registarVacinacao">
        {!formularioAberto ? (
          <div className="acoes">
            <button type="button" onClick={abrirNova}>
              Registar administração
            </button>
          </div>
        ) : (
          <form
            className="formulario"
            onSubmit={guardar}
            aria-label={tituloFormulario}
            noValidate
          >
            <h2>{tituloFormulario}</h2>
            <FormErro erro={erro} />
            <div className="campo">
              <label htmlFor={`${idCampos}-tipo`}>Tipo</label>
              <select
                id={`${idCampos}-tipo`}
                value={formulario.tipo}
                onChange={(e) =>
                  alterar("tipo", e.target.value as TipoAdministracao)
                }
              >
                {TIPOS_ADMINISTRACAO.map((t) => (
                  <option key={t} value={t}>
                    {ROTULO_TIPO_ADMINISTRACAO[t]}
                  </option>
                ))}
              </select>
            </div>
            <FormField
              rotulo="Produto"
              required
              maxLength={MAX_PRODUTO}
              value={formulario.produto}
              onChange={(e) => alterar("produto", e.target.value)}
              erro={errosCampos.produto}
            />
            <FormField
              rotulo="Lote"
              required
              maxLength={MAX_LOTE}
              value={formulario.lote}
              onChange={(e) => alterar("lote", e.target.value)}
              erro={errosCampos.lote}
            />
            <FormField
              rotulo="Data de administração"
              type="date"
              required
              max={hojeEmLisboa()}
              value={formulario.dataAdministracao}
              onChange={(e) => alterar("dataAdministracao", e.target.value)}
              erro={errosCampos.dataAdministracao}
            />
            <FormField
              rotulo="Intervalo até à próxima, em dias (opcional)"
              type="number"
              inputMode="numeric"
              min={1}
              max={MAX_INTERVALO_DIAS}
              step={1}
              value={formulario.intervaloDias}
              onChange={(e) => alterar("intervaloDias", e.target.value)}
              erro={errosCampos.intervaloDias}
            />
            <p className="detalhe-conta">
              A próxima administração é calculada pelo servidor quando guardar.
            </p>
            <div className="acoes">
              <button
                type="button"
                className="secundario"
                onClick={fecharFormulario}
                disabled={aGuardar}
              >
                Cancelar
              </button>
              <button type="submit" disabled={aGuardar}>
                {aGuardar ? "A guardar…" : "Guardar"}
              </button>
            </div>
          </form>
        )}
      </SoPara>

      <div className="filtros">
        <div className="campo">
          <label htmlFor={`${idCampos}-filtro`}>Mostrar</label>
          <select
            id={`${idCampos}-filtro`}
            value={tipo}
            onChange={(e) => setTipo(e.target.value as TipoAdministracao | "")}
          >
            <option value="">Vacinas e desparasitações</option>
            <option value="Vacina">Só vacinas</option>
            <option value="Desparasitacao">Só desparasitações</option>
          </select>
        </div>
      </div>

      {aCarregar ? <p role="status">A carregar…</p> : null}
      {erroBoletim ? (
        <>
          <FormErro erro={erroBoletim} />
          <div className="acoes">
            <button
              type="button"
              className="secundario"
              onClick={() => setVersao((v) => v + 1)}
            >
              Tentar novamente
            </button>
          </div>
        </>
      ) : null}

      {boletim ? (
        <>
          <section aria-labelledby={`${idCampos}-proximas`}>
            <h2 id={`${idCampos}-proximas`}>Próximas administrações</h2>
            <DataTable
              colunas={colunasProximas}
              linhas={boletim.proximas.map((p, indice) => ({
                ...p,
                chave: `${p.tipo}-${p.produto}-${p.dataProximaPrevista}-${indice}`,
              }))}
              chaveLinha={(p) => p.chave}
              mensagemVazia="Sem próximas administrações previstas."
            />
          </section>
          <section aria-labelledby={`${idCampos}-historico`}>
            <h2 id={`${idCampos}-historico`}>Boletim</h2>
            <DataTable
              colunas={colunasHistorico}
              linhas={boletim.administracoes}
              chaveLinha={(a) => a.id}
              mensagemVazia={
                tipo
                  ? "Não há administrações deste tipo registadas."
                  : "Este paciente ainda não tem vacinações nem desparasitações registadas."
              }
            />
          </section>
        </>
      ) : null}

      <div className="acoes">
        <Link to={`/pacientes/${animalId}`} className="botao secundario">
          Voltar à ficha do paciente
        </Link>
      </div>
    </section>
  );
}

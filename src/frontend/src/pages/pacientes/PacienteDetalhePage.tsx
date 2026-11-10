import { useEffect, useId, useState, type FormEvent } from "react";
import { Link, useLocation, useNavigate, useParams } from "react-router-dom";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import { SoPara } from "../../components/SoPara";
import { useAuth } from "../../core/auth/useAuth";
import {
  ESPECIES,
  RACAS_POR_ESPECIE,
  ROTULO_SEXO,
  SEXOS,
  type PacienteDetalhe,
  type PacienteFormState,
  type ProprietarioResumo,
  type Sexo,
} from "../../models/pacientes";
import type { ServicoClientes } from "../../services/ClientesService";
import {
  pacientesService,
  type ServicoPacientes,
} from "../../services/PacientesService";
import {
  formatarData,
  formatarDataHora,
  formatarIdade,
  formatarPeso,
} from "./formatacao";
import { SeletorProprietario } from "./SeletorProprietario";
import {
  hojeIso,
  lerPeso,
  validarPaciente,
  type ErrosPaciente,
} from "./validacaoPaciente";

interface PacienteDetalhePageProps {
  /** Só para testes: permitem trocar os serviços. */
  servico?: ServicoPacientes;
  servicoClientes?: Pick<ServicoClientes, "listar">;
}

interface EstadoNavegacao {
  aviso?: string;
  /** Proprietário já escolhido em W07 (ao criar). */
  proprietario?: ProprietarioResumo;
}

function formularioVazio(
  proprietario: ProprietarioResumo | null,
): PacienteFormState {
  return {
    nome: "",
    especie: "",
    raca: "",
    sexo: "Desconhecido",
    dataNascimento: "",
    peso: "",
    proprietario,
  };
}

function formularioDe(p: PacienteDetalhe): PacienteFormState {
  return {
    nome: p.nome,
    especie: p.especie,
    raca: p.raca ?? "",
    sexo: p.sexo,
    dataNascimento: p.dataNascimento?.slice(0, 10) ?? "",
    peso: p.peso === null ? "" : String(p.peso),
    proprietario: { id: p.cliente.id, nome: p.cliente.nome },
  };
}

/**
 * W08 — Ficha administrativa do paciente (EP-016 a EP-018).
 * Sem :id regista (Rececionista e Veterinário); com :id mostra a ficha,
 * editável para Rececionista e Veterinário e só de leitura para o Auxiliar.
 * Não mostra informação clínica (F004).
 *
 * A chave obriga a montar a página de novo quando o :id muda.
 */
export function PacienteDetalhePage(props: PacienteDetalhePageProps) {
  const { id } = useParams();
  return <FichaPaciente key={id ?? "novo"} idTexto={id} {...props} />;
}

interface FichaPacienteProps extends PacienteDetalhePageProps {
  idTexto: string | undefined;
}

function FichaPaciente({
  idTexto,
  servico = pacientesService,
  servicoClientes,
}: FichaPacienteProps) {
  const navegar = useNavigate();
  const localizacao = useLocation();
  const { pode } = useAuth();
  const idCampos = useId();
  const estado = (localizacao.state as EstadoNavegacao | null) ?? {};

  const id = idTexto === undefined ? null : Number(idTexto);
  const aCriar = id === null;
  const idInvalido = id !== null && (!Number.isInteger(id) || id <= 0);
  const podeEditar = pode("gerirPacientes");

  const [formulario, setFormulario] = useState<PacienteFormState>(() =>
    formularioVazio(estado.proprietario ?? null),
  );
  const [paciente, setPaciente] = useState<PacienteDetalhe | null>(null);
  const [erroCarregar, setErroCarregar] = useState<unknown>(null);
  const [erro, setErro] = useState<unknown>(null);
  const [errosCampos, setErrosCampos] = useState<ErrosPaciente>({});
  const [aGuardar, setAGuardar] = useState(false);
  const [aviso, setAviso] = useState<string | null>(estado.aviso ?? null);

  useEffect(() => {
    if (id === null || idInvalido) return;
    let cancelado = false;
    servico
      .obter(id)
      .then((dados) => {
        if (cancelado) return;
        setPaciente(dados);
        setFormulario(formularioDe(dados));
      })
      .catch((e: unknown) => {
        if (!cancelado) setErroCarregar(e);
      });
    return () => {
      cancelado = true;
    };
  }, [servico, id, idInvalido]);

  function alterar<K extends keyof PacienteFormState>(
    campo: K,
    valor: PacienteFormState[K],
  ) {
    setFormulario((atual) => ({ ...atual, [campo]: valor }));
    setErrosCampos((atuais) => ({ ...atuais, [campo]: undefined }));
  }

  async function guardar(evento: FormEvent) {
    evento.preventDefault();
    setErro(null);
    setAviso(null);

    const erros = validarPaciente(formulario);
    setErrosCampos(erros);
    if (Object.keys(erros).length > 0 || formulario.proprietario === null) {
      return;
    }

    const dados = {
      nome: formulario.nome,
      especie: formulario.especie,
      raca: formulario.raca,
      sexo: formulario.sexo,
      dataNascimento: formulario.dataNascimento,
      peso: lerPeso(formulario.peso),
      clienteId: formulario.proprietario.id,
    };
    setAGuardar(true);
    try {
      if (aCriar) {
        const criado = await servico.criar(dados);
        navegar(`/pacientes/${criado.id}`, {
          replace: true,
          state: {
            aviso: `Paciente ${criado.nome} registado para ${criado.cliente.nome}.`,
          },
        });
        return;
      }
      const atualizado = await servico.atualizar(id as number, dados);
      setPaciente(atualizado);
      setFormulario(formularioDe(atualizado));
      setAviso("Alterações guardadas.");
    } catch (e) {
      setErro(e);
    } finally {
      setAGuardar(false);
    }
  }

  const titulo = aCriar ? "Novo paciente" : "Ficha do paciente";
  const voltar = podeEditar ? (
    <Link to="/pacientes">Voltar à lista de pacientes</Link>
  ) : null;

  if (idInvalido) {
    return (
      <section>
        <h1>{titulo}</h1>
        <div role="alert" className="form-erro">
          O registo pedido não foi encontrado.
        </div>
        {voltar}
      </section>
    );
  }

  if (!aCriar && paciente === null) {
    return (
      <section>
        <h1>{titulo}</h1>
        {erroCarregar ? (
          <>
            <FormErro erro={erroCarregar} />
            {voltar}
          </>
        ) : (
          <p role="status">A carregar…</p>
        )}
      </section>
    );
  }

  const avisoSucesso = aviso ? (
    <p role="status" className="sucesso">
      {aviso}
    </p>
  ) : null;

  const areaClinica = paciente ? (
    <SoPara acao="consultarRegistoClinico">
      <nav aria-label="Área clínica" className="acoes">
        <Link to={`/pacientes/${paciente.id}/registo`}>Registo clínico</Link>
        <SoPara acao="registarVacinacao">
          <Link to={`/pacientes/${paciente.id}/vacinacao`}>Vacinação</Link>
        </SoPara>
      </nav>
    </SoPara>
  ) : null;

  // Auxiliar Veterinário: só leitura, sem ações de edição (critério 2).
  if (!podeEditar && paciente) {
    return (
      <section>
        <h1>{titulo}</h1>
        <dl className="formulario" aria-label="Dados administrativos">
          <dt>Nome</dt>
          <dd>{paciente.nome}</dd>
          <dt>Proprietário</dt>
          <dd>
            {paciente.cliente.nome}
            {paciente.cliente.telefone ? ` · ${paciente.cliente.telefone}` : ""}
          </dd>
          <dt>Espécie</dt>
          <dd>{paciente.especie}</dd>
          <dt>Raça</dt>
          <dd>{paciente.raca ?? "—"}</dd>
          <dt>Sexo</dt>
          <dd>{ROTULO_SEXO[paciente.sexo] ?? paciente.sexo}</dd>
          <dt>Data de nascimento</dt>
          <dd>{formatarData(paciente.dataNascimento)}</dd>
          <dt>Idade</dt>
          <dd>{formatarIdade(paciente.idadeAnos)}</dd>
          <dt>Peso</dt>
          <dd>{formatarPeso(paciente.peso)}</dd>
        </dl>
        {areaClinica}
      </section>
    );
  }

  const racasSugeridas = RACAS_POR_ESPECIE[formulario.especie.trim()] ?? [];

  return (
    <section>
      <h1>{titulo}</h1>
      {avisoSucesso}

      {paciente ? (
        <p className="detalhe-conta">
          Idade: <strong>{formatarIdade(paciente.idadeAnos)}</strong> · Última
          atualização: {formatarDataHora(paciente.atualizadoEm)}
          {paciente.cliente.telefone
            ? ` · Telefone do proprietário: ${paciente.cliente.telefone}`
            : ""}{" "}
          <SoPara acao="gerirClientes">
            <Link to={`/clientes/${paciente.cliente.id}`}>Ver cliente</Link>
          </SoPara>
        </p>
      ) : null}

      <form
        className="formulario"
        onSubmit={guardar}
        aria-label={titulo}
        noValidate
      >
        <FormErro erro={erro} />

        <SeletorProprietario
          selecionado={formulario.proprietario}
          aoSelecionar={(p) => alterar("proprietario", p)}
          erro={errosCampos.proprietario}
          servico={servicoClientes}
        />
        <FormField
          rotulo="Nome do animal"
          required
          maxLength={80}
          value={formulario.nome}
          onChange={(e) => alterar("nome", e.target.value)}
          erro={errosCampos.nome}
        />
        <FormField
          rotulo="Espécie"
          required
          maxLength={50}
          list={`${idCampos}-especies`}
          value={formulario.especie}
          onChange={(e) => alterar("especie", e.target.value)}
          erro={errosCampos.especie}
        />
        <datalist id={`${idCampos}-especies`}>
          {ESPECIES.map((e) => (
            <option key={e} value={e} />
          ))}
        </datalist>
        <FormField
          rotulo="Raça (opcional)"
          maxLength={80}
          list={`${idCampos}-racas`}
          value={formulario.raca}
          onChange={(e) => alterar("raca", e.target.value)}
          erro={errosCampos.raca}
        />
        <datalist id={`${idCampos}-racas`}>
          {racasSugeridas.map((r) => (
            <option key={r} value={r} />
          ))}
        </datalist>
        <div className="campo">
          <label htmlFor={`${idCampos}-sexo`}>Sexo</label>
          <select
            id={`${idCampos}-sexo`}
            value={formulario.sexo}
            onChange={(e) => alterar("sexo", e.target.value as Sexo)}
          >
            {SEXOS.map((s) => (
              <option key={s} value={s}>
                {ROTULO_SEXO[s]}
              </option>
            ))}
          </select>
        </div>
        <FormField
          rotulo="Data de nascimento (opcional)"
          type="date"
          max={hojeIso()}
          value={formulario.dataNascimento}
          onChange={(e) => alterar("dataNascimento", e.target.value)}
          erro={errosCampos.dataNascimento}
        />
        <FormField
          rotulo="Peso em kg (opcional)"
          type="number"
          inputMode="decimal"
          step="0.01"
          min="0.01"
          max="9999.99"
          value={formulario.peso}
          onChange={(e) => alterar("peso", e.target.value)}
          erro={errosCampos.peso}
        />

        <div className="acoes">
          <Link to="/pacientes" className="botao secundario">
            {aCriar ? "Cancelar" : "Voltar à lista"}
          </Link>
          <button type="submit" disabled={aGuardar}>
            {aGuardar ? "A guardar…" : "Guardar dados administrativos"}
          </button>
        </div>
      </form>

      {areaClinica}
    </section>
  );
}

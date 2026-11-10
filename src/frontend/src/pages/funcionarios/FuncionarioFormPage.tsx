import { useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { ConfirmarBloqueio } from "../../components/ConfirmarBloqueio";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import {
  PERFIS_INTERNOS,
  ROTULO_PERFIL,
  perfilTemAutorizacaoApoio,
  type FuncionarioDetalhe,
  type FuncionarioFormState,
  type PerfilInterno,
} from "../../models/utilizadores";
import {
  utilizadoresService,
  type ServicoUtilizadores,
} from "../../services/UtilizadoresService";

interface FuncionarioFormPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoUtilizadores;
}

const FORMULARIO_VAZIO: FuncionarioFormState = {
  nome: "",
  email: "",
  telefone: "",
  perfil: "Rececionista",
  podeRegistarApoio: false,
  passwordInicial: "",
};

function formularioDe(f: FuncionarioDetalhe): FuncionarioFormState {
  return {
    nome: f.nome,
    email: f.email,
    telefone: f.telefone ?? "",
    perfil: f.perfil,
    podeRegistarApoio: f.podeRegistarApoio,
    passwordInicial: "",
  };
}

function dataHora(iso: string): string {
  const data = new Date(iso);
  return Number.isNaN(data.getTime())
    ? iso
    : data.toLocaleString("pt-PT", { dateStyle: "short", timeStyle: "short" });
}

/**
 * W03 — Criar / editar funcionário (EP-005 a EP-008). Só para o Administrador.
 * Sem :id cria uma conta nova; com :id carrega, atualiza e permite bloquear.
 */
export function FuncionarioFormPage({
  servico = utilizadoresService,
}: FuncionarioFormPageProps) {
  const { id: idTexto } = useParams();
  const navegar = useNavigate();
  const id = idTexto === undefined ? null : Number(idTexto);
  const aCriar = id === null;
  const idInvalido = id !== null && (!Number.isInteger(id) || id <= 0);

  const [formulario, setFormulario] =
    useState<FuncionarioFormState>(FORMULARIO_VAZIO);
  const [funcionario, setFuncionario] = useState<FuncionarioDetalhe | null>(
    null,
  );
  const [erroCarregar, setErroCarregar] = useState<unknown>(null);
  const [erro, setErro] = useState<unknown>(null);
  const [aGuardar, setAGuardar] = useState(false);
  const [aviso, setAviso] = useState<string | null>(null);
  const [aConfirmarBloqueio, setAConfirmarBloqueio] = useState(false);

  useEffect(() => {
    if (id === null || idInvalido) return;
    let cancelado = false;
    servico
      .obter(id)
      .then((dados) => {
        if (cancelado) return;
        setFuncionario(dados);
        setFormulario(formularioDe(dados));
      })
      .catch((e: unknown) => {
        if (!cancelado) setErroCarregar(e);
      });
    return () => {
      cancelado = true;
    };
  }, [servico, id, idInvalido]);

  function alterar<K extends keyof FuncionarioFormState>(
    campo: K,
    valor: FuncionarioFormState[K],
  ) {
    setFormulario((atual) => ({ ...atual, [campo]: valor }));
  }

  function alterarPerfil(perfil: PerfilInterno) {
    setFormulario((atual) => ({
      ...atual,
      perfil,
      // A autorização só existe para o Auxiliar; ao mudar de perfil deixa de valer.
      podeRegistarApoio: perfilTemAutorizacaoApoio(perfil)
        ? atual.podeRegistarApoio
        : false,
    }));
  }

  async function guardar(evento: FormEvent) {
    evento.preventDefault();
    setErro(null);
    setAviso(null);
    setAGuardar(true);
    const dados = {
      nome: formulario.nome.trim(),
      email: formulario.email.trim(),
      telefone: formulario.telefone,
      perfil: formulario.perfil,
      podeRegistarApoio: formulario.podeRegistarApoio,
    };
    try {
      if (aCriar) {
        const criado = await servico.criar({
          ...dados,
          passwordInicial: formulario.passwordInicial,
        });
        setFormulario(FORMULARIO_VAZIO);
        navegar("/funcionarios", {
          replace: true,
          state: { aviso: `Conta de ${criado.nome} criada.` },
        });
        return;
      }
      const atualizado = await servico.atualizar(id as number, dados);
      setFuncionario(atualizado);
      setFormulario(formularioDe(atualizado));
      setAviso("Alterações guardadas.");
    } catch (e) {
      setErro(e);
    } finally {
      // A palavra-passe inicial nunca fica no formulário depois de um envio.
      setFormulario((atual) => ({ ...atual, passwordInicial: "" }));
      setAGuardar(false);
    }
  }

  async function bloquear(motivo: string) {
    if (id === null) return;
    await servico.bloquear(id, motivo);
    setAConfirmarBloqueio(false);
    setAviso("Conta bloqueada.");
    try {
      setFuncionario(await servico.obter(id));
    } catch {
      setFuncionario((atual) =>
        atual ? { ...atual, estado: "Bloqueado" } : atual,
      );
    }
  }

  const titulo = aCriar ? "Novo funcionário" : "Editar funcionário";

  if (idInvalido) {
    return (
      <section>
        <h1>{titulo}</h1>
        <div role="alert" className="form-erro">
          O registo pedido não foi encontrado.
        </div>
        <Link to="/funcionarios">Voltar à lista</Link>
      </section>
    );
  }

  if (!aCriar && funcionario === null) {
    return (
      <section>
        <h1>{titulo}</h1>
        {erroCarregar ? (
          <>
            <FormErro erro={erroCarregar} />
            <Link to="/funcionarios">Voltar à lista</Link>
          </>
        ) : (
          <p role="status">A carregar…</p>
        )}
      </section>
    );
  }

  const mostrarApoio = perfilTemAutorizacaoApoio(formulario.perfil);

  return (
    <section>
      <h1>{titulo}</h1>

      {funcionario ? (
        <p className="detalhe-conta">
          Estado: <strong>{funcionario.estado}</strong> · Conta criada em{" "}
          {dataHora(funcionario.criadoEm)}
        </p>
      ) : null}

      {aviso ? (
        <p role="status" className="sucesso">
          {aviso}
        </p>
      ) : null}

      <form className="formulario" onSubmit={guardar} aria-label={titulo}>
        <FormErro erro={erro} />
        <FormField
          rotulo="Nome"
          required
          maxLength={150}
          value={formulario.nome}
          onChange={(e) => alterar("nome", e.target.value)}
        />
        <FormField
          rotulo="E-mail"
          type="email"
          required
          autoComplete="off"
          value={formulario.email}
          onChange={(e) => alterar("email", e.target.value)}
        />
        <FormField
          rotulo="Telefone (opcional)"
          type="tel"
          maxLength={20}
          value={formulario.telefone}
          onChange={(e) => alterar("telefone", e.target.value)}
        />
        {aCriar ? (
          <FormField
            rotulo="Palavra-passe inicial"
            type="password"
            required
            autoComplete="new-password"
            value={formulario.passwordInicial}
            onChange={(e) => alterar("passwordInicial", e.target.value)}
          />
        ) : null}
        <div className="campo">
          <label htmlFor="funcionario-perfil">Perfil</label>
          <select
            id="funcionario-perfil"
            value={formulario.perfil}
            onChange={(e) => alterarPerfil(e.target.value as PerfilInterno)}
          >
            {PERFIS_INTERNOS.map((p) => (
              <option key={p} value={p}>
                {ROTULO_PERFIL[p]}
              </option>
            ))}
          </select>
        </div>
        {mostrarApoio ? (
          <div className="campo campo-caixa">
            <input
              id="funcionario-apoio"
              type="checkbox"
              checked={formulario.podeRegistarApoio}
              onChange={(e) => alterar("podeRegistarApoio", e.target.checked)}
            />
            <label htmlFor="funcionario-apoio">
              Pode registar informação não clínica de apoio
            </label>
          </div>
        ) : null}

        <div className="acoes">
          <Link to="/funcionarios" className="botao secundario">
            Cancelar
          </Link>
          <button type="submit" disabled={aGuardar}>
            {aGuardar ? "A guardar…" : "Guardar"}
          </button>
        </div>
      </form>

      {funcionario && funcionario.estado === "Ativo" ? (
        aConfirmarBloqueio ? (
          <ConfirmarBloqueio
            nome={funcionario.nome}
            aoConfirmar={bloquear}
            aoCancelar={() => setAConfirmarBloqueio(false)}
          />
        ) : (
          <button
            type="button"
            className="perigo"
            onClick={() => {
              setAviso(null);
              setAConfirmarBloqueio(true);
            }}
          >
            Bloquear conta
          </button>
        )
      ) : null}
    </section>
  );
}

import { useEffect, useState, type FormEvent } from "react";
import { Link, useLocation, useNavigate, useParams } from "react-router-dom";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import { SoPara } from "../../components/SoPara";
import type { ClienteDetalhe, ClienteFormState } from "../../models/clientes";
import {
  clientesService,
  type ServicoClientes,
} from "../../services/ClientesService";
import { validarCliente, type ErrosCliente } from "./validacaoCliente";

interface ClienteFormPageProps {
  /** Só para testes: permite trocar o serviço. */
  servico?: ServicoClientes;
}

const FORMULARIO_VAZIO: ClienteFormState = {
  nome: "",
  nif: "",
  telefone: "",
  email: "",
  morada: "",
  passwordInicial: "",
};

function formularioDe(c: ClienteDetalhe): ClienteFormState {
  return {
    nome: c.nome,
    nif: c.nif ?? "",
    telefone: c.telefone,
    email: c.email,
    morada: c.morada ?? "",
    passwordInicial: "",
  };
}

/**
 * W05 — Criar / consultar / editar Cliente (EP-012 a EP-014).
 * Administrador e Rececionista. Sem :id cria; com :id carrega e atualiza.
 *
 * A chave obriga a montar o formulário de novo quando o :id muda
 * (por exemplo, de /clientes/novo para /clientes/7 depois de criar).
 */
export function ClienteFormPage(props: ClienteFormPageProps) {
  const { id } = useParams();
  return <FormularioCliente key={id ?? "novo"} idTexto={id} {...props} />;
}

interface FormularioClienteProps extends ClienteFormPageProps {
  idTexto: string | undefined;
}

function FormularioCliente({
  idTexto,
  servico = clientesService,
}: FormularioClienteProps) {
  const navegar = useNavigate();
  const localizacao = useLocation();
  const id = idTexto === undefined ? null : Number(idTexto);
  const aCriar = id === null;
  const idInvalido = id !== null && (!Number.isInteger(id) || id <= 0);

  const [formulario, setFormulario] =
    useState<ClienteFormState>(FORMULARIO_VAZIO);
  const [cliente, setCliente] = useState<ClienteDetalhe | null>(null);
  const [erroCarregar, setErroCarregar] = useState<unknown>(null);
  const [erro, setErro] = useState<unknown>(null);
  const [errosCampos, setErrosCampos] = useState<ErrosCliente>({});
  const [aGuardar, setAGuardar] = useState(false);
  const [aviso, setAviso] = useState<string | null>(
    () => (localizacao.state as { aviso?: string } | null)?.aviso ?? null,
  );

  useEffect(() => {
    if (id === null || idInvalido) return;
    let cancelado = false;
    servico
      .obter(id)
      .then((dados) => {
        if (cancelado) return;
        setCliente(dados);
        setFormulario(formularioDe(dados));
      })
      .catch((e: unknown) => {
        if (!cancelado) setErroCarregar(e);
      });
    return () => {
      cancelado = true;
    };
  }, [servico, id, idInvalido]);

  function alterar(campo: keyof ClienteFormState, valor: string) {
    setFormulario((atual) => ({ ...atual, [campo]: valor }));
    setErrosCampos((atuais) => ({ ...atuais, [campo]: undefined }));
  }

  async function guardar(evento: FormEvent) {
    evento.preventDefault();
    setErro(null);
    setAviso(null);

    const erros = validarCliente(formulario, aCriar);
    setErrosCampos(erros);
    if (Object.keys(erros).length > 0) return;

    const dados = {
      nome: formulario.nome,
      nif: formulario.nif,
      telefone: formulario.telefone,
      email: formulario.email,
      morada: formulario.morada,
    };
    setAGuardar(true);
    try {
      if (aCriar) {
        const criado = await servico.criar({
          ...dados,
          passwordInicial: formulario.passwordInicial,
        });
        navegar(`/clientes/${criado.id}`, {
          replace: true,
          state: {
            aviso: `Cliente ${criado.nome} registado, com conta de acesso para ${criado.email}.`,
          },
        });
        return;
      }
      const atualizado = await servico.atualizar(id as number, dados);
      setCliente(atualizado);
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

  const titulo = aCriar ? "Novo cliente" : "Ficha de cliente";

  if (idInvalido) {
    return (
      <section>
        <h1>{titulo}</h1>
        <div role="alert" className="form-erro">
          O registo pedido não foi encontrado.
        </div>
        <Link to="/clientes">Voltar à lista</Link>
      </section>
    );
  }

  if (!aCriar && cliente === null) {
    return (
      <section>
        <h1>{titulo}</h1>
        {erroCarregar ? (
          <>
            <FormErro erro={erroCarregar} />
            <Link to="/clientes">Voltar à lista</Link>
          </>
        ) : (
          <p role="status">A carregar…</p>
        )}
      </section>
    );
  }

  return (
    <section>
      <h1>{titulo}</h1>

      {aviso ? (
        <p role="status" className="sucesso">
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

        <fieldset>
          <legend>Dados do cliente</legend>
          <FormField
            rotulo="Nome"
            required
            maxLength={150}
            value={formulario.nome}
            onChange={(e) => alterar("nome", e.target.value)}
            erro={errosCampos.nome}
          />
          <FormField
            rotulo="NIF (opcional)"
            inputMode="numeric"
            maxLength={9}
            value={formulario.nif}
            onChange={(e) => alterar("nif", e.target.value)}
            erro={errosCampos.nif}
          />
          <FormField
            rotulo="Telefone"
            type="tel"
            required
            maxLength={20}
            value={formulario.telefone}
            onChange={(e) => alterar("telefone", e.target.value)}
            erro={errosCampos.telefone}
          />
          <FormField
            rotulo="Morada (opcional)"
            maxLength={250}
            value={formulario.morada}
            onChange={(e) => alterar("morada", e.target.value)}
            erro={errosCampos.morada}
          />
        </fieldset>

        <fieldset>
          <legend>Conta de acesso (aplicação móvel)</legend>
          <FormField
            rotulo="E-mail"
            type="email"
            required
            maxLength={256}
            autoComplete="off"
            value={formulario.email}
            onChange={(e) => alterar("email", e.target.value)}
            erro={errosCampos.email}
          />
          {aCriar ? (
            <FormField
              rotulo="Palavra-passe inicial"
              type="password"
              required
              maxLength={128}
              autoComplete="new-password"
              value={formulario.passwordInicial}
              onChange={(e) => alterar("passwordInicial", e.target.value)}
              erro={errosCampos.passwordInicial}
            />
          ) : (
            <p className="detalhe-conta">
              Estado da conta:{" "}
              <strong>{cliente?.estadoConta ?? "Sem conta de acesso"}</strong>
            </p>
          )}
        </fieldset>

        <div className="acoes">
          <Link to="/clientes" className="botao secundario">
            {aCriar ? "Cancelar" : "Voltar à lista"}
          </Link>
          <button type="submit" disabled={aGuardar}>
            {aGuardar
              ? "A guardar…"
              : aCriar
                ? "Registar cliente"
                : "Guardar alterações"}
          </button>
        </div>
      </form>

      {cliente ? (
        <section aria-labelledby="titulo-animais">
          <h2 id="titulo-animais">Animais associados</h2>
          {cliente.animais.length === 0 ? (
            <p className="tabela-vazia">Este cliente não tem animais.</p>
          ) : (
            <ul>
              {cliente.animais.map((animal) => (
                <li key={animal.id}>
                  {animal.nome}
                  {animal.especie ? ` · ${animal.especie}` : ""}{" "}
                  <SoPara acao="consultarFichaPaciente">
                    <Link to={`/pacientes/${animal.id}`}>Ver paciente</Link>
                  </SoPara>
                </li>
              ))}
            </ul>
          )}
        </section>
      ) : null}
    </section>
  );
}

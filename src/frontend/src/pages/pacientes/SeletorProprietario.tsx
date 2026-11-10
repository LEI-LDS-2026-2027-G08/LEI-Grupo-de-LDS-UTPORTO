import { useId, useState, type KeyboardEvent } from "react";
import { mensagemDeErro } from "../../core/api";
import type { ClienteResumo } from "../../models/clientes";
import type { ProprietarioResumo } from "../../models/pacientes";
import {
  clientesService,
  type ServicoClientes,
} from "../../services/ClientesService";

const MAXIMO_RESULTADOS = 10;

interface SeletorProprietarioProps {
  selecionado: ProprietarioResumo | null;
  aoSelecionar: (proprietario: ProprietarioResumo | null) => void;
  /** Texto do botão que limpa a escolha (ex.: «Alterar» ou «Remover filtro»). */
  rotuloLimpar?: string;
  erro?: string;
  /** Só para testes: permite trocar o serviço. */
  servico?: Pick<ServicoClientes, "listar">;
}

/**
 * Pesquisa e escolha do proprietário (Cliente) dentro da área de pacientes
 * (W07 e W08), através de EP-011 em modo de consulta. O Veterinário usa-a
 * sem ter acesso à gestão de Clientes (W04), como prevê o Modelo de Classes.
 */
export function SeletorProprietario({
  selecionado,
  aoSelecionar,
  rotuloLimpar = "Alterar",
  erro,
  servico = clientesService,
}: SeletorProprietarioProps) {
  const id = useId();
  const [texto, setTexto] = useState("");
  const [resultados, setResultados] = useState<ClienteResumo[] | null>(null);
  const [aProcurar, setAProcurar] = useState(false);
  const [erroPesquisa, setErroPesquisa] = useState<unknown>(null);

  async function procurar() {
    const pesquisa = texto.trim();
    if (pesquisa === "") return;
    setAProcurar(true);
    setErroPesquisa(null);
    try {
      const pagina = await servico.listar({
        pesquisa,
        pagina: 1,
        tamanho: MAXIMO_RESULTADOS,
      });
      setResultados(pagina.itens);
    } catch (e) {
      setResultados(null);
      setErroPesquisa(e);
    } finally {
      setAProcurar(false);
    }
  }

  function teclaNaPesquisa(evento: KeyboardEvent<HTMLInputElement>) {
    // Enter procura o proprietário em vez de submeter o formulário à volta.
    if (evento.key === "Enter") {
      evento.preventDefault();
      void procurar();
    }
  }

  function escolher(cliente: ClienteResumo) {
    aoSelecionar({ id: cliente.id, nome: cliente.nome });
    setResultados(null);
    setTexto("");
  }

  if (selecionado) {
    return (
      <div className="campo">
        <span>Proprietário</span>
        <p>
          <strong>{selecionado.nome}</strong>{" "}
          <button
            type="button"
            className="ligacao"
            onClick={() => aoSelecionar(null)}
          >
            {rotuloLimpar}
          </button>
        </p>
      </div>
    );
  }

  return (
    <div className="campo">
      <label htmlFor={`${id}-pesquisa`}>
        Proprietário (nome, NIF, telefone ou e-mail)
      </label>
      <div className="acoes">
        <input
          id={`${id}-pesquisa`}
          type="search"
          maxLength={100}
          value={texto}
          aria-invalid={erro ? true : undefined}
          aria-describedby={erro ? `${id}-erro` : undefined}
          onChange={(e) => setTexto(e.target.value)}
          onKeyDown={teclaNaPesquisa}
        />
        <button
          type="button"
          className="secundario"
          disabled={aProcurar}
          onClick={() => void procurar()}
        >
          {aProcurar ? "A procurar…" : "Procurar proprietário"}
        </button>
      </div>
      {erro ? (
        <p id={`${id}-erro`} className="campo-erro">
          {erro}
        </p>
      ) : null}
      {erroPesquisa ? (
        <div role="alert" className="form-erro">
          {mensagemDeErro(erroPesquisa)}
        </div>
      ) : null}
      {resultados !== null ? (
        resultados.length === 0 ? (
          <p className="tabela-vazia">Nenhum cliente encontrado.</p>
        ) : (
          <ul aria-label="Clientes encontrados">
            {resultados.map((cliente) => (
              <li key={cliente.id}>
                {cliente.nome} · {cliente.telefone}{" "}
                <button
                  type="button"
                  className="ligacao"
                  aria-label={`Selecionar ${cliente.nome}`}
                  onClick={() => escolher(cliente)}
                >
                  Selecionar
                </button>
              </li>
            ))}
          </ul>
        )
      ) : null}
    </div>
  );
}

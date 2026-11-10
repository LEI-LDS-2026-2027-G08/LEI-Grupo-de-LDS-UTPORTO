import { useId, useState, type KeyboardEvent } from "react";
import { mensagemDeErro } from "../../core/api";
import type { PacienteResumo } from "../../models/pacientes";
import {
  pacientesService,
  type ServicoPacientes,
} from "../../services/PacientesService";

const MAXIMO_RESULTADOS = 10;

/** Paciente escolhido para a marcação. */
export interface PacienteEscolhido {
  id: number;
  nome: string;
  /** Nome do proprietário, só para mostrar. */
  cliente: string;
}

interface SeletorPacienteProps {
  selecionado: PacienteEscolhido | null;
  aoSelecionar: (paciente: PacienteEscolhido | null) => void;
  erro?: string;
  /** Só para testes: permite trocar o serviço. */
  servico?: Pick<ServicoPacientes, "listar">;
}

/**
 * Pesquisa e escolha do paciente de uma marcação (W10), por nome do animal
 * ou do proprietário, através de EP-015.
 */
export function SeletorPaciente({
  selecionado,
  aoSelecionar,
  erro,
  servico = pacientesService,
}: SeletorPacienteProps) {
  const id = useId();
  const [texto, setTexto] = useState("");
  const [resultados, setResultados] = useState<PacienteResumo[] | null>(null);
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
    // Enter procura o paciente em vez de submeter o formulário à volta.
    if (evento.key === "Enter") {
      evento.preventDefault();
      void procurar();
    }
  }

  function escolher(paciente: PacienteResumo) {
    aoSelecionar({
      id: paciente.id,
      nome: paciente.nome,
      cliente: paciente.cliente.nome,
    });
    setResultados(null);
    setTexto("");
  }

  if (selecionado) {
    return (
      <div className="campo">
        <span>Paciente</span>
        <p>
          <strong>{selecionado.nome}</strong> · {selecionado.cliente}{" "}
          <button
            type="button"
            className="ligacao"
            onClick={() => aoSelecionar(null)}
          >
            Alterar
          </button>
        </p>
      </div>
    );
  }

  return (
    <div className="campo">
      <label htmlFor={`${id}-pesquisa`}>
        Paciente (nome do animal ou do proprietário)
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
          {aProcurar ? "A procurar…" : "Procurar paciente"}
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
          <p className="tabela-vazia">Nenhum paciente encontrado.</p>
        ) : (
          <ul aria-label="Pacientes encontrados">
            {resultados.map((p) => (
              <li key={p.id}>
                {p.nome} · {p.especie} · {p.cliente.nome}{" "}
                <button
                  type="button"
                  className="ligacao"
                  aria-label={`Selecionar ${p.nome} (${p.cliente.nome})`}
                  onClick={() => escolher(p)}
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

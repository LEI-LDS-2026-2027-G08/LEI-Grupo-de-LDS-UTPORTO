import { apiClient, type ApiClient } from "../core/api";
import type {
  AtualizarClienteRequest,
  ClienteDetalhe,
  ClienteFiltros,
  ClienteResumo,
  CriarClienteRequest,
} from "../models/clientes";
import type { PagedResult } from "../models/common";

/** Texto aparado; vazio passa a null (campos opcionais: NIF e morada). */
function opcional(valor: string | null | undefined): string | null {
  const limpo = valor?.trim() ?? "";
  return limpo === "" ? null : limpo;
}

/** Corpo comum de EP-013 e EP-014, com os textos aparados. */
function corpoCliente(dados: AtualizarClienteRequest): AtualizarClienteRequest {
  return {
    nome: dados.nome.trim(),
    nif: opcional(dados.nif),
    telefone: dados.telefone.trim(),
    email: dados.email.trim(),
    morada: opcional(dados.morada),
  };
}

/** Chamadas ao backend para os Clientes (EP-011 a EP-014). */
export class ClientesService {
  private readonly api: ApiClient;

  constructor(api: ApiClient = apiClient) {
    this.api = api;
  }

  /** EP-011 — lista paginada; a pesquisa é opcional. */
  listar(filtros: ClienteFiltros): Promise<PagedResult<ClienteResumo>> {
    return this.api.get<PagedResult<ClienteResumo>>("/clientes", {
      query: {
        pesquisa: filtros.pesquisa?.trim() || undefined,
        pagina: filtros.pagina,
        tamanho: filtros.tamanho,
      },
    });
  }

  /** EP-012 — ficha do Cliente, com os animais associados. */
  obter(id: number): Promise<ClienteDetalhe> {
    return this.api.get<ClienteDetalhe>(`/clientes/${id}`);
  }

  /** EP-013 — cria o Cliente e a conta de acesso (201). */
  criar(dados: CriarClienteRequest): Promise<ClienteDetalhe> {
    const corpo: CriarClienteRequest = {
      ...corpoCliente(dados),
      // A palavra-passe vai tal como foi escrita (espaços incluídos).
      passwordInicial: dados.passwordInicial,
    };
    return this.api.post<ClienteDetalhe>("/clientes", corpo);
  }

  /** EP-014 — atualiza os dados editáveis; nunca envia palavra-passe. */
  atualizar(
    id: number,
    dados: AtualizarClienteRequest,
  ): Promise<ClienteDetalhe> {
    return this.api.put<ClienteDetalhe>(`/clientes/${id}`, corpoCliente(dados));
  }
}

export type ServicoClientes = Pick<
  ClientesService,
  "listar" | "obter" | "criar" | "atualizar"
>;

export const clientesService = new ClientesService();

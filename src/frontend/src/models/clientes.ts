/**
 * Tipos dos Clientes (EP-011 a EP-014), iguais aos DTOs da API
 * (ClienteListItemDto, ClienteDto, CreateClienteDto e UpdateClienteDto).
 *
 * Um Cliente tem sempre uma conta de acesso (Utilizador com perfil Cliente):
 * EP-013 cria os dois ao mesmo tempo e o e-mail é o da conta. RF053 e RF054
 * (Cliente sem conta) ainda não estão especificados (ver a I111 no Backlog).
 */

/** Estado da conta de acesso (Utilizador.Estado). */
export type EstadoContaCliente = "Ativo" | "Bloqueado";

/** Linha da lista de Clientes (EP-011). */
export interface ClienteResumo {
  id: number;
  nome: string;
  nif: string | null;
  telefone: string;
  email: string;
  numeroAnimais: number;
}

/** Animal associado ao Cliente, na ficha (EP-012). */
export interface AnimalClienteResumo {
  id: number;
  nome: string;
  especie: string | null;
}

/** Ficha do Cliente (EP-012, e resposta de EP-013 e EP-014). */
export interface ClienteDetalhe {
  id: number;
  nome: string;
  nif: string | null;
  telefone: string;
  /** E-mail da conta de acesso (é com ele que o Cliente inicia sessão). */
  email: string;
  morada: string | null;
  utilizadorId: number | null;
  estadoConta: EstadoContaCliente | null;
  animais: AnimalClienteResumo[];
}

/** Valores do formulário de W05 (todos texto, como nos inputs). */
export interface ClienteFormState {
  nome: string;
  nif: string;
  telefone: string;
  email: string;
  morada: string;
  /** Só usado ao criar; nunca é guardado nem reenviado depois. */
  passwordInicial: string;
}

/** Corpo de EP-014 (PUT /clientes/{id}). */
export interface AtualizarClienteRequest {
  nome: string;
  nif: string | null;
  telefone: string;
  email: string;
  morada: string | null;
}

/** Corpo de EP-013 (POST /clientes): os dados e a palavra-passe inicial. */
export interface CriarClienteRequest extends AtualizarClienteRequest {
  passwordInicial: string;
}

/** Parâmetros de EP-011. */
export interface ClienteFiltros {
  /** Texto procurado no nome, NIF, telefone ou e-mail. */
  pesquisa?: string;
  pagina: number;
  tamanho: number;
}

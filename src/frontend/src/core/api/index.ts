import { lerApiUrl } from "../config";
import { ApiClient } from "./ApiClient";

export { ApiClient, type PedidoOpcoes } from "./ApiClient";
export {
  ApiRequestError,
  mensagemDeErro,
  type CategoriaErro,
} from "./ApiRequestError";

/** Instância única usada por todos os Services da aplicação. */
export const apiClient = new ApiClient(lerApiUrl());

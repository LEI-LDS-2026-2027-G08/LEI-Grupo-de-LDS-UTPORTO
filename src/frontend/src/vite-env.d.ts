/// <reference types="vite/client" />

/**
 * Variáveis de ambiente que a aplicação lê (têm de começar por VITE_).
 * Os valores vêm do ficheiro `.env` (ver `.env.example`).
 */
interface ImportMetaEnv {
  /** URL base do backend, sem barra final. Ex.: http://localhost:5159 */
  readonly VITE_API_URL?: string;
}

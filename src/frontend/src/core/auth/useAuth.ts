import { useContext } from "react";
import { AuthContext, type AuthContextValue } from "./contextoAuth";

/** Acesso à sessão a partir de qualquer componente dentro do AuthProvider. */
export function useAuth(): AuthContextValue {
  const contexto = useContext(AuthContext);
  if (contexto === null) {
    throw new Error("useAuth tem de ser usado dentro de <AuthProvider>.");
  }
  return contexto;
}

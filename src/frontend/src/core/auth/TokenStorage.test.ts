import { beforeEach, describe, expect, it } from "vitest";
import { sessaoDe } from "../../test/auxiliares";
import { TokenStorage } from "./TokenStorage";

describe("TokenStorage", () => {
  beforeEach(() => sessionStorage.clear());

  it("guarda e devolve a sessão", () => {
    const sessao = sessaoDe("Rececionista");
    TokenStorage.guardar(sessao);
    expect(TokenStorage.obter()).toEqual(sessao);
  });

  it("devolve null quando não há sessão", () => {
    expect(TokenStorage.obter()).toBeNull();
  });

  it("descarta uma sessão expirada", () => {
    TokenStorage.guardar({
      ...sessaoDe("Rececionista"),
      expiraEm: "2000-01-01T00:00:00Z",
    });
    expect(TokenStorage.obter()).toBeNull();
    expect(sessionStorage.length).toBe(0);
  });

  it("descarta conteúdo corrompido ou com perfil não web", () => {
    sessionStorage.setItem("clinica.sessao", "{isto não é json");
    expect(TokenStorage.obter()).toBeNull();

    TokenStorage.guardar(sessaoDe("Cliente"));
    expect(TokenStorage.obter()).toBeNull();
  });

  it("limpar remove a sessão", () => {
    TokenStorage.guardar(sessaoDe("Veterinario"));
    TokenStorage.limpar();
    expect(TokenStorage.obter()).toBeNull();
  });

  it("não usa o localStorage (o token não sobrevive ao fecho do separador)", () => {
    TokenStorage.guardar(sessaoDe("Veterinario"));
    expect(localStorage.length).toBe(0);
  });
});

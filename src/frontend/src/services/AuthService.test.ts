import { describe, expect, it, vi } from "vitest";
import { ApiClient, ApiRequestError } from "../core/api";
import { resposta } from "../test/auxiliares";
import { AuthService } from "./AuthService";

function servicoCom(status: number, corpo?: unknown) {
  const cliente = new ApiClient(
    "http://api.teste",
    vi.fn().mockResolvedValue(resposta(status, corpo)),
  );
  return new AuthService(cliente);
}

async function falhaDoLogin(status: number, corpo: unknown) {
  return (await servicoCom(status, corpo)
    .login({ email: "a@b.pt", password: "x" })
    .catch((e: unknown) => e)) as ApiRequestError;
}

describe("AuthService.login", () => {
  it("credenciais incorretas: mensagem fixa que não diz qual campo errou", async () => {
    const erro = await falhaDoLogin(401, {
      code: 1,
      message: "Utilizador não existe",
    });
    expect(erro.message).toBe("Email ou palavra-passe incorretos.");
    expect(erro.message).not.toMatch(/não existe/);
  });

  it("conta bloqueada: mensagem própria", async () => {
    const erro = await falhaDoLogin(403, {
      code: 2,
      message: "Conta bloqueada",
    });
    expect(erro.categoria).toBe("semPermissao");
    expect(erro.message).toMatch(/bloqueada/);
  });

  it("campos em falta: pede email e palavra-passe", async () => {
    const erro = await falhaDoLogin(400, { code: 3, message: "..." });
    expect(erro.message).toBe("Indique o email e a palavra-passe.");
  });

  it("deixa passar os outros erros (ex.: 500 genérico)", async () => {
    const erro = await falhaDoLogin(500, { code: 4, message: "stack trace" });
    expect(erro.categoria).toBe("servidor");
    expect(erro.message).not.toMatch(/stack/);
  });

  it("devolve a sessão quando o login é aceite", async () => {
    const sessao = {
      token: "t",
      expiraEm: "2099-01-01T00:00:00Z",
      utilizador: {},
    };
    await expect(
      servicoCom(200, sessao).login({ email: "a@b.pt", password: "x" }),
    ).resolves.toEqual(sessao);
  });
});

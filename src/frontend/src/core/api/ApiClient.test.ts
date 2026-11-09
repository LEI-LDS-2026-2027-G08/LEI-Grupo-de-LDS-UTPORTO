import { describe, expect, it, vi } from "vitest";
import { resposta } from "../../test/auxiliares";
import { ApiClient } from "./ApiClient";
import { ApiRequestError, type CategoriaErro } from "./ApiRequestError";

const URL_BASE = "http://api.teste";

function criar(fetchFalso: typeof fetch) {
  return new ApiClient(URL_BASE, fetchFalso);
}

describe("ApiClient", () => {
  it("junta o token Bearer aos pedidos", async () => {
    const fetchFalso = vi.fn().mockResolvedValue(resposta(200, { ok: true }));
    const cliente = criar(fetchFalso);
    cliente.setToken("abc");

    await cliente.get("/clientes");

    const [url, init] = fetchFalso.mock.calls[0];
    expect(url).toBe("http://api.teste/clientes");
    expect(init.headers.Authorization).toBe("Bearer abc");
  });

  it("não envia o token em pedidos públicos", async () => {
    const fetchFalso = vi.fn().mockResolvedValue(resposta(200, {}));
    const cliente = criar(fetchFalso);
    cliente.setToken("abc");

    await cliente.post("/auth/login", { email: "a" }, { publico: true });

    expect(fetchFalso.mock.calls[0][1].headers.Authorization).toBeUndefined();
  });

  it("envia o corpo como JSON e aceita cabeçalhos extra (If-Match)", async () => {
    const fetchFalso = vi.fn().mockResolvedValue(resposta(200, {}));
    const cliente = criar(fetchFalso);

    await cliente.put(
      "/marcacoes/1",
      { nota: "x" },
      { headers: { "If-Match": '"v1"' } },
    );

    const init = fetchFalso.mock.calls[0][1];
    expect(init.method).toBe("PUT");
    expect(init.body).toBe('{"nota":"x"}');
    expect(init.headers["Content-Type"]).toBe("application/json");
    expect(init.headers["If-Match"]).toBe('"v1"');
  });

  it("monta a query string ignorando valores vazios", async () => {
    const fetchFalso = vi.fn().mockResolvedValue(resposta(200, {}));
    const cliente = criar(fetchFalso);

    await cliente.get("clientes", {
      query: { pagina: 2, nome: "Ana", filtro: undefined },
    });

    expect(fetchFalso.mock.calls[0][0]).toBe(
      "http://api.teste/clientes?pagina=2&nome=Ana",
    );
  });

  it("devolve undefined em respostas 204", async () => {
    const cliente = criar(vi.fn().mockResolvedValue(resposta(204)));
    await expect(cliente.post("/auth/logout")).resolves.toBeUndefined();
  });

  it.each<[number, CategoriaErro]>([
    [400, "validacao"],
    [401, "naoAutenticado"],
    [403, "semPermissao"],
    [404, "naoEncontrado"],
    [409, "conflito"],
    [412, "versaoDesatualizada"],
    [422, "regraNegocio"],
    [500, "servidor"],
  ])(
    "traduz o estado %i em ApiRequestError '%s'",
    async (status, categoria) => {
      const cliente = criar(
        vi.fn().mockResolvedValue(
          resposta(status, {
            code: status,
            message: "Mensagem fixa.",
            detail: "Explicação específica.",
          }),
        ),
      );

      const erro = await cliente.get("/x").catch((e: unknown) => e);

      expect(erro).toBeInstanceOf(ApiRequestError);
      expect((erro as ApiRequestError).status).toBe(status);
      expect((erro as ApiRequestError).categoria).toBe(categoria);
    },
  );

  it("mostra o detail da API (o message é fixo por código)", async () => {
    const cliente = criar(
      vi.fn().mockResolvedValue(
        resposta(409, {
          code: 409,
          message: "Conflito ao processar o pedido.",
          detail: "O e-mail já está registado.",
        }),
      ),
    );
    const erro = (await cliente
      .post("/x", {})
      .catch((e: unknown) => e)) as ApiRequestError;
    expect(erro.message).toBe("O e-mail já está registado.");
    expect(erro.erro?.code).toBe(409);
  });

  it("num 400 mostra as mensagens de todos os campos juntas (detail)", async () => {
    const cliente = criar(
      vi.fn().mockResolvedValue(
        resposta(400, {
          code: 400,
          message: "Pedido inválido.",
          detail: "O e-mail é obrigatório. O peso deve ser superior a 0.",
        }),
      ),
    );
    const erro = (await cliente
      .post("/x", {})
      .catch((e: unknown) => e)) as ApiRequestError;
    expect(erro.categoria).toBe("validacao");
    expect(erro.message).toBe(
      "O e-mail é obrigatório. O peso deve ser superior a 0.",
    );
  });

  it("sem detail (ou vazio) usa o message da API", async () => {
    for (const corpo of [
      { code: 404, message: "Recurso não encontrado." },
      { code: 404, message: "Recurso não encontrado.", detail: "  " },
      { code: 404, message: "Recurso não encontrado.", detail: null },
    ]) {
      const cliente = criar(vi.fn().mockResolvedValue(resposta(404, corpo)));
      const erro = (await cliente
        .get("/x")
        .catch((e: unknown) => e)) as ApiRequestError;
      expect(erro.message).toBe("Recurso não encontrado.");
    }
  });

  it("esconde os detalhes técnicos dos erros 500", async () => {
    const cliente = criar(
      vi.fn().mockResolvedValue(
        resposta(500, {
          code: 500,
          message: "Ocorreu um erro interno no servidor.",
          detail: "NullReferenceException em Foo.cs:42",
        }),
      ),
    );
    const erro = (await cliente
      .get("/x")
      .catch((e: unknown) => e)) as ApiRequestError;
    expect(erro.message).not.toContain("NullReference");
    expect(erro.erro).toBeNull();
  });

  it("usa mensagem por omissão se o erro não vier em JSON", async () => {
    const cliente = criar(
      vi.fn().mockResolvedValue(new Response("<html>", { status: 403 })),
    );
    const erro = (await cliente
      .get("/x")
      .catch((e: unknown) => e)) as ApiRequestError;
    expect(erro.categoria).toBe("semPermissao");
    expect(erro.message).toMatch(/permissão/);
  });

  it("avisa quem estiver registado quando recebe 401", async () => {
    const cliente = criar(
      vi.fn().mockResolvedValue(resposta(401, { code: 1, message: "x" })),
    );
    const aoNaoAutorizado = vi.fn();
    cliente.definirAoNaoAutorizado(aoNaoAutorizado);

    await cliente.get("/x").catch(() => undefined);

    expect(aoNaoAutorizado).toHaveBeenCalledTimes(1);
  });

  it("não trata como sessão expirada um 401 num pedido público (login)", async () => {
    const cliente = criar(
      vi.fn().mockResolvedValue(
        resposta(401, {
          code: 401,
          message: "Autenticação necessária.",
          detail: "Credenciais inválidas.",
        }),
      ),
    );
    const aoNaoAutorizado = vi.fn();
    cliente.definirAoNaoAutorizado(aoNaoAutorizado);

    const erro = (await cliente
      .post("/auth/login", {}, { publico: true })
      .catch((e: unknown) => e)) as ApiRequestError;

    expect(aoNaoAutorizado).not.toHaveBeenCalled();
    expect(erro.message).toBe("Credenciais inválidas.");
  });

  it("converte falhas de rede em erro de categoria 'rede'", async () => {
    const cliente = criar(
      vi.fn().mockRejectedValue(new TypeError("Failed to fetch")),
    );
    const erro = (await cliente
      .get("/x")
      .catch((e: unknown) => e)) as ApiRequestError;
    expect(erro.categoria).toBe("rede");
  });

  it("avisa claramente se falta configurar o URL da API", async () => {
    const fetchFalso = vi.fn();
    const cliente = new ApiClient("", fetchFalso);
    const erro = (await cliente
      .get("/x")
      .catch((e: unknown) => e)) as ApiRequestError;
    expect(erro.categoria).toBe("configuracao");
    expect(fetchFalso).not.toHaveBeenCalled();
  });
});

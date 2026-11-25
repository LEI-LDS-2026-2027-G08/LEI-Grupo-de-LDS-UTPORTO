import { describe, expect, it, vi } from "vitest";
import { ApiClient, ApiRequestError } from "../core/api";
import { resposta } from "../test/auxiliares";
import { ClientesService } from "./ClientesService";

function preparar(status: number, corpo?: unknown) {
  const fetchFn = vi.fn().mockResolvedValue(resposta(status, corpo));
  const servico = new ClientesService(
    new ApiClient("http://api.teste", fetchFn),
  );
  const pedido = () => {
    const [url, init] = fetchFn.mock.calls[0] as [string, RequestInit];
    return {
      url,
      metodo: init.method,
      corpo: init.body ? JSON.parse(init.body as string) : undefined,
    };
  };
  return { servico, pedido };
}

const carlos = {
  id: 7,
  nome: "Carlos Sousa",
  nif: "123456789",
  telefone: "912345678",
  email: "carlos@email.pt",
  morada: "Rua A, 1",
  utilizadorId: 30,
  estadoConta: "Ativo",
  animais: [],
};

describe("ClientesService", () => {
  it("listar: EP-011 com a pesquisa aparada e a página", async () => {
    const { servico, pedido } = preparar(200, {
      itens: [],
      pagina: 2,
      tamanho: 20,
      total: 0,
    });
    await servico.listar({ pesquisa: "  sousa ", pagina: 2, tamanho: 20 });
    expect(pedido()).toMatchObject({
      url: "http://api.teste/clientes?pesquisa=sousa&pagina=2&tamanho=20",
      metodo: "GET",
    });
  });

  it("listar: sem pesquisa não envia o parâmetro", async () => {
    const { servico, pedido } = preparar(200, {
      itens: [],
      pagina: 1,
      tamanho: 20,
      total: 0,
    });
    await servico.listar({ pesquisa: "   ", pagina: 1, tamanho: 20 });
    expect(pedido().url).toBe("http://api.teste/clientes?pagina=1&tamanho=20");
  });

  it("obter: EP-012", async () => {
    const { servico, pedido } = preparar(200, carlos);
    const cliente = await servico.obter(7);
    expect(cliente.nome).toBe("Carlos Sousa");
    expect(pedido()).toMatchObject({
      url: "http://api.teste/clientes/7",
      metodo: "GET",
    });
  });

  it("criar: EP-013 com a palavra-passe e os opcionais vazios a null", async () => {
    const { servico, pedido } = preparar(201, carlos);
    await servico.criar({
      nome: " Carlos Sousa ",
      nif: "  ",
      telefone: " 912345678 ",
      email: " carlos@email.pt ",
      morada: "",
      passwordInicial: " Temp#2026",
    });
    expect(pedido()).toEqual({
      url: "http://api.teste/clientes",
      metodo: "POST",
      corpo: {
        nome: "Carlos Sousa",
        nif: null,
        telefone: "912345678",
        email: "carlos@email.pt",
        morada: null,
        passwordInicial: " Temp#2026",
      },
    });
  });

  it("atualizar: EP-014 sem palavra-passe", async () => {
    const { servico, pedido } = preparar(200, carlos);
    await servico.atualizar(7, {
      nome: "Carlos Sousa",
      nif: "123456789",
      telefone: "912345678",
      email: "carlos@email.pt",
      morada: " Rua A, 1 ",
    });
    expect(pedido()).toEqual({
      url: "http://api.teste/clientes/7",
      metodo: "PUT",
      corpo: {
        nome: "Carlos Sousa",
        nif: "123456789",
        telefone: "912345678",
        email: "carlos@email.pt",
        morada: "Rua A, 1",
      },
    });
  });

  it("um 409 chega à página com o detail da API", async () => {
    const { servico } = preparar(409, {
      code: 409,
      message: "Conflito.",
      detail: "Já existe uma conta com este e-mail.",
    });
    const falha = servico.criar({
      nome: "Rita",
      nif: "",
      telefone: "919876543",
      email: "rita@email.pt",
      morada: "",
      passwordInicial: "Temp#2026",
    });
    await expect(falha).rejects.toBeInstanceOf(ApiRequestError);
    await expect(falha).rejects.toMatchObject({
      categoria: "conflito",
      message: "Já existe uma conta com este e-mail.",
    });
  });
});

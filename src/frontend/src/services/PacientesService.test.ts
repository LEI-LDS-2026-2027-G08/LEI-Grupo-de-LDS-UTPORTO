import { describe, expect, it, vi } from "vitest";
import { ApiClient } from "../core/api";
import { resposta } from "../test/auxiliares";
import { PacientesService } from "./PacientesService";

function preparar(status: number, corpo?: unknown) {
  const fetchFn = vi.fn().mockResolvedValue(resposta(status, corpo));
  const servico = new PacientesService(
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

const bobi = {
  id: 3,
  nome: "Bobi",
  especie: "Cão",
  raca: "Labrador Retriever",
  sexo: "Macho",
  dataNascimento: "2022-05-10",
  idadeAnos: 4,
  peso: 28.4,
  cliente: { id: 7, nome: "João Costa", telefone: "960000001" },
  atualizadoEm: "2026-10-05T10:12:00Z",
};

const paginaVazia = { itens: [], pagina: 1, tamanho: 20, total: 0 };

describe("PacientesService", () => {
  it("listar: EP-015 com proprietário, pesquisa e espécie", async () => {
    const { servico, pedido } = preparar(200, paginaVazia);
    await servico.listar({
      clienteId: 7,
      pesquisa: " bobi ",
      especie: "Cão",
      pagina: 1,
      tamanho: 20,
    });
    expect(pedido()).toMatchObject({
      url: "http://api.teste/animais?clienteId=7&pesquisa=bobi&especie=C%C3%A3o&pagina=1&tamanho=20",
      metodo: "GET",
    });
  });

  it("listar: sem filtros só envia a página", async () => {
    const { servico, pedido } = preparar(200, paginaVazia);
    await servico.listar({
      pesquisa: "  ",
      especie: "",
      pagina: 2,
      tamanho: 20,
    });
    expect(pedido().url).toBe("http://api.teste/animais?pagina=2&tamanho=20");
  });

  it("obter: EP-016", async () => {
    const { servico, pedido } = preparar(200, bobi);
    const paciente = await servico.obter(3);
    expect(paciente.cliente.nome).toBe("João Costa");
    expect(pedido()).toMatchObject({
      url: "http://api.teste/animais/3",
      metodo: "GET",
    });
  });

  it("criar: EP-017 com os opcionais vazios a null", async () => {
    const { servico, pedido } = preparar(201, bobi);
    await servico.criar({
      nome: " Bobi ",
      especie: " Cão ",
      raca: "  ",
      sexo: "Macho",
      dataNascimento: "",
      peso: null,
      clienteId: 7,
    });
    expect(pedido()).toEqual({
      url: "http://api.teste/animais",
      metodo: "POST",
      corpo: {
        nome: "Bobi",
        especie: "Cão",
        raca: null,
        sexo: "Macho",
        dataNascimento: null,
        peso: null,
        clienteId: 7,
      },
    });
  });

  it("atualizar: EP-018 com todos os dados administrativos", async () => {
    const { servico, pedido } = preparar(200, bobi);
    await servico.atualizar(3, {
      nome: "Bobi",
      especie: "Cão",
      raca: "Labrador Retriever",
      sexo: "Macho",
      dataNascimento: "2022-05-10",
      peso: 29.1,
      clienteId: 7,
    });
    expect(pedido()).toEqual({
      url: "http://api.teste/animais/3",
      metodo: "PUT",
      corpo: {
        nome: "Bobi",
        especie: "Cão",
        raca: "Labrador Retriever",
        sexo: "Macho",
        dataNascimento: "2022-05-10",
        peso: 29.1,
        clienteId: 7,
      },
    });
  });
});

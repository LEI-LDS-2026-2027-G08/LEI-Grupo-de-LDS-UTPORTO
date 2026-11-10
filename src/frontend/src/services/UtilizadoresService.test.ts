import { describe, expect, it, vi } from "vitest";
import { ApiClient } from "../core/api";
import { resposta } from "../test/auxiliares";
import { UtilizadoresService } from "./UtilizadoresService";

function preparar(status: number, corpo?: unknown) {
  const fetchFn = vi.fn().mockResolvedValue(resposta(status, corpo));
  const servico = new UtilizadoresService(
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

const resumo = {
  id: 9,
  nome: "Rui Lopes",
  email: "rui@clinica.pt",
  perfil: "AuxiliarVeterinario",
  estado: "Ativo",
  podeRegistarApoio: true,
};

describe("UtilizadoresService", () => {
  it("listar: EP-004 com os filtros preenchidos e sem os vazios", async () => {
    const { servico, pedido } = preparar(200, {
      itens: [],
      pagina: 2,
      tamanho: 20,
      total: 0,
    });
    await servico.listar({
      perfil: "Veterinario",
      pesquisa: "  ana ",
      pagina: 2,
      tamanho: 20,
    });
    const { url, metodo } = pedido();
    expect(metodo).toBe("GET");
    expect(url).toBe(
      "http://api.teste/utilizadores?perfil=Veterinario&pesquisa=ana&pagina=2&tamanho=20",
    );
  });

  it("obter: EP-005", async () => {
    const { servico, pedido } = preparar(200, { ...resumo, criadoEm: "x" });
    await servico.obter(9);
    expect(pedido()).toMatchObject({
      url: "http://api.teste/utilizadores/9",
      metodo: "GET",
    });
  });

  it("criar: EP-006 com a palavra-passe e a autorização do Auxiliar", async () => {
    const { servico, pedido } = preparar(201, resumo);
    const criado = await servico.criar({
      nome: "Rui Lopes",
      email: "rui@clinica.pt",
      telefone: " 934567890 ",
      perfil: "AuxiliarVeterinario",
      podeRegistarApoio: true,
      passwordInicial: "Temp#2026",
    });
    expect(criado.id).toBe(9);
    expect(pedido()).toEqual({
      url: "http://api.teste/utilizadores",
      metodo: "POST",
      corpo: {
        nome: "Rui Lopes",
        email: "rui@clinica.pt",
        telefone: "934567890",
        perfil: "AuxiliarVeterinario",
        podeRegistarApoio: true,
        passwordInicial: "Temp#2026",
      },
    });
  });

  it("criar: não envia podeRegistarApoio para perfis que não são Auxiliar", async () => {
    const { servico, pedido } = preparar(201, resumo);
    await servico.criar({
      nome: "Ana",
      email: "ana@clinica.pt",
      telefone: "",
      perfil: "Veterinario",
      podeRegistarApoio: true,
      passwordInicial: "Temp#2026",
    });
    const { corpo } = pedido();
    expect(corpo).not.toHaveProperty("podeRegistarApoio");
    expect(corpo.telefone).toBeNull();
  });

  it("atualizar: EP-007 sem palavra-passe", async () => {
    const { servico, pedido } = preparar(200, { ...resumo, criadoEm: "x" });
    await servico.atualizar(9, {
      nome: "Rui Lopes",
      email: "rui@clinica.pt",
      telefone: null,
      perfil: "Rececionista",
      podeRegistarApoio: true,
    });
    const { url, metodo, corpo } = pedido();
    expect(url).toBe("http://api.teste/utilizadores/9");
    expect(metodo).toBe("PUT");
    expect(corpo).toEqual({
      nome: "Rui Lopes",
      email: "rui@clinica.pt",
      telefone: null,
      perfil: "Rececionista",
    });
  });

  it("bloquear: EP-008 com o estado Bloqueado e o motivo, se houver", async () => {
    const { servico, pedido } = preparar(204);
    await servico.bloquear(9, "  Saiu da clínica. ");
    expect(pedido()).toEqual({
      url: "http://api.teste/utilizadores/9/estado",
      metodo: "PATCH",
      corpo: { estado: "Bloqueado", motivo: "Saiu da clínica." },
    });
  });

  it("bloquear: sem motivo envia só o estado", async () => {
    const { servico, pedido } = preparar(204);
    await servico.bloquear(9, "   ");
    expect(pedido().corpo).toEqual({ estado: "Bloqueado" });
  });
});

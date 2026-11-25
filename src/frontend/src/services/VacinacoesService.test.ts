import { describe, expect, it, vi } from "vitest";
import { ApiClient } from "../core/api";
import { resposta } from "../test/auxiliares";
import { VacinacoesService } from "./VacinacoesService";

function preparar(status: number, corpo?: unknown) {
  const fetchFn = vi.fn().mockResolvedValue(resposta(status, corpo));
  const servico = new VacinacoesService(
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

const administracao = {
  id: 21,
  tipo: "Vacina",
  produto: "Nobivac",
  lote: "L-2026-01",
  dataAdministracao: "2026-11-10",
  intervaloDias: 365,
  dataProximaPrevista: "2027-11-10",
  veterinario: "Dra. Ana",
};

describe("VacinacoesService", () => {
  it("obterBoletim: EP-042 sem filtro", async () => {
    const { servico, pedido } = preparar(200, {
      animal: { id: 3, nome: "Bobi", especie: "Cão" },
      administracoes: [],
      proximas: [],
    });
    await servico.obterBoletim(3);
    expect(pedido()).toMatchObject({
      url: "http://api.teste/animais/3/vacinacoes",
      metodo: "GET",
    });
  });

  it("obterBoletim: EP-042 filtrado por tipo", async () => {
    const { servico, pedido } = preparar(200, {
      animal: { id: 3, nome: "Bobi", especie: "Cão" },
      administracoes: [],
      proximas: [],
    });
    await servico.obterBoletim(3, "Desparasitacao");
    expect(pedido().url).toBe(
      "http://api.teste/animais/3/vacinacoes?tipo=Desparasitacao",
    );
  });

  it("registar: EP-043 com textos aparados e sem enviar a próxima prevista", async () => {
    const { servico, pedido } = preparar(201, administracao);
    const criada = await servico.registar(3, {
      tipo: "Vacina",
      produto: "  Nobivac ",
      lote: " L-2026-01 ",
      dataAdministracao: "2026-11-10",
      intervaloDias: 365,
    });
    expect(pedido()).toEqual({
      url: "http://api.teste/animais/3/vacinacoes",
      metodo: "POST",
      corpo: {
        tipo: "Vacina",
        produto: "Nobivac",
        lote: "L-2026-01",
        dataAdministracao: "2026-11-10",
        intervaloDias: 365,
      },
    });
    expect(criada.dataProximaPrevista).toBe("2027-11-10");
  });
});

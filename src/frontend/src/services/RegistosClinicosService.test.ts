import { describe, expect, it, vi } from "vitest";
import { ApiClient, ApiRequestError } from "../core/api";
import { resposta } from "../test/auxiliares";
import { RegistosClinicosService } from "./RegistosClinicosService";

function preparar(...respostas: Response[]) {
  const fetchFn = vi.fn();
  for (const r of respostas) fetchFn.mockResolvedValueOnce(r);
  const servico = new RegistosClinicosService(
    new ApiClient("http://api.teste", fetchFn),
  );
  const pedido = (i = 0) => {
    const [url, init] = fetchFn.mock.calls[i] as [string, RequestInit];
    return {
      url,
      metodo: init.method,
      corpo: init.body ? JSON.parse(init.body as string) : undefined,
    };
  };
  return { servico, pedido, fetchFn };
}

const registo = {
  id: 9,
  consultaId: 41,
  animalId: 3,
  veterinario: "Dra. Ana",
  criadoEm: "2026-11-20T10:40:00Z",
  notasClinicas: "Otite externa.",
  diagnostico: "Otite",
  tratamentos: [{ id: 1, descricao: "Limpeza", dataAplicacao: "2026-11-20" }],
  prescricoes: [],
  atualizadoEm: null,
  atualizadoPor: null,
};

describe("RegistosClinicosService", () => {
  it("criar: EP-031 com consultaId, textos aparados e opcionais vazios a null", async () => {
    const { servico, pedido } = preparar(resposta(201, registo));
    await servico.criar({
      consultaId: 41,
      notasClinicas: "  Otite externa.  ",
      diagnostico: "   ",
      tratamentos: [{ descricao: " Limpeza ", dataAplicacao: "2026-11-20" }],
      prescricoes: [
        {
          medicamento: " Otomax ",
          dosagem: "5 gotas",
          frequencia: "2x/dia",
          duracaoDias: null,
        },
      ],
    });
    expect(pedido()).toEqual({
      url: "http://api.teste/registos-clinicos",
      metodo: "POST",
      corpo: {
        consultaId: 41,
        notasClinicas: "Otite externa.",
        diagnostico: null,
        tratamentos: [{ descricao: "Limpeza", dataAplicacao: "2026-11-20" }],
        prescricoes: [
          {
            medicamento: "Otomax",
            dosagem: "5 gotas",
            frequencia: "2x/dia",
            duracaoDias: null,
          },
        ],
      },
    });
  });

  it("atualizar: EP-032 mantém os ids dos elementos existentes e não envia consultaId", async () => {
    const { servico, pedido } = preparar(resposta(200, registo));
    await servico.atualizar(9, {
      notasClinicas: "Notas",
      diagnostico: "Otite",
      tratamentos: [
        { id: 1, descricao: "Limpeza", dataAplicacao: "2026-11-20" },
        { descricao: "Reavaliação", dataAplicacao: "2026-11-27" },
      ],
      prescricoes: [],
    });
    const { url, metodo, corpo } = pedido();
    expect(url).toBe("http://api.teste/registos-clinicos/9");
    expect(metodo).toBe("PUT");
    expect(corpo).not.toHaveProperty("consultaId");
    expect(corpo.tratamentos).toEqual([
      { id: 1, descricao: "Limpeza", dataAplicacao: "2026-11-20" },
      { descricao: "Reavaliação", dataAplicacao: "2026-11-27" },
    ]);
    expect(corpo.prescricoes).toEqual([]);
  });

  it("obter: EP-033", async () => {
    const { servico, pedido } = preparar(resposta(200, registo));
    await expect(servico.obter(9)).resolves.toEqual(registo);
    expect(pedido()).toMatchObject({
      url: "http://api.teste/registos-clinicos/9",
      metodo: "GET",
    });
  });

  it("obterHistorico: EP-034 com e sem período", async () => {
    const historico = { animal: { id: 3, nome: "Bobi" }, registos: [] };
    const { servico, pedido } = preparar(
      resposta(200, historico),
      resposta(200, historico),
    );
    await servico.obterHistorico(3);
    await servico.obterHistorico(3, {
      dataInicio: "2026-01-01",
      dataFim: "2026-11-30",
    });
    expect(pedido(0).url).toBe("http://api.teste/animais/3/historico-clinico");
    expect(pedido(1).url).toBe(
      "http://api.teste/animais/3/historico-clinico?dataInicio=2026-01-01&dataFim=2026-11-30",
    );
  });

  it("obterDaConsulta: procura no histórico do paciente e lê o registo da consulta", async () => {
    const { servico, pedido } = preparar(
      resposta(200, {
        animal: { id: 3, nome: "Bobi" },
        registos: [
          {
            id: 8,
            consultaId: 30,
            data: "2026-10-01",
            veterinario: "Dra. Ana",
            diagnostico: null,
            numeroTratamentos: 0,
            numeroPrescricoes: 0,
          },
          {
            id: 9,
            consultaId: 41,
            data: "2026-11-20",
            veterinario: "Dra. Ana",
            diagnostico: "Otite",
            numeroTratamentos: 1,
            numeroPrescricoes: 0,
          },
        ],
      }),
      resposta(200, registo),
    );
    await expect(servico.obterDaConsulta(3, 41)).resolves.toEqual(registo);
    expect(pedido(1).url).toBe("http://api.teste/registos-clinicos/9");
  });

  it("obterDaConsulta: devolve null quando a consulta ainda não tem registo", async () => {
    const { servico, fetchFn } = preparar(
      resposta(200, { animal: { id: 3, nome: "Bobi" }, registos: [] }),
    );
    await expect(servico.obterDaConsulta(3, 41)).resolves.toBeNull();
    expect(fetchFn).toHaveBeenCalledTimes(1);
  });

  it("propaga o erro da API (ex.: 409, a consulta já tem registo)", async () => {
    const { servico } = preparar(
      resposta(409, {
        code: 409,
        message: "Conflito.",
        detail: "A consulta já tem registo clínico.",
      }),
    );
    const erro = await servico
      .criar({
        consultaId: 41,
        notasClinicas: "x",
        diagnostico: null,
        tratamentos: [],
        prescricoes: [],
      })
      .catch((e: unknown) => e);
    expect(erro).toBeInstanceOf(ApiRequestError);
    expect((erro as ApiRequestError).categoria).toBe("conflito");
    expect((erro as ApiRequestError).message).toBe(
      "A consulta já tem registo clínico.",
    );
  });
});

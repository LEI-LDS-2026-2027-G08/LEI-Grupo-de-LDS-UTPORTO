import { describe, expect, it, vi } from "vitest";
import { ApiClient } from "../core/api";
import { AgendaService } from "./AgendaService";

/** Resposta JSON falsa com ETag. */
function respostaComVersao(status: number, corpo: unknown, etag?: string) {
  const cabecalhos: Record<string, string> = {
    "Content-Type": "application/json",
  };
  if (etag) cabecalhos.ETag = etag;
  return new Response(corpo === undefined ? null : JSON.stringify(corpo), {
    status,
    headers: cabecalhos,
  });
}

const marcacao = {
  id: 41,
  dataHora: "2026-11-20T10:00:00Z",
  duracaoMinutos: 30,
  estado: "Pendente",
  motivo: null,
  animal: { id: 3, nome: "Bobi", especie: "Cão" },
  cliente: { id: 7, nome: "João Costa" },
  veterinario: { id: 5, nome: "Dra. Ana" },
  chegadaEm: null,
  notasPreparacao: [],
};

function preparar(...respostas: Response[]) {
  const fetchFn = vi.fn();
  for (const r of respostas) fetchFn.mockResolvedValueOnce(r);
  const servico = new AgendaService(new ApiClient("http://api.teste", fetchFn));
  const pedido = (i: number) => {
    const [url, init] = fetchFn.mock.calls[i] as [
      string,
      RequestInit & { headers: Record<string, string> },
    ];
    return {
      url,
      metodo: init.method,
      ifMatch: init.headers["If-Match"],
      corpo: init.body ? JSON.parse(init.body as string) : undefined,
    };
  };
  return { servico, pedido };
}

describe("AgendaService", () => {
  it("listarVeterinarios: EP-021", async () => {
    const { servico, pedido } = preparar(
      respostaComVersao(200, [{ id: 5, nome: "Dra. Ana" }]),
    );
    expect(await servico.listarVeterinarios()).toHaveLength(1);
    expect(pedido(0)).toMatchObject({
      url: "http://api.teste/veterinarios",
      metodo: "GET",
    });
  });

  it("obterDisponibilidade: EP-022 com o período", async () => {
    const { servico, pedido } = preparar(
      respostaComVersao(200, {
        veterinarioId: 5,
        duracaoConsultaMinutos: 30,
        dias: [],
      }),
    );
    await servico.obterDisponibilidade(5, "2026-11-20", "2026-11-26");
    expect(pedido(0).url).toBe(
      "http://api.teste/veterinarios/5/disponibilidade?dataInicio=2026-11-20&dataFim=2026-11-26",
    );
  });

  it("criar: EP-025 com o motivo vazio a null", async () => {
    const { servico, pedido } = preparar(
      respostaComVersao(201, marcacao, '"v1"'),
    );
    await servico.criar({
      animalId: 3,
      veterinarioId: 5,
      dataHora: "2026-11-20T10:00:00.000Z",
      motivo: "  ",
    });
    expect(pedido(0)).toMatchObject({
      url: "http://api.teste/consultas",
      metodo: "POST",
      corpo: {
        animalId: 3,
        veterinarioId: 5,
        dataHora: "2026-11-20T10:00:00.000Z",
        motivo: null,
      },
    });
  });

  it("guarda o ETag lido (EP-024) e envia-o em If-Match ao alterar (EP-026)", async () => {
    const { servico, pedido } = preparar(
      respostaComVersao(200, marcacao, '"v1"'),
      respostaComVersao(200, marcacao, '"v2"'),
      respostaComVersao(204, undefined, '"v3"'),
      respostaComVersao(204, undefined, '"v4"'),
    );
    await servico.obterConsulta(41);
    await servico.atualizar(41, {
      veterinarioId: 6,
      dataHora: "2026-11-21T09:00:00.000Z",
      motivo: " Revisão ",
    });
    await servico.alterarEstado(41, { estado: "Confirmada", observacao: "" });
    await servico.alterarEstado(41, {
      estado: "Cancelada",
      observacao: "Pedido do tutor.",
    });

    expect(pedido(1)).toEqual({
      url: "http://api.teste/consultas/41",
      metodo: "PUT",
      ifMatch: '"v1"',
      corpo: {
        veterinarioId: 6,
        dataHora: "2026-11-21T09:00:00.000Z",
        motivo: "Revisão",
      },
    });
    expect(pedido(2)).toEqual({
      url: "http://api.teste/consultas/41/estado",
      metodo: "PATCH",
      ifMatch: '"v2"',
      corpo: { estado: "Confirmada", observacao: null },
    });
    // O ETag devolvido pelo 204 de EP-027 é o que segue no pedido seguinte.
    expect(pedido(3)).toMatchObject({
      ifMatch: '"v3"',
      corpo: { estado: "Cancelada", observacao: "Pedido do tutor." },
    });
  });

  it("sem versão conhecida não envia If-Match", async () => {
    const { servico, pedido } = preparar(respostaComVersao(204, undefined));
    await servico.alterarEstado(99, { estado: "Confirmada", observacao: null });
    expect(pedido(0).ifMatch).toBeUndefined();
  });

  it("listarConsultas: EP-023 com o dia, o Veterinário e o estado", async () => {
    const { servico, pedido } = preparar(
      respostaComVersao(200, { itens: [], pagina: 1, tamanho: 100, total: 0 }),
    );
    await servico.listarConsultas({
      dataInicio: "2026-11-20",
      dataFim: "2026-11-20",
      veterinarioId: 5,
      estado: "Confirmada",
      pagina: 1,
      tamanho: 100,
    });
    expect(pedido(0).url).toBe(
      "http://api.teste/consultas?dataInicio=2026-11-20&dataFim=2026-11-20&veterinarioId=5&estado=Confirmada&pagina=1&tamanho=100",
    );
  });

  it("registarChegada: EP-028 com If-Match e guarda o novo ETag", async () => {
    const { servico, pedido } = preparar(
      respostaComVersao(200, marcacao, '"v1"'),
      respostaComVersao(
        200,
        { id: 41, chegadaEm: "2026-11-20T09:52:00Z" },
        '"v2"',
      ),
      respostaComVersao(204, undefined, '"v3"'),
    );
    await servico.obterConsulta(41);
    const chegada = await servico.registarChegada(41);
    await servico.alterarEstado(41, { estado: "Realizada", observacao: null });

    expect(chegada.chegadaEm).toBe("2026-11-20T09:52:00Z");
    expect(pedido(1)).toEqual({
      url: "http://api.teste/consultas/41/chegada",
      metodo: "PATCH",
      ifMatch: '"v1"',
      corpo: undefined,
    });
    expect(pedido(2).ifMatch).toBe('"v2"');
  });

  it("registarNotaPreparacao: EP-029 com o texto aparado", async () => {
    const { servico, pedido } = preparar(
      respostaComVersao(201, {
        id: 2,
        consultaId: 41,
        texto: "Sala 2 preparada.",
        autor: "Carla",
        criadaEm: "2026-11-20T09:25:00Z",
      }),
    );
    await servico.registarNotaPreparacao(41, "  Sala 2 preparada.  ");
    expect(pedido(0)).toEqual({
      url: "http://api.teste/consultas/41/notas-preparacao",
      metodo: "POST",
      ifMatch: undefined,
      corpo: { texto: "Sala 2 preparada." },
    });
  });
});

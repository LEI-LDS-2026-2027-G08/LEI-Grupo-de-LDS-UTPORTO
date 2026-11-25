import { describe, expect, it, vi } from "vitest";
import { ApiClient, ApiRequestError } from "../core/api";
import type { ClinicaConfig } from "../models/clinica";
import { resposta } from "../test/auxiliares";
import { ClinicaService } from "./ClinicaService";

function preparar(status: number, corpo?: unknown) {
  const fetchFn = vi.fn().mockResolvedValue(resposta(status, corpo));
  const cliente = new ApiClient("http://api.teste", fetchFn);
  cliente.setToken("token-de-teste");
  const servico = new ClinicaService(cliente);
  const pedido = () => {
    const [url, init] = fetchFn.mock.calls[0] as [string, RequestInit];
    return {
      url,
      metodo: init.method,
      cabecalhos: init.headers as Record<string, string>,
      corpo: init.body ? JSON.parse(init.body as string) : undefined,
    };
  };
  return { servico, pedido };
}

const clinica: ClinicaConfig = {
  nome: "Clínica Veterinária G08",
  nif: "501234567",
  morada: "Rua do Exemplo, 100, 4470-000 Maia",
  telefone: "229000000",
  email: "geral@clinica-g08.pt",
  duracaoConsultaMinutos: 30,
  horarioFuncionamento: [
    { diaSemana: "Segunda", abertura: "09:00", fecho: "13:00" },
    { diaSemana: "Segunda", abertura: "14:00", fecho: "19:00" },
    { diaSemana: "Sabado", abertura: "09:00", fecho: "13:00" },
  ],
};

describe("ClinicaService", () => {
  it("obter: EP-009, GET /clinica com o token e sem corpo", async () => {
    const { servico, pedido } = preparar(200, clinica);
    await expect(servico.obter()).resolves.toEqual(clinica);
    const { url, metodo, cabecalhos, corpo } = pedido();
    expect(metodo).toBe("GET");
    expect(url).toBe("http://api.teste/clinica");
    expect(cabecalhos.Authorization).toBe("Bearer token-de-teste");
    expect(corpo).toBeUndefined();
  });

  it("atualizar: EP-010, PUT /clinica com os dados e o horário completos", async () => {
    const { servico, pedido } = preparar(200, clinica);
    await expect(servico.atualizar(clinica)).resolves.toEqual(clinica);
    const { url, metodo, corpo } = pedido();
    expect(metodo).toBe("PUT");
    expect(url).toBe("http://api.teste/clinica");
    expect(corpo).toEqual(clinica);
  });

  it("atualizar: um 400 chega como erro de validação com o texto da API", async () => {
    const { servico } = preparar(400, {
      code: 400,
      message: "Pedido inválido.",
      detail: "Segunda: os períodos 09:00–13:00 e 12:00–19:00 sobrepõem-se.",
    });
    const erro = await servico.atualizar(clinica).catch((e: unknown) => e);
    expect(erro).toBeInstanceOf(ApiRequestError);
    expect((erro as ApiRequestError).status).toBe(400);
    expect((erro as ApiRequestError).message).toContain("sobrepõem-se");
  });

  it("obter: um 403 (perfil sem permissão) chega como erro", async () => {
    const { servico } = preparar(403, { code: 403, message: "Acesso negado." });
    const erro = await servico.obter().catch((e: unknown) => e);
    expect(erro).toBeInstanceOf(ApiRequestError);
    expect((erro as ApiRequestError).status).toBe(403);
  });
});

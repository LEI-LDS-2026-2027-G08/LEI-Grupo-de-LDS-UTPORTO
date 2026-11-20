import { describe, expect, it } from "vitest";
import {
  formularioDeAdministracao,
  formularioVazio,
  lerIntervalo,
  pedidoDoFormulario,
  validarAdministracao,
} from "./validacaoAdministracao";

describe("validarAdministracao", () => {
  it("exige produto, lote e data", () => {
    expect(validarAdministracao(formularioVazio())).toEqual({
      produto: "O produto é obrigatório.",
      lote: "O lote é obrigatório.",
      dataAdministracao: "A data de administração é obrigatória.",
    });
  });

  it("limita produto (150) e lote (50)", () => {
    const erros = validarAdministracao({
      ...formularioVazio("2026-11-10"),
      produto: "p".repeat(151),
      lote: "l".repeat(51),
    });
    expect(erros.produto).toMatch(/150/);
    expect(erros.lote).toMatch(/50/);
  });

  it("intervalo opcional, inteiro entre 1 e 32767", () => {
    const base = {
      ...formularioVazio("2026-11-10"),
      produto: "Nobivac",
      lote: "L1",
    };
    expect(validarAdministracao(base)).toEqual({});
    expect(validarAdministracao({ ...base, intervaloDias: "365" })).toEqual({});
    for (const intervalo of ["0", "-5", "1.5", "abc", "32768"]) {
      expect(
        validarAdministracao({ ...base, intervaloDias: intervalo })
          .intervaloDias,
      ).toBeDefined();
    }
  });
});

describe("conversões do formulário", () => {
  it("lerIntervalo: vazio é null, inteiro é número", () => {
    expect(lerIntervalo(" ")).toBeNull();
    expect(lerIntervalo("30")).toBe(30);
    expect(lerIntervalo("3,5")).toBeNaN();
  });

  it("pedidoDoFormulario nunca envia a próxima prevista", () => {
    const form = formularioDeAdministracao({
      id: 21,
      tipo: "Vacina",
      produto: "Nobivac",
      lote: "L1",
      dataAdministracao: "2025-11-10",
      intervaloDias: null,
      dataProximaPrevista: null,
      veterinario: "Dra. Ana",
    });
    expect(form.intervaloDias).toBe("");
    expect(pedidoDoFormulario(form)).toEqual({
      tipo: "Vacina",
      produto: "Nobivac",
      lote: "L1",
      dataAdministracao: "2025-11-10",
      intervaloDias: null,
    });
  });
});

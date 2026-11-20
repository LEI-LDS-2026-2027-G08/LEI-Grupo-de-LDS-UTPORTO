import { describe, expect, it } from "vitest";
import {
  MENSAGEM_FIM_ANTERIOR,
  MENSAGEM_FORA_DOS_LIMITES,
  validarPeriodo,
} from "./periodoHistorico";

describe("validarPeriodo (EP-034)", () => {
  it("as duas datas são opcionais", () => {
    expect(validarPeriodo("", "")).toBeNull();
    expect(validarPeriodo("2026-01-01", "")).toBeNull();
    expect(validarPeriodo("", "2026-12-31")).toBeNull();
  });

  it("aceita o mesmo dia e um período normal", () => {
    expect(validarPeriodo("2026-11-20", "2026-11-20")).toBeNull();
    expect(validarPeriodo("2026-01-01", "2026-11-20")).toBeNull();
  });

  it("recusa a data de fim anterior à de início", () => {
    expect(validarPeriodo("2026-11-20", "2026-11-19")).toBe(
      MENSAGEM_FIM_ANTERIOR,
    );
  });

  it("recusa datas fora de 2000-01-01 a 9998-12-31", () => {
    expect(validarPeriodo("1999-12-31", "")).toBe(MENSAGEM_FORA_DOS_LIMITES);
    expect(validarPeriodo("", "9999-01-01")).toBe(MENSAGEM_FORA_DOS_LIMITES);
  });
});

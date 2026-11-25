import { describe, expect, it } from "vitest";
import {
  validarHorario,
  type DiaSemana,
  type PeriodoFormulario,
} from "./clinica";

let chave = 0;
function periodo(
  diaSemana: DiaSemana,
  abertura: string,
  fecho: string,
): PeriodoFormulario {
  return { chave: ++chave, diaSemana, abertura, fecho };
}

describe("validarHorario (RF019)", () => {
  it("aceita um horário sem períodos (clínica sempre encerrada)", () => {
    expect(validarHorario([])).toEqual([]);
  });

  it("aceita vários períodos no mesmo dia e períodos seguidos", () => {
    expect(
      validarHorario([
        periodo("Segunda", "09:00", "13:00"),
        periodo("Segunda", "14:00", "19:00"),
        periodo("Terca", "09:00", "13:00"),
        periodo("Terca", "13:00", "19:00"),
      ]),
    ).toEqual([]);
  });

  it("aceita os períodos fora de ordem", () => {
    expect(
      validarHorario([
        periodo("Sabado", "14:00", "18:00"),
        periodo("Sabado", "09:00", "13:00"),
      ]),
    ).toEqual([]);
  });

  it("rejeita fecho igual à abertura", () => {
    expect(validarHorario([periodo("Quarta", "09:00", "09:00")])).toEqual([
      "Quarta, período 1: o fecho tem de ser posterior à abertura.",
    ]);
  });

  it("rejeita fecho anterior à abertura", () => {
    expect(validarHorario([periodo("Sexta", "19:00", "09:00")])).toEqual([
      "Sexta, período 1: o fecho tem de ser posterior à abertura.",
    ]);
  });

  it("rejeita períodos sobrepostos no mesmo dia, mesmo fora de ordem", () => {
    expect(
      validarHorario([
        periodo("Segunda", "12:00", "19:00"),
        periodo("Segunda", "09:00", "13:00"),
      ]),
    ).toEqual(["Segunda: os períodos 09:00–13:00 e 12:00–19:00 sobrepõem-se."]);
  });

  it("não confunde períodos de dias diferentes", () => {
    expect(
      validarHorario([
        periodo("Segunda", "09:00", "19:00"),
        periodo("Terca", "09:00", "19:00"),
      ]),
    ).toEqual([]);
  });

  it("rejeita horas por preencher ou mal escritas, indicando o dia e o período", () => {
    expect(
      validarHorario([
        periodo("Quinta", "09:00", "13:00"),
        periodo("Quinta", "", "19:00"),
        periodo("Domingo", "25:00", "26:00"),
      ]),
    ).toEqual([
      "Quinta, período 2: indique a hora de abertura e a de fecho.",
      "Domingo, período 1: indique a hora de abertura e a de fecho.",
    ]);
  });

  it("reúne todos os problemas de uma vez", () => {
    expect(
      validarHorario([
        periodo("Segunda", "09:00", "09:00"),
        periodo("Terca", "09:00", "13:00"),
        periodo("Terca", "12:00", "15:00"),
      ]),
    ).toHaveLength(2);
  });
});

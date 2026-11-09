import { describe, expect, it } from "vitest";
import type { Perfil } from "../models/auth";
import { itensMenuPara, rotaInicial } from "./navegacao";

const rotulos = (perfil: Perfil) => itensMenuPara(perfil).map((i) => i.rotulo);

describe("menu por perfil", () => {
  it("Administrador", () => {
    expect(rotulos("Administrador")).toEqual([
      "Clientes",
      "Funcionários",
      "Clínica e horários",
    ]);
  });

  it("Rececionista", () => {
    expect(rotulos("Rececionista")).toEqual([
      "Agenda",
      "Pacientes",
      "Clientes",
    ]);
  });

  it("Veterinário", () => {
    expect(rotulos("Veterinario")).toEqual(["Agenda", "Pacientes"]);
  });

  it("Auxiliar veterinário", () => {
    expect(rotulos("AuxiliarVeterinario")).toEqual(["Preparação de consultas"]);
  });

  it("Cliente não vê nada", () => {
    expect(rotulos("Cliente")).toEqual([]);
  });

  it("a página inicial é o primeiro item do menu", () => {
    expect(rotaInicial("Rececionista")).toBe("/agenda");
    expect(rotaInicial("Administrador")).toBe("/clientes");
    expect(rotaInicial("AuxiliarVeterinario")).toBe("/preparacao");
  });
});

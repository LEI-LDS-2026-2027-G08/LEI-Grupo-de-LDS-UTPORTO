import { describe, expect, it } from "vitest";
import type { Perfil } from "../../models/auth";
import { PERMISSOES, perfilPodeExecutar, type Acao } from "./permissoes";

const acoesDe = (perfil: Perfil) =>
  (Object.keys(PERMISSOES) as Acao[]).filter((a) =>
    perfilPodeExecutar(perfil, a),
  );

describe("permissões por ação (RF005)", () => {
  it("Administrador: contas, clientes e clínica, sem ações clínicas", () => {
    expect(acoesDe("Administrador")).toEqual([
      "gerirFuncionarios",
      "gerirClientes",
      "configurarClinica",
    ]);
  });

  it("Rececionista: administrativo e marcações, sem escrita clínica", () => {
    expect(acoesDe("Rececionista")).toEqual([
      "gerirClientes",
      "gerirPacientes",
      "consultarFichaPaciente",
      "gerirMarcacoes",
    ]);
  });

  it("Veterinário: clínica e vacinação, sem gerir marcações", () => {
    const acoes = acoesDe("Veterinario");
    expect(acoes).toContain("registarClinico");
    expect(acoes).toContain("registarVacinacao");
    expect(acoes).not.toContain("gerirMarcacoes");
    expect(acoes).not.toContain("gerirClientes");
  });

  it("Auxiliar: só leitura da ficha e do registo clínico, mais o apoio não clínico", () => {
    expect(acoesDe("AuxiliarVeterinario")).toEqual([
      "consultarFichaPaciente",
      "consultarRegistoClinico",
      "registarApoioNaoClinico",
    ]);
  });

  it("Cliente não pode nada na aplicação web", () => {
    expect(acoesDe("Cliente")).toEqual([]);
  });
});

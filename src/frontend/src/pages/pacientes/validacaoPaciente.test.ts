import { describe, expect, it } from "vitest";
import type { PacienteFormState } from "../../models/pacientes";
import { hojeIso, lerPeso, validarPaciente } from "./validacaoPaciente";

const valido: PacienteFormState = {
  nome: "Bobi",
  especie: "Cão",
  raca: "",
  sexo: "Macho",
  dataNascimento: "2022-05-10",
  peso: "28,4",
  proprietario: { id: 7, nome: "João Costa" },
};
const HOJE = "2026-11-10";

describe("validacaoPaciente", () => {
  it("um formulário válido não tem erros", () => {
    expect(validarPaciente(valido, HOJE)).toEqual({});
  });

  it("raça, data de nascimento e peso são opcionais", () => {
    expect(
      validarPaciente(
        { ...valido, raca: "", dataNascimento: "", peso: "" },
        HOJE,
      ),
    ).toEqual({});
  });

  it("exige nome, espécie e proprietário", () => {
    const erros = validarPaciente(
      { ...valido, nome: " ", especie: "", proprietario: null },
      HOJE,
    );
    expect(erros.nome).toBe("O nome é obrigatório.");
    expect(erros.especie).toBe("A espécie é obrigatória.");
    expect(erros.proprietario).toMatch(/proprietário/);
  });

  it("recusa a data de nascimento futura, mas aceita a de hoje", () => {
    expect(
      validarPaciente({ ...valido, dataNascimento: "2026-11-11" }, HOJE)
        .dataNascimento,
    ).toBe("A data de nascimento não pode ser futura.");
    expect(validarPaciente({ ...valido, dataNascimento: HOJE }, HOJE)).toEqual(
      {},
    );
  });

  it.each(["0", "-2", "10000", "abc"])("recusa o peso %s", (peso) => {
    expect(validarPaciente({ ...valido, peso }, HOJE).peso).toBe(
      "O peso deve estar entre 0,01 e 9999,99 kg.",
    );
  });

  it("lê o peso com vírgula ou ponto", () => {
    expect(lerPeso("28,4")).toBe(28.4);
    expect(lerPeso("28.4")).toBe(28.4);
    expect(lerPeso(" ")).toBeNull();
  });

  it("hojeIso usa o formato aaaa-mm-dd", () => {
    expect(hojeIso(new Date(2026, 0, 5))).toBe("2026-01-05");
  });
});

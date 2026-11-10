import { describe, expect, it } from "vitest";
import type { ClienteFormState } from "../../models/clientes";
import { nifValido, validarCliente } from "./validacaoCliente";

const valido: ClienteFormState = {
  nome: "Carlos Sousa",
  nif: "123456789",
  telefone: "+351 912 345 678",
  email: "carlos@email.pt",
  morada: "",
  passwordInicial: "Temp#2026",
};

describe("validacaoCliente", () => {
  it.each(["123456789", "501964843", "287024008"])("aceita o NIF %s", (nif) =>
    expect(nifValido(nif)).toBe(true),
  );

  it.each(["123456780", "12345678", "12345678a", ""])(
    "recusa o NIF '%s'",
    (nif) => expect(nifValido(nif)).toBe(false),
  );

  it("um formulário válido não tem erros, ao criar e ao editar", () => {
    expect(validarCliente(valido, true)).toEqual({});
    expect(validarCliente({ ...valido, passwordInicial: "" }, false)).toEqual(
      {},
    );
  });

  it("o NIF e a morada são opcionais", () => {
    expect(validarCliente({ ...valido, nif: " ", morada: "" }, true)).toEqual(
      {},
    );
  });

  it("recusa telefones fora do formato da API", () => {
    expect(
      validarCliente({ ...valido, telefone: "12345" }, false).telefone,
    ).toMatch(/entre 9 e 20 caracteres/);
    expect(
      validarCliente({ ...valido, telefone: "91234567x" }, false).telefone,
    ).toBeDefined();
  });

  it("recusa e-mails sem domínio e nomes demasiado longos", () => {
    const erros = validarCliente(
      { ...valido, email: "carlos@", nome: "a".repeat(151) },
      false,
    );
    expect(erros.email).toBe("O e-mail é inválido.");
    expect(erros.nome).toBe("O nome não pode ter mais de 150 caracteres.");
  });

  it("só exige a palavra-passe ao criar", () => {
    const semPassword = { ...valido, passwordInicial: "" };
    expect(validarCliente(semPassword, true).passwordInicial).toBe(
      "A palavra-passe inicial é obrigatória.",
    );
    expect(validarCliente(semPassword, false).passwordInicial).toBeUndefined();
  });
});

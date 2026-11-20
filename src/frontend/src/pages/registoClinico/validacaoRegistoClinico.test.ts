import { describe, expect, it } from "vitest";
import type { RegistoClinicoForm } from "../../models/registosClinicos";
import {
  MENSAGEM_SEM_ELEMENTO_CLINICO,
  assinaturaFormulario,
  formularioDeRegisto,
  formularioVazio,
  lerDuracao,
  pedidoDoFormulario,
  prescricaoVazia,
  temErros,
  tratamentoVazio,
  validarRegistoClinico,
} from "./validacaoRegistoClinico";

function com(campos: Partial<RegistoClinicoForm>): RegistoClinicoForm {
  return { ...formularioVazio(), ...campos };
}

describe("validarRegistoClinico", () => {
  it("exige pelo menos um elemento clínico (espaços não contam)", () => {
    const erros = validarRegistoClinico(com({ notasClinicas: "   " }));
    expect(erros.geral).toBe(MENSAGEM_SEM_ELEMENTO_CLINICO);
    expect(temErros(erros)).toBe(true);
  });

  it("aceita só notas ou só diagnóstico", () => {
    expect(temErros(validarRegistoClinico(com({ notasClinicas: "Ok" })))).toBe(
      false,
    );
    expect(temErros(validarRegistoClinico(com({ diagnostico: "Otite" })))).toBe(
      false,
    );
  });

  it("limita notas (4000) e diagnóstico (1000)", () => {
    const erros = validarRegistoClinico(
      com({ notasClinicas: "a".repeat(4001), diagnostico: "b".repeat(1001) }),
    );
    expect(erros.notasClinicas).toMatch(/4000/);
    expect(erros.diagnostico).toMatch(/1000/);
  });

  it("tratamento precisa de descrição e data de aplicação", () => {
    const t = tratamentoVazio();
    const erros = validarRegistoClinico(com({ tratamentos: [t] }));
    expect(erros.geral).toBeUndefined();
    expect(erros.tratamentos[t.chave]).toEqual({
      descricao: "A descrição do tratamento é obrigatória.",
      dataAplicacao: "A data de aplicação do tratamento é obrigatória.",
    });
  });

  it("prescrição precisa de medicamento, dosagem e frequência; duração opcional entre 1 e 32767", () => {
    const vazia = prescricaoVazia();
    const errosVazia = validarRegistoClinico(com({ prescricoes: [vazia] }));
    expect(Object.keys(errosVazia.prescricoes[vazia.chave])).toEqual([
      "medicamento",
      "dosagem",
      "frequencia",
    ]);

    for (const duracao of ["0", "-1", "2.5", "abc", "32768"]) {
      const p = {
        ...prescricaoVazia(),
        medicamento: "M",
        dosagem: "D",
        frequencia: "F",
        duracaoDias: duracao,
      };
      expect(
        validarRegistoClinico(com({ prescricoes: [p] })).prescricoes[p.chave]
          ?.duracaoDias,
      ).toBeDefined();
    }
    const valida = {
      ...prescricaoVazia(),
      medicamento: "M",
      dosagem: "D",
      frequencia: "F",
      duracaoDias: "7",
    };
    expect(
      temErros(validarRegistoClinico(com({ prescricoes: [valida] }))),
    ).toBe(false);
  });
});

describe("conversões do formulário", () => {
  const registo = {
    id: 9,
    consultaId: 41,
    animalId: 3,
    veterinario: "Dra. Ana",
    criadoEm: "2026-11-20T10:40:00Z",
    notasClinicas: null,
    diagnostico: "Otite",
    tratamentos: [{ id: 1, descricao: "Limpeza", dataAplicacao: "2026-11-20" }],
    prescricoes: [
      {
        id: 2,
        medicamento: "Otomax",
        dosagem: "5 gotas",
        frequencia: "2x/dia",
        duracaoDias: 7,
      },
    ],
    atualizadoEm: null,
    atualizadoPor: null,
  };

  it("formularioDeRegisto + pedidoDoFormulario mantêm os ids (a ligação ao registo)", () => {
    const pedido = pedidoDoFormulario(formularioDeRegisto(registo));
    expect(pedido).toEqual({
      notasClinicas: "",
      diagnostico: "Otite",
      tratamentos: [
        { id: 1, descricao: "Limpeza", dataAplicacao: "2026-11-20" },
      ],
      prescricoes: [
        {
          id: 2,
          medicamento: "Otomax",
          dosagem: "5 gotas",
          frequencia: "2x/dia",
          duracaoDias: 7,
        },
      ],
    });
  });

  it("lerDuracao: vazio é null, inteiro é número, resto é NaN", () => {
    expect(lerDuracao("  ")).toBeNull();
    expect(lerDuracao("14")).toBe(14);
    expect(lerDuracao("1,5")).toBeNaN();
  });

  it("assinaturaFormulario ignora as chaves locais e os espaços nas pontas", () => {
    const a = formularioDeRegisto(registo);
    const b = { ...a, diagnostico: " Otite " };
    expect(assinaturaFormulario(a)).toBe(assinaturaFormulario(b));
    expect(assinaturaFormulario(a)).not.toBe(
      assinaturaFormulario({ ...a, diagnostico: "Otite média" }),
    );
  });
});

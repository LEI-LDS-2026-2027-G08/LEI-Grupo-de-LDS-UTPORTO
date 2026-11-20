import type {
  AtualizarRegistoClinicoRequest,
  PrescricaoForm,
  RegistoClinico,
  RegistoClinicoForm,
  TratamentoForm,
} from "../../models/registosClinicos";

/**
 * Validação local do formulário de W13 (registo clínico), com as regras e as
 * mensagens da API (UpdateRegistoClinicoDto, CreateTratamentoDto,
 * CreatePrescricaoDto e ValidadorRegistoClinico). A API continua a ser quem
 * valida: isto só evita pedidos que já se sabe que vão falhar.
 */

export const MAX_NOTAS = 4000;
export const MAX_DIAGNOSTICO = 1000;
export const MAX_DESCRICAO_TRATAMENTO = 1000;
export const MAX_MEDICAMENTO = 150;
export const MAX_DOSAGEM = 100;
export const MAX_FREQUENCIA = 100;
/** A coluna da duração é smallint (CK_Prescricao_Duracao). */
export const MAX_DURACAO_DIAS = 32767;

export const MENSAGEM_SEM_ELEMENTO_CLINICO =
  "O registo clínico tem de ter pelo menos um elemento clínico: notas clínicas, diagnóstico, um tratamento ou uma prescrição.";

export type ErrosTratamento = Partial<
  Record<"descricao" | "dataAplicacao", string>
>;
export type ErrosPrescricao = Partial<
  Record<"medicamento" | "dosagem" | "frequencia" | "duracaoDias", string>
>;

export interface ErrosRegistoClinico {
  geral?: string;
  notasClinicas?: string;
  diagnostico?: string;
  /** Por chave da linha. */
  tratamentos: Record<string, ErrosTratamento>;
  prescricoes: Record<string, ErrosPrescricao>;
}

let contador = 0;
/** Chave local única para uma linha nova de tratamento ou prescrição. */
export function novaChave(prefixo: string): string {
  contador += 1;
  return `${prefixo}-${contador}`;
}

export function tratamentoVazio(dataAplicacao = ""): TratamentoForm {
  return { chave: novaChave("t"), descricao: "", dataAplicacao };
}

export function prescricaoVazia(): PrescricaoForm {
  return {
    chave: novaChave("p"),
    medicamento: "",
    dosagem: "",
    frequencia: "",
    duracaoDias: "",
  };
}

export function formularioVazio(): RegistoClinicoForm {
  return {
    notasClinicas: "",
    diagnostico: "",
    tratamentos: [],
    prescricoes: [],
  };
}

/** Preenche o formulário com um registo que já existe (modo de alteração). */
export function formularioDeRegisto(
  registo: RegistoClinico,
): RegistoClinicoForm {
  return {
    notasClinicas: registo.notasClinicas ?? "",
    diagnostico: registo.diagnostico ?? "",
    tratamentos: registo.tratamentos.map((t) => ({
      chave: `t-api-${t.id}`,
      id: t.id,
      descricao: t.descricao,
      dataAplicacao: t.dataAplicacao,
    })),
    prescricoes: registo.prescricoes.map((p) => ({
      chave: `p-api-${p.id}`,
      id: p.id,
      medicamento: p.medicamento,
      dosagem: p.dosagem,
      frequencia: p.frequencia,
      duracaoDias: p.duracaoDias === null ? "" : String(p.duracaoDias),
    })),
  };
}

/**
 * Representação estável do conteúdo do formulário (sem as chaves locais),
 * para saber se há alterações por guardar.
 */
export function assinaturaFormulario(form: RegistoClinicoForm): string {
  return JSON.stringify({
    n: form.notasClinicas.trim(),
    d: form.diagnostico.trim(),
    t: form.tratamentos.map((t) => [
      t.id ?? null,
      t.descricao.trim(),
      t.dataAplicacao,
    ]),
    p: form.prescricoes.map((p) => [
      p.id ?? null,
      p.medicamento.trim(),
      p.dosagem.trim(),
      p.frequencia.trim(),
      p.duracaoDias.trim(),
    ]),
  });
}

/** Duração escrita no campo; vazio dá null; texto inválido dá NaN. */
export function lerDuracao(texto: string): number | null {
  const limpo = texto.trim();
  if (limpo === "") return null;
  return /^\d+$/.test(limpo) ? Number(limpo) : Number.NaN;
}

function textoObrigatorio(
  valor: string,
  maximo: number,
  emFalta: string,
  demasiadoLongo: string,
): string | undefined {
  const limpo = valor.trim();
  if (limpo === "") return emFalta;
  if (limpo.length > maximo) return demasiadoLongo;
  return undefined;
}

function validarTratamento(t: TratamentoForm): ErrosTratamento {
  const erros: ErrosTratamento = {};
  const descricao = textoObrigatorio(
    t.descricao,
    MAX_DESCRICAO_TRATAMENTO,
    "A descrição do tratamento é obrigatória.",
    "A descrição do tratamento não pode ter mais de 1000 caracteres.",
  );
  if (descricao) erros.descricao = descricao;
  if (!/^\d{4}-\d{2}-\d{2}$/.test(t.dataAplicacao)) {
    erros.dataAplicacao = "A data de aplicação do tratamento é obrigatória.";
  }
  return erros;
}

function validarPrescricao(p: PrescricaoForm): ErrosPrescricao {
  const erros: ErrosPrescricao = {};
  const medicamento = textoObrigatorio(
    p.medicamento,
    MAX_MEDICAMENTO,
    "O medicamento da prescrição é obrigatório.",
    "O medicamento não pode ter mais de 150 caracteres.",
  );
  if (medicamento) erros.medicamento = medicamento;
  const dosagem = textoObrigatorio(
    p.dosagem,
    MAX_DOSAGEM,
    "A dosagem da prescrição é obrigatória.",
    "A dosagem não pode ter mais de 100 caracteres.",
  );
  if (dosagem) erros.dosagem = dosagem;
  const frequencia = textoObrigatorio(
    p.frequencia,
    MAX_FREQUENCIA,
    "A frequência da prescrição é obrigatória.",
    "A frequência não pode ter mais de 100 caracteres.",
  );
  if (frequencia) erros.frequencia = frequencia;
  const duracao = lerDuracao(p.duracaoDias);
  if (
    duracao !== null &&
    (Number.isNaN(duracao) || duracao < 1 || duracao > MAX_DURACAO_DIAS)
  ) {
    erros.duracaoDias =
      "A duração da prescrição, quando indicada, deve ser um número inteiro de dias entre 1 e 32767.";
  }
  return erros;
}

export function validarRegistoClinico(
  form: RegistoClinicoForm,
): ErrosRegistoClinico {
  const erros: ErrosRegistoClinico = { tratamentos: {}, prescricoes: {} };
  const notas = form.notasClinicas.trim();
  const diagnostico = form.diagnostico.trim();

  if (notas.length > MAX_NOTAS) {
    erros.notasClinicas =
      "As notas clínicas não podem ter mais de 4000 caracteres.";
  }
  if (diagnostico.length > MAX_DIAGNOSTICO) {
    erros.diagnostico = "O diagnóstico não pode ter mais de 1000 caracteres.";
  }
  for (const t of form.tratamentos) {
    const e = validarTratamento(t);
    if (Object.keys(e).length > 0) erros.tratamentos[t.chave] = e;
  }
  for (const p of form.prescricoes) {
    const e = validarPrescricao(p);
    if (Object.keys(e).length > 0) erros.prescricoes[p.chave] = e;
  }
  if (
    notas === "" &&
    diagnostico === "" &&
    form.tratamentos.length === 0 &&
    form.prescricoes.length === 0
  ) {
    erros.geral = MENSAGEM_SEM_ELEMENTO_CLINICO;
  }
  return erros;
}

export function temErros(erros: ErrosRegistoClinico): boolean {
  return (
    erros.geral !== undefined ||
    erros.notasClinicas !== undefined ||
    erros.diagnostico !== undefined ||
    Object.keys(erros.tratamentos).length > 0 ||
    Object.keys(erros.prescricoes).length > 0
  );
}

/** Converte o formulário (já validado) no corpo de EP-031/EP-032. */
export function pedidoDoFormulario(
  form: RegistoClinicoForm,
): AtualizarRegistoClinicoRequest {
  return {
    notasClinicas: form.notasClinicas,
    diagnostico: form.diagnostico,
    tratamentos: form.tratamentos.map((t) => ({
      ...(t.id ? { id: t.id } : {}),
      descricao: t.descricao,
      dataAplicacao: t.dataAplicacao,
    })),
    prescricoes: form.prescricoes.map((p) => ({
      ...(p.id ? { id: p.id } : {}),
      medicamento: p.medicamento,
      dosagem: p.dosagem,
      frequencia: p.frequencia,
      duracaoDias: lerDuracao(p.duracaoDias),
    })),
  };
}

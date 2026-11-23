import { randomInt } from "node:crypto";

/**
 * Dados gerados por cada cenário (I128, secção 5.4). Tudo o que um cenário
 * cria leva o seu {sufixo}, para nunca coincidir com outro cenário ou execução.
 */

/** {sufixo}: identificador do cenário + marca temporal (ex.: F001-07-1763290000). */
export function sufixo(cenario: string): string {
  return `${cenario}-${Math.floor(Date.now() / 1000)}${randomInt(10, 100)}`;
}

/** E-mail criado pelo cenário: st.{sufixo}@clinica-g08.test, em minúsculas. */
export function emailDe(suf: string): string {
  return `st.${suf}@clinica-g08.test`.toLowerCase();
}

/** Telefone criado: 93 seguido de 7 algarismos. */
export function telefoneNovo(): string {
  const algarismos = (Date.now() % 1e7) + randomInt(0, 1e7);
  return `93${String(algarismos % 1e7).padStart(7, "0")}`;
}

/** NIF válido (9 algarismos, dígito de controlo correto), começado por 2. */
export function nifValido(): string {
  const base = `2${String(randomInt(0, 1e7)).padStart(7, "0")}`;
  const soma = [...base].reduce((s, d, i) => s + Number(d) * (9 - i), 0);
  const resto = soma % 11;
  return `${base}${resto < 2 ? 0 : 11 - resto}`;
}

/**
 * Palavra-passe gerada na execução: 12 caracteres com minúscula, maiúscula,
 * algarismo e símbolo (EP-006 e EP-013). Só existe em memória.
 */
export function senhaGerada(): string {
  const grupos = [
    "abcdefghijkmnpqrstuvwxyz",
    "ABCDEFGHJKLMNPQRSTUVWXYZ",
    "23456789",
    "#$%&*+-=?@",
  ];
  const todos = grupos.join("");
  const escolher = (texto: string) => texto[randomInt(0, texto.length)];
  const caracteres = grupos.map(escolher);
  while (caracteres.length < 12) caracteres.push(escolher(todos));
  for (let i = caracteres.length - 1; i > 0; i--) {
    const j = randomInt(0, i + 1);
    [caracteres[i], caracteres[j]] = [caracteres[j], caracteres[i]];
  }
  return caracteres.join("");
}

/** Data (aaaa-mm-dd) em Lisboa, com um desvio em dias a partir de hoje. */
export function dataLisboa(desvioDias = 0): string {
  const instante = new Date(Date.now() + desvioDias * 86_400_000);
  return new Intl.DateTimeFormat("en-CA", {
    timeZone: "Europe/Lisbon",
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  }).format(instante);
}

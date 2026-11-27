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

/** Soma (ou subtrai) dias a uma data aaaa-mm-dd. */
export function somarDias(data: string, dias: number): string {
  const [ano, mes, dia] = data.split("-").map(Number);
  return new Date(Date.UTC(ano, mes - 1, dia + dias))
    .toISOString()
    .slice(0, 10);
}

/** Dia da semana de uma data aaaa-mm-dd (0 = domingo … 6 = sábado). */
export function diaDaSemana(data: string): number {
  const [ano, mes, dia] = data.split("-").map(Number);
  return new Date(Date.UTC(ano, mes - 1, dia)).getUTCDay();
}

/**
 * {D} (I128, secção 5.4) — dia útil (segunda a sexta) das marcações de um
 * cenário, com o desvio N indicado nos seus dados: o N.º dia útil a seguir a
 * hoje, em Lisboa. Fica sempre a pelo menos N dias de hoje e, ao contrário do
 * «primeiro dia útil a pelo menos N dias», dá dias diferentes a desvios
 * diferentes também perto do fim de semana (a partir de uma sexta-feira, os
 * desvios 8, 9 e 10 dariam todos a mesma segunda-feira). Assim, dois cenários
 * a correr em paralelo nunca disputam a mesma vaga do mesmo Veterinário.
 */
export function diaDeMarcacao(desvio: number): string {
  let dia = dataLisboa(0);
  let uteis = 0;
  while (uteis < desvio) {
    dia = somarDias(dia, 1);
    const semana = diaDaSemana(dia);
    if (semana >= 1 && semana <= 5) uteis += 1;
  }
  return dia;
}

/** Data aaaa-mm-dd como a aplicação a mostra: dd/mm/aaaa. */
export function dataPt(data: string): string {
  const [ano, mes, dia] = data.split("-");
  return `${dia}/${mes}/${ano}`;
}

/** Hora atual de Lisboa (HH:mm). */
export function horaLisboa(instante: Date = new Date()): string {
  return new Intl.DateTimeFormat("en-GB", {
    timeZone: "Europe/Lisbon",
    hour: "2-digit",
    minute: "2-digit",
    hourCycle: "h23",
  }).format(instante);
}

/** Texto para usar dentro de uma expressão regular, sem significado especial. */
export function literal(texto: string): string {
  return texto.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

/**
 * Conversões entre a hora de Lisboa e UTC. A API guarda e devolve as
 * marcações em UTC, mas a disponibilidade (EP-022) e os filtros da agenda
 * usam dias e horas de Lisboa (Documentação de API, §2 e EP-022).
 */

export const FUSO_CLINICA = "Europe/Lisbon";

interface PartesData {
  ano: number;
  mes: number;
  dia: number;
  hora: number;
  minuto: number;
}

function partesEmLisboa(instante: Date): PartesData {
  const partes = new Intl.DateTimeFormat("en-GB", {
    timeZone: FUSO_CLINICA,
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
    hourCycle: "h23",
  }).formatToParts(instante);
  const valor = (tipo: string) =>
    Number(partes.find((p) => p.type === tipo)?.value ?? 0);
  return {
    ano: valor("year"),
    mes: valor("month"),
    dia: valor("day"),
    hora: valor("hour"),
    minuto: valor("minute"),
  };
}

const doisDigitos = (n: number) => String(n).padStart(2, "0");

/**
 * Dia (aaaa-mm-dd) e hora (HH:mm) de Lisboa → instante UTC em ISO 8601
 * (ex.: «2026-07-14» e «09:30» → «2026-07-14T08:30:00.000Z»).
 */
export function lisboaParaUtc(data: string, hora: string): string {
  const [ano, mes, dia] = data.split("-").map(Number);
  const [h, m] = hora.split(":").map(Number);
  const pretendido = Date.UTC(ano, mes - 1, dia, h, m);
  // Primeira aproximação: tratar a hora de Lisboa como se fosse UTC e
  // corrigir pela diferença observada (0 ou 1 hora, consoante a época).
  let instante = pretendido;
  for (let i = 0; i < 2; i++) {
    const p = partesEmLisboa(new Date(instante));
    const visto = Date.UTC(p.ano, p.mes - 1, p.dia, p.hora, p.minuto);
    instante += pretendido - visto;
  }
  return new Date(instante).toISOString();
}

/** Instante ISO 8601 → dia (aaaa-mm-dd) e hora (HH:mm) de Lisboa. */
export function utcParaLisboa(iso: string): { data: string; hora: string } {
  const p = partesEmLisboa(new Date(iso));
  return {
    data: `${p.ano}-${doisDigitos(p.mes)}-${doisDigitos(p.dia)}`,
    hora: `${doisDigitos(p.hora)}:${doisDigitos(p.minuto)}`,
  };
}

/** Dia de hoje em Lisboa (aaaa-mm-dd). */
export function hojeEmLisboa(agora: Date = new Date()): string {
  return utcParaLisboa(agora.toISOString()).data;
}

/** Soma dias a uma data aaaa-mm-dd. */
export function somarDias(data: string, dias: number): string {
  const [ano, mes, dia] = data.split("-").map(Number);
  const resultado = new Date(Date.UTC(ano, mes - 1, dia + dias));
  return resultado.toISOString().slice(0, 10);
}

/** Data aaaa-mm-dd → dia da semana e data (ex.: «sábado, 14/11/2026»). */
export function formatarDiaCurto(data: string): string {
  const [ano, mes, dia] = data.split("-").map(Number);
  return new Date(Date.UTC(ano, mes - 1, dia)).toLocaleDateString("pt-PT", {
    timeZone: "UTC",
    weekday: "short",
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  });
}

/** Instante ISO 8601 → «14/11/2026, 09:30» em hora de Lisboa. */
export function formatarDataHoraLisboa(iso: string | null): string {
  if (!iso) return "—";
  const { data, hora } = utcParaLisboa(iso);
  const [ano, mes, dia] = data.split("-");
  return `${dia}/${mes}/${ano}, ${hora}`;
}

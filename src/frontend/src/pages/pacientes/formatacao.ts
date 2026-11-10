/** Peso para mostrar (ex.: 28.4 → «28,4 kg»); sem valor → «—». */
export function formatarPeso(peso: number | null): string {
  if (peso === null) return "—";
  return `${peso.toLocaleString("pt-PT", { maximumFractionDigits: 2 })} kg`;
}

/** Idade em anos para mostrar (ex.: 1 → «1 ano», 4 → «4 anos»). */
export function formatarIdade(idadeAnos: number | null): string {
  if (idadeAnos === null) return "—";
  return idadeAnos === 1 ? "1 ano" : `${idadeAnos} anos`;
}

/** Data aaaa-mm-dd para mostrar como dd/mm/aaaa. */
export function formatarData(iso: string | null): string {
  if (!iso) return "—";
  const [ano, mes, dia] = iso.slice(0, 10).split("-");
  return dia && mes && ano ? `${dia}/${mes}/${ano}` : iso;
}

/** Data e hora (ISO 8601) para mostrar em pt-PT. */
export function formatarDataHora(iso: string | null): string {
  if (!iso) return "—";
  const data = new Date(iso);
  return Number.isNaN(data.getTime())
    ? iso
    : data.toLocaleString("pt-PT", { dateStyle: "short", timeStyle: "short" });
}

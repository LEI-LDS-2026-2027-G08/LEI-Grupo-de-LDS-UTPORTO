function ultimoDomingo(ano, mes) {
  const dia = new Date(Date.UTC(ano, mes + 1, 0, 1));
  dia.setUTCDate(dia.getUTCDate() - dia.getUTCDay());
  return dia;
}

// R-FUSO. A mudança de hora em Lisboa ocorre às 01:00 UTC.
const agora = new Date();
const ano = agora.getUTCFullYear();
const emHorarioDeVerao =
  agora >= ultimoDomingo(ano, 2) && agora < ultimoDomingo(ano, 9);
const hojeLisboa = emHorarioDeVerao
  ? new Date(agora.getTime() + 3600000)
  : agora;

function dataComDeslocamento(dias) {
  const data = new Date(hojeLisboa.getTime());
  data.setUTCDate(data.getUTCDate() + dias);
  return data.toISOString().slice(0, 10);
}

request.variables.set("vacDataPassada", dataComDeslocamento(-30));
request.variables.set("vacDataAtualizacao", dataComDeslocamento(-10));
request.variables.set("vacDataFutura", dataComDeslocamento(2));

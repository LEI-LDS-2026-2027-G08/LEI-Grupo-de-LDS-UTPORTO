/** Dias da semana como a API os envia e recebe (Documentação de API, secção 2.2). */
export const DIAS_SEMANA = [
  "Segunda",
  "Terca",
  "Quarta",
  "Quinta",
  "Sexta",
  "Sabado",
  "Domingo",
] as const;
export type DiaSemana = (typeof DIAS_SEMANA)[number];

/** Nome de cada dia para mostrar ao utilizador. */
export const ROTULO_DIA: Record<DiaSemana, string> = {
  Segunda: "Segunda",
  Terca: "Terça",
  Quarta: "Quarta",
  Quinta: "Quinta",
  Sexta: "Sexta",
  Sabado: "Sábado",
  Domingo: "Domingo",
};

/** Período de funcionamento de um dia (EP-009 e EP-010); horas em HH:mm, hora de Lisboa. */
export interface HorarioFuncionamento {
  diaSemana: DiaSemana;
  abertura: string;
  fecho: string;
}

/**
 * Dados gerais, configuração e horário da clínica (EP-009 e EP-010).
 * Um dia sem períodos está encerrado; o mesmo dia pode ter vários períodos.
 */
export interface ClinicaConfig {
  nome: string;
  nif: string;
  morada: string;
  telefone: string;
  email: string;
  duracaoConsultaMinutos: number;
  horarioFuncionamento: HorarioFuncionamento[];
}

/** Limites da duração de uma consulta (API v03, secção 5). */
export const DURACAO_MINIMA_MINUTOS = 5;
export const DURACAO_MAXIMA_MINUTOS = 240;

/** Período no formulário: `chave` só serve para a lista do React; as horas podem estar vazias. */
export interface PeriodoFormulario {
  chave: number;
  diaSemana: DiaSemana;
  abertura: string;
  fecho: string;
}

const HORA = /^([01]\d|2[0-3]):[0-5]\d$/;

/**
 * Regras do horário de funcionamento (RF019), as mesmas que a API aplica em
 * EP-010: horas válidas, fecho posterior à abertura e períodos do mesmo dia
 * sem sobreposição (períodos seguidos, como 13:00–14:00 e 14:00–19:00, são
 * aceites). Devolve uma mensagem por problema; lista vazia = horário válido.
 */
export function validarHorario(
  periodos: readonly PeriodoFormulario[],
): string[] {
  const erros: string[] = [];
  for (const dia of DIAS_SEMANA) {
    const validos: PeriodoFormulario[] = [];
    periodos
      .filter((p) => p.diaSemana === dia)
      .forEach((p, indice) => {
        const local = `${ROTULO_DIA[dia]}, período ${indice + 1}`;
        if (!HORA.test(p.abertura) || !HORA.test(p.fecho)) {
          erros.push(`${local}: indique a hora de abertura e a de fecho.`);
        } else if (p.fecho <= p.abertura) {
          erros.push(`${local}: o fecho tem de ser posterior à abertura.`);
        } else {
          validos.push(p);
        }
      });
    const ordenados = [...validos].sort((a, b) =>
      a.abertura.localeCompare(b.abertura),
    );
    for (let i = 1; i < ordenados.length; i++) {
      const anterior = ordenados[i - 1];
      const atual = ordenados[i];
      if (atual.abertura < anterior.fecho) {
        erros.push(
          `${ROTULO_DIA[dia]}: os períodos ${anterior.abertura}–${anterior.fecho} e ${atual.abertura}–${atual.fecho} sobrepõem-se.`,
        );
      }
    }
  }
  return erros;
}

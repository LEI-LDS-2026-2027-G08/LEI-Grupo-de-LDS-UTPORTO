/**
 * Tipos dos pacientes (EP-015 a EP-018), iguais aos DTOs da API
 * (AnimalListItemDto, AnimalDto, CreateAnimalDto e UpdateAnimalDto).
 * Só dados administrativos: a informação clínica é de outras tarefas (F004).
 */

export const SEXOS = ["Macho", "Femea", "Desconhecido"] as const;
export type Sexo = (typeof SEXOS)[number];

export const ROTULO_SEXO: Record<Sexo, string> = {
  Macho: "Macho",
  Femea: "Fêmea",
  Desconhecido: "Desconhecido",
};

/**
 * Espécies e raças registadas na base de dados (seed do backend). A API não
 * tem endpoint para as listar e recusa (400) espécies ou raças que não
 * existam, por isso a interface sugere estas, mas aceita outras.
 */
export const RACAS_POR_ESPECIE: Readonly<Record<string, readonly string[]>> = {
  Cão: ["Labrador Retriever", "Pastor Alemão", "Sem raça definida"],
  Gato: ["Europeu Comum", "Siamês"],
};
export const ESPECIES = Object.keys(RACAS_POR_ESPECIE);

/** Proprietário, como vem na lista (EP-015). */
export interface ProprietarioResumo {
  id: number;
  nome: string;
}

/** Proprietário, como vem na ficha (EP-016). */
export interface ClientePaciente extends ProprietarioResumo {
  telefone: string | null;
}

/** Linha da lista de pacientes (EP-015). */
export interface PacienteResumo {
  id: number;
  nome: string;
  especie: string;
  raca: string | null;
  idadeAnos: number | null;
  peso: number | null;
  cliente: ProprietarioResumo;
}

/** Ficha administrativa do paciente (EP-016, e resposta de EP-017 e EP-018). */
export interface PacienteDetalhe {
  id: number;
  nome: string;
  especie: string;
  raca: string | null;
  sexo: Sexo;
  /** Data no formato aaaa-mm-dd. */
  dataNascimento: string | null;
  idadeAnos: number | null;
  peso: number | null;
  cliente: ClientePaciente;
  atualizadoEm: string | null;
}

/** Valores do formulário de W08 (texto, como nos inputs). */
export interface PacienteFormState {
  nome: string;
  especie: string;
  raca: string;
  sexo: Sexo;
  dataNascimento: string;
  peso: string;
  proprietario: ProprietarioResumo | null;
}

/** Corpo de EP-017 (POST /animais) e EP-018 (PUT /animais/{id}). */
export interface PacienteRequest {
  nome: string;
  especie: string;
  raca: string | null;
  sexo: Sexo;
  dataNascimento: string | null;
  peso: number | null;
  clienteId: number;
}

/** Parâmetros de EP-015. */
export interface PacienteFiltros {
  /** Proprietário (Cliente) escolhido. */
  clienteId?: number;
  /** Texto procurado no nome do animal ou do proprietário. */
  pesquisa?: string;
  especie?: string;
  pagina: number;
  tamanho: number;
}

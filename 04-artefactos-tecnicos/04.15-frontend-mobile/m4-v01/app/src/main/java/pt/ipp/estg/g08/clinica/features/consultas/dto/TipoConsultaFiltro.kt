package pt.ipp.estg.g08.clinica.features.consultas.dto

/**
 * Parâmetro `tipo` de EP-030 (secção 9.1 do Modelo de Classes Mobile): [valorApi] é o texto
 * enviado à API. A regra que decide o que é futuro e o que é histórico é só do servidor.
 */
enum class TipoConsultaFiltro(val valorApi: String) {
    /** Marcações Pendentes ou Confirmadas que ainda não começaram, por data crescente. */
    Futuras("futuras"),

    /** Marcações passadas e as Realizadas ou Canceladas, por data decrescente. */
    Historico("historico"),
}

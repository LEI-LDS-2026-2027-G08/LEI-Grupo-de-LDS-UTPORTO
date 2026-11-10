package pt.ipp.estg.g08.clinica.sessao

/** Porque é que a sessão local terminou. */
enum class MotivoFimSessao {
    /** A API respondeu 401 a um pedido autenticado (token expirado, revogado ou inválido). */
    NaoAutenticado,

    /** O utilizador terminou a sessão. */
    Utilizador,
}

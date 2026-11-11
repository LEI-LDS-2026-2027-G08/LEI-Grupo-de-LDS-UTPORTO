package pt.ipp.estg.g08.clinica.sessao

/** Estado da sessão (secção 6.3 do Modelo de Classes Mobile). */
enum class EstadoSessao {
    /** Arranque: a sessão guardada ainda está a ser confirmada (EP-003). */
    Desconhecida,

    /** Sem sessão: mostra o início de sessão (M01). */
    NaoAutenticada,

    /** Sessão de Cliente válida: mostra a área autenticada. */
    Autenticada,
}

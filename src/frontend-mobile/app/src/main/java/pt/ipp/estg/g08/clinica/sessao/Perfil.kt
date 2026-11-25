package pt.ipp.estg.g08.clinica.sessao

import kotlinx.serialization.Serializable

/** Perfis da API. Os valores coincidem com os da API (secção 2.2 da Documentação da API). */
@Serializable
enum class Perfil {
    Administrador,
    Rececionista,
    Veterinario,
    AuxiliarVeterinario,
    Cliente,
}

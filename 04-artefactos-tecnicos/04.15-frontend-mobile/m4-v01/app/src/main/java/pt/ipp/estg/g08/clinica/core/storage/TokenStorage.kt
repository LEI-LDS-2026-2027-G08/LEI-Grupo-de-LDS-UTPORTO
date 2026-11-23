package pt.ipp.estg.g08.clinica.core.storage

import pt.ipp.estg.g08.clinica.sessao.SessaoAutenticada

/** Guarda, lê e apaga a sessão do Cliente. O token só pode ser guardado através desta interface. */
interface TokenStorage {
    suspend fun guardar(sessao: SessaoAutenticada)

    /** A sessão guardada, ou `null` se não existe ou não pôde ser lida (nesse caso é apagada). */
    suspend fun obter(): SessaoAutenticada?

    suspend fun limpar()
}

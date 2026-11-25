package pt.ipp.estg.g08.clinica.core.storage

import android.content.Context
import androidx.core.content.edit

/** Armazenamento simples de texto por chave. Separa a lógica do `TokenStorage` do Android. */
interface ArmazenamentoChaveValor {
    fun ler(chave: String): String?

    fun escrever(chave: String, valor: String)

    fun apagar(chave: String)
}

/** Implementação sobre `SharedPreferences` privadas da aplicação. */
class SharedPreferencesArmazenamento(contexto: Context, nomeFicheiro: String) :
    ArmazenamentoChaveValor {
    private val preferencias =
        contexto.applicationContext.getSharedPreferences(nomeFicheiro, Context.MODE_PRIVATE)

    override fun ler(chave: String): String? = preferencias.getString(chave, null)

    override fun escrever(chave: String, valor: String) {
        preferencias.edit { putString(chave, valor) }
    }

    override fun apagar(chave: String) {
        preferencias.edit { remove(chave) }
    }
}

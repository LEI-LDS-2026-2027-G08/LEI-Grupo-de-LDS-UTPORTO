package pt.ipp.estg.g08.clinica.navegacao

import org.junit.Assert.assertEquals
import org.junit.Test

class RotasTest {
    @Test
    fun `rotas com argumentos`() {
        assertEquals("animais/12", Rotas.animalDetalhe(12))
        assertEquals("boletim/12", Rotas.boletim(12))
    }

    @Test
    fun `consultas com e sem filtro por animal`() {
        assertEquals("consultas", Rotas.consultas())
        assertEquals("consultas?animalId=5", Rotas.consultas(5))
    }

    @Test
    fun `os padroes de rota usam o nome do argumento`() {
        assertEquals("animais/{animalId}", Rotas.ANIMAL_DETALHE)
        assertEquals("consultas?animalId={animalId}", Rotas.CONSULTAS)
        assertEquals("boletim/{animalId}", Rotas.BOLETIM)
    }
}

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
    fun `consultas de todos os animais e so de um animal`() {
        assertEquals("consultas", Rotas.CONSULTAS)
        assertEquals("animais/5/consultas", Rotas.consultasDoAnimal(5))
    }

    @Test
    fun `os padroes de rota usam o nome do argumento`() {
        assertEquals("animais/{animalId}", Rotas.ANIMAL_DETALHE)
        assertEquals("animais/{animalId}/consultas", Rotas.CONSULTAS_ANIMAL)
        assertEquals("boletim/{animalId}", Rotas.BOLETIM)
    }
}

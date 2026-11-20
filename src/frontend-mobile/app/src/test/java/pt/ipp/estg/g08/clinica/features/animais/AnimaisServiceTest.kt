package pt.ipp.estg.g08.clinica.features.animais

import java.time.LocalDate
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.test.runTest
import org.junit.Assert.assertEquals
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test
import pt.ipp.estg.g08.clinica.core.api.ApiClient
import pt.ipp.estg.g08.clinica.core.api.ApiException
import pt.ipp.estg.g08.clinica.core.api.ERRO_403
import pt.ipp.estg.g08.clinica.core.api.ServidorFalso
import pt.ipp.estg.g08.clinica.core.api.esperarErro
import pt.ipp.estg.g08.clinica.core.api.respostaJson
import pt.ipp.estg.g08.clinica.core.config.AppConfig
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalDto
import pt.ipp.estg.g08.clinica.features.animais.dto.MeuAnimalListItemDto
import pt.ipp.estg.g08.clinica.features.animais.dto.Sexo

/** Exemplo de resposta de EP-019 da Documentação da API v03. */
private const val LISTA =
    """[{"id":3,"nome":"Bobi","especie":"Cão","raca":"Labrador","idadeAnos":4,"peso":29.1},""" +
        """{"id":8,"nome":"Mia","especie":"Gato","raca":"Europeu Comum","idadeAnos":2,"peso":4.2}]"""

/** Exemplo de resposta de EP-020 da Documentação da API v03. */
private const val ANIMAL =
    """{"id":3,"nome":"Bobi","especie":"Cão","raca":"Labrador","sexo":"Macho",""" +
        """"dataNascimento":"2022-05-10","idadeAnos":4,"peso":29.1}"""

private const val ANIMAL_SEM_DADOS_OPCIONAIS =
    """{"id":9,"nome":"Tico","especie":"Ave","raca":null,"sexo":"Desconhecido",""" +
        """"dataNascimento":null,"idadeAnos":null,"peso":null}"""

private const val ERRO_404 =
    """{"code":404,"message":"Recurso não encontrado.","detail":"Animal não encontrado."}"""

/** EP-019 e EP-020 com o `ApiClient` real e um servidor falso (sem rede). */
class AnimaisServiceTest {
    private fun api(servidor: ServidorFalso) = ApiClient(
        AppConfig(apiBaseUrl = "http://localhost:5159"),
        httpClient = servidor.cliente,
        dispatcher = Dispatchers.Unconfined,
    )

    @Test
    fun `listar pede EP-019 com o token e sem identificar o Cliente`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, LISTA) }
        val api = api(servidor)
        api.definirToken("token-cliente")

        val animais = AnimaisService(api).listar()

        val pedido = servidor.pedidos.single()
        assertEquals("GET", pedido.method)
        assertEquals("http://localhost:5159/me/animais", pedido.url.toString())
        assertEquals("Bearer token-cliente", pedido.header("Authorization"))
        assertEquals(
            listOf(
                MeuAnimalListItemDto(3, "Bobi", "Cão", "Labrador", 4, 29.1),
                MeuAnimalListItemDto(8, "Mia", "Gato", "Europeu Comum", 2, 4.2),
            ),
            animais,
        )
    }

    @Test
    fun `listar devolve lista vazia quando o Cliente nao tem animais`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, "[]") }

        assertTrue(AnimaisService(api(servidor)).listar().isEmpty())
    }

    @Test
    fun `obter pede EP-020 e le os dados basicos do animal`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, ANIMAL) }

        val animal = AnimaisService(api(servidor)).obter(3)

        assertEquals("http://localhost:5159/me/animais/3", servidor.pedidos.single().url.toString())
        val esperado =
            MeuAnimalDto(
                id = 3,
                nome = "Bobi",
                especie = "Cão",
                raca = "Labrador",
                sexo = Sexo.Macho,
                dataNascimento = LocalDate.of(2022, 5, 10),
                idadeAnos = 4,
                peso = 29.1,
            )
        assertEquals(esperado, animal)
    }

    @Test
    fun `obter aceita os campos opcionais a null`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 200, ANIMAL_SEM_DADOS_OPCIONAIS) }

        val animal = AnimaisService(api(servidor)).obter(9)

        assertEquals(Sexo.Desconhecido, animal.sexo)
        assertNull(animal.raca)
        assertNull(animal.dataNascimento)
        assertNull(animal.idadeAnos)
        assertNull(animal.peso)
    }

    @Test
    fun `animal de outro Cliente da 403 com mensagem de acesso recusado`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 403, ERRO_403) }

        val erro = esperarErro<ApiException> { AnimaisService(api(servidor)).obter(99) }

        assertEquals(403, erro.status)
        assertEquals("Não tem acesso a esta informação.", erro.mensagemParaUtilizador())
    }

    @Test
    fun `animal inexistente da 404`() = runTest {
        val servidor = ServidorFalso { respostaJson(it, 404, ERRO_404) }

        val erro = esperarErro<ApiException> { AnimaisService(api(servidor)).obter(77) }

        assertEquals(404, erro.status)
        assertEquals("A informação pedida já não existe.", erro.mensagemParaUtilizador())
    }
}

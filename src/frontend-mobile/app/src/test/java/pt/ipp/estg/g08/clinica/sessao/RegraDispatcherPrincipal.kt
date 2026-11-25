package pt.ipp.estg.g08.clinica.sessao

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.ExperimentalCoroutinesApi
import kotlinx.coroutines.test.TestDispatcher
import kotlinx.coroutines.test.UnconfinedTestDispatcher
import kotlinx.coroutines.test.resetMain
import kotlinx.coroutines.test.setMain
import org.junit.rules.TestWatcher
import org.junit.runner.Description

/**
 * Substitui `Dispatchers.Main` (usado pelo `viewModelScope`) por um dispatcher de teste.
 * Os testes usam `runTest(regra.dispatcher)` para partilhar o mesmo relógio virtual.
 */
@OptIn(ExperimentalCoroutinesApi::class)
class RegraDispatcherPrincipal(val dispatcher: TestDispatcher = UnconfinedTestDispatcher()) :
    TestWatcher() {
    override fun starting(description: Description) {
        Dispatchers.setMain(dispatcher)
    }

    override fun finished(description: Description) {
        Dispatchers.resetMain()
    }
}

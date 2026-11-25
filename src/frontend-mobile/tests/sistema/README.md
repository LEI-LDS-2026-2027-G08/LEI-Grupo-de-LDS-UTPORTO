# I137 — Testes de sistema instrumentados

Automação dos **18 cenários atribuídos à I137** na secção 13.1 da [Especificação I136](../../../../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema/m4-especificacao-testes-sistema-frontend-mobile-v01.md). Os restantes 12 pertencem à I138. Despesas/I124 estão fora do âmbito.

## Executar localmente

Pré-requisitos: Python 3.11 ou superior, Docker ligado com contentores Linux, .NET 10, JDK 21, Android SDK 37, Platform-Tools, Android Emulator e um AVD API 37 instalado. `docker`, `dotnet` e o JDK têm de estar disponíveis no terminal. As portas locais **5159 e 5160 têm de estar livres**. O script recusa portas ocupadas; não termina serviços existentes.

O script respeita `ANDROID_HOME` ou, na sua ausência, `ANDROID_SDK_ROOT`, e transmite a pasta encontrada ao Gradle. Sem configuração explícita, procura o SDK em `~/Library/Android/sdk` no macOS, `%LOCALAPPDATA%\Android\Sdk` no Windows e `~/Android/Sdk` no Linux.

### Windows — PowerShell

1. Instalar Python 3.11 ou superior com o launcher `py`, .NET 10, JDK 21 e Android Studio. No SDK Manager, instalar os componentes Android acima; no Device Manager, criar um AVD API 37.
2. Abrir o Docker Desktop em modo de contentores Linux e aguardar que esteja pronto.
3. No PowerShell, entrar na pasta `src\frontend-mobile` do clone e executar:

```powershell
py -3 -m pip install tzdata
$env:ANDROID_HOME = "$env:LOCALAPPDATA\Android\Sdk"
& "$env:ANDROID_HOME\emulator\emulator.exe" -list-avds
```

Se o SDK estiver noutra pasta, usar essa pasta em `ANDROID_HOME`. O pacote `tzdata` fornece os dados de `Europe/Lisbon` que normalmente não existem no Windows. É verificado antes de criar API, emulador ou contentor; se faltar, o script indica como o instalar com o mesmo Python que executa os testes.

Se o AVD tiver o nome predefinido `Medium_Phone_API_37.0`:

```powershell
py -3 tests\sistema\executar.py
```

Para outro AVD, acrescentar `--avd "NOME_DO_AVD"`, usando um nome devolvido por `-list-avds`. Para um emulador já iniciado, usar `--serial emulator-5554` com o identificador correspondente.

Se o Gradle não encontrar o JDK, definir `$env:JAVA_HOME` com a pasta da instalação do JDK 21. O script escolhe `gradlew.bat`, `adb.exe` e `emulator.exe` no Windows. Ao terminar, usa `taskkill /PID ... /T /F` apenas para as árvores dos processos que iniciou. Uma falha numa etapa de limpeza não impede as restantes; fica registada em `erros_limpeza` e faz a execução falhar.

### macOS e Linux

Na pasta `src/frontend-mobile`:

```sh
python3 tests/sistema/executar.py
```

No macOS, se o terminal não encontrar o JDK, pode indicar-se a instalação usada pelo Android Studio:

```sh
JAVA_HOME="/Applications/Android Studio.app/Contents/jbr/Contents/Home" python3 tests/sistema/executar.py
```

No Linux, definir `ANDROID_HOME` se o SDK não estiver em `~/Android/Sdk`. Se faltarem dados de fuso horário no sistema, instalar `tzdata` com `python3 -m pip install tzdata`. No macOS e Linux, a limpeza termina os grupos dos processos criados pelo script.

### Opções comuns e resultados

Por omissão, inicia o AVD `Medium_Phone_API_37.0` sem janela, na porta 5580, executa a suite duas vezes e fecha os recursos que criou. Para outro AVD, acrescentar `--avd NOME`. Não abrir o mesmo AVD no Android Studio durante a execução.

Com um emulador já iniciado:

```sh
python3 tests/sistema/executar.py --serial emulator-5554
```

O script deixa esse emulador aberto. Os túneis das portas de teste são removidos no fim. O fuso horário tem de ser `Europe/Lisbon`; as animações são desligadas.

Para diagnosticar só um cenário, sem apresentar esse ensaio como aprovação da suite:

```sh
python3 tests/sistema/executar.py --execucoes 1 --case ST-MOB-F001-01
```

As evidências ficam em `tests/sistema/resultados/<execução>/`: resultado JSON, registos, JUnit de cada execução e capturas quando há falhas. Esta pasta está excluída do Git. O script exige **18 testes aprovados, zero falhas, erros ou ignorados** em cada execução completa; um conjunto vazio nunca passa.

### Testes de regressão do orquestrador

Na raiz do repositório:

```sh
python3 -m unittest discover -s src/frontend-mobile/tests/sistema -p "test_*.py" -v
```

No PowerShell, substituir `python3` por `py -3`. Estes testes verificam descoberta do SDK, escolha do wrapper, falta de `tzdata`, terminação dos processos, continuação da limpeza após uma falha e atualização segura das fontes arquivadas. Usam processos simulados e não substituem as duas execuções dos 18 cenários no emulador nem uma execução real em Windows.

## Isolamento e preparação

- É criado um contentor SQL Server 2025 exclusivo, com porta atribuída pelo Docker, nome único e credencial temporária. A base habitual do utilizador não é usada.
- A API real arranca na porta 5159, com uma ligação exclusiva a esse contentor e migrações aplicadas. Não usa o Azure.
- Os dados de demonstração da I061 são carregados e as credenciais das contas necessárias são substituídas, apenas na base descartável, por valores gerados para a execução.
- Antes de **cada teste**, o serviço de preparação local repõe a base a partir de uma cópia limpa e cria PD-01 a PD-04 **pela API real**: duas consultas futuras, Cliente sem animais e vacinação do Bobi. A Mia permanece sem vacinações e sem consultas futuras.
- O serviço de preparação só aceita pedidos em `127.0.0.1:5160` com um token temporário. Não é acrescentado nenhum endpoint ao backend da aplicação.
- Cada teste recebe uma Activity, contentor e sessão limpos. As respostas da aplicação não são simuladas. O interceptor limita-se a registar rota e código para asserções, sem corpos, tokens ou palavras-passe.
- As palavras-passe das contas de teste são passadas como argumentos de instrumentação. O script retira os valores privados dos registos e relatórios antes de os publicar.
- A API, o SQL descartável e o emulador criado pelo script são terminados no fim, mesmo quando há falhas. A imagem Docker descarregada pode ficar em cache.

## Rastreabilidade

Classe: `app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt`. Cada método tem o identificador exato da especificação no nome.

| Cenários | Verificação |
|---|---|
| ST-MOB-F001-01 | Início de sessão e área do Cliente |
| ST-MOB-F001-02 | Palavra-passe errada e conta inexistente, com a mesma mensagem |
| ST-MOB-F001-03 | Conta bloqueada |
| ST-MOB-F001-04 | Validação local, sem pedido de login |
| ST-MOB-F002-01 | Apenas os animais do Cliente; sem escrita |
| ST-MOB-F002-02 | Detalhe, dados administrativos e regresso à lista |
| ST-MOB-F002-03 | Cliente sem animais |
| ST-MOB-F002-04 | Detalhe de animal de outro Cliente: 403 real |
| ST-MOB-F003-01 | Consultas futuras do Cliente |
| ST-MOB-F003-02 | Histórico por data, sem dados clínicos |
| ST-MOB-F003-03 | Ausência de ações de escrita nas consultas |
| ST-MOB-F003-04 | Filtro pelo animal e navegação de regresso |
| ST-MOB-F003-05 | Listas vazias de consultas |
| ST-MOB-F003-06 | Consultas de outro Cliente: 403 real |
| ST-MOB-F006-01 | Boletim, campos, leitura e regresso |
| ST-MOB-F006-02 | Datas calculadas e vacinação criada pela API |
| ST-MOB-F006-03 | Boletim vazio e ausência de próximas administrações |
| ST-MOB-F006-04 | Boletim de outro Cliente: 403 real |

Nos três cenários de acesso indevido, a navegação direta usa o `NavHostController` do teste, conforme a I136, secção 13.2. A API mantém a decisão de autorização.

## Espera, falhas e pipeline

Os passos esperam por condições com `waitUntil`: elementos, textos ou respostas concluídas. Não há pausas fixas. Em falha, a captura do emulador é guardada antes de fechar a Activity.

A abordagem segue a [sincronização de testes de Compose](https://developer.android.com/develop/ui/compose/testing/synchronization). A Activity de alojamento só existe em debug; a aplicação de release mantém o seu ponto de entrada.

As dependências de teste foram alinhadas com AndroidX JUnit 1.3.0 e Espresso 3.7.0. A versão anterior de Espresso (3.5.1) falhou no emulador API 37 ao procurar `InputManager.getInstance`. A [correção está nas notas oficiais do AndroidX Test](https://developer.android.com/jetpack/androidx/releases/test). Os testes unitários e de componentes existentes também foram repetidos depois desta alteração.

O workflow `.github/workflows/mobile-sistema.yml` instala .NET/JDK, inicia um emulador API 37 sem interface e executa o mesmo script duas vezes. Publica os resultados mesmo em falha. O estado da pipeline só pode ser confirmado depois de o código ser submetido e o workflow executar; a configuração do workflow não constitui uma execução aprovada.

## Arquivar a versão final

Depois de guardar as fontes testadas em commit, executar a partir da raiz do repositório:

```sh
python3 src/frontend-mobile/tests/sistema/arquivar.py
```

O script compara os ficheiros do commit com o manifesto da execução e recusa uma versão antiga ou diferente. Exporta diretamente a estrutura do projeto para `04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01`, conforme a secção 19 do regulamento da UC, sem criar outro ZIP. Preserva os documentos da I127. Nas correções da revisão, só substitui uma cópia anterior se ainda corresponder ao seu manifesto de integridade; se alguém a tiver editado manualmente, recusa escrever. As instruções do projeto ficam em `m4-frontend-mobile-instrucoes-fontes-v02.md`, para preservar o índice existente; `m4-i137-origem-fontes-v01.json` identifica o commit e as somas dos ficheiros exportados. Estas novas fontes devem depois ser incluídas no commit de entrega pelo Executor. O script não cria commits nem faz push.

A cópia é a raiz do projeto Android e permite compilar a aplicação. Para executar o orquestrador de sistema, usar `src/frontend-mobile/tests/sistema/executar.py` no repositório completo, onde também estão o backend e os dados SQL. O workflow permanece em `.github/workflows/mobile-sistema.yml`.

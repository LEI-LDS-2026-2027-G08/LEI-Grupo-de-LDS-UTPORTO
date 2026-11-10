# Frontend mobile (Android)

Aplicação Android do **Cliente** da clínica (consulta dos próprios animais, Consultas futuras e histórico, boletim de vacinação). Esta pasta contém a **base** da aplicação (Issue I120) e a autenticação do Cliente (Issue I121); as consultas são implementadas nas Issues I122 e I123.

Stack (Regulamento Interno do Grupo v02, secção 11): Kotlin 2.2, Jetpack Compose com Material 3, Android SDK API 37 (`minSdk` 26), JDK 21, Gradle 9.6 e Android Gradle Plugin 9.4.

## Pré-requisitos

- **Android Studio** recente, com o **Android SDK Platform 37** instalado (SDK Manager).
- **JDK 21** (Eclipse Temurin). O projeto declara a versão em `gradle/gradle-daemon-jvm.properties`; no Android Studio, *Settings → Build Tools → Gradle → Gradle JDK* deve ser o 21.
- Um **emulador Android** (API 26 ou superior; recomenda-se uma imagem recente) ou um telemóvel com depuração USB.
- O **backend local a correr** (ver `src/backend/README.md`), por omissão em `http://localhost:5159`.

Não é preciso instalar o Gradle: o projeto traz o *wrapper* (`gradlew`), que descarrega a versão certa.

## Configurar a URL da API

A URL base da API **nunca está escrita no código**: é definida em tempo de compilação (`BuildConfig.API_BASE_URL`) pela propriedade Gradle `apiBaseUrl`.

| Onde corre a app | Comando / configuração |
|---|---|
| Emulador Android (predefinição) | nada a fazer: usa `http://10.0.2.2:5159` (o `10.0.2.2` é o computador onde corre o emulador) |
| Emulador com timeout em `10.0.2.2` | `adb reverse tcp:5159 tcp:5159` e `./gradlew installDebug -PapiBaseUrl=http://localhost:5159` (ver nota abaixo) |
| Telemóvel por USB | `adb reverse tcp:5159 tcp:5159` e `./gradlew installDebug -PapiBaseUrl=http://localhost:5159` |
| Outra porta ou outro backend | `-PapiBaseUrl=http://10.0.2.2:<porta>` (ou `apiBaseUrl=...` em `~/.gradle/gradle.properties`) |
| Backend com HTTPS | `-PapiBaseUrl=https://<endereço>` |

> **Nota (emulador):** em algumas máquinas o emulador não consegue ligar-se a `10.0.2.2` pela app, mesmo que o Chrome do emulador abra `http://10.0.2.2:5159/swagger`; o ecrã técnico mostra `SocketTimeoutException ... failed to connect to /10.0.2.2`. Nesse caso usa o túnel `adb reverse` da tabela. O `adb` fica em `~/Library/Android/sdk/platform-tools/` (macOS) e é preciso repetir o `adb reverse` sempre que o emulador é reiniciado.

Em **debug**, o HTTP sem TLS só é permitido para `10.0.2.2`, `localhost` e `127.0.0.1` (`src/debug/res/xml/network_security_config.xml`). Em **release**, só HTTPS.

O backend local tem de aceitar o pedido (CORS não se aplica a apps Android) e a conta de teste tem de existir na base de dados (ver o README do backend e o ficheiro de credenciais da submissão; as palavras-passe não estão no repositório).

## Compilar, testar e verificar

Na pasta `src/frontend-mobile`:

```bash
./gradlew assembleDebug        # compila a app
./gradlew testDebugUnitTest    # testes unitários
./gradlew ktlintCheck          # formatação (ktlint)
./gradlew lintDebug            # Android Lint (warningsAsErrors)
```

Para corrigir automaticamente a formatação: `./gradlew ktlintFormat`.

No Windows (PowerShell), usar `.\gradlew.bat` em vez de `./gradlew`. Fora do Android Studio, o Gradle precisa de saber onde está o SDK: `$env:ANDROID_HOME = "$env:LOCALAPPDATA\Android\Sdk"` (ou `sdk.dir` no `local.properties`, que não é versionado).

Para instalar e abrir no emulador: abrir a pasta no Android Studio e carregar em *Run*, ou `./gradlew installDebug`.

## Arquitetura

MVVM com fluxo de dados unidirecional (DM-01 do Modelo de Classes Mobile): o ecrã (`@Composable`) observa o `StateFlow` do seu `ViewModel`; o `ViewModel` usa serviços; os serviços usam apenas o `ApiClient`.

```text
app/src/main/java/pt/ipp/estg/g08/clinica
├── ClinicaApplication.kt, AppContainer.kt   injeção de dependências manual (DM-05)
├── MainActivity.kt
├── core/
│   ├── config/AppConfig.kt                  URL da API (BuildConfig) e tempo limite
│   ├── api/ApiClient.kt, ApiException.kt    camada única de comunicação com a API
│   └── storage/                             TokenStorage: sessão cifrada (Android Keystore)
├── sessao/                                  AuthService, SessaoViewModel, DTOs de sessão, SessaoLocal
├── navegacao/                               Rotas, AppNavHost, AuthGate, MainShell (Navigation Compose)
├── features/login, features/tecnico         M01 (início de sessão) e ecrã técnico (só em debug)
└── shared/                                  EstadoCarregamento e componentes comuns
```

### Comunicação com a API (`ApiClient`)

- Único ponto de saída para a rede. Só aceita rotas `/auth/*` e `/me/*` (RNF003).
- Acrescenta `Authorization: Bearer <token>` quando há sessão.
- Tempo limite de 30 s por pedido (`AppConfig.timeout`).
- Converte erros da API (formato `{ code, message, detail }`) e falhas de rede em `ApiException`, com mensagem em português (`mensagemParaUtilizador()`):

| Situação | Comportamento |
|---|---|
| `401` com sessão | O token é descartado e é invocado `onNaoAutenticado` **uma só vez**: a sessão local termina e a navegação regressa ao início de sessão. |
| `401` sem sessão (login) | Erro normal: credenciais incorretas. A sessão não é tocada. |
| `403` | "Não tem acesso a esta informação." **A sessão mantém-se** (o Cliente pode ter pedido um recurso que não é seu). |
| `404`, `409`, `412`, `422`, `5xx` | Mensagem fixa ou o `detail` da API. |
| Sem rede / tempo esgotado | "Sem ligação ao servidor. Verifique a sua ligação à rede." |

### Sessão

- A sessão (token, expiração, utilizador) é a **única informação guardada no dispositivo**, em `SharedPreferences` privadas, **cifrada com AES-256-GCM**; a chave fica no Android Keystore e nunca sai dele (`CifraKeystore`).
- A sessão fica fora das cópias de segurança do Android (`allowBackup=false` e regras de exclusão).
- `SessaoLocal.terminar(motivo)` remove o token da memória e do armazenamento e emite um evento (`sessaoTerminada`); o `SessaoViewModel` reage e o `AuthGate` volta ao início de sessão.
- Quando o token é ilegível (dados corrompidos, chave perdida), é tratado como "sem sessão" e apagado.

### Autenticação do Cliente (I121)

`SessaoViewModel` (um por atividade, criado no `AppNavHost`) gere o ciclo de vida da sessão, com o `AuthService` (EP-001 `POST /auth/login`, EP-002 `POST /auth/logout` e EP-003 `GET /auth/me`). O `AuthGate` mostra o ecrã que corresponde ao `EstadoSessao`:

| Estado | Ecrã |
|---|---|
| `Desconhecida` | Arranque: indicador de progresso enquanto a sessão guardada é confirmada; sem rede, erro com "Tentar novamente" |
| `NaoAutenticada` | M01 — Início de sessão (e, só em debug, o ecrã técnico) |
| `Autenticada` | Área autenticada (`MainShell`) |

A área autenticada não tem outra entrada: sem sessão de Cliente válida não há rota nem botão que a abra.

| Fluxo | Comportamento |
|---|---|
| Arranque com sessão guardada (RF004) | Se o token já expirou, a sessão é apagada sem chamar a API e M01 mostra "A sua sessão terminou. Inicie sessão novamente.". Caso contrário, confirma com EP-003: válida → área autenticada; `401` → sessão apagada e o mesmo aviso; sem rede → erro com "Tentar novamente" (a sessão guardada mantém-se). |
| Início de sessão (RF001, RF002) | Validação local (email preenchido e com formato válido; palavra-passe preenchida), depois EP-001. Perfil Cliente → sessão guardada cifrada e área autenticada. |
| Conta de perfil interno (RF005, RNF003) | O token recebido é revogado de imediato (EP-002), nada fica guardado e M01 mostra "Esta aplicação destina-se apenas a clientes da clínica." |
| Erros do início de sessão | `401` → "Email ou palavra-passe incorretos." (não revela qual dos campos está errado); `403` → "A conta encontra-se bloqueada."; `400` → a mensagem da API; sem rede → "Sem ligação ao servidor. Verifique a sua ligação à rede."; `5xx` → "Ocorreu um erro no servidor. Tente novamente mais tarde." |
| Terminar sessão (RF003) | EP-002 e, mesmo que o pedido falhe (sem rede, token já revogado), a sessão local é sempre apagada; M01 mostra "Terminou a sessão." |
| Token revogado ou expirado durante a utilização (RF004) | Qualquer `401` termina a sessão (via `SessaoLocal`). Com a aplicação aberta, a sessão também termina no momento de `expiraEm` e quando a aplicação volta a primeiro plano com o token já expirado. |

O token e a palavra-passe nunca aparecem no texto dos objetos (`toString()` de `LoginRequestDto`, `LoginResponseDto` e `SessaoAutenticada`) e a palavra-passe não é guardada no estado restaurável do ecrã.

Contas de demonstração para verificar os fluxos (scripts em `04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao`; as palavras-passe estão só no ficheiro de credenciais da submissão): `cliente.joao@clinica-g08.test`, `cliente.maria@clinica-g08.test` (Cliente), `rececao.bloqueada@clinica-g08.test` (conta bloqueada) e `admin@`, `rececao@`, `vet.ana@`, `aux.carla@clinica-g08.test` (perfis internos, que têm de ser recusados).

## Ecrã técnico (só em debug)

Em debug, o início de sessão tem o botão **Abrir ecrã técnico**, que permite verificar a configuração sem iniciar sessão:

- mostra a **URL da API** configurada e se existe **sessão guardada** (nunca mostra o token);
- **Guardar sessão de teste** grava uma sessão fictícia cifrada (no arranque seguinte, a API recusa-a com `401` e a aplicação apaga-a e mostra o aviso de sessão terminada);
- **Testar ligação à API** faz `GET /auth/me`: sem backend mostra o erro de rede com "Tentar novamente"; com a sessão de teste a API responde `401` e a sessão guardada é apagada;
- **Limpar dados de sessão** apaga a sessão guardada.

O ecrã técnico não dá acesso à área autenticada. Em release, o botão não aparece e a rota não é registada.

## Qualidade

- ktlint 1.5 (plugin Gradle 14.2), estilo `android_studio`, configurado em `.editorconfig`.
- Android Lint com `warningsAsErrors` e `lint.xml`. Regras desativadas: `GradleDependency`, `NewerVersionAvailable` e `AndroidGradlePluginVersion`, todas por dependerem de consultar versões externas, o que tornaria o resultado da pipeline não determinístico. As versões do Gradle e do AGP são as fixadas pelo Regulamento.
- Testes unitários (JUnit 4, MockK, `kotlinx-coroutines-test`) e de componentes (Compose UI Test com Robolectric) em `app/src/test`; o `ApiClient` e o `AuthService` são testados sem rede, com um servidor falso. Na primeira execução, o Robolectric descarrega a imagem do Android usada nos testes (`@Config(sdk = [35])`).

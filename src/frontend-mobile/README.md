# Frontend mobile (Android)

Aplicação Android do **Cliente** da clínica (consulta dos próprios animais, Consultas futuras e histórico, boletim de vacinação). Esta pasta contém a **base** da aplicação (Issue I120); os fluxos de negócio são implementados nas Issues I121 a I123.

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
├── sessao/                                  SessaoAutenticada, Perfil, SessaoLocal
├── navegacao/                               Rotas, AppNavHost, MainShell (Navigation Compose)
├── features/login, features/tecnico         ecrãs (login: I121; técnico: diagnóstico)
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
- `SessaoLocal.terminar(motivo)` remove o token da memória e do armazenamento e emite um evento (`sessaoTerminada`); o `AppNavHost` reage e volta ao início de sessão.
- Quando o token é ilegível (dados corrompidos, chave perdida), é tratado como "sem sessão" e apagado.

### Para a I121 (autenticação)

A base não faz login nem logout no servidor. A I121 acrescenta: `AuthService` (EP-001, EP-002, EP-003), `SessaoViewModel` (restaurar sessão, login, logout), o ecrã M01 real (`features/login/LoginScreen.kt`, hoje provisório) e a rejeição de contas que não são Cliente. Deve:

1. guardar a sessão com `TokenStorage.guardar(...)` e chamar `ApiClient.definirToken(...)`;
2. terminar a sessão sempre por `SessaoLocal.terminar(...)`;
3. mudar `Rotas.inicial()` para o fluxo de arranque (hoje, em debug, abre o ecrã técnico).

## Ecrã técnico (só em debug)

Em debug a app abre num **ecrã técnico** que permite verificar a base sem login:

- mostra a **URL da API** configurada e se existe **sessão guardada** (nunca mostra o token);
- **Guardar sessão de teste** grava uma sessão fictícia cifrada;
- **Testar ligação à API** faz `GET /auth/me`: sem backend mostra o erro de rede com "Tentar novamente"; com a sessão de teste (token inválido) a API responde `401`, a sessão é apagada e a app volta ao início de sessão com o aviso;
- **Limpar dados de sessão** apaga a sessão guardada;
- **Abrir área autenticada** mostra a navegação inferior (Animais e Consultas) e "Terminar sessão".

Em release o ecrã técnico não é alcançável.

## Qualidade

- ktlint 1.5 (plugin Gradle 14.2), estilo `android_studio`, configurado em `.editorconfig`.
- Android Lint com `warningsAsErrors` e `lint.xml`. Regras desativadas: `GradleDependency`, `NewerVersionAvailable` e `AndroidGradlePluginVersion`, todas por dependerem de consultar versões externas, o que tornaria o resultado da pipeline não determinístico. As versões do Gradle e do AGP são as fixadas pelo Regulamento.
- Testes unitários (JUnit 4, MockK disponível, `kotlinx-coroutines-test`) em `app/src/test`; o `ApiClient` é testado sem rede, com um servidor falso.

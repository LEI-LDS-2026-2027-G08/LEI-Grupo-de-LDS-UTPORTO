# Plano de Testes do Frontend Mobile — Gestão de Clínica Veterinária

| Campo | Valor |
| --- | --- |
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Plano de Testes do Frontend Mobile (`OF-M4-017`) |
| Ficheiro | `m4-plano-testes-frontend-mobile-v01.md` |
| Versão | v01 |
| Data | 2026-11-10 |
| Milestone de Entrega | m4 |
| Issue | I047 — Elaborar o Plano de Testes do Frontend Mobile (Sprint 06) |
| Papéis | Executor: Gonçalo Magalhães Ribeiro · Revisor: Gonçalo Dinis Magalhães Teixeira · Auditor: Ivan Cristiano Silva Mendes |
| Pasta de destino | `/projeto/03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.01-plano-de-testes` |
| Fontes | `m3-declaracao-ambito-v05.pdf`; `m3-especificacao-requisitos-v02.md` (I012); `m3-especificacoes-casos-uso-v02.md` (I015); `m3-documentacao-api-v03.md` (I018 e deliberações 10 a 12 da ata do Sprint Review do Sprint 05); `m3-modelo-classes-frontend-mobile-v03.md` (I022); `m3-prototipo-baixa-fidelidade-v02.md` (I017); `m1-plano-qualidade-processo-v01.pdf` (I006) e `m3-plano-qualidade-processo-v02.md`; `m3-checklist-controlo-artefactos-v08.xlsx`; `m3-plano-testes-backend-v03.md`; `m4-plano-testes-frontend-web-v01.md` (I043); dados de demonstração em `04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao`; `m4-backlog-projeto-v04.xlsx` e `m4-s06-sprint-backlog-v01.xlsx`; base da aplicação Android em `src/frontend-mobile` (I120, Pull Request #258, integrado em `develop` no commit `f1b08f7`: `gradle/libs.versions.toml`, `app/build.gradle.kts`, `README.md`) |

## Histórico de versões

| Versão | Data | Autor | Descrição |
| --- | --- | --- | --- |
| v01 | 2026-11-10 | Gonçalo Magalhães Ribeiro | Versão inicial: âmbito, estratégia e níveis de teste, ferramentas e versões, ambiente local e na pipeline (emulador sem interface), dados de teste reproduzíveis, comandos, critérios de entrada e de saída, quality gates, decisões sobre os artefactos facultativos do mobile, condições de teste por funcionalidade, gestão de defeitos, responsabilidades, riscos e rastreabilidade entre requisitos e níveis de teste. |

## Índice

1. Introdução
2. Documentos de referência
3. Âmbito
4. Estratégia e níveis de teste
5. Ferramentas e versões
6. Ambiente e dados de teste
7. Execução dos testes: comandos locais e na pipeline
8. Critérios de entrada e de saída
9. Quality gates e cobertura
10. Condições de teste prioritárias por funcionalidade
11. Testes de aceitação (simulação)
12. Gestão de defeitos
13. Responsabilidades e calendário
14. Riscos
15. Rastreabilidade
16. Coerência com o Plano de Qualidade e com a CCA
17. Decisões tomadas

---

## 1. Introdução

Este plano define como o Grupo G08 verifica a aplicação móvel Android do sistema de Gestão de Clínica Veterinária na Milestone de Entrega m4: o que é testado, a que nível, com que ferramentas, em que ambiente, com que dados, quando um teste pode começar e quando a aplicação se considera pronta para entrega.

A aplicação móvel é usada exclusivamente pelo perfil **Cliente** (RNF003): consulta dos próprios animais, das Consultas futuras e do histórico administrativo e do boletim de vacinação e desparasitação. Não tem operações de escrita sobre estes dados. Os quatro perfis internos usam a aplicação web, que tem plano de testes próprio (I043); na aplicação móvel só entram para confirmar que são recusados.

O plano complementa o Plano de Testes do Backend (`m3-plano-testes-backend-v03.md`): as regras de negócio, a autorização no servidor, o isolamento dos dados entre Clientes e a persistência são verificados no backend; aqui verifica-se que a aplicação móvel usa corretamente as rotas `/auth/*` e `/me/*`, mostra apenas o que o Cliente pode ver, guarda a sessão de forma segura e trata as respostas e as falhas da API.

## 2. Documentos de referência

| Documento | Uso neste plano |
| --- | --- |
| Especificação de Requisitos v02 (I012) | Requisitos funcionais do Cliente (RF001–RF005, RF018, RF028, RF029, RF047) e requisitos não funcionais (secções 5 e 6) |
| Especificações de Casos de Uso v02 (I015) | Fluxos do Cliente usados nas condições de teste e nos cenários de aceitação: UC01, UC02, UC08, UC11 e UC17 |
| Documentação da API v03 | Operações do Cliente: EP-001 a EP-003, EP-019, EP-020, EP-030 e EP-044 (secção 12.3), formato de erros `{ code, message, detail }` e respostas esperadas |
| Modelo de Classes do Frontend Mobile v03 (I022) | Organização da aplicação (MVVM, `ApiClient`, `TokenStorage`, `SessaoViewModel`), regras de sessão (secção 6.2), ecrãs M01–M04 e M06 (secção 7), restrições do Cliente (secção 11) e `testTag` previstas (secção 13) |
| Protótipo de baixa fidelidade v02 (I017) | Ecrãs M01 a M04 e M06; M05 fora do âmbito |
| Plano de Qualidade do Processo (I006, v01 e v02) | Objetivo 5 (sem limiar de cobertura no mobile), quality gates QG-01, QG-05 e QG-07, métricas MQ-03 e MQ-04 |
| Checklist de Controlo de Artefactos v08 | Artefactos obrigatórios do mobile (`OF-M4-003`, `OF-M4-017` a `OF-M4-022`) e condicionais (`REF-M4-001` a `REF-M4-003`) |
| Plano de Testes do Backend v03 e Plano de Testes do Frontend Web v01 | Estrutura do plano, convenções e divisão de responsabilidades entre componentes |
| Scripts de dados de demonstração (`04.13`, m3-v02) e Manual de Instalação e Configuração (I038) | Contas e dados usados nos testes; arranque e reposição do backend |
| Base da aplicação Android (I120, PR #258, integrada em `develop` no commit `f1b08f7`) e respetivo README | Versões das ferramentas, configuração da URL da API, comandos Gradle e comportamento já implementado do `ApiClient` e do `TokenStorage` |
| Backlog de Projeto v04 | Tarefas de desenvolvimento e de teste do mobile e respetivos critérios |
| Regulamento de Funcionamento da UC v19 (secções 17 e 19) | Artefactos de teste exigidos e nomes e pastas dos ficheiros (`03.03`) |

## 3. Âmbito

### 3.1. Dentro do âmbito

- **Requisitos funcionais suportados pela aplicação móvel:** os 9 requisitos cujo ator inclui o Cliente — RF001–RF005 (F001), RF018 (F002), RF028 e RF029 (F003) e RF047 (F006). Lista completa na secção 15.
- **Fluxos do Cliente:** início de sessão (M01), restauro e manutenção da sessão, fim de sessão, os meus animais (M02), detalhe do animal (M03), Consultas futuras e histórico administrativo (M04) e boletim de vacinação e desparasitação (M06).
- **Requisitos não funcionais com verificação na aplicação móvel:** RNF001 (controlo de acesso), RNF002 (isolamento dos dados de Clientes), RNF003 (separação de canais), RNF005 (dados confirmados visíveis numa consulta posterior) e RNF006 (dependência de ligação à rede). O RNF004 aplica-se a operações de criação ou alteração, que a aplicação móvel não tem; a validação local do formulário de início de sessão é verificada no RF002 (secção 15.3).
- **Perfis:** Cliente, com pelo menos duas contas diferentes para o isolamento (RNF002); os quatro perfis internos só para confirmar que a aplicação os recusa sem guardar sessão (RNF003).
- **Integração com a API:** as operações do Cliente segundo a Documentação da API v03, contra o backend real nos testes de sistema e na verificação manual.
- **Comportamento transversal:** armazenamento cifrado da sessão e remoção no fim de sessão, sessão expirada ou revogada (401), acesso recusado (403), erros 4xx/5xx, falha de rede e tempo esgotado, estados de carregamento, lista vazia e erro.

### 3.2. Fora do âmbito

| Elemento | Motivo |
| --- | --- |
| F005 — Gestão de contas e pagamentos: RF042 e ecrã M05 (despesas) | Fora do âmbito pela Declaração de Âmbito v05; a I124 foi descontinuada; o ecrã M05 tem a numeração reservada e não é implementado |
| F007 — Internamento (RF048–RF052) | Fora do âmbito pela Declaração de Âmbito v05; não existe na aplicação móvel |
| Funcionalidades dos perfis internos (RF006–RF017, RF019–RF027, RF030–RF036, RF043–RF046) | Exclusivas da aplicação web (RNF003); cobertas pelo Plano de Testes do Frontend Web (I043). No mobile só se verifica a recusa destes perfis |
| RF053 a RF056 | Não estão especificados na Especificação de Requisitos v02 (secção 9, ponto em aberto 1); nenhum se destina ao Cliente nem à aplicação móvel (RF053 e RF054: Cliente sem conta de acesso; RF055: horários regulares dos Veterinários; RF056: implementado no backend pela I082, para a aplicação web). O estado destes requisitos é registado na RTM final (I051), conforme as deliberações 5 e 12 da ata do Backlog Refinement de 2026-11-07 |
| Regras de negócio, autorização, isolamento e persistência no servidor | Verificados pelos testes unitários e de integração do backend (Plano de Testes do Backend v03) |
| Funcionamento offline e cache local de dados | Não exigidos (Especificação de Requisitos v02, secção 7; RNF006): a aplicação só guarda a sessão |
| Testes de desempenho, carga, segurança ofensiva, acessibilidade formal e usabilidade em dispositivos reais | Não exigidos pelos requisitos aprovados nem pela CCA |
| Vários dispositivos, tamanhos de ecrã e versões Android, e iOS | Ver decisão D-04: verificação num emulador Android de referência; a aplicação é só Android (Regulamento Interno do Grupo, secção 11) |
| Publicação na Play Store e build de release assinada | Fora do âmbito da UC; os testes usam a build de debug |

## 4. Estratégia e níveis de teste

| Nível | O que verifica | Quando | Ferramenta | Artefacto | Obrigatório |
| --- | --- | --- | --- | --- | --- |
| Verificação estática | Compilação, regras do Android Lint e formatação Kotlin | Em cada push e Pull Request | Gradle (`assembleDebug`), Android Lint, ktlint | Relatório de análise estática (`OF-M4-022`, I050) | Sim (QG-01, QG-05) |
| Testes unitários e de componentes (JVM) | Lógica local: `ApiClient`, `ApiException`, `TokenStorage`, sessão, rotas, `ViewModel` e ecrãs isolados com dados simulados | Em cada push e Pull Request | JUnit 4.13, MockK 1.14, `kotlinx-coroutines-test`, Compose UI Test com Robolectric 4.17 (`testDebugUnitTest`) | Nenhum artefacto próprio (decisão D-05); resultado incluído na pipeline e no relatório de resultados | Sim (QG-07, a passar a 100%) |
| Verificação manual por tarefa | Critérios de aceitação de cada tarefa de desenvolvimento, por operação e com as contas de demonstração | Antes de abrir o Pull Request | Emulador Android contra o backend local com dados de demonstração | Registo da verificação manual e capturas no Pull Request (I120 a I123) | Sim (critérios do Backlog) |
| Testes de sistema automáticos | Fluxos completos de cada requisito do Cliente, contra o backend real, na aplicação instalada | Em cada Pull Request (pipeline) e antes do Sprint Review | Compose UI Test (instrumentado, `connectedDebugAndroidTest`) em emulador sem interface | Especificação (`OF-M4-018`, I136) e Resultados (`OF-M4-019`, I139) | Sim (QG-07) |
| Testes de aceitação (simulação) | Utilização da aplicação pelo Cliente em cenários de negócio de ponta a ponta | No Sprint 08, sobre a versão candidata | Sessão guiada no emulador, com elementos do grupo a representar Clientes | Especificação (`OF-M4-020`, I140) e Relatório da simulação (`OF-M4-021`, I141) | Sim |

### 4.1. Convenção de identificadores

| Prefixo | Uso | Exemplo |
| --- | --- | --- |
| `CT-MOB-<funcionalidade>-<n.º>` | Condição de teste definida neste plano (secção 10) | `CT-MOB-F001-03` |
| `CT-MOB-TRV-<n.º>` | Condição transversal (sessão, perfis, isolamento, rede, erros) | `CT-MOB-TRV-02` |
| `ST-MOB-<funcionalidade>-<n.º>` | Cenário de teste de sistema (definido na I136); o identificador consta do nome do teste automático (critério 1 da I137) | `ST-MOB-F003-04` |
| `AT-MOB-<n.º>` | Cenário de aceitação (definido na I140) | `AT-MOB-03` |

Cada cenário indica o requisito ou requisitos que verifica, a conta usada e o caso de uso de origem.

## 5. Ferramentas e versões

Versões declaradas em `src/frontend-mobile/gradle/libs.versions.toml`, `app/build.gradle.kts`, `gradle/wrapper/gradle-wrapper.properties` e `gradle/gradle-daemon-jvm.properties` da base da aplicação (I120, PR #258, integrada em `develop` no commit `f1b08f7`), alinhadas com o Regulamento Interno do Grupo (secção 11) e o Modelo de Classes do Frontend Mobile v03.

| Ferramenta | Versão | Uso |
| --- | --- | --- |
| JDK | 21 (toolchain do daemon Gradle) | Compilação e execução das tarefas Gradle |
| Gradle (wrapper) | 9.6.0 | Execução de todas as verificações |
| Android Gradle Plugin | 9.4.1 | Build, Android Lint e testes instrumentados |
| Kotlin | 2.2.10 | Linguagem |
| Android SDK | compileSdk e targetSdk 37; minSdk 26 | Plataforma |
| Jetpack Compose (BOM) | 2026.02.01, com Material 3 | Interface |
| JUnit | 4.13.2 | Testes unitários e base dos testes Compose |
| MockK | 1.14.11 | Substituição de serviços e `ApiClient` nos testes de `ViewModel` |
| `kotlinx-coroutines-test` | 1.10.2 | Testes de código assíncrono (`runTest`) |
| Robolectric | 4.17 | Testes de componentes Compose e de Android na JVM (`testDebugUnitTest`) |
| Compose UI Test (`ui-test-junit4`) | do BOM 2026.02.01 | Testes de componentes (Robolectric) e testes de sistema instrumentados (emulador) |
| AndroidX Test (`ext.junit`) e Espresso | 1.1.5 e 3.5.1 | Execução dos testes instrumentados |
| ktlint (plugin Gradle) | ktlint 1.5, `org.jlleitschuh.gradle.ktlint` 14.2.0 | Formatação (QG-05) |
| Android Lint | do AGP 9.4.1, com `warningsAsErrors` | Análise estática (QG-05) |
| Emulador Android | Android Emulator do SDK, imagem x86_64 com Google APIs | Verificação manual e testes de sistema (local e pipeline) |
| GitHub Actions | `mobile-ci.yml` (I143), com `reactivecircus/android-emulator-runner` para o emulador sem interface | Pipeline do frontend mobile |
| Backend de teste | .NET 10 e SQL Server 2025, como no Manual de Instalação e Configuração (I038) | API real contra a qual correm os testes de sistema |

Qualquer alteração de versão destas ferramentas durante a m4 é registada no Pull Request correspondente e, se afetar os testes, numa versão nova deste plano.

## 6. Ambiente e dados de teste

### 6.1. Ambientes

| Ambiente | Aplicação | Backend e base de dados | Uso |
| --- | --- | --- | --- |
| Local | `./gradlew installDebug` num emulador Android de referência (Pixel 8, imagem com Android 17 / API 37) criado no Android Studio | Backend em `http://localhost:5159`, arrancado segundo o Manual de Instalação e Configuração (I038), com a base reposta e os dados de demonstração carregados | Verificação manual por tarefa e execução local dos testes de sistema |
| Pipeline | APK de debug compilado e instalado na própria execução, num emulador **sem interface** (`-no-window`, aceleração KVM) | Backend e SQL Server criados na execução, sem instâncias partilhadas (critério 2 da I143), com a mesma reposição e os mesmos dados de demonstração | Testes unitários e de sistema em cada Pull Request (QG-07) |
| Ambiente público de avaliação | APK de debug apontado para o backend público | Backend público (I104) | Apenas validação final dos fluxos (I127) e demonstração; não substitui a pipeline |

**Configuração da API.** O endereço da API é definido em tempo de compilação pela propriedade Gradle `apiBaseUrl` (`BuildConfig.API_BASE_URL`, I120); nunca fica escrito no código e a troca de ambiente não exige alterar código (critério 2 da I127). A predefinição é `http://10.0.2.2:5159` (o computador onde corre o emulador). Para tornar os testes independentes da rede do emulador, os testes de sistema usam o túnel `adb reverse tcp:5159 tcp:5159` e `-PapiBaseUrl=http://localhost:5159`, localmente e na pipeline (decisão D-03). Em debug, o HTTP sem TLS só é permitido para `10.0.2.2`, `localhost` e `127.0.0.1`.

**Viabilidade verificada.** Na revisão da I120 (2026-11-10, Windows), os comandos `assembleDebug`, `testDebugUnitTest` (32 testes), `ktlintCheck` e `lintDebug` terminaram com sucesso num clone limpo, e a aplicação foi instalada e executada no emulador Pixel 8 (Android 17). Nesse computador o emulador não chegou a `10.0.2.2`; com o `adb reverse` e `localhost` a ligação funcionou, o que fundamenta a decisão D-03. Ainda não foi executado nenhum emulador sem interface no GitHub Actions: a viabilidade na pipeline é demonstrada na I143 (critérios 2 e 3), e o risco correspondente está na secção 14.

### 6.2. Dados de teste

- **Contas de demonstração** criadas pelos scripts de dados de demonstração (`04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao`):

| Perfil | Conta | Dados | Uso principal |
| --- | --- | --- | --- |
| Cliente | `cliente.joao@clinica-g08.test` | Bobi (cão) e Mia (gato); M1 Realizada (Bobi, com registo clínico), M2 Cancelada (Mia), M5 Pendente (Bobi); boletim do Bobi com duas vacinas e uma desparasitação; Mia sem registos de profilaxia | Fluxos principais (RF018, RF029, RF047), histórico sem dados clínicos, boletim vazio (Mia) |
| Cliente | `cliente.maria@clinica-g08.test` | Kiko (cão); M3 Confirmada; uma desparasitação | Isolamento entre Clientes (RNF002) |
| Cliente | `cliente.rui@clinica-g08.test` | Luna (gato), sem NIF; M4 Pendente; sem registos de profilaxia | Terceira conta de Cliente; boletim vazio |
| Rececionista bloqueado | `rececao.bloqueada@clinica-g08.test` | — | Conta sem acesso permitido (RF002) |
| Perfis internos | `admin@`, `rececao@`, `vet.ana@`, `aux.carla@clinica-g08.test` | — | Recusa dos perfis internos na aplicação móvel (RNF003) |

- **Palavras-passe:** nunca ficam no repositório, no código dos testes nem nos relatórios. Os testes instrumentados correm no emulador e recebem-nas como argumentos de instrumentação (`-Pandroid.testInstrumentationRunnerArguments.<nome>=<valor>`, lidos com `InstrumentationRegistry.getArguments()`); localmente o valor vem de uma variável de ambiente e na pipeline de _secrets_ do GitHub, mascarados nos registos (decisão D-08). Os nomes dos argumentos são definidos na I137.
- **Limitações dos dados de demonstração para o mobile.** As datas são fixas: as cinco marcações vão de 2026-10-14 a 2026-11-05, pelo que, a partir de 2026-11-06, nenhum Cliente tem Consultas futuras e o RF028 não tem caso positivo; e todos os Clientes têm pelo menos um animal, pelo que não existe o estado «sem animais» (RF018). Os scripts de dados de demonstração (I061) não são alterados por este plano.
- **Dados preparados na execução (decisão D-13).** Antes dos testes de sistema, uma preparação de dados cria, através da própria API e com as contas internas de demonstração, o que os dados fixos não têm: marcações futuras relativas à data da execução para `cliente.joao` e `cliente.maria` (EP-025, Rececionista), uma conta de Cliente sem animais (EP-013, Rececionista) e um registo de vacinação de um animal do `cliente.joao` (EP-043, Veterinário), para verificar o RNF005 e as próximas administrações. Os dados criados respeitam as regras de negócio (horário de funcionamento, sem sobreposições) porque passam pela API. A especificação dos testes de sistema (I136) define estes dados por cenário e a I137 implementa a preparação.
- **Dados reproduzíveis:** a aplicação móvel não cria dados. Antes de cada execução completa a base é reposta com o procedimento de reposição do backend (I062), carregada com os dados de demonstração (I061) e completada pela preparação acima, para que os resultados sejam reproduzíveis, independentes da data e independentes da ordem dos testes.
- **Dados que dependem da data:** os testes verificam a separação entre futuras e histórico pelas datas devolvidas pela API e pelas marcações criadas na preparação, e não por um número fixo de marcações.
- **Testes unitários e de componentes:** usam dados simulados (servidor HTTP falso, MockK e `ArmazenamentoChaveValor` em memória), sem backend.
- **Dados fictícios:** todos os dados são fictícios, como exige o âmbito académico.

## 7. Execução dos testes: comandos locais e na pipeline

### 7.1. Localmente (em `src/frontend-mobile`)

No Windows usa-se `.\gradlew.bat` em vez de `./gradlew`. Fora do Android Studio, o SDK tem de estar indicado em `ANDROID_HOME` (ou em `sdk.dir` no `local.properties`, que não é versionado).

```bash
# 0. Indicar o SDK (exemplo Windows: $env:ANDROID_HOME = "$env:LOCALAPPDATA\Android\Sdk")

# 1. Compilar (QG-01)
./gradlew assembleDebug

# 2. Formatação e análise estática (QG-05): zero violações
./gradlew ktlintCheck lintDebug

# 3. Testes unitários e de componentes na JVM (QG-07)
./gradlew testDebugUnitTest

# 4. Testes de sistema (QG-07), com o emulador ligado e o backend local em execução
#    (as palavras-passe das contas de teste são passadas como argumentos de instrumentação,
#     a partir de variáveis de ambiente; ver secção 6.2)
adb reverse tcp:5159 tcp:5159
./gradlew connectedDebugAndroidTest -PapiBaseUrl=http://localhost:5159
```

Os resultados ficam em `app/build/test-results` e `app/build/reports` (testes e Lint) e em `app/build/outputs/androidTest-results` (testes instrumentados).

### 7.2. Na pipeline

O workflow `.github/workflows/mobile-ci.yml` (I143) corre em cada push e Pull Request que altere `src/frontend-mobile` e executa, por esta ordem:

1. preparação do JDK 21 e da cache do Gradle;
2. compilação (`assembleDebug`, QG-01) e publicação do APK de debug como artefacto (critério 4 da I143);
3. formatação e análise estática (`ktlintCheck lintDebug`, QG-05), com publicação dos relatórios;
4. testes unitários e de componentes (`testDebugUnitTest`, QG-07);
5. arranque do SQL Server e do backend criados na execução, com reposição da base, dados de demonstração e preparação dos dados de teste (secção 6.2);
6. arranque do emulador sem interface, `adb reverse tcp:5159 tcp:5159` e testes de sistema (`connectedDebugAndroidTest -PapiBaseUrl=http://localhost:5159`, QG-07);
7. publicação dos relatórios de testes e, em caso de falha, das capturas de ecrã guardadas pelos testes (critério 3 da I137).

Uma falha em qualquer destes passos bloqueia o merge, tal como no backend e no web (critério 3 da I143).

## 8. Critérios de entrada e de saída

### 8.1. Critérios de entrada

**Verificação manual de uma tarefa (antes do Pull Request):**

- a aplicação compila e passa no ktlint e no Android Lint localmente;
- os testes unitários existentes passam;
- o backend local está em execução com os dados de demonstração;
- as operações usadas existem na Documentação da API v03 e no backend em `develop`.

**Testes de sistema automáticos:**

- a especificação dos testes de sistema (I136) está aprovada;
- as funcionalidades a testar estão integradas em `develop` (I121 a I123);
- a pipeline do mobile (I143) ou o ambiente local descrito em 6.1 está operacional, com o emulador a arrancar;
- as contas de demonstração e os _secrets_ estão disponíveis;
- a preparação dos dados de teste (secção 6.2) terminou sem erros.

**Testes de aceitação (simulação):**

- todos os testes de sistema da versão candidata passaram;
- a especificação dos testes de aceitação (I140) está aprovada;
- existe uma versão identificada (commit) da aplicação e do backend para a sessão.

### 8.2. Critérios de saída

A aplicação móvel considera-se pronta para entrega quando:

1. todos os testes unitários e de sistema automáticos passam na pipeline (MQ-03 = 100%, QG-07), em duas execuções completas consecutivas sem falhas intermitentes (critério 4 da I137), e o relatório de resultados refere essa execução (critério 2 da I139);
2. cada requisito do Cliente da secção 15 tem pelo menos um cenário de sistema positivo a passar e, quando aplicável, um negativo (critério 1 da I136);
3. passam os cenários transversais de sessão expirada, perfil interno recusado, dois Clientes diferentes e falha de rede (critério 3 da I136);
4. QG-01 e QG-05 estão cumpridos na versão entregue;
5. não existe nenhum defeito válido por corrigir (MQ-04 = 100%);
6. a simulação de aceitação está concluída e todos os desvios têm Issue, severidade e decisão (I141);
7. os relatórios de resultados (I139), de aceitação (I141) e de análise estática (I050) estão arquivados nas pastas previstas.

## 9. Quality gates e cobertura

| ID | Âmbito | Verificação | Comando | Limite | Efeito |
| --- | --- | --- | --- | --- | --- |
| `QG-01` | Mobile | Compilação | `./gradlew assembleDebug` | Zero erros | Bloqueia a pipeline |
| `QG-05` | Mobile | Formatação e análise estática | `./gradlew ktlintCheck lintDebug` (`warningsAsErrors`) | Zero violações | Bloqueia o merge |
| `QG-07` | Mobile | Testes automáticos (unitários/componentes e de sistema) | `./gradlew testDebugUnitTest` e `./gradlew connectedDebugAndroidTest` (emulador sem interface) | 100% dos testes a passar | Bloqueia o merge |

**Regras desativadas no Android Lint.** Só `GradleDependency`, `NewerVersionAvailable` e `AndroidGradlePluginVersion` (`app/lint.xml`, I120), por dependerem de consultar versões externas e tornarem a pipeline não determinística; as versões são as fixadas pelo Regulamento Interno do Grupo. Qualquer outra exclusão tem de ser justificada no relatório de análise estática (critério 4 da I050).

**Cobertura:** não é assumido um limiar mínimo de cobertura no frontend mobile (Plano de Qualidade v02, objetivo 5). A garantia de qualidade vem dos testes de sistema contra o backend real e da cobertura de 100% dos requisitos do Cliente por cenários (critério de saída 2), não da percentagem de linhas executadas. Ver decisão D-06.

## 10. Condições de teste prioritárias por funcionalidade

As condições seguintes são o ponto de partida da especificação dos testes de sistema (I136), que as detalha em cenários com pré-condições, dados, passos e resultado esperado.

### 10.1. F001 — Autenticação e sessão (M01, I121)

| ID | Condição | Requisitos |
| --- | --- | --- |
| CT-MOB-F001-01 | Um Cliente com credenciais válidas inicia sessão e chega à área autenticada (M02) | RF001, RF005 |
| CT-MOB-F001-02 | Credenciais inválidas e e-mail inexistente são rejeitados (401) com a mesma mensagem «Email ou palavra-passe incorretos.», sem revelar qual o campo errado; uma conta bloqueada (`rececao.bloqueada`, 403) mostra «A conta encontra-se bloqueada.»; em nenhum caso fica sessão guardada | RF002 |
| CT-MOB-F001-03 | Campos obrigatórios em falta no formulário de início de sessão são assinalados antes do pedido ou pela resposta 400 da API | RF002 |
| CT-MOB-F001-04 | Uma conta de perfil interno (Administrador, Rececionista, Veterinário, Auxiliar Veterinário) é recusada na aplicação com a mensagem «Esta aplicação destina-se apenas a clientes da clínica.»; a sessão é terminada no servidor (EP-002) e nada fica guardado no dispositivo | RF005, RNF003 |
| CT-MOB-F001-05 | Terminar sessão chama EP-002, remove os dados de autenticação do dispositivo e volta a M01; o token antigo deixa de ser aceite | RF003 |
| CT-MOB-F001-06 | Ao reabrir a aplicação com sessão válida, a sessão é restaurada (EP-003) sem pedir novo início de sessão | RF004 |
| CT-MOB-F001-07 | Com a sessão expirada (`expiraEm`) ou revogada no servidor (401), a aplicação limpa a sessão local (`terminarSessaoLocal()`) e volta a M01 | RF004 |
| CT-MOB-F001-08 | A sessão é guardada apenas cifrada (Android Keystore) e fica fora das cópias de segurança; nunca é mostrada nem registada em claro | RNF001 |

### 10.2. F002 — Os meus animais (M02, M03, I122)

| ID | Condição | Requisitos |
| --- | --- | --- |
| CT-MOB-F002-01 | O Cliente vê a lista dos seus animais (EP-019), com os dados básicos | RF018 |
| CT-MOB-F002-02 | O detalhe de um animal (EP-020) mostra os dados básicos e as ligações para o boletim e as Consultas desse animal, sem ações de criar, alterar ou eliminar | RF018 |
| CT-MOB-F002-03 | Um Cliente sem animais (conta criada na preparação dos dados) vê um estado vazio adequado (`m02-vazio`) | RF018 |
| CT-MOB-F002-04 | O pedido de um animal de outro Cliente é recusado (403) com «Não tem acesso a esta informação.» e sem mostrar dados | RF018, RNF002 |

### 10.3. F003 — Consultas futuras e histórico administrativo (M04, I123)

| ID | Condição | Requisitos |
| --- | --- | --- |
| CT-MOB-F003-01 | O separador de Consultas futuras (EP-030, `tipo=futuras`) mostra as marcações futuras dos animais do Cliente criadas na preparação dos dados, e não mostra as do outro Cliente nem as passadas | RF028 |
| CT-MOB-F003-02 | O separador de histórico (EP-030, `tipo=historico`) mostra marcações passadas e consultas realizadas com informação administrativa (data, animal, Veterinário, estado), sem dados clínicos: a M1 do Bobi aparece, mas o diagnóstico «Otite externa», as notas, o tratamento e a prescrição registados nessa consulta não aparecem | RF029 |
| CT-MOB-F003-03 | Não existem ações para marcar, alterar, confirmar ou cancelar Consultas | RF028 |
| CT-MOB-F003-04 | A filtragem pelo animal (a partir de M03) mostra só as marcações desse animal; um `animalId` de outro Cliente é recusado (403) sem mostrar dados | RF028, RF029, RNF002 |
| CT-MOB-F003-05 | Listas vazias (sem futuras ou sem histórico) têm estado vazio adequado | RF028, RF029 |

### 10.4. F006 — Boletim de vacinação e desparasitação (M06, I122)

| ID | Condição | Requisitos |
| --- | --- | --- |
| CT-MOB-F006-01 | O boletim de um animal do Cliente (EP-044) mostra o histórico de vacinações e desparasitações | RF047 |
| CT-MOB-F006-02 | As próximas administrações previstas apresentadas correspondem às datas calculadas pela API, incluindo a da vacinação registada na preparação dos dados | RF047, RNF005 |
| CT-MOB-F006-03 | Um animal sem registos de profilaxia (Mia) mostra um estado vazio adequado (`m06-vazio`) | RF047 |
| CT-MOB-F006-04 | O boletim de um animal de outro Cliente é recusado (403) sem mostrar dados | RF047, RNF002 |

### 10.5. Transversal — sessão, perfis, isolamento, rede e erros

| ID | Condição | Requisitos |
| --- | --- | --- |
| CT-MOB-TRV-01 | Duas contas de Cliente diferentes (`cliente.joao` e `cliente.maria`) veem cada uma apenas os seus animais, Consultas e boletins | RNF002 |
| CT-MOB-TRV-02 | A aplicação só comunica com as rotas `/auth/*` e `/me/*` e não disponibiliza funcionalidades dos perfis internos | RNF001, RNF003 |
| CT-MOB-TRV-03 | Os erros da API (`{ code, message, detail }`) são mostrados em português europeu sem expor detalhes técnicos, segundo a secção 12.2 do Modelo de Classes do Frontend Mobile v03: 400 mostra a mensagem de dados inválidos, 401 leva a M01, 403 mostra «Não tem acesso a esta informação.», 404 mostra «A informação pedida já não existe.» e 500 mostra erro do servidor | RNF001, RF004 |
| CT-MOB-TRV-04 | Sem rede, com o backend indisponível ou com tempo esgotado, a aplicação mostra «Sem ligação ao servidor. Verifique a sua ligação à rede.» com «Tentar novamente» e não bloqueia | RNF006 |
| CT-MOB-TRV-05 | Os estados de carregamento são apresentados e os botões ficam inativos durante o pedido | — (qualidade de interface) |
| CT-MOB-TRV-06 | Em release só é permitido HTTPS; em debug o HTTP fica limitado aos endereços locais | RNF001 |

## 11. Testes de aceitação (simulação)

A simulação de aceitação (I140 e I141) verifica se a aplicação permite a um Cliente realizar do princípio ao fim o que precisa, segundo os casos de uso aprovados. É uma simulação académica e não equivale a uma homologação por um cliente real.

- **Participantes:** os quatro elementos do grupo. Cada cenário é executado por um elemento que não implementou essa funcionalidade, no papel de Cliente (com as contas `cliente.joao` e `cliente.maria`), e outro elemento regista os resultados.
- **Cenários:** pelo menos um por funcionalidade do Cliente — início e fim de sessão (UC01, UC02), os meus animais (UC08), Consultas futuras e histórico (UC11) e boletim (UC17) —, em linguagem do Cliente e com critério de aceitação explícito.
- **Ambiente:** emulador Android de referência (secção 6.1) com a versão identificada (commit) da aplicação e do backend.
- **Resultado:** cada cenário fica «Aceite», «Aceite com reservas» ou «Não aceite»; os desvios têm Issue, severidade e decisão (corrigir antes da entrega ou registar como limitação conhecida nas Notas de Release finais, I056).

## 12. Gestão de defeitos

- Uma falha num teste de sistema ou de aceitação é analisada pelo responsável da tarefa de teste: falha do teste (corrige-se o teste) ou defeito do produto.
- Um defeito do produto é registado como Issue no GitHub, com a etiqueta `bug` e a etiqueta de componente `frontend-mobile`, a identificação do cenário, os passos, o resultado obtido, o esperado, a captura de ecrã guardada pelo teste e a severidade (bloqueante, maior ou menor). Um defeito do backend detetado no mobile é registado com a etiqueta `backend` (I127).
- O defeito é corrigido por Pull Request ligado à Issue e revalidado pelo mesmo cenário antes de fechar.
- Os relatórios de resultados (I139) e de aceitação (I141) indicam, para cada falha, a descrição, a causa, a ação e o estado.
- Não é mantido um registo de defeitos do mobile em ficheiro separado (decisão D-07); se no fim da m4 ficarem defeitos válidos por corrigir, o grupo cria esse registo em `03.03.07-defeitos` e atualiza a CCA.

## 13. Responsabilidades e calendário

### 13.1. Papéis

| Papel | Responsabilidade nos testes do frontend mobile |
| --- | --- |
| Executor de cada tarefa de desenvolvimento (I120 a I123) | Verificação manual da sua funcionalidade no emulador, com as contas de demonstração, e testes unitários do código novo, com evidências no Pull Request |
| Revisor | Repete a verificação a partir de um clone limpo, seguindo o README, e confirma o tratamento de erros, a sessão e o isolamento |
| Executores das tarefas de teste (I136 a I141) | Especificar, automatizar e executar os testes e produzir os relatórios |
| Responsável pelos frameworks de teste (Gonçalo Magalhães Ribeiro, Plano de Qualidade) | Configuração do JUnit, MockK, Robolectric e Compose UI Test no projeto e apoio aos Executores |
| Auditor | Confirma que as evidências correspondem às execuções reais e que os quality gates foram cumpridos |

### 13.2. Tarefas de testes e qualidade do frontend mobile

| Issue | Tarefa | Sprint previsto | Artefacto |
| --- | --- | --- | --- |
| I047 | Plano de Testes do Frontend Mobile (este documento) | S06 | `m4-plano-testes-frontend-mobile-v01.md` |
| I120 | Base da aplicação Android (inclui os primeiros testes unitários) | S06 | Código em `src/frontend-mobile` |
| I121 | Autenticação do Cliente (RF001–RF005) | S06 | Código e evidências no Pull Request |
| I122 | Animais do Cliente e boletim (RF018, RF047) | S07 | Código e evidências no Pull Request |
| I123 | Consultas futuras e histórico administrativo (RF028, RF029) | S07 | Código e evidências no Pull Request |
| I136 | Especificar os testes de sistema do frontend mobile | S07 | `m4-especificacao-testes-sistema-frontend-mobile-v01.md` |
| I140 | Especificar os testes de aceitação do frontend mobile | S07 | `m4-especificacao-testes-aceitacao-frontend-mobile-v01.md` |
| I137 | Automatizar os testes de sistema do frontend mobile | S08 | Código dos testes instrumentados |
| I138 | Automatizar os testes de sessão, permissões e falhas de rede | S08 | Código dos testes instrumentados |
| I139 | Executar os testes de sistema e produzir o relatório de resultados | S08 | `m4-resultados-testes-sistema-frontend-mobile-v01.pdf` |
| I141 | Simular a aceitação e produzir o relatório | S08 | `m4-relatorio-simulacao-testes-aceitacao-frontend-mobile-v01.pdf` |
| I050 | Executar a análise estática e produzir o relatório | S08 | `m4-relatorio-analise-estatica-frontend-mobile-v01.pdf` |
| I143 | Integrar compilação, análise estática e testes do mobile na pipeline | S08 | `mobile-ci.yml` |
| I127 | Validar e corrigir os fluxos integrados da aplicação móvel | S08 | Correções e evidências nos Pull Requests |

## 14. Riscos

| Risco | Efeito | Mitigação |
| --- | --- | --- |
| Atraso da base (I120) e da autenticação (I121) | Atrasa I122, I123 e todos os testes de sistema | Prioridade à base no início do Sprint 06; testes escritos por funcionalidade à medida que são integradas |
| Emulador sem interface lento ou instável na pipeline | Bloqueios falsos do merge e pipeline demorada | Aceleração KVM, imagem x86_64, animações desligadas, cache do Gradle e do AVD; esperas por elementos (`waitUntil`) sem pausas fixas; duas execuções completas seguidas sem falhas (critério 4 da I137) |
| Imagem do emulador para a API 37 indisponível no executor do GitHub | Testes instrumentados não arrancam | Usar a imagem mais recente disponível que cumpra o minSdk 26, registada no `mobile-ci.yml` e no relatório de resultados; a referência local mantém-se no Android 17 |
| Emulador sem acesso a `10.0.2.2` (observado na revisão da I120) | Falhas de rede falsas | Túnel `adb reverse` e `apiBaseUrl=http://localhost:5159` em todos os testes de sistema (D-03) |
| Dados de demonstração com datas fixas (sem marcações futuras depois de 2026-11-05) e sem Cliente sem animais | RF028 e estado vazio de M02 sem caso de teste; resultados diferentes conforme o dia | Preparação dos dados de teste pela API em cada execução, com datas relativas à data da execução (D-13); verificação pelas datas devolvidas pela API e não por contagens fixas |
| Contradição entre o Plano de Qualidade v02 e o Modelo de Classes do Frontend Mobile v03 (QG-07 com `testDebugUnitTest` e Robolectric) e o Backlog v04 (testes de sistema em emulador sem interface) | Revisor ou Auditor consideram o plano incoerente com o PQ | Decisão D-02 e proposta de atualização do PQ e do Modelo por Issue própria, com revisão e auditoria |
| Divergência entre a API documentada e a implementada | Falhas na integração | Os testes correm contra o backend real; divergências registadas no Pull Request e decididas antes do merge |
| Requisitos por especificar (RF053 a RF056) | Funcionalidade sem critério de verificação | Mantidos fora do âmbito até serem aprovados; registo na RTM final (I051) |
| Credenciais expostas nos testes | Falha de segurança | Palavras-passe só em propriedades locais, variáveis de ambiente e _secrets_ (D-08) |

## 15. Rastreabilidade

### 15.1. Requisitos funcionais e níveis de teste

Legenda dos níveis: **U** — testes unitários e de componentes (JVM); **M** — verificação manual na tarefa de desenvolvimento; **S** — testes de sistema automáticos; **A** — simulação de aceitação.

| Requisito | Título | Ecrã | Caso de uso | Endpoint(s) | Tarefa de desenvolvimento | Tarefa de automação | Condições | Níveis |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| RF001 | Autenticar utilizador | M01 | UC01 | EP-001 | I121 | I137 | CT-MOB-F001-01 | U, M, S, A |
| RF002 | Rejeitar autenticação inválida | M01 | UC01 | EP-001 | I121 | I137 | CT-MOB-F001-02, CT-MOB-F001-03 | U, M, S, A |
| RF003 | Terminar sessão | Barra de navegação | UC02 | EP-002 | I121 | I138 | CT-MOB-F001-05 | U, M, S, A |
| RF004 | Manter acesso autenticado válido | M01 e área autenticada | UC01 | EP-001, EP-003 | I120, I121 | I138 | CT-MOB-F001-06, CT-MOB-F001-07, CT-MOB-TRV-03 | U, M, S |
| RF005 | Restringir funcionalidades por perfil | M01 | UC01 | EP-001, EP-003 (EP-002 para revogar o token de um perfil interno) | I121 | I138 | CT-MOB-F001-01, CT-MOB-F001-04 | U, M, S, A |
| RF018 | Consultar animais do Cliente | M02, M03 | UC08 | EP-019, EP-020 | I122 | I137 | CT-MOB-F002-01 a 04 | U, M, S, A |
| RF028 | Consultar marcações futuras | M04 | UC11 | EP-030 | I123 | I137 | CT-MOB-F003-01, 03, 04, 05 | U, M, S, A |
| RF029 | Consultar histórico administrativo de marcações e consultas | M04 | UC11 | EP-030 | I123 | I137 | CT-MOB-F003-02, 04, 05 | U, M, S, A |
| RF047 | Consultar boletim na aplicação móvel | M06 | UC17 | EP-044 | I122 | I137 | CT-MOB-F006-01 a 04 | U, M, S, A |

### 15.2. Cobertura dos requisitos do Cliente

Os 9 requisitos funcionais suportados pela aplicação móvel têm verificação manual na tarefa de desenvolvimento e pelo menos uma condição de teste de sistema automático; 8 deles entram também na simulação de aceitação, que cobre todos os casos de uso do Cliente (UC01, UC02, UC08, UC11 e UC17). Nenhuma funcionalidade móvel fora do âmbito atual é incluída (secção 3.2).

### 15.3. Requisitos não funcionais

| Requisito | Como é verificado no frontend mobile | Condições | Níveis |
| --- | --- | --- | --- |
| RNF001 — Controlo de acesso às operações | Só rotas `/auth/*` e `/me/*`; sessão cifrada; perfis internos recusados; sem dados sensíveis em claro | CT-MOB-F001-04, CT-MOB-F001-08, CT-MOB-TRV-02, CT-MOB-TRV-03, CT-MOB-TRV-06 | U, M, S |
| RNF002 — Isolamento dos dados de Clientes | Dois Clientes só veem a sua informação; recursos de outro Cliente recusados sem mostrar dados | CT-MOB-TRV-01, CT-MOB-F002-04, CT-MOB-F003-04, CT-MOB-F006-04 | M, S, A |
| RNF003 — Separação dos canais de utilização | Perfis internos recusados na aplicação móvel; sem funcionalidades internas | CT-MOB-F001-04, CT-MOB-TRV-02 | M, S |
| RNF004 — Validação dos dados de entrada | Não aplicável ao mobile: a aplicação não tem operações de criação nem de alteração (secção 11 do Modelo de Classes do Frontend Mobile v03); a validação destas operações é verificada no backend e no web. A validação local do início de sessão é verificada como parte do RF002 | CT-MOB-F001-03 (RF002) | — |
| RNF005 — Persistência dos dados confirmados | Dados registados pelos perfis internos através da API (marcações e vacinação criadas na preparação dos dados) visíveis na aplicação móvel numa consulta posterior | CT-MOB-F003-01, CT-MOB-F006-02 | S, A |
| RNF006 — Dependência de ligação à rede | Mensagem de falha sem ligação e «Tentar novamente»; sem modo offline | CT-MOB-TRV-04 | U, M, S |

## 16. Coerência com o Plano de Qualidade e com a CCA

| Elemento | Plano de Qualidade v02 | CCA v08 | Este plano |
| --- | --- | --- | --- |
| Plano de testes do frontend mobile | `OF-M4-017` | `OF-M4-017` (previsto) | Este documento |
| Especificação e resultados dos testes de sistema | `OF-M4-018`, `OF-M4-019`; QG-07 | `OF-M4-018`, `OF-M4-019` (previstos) | Obrigatórios (I136, I139) |
| Especificação e relatório de aceitação | `OF-M4-020`, `OF-M4-021` | `OF-M4-020`, `OF-M4-021` (previstos) | Obrigatórios (I140, I141) |
| Análise estática | `OF-M4-022` (ktlint e Android Lint); QG-05 | `OF-M4-022` (previsto) | Obrigatória (I050) |
| Frontend mobile (código) | — | `OF-M4-003` (previsto) | Arquivado em `04.15-frontend-mobile/m4-v01` na entrega da M4 |
| Resultados dos testes unitários do frontend (`03.03.04-testes-unitarios`) | Não previstos como artefacto; o PQ v02 inclui os testes de `testDebugUnitTest` no QG-07 | `REF-M4-001` (condicional: «quando o grupo assuma testes unitários para os frontends») não incluído | Testes JVM executados no QG-07 e reportados nos Resultados dos testes de sistema (`OF-M4-019`); sem artefacto próprio (D-05) |
| Relatório de cobertura do frontend (`03.03.05-cobertura-de-testes`) | Sem limiar mínimo (objetivo 5) | `REF-M4-002` (condicional) não incluído | Não assumido (D-06) |
| Registo de defeitos do frontend (`03.03.07-defeitos`) | «Quando aplicável» | `REF-M4-003` (condicional) não incluído | Defeitos em Issues e nos relatórios; registo separado só se necessário (D-07) |
| Taxa de sucesso dos testes de sistema | MQ-03 = 100% | — | Critério de saída 1 |
| Resolução de defeitos válidos | MQ-04 = 100% | — | Critério de saída 5 |

## 17. Decisões tomadas

| N.º | Decisão | Razão |
| --- | --- | --- |
| D-01 | Os testes de sistema automáticos são testes Compose UI instrumentados (`connectedDebugAndroidTest`), na aplicação instalada num emulador Android sem interface, contra o backend real com dados de demonstração, sem simular a API | Critério 2 da I047 e critérios 2 das I137 e I143 (emulador sem interface contra backend criado na execução); testar contra o backend real deteta divergências de contrato e verifica o isolamento entre Clientes |
| D-02 | O QG-07 inclui dois comandos: `testDebugUnitTest` (JUnit, MockK, Robolectric e Compose UI Test na JVM) e `connectedDebugAndroidTest` (testes de sistema em emulador); ambos têm de passar a 100% | O Plano de Qualidade v02 (QG-07) e o Modelo de Classes do Frontend Mobile v03 (secção 13) indicam o `testDebugUnitTest` com Compose UI Test e Robolectric; o Backlog v04, aprovado depois (critério 2 da I047, critério 2 da I137 e critério 2 da I143), exige os testes de sistema em emulador sem interface contra um backend criado na execução. O plano cumpre os dois: mantém o `testDebugUnitTest` e acrescenta o emulador. A atualização do texto do QG-07 no Plano de Qualidade e da secção 13 do Modelo é proposta por Issue própria, com revisão e auditoria; até lá prevalecem os critérios aprovados no Backlog |
| D-03 | Os testes de sistema usam `adb reverse tcp:5159 tcp:5159` e `-PapiBaseUrl=http://localhost:5159`, localmente e na pipeline | Na revisão da I120 o emulador não chegou a `10.0.2.2`; o túnel torna a ligação determinística e a troca faz-se por configuração, sem alterar código |
| D-04 | A verificação usa um emulador Android de referência (Pixel 8, Android 17 / API 37) localmente e a imagem mais recente disponível no executor da pipeline; não são testados vários dispositivos nem versões | Os requisitos não exigem compatibilidade com vários dispositivos; reduz a duração e a instabilidade da pipeline |
| D-05 | Os testes unitários e de componentes do mobile são obrigatórios (QG-07), mas não geram artefacto próprio em `03.03.04-testes-unitarios`; os seus resultados entram nos Resultados dos testes de sistema (`OF-M4-019`, I139) e nos registos da pipeline | O Plano de Qualidade v02 classifica o `testDebugUnitTest` como parte dos testes de sistema automáticos do QG-07, e não como artefacto autónomo; `REF-M4-001` não foi incluído na CCA v08. Se a revisão ou a auditoria entenderem que a condição do `REF-M4-001` se verifica, a CCA é atualizada numa nova versão e o artefacto é produzido em `03.03.04-testes-unitarios` |
| D-06 | Não é assumido limiar de cobertura nem relatório de cobertura do mobile (`03.03.05-cobertura-de-testes`) | Plano de Qualidade v02, objetivo 5, e `REF-M4-002` não incluído na CCA v08; a cobertura de requisitos por cenários é a medida usada |
| D-07 | Os defeitos são registados em Issues do GitHub e nos relatórios de resultados e de aceitação; não há registo de defeitos em ficheiro separado (`03.03.07-defeitos`), salvo se ficarem defeitos válidos por corrigir no fim da m4 | Plano de Qualidade v02 («quando aplicável») e `REF-M4-003` não incluído na CCA v08 |
| D-08 | Palavras-passe e endereços dos ambientes ficam em propriedades locais, variáveis de ambiente e _secrets_; nunca no código, nos testes nem nos relatórios | Regulamento Interno do Grupo (segredos técnicos geridos em GitHub Actions Secrets e ficheiros locais excluídos pelo `.gitignore`) e regra de credenciais do projeto (credenciais de avaliação só no ficheiro entregue no ZIP) |
| D-09 | RF042 (M05), F007 e RF053 a RF056 ficam fora do âmbito deste plano | Declaração de Âmbito v05; deliberações 5 e 12 da ata do Backlog Refinement de 2026-11-07; ponto em aberto 1 da Especificação de Requisitos v02 |
| D-10 | Os erros da API são verificados no formato `{ code, message, detail }` e mostrados em português europeu sem detalhe técnico | Documentação da API v03 (deliberação 10 da ata do Sprint Review do Sprint 05); critério 5 da I121 |
| D-11 | Os testes de sistema identificam o cenário no nome do teste, são independentes da ordem, não usam pausas fixas e guardam captura de ecrã quando falham; a base é reposta antes de cada execução completa | Critérios 1 e 3 da I137; reprodutibilidade na pipeline |
| D-12 | Os testes usam os `testTag` previstos na secção 13 do Modelo de Classes do Frontend Mobile v03 (por exemplo `m01-email`, `m02-lista-animais`, `nav-terminar-sessao`) | Decisão DM-06 do modelo; seletores estáveis, independentes do texto apresentado |
| D-13 | Os dados de teste que os dados de demonstração não têm (marcações futuras, Cliente sem animais, vacinação nova) são criados em cada execução através da API, com as contas internas de demonstração e datas relativas à data da execução; os scripts de dados de demonstração não são alterados | Os dados de demonstração (I061) têm datas fixas e já não têm marcações futuras desde 2026-11-06, e todos os Clientes têm animais; criar os dados pela API respeita as regras de negócio e mantém os testes reproduzíveis em qualquer dia (critério 2 da I047) |

# I137 — Relatório do Executor — v01

Data: 23/11/2026. Issue: [I137 — GitHub #125](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/125).

Estado: implementação e verificação local preparadas para revisão. A pipeline, a revisão independente, a auditoria e a integração não são declaradas aprovadas.

## 1. Resultado produzido

Foram implementados os **18 cenários atribuídos à I137** na secção 13.1 da Especificação de Testes de Sistema Mobile v01 (I136): F001-01 a F001-04, F002-01 a F002-04, F003-01 a F003-06 e F006-01 a F006-04. Os outros 12 cenários são da I138. Despesas/I124 estão fora do âmbito documental do projeto.

- [Matriz cenário ↔ método ↔ requisitos](m4-i137-matriz-cenarios-testes-v01.md).
- [Guia de execução e preparação](../../../src/frontend-mobile/tests/sistema/README.md).
- [Classe de testes instrumentados](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt).
- [Workflow da pipeline](../../../.github/workflows/mobile-sistema.yml).

Cada teste abre a interface Compose no emulador e faz pedidos à API real. O script cria uma API e um contentor SQL descartáveis. Antes de cada cenário, repõe a base e cria pela API os dados PD-01 a PD-04. As contas e as credenciais são geradas para a base de teste; não se usam as palavras-passe privadas do grupo ou o Azure.

O contentor da base habitual do utilizador não foi usado. Os serviços de teste foram terminados no fim e o emulador iniciado para esta verificação foi fechado.

## 2. Identificação do código verificado

Base Git das execuções mais recentes: `bc25b2dc2563be0f8c8c9b61b714280635a2a48b`.

Os testes e o workflow já constam desse commit. A correção que transmite a localização do Android SDK ao Gradle ainda era uma alteração local durante estas execuções. A versão verificada está identificada pelas somas SHA-256 de 114 ficheiros relevantes em [m4-i137-fontes-executadas-v01.json](m4-i137-fontes-executadas-v01.json).

O ensaio do Executor `a8c2ffaf7fdd` não chegou a executar testes: o Gradle não encontrou o SDK. O script encontrava o emulador na pasta predefinida do macOS, mas não transmitia essa pasta ao Gradle. Foi corrigido para incluir `ANDROID_HOME` no ambiente dos processos. A execução `d27fcc4bc41c` repetiu a suite duas vezes, sem `ANDROID_HOME` nem `ANDROID_SDK_ROOT` definidos no terminal, com sucesso em ambas.

As duas execuções iniciais, sobre a base `c45180e8a6b6fe93e4aeed9882f059fbd32b465a`, também tinham passado. Os resultados arquivados abaixo correspondem à repetição após a correção do SDK. Os testes da aplicação não foram alterados nesta correção.

O Executor deve guardar a correção e as evidências em commit antes de exportar as fontes diretamente para a pasta de entrega. O script de arquivo recusa um commit que não contenha os ficheiros verificados. O commit de entrega e o endereço do PR devem ser associados ao processo quando forem criados.

## 3. Duas execuções completas consecutivas

Ambiente observado: macOS, emulador Android 17/API 37 ARM64 sem janela, fuso `Europe/Lisbon`, animações desligadas, API .NET 10 e SQL Server 2025 em contentor exclusivo. As duas execuções usaram o mesmo código, sem alteração entre elas.

Comando executado na pasta `src/frontend-mobile`:

```sh
env -u ANDROID_HOME -u ANDROID_SDK_ROOT JAVA_HOME="/Applications/Android Studio.app/Contents/jbr/Contents/Home" python3 tests/sistema/executar.py
```

O próprio script iniciou o emulador sem janela e fechou-o no fim. A localização do SDK foi descoberta pela configuração predefinida do macOS e transmitida ao Gradle pelo script.

| Execução | Testes | Falhas | Erros | Ignorados | Resultado Gradle |
|---|---:|---:|---:|---:|---|
| 1 | 18 | 0 | 0 | 0 | Aprovada, 3 min 7 s |
| 2 | 18 | 0 | 0 | 0 | Aprovada, 2 min 9 s |

Evidências:

- [Resumo das duas execuções](m4-i137-resultados-execucoes-v01.json).
- [Registo da execução 1](evidencias-i137/execucao-1/registo-gradle.md).
- [Registo da execução 2](evidencias-i137/execucao-2/registo-gradle.md).
- Relatórios JUnit nas mesmas pastas, com os identificadores dos 18 métodos.

Não houve falhas intermitentes nestas duas execuções completas da versão final local. Os ensaios de desenvolvimento anteriores não são apresentados como execuções aprovadas.

## 4. Falha controlada e captura

Depois das duas execuções iniciais aprovadas, antes da correção da localização do SDK, foi alterado temporariamente o serviço de animais para omitir o Bobi da lista devolvida pela API. Foi executado apenas ST-MOB-F001-01: o teste falhou como esperado, por faltar o Bobi, e guardou a captura com o identificador do cenário.

O ficheiro original foi reposto byte a byte e a soma SHA-256 foi conferida. A alteração temporária não integra a entrega. Esta verificação do Executor não substitui a quebra de um fluxo e a execução independente exigidas ao Revisor.

[Registo da falha controlada e da reposição](m4-i137-falha-controlada-v01.json).

<details>
<summary>Falha induzida: Bobi omitido, apenas Mia visível</summary>

Esta imagem corresponde à falha deliberadamente introduzida, não a um resultado aprovado nem a um defeito deixado na aplicação.

![ST-MOB-F001-01: deteção da ausência do Bobi](evidencias-i137/falha-controlada/ST-MOB-F001-01.png)

</details>

## 5. Verificações complementares

Depois de repor o código, foram executados `testDebugUnitTest`, `ktlintCheck`, `lintDebug` e `assembleDebug`. Resultado: `BUILD SUCCESSFUL in 17s`; **166 testes unitários e de componentes aprovados, sem falhas, erros ou ignorados**. A compilação final repôs a predefinição local `http://10.0.2.2:5159`; não foi instalada num dispositivo nesta etapa.

As dependências de teste foram atualizadas para AndroidX JUnit 1.3.0 e Espresso 3.7.0 devido à incompatibilidade reproduzida de Espresso 3.5.1 com a API 37 (`InputManager.getInstance`). [Notas oficiais do AndroidX Test](https://developer.android.com/jetpack/androidx/releases/test).

O YAML do workflow foi analisado localmente. Os passos de interação usam espera por condições (`waitUntil`); não foram introduzidas pausas fixas. As capturas são guardadas fora do diretório privado do APK para sobreviverem à desinstalação automática do teste, e recolhidas pelo script.

## 6. Critérios de aceitação e pendências

| Critério | Estado desta entrega |
|---|---|
| CA1 — cenários automatizados e identificados | 18/18 dos atribuídos à I137 na I136; matriz arquivada. I138 mantém os outros 12. |
| CA2 — emulador sem interface, local e pipeline, backend isolado | Verificado localmente; workflow preparado. **Execução na pipeline pendente de submissão e resultado real.** |
| CA3 — independência, espera e captura de falhas | Base e sessão repostas por cenário, dados preparados pela API, espera por condições; captura comprovada no ensaio de falha controlada. |
| CA4 — duas execuções completas consecutivas | Cumprido localmente: 18/18 nas duas execuções. |

Para concluir a entrega: commit das fontes testadas e documentos; geração do arquivo das fontes a partir desse commit; submissão do PR; confirmação da pipeline; revisão em clone limpo duas vezes e falha provocada pelo Revisor; auditoria e aceitação conforme o processo do grupo.

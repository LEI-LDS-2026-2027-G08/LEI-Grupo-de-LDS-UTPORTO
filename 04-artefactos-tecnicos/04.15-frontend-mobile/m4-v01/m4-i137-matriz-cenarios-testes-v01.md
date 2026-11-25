# I137 — Matriz de cenários e testes — v01

Estado: para revisão. Issue I137 (GitHub #125).

A secção 13.1 da [Especificação I136](../../../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema/m4-especificacao-testes-sistema-frontend-mobile-v01.md) atribui estes 18 cenários à I137. Os outros 12 são da I138 e não são declarados automatizados por esta entrega. Despesas/I124 estão fora do âmbito.

Cada cenário corresponde a um método instrumentado com o seu identificador exato na classe `FluxosClienteSistemaTest`. Todos usam a aplicação e a API reais, com a base SQL isolada reposta e os dados criados antes de cada teste.

| Cenário | Método | Requisitos |
|---|---|---|
| ST-MOB-F001-01 | [`ST-MOB-F001-01`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L199) | RF001, RF005 |
| ST-MOB-F001-02 | [`ST-MOB-F001-02`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L211) | RF002 |
| ST-MOB-F001-03 | [`ST-MOB-F001-03`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L230) | RF002 |
| ST-MOB-F001-04 | [`ST-MOB-F001-04`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L237) | RF002 |
| ST-MOB-F002-01 | [`ST-MOB-F002-01`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L249) | RF018 |
| ST-MOB-F002-02 | [`ST-MOB-F002-02`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L264) | RF018 |
| ST-MOB-F002-03 | [`ST-MOB-F002-03`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L284) | RF018 |
| ST-MOB-F002-04 | [`ST-MOB-F002-04`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L291) | RF018, RNF002 |
| ST-MOB-F003-01 | [`ST-MOB-F003-01`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L306) | RF028, RNF005 |
| ST-MOB-F003-02 | [`ST-MOB-F003-02`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L324) | RF029 |
| ST-MOB-F003-03 | [`ST-MOB-F003-03`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L354) | RF028, RF029 |
| ST-MOB-F003-04 | [`ST-MOB-F003-04`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L371) | RF028, RF029 |
| ST-MOB-F003-05 | [`ST-MOB-F003-05`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L390) | RF028, RF029 |
| ST-MOB-F003-06 | [`ST-MOB-F003-06`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L404) | RF028, RF029, RNF002 |
| ST-MOB-F006-01 | [`ST-MOB-F006-01`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L416) | RF047 |
| ST-MOB-F006-02 | [`ST-MOB-F006-02`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L432) | RF047, RNF005 |
| ST-MOB-F006-03 | [`ST-MOB-F006-03`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L461) | RF047 |
| ST-MOB-F006-04 | [`ST-MOB-F006-04`](../../../src/frontend-mobile/app/src/androidTest/java/pt/ipp/estg/g08/clinica/sistema/FluxosClienteSistemaTest.kt#L473) | RF047, RNF002 |

Execução, preparação, capturas de falhas e configuração da pipeline: [guia dos testes](../../../src/frontend-mobile/tests/sistema/README.md).

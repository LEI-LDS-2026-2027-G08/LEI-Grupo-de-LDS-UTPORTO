# I093 — Testes de Integração de Agenda e Área do Cliente

| Campo     | Valor                                                                     |
| --------- | ------------------------------------------------------------------------- |
| Issue     | I093 — Implementar testes de integração de agenda e área do Cliente (#28) |
| Sprint    | S05                                                                       |
| Milestone | M3                                                                        |
| Executor  | Gonçalo Magalhães Ribeiro                                                 |
| Revisor   | André Filipe Moreira Mesquita                                             |
| Auditor   | Ivan Cristiano Silva Mendes                                               |
| Data      | 2026-11-01                                                                |
| Estado    | Implementação do Executor concluída; em revisão                           |

## 1. Objetivo

Garantir que os casos da Especificação de Testes de Integração do Backend (I030) para a agenda e a área do Cliente — EP-021 a EP-030 (secção 11) e a concorrência das marcações (secção 15, IT-CONC-01 a IT-CONC-05) — estão implementados no projeto `ClinicaVeterinaria.IntegrationTests`, com o identificador do caso no nome do teste, a correr contra a API real em memória e um SQL Server isolado (infraestrutura da I091), e registar a correspondência entre cada caso e o teste (evidência 3).

Grande parte dos casos foi escrita pelas tarefas de implementação de cada endpoint (I071 a I076), à medida que o código foi integrado. Esta tarefa confirmou a cobertura caso a caso, implementou os casos em falta (IT-EP022-05, IT-EP022-06 e IT-CONC-01 a 05), alargou a EP-027 a todas as combinações inválidas da tabela de transições e acrescentou o teste de visibilidade da agenda por perfil (critério 4).

## 2. Documentos de referência

| Documento                                               | Versão                                            |
| ------------------------------------------------------- | ------------------------------------------------- |
| Backlog de Projeto                                      | v03 (I093, aprovada como Ready a 2026-10-27)      |
| Especificação de Testes de Integração do Backend (I030) | v01 (secções 11, 14 e 15)                         |
| Documentação da API                                     | v02 (EP-021 a EP-030)                             |
| Especificação de Requisitos                             | v02 (RF019 a RF029)                               |
| Declaração de Âmbito                                    | v05                                               |
| Plano de Testes do Backend                              | v02                                               |
| Infraestrutura dos testes de integração (I091)          | `m3-i091-infraestrutura-testes-integracao-v01.md` |

## 3. Ficheiros

Todos em `src/backend/ClinicaVeterinaria.IntegrationTests`:

| Ficheiro                                             | Sigla | Endpoints               | Origem                                                                  |
| ---------------------------------------------------- | ----- | ----------------------- | ----------------------------------------------------------------------- |
| `Veterinarios/VeterinariosEndpointsTests.cs`         | VET   | EP-021, EP-022          | I071; IT-EP022-05 e 06 acrescentados (I093)                             |
| `Consultas/ConsultasEndpointsTests.cs`               | AGE   | EP-023, EP-024          | I072, sem alterações                                                    |
| `Consultas/CriarConsultaEndpointsTests.cs`           | CRI   | EP-025                  | I073, sem alterações                                                    |
| `Consultas/AtualizarConsultaEndpointsTests.cs`       | ALT   | EP-026                  | I074, sem alterações                                                    |
| `Consultas/AtualizarEstadoConsultaEndpointsTests.cs` | EST   | EP-027                  | I074; IT-EP027-05, 07 e 08 alargados e identificadores completos (I093) |
| `Consultas/ChegadaENotasPreparacaoEndpointsTests.cs` | CHN   | EP-028, EP-029          | I075; só o identificador completo de IT-EP029-03 no nome (I093)         |
| `Consultas/MinhasConsultasEndpointsTests.cs`         | MIN   | EP-030                  | I076, sem alterações                                                    |
| `Consultas/ConcorrenciaMarcacoesEndpointsTests.cs`   | CON   | EP-024, EP-026 a EP-028 | Novo (I093): IT-CONC-01 a 05 e pedidos em paralelo (critério 3)         |
| `Consultas/VisibilidadeAgendaEndpointsTests.cs`      | VIS   | EP-023, EP-024          | Novo (I093): visibilidade por perfil (critério 4)                       |

Não há alterações ao código de produção nem ao esquema da base de dados.

## 4. Decisões de implementação

1. **Dados e independência.** Cada teste parte dos dados de demonstração da I061, repostos antes de cada teste (Respawn, I091). Os testes não dependem da ordem nem de outros testes: o que precisam de criar é criado no próprio teste (marcações por EP-025 ou alterações por EP-026 a EP-028).
2. **Relógio real.** A API usa o relógio real (`TimeProvider.System`). Os testes que criam ou movem marcações usam uma quarta-feira pelo menos 28 dias à frente, em hora de Lisboa, onde os dados de demonstração não têm marcações (mesma regra da I071, I073 e I074).
3. **Tabela de transições (critério 4).** As 16 combinações de origem e destino de EP-027 estão cobertas: 4 válidas (IT-EP027-01 a 04: 204 e estado persistido), 2 inválidas a partir de estados não finais (IT-EP027-07), 6 a partir dos estados finais (IT-EP027-08) e 4 com destino `Pendente` (IT-EP027-05, mais o estado inexistente `Faltou`). Cada transição recusada confirma o código, o formato do erro e que o estado e a `RowVersion` da marcação não mudaram (IT-TRV-26 e IT-TRV-28).
4. **Visibilidade da agenda por perfil (critério 4).** `VisibilidadePorPerfil` lê a agenda (EP-023) e a marcação M3 (EP-024) com cada conta: Rececionista e os dois Auxiliares (com e sem autorização de apoio) veem as 5 marcações; cada Veterinário só vê as suas (3 e 2) e o Dr. Bruno recebe 403 na M3, que é da Dra. Ana; Administrador e Cliente recebem 403; sem token, 401 (matriz da secção 14.1 da I030).
5. **Concorrência (critério 3).** Os IT-CONC-02 a 04 seguem a I030: dois clientes leem a mesma versão, o primeiro grava e o segundo recebe 412, sem sobrepor a alteração do primeiro. `PedidosEmParaleloComOMesmoIfMatch` envia os dois pedidos ao mesmo tempo (`Task.WhenAll`) em EP-026, EP-027 e EP-028: só um é aceite, o outro recebe 412, e a base fica com o efeito do pedido aceite e com a versão do ETag que lhe foi devolvido. A criação simultânea de marcações para a mesma vaga (8 pedidos em paralelo, só um 201) já estava coberta pela I073 (`PedidosSimultaneosParaAMesmaVaga`).
6. **Repetibilidade dos testes em paralelo.** A ordem em que o SQL Server executa os pedidos não é controlada. As verificações valem para qualquer ordem: com o mesmo `If-Match`, quem perde recebe sempre 412 (a versão já não corresponde, ou a gravação deteta a `RowVersion` alterada); sem `If-Match` (IT-CONC-05), recebe 412 (conflito de versão) ou 422 (já viu o estado gravado pelo outro pedido). Em nenhuma ordem os dois são aceites.
7. **Formato de erro.** Todos os erros são verificados no formato único da I058 (`{ code, message, detail }`), com `code` igual ao código HTTP, como na I092.
8. **Identificadores completos.** Os nomes que agrupavam casos de forma abreviada («IT-EP027-01 a 04», «IT-EP029-02 e 03») passam a ter os identificadores completos, para que `--filter "DisplayName~IT-EP027-03"` encontre o teste.

## 5. Correspondência caso da especificação ↔ teste

Legenda dos ficheiros: ver a coluna «Sigla» da secção 3.

### 5.1. EP-021 a EP-030 (I030, secção 11)

| Caso I030   | Ficheiro | Teste                                   | Estado                                           |
| ----------- | -------- | --------------------------------------- | ------------------------------------------------ |
| IT-EP021-01 | VET      | `RececionistaListaVeterinariosAtivos`   | Implementado (I071)                              |
| IT-EP022-01 | VET      | `DisponibilidadeDeUmDiaSemMarcacoes`    | Implementado (I071)                              |
| IT-EP022-02 | VET      | `PeriodoInvalidoDevolve400` (4 casos)   | Implementado (I071)                              |
| IT-EP022-03 | VET      | `PeriodoInvalidoDevolve400` (4 casos)   | Implementado (I071)                              |
| IT-EP022-04 | VET      | `VeterinarioInexistenteDevolve404`      | Implementado (I071)                              |
| IT-EP022-05 | VET      | `HorariosOcupadosNaoSaoLivres`          | Implementado (I093)                              |
| IT-EP022-06 | VET      | `MarcacaoCanceladaLibertaOHorario`      | Implementado (I093)                              |
| IT-EP023-01 | AGE      | `RececionistaConsultaAgenda`            | Implementado (I072)                              |
| IT-EP023-02 | AGE      | `FiltroInvalidoDevolve400` (4 casos)    | Implementado (I072)                              |
| IT-EP023-03 | AGE      | `VeterinarioVeSoAsSuas`                 | Implementado (I072)                              |
| IT-EP023-04 | AGE      | `VeterinarioIdDeOutroEIgnorado`         | Implementado (I072)                              |
| IT-EP023-05 | AGE      | `AuxiliarConsultaAgenda`                | Implementado (I072)                              |
| IT-EP024-01 | AGE      | `RececionistaLeMarcacaoComNotas`        | Implementado (I072)                              |
| IT-EP024-02 | AGE      | `VeterinarioAtribuidoLeMarcacao`        | Implementado (I072)                              |
| IT-EP024-03 | AGE      | `MarcacaoDeOutroVeterinarioDevolve403`  | Implementado (I072)                              |
| IT-EP024-04 | AGE      | `MarcacaoInexistenteDevolve404`         | Implementado (I072)                              |
| IT-EP024-05 | AGE      | `RespostaComETag`                       | Implementado (I072)                              |
| IT-EP025-01 | CRI      | `CriaMarcacaoPendente`                  | Implementado (I073)                              |
| IT-EP025-02 | CRI      | `CampoEmFaltaDevolve400` (3 casos)      | Implementado (I073)                              |
| IT-EP025-03 | CRI      | `DataNoPassadoDevolve400`               | Implementado (I073)                              |
| IT-EP025-04 | CRI      | `AnimalInexistenteDevolve404`           | Implementado (I073)                              |
| IT-EP025-05 | CRI      | `VeterinarioInexistenteDevolve404`      | Implementado (I073)                              |
| IT-EP025-06 | CRI      | `ForaDoHorarioDevolve422` (3 casos)     | Implementado (I073)                              |
| IT-EP025-07 | CRI      | `VeterinarioOcupadoDevolve422`          | Implementado (I073)                              |
| IT-EP025-08 | CRI      | `MarcacaoPersistida`                    | Implementado (I073)                              |
| IT-EP026-01 | ALT      | `AtualizacaoValida` (2 casos)           | Implementado (I074)                              |
| IT-EP026-02 | ALT      | `DadosInvalidosDevolvem400`             | Implementado (I074)                              |
| IT-EP026-03 | ALT      | `ConsultaInexistenteDevolve404`         | Implementado (I074)                              |
| IT-EP026-04 | ALT      | `IfMatchDesatualizadoDevolve412`        | Implementado (I074)                              |
| IT-EP026-05 | ALT      | `RealizadaDevolve422`                   | Implementado (I074)                              |
| IT-EP026-06 | ALT      | `CanceladaDevolve422`                   | Implementado (I074)                              |
| IT-EP026-07 | ALT      | `HorarioIndisponivelDevolve422`         | Implementado (I074)                              |
| IT-EP026-08 | ALT      | `IfMatchAtualEAceite`                   | Implementado (I074)                              |
| IT-EP027-01 | EST      | `TransicoesValidas` (4 casos)           | Implementado (I074)                              |
| IT-EP027-02 | EST      | `TransicoesValidas` (4 casos)           | Implementado (I074)                              |
| IT-EP027-03 | EST      | `TransicoesValidas` (4 casos)           | Implementado (I074)                              |
| IT-EP027-04 | EST      | `TransicoesValidas` (4 casos)           | Implementado (I074)                              |
| IT-EP027-05 | EST      | `EstadoInvalidoDevolve400` (5 casos)    | Implementado (I074); alargado na I093 (5 linhas) |
| IT-EP027-06 | EST      | `ConsultaInexistenteDevolve404`         | Implementado (I074)                              |
| IT-EP027-07 | EST      | `TransicaoInvalidaDevolve422` (2 casos) | Implementado (I074); alargado na I093 (2 linhas) |
| IT-EP027-08 | EST      | `EstadoFinalDevolve422` (6 casos)       | Implementado (I074); alargado na I093 (6 linhas) |
| IT-EP027-09 | EST      | `IfMatchAntigoDevolve412`               | Implementado (I074)                              |
| IT-EP027-10 | EST      | `IfMatchAtualDevolveNovoETag`           | Implementado (I074)                              |
| IT-EP028-01 | CHN      | `ChegadaValidaDevolve200`               | Implementado (I075)                              |
| IT-EP028-02 | CHN      | `ChegadaMarcacaoInexistenteDevolve404`  | Implementado (I075)                              |
| IT-EP028-03 | CHN      | `ChegadaIfMatchDesatualizadoDevolve412` | Implementado (I075)                              |
| IT-EP028-04 | CHN      | `ChegadaMarcacaoPendenteDevolve422`     | Implementado (I075)                              |
| IT-EP028-05 | CHN      | `ChegadaRepetidaDevolve422`             | Implementado (I075)                              |
| IT-EP029-01 | CHN      | `NotaValidaDevolve201` (2 casos)        | Implementado (I075)                              |
| IT-EP029-02 | CHN      | `TextoInvalidoDevolve400` (2 casos)     | Implementado (I075)                              |
| IT-EP029-03 | CHN      | `TextoInvalidoDevolve400` (2 casos)     | Implementado (I075)                              |
| IT-EP029-04 | CHN      | `AuxiliarSemApoioDevolve403`            | Implementado (I075)                              |
| IT-EP029-05 | CHN      | `NotaMarcacaoInexistenteDevolve404`     | Implementado (I075)                              |
| IT-EP029-06 | CHN      | `NotaMarcacaoCanceladaDevolve422`       | Implementado (I075)                              |
| IT-EP030-01 | MIN      | `FuturasSoDoCliente`                    | Implementado (I076)                              |
| IT-EP030-02 | MIN      | `HistoricoSoDoCliente`                  | Implementado (I076)                              |
| IT-EP030-03 | MIN      | `TipoInvalidoDevolve400` (2 casos)      | Implementado (I076)                              |
| IT-EP030-04 | MIN      | `AnimalProprioFiltra`                   | Implementado (I076)                              |
| IT-EP030-05 | MIN      | `AnimalDeOutroClienteDevolve403`        | Implementado (I076)                              |
| IT-EP030-06 | MIN      | `RespostaSemInformacaoClinica`          | Implementado (I076)                              |

**Resumo:** 60 casos da I030 para EP-021 a EP-030; 60 implementados; 0 não implementados.

### 5.2. Concorrência das marcações (I030, secção 15)

| Caso I030  | Ficheiro | Teste                                                                                     | Estado              |
| ---------- | -------- | ----------------------------------------------------------------------------------------- | ------------------- |
| IT-CONC-01 | CON      | `AlteracaoComETagAtual`                                                                   | Implementado (I093) |
| IT-CONC-02 | CON      | `AlteracaoComETagDesatualizado`, `PedidosEmParaleloComOMesmoIfMatch` (linha EP-026)       | Implementado (I093) |
| IT-CONC-03 | CON      | `MudancaDeEstadoComETagDesatualizado`, `PedidosEmParaleloComOMesmoIfMatch` (linha EP-027) | Implementado (I093) |
| IT-CONC-04 | CON      | `ChegadaComETagDesatualizado`, `PedidosEmParaleloComOMesmoIfMatch` (linha EP-028)         | Implementado (I093) |
| IT-CONC-05 | CON      | `GravacoesSimultaneasSemIfMatch` (EP-027 e EP-028)                                        | Implementado (I093) |

**Resumo:** 5 casos IT-CONC; 5 implementados.

### 5.3. Casos transversais da agenda e da área do Cliente (I030, secção 14)

| Caso I030 | Cenário                                     | Testes                                                                                         |
| --------- | ------------------------------------------- | ---------------------------------------------------------------------------------------------- |
| IT-TRV-10 | Veterinário tenta alterar marcação → 403    | ALT `OutrosPerfisRecebem403` (linha vet.ana); EST `OutrosPerfisRecebem403` (linha vet.ana)     |
| IT-TRV-13 | Cliente só vê as marcações dos seus animais | MIN `FuturasSoDoCliente`, `HistoricoSoDoCliente` (IT-EP030-01 e 02)                            |
| IT-TRV-14 | Cliente filtra por animal de outro → 403    | MIN `AnimalDeOutroClienteDevolve403` (IT-EP030-05)                                             |
| IT-TRV-26 | 400 sem alteração parcial                   | EST `EstadoInvalidoDevolve400`                                                                 |
| IT-TRV-28 | 422 preserva o estado anterior              | EST `TransicaoInvalidaDevolve422`, `EstadoFinalDevolve422`; CRI `VeterinarioOcupadoDevolve422` |

Os restantes casos transversais (IT-TRV-01 a 09, 11, 12, 15 a 25, 27 e 29 a 34) são da I096 (testes de segurança e isolamento por perfil) ou de outras áreas.

### 5.4. Critérios de aceitação da I093

| Critério                                                              | Como é cumprido                                                                                                                                                                                                                                            |
| --------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1. Casos da I030 implementados, com o identificador no nome           | 60 casos de EP-021 a EP-030 e 5 IT-CONC, todos com o identificador completo no `DisplayName` (secções 5.1 e 5.2).                                                                                                                                          |
| 2. API real em memória, SQL Server isolado, independente da ordem     | `WebApplicationFactory` com a API real e SQL Server em contentor (Testcontainers), base reposta antes de cada teste (Respawn e dados da I061); cada teste cria o que precisa (secção 4, decisão 1).                                                        |
| 3. Concorrência com pedidos em paralelo: só um aceite e base coerente | `PedidosEmParaleloComOMesmoIfMatch` (EP-026 a EP-028) e `GravacoesSimultaneasSemIfMatch` (IT-CONC-05); criação simultânea na I073 (`PedidosSimultaneosParaAMesmaVaga`). Verificam o número de pedidos aceites e o estado gravado na base (decisões 5 e 6). |
| 4. Um teste por transição inválida e para a visibilidade da agenda    | As 12 transições recusadas de EP-027, uma linha cada (decisão 3); `VisibilidadePorPerfil` com os cinco perfis e o pedido anónimo (decisão 4).                                                                                                              |
| 5. Sem falhas, resultados num formato legível pela pipeline           | Execução local na secção 7.1. A Backend CI (job «3 · Testes de integração (QG-02)») corre os testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`.                                                                               |

## 6. Divergências e notas

- **D1 — IT-CONC-05: 412 em vez de 409.** A I030 prevê `409 Conflict` quando a gravação deteta a `DbUpdateConcurrencyException`. A implementação (I074 e I075) devolve `412 Precondition Failed` (`PreconditionFailedException`), o mesmo código do `If-Match` desatualizado. O teste segue a implementação e aceita 412 ou 422 (decisão 6). A divergência já está registada na revisão da I075 (melhoria M2); a decisão fica para a Documentação da API v03.
- **D2 — formato do erro.** Como na I092 (D1), a I030 indica Problem Details; a implementação devolve o formato único da I058.
- **N1 — Definição da tarefa.** Esta tarefa segue a definição da I093 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog v02. A descrição da Issue #28 no GitHub é anterior a esse refinamento.
- **N2 — IT-CONC-05 em EP-026.** Sem `If-Match`, duas alterações de data e hora seguidas são as duas válidas (a segunda substitui a primeira): o `If-Match` é opcional no contrato. Por isso o IT-CONC-05 é verificado em EP-027 e EP-028, onde o segundo pedido não pode ser aceite.

## 7. Execução

### 7.1. Local

```powershell
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
```

| Execução      | Total | Aprovados | Falhados | Ignorados | Duração |
| ------------- | ----- | --------- | -------- | --------- | ------- |
| 1.ª           | 370   | 370       | 0        | 0         | 171,5 s |
| 2.ª (seguida) | 370   | 370       | 0        | 0         | 166,9 s |

Execuções feitas a 2026-11-01 sobre o ramo da I093, criado a partir de `develop` (`7f85c75`, com a I086), com a suite completa do projeto de testes de integração: os 27 testes novos ou alargados da I093 e os 343 já existentes. Linha de base antes da I093: 343 testes, 0 falhas. Só as classes de agenda, área do Cliente e concorrência (`Consultas` e `Veterinarios`): 144 testes, 144 aprovados (77,0 s).

### 7.2. Pipeline

O job «3 · Testes de integração (QG-02)» da «Backend CI» (`backend-ci.yml`) executa estes testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`.

Execução da Pull Request #224 (commit `0b4b493`), concluída com sucesso: [Backend CI](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41325848766) (11 verificações, incluindo «3 · Testes de integração (QG-02)»).

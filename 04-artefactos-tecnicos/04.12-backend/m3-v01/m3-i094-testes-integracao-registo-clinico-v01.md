# I094 — Testes de Integração de Registo Clínico

| Campo     | Valor                                                              |
| --------- | ------------------------------------------------------------------ |
| Issue     | I094 — Implementar testes de integração de registo clínico (#97)   |
| Sprint    | S05                                                                |
| Milestone | M3                                                                 |
| Executor  | André Filipe Moreira Mesquita                                      |
| Revisor   | Gonçalo Dinis Magalhães Teixeira                                   |
| Auditor   | Gonçalo Magalhães Ribeiro                                          |
| Data      | 2026-11-02                                                         |
| Estado    | Implementação do Executor concluída; em revisão                    |

## 1. Objetivo

Garantir que os casos da Especificação de Testes de Integração do Backend (I030) para o registo clínico — EP-031 a EP-034 (secção 12) — estão implementados no projeto `ClinicaVeterinaria.IntegrationTests`, com o identificador do caso no nome do teste, a correr contra a API real em memória e um SQL Server isolado (infraestrutura da I091), e registar a correspondência entre cada caso e o teste (evidência 3).

Os 22 casos foram escritos pelas tarefas de implementação dos endpoints (I077 e I078), à medida que o código foi integrado. Esta tarefa confirmou a cobertura caso a caso, completou o identificador abreviado de IT-EP032-04 e acrescentou os testes que os critérios 3 e 4 do Backlog de Projeto v03 pedem e que a I030 não tinha: a associação do registo à consulta e ao paciente vista de ponta a ponta (EP-031/EP-032 → EP-033 → EP-034 → base de dados) e a confirmação de que os perfis sem permissão de escrita não gravam nada.

## 2. Documentos de referência

| Documento                                               | Versão                                            |
| ------------------------------------------------------- | ------------------------------------------------- |
| Backlog de Projeto                                      | v03 (I094, aprovada como Ready a 2026-10-27)      |
| Especificação de Testes de Integração do Backend (I030) | v01 (secções 12 e 14)                             |
| Documentação da API                                     | v02 (EP-031 a EP-034)                             |
| Especificação de Requisitos                             | v02 (RF030 a RF036)                               |
| Plano de Testes do Backend                              | v02                                               |
| Infraestrutura dos testes de integração (I091)          | `m3-i091-infraestrutura-testes-integracao-v01.md` |

## 3. Ficheiros

Todos em `src/backend/ClinicaVeterinaria.IntegrationTests/RegistosClinicos`:

| Ficheiro                                     | Sigla | Endpoints      | Origem                                                                    |
| -------------------------------------------- | ----- | -------------- | ------------------------------------------------------------------------- |
| `RegistosClinicosEndpointsTests.cs`          | ESC   | EP-031, EP-032 | I077; só o identificador completo de IT-EP032-04 no nome (I094)           |
| `RegistosClinicosLeituraEndpointsTests.cs`   | LEI   | EP-033, EP-034 | I078, sem alterações                                                      |
| `RegistoClinicoPersistenciaEndpointsTests.cs` | PER   | EP-031 a EP-034 | Novo (I094): associação à consulta e ao paciente e perfis sem permissão |

Não há alterações ao código de produção nem ao esquema da base de dados.

## 4. Decisões de implementação

1. **Dados e independência.** Cada teste parte dos dados de demonstração da I061, repostos antes de cada teste (Respawn, I091): M1 (Bobi, Dra. Ana, `Realizada`, com registo clínico), M2 (Mia, Dr. Bruno, `Cancelada`), M3 (Kiko, Dra. Ana, `Confirmada`, sem registo) e M5 (Bobi, Dra. Ana, `Pendente`). O que um teste cria (um registo na M3, uma consulta de setembro no histórico do Bobi) é criado no próprio teste.
2. **Associação de ponta a ponta (critério 3).** `RegistoCriadoFicaNaConsultaENoPaciente` cria o registo na M3 e confirma a associação em três sítios: EP-033 devolve `consultaId` = M3 e `animalId` = Kiko; EP-034 do Kiko mostra o registo com a data da consulta e as contagens certas, e o histórico do Bobi não o mostra; na base, o registo aponta para a M3, a M3 para o Kiko e o tratamento e a prescrição para o registo. `AtualizacaoGravaElementosEMantemAssociacoes` atualiza notas, diagnóstico, um tratamento e uma prescrição existentes (pelo `id`) e acrescenta um tratamento, e confirma o mesmo depois da atualização.
3. **Perfis sem permissão (critério 4).** Além do 403 que a I077 e a I078 já verificavam, `PerfisSemPermissaoNaoGravamNada` confirma, para a Rececionista, o Administrador e o Cliente, que os pedidos de criação e de alteração recusados não deixam registos, tratamentos nem prescrições gravados e não mudam o diagnóstico do registo existente. O caso do Auxiliar Veterinário já estava coberto pelo IT-EP033-02 (`AuxiliarNaoAlteraRegisto`).
4. **Formato de erro.** Todos os erros são verificados no formato único da I058 (`{ code, message, detail }`), com `code` igual ao código HTTP, como na I092 e na I093.
5. **Identificadores completos.** O nome «IT-EP032-01 e 04» passa a «IT-EP032-01 e IT-EP032-04», para que `--filter "DisplayName~IT-EP032-04"` encontre o teste.

## 5. Correspondência caso da especificação ↔ teste

Legenda dos ficheiros: ver a coluna «Sigla» da secção 3.

### 5.1. EP-031 a EP-034 (I030, secção 12)

| Caso I030   | Ficheiro | Teste                                                     | Estado              |
| ----------- | -------- | --------------------------------------------------------- | ------------------- |
| IT-EP031-01 | ESC      | `CriaRegisto`                                             | Implementado (I077) |
| IT-EP031-02 | ESC      | `ConsultaIdEmFaltaDevolve400`                             | Implementado (I077) |
| IT-EP031-03 | ESC      | `SemElementoClinicoDevolve400`                            | Implementado (I077) |
| IT-EP031-04 | ESC      | `PrescricaoIncompletaDevolve400`                          | Implementado (I077) |
| IT-EP031-05 | ESC      | `ConsultaInexistenteDevolve404`                           | Implementado (I077) |
| IT-EP031-06 | ESC      | `ConsultaComRegistoDevolve409`                            | Implementado (I077) |
| IT-EP031-07 | ESC      | `ConsultaPendenteDevolve422`                              | Implementado (I077) |
| IT-EP031-08 | ESC      | `ConsultaCanceladaDevolve422`                             | Implementado (I077) |
| IT-EP031-09 | ESC      | `PedidosRejeitadosNaoGravamNada`                          | Implementado (I077) |
| IT-EP032-01 | ESC      | `AtualizaRegisto`                                         | Implementado (I077) |
| IT-EP032-02 | ESC      | `DadosInvalidosDevolvem400`                               | Implementado (I077) |
| IT-EP032-03 | ESC      | `RegistoInexistenteDevolve404`                            | Implementado (I077) |
| IT-EP032-04 | ESC      | `AtualizaRegisto`                                         | Implementado (I077); identificador completo (I094) |
| IT-EP032-05 | ESC      | `ListasFicamCoerentes`                                    | Implementado (I077) |
| IT-EP033-01 | LEI      | `VeterinarioConsultaRegisto`                              | Implementado (I078) |
| IT-EP033-02 | LEI      | `AuxiliarConsultaRegisto` (2 casos), `AuxiliarNaoAlteraRegisto` | Implementado (I078) |
| IT-EP033-03 | LEI      | `RegistoInexistenteDevolve404`                            | Implementado (I078) |
| IT-EP034-01 | LEI      | `HistoricoPorOrdemDescendente`                            | Implementado (I078) |
| IT-EP034-02 | LEI      | `HistoricoFiltradoPorIntervalo`                           | Implementado (I078) |
| IT-EP034-03 | LEI      | `IntervaloInvalidoDevolve400` (3 casos)                   | Implementado (I078) |
| IT-EP034-04 | LEI      | `AnimalInexistenteDevolve404`                             | Implementado (I078) |
| IT-EP034-05 | LEI      | `AnimalSemHistorico`                                      | Implementado (I078) |

**Resumo:** 22 casos da I030 para EP-031 a EP-034; 22 implementados; 0 não implementados.

### 5.2. Testes além da I030

| Teste                                         | Ficheiro | Origem | O que verifica                                                                                       |
| --------------------------------------------- | -------- | ------ | ---------------------------------------------------------------------------------------------------- |
| `RegistoCriadoFicaNaConsultaENoPaciente`      | PER      | I094   | Associação do registo criado à consulta e ao paciente em EP-033, EP-034 e na base (critério 3)       |
| `AtualizacaoGravaElementosEMantemAssociacoes` | PER      | I094   | Persistência de notas, diagnóstico, tratamentos e prescrições na atualização e associações mantidas (critério 3) |
| `PerfisSemPermissaoNaoGravamNada` (3 casos)   | PER      | I094   | Rececionista, Administrador e Cliente: 403 na criação e na alteração, sem nada gravado (critério 4)  |
| `ConsultaDeOutroVeterinarioDevolve403`        | ESC      | I077   | Só o Veterinário da consulta cria o registo                                                          |
| `RegistoDeOutroVeterinarioDevolve403`         | ESC      | I077   | Só o Veterinário do registo o altera                                                                 |
| `PedidosSimultaneosParaAMesmaConsulta`        | ESC      | I077   | 5 pedidos em paralelo para a mesma consulta: um 201, quatro 409 e um só registo                      |
| `OutrosPerfisRecebem403` (4 casos)            | ESC      | I077   | 403 em EP-031 e EP-032 para Rececionista, Auxiliar, Administrador e Cliente                          |
| `SemTokenDevolve401`                          | ESC      | I077   | 401 em EP-031 e EP-032 sem token                                                                     |
| `VeterinarioConsultaRegistoDeColega`          | LEI      | I078   | Um Veterinário lê o registo de outro Veterinário do mesmo paciente                                    |
| `AuxiliarConsultaHistorico`                   | LEI      | I078   | O Auxiliar lê o mesmo histórico que o Veterinário                                                    |
| `OutrosPerfisRecebem403` (3 casos)            | LEI      | I078   | 403 em EP-033 e EP-034 para Rececionista, Administrador e Cliente                                    |
| `SemTokenDevolve401`                          | LEI      | I078   | 401 em EP-033 e EP-034 sem token                                                                     |

### 5.3. Casos transversais relacionados (I030, secção 14)

| Caso I030 | Cenário                                         | Testes                                                                                     |
| --------- | ----------------------------------------------- | ------------------------------------------------------------------------------------------ |
| IT-TRV-07 | Administrador tenta aceder a registo clínico    | ESC e LEI `OutrosPerfisRecebem403` (linha admin); PER `PerfisSemPermissaoNaoGravamNada` (linha admin) |
| IT-TRV-08 | Rececionista tenta criar/alterar registo clínico | ESC `OutrosPerfisRecebem403` (linha rececao); PER `PerfisSemPermissaoNaoGravamNada` (linha rececao) |
| IT-TRV-09 | Auxiliar tenta alterar registo clínico           | LEI `AuxiliarNaoAlteraRegisto`; ESC `OutrosPerfisRecebem403` (linha aux.carla)              |

Os casos IT-TRV são da I096 (testes de segurança e isolamento por perfil); ficam aqui só para rastreabilidade.

### 5.4. Critérios de aceitação da I094

| Critério                                                                                    | Como é cumprido |
| ------------------------------------------------------------------------------------------- | --------------- |
| 1. Casos da I030 implementados, com o identificador no nome                                 | 22 de 22, todos com o identificador completo no `DisplayName` (secção 5.1). |
| 2. API real em memória, SQL Server isolado, independente da ordem                           | `WebApplicationFactory` com a API real e SQL Server em contentor (Testcontainers), base reposta antes de cada teste (Respawn e dados da I061); cada teste cria o que precisa (decisão 1). |
| 3. Criação e atualização de notas, diagnósticos, tratamentos e prescrições, com persistência e associação à consulta e ao paciente | IT-EP031-01, IT-EP032-01, 04 e 05 e os dois testes novos da I094 (decisão 2). |
| 4. Histórico pelo Veterinário, consulta autorizada pelo Auxiliar e rejeição de escritas sem permissão | IT-EP033-01 e 02, IT-EP034-01 a 05, `AuxiliarConsultaHistorico`, `AuxiliarNaoAlteraRegisto`, `OutrosPerfisRecebem403` (ESC e LEI) e `PerfisSemPermissaoNaoGravamNada` (decisão 3). |
| 5. Dados inválidos, inexistentes e acessos não autorizados conforme a API v02               | 400 (IT-EP031-02 a 04, IT-EP032-02, IT-EP034-03), 404 (IT-EP031-05, IT-EP032-03, IT-EP033-03, IT-EP034-04), 409 (IT-EP031-06), 422 (IT-EP031-07 e 08), 403 e 401 (secção 5.2). |
| 6. Sem falhas, resultados num formato legível pela pipeline                                 | Execução local na secção 7.1. A Backend CI (job «3 · Testes de integração (QG-02)») corre os testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`. |

## 6. Divergências e notas

- **D1 — formato do erro.** Como na I092 e na I093, a I030 indica Problem Details; a implementação devolve o formato único da I058.
- **N1 — Definição da tarefa.** Esta tarefa segue a definição da I094 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog v02. A descrição da Issue #97 no GitHub («registo clínico e de contas») é anterior a esse refinamento: as contas (F005) estão fora do âmbito pela Declaração de Âmbito v05.
- **N2 — Prazo.** A entrega é posterior ao limite do Executor do Sprint Backlog v02 (2026-11-01 17:30); a I078, dependência da I094, foi integrada em `develop` a 2026-11-01 às 17:55.

## 7. Execução

### 7.1. Local

```powershell
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
```

| Execução                                                | Total | Aprovados | Falhados | Ignorados | Duração |
| ------------------------------------------------------- | ----- | --------- | -------- | --------- | ------- |
| Pasta `RegistosClinicos` (`FullyQualifiedName~RegistosClinicos`) | 44    | 44        | 0        | 0         | 55,7 s  |
| Suite completa, 1.ª                                     | 393   | 393       | 0        | 0         | 318,3 s |
| Suite completa, 2.ª (seguida)                           | 393   | 393       | 0        | 0         | 312,2 s |

Execuções feitas a 2026-11-02 sobre o ramo da I094, criado a partir de `develop` (`8bff1a4`, com a I078, a I093 e a I100), com o SQL Server 2025 em Docker (Testcontainers). Linha de base antes da I094: 388 testes de integração (39 na pasta `RegistosClinicos`); a I094 acrescenta 5 execuções (2 testes e 1 teste com 3 casos), todas na pasta `RegistosClinicos`. Compilação da solução sem avisos nem erros.

### 7.2. Pipeline

O job «3 · Testes de integração (QG-02)» da «Backend CI» (`backend-ci.yml`) executa estes testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`. O resultado da execução sobre a Pull Request fica nas verificações da própria PR («Resultado da pipeline»).

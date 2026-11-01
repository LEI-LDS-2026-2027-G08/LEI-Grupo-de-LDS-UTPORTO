# I088 — Testes Unitários de Registo Clínico

| Campo     | Valor                                                        |
| --------- | ------------------------------------------------------------ |
| Issue     | I088 — Implementar testes unitários de registo clínico (#95) |
| Sprint    | S05                                                          |
| Milestone | M3                                                           |
| Executor  | Gonçalo Magalhães Ribeiro                                    |
| Revisor   | André Filipe Moreira Mesquita                                |
| Auditor   | Ivan Cristiano Silva Mendes                                  |
| Data      | 2026-11-01                                                   |
| Estado    | Implementação do Executor concluída; em revisão              |

## 1. Objetivo

Garantir que os 27 casos da funcionalidade F004 (registo clínico e tratamentos) da Especificação de Testes Unitários do Backend (I028, secção 7.4) estão implementados no projeto `ClinicaVeterinaria.UnitTests`, com o identificador do caso no nome do teste, sem base de dados real, rede nem relógio real, e registar a correspondência entre cada caso e o teste que o implementa (evidência 3).

A tarefa segue a definição aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03): testes unitários da F004 — criação e atualização do registo clínico, notas, diagnósticos, tratamentos, prescrições, associação à consulta, autoria e projeção autorizada para o Auxiliar.

Os casos da I028 foram escritos pelas tarefas de implementação (I077 e I078), à medida que o código foi integrado. Esta tarefa confirmou a cobertura caso a caso, reforçou os três casos de validação que só verificavam a exceção, acrescentou a fronteira dos 1000 caracteres do diagnóstico, os casos de atualização e de associação pedidos pelos critérios 3 e 4 do v03 e os testes da projeção do registo clínico, e regista a execução local e na pipeline.

## 2. Documentos de referência

| Documento                                           | Versão                                                        |
| --------------------------------------------------- | ------------------------------------------------------------- |
| Backlog de Projeto                                  | v03 (I088, aprovada como Ready a 2026-10-27)                  |
| Especificação de Testes Unitários do Backend (I028) | v01 (`m3-especificacao-testes-unitarios-backend-v01.md`, 7.4) |
| Documentação da API                                 | v02 (EP-031 a EP-034; ponto em aberto 4)                      |
| Especificação de Requisitos                         | v02 (RF030 a RF036)                                           |
| Plano de Testes do Backend                          | v02                                                           |
| Declaração de Âmbito                                | v05                                                           |

## 3. Ficheiros de teste

Todos em `src/backend/ClinicaVeterinaria.UnitTests/RegistosClinicos`.

| Ficheiro                                 | Casos UT-F004                        | Origem                                                          |
| ---------------------------------------- | ------------------------------------ | --------------------------------------------------------------- |
| `RegistosClinicosServiceTests.cs`        | 21 (UT-F004-001 a 021)               | I077; UT-F004-009, 011 e 012 reforçados e 5 testes novos (I088) |
| `RegistosClinicosLeituraServiceTests.cs` | 6 (UT-F004-022 a 027)                | I078                                                            |
| `ProjecaoRegistoClinicoTests.cs`         | — (critério 5)                       | Novo (I088)                                                     |
| `RegistosClinicosControllerTests.cs`     | — (critérios 4 e 5: perfis por ação) | I077 e I078, sem alterações                                     |

Total: 27 métodos de teste com identificador UT-F004, que dão 30 execuções (UT-F004-005 tem 2 linhas `[InlineData]` e UT-F004-008 tem 3), para os 27 casos. Com o teste da fronteira de UT-F004-012 acrescentado pela I088, que também tem o identificador no nome, o filtro `DisplayName~UT-F004` executa 31 testes. Não há alterações ao código de produção.

## 4. Alterações feitas nesta tarefa

- **UT-F004-009, 011 e 012** só verificavam que era lançada uma `ArgumentException`: passariam com qualquer erro de validação, mesmo noutro campo. Passam a verificar a mensagem exata do campo (`duracaoDias`, `descricao` do tratamento e `diagnostico`), que é o único erro de cada pedido, e que nada foi gravado (um só motivo de falha).
- **Fronteira de UT-F004-012:** «I088 — diagnóstico com exatamente 1000 caracteres é aceite». A I028 só especifica o lado inválido (1001).
- **Critério 3 do v03** (atualização com dados válidos e inválidos): «a atualização grava as notas clínicas e o diagnóstico novos», «atualização com um tratamento sem descrição devolve 400 e o registo fica como estava» e «atualização com uma prescrição sem dosagem devolve 400 e a prescrição fica como estava». A I028 só tinha casos inválidos de tratamentos e prescrições na criação.
- **Critério 4 do v03** (associação): «depois da atualização, o registo, os tratamentos e as prescrições continuam na mesma consulta e no mesmo paciente».
- **Critério 5 do v03:** novo `ProjecaoRegistoClinicoTests` (secção 5).

## 5. Verificação dos critérios de aceitação

Critérios da I088 no Backlog de Projeto v03.

| Critério                                                                                                       | Como é cumprido                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| -------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| 1. Casos da I028 aplicáveis ao registo clínico implementados, com o identificador no nome                      | Os 27 casos UT-F004-001 a 027 (secção 7.4 da I028) estão implementados, cada um num método com o identificador completo no `DisplayName` (I028, 4.3); nenhum fica por implementar (secção 6).                                                                                                                                                                                                                                                                                                                                                    |
| 2. Sem base de dados, rede nem relógio real; duplos quando necessário                                          | O `RegistosClinicosService` usa `ClinicaDbContext` com o fornecedor EF Core InMemory (`AjudaBaseDados`), a opção proposta pela I028 (4.2; PA-14), e recebe a hora através de `TimeProvider` (`RelogioFixo`, D-07). Não há chamadas de rede.                                                                                                                                                                                                                                                                                                      |
| 3. Registo e atualização de notas, diagnósticos, tratamentos e prescrições, com dados válidos e inválidos      | **Criação, válidos:** UT-F004-001 a 004 e 010. **Criação, inválidos:** UT-F004-005 a 017. **Atualização, válidos:** UT-F004-018, «a atualização grava as notas clínicas e o diagnóstico novos» e «uma prescrição existente mantém-se indicando o id e é atualizada» (I077). **Atualização, inválidos:** UT-F004-019 a 021, «atualização com um tratamento sem descrição…», «atualização com uma prescrição sem dosagem…» (I088), «o mesmo id de tratamento repetido no pedido devolve 400» (I077). Fronteira dos 1000 caracteres do diagnóstico. |
| 4. Os elementos clínicos ficam associados à mesma consulta e ao mesmo paciente; só o Veterinário cria e altera | **Associação:** UT-F004-001 (consulta e animal do registo criado), UT-F004-018 (`MarcacaoId` inalterado), «depois da atualização, o registo, os tratamentos e as prescrições continuam na mesma consulta e no mesmo paciente» (I088) e UT-F004-013 (um só registo por consulta). **Só o Veterinário:** UT-F004-017 (consulta de outro Veterinário: 403), «registo de outro Veterinário devolve 403 e não é alterado» (I077) e, no controlador, «EP-031 e EP-032 exigem também a política Veterinário» (`RegistosClinicosControllerTests`).       |
| 5. A projeção disponibilizada ao Auxiliar Veterinário não concede operações de edição                          | `RegistosClinicosControllerTests`: «o Auxiliar Veterinário só tem operações de leitura (GET) neste controlador» e «EP-031 e EP-032 exigem também a política Veterinário (o Auxiliar recebe 403)» (I077 e I078). `ProjecaoRegistoClinicoTests` (I088) fixa os campos da projeção (EP-033 e EP-034), confirma que o resumo do histórico não tem o detalhe clínico e que a leitura não depende do perfil (contrato v02, UC13 e PA-08 da I028). Leitura pelo Auxiliar: UT-F004-022 e, na integração, IT-EP033-02.                                    |
| 6. Sem falhas e sem testes ignorados sem justificação                                                          | Execução na secção 8. O único teste ignorado do projeto é o UT-TRV-007 (I086), com a justificação no próprio teste (defeito D-01, PA-07 da I028); não é da F004.                                                                                                                                                                                                                                                                                                                                                                                 |

## 6. Correspondência caso da especificação ↔ teste

| Caso        | Tipo      | EP     | Teste                                                                                                                                           | Origem                  |
| ----------- | --------- | ------ | ----------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------- |
| UT-F004-001 | Positivo  | EP-031 | `RegistosClinicosServiceTests.CriaRegistoCompleto`                                                                                              | I077                    |
| UT-F004-002 | Fronteira | EP-031 | `RegistosClinicosServiceTests.SoPrescricaoEAceite`                                                                                              | I077                    |
| UT-F004-003 | Fronteira | EP-031 | `RegistosClinicosServiceTests.SoTratamentoEAceite`                                                                                              | I077                    |
| UT-F004-004 | Fronteira | EP-031 | `RegistosClinicosServiceTests.ConsultaRealizadaEAceite`                                                                                         | I077                    |
| UT-F004-005 | Negativo  | EP-031 | `RegistosClinicosServiceTests.ConsultaIdInvalidoDevolve400` (2 linhas)                                                                          | I077                    |
| UT-F004-006 | Negativo  | EP-031 | `RegistosClinicosServiceTests.SemElementoClinicoDevolve400`                                                                                     | I077                    |
| UT-F004-007 | Fronteira | EP-031 | `RegistosClinicosServiceTests.NotasSoComEspacosDevolve400`                                                                                      | I077                    |
| UT-F004-008 | Negativo  | EP-031 | `RegistosClinicosServiceTests.PrescricaoIncompletaDevolve400` (3 linhas)                                                                        | I077                    |
| UT-F004-009 | Fronteira | EP-031 | `RegistosClinicosServiceTests.DuracaoZeroDevolve400`                                                                                            | I077; reforçado na I088 |
| UT-F004-010 | Positivo  | EP-031 | `RegistosClinicosServiceTests.PrescricaoSemDuracaoEAceite`                                                                                      | I077                    |
| UT-F004-011 | Negativo  | EP-031 | `RegistosClinicosServiceTests.TratamentoSemDescricaoDevolve400`                                                                                 | I077; reforçado na I088 |
| UT-F004-012 | Fronteira | EP-031 | `RegistosClinicosServiceTests.DiagnosticoDemasiadoLongoDevolve400`; fronteira dos 1000 caracteres: `DiagnosticoCom1000CaracteresEAceite` (I088) | I077; reforçado na I088 |
| UT-F004-013 | Negativo  | EP-031 | `RegistosClinicosServiceTests.SegundoRegistoDevolve409`                                                                                         | I077                    |
| UT-F004-014 | Negativo  | EP-031 | `RegistosClinicosServiceTests.ConsultaPendenteDevolve422`                                                                                       | I077                    |
| UT-F004-015 | Negativo  | EP-031 | `RegistosClinicosServiceTests.ConsultaCanceladaDevolve422`                                                                                      | I077                    |
| UT-F004-016 | Negativo  | EP-031 | `RegistosClinicosServiceTests.ConsultaInexistenteDevolve404`                                                                                    | I077                    |
| UT-F004-017 | Negativo  | EP-031 | `RegistosClinicosServiceTests.ConsultaDeOutroVeterinarioDevolve403`                                                                             | I077                    |
| UT-F004-018 | Positivo  | EP-032 | `RegistosClinicosServiceTests.AtualizaListasERegistaAutor`                                                                                      | I077                    |
| UT-F004-019 | Negativo  | EP-032 | `RegistosClinicosServiceTests.TratamentoDeOutroRegistoDevolve400`                                                                               | I077                    |
| UT-F004-020 | Negativo  | EP-032 | `RegistosClinicosServiceTests.AtualizacaoVaziaDevolve400`                                                                                       | I077                    |
| UT-F004-021 | Negativo  | EP-032 | `RegistosClinicosServiceTests.RegistoInexistenteDevolve404`                                                                                     | I077                    |
| UT-F004-022 | Positivo  | EP-033 | `RegistosClinicosLeituraServiceTests.ObterDevolveRegistoCompleto`                                                                               | I078                    |
| UT-F004-023 | Positivo  | EP-034 | `RegistosClinicosLeituraServiceTests.HistoricoPorOrdemDescendente`                                                                              | I078                    |
| UT-F004-024 | Positivo  | EP-034 | `RegistosClinicosLeituraServiceTests.ResumoContaTratamentosEPrescricoes`                                                                        | I078                    |
| UT-F004-025 | Fronteira | EP-034 | `RegistosClinicosLeituraServiceTests.HistoricoFiltradoPorUmDia`                                                                                 | I078                    |
| UT-F004-026 | Negativo  | EP-034 | `RegistosClinicosLeituraServiceTests.IntervaloInvalidoDevolve400`                                                                               | I078                    |
| UT-F004-027 | Negativo  | EP-034 | `RegistosClinicosLeituraServiceTests.AnimalInexistenteDevolve404`                                                                               | I078                    |

Distribuição: 6 positivos, 14 negativos e 7 de fronteira, como na secção 10.4 da I028. Casos não implementados: nenhum.

## 7. Divergências e notas

| Ref. | Caso                              | Divergência ou nota                                                                                                                                                                                                                               | Situação                                                                  |
| ---- | --------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| D1   | UT-F004-005 a 012, 019, 020 e 026 | A I028 descreve o resultado como «400». O teste unitário verifica a `ArgumentException`, que o `ExceptionHandlingMiddleware` converte em 400; o código HTTP é verificado nos testes de integração da I077 e da I078.                              | Sem impacto: é o padrão de todos os casos 400 da I028 (como na I087, D2). |
| D2   | UT-F004-022                       | A I028 indica «autenticado `AUX_SEM_APOIO`». O serviço não recebe o utilizador (secção 5, critério 5): o acesso do Auxiliar a EP-033 é verificado no controlador (`RegistosClinicosControllerTests`, I078) e nos testes de integração (IT-EP033). | Sem impacto.                                                              |
| N1   | —                                 | Esta tarefa segue a definição da I088 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog v02. A descrição da Issue #95 no GitHub é anterior a esse refinamento.                                              | Atualizar a descrição da Issue #95 a partir do Backlog de Projeto v03.    |
| N2   | —                                 | A I088 depende da I078, integrada em `develop` a 2026-11-01 17:55, depois do limite do Executor do Sprint Backlog v02 (2026-11-01 17:30). A tarefa só pôde começar a partir dessa hora.                                                           | Registado para a auditoria (prazos).                                      |

## 8. Execução

### 8.1. Local

| Execução                                                         | Total | Aprovados | Falhados | Ignorados | Duração |
| ---------------------------------------------------------------- | ----- | --------- | -------- | --------- | ------- |
| Todos os testes unitários                                        | 710   | 709       | 0        | 1         | 2,7 s   |
| Só UT-F004 (`--filter "DisplayName~UT-F004"`)                    | 31    | 31        | 0        | 0         | 1,4 s   |
| Pasta `RegistosClinicos` (`FullyQualifiedName~RegistosClinicos`) | 70    | 70        | 0        | 0         | 1,7 s   |

Execução a 2026-11-01, com o .NET 10, em Release, no ramo da I088 criado a partir de `develop` (`16c4bbb`, com a I078), com o script `testar-i088.ps1` (resultados TRX). O teste ignorado é o UT-TRV-007 (I086), com a justificação no próprio teste (defeito D-01, PA-07 da I028). Os 31 testes UT-F004 são as 30 execuções dos 27 casos e o teste da fronteira de UT-F004-012; a pasta `RegistosClinicos` inclui os 5 testes novos dos critérios 3 e 4 e os 4 da projeção (critério 5).

### 8.2. Pipeline

O job «2 · Testes unitários (QG-02)» da «Backend CI» (`backend-ci.yml`) corre os testes unitários com `--logger trx` e publica o artefacto `resultados-testes-unitarios`.

Execução da Pull Request #226 (commit `80ccbfe`), concluída com sucesso: [Backend CI](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41337018798).

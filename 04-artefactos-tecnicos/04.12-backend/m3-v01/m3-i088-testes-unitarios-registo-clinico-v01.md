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

Os testes foram escritos pelas tarefas de implementação (I077 e I078), à medida que o código foi integrado. Esta tarefa confirmou a cobertura caso a caso, reforçou os três casos de validação que só verificavam a exceção (secção 4), acrescentou a fronteira dos 1000 caracteres do diagnóstico e os testes da projeção do registo clínico (critério 5), e regista a execução local e na pipeline.

## 2. Documentos de referência

| Documento                                           | Versão                                                        |
| --------------------------------------------------- | ------------------------------------------------------------- |
| Especificação de Testes Unitários do Backend (I028) | v01 (`m3-especificacao-testes-unitarios-backend-v01.md`, 7.4) |
| Documentação da API                                 | v02 (EP-031 a EP-034; ponto em aberto 4)                      |
| Especificação de Requisitos                         | v02 (RF030 a RF036)                                           |
| Plano de Testes do Backend                          | v02                                                           |
| Declaração de Âmbito                                | v05                                                           |

## 3. Ficheiros de teste

Todos em `src/backend/ClinicaVeterinaria.UnitTests/RegistosClinicos`.

| Ficheiro                                 | Casos UT-F004          | Origem                                                         |
| ---------------------------------------- | ---------------------- | -------------------------------------------------------------- |
| `RegistosClinicosServiceTests.cs`        | 21 (UT-F004-001 a 021) | I077; UT-F004-009, 011 e 012 reforçados e um teste novo (I088) |
| `RegistosClinicosLeituraServiceTests.cs` | 6 (UT-F004-022 a 027)  | I078                                                           |
| `ProjecaoRegistoClinicoTests.cs`         | — (critério 5)         | Novo (I088)                                                    |
| `RegistosClinicosControllerTests.cs`     | — (perfis por ação)    | I077 e I078, sem alterações                                    |

Total: 27 métodos de teste com identificador UT-F004, que dão 30 execuções (UT-F004-005 tem 2 linhas `[InlineData]` e UT-F004-008 tem 3), para os 27 casos. Com o teste da fronteira de UT-F004-012 acrescentado pela I088, que também tem o identificador no nome, o filtro `DisplayName~UT-F004` executa 31 testes. Não há alterações ao código de produção.

## 4. Alterações feitas nesta tarefa

- **UT-F004-009, 011 e 012** só verificavam que era lançada uma `ArgumentException`: passariam com qualquer erro de validação, mesmo noutro campo. Passam a verificar a mensagem exata do campo (`duracaoDias`, `descricao` do tratamento e `diagnostico`), que é o único erro de cada pedido, e que nada foi gravado (um só motivo de falha, critério 3).
- **Fronteira de UT-F004-012:** novo teste «I088 — diagnóstico com exatamente 1000 caracteres é aceite». A I028 só especifica o lado inválido (1001).
- **Critério 5:** novo `ProjecaoRegistoClinicoTests` (secção 5, critério 5).

## 5. Verificação dos critérios de aceitação

| Critério                                                       | Como é cumprido                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| -------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1. Casos aplicáveis implementados, com o identificador no nome | Os 27 casos UT-F004-001 a 027 (secção 7.4 da I028) estão implementados, cada um num método com o identificador completo no `DisplayName` (I028, 4.3); nenhum fica por implementar (secção 6).                                                                                                                                                                                                                                                                                                                                                                                               |
| 2. Sem base de dados, rede nem relógio real                    | O `RegistosClinicosService` usa `ClinicaDbContext` com o fornecedor EF Core InMemory (`AjudaBaseDados`), a opção proposta pela I028 (4.2; PA-14), e recebe a hora através de `TimeProvider` (`RelogioFixo`, D-07). Não há chamadas de rede.                                                                                                                                                                                                                                                                                                                                                 |
| 3. Preparar–agir–verificar, um motivo de falha, dados mínimos  | Verificado por leitura dos dois ficheiros: três blocos separados, dados de referência da I028 (secção 5: `VET_A`, `VET_B`, `ANIMAL_A1`, consultas em cada estado, `REGISTO_CLINICO_A`). Os casos 009, 011 e 012 passam a ter um só motivo de falha (secção 4).                                                                                                                                                                                                                                                                                                                              |
| 4. Cálculos monetários com valores decimais                    | **Não aplicável.** Os cálculos monetários eram da F005 (contas e pagamentos), que saiu do âmbito na Declaração de Âmbito v05. A I028 não tem casos para a F005 (secção 3.2) e regista a atualização da descrição da I088 como ponto em aberto PA-01. O Sprint Backlog do S05 já designa a tarefa como «Implementar testes unitários de registo clínico». A F004 não tem valores monetários.                                                                                                                                                                                                 |
| 5. Projeção do Auxiliar Veterinário sem os campos excluídos    | Pelo contrato v02 (EP-033), pelo UC13 e pelo PA-08 da I028, o Auxiliar lê o registo completo, o mesmo do Veterinário (UT-F004-022): não há campos excluídos no registo. `ProjecaoRegistoClinicoTests` fixa os campos de cada DTO, confirma que o resumo do histórico (EP-034) não tem o detalhe clínico e que a leitura não depende do perfil. Se o grupo aprovar a projeção reduzida (ponto em aberto 4 da API), estes testes falham até a nova projeção ser testada. Na agenda, a ausência de campos clínicos na vista do Auxiliar já está garantida pelo `ProjecaoConsultaTests` (I072). |
| 6. Sem falhas e sem testes ignorados sem justificação          | Execução na secção 8. O único teste ignorado do projeto é o UT-TRV-007 (I086), com a justificação no próprio teste (defeito D-01, PA-07 da I028); não é da F004.                                                                                                                                                                                                                                                                                                                                                                                                                            |

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
| N1   | —                                 | Contas e pagamentos (F005) fora do âmbito: ver critério 4.                                                                                                                                                                                        | PA-01 da I028; atualizar a descrição da I088 no Backlog Refinement.       |
| N2   | —                                 | A I088 depende da I078, integrada em `develop` a 2026-11-01 17:55, depois do limite do Executor do Sprint Backlog v02 (2026-11-01 17:30). A tarefa só pôde começar a partir dessa hora.                                                           | Registado para a auditoria (prazos).                                      |

## 8. Execução

### 8.1. Local

| Execução                                                         | Total | Aprovados | Falhados | Ignorados | Duração |
| ---------------------------------------------------------------- | ----- | --------- | -------- | --------- | ------- |
| Todos os testes unitários                                        | 706   | 705       | 0        | 1         | 9,0 s   |
| Só UT-F004 (`--filter "DisplayName~UT-F004"`)                    | 31    | 31        | 0        | 0         | 1,5 s   |
| Pasta `RegistosClinicos` (`FullyQualifiedName~RegistosClinicos`) | 66    | 66        | 0        | 0         | 1,5 s   |

Execução a 2026-11-01, com o .NET 10, em Release, no ramo da I088 criado a partir de `develop` (`16c4bbb`, com a I078), com o script `testar-i088.ps1` (resultados TRX). O teste ignorado é o UT-TRV-007 (I086), com a justificação no próprio teste (defeito D-01, PA-07 da I028). Os 31 testes UT-F004 são as 30 execuções dos 27 casos e o teste da fronteira de UT-F004-012.

### 8.2. Pipeline

O job «2 · Testes unitários (QG-02)» da «Backend CI» (`backend-ci.yml`) corre os testes unitários com `--logger trx` e publica o artefacto `resultados-testes-unitarios`.

Execução da Pull Request #<N.º DA PR> (commit `<COMMIT>`): <LIGAÇÃO DA EXECUÇÃO>.

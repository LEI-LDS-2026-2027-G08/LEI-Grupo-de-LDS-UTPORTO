# I089 — Testes Unitários de Vacinação e Desparasitação

| Campo     | Valor                                                                         |
| --------- | ----------------------------------------------------------------------------- |
| Issue     | I089 — Implementar testes unitários de vacinação e desparasitação (#142)      |
| Sprint    | S05                                                                           |
| Milestone | M3                                                                            |
| Executor  | André Filipe Moreira Mesquita                                                 |
| Revisor   | Gonçalo Dinis Magalhães Teixeira                                              |
| Auditor   | Ivan Cristiano Silva Mendes                                                   |
| Data      | 2026-11-02                                                                    |
| Estado    | Implementação do Executor concluída; em revisão                               |

## 1. Objetivo

Garantir que os casos da Especificação de Testes Unitários do Backend (I028) para a F006 — UT-F006-001 a UT-F006-024 (secção 7.5) — estão implementados no projeto `ClinicaVeterinaria.UnitTests`, com o identificador do caso no nome do teste, sem base de dados real, rede nem relógio real, e registar a correspondência entre cada caso e o teste (evidência 3).

Os 24 casos foram escritos na I082, à medida que o boletim foi implementado. Esta tarefa confirmou a cobertura caso a caso, completou os identificadores abreviados e acrescentou os testes que o critério de aceitação do Backlog de Projeto v03 pede e que a I028 não tinha: vacinação sem intervalo e desparasitação com intervalo, recálculo na atualização nos casos de calendário, uma desparasitação que passa a ter próxima administração e a confirmação de que o 403 na atualização não altera o registo.

## 2. Documentos de referência

| Documento                                           | Versão                                       |
| --------------------------------------------------- | -------------------------------------------- |
| Backlog de Projeto                                  | v03 (I089, aprovada como Ready a 2026-10-27) |
| Especificação de Testes Unitários do Backend (I028) | v01 (secções 4, 5 e 7.5)                     |
| Documentação da API                                 | v02 (EP-042 a EP-044)                        |
| Especificação de Requisitos                         | v02 (RF043 a RF047)                          |
| Plano de Testes do Backend                          | v02                                          |

## 3. Ficheiros

Todos em `src/backend/ClinicaVeterinaria.UnitTests/Vacinacao`:

| Ficheiro                                     | Sigla | Unidade                                  | Origem                                                                |
| -------------------------------------------- | ----- | ---------------------------------------- | --------------------------------------------------------------------- |
| `CalculadoraProximaAdministracaoTests.cs`    | CAL   | `CalculadoraProximaAdministracao`        | I082; identificadores completos no nome (I089)                        |
| `AdministracaoDtoValidacaoTests.cs`          | DTO   | `CreateAdministracaoDto`, `BoletimQuery` | I082; identificadores completos no nome (I089)                        |
| `VacinacoesServiceTests.cs`                  | SRV   | `VacinacoesService`                      | I082; identificadores completos e quatro testes novos (I089)          |
| `VacinacoesControllerTests.cs`               | CTL   | `VacinacoesController`, `MinhasVacinacoesController` | I082, sem alterações                                     |

Não há alterações ao código de produção, ao esquema nem aos pacotes do projeto de testes.

## 4. Decisões de implementação

1. **Duplos e relógio.** O `VacinacoesService` é testado com o EF Core InMemory (`AjudaBaseDados`), como nos restantes serviços (I028, secção 4.2), e com o `RelogioFixo` (`RELOGIO = 2026-10-14T08:00:00Z`, 09:00 em Lisboa). A `CalculadoraProximaAdministracao` é uma função pura. Nenhum teste usa base de dados real, rede ou o relógio do sistema.
2. **Calendário na atualização.** Os casos UT-F006-003 a 007 verificam a calculadora com datas futuras. Na atualização, a data de administração não pode ser futura, por isso o teste novo usa os mesmos tipos de fronteira com datas passadas: fim de mês (2026-01-31 + 1), fim de ano (2025-12-31 + 1) e 29 de fevereiro (2024-02-28 + 1 e 2023-03-01 + 365).
3. **Tipo e intervalo.** A I028 cobre a vacina com intervalo (UT-F006-008) e a desparasitação sem intervalo (UT-F006-009). O teste novo cobre as outras duas combinações e confirma que a próxima administração depende só do intervalo.
4. **Identificadores completos.** Os nomes que agrupavam casos de forma abreviada («UT-F006-001 e 003 a 007», «UT-F006-013 a 016», «UT-F006-013 e 015») passam a ter os identificadores completos, para que `--filter "DisplayName~UT-F006-005"` encontre o teste.

## 5. Correspondência caso da especificação ↔ teste

### 5.1. UT-F006 (I028, secção 7.5)

| Caso I028   | Ficheiro | Teste                                                    | Estado              |
| ----------- | -------- | -------------------------------------------------------- | ------------------- |
| UT-F006-001 | CAL      | `SomaOIntervalo` (linha 2026-10-14 + 90)                 | Implementado (I082) |
| UT-F006-002 | CAL      | `SemIntervaloDevolveNull`                                | Implementado (I082) |
| UT-F006-003 | CAL      | `SomaOIntervalo` (linha 2026-01-31 + 1)                  | Implementado (I082) |
| UT-F006-004 | CAL      | `SomaOIntervalo` (linha 2028-02-28 + 1)                  | Implementado (I082) |
| UT-F006-005 | CAL      | `SomaOIntervalo` (linha 2027-02-28 + 1)                  | Implementado (I082) |
| UT-F006-006 | CAL      | `SomaOIntervalo` (linha 2026-12-31 + 1)                  | Implementado (I082) |
| UT-F006-007 | CAL      | `SomaOIntervalo` (linha 2027-03-01 + 365)                | Implementado (I082) |
| UT-F006-008 | SRV      | `RegistaVacinaComIntervalo`                              | Implementado (I082) |
| UT-F006-009 | SRV      | `RegistaDesparasitacaoSemIntervalo`                      | Implementado (I082) |
| UT-F006-010 | SRV, DTO | `CampoEmFaltaDevolve400` (4 casos), `CamposObrigatorios` (4 casos) | Implementado (I082) |
| UT-F006-011 | SRV      | `DataFuturaDevolve400`                                   | Implementado (I082) |
| UT-F006-012 | SRV      | `DataDeHojeEAceite`                                      | Implementado (I082) |
| UT-F006-013 | SRV, DTO | `IntervaloNaoPositivoDevolve400` (linha 0), `LimitesDosCampos` | Implementado (I082) |
| UT-F006-014 | SRV, DTO | `IntervaloDeUmDia`, `LimitesDosCampos`                   | Implementado (I082) |
| UT-F006-015 | SRV, DTO | `IntervaloNaoPositivoDevolve400` (linha -30), `LimitesDosCampos` | Implementado (I082) |
| UT-F006-016 | SRV, DTO | `LoteDemasiadoLongoDevolve400`, `LimitesDosCampos`       | Implementado (I082) |
| UT-F006-017 | SRV      | `AnimalInexistenteNoRegistoDevolve404`                   | Implementado (I082) |
| UT-F006-018 | SRV      | `BoletimCompleto`                                        | Implementado (I082) |
| UT-F006-019 | SRV      | `BoletimFiltradoPorTipo`                                 | Implementado (I082) |
| UT-F006-020 | SRV, DTO | `TipoInvalidoNoBoletimDevolve400`, `TipoDoBoletim`       | Implementado (I082) |
| UT-F006-021 | SRV      | `ProximaPassadaNaoAparece`                               | Implementado (I082) |
| UT-F006-022 | SRV      | `BoletimDoCliente`                                       | Implementado (I082) |
| UT-F006-023 | SRV      | `BoletimDeAnimalDeOutroClienteDevolve403`                | Implementado (I082) |
| UT-F006-024 | SRV      | `BoletimDoClienteSemRegistos`                            | Implementado (I082) |

**Resumo:** 24 casos UT-F006 da I028; 24 implementados; 0 não implementados.

### 5.2. Testes além da I028

| Teste                                            | Ficheiro | Origem | O que verifica |
| ------------------------------------------------ | -------- | ------ | -------------- |
| `ProximaDependeSoDoIntervalo` (2 casos)          | SRV      | I089   | Vacinação sem intervalo (próxima nula) e desparasitação com intervalo (próxima calculada) |
| `AtualizacaoRecalculaNosCasosDeCalendario` (4 casos) | SRV  | I089   | Recálculo na atualização: fim de mês, fim de ano e 29 de fevereiro; valores gravados |
| `AtualizarDesparasitacaoComIntervaloCriaProxima` | SRV      | I089   | Dar intervalo a uma desparasitação sem próxima cria a próxima e esta entra no boletim |
| `AtualizacaoDeOutroVeterinarioNaoAlteraORegisto` | SRV      | I089   | O 403 na atualização não muda o registo (tipo, data, intervalo e Veterinário) |
| Atualização com recálculo (data e intervalo), substituição de campos, dados inválidos, outro animal, outro Veterinário | SRV | I082 | Regras da atualização (RF056) |
| `HojeEmHoraDeLisboa`, `TextosAparados`, próximas por ordem crescente, animal sem registos, isolamento entre animais | SRV | I082 | Regras próprias do serviço |
| `IntervaloNaoPositivoERecusado`                  | CAL      | I082   | A calculadora recusa intervalos não positivos |
| Políticas e rotas de EP-042 a EP-044 e da atualização | CTL | I082   | Só o Veterinário regista, atualiza e consulta o boletim web; EP-044 só o Cliente, com o Cliente do token |

As políticas por perfil (Veterinário, Cliente e propriedade) têm os seus próprios casos na I086 (UT-F001-042, UT-F001-044 e UT-F001-046 a 048).

### 5.3. Critério de aceitação da I089

| Parte do critério (Backlog de Projeto v03) | Como é cumprido |
| ------------------------------------------ | --------------- |
| Vacinação e desparasitação com e sem intervalo | UT-F006-008 (vacina com intervalo), UT-F006-009 (desparasitação sem intervalo) e `ProximaDependeSoDoIntervalo` (as outras duas combinações) |
| Atualização de data e intervalo e recálculo da próxima administração | Testes de atualização da I082 (data, intervalo, retirar o intervalo), `AtualizacaoRecalculaNosCasosDeCalendario` e `AtualizarDesparasitacaoComIntervaloCriaProxima` |
| Validações inválidas | UT-F006-010, 011, 013, 015, 016, 017, 020 e 023; tipo inválido; atualização com dados inválidos sem alterar o registo |
| Permissões relevantes | CTL (políticas por ação), atualização por outro Veterinário (403, sem alterar o registo), UT-F006-023 (Cliente com animal de outro Cliente) |
| O projeto de testes executa sem erros | Execução na secção 7 |

## 6. Divergências e notas

- **D1 — 400 nos testes unitários.** Como na I088, a I028 descreve o resultado como «400»; o teste unitário verifica a `ArgumentException`, que o `ExceptionHandlingMiddleware` converte em 400. O código HTTP é verificado nos testes de integração da I082.
- **N1 — Definição da tarefa.** Esta tarefa segue a definição da I089 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog v02. A descrição da Issue #142 no GitHub («vacinação e internamentos») é anterior a esse refinamento: os internamentos (F007) estão fora do âmbito pela Declaração de Âmbito v05.
- **N2 — Prazo.** A entrega é posterior ao limite do Executor do Sprint Backlog v02 (2026-11-01 17:30).

## 7. Execução

### 7.1. Local

```powershell
dotnet test src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
```

| Execução                                               | Total | Aprovados | Falhados | Ignorados | Duração |
| ------------------------------------------------------ | ----- | --------- | -------- | --------- | ------- |
| Pasta `Vacinacao` (`FullyQualifiedName~Vacinacao`)     | 83    | 83        | 0        | 0         | 4,2 s   |
| Projeto completo, 1.ª                                  | 709   | 708       | 0        | 1         | 3,5 s   |
| Projeto completo, 2.ª (seguida)                        | 709   | 708       | 0        | 1         | 3,5 s   |

Execuções feitas a 2026-11-02 sobre o ramo da I089, criado a partir de `develop` (`8bff1a4`), com o .NET 10. Linha de base antes da I089: 701 testes unitários (75 na pasta `Vacinacao`); a I089 acrescenta 8 execuções (2 testes e 2 testes parametrizados com 2 e 4 casos), todas na pasta `Vacinacao`. O teste ignorado é o UT-TRV-007 (I086), com a justificação no próprio teste (defeito D-01, PA-07 da I028). Compilação da solução sem avisos nem erros.

### 7.2. Pipeline

O job «2 · Testes unitários (QG-02)» da «Backend CI» (`backend-ci.yml`) corre os testes unitários com `--logger trx` e publica o artefacto `resultados-testes-unitarios`. O resultado da execução sobre a Pull Request fica nas verificações da própria PR («Resultado da pipeline»).

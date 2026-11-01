# I087 — Testes Unitários de Agenda e Consultas

| Campo     | Valor                                                           |
| --------- | --------------------------------------------------------------- |
| Issue     | I087 — Implementar testes unitários de agenda e Consultas (#94) |
| Sprint    | S05                                                             |
| Milestone | M3                                                              |
| Executor  | Gonçalo Dinis Magalhães Teixeira                                |
| Revisor   | Gonçalo Magalhães Ribeiro                                       |
| Auditor   | Ivan Cristiano Silva Mendes                                     |
| Data      | 2026-11-01                                                      |
| Estado    | Implementação do Executor concluída; em revisão                 |

## 1. Objetivo

Garantir que os 90 casos da funcionalidade F003 (agendamento de consultas) da Especificação de Testes Unitários do Backend (I028) estão implementados no projeto `ClinicaVeterinaria.UnitTests`, com o identificador do caso no nome do teste, sem base de dados real, rede nem relógio real, e registar a correspondência entre cada caso e o teste que o implementa (evidência 3).

Os testes foram escritos pelas tarefas de implementação de cada área (I071 a I076), à medida que o código foi integrado. Esta tarefa confirmou a cobertura caso a caso, corrigiu os dois testes da máquina de estados que não cumpriam os critérios 5 e 6 da Issue e o ponto do Auditor sobre testes sem asserções (secção 4), e regista a execução local e na pipeline.

## 2. Documentos de referência

| Documento                                           | Versão                                                        |
| --------------------------------------------------- | ------------------------------------------------------------- |
| Especificação de Testes Unitários do Backend (I028) | v01 (`m3-especificacao-testes-unitarios-backend-v01.md`, 7.3) |
| Documentação da API                                 | v02 (EP-021 a EP-030)                                         |
| Especificação de Requisitos                         | v02 (RF019 a RF029)                                           |
| Plano de Testes do Backend                          | v02                                                           |
| Declaração de Âmbito                                | v05                                                           |

## 3. Ficheiros de teste

Todos em `src/backend/ClinicaVeterinaria.UnitTests`. A coluna «Casos» conta os casos UT-F003 diferentes de cada ficheiro (alguns casos têm testes em mais de um ficheiro).

| Ficheiro                                            | Métodos UT-F003 | Casos | Origem                               |
| --------------------------------------------------- | --------------- | ----- | ------------------------------------ |
| `Agenda/CalculadoraDisponibilidadeTests.cs`         | 11              | 11    | I071                                 |
| `Agenda/MaquinaEstadosMarcacaoTests.cs`             | 4               | 14    | I074; corrigido nesta tarefa (ver 4) |
| `Agenda/VersaoMarcacaoTests.cs`                     | 6               | 6     | I072 e I074                          |
| `Consultas/AtualizarConsultaServiceTests.cs`        | 10              | 11    | I074                                 |
| `Consultas/AtualizarEstadoConsultaServiceTests.cs`  | 1               | 1     | I074                                 |
| `Consultas/ConsultaFiltroQueryTests.cs`             | 1               | 1     | I072                                 |
| `Consultas/ConsultasServiceTests.cs`                | 6               | 6     | I072                                 |
| `Consultas/CriarConsultaServiceTests.cs`            | 14              | 14    | I073                                 |
| `Consultas/MinhasConsultasServiceTests.cs`          | 6               | 6     | I076                                 |
| `Consultas/NotasPreparacaoServiceTests.cs`          | 7               | 9     | I075                                 |
| `Consultas/RegistarChegadaServiceTests.cs`          | 4               | 5     | I075                                 |
| `Validacao/ValidadorPeriodoDisponibilidadeTests.cs` | 4               | 4     | I071                                 |
| `Veterinarios/VeterinariosServiceTests.cs`          | 5               | 7     | I071                                 |

Total: 79 métodos de teste com identificador UT-F003, que dão 103 execuções (os `[Theory]` correm uma vez por linha `[InlineData]`), para os 90 casos.

## 4. Alterações feitas nesta tarefa

Só `Agenda/MaquinaEstadosMarcacaoTests.cs` foi alterado (commit `56d9b8d`):

- **UT-F003-062 a 065** (transições válidas) não tinha nenhuma asserção: passava apenas por não ser lançada exceção, o que o checklist do Auditor não aceita. Passa a usar `Record.Exception` e `Assert.Null`.
- **UT-F003-074** (`Pendente` nunca é destino) recebia o estado de origem de cada linha num parâmetro que não usava (`_`): as quatro linhas faziam exatamente a mesma verificação, e o critério 5 pede um caso por cada combinação da tabela de transições. Passa a usar o estado de origem e segue o mesmo caminho do `ConsultasService` (normalizar o destino e validar a transição).
- O ficheiro foi reindentado para 4 espaços, conforme o `.editorconfig` (com o diff a ignorar espaços, só aparecem as duas alterações).

## 5. Verificação dos critérios de aceitação

| Critério                                                                | Como é cumprido                                                                                                                                                                                                                                                                                                                                                                                                                    |
| ----------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1. Todos os casos aplicáveis implementados, com o identificador no nome | Os 90 casos UT-F003-001 a 090 (secção 7.3 da I028) estão implementados; nenhum fica por implementar (secção 6). O identificador está no `DisplayName`, como recomenda a I028 (4.3) por causa da regra CA1707. Alguns `[Theory]` agrupam casos vizinhos e indicam-nos no nome (por exemplo, «UT-F003-066 a 073»), com uma linha `[InlineData]` por caso.                                                                            |
| 2. Sem base de dados, rede nem relógio real                             | Os serviços usam `ClinicaDbContext` com o fornecedor EF Core InMemory (`AjudaBaseDados`), a opção proposta pela I028 (4.2; PA-14); as regras puras (`CalculadoraDisponibilidade`, `MaquinaEstadosMarcacao`, `VersaoMarcacao`, `ValidadorPeriodoDisponibilidade`) são testadas sem dependências. A hora é injetada com `RelogioFixo` (`TimeProvider`, D-07); nenhum serviço de F003 usa `DateTime.UtcNow`. Não há chamadas de rede. |
| 3. Preparar–agir–verificar, um motivo de falha, dados mínimos           | Verificado por leitura dos 13 ficheiros: as três partes estão separadas por linhas em branco e cada teste usa só os dados do caso, a partir das constantes da I028 (secção 5: `RELOGIO`, `HORARIO_PADRAO`, `VET_A`, …).                                                                                                                                                                                                            |
| 4. Fronteiras do cálculo de disponibilidade                             | Marcações coladas: UT-F003-003 e 029. Limite do fecho: UT-F003-004, 024, 025 e 026. Dia fechado: UT-F003-006 e 023. Dia com dois períodos: UT-F003-007. Marcação cancelada: UT-F003-005 e 030. Também: duração de 45 minutos (008), marcação de 60 minutos fora da grelha (009), mudança da hora (011), horários já passados (012) e 31/32 dias (013, 014).                                                                        |
| 5. Uma verificação por transição válida e inválida                      | As 16 combinações de origem e destino: 4 válidas (UT-F003-062 a 065), 8 inválidas para `Confirmada`, `Realizada` e `Cancelada` (066 a 073) e as 4 com destino `Pendente` (074, agora uma por estado de origem). Mais o estado inexistente (075).                                                                                                                                                                                   |
| 6. Sem falhas e sem testes ignorados                                    | 471 testes unitários, 0 falhados, 0 ignorados (secção 8); nenhum teste do projeto tem `Skip`.                                                                                                                                                                                                                                                                                                                                      |

## 6. Correspondência caso da especificação ↔ teste

Unidades: DISP (`CalculadoraDisponibilidade` e período de EP-022), CONS (`ConsultasService`), EST (`MaquinaEstadosMarcacao`), NOTA (notas de preparação) e ETAG (`VersaoMarcacao`). Os testes estão identificados por classe e método. Casos não implementados: nenhum.

| Caso        | Tipo      | Unidade | EP             | Teste                                                                                                                             |
| ----------- | --------- | ------- | -------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| UT-F003-001 | Positivo  | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.DiaUtilSemMarcacoesTemDezoitoHorarios`                                                           |
| UT-F003-002 | Positivo  | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.MarcacaoConfirmadaOcupaOHorario`                                                                 |
| UT-F003-003 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.MarcacoesSeguidasOcupamOsDoisHorarios`                                                           |
| UT-F003-004 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.UltimoHorarioAcabaNoFecho`                                                                       |
| UT-F003-005 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.MarcacaoCanceladaNaoOcupaHorario`                                                                |
| UT-F003-006 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.DomingoSemPeriodosTemListaVazia`                                                                 |
| UT-F003-007 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.IntervaloEntrePeriodosNaoTemHorarios`                                                            |
| UT-F003-008 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.DuracaoDe45MinutosMudaAGrelha`                                                                   |
| UT-F003-009 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.MarcacaoDe60MinutosForaDaGrelha`                                                                 |
| UT-F003-010 | Fronteira | DISP    | EP-022         | `VeterinariosServiceTests.MarcacaoDeOutroVeterinarioNaoOcupa`                                                                     |
| UT-F003-011 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.HoraDeInvernoConverteParaHoraLocal`                                                              |
| UT-F003-012 | Fronteira | DISP    | EP-022         | `CalculadoraDisponibilidadeTests.HorariosQueJaComecaramNaoSaoDevolvidos`                                                          |
| UT-F003-013 | Fronteira | DISP    | EP-022         | `ValidadorPeriodoDisponibilidadeTests.PeriodoDe31DiasEAceite`<br>`VeterinariosServiceTests.PeriodoDe31DiasDevolve31Dias`          |
| UT-F003-014 | Fronteira | DISP    | EP-022         | `ValidadorPeriodoDisponibilidadeTests.PeriodoDe32DiasERecusado`<br>`VeterinariosServiceTests.PeriodoInvalidoERecusado` (3 linhas) |
| UT-F003-015 | Negativo  | DISP    | EP-022         | `ValidadorPeriodoDisponibilidadeTests.DataFimAnteriorERecusada`<br>`VeterinariosServiceTests.PeriodoInvalidoERecusado` (3 linhas) |
| UT-F003-016 | Negativo  | DISP    | EP-022         | `ValidadorPeriodoDisponibilidadeTests.SemDataInicioERecusado`<br>`VeterinariosServiceTests.PeriodoInvalidoERecusado` (3 linhas)   |
| UT-F003-017 | Positivo  | DISP    | EP-022         | `VeterinariosServiceTests.SemDataFimTemUmDia`                                                                                     |
| UT-F003-018 | Negativo  | DISP    | EP-022         | `VeterinariosServiceTests.NaoVeterinarioOuInexistenteDaNaoEncontrado` (2 linhas)                                                  |
| UT-F003-019 | Positivo  | CONS    | EP-025         | `CriarConsultaServiceTests.CriaMarcacaoPendente`                                                                                  |
| UT-F003-020 | Negativo  | CONS    | EP-025         | `CriarConsultaServiceTests.DataNoPassadoDevolve400`                                                                               |
| UT-F003-021 | Fronteira | CONS    | EP-025         | `CriarConsultaServiceTests.DataIgualAoRelogioDevolve400`                                                                          |
| UT-F003-022 | Negativo  | CONS    | EP-025         | `CriarConsultaServiceTests.IdentificadorZeroDevolve400` (2 linhas)                                                                |
| UT-F003-023 | Negativo  | CONS    | EP-025         | `CriarConsultaServiceTests.DomingoDevolve422`                                                                                     |
| UT-F003-024 | Fronteira | CONS    | EP-025         | `CriarConsultaServiceTests.ConsultaQueUltrapassaOFechoDevolve422`                                                                 |
| UT-F003-025 | Fronteira | CONS    | EP-025         | `CriarConsultaServiceTests.InicioNoFechoDevolve422`                                                                               |
| UT-F003-026 | Fronteira | CONS    | EP-025         | `CriarConsultaServiceTests.ConsultaQueAcabaNoFechoEAceite`                                                                        |
| UT-F003-027 | Negativo  | CONS    | EP-025         | `CriarConsultaServiceTests.MesmaHoraOcupadaDevolve422`                                                                            |
| UT-F003-028 | Fronteira | CONS    | EP-025         | `CriarConsultaServiceTests.SobreposicaoParcialDevolve422`                                                                         |
| UT-F003-029 | Fronteira | CONS    | EP-025         | `CriarConsultaServiceTests.MarcacoesQueSeTocamSaoAceites`                                                                         |
| UT-F003-030 | Fronteira | CONS    | EP-025         | `CriarConsultaServiceTests.MarcacaoCanceladaNaoOcupa`                                                                             |
| UT-F003-031 | Negativo  | CONS    | EP-025         | `CriarConsultaServiceTests.AnimalInexistenteDevolve404`                                                                           |
| UT-F003-032 | Negativo  | CONS    | EP-025         | `CriarConsultaServiceTests.VeterinarioInexistenteDevolve404`                                                                      |
| UT-F003-033 | Positivo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.AlteraMarcacaoPendente`                                                                            |
| UT-F003-034 | Positivo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.AlteraMarcacaoConfirmada`                                                                          |
| UT-F003-035 | Negativo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.EstadoFinalNaoPodeSerAlterado` (2 linhas)                                                          |
| UT-F003-036 | Negativo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.EstadoFinalNaoPodeSerAlterado` (2 linhas)                                                          |
| UT-F003-037 | Negativo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.ReagendarParaHorarioOcupadoDevolve422`                                                             |
| UT-F003-038 | Fronteira | CONS    | EP-026         | `AtualizarConsultaServiceTests.PropriaMarcacaoNaoContaComoOcupacao`                                                               |
| UT-F003-039 | Negativo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.ReagendarParaPassadoDevolve400`                                                                    |
| UT-F003-040 | Negativo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.AtualizarMarcacaoInexistenteDevolve404`                                                            |
| UT-F003-041 | Negativo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.AtualizarComIfMatchDesatualizadoDevolve412`                                                        |
| UT-F003-042 | Positivo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.AtualizarComIfMatchAtualEAceite`                                                                   |
| UT-F003-043 | Positivo  | CONS    | EP-026         | `AtualizarConsultaServiceTests.AtualizarSemIfMatchEAceite`                                                                        |
| UT-F003-044 | Negativo  | CONS    | EP-027         | `AtualizarEstadoConsultaServiceTests.AtualizarEstadoComIfMatchDesatualizadoDevolve412`                                            |
| UT-F003-045 | Positivo  | CONS    | EP-028         | `RegistarChegadaServiceTests.ChegadaEmConfirmadaGravaHoraDoServidor`                                                              |
| UT-F003-046 | Negativo  | CONS    | EP-028         | `RegistarChegadaServiceTests.ChegadaEmMarcacaoNaoConfirmadaDevolve422` (2 linhas)                                                 |
| UT-F003-047 | Negativo  | CONS    | EP-028         | `RegistarChegadaServiceTests.ChegadaEmMarcacaoNaoConfirmadaDevolve422` (2 linhas)                                                 |
| UT-F003-048 | Fronteira | CONS    | EP-028         | `RegistarChegadaServiceTests.SegundaChegadaDevolve422`                                                                            |
| UT-F003-049 | Negativo  | CONS    | EP-028         | `RegistarChegadaServiceTests.IfMatchDesatualizadoDevolve412`                                                                      |
| UT-F003-050 | Positivo  | CONS    | EP-023         | `ConsultasServiceTests.VeterinarioIgnoraFiltroDeOutroVeterinario`                                                                 |
| UT-F003-051 | Positivo  | CONS    | EP-023         | `ConsultasServiceTests.RececionistaFiltraPorEstado`                                                                               |
| UT-F003-052 | Negativo  | CONS    | EP-023         | `ConsultaFiltroQueryTests.DataFimAnteriorInvalida`<br>`ConsultasServiceTests.DataFimAnteriorDevolve400`                           |
| UT-F003-053 | Positivo  | CONS    | EP-024         | `ConsultasServiceTests.AuxiliarSemApoioLeNotaDePreparacao`                                                                        |
| UT-F003-054 | Negativo  | CONS    | EP-024         | `ConsultasServiceTests.VeterinarioNaoLeMarcacaoDeOutro`                                                                           |
| UT-F003-055 | Positivo  | CONS    | EP-024         | `ConsultasServiceTests.VeterinarioLeAPropriaMarcacao`                                                                             |
| UT-F003-056 | Positivo  | CONS    | EP-030         | `MinhasConsultasServiceTests.FuturasSoDoClientePorDataCrescente`                                                                  |
| UT-F003-057 | Fronteira | CONS    | EP-030         | `MinhasConsultasServiceTests.MarcacaoQueComecaAgoraEFutura`                                                                       |
| UT-F003-058 | Positivo  | CONS    | EP-030         | `MinhasConsultasServiceTests.HistoricoPassadasECanceladasPorDataDecrescente`                                                      |
| UT-F003-059 | Positivo  | CONS    | EP-030         | `MinhasConsultasServiceTests.SemTipoEIgualAFuturas`                                                                               |
| UT-F003-060 | Negativo  | CONS    | EP-030         | `MinhasConsultasServiceTests.TipoInvalidoDevolve400` (3 linhas)                                                                   |
| UT-F003-061 | Negativo  | CONS    | EP-030         | `MinhasConsultasServiceTests.AnimalDeOutroClienteDevolve403`                                                                      |
| UT-F003-062 | Positivo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesPermitidasSaoAceites` (4 linhas)                                                           |
| UT-F003-063 | Positivo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesPermitidasSaoAceites` (4 linhas)                                                           |
| UT-F003-064 | Positivo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesPermitidasSaoAceites` (4 linhas)                                                           |
| UT-F003-065 | Positivo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesPermitidasSaoAceites` (4 linhas)                                                           |
| UT-F003-066 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-067 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-068 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-069 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-070 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-071 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-072 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-073 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.TransicoesInvalidasSaoRecusadas` (8 linhas)                                                          |
| UT-F003-074 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.PendenteNuncaEUmDestinoValido` (4 linhas)                                                            |
| UT-F003-075 | Negativo  | EST     | EP-027         | `MaquinaEstadosMarcacaoTests.EstadoInexistenteERecusado`                                                                          |
| UT-F003-076 | Positivo  | NOTA    | EP-029         | `NotasPreparacaoServiceTests.AuxiliarComApoioCriaNota`                                                                            |
| UT-F003-077 | Negativo  | NOTA    | EP-029         | `NotasPreparacaoServiceTests.AuxiliarSemApoioDevolve403`                                                                          |
| UT-F003-078 | Negativo  | NOTA    | EP-029         | `NotasPreparacaoServiceTests.TextoVazioDevolve400` (3 linhas)                                                                     |
| UT-F003-079 | Fronteira | NOTA    | EP-029         | `NotasPreparacaoServiceTests.TextoVazioDevolve400` (3 linhas)                                                                     |
| UT-F003-080 | Fronteira | NOTA    | EP-029         | `NotasPreparacaoServiceTests.LimiteDeMilCaracteres` (2 linhas)                                                                    |
| UT-F003-081 | Fronteira | NOTA    | EP-029         | `NotasPreparacaoServiceTests.TextoVazioDevolve400` (3 linhas)<br>`NotasPreparacaoServiceTests.LimiteDeMilCaracteres` (2 linhas)   |
| UT-F003-082 | Negativo  | NOTA    | EP-029         | `NotasPreparacaoServiceTests.MarcacaoCanceladaDevolve422`                                                                         |
| UT-F003-083 | Fronteira | NOTA    | EP-029         | `NotasPreparacaoServiceTests.MarcacaoRealizadaAceite`                                                                             |
| UT-F003-084 | Negativo  | NOTA    | EP-029         | `NotasPreparacaoServiceTests.MarcacaoInexistenteDevolve404`                                                                       |
| UT-F003-085 | Positivo  | ETAG    | EP-024, EP-026 | `VersaoMarcacaoTests.RowVersionEmBase64EntreAspas`                                                                                |
| UT-F003-086 | Positivo  | ETAG    | EP-026         | `VersaoMarcacaoTests.IfMatchComVersaoAtualCorresponde`                                                                            |
| UT-F003-087 | Negativo  | ETAG    | EP-026         | `VersaoMarcacaoTests.IfMatchComVersaoDiferenteNaoCorresponde`                                                                     |
| UT-F003-088 | Fronteira | ETAG    | EP-026         | `VersaoMarcacaoTests.IfMatchFracoNaoCorresponde`                                                                                  |
| UT-F003-089 | Fronteira | ETAG    | EP-026         | `VersaoMarcacaoTests.IfMatchAsteriscoCorresponde`                                                                                 |
| UT-F003-090 | Fronteira | ETAG    | EP-026         | `VersaoMarcacaoTests.IfMatchSemAspasNaoCorresponde`                                                                               |

Distribuição: 23 positivos, 38 negativos e 29 de fronteira, como na secção 10.4 da I028.

## 7. Divergências face à especificação

| Ref. | Caso              | Divergência                                                                                                                                                                                                                                                                                                                                                                                                                      | Situação                                                                                          |
| ---- | ----------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- |
| D1   | UT-F003-056 a 058 | A I028 separa as vistas de EP-030 só pela data (`dataHora ≥ agora` / `< agora`). A implementação da I076 (#217) também usa o estado: as futuras são só as `Pendente` ou `Confirmada`, e uma marcação futura `Cancelada` ou `Realizada` vai para o histórico. Os testes seguem a implementação. A regra é coerente com o RF029 e com a descrição de EP-030 na API v02 («histórico de marcações passadas e consultas realizadas»). | Registada como melhoria M1 no relatório de revisão da I076; atualizar a I028 numa próxima versão. |
| D2   | UT-F003-074       | A I028 descreve o caso como «400 em todos». O teste unitário verifica a `ArgumentException`, que o `ExceptionHandlingMiddleware` converte em 400; o código HTTP é verificado no teste de integração IT-EP027-05 (`AtualizarEstadoConsultaEndpointsTests`, I074).                                                                                                                                                                 | Sem impacto: é o mesmo padrão de todos os casos 400 da I028.                                      |

## 8. Execução

### 8.1. Local

Execução a 2026-11-01, com o .NET 10, no ramo `i087-testes-unitarios-agendamento` atualizado com o `develop` (`93c5723`, com a I076):

```text
dotnet test src/backend/ClinicaVeterinaria.UnitTests
Passed!  - Failed:     0, Passed:   471, Skipped:     0, Total:   471, Duration: 3 s - ClinicaVeterinaria.UnitTests.dll (net10.0)

dotnet test src/backend/ClinicaVeterinaria.UnitTests --filter "DisplayName~UT-F003" --logger "console;verbosity=normal"
Test Run Successful.
Total tests: 103
     Passed: 103
 Total time: 2,0575 Seconds
```

As 103 execuções correspondem aos 79 métodos UT-F003 da secção 3, com uma execução por linha dos `[Theory]`.

### 8.2. Pipeline

Ver a secção «Testes» da Pull Request: a pipeline Backend CI corre os testes unitários (job «2 · Testes unitários») sobre o commit da PR, e a ligação da execução é acrescentada aqui depois de concluída.

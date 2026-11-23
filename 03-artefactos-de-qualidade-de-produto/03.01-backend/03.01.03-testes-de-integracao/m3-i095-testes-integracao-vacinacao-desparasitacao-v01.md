# I095 — Testes de Integração de Vacinação e Desparasitação

| Campo     | Valor                                                                              |
| --------- | ---------------------------------------------------------------------------------- |
| Issue     | I095 — Implementar testes de integração de vacinação e desparasitação (#143)       |
| Sprint    | S05                                                                                |
| Milestone | M3                                                                                 |
| Executor  | Ivan Cristiano Silva Mendes                                                        |
| Revisor   | Gonçalo Dinis Magalhães Teixeira                                                   |
| Auditor   | André Filipe Moreira Mesquita                                                      |
| Data      | 2026-11-02                                                                         |
| Estado    | Implementação do Executor concluída; em revisão                                    |

## 1. Objetivo

Garantir que os casos da Especificação de Testes de Integração do Backend (I030) para o boletim de vacinas e desparasitações — EP-042 a EP-044 (secção 13) — estão implementados no projeto `ClinicaVeterinaria.IntegrationTests`, com o identificador do caso no nome do teste, a correr contra a API real em memória e um SQL Server isolado (infraestrutura da I091), e registar a correspondência entre cada caso e o teste (evidência 3).

Os 16 casos foram escritos pela tarefa de implementação dos endpoints (I082). Esta tarefa confirmou a cobertura caso a caso, completou os identificadores abreviados de IT-EP042-03 e IT-EP043-02 e acrescentou os testes que o critério do Backlog de Projeto v03 pede e que a I030 não tinha: as alterações não afetam outros pacientes nem outros Clientes, a próxima administração recalculada na atualização chega ao boletim do Cliente (EP-044), o recálculo nos casos de calendário confrontado com a coluna calculada do SQL Server, a atualização recusada deixa o registo como estava e a recusa de EP-044 aos perfis da clínica.

## 2. Documentos de referência

| Documento                                               | Versão                                            |
| ------------------------------------------------------- | ------------------------------------------------- |
| Backlog de Projeto                                      | v03 (I095, aprovada como Ready a 2026-10-27)      |
| Especificação de Testes de Integração do Backend (I030) | v01 (secções 13 e 14)                             |
| Documentação da API                                     | v02 (EP-042 a EP-044)                             |
| Especificação de Requisitos                             | v02 (RF043 a RF047, RF056)                        |
| Plano de Testes do Backend                              | v02                                               |
| Infraestrutura dos testes de integração (I091)          | `m3-i091-infraestrutura-testes-integracao-v01.md` |

## 3. Ficheiros

Todos em `src/backend/ClinicaVeterinaria.IntegrationTests/Vacinacoes`:

| Ficheiro                                  | Sigla | Endpoints                     | Origem                                                                                 |
| ----------------------------------------- | ----- | ----------------------------- | -------------------------------------------------------------------------------------- |
| `VacinacoesEndpointsTests.cs`             | VAC   | EP-042, EP-043, RF056, EP-044 | I082; só os identificadores completos de IT-EP042-03 e IT-EP043-02 no nome (I095)      |
| `VacinacoesPersistenciaEndpointsTests.cs` | PER   | EP-042, RF056, EP-044         | Novo (I095): isolamento, recálculo, boletim do Cliente e perfis sem acesso a EP-044    |

Não há alterações ao código de produção nem ao esquema da base de dados.

## 4. Decisões de implementação

1. **Dados e independência.** Cada teste parte dos dados de demonstração da I061, repostos antes de cada teste (Respawn, I091): Bobi (João) com a vacina antirrábica, a vacina polivalente PV-1187 e o desparasitante interno DP-0042 (90 dias, próxima 2027-01-12); Kiko (Maria) com o desparasitante externo DE-0310 de 2026-08-01, sem intervalo; Mia (João) sem registos. Todos foram registados pela vet.ana. O que um teste cria ou altera é feito no próprio teste.
2. **Isolamento entre pacientes e Clientes.** `AlteracoesNoBobiNaoAfetamOutrosPacientesNemClientes` regista uma vacina no Bobi e atualiza o DP-0042 e confirma, campo a campo na base, que os registos do Kiko e da Mia e os outros registos do Bobi ficam iguais, e que o boletim da Maria (EP-044) é o mesmo antes e depois.
3. **Boletim do Cliente depois da atualização.** `AtualizacaoApareceNoBoletimDoCliente` dá um intervalo de 365 dias ao DE-0310 do Kiko: a próxima (2027-08-01) é devolvida na resposta, gravada na coluna calculada, mostrada nas próximas de EP-042 e nas próximas do boletim da Maria (EP-044); o boletim do João não muda.
4. **Recálculo nos casos de calendário.** `AtualizacaoRecalculaNosCasosDeCalendario` (4 casos: fim de ano, fim de mês com 30 dias, 29 de fevereiro em ano bissexto e 365 dias sobre fevereiro) confronta a data da resposta, calculada pela `CalculadoraProximaAdministracao`, com a coluna `DataProximaAdministracao`, calculada pelo SQL Server. As datas de administração são passadas, porque a data futura é recusada e os testes correm com o relógio real.
5. **Atualização recusada.** Além do 403 que a I082 já verificava, `AtualizacaoDeOutroVeterinarioNaoAlteraORegisto` confirma que o pedido do vet.bruno não muda nenhum campo dos registos do Bobi.
6. **Perfis sem acesso a EP-044.** A I082 verificava o Veterinário; `PerfisDaClinicaRecebem403NoMobile` completa a matriz da I030 (secção 14.1) com a Rececionista, o Auxiliar e o Administrador, sem dados do boletim na resposta.
7. **Formato de erro.** Todos os erros são verificados no formato único da I058 (`{ code, message, detail }`), com `code` igual ao código HTTP, como na I092 a I094.
8. **Identificadores completos.** Os nomes «IT-EP042-02 e 03» e «IT-EP043-01 e 02» passam a «IT-EP042-02 e IT-EP042-03» e «IT-EP043-01 e IT-EP043-02», para que `--filter "DisplayName~IT-EP042-03"` e `--filter "DisplayName~IT-EP043-02"` encontrem os testes.

## 5. Correspondência caso da especificação ↔ teste

Legenda dos ficheiros: ver a coluna «Sigla» da secção 3.

### 5.1. EP-042 a EP-044 (I030, secção 13)

| Caso I030   | Ficheiro | Teste                                                   | Estado                                                     |
| ----------- | -------- | ------------------------------------------------------- | ---------------------------------------------------------- |
| IT-EP042-01 | VAC      | `BoletimComHistoricoEProximas`                          | Implementado (I082)                                        |
| IT-EP042-02 | VAC      | `BoletimFiltradoPorTipo` (caso `Vacina`)                | Implementado (I082)                                        |
| IT-EP042-03 | VAC      | `BoletimFiltradoPorTipo` (caso `Desparasitacao`)        | Implementado (I082); identificador completo (I095)         |
| IT-EP042-04 | VAC      | `TipoInvalidoDevolve400`                                | Implementado (I082)                                        |
| IT-EP042-05 | VAC      | `BoletimDeAnimalInexistenteDevolve404`                  | Implementado (I082)                                        |
| IT-EP043-01 | VAC      | `RegistaComIntervalo`                                   | Implementado (I082)                                        |
| IT-EP043-02 | VAC      | `RegistaComIntervalo`                                   | Implementado (I082); identificador completo (I095)         |
| IT-EP043-03 | VAC      | `RegistaSemIntervalo`                                   | Implementado (I082)                                        |
| IT-EP043-04 | VAC      | `CampoEmFaltaDevolve400` (5 casos)                      | Implementado (I082)                                        |
| IT-EP043-05 | VAC      | `DataFuturaDevolve400`                                  | Implementado (I082)                                        |
| IT-EP043-06 | VAC      | `IntervaloNaoPositivoDevolve400` (2 casos)              | Implementado (I082)                                        |
| IT-EP043-07 | VAC      | `RegistoEmAnimalInexistenteDevolve404`                  | Implementado (I082)                                        |
| IT-EP044-01 | VAC      | `ClienteConsultaOSeuAnimal`, `OutroClienteConsultaOSeuAnimal` | Implementado (I082)                                  |
| IT-EP044-02 | VAC      | `ClienteConsultaAnimalSemRegistos`                      | Implementado (I082)                                        |
| IT-EP044-03 | VAC      | `AnimalDeOutroClienteDevolve403`                        | Implementado (I082)                                        |
| IT-EP044-04 | VAC      | `ClienteAnimalInexistenteDevolve404`                    | Implementado (I082)                                        |

**Resumo:** 16 casos da I030 para EP-042 a EP-044; 16 implementados; 0 não implementados.

### 5.2. Testes além da I030

| Teste                                                  | Ficheiro | Origem | O que verifica                                                                                              |
| ------------------------------------------------------ | -------- | ------ | ----------------------------------------------------------------------------------------------------------- |
| `AlteracoesNoBobiNaoAfetamOutrosPacientesNemClientes`  | PER      | I095   | Registar e atualizar num animal não altera os registos dos outros animais nem o boletim de outro Cliente (decisão 2) |
| `AtualizacaoApareceNoBoletimDoCliente`                 | PER      | I095   | Próxima recalculada na atualização: resposta, coluna calculada, EP-042 e EP-044 do dono; boletim de outro Cliente igual (decisão 3) |
| `AtualizacaoRecalculaNosCasosDeCalendario` (4 casos)   | PER      | I095   | Recálculo em fim de ano, fim de mês e 29 de fevereiro, igual na resposta e na coluna calculada (decisão 4)  |
| `AtualizacaoDeOutroVeterinarioNaoAlteraORegisto`       | PER      | I095   | 403 ao vet.bruno e registos do Bobi inalterados (decisão 5)                                                 |
| `PerfisDaClinicaRecebem403NoMobile` (3 casos)          | PER      | I095   | 403 em EP-044 para Rececionista, Auxiliar e Administrador, sem dados do boletim (decisão 6)                 |
| `BoletimSemRegistos`                                   | VAC      | I082   | EP-042 de um animal sem registos: 200 com as listas vazias                                                  |
| `AtualizaERecalcula`                                   | VAC      | I082   | Atualização de data e intervalo: 200 e próxima recalculada (resposta e coluna calculada)                    |
| `AtualizaSemIntervalo`                                 | VAC      | I082   | Retirar o intervalo deixa a próxima nula                                                                    |
| `AtualizacaoInvalidaDevolve400`                        | VAC      | I082   | Atualização com intervalo 0: 400 e registo inalterado                                                       |
| `AtualizarRegistoDeOutroAnimalDevolve404`              | VAC      | I082   | Registo de outro animal ou inexistente: 404                                                                 |
| `AtualizarRegistoDeOutroVeterinarioDevolve403`         | VAC      | I082   | Registo feito por outro Veterinário: 403                                                                    |
| `OutrosPerfisRecebem403NoWeb` (4 casos)                | VAC      | I082   | 403 em EP-042, EP-043 e na atualização para Rececionista, Auxiliar, Administrador e Cliente, sem gravar nada |
| `VeterinarioRecebe403NoMobile`                         | VAC      | I082   | 403 em EP-044 para o Veterinário                                                                            |
| `SemTokenDevolve401`                                   | VAC      | I082   | 401 em EP-042, EP-043, atualização e EP-044 sem token                                                       |

### 5.3. Casos transversais relacionados (I030, secção 14)

| Caso I030          | Cenário                                          | Testes                                                                                  |
| ------------------ | ------------------------------------------------ | --------------------------------------------------------------------------------------- |
| IT-TRV-15          | CLIENTE_A consulta o boletim de ANIMAL_B1        | VAC `AnimalDeOutroClienteDevolve403` (com IT-EP044-03)                                  |
| Matriz (EP-042/043) | Só o Veterinário; restantes 403; sem token 401  | VAC `OutrosPerfisRecebem403NoWeb` e `SemTokenDevolve401`                                |
| Matriz (EP-044)    | Só o Cliente; restantes 403; sem token 401       | VAC `VeterinarioRecebe403NoMobile` e `SemTokenDevolve401`; PER `PerfisDaClinicaRecebem403NoMobile` |

Os casos IT-TRV são da I096 (testes de segurança e isolamento por perfil); ficam aqui só para rastreabilidade.

### 5.4. Critério de aceitação da I095

| Parte do critério                                                         | Como é cumprido |
| ------------------------------------------------------------------------- | --------------- |
| Criação de `RegistoProfilaxia`                                            | IT-EP043-01 a 07 (VAC). |
| Atualização de `RegistoProfilaxia`                                        | `AtualizaERecalcula`, `AtualizaSemIntervalo`, `AtualizacaoInvalidaDevolve400`, `AtualizarRegistoDeOutroAnimalDevolve404` (VAC) e os testes PER das decisões 3 a 5. |
| Consulta (boletim Web e Mobile)                                           | IT-EP042-01 a 05 e IT-EP044-01 a 04 (VAC); `AtualizacaoApareceNoBoletimDoCliente` (PER). |
| Próxima administração                                                     | IT-EP043-02 e 03 (VAC); `AtualizaERecalcula` e `AtualizaSemIntervalo` (VAC); `AtualizacaoApareceNoBoletimDoCliente` e `AtualizacaoRecalculaNosCasosDeCalendario` (PER). |
| Autorização                                                               | IT-EP044-03, `AtualizarRegistoDeOutroVeterinarioDevolve403`, `OutrosPerfisRecebem403NoWeb`, `VeterinarioRecebe403NoMobile`, `SemTokenDevolve401` (VAC); `AtualizacaoDeOutroVeterinarioNaoAlteraORegisto` e `PerfisDaClinicaRecebem403NoMobile` (PER). |
| As alterações persistem corretamente                                      | Verificação na base em IT-EP043-01 a 03, `AtualizaERecalcula`, `AtualizaSemIntervalo` (VAC) e em todos os testes PER. |
| As alterações não afetam outros pacientes/Clientes                        | `AlteracoesNoBobiNaoAfetamOutrosPacientesNemClientes` e `AtualizacaoApareceNoBoletimDoCliente` (PER). |

## 6. Divergências e notas

- **D1 — formato do erro.** Como na I092 a I094, a I030 indica Problem Details; a implementação devolve o formato único da I058.
- **N1 — Definição da tarefa.** Esta tarefa segue a definição da I095 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog v02. A descrição da Issue #143 no GitHub («vacinação e internamentos») é anterior a esse refinamento: os internamentos (F007, EP-045 a EP-051) estão fora do âmbito pela Declaração de Âmbito v05, como a própria I030 regista na secção 20.
- **N2 — Relógio real.** Os testes de integração correm com o relógio real (I091). As próximas que devem aparecer no boletim são de 2027 (2027-01-12, 2027-08-01 e 2027-10-14), como nos testes da I082; a partir de 2027-01-12 os testes da I082 e da I095 que contam com essas datas nas «próximas» terão de ser revistos.
- **N3 — Prazo.** A entrega é posterior ao limite do Executor do Sprint Backlog v02 (2026-11-01 17:30).

## 7. Execução

### 7.1. Local

```powershell
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
```

| Execução                                                   | Total | Aprovados | Falhados | Ignorados | Duração |
| ---------------------------------------------------------- | ----- | --------- | -------- | --------- | ------- |
| Pasta `Vacinacoes` (`FullyQualifiedName~Vacinacoes`)       | 43    | 43        | 0        | 0         | 14 s    |
| Casos IT-EP042 (`DisplayName~IT-EP042`)                    | 5     | 5         | 0        | 0         | 0,8 s   |
| Casos IT-EP043 (`DisplayName~IT-EP043`)                    | 11    | 11        | 0        | 0         | 1 s     |
| Casos IT-EP044 (`DisplayName~IT-EP044`)                    | 5     | 5         | 0        | 0         | 0,8 s   |
| Testes novos da I095 (`DisplayName~I095`)                  | 10    | 10        | 0        | 0         | 3 s     |
| Suite completa, 1.ª                                        | 403   | 403       | 0        | 0         | 238 s   |
| Suite completa, 2.ª (seguida)                              | 403   | 403       | 0        | 0         | 277 s   |

Execuções feitas a 2026-11-02 sobre o ramo da I095, atualizado a partir de `develop` (`cdffff8`, com a I089), com o SQL Server 2025 em Docker (Testcontainers). Linha de base antes da I095: 393 testes de integração (33 na pasta `Vacinacoes`); a I095 acrescenta 10 execuções (3 testes e 2 testes parametrizados com 4 e 3 casos), todas na pasta `Vacinacoes`. Compilação da solução com `-warnaserror` sem avisos nem erros.

### 7.2. Pipeline

O job «3 · Testes de integração (QG-02)» da «Backend CI» (`backend-ci.yml`) executa estes testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`. O resultado da execução sobre a Pull Request fica nas verificações da própria PR («Resultado da pipeline»); a ligação à execução é acrescentada aqui quando a PR for aberta.

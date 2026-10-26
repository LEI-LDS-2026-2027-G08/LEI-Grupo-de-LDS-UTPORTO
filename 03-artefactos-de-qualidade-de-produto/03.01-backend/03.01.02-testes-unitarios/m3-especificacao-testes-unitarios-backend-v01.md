# Especificação dos Testes Unitários do Backend — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Especificação dos Testes Unitários do Backend |
| Ficheiro | `m3-especificacao-testes-unitarios-backend-v01.md` |
| Versão | v01 — rascunho do Executor |
| Milestone | m3 |
| Issue | I028 — Especificar os Testes Unitários do Backend |
| Sprint | S04 |
| Executor | André Filipe Moreira Mesquita |
| Revisor | Ivan Cristiano Silva Mendes |
| Auditor | Gonçalo Magalhães Ribeiro |
| Pasta | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios` |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-10-26 | André Filipe Moreira Mesquita | Versão inicial: 306 casos de teste unitário para as funcionalidades em âmbito (F001, F002, F003, F004 e F006), alinhada com as versões v02 (em rascunho) da Documentação da API, do Modelo de Classes do Backend e do Plano de Testes do Backend. Revista após a revisão de 2026-10-26: dependências explícitas da D-07, da D-08 e das referências em rascunho; defeito do PA-07 registado. |

---

## Índice

- [1. Objetivo](#1-objetivo)
- [2. Documentos de referência](#2-documentos-de-referência)
- [3. Âmbito](#3-âmbito)
- [4. Abordagem e convenções](#4-abordagem-e-convenções)
- [5. Dados de referência dos testes](#5-dados-de-referência-dos-testes)
- [6. Unidades abrangidas](#6-unidades-abrangidas)
- [7. Casos de teste](#7-casos-de-teste)
- [8. Cobertura das regras do contrato da API](#8-cobertura-das-regras-do-contrato-da-api)
- [9. Unidades não testáveis unitariamente](#9-unidades-não-testáveis-unitariamente)
- [10. Rastreabilidade](#10-rastreabilidade)
- [11. Testes já implementados](#11-testes-já-implementados)
- [12. Pontos em aberto](#12-pontos-em-aberto)
- [13. Verificação dos critérios de aceitação](#13-verificação-dos-critérios-de-aceitação)

---

## 1. Objetivo

Este documento especifica os testes unitários do backend do sistema de Gestão de Clínica Veterinária. Define, **antes de se programarem os testes**, o que cada unidade tem de garantir, para que a implementação nas tarefas I086, I087, I088 e I089 seja verificável e não dependa da interpretação de cada pessoa.

Um teste unitário verifica uma unidade isolada — um serviço, um validador, um cálculo ou uma política — sem base de dados real, sem rede e sem relógio real. O comportamento conjunto da API, da base de dados e da autenticação é verificado pelos testes de integração (I030).

Cada caso tem identificador próprio e indica a unidade, as pré-condições, os dados de entrada, o resultado esperado e os requisitos, endpoints e condições de teste a que corresponde. Os casos distinguem-se em **positivos** (P), **negativos** (N) e **de fronteira** (F).

Resumo: **306 casos** (85 positivos, 138 negativos e 83 de fronteira), em 22 unidades.

---

## 2. Documentos de referência

| Documento | Estado | Utilização |
|---|---|---|
| Declaração de Âmbito v05 | Aprovada e homologada (21/10/2026) | Funcionalidades em âmbito: F001 a F004 e F006; F005 e F007 fora. |
| Documentação da API v02 (`m3-documentacao-api-v02.md`) | **Rascunho** — PR #170, por integrar | Regras de validação (400), de negócio (422), conflito (409), concorrência (412) e propriedade (403) de cada endpoint. |
| Modelo de Classes do Backend v02 (`m3-modelo-classes-backend-v02.md`) | **Rascunho** — PR #169, por integrar | Serviços, DTOs e regras de consistência do modelo (secção 8). |
| Plano de Testes do Backend v02 (`m3-plano-testes-backend-v02.md`) | **Rascunho** — PR #172, por integrar | Estratégia, convenção `UT-<módulo>-<nnn>`, condições de teste `CT-…` (secção 10) e decisões D-01 a D-10 (secção 17). |
| Especificação de Requisitos v02 (`m3-especificacao-requisitos-v02.md`) | **Rascunho** — PR #179, por integrar | Requisitos RF001–RF036 e RF043–RF047 em âmbito. |
| Modelo de Dados v02 (`m3-modelo-de-dados-clinica-veterinaria-v02.md`) | **Rascunho** — PR #183, por integrar | Limites de comprimento, restrições `CHECK` e convenção de datas em UTC. |
| Especificação dos Testes de Integração v01 (I030) | Integrada no `develop` (PR #186) | Divisão de responsabilidades entre testes unitários e de integração; identificadores lógicos dos dados. |
| Código do backend (`src/backend`) | `develop`; I063 em revisão | Unidades já existentes: `AuthService`, `JwtTokenService`, políticas de autorização e middleware de erros. |

### 2.1. Dependência de documentos em rascunho

Cinco das referências são versões v02 que ainda estão em Pull Requests por integrar (estado verificado na revisão de 2026-10-26). Foram usadas porque são as únicas alinhadas com a Declaração de Âmbito v05, que está homologada: as versões v01 aprovadas (I012, I018, I020, I027) ainda incluem F005 e F007 e não têm, por exemplo, o controlo de alterações concorrentes das marcações. A Especificação dos Testes de Integração (I030), já integrada no `develop`, usa as mesmas versões v02.

Esta especificação fica, por isso, **condicionada à aprovação dessas versões v02** (ponto em aberto PA-17). A tabela indica o que depende só delas:

| Conteúdo | Origem | Casos afetados se a origem mudar |
|---|---|---|
| Âmbito de 37 endpoints (sem F005 e F007) | Declaração de Âmbito v05 (aprovada) | Nenhum: não depende de rascunhos. |
| Resposta 412 e comparação do `If-Match` (EP-026 a EP-028) | API v02, secção 5.1 (mecanismo ainda «proposta a confirmar pelo Revisor») | UT-F003-041, UT-F003-042, UT-F003-043, UT-F003-044, UT-F003-049, UT-F003-085, UT-F003-086, UT-F003-087, UT-F003-088, UT-F003-089, UT-F003-090 |
| Nomes de serviços e DTOs | Modelo de Classes v02 | Nenhum: os casos descrevem comportamento e não dependem dos nomes (secção 6). |
| Limites de comprimento e datas em UTC | Modelo de Dados v02 | Casos de fronteira de comprimento (por exemplo UT-F001-082, UT-F002-006, UT-F001-072, UT-F003-081, UT-F006-016) e a regra R-FUSO. |
| Regras 400/409/422 por endpoint | API v02 (iguais às da v01 nos endpoints em âmbito, exceto a 412) | Secção 8. |

---

## 3. Âmbito

### 3.1. Funcionalidades incluídas

| Funcionalidade | Endpoints | Tarefa de implementação dos testes | Casos |
|---|---|---|---|
| F001 — Autenticação e controlos de acesso | EP-001 a EP-014 | I086 | 125 |
| F002 — Gestão de pacientes | EP-015 a EP-020 | I086 | 30 |
| F003 — Agendamento de consultas | EP-021 a EP-030 | I087 | 90 |
| F004 — Registo clínico e tratamentos | EP-031 a EP-034 | I088 | 27 |
| F006 — Boletim de vacinas e desparasitações | EP-042 a EP-044 | I089 | 24 |
| Transversais (paginação, erros, registos) | Vários | I086 | 10 |

### 3.2. Fora do âmbito

Em conformidade com a Declaração de Âmbito v05 e a Documentação da API v02, **não** são especificados testes para:

- F005 — Gestão de contas e pagamentos (EP-035 a EP-041; RF037 a RF042);
- F007 — Gestão de internamento (EP-045 a EP-051; RF048 a RF052).

A descrição da I088 ainda refere o cálculo do valor em dívida, os pagamentos e o fecho de conta, e a da I089 as regras de internamento. Essas partes ficam sem casos nesta especificação e as descrições devem ser atualizadas no Backlog Refinement (ponto em aberto PA-01).

---

## 4. Abordagem e convenções

### 4.1. Princípios

Os princípios seguem os critérios de aceitação das tarefas I086 a I089:

1. **Sem base de dados real, rede nem relógio real.** As dependências são substituídas por duplos e a hora é injetada através de `TimeProvider`. `RELOGIO` (secção 5) é a hora de todos os casos, salvo indicação em contrário. Esta forma de injetar a hora corresponde à decisão D-07 do Plano de Testes v02, que **ainda está como proposta**; os casos que dependem dela estão na secção 4.5 (PA-16).
2. **Preparar–agir–verificar.** Cada teste tem as três partes separadas, um único motivo de falha e os dados mínimos para o caso.
3. **Resultado observável.** O resultado esperado é sempre um valor devolvido, uma exceção (com o código HTTP que lhe corresponde no `ExceptionHandlingMiddleware`) ou o estado gravado no duplo. Nos casos negativos, verifica-se também que **nada é gravado**, quando o resultado o indica.
4. **Casos parametrizados.** Quando um caso indica «parametrizado», é implementado como um único `[Theory]` com uma linha `[InlineData]` por variante. O identificador do caso aplica-se a todas as linhas.

### 4.2. Duplos e serviços que usam `ClinicaDbContext`

As regras puras (validadores, cálculos, máquina de estados, versão da marcação) devem ficar em classes sem dependências de infraestrutura, testadas diretamente. É aí que se concentram os casos de fronteira.

Para os serviços que usam `ClinicaDbContext` (padrão Controller → Service → `ClinicaDbContext` do Modelo de Classes v02, secção 4), esta especificação **propõe** o fornecedor **EF Core InMemory** (`Microsoft.EntityFrameworkCore.InMemory`). Esta dependência **não consta** da decisão D-08 do Plano de Testes v02, que lista Moq, Mvc.Testing, Testcontainers.MsSql, Respawn e ReportGenerator, e o nível T1 do Plano prevê xUnit e Moq. Fica, por isso, como proposta a ratificar (PA-14).

A alternativa compatível com o Plano tal como está é os serviços dependerem de interfaces de acesso a dados (por exemplo, `IUtilizadoresRepositorio`) substituídas por duplos Moq. **Os casos não dependem desta escolha:** as pré-condições descrevem os dados que existem e o resultado esperado descreve o comportamento observável e o que é gravado, seja num contexto InMemory, seja nas chamadas registadas no duplo. Em qualquer das opções, o `IPasswordHasher<Utilizador>` e o `IJwtTokenService` são duplos Moq.

Nenhum dos dois tipos de duplo aplica restrições `UNIQUE` ou `CHECK`, preenche a `RowVersion` ou tem transações (e o InMemory compara texto com distinção de maiúsculas). Por isso, os casos de conflito (409) verificam a **verificação prévia feita pelo serviço** (que tem de existir no código) e os comportamentos que dependem do SQL Server ficam para os testes de integração (secção 9).

### 4.3. Identificadores e nomes dos testes

Os identificadores seguem o Plano de Testes v02 (secção 4.1): `UT-<módulo>-<nnn>`, com o módulo `F001`, `F002`, `F003`, `F004`, `F006` ou `TRV` (transversal).

O critério 1 das tarefas I086 a I089 exige o identificador no nome do teste. Como o quality gate trata os avisos como erros e as regras de nomes do .NET desaconselham `_` em nomes de métodos (CA1707), recomenda-se colocar o identificador no `DisplayName`, mantendo o nome do método legível:

```csharp
[Fact(DisplayName = "UT-F003-062 — Pendente para Confirmada é aceite")]
public void PendenteParaConfirmadaEAceite() { … }
```

### 4.4. Regras propostas que o contrato não define

Para os casos serem executáveis tal como estão escritos (critério 5), fixam-se as regras seguintes, que a Documentação da API refere mas não define. São **propostas a ratificar** (secção 12).

| Regra | Definição proposta | Casos |
|---|---|---|
| **R-PWD** — palavra-passe forte | Entre 8 e 128 caracteres, com pelo menos uma maiúscula, uma minúscula, um algarismo e um símbolo (carácter não alfanumérico). Coerente com os exemplos da API (`Temp#2026`, `Segura#2026`). | UT-F001-050, UT-F001-051, UT-F001-052, UT-F001-053, UT-F001-054, UT-F001-055, UT-F001-056, UT-F001-057, UT-F001-058, UT-F001-059 |
| **R-NIF** — NIF português | Exatamente 9 algarismos, sem espaços. Dígito de controlo: soma dos 8 primeiros algarismos multiplicados por 9, 8, …, 2; resto `r` da divisão por 11; controlo = 0 se `r < 2`, senão `11 − r`. Os NIF dos dados de demonstração da I061 cumprem esta regra. | UT-F001-060, UT-F001-061, UT-F001-062, UT-F001-063, UT-F001-064, UT-F001-065, UT-F001-066 |
| **R-EMAIL** — e-mail | Até 256 caracteres (coluna `Utilizador.Email`), sem espaços, com exatamente um `@`, parte local não vazia e domínio com pelo menos um ponto. | UT-F001-067, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-071, UT-F001-072 |
| **R-FUSO** — hora local da clínica | As datas e horas com instante são UTC (Modelo de Dados, D15); o horário de funcionamento (`TIME`) é hora local de `Europe/Lisbon`. A disponibilidade e a verificação «dentro do horário» convertem a marcação para hora local, o que muda com a hora de verão. `horariosLivres` (EP-022) é devolvido em hora local `HH:mm`. | UT-F003-011, UT-F003-019, UT-F003-024 |
| **R-HORARIO** — marcação dentro do horário | O intervalo `[dataHora, dataHora + duração)` tem de caber inteiro num período do dia e não se pode sobrepor a outra marcação não cancelada do mesmo Veterinário. Intervalos que se tocam (um acaba quando o outro começa) não se sobrepõem. | UT-F003-026, UT-F003-024, UT-F003-029, UT-F003-028 |

### 4.5. Casos que dependem da hora injetada (D-07)

Os casos seguintes usam `RELOGIO` ou outra hora fixa. Só são determinísticos se a unidade receber a hora através de `TimeProvider`, como propõe a D-07, que ainda não está ratificada (PA-16). Sem essa decisão, ficariam a depender do relógio real e não seriam executáveis tal como estão escritos.

| Unidade | Situação no código | Casos que dependem da hora |
|---|---|---|
| AUTH | `AuthService` (I063) usa `DateTime.UtcNow` em `DataUltimoAcesso`: tem de passar a receber `TimeProvider`. | UT-F001-001, UT-F001-021 |
| JWT | `JwtTokenService` (I063) já recebe `TimeProvider`. | UT-F001-025, UT-F001-028, UT-F001-029, UT-F001-030 |
| ANI | Ainda por criar: deve receber `TimeProvider` desde o início. | Todos (24): UT-F002-001 a UT-F002-024 |
| IDADE | Ainda por criar: deve receber `TimeProvider` desde o início. | Todos (6): UT-F002-025 a UT-F002-030 |
| DISP | Ainda por criar: deve receber `TimeProvider` desde o início. | Todos (18): UT-F003-001 a UT-F003-018 |
| CONS | Ainda por criar: deve receber `TimeProvider` desde o início. | Todos (43): UT-F003-019 a UT-F003-061 |
| NOTA | Ainda por criar: deve receber `TimeProvider` desde o início. | Todos (9): UT-F003-076 a UT-F003-084 |
| REG | Ainda por criar: deve receber `TimeProvider` desde o início. | Todos (27): UT-F004-001 a UT-F004-027 |
| VAC | Ainda por criar: deve receber `TimeProvider` desde o início. | Todos (17): UT-F006-008 a UT-F006-024 |

Nas unidades que leem a hora atual, todos os casos dependem dela, mesmo os que não a mencionam: por exemplo, uma marcação para 2026-10-15 só é «futura» porque `RELOGIO` é 2026-10-14.

Na situação atual do código, o caso UT-F001-001 (`DataUltimoAcesso = RELOGIO`) só passa depois de o `AuthService` receber `TimeProvider`. O mesmo vale para UT-F001-021 (`DataRevogacao = RELOGIO`), na I064.

---

## 5. Dados de referência dos testes

Os nomes lógicos são os da Especificação dos Testes de Integração (I030, secção 7), para que os dois níveis falem dos mesmos dados. Nos testes unitários estes dados são criados pelo próprio teste (por *builders*) no duplo, e não carregados da base de dados.

| Nome | Valor |
|---|---|
| `RELOGIO` | `2026-10-14T08:00:00Z` — quarta-feira, 09:00 em Lisboa (hora de verão, UTC+1). |
| `FUSO` | `Europe/Lisbon`. A hora de verão termina a 2026-10-25 (domingo): a partir daí, hora local = UTC. |
| `HORARIO_PADRAO` | O mesmo dos dados da I061: segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00; domingo fechado (11 períodos; `DiaSemana` 1 = segunda … 7 = domingo). |
| Duração das consultas | 30 minutos (`DuracaoPadraoConsultaMinutos`), salvo indicação em contrário. |
| `ADMIN_A`, `REC_A` | Administrador e Rececionista ativos. |
| `VET_A`, `VET_B` | Dois Veterinários ativos (o Veterinário é o `Funcionario` com o mesmo `Id` do `Utilizador`). |
| `AUX_COM_APOIO`, `AUX_SEM_APOIO` | Auxiliares Veterinários com `PodeRegistarApoio` a `true` e a `false`. |
| `CLIENTE_A`, `CLIENTE_B`, `CLIENTE_C` | Clientes ativos; `CLIENTE_C` não tem animais. |
| `UTILIZADOR_BLOQUEADO` | Rececionista com estado `Bloqueado`. |
| `ANIMAL_A1`, `ANIMAL_A2`, `ANIMAL_B1` | Dois animais de `CLIENTE_A` e um de `CLIENTE_B`. |
| `CONSULTA_PENDENTE`, `CONSULTA_CONFIRMADA`, `CONSULTA_REALIZADA`, `CONSULTA_CANCELADA` | Marcações de `VET_A` em cada estado, por defeito com `ANIMAL_A1`. |
| `REGISTO_CLINICO_A` | Registo clínico da `CONSULTA_REALIZADA`. |
| `V1`, `V2` | Duas versões (`RowVersion`) de uma marcação; `V2` é a atual. |

Não são usadas palavras-passe reais. As palavras-passe que aparecem nos casos servem apenas para testar os validadores.

---

## 6. Unidades abrangidas

Os nomes das unidades novas são **propostas** coerentes com o Modelo de Classes v02. O que é normativo é o comportamento descrito nos casos: se a implementação usar outro nome ou repartir a lógica de outra forma, os casos mantêm-se.

| Código | Unidade | Camada | Estado no código | Tarefa dos testes | Casos |
|---|---|---|---|---|---|
| AUTH | `AuthService` | Infraestrutura | Existe (I063); `TerminarSessaoAsync` na I064 | I086 | 22 |
| JWT | `JwtTokenService` e parâmetros de validação de `AddAutenticacaoJwt` | API (`Seguranca`) | Existe (I063) | I086 | 12 |
| JWTOPT | `JwtOptionsValidator` | API (`Seguranca`) | Existe (I063) | I086 | 5 |
| POL | Políticas de autorização de `AddAutenticacaoJwt` | API (`Seguranca`) | Existe (I063) | I086 | 10 |
| PWD | `ValidadorPalavraPasse` | Aplicação | A criar (I065, I068) | I086 | 10 |
| NIF | `ValidadorNif` | Aplicação | A criar (I067, I068) | I086 | 7 |
| EMAIL | `ValidadorEmail` | Aplicação | A criar (I065, I068) | I086 | 6 |
| UTIL | `UtilizadoresService` | Aplicação/Infraestrutura | A criar (I065, I066) | I086 | 27 |
| CLIN | `ClinicaService` | Aplicação/Infraestrutura | A criar (I067) | I086 | 12 |
| CLI | `ClientesService` | Aplicação/Infraestrutura | A criar (I068) | I086 | 14 |
| ANI | `AnimaisService` | Aplicação/Infraestrutura | A criar (I069, I070) | I086 | 24 |
| IDADE | `CalculadoraIdade` | Aplicação | A criar (I069) | I086 | 6 |
| PAG | `PaginacaoQuery` (validação comum de `pagina` e `tamanho`) | Aplicação | A criar (com a primeira lista paginada) | I086 | 5 |
| ERR | `ExceptionHandlingMiddleware`, `RespostaValidacaoModelo` e `RequestLoggingMiddleware` | API (`Middleware`) | Existem (I058, I063) | I086 | 5 |
| DISP | `CalculadoraDisponibilidade` (usada por `VeterinariosService`) | Aplicação | A criar (I071) | I087 | 18 |
| CONS | `ConsultasService` | Aplicação/Infraestrutura | A criar (I072 a I075) | I087 | 43 |
| EST | `MaquinaEstadosMarcacao` | Domínio/Aplicação | A criar (I074) | I087 | 14 |
| NOTA | `ConsultasService` — notas de preparação | Aplicação/Infraestrutura | A criar (I075) | I087 | 9 |
| ETAG | `VersaoMarcacao` (ETag e `If-Match`) | Aplicação | A criar (I074) | I087 | 6 |
| REG | `RegistosClinicosService` | Aplicação/Infraestrutura | A criar (I077 e tarefas seguintes da F004) | I088 | 27 |
| PROX | `CalculadoraProximaAdministracao` | Domínio/Aplicação | A criar (tarefa de implementação da F006) | I089 | 7 |
| VAC | `VacinacoesService` | Aplicação/Infraestrutura | A criar (tarefa de implementação da F006) | I089 | 17 |

---

## 7. Casos de teste

Colunas: **Tipo** P = positivo, N = negativo, F = fronteira. Os códigos HTTP indicados no resultado esperado são os que a exceção produz no `ExceptionHandlingMiddleware`: `ArgumentException` → 400, `UnauthorizedAccessException` → 401, `ForbiddenException` → 403, `KeyNotFoundException` → 404, `InvalidOperationException` → 409 e `UnprocessableEntityException` → 422. O 412 é uma resposta própria da operação, decidida pela unidade `VersaoMarcacao`.

### 7.1. F001 — Autenticação e controlos de acesso

Tarefa de implementação: **I086**.

#### 7.1.1. Autenticação e sessão — `AuthService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-001 | P | `ADMIN_A` ativo; hasher devolve `Success`. | `email = admin@clinica-g08.test`, palavra-passe correta. | Devolve `LoginResponseDto` com o token emitido pelo `IJwtTokenService` (duplo), `utilizador.perfil = Administrador`, `clienteId = null`; `DataUltimoAcesso` passa a ser `RELOGIO`. | RF001 | EP-001 | CT-F001-01 |
| UT-F001-002 | P | `CLIENTE_A` ativo, com linha em `Cliente` (`Id = 1`). | Credenciais corretas de `CLIENTE_A`. | `utilizador.perfil = Cliente` e `utilizador.clienteId = 1`; o `UtilizadorAutenticadoDto` passado ao `IJwtTokenService` tem `ClienteId = 1`. | RF001, RF005 | EP-001 | CT-F001-01 |
| UT-F001-003 | N | — | `email = ""`, `password = "Teste#2026"`. | `ArgumentException` (400) com «O e-mail é obrigatório.»; o hasher e a base de dados não são consultados. | RF002 | EP-001 | CT-F001-02 |
| UT-F001-004 | N | — | `email = admin@clinica-g08.test`, `password = ""`. | `ArgumentException` (400) com «A palavra-passe é obrigatória.». | RF002 | EP-001 | CT-F001-02 |
| UT-F001-005 | F | — | `email = "   "` (só espaços), palavra-passe preenchida. | `ArgumentException` (400): um e-mail só com espaços conta como em falta. | RF002 | EP-001 | CT-F001-02 |
| UT-F001-006 | F | `ADMIN_A` ativo. | `email = " admin@clinica-g08.test "` (espaços à volta), palavra-passe correta. | Autentica: o e-mail é comparado sem os espaços iniciais e finais. | RF001 | EP-001 | CT-F001-01 |
| UT-F001-007 | N | Nenhuma conta com o e-mail indicado. | `email = nao.existe@clinica-g08.test`, qualquer palavra-passe. | `UnauthorizedAccessException` (401) com «Credenciais inválidas.»; o hasher é chamado **uma vez** (hash de referência), para o tempo de resposta não revelar que o e-mail não existe; é registado o evento 3001 com o motivo. | RF002 | EP-001 | CT-F001-02 |
| UT-F001-008 | N | `ADMIN_A` ativo; hasher devolve `Failed`. | E-mail de `ADMIN_A`, `password = "Errada#0000"`. | `UnauthorizedAccessException` (401) com a **mesma** mensagem do caso anterior; nenhum token emitido; `DataUltimoAcesso` inalterada. | RF002 | EP-001 | CT-F001-02 |
| UT-F001-009 | N | `UTILIZADOR_BLOQUEADO`; hasher devolve `Success`. | Credenciais corretas da conta bloqueada. | `ForbiddenException` (403); o `IJwtTokenService` não é chamado. | RF002, RF009 | EP-001 | CT-F001-02 |
| UT-F001-010 | N | `UTILIZADOR_BLOQUEADO`; hasher devolve `Failed`. | E-mail da conta bloqueada com palavra-passe errada. | 401 «Credenciais inválidas.» e **não** 403: o bloqueio só é revelado a quem conhece a palavra-passe. | RF002, RF009 | EP-001 | CT-F001-02 |
| UT-F001-011 | P | `ADMIN_A` ativo; hasher devolve `SuccessRehashNeeded`. | Credenciais corretas. | Autentica e `PasswordHash` passa a ser o novo hash devolvido por `HashPassword`. | RF001 | EP-001 | CT-F001-01 |
| UT-F001-012 | N | Logger de teste que guarda as mensagens. | Tentativa com `password = "Segredo#123"` e e-mail inexistente. | A mensagem registada contém o e-mail e o motivo, e **não** contém `Segredo#123`. | RF002 | EP-001 | CT-TRV-08 |
| UT-F001-013 | P | `REC_A` ativo, perfil `Rececionista`; `jti` não revogado. | `utilizadorId` de `REC_A`, `perfil = Rececionista`, `jti = "j1"`. | Devolve `true`. | RF004 | EP-003 | CT-F001-04 |
| UT-F001-014 | N | `REC_A` passou a `Bloqueado` depois da emissão do token. | Claims do token de `REC_A`. | Devolve `false` (o token deixa de dar acesso de imediato). | RF009 | EP-003 | CT-F001-07 |
| UT-F001-015 | N | Token emitido com `Rececionista`; na base de dados o perfil é agora `Veterinario`. | Claims do token antigo. | Devolve `false` (a alteração de perfil da I066 tem efeito imediato). | RF008 | EP-003 | CT-F001-07 |
| UT-F001-016 | N | `TokenRevogado` com `jti = "j1"`. | Claims com `jti = "j1"`. | Devolve `false`. | RF003 | EP-002 | CT-F001-03 |
| UT-F001-017 | N | Nenhum utilizador com `Id = 999`. | `utilizadorId = 999`. | Devolve `false`. | RF004 | EP-003 | CT-F001-04 |
| UT-F001-018 | P | `VET_A` ativo, com linha em `Funcionario`. | `utilizadorId` de `VET_A`. | `UtilizadorAutenticadoDto` com o nome de `Funcionario.Nome`, `perfil = Veterinario` e `clienteId = null`. | RF004, RF005 | EP-003 | CT-F001-01 |
| UT-F001-019 | P | `CLIENTE_A` ativo. | `utilizadorId` de `CLIENTE_A`. | `clienteId` igual a `Cliente.Id` e nome de `Cliente.Nome`. | RF005 | EP-003 | CT-F001-01 |
| UT-F001-020 | N | Conta bloqueada. | `utilizadorId` da conta bloqueada. | `UnauthorizedAccessException` (401). | RF009 | EP-003 | CT-F001-07 |
| UT-F001-021 | P | Token com `jti = "j2"` e `exp = RELOGIO + 8 h`. | `jti = "j2"`, `utilizadorId` de `REC_A`, `exp` do token. | Cria `TokenRevogado` com `Jti = "j2"`, `DataExpiracao` = `exp` e `DataRevogacao = RELOGIO`. | RF003 | EP-002 | CT-F001-03 |
| UT-F001-022 | F | Já existe `TokenRevogado` com `jti = "j2"`. | Segundo pedido com `jti = "j2"`. | Não lança exceção nem cria uma segunda linha (operação idempotente). | RF003 | EP-002 | CT-F001-03 |

#### 7.1.2. Emissão e validação do token — `JwtTokenService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-023 | P | `RELOGIO_JWT = 2026-10-25T10:00:00.567Z`. | Utilizador `Id = 12`, `Perfil = Cliente`, `ClienteId = 7`. | Token com `sub = "12"`, `email`, `role = Cliente`, `clienteId = "7"` e `jti` igual a `TokenEmitido.Jti`; **sem** `veterinarioId`. | RF001, RF005 | EP-001 | CT-F001-01 |
| UT-F001-024 | P | — | Utilizador `Id = 4`, `Perfil = Veterinario`. | `veterinarioId = "4"` e **sem** `clienteId`. | RF005 | EP-001 | CT-F001-01 |
| UT-F001-025 | F | `RELOGIO_JWT` (com milissegundos). | Qualquer utilizador. | `ExpiraEm = 2026-10-25T18:00:00Z` exatamente (8 h, sem frações de segundo) e igual à claim `exp`. | RF004 | EP-001 | CT-F001-01 |
| UT-F001-026 | P | — | Dois tokens para o mesmo utilizador. | Os dois `jti` são diferentes. | RF003 | EP-001 | CT-F001-01 |
| UT-F001-027 | P | — | Qualquer utilizador. | Cabeçalho do token com `alg = HS256`. | RF001 | EP-001 | CT-F001-01 |
| UT-F001-028 | P | Parâmetros de validação obtidos de `AddAutenticacaoJwt` (configuração real da API). | Token emitido agora com a mesma chave. | Validação com sucesso e claim `role` lida com o nome curto. | RF004 | EP-003 | CT-F001-04 |
| UT-F001-029 | N | Token emitido com o relógio 8 h 01 min no passado. | Esse token. | Validação falha com `SecurityTokenExpiredException`. | RF004 | EP-003 | CT-F001-04 |
| UT-F001-030 | F | Token emitido com o relógio 7 h 59 min no passado. | Esse token. | Validação com sucesso (sem tolerância de relógio, mas ainda dentro das 8 h). | RF004 | EP-003 | CT-F001-04 |
| UT-F001-031 | N | Token assinado com outra chave de 256 bits. | Esse token. | Validação falha. | RF004 | EP-003 | CT-F001-04 |
| UT-F001-032 | N | Token válido de um Cliente. | O mesmo token com o payload de um token de Administrador e a assinatura original. | Validação falha. | RF004, RF005 | EP-003 | CT-F001-04 |
| UT-F001-033 | N | — | Token com `alg = none` e sem assinatura. | Validação falha (só HS256 é aceite). | RF004 | EP-003 | CT-F001-04 |
| UT-F001-034 | N | — | Token assinado com a chave certa mas `iss = "OutroEmissor"`. | Validação falha. | RF004 | EP-003 | CT-F001-04 |

#### 7.1.3. Configuração JWT — `JwtOptionsValidator`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-035 | N | — | `Key = ""`. | Falha com mensagem que indica `Jwt:Key` e os User Secrets. | RF004 | — | CT-F001-04 |
| UT-F001-036 | F | — | Chave Base64 com 31 bytes. | Falha: mínimo 32 bytes (256 bits). | RF004 | — | CT-F001-04 |
| UT-F001-037 | F | — | Chave Base64 com 32 bytes, `Issuer`, `Audience` e `ExpirationHours = 8`. | Sucesso. | RF004 | — | CT-F001-04 |
| UT-F001-038 | N | — | `Key = "isto não é base64"`. | Falha com mensagem sobre Base64. | RF004 | — | CT-F001-04 |
| UT-F001-039 | N | — | `ExpirationHours = 0`. | Falha. | RF004 | — | CT-F001-04 |

#### 7.1.4. Políticas de autorização por perfil e de propriedade

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-040 | P | Contentor de serviços com `AddAutenticacaoJwt`. | Política `Administrador` com um utilizador de cada um dos 5 perfis (parametrizado, 5 linhas). | Só `Administrador` passa; os outros 4 perfis são recusados. | RF005 | EP-004 a EP-010 | CT-F001-05 |
| UT-F001-041 | P | Idem. | Política `Rececionista`, 5 perfis. | Só `Rececionista` passa. | RF005 | EP-021, EP-022, EP-025 a EP-028 | CT-TRV-01 |
| UT-F001-042 | P | Idem. | Política `Veterinario`, 5 perfis. | Só `Veterinario` passa. | RF005 | EP-031, EP-032, EP-042, EP-043 | CT-F004-01 |
| UT-F001-043 | P | Idem. | Política `AuxiliarVeterinario`, 5 perfis. | Só `AuxiliarVeterinario` passa. | RF005 | EP-029 | CT-F003-07 |
| UT-F001-044 | P | Idem. | Política `Cliente`, 5 perfis. | Só `Cliente` passa. | RF005 | EP-019, EP-020, EP-030, EP-044 | CT-TRV-02 |
| UT-F001-045 | P | Idem. | Política `Funcionario`, 5 perfis. | Os 4 perfis internos passam; `Cliente` é recusado. | RF005 | — | CT-TRV-02 |
| UT-F001-046 | P | Idem. | Política `ClienteProprietario`; `Cliente` com claim `clienteId = 7`. | Passa. | RF018 | EP-019, EP-020, EP-030, EP-044 | CT-TRV-03 |
| UT-F001-047 | N | Idem. | Política `ClienteProprietario`; `Cliente` **sem** `clienteId`. | Recusado. | RF018 | EP-019 | CT-TRV-03 |
| UT-F001-048 | N | Idem. | Política `ClienteProprietario`; cada perfil interno **com** `clienteId = 7` (parametrizado). | Todos recusados: a claim não substitui o perfil. | RF018 | EP-019 | CT-TRV-02 |
| UT-F001-049 | N | Idem. | Política por omissão (fallback) com um utilizador não autenticado. | Recusado (resultaria em 401). | RF004 | Todos | CT-F001-04 |

#### 7.1.5. Palavra-passe — `ValidadorPalavraPasse` (regra R-PWD)

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-050 | P | Regra proposta R-PWD (secção 4.4). | `"Temp#2026"`. | Válida. | RF006, RF010 | EP-006, EP-013 | CT-F001-05 |
| UT-F001-051 | F | R-PWD. | `"Abcde#1x"` (8 caracteres). | Válida. | RF006 | EP-006 | CT-F001-05 |
| UT-F001-052 | F | R-PWD. | `"Abcd#1x"` (7 caracteres). | Inválida: «A palavra-passe deve ter pelo menos 8 caracteres.». | RF006 | EP-006 | CT-F001-05 |
| UT-F001-053 | F | R-PWD. | 128 caracteres com as quatro classes. | Válida. | RF006 | EP-006 | CT-F001-05 |
| UT-F001-054 | F | R-PWD. | 129 caracteres com as quatro classes. | Inválida (máximo 128). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-055 | N | R-PWD. | `"temp#2026"`. | Inválida (falta maiúscula). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-056 | N | R-PWD. | `"TEMP#2026"`. | Inválida (falta minúscula). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-057 | N | R-PWD. | `"Temp#abcd"`. | Inválida (falta algarismo). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-058 | N | R-PWD. | `"Temp2026a"`. | Inválida (falta símbolo). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-059 | N | R-PWD. | `null` e `""` (parametrizado). | Inválida (obrigatória). | RF006 | EP-006 | CT-F001-05 |

#### 7.1.6. NIF — `ValidadorNif` (regra R-NIF)

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-060 | P | Regra R-NIF (secção 4.4). | `"501234560"` (resto 1, dígito de controlo 0). | Válido. | RF010, RF013 | EP-010, EP-013 | CT-F001-09 |
| UT-F001-061 | P | R-NIF. | `"234567899"` (resto 2, dígito de controlo 9). | Válido. | RF010 | EP-013 | CT-F001-09 |
| UT-F001-062 | N | R-NIF. | `"501234567"`. | Inválido (dígito de controlo). | RF010 | EP-013 | CT-F001-09 |
| UT-F001-063 | N | R-NIF. | `"50123456"`. | Inválido (comprimento). | RF010 | EP-013 | CT-F001-09 |
| UT-F001-064 | N | R-NIF. | `"5012345600"`. | Inválido (comprimento). | RF010 | EP-013 | CT-F001-09 |
| UT-F001-065 | N | R-NIF. | `"50123456A"`. | Inválido (caracteres não numéricos). | RF010 | EP-013 | CT-F001-09 |
| UT-F001-066 | F | R-NIF. | `"501 234 560"`. | Inválido: não há normalização de espaços. | RF010 | EP-013 | CT-F001-09 |

#### 7.1.7. E-mail — `ValidadorEmail` (regra R-EMAIL)

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-067 | P | Regra R-EMAIL (secção 4.4). | `"joao.costa@mail.pt"`. | Válido. | RF006, RF010 | EP-006, EP-013 | CT-F001-05 |
| UT-F001-068 | N | R-EMAIL. | `"joao.costa.mail.pt"`. | Inválido. | RF006, RF010 | EP-006, EP-013 | CT-F001-05 |
| UT-F001-069 | N | R-EMAIL. | `"joao@mail"`. | Inválido. | RF006, RF010 | EP-006, EP-013 | CT-F001-05 |
| UT-F001-070 | N | R-EMAIL. | `"joao costa@mail.pt"`. | Inválido. | RF006, RF010 | EP-006, EP-013 | CT-F001-05 |
| UT-F001-071 | F | R-EMAIL. | E-mail válido com 256 caracteres. | Válido (limite da coluna `Utilizador.Email`). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-072 | F | R-EMAIL. | E-mail válido com 257 caracteres. | Inválido. | RF006 | EP-006 | CT-F001-05 |

#### 7.1.8. Contas de funcionários — `UtilizadoresService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-073 | P | Sem conta com o e-mail indicado. | `CreateUtilizadorDto` do exemplo da API (Auxiliar, `podeRegistarApoio = true`, `passwordInicial = "Temp#2026"`). | Cria `Utilizador` (estado `Ativo`) e `Funcionario` com `PodeRegistarApoio = true`; `PasswordHash` vem de `IPasswordHasher` e é diferente de `Temp#2026`; devolve `UtilizadorListItemDto`. | RF006, RF008 | EP-006 | CT-F001-05 |
| UT-F001-074 | P | Idem. | Perfil `Veterinario`, `podeRegistarApoio = false`. | Cria a conta com `PodeRegistarApoio = false`. | RF006, RF008 | EP-006 | CT-F001-11 |
| UT-F001-075 | N | — | Perfil `Cliente`. | `UnprocessableEntityException` (422); nada é gravado. | RF006 | EP-006 | CT-F001-05 |
| UT-F001-076 | N | — | Perfil `Rececionista`, `podeRegistarApoio = true`. | 422; nada é gravado. | RF008 | EP-006 | CT-F001-11 |
| UT-F001-077 | N | Já existe conta com `rui.lopes@clinica-g08.test`. | Mesmo e-mail. | Conflito (409); nada é gravado. | RF006 | EP-006 | CT-F001-05 |
| UT-F001-078 | N | — | `nome = ""`. | 400 com erro no campo `nome`. | RF006 | EP-006 | CT-F001-05 |
| UT-F001-079 | N | — | `email = "rui.lopes"`. | 400 com erro no campo `email` (usa `ValidadorEmail`). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-080 | N | — | `passwordInicial = "12345678"`. | 400 com erro no campo `passwordInicial` (usa `ValidadorPalavraPasse`). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-081 | F | — | `nome` com 150 caracteres. | Aceite (limite de `Funcionario.Nome`). | RF006 | EP-006 | CT-F001-05 |
| UT-F001-082 | F | — | `nome` com 151 caracteres. | 400. | RF006 | EP-006 | CT-F001-05 |
| UT-F001-083 | P | `REC_A` existe; Administrador autenticado é `ADMIN_A`. | `UpdateUtilizadorDto` com `perfil = Veterinario`, `podeRegistarApoio = false`. | Perfil e dados atualizados; devolve `UtilizadorDto`. | RF007, RF008 | EP-007 | CT-F001-05 |
| UT-F001-084 | N | `REC_A` existe. | `perfil = Cliente`. | 400 (em EP-007 o contrato prevê 400, e não 422 como em EP-006). | RF008 | EP-007 | CT-F001-05 |
| UT-F001-085 | N | Autenticado `ADMIN_A`. | Atualizar `ADMIN_A` com `perfil = Rececionista`. | 422; o perfil mantém-se `Administrador`. | RF008 | EP-007 | CT-F001-06 |
| UT-F001-086 | P | Autenticado `ADMIN_A`. | Atualizar `ADMIN_A` com outro nome e `perfil = Administrador`. | Aceite: o Administrador pode alterar os seus dados sem perder o perfil. | RF007 | EP-007 | CT-F001-06 |
| UT-F001-087 | N | `VET_A` existe. | `perfil = Veterinario`, `podeRegistarApoio = true`. | 422. | RF008 | EP-007 | CT-F001-11 |
| UT-F001-088 | N | `REC_A` e `VET_A` existem. | Atualizar `REC_A` com o e-mail de `VET_A`. | 409. | RF007 | EP-007 | CT-F001-05 |
| UT-F001-089 | F | `REC_A` existe. | Atualizar `REC_A` mantendo o próprio e-mail. | Aceite: o próprio e-mail não conta como conflito. | RF007 | EP-007 | CT-F001-05 |
| UT-F001-090 | N | — | `id = 999`. | 404. | RF007 | EP-007 | CT-F001-05 |
| UT-F001-091 | N | `REC_A` existe. | `email = "rec"`. | 400. | RF007 | EP-007 | CT-F001-05 |
| UT-F001-092 | P | `REC_A` ativo; autenticado `ADMIN_A`. | `estado = Bloqueado`, `motivo` preenchido. | `Utilizador.Estado = Bloqueado`. | RF009 | EP-008 | CT-F001-07 |
| UT-F001-093 | N | `REC_A` ativo. | `estado = Ativo`. | 400 (só é aceite `Bloqueado`). | RF009 | EP-008 | CT-F001-07 |
| UT-F001-094 | N | `UTILIZADOR_BLOQUEADO`. | `estado = Bloqueado`. | 422. | RF009 | EP-008 | CT-F001-07 |
| UT-F001-095 | N | Autenticado `ADMIN_A`. | Bloquear `ADMIN_A`. | 422; a conta mantém-se ativa. | RF009 | EP-008 | CT-F001-06 |
| UT-F001-096 | N | `CLIENTE_A` ativo. | Bloquear `CLIENTE_A`. | 422 (EP-008 é só para funcionários). | RF009 | EP-008 | CT-F001-07 |
| UT-F001-097 | N | — | `id = 999`. | 404. | RF009 | EP-008 | CT-F001-07 |
| UT-F001-098 | P | Contas de todos os perfis, incluindo Clientes. | `perfil = Veterinario`, `estado = Ativo`. | Só Veterinários ativos; **nunca** contas de Cliente; envelope com `total` correto. | RF007, RF009 | EP-004 | CT-F001-05 |
| UT-F001-099 | N | — | `perfil = "Gestor"`. | 400. | RF007 | EP-004 | CT-F001-05 |

#### 7.1.9. Dados e horário da clínica — `ClinicaService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-100 | P | `Clinica` com `Id = 1`; `DuracaoPadraoConsultaMinutos = 30`; `HORARIO_PADRAO`. | — | `ClinicaDto` com `duracaoConsultaMinutos = 30` e 11 períodos; `DiaSemana = 1` convertido em `Segunda`. | RF012, RF019 | EP-009 | CT-F001-08 |
| UT-F001-101 | F | Período com `DiaSemana = 7`. | — | Convertido em `Domingo` (1 = Segunda, …, 7 = Domingo). | RF019 | EP-009 | CT-F001-08 |
| UT-F001-102 | P | Clínica existente. | `UpdateClinicaDto` válido com o `HORARIO_PADRAO`. | Dados, duração e horário substituídos; devolve `ClinicaDto`. | RF013, RF019 | EP-010 | CT-F001-08 |
| UT-F001-103 | F | — | Período `Segunda 09:00–09:00`. | 400. | RF019 | EP-010 | CT-F001-08 |
| UT-F001-104 | N | — | Período `Segunda 14:00–13:00`. | 400. | RF019 | EP-010 | CT-F001-08 |
| UT-F001-105 | N | — | `Segunda 09:00–13:00` e `Segunda 12:00–15:00`. | 400 (períodos sobrepostos no mesmo dia). | RF019 | EP-010 | CT-F001-08 |
| UT-F001-106 | F | — | `Segunda 09:00–13:00` e `Segunda 13:00–15:00`. | Aceite: um período que acaba às 13:00 não se sobrepõe a outro que começa às 13:00. | RF019 | EP-010 | CT-F001-08 |
| UT-F001-107 | P | — | `Segunda 09:00–13:00` e `Terca 09:00–13:00`. | Aceite. | RF019 | EP-010 | CT-F001-08 |
| UT-F001-108 | N | — | `abertura = "9h00"` e `fecho = "25:00"` (parametrizado). | 400 (formato `HH:mm`). | RF019 | EP-010 | CT-F001-08 |
| UT-F001-109 | N | — | `duracaoConsultaMinutos = 0`. | 400. | RF013 | EP-010 | CT-F001-08 |
| UT-F001-110 | N | — | `horarioFuncionamento = []`. | 400 (pelo menos um período). | RF019 | EP-010 | CT-F001-08 |
| UT-F001-111 | N | — | `nif = "501234567"`. | 400 (usa `ValidadorNif`). | RF013 | EP-010 | CT-F001-08 |

#### 7.1.10. Clientes — `ClientesService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F001-112 | P | Sem Cliente com o NIF ou o e-mail indicados. | `CreateClienteDto` com NIF válido e `passwordInicial = "Temp#2026"`. | Cria `Utilizador` (perfil `Cliente`, estado `Ativo`) e `Cliente` ligado a ele; o e-mail fica só em `Utilizador.Email`; uma única gravação. | RF010 | EP-013 | CT-F001-09 |
| UT-F001-113 | P | — | `nif = null`. | Aceite (NIF opcional). | RF010 | EP-013 | CT-F001-09 |
| UT-F001-114 | N | — | `nif = "501234567"`. | 400. | RF010 | EP-013 | CT-F001-09 |
| UT-F001-115 | N | — | `email = "joao"`. | 400. | RF010 | EP-013 | CT-F001-09 |
| UT-F001-116 | N | — | `telefone = ""`. | 400. | RF010 | EP-013 | CT-F001-09 |
| UT-F001-117 | N | `CLIENTE_A` com NIF `210000015`. | Mesmo NIF. | 409; nada é gravado. | RF010 | EP-013 | CT-F001-09 |
| UT-F001-118 | N | Já existe conta com o e-mail. | Mesmo e-mail. | 409; nada é gravado. | RF010 | EP-013 | CT-F001-09 |
| UT-F001-119 | P | `CLIENTE_A` existe. | `UpdateClienteDto` com novo e-mail. | `Cliente` e `Utilizador.Email` atualizados na mesma gravação. | RF011 | EP-014 | CT-F001-09 |
| UT-F001-120 | N | `CLIENTE_A` e `CLIENTE_B` existem. | Atualizar `CLIENTE_A` com o NIF de `CLIENTE_B`. | 409. | RF011 | EP-014 | CT-F001-09 |
| UT-F001-121 | F | `CLIENTE_A` existe. | Atualizar `CLIENTE_A` mantendo o próprio NIF. | Aceite. | RF011 | EP-014 | CT-F001-09 |
| UT-F001-122 | N | `CLIENTE_A` existe. | `nif = "12345"`. | 400. | RF011 | EP-014 | CT-F001-09 |
| UT-F001-123 | N | — | `id = 999`. | 404. | RF011 | EP-014 | CT-F001-09 |
| UT-F001-124 | P | `CLIENTE_A` (Costa, 2 animais) e `CLIENTE_B`. | `pesquisa = "Costa"`. | Só `CLIENTE_A`, com `numeroAnimais = 2`. | RF011, RF017 | EP-011 | CT-F001-09 |
| UT-F001-125 | P | `CLIENTE_A` com `ANIMAL_A1` e `ANIMAL_A2`. | `id` de `CLIENTE_A`. | `ClienteDto` com o e-mail da conta, `estadoConta` e os dois animais. | RF011, RF017 | EP-012 | CT-F001-09 |

### 7.2. F002 — Gestão de pacientes

Tarefa de implementação: **I086**.

#### 7.2.1. Pacientes — `AnimaisService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F002-001 | P | `CLIENTE_A` existe; espécie `Cão` e raça `Labrador Retriever` existem. | `CreateAnimalDto` com `nome = "Bobi"`, `especie = "Cão"`, `raca = "Labrador Retriever"`, `sexo = Macho`, `dataNascimento = 2022-05-10`, `peso = 28.4`, `clienteId` de `CLIENTE_A`. | Cria `Paciente` ligado a `CLIENTE_A` com `DataRegisto = RELOGIO`; devolve `AnimalDto` com `idadeAnos = 4`. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-002 | P | Idem. | Sem `raca`, `dataNascimento` e `peso`. | Aceite; os três campos ficam vazios. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-003 | N | — | `clienteId = 0` (em falta). | 400 com erro em `clienteId`. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-004 | N | — | `clienteId = 999`. | `KeyNotFoundException` (404); nada é gravado. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-005 | N | — | `nome = ""`. | 400. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-006 | F | — | `nome` com 81 caracteres. | 400 (máximo 80, `Paciente.Nome`). | RF014 | EP-017 | CT-F002-01 |
| UT-F002-007 | F | — | `peso = 0`. | 400 («O peso deve ser superior a 0.»). | RF014 | EP-017 | CT-F002-01 |
| UT-F002-008 | F | — | `peso = 0.01`. | Aceite. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-009 | N | — | `peso = -3`. | 400. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-010 | F | `RELOGIO` (hoje é 2026-10-14). | `dataNascimento = 2026-10-15`. | 400 (data futura). | RF014 | EP-017 | CT-F002-01 |
| UT-F002-011 | F | `RELOGIO`. | `dataNascimento = 2026-10-14`. | Aceite; `idadeAnos = 0`. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-012 | N | — | `especie = "Dragão"`. | 400 com erro em `especie`. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-013 | N | — | `especie = "Cão"`, `raca = "Siamês"` (raça de gato). | 400 com erro em `raca`. | RF014 | EP-017 | CT-F002-01 |
| UT-F002-014 | P | `ANIMAL_A1` existe. | `UpdateAnimalDto` com `peso = 29.1`. | Peso atualizado e `DataUltimaAlteracao = RELOGIO`. | RF016 | EP-018 | CT-F002-01 |
| UT-F002-015 | N | `ANIMAL_A1` existe. | `peso = 0`. | 400; nada é alterado. | RF016 | EP-018 | CT-F002-01 |
| UT-F002-016 | N | — | `id = 999`. | 404. | RF016 | EP-018 | CT-F002-01 |
| UT-F002-017 | N | `ANIMAL_A1` existe. | `clienteId = 999`. | 404. | RF016 | EP-018 | CT-F002-01 |
| UT-F002-018 | P | `ANIMAL_A1` de `CLIENTE_A`. | `id` de `ANIMAL_A1`. | `AnimalDto` só com dados administrativos e `cliente.telefone` preenchido. | RF015 | EP-016 | CT-F002-03 |
| UT-F002-019 | P | `ANIMAL_A1`, `ANIMAL_A2` (de `CLIENTE_A`) e `ANIMAL_B1` (de `CLIENTE_B`). | `clienteId` de `CLIENTE_A`. | Devolve só `ANIMAL_A1` e `ANIMAL_A2`. | RF017 | EP-015 | CT-F002-02 |
| UT-F002-020 | P | Idem. | `clienteId` do token de `CLIENTE_A`. | Só os animais de `CLIENTE_A`. | RF018 | EP-019 | CT-F002-05 |
| UT-F002-021 | F | `CLIENTE_C` sem animais. | `clienteId` do token de `CLIENTE_C`. | Lista vazia (`[]`), sem erro. | RF018 | EP-019 | CT-F002-05 |
| UT-F002-022 | P | `ANIMAL_A1` de `CLIENTE_A`. | Token de `CLIENTE_A`, `id` de `ANIMAL_A1`. | `MeuAnimalDto` do animal. | RF018 | EP-020 | CT-F002-05 |
| UT-F002-023 | N | `ANIMAL_B1` de `CLIENTE_B`. | Token de `CLIENTE_A`, `id` de `ANIMAL_B1`. | `ForbiddenException` (403); nenhum dado de `ANIMAL_B1` devolvido. | RF018 | EP-020 | CT-F002-05 |
| UT-F002-024 | N | — | Token de `CLIENTE_A`, `id = 999`. | 404. | RF018 | EP-020 | CT-F002-05 |

#### 7.2.2. Idade do animal — `CalculadoraIdade`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F002-025 | P | Hoje = 2026-10-14. | `dataNascimento = 2022-05-10`. | 4. | RF015 | EP-016 | CT-F002-01 |
| UT-F002-026 | F | Hoje = 2026-10-14. | `dataNascimento = 2022-10-14`. | 4 (faz anos hoje). | RF015 | EP-016 | CT-F002-01 |
| UT-F002-027 | F | Hoje = 2026-10-14. | `dataNascimento = 2022-10-15`. | 3 (faz anos amanhã). | RF015 | EP-016 | CT-F002-01 |
| UT-F002-028 | F | Hoje = 2025-02-28. | `dataNascimento = 2024-02-29`. | 0. | RF015 | EP-016 | CT-F002-01 |
| UT-F002-029 | F | Hoje = 2025-03-01. | `dataNascimento = 2024-02-29`. | 1. | RF015 | EP-016 | CT-F002-01 |
| UT-F002-030 | F | — | `dataNascimento = null`. | 0 (proposta, ver ponto em aberto PA-09). | RF015 | EP-016 | CT-F002-01 |

### 7.3. F003 — Agendamento de consultas

Tarefa de implementação: **I087**.

#### 7.3.1. Disponibilidade — `CalculadoraDisponibilidade`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F003-001 | P | `HORARIO_PADRAO`, duração 30, `FUSO`; `VET_A` sem marcações; `RELOGIO`. | `VET_A`, `dataInicio = 2026-10-15` (quinta). | 18 horários: `09:00` a `12:30` e `14:00` a `18:30`, de 30 em 30 minutos. | RF020, RF019 | EP-022 | CT-F003-02 |
| UT-F003-002 | P | Idem; marcação `Confirmada` de `VET_A` às 10:00 locais (09:00Z) de 2026-10-15. | `VET_A`, 2026-10-15. | `10:00` ausente; `09:30` e `10:30` presentes. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-003 | F | Marcações de `VET_A` às 10:00 e às 10:30 locais. | `VET_A`, 2026-10-15. | `10:00` e `10:30` ausentes; `09:30` e `11:00` presentes. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-004 | F | Sem marcações. | `VET_A`, 2026-10-15. | `12:30` presente (acaba às 13:00); `13:00` ausente; `18:30` presente e `19:00` ausente. | RF020, RF019 | EP-022 | CT-F003-02 |
| UT-F003-005 | F | Marcação `Cancelada` de `VET_A` às 10:00 locais. | `VET_A`, 2026-10-15. | `10:00` presente: as canceladas não ocupam horário. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-006 | F | Domingo sem períodos no `HORARIO_PADRAO`. | `VET_A`, `dataInicio = 2026-10-18` (domingo). | Dia devolvido com lista vazia. | RF020, RF019 | EP-022 | CT-F003-02 |
| UT-F003-007 | F | Dia com dois períodos (09:00–13:00 e 14:00–19:00). | `VET_A`, 2026-10-15. | Nenhum horário que comece entre `13:00` e `13:59`. | RF019 | EP-022 | CT-F003-02 |
| UT-F003-008 | F | Duração configurada = 45; período 09:00–13:00. | `VET_A`, 2026-10-15. | Manhã: `09:00`, `09:45`, `10:30`, `11:15`, `12:00` (`12:45` não cabe). | RF020, RF019 | EP-022 | CT-F003-02 |
| UT-F003-009 | F | Marcação de `VET_A` às 10:15 locais com 60 minutos. | `VET_A`, 2026-10-15. | `10:00` e `11:00` ausentes (sobrepõem-se a 10:15–11:15); `09:30` e `11:30` presentes. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-010 | F | Marcação de `VET_B` às 10:00 locais. | `VET_A`, 2026-10-15. | `10:00` presente para `VET_A`. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-011 | F | Hora de inverno desde 2026-10-25; marcação de `VET_A` guardada como `2026-10-26T09:00:00Z`. | `VET_A`, `dataInicio = 2026-10-26` (segunda). | `09:00` ausente (09:00Z = 09:00 em Lisboa no inverno) e `10:00` presente. Em 2026-10-15 a mesma hora UTC corresponderia às 10:00 locais. | RF020, RF019 | EP-022 | CT-F003-02 |
| UT-F003-012 | F | Agora = `2026-10-14T08:10:00Z` (09:10 em Lisboa). | `VET_A`, `dataInicio = 2026-10-14`. | O primeiro horário é `09:30`: horários que já começaram não são devolvidos (proposta PA-05). | RF020 | EP-022 | CT-F003-02 |
| UT-F003-013 | F | — | `dataInicio = 2026-10-01`, `dataFim = 2026-10-31` (31 dias). | Aceite; 31 dias na resposta. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-014 | F | — | `dataInicio = 2026-10-01`, `dataFim = 2026-11-01` (32 dias). | 400. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-015 | N | — | `dataInicio = 2026-10-15`, `dataFim = 2026-10-14`. | 400. | RF020 | EP-022 | CT-F003-02 |
| UT-F003-016 | N | — | Sem `dataInicio`. | 400 (obrigatório). | RF020 | EP-022 | CT-F003-02 |
| UT-F003-017 | P | — | `dataInicio = 2026-10-15`, sem `dataFim`. | Um único dia (2026-10-15). | RF020 | EP-022 | CT-F003-02 |
| UT-F003-018 | N | — | `id` de `REC_A` (não é Veterinário) e `id = 999` (parametrizado). | 404 nos dois. | RF020 | EP-022 | CT-F003-02 |

#### 7.3.2. Marcações — `ConsultasService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F003-019 | P | `RELOGIO`; `HORARIO_PADRAO`; `VET_A` livre; `ANIMAL_A1` existe; autenticado `REC_A`. | `CreateConsultaDto` com `animalId` de `ANIMAL_A1`, `veterinarioId` de `VET_A`, `dataHora = 2026-10-15T09:00:00Z` (10:00 locais). | Marcação `Pendente`, `DuracaoMinutos = 30` (da configuração), `CriadaPorId` = `REC_A`; devolve `ConsultaDto`. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-020 | N | `RELOGIO`. | `dataHora = RELOGIO − 1 min`. | 400 («A data da marcação deve ser futura.»). | RF021 | EP-025 | CT-F003-01 |
| UT-F003-021 | F | `RELOGIO`. | `dataHora = RELOGIO` (09:00 locais de 2026-10-14). | 400: tem de ser estritamente futura. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-022 | N | — | `animalId = 0` e `veterinarioId = 0` (parametrizado). | 400. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-023 | N | — | `dataHora = 2026-10-18T10:00:00Z` (domingo). | 422 (fora do horário de funcionamento). | RF021, RF019 | EP-025 | CT-F003-01 |
| UT-F003-024 | F | — | Início às 12:45 locais (30 min → acaba às 13:15). | 422: a consulta tem de caber toda no período. | RF021, RF019 | EP-025 | CT-F003-01 |
| UT-F003-025 | F | — | Início às 13:00 locais. | 422. | RF021, RF019 | EP-025 | CT-F003-01 |
| UT-F003-026 | F | — | Início às 12:30 locais (acaba às 13:00). | Aceite. | RF021, RF019 | EP-025 | CT-F003-01 |
| UT-F003-027 | N | Marcação `Confirmada` de `VET_A` às 10:00 locais. | Nova marcação de `VET_A` às 10:00 locais. | 422 (veterinário indisponível). | RF021, RF020 | EP-025 | CT-F003-01 |
| UT-F003-028 | F | Marcação de `VET_A` das 10:15 às 10:45 locais. | Nova marcação às 10:30 locais. | 422. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-029 | F | Marcação de `VET_A` das 10:00 às 10:30 locais. | Nova marcação às 10:30 locais. | Aceite. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-030 | F | Marcação `Cancelada` de `VET_A` às 10:00 locais. | Nova marcação às 10:00 locais. | Aceite. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-031 | N | — | `animalId = 999`. | 404. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-032 | N | — | `veterinarioId = 999` e `veterinarioId` de `REC_A` (parametrizado). | 404 nos dois. | RF021 | EP-025 | CT-F003-01 |
| UT-F003-033 | P | `CONSULTA_PENDENTE` de `VET_A`; `VET_B` livre. | `veterinarioId` de `VET_B`, nova data livre. | Marcação atualizada; estado mantém-se `Pendente`. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-034 | P | `CONSULTA_CONFIRMADA`. | Novo horário livre. | Aceite; estado mantém-se `Confirmada`. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-035 | N | `CONSULTA_REALIZADA`. | Novo horário livre. | 422; nada é alterado. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-036 | N | `CONSULTA_CANCELADA`. | Novo horário livre. | 422. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-037 | N | `CONSULTA_PENDENTE` e outra marcação de `VET_A` às 11:00 locais. | Mudar para as 11:00 locais. | 422. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-038 | F | `CONSULTA_PENDENTE` de `VET_A` das 10:00 às 10:30 locais. | Mudar para as 10:15 locais. | Aceite: a própria marcação não conta como ocupação. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-039 | N | `CONSULTA_PENDENTE`. | `dataHora = RELOGIO − 1 h`. | 400. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-040 | N | — | `id = 999`. | 404. | RF022 | EP-026 | CT-F003-04 |
| UT-F003-041 | N | `CONSULTA_PENDENTE` com versão `V2`. | `If-Match` com a versão `V1`. | Pré-condição falhada (412); a marcação fica como estava. | RF022 | EP-026 | CT-F003-09 |
| UT-F003-042 | P | Idem. | `If-Match` com a versão `V2`. | Aceite. | RF022 | EP-026 | CT-F003-09 |
| UT-F003-043 | P | Idem. | Sem `If-Match`. | Aceite (o cabeçalho é opcional). | RF022 | EP-026 | CT-F003-09 |
| UT-F003-044 | N | `CONSULTA_PENDENTE` com versão `V2`. | `estado = Confirmada`, `If-Match` com `V1`. | 412; o estado mantém-se `Pendente`. | RF023 | EP-027 | CT-F003-09 |
| UT-F003-045 | P | `CONSULTA_CONFIRMADA` sem chegada; `RELOGIO`. | — | `ChegadaEm = RELOGIO` (hora do servidor, nunca do pedido); devolve `ChegadaDto`. | RF025 | EP-028 | CT-F003-05 |
| UT-F003-046 | N | `CONSULTA_PENDENTE`. | — | 422. | RF025 | EP-028 | CT-F003-05 |
| UT-F003-047 | N | `CONSULTA_CANCELADA`. | — | 422. | RF025 | EP-028 | CT-F003-05 |
| UT-F003-048 | F | `CONSULTA_CONFIRMADA` com chegada já registada. | Segundo registo. | 422; a primeira hora mantém-se. | RF025 | EP-028 | CT-F003-05 |
| UT-F003-049 | N | `CONSULTA_CONFIRMADA` com versão `V2`. | `If-Match` com `V1`. | 412; sem chegada registada. | RF025 | EP-028 | CT-F003-09 |
| UT-F003-050 | P | Marcações de `VET_A` e de `VET_B` no dia; autenticado `VET_A`. | Filtro com `veterinarioId` de `VET_B`. | Devolve só as marcações de `VET_A`: o filtro é ignorado e vale a claim `veterinarioId`. | RF024 | EP-023 | CT-F003-06 |
| UT-F003-051 | P | Idem; autenticado `REC_A`. | `estado = Confirmada`, sem `veterinarioId`. | Marcações `Confirmada` dos dois Veterinários. | RF024 | EP-023 | CT-F003-06 |
| UT-F003-052 | N | — | `dataInicio = 2026-10-15`, `dataFim = 2026-10-14`. | 400. | RF024 | EP-023 | CT-F003-06 |
| UT-F003-053 | P | `CONSULTA_CONFIRMADA` com uma nota de preparação; autenticado `AUX_SEM_APOIO`. | `id` da consulta. | `ConsultaDto` com a nota em `notasPreparacao` (só leitura). | RF026 | EP-024 | CT-F003-07 |
| UT-F003-054 | N | Marcação de `VET_B`; autenticado `VET_A`. | `id` dessa marcação. | `ForbiddenException` (403). | RF024 | EP-024 | CT-F003-06 |
| UT-F003-055 | P | Marcação de `VET_A`; autenticado `VET_A`. | `id` dessa marcação. | `ConsultaDto`. | RF024 | EP-024 | CT-F003-06 |
| UT-F003-056 | P | `RELOGIO`; marcações passadas e futuras de `ANIMAL_A1`, `ANIMAL_A2` e `ANIMAL_B1`. | Token de `CLIENTE_A`, `tipo = futuras`. | Só marcações futuras de `ANIMAL_A1` e `ANIMAL_A2`, por ordem crescente de data. | RF028 | EP-030 | CT-F003-08 |
| UT-F003-057 | F | Marcação de `ANIMAL_A1` com `dataHora = RELOGIO`. | `tipo = futuras`. | Incluída em `futuras` (fronteira: `dataHora ≥ agora`). | RF028 | EP-030 | CT-F003-08 |
| UT-F003-058 | P | Idem. | `tipo = historico`. | Só marcações com `dataHora < agora`, por ordem decrescente. | RF029 | EP-030 | CT-F003-08 |
| UT-F003-059 | P | Idem. | Sem `tipo`. | Igual a `tipo = futuras`. | RF028 | EP-030 | CT-F003-08 |
| UT-F003-060 | N | — | `tipo = "todas"`. | 400. | RF028 | EP-030 | CT-F003-08 |
| UT-F003-061 | N | — | Token de `CLIENTE_A`, `animalId` de `ANIMAL_B1`. | `ForbiddenException` (403). | RF028 | EP-030 | CT-F003-08 |

#### 7.3.3. Estados da marcação — `MaquinaEstadosMarcacao`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F003-062 | P | Marcação `Pendente`. | `estado = Confirmada`. | Aceite. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-063 | P | Marcação `Confirmada`. | `estado = Realizada`. | Aceite. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-064 | P | Marcação `Pendente`. | `estado = Cancelada`. | Aceite. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-065 | P | Marcação `Confirmada`. | `estado = Cancelada`. | Aceite. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-066 | N | Marcação `Pendente`. | `estado = Realizada`. | 422 (tem de ser confirmada primeiro). | RF023 | EP-027 | CT-F003-03 |
| UT-F003-067 | N | Marcação `Confirmada`. | `estado = Confirmada`. | 422 (sem mudança). | RF023 | EP-027 | CT-F003-03 |
| UT-F003-068 | N | Marcação `Realizada`. | `estado = Confirmada`. | 422 (estado final). | RF023 | EP-027 | CT-F003-03 |
| UT-F003-069 | N | Marcação `Realizada`. | `estado = Realizada`. | 422. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-070 | N | Marcação `Realizada`. | `estado = Cancelada`. | 422. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-071 | N | Marcação `Cancelada`. | `estado = Confirmada`. | 422 (estado final). | RF023 | EP-027 | CT-F003-03 |
| UT-F003-072 | N | Marcação `Cancelada`. | `estado = Realizada`. | 422. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-073 | N | Marcação `Cancelada`. | `estado = Cancelada`. | 422. | RF023 | EP-027 | CT-F003-03 |
| UT-F003-074 | N | Marcação em cada um dos 4 estados (parametrizado, 4 linhas). | `estado = Pendente`. | 400 em todos (`Pendente` nunca é destino). | RF023 | EP-027 | CT-F003-03 |
| UT-F003-075 | N | Marcação `Pendente`. | `estado = "Faltou"`. | 400 (estado inexistente). | RF023 | EP-027 | CT-F003-03 |

#### 7.3.4. Notas de preparação — `ConsultasService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F003-076 | P | `AUX_COM_APOIO`; `CONSULTA_CONFIRMADA`; `RELOGIO`. | `texto = "Sala 2 preparada."`. | Cria `InformacaoApoio` com `AuxiliarId` do token e `DataRegisto = RELOGIO`; devolve `NotaPreparacaoDto` com `autor` = nome do Auxiliar. | RF027 | EP-029 | CT-F003-07 |
| UT-F003-077 | N | `AUX_SEM_APOIO`. | Texto válido. | `ForbiddenException` (403); nada é gravado. | RF027 | EP-029 | CT-F003-07 |
| UT-F003-078 | N | `AUX_COM_APOIO`. | `texto = ""`. | 400. | RF027 | EP-029 | CT-F003-07 |
| UT-F003-079 | F | `AUX_COM_APOIO`. | `texto = "   "`. | 400 (só espaços conta como vazio). | RF027 | EP-029 | CT-F003-07 |
| UT-F003-080 | F | `AUX_COM_APOIO`. | `texto` com 1000 caracteres. | Aceite. | RF027 | EP-029 | CT-F003-07 |
| UT-F003-081 | F | `AUX_COM_APOIO`. | `texto` com 1001 caracteres. | 400. | RF027 | EP-029 | CT-F003-07 |
| UT-F003-082 | N | `AUX_COM_APOIO`; `CONSULTA_CANCELADA`. | Texto válido. | 422. | RF027 | EP-029 | CT-F003-07 |
| UT-F003-083 | F | `AUX_COM_APOIO`; `CONSULTA_REALIZADA`. | Texto válido. | Aceite: só `Cancelada` impede o registo. | RF027 | EP-029 | CT-F003-07 |
| UT-F003-084 | N | `AUX_COM_APOIO`. | `id = 999`. | 404. | RF027 | EP-029 | CT-F003-07 |

#### 7.3.5. Versão da marcação — `VersaoMarcacao`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F003-085 | P | `RowVersion = 00 00 00 00 00 00 07 D1`. | — | ETag `"AAAAAAAAB9E="` (Base64 entre aspas). | RF022 | EP-024, EP-026 | CT-F003-09 |
| UT-F003-086 | P | Idem. | `If-Match: "AAAAAAAAB9E="`. | Corresponde. | RF022 | EP-026 | CT-F003-09 |
| UT-F003-087 | N | Idem. | `If-Match: "AAAAAAAAB9I="`. | Não corresponde (→ 412). | RF022 | EP-026 | CT-F003-09 |
| UT-F003-088 | F | Idem. | `If-Match: W/"AAAAAAAAB9E="`. | Não corresponde: o `If-Match` usa comparação forte (RFC 9110). | RF022 | EP-026 | CT-F003-09 |
| UT-F003-089 | F | Idem. | `If-Match: *`. | Corresponde (existe uma versão atual). | RF022 | EP-026 | CT-F003-09 |
| UT-F003-090 | F | Idem. | `If-Match: AAAAAAAAB9E=` (sem aspas). | Não corresponde (formato inválido → 412). | RF022 | EP-026 | CT-F003-09 |

### 7.4. F004 — Registo clínico e tratamentos

Tarefa de implementação: **I088**.

#### 7.4.1. Registo clínico e histórico — `RegistosClinicosService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F004-001 | P | `CONSULTA_CONFIRMADA` de `VET_A`, sem registo; autenticado `VET_A`; `RELOGIO`. | `CreateRegistoClinicoDto` do exemplo da API (notas, diagnóstico, 1 tratamento, 1 prescrição). | Cria `RegistoClinico` com `VeterinarioId` = `VET_A` e `CriadoEm = RELOGIO`, o tratamento e a prescrição; o estado da marcação **não** muda. | RF030, RF031, RF032, RF033 | EP-031 | CT-F004-02 |
| UT-F004-002 | F | Idem. | Sem notas nem diagnóstico; uma prescrição completa. | Aceite: basta um elemento clínico. | RF033 | EP-031 | CT-F004-02 |
| UT-F004-003 | F | Idem. | Sem notas, diagnóstico nem prescrições; um tratamento. | Aceite. | RF032 | EP-031 | CT-F004-02 |
| UT-F004-004 | F | `CONSULTA_REALIZADA` de `VET_A`, sem registo. | Pedido válido. | Aceite (`Confirmada` e `Realizada` são os estados admitidos). | RF030 | EP-031 | CT-F004-03 |
| UT-F004-005 | N | — | `consultaId = 0`. | 400 com erro em `consultaId`. | RF030 | EP-031 | CT-F004-02 |
| UT-F004-006 | N | Idem. | Notas e diagnóstico vazios; listas vazias. | 400 (nenhum elemento clínico). | RF031 | EP-031 | CT-F004-02 |
| UT-F004-007 | F | Idem. | `notasClinicas = "   "`, listas vazias. | 400: só espaços não conta como elemento clínico. | RF031 | EP-031 | CT-F004-02 |
| UT-F004-008 | N | Idem. | Prescrição sem `medicamento`, sem `dosagem` e sem `frequencia` (parametrizado, 3 linhas). | 400 com erro no campo em falta. | RF033 | EP-031 | CT-F004-02 |
| UT-F004-009 | F | Idem. | Prescrição com `duracaoDias = 0`. | 400 (quando indicada, tem de ser > 0, `CK_Prescricao_Duracao`). | RF033 | EP-031 | CT-F004-02 |
| UT-F004-010 | P | Idem. | Prescrição sem `duracaoDias`. | Aceite. | RF033 | EP-031 | CT-F004-02 |
| UT-F004-011 | N | Idem. | Tratamento com `descricao = ""`. | 400. | RF032 | EP-031 | CT-F004-02 |
| UT-F004-012 | F | Idem. | `diagnostico` com 1001 caracteres. | 400 (máximo 1000). | RF031 | EP-031 | CT-F004-02 |
| UT-F004-013 | N | `CONSULTA_REALIZADA` com `REGISTO_CLINICO_A`. | Novo registo para a mesma consulta. | 409; nada é gravado. | RF030 | EP-031 | CT-F004-03 |
| UT-F004-014 | N | `CONSULTA_PENDENTE` de `VET_A`. | Pedido válido. | 422. | RF030 | EP-031 | CT-F004-03 |
| UT-F004-015 | N | `CONSULTA_CANCELADA` de `VET_A`. | Pedido válido. | 422. | RF030 | EP-031 | CT-F004-03 |
| UT-F004-016 | N | — | `consultaId = 999`. | 404. | RF030 | EP-031 | CT-F004-03 |
| UT-F004-017 | N | `CONSULTA_CONFIRMADA` de `VET_B`; autenticado `VET_A`. | Pedido válido. | `ForbiddenException` (403): só o Veterinário da consulta regista (API 3.3; verificação D16 do Modelo de Dados). | RF030 | EP-031 | CT-F004-01 |
| UT-F004-018 | P | `REGISTO_CLINICO_A` com tratamentos `T1`, `T2` e prescrição `P1`; autenticado `VET_A`; `RELOGIO`. | `tratamentos = [ {id: T1, …}, {sem id, …} ]`, `prescricoes = []`. | Fica `T1` (atualizado), um tratamento novo, `T2` e `P1` removidos; `AtualizadoEm = RELOGIO` e `AtualizadoPorId` = `VET_A`; `MarcacaoId` inalterado. | RF034, RF032, RF033 | EP-032 | CT-F004-02 |
| UT-F004-019 | N | Tratamento `T9` pertence a outro registo. | `tratamentos = [ {id: T9, …} ]`. | 400: o `id` indicado não pertence a este registo (proposta PA-10). | RF034 | EP-032 | CT-F004-02 |
| UT-F004-020 | N | `REGISTO_CLINICO_A`. | Tudo vazio. | 400 (o registo não pode ficar sem elemento clínico). | RF034, RF031 | EP-032 | CT-F004-02 |
| UT-F004-021 | N | — | `id = 999`. | 404. | RF034 | EP-032 | CT-F004-02 |
| UT-F004-022 | P | `REGISTO_CLINICO_A`; autenticado `AUX_SEM_APOIO`. | `id` do registo. | `RegistoClinicoDto` completo (contrato v02; ver PA-08). | RF035, RF036 | EP-033 | CT-F004-04 |
| UT-F004-023 | P | `ANIMAL_A1` com registos de 2026-09-02 e de 2026-10-14. | Sem datas. | Dois resumos: primeiro o de 2026-10-14, depois o de 2026-09-02. | RF035 | EP-034 | CT-F004-05 |
| UT-F004-024 | P | Registo com 1 tratamento e 2 prescrições. | — | `numeroTratamentos = 1`, `numeroPrescricoes = 2`. | RF035 | EP-034 | CT-F004-05 |
| UT-F004-025 | F | Idem ao primeiro. | `dataInicio = dataFim = 2026-10-14`. | Só o registo de 2026-10-14. | RF035 | EP-034 | CT-F004-05 |
| UT-F004-026 | N | — | `dataInicio = 2026-10-14`, `dataFim = 2026-09-01`. | 400. | RF035 | EP-034 | CT-F004-05 |
| UT-F004-027 | N | — | `id = 999`. | 404. | RF035 | EP-034 | CT-F004-05 |

### 7.5. F006 — Boletim de vacinas e desparasitações

Tarefa de implementação: **I089**.

#### 7.5.1. Próxima administração — `CalculadoraProximaAdministracao`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F006-001 | P | — | `2026-10-14` + 90 dias. | `2027-01-12` (exemplo da API). | RF045 | EP-043 | CT-F006-01 |
| UT-F006-002 | P | — | `2026-10-14`, `intervaloDias = null`. | `null`. | RF045 | EP-043 | CT-F006-01 |
| UT-F006-003 | F | — | `2026-01-31` + 1. | `2026-02-01`. | RF045 | EP-043 | CT-F006-01 |
| UT-F006-004 | F | — | `2028-02-28` + 1. | `2028-02-29`. | RF045 | EP-043 | CT-F006-01 |
| UT-F006-005 | F | — | `2027-02-28` + 1. | `2027-03-01`. | RF045 | EP-043 | CT-F006-01 |
| UT-F006-006 | F | — | `2026-12-31` + 1. | `2027-01-01`. | RF045 | EP-043 | CT-F006-01 |
| UT-F006-007 | F | — | `2027-03-01` + 365. | `2028-02-29` (o ano atravessa um 29 de fevereiro). | RF045 | EP-043 | CT-F006-01 |

#### 7.5.2. Registo e boletim — `VacinacoesService`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-F006-008 | P | `ANIMAL_A1`; autenticado `VET_A`; `RELOGIO`. | `tipo = Vacina`, `produto = "Vacina antirrábica"`, `lote = "RB-2291"`, `dataAdministracao = 2026-10-14`, `intervaloDias = 365`. | Cria `RegistoProfilaxia` com `VeterinarioId` = `VET_A`; `dataProximaPrevista = 2027-10-14`. | RF043, RF045 | EP-043 | CT-F006-02 |
| UT-F006-009 | P | Idem. | `tipo = Desparasitacao`, sem `intervaloDias`. | Cria o registo com `dataProximaPrevista = null`. | RF044, RF045 | EP-043 | CT-F006-02 |
| UT-F006-010 | N | Idem. | Sem `tipo`, sem `produto`, sem `lote` e sem `dataAdministracao` (parametrizado, 4 linhas). | 400 com erro no campo em falta. | RF043, RF044 | EP-043 | CT-F006-02 |
| UT-F006-011 | F | Idem. | `dataAdministracao = 2026-10-15` (amanhã). | 400. | RF043 | EP-043 | CT-F006-02 |
| UT-F006-012 | F | Idem. | `dataAdministracao = 2026-10-14` (hoje). | Aceite. | RF043 | EP-043 | CT-F006-02 |
| UT-F006-013 | F | Idem. | `intervaloDias = 0`. | 400. | RF045 | EP-043 | CT-F006-02 |
| UT-F006-014 | F | Idem. | `intervaloDias = 1`. | Aceite; `dataProximaPrevista = 2026-10-15`. | RF045 | EP-043 | CT-F006-02 |
| UT-F006-015 | N | Idem. | `intervaloDias = -30`. | 400. | RF045 | EP-043 | CT-F006-02 |
| UT-F006-016 | F | Idem. | `lote` com 51 caracteres. | 400 (máximo 50). | RF043 | EP-043 | CT-F006-02 |
| UT-F006-017 | N | — | `id = 999`. | 404. | RF043 | EP-043 | CT-F006-02 |
| UT-F006-018 | P | `ANIMAL_A1` com vacina de 2026-10-14 (próxima 2027-10-14) e desparasitação de 2026-08-01 (sem próxima); `RELOGIO`. | — | `administracoes` por ordem decrescente de data; `proximas` só com a vacina de 2027-10-14. | RF046, RF045 | EP-042 | CT-F006-03 |
| UT-F006-019 | P | Idem. | `tipo = Vacina`. | Só a vacina. | RF046 | EP-042 | CT-F006-03 |
| UT-F006-020 | N | Idem. | `tipo = "Vacinas"`. | 400. | RF046 | EP-042 | CT-F006-03 |
| UT-F006-021 | F | Administração com `dataProximaPrevista = 2026-10-13` (ontem). | — | Não aparece em `proximas` (só datas ≥ hoje, por ordem crescente; proposta PA-11). | RF045, RF046 | EP-042 | CT-F006-01 |
| UT-F006-022 | P | Idem a `bol-web`. | Token de `CLIENTE_A`, `id` de `ANIMAL_A1`. | `MeuBoletimDto` com o mesmo `historico` e as mesmas `proximas`. | RF047 | EP-044 | CT-F006-04 |
| UT-F006-023 | N | `ANIMAL_B1` de `CLIENTE_B`. | Token de `CLIENTE_A`, `id` de `ANIMAL_B1`. | `ForbiddenException` (403). | RF047 | EP-044 | CT-F006-04 |
| UT-F006-024 | F | `ANIMAL_A2` sem registos. | Token de `CLIENTE_A`, `id` de `ANIMAL_A2`. | `historico = []` e `proximas = []`. | RF047 | EP-044 | CT-F006-04 |

### 7.6. Transversais — paginação, erros e registos

Tarefa de implementação: **I086**.

#### 7.6.1. Paginação — `PaginacaoQuery`

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-TRV-001 | P | — | Sem `pagina` nem `tamanho`. | `pagina = 1`, `tamanho = 20`. | RF017 | EP-004, EP-011, EP-015, EP-023 | CT-TRV-07 |
| UT-TRV-002 | F | — | `tamanho = 100`. | Aceite. | RF017 | EP-015 | CT-TRV-07 |
| UT-TRV-003 | F | — | `tamanho = 101`. | 400. | RF017 | EP-015 | CT-TRV-07 |
| UT-TRV-004 | N | — | `tamanho = 0` e `pagina = 0` (parametrizado). | 400 nos dois. | RF017 | EP-015 | CT-TRV-07 |
| UT-TRV-005 | P | 57 elementos. | `pagina = 3`, `tamanho = 20`. | `itens` com 17 elementos, `pagina = 3`, `tamanho = 20`, `total = 57`. | RF017 | EP-011 | CT-TRV-07 |

#### 7.6.2. Tratamento de erros e registos — middleware

| ID | Tipo | Pré-condições | Dados de entrada | Resultado esperado | RF | EP | CT |
|---|---|---|---|---|---|---|---|
| UT-TRV-006 | P | `ExceptionHandlingMiddleware` com um `next` que lança a exceção indicada (parametrizado, 6 linhas). | `ArgumentException`, `UnauthorizedAccessException`, `ForbiddenException`, `KeyNotFoundException`, `InvalidOperationException`, `UnprocessableEntityException`. | 400, 401, 403, 404, 409 e 422, respetivamente, com `detail` igual à mensagem da exceção e o formato de erro único (PA-06). | — | Todos | CT-TRV-04 |
| UT-TRV-007 | N | Idem. | Exceção inesperada (`NullReferenceException` com a mensagem «segredo interno»). | 500 com mensagem genérica; o corpo **não** contém «segredo interno» nem o stack trace (ver PA-07: falha no código atual). | — | Todos | CT-TRV-08 |
| UT-TRV-008 | P | `RespostaValidacaoModelo` com erros nos campos `email` e `password`. | — | 400 com as mensagens dos dois campos no `detail`, em português. | RF002 | EP-001 | CT-TRV-04 |
| UT-TRV-009 | F | Erro na chave vazia (corpo em falta) ou em `$.email` (JSON mal formado). | — | 400 com a mensagem genérica «O corpo do pedido está em falta ou não é um JSON válido.»; nenhuma mensagem interna em inglês. | RF002 | EP-001 | CT-TRV-04 |
| UT-TRV-010 | N | `RequestLoggingMiddleware` com logger de teste; pedido com `Authorization: Bearer abc.def.ghi` e corpo com `password`. | — | Regista método, caminho, código e duração; **nenhuma** mensagem contém `abc.def.ghi` nem o corpo. | RF004 | Todos | CT-TRV-08 |

---

## 8. Cobertura das regras do contrato da API

Cada regra com resposta 400 ou 422 no contrato da API v02 tem pelo menos um caso positivo (o pedido que cumpre a regra é aceite) e um negativo (o pedido que a viola é recusado com o código previsto). Incluem-se também as regras de 403 por propriedade, 409 e 412, que não são simples verificações de perfil. As respostas 401 e 403 por perfil são verificadas pelas políticas (secção 7.1.4) e pela matriz endpoint × perfil dos testes de integração (CT-TRV-01).

| EP | Código | Regra | Casos positivos | Casos negativos |
|---|---|---|---|---|
| EP-001 | 400 | E-mail ou palavra-passe em falta | UT-F001-001, UT-F001-006 | UT-F001-003, UT-F001-004, UT-F001-005 |
| EP-004 | 400 | Perfil ou estado inválido | UT-F001-098 | UT-F001-099 |
| EP-006 | 400 | Campos em falta | UT-F001-073 | UT-F001-078, UT-F001-059, UT-F001-082 |
| EP-006 | 400 | E-mail inválido | UT-F001-073, UT-F001-067, UT-F001-071 | UT-F001-079, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-072 |
| EP-006 | 400 | Palavra-passe fraca | UT-F001-050, UT-F001-051, UT-F001-053 | UT-F001-080, UT-F001-052, UT-F001-054, UT-F001-055, UT-F001-056, UT-F001-057, UT-F001-058 |
| EP-006 | 409 | E-mail já registado | UT-F001-073 | UT-F001-077 |
| EP-006 | 422 | Perfil `Cliente` não permitido | UT-F001-073 | UT-F001-075 |
| EP-006 | 422 | `podeRegistarApoio = true` fora do perfil Auxiliar | UT-F001-073, UT-F001-074 | UT-F001-076 |
| EP-007 | 400 | Dados inválidos | UT-F001-083 | UT-F001-091 |
| EP-007 | 400 | Perfil `Cliente` | UT-F001-083 | UT-F001-084 |
| EP-007 | 409 | E-mail usado por outra conta | UT-F001-089 | UT-F001-088 |
| EP-007 | 422 | Administrador remove o próprio perfil de Administrador | UT-F001-086 | UT-F001-085 |
| EP-007 | 422 | `podeRegistarApoio = true` fora do perfil Auxiliar | UT-F001-083 | UT-F001-087 |
| EP-008 | 400 | Estado diferente de `Bloqueado` | UT-F001-092 | UT-F001-093 |
| EP-008 | 422 | Conta já bloqueada | UT-F001-092 | UT-F001-094 |
| EP-008 | 422 | Administrador bloqueia a própria conta | UT-F001-092 | UT-F001-095 |
| EP-008 | 422 | Conta de Cliente | UT-F001-092 | UT-F001-096 |
| EP-010 | 400 | Campos inválidos | UT-F001-102 | UT-F001-108, UT-F001-109, UT-F001-110, UT-F001-111 |
| EP-010 | 400 | Períodos sobrepostos no mesmo dia | UT-F001-106, UT-F001-107 | UT-F001-105 |
| EP-010 | 400 | Fecho igual ou anterior à abertura | UT-F001-102 | UT-F001-103, UT-F001-104 |
| EP-013 | 400 | Campos em falta | UT-F001-112 | UT-F001-116 |
| EP-013 | 400 | NIF inválido | UT-F001-112, UT-F001-113, UT-F001-060, UT-F001-061 | UT-F001-114, UT-F001-062, UT-F001-063, UT-F001-064, UT-F001-065, UT-F001-066 |
| EP-013 | 400 | E-mail inválido | UT-F001-112 | UT-F001-115 |
| EP-013 | 409 | NIF ou e-mail já registado | UT-F001-112 | UT-F001-117, UT-F001-118 |
| EP-014 | 400 | Dados inválidos | UT-F001-119 | UT-F001-122 |
| EP-014 | 409 | NIF ou e-mail de outro Cliente | UT-F001-121 | UT-F001-120 |
| EP-015 | 400 | Parâmetros inválidos | UT-F002-019, UT-TRV-001, UT-TRV-002 | UT-TRV-003, UT-TRV-004 |
| EP-017 | 400 | Campos em falta ou inválidos | UT-F002-001, UT-F002-002 | UT-F002-005, UT-F002-006, UT-F002-012, UT-F002-013 |
| EP-017 | 400 | `clienteId` em falta | UT-F002-001 | UT-F002-003 |
| EP-017 | 400 | Peso ≤ 0 | UT-F002-008 | UT-F002-007, UT-F002-009 |
| EP-017 | 400 | Data de nascimento futura | UT-F002-011 | UT-F002-010 |
| EP-018 | 400 | Dados inválidos | UT-F002-014 | UT-F002-015 |
| EP-020 | 403 | Animal de outro Cliente | UT-F002-022 | UT-F002-023 |
| EP-022 | 400 | Datas em falta, inválidas ou período > 31 dias | UT-F003-001, UT-F003-017, UT-F003-013 | UT-F003-016, UT-F003-015, UT-F003-014 |
| EP-023 | 400 | Intervalo de datas inválido | UT-F003-051 | UT-F003-052 |
| EP-024 | 403 | Consulta não atribuída ao Veterinário | UT-F003-055 | UT-F003-054 |
| EP-025 | 400 | Campos em falta | UT-F003-019 | UT-F003-022 |
| EP-025 | 400 | Data no passado | UT-F003-019 | UT-F003-020, UT-F003-021 |
| EP-025 | 422 | Fora do horário de funcionamento | UT-F003-019, UT-F003-026 | UT-F003-023, UT-F003-024, UT-F003-025 |
| EP-025 | 422 | Veterinário indisponível | UT-F003-029, UT-F003-030 | UT-F003-027, UT-F003-028 |
| EP-026 | 400 | Dados inválidos | UT-F003-033 | UT-F003-039 |
| EP-026 | 412 | `If-Match` desatualizado | UT-F003-042, UT-F003-043, UT-F003-086, UT-F003-089 | UT-F003-041, UT-F003-087, UT-F003-088, UT-F003-090 |
| EP-026 | 422 | Marcação `Realizada` ou `Cancelada` | UT-F003-033, UT-F003-034 | UT-F003-035, UT-F003-036 |
| EP-026 | 422 | Novo horário indisponível | UT-F003-038 | UT-F003-037 |
| EP-027 | 400 | Estado inexistente ou `Pendente` | UT-F003-062 | UT-F003-074, UT-F003-075 |
| EP-027 | 412 | `If-Match` desatualizado | UT-F003-062 | UT-F003-044 |
| EP-027 | 422 | Transição inválida | UT-F003-062, UT-F003-063, UT-F003-064, UT-F003-065 | UT-F003-066, UT-F003-067, UT-F003-068, UT-F003-069, UT-F003-070, UT-F003-071, UT-F003-072, UT-F003-073 |
| EP-028 | 412 | `If-Match` desatualizado | UT-F003-045 | UT-F003-049 |
| EP-028 | 422 | Marcação não `Confirmada` | UT-F003-045 | UT-F003-046, UT-F003-047 |
| EP-028 | 422 | Chegada já registada | UT-F003-045 | UT-F003-048 |
| EP-029 | 400 | Texto vazio ou com mais de 1000 caracteres | UT-F003-076, UT-F003-080 | UT-F003-078, UT-F003-079, UT-F003-081 |
| EP-029 | 403 | Auxiliar sem autorização de registo de apoio | UT-F003-076 | UT-F003-077 |
| EP-029 | 422 | Marcação `Cancelada` | UT-F003-076, UT-F003-083 | UT-F003-082 |
| EP-030 | 400 | Tipo inválido | UT-F003-056, UT-F003-058, UT-F003-059 | UT-F003-060 |
| EP-030 | 403 | `animalId` de outro Cliente | UT-F003-056 | UT-F003-061 |
| EP-031 | 400 | `consultaId` em falta | UT-F004-001 | UT-F004-005 |
| EP-031 | 400 | Nenhum elemento clínico preenchido | UT-F004-001, UT-F004-002, UT-F004-003 | UT-F004-006, UT-F004-007, UT-F004-012 |
| EP-031 | 400 | Prescrição incompleta | UT-F004-001, UT-F004-010 | UT-F004-008, UT-F004-009, UT-F004-011 |
| EP-031 | 409 | A consulta já tem registo clínico | UT-F004-001 | UT-F004-013 |
| EP-031 | 422 | Marcação `Pendente` ou `Cancelada` | UT-F004-001, UT-F004-004 | UT-F004-014, UT-F004-015 |
| EP-032 | 400 | Dados inválidos | UT-F004-018 | UT-F004-020, UT-F004-019 |
| EP-034 | 400 | Intervalo de datas inválido | UT-F004-023, UT-F004-025 | UT-F004-026 |
| EP-042 | 400 | Parâmetro `tipo` inválido | UT-F006-019 | UT-F006-020 |
| EP-043 | 400 | Tipo, produto, lote ou data em falta | UT-F006-008, UT-F006-009 | UT-F006-010, UT-F006-016 |
| EP-043 | 400 | Data de administração futura | UT-F006-012 | UT-F006-011 |
| EP-043 | 400 | `intervaloDias` ≤ 0 quando indicado | UT-F006-014, UT-F006-009 | UT-F006-013, UT-F006-015 |
| EP-044 | 403 | Animal de outro Cliente | UT-F006-022 | UT-F006-023 |

Total: 67 regras (38 de 400, 16 de 422 e 13 de 403, 409 e 412), todas com casos positivos e negativos.

---

## 9. Unidades não testáveis unitariamente

| Elemento | Justificação | Onde é verificado |
|---|---|---|
| Controladores e atributos `[Authorize]` de cada endpoint | São finos: encaminham para o serviço. O que interessa é que cada rota tenha a política certa, o que só se vê com pedidos HTTP reais. | Testes de integração, matriz endpoint × perfil (CT-TRV-01, CT-TRV-02). |
| Pipeline completa do `JwtBearer` (cabeçalho `Authorization` → 401 com corpo; `OnChallenge`, `OnForbidden`, `OnTokenValidated`) | Depende do *pipeline* do ASP.NET Core. As peças (emissão, parâmetros de validação, validação da sessão) têm casos unitários. | CT-F001-04, CT-F001-07 (I030). |
| `ClinicaDbContext`: mapeamentos, restrições `CHECK` e `UNIQUE`, `HasData` e migrações | Só o SQL Server aplica as restrições; os duplos (InMemory ou Moq) não as aplicam. | Script de reposição da I062 e testes de integração (CT-TRV-05). |
| Unicidade com pedidos simultâneos (dois pedidos com o mesmo e-mail ou NIF) | A verificação prévia do serviço é testada; a garantia final é o índice `UQ_*`. | Testes de integração. |
| Comparação de texto sem distinção de maiúsculas e ordenação | Dependem da *collation* `Latin1_General_100_CI_AS` da base de dados. | Testes de integração (PA-13). |
| `RowVersion` e resposta 409 por `DbUpdateConcurrencyException` | O SQL Server é que preenche a `RowVersion`; um fornecedor em memória não a atualiza (decisão D-10). A comparação do `If-Match` (412) tem casos unitários. | IT-CONC-01 a IT-CONC-05 (CT-F003-09). |
| Atomicidade e persistência após 201/200/204 | Exigem transações e uma leitura posterior real. | CT-TRV-05, CT-TRV-06. |
| CORS (`ETag` exposto e `If-Match` aceite) | Configuração do *pipeline*. | CT-F003-09. |
| `Program.cs`, `DependencyInjection.cs` e ligação da configuração | Código de arranque, excluído da medição de cobertura (Plano de Testes, secção 9). A validação da secção `Jwt` tem casos unitários (`JwtOptionsValidator`). | Arranque da API e testes de integração. |
| Compatibilidade dos hashes dos dados da I061 com o `PasswordHasher` | Exige os hashes reais e a base de dados. Nos casos unitários o hasher é um duplo. | Verificação manual da I063 e testes de integração do login. |

---

## 10. Rastreabilidade

### 10.1. Requisitos funcionais → casos

Os 41 requisitos funcionais com implementação no backend (RF001–RF036 e RF043–RF047) têm todos pelo menos um caso. RF037–RF042 e RF048–RF052 estão fora do âmbito.

| RF | N.º de casos | Casos |
|---|---|---|
| RF001 | 6 | UT-F001-001, UT-F001-002, UT-F001-006, UT-F001-011, UT-F001-023, UT-F001-027 |
| RF002 | 10 | UT-F001-003, UT-F001-004, UT-F001-005, UT-F001-007, UT-F001-008, UT-F001-009, UT-F001-010, UT-F001-012, UT-TRV-008, UT-TRV-009 |
| RF003 | 4 | UT-F001-016, UT-F001-021, UT-F001-022, UT-F001-026 |
| RF004 | 18 | UT-F001-013, UT-F001-017, UT-F001-018, UT-F001-025, UT-F001-028, UT-F001-029, UT-F001-030, UT-F001-031, UT-F001-032, UT-F001-033, UT-F001-034, UT-F001-035, UT-F001-036, UT-F001-037, UT-F001-038, UT-F001-039, UT-F001-049, UT-TRV-010 |
| RF005 | 12 | UT-F001-002, UT-F001-018, UT-F001-019, UT-F001-023, UT-F001-024, UT-F001-032, UT-F001-040, UT-F001-041, UT-F001-042, UT-F001-043, UT-F001-044, UT-F001-045 |
| RF006 | 25 | UT-F001-050, UT-F001-051, UT-F001-052, UT-F001-053, UT-F001-054, UT-F001-055, UT-F001-056, UT-F001-057, UT-F001-058, UT-F001-059, UT-F001-067, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-071, UT-F001-072, UT-F001-073, UT-F001-074, UT-F001-075, UT-F001-077, UT-F001-078, UT-F001-079, UT-F001-080, UT-F001-081, UT-F001-082 |
| RF007 | 8 | UT-F001-083, UT-F001-086, UT-F001-088, UT-F001-089, UT-F001-090, UT-F001-091, UT-F001-098, UT-F001-099 |
| RF008 | 8 | UT-F001-015, UT-F001-073, UT-F001-074, UT-F001-076, UT-F001-083, UT-F001-084, UT-F001-085, UT-F001-087 |
| RF009 | 11 | UT-F001-009, UT-F001-010, UT-F001-014, UT-F001-020, UT-F001-092, UT-F001-093, UT-F001-094, UT-F001-095, UT-F001-096, UT-F001-097, UT-F001-098 |
| RF010 | 19 | UT-F001-050, UT-F001-060, UT-F001-061, UT-F001-062, UT-F001-063, UT-F001-064, UT-F001-065, UT-F001-066, UT-F001-067, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-112, UT-F001-113, UT-F001-114, UT-F001-115, UT-F001-116, UT-F001-117, UT-F001-118 |
| RF011 | 7 | UT-F001-119, UT-F001-120, UT-F001-121, UT-F001-122, UT-F001-123, UT-F001-124, UT-F001-125 |
| RF012 | 1 | UT-F001-100 |
| RF013 | 4 | UT-F001-060, UT-F001-102, UT-F001-109, UT-F001-111 |
| RF014 | 13 | UT-F002-001, UT-F002-002, UT-F002-003, UT-F002-004, UT-F002-005, UT-F002-006, UT-F002-007, UT-F002-008, UT-F002-009, UT-F002-010, UT-F002-011, UT-F002-012, UT-F002-013 |
| RF015 | 7 | UT-F002-018, UT-F002-025, UT-F002-026, UT-F002-027, UT-F002-028, UT-F002-029, UT-F002-030 |
| RF016 | 4 | UT-F002-014, UT-F002-015, UT-F002-016, UT-F002-017 |
| RF017 | 8 | UT-F001-124, UT-F001-125, UT-F002-019, UT-TRV-001, UT-TRV-002, UT-TRV-003, UT-TRV-004, UT-TRV-005 |
| RF018 | 8 | UT-F001-046, UT-F001-047, UT-F001-048, UT-F002-020, UT-F002-021, UT-F002-022, UT-F002-023, UT-F002-024 |
| RF019 | 20 | UT-F001-100, UT-F001-101, UT-F001-102, UT-F001-103, UT-F001-104, UT-F001-105, UT-F001-106, UT-F001-107, UT-F001-108, UT-F001-110, UT-F003-001, UT-F003-004, UT-F003-006, UT-F003-007, UT-F003-008, UT-F003-011, UT-F003-023, UT-F003-024, UT-F003-025, UT-F003-026 |
| RF020 | 18 | UT-F003-001, UT-F003-002, UT-F003-003, UT-F003-004, UT-F003-005, UT-F003-006, UT-F003-008, UT-F003-009, UT-F003-010, UT-F003-011, UT-F003-012, UT-F003-013, UT-F003-014, UT-F003-015, UT-F003-016, UT-F003-017, UT-F003-018, UT-F003-027 |
| RF021 | 14 | UT-F003-019, UT-F003-020, UT-F003-021, UT-F003-022, UT-F003-023, UT-F003-024, UT-F003-025, UT-F003-026, UT-F003-027, UT-F003-028, UT-F003-029, UT-F003-030, UT-F003-031, UT-F003-032 |
| RF022 | 17 | UT-F003-033, UT-F003-034, UT-F003-035, UT-F003-036, UT-F003-037, UT-F003-038, UT-F003-039, UT-F003-040, UT-F003-041, UT-F003-042, UT-F003-043, UT-F003-085, UT-F003-086, UT-F003-087, UT-F003-088, UT-F003-089, UT-F003-090 |
| RF023 | 15 | UT-F003-044, UT-F003-062, UT-F003-063, UT-F003-064, UT-F003-065, UT-F003-066, UT-F003-067, UT-F003-068, UT-F003-069, UT-F003-070, UT-F003-071, UT-F003-072, UT-F003-073, UT-F003-074, UT-F003-075 |
| RF024 | 5 | UT-F003-050, UT-F003-051, UT-F003-052, UT-F003-054, UT-F003-055 |
| RF025 | 5 | UT-F003-045, UT-F003-046, UT-F003-047, UT-F003-048, UT-F003-049 |
| RF026 | 1 | UT-F003-053 |
| RF027 | 9 | UT-F003-076, UT-F003-077, UT-F003-078, UT-F003-079, UT-F003-080, UT-F003-081, UT-F003-082, UT-F003-083, UT-F003-084 |
| RF028 | 5 | UT-F003-056, UT-F003-057, UT-F003-059, UT-F003-060, UT-F003-061 |
| RF029 | 1 | UT-F003-058 |
| RF030 | 8 | UT-F004-001, UT-F004-004, UT-F004-005, UT-F004-013, UT-F004-014, UT-F004-015, UT-F004-016, UT-F004-017 |
| RF031 | 5 | UT-F004-001, UT-F004-006, UT-F004-007, UT-F004-012, UT-F004-020 |
| RF032 | 4 | UT-F004-001, UT-F004-003, UT-F004-011, UT-F004-018 |
| RF033 | 6 | UT-F004-001, UT-F004-002, UT-F004-008, UT-F004-009, UT-F004-010, UT-F004-018 |
| RF034 | 4 | UT-F004-018, UT-F004-019, UT-F004-020, UT-F004-021 |
| RF035 | 6 | UT-F004-022, UT-F004-023, UT-F004-024, UT-F004-025, UT-F004-026, UT-F004-027 |
| RF036 | 1 | UT-F004-022 |
| RF043 | 6 | UT-F006-008, UT-F006-010, UT-F006-011, UT-F006-012, UT-F006-016, UT-F006-017 |
| RF044 | 2 | UT-F006-009, UT-F006-010 |
| RF045 | 14 | UT-F006-001, UT-F006-002, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006, UT-F006-007, UT-F006-008, UT-F006-009, UT-F006-013, UT-F006-014, UT-F006-015, UT-F006-018, UT-F006-021 |
| RF046 | 4 | UT-F006-018, UT-F006-019, UT-F006-020, UT-F006-021 |
| RF047 | 3 | UT-F006-022, UT-F006-023, UT-F006-024 |

### 10.2. Endpoints → casos

| Endpoint | N.º de casos | Casos |
|---|---|---|
| EP-001 | 19 | UT-F001-001, UT-F001-002, UT-F001-003, UT-F001-004, UT-F001-005, UT-F001-006, UT-F001-007, UT-F001-008, UT-F001-009, UT-F001-010, UT-F001-011, UT-F001-012, UT-F001-023, UT-F001-024, UT-F001-025, UT-F001-026, UT-F001-027, UT-TRV-008, UT-TRV-009 |
| EP-002 | 3 | UT-F001-016, UT-F001-021, UT-F001-022 |
| EP-003 | 14 | UT-F001-013, UT-F001-014, UT-F001-015, UT-F001-017, UT-F001-018, UT-F001-019, UT-F001-020, UT-F001-028, UT-F001-029, UT-F001-030, UT-F001-031, UT-F001-032, UT-F001-033, UT-F001-034 |
| EP-004 | 4 | UT-F001-040, UT-F001-098, UT-F001-099, UT-TRV-001 |
| EP-005 | 1 | UT-F001-040 |
| EP-006 | 27 | UT-F001-040, UT-F001-050, UT-F001-051, UT-F001-052, UT-F001-053, UT-F001-054, UT-F001-055, UT-F001-056, UT-F001-057, UT-F001-058, UT-F001-059, UT-F001-067, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-071, UT-F001-072, UT-F001-073, UT-F001-074, UT-F001-075, UT-F001-076, UT-F001-077, UT-F001-078, UT-F001-079, UT-F001-080, UT-F001-081, UT-F001-082 |
| EP-007 | 10 | UT-F001-040, UT-F001-083, UT-F001-084, UT-F001-085, UT-F001-086, UT-F001-087, UT-F001-088, UT-F001-089, UT-F001-090, UT-F001-091 |
| EP-008 | 7 | UT-F001-040, UT-F001-092, UT-F001-093, UT-F001-094, UT-F001-095, UT-F001-096, UT-F001-097 |
| EP-009 | 3 | UT-F001-040, UT-F001-100, UT-F001-101 |
| EP-010 | 12 | UT-F001-040, UT-F001-060, UT-F001-102, UT-F001-103, UT-F001-104, UT-F001-105, UT-F001-106, UT-F001-107, UT-F001-108, UT-F001-109, UT-F001-110, UT-F001-111 |
| EP-011 | 3 | UT-F001-124, UT-TRV-001, UT-TRV-005 |
| EP-012 | 1 | UT-F001-125 |
| EP-013 | 19 | UT-F001-050, UT-F001-060, UT-F001-061, UT-F001-062, UT-F001-063, UT-F001-064, UT-F001-065, UT-F001-066, UT-F001-067, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-112, UT-F001-113, UT-F001-114, UT-F001-115, UT-F001-116, UT-F001-117, UT-F001-118 |
| EP-014 | 5 | UT-F001-119, UT-F001-120, UT-F001-121, UT-F001-122, UT-F001-123 |
| EP-015 | 5 | UT-F002-019, UT-TRV-001, UT-TRV-002, UT-TRV-003, UT-TRV-004 |
| EP-016 | 7 | UT-F002-018, UT-F002-025, UT-F002-026, UT-F002-027, UT-F002-028, UT-F002-029, UT-F002-030 |
| EP-017 | 13 | UT-F002-001, UT-F002-002, UT-F002-003, UT-F002-004, UT-F002-005, UT-F002-006, UT-F002-007, UT-F002-008, UT-F002-009, UT-F002-010, UT-F002-011, UT-F002-012, UT-F002-013 |
| EP-018 | 4 | UT-F002-014, UT-F002-015, UT-F002-016, UT-F002-017 |
| EP-019 | 6 | UT-F001-044, UT-F001-046, UT-F001-047, UT-F001-048, UT-F002-020, UT-F002-021 |
| EP-020 | 5 | UT-F001-044, UT-F001-046, UT-F002-022, UT-F002-023, UT-F002-024 |
| EP-021 | 1 | UT-F001-041 |
| EP-022 | 19 | UT-F001-041, UT-F003-001, UT-F003-002, UT-F003-003, UT-F003-004, UT-F003-005, UT-F003-006, UT-F003-007, UT-F003-008, UT-F003-009, UT-F003-010, UT-F003-011, UT-F003-012, UT-F003-013, UT-F003-014, UT-F003-015, UT-F003-016, UT-F003-017, UT-F003-018 |
| EP-023 | 4 | UT-F003-050, UT-F003-051, UT-F003-052, UT-TRV-001 |
| EP-024 | 4 | UT-F003-053, UT-F003-054, UT-F003-055, UT-F003-085 |
| EP-025 | 15 | UT-F001-041, UT-F003-019, UT-F003-020, UT-F003-021, UT-F003-022, UT-F003-023, UT-F003-024, UT-F003-025, UT-F003-026, UT-F003-027, UT-F003-028, UT-F003-029, UT-F003-030, UT-F003-031, UT-F003-032 |
| EP-026 | 18 | UT-F001-041, UT-F003-033, UT-F003-034, UT-F003-035, UT-F003-036, UT-F003-037, UT-F003-038, UT-F003-039, UT-F003-040, UT-F003-041, UT-F003-042, UT-F003-043, UT-F003-085, UT-F003-086, UT-F003-087, UT-F003-088, UT-F003-089, UT-F003-090 |
| EP-027 | 16 | UT-F001-041, UT-F003-044, UT-F003-062, UT-F003-063, UT-F003-064, UT-F003-065, UT-F003-066, UT-F003-067, UT-F003-068, UT-F003-069, UT-F003-070, UT-F003-071, UT-F003-072, UT-F003-073, UT-F003-074, UT-F003-075 |
| EP-028 | 6 | UT-F001-041, UT-F003-045, UT-F003-046, UT-F003-047, UT-F003-048, UT-F003-049 |
| EP-029 | 10 | UT-F001-043, UT-F003-076, UT-F003-077, UT-F003-078, UT-F003-079, UT-F003-080, UT-F003-081, UT-F003-082, UT-F003-083, UT-F003-084 |
| EP-030 | 8 | UT-F001-044, UT-F001-046, UT-F003-056, UT-F003-057, UT-F003-058, UT-F003-059, UT-F003-060, UT-F003-061 |
| EP-031 | 18 | UT-F001-042, UT-F004-001, UT-F004-002, UT-F004-003, UT-F004-004, UT-F004-005, UT-F004-006, UT-F004-007, UT-F004-008, UT-F004-009, UT-F004-010, UT-F004-011, UT-F004-012, UT-F004-013, UT-F004-014, UT-F004-015, UT-F004-016, UT-F004-017 |
| EP-032 | 5 | UT-F001-042, UT-F004-018, UT-F004-019, UT-F004-020, UT-F004-021 |
| EP-033 | 1 | UT-F004-022 |
| EP-034 | 5 | UT-F004-023, UT-F004-024, UT-F004-025, UT-F004-026, UT-F004-027 |
| EP-042 | 5 | UT-F001-042, UT-F006-018, UT-F006-019, UT-F006-020, UT-F006-021 |
| EP-043 | 18 | UT-F001-042, UT-F006-001, UT-F006-002, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006, UT-F006-007, UT-F006-008, UT-F006-009, UT-F006-010, UT-F006-011, UT-F006-012, UT-F006-013, UT-F006-014, UT-F006-015, UT-F006-016, UT-F006-017 |
| EP-044 | 5 | UT-F001-044, UT-F001-046, UT-F006-022, UT-F006-023, UT-F006-024 |

### 10.3. Condições de teste do Plano de Testes → casos

O Plano de Testes v02 (secção 10) marca com nível U as condições que têm componente unitária. Todas têm casos. As condições só de integração aparecem quando há uma parte da regra que se pode isolar.

| Condição | N.º de casos | Casos |
|---|---|---|
| CT-F001-01 | 11 | UT-F001-001, UT-F001-002, UT-F001-006, UT-F001-011, UT-F001-018, UT-F001-019, UT-F001-023, UT-F001-024, UT-F001-025, UT-F001-026, UT-F001-027 |
| CT-F001-02 | 7 | UT-F001-003, UT-F001-004, UT-F001-005, UT-F001-007, UT-F001-008, UT-F001-009, UT-F001-010 |
| CT-F001-03 | 3 | UT-F001-016, UT-F001-021, UT-F001-022 |
| CT-F001-04 | 15 | UT-F001-013, UT-F001-017, UT-F001-028, UT-F001-029, UT-F001-030, UT-F001-031, UT-F001-032, UT-F001-033, UT-F001-034, UT-F001-035, UT-F001-036, UT-F001-037, UT-F001-038, UT-F001-039, UT-F001-049 |
| CT-F001-05 | 33 | UT-F001-040, UT-F001-050, UT-F001-051, UT-F001-052, UT-F001-053, UT-F001-054, UT-F001-055, UT-F001-056, UT-F001-057, UT-F001-058, UT-F001-059, UT-F001-067, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-071, UT-F001-072, UT-F001-073, UT-F001-075, UT-F001-077, UT-F001-078, UT-F001-079, UT-F001-080, UT-F001-081, UT-F001-082, UT-F001-083, UT-F001-084, UT-F001-088, UT-F001-089, UT-F001-090, UT-F001-091, UT-F001-098, UT-F001-099 |
| CT-F001-06 | 3 | UT-F001-085, UT-F001-086, UT-F001-095 |
| CT-F001-07 | 8 | UT-F001-014, UT-F001-015, UT-F001-020, UT-F001-092, UT-F001-093, UT-F001-094, UT-F001-096, UT-F001-097 |
| CT-F001-08 | 12 | UT-F001-100, UT-F001-101, UT-F001-102, UT-F001-103, UT-F001-104, UT-F001-105, UT-F001-106, UT-F001-107, UT-F001-108, UT-F001-109, UT-F001-110, UT-F001-111 |
| CT-F001-09 | 21 | UT-F001-060, UT-F001-061, UT-F001-062, UT-F001-063, UT-F001-064, UT-F001-065, UT-F001-066, UT-F001-112, UT-F001-113, UT-F001-114, UT-F001-115, UT-F001-116, UT-F001-117, UT-F001-118, UT-F001-119, UT-F001-120, UT-F001-121, UT-F001-122, UT-F001-123, UT-F001-124, UT-F001-125 |
| CT-F001-11 | 3 | UT-F001-074, UT-F001-076, UT-F001-087 |
| CT-F002-01 | 23 | UT-F002-001, UT-F002-002, UT-F002-003, UT-F002-004, UT-F002-005, UT-F002-006, UT-F002-007, UT-F002-008, UT-F002-009, UT-F002-010, UT-F002-011, UT-F002-012, UT-F002-013, UT-F002-014, UT-F002-015, UT-F002-016, UT-F002-017, UT-F002-025, UT-F002-026, UT-F002-027, UT-F002-028, UT-F002-029, UT-F002-030 |
| CT-F002-02 | 1 | UT-F002-019 |
| CT-F002-03 | 1 | UT-F002-018 |
| CT-F002-05 | 5 | UT-F002-020, UT-F002-021, UT-F002-022, UT-F002-023, UT-F002-024 |
| CT-F003-01 | 14 | UT-F003-019, UT-F003-020, UT-F003-021, UT-F003-022, UT-F003-023, UT-F003-024, UT-F003-025, UT-F003-026, UT-F003-027, UT-F003-028, UT-F003-029, UT-F003-030, UT-F003-031, UT-F003-032 |
| CT-F003-02 | 18 | UT-F003-001, UT-F003-002, UT-F003-003, UT-F003-004, UT-F003-005, UT-F003-006, UT-F003-007, UT-F003-008, UT-F003-009, UT-F003-010, UT-F003-011, UT-F003-012, UT-F003-013, UT-F003-014, UT-F003-015, UT-F003-016, UT-F003-017, UT-F003-018 |
| CT-F003-03 | 14 | UT-F003-062, UT-F003-063, UT-F003-064, UT-F003-065, UT-F003-066, UT-F003-067, UT-F003-068, UT-F003-069, UT-F003-070, UT-F003-071, UT-F003-072, UT-F003-073, UT-F003-074, UT-F003-075 |
| CT-F003-04 | 8 | UT-F003-033, UT-F003-034, UT-F003-035, UT-F003-036, UT-F003-037, UT-F003-038, UT-F003-039, UT-F003-040 |
| CT-F003-05 | 4 | UT-F003-045, UT-F003-046, UT-F003-047, UT-F003-048 |
| CT-F003-06 | 5 | UT-F003-050, UT-F003-051, UT-F003-052, UT-F003-054, UT-F003-055 |
| CT-F003-07 | 11 | UT-F001-043, UT-F003-053, UT-F003-076, UT-F003-077, UT-F003-078, UT-F003-079, UT-F003-080, UT-F003-081, UT-F003-082, UT-F003-083, UT-F003-084 |
| CT-F003-08 | 6 | UT-F003-056, UT-F003-057, UT-F003-058, UT-F003-059, UT-F003-060, UT-F003-061 |
| CT-F003-09 | 11 | UT-F003-041, UT-F003-042, UT-F003-043, UT-F003-044, UT-F003-049, UT-F003-085, UT-F003-086, UT-F003-087, UT-F003-088, UT-F003-089, UT-F003-090 |
| CT-F004-01 | 2 | UT-F001-042, UT-F004-017 |
| CT-F004-02 | 15 | UT-F004-001, UT-F004-002, UT-F004-003, UT-F004-005, UT-F004-006, UT-F004-007, UT-F004-008, UT-F004-009, UT-F004-010, UT-F004-011, UT-F004-012, UT-F004-018, UT-F004-019, UT-F004-020, UT-F004-021 |
| CT-F004-03 | 5 | UT-F004-004, UT-F004-013, UT-F004-014, UT-F004-015, UT-F004-016 |
| CT-F004-04 | 1 | UT-F004-022 |
| CT-F004-05 | 5 | UT-F004-023, UT-F004-024, UT-F004-025, UT-F004-026, UT-F004-027 |
| CT-F006-01 | 8 | UT-F006-001, UT-F006-002, UT-F006-003, UT-F006-004, UT-F006-005, UT-F006-006, UT-F006-007, UT-F006-021 |
| CT-F006-02 | 10 | UT-F006-008, UT-F006-009, UT-F006-010, UT-F006-011, UT-F006-012, UT-F006-013, UT-F006-014, UT-F006-015, UT-F006-016, UT-F006-017 |
| CT-F006-03 | 3 | UT-F006-018, UT-F006-019, UT-F006-020 |
| CT-F006-04 | 3 | UT-F006-022, UT-F006-023, UT-F006-024 |
| CT-TRV-01 | 1 | UT-F001-041 |
| CT-TRV-02 | 3 | UT-F001-044, UT-F001-045, UT-F001-048 |
| CT-TRV-03 | 2 | UT-F001-046, UT-F001-047 |
| CT-TRV-04 | 3 | UT-TRV-006, UT-TRV-008, UT-TRV-009 |
| CT-TRV-07 | 5 | UT-TRV-001, UT-TRV-002, UT-TRV-003, UT-TRV-004, UT-TRV-005 |
| CT-TRV-08 | 3 | UT-F001-012, UT-TRV-007, UT-TRV-010 |

### 10.4. Distribuição por tarefa e por tipo

| Tarefa | Casos | Positivos | Negativos | Fronteira |
|---|---|---|---|---|
| I086 | 165 | 49 | 81 | 35 |
| I087 | 90 | 23 | 38 | 29 |
| I088 | 27 | 6 | 14 | 7 |
| I089 | 24 | 7 | 5 | 12 |
| **Total** | **306** | **85** | **138** | **83** |

---

## 11. Testes já implementados

A I063 (Pull Request em revisão) já inclui testes no projeto `ClinicaVeterinaria.UnitTests` que correspondem a casos desta especificação. Na I086 basta acrescentar-lhes o identificador no `DisplayName` e completar os casos em falta.

| Teste existente | Caso(s) |
|---|---|
| `JwtTokenServiceTests.TokenDeClienteTemAsClaimsDoContrato` | UT-F001-023 |
| `JwtTokenServiceTests.TokenDeVeterinarioTemVeterinarioIdESemClienteId` | UT-F001-024 |
| `JwtTokenServiceTests.TokenExpiraOitoHorasDepoisDaEmissao` | UT-F001-025 |
| `JwtTokenServiceTests.CadaTokenTemUmJtiDiferente` | UT-F001-026 |
| `JwtTokenServiceTests.TokenAtualEAceitePelaConfiguracaoDaApi` | UT-F001-028 |
| `JwtTokenServiceTests.TokenExpiradoERejeitado` | UT-F001-029 |
| `JwtTokenServiceTests.TokenAssinadoComOutraChaveERejeitado` | UT-F001-031 |
| `JwtTokenServiceTests.TokenAdulteradoERejeitado` | UT-F001-032 |
| `PoliticasAutorizacaoTests.PoliticaAceitaSoOsPerfisPrevistos (6 linhas)` | UT-F001-040, UT-F001-041, UT-F001-042, UT-F001-043, UT-F001-044, UT-F001-045 |
| `PoliticasAutorizacaoTests.PoliticaDePropriedadeAceitaClienteComClienteId` | UT-F001-046 |
| `PoliticasAutorizacaoTests.PoliticaDePropriedadeRejeitaClienteSemClienteId` | UT-F001-047 |
| `PoliticasAutorizacaoTests.PoliticaDePropriedadeRejeitaFuncionariosMesmoComClienteId` | UT-F001-048 |
| `PoliticasAutorizacaoTests.SemAutenticacaoAPoliticaPorOmissaoRecusaOAcesso` | UT-F001-049 |

O teste `TokenExpiradoERejeitado` usa um token emitido 9 horas antes; o caso UT-F001-029 pede 8 h 01 min e o UT-F001-030 acrescenta a fronteira das 7 h 59 min.

---

## 12. Pontos em aberto

| N.º | Ponto | Proposta | Casos afetados |
|---|---|---|---|
| PA-01 | **Âmbito da I088 e da I089.** As descrições ainda incluem contas e pagamentos (I088) e internamentos (I089), retirados pela Declaração de Âmbito v05. | Atualizar as descrições e os critérios 4 da I088 e 5 da I089 no Backlog Refinement. | — |
| PA-02 | **Palavra-passe fraca** (EP-006, EP-013) não está definida. | Aprovar a regra R-PWD (secção 4.4). | UT-F001-050, UT-F001-051, UT-F001-052, UT-F001-053, UT-F001-054, UT-F001-055, UT-F001-056, UT-F001-057, UT-F001-058, UT-F001-059 |
| PA-03 | **Validação do NIF** não está definida. Com a regra R-NIF, dois exemplos da API v02 ficam inválidos: `501234567` (EP-009) e `234567890` (EP-011 a EP-014). | Aprovar R-NIF e corrigir os exemplos numa próxima versão da API (por exemplo, `501234560` e `234567899`). | UT-F001-060, UT-F001-061, UT-F001-062, UT-F001-063, UT-F001-064, UT-F001-065, UT-F001-066 |
| PA-04 | **Validação do e-mail** não está definida. | Aprovar a regra R-EMAIL. | UT-F001-067, UT-F001-068, UT-F001-069, UT-F001-070, UT-F001-071, UT-F001-072 |
| PA-05 | **Fuso horário e horários já passados.** O contrato não diz em que hora estão `horariosLivres`, nem se a disponibilidade de hoje inclui horários que já começaram. | Aprovar R-FUSO e excluir da disponibilidade os horários que já começaram. | UT-F003-011, UT-F003-012 |
| PA-06 | **Formato de erro.** A API v02 (secção 5) prevê Problem Details (RFC 9457); a infraestrutura da I058 usa `{ code, message, detail }`. A I030 (CT-TRV-04) segue a API. | Decidir um formato único. Os casos unitários verificam o código e o `detail`, que existem nos dois formatos. | UT-TRV-006, UT-TRV-008, UT-TRV-009 |
| PA-07 | **Defeito de segurança na I058.** O `ExceptionHandlingMiddleware` devolve `exception.Message` também nos erros 500, o que pode expor detalhes internos ao cliente. | Registado como defeito numa Issue própria do GitHub (etiqueta `defeito`), que referencia este caso. Corrigir devolvendo uma mensagem genérica no 500. O caso fica a falhar até à correção e a Issue garante a rastreabilidade. | UT-TRV-007 |
| PA-08 | **Leitura do registo clínico pelo Auxiliar.** O critério 5 da I088 pede uma projeção sem campos excluídos, mas a API v02 (ponto em aberto 4) ainda dá ao Auxiliar o registo completo. | Manter o contrato atual. Se o grupo decidir uma projeção reduzida, acrescentar um caso que verifique a ausência desses campos. | UT-F004-022 |
| PA-09 | **Idade com data de nascimento desconhecida.** `IdadeAnos` é `int` nos DTOs, mas `DataNascimento` é opcional. | Devolver 0, ou passar `IdadeAnos` a `int?` no Modelo de Classes. | UT-F002-030 |
| PA-10 | **EP-032 com `id` de um elemento de outro registo.** O contrato não prevê o caso. | Responder 400. | UT-F004-019 |
| PA-11 | **Lista `proximas` do boletim.** Não está definido se inclui datas previstas já passadas. | Incluir só datas ≥ hoje, por ordem crescente. | UT-F006-021 |
| PA-12 | **Início das marcações.** Não está definido se `dataHora` (EP-025, EP-026) tem de coincidir com um horário devolvido por EP-022 (por exemplo, 10:15 quando a grelha é de 30 em 30 minutos). | Não exigir alinhamento (só a regra R-HORARIO). Se o grupo decidir exigi-lo, acrescentar casos. | UT-F003-028, UT-F003-038 |
| PA-13 | **E-mails com maiúsculas diferentes.** Na base de dados, a comparação ignora maiúsculas por causa da *collation*; nos duplos, depende da implementação (no InMemory, não ignora). | Comparar com normalização explícita no código, ou deixar este comportamento só para a integração. | — |
| PA-14 | **Duplos dos serviços com `ClinicaDbContext`.** O EF Core InMemory proposto na secção 4.2 não consta da D-08 do Plano de Testes v02, e o nível T1 prevê xUnit e Moq. Moq também ainda não está no projeto de testes. | Decidir no Backlog Refinement entre (a) acrescentar `Microsoft.EntityFrameworkCore.InMemory` à D-08 ou (b) usar interfaces de acesso a dados com duplos Moq. Os casos servem para as duas opções. | Casos dos serviços AUTH, UTIL, CLIN, CLI, ANI, CONS, NOTA, REG e VAC |
| PA-15 | **Logout repetido.** O contrato não diz o que acontece quando o mesmo `jti` é revogado duas vezes no serviço. | Operação idempotente (sem erro e sem linha repetida). | UT-F001-022 |
| PA-16 | **Hora injetada (D-07).** A D-07 do Plano de Testes v02 (`TimeProvider`) ainda está como proposta, e o `AuthService` da I063 usa `DateTime.UtcNow`. | Ratificar a D-07 no Backlog Refinement e adaptar o `AuthService` para receber `TimeProvider`. Até lá, os casos da secção 4.5 ficam condicionados a essa decisão. | Secção 4.5 |
| PA-17 | **Referências em rascunho.** A API v02 (#170), o Modelo de Classes v02 (#169), o Plano de Testes v02 (#172), a Especificação de Requisitos v02 (#179) e o Modelo de Dados v02 (#183) estão em PR por integrar. | Decisão do grupo: (a) integrar a I028 só depois dessas PRs, ou (b) aprová-la condicionada, como aconteceu com a I030. Em qualquer caso, se uma v02 mudar na revisão, rever os casos indicados na secção 2.1. | Secção 2.1 |

---

## 13. Verificação dos critérios de aceitação

| Critério da I028 | Como é cumprido |
|---|---|
| 1. Cada regra 400 ou 422 do contrato tem pelo menos um caso negativo e um positivo | Secção 8: 38 regras de 400 e 16 de 422, todas com casos positivos e negativos (mais 13 regras de 403, 409 e 412). |
| 2. As unidades críticas têm casos de fronteira | Disponibilidade: 12 casos de fronteira (marcações coladas, limite do fecho, dia fechado, dia com dois períodos, marcação cancelada, mudança da hora, 31/32 dias). Estados da marcação: as 16 combinações de origem e destino (secção 7.3.3), mais 10 fronteiras no `ConsultasService`. Próxima administração: 5 casos (mudança de mês e de ano, ano bissexto). Contas e pagamentos estão fora do âmbito (secção 3.2). |
| 3. Cada caso tem identificador, unidade, pré-condições, dados, resultado esperado e requisito/endpoint | Colunas das tabelas da secção 7; a unidade é a da subsecção onde o caso está. |
| 4. A matriz liga cada RF com implementação no backend a pelo menos um caso | Secção 10.1: os 41 RF em âmbito têm casos. |
| 5. Os casos bastam para fundamentar a implementação em I086 a I089 | Dados de referência (secção 5), regras propostas (secção 4.4), unidades e tarefas (secção 6) e casos com valores concretos. Os pontos ainda por decidir estão na secção 12, com a proposta usada nos casos. Fica condicionado à ratificação da D-07 (PA-16), à escolha dos duplos (PA-14) e à aprovação das referências v02 (PA-17). |


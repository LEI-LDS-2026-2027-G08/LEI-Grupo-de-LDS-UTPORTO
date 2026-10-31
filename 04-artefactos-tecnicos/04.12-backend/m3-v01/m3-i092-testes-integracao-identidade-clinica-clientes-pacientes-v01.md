# I092 — Testes de Integração de Identidade, Clínica, Clientes e Pacientes

| Campo     | Valor                                                                                         |
| --------- | --------------------------------------------------------------------------------------------- |
| Issue     | I092 — Implementar os testes de integração de identidade, clínica, Clientes e pacientes (#27) |
| Sprint    | S05                                                                                           |
| Milestone | M3                                                                                            |
| Executor  | Gonçalo Dinis Magalhães Teixeira                                                              |
| Revisor   | Ivan Cristiano Silva Mendes                                                                   |
| Auditor   | André Filipe Moreira Mesquita                                                                 |
| Data      | 2026-10-31                                                                                    |
| Estado    | Implementação do Executor concluída; em revisão                                               |

## 1. Objetivo

Implementar os testes de integração de EP-001 a EP-020 (autenticação e fim de sessão, contas de funcionário, dados e horário da clínica, Clientes, pacientes e área do Cliente), com o identificador de cada caso da Especificação de Testes de Integração (I030) no nome do teste, a correr contra a API real em memória e um SQL Server isolado (infraestrutura da I091).

## 2. Documentos de referência

| Documento                                               | Versão                                            |
| ------------------------------------------------------- | ------------------------------------------------- |
| Especificação de Testes de Integração do Backend (I030) | v01                                               |
| Documentação da API                                     | v02                                               |
| Especificação de Requisitos                             | v02                                               |
| Declaração de Âmbito                                    | v05                                               |
| Plano de Testes do Backend                              | v02                                               |
| Infraestrutura dos testes de integração (I091)          | `m3-i091-infraestrutura-testes-integracao-v01.md` |

## 3. Ficheiros

Todos em `src/backend/ClinicaVeterinaria.IntegrationTests`:

| Ficheiro                                            | Endpoints              | Origem                                                  |
| --------------------------------------------------- | ---------------------- | ------------------------------------------------------- |
| `Autenticacao/AutenticacaoEndpointsTests.cs`        | EP-001, EP-002, EP-003 | Novo (I092)                                             |
| `Infraestrutura/AutenticacaoInfraestruturaTests.cs` | EP-003 (IT-EP003-01)   | I091, sem alterações                                    |
| `Utilizadores/FuncionariosEndpointsTests.cs`        | EP-004, EP-005, EP-006 | Novo (I092)                                             |
| `Utilizadores/UtilizadoresEndpointsTests.cs`        | EP-007, EP-008         | I066, sem alterações                                    |
| `DadosClinica/ClinicaEndpointsTests.cs`             | EP-009, EP-010         | Novo (I092)                                             |
| `Clientes/ClientesEndpointsTests.cs`                | EP-011 a EP-014        | Novo (I092)                                             |
| `Animais/AnimaisEndpointsTests.cs`                  | EP-015 a EP-018        | Reescrito (I069 → I092)                                 |
| `Animais/MeusAnimaisEndpointsTests.cs`              | EP-019, EP-020         | I070; identificadores dos casos acrescentados aos nomes |

## 4. Decisões de implementação

1. **Palavras-passe.** As palavras-passe dos dados de demonstração não estão no repositório. Os testes de EP-001 e EP-002 definem uma palavra-passe de teste na conta que usam, através do `IPasswordHasher<Utilizador>` da aplicação; a base é reposta antes de cada teste (Respawn e dados da I061), por isso não há efeitos noutros testes.
2. **Token expirado (IT-EP003-04).** É emitido pelo próprio `JwtTokenService`, com a chave e a configuração da API, mas com um relógio 9 horas no passado; o token expirou há 1 hora.
3. **Clínica (EP-010).** As tabelas `Clinica` e `ConfiguracaoSistema` são dados de referência que o Respawn não repõe (I091). Cada teste que altera a clínica lê os dados no início e repõe-nos no fim (`try/finally`).
4. **Concorrência (IT-EP014-07).** Dois pedidos simultâneos tentam dar o mesmo e-mail novo a dois Clientes: um recebe 200, o outro 409, e o Cliente e a conta Utilizador do pedido rejeitado ficam inalterados.
5. **Paginação (IT-EP015-06).** São criados 105 pacientes diretamente na base; com `tamanho=100` a primeira página tem 100 elementos e a segunda o resto.
6. **401 e 403.** A I030 trata o 401 e o 403 de forma transversal; o critério 3 da I092 exige um teste por código de erro do contrato. Por isso cada ficheiro tem testes de 401 (sem token) e de 403 (perfis sem acesso, segundo a API v02), que verificam também que nada é alterado.
7. **Formato de erro.** Todos os erros são verificados no formato único da I058 (`{ code, message, detail }`), com `code` igual ao código HTTP.

## 5. Correspondência caso da especificação ↔ teste

Legenda dos ficheiros: **AUT** `AutenticacaoEndpointsTests`, **INF** `AutenticacaoInfraestruturaTests`, **FUN** `FuncionariosEndpointsTests`, **UTI** `UtilizadoresEndpointsTests`, **CLI** `ClinicaEndpointsTests`, **CLT** `ClientesEndpointsTests`, **ANI** `AnimaisEndpointsTests`, **MEU** `MeusAnimaisEndpointsTests`.

| Caso I030   | Ficheiro | Teste                                                            | Estado                        |
| ----------- | -------- | ---------------------------------------------------------------- | ----------------------------- |
| IT-EP001-01 | AUT      | `LoginValidoDevolveTokenComClaims`                               | Implementado                  |
| IT-EP001-02 | AUT      | `CamposEmFaltaDevolvem400` (4 casos)                             | Implementado — divergência D1 |
| IT-EP001-03 | AUT      | `CredenciaisIncorretasDevolvem401` (2 casos)                     | Implementado                  |
| IT-EP001-04 | AUT      | `ContaBloqueadaDevolve403`                                       | Implementado                  |
| IT-EP001-05 | AUT      | `LoginClienteIncluiClienteId`                                    | Implementado                  |
| IT-EP001-06 | AUT      | `LoginVeterinarioIncluiVeterinarioId`                            | Implementado                  |
| IT-EP002-01 | AUT      | `LogoutRevogaJti`                                                | Implementado                  |
| IT-EP002-02 | AUT      | `TokenRevogadoDevolve401`                                        | Implementado                  |
| IT-EP002-03 | AUT      | `LogoutSemTokenOuInvalidoDevolve401`                             | Implementado                  |
| IT-EP003-01 | INF      | `ClienteAutenticadoPorPerfilAcedeAAuthMe` (5 perfis)             | Implementado (I091)           |
| IT-EP003-02 | AUT      | `MeSemTokenDevolve401`                                           | Implementado                  |
| IT-EP003-03 | AUT      | `MeComTokenAdulteradoDevolve401`                                 | Implementado                  |
| IT-EP003-04 | AUT      | `MeComTokenExpiradoDevolve401`                                   | Implementado                  |
| IT-EP004-01 | FUN      | `ListarDevolveEnvelopePaginado`                                  | Implementado                  |
| IT-EP004-02 | FUN      | `FiltroPorPerfilEEstado`                                         | Implementado                  |
| IT-EP004-03 | FUN      | `FiltroInvalidoDevolve400` (2 casos)                             | Implementado                  |
| IT-EP005-01 | FUN      | `ObterFuncionarioExistente`                                      | Implementado                  |
| IT-EP005-02 | FUN      | `ObterInexistenteOuClienteDevolve404`                            | Implementado                  |
| IT-EP006-01 | FUN      | `CriarFuncionarioValido`                                         | Implementado                  |
| IT-EP006-02 | FUN      | `DadosInvalidosDevolvem400` (5 casos), `EmailInvalidoDevolve400` | Implementado                  |
| IT-EP006-03 | FUN      | `EmailRegistadoDevolve409`                                       | Implementado                  |
| IT-EP006-04 | FUN      | `PerfilClienteDevolve422`                                        | Implementado                  |
| IT-EP006-05 | FUN      | `ApoioForaDoAuxiliarDevolve422` (3 casos)                        | Implementado                  |
| IT-EP007-01 | UTI      | `AtualizaFuncionarioExistente`                                   | Implementado (I066)           |
| IT-EP007-02 | UTI      | `DadosInvalidosOuPerfilClienteDevolvem400`                       | Implementado (I066)           |
| IT-EP007-03 | UTI      | `ContaInexistenteOuClienteDevolve404`                            | Implementado (I066)           |
| IT-EP007-04 | UTI      | `EmailDeOutraContaDevolve409`                                    | Implementado (I066)           |
| IT-EP007-05 | UTI      | `AdministradorNaoRemoveOProprioPerfil`                           | Implementado (I066)           |
| IT-EP007-06 | UTI      | `ApoioEmPerfilIncompativelDevolve422`                            | Implementado (I066)           |
| IT-EP008-01 | UTI      | `BloqueiaFuncionarioAtivo`                                       | Implementado (I066)           |
| IT-EP008-02 | UTI      | `EstadoDiferenteDeBloqueadoDevolve400`                           | Implementado (I066)           |
| IT-EP008-03 | UTI      | `ContaInexistenteNoBloqueioDevolve404`                           | Implementado (I066)           |
| IT-EP008-04 | UTI      | `ContaJaBloqueadaDevolve422`                                     | Implementado (I066)           |
| IT-EP008-05 | UTI      | `AdministradorNaoSeBloqueia`                                     | Implementado (I066)           |
| IT-EP008-06 | UTI      | `ContaDeClienteDevolve422`                                       | Implementado (I066)           |
| IT-EP008-07 | UTI      | `TokenEmitidoAntesDoBloqueioERejeitado`                          | Implementado (I066)           |
| IT-EP009-01 | CLI      | `ObterCoincideComPersistencia`                                   | Implementado                  |
| IT-EP009-02 | CLI      | `ObterDevolveDoisPeriodosNoMesmoDia`                             | Implementado                  |
| IT-EP010-01 | CLI      | `AtualizarDadosValidos`                                          | Implementado                  |
| IT-EP010-02 | CLI      | `CamposInvalidosDevolvem400` (7 casos)                           | Implementado                  |
| IT-EP010-03 | CLI      | `FechoNaoPosteriorDevolve400` (2 casos)                          | Implementado                  |
| IT-EP010-04 | CLI      | `PeriodosSobrepostosDevolvem400`                                 | Implementado                  |
| IT-EP010-05 | CLI      | `DoisPeriodosNaoSobrepostos`                                     | Implementado                  |
| IT-EP011-01 | CLT      | `ListarDevolveEnvelopePaginado`                                  | Implementado                  |
| IT-EP011-02 | CLT      | `PesquisaPorNome`                                                | Implementado                  |
| IT-EP011-03 | CLT      | `PesquisaPorNifTelefoneOuEmail` (3 casos)                        | Implementado                  |
| IT-EP011-04 | CLT      | `PesquisaSemResultados`                                          | Implementado                  |
| IT-EP011-05 | CLT      | `PaginacaoConsistente`                                           | Implementado                  |
| IT-EP012-01 | CLT      | `ObterClienteComAnimais`                                         | Implementado                  |
| IT-EP012-02 | CLT      | `ObterInexistenteDevolve404`                                     | Implementado                  |
| IT-EP013-01 | CLT      | `CriarClienteValido`                                             | Implementado                  |
| IT-EP013-02 | CLT      | `CampoObrigatorioEmFaltaDevolve400` (4 casos)                    | Implementado                  |
| IT-EP013-03 | CLT      | `NifInvalidoDevolve400` (3 casos)                                | Implementado                  |
| IT-EP013-04 | CLT      | `EmailInvalidoDevolve400`                                        | Implementado                  |
| IT-EP013-05 | CLT      | `NifRepetidoDevolve409`                                          | Implementado                  |
| IT-EP013-06 | CLT      | `EmailRepetidoDevolve409` (2 casos)                              | Implementado                  |
| IT-EP014-01 | CLT      | `AtualizarClienteExistente`                                      | Implementado                  |
| IT-EP014-02 | CLT      | `AtualizarEmailAtualizaUtilizador`                               | Implementado                  |
| IT-EP014-03 | CLT      | `DadosInvalidosDevolvem400` (4 casos)                            | Implementado                  |
| IT-EP014-04 | CLT      | `AtualizarInexistenteDevolve404`                                 | Implementado                  |
| IT-EP014-05 | CLT      | `NifDeOutroClienteDevolve409`                                    | Implementado                  |
| IT-EP014-06 | CLT      | `EmailDeOutroClienteDevolve409`                                  | Implementado                  |
| IT-EP014-07 | CLT      | `ConflitoSimultaneoEAtomico`                                     | Implementado                  |
| IT-EP015-01 | ANI      | `ListarDevolveEnvelopePaginado`                                  | Implementado                  |
| IT-EP015-02 | ANI      | `FiltrarPorCliente`                                              | Implementado                  |
| IT-EP015-03 | ANI      | `PesquisarPorNome` (4 casos)                                     | Implementado                  |
| IT-EP015-04 | ANI      | `FiltrarPorEspecie`                                              | Implementado                  |
| IT-EP015-05 | ANI      | `ParametrosInvalidosDevolvem400` (5 casos)                       | Implementado                  |
| IT-EP015-06 | ANI      | `PaginacaoComTamanhoMaximo`                                      | Implementado                  |
| IT-EP016-01 | ANI      | `ObterFichaAdministrativa`                                       | Implementado                  |
| IT-EP016-02 | ANI      | `ObterInexistenteDevolve404`                                     | Implementado                  |
| IT-EP016-03 | ANI      | `AuxiliarSoTemLeitura`                                           | Implementado                  |
| IT-EP017-01 | ANI      | `RegistarAnimalValido`                                           | Implementado                  |
| IT-EP017-02 | ANI      | `CampoEmFaltaDevolve400` (3 casos)                               | Implementado — nota N1        |
| IT-EP017-03 | ANI      | `PesoNaoPositivoDevolve400` (2 casos)                            | Implementado                  |
| IT-EP017-04 | ANI      | `DataNascimentoFuturaDevolve400`                                 | Implementado                  |
| IT-EP017-05 | ANI      | `ClienteInexistenteDevolve404`                                   | Implementado                  |
| IT-EP017-06 | ANI      | `PedidosRejeitadosNaoCriamAnimais`                               | Implementado                  |
| IT-EP018-01 | ANI      | `AtualizarAnimalValido`                                          | Implementado                  |
| IT-EP018-02 | ANI      | `DadosInvalidosNaAtualizacaoDevolvem400` (2 casos)               | Implementado                  |
| IT-EP018-03 | ANI      | `AtualizarInexistenteDevolve404`                                 | Implementado                  |
| IT-EP018-04 | ANI      | `AtualizarParaClienteInexistenteMantemAssociacao`                | Implementado                  |
| IT-EP019-01 | MEU      | `ListaSoOsAnimaisDoClienteAutenticado`                           | Implementado (I070)           |
| IT-EP019-02 | MEU      | `ClienteSemAnimaisRecebeListaVazia`                              | Implementado (I070)           |
| IT-EP019-03 | MEU      | `OutroClienteVeApenasOSeuAnimal`, `ClienteIdNoPedidoEIgnorado`   | Implementado (I070)           |
| IT-EP020-01 | MEU      | `ConsultaAnimalProprio`                                          | Implementado (I070)           |
| IT-EP020-02 | MEU      | `AnimalDeOutroClienteDevolve403`                                 | Implementado (I070)           |
| IT-EP020-03 | MEU      | `AnimalInexistenteDevolve404`                                    | Implementado (I070)           |

**Resumo:** 88 casos da I030 para EP-001 a EP-020; 88 implementados; 0 não implementados.

### Testes adicionais de 401 e 403 (critério 3)

| Endpoints       | Ficheiro | Testes                                                                                                       |
| --------------- | -------- | ------------------------------------------------------------------------------------------------------------ |
| EP-004 a EP-006 | FUN      | `OutrosPerfisRecebem403` (4 perfis), `SemTokenDevolve401`                                                    |
| EP-007 e EP-008 | UTI      | `OutrosPerfisRecebem403` (4 perfis), `SemTokenDevolve401` (I066)                                             |
| EP-009 e EP-010 | CLI      | `OutrosPerfisRecebem403` (4 perfis), `SemTokenDevolve401`                                                    |
| EP-011 a EP-014 | CLT      | `ConsultaSemPermissaoDevolve403` (2 perfis), `GestaoSemPermissaoDevolve403` (3 perfis), `SemTokenDevolve401` |
| EP-015 a EP-018 | ANI      | `PerfisSemAcessoRecebem403` (2 perfis), `AuxiliarNaoPesquisa`, `SemTokenDevolve401`                          |
| EP-019 e EP-020 | MEU      | `PerfisInternosRecebem403`, `SemTokenDevolve401` (I070)                                                      |

### Critério 4 da I092

| Exigência                                  | Testes                                                                                                 |
| ------------------------------------------ | ------------------------------------------------------------------------------------------------------ |
| Revogação do token após o fim de sessão    | `LogoutRevogaJti`, `TokenRevogadoDevolve401` (AUT)                                                     |
| Conta bloqueada                            | `ContaBloqueadaDevolve403` (AUT); `TokenEmitidoAntesDoBloqueioERejeitado` (UTI)                        |
| Isolamento entre Clientes em `/me/animais` | `OutroClienteVeApenasOSeuAnimal`, `ClienteIdNoPedidoEIgnorado`, `AnimalDeOutroClienteDevolve403` (MEU) |

## 6. Divergências e notas

- **D1 — formato do 400 (IT-EP001-02).** A I030 e a Documentação da API v02 (secção 5) indicam Problem Details com o campo `errors`. A implementação devolve o formato único da I058 (`{ code, message, detail }`), também usado em 401 e 403. O teste segue a implementação. A divergência já tinha sido registada na revisão da I063 (melhoria M1, decisão do grupo pendente).
- **N1 — `sexo` em EP-017.** A API v02 não define o `sexo` como obrigatório; quando é omitido, o `CreateAnimalDto` assume `Desconhecido` e o pedido é aceite (201). Por isso o IT-EP017-02 cobre a falta de `nome`, `especie` e `clienteId`.

## 7. Execução

### 7.1. Local

```powershell
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
```

| Execução      | Total | Aprovados | Falhados | Ignorados | Duração |
| ------------- | ----- | --------- | -------- | --------- | ------- |
| 1.ª           | 295   | 295       | 0        | 0         | 164,1 s |
| 2.ª (seguida) | 295   | 295       | 0        | 0         | 166,4 s |

Execuções feitas a 2026-10-31 sobre o ramo `27-i092-testes-integracao-identidade-clinica-clientes-pacientes`, com a suite completa do projeto de testes de integração (os 129 testes novos da I092 e os 166 já existentes). Linha de base antes da I092: 177 testes, 0 falhas.

### 7.2. Pipeline

O job «3 · Testes de integração (QG-02)» da «Backend CI» (`backend-ci.yml`) executa estes testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`.

Execução: `<preencher com a ligação da execução da Pull Request no separador Actions>`

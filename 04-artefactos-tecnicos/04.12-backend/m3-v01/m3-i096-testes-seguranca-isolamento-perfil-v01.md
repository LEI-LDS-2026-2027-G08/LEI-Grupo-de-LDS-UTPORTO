# I096 — Testes de Segurança e de Isolamento por Perfil

| Campo     | Valor                                                                              |
| --------- | ---------------------------------------------------------------------------------- |
| Issue     | I096 — Implementar os testes de segurança e de isolamento por perfil (#98)         |
| Sprint    | S05                                                                                |
| Milestone | M3                                                                                 |
| Executor  | Ivan Cristiano Silva Mendes                                                        |
| Revisor   | Gonçalo Dinis Magalhães Teixeira                                                   |
| Auditor   | Gonçalo Magalhães Ribeiro                                                          |
| Data      | 2026-11-02                                                                         |
| Estado    | Implementação do Executor concluída; em revisão                                    |

## 1. Objetivo

Implementar a matriz automática de segurança da I030 (secção 14.1) para todas as operações ativas da API v02 — ausência de token, token expirado, adulterado e revogado, perfil sem permissão e perfil autorizado — e os casos de isolamento entre Clientes da secção 14.2, com dois Clientes, e registar a correspondência entre cada caso e o teste (evidência 3).

## 2. Documentos de referência

| Documento                                               | Versão                                            |
| ------------------------------------------------------- | ------------------------------------------------- |
| Backlog de Projeto                                      | v03 (I096, aprovada como Ready a 2026-10-27)      |
| Especificação de Testes de Integração do Backend (I030) | v01 (secções 14.1 e 14.2)                         |
| Documentação da API                                     | v02 (linha «Perfis» de EP-001 a EP-044)           |
| Plano de Testes do Backend                              | v02                                               |
| Infraestrutura dos testes de integração (I091)          | `m3-i091-infraestrutura-testes-integracao-v01.md` |

## 3. Ficheiros

Todos novos, em `src/backend/ClinicaVeterinaria.IntegrationTests/Seguranca`:

| Ficheiro                               | Sigla | Conteúdo                                                                                           |
| -------------------------------------- | ----- | -------------------------------------------------------------------------------------------------- |
| `MatrizPerfis.cs`                      | TAB   | Tabela de perfis autorizados por operação (38 operações), de onde a matriz é gerada (critério 3)   |
| `MatrizSegurancaEndpointsTests.cs`     | MAT   | Cobertura da tabela, matriz endpoint × perfil, tokens inválidos e IT-TRV-01 a 10                   |
| `IsolamentoClientesEndpointsTests.cs`  | ISO   | IT-TRV-11 a 16 com os dois Clientes, nos dois sentidos (critério 4)                                |

Não há alterações ao código de produção nem ao esquema da base de dados.

## 4. Decisões de implementação

1. **Tabela única, conferida com a API v02 (critério 3).** `MatrizPerfis` tem uma linha por operação: código (EP-xxx), método, rota e perfis autorizados. Os perfis de cada linha foram conferidos com a linha «Perfis» da Documentação da API v02 e com a matriz da I030 (secção 14.1): coincidem nos 37 endpoints. A tabela tem ainda a atualização do boletim (RF056, `PUT /animais/{id}/vacinacoes/{administracaoId}`, I082), que a API v02 não descreve (D2).
2. **A tabela não pode ficar desatualizada.** `TabelaCobreTodasAsRotasDaApi` lê as rotas registadas na API em execução (`IActionDescriptorCollectionProvider`) e exige que sejam exatamente as da tabela: um endpoint novo sem linha na matriz, ou uma linha sem endpoint, faz o teste falhar.
3. **Matriz gerada (IT-TRV-01 e IT-TRV-05).** `MatrizEndpointPerfil` corre uma vez por operação (38 casos) e, em cada uma, pede com os cinco perfis e sem token: o perfil autorizado tem de passar a autorização (nem 401, nem 403, nem erro 5xx); o não autorizado recebe 403; o pedido anónimo recebe 401, exceto EP-001, que é público. As rotas usam identificadores inexistentes (999999) e os pedidos de escrita levam um corpo vazio: o perfil autorizado termina na validação (400) ou no 404, sem alterar dados. Cada caso junta todas as diferenças numa só mensagem (por exemplo, `EP-016 GET /animais/999999: AuxiliarVeterinario: esperado 403, obtido 404`).
4. **Tokens inválidos (IT-TRV-02 a 04).** Cada tipo de token é enviado às 37 operações protegidas: expirado (emitido há 9 horas pelo serviço de JWT da API, com a chave certa), adulterado de duas formas (perfil mudado para Administrador com a assinatura original; assinatura trocada) e revogado pelo EP-002.
5. **Cenários nomeados (IT-TRV-06 a 10).** O Cliente recebe 403 nas 31 operações fora de `/auth/*` e `/me/*`; o Administrador no registo clínico; a Rececionista ao criar e alterar o registo clínico; o Auxiliar ao registar ou alterar paciente e registo clínico; o Veterinário ao criar, alterar, mudar o estado ou registar a chegada da marcação. Cada teste confirma também que a tabela não autoriza o perfil nessas operações.
6. **Dois Clientes, nos dois sentidos (critério 4).** Todos os testes de isolamento correm com o João (Bobi e Mia) e com a Maria (Kiko), cada um a tentar os dados do outro. Os animais e os Clientes são lidos da base pelo e-mail. IT-TRV-16 tenta, por Cliente, 3 URLs com o `clienteId` do outro na query e 4 URLs por identificador (cada animal do outro, `0`, `-1` e `int.MaxValue`): 19 pedidos do João e 23 da Maria e exige que nenhuma resposta 200 tenha animais que não sejam do Cliente e que nenhuma resposta contenha o nome de um animal do outro.
7. **Um token por pedido.** Cada pedido da matriz usa um token novo, emitido pelo serviço de JWT da API para a conta de demonstração do perfil (I061), porque o EP-002 revoga o token com que é chamado. As contas usadas são `admin`, `rececao`, `vet.ana`, `aux.carla` (com `podeRegistarApoio`, exigido pelo EP-029) e `cliente.joao`.
8. **Formato de erro.** Os 401 e 403 são verificados no formato único da I058 (`{ code, message, detail }`), com `code` igual ao código HTTP.

## 5. Correspondência caso da especificação ↔ teste

Legenda dos ficheiros: ver a coluna «Sigla» da secção 3.

### 5.1. Autenticação e autorização (I030, secção 14.1)

| Caso I030 | Cenário                                                       | Ficheiro | Teste                                                         | Estado              |
| --------- | ------------------------------------------------------------- | -------- | ------------------------------------------------------------- | ------------------- |
| IT-TRV-01 | Endpoint protegido sem JWT → 401                              | MAT      | `MatrizEndpointPerfil` (coluna «Anónimo», 38 casos)           | Implementado (I096) |
| IT-TRV-02 | JWT expirado → 401                                            | MAT      | `TokenExpiradoDevolve401` (37 operações)                      | Implementado (I096) |
| IT-TRV-03 | JWT adulterado → 401                                          | MAT      | `TokenAdulteradoDevolve401` (2 casos × 37 operações)          | Implementado (I096) |
| IT-TRV-04 | JWT revogado → 401                                            | MAT      | `TokenRevogadoDevolve401` (37 operações)                      | Implementado (I096) |
| IT-TRV-05 | Perfil autenticado não autorizado → 403                       | MAT      | `MatrizEndpointPerfil` (células F, 38 casos)                  | Implementado (I096) |
| IT-TRV-06 | Cliente fora de `/auth/*` e `/me/*` → 403                     | MAT      | `ClienteForaDaSuaAreaRecebe403` (31 operações)                | Implementado (I096) |
| IT-TRV-07 | Administrador no registo clínico → 403                        | MAT      | `AdministradorNoRegistoClinicoRecebe403`                      | Implementado (I096) |
| IT-TRV-08 | Rececionista cria/altera registo clínico → 403                | MAT      | `RececionistaNoRegistoClinicoRecebe403`                       | Implementado (I096) |
| IT-TRV-09 | Auxiliar altera paciente ou registo clínico → 403             | MAT      | `AuxiliarNaoAlteraPacienteNemRegisto`                         | Implementado (I096) |
| IT-TRV-10 | Veterinário altera marcação → 403                             | MAT      | `VeterinarioNaoAlteraMarcacao`                                | Implementado (I096) |
| Matriz    | Endpoint × perfil (secção 14.1), 37 endpoints + RF056         | MAT      | `MatrizEndpointPerfil` (38 casos), `TabelaCobreTodasAsRotasDaApi` | Implementado (I096) |

### 5.2. Isolamento entre Clientes (I030, secção 14.2)

| Caso I030 | Cenário                                          | Ficheiro | Teste                                                     | Estado              |
| --------- | ------------------------------------------------ | -------- | --------------------------------------------------------- | ------------------- |
| IT-TRV-11 | `/me/animais` só com os animais do Cliente       | ISO      | `MeusAnimaisSoDoCliente` (2 casos)                        | Implementado (I096) |
| IT-TRV-12 | Animal do outro Cliente → 403                    | ISO      | `AnimalDoOutroDevolve403` (2 casos)                       | Implementado (I096) |
| IT-TRV-13 | Marcações só dos animais do Cliente              | ISO      | `MinhasConsultasSoDosMeusAnimais` (2 casos)               | Implementado (I096) |
| IT-TRV-14 | Filtro de marcações por animal do outro → 403    | ISO      | `FiltroPorAnimalDoOutroDevolve403` (2 casos)              | Implementado (I096) |
| IT-TRV-15 | Boletim de animal do outro → 403                 | ISO      | `BoletimDoOutroDevolve403` (2 casos); também `AnimalDeOutroClienteDevolve403` (I082) | Implementado (I096; I082 num sentido) |
| IT-TRV-16 | Identificadores mudados à mão no URL             | ISO      | `IdentificadoresManipuladosNuncaDevolvemDadosDoOutro` (2 casos) | Implementado (I096) |

**Resumo:** 16 casos da I030 nas secções 14.1 e 14.2 e a matriz endpoint × perfil; 16 implementados; 0 não implementados.

### 5.3. Casos transversais fora do âmbito da I096

Os casos IT-TRV-17 a 34 (secções 14.3 a 14.6: formato de erro, persistência e atomicidade, paginação e ordenação, segurança de logs) não são testes de segurança por perfil nem de isolamento entre Clientes e não fazem parte da definição da I096 no Backlog de Projeto v03. IT-TRV-26 e IT-TRV-28 estão cobertos na I093; os restantes ficam registados para decisão no Backlog Refinement (N1).

### 5.4. Critérios de aceitação da I096

| Critério                                                                                     | Como é cumprido |
| -------------------------------------------------------------------------------------------- | --------------- |
| 1. Casos da I030 implementados, com o identificador no nome                                   | IT-TRV-01 a 16, todos com o identificador no `DisplayName` (secções 5.1 e 5.2). |
| 2. API real em memória, SQL Server isolado, independente da ordem                             | `WebApplicationFactory` com a API real e SQL Server em contentor (Testcontainers); base reposta antes de cada teste (Respawn e dados da I061); cada pedido usa o seu token (decisão 7). |
| 3. Matriz gerada a partir de uma tabela de perfis por endpoint conferida com a API            | `MatrizPerfis` conferida com a API v02 e com a I030 (decisão 1); matriz gerada da tabela (decisão 3); a tabela tem de coincidir com as rotas da API (decisão 2). |
| 4. `/me/*` testados com dois Clientes, sem dados do outro                                     | IT-TRV-11 a 16 com o João e a Maria, nos dois sentidos (decisão 6). |
| 5. Sem falhas, resultados num formato legível pela pipeline                                   | Execução local na secção 7.1. A Backend CI (job «3 · Testes de integração (QG-02)») corre os testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`. |

## 6. Divergências e notas

- **D1 — formato do erro.** Como na I092 a I095, a I030 indica Problem Details; a implementação devolve o formato único da I058.
- **D2 — RF056 fora da API v02.** A atualização do boletim (`PUT /animais/{id}/vacinacoes/{administracaoId}`, I082) existe na API mas não está descrita na Documentação da API v02 nem na matriz da I030. Entra na tabela com o perfil que o código aplica (Veterinário), para a matriz cobrir todas as rotas; a Documentação da API deve acrescentá-la numa próxima versão.
- **N1 — Casos IT-TRV-17 a 34.** Ver a secção 5.3.
- **N2 — Definição da tarefa.** Esta tarefa segue a definição da I096 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog v02.
- **N3 — Prazo.** A entrega é posterior ao limite do Executor do Sprint Backlog v02 (2026-11-01 17:30).

## 7. Execução

### 7.1. Local

```powershell
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
```

| Execução                                                 | Total | Aprovados | Falhados | Ignorados | Duração |
| -------------------------------------------------------- | ----- | --------- | -------- | --------- | ------- |
| Pasta `Seguranca` (`FullyQualifiedName~Seguranca`)       | 60    | 60        | 0        | 0         | 15 s    |
| Casos IT-TRV (`DisplayName~IT-TRV`)                      | 60    | 60        | 0        | 0         | 47 s    |
| Suite completa, 1.ª                                      | 453   | 453       | 0        | 0         | 245 s   |
| Suite completa, 2.ª (seguida)                            | 453   | 453       | 0        | 0         | 376 s   |

Execuções feitas a 2026-11-02 sobre o ramo da I096, criado a partir de `develop` (`cdffff8`), com o SQL Server 2025 em Docker (Testcontainers). Linha de base antes da I096: 393 testes de integração. O filtro `DisplayName~IT-TRV` encontra 59 testes da I096 (todos menos `TabelaCobreTodasAsRotasDaApi`) e o IT-TRV-15 da I082. A I096 acrescenta 60 execuções, todas na pasta `Seguranca`: 48 em `MatrizSegurancaEndpointsTests` (1 + 38 + 1 + 2 + 1 + 5) e 12 em `IsolamentoClientesEndpointsTests` (6 testes × 2 Clientes). Compilação da solução com `-warnaserror` sem avisos nem erros.

Falha controlada feita pelo Executor para confirmar que a matriz deteta erros (reposta a seguir): retirar o Auxiliar de EP-016 e autorizar o Cliente em EP-009 na tabela fez falhar exatamente esses dois casos, com `AuxiliarVeterinario: esperado 403, obtido 404` e `Cliente: esperado acesso, obtido 403`.

### 7.2. Pipeline

O job «3 · Testes de integração (QG-02)» da «Backend CI» (`backend-ci.yml`) executa estes testes com `--logger trx` e publica o artefacto `resultados-testes-integracao`. O resultado da execução sobre a Pull Request fica nas verificações da própria PR («Resultado da pipeline»); a ligação à execução é acrescentada aqui quando a PR for aberta.

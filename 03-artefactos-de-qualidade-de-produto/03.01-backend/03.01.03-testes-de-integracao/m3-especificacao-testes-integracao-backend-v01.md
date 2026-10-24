# Especificação dos Testes de Integração do Backend — Gestão de Clínica Veterinária

| Campo     | Valor                                                                                        |
| --------- | -------------------------------------------------------------------------------------------- |
| Grupo     | G08                                                                                          |
| Projeto   | Gestão de Clínica Veterinária                                                                |
| Artefacto | Especificação dos Testes de Integração do Backend                                            |
| Ficheiro  | `m3-especificacao-testes-integracao-backend-v01.md`                                          |
| Versão    | v01 — rascunho do Executor                                                                   |
| Milestone | m3                                                                                           |
| Issue     | I030 — Especificar os Testes de Integração do Backend                                        |
| Sprint    | S04                                                                                          |
| Executor  | Gonçalo Dinis Magalhães Teixeira                                                             |
| Revisor   | Gonçalo Magalhães Ribeiro                                                                    |
| Auditor   | André Filipe Moreira Mesquita                                                                |
| Pasta     | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao` |

## Histórico de versões

| Versão | Data       | Autor                            | Descrição                                                                                                                                                               |
| ------ | ---------- | -------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| v01    | 2026-10-24 | Gonçalo Dinis Magalhães Teixeira | Versão inicial da Especificação dos Testes de Integração do Backend, alinhada com a Declaração de Âmbito v05, Documentação da API v02 e Plano de Testes do Backend v02. |

---

## 1. Objetivo

Este documento especifica os testes de integração do backend do sistema de Gestão de Clínica Veterinária.

Os testes verificam o comportamento conjunto dos principais componentes do backend:

- API ASP.NET Core;
- camada de Aplicação;
- camada de Infraestrutura;
- Entity Framework Core;
- SQL Server;
- autenticação JWT;
- autorização por perfil;
- regras de propriedade e isolamento de dados;
- persistência;
- transações;
- controlo de concorrência.

Para cada endpoint são definidos casos de sucesso e os casos necessários para verificar os códigos de erro previstos no contrato da API.

Os códigos `401 Unauthorized` e `403 Forbidden` comuns aos endpoints protegidos são consolidados na matriz transversal de autorização, evitando a repetição do mesmo caso em todas as secções.

Os casos especificados neste documento constituem a base para a posterior implementação dos testes automáticos de integração do backend.

---

## 2. Documentos de referência

| Documento                          | Utilização                                                             |
| ---------------------------------- | ---------------------------------------------------------------------- |
| Declaração de Âmbito v05           | Define as funcionalidades incluídas e excluídas do projeto.            |
| Especificação de Requisitos v02    | Define os requisitos funcionais e não funcionais em vigor.             |
| Especificação dos Casos de Uso v02 | Define os fluxos principais e alternativos dos casos de uso em âmbito. |
| Documentação da API v02            | Define os 37 endpoints, métodos, rotas, perfis, DTOs e códigos HTTP.   |
| Documentação da Arquitetura v02    | Define a arquitetura e mecanismos técnicos do backend.                 |
| Modelo de Dados v02                | Define entidades, relações, restrições e `Marcacao.RowVersion`.        |
| Plano de Testes do Backend v02     | Define estratégia, níveis, infraestrutura e condições de teste.        |
| Regulamento de Grupo v02           | Define a baseline tecnológica e ferramentas aplicáveis.                |

---

## 3. Âmbito

### 3.1. Funcionalidades incluídas

A especificação cobre exclusivamente:

| Funcionalidade | Designação                           |
| -------------- | ------------------------------------ |
| F001           | Autenticação e Controlos de Acesso   |
| F002           | Gestão de pacientes                  |
| F003           | Agendamento de consultas             |
| F004           | Registo clínico e tratamentos        |
| F006           | Boletim de vacinas e desparasitações |

A Documentação da API v02 contém **37 endpoints ativos**.

Os identificadores dos endpoints não foram renumerados após a redução de âmbito.

### 3.2. Funcionalidades fora do âmbito

Não são especificados testes para:

- F005 — Gestão de contas e pagamentos;
- F007 — Gestão de internamento.

Consequentemente, ficam fora desta especificação:

- EP-035 a EP-041;
- EP-045 a EP-051;
- RF037 a RF042;
- RF048 a RF052;
- UC14;
- UC15;
- UC18.

Os antigos cenários de concorrência relativos a pagamentos e alojamentos deixam igualmente de fazer parte da I030.

---

## 4. Integrações verificadas

| Integração                  | Verificação                                                   |
| --------------------------- | ------------------------------------------------------------- |
| API ↔ Aplicação             | Encaminhamento dos pedidos e aplicação das regras funcionais. |
| Aplicação ↔ Persistência    | Leitura e escrita correta dos dados.                          |
| EF Core ↔ SQL Server        | Relações, restrições, persistência e concorrência reais.      |
| Autenticação ↔ API          | Validação de tokens JWT.                                      |
| Autorização ↔ Perfil        | Restrição dos endpoints aos perfis autorizados.               |
| Cliente ↔ Dados próprios    | Isolamento através das rotas `/me/...`.                       |
| Escrita ↔ Leitura posterior | Persistência dos dados após operações bem-sucedidas.          |
| Erro ↔ Persistência         | Pedidos rejeitados não produzem alterações parciais.          |
| Concorrência ↔ Marcação     | Validação através de `RowVersion`, `ETag` e `If-Match`.       |

---

## 5. Ambiente previsto

Os testes serão implementados utilizando:

- .NET 10;
- xUnit 2.6;
- `WebApplicationFactory`;
- Entity Framework Core 10;
- SQL Server;
- Testcontainers for .NET;
- Respawn.

Cada execução deverá usar uma base de dados isolada.

A infraestrutura técnica necessária será criada na I091.

Os testes não podem depender:

- de uma base partilhada;
- da ordem de execução;
- de dados pessoais reais;
- de tokens ou credenciais versionados;
- do estado produzido por outro teste.

---

## 6. Convenção dos identificadores

Os testes seguem:

```text
IT-EP<endpoint>-<número>
```

Exemplos:

```text
IT-EP001-01
IT-EP026-05
IT-EP044-03
```

Os testes transversais seguem:

```text
IT-TRV-<número>
```

e os testes específicos de concorrência:

```text
IT-CONC-<número>
```

---

## 7. Dados de teste

O conjunto reproduzível de dados deverá incluir, no mínimo:

| Identificador lógico   | Estado                                 |
| ---------------------- | -------------------------------------- |
| `ADMIN_A`              | Administrador ativo                    |
| `REC_A`                | Rececionista ativo                     |
| `VET_A`                | Veterinário ativo                      |
| `VET_B`                | Segundo Veterinário ativo              |
| `AUX_COM_APOIO`        | Auxiliar com `podeRegistarApoio=true`  |
| `AUX_SEM_APOIO`        | Auxiliar com `podeRegistarApoio=false` |
| `CLIENTE_A`            | Cliente ativo                          |
| `CLIENTE_B`            | Segundo Cliente ativo                  |
| `UTILIZADOR_BLOQUEADO` | Conta bloqueada                        |
| `ANIMAL_A1`            | Animal pertencente a `CLIENTE_A`       |
| `ANIMAL_A2`            | Segundo animal de `CLIENTE_A`          |
| `ANIMAL_B1`            | Animal pertencente a `CLIENTE_B`       |
| `CONSULTA_PENDENTE`    | Consulta em estado `Pendente`          |
| `CONSULTA_CONFIRMADA`  | Consulta em estado `Confirmada`        |
| `CONSULTA_REALIZADA`   | Consulta em estado `Realizada`         |
| `CONSULTA_CANCELADA`   | Consulta em estado `Cancelada`         |
| `REGISTO_CLINICO_A`    | Registo clínico já existente           |
| `ADMINISTRACAO_A`      | Vacinação ou desparasitação existente  |

Devem ainda existir:

- NIF e e-mail livres para testes de criação;
- NIF e e-mail já utilizados para testes de conflito;
- horários livres e ocupados;
- configuração conhecida do horário da clínica;
- consultas atribuídas a Veterinários diferentes.

Não são incluídas palavras-passe reais nesta especificação.

---

## 8. Estado inicial e reposição

Cada teste deve partir de um estado conhecido.

Antes da execução:

1. a base de testes é criada ou inicializada;
2. o esquema é aplicado;
3. os dados de referência são carregados;
4. o estado necessário ao caso é preparado;
5. após o teste, o estado é reposto quando necessário.

A infraestrutura da I091 e o mecanismo de reposição da I062 deverão garantir a reprodutibilidade.

---

# 9. F001 — Autenticação e Controlos de Acesso

## 9.1. EP-001 — Iniciar sessão

`POST /auth/login`
RF001, RF002, RF004, RF005

| ID          | Condições / Dados                   | Ação          | Resultado esperado                                                   |
| ----------- | ----------------------------------- | ------------- | -------------------------------------------------------------------- |
| IT-EP001-01 | Conta ativa com credenciais válidas | Efetuar login | `200`; JWT com `sub`, `email`, `role`, `jti`, `exp` e perfil correto |
| IT-EP001-02 | E-mail ou palavra-passe em falta    | Efetuar login | `400`; Problem Details com `errors`                                  |
| IT-EP001-03 | Palavra-passe incorreta             | Efetuar login | `401`; nenhum token                                                  |
| IT-EP001-04 | Conta bloqueada                     | Efetuar login | `403`; nenhum token                                                  |
| IT-EP001-05 | Conta Cliente válida                | Efetuar login | `200`; JWT inclui `clienteId`                                        |
| IT-EP001-06 | Conta Veterinário válida            | Efetuar login | `200`; JWT inclui `veterinarioId`                                    |

---

## 9.2. EP-002 — Terminar sessão

`POST /auth/logout`
RF003

| ID          | Condições / Dados          | Ação                              | Resultado esperado      |
| ----------- | -------------------------- | --------------------------------- | ----------------------- |
| IT-EP002-01 | JWT válido                 | Logout                            | `204`; `jti` é revogado |
| IT-EP002-02 | Token previamente revogado | Usar token num endpoint protegido | `401`                   |
| IT-EP002-03 | Token ausente ou inválido  | Logout                            | `401`                   |

---

## 9.3. EP-003 — Consultar utilizador autenticado

`GET /auth/me`
RF004, RF005

| ID          | Condições / Dados | Ação                 | Resultado esperado                               |
| ----------- | ----------------- | -------------------- | ------------------------------------------------ |
| IT-EP003-01 | JWT válido        | Consultar `/auth/me` | `200`; dados correspondem ao utilizador do token |
| IT-EP003-02 | Sem token         | Consultar            | `401`                                            |
| IT-EP003-03 | JWT adulterado    | Consultar            | `401`                                            |
| IT-EP003-04 | JWT expirado      | Consultar            | `401`                                            |

---

## 9.4. EP-004 — Listar contas de funcionários

`GET /utilizadores`
RF007, RF008, RF009

| ID          | Condições / Dados                 | Ação      | Resultado esperado                     |
| ----------- | --------------------------------- | --------- | -------------------------------------- |
| IT-EP004-01 | Administrador autenticado         | Listar    | `200`; envelope paginado               |
| IT-EP004-02 | `perfil=Veterinario&estado=Ativo` | Pesquisar | `200`; apenas registos correspondentes |
| IT-EP004-03 | Perfil ou estado inválido         | Pesquisar | `400`                                  |

`401` e `403` são cobertos transversalmente.

---

## 9.5. EP-005 — Consultar conta de funcionário

`GET /utilizadores/{id}`
RF007

| ID          | Condições / Dados     | Ação      | Resultado esperado             |
| ----------- | --------------------- | --------- | ------------------------------ |
| IT-EP005-01 | Funcionário existente | Consultar | `200`; `UtilizadorDto` correto |
| IT-EP005-02 | ID inexistente        | Consultar | `404`                          |

---

## 9.6. EP-006 — Criar conta de funcionário

`POST /utilizadores`
RF006, RF008

| ID          | Condições / Dados                                | Ação  | Resultado esperado                  |
| ----------- | ------------------------------------------------ | ----- | ----------------------------------- |
| IT-EP006-01 | Dados válidos e e-mail livre                     | Criar | `201`; `Location`; conta persistida |
| IT-EP006-02 | Campo obrigatório inválido ou password fraca     | Criar | `400`; nenhuma conta                |
| IT-EP006-03 | E-mail já registado                              | Criar | `409`; nenhuma duplicação           |
| IT-EP006-04 | `perfil=Cliente`                                 | Criar | `422`                               |
| IT-EP006-05 | Perfil não Auxiliar com `podeRegistarApoio=true` | Criar | `422`                               |

---

## 9.7. EP-007 — Atualizar conta de funcionário

`PUT /utilizadores/{id}`
RF007, RF008

| ID          | Condições / Dados                               | Ação      | Resultado esperado                 |
| ----------- | ----------------------------------------------- | --------- | ---------------------------------- |
| IT-EP007-01 | Funcionário existente, dados válidos            | Atualizar | `200`; alterações persistidas      |
| IT-EP007-02 | Dados inválidos ou perfil Cliente               | Atualizar | `400`; estado anterior mantido     |
| IT-EP007-03 | ID inexistente                                  | Atualizar | `404`                              |
| IT-EP007-04 | E-mail utilizado por outra conta                | Atualizar | `409`                              |
| IT-EP007-05 | Administrador altera o próprio perfil           | Atualizar | `422`; mantém perfil Administrador |
| IT-EP007-06 | `podeRegistarApoio=true` em perfil incompatível | Atualizar | `422`                              |

---

## 9.8. EP-008 — Bloquear conta

`PATCH /utilizadores/{id}/estado`
RF009

| ID          | Condições / Dados                            | Ação                        | Resultado esperado                        |
| ----------- | -------------------------------------------- | --------------------------- | ----------------------------------------- |
| IT-EP008-01 | Funcionário ativo                            | Bloquear                    | `204`; estado persistido como `Bloqueado` |
| IT-EP008-02 | Estado diferente de `Bloqueado`              | Alterar                     | `400`                                     |
| IT-EP008-03 | ID inexistente                               | Alterar                     | `404`                                     |
| IT-EP008-04 | Conta já bloqueada                           | Bloquear novamente          | `422`                                     |
| IT-EP008-05 | Administrador tenta bloquear a própria conta | Bloquear                    | `422`                                     |
| IT-EP008-06 | Conta alvo é Cliente                         | Bloquear                    | `422`                                     |
| IT-EP008-07 | Token emitido antes do bloqueio              | Bloquear e reutilizar token | Token passa a ser rejeitado               |

---

## 9.9. EP-009 — Consultar clínica

`GET /clinica`
RF012, RF019

| ID          | Condições / Dados          | Ação      | Resultado esperado                            |
| ----------- | -------------------------- | --------- | --------------------------------------------- |
| IT-EP009-01 | Configuração existente     | Consultar | `200`; `ClinicaDto` coincide com persistência |
| IT-EP009-02 | Dois períodos no mesmo dia | Consultar | `200`; ambos são devolvidos corretamente      |

---

## 9.10. EP-010 — Atualizar clínica

`PUT /clinica`
RF013, RF019

| ID          | Condições / Dados             | Ação              | Resultado esperado             |
| ----------- | ----------------------------- | ----------------- | ------------------------------ |
| IT-EP010-01 | Dados e horário válidos       | Atualizar e reler | `200`; dados persistidos       |
| IT-EP010-02 | Campos inválidos              | Atualizar         | `400`; estado anterior mantido |
| IT-EP010-03 | Fecho ≤ abertura              | Atualizar         | `400`                          |
| IT-EP010-04 | Períodos sobrepostos          | Atualizar         | `400`                          |
| IT-EP010-05 | Dois períodos não sobrepostos | Atualizar         | `200`; ambos persistidos       |

---

# 10. F002 — Gestão de pacientes

## 10.1. EP-011 — Pesquisar clientes

`GET /clientes`
RF011, RF017

| ID          | Condições / Dados       | Ação                     | Resultado esperado               |
| ----------- | ----------------------- | ------------------------ | -------------------------------- |
| IT-EP011-01 | Existem Clientes        | Listar                   | `200`; envelope paginado         |
| IT-EP011-02 | Pesquisa por nome       | Pesquisar                | `200`; correspondências corretas |
| IT-EP011-03 | NIF, telefone ou e-mail | Pesquisar                | `200`; Cliente correto           |
| IT-EP011-04 | Termo inexistente       | Pesquisar                | `200`; lista vazia               |
| IT-EP011-05 | Várias páginas          | Alterar `pagina/tamanho` | Paginação consistente            |

---

## 10.2. EP-012 — Consultar Cliente

`GET /clientes/{id}`
RF011, RF017

| ID          | Condições / Dados             | Ação      | Resultado esperado                  |
| ----------- | ----------------------------- | --------- | ----------------------------------- |
| IT-EP012-01 | Cliente existente com animais | Consultar | `200`; Cliente e animais associados |
| IT-EP012-02 | ID inexistente                | Consultar | `404`                               |

---

## 10.3. EP-013 — Criar Cliente

`POST /clientes`
RF010

| ID          | Condições / Dados          | Ação  | Resultado esperado                              |
| ----------- | -------------------------- | ----- | ----------------------------------------------- |
| IT-EP013-01 | NIF/e-mail livres          | Criar | `201`; Cliente + Utilizador Cliente persistidos |
| IT-EP013-02 | Campo obrigatório em falta | Criar | `400`; nada persistido                          |
| IT-EP013-03 | NIF inválido               | Criar | `400`                                           |
| IT-EP013-04 | E-mail inválido            | Criar | `400`                                           |
| IT-EP013-05 | NIF repetido               | Criar | `409`; sem duplicação                           |
| IT-EP013-06 | E-mail repetido            | Criar | `409`; sem duplicação                           |

---

## 10.4. EP-014 — Atualizar Cliente

`PUT /clientes/{id}`
RF011

| ID          | Condições / Dados         | Ação      | Resultado esperado                                            |
| ----------- | ------------------------- | --------- | ------------------------------------------------------------- |
| IT-EP014-01 | Cliente existente         | Atualizar | `200`; persistência confirmada                                |
| IT-EP014-02 | Novo e-mail válido        | Atualizar | Cliente e Utilizador usam o novo e-mail                       |
| IT-EP014-03 | Dados inválidos           | Atualizar | `400`; sem alterações                                         |
| IT-EP014-04 | Cliente inexistente       | Atualizar | `404`                                                         |
| IT-EP014-05 | NIF de outro Cliente      | Atualizar | `409`                                                         |
| IT-EP014-06 | E-mail de outro Cliente   | Atualizar | `409`                                                         |
| IT-EP014-07 | Conflito durante operação | Atualizar | Operação atómica; Cliente e Utilizador mantêm estado anterior |

---

## 10.5. EP-015 — Pesquisar pacientes

`GET /animais`
RF017

| ID          | Condições / Dados                        | Ação      | Resultado esperado                            |
| ----------- | ---------------------------------------- | --------- | --------------------------------------------- |
| IT-EP015-01 | Existem animais                          | Listar    | `200`; envelope paginado                      |
| IT-EP015-02 | `clienteId` válido                       | Filtrar   | Apenas animais desse Cliente                  |
| IT-EP015-03 | Pesquisa por nome de animal/proprietário | Pesquisar | Resultados corretos                           |
| IT-EP015-04 | Filtro por espécie                       | Pesquisar | Apenas espécie solicitada                     |
| IT-EP015-05 | Parâmetros inválidos                     | Pesquisar | `400`                                         |
| IT-EP015-06 | Paginação com `tamanho=100`              | Pesquisar | `200`; são devolvidos no máximo 100 elementos |

---

## 10.6. EP-016 — Consultar ficha do paciente

`GET /animais/{id}`
RF015, RF026

| ID          | Condições / Dados    | Ação      | Resultado esperado                  |
| ----------- | -------------------- | --------- | ----------------------------------- |
| IT-EP016-01 | Animal existente     | Consultar | `200`; apenas dados administrativos |
| IT-EP016-02 | ID inexistente       | Consultar | `404`                               |
| IT-EP016-03 | Auxiliar autenticado | Consultar | `200`; acesso apenas de leitura     |

---

## 10.7. EP-017 — Registar paciente

`POST /animais`
RF014

| ID          | Condições / Dados                         | Ação                     | Resultado esperado                   |
| ----------- | ----------------------------------------- | ------------------------ | ------------------------------------ |
| IT-EP017-01 | Dados válidos e Cliente existente         | Criar                    | `201`; `Location`; animal persistido |
| IT-EP017-02 | Campo obrigatório ou `clienteId` em falta | Criar                    | `400`                                |
| IT-EP017-03 | Peso ≤ 0                                  | Criar                    | `400`                                |
| IT-EP017-04 | Data de nascimento futura                 | Criar                    | `400`                                |
| IT-EP017-05 | Cliente inexistente                       | Criar                    | `404`                                |
| IT-EP017-06 | Pedido rejeitado                          | Consultar posteriormente | Nenhum animal parcialmente criado    |

---

## 10.8. EP-018 — Atualizar paciente

`PUT /animais/{id}`
RF016

| ID          | Condições / Dados               | Ação      | Resultado esperado                 |
| ----------- | ------------------------------- | --------- | ---------------------------------- |
| IT-EP018-01 | Animal existente, dados válidos | Atualizar | `200`; alterações persistidas      |
| IT-EP018-02 | Peso ≤ 0 ou data futura         | Atualizar | `400`; estado anterior             |
| IT-EP018-03 | Animal inexistente              | Atualizar | `404`                              |
| IT-EP018-04 | Novo Cliente inexistente        | Atualizar | `404`; associação anterior mantida |

---

## 10.9. EP-019 — Listar os meus animais

`GET /me/animais`
RF018

| ID          | Condições / Dados             | Ação                     | Resultado esperado                   |
| ----------- | ----------------------------- | ------------------------ | ------------------------------------ |
| IT-EP019-01 | CLIENTE_A possui dois animais | Consultar                | `200`; apenas A1 e A2                |
| IT-EP019-02 | Cliente sem animais           | Consultar                | `200`; `[]`                          |
| IT-EP019-03 | Existem animais de CLIENTE_B  | Consultar como CLIENTE_A | Nenhum dado de CLIENTE_B é devolvido |

---

## 10.10. EP-020 — Consultar o meu animal

`GET /me/animais/{id}`
RF018

| ID          | Condições / Dados                         | Ação                     | Resultado esperado |
| ----------- | ----------------------------------------- | ------------------------ | ------------------ |
| IT-EP020-01 | ANIMAL_A1 pertence ao Cliente autenticado | Consultar                | `200`              |
| IT-EP020-02 | ANIMAL_B1 pertence a outro Cliente        | Consultar como CLIENTE_A | `403`              |
| IT-EP020-03 | Animal inexistente                        | Consultar                | `404`              |

---

# 11. F003 — Agendamento de consultas

## 11.1. EP-021 — Listar Veterinários

`GET /veterinarios`
RF020, RF021

| ID          | Condições / Dados           | Ação   | Resultado esperado                              |
| ----------- | --------------------------- | ------ | ----------------------------------------------- |
| IT-EP021-01 | Existem Veterinários ativos | Listar | `200`; apenas Veterinários válidos para seleção |

---

## 11.2. EP-022 — Consultar disponibilidade

`GET /veterinarios/{id}/disponibilidade`
RF020, RF019

| ID          | Condições / Dados                     | Ação      | Resultado esperado                                      |
| ----------- | ------------------------------------- | --------- | ------------------------------------------------------- |
| IT-EP022-01 | Veterinário existente, período válido | Consultar | `200`; horários livres coerentes                        |
| IT-EP022-02 | `dataInicio` em falta/inválida        | Consultar | `400`                                                   |
| IT-EP022-03 | Período > 31 dias                     | Consultar | `400`                                                   |
| IT-EP022-04 | Veterinário inexistente               | Consultar | `404`                                                   |
| IT-EP022-05 | Existem horários ocupados             | Consultar | Horários ocupados não aparecem como livres              |
| IT-EP022-06 | Consulta cancelada no período         | Consultar | Horário da consulta cancelada pode voltar a estar livre |

---

## 11.3. EP-023 — Consultar agenda

`GET /consultas`
RF024, RF026

| ID          | Condições / Dados                       | Ação             | Resultado esperado                                 |
| ----------- | --------------------------------------- | ---------------- | -------------------------------------------------- |
| IT-EP023-01 | Rececionista autenticado                | Consultar agenda | `200`; consultas do período                        |
| IT-EP023-02 | Intervalo inválido                      | Consultar        | `400`                                              |
| IT-EP023-03 | Veterinário autenticado                 | Consultar        | Apenas consultas atribuídas ao próprio Veterinário |
| IT-EP023-04 | Veterinário envia outro `veterinarioId` | Consultar        | Parâmetro é ignorado; mantém isolamento            |
| IT-EP023-05 | Auxiliar autenticado                    | Consultar        | `200`; acesso de leitura conforme contrato         |

---

## 11.4. EP-024 — Consultar marcação

`GET /consultas/{id}`
RF024, RF026

| ID          | Condições / Dados                     | Ação                 | Resultado esperado                 |
| ----------- | ------------------------------------- | -------------------- | ---------------------------------- |
| IT-EP024-01 | Consulta existente                    | Consultar            | `200`; dados e notas de preparação |
| IT-EP024-02 | Veterinário atribuído                 | Consultar            | `200`                              |
| IT-EP024-03 | Consulta pertence a outro Veterinário | Consultar como VET_A | `403`                              |
| IT-EP024-04 | Consulta inexistente                  | Consultar            | `404`                              |
| IT-EP024-05 | Consulta válida                       | Consultar            | Resposta expõe `ETag` atual        |

---

## 11.5. EP-025 — Criar marcação

`POST /consultas`
RF021

| ID          | Condições / Dados                         | Ação                     | Resultado esperado                           |
| ----------- | ----------------------------------------- | ------------------------ | -------------------------------------------- |
| IT-EP025-01 | Animal, Veterinário e horário disponíveis | Criar                    | `201`; estado inicial `Pendente`; `Location` |
| IT-EP025-02 | Campo obrigatório em falta                | Criar                    | `400`                                        |
| IT-EP025-03 | Data no passado                           | Criar                    | `400`                                        |
| IT-EP025-04 | Animal inexistente                        | Criar                    | `404`                                        |
| IT-EP025-05 | Veterinário inexistente                   | Criar                    | `404`                                        |
| IT-EP025-06 | Fora do horário da clínica                | Criar                    | `422`                                        |
| IT-EP025-07 | Veterinário já ocupado                    | Criar                    | `422`; nenhuma segunda marcação              |
| IT-EP025-08 | Criação válida                            | Consultar posteriormente | Marcação persistida                          |

---

## 11.6. EP-026 — Alterar marcação

`PUT /consultas/{id}`
RF022

| ID          | Condições / Dados                        | Ação      | Resultado esperado                    |
| ----------- | ---------------------------------------- | --------- | ------------------------------------- |
| IT-EP026-01 | Pendente/Confirmada e novo horário livre | Atualizar | `200`; novo `ETag`; dados persistidos |
| IT-EP026-02 | Dados inválidos                          | Atualizar | `400`                                 |
| IT-EP026-03 | Consulta inexistente                     | Atualizar | `404`                                 |
| IT-EP026-04 | `If-Match` desatualizado                 | Atualizar | `409`; nenhuma alteração              |
| IT-EP026-05 | Consulta `Realizada`                     | Atualizar | `422`                                 |
| IT-EP026-06 | Consulta `Cancelada`                     | Atualizar | `422`                                 |
| IT-EP026-07 | Novo horário indisponível                | Atualizar | `422`                                 |
| IT-EP026-08 | `If-Match` atual                         | Atualizar | Sucesso e emissão de novo `ETag`      |

---

## 11.7. EP-027 — Atualizar estado

`PATCH /consultas/{id}/estado`
RF023

Transições válidas:

- `Pendente → Confirmada`;
- `Pendente → Cancelada`;
- `Confirmada → Realizada`;
- `Confirmada → Cancelada`.

| ID          | Condições / Dados                           | Ação                    | Resultado esperado |
| ----------- | ------------------------------------------- | ----------------------- | ------------------ |
| IT-EP027-01 | Pendente                                    | Alterar para Confirmada | `204`              |
| IT-EP027-02 | Pendente                                    | Alterar para Cancelada  | `204`              |
| IT-EP027-03 | Confirmada                                  | Alterar para Realizada  | `204`              |
| IT-EP027-04 | Confirmada                                  | Alterar para Cancelada  | `204`              |
| IT-EP027-05 | Estado solicitado inexistente ou `Pendente` | Alterar                 | `400`              |
| IT-EP027-06 | Consulta inexistente                        | Alterar                 | `404`              |
| IT-EP027-07 | Transição inválida                          | Alterar                 | `422`              |
| IT-EP027-08 | Estado final Realizada/Cancelada            | Tentar nova transição   | `422`              |
| IT-EP027-09 | `If-Match` antigo                           | Alterar                 | `409`              |
| IT-EP027-10 | `If-Match` atual                            | Alterar                 | `204`; novo `ETag` |

---

## 11.8. EP-028 — Registar chegada

`PATCH /consultas/{id}/chegada`
RF025

| ID          | Condições / Dados               | Ação     | Resultado esperado            |
| ----------- | ------------------------------- | -------- | ----------------------------- |
| IT-EP028-01 | Consulta Confirmada sem chegada | Registar | `200`; `chegadaEm` preenchido |
| IT-EP028-02 | Consulta inexistente            | Registar | `404`                         |
| IT-EP028-03 | `If-Match` desatualizado        | Registar | `409`                         |
| IT-EP028-04 | Consulta não Confirmada         | Registar | `422`                         |
| IT-EP028-05 | Chegada já registada            | Repetir  | `422`; hora original mantida  |

---

## 11.9. EP-029 — Notas de preparação

`POST /consultas/{id}/notas-preparacao`
RF027

| ID          | Condições / Dados                      | Ação       | Resultado esperado     |
| ----------- | -------------------------------------- | ---------- | ---------------------- |
| IT-EP029-01 | Auxiliar autorizado, texto válido      | Criar nota | `201`; nota persistida |
| IT-EP029-02 | Texto vazio                            | Criar      | `400`                  |
| IT-EP029-03 | Texto > 1000 caracteres                | Criar      | `400`                  |
| IT-EP029-04 | Auxiliar com `podeRegistarApoio=false` | Criar      | `403`                  |
| IT-EP029-05 | Consulta inexistente                   | Criar      | `404`                  |
| IT-EP029-06 | Consulta Cancelada                     | Criar      | `422`                  |

---

## 11.10. EP-030 — Consultar as minhas marcações

`GET /me/consultas`
RF028, RF029

| ID          | Condições / Dados             | Ação             | Resultado esperado                    |
| ----------- | ----------------------------- | ---------------- | ------------------------------------- |
| IT-EP030-01 | Cliente com consultas futuras | `tipo=futuras`   | `200`; apenas futuras próprias        |
| IT-EP030-02 | Cliente com histórico         | `tipo=historico` | `200`; apenas histórico próprio       |
| IT-EP030-03 | `tipo` inválido               | Consultar        | `400`                                 |
| IT-EP030-04 | `animalId` próprio            | Filtrar          | Apenas consultas desse animal         |
| IT-EP030-05 | `animalId` de outro Cliente   | Consultar        | `403`                                 |
| IT-EP030-06 | Existem registos clínicos     | Consultar        | Resposta não expõe informação clínica |

---

# 12. F004 — Registo clínico e tratamentos

## 12.1. EP-031 — Criar registo clínico

`POST /registos-clinicos`
RF030, RF031, RF032, RF033

| ID          | Condições / Dados                  | Ação                   | Resultado esperado                                        |
| ----------- | ---------------------------------- | ---------------------- | --------------------------------------------------------- |
| IT-EP031-01 | Consulta válida e conteúdo clínico | Criar                  | `201`; registo, tratamentos e prescrições persistidos     |
| IT-EP031-02 | `consultaId` em falta              | Criar                  | `400`                                                     |
| IT-EP031-03 | Nenhum elemento clínico preenchido | Criar                  | `400`                                                     |
| IT-EP031-04 | Prescrição incompleta              | Criar                  | `400`                                                     |
| IT-EP031-05 | Consulta inexistente               | Criar                  | `404`                                                     |
| IT-EP031-06 | Consulta já tem registo            | Criar                  | `409`                                                     |
| IT-EP031-07 | Consulta Pendente                  | Criar                  | `422`                                                     |
| IT-EP031-08 | Consulta Cancelada                 | Criar                  | `422`                                                     |
| IT-EP031-09 | Pedido rejeitado                   | Consultar persistência | Não existem tratamentos/prescrições parcialmente gravados |

---

## 12.2. EP-032 — Atualizar registo clínico

`PUT /registos-clinicos/{id}`
RF034, RF032, RF033

| ID          | Condições / Dados                           | Ação             | Resultado esperado                    |
| ----------- | ------------------------------------------- | ---------------- | ------------------------------------- |
| IT-EP032-01 | Registo existente                           | Atualizar        | `200`; alterações persistidas         |
| IT-EP032-02 | Dados clínicos inválidos                    | Atualizar        | `400`; estado anterior mantido        |
| IT-EP032-03 | Registo inexistente                         | Atualizar        | `404`                                 |
| IT-EP032-04 | Atualização válida                          | Consultar depois | `atualizadoEm` e autor registados     |
| IT-EP032-05 | Listas de tratamentos/prescrições alteradas | Atualizar        | Relações ficam coerentes com o pedido |

---

## 12.3. EP-033 — Consultar registo clínico

`GET /registos-clinicos/{id}`
RF035, RF036

| ID          | Condições / Dados | Ação                       | Resultado esperado     |
| ----------- | ----------------- | -------------------------- | ---------------------- |
| IT-EP033-01 | Registo existente | Consultar como Veterinário | `200`                  |
| IT-EP033-02 | Registo existente | Consultar como Auxiliar    | `200`; somente leitura |
| IT-EP033-03 | ID inexistente    | Consultar                  | `404`                  |

---

## 12.4. EP-034 — Histórico clínico

`GET /animais/{id}/historico-clinico`
RF035, RF036

| ID          | Condições / Dados          | Ação      | Resultado esperado                   |
| ----------- | -------------------------- | --------- | ------------------------------------ |
| IT-EP034-01 | Animal com vários registos | Consultar | `200`; ordem cronológica descendente |
| IT-EP034-02 | Intervalo válido           | Filtrar   | Apenas registos do intervalo         |
| IT-EP034-03 | Intervalo inválido         | Consultar | `400`                                |
| IT-EP034-04 | Animal inexistente         | Consultar | `404`                                |
| IT-EP034-05 | Animal sem histórico       | Consultar | `200`; coleção vazia                 |

---

# 13. F006 — Boletim de vacinas e desparasitações

## 13.1. EP-042 — Consultar boletim

`GET /animais/{id}/vacinacoes`
RF046, RF045

| ID          | Condições / Dados         | Ação      | Resultado esperado          |
| ----------- | ------------------------- | --------- | --------------------------- |
| IT-EP042-01 | Animal com administrações | Consultar | `200`; histórico e próximas |
| IT-EP042-02 | `tipo=Vacina`             | Filtrar   | Apenas vacinas              |
| IT-EP042-03 | `tipo=Desparasitacao`     | Filtrar   | Apenas desparasitações      |
| IT-EP042-04 | Tipo inválido             | Consultar | `400`                       |
| IT-EP042-05 | Animal inexistente        | Consultar | `404`                       |

---

## 13.2. EP-043 — Registar vacinação/desparasitação

`POST /animais/{id}/vacinacoes`
RF043, RF044, RF045

| ID          | Condições / Dados               | Ação  | Resultado esperado                                  |
| ----------- | ------------------------------- | ----- | --------------------------------------------------- |
| IT-EP043-01 | Dados válidos + intervalo       | Criar | `201`; administração persistida                     |
| IT-EP043-02 | Intervalo de 90 dias            | Criar | `dataProximaPrevista = dataAdministracao + 90 dias` |
| IT-EP043-03 | Sem intervalo                   | Criar | `dataProximaPrevista=null`                          |
| IT-EP043-04 | Tipo/produto/lote/data em falta | Criar | `400`                                               |
| IT-EP043-05 | Data futura                     | Criar | `400`                                               |
| IT-EP043-06 | `intervaloDias ≤ 0`             | Criar | `400`                                               |
| IT-EP043-07 | Animal inexistente              | Criar | `404`                                               |

---

## 13.3. EP-044 — Consultar boletim do meu animal

`GET /me/animais/{id}/vacinacoes`
RF047

| ID          | Condições / Dados           | Ação      | Resultado esperado          |
| ----------- | --------------------------- | --------- | --------------------------- |
| IT-EP044-01 | Animal próprio com registos | Consultar | `200`; histórico e próximas |
| IT-EP044-02 | Animal próprio sem registos | Consultar | `200`; listas vazias        |
| IT-EP044-03 | Animal de outro Cliente     | Consultar | `403`                       |
| IT-EP044-04 | Animal inexistente          | Consultar | `404`                       |

---

# 14. Testes transversais

## 14.1. Autenticação e autorização

| ID        | Cenário                                                                | Resultado esperado |
| --------- | ---------------------------------------------------------------------- | ------------------ |
| IT-TRV-01 | Endpoint protegido sem JWT                                             | `401 Unauthorized` |
| IT-TRV-02 | JWT expirado                                                           | `401 Unauthorized` |
| IT-TRV-03 | JWT adulterado                                                         | `401 Unauthorized` |
| IT-TRV-04 | JWT revogado                                                           | `401 Unauthorized` |
| IT-TRV-05 | Perfil autenticado não autorizado no endpoint                          | `403 Forbidden`    |
| IT-TRV-06 | Cliente tenta utilizar rota administrativa fora de `/auth/*` e `/me/*` | `403 Forbidden`    |
| IT-TRV-07 | Administrador tenta aceder a registo clínico                           | `403 Forbidden`    |
| IT-TRV-08 | Rececionista tenta criar/alterar registo clínico                       | `403 Forbidden`    |
| IT-TRV-09 | Auxiliar tenta alterar paciente ou registo clínico                     | `403 Forbidden`    |
| IT-TRV-10 | Veterinário tenta alterar marcação                                     | `403 Forbidden`    |

### Matriz endpoint × perfil

Legenda:

- **A** — permitido;
- **F** — `403 Forbidden`;
- **401** — pedido anónimo num endpoint protegido;
- **Público** — não requer JWT.

| EP     | Admin | Rececionista | Veterinário | Auxiliar | Cliente | Anónimo |
| ------ | ----- | ------------ | ----------- | -------- | ------- | ------- |
| EP-001 | A     | A            | A           | A        | A       | Público |
| EP-002 | A     | A            | A           | A        | A       | 401     |
| EP-003 | A     | A            | A           | A        | A       | 401     |
| EP-004 | A     | F            | F           | F        | F       | 401     |
| EP-005 | A     | F            | F           | F        | F       | 401     |
| EP-006 | A     | F            | F           | F        | F       | 401     |
| EP-007 | A     | F            | F           | F        | F       | 401     |
| EP-008 | A     | F            | F           | F        | F       | 401     |
| EP-009 | A     | F            | F           | F        | F       | 401     |
| EP-010 | A     | F            | F           | F        | F       | 401     |
| EP-011 | A     | A            | A           | F        | F       | 401     |
| EP-012 | A     | A            | A           | F        | F       | 401     |
| EP-013 | A     | A            | F           | F        | F       | 401     |
| EP-014 | A     | A            | F           | F        | F       | 401     |
| EP-015 | F     | A            | A           | F        | F       | 401     |
| EP-016 | F     | A            | A           | A        | F       | 401     |
| EP-017 | F     | A            | A           | F        | F       | 401     |
| EP-018 | F     | A            | A           | F        | F       | 401     |
| EP-019 | F     | F            | F           | F        | A       | 401     |
| EP-020 | F     | F            | F           | F        | A       | 401     |
| EP-021 | F     | A            | F           | F        | F       | 401     |
| EP-022 | F     | A            | F           | F        | F       | 401     |
| EP-023 | F     | A            | A           | A        | F       | 401     |
| EP-024 | F     | A            | A           | A        | F       | 401     |
| EP-025 | F     | A            | F           | F        | F       | 401     |
| EP-026 | F     | A            | F           | F        | F       | 401     |
| EP-027 | F     | A            | F           | F        | F       | 401     |
| EP-028 | F     | A            | F           | F        | F       | 401     |
| EP-029 | F     | F            | F           | A\*      | F       | 401     |
| EP-030 | F     | F            | F           | F        | A       | 401     |
| EP-031 | F     | F            | A           | F        | F       | 401     |
| EP-032 | F     | F            | A           | F        | F       | 401     |
| EP-033 | F     | F            | A           | A        | F       | 401     |
| EP-034 | F     | F            | A           | A        | F       | 401     |
| EP-042 | F     | F            | A           | F        | F       | 401     |
| EP-043 | F     | F            | A           | F        | F       | 401     |
| EP-044 | F     | F            | F           | F        | A       | 401     |

\* EP-029 exige adicionalmente `podeRegistarApoio=true`.

A Documentação da API define EP-001 como endpoint público. Assim, apesar da regra transversal do Plano de Testes referir genericamente pedidos anónimos, EP-001 constitui a exceção necessária para permitir o início de sessão.

---

## 14.2. Isolamento entre Clientes

| ID        | Cenário                                    | Resultado esperado                       |
| --------- | ------------------------------------------ | ---------------------------------------- |
| IT-TRV-11 | CLIENTE_A consulta `/me/animais`           | Apenas animais de CLIENTE_A              |
| IT-TRV-12 | CLIENTE_A consulta ANIMAL_B1               | `403`                                    |
| IT-TRV-13 | CLIENTE_A consulta marcações               | Apenas marcações dos seus animais        |
| IT-TRV-14 | CLIENTE_A filtra marcações por ANIMAL_B1   | `403`                                    |
| IT-TRV-15 | CLIENTE_A consulta boletim de ANIMAL_B1    | `403`                                    |
| IT-TRV-16 | Alteração manual de identificadores no URL | Nunca permite ler dados de outro Cliente |

O identificador do Cliente autenticado deve resultar do token e não de parâmetros fornecidos pelo cliente da API.

---

## 14.3. Problem Details

| ID        | Cenário    | Resultado esperado                                                    |
| --------- | ---------- | --------------------------------------------------------------------- |
| IT-TRV-17 | Erro `400` | `type`, `title`, `status`, `instance`, `traceId` e `errors` por campo |
| IT-TRV-18 | Erro `401` | Resposta coerente com política de autenticação                        |
| IT-TRV-19 | Erro `403` | Problem Details / resposta padronizada                                |
| IT-TRV-20 | Erro `404` | Estado 404 e formato de erro consistente                              |
| IT-TRV-21 | Erro `409` | Estado 409 e descrição do conflito                                    |
| IT-TRV-22 | Erro `422` | Estado 422 e descrição da regra de negócio                            |

---

## 14.4. Persistência e atomicidade

| ID        | Cenário                   | Resultado esperado                            |
| --------- | ------------------------- | --------------------------------------------- |
| IT-TRV-23 | Operação `201`            | Novo recurso existe numa leitura posterior    |
| IT-TRV-24 | Operação `200` de escrita | Alterações existem numa leitura posterior     |
| IT-TRV-25 | Operação `204` de escrita | Estado persistido é confirmado posteriormente |
| IT-TRV-26 | Pedido termina em `400`   | Nenhuma alteração parcial                     |
| IT-TRV-27 | Pedido termina em `409`   | Estado anterior preservado                    |
| IT-TRV-28 | Pedido termina em `422`   | Estado anterior preservado                    |

---

## 14.5. Paginação e ordenação

| ID        | Cenário                | Resultado esperado                                   |
| --------- | ---------------------- | ---------------------------------------------------- |
| IT-TRV-29 | Sem `pagina`/`tamanho` | São usados os valores por omissão                    |
| IT-TRV-30 | `tamanho` válido       | Número máximo de itens respeitado                    |
| IT-TRV-31 | `tamanho=100`          | A resposta respeita o limite máximo de 100 elementos |
| IT-TRV-32 | Múltiplas páginas      | `itens`, `pagina`, `tamanho` e `total` consistentes  |
| IT-TRV-33 | Ordenação suportada    | Ordem ascendente/descendente aplicada corretamente   |

---

## 14.6. Segurança de logs

### IT-TRV-34 — Segredos não aparecem nos logs

Executar pedidos de autenticação e pedidos autenticados e confirmar que:

- palavras-passe não aparecem nos logs;
- JWT completos não aparecem nos logs;
- chaves ou segredos de configuração não são escritos;
- os logs mantêm informação suficiente para diagnóstico sem divulgar credenciais.

---

# 15. Concorrência de marcações

Os cenários de concorrência são executados contra **SQL Server real**, uma vez que `RowVersion` depende do comportamento do SQL Server.

## IT-CONC-01 — Alteração com ETag atual

1. Ler a marcação através de EP-024.
2. Guardar o `ETag`.
3. Alterar através de EP-026 com `If-Match` igual ao valor lido.

**Resultado esperado:**

- `200 OK`;
- alteração persistida;
- resposta contém novo `ETag`.

## IT-CONC-02 — Alteração com ETag desatualizado

1. Dois clientes leem a mesma marcação.
2. Ambos recebem a mesma versão.
3. O primeiro altera a marcação com sucesso.
4. O segundo utiliza o `ETag` antigo.

**Resultado esperado:**

- segundo pedido devolve `409 Conflict`;
- a alteração do primeiro pedido permanece;
- o segundo pedido não sobrepõe dados.

## IT-CONC-03 — Concorrência na mudança de estado

Repetir o cenário anterior utilizando EP-027.

**Resultado esperado:** segundo pedido devolve `409`.

## IT-CONC-04 — Concorrência no registo de chegada

Repetir o cenário utilizando EP-028.

**Resultado esperado:** segundo pedido com versão antiga devolve `409`.

## IT-CONC-05 — Duas gravações simultâneas sem If-Match

Executar duas alterações concorrentes sobre a mesma `RowVersion`, sem `If-Match`.

**Resultado esperado:**

- apenas uma alteração é efetivamente persistida;
- a outra operação deteta `DbUpdateConcurrencyException`;
- a API devolve `409 Conflict`.

---

# 16. Rastreabilidade

| Endpoint | Casos principais                   | Requisitos                 |
| -------- | ---------------------------------- | -------------------------- |
| EP-001   | IT-EP001-01 a 06                   | RF001, RF002, RF004, RF005 |
| EP-002   | IT-EP002-01 a 03                   | RF003                      |
| EP-003   | IT-EP003-01 a 04                   | RF004, RF005               |
| EP-004   | IT-EP004-01 a 03                   | RF007, RF008, RF009        |
| EP-005   | IT-EP005-01 a 02                   | RF007                      |
| EP-006   | IT-EP006-01 a 05                   | RF006, RF008               |
| EP-007   | IT-EP007-01 a 06                   | RF007, RF008               |
| EP-008   | IT-EP008-01 a 07                   | RF009                      |
| EP-009   | IT-EP009-01 a 02                   | RF012, RF019               |
| EP-010   | IT-EP010-01 a 05                   | RF013, RF019               |
| EP-011   | IT-EP011-01 a 05                   | RF011, RF017               |
| EP-012   | IT-EP012-01 a 02                   | RF011, RF017               |
| EP-013   | IT-EP013-01 a 06                   | RF010                      |
| EP-014   | IT-EP014-01 a 07                   | RF011                      |
| EP-015   | IT-EP015-01 a 06                   | RF017                      |
| EP-016   | IT-EP016-01 a 03                   | RF015, RF026               |
| EP-017   | IT-EP017-01 a 06                   | RF014                      |
| EP-018   | IT-EP018-01 a 04                   | RF016                      |
| EP-019   | IT-EP019-01 a 03                   | RF018                      |
| EP-020   | IT-EP020-01 a 03                   | RF018                      |
| EP-021   | IT-EP021-01                        | RF020, RF021               |
| EP-022   | IT-EP022-01 a 06                   | RF020, RF019               |
| EP-023   | IT-EP023-01 a 05                   | RF024, RF026               |
| EP-024   | IT-EP024-01 a 05                   | RF024, RF026               |
| EP-025   | IT-EP025-01 a 08                   | RF021                      |
| EP-026   | IT-EP026-01 a 08, IT-CONC-01/02/05 | RF022                      |
| EP-027   | IT-EP027-01 a 10, IT-CONC-03       | RF023                      |
| EP-028   | IT-EP028-01 a 05, IT-CONC-04       | RF025                      |
| EP-029   | IT-EP029-01 a 06                   | RF027                      |
| EP-030   | IT-EP030-01 a 06                   | RF028, RF029               |
| EP-031   | IT-EP031-01 a 09                   | RF030, RF031, RF032, RF033 |
| EP-032   | IT-EP032-01 a 05                   | RF034, RF032, RF033        |
| EP-033   | IT-EP033-01 a 03                   | RF035, RF036               |
| EP-034   | IT-EP034-01 a 05                   | RF035, RF036               |
| EP-042   | IT-EP042-01 a 05                   | RF046, RF045               |
| EP-043   | IT-EP043-01 a 07                   | RF043, RF044, RF045        |
| EP-044   | IT-EP044-01 a 04                   | RF047                      |

Os testes `IT-TRV-*` complementam todos os endpoints protegidos relativamente a:

- RNF001 — autorização;
- RNF002 — isolamento dos Clientes;
- RNF003 — separação de canais;
- RNF004 — validação e formato de erros;
- RNF005 — persistência dos dados confirmados.

---

# 17. Cobertura das condições do Plano de Testes

| Condição   | Casos desta especificação                 |
| ---------- | ----------------------------------------- |
| CT-F001-01 | IT-EP001-01, 05, 06                       |
| CT-F001-02 | IT-EP001-02, 03, 04                       |
| CT-F001-03 | IT-EP002-01, 02                           |
| CT-F001-04 | IT-EP003-02 a 04, IT-TRV-01 a 04          |
| CT-F001-05 | IT-EP006-\*                               |
| CT-F001-06 | IT-EP007-05, IT-EP008-05                  |
| CT-F001-07 | IT-EP008-01, 07                           |
| CT-F001-08 | IT-EP010-03 a 05                          |
| CT-F001-09 | IT-EP013-_, IT-EP014-_                    |
| CT-F001-10 | IT-TRV-07                                 |
| CT-F001-11 | IT-EP006-05, IT-EP007-06                  |
| CT-F002-01 | IT-EP017-_, IT-EP018-_                    |
| CT-F002-02 | IT-EP015-\*                               |
| CT-F002-03 | IT-EP016-03 e matriz transversal          |
| CT-F002-04 | IT-TRV-08                                 |
| CT-F002-05 | IT-EP019-_, IT-EP020-_                    |
| CT-F003-01 | IT-EP025-\*                               |
| CT-F003-02 | IT-EP022-\*                               |
| CT-F003-03 | IT-EP027-\*                               |
| CT-F003-04 | IT-EP026-05 a 07                          |
| CT-F003-05 | IT-EP028-\*                               |
| CT-F003-06 | IT-EP023-03/04, IT-EP024-02/03, IT-TRV-10 |
| CT-F003-07 | IT-EP029-\*                               |
| CT-F003-08 | IT-EP030-\*                               |
| CT-F003-09 | IT-CONC-01 a 05                           |
| CT-F004-01 | matriz transversal, IT-TRV-08             |
| CT-F004-02 | IT-EP031-02 a 04                          |
| CT-F004-03 | IT-EP031-05 a 08                          |
| CT-F004-04 | IT-EP033-02 e matriz transversal          |
| CT-F004-05 | IT-EP034-\*                               |
| CT-F004-06 | matriz transversal                        |
| CT-F006-01 | IT-EP043-02/03                            |
| CT-F006-02 | IT-EP043-04 a 07                          |
| CT-F006-03 | matriz transversal                        |
| CT-F006-04 | IT-EP044-03                               |
| CT-TRV-01  | Matriz endpoint × perfil                  |
| CT-TRV-02  | IT-TRV-06                                 |
| CT-TRV-03  | IT-TRV-11 a 16                            |
| CT-TRV-04  | IT-TRV-17 a 22                            |
| CT-TRV-05  | IT-TRV-26 a 28                            |
| CT-TRV-06  | IT-TRV-23 a 25                            |
| CT-TRV-07  | IT-TRV-29 a 33                            |
| CT-TRV-08  | IT-TRV-34                                 |

---

# 18. Critérios de execução

Um caso é considerado aprovado quando:

1. o código HTTP obtido coincide com o esperado;
2. o corpo da resposta respeita o DTO/Problem Details previsto;
3. os dados devolvidos são os esperados;
4. a persistência foi alterada apenas quando a operação é bem-sucedida;
5. nenhum dado de outro Cliente é exposto;
6. as restrições de perfil são respeitadas;
7. as operações concorrentes não provocam perda silenciosa de dados.

Cada teste automático deve ser independente e reproduzível.

---

# 19. Evidências previstas

A execução dos testes deverá produzir:

- resultado do `dotnet test`;
- identificação dos testes aprovados e falhados;
- logs da execução;
- evidência da utilização de SQL Server isolado;
- relatório de testes de integração;
- rastreabilidade entre testes, endpoints e requisitos;
- Issues de defeito quando aplicável.

Os logs não devem conter palavras-passe, tokens JWT completos ou outros segredos.

---

# 20. Relação com tarefas posteriores

Esta especificação serve de base para:

- I091 — infraestrutura dos testes de integração;
- I092 — testes de identidade, clínica, Clientes e pacientes;
- I093 — testes da agenda e área do Cliente;
- I094 — testes de registo clínico;
- I096 — testes de segurança e isolamento por perfil;
- I095 — deverá ser refinada para cobrir apenas os testes de integração do boletim de vacinação e desparasitação (EP-042 a EP-044), uma vez que os internamentos (EP-045 a EP-051) pertencem à F007 e estão fora do âmbito pela Declaração de Âmbito v05;
- execução e relatório final dos testes de integração.

  > Nota: à data desta especificação, a descrição atual da I095 ainda inclui vacinação e internamentos. Essa descrição pertence à baseline anterior e deverá ser atualizada em Backlog Refinement, sem reintroduzir a F007 no âmbito.

  A antiga cobertura de F005 e F007 não é aplicável devido à Declaração de Âmbito v05.

---

# 21. Estado da I030

À data desta versão:

- os 37 endpoints ativos estão contemplados;
- F005 e F007 foram removidas da especificação;
- estão definidos casos de sucesso e de erro;
- os códigos `401` e `403` são cobertos pela matriz transversal;
- os cenários de isolamento entre Clientes estão definidos;
- os cenários de concorrência das marcações estão definidos;
- o conjunto mínimo de dados de teste está especificado;
- a reposição de estado está prevista;
- existe rastreabilidade entre casos, endpoints e requisitos;
- a especificação fornece base suficiente para implementação dos testes de integração das funcionalidades atualmente em âmbito.

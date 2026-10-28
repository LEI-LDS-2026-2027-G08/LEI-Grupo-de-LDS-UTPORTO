# Documentação da API — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Documentação da API (prevista) |
| Ficheiro | `m3-documentacao-api-v02.md` |
| Versão | v02 |
| Milestone de Entrega | m3 |
| Issue e papéis | Por atribuir. Esta versão ainda não tem Issue própria; será planeada no Backlog Refinement e no Sprint Planning, onde se atribuem Executor, Revisor e Auditor. A Issue I018 (v01, m2) está concluída e não é reaberta. |
| Estado | Rascunho para revisão e auditoria |
| Pasta de destino | `/projeto/04-artefactos-tecnicos/04.11-documentacao-de-api` |
| Fontes | `m3-declaracao-ambito-v05.pdf`; `m3-proposta-sistema-v05.pdf`; `m3-especificacao-requisitos-v02.md`; `m3-especificacoes-casos-uso-v02.md`; `m3-documentacao-arquitetura-v02.md`; `m3-modelo-classes-backend-v02.md`; `m3-modelo-de-dados-clinica-veterinaria-v02.md`; `m3-regulamento-grupo-v02.md`; `m2-documentacao-api-v01.md` (versão anterior) |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-10-09 | André Filipe Moreira Mesquita | Versão inicial da documentação prevista da API para a m2, alinhada com a Especificação de Requisitos (RF001–RF052, RNF001–RNF006) e com os Casos de Uso (UC01–UC18). Mantém-se inalterada como histórico (`m2-documentacao-api-v01.md`). |
| v02 | 2026-10-22 | Por atribuir | Reflete a Declaração de Âmbito v05: saem as funcionalidades F005 (contas e pagamentos, EP-035 a EP-041) e F007 (internamento, EP-045 a EP-051), ficando **37 endpoints**. Acrescenta a cada endpoint os DTOs de pedido e de resposta definidos em `m3-modelo-classes-backend-v02.md`, acrescenta a enumeração `DiaSemana`, retira as enumerações de pagamento e internamento, atualiza as tecnologias de referência (Regulamento de Grupo v02), refaz a rastreabilidade e documenta o controlo de alterações concorrentes nas marcações (412 quando o `If-Match` está desatualizado, 409 em gravações simultâneas). |

### Alterações da v02 em relação à v01

| Tema | Alteração |
|---|---|
| Âmbito | Retirados os endpoints EP-035 a EP-041 (F005) e EP-045 a EP-051 (F007). Os identificadores dos 37 endpoints restantes **não foram renumerados**, para manter a ligação à v01, ao Modelo de Classes v02 e aos restantes documentos. Os números retirados não voltam a ser usados. |
| DTOs | Cada endpoint indica o DTO de pedido e o DTO de resposta (secção 2.3). Os nomes são os do Modelo de Classes v02, secção 3. |
| Enumerações | Saem `EstadoPagamento`, `MetodoPagamento` e `EstadoInternamento`. Entra `DiaSemana` (já era usada nos exemplos de EP-009 e EP-010, mas não constava da tabela). |
| Convenções | Retirada a convenção de valores monetários, que só existia para a F005. |
| Permissões | Retirados da matriz os recursos «Contas e pagamentos», «Alojamentos» e «Internamentos» e as regras que lhes diziam respeito. |
| Tecnologias | ASP.NET Core 10 com Entity Framework Core 10, React e aplicação Android nativa (Kotlin e Jetpack Compose) em vez de Flutter, conforme o Regulamento de Grupo v02. |
| Concorrência | EP-026, EP-027 e EP-028 passam a poder responder **412** quando o `If-Match` enviado já não corresponde à versão da marcação, e **409** quando duas gravações da mesma marcação chegam em simultâneo. Mecanismo proposto na secção 5.1 (`ETag` e `If-Match`), assente na coluna `Marcacao.RowVersion` do Modelo de Dados v02 (decisão do Líder do grupo a 24/10/2026 sobre a coluna; o mecanismo na API é proposta a confirmar pelo Revisor). |
| Rastreabilidade | Secção 12 refeita para os 37 endpoints. Os requisitos RF037 a RF042 e RF048 a RF052 ficam sem endpoint por estarem fora do âmbito. |

---

## Índice

1. [Introdução](#1-introdução)
2. [Convenções gerais](#2-convenções-gerais)
3. [Autenticação e autorização](#3-autenticação-e-autorização)
4. [Matriz de permissões por perfil](#4-matriz-de-permissões-por-perfil)
5. [Formato de erros e códigos HTTP](#5-formato-de-erros-e-códigos-http)
6. [Endpoints — F001 Autenticação e Controlos de Acesso](#6-endpoints--f001-autenticação-e-controlos-de-acesso)
7. [Endpoints — F002 Gestão de pacientes](#7-endpoints--f002-gestão-de-pacientes)
8. [Endpoints — F003 Agendamento de consultas](#8-endpoints--f003-agendamento-de-consultas)
9. [Endpoints — F004 Registo clínico e tratamentos](#9-endpoints--f004-registo-clínico-e-tratamentos)
10. [Endpoints — F006 Boletim de vacinas e desparasitações](#10-endpoints--f006-boletim-de-vacinas-e-desparasitações)
11. [Elementos fora do âmbito da API](#11-elementos-fora-do-âmbito-da-api)
12. [Rastreabilidade](#12-rastreabilidade)
13. [Pontos em aberto](#13-pontos-em-aberto)

---

## 1. Introdução

Este documento descreve a API REST **prevista** para o backend do sistema de Gestão de Clínica Veterinária. A API será implementada em ASP.NET Core 10 com Entity Framework Core 10 (code-first) e autenticação JWT (Regulamento de Grupo v02, secção 11.2), e será consumida por dois clientes:

- **Frontend web** (React): utilizado pelos perfis Administrador, Rececionista, Veterinário e Auxiliar Veterinário.
- **Frontend mobile** (aplicação Android nativa, em Kotlin com Jetpack Compose): utilizado exclusivamente pelo perfil Cliente (Proprietário do animal).

A documentação cobre as funcionalidades **F001, F002, F003, F004 e F006** da Proposta de Sistema v05, que são as que estão em âmbito segundo a Declaração de Âmbito v05, num total de **37 endpoints**. As funcionalidades **F005 (Gestão de contas e pagamentos)** e **F007 (Gestão de internamento)** passaram para o não-âmbito (transição autorizada a 20/10/2026 e homologada por e-mail a 21/10/2026) e não têm endpoints (secção 11). Os requisitos funcionais associados (RF037 a RF042 e RF048 a RF052) estão descontinuados na Especificação de Requisitos v02 (a v01, da m2, ainda os trata como ativos) e não são implementados.

Este documento continua a descrever a API **prevista**. Qualquer ajuste decidido durante a implementação dá origem a uma nova versão do documento.

## 2. Convenções gerais

| Aspeto | Convenção |
|---|---|
| URL base | `https://<servidor>/api/v1`. Em desenvolvimento, a URL é configurável por cliente (por exemplo, `https://localhost:5001/api/v1` no Web e `http://10.0.2.2:<porta>/api/v1` no emulador Android). O uso de HTTP fica limitado ao ambiente local de desenvolvimento. |
| Formato | JSON (`Content-Type: application/json; charset=utf-8`) |
| Nomes de campos | camelCase, em português, sem acentos (ex.: `dataNascimento`) |
| Rotas | Substantivos no plural, minúsculas, separados por hífen (ex.: `/registos-clinicos`). Operações sobre um atributo específico usam um sub-recurso (ex.: `/consultas/{id}/estado`) |
| Identificadores | Inteiros positivos (`id`) |
| Datas | ISO 8601. Datas simples: `AAAA-MM-DD`. Data e hora: UTC, `AAAA-MM-DDThh:mm:ssZ` |
| Enumerações | Strings em PascalCase (ex.: `Pendente`, `Confirmada`) |
| Paginação | Parâmetros `pagina` (por omissão 1) e `tamanho` (por omissão 20, máximo 100) |
| Ordenação | Parâmetro `ordenar` (ex.: `ordenar=nome` ou `ordenar=-dataHora` para ordem descendente) |
| Rotas `/me/...` | Devolvem apenas dados do Cliente autenticado; o identificador do Cliente é obtido do token e nunca do pedido (RNF002) |
| Ligação à rede | A API pressupõe ligação ativa à rede; não existe modo offline (RNF006) |

### 2.1. Envelope de listas paginadas

```json
{
  "itens": [ ],
  "pagina": 1,
  "tamanho": 20,
  "total": 57
}
```

### 2.2. Enumerações utilizadas

| Enumeração | Valores |
|---|---|
| `Perfil` | `Administrador`, `Rececionista`, `Veterinario`, `AuxiliarVeterinario`, `Cliente` |
| `EstadoUtilizador` | `Ativo`, `Bloqueado` |
| `EstadoConsulta` | `Pendente`, `Confirmada`, `Realizada`, `Cancelada` |
| `TipoAdministracao` | `Vacina`, `Desparasitacao` |
| `Sexo` | `Macho`, `Femea`, `Desconhecido` |
| `DiaSemana` | `Segunda`, `Terca`, `Quarta`, `Quinta`, `Sexta`, `Sabado`, `Domingo` (horário de funcionamento da clínica, EP-009 e EP-010) |

### 2.3. DTOs de pedido e de resposta

Cada endpoint indica, nas duas últimas linhas da sua tabela, o **DTO de pedido** e o **DTO de resposta**. Um DTO (*Data Transfer Object*) é uma classe simples, apenas com atributos, que define os campos que viajam entre os frontends e a API. As entidades de domínio, que correspondem às tabelas da base de dados, nunca são devolvidas diretamente. Os campos de cada DTO estão no Modelo de Classes do Backend v02 (`m3-modelo-classes-backend-v02.md`, secção 3); os exemplos JSON deste documento correspondem a esses DTOs.

| Tipo de DTO | Padrão de nome | Exemplo |
|---|---|---|
| Pedido de criação (POST) | `Create<Coisa>Dto` | `CreateAnimalDto` |
| Pedido de alteração (PUT/PATCH) | `Update<Coisa>Dto` | `UpdateClienteDto` |
| Resposta de detalhe | `<Coisa>Dto` | `AnimalDto` |
| Item de uma lista | `<Coisa>ListItemDto` | `ClienteListItemDto` |
| Parâmetros no URL | `<Coisa>FiltroQuery` ou `<Coisa>Query` | `UtilizadorFiltroQuery` |
| Lista paginada | `PagedResultDto<T>` | `PagedResultDto<ClienteListItemDto>` |
| Referência curta | `<Coisa>RefDto` | `AnimalRefDto` |

## 3. Autenticação e autorização

### 3.1. Mecanismo

- Autenticação por **JSON Web Token (JWT)**, assinado com HMAC-SHA256.
- O token é obtido em `POST /auth/login` e enviado em todos os pedidos protegidos no cabeçalho:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

- Validade do token: **8 horas**. Expirado o token, o utilizador volta a autenticar-se (não é previsto refresh token) (RF004).
- `POST /auth/logout` invalida o token atual: o identificador do token (`jti`) é registado numa lista de tokens revogados até à sua expiração (RF003).

### 3.2. Claims do token

| Claim | Conteúdo |
|---|---|
| `sub` | Identificador do utilizador |
| `email` | E-mail do utilizador |
| `role` | Perfil (`Perfil`) |
| `veterinarioId` | Identificador do Veterinário (apenas no perfil Veterinário) |
| `clienteId` | Identificador do Cliente (apenas no perfil Cliente) |
| `jti` | Identificador único do token |
| `exp` | Data de expiração |

### 3.3. Regras de autorização

- A autorização é feita por perfil (atributo `[Authorize(Roles = ...)]`) e, quando necessário, por regras de propriedade (RF005, RNF001).
- Regras de propriedade aplicadas pelo servidor:
  - um **Cliente** só acede aos seus animais, consultas e boletins (RNF002);
  - um **Veterinário** só acede às consultas que lhe estão atribuídas na agenda (RF024).
- Uma conta com estado `Bloqueado` não consegue autenticar-se; tokens já emitidos para essa conta são rejeitados (RF009).
- O perfil **Cliente** só tem acesso às rotas `/auth/*` e `/me/*`, consumidas pela aplicação móvel (RNF003).
- O perfil **Administrador** não acede a registos clínicos nem a boletins de vacinação (Declaração de Âmbito, F001).
- O perfil **Auxiliar Veterinário** não tem acesso a operações de escrita clínica nem de marcações (RF027, RF036).

## 4. Matriz de permissões por perfil

Legenda: **L** — leitura; **E** — escrita (criar/alterar); **—** — sem acesso.

| Recurso | Administrador | Rececionista | Veterinário | Auxiliar Vet. | Cliente |
|---|---|---|---|---|---|
| Sessão (`/auth`) | L/E | L/E | L/E | L/E | L/E |
| Contas de funcionários | L/E | — | — | — | — |
| Dados gerais, configurações e horário da clínica | L/E | — | — | — | — |
| Clientes | L/E | L/E | L | — | — |
| Animais (dados administrativos) | — | L/E | L/E | L (ficha) | L (`/me`) |
| Veterinários e disponibilidade | — | L | — | — | — |
| Consultas (marcações) | — | L/E | L (próprias) | L | L (`/me`) |
| Notas de preparação (não clínicas) | — | — | L | L/E | — |
| Registos clínicos | — | — | L/E | L | — |
| Vacinações e desparasitações | — | — | L/E | — | L (`/me`) |

Os recursos «Contas e pagamentos», «Alojamentos» e «Internamentos», que constavam da v01, foram retirados com as funcionalidades F005 e F007, que passaram a fora do âmbito.

## 5. Formato de erros e códigos HTTP

Os erros seguem o formato **Problem Details** (RFC 9457), nativo do ASP.NET Core.

```json
{
  "type": "https://httpstatuses.io/422",
  "title": "Regra de negócio violada",
  "status": 422,
  "detail": "O veterinário já tem uma consulta marcada nesse horário.",
  "instance": "/api/v1/consultas",
  "traceId": "00-4bf92f3577b34da6a3ce929d0e0e4736-00"
}
```

Nos erros de validação (400) é acrescentado o campo `errors`, indicando os campos que violam as regras de validação (RNF004):

```json
{
  "type": "https://httpstatuses.io/400",
  "title": "Pedido inválido",
  "status": 400,
  "errors": {
    "email": ["O e-mail é obrigatório."],
    "peso": ["O peso deve ser superior a 0."]
  }
}
```

| Código | Significado na API |
|---|---|
| 200 OK | Pedido bem-sucedido com resposta |
| 201 Created | Recurso criado; o cabeçalho `Location` indica o URL do novo recurso |
| 204 No Content | Pedido bem-sucedido sem corpo de resposta |
| 400 Bad Request | Dados em falta, com formato inválido ou fora dos limites |
| 401 Unauthorized | Token ausente, inválido, expirado ou revogado; credenciais incorretas |
| 403 Forbidden | Perfil sem permissão, conta bloqueada ou recurso que não pertence ao utilizador |
| 404 Not Found | Recurso inexistente |
| 409 Conflict | Conflito com dados existentes (ex.: e-mail já registado) ou duas gravações simultâneas da mesma marcação (secção 5.1) |
| 412 Precondition Failed | O `If-Match` enviado já não corresponde à versão atual da marcação (secção 5.1) |
| 422 Unprocessable Entity | Regra de negócio violada (ex.: transição de estado inválida) |
| 500 Internal Server Error | Erro inesperado no servidor |

Nas tabelas de endpoints seguintes, **401** aplica-se a todos os endpoints protegidos e **403** a todos os endpoints com restrição de perfil, mesmo quando não repetidos.

### 5.1. Alterações concorrentes de marcações

Quando dois utilizadores (por exemplo, dois Rececionistas) abrem a mesma marcação e ambos a gravam, a segunda gravação não pode apagar em silêncio a primeira. Por isso a marcação tem uma versão, guardada na coluna `Marcacao.RowVersion` (Modelo de Dados v02), que o SQL Server muda em cada alteração.

- A API expõe a versão como validador `ETag` (o valor da `RowVersion` em Base64, entre aspas). Os endpoints EP-024, EP-025, EP-026, EP-027 e EP-028 devolvem o cabeçalho `ETag` com a versão atual da marcação.
- EP-026, EP-027 e EP-028 aceitam o cabeçalho `If-Match` com o último `ETag` que o cliente conhece. Se for enviado e já não corresponder à versão atual, a resposta é **412 Precondition Failed** (Problem Details, como prevê o RFC 9110 para um pré-requisito que falhou) e nada é alterado. O cliente deve voltar a ler a marcação (EP-024) e repetir a operação se ainda fizer sentido.
- O cabeçalho `If-Match` é opcional, para não alterar o contrato da v01. Sem ele, o servidor rejeita com **409 Conflict** duas gravações simultâneas da mesma marcação (`DbUpdateConcurrencyException` do Entity Framework Core), mas não deteta uma alteração feita antes do pedido. O frontend web deve enviar sempre `If-Match`.
- **CORS.** O frontend web corre no browser e, se estiver noutra origem que a API, o browser só deixa o JavaScript ler o `ETag` e enviar o `If-Match` se o servidor os autorizar: a política de CORS da API tem de incluir `Access-Control-Expose-Headers: ETag` e `Access-Control-Allow-Headers: If-Match` (além dos cabeçalhos que já usa, como `Authorization` e `Content-Type`). A aplicação Android não é afetada.
- Os DTOs não mudam: a versão viaja só nos cabeçalhos. A alternativa de um campo `versao` nos DTOs está no ponto em aberto 5 da secção 13.

---

## 6. Endpoints — F001 Autenticação e Controlos de Acesso

### EP-001 — Iniciar sessão

| Campo | Valor |
|---|---|
| Método e rota | `POST /auth/login` |
| Autenticação | Não requer |
| Perfis | Todos |
| Requisitos | RF001, RF002, RF004, RF005 |
| Descrição | Valida as credenciais e devolve um token JWT com o perfil do utilizador. |
| DTO de pedido | `LoginRequestDto` (corpo do pedido) |
| DTO de resposta | `LoginResponseDto` |

**Pedido**
```json
{ "email": "rececao@clinica-g08.pt", "password": "Segura#2026" }
```

**Resposta 200**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiraEm": "2026-10-09T22:00:00Z",
  "utilizador": { "id": 4, "nome": "Ana Silva", "email": "rececao@clinica-g08.pt", "perfil": "Rececionista" }
}
```

**Erros:** 400 — e-mail ou palavra-passe em falta; 401 — credenciais incorretas; 403 — conta bloqueada.

### EP-002 — Terminar sessão

| Campo | Valor |
|---|---|
| Método e rota | `POST /auth/logout` |
| Autenticação | JWT |
| Perfis | Todos |
| Requisitos | RF003 |
| Descrição | Revoga o token utilizado no pedido; o token deixa de permitir acesso autenticado. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | — (204, sem corpo) |

**Pedido:** sem corpo.
**Resposta 204:** sem corpo.
**Erros:** 401 — token ausente ou inválido.

### EP-003 — Consultar utilizador autenticado

| Campo | Valor |
|---|---|
| Método e rota | `GET /auth/me` |
| Autenticação | JWT |
| Perfis | Todos |
| Requisitos | RF004, RF005 |
| Descrição | Devolve os dados e o perfil do utilizador associado ao token, permitindo aos frontends apresentar apenas as funcionalidades autorizadas. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `UtilizadorAutenticadoDto` |

**Resposta 200**
```json
{ "id": 12, "nome": "João Costa", "email": "joao.costa@mail.pt", "perfil": "Cliente", "clienteId": 7 }
```

**Erros:** 401.

### EP-004 — Listar contas de funcionários

| Campo | Valor |
|---|---|
| Método e rota | `GET /utilizadores` |
| Autenticação | JWT |
| Perfis | Administrador |
| Requisitos | RF007, RF008, RF009 |
| Parâmetros (query) | `perfil` (opcional), `estado` (opcional), `pesquisa` (nome ou e-mail), `pagina`, `tamanho` |
| Descrição | Lista as contas de funcionários, para seleção nas operações de atualização, atribuição de perfil e bloqueio. |
| DTO de pedido | `UtilizadorFiltroQuery` (parâmetros no URL) |
| DTO de resposta | `PagedResultDto<UtilizadorListItemDto>` |

**Exemplo:** `GET /utilizadores?perfil=Veterinario&estado=Ativo`

**Resposta 200**
```json
{
  "itens": [
    { "id": 5, "nome": "Dra. Marta Reis", "email": "marta.reis@clinica-g08.pt", "perfil": "Veterinario", "estado": "Ativo", "podeRegistarApoio": false }
  ],
  "pagina": 1, "tamanho": 20, "total": 1
}
```

**Erros:** 400 — perfil ou estado inválido; 401; 403.

### EP-005 — Consultar conta de funcionário

| Campo | Valor |
|---|---|
| Método e rota | `GET /utilizadores/{id}` |
| Autenticação | JWT |
| Perfis | Administrador |
| Requisitos | RF007 |
| Parâmetros (rota) | `id` — identificador do utilizador |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `UtilizadorDto` |

**Resposta 200**
```json
{ "id": 5, "nome": "Dra. Marta Reis", "email": "marta.reis@clinica-g08.pt", "telefone": "912345678", "perfil": "Veterinario", "estado": "Ativo", "podeRegistarApoio": false, "criadoEm": "2026-10-01T09:00:00Z" }
```

**Erros:** 401; 403; 404 — utilizador inexistente.

### EP-006 — Criar conta de funcionário

| Campo | Valor |
|---|---|
| Método e rota | `POST /utilizadores` |
| Autenticação | JWT |
| Perfis | Administrador |
| Requisitos | RF006, RF008 |
| Descrição | Cria uma conta de funcionário com um dos perfis internos. O perfil `Cliente` não é aceite neste endpoint (ver EP-013). Para o perfil `AuxiliarVeterinario`, pode ser definida a autorização `podeRegistarApoio`. |
| DTO de pedido | `CreateUtilizadorDto` (corpo do pedido) |
| DTO de resposta | `UtilizadorListItemDto` (201) |

**Pedido**
```json
{ "nome": "Rui Lopes", "email": "rui.lopes@clinica-g08.pt", "telefone": "934567890", "perfil": "AuxiliarVeterinario", "podeRegistarApoio": true, "passwordInicial": "Temp#2026" }
```

**Resposta 201** (`Location: /api/v1/utilizadores/9`)
```json
{ "id": 9, "nome": "Rui Lopes", "email": "rui.lopes@clinica-g08.pt", "perfil": "AuxiliarVeterinario", "estado": "Ativo", "podeRegistarApoio": true }
```

**Erros:** 400 — campos em falta, e-mail inválido ou palavra-passe fraca; 401; 403; 409 — e-mail já registado; 422 — perfil `Cliente` não permitido ou `podeRegistarApoio=true` para um perfil diferente de `AuxiliarVeterinario`.

### EP-007 — Atualizar conta de funcionário e atribuir perfil

| Campo | Valor |
|---|---|
| Método e rota | `PUT /utilizadores/{id}` |
| Autenticação | JWT |
| Perfis | Administrador |
| Requisitos | RF007, RF008 |
| Descrição | Atualiza nome, e-mail, telefone e perfil interno de um funcionário. Para o perfil `AuxiliarVeterinario`, permite também atualizar a autorização `podeRegistarApoio`. |
| DTO de pedido | `UpdateUtilizadorDto` (corpo do pedido) |
| DTO de resposta | `UtilizadorDto` |

**Pedido**
```json
{ "nome": "Rui Lopes", "email": "rui.lopes@clinica-g08.pt", "telefone": "934567890", "perfil": "AuxiliarVeterinario", "podeRegistarApoio": true }
```

**Resposta 200:** conta atualizada (formato de EP-005).
**Erros:** 400 — dados inválidos ou perfil `Cliente`; 401; 403; 404; 409 — e-mail já usado por outra conta; 422 — o Administrador não pode remover o próprio perfil de Administrador ou `podeRegistarApoio=true` é enviado para um perfil diferente de `AuxiliarVeterinario`.

### EP-008 — Bloquear conta de funcionário

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /utilizadores/{id}/estado` |
| Autenticação | JWT |
| Perfis | Administrador |
| Requisitos | RF009 |
| Descrição | Bloqueia uma conta de funcionário. Uma conta `Bloqueado` não permite autenticação. |
| DTO de pedido | `UpdateEstadoUtilizadorDto` (corpo do pedido) |
| DTO de resposta | — (204, sem corpo) |

**Pedido**
```json
{ "estado": "Bloqueado", "motivo": "Funcionário deixou a clínica." }
```

**Resposta 204:** sem corpo.
**Erros:** 400 — estado diferente de `Bloqueado`; 401; 403; 404; 422 — a conta já está bloqueada, o Administrador tenta bloquear a própria conta ou a conta indicada é de um Cliente.

### EP-009 — Consultar dados gerais, configurações e horário da clínica

| Campo | Valor |
|---|---|
| Método e rota | `GET /clinica` |
| Autenticação | JWT |
| Perfis | Administrador |
| Requisitos | RF012, RF019 |
| Descrição | Devolve os dados gerais da clínica e as configurações do sistema: duração das consultas e horário de funcionamento utilizado no cálculo das disponibilidades (F003). |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `ClinicaDto` |

**Resposta 200**
```json
{
  "nome": "Clínica Veterinária G08",
  "nif": "501234567",
  "morada": "Rua do Exemplo, 100, 4470-000 Maia",
  "telefone": "229000000",
  "email": "geral@clinica-g08.pt",
  "duracaoConsultaMinutos": 30,
  "horarioFuncionamento": [
    { "diaSemana": "Segunda", "abertura": "09:00", "fecho": "13:00" },
    { "diaSemana": "Segunda", "abertura": "14:00", "fecho": "19:00" },
    { "diaSemana": "Sabado", "abertura": "09:00", "fecho": "13:00" }
  ]
}
```

**Erros:** 401; 403.

### EP-010 — Atualizar dados gerais, configurações e horário da clínica

| Campo | Valor |
|---|---|
| Método e rota | `PUT /clinica` |
| Autenticação | JWT |
| Perfis | Administrador |
| Requisitos | RF013, RF019 |
| Descrição | Atualiza os dados gerais, as configurações e o horário de funcionamento. O horário é uma lista de períodos; um mesmo dia pode ter vários períodos desde que não se sobreponham. A configuração passa a ser usada no cálculo das disponibilidades (EP-022). |
| DTO de pedido | `UpdateClinicaDto` (corpo do pedido) |
| DTO de resposta | `ClinicaDto` |

**Pedido:** mesmo formato da resposta de EP-009.
**Resposta 200:** dados atualizados.
**Erros:** 400 — campos inválidos, períodos sobrepostos no mesmo dia ou hora de fecho igual ou anterior à de abertura; 401; 403.

### EP-011 — Pesquisar clientes

| Campo | Valor |
|---|---|
| Método e rota | `GET /clientes` |
| Autenticação | JWT |
| Perfis | Administrador, Rececionista, Veterinário |
| Requisitos | RF011, RF017 |
| Parâmetros (query) | `pesquisa` (nome, NIF, telefone ou e-mail), `pagina`, `tamanho` |
| Descrição | Pesquisa clientes (proprietários), para atualização da conta ou para pesquisa de pacientes por proprietário. |
| DTO de pedido | `ClienteFiltroQuery` (parâmetros no URL) |
| DTO de resposta | `PagedResultDto<ClienteListItemDto>` |

**Exemplo:** `GET /clientes?pesquisa=costa`

**Resposta 200**
```json
{
  "itens": [
    { "id": 7, "nome": "João Costa", "nif": "234567890", "telefone": "961234567", "email": "joao.costa@mail.pt", "numeroAnimais": 2 }
  ],
  "pagina": 1, "tamanho": 20, "total": 1
}
```

**Erros:** 401; 403.

### EP-012 — Consultar cliente

| Campo | Valor |
|---|---|
| Método e rota | `GET /clientes/{id}` |
| Autenticação | JWT |
| Perfis | Administrador, Rececionista, Veterinário |
| Requisitos | RF011, RF017 |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `ClienteDto` |

**Resposta 200**
```json
{
  "id": 7, "nome": "João Costa", "nif": "234567890", "telefone": "961234567",
  "email": "joao.costa@mail.pt", "morada": "Av. Central, 20, Porto",
  "utilizadorId": 12, "estadoConta": "Ativo",
  "animais": [ { "id": 3, "nome": "Bobi", "especie": "Cão" } ]
}
```

**Erros:** 401; 403; 404.

### EP-013 — Criar conta de Cliente

| Campo | Valor |
|---|---|
| Método e rota | `POST /clientes` |
| Autenticação | JWT |
| Perfis | Administrador, Rececionista |
| Requisitos | RF010 |
| Descrição | Regista o proprietário e cria a respetiva conta de acesso à aplicação móvel, com o perfil `Cliente`. O e-mail de acesso fica associado à conta `Utilizador`, evitando duplicação do mesmo dado no registo de Cliente. |
| DTO de pedido | `CreateClienteDto` (corpo do pedido) |
| DTO de resposta | `ClienteDto` (201) |

**Pedido**
```json
{
  "nome": "João Costa", "nif": "234567890", "telefone": "961234567",
  "email": "joao.costa@mail.pt", "morada": "Av. Central, 20, Porto",
  "passwordInicial": "Temp#2026"
}
```

**Resposta 201** (`Location: /api/v1/clientes/7`): cliente criado (formato de EP-012).
**Erros:** 400 — campos em falta, NIF ou e-mail inválido; 401; 403; 409 — NIF ou e-mail já registado.

### EP-014 — Atualizar conta de Cliente

| Campo | Valor |
|---|---|
| Método e rota | `PUT /clientes/{id}` |
| Autenticação | JWT |
| Perfis | Administrador, Rececionista |
| Requisitos | RF011 |
| Descrição | Atualiza os dados administrativos do Cliente e, quando o e-mail é alterado, atualiza o e-mail da conta `Utilizador` associada na mesma operação. |
| DTO de pedido | `UpdateClienteDto` (corpo do pedido) |
| DTO de resposta | `ClienteDto` |

**Pedido**
```json
{ "nome": "João Costa", "nif": "234567890", "telefone": "961234000", "email": "joao.costa@mail.pt", "morada": "Av. Central, 22, Porto" }
```

**Resposta 200:** cliente atualizado (formato de EP-012).
**Erros:** 400; 401; 403; 404; 409 — NIF ou e-mail já usado por outro cliente.

---

## 7. Endpoints — F002 Gestão de pacientes

### EP-015 — Pesquisar pacientes

| Campo | Valor |
|---|---|
| Método e rota | `GET /animais` |
| Autenticação | JWT |
| Perfis | Rececionista, Veterinário |
| Requisitos | RF017 |
| Parâmetros (query) | `clienteId` (opcional), `pesquisa` (nome do animal ou do proprietário), `especie` (opcional), `pagina`, `tamanho` |
| Descrição | Lista os pacientes, nomeadamente os associados a um determinado Cliente. |
| DTO de pedido | `AnimalFiltroQuery` (parâmetros no URL) |
| DTO de resposta | `PagedResultDto<AnimalListItemDto>` |

**Exemplo:** `GET /animais?clienteId=7`

**Resposta 200**
```json
{
  "itens": [
    { "id": 3, "nome": "Bobi", "especie": "Cão", "raca": "Labrador", "idadeAnos": 4, "peso": 28.4, "cliente": { "id": 7, "nome": "João Costa" } }
  ],
  "pagina": 1, "tamanho": 20, "total": 1
}
```

**Erros:** 400 — parâmetros inválidos; 401; 403.

### EP-016 — Consultar ficha do paciente

| Campo | Valor |
|---|---|
| Método e rota | `GET /animais/{id}` |
| Autenticação | JWT |
| Perfis | Rececionista, Veterinário, Auxiliar Veterinário |
| Requisitos | RF015, RF026 |
| Descrição | Devolve apenas os dados administrativos da ficha. O Auxiliar Veterinário tem acesso de leitura para a preparação das consultas (RF026; pressuposto 6). O histórico clínico é obtido em EP-034. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `AnimalDto` |

**Resposta 200**
```json
{
  "id": 3, "nome": "Bobi", "especie": "Cão", "raca": "Labrador", "sexo": "Macho",
  "dataNascimento": "2022-05-10", "idadeAnos": 4, "peso": 28.4,
  "cliente": { "id": 7, "nome": "João Costa", "telefone": "961234567" },
  "atualizadoEm": "2026-10-05T10:12:00Z"
}
```

**Erros:** 401; 403; 404.

### EP-017 — Registar paciente

| Campo | Valor |
|---|---|
| Método e rota | `POST /animais` |
| Autenticação | JWT |
| Perfis | Rececionista, Veterinário |
| Requisitos | RF014 |
| Descrição | Cria a ficha administrativa do paciente, associada inequivocamente a um Cliente existente. |
| DTO de pedido | `CreateAnimalDto` (corpo do pedido) |
| DTO de resposta | `AnimalDto` (201) |

**Pedido**
```json
{ "nome": "Bobi", "especie": "Cão", "raca": "Labrador", "sexo": "Macho", "dataNascimento": "2022-05-10", "peso": 28.4, "clienteId": 7 }
```

**Resposta 201** (`Location: /api/v1/animais/3`): ficha criada (formato de EP-016).
**Erros:** 400 — campos em falta, `clienteId` em falta, peso ≤ 0 ou data de nascimento futura; 401; 403; 404 — cliente inexistente.

### EP-018 — Atualizar dados administrativos do paciente

| Campo | Valor |
|---|---|
| Método e rota | `PUT /animais/{id}` |
| Autenticação | JWT |
| Perfis | Rececionista, Veterinário |
| Requisitos | RF016 |
| Descrição | Atualiza apenas dados administrativos. Não aceita campos clínicos, geridos em F004. |
| DTO de pedido | `UpdateAnimalDto` (corpo do pedido) |
| DTO de resposta | `AnimalDto` |

**Pedido**
```json
{ "nome": "Bobi", "especie": "Cão", "raca": "Labrador", "sexo": "Macho", "dataNascimento": "2022-05-10", "peso": 29.1, "clienteId": 7 }
```

**Resposta 200:** ficha atualizada (formato de EP-016).
**Erros:** 400; 401; 403; 404 — animal ou cliente inexistente.

### EP-019 — Listar os meus animais (Cliente)

| Campo | Valor |
|---|---|
| Método e rota | `GET /me/animais` |
| Autenticação | JWT |
| Perfis | Cliente |
| Requisitos | RF018 |
| Descrição | Lista apenas os animais associados ao Cliente autenticado. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `List<MeuAnimalListItemDto>` |

**Resposta 200**
```json
[
  { "id": 3, "nome": "Bobi", "especie": "Cão", "raca": "Labrador", "idadeAnos": 4, "peso": 29.1 },
  { "id": 8, "nome": "Mia", "especie": "Gato", "raca": "Europeu Comum", "idadeAnos": 2, "peso": 4.2 }
]
```

Se o Cliente não tiver animais, é devolvida uma lista vazia (`[]`).

**Erros:** 401; 403 — perfil diferente de Cliente.

### EP-020 — Consultar o meu animal (Cliente)

| Campo | Valor |
|---|---|
| Método e rota | `GET /me/animais/{id}` |
| Autenticação | JWT |
| Perfis | Cliente |
| Requisitos | RF018 |
| Descrição | Devolve os dados básicos de um animal do Cliente autenticado. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `MeuAnimalDto` |

**Resposta 200**
```json
{ "id": 3, "nome": "Bobi", "especie": "Cão", "raca": "Labrador", "sexo": "Macho", "dataNascimento": "2022-05-10", "idadeAnos": 4, "peso": 29.1 }
```

**Erros:** 401; 403 — o animal não pertence ao Cliente autenticado; 404.

---

## 8. Endpoints — F003 Agendamento de consultas

### Transições de estado da consulta

| Estado atual | Novo estado | Quem | Endpoint |
|---|---|---|---|
| — | `Pendente` | Rececionista | EP-025 |
| `Pendente` | `Confirmada` | Rececionista | EP-027 |
| `Confirmada` | `Realizada` | Rececionista | EP-027 |
| `Pendente` / `Confirmada` | `Cancelada` | Rececionista | EP-027 |

`Realizada` e `Cancelada` são estados finais. Qualquer outra transição devolve **422**. O horário de funcionamento usado pela agenda é gerido em EP-009/EP-010 (RF019).

### EP-021 — Listar veterinários

| Campo | Valor |
|---|---|
| Método e rota | `GET /veterinarios` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF020, RF021 |
| Descrição | Lista os veterinários ativos, para seleção na consulta de disponibilidade e na marcação. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `List<VeterinarioRefDto>` |

**Resposta 200**
```json
[ { "id": 5, "nome": "Dra. Marta Reis" }, { "id": 6, "nome": "Dr. Paulo Santos" } ]
```

**Erros:** 401; 403.

### EP-022 — Consultar disponibilidade de um veterinário

| Campo | Valor |
|---|---|
| Método e rota | `GET /veterinarios/{id}/disponibilidade` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF020, RF019 |
| Parâmetros (query) | `dataInicio` (obrigatório, `AAAA-MM-DD`), `dataFim` (opcional; por omissão igual a `dataInicio`; máximo 31 dias) |
| Descrição | Devolve os horários livres no período, calculados a partir do horário de funcionamento da clínica e das consultas já marcadas (exceto as canceladas). |
| DTO de pedido | `DisponibilidadeQuery` (parâmetros no URL) |
| DTO de resposta | `DisponibilidadeDto` |

**Exemplo:** `GET /veterinarios/5/disponibilidade?dataInicio=2026-10-14&dataFim=2026-10-15`

**Resposta 200**
```json
{
  "veterinarioId": 5, "duracaoConsultaMinutos": 30,
  "dias": [
    { "data": "2026-10-14", "horariosLivres": ["09:00", "09:30", "11:00", "14:30"] },
    { "data": "2026-10-15", "horariosLivres": ["10:00", "10:30"] }
  ]
}
```

**Erros:** 400 — datas em falta, inválidas ou período superior a 31 dias; 401; 403; 404 — veterinário inexistente.

### EP-023 — Consultar agenda

| Campo | Valor |
|---|---|
| Método e rota | `GET /consultas` |
| Autenticação | JWT |
| Perfis | Rececionista, Veterinário, Auxiliar Veterinário |
| Requisitos | RF024, RF026 |
| Parâmetros (query) | `dataInicio`, `dataFim`, `veterinarioId`, `animalId`, `estado` (todos opcionais), `pagina`, `tamanho` |
| Descrição | Lista as marcações do período. Quando o perfil é Veterinário, o servidor ignora `veterinarioId` e devolve apenas as consultas atribuídas ao Veterinário autenticado (claim `veterinarioId`). |
| DTO de pedido | `ConsultaFiltroQuery` (parâmetros no URL) |
| DTO de resposta | `PagedResultDto<ConsultaListItemDto>` |

**Exemplo:** `GET /consultas?dataInicio=2026-10-14&dataFim=2026-10-14`

**Resposta 200**
```json
{
  "itens": [
    {
      "id": 41, "dataHora": "2026-10-14T09:30:00Z", "duracaoMinutos": 30, "estado": "Confirmada",
      "motivo": "Consulta de rotina",
      "animal": { "id": 3, "nome": "Bobi" }, "cliente": { "id": 7, "nome": "João Costa" },
      "veterinario": { "id": 5, "nome": "Dra. Marta Reis" }, "chegadaEm": null
    }
  ],
  "pagina": 1, "tamanho": 20, "total": 1
}
```

**Erros:** 400 — intervalo de datas inválido; 401; 403.

### EP-024 — Consultar marcação

| Campo | Valor |
|---|---|
| Método e rota | `GET /consultas/{id}` |
| Autenticação | JWT |
| Perfis | Rececionista, Veterinário, Auxiliar Veterinário |
| Requisitos | RF024, RF026 |
| Descrição | Devolve a marcação e as notas de preparação não clínicas. O Veterinário só acede às consultas que lhe estão atribuídas. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `ConsultaDto` |

**Resposta 200**
```json
{
  "id": 41, "dataHora": "2026-10-14T09:30:00Z", "duracaoMinutos": 30, "estado": "Confirmada",
  "motivo": "Consulta de rotina",
  "animal": { "id": 3, "nome": "Bobi" }, "cliente": { "id": 7, "nome": "João Costa" },
  "veterinario": { "id": 5, "nome": "Dra. Marta Reis" }, "chegadaEm": "2026-10-14T09:22:00Z",
  "notasPreparacao": [ { "id": 2, "texto": "Sala 2 preparada; tutor pediu corte de unhas.", "autor": "Rui Lopes", "criadaEm": "2026-10-14T09:25:00Z" } ]
}
```

**Erros:** 401; 403 — consulta não atribuída ao Veterinário autenticado; 404.

### EP-025 — Criar marcação

| Campo | Valor |
|---|---|
| Método e rota | `POST /consultas` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF021 |
| Descrição | Cria uma marcação no estado `Pendente`, associada a um paciente, a uma data e hora e a um Veterinário, desde que exista disponibilidade. |
| DTO de pedido | `CreateConsultaDto` (corpo do pedido) |
| DTO de resposta | `ConsultaDto` (201) |

**Pedido**
```json
{ "animalId": 3, "veterinarioId": 5, "dataHora": "2026-10-14T09:30:00Z", "motivo": "Consulta de rotina" }
```

**Resposta 201** (`Location: /api/v1/consultas/41`): marcação criada (formato de EP-024).
**Erros:** 400 — campos em falta ou data no passado; 401; 403; 404 — animal ou veterinário inexistente; 422 — fora do horário de funcionamento ou veterinário indisponível nesse horário.

### EP-026 — Alterar marcação

| Campo | Valor |
|---|---|
| Método e rota | `PUT /consultas/{id}` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF022 |
| Descrição | Altera data, hora, veterinário ou motivo de uma marcação `Pendente` ou `Confirmada`, validando a disponibilidade aplicável. |
| Cabeçalhos | `If-Match` (opcional, mas recomendado): último `ETag` conhecido (secção 5.1) |
| DTO de pedido | `UpdateConsultaDto` (corpo do pedido) |
| DTO de resposta | `ConsultaDto` |

**Pedido**
```json
{ "veterinarioId": 6, "dataHora": "2026-10-15T10:00:00Z", "motivo": "Consulta de rotina" }
```

**Resposta 200:** marcação atualizada (formato de EP-024), com o novo `ETag`.
**Erros:** 400; 401; 403; 404; 409 — duas gravações simultâneas da mesma marcação; 412 — o `If-Match` enviado já não corresponde à versão atual; 422 — marcação já `Realizada` ou `Cancelada`, ou novo horário indisponível.

### EP-027 — Atualizar estado da marcação

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /consultas/{id}/estado` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF023 |
| Descrição | Altera o estado da marcação para `Confirmada`, `Realizada` ou `Cancelada`, segundo as transições permitidas. |
| Cabeçalhos | `If-Match` (opcional, mas recomendado): último `ETag` conhecido (secção 5.1) |
| DTO de pedido | `UpdateEstadoConsultaDto` (corpo do pedido) |
| DTO de resposta | — (204, sem corpo) |

**Pedido**
```json
{ "estado": "Realizada", "observacao": null }
```

**Exemplo de cancelamento**
```json
{ "estado": "Cancelada", "observacao": "Pedido do tutor por telefone." }
```

**Resposta 204:** sem corpo, com o novo `ETag`.
**Erros:** 400 — estado inexistente ou `Pendente`; 401; 403; 404; 409 — duas gravações simultâneas da mesma marcação; 412 — o `If-Match` enviado já não corresponde à versão atual; 422 — transição inválida.

### EP-028 — Registar chegada

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /consultas/{id}/chegada` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF025 |
| Descrição | Regista a chegada do Cliente e do animal à consulta, com a hora atual do servidor. |
| Cabeçalhos | `If-Match` (opcional, mas recomendado): último `ETag` conhecido (secção 5.1) |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `ChegadaDto` |

**Pedido:** sem corpo.
**Resposta 200**
```json
{ "id": 41, "chegadaEm": "2026-10-14T09:22:00Z" }
```

**Erros:** 401; 403; 404; 409 — duas gravações simultâneas da mesma marcação; 412 — o `If-Match` enviado já não corresponde à versão atual; 422 — marcação não está `Confirmada` ou a chegada já foi registada.

### EP-029 — Registar informação não clínica de apoio

| Campo | Valor |
|---|---|
| Método e rota | `POST /consultas/{id}/notas-preparacao` |
| Autenticação | JWT |
| Perfis | Auxiliar Veterinário |
| Requisitos | RF027 |
| Descrição | Regista informação não clínica de apoio à preparação ou acompanhamento da consulta quando `podeRegistarApoio` está ativo para o Auxiliar autenticado. Não altera registos clínicos nem a marcação. |
| DTO de pedido | `CreateNotaPreparacaoDto` (corpo do pedido) |
| DTO de resposta | `NotaPreparacaoDto` (201) |

**Pedido**
```json
{ "texto": "Sala 2 preparada; tutor pediu corte de unhas." }
```

**Resposta 201**
```json
{ "id": 2, "consultaId": 41, "texto": "Sala 2 preparada; tutor pediu corte de unhas.", "autor": "Rui Lopes", "criadaEm": "2026-10-14T09:25:00Z" }
```

**Erros:** 400 — texto vazio ou superior a 1000 caracteres; 401; 403 — perfil sem permissão ou Auxiliar sem autorização de registo de apoio; 404; 422 — marcação `Cancelada`.

### EP-030 — Consultar as minhas marcações (Cliente)

| Campo | Valor |
|---|---|
| Método e rota | `GET /me/consultas` |
| Autenticação | JWT |
| Perfis | Cliente |
| Requisitos | RF028, RF029 |
| Parâmetros (query) | `tipo` = `futuras` (por omissão) ou `historico`; `animalId` (opcional) |
| Descrição | Devolve as marcações futuras ou o histórico de marcações passadas e consultas realizadas dos animais do Cliente. Apenas informação administrativa (data, animal, Veterinário, estado), sem registos clínicos. Não existem operações de escrita para o Cliente. |
| DTO de pedido | `MinhasConsultasQuery` (parâmetros no URL) |
| DTO de resposta | `List<MinhaConsultaDto>` |

**Exemplo:** `GET /me/consultas?tipo=historico`

**Resposta 200**
```json
[
  { "id": 33, "dataHora": "2026-09-02T15:00:00Z", "estado": "Realizada", "animal": { "id": 3, "nome": "Bobi" }, "veterinario": "Dra. Marta Reis" }
]
```

**Erros:** 400 — tipo inválido; 401; 403 — perfil diferente de Cliente ou `animalId` de outro Cliente.

---

## 9. Endpoints — F004 Registo clínico e tratamentos

### EP-031 — Criar registo clínico

| Campo | Valor |
|---|---|
| Método e rota | `POST /registos-clinicos` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF030, RF031, RF032, RF033 |
| Descrição | Regista notas clínicas, diagnóstico, tratamentos e prescrições associados a uma consulta e ao respetivo paciente. Existe no máximo um registo clínico por consulta; tratamentos e prescrições adicionais são acrescentados através de EP-032. O registo não altera o estado da marcação. |
| DTO de pedido | `CreateRegistoClinicoDto` (corpo do pedido) |
| DTO de resposta | `RegistoClinicoDto` (201) |

**Pedido**
```json
{
  "consultaId": 41,
  "notasClinicas": "Animal ativo; ligeira inflamação no ouvido esquerdo.",
  "diagnostico": "Otite externa",
  "tratamentos": [ { "descricao": "Limpeza auricular", "dataAplicacao": "2026-10-14" } ],
  "prescricoes": [ { "medicamento": "Gotas auriculares X", "dosagem": "3 gotas", "frequencia": "2 vezes por dia", "duracaoDias": 7 } ]
}
```

**Resposta 201** (`Location: /api/v1/registos-clinicos/18`)
```json
{
  "id": 18, "consultaId": 41, "animalId": 3, "veterinario": "Dra. Marta Reis", "criadoEm": "2026-10-14T10:05:00Z",
  "notasClinicas": "Animal ativo; ligeira inflamação no ouvido esquerdo.", "diagnostico": "Otite externa",
  "tratamentos": [ { "id": 25, "descricao": "Limpeza auricular", "dataAplicacao": "2026-10-14" } ],
  "prescricoes": [ { "id": 30, "medicamento": "Gotas auriculares X", "dosagem": "3 gotas", "frequencia": "2 vezes por dia", "duracaoDias": 7 } ]
}
```

**Erros:** 400 — `consultaId` em falta, nenhum elemento clínico preenchido ou prescrição incompleta; 401; 403; 404 — consulta inexistente; 409 — a consulta já tem registo clínico; 422 — marcação `Pendente` ou `Cancelada`.

### EP-032 — Atualizar registo clínico

| Campo | Valor |
|---|---|
| Método e rota | `PUT /registos-clinicos/{id}` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF034, RF032, RF033 |
| Descrição | Altera notas clínicas, diagnóstico, tratamentos e prescrições anteriormente registados. O registo mantém-se associado à consulta e ao paciente. A alteração fica registada com autor e data (`atualizadoEm`, `atualizadoPor`). |
| DTO de pedido | `UpdateRegistoClinicoDto` (corpo do pedido) |
| DTO de resposta | `RegistoClinicoDto` |

**Pedido:** mesmo formato de EP-031, sem `consultaId`. As listas `tratamentos` e `prescricoes` substituem as anteriores; elementos existentes mantêm-se indicando o respetivo `id`.

**Resposta 200:** registo atualizado (formato da resposta de EP-031, com `atualizadoEm` e `atualizadoPor`).
**Erros:** 400; 401; 403; 404.

### EP-033 — Consultar registo clínico

| Campo | Valor |
|---|---|
| Método e rota | `GET /registos-clinicos/{id}` |
| Autenticação | JWT |
| Perfis | Veterinário, Auxiliar Veterinário |
| Requisitos | RF035, RF036 |
| Descrição | Devolve um registo clínico. Para o Auxiliar Veterinário, o acesso é apenas de leitura. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `RegistoClinicoDto` |

**Resposta 200:** formato da resposta de EP-031.
**Erros:** 401; 403; 404.

### EP-034 — Consultar histórico clínico do paciente

| Campo | Valor |
|---|---|
| Método e rota | `GET /animais/{id}/historico-clinico` |
| Autenticação | JWT |
| Perfis | Veterinário, Auxiliar Veterinário |
| Requisitos | RF035, RF036 |
| Parâmetros (query) | `dataInicio`, `dataFim` (opcionais) |
| Descrição | Devolve os registos clínicos do paciente por ordem cronológica descendente, permitindo acompanhar a evolução. |
| DTO de pedido | `HistoricoClinicoQuery` (parâmetros no URL) |
| DTO de resposta | `HistoricoClinicoDto` |

**Resposta 200**
```json
{
  "animal": { "id": 3, "nome": "Bobi", "especie": "Cão" },
  "registos": [
    { "id": 18, "consultaId": 41, "data": "2026-10-14", "veterinario": "Dra. Marta Reis", "diagnostico": "Otite externa", "numeroTratamentos": 1, "numeroPrescricoes": 1 },
    { "id": 11, "consultaId": 33, "data": "2026-09-02", "veterinario": "Dra. Marta Reis", "diagnostico": "Check-up sem alterações", "numeroTratamentos": 0, "numeroPrescricoes": 0 }
  ]
}
```

**Erros:** 400 — intervalo de datas inválido; 401; 403; 404.

---

## 10. Endpoints — F006 Boletim de vacinas e desparasitações

### EP-042 — Consultar boletim do paciente (web)

| Campo | Valor |
|---|---|
| Método e rota | `GET /animais/{id}/vacinacoes` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF046, RF045 |
| Parâmetros (query) | `tipo` (opcional: `Vacina`/`Desparasitacao`) |
| Descrição | Devolve o histórico completo de vacinação e desparasitação do paciente e as próximas administrações previstas, quando existam. |
| DTO de pedido | `BoletimQuery` (parâmetros no URL) |
| DTO de resposta | `BoletimDto` |

**Resposta 200**
```json
{
  "animal": { "id": 3, "nome": "Bobi" },
  "administracoes": [
    { "id": 21, "tipo": "Vacina", "produto": "Vacina antirrábica", "lote": "RB-2291", "dataAdministracao": "2026-10-14", "intervaloDias": 365, "dataProximaPrevista": "2027-10-14", "veterinario": "Dra. Marta Reis" },
    { "id": 19, "tipo": "Desparasitacao", "produto": "Desparasitante externo Z", "lote": "DZ-101", "dataAdministracao": "2026-08-01", "intervaloDias": null, "dataProximaPrevista": null, "veterinario": "Dr. Paulo Santos" }
  ],
  "proximas": [ { "tipo": "Vacina", "produto": "Vacina antirrábica", "dataProximaPrevista": "2027-10-14" } ]
}
```

**Erros:** 400; 401; 403; 404.

### EP-043 — Registar vacinação ou desparasitação

| Campo | Valor |
|---|---|
| Método e rota | `POST /animais/{id}/vacinacoes` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF043, RF044, RF045 |
| Descrição | Regista uma vacinação ou desparasitação com data de administração, produto, lote e, quando aplicável, o intervalo até à próxima toma. Se `intervaloDias` for indicado, o servidor calcula `dataProximaPrevista = dataAdministracao + intervaloDias`; caso contrário, `dataProximaPrevista` fica `null`. |
| DTO de pedido | `CreateAdministracaoDto` (corpo do pedido) |
| DTO de resposta | `AdministracaoDto` (201) |

**Pedido**
```json
{ "tipo": "Desparasitacao", "produto": "Desparasitante interno Y", "lote": "DP-0042", "dataAdministracao": "2026-10-14", "intervaloDias": 90 }
```

**Resposta 201**
```json
{ "id": 22, "tipo": "Desparasitacao", "produto": "Desparasitante interno Y", "lote": "DP-0042", "dataAdministracao": "2026-10-14", "intervaloDias": 90, "dataProximaPrevista": "2027-01-12", "veterinario": "Dra. Marta Reis" }
```

**Erros:** 400 — tipo, produto, lote ou data em falta, data de administração futura ou `intervaloDias` ≤ 0 quando indicado; 401; 403; 404 — animal inexistente.

### EP-044 — Consultar boletim do meu animal (Cliente)

| Campo | Valor |
|---|---|
| Método e rota | `GET /me/animais/{id}/vacinacoes` |
| Autenticação | JWT |
| Perfis | Cliente |
| Requisitos | RF047 |
| Descrição | Devolve o histórico de vacinação e desparasitação de um animal do Cliente e as próximas administrações previstas. |
| DTO de pedido | — (sem corpo nem parâmetros específicos) |
| DTO de resposta | `MeuBoletimDto` |

**Resposta 200**
```json
{
  "historico": [ { "tipo": "Vacina", "produto": "Vacina antirrábica", "dataAdministracao": "2026-10-14" } ],
  "proximas": [
    { "tipo": "Desparasitacao", "produto": "Desparasitante interno Y", "dataProximaPrevista": "2027-01-12" },
    { "tipo": "Vacina", "produto": "Vacina antirrábica", "dataProximaPrevista": "2027-10-14" }
  ]
}
```

Se não existirem registos, `historico` e `proximas` são listas vazias.

**Erros:** 401; 403 — animal de outro Cliente; 404.

---

## 11. Elementos fora do âmbito da API

Em conformidade com a Declaração de Âmbito v05 e a secção 7 da Especificação de Requisitos, a API **não** disponibiliza:

- **gestão de contas e pagamentos (F005)**: registo de custos, consulta de contas e valores em dívida, registo de pagamentos, estado de pagamento, fecho de conta e consulta de despesas pelo Cliente (anteriores EP-035 a EP-041);
- **gestão de internamento (F007)**: alojamentos, painel de internamentos, registo de internamento, atribuição de alojamento, estado clínico do internamento e alta médica (anteriores EP-045 a EP-051);
- autenticação através de fornecedores externos (Google, Facebook) nem autenticação de dupla camada ou multifator;
- marcação, alteração ou cancelamento de consultas pelo Cliente;
- criação ou edição de fichas de animais pelo Cliente;
- pagamentos eletrónicos (MB Way, Referências Multibanco, TPA) nem emissão de faturas certificadas ou comunicação com a Autoridade Tributária;
- envio de notificações ou lembretes por SMS ou e-mail, nem sincronização com calendários externos;
- geração de prescrições em PDF;
- integração com o SIAC, bases de dados de microchips ou de saúde animal;
- leitura de códigos de barras ou QR de produtos;
- integração com equipamentos (leitores de microchip, balanças, monitores de sinais vitais, câmaras, diagnóstico por imagem);
- acesso do Cliente a registos clínicos;
- migração de dados de aplicações anteriores;
- funcionamento offline.

## 12. Rastreabilidade

### 12.1. Resumo por funcionalidade

| Funcionalidade | Endpoints | Requisitos | Casos de uso |
|---|---|---|---|
| F001 — Autenticação e Controlos de Acesso | EP-001 a EP-014 (14) | RF001–RF013, RF019 | UC01–UC05 |
| F002 — Gestão de pacientes | EP-015 a EP-020 (6) | RF014–RF018 | UC06–UC08 |
| F003 — Agendamento de consultas | EP-021 a EP-030 (10) | RF019–RF029 | UC09–UC11 |
| F004 — Registo clínico e tratamentos | EP-031 a EP-034 (4) | RF030–RF036 | UC12, UC13 |
| F005 — Gestão de contas e pagamentos | **Fora do âmbito** (EP-035 a EP-041 retirados) | RF037–RF042 | UC14, UC15 (fora do âmbito) |
| F006 — Boletim de vacinas e desparasitações | EP-042 a EP-044 (3) | RF043–RF047 | UC16, UC17 |
| F007 — Gestão de internamento | **Fora do âmbito** (EP-045 a EP-051 retirados) | RF048–RF052 | UC18 (fora do âmbito) |
| **Total** | **37 endpoints** | | |

O RF019 (horários de funcionamento) pertence à F003, mas é satisfeito pelos endpoints de configuração da clínica (EP-009, EP-010).

### 12.2. Endpoints por cliente da API (RNF003)

| Cliente da API | Endpoints utilizados |
|---|---|
| Frontend web | EP-001 a EP-018, EP-021 a EP-029, EP-031 a EP-034, EP-042, EP-043 |
| Frontend mobile (Cliente) | EP-001 a EP-003, EP-019, EP-020, EP-030, EP-044 |

Os endpoints EP-001 a EP-003 são partilhados pelos dois clientes: 33 endpoints web + 7 mobile − 3 partilhados = 37.

### 12.3. Ligação a requisitos funcionais e casos de uso

A matriz seguinte relaciona cada endpoint com os requisitos funcionais definidos em `m3-especificacao-requisitos-v02.md` e com os casos de uso descritos em `m3-especificacoes-casos-uso-v02.md`.

| Endpoint | Método e rota | Requisito(s) | Caso(s) de uso |
|---|---|---|---|
| EP-001 | `POST /auth/login` | RF001, RF002, RF004, RF005 | UC01 |
| EP-002 | `POST /auth/logout` | RF003 | UC02 |
| EP-003 | `GET /auth/me` | RF004, RF005 | UC01 |
| EP-004 | `GET /utilizadores` | RF007, RF008, RF009 | UC03 (A1, A2, A3) |
| EP-005 | `GET /utilizadores/{id}` | RF007 | UC03 (A1) |
| EP-006 | `POST /utilizadores` | RF006, RF008 | UC03 |
| EP-007 | `PUT /utilizadores/{id}` | RF007, RF008 | UC03 (A1, A2) |
| EP-008 | `PATCH /utilizadores/{id}/estado` | RF009 | UC03 (A3) |
| EP-009 | `GET /clinica` | RF012, RF019 | UC05, UC09 |
| EP-010 | `PUT /clinica` | RF013, RF019 | UC05 (A1), UC09 |
| EP-011 | `GET /clientes` | RF011, RF017 | UC04 (A1), UC07 (A1) |
| EP-012 | `GET /clientes/{id}` | RF011, RF017 | UC04 (A1), UC07 (A1) |
| EP-013 | `POST /clientes` | RF010 | UC04 |
| EP-014 | `PUT /clientes/{id}` | RF011 | UC04 (A1) |
| EP-015 | `GET /animais` | RF017 | UC07 (A1) |
| EP-016 | `GET /animais/{id}` | RF015, RF026 | UC07, UC10 |
| EP-017 | `POST /animais` | RF014 | UC06 |
| EP-018 | `PUT /animais/{id}` | RF016 | UC07 (A2) |
| EP-019 | `GET /me/animais` | RF018 | UC08 |
| EP-020 | `GET /me/animais/{id}` | RF018 | UC08 |
| EP-021 | `GET /veterinarios` | RF020, RF021 | UC09 |
| EP-022 | `GET /veterinarios/{id}/disponibilidade` | RF020, RF019 | UC09 |
| EP-023 | `GET /consultas` | RF024, RF026 | UC09 (A3, A4), UC10 |
| EP-024 | `GET /consultas/{id}` | RF024, RF026 | UC09, UC10 |
| EP-025 | `POST /consultas` | RF021 | UC09 |
| EP-026 | `PUT /consultas/{id}` | RF022 | UC09 (A1) |
| EP-027 | `PATCH /consultas/{id}/estado` | RF023 | UC09 (A2) |
| EP-028 | `PATCH /consultas/{id}/chegada` | RF025 | UC09 (A5) |
| EP-029 | `POST /consultas/{id}/notas-preparacao` | RF027 | UC10 (A1) |
| EP-030 | `GET /me/consultas` | RF028, RF029 | UC11 |
| EP-031 | `POST /registos-clinicos` | RF030, RF031, RF032, RF033 | UC12 (fluxo principal, E1 a E5) |
| EP-032 | `PUT /registos-clinicos/{id}` | RF034, RF032, RF033 | UC12 (A1) |
| EP-033 | `GET /registos-clinicos/{id}` | RF035, RF036 | UC12 (A2), UC13 |
| EP-034 | `GET /animais/{id}/historico-clinico` | RF035, RF036 | UC12 (A3), UC13 |
| EP-042 | `GET /animais/{id}/vacinacoes` | RF046, RF045 | UC16 (A2) |
| EP-043 | `POST /animais/{id}/vacinacoes` | RF043, RF044, RF045 | UC16 (fluxo principal e A1) |
| EP-044 | `GET /me/animais/{id}/vacinacoes` | RF047 | UC17 |

Os endpoints EP-035 a EP-041 e EP-045 a EP-051 foram retirados e os seus números não voltam a ser usados.

### 12.4. Cobertura dos requisitos funcionais

Todos os requisitos funcionais **em âmbito** estão cobertos por, pelo menos, um endpoint. Os requisitos RF037 a RF042 (F005) e RF048 a RF052 (F007) estão fora do âmbito (Declaração de Âmbito v05) e não têm endpoint.

| Requisito | Endpoint(s) | Requisito | Endpoint(s) |
|---|---|---|---|
| RF001 | EP-001 | RF027 | EP-029 |
| RF002 | EP-001 | RF028 | EP-030 |
| RF003 | EP-002 | RF029 | EP-030 |
| RF004 | EP-001, EP-003 | RF030 | EP-031 |
| RF005 | EP-001, EP-003 | RF031 | EP-031 |
| RF006 | EP-006 | RF032 | EP-031, EP-032 |
| RF007 | EP-004, EP-005, EP-007 | RF033 | EP-031, EP-032 |
| RF008 | EP-004, EP-006, EP-007 | RF034 | EP-032 |
| RF009 | EP-004, EP-008 | RF035 | EP-033, EP-034 |
| RF010 | EP-013 | RF036 | EP-033, EP-034 |
| RF011 | EP-011, EP-012, EP-014 | RF037 (fora do âmbito) | — |
| RF012 | EP-009 | RF038 (fora do âmbito) | — |
| RF013 | EP-010 | RF039 (fora do âmbito) | — |
| RF014 | EP-017 | RF040 (fora do âmbito) | — |
| RF015 | EP-016 | RF041 (fora do âmbito) | — |
| RF016 | EP-018 | RF042 (fora do âmbito) | — |
| RF017 | EP-011, EP-012, EP-015 | RF043 | EP-043 |
| RF018 | EP-019, EP-020 | RF044 | EP-043 |
| RF019 | EP-009, EP-010, EP-022 | RF045 | EP-042, EP-043 |
| RF020 | EP-021, EP-022 | RF046 | EP-042 |
| RF021 | EP-021, EP-025 | RF047 | EP-044 |
| RF022 | EP-026 | RF048 (fora do âmbito) | — |
| RF023 | EP-027 | RF049 (fora do âmbito) | — |
| RF024 | EP-023, EP-024 | RF050 (fora do âmbito) | — |
| RF025 | EP-028 | RF051 (fora do âmbito) | — |
| RF026 | EP-016, EP-023, EP-024 | RF052 (fora do âmbito) | — |

### 12.5. Ligação a requisitos não funcionais

| Requisito | Como é suportado pela API |
|---|---|
| RNF001 — Controlo de acesso às operações | Autorização por perfil e por propriedade (secção 3.3), matriz de permissões (secção 4) e resposta 403 em todos os endpoints com restrição de perfil. |
| RNF002 — Isolamento dos dados de Clientes | Rotas `/me/...` (EP-019, EP-020, EP-030, EP-044), com o Cliente identificado exclusivamente pelo token; 403 em acesso a dados de outro Cliente. |
| RNF003 — Separação dos canais de utilização | O perfil Cliente só acede a `/auth/*` e `/me/*`; os restantes endpoints são exclusivos dos perfis internos (secção 12.2). |
| RNF004 — Validação dos dados de entrada | Validação de campos obrigatórios e restrições em todas as operações de criação e alteração, com resposta 400 e campo `errors` (secção 5). |
| RNF005 — Persistência dos dados confirmados | Operações de criação e alteração devolvem 201/200 apenas após gravação; os dados ficam disponíveis nos endpoints de consulta correspondentes. |
| RNF006 — Dependência de ligação à rede | API REST acessível por rede; não é previsto modo offline (secção 2). |

### 12.6. Pressupostos assumidos

1. O estado `Realizada` de uma marcação é definido pelo Rececionista (RF023). O registo clínico não altera o estado da marcação.
2. O Auxiliar Veterinário consulta a ficha administrativa do paciente (EP-016) ao abrigo do RF026 e só pode registar informação de apoio em EP-029 quando a respetiva autorização está ativa.
3. Os identificadores EP-035 a EP-041 e EP-045 a EP-051 ficam reservados e não são reutilizados, para que as referências à v01 continuem inequívocas.
4. Em alguns exemplos o Veterinário aparece como texto (`"veterinario": "Dra. Marta Reis"` em EP-030, EP-031 a EP-034, EP-042 e EP-043) e noutros como objeto (`VeterinarioRefDto`, em EP-021, EP-023 e EP-024). Esta v02 mantém o contrato da v01; a eventual uniformização está no ponto em aberto 1 da secção 13.
5. A concorrência nas marcações assenta em `Marcacao.RowVersion` e é exposta por `ETag` e `If-Match` (secção 5.1; 412 com `If-Match` desatualizado, 409 em gravações simultâneas). O mesmo mecanismo não foi estendido a outros recursos, que nesta versão não têm token de versão.

## 13. Pontos em aberto

| N.º | Ponto | Proposta |
|---|---|---|
| 1 | **Veterinário como texto ou como objeto.** Ver pressuposto 4 da secção 12.6. | Decidir na revisão se `MinhaConsultaDto`, `RegistoClinicoDto`, `RegistoClinicoResumoDto` e `AdministracaoDto` passam a usar `VeterinarioRefDto`. Se sim, atualizam-se em conjunto esta documentação e o Modelo de Classes. |
| 2 | **Requisitos RF053 a RF056 do Backlog de Projeto v03.** O backlog em preparação refere requisitos que ainda não constam de nenhuma Especificação de Requisitos aprovada: Cliente sem conta de acesso (RF053, RF054), horários regulares de atendimento por Veterinário (RF055) e atualização de um registo de vacinação ou desparasitação (RF056). | Esta v02 **não** cria endpoints para esses requisitos, para não documentar comportamento por aprovar. Quando a Especificação de Requisitos v02 estiver aprovada, prevê-se acrescentar, por exemplo, ajustes a EP-013 e EP-014 (conta de acesso opcional), um recurso de horários por Veterinário e uma operação de atualização em EP-043. Será uma v03 desta documentação. |
| 3 | **Dependência do Modelo de Classes v02.** Os nomes dos DTOs referidos neste documento são os do `m3-modelo-classes-backend-v02.md`. | Aprovar os dois documentos em conjunto, ou aprovar primeiro o Modelo de Classes. |
| 4 | **Informação clínica lida por outros perfis e por outros Veterinários.** O Auxiliar Veterinário lê o registo clínico completo (EP-033 e EP-034) e o histórico (EP-034) lista registos com o nome do Veterinário que os fez. A regra 3.3 só diz que o Veterinário acede às consultas que lhe estão atribuídas. | Decidir na revisão (1) se o Auxiliar continua a ler tudo, como nesta v02 (o UC13 já está escrito assim), ou se passa a haver um DTO reduzido para este perfil; e (2) se um Veterinário pode ler registos de consultas de colegas ou só os das suas. Se a resposta mudar, atualizam-se os casos de uso UC12 e UC13 e o Modelo de Classes. |
| 5 | **Como o cliente envia a versão da marcação.** Esta v02 propõe `ETag` e `If-Match` (secção 5.1), que não mudam os DTOs. A alternativa é um campo `versao` em `ConsultaDto`, `UpdateConsultaDto` e `UpdateEstadoConsultaDto`, mas EP-028 não tem corpo. | Decidir na revisão. Se se escolher o campo `versao`, atualizam-se esta documentação, o Modelo de Classes do Backend e os modelos de classes do frontend web. Em qualquer caso, o frontend web tem de guardar a versão lida em EP-024 e devolvê-la nas gravações. |

# Documentação da API — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Documentação da API (prevista) |
| Ficheiro | `m2-documentacao-api-v01.md` |
| Versão | v01 |
| Milestone de Entrega | m2 |
| Issue | I018 — Documentação de API |
| Executor | André Filipe Moreira Mesquita |
| Revisor | Ivan Cristiano Silva Mendes |
| Auditor | Gonçalo Magalhães Ribeiro |
| Pasta de destino | `/projeto/04-artefactos-tecnicos/04.11-documentacao-de-api` |
| Fontes | `m1-proposta-sistema-v03.pdf`; `m1-declaracao-ambito-v03.pdf`; `m2-especificacao-requisitos-v01.md`; `m2-especificacoes-casos-uso-v01.md`; `m2-documentacao-arquitetura-v01.md` |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-10-09 | André Filipe Moreira Mesquita | Versão inicial da documentação prevista da API para a m2, alinhada com a Especificação de Requisitos (RF001–RF052, RNF001–RNF006) e com os Casos de Uso (UC01–UC18). |

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
10. [Endpoints — F005 Gestão de contas e pagamentos](#10-endpoints--f005-gestão-de-contas-e-pagamentos)
11. [Endpoints — F006 Boletim de vacinas e desparasitações](#11-endpoints--f006-boletim-de-vacinas-e-desparasitações)
12. [Endpoints — F007 Gestão de internamento](#12-endpoints--f007-gestão-de-internamento)
13. [Elementos fora do âmbito da API](#13-elementos-fora-do-âmbito-da-api)
14. [Rastreabilidade](#14-rastreabilidade)

---

## 1. Introdução

Este documento descreve a API REST **prevista** para o backend do sistema de Gestão de Clínica Veterinária. A API será implementada em ASP.NET Core 8 com Entity Framework Core (code-first) e autenticação JWT, e será consumida por dois clientes:

- **Frontend web** (React): utilizado pelos perfis Administrador, Rececionista, Veterinário e Auxiliar Veterinário.
- **Frontend mobile** (Flutter): utilizado exclusivamente pelo perfil Cliente (Proprietário do animal).

A documentação cobre todas as funcionalidades F001 a F007 da Proposta de Sistema e todos os requisitos funcionais RF001 a RF052 da Especificação de Requisitos, respeitando o âmbito e as restrições de permissões da Declaração de Âmbito. Na m3 será produzida uma versão atualizada que refletirá a API efetivamente implementada.

## 2. Convenções gerais

| Aspeto | Convenção |
|---|---|
| URL base | `https://<servidor>/api/v1`. Em desenvolvimento, a URL é configurável por cliente (por exemplo, `https://localhost:5001/api/v1` no Web e `http://10.0.2.2:<porta>/api/v1` no emulador Android). O uso de HTTP fica limitado ao ambiente local de desenvolvimento. |
| Formato | JSON (`Content-Type: application/json; charset=utf-8`) |
| Nomes de campos | camelCase, em português, sem acentos (ex.: `dataNascimento`) |
| Rotas | Substantivos no plural, minúsculas, separados por hífen (ex.: `/registos-clinicos`). Operações sobre um atributo específico usam um sub-recurso (ex.: `/consultas/{id}/estado`) |
| Identificadores | Inteiros positivos (`id`) |
| Datas | ISO 8601. Datas simples: `AAAA-MM-DD`. Data e hora: UTC, `AAAA-MM-DDThh:mm:ssZ` |
| Valores monetários | Número decimal em euros, com duas casas decimais (ex.: `35.50`) |
| Enumerações | Strings em PascalCase (ex.: `Pendente`, `EmObservacao`) |
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
| `EstadoPagamento` | `Pendente`, `Pago` |
| `MetodoPagamento` | `Numerario`, `Multibanco`, `Transferencia` (registo manual ao balcão) |
| `TipoAdministracao` | `Vacina`, `Desparasitacao` |
| `EstadoInternamento` | `EmObservacao`, `Recobro`, `AltaMedica` |
| `Sexo` | `Macho`, `Femea`, `Desconhecido` |

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
  - um **Cliente** só acede aos seus animais, consultas, despesas e boletins (RNF002);
  - um **Veterinário** só acede às consultas que lhe estão atribuídas na agenda (RF024).
- Uma conta com estado `Bloqueado` não consegue autenticar-se; tokens já emitidos para essa conta são rejeitados (RF009).
- O perfil **Cliente** só tem acesso às rotas `/auth/*` e `/me/*`, consumidas pela aplicação móvel (RNF003).
- O perfil **Administrador** não acede a registos clínicos, internamentos nem boletins de vacinação (Declaração de Âmbito, F001).
- O perfil **Auxiliar Veterinário** não tem acesso a operações de escrita clínica, de marcações, de estado de internamento nem de alta médica (RF027, RF036, RF051, RF052).

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
| Contas e pagamentos | — | L/E | — | — | L (`/me`) |
| Vacinações e desparasitações | — | — | L/E | — | L (`/me`) |
| Alojamentos | — | — | L | L | — |
| Internamentos | — | — | L/E | L | — |

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
| 409 Conflict | Conflito com dados existentes (ex.: e-mail já registado) |
| 422 Unprocessable Entity | Regra de negócio violada (ex.: transição de estado inválida) |
| 500 Internal Server Error | Erro inesperado no servidor |

Nas tabelas de endpoints seguintes, **401** aplica-se a todos os endpoints protegidos e **403** a todos os endpoints com restrição de perfil, mesmo quando não repetidos.

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

**Pedido**
```json
{ "veterinarioId": 6, "dataHora": "2026-10-15T10:00:00Z", "motivo": "Consulta de rotina" }
```

**Resposta 200:** marcação atualizada (formato de EP-024).
**Erros:** 400; 401; 403; 404; 422 — marcação já `Realizada` ou `Cancelada`, ou novo horário indisponível.

### EP-027 — Atualizar estado da marcação

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /consultas/{id}/estado` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF023 |
| Descrição | Altera o estado da marcação para `Confirmada`, `Realizada` ou `Cancelada`, segundo as transições permitidas. |

**Pedido**
```json
{ "estado": "Realizada", "observacao": null }
```

**Exemplo de cancelamento**
```json
{ "estado": "Cancelada", "observacao": "Pedido do tutor por telefone." }
```

**Resposta 204:** sem corpo.
**Erros:** 400 — estado inexistente ou `Pendente`; 401; 403; 404; 422 — transição inválida.

### EP-028 — Registar chegada

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /consultas/{id}/chegada` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF025 |
| Descrição | Regista a chegada do Cliente e do animal à consulta, com a hora atual do servidor. |

**Pedido:** sem corpo.
**Resposta 200**
```json
{ "id": 41, "chegadaEm": "2026-10-14T09:22:00Z" }
```

**Erros:** 401; 403; 404; 422 — marcação não está `Confirmada` ou a chegada já foi registada.

### EP-029 — Registar informação não clínica de apoio

| Campo | Valor |
|---|---|
| Método e rota | `POST /consultas/{id}/notas-preparacao` |
| Autenticação | JWT |
| Perfis | Auxiliar Veterinário |
| Requisitos | RF027 |
| Descrição | Regista informação não clínica de apoio à preparação ou acompanhamento da consulta quando `podeRegistarApoio` está ativo para o Auxiliar autenticado. Não altera registos clínicos nem a marcação. |

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

## 10. Endpoints — F005 Gestão de contas e pagamentos

> O sistema efetua apenas registo interno. Não emite documentos fiscalmente certificados nem comunica com a Autoridade Tributária.

Cada **conta** corresponde a um custo registado para um Cliente, associado a uma consulta ou tratamento. Tem um estado de pagamento (`Pendente`/`Pago`) e pode ser **encerrada** pelo Rececionista. Uma conta encerrada deixa de ser apresentada como conta em aberto e não admite novas operações.

### EP-035 — Listar contas e valores em dívida

| Campo | Valor |
|---|---|
| Método e rota | `GET /contas` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF038 |
| Parâmetros (query) | `clienteId`, `estado` (`Pendente`/`Pago`), `encerrada` (por omissão `false`), `dataInicio`, `dataFim`, `pagina`, `tamanho` |
| Descrição | Lista as contas; com `clienteId` e `estado=Pendente` apresenta os valores em dívida de um Cliente. |

**Exemplo:** `GET /contas?clienteId=7&estado=Pendente`

**Resposta 200**
```json
{
  "itens": [
    { "id": 15, "cliente": { "id": 7, "nome": "João Costa" }, "descricao": "Consulta de rotina + limpeza auricular", "valor": 45.00, "valorPago": 20.00, "valorEmDivida": 25.00, "estado": "Pendente", "encerrada": false, "registadaEm": "2026-10-14T10:10:00Z" }
  ],
  "pagina": 1, "tamanho": 20, "total": 1,
  "totalEmDivida": 25.00
}
```

**Erros:** 400; 401; 403.

### EP-036 — Registar custo

| Campo | Valor |
|---|---|
| Método e rota | `POST /contas` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF037 |
| Descrição | Regista o custo de uma consulta ou tratamento, associado ao Cliente e ao serviço. A conta é criada com o estado `Pendente`. |

**Pedido**
```json
{ "clienteId": 7, "consultaId": 41, "animalId": 3, "descricao": "Consulta de rotina + limpeza auricular", "valor": 45.00 }
```

**Resposta 201** (`Location: /api/v1/contas/15`): conta criada (formato de EP-037).
**Erros:** 400 — valor ≤ 0 ou descrição em falta; 401; 403; 404 — cliente, consulta ou animal inexistente; 422 — consulta ou animal não pertence ao cliente indicado.

### EP-037 — Consultar conta

| Campo | Valor |
|---|---|
| Método e rota | `GET /contas/{id}` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF038, RF039 |

**Resposta 200**
```json
{
  "id": 15, "cliente": { "id": 7, "nome": "João Costa" }, "consultaId": 41, "animal": { "id": 3, "nome": "Bobi" },
  "descricao": "Consulta de rotina + limpeza auricular", "valor": 45.00, "valorPago": 20.00, "valorEmDivida": 25.00,
  "estado": "Pendente", "encerrada": false, "encerradaEm": null,
  "pagamentos": [ { "id": 9, "valor": 20.00, "metodo": "Numerario", "data": "2026-10-14T10:15:00Z", "registadoPor": "Ana Silva" } ]
}
```

**Erros:** 401; 403; 404.

### EP-038 — Registar pagamento ao balcão

| Campo | Valor |
|---|---|
| Método e rota | `POST /contas/{id}/pagamentos` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF039, RF040 |
| Descrição | Regista manualmente um pagamento efetuado ao balcão (total ou parcial), associado ao Cliente e à conta. Quando o valor pago iguala o valor da conta, o estado de pagamento passa automaticamente a `Pago`. A conta não é encerrada automaticamente (ver EP-040). |

**Pedido**
```json
{ "valor": 25.00, "metodo": "Multibanco" }
```

**Resposta 201**
```json
{ "contaId": 15, "pagamentoId": 10, "valorPago": 45.00, "valorEmDivida": 0.00, "estado": "Pago", "encerrada": false }
```

**Erros:** 400 — valor ≤ 0 ou método inválido; 401; 403; 404; 422 — conta encerrada, já `Pago` ou valor superior ao valor em dívida.

### EP-039 — Atualizar estado de pagamento

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /contas/{id}/estado-pagamento` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF040 |
| Descrição | Corrige administrativamente o estado de pagamento (`Pendente` ou `Pago`). A justificação é obrigatória e fica guardada no histórico da conta. O estado tem de permanecer coerente com o montante em dívida: `Pago` apenas quando `valorEmDivida = 0` e `Pendente` quando `valorEmDivida > 0`. |

**Pedido**
```json
{ "estado": "Pendente", "observacao": "Pagamento registado por engano na conta errada." }
```

**Resposta 200**
```json
{ "id": 15, "estado": "Pendente", "valorEmDivida": 25.00, "encerrada": false }
```

**Erros:** 400 — estado inválido ou `observacao` em falta; 401; 403; 404; 422 — conta encerrada, estado igual ao atual ou estado incoerente com `valorEmDivida`.

### EP-040 — Fechar conta

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /contas/{id}/fecho` |
| Autenticação | JWT |
| Perfis | Rececionista |
| Requisitos | RF041 |
| Descrição | Encerra a conta. Só é permitido quando o estado de pagamento é `Pago` e `valorEmDivida = 0`. Uma conta encerrada deixa de ser listada como conta em aberto (EP-035 com `encerrada=false`). |

**Pedido:** sem corpo.

**Resposta 200**
```json
{ "id": 15, "estado": "Pago", "encerrada": true, "encerradaEm": "2026-10-14T10:20:00Z" }
```

**Erros:** 401; 403; 404; 422 — conta já encerrada, com estado `Pendente` ou com valor em dívida superior a zero.

### EP-041 — Consultar as minhas despesas (Cliente)

| Campo | Valor |
|---|---|
| Método e rota | `GET /me/despesas` |
| Autenticação | JWT |
| Perfis | Cliente |
| Requisitos | RF042 |
| Parâmetros (query) | `estado` (opcional), `animalId` (opcional) |
| Descrição | Lista as despesas dos serviços prestados aos animais do Cliente e os respetivos estados de pagamento. Apenas leitura; não existem operações para alterar valores, registar ou efetuar pagamentos. |

**Resposta 200**
```json
[
  { "id": 15, "data": "2026-10-14", "animal": "Bobi", "descricao": "Consulta de rotina + limpeza auricular", "valor": 45.00, "valorEmDivida": 0.00, "estado": "Pago" }
]
```

**Erros:** 400; 401; 403 — perfil diferente de Cliente ou `animalId` de outro Cliente.

---

## 11. Endpoints — F006 Boletim de vacinas e desparasitações

### EP-042 — Consultar boletim do paciente (web)

| Campo | Valor |
|---|---|
| Método e rota | `GET /animais/{id}/vacinacoes` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF046, RF045 |
| Parâmetros (query) | `tipo` (opcional: `Vacina`/`Desparasitacao`) |
| Descrição | Devolve o histórico completo de vacinação e desparasitação do paciente e as próximas administrações previstas, quando existam. |

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

## 12. Endpoints — F007 Gestão de internamento

### Transições de estado do internamento

| Estado atual | Novo estado | Quem | Endpoint |
|---|---|---|---|
| — | `EmObservacao` | Veterinário | EP-048 |
| `EmObservacao` | `Recobro` | Veterinário | EP-050 |
| `Recobro` | `EmObservacao` | Veterinário | EP-050 |
| `EmObservacao` / `Recobro` | `AltaMedica` | Veterinário | EP-051 |

Após `AltaMedica` o internamento deixa de estar ativo e não pode ser alterado (**422**). O Auxiliar Veterinário tem apenas acesso de leitura.

### EP-045 — Listar alojamentos

| Campo | Valor |
|---|---|
| Método e rota | `GET /alojamentos` |
| Autenticação | JWT |
| Perfis | Veterinário, Auxiliar Veterinário |
| Requisitos | RF049, RF050 |
| Parâmetros (query) | `disponivel` (opcional, `true`/`false`) |
| Descrição | Lista os alojamentos (jaulas, boxes ou outras localizações) e a sua ocupação. Os alojamentos são carregados por script de seed (pressuposto 2). |

**Resposta 200**
```json
[
  { "id": 1, "codigo": "J-01", "tipo": "Jaula pequena", "disponivel": false, "internamentoAtualId": 6 },
  { "id": 2, "codigo": "J-02", "tipo": "Jaula pequena", "disponivel": true, "internamentoAtualId": null }
]
```

**Erros:** 401; 403.

### EP-046 — Consultar painel de internamentos

| Campo | Valor |
|---|---|
| Método e rota | `GET /internamentos` |
| Autenticação | JWT |
| Perfis | Veterinário, Auxiliar Veterinário |
| Requisitos | RF050 |
| Parâmetros (query) | `ativos` (por omissão `true`), `pagina`, `tamanho` |
| Descrição | Painel dos pacientes atualmente internados e respetivos alojamentos, quando associados. |

**Resposta 200**
```json
{
  "itens": [
    { "id": 6, "animal": { "id": 8, "nome": "Mia" }, "alojamento": "J-01", "estado": "Recobro", "dataEntrada": "2026-10-12T16:00:00Z", "veterinarioResponsavel": "Dr. Paulo Santos" },
    { "id": 7, "animal": { "id": 3, "nome": "Bobi" }, "alojamento": null, "estado": "EmObservacao", "dataEntrada": "2026-10-14T11:00:00Z", "veterinarioResponsavel": "Dra. Marta Reis" }
  ],
  "pagina": 1, "tamanho": 20, "total": 2
}
```

**Erros:** 400; 401; 403.

### EP-047 — Consultar internamento

| Campo | Valor |
|---|---|
| Método e rota | `GET /internamentos/{id}` |
| Autenticação | JWT |
| Perfis | Veterinário, Auxiliar Veterinário |
| Requisitos | RF050 |

**Resposta 200**
```json
{
  "id": 6, "animal": { "id": 8, "nome": "Mia", "especie": "Gato" }, "alojamento": { "id": 1, "codigo": "J-01" },
  "motivo": "Pós-operatório de esterilização", "estado": "Recobro", "dataEntrada": "2026-10-12T16:00:00Z", "dataAlta": null,
  "historicoEstados": [
    { "estado": "EmObservacao", "data": "2026-10-12T16:00:00Z", "veterinario": "Dr. Paulo Santos" },
    { "estado": "Recobro", "data": "2026-10-13T09:00:00Z", "veterinario": "Dr. Paulo Santos", "observacao": "Sinais vitais estáveis." }
  ]
}
```

**Erros:** 401; 403; 404.

### EP-048 — Registar internamento

| Campo | Valor |
|---|---|
| Método e rota | `POST /internamentos` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF048 |
| Descrição | Cria um internamento associado ao paciente, com estado inicial `EmObservacao`. O alojamento é opcional neste momento e pode ser associado depois através de EP-049. |

**Pedido**
```json
{ "animalId": 8, "motivo": "Pós-operatório de esterilização", "alojamentoId": 1 }
```

**Resposta 201** (`Location: /api/v1/internamentos/6`): internamento criado (formato de EP-047).
**Erros:** 400 — `animalId` ou `motivo` em falta; 401; 403; 404 — animal ou alojamento inexistente; 409 — alojamento ocupado ou animal já com internamento ativo.

### EP-049 — Associar alojamento ao internamento

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /internamentos/{id}/alojamento` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF049 |
| Descrição | Associa ou altera o alojamento de um internamento ativo. Se já existia um alojamento, este é libertado. |

**Pedido**
```json
{ "alojamentoId": 2 }
```

**Resposta 200**
```json
{ "id": 7, "alojamento": { "id": 2, "codigo": "J-02" } }
```

**Erros:** 400 — `alojamentoId` em falta; 401; 403; 404 — internamento ou alojamento inexistente; 409 — alojamento ocupado; 422 — internamento com alta médica.

### EP-050 — Atualizar estado clínico do internamento

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /internamentos/{id}/estado` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF051 |
| Descrição | Atualiza o estado clínico do paciente internado (`EmObservacao` ou `Recobro`). Operação exclusiva do Veterinário. |

**Pedido**
```json
{ "estado": "Recobro", "observacao": "Sinais vitais estáveis." }
```

**Resposta 204:** sem corpo.
**Erros:** 400 — estado inválido (a alta é registada em EP-051); 401; 403; 404; 422 — internamento com alta médica ou estado igual ao atual.

### EP-051 — Registar alta médica

| Campo | Valor |
|---|---|
| Método e rota | `PATCH /internamentos/{id}/alta` |
| Autenticação | JWT |
| Perfis | Veterinário |
| Requisitos | RF052 |
| Descrição | Regista a alta médica, liberta o alojamento (quando associado) e encerra o internamento, que deixa de ser apresentado como ativo. Operação exclusiva do Veterinário. |

**Pedido**
```json
{ "observacao": "Recuperação completa; revisão em 10 dias." }
```

**Resposta 200**
```json
{ "id": 6, "estado": "AltaMedica", "dataAlta": "2026-10-15T11:00:00Z", "alojamentoLibertado": "J-01" }
```

**Erros:** 401; 403; 404; 422 — internamento já com alta médica.

---

## 13. Elementos fora do âmbito da API

Em conformidade com a Declaração de Âmbito e a secção 7 da Especificação de Requisitos, a API **não** disponibiliza:

- autenticação através de fornecedores externos (Google, Facebook) nem autenticação de dupla camada ou multifator;
- marcação, alteração ou cancelamento de consultas pelo Cliente;
- criação ou edição de fichas de animais pelo Cliente;
- pagamentos eletrónicos (MB Way, Referências Multibanco, TPA) nem emissão de faturas certificadas ou comunicação com a Autoridade Tributária;
- envio de notificações ou lembretes por SMS ou e-mail, nem sincronização com calendários externos;
- geração de prescrições em PDF;
- integração com o SIAC, bases de dados de microchips ou de saúde animal;
- leitura de códigos de barras ou QR de produtos;
- integração com equipamentos (leitores de microchip, balanças, monitores de sinais vitais, câmaras, diagnóstico por imagem);
- acesso do Cliente a registos clínicos ou a internamentos;
- migração de dados de aplicações anteriores;
- funcionamento offline.

## 14. Rastreabilidade

### 14.1. Resumo por funcionalidade

| Funcionalidade | Endpoints | Requisitos | Casos de uso |
|---|---|---|---|
| F001 — Autenticação e Controlos de Acesso | EP-001 a EP-014 | RF001–RF013, RF019 | UC01–UC05 |
| F002 — Gestão de pacientes | EP-015 a EP-020 | RF014–RF018 | UC06–UC08 |
| F003 — Agendamento de consultas | EP-021 a EP-030 | RF019–RF029 | UC09–UC11 |
| F004 — Registo clínico e tratamentos | EP-031 a EP-034 | RF030–RF036 | UC12, UC13 |
| F005 — Gestão de contas e pagamentos | EP-035 a EP-041 | RF037–RF042 | UC14, UC15 |
| F006 — Boletim de vacinas e desparasitações | EP-042 a EP-044 | RF043–RF047 | UC16, UC17 |
| F007 — Gestão de internamento | EP-045 a EP-051 | RF048–RF052 | UC18 |

O RF019 (horários de funcionamento) pertence à F003, mas é satisfeito pelos endpoints de configuração da clínica (EP-009, EP-010).

### 14.2. Endpoints por cliente da API (RNF003)

| Cliente da API | Endpoints utilizados |
|---|---|
| Frontend web | EP-001 a EP-018, EP-021 a EP-029, EP-031 a EP-040, EP-042, EP-043, EP-045 a EP-051 |
| Frontend mobile (Cliente) | EP-001 a EP-003, EP-019, EP-020, EP-030, EP-041, EP-044 |

### 14.3. Ligação a requisitos funcionais e casos de uso

A matriz seguinte relaciona cada endpoint com os requisitos funcionais definidos em `m2-especificacao-requisitos-v01.md` e com os casos de uso descritos em `m2-especificacoes-casos-uso-v01.md`.

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
| EP-031 | `POST /registos-clinicos` | RF030, RF031, RF032, RF033 | UC12 (fluxo principal, A1, A2, A3) |
| EP-032 | `PUT /registos-clinicos/{id}` | RF034, RF032, RF033 | UC12 (A2, A3, A4) |
| EP-033 | `GET /registos-clinicos/{id}` | RF035, RF036 | UC12 (A5), UC13 |
| EP-034 | `GET /animais/{id}/historico-clinico` | RF035, RF036 | UC12 (A5), UC13 |
| EP-035 | `GET /contas` | RF038 | UC14 (A1) |
| EP-036 | `POST /contas` | RF037 | UC14 |
| EP-037 | `GET /contas/{id}` | RF038, RF039 | UC14 (A1, A2) |
| EP-038 | `POST /contas/{id}/pagamentos` | RF039, RF040 | UC14 (A2) |
| EP-039 | `PATCH /contas/{id}/estado-pagamento` | RF040 | UC14 (A3) |
| EP-040 | `PATCH /contas/{id}/fecho` | RF041 | UC14 (A4) |
| EP-041 | `GET /me/despesas` | RF042 | UC15 |
| EP-042 | `GET /animais/{id}/vacinacoes` | RF046, RF045 | UC16 (A2) |
| EP-043 | `POST /animais/{id}/vacinacoes` | RF043, RF044, RF045 | UC16, UC16 (A1) |
| EP-044 | `GET /me/animais/{id}/vacinacoes` | RF047 | UC17 |
| EP-045 | `GET /alojamentos` | RF049, RF050 | UC18 (A1, A2) |
| EP-046 | `GET /internamentos` | RF050 | UC18 (A2) |
| EP-047 | `GET /internamentos/{id}` | RF050 | UC18 (A2) |
| EP-048 | `POST /internamentos` | RF048 | UC18 |
| EP-049 | `PATCH /internamentos/{id}/alojamento` | RF049 | UC18 (A1) |
| EP-050 | `PATCH /internamentos/{id}/estado` | RF051 | UC18 (A3) |
| EP-051 | `PATCH /internamentos/{id}/alta` | RF052 | UC18 (A4) |

### 14.4. Cobertura dos requisitos funcionais

Todos os requisitos funcionais estão cobertos por, pelo menos, um endpoint:

| Requisito | Endpoint(s) | Requisito | Endpoint(s) |
|---|---|---|---|
| RF001 | EP-001 | RF027 | EP-029 |
| RF002 | EP-001 | RF028 | EP-030 |
| RF003 | EP-002 | RF029 | EP-030 |
| RF004 | EP-001, EP-003 | RF030 | EP-031 |
| RF005 | EP-001, EP-003 | RF031 | EP-031 |
| RF006 | EP-006 | RF032 | EP-031, EP-032 |
| RF007 | EP-004, EP-005, EP-007 | RF033 | EP-031, EP-032 |
| RF008 | EP-006, EP-007 | RF034 | EP-032 |
| RF009 | EP-008 | RF035 | EP-033, EP-034 |
| RF010 | EP-013 | RF036 | EP-033, EP-034 |
| RF011 | EP-011, EP-012, EP-014 | RF037 | EP-036 |
| RF012 | EP-009 | RF038 | EP-035, EP-037 |
| RF013 | EP-010 | RF039 | EP-038 |
| RF014 | EP-017 | RF040 | EP-038, EP-039 |
| RF015 | EP-016 | RF041 | EP-040 |
| RF016 | EP-018 | RF042 | EP-041 |
| RF017 | EP-011, EP-015 | RF043 | EP-043 |
| RF018 | EP-019, EP-020 | RF044 | EP-043 |
| RF019 | EP-009, EP-010, EP-022 | RF045 | EP-042, EP-043 |
| RF020 | EP-021, EP-022 | RF046 | EP-042 |
| RF021 | EP-025 | RF047 | EP-044 |
| RF022 | EP-026 | RF048 | EP-048 |
| RF023 | EP-027 | RF049 | EP-045, EP-049 |
| RF024 | EP-023, EP-024 | RF050 | EP-046, EP-047 |
| RF025 | EP-028 | RF051 | EP-050 |
| RF026 | EP-016, EP-023, EP-024 | RF052 | EP-051 |

### 14.5. Ligação a requisitos não funcionais

| Requisito | Como é suportado pela API |
|---|---|
| RNF001 — Controlo de acesso às operações | Autorização por perfil e por propriedade (secção 3.3), matriz de permissões (secção 4) e resposta 403 em todos os endpoints com restrição de perfil. |
| RNF002 — Isolamento dos dados de Clientes | Rotas `/me/...` (EP-019, EP-020, EP-030, EP-041, EP-044), com o Cliente identificado exclusivamente pelo token; 403 em acesso a dados de outro Cliente. |
| RNF003 — Separação dos canais de utilização | O perfil Cliente só acede a `/auth/*` e `/me/*`; os restantes endpoints são exclusivos dos perfis internos (secção 14.2). |
| RNF004 — Validação dos dados de entrada | Validação de campos obrigatórios e restrições em todas as operações de criação e alteração, com resposta 400 e campo `errors` (secção 5). |
| RNF005 — Persistência dos dados confirmados | Operações de criação e alteração devolvem 201/200 apenas após gravação; os dados ficam disponíveis nos endpoints de consulta correspondentes. |
| RNF006 — Dependência de ligação à rede | API REST acessível por rede; não é previsto modo offline (secção 2). |

### 14.6. Pressupostos assumidos

1. Os alojamentos são carregados por script de seed; a sua criação e edição não fazem parte do âmbito.
2. O estado `Realizada` de uma marcação é definido pelo Rececionista (RF023). O registo clínico não altera o estado da marcação.
3. O Rececionista regista os custos das consultas e tratamentos (RF037).
4. São aceites pagamentos parciais; o estado passa automaticamente a `Pago` quando o valor em dívida chega a zero. Uma correção administrativa do estado (RF040) não altera pagamentos e só é aceite quando o estado final é coerente com o montante em dívida. O fecho da conta é uma operação explícita, permitida apenas quando a conta está `Pago` e sem valor em dívida (RF041).
5. O Auxiliar Veterinário consulta a ficha administrativa do paciente (EP-016) ao abrigo do RF026 e só pode registar informação de apoio em EP-029 quando a respetiva autorização está ativa.

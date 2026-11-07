# Modelo de Classes — Frontend Web

**Projeto:** Gestão de Clínica Veterinária  
**Grupo:** G08  
**Milestone:** m3  
**Versão:** v02  
**Issue e papéis:** por atribuir. Esta versão ainda não tem Issue própria; será planeada no Backlog Refinement e no Sprint Planning, onde se atribuem Executor, Revisor e Auditor. A Issue I021 (v01, m2) está concluída e não é reaberta.  
**Estado:** Rascunho para revisão e auditoria  

## Histórico de versões

| Versão | Data | Descrição das alterações |
|---|---|---|
| v01 | m2 | Modelo de classes e estruturas do frontend web (W01 a W18), com as sete funcionalidades. Mantém-se inalterado como histórico (`m2-modelo-classes-frontend-web-v01.md`). |
| v02 | 23/10/2026 | Reflete a Declaração de Âmbito v05: retira as páginas, serviços e modelos de F005 (contas e pagamentos) e de F007 (internamento). Atualiza as tecnologias para o Regulamento Interno do Grupo v02 (React 19.3, TypeScript 6.0, Vite 8.3). Acrescenta a correspondência entre os modelos TypeScript e os DTOs da API v02 (secção 21) e corrige dois pontos: o corpo de EP-008 (`motivo`) e a enumeração `DiaSemana`. |

### Alterações da v02 em relação à v01

| Tema | Alteração |
|---|---|
| Âmbito | W14, W15, W17 e W18 passam a **fora do âmbito**; ficam W01 a W13 e W16. A numeração dos ecrãs mantém-se reservada. Saem `ContasPage`, `ContaDetalhePage`, `InternamentosPage`, `InternamentoDetalhePage`, `ContasService`, `InternamentosService` e os modelos de contas, pagamentos e internamentos. |
| Endpoints | O frontend web passa a consumir 33 endpoints (EP-001 a EP-018, EP-021 a EP-029, EP-031 a EP-034, EP-042 e EP-043); os restantes 4 dos 37 (EP-019, EP-020, EP-030 e EP-044) são só da aplicação móvel. |
| Tecnologias | React 18.2 e TypeScript 5.3 passam a React 19.3 e TypeScript 6.0; acrescentados Vite 8.3, Node.js 24 LTS e as ferramentas de teste e de análise estática do Regulamento Interno do Grupo v02 (secção 4.1). |
| Enumerações | Saem `EstadoPagamento`, `MetodoPagamento`, `EstadoInternamento` e `EstadoInternamentoAtualizavel`. Entra `DiaSemana`, já usada pela API em EP-009 e EP-010. |
| Modelos | `HorarioFuncionamento.diaSemana` passa de texto a `DiaSemana`. Novo `AtualizarEstadoUtilizadorRequest` (EP-008: `estado` e `motivo` opcional) e `UtilizadoresService.bloquear` passa a aceitar o motivo. `UtilizadoresService.criar` (EP-006) passa a devolver `FuncionarioResumo`, porque a resposta 201 da API traz os campos da lista e não os do detalhe. |
| DTOs da API | Nova secção 21 com a correspondência entre cada modelo TypeScript e o DTO da API v02, por endpoint. |
| Perfis | Retiradas as referências a contas, pagamentos e internamentos nas regras por perfil (secção 13). |
| Concorrência | Nova secção 9.2: o `AgendaService` guarda o `ETag` lido nas respostas de marcações e envia-o em `If-Match` nas gravações EP-026, EP-027 e EP-028; 412 e 409 têm tratamento próprio (secção 14). Segue a proposta da Documentação da API v02, secção 5.1, e a coluna `Marcacao.RowVersion` do Modelo de Dados v02, a confirmar pelo Revisor. |

---

## 1. Objetivo

O presente documento define o modelo de classes e estruturas previsto para o **Frontend Web** do sistema de Gestão de Clínica Veterinária.

O modelo representa as estruturas relevantes da aplicação React/TypeScript, as respetivas responsabilidades, atributos, operações e relações, mantendo coerência com:

- a Especificação de Requisitos;
- as Especificações dos Casos de Uso;
- o Protótipo de Baixa Fidelidade;
- a Documentação da Arquitetura;
- a Documentação da API;
- o Modelo de Dados;
- o Modelo de Classes do Backend.

O Frontend Web corresponde ao componente arquitetural **ARC-WEB** e destina-se exclusivamente aos perfis internos:

- Administrador;
- Rececionista;
- Veterinário;
- Auxiliar Veterinário.

O perfil **Cliente** utiliza exclusivamente a aplicação móvel e não faz parte deste modelo.

A versão v02 reflete a Declaração de Âmbito v05: as funcionalidades F005 (Gestão de contas e pagamentos) e F007 (Gestão de internamento) estão fora do âmbito e não fazem parte deste modelo.

---

## 2. Fontes e referências

| Referência | Utilização neste documento |
|---|---|
| `m1-proposta-sistema-v03.pdf` e `m3-proposta-sistema-v05.pdf` | Âmbito funcional homologado e atores. |
| `m3-declaracao-ambito-v05.pdf` | Limites dos canais Web/Mobile; F005 e F007 fora do âmbito. |
| `m3-regulamento-grupo-v02.pdf` | Tecnologias, convenções e ferramentas adotadas pelo grupo (secções 11.2 a 11.4). |
| `m3-especificacao-requisitos-v02.md` | Requisitos RF001–RF036 e RF043–RF047 em âmbito e RNF001–RNF006. |
| `m3-especificacoes-casos-uso-v02.md` | UC01–UC13, UC16 e UC17, atores, fluxos, alternativas e permissões. |
| `m3-prototipo-baixa-fidelidade-v02.md` | Ecrãs W01–W13 e W16 e organização prevista da interface Web. |
| `m3-documentacao-arquitetura-v02.md` | ARC-WEB, ARC-API, autenticação, autorização e separação de responsabilidades. Tecnologias conforme o Regulamento Interno do Grupo v02. |
| `m3-documentacao-api-v02.md` | Endpoints, contratos JSON, DTOs, perfis autorizados, erros e enumerações. |
| `m3-modelo-de-dados-clinica-veterinaria-v02.md` (rascunho) | Entidades e conceitos persistentes relevantes para os DTOs (16 tabelas, sem F005 e F007). |
| `m3-modelo-classes-backend-v02.md` | Nomenclatura e estruturas previstas no backend, incluindo os DTOs. |
| `m3-diagrama-sequencia-autenticar-utilizador-v02.md` | Fluxo UC01 e armazenamento do token no cliente. |

Quando exista conflito, prevalecem os documentos homologados e normativos e, no que respeita ao contrato entre frontend e backend, a Documentação da API v02.

---

## 3. Âmbito do modelo

O modelo cobre exclusivamente o **Frontend Web React/TypeScript**.

São representadas quatro categorias principais de estruturas:

1. **Core e segurança do frontend**
   - cliente HTTP;
   - contexto de autenticação;
   - proteção de rotas;
   - tratamento normalizado de erros;
   - layout e navegação condicionada pelo perfil.

2. **Páginas da aplicação**
   - correspondentes aos ecrãs W01–W13 e W16 definidos no protótipo v02 (W14, W15, W17 e W18 pertencem a F005 e F007 e estão fora do âmbito).

3. **Serviços de acesso à API**
   - encapsulam as operações HTTP previstas na Documentação da API.

4. **Models / DTOs TypeScript**
   - representam os dados recebidos e enviados através da API;
   - não constituem entidades persistentes do frontend;
   - não substituem as entidades do backend nem o modelo de dados.

Não são modelados:

- o Frontend Mobile;
- entidades Entity Framework;
- acesso direto à base de dados;
- regras de negócio autoritativas;
- endpoints exclusivos do Cliente em `/me/*`;
- funcionalidades fora do âmbito aprovado, em particular F005 (contas e pagamentos) e F007 (internamento).

---

## 4. Convenções de modelação

### 4.1. React e UML

A aplicação Web utiliza as tecnologias do Regulamento Interno do Grupo v02 (secção 11.2):

| Elemento | Tecnologia |
|---|---|
| Linguagem | TypeScript 6.0 |
| Biblioteca de interface | React 19.3 |
| Ambiente de execução | Node.js 24 LTS |
| Compilação e gestão de pacotes | Vite 8.3 e npm 12 |
| Testes | Vitest 5.0 e React Testing Library 16.3 (unitários); Playwright 1.64 (testes de sistema) |
| Análise estática e formatação | ESLint 10.13 com typescript-eslint, eslint-plugin-react-hooks e eslint-plugin-react-refresh; Prettier 3.9 |

Em React, as páginas e componentes serão preferencialmente implementados como componentes funcionais e não como classes ES/TypeScript tradicionais. Por esse motivo, a notação UML de classe é utilizada neste documento como forma de representar **estruturas, responsabilidades, estado, propriedades e operações**, através dos seguintes estereótipos:

- `<<page>>` — página React;
- `<<component>>` — componente reutilizável;
- `<<context>>` — contexto/estado transversal;
- `<<guard>>` — proteção de acesso;
- `<<service>>` — serviço de comunicação com a API;
- `<<interface>>` — interface/DTO TypeScript;
- `<<enum>>` — enumeração utilizada pelo frontend;
- `<<infrastructure>>` — infraestrutura transversal.

### 4.2. Responsabilidade do frontend

O Frontend Web:

- apresenta apenas as opções compatíveis com o perfil autenticado;
- recolhe dados e efetua validações de interface;
- comunica com a API;
- apresenta respostas e erros;
- suporta os fluxos Web dos casos de uso.

As decisões finais de autorização, validação de regras de negócio e persistência continuam a pertencer ao Backend / API.

### 4.3. Decisões internas introduzidas por este modelo

As seguintes decisões são de **organização interna do Frontend Web** e não constituem novas funcionalidades de negócio:

- utilização de um `ApiClient` genérico para centralizar pedidos HTTP e o token JWT;
- utilização de um `AuthContext` para manter o utilizador autenticado durante a execução da aplicação;
- utilização de `ProtectedRoute` para restringir a apresentação das páginas por perfil;
- separação entre `pages`, `services`, `models` e elementos transversais;
- utilização de interfaces TypeScript específicas para os contratos da API.

Estas decisões concretizam ARC-WEB sem alterar o âmbito funcional definido nos documentos anteriores.

---

## 5. Organização lógica prevista

```text
/src/frontend/src
│
├── core/
│   ├── api/
│   │   └── ApiClient.ts
│   └── auth/
│       ├── AuthContext.tsx
│       ├── ProtectedRoute.tsx
│       └── TokenStorage.ts
│
├── layout/
│   └── AppLayout.tsx
│
├── pages/
│   ├── auth/
│   ├── funcionarios/
│   ├── clientes/
│   ├── pacientes/
│   ├── clinica/
│   ├── agenda/
│   ├── clinica-registos/
│   └── profilaxia/
│
├── services/
│   ├── AuthService.ts
│   ├── UtilizadoresService.ts
│   ├── ClinicaService.ts
│   ├── ClientesService.ts
│   ├── PacientesService.ts
│   ├── AgendaService.ts
│   ├── RegistosClinicosService.ts
│   └── ProfilaxiaService.ts
│
└── models/
    ├── common.ts
    ├── auth.ts
    ├── utilizadores.ts
    ├── clinica.ts
    ├── clientes.ts
    ├── pacientes.ts
    ├── agenda.ts
    ├── clinica-registos.ts
    └── profilaxia.ts
```

A estrutura física concreta pode sofrer pequenos ajustamentos durante a implementação desde que sejam preservadas as responsabilidades, relações e contratos definidos neste modelo.

---

# 6. Modelo transversal — Core, autenticação e navegação

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
skinparam packageStyle rectangle
hide empty members

package "Core / Infraestrutura" {
  class ApiClient <<infrastructure>> {
    - baseUrl : string
    - token : string?
    + setToken(token : string?) : void
    + get<T>(rota : string) : Promise<T>
    + post<TReq,TRes>(rota : string, body : TReq) : Promise<TRes>
    + put<TReq,TRes>(rota : string, body : TReq) : Promise<TRes>
    + patch<TReq,TRes>(rota : string, body? : TReq) : Promise<TRes>
  }

  interface ApiProblemDetails <<interface>> {
    + type : string?
    + title : string
    + status : number
    + detail : string?
    + instance : string?
    + traceId : string?
    + errors : Record<string,string[]>?
  }

  interface PagedResult<T> <<interface>> {
    + itens : T[]
    + pagina : number
    + tamanho : number
    + total : number
  }

  class TokenStorage <<infrastructure>> {
    + guardar(sessao : SessaoAutenticada) : void
    + obter() : SessaoAutenticada?
    + limpar() : void
  }
}

package "Autenticação" {
  enum Perfil <<enum>> {
    Administrador
    Rececionista
    Veterinario
    AuxiliarVeterinario
    Cliente
  }

  interface CredenciaisLogin <<interface>> {
    + email : string
    + password : string
  }

  interface UtilizadorAutenticado <<interface>> {
    + id : number
    + nome : string
    + email : string
    + perfil : Perfil
  }

  interface SessaoAutenticada <<interface>> {
    + token : string
    + expiraEm : string
    + utilizador : UtilizadorAutenticado
  }

  class AuthService <<service>> {
    - api : ApiClient
    + login(dados : CredenciaisLogin) : Promise<SessaoAutenticada>
    + logout() : Promise<void>
    + me() : Promise<UtilizadorAutenticado>
  }

  class AuthContext <<context>> {
    + utilizador : UtilizadorAutenticado?
    + token : string?
    + autenticado : boolean
    + login(dados : CredenciaisLogin) : Promise<void>
    + restaurarSessao() : Promise<void>
    + logout() : Promise<void>
    + terminarSessaoLocal() : void
    + carregarUtilizador() : Promise<void>
    + temPerfil(perfis : Perfil[]) : boolean
  }

  class ProtectedRoute <<guard>> {
    + perfisPermitidos : Perfil[]
    + validarAcesso() : boolean
  }
}

package "Layout" {
  class AppLayout <<component>> {
    + utilizador : UtilizadorAutenticado
    + obterItensMenu() : MenuItem[]
    + terminarSessao() : Promise<void>
  }

  interface MenuItem <<interface>> {
    + label : string
    + destino : string
    + perfis : Perfil[]
  }
}

ApiClient ..> ApiProblemDetails : normaliza erros
AuthService --> ApiClient
AuthService ..> CredenciaisLogin
AuthService ..> SessaoAutenticada
AuthService ..> UtilizadorAutenticado
AuthContext --> AuthService
AuthContext --> TokenStorage
AuthContext --> ApiClient : define token
AuthContext o-- UtilizadorAutenticado
TokenStorage ..> SessaoAutenticada
ProtectedRoute --> AuthContext : consulta perfil
ProtectedRoute ..> Perfil
AppLayout --> AuthContext
AppLayout ..> MenuItem
MenuItem ..> Perfil
@enduml
```

## 6.1. Responsabilidades

| Estrutura | Responsabilidade |
|---|---|
| `ApiClient` | Centralizar o acesso HTTP à API, aplicar a URL base, enviar JWT quando existente e normalizar erros. |
| `ApiProblemDetails` | Representar o formato de erro devolvido pela API, incluindo erros de validação por campo. |
| `PagedResult<T>` | Representar listas paginadas devolvidas pela API. |
| `AuthService` | Consumir EP-001, EP-002 e EP-003. |
| `TokenStorage` | Guardar e limpar a sessão Web em `sessionStorage`, para que não persista depois de fechado o separador. |
| `AuthContext` | Manter o estado de autenticação, restaurar a sessão válida e rejeitar perfis que não pertencem ao canal Web. |
| `ProtectedRoute` | Impedir a apresentação de páginas não permitidas ao perfil atual. |
| `AppLayout` | Apresentar navegação e ações adequadas ao perfil autenticado. |
| `MenuItem` | Representar uma opção de navegação e os perfis que a podem visualizar. |

### 6.2. Regras

- O JWT é obtido no login e utilizado nos pedidos protegidos. No Web, a sessão é guardada em `sessionStorage` através de `TokenStorage`; esta é a concretização do armazenamento do token previsto no I023 para este canal.
- Depois de EP-001, `AuthContext` aceita apenas os perfis `Administrador`, `Rececionista`, `Veterinario` e `AuxiliarVeterinario`. Se a API devolver um utilizador com perfil `Cliente`, o token recebido é definido apenas temporariamente para chamar EP-002, é revogado e limpo, e o acesso Web é recusado (RNF003).
- Ao arrancar, `restaurarSessao()` recupera uma sessão existente e confirma-a com EP-003. Uma sessão expirada ou uma resposta `401` é limpa e conduz a W01.
- O frontend não implementa refresh token; após a expiração do JWT, o utilizador volta a autenticar-se.
- A ocultação de uma opção no frontend não substitui a autorização obrigatória do backend.
- Embora a enumeração `Perfil` inclua `Cliente` por corresponder ao contrato da API, `ProtectedRoute` nunca permite páginas Web ao perfil `Cliente`.

---

# 7. Páginas Web — F001, F002 e F003

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty members

package "F001 - Autenticação, Contas e Configuração" {
  class LoginPage <<page>> {
    + email : string
    + password : string
    + erro : string?
    + submeter() : Promise<void>
  }

  class FuncionariosPage <<page>> {
    + pesquisa : string
    + perfil : Perfil?
    + estado : EstadoUtilizador?
    + carregar() : Promise<void>
    + selecionar(id : number) : void
    + bloquear(id : number) : Promise<void>
  }

  class FuncionarioFormPage <<page>> {
    + funcionarioId : number?
    + formulario : FuncionarioFormState
    + guardar() : Promise<void>
    + bloquear() : Promise<void>
  }

  class ClientesPage <<page>> {
    + pesquisa : string
    + carregar() : Promise<void>
    + selecionar(id : number) : void
  }

  class ClienteFormPage <<page>> {
    + clienteId : number?
    + formulario : ClienteFormState
    + carregar() : Promise<void>
    + guardar() : Promise<void>
  }

  class ClinicaConfiguracaoPage <<page>> {
    + formulario : ClinicaConfig
    + carregar() : Promise<void>
    + guardar() : Promise<void>
  }
}

package "F002 - Pacientes" {
  class PacientesPage <<page>> {
    + pesquisa : string
    + pesquisaProprietario : string
    + proprietarios : ClienteResumo[]
    + clienteId : number?
    + especie : string?
    + pesquisarProprietarios() : Promise<void>
    + selecionarProprietario(id : number) : void
    + carregar() : Promise<void>
    + selecionar(id : number) : void
    + novoPaciente() : void
  }

  class PacienteDetalhePage <<page>> {
    + pacienteId : number?
    + paciente : PacienteDetalhe?
    + formulario : PacienteForm
    + modoCriacao : boolean
    + modoLeitura : boolean
    + carregar() : Promise<void>
    + guardar() : Promise<void>
  }
}

package "F003 - Agenda e Apoio" {
  class AgendaPage <<page>> {
    + filtros : AgendaFiltros
    + carregar() : Promise<void>
    + novaMarcacao() : void
    + abrirMarcacao(id : number) : void
  }

  class MarcacaoFormPage <<page>> {
    + consultaId : number?
    + formulario : MarcacaoForm
    + disponibilidade : Disponibilidade?
    + consultarDisponibilidade() : Promise<void>
    + guardar() : Promise<void>
  }

  class MarcacaoDetalhePage <<page>> {
    + consultaId : number
    + marcacao : MarcacaoDetalhe?
    + carregar() : Promise<void>
    + alterarEstado(estado : EstadoConsulta) : Promise<void>
    + registarChegada() : Promise<void>
  }

  class PreparacaoConsultaPage <<page>> {
    + filtros : AgendaFiltros
    + consultas : MarcacaoResumo[]
    + consultaId : number?
    + marcacao : MarcacaoDetalhe?
    + paciente : PacienteDetalhe?
    + textoApoio : string
    + listarConsultas() : Promise<void>
    + selecionarConsulta(id : number) : Promise<void>
    + carregarDetalhe() : Promise<void>
    + guardarApoio() : Promise<void>
  }
}

LoginPage --> AuthService

FuncionariosPage --> UtilizadoresService
FuncionarioFormPage --> UtilizadoresService
ClientesPage --> ClientesService
ClienteFormPage --> ClientesService
ClinicaConfiguracaoPage --> ClinicaService

PacientesPage --> PacientesService
PacientesPage --> ClientesService : pesquisa proprietário
PacienteDetalhePage --> PacientesService

AgendaPage --> AgendaService
MarcacaoFormPage --> AgendaService
MarcacaoDetalhePage --> AgendaService
PreparacaoConsultaPage --> AgendaService
PreparacaoConsultaPage --> PacientesService

FuncionariosPage ..> Perfil
FuncionariosPage ..> EstadoUtilizador
FuncionarioFormPage ..> FuncionarioFormState
ClienteFormPage ..> ClienteFormState
ClinicaConfiguracaoPage ..> ClinicaConfig
PacienteDetalhePage ..> PacienteDetalhe
PacienteDetalhePage ..> PacienteForm
AgendaPage ..> AgendaFiltros
MarcacaoFormPage ..> MarcacaoForm
MarcacaoFormPage ..> Disponibilidade
MarcacaoDetalhePage ..> MarcacaoDetalhe
MarcacaoDetalhePage ..> EstadoConsulta
PreparacaoConsultaPage ..> AgendaFiltros
PreparacaoConsultaPage ..> MarcacaoResumo
PreparacaoConsultaPage ..> MarcacaoDetalhe
PreparacaoConsultaPage ..> PacienteDetalhe
@enduml
```

---

# 8. Páginas Web — F004 e F006

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty members

package "F004 - Registo Clínico" {
  class RegistoClinicoPage <<page>> {
    + animalId : number
    + consultaId : number?
    + registoId : number?
    + registo : RegistoClinico?
    + historico : HistoricoClinicoItem[]
    + modoLeitura : boolean
    + carregarHistorico() : Promise<void>
    + carregarRegisto(id : number) : Promise<void>
    + guardar() : Promise<void>
  }
}

package "F006 - Vacinação e Desparasitação" {
  class ProfilaxiaPage <<page>> {
    + animalId : number
    + boletim : BoletimPaciente?
    + formulario : ProfilaxiaForm
    + carregar() : Promise<void>
    + registar() : Promise<void>
  }
}

RegistoClinicoPage --> RegistosClinicosService
ProfilaxiaPage --> ProfilaxiaService

RegistoClinicoPage ..> RegistoClinico
RegistoClinicoPage ..> HistoricoClinicoItem
ProfilaxiaPage ..> BoletimPaciente
ProfilaxiaPage ..> ProfilaxiaForm
@enduml
```

---

# 9. Serviços de acesso à API

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty members

class ApiClient <<infrastructure>>

class AuthService <<service>> {
  + login(dados : CredenciaisLogin) : Promise<SessaoAutenticada>
  + logout() : Promise<void>
  + me() : Promise<UtilizadorAutenticado>
}

class UtilizadoresService <<service>> {
  + listar(filtros : UtilizadorFiltros) : Promise<PagedResult<FuncionarioResumo>>
  + obter(id : number) : Promise<FuncionarioDetalhe>
  + criar(dados : CriarFuncionarioRequest) : Promise<FuncionarioResumo>
  + atualizar(id : number, dados : AtualizarFuncionarioRequest) : Promise<FuncionarioDetalhe>
  + bloquear(id : number, motivo? : string) : Promise<void>
}

class ClinicaService <<service>> {
  + obter() : Promise<ClinicaConfig>
  + atualizar(dados : ClinicaConfig) : Promise<ClinicaConfig>
}

class ClientesService <<service>> {
  + listar(pesquisa : string?, pagina : number, tamanho : number) : Promise<PagedResult<ClienteResumo>>
  + obter(id : number) : Promise<ClienteDetalhe>
  + criar(dados : CriarClienteRequest) : Promise<ClienteDetalhe>
  + atualizar(id : number, dados : AtualizarClienteRequest) : Promise<ClienteDetalhe>
}

class PacientesService <<service>> {
  + listar(filtros : PacienteFiltros) : Promise<PagedResult<PacienteResumo>>
  + obter(id : number) : Promise<PacienteDetalhe>
  + criar(dados : PacienteForm) : Promise<PacienteDetalhe>
  + atualizar(id : number, dados : PacienteForm) : Promise<PacienteDetalhe>
}

class AgendaService <<service>> {
  - versoes : Map<number, string>
  + listarVeterinarios() : Promise<VeterinarioResumo[]>
  + obterDisponibilidade(id : number, inicio : string, fim? : string) : Promise<Disponibilidade>
  + listarConsultas(filtros : AgendaFiltros) : Promise<PagedResult<MarcacaoResumo>>
  + obterConsulta(id : number) : Promise<MarcacaoDetalhe>
  + criar(dados : MarcacaoForm) : Promise<MarcacaoDetalhe>
  + atualizar(id : number, dados : MarcacaoForm) : Promise<MarcacaoDetalhe>
  + alterarEstado(id : number, dados : EstadoMarcacaoForm) : Promise<void>
  + registarChegada(id : number) : Promise<ChegadaConsulta>
  + registarNotaPreparacao(id : number, texto : string) : Promise<NotaPreparacao>
}

class RegistosClinicosService <<service>> {
  + criar(dados : RegistoClinicoForm) : Promise<RegistoClinico>
  + atualizar(id : number, dados : RegistoClinicoForm) : Promise<RegistoClinico>
  + obter(id : number) : Promise<RegistoClinico>
  + historicoAnimal(animalId : number, inicio? : string, fim? : string) : Promise<HistoricoClinico>
}

class ProfilaxiaService <<service>> {
  + obterBoletim(animalId : number, tipo? : TipoAdministracao) : Promise<BoletimPaciente>
  + registar(animalId : number, dados : ProfilaxiaForm) : Promise<RegistoProfilaxia>
}

AuthService --> ApiClient
UtilizadoresService --> ApiClient
ClinicaService --> ApiClient
ClientesService --> ApiClient
PacientesService --> ApiClient
AgendaService --> ApiClient
RegistosClinicosService --> ApiClient
ProfilaxiaService --> ApiClient
@enduml
```

## 9.1. Endpoints consumidos por serviço

| Serviço | Endpoints previstos |
|---|---|
| `AuthService` | EP-001 `POST /auth/login`; EP-002 `POST /auth/logout`; EP-003 `GET /auth/me` |
| `UtilizadoresService` | EP-004 a EP-008 |
| `ClinicaService` | EP-009 e EP-010 |
| `ClientesService` | EP-011 a EP-014 |
| `PacientesService` | EP-015 a EP-018 |
| `AgendaService` | EP-021 a EP-029 |
| `RegistosClinicosService` | EP-031 a EP-034 |
| `ProfilaxiaService` | EP-042 e EP-043 |

Os endpoints EP-019, EP-020, EP-030 e EP-044 são exclusivos da aplicação móvel do Cliente e não são consumidos pelo Frontend Web. Os serviços consomem, no total, 33 dos 37 endpoints em âmbito.

## 9.2. Alterações concorrentes de marcações (`ETag` e `If-Match`)

A Documentação da API v02 (secção 5.1) propõe que a versão de uma marcação, guardada em `Marcacao.RowVersion`, viaje no cabeçalho `ETag` e volte no cabeçalho `If-Match`. Este modelo prevê o seguinte para o Frontend Web:

- `ApiClient` deixa as respostas acessíveis com os seus cabeçalhos e aceita cabeçalhos de pedido, para o `AgendaService` ler o `ETag` e enviar o `If-Match`.
- `AgendaService` guarda em memória, por identificador de marcação, o último `ETag` recebido em EP-024, EP-025, EP-026, EP-027 e EP-028 (atributo `versoes`).
- `atualizar` (EP-026), `alterarEstado` (EP-027) e `registarChegada` (EP-028) enviam sempre o `If-Match` com a versão guardada. Se a marcação nunca foi lida nesta sessão, a página carrega-a primeiro (EP-024).
- Resposta 412 (a marcação mudou desde a última leitura) ou 409 (duas gravações simultâneas): a página não repete o pedido sozinha, volta a ler a marcação (EP-024), mostra que foi alterada por outra pessoa e deixa o utilizador decidir se repete a operação.
- O servidor tem de expor o `ETag` e aceitar o `If-Match` na política de CORS (Documentação da API v02, secção 5.1). Sem isso, o browser não deixa o JavaScript ler nem enviar estes cabeçalhos.
- Os modelos TypeScript das marcações não ganham campo de versão: a versão fica fora dos DTOs. Se o grupo preferir um campo `versao` (ponto em aberto 5 da API), este modelo e a secção 21.3 mudam em conjunto com a API.

---

# 10. Models e DTOs TypeScript

## 10.1. Enumerações

```plantuml {kroki=true}
@startuml
enum Perfil {
  Administrador
  Rececionista
  Veterinario
  AuxiliarVeterinario
  Cliente
}

enum PerfilInterno {
  Administrador
  Rececionista
  Veterinario
  AuxiliarVeterinario
}

enum EstadoUtilizador {
  Ativo
  Bloqueado
}

enum Sexo {
  Macho
  Femea
  Desconhecido
}

enum EstadoConsulta {
  Pendente
  Confirmada
  Realizada
  Cancelada
}

enum EstadoMarcacaoAtualizavel {
  Confirmada
  Realizada
  Cancelada
}

enum TipoAdministracao {
  Vacina
  Desparasitacao
}

enum DiaSemana {
  Segunda
  Terca
  Quarta
  Quinta
  Sexta
  Sabado
  Domingo
}
@enduml
```

As enumerações `Perfil`, `EstadoUtilizador`, `Sexo`, `EstadoConsulta`, `TipoAdministracao` e `DiaSemana` seguem diretamente os valores definidos na Documentação da API v02. Os valores de `DiaSemana` (`Segunda` a `Domingo`) constam da tabela de enumerações da API v02.

`PerfilInterno` e `EstadoMarcacaoAtualizavel` são restrições de tipagem introduzidas apenas no Frontend Web para impedir, ainda na interface, valores que a própria API declara inválidos: o perfil `Cliente` não pode ser atribuído por EP-006/EP-007 e `Pendente` não é um destino válido em EP-027.

---

## 10.2. Autenticação, funcionários e configuração

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty methods

interface CredenciaisLogin {
  + email : string
  + password : string
}

interface UtilizadorAutenticado {
  + id : number
  + nome : string
  + email : string
  + perfil : Perfil
}

interface SessaoAutenticada {
  + token : string
  + expiraEm : string
  + utilizador : UtilizadorAutenticado
}

interface FuncionarioResumo {
  + id : number
  + nome : string
  + email : string
  + perfil : PerfilInterno
  + estado : EstadoUtilizador
  + podeRegistarApoio : boolean
}

interface FuncionarioDetalhe {
  + id : number
  + nome : string
  + email : string
  + telefone : string?
  + perfil : PerfilInterno
  + estado : EstadoUtilizador
  + podeRegistarApoio : boolean
  + criadoEm : string
}

interface FuncionarioFormState {
  + nome : string
  + email : string
  + telefone : string?
  + perfil : PerfilInterno
  + podeRegistarApoio : boolean
  + passwordInicial : string?
}

interface CriarFuncionarioRequest {
  + nome : string
  + email : string
  + telefone : string?
  + perfil : PerfilInterno
  + podeRegistarApoio : boolean
  + passwordInicial : string
}

interface AtualizarFuncionarioRequest {
  + nome : string
  + email : string
  + telefone : string?
  + perfil : PerfilInterno
  + podeRegistarApoio : boolean
}

interface AtualizarEstadoUtilizadorRequest {
  + estado : "Bloqueado"
  + motivo : string?
}

interface UtilizadorFiltros {
  + perfil : PerfilInterno?
  + estado : EstadoUtilizador?
  + pesquisa : string?
  + pagina : number
  + tamanho : number
}

interface HorarioFuncionamento {
  + diaSemana : DiaSemana
  + abertura : string
  + fecho : string
}

interface ClinicaConfig {
  + nome : string
  + nif : string
  + morada : string
  + telefone : string
  + email : string
  + duracaoConsultaMinutos : number
  + horarioFuncionamento : HorarioFuncionamento[]
}

SessaoAutenticada *-- UtilizadorAutenticado
FuncionarioResumo ..> PerfilInterno
FuncionarioResumo ..> EstadoUtilizador
FuncionarioDetalhe ..> PerfilInterno
FuncionarioDetalhe ..> EstadoUtilizador
FuncionarioFormState ..> PerfilInterno
CriarFuncionarioRequest ..> PerfilInterno
AtualizarFuncionarioRequest ..> PerfilInterno
ClinicaConfig *-- "1..*" HorarioFuncionamento
HorarioFuncionamento ..> DiaSemana
@enduml
```

`FuncionarioFormState` representa o estado visual partilhado do ecrã W03. Os pedidos HTTP mantêm contratos distintos: `CriarFuncionarioRequest` exige `passwordInicial`, enquanto `AtualizarFuncionarioRequest` não envia esse campo. `podeRegistarApoio` só é apresentado como autorização específica quando o perfil selecionado é `AuxiliarVeterinario`. O bloqueio da conta é uma ação separada em EP-008, enviada com `AtualizarEstadoUtilizadorRequest` (o único `estado` aceite é `Bloqueado`; o `motivo` é opcional); não é disponibilizada uma ação de desbloqueio.

`ClinicaConfig.horarioFuncionamento` é uma lista de períodos. O mesmo `diaSemana` pode surgir mais do que uma vez, desde que os períodos não se sobreponham.

---

## 10.3. Clientes e pacientes

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty methods

interface ReferenciaIdNome {
  + id : number
  + nome : string
}

interface ClienteResumo {
  + id : number
  + nome : string
  + nif : string
  + telefone : string
  + email : string
  + numeroAnimais : number
}

interface AnimalClienteResumo {
  + id : number
  + nome : string
  + especie : string
}

interface ClienteDetalhe {
  + id : number
  + nome : string
  + nif : string
  + telefone : string
  + email : string
  + morada : string
  + utilizadorId : number
  + estadoConta : EstadoUtilizador
  + animais : AnimalClienteResumo[]
}

interface ClienteFormState {
  + nome : string
  + nif : string
  + telefone : string
  + email : string
  + morada : string
  + passwordInicial : string?
}

interface CriarClienteRequest {
  + nome : string
  + nif : string
  + telefone : string
  + email : string
  + morada : string
  + passwordInicial : string
}

interface AtualizarClienteRequest {
  + nome : string
  + nif : string
  + telefone : string
  + email : string
  + morada : string
}

interface PacienteResumo {
  + id : number
  + nome : string
  + especie : string
  + raca : string?
  + idadeAnos : number?
  + peso : number?
  + cliente : ReferenciaIdNome
}

interface PacienteDetalhe {
  + id : number
  + nome : string
  + especie : string
  + raca : string?
  + sexo : Sexo
  + dataNascimento : string?
  + idadeAnos : number?
  + peso : number?
  + cliente : ClientePaciente
  + atualizadoEm : string?
}

interface ClientePaciente {
  + id : number
  + nome : string
  + telefone : string
}

interface PacienteForm {
  + nome : string
  + especie : string
  + raca : string?
  + sexo : Sexo
  + dataNascimento : string?
  + peso : number?
  + clienteId : number
}

interface PacienteFiltros {
  + clienteId : number?
  + pesquisa : string?
  + especie : string?
  + pagina : number
  + tamanho : number
}

ClienteDetalhe o-- AnimalClienteResumo : animais
PacienteResumo --> ReferenciaIdNome : cliente
PacienteDetalhe --> ClientePaciente : cliente
PacienteDetalhe ..> Sexo
PacienteForm ..> Sexo
@enduml
```

`ClienteFormState` representa o estado visual partilhado do ecrã W05. Na criação, o frontend converte-o para `CriarClienteRequest`, onde `passwordInicial` é obrigatório; na atualização utiliza `AtualizarClienteRequest`, sem palavra-passe.

---

## 10.4. Agenda e preparação de consulta

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty methods

interface VeterinarioResumo {
  + id : number
  + nome : string
}

interface DiaDisponibilidade {
  + data : string
  + horariosLivres : string[]
}

interface Disponibilidade {
  + veterinarioId : number
  + duracaoConsultaMinutos : number
  + dias : DiaDisponibilidade[]
}

interface AgendaFiltros {
  + dataInicio : string?
  + dataFim : string?
  + veterinarioId : number?
  + animalId : number?
  + estado : EstadoConsulta?
  + pagina : number
  + tamanho : number
}

interface MarcacaoResumo {
  + id : number
  + dataHora : string
  + duracaoMinutos : number
  + estado : EstadoConsulta
  + motivo : string
  + animal : ReferenciaIdNome
  + cliente : ReferenciaIdNome
  + veterinario : VeterinarioResumo
  + chegadaEm : string?
}

interface NotaPreparacao {
  + id : number
  + consultaId : number
  + texto : string
  + autor : string
  + criadaEm : string
}

interface MarcacaoDetalhe {
  + id : number
  + dataHora : string
  + duracaoMinutos : number
  + estado : EstadoConsulta
  + motivo : string
  + animal : ReferenciaIdNome
  + cliente : ReferenciaIdNome
  + veterinario : VeterinarioResumo
  + chegadaEm : string?
  + notasPreparacao : NotaPreparacao[]
}

interface MarcacaoForm {
  + animalId : number
  + veterinarioId : number
  + dataHora : string
  + motivo : string
}

interface EstadoMarcacaoForm {
  + estado : EstadoMarcacaoAtualizavel
  + observacao : string?
}

interface ChegadaConsulta {
  + id : number
  + chegadaEm : string
}

Disponibilidade *-- DiaDisponibilidade
MarcacaoResumo --> ReferenciaIdNome : animal
MarcacaoResumo --> ReferenciaIdNome : cliente
MarcacaoResumo --> VeterinarioResumo
MarcacaoDetalhe --> ReferenciaIdNome : animal
MarcacaoDetalhe --> ReferenciaIdNome : cliente
MarcacaoDetalhe --> VeterinarioResumo
MarcacaoDetalhe *-- NotaPreparacao
AgendaFiltros ..> EstadoConsulta
MarcacaoResumo ..> EstadoConsulta
MarcacaoDetalhe ..> EstadoConsulta
EstadoMarcacaoForm ..> EstadoMarcacaoAtualizavel
@enduml
```

---

## 10.5. Registo clínico

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty methods

interface Tratamento {
  + id : number?
  + descricao : string
  + dataAplicacao : string
}

interface Prescricao {
  + id : number?
  + medicamento : string
  + dosagem : string
  + frequencia : string
  + duracaoDias : number?
}

interface RegistoClinicoForm {
  + consultaId : number?
  + notasClinicas : string?
  + diagnostico : string?
  + tratamentos : Tratamento[]
  + prescricoes : Prescricao[]
}

interface RegistoClinico {
  + id : number
  + consultaId : number
  + animalId : number
  + veterinario : string
  + criadoEm : string
  + notasClinicas : string?
  + diagnostico : string?
  + tratamentos : Tratamento[]
  + prescricoes : Prescricao[]
  + atualizadoEm : string?
  + atualizadoPor : string?
}

interface HistoricoClinicoItem {
  + id : number
  + consultaId : number
  + data : string
  + veterinario : string
  + diagnostico : string?
  + numeroTratamentos : number
  + numeroPrescricoes : number
}

interface HistoricoClinico {
  + animal : ReferenciaIdNome
  + registos : HistoricoClinicoItem[]
}

RegistoClinicoForm *-- Tratamento
RegistoClinicoForm *-- Prescricao
RegistoClinico *-- Tratamento
RegistoClinico *-- Prescricao
HistoricoClinico --> ReferenciaIdNome
HistoricoClinico *-- HistoricoClinicoItem
@enduml
```

Para criação, `consultaId` é obrigatório. Na atualização de um registo existente, a API reutiliza o mesmo formato sem `consultaId`, motivo pelo qual o campo é representado como opcional no modelo partilhado do frontend.

---

## 10.6. Vacinação e desparasitação

```plantuml {kroki=true}
@startuml
skinparam classAttributeIconSize 0
hide empty methods

interface RegistoProfilaxia {
  + id : number
  + tipo : TipoAdministracao
  + produto : string
  + lote : string
  + dataAdministracao : string
  + intervaloDias : number?
  + dataProximaPrevista : string?
  + veterinario : string
}

interface ProximaAdministracao {
  + tipo : TipoAdministracao
  + produto : string
  + dataProximaPrevista : string
}

interface BoletimPaciente {
  + animal : ReferenciaIdNome
  + administracoes : RegistoProfilaxia[]
  + proximas : ProximaAdministracao[]
}

interface ProfilaxiaForm {
  + tipo : TipoAdministracao
  + produto : string
  + lote : string
  + dataAdministracao : string
  + intervaloDias : number?
}

RegistoProfilaxia ..> TipoAdministracao
ProximaAdministracao ..> TipoAdministracao
BoletimPaciente --> ReferenciaIdNome
BoletimPaciente *-- RegistoProfilaxia
BoletimPaciente *-- ProximaAdministracao
ProfilaxiaForm ..> TipoAdministracao
@enduml
```

A data da próxima administração é calculada pelo servidor quando `intervaloDias` é fornecido; o frontend não é a fonte autoritativa desse cálculo.

---

# 11. Relação entre páginas, serviços e protótipo

| Ecrã | Página / estrutura principal | Serviço(s) | UC / RF principais |
|---|---|---|---|
| W01 — Login Web | `LoginPage` | `AuthService` | UC01; RF001, RF002, RF004, RF005 |
| W02 — Funcionários | `FuncionariosPage` | `UtilizadoresService` | UC03; RF007–RF009 |
| W03 — Criar/Editar Funcionário | `FuncionarioFormPage` | `UtilizadoresService` | UC03; RF006–RF009 |
| W04 — Clientes | `ClientesPage` | `ClientesService` | UC04; RF010, RF011 |
| W05 — Detalhe/Criar/Editar Cliente | `ClienteFormPage` | `ClientesService` | UC04; RF010, RF011 |
| W06 — Clínica, Configurações e Horários | `ClinicaConfiguracaoPage` | `ClinicaService` | UC05/UC09; RF012, RF013, RF019 |
| W07 — Pacientes | `PacientesPage` | `PacientesService`, `ClientesService` (pesquisa de proprietário) | UC06/UC07; RF014, RF017 |
| W08 — Ficha do Paciente | `PacienteDetalhePage` | `PacientesService` | UC06/UC07/UC10; RF014–RF016, RF026 |
| W09 — Agenda | `AgendaPage` | `AgendaService` | UC09; RF020, RF024 |
| W10 — Criar/Editar Marcação | `MarcacaoFormPage` | `AgendaService` | UC09; RF020–RF023 |
| W11 — Detalhe da Marcação | `MarcacaoDetalhePage` | `AgendaService` | UC09; RF023–RF025 |
| W12 — Preparação e Apoio | `PreparacaoConsultaPage` | `AgendaService`, `PacientesService` | UC10; RF026, RF027; inclui listagem/seleção das consultas disponíveis |
| W13 — Registo e Histórico Clínico | `RegistoClinicoPage` | `RegistosClinicosService` | UC12/UC13; RF030–RF036 |
| W16 — Vacinação e Desparasitação | `ProfilaxiaPage` | `ProfilaxiaService` | UC16; RF043–RF046 |

Os ecrãs **W14, W15, W17 e W18** (contas e pagamentos; internamento) estão **fora do âmbito** (Declaração de Âmbito v05) e a sua numeração fica reservada. Não têm página, serviço nem modelo neste documento.

---

# 12. Matriz de acesso das páginas Web

| Página | Administrador | Rececionista | Veterinário | Auxiliar Veterinário |
|---|---:|---:|---:|---:|
| W01 Login | Sim | Sim | Sim | Sim |
| W02 Funcionários | Leitura/Escrita | — | — | — |
| W03 Criar/Editar Funcionário | Escrita | — | — | — |
| W04 Clientes | Leitura/Escrita | Leitura/Escrita | — | — |
| W05 Detalhe/Criar/Editar Cliente | Leitura/Escrita | Leitura/Escrita | — | — |
| W06 Clínica/Configuração/Horários | Leitura/Escrita | — | — | — |
| W07 Pacientes | — | Leitura/Escrita | Leitura/Escrita | — |
| W08 Ficha do Paciente | — | Leitura/Escrita | Leitura/Escrita | Leitura |
| W09 Agenda | — | Agenda da clínica | Consultas próprias | — |
| W10 Criar/Editar Marcação | — | Escrita | — | — |
| W11 Detalhe da Marcação | — | Leitura/Escrita administrativa | Leitura das próprias | — |
| W12 Preparação/Apoio | — | — | — | Leitura + apoio não clínico autorizado |
| W13 Registo Clínico | — | — | Leitura/Escrita | Leitura autorizada |
| W16 Vacinação/Desparasitação | — | — | Leitura/Escrita | — |

A matriz descreve o comportamento esperado da interface. A API continua a ser responsável por rejeitar operações não autorizadas.

**Clarificações de navegação:**

- o Veterinário não entra em W04. A pesquisa de proprietário necessária ao UC07 é efetuada dentro de W07 através de `ClientesService` em modo de consulta;
- o Auxiliar não necessita de entrar em W09. O W12 apresenta a própria lista de consultas disponíveis, permite selecionar uma consulta e só depois mostra a informação de preparação/apoio;
- os ecrãs W14, W15, W17 e W18 não constam da matriz por estarem fora do âmbito.

---

# 13. Regras de comportamento por perfil

## 13.1. Administrador

A interface do Administrador disponibiliza:

- gestão de contas de funcionários;
- atribuição de perfis;
- bloqueio de contas;
- gestão de contas de Clientes;
- dados gerais e configurações da clínica;
- horários de funcionamento.

Não apresenta operações clínicas nem vacinação.

## 13.2. Rececionista

A interface do Rececionista disponibiliza:

- gestão administrativa de Clientes;
- registo, consulta e atualização administrativa de pacientes;
- consulta da agenda;
- consulta de disponibilidade;
- criação e alteração de marcações;
- atualização do estado da marcação;
- registo de chegada.

Não apresenta operações de escrita clínica.

## 13.3. Veterinário

A interface do Veterinário disponibiliza:

- pesquisa e gestão administrativa de pacientes prevista nos requisitos;
- pesquisa/seleção de proprietário dentro de W07 para localizar pacientes, sem acesso à área W04 de gestão de Clientes;
- consulta apenas das consultas que lhe estão atribuídas;
- consulta de detalhe das próprias marcações;
- registo e atualização clínica;
- histórico clínico;
- vacinação e desparasitação.

Não apresenta administração de utilizadores nem configurações da clínica.

## 13.4. Auxiliar Veterinário

A interface do Auxiliar disponibiliza:

- no próprio W12, listagem e seleção das consultas disponíveis abrangidas pelas suas permissões;
- consulta da ficha administrativa necessária à preparação;
- consulta da informação da consulta necessária às suas funções;
- registo de informação não clínica de apoio quando autorizado;
- consulta de informação clínica autorizada.

Não disponibiliza:

- criação/alteração clínica;
- gestão de marcações.

---

# 14. Validação e tratamento de erros

O Frontend Web pode efetuar validações locais para melhorar a experiência de utilização, incluindo:

- campos obrigatórios;
- formatos básicos;
- datas;
- valores numéricos;
- seleção de opções válidas.

Estas validações não substituem a validação autoritativa da API.

O `ApiClient` deve permitir às páginas distinguir pelo menos:

- `400` — pedido inválido e erros de validação;
- `401` — autenticação ausente, inválida, expirada ou revogada;
- `403` — perfil sem permissão;
- `404` — recurso inexistente;
- `409` — conflito (dados repetidos ou duas gravações simultâneas da mesma marcação);
- `412` — a marcação mudou desde a última leitura (`If-Match` desatualizado; secção 9.2);
- `422` — regra de negócio violada;
- `500` — erro inesperado.

Quando a API devolver o campo `errors`, os erros devem poder ser associados aos respetivos campos do formulário.

---

# 15. Relações estruturais principais

O modelo segue as relações abaixo:

```text
Página React
    ↓ utiliza
Service
    ↓ utiliza
ApiClient
    ↓ HTTP/JSON
ARC-API

Página React
    ↓ apresenta / edita
DTOs / Models TypeScript

ProtectedRoute
    ↓ consulta
AuthContext
    ↓ utiliza
AuthService
    ↓ utiliza
ApiClient
```

Não existe relação direta:

```text
Frontend Web → Base de Dados
Frontend Web → Entity Framework Core
Frontend Web → entidades persistentes internas do backend
```

Os DTOs do frontend representam exclusivamente o contrato necessário à interface.

---

# 16. Rastreabilidade com ARC-WEB e requisitos não funcionais

| Requisito / componente | Tratamento no modelo |
|---|---|
| ARC-WEB | Páginas, serviços, models, autenticação e infraestrutura definidos neste documento. |
| RNF001 — Controlo de acesso | `AuthContext`, `ProtectedRoute`, menus por perfil e permissões de ações; backend continua autoritativo. |
| RNF003 — Separação dos canais | O modelo contém apenas perfis internos; Cliente não possui páginas Web. |
| RNF004 — Validação dos dados de entrada | Formulários podem validar localmente; `ApiProblemDetails.errors` apresenta validações devolvidas pelo backend. |
| RNF005 — Persistência dos dados confirmados | O frontend envia as operações para a API e atualiza o estado visual após confirmação do backend. |
| RNF006 — Dependência de ligação à rede | Todas as funcionalidades dependem do `ApiClient`; não existe modo offline. |

RNF002 — Isolamento dos dados de Clientes é principalmente relevante para a aplicação móvel e para as regras do backend. O Frontend Web continua sujeito às permissões e aos dados devolvidos pela API.

---

# 17. Elementos deliberadamente fora do modelo

Este modelo não introduz:

- registo autónomo de Cliente pelo próprio;
- funcionalidades mobile;
- criação, alteração ou cancelamento de marcações pelo Cliente;
- edição de animais pelo Cliente;
- pagamentos eletrónicos;
- faturação certificada;
- notificações SMS/e-mail;
- calendários externos;
- autenticação Google/Facebook;
- autenticação multifator;
- refresh token;
- geração automática de prescrições em PDF;
- integração com SIAC ou bases de saúde animal;
- leitura de QR/códigos de barras;
- integração com equipamentos clínicos;
- gestão de contas e pagamentos (F005) e gestão de internamento (F007), fora do âmbito pela Declaração de Âmbito v05;
- acompanhamento de internamentos pelo Cliente;
- modo offline;
- acesso direto à base de dados.

---

# 18. Correspondência com a API Web

A Documentação da API v02 identifica como utilizados pelo Frontend Web:

- EP-001 a EP-018;
- EP-021 a EP-029;
- EP-031 a EP-034;
- EP-042 e EP-043.

São 33 endpoints. O modelo de serviços desta versão cobre exatamente estes grupos.

Não são modelados como operações do Frontend Web:

- EP-019;
- EP-020;
- EP-030;
- EP-044;

por serem operações `/me/...` destinadas à aplicação móvel do Cliente. Os 33 endpoints do web e os 4 do mobile somam os 37 endpoints em âmbito.

---

# 19. Critérios de implementação derivados do modelo

Para manter coerência com este artefacto durante a implementação:

1. páginas não devem efetuar pedidos HTTP diretamente; devem utilizar o respetivo serviço;
2. serviços devem utilizar `ApiClient`;
3. páginas protegidas devem declarar os perfis permitidos através da camada de proteção/navegação;
4. ações de escrita devem ser ocultadas ou desativadas quando o perfil apenas possui leitura;
5. o frontend não deve reproduzir como definitivas regras de negócio pertencentes ao backend;
6. os nomes e valores das enumerações devem permanecer compatíveis com a API;
7. os DTOs podem ser refinados durante a implementação apenas se o contrato da API também for atualizado de forma controlada;
8. os ecrãs W01–W13 e W16 devem continuar identificáveis na estrutura implementada, ainda que a composição visual evolua;
9. a pesquisa de proprietários do Veterinário deve ocorrer no contexto de W07, sem conceder acesso funcional à gestão W04;
10. W12 deve disponibilizar ao Auxiliar um ponto de entrada autónomo para listar e selecionar consultas disponíveis antes de carregar o detalhe;
11. W08 deve funcionar em modo de criação quando não recebe `pacienteId` e em modo de consulta/edição quando recebe um identificador, respeitando o perfil atual;
12. W03 deve permitir ao Administrador definir `podeRegistarApoio` para Auxiliares e apenas bloquear contas, conforme o contrato da API;
13. as gravações de marcações (EP-026, EP-027 e EP-028) devem enviar `If-Match` com a versão lida e tratar 412 e 409 como descrito na secção 9.2.

---

# 20. Diagrama global simplificado

```plantuml {kroki=true}
@startuml
left to right direction
skinparam componentStyle rectangle

actor Administrador as Admin
actor Rececionista as Rec
actor Veterinario as Vet
actor "Auxiliar Veterinario" as Aux

package "ARC-WEB — React 19.3 + TypeScript 6.0" {
  component "AuthContext\nProtectedRoute\nTokenStorage\nAppLayout" as CORE

  package "Pages W01-W13 e W16" {
    component "F001\nLogin / Funcionarios /\nClientes / Clinica" as P1
    component "F002\nPacientes" as P2
    component "F003\nAgenda / Marcacoes /\nApoio" as P3
    component "F004\nRegisto Clinico" as P4
    component "F006\nProfilaxia" as P6
  }

  package "Services" {
    component "AuthService" as S0
    component "UtilizadoresService\nClinicaService\nClientesService" as S1
    component "PacientesService" as S2
    component "AgendaService" as S3
    component "RegistosClinicosService" as S4
    component "ProfilaxiaService" as S6
  }

  component "ApiClient" as HTTP
  component "Models / DTOs" as DTO
}

component "ARC-API\nASP.NET Core Web API" as API

Admin --> CORE
Rec --> CORE
Vet --> CORE
Aux --> CORE

CORE --> P1
CORE --> P2
CORE --> P3
CORE --> P4
CORE --> P6

P1 --> S0
P1 --> S1
P2 --> S2
P3 --> S3
P4 --> S4
P6 --> S6

P1 ..> DTO
P2 ..> DTO
P3 ..> DTO
P4 ..> DTO
P6 ..> DTO

S0 --> HTTP
S1 --> HTTP
S2 --> HTTP
S3 --> HTTP
S4 --> HTTP
S6 --> HTTP

HTTP --> API : HTTP(S) / JSON / JWT
@enduml
```


---

# 21. Correspondência com os DTOs da API v02

Os modelos TypeScript do frontend web têm nomes orientados à interface (por exemplo, `FuncionarioResumo`). Os DTOs da API v02 (`m3-documentacao-api-v02.md` e `m3-modelo-classes-backend-v02.md`, secção 3) têm os nomes do contrato (por exemplo, `UtilizadorListItemDto`). A tabela seguinte liga cada operação dos serviços ao endpoint e aos DTOs de pedido e de resposta, para que o contrato se mantenha verificável. Os campos de cada modelo TypeScript coincidem com os do DTO correspondente, com os nomes em `camelCase` do JSON. Há duas exceções deliberadas: `ReferenciaIdNome` só usa `id` e `nome`, sem os campos opcionais `especie` e `telefone` de `AnimalRefDto` e `ClienteRefDto` (o telefone do proprietário vem em `ClientePaciente`, na ficha do paciente, EP-016); e `UtilizadorAutenticado` não usa `clienteId`, que só existe para o perfil Cliente.

## 21.1. F001 — Autenticação, funcionários, configuração e Clientes

| Serviço e operação | Endpoint | Pedido (modelo TypeScript → DTO da API) | Resposta (modelo TypeScript → DTO da API) |
|---|---|---|---|
| `AuthService.login` | EP-001 | `CredenciaisLogin` → `LoginRequestDto` | `SessaoAutenticada` → `LoginResponseDto` (inclui `UtilizadorResumoDto`) |
| `AuthService.logout` | EP-002 | — | — (204) |
| `AuthService.me` | EP-003 | — | `UtilizadorAutenticado` → `UtilizadorAutenticadoDto` (o campo `clienteId` só existe para o perfil Cliente e não é usado no web) |
| `UtilizadoresService.listar` | EP-004 | `UtilizadorFiltros` → `UtilizadorFiltroQuery` | `PagedResult<FuncionarioResumo>` → `PagedResultDto<UtilizadorListItemDto>` |
| `UtilizadoresService.obter` | EP-005 | — | `FuncionarioDetalhe` → `UtilizadorDto` |
| `UtilizadoresService.criar` | EP-006 | `CriarFuncionarioRequest` → `CreateUtilizadorDto` | `FuncionarioResumo` → `UtilizadorListItemDto` (201) |
| `UtilizadoresService.atualizar` | EP-007 | `AtualizarFuncionarioRequest` → `UpdateUtilizadorDto` | `FuncionarioDetalhe` → `UtilizadorDto` |
| `UtilizadoresService.bloquear` | EP-008 | `AtualizarEstadoUtilizadorRequest` → `UpdateEstadoUtilizadorDto` | — (204) |
| `ClinicaService.obter` | EP-009 | — | `ClinicaConfig` → `ClinicaDto` (`HorarioFuncionamento` → `PeriodoHorarioDto`) |
| `ClinicaService.atualizar` | EP-010 | `ClinicaConfig` → `UpdateClinicaDto` | `ClinicaConfig` → `ClinicaDto` |
| `ClientesService.listar` | EP-011 | parâmetros `pesquisa`, `pagina`, `tamanho` → `ClienteFiltroQuery` | `PagedResult<ClienteResumo>` → `PagedResultDto<ClienteListItemDto>` |
| `ClientesService.obter` | EP-012 | — | `ClienteDetalhe` → `ClienteDto` (`AnimalClienteResumo` → `AnimalRefDto`) |
| `ClientesService.criar` | EP-013 | `CriarClienteRequest` → `CreateClienteDto` | `ClienteDetalhe` → `ClienteDto` (201) |
| `ClientesService.atualizar` | EP-014 | `AtualizarClienteRequest` → `UpdateClienteDto` | `ClienteDetalhe` → `ClienteDto` |

## 21.2. F002 — Pacientes

| Serviço e operação | Endpoint | Pedido (modelo TypeScript → DTO da API) | Resposta (modelo TypeScript → DTO da API) |
|---|---|---|---|
| `PacientesService.listar` | EP-015 | `PacienteFiltros` → `AnimalFiltroQuery` | `PagedResult<PacienteResumo>` → `PagedResultDto<AnimalListItemDto>` (`ReferenciaIdNome` → `ClienteRefDto`) |
| `PacientesService.obter` | EP-016 | — | `PacienteDetalhe` → `AnimalDto` (`ClientePaciente` → `ClienteRefDto`, com `telefone`) |
| `PacientesService.criar` | EP-017 | `PacienteForm` → `CreateAnimalDto` | `PacienteDetalhe` → `AnimalDto` (201) |
| `PacientesService.atualizar` | EP-018 | `PacienteForm` → `UpdateAnimalDto` | `PacienteDetalhe` → `AnimalDto` |

## 21.3. F003 — Agenda e apoio à consulta

| Serviço e operação | Endpoint | Pedido (modelo TypeScript → DTO da API) | Resposta (modelo TypeScript → DTO da API) |
|---|---|---|---|
| `AgendaService.listarVeterinarios` | EP-021 | — | `VeterinarioResumo[]` → `List<VeterinarioRefDto>` |
| `AgendaService.obterDisponibilidade` | EP-022 | parâmetros `inicio` e `fim` → `DisponibilidadeQuery` | `Disponibilidade` → `DisponibilidadeDto` (`DiaDisponibilidade` → `DiaDisponivelDto`) |
| `AgendaService.listarConsultas` | EP-023 | `AgendaFiltros` → `ConsultaFiltroQuery` | `PagedResult<MarcacaoResumo>` → `PagedResultDto<ConsultaListItemDto>` |
| `AgendaService.obterConsulta` | EP-024 | — | `MarcacaoDetalhe` → `ConsultaDto` (`NotaPreparacao` → `NotaPreparacaoDto`) |
| `AgendaService.criar` | EP-025 | `MarcacaoForm` → `CreateConsultaDto` | `MarcacaoDetalhe` → `ConsultaDto` (201) |
| `AgendaService.atualizar` | EP-026 | `MarcacaoForm` → `UpdateConsultaDto` | `MarcacaoDetalhe` → `ConsultaDto` |
| `AgendaService.alterarEstado` | EP-027 | `EstadoMarcacaoForm` → `UpdateEstadoConsultaDto` | — (204) |
| `AgendaService.registarChegada` | EP-028 | — | `ChegadaConsulta` → `ChegadaDto` |
| `AgendaService.registarNotaPreparacao` | EP-029 | parâmetro `texto` → `CreateNotaPreparacaoDto` | `NotaPreparacao` → `NotaPreparacaoDto` (201) |

`MarcacaoForm` serve a criação (inclui `animalId`) e a alteração; em EP-026 o `animalId` não é enviado (o DTO `UpdateConsultaDto` só tem `veterinarioId`, `dataHora` e `motivo`).

O `ETag` das respostas de EP-024 a EP-028 e o `If-Match` dos pedidos de EP-026, EP-027 e EP-028 viajam nos cabeçalhos e não aparecem nos DTOs (secção 9.2).

## 21.4. F004 e F006 — Registo clínico e boletim de vacinas

| Serviço e operação | Endpoint | Pedido (modelo TypeScript → DTO da API) | Resposta (modelo TypeScript → DTO da API) |
|---|---|---|---|
| `RegistosClinicosService.criar` | EP-031 | `RegistoClinicoForm` → `CreateRegistoClinicoDto` (`Tratamento` → `CreateTratamentoDto`, `Prescricao` → `CreatePrescricaoDto`) | `RegistoClinico` → `RegistoClinicoDto` (201) |
| `RegistosClinicosService.atualizar` | EP-032 | `RegistoClinicoForm` sem `consultaId` → `UpdateRegistoClinicoDto` | `RegistoClinico` → `RegistoClinicoDto` |
| `RegistosClinicosService.obter` | EP-033 | — | `RegistoClinico` → `RegistoClinicoDto` (`Tratamento` → `TratamentoDto`, `Prescricao` → `PrescricaoDto`) |
| `RegistosClinicosService.historicoAnimal` | EP-034 | parâmetros `inicio` e `fim` → `HistoricoClinicoQuery` | `HistoricoClinico` → `HistoricoClinicoDto` (`HistoricoClinicoItem` → `RegistoClinicoResumoDto`) |
| `ProfilaxiaService.obterBoletim` | EP-042 | parâmetro `tipo` → `BoletimQuery` | `BoletimPaciente` → `BoletimDto` (`RegistoProfilaxia` → `AdministracaoDto`, `ProximaAdministracao` → `ProximaAdministracaoDto`) |
| `ProfilaxiaService.registar` | EP-043 | `ProfilaxiaForm` → `CreateAdministracaoDto` | `RegistoProfilaxia` → `AdministracaoDto` (201) |

## 21.5. Observações

- As respostas de EP-021, EP-023, EP-024 usam o Veterinário como objeto `{ id, nome }` (`VeterinarioResumo` → `VeterinarioRefDto`); nas respostas de EP-031 a EP-034, EP-042 e EP-043 o Veterinário é um texto (`veterinario : string`), como no contrato da API. A uniformização é um ponto em aberto da API v02 (secção 13 da API e ponto 2 do Modelo de Classes do Backend v02).
- Os requisitos RF053 a RF056, referidos no backlog em preparação, ainda não existem na Especificação de Requisitos nem na API v02. Quando forem aprovados, este modelo terá de acrescentar, por exemplo, `ClienteDetalhe.utilizadorId` opcional, os horários regulares por Veterinário e a atualização de uma administração.

---

## 22. Histórico de versões

| Versão | Autores |
|---|---|
| v01 | Gonçalo Dinis Magalhães Teixeira |
| v02 | Por atribuir (ver «Issue e papéis» no cabeçalho). Rascunho preparado com apoio de IA, a rever pelo Executor. |

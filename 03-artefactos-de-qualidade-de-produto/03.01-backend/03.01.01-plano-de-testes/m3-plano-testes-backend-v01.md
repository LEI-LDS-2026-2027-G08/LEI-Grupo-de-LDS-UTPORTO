# Plano de Testes do Backend — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Plano de Testes do Backend |
| Ficheiro | `m3-plano-testes-backend-v01.md` |
| Versão | v01 (rascunho do Executor, por rever) |
| Milestone de Entrega | m3 |
| Issue | I027 — Elaborar o Plano de Testes do Backend |
| Sprint | S03 |
| Executor | Ivan Cristiano Silva Mendes |
| Revisor | André Filipe Moreira Mesquita |
| Auditor | Gonçalo Magalhães Ribeiro |
| Pasta de destino | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.01-plano-de-testes` |
| Fontes | `m2-especificacao-requisitos-v01.md` (I012); `m2-especificacoes-casos-uso-v01.md` (I015); `m2-documentacao-api-v01.md` (I018); `m2-documentacao-arquitetura-v01.md` (I019); `m1-plano-qualidade-processo-v01.pdf` (I006); Regulamento de Funcionamento da UC v19 (secções 17 e 19); código do backend em `src/backend` (PR #150, I058) |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-10-16 | Ivan Cristiano Silva Mendes | Versão inicial do Plano de Testes do Backend para a m3: âmbito, níveis de teste, ferramentas, ambiente e dados, critérios de entrada e saída, limiares de cobertura, gestão de defeitos, responsabilidades e rastreabilidade. |

---

## Índice

1. [Introdução](#1-introdução)
2. [Documentos de referência](#2-documentos-de-referência)
3. [Âmbito](#3-âmbito)
4. [Estratégia e níveis de teste](#4-estratégia-e-níveis-de-teste)
5. [Ferramentas e versões](#5-ferramentas-e-versões)
6. [Ambiente e dados de teste](#6-ambiente-e-dados-de-teste)
7. [Execução dos testes: comandos locais e na pipeline](#7-execução-dos-testes-comandos-locais-e-na-pipeline)
8. [Critérios de entrada e de saída](#8-critérios-de-entrada-e-de-saída)
9. [Limiares de cobertura e justificação](#9-limiares-de-cobertura-e-justificação)
10. [Condições de teste prioritárias por funcionalidade](#10-condições-de-teste-prioritárias-por-funcionalidade)
11. [Collection Postman](#11-collection-postman)
12. [Gestão de defeitos](#12-gestão-de-defeitos)
13. [Responsabilidades e calendário](#13-responsabilidades-e-calendário)
14. [Riscos](#14-riscos)
15. [Rastreabilidade](#15-rastreabilidade)
16. [Coerência com o Plano de Qualidade (I006)](#16-coerência-com-o-plano-de-qualidade-i006)
17. [Decisões tomadas e pontos a ratificar](#17-decisões-tomadas-e-pontos-a-ratificar)

---

## 1. Introdução

Este documento define **o que será testado, como e com que critérios** no backend do sistema de Gestão de Clínica Veterinária (ASP.NET Core 8, Entity Framework Core 8, Microsoft SQL Server), antes da escrita dos testes. Serve de base à Especificação dos Testes Unitários (I028), à Especificação dos Testes de Integração (I030), à infraestrutura de testes (I091), à configuração da análise estática e do quality gate (I101) e à pipeline CI/CD (I102, I103).

Cumpre o conteúdo exigido pela secção 19 do Regulamento de Funcionamento da UC para este artefacto: âmbito, estratégia, ambiente, dados, responsabilidades e critérios de entrada e de saída dos testes unitários e de integração do backend, e identificação da utilização da Collection Postman para verificar todas as funcionalidades da API.

## 2. Documentos de referência

| Documento | Utilização neste plano |
|---|---|
| Especificação de Requisitos (`m2-especificacao-requisitos-v01.md`) | RF001–RF052 e RNF001–RNF006 |
| Especificação dos Casos de Uso (`m2-especificacoes-casos-uso-v01.md`) | UC01–UC18, fluxos alternativos a cobrir |
| Documentação da API (`m2-documentacao-api-v01.md`) | 51 endpoints (EP-001 a EP-051), perfis, códigos HTTP, formato de erros |
| Documentação da Arquitetura (`m2-documentacao-arquitetura-v01.md`) | Camadas do backend (API, Aplicação, Domínio, Infraestrutura) e tecnologias |
| Plano de Qualidade do Processo (`m1-plano-qualidade-processo-v01.pdf`) | Quality gates QG-01 a QG-04, métrica MQ-02, regras de qualidade a verificar por funcionalidade (secção 6.2) e papéis |
| Regulamento de Funcionamento da UC v19 (secções 17 e 19) | Artefactos obrigatórios de m3 e conteúdo do plano |
| Backend (`src/backend`) | Estrutura da solução e dos projetos de teste (`UnitTests`, `IntegrationTests`) |

## 3. Âmbito

### 3.1. Dentro do âmbito

- Os **51 endpoints** da API (EP-001 a EP-051), agrupados nas funcionalidades F001 a F007.
- Os requisitos funcionais **RF001–RF052** e os requisitos não funcionais **RNF001–RNF006** aplicáveis ao backend.
- As camadas **Domínio** (`ClinicaVeterinaria.Domain`) e **Aplicação/Serviços** (`ClinicaVeterinaria.Application`) nos testes unitários.
- A API (`ClinicaVeterinaria.Api`) com a camada de infraestrutura e a base de dados nos testes de integração.
- Autenticação JWT, autorização por perfil (5 perfis) e por propriedade, validação de dados de entrada e formato de erros (Problem Details).
- Persistência dos dados confirmados em SQL Server.

### 3.2. Fora do âmbito

| Elemento | Justificação |
|---|---|
| Frontend web (React) e frontend mobile (Flutter) | Têm planos de testes próprios na m4 (I043 e I047). |
| Testes de desempenho, carga e segurança ofensiva (penetration testing) | Não fazem parte dos requisitos nem das obrigações da UC. |
| RNF006 — dependência de ligação à rede | Não é comportamento testável no backend; é uma característica do modelo cliente-servidor. |
| Criação e edição de alojamentos | Fora do âmbito funcional; são carregados por script de seed (pressuposto da API). |
| Pagamento eletrónico, emissão fiscal, comunicações a clientes | Fora do âmbito da Declaração de Âmbito v03. |

## 4. Estratégia e níveis de teste

A estratégia é **baseada em risco e em rastreabilidade**: todo o endpoint e requisito tem pelo menos um teste de integração e uma verificação na Collection Postman; as regras de negócio, as transições de estado e as autorizações têm também testes unitários.

| Nível | Objetivo | O que cobre | Ferramenta | Artefactos de evidência (CCA) |
|---|---|---|---|---|
| **T1 — Testes unitários** | Verificar regras de negócio e validações isoladas, sem base de dados | Serviços da camada Aplicação e entidades do Domínio: transições de estado, cálculos, validações, regras de autorização por propriedade | xUnit, Moq | OF-M3-010 (especificação), OF-M3-011 (resultados) |
| **T2 — Testes de integração** | Verificar a API de ponta a ponta contra uma base de dados real | Pedidos HTTP reais ao `WebApplicationFactory`, autenticação JWT, autorização por perfil, validação, persistência e códigos de resposta, para os 51 endpoints | xUnit, `WebApplicationFactory`, Testcontainers (SQL Server), Respawn | OF-M3-012 (especificação), OF-M3-013 (resultados) |
| **T3 — Collection Postman** | Permitir testar todas as funcionalidades da API, manualmente ou por linha de comandos | Um pedido por endpoint, organizado por funcionalidade, com variáveis, dados de exemplo e verificações de resposta (secção 11) | Postman, Newman | OF-M3-015 (`m3-collection-postman-v01.json`) |
| **T4 — Análise estática** | Detetar defeitos e violações de estilo sem executar o código | Todos os projetos do backend | Analisadores Roslyn / .NET (`dotnet build` com `TreatWarningsAsErrors` nas regras acordadas) | OF-M3-016 (relatório) |
| **T5 — Execução na pipeline** | Executar automaticamente T1, T2 e T4 e aplicar os quality gates | GitHub Actions (`backend-ci.yml`) | GitHub Actions | OF-M3-004, OF-M3-005, OF-M3-006 |

Complementarmente, a **cobertura** (OF-M3-014) é medida sobre T1 e T2 em conjunto.

### 4.1. Convenção de identificadores

- Testes unitários: `UT-<módulo>-<nnn>` (por exemplo `UT-F003-004`).
- Testes de integração: `IT-<endpoint>-<nn>` (por exemplo `IT-EP027-03`).
- Pedidos Postman: `PM-<funcionalidade>-<endpoint>` (por exemplo `PM-F003-EP027`).
- Condições de teste deste plano: `CT-<funcionalidade>-<nn>` (secção 10). As Especificações I028 e I030 devem referenciar estes identificadores.

### 4.2. Tipos de verificação aplicados a cada endpoint

Para cada endpoint, o teste de integração verifica, no mínimo:

1. **Caminho feliz** com o perfil autorizado e o código de resposta documentado (200, 201 ou 204) e o corpo esperado.
2. **Cada código de erro documentado** na API (400, 404, 409, 422), com dados que provocam exatamente esse erro.
3. **Pedido sem token ou com token inválido/expirado/revogado** — 401.
4. **Cada perfil não autorizado** — 403 (matriz de permissões, secção 10.8).
5. **Persistência**: nas operações de escrita, o dado fica disponível no endpoint de consulta correspondente (RNF005); nos erros, não há alteração persistida (secção 11 da Arquitetura).

## 5. Ferramentas e versões

| Ferramenta | Versão prevista no Plano de Qualidade | Versão atual em `src/backend` (I058) | Utilização |
|---|---|---|---|
| .NET SDK | 8.0 | 8.0.100 (`global.json`, `rollForward: latestFeature`) | Compilação e execução dos testes |
| xUnit | 2.6 | 2.5.3 | Framework de testes |
| Microsoft.NET.Test.Sdk | — | 17.8.0 | Execução dos testes |
| Moq | 4.20 | **ainda não instalado** | Isolamento de dependências nos testes unitários |
| coverlet.collector | 6.0 | 6.0.0 | Recolha de cobertura |
| ReportGenerator | 5.2 | **ainda não instalado** (ferramenta .NET) | Relatório HTML/Cobertura |
| Microsoft.AspNetCore.Mvc.Testing | — | **ainda não instalado** | `WebApplicationFactory` |
| Testcontainers para .NET (módulo MsSql) | — | **ainda não instalado** | Base de dados SQL Server isolada por execução |
| Respawn | — | **ainda não instalado** | Reposição do estado da base de dados entre testes |
| Newman | — | — | Execução da Collection Postman por linha de comandos |
| Analisadores Roslyn / .NET | — | integrados no SDK | Análise estática |

A divergência entre as versões do xUnit (2.6 no Plano de Qualidade e 2.5.3 no código atual) é tratada na decisão D-02 (secção 17).

## 6. Ambiente e dados de teste

### 6.1. Ambientes

| Ambiente | Utilização | Base de dados |
|---|---|---|
| Local (estudante) | Desenvolvimento e execução dos testes antes de pedir revisão | Contentor SQL Server criado pelo Testcontainers (requer Docker) |
| Pipeline (GitHub Actions) | Execução automática em cada Pull Request e push para `develop` e `main` | Contentor SQL Server criado pelo Testcontainers no próprio runner |

Nenhum teste automático liga à base de dados de desenvolvimento partilhada ou ao ambiente público de avaliação.

### 6.2. Base de dados isolada por execução

- Cada execução dos testes de integração cria **um contentor SQL Server novo** (imagem de SQL Server 2025, alinhada com a Arquitetura) e aplica o esquema da base de dados `clinica-veterinaria`.
- Entre testes, o **Respawn** repõe a base de dados ao estado inicial, preservando apenas os dados de referência (seed de alojamentos).
- Os testes não dependem da ordem de execução e podem ser executados individualmente.

### 6.3. Dados de teste

| Dados | Origem | Regra |
|---|---|---|
| Utilizadores por perfil | Criados pela fixture de testes: um Administrador, um Rececionista, dois Veterinários, um Auxiliar Veterinário (com e sem autorização de registo de apoio) e dois Clientes | Palavras-passe e e-mails fictícios, claramente identificados como de teste |
| Clientes, animais, marcações, registos clínicos, contas, vacinações, internamentos | *Builders* de dados nos projetos de teste | Valores fictícios; sem dados pessoais reais |
| Alojamentos | Seed de referência (pressuposto da API) | Carregado uma vez e preservado pelo Respawn |
| Tokens JWT | Gerados pela fixture com a chave de teste, para cada perfil, e variantes expirada e revogada | A chave de teste é fornecida por variável de ambiente ou *User Secrets*; **nunca** é versionada |
| Relógio | Abstração do tempo (por exemplo `TimeProvider`) | Permite testar validade de tokens, datas no passado/futuro e próxima administração sem depender da hora real |

Os segredos reais (chave JWT, cadeias de ligação) não são versionados em nenhum teste nem na Collection Postman. As variáveis necessárias constam do `appsettings.Example.json` e do README do backend.

## 7. Execução dos testes: comandos locais e na pipeline

Os comandos partem da pasta da solução (`src/backend`), que contém `ClinicaVeterinaria.sln`.

### 7.1. Localmente

```bash
# 1. Compilar (QG-01)
dotnet build ./ClinicaVeterinaria.sln

# 2. Executar todos os testes (QG-02) — comando já descrito no README do backend
dotnet test ./ClinicaVeterinaria.sln

# 3. Executar só os testes unitários ou só os de integração
dotnet test ./ClinicaVeterinaria.UnitTests
dotnet test ./ClinicaVeterinaria.IntegrationTests

# 4. Executar com cobertura (QG-03)
dotnet test ./ClinicaVeterinaria.sln --collect:"XPlat Code Coverage" --results-directory ./TestResults

# 5. Gerar o relatório de cobertura (requer: dotnet tool install -g dotnet-reportgenerator-globaltool)
reportgenerator -reports:"./TestResults/**/coverage.cobertura.xml" -targetdir:"./TestResults/coverage-report" -reporttypes:"Html;TextSummary"

# 6. Executar a Collection Postman (com a API em execução e a variável baseUrl definida)
newman run ../../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m3-collection-postman-v01.json --env-var baseUrl=https://localhost:5001/api/v1
```

Os passos 4 a 6 dependem de ferramentas ainda não instaladas (secção 5); os comandos exatos serão validados e acrescentados ao `src/backend/README.md` nas tarefas I091 e I103, para que o Revisor os possa repetir a partir do README.

### 7.2. Na pipeline

A pipeline `.github/workflows/backend-ci.yml` (I102 e I103) executa, por esta ordem, e bloqueia a integração em caso de falha:

| Etapa | Quality gate | Critério |
|---|---|---|
| Compilação | QG-01 | Zero erros de compilação |
| Testes unitários e de integração | QG-02 | 100% dos testes a passar |
| Cobertura | QG-03 | Cobertura de linhas ≥ 70% em Domínio e Serviços |
| Análise estática | QG-04 | Zero erros e zero avisos de severidade Warning/Error nas regras acordadas |

Os relatórios de teste (TRX/JUnit), de cobertura e de análise estática são guardados como artefactos da execução e servem de evidência para OF-M3-011, OF-M3-013, OF-M3-014 e OF-M3-016. O relatório de evidências da pipeline (OF-M3-006) inclui execuções bem-sucedidas e bloqueios provocados por falhas.

## 8. Critérios de entrada e de saída

### 8.1. Critérios de entrada

| Nível | Critérios |
|---|---|
| Para escrever testes (T1 e T2) | Plano de Testes revisto e auditado; Especificações I028 e I030 aprovadas; solução .NET compilável (I058); modelo de dados e esquema disponíveis (I060); infraestrutura de testes criada (I091) |
| Para executar testes de integração (T2) | Código da funcionalidade integrado em `develop` por Pull Request aprovado; Docker disponível no ambiente de execução; contentor SQL Server arranca; fixture e *builders* disponíveis |
| Para a Collection Postman (T3) | Endpoint implementado e documentado; API em execução com base de dados carregada com dados de exemplo; variáveis de ambiente definidas |
| Para análise estática (T4) | Configuração acordada em I101 e aplicada ao repositório |

### 8.2. Critérios de saída

| Critério | Limiar | Fonte |
|---|---|---|
| Testes unitários e de integração | 100% dos testes obrigatórios a passar; zero testes ignorados sem justificação | QG-02 |
| Cobertura de linhas, Domínio e Serviços | ≥ 70% | QG-03, MQ-02 |
| Compilação | Zero erros | QG-01 |
| Análise estática | Zero erros e zero avisos de severidade Warning/Error | QG-04 |
| Cobertura dos endpoints | Os 51 endpoints com teste de integração e pedido na Collection Postman | Secção 15 |
| Cobertura dos requisitos | RF001–RF052 e RNF001–RNF005 rastreados para testes | Secção 15 |
| Defeitos | Zero defeitos abertos de severidade Crítica ou Alta; defeitos Médios e Baixos abertos justificados e com Issue | Secção 12 |
| Matriz de permissões | 100% das combinações endpoint × perfil verificadas (secção 10.8) | RNF001 |
| Pipeline | Pelo menos uma execução válida completa e evidência de pelo menos um bloqueio por falha | Regulamento UC, secção 17 |

Os critérios de saída aplicam-se por Pull Request (QG-01 a QG-04) e à entrega da m3 (restantes critérios).

## 9. Limiares de cobertura e justificação

| Código | Limiar | Tipo | Justificação |
|---|---|---|---|
| LT-01 | **Cobertura de linhas ≥ 70%** em Domínio e Serviços (Aplicação), medida com testes unitários e de integração em conjunto | **Obrigatório** (QG-03, MQ-02) | Valor já assumido pelo grupo no Plano de Qualidade (I006). É o mínimo que garante que a maior parte da lógica de negócio, onde se concentram as regras do sistema, é exercitada. Não é aplicado à camada API (controladores finos, sem regras de negócio) nem à Infraestrutura (código de configuração do EF Core). |
| LT-02 | Cobertura de ramos ≥ 60% nas mesmas camadas | Proposta, informativa (não bloqueia a pipeline) | As regras de negócio têm muitos ramos (transições de estado, validações). A cobertura de linhas pode estar acima de 70% e deixar sem teste caminhos de erro. A métrica é medida e interpretada no relatório de cobertura, sem criar um quality gate novo. |
| LT-03 | Cobertura de linhas ≥ 85% nos módulos críticos: autorização por perfil e propriedade, transições de estado (marcação, internamento), regras financeiras (pagamentos, fecho de conta) e cálculo da próxima administração de vacina | Proposta, verificada na revisão | O risco e o impacto de um defeito são maiores nestes módulos (RNF001, RNF002 e separação clínica/administrativa). A verificação na revisão evita aumentar o quality gate global. |

**Exclusões justificadas** da medição: `Program.cs` e registo de dependências (`DependencyInjection.cs`), migrações e ficheiros gerados, DTOs sem lógica, classes marcadas com `[ExcludeFromCodeCoverage]` com justificação em comentário. A lista final de exclusões consta do relatório de cobertura (OF-M3-014).

**Cobertura não substitui verificação.** Uma linha coberta sem *assert* relevante não conta como teste. O Revisor confirma por amostragem que os testes de regras críticas verificam o resultado e não apenas executam o código.

## 10. Condições de teste prioritárias por funcionalidade

As condições seguintes concretizam as regras críticas do Plano de Qualidade (secção 6.2) e as regras da API. As Especificações I028 e I030 desdobram cada condição em casos de teste com dados e resultados esperados. Nível: **U** = unitário, **I** = integração.

### 10.1. F001 — Autenticação e controlos de acesso

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-F001-01 | Login válido devolve token JWT com as claims `sub`, `email`, `role`, `jti`, `exp`, e `veterinarioId` ou `clienteId` conforme o perfil | U, I | RF001, RF004, RF005 |
| CT-F001-02 | Credenciais incorretas → 401; campos em falta → 400; conta bloqueada → 403 | U, I | RF002, RF009 |
| CT-F001-03 | Logout revoga o token (`jti`); reutilizar o token → 401 | U, I | RF003 |
| CT-F001-04 | Token expirado (8 h), adulterado ou ausente → 401 em qualquer endpoint protegido | I | RF004 |
| CT-F001-05 | Apenas o Administrador gere contas de funcionários; criar conta com perfil `Cliente` → 422; e-mail repetido → 409; palavra-passe fraca → 400 | U, I | RF006–RF008 |
| CT-F001-06 | O Administrador não pode bloquear a própria conta nem remover o seu perfil de Administrador → 422 | U, I | RF008, RF009 |
| CT-F001-07 | Conta bloqueada deixa de autenticar e os tokens já emitidos são rejeitados | I | RF009 |
| CT-F001-08 | Horário de funcionamento: períodos sobrepostos ou fecho antes da abertura → 400 | U, I | RF012, RF013, RF019 |
| CT-F001-09 | Administrador e Rececionista criam e atualizam Clientes; NIF ou e-mail repetidos → 409 | U, I | RF010, RF011 |
| CT-F001-10 | O Administrador não acede a registos clínicos, internamentos nem boletins (403) | I | Declaração de Âmbito, RNF001 |
| CT-F001-11 | `podeRegistarApoio=true` só é aceite para o perfil `AuxiliarVeterinario` → 422 nos restantes | U, I | RF008 |

### 10.2. F002 — Gestão de pacientes

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-F002-01 | Rececionista e Veterinário registam e atualizam dados administrativos do paciente; peso ≤ 0, data de nascimento futura ou `clienteId` em falta → 400; cliente inexistente → 404 | U, I | RF014, RF016 |
| CT-F002-02 | Pesquisa de pacientes com filtros e paginação (`pagina`, `tamanho` ≤ 100, `ordenar`); parâmetros inválidos → 400 | I | RF017 |
| CT-F002-03 | Auxiliar Veterinário consulta a ficha administrativa; não cria nem altera pacientes (403) | I | RF015, RF026 |
| CT-F002-04 | O Rececionista não cria nem altera registos clínicos (403) | I | Plano de Qualidade 6.2 |
| CT-F002-05 | O Cliente só vê os seus animais em `/me/animais`; animal de outro Cliente → 403; sem operações de escrita | I | RF018, RNF002 |

### 10.3. F003 — Agendamento de consultas

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-F003-01 | Criar marcação válida (201); data no passado → 400; fora do horário de funcionamento ou veterinário indisponível → 422 | U, I | RF021, RF019, RF020 |
| CT-F003-02 | Disponibilidade de um veterinário: período > 31 dias ou datas inválidas → 400; resultado coerente com o horário da clínica e as marcações existentes | U, I | RF020 |
| CT-F003-03 | Transições do estado da marcação (`Pendente`, `Confirmada`, `Realizada`, `Cancelada`): todas as válidas aceites; todas as inválidas → 422; `Pendente` como destino → 400 | U, I | RF023 |
| CT-F003-04 | Alterar marcação `Realizada` ou `Cancelada`, ou para horário indisponível → 422 | U, I | RF022 |
| CT-F003-05 | Registar chegada apenas em marcação `Confirmada` e uma só vez; segunda tentativa → 422 | U, I | RF025 |
| CT-F003-06 | O Veterinário apenas consulta a agenda e as consultas que lhe estão atribuídas; consulta de outro Veterinário → 403; não cria, altera nem cancela | I | RF024, RNF001 |
| CT-F003-07 | O Auxiliar Veterinário regista notas de preparação (não clínicas) apenas com autorização ativa; texto vazio ou > 1000 caracteres → 400; marcação `Cancelada` → 422 | U, I | RF027 |
| CT-F003-08 | O Cliente consulta apenas as suas marcações em `/me/consultas` (futuras e histórico administrativo), sem dados clínicos e sem marcar, alterar ou cancelar | I | RF028, RF029, RNF002 |

### 10.4. F004 — Registo clínico e tratamentos

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-F004-01 | Só o Veterinário cria e altera registos clínicos; qualquer outro perfil → 403 | I | RF030, RF034, RNF001 |
| CT-F004-02 | Criar registo sem elemento clínico ou com prescrição incompleta → 400 | U, I | RF031–RF033 |
| CT-F004-03 | Segundo registo para a mesma consulta → 409; consulta `Pendente` ou `Cancelada` → 422; consulta inexistente → 404 | U, I | RF030 |
| CT-F004-04 | O Auxiliar Veterinário apenas lê registos clínicos (sem escrita) | I | RF035, RF036 |
| CT-F004-05 | Histórico clínico do paciente filtrado por intervalo de datas; intervalo inválido → 400 | I | RF035 |
| CT-F004-06 | Registos clínicos não existem em `/me/...` nem são acessíveis ao perfil Cliente | I | RNF002, RNF003 |

### 10.5. F005 — Contas e pagamentos

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-F005-01 | Registar custo: valor ≤ 0 ou descrição em falta → 400; consulta ou animal que não pertence ao cliente → 422 | U, I | RF037 |
| CT-F005-02 | Pagamento parcial reduz o valor em dívida; o estado passa a `Pago` automaticamente quando a dívida chega a zero | U, I | RF039, RF040 |
| CT-F005-03 | Pagamento superior ao valor em dívida, em conta já `Pago` ou encerrada → 422; valor ≤ 0 ou método inválido → 400 | U, I | RF039 |
| CT-F005-04 | Correção do estado de pagamento: estado incoerente com o valor em dívida, igual ao atual, ou conta encerrada → 422; `observacao` em falta → 400 | U, I | RF040 |
| CT-F005-05 | Fecho de conta só quando `Pago` e sem dívida; caso contrário → 422 | U, I | RF041 |
| CT-F005-06 | Só o Rececionista gere contas e pagamentos; o Cliente consulta apenas as suas despesas em `/me/despesas` (leitura) | I | RF038, RF042, RNF002 |

### 10.6. F006 — Boletim de vacinas e desparasitações

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-F006-01 | Cálculo automático da data prevista da próxima administração: data de administração + `intervaloDias`; sem intervalo, não é calculada data | U | RF044 |
| CT-F006-02 | Registar vacinação ou desparasitação: tipo, produto, lote ou data em falta, data futura ou `intervaloDias` ≤ 0 → 400; animal inexistente → 404 | U, I | RF043, RF045 |
| CT-F006-03 | Só o Veterinário regista e consulta o boletim na web; outros perfis → 403 | I | RF043, RF046, RNF001 |
| CT-F006-04 | O Cliente consulta apenas o boletim dos seus animais; animal de outro Cliente → 403 | I | RF047, RNF002 |

### 10.7. F007 — Gestão de internamento

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-F007-01 | Registar internamento (201); alojamento ocupado ou animal já com internamento ativo → 409; animal ou alojamento inexistente → 404 | U, I | RF048 |
| CT-F007-02 | Associar alojamento: alojamento ocupado → 409; internamento com alta → 422 | U, I | RF049 |
| CT-F007-03 | Estados `EmObservacao` e `Recobro`: estado igual ao atual ou internamento com alta → 422; a alta só pelo endpoint EP-051 (→ 400 se pedida por EP-050) | U, I | RF051 |
| CT-F007-04 | Registar alta médica: internamento já com alta → 422; o alojamento fica livre | U, I | RF052 |
| CT-F007-05 | Veterinário e Auxiliar Veterinário consultam; atualizar estado e registar alta são exclusivos do Veterinário (403 ao Auxiliar) | I | RF050–RF052, RNF001 |

### 10.8. Transversal — permissões, isolamento e validação

| ID | Condição | Nível | Origem |
|---|---|---|---|
| CT-TRV-01 | **Matriz endpoint × perfil**: para cada um dos 51 endpoints e cada um dos 5 perfis, o código esperado (2xx ou 403) conforme a matriz de permissões da API; mais um pedido anónimo (401) por endpoint. Total: **51 × 6 = 306 verificações**, geradas de forma parametrizada a partir de uma única tabela | I | RNF001 |
| CT-TRV-02 | O Cliente só acede a `/auth/*` e `/me/*`; qualquer outra rota → 403 | I | RNF003 |
| CT-TRV-03 | Isolamento: o identificador do Cliente vem sempre do token; `animalId` de outro Cliente → 403; nunca dados de outro Cliente em respostas de listas | I | RNF002 |
| CT-TRV-04 | Formato de erro Problem Details (RFC 9457) com `type`, `title`, `status`, `detail`, `instance`, `traceId`; erros 400 incluem `errors` por campo | I | RNF004 |
| CT-TRV-05 | Dados inválidos não produzem alterações persistentes (consulta posterior mostra o estado anterior) | I | RNF004, Arquitetura §11 |
| CT-TRV-06 | Dado confirmado (201/200/204) fica disponível em consulta posterior | I | RNF005 |
| CT-TRV-07 | Paginação e ordenação: valores por omissão, máximo `tamanho` = 100, envelope `itens/pagina/tamanho/total` | I | API §2 |
| CT-TRV-08 | Logs não expõem tokens nem palavras-passe | I | Arquitetura (segurança) |

## 11. Collection Postman

A **Collection Postman** (`m3-collection-postman-v01.json`, em `03.01.05-postman`) permite **verificar todas as funcionalidades da API**, quer manualmente no Postman, quer por linha de comandos com o Newman. É um artefacto obrigatório da m3 e complementa, sem substituir, os testes automáticos T1 e T2.

- **Organização**: uma pasta por funcionalidade (F001 a F007) e, dentro de cada uma, um pedido por endpoint (51 pedidos, `PM-F00x-EPnnn`). Pedidos adicionais podem exemplificar erros importantes (por exemplo 422 em transições inválidas).
- **Variáveis**: `baseUrl`, uma variável de token por perfil (`tokenAdmin`, `tokenRececionista`, `tokenVeterinario`, `tokenAuxiliar`, `tokenCliente`) e identificadores de dados de exemplo (`clienteId`, `animalId`, `consultaId`, entre outros).
- **Autenticação**: a pasta F001 contém os pedidos de login por perfil. Cada um guarda o token na variável correspondente através de um *test script*.
- **Verificações automáticas** em cada pedido: código de resposta esperado, tipo de conteúdo JSON e campos obrigatórios do corpo; nos pedidos de criação, o identificador devolvido é guardado para os pedidos seguintes.
- **Dados de exemplo**: valores fictícios, coerentes com a documentação da API.
- **Segredos**: a collection não contém palavras-passe reais, tokens nem chaves; as credenciais dos utilizadores de teste ficam num ficheiro de ambiente local, fora do repositório, ou em variáveis de ambiente.
- **Execução**: manual no Postman, ou por `newman run`, com a API em execução e dados de exemplo carregados. A execução na pipeline é recomendada como evidência complementar, mas não é um quality gate do Plano de Qualidade (D-05).

## 12. Gestão de defeitos

Seguindo o Plano de Qualidade (secção 5.3), é **defeito** qualquer incumprimento de requisito, critério de aceitação, DoD, norma adotada ou decisão anterior, e tem correção obrigatória. Sugestões de valor que ultrapassem as obrigações aprovadas são **propostas de melhoria** e seguem como novas Issues, sem bloquear.

| Item | Regra |
|---|---|
| Registo | Cada defeito é uma **Issue no GitHub** (repositório e Project do grupo), com a etiqueta `defeito` (a criar), a ligação à Issue ou Pull Request de origem e ao teste que o detetou. A Issue é a fonte única de verdade. |
| Registo consolidado | Se existirem defeitos, é mantido o `m3-registo-defeitos-backend-v01.xlsx` (artefacto condicional, `03.01.07-defeitos`), a partir das Issues, com severidade, prioridade, origem, responsável, estado, correção e teste de confirmação. |
| Quem classifica | O **Revisor** distingue defeito de proposta de melhoria e atribui a severidade; o **Auditor** verifica na auditoria que os defeitos obrigatórios foram corrigidos. |
| Quem corrige | O **Executor da tarefa de origem**. Se o defeito for detetado num teste de outra tarefa, abre-se a Issue e o responsável é o Executor da tarefa que introduziu o código. |
| Confirmação | Cada correção inclui um teste (ou caso de teste) que falha antes da correção e passa depois; o Revisor confirma. |
| Tarefas já concluídas | Conforme o Plano de Qualidade, tarefas concluídas não são reabertas: um defeito posterior origina nova Issue relacionada. |

| Severidade | Definição | Prazo-alvo de correção | Impacto na saída |
|---|---|---|---|
| Crítica | Falha de segurança ou de isolamento de perfis; perda ou corrupção de dados; endpoint essencial inoperante | Antes de qualquer nova integração | Bloqueia a entrega |
| Alta | Regra de negócio errada ou código de resposta errado com impacto funcional | No mesmo Sprint | Bloqueia a entrega |
| Média | Comportamento incorreto com solução alternativa; mensagem de erro incorreta | Até ao fim da milestone | Permitido com Issue e justificação |
| Baixa | Cosmético, documentação ou melhoria de estilo | Quando conveniente | Permitido |

## 13. Responsabilidades e calendário

### 13.1. Papéis

| Papel | Responsabilidade na atividade de testes |
|---|---|
| Executor | Escreve e executa os testes da sua tarefa; corrige defeitos da tarefa de origem; entrega evidências até 48 h antes do prazo, conforme o Plano de Qualidade |
| Revisor | Verifica critérios de aceitação, comandos, rastreabilidade e coerência com os limiares; classifica defeitos; regista a revisão (02.04) |
| Auditor | Verifica independência dos três papéis, prazos, pontos, versão e pasta; confirma que o plano cumpre o conteúdo mínimo da secção 19 (02.05) |
| Líder | Mantém o Backlog, o Sprint Backlog e os quality gates da pipeline alinhados com este plano |
| Secretário | Regista nas atas as decisões sobre limiares e ferramentas ratificadas |

### 13.2. Tarefas de testes do backend

| Issue | Tarefa | Sprint |
|---|---|---|
| I027 | Elaborar o Plano de Testes do Backend (este documento) | S03 |
| I028 | Especificar os Testes Unitários do Backend | S04 |
| I030 | Especificar os Testes de Integração do Backend | S04 |
| I091 | Criar a infraestrutura dos testes de integração | S04 |
| I101 | Configurar a análise estática e o quality gate do backend | S04 |
| I102 | Criar a pipeline de compilação e testes do backend | S04 |
| I086, I087, I088, I089 | Implementar os testes unitários por funcionalidade | S05 |
| I090 | Executar os testes unitários e produzir o relatório de resultados | S05 |
| I092, I093, I094, I095 | Implementar os testes de integração por funcionalidade | S05 |
| I096 | Implementar os testes de segurança e de isolamento por perfil | S05 |
| I097 | Executar os testes de integração e produzir o relatório de resultados | S05 |
| I103 | Integrar análise estática, cobertura e quality gates na pipeline | S05 |

A Collection Postman e o relatório de cobertura têm tarefas próprias no Backlog; as dependências e os Sprints seguem o Backlog de Projeto vigente.

## 14. Riscos

| Risco | Probabilidade | Impacto | Mitigação |
|---|---|---|---|
| Docker indisponível no computador de um estudante ou no runner | Média | Alto | Documentar pré-requisitos no README; a pipeline executa os mesmos testes; alternativa de contentor SQL Server local documentada |
| Testes de integração lentos pelo arranque do SQL Server | Média | Médio | Um contentor por execução (fixture partilhada) e reposição com Respawn em vez de recriar a base de dados |
| Testes instáveis por dependência do relógio ou da ordem de execução | Média | Médio | Abstração do tempo; testes independentes; dados criados por *builders* em cada teste |
| Cobertura ≥ 70% não atingida a tempo | Baixa a média | Alto | Medir a cobertura desde S04; testes unitários escritos junto de cada funcionalidade; acompanhar a métrica MQ-02 nas revisões |
| Divergência entre versões das ferramentas do Plano de Qualidade e do código | Alta | Baixo | Decisão D-02: alinhar o código ou atualizar o Plano de Qualidade (via pedido formal) |
| Mudança na API sem atualização dos testes e da Collection | Média | Alto | Rastreabilidade EP → testes (secção 15); a documentação da API m3 é atualizada com o código; revisão cruzada |
| Segredos versionados por engano nos testes ou na Collection | Baixa | Alto | Segredos só em variáveis de ambiente ou *User Secrets*; revisão e análise do repositório antes de cada submissão |

## 15. Rastreabilidade

### 15.1. Endpoints, requisitos e níveis de teste

**U** = testes unitários; **I** = testes de integração; **PM** = pedido na Collection Postman. Os códigos de resposta são os documentados na API; **2xx** representa o código de sucesso (200, 201 ou 204) do endpoint, e 401 e 403 aplicam-se conforme a secção 4.2. Dos 51 endpoints, 27 têm testes unitários dedicados porque contêm regras de negócio, validações específicas ou transições de estado; nos restantes a lógica reside em consultas simples e é exercitada pelos testes de integração.

| Endpoint | Método e rota | Perfis | Requisitos | Casos de uso | Códigos a verificar | U | I | PM |
|---|---|---|---|---|---|---|---|---|
| EP-001 | `POST /auth/login` | Todos | RF001, RF002, RF004, RF005 | UC01 | 2xx, 400, 401, 403 | Sim | Sim | Sim |
| EP-002 | `POST /auth/logout` | Todos | RF003 | UC02 | 2xx, 401 | Sim | Sim | Sim |
| EP-003 | `GET /auth/me` | Todos | RF004, RF005 | UC01 | 2xx, 401 | Não | Sim | Sim |
| EP-004 | `GET /utilizadores` | Admin. | RF007, RF008, RF009 | UC03 (A1, A2, A3) | 2xx, 400, 401, 403 | Não | Sim | Sim |
| EP-005 | `GET /utilizadores/{id}` | Admin. | RF007 | UC03 (A1) | 2xx, 401, 403, 404 | Não | Sim | Sim |
| EP-006 | `POST /utilizadores` | Admin. | RF006, RF008 | UC03 | 2xx, 400, 401, 403, 409, 422 | Sim | Sim | Sim |
| EP-007 | `PUT /utilizadores/{id}` | Admin. | RF007, RF008 | UC03 (A1, A2) | 2xx, 400, 401, 403, 404, 409, 422 | Sim | Sim | Sim |
| EP-008 | `PATCH /utilizadores/{id}/estado` | Admin. | RF009 | UC03 (A3) | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-009 | `GET /clinica` | Admin. | RF012, RF019 | UC05, UC09 | 2xx, 401, 403 | Não | Sim | Sim |
| EP-010 | `PUT /clinica` | Admin. | RF013, RF019 | UC05 (A1), UC09 | 2xx, 400, 401, 403 | Não | Sim | Sim |
| EP-011 | `GET /clientes` | Admin., Rec., Vet. | RF011, RF017 | UC04 (A1), UC07 (A1) | 2xx, 401, 403 | Não | Sim | Sim |
| EP-012 | `GET /clientes/{id}` | Admin., Rec., Vet. | RF011, RF017 | UC04 (A1), UC07 (A1) | 2xx, 401, 403, 404 | Não | Sim | Sim |
| EP-013 | `POST /clientes` | Admin., Rec. | RF010 | UC04 | 2xx, 400, 401, 403, 409 | Sim | Sim | Sim |
| EP-014 | `PUT /clientes/{id}` | Admin., Rec. | RF011 | UC04 (A1) | 2xx, 400, 401, 403, 404, 409 | Sim | Sim | Sim |
| EP-015 | `GET /animais` | Rec., Vet. | RF017 | UC07 (A1) | 2xx, 400, 401, 403 | Não | Sim | Sim |
| EP-016 | `GET /animais/{id}` | Rec., Vet., Aux. Vet. | RF015, RF026 | UC07, UC10 | 2xx, 401, 403, 404 | Não | Sim | Sim |
| EP-017 | `POST /animais` | Rec., Vet. | RF014 | UC06 | 2xx, 400, 401, 403, 404 | Não | Sim | Sim |
| EP-018 | `PUT /animais/{id}` | Rec., Vet. | RF016 | UC07 (A2) | 2xx, 400, 401, 403, 404 | Não | Sim | Sim |
| EP-019 | `GET /me/animais` | Cliente | RF018 | UC08 | 2xx, 401, 403 | Não | Sim | Sim |
| EP-020 | `GET /me/animais/{id}` | Cliente | RF018 | UC08 | 2xx, 401, 403, 404 | Sim | Sim | Sim |
| EP-021 | `GET /veterinarios` | Rec. | RF020, RF021 | UC09 | 2xx, 401, 403 | Não | Sim | Sim |
| EP-022 | `GET /veterinarios/{id}/disponibilidade` | Rec. | RF020, RF019 | UC09 | 2xx, 400, 401, 403, 404 | Sim | Sim | Sim |
| EP-023 | `GET /consultas` | Rec., Vet., Aux. Vet. | RF024, RF026 | UC09 (A3, A4), UC10 | 2xx, 400, 401, 403 | Sim | Sim | Sim |
| EP-024 | `GET /consultas/{id}` | Rec., Vet., Aux. Vet. | RF024, RF026 | UC09, UC10 | 2xx, 401, 403, 404 | Sim | Sim | Sim |
| EP-025 | `POST /consultas` | Rec. | RF021 | UC09 | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-026 | `PUT /consultas/{id}` | Rec. | RF022 | UC09 (A1) | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-027 | `PATCH /consultas/{id}/estado` | Rec. | RF023 | UC09 (A2) | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-028 | `PATCH /consultas/{id}/chegada` | Rec. | RF025 | UC09 (A5) | 2xx, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-029 | `POST /consultas/{id}/notas-preparacao` | Aux. Vet. | RF027 | UC10 (A1) | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-030 | `GET /me/consultas` | Cliente | RF028, RF029 | UC11 | 2xx, 400, 401, 403 | Não | Sim | Sim |
| EP-031 | `POST /registos-clinicos` | Vet. | RF030, RF031, RF032, RF033 | UC12 (fluxo principal, A1, A2, A3) | 2xx, 400, 401, 403, 404, 409, 422 | Sim | Sim | Sim |
| EP-032 | `PUT /registos-clinicos/{id}` | Vet. | RF034, RF032, RF033 | UC12 (A2, A3, A4) | 2xx, 400, 401, 403, 404 | Não | Sim | Sim |
| EP-033 | `GET /registos-clinicos/{id}` | Vet., Aux. Vet. | RF035, RF036 | UC12 (A5), UC13 | 2xx, 401, 403, 404 | Não | Sim | Sim |
| EP-034 | `GET /animais/{id}/historico-clinico` | Vet., Aux. Vet. | RF035, RF036 | UC12 (A5), UC13 | 2xx, 400, 401, 403, 404 | Não | Sim | Sim |
| EP-035 | `GET /contas` | Rec. | RF038 | UC14 (A1) | 2xx, 400, 401, 403 | Não | Sim | Sim |
| EP-036 | `POST /contas` | Rec. | RF037 | UC14 | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-037 | `GET /contas/{id}` | Rec. | RF038, RF039 | UC14 (A1, A2) | 2xx, 401, 403, 404 | Não | Sim | Sim |
| EP-038 | `POST /contas/{id}/pagamentos` | Rec. | RF039, RF040 | UC14 (A2) | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-039 | `PATCH /contas/{id}/estado-pagamento` | Rec. | RF040 | UC14 (A3) | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-040 | `PATCH /contas/{id}/fecho` | Rec. | RF041 | UC14 (A4) | 2xx, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-041 | `GET /me/despesas` | Cliente | RF042 | UC15 | 2xx, 400, 401, 403 | Não | Sim | Sim |
| EP-042 | `GET /animais/{id}/vacinacoes` | Vet. | RF046, RF045 | UC16 (A2) | 2xx, 400, 401, 403, 404 | Não | Sim | Sim |
| EP-043 | `POST /animais/{id}/vacinacoes` | Vet. | RF043, RF044, RF045 | UC16, UC16 (A1) | 2xx, 400, 401, 403, 404 | Sim | Sim | Sim |
| EP-044 | `GET /me/animais/{id}/vacinacoes` | Cliente | RF047 | UC17 | 2xx, 401, 403, 404 | Sim | Sim | Sim |
| EP-045 | `GET /alojamentos` | Vet., Aux. Vet. | RF049, RF050 | UC18 (A1, A2) | 2xx, 401, 403 | Não | Sim | Sim |
| EP-046 | `GET /internamentos` | Vet., Aux. Vet. | RF050 | UC18 (A2) | 2xx, 400, 401, 403 | Não | Sim | Sim |
| EP-047 | `GET /internamentos/{id}` | Vet., Aux. Vet. | RF050 | UC18 (A2) | 2xx, 401, 403, 404 | Não | Sim | Sim |
| EP-048 | `POST /internamentos` | Vet. | RF048 | UC18 | 2xx, 400, 401, 403, 404, 409 | Sim | Sim | Sim |
| EP-049 | `PATCH /internamentos/{id}/alojamento` | Vet. | RF049 | UC18 (A1) | 2xx, 400, 401, 403, 404, 409, 422 | Sim | Sim | Sim |
| EP-050 | `PATCH /internamentos/{id}/estado` | Vet. | RF051 | UC18 (A3) | 2xx, 400, 401, 403, 404, 422 | Sim | Sim | Sim |
| EP-051 | `PATCH /internamentos/{id}/alta` | Vet. | RF052 | UC18 (A4) | 2xx, 401, 403, 404, 422 | Sim | Sim | Sim |

### 15.2. Cobertura dos requisitos

- **Requisitos funcionais RF001–RF052**: cada requisito está ligado a, pelo menos, um endpoint (secção 14.4 da Documentação da API) e cada endpoint tem teste de integração e pedido Postman. Logo, todos os RF têm, no mínimo, teste de integração e verificação na Collection Postman. As condições da secção 10 reforçam os requisitos com maior risco.
- **Casos de uso UC01–UC18**: fluxos principais e alternativos a cobrir pelos testes de integração e, quando há regras, pelos unitários, conforme a coluna «Casos de uso» da tabela 15.1.

### 15.3. Requisitos não funcionais

| Requisito | Verificação | Condições | Nível |
|---|---|---|---|
| RNF001 — Controlo de acesso às operações | Matriz endpoint × perfil com 306 verificações | CT-TRV-01, CT-F001-10, CT-F003-06 | I |
| RNF002 — Isolamento dos dados de Clientes | `/me/...` e acesso a recursos de outros Clientes | CT-TRV-03, CT-F002-05, CT-F005-06, CT-F006-04 | I |
| RNF003 — Separação dos canais de utilização | O Cliente só acede a `/auth/*` e `/me/*` | CT-TRV-02, CT-F004-06 | I |
| RNF004 — Validação dos dados de entrada | Validadores e respostas 400 com `errors` por campo; sem persistência em erro | CT-TRV-04, CT-TRV-05 | U, I |
| RNF005 — Persistência dos dados confirmados | Escrita seguida de leitura | CT-TRV-06 | I |
| RNF006 — Dependência de ligação à rede | Fora do âmbito de teste do backend (secção 3.2) | — | — |

## 16. Coerência com o Plano de Qualidade (I006)

| Elemento do Plano de Qualidade | Como é respeitado neste plano |
|---|---|
| Objetivo 4 — Segurança funcional (RBAC) validada por testes automáticos para os 5 perfis | Matriz endpoint × perfil (CT-TRV-01) e condições da secção 10 |
| Objetivo 5 e MQ-02 — Cobertura de linhas ≥ 70% no backend | LT-01 (obrigatório) |
| QG-01 a QG-04 | Secções 7.2 e 8.2 |
| Secção 6.2 — Regras críticas por funcionalidade F001–F007 | Secção 10 |
| Secção 6.3 — Artefactos de m3 (OF-M3-009 a OF-M3-016) | Secção 4 (coluna de evidências) |
| Secção 5.3 — Defeito vs. proposta de melhoria | Secção 12 |
| Ferramentas: xUnit 2.6, Moq 4.20, Coverlet 6.0, ReportGenerator 5.2, Respawn / Testcontainers .NET, Roslyn | Secção 5 (com divergência assinalada na D-02) |

## 17. Decisões tomadas e pontos a ratificar

Esta lista regista as decisões deste plano (evidência exigida na I027). Todas são propostas do Executor, sujeitas a revisão, e as que alteram compromissos do grupo devem ficar em ata.

| ID | Decisão | Justificação | Estado |
|---|---|---|---|
| D-01 | Testes de integração com `WebApplicationFactory`, Testcontainers (SQL Server) e Respawn | Base de dados real e isolada por execução, alinhada com o Plano de Qualidade e com o SQL Server da Arquitetura; evita dependência de uma base partilhada | Proposta |
| D-02 | Alinhar as versões do código com o Plano de Qualidade (xUnit 2.6 e Moq 4.20) em I091, ou, se preferirem manter a 2.5.3, atualizar o Plano de Qualidade por pedido formal | O código atual usa xUnit 2.5.3; o Plano de Qualidade (entregue) refere 2.6 | **A decidir** |
| D-03 | Limiar obrigatório LT-01 (linhas ≥ 70% em Domínio e Serviços); LT-02 e LT-03 como metas informativas e de revisão | Respeita o Plano de Qualidade sem criar novos quality gates; foca o esforço onde o risco é maior | Proposta |
| D-04 | Matriz endpoint × perfil gerada de forma parametrizada a partir de uma tabela única | Garante 306 verificações com manutenção mínima e rastreabilidade direta à matriz de permissões da API | Proposta |
| D-05 | Newman recomendado na pipeline como evidência complementar, mas fora dos quality gates | O Plano de Qualidade define QG-01 a QG-04; acrescentar um gate novo exigiria alterar o Plano | Proposta |
| D-06 | Defeitos como Issues com etiqueta `defeito`; registo consolidado em xlsx só se existirem defeitos | Reutiliza as ferramentas de gestão já em uso e cumpre a secção 19 | Proposta |
| D-07 | Abstração do relógio (`TimeProvider`) nos serviços com regras temporais | Permite testes determinísticos de validade de token, datas e próxima administração | Proposta |
| D-08 | Novas dependências de teste: Moq, Mvc.Testing, Testcontainers.MsSql, Respawn, ReportGenerator (ferramenta) | Necessárias para os níveis T1 a T3 e para o relatório de cobertura | A concretizar em I091 |

**Pontos a confirmar no Backlog Refinement ou na revisão:**

1. Valores de LT-02 (60%) e LT-03 (85%).
2. Etiqueta `defeito` e política de registo (D-06).
3. Versões das ferramentas (D-02).
4. Se o relatório de análise estática (QG-04) usa apenas os analisadores do SDK ou também outros analisadores adicionais.

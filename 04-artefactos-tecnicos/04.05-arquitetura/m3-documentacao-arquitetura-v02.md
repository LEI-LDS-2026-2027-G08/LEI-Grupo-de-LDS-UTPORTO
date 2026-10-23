# Documentação da Arquitetura

**Projeto:** Gestão de Clínica Veterinária  
**Grupo:** G08  
**Milestone:** m3  
**Versão:** v02  
**Issue e papéis:** por atribuir. Esta versão ainda não tem Issue própria; será planeada no Backlog Refinement e no Sprint Planning, onde se atribuem Executor, Revisor e Auditor. A Issue I019 corresponde à v01 (m2) e não é reaberta.  
**Estado:** Rascunho para revisão e auditoria  
**Ficheiro:** `m3-documentacao-arquitetura-v02.md`

---

## Histórico de versões

| Versão | Data | Descrição das alterações |
|---|---|---|
| v01 | m2 | Versão inicial da Documentação da Arquitetura (Issue I019). Mantém-se inalterada como histórico (`m2-documentacao-arquitetura-v01.md`). |
| v02 | 23/10/2026 | Reflete a Declaração de Âmbito v05 (F005 e F007 passam para o não-âmbito) e as tecnologias do Regulamento Interno do Grupo v02 (21/10/2026). Os componentes, as responsabilidades, as decisões e os diagramas que não dependem destas duas alterações mantêm-se. |

### Alterações da v02 em relação à v01

| Ponto | v01 | v02 | Motivo |
|---|---|---|---|
| Módulos funcionais (secções 3, 5, 7, 8, 9, 13, 14, 16, 17 e 18) | Sete módulos, ARC-F001 a ARC-F007 | Cinco módulos em âmbito: ARC-F001 a ARC-F004 e ARC-F006. ARC-F005 e ARC-F007 ficam fora do âmbito, com os identificadores reservados | Declaração de Âmbito v05 (transição de F005 e F007 para o não-âmbito, autorizada a 20/10/2026 e homologada por e-mail a 21/10/2026); Casos de Uso v02; Documentação da API v02 |
| Tecnologias (secções 6, 7.1 a 7.3, 7.12, 16, 17 e 18) | .NET 8, C# 12, EF Core 8.0, React 18.2, TypeScript 5.3, Node.js 20, Flutter 3.19, Dart 3.2, Android SDK 34, npm 10 | .NET 10, C# 14, EF Core 10.0, React 19.3, TypeScript 6.0, Node.js 24 LTS, Vite 8.3 e npm 12, Kotlin 2.2 com Jetpack Compose, Android SDK API 37 | Regulamento Interno do Grupo v02, secção 11.2 |
| Condicionantes e perfis (secções 4 e 10.2) | Incluíam regras de pagamentos e de internamentos | Retiradas as regras de pagamentos e de internamentos | Declaração de Âmbito v05 |
| ARC-MOB (secção 7.2) | Apresentava despesas e estados de pagamento | Retirada esta responsabilidade | Modelo de Classes do Frontend Mobile v02 (7 endpoints) |
| Exclusões (secção 15) | Pagamentos e internamentos tratados como elementos específicos da arquitetura | Passam a constar também como funcionalidades fora do âmbito | Declaração de Âmbito v05 |
| Decisões (secção 16) | DA-ARQ-01 a DA-ARQ-09 | DA-ARQ-04 e DA-ARQ-05 atualizadas; acrescentadas DA-ARQ-10 e DA-ARQ-11 | Pontos anteriores |
| Referências (secção 2) | Documentos da m1 e m2 | Documentos em vigor na m3 | Controlo de versões |
| Base de dados (secção 7.13) | Tabelas para F001 a F007 | Nota sobre as tabelas de F005 e F007 que ainda existem no esquema e novo ponto em aberto (secção 20) | Scripts `m3-v01` e Modelo de Dados v01 ainda não revistos |
| Histórico e estado | — | Acrescentados | Controlo de versões |

As secções 7.4 a 7.8, 7.10, 10, 11, 12 e 19 mantêm o conteúdo da v01, salvo as referências a módulos e a tecnologias já indicadas.

---

## 1. Objetivo

O presente documento descreve a arquitetura prevista para o sistema de Gestão de Clínica Veterinária.

A documentação identifica os principais componentes do sistema, as respetivas responsabilidades, as relações entre componentes, as tecnologias previstas, as interfaces de comunicação, as restrições e as decisões arquiteturais relevantes.

A arquitetura é definida de forma coerente com a Proposta de Sistema v05 e a Declaração de Âmbito v05 homologadas, com a Especificação de Requisitos v02 e com as Especificações dos Casos de Uso v02.

Este documento constitui ainda uma referência para os modelos de classes, documentação da API, protótipos, implementação e Requirements Traceability Matrix (RTM).

---

## 2. Documentos de referência

A arquitetura descrita neste documento tem como referências:

- `m3-proposta-sistema-v05.pdf`;
- `m3-declaracao-ambito-v05.pdf`;
- `m3-regulamento-grupo-v02.pdf`;
- `m1-plano-qualidade-processo-v01.pdf` e `m3-plano-qualidade-processo-v02.md` (rascunho);
- `m1-definition-of-ready-v01.pdf`;
- `m1-definition-of-done-v01.pdf`;
- `m3-especificacao-requisitos-v02.md` (rascunho);
- `m3-especificacoes-casos-uso-v02.md` (rascunho);
- `m3-documentacao-api-v02.md` (rascunho);
- `m3-backlog-projeto-v02.pdf`.

Quando existir conflito entre este documento e os documentos homologados ou normativos aplicáveis, prevalecem os documentos de nível superior.

---

## 3. Âmbito arquitetural

A arquitetura cobre os quatro componentes técnicos principais previstos para o sistema:

1. **Frontend Web**, destinado aos utilizadores internos da clínica;
2. **Frontend Mobile**, destinado exclusivamente ao Cliente;
3. **Backend / API**, responsável pela lógica de negócio, autenticação, autorização, validação e acesso aos dados;
4. **Base de Dados**, responsável pela persistência da informação do sistema.

A arquitetura documenta ainda a organização lógica do backend em módulos correspondentes às funcionalidades em âmbito (F001 a F004 e F006). As funcionalidades F005 e F007 passaram para o não-âmbito (Declaração de Âmbito v05) e já não têm módulo.

Não são detalhados neste documento:

- os endpoints concretos, métodos, parâmetros e formatos de pedido/resposta, que pertencem à Documentação de API;
- as entidades, atributos, relações e cardinalidades detalhadas da base de dados, que pertencem ao Modelo de Dados;
- as classes, atributos, operações e relações detalhadas de backend, frontend web e frontend mobile, que pertencem aos respetivos Modelos de Classes;
- o desenho visual detalhado dos ecrãs, que pertence ao Protótipo de Baixa Fidelidade.

---

## 4. Condicionantes arquiteturais

A arquitetura deve respeitar as seguintes condicionantes do âmbito aprovado:

- os perfis **Administrador**, **Rececionista**, **Veterinário** e **Auxiliar Veterinário** utilizam a aplicação web;
- o **Administrador** é responsável pela gestão dos horários de funcionamento da clínica no âmbito de F003;
- o perfil **Cliente (Proprietário do animal)** utiliza exclusivamente a aplicação móvel;
- as permissões devem respeitar a separação definida entre dados administrativos e dados clínicos;
- o Administrador não pode consultar, criar ou alterar registos clínicos;
- o Rececionista não pode criar ou alterar registos clínicos, diagnósticos, tratamentos ou prescrições;
- o Auxiliar Veterinário apenas pode consultar a informação clínica necessária às suas funções e não pode criar ou alterar registos clínicos;
- o Cliente apenas pode consultar informação pertencente aos seus próprios animais;
- o Cliente não pode criar, alterar ou cancelar marcações através da aplicação móvel;
- o Cliente não pode criar ou editar fichas de animais;
- a gestão de contas e pagamentos (F005) e a gestão de internamento (F007) estão fora do âmbito (Declaração de Âmbito v05) e não fazem parte desta arquitetura;
- o sistema depende de ligação à rede e não garante funcionamento offline;
- o sistema é desenvolvido e demonstrado em ambiente académico controlado com dados fictícios;
- não é prevista a migração de dados provenientes de sistemas veterinários anteriores;
- ficam excluídas integrações externas não previstas no âmbito homologado.

---

## 5. Visão geral da arquitetura

O sistema adota uma arquitetura cliente-servidor, com separação entre as interfaces de utilizador, o backend e a persistência de dados.

O **Frontend Web** e o **Frontend Mobile** comunicam com o **Backend / API**. As aplicações cliente não acedem diretamente à base de dados.

O Backend concentra a aplicação das regras de negócio e das regras de autorização. A persistência é realizada através da camada de acesso a dados, utilizando Entity Framework Core sobre uma base de dados Microsoft SQL Server.

A organização interna do backend é lógica e modular. Os módulos correspondem às áreas funcionais em âmbito (F001 a F004 e F006), não representando serviços independentes nem microserviços. O objetivo desta separação é manter responsabilidades claras e permitir rastreabilidade direta entre requisitos, casos de uso, arquitetura e implementação futura.

---

## 6. Tecnologias previstas

| Componente | Tecnologia principal | Versão / ambiente |
|---|---|---|
| Backend | C# / ASP.NET Core Web API | C# 14 / ASP.NET Core Web API 10.0 / .NET 10.0 SDK |
| Persistência no backend | Entity Framework Core | EF Core 10.0, abordagem code-first |
| Autenticação | JWT | Integrada no backend |
| Frontend Web | React / TypeScript | React 19.3 / TypeScript 6.0 / Node.js 24 LTS |
| Construção Frontend Web | Vite / npm | Vite 8.3 / npm 12 |
| Frontend Mobile | Kotlin / Jetpack Compose (Android nativo) | Kotlin 2.2 / Jetpack Compose com Material 3 (Compose BOM 2026.02.01) / Android SDK API 37 (minSdk 26) / JDK 21 / Gradle 9.6 |
| Base de Dados | Microsoft SQL Server / T-SQL | MS SQL Server 2025 |
| Migrações de dados | EF Core Migrations | `dotnet-ef` |
| Diagramas técnicos | PlantUML | Fonte textual incluída nos documentos Markdown |
| Controlo de versões | Git / GitHub | Repositório único do projeto |

As tecnologias identificadas nesta secção correspondem aos compromissos técnicos definidos pelo Grupo G08 no Regulamento Interno do Grupo v02 (secção 11.2, de 21/10/2026).

---

## 7. Catálogo de componentes arquiteturais

Para efeitos de rastreabilidade, cada componente recebe um identificador estável que pode ser utilizado na RTM.

### 7.1. ARC-WEB — Frontend Web

**Tecnologia:** React 19.3 + TypeScript 6.0 (Node.js 24 LTS, Vite 8.3 e npm 12).

**Responsabilidades:**

- disponibilizar a interface utilizada pelos perfis Administrador, Rececionista, Veterinário e Auxiliar Veterinário;
- apresentar apenas as funcionalidades permitidas ao perfil autenticado;
- recolher dados introduzidos pelo utilizador;
- efetuar validações de interface que não substituem a validação obrigatória do backend;
- enviar pedidos ao Backend / API;
- apresentar os resultados e erros devolvidos pelo backend;
- suportar os fluxos web definidos nos casos de uso.

O Frontend Web não acede diretamente à base de dados e não é responsável pela decisão final de autorização de uma operação.

---

### 7.2. ARC-MOB — Frontend Mobile

**Tecnologia:** Android nativo em Kotlin 2.2 com Jetpack Compose e Material 3 (Android SDK API 37, minSdk 26).

**Responsabilidades:**

- disponibilizar a aplicação destinada exclusivamente ao Cliente;
- permitir autenticação e fim de sessão do Cliente;
- apresentar os dados básicos dos animais associados à conta autenticada;
- apresentar marcações futuras e histórico administrativo;
- apresentar boletim de vacinação e desparasitação;
- impedir, ao nível da interface, operações que não pertencem ao âmbito mobile;
- comunicar exclusivamente com o Backend / API para obter os dados autorizados.

O Frontend Mobile não permite administração de contas, alteração das fichas dos animais, atos clínicos ou gestão de marcações. As funcionalidades que passaram para o não-âmbito (F005 e F007), como despesas e pagamentos ou internamentos, não existem na aplicação.

---

### 7.3. ARC-API — Backend / API

**Tecnologia:** ASP.NET Core Web API 10.0 em C# 14 / .NET 10.

**Responsabilidades:**

- disponibilizar as operações necessárias ao Frontend Web e ao Frontend Mobile;
- receber e validar pedidos;
- coordenar a execução dos módulos funcionais;
- aplicar as regras de negócio;
- aplicar autenticação e autorização;
- impedir operações não permitidas ao perfil autenticado;
- garantir o isolamento dos dados dos Clientes;
- coordenar a persistência e consulta de dados;
- devolver resultados e erros às aplicações cliente.

O Backend / API constitui o ponto central de aplicação das regras funcionais e de segurança.

---

### 7.4. ARC-SEC — Autenticação e Autorização

**Tecnologia:** JWT e mecanismos de autorização do ASP.NET Core.

**Responsabilidades:**

- autenticar utilizadores através das credenciais associadas às contas;
- rejeitar autenticações inválidas ou contas sem acesso permitido;
- emitir e validar mecanismos de acesso baseados em JWT;
- identificar o perfil associado ao utilizador autenticado;
- aplicar controlo de acesso baseado nos cinco perfis do sistema;
- impedir operações não autorizadas;
- suportar o isolamento entre dados pertencentes a Clientes diferentes;
- consultar, através da camada de persistência, o estado das contas e a informação necessária à validação de tokens revogados.

As regras de autorização devem ser aplicadas no backend mesmo quando a interface já oculta uma funcionalidade não permitida.

---

### 7.5. ARC-F001 — Módulo de Autenticação, Contas e Configuração

**Origem funcional:** F001 — Autenticação e Controlos de Acesso.  
**Requisitos principais:** RF001–RF013.  
**Casos de uso principais:** UC01–UC05.

**Responsabilidades:**

- autenticação e fim de sessão;
- gestão de contas de funcionários;
- atribuição de perfis;
- bloqueio de contas;
- criação e atualização de contas de Clientes;
- consulta dos dados gerais e configurações da clínica;
- atualização dos dados gerais e configurações da clínica;
- aplicação das regras de acesso associadas a estas operações.

A operação **RF019 — Gerir horários de funcionamento** pertence funcionalmente ao módulo ARC-F003 — Agenda e Marcações e é executada pelo Administrador. O ARC-F001 assegura apenas a gestão da conta, do perfil e das permissões necessárias ao acesso a essa operação.

A coordenação dos casos de uso de autenticação e gestão de contas pertence ao ARC-F001; os mecanismos técnicos de emissão, validação e revogação de JWT são responsabilidade do ARC-SEC.

---

### 7.6. ARC-F002 — Módulo de Pacientes

**Origem funcional:** F002 — Gestão de pacientes.  
**Requisitos principais:** RF014–RF018.  
**Casos de uso principais:** UC06–UC08.

**Responsabilidades:**

- registar pacientes;
- consultar fichas administrativas;
- atualizar dados administrativos;
- pesquisar pacientes por proprietário;
- manter a associação entre paciente e Cliente;
- disponibilizar ao Cliente apenas os dados dos seus próprios animais.

Os registos clínicos não são responsabilidade deste módulo, sendo tratados pelo módulo ARC-F004.

---

### 7.7. ARC-F003 — Módulo de Agenda e Marcações

**Origem funcional:** F003 — Agendamento de consultas.  
**Requisitos principais:** RF019–RF029.  
**Casos de uso principais:** UC09–UC11.

**Responsabilidades:**

- considerar os horários de funcionamento na gestão da agenda;
- consultar disponibilidade dos Veterinários;
- criar e alterar marcações;
- atualizar o estado das marcações;
- disponibilizar a agenda da clínica;
- registar a chegada de Clientes e animais;
- disponibilizar informação necessária à preparação das consultas;
- guardar informação não clínica de apoio quando autorizada;
- disponibilizar ao Cliente marcações futuras e histórico administrativo.

**Regra de autorização:** a gestão dos horários de funcionamento é executada pelo Administrador. Apesar de o Administrador ser o ator responsável pela operação, esta responsabilidade permanece arquiteturalmente no módulo ARC-F003 por pertencer à funcionalidade F003 — Agendamento de consultas.

---

### 7.8. ARC-F004 — Módulo de Registo Clínico

**Origem funcional:** F004 — Registo clínico e tratamentos.  
**Requisitos principais:** RF030–RF036.  
**Casos de uso principais:** UC12–UC13.

**Responsabilidades:**

- registar notas clínicas;
- registar diagnósticos;
- registar tratamentos;
- registar prescrições;
- atualizar registos clínicos;
- disponibilizar histórico clínico ao Veterinário;
- disponibilizar ao Auxiliar Veterinário apenas a informação clínica autorizada.

A criação e alteração de informação clínica são reservadas ao Veterinário.

---

### 7.9. ARC-F005 — Módulo de Contas e Pagamentos (fora do âmbito)

**Origem funcional:** F005 — Gestão de contas e pagamentos.  
**Requisitos:** RF037–RF042, descontinuados na Especificação de Requisitos v02.  
**Casos de uso:** UC14–UC15, fora do âmbito nos Casos de Uso v02.

A funcionalidade F005 passou para o não-âmbito pela Declaração de Âmbito v05. O módulo deixa de fazer parte da arquitetura em vigor e o identificador `ARC-F005` fica reservado, sem ser reutilizado. A descrição que a v01 fazia deste módulo (custos, valores em dívida, registo manual de pagamentos, fecho de contas e consulta pelo Cliente) consta apenas como histórico, em `m2-documentacao-arquitetura-v01.md`.

---

### 7.10. ARC-F006 — Módulo de Vacinação e Desparasitação

**Origem funcional:** F006 — Boletim de Vacinas e Desparasitações.  
**Requisitos principais:** RF043–RF047.  
**Casos de uso principais:** UC16–UC17.

**Responsabilidades:**

- registar vacinação;
- registar desparasitação;
- calcular a data prevista para a próxima administração a partir da data e intervalo definidos;
- disponibilizar o boletim no sistema web;
- disponibilizar ao Cliente o boletim e próximas administrações previstas.

O módulo não integra bases de dados nacionais de saúde animal nem leitura automática de códigos de barras ou QR.

---

### 7.11. ARC-F007 — Módulo de Internamento (fora do âmbito)

**Origem funcional:** F007 — Gestão de internamento.  
**Requisitos:** RF048–RF052, descontinuados na Especificação de Requisitos v02.  
**Caso de uso:** UC18, fora do âmbito nos Casos de Uso v02.

A funcionalidade F007 passou para o não-âmbito pela Declaração de Âmbito v05. O módulo deixa de fazer parte da arquitetura em vigor e o identificador `ARC-F007` fica reservado, sem ser reutilizado. A descrição que a v01 fazia deste módulo (internamentos, alojamentos, painel de internamentos ativos, estado clínico e alta médica) consta apenas como histórico, em `m2-documentacao-arquitetura-v01.md`.

---

### 7.12. ARC-DATA — Camada de Persistência

**Tecnologia:** Entity Framework Core 10.0, abordagem code-first.

**Responsabilidades:**

- disponibilizar ao backend acesso persistente aos dados;
- traduzir as operações da aplicação para operações sobre a base de dados;
- suportar consultas e alterações requeridas pelos módulos funcionais;
- gerir a evolução do esquema através de EF Core Migrations;
- assegurar que dados confirmados permanecem disponíveis para consulta posterior.

A camada de persistência é utilizada pelo backend e não é acedida diretamente pelos frontends.

---

### 7.13. ARC-DB — Base de Dados `clinica-veterinaria`

**Tecnologia:** Microsoft SQL Server 2025 / T-SQL.

**Responsabilidades:**

- armazenar os dados persistentes necessários às funcionalidades em âmbito (F001 a F004 e F006);
- armazenar relações necessárias entre Clientes, pacientes e informação funcional associada;
- fornecer persistência ao backend através da camada ARC-DATA.

A definição das entidades, atributos, relações e cardinalidades pertence ao artefacto I016 — Modelo de Dados.

**Nota (v02):** o Modelo de Dados v01 e os scripts SQL `m3-v01` ainda incluem as tabelas de F005 e F007 (6 das 22 tabelas). Nenhum módulo, serviço, endpoint ou DTO as utiliza. O grupo decide se as mantém, com uma nota de âmbito, ou se as retira numa nova versão do modelo de dados e dos scripts (secção 20).

---

## 8. Organização lógica do Backend

A arquitetura interna prevista para o backend separa responsabilidades em quatro níveis lógicos.

### 8.1. Camada de API

Responsável por:

- receber pedidos das aplicações cliente;
- expor as operações documentadas na API;
- iniciar o processamento de cada pedido;
- devolver respostas e erros.

Esta camada não deve concentrar regras de negócio que pertençam aos serviços ou módulos funcionais.

### 8.2. Camada de Aplicação e Serviços

Responsável por:

- coordenar os casos de uso;
- aplicar regras de negócio;
- coordenar validações;
- utilizar os módulos ARC-F001 a ARC-F004 e ARC-F006;
- solicitar operações de persistência;
- garantir que as operações respeitam as permissões aplicáveis.

### 8.3. Camada de Domínio / Módulos Funcionais

Agrupa as responsabilidades correspondentes às cinco áreas funcionais em âmbito:

- ARC-F001 — Autenticação, Contas e Configuração;
- ARC-F002 — Pacientes;
- ARC-F003 — Agenda e Marcações;
- ARC-F004 — Registo Clínico;
- ARC-F006 — Vacinação e Desparasitação.

Esta divisão é lógica e destina-se a manter separação de responsabilidades e rastreabilidade. Não implica a criação de serviços independentes. ARC-F005 e ARC-F007 estão fora do âmbito (Declaração de Âmbito v05); os identificadores ficam reservados e não são reutilizados.

### 8.4. Camada de Infraestrutura e Persistência

Responsável por:

- implementação do acesso a dados;
- integração com Entity Framework Core;
- configuração de persistência;
- migrações;
- comunicação com Microsoft SQL Server.

---

## 9. Interfaces e comunicação entre componentes

| Origem | Destino | Interface | Finalidade |
|---|---|---|---|
| ARC-WEB | ARC-API | API Web através da rede | Executar as operações permitidas aos perfis internos |
| ARC-MOB | ARC-API | API Web através da rede | Autenticação e consultas permitidas ao Cliente |
| ARC-API | ARC-SEC | Serviços internos do backend | Autenticação, identificação de perfil e autorização |
| ARC-API | ARC-F001 a ARC-F004 e ARC-F006 | Chamadas internas | Executar regras e casos de uso |
| ARC-F001 a ARC-F004 e ARC-F006 | ARC-DATA | Serviços de persistência | Consultar e alterar dados persistentes |
| ARC-DATA | ARC-DB | EF Core / SQL Server | Persistir e recuperar dados |

A definição concreta de métodos, rotas, parâmetros, pedidos, respostas e códigos de erro é realizada no artefacto I018 — Documentação de API.

Não existem interfaces diretas entre os frontends e ARC-DB.

---

## 10. Autenticação, autorização e separação de perfis

O backend utiliza autenticação baseada em JWT, de acordo com as tecnologias assumidas pelo grupo.

Após autenticação, o perfil associado à conta é utilizado para determinar as operações autorizadas.

A autorização deve ser aplicada no backend e não pode depender exclusivamente da ocultação de opções nas interfaces.

### 10.1. Perfis e canais

| Perfil | Canal |
|---|---|
| Administrador | Frontend Web |
| Rececionista | Frontend Web |
| Veterinário | Frontend Web |
| Auxiliar Veterinário | Frontend Web |
| Cliente | Frontend Mobile |

### 10.2. Regras arquiteturais de autorização

- o Administrador gere funcionários, perfis, contas de Clientes, configurações e horários de funcionamento da clínica, mas não acede a registos clínicos;
- o Rececionista gere informação administrativa e agenda, mas não cria nem altera registos clínicos;
- o Veterinário consulta apenas as consultas que lhe estão atribuídas e gere informação clínica e vacinação;
- o Auxiliar Veterinário possui apenas as permissões de consulta previstas e só pode registar informação não clínica de apoio quando essa autorização está ativa na respetiva conta;
- o Cliente apenas acede às funcionalidades mobile e aos dados associados à sua própria conta;
- pedidos não autorizados devem ser rejeitados pelo backend mesmo que tenham sido enviados fora das interfaces previstas.

---

## 11. Validação de dados

As aplicações cliente podem realizar validações de interface para melhorar a interação com o utilizador.

Contudo, o Backend / API é responsável pela validação autoritativa antes de aceitar operações de criação ou alteração.

A validação deve considerar:

- presença dos dados obrigatórios;
- formato e restrições aplicáveis;
- existência das entidades relacionadas quando necessária;
- permissões do utilizador;
- regras de negócio associadas ao módulo funcional.

Dados inválidos não devem produzir alterações persistentes.

---

## 12. Persistência e consistência dos dados

Toda a persistência da aplicação é coordenada pelo Backend / API através de ARC-DATA.

Os frontends não mantêm uma fonte de dados autónoma que substitua a base de dados do sistema.

Depois de uma operação válida ser confirmada e persistida com sucesso, os dados devem permanecer disponíveis para consulta posterior.

A definição física das estruturas de armazenamento é responsabilidade do Modelo de Dados e das migrações associadas ao backend.

---

## 13. Rastreabilidade funcional da arquitetura

| Funcionalidade | Requisitos | Casos de uso | Componentes principais |
|---|---|---|---|
| F001 — Autenticação e Controlos de Acesso | RF001–RF013 | UC01–UC05 | ARC-WEB, ARC-MOB, ARC-API, ARC-SEC, ARC-F001, ARC-DATA, ARC-DB |
| F002 — Gestão de pacientes | RF014–RF018 | UC06–UC08 | ARC-WEB, ARC-MOB, ARC-API, ARC-F002, ARC-DATA, ARC-DB |
| F003 — Agendamento de consultas | RF019–RF029 | UC09–UC11 | ARC-WEB, ARC-MOB, ARC-API, ARC-F003, ARC-DATA, ARC-DB |
| F004 — Registo clínico e tratamentos | RF030–RF036 | UC12–UC13 | ARC-WEB, ARC-API, ARC-F004, ARC-DATA, ARC-DB |
| F005 — Gestão de contas e pagamentos | RF037–RF042 (descontinuados) | UC14–UC15 (fora do âmbito) | Fora do âmbito (Declaração de Âmbito v05); ARC-F005 reservado |
| F006 — Boletim de Vacinas e Desparasitações | RF043–RF047 | UC16–UC17 | ARC-WEB, ARC-MOB, ARC-API, ARC-F006, ARC-DATA, ARC-DB |
| F007 — Gestão de internamento | RF048–RF052 (descontinuados) | UC18 (fora do âmbito) | Fora do âmbito (Declaração de Âmbito v05); ARC-F007 reservado |

A tabela representa rastreabilidade de alto nível. Na RTM, cada requisito pode referenciar apenas os componentes efetivamente envolvidos na respetiva operação.

---

## 14. Rastreabilidade dos requisitos não funcionais

| Requisito | Componentes arquiteturais relacionados | Tratamento arquitetural |
|---|---|---|
| RNF001 — Controlo de acesso às operações | ARC-API, ARC-SEC, ARC-F001 a ARC-F004 e ARC-F006 | Autorização centralizada no backend de acordo com o perfil |
| RNF002 — Isolamento dos dados de Clientes | ARC-MOB, ARC-API, ARC-SEC, ARC-DATA, ARC-DB | O backend restringe o acesso aos dados associados ao Cliente autenticado |
| RNF003 — Separação dos canais de utilização | ARC-WEB, ARC-MOB, ARC-API, ARC-SEC | Perfis internos utilizam Web; Cliente utiliza Mobile |
| RNF004 — Validação dos dados de entrada | ARC-WEB, ARC-MOB, ARC-API, ARC-F001 a ARC-F004 e ARC-F006 | A interface pode validar, mas o backend efetua a validação autoritativa |
| RNF005 — Persistência dos dados confirmados | ARC-API, ARC-DATA, ARC-DB | Operações confirmadas são persistidas através de EF Core / SQL Server |
| RNF006 — Dependência de ligação à rede | ARC-WEB, ARC-MOB, ARC-API | A utilização das aplicações depende de comunicação com o backend; não existe modo offline garantido |

---

## 15. Restrições e elementos fora do âmbito

A arquitetura não prevê os seguintes elementos:

- a gestão de contas e pagamentos (F005) e a gestão de internamento (F007), que passaram para o não-âmbito pela Declaração de Âmbito v05;
- fornecedores externos de identidade, como Google ou Facebook;
- autenticação multifator;
- integração com SIAC ou outras bases de dados nacionais de microchips;
- sincronização com calendários externos;
- envio automático de SMS ou e-mail para lembretes;
- geração ou envio automático de prescrições em PDF;
- integrações para execução de pagamentos eletrónicos, MB Way, referências Multibanco ou TPA;
- certificação fiscal e comunicação direta com a Autoridade Tributária;
- leitura automática de códigos de barras ou QR de vacinas;
- bases de dados nacionais de saúde animal;
- integração com monitores de sinais vitais, câmaras, balanças, leitores de microchips ou equipamentos de diagnóstico por imagem;
- operação offline garantida;
- colocação efetiva do sistema em produção numa clínica real nesta fase;
- migração de dados históricos ou clínicos provenientes de aplicações anteriores.

Estas exclusões não devem ser introduzidas por decisões de arquitetura, modelos, protótipos ou implementação.

---

## 16. Decisões arquiteturais

### DA-ARQ-01 — Separação entre Web e Mobile

Os utilizadores internos utilizam o Frontend Web e o Cliente utiliza exclusivamente o Frontend Mobile.

**Justificação:** corresponde diretamente ao âmbito homologado e ao requisito de separação dos canais de utilização.

### DA-ARQ-02 — Backend como ponto central das regras de negócio

As regras de negócio, validações autoritativas e verificações de permissões são executadas no Backend / API.

**Justificação:** evita depender das interfaces para garantir segurança e mantém comportamento consistente entre os dois canais.

### DA-ARQ-03 — Sem acesso direto dos frontends à base de dados

Frontend Web e Frontend Mobile comunicam apenas com o Backend / API.

**Justificação:** centraliza segurança, regras de negócio e persistência.

### DA-ARQ-04 — Organização lógica por funcionalidades em âmbito (F001–F004 e F006)

O backend é organizado logicamente em módulos correspondentes às cinco funcionalidades em âmbito (ARC-F001 a ARC-F004 e ARC-F006).

**Justificação:** melhora separação de responsabilidades e permite rastreabilidade direta com requisitos e casos de uso.

### DA-ARQ-05 — Persistência relacional com EF Core e SQL Server

O backend utiliza Entity Framework Core 10.0 em abordagem code-first sobre Microsoft SQL Server 2025.

**Justificação:** corresponde às tecnologias formalmente adotadas pelo grupo no Regulamento Interno do Grupo v02.

### DA-ARQ-06 — Autenticação JWT e autorização por perfil

A autenticação é suportada por JWT assinado com HMAC-SHA256, com validade de 8 horas e sem mecanismo de refresh token. O fim de sessão invalida o token atual através do respetivo identificador `jti`, mantido como revogado até à expiração. A autorização respeita os cinco perfis previstos e, quando aplicável, regras de propriedade sobre os dados.

**Justificação:** centraliza autenticação e autorização no backend, mantém o controlo de acesso independente das interfaces e assegura coerência com os requisitos e com a Documentação de API.

### DA-ARQ-07 — Dependência de rede

A arquitetura não implementa sincronização ou funcionamento offline.

**Justificação:** o âmbito homologado exige ligação constante à rede e não garante funcionamento offline.

### DA-ARQ-08 — Diagramas versionáveis em PlantUML

Os diagramas arquiteturais são mantidos em código PlantUML no próprio documento Markdown.

**Justificação:** preserva uma representação textual, editável e versionável.

### DA-ARQ-09 — Gestão dos horários de funcionamento pelo Administrador

A operação RF019 — Gerir horários de funcionamento é atribuída ao perfil Administrador e permanece integrada no módulo ARC-F003. A configuração pode conter vários períodos no mesmo dia, desde que não se sobreponham.

**Justificação:** o grupo decidiu atribuir ao Administrador a responsabilidade pela configuração dos horários de funcionamento, mantendo a operação dentro de F003 por afetar diretamente a determinação das disponibilidades da agenda.

### DA-ARQ-10 — F005 e F007 fora do âmbito, sem módulo

Os módulos ARC-F005 e ARC-F007 deixam de existir na arquitetura em vigor. Os identificadores ficam reservados e não são reutilizados.

**Justificação:** Declaração de Âmbito v05, que passou F005 e F007 para o não-âmbito por transição autorizada pelo docente a 20/10/2026 e homologada por e-mail a 21/10/2026.

### DA-ARQ-11 — Tecnologias do Regulamento Interno do Grupo v02

O backend usa .NET 10, o frontend web usa React 19.3 com TypeScript 6.0 e o frontend mobile é Android nativo em Kotlin com Jetpack Compose.

**Justificação:** secção 11.2 do Regulamento Interno do Grupo v02 (21/10/2026), que substituiu as tecnologias da v01 (.NET 8, React 18.2 e Flutter).

---

## 17. Diagrama geral de componentes

```plantuml {kroki=true}
@startuml
left to right direction
skinparam componentStyle rectangle

actor Administrador as Admin
actor Rececionista as Rec
actor Veterinario as Vet
actor "Auxiliar Veterinario" as Aux
actor "Cliente" as Cli

component "ARC-WEB\nFrontend Web\nReact 19.3 + TypeScript 6.0" as WEB
component "ARC-MOB\nFrontend Mobile\nKotlin 2.2 + Jetpack Compose" as MOB

rectangle "Backend / API (.NET 10)" {
  component "ARC-API\nASP.NET Core Web API" as API
  component "ARC-SEC\nAutenticacao e Autorizacao\nJWT + perfis" as SEC

  package "Modulos Funcionais" {
    component "ARC-F001\nAutenticacao, Contas\ne Configuracao" as F001
    component "ARC-F002\nPacientes" as F002
    component "ARC-F003\nAgenda e Marcacoes" as F003
    component "ARC-F004\nRegisto Clinico" as F004
    component "ARC-F006\nVacinacao e\nDesparasitacao" as F006
  }

  component "ARC-DATA\nPersistencia\nEF Core 10.0" as DATA
}

database "ARC-DB\nclinica-veterinaria\nMS SQL Server 2025" as DB

Admin --> WEB
Rec --> WEB
Vet --> WEB
Aux --> WEB
Cli --> MOB

WEB --> API : API atraves da rede
MOB --> API : API atraves da rede

API --> SEC
SEC --> DATA : estado da conta / tokens revogados
API --> F001
API --> F002
API --> F003
API --> F004
API --> F006

F001 --> DATA
F002 --> DATA
F003 --> DATA
F004 --> DATA
F006 --> DATA

DATA --> DB
@enduml
```

---

## 18. Diagrama lógico do Backend

```plantuml
@startuml
top to bottom direction
skinparam componentStyle rectangle

package "Backend .NET 10" {
  component "Camada de API\nControllers / entrada de pedidos" as API_LAYER
  component "ARC-SEC\nAutenticacao e Autorizacao" as SEC

  component "Camada de Aplicacao e Servicos\nCoordenacao dos casos de uso" as APP

  package "Dominio / Modulos Funcionais" {
    component "ARC-F001" as F001
    component "ARC-F002" as F002
    component "ARC-F003" as F003
    component "ARC-F004" as F004
    component "ARC-F006" as F006
  }

  component "ARC-DATA\nInfraestrutura e Persistencia\nEF Core 10.0" as DATA
}

database "ARC-DB\nMS SQL Server 2025" as DB

API_LAYER --> SEC : autenticar / autorizar
SEC --> DATA : estado da conta / revogacao JWT
API_LAYER --> APP : pedido autorizado

APP --> F001
APP --> F002
APP --> F003
APP --> F004
APP --> F006

F001 --> DATA
F002 --> DATA
F003 --> DATA
F004 --> DATA
F006 --> DATA

DATA --> DB
@enduml
```

---

## 19. Organização prevista no repositório

De acordo com a estratégia de repositório adotada pelo grupo, os principais componentes são mantidos no mesmo repositório, com separação por diretório:

```text
/src/backend
/src/frontend
/src/frontend-mobile
/docs
/projeto
```

A documentação técnica e os diagramas versionáveis são preservados no repositório juntamente com os restantes artefactos do projeto.

---

## 20. Pontos em aberto

| N.º | Ponto | Decisão necessária |
|---|---|---|
| 1 | O Modelo de Dados v01 e os scripts SQL `m3-v01` ainda incluem as tabelas de F005 e F007 (6 das 22 tabelas), embora a Declaração de Âmbito v05 as tenha passado para o não-âmbito. | O grupo decide se a base de dados mantém essas tabelas (sem as expor) ou se são retiradas numa nova versão do modelo de dados e dos scripts. Se se mantiverem, a Issue I059 (GitHub #16) tem de registar que os critérios de aceitação 1, 3 e 4 se aplicam só às 16 tabelas em âmbito. |

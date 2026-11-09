# Plano de Testes do Frontend Web — Gestão de Clínica Veterinária

| Campo                | Valor                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| -------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Grupo                | G08                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Projeto              | Gestão de Clínica Veterinária                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| Artefacto            | Plano de Testes do Frontend Web (`OF-M4-011`)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| Ficheiro             | `m4-plano-testes-frontend-web-v01.md`                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| Versão               | v01                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Data                 | 2026-11-09                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| Milestone de Entrega | m4                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| Issue                | I043 — Elaborar o Plano de Testes do Frontend Web (Sprint 06)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| Papéis               | Executor: Gonçalo Dinis Magalhães Teixeira · Revisor: Ivan Cristiano Silva Mendes · Auditor: Gonçalo Magalhães Ribeiro                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| Pasta de destino     | `/projeto/03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.01-plano-de-testes`                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| Fontes               | `m3-declaracao-ambito-v05.pdf`; `m3-especificacao-requisitos-v02.md` (I012); `m3-especificacoes-casos-uso-v02.md` (I015); `m3-documentacao-api-v03.md` (I018 e deliberações 10 a 12 da ata do Sprint Review do Sprint 05); `m3-modelo-classes-frontend-web-v03.md` (I021); `m3-prototipo-baixa-fidelidade-v02.md` (I017); `m1-plano-qualidade-processo-v01.pdf` (I006) e `m3-plano-qualidade-processo-v02.md`; `m3-checklist-controlo-artefactos-v08.xlsx`; `m3-plano-testes-backend-v03.md`; `m3-manual-instalacao-configuracao-v01.md` (I038); `m4-backlog-projeto-v04.xlsx`; base do frontend web em `src/frontend` (`package.json`, `vite.config.ts`, `playwright.config.ts`) |

## Histórico de versões

| Versão | Data       | Autor                            | Descrição                                                                                                                                                                                                                                                                                                                                                           |
| ------ | ---------- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| v01    | 2026-11-09 | Gonçalo Dinis Magalhães Teixeira | Versão inicial: âmbito, estratégia e níveis de teste, ferramentas e versões, ambiente e dados, comandos locais e na pipeline, critérios de entrada e de saída, quality gates, decisões sobre os artefactos facultativos, condições de teste por funcionalidade, gestão de defeitos, responsabilidades, riscos e rastreabilidade entre requisitos e níveis de teste. |

## Índice

1. Introdução
2. Documentos de referência
3. Âmbito
4. Estratégia e níveis de teste
5. Ferramentas e versões
6. Ambiente e dados de teste
7. Execução dos testes: comandos locais e na pipeline
8. Critérios de entrada e de saída
9. Quality gates e cobertura
10. Condições de teste prioritárias por funcionalidade
11. Testes de aceitação (simulação)
12. Gestão de defeitos
13. Responsabilidades e calendário
14. Riscos
15. Rastreabilidade
16. Coerência com o Plano de Qualidade e com a CCA
17. Decisões tomadas

---

## 1. Introdução

Este plano define como o Grupo G08 verifica o frontend web do sistema de Gestão de Clínica Veterinária na Milestone de Entrega m4: o que é testado, a que nível, com que ferramentas, em que ambiente, com que dados, quando um teste pode começar e quando o frontend web se considera pronto para entrega.

O frontend web é a aplicação usada pelos quatro perfis internos (Administrador, Rececionista, Veterinário e Auxiliar Veterinário). O perfil Cliente usa exclusivamente a aplicação móvel (RNF003), que tem plano de testes próprio (I047).

O plano complementa o Plano de Testes do Backend (`m3-plano-testes-backend-v03.md`): as regras de negócio, a autorização no servidor e a persistência são verificadas no backend; aqui verifica-se que a interface web as usa corretamente, mostra apenas o que cada perfil pode fazer e trata as respostas da API.

## 2. Documentos de referência

| Documento                                                | Uso neste plano                                                                                                      |
| -------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------- |
| Especificação de Requisitos v02 (I012)                   | Requisitos funcionais e não funcionais em âmbito e respetivos atores (secções 5 e 6)                                 |
| Especificações de Casos de Uso v02 (I015)                | Fluxos principais e alternativos usados nas condições de teste e nos cenários de aceitação                           |
| Documentação da API v03                                  | Endpoints usados pelo frontend web (secção 12.2), formato de erros `{ code, message, detail }` e respostas esperadas |
| Modelo de Classes do Frontend Web v03 (I021)             | Organização da aplicação (páginas, serviços, guarda de rotas por perfil)                                             |
| Protótipo de baixa fidelidade v02 (I017)                 | Ecrãs W01 a W13 e W16                                                                                                |
| Plano de Qualidade do Processo (I006, v01 e v02)         | Objetivos de qualidade, quality gates QG-01, QG-04, QG-06 e QG-08, métricas MQ-03 e MQ-04                            |
| Checklist de Controlo de Artefactos v08                  | Artefactos obrigatórios e condicionais do frontend web                                                               |
| Plano de Testes do Backend v03                           | Estrutura do plano e divisão de responsabilidades entre backend e frontend                                           |
| Manual de Instalação e Configuração do backend (I038)    | Arranque do backend e dos dados de demonstração usados nos testes                                                    |
| Backlog de Projeto v04                                   | Tarefas de desenvolvimento e de teste do frontend web e respetivos critérios                                         |
| Regulamento de Funcionamento da UC v19 (secções 17 e 19) | Artefactos de teste exigidos e nomes e pastas dos ficheiros                                                          |

## 3. Âmbito

### 3.1. Dentro do âmbito

- **Requisitos funcionais suportados pela aplicação web:** 37 requisitos das funcionalidades F001, F002, F003, F004 e F006, cujos atores incluem pelo menos um perfil interno: RF001–RF017, RF019–RF027, RF030–RF036 e RF043–RF046 (lista completa na secção 15).
- **Requisitos não funcionais com verificação na interface web:** RNF001 (controlo de acesso), RNF003 (separação de canais), RNF004 (validação dos dados de entrada), RNF005 (persistência dos dados confirmados) e RNF006 (dependência de ligação à rede).
- **Perfis:** Administrador, Rececionista, Veterinário e Auxiliar Veterinário; o Cliente só entra para confirmar que não tem acesso à aplicação web (RNF003).
- **Integração com a API:** os endpoints usados pelo frontend web segundo a Documentação da API v03, sempre contra o backend real (sem simulação da API nos testes de sistema).
- **Comportamento transversal da interface:** menus e rotas por perfil, acesso por endereço direto, sessão expirada ou terminada, falha de rede, estados de carregamento e erro e apresentação das mensagens de erro da API.

### 3.2. Fora do âmbito

| Elemento                                                                                                                       | Motivo                                                                                                                                                                                             |
| ------------------------------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| F005 — Gestão de contas e pagamentos (RF037–RF042) e F007 — Internamento (RF048–RF052); ecrãs W14, W15, W17 e W18 do protótipo | Fora do âmbito pela Declaração de Âmbito v05; não são tratados como funcionalidades ativas nem têm cenários de teste                                                                               |
| RF018, RF028, RF029 e RF047                                                                                                    | Requisitos do Cliente, exclusivos da aplicação móvel (RNF003); cobertos pelo Plano de Testes do Frontend Mobile (I047)                                                                             |
| RF053 e RF054 (Cliente sem conta de acesso) e RF055 (horários regulares por Veterinário)                                       | Não existem na Especificação de Requisitos v02 (ponto em aberto 1); excluídos das I110 e I111 até serem especificados e aprovados (deliberações 5 e 12 da ata do Backlog Refinement de 2026-11-07) |
| RF056 / EP-052 (atualização de uma administração)                                                                              | Sem requisito nem caso de uso aprovados (deliberação 12 da ata do Backlog Refinement de 2026-11-07); só entra no plano depois de especificado. O estado é registado na RTM final (I051)            |
| Regras de negócio, autorização e persistência no servidor                                                                      | Verificadas pelos testes unitários e de integração do backend e pela Collection Postman (Plano de Testes do Backend v03)                                                                           |
| Testes de desempenho, carga, segurança ofensiva e acessibilidade formal                                                        | Não exigidos pelos requisitos aprovados nem pela CCA                                                                                                                                               |
| Funcionamento offline                                                                                                          | Fora do âmbito (Especificação de Requisitos v02, secção 7; RNF006)                                                                                                                                 |
| Navegadores além do Chromium                                                                                                   | Ver decisão D-03                                                                                                                                                                                   |

## 4. Estratégia e níveis de teste

| Nível                             | O que verifica                                                                      | Quando                                                    | Ferramenta                                           | Artefacto                                                                              | Obrigatório                |
| --------------------------------- | ----------------------------------------------------------------------------------- | --------------------------------------------------------- | ---------------------------------------------------- | -------------------------------------------------------------------------------------- | -------------------------- |
| Verificação estática              | Tipos, regras de lint e formatação                                                  | Em cada push e Merge Request                              | `tsc`, ESLint com typescript-eslint, Prettier        | Relatório de análise estática (`OF-M4-016`, I046)                                      | Sim (QG-01, QG-04, QG-08)  |
| Verificação manual por tarefa     | Critérios de aceitação de cada tarefa de desenvolvimento, por perfil e por operação | Antes de abrir o Merge Request                            | Navegador contra o backend com dados de demonstração | Registo da verificação manual e capturas no Merge Request (evidências das I108 a I118) | Sim (critérios do Backlog) |
| Testes de sistema automáticos     | Fluxos completos de cada requisito web, por perfil, contra o backend real           | Em cada Merge Request (pipeline) e antes do Sprint Review | Playwright                                           | Especificação (`OF-M4-012`, I128) e Resultados (`OF-M4-013`, I133)                     | Sim (QG-06)                |
| Testes de aceitação (simulação)   | Utilização da aplicação por perfil, em cenários de negócio de ponta a ponta         | No Sprint 08, sobre a versão candidata à entrega          | Sessão guiada com o grupo a representar os perfis    | Especificação (`OF-M4-014`, I134) e Relatório da simulação (`OF-M4-015`, I135)         | Sim                        |
| Testes unitários e de componentes | Lógica local não trivial (validações de formulários, formatação, guarda de rotas)   | Quando o Executor os considere úteis                      | Vitest e React Testing Library                       | Nenhum (decisão D-04)                                                                  | Não                        |

### 4.1. Convenção de identificadores

| Prefixo                         | Uso                                                | Exemplo          |
| ------------------------------- | -------------------------------------------------- | ---------------- |
| `CT-WEB-<funcionalidade>-<n.º>` | Condição de teste definida neste plano (secção 10) | `CT-WEB-F001-03` |
| `ST-WEB-<funcionalidade>-<n.º>` | Cenário de teste de sistema (definido na I128)     | `ST-WEB-F003-07` |
| `AT-WEB-<n.º>`                  | Cenário de aceitação (definido na I134)            | `AT-WEB-04`      |
| `CT-WEB-TRV-<n.º>`              | Condição transversal (perfis, sessão, rede, erros) | `CT-WEB-TRV-02`  |

Cada cenário indica o requisito ou requisitos que verifica, o perfil e o caso de uso de origem.

## 5. Ferramentas e versões

Versões fixadas em `src/frontend/package.json` (base criada na I107).

| Ferramenta                      | Versão                                                                        | Uso                                                |
| ------------------------------- | ----------------------------------------------------------------------------- | -------------------------------------------------- |
| Node.js                         | 24 (LTS)                                                                      | Execução das ferramentas e da pipeline             |
| React                           | 19.3                                                                          | Aplicação                                          |
| TypeScript                      | 6.0                                                                           | Linguagem e verificação de tipos (`tsc -b`)        |
| Vite                            | 8.3                                                                           | Servidor de desenvolvimento e build                |
| Playwright (`@playwright/test`) | 1.64                                                                          | Testes de sistema automáticos (QG-06)              |
| Vitest                          | 5.0                                                                           | Testes unitários facultativos (D-04)               |
| React Testing Library           | 16.3 (`user-event` 14.6, `jest-dom` 7.0)                                      | Testes de componentes facultativos (D-04)          |
| jsdom                           | 30.1                                                                          | Ambiente dos testes Vitest                         |
| ESLint                          | 10.13, com typescript-eslint 8.71 e eslint-plugin-react-hooks 7.1             | Análise estática (QG-04)                           |
| Prettier                        | 3.9                                                                           | Formatação (QG-08)                                 |
| GitHub Actions                  | `frontend-ci.yml` (I142)                                                      | Pipeline do frontend web                           |
| Backend de teste                | .NET 10 e SQL Server 2025, como no Manual de Instalação e Configuração (I038) | API real contra a qual correm os testes de sistema |

Qualquer alteração de versão destas ferramentas durante a m4 é registada no Merge Request correspondente e, se afetar os testes, numa versão nova deste plano.

## 6. Ambiente e dados de teste

### 6.1. Ambientes

| Ambiente                      | Frontend                                      | Backend e base de dados                                                                                                                                                           | Uso                                                                              |
| ----------------------------- | --------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------- |
| Local                         | `npm run dev` (Vite, `http://localhost:5173`) | Backend em `http://localhost:5159`, arrancado segundo o Manual de Instalação e Configuração (I038), com a base `ClinicaVeterinaria` reposta e os dados de demonstração carregados | Verificação manual por tarefa e execução local dos testes de sistema             |
| Pipeline                      | Build do frontend servido na própria execução | Backend e SQL Server criados na execução, sem instâncias partilhadas (critério 2 da I142), com a mesma reposição e os mesmos dados de demonstração                                | Testes de sistema em cada Merge Request (QG-06)                                  |
| Ambiente público de avaliação | Publicação do frontend na m4 (I145)           | Backend público (I104)                                                                                                                                                            | Apenas ensaio do deployment e demonstração; não substitui a execução na pipeline |

O endereço da API é configurado por variável de ambiente do Vite (definida na I107), nunca escrito no código. A origem do frontend (`http://localhost:5173` e a da pipeline) tem de constar da configuração `Cors:AllowedOrigins` do backend.

### 6.2. Dados de teste

- **Contas de demonstração** criadas pelos scripts de dados de demonstração do backend (`04.13-scripts-de-base-de-dados`):

| Perfil                 | Conta                                                      | Uso principal                                                                          |
| ---------------------- | ---------------------------------------------------------- | -------------------------------------------------------------------------------------- |
| Administrador          | `admin@clinica-g08.test`                                   | Funcionários, clínica e horários                                                       |
| Rececionista           | `rececao@clinica-g08.test`                                 | Clientes, pacientes, marcações, agenda e chegada                                       |
| Rececionista bloqueado | `rececao.bloqueada@clinica-g08.test`                       | Autenticação rejeitada (RF002, RF009)                                                  |
| Veterinário            | `vet.ana@clinica-g08.test`, `vet.bruno@clinica-g08.test`   | Registo clínico, histórico e boletim; acesso a registos de colegas                     |
| Auxiliar Veterinário   | `aux.carla@clinica-g08.test`, `aux.diogo@clinica-g08.test` | Preparação da consulta e informação clínica autorizada; com e sem autorização de apoio |
| Cliente                | `cliente.joao@clinica-g08.test`                            | Confirmar que o Cliente não tem acesso à aplicação web (RNF003)                        |

- **Palavras-passe:** nunca ficam no repositório nem nos relatórios. Localmente são lidas de variáveis de ambiente; na pipeline vêm de _secrets_ do GitHub.
- **Dados criados pelos testes:** cada cenário cria os dados de que precisa com identificadores únicos (prefixo do cenário e marca temporal) e não depende de dados criados por outro cenário nem da ordem de execução.
- **Reposição:** antes de cada execução completa, a base é reposta com o script de reposição do backend (I062), para que os resultados sejam reproduzíveis.
- **Dados fictícios:** todos os dados são fictícios, como exige o âmbito académico.

## 7. Execução dos testes: comandos locais e na pipeline

### 7.1. Localmente (em `src/frontend`)

```powershell
# 0. Instalar as dependências exatamente como no package-lock.json
npm ci

# 1. Verificar a formatação (QG-08)
npm run format:check

# 2. Análise estática (QG-04): zero avisos e zero erros
npm run lint

# 3. Compilar (QG-01): tsc -b e vite build
npm run build

# 4. Testes unitários facultativos (Vitest), quando existam
npm run test

# 5. Testes de sistema (QG-06), com o backend local em execução
npx playwright install chromium
npm run test:e2e -- --project=chromium
```

### 7.2. Na pipeline

O workflow `.github/workflows/frontend-ci.yml` (I142) corre em cada push e Merge Request que altere `src/frontend` e executa, por esta ordem:

1. instalação das dependências (`npm ci`);
2. formatação (QG-08) e análise estática (QG-04);
3. build (QG-01);
4. arranque do SQL Server e do backend criados na execução, com reposição da base e dados de demonstração;
5. testes de sistema Playwright no Chromium (QG-06);
6. publicação do relatório HTML do Playwright e, em caso de falha, dos traces e capturas como artefactos da execução.

Uma falha em qualquer destes passos bloqueia o merge, tal como no backend.

## 8. Critérios de entrada e de saída

### 8.1. Critérios de entrada

**Verificação manual de uma tarefa (antes do Merge Request):**

- a funcionalidade compila e passa no lint e na formatação localmente;
- o backend local está em execução com os dados de demonstração;
- os endpoints usados existem na Documentação da API v03 e no backend em `develop`.

**Testes de sistema automáticos:**

- a especificação dos testes de sistema (I128) está aprovada;
- as funcionalidades a testar estão integradas em `develop`;
- a pipeline do frontend web (I142) ou o ambiente local descrito em 6.1 está operacional;
- as contas de demonstração e os _secrets_ estão disponíveis.

**Testes de aceitação (simulação):**

- todos os testes de sistema da versão candidata passaram;
- a especificação dos testes de aceitação (I134) está aprovada;
- existe uma versão identificada (commit) do frontend e do backend para a sessão.

### 8.2. Critérios de saída

O frontend web considera-se pronto para entrega quando:

1. todos os testes de sistema automáticos passam na pipeline (MQ-03 = 100%, QG-06);
2. cada requisito web da secção 15 tem pelo menos um cenário de sistema positivo a passar e, quando aplicável, um negativo;
3. os cenários de controlo de acesso por perfil, incluindo o acesso por endereço direto a rotas de outro perfil, passam para os quatro perfis internos e para o Cliente;
4. QG-01, QG-04 e QG-08 estão cumpridos na versão entregue;
5. não existe nenhum defeito válido por corrigir (MQ-04 = 100%);
6. a simulação de aceitação está concluída e todos os desvios têm decisão registada no relatório (I135);
7. os relatórios de resultados (I133), de aceitação (I135) e de análise estática (I046) estão arquivados nas pastas previstas.

## 9. Quality gates e cobertura

| ID      | Âmbito | Verificação                   | Comando                             | Limite                      | Efeito              |
| ------- | ------ | ----------------------------- | ----------------------------------- | --------------------------- | ------------------- |
| `QG-01` | Web    | Compilação                    | `npm run build`                     | Zero erros                  | Bloqueia a pipeline |
| `QG-04` | Web    | Análise estática              | `npm run lint` (`--max-warnings 0`) | Zero avisos e zero erros    | Bloqueia o merge    |
| `QG-06` | Web    | Testes de sistema automáticos | `npm run test:e2e` (Chromium)       | 100% dos testes a passar    | Bloqueia o merge    |
| `QG-08` | Web    | Formatação                    | `npm run format:check`              | Zero ficheiros por formatar | Bloqueia o merge    |

**Cobertura:** não é assumido um limiar mínimo de cobertura no frontend web (Plano de Qualidade v02, objetivo 5). A garantia de qualidade da interface vem dos testes de sistema contra o backend real e da cobertura de 100% dos requisitos web por cenários (critério de saída 2), não da percentagem de linhas executadas. Ver decisão D-05.

## 10. Condições de teste prioritárias por funcionalidade

As condições seguintes são o ponto de partida da especificação dos testes de sistema (I128), que as detalha em cenários com pré-condições, dados, passos e resultado esperado.

### 10.1. F001 — Autenticação, contas e clínica

| ID             | Condição                                                                                                                              | Requisitos           |
| -------------- | ------------------------------------------------------------------------------------------------------------------------------------- | -------------------- |
| CT-WEB-F001-01 | Cada perfil interno autentica-se com credenciais válidas e vê apenas o menu do seu perfil                                             | RF001, RF005         |
| CT-WEB-F001-02 | Credenciais inválidas e conta bloqueada são rejeitadas com mensagem, sem acesso                                                       | RF002, RF009         |
| CT-WEB-F001-03 | Terminar sessão invalida o acesso; voltar atrás no navegador não reabre páginas protegidas                                            | RF003, RF004         |
| CT-WEB-F001-04 | A sessão mantém-se ao navegar e ao recarregar enquanto o token for válido; com token expirado ou revogado, a aplicação volta ao login | RF004                |
| CT-WEB-F001-05 | O Administrador cria, consulta e atualiza funcionários e atribui um perfil interno                                                    | RF006, RF007, RF008  |
| CT-WEB-F001-06 | A autorização de apoio não clínico só aparece e só é enviada para o Auxiliar Veterinário                                              | RF008                |
| CT-WEB-F001-07 | O Administrador bloqueia uma conta e o funcionário bloqueado deixa de conseguir entrar                                                | RF009, RF002         |
| CT-WEB-F001-08 | Administrador e Rececionista criam e atualizam Clientes e respetiva conta de acesso; dados inválidos são rejeitados                   | RF010, RF011, RNF004 |
| CT-WEB-F001-09 | Só o Administrador consulta e atualiza os dados gerais e as configurações da clínica                                                  | RF012, RF013         |

### 10.2. F002 — Pacientes

| ID             | Condição                                                                                                            | Requisitos    |
| -------------- | ------------------------------------------------------------------------------------------------------------------- | ------------- |
| CT-WEB-F002-01 | Rececionista e Veterinário registam um paciente associado a um Cliente; campos obrigatórios em falta são rejeitados | RF014, RNF004 |
| CT-WEB-F002-02 | A ficha do paciente mostra os dados administrativos registados, também depois de recarregar                         | RF015, RNF005 |
| CT-WEB-F002-03 | A atualização dos dados administrativos fica visível numa consulta posterior                                        | RF016, RNF005 |
| CT-WEB-F002-04 | A pesquisa por proprietário devolve apenas os pacientes desse Cliente                                               | RF017         |

### 10.3. F003 — Agendamento e consultas

| ID             | Condição                                                                                                                                                       | Requisitos    |
| -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------- |
| CT-WEB-F003-01 | O Administrador gere o horário de funcionamento; períodos sobrepostos ou com início não anterior ao fim são rejeitados                                         | RF019, RNF004 |
| CT-WEB-F003-02 | A Rececionista consulta a disponibilidade dos Veterinários dentro do horário da clínica                                                                        | RF020         |
| CT-WEB-F003-03 | A Rececionista cria uma marcação numa vaga livre; uma vaga ocupada é recusada com a mensagem da API                                                            | RF021         |
| CT-WEB-F003-04 | A alteração de uma marcação atualiza a agenda; a gravação simultânea por dois utilizadores mostra o conflito devolvido pela API                                | RF022         |
| CT-WEB-F003-05 | A atualização do estado da marcação respeita as transições permitidas                                                                                          | RF023         |
| CT-WEB-F003-06 | Rececionista e Veterinário consultam a agenda da clínica                                                                                                       | RF024         |
| CT-WEB-F003-07 | A Rececionista regista a chegada do paciente à consulta                                                                                                        | RF025         |
| CT-WEB-F003-08 | O Auxiliar Veterinário consulta a informação de preparação e, quando autorizado, regista informação não clínica de apoio; sem autorização, a opção não aparece | RF026, RF027  |

### 10.4. F004 — Registo clínico

| ID             | Condição                                                                                                                      | Requisitos                 |
| -------------- | ----------------------------------------------------------------------------------------------------------------------------- | -------------------------- |
| CT-WEB-F004-01 | O Veterinário regista notas clínicas, diagnóstico, tratamento e prescrição numa consulta                                      | RF030, RF031, RF032, RF033 |
| CT-WEB-F004-02 | O Veterinário atualiza um registo clínico e a alteração fica visível no histórico                                             | RF034, RNF005              |
| CT-WEB-F004-03 | O Veterinário consulta o histórico clínico do paciente                                                                        | RF035                      |
| CT-WEB-F004-04 | O Auxiliar Veterinário consulta apenas a informação clínica autorizada e não tem opções de criar ou alterar registos clínicos | RF036, RNF001              |

### 10.5. F006 — Boletim de vacinas e desparasitações

| ID             | Condição                                                                                                     | Requisitos   |
| -------------- | ------------------------------------------------------------------------------------------------------------ | ------------ |
| CT-WEB-F006-01 | O Veterinário regista uma vacinação e uma desparasitação                                                     | RF043, RF044 |
| CT-WEB-F006-02 | A data da próxima administração apresentada corresponde à calculada pela API a partir da data e do intervalo | RF045        |
| CT-WEB-F006-03 | O Veterinário consulta o boletim completo do paciente na aplicação web                                       | RF046        |

### 10.6. Transversal — perfis, sessão, rede e erros

| ID            | Condição                                                                                                                                                       | Requisitos                 |
| ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------- |
| CT-WEB-TRV-01 | Para cada perfil, as entradas de menu de outros perfis não aparecem                                                                                            | RNF001, RF005              |
| CT-WEB-TRV-02 | O acesso por endereço direto a uma rota de outro perfil é recusado e redireciona para uma página permitida                                                     | RNF001, RF005              |
| CT-WEB-TRV-03 | O Cliente não consegue usar a aplicação web                                                                                                                    | RNF003                     |
| CT-WEB-TRV-04 | Os erros da API (`{ code, message, detail }`) são mostrados ao utilizador sem expor detalhes técnicos; 401 leva ao login e 403 a uma mensagem de acesso negado | RNF004, RF004              |
| CT-WEB-TRV-05 | Com o backend indisponível ou sem rede, a aplicação mostra uma mensagem de falha e não perde o formulário em edição                                            | RNF006                     |
| CT-WEB-TRV-06 | Os estados de carregamento são apresentados e os botões de envio ficam inativos durante o pedido                                                               | — (qualidade de interface) |
| CT-WEB-TRV-07 | Palavras-passe e tokens não aparecem em claro na interface, nos URL nem nos registos da consola                                                                | RNF001                     |

## 11. Testes de aceitação (simulação)

A simulação de aceitação (I134 e I135) verifica se a aplicação permite a cada perfil interno realizar o seu trabalho do princípio ao fim, segundo os casos de uso aprovados. Não equivale a uma homologação por um cliente real.

- **Participantes:** os quatro elementos do grupo, cada um a representar um perfil diferente da equipa que desenvolveu essa funcionalidade, e um observador que regista os resultados.
- **Cenários:** um ou mais por caso de uso com fluxo web (UC01 a UC07, UC09, UC10, UC12, UC13 e UC16), escritos em linguagem de negócio, com critério de aceitação explícito.
- **Versão:** commit identificado do frontend e do backend, a correr no ambiente local ou no ambiente público de avaliação.
- **Resultado:** cada cenário fica «Aceite», «Aceite com desvio» ou «Não aceite»; os desvios têm decisão (corrigir antes da entrega ou registar como limitação conhecida nas Notas de Release finais, I056).

## 12. Gestão de defeitos

- Uma falha num teste de sistema ou de aceitação é analisada pelo responsável da tarefa de teste: falha do teste (corrige-se o teste) ou defeito do produto.
- Um defeito do produto é registado como Issue no GitHub, com a etiqueta `bug` e a etiqueta de componente `frontend-web`, a identificação do cenário, os passos, o resultado obtido, o esperado, a captura ou _trace_ do Playwright e a severidade (bloqueante, maior ou menor).
- O defeito é corrigido por Merge Request ligado à Issue e revalidado pelo mesmo cenário antes de fechar.
- Os relatórios de resultados (I133) e de aceitação (I135) indicam, para cada falha, a descrição, a causa, a ação e o estado.
- Não é mantido um registo de defeitos do frontend web em ficheiro separado (decisão D-06); se no fim da m4 ficarem defeitos válidos por corrigir, o grupo cria esse registo em `03.02.07-defeitos` e atualiza a CCA.

## 13. Responsabilidades e calendário

### 13.1. Papéis

| Papel                                                                                 | Responsabilidade nos testes do frontend web                                                            |
| ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| Executor de cada tarefa de desenvolvimento (I108 a I118)                              | Verificação manual da sua funcionalidade por perfil e por operação, com evidências no Merge Request    |
| Revisor                                                                               | Repete a verificação a partir de um clone limpo e confirma o tratamento de erros e o acesso por perfil |
| Executores das tarefas de teste (I128 a I135)                                         | Especificar, automatizar e executar os testes e produzir os relatórios                                 |
| Responsável pelos frameworks de teste (Gonçalo Magalhães Ribeiro, Plano de Qualidade) | Configuração do Vitest e do Playwright no repositório e apoio aos Executores                           |
| Auditor                                                                               | Confirma que as evidências correspondem às execuções reais e que os quality gates foram cumpridos      |

### 13.2. Tarefas de testes e qualidade do frontend web

| Issue       | Tarefa                                                                         | Sprint previsto | Artefacto                                             |
| ----------- | ------------------------------------------------------------------------------ | --------------- | ----------------------------------------------------- |
| I043        | Plano de Testes do Frontend Web (este documento)                               | S06             | `m4-plano-testes-frontend-web-v01.md`                 |
| I128        | Especificar os testes de sistema do frontend web                               | S07             | `m4-especificacao-testes-sistema-frontend-web-v01.md` |
| I129        | Automatizar testes de sistema — identidade, configuração, Clientes e pacientes | S07             | Código Playwright                                     |
| I130        | Automatizar testes de sistema — agenda e registo clínico                       | S08             | Código Playwright                                     |
| I131        | Automatizar testes de sistema — vacinação e desparasitação                     | S08             | Código Playwright                                     |
| I132        | Automatizar os testes de sessão, permissões e falhas de rede                   | S08             | Código Playwright                                     |
| I133        | Executar os testes de sistema e produzir o relatório de resultados             | S08             | `m4-resultados-testes-sistema-frontend-web-v01.pdf`   |
| I134        | Especificar os testes de aceitação do frontend web                             | S07             | Especificação dos testes de aceitação                 |
| I135        | Simular a aceitação e produzir o relatório                                     | S08             | Relatório da simulação                                |
| I046        | Executar a análise estática e produzir o relatório                             | S08             | Relatório de análise estática                         |
| I142        | Integrar build, análise estática e testes do web na pipeline                   | S08             | `frontend-ci.yml`                                     |
| I125 e I126 | Validar e corrigir os fluxos integrados do frontend web                        | S07 e S08       | Correções e evidências nos Merge Requests             |

## 14. Riscos

| Risco                                                | Efeito                                                             | Mitigação                                                                                                    |
| ---------------------------------------------------- | ------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------ |
| Atraso das bases (I107 e I108)                       | Atrasa todas as funcionalidades web e, por consequência, os testes | Prioridade às bases no início do Sprint 06; testes escritos por funcionalidade à medida que são integradas   |
| Testes de sistema instáveis (_flaky_)                | Bloqueios falsos do merge                                          | Esperas por elementos e respostas, sem tempos fixos; dados únicos por cenário; _trace_ na primeira repetição |
| Divergência entre a API documentada e a implementada | Falhas na integração                                               | Os testes correm contra o backend real; divergências registadas no Merge Request e decididas antes do merge  |
| Dados partilhados entre cenários                     | Resultados dependentes da ordem                                    | Cenários independentes e base reposta antes de cada execução completa                                        |
| Pipeline lenta por arrancar backend e base de dados  | Ciclo de revisão mais longo                                        | Só Chromium na pipeline (D-03); reutilização dos passos de preparação do backend                             |
| Requisitos por especificar (RF053 a RF056)           | Funcionalidade sem critério de verificação                         | Mantidos fora do âmbito até serem aprovados; registo na RTM final                                            |
| Credenciais expostas nos testes                      | Falha de segurança                                                 | Palavras-passe só em variáveis de ambiente e _secrets_                                                       |

## 15. Rastreabilidade

### 15.1. Requisitos funcionais e níveis de teste

Legenda dos níveis: **M** — verificação manual na tarefa de desenvolvimento; **S** — testes de sistema automáticos; **A** — simulação de aceitação.

| Requisito | Título                                            | Perfil(is) web              | Caso de uso | Endpoint(s)            | Tarefa de desenvolvimento | Tarefa de automação | Níveis  |
| --------- | ------------------------------------------------- | --------------------------- | ----------- | ---------------------- | ------------------------- | ------------------- | ------- |
| RF001     | Autenticar utilizador                             | Todos os internos           | UC01        | EP-001                 | I108                      | I129                | M, S, A |
| RF002     | Rejeitar autenticação inválida                    | Todos os internos           | UC01        | EP-001                 | I108                      | I129                | M, S    |
| RF003     | Terminar sessão                                   | Todos os internos           | UC02        | EP-002                 | I108                      | I132                | M, S, A |
| RF004     | Manter acesso autenticado válido                  | Todos os internos           | UC01        | EP-001, EP-003         | I108                      | I132                | M, S    |
| RF005     | Restringir funcionalidades por perfil             | Todos os internos           | UC01        | EP-001, EP-003         | I108                      | I132                | M, S, A |
| RF006     | Criar conta de funcionário                        | Administrador               | UC03        | EP-006                 | I109                      | I129                | M, S, A |
| RF007     | Atualizar conta de funcionário                    | Administrador               | UC03        | EP-004, EP-005, EP-007 | I109                      | I129                | M, S    |
| RF008     | Atribuir perfil a funcionário                     | Administrador               | UC03        | EP-006, EP-007         | I109                      | I129                | M, S    |
| RF009     | Bloquear conta de funcionário                     | Administrador               | UC03        | EP-008                 | I109                      | I129                | M, S, A |
| RF010     | Criar conta de Cliente                            | Administrador, Rececionista | UC04        | EP-013                 | I111                      | I129                | M, S, A |
| RF011     | Atualizar conta de Cliente                        | Administrador, Rececionista | UC04        | EP-011, EP-012, EP-014 | I111                      | I129                | M, S    |
| RF012     | Consultar dados gerais e configurações da clínica | Administrador               | UC05        | EP-009                 | I110                      | I129                | M, S    |
| RF013     | Atualizar dados gerais e configurações da clínica | Administrador               | UC05        | EP-010                 | I110                      | I129                | M, S, A |
| RF014     | Registar paciente                                 | Rececionista, Veterinário   | UC06        | EP-017                 | I112                      | I129                | M, S, A |
| RF015     | Consultar ficha do paciente                       | Rececionista, Veterinário   | UC07        | EP-016                 | I112                      | I129                | M, S    |
| RF016     | Atualizar dados administrativos do paciente       | Rececionista, Veterinário   | UC07        | EP-018                 | I112                      | I129                | M, S    |
| RF017     | Pesquisar pacientes por proprietário              | Rececionista, Veterinário   | UC07        | EP-011, EP-012, EP-015 | I112                      | I129                | M, S, A |
| RF019     | Gerir horários de funcionamento                   | Administrador               | UC09        | EP-009, EP-010, EP-022 | I110                      | I129                | M, S    |
| RF020     | Consultar disponibilidade dos Veterinários        | Rececionista                | UC09        | EP-021, EP-022         | I113                      | I130                | M, S, A |
| RF021     | Criar marcação                                    | Rececionista                | UC09        | EP-025                 | I113                      | I130                | M, S, A |
| RF022     | Alterar marcação                                  | Rececionista                | UC09        | EP-026                 | I113                      | I130                | M, S    |
| RF023     | Atualizar estado da marcação                      | Rececionista                | UC09        | EP-027                 | I113                      | I130                | M, S    |
| RF024     | Consultar agenda da clínica                       | Rececionista, Veterinário   | UC09, UC10  | EP-023, EP-024         | I114                      | I130                | M, S, A |
| RF025     | Registar chegada à consulta                       | Rececionista                | UC09        | EP-028                 | I114                      | I130                | M, S, A |
| RF026     | Consultar informação de preparação da consulta    | Auxiliar Veterinário        | UC10        | EP-016, EP-023, EP-024 | I114                      | I130                | M, S    |
| RF027     | Registar informação não clínica de apoio          | Auxiliar Veterinário        | UC10        | EP-029                 | I114                      | I130                | M, S    |
| RF030     | Registar notas clínicas                           | Veterinário                 | UC12        | EP-031                 | I115                      | I130                | M, S, A |
| RF031     | Registar diagnóstico                              | Veterinário                 | UC12        | EP-031                 | I115                      | I130                | M, S    |
| RF032     | Registar tratamento                               | Veterinário                 | UC12        | EP-031, EP-032         | I115                      | I130                | M, S    |
| RF033     | Registar prescrição                               | Veterinário                 | UC12        | EP-031, EP-032         | I115                      | I130                | M, S    |
| RF034     | Atualizar registo clínico                         | Veterinário                 | UC12        | EP-032                 | I115                      | I130                | M, S    |
| RF035     | Consultar histórico clínico                       | Veterinário                 | UC12        | EP-033, EP-034         | I116                      | I130                | M, S, A |
| RF036     | Consultar informação clínica autorizada           | Auxiliar Veterinário        | UC13        | EP-033, EP-034         | I116                      | I130                | M, , A  |
| RF043     | Registar vacinação                                | Veterinário                 | UC16        | EP-043                 | I118                      | I131                | M, S, A |
| RF044     | Registar desparasitação                           | Veterinário                 | UC16        | EP-043                 | I118                      | I131                | M, S    |
| RF045     | Calcular próxima administração                    | Veterinário                 | UC16        | EP-042, EP-043         | I118                      | I131                | M, S    |
| RF046     | Consultar boletim no sistema web                  | Veterinário                 | UC16        | EP-042                 | I118                      | I131                | M, S, A |

### 15.2. Cobertura dos requisitos web

Os 37 requisitos funcionais web em âmbito têm verificação manual na tarefa de desenvolvimento e pelo menos uma condição de teste de sistema automático; 18 deles entram também na simulação de aceitação, que cobre todos os casos de uso com fluxo web. Os requisitos excluídos estão justificados na secção 3.2.

### 15.3. Requisitos não funcionais

| Requisito                                   | Como é verificado no frontend web                                                          | Condições                                                     | Níveis |
| ------------------------------------------- | ------------------------------------------------------------------------------------------ | ------------------------------------------------------------- | ------ |
| RNF001 — Controlo de acesso às operações    | Menus e rotas por perfil, acesso por endereço direto, ausência de dados sensíveis em claro | CT-WEB-TRV-01, CT-WEB-TRV-02, CT-WEB-TRV-07, CT-WEB-F004-04   | M, S   |
| RNF003 — Separação dos canais de utilização | O Cliente não consegue usar a aplicação web                                                | CT-WEB-TRV-03                                                 | S      |
| RNF004 — Validação dos dados de entrada     | Formulários com dados inválidos rejeitados e mensagem de erro da API apresentada           | CT-WEB-F001-08, CT-WEB-F002-01, CT-WEB-F003-01, CT-WEB-TRV-04 | M, S   |
| RNF005 — Persistência dos dados confirmados | Dados gravados visíveis numa consulta posterior e após recarregar                          | CT-WEB-F002-02, CT-WEB-F002-03, CT-WEB-F004-02                | S      |
| RNF006 — Dependência de ligação à rede      | Mensagem de falha sem ligação ao backend; sem modo offline                                 | CT-WEB-TRV-05                                                 | S      |

O RNF002 (isolamento dos dados de Clientes) aplica-se à aplicação móvel e ao backend e não tem verificação no frontend web.

## 16. Coerência com o Plano de Qualidade e com a CCA

| Elemento                                         | Plano de Qualidade v02                                              | CCA v08                                 | Este plano                                                                    |
| ------------------------------------------------ | ------------------------------------------------------------------- | --------------------------------------- | ----------------------------------------------------------------------------- |
| Plano de testes do frontend web                  | `OF-M4-011`                                                         | `OF-M4-011` (previsto)                  | Este documento                                                                |
| Especificação e resultados dos testes de sistema | `OF-M4-012`, `OF-M4-013`; QG-06                                     | `OF-M4-012`, `OF-M4-013` (previstos)    | Obrigatórios (I128, I133)                                                     |
| Especificação e relatório de aceitação           | `OF-M4-014`, `OF-M4-015`                                            | `OF-M4-014`, `OF-M4-015` (previstos)    | Obrigatórios (I134, I135)                                                     |
| Análise estática                                 | `OF-M4-016` (ESLint com typescript-eslint e Prettier); QG-04, QG-08 | `OF-M4-016` (previsto)                  | Obrigatória (I046)                                                            |
| Testes unitários do frontend                     | Não previstos                                                       | `REF-M4-001` (condicional) não incluído | Não assumidos como artefacto (D-04)                                           |
| Cobertura do frontend                            | Sem limiar mínimo (objetivo 5)                                      | `REF-M4-002` (condicional) não incluído | Não assumida (D-05)                                                           |
| Registo de defeitos do frontend web              | «Quando aplicável»                                                  | `REF-M4-003` (condicional) não incluído | Defeitos em Issues e nos relatórios; registo separado só se necessário (D-06) |
| Taxa de sucesso dos testes de sistema            | MQ-03 = 100%                                                        | —                                       | Critério de saída 1                                                           |
| Resolução de defeitos válidos                    | MQ-04 = 100%                                                        | —                                       | Critério de saída 5                                                           |

## 17. Decisões tomadas

| N.º  | Decisão                                                                                                                                                                                                    | Razão                                                                                                                                                                                           |
| ---- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| D-01 | Os testes de sistema automáticos usam Playwright 1.64 e correm sempre contra o backend real com dados de demonstração, sem simular a API                                                                   | É o nível exigido pela CCA (`OF-M4-012`, `OF-M4-013`) e pelo QG-06; testar contra o backend real deteta divergências de contrato                                                                |
| D-02 | Os testes de sistema são o principal meio de verificação do frontend web; cada requisito web tem pelo menos um cenário positivo e, quando aplicável, um negativo                                           | Cumpre o critério 1 da I128 e o critério de saída 2                                                                                                                                             |
| D-03 | Na pipeline os testes correm apenas no Chromium (Desktop Chrome); Firefox e WebKit podem correr localmente, sem bloquear o merge                                                                           | Reduz a duração e a instabilidade da pipeline; os requisitos não exigem compatibilidade com vários navegadores. O `playwright.config.ts` passa a ter o Chromium como projeto da pipeline (I142) |
| D-04 | Os testes unitários e de componentes (Vitest e React Testing Library) são facultativos e não geram artefacto nem quality gate                                                                              | `REF-M4-001` não foi incluído na CCA v08 e o Plano de Qualidade v02 não os prevê para o web; a lógica de negócio está no backend, já com testes unitários e de integração                       |
| D-05 | Não é assumido limiar de cobertura nem relatório de cobertura do frontend web                                                                                                                              | Plano de Qualidade v02, objetivo 5, e `REF-M4-002` não incluído na CCA v08; a cobertura de requisitos por cenários é a medida usada                                                             |
| D-06 | Os defeitos são registados em Issues do GitHub e nos relatórios de resultados e de aceitação; não há registo de defeitos em ficheiro separado, salvo se ficarem defeitos válidos por corrigir no fim da m4 | Plano de Qualidade v02 («quando aplicável») e `REF-M4-003` não incluído na CCA v08                                                                                                              |
| D-07 | Palavras-passe e endereços dos ambientes ficam em variáveis de ambiente e _secrets_; nunca no código, nos testes nem nos relatórios                                                                        | Regra de segurança do grupo e Definition of Done («sem segredos no repositório»)                                                                                                                |
| D-08 | RF053 a RF055 e RF056 (EP-052) ficam fora do âmbito deste plano até serem especificados e aprovados                                                                                                        | Deliberações 5 e 12 da ata do Backlog Refinement de 2026-11-07; ponto em aberto 1 da Especificação de Requisitos v02                                                                            |
| D-09 | Os erros da API são verificados no formato `{ code, message, detail }`                                                                                                                                     | Documentação da API v03 (deliberação 10 da ata do Sprint Review do Sprint 05)                                                                                                                   |
| D-10 | Cada cenário é independente, cria os seus dados com identificadores únicos e a base é reposta antes de cada execução completa                                                                              | Critério 4 da I128; reprodutibilidade na pipeline                                                                                                                                               |

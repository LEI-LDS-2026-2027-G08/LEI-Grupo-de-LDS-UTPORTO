# README DO PROJETO — M4

## 1. Identificação

| Campo | Valor |
|---|---|
| Unidade curricular | Laboratório de Desenvolvimento de Software — LDS |
| Ano letivo | 2026/2027 |
| Grupo | G08 |
| Sistema | Gestão de Clínica Veterinária |
| Milestone abrangida | M4 (entrega final), com documentação cumulativa de M3, M2 e M1 |
| Versão do README | v04 |
| Data de atualização do índice | ⟨dd/12/2026⟩ |
| Issue | I146 — Preparar o README final e as credenciais de avaliação da M4 (Sprint 08) |

| Elemento | Número |
|---|---|
| André Filipe Moreira Mesquita | 8220764 |
| Gonçalo Dinis Magalhães Teixeira | 8240293 |
| Gonçalo Magalhães Ribeiro | 8250466 |
| Ivan Cristiano Silva Mendes — Líder | 8220390 |

## 2. Apresentação da entrega e leitura do índice

Esta entrega corresponde à Milestone de Entrega `m4` (entrega final) do projeto **Gestão de Clínica Veterinária**, desenvolvido pelo Grupo **G08**: o sistema completo, formado pelo backend (API REST em .NET), pelo frontend web (React e TypeScript) e pela aplicação móvel Android (Kotlin e Jetpack Compose), com os respetivos testes, a pipeline de integração e entrega contínuas e a documentação de instalação, utilização, release e deployment. O arquivo submetido contém a estrutura cumulativa da pasta `/projeto`.

Os índices estão por ordem descendente de milestone: **m4** (secção 4), **m3** (secção 5), **m2** (secção 6) e **m1** (secção 7). Os READMEs anteriores mantêm-se nesta pasta, sem alterações: [m3-readme-v03.md](m3-readme-v03.md), [m2-readme-v01.md](m2-readme-v01.md) e [m1-readme-v01.md](m1-readme-v01.md).

Todos os caminhos são relativos à localização deste ficheiro (`00-indice-e-submissao`). Na coluna **Estado**, «Entregue» significa que o artefacto foi revisto, auditado e integrado; «Em curso» e «Pendente» identificam a Issue que o produz. Na coluna **Verificado**, «Sim» significa que a ligação foi testada em ⟨dd/12/2026⟩ num clone do ZIP e que o ficheiro existe, tem o formato esperado e corresponde ao nome, à versão e à pasta indicados. As pendências de fecho constam da secção 8.

## 3. Particularidades relevantes para a avaliação

### 3.1. Âmbito

- O âmbito é o da **Proposta de Sistema v05** e da **Declaração de Âmbito v05**, homologadas em 21/10/2026 (secção 5.2): funcionalidades **F001** (Autenticação e Controlos de Acesso), **F002** (Gestão de pacientes), **F003** (Agendamento de consultas), **F004** (Registo clínico e tratamentos) e **F006** (Boletim de vacinas e desparasitações). F005 e F007 estão fora do âmbito; as tarefas correspondentes (I117, I119, I124) foram descontinuadas e mantêm o identificador para rastreabilidade.
- O **frontend web** serve os perfis da clínica (Administrador, Rececionista, Veterinário e Auxiliar de Veterinário). A **aplicação móvel** serve apenas o perfil Cliente; um funcionário que tente entrar na aplicação móvel é recusado.
- A CCA de `m4` a submeter é a **⟨v09⟩**: 22 artefactos `OF-M4-001` a `OF-M4-022` em «Pronto para entrega», `m1`, `m2` e `m3` em «Entregue». Os estados de `m4` passam a «Entregue» apenas depois da submissão efetiva.
- A rastreabilidade final entre requisitos, implementação, testes e defeitos está na **RTM v03** (OF-M4-001).

### 3.2. Ambientes de avaliação e credenciais

| Componente | Endereço ou ficheiro | Observação |
|---|---|---|
| Backend (API e Swagger) | **https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net/swagger** | Microsoft Azure (App Service e Azure SQL Database), com HTTPS. ⟨Confirmar na I145.⟩ |
| Frontend web | ⟨endereço público — I145⟩ | ⟨Confirmar na I145.⟩ |
| Aplicação móvel | ⟨APK de avaliação e respetiva pasta — I145/I056⟩ | Compilada para o backend público. Instalação num emulador ou telemóvel Android (minSdk 26). |

- As bases de dados públicas contêm apenas dados fictícios (nomes com «(demonstração)» ou «(fictício)» e e-mails do domínio reservado `.test`).
- O ambiente usa planos gratuitos: se a aplicação estiver adormecida, o primeiro pedido pode demorar alguns segundos e, se a base de dados estiver em pausa, cerca de 1 minuto; basta repetir o pedido.
- A publicação, a validação e a reversão do sistema completo estão no [Guia de Deployment](../04-artefactos-tecnicos/04.19-releases-e-deployment/m4-guia-deployment-v01.pdf) (OF-M4-010).
- **Credenciais:** as palavras-passe das contas de avaliação **não estão no repositório nem neste README**. Estão no ficheiro [m4-credenciais-avaliacao-v02.md](m4-credenciais-avaliacao-v02.md), incluído na submissão, nesta pasta. São contas temporárias e limitadas, com dados fictícios, a bloquear ou remover depois da avaliação.

Contas de demonstração disponíveis (um perfil por conta; palavras-passe no ficheiro de credenciais):

| Perfil | Conta | Onde usar | Observação |
|---|---|---|---|
| Administrador | `admin@clinica-g08.test` | Web, API | Conta inicial. |
| Rececionista | `rececao@clinica-g08.test` | Web, API | Conta ativa. |
| Rececionista (bloqueada) | `rececao.bloqueada@clinica-g08.test` | Web, API | Bloqueada de propósito: o início de sessão deve ser recusado (403). |
| Veterinário | `vet.ana@clinica-g08.test`, `vet.bruno@clinica-g08.test` | Web, API | Duas contas ativas. |
| Auxiliar de Veterinário | `aux.carla@clinica-g08.test` | Web, API | Com autorização para registar informação de apoio. |
| Auxiliar de Veterinário | `aux.diogo@clinica-g08.test` | Web, API | Sem autorização para registar informação de apoio. |
| Cliente | `cliente.joao@clinica-g08.test` | Mobile, API | Dois animais (Bobi e Mia). |
| Cliente | `cliente.maria@clinica-g08.test` | Mobile, API | Um animal (Kiko). |
| Cliente | `cliente.rui@clinica-g08.test` | Mobile, API | Um animal (Luna); cliente sem NIF. |

Origem e conteúdo destes dados: [README dos dados de demonstração (I061)](../04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao/README.md).

### 3.3. Como avaliar o sistema

1. **Frontend web.** Abrir o endereço da secção 3.2 e entrar com uma conta de funcionário. Percurso sugerido: Rececionista — marcar e alterar uma Consulta e registar a chegada; Veterinário — registar a informação clínica e uma vacina; Auxiliar — consultar o histórico reduzido; Administrador — gerir funcionários, Clientes e os dados da clínica.
2. **Aplicação móvel.** Instalar o APK da secção 3.2 e entrar como `cliente.joao`: ver os animais, o boletim de vacinação e as Consultas futuras e passadas. Entrar como `cliente.maria` mostra apenas o Kiko.
3. **Backend.** No Swagger, em «POST /auth/login» (EP-001), usar *Try it out*; copiar o `token`; em **Authorize** escrever `Bearer <token>`; experimentar «GET /clinica» (EP-009), que deve devolver 200. Sem token, os pedidos devolvem 401.
4. **Instalação local.** Pré-requisitos, base de dados, dados de demonstração, segredos (nunca no repositório), backend, frontend web e aplicação móvel: [Manual de Instalação e Configuração v02](../04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao/m4-manual-instalacao-configuracao-v02.pdf) (OF-M4-007).
5. **Versão, alterações e limitações.** Ver as [Notas de Release do sistema completo v02](../04-artefactos-tecnicos/04.19-releases-e-deployment/m4-notas-release-v02.md) (OF-M4-009).
6. **Testes e qualidade.** Os resultados abaixo são os das Notas de Release; os documentos indicados prevalecem.

| Verificação | Resultado | Documento |
|---|---|---|
| Testes de sistema do frontend web | ⟨n.º cenários / aprovados — I133⟩ | [Resultados dos testes de sistema web](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.02-testes-de-sistema/m4-resultados-testes-sistema-frontend-web-v01.pdf) |
| Testes de sistema do frontend mobile | ⟨n.º cenários / aprovados — I139⟩ | [Resultados dos testes de sistema mobile](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema/m4-resultados-testes-sistema-frontend-mobile-v01.pdf) |
| Simulação da aceitação — web | ⟨resultado — I135⟩ | [Relatório da simulação web](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.03-testes-de-aceitacao/m4-relatorio-simulacao-testes-aceitacao-frontend-web-v01.pdf) |
| Simulação da aceitação — mobile | ⟨resultado — I141⟩ | [Relatório da simulação mobile](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.03-testes-de-aceitacao/m4-relatorio-simulacao-testes-aceitacao-frontend-mobile-v01.pdf) |
| Análise estática — web | ⟨avisos / erros — I046⟩ | [Relatório de análise estática web](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.06-analise-estatica/m4-relatorio-analise-estatica-frontend-web-v01.pdf) |
| Análise estática — mobile | ⟨avisos / erros — I050⟩ | [Relatório de análise estática mobile](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.06-analise-estatica/m4-relatorio-analise-estatica-frontend-mobile-v01.pdf) |
| Pipeline CI/CD do sistema completo | ⟨etapas e quality gates — I053⟩ | [Relatório de evidências da pipeline v02](../04-artefactos-tecnicos/04.16-ci-cd/m4-relatorio-evidencias-pipeline-ci-cd-v02.md) |

## 4. Índice da Milestone m4

### 4.1. Artefactos com compromisso na CCA

Os códigos correspondem às linhas da CCA de `m4`. Os condicionais não assumidos estão na secção 4.7.

| Entregável | Obrigatório | Pasta | Versão | Link | Issue | Estado | Verificado |
|---|---|---|---|---|---|---|---|
| OF-M4-001 — Requirements Traceability Matrix | Sim | `04-artefactos-tecnicos/04.03-requisitos-e-rtm` | v03 | [XLSX](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m4-requirements-traceability-matrix-v03.xlsx) · [PDF](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m4-requirements-traceability-matrix-v03.pdf) | I051 | Em curso | ⟨⟩ |
| OF-M4-002 — Frontend web | Sim | `04-artefactos-tecnicos/04.14-frontend-web/m4-v01` | v01 | [LEIA-ME](../04-artefactos-tecnicos/04.14-frontend-web/m4-v01/LEIA-ME.md) · [Fontes (ZIP)](../04-artefactos-tecnicos/04.14-frontend-web/m4-v01/m4-frontend-web-fontes-v01.zip) · [Pasta](../04-artefactos-tecnicos/04.14-frontend-web/m4-v01/) | I040 (I107–I118, I125, I126, I129–I132) | Em curso | ⟨⟩ |
| OF-M4-003 — Frontend mobile | Sim | `04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01` | v01 | [README](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/README.md) · [Fontes (ZIP)](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/m4-frontend-mobile-fontes-v01.zip) · [Pasta](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/) | I041 (I120–I123, I127, I137, I138) | Em curso | ⟨⟩ |
| OF-M4-004 — Configuração da pipeline CI/CD integrada | Sim | `04-artefactos-tecnicos/04.16-ci-cd/m4-v02` | m4-v02 (m3-v01 a m3-v03 conservadas) | [README m4-v02](../04-artefactos-tecnicos/04.16-ci-cd/m4-v02/README.md) · [Pasta](../04-artefactos-tecnicos/04.16-ci-cd/m4-v02/) | I142, I143 | Pendente | ⟨⟩ |
| OF-M4-005 — Descrição da pipeline CI/CD | Sim | `04-artefactos-tecnicos/04.16-ci-cd` | v02 | [Markdown](../04-artefactos-tecnicos/04.16-ci-cd/m4-descricao-pipeline-ci-cd-v02.md) | I144 | Pendente | ⟨⟩ |
| OF-M4-006 — Relatório de evidências da pipeline CI/CD | Sim | `04-artefactos-tecnicos/04.16-ci-cd` | v02 | [Markdown](../04-artefactos-tecnicos/04.16-ci-cd/m4-relatorio-evidencias-pipeline-ci-cd-v02.md) | I053 | Pendente | ⟨⟩ |
| OF-M4-007 — Manual de instalação e configuração | Sim | `04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao` | v02 | [Markdown](../04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao/m4-manual-instalacao-configuracao-v02.md) · [PDF](../04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao/m4-manual-instalacao-configuracao-v02.pdf) · [Registo de validação](../04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao/m4-s08-i054-⟨data⟩-registo-validacao-v⟨nn⟩.md) | I054 | Em revisão (PR #294) | ⟨⟩ |
| OF-M4-008 — Guia de utilizador — gestão de clientes | Sim | `04-artefactos-tecnicos/04.18-guia-de-utilizador` | v01 | [PDF](../04-artefactos-tecnicos/04.18-guia-de-utilizador/m4-guia-utilizador-amostra-v01.pdf) | I055 | Pendente | ⟨⟩ |
| OF-M4-009 — Notas de release do sistema completo | Sim | `04-artefactos-tecnicos/04.19-releases-e-deployment` | v02 | [Markdown](../04-artefactos-tecnicos/04.19-releases-e-deployment/m4-notas-release-v02.md) | I056 | Pendente | ⟨⟩ |
| OF-M4-010 — Guia de deployment | Sim | `04-artefactos-tecnicos/04.19-releases-e-deployment` | v01 | [PDF](../04-artefactos-tecnicos/04.19-releases-e-deployment/m4-guia-deployment-v01.pdf) | I057 (ensaio I145) | Pendente | ⟨⟩ |
| OF-M4-011 — Plano de testes do frontend web | Sim | `03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.01-plano-de-testes` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.01-plano-de-testes/m4-plano-testes-frontend-web-v01.md) | I043 | Entregue | Sim |
| OF-M4-012 — Especificação dos testes de sistema do frontend web | Sim | `03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.02-testes-de-sistema` | v01 e v02 | [v01](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.02-testes-de-sistema/m4-especificacao-testes-sistema-frontend-web-v01.md) · [v02](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.02-testes-de-sistema/m4-especificacao-testes-sistema-frontend-web-v02.md) | I128 | Entregue | Sim |
| OF-M4-013 — Resultados dos testes de sistema do frontend web | Sim | `03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.02-testes-de-sistema` | v01 | [PDF](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.02-testes-de-sistema/m4-resultados-testes-sistema-frontend-web-v01.pdf) | I133 | Pendente | ⟨⟩ |
| OF-M4-014 — Especificação dos testes de aceitação do frontend web | Sim | `03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.03-testes-de-aceitacao` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.03-testes-de-aceitacao/m4-especificacao-testes-aceitacao-frontend-web-v01.md) | I134 | Entregue | Sim |
| OF-M4-015 — Relatório da simulação dos testes de aceitação do frontend web | Sim | `03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.03-testes-de-aceitacao` | v01 | [PDF](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.03-testes-de-aceitacao/m4-relatorio-simulacao-testes-aceitacao-frontend-web-v01.pdf) | I135 | Pendente | ⟨⟩ |
| OF-M4-016 — Relatório de análise estática do frontend web | Sim | `03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.06-analise-estatica` | v01 | [PDF](../03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.06-analise-estatica/m4-relatorio-analise-estatica-frontend-web-v01.pdf) | I046 | Pendente | ⟨⟩ |
| OF-M4-017 — Plano de testes do frontend mobile | Sim | `03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.01-plano-de-testes` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.01-plano-de-testes/m4-plano-testes-frontend-mobile-v01.md) | I047 | Entregue | Sim |
| OF-M4-018 — Especificação dos testes de sistema do frontend mobile | Sim | `03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema/m4-especificacao-testes-sistema-frontend-mobile-v01.md) | I136 | Entregue | Sim |
| OF-M4-019 — Resultados dos testes de sistema do frontend mobile | Sim | `03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema` | v01 | [PDF](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema/m4-resultados-testes-sistema-frontend-mobile-v01.pdf) | I139 | Pendente | ⟨⟩ |
| OF-M4-020 — Especificação dos testes de aceitação do frontend mobile | Sim | `03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.03-testes-de-aceitacao` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.03-testes-de-aceitacao/m4-especificacao-testes-aceitacao-frontend-mobile-v01.md) | I140 | Entregue | Sim |
| OF-M4-021 — Relatório da simulação dos testes de aceitação do frontend mobile | Sim | `03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.03-testes-de-aceitacao` | v01 | [PDF](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.03-testes-de-aceitacao/m4-relatorio-simulacao-testes-aceitacao-frontend-mobile-v01.pdf) | I141 | Pendente | ⟨⟩ |
| OF-M4-022 — Relatório de análise estática do frontend mobile | Sim | `03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.06-analise-estatica` | v01 | [PDF](../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.06-analise-estatica/m4-relatorio-analise-estatica-frontend-mobile-v01.pdf) | I050 | Pendente | ⟨⟩ |

Observações: a CCA designa OF-M4-008 como «Guia de utilizador — gestão de clientes»; o ficheiro é `m4-guia-utilizador-amostra-v01.pdf` (Backlog de Projeto v06, I055). Em OF-M4-012, a v01 corresponde ao compromisso da CCA e a v02 é a versão posterior (I128, ST-WEB-F004-07).

### 4.2. Checklist de Controlo de Artefactos

| Documento | Versão | Link | Verificado |
|---|---|---|---|
| CCA de m4 a submeter | ⟨v09⟩ | [XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m4-checklist-controlo-artefactos-⟨v09⟩.xlsx) · [PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m4-checklist-controlo-artefactos-⟨v09⟩.pdf) | ⟨⟩ |
| CCA de m3 (submetida em m3, última versão anterior) | v08 | [XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v08.xlsx) · [PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v08.pdf) | Sim |

### 4.3. Gestão e avaliação interna

| Documento | Versão em vigor | Editável | PDF | Versões anteriores de m4 | Verificado |
|---|---|---|---|---|---|
| Regulamento Interno do Grupo | v02 (m3) | [MD](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m3-regulamento-grupo-v02.md) | [PDF](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m3-regulamento-grupo-v02.pdf) | — | Sim |
| Backlog de Projeto | v06 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-backlog-projeto-v06.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-backlog-projeto-v06.pdf) | [v04 XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-backlog-projeto-v04.xlsx) · [v04 PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-backlog-projeto-v04.pdf) · [v05 XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-backlog-projeto-v05.xlsx) · [v05 PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-backlog-projeto-v05.pdf) | Sim |
| Sprint Backlog S06 | v01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-s06-sprint-backlog-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-s06-20261107-sprint-backlog-v01.pdf) | — | Sim |
| Sprint Backlog S07 | v01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-s07-sprint-backlog-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-s07-20261116-sprint-backlog-v01.pdf) | — | Sim |
| Sprint Backlog S08 | v01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-s08-sprint-backlog-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m4-s08-sprint-backlog-v01.pdf) | — | Sim |
| Grelha de Rotação de Papéis | v07 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-grelha-rotacao-papeis-v07.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-grelha-rotacao-papeis-v07.pdf) | [v05 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-grelha-rotacao-papeis-v05.xlsx) · [v05 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-20261107-grelha-rotacao-papeis-v05.pdf) · [v06 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-grelha-rotacao-papeis-v06.xlsx) · [v06 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-20261116-grelha-rotacao-papeis-v06.pdf) | Sim |
| Mapa de Carga Planeada | v06 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-mapa-carga-planeada-v06.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-mapa-carga-planeada-v06.pdf) | [v04 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-mapa-carga-planeada-v04.xlsx) · [v04 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-20261107-mapa-carga-planeada-v04.pdf) · [v05 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-mapa-carga-planeada-v05.xlsx) · [v05 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-20261116-mapa-carga-planeada-v05.pdf) | Sim |
| Mapa de Pontuação Individual — Sprint 06 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-s06-mapa-pontuacao-individual-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-s06-mapa-pontuacao-individual-v01.pdf) | — | Sim |
| Mapa de Pontuação Individual — Sprint 07 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-s07-mapa-pontuacao-individual-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-s07-mapa-pontuacao-individual-v01.pdf) | — | Sim |
| Mapa de Pontuação Individual — Sprint 08 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-s08-mapa-pontuacao-individual-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m4-s08-mapa-pontuacao-individual-v01.pdf) | — | ⟨após a Sprint Review S08⟩ |
| Registo de Não Conformidades e Ações Corretivas | v04 | [XLSX](../02-artefactos-de-qualidade-de-processo/02.06-nao-conformidades-e-acoes-corretivas/m4-registo-nao-conformidades-v04.xlsx) | — | [m3 v01 a v03](../02-artefactos-de-qualidade-de-processo/02.06-nao-conformidades-e-acoes-corretivas/) | Sim |

### 4.4. Reuniões de m4

Convocatórias e atas em PDF, na pasta [01.03-reunioes-de-grupo](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/).

| Sprint | Reunião | Convocatória | Ata | Verificado |
|---|---|---|---|---|
| S06 | Backlog Refinement | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s06-20261107-backlog-refinement-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s06-20261107-backlog-refinement-ata-v01.pdf) | Sim |
| S06 | Sprint Planning | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s06-20261107-sprint-planning-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s06-20261107-sprint-planning-ata-v01.pdf) | Sim |
| S06 | Sprint Review | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s06-20261111-sprint-review-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s06-20261112-sprint-review-ata-v01.pdf) | Sim |
| S07 | Backlog Refinement | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261113-backlog-refinement-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261113-backlog-refinement-ata-v01.pdf) | Sim |
| S07 | Sprint Planning | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261116-sprint-planning-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261116-sprint-planning-ata-v01.pdf) | Sim |
| S07 | Sprint Review | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261121-sprint-review-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261122-sprint-review-ata-v01.pdf) | Sim |
| S08 | Backlog Refinement | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s08-20261121-backlog-refinement-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s08-20261122-backlog-refinement-ata-v01.pdf) | Sim |
| S08 | Sprint Planning | ⟨convocatória⟩ | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s08-20261125-sprint-planning-ata-v01.pdf) | ⟨⟩ |
| S08 | Sprint Review | ⟨PDF⟩ | ⟨PDF⟩ | ⟨⟩ |
| m4 | Controlo Final da Milestone m4 | ⟨PDF⟩ | A acrescentar após a reunião | ⟨⟩ |

### 4.5. Revisões e auditorias de qualidade

Cada tarefa tem um relatório de revisão ([02.04-revisoes](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/)) e uma checklist de auditoria ([02.05-auditorias-de-qualidade](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/)). Quando existem várias versões, todas estão ligadas. Títulos conforme o Backlog de Projeto v06. As tarefas do Sprint 08 ainda sem relatório estão assinaladas «⟨⟩» e são completadas no fecho.

| Issue | Tarefa | Sprint | Revisão | Auditoria | Verificado |
|---|---|---|---|---|---|
| I043 | Elaborar o Plano de Testes do Frontend Web | S06 | [v01 · 10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i043-20261110-relatorio-revisao-v01.pdf) · [v02 · 10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i043-20261110-relatorio-revisao-v02.pdf) | [v01 · 10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i043-20261110-checklist-auditoria-v01.pdf) · [v02 · 10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i043-20261110-checklist-auditoria-v02.pdf) | Sim |
| I046 | Executar a análise estática do frontend web e produzir o relatório | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I047 | Elaborar o Plano de Testes do Frontend Mobile | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i047-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i047-20261110-checklist-auditoria-v01.pdf) | Sim |
| I050 | Executar a análise estática do frontend mobile e produzir o relatório | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I051 | Atualizar a RTM final | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I053 | Produzir as evidências finais da pipeline CI/CD | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I054 | Atualizar o Manual de Instalação e Configuração para o sistema completo | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I055 | Elaborar o Guia de Utilizador (amostra) | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I056 | Elaborar as Notas de Release finais do sistema | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I057 | Elaborar o Guia de Deployment | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I107 | Criar a base do frontend web em React e TypeScript | S06 | [v01 · 09/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i107-20261109-relatorio-revisao-v01.pdf) · [v02 · 10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i107-20261110-relatorio-revisao-v02.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i107-20261110-checklist-auditoria-v01.pdf) | Sim |
| I108 | Implementar a autenticação e a gestão de sessão no frontend web (RF001–RF005) | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i108-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i108-20261110-checklist-auditoria-v01.pdf) | Sim |
| I109 | Implementar a gestão de funcionários no frontend web (RF006–RF009) | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i109-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i109-20261110-checklist-auditoria-v01.pdf) | Sim |
| I110 | Implementar gestão da clínica e dos horários no frontend web (RF012, RF013, RF019, RF055) | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i110-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i110-20261110-checklist-auditoria-v01.pdf) | Sim |
| I111 | Implementar gestão de Clientes e contas de acesso no frontend web (RF010, RF011, RF053, RF054) | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i111-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i111-20261110-checklist-auditoria-v01.pdf) | Sim |
| I112 | Implementar a gestão de pacientes no frontend web (RF014–RF017) | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i112-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i112-20261110-checklist-auditoria-v01.pdf) | Sim |
| I113 | Implementar disponibilidade e gestão de Consultas no frontend web (RF020–RF023) | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i113-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i113-20261110-checklist-auditoria-v01.pdf) | Sim |
| I114 | Implementar agenda, chegada e apoio à Consulta no frontend web (RF024–RF027) | S06 | [11/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i114-20261111-relatorio-revisao-v01.pdf) | [11/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i114-20261111-checklist-auditoria-v01.pdf) | Sim |
| I115 | Implementar registo e atualização clínica no frontend web (RF030–RF034) | S07 | [v01 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i115-20261120-relatorio-revisao-v01.pdf) · [v02 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i115-20261120-relatorio-revisao-v02.pdf) | [20/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i115-20261120-checklist-auditoria-v01.pdf) | Sim |
| I116 | Implementar o histórico clínico e a consulta de informação autorizada no frontend web (RF035, RF036) | S07 | [20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i116-20261120-relatorio-revisao-v01.pdf) | [20/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i116-20261120-checklist-auditoria-v01.pdf) | Sim |
| I118 | Implementar boletim de vacinação e desparasitação no frontend web (RF043–RF046; RF056 excluído — deliberação 5) | S07 | [v01 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i118-20261120-relatorio-revisao-v01.pdf) · [v02 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i118-20261120-relatorio-revisao-v02.pdf) | [22/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i118-20261122-checklist-auditoria-v01.pdf) | Sim |
| I120 | Criar a base da aplicação móvel Android em Kotlin e Jetpack Compose | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i120-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i120-20261110-checklist-auditoria-v01.pdf) | Sim |
| I121 | Implementar a autenticação do Cliente na aplicação móvel (RF001–RF005) | S06 | [10/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s06-i121-20261110-relatorio-revisao-v01.pdf) | [10/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s06-i121-20261110-checklist-auditoria-v01.pdf) | Sim |
| I122 | Implementar animais do Cliente e boletim de vacinação/desparasitação na aplicação móvel (RF018, RF047) | S07 | [19/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i122-20261119-relatorio-revisao-v01.pdf) | [20/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i122-20261120-checklist-auditoria-v01.pdf) | Sim |
| I123 | Implementar Consultas futuras e histórico administrativo do Cliente na aplicação móvel (RF028, RF029) | S07 | [v01 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i123-20261120-relatorio-revisao-v01.pdf) · [v02 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i123-20261120-relatorio-revisao-v02.pdf) | [22/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i123-20261122-checklist-auditoria-v01.pdf) | Sim |
| I125 | Validar e corrigir fluxos integrados do frontend web — identidade, registo, agenda e clínico | S07 (concluída no S08) | [v01 · 23/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i125-20261123-relatorio-revisao-v01.pdf) · [v02 · 23/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i125-20261123-relatorio-revisao-v02.pdf) · [v03 · 23/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i125-20261123-relatorio-revisao-v03.pdf) | [26/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i125-20261126-checklist-auditoria-v01.pdf) | Sim |
| I126 | Validar e corrigir fluxos integrados do frontend web — vacinação/desparasitação e permissões | S08 | ⟨⟩ | [27/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s08-i126-20261127-checklist-auditoria-v01.pdf) | ⟨⟩ |
| I127 | Validar e corrigir os fluxos integrados da aplicação móvel contra o backend | S07 | [23/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i127-20261123-relatorio-revisao-v01.pdf) | [23/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i127-20261123-checklist-auditoria-v01.pdf) | Sim |
| I128 | Especificar os testes de sistema do frontend web | S07 | [20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i128-20261120-relatorio-revisao-v01.pdf) | [v01 · 20/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i128-20261120-checklist-auditoria-v01.pdf) · [v02 · 22/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i128-20261122-checklist-auditoria-v02.pdf) | Sim |
| I129 | Automatizar testes de sistema Web — identidade, configuração, Clientes e pacientes | S07 | [v01 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i129-20261120-relatorio-revisao-v01.pdf) · [v02 · 21/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i129-20261121-relatorio-revisao-v02.pdf) | [v01 · 20/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i129-20261120-checklist-auditoria-v01.pdf) · [v02 · 21/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i129-20261121-checklist-auditoria-v02.pdf) | Sim |
| I130 | Automatizar testes de sistema Web — agenda e registo clínico | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I131 | Automatizar testes de sistema Web — vacinação e desparasitação | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I132 | Automatizar os testes de sessão, permissões e falhas de rede do frontend web | S07 | [v01 · 23/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i132-20261123-relatorio-revisao-v01.pdf) · [v02 · 23/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i132-20261123-relatorio-revisao-v02.pdf) | [23/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i132-20261123-checklist-auditoria-v01.pdf) | Sim |
| I133 | Executar os testes de sistema do frontend web e produzir o relatório de resultados | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I134 | Especificar os testes de aceitação do frontend web | S07 | [18/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i134-20261118-relatorio-revisao-v01.pdf) | [20/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i134-20261120-checklist-auditoria-v01.pdf) | Sim |
| I135 | Simular a aceitação do frontend web e produzir o relatório | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I136 | Especificar os testes de sistema do frontend mobile | S07 | [v01 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i136-20261120-relatorio-revisao-v01.pdf) · [v02 · 20/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i136-20261120-relatorio-revisao-v02.pdf) | [20/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i136-20261120-checklist-auditoria-v01.pdf) | Sim |
| I137 | Automatizar os testes de sistema do frontend mobile | S07 | [25/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i137-20261125-relatorio-revisao-v01.pdf) | [25/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i137-20261125-checklist-auditoria-v01.pdf) | Sim |
| I138 | Automatizar os testes de sessão, permissões e falhas de rede do frontend mobile | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I139 | Executar os testes de sistema do frontend mobile e produzir o relatório de resultados | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I140 | Especificar os testes de aceitação do frontend mobile | S07 | [v01 · 21/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i140-20261121-relatorio-revisao-v01.pdf) · [v02 · 21/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m4-s07-i140-20261121-relatorio-revisao-v02.pdf) | [22/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m4-s07-i140-20261122-checklist-auditoria-v01.pdf) | Sim |
| I141 | Simular a aceitação do frontend mobile e produzir o relatório | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I142 | Integrar a compilação, a análise estática e os testes do frontend web na pipeline | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I143 | Integrar a compilação, a análise estática e os testes do frontend mobile na pipeline | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I144 | Atualizar a descrição da pipeline CI/CD para o sistema completo | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I145 | Ensaiar o deployment do sistema completo | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |
| I146 | Preparar o README final e as credenciais de avaliação da M4 | S08 | ⟨⟩ | ⟨⟩ | ⟨⟩ |

### 4.6. Evidências e documentação técnica complementar

Documentos de apoio aos artefactos da secção 4.1 e documentação técnica atualizada em `m4`. Os que não constam da CCA como compromisso de `m4` não são apresentados como entregáveis autónomos.

| Área | Documento | Ligações | Verificado |
|---|---|---|---|
| Requisitos e modelação | Especificação de requisitos v03; casos de uso v03; modelo de dados v03; modelos de classes do backend v03 e do frontend web v04 | [Requisitos v03](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m4-especificacao-requisitos-v03.md) · [Casos de uso v03](../04-artefactos-tecnicos/04.04-casos-de-uso/m4-especificacoes-casos-uso-v03.md) · [Dados v03](../04-artefactos-tecnicos/04.06-modelos-de-dados/m4-modelo-de-dados-clinica-veterinaria-v03.md) · [Classes backend v03](../04-artefactos-tecnicos/04.07-modelos-de-classes/m4-modelo-classes-backend-v03.md) · [Classes web v04](../04-artefactos-tecnicos/04.07-modelos-de-classes/m4-modelo-classes-frontend-web-v04.md) | Sim |
| API | Documentação da API v04 | [Markdown](../04-artefactos-tecnicos/04.11-documentacao-de-api/m4-documentacao-api-v04.md) | Sim |
| Backend | Backend m4-v02 (correções da integração com os frontends) | [README](../04-artefactos-tecnicos/04.12-backend/m4-v02/README.md) · [Solução .NET](../04-artefactos-tecnicos/04.12-backend/m4-v02/ClinicaVeterinaria.sln) | Sim |
| Base de dados | Scripts m4-v03 (migrações idempotentes) | [README](../04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m4-v03/README.md) · [SQL](../04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m4-v03/01-migracoes-idempotentes.sql) | Sim |
| Testes do backend | Especificações v02 dos testes unitários e de integração; collection Postman v02 e resultado Newman da I125 | [Unitários v02](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios/m4-especificacao-testes-unitarios-backend-v02.md) · [Integração v02](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao/m4-especificacao-testes-integracao-backend-v02.md) · [Collection v02](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m4-collection-postman-v02.json) · [Newman I125](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m4-i125-20261122-resultado-newman-v02.txt) | Sim |
| Integração web | Roteiros e evidências da I125 e da I126 | [I125 roteiro](../04-artefactos-tecnicos/04.14-frontend-web/m4-v01/m4-i125-roteiro-resultados-v01.md) · [I125 evidências](../04-artefactos-tecnicos/04.14-frontend-web/m4-v01/m4-i125-evidencias-v01.zip) · [I126 roteiro](../04-artefactos-tecnicos/04.14-frontend-web/m4-v01/m4-i126-roteiro-resultados-v01.md) · [I126 evidências](../04-artefactos-tecnicos/04.14-frontend-web/m4-v01/m4-i126-evidencias-v01/) | Sim |
| Integração mobile | Roteiro, registos e decisões da I127 | [Roteiro](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/m4-i127-roteiro-resultados-v01.md) · [Ocorrências e decisões](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/m4-i127-ocorrencias-decisoes-v01.md) · [Registos](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/m4-i127-registos-verificacao-v01.md) | Sim |
| Testes de sistema mobile | Relatório do executor, matriz de cenários e execuções da I137 | [Relatório](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/m4-i137-relatorio-executor-v01.md) · [Matriz](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/m4-i137-matriz-cenarios-testes-v01.md) · [Evidências](../04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01/evidencias-i137/) | Sim |
| Deployment | Ensaio do deployment do sistema completo (I145) | ⟨evidências da I145⟩ | ⟨⟩ |

### 4.7. Artefactos condicionais e pastas sem conteúdo

Artefactos condicionais do catálogo da CCA que o grupo **não assumiu** para `m4` e, por isso, **não entregou**:

| Artefacto | Natureza | Pasta | Estado |
|---|---|---|---|
| Resultados dos testes unitários do frontend web e mobile (REF-M4-001) | Condicional | `03.02-frontend-web/03.02.04-testes-unitarios` · `03.03-frontend-mobile/03.03.04-testes-unitarios` | Não entregue |
| Relatório de cobertura dos testes do frontend web e mobile (REF-M4-002) | Condicional | `03.02-frontend-web/03.02.05-cobertura-de-testes` · `03.03-frontend-mobile/03.03.05-cobertura-de-testes` | Não entregue |
| Registo de defeitos do frontend web e mobile (REF-M4-003) | Condicional | `03.02-frontend-web/03.02.07-defeitos` · `03.03-frontend-mobile/03.03.07-defeitos` | ⟨Não entregue — os defeitos estão na RTM v03, coluna K⟩ |
| Documentação técnica complementar (REF-GERAL-001) | Condicional | `04-artefactos-tecnicos/04.20-documentacao-complementar` | Não entregue |
| Cronograma do Projeto | Não obrigatório | `01-artefactos-de-gestao/01.05-cronograma` | Não entregue |
| Registo de Riscos | Não obrigatório | `01-artefactos-de-gestao/01.07-riscos` | Não entregue |
| Decisão Técnica ou de Gestão — documento autónomo | Não obrigatório | `01-artefactos-de-gestao/01.08-decisoes` | Não entregue |

As pastas estruturais sem artefactos contêm apenas o marcador técnico `delete-me.txt` (Regulamento da UC, secção 18.1); os marcadores e os ficheiros-base de [01.01-templates](../01-artefactos-de-gestao/01.01-templates/) não constituem entregáveis.

## 5. Índice da Milestone m3 (mantido)

Índice de `m3` conservado tal como submetido em `m3`. Atualizações: a ata do Controlo Final da Milestone `m3` passou a estar ligada (secção 5.4) e as remissões internas foram renumeradas. O README original continua em [m3-readme-v03.md](m3-readme-v03.md).

### 5.1. Artefactos com compromisso na CCA

Os códigos correspondem às linhas da CCA de `m3`. Todos os artefactos previstos para `m3` foram entregues; os condicionais não entregues estão na secção 5.7.

| Entregável | Obrigatório | Pasta | Versão | Link | Verificado |
|---|---|---|---|---|---|
| OF-M3-001 — Requirements Traceability Matrix | Sim | `04-artefactos-tecnicos/04.03-requisitos-e-rtm` | v02 | [XLSX](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m3-requirements-traceability-matrix-v02.xlsx) · [PDF](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m3-requirements-traceability-matrix-v02.pdf) | Sim |
| OF-M3-002 — Backend | Sim | `04-artefactos-tecnicos/04.12-backend/m3-v01` | v01 | [README](../04-artefactos-tecnicos/04.12-backend/m3-v01/README.md) · [Solução .NET](../04-artefactos-tecnicos/04.12-backend/m3-v01/ClinicaVeterinaria.sln) · [Pasta](../04-artefactos-tecnicos/04.12-backend/m3-v01/) | Sim |
| OF-M3-003 — Scripts das bases de dados | Sim | `04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v01` | v01 | [README](../04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v01/README.md) · [Pasta](../04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v01/) · [Dados de demonstração (m3-v02)](../04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao/README.md) | Sim |
| OF-M3-004 — Configuração da pipeline CI/CD do backend | Sim | `04-artefactos-tecnicos/04.16-ci-cd/m3-v03` | m3-v03 (m3-v01 e m3-v02 conservadas) | [README m3-v03](../04-artefactos-tecnicos/04.16-ci-cd/m3-v03/README.md) · [Workflow](../04-artefactos-tecnicos/04.16-ci-cd/m3-v03/workflows/backend-ci.yml) · [Quality gates](../04-artefactos-tecnicos/04.16-ci-cd/m3-v03/configuracao/quality-gates.json) · [README m3-v02](../04-artefactos-tecnicos/04.16-ci-cd/m3-v02/README.md) · [README m3-v01](../04-artefactos-tecnicos/04.16-ci-cd/m3-v01/README.md) | Sim |
| OF-M3-005 — Descrição da pipeline CI/CD | Sim | `04-artefactos-tecnicos/04.16-ci-cd` | v01 | [Markdown](../04-artefactos-tecnicos/04.16-ci-cd/m3-descricao-pipeline-ci-cd-v01.md) | Sim |
| OF-M3-006 — Relatório de evidências da pipeline CI/CD | Sim | `04-artefactos-tecnicos/04.16-ci-cd` | v01 | [Markdown](../04-artefactos-tecnicos/04.16-ci-cd/m3-relatorio-evidencias-pipeline-ci-cd-v01.md) · [Evidências (I037)](../04-artefactos-tecnicos/04.16-ci-cd/m3-s05-i037-evidencias/) · [Registo de verificação (I036)](../04-artefactos-tecnicos/04.16-ci-cd/m3-s05-i036-20261104-registo-verificacao-v01.md) | Sim |
| OF-M3-007 — Manual de instalação e configuração | Sim | `04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao` | v01 | [Markdown](../04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao/m3-manual-instalacao-configuracao-v01.md) · [PDF](../04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao/m3-manual-instalacao-configuracao-v01.pdf) · [Registo de validação (I038)](../04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao/m3-s05-i038-20261104-registo-validacao-v01.md) | Sim |
| OF-M3-008 — Notas de release do backend | Sim | `04-artefactos-tecnicos/04.19-releases-e-deployment` | v01 | [Markdown](../04-artefactos-tecnicos/04.19-releases-e-deployment/m3-notas-release-v01.md) | Sim |
| OF-M3-009 — Plano de testes do backend | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.01-plano-de-testes` | v01 e v03 | [v01](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.01-plano-de-testes/m3-plano-testes-backend-v01.md) · [v03](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.01-plano-de-testes/m3-plano-testes-backend-v03.md) | Sim |
| OF-M3-010 — Especificação dos testes unitários do backend | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios/m3-especificacao-testes-unitarios-backend-v01.md) | Sim |
| OF-M3-011 — Resultados dos testes unitários do backend | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios/m3-resultados-testes-unitarios-backend-v01.md) · [PDF](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios/m3-resultados-testes-unitarios-backend-v01.pdf) · [Relatório da execução (I090)](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios/m3-s05-i090-20261103-resultados-testes-unitarios-v01.md) | Sim |
| OF-M3-012 — Especificação dos testes de integração do backend | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao/m3-especificacao-testes-integracao-backend-v01.md) | Sim |
| OF-M3-013 — Resultados dos testes de integração do backend | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao/m3-resultados-testes-integracao-backend-v01.md) · [PDF](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao/m3-resultados-testes-integracao-backend-v01.pdf) · [Relatório da execução (I097)](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao/m3-s05-i097-20261102-resultados-testes-integracao-v01.md) | Sim |
| OF-M3-014 — Relatório de cobertura dos testes do backend | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes/m3-relatorio-cobertura-testes-backend-v01.md) · [PDF](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes/m3-relatorio-cobertura-testes-backend-v01.pdf) · [Relatório HTML (I032)](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes/m3-s05-i032-20261104-relatorio-cobertura-html-v01/) | Sim |
| OF-M3-015 — Collection Postman | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman` | v01 | [Collection](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m3-collection-postman-v01.json) · [Ambiente](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m3-ambiente-postman-v01.json) · [README](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/README.md) | Sim |
| OF-M3-016 — Relatório de análise estática do backend | Sim | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica` | v01 | [Markdown](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261101-relatorio-analise-estatica-backend-v01.md) · [PDF](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261101-relatorio-analise-estatica-backend-v01.pdf) | Sim |
| AD-002 — Proposta de Sistema | Compromisso do grupo (CCA) | `04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito` | v05 | [PDF](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m3-proposta-sistema-v05.pdf) | Sim |
| AD-003 — Declaração de Âmbito | Compromisso do grupo (CCA) | `04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito` | v05 | [PDF](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m3-declaracao-ambito-v05.pdf) | Sim |

Observações: a CCA identifica a pasta de OF-M3-004 como `04.16-ci-cd/m3-v03`; as versões `m3-v01` (I102) e `m3-v02` (I103) permanecem conservadas e documentadas, nos termos da secção 18.2 do Regulamento da UC. Em OF-M3-009, o ficheiro v01 corresponde ao compromisso da CCA e a v03 é a versão posterior.

### 5.2. Homologação e Checklist de Controlo de Artefactos

| Documento | Versão | Link | Verificado |
|---|---|---|---|
| Pedido de homologação da PS v05, DA v05 e CCA v07 (21/10/2026) | v03 | [PDF](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m3-email-homologacao-ps-da-cca-v03.pdf) | Sim |
| Resposta do docente — «Homologação confirmada» (21/10/2026) | v03 | [PDF](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m3-resposta-email-homologacao-ps-da-cca-v03.pdf) | Sim |
| Pedidos e respostas anteriores, sem efeito (versões v04 da PS e da DA e v06 da CCA) | v01 e v02 | [Pedido v01](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m3-email-homologacao-ps-da-cca-v01.pdf) · [Pedido v02](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m3-email-homologacao-ps-da-cca-v02.pdf) · [Resposta v01](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m3-resposta-email-homologacao-ps-da-cca-v01.pdf) · [Resposta v02](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m3-resposta-email-homologacao-ps-da-cca-v02.pdf) | Sim |
| Declaração de Âmbito v04 (substituída pela v05) | v04 | [PDF](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m3-declaracao-ambito-v04.pdf) | Sim |
| CCA de m3 a submeter | v08 | [XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v08.xlsx) · [PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v08.pdf) | Sim |
| CCA de m3 homologada | v07 | [XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v07.xlsx) · [PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v07.pdf) | Sim |
| CCA de m3, versões anteriores (v06 sem efeito) | v01 e v06 | [v01 XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v01.xlsx) · [v06 XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v06.xlsx) · [v06 PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m3-checklist-controlo-artefactos-v06.pdf) | Sim |

### 5.3. Gestão e avaliação interna

| Documento | Versão em vigor | Editável | PDF | Versões anteriores | Verificado |
|---|---|---|---|---|---|
| Regulamento Interno do Grupo | v02 | [MD](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m3-regulamento-grupo-v02.md) | [PDF](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m3-regulamento-grupo-v02.pdf) | [v01 MD](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m3-regulamento-grupo-v01.md) · [v01 PDF](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m3-regulamento-grupo-v01.pdf) | Sim |
| Backlog de Projeto | v03 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-backlog-projeto-v03.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-backlog-projeto-v03.pdf) | [v01 XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-backlog-projeto-v01.xlsx) · [v01 PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-backlog-projeto-v01.pdf) · [v02 XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-backlog-projeto-v02.xlsx) | Sim |
| Sprint Backlog S03 | v01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s03-sprint-backlog-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s03-sprint-backlog-v01.pdf) | — | Sim |
| Sprint Backlog S04 | v01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s04-sprint-backlog-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s04-sprint-backlog-v01.pdf) | — | Sim |
| Sprint Backlog S05 | v02 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s05-sprint-backlog-v02.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s05-20261030-sprint-backlog-v02.pdf) | [v01 XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s05-sprint-backlog-v01.xlsx) · [v01 PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m3-s05-20261028-sprint-backlog-v01.pdf) | Sim |
| Grelha de Rotação de Papéis | v04 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-grelha-rotacao-papeis-v04.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-20261103-grelha-rotacao-papeis-v04.pdf) | [v01 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-grelha-rotacao-papeis-v01.xlsx) · [v02 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-grelha-rotacao-papeis-v02.xlsx) · [v02 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-grelha-rotacao-papeis-v02.pdf) · [v03 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-grelha-rotacao-papeis-v03.xlsx) · [v03 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-20261028-grelha-rotacao-papeis-v03.pdf) | Sim |
| Mapa de Carga Planeada | v03 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-mapa-carga-planeada-v03.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-20261028-mapa-carga-planeada-v03.pdf) | [v01 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-mapa-carga-planeada-v01.xlsx) · [v01 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-mapa-carga-planeada-v01.pdf) · [v02 XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-mapa-carga-planeada-v02.xlsx) · [v02 PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-mapa-carga-planeada-v02.pdf) | Sim |
| Mapa de Pontuação Individual — Sprint 03 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-s03-mapa-pontuacao-individual-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-s03-mapa-pontuacao-individual-v01.pdf) | — | Sim |
| Mapa de Pontuação Individual — Sprint 04 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-s04-mapa-pontuacao-individual-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-s04-mapa-pontuacao-individual-v01.pdf) | — | Sim |
| Mapa de Pontuação Individual — Sprint 05 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-s05-mapa-pontuacao-individual-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m3-s05-mapa-pontuacao-individual-v01.pdf) | — | Sim |

### 5.4. Reuniões de m3

Convocatórias e atas em PDF, na pasta [01.03-reunioes-de-grupo](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/).

| Sprint | Reunião | Convocatória | Ata | Verificado |
|---|---|---|---|---|
| S03 | Backlog Refinement | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s03-20261011-backlog-refinement-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s03-20261012-backlog-refinement-ata-v01.pdf) | Sim |
| S03 | Sprint Planning | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s03-20261011-sprint-planning-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s03-20261013-sprint-planning-ata-v01.pdf) | Sim |
| S03 | Reunião formal — alteração do Regulamento Interno do Grupo | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s03-20261016-convocatoria-reuniao-formal-alteracao-regulamento-grupo-v01.pdf) | Ver [m3-readme-v03.md](m3-readme-v03.md), secção 7 | Sim |
| S03 | Sprint Review | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s03-20261016-sprint-review-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s03-20261017-sprint-review-ata-v01.pdf) | Sim |
| S04 | Backlog Refinement | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s04-20261018-backlog-refinement-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s04-20261019-backlog-refinement-ata-v01.pdf) | Sim |
| S04 | Sprint Planning | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s04-20261020-sprint-planning-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s04-20261020-sprint-planning-ata-v01.pdf) | Sim |
| S04 | Sprint Review | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s04-20261025-sprint-review-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s04-20261027-sprint-review-ata-v01.pdf) | Sim |
| S05 | Backlog Refinement | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261026-backlog-refinement-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261027-backlog-refinement-ata-v01.pdf) | Sim |
| S05 | Sprint Planning | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261027-sprint-planning-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261028-sprint-planning-ata-v01.pdf) | Sim |
| S05 | Reunião extraordinária | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261029-reuniao-extraordinaria-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261030-reuniao-extraordinaria-ata-v01.pdf) | Sim |
| S05 | Sprint Review | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261102-sprint-review-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261103-sprint-review-ata-v01.pdf) | Sim |
| m3 | Controlo Final da Milestone m3 | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261106-controlo-final-milestone-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m3-s05-20261106-controlo-final-milestone-ata-v01.pdf) | Sim |

### 5.5. Revisões e auditorias de qualidade

Cada tarefa tem um relatório de revisão ([02.04-revisoes](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/)) e uma checklist de auditoria ([02.05-auditorias-de-qualidade](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/)). Quando existe uma segunda versão da auditoria, ambas estão ligadas. Títulos conforme o Backlog de Projeto v03.

| Issue | Tarefa | Sprint | Revisão | Auditoria | Verificado |
|---|---|---|---|---|---|
| I024 | Atualizar a RTM para a baseline v02 e implementação backend | S05 | [04/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i024-20261104-relatorio-revisao-v01.pdf) | [04/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i024-20261104-checklist-auditoria-v01.pdf) | Sim |
| I027 | Elaborar o Plano de Testes do Backend | S03 | [17/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s03-i027-20261017-relatorio-revisao-v01.pdf) | [17/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s03-i027-20261017-checklist-auditoria-v01.pdf) | Sim |
| I028 | Especificar os Testes Unitários do Backend | S04 | [26/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i028-20261026-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i028-20261030-checklist-auditoria-v01.pdf) | Sim |
| I030 | Especificar os Testes de Integração do Backend | S04 | [29/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i030-20261029-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i030-20261030-checklist-auditoria-v01.pdf) | Sim |
| I032 | Produzir o Relatório de Cobertura do Backend | S05 | [04/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i032-20261104-relatorio-revisao-v01.pdf) | [04/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i032-20261104-checklist-auditoria-v01.pdf) | Sim |
| I034 | Executar a análise estática do backend e produzir o relatório | S05 | [03/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i034-20261103-relatorio-revisao-v01.pdf) | [03/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i034-20261103-checklist-auditoria-v01.pdf) | Sim |
| I036 | Documentar a pipeline CI/CD do backend | S05 | [04/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i036-20261104-relatorio-revisao-v01.pdf) | [04/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i036-20261104-checklist-auditoria-v01.pdf) | Sim |
| I037 | Produzir o relatório de evidências da pipeline CI/CD do backend | S05 | [04/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i037-20261104-relatorio-revisao-v01.pdf) | [04/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i037-20261104-checklist-auditoria-v01.pdf) | Sim |
| I038 | Elaborar o Manual de Instalação e Configuração do backend | S05 | [04/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i038-20261104-relatorio-revisao-v01.pdf) | [04/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i038-20261104-checklist-auditoria-v01.pdf) | Sim |
| I039 | Elaborar as Notas de Release do backend | S05 | [04/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i039-20261104-relatorio-revisao-v01.pdf) | [05/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i039-20261105-checklist-auditoria-v01.pdf) | Sim |
| I058 | Criar a solução .NET 8 do backend e a infraestrutura transversal da API | S03 | [17/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s03-i058-20261017-relatorio-revisao-v01.pdf) | [17/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s03-i058-20261017-checklist-auditoria-v01.pdf) | Sim |
| I059 | Implementar a camada de persistência com Entity Framework Core | S04 | [29/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i059-20261029-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i059-20261030-checklist-auditoria-v01.pdf) | Sim |
| I060 | Escrever o script de criação do esquema da base de dados | S03 | [17/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s03-i060-20261017-relatorio-revisao-v01.pdf) | [17/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s03-i060-20261017-checklist-auditoria-v01.pdf) | Sim |
| I061 | Preparar dados iniciais e de demonstração da base de dados | S04 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i061-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i061-20261030-checklist-auditoria-v01.pdf) | Sim |
| I062 | Preparar migrations e reposição da base de dados por ambiente | S04 | [26/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i062-20261026-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i062-20261030-checklist-auditoria-v01.pdf) | Sim |
| I063 | Implementar autenticação JWT e controlo de acesso por perfil (RF001, RF002, RF004, RF005) | S04 | [26/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i063-20261026-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i063-20261030-checklist-auditoria-v01.pdf) | Sim |
| I064 | Implementar o fim de sessão com revogação do token (RF003) | S04 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i064-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i064-20261030-checklist-auditoria-v01.pdf) | Sim |
| I065 | Implementar criação e atualização de contas de funcionários (RF006, RF007) | S04 | [26/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i065-20261026-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i065-20261030-checklist-auditoria-v01.pdf) | Sim |
| I066 | Implementar atribuição de perfil e bloqueio de contas (RF008, RF009) | S05 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i066-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i066-20261030-checklist-auditoria-v01.pdf) | Sim |
| I067 | Implementar dados gerais da clínica e gestão de horários (RF012, RF013, RF019, RF055) | S04 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i067-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i067-20261030-checklist-auditoria-v01.pdf) | Sim |
| I068 | Implementar gestão administrativa de Clientes e contas de acesso (RF010, RF011, RF053, RF054) | S04 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i068-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i068-20261030-checklist-auditoria-v01.pdf) | Sim |
| I069 | Implementar a gestão de pacientes (RF014–RF017) | S04 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i069-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i069-20261030-checklist-auditoria-v01.pdf) | Sim |
| I070 | Implementar a consulta de animais na área do Cliente (RF018) | S04 | [29/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i070-20261029-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i070-20261030-checklist-auditoria-v01.pdf) | Sim |
| I071 | Implementar o cálculo de disponibilidade dos Veterinários (RF020) | S05 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i071-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i071-20261030-checklist-auditoria-v01.pdf) | Sim |
| I072 | Implementar a agenda da clínica e as Consultas atribuídas ao Veterinário (RF024) | S05 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i072-20261030-relatorio-revisao-v01.pdf) | [v01 · 30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i072-20261030-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i072-20261106-checklist-auditoria-v02.pdf) | Sim |
| I073 | Implementar o agendamento de Consultas sem sobreposição (RF021) | S05 | [29/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i073-20261029-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i073-20261030-checklist-auditoria-v01.pdf) | Sim |
| I074 | Implementar alteração de Consulta e atualização de estado (RF022, RF023) | S05 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i074-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i074-20261030-checklist-auditoria-v01.pdf) | Sim |
| I075 | Implementar chegada, preparação e apoio não clínico à Consulta (RF025–RF027) | S05 | [29/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i075-20261029-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i075-20261030-checklist-auditoria-v01.pdf) | Sim |
| I076 | Implementar Consultas futuras e histórico administrativo na área do Cliente (RF028, RF029) | S05 | [31/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i076-20261031-relatorio-revisao-v01.pdf) | [01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i076-20261101-checklist-auditoria-v01.pdf) | Sim |
| I077 | Implementar registo e atualização da informação clínica (RF030–RF034) | S05 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i077-20261030-relatorio-revisao-v01.pdf) | [v01 · 30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i077-20261030-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i077-20261106-checklist-auditoria-v02.pdf) | Sim |
| I078 | Implementar consulta do histórico clínico e da informação clínica autorizada (RF035, RF036) | S05 | [01/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i078-20261101-relatorio-revisao-v01.pdf) | [01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i078-20261101-checklist-auditoria-v01.pdf) | Sim |
| I082 | Implementar vacinação e desparasitação (RF043–RF047, RF056) | S05 | [31/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i082-20261031-relatorio-revisao-v01.pdf) | [v01 · 01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i082-20261101-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i082-20261106-checklist-auditoria-v02.pdf) | Sim |
| I086 | Implementar testes unitários de identidade, clínica, Clientes e pacientes | S05 | [01/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i086-20261101-relatorio-revisao-v01.pdf) | [01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i086-20261101-checklist-auditoria-v01.pdf) | Sim |
| I087 | Implementar testes unitários de agenda e Consultas | S05 | [01/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i087-20261101-relatorio-revisao-v01.pdf) | [01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i087-20261101-checklist-auditoria-v01.pdf) | Sim |
| I088 | Implementar testes unitários de registo clínico | S05 | [01/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i088-20261101-relatorio-revisao-v01.pdf) | [v01 · 01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i088-20261101-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i088-20261106-checklist-auditoria-v02.pdf) | Sim |
| I089 | Implementar testes unitários de vacinação e desparasitação | S05 | [02/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i089-20261102-relatorio-revisao-v01.pdf) | [02/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i089-20261102-checklist-auditoria-v01.pdf) | Sim |
| I090 | Executar os testes unitários e produzir o relatório de resultados | S05 | [04/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i090-20261104-relatorio-revisao-v01.pdf) | [04/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i090-20261104-checklist-auditoria-v01.pdf) | Sim |
| I091 | Criar a infraestrutura dos testes de integração | S04 | [26/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i091-20261026-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i091-20261030-checklist-auditoria-v01.pdf) | Sim |
| I092 | Implementar testes de integração de identidade, clínica, Clientes e pacientes | S05 | [31/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i092-20261031-relatorio-revisao-v01.pdf) | [v01 · 01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i092-20261101-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i092-20261106-checklist-auditoria-v02.pdf) | Sim |
| I093 | Implementar testes de integração de agenda e área do Cliente | S05 | [01/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i093-20261101-relatorio-revisao-v01.pdf) | [01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i093-20261101-checklist-auditoria-v01.pdf) | Sim |
| I094 | Implementar testes de integração de registo clínico | S05 | [02/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i094-20261102-relatorio-revisao-v01.pdf) | [02/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i094-20261102-checklist-auditoria-v01.pdf) | Sim |
| I095 | Implementar testes de integração de vacinação e desparasitação | S05 | [02/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i095-20261102-relatorio-revisao-v01.pdf) | [v01 · 02/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i095-20261102-checklist-auditoria-v01.pdf) · [v02 · 02/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i095-20261102-checklist-auditoria-v02.pdf) | Sim |
| I096 | Implementar os testes de segurança e de isolamento por perfil | S05 | [02/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i096-20261102-relatorio-revisao-v01.pdf) | [02/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i096-20261102-checklist-auditoria-v01.pdf) | Sim |
| I097 | Executar os testes de integração e produzir o relatório de resultados | S05 | [03/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i097-20261103-relatorio-revisao-v01.pdf) | [v01 · 03/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i097-20261103-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i097-20261106-checklist-auditoria-v02.pdf) | Sim |
| I098 | Criar a Collection Postman e os pedidos de identidade, clínica, Clientes e pacientes | S05 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i098-20261030-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i098-20261030-checklist-auditoria-v01.pdf) | Sim |
| I099 | Preparar os pedidos Postman de agenda e área do Cliente | S05 | [03/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i099-20261103-relatorio-revisao-v01.pdf) | [03/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i099-20261103-checklist-auditoria-v01.pdf) | Sim |
| I100 | Preparar os pedidos Postman de registo clínico e vacinação/desparasitação e exportar a collection final | S05 | [01/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i100-20261101-relatorio-revisao-v01.pdf) | [v01 · 01/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i100-20261101-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i100-20261106-checklist-auditoria-v02.pdf) | Sim |
| I101 | Configurar a análise estática e o quality gate do backend | S04 | [29/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s04-i101-20261029-relatorio-revisao-v01.pdf) | [30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s04-i101-20261030-checklist-auditoria-v01.pdf) | Sim |
| I102 | Criar a pipeline de compilação e testes do backend | S05 | [30/10](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i102-20261030-relatorio-revisao-v01.pdf) | [v01 · 30/10](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i102-20261030-checklist-auditoria-v01.pdf) · [v02 · 06/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i102-20261106-checklist-auditoria-v02.pdf) | Sim |
| I103 | Integrar análise estática, cobertura e quality gates na pipeline | S05 | [03/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i103-20261103-relatorio-revisao-v01.pdf) | [03/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i103-20261103-checklist-auditoria-v01.pdf) | Sim |
| I104 | Disponibilizar o backend em ambiente público de avaliação com entrega contínua | S05 | [03/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i104-20261103-relatorio-revisao-v01.pdf) | [04/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i104-20261104-checklist-auditoria-v01.pdf) | Sim |
| I148 | Atualizar a Grelha de Rotação de Papéis (M3) | S05 | [03/11](../02-artefactos-de-qualidade-de-processo/02.04-revisoes/m3-s05-i148-20261103-relatorio-revisao-v01.pdf) | [03/11](../02-artefactos-de-qualidade-de-processo/02.05-auditorias-de-qualidade/m3-s05-i148-20261103-checklist-auditoria-v01.pdf) | Sim |

### 5.6. Evidências e documentação técnica complementar

Documentos de apoio aos artefactos da secção 5.1. Os que não constam da CCA como compromisso de `m3` não são apresentados como entregáveis autónomos.

| Área | Documento | Ligações | Verificado |
|---|---|---|---|
| Pipeline CI/CD | Execuções de evidência da I037 (sucesso e falhas provocadas) | [Pasta de evidências](../04-artefactos-tecnicos/04.16-ci-cd/m3-s05-i037-evidencias/) · [Resultado da execução com sucesso](../04-artefactos-tecnicos/04.16-ci-cd/m3-s05-i037-evidencias/m3-s05-i037-20261104-e1-sucesso-logs/resultado-da-pipeline.md) | Sim |
| Pipeline CI/CD | Ambiente público (I104): capturas do Swagger e registos da carga de dados | [Swagger — login](../04-artefactos-tecnicos/04.16-ci-cd/m3-v03/m3-s05-i104-20261103-captura-swagger-login-v01.png) · [Swagger — consulta](../04-artefactos-tecnicos/04.16-ci-cd/m3-v03/m3-s05-i104-20261103-captura-swagger-consulta-v01.png) · [Carga de dados](../04-artefactos-tecnicos/04.16-ci-cd/m3-v03/m3-s05-i104-20261103-carga-dados-avaliacao-v01.log) · [Repetição da carga](../04-artefactos-tecnicos/04.16-ci-cd/m3-v03/m3-s05-i104-20261103-carga-dados-avaliacao-repeticao-v01.log) | Sim |
| Testes | Resultados de referência com o Newman (I098, I099, I100) | [I098](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m3-s05-i098-20261030-resultado-newman-v01.md) · [I100](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m3-s05-i100-20261101-resultado-newman-v01.md) · [I099](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/m3-s05-i099-20261102-resultado-newman-v01.md) | Sim |
| Cobertura | Resumos da cobertura (I032) | [Resumo completo](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes/m3-s05-i032-20261104-resumo-cobertura-completo-v01.md) · [Resumo do QG-03](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes/m3-s05-i032-20261104-resumo-cobertura-qg03-v01.md) | Sim |
| Análise estática | Antes, depois e final da I034; registos da pipeline | [Antes](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261101-analise-estatica-antes-v01.md) · [Depois](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261101-analise-estatica-depois-v01.md) · [Final](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261103-analise-estatica-final-v01.md) · [QG-01](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261103-log-pipeline-qg01-v01.md) · [QG-04](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261103-log-pipeline-qg04-v01.md) · [QG-08](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s05-i034-20261103-log-pipeline-qg08-v01.md) | Sim |
| Análise estática | Análise do Sprint 04 (I101), conservada como histórico | [Relatório v01](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s04-i101-20261024-relatorio-analise-estatica-backend-v01.md) · [Relatório v02](../03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica/m3-s04-i101-20261025-relatorio-analise-estatica-backend-v02.md) | Sim |
| Backend | Notas de implementação por Issue (I069 a I096), na pasta do backend | [Pasta m3-v01](../04-artefactos-tecnicos/04.12-backend/m3-v01/) | Sim |
| Documentação técnica atualizada em m3 | Documentação da API v03; modelos de classes dos frontends web e mobile v03 | [API v03](../04-artefactos-tecnicos/04.11-documentacao-de-api/m3-documentacao-api-v03.md) · [Classes web v03](../04-artefactos-tecnicos/04.07-modelos-de-classes/m3-modelo-classes-frontend-web-v03.md) · [Classes mobile v03](../04-artefactos-tecnicos/04.07-modelos-de-classes/m3-modelo-classes-frontend-mobile-v03.md) | Sim |

### 5.7. Artefactos condicionais e pastas sem conteúdo

Artefactos condicionais do catálogo da CCA que o grupo **não assumiu** para `m3` e, por isso, **não entregou**:

| Artefacto | Natureza | Pasta | Estado |
|---|---|---|---|
| Registo de defeitos do backend (REF-M3-001) | Condicional | `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.07-defeitos` | Não entregue |
| Documentação técnica complementar (REF-GERAL-001) | Condicional | `04-artefactos-tecnicos/04.20-documentacao-complementar` | Não entregue |
| Cronograma do Projeto | Não obrigatório | `01-artefactos-de-gestao/01.05-cronograma` | Não entregue |
| Registo de Riscos | Não obrigatório | `01-artefactos-de-gestao/01.07-riscos` | Não entregue |
| Decisão Técnica ou de Gestão — documento autónomo | Não obrigatório | `01-artefactos-de-gestao/01.08-decisoes` | Não entregue |

As pastas estruturais ainda sem artefactos contêm apenas o marcador técnico `delete-me.txt` (Regulamento da UC, secção 18.1); os marcadores e os ficheiros-base de [01.01-templates](../01-artefactos-de-gestao/01.01-templates/) não constituem entregáveis. 

## 6. Índice da Milestone m2 (mantido)

Índice de `m2` conservado, com as ligações atualizadas para a estrutura atual do arquivo: as reuniões estão em PDF diretamente em `01.03-reunioes-de-grupo`. O README original continua em [m2-readme-v01.md](m2-readme-v01.md).

### 6.1. Artefactos técnicos de m2

| Entregável | Obrigatório | Pasta | Versão | Link | Verificado |
|---|---|---|---|---|---|
| OF-M2-001 — Especificação de requisitos | Sim | `04-artefactos-tecnicos/04.03-requisitos-e-rtm` | v01 | [Markdown](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m2-especificacao-requisitos-v01.md) | Sim |
| OF-M2-002 — Checklist de aprovação dos requisitos | Sim | `04-artefactos-tecnicos/04.03-requisitos-e-rtm` | v01 | [XLSX](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m2-checklist-aprovacao-requisitos-v01.xlsx) · [PDF](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m2-checklist-aprovacao-requisitos-v01.pdf) | Sim |
| OF-M2-003 — Requirements Traceability Matrix | Sim | `04-artefactos-tecnicos/04.03-requisitos-e-rtm` | v01 | [XLSX](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m2-requirements-traceability-matrix-v01.xlsx) · [PDF](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m2-requirements-traceability-matrix-v01.pdf) | Sim |
| OF-M2-004 — Especificações e diagrama dos casos de uso | Sim | `04-artefactos-tecnicos/04.04-casos-de-uso` | v01 | [Markdown](../04-artefactos-tecnicos/04.04-casos-de-uso/m2-especificacoes-casos-uso-v01.md) · [Diagrama SVG](../04-artefactos-tecnicos/04.04-casos-de-uso/m2-s02-diagrama-casos-de-uso.svg) | Sim |
| OF-M2-005 — Documentação da arquitetura | Sim | `04-artefactos-tecnicos/04.05-arquitetura` | v01 | [Markdown](../04-artefactos-tecnicos/04.05-arquitetura/m2-documentacao-arquitetura-v01.md) | Sim |
| OF-M2-006 — Modelo de dados — clinica-veterinaria | Sim | `04-artefactos-tecnicos/04.06-modelos-de-dados` | v01 | [Markdown](../04-artefactos-tecnicos/04.06-modelos-de-dados/m2-modelo-de-dados-clinica-veterinaria-v01.md) | Sim |
| OF-M2-007 — Modelo de classes — Backend | Sim | `04-artefactos-tecnicos/04.07-modelos-de-classes` | v01 | [Markdown](../04-artefactos-tecnicos/04.07-modelos-de-classes/m2-modelo-classes-backend-v01.md) | Sim |
| OF-M2-008 — Modelo de classes — Frontend Web | Sim | `04-artefactos-tecnicos/04.07-modelos-de-classes` | v01 | [Markdown](../04-artefactos-tecnicos/04.07-modelos-de-classes/m2-modelo-classes-frontend-web-v01.md) | Sim |
| OF-M2-009 — Modelo de classes — Frontend Mobile | Sim | `04-artefactos-tecnicos/04.07-modelos-de-classes` | v01 | [Markdown](../04-artefactos-tecnicos/04.07-modelos-de-classes/m2-modelo-classes-frontend-mobile-v01.md) | Sim |
| OF-M2-010 — Protótipo de baixa fidelidade | Sim | `04-artefactos-tecnicos/04.10-prototipos-de-interfaces` | v01 | [PDF](../04-artefactos-tecnicos/04.10-prototipos-de-interfaces/m2-prototipo-baixa-fidelidade-v01.pdf) · [Fontes PlantUML](../04-artefactos-tecnicos/04.10-prototipos-de-interfaces/fonte-prototipo-baixa-fidelidade/) | Sim |
| OF-M2-011 — Documentação da API | Sim | `04-artefactos-tecnicos/04.11-documentacao-de-api` | v01 | [Markdown](../04-artefactos-tecnicos/04.11-documentacao-de-api/m2-documentacao-api-v01.md) | Sim |
| AD-001 — Diagrama de sequência — Autenticar utilizador | Compromisso do grupo (CCA) | `04-artefactos-tecnicos/04.05-arquitetura` | v01 | [Markdown](../04-artefactos-tecnicos/04.05-arquitetura/m2-diagrama-sequencia-autenticar-utilizador-v01.md) | Sim |

O diagrama de sequência (AD-001) está em `04.05-arquitetura`; a CCA indica `04.09-modelos-de-interacao`. O link abre o ficheiro efetivamente existente.

### 6.2. Checklist de Controlo de Artefactos e gestão de m2

| Documento | Versão | Editável | PDF | Verificado |
|---|---|---|---|---|
| CCA de m2 (arquivada com a etiqueta `m2`) | v01 | [XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m2-checklist-controlo-artefactos-v01.xlsx) | [PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m2-checklist-controlo-artefactos-v01.pdf) | Sim |
| CCA de m2, versão posterior | v05 | [XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m2-checklist-controlo-artefactos-v05.xlsx) | [PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m2-checklist-controlo-artefactos-v05.pdf) | Sim |
| Backlog de Projeto de m2 | v01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m2-backlog-projeto-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m2-20261007-backlog-projeto-v01.pdf) | Sim |
| Sprint Backlog S02 | v01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m2-s02-sprint-backlog-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m2-s02-20261007-sprint-backlog-v01.pdf) | Sim |
| Grelha de Rotação de Papéis de m2 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m2-grelha-rotacao-papeis-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m2-s02-20261007-grelha-rotacao-papeis-v01.pdf) | Sim |
| Mapa de Carga Planeada de m2 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m2-mapa-carga-planeada-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m2-20261007-mapa-carga-planeada-v01.pdf) | Sim |
| Mapa de Pontuação Individual — Sprint 02 | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m2-s02-mapa-pontuacao-individual-v01.xlsx) | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m2-s02-mapa-pontuacao-individual-v01.pdf) | Sim |

### 6.3. Reuniões de m2

| Reunião | Convocatória | Ata | Verificado |
|---|---|---|---|
| Backlog Refinement | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-s02-20260930-backlog-refinement-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-s02-backlog-refinement-ata-v01.pdf) | Sim |
| Sprint Planning | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-s02-20261001-sprint-planning-convocatoria-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-s02-20261002-sprint-planning-ata-v01.pdf) | Sim |
| Sprint Review | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-20261001-convocatoria-sprint-review-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-s02-20261008-sprint-review-ata-v01.pdf) | Sim |
| Controlo Final da Milestone m2 | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-s02-20261007-convocatoria-de-reuniao-de-controlo-final-da-milestone-2-v01.pdf) | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m2-controlo-final-milestone-ata-v01.pdf) | Sim |

## 7. Índice da Milestone m1 (mantido)

Índice de `m1` conservado, com as ligações atualizadas para os nomes atuais dos ficheiros. O README original continua em [m1-readme-v01.md](m1-readme-v01.md). Os ficheiros `delete-me.txt` e os ficheiros-base de `01.01-templates` não constituem entregáveis.

### 7.1. Índice da milestone

| Entregável | Obrigatório | Pasta | Versão | Link | Verificado |
|---|---|---|---|---|---|
| Regulamento Interno do Grupo | Sim | `01-artefactos-de-gestao/01.02-regulamento-de-grupo` | v01 | [PDF](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m1-regulamento-grupo-v01.pdf) · [MD](../01-artefactos-de-gestao/01.02-regulamento-de-grupo/m1-regulamento-grupo-v01.md) | Sim |
| Reunião Extraordinária — Convocatória | Não | `01-artefactos-de-gestao/01.03-reunioes-de-grupo` | - | Não entregue | Sim |
| Reunião Extraordinária — Ata | Não | `01-artefactos-de-gestao/01.03-reunioes-de-grupo` | - | Não entregue | Sim |
| Controlo Final da Milestone m1 — Convocatória | Sim | `01-artefactos-de-gestao/01.03-reunioes-de-grupo` | v01 | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-s01-20260924-convocatoria-de-reuniao-de-controlo-final-da-milestone-1-v01.pdf) | Sim |
| Controlo Final da Milestone m1 — Ata | Sim | `01-artefactos-de-gestao/01.03-reunioes-de-grupo` | v01 | [PDF](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-s01-20260925-controlo-final-milestone-ata-v01.pdf) | Sim |
| Anexo autónomo de reunião | Não | `01-artefactos-de-gestao/01.03-reunioes-de-grupo` | - | Não entregue | Sim |
| Cronograma do Projeto | Não | `01-artefactos-de-gestao/01.05-cronograma` | - | Não entregue | Sim |
| Grelha de Rotação de Papéis | Sim | `01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo` | v01 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m1-grelha-rotacao-papeis-v01.xlsx) | Sim |
| Registo de Riscos | Não | `01-artefactos-de-gestao/01.07-riscos` | - | Não entregue | Sim |
| Decisão Técnica ou de Gestão — documento autónomo complementar | Não | `01-artefactos-de-gestao/01.08-decisoes` | - | Não entregue | Sim |
| Plano de Qualidade do Processo | Sim | `02-artefactos-de-qualidade-de-processo/02.01-plano-de-qualidade` | v01 | [PDF](../02-artefactos-de-qualidade-de-processo/02.01-plano-de-qualidade/m1-plano-qualidade-processo-v01.pdf) · [MD](../02-artefactos-de-qualidade-de-processo/02.01-plano-de-qualidade/m1-plano-qualidade-processo-v01.md) | Sim |
| Definition of Ready | Sim | `02-artefactos-de-qualidade-de-processo/02.02-definition-of-ready-e-done` | v01 | [PDF](../02-artefactos-de-qualidade-de-processo/02.02-definition-of-ready-e-done/m1-definition-of-ready-v01.pdf) | Sim |
| Definition of Done | Sim | `02-artefactos-de-qualidade-de-processo/02.02-definition-of-ready-e-done` | v01 | [PDF](../02-artefactos-de-qualidade-de-processo/02.02-definition-of-ready-e-done/m1-definition-of-done-v01.pdf) | Sim |
| Checklist de Controlo de Artefactos — Editável | Sim | `02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos` | v03 | [XLSX](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m1-checklist-controlo-artefactos-v03.xlsx) | Sim |
| Checklist de Controlo de Artefactos — PDF assinado | Sim | `02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos` | v03 | [PDF](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m1-checklist-controlo-artefactos-v03.pdf) | Sim |
| Proposta de Sistema | Sim | `04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito` | v03 | [PDF](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m1-proposta-sistema-v03.pdf) | Sim |
| Declaração de Âmbito | Sim | `04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito` | v03 | [PDF](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m1-declaracao-ambito-v03.pdf) | Sim |
| E-mail de Homologação da PS, DA e CCA | Sim | `04-artefactos-tecnicos/04.02-homologacao-ps-da-cca` | v01 | [PDF](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-email-homologacao-ps-da-cca-v01.pdf) | Sim |
| Critérios de Qualidade dos Requisitos | Sim | `04-artefactos-tecnicos/04.03-requisitos-e-rtm` | v01 | [MD](../04-artefactos-tecnicos/04.03-requisitos-e-rtm/m1-criterios-qualidade-requisitos-v01.md) | Sim |
| Documentação Complementar | Não | `04-artefactos-tecnicos/04.20-documentacao-complementar` | - | Não entregue | Sim |

A Proposta de Sistema v03, a Declaração de Âmbito v03 e a CCA v03 são as versões homologadas pelo docente em 25/09/2026 ([resposta v04](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-resposta-email-homologacao-ps-da-cca-v04.pdf)). As versões anteriores ficam conservadas como histórico nas respetivas pastas: PS [v01](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m1-proposta-sistema-v01.pdf) · [v02](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m1-proposta-sistema-v02.pdf); DA [v01](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m1-declaracao-ambito-v01.pdf) · [v02](../04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m1-declaracao-ambito-v02.pdf); CCA [v01](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m1-checklist-controlo-artefactos-v01.pdf) · [v02](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m1-checklist-controlo-artefactos-v02.pdf) · [v04](../02-artefactos-de-qualidade-de-processo/02.03-checklist-de-controlo-de-artefactos/m1-checklist-controlo-artefactos-v04.pdf) (a v04 é posterior à homologada e não constitui nova homologação).

### 7.2. Documentação histórica de m1

| Documento | Ligações | Verificado |
|---|---|---|
| Backlog de Projeto de m1 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m1-backlog-projeto-v01.xlsx) · [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m1-backlog-projeto-v01.pdf) | Sim |
| Sprint Backlog S01 | [XLSX](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m1-s01-sprint-backlog-v01.xlsx) · [PDF](../01-artefactos-de-gestao/01.04-backlog-de-projeto/m1-s01-sprint-backlog-v01.pdf) | Sim |
| Mapa de Carga Planeada de m1 | [XLSX](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m1-mapa-carga-planeada-v01.xlsx) | Sim |
| Mapa de Pontuação Individual — Sprint 01 | [PDF](../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m1-s01-mapa-pontuacao-individual-v01.pdf) | Sim |
| Reunião inicial do grupo | [Convocatória](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-20260916-convocatoria-reuniao-inicial-grupo-v01.pdf) · [Ata](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-20260917-ata-reuniao-inicial-grupo-v01.pdf) | Sim |
| Backlog Refinement S01 | [Convocatória](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-s01-20260917-backlog-refinement-convocatoria-v01.pdf) · [Ata](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-20260918-backlog-refinement-ata-v01.pdf) | Sim |
| Sprint Planning S01 | [Convocatória](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-s01-20260918-sprint-planning-convocatoria-v01.pdf) · [Ata](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-s01-20260919-sprint-planning-ata-v02.pdf) | Sim |
| Sprint Review S01 | [Convocatória](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-s01-20260924-sprint-review-convocatoria-v01.pdf) · [Ata](../01-artefactos-de-gestao/01.03-reunioes-de-grupo/m1-s01-20260925-sprint-review-ata-v01.pdf) | Sim |
| Pedidos e respostas de homologação de m1 | [Pedido v01](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-email-homologacao-ps-da-cca-v01.pdf) · [v02](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-email-homologacao-ps-da-cca-v02.pdf) · [v03](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-email-homologacao-ps-da-cca-v03.pdf) · [v04](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-email-homologacao-ps-da-cca-v04.pdf) · [Resposta v01](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-resposta-email-homologacao-ps-da-cca-v01.pdf) · [v02](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-resposta-email-homologacao-ps-da-cca-v02.pdf) · [v03](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-resposta-email-homologacao-ps-da-cca-v03.pdf) · [v04](../04-artefactos-tecnicos/04.02-homologacao-ps-da-cca/m1-resposta-email-homologacao-ps-da-cca-v04.pdf) | Sim |

## 8. Pendências e notas de fecho

Situação observada em ⟨dd/12/2026⟩. Cada linha deve ser removida ou atualizada quando o ponto ficar resolvido.

| Área | Nota |
|---|---|
| Artefactos do Sprint 08 | As linhas da secção 4.1 em «Em curso» ou «Pendente» são atualizadas à medida que as Issues do Sprint 08 forem integradas em `develop` (prazo dos Executores: 30/11/2026 22:35). |
| Integração dos ramos | Os documentos de gestão e de qualidade de processo estão em `main` e no ramo `docs/s08`; os artefactos técnicos de `m4` estão em `develop`. Antes de gerar o ZIP, integrar `docs/s08` e `develop` em `main` e testar as ligações nesse estado. |
| Controlo Final de m4 | A ata e as assinaturas do Controlo Final da Milestone m4 são acrescentadas depois da reunião, e este índice é atualizado. |
| Estados da CCA | Os 22 artefactos de m4 ficam em «Pronto para entrega». Passam a «Entregue» apenas depois da submissão efetiva no Moodle e com o respetivo comprovativo. |
| Credenciais | O ficheiro `m4-credenciais-avaliacao-v02.md` é colocado nesta pasta do ZIP, com as palavras-passe das contas temporárias preenchidas pelo grupo. Não é versionado no repositório. |
| Ambientes públicos | Confirmar, depois da I145, os endereços do backend, do frontend web e do APK (secção 3.2) e repetir o início de sessão com uma conta de cada perfil. |
| Convocatórias duplicadas | Existem duas convocatórias do Sprint Planning S07 (14/11 e 16/11). O índice liga a de 16/11; decidir se a de 14/11 se mantém como histórico. Falta a convocatória do Sprint Planning S08 no arquivo. |
| Pastas com `delete-me.txt` | `04.14-frontend-web` e `04.15-frontend-mobile` têm conteúdo e ainda o marcador `delete-me.txt`. Remover antes de gerar o ZIP. |
| Ligações | Depois de montar o ZIP, voltar a testar todas as ligações deste README num clone do ZIP e preencher a coluna «Verificado». |

Este README regista o estado documental observado em ⟨dd/12/2026⟩. Não declara que o pacote tenha sido submetido nem que todas as pendências estejam resolvidas.

## 9. Histórico do README

| Versão | Milestone | Ficheiro | Nota |
|---|---|---|---|
| v01 | m1 | [m1-readme-v01.md](m1-readme-v01.md) | Índice da submissão de m1. |
| v01 | m2 | [m2-readme-v01.md](m2-readme-v01.md) | Índice de m2, com documentação cumulativa de m1. |
| v03 | m3 | [m3-readme-v03.md](m3-readme-v03.md) | Índice cumulativo de m3, com os índices de m2 e m1 mantidos. Atualizado em 06/11/2026. |
| v04 | m4 | m4-readme-v04.md (este ficheiro) | Índice cumulativo final de m4 (I146), com os índices de m3, m2 e m1 mantidos por ordem descendente. |

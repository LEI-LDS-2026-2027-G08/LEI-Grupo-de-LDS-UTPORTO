# Plano de Qualidade — Grupo G08
**Unidade curricular:** Laboratório de Desenvolvimento de Software — LDS  

**Projeto:** Gestão de Clínica Veterinária  

**Grupo:** G08  

**Versão:** v02  

**Data:** 23/10/2026  

**Issue e papéis:** por atribuir. Esta versão ainda não tem Issue própria; será planeada no Backlog Refinement e no Sprint Planning, onde se atribuem Executor, Revisor e Auditor. A Issue I006 (v01, m1) está concluída e não é reaberta.  

**Estado:** Rascunho para revisão e auditoria  

---
## Histórico de versões
| Versão | Data | Descrição das alterações |
|---|---|---|
| v01 | 25/09/2026 | Plano de Qualidade inicial (m1). Mantém-se inalterado como histórico (`m1-plano-qualidade-processo-v01.md` e `.pdf`). |
| v02 | 23/10/2026 | Alinha o plano com o Regulamento Interno do Grupo v02 (24/10/2026): tecnologias (.NET 10, React 19.3 e TypeScript 6.0, Android nativo em Kotlin com Jetpack Compose), GitHub em vez de SharePoint, quality gates QG-01 a QG-08. Reflete a Declaração de Âmbito v05 (F005 e F007 fora do âmbito) e a CCA (v07 homologada). Os objetivos, responsabilidades, procedimentos de processo e métricas não mudam. |

---
## 1. Objetivo e Âmbito
O presente Plano de Qualidade define os objetivos, responsabilidades, procedimentos, métricas, verificações e **quality gates** adotados pelo Grupo G08 no desenvolvimento do sistema de **Gestão de Clínica Veterinária**, em cumprimento da Secção 17 do Regulamento de Funcionamento da Unidade Curricular.

Este documento abrange duas vertentes complementares e indissociáveis:

1. **Qualidade de Processo:** Garantia do rigor no planeamento, estimativa, execução, revisão, auditoria, controlo de versões, arquivo e rastreabilidade de todas as tarefas do Backlog de Projeto e dos Sprint Backlogs.

2. **Qualidade de Produto:** Garantia da correção funcional, controlo estrito de acessos por perfil, fiabilidade, manutenibilidade e testabilidade dos artefactos documentais e dos três componentes técnicos principais do sistema (Backend, Frontend Web e Frontend Mobile), suportados pela base de dados `clinica-veterinaria`, em estrita conformidade com os documentos homologados pelo docente.

---
## 2. Documentos Normativos e de Referência
Este plano articula-se diretamente com os documentos oficiais da UC e com os artefactos aprovados e homologados do Grupo G08:

* Regulamento de Funcionamento da Unidade Curricular de LDS (v19) e documento Avaliação Interna dos Grupos;

* Regulamento Interno do Grupo G08 (`m3-regulamento-grupo-v02.pdf`, assinado a 24/10/2026; a v01 mantém-se como histórico);

* Proposta de Sistema homologada (`m1-proposta-sistema-v03.pdf` — código CCA `OF-M1-001`) e a sua reposição integral em `m3-proposta-sistema-v05.pdf` (código CCA `AD-002`, homologada a 21/10/2026);

* Declaração de Âmbito homologada (`m1-declaracao-ambito-v03.pdf` — código CCA `OF-M1-002`) e `m3-declaracao-ambito-v05.pdf` (código CCA `AD-003`, homologada a 21/10/2026), que passa `F005` e `F007` para o não-âmbito;

* Checklist de Controlo de Artefactos homologada em 21/10/2026 (`m3-checklist-controlo-artefactos-v07.xlsx` e `m3-checklist-controlo-artefactos-v07.pdf`);

* Critérios de Qualidade dos Requisitos (`m1-criterios-qualidade-requisitos-v01.md` — código CCA `OF-M1-003`);

* Definition of Ready do G08 (`m1-definition-of-ready-v01.pdf`);

* Definition of Done do G08 (`m1-definition-of-done-v01.pdf`).

---
## 3. Objetivos da Qualidade
O Grupo G08 estabelece os seguintes objetivos mensuráveis de qualidade para o projeto:

1. **Independência Estrita de Papéis:** Garantir que 100% das tarefas normais são executadas, revistas e auditadas por três alunos distintos (Executor, Revisor e Auditor), impedindo técnica e processualmente qualquer autoaprovação de código ou documentação.

2. **Cumprimento Integral da CCA e do Âmbito (`F001` a `F004` e `F006`; `F005` e `F007` estão fora do âmbito desde a Declaração de Âmbito `v05`):** Assegurar a entrega atempada e validada dos **55 artefactos com compromisso assumido na CCA `v07`** (`3` em `m1`, `12` em `m2`, `18` em `m3` e `22` em `m4`), mantendo rastreabilidade total na *Requirements Traceability Matrix* (RTM — `OF-M2-003`, `OF-M3-001` e `OF-M4-001`).

3. **Qualidade e Padronização dos Requisitos:** Garantir que 100% dos requisitos especificados em `OF-M2-001` (incluindo a sua versão 2) são avaliados face aos 11 critérios definidos em `m1-criterios-qualidade-requisitos-v01.md` e obtêm o resultado final `Aprovado` na Checklist de Aprovação dos Requisitos (`OF-M2-002`).

4. **Segurança Funcional e Isolamento de Registos Clínicos (RBAC):** Validar através de testes automáticos que os 5 perfis do sistema (`Administrador`, `Rececionista`, `Veterinário`, `Auxiliar Veterinário` e `Cliente (Proprietário do animal)`) respeitam rigorosamente a separação entre dados administrativos e registos clínicos fixada na Declaração de Âmbito (`v05`, que mantém o conteúdo da `v03` nesta matéria).

5. **Cobertura de Testes e *Quality Gates*:** Atingir e manter uma cobertura mínima de linhas (*Line Coverage*) de **70% no Backend** (`OF-M3-014`). Nos Frontends Web e Mobile não é assumido um limiar mínimo de cobertura; os testes de sistema automáticos e as verificações de análise estática aplicáveis devem concluir com sucesso na pipeline CI/CD antes da integração.

6. **Equilíbrio de Carga e Prazos Operacionais:** Manter o rácio de equilíbrio da Carga das Tarefas Planeadas ($\max(CTP) / \min(CTP) \le 1,10$) em todos os Momentos de Avaliação e cumprir os prazos operacionais de transição entre papéis.

---
## 4. Responsabilidades da Qualidade
### 4.1. Responsabilidades Operacionais por Tarefa
| Papel | Responsabilidades na Garantia da Qualidade | Prazo Operacional Máximo |
|---|---|---|
| **Executor** | Produzir o artefacto ou código cumprindo as convenções do G08; executar localmente linters, formatação e testes; abrir o Pull Request associado à Issue; reunir e arquivar as evidências do Executor e colocar a tarefa `Em Revisão`. | Até **48 horas** antes do **Sprint Review** |
| **Revisor** | Verificar tecnicamente o resultado face aos critérios de aceitação, normas e DoD; distinguir formalmente **defeitos** de **propostas de melhoria**; produzir e arquivar o relatório e evidências de revisão em `02.04-revisoes`; atribuir a classificação do Executor; após autorização do Auditor, aprovar a versão revista ou executar o **squash and merge** e registar `ACEITAÇÃO FINAL CONCLUÍDA`. | Entrega ao Auditor: até **24 horas** antes do **Sprint Review**<br>Aceitação/Merge: até **12 horas** antes do **Sprint Review** |
| **Auditor** | Verificar a independência dos papéis, o cumprimento dos prazos, os cálculos e a completude da revisão; preencher a checklist e arquivar evidências em `02.05-auditorias-de-qualidade`; atribuir a classificação do Revisor e registar `AUTORIZAÇÃO DE ACEITAÇÃO`; após o merge/aceitação, confirmar a versão final e registar `AUDITORIA FINAL CONCLUÍDA`, movendo a tarefa para `Pronta para Sprint Review`. | Auditoria inicial: até **16 horas** antes do **Sprint Review**<br>Confirmação final: até **8 horas** antes do **Sprint Review** |
| **Secretário do Sprint** | Verificar a conformidade formal e de nomenclatura de convocatórias, atas e ficheiros; assegurar o congelamento em PDF e a recolha de assinaturas digitais dos anexos ratificados; mover as tarefas aprovadas no **Sprint Review** para `Done`. | Durante o sprint e no fecho do **Sprint Review** |
| **Líder do Grupo** | Monitorizar semanalmente o quadro de Issues no GitHub, atualizar a Grelha de Rotação de Papéis no repositório GitHub e coordenar o tratamento de impedimentos e não conformidades internas. | Semanal (24h antes das reuniões formais) |

### 4.2. Administração Técnica das Ferramentas de Suporte à Qualidade
De acordo com a Secção 5 do Regulamento Interno do Grupo G08 (v02), a administração técnica das ferramentas encontra-se distribuída pelos quatro elementos:

* **André Filipe Moreira Mesquita:** Pipeline CI/CD (GitHub Actions) e Produção Colaborativa de Documentação (Word / VS Code / GitHub);

* **Gonçalo Magalhães Ribeiro:** Controlo de Versões (GitHub) e Frameworks de Teste e Qualidade (xUnit no backend; Vitest e Playwright no frontend web; JUnit, MockK, Compose UI Test e Robolectric no frontend mobile);

* **Gonçalo Dinis Magalhães Teixeira:** Edição das Folhas de Cálculo Oficiais (XLS/XLSX) e Modelação/Diagramas (PlantUML / Draw.io);

* **Ivan Cristiano Silva Mendes:** Gestão Operacional de Issues (GitHub), Comunicação Interna (Microsoft Teams) e Arquivo/Cópias de Segurança (GitHub).

---
## 5. Procedimentos de Qualidade de Processo
### 5.1. Controlo de Entrada no Sprint (Definition of Ready — DoR)
Nenhuma tarefa normal pode ser selecionada no **Sprint Planning** sem ter sido previamente aprovada como `Ready` em reunião de **Backlog Refinement**, mediante verificação integral da checklist definida em `m1-definition-of-ready-v01.pdf`. O grupo estima a tarefa por **Planning Poker** (escala oficial: 10, 20, 30, 50, 80, 130 pontos), reparte o orçamento em 60% Executor, 30% Revisor e 10% Auditor, e preenche as 27 colunas obrigatórias do Backlog de Projeto, congelando a versão ratificada em PDF assinado digitalmente.

### 5.2. Controlo de Execução, Revisão, Auditoria e Encerramento (Definition of Done — DoD)
Todas as tarefas cumprem o fluxo obrigatório da Secção 15 do Regulamento da UC e os critérios de `m1-definition-of-done-v01.pdf`:

1. **Rastreabilidade no Git:** Para tarefas com código, o Executor trabalha numa branch temporária `<tipo>/i<numero_issue>-<descricao_curta>`, efetua commits no padrão `<tipo>(<escopo>): <descrição_curta> [I<numero_issue>]` e submete um Pull Request. As branches `main` e `develop` estão protegidas contra **push** direto.

2. **Ciclo de Verificação Independente:**

   * Se o Revisor identificar defeitos obrigatórios, regista `REVISÃO REJEITADA` e devolve a tarefa a `Em Execução`; caso contrário, produz o relatório de revisão e transita a tarefa para `Em Auditoria de Qualidade`.

   * Se o Auditor detetar falhas na revisão ou falta de evidências, regista `AUDITORIA REJEITADA` e devolve a tarefa a `Em Revisão`; se estiver conforme, regista `AUTORIZAÇÃO DE ACEITAÇÃO` e devolve ao Revisor para integração.

   * O Revisor executa o **squash and merge** (ou aceitação formal do documento), regista `ACEITAÇÃO FINAL CONCLUÍDA` e devolve ao Auditor.

   * O Auditor confirma que a versão aceite/integrada corresponde à versão revista sem alterações posteriores, regista `AUDITORIA FINAL CONCLUÍDA` e coloca a tarefa em `Pronta para Sprint Review`.

3. **Encerramento e Imutabilidade:** Após aprovação formal no **Sprint Review**, o Secretário move a tarefa para `Done`. Tarefas `Done` não são reabertas; defeitos ou melhorias posteriores originam novas Issues relacionadas.

### 5.3. Gestão de Defeitos, Melhorias, Transferências e Não Conformidades
* **Defeito vs. Proposta de Melhoria:** Qualquer incumprimento de requisito, critério de aceitação, DoD, norma adotada ou decisão anterior constitui um **defeito**, exigindo correção obrigatória. Sugestões que acrescentem valor fora das obrigações aprovadas constituem **propostas de melhoria**, não penalizando o Executor.

* **Transferências Formais:** Qualquer ajuda material na produção de código, testes, modelos ou documentação atribuídos a outro aluno exige uma transferência formal deliberada em reunião extraordinária, mantendo o orçamento original da tarefa e a independência entre Executor, Revisor e Auditor.

* **Não Conformidades e Ações Corretivas:** Qualquer incumprimento processual ou documental é registado em ata e num relatório próprio arquivado em `02.06-nao-conformidades-e-acoes-corretivas`, identificando o facto, a regra afetada, o impacto, a evidência, o responsável pela ação corretiva e o prazo de verificação.

* **Controlo Final de Milestone:** Antes da submissão de cada milestone (`m1` a `m4`), o grupo realiza uma reunião formal autónoma de controlo final para validar o diretório `/projeto`, o README cumulativo e a versão atualizada e assinada da CCA (e, em `m2`, `m3` e `m4`, aprovar o fecho do Momento de Avaliação).

---
## 6. Procedimentos de Qualidade de Produto
### 6.1. Qualidade dos Requisitos, Modelos e Arquitetura (`m1` e `m2`)
* **Validação de Requisitos (`OF-M1-003`, `OF-M2-001`, `OF-M2-002` e a sua versão 2):** Todos os requisitos funcionais e não funcionais são verificados pelo Revisor e auditados pelo Auditor face aos 11 critérios definidos em `m1-criterios-qualidade-requisitos-v01.md` (Correção, Clareza, Completude, Consistência, Necessidade, Viabilidade, Verificabilidade, Rastreabilidade, Singularidade, Ausência de Ambiguidade e Independência da Solução), registando na **Checklist de Aprovação dos Requisitos** `Cumpre`, `Não cumpre — <justificação objetiva>` ou `Não aplicável — <justificação>`, e aprovando apenas requisitos com Resultado Final `Aprovado`.

* **Coerência de Modelos e Diagramas (`OF-M2-004` a `OF-M2-011`, `AD-001` e as respetivas versões 2):** Todos os diagramas (incluindo casos de uso, arquitetura, modelo de dados `clinica-veterinaria v1`, modelos de classes de backend, frontend web e frontend mobile, o compromisso adicional `AD-001 — Diagrama de sequência — autenticar utilizador v1` e as versões 2 dos casos de uso, da API, dos três modelos de classes, do protótipo, do diagrama de sequência, da documentação da arquitetura e do modelo de dados) são produzidos em **PlantUML**, preservando o código-fonte textual e editável dentro dos ficheiros Markdown.

### 6.2. Verificação das Regras de Negócio e Permissões Homologadas (`F001` a `F004` e `F006`; `F005` e `F007` fora do âmbito)
Os planos e especificações de testes (`m3` e `m4`) têm de validar obrigatoriamente as regras de negócio e restrições de acesso fixadas na Proposta de Sistema (`v03`, reposta na `v05`) e na Declaração de Âmbito (`v05`):

| Código | Funcionalidade | Regras Críticas de Qualidade e Controlo de Acessos a Verificar nos Testes |
|---|---|---|
| `F001` | Autenticação e Controlos de Acesso | Autenticação JWT e controlo dos 5 perfis (`Administrador`, `Rececionista`, `Veterinário`, `Auxiliar Veterinário`, `Cliente`). Verificar que o `Administrador` gere contas de funcionários, perfis e configurações da clínica, que `Administrador` e `Rececionista` criam/atualizam contas de Clientes (Web), que o `Administrador` está **bloqueado de consultar, criar ou alterar registos clínicos**, e que a app Mobile permite login exclusivamente ao perfil `Cliente (Proprietário do animal)`. |
| `F002` | Gestão de pacientes (Fichas dos animais) | Verificar que `Rececionista` e `Veterinário` criam, consultam e atualizam dados administrativos (espécie, raça, idade, peso e dono) na Web, que o `Rececionista` está **impedido de criar/alterar registos clínicos**, e que o `Cliente` apenas visualiza os dados básicos dos seus animais no Mobile sem permissão de edição. |
| `F003` | Agendamento de consultas | Validar estados da consulta (`pendente`, `confirmada`, `realizada`, `cancelada`). Verificar que na Web o `Rececionista` cria, altera, cancela marcações e regista chegadas, que o `Veterinário` apenas consulta a agenda e consultas atribuídas (**sem criar, alterar ou cancelar marcações**), que o `Auxiliar Veterinário` consulta e regista informação não clínica de apoio quando autorizado, e que no Mobile o `Cliente` apenas consulta marcações futuras e histórico administrativo (**sem registos clínicos e sem poder marcar, alterar ou cancelar**). |
| `F004` | Registo clínico e tratamentos | Verificar que a criação e alteração de notas clínicas, diagnósticos, tratamentos e prescrições na Web é **exclusiva do `Veterinário`**, que o `Auxiliar Veterinário` apenas tem acesso de leitura para apoio às consultas, e que a funcionalidade não está exposta no Mobile. |
| `F005` | Gestão de contas e pagamentos | **Fora do âmbito** (Declaração de Âmbito `v05`). Não há testes nem verificações previstas; o código `F005` não é reutilizado. |
| `F006` | Boletim de Vacinas e Desparasitações | Testar unitariamente o **cálculo automático da data prevista para a próxima administração** (a partir da data de administração e do intervalo definido pelo `Veterinário`), o registo/atualização exclusivo pelo `Veterinário` na Web e a visualização do boletim digital pelo `Cliente` no Mobile. |
| `F007` | Gestão de internamento (Hospitalização) | **Fora do âmbito** (Declaração de Âmbito `v05`). Não há testes nem verificações previstas; o código `F007` não é reutilizado. |

### 6.3. Artefactos e Verificações de Qualidade de Produto por Milestone (CCA `v07` homologada)
Em articulação direta com a CCA homologada (`m3-checklist-controlo-artefactos-v07.xlsx`), o grupo produzirá e validará os seguintes artefactos de qualidade de produto:

* **Milestone `m3` — Backend (.NET 10, EF Core 10, MS SQL Server 2025):**

  * Plano de testes do backend v1 (`OF-M3-009`) e v2;

  * Especificação (`OF-M3-010`) e Resultados (`OF-M3-011`) dos testes unitários do backend v1 (xUnit 2.6 e Moq 4.20);

  * Especificação (`OF-M3-012`) e Resultados (`OF-M3-013`) dos testes de integração do backend v1 (Respawn / Testcontainers .NET);

  * Relatório de cobertura dos testes do backend v1 (`OF-M3-014` — Coverlet 6.0 / ReportGenerator 5.5 com meta $\ge 70\%$);

  * Collection Postman v1 (`OF-M3-015` — ficheiro `.json` importável permitindo testar todas as funcionalidades disponibilizadas pela API);

  * Relatório de análise estática do backend v1 (`OF-M3-016` — Roslyn Analyzers);

  * Configuração (`OF-M3-004`), Descrição (`OF-M3-005`) e Relatório de evidências (`OF-M3-006`) da pipeline CI/CD do backend v1.

* **Milestone `m4` — Frontend Web (React 19.3 + TypeScript 6.0) e Frontend Mobile (Android nativo: Kotlin 2.2 + Jetpack Compose):**

  * **Frontend Web:** Plano de testes (`OF-M4-011`), Especificação (`OF-M4-012`) e Resultados (`OF-M4-013`) dos testes de sistema automáticos, Especificação (`OF-M4-014`) e Relatório da simulação (`OF-M4-015`) dos testes de aceitação, e Relatório de análise estática (`OF-M4-016` — ESLint com typescript-eslint e verificação do Prettier).

  * **Frontend Mobile:** Plano de testes (`OF-M4-017`), Especificação (`OF-M4-018`) e Resultados (`OF-M4-019`) dos testes de sistema automáticos, Especificação (`OF-M4-020`) e Relatório da simulação (`OF-M4-021`) dos testes de aceitação, e Relatório de análise estática (`OF-M4-022` — Android Lint e ktlint).

  * **Pipeline Integrada:** Configuração (`OF-M4-004`), Descrição (`OF-M4-005`) e Relatório de evidências (`OF-M4-006`) da pipeline CI/CD integrada v2.

---
## 7. Quality Gates Automatizados (Pipeline CI/CD)
A pipeline CI/CD executada no **GitHub Actions** (`.github/workflows/backend-ci.yml`, `.github/workflows/frontend-ci.yml` e `.github/workflows/mobile-ci.yml`) aplica automaticamente os seguintes *quality gates* definidos na Secção 11.6 do Regulamento Interno do G08 (v02):

| Código | Componente / Etapa | Verificação | Ferramenta / Regra | Critério / Limiar | Efeito do Incumprimento | Evidência Produzida |
|---|---|---|---|---|---|---|
| `QG-01` | Geral | Compilação global (Backend, Web, Mobile) | `dotnet build`, `npm run build`, `./gradlew assembleDebug` | Zero erros de compilação | Bloqueio imediato da pipeline | Log de execução da etapa de Build |
| `QG-02` | Backend | Testes unitários e de integração | xUnit CLI (`dotnet test`) | 100% dos testes a passar | Bloqueio do Merge Request | Relatório XML/JUnit de testes (`OF-M3-011`, `OF-M3-013`) |
| `QG-03` | Backend | Cobertura de código backend | Coverlet 6.0 + ReportGenerator 5.5 | Cobertura de linhas $\ge 70\%$ (Domínio e Serviços) | Bloqueio do Merge Request | Relatório HTML/Cobertura (`OF-M3-014`) |
| `QG-04` | Backend & Web | Linter e Análise Estática | Roslyn / ESLint | Zero erros de severidade Warning/Error | Bloqueio do Merge Request | Log de output do linter (`OF-M3-016`, `OF-M4-016`) |
| `QG-05` | Frontend mobile | Análise Estática e Formatação | `./gradlew ktlintCheck lintDebug` | Zero erros ou violações de regras | Bloqueio do Merge Request | Logs e relatórios do ktlint e do Android Lint (`OF-M4-022`) |
| `QG-06` | Frontend web | Testes de sistema automáticos | Playwright | 100% dos testes a passar | Bloqueio do Merge Request | Relatório/log de execução dos testes |
| `QG-07` | Frontend mobile | Testes de sistema automáticos | Compose UI Test + Robolectric (`./gradlew testDebugUnitTest`) | 100% dos testes a passar | Bloqueio do Merge Request | Relatório/log de execução dos testes |
| `QG-08` | Backend & Web | Formatação | `dotnet format --verify-no-changes`, `npm run format:check` | Zero ficheiros por formatar | Bloqueio do Merge Request | Log de output do formatter |

Nos relatórios `OF-M3-006` (em `m3`) e `OF-M4-006` (em `m4`), o grupo conservará e demonstrará tanto as evidências de execuções válidas bem-sucedidas como as evidências dos bloqueios automáticos provocados por falhas de compilação, testes, análise estática ou **quality gates**.

---
## 8. Métricas de Qualidade (Processo e Produto)
As seguintes métricas serão monitorizadas pelo grupo e arquivadas em `02.07-metricas-de-processo`:

| ID | Métrica | Dimensão | Fórmula / Modo de Medição | Meta / Limiar do G08 | Periodicidade |
|---|---|---|---|---|---|
| `MP-01` | Equilíbrio da Carga Planeada (CTP) | Processo | $\max(CTP_A, CTP_B, CTP_C, CTP_D) / \min(CTP_A, CTP_B, CTP_C, CTP_D)$ | $\le 1,10$ | Por Sprint e Momento de Avaliação |
| `MP-02` | Taxa de Conformidade com a DoR | Processo | $(\text{Tarefas aprovadas como Ready} / \text{Tarefas selecionadas no Sprint}) \times 100$ | $100\%$ | Semanal (**Backlog Refinement**) |
| `MP-03` | Cumprimento de Prazos Operacionais | Processo | $(\text{Entregas de papel dentro do prazo máximo} / \text{Total de entregas}) \times 100$ | $\ge 90\%$ sem penalização | Semanal (**Sprint Review**) |
| `MP-04` | Taxa de Rejeição Interna | Processo | N.º de devoluções com `REVISÃO REJEITADA` ou `AUDITORIA REJEITADA` | Tendência decrescente entre Sprints | Semanal (**Sprint Review**) |
| `MP-05` | Execução de Compromissos da CCA | Processo | $(\text{Artefactos Prontos/Entregues na Milestone} / \text{Artefactos Previstos na CCA}) \times 100$ | $100\%$ em cada Milestone (`m1` a `m4`) | Antes de cada submissão |
| `MQ-01` | Taxa de Aprovação de Requisitos | Produto | $(\text{Requisitos com Resultado Final = Aprovado} / \text{Total de Requisitos}) \times 100$ | $100\%$ (`OF-M2-002`) | Milestone `m2` |
| `MQ-02` | Cobertura de Linhas do Backend | Produto | Percentagem de linhas cobertas na camada de Domínio e Serviços | $\ge 70\%$ (`OF-M3-014`) | Contínua (`m3` e `m4`) |
| `MQ-03` | Taxa de Sucesso dos Testes de Sistema dos Frontends | Produto | `(Testes de sistema aprovados / Total de testes de sistema executados) × 100` | `100%` (Web e Mobile) | Contínua (`m4`) |
| `MQ-04` | Resolução de Defeitos Válidos | Produto | $(\text{Defeitos corrigidos e revalidados} / \text{Defeitos válidos detetados}) \times 100$ | $100\%$ antes do estado `Done` | Contínua (por Sprint e Milestone) |

---
## 9. Evidências Obrigatórias e Estrutura de Arquivo
Todas as evidências produzidas no âmbito deste Plano de Qualidade seguem a nomenclatura oficial `m<milestone-real>-s<sprint>-i<issue>-<data>-<artefacto>-v<versao>.<extensao>` e são arquivadas nas pastas funcionais obrigatórias da Secção 19 do Regulamento da UC dentro de `/projeto`:

### 9.1. Qualidade de Processo (`/projeto/02-artefactos-de-qualidade-de-processo`)
* `02.01-plano-de-qualidade`: Plano de Qualidade do Processo (`m1-plano-qualidade-processo-v01.pdf` e, na `m3`, `m3-plano-qualidade-processo-v02.pdf`).

* `02.02-definition-of-ready-e-done`: Documentos da DoR (`m1-definition-of-ready-v01.pdf`) e da DoD (`m1-definition-of-done-v01.pdf`).

* `02.03-checklist-de-controlo-de-artefactos`: Ficheiro editável cumulativo da CCA (`<milestone>-checklist-controlo-artefactos-v<versao>.xlsx`) e respetivo PDF congelado e assinado digitalmente.

* `02.04-revisoes`: Relatórios e evidências de revisão produzidos pelo Revisor para cada tarefa/Issue.

* `02.05-auditorias-de-qualidade`: Checklists e evidências de auditoria produzidas pelo Auditor para cada tarefa/Issue.

* `02.06-nao-conformidades-e-acoes-corretivas`: Registos de não conformidades de processo e comprovação das ações corretivas.

* `02.07-metricas-de-processo`: Relatórios de recolha e análise das métricas de processo (`MP-01` a `MP-05`).

### 9.2. Qualidade de Produto (`/projeto/03-artefactos-de-qualidade-de-produto`)
* `03.01-backend`:

  * `03.01.01-plano-de-testes` (`OF-M3-009`)

  * `03.01.02-testes-unitarios` (`OF-M3-010`, `OF-M3-011`)

  * `03.01.03-testes-de-integracao` (`OF-M3-012`, `OF-M3-013`)

  * `03.01.04-cobertura-de-testes` (`OF-M3-014`)

  * `03.01.05-postman` (`OF-M3-015`)

  * `03.01.06-analise-estatica` (`OF-M3-016`)

  * `03.01.07-defeitos` (Registo de defeitos do backend, quando aplicável)

* `03.02-frontend-web`:

  * `03.02.01-plano-de-testes` (`OF-M4-011`)

  * `03.02.02-testes-de-sistema` (`OF-M4-012`, `OF-M4-013`)

  * `03.02.03-testes-de-aceitacao` (`OF-M4-014`, `OF-M4-015`)

  * `03.02.06-analise-estatica` (`OF-M4-016`)

  * `03.02.07-defeitos` (Registo de defeitos do frontend web, quando aplicável)

* `03.03-frontend-mobile`:

  * `03.03.01-plano-de-testes` (`OF-M4-017`)

  * `03.03.02-testes-de-sistema` (`OF-M4-018`, `OF-M4-019`)

  * `03.03.03-testes-de-aceitacao` (`OF-M4-020`, `OF-M4-021`)

  * `03.03.06-analise-estatica` (`OF-M4-022`)

  * `03.03.07-defeitos` (Registo de defeitos do frontend mobile, quando aplicável)

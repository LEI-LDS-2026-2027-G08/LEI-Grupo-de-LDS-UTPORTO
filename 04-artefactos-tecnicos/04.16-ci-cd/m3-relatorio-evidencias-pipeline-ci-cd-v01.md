# Relatório de Evidências da Pipeline de CI/CD do Backend — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Relatório de evidências da pipeline de CI/CD do backend |
| Ficheiro | `m3-relatorio-evidencias-pipeline-ci-cd-v01.md` |
| Versão | v01 |
| Milestone | m3 |
| Issue | I037 — Produzir o relatório de evidências da pipeline CI/CD do backend (GitHub #66) |
| Sprint | S05 |
| Executor | Gonçalo Magalhães Ribeiro |
| Revisor | Ivan Cristiano Silva Mendes |
| Auditor | Gonçalo Dinis Magalhães Teixeira |
| Pipeline | `.github/workflows/backend-ci.yml`, versão m3-v03 (I102, I103 e I104), descrita na I036 (`m3-descricao-pipeline-ci-cd-v01.md`) |
| Versão de referência | `develop` no commit `89d6b31` (2026-11-04 18:37 UTC) |
| Anexos | Pasta `m3-s05-i037-evidencias/`, nesta pasta (secção 10) |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-11-04 | Gonçalo Magalhães Ribeiro | Versão inicial: uma execução completa com sucesso em `develop` e três execuções com falha provocada (teste, etapa de compilação e limite de qualidade QG-03), com capturas, registos e a demonstração do bloqueio do merge. |

---

## 1. Objetivo

Demonstrar, com execuções reais e identificáveis por commit, que a pipeline do backend executa todas as verificações obrigatórias (compilação, testes, cobertura, análise estática, formatação e Collection Postman) e que **uma falha obrigatória impede a integração** em `develop`. Segue a definição da I037 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog do Sprint 05 v02.

## 2. Documentos de referência

| Documento | Utilização |
|---|---|
| Backlog de Projeto v03 (I037) | Critérios de aceitação, evidências e verificações |
| Descrição da Pipeline de CI/CD (I036, `m3-descricao-pipeline-ci-cd-v01.md`) | Etapas, quality gates, artefactos e proteção dos ramos |
| README da pipeline `m3-v02` (I103) e `m3-v03` (I104) | Limites dos quality gates e etapa 8 de entrega |
| `.github/quality-gates.json` | Limiares usados pela pipeline (QG-03 ≥ 70% de linhas; QG-04, QG-08 e Newman com máximo 0) |
| Regulamento de Funcionamento da UC v19, secção 19 (04.16-ci-cd) | Pasta e nome do relatório |

## 3. Método

1. **Execução com sucesso (E1):** a execução da pipeline no push para `develop` do commit `89d6b31`, a integração da I036, que é a versão de referência deste relatório.
2. **Execuções com falha provocada (E2 a E4):** três ramos temporários criados a partir de `develop` em `89d6b31`, cada um com **uma única alteração** (um ficheiro), enviados para o GitHub e abertos como Pull Request para `develop` com o título «I037 — falha provocada (…) — NÃO FAZER MERGE». Os Pull Requests foram fechados sem merge e os ramos apagados depois da recolha das evidências.

   | Ramo | Commit | Única alteração | Falha esperada |
   |---|---|---|---|
   | `i037-falha-teste` | `a94b715` | Novo teste unitário `FalhaProvocadaI037Tests` com `Assert.Fail(...)` | Teste (etapa 2, QG-02) |
   | `i037-falha-compilacao` | `79f1c47` | Novo ficheiro `FalhaProvocadaI037.cs` no Domínio com `int Valor = "texto";` | Etapa de compilação (etapa 1, QG-01) |
   | `i037-falha-qg03` | `6852c44` | `.github/quality-gates.json`: `limiarMinimoPercentagem` de 70 para 99 | Limite de qualidade (etapa 4, QG-03) |

3. **Recolha:** para cada execução, captura da página da execução (etapas, duração e resultado), captura das linhas do registo que mostram o resultado, e o arquivo de registos descarregado do GitHub (*Download log archive*), do qual se guardou o registo completo de cada job (um ficheiro de texto por job). Para E2 a E4, captura da página do Pull Request com o merge bloqueado.
4. **Verificação dos anexos:** os registos foram percorridos à procura de palavras-passe, cadeias de ligação com palavra-passe, tokens JWT, nomes de computador e caminhos pessoais. Só aparecem nomes de variáveis (por exemplo, `SQLCMDPASSWORD=$SA`); os valores gerados em cada execução estão escondidos pelo GitHub (`***`). Os caminhos são os do runner (`/home/runner/work/...`).

Nas execuções de Pull Request, o GitHub testa o *merge commit* do ramo com `develop` (por exemplo, `1e0181f` = `a94b715` sobre `89d6b31`). As horas são em UTC.

## 4. Resumo das execuções

| ID | Execução | Evento | Commit | Início e fim (UTC) | Duração total | Etapa que falha | Resultado | Merge |
|---|---|---|---|---|---|---|---|---|
| **E1** | Backend CI **#221** (`41888968713`) | push em `develop` | `89d6b31` | 2026-11-04 18:37:26 – 18:43:57 | 6 min 37 s | — | ✅ Success | Integração da I036 (já integrada) |
| **E2** | Backend CI **#225** (`41891706460`) | Pull Request #247 | `a94b715` (merge `1e0181f`) | 2026-11-04 18:54:25 – 18:59:57 | 5 min 39 s | 2 · Testes unitários (QG-02) | ❌ Failure | Bloqueado |
| **E3** | Backend CI **#226** (`41891749860`) | Pull Request #248 | `79f1c47` (merge `4ca137d`) | 2026-11-04 18:54:45 – 18:55:43 | 1 min 14 s | 1 · Restauro e compilação (QG-01) | ❌ Failure | Bloqueado |
| **E4** | Backend CI **#227** (`41891774877`) | Pull Request #249 | `6852c44` (merge `904ebe1`) | 2026-11-04 18:54:50 – 18:59:44 | 5 min 1 s | 4 · Cobertura (QG-03) | ❌ Failure | Bloqueado |

Resultado de cada etapa, tal como registado pelo job «Resultado da pipeline» de cada execução:

| Etapa | E1 (#221) | E2 (#225) | E3 (#226) | E4 (#227) |
|---|---|---|---|---|
| 1 · Restauro e compilação (QG-01) | ✅ 49 s | ✅ 42 s | ❌ 32 s | ✅ 45 s |
| 2 · Testes unitários (QG-02) | ✅ 45 s | ❌ 1 min 8 s | ⊘ não executada | ✅ 46 s |
| 3 · Testes de integração (QG-02) | ✅ 5 min 18 s | ✅ 4 min 24 s | ⊘ não executada | ✅ 3 min 37 s |
| 4 · Cobertura (QG-03) | ✅ 15 s | ✅ 12 s | ✅ 19 s (sem dados, ver 7) | ❌ 23 s |
| 5 · Análise estática (QG-04) | ✅ 7 s | ✅ 9 s | ❌ 11 s | ✅ 9 s |
| 6 · Formatação (QG-08) | ✅ 53 s | ✅ 53 s | ⊘ não executada | ✅ 44 s |
| 7 · Collection Postman (Newman) | ✅ 1 min 35 s | ✅ 1 min 47 s | ⊘ não executada | ✅ 1 min 28 s |
| Resultado da pipeline (verificação obrigatória) | ✅ 2 s | ❌ 6 s | ❌ 7 s | ❌ 3 s |
| 8 · Entrega no ambiente público (I104) | ⊘ (não é `main`) | ⊘ | ⊘ | ⊘ |

As durações são as da página de cada execução (capturas «execucao»).

## 5. E1 — execução completa com sucesso (#221, `develop` em `89d6b31`)

Todas as etapas 1 a 7 e o «Resultado da pipeline» terminaram com sucesso; a etapa 8 fica *skipped* porque só corre num push para `main` (I104).

**Resultados dos quality gates nesta execução** (resumos da execução e registos dos jobs):

| Quality gate / verificação | Limite (`.github/quality-gates.json` e Plano de Testes) | Valor medido | Resultado | Evidência |
|---|---|---|---|---|
| QG-01 — compilação | 0 erros, avisos tratados como erros | Solução compilada em Release sem erros nem avisos | ✅ | `…-e1-sucesso-resumo-qg01.png`; job 1 em `…-e1-sucesso-logs.zip` |
| QG-02 — testes unitários | 100% a passar | 718 testes: 717 aprovados, 0 falhados, 1 ignorado (UT-TRV-007, justificado na I090) | ✅ | `…-e1-sucesso-resumo-qg02.png`; job 2 |
| QG-02 — testes de integração | 100% a passar | 463 testes: 463 aprovados | ✅ | Job 3 em `…-e1-sucesso-logs.zip` |
| **QG-03 — cobertura de linhas** (Domínio e Serviços) | ≥ 70% | **98,51%** (2118 de 2150 linhas); ramos 92,83% (referência 60%) | ✅ «QG-03 cumprido: 98.51% ≥ 70%» | `…-e1-sucesso-resumo-qg03.png`; job 4 |
| **QG-04 — análise estática** (Roslyn) | 0 ocorrências com nível error ou warning | **0** ocorrências que bloqueiam; 3161 de nível note; 0 suprimidas; 6 relatórios SARIF | ✅ «QG-04 cumprido» | `…-e1-sucesso-resumo-qg04.png`; job 5 |
| QG-08 — formatação | 0 ficheiros por formatar | 0 ficheiros, 0 alterações | ✅ «QG-08 cumprido» | `…-e1-sucesso-resumo-qg08.png`; job 6 |
| Collection Postman (Newman) | 0 falhas | 269 pedidos e 862 verificações, 0 falhas | ✅ | Job 7 em `…-e1-sucesso-logs.zip` |

Os números coincidem com os relatórios da I090 (718 / 717 / 0 / 1), da I097 (463 / 463), da I032 (98,5% das linhas, 2118 de 2150) e da I099 (269 pedidos, 862 verificações).

Captura da execução: `m3-s05-i037-evidencias/m3-s05-i037-20261104-e1-sucesso-execucao.png`.

## 6. E2 — falha provocada num teste (#225, Pull Request #247)

- **Alteração:** um teste unitário novo, `FalhaProvocadaI037Tests.FalhaProvocada`, que termina com `Assert.Fail("Falha provocada para a I037.")`.
- **Resultado da etapa 2:** `Total tests: 719` (os 718 da suite mais o teste provocado), `Passed: 717`, **`Failed: 1`**, `Skipped: 1`; o registo identifica o teste: `Failed I037 - falha provocada (nao fazer merge)` com a mensagem «Falha provocada para a I037.» e termina com `Process completed with exit code 1`.
- **Restantes etapas:** 1, 3, 4, 5, 6 e 7 com sucesso; o «Resultado da pipeline» falha porque a etapa 2 não terminou com sucesso.
- **Merge:** bloqueado (secção 9).

Evidências: `…-e2-teste-execucao.png`, `…-e2-teste-log-falha.png`, `…-e2-teste-log-totais.png`, `…-e2-teste-pr247-bloqueado.png` e `…-e2-teste-logs.zip`.

## 7. E3 — falha provocada numa etapa: compilação (#226, Pull Request #248)

- **Alteração:** ficheiro novo `src/backend/ClinicaVeterinaria.Domain/FalhaProvocadaI037.cs` com uma atribuição de texto a um inteiro.
- **Resultado da etapa 1:** `error CS0029: Cannot implicitly convert type 'string' to 'int'` em `FalhaProvocadaI037.cs(5,42)`, `Build FAILED`, `0 Warning(s)`, `1 Error(s)`, `Process completed with exit code 1`.
- **Restantes etapas:**
  - 2, 3, 6 e 7 **não são executadas** (dependem da compilação), o que encurta a execução para 1 min 14 s;
  - 5 (análise estática) também falha, porque compila a solução para produzir os relatórios SARIF e encontra o mesmo erro;
  - 4 (cobertura) corre por desenho mesmo quando os testes não terminam (I103, README `m3-v02`, secção 2) e termina com o aviso «Sem ficheiros de cobertura, porque os testes não terminaram com sucesso.»; o bloqueio é garantido pelo «Resultado da pipeline», que exige o sucesso de todas as etapas e falha.
- **Outro workflow:** no Pull Request, o workflow `integration-tests.yml` (I062) também falha, porque também compila a solução; não é a verificação obrigatória da proteção do ramo.
- **Merge:** bloqueado (secção 9).

Evidências: `…-e3-compilacao-execucao.png`, `…-e3-compilacao-log.png`, `…-e3-compilacao-pr248-bloqueado.png` e `…-e3-compilacao-logs.zip`.

## 8. E4 — falha provocada num limite de qualidade: QG-03 (#227, Pull Request #249)

- **Alteração:** apenas a configuração: `limiarMinimoPercentagem` de 70 para 99 em `.github/quality-gates.json`. O código e os testes são os de `develop`.
- **Resultado da etapa 4:** «❌ **QG-03 não cumprido:** 98.51% < 99%. O merge fica bloqueado.» e `Error: Cobertura de linhas 98.51% abaixo do limiar de 99% (.github/quality-gates.json).`, com `exit code 1`.
- **Restantes etapas:** 1, 2, 3, 5, 6 e 7 com sucesso (a cobertura medida é a mesma de E1, 98,51%); o «Resultado da pipeline» falha.
- Esta execução demonstra também que o limiar é lido da configuração e não está escrito no workflow (critério 1 da I103).

Evidências: `…-e4-qg03-execucao.png`, `…-e4-qg03-log.png`, `…-e4-qg03-pr249-bloqueado.png` e `…-e4-qg03-logs.zip`.

## 9. Bloqueio da integração

O ramo `develop` está protegido: exige Pull Request com aprovação e a verificação obrigatória **«Backend CI / Resultado da pipeline»** (I036, secção 9). Nas três execuções com falha:

| Pull Request | Verificações falhadas | «Resultado da pipeline» (Required) | Estado do merge |
|---|---|---|---|
| #247 (teste) | 4 (etapa 2 e Resultado, nos eventos push e pull_request) | ❌ Failing | **Merging is blocked**; botão «Squash and merge» inativo |
| #248 (compilação) | 7 (etapas 1 e 5, Resultado, e o workflow `integration-tests.yml`) | ❌ Failing | **Merging is blocked**; botão inativo |
| #249 (QG-03) | 4 (etapa 4 e Resultado, nos eventos push e pull_request) | ❌ Failing | **Merging is blocked**; botão inativo |

Em todos, a etapa 8 (entrega) fica *skipped*, por isso nenhuma versão com falha chega ao ambiente público. As capturas `…-pr247-bloqueado.png`, `…-pr248-bloqueado.png` e `…-pr249-bloqueado.png` mostram as verificações e o bloqueio. Os três Pull Requests foram fechados sem merge.

## 10. Anexos (pasta `m3-s05-i037-evidencias/`)

Todos os ficheiros estão no repositório e no ZIP da entrega; as ligações às execuções do GitHub (secção 11) são informativas, porque os registos do GitHub Actions expiram ao fim de 90 dias.

| Ficheiro | Conteúdo |
|---|---|
| `m3-s05-i037-20261104-e1-sucesso-execucao.png` | E1: página da execução #221 (etapas, durações, resultado) |
| `m3-s05-i037-20261104-e1-sucesso-resumo-qg01.png` | E1: resumo da etapa 1 (QG-01) |
| `m3-s05-i037-20261104-e1-sucesso-resumo-qg02.png` | E1: resumo da etapa 2 (QG-02, testes unitários) |
| `m3-s05-i037-20261104-e1-sucesso-resumo-qg03.png` | E1: resumo da etapa 4 (QG-03, cobertura) |
| `m3-s05-i037-20261104-e1-sucesso-resumo-qg04.png` | E1: resumo da etapa 5 (QG-04, análise estática) |
| `m3-s05-i037-20261104-e1-sucesso-resumo-qg08.png` | E1: resumo da etapa 6 (QG-08, formatação) |
| `m3-s05-i037-20261104-e1-sucesso-logs.zip` | E1: registos completos dos 8 jobs |
| `m3-s05-i037-20261104-e2-teste-execucao.png` | E2: página da execução #225 |
| `m3-s05-i037-20261104-e2-teste-log-falha.png` | E2: linha do teste falhado no registo da etapa 2 |
| `m3-s05-i037-20261104-e2-teste-log-totais.png` | E2: totais da etapa 2 (719 / 717 / 1 / 1) |
| `m3-s05-i037-20261104-e2-teste-pr247-bloqueado.png` | E2: Pull Request #247 com o merge bloqueado |
| `m3-s05-i037-20261104-e2-teste-logs.zip` | E2: registos completos dos 8 jobs |
| `m3-s05-i037-20261104-e3-compilacao-execucao.png` | E3: página da execução #226 |
| `m3-s05-i037-20261104-e3-compilacao-log.png` | E3: erro CS0029 no registo da etapa 1 |
| `m3-s05-i037-20261104-e3-compilacao-pr248-bloqueado.png` | E3: Pull Request #248 com o merge bloqueado |
| `m3-s05-i037-20261104-e3-compilacao-logs.zip` | E3: registos dos 4 jobs executados |
| `m3-s05-i037-20261104-e4-qg03-execucao.png` | E4: página da execução #227 |
| `m3-s05-i037-20261104-e4-qg03-log.png` | E4: «QG-03 não cumprido» no registo da etapa 4 |
| `m3-s05-i037-20261104-e4-qg03-pr249-bloqueado.png` | E4: Pull Request #249 com o merge bloqueado |
| `m3-s05-i037-20261104-e4-qg03-logs.zip` | E4: registos completos dos 8 jobs |

## 11. Ligações aos commits e às execuções

| ID | Commit | Execução |
|---|---|---|
| E1 | [`89d6b31`](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/commit/89d6b31c5e788b4e8e5cfc7178d991ef65856c06) | [Backend CI #221](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41888968713) |
| E2 | [`a94b715`](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/commit/a94b7155edcdf8d0c0779b09a41573ebfadc26d4) | [Backend CI #225](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891706460) — [PR #247](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/pull/247) |
| E3 | [`79f1c47`](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/commit/79f1c47a6bcb41647a4c04ad978f43f0f6899865) | [Backend CI #226](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891749860) — [PR #248](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/pull/248) |
| E4 | [`6852c44`](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/commit/6852c4461708be065c45ec293936ef6fea1e4a53) | [Backend CI #227](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41891774877) — [PR #249](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/pull/249) |

Os commits de E2 a E4 ficam acessíveis pelos Pull Requests fechados, mesmo depois de os ramos serem apagados.

## 12. Correspondência com os critérios de aceitação da I037

| Critério (Backlog v03) | Como é cumprido | Secções |
|---|---|---|
| 1. Pelo menos uma execução completa com sucesso e três com falha provocada (teste, etapa e limite de qualidade), identificadas por commit | E1 com sucesso; E2 (teste), E3 (etapa de compilação) e E4 (limite do QG-03), cada uma com commit e execução | 3, 4, 11 |
| 2. Cada execução mostra etapas, duração e resultado e liga ao registo ou captura arquivados | Tabelas da secção 4 e uma captura da página e um ZIP de registos por execução | 4 a 8, 10 |
| 3. Resultados dos quality gates (cobertura e análise estática) na execução com sucesso | QG-03 98,51% ≥ 70% e QG-04 com 0 ocorrências que bloqueiam, além de QG-01, QG-02, QG-08 e Newman | 5 |
| 4. Capturas e registos dentro do ZIP, sem depender de ligações externas | 16 capturas e 4 ZIP de registos na pasta `m3-s05-i037-evidencias/` | 10 |
| Descrição: uma falha obrigatória impede a integração | «Resultado da pipeline» obrigatório a falhar e «Merging is blocked» nos três Pull Requests | 9 |

## 13. Observações

| Ref. | Observação |
|---|---|
| O1 | Na falha de compilação (E3), a etapa 4 termina com sucesso sem dados de cobertura. É o comportamento desenhado na I103 (a etapa corre para o resumo mostrar o motivo); o bloqueio vem do «Resultado da pipeline». Fica como sugestão para a I036/I103 a etapa 4 passar a *skipped* ou a falhar quando não há ficheiros de cobertura, para o resultado da etapa ser mais claro. |

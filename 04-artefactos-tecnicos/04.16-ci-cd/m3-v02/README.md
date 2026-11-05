# Pipeline do backend com quality gates — m3-v02

| Campo | Valor |
|---|---|
| Projeto | Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027 |
| Issue | I103 — Integrar análise estática, cobertura e quality gates na pipeline (GitHub #30) |
| Milestone / Sprint | M3 / Sprint 05 |
| Executor / Revisor / Auditor | André Filipe Moreira Mesquita / Ivan Cristiano Silva Mendes / Gonçalo Dinis Magalhães Teixeira |
| Versão | m3-v02 (`/projeto/04-artefactos-tecnicos/04.16-ci-cd/m3-v02`); a m3-v01 (I102) mantém-se sem alterações |
| Plataforma | GitHub Actions, runner `ubuntu-latest` (Regulamento Interno do Grupo v02, secção 11.6) |
| Ficheiro em uso | `.github/workflows/backend-ci.yml` |
| Configuração dos limiares | `.github/quality-gates.json` |
| Referências | Plano de Testes do Backend m3 v01 (I027), secções 7.2, 8.2 e 9; Regulamento Interno do Grupo v02, secções 11.4 a 11.6; Relatório de Análise Estática do Backend v02 (I101); README da Collection Postman (I098, I100) |

## 1. Conteúdo desta pasta

A I102 arquivou a primeira versão da pipeline em `m3-v01` e indicou que uma alteração posterior cria uma versão nova, mantendo a anterior (Regulamento da UC, secção 18.2). Esta pasta é essa versão nova. O Backlog de Projeto v03 indica `m3-v01` como pasta de destino da I103; a pasta `m3-v02` segue a regra de versões da própria `m3-v01` e fica registada como divergência (secção 9).

As pastas começadas por `.` foram copiadas para nomes visíveis, para não ficarem escondidas no ZIP de entrega.

| Ficheiro nesta pasta | Caminho no repositório | Finalidade |
|---|---|---|
| `workflows/backend-ci.yml` | `.github/workflows/backend-ci.yml` | Definição da pipeline (I102 + I103) |
| `actions/preparar-dotnet/action.yml` | `.github/actions/preparar-dotnet/action.yml` | Passos comuns às etapas .NET (sem alterações desde a I102) |
| `configuracao/quality-gates.json` | `.github/quality-gates.json` | **Limiares e âmbitos dos quality gates** (critério 1) |
| `configuracao/Directory.Build.props` | `Directory.Build.props` | Configuração da análise estática da I101, com o relatório SARIF acrescentado pela I103 |
| `scripts/gate-cobertura.py` | `.github/scripts/gate-cobertura.py` | QG-03: filtros do ReportGenerator e comparação com o limiar |
| `scripts/gate-analise-estatica.py` | `.github/scripts/gate-analise-estatica.py` | QG-04: leitura dos relatórios SARIF e comparação com o máximo |
| `scripts/preparar-newman.py` | `.github/scripts/preparar-newman.py` | Newman: palavra-passe descartável e cópia preenchida do ambiente Postman |
| `scripts/resumo-testes.py` | `.github/scripts/resumo-testes.py` | Resumo dos TRX (sem alterações desde a I102) |
| `README.md` | — | Este documento |

## 2. Etapas da pipeline

Os gatilhos, a concorrência e as permissões são os da I102 (`m3-v01`, secção 2). A I103 acrescenta as etapas 4 a 7 como quality gates e liga-as ao **«Resultado da pipeline»**, a verificação obrigatória da proteção dos ramos. A regra de proteção não muda.

| Etapa (job) | Quality gate | Limiar (de `.github/quality-gates.json`) | Bloqueia quando | Relatório anexado |
|---|---|---|---|---|
| **1 · Restauro e compilação (QG-01)** | QG-01 | Zero erros (avisos tratados como erros, I101) | Erro de compilação ou aviso | `relatorio-analise-estatica-sarif` |
| **2 · Testes unitários (QG-02)** | QG-02 | 100% dos testes a passar | Um teste falha | `resultados-testes-unitarios` |
| **3 · Testes de integração (QG-02)** | QG-02 | 100% dos testes a passar | Um teste falha | `resultados-testes-integracao` |
| **4 · Cobertura (QG-03)** | QG-03 | `QG-03.limiarMinimoPercentagem` = 70% de linhas | Cobertura de linhas do âmbito < 70% | `relatorio-cobertura` (completo e `qg-03/`) |
| **5 · Análise estática (QG-04)** | QG-04 | `QG-04.niveisQueBloqueiam` = error, warning; `maximoOcorrencias` = 0 | Mais ocorrências do que o máximo, ou nenhum SARIF | `relatorio-analise-estatica` (SARIF e Markdown) |
| **6 · Formatação (QG-08)** | QG-08 | `QG-08.maximoFicheirosPorFormatar` = 0 | Um ficheiro por formatar | `relatorio-formatacao` (`format-report.json`) |
| **7 · Collection Postman (Newman)** | Critério 3 da I103 | `Newman.maximoFalhas` = 0 | Um pedido ou verificação falhada | `resultado-newman` (consola, JUnit, registo da API e dos dados) |
| **Resultado da pipeline** | Agrega todos | — | Qualquer etapa sem sucesso | — |

As etapas 2, 3, 5, 6 e 7 correm em paralelo depois da 1; a 4 espera pelos testes. As etapas 4 e 5 correm também quando a etapa anterior falha, para o resumo mostrar o motivo.

### 2.1. QG-03 — cobertura

- **Âmbito.** O Regulamento (11.5) e o Plano de Testes (LT-01) aplicam o limiar a «Domínio e Serviços». No código, os serviços (`*Service`) estão em `ClinicaVeterinaria.Infrastructure`, por isso o âmbito inclui `Domain`, `Application` e `Infrastructure`, e exclui o que os dois documentos mandam excluir: DTOs, migrações, `ClinicaDbContext` (configuração do EF Core), `DependencyInjection` e o código gerado em `obj/`. A API (`Program.cs` e controladores) fica fora, como no Plano de Testes. Cada exclusão tem a justificação em `quality-gates.json`.
- **Medição.** Coverlet (`XPlat Code Coverage`) nos testes unitários e de integração; o ReportGenerator junta os dois e gera dois relatórios: o completo (informativo, como na I102) e o do âmbito do QG-03 (`relatorio-cobertura/qg-03`), com os filtros lidos da configuração.
- **Comparação.** `gate-cobertura.py` lê `lines-covered` e `lines-valid` do `Cobertura.xml` do âmbito e compara com o limiar sem arredondamentos (69,99% não passa). A cobertura de ramos (LT-02) aparece no resumo, sem bloquear.

### 2.2. QG-04 — análise estática

A análise Roslyn (perfil `Recommended`, I101) corre na compilação da etapa 1, que já falha com um aviso. A I103 acrescenta ao `Directory.Build.props` a propriedade `PastaRelatoriosAnalise`: quando a pipeline a indica, cada projeto grava um relatório SARIF 2.1 com todas as ocorrências. Localmente, sem a propriedade, nada muda. A etapa 5 conta as ocorrências não suprimidas com os níveis da configuração, compara com o máximo e publica um relatório Markdown por regra e local.

### 2.3. QG-08 — formatação

`dotnet format <solução> --verify-no-changes` com o `.editorconfig` da I101, com o relatório `format-report.json`. O `.editorconfig` fixa `end_of_line = crlf`, mas o repositório guarda os ficheiros com LF: em Windows, onde o grupo trabalha, o Git converte para CRLF no checkout e a verificação passa; no runner Linux, sem conversão, todos os ficheiros apareceriam por formatar. A etapa converte a cópia de trabalho do runner para o final de linha indicado em `QG-08.finalDeLinha` antes de verificar, para o runner verificar o mesmo que um computador do grupo.

### 2.4. Collection Postman com o Newman (critério 3)

1. SQL Server 2025 descartável no próprio runner (a imagem do workflow da I062), com uma palavra-passe gerada na execução.
2. Migrações da I062 (`dotnet ef database update`) e dados de demonstração da I061 (`executar-dados.sh`, com as 28 verificações do script).
3. **Palavra-passe descartável das contas de demonstração.** As palavras-passe reais não estão no repositório. `preparar-newman.py` gera uma palavra-passe nova, pede ao GitHub Actions que a esconda em todos os registos (`::add-mask::`), aplica o hash no formato da API (PasswordHasher v3: PBKDF2-HMAC-SHA512, 100 000 iterações) às contas `@clinica-g08.test` **só nesta base de dados temporária** e escreve, fora do repositório, uma cópia do ambiente Postman com as variáveis `password…` preenchidas.
4. A API compilada nesta execução arranca em `http://localhost:5159` (o `baseUrl` do ambiente), com uma chave JWT gerada na execução.
5. `newman run` da collection inteira, com o relatório da consola e JUnit. A etapa falha se houver uma verificação falhada ou se o Newman terminar com erro.
6. Antes de publicar os resultados, a etapa confirma que nenhum ficheiro contém a palavra-passe descartável. No fim, a API é parada, o contentor destruído e a cópia do ambiente apagada.

## 3. Correspondência com os critérios de aceitação da I103

| Critério | Como é cumprido | Evidência |
|---|---|---|
| 1. Limites lidos de configuração e não escritos no workflow | `.github/quality-gates.json` tem o limiar e o âmbito do QG-03, os níveis e o máximo do QG-04, o máximo do QG-08 e o máximo de falhas do Newman; o workflow só lê o ficheiro (`gate-*.py`, `jq`). | Este README (secção 2) e a execução com o limiar alterado só no ficheiro de configuração (secção 5, 2b) |
| 2. Um limite não cumprido falha a etapa e bloqueia o merge; tudo cumprido passa | Cada gate termina com código 1 quando falha; todas as etapas entram no «Resultado da pipeline», a verificação obrigatória da proteção dos ramos. | Execução verde (2a) e execuções com falha provocada (2b a 2e) |
| 3. Collection Postman completa contra a API arrancada na pipeline; falha se algum pedido falhar | Etapa 7 (secção 2.4). | Artefacto `resultado-newman` e execução 2e |
| 4. Relatórios de cobertura, análise estática e Newman anexados à execução | Artefactos `relatorio-cobertura`, `relatorio-analise-estatica`, `relatorio-analise-estatica-sarif`, `relatorio-formatacao` e `resultado-newman`, durante 90 dias. | Página de cada execução, secção *Artifacts* |

## 4. Reproduzir localmente

Ver a secção «Quality gates — verificar antes de abrir um Pull Request» do `src/backend/README.md`, com os comandos de cada gate. A Collection executa-se com o Newman como descrito no README de `03.01.05-postman`.

## 5. Evidências (Executor) e verificações (Revisor)

As execuções com falha provocada são feitas em ramos temporários a partir do ramo da I103, abertos como Pull Request para `develop` com o título «I103 — falha provocada (…) — NÃO FAZER MERGE» e fechados sem merge, como na I102. Cada um altera **uma** coisa:

| N.º | Execução | Alteração no ramo temporário | Resultado esperado |
|---|---|---|---|
| 2a | Tudo cumprido | — (Pull Request da I103) | Todas as etapas verdes |
| 2b | QG-03 abaixo do limiar | `.github/quality-gates.json`: `limiarMinimoPercentagem` = 99 (só a configuração) | Etapa 4 vermelha com «QG-03 não cumprido»; merge bloqueado |
| 2c | QG-04 com uma ocorrência | Um método de instância que não usa a instância num serviço (CA1822), ou uma variável não usada | Etapas 1 e 5 vermelhas; o relatório indica a regra e a linha |
| 2d | QG-08 com um ficheiro por formatar | Espaços a mais numa linha de um ficheiro `.cs` | Etapa 6 vermelha com o ficheiro no resumo |
| 2e | Newman com um pedido falhado | Na collection, mudar o código esperado de um pedido (por exemplo, `200` para `201` num `pm.response.to.have.status`) | Etapa 7 vermelha com o pedido no resumo |

Ligações às execuções: registadas na descrição do Pull Request da I103.

## 6. Tempos e custos (verificação 3 do Revisor)

Os tempos de cada etapa ficam na página da execução e são registados no Pull Request. As etapas novas correm em paralelo com os testes, por isso a duração total cresce pouco; o custo em minutos cresce com o número de etapas (cada uma é um runner próprio). Se os minutos gratuitos ficarem curtos, o grupo pode restringir o `push` a `develop` e `main` (ver `m3-v01`, secção 9).

## 7. Segurança

- Nenhuma credencial no repositório nem nos *secrets* do GitHub; o workflow mantém `contents: read` e `persist-credentials: false`.
- As credenciais da etapa 7 (SQL Server, JWT e palavra-passe das contas de demonstração) são geradas em cada execução, escondidas nos registos e destruídas com o runner. A palavra-passe descartável só existe na base de dados temporária e não serve para a base de nenhum elemento do grupo nem para a avaliação.
- As ações usadas são as da I102, fixadas pelo *commit* (`m3-v01`, secção 5). O Newman é instalado pelo `npx` na versão de `quality-gates.json`.

## 8. Âmbito e relação com outras tarefas

| Assunto | Situação |
|---|---|
| Entrega contínua | I104 (depende desta tarefa) |
| Workflow `integration-tests.yml` (I062) | Sem alterações |
| Pasta 07 da Collection (I099) | Quando a I099 for integrada, a etapa 7 passa a executá-la sem alterações à pipeline |
| Datas da collection | A pasta 08 usa a marcação M4 da I061 (2026-11-04) e horários livres seguintes; se uma data deixar de ser válida, a etapa 7 falha e a collection tem de ser atualizada (o gate cumpre a sua função) |

## 9. Divergências

| Ref. | Divergência | Tratamento |
|---|---|---|
| DV1 | O Backlog de Projeto v03 indica `04.16-ci-cd/m3-v01` como pasta de destino; essa pasta já tem a versão da I102. | Nova pasta `m3-v02`, como pede a regra de versões da `m3-v01` (secção 1). |
| DV2 | O Plano de Testes refere «Domínio e Serviços (Aplicação)», mas os serviços estão no projeto `Infrastructure`. | O âmbito inclui `Infrastructure`, sem a configuração do EF Core nem as migrações (secção 2.1). |
| DV3 | O Plano de Testes (secção 11, D-05) diz que a Collection na pipeline não é quality gate do Plano de Qualidade; o critério 3 da I103 pede que a faça falhar. | Segue-se a I103: a etapa 7 bloqueia o merge. |

## 10. Histórico de versões

| Versão | Data | Autor | Alteração |
|---|---|---|---|
| m3-v01 | 2026-10-27 | Gonçalo Magalhães Ribeiro | Versão da I102 (pasta `m3-v01`). |
| m3-v02 | 2026-11-03 | André Filipe Moreira Mesquita | I103: QG-03 (cobertura ≥ 70% em Domínio e Serviços), QG-04 (relatório SARIF), QG-08 (formatação) e execução da Collection Postman com o Newman, com os limiares em `.github/quality-gates.json`. |

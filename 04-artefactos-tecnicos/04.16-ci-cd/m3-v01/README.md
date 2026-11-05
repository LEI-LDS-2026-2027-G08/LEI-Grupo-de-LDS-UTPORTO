# Pipeline de compilação e testes do backend — m3-v01

| Campo | Valor |
|---|---|
| Projeto | Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027 |
| Issue | I102 — Criar a pipeline de compilação e testes do backend |
| Milestone / Sprint | M3 / Sprint 04 |
| Executor / Revisor / Auditor | Gonçalo Magalhães Ribeiro / André Filipe Moreira Mesquita / Ivan Cristiano Silva Mendes |
| Versão | m3-v01 (`/projeto/04-artefactos-tecnicos/04.16-ci-cd/m3-v01`) |
| Plataforma | GitHub Actions, runner `ubuntu-latest` (Regulamento Interno do Grupo v02, secção 11.6) |
| Ficheiro em uso | `.github/workflows/backend-ci.yml` (nome fixado no Regulamento do Grupo v02, secção 11.6) |
| Dependências | I058 (solução e API), I059 (EF Core), I062 (migrações e reposição da base de dados), I091 (infraestrutura dos testes de integração); usa também o quality gate da I101 |

## 1. Conteúdo desta pasta

Esta pasta conserva, sem alterações, a configuração da pipeline usada no repositório (Regulamento da UC, secção 18.2: uma alteração posterior cria uma versão nova e esta mantém-se). As pastas começadas por `.` foram copiadas para nomes visíveis, para não ficarem escondidas no ZIP de entrega.

| Ficheiro nesta pasta | Caminho no repositório | Finalidade |
|---|---|---|
| `workflows/backend-ci.yml` | `.github/workflows/backend-ci.yml` | Definição da pipeline: gatilhos, etapas, artefactos e verificação obrigatória |
| `actions/preparar-dotnet/action.yml` | `.github/actions/preparar-dotnet/action.yml` | Passos comuns às etapas .NET: SDK do .NET 10 e cache dos pacotes NuGet |
| `scripts/resumo-testes.py` | `.github/scripts/resumo-testes.py` | Resumo dos ficheiros TRX no resumo da execução (informativo) |
| `README.md` | — | Este documento |

## 2. Quando a pipeline corre (critério 1)

| Gatilho | Configuração | Origem |
|---|---|---|
| Push para qualquer ramo | `push: branches: ["**"]` | Critério 1 da I102: «em cada push» |
| Pull Request para `develop` ou `main` | `pull_request: branches: [develop, main]` | Critério 1 da I102: «em cada Merge Request» |
| Manual | `workflow_dispatch` | Repetir uma verificação ou recolher evidências |

A pipeline não tem filtro de caminhos: corre em todos os Pull Requests, mesmo nos que só mudam documentos. Se tivesse filtro, um Pull Request sem alterações no backend ficava à espera, para sempre, da verificação obrigatória.

Uma nova execução no mesmo Pull Request ou ramo de trabalho cancela a anterior, que já está desatualizada. Em `develop` e `main` nenhuma execução é cancelada.

## 3. Etapas (critério 2)

As etapas são *jobs* separados, cada um com o seu resultado visível no separador *Actions* e na lista de verificações do Pull Request. Dentro de cada etapa, o restauro, a compilação e os testes são passos com nome próprio.

| Etapa (job) | O que faz | Quality gate | Bloqueia quando |
|---|---|---|---|
| **1 · Restauro e compilação (QG-01)** | Restaura as dependências (`dotnet restore`) e compila a solução em `Release` (`dotnet build`), com o `Directory.Build.props` da I101 (análise `Recommended`, avisos tratados como erros, regras de estilo na compilação) | QG-01 — zero erros de compilação | Há um erro de restauro, de compilação ou um aviso |
| **2 · Testes unitários (QG-02)** | Compila e executa `ClinicaVeterinaria.UnitTests`, com resultados TRX e recolha de cobertura (Coverlet) | QG-02 — 100% dos testes a passar | Um teste falha ou fica bloqueado mais de 5 minutos |
| **3 · Testes de integração (QG-02)** | Compila e executa `ClinicaVeterinaria.IntegrationTests`, com resultados TRX e cobertura, contra uma base de dados SQL Server criada para a execução (secção 5). No fim confirma que os contentores SQL Server dos testes foram destruídos | QG-02 — 100% dos testes a passar | Um teste falha, fica bloqueado mais de 10 minutos ou fica um contentor por destruir |
| **4 · Relatório de cobertura (artefacto)** | Junta a cobertura dos testes unitários e de integração e gera o relatório com ReportGenerator 5.5.11 (HTML, Cobertura e resumo) | — (o limiar de 70%, QG-03, é aplicado na I103) | Os testes passaram e não foi produzida cobertura |
| **Resultado da pipeline** | Confirma que as quatro etapas terminaram com sucesso e mostra a tabela final | Agrega QG-01 e QG-02 | Qualquer etapa não terminou com sucesso |

As etapas 2 e 3 correm em paralelo depois da 1. Cada etapa tem um limite de tempo próprio (5 a 15 minutos), para nenhuma execução ficar presa; a duração total esperada fica abaixo dos 15 minutos do critério 1 e é confirmada na execução registada nas evidências.

A verificação **«Resultado da pipeline»** é a que se torna obrigatória na proteção dos ramos (secção 6). Basta uma regra para cobrir todas as etapas, e a regra não precisa de mudar quando a I103 acrescentar etapas.

## 4. Artefactos de cada execução (critério 4)

| Artefacto | Conteúdo |
|---|---|
| `resultados-testes-unitarios` | `testes-unitarios.trx` e `coverage.cobertura.xml` |
| `resultados-testes-integracao` | `testes-integracao.trx` e `coverage.cobertura.xml` |
| `relatorio-cobertura` | Relatório HTML, `Cobertura.xml`, `Summary.txt` e `SummaryGithub.md` |

Os artefactos ficam disponíveis durante 90 dias, na página da execução (secção *Artifacts*). Cada etapa escreve também um resumo legível no separador *Summary* da execução.

## 5. Base de dados e credenciais (critério 3 e verificação 3 do Revisor)

- **Nenhuma instância partilhada.** A infraestrutura de testes da I091 (`IntegracaoFixture`) cria, no próprio runner, um contentor SQL Server 2025 novo pelo Testcontainers, cria uma base de dados com nome único, aplica as migrações EF Core (I062) e carrega os dados de demonstração (I061). Entre testes, a base é reposta com o Respawn. No fim, o contentor é destruído (e, se o processo terminar de forma anormal, o Ryuk do Testcontainers remove-o). A etapa 3 confirma que não ficou nenhum contentor SQL Server dos testes e falha se ficar.
- **Nenhum segredo no repositório nem nos *secrets* do GitHub.** A palavra-passe do SQL Server é a do módulo MsSql do Testcontainers e só serve o contentor descartável, acessível apenas dentro do runner; a chave JWT da API é gerada pela fixture em cada execução. Nenhum valor é guardado no repositório nem configurado no GitHub, por isso a pipeline corre igual num ramo novo ou num *fork* (verificação 2 do Revisor).
- **Privilégio mínimo.** O workflow só tem `contents: read` e o *checkout* não guarda credenciais (`persist-credentials: false`).
- **Ações fixadas pelo *commit*:**

| Ação | Versão | Commit |
|---|---|---|
| `actions/checkout` | v7.0.1 | `3d3c42e5aac5ba805825da76410c181273ba90b1` |
| `actions/setup-dotnet` | v6.0.0 | `a98b56852c35b8e3190ac28c8c2271da59106c68` |
| `actions/cache` | v6.1.0 | `55cc8345863c7cc4c66a329aec7e433d2d1c52a9` |
| `actions/upload-artifact` | v7.0.2 | `cf430e030ddbb5b0abf93d22962f4752f3646cd9` |
| `actions/download-artifact` | v8.0.2 | `9000827ccba6bdab643e8b6fd33ac0654aef8333` |

O SDK instalado é a última versão 10.0 (`dotnet-version: 10.0.x`), a mesma do workflow da I062, que o `src/backend/global.json` aceita (`10.0.100` com `rollForward: latestFeature`).

## 6. Bloqueio do merge (critério 5) — proteção dos ramos

O Regulamento do Grupo v02 (10.4) protege `main` e `develop` com: bloqueio de push direto, Pull Request obrigatório, aprovação mínima de 1 Revisor e passagem nas verificações automáticas. A configuração é feita pelo administrador do repositório, **depois de o workflow estar em `develop`** (e em `main`, quando for integrado). Se a regra for ativada antes, os Pull Requests ficam à espera de uma verificação que ainda não existe.

No GitHub: **Settings → Branches → Add branch protection rule** (ou **Settings → Rules → Rulesets**), uma regra para `develop` e outra para `main`:

1. *Require a pull request before merging*, com *Require approvals* = 1.
2. *Require status checks to pass before merging*, e escolher a verificação **`Resultado da pipeline`** (aparece na lista depois da primeira execução).
3. *Do not allow bypassing the above settings*.
4. Confirmar que *Allow force pushes* e *Allow deletions* ficam desligados.

A captura desta configuração é a evidência 3 do Executor.

## 7. Reproduzir localmente

A partir da raiz do repositório (os testes de integração precisam do Docker):

```bash
dotnet restore src/backend/ClinicaVeterinaria.sln
dotnet build   src/backend/ClinicaVeterinaria.sln --configuration Release --no-restore
dotnet test    src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj --configuration Release --collect:"XPlat Code Coverage"
dotnet test    src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj --configuration Release --collect:"XPlat Code Coverage"
```

## 8. Evidências (Executor) e verificações (Revisor)

| N.º | Evidência da I102 | Como se obtém |
|---|---|---|
| 1 | Ficheiros do workflow no repositório | Pull Request da I102 (`.github/` e esta pasta) |
| 2a | Execução com sucesso | Ligação para a execução verde no Pull Request da I102 e registo descarregado (*Download log archive*) |
| 2b | Execução com falha provocada | Ramo temporário a partir do ramo da I102, aberto como Pull Request para `develop` e fechado sem merge, com: (i) um erro de compilação, por exemplo `int x = "texto";` num ficheiro do backend; (ii) num segundo commit ou ramo, um teste falhado, por exemplo `Assert.True(false);`. As execuções terminam a vermelho e, com a proteção ativa, o botão de merge fica bloqueado |
| 3 | Captura da regra de proteção do ramo | Secção 6, depois de configurada |

O Revisor repete a evidência 2b para os dois casos (verificação 1), confirma a execução num ramo novo (verificação 2) e confirma que o workflow não contém credenciais (verificação 3).

## 9. Âmbito e relação com outros workflows

| Assunto | Situação |
|---|---|
| Limiar de cobertura (QG-03), formatação (QG-08), Collection Postman | Issue I103 (S05). Acrescentam-se como etapas novas, ligadas ao «Resultado da pipeline» |
| Análise estática (QG-04) | Configurada na I101 (`Directory.Build.props`) e aplicada na etapa 1: um aviso faz falhar a compilação |
| Entrega contínua | Issue I104 (S05) |
| Workflow `integration-tests.yml` (I062) | Mantém-se sem alterações: verifica também o script de reposição da base de dados (`scripts/reposicao-base-dados.ps1`), que esta pipeline não usa. Corre só quando há alterações em `src/backend`. Como também executa os testes de integração, a junção dos dois workflows pode ser decidida no Backlog Refinement |
| Consumo de minutos | Num Pull Request, cada push corre a pipeline duas vezes (eventos `push` e `pull_request`), como pede o critério 1. Se os minutos gratuitos ficarem curtos, o grupo pode restringir o `push` a `develop` e `main`, registando a decisão |

## 10. Histórico de versões

| Versão | Data | Autor | Alteração |
|---|---|---|---|
| m3-v01 | 2026-10-27 | Gonçalo Magalhães Ribeiro | Versão inicial: restauro, compilação com o quality gate da I101 (QG-01), testes unitários e de integração (QG-02) com base de dados efémera da I091, relatório de cobertura e verificação obrigatória «Resultado da pipeline». |

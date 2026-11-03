# Relatório de Resultados dos Testes de Integração do Backend — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Relatório de resultados dos testes de integração do backend (OF-M3-013 do Plano de Testes) |
| Ficheiro | `m3-resultados-testes-integracao-backend-v01.pdf` (fonte: `m3-resultados-testes-integracao-backend-v01.md`) |
| Versão | v01 |
| Milestone | m3 |
| Issue | I097 — Executar os testes de integração e produzir o relatório de resultados |
| Sprint | S05 |
| Executor | Gonçalo Magalhães Ribeiro |
| Revisor | Gonçalo Dinis Magalhães Teixeira |
| Auditor | Ivan Cristiano Silva Mendes |
| Versão testada | `develop` no commit `69e70c71486bd927d665c4d1a9212f2b57bd78f6` (2026-11-02 23:15:32 UTC) |
| Pasta | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao` |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-11-03 | Gonçalo Magalhães Ribeiro | Execução da suite completa de testes de integração sobre `69e70c7` (pipeline e reexecução local), análise das falhas, repetição dos testes de concorrência e verificação dos critérios de saída do Plano de Testes. |

## Resumo

Suite completa de testes de integração do backend executada sobre `develop` no commit `69e70c7`, com todas as dependências da I097 integradas. **Pipeline: 463 testes, 463 aprovados, 0 falhados, 0 ignorados.** Duas reexecuções locais seguidas, em clone limpo, com o mesmo resultado e o mesmo conjunto de testes. Testes de concorrência repetidos 13 vezes sem falhas. Os critérios de saída dos testes de integração estão cumpridos; ficam pendentes de outras tarefas os pedidos Postman de EP-021 a EP-030 (I099) e a formatação (QG-08, I103). Identificado um defeito de severidade Média nos testes do boletim, que dependem da data real e falham a partir de 2027-01-13 (secção 6.3).

---

## 1. Objetivo

Documentar de forma verificável o resultado da suite de testes de integração do backend que sustenta a entrega da m3: a versão testada, o ambiente, o comando, o número de casos executados, aprovados, falhados e ignorados, a correspondência com a execução da pipeline, a análise de cada falha, a repetição dos testes de concorrência e o cumprimento dos critérios de saída do Plano de Testes do Backend.

Segue a definição da I097 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog do Sprint 05 v02.

## 2. Documentos de referência

| Documento | Versão | Utilização neste relatório |
|---|---|---|
| Backlog de Projeto (I097) | v03, aprovada como Ready a 2026-10-27 | Critérios de aceitação, evidências e verificações |
| Sprint Backlog do Sprint 05 | v02 (2026-10-30) | Papéis, pontos e prazo |
| Plano de Testes do Backend | `m3-plano-testes-backend-v02.md` | Ambiente (secção 6), comandos (secção 7), critérios de saída (secção 8.2), defeitos (secção 12) |
| Especificação dos Testes de Integração do Backend (I030) | `m3-especificacao-testes-integracao-backend-v01.md` | Casos IT-EP, IT-TRV e IT-CONC; critérios de execução (secção 18); evidências previstas (secção 19) |
| Infraestrutura dos testes de integração (I091) | `m3-i091-infraestrutura-testes-integracao-v01.md` | SQL Server isolado, migrações, dados da I061 e reposição com Respawn |
| Documentos de correspondência caso ↔ teste | I092, I093, I094, I095 e I096 (`04.12-backend/m3-v01`) | Origem de cada teste e casos não implementados |
| Pipeline do backend (I102) | `.github/workflows/backend-ci.yml` no commit `69e70c7` | Comando e ambiente da execução oficial |
| Regulamento de Funcionamento da UC | v19, secções 17 e 19 | Evidências e pasta 03.01.03 |

## 3. Versão testada e âmbito

- **Commit:** `69e70c71486bd927d665c4d1a9212f2b57bd78f6` em `develop`, de 2026-11-02 23:15:32 UTC: «143 i095 implementar os testes de integração de vacinação e internamentos (#229)» (PR #229). É o primeiro commit em que estão integradas todas as dependências da I097 no Backlog v03: I092 (`320781f`), I093 (`548e8d8`), I094 (`38dcc32`), I095 (`69e70c7`), I096 (`189df4c`) e I102 (pipeline, integrada no Sprint 04).
- **Funcionalidades:** F001, F002, F003, F004 e F006 — os 37 endpoints ativos da Documentação da API v02 (EP-001 a EP-034 e EP-042 a EP-044) e a atualização do boletim (RF056). F005 e F007 estão fora do âmbito (Declaração de Âmbito v05).
- **Projeto de testes:** `src/backend/ClinicaVeterinaria.IntegrationTests` — 25 classes de testes e 463 execuções (testes e linhas de testes parametrizados).
- **Fora do âmbito deste relatório:** testes unitários (I090), cobertura (I032), análise estática (I034) e Collection Postman (I098 a I100), que têm relatórios próprios. São referidos na secção 9 só para avaliar os critérios de saída do Plano de Testes.

## 4. Ambiente e comando

### 4.1. Execução oficial — pipeline

| Elemento | Valor |
|---|---|
| Workflow | «Backend CI» (`.github/workflows/backend-ci.yml`), evento `push` em `develop` |
| Execução | Backend CI, execução #133 — https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41544014877 |
| Job | «3 · Testes de integração (QG-02)» — success (4 min 39 s); todos os jobs e «Resultado da pipeline» em success (duração total 5 min 57 s) |
| Runner | GitHub Actions `ubuntu-latest` |
| SDK do .NET | runtime .NET 10.0.14 (SDK 10.0.x instalado pelo setup-dotnet); Ubuntu 24.04.5 LTS; Docker 28.0.4 (`global.json`: 10.0.100, `rollForward: latestFeature`) |
| Base de dados | SQL Server 2025 (`mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04`) no Docker do runner, criado pelo Testcontainers |
| Início e fim dos testes (TRX) | 2026-11-02 23:17:00 — 2026-11-02 23:20:52 (UTC) |
| Resultados | Artefacto `resultados-testes-integracao` (TRX), arquivado como `m3-s05-i097-20261102-resultados-testes-integracao-v01.trx` |
| Execução complementar | Workflow «Testes de integracao» #92 no mesmo commit (`integration-tests.yml`, configuração Debug): success, 5 min 16 s. Não é a verificação obrigatória e não publica TRX |

Comando executado pela pipeline (a partir da raiz do repositório, depois de `dotnet restore` e `dotnet build` do projeto em Release):

```
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj --configuration Release --no-build --results-directory resultados-ci/testes-integracao --logger "trx;LogFileName=testes-integracao.trx" --logger "console;verbosity=normal" --collect:"XPlat Code Coverage" --blame-hang-timeout 10m
```

### 4.2. Reexecução local

| Elemento | Valor |
|---|---|
| Data | 2026-11-02 23:40:44 +00:00 |
| Versão | Clone limpo do commit `69e70c7` numa pasta temporária (`git status` vazio) |
| Sistema operativo | Microsoft Windows 11 Home (versão 10.0.26200, compilação 26200) |
| Processador e memória | 13th Gen Intel(R) Core(TM) i7-13700H (20 processadores lógicos); memória: 31,6 GB |
| SDK do .NET | 10.0.402 |
| Docker | 28.5.2 (linux/amd64) |
| Base de dados | A mesma imagem de SQL Server 2025, criada pelo Testcontainers |
| Compilação | `dotnet build src/backend/ClinicaVeterinaria.sln --configuration Release`: código de saída 0; avisos 0; erros 0; 0 min 35 s |
| Formatação (QG-08) | `dotnet format --verify-no-changes`: código de saída 2 (há ficheiros por formatar — informativo, QG-08 é da I103) |

O comando dos testes é o mesmo da pipeline (secção 4.1), com outra pasta de resultados.

### 4.3. Isolamento da base de dados

Cada classe de testes usa a `IntegracaoFixture` da I091: cria um contentor SQL Server novo, uma base de dados com nome único, aplica as migrações (I062) e os dados de demonstração (I061). Antes de cada teste, o Respawn repõe a base (exceto as tabelas de referência) e volta a carregar os dados da I061. No fim da classe, o contentor é destruído. Não há base partilhada nem dependência da ordem: cada classe usa uma chave JWT aleatória e não tem credenciais versionadas.

Confirmação: na pipeline, o registo do Testcontainers guardado no TRX mostra 25 contentores SQL Server criados e 25 apagados (um por classe de testes, mais o contentor auxiliar Ryuk), e o passo «Confirmar que as bases de dados dos testes foram destruídas» terminou com sucesso; na execução local, o número de contentores SQL Server dos testes ainda existentes foi 0, 0, 0 (depois da 1.ª e da 2.ª execuções completas e no fim das repetições de concorrência).

## 5. Resultados

### 5.1. Resumo das execuções

| Execução | Ambiente | Total | Aprovados | Falhados | Ignorados | Duração dos testes |
|---|---|---|---|---|---|---|
| Pipeline (oficial) | GitHub Actions | 463 | 463 | 0 | 0 | 3 min 52 s |
| Local, 1.ª | Windows + Docker Desktop | 463 | 463 | 0 | 0 | 3 min 39 s |
| Local, 2.ª (seguida) | Windows + Docker Desktop | 463 | 463 | 0 | 0 | 3 min 31 s |

### 5.2. Resultados por área (execução oficial)

| Área | Tarefas | Classes | Execuções | Aprovadas | Falhadas | Tempo dos testes |
|---|---|---|---|---|---|---|
| F001 — Autenticação | I092 | 1 | 16 | 16 | 0 | 1,5 s |
| F001 — Contas de funcionários | I092, I066 | 2 | 47 | 47 | 0 | 0,8 s |
| F001 — Dados da clínica | I092 | 1 | 19 | 19 | 0 | 0,6 s |
| F001 — Clientes | I092 | 1 | 37 | 37 | 0 | 0,7 s |
| F002 — Pacientes | I092, I070 | 2 | 46 | 46 | 0 | 0,9 s |
| F003 — Veterinários e disponibilidade | I093, I071 | 1 | 14 | 14 | 0 | 0,4 s |
| F003 — Concorrência das marcações | I093 | 1 | 9 | 9 | 0 | 0,3 s |
| F003 — Agenda, marcações e área do Cliente | I093, I072 a I076 | 7 | 121 | 121 | 0 | 3,3 s |
| F004 — Registo clínico | I094, I077, I078 | 3 | 44 | 44 | 0 | 1,3 s |
| F006 — Boletim de vacinas e desparasitações | I095, I082 | 2 | 43 | 43 | 0 | 1,2 s |
| Transversal — perfis e isolamento | I096 | 2 | 60 | 60 | 0 | 2,4 s |
| Infraestrutura dos testes | I091 | 2 | 7 | 7 | 0 | 0,1 s |
| **Total** |  | **25** | **463** | **463** | **0** |  |

O tempo por área soma só a duração registada de cada teste; não inclui a preparação de cada classe (arranque do contentor SQL Server, migrações e dados da I061), que ocupa a maior parte da duração total.

### 5.3. Correspondência com a Especificação dos Testes de Integração (I030)

| Secção da I030 | Casos especificados | Casos com teste | Tarefa |
|---|---|---|---|
| 9 — F001 (EP-001 a EP-014) | 63 | 63 | I092 (e I091, I066) |
| 10 — F002 (EP-015 a EP-020) | 25 | 25 | I092 (e I070) |
| 11 — F003 (EP-021 a EP-030) | 60 | 60 | I093 (e I071 a I076) |
| 12 — F004 (EP-031 a EP-034) | 22 | 22 | I094 (e I077, I078) |
| 13 — F006 (EP-042 a EP-044) | 16 | 16 | I095 (e I082) |
| 14.1 e 14.2 — Autorização e isolamento (IT-TRV-01 a 16) | 16 | 16 | I096 |
| 14.3 a 14.6 — Problem Details, persistência, paginação, logs (IT-TRV-17 a 34) | 18 | 2 | I093 (IT-TRV-26 e 28) |
| 15 — Concorrência (IT-CONC-01 a 05) | 5 | 5 | I093 |
| **Total** | **225** | **209** |  |

Os 209 casos implementados têm o identificador completo no nome do teste (`DisplayName`), como pede a I030. A suite tem ainda 128 execuções de testes sem identificador da I030, acrescentados pelas tarefas de implementação e de testes para critérios próprios (por exemplo, perfis sem acesso, pedidos simultâneos e persistência); todos foram executados e aprovados.

**Casos não implementados (16):** `IT-TRV-17` a `IT-TRV-25`, `IT-TRV-27` e `IT-TRV-29` a `IT-TRV-34` (secções 14.3 a 14.6: formato Problem Details, persistência e atomicidade, paginação e ordenação, segurança dos logs). Justificação:

1. Não pertencem à definição de nenhuma tarefa do Backlog de Projeto v03: a I092 a I095 cobrem os casos dos endpoints, a I096 os casos `IT-TRV-01` a `16` e a I093 também os `IT-TRV-26` e `28`. A situação já está registada na revisão da I096 (melhoria M4), para decisão no Backlog Refinement do Sprint 06.
2. Os casos `IT-TRV-17` a `22` descrevem o formato Problem Details (RFC 9457), que a implementação não usa: o formato único da I058 (`code`, `message`, `detail`) é o verificado em todos os erros pelos testes de endpoint (divergência D1 registada na I092 a I096).
3. Parte do comportamento é verificado pelos casos de endpoint: a persistência depois de cada escrita e a ausência de alterações depois de erros (I092 a I095) e os 409 sem alteração do estado (por exemplo `IT-EP014-07`).

Não são falhas: são casos sem teste, justificados, que entram na conclusão (secção 10).

### 5.4. Correspondência entre a pipeline e a execução local

A pipeline e as duas execuções locais executaram **exatamente o mesmo conjunto de 463 testes**: os nomes dos testes do TRX da pipeline e do resumo local coincidem um a um, sem testes só numa das execuções. O resultado é igual nos três casos (todos aprovados), com o mesmo comando, a mesma configuração (Release) e a mesma imagem de SQL Server. As diferenças de duração devem-se à máquina (runner do GitHub com Linux e Docker nativo; Windows com Docker Desktop) e não ao conteúdo da execução.

## 6. Falhas

### 6.1. Falhas nas execuções deste relatório

**Nenhuma.** As três execuções completas (463 × 3 = 1 389 execuções de testes) e as 10 repetições dos testes de concorrência (120 execuções) terminaram sem falhas e sem testes ignorados. O `dotnet test` terminou com código 0 em todas.

### 6.2. Falhas e instabilidades registadas durante o Sprint 05

Falhas observadas noutras execuções da mesma suite durante o Sprint 05, registadas aqui para não se perderem:

| Ref. | Descrição | Causa | Ação | Estado |
|---|---|---|---|---|
| F-S05-01 | Revisão da I094, 1.ª execução local (2026-11-02): os 24 testes de `FuncionariosEndpointsTests` falharam, cada um em 1 ms; as 3 execuções seguintes e a pipeline passaram. | Falha na preparação da classe (fixture: contentor e base da classe), antes de os testes correrem. Classe não alterada pela PR; mensagem não registada. | Registada na revisão da I094 (M3). Repetição da suite. | Não reproduzida: 0 falhas nas execuções deste relatório. |
| F-S05-02 | Pipeline da PR #226 (I088), 2026-11-01: o job «3 · Testes de integração» terminou com 18 testes falhados. A PR só altera testes unitários e documentação; o código testado era o de `develop`. | Não determinada. Mesmo código aprovado noutras execuções: indica instabilidade do ambiente do runner, não defeito do código. | «Re-run failed jobs» sem alterações: todos os testes aprovados. | Não reproduzida: 0 falhas nas execuções deste relatório. |

### 6.3. Risco identificado na análise: testes que dependem da data real

Os testes de integração correm com o relógio real (I091) e os dados de demonstração da I061 têm datas fixas. Na análise do código da suite (`69e70c7`) verifiquei onde isso pode mudar o resultado:

- **Boletim de vacinas — lista «próximas».** A API devolve só as próximas administrações com data igual ou posterior a hoje (`VacinacoesService.Proximas`). Os testes `IT-EP042-01` e `IT-EP044-01` (boletim do Bobi) esperam que as próximas incluam 2027-01-12 e 2027-10-14; o teste da I095 «dar intervalo à desparasitação do Kiko…» espera uma única próxima a 2027-08-01. **Estes testes vão falhar a partir de 2027-01-13** (e de 2027-08-02 e 2027-10-15), sem nenhuma alteração ao código. A I095 já o assinala (nota N2).
- **Marcações da I061 (2026-11-03 a 2026-11-05).** Os testes que precisam de marcações futuras ou passadas criam-nas relativamente à data atual (I076, I073, I074, I093), e as regras com data da API (EP-025 e EP-026: data no futuro) são testadas com datas relativas. Não encontrei dependência destas datas; a reexecução pelo Revisor, depois de 2026-11-03, confirma-o.

**Classificação:** defeito dos testes (não da API, que cumpre o contrato), severidade **Média** (Plano de Testes, secção 12): há solução e não afeta a m3, mas faz falhar a pipeline a partir de 2027-01-13. **Ação proposta:** abrir uma Issue de defeito para a infraestrutura (I091) — registar na `ClinicaWebApplicationFactory` um `TimeProvider` fixo (como o `RelogioFixo` dos testes unitários) ou gerar as datas do boletim relativas à data atual — a decidir no Backlog Refinement do Sprint 06.

## 7. Testes de concorrência

Testes que enviam pedidos em simultâneo ou com versões (`ETag`/`If-Match`) concorrentes: `IT-CONC-01` a `IT-CONC-04`; pedidos em paralelo com o mesmo `If-Match` em EP-026, EP-027 e EP-028; `IT-CONC-05` (duas gravações simultâneas sem `If-Match`, em EP-027 e EP-028) — `ConcorrenciaMarcacoesEndpointsTests` (I093); pedidos simultâneos para a mesma vaga (I073, critério 2); pedidos simultâneos para a mesma consulta (I077); dois pedidos simultâneos com o mesmo e-mail (`IT-EP014-07`, I092).

| Execução | Repetições | Execuções | Aprovadas | Falhadas |
|---|---|---|---|---|
| Pipeline (dentro da suite) | 1 | 12 | 12 | 0 |
| Local, suites completas | 2 | 24 | 24 | 0 |
| Local, repetição isolada | 10 | 120 | 120 | 0 |
| **Total** | **13** | **156** | **156** | **0** |

Os 12 testes de concorrência foram executados 13 vezes no total, sempre com o mesmo resultado: com o mesmo `If-Match`, só um pedido é aceite e o outro recebe 412; sem `If-Match`, só uma gravação é persistida; na mesma vaga e na mesma consulta, só um pedido cria o recurso. As verificações destes testes valem para qualquer ordem de execução dos pedidos no SQL Server (documento da I093), por isso o resultado não depende do acaso. Divergência já conhecida: em `IT-CONC-05` a implementação devolve 412 em vez do 409 da I030 (D1 da I093, para a API v03).

## 8. Defeitos

As execuções deste relatório não detetaram defeitos na API. Defeitos e pontos em aberto relevantes para os testes de integração:

| Ref. | Descrição | Severidade | Situação |
|---|---|---|---|
| Relógio real (secção 6.3) | Testes do boletim dependentes da data real; falham a partir de 2027-01-13 | Média | Novo; proposta de Issue de defeito (I091) |
| D1 (I092 a I096) | Formato de erro da I058 em vez de Problem Details (I030, secção 14.3) | Baixa (documentação) | Conhecido; atualizar a I030 ou a API v03 |
| D1 (I093) | `IT-CONC-05`: 412 em vez de 409 | Baixa (documentação) | Conhecido; registado para a API v03 |
| QG-08 (secção 9) | `dotnet format --verify-no-changes` indica ficheiros por formatar no commit testado | Baixa | Novo; a tratar com a aplicação do QG-08 na pipeline (I103) |
| D-01, D-02, N1 (I086) | Mensagem interna no 500; validação do e-mail; palavra-passe inicial do Cliente | Média | Detetados nos testes unitários; sem Issue de defeito (RTM v02, lacuna L12) |

Não há defeitos de severidade Crítica ou Alta em aberto.

## 9. Critérios de saída do Plano de Testes (secção 8.2)

| Critério | Limiar | Resultado | Evidência |
|---|---|---|---|
| Testes de integração | 100% a passar; zero ignorados sem justificação | **Cumprido**: 463/463 na pipeline e nas duas execuções locais; 0 ignorados | Secção 5.1; TRX |
| Testes unitários | 100% a passar; zero ignorados sem justificação | **Cumprido**: job «2 · Testes unitários» com sucesso na pipeline (50 s); números no relatório da I090 | Pipeline, job 2; I090 |
| Compilação (QG-01) | Zero erros | **Cumprido**: pipeline (job 1) e local: código de saída 0; avisos 0; erros 0; 0 min 35 s | Secção 4 |
| Análise estática (QG-04) | Zero erros e avisos Warning/Error | **Cumprido** na compilação (`TreatWarningsAsErrors`, I101); relatório próprio na I034 | Secção 4; I034 |
| Formatação (QG-08) | Zero ficheiros por formatar | **Não cumprido**: localmente, `dotnet format --verify-no-changes` terminou com código 2 (há ficheiros por formatar). O QG-08 ainda não é aplicado na pipeline (I103) | Secção 4.2; I103 |
| Cobertura de linhas, Domínio e Serviços (QG-03) | ≥ 70% | Medida na pipeline: 97,7% de linhas e 91,1% de ramos (Domain 97,9%, Application 98,2%, Infrastructure 98,2%), unitários e integração em conjunto. Limiar ainda não aplicado na pipeline (I103); relatório formal na I032 | Artefacto `relatorio-cobertura`; I032 |
| Cobertura dos endpoints | 37 endpoints com teste de integração e pedido Postman | **Parcial**: os 37 têm testes de integração; os pedidos Postman de EP-021 a EP-030 dependem da I099 (por entregar) | Secção 5.3; RTM v02, lacuna L10 |
| Cobertura dos requisitos | RF001–RF036, RF043–RF047 e RNF001–RNF005 rastreados para testes | **Dependente da I024**: rastreados na RTM v02, em revisão | RTM v02 |
| Defeitos | Zero Crítica/Alta; Médios e Baixos com Issue e justificação | **Parcial**: zero Crítica/Alta; os Médios (relógio real e D-01/D-02/N1 da I086) ainda não têm Issue de defeito | Secção 8 |
| Matriz de permissões | 100% das combinações endpoint × perfil | **Cumprido**: 38 operações × 6 (5 perfis e anónimo) = 228 verificações, mais que as 222 do plano (inclui RF056) | I096; secção 5.2 |
| Pipeline | Uma execução válida completa e um bloqueio por falha | Execução válida completa: **cumprido** (secção 4.1). Bloqueio por falha: a demonstrar no relatório de evidências da pipeline (I037) | Secção 4.1; I037 |

## 10. Conclusão

A suite de testes de integração do backend, no commit `69e70c7` de `develop`, **executou 463 testes sem falhas e sem testes ignorados**, na pipeline e em duas execuções locais seguidas, com o mesmo conjunto de testes e o mesmo resultado. Os testes de concorrência foram repetidos 13 vezes sem nenhuma falha. A base de dados é isolada por classe e destruída no fim, e os resultados não contêm tokens nem palavras-passe (JWT completos encontrados: 0; cadeias com palavra-passe (Password=/Pwd=) encontradas: 0).

**Critérios de saída do Plano de Testes (secção 9):** os critérios dos testes de integração estão cumpridos — 100% de aprovação, zero ignorados, os 37 endpoints com testes de integração e a matriz de permissões completa. Ficam por cumprir, por dependerem de outras tarefas do Sprint 05: os pedidos Postman de EP-021 a EP-030 (I099); a formatação (QG-08), que localmente indica ficheiros por formatar e ainda não é aplicada na pipeline (I103); e o registo em Issue dos defeitos Médios. A cobertura medida (97,7% das linhas) está muito acima do limiar de 70%, que a I103 vai aplicar como quality gate. Dos 225 casos da I030, 209 têm teste; os 16 restantes (`IT-TRV-17` a `34`, exceto 26 e 28) estão justificados na secção 5.3 e seguem para decisão no Backlog Refinement do Sprint 06.

**Recomendações:** (1) abrir a Issue de defeito do relógio real (secção 6.3) antes de 2027-01-13; (2) decidir os casos `IT-TRV-17` a `34` e alinhar a I030 com o formato de erro da I058; (3) se voltar a acontecer uma falha de preparação de classe (secção 6.2), registar a mensagem do TRX e analisar a `IntegracaoFixture` (I091).

## 11. Critérios de aceitação da I097

| Critério (Backlog v03) | Verificação | Estado |
|---|---|---|
| 1. Commit, ambiente, comando, número de casos, passados, falhados e ignorados | Secções 3, 4 e 5.1 | Cumprido |
| 2. A execução corresponde à da pipeline, cuja ligação ou log é referido | Secções 4.1 e 5.4: mesmo commit, comando e conjunto de testes; ligação à execução; TRX da pipeline arquivado | Cumprido |
| 3. Cada falha com descrição, causa, ação e estado; testes de concorrência repetidos e resultado registado | Secção 6 (sem falhas nas execuções; instabilidades do Sprint e risco do relógio analisados) e secção 7 | Cumprido |
| 4. A conclusão refere o cumprimento dos critérios de saída do plano de testes | Secções 9 e 10 | Cumprido |

**Evidências da Issue:** (1) este relatório em PDF; (2) `m3-s05-i097-20261102-resultados-testes-integracao-v01.trx`, o TRX gerado pela pipeline; (3) a ligação à execução da pipeline (secção 4.1).

## Anexo A — Como repetir a execução

Na raiz de um clone do repositório, com o SDK do .NET 10 e o Docker a correr:

```
git fetch origin
git checkout --detach 69e70c71486bd927d665c4d1a9212f2b57bd78f6
dotnet restore src/backend/ClinicaVeterinaria.sln
dotnet build src/backend/ClinicaVeterinaria.sln --configuration Release --no-restore
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj --configuration Release --no-build --logger "trx;LogFileName=testes-integracao.trx" --logger "console;verbosity=normal" --collect:"XPlat Code Coverage" --blame-hang-timeout 10m
```

Só os testes de concorrência (secção 7):

```
dotnet test src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj --configuration Release --no-build --filter "FullyQualifiedName~ConcorrenciaMarcacoesEndpointsTests|FullyQualifiedName~PedidosSimultaneosParaAMesmaVaga|FullyQualifiedName~PedidosSimultaneosParaAMesmaConsulta|FullyQualifiedName~ConflitoSimultaneoEAtomico"
```

Os números a comparar são os da secção 5.1 (463 execuções, 0 falhadas, 0 ignoradas).

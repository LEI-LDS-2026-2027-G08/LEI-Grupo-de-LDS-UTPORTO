# Relatório de Resultados dos Testes Unitários do Backend — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Relatório de resultados dos testes unitários do backend |
| Ficheiro | `m3-resultados-testes-unitarios-backend-v01.pdf` (fonte: `m3-resultados-testes-unitarios-backend-v01.md`) |
| Versão | v01 |
| Milestone | m3 |
| Issue | I090 — Executar os testes unitários e produzir o relatório de resultados (GitHub #96) |
| Sprint | S05 |
| Executor | Ivan Cristiano Silva Mendes |
| Revisor | André Filipe Moreira Mesquita |
| Auditor | Gonçalo Magalhães Ribeiro |
| Versão testada | `develop` no commit `39040cdc1c6e6c018182c674169037670e90fdf0` (2026-11-03 18:41:10 UTC) |
| Pasta | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios` |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-11-03 | Ivan Cristiano Silva Mendes | Execução da suite completa de testes unitários sobre `39040cd` (pipeline e duas reexecuções locais), correspondência com a Especificação dos Testes Unitários (I028), análise do teste ignorado e verificação dos critérios de saída do Plano de Testes. |

## Resumo

Suite completa de testes unitários do backend executada sobre `develop` no commit `39040cd`, com todas as dependências da I090 integradas. **Pipeline: 718 testes, 717 aprovados, 0 falhados, 1 ignorado.** O teste ignorado (`UT-TRV-007`) está justificado no próprio atributo `Skip`: deteta o defeito D-01, ainda por corrigir (secção 6). Duas reexecuções locais seguidas, num clone limpo, deram o mesmo resultado com o mesmo conjunto de testes. Os **306 casos da Especificação dos Testes Unitários (I028) têm teste**. Os testes unitários, sozinhos, cobrem 96,2% das linhas do âmbito do QG-03 (limiar: 70%). Os critérios de saída dos testes unitários estão cumpridos; ficam dependentes de outras tarefas a cobertura dos requisitos (I024) e o registo em Issue dos defeitos Médios (secção 8).

---

## 1. Objetivo

Documentar de forma verificável o resultado da suite de testes unitários do backend que sustenta a entrega da m3: a versão testada, o ambiente, o comando, o número de casos executados, aprovados, falhados e ignorados, a correspondência com a execução da pipeline, a análise de cada falha ou teste ignorado e o cumprimento dos critérios de saída do Plano de Testes do Backend.

Segue a definição da I090 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog do Sprint 05 v02.

## 2. Documentos de referência

| Documento | Versão | Utilização neste relatório |
|---|---|---|
| Backlog de Projeto (I090) | v03, aprovada como Ready a 2026-10-27 | Critérios de aceitação, evidências e verificações |
| Sprint Backlog do Sprint 05 | v02 (2026-10-30) | Papéis, pontos e prazo |
| Plano de Testes do Backend | `m3-plano-testes-backend-v02.md` | Comandos (secção 7), critérios de saída (secção 8.2), limiares de cobertura (secção 9), defeitos (secção 12) |
| Especificação dos Testes Unitários do Backend (I028) | `m3-especificacao-testes-unitarios-backend-v01.md` | Casos UT-F001 a UT-F006 e UT-TRV; identificadores no nome dos testes (secção 4.3) |
| Documentos das tarefas de testes unitários | I086, I087, I088 e I089 (`04.12-backend/m3-v01`) | Origem de cada teste e casos parametrizados |
| Pipeline do backend (I102, I103) | `.github/workflows/backend-ci.yml` no commit `39040cd` | Comando e ambiente da execução oficial |
| Regulamento de Funcionamento da UC | v19, secções 17 e 19 | Evidências e pasta 03.01.02 |

## 3. Versão testada e âmbito

- **Commit:** `39040cdc1c6e6c018182c674169037670e90fdf0` em `develop`, de 2026-11-03 18:41:10 UTC: «test(postman): pedidos de agenda e área do Cliente (EP-021 a EP-030) [I099] (#231)». Inclui todas as dependências da I090 no Backlog v03: I086 (`7f85c75`), I087 (`73e6b18`), I088 (`4063a13`), I089 (`cdffff8`) e I102 (`37c5f38`). O código em `src` é igual ao de `7227d4d` (I103): as tarefas integradas depois (I034, I099) só alteram documentação e a Collection Postman.
- **Funcionalidades:** F001, F002, F003, F004 e F006 e os comportamentos transversais (paginação, erros e registos). F005 e F007 estão fora do âmbito (Declaração de Âmbito v05).
- **Projeto de testes:** `src/backend/ClinicaVeterinaria.UnitTests` (xUnit) — 46 classes de testes e 718 execuções (testes e linhas de testes parametrizados).
- **Fora do âmbito deste relatório:** testes de integração (I097), cobertura (I032), análise estática (I034) e Collection Postman (I098 a I100), que têm relatórios próprios. São referidos na secção 8 só para avaliar os critérios de saída do Plano de Testes.

## 4. Ambiente e comando

### 4.1. Execução oficial — pipeline

| Elemento | Valor |
|---|---|
| Workflow | «Backend CI» (`.github/workflows/backend-ci.yml`), evento `push` em `develop` |
| Execução | Backend CI, execução #170 — https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41696169008 |
| Job | «2 · Testes unitários (QG-02)» — success (18:42:22 a 18:43:28 UTC); todos os jobs e «Resultado da pipeline» em success |
| Runner | GitHub Actions `ubuntu-latest` (máquina `runnervm48cm3`) |
| SDK do .NET | 10.0.x instalado pelo `setup-dotnet` (`global.json`: 10.0.100, `rollForward: latestFeature`) |
| Início e fim dos testes (TRX) | 2026-11-03 18:43:15 — 18:43:23 (UTC) |
| Resultados | Artefacto `resultados-testes-unitarios` (TRX e cobertura), arquivado como `m3-s05-i090-20261103-resultados-testes-unitarios-v01.trx` |

Comando executado pela pipeline (a partir da raiz do repositório, depois de `dotnet restore` e `dotnet build` do projeto em Release):

```
dotnet test src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj --configuration Release --no-build --results-directory resultados-ci/testes-unitarios --logger "trx;LogFileName=testes-unitarios.trx" --logger "console;verbosity=normal" --collect:"XPlat Code Coverage" --blame-hang-timeout 5m
```

### 4.2. Reexecução local

| Elemento | Valor |
|---|---|
| Data | 2026-11-03 23:52 UTC |
| Versão | Clone limpo do commit `39040cd` numa pasta temporária (`git status` vazio) |
| Sistema operativo | Microsoft Windows 11 Home (versão 10.0.26300, compilação 26300) |
| Processador e memória | AMD Ryzen 7 7735HS (16 processadores lógicos); memória: 15,2 GB |
| SDK do .NET | 10.0.403 |
| Compilação | `dotnet build src/backend/ClinicaVeterinaria.sln --configuration Release --no-restore`: avisos 0; erros 0 |

O comando dos testes é o mesmo da pipeline (secção 4.1), com outra pasta de resultados. Os testes unitários não usam base de dados nem serviços externos: os serviços que dependem do `ClinicaDbContext` usam a base em memória do EF Core e a hora é injetada com um relógio fixo (I028, secções 4.2 e 4.5).

## 5. Resultados

### 5.1. Resumo das execuções

| Execução | Ambiente | Total | Aprovados | Falhados | Ignorados | Duração dos testes |
|---|---|---|---|---|---|---|
| Pipeline (oficial) | GitHub Actions | 718 | 717 | 0 | 1 | 8 s |
| Local, 1.ª | Windows 11 | 718 | 717 | 0 | 1 | 15 s |
| Local, 2.ª (seguida) | Windows 11 | 718 | 717 | 0 | 1 | 8 s |

O `dotnet test` terminou com o código 0 nas três execuções. O teste ignorado é o mesmo nas três (`UT-TRV-007`, secção 6.2).

### 5.2. Resultados por área (execução oficial)

| Área (pasta do projeto de testes) | Funcionalidade | Classes | Execuções | Aprovadas | Ignoradas |
|---|---|---|---|---|---|
| `Seguranca` | F001 — autenticação, token JWT e políticas | 7 | 102 | 102 | 0 |
| `Utilizadores` | F001 — contas de funcionários | 1 | 67 | 67 | 0 |
| `DadosClinica` | F001 — dados e horário da clínica | 1 | 14 | 14 | 0 |
| `Clientes` | F001 — Clientes | 1 | 15 | 15 | 0 |
| `Validacao` | F001 e F002 — palavra-passe, NIF, e-mail e idade | 8 | 88 | 88 | 0 |
| `Animais` | F002 — pacientes | 1 | 30 | 30 | 0 |
| `Agenda` | F003 — disponibilidade | 3 | 65 | 65 | 0 |
| `Veterinarios` | F003 — Veterinários e disponibilidade | 2 | 18 | 18 | 0 |
| `Consultas` | F003 — marcações, estados, notas e versão | 12 | 154 | 154 | 0 |
| `RegistosClinicos` | F004 — registo clínico | 4 | 70 | 70 | 0 |
| `Vacinacao` | F006 — boletim de vacinas e desparasitações | 4 | 83 | 83 | 0 |
| `Middleware` | Transversal — erros e registos | 2 | 12 | 11 | 1 |
| **Total** | | **46** | **718** | **717** | **1** |

### 5.3. Correspondência com a Especificação dos Testes Unitários (I028)

| Secção da I028 | Casos especificados | Casos com teste |
|---|---|---|
| 7.1 — F001 (UT-F001) | 125 | 125 |
| 7.2 — F002 (UT-F002) | 30 | 30 |
| 7.3 — F003 (UT-F003) | 90 | 90 |
| 7.4 — F004 (UT-F004) | 27 | 27 |
| 7.5 — F006 (UT-F006) | 24 | 24 |
| 7.6 — Transversais (UT-TRV) | 10 | 10 |
| **Total** | **306** | **306** |

Os identificadores estão no nome do teste (`DisplayName`), como pede a I028 (secção 4.3). Nos testes parametrizados que verificam vários casos, o nome indica o conjunto, por exemplo «UT-F003-062 a 065», «UT-F003-035/036» ou «UT-F003-078, 079 e 081»; a correspondência caso a caso está nos documentos da I086 a I089. Nenhum teste usa um identificador que não exista na I028. A suite tem ainda 333 execuções sem identificador da I028, acrescentadas pelas tarefas de implementação e de testes para critérios próprios; todas foram executadas e aprovadas.

### 5.4. Correspondência entre a pipeline e a execução local

A pipeline e as duas execuções locais executaram **exatamente o mesmo conjunto de 718 testes**: os nomes do TRX da pipeline e dos TRX locais coincidem um a um, sem testes só numa das execuções. O resultado é igual nos três casos, com o mesmo comando e a mesma configuração (Release). As diferenças de duração devem-se à máquina e ao arranque da primeira execução.

## 6. Falhas e testes ignorados

### 6.1. Falhas

**Nenhuma.** As três execuções (718 × 3 = 2 154 execuções de testes) terminaram sem falhas, sem erros e sem *timeouts*.

### 6.2. Teste ignorado

| Ref. | Teste | Causa | Ação | Estado |
|---|---|---|---|---|
| UT-TRV-007 | «Uma exceção inesperada dá 500 com mensagem genérica, sem a mensagem interna» (`Middleware/ErrosERegistosTests.cs`) | Ignorado de propósito pela I086, com a justificação no atributo `Skip`: deteta o defeito **D-01** — o `ExceptionHandlingMiddleware` copia para `detail` a mensagem de qualquer exceção, também no 500. | Retirar o `Skip` quando o D-01 for corrigido (melhoria M2 da revisão da I086). | Ignorado com justificação; defeito D-01 em aberto (secção 7) |

O critério de saída do Plano de Testes admite testes ignorados com justificação; este é o único.

### 6.3. Dependência da data e da hora

Os testes unitários não dependem da data real: os serviços recebem a hora através de um `TimeProvider` e os testes usam o relógio fixo `RelogioFixo` (I028, secção 4.5). Os únicos testes que usam a hora do sistema são os da emissão e validação do token JWT, que comparam sempre com o momento da própria execução; não têm datas fixas que deixem de ser válidas.

## 7. Defeitos

As execuções deste relatório não detetaram defeitos novos. Defeitos em aberto relevantes para os testes unitários:

| Ref. | Descrição | Severidade | Situação |
|---|---|---|---|
| D-01 (I086) | Mensagem interna da exceção devolvida no 500 (`UT-TRV-007` ignorado) | Média | Conhecido; sem Issue de defeito (RTM v02, lacuna L12) |
| D-02 (I086) | Validação do e-mail | Média | Conhecido; sem Issue de defeito |
| N1 (I086) | Palavra-passe inicial do Cliente | Média | Conhecido; sem Issue de defeito |

Não há defeitos de severidade Crítica ou Alta em aberto.

## 8. Critérios de saída do Plano de Testes (secção 8.2)

| Critério | Limiar | Resultado | Evidência |
|---|---|---|---|
| Testes unitários | 100% a passar; zero ignorados sem justificação | **Cumprido**: 717/717 executados aprovados na pipeline e nas duas execuções locais; 1 ignorado com justificação (D-01) | Secções 5.1 e 6.2; TRX |
| Testes de integração | 100% a passar; zero ignorados sem justificação | **Cumprido**: job «3 · Testes de integração» com sucesso na execução #170; números no relatório da I097 | Pipeline, job 3; I097 |
| Compilação (QG-01) | Zero erros | **Cumprido**: pipeline (job 1) e local: avisos 0; erros 0 | Secção 4 |
| Análise estática (QG-04) | Zero erros e avisos Warning/Error | **Cumprido**: job «5 · Análise estática» com sucesso; relatório próprio na I034 | Pipeline, job 5; I034 |
| Formatação (QG-08) | Zero ficheiros por formatar | **Cumprido**: job «6 · Formatação» com sucesso (aplicado na pipeline desde a I103) | Pipeline, job 6 |
| Cobertura de linhas, Domínio e Serviços (QG-03) | ≥ 70% | **Cumprido**: job «4 · Cobertura» com sucesso. Só os testes unitários cobrem 96,2% das linhas (2070/2150) e 90,1% dos ramos (579/642) no âmbito do QG-03; com os de integração, a pipeline mediu 98,5% no mesmo código de `src` (revisão da I103, `7227d4d`). Relatório formal na I032 | Pipeline, job 4; artefacto `resultados-testes-unitarios`; I032 |
| Cobertura dos endpoints | 37 endpoints com teste de integração e pedido Postman | **Cumprido**: testes de integração dos 37 (I097) e Collection Postman com EP-001 a EP-034 e EP-042 a EP-044 (I098 a I100); job «7 · Collection Postman» com sucesso | I097; I099; pipeline, job 7 |
| Cobertura dos requisitos | RF001–RF036, RF043–RF047 e RNF001–RNF005 rastreados para testes | **Dependente da I024**: rastreados na RTM v02, em revisão | RTM v02 |
| Defeitos | Zero Crítica/Alta; Médios e Baixos com Issue e justificação | **Parcial**: zero Crítica/Alta; os Médios (D-01, D-02, N1) ainda não têm Issue de defeito | Secção 7 |
| Matriz de permissões | 100% das combinações endpoint × perfil | **Cumprido** nos testes de integração (I096) | I096; I097 |
| Pipeline | Uma execução válida completa e um bloqueio por falha | **Cumprido**: execução #170 completa; bloqueio por falha demonstrado na I103 (execuções #139 a #142) | Secção 4.1; I103 |

## 9. Conclusão

A suite de testes unitários do backend, no commit `39040cd` de `develop`, **executou 718 testes: 717 aprovados, 0 falhados e 1 ignorado com justificação**, na pipeline e em duas execuções locais seguidas, com o mesmo conjunto de testes e o mesmo resultado. Os 306 casos da I028 têm teste. Os resultados não contêm tokens nem palavras-passe (JWT completos encontrados: 0; cadeias com palavra-passe (`Password=`/`Pwd=`) encontradas: 0).

**Critérios de saída do Plano de Testes (secção 8):** os critérios dos testes unitários estão cumpridos — 100% dos testes obrigatórios aprovados, um único teste ignorado e justificado, e cobertura muito acima do limiar de 70% (96,2% só com os testes unitários). Ficam por cumprir, por dependerem de outras tarefas: a cobertura dos requisitos (I024) e o registo em Issue dos defeitos Médios D-01, D-02 e N1.

**Recomendações:** (1) abrir a Issue de defeito do D-01 e, depois da correção, retirar o `Skip` do `UT-TRV-007`; (2) abrir as Issues de defeito do D-02 e do N1.

## 10. Critérios de aceitação da I090

| Critério (Backlog v03) | Verificação | Estado |
|---|---|---|
| 1. Versão (commit), ambiente, comando, número de casos, passados, falhados e ignorados | Secções 3, 4 e 5.1 | Cumprido |
| 2. A execução corresponde à da pipeline, cuja ligação ou log é referido | Secções 4.1 e 5.4: mesmo commit, comando e conjunto de testes; ligação à execução; TRX da pipeline arquivado | Cumprido |
| 3. Cada falha tem descrição, causa, ação e estado; não há falhas não explicadas | Secção 6: sem falhas; o teste ignorado tem descrição, causa, ação e estado | Cumprido |
| 4. Conclusão sobre o cumprimento dos critérios de saída do plano de testes | Secções 8 e 9 | Cumprido |

**Evidências da Issue:** (1) este relatório em PDF; (2) `m3-s05-i090-20261103-resultados-testes-unitarios-v01.trx`, o TRX gerado pela pipeline; (3) a ligação à execução da pipeline (secção 4.1).

## Anexo A — Como repetir a execução

Na raiz de um clone do repositório, com o SDK do .NET 10:

```
git fetch origin
git checkout --detach 39040cdc1c6e6c018182c674169037670e90fdf0
dotnet restore src/backend/ClinicaVeterinaria.sln
dotnet build src/backend/ClinicaVeterinaria.sln --configuration Release --no-restore
dotnet test src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj --configuration Release --no-build --logger "trx;LogFileName=testes-unitarios.trx" --logger "console;verbosity=normal" --collect:"XPlat Code Coverage" --blame-hang-timeout 5m
```

Os números a comparar são os da secção 5.1 (718 testes, 717 aprovados, 0 falhados, 1 ignorado).

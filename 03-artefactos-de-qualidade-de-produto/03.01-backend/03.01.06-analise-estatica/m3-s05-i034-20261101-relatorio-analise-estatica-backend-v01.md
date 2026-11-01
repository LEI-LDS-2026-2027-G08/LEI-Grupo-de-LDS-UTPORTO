# Relatório de Análise Estática do Backend — Gestão de Clínica Veterinária

| Campo     | Valor                                                                                    |
| --------- | ---------------------------------------------------------------------------------------- |
| Grupo     | G08                                                                                      |
| Projeto   | Gestão de Clínica Veterinária                                                            |
| Artefacto | Relatório de Análise Estática do Backend                                                 |
| Ficheiro  | `m3-s05-i034-20261101-relatorio-analise-estatica-backend-v01.pdf` (fonte `.md`)          |
| Versão    | v01                                                                                      |
| Milestone | m3                                                                                       |
| Issue     | I034 — Executar a análise estática do backend e produzir o relatório (#64)               |
| Sprint    | S05                                                                                      |
| Executor  | Gonçalo Dinis Magalhães Teixeira                                                         |
| Revisor   | Ivan Cristiano Silva Mendes                                                              |
| Auditor   | Gonçalo Magalhães Ribeiro                                                                |
| Pasta     | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica` |

## Histórico de versões

| Versão | Data       | Autor                            | Descrição |
| ------ | ---------- | -------------------------------- | --------- |
| v01    | 2026-11-01 | Gonçalo Dinis Magalhães Teixeira | Análise sobre o `develop` em `7f85c75`, correção da formatação (`9a14849`), nova análise sobre o ramo atualizado com o `develop` em `16c4bbb` (`d98e1ff`), justificação das ocorrências aceites e log da pipeline do quality gate (execução 41336627718). |

## 1. Objetivo

Demonstrar que o backend cumpre as regras de análise estática do Regulamento Interno do Grupo e do Plano de Qualidade do Processo (quality gate sem `Warning` nem `Error`) e que todas as ocorrências encontradas foram corrigidas ou justificadas. A configuração da análise foi criada pela I101; este relatório regista a sua execução sobre o backend completo do Sprint 05.

## 2. Ferramenta, versão e configuração (critério 1)

| Elemento | Valor |
| --- | --- |
| SDK | .NET SDK 10.0.402 (`src/backend/global.json`), C# 14 |
| Analisadores | Roslyn Analyzers / Microsoft.CodeAnalysis.NetAnalyzers incluídos no SDK, mais os analisadores de estilo (IDE), do ASP.NET Core e do xUnit |
| Perfil | `AnalysisMode = Recommended` (`Directory.Build.props`, na raiz) |
| Quality gate | `TreatWarningsAsErrors = true` e `EnforceCodeStyleInBuild = true`: qualquer aviso de análise ou de estilo faz falhar a compilação (QG-01) |
| Formatação | `dotnet format` do SDK, com as regras do `.editorconfig` versionado: `charset = utf-8`, `end_of_line = crlf`, `insert_final_newline = true`, `trim_trailing_whitespace = true` e indentação de 4 espaços nos `.cs` (QG-08) |
| Exceções da configuração | `IDE0290` (primary constructor) com severidade `suggestion` (decisão da I101); código gerado das migrações do EF Core (`**/Migrations/*.cs`) excluído com `generated_code = true` e severidade `none` |

**Âmbito:** os seis projetos de `src/backend/ClinicaVeterinaria.sln`: `Api`, `Application`, `Domain`, `Infrastructure`, `UnitTests` e `IntegrationTests`, com as funcionalidades F001, F002, F003, F004 e F006 da Declaração de Âmbito v05.

**Commits analisados:**

- antes: `develop` em `7f85c75` (2026-11-01 15:58, depois da integração da I086);
- depois: `d98e1ff` (este ramo), com a correção da formatação descrita na secção 5 (`9a14849`) e atualizado com o `develop` em `16c4bbb` (2026-11-01 17:55, depois da integração da I078). É o código da versão entregue: o commit seguinte (`0e277f7`) e os da atualização deste relatório só alteram ficheiros desta pasta.

**Comandos executados** (na raiz do repositório):

```text
dotnet build src/backend/ClinicaVeterinaria.sln -warnaserror --no-incremental
dotnet format src/backend/ClinicaVeterinaria.sln --verify-no-changes
dotnet format analyzers src/backend/ClinicaVeterinaria.sln --verify-no-changes --severity info
```

O primeiro comando aplica os analisadores com o quality gate; o segundo verifica a formatação; o terceiro lista também as sugestões de nível `info`, que não bloqueiam o quality gate.

## 3. Resultados antes e depois, por severidade (critério 5)

| Severidade / categoria | Antes (`7f85c75`) | Depois (`d98e1ff`) |
| --- | --- | --- |
| `Error` de análise (compilação com `-warnaserror`) | 0 | 0 |
| `Warning` de análise | 0 | 0 |
| Formatação (`.editorconfig`, QG-08) | **2095** | **0** |
| · `WHITESPACE` (indentação e espaços) | 2085 | 0 |
| · `CHARSET` (ficheiros com BOM) | 7 | 0 |
| · `FINALNEWLINE` (sem linha final) | 3 | 0 |
| `Info` (sugestões, não bloqueiam) | 47 | 48 |
| · `IDE0290` | 44 | 45 |
| · `ASP0027` | 1 | 1 |
| · `SYSLIB1045` | 1 | 1 |
| · `xUnit1004` | 1 | 1 |

Não existem ocorrências de severidade alta ou crítica (`Error` ou `Warning`) nem antes nem depois: o código já cumpria o quality gate dos analisadores. As ocorrências a tratar eram de formatação; as de nível `info` estão justificadas na secção 6. O `IDE0290` a mais no depois vem da classe nova `RegistosClinicosLeituraEndpointsTests` (I078), integrada no `develop` entre as duas análises; o código da I078 não trouxe ocorrências de formatação.

Registos completos: `m3-s05-i034-20261101-analise-estatica-antes-v01.txt` e `m3-s05-i034-20261101-analise-estatica-depois-v01.txt`, nesta pasta. Cada um começa com o commit analisado.

## 4. Origem das ocorrências de formatação

As 2095 ocorrências estavam em 22 ficheiros, todos já integrados no `develop` por outras tarefas:

| Origem | Ficheiros |
| --- | --- |
| I063 | `LoginResponseDto.cs`, `UtilizadorAutenticadoDto.cs`, `UtilizadorResumoDto.cs` (sem linha final) |
| I069 | `AnimaisController.cs`, `UpdateAnimalDto.cs`, `AnimaisService.cs` |
| I065 e I098 | `UtilizadoresServiceTests.cs` |
| I074 | `AtualizarConsultaEndpointsTests.cs`, `ExceptionHandlingMiddlewareTests.cs` |
| I082 | `VacinacoesServiceTests.cs` |
| I086 | `AnimaisServiceTests.cs` |
| I091 | os 7 ficheiros de `IntegrationTests/Infraestrutura/` (gravados com BOM) |
| I092 | `AutenticacaoEndpointsTests.cs`, `ClientesEndpointsTests.cs`, `ClinicaEndpointsTests.cs`, `FuncionariosEndpointsTests.cs` (indentação de 2 espaços) |

A formatação não é verificada pela pipeline atual (o `backend-ci.yml` deixa o QG-08 fora do âmbito e atribui-o à I103), por isso estas ocorrências passaram pelas PR sem falhar.

## 5. Correções (critério 2)

Correção feita com `dotnet format whitespace src/backend/ClinicaVeterinaria.sln`, que só altera espaços, quebras de linha, codificação e linha final, segundo o `.editorconfig`. Commit `9a14849` (22 ficheiros).

Verificação de que a correção não altera o código:

- `git diff -w --ignore-blank-lines` (ignorando espaços) só mostra a remoção do BOM na primeira linha dos 7 ficheiros de `Infraestrutura/` e, em `AnimaisServiceTests.cs`, a inicialização de um `Paciente` passada de duas linhas para uma linha por campo, com os mesmos campos e valores;
- testes locais depois da correção (`9a14849`): **unitários 673, 672 aprovados, 0 falhados, 1 ignorado** (UT-TRV-007, ver 6); **integração 343, 343 aprovados, 0 falhados** (2 min 53 s, SQL Server com Testcontainers);
- testes na pipeline sobre a versão entregue: jobs «2 · Testes unitários (QG-02)» e «3 · Testes de integração (QG-02)» da execução 41336627718, com sucesso.

## 6. Ocorrências aceites e justificações (critério 4)

| Regra | N.º | Onde | Justificação | Seguimento |
| --- | --- | --- | --- | --- |
| `IDE0290` — usar *primary constructor* | 45 | Controladores, serviços, middleware, exceções e classes de teste | Preferência de estilo, não defeito. A I101 fixou a severidade em `suggestion` no `.editorconfig` («primary constructors são uma preferência estilística, não uma condição do quality gate»). Converter 45 construtores mudaria a forma de 45 classes sem ganho funcional. | Aceite pela configuração. |
| `ASP0027` — `public partial class Program { }` já não é necessário | 1 | `Api/Program.cs` | Desde o .NET 10 o `Program` gerado já é público. A declaração mantém-se porque é a referência usada pelo `WebApplicationFactory<Program>` dos testes de integração (I091); retirá-la exige voltar a validar os 343 testes de integração. | Aceite; pode ser removida numa alteração futura à infraestrutura de testes. |
| `SYSLIB1045` — usar `[GeneratedRegex]` | 1 | `IntegrationTests/Infraestrutura/IntegracaoFixture.cs` | Sugestão de desempenho (expressão regular gerada em compilação) num ponto do código de testes executado poucas vezes por execução; sem impacto no produto. | Aceite. |
| `xUnit1004` — teste com `Skip` | 1 | `UnitTests` — UT-TRV-007 | Ignorado de propósito pela I086, com a justificação no atributo: o caso deteta o defeito D-01 do `ExceptionHandlingMiddleware` (devolve a mensagem interna no 500). | Retirar o `Skip` quando o D-01 for corrigido (melhoria M2 da revisão da I086). |

Não foram identificados falsos positivos: as quatro regras acima são ocorrências reais, aceites com a justificação indicada.

## 7. Quality gate e pipeline (critério 3)

- **Analisadores (QG-01):** a pipeline Backend CI compila a solução com os avisos tratados como erros (`Directory.Build.props` da I101), no job «1 · Restauro e compilação (QG-01)»; qualquer `Warning` ou `Error` de análise faz falhar a etapa e bloqueia o merge.
- **Evidência (critério 3):** [Backend CI, execução 41336627718](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41336627718), do evento `pull_request` da PR #225 sobre o commit `0e277f7` (o resultado do merge com o `develop`, ou seja, o que vai ser integrado). O log do job 1 está arquivado nesta pasta, em `m3-s05-i034-20261101-log-pipeline-qg01-v01.txt`, e termina com `Build succeeded.`, `0 Warning(s)` e `0 Error(s)`.
- **Formatação (QG-08):** não faz parte do quality gate da pipeline atual; a sua integração é o objeto da I103 (Integrar análise estática, cobertura e quality gates na pipeline), fora do âmbito desta tarefa. O resultado é comprovado pelo registo local `…-analise-estatica-depois-v01.txt` sobre `d98e1ff` (0 ocorrências).

## 8. Resultado

| Quality gate | Resultado |
| --- | --- |
| Zero `Warning` e `Error` de análise (QG-01) | Cumprido, antes e depois; comprovado na pipeline (execução 41336627718) |
| Zero ocorrências de formatação (QG-08) | Cumprido depois da correção (2095 → 0) |
| Sugestões `info` | 48, todas justificadas (secção 6) |

## 9. Pendentes

1. Correção do defeito D-01 (I086) e reativação do UT-TRV-007 (fora do âmbito desta tarefa).
2. Integração da verificação de formatação (QG-08) na pipeline pela I103 (fora do âmbito desta tarefa).

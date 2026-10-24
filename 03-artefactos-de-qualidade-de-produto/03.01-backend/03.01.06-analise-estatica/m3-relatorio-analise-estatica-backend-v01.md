# Relatório de Análise Estática do Backend — Gestão de Clínica Veterinária

| Campo     | Valor                                                                                    |
| --------- | ---------------------------------------------------------------------------------------- |
| Grupo     | G08                                                                                      |
| Projeto   | Gestão de Clínica Veterinária                                                            |
| Artefacto | Relatório de Análise Estática do Backend                                                 |
| Ficheiro  | `m3-relatorio-analise-estatica-backend-v01.md`                                           |
| Versão    | v01 — rascunho do Executor                                                               |
| Milestone | m3                                                                                       |
| Issue     | I101 — Configurar a análise estática e o quality gate do backend                         |
| Sprint    | S04                                                                                      |
| Executor  | Gonçalo Dinis Magalhães Teixeira                                                         |
| Revisor   | Gonçalo Magalhães Ribeiro                                                                |
| Auditor   | Ivan Cristiano Silva Mendes                                                              |
| Pasta     | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica` |

## Histórico de versões

| Versão | Data       | Autor                            | Descrição                                                                                                                       |
| ------ | ---------- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| v01    | 2026-10-24 | Gonçalo Dinis Magalhães Teixeira | Configuração inicial da análise estática do backend, definição do quality gate, registo da baseline e primeira execução válida. |

---

## 1. Objetivo

A análise estática do backend tem como objetivo detetar automaticamente problemas de qualidade, manutenção, estilo e utilização incorreta das APIs antes da integração do código.

A configuração é versionada no repositório e destina-se à execução local e posterior integração na pipeline CI/CD do projeto.

A configuração segue o Regulamento Interno do Grupo G08 v02 e o Plano de Qualidade v02, que definem para o backend a utilização dos analisadores Roslyn e um quality gate que exige zero problemas classificados como `Warning` ou `Error`.

---

## 2. Âmbito

A análise é aplicada aos projetos do backend:

- `ClinicaVeterinaria.Api`;
- `ClinicaVeterinaria.Application`;
- `ClinicaVeterinaria.Domain`;
- `ClinicaVeterinaria.Infrastructure`;
- `ClinicaVeterinaria.UnitTests`;
- `ClinicaVeterinaria.IntegrationTests`.

A configuração é transversal ao backend e não introduz dependências relativamente às funcionalidades removidas pela Declaração de Âmbito v05.

A baseline funcional considerada é constituída por:

- F001 — Autenticação e Controlos de Acesso;
- F002 — Gestão de pacientes;
- F003 — Agendamento de consultas;
- F004 — Registo clínico e tratamentos;
- F006 — Boletim de Vacinas e Desparasitações.

F005 — Gestão de contas e pagamentos e F007 — Gestão de internamento encontram-se fora do âmbito atual e não são consideradas na evolução do backend.

---

## 3. Baseline tecnológica

A baseline tecnológica definida para a Sprint 04 é:

- .NET 10;
- C# 14;
- ASP.NET Core 10;
- Roslyn Analyzers incluídos no .NET SDK 10;
- perfil `Recommended`.

A migração efetiva do backend de .NET 8 para .NET 10 pertence à I059.

Por esse motivo, a primeira execução da I101 foi realizada sobre o estado transitório atual da branch `develop`, ainda baseado em .NET 8. Após a integração da I059, a análise deve ser novamente executada em .NET 10 antes do fecho definitivo da I101.

---

## 4. Configuração adotada

Foram acrescentados à raiz do repositório:

- `.editorconfig`;
- `Directory.Build.props`.

O `Directory.Build.props` ativa:

```xml
<AnalysisMode>Recommended</AnalysisMode>
<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
```

Desta forma, as regras do perfil `Recommended` dos analisadores .NET são executadas durante o build e os avisos que fazem parte do quality gate impedem uma integração inválida.

---

## 5. Convenções configuradas

O `.editorconfig` define, entre outras, as seguintes regras:

- codificação UTF-8;
- finais de linha CRLF;
- newline obrigatório no fim dos ficheiros;
- remoção de whitespace no fim das linhas;
- indentação de quatro espaços para C#;
- namespaces `file-scoped`;
- organização consistente dos `using`;
- preferências relativas à utilização de `var`.

### 5.1. Primary constructors

A regra `IDE0290` foi definida com severidade `suggestion`.

A utilização de primary constructors é considerada uma preferência estilística e não um problema de qualidade suficientemente grave para bloquear uma integração.

A configuração mantém a sugestão visível ao programador, sem a incluir nas condições de bloqueio do quality gate.

### 5.2. Código gerado pelo Entity Framework Core

Os ficheiros existentes em:

```text
**/Migrations/*.cs
```

são identificados como código gerado e excluídos da análise.

Esta decisão segue o Regulamento do Grupo G08, uma vez que as migrations são geradas automaticamente pelo Entity Framework Core e não devem produzir falhas de qualidade sobre código que não é mantido manualmente.

---

## 6. Quality gates aplicáveis

### 6.1. QG-04 — Análise estática

Comando local utilizado:

```powershell
dotnet build ClinicaVeterinaria.sln --no-restore -warnaserror
```

Critério de aprovação:

```text
0 Warning(s)
0 Error(s)
```

Qualquer problema de severidade `Warning` ou `Error` provoca falha do build e bloqueia a integração.

### 6.2. QG-08 — Formatação

Comando local utilizado:

```powershell
dotnet format ClinicaVeterinaria.sln --verify-no-changes
```

Critério de aprovação:

- zero violações de formatação;
- zero ficheiros que necessitem de alteração.

---

## 7. Primeira execução — baseline

Na primeira execução da análise estática foram encontrados:

```text
Build succeeded.
3 Warning(s)
0 Error(s)
```

A evidência encontra-se em:

```text
m3-analise-estatica-backend-baseline-v01.txt
```

A baseline identificou três ocorrências da regra `CA1848`, todas na camada API:

- `ExceptionHandlingMiddleware.cs` — 1 ocorrência na utilização de `LogError`;
- `RequestLoggingMiddleware.cs` — 2 ocorrências na utilização de `LogInformation`.

A regra `CA1848` recomenda a utilização de delegates `LoggerMessage` para evitar custos desnecessários associados às chamadas tradicionais das extensões de logging.

As restantes alterações de estilo e formatação foram identificadas posteriormente durante a configuração e validação do `dotnet format`, não fazendo parte dos três warnings da baseline Roslyn.

---

## 8. Correções efetuadas

A configuração foi ajustada para refletir explicitamente as convenções do projeto, evitando que preferências puramente estilísticas sejam tratadas como defeitos críticos.

Foram ainda realizadas correções de código identificadas pela análise estática.

### 8.1. Logging

Os middlewares:

- `RequestLoggingMiddleware`;
- `ExceptionHandlingMiddleware`;

foram alterados para utilizar `LoggerMessage`, eliminando as violações `CA1848`.

Esta alteração reduz a criação desnecessária de objetos durante operações de logging e segue a recomendação dos analisadores .NET.

### 8.2. Namespaces

Os projetos de testes foram alinhados com a convenção de namespaces `file-scoped` adotada no backend.

### 8.3. Imports e formatação

Foram corrigidos:

- ordem dos imports;
- finais de linha;
- newline final dos ficheiros;
- restantes diferenças detetadas por `dotnet format`.

---

## 9. Resultado após as correções

Após configuração e correção dos problemas identificados, foi executado:

```powershell
dotnet build ClinicaVeterinaria.sln --no-restore -warnaserror
```

Resultado:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

Foi igualmente executado:

```powershell
dotnet format ClinicaVeterinaria.sln --verify-no-changes
```

sem serem identificadas alterações de formatação necessárias.

A evidência da execução encontra-se em:

```text
m3-analise-estatica-backend-final-v01.txt
```

Este resultado corresponde ao estado atual do backend antes da integração da migração .NET 10 da I059.

Após a integração da I059, estes comandos devem ser novamente executados para validar a baseline tecnológica definitiva da Sprint 04.

---

## 10. Tratamento de falsos positivos e exceções

Uma regra de análise estática não deve ser desativada silenciosamente apenas para permitir a passagem do quality gate.

Quando uma regra for considerada inadequada ao contexto do projeto:

1. deve ser avaliada tecnicamente;
2. a exclusão ou alteração da severidade deve possuir justificação escrita;
3. a decisão deve ficar visível no `.editorconfig`;
4. deve ser identificada no Pull Request correspondente;
5. quando relevante, deve ser registada numa Issue ou decisão do grupo.

Não são permitidas supressões globais sem justificação.

As exceções devem ser tão específicas quanto possível.

---

## 11. Integração na pipeline

A configuração criada na I101 fica preparada para utilização pela pipeline do backend.

A I102 é responsável pela pipeline de compilação e testes, onde devem ser aplicados os quality gates definidos pelo grupo.

A integração deve executar, entre outras verificações:

```powershell
dotnet build ClinicaVeterinaria.sln --no-restore -warnaserror
dotnet format ClinicaVeterinaria.sln --verify-no-changes
```

Uma falha em qualquer um destes comandos deve provocar o insucesso da pipeline e impedir a integração do Pull Request.

---

## 12. Evidências

| Evidência               | Localização                                                     |
| ----------------------- | --------------------------------------------------------------- |
| Configuração das regras | `/.editorconfig`                                                |
| Configuração MSBuild    | `/Directory.Build.props`                                        |
| Baseline inicial        | `m3-analise-estatica-backend-baseline-v01.txt`                  |
| Execução após correções | `m3-analise-estatica-backend-final-v01.txt`                     |
| Comando QG-04           | `dotnet build ClinicaVeterinaria.sln --no-restore -warnaserror` |
| Comando QG-08           | `dotnet format ClinicaVeterinaria.sln --verify-no-changes`      |

---

## 13. Estado da I101

À data desta versão:

- configuração de análise estática criada;
- regras e severidades versionadas;
- exclusão das migrations documentada;
- tratamento de falsos positivos definido;
- primeira execução registada;
- baseline inicial registada;
- QG-04 executado com sucesso no backend atual;
- QG-08 executado com sucesso no backend atual;
- validação final em .NET 10 pendente da integração da I059;
- integração automática na pipeline dependente da I102.

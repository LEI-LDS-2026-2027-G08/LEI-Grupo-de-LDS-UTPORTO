# Registo da verificação da Descrição da Pipeline de CI/CD do backend

| Campo | Valor |
|---|---|
| Grupo | G08 — Gestão de Clínica Veterinária |
| Issue | I036 (GitHub #65) — critério de aceitação 5 e evidência 2 do Executor |
| Documento seguido | `m3-descricao-pipeline-ci-cd-v01.md`, secção 12, commit: `33f9c9e` (ramo `65-i036-documentar-pipeline-ci-cd`) |
| Autor do documento | André Filipe Moreira Mesquita |
| Verificado por | Gonçalo Ribeiro |
| Data e hora | 2026-11-04, 14:12 — 14:47 |
| Ramo novo criado | `teste-pipeline-goncalo-ribeiro`, a partir de `develop` no commit `eab3945` |

## Resultado por passo da secção 12

| Passo | Resultado esperado obtido? | Ligação ou observações |
|---|---|---|
| 1. Criar o ramo a partir de `develop` | Sim | Ramo `teste-pipeline-goncalo-ribeiro` criado a partir de `develop` (`eab3945`). |
| 2. Commit e push; execução nova em *Actions → Backend CI* | Sim | Commit `cc0c990`. Ligação da execução: [#201](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41845646144) (evento `push`). O comando `Add-Content` só funcionou no PowerShell (ver desvio 1). |
| 3. Etapas 1 a 7 e «Resultado da pipeline» verdes; etapa 8 *skipped*; 7 artefactos | Sim | Duração total: 5 min 9 s. Etapas 1 a 7 e «Resultado da pipeline» verdes, etapa 8 *skipped*, os 7 artefactos da secção 7. |
| 4. (Opcional) Pull Request *draft* mostra «Resultado da pipeline» obrigatório | Sim | Ligação do Pull Request: [#245](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/pull/245). «Resultado da pipeline» aparece como *Required* (eventos `push` e `pull_request`); 16 verificações com sucesso e 2 *skipped* (etapa 8). |
| 5. (Opcional) Falha provocada na formatação: etapa 6 e «Resultado da pipeline» vermelhos | Sim | Ligação da execução: [#205](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41849720537) (commit `84bdba5`). Etapa 6 e «Resultado da pipeline» vermelhos; o resumo indica `ClinicaVeterinaria.Api/Program.cs`, linha 10 (WHITESPACE). Na primeira tentativa ([#203](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41848039706), commit `1353fb6`) foi acrescentada uma linha nova de comentário com espaços no fim, no final do ficheiro, e a etapa 6 ficou verde (ver observação). |
| 6. Ramo apagado e Pull Request fechado sem merge | Sim | Ramo remoto e local apagados; Pull Request #245 fechado sem merge. |

## Desvios encontrados e correções

| N.º | Secção ou passo | Desvio | Correção no documento (commit) |
|---|---|---|---|
| 1 | Secção 12, passo 2 | Os comandos estão num bloco `powershell`, mas o texto não diz que têm de ser corridos no PowerShell. Na Linha de Comandos (`cmd`), que é o que abre o GitHub Desktop em *Repository → Open in Command Prompt*, o `Add-Content` falha («não é reconhecido como comando») e o commit fica vazio. Sugestão: indicar «abrir o PowerShell» antes do passo 1, ou dar também a versão para `cmd`. | `63e041b`: a secção 12 passa a indicar, antes do passo 1, que os comandos são de PowerShell e que não se deve usar a Linha de Comandos (`cmd`); o passo 5 passa a pedir espaços no fim de uma linha de código já existente (observação deste registo) |

**Observação (não é desvio):** no passo 5, espaços no fim de uma linha nova de comentário acrescentada no final do ficheiro não foram detetados pela etapa 6. Espaços no fim de uma linha de código existente, que é o que o documento indica, foram detetados como previsto. Pode valer a pena dizer explicitamente «uma linha de código já existente».

## Conclusão

Sim. Seguindo o documento, a pipeline foi executada num ramo novo e todos os passos da secção 12, incluindo os opcionais 4 e 5, deram o resultado esperado. Foi encontrado um desvio menor (n.º 1), que não impediu a verificação, a corrigir pelo autor.

Não registar neste ficheiro palavras-passe, tokens ou o conteúdo dos secrets.

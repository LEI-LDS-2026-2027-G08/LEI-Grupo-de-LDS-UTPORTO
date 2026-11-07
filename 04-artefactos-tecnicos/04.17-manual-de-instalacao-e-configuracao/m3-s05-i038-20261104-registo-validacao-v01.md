# Registo da Validação do Manual de Instalação e Configuração do Backend — I038

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto validado | `m3-manual-instalacao-configuracao-v01.pdf` (fonte `.md`), em `04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao` |
| Issue | I038 — Elaborar o Manual de Instalação e Configuração do backend (GitHub #67) — critério de aceitação 4 e evidência 2 |
| Sprint | S05 |
| Autor do manual (Executor) | André Filipe Moreira Mesquita |
| Validador | Gonçalo Dinis Magalhães Teixeira (elemento do grupo diferente do autor, do Revisor e do Auditor da I038) |
| Revisor | Gonçalo Magalhães Ribeiro |
| Auditor | Ivan Cristiano Silva Mendes |
| Versão do registo | v01 |

## 1. Quando e com que versões

| Passagem | Data e hora | Manual validado | Código do backend |
|---|---|---|---|
| 1.ª — validação completa | 2026-11-04, 01:14 a 01:16 | commit `5f03308` (2026-11-04 01:07) | `develop` em `df5b5f2` |
| 2.ª — confirmação das correções (passos 3.2, 4.1 e 6.3) | 2026-11-04, 01:28 a 01:29 | commit `515c6b0` (2026-11-04 01:26), PDF com SHA-256 `60cc5f53e928ab04…` | `develop` em `eab3945` (face a `df5b5f2` só acrescenta o relatório de cobertura da I032; sem alterações ao código) |

## 2. Máquina e ambiente

| Elemento | Valor |
|---|---|
| Máquina | Portátil do validador, Windows 11 Home (10.0.26200), 64 bits |
| PowerShell | Windows PowerShell 5.1.26100 (Desktop) — o terminal por omissão do Windows e do VS Code |
| .NET SDK | 10.0.403 |
| SQL Server | SQL Server 2025 (RTM-GDR) 17.0.1145.4, instância por omissão, autenticação do Windows |
| Opção validada | **Opção A** — SQL Server 2025 instalado no computador (autenticação do Windows) |
| Opção não validada | **Opção B** — SQL Server 2025 em Docker (autenticação SQL): **não foi validada** |

**Ambiente limpo.** A validação seguiu o manual de raiz numa cópia nova do repositório, numa pasta nova, sem ferramentas nem segredos do projeto: o `dotnet-ef` foi desinstalado antes do passo 1.3, os user secrets da API foram retirados antes do passo 4.1 (e repostos no fim) e a base `ClinicaVeterinaria` foi apagada e recriada pelo passo 3.1. O .NET SDK, o Git, o `sqlcmd` e o SQL Server 2025 já estavam instalados no computador, por isso os passos de instalação desses pré-requisitos (secção 2) não foram repetidos; as versões foram confirmadas nos passos 1.2 e 2.A.2.

**Como foi feita a validação.** Os comandos de cada passo foram copiados do manual e executados pela ordem indicada, numa janela do Windows PowerShell 5.1, através de um script do validador que corre esses comandos sem os alterar e regista, para cada passo, o comando, o resultado esperado no manual e o resultado obtido. O validador conferiu o resultado de cada passo. A escrita da palavra-passe no passo 6.3 foi feita e observada pelo próprio validador nas duas passagens (foi assim que se detetou o desvio D1 e que se confirmou a correção). O Swagger (passo 6.2) foi verificado pelo pedido a `http://localhost:5159/swagger` (resposta 200) e pela contagem das operações no `swagger.json` (38).

## 3. Resultado por passo (1.ª passagem, manual `5f03308`)

| Passo | Resultado esperado no manual | Resultado obtido | Estado |
|---|---|---|---|
| 1.1 — clonar e mudar para `develop` | `src/backend` com `ClinicaVeterinaria.sln` | Existe (`develop` em `df5b5f2`) | OK |
| 1.2 — confirmar o SDK | `10.0.x` | `10.0.403` | OK |
| 1.3 — instalar o `dotnet-ef` | 10.0.13 | 10.0.13 | OK |
| 1.4 — compilar | `Build succeeded`, 0 avisos, 0 erros | `Build succeeded`, 0 avisos, 0 erros | OK |
| 2.A.2 — ligação ao SQL Server | Microsoft SQL Server 2025 | Microsoft SQL Server 2025 (RTM-GDR) 17.0.1145.4 | OK |
| 3.1 — base e migrações | `[1/4]` a `[4/4]` e «Reposicao concluida com sucesso»; 16 tabelas; a clínica de demonstração | Igual; 16 tabelas; «Clínica Veterinária G08» | OK |
| 3.2 — dados de demonstração | 28 verificações, 28 OK; 2.ª execução com 0 linhas inseridas | 28 verificações, 28 OK, 0 falhadas; 2.ª execução sem inserções | OK, com o desvio D2 |
| 4.1 — chave JWT nos user secrets | `Successfully saved Jwt:Key` | Igual | OK, com o desvio D2 |
| 4.2 — cadeia de ligação (opção A) | `Successfully saved ConnectionStrings:ClinicaDb` | Igual | OK |
| 5.1 — arrancar a API | `Now listening on: http://localhost:5159` e `Application started` | Igual | OK |
| 6.1 — API exige autenticação | `401 Unauthorized` com o corpo no formato da I058 | Igual | OK |
| 6.2 — Swagger | Swagger UI com 38 operações | Swagger UI (200), 38 operações | OK |
| 6.3 — login do Administrador | Perfil Administrador e token | Perfil Administrador e token, mas a palavra-passe ficou visível no ecrã | **Desvio D1** |
| 6.4 — pedido autenticado | `nome = Clínica Veterinária G08` | Igual | OK |
| 7.3 — testes unitários (opcional) | 718 testes: 717 aprovados, 1 ignorado | 718 testes: 717 aprovados, 0 falhados, 1 ignorado | OK |

Observação: o registo automático usado pelo validador marcou o passo 7.3 como desvio por um erro do próprio script (procurava a linha de resumo noutro formato); o resultado obtido é igual ao do manual, pelo que não é um desvio do manual.

## 4. Desvios encontrados e correção

| Ref. | Passo(s) | Desvio | Correção no manual | Commit da correção | Confirmação (2.ª passagem, `515c6b0`) |
|---|---|---|---|---|---|
| D1 | 6.3 e 2.B.1 | No Windows PowerShell 5.1, `Read-Host -MaskInput` não dá erro: o `-MaskInput` é tratado como parte do texto da pergunta e a palavra-passe aparece em claro no ecrã ao ser escrita. O aviso do manual só existia no passo 2.B.1 e indicava que o comando falhava. | Os passos 2.B.1 e 6.3 passam a usar `Read-Host -AsSecureString` com a conversão para texto (`PtrToStringBSTR`) e `Remove-Variable` no fim; a tabela dos pré-requisitos avisa para não usar `-MaskInput` no 5.1. | `515c6b0` | 6.3: corrigido e confirmado — no Windows PowerShell 5.1 a palavra-passe aparece só como asteriscos; login com perfil Administrador e token; `GET /clinica` com 200. 2.B.1: corrigido, não confirmado (opção B não validada) |
| D2 | 3.2 e 4.1 | O manual não indicava o comando para mudar de `src\backend` para a pasta dos dados de demonstração (passo 3.2) nem para voltar a `src\backend` (passo 4.1). | Acrescentados `cd ..\..\04-artefactos-tecnicos\04.13-scripts-de-base-de-dados\m3-v02\dados-demonstracao` antes do 3.2 e `cd ..\..\..\..\src\backend` antes do 4.1. | `515c6b0` | Corrigido: cada `cd` leva à pasta certa; 28 verificações, 28 OK; `Successfully saved Jwt:Key` |

## 5. Conclusão

O manual foi seguido passo a passo, de raiz, num ambiente limpo, por um elemento do grupo diferente do autor, com a **opção A** (SQL Server 2025 local). Foram encontrados dois desvios (D1 e D2), corrigidos pelo autor no commit `515c6b0` e confirmados na 2.ª passagem sem novos desvios; a correção do D1 no passo 2.B.1 não foi confirmada, porque pertence à opção B. A **opção B** (SQL Server em Docker) **não foi validada**. Com esta ressalva, o critério de aceitação 4 da I038 está cumprido para a versão do manual do commit `515c6b0`.

Os registos completos das duas execuções ficam com o validador e no SharePoint do grupo; não são versionados, porque contêm o nome do computador e caminhos locais. Nenhuma palavra-passe, token ou chave foi gravada nesses registos.

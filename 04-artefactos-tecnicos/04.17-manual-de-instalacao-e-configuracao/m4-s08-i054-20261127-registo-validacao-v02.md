# I054 - Registo de preparação e validação do manual v02

Atualização v02 após o comentário preliminar do Revisor. O ensaio Windows abaixo foi comunicado pelo Revisor e não foi repetido pelo Executor nesta atualização. O registo v01 conserva-se sem alterações.

| Campo | Valor |
|---|---|
| Data / Versão | 27/11/2026 / v02 |
| Manual | `m4-manual-instalacao-configuracao-v02.pdf`; fonte `.md` com o mesmo nome |
| Código de referência | `f3feedd26de33694b23fb203d24b5d92c0fad68f` |
| Executor | Ivan Cristiano Silva Mendes |
| Revisor / Auditor | Gonçalo Dinis Magalhães Teixeira / Gonçalo Magalhães Ribeiro |
| Estado | Validação local de raiz comunicada pelo Revisor; D1 e D2 corrigidos documentalmente, sujeitos a confirmação. Relatório formal, ensaio público, auditoria e autorização de merge pendentes. |

## 1. Verificações da preparação anterior

As verificações seguintes pertencem à preparação registada na v01, anterior à passagem Windows do Revisor. Os limites desta tabela referem-se apenas a essa preparação.

Ambiente existente do Executor: macOS ARM64, .NET SDK 10.0.100, Node.js 24.22.3, npm 11.19.1, Temurin 21.0.13+9, ADB 37.0.3 e Android SDK Platform 37.0. Não se trata de um computador Windows limpo nem de uma instalação feita por outro elemento.

| Verificação | Resultado | Limite |
|---|---|---|
| Manifestos e ficheiros de configuração | Versões, portas, endereço da aplicação web, `apiBaseUrl`, APK, HTTP em modo de depuração e perfis confrontados com o código. | Confronto estático; não comprova instalação. |
| Web: `npm ci --offline --ignore-scripts --no-audit --no-fund` | 242 pacotes instalados a partir da cache. | Não verifica transferência inicial nem instalação das ferramentas numa máquina limpa. |
| Web: `npm run build` | Passou; 87 módulos transformados e ficheiros de distribuição criados. | Não confirma, por si só, o início de sessão, o CORS ou a interação com a API. |
| API: compilação Release sem restauro | Passou; zero avisos e zero erros. | Foram usados pacotes já disponíveis; o restauro inicial da solução completa fica por verificar pelo validador. |
| Ferramentas Android | JDK, ADB e SDK instalados identificados; wrapper declara Gradle 9.6.0 e AGP 9.4.1. | O wrapper não pôde escrever o ficheiro de bloqueio na cache global neste ambiente restrito. Compilação e instalação do APK não executados nesta preparação. |
| Ferramenta EF global | Foi identificada a versão 11.0.0 instalada neste computador; o manual requer 10.0.13. | A instalação global existente foi preservada. Migrações com a ferramenta 10.0.13 não executadas nesta preparação. |
| PDF | Gerado a partir da fonte v02; índice, texto e páginas revistos. | Não substitui a execução do procedimento. |
| Instalação completa em Windows | Não executada nesta preparação. | É a passagem independente necessária para o critério 4. |
| Ensaio público e percurso integrado web → API → aplicação | Não executados nesta preparação. | Devem ser registados separadamente dos resultados locais e estáticos. |

Comando de compilação da API usado nesta preparação:

```text
dotnet build src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
  --no-restore --disable-build-servers -m:1 -nr:false
  -p:UseSharedCompilation=false -c Release
```

As linhas acima representam um único comando. A compilação da solução completa e a sintaxe PowerShell 5.1 do procedimento de instalação não foram executadas nesta passagem macOS.

## 2. Desvios e correções documentais

| Ref. | Constatação | Tratamento | Estado |
|---|---|---|---|
| D01 | O Backlog v06 indica npm 12; a preparação usou npm 11.19.1 e o Revisor validou npm 11.19.0 com Node 24 em Windows. | Adotado npm 11.19.x no procedimento, conforme pedido do Revisor; retirada a indicação de decisão anterior à aprovação. A divergência face ao Backlog fica explícita. | Resolvido documentalmente; npm 12 não é apresentado como testado. Confirmar na revisão formal. |
| D02 | A descrição antiga da #78 refere Flutter; implementação e Backlog v06 usam Kotlin/Jetpack Compose. | O manual documenta as tecnologias utilizadas e as versões dos manifestos. | Corrigido no manual; descrição da Issue ainda carece de sincronização. |
| D03 | A v01 descrevia a lista CORS vazia como autorização de qualquer origem. | A v02 configura expressamente as origens locais e explica a recusa quando faltam. | Corrigido no texto; o Revisor comunicou acesso web e pré-visualização sem erro CORS no ensaio local. |
| D04 | A v01 fixava contagens de endpoints e testes de uma versão anterior. | A v02 usa resultados observáveis da versão instalada e identifica o commit. | Corrigido no texto. |
| D05 | O script de reposição apaga a base antes de a recriar. | A instalação v02 usa migrações sem apagar a base; a reposição destrutiva fica separada e identificada. | Corrigido no procedimento; o Revisor comunicou 3 migrações e ausência de alterações pendentes, sobre base nova. |
| D06 | `executar-dados.sh` recebe servidor/utilizador por variáveis, não por opções com esses nomes. | As instruções macOS/Linux indicam `SQLCMD_SERVIDOR` e `SQLCMD_UTILIZADOR`. | Corrigido antes de gerar a versão final do PDF. |
| D07 | Uma ferramenta EF global mais recente requer a opção explícita de mudança para uma versão anterior na CLI. | Acrescentado `--allow-downgrade` ao comando de ajuste para 10.0.13. | Corrigido no texto; comando não aplicado às ferramentas do utilizador. |

## 3. Validação local de raiz comunicada pelo Revisor

Fonte: comentário preliminar de revisão da I054 comunicado pelo Revisor e fornecido pelo Executor nesta atualização. O comentário termina com o compromisso de confirmar D1 e D2 e emitir o relatório; não constitui ainda esse relatório nem autorização de merge.

| Campo | Valor comunicado |
|---|---|
| Quem executou | Gonçalo Dinis Magalhães Teixeira, Revisor da I054 |
| Quando | 27/11/2026, 16:12–16:50, hora de Lisboa |
| Máquina / sistema | Windows 11, build 26200, x64; nome do computador não indicado no comentário |
| Consola | Windows PowerShell 5.1 |
| Condição de partida | Clone novo, base nova e user secrets próprios; ferramentas já instaladas. Não foi ensaiada a instalação inicial das ferramentas. |
| Código | `f3feedd26de33694b23fb203d24b5d92c0fad68f` |
| PDF seguido | `m4-manual-instalacao-configuracao-v02.pdf`, antes desta atualização |
| SHA-256 do PDF seguido | `f3eae4873ac0f0f34a5568d9ac452cf2cc9ea709630a38bab2a21cb0441d45b3`; coincide com o PDF existente antes das correções |
| SQL e base | SQL Server 2025 17.0, opção A (autenticação Windows); base `ClinicaVeterinaria_M4_Local` |
| Android | Emulador Medium Phone API 37.0; ligação funcional por túnel ADB e `localhost` |
| Credenciais | User secrets próprios. O comentário não identifica o canal de obtenção das palavras-passe de demonstração; os valores não são reproduzidos neste registo. |
| Âmbito concluído | Passos 1 a 16, com correção local do ficheiro `.env.local` sem BOM |
| Âmbito não executado | Passos 17 a 19: ensaio da API pública e regresso ao ambiente local |

### 3.1. Versões das ferramentas

| Ferramenta | Versão comunicada |
|---|---|
| .NET SDK | 10.0.404 |
| dotnet-ef | 10.0.13 |
| SQL Server | 2025, 17.0 |
| Node.js / npm | 24.20.0 / 11.19.0 |
| JDK | Eclipse Temurin 21.0.13+9 |
| ADB | 37.0.1 |
| Gradle | 9.6.0 |
| Dispositivo Android | Medium Phone API 37.0 |

As restantes versões e o identificador do computador não foram fornecidos no comentário. Não se substituem por valores da máquina do Executor.

### 3.2. Resultados dos passos

| Passos | Resultado comunicado | Limite ou desvio |
|---|---|---|
| 1–2 — clone e backend | Clone novo no commit indicado; build sem avisos. | Ferramentas previamente instaladas. |
| 3–5 — ligação e configuração | Base nova pela opção A; user secrets próprios. | O comentário confirma a conclusão, sem transcrição de segredos. |
| 6 — migrações | 3 migrações aplicadas e «No changes». | Resultado comunicado pelo Revisor. |
| 7 — dados | 28/28 verificações OK; segunda carga sem inserções. | O registo gerado dentro do repositório motivou M5. |
| 8–9 — API | API em 5159; 401 sem token; login com `password`, leitura de `/clinica` e logout concluídos. | M2: separar a apresentação do utilizador e da clínica para mostrar também o NIF. |
| 10–11 — web | Rececionista e Veterinária com acesso; build e preview em `127.0.0.1:5173` concluídos. | D1: `Set-Content -Encoding utf8` no PowerShell 5.1 gerou BOM e erro de configuração. Sem BOM, funcionou. |
| 12–15 — Android | APK compilado e instalado; Cliente João vê Bobi, Mia, boletim e consultas; Rececionista recusada. | `10.0.2.2` terminou com timeout após 10 s; túnel ADB com `localhost` funcionou. |
| 16 — percurso integrado | Vacinação registada pela Dra. Ana na web aparece no boletim do Bobi na aplicação; próxima data 27/12/2026. | Prova do percurso local web → API → aplicação no ensaio comunicado. |
| 17 — web pública | Não executado. | Comando corrigido documentalmente para gravar sem BOM; ligação pública não validada. |
| 18 — Android público | Não executado. | Não se declara funcionamento com a API pública. |
| 19 — regresso ao local | Não executado no percurso público. | Acrescentado comando explícito sem BOM e remissão para recompilar e reinstalar o APK. |

## 4. Tratamento do comentário preliminar

As referências D1 e D2 abaixo são as do Revisor; distinguem-se de D01 a D07, que pertencem à preparação anterior.

| Ref. | Pedido ou constatação | Alteração efetuada | Estado |
|---|---|---|---|
| D1 | `.env.local` com BOM em PowerShell 5.1, nos passos 10, 17 e 19. | Os três passos usam `Set-Content ... -Encoding ascii`. Acrescentadas explicação no passo 10 e linha em Problemas frequentes. | Correção documental preparada; confirmação do Revisor pendente. Não foi repetido o ensaio em PowerShell 5.1 nesta atualização. |
| D2 | Estado e secção 13 desatualizados; falta registo da passagem independente e fecho de D01. | Estado e secção 13 atualizados; emitido este registo v02 com ambiente, versões, resultados e limites. Adotado npm 11.19.x e retirada a decisão pendente de D01. | Atualização preparada; relatório formal de revisão ainda por emitir e associar ao PR #294. |
| M1 | Timeout em `10.0.2.2`; reinstalação após recompilar pouco explícita. | Túnel ADB com `localhost` passa a opção principal para emulador e telemóvel por USB. Alternativa `10.0.2.2` e timeout observado explicados. Passos 13 e 19 remetem para instalar o novo APK no passo 14. | Incorporada no manual. |
| M2 | Saídas de `Select-Object` misturadas; NIF não mostrado. | Acrescentado `Format-List` às duas saídas do passo 9; resultado esperado inclui o NIF. | Incorporada; confirmação visual pelo Revisor pendente. |
| M3 | Aviso «9 moderate severity vulnerabilities» durante `npm ci`. | Aviso observado registado; instrução para não executar `npm audit fix` no procedimento de instalação. | Incorporada; não foram alteradas dependências. |
| M4 | Aviso de strip de `libandroidx.graphics.path.so`. | Explicado que o aviso não impede a instalação quando o build termina com `BUILD SUCCESSFUL` e gera o APK. | Incorporada; não se generaliza a falhas de build. |
| M5 | `dados-execucao-*.log` aparece no `git status`. | Comandos do passo 7 passam `-PastaRegistos $env:TEMP`; indicado guardar o log nas evidências e não o versionar. | Incorporada sem alterar o `.gitignore` ou apagar registos existentes. |
| M6 | Atualizar o commit de referência na entrega final da M4. | Mantido o commit efetivamente validado nesta revisão; acrescentada a necessidade de atualizar o commit e as verificações na entrega final. | Seguimento para a entrega final; não se atribui esta validação a código posterior. |
| M7 | Nome do ramo contém acentos. | Nenhuma alteração ao ramo. | Sem ação neste PR, conforme indicação do Revisor. |

## 5. Verificação desta atualização e conclusão

Verificações do Executor sobre as correções: conferência das três gravações de `.env.local` com ASCII e leitura dos três conteúdos sem BOM pelo Vite instalado no projeto, em ficheiros temporários; presença de `Format-List` nas duas saídas; revisão das referências de versão, do túnel ADB e dos passos de reinstalação; regeneração e inspeção do PDF. A verificação do Vite não executou os comandos PowerShell nem fez pedidos à API pública. Não foi repetida a instalação Windows nem foram executados os passos da API pública nesta atualização.

A validação local de raiz foi comunicada pelo Revisor, que concluiu o percurso integrado depois de resolver o BOM no seu ambiente. O critério 4 deixa de estar sem qualquer passagem independente, mas o seu fecho continua dependente da confirmação de D1 e D2 pelo Revisor e do relatório formal. A instalação de ferramentas numa máquina sem ferramentas e o ensaio público não estão comprovados por esta evidência.

O relatório de revisão será emitido pelo Revisor após a confirmação das correções e deverá ficar associado ao PR #294 e a este registo. Enquanto não for recebido, não se indica nome de ficheiro, assinatura ou classificação como se já existissem. A auditoria e a autorização de merge pertencem a Gonçalo Magalhães Ribeiro, não ao Executor.

PDF regenerado após estas correções: `m4-manual-instalacao-configuracao-v02.pdf`, SHA-256 `af1e1fcd6a720cb408cd0645dac035dc2d4892cdd40336720e2d1f4dd5200a30`. Este hash identifica a atualização a confirmar pelo Revisor; o hash da secção 3 identifica o PDF anterior efetivamente seguido no ensaio.

### Histórico do registo

| Versão | Data | Conteúdo |
|---|---|---|
| v01 | 27/11/2026 | Preparação técnica do Executor; passagem independente ainda não comunicada. Conservada como histórico. |
| v02 | 27/11/2026 | Incorpora o comentário preliminar do Revisor, a validação local de raiz e o tratamento de D1, D2 e M1 a M7; mantém explícitas as confirmações pendentes. |

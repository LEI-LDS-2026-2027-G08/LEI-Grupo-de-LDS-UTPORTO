# I054 - Registo de preparação e validação do manual v02

| Campo | Valor |
|---|---|
| Data / Versão | 27/11/2026 / v01 |
| Manual | `m4-manual-instalacao-configuracao-v02.pdf`; fonte `.md` com o mesmo nome |
| Código de referência | `f3feedd26de33694b23fb203d24b5d92c0fad68f` |
| Executor | Ivan Cristiano Silva Mendes |
| Revisor / Auditor | Gonçalo Dinis Magalhães Teixeira / Gonçalo Magalhães Ribeiro |
| Estado | Preparação técnica efetuada. Validação independente de raiz ainda não executada. |

## 1. Verificações desta preparação

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
| D01 | O Backlog v06 indica npm 12; o README web e o ambiente observado usam npm 11. | Manual identifica 11.19.1 como a versão efetivamente utilizada e explicita a diferença. Revisor deve confirmar a versão a adotar na entrega. | Pendente de decisão na revisão; npm 12 não apresentado como testado. |
| D02 | A descrição antiga da #78 refere Flutter; implementação e Backlog v06 usam Kotlin/Jetpack Compose. | O manual documenta as tecnologias utilizadas e as versões dos manifestos. | Corrigido no manual; descrição da Issue ainda carece de sincronização. |
| D03 | A v01 descrevia a lista CORS vazia como autorização de qualquer origem. | A v02 configura expressamente as origens locais e explica a recusa quando faltam. | Corrigido no texto; confirmar no ensaio real. |
| D04 | A v01 fixava contagens de endpoints e testes de uma versão anterior. | A v02 usa resultados observáveis da versão instalada e identifica o commit. | Corrigido no texto. |
| D05 | O script de reposição apaga a base antes de a recriar. | A instalação v02 usa migrações sem apagar a base; a reposição destrutiva fica separada e identificada. | Corrigido no procedimento; migrações a confirmar na passagem independente. |
| D06 | `executar-dados.sh` recebe servidor/utilizador por variáveis, não por opções com esses nomes. | As instruções macOS/Linux indicam `SQLCMD_SERVIDOR` e `SQLCMD_UTILIZADOR`. | Corrigido antes de gerar a versão final do PDF. |
| D07 | Uma ferramenta EF global mais recente requer a opção explícita de mudança para uma versão anterior na CLI. | Acrescentado `--allow-downgrade` ao comando de ajuste para 10.0.13. | Corrigido no texto; comando não aplicado às ferramentas do utilizador. |

## 3. Passagem independente por realizar

O Revisor deve seguir a v02 numa máquina ou ambiente limpo e registar numa nova versão deste registo:

1. Nome de quem executou, data e intervalo de execução, sistema operativo, arquitetura e versões das ferramentas.
2. Commit do código e SHA-256 do PDF seguido; origem do acesso às credenciais, sem copiar os valores.
3. Resultado dos passos 1 a 19 aplicáveis; opção SQL escolhida e tipo de dispositivo Android. Assinalar expressamente as opções não executadas.
4. Percurso integrado do passo 16, incluindo a vacinação criada na aplicação web e consultada na aplicação através da mesma API local.
5. Cada desvio: passo, resultado esperado, resultado obtido, correção, versão do manual e nova confirmação.
6. Resultado final: instalação completa concluída ou ainda incompleta, com indicação das limitações.

Enquanto essa passagem não ocorrer e os desvios necessários não estiverem resolvidos, o critério de aceitação 4 da I054 permanece **pendente**. O registo da I038, de 04/11/2026, conserva-se como evidência histórica da v01 e não é reutilizado como validação desta v02.

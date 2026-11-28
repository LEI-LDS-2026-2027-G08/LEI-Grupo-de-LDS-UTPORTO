# Relatório de análise estática do frontend web

| Campo | Valor |
|---|---|
| Grupo e sistema | G08 - Gestão de Clínica Veterinária |
| Issue e entrega | I046, GitHub #71; M4 / Sprint 08; artefacto OF-M4-016 |
| Data e versão | 28/11/2026 / v01; sessão de análise iniciada em 27/11 e concluída após a meia-noite |
| Executor | Ivan Cristiano Silva Mendes |
| Revisor e Auditor | André Filipe Moreira Mesquita / Gonçalo Magalhães Ribeiro, conforme Sprint Planning S08 |
| Base de código | `340da71cb862b14c7d7073160c9f307ef7451eda`; branch da I046, com as alterações desta tarefa ainda sem commit |
| Ambiente | macOS ARM64; Node.js 24.22.3; npm 11.19.1 |
| Estado | Gates locais aprovados e bloqueios demonstrados. Workflow preparado; execução remota, revisão e auditoria por confirmar. |

## 1. Conclusão e âmbito

O frontend web cumpre localmente QG-01, QG-04 e QG-08 no conteúdo identificado pelas evidências desta execução. O ESLint analisou 149 ficheiros, com zero erros e zero avisos; TypeScript, Prettier e compilação terminaram com código zero. Três ensaios temporários demonstraram o bloqueio de execução dinâmica, de erro de tipos e de formatação incorreta. Os ficheiros usados nesses ensaios foram removidos.

O âmbito inclui a aplicação React/TypeScript, os testes e objetos de página Playwright, as configurações Vite/Playwright e o executor da análise. Não abrange o backend, o Android, a execução funcional dos testes de sistema, vulnerabilidades de dependências ou uma auditoria de segurança exaustiva. F005 e F007 continuam fora do âmbito aprovado, como no Plano de Testes I043; a referência histórica à I119 na issue não reintroduz internamentos.

### Ferramentas utilizadas

| Ferramenta | Versão instalada | Função |
|---|---|---|
| ESLint e typescript-eslint | 10.13.0 / 8.72.0 | Regras recomendadas JavaScript/TypeScript, React Hooks, React Refresh e regras contra execução dinâmica |
| TypeScript | 6.0.3 | Tipos em modo estrito na aplicação, Vite e Playwright |
| Prettier | 3.9.10 | Ferramenta adicional de qualidade: consistência de formatação |
| Vite | 8.3.4 | Compilação de distribuição; complemento de QG-01 |

O Prettier é a ferramenta adicional justificada pela I046: cobre a consistência textual que não é garantida pelos tipos ou pelo lint e corresponde ao QG-08 do Plano de Testes. Não mede segurança. Não foi necessário instalar SonarQube nem alterar dependências; não se atribuem a estas ferramentas métricas ou classificações SonarQube.

## 2. Configuração e quality gates

O comando versionado `npm run analise:estatica`, na pasta `src/frontend`, chama `scripts/analisar.mjs`. Todas as etapas são executadas e os resultados ficam em `resultados-analise-estatica/`. Se alguma falhar, o resultado global termina com código 1. A pasta de saída local é ignorada pelo Git; as evidências da execução entregue são arquivadas separadamente.

| Gate | Verificação executada | Limite de aceitação |
|---|---|---|
| QG-04 | ESLint com `--max-warnings 0 --format json` | Zero erros e zero avisos |
| QG-01 | `tsc -b --force` | Zero erros de tipos; `strict: true` nos três projetos |
| QG-08 | `prettier --check .` | Zero ficheiros por formatar |
| QG-01 | `vite build` | Compilação concluída, código zero |

O ESLint mantém as regras recomendadas existentes para TypeScript, React Hooks e React Refresh, acrescenta as regras recomendadas JavaScript aos executores/configurações e define `no-eval`, `no-implied-eval`, `no-new-func` e `no-script-url` como erros. Para classificação de ocorrências, execução dinâmica é considerada alta; erros de compilação ou tipos também bloqueiam. Problemas de manutenção e estilo são médios ou baixos conforme o impacto, mas nenhum erro ou aviso é aceite no gate final. A severidade 2 do ESLint significa erro; não equivale automaticamente a uma vulnerabilidade de severidade alta.

A configuração TypeScript da aplicação já era estrita. A I046 acrescenta `strict: true` à configuração Vite e um projeto próprio para Playwright, incluído em `tsconfig.json`. Os testes passam a ser verificados pelo compilador, além de serem transpilados pelo Playwright. As opções de verificação de variáveis não usadas e de controlo de fluxo mantêm-se.

### Âmbitos e exceções justificadas

`dist` e saídas geradas ficam fora do lint; dependências e resultados locais não são código de aplicação a rever. O Prettier respeita as exclusões locais versionadas. `skipLibCheck` mantém a opção existente de não verificar declarações de bibliotecas externas; não exclui o código do projeto da verificação.

Em `tsconfig.playwright.json`, `erasableSyntaxOnly: false` permite propriedades nos parâmetros dos construtores dos objetos de página, sintaxe suportada pelo transpilador Playwright. A resolução `bundler` corresponde a esse executor. `strict: true` mantém-se; a restrição de sintaxe apagável continua ativa na aplicação e na configuração Vite. Esta é uma configuração de âmbito justificada, não uma supressão de erros de tipos. Não foram introduzidos `eslint-disable`, `@ts-ignore` ou exceções por ficheiro para fazer passar a análise.

## 3. Resultados antes e depois

Na base 340da71, lint, build e formatação já passavam. A I046 alargou a cobertura e automatização e demonstrou os bloqueios; não foram necessários acertos no código de produção.

| Verificação | Antes | Depois |
|---|---|---|
| ESLint | Código 0; sem erros nem avisos no âmbito existente | 149 ficheiros; 0 erros e 0 avisos, incluindo executores JavaScript e novas regras |
| TypeScript e build | Compilação aprovada; aplicação estrita; Playwright fora dos projetos tsc | Três projetos estritos; tipos e compilação aprovados |
| Prettier | Sem ficheiros por formatar | Sem ficheiros por formatar, incluindo os novos ficheiros do frontend |
| Execução unificada | Comandos separados; sem resumo próprio da I046 | Um comando local/CI, resultados por ferramenta e resumo com hashes |
| Automatização remota | Sem workflow de análise web nesta base | Workflow de análise estática preparado; execução no GitHub ainda não observada |

### Ações e classificação

| Ref. | Constatação | Ação e resultado |
|---|---|---|
| A1 | Testes Playwright fora do compilador; Vite sem `strict` explícito | Projeto Playwright próprio e Vite estritos; zero erros. Sem alterações de regras de negócio. |
| A2 | Regras de execução dinâmica e JavaScript não explícitas | Regras bloqueantes versionadas; sem ocorrências reais finais. Ensaio `no-eval` recusado. |
| A3 | Falta de um executor de análise e de evidências agregadas | Acrescentados comando, manifesto SHA-256 e workflow. O workflow não altera proteção de ramos nem publica a aplicação. |

Resultado final: zero ocorrências altas e zero falsos positivos, sem supressões de regras. As falhas temporárias abaixo são ensaios dos gates e não fazem parte da entrega.

### Ensaios de bloqueio do Executor

Foi criado, de cada vez, `src/i046-verificacao-temporaria.ts`; o comando completo correu e o ficheiro foi removido no fim. Em cada caso, o código global foi 1 e a ferramenta pretendida recusou o problema.

| Ensaio | Problema introduzido | Resultado |
|---|---|---|
| Alta, regra `no-eval` | `export const resultadoI046 = eval("1 + 1");` | ESLint: erro de severidade 2; QG-04 falhou |
| Tipos | Número declarado com valor string | TypeScript: incompatibilidade de tipos; QG-01 falhou |
| Formatação | Declaração sem espaçamento e terminador esperados | Prettier: ficheiro por formatar; QG-08 falhou |

Após remoção dos ensaios, a execução final das 00:03:30 às 00:03:37 de 28/11/2026 (Lisboa) passou em todos os gates. A descoberta Playwright manteve 69 testes em 11 ficheiros; não foram reexecutados os testes funcionais de sistema nesta tarefa. O Revisor deve repetir a análise e o ensaio de severidade alta de forma independente.

## 4. Reprodutibilidade, pipeline e evidências

### Execução local e no CI

Depois de obter a versão entregue e instalar as dependências com `npm ci`, executar em `src/frontend`:

```text
npm run analise:estatica
```

O workflow `.github/workflows/frontend-estatica.yml` executa esse mesmo comando em push e pull request com alterações no frontend ou no próprio workflow, e admite execução manual. Usa Node 24, permissões de leitura e publica os resultados mesmo se um gate falhar. As ações externas estão fixadas por SHA. A sintaxe YAML foi processada pelo Prettier; não foi executado um runner GitHub nesta sessão.

A I142 integra os gates na pipeline completa `frontend-ci.yml` e confirma a proteção dos ramos. Um job em falha não comprova, por si só, que o GitHub impede o merge: é necessária a verificação obrigatória na proteção do ramo. A execução remota e a respetiva evidência terão de ser confirmadas após o push; o critério 1 está configurado, mas a sua execução remota ainda não está demonstrada.

### Identificação do conteúdo analisado

Commit base: `340da71cb862b14c7d7073160c9f307ef7451eda`. As alterações da I046 ainda não tinham commit na execução registada. O identificador exato do conteúdo analisado é o manifesto `depois/SHA256-ficheiros.txt`, cujo SHA-256 é `3f955c9b323332e58d7f7f6ed0bbb459a2cee5379108b72bdfb58c0b5ad9a5b5`. Inclui aplicação, testes, executores, ficheiros de configuração e workflow. O relatório e os resultados, gerados depois da análise, não entram nesse manifesto para evitar autorreferência.

Depois do commit, o Revisor deve identificar o SHA entregue e comparar o conteúdo com o manifesto, sem apresentar o commit base como se já incluísse as alterações. Não foram criados commits, PRs ou publicações nesta sessão.

### Evidências arquivadas

Pasta: `m4-s08-i046-20261128-evidencias-v01`, junto deste relatório.

| Conteúdo | Evidência |
|---|---|
| `antes-eslint.txt`, `antes-build.txt`, `antes-prettier.txt` | Resultados da medição inicial |
| `depois/` e `depois-comando-final.txt` | ESLint JSON, tipos, formatação, build, resumo e manifesto da execução final |
| `bloqueio-eslint-alta/`, `bloqueio-typescript/`, `bloqueio-prettier/` | Resultados e códigos das três recusas controladas |
| `ensaios-temporarios.json` | Conteúdos usados nos ensaios e confirmação de remoção |
| `playwright-lista.txt` | Descoberta de 69 testes, sem execução funcional |
| `SHA256-evidencias.txt` | Integridade dos ficheiros de evidência arquivados |

### Conclusão de entrega

QG-01, QG-04 e QG-08 aprovados localmente, sem ocorrências altas por resolver e com bloqueio comprovado nos três ensaios. Configuração, comando e workflow estão preparados para revisão. O relatório cumpre o registo de antes/depois, ferramentas, âmbito, exceções e ações; não certifica a execução remota. Permanecem a confirmação da pipeline após push, a revisão independente de André Mesquita, a auditoria de Gonçalo Ribeiro e a correspondência à versão final da M4.

Referências: issue I046 #71; Plano de Testes do Frontend Web I043, secções 4, 5 e 8; Regulamento da UC v19, secção 19 (03.02.06); Sprint Planning S08 de 25/11/2026. A responsabilidade pela pipeline integrada pertence à I142.

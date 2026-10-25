# Registo de não conformidades e ações corretivas de M1

Data de identificação neste registo: 14/10/2026. Âmbito: S01 e documentação M1 disponível. Preparação assistida por Codex. As ocorrências históricas mantêm a sua data original; esta é a data do registo atual.

Não foram nomeados responsáveis formais nem atribuídos pontos por estas ações. A designação de responsáveis, revisão independente, aprovação e encerramento cabem ao grupo segundo o procedimento aplicável.

| ID | Facto e localização | Ação preparada ou necessária | Estado |
|---|---|---|---|
| NC-M1-01 | MPI v01: valores guardados `#VALUE!` em Validação B27/C27; a comparação incluía linhas sem registos e fórmulas que devolviam texto vazio. | MPI v02: comparação das 31 linhas reais e teste positivo de deteção de excesso. | Correção técnica verificada; aprovação pendente. |
| NC-M1-02 | Carga v01: E30 aponta para um XLSX inexistente no campo PDF; H30 aponta para ata não localizada com aquele nome. | Carga v02: caminhos para o PDF do Backlog S01 e ata de Planning v02 existentes. Confirmar a versão originalmente apreciada. | Caminhos corrigidos; identificação histórica pendente. |
| NC-M1-03 | Rotação v01: B29 vazio, apesar de D6 indicar S01 Concluído. | Rotação v02: reproduzido em B29 o estado já registado em D6. | Correção técnica verificada; aprovação pendente. |
| NC-M1-04 | Sprint Backlog S01, K9:K18: prazos de outubro, posteriores à Review de 25/09/2026; há observações que referem simulação. | Identificar o significado dos dados e eventual autorização; conservar o original. Recuperar os prazos/entregas reais ou declarar que não são demonstráveis. | Aberto; dados reais ou enquadramento não disponíveis na documentação examinada. |
| NC-M1-05 | Rotação, linhas 54, 55, 59, 60 e 61: Ivan consta como Revisor e Auditor de I001, I002, I006, I007 e I008. | Preservar as atribuições e registar a apreciação formal do desvio e os seus efeitos. Garantir independência nas novas tarefas. | Desvio documentado; regularização pendente. |
| NC-M1-06 | Não foram localizados registos individuais de revisão/auditoria nas pastas funcionais, nem prova de submissão/ZIP original na pasta indicada. | Manter declarada a lacuna. Eventuais verificações novas devem ser datadas de agora e realizadas pelos responsáveis efetivos; não sustentam automaticamente Q=100% nem entrega atempada. | Lacuna documentada; não encerrada. |
| NC-M1-07 | Carga planeada S01: 81, 40, 69 e 130 pontos; QCTP 3,25 face ao limite 1,10 do modelo/plano. | Preservar o planeamento original, registar o desvio e distingui-lo do equilíbrio cumulativo posterior. | Desvio documentado; apreciação pendente. |
| NC-M1-08 | Não foram encontrados PDF autónomos históricos de carga/rotação. | Produzidas cópias de consulta atuais, identificadas como não assinadas. Não substituem comprovativos ou assinaturas da entrega original. | Consulta preparada; fecho formal pendente. |

## Prazos atualmente inscritos no Sprint Backlog S01

Os valores abaixo são transcritos para tornar o problema verificável. Não são aceites neste registo como os prazos reais do trabalho de setembro.

| Issue | Célula | Prazo registado |
|---|---|---|
| I001 | K9 | 20/10/2026 18:00 |
| I002 | K10 | 19/10/2026 18:00 |
| I003 | K11 | 20/10/2026 18:00 |
| I004 | K12 | 21/10/2026 18:00 |
| I005 | K13 | 22/10/2026 18:00 |
| I006 | K14 | 23/10/2026 18:00 |
| I007 | K15 | 24/10/2026 18:00 |
| I008 | K16 | 25/10/2026 18:00 |
| I009 | K17 | 26/10/2026 18:00 |
| I010 | K18 | 27/10/2026 18:00 |

## Critério de encerramento

Cada ocorrência só deve ser encerrada depois de identificados o responsável real, a evidência, a versão resultante, a verificação independente aplicável e a decisão de aprovação. Não preencher nomes, votos, datas de execução ou classificações por inferência. Os desvios que não possam ser reconstituídos devem permanecer declarados e ser tratados pela via prevista na UC.

## Fontes

- [m1-s01-sprint-backlog-v01.xlsx](../../01-artefactos-de-gestao/01.04-backlog-de-projeto/m1-s01-sprint-backlog-v01.xlsx)
- [m1-s01-20260925-sprint-review-ata-v01.pdf](../../01-artefactos-de-gestao/01.03-reunioes-de-grupo/01.03.04-reunioes-de-sprint-review/01.03.04.02-atas/m1-s01-20260925-sprint-review-ata-v01.pdf)
- [m1-grelha-rotacao-papeis-v01.xlsx](../../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m1-grelha-rotacao-papeis-v01.xlsx)
- [m1-mapa-carga-planeada-v01.xlsx](../../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m1-mapa-carga-planeada-v01.xlsx)
- [m1-s01-mapa-pontuacao-individual-v01.xlsx](../../01-artefactos-de-gestao/01.06-avaliacao-interna-do-grupo/m1-s01-mapa-pontuacao-individual-v01.xlsx)
- [Verificação técnica das correções](../02.04-revisoes/m1-s01-20261014-verificacao-documental-v01.md)

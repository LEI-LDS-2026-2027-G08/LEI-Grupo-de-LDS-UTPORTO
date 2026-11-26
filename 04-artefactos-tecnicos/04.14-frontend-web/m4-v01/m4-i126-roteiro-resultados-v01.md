# I126 — Validação do frontend web: vacinação, desparasitação e permissões

**Data:** 26/11/2026 · **Executor:** Ivan · **Versão:** v01  
**Estado:** validação parcial; correção preparada para revisão. A I126 não está concluída.  
**Base:** `f3feedd` — I125 integrada. Alterações locais: `VacinacaoPage.tsx` e respetivo teste.

## 1. Âmbito e pontos por decidir

O registo, a consulta do boletim, o cálculo inicial da próxima administração e as permissões foram verificados. Pagamentos e internamentos estão fora do âmbito nos Requisitos v03 e nos Casos de Uso v03.

A Issue [#114](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/114) ainda tem o texto antigo. O Backlog v06 consultado na `main` já limita a I126 à vacinação/desparasitação e permissões, mas inclui **atualização de administrações e recálculo**. A I118 exclui o RF056 até especificação e aprovação; o frontend não tem essa ação. É necessário resolver a divergência [#291](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/291). A existência da operação no backend não comprova a aprovação do fluxo web.

## 2. Ambiente

- Frontend e backend reais, executados localmente, com SQL Server e migrações do projeto.
- Base isolada `ClinicaVeterinaria_I126_939927b4`, com o script de demonstração `m3-v02`. A base local existente foi preservada.
- Navegador integrado, paciente Luna e contas fictícias de demonstração. Palavras-passe temporárias apenas na configuração local.
- Execução inicial: frontend `http://localhost:5174`, API `http://127.0.0.1:5161`.
- Confirmação após a correção: frontend `http://localhost:5175`, API `http://127.0.0.1:5162`. Foi iniciado um servidor novo para evitar a versão anterior em cache.

## 3. Roteiro e resultados

| Caso | Passos e resultado esperado | Resultado observado |
|---|---|---|
| V01 — Boletim vazio | Veterinário: Pacientes → ficha de Luna → Vacinação. | Passou. Mensagens de boletim vazio e sem próximas administrações. |
| V02 — Vacinação | Registar `Vacina I126`, lote `I126-V01`, em 26/11/2026, com intervalo de 365 dias. | Passou. Registo no boletim; próxima administração em 26/11/2027, confirmada também na API. |
| V03 — Desparasitação | Registar `Desparasitante I126`, lote `I126-D01`, sem intervalo. | Passou. Registo no boletim; intervalo e próxima data nulos na API. |
| V04 — Persistência | Recarregar a página e consultar os registos. | Passou. Os registos mantêm os dados introduzidos. |
| V05 — Filtros | Selecionar só vacinas, só desparasitações e ambos os tipos. | Passou. Histórico e próximas administrações respeitam o filtro. |
| V06 — Cancelar | Preencher um registo com lote `I126-CANCELAR`, cancelar e recarregar. | Passou. O lote não aparece no boletim. |
| V07 — Validação local | Submeter produto e lote vazios e intervalo zero. | Passou. Mensagens dos campos obrigatórios e do intervalo; formulário mantém-se aberto. |
| V08 — Permissões web | Abrir diretamente o boletim como Administrador, Rececionista e Auxiliares com e sem apoio. | Passou nos quatro casos. Página «Acesso negado». |
| V09 — Erros da API | Pedir o boletim de um paciente inexistente. | Passou. Mensagem «O animal indicado não existe.» e opção «Tentar novamente». |
| V10 — Registos semelhantes | Registar duas vacinas com produto, data e intervalo iguais, mas lotes diferentes; alternar filtros. | Defeito #290 reproduzido e corrigido. Na execução nova ficaram duas linhas distintas, sem aviso de chaves duplicadas. |
| V11 — Recusas HTTP | Verificar sem sessão; perfis sem permissão; data futura, tipo, campos e intervalos inválidos. | Passaram as 23 verificações auxiliares da API. Respostas 401/403/400/404 conforme o caso; nenhum registo adicional criado pelas recusas. |
| V12 — Atualização | Alterar uma administração e confirmar o recálculo no web. | Não executado. A ação não existe no frontend; depende da resolução da #291. |

No ensaio de data futura pelo navegador, o campo acabou submetido com 26/11/2026. Esse registo, com lote `I126-FUTURO`, foi aceite como data atual e **não comprova uma recusa de data futura no ecrã**. A recusa foi confirmada diretamente na API com 27/11/2026. O teste de componente existente cobre a apresentação do erro e a preservação dos dados, com resposta simulada.

## 4. Defeitos e decisões

| Issue | Origem | Decisão e estado |
|---|---|---|
| [#290 — chaves duplicadas](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/290) | V10, consola do navegador. Dois registos válidos recebiam a mesma chave de apresentação. | Acrescentada a ocorrência à chave da linha, sem mudar API ou dados. Teste de regressão e confirmação no navegador passaram. Revisão e integração pendentes. Sem perda de dados observada. |
| [#291 — critérios divergentes](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/291) | Comparação da #114, Backlog v06 e exclusão do RF056 na I118. | Pendente de decisão e alinhamento documental. Impede considerar todos os critérios cumpridos. |

Não foi identificado outro defeito bloqueador nos fluxos executados. Esta observação não abrange o fluxo de atualização não executado.

## 5. Verificação técnica e evidências

| Verificação | Resultado |
|---|---|
| Vitest: vacinação e `VacinacoesService` | 23 testes passaram, incluindo a regressão da #290. |
| Backend: filtro `FullyQualifiedName~Vacinacao` | 83 testes unitários passaram; TRX no pacote de evidências. |
| Compilação do frontend e da API | Passou; API com zero erros e zero avisos. |
| ESLint e Prettier dos ficheiros alterados | Passaram. |
| Verificações HTTP auxiliares | 23 passaram; resultados sem tokens nem palavras-passe. |

Pacote: [m4-i126-evidencias-v01.zip](m4-i126-evidencias-v01.zip), com 14 capturas, resultados HTTP, registo da consola antes da correção, confirmação posterior e identificação dos ficheiros de código por SHA-256.

A execução automática do navegador pelo Playwright foi bloqueada pelas permissões do sistema. Os percursos visuais foram executados pelo navegador integrado, contra a API real. Não foi executada a suite final de testes de sistema nem um ensaio no ambiente público. A recuperação de falha de rede só tem cobertura nos testes de componente existentes; não foi repetida ponta a ponta nesta execução.

## 6. Para fechar a I126

1. Resolver a #291 e alinhar a #114 com a decisão aplicável, incluindo o tratamento da atualização e do recálculo.
2. Integrar e rever a correção da #290; repetir os casos afetados na versão final.
3. O Revisor executa os fluxos com contas próprias e confirma os critérios. Seguem-se relatório de revisão, auditoria e aceitação.

Este roteiro não substitui a revisão independente, a auditoria ou a decisão de aceitação.

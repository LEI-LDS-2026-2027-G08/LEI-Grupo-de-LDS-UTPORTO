# I127 — Ocorrências e decisões — v01

Data de consolidação: 23/11/2026. Referência: [I127 — #115](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/115).

## Defeitos funcionais

Não foi identificado, nesta consolidação, um novo defeito funcional reproduzido na aplicação que exija alteração de código. Os testes manuais foram confirmados pelo Executor; as verificações automáticas são registadas separadamente. Não se criou uma Issue de defeito artificial para preencher este registo.

Se surgir um defeito na reprodução, registar uma Issue com o ambiente, passos, resultado esperado/obtido, origem confirmada ou por determinar e correção ou decisão. Não usar apenas «I127» como título de vários defeitos.

## Ocorrências de configuração, dados e âmbito

| ID | Ocorrência | Origem | Decisão e referência |
|---|---|---|---|
| O01 | Emulador sem ligação à API por `10.0.2.2` | Configuração da ligação local; a origem funcional não foi atribuída à aplicação | Alternativa já documentada: `adb reverse tcp:5159 tcp:5159` e compilação com `-PapiBaseUrl=http://localhost:5159`. Confirmar a API local ligada. Não é uma nova alteração do contrato. |
| O02 | João sem próximas consultas no Azure | Dados desse ambiente; não foi demonstrada uma marcação futura existente | Aceitar a apresentação da lista vazia. Para repetir o cenário com dados, criar uma marcação futura nesse ambiente pela Receção. A captura local comprova a apresentação de uma marcação futura. |
| O03 | A descrição antiga da I127 ainda menciona despesas e I124 | Divergência do texto da Issue com o âmbito documental atual | F005/RF042/M05 e I124 estão fora do âmbito no [Plano de Testes Mobile v01, secção 3.2](../../../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.01-plano-de-testes/m4-plano-testes-frontend-mobile-v01.md). Não implementar despesas, pagamentos ou internamentos. Alinhar o texto da Issue com a decisão já referida nos documentos do grupo. |
| O04 | Timeout e uma resposta `503` no contacto inicial com o Azure; recuperação posterior | Indisponibilidade transitória observada no acesso ao ambiente público; causa por determinar | Sem intervenção no serviço, Swagger `200` e 31 verificações seguintes passaram. Não foi reproduzido um defeito persistente. Se se repetir, registar uma Issue de disponibilidade com data, rotas e resultados. Ver o relatório automático. |

Nenhuma destas ocorrências, por si só, demonstra um defeito funcional aberto do frontend ou do backend. A decisão sobre uma divergência documental não constitui uma nova aprovação de âmbito.

## Limites da evidência

- Maria no Android público e o Histórico dos dois Clientes nos dois ambientes: confirmação expressa do Executor, sem capturas específicas nesta pasta.
- Dispositivo Android físico: não comprovado.
- Não se declara uma nova execução do Android nesta consolidação; não havia emulador nem dispositivo ligado ao iniciar a verificação.
- Não foram alterados dados do backend público nem criadas marcações durante esta consolidação.

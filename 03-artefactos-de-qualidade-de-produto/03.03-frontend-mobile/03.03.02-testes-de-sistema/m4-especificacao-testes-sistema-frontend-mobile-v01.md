# Especificação dos Testes de Sistema do Frontend Mobile — Gestão de Clínica Veterinária

| Campo                | Valor                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Grupo                | G08                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| Projeto              | Gestão de Clínica Veterinária                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| Artefacto            | Especificação dos Testes de Sistema do Frontend Mobile (`OF-M4-018`)                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| Ficheiro             | `m4-especificacao-testes-sistema-frontend-mobile-v01.md`                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| Versão               | v01                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| Data                 | 2026-11-20                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| Milestone de Entrega | m4                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| Issue                | I136 — Especificar os testes de sistema do frontend mobile (GitHub #124, Sprint 07)                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| Papéis               | Executor: Gonçalo Magalhães Ribeiro · Revisor: Gonçalo Dinis Magalhães Teixeira · Auditor: Ivan Cristiano Silva Mendes                                                                                                                                                                                                                                                                                                                                                                                                               |
| Pasta de destino     | `/projeto/03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.02-testes-de-sistema`                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| Fontes               | `m4-plano-testes-frontend-mobile-v01.md` (I047); `m3-especificacao-requisitos-v02.md` (I012); `m3-especificacoes-casos-uso-v02.md` (I015); `m3-documentacao-api-v03.md`; `m3-modelo-classes-frontend-mobile-v03.md` (I022); `m3-prototipo-baixa-fidelidade-v02.md` (I017); `m3-declaracao-ambito-v05.pdf`; scripts de dados de demonstração `04.13-scripts-de-base-de-dados/m3-v02` (I061); `m4-backlog-projeto-v05.xlsx`; código da aplicação Android: I121 e I122 em `develop` (`6ea37e2`) e I123 no Pull Request #274 (`7ee3a48`) |

## Histórico de versões

| Versão | Data       | Autor                     | Alterações                                                                                                                                                                                                       |
| ------ | ---------- | ------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| v01    | 2026-11-20 | Gonçalo Magalhães Ribeiro | Versão inicial: 30 cenários de sistema para os 9 requisitos funcionais do Cliente na aplicação móvel e para os requisitos não funcionais com verificação na aplicação (RNF001, RNF002, RNF003, RNF005 e RNF006). |

## Índice

1. Introdução
2. Documentos de referência
3. Âmbito
4. Convenções
5. Ambiente, dados e regras de execução
6. Estado das funcionalidades na data desta versão
7. Cenários — F001 Autenticação e sessão
8. Cenários — F002 Os meus animais
9. Cenários — F003 Consultas futuras e histórico administrativo
10. Cenários — F006 Boletim de vacinação e desparasitação
11. Cenários transversais — perfis, isolamento, rede e erros
12. Matriz cenários ↔ requisitos
13. Automação e casos não automatizados
14. Decisões e pontos em aberto

## 1. Introdução

Esta especificação detalha os testes de sistema da aplicação Android do Cliente, a partir das condições de teste do Plano de Testes do Frontend Mobile (I047, secção 10). Cada cenário verifica, na aplicação instalada num emulador e contra o backend real, um fluxo completo de um requisito do Cliente: autenticação e sessão, os próprios animais, as Consultas futuras e o histórico administrativo, e o boletim de vacinação e desparasitação. Inclui também a recusa dos perfis internos, o isolamento entre dois Clientes, a sessão expirada ou revogada e as falhas de rede e da API.

Os cenários são a base da automação com Compose UI Test instrumentado (I137 e I138), do relatório de resultados (I139) e servem de guia para a execução manual (secção 13.3). A aplicação móvel não cria nem altera dados: os únicos dados novos são os da preparação feita pela API antes da execução (secção 5.4).

## 2. Documentos de referência

| Documento                                                                 | Uso nesta especificação                                                                                                                                   |
| ------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Plano de Testes do Frontend Mobile v01 (I047)                             | Condições de teste (secção 10), ambiente (6.1), dados (6.2), comandos (7), critérios de saída (8.2) e decisões D-01 a D-13                                |
| Especificação de Requisitos v02 (I012)                                    | RF001–RF005, RF018, RF028, RF029, RF047 e RNF001–RNF006                                                                                                   |
| Especificações de Casos de Uso v02 (I015)                                 | UC01, UC02, UC08, UC11 e UC17 (fluxos e exceções do Cliente)                                                                                              |
| Documentação da API v03                                                   | EP-001 a EP-003, EP-019, EP-020, EP-030 e EP-044 (aplicação); EP-013, EP-025 e EP-043 (preparação dos dados); formato de erro `{ code, message, detail }` |
| Modelo de Classes do Frontend Mobile v03 (I022)                           | Ecrãs M01–M04 e M06, regras de sessão (6.2), mensagens de erro (12.2) e `testTag` (13)                                                                    |
| Protótipo de baixa fidelidade v02 (I017)                                  | Ecrãs M01 a M04 e M06; M05 fora do âmbito                                                                                                                 |
| Declaração de Âmbito v05                                                  | F005 (M05, despesas) e F007 fora do âmbito                                                                                                                |
| Código da aplicação (I121, I122 e I123) e `src/frontend-mobile/README.md` | Textos reais dos ecrãs, `testTag`, ecrã técnico de debug e configuração da URL da API                                                                     |
| Backlog de Projeto v05                                                    | Critérios da I136 e das tarefas de automação I137 e I138                                                                                                  |

## 3. Âmbito

- **Requisitos funcionais:** os 9 requisitos cujo ator inclui o Cliente — RF001–RF005 (F001), RF018 (F002), RF028 e RF029 (F003) e RF047 (F006).
- **Requisitos não funcionais:** RNF001, RNF002, RNF003, RNF005 e RNF006, onde se verificam na aplicação. O RNF004 não se aplica (a aplicação não cria nem altera dados); a validação local do início de sessão faz parte do RF002 (Plano, secção 15.3).
- **Perfis:** Cliente (contas `cliente.joao`, `cliente.maria` e uma conta criada na preparação); os quatro perfis internos e a conta bloqueada só para confirmar que são recusados.

Ficam fora desta especificação, sem cenários:

| Elemento                                                                       | Motivo                                                                                                                 |
| ------------------------------------------------------------------------------ | ---------------------------------------------------------------------------------------------------------------------- |
| F005 — Gestão de contas e pagamentos (RF042, ecrã M05 «despesas»)              | Fora do âmbito (Declaração de Âmbito v05); a I124 foi descontinuada e o ecrã M05 não é implementado                    |
| F007 — Internamento (RF048–RF052)                                              | Fora do âmbito (Declaração de Âmbito v05); não existe na aplicação móvel                                               |
| Funcionalidades dos perfis internos                                            | Exclusivas da aplicação web (RNF003); cobertas pela especificação do frontend web (I128). Aqui só se verifica a recusa |
| RF053 a RF056                                                                  | Sem requisito aprovado na Especificação de Requisitos v02; nenhum se destina à aplicação móvel (Plano, secção 3.2)     |
| Regras de negócio, autorização e persistência no servidor                      | Verificadas nos testes do backend                                                                                      |
| Modo offline, vários dispositivos, iOS, desempenho e build de release assinada | Não exigidos (Plano, secção 3.2 e D-04)                                                                                |

## 4. Convenções

### 4.1. Identificadores

| Prefixo                                                                | Uso                                                    | Exemplo          |
| ---------------------------------------------------------------------- | ------------------------------------------------------ | ---------------- |
| `ST-MOB-F001-nn`, `ST-MOB-F002-nn`, `ST-MOB-F003-nn`, `ST-MOB-F006-nn` | Cenário de uma funcionalidade                          | `ST-MOB-F003-02` |
| `ST-MOB-TRV-nn`                                                        | Cenário transversal                                    | `ST-MOB-TRV-01`  |
| `PD-nn`                                                                | Dado preparado pela API antes da execução (secção 5.4) | `PD-01`          |
| `PC-nn`                                                                | Procedimento comum (secção 5.6)                        | `PC-01`          |

Os testes automáticos têm no nome o identificador do cenário (critério 1 da I137), por exemplo `ST_MOB_F002_01_lista_dos_animais_do_cliente` ou, com nomes entre acentos graves, `` `ST-MOB-F002-01 — lista dos animais do Cliente` ``.

### 4.2. Campos de cada cenário

| Campo                     | Conteúdo                                                             |
| ------------------------- | -------------------------------------------------------------------- |
| Tipo                      | Positivo (fluxo esperado) ou Negativo (erro, recusa ou estado vazio) |
| Conta                     | Conta usada (secção 5.2)                                             |
| Requisitos                | RF e RNF verificados                                                 |
| UC / ecrãs                | Caso de uso e ecrãs                                                  |
| Condição                  | Condição de teste do Plano (I047, secção 10)                         |
| Automação                 | Tarefa que automatiza o cenário (I137 ou I138)                       |
| Pré-condições             | Estado antes do primeiro passo                                       |
| Dados                     | Dados de demonstração e dados preparados usados                      |
| Passos                    | Ações numeradas, com os rótulos reais da aplicação                   |
| Resultado esperado        | O que tem de se observar; um ponto que não se verifique é uma falha  |
| Dados criados e removidos | O que o cenário deixa na base                                        |

### 4.3. Notação nos passos

- «Texto» — texto visível na aplicação, tal como aparece.
- `m02-lista-animais` — `testTag` do elemento (Modelo de Classes v03, secção 13; D-12 do Plano), usado pela automação.
- `{D}` — data da marcação futura preparada (secção 5.4); `{hoje}` — data de hoje em Lisboa.
- Datas e horas na aplicação: formato `dd/MM/aaaa • HH:mm`, no fuso do dispositivo; o emulador está em `Europe/Lisbon`.

## 5. Ambiente, dados e regras de execução

### 5.1. Ambiente

Como no Plano de Testes do Frontend Mobile, secção 6.1:

- emulador Android de referência (Pixel 8, Android 17 / API 37) localmente e emulador sem interface na pipeline, com o **fuso horário `Europe/Lisbon`** e as animações desligadas;
- aplicação de debug instalada com `-PapiBaseUrl=http://localhost:5159` e o túnel `adb reverse tcp:5159 tcp:5159` (D-03 do Plano);
- backend real em `http://localhost:5159`, com a base reposta (I062), os dados de demonstração carregados (I061) e a preparação da secção 5.4 executada antes de cada execução completa. A API nunca é simulada, exceto na falha de rede da secção 13.2.

### 5.2. Contas

As palavras-passe nunca ficam no repositório, no código dos testes nem nos relatórios (D-08 do Plano). Os testes instrumentados recebem-nas como argumentos de instrumentação, cujo valor vem de variáveis de ambiente (localmente) ou de _secrets_ do GitHub (pipeline). Os nomes dos argumentos são definidos na I137; os nomes propostos são os da tabela.

| Perfil                 | Conta                                | Argumento proposto      | Uso                                                      |
| ---------------------- | ------------------------------------ | ----------------------- | -------------------------------------------------------- |
| Cliente                | `cliente.joao@clinica-g08.test`      | `senhaClienteJoao`      | Fluxos principais; Bobi e Mia                            |
| Cliente                | `cliente.maria@clinica-g08.test`     | `senhaClienteMaria`     | Segundo Cliente (isolamento); Kiko                       |
| Cliente sem animais    | criada em PD-03                      | gerada na preparação    | Estado vazio de M02                                      |
| Rececionista bloqueado | `rececao.bloqueada@clinica-g08.test` | `senhaRececaoBloqueada` | Conta bloqueada                                          |
| Administrador          | `admin@clinica-g08.test`             | `senhaAdmin`            | Perfil interno recusado                                  |
| Rececionista           | `rececao@clinica-g08.test`           | `senhaRececao`          | Perfil interno recusado; preparação PD-01, PD-02 e PD-03 |
| Veterinário            | `vet.ana@clinica-g08.test`           | `senhaVetAna`           | Perfil interno recusado; preparação PD-04                |
| Auxiliar Veterinário   | `aux.carla@clinica-g08.test`         | `senhaAuxCarla`         | Perfil interno recusado                                  |

### 5.3. Dados de base (dados de demonstração)

Os cenários só **leem** estes dados. Datas e horas em Lisboa.

| Cliente                       | Animais (M02)                                                                                            | Marcações (EP-030)                                                                                                                  | Boletim (EP-044)                                                                                                                                                                                                             |
| ----------------------------- | -------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| João Costa (`cliente.joao`)   | Bobi (Cão, Labrador Retriever, Macho, nascido a 10/03/2022, 28,4 kg) e Mia (Gato, Europeu Comum, 4,2 kg) | Bobi: 14/10/2026 10:30 Realizada (com registo clínico «Otite externa») e 05/11/2026 11:00 Pendente; Mia: 20/10/2026 11:00 Cancelada | Bobi: vacina antirrábica (fictícia, 14/10/2025, 365 dias), vacina polivalente (fictícia, 14/10/2026, 365 dias) e desparasitante interno (fictício, 14/10/2026, 90 dias); próximas 12/01/2027 e 14/10/2027. Mia: sem registos |
| Maria Silva (`cliente.maria`) | Kiko (Cão, sem raça definida, 12,8 kg)                                                                   | Kiko: 03/11/2026 09:00 Confirmada                                                                                                   | Kiko: uma desparasitação a 01/08/2026, sem próxima prevista                                                                                                                                                                  |

A antirrábica do Bobi (próxima a 14/10/2026) já passou e não aparece nas próximas administrações (comportamento da API verificado na I122).

### 5.4. Dados preparados antes da execução (D-13 do Plano)

Os dados de demonstração têm datas fixas: depois de 2026-11-05 nenhum Cliente tem Consultas futuras, e todos os Clientes têm animais. Antes de cada execução completa, a preparação cria pela API, com as contas internas de demonstração, os dados seguintes. Na automação, a preparação é feita pela I137 (pedidos HTTP antes dos testes); na execução manual, pelo Swagger do backend ou pelo frontend web com o perfil indicado.

| ID    | Dado                    | Operação e conta                                      | Valores                                                                                                                                                                                       |
| ----- | ----------------------- | ----------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| PD-01 | Marcação futura do Bobi | EP-025 (`POST /consultas`), Rececionista              | Bobi, Dra. Ana Veterinária, `{D}` às 10:00, motivo «Teste ST-MOB {execução}». `{D}` é o primeiro dia útil a pelo menos 7 dias de hoje                                                         |
| PD-02 | Marcação futura do Kiko | EP-025, Rececionista                                  | Kiko, Dra. Ana Veterinária, `{D}` às 11:00, motivo «Teste ST-MOB {execução}»                                                                                                                  |
| PD-03 | Cliente sem animais     | EP-013 (`POST /clientes`), Rececionista               | Nome «Cliente ST-MOB {execução}», e-mail `st.mob.{execução}@clinica-g08.test`, palavra-passe gerada na execução (12 caracteres, com minúscula, maiúscula, algarismo e símbolo), só em memória |
| PD-04 | Vacinação nova do Bobi  | EP-043 (`POST /animais/{id}/vacinacoes`), Veterinário | Tipo Vacinacao, produto «Vacina ST-MOB {execução} (fictícia)», lote «ST-{execução}», data de administração `{hoje}`, intervalo 30 dias (próxima prevista `{hoje}` + 30 dias)                  |

`{execução}` é a marca temporal da execução (por exemplo `20261120-1700`). Os dados ficam na base até à reposição seguinte e não interferem com outras execuções.

A preparação nunca altera a Mia, que continua sem registos de profilaxia e sem marcações futuras (boletim vazio e lista vazia). Os cenários indicam sempre se dependem da preparação.

### 5.5. Independência e espera

1. Cada cenário começa com a aplicação acabada de instalar ou com os dados da aplicação limpos (sem sessão guardada) e inicia a sua própria sessão (PC-01); nenhum depende de outro cenário nem da ordem.
2. A aplicação móvel não cria nem altera dados; os cenários não deixam dados na base, além dos da preparação.
3. Nunca se usam pausas fixas: cada passo espera por um elemento ou texto (`waitUntil` com `testTag`), pelo fim do indicador `estado-a-carregar` ou pela mensagem de erro.
4. Quando um cenário falha, a automação guarda uma captura de ecrã (critério 3 da I137).

### 5.6. Procedimentos comuns

**PC-01 — Iniciar sessão como \<conta\>**

1. Abrir a aplicação; esperar pelo ecrã «Iniciar sessão» (`m01-email`).
2. Escrever o e-mail em «Email» (`m01-email`) e a palavra-passe em «Palavra-passe» (`m01-password`).
3. Carregar em «Entrar» (`m01-entrar`).
4. Esperar por «Os meus animais» e pela lista `m02-lista-animais` (ou pelo estado vazio `m02-vazio`).

**PC-02 — Terminar sessão:** carregar em «Terminar sessão» (`nav-terminar-sessao`) e esperar pelo ecrã «Iniciar sessão» com o aviso «Terminou a sessão.» (`m01-aviso`).

**PC-03 — Limpar a aplicação:** `adb shell pm clear pt.ipp.estg.g08.clinica` (ou, na automação, apagar o armazenamento da sessão antes do teste).

## 6. Estado das funcionalidades na data desta versão

| Funcionalidade                                           | Tarefa | Estado a 2026-11-20                         |
| -------------------------------------------------------- | ------ | ------------------------------------------- |
| F001 — Autenticação e sessão (M01)                       | I121   | Integrada em `develop` (PR #261)            |
| F002 — Os meus animais (M02, M03) e F006 — Boletim (M06) | I122   | Integrada em `develop` (PR #273, `6ea37e2`) |
| F003 — Consultas (M04)                                   | I123   | Pull Request #274 em revisão (`7ee3a48`)    |

Os textos esperados foram confirmados no código destas três tarefas (`strings.xml`, `ApiException.kt`, `ValidacaoLogin.kt`). Se a I123 mudar algum texto na revisão, os cenários de F003 são atualizados numa versão nova desta especificação.

## 7. Cenários — F001 Autenticação e sessão

### ST-MOB-F001-01 — Iniciar sessão como Cliente

| Tipo     | Conta          | Requisitos   | UC / ecrãs      | Condição       | Automação |
| -------- | -------------- | ------------ | --------------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF001, RF005 | UC01 · M01, M02 | CT-MOB-F001-01 | I137      |

- **Pré-condições:** aplicação sem sessão guardada (PC-03).
- **Dados:** `cliente.joao`.
- **Passos:** 1. PC-01 com `cliente.joao`.
- **Resultado esperado:** abre «Os meus animais» com o Bobi e a Mia; a barra inferior tem «Animais», «Consultas» e «Terminar sessão»; não aparece nenhum ecrã de perfis internos.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-02 — Recusar credenciais inválidas e e-mail inexistente

| Tipo     | Conta                               | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | ----------------------------------- | ---------- | --------------- | -------------- | --------- |
| Negativo | `cliente.joao` e e-mail inexistente | RF002      | UC01 (E1) · M01 | CT-MOB-F001-02 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao@clinica-g08.test` com a palavra-passe `Errada#{execução}`; `naoexiste.{execução}@clinica-g08.test` com a mesma palavra-passe.
- **Passos:**
  1. Em «Iniciar sessão», escrever `cliente.joao@clinica-g08.test` e a palavra-passe errada; carregar em «Entrar».
  2. Substituir o e-mail pelo e-mail inexistente; carregar em «Entrar».
- **Resultado esperado:** nos passos 1 e 2 aparece a mesma mensagem «Email ou palavra-passe incorretos.» (`m01-erro`), sem indicar qual dos campos está errado; a aplicação continua em «Iniciar sessão» e não fica sessão guardada (ao fechar e reabrir a aplicação, volta a «Iniciar sessão»).
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-03 — Recusar uma conta bloqueada

| Tipo     | Conta               | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | ------------------- | ---------- | --------------- | -------------- | --------- |
| Negativo | `rececao.bloqueada` | RF002      | UC01 (E2) · M01 | CT-MOB-F001-02 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `rececao.bloqueada@clinica-g08.test` com a palavra-passe correta.
- **Passos:** 1. Escrever o e-mail e a palavra-passe; carregar em «Entrar».
- **Resultado esperado:** aparece «A conta encontra-se bloqueada.» (`m01-erro`); a aplicação continua em «Iniciar sessão» e não fica sessão guardada.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-04 — Assinalar campos em falta ou e-mail inválido antes do pedido

| Tipo     | Conta | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ----- | ---------- | ---------- | -------------- | --------- |
| Negativo | —     | RF002      | UC01 · M01 | CT-MOB-F001-03 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** e-mail `cliente.joao` (sem domínio).
- **Passos:**
  1. Com os dois campos vazios, carregar em «Entrar».
  2. Escrever `cliente.joao` em «Email», deixar «Palavra-passe» vazia e carregar em «Entrar».
- **Resultado esperado:** no passo 1 aparecem «Indique o email.» e «Indique a palavra-passe.»; no passo 2 aparecem «Indique um email válido.» e «Indique a palavra-passe.». Em nenhum dos passos é feito pedido à API (na automação: nenhum pedido a `/auth/login` no registo do servidor de teste ou do `ApiClient`); a aplicação continua em «Iniciar sessão».
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-05 — Recusar cada perfil interno

| Tipo     | Conta                                      | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------------------------------------ | ------------- | ---------- | -------------- | --------- |
| Negativo | `admin`, `rececao`, `vet.ana`, `aux.carla` | RF005, RNF003 | UC01 · M01 | CT-MOB-F001-04 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** as quatro contas internas, com as palavras-passe corretas.
- **Passos:** para cada conta: 1. escrever o e-mail e a palavra-passe e carregar em «Entrar»; 2. fechar e reabrir a aplicação.
- **Resultado esperado:** no passo 1 aparece «Esta aplicação destina-se apenas a clientes da clínica.» e a aplicação continua em «Iniciar sessão», sem barra inferior nem dados; no passo 2 abre «Iniciar sessão» (nada ficou guardado). O token recebido é revogado no servidor (EP-002): na automação confirma-se o pedido `POST /auth/logout` com resposta 204.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-06 — Terminar sessão

| Tipo     | Conta          | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | -------------- | ---------- | --------------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF003      | UC02 · M02, M01 | CT-MOB-F001-05 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. PC-02. 3. Fechar e reabrir a aplicação.
- **Resultado esperado:** no passo 2 aparece «Iniciar sessão» com «Terminou a sessão.»; no passo 3 continua em «Iniciar sessão» (a sessão foi apagada do dispositivo) e não aparece «Os meus animais».
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-07 — Restaurar a sessão ao reabrir a aplicação

| Tipo     | Conta          | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ---------- | ---------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF004      | UC01 · M02 | CT-MOB-F001-06 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Fechar a aplicação (remover da lista de aplicações recentes ou `adb shell am force-stop pt.ipp.estg.g08.clinica`). 3. Abrir a aplicação.
- **Resultado esperado:** no passo 3 aparece o indicador de arranque e, sem pedir credenciais, abre «Os meus animais» com o Bobi e a Mia (sessão confirmada com EP-003).
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-08 — Sessão inválida ou revogada ao abrir a aplicação

| Tipo     | Conta               | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------------- | ---------- | ---------- | -------------- | --------- |
| Negativo | — (sessão de teste) | RF004      | UC01 · M01 | CT-MOB-F001-07 | I138      |

- **Pré-condições:** aplicação de **debug** sem sessão guardada.
- **Dados:** a sessão de teste do ecrã técnico (token fictício, que a API recusa com 401).
- **Passos:**
  1. Em «Iniciar sessão», carregar em «Abrir ecrã técnico» (`m01-abrir-tecnico`).
  2. Carregar em «Guardar sessão de teste» (`tecnico-guardar-teste`); confirmar que «Sessão guardada (cifrada)» indica «Sim».
  3. Fechar e reabrir a aplicação.
- **Resultado esperado:** no passo 3 a aplicação confirma a sessão com EP-003, recebe 401, apaga a sessão e mostra «Iniciar sessão» com «A sua sessão terminou. Inicie sessão novamente.» (`m01-aviso`); não abre a área autenticada.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F001-09 — Sessão terminada no servidor durante a utilização

| Tipo     | Conta          | Requisitos   | UC / ecrãs      | Condição       | Automação |
| -------- | -------------- | ------------ | --------------- | -------------- | --------- |
| Negativo | `cliente.joao` | RF004, RF003 | UC01 · M02, M01 | CT-MOB-F001-07 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:**
  1. PC-01 com `cliente.joao`.
  2. Revogar no servidor o token da sessão aberta (automação: `POST /auth/logout` com o token lido da sessão da aplicação, técnica da secção 13.2).
  3. Na barra inferior, carregar em «Consultas».
- **Resultado esperado:** o pedido de M04 recebe 401; a aplicação termina a sessão local e mostra «Iniciar sessão» com «A sua sessão terminou. Inicie sessão novamente.»; ao reabrir a aplicação continua em «Iniciar sessão».
- **Dados criados e removidos:** nenhum.

## 8. Cenários — F002 Os meus animais

### ST-MOB-F002-01 — Ver a lista dos próprios animais

| Tipo     | Conta          | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ---------- | ---------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF018      | UC08 · M02 | CT-MOB-F002-01 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** animais do `cliente.joao` (secção 5.3).
- **Passos:** 1. PC-01 com `cliente.joao`.
- **Resultado esperado:** «Os meus animais» (`m02-lista-animais`) mostra o Bobi («Cão • Labrador Retriever» e «… • 28,4 kg») e a Mia («Gato • Europeu Comum» e «… • 4,2 kg»), cada um com «Ver detalhe»; o Kiko (da Maria) não aparece; não há ações de criar, alterar ou eliminar animais.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F002-02 — Ver o detalhe de um animal

| Tipo     | Conta          | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | -------------- | ---------- | --------------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF018      | UC08 · M02, M03 | CT-MOB-F002-02 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** Bobi.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Em «Bobi», carregar em «Ver detalhe». 3. Carregar em «Voltar» (`m03-voltar`).
- **Resultado esperado:** no passo 2 o detalhe (`m03-nome` = «Bobi») mostra «Espécie» Cão, «Raça» Labrador Retriever, «Sexo» Macho, «Data de nascimento» 10/03/2022, «Idade» e «Peso» 28,4 kg, e as ações «Ver boletim» e «Ver consultas», sem nenhuma ação de alteração; no passo 3 volta a «Os meus animais».
- **Dados criados e removidos:** nenhum.

### ST-MOB-F002-03 — Cliente sem animais

| Tipo     | Conta            | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ---------------- | ---------- | ---------- | -------------- | --------- |
| Negativo | Cliente de PD-03 | RF018      | UC08 · M02 | CT-MOB-F002-03 | I137      |

- **Pré-condições:** aplicação sem sessão guardada; preparação PD-03 executada.
- **Dados:** conta criada em PD-03.
- **Passos:** 1. PC-01 com a conta de PD-03.
- **Resultado esperado:** «Os meus animais» mostra «Ainda não tem animais associados à sua conta.» (`m02-vazio`), sem erro e sem indicador de carregamento; a barra inferior está disponível.
- **Dados criados e removidos:** nenhum (a conta é da preparação).

### ST-MOB-F002-04 — Recusar o detalhe de um animal de outro Cliente

| Tipo     | Conta          | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | `cliente.joao` | RF018, RNF002 | UC08 · M03 | CT-MOB-F002-04 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `id` do Kiko (da Maria), lido pela automação com EP-019 na sessão da Maria.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Abrir M03 com o `id` do Kiko (automação: navegação direta para a rota `animais/{id}`, técnica da secção 13.2).
- **Resultado esperado:** a API responde 403; M03 mostra «Não tem acesso a esta informação.» (`estado-erro`), sem nome, espécie ou outros dados do Kiko; a sessão do João mantém-se (a barra inferior continua disponível).
- **Dados criados e removidos:** nenhum.

## 9. Cenários — F003 Consultas futuras e histórico administrativo

### ST-MOB-F003-01 — Ver as Consultas futuras dos próprios animais

| Tipo     | Conta          | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ------------- | ---------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF028, RNF005 | UC11 · M04 | CT-MOB-F003-01 | I137      |

- **Pré-condições:** aplicação sem sessão guardada; PD-01 e PD-02 executadas.
- **Dados:** PD-01 (Bobi) e PD-02 (Kiko).
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Na barra inferior, carregar em «Consultas». 3. Confirmar que o separador «Próximas» (`m04-separador-futuras`) está selecionado.
- **Resultado esperado:** a lista `m04-lista-futuras` mostra a marcação de PD-01: estado «Pendente», «`{D}` • 10:00», «Bobi • Cão» e «Dra. Ana Veterinária (demonstração)»; o motivo não aparece; a marcação do Kiko (PD-02) e as marcações passadas não aparecem; as marcações estão por data crescente.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F003-02 — Ver o histórico administrativo sem dados clínicos

| Tipo     | Conta          | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ---------- | ---------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF029      | UC11 · M04 | CT-MOB-F003-02 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** marcações passadas do Bobi e da Mia (secção 5.3).
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Carregar em «Consultas» e no separador «Histórico» (`m04-separador-historico`).
- **Resultado esperado:** `m04-lista-historico` mostra, da mais recente para a mais antiga, pelo menos: «05/11/2026 • 11:00» Bobi Pendente, «20/10/2026 • 11:00» Mia Cancelada e «14/10/2026 • 10:30» Bobi Realizada, cada uma só com estado, data e hora, animal e Veterinário. Não aparecem «Otite externa», notas clínicas, tratamento nem prescrição; não aparece nenhuma marcação do Kiko.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F003-03 — Sem ações de marcar, alterar, confirmar ou cancelar

| Tipo     | Conta          | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ------------ | ---------- | -------------- | --------- |
| Negativo | `cliente.joao` | RF028, RF029 | UC11 · M04 | CT-MOB-F003-03 | I137      |

- **Pré-condições:** aplicação sem sessão guardada; PD-01 executada.
- **Dados:** PD-01.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Abrir «Consultas» e percorrer «Próximas» e «Histórico». 3. Tocar numa marcação de cada separador.
- **Resultado esperado:** não existe nenhum botão ou ação para marcar, alterar, confirmar ou cancelar; os únicos elementos acionáveis do ecrã são os dois separadores e a barra inferior; tocar numa marcação não abre nenhuma ação.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F003-04 — Consultas de um só animal a partir do detalhe

| Tipo     | Conta          | Requisitos   | UC / ecrãs      | Condição       | Automação |
| -------- | -------------- | ------------ | --------------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF028, RF029 | UC11 · M03, M04 | CT-MOB-F003-04 | I137      |

- **Pré-condições:** aplicação sem sessão guardada; PD-01 executada.
- **Dados:** Bobi e Mia.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Abrir o detalhe do Bobi e carregar em «Ver consultas». 3. Abrir «Histórico». 4. Carregar em «Voltar» (`m04-voltar`).
- **Resultado esperado:** no passo 2 aparece «Só as Consultas deste animal.» (`m04-so-deste-animal`) e «Próximas» mostra só a marcação de PD-01; no passo 3 o histórico tem só marcações do Bobi (05/11/2026 e 14/10/2026), sem a da Mia; o separador «Animais» da barra inferior está assinalado; no passo 4 volta ao detalhe do Bobi.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F003-05 — Listas vazias de Consultas

| Tipo     | Conta                            | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | -------------------------------- | ------------ | ---------- | -------------- | --------- |
| Negativo | `cliente.joao`; Cliente de PD-03 | RF028, RF029 | UC11 · M04 | CT-MOB-F003-05 | I137      |

- **Pré-condições:** aplicação sem sessão guardada; PD-03 executada.
- **Dados:** Mia (sem marcações futuras); conta de PD-03 (sem marcações).
- **Passos:**
  1. PC-01 com `cliente.joao`; abrir o detalhe da Mia e «Ver consultas».
  2. PC-02; PC-01 com a conta de PD-03; abrir «Consultas» e os dois separadores.
- **Resultado esperado:** no passo 1 «Próximas» mostra «Este animal não tem Consultas marcadas.»; no passo 2 «Próximas» mostra «Não tem Consultas marcadas.» e «Histórico» mostra «Ainda não há Consultas no histórico.»; nenhum dos casos mostra erro.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F003-06 — Recusar as Consultas de um animal de outro Cliente

| Tipo     | Conta          | Requisitos           | UC / ecrãs      | Condição       | Automação |
| -------- | -------------- | -------------------- | --------------- | -------------- | --------- |
| Negativo | `cliente.joao` | RF028, RF029, RNF002 | UC11 (E1) · M04 | CT-MOB-F003-04 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `id` do Kiko (como em ST-MOB-F002-04).
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Abrir M04 com o `id` do Kiko (automação: rota `animais/{id}/consultas`, secção 13.2).
- **Resultado esperado:** a API responde 403; cada separador mostra «Não tem acesso a esta informação.», sem nenhuma marcação; a sessão do João mantém-se.
- **Dados criados e removidos:** nenhum.

## 10. Cenários — F006 Boletim de vacinação e desparasitação

### ST-MOB-F006-01 — Ver o boletim de um animal

| Tipo     | Conta          | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | -------------- | ---------- | --------------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF047      | UC17 · M03, M06 | CT-MOB-F006-01 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** boletim do Bobi (secção 5.3).
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Abrir o detalhe do Bobi e carregar em «Ver boletim». 3. Carregar em «Voltar» (`m06-voltar`).
- **Resultado esperado:** abre «Boletim de vacinação e desparasitação»; «Histórico» (`m06-historico`) mostra, pelo menos, as três administrações dos dados de demonstração (antirrábica, polivalente e desparasitante interno), cada uma com o produto, «Administrada em …», o lote e o Veterinário; não há ações de registar ou alterar; no passo 3 volta ao detalhe do Bobi.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F006-02 — Próximas administrações calculadas pela API

| Tipo     | Conta          | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ------------- | ---------- | -------------- | --------- |
| Positivo | `cliente.joao` | RF047, RNF005 | UC17 · M06 | CT-MOB-F006-02 | I137      |

- **Pré-condições:** aplicação sem sessão guardada; PD-04 executada.
- **Dados:** boletim do Bobi, com a vacinação criada em PD-04.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Abrir o detalhe do Bobi e «Ver boletim».
- **Resultado esperado:** «Próximas administrações previstas» (`m06-proximas`) mostra a vacina de PD-04 com «Prevista para `{hoje}` + 30 dias» (o valor de `dataProximaPrevista` devolvido pela API em PD-04), «Prevista para 12/01/2027» (desparasitante interno) e «Prevista para 14/10/2027» (polivalente), e não mostra a antirrábica, cuja data prevista já passou; o «Histórico» mostra a vacina de PD-04 com «Administrada em `{hoje}`». Os dados registados por um Veterinário pela API aparecem na consulta posterior do Cliente (RNF005).
- **Dados criados e removidos:** nenhum.

### ST-MOB-F006-03 — Boletim sem registos e boletim sem próximas

| Tipo     | Conta                           | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------------------------- | ---------- | ---------- | -------------- | --------- |
| Negativo | `cliente.maria`; `cliente.joao` | RF047      | UC17 · M06 | CT-MOB-F006-03 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** Kiko (uma desparasitação sem próxima); Mia (sem registos).
- **Passos:**
  1. PC-01 com `cliente.maria`; abrir o detalhe do Kiko e «Ver boletim».
  2. PC-02; PC-01 com `cliente.joao`; abrir o detalhe da Mia e «Ver boletim».
- **Resultado esperado:** no passo 1 «Histórico» mostra a desparasitação de «Administrada em 01/08/2026» e «Próximas administrações previstas» mostra «Não há administrações previstas.»; no passo 2 aparece «Este animal ainda não tem vacinações nem desparasitações registadas.» (`m06-vazio`); nenhum dos casos mostra erro.
- **Dados criados e removidos:** nenhum.

### ST-MOB-F006-04 — Recusar o boletim de um animal de outro Cliente

| Tipo     | Conta          | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | -------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | `cliente.joao` | RF047, RNF002 | UC17 · M06 | CT-MOB-F006-04 | I137      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `id` do Kiko.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Abrir M06 com o `id` do Kiko (automação: rota `animais/{id}/boletim`, secção 13.2).
- **Resultado esperado:** a API responde 403; o ecrã mostra «Não tem acesso a esta informação.», sem nenhuma administração do Kiko; a sessão do João mantém-se.
- **Dados criados e removidos:** nenhum.

## 11. Cenários transversais — perfis, isolamento, rede e erros

### ST-MOB-TRV-01 — Dois Clientes veem só a sua informação

| Tipo     | Conta                            | Requisitos                         | UC / ecrãs                       | Condição      | Automação |
| -------- | -------------------------------- | ---------------------------------- | -------------------------------- | ------------- | --------- |
| Negativo | `cliente.joao` e `cliente.maria` | RNF002, RF018, RF028, RF029, RF047 | UC08, UC11, UC17 · M02, M04, M06 | CT-MOB-TRV-01 | I138      |

- **Pré-condições:** aplicação sem sessão guardada; PD-01 e PD-02 executadas.
- **Dados:** animais, marcações e boletins dos dois Clientes.
- **Passos:**
  1. PC-01 com `cliente.maria`; ler «Os meus animais», «Consultas» (os dois separadores) e o boletim do Kiko.
  2. PC-02; PC-01 com `cliente.joao` no mesmo dispositivo; ler «Os meus animais» e «Consultas».
- **Resultado esperado:** no passo 1 só aparece o Kiko; «Próximas» mostra só a marcação de PD-02 e «Histórico» só a de 03/11/2026 09:00 (Confirmada); o boletim tem só a desparasitação do Kiko. No passo 2 só aparecem o Bobi e a Mia e as suas marcações; nenhum dado da Maria fica visível depois da troca de conta.
- **Dados criados e removidos:** nenhum.

### ST-MOB-TRV-02 — Sem funcionalidades internas nem fora do âmbito

| Tipo     | Conta          | Requisitos     | UC / ecrãs   | Condição      | Automação |
| -------- | -------------- | -------------- | ------------ | ------------- | --------- |
| Negativo | `cliente.joao` | RNF001, RNF003 | UC01 · todos | CT-MOB-TRV-02 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Percorrer a barra inferior e as ações de M02, M03, M04 e M06.
- **Resultado esperado:** a barra inferior tem só «Animais», «Consultas» e «Terminar sessão»; não há entradas de despesas (M05), agenda, pacientes, clientes, funcionários, registo clínico, internamento nem nenhuma ação de escrita. Na automação, todos os pedidos registados pelo `ApiClient` durante o cenário são a rotas `/auth/*` ou `/me/*`.
- **Dados criados e removidos:** nenhum.

### ST-MOB-TRV-03 — Backend indisponível ao abrir um ecrã e «Tentar novamente»

| Tipo     | Conta           | Requisitos | UC / ecrãs            | Condição      | Automação |
| -------- | --------------- | ---------- | --------------------- | ------------- | --------- |
| Negativo | `cliente.maria` | RNF006     | UC08, UC11 · M03, M04 | CT-MOB-TRV-04 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** Kiko.
- **Passos:**
  1. PC-01 com `cliente.maria`.
  2. Tornar o backend inacessível (manual: parar o backend; automação: técnica da secção 13.2).
  3. Abrir o detalhe do Kiko.
  4. Carregar em «Tentar novamente» (`estado-tentar-novamente`) com o backend ainda inacessível.
  5. Repor o backend e carregar em «Tentar novamente».
  6. Repetir os passos 2 a 5 em «Consultas».
- **Resultado esperado:** nos passos 3 e 4 aparece «Sem ligação ao servidor. Verifique a sua ligação à rede.» (`estado-erro`) com «Tentar novamente», sem bloquear a aplicação (a barra inferior continua a responder); no passo 5 aparecem os dados do Kiko; em M04, o erro e a recuperação são os mesmos em cada separador; a sessão mantém-se.
- **Dados criados e removidos:** nenhum.

### ST-MOB-TRV-04 — Backend indisponível no início de sessão e no arranque

| Tipo     | Conta          | Requisitos    | UC / ecrãs | Condição      | Automação |
| -------- | -------------- | ------------- | ---------- | ------------- | --------- |
| Negativo | `cliente.joao` | RNF006, RF004 | UC01 · M01 | CT-MOB-TRV-04 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:**
  1. Com o backend inacessível, escrever as credenciais do João e carregar em «Entrar».
  2. Repor o backend; PC-01 com `cliente.joao`; fechar a aplicação.
  3. Tornar o backend inacessível e abrir a aplicação.
  4. Repor o backend e carregar em «Tentar novamente».
- **Resultado esperado:** no passo 1 aparece «Sem ligação ao servidor. Verifique a sua ligação à rede.» e não fica sessão guardada; no passo 3 aparece o mesmo erro com «Tentar novamente» e a sessão guardada **não** é apagada; no passo 4 abre «Os meus animais» sem pedir credenciais.
- **Dados criados e removidos:** nenhum.

### ST-MOB-TRV-05 — Erro do servidor mostrado em português, sem detalhe técnico

| Tipo     | Conta          | Requisitos | UC / ecrãs | Condição      | Automação |
| -------- | -------------- | ---------- | ---------- | ------------- | --------- |
| Negativo | `cliente.joao` | RNF001     | UC08 · M02 | CT-MOB-TRV-03 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:** 1. PC-01 com `cliente.joao`. 2. Fazer a API responder 500 a `GET /me/animais` (automação: técnica da secção 13.2). 3. Carregar em «Animais» ou em «Tentar novamente».
- **Resultado esperado:** aparece «Ocorreu um erro no servidor. Tente novamente mais tarde.» com «Tentar novamente»; não aparecem códigos, nomes de exceções, rotas nem o corpo técnico da resposta; a sessão mantém-se.
- **Dados criados e removidos:** nenhum.

### ST-MOB-TRV-06 — Estados de carregamento

| Tipo     | Conta          | Requisitos                         | UC / ecrãs            | Condição      | Automação |
| -------- | -------------- | ---------------------------------- | --------------------- | ------------- | --------- |
| Positivo | `cliente.joao` | — (qualidade de interface), RNF006 | UC01, UC08 · M01, M02 | CT-MOB-TRV-05 | I138      |

- **Pré-condições:** aplicação sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:** 1. Com a resposta da API atrasada (automação: técnica da secção 13.2; manual: rede do emulador limitada), iniciar sessão com o João. 2. Abrir «Consultas».
- **Resultado esperado:** durante o pedido de início de sessão o botão «Entrar» fica inativo; durante cada carregamento aparece o indicador `estado-a-carregar`, que desaparece quando chegam os dados; nenhum ecrã fica preso no indicador.
- **Dados criados e removidos:** nenhum.

### ST-MOB-TRV-07 — Sessão guardada só cifrada e sem token visível

| Tipo     | Conta          | Requisitos | UC / ecrãs               | Condição       | Automação |
| -------- | -------------- | ---------- | ------------------------ | -------------- | --------- |
| Positivo | `cliente.joao` | RNF001     | UC01 · M01, ecrã técnico | CT-MOB-F001-08 | I138      |

- **Pré-condições:** aplicação de **debug** sem sessão guardada.
- **Dados:** `cliente.joao`.
- **Passos:**
  1. PC-01 com `cliente.joao`.
  2. Ler o ficheiro de preferências da sessão no dispositivo (`adb shell run-as pt.ipp.estg.g08.clinica ls shared_prefs` e o ficheiro da sessão).
  3. PC-02 e abrir o ecrã técnico.
- **Resultado esperado:** no passo 2 o ficheiro não contém o token nem o e-mail em claro (só dados cifrados); no passo 3 o ecrã técnico indica «Sessão guardada (cifrada): Não» e nunca mostra o token. Os dados da sessão estão excluídos das cópias de segurança (`allowBackup=false`), confirmado no `AndroidManifest.xml` da build testada.
- **Dados criados e removidos:** nenhum.

## 12. Matriz cenários ↔ requisitos

### 12.1. Por requisito

| Requisito                                     | Positivos                     | Negativos                                        |
| --------------------------------------------- | ----------------------------- | ------------------------------------------------ |
| RF001 — Autenticar utilizador                 | F001-01                       | — (as falhas do início de sessão estão no RF002) |
| RF002 — Rejeitar autenticação inválida        | — (o caso positivo é o RF001) | F001-02, F001-03, F001-04                        |
| RF003 — Terminar sessão                       | F001-06                       | F001-09 (sessão terminada no servidor)           |
| RF004 — Manter acesso autenticado válido      | F001-07                       | F001-08, F001-09, TRV-04                         |
| RF005 — Restringir funcionalidades por perfil | F001-01                       | F001-05                                          |
| RF018 — Consultar animais do Cliente          | F002-01, F002-02              | F002-03, F002-04, TRV-01                         |
| RF028 — Consultar marcações futuras           | F003-01, F003-04              | F003-03, F003-05, F003-06, TRV-01                |
| RF029 — Consultar histórico administrativo    | F003-02, F003-04              | F003-03, F003-05, F003-06, TRV-01                |
| RF047 — Consultar boletim na aplicação móvel  | F006-01, F006-02              | F006-03, F006-04, TRV-01                         |
| RNF001 — Controlo de acesso                   | TRV-07                        | TRV-02, TRV-05                                   |
| RNF002 — Isolamento dos dados de Clientes     | —                             | F002-04, F003-06, F006-04, TRV-01                |
| RNF003 — Separação dos canais                 | —                             | F001-05, TRV-02                                  |
| RNF005 — Persistência dos dados confirmados   | F003-01, F006-02              | —                                                |
| RNF006 — Dependência de ligação à rede        | TRV-06                        | TRV-03, TRV-04                                   |

Os 9 requisitos funcionais do Cliente têm pelo menos um cenário positivo; os que têm caso de erro aplicável têm também um negativo. O RF001 e o RF002 são o par positivo/negativo do mesmo fluxo (início de sessão), como na especificação do frontend web (I128). Nenhum cenário verifica F005, F007 ou funcionalidades dos perfis internos.

### 12.2. Critério 3 da I136

| Situação exigida        | Cenários                                                                                                                  |
| ----------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| Sessão expirada         | ST-MOB-F001-08 (sessão recusada pela API no arranque), ST-MOB-F001-09 (sessão terminada no servidor durante a utilização) |
| Conta de perfil interno | ST-MOB-F001-05                                                                                                            |
| Dois Clientes           | ST-MOB-TRV-01, ST-MOB-F002-04, ST-MOB-F003-06, ST-MOB-F006-04                                                             |
| Falha de rede           | ST-MOB-TRV-03, ST-MOB-TRV-04                                                                                              |

### 12.3. Cenários, condições do Plano e casos de uso

| Cenário        | Tipo     | Requisitos                         | Condição       | UC               | Automação |
| -------------- | -------- | ---------------------------------- | -------------- | ---------------- | --------- |
| ST-MOB-F001-01 | Positivo | RF001, RF005                       | CT-MOB-F001-01 | UC01             | I137      |
| ST-MOB-F001-02 | Negativo | RF002                              | CT-MOB-F001-02 | UC01             | I137      |
| ST-MOB-F001-03 | Negativo | RF002                              | CT-MOB-F001-02 | UC01             | I137      |
| ST-MOB-F001-04 | Negativo | RF002                              | CT-MOB-F001-03 | UC01             | I137      |
| ST-MOB-F001-05 | Negativo | RF005, RNF003                      | CT-MOB-F001-04 | UC01             | I138      |
| ST-MOB-F001-06 | Positivo | RF003                              | CT-MOB-F001-05 | UC02             | I138      |
| ST-MOB-F001-07 | Positivo | RF004                              | CT-MOB-F001-06 | UC01             | I138      |
| ST-MOB-F001-08 | Negativo | RF004                              | CT-MOB-F001-07 | UC01             | I138      |
| ST-MOB-F001-09 | Negativo | RF004, RF003                       | CT-MOB-F001-07 | UC01             | I138      |
| ST-MOB-F002-01 | Positivo | RF018                              | CT-MOB-F002-01 | UC08             | I137      |
| ST-MOB-F002-02 | Positivo | RF018                              | CT-MOB-F002-02 | UC08             | I137      |
| ST-MOB-F002-03 | Negativo | RF018                              | CT-MOB-F002-03 | UC08             | I137      |
| ST-MOB-F002-04 | Negativo | RF018, RNF002                      | CT-MOB-F002-04 | UC08             | I137      |
| ST-MOB-F003-01 | Positivo | RF028, RNF005                      | CT-MOB-F003-01 | UC11             | I137      |
| ST-MOB-F003-02 | Positivo | RF029                              | CT-MOB-F003-02 | UC11             | I137      |
| ST-MOB-F003-03 | Negativo | RF028, RF029                       | CT-MOB-F003-03 | UC11             | I137      |
| ST-MOB-F003-04 | Positivo | RF028, RF029                       | CT-MOB-F003-04 | UC11             | I137      |
| ST-MOB-F003-05 | Negativo | RF028, RF029                       | CT-MOB-F003-05 | UC11             | I137      |
| ST-MOB-F003-06 | Negativo | RF028, RF029, RNF002               | CT-MOB-F003-04 | UC11             | I137      |
| ST-MOB-F006-01 | Positivo | RF047                              | CT-MOB-F006-01 | UC17             | I137      |
| ST-MOB-F006-02 | Positivo | RF047, RNF005                      | CT-MOB-F006-02 | UC17             | I137      |
| ST-MOB-F006-03 | Negativo | RF047                              | CT-MOB-F006-03 | UC17             | I137      |
| ST-MOB-F006-04 | Negativo | RF047, RNF002                      | CT-MOB-F006-04 | UC17             | I137      |
| ST-MOB-TRV-01  | Negativo | RNF002, RF018, RF028, RF029, RF047 | CT-MOB-TRV-01  | UC08, UC11, UC17 | I138      |
| ST-MOB-TRV-02  | Negativo | RNF001, RNF003                     | CT-MOB-TRV-02  | UC01             | I138      |
| ST-MOB-TRV-03  | Negativo | RNF006                             | CT-MOB-TRV-04  | UC08, UC11       | I138      |
| ST-MOB-TRV-04  | Negativo | RNF006, RF004                      | CT-MOB-TRV-04  | UC01             | I138      |
| ST-MOB-TRV-05  | Negativo | RNF001                             | CT-MOB-TRV-03  | UC08             | I138      |
| ST-MOB-TRV-06  | Positivo | RNF006                             | CT-MOB-TRV-05  | UC01, UC08       | I138      |
| ST-MOB-TRV-07  | Positivo | RNF001                             | CT-MOB-F001-08 | UC01             | I138      |

Total: 30 cenários (12 positivos e 18 negativos). Todas as condições do Plano (secção 10) têm cenário, exceto a CT-MOB-TRV-06 (HTTPS em release), que é verificada estaticamente (secção 13.4).

## 13. Automação e casos não automatizados

### 13.1. Distribuição pelas tarefas de automação

| Tarefa    | Âmbito                                            | Cenários                                                                   | N.º    |
| --------- | ------------------------------------------------- | -------------------------------------------------------------------------- | ------ |
| I137      | Início de sessão, animais, Consultas e boletim    | F001-01 a F001-04, F002-01 a F002-04, F003-01 a F003-06, F006-01 a F006-04 | 18     |
| I138      | Sessão, perfis internos, isolamento, rede e erros | F001-05 a F001-09, TRV-01 a TRV-07                                         | 12     |
| **Total** |                                                   | 12 positivos e 18 negativos                                                | **30** |

### 13.2. Técnicas da automação

Todos os cenários são automatizáveis com Compose UI Test instrumentado contra o backend real. As situações que à mão exigem intervenção fora da aplicação têm uma técnica equivalente:

| Cenário                   | Intervenção manual                                              | Técnica na automação                                                                                                                                                           |
| ------------------------- | --------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| F002-04, F003-06, F006-04 | Não há forma de pedir pela interface um animal de outro Cliente | Navegação direta para a rota do ecrã (`animais/{id}`, `animais/{id}/consultas`, `animais/{id}/boletim`) com o `NavController` do teste e o `id` do Kiko; o pedido à API é real |
| F001-09                   | Revogar o token no servidor                                     | `POST /auth/logout` feito pelo teste com o token lido do `TokenStorage` da aplicação                                                                                           |
| TRV-03, TRV-04            | Parar o backend                                                 | Remover o túnel (`adb reverse --remove tcp:5159`) durante o passo e repô-lo depois, ou parar o backend de teste; nunca simular uma resposta com dados                          |
| TRV-05                    | Provocar um erro 500                                            | `ApiClient` do teste apontado para um servidor intermédio que devolve 500 só para `GET /me/animais`; os restantes pedidos seguem para o backend real                           |
| TRV-06                    | Limitar a rede                                                  | O mesmo servidor intermédio atrasa a resposta real até o indicador ser verificado (espera por condição)                                                                        |
| TRV-02                    | —                                                               | Registo dos pedidos feitos pelo `ApiClient` (interceptor de teste)                                                                                                             |

Se algum cenário não puder ser automatizado, a tarefa de automação regista a justificação no Pull Request e o cenário é executado à mão e registado no relatório de resultados (I139).

### 13.3. Execução manual

Para a verificação 2 do Revisor da I136 e para repetir um cenário à mão:

1. Repor a base e carregar os dados de demonstração; arrancar o backend (secção 5.1).
2. Fazer a preparação da secção 5.4 que o cenário indica (Swagger do backend ou frontend web).
3. Instalar a aplicação com `-PapiBaseUrl=http://localhost:5159`, fazer `adb reverse tcp:5159 tcp:5159` e limpar os dados da aplicação (PC-03).
4. Seguir os passos e comparar com o resultado esperado.

Os cenários F002-04, F003-06 e F006-04 não são executáveis à mão pela interface (a aplicação não oferece forma de pedir um animal de outro Cliente); à mão verifica-se a resposta 403 de EP-020, EP-030 com `animalId` e EP-044 com o token do João no Swagger, e a apresentação do 403 está coberta pelos testes de componentes da I122 e da I123.

### 13.4. Verificação estática

A condição CT-MOB-TRV-06 (HTTPS em release; HTTP só para endereços locais em debug) não é um fluxo da aplicação: verifica-se no `network_security_config.xml` de debug e na configuração de release, e no Android Lint (QG-05). Fica registada no relatório de análise estática (I050).

## 14. Decisões e pontos em aberto

| N.º  | Decisão                                                                                                                                                                                                                                                                                                              | Razão                                                                          |
| ---- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------ |
| E-01 | Os textos esperados são os da aplicação (I121, I122 e I123), e não os `detail` da API, porque o `ApiClient` substitui as mensagens de erro pelas suas (`ApiException.mensagemParaUtilizador()`)                                                                                                                      | Os cenários têm de ser exequíveis tal como escritos (verificação 2 do Revisor) |
| E-02 | Os dados que faltam aos dados de demonstração (marcações futuras, Cliente sem animais, vacinação nova) são criados pela API antes da execução (PD-01 a PD-04), com datas relativas à data da execução                                                                                                                | D-13 do Plano; RF028 e RNF005 sem caso positivo de outra forma                 |
| E-03 | A vacinação de PD-04 é registada no Bobi e não na Mia, que fica sem registos para o boletim vazio (ST-MOB-F006-03) e sem marcações futuras (ST-MOB-F003-05)                                                                                                                                                          | Cenários independentes da ordem e da preparação (secção 5.5)                   |
| E-04 | A expiração local da sessão (`expiraEm`) não tem cenário de sistema próprio: o token dos dados de demonstração dura mais do que uma execução. É verificada nos testes de componentes da I121; os cenários de sistema verificam a sessão recusada pela API (F001-08 e F001-09), que tem o mesmo efeito para o Cliente | Não alterar o tempo do emulador nem a configuração do backend nos testes       |
| E-05 | Os cenários de F003 baseiam-se na I123 ainda em revisão (Pull Request #274); a I137 confirma os textos depois da integração                                                                                                                                                                                          | Secção 6                                                                       |
| E-06 | O RNF004 não tem cenário: a aplicação não cria nem altera dados; a validação local do início de sessão está em F001-04 (RF002)                                                                                                                                                                                       | Plano, secção 15.3                                                             |

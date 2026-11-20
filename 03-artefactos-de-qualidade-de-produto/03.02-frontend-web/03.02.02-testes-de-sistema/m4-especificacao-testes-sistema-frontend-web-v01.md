# Especificação dos Testes de Sistema do Frontend Web — Gestão de Clínica Veterinária

| Campo                | Valor                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Grupo                | G08                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| Projeto              | Gestão de Clínica Veterinária                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| Artefacto            | Especificação dos Testes de Sistema do Frontend Web (`OF-M4-012`)                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Ficheiro             | `m4-especificacao-testes-sistema-frontend-web-v01.md`                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| Versão               | v01                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| Data                 | 2026-11-18                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| Milestone de Entrega | m4                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| Issue                | I128 — Especificar os testes de sistema do frontend web (GitHub #116, Sprint 07)                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| Papéis               | Executor: Gonçalo Dinis Magalhães Teixeira · Revisor: André Filipe Moreira Mesquita · Auditor: Gonçalo Magalhães Ribeiro                                                                                                                                                                                                                                                                                                                                                                                                        |
| Pasta de destino     | `/projeto/03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.02-testes-de-sistema`                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| Fontes               | `m4-plano-testes-frontend-web-v01.md` (I043); `m3-especificacao-requisitos-v02.md` (I012); `m3-especificacoes-casos-uso-v02.md` (I015); `m3-documentacao-api-v03.md`; `m3-modelo-classes-frontend-web-v03.md` (I021); `m3-prototipo-baixa-fidelidade-v02.md` (I017); `m3-declaracao-ambito-v05.pdf`; scripts de dados de demonstração `04.13-scripts-de-base-de-dados/m3-v02` (I061); `m4-backlog-projeto-v05.xlsx`; código do frontend web em `develop` (commit `1276a7d`: I107 a I114 e as correções integradas pelo PR #270) |

## Histórico de versões

| Versão | Data       | Autor                            | Alterações                                                                                                                                                                                        |
| ------ | ---------- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| v01    | 2026-11-18 | Gonçalo Dinis Magalhães Teixeira | Versão inicial: 73 cenários de sistema para os 37 requisitos funcionais web em âmbito e para os requisitos não funcionais com verificação na interface (RNF001, RNF003, RNF004, RNF005 e RNF006). |

## Índice

1. [Introdução](#1-introdução)
2. [Documentos de referência](#2-documentos-de-referência)
3. [Âmbito](#3-âmbito)
4. [Convenções](#4-convenções)
5. [Ambiente, dados e regras de execução](#5-ambiente-dados-e-regras-de-execução)
6. [Estado das funcionalidades na data desta versão](#6-estado-das-funcionalidades-na-data-desta-versão)
7. [Cenários — F001 Autenticação, contas e clínica](#7-cenários--f001-autenticação-contas-e-clínica)
8. [Cenários — F002 Pacientes](#8-cenários--f002-pacientes)
9. [Cenários — F003 Agendamento e consultas](#9-cenários--f003-agendamento-e-consultas)
10. [Cenários — F004 Registo clínico](#10-cenários--f004-registo-clínico)
11. [Cenários — F006 Boletim de vacinas e desparasitações](#11-cenários--f006-boletim-de-vacinas-e-desparasitações)
12. [Cenários transversais — perfis, sessão, rede e erros](#12-cenários-transversais--perfis-sessão-rede-e-erros)
13. [Matriz cenários ↔ requisitos](#13-matriz-cenários--requisitos)
14. [Automação e casos não automatizados](#14-automação-e-casos-não-automatizados)
15. [Decisões e pontos em aberto](#15-decisões-e-pontos-em-aberto)

## 1. Introdução

Este documento especifica os testes de sistema do frontend web: os cenários que verificam, do princípio ao fim e contra o backend real, que cada requisito funcional suportado pela aplicação web funciona para os perfis que o podem usar e é recusado aos restantes.

Cada cenário parte de uma condição de teste do Plano de Testes do Frontend Web (I043, secção 10) e indica o identificador, o tipo (positivo ou negativo), o perfil, os requisitos, as pré-condições, os dados, os passos numerados, o resultado esperado e os dados que cria e remove. A especificação serve de base à automação com Playwright (I129 a I132) e à execução e relatório dos resultados (I133).

Os cenários estão escritos para poderem ser executados à mão, tal como estão, por quem conheça a aplicação apenas como utilizador: os passos usam os nomes dos menus, campos, botões e mensagens que aparecem no ecrã.

## 2. Documentos de referência

| Documento                                    | Uso nesta especificação                                                                                                                                                     |
| -------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Plano de Testes do Frontend Web v01 (I043)   | Âmbito, convenção de identificadores (secção 4.1), ambiente e dados de teste (secção 6), condições de teste (secção 10), rastreabilidade (secção 15) e decisões D-01 a D-10 |
| Especificação de Requisitos v02 (I012)       | Requisitos funcionais e não funcionais, atores e critérios de aceitação                                                                                                     |
| Especificações de Casos de Uso v02 (I015)    | Fluxos principais e alternativos (UC01 a UC07, UC09, UC10, UC12, UC13 e UC16)                                                                                               |
| Documentação da API v03                      | Endpoints, limites de validação (secção 2.4), formato de erros `{ code, message, detail }` (secção 5) e alterações concorrentes (secção 5.1)                                |
| Modelo de Classes do Frontend Web v03 (I021) | Ecrãs W01 a W13 e W16, rotas e matriz de acessos por perfil (secção 12)                                                                                                     |
| Protótipo de baixa fidelidade v02 (I017)     | Ecrãs W13 e W16, ainda por implementar à data desta versão                                                                                                                  |
| Scripts de dados de demonstração (I061)      | Contas, Clientes, pacientes, marcações, registos clínicos e boletins usados como dados de base                                                                              |
| Backlog de Projeto v05                       | Critérios da I128 e das tarefas de automação I129 a I132                                                                                                                    |

## 3. Âmbito

O âmbito é o do Plano de Testes do Frontend Web, secção 3:

- **Requisitos funcionais:** os 37 requisitos web de F001, F002, F003, F004 e F006 — RF001 a RF017, RF019 a RF027, RF030 a RF036 e RF043 a RF046.
- **Requisitos não funcionais:** RNF001, RNF003, RNF004, RNF005 e RNF006.
- **Perfis:** Administrador, Rececionista, Veterinário e Auxiliar Veterinário; o Cliente só para confirmar que não usa a aplicação web (RNF003).

Ficam fora desta especificação, sem cenários:

| Elemento                                                               | Motivo                                                                                                                                                                                |
| ---------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| F005 (RF037 a RF042) e F007 (RF048 a RF052); ecrãs W14, W15, W17 e W18 | Fora do âmbito (Declaração de Âmbito v05). Nenhum cenário desta especificação usa estas funcionalidades                                                                               |
| RF018, RF028, RF029 e RF047                                            | Exclusivos do Cliente na aplicação móvel (RNF003); cobertos pela especificação do frontend mobile (I136)                                                                              |
| RF053 a RF055 e RF056 (EP-052)                                         | Sem requisito aprovado na Especificação de Requisitos v02 (deliberações 5 e 12 da ata do Backlog Refinement de 2026-11-07 e deliberação 5 da ata do Backlog Refinement de 2026-11-13) |
| RNF002                                                                 | Aplica-se à aplicação móvel e ao backend (Plano de Testes do Frontend Web, secção 15.3)                                                                                               |
| Regras de negócio e persistência no servidor                           | Verificadas nos testes do backend; aqui verifica-se o comportamento visto pelo utilizador web                                                                                         |

## 4. Convenções

### 4.1. Identificadores

| Prefixo                             | Uso                                                 | Exemplo          |
| ----------------------------------- | --------------------------------------------------- | ---------------- |
| `ST-WEB-F001-nn` a `ST-WEB-F006-nn` | Cenário de sistema de uma funcionalidade            | `ST-WEB-F003-05` |
| `ST-WEB-TRV-nn`                     | Cenário transversal (perfis, sessão, rede, erros)   | `ST-WEB-TRV-02`  |
| `PC-nn`                             | Procedimento comum, chamado nos passos dos cenários | `PC-01`          |

Os testes automáticos têm no nome o identificador do cenário (critério 1 da I129), por exemplo `ST-WEB-F001-07 — criar funcionário Rececionista`.

### 4.2. Campos de cada cenário

| Campo                     | Conteúdo                                                                                                                                                  |
| ------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Tipo                      | **Positivo** (a operação é aceite e o resultado fica visível) ou **Negativo** (dados inválidos, perfil sem permissão ou estado que não admite a operação) |
| Perfil                    | Perfil ou perfis com que o cenário é executado                                                                                                            |
| Requisitos                | Requisitos funcionais e não funcionais verificados                                                                                                        |
| UC / ecrãs                | Caso de uso de origem e ecrãs envolvidos                                                                                                                  |
| Condição                  | Condição de teste do Plano de Testes (secção 10) que o cenário detalha                                                                                    |
| Automação                 | Tarefa que automatiza o cenário (I129, I130, I131 ou I132)                                                                                                |
| Pré-condições             | Estado necessário antes do passo 1                                                                                                                        |
| Dados                     | Valores usados; `{sufixo}` e `{D…}` são definidos na secção 5.4                                                                                           |
| Passos                    | Ações numeradas do utilizador                                                                                                                             |
| Resultado esperado        | O que tem de se verificar; o cenário falha se algum ponto não se verificar                                                                                |
| Dados criados e removidos | O que o cenário deixa na base e como é limpo                                                                                                              |

### 4.3. Notação nos passos

- «Texto» — texto visível no ecrã: menu, campo, botão, título ou mensagem, escrito como aparece.
- `/rota` — endereço escrito diretamente na barra do navegador, depois de `http://localhost:5173` (ou do endereço do frontend na pipeline).
- «Escolher X em Y» — selecionar a opção X na lista Y.
- As horas são de Lisboa, como a aplicação as mostra.

## 5. Ambiente, dados e regras de execução

### 5.1. Ambiente

Como no Plano de Testes do Frontend Web, secção 6.1: frontend em `npm run dev` (`http://localhost:5173`) ou servido na pipeline; backend real em `http://localhost:5159` (ou o da pipeline), com a base `ClinicaVeterinaria` reposta pelo script de reposição (I062) e com os dados de demonstração (I061) carregados antes de cada execução completa. A API nunca é simulada (D-01). Na pipeline os cenários correm no Chromium (D-03).

### 5.2. Contas

As palavras-passe nunca ficam no repositório nem nos relatórios (D-07). Localmente são lidas de variáveis de ambiente e na pipeline de _secrets_ do GitHub, com os nomes seguintes:

| Perfil                                        | Conta                                | Nome apresentado                     | Variável com a palavra-passe  |
| --------------------------------------------- | ------------------------------------ | ------------------------------------ | ----------------------------- |
| Administrador                                 | `admin@clinica-g08.test`             | Administração (demonstração)         | `E2E_SENHA_ADMIN`             |
| Rececionista                                  | `rececao@clinica-g08.test`           | Rita Rececionista (demonstração)     | `E2E_SENHA_RECECAO`           |
| Rececionista bloqueado                        | `rececao.bloqueada@clinica-g08.test` | Rui Bloqueado (demonstração)         | `E2E_SENHA_RECECAO_BLOQUEADA` |
| Veterinário                                   | `vet.ana@clinica-g08.test`           | Dra. Ana Veterinária (demonstração)  | `E2E_SENHA_VET_ANA`           |
| Veterinário                                   | `vet.bruno@clinica-g08.test`         | Dr. Bruno Veterinário (demonstração) | `E2E_SENHA_VET_BRUNO`         |
| Auxiliar Veterinário com autorização de apoio | `aux.carla@clinica-g08.test`         | Carla Auxiliar (com apoio)           | `E2E_SENHA_AUX_CARLA`         |
| Auxiliar Veterinário sem autorização de apoio | `aux.diogo@clinica-g08.test`         | Diogo Auxiliar (sem apoio)           | `E2E_SENHA_AUX_DIOGO`         |
| Cliente                                       | `cliente.joao@clinica-g08.test`      | João Costa (demonstração)            | `E2E_SENHA_CLIENTE_JOAO`      |

As contas criadas pelos próprios cenários usam uma palavra-passe gerada na execução (12 caracteres, com minúscula, maiúscula, algarismo e símbolo, como exigem EP-006 e EP-013), que só existe em memória durante o cenário.

### 5.3. Dados de base (dados de demonstração)

Os cenários só **leem** estes dados; nenhum cenário os altera, exceto os que o indicam e os repõem no fim (secção 5.5).

| Tipo            | Dados usados nos cenários                                                                                                                                                                                                                |
| --------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Clínica         | «Clínica Veterinária G08», NIF 501234560, duração da consulta 30 minutos                                                                                                                                                                 |
| Horário         | Segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00; domingo encerrado                                                                                                                                                         |
| Clientes        | João Costa (demonstração), telefone 960000001, animais Bobi (Cão, Labrador Retriever) e Mia (Gato, Europeu Comum); Maria Silva (demonstração), animal Kiko (Cão); Rui Santos (demonstração), sem NIF, animal Luna (Gato, Siamês)         |
| Registo clínico | Bobi, consulta de 2026-10-14 09:30 com a Dra. Ana: diagnóstico «Otite externa», tratamento «Limpeza auricular com solução fisiológica»                                                                                                   |
| Boletim do Bobi | Vacina antirrábica (fictícia), lote RB-2291, 2025-10-14, intervalo 365 dias; Vacina polivalente (fictícia), lote PV-1187, 2026-10-14, intervalo 365 dias; Desparasitante interno (fictício), lote DP-0042, 2026-10-14, intervalo 90 dias |

### 5.4. Dados gerados por cada cenário

- **`{sufixo}`** — identificador curto do cenário seguido da marca temporal da execução, por exemplo `F001-07-1763290000`. Todos os nomes, e-mails e textos criados por um cenário levam o seu `{sufixo}`, para que nunca coincidam com dados de outro cenário ou de outra execução.
- **E-mails criados** — `st.{sufixo}@clinica-g08.test` (em minúsculas).
- **Telefones criados** — `93` seguido de 7 algarismos derivados da marca temporal.
- **`{D}`** — dia útil (segunda a sexta) de marcação, calculado na execução como o primeiro dia útil a pelo menos **N** dias de hoje, em que **N** é o desvio próprio de cada cenário indicado nos dados. Cenários diferentes usam desvios diferentes, para que dois cenários a correr em paralelo nunca disputem a mesma vaga do mesmo Veterinário.
- **`{hoje}`** — data de hoje em Lisboa.

### 5.5. Independência, limpeza e espera

1. Cada cenário inicia a sua própria sessão e cria os dados de que precisa (procedimentos da secção 5.6); nenhum cenário depende de dados criados por outro cenário nem da ordem de execução (D-10).
2. A API não tem operações de eliminação de funcionários, Clientes, pacientes, marcações, registos clínicos ou administrações. Os dados criados ficam isolados pelo `{sufixo}` e saem da base na reposição antes da execução completa seguinte. Em cada cenário, o campo «Dados criados e removidos» indica o que fica.
3. Os cenários que alteram dados partilhados (configuração e horário da clínica) repõem os valores originais no fim, também quando falham (passo de limpeza executado sempre), e estão marcados como **exclusivos**: não correm em paralelo com nenhum outro cenário.
4. Nunca se usam pausas fixas: cada passo espera por uma condição visível (um texto, um campo ativo, o fim do estado «A carregar…» ou «A guardar…») ou pela resposta do pedido correspondente.
5. Quando um cenário falha, a automação guarda uma captura de ecrã e o _trace_ (critério 3 da I129).

### 5.6. Procedimentos comuns

**PC-01 — Iniciar sessão como \<conta\>**

1. Abrir `/login`.
2. Escrever o e-mail da conta em «Email» e a palavra-passe (secção 5.2) em «Palavra-passe».
3. Carregar em «Entrar».
4. Esperar que o cabeçalho mostre o nome e o perfil da conta e que «Terminar sessão» esteja visível.

**PC-02 — Terminar sessão:** carregar em «Terminar sessão» e esperar pelo ecrã «Iniciar sessão».

**PC-03 — Criar um Cliente de teste (Rececionista)**

1. PC-01 com `rececao@clinica-g08.test`.
2. Menu «Clientes» → «Novo cliente».
3. Preencher «Nome» = `Cliente {sufixo}`, «Telefone» = telefone gerado, «E-mail» = e-mail gerado e «Palavra-passe inicial» = palavra-passe gerada; «NIF (opcional)» e «Morada (opcional)» vazios.
4. Carregar em «Registar cliente» e esperar pela mensagem «Cliente Cliente {sufixo} registado, com conta de acesso para …».

**PC-04 — Criar um paciente de teste (Rececionista)**

1. Com a sessão da Rececionista, menu «Pacientes» → «Novo paciente».
2. Em «Proprietário (nome, NIF, telefone ou e-mail)» escrever `Cliente {sufixo}`, carregar em «Procurar proprietário» e em «Selecionar Cliente {sufixo}».
3. «Nome do animal» = `Animal {sufixo}`, «Espécie» = Cão, «Raça (opcional)» = Labrador Retriever, «Sexo» = Macho, «Data de nascimento (opcional)» = 2023-01-01, «Peso em kg (opcional)» = 10,5.
4. Carregar em «Guardar dados administrativos» e esperar por «Paciente Animal {sufixo} registado para Cliente {sufixo}.».

**PC-05 — Criar uma marcação de teste (Rececionista)**

1. Com a sessão da Rececionista, menu «Agenda» → «Nova marcação».
2. Em «Paciente (nome do animal ou do proprietário)» escrever `Animal {sufixo}`, carregar em «Procurar paciente» e em «Selecionar Animal {sufixo} (Cliente {sufixo})».
3. Escolher o Veterinário indicado no cenário em «Veterinário», escrever `{D}` em «A partir de» e carregar em «Consultar disponibilidade».
4. Em «Horários livres», escolher o horário de `{D}` indicado no cenário (por omissão, o primeiro livre).
5. «Motivo (opcional)» = `Motivo {sufixo}`; carregar em «Criar marcação» e esperar por «Marcação criada no estado Pendente.» no detalhe da marcação.

**PC-06 — Confirmar a marcação aberta (Rececionista):** no detalhe da marcação, carregar em «Confirmar marcação», confirmar em «Confirmação» e esperar por «Estado alterado para Confirmada.».

## 6. Estado das funcionalidades na data desta versão

| Funcionalidade | Ecrãs     | Tarefa de desenvolvimento | Estado em `develop` (`1276a7d`)                      | Consequência para os cenários                                                                                                                                                  |
| -------------- | --------- | ------------------------- | ---------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| F001           | W01 a W06 | I107 a I111               | Integrado                                            | Passos escritos com os rótulos reais da aplicação                                                                                                                              |
| F002           | W07, W08  | I112                      | Integrado                                            | Idem                                                                                                                                                                           |
| F003           | W09 a W12 | I113, I114                | Integrado                                            | Idem                                                                                                                                                                           |
| F004           | W13       | I115, I116 (Sprint 07)    | Por integrar: a rota existe e mostra «em construção» | Passos escritos por ação, a partir do protótipo W13, da API v03 e dos critérios das I115 e I116; os nomes exatos dos botões confirmam-se quando estas tarefas forem integradas |
| F006           | W16       | I118 (Sprint 07)          | Por integrar: a rota existe e mostra «em construção» | Idem, a partir do protótipo W16 e dos critérios da I118                                                                                                                        |

O resultado esperado dos cenários de F004 e F006 não depende dos rótulos: está fixado pela especificação de requisitos e pela API v03. Se a implementação usar rótulos diferentes, a I130 e a I131 usam os da implementação e o relatório de resultados (I133) regista a correspondência; se a implementação mudar o comportamento esperado, sai uma versão nova desta especificação.

## 7. Cenários — F001 Autenticação, contas e clínica

### ST-WEB-F001-01 — Iniciar sessão com cada perfil interno

| Tipo     | Perfil                                                         | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | -------------------------------------------------------------- | ------------ | ---------- | -------------- | --------- |
| Positivo | Administrador, Rececionista, Veterinário, Auxiliar Veterinário | RF001, RF005 | UC01 · W01 | CT-WEB-F001-01 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** contas `admin@`, `rececao@`, `vet.ana@` e `aux.carla@` (secção 5.2).
- **Passos:** para cada uma das quatro contas:
  1. PC-01 com a conta.
  2. Registar o endereço aberto e as entradas do menu lateral.
  3. PC-02.
- **Resultado esperado:**

  | Conta        | Página inicial | Cabeçalho                                           | Menu                                       |
  | ------------ | -------------- | --------------------------------------------------- | ------------------------------------------ |
  | `admin@`     | `/clientes`    | «Administração (demonstração) · Administrador»      | Clientes, Funcionários, Clínica e horários |
  | `rececao@`   | `/agenda`      | «Rita Rececionista (demonstração) · Rececionista»   | Agenda, Pacientes, Clientes                |
  | `vet.ana@`   | `/agenda`      | «Dra. Ana Veterinária (demonstração) · Veterinário» | Agenda, Pacientes                          |
  | `aux.carla@` | `/preparacao`  | «Carla Auxiliar (com apoio) · Auxiliar veterinário» | Preparação de consultas                    |

- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-02 — Recusar credenciais inválidas

| Tipo     | Perfil       | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF002, RNF004 | UC01 · W01 | CT-WEB-F001-02 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** e-mail `rececao@clinica-g08.test` com a palavra-passe `Errada#{sufixo}`; e-mail `naoexiste.{sufixo}@clinica-g08.test` com a mesma palavra-passe.
- **Passos:**
  1. Abrir `/login`, escrever o e-mail existente com a palavra-passe errada e carregar em «Entrar».
  2. Repetir com o e-mail inexistente.
  3. Abrir `/agenda`.
- **Resultado esperado:**
  1. Nos passos 1 e 2 aparece «Email ou palavra-passe incorretos.», o endereço continua em `/login`, o cabeçalho não mostra utilizador e o campo «Palavra-passe» fica utilizável para nova tentativa.
  2. A mensagem é a mesma nos dois casos (não revela se o e-mail existe).
  3. No passo 3 a aplicação volta a `/login` (não há sessão).
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-03 — Recusar o início de sessão de uma conta bloqueada

| Tipo     | Perfil                 | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | ---------------------- | ------------ | ---------- | -------------- | --------- |
| Negativo | Rececionista bloqueado | RF002, RF009 | UC01 · W01 | CT-WEB-F001-02 | I129      |

- **Pré-condições:** sem sessão iniciada; a conta `rececao.bloqueada@` está bloqueada nos dados de demonstração.
- **Dados:** `rececao.bloqueada@clinica-g08.test` com a palavra-passe correta (`E2E_SENHA_RECECAO_BLOQUEADA`).
- **Passos:**
  1. Abrir `/login`, preencher as credenciais e carregar em «Entrar».
- **Resultado esperado:** aparece «Esta conta está bloqueada. Contacte o administrador da clínica.»; o endereço continua em `/login` e não é aberta nenhuma página protegida.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-04 — Terminar sessão e não voltar a páginas protegidas

| Tipo     | Perfil       | Requisitos   | UC / ecrãs      | Condição       | Automação |
| -------- | ------------ | ------------ | --------------- | -------------- | --------- |
| Positivo | Rececionista | RF003, RF004 | UC02 · W01, W04 | CT-WEB-F001-03 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** `rececao@`.
- **Passos:**
  1. PC-01 com `rececao@`; abrir o menu «Clientes».
  2. Carregar em «Terminar sessão».
  3. Usar o botão «Retroceder» do navegador.
  4. Abrir `/clientes` diretamente.
- **Resultado esperado:**
  1. Depois do passo 2 aparece o ecrã «Iniciar sessão» e o cabeçalho deixa de mostrar o utilizador.
  2. Nos passos 3 e 4 a lista de Clientes não volta a aparecer: a aplicação mostra «Iniciar sessão» em `/login`.
  3. O pedido de fim de sessão (EP-002) é enviado ao backend; um novo início de sessão volta a funcionar normalmente.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-05 — Manter a sessão ao navegar e ao recarregar

| Tipo     | Perfil      | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | ----------- | ---------- | --------------- | -------------- | --------- |
| Positivo | Veterinário | RF004      | UC01 · W07, W09 | CT-WEB-F001-04 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** `vet.ana@`.
- **Passos:**
  1. PC-01 com `vet.ana@`.
  2. Abrir «Pacientes», depois «Agenda», depois «Pacientes».
  3. Recarregar a página (F5).
- **Resultado esperado:** em todas as navegações e depois de recarregar, a página pedida abre sem voltar a pedir credenciais, o cabeçalho continua com «Dra. Ana Veterinária (demonstração) · Veterinário» e, depois de recarregar, a lista de pacientes volta a aparecer (a sessão é confirmada no backend, EP-003).
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-06 — Listar e pesquisar funcionários

| Tipo     | Perfil        | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------ | ---------- | -------------- | --------- |
| Positivo | Administrador | RF007, RF009 | UC03 · W02 | CT-WEB-F001-05 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** pesquisa «Carla»; filtro «Perfil» = Auxiliar Veterinário; filtro «Estado» = Bloqueado.
- **Passos:**
  1. PC-01 com `admin@`; menu «Funcionários».
  2. Escrever «Carla» em «Pesquisar (nome ou e-mail)» e pesquisar.
  3. Limpar a pesquisa e escolher Auxiliar Veterinário em «Perfil».
  4. Escolher «Todos» em «Perfil» e Bloqueado em «Estado».
- **Resultado esperado:**
  1. No passo 1 a lista mostra, para cada funcionário, nome, e-mail, perfil, estado e a coluna «Registo de apoio» («Autorizado» ou «Não autorizado» nos Auxiliares e «—» nos restantes).
  2. No passo 2 aparece só «Carla Auxiliar (com apoio)», com «Registo de apoio» = «Autorizado».
  3. No passo 3 aparecem só Auxiliares, incluindo Carla («Autorizado») e Diogo («Não autorizado»).
  4. No passo 4 aparece «Rui Bloqueado (demonstração)» e não aparecem contas ativas.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-07 — Criar uma conta de funcionário e atribuir-lhe um perfil

| Tipo     | Perfil                                    | Requisitos          | UC / ecrãs      | Condição       | Automação |
| -------- | ----------------------------------------- | ------------------- | --------------- | -------------- | --------- |
| Positivo | Administrador; depois o novo Rececionista | RF006, RF008, RF005 | UC03 · W02, W03 | CT-WEB-F001-05 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** «Nome» = `Funcionário {sufixo}`; «E-mail» = e-mail gerado; «Telefone (opcional)» = telefone gerado; «Perfil» = Rececionista; «Palavra-passe inicial» = palavra-passe gerada.
- **Passos:**
  1. PC-01 com `admin@`; menu «Funcionários» → «Novo funcionário».
  2. Preencher os dados e carregar em «Guardar».
  3. Pesquisar `Funcionário {sufixo}` na lista.
  4. PC-02; PC-01 com o e-mail e a palavra-passe da conta criada.
- **Resultado esperado:**
  1. No passo 2 a aplicação volta à lista com a mensagem «Conta de Funcionário {sufixo} criada.».
  2. No passo 3 a conta aparece com perfil Rececionista e estado Ativo.
  3. No passo 4 a nova conta entra em `/agenda`, com o menu Agenda, Pacientes e Clientes (permissões do perfil atribuído).
- **Dados criados e removidos:** cria uma conta de funcionário com perfil Rececionista; fica até à reposição da base.

### ST-WEB-F001-08 — Recusar a criação de funcionário com dados inválidos

| Tipo     | Perfil        | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | Administrador | RF006, RNF004 | UC03 · W03 | CT-WEB-F001-05 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** (a) formulário vazio; (b) dados válidos, mas «Palavra-passe inicial» = `fraca123` (sem maiúscula nem símbolo); (c) dados válidos, mas «E-mail» = `rececao@clinica-g08.test` (já registado).
- **Passos:**
  1. PC-01 com `admin@`; menu «Funcionários» → «Novo funcionário».
  2. Carregar em «Guardar» com o formulário vazio.
  3. Preencher os dados (b) e carregar em «Guardar».
  4. Corrigir a palavra-passe, preencher os dados (c) e carregar em «Guardar».
  5. Voltar à lista e pesquisar `Funcionário {sufixo}`.
- **Resultado esperado:**
  1. No passo 2 aparecem as mensagens dos campos obrigatórios e nenhum pedido é enviado à API.
  2. Nos passos 3 e 4 a aplicação mostra junto ao formulário a mensagem da API (400 para a palavra-passe fraca; 409 para o e-mail repetido), continua no formulário com os restantes campos preenchidos e limpa a «Palavra-passe inicial».
  3. No passo 5 não existe nenhuma conta com o `{sufixo}`.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-09 — Atualizar uma conta de funcionário e a autorização de apoio

| Tipo     | Perfil        | Requisitos   | UC / ecrãs      | Condição                       | Automação |
| -------- | ------------- | ------------ | --------------- | ------------------------------ | --------- |
| Positivo | Administrador | RF007, RF008 | UC03 · W02, W03 | CT-WEB-F001-05, CT-WEB-F001-06 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** conta nova `Auxiliar {sufixo}`, perfil Auxiliar Veterinário, sem a autorização de apoio; novo telefone gerado.
- **Passos:**
  1. PC-01 com `admin@`; criar a conta como em ST-WEB-F001-07, com «Perfil» = Auxiliar Veterinário e sem marcar «Pode registar informação não clínica de apoio».
  2. Na lista, pesquisar a conta e confirmar «Registo de apoio» = «Não autorizado».
  3. Abrir «Ver / editar» da conta, alterar o telefone, marcar «Pode registar informação não clínica de apoio» e carregar em «Guardar».
  4. Voltar à lista e pesquisar a conta; recarregar a página (F5) e voltar a abrir a ficha.
  5. Na ficha, mudar «Perfil» para Veterinário e carregar em «Guardar»; voltar à lista.
- **Resultado esperado:**
  1. No passo 3 aparece «Alterações guardadas.».
  2. No passo 4 a lista mostra «Registo de apoio» = «Autorizado» e, depois de recarregar, a ficha mostra o novo telefone e a caixa marcada (RNF005).
  3. No passo 5, ao escolher Veterinário a caixa «Pode registar informação não clínica de apoio» deixa de aparecer; depois de guardar, a lista mostra o perfil Veterinário e «Registo de apoio» = «—».
- **Dados criados e removidos:** cria uma conta de funcionário (no fim com perfil Veterinário); fica até à reposição da base.

### ST-WEB-F001-10 — Mostrar a autorização de apoio só ao Auxiliar Veterinário

| Tipo     | Perfil        | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------ | ---------- | -------------- | --------- |
| Negativo | Administrador | RF008, RF007 | UC03 · W03 | CT-WEB-F001-06 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** nenhum (não se grava).
- **Passos:**
  1. PC-01 com `admin@`; menu «Funcionários» → «Novo funcionário».
  2. Escolher, em «Perfil», sucessivamente Administrador, Rececionista, Veterinário e Auxiliar Veterinário.
  3. Com Auxiliar Veterinário, marcar a caixa e voltar a escolher Rececionista.
- **Resultado esperado:** a caixa «Pode registar informação não clínica de apoio» só aparece com Auxiliar Veterinário; ao voltar a Rececionista desaparece. Se o formulário for gravado nesse estado, o pedido não leva a autorização (verificado pela automação no pedido EP-006).
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-11 — Bloquear uma conta de funcionário

| Tipo     | Perfil                                  | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | --------------------------------------- | ------------ | ---------- | -------------- | --------- |
| Positivo | Administrador; depois a conta bloqueada | RF009, RF002 | UC03 · W02 | CT-WEB-F001-07 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** conta nova `Bloquear {sufixo}`, perfil Rececionista; motivo `Teste {sufixo}`.
- **Passos:**
  1. PC-01 com `admin@`; criar a conta como em ST-WEB-F001-07.
  2. Na lista, pesquisar a conta e carregar em «Bloquear».
  3. Na janela «Confirmar bloqueio», escrever o motivo em «Motivo (opcional)» e confirmar.
  4. Abrir «Ver / editar» da conta bloqueada.
  5. PC-02; tentar PC-01 com a conta bloqueada.
- **Resultado esperado:**
  1. No passo 3 aparece «A conta de Bloquear {sufixo} foi bloqueada.» e a lista mostra o estado Bloqueado, sem «Bloquear» nessa linha.
  2. No passo 4 a ficha mostra o estado Bloqueado e «Motivo do bloqueio: Teste {sufixo}» (o motivo fica gravado).
  3. No passo 5 o início de sessão é recusado com «Esta conta está bloqueada. Contacte o administrador da clínica.».
- **Dados criados e removidos:** cria uma conta de funcionário bloqueada; fica até à reposição da base.

### ST-WEB-F001-12 — Recusar o bloqueio da própria conta do Administrador

| Tipo     | Perfil        | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ---------- | ---------- | -------------- | --------- |
| Negativo | Administrador | RF009      | UC03 · W02 | CT-WEB-F001-07 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** a conta `admin@`.
- **Passos:**
  1. PC-01 com `admin@`; menu «Funcionários»; pesquisar «admin@clinica-g08.test».
  2. Carregar em «Bloquear» na própria conta e confirmar.
  3. Recarregar a lista.
- **Resultado esperado:** no passo 2 a janela de confirmação mostra a mensagem da API (422) a indicar que o Administrador não pode bloquear a própria conta e não fecha como sucesso; no passo 3 a conta continua Ativa e a sessão mantém-se.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-13 — Registar um Cliente com conta de acesso (Rececionista)

| Tipo     | Perfil       | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | ------------ | ---------- | --------------- | -------------- | --------- |
| Positivo | Rececionista | RF010      | UC04 · W04, W05 | CT-WEB-F001-08 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** «Nome» = `Cliente {sufixo}`; «Telefone» e «E-mail» gerados; «Palavra-passe inicial» gerada; sem NIF nem morada.
- **Passos:**
  1. PC-01 com `rececao@`; menu «Clientes» → «Novo cliente».
  2. Confirmar que o formulário separa «Dados do cliente» de «Conta de acesso (aplicação móvel)».
  3. Preencher os dados e carregar em «Registar cliente».
  4. Voltar à lista e pesquisar `Cliente {sufixo}`.
- **Resultado esperado:**
  1. No passo 3 abre a «Ficha de cliente» com «Cliente Cliente {sufixo} registado, com conta de acesso para st.{sufixo}@clinica-g08.test.», «Estado da conta: Ativo», «Este cliente não tem animais.» e sem campo de palavra-passe.
  2. No passo 4 a lista mostra o Cliente com o telefone e o e-mail indicados e 0 animais.
- **Dados criados e removidos:** cria um Cliente e a respetiva conta de acesso; ficam até à reposição da base.

### ST-WEB-F001-14 — Registar um Cliente com NIF e morada (Administrador)

| Tipo     | Perfil        | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | ------------- | ---------- | --------------- | -------------- | --------- |
| Positivo | Administrador | RF010      | UC04 · W04, W05 | CT-WEB-F001-08 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** como em ST-WEB-F001-13, com «NIF (opcional)» = NIF válido gerado na execução (9 algarismos com dígito de controlo correto, começado por 2) e «Morada (opcional)» = `Rua {sufixo}, 1, 4470-000 Maia`.
- **Passos:**
  1. PC-01 com `admin@`; menu «Clientes» → «Novo cliente».
  2. Preencher os dados e carregar em «Registar cliente».
- **Resultado esperado:** abre a ficha com a mensagem de registo e com o NIF e a morada indicados; na lista de animais aparece «Este cliente não tem animais.», sem ligações «Ver paciente» (o Administrador não tem acesso a W08).
- **Dados criados e removidos:** cria um Cliente com conta de acesso; fica até à reposição da base.

### ST-WEB-F001-15 — Recusar o registo de um Cliente com dados inválidos

| Tipo     | Perfil       | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF010, RNF004 | UC04 · W05 | CT-WEB-F001-08 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** (a) formulário vazio; (b) NIF `123456788`; (c) telefone `12`; (d) palavra-passe inicial `fraca123` (sem maiúscula nem símbolo); (e) e-mail `cliente.joao@clinica-g08.test` (já registado), com os restantes dados válidos.
- **Passos:**
  1. PC-01 com `rececao@`; menu «Clientes» → «Novo cliente».
  2. Carregar em «Registar cliente» com o formulário vazio.
  3. Preencher os dados válidos com o NIF (b) e carregar em «Registar cliente»; corrigir o NIF (deixá-lo vazio).
  4. Repetir com o telefone (c); corrigir o telefone.
  5. Repetir com a palavra-passe (d); corrigir a palavra-passe.
  6. Repetir com o e-mail (e).
  7. Voltar à lista e pesquisar `Cliente {sufixo}`.
- **Resultado esperado:**
  1. Passo 2: «O nome é obrigatório.», «O telefone é obrigatório.», «O e-mail é obrigatório.» e «A palavra-passe inicial é obrigatória.», sem pedido à API.
  2. Passo 3: «O NIF é inválido: deve ter 9 dígitos e um dígito de controlo correto.», sem pedido à API.
  3. Passo 4: «O telefone deve ter entre 9 e 20 caracteres: dígitos, espaços e, opcionalmente, o indicativo + no início.», sem pedido à API.
  4. Passo 5: «A palavra-passe deve ter entre 8 e 128 caracteres, com pelo menos uma letra minúscula, uma letra maiúscula, um algarismo e um carácter de pontuação ou símbolo.», sem pedido à API.
  5. Passo 6: a mensagem da API (409) sobre o e-mail já registado aparece no formulário e a «Palavra-passe inicial» fica vazia.
  6. Passo 7: nenhum Cliente com o `{sufixo}`.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-16 — Atualizar os dados de um Cliente

| Tipo     | Perfil       | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------- | ---------- | -------------- | --------- |
| Positivo | Rececionista | RF011, RNF005 | UC04 · W05 | CT-WEB-F001-08 | I129      |

- **Pré-condições:** Cliente de teste criado com PC-03.
- **Dados:** novo telefone gerado; «Morada (opcional)» = `Rua {sufixo}, 2, 4470-000 Maia`.
- **Passos:**
  1. Na ficha do Cliente de teste, alterar o telefone e a morada e carregar em «Guardar alterações».
  2. Recarregar a página (F5).
  3. Voltar à lista e pesquisar o Cliente.
- **Resultado esperado:** no passo 1 aparece «Alterações guardadas.»; depois de recarregar, a ficha mostra o novo telefone e a nova morada; na lista aparece o novo telefone. A secção «Conta de acesso (aplicação móvel)» não pede palavra-passe.
- **Dados criados e removidos:** o Cliente de PC-03; fica até à reposição da base.

### ST-WEB-F001-17 — Recusar a atualização de um Cliente com dados inválidos

| Tipo     | Perfil        | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | Administrador | RF011, RNF004 | UC04 · W05 | CT-WEB-F001-08 | I129      |

- **Pré-condições:** Cliente de teste criado com PC-03; sessão do Administrador (PC-01 com `admin@`).
- **Dados:** (a) «Nome» vazio; (b) e-mail `cliente.maria@clinica-g08.test` (já registado).
- **Passos:**
  1. Abrir a ficha do Cliente de teste, apagar o nome e carregar em «Guardar alterações».
  2. Repor o nome, escrever o e-mail (b) e carregar em «Guardar alterações».
  3. Recarregar a página.
- **Resultado esperado:** no passo 1 aparece «O nome é obrigatório.» sem pedido à API; no passo 2 aparece a mensagem da API (409) no formulário; no passo 3 a ficha mostra o nome e o e-mail originais.
- **Dados criados e removidos:** o Cliente de PC-03; fica até à reposição da base.

### ST-WEB-F001-18 — Pesquisar Clientes

| Tipo     | Perfil       | Requisitos   | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------ | ---------- | -------------- | --------- |
| Positivo | Rececionista | RF011, RF017 | UC04 · W04 | CT-WEB-F001-08 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** pesquisas «joão», «960000002» e «zzz{sufixo}».
- **Passos:**
  1. PC-01 com `rececao@`; menu «Clientes».
  2. Pesquisar cada um dos textos em «Pesquisar (nome, NIF, telefone ou e-mail)».
- **Resultado esperado:** «joão» mostra só «João Costa (demonstração)», com 2 animais; «960000002» mostra só «Maria Silva (demonstração)»; o terceiro texto mostra «Nenhum cliente corresponde à pesquisa.».
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-19 — Consultar os dados gerais e as configurações da clínica

| Tipo     | Perfil        | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ---------- | ---------- | -------------- | --------- |
| Positivo | Administrador | RF012      | UC05 · W06 | CT-WEB-F001-09 | I129      |

- **Pré-condições:** sem sessão iniciada; configuração da clínica igual à dos dados de demonstração.
- **Dados:** dados de base (secção 5.3).
- **Passos:**
  1. PC-01 com `admin@`; menu «Clínica e horários».
- **Resultado esperado:** depois de «A carregar…», o ecrã mostra em «Dados gerais» o «Nome da clínica» «Clínica Veterinária G08», o «NIF» 501234560, a morada, o telefone, o e-mail e a «Duração da consulta (minutos)» 30; em «Horário de funcionamento», segunda a sexta com 09:00–13:00 e 14:00–19:00, sábado com 09:00–13:00 e domingo «Encerrado».
- **Dados criados e removidos:** nenhum.

### ST-WEB-F001-20 — Atualizar os dados gerais e as configurações da clínica (exclusivo)

| Tipo     | Perfil        | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------- | ---------- | -------------- | --------- |
| Positivo | Administrador | RF013, RNF005 | UC05 · W06 | CT-WEB-F001-09 | I129      |

- **Pré-condições:** sem sessão iniciada; configuração da clínica igual à dos dados de demonstração.
- **Dados:** «Telefone» = telefone gerado; «Duração da consulta (minutos)» = 45.
- **Passos:**
  1. PC-01 com `admin@`; menu «Clínica e horários»; guardar os valores originais do telefone e da duração.
  2. Alterar o telefone e a duração e carregar em «Guardar».
  3. Recarregar a página (F5).
  4. **Limpeza (executada sempre):** repor o telefone e a duração originais e carregar em «Guardar».
- **Resultado esperado:** no passo 2 aparece «Alterações guardadas.»; no passo 3 o ecrã mostra o novo telefone e 45; no passo 4 voltam os valores originais com «Alterações guardadas.».
- **Dados criados e removidos:** altera a configuração da clínica e repõe-na no passo 4.

### ST-WEB-F001-21 — Recusar dados gerais inválidos da clínica (exclusivo)

| Tipo     | Perfil        | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | Administrador | RF013, RNF004 | UC05 · W06 | CT-WEB-F001-09 | I129      |

- **Pré-condições:** sem sessão iniciada; configuração da clínica igual à dos dados de demonstração.
- **Dados:** (a) «Nome da clínica» vazio; (b) duração 3; (c) «NIF» `123456788`.
- **Passos:**
  1. PC-01 com `admin@`; menu «Clínica e horários».
  2. Apagar o nome e carregar em «Guardar»; carregar em «Cancelar».
  3. Escrever 3 na duração e carregar em «Guardar»; carregar em «Cancelar».
  4. Escrever o NIF (c) e carregar em «Guardar».
  5. Carregar em «Cancelar» e recarregar a página.
- **Resultado esperado:**
  1. Passo 2: «Corrija os seguintes pontos antes de guardar:» com «Indique o nome da clínica.», sem pedido à API.
  2. Passo 3: «A duração da consulta deve ser um número inteiro de minutos entre 5 e 240.», sem pedido à API.
  3. Passo 4: o pedido é enviado e a mensagem da API (400) sobre o NIF aparece no ecrã, com os dados escritos mantidos no formulário.
  4. Passo 5: o ecrã mostra os valores originais (nada foi gravado).
- **Dados criados e removidos:** nenhum.

## 8. Cenários — F002 Pacientes

### ST-WEB-F002-01 — Registar um paciente associado a um Cliente (Rececionista)

| Tipo     | Perfil       | Requisitos    | UC / ecrãs      | Condição       | Automação |
| -------- | ------------ | ------------- | --------------- | -------------- | --------- |
| Positivo | Rececionista | RF014, RNF005 | UC06 · W07, W08 | CT-WEB-F002-01 | I129      |

- **Pré-condições:** Cliente de teste criado com PC-03 (sessão da Rececionista aberta).
- **Dados:** os de PC-04.
- **Passos:**
  1. Executar PC-04.
  2. Recarregar a ficha (F5).
- **Resultado esperado:** no passo 1 abre a «Ficha do paciente» com a mensagem «Paciente Animal {sufixo} registado para Cliente {sufixo}.», a espécie Cão, a raça Labrador Retriever, o sexo, a data de nascimento, o peso 10,5 kg, a idade calculada pela API e o proprietário `Cliente {sufixo}` com o telefone; no passo 2 os mesmos dados continuam visíveis.
- **Dados criados e removidos:** cria um Cliente e um paciente; ficam até à reposição da base.

### ST-WEB-F002-02 — Registar um paciente a partir da pesquisa por proprietário (Veterinário)

| Tipo     | Perfil      | Requisitos   | UC / ecrãs      | Condição       | Automação |
| -------- | ----------- | ------------ | --------------- | -------------- | --------- |
| Positivo | Veterinário | RF014, RF017 | UC06 · W07, W08 | CT-WEB-F002-01 | I129      |

- **Pré-condições:** Cliente de teste criado com PC-03; PC-02.
- **Dados:** «Nome do animal» = `Gato {sufixo}`; «Espécie» = Gato; «Raça (opcional)» = Siamês; «Sexo» = Femea; sem data de nascimento nem peso.
- **Passos:**
  1. PC-01 com `vet.ana@`; menu «Pacientes».
  2. Em «Proprietário (nome, NIF, telefone ou e-mail)» escrever `Cliente {sufixo}`, carregar em «Procurar proprietário» e em «Selecionar Cliente {sufixo}».
  3. Carregar em «Novo paciente»; confirmar que o proprietário já vem escolhido; preencher os dados e carregar em «Guardar dados administrativos».
  4. Voltar à lista: a pesquisa por proprietário mostra o novo paciente.
- **Resultado esperado:** no passo 3 a ficha abre com «Paciente Gato {sufixo} registado para Cliente {sufixo}.», sem data de nascimento nem peso e com a «Área clínica» («Registo clínico» e «Vacinação») visível para o Veterinário; no passo 4 a lista filtrada pelo Cliente mostra só esse paciente.
- **Dados criados e removidos:** cria um Cliente e um paciente; ficam até à reposição da base.

### ST-WEB-F002-03 — Recusar o registo de um paciente com dados inválidos

| Tipo     | Perfil       | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF014, RNF004 | UC06 · W08 | CT-WEB-F002-01 | I129      |

- **Pré-condições:** Cliente de teste criado com PC-03.
- **Dados:** (a) formulário vazio; (b) data de nascimento = amanhã; (c) peso = 0; (d) espécie `Coelho`; (e) espécie Cão com raça `Poodle`.
- **Passos:**
  1. Menu «Pacientes» → «Novo paciente»; carregar em «Guardar dados administrativos» com o formulário vazio.
  2. Escolher o proprietário de teste, escrever `Animal {sufixo}` e Cão, e repetir sucessivamente com (b) e (c), corrigindo cada campo antes do seguinte.
  3. Repetir com (d) e depois com (e).
  4. Pesquisar `Animal {sufixo}` na lista de pacientes.
- **Resultado esperado:**
  1. Passo 1: «Escolha o proprietário (Cliente) do paciente.», «O nome é obrigatório.» e «A espécie é obrigatória.», sem pedido à API.
  2. Passo 2: «A data de nascimento não pode ser futura.» e «O peso deve estar entre 0,01 e 9999,99 kg.», sem pedido à API.
  3. Passo 3: o pedido é enviado e a API responde 400; o formulário mostra «A espécie indicada não existe.» e depois «A raça indicada não existe para a espécie selecionada.», com os campos editáveis e os dados escritos mantidos.
  4. Passo 4: «Nenhum paciente corresponde à pesquisa.».
- **Dados criados e removidos:** cria um Cliente (PC-03); nenhum paciente.

### ST-WEB-F002-04 — Consultar a ficha administrativa do paciente

| Tipo     | Perfil                    | Requisitos    | UC / ecrãs      | Condição       | Automação |
| -------- | ------------------------- | ------------- | --------------- | -------------- | --------- |
| Positivo | Rececionista, Veterinário | RF015, RNF005 | UC07 · W07, W08 | CT-WEB-F002-02 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** paciente Bobi (dados de base).
- **Passos:**
  1. PC-01 com `rececao@`; menu «Pacientes»; pesquisar «Bobi» e carregar em «Ver ficha».
  2. Recarregar a página.
  3. PC-02; PC-01 com `vet.ana@`; abrir a mesma ficha.
- **Resultado esperado:**
  1. Nos passos 1 e 2 a ficha mostra Bobi, Cão, Labrador Retriever, Macho, data de nascimento 10/03/2022, peso 28,4 kg, a idade e o proprietário «João Costa (demonstração) · 960000001», com «Ver cliente» e «Nova marcação»; não mostra a «Área clínica» nem ligações clínicas (critério 4 da I112).
  2. No passo 3 a mesma ficha mostra a «Área clínica» com «Registo clínico» e «Vacinação» e não mostra «Nova marcação».
- **Dados criados e removidos:** nenhum.

### ST-WEB-F002-05 — Abrir a ficha de um paciente inexistente

| Tipo     | Perfil       | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ---------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF015      | UC07 · W08 | CT-WEB-F002-02 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** `/pacientes/999999` e `/pacientes/abc`.
- **Passos:**
  1. PC-01 com `rececao@`; abrir `/pacientes/999999`.
  2. Abrir `/pacientes/abc`.
- **Resultado esperado:** no passo 1 a API responde 404 e aparece a mensagem do `detail` da API, «O animal indicado não existe.»; no passo 2 aparece «O registo pedido não foi encontrado.» sem nenhum pedido à API (endereço inválido). Nos dois casos aparece «Voltar à lista de pacientes», sem dados de outro paciente.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F002-06 — Atualizar os dados administrativos do paciente

| Tipo     | Perfil      | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ----------- | ------------- | ---------- | -------------- | --------- |
| Positivo | Veterinário | RF016, RNF005 | UC07 · W08 | CT-WEB-F002-03 | I129      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; PC-02.
- **Dados:** «Peso em kg (opcional)» = 12,3; «Raça (opcional)» = Pastor Alemão.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir a ficha de `Animal {sufixo}`.
  2. Alterar o peso e a raça e carregar em «Guardar dados administrativos».
  3. Recarregar a página; voltar à lista de pacientes e pesquisar `Animal {sufixo}`.
- **Resultado esperado:** no passo 2 aparece «Alterações guardadas.» e a «Última atualização» fica preenchida; no passo 3 a ficha e a lista mostram 12,3 kg e Pastor Alemão.
- **Dados criados e removidos:** cria um Cliente e um paciente; ficam até à reposição da base.

### ST-WEB-F002-07 — Recusar a atualização do paciente com dados inválidos

| Tipo     | Perfil       | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF016, RNF004 | UC07 · W08 | CT-WEB-F002-03 | I129      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04.
- **Dados:** (a) nome vazio; (b) peso 10000.
- **Passos:**
  1. Na ficha de `Animal {sufixo}`, apagar o nome e carregar em «Guardar dados administrativos».
  2. Repor o nome, escrever o peso (b) e carregar de novo.
  3. Recarregar a página.
- **Resultado esperado:** no passo 1 «O nome é obrigatório.» e no passo 2 «O peso deve estar entre 0,01 e 9999,99 kg.», sem pedidos à API; no passo 3 a ficha mostra os valores registados em PC-04 (nada foi gravado).
- **Dados criados e removidos:** cria um Cliente e um paciente; ficam até à reposição da base.

### ST-WEB-F002-08 — Pesquisar os pacientes de um proprietário

| Tipo     | Perfil       | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ---------- | ---------- | -------------- | --------- |
| Positivo | Rececionista | RF017      | UC07 · W07 | CT-WEB-F002-04 | I129      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** proprietário João Costa (demonstração).
- **Passos:**
  1. PC-01 com `rececao@`; menu «Pacientes».
  2. Em «Proprietário (nome, NIF, telefone ou e-mail)» escrever «João», carregar em «Procurar proprietário» e em «Selecionar João Costa (demonstração)».
  3. Carregar em «Remover filtro».
- **Resultado esperado:** no passo 2 a lista mostra só Bobi e Mia («2 pacientes»), com espécie, raça, idade, peso e proprietário; no passo 3 voltam a aparecer os pacientes de todos os Clientes, incluindo Kiko e Luna.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F002-09 — Pesquisa por proprietário sem resultados

| Tipo     | Perfil      | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ----------- | ---------- | ---------- | -------------- | --------- |
| Negativo | Veterinário | RF017      | UC07 · W07 | CT-WEB-F002-04 | I129      |

- **Pré-condições:** Cliente de teste criado com PC-03 (sem animais); PC-02.
- **Dados:** pesquisa de proprietário «zzz{sufixo}»; proprietário `Cliente {sufixo}`.
- **Passos:**
  1. PC-01 com `vet.ana@`; menu «Pacientes».
  2. Procurar o proprietário «zzz{sufixo}».
  3. Procurar `Cliente {sufixo}` e selecioná-lo.
- **Resultado esperado:** no passo 2 aparece «Nenhum cliente encontrado.» e a lista não é filtrada; no passo 3 a lista fica vazia, com «Nenhum paciente corresponde à pesquisa.» (o Cliente não tem animais e não aparecem pacientes de outros Clientes).
- **Dados criados e removidos:** cria um Cliente; fica até à reposição da base.

### ST-WEB-F002-10 — Ficha do paciente só em leitura para o Auxiliar Veterinário

| Tipo     | Perfil               | Requisitos           | UC / ecrãs | Condição      | Automação |
| -------- | -------------------- | -------------------- | ---------- | ------------- | --------- |
| Negativo | Auxiliar Veterinário | RF005, RF015, RNF001 | UC07 · W08 | CT-WEB-TRV-01 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** paciente Bobi; rotas `/pacientes` e `/pacientes/novo`.
- **Passos:**
  1. PC-01 com `aux.carla@`; abrir a ficha do Bobi pelo endereço `/pacientes/{id do Bobi}`.
  2. Abrir `/pacientes` e `/pacientes/novo`.
- **Resultado esperado:** no passo 1 a ficha mostra os dados administrativos em texto, sem campos editáveis nem «Guardar dados administrativos», com «Registo clínico» e sem «Vacinação» nem «Nova marcação»; no passo 2 aparece «Acesso negado» nos dois endereços.
- **Dados criados e removidos:** nenhum.

## 9. Cenários — F003 Agendamento e consultas

### ST-WEB-F003-01 — Gerir o horário de funcionamento (exclusivo)

| Tipo     | Perfil        | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------- | ---------- | -------------- | --------- |
| Positivo | Administrador | RF019, RNF005 | UC09 · W06 | CT-WEB-F003-01 | I129      |

- **Pré-condições:** sem sessão iniciada; horário igual ao dos dados de demonstração.
- **Dados:** segunda-feira com o 1.º período 09:00–14:00 (seguido do 2.º, 14:00–19:00); sábado sem períodos; domingo com 10:00–12:00.
- **Passos:**
  1. PC-01 com `admin@`; menu «Clínica e horários».
  2. Mudar «Segunda, período 1 — fecho» para 14:00.
  3. Carregar em «Remover Sábado, período 1».
  4. Carregar em «Adicionar período a Domingo» e preencher 10:00 e 12:00.
  5. Carregar em «Guardar»; recarregar a página.
  6. **Limpeza (executada sempre):** repor segunda 09:00–13:00, sábado 09:00–13:00 e domingo encerrado; carregar em «Guardar».
- **Resultado esperado:** no passo 5 aparece «Alterações guardadas.»; depois de recarregar, segunda mostra 09:00–14:00 e 14:00–19:00 (períodos seguidos aceites), sábado «Encerrado» e domingo 10:00–12:00; no passo 6 o horário volta ao original com «Alterações guardadas.».
- **Dados criados e removidos:** altera o horário da clínica e repõe-no no passo 6.

### ST-WEB-F003-02 — Recusar períodos sobrepostos ou com fecho não posterior à abertura (exclusivo)

| Tipo     | Perfil        | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | Administrador | RF019, RNF004 | UC09 · W06 | CT-WEB-F003-01 | I129      |

- **Pré-condições:** sem sessão iniciada; horário igual ao dos dados de demonstração.
- **Dados:** (a) período 12:00–15:00 acrescentado à segunda-feira; (b) terça, período 2, com fecho 14:00 (igual à abertura); (c) período acrescentado à quinta-feira sem horas.
- **Passos:**
  1. PC-01 com `admin@`; menu «Clínica e horários».
  2. Acrescentar o período (a) e carregar em «Guardar».
  3. Carregar em «Cancelar»; alterar (b) e carregar em «Guardar».
  4. Carregar em «Cancelar»; acrescentar (c) e carregar em «Guardar».
  5. Carregar em «Cancelar» e recarregar a página.
- **Resultado esperado:**
  1. Passo 2: «Corrija os seguintes pontos antes de guardar:» com «Segunda: os períodos 09:00–13:00 e 12:00–15:00 sobrepõem-se.» e «Segunda: os períodos 12:00–15:00 e 14:00–19:00 sobrepõem-se.».
  2. Passo 3: «Terça, período 2: o fecho tem de ser posterior à abertura.».
  3. Passo 4: «Quinta, período 3: indique a hora de abertura e a de fecho.».
  4. Em nenhum dos passos é enviado o pedido de gravação (EP-010); no passo 5 o horário é o original.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F003-03 — Consultar a disponibilidade de um Veterinário

| Tipo     | Perfil       | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ---------- | ---------- | -------------- | --------- |
| Positivo | Rececionista | RF020      | UC09 · W10 | CT-WEB-F003-02 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; horário igual ao dos dados de demonstração.
- **Dados:** Veterinário «Dra. Ana Veterinária (demonstração)»; `{D}` com desvio 3.
- **Passos:**
  1. Menu «Agenda» → «Nova marcação»; escolher o paciente de teste (PC-05, passo 2).
  2. Escolher a Dra. Ana em «Veterinário», escrever `{D}` em «A partir de» e carregar em «Consultar disponibilidade».
- **Resultado esperado:** em «Horários livres» aparecem, para `{D}`, horários entre 09:00 e 12:30 e entre 14:00 e 18:30, com intervalos de 30 minutos (duração da consulta); não aparecem horários às 13:00–13:30 nem a partir das 19:00; os domingos do intervalo não têm horários.
- **Dados criados e removidos:** cria um Cliente e um paciente; ficam até à reposição da base.

### ST-WEB-F003-04 — A disponibilidade exclui as marcações não canceladas

| Tipo     | Perfil       | Requisitos   | UC / ecrãs      | Condição       | Automação |
| -------- | ------------ | ------------ | --------------- | -------------- | --------- |
| Negativo | Rececionista | RF020, RF023 | UC09 · W10, W11 | CT-WEB-F003-02 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04.
- **Dados:** Veterinário Dr. Bruno; `{D}` com desvio 4; horário 10:00 de `{D}`.
- **Passos:**
  1. PC-05 com o Dr. Bruno e o horário 10:00 de `{D}`.
  2. Abrir «Nova marcação», escolher o paciente de teste e o Dr. Bruno e consultar a disponibilidade a partir de `{D}`.
  3. Abrir o detalhe da marcação do passo 1, carregar em «Cancelar marcação» e confirmar.
  4. Repetir o passo 2.
- **Resultado esperado:** no passo 2 o horário 10:00 de `{D}` não aparece em «Horários livres»; no passo 3 aparece «Estado alterado para Cancelada.»; no passo 4 o horário 10:00 de `{D}` volta a aparecer.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação cancelada; ficam até à reposição da base.

### ST-WEB-F003-05 — Criar uma marcação

| Tipo     | Perfil       | Requisitos           | UC / ecrãs           | Condição       | Automação |
| -------- | ------------ | -------------------- | -------------------- | -------------- | --------- |
| Positivo | Rececionista | RF021, RF024, RNF005 | UC09 · W09, W10, W11 | CT-WEB-F003-03 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04.
- **Dados:** Dra. Ana; `{D}` com desvio 5; primeiro horário livre.
- **Passos:**
  1. PC-05 com a Dra. Ana.
  2. Menu «Agenda»; mudar o dia para `{D}` com «Dia seguinte» ou no campo «Dia».
- **Resultado esperado:** no passo 1 o detalhe mostra «Marcação criada no estado Pendente.», o paciente `Animal {sufixo} · Cão`, a Dra. Ana, a data e hora escolhidas, o motivo e o estado Pendente; no passo 2 a agenda de `{D}` tem uma linha com a hora, o paciente, o Cliente, a Dra. Ana, o motivo e o estado Pendente.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação; ficam até à reposição da base.

### ST-WEB-F003-06 — Recusar uma marcação incompleta ou numa vaga ocupada

| Tipo     | Perfil       | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF021, RNF004 | UC09 · W10 | CT-WEB-F003-03 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; uma segunda janela anónima do navegador com sessão da Rececionista.
- **Dados:** Dr. Bruno; `{D}` com desvio 6; primeiro horário livre de `{D}`.
- **Passos:**
  1. Na primeira janela, abrir «Nova marcação» e carregar em «Criar marcação» sem preencher nada.
  2. Escolher o paciente de teste e o Dr. Bruno, consultar a disponibilidade a partir de `{D}` e escolher o primeiro horário livre, sem criar.
  3. Na segunda janela, criar com PC-05 uma marcação do mesmo paciente no mesmo horário.
  4. Na primeira janela, carregar em «Criar marcação».
- **Resultado esperado:** no passo 1 aparecem «Escolha o paciente.», «Escolha o Veterinário.» e «Consulte a disponibilidade e escolha um dos horários livres.», sem pedido à API; no passo 4 a API recusa o horário e aparecem «O veterinário já tem uma marcação que se sobrepõe a esse horário.» e «A disponibilidade foi atualizada. Escolha outro horário livre.», com o horário ocupado retirado da lista; existe uma única marcação nesse horário (a do passo 3).
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação; ficam até à reposição da base.

### ST-WEB-F003-07 — Alterar uma marcação

| Tipo     | Perfil       | Requisitos    | UC / ecrãs      | Condição       | Automação |
| -------- | ------------ | ------------- | --------------- | -------------- | --------- |
| Positivo | Rececionista | RF022, RNF005 | UC09 · W10, W11 | CT-WEB-F003-04 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dra. Ana, `{D}` com desvio 7).
- **Dados:** novo motivo `Motivo alterado {sufixo}`; novo horário = segundo horário livre de `{D}`.
- **Passos:**
  1. No detalhe da marcação, carregar em «Editar marcação».
  2. Alterar só o motivo e carregar em «Guardar alterações».
  3. Carregar de novo em «Editar marcação», consultar a disponibilidade, escolher o novo horário e carregar em «Guardar alterações».
  4. Recarregar o detalhe.
- **Resultado esperado:** no passo 2 o horário atual aparece com «(horário atual)» e mantém-se, e aparece «Marcação alterada.» com o novo motivo; no passo 3 aparece «Marcação alterada.» com o novo horário; no passo 4 o detalhe e a agenda de `{D}` mostram o novo motivo e o novo horário, com o estado Pendente inalterado.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação; ficam até à reposição da base.

### ST-WEB-F003-08 — Recusar a alteração concorrente ou de uma marcação final

| Tipo     | Perfil       | Requisitos   | UC / ecrãs      | Condição                      | Automação |
| -------- | ------------ | ------------ | --------------- | ----------------------------- | --------- |
| Negativo | Rececionista | RF022, RF023 | UC09 · W10, W11 | CT-WEB-F003-04, CT-WEB-TRV-04 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dr. Bruno, `{D}` com desvio 8); uma segunda janela anónima com sessão da Rececionista.
- **Dados:** motivos `Janela A {sufixo}` e `Janela B {sufixo}`.
- **Passos:**
  1. Nas duas janelas, abrir «Editar marcação» da mesma marcação.
  2. Na janela B, mudar o motivo para `Janela B {sufixo}` e guardar.
  3. Na janela A, mudar o motivo para `Janela A {sufixo}` e guardar.
  4. Na janela B, abrir o detalhe, carregar em «Cancelar marcação» e confirmar.
  5. Abrir `/agenda/{id}/editar` da marcação cancelada.
- **Resultado esperado:**
  1. No passo 3 a API responde 412 e a aplicação mostra «A marcação foi alterada por outra pessoa. Os dados foram atualizados: reveja-os e guarde de novo, se ainda fizer sentido.», com o motivo `Janela B {sufixo}` no formulário (a alteração da janela A não é gravada).
  2. No passo 4 a marcação fica Cancelada, sem «Editar marcação» nem ações de estado.
  3. No passo 5 aparece «Esta marcação está Cancelada e já não pode ser alterada.» e não é possível guardar.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação cancelada; ficam até à reposição da base.

### ST-WEB-F003-09 — Atualizar o estado da marcação segundo as transições permitidas

| Tipo     | Perfil       | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ------------- | ---------- | -------------- | --------- |
| Positivo | Rececionista | RF023, RNF005 | UC09 · W11 | CT-WEB-F003-05 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dra. Ana, `{D}` com desvio 9).
- **Dados:** observação `Confirmada por telefone {sufixo}`.
- **Passos:**
  1. No detalhe da marcação (Pendente), confirmar que as ações são só «Confirmar marcação» e «Cancelar marcação».
  2. Carregar em «Confirmar marcação», escrever a observação em «Observação (opcional)» e confirmar.
  3. Carregar em «Marcar como realizada» e confirmar.
  4. Recarregar a página.
- **Resultado esperado:** no passo 2 a janela «Confirmação» indica a mudança de Pendente para Confirmada e aparece «Estado alterado para Confirmada.», com «Observação do estado» = `Confirmada por telefone {sufixo}`; no passo 3 aparece «Estado alterado para Realizada.»; no passo 4 o estado é Realizada, sem ações de estado nem «Editar marcação» (estado final).
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação realizada; ficam até à reposição da base.

### ST-WEB-F003-10 — Não oferecer transições de estado não previstas

| Tipo     | Perfil       | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ---------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF023      | UC09 · W11 | CT-WEB-F003-05 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dr. Bruno, `{D}` com desvio 10).
- **Dados:** motivo de cancelamento `Pedido do tutor {sufixo}`.
- **Passos:**
  1. No detalhe da marcação (Pendente), verificar as ações oferecidas.
  2. Carregar em «Cancelar marcação», escrever o motivo em «Motivo do cancelamento (opcional)» e confirmar.
  3. Recarregar a página.
- **Resultado esperado:** no passo 1 não aparece «Marcar como realizada» (Pendente só passa a Confirmada ou Cancelada); no passo 2 aparece «Estado alterado para Cancelada.»; no passo 3 o estado é Cancelada, com «Observação do estado» = `Pedido do tutor {sufixo}`, sem ações de estado, sem «Editar marcação» e sem «Registar chegada».
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação cancelada; ficam até à reposição da base.

### ST-WEB-F003-11 — Consultar a agenda da clínica (Rececionista)

| Tipo     | Perfil       | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ---------- | ---------- | -------------- | --------- |
| Positivo | Rececionista | RF024      | UC09 · W09 | CT-WEB-F003-06 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; duas marcações criadas com PC-05 em `{D}` com desvio 11: uma com a Dra. Ana e outra com o Dr. Bruno; a da Dra. Ana confirmada com PC-06.
- **Dados:** filtros «Veterinário» e «Estado».
- **Passos:**
  1. Menu «Agenda»; ir para o dia `{D}`.
  2. Escolher a Dra. Ana em «Veterinário».
  3. Escolher «Todos» em «Veterinário» e Pendente em «Estado».
  4. Ir para o dia seguinte a `{D}` com «Dia seguinte» e voltar com «Dia anterior».
- **Resultado esperado:** no passo 1 aparecem as duas marcações (hora, paciente, Cliente, Veterinário, motivo, estado e chegada) e o botão «Nova marcação»; no passo 2 só a da Dra. Ana; no passo 3 só a do Dr. Bruno; no passo 4 a navegação muda o dia e, ao voltar, as marcações de `{D}` reaparecem com os filtros aplicados. Se um filtro não tiver resultados aparece «Não há marcações neste dia com estes filtros.».
- **Dados criados e removidos:** cria um Cliente, um paciente e duas marcações; ficam até à reposição da base.

### ST-WEB-F003-12 — O Veterinário vê só as consultas que lhe estão atribuídas

| Tipo     | Perfil      | Requisitos    | UC / ecrãs      | Condição       | Automação |
| -------- | ----------- | ------------- | --------------- | -------------- | --------- |
| Negativo | Veterinário | RF024, RNF001 | UC10 · W09, W11 | CT-WEB-F003-06 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; duas marcações criadas com PC-05 em `{D}` com desvio 12, uma com a Dra. Ana e outra com o Dr. Bruno; PC-02.
- **Dados:** conta `vet.ana@`.
- **Passos:**
  1. PC-01 com `vet.ana@`; menu «Agenda»; ir para o dia `{D}`.
  2. Abrir o detalhe da sua marcação.
  3. Abrir `/agenda/nova`.
- **Resultado esperado:** no passo 1 aparece «Mostra só as consultas que lhe estão atribuídas.», sem filtro «Veterinário» nem «Nova marcação», e só a marcação da Dra. Ana (a do Dr. Bruno não aparece); no passo 2 o detalhe abre sem a secção «Ações» e com «Abrir ficha do paciente»; no passo 3 aparece «Acesso negado».
- **Dados criados e removidos:** cria um Cliente, um paciente e duas marcações; ficam até à reposição da base.

### ST-WEB-F003-13 — Registar a chegada à consulta

| Tipo     | Perfil       | Requisitos    | UC / ecrãs      | Condição       | Automação |
| -------- | ------------ | ------------- | --------------- | -------------- | --------- |
| Positivo | Rececionista | RF025, RNF005 | UC09 · W09, W11 | CT-WEB-F003-07 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dra. Ana, `{D}` com desvio 13) e confirmada com PC-06.
- **Dados:** nenhum além da marcação.
- **Passos:**
  1. No detalhe da marcação (Confirmada), carregar em «Registar chegada».
  2. Recarregar o detalhe; abrir a agenda do dia `{D}`.
- **Resultado esperado:** no passo 1 aparece «Chegada registada às HH:MM. O estado da marcação não muda.», com HH:MM igual à hora atual de Lisboa; o estado continua Confirmada e o botão «Registar chegada» desaparece; no passo 2 o detalhe mostra a hora de chegada e a coluna de chegada da agenda mostra a mesma hora.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação com chegada; ficam até à reposição da base.

### ST-WEB-F003-14 — Não registar a chegada fora de uma marcação Confirmada nem duas vezes

| Tipo     | Perfil       | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------ | ---------- | ---------- | -------------- | --------- |
| Negativo | Rececionista | RF025      | UC09 · W11 | CT-WEB-F003-07 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; marcação criada com PC-05 (Dr. Bruno, `{D}` com desvio 14), ainda Pendente; uma segunda janela anónima com sessão da Rececionista.
- **Dados:** nenhum.
- **Passos:**
  1. No detalhe da marcação Pendente, verificar se existe «Registar chegada».
  2. Confirmar a marcação com PC-06.
  3. Abrir o detalhe nas duas janelas; registar a chegada na janela B; depois carregar em «Registar chegada» na janela A.
- **Resultado esperado:** no passo 1 não existe «Registar chegada»; no passo 3 a janela A mostra o erro da API («A chegada desta marcação já foi registada.» ou, se o pedido levar o `ETag` antigo, o aviso de alteração concorrente) e recarrega a marcação com a hora de chegada registada na janela B, que não muda.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação com chegada; ficam até à reposição da base.

### ST-WEB-F003-15 — Consultar a informação de preparação da consulta

| Tipo     | Perfil               | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | -------------------- | ---------- | ---------- | -------------- | --------- |
| Positivo | Auxiliar Veterinário | RF026      | UC10 · W12 | CT-WEB-F003-08 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dra. Ana, `{D}` com desvio 15) e confirmada com PC-06; PC-02.
- **Dados:** conta `aux.carla@`.
- **Passos:**
  1. PC-01 com `aux.carla@` (abre «Preparação de consultas»).
  2. Ir para o dia `{D}` e carregar em «Selecionar a consulta de Animal {sufixo} às HH:MM».
- **Resultado esperado:** aparece «Consulta selecionada» e a «Informação de preparação» com a data, a hora, a duração, o estado, a Dra. Ana, o motivo, a chegada («Não registada»), o paciente (espécie, raça, sexo, idade e peso, data de nascimento) e o proprietário com o telefone; não aparece informação clínica (diagnóstico, notas clínicas, tratamentos ou prescrições) nem ações de agenda («Registar chegada», «Editar marcação», mudanças de estado).
- **Dados criados e removidos:** cria um Cliente, um paciente e uma marcação; ficam até à reposição da base.

### ST-WEB-F003-16 — Registar informação não clínica de apoio com autorização

| Tipo     | Perfil                                             | Requisitos    | UC / ecrãs      | Condição       | Automação |
| -------- | -------------------------------------------------- | ------------- | --------------- | -------------- | --------- |
| Positivo | Auxiliar Veterinário com autorização; Rececionista | RF027, RNF005 | UC10 · W11, W12 | CT-WEB-F003-08 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dr. Bruno, `{D}` com desvio 16) e confirmada com PC-06; PC-02.
- **Dados:** nota `Sala preparada {sufixo}`.
- **Passos:**
  1. PC-01 com `aux.carla@`; ir para o dia `{D}` e selecionar a consulta de teste.
  2. Carregar em «Guardar informação de apoio» com o campo «Nova nota de apoio (não clínica)» vazio.
  3. Escrever a nota e carregar em «Guardar informação de apoio».
  4. PC-02; PC-01 com `rececao@`; abrir o detalhe da marcação.
- **Resultado esperado:** no passo 2 aparece «Escreva a informação de apoio.», sem pedido à API; no passo 3 aparece «Informação de apoio guardada.» e a nota aparece em «Notas de apoio» com o texto, a autora (Carla Auxiliar) e a data e hora; no passo 4 o detalhe mostra a nota em «Notas de preparação» e o estado continua Confirmada (a nota não altera a marcação).
- **Dados criados e removidos:** cria um Cliente, um paciente, uma marcação e uma nota de apoio; ficam até à reposição da base.

### ST-WEB-F003-17 — Recusar a nota de apoio sem autorização ou numa marcação cancelada

| Tipo     | Perfil                                                                     | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | -------------------------------------------------------------------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | Auxiliar Veterinário sem autorização; Auxiliar Veterinário com autorização | RF027, RNF001 | UC10 · W12 | CT-WEB-F003-08 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; duas marcações criadas com PC-05 em `{D}` com desvio 17: a primeira confirmada com PC-06 e a segunda cancelada; PC-02.
- **Dados:** nota `Nota {sufixo}`.
- **Passos:**
  1. PC-01 com `aux.diogo@`; ir para `{D}` e selecionar a primeira marcação; escrever a nota e carregar em «Guardar informação de apoio».
  2. PC-02; PC-01 com `aux.carla@`; selecionar a marcação cancelada; escrever a nota e guardar.
  3. Selecionar de novo a primeira marcação.
- **Resultado esperado:**
  1. No passo 1 a API responde 403 e aparece «A sua conta não tem autorização para registar informação de apoio. Esta autorização é dada pelo Administrador, na conta de funcionário.»; o formulário de nota deixa de ser oferecido.
  2. No passo 2 a API responde 422 e a mensagem sobre a marcação Cancelada aparece no formulário, sem perder o texto escrito.
  3. No passo 3 a lista «Notas de apoio» não tem nenhuma nota com o `{sufixo}`.
- **Dados criados e removidos:** cria um Cliente, um paciente e duas marcações; ficam até à reposição da base.

## 10. Cenários — F004 Registo clínico

Os passos deste capítulo descrevem as ações sobre o ecrã W13 a partir do protótipo e da API v03 (secção 6). As consultas usadas são criadas pelo próprio cenário e confirmadas, porque a API só aceita registos clínicos de consultas `Confirmada` ou `Realizada` do Veterinário autenticado (EP-031).

### ST-WEB-F004-01 — Registar notas clínicas, diagnóstico, tratamento e prescrição numa consulta

| Tipo     | Perfil      | Requisitos                         | UC / ecrãs      | Condição       | Automação |
| -------- | ----------- | ---------------------------------- | --------------- | -------------- | --------- |
| Positivo | Veterinário | RF030, RF031, RF032, RF033, RNF005 | UC12 · W08, W13 | CT-WEB-F004-01 | I130      |

- **Pré-condições:** marcação de teste criada com PC-03, PC-04 e PC-05 (Dra. Ana, `{D}` com desvio 18) e confirmada com PC-06; PC-02.
- **Dados:** notas clínicas `Notas {sufixo}`; diagnóstico `Diagnóstico {sufixo}`; tratamento `Tratamento {sufixo}` com data de aplicação `{hoje}`; prescrição: medicamento `Medicamento {sufixo}`, dosagem «1 comprimido», frequência «2 vezes por dia», duração 5 dias.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir a ficha de `Animal {sufixo}` e, na «Área clínica», abrir «Registo clínico».
  2. Escolher a consulta de `{D}` e criar o registo clínico com os quatro elementos dos dados.
  3. Gravar; recarregar a página e voltar a abrir o registo.
- **Resultado esperado:** o registo fica associado à consulta de `{D}` e ao paciente, com a Dra. Ana como autora e a data de criação; mostra as notas, o diagnóstico, o tratamento com a data e a prescrição com medicamento, dosagem, frequência e duração; depois de recarregar, mostra os mesmos valores. O estado da marcação continua Confirmada.
- **Dados criados e removidos:** cria um Cliente, um paciente, uma marcação e um registo clínico; ficam até à reposição da base.

### ST-WEB-F004-02 — Recusar registos clínicos inválidos

| Tipo     | Perfil      | Requisitos                         | UC / ecrãs | Condição       | Automação |
| -------- | ----------- | ---------------------------------- | ---------- | -------------- | --------- |
| Negativo | Veterinário | RF030, RF031, RF032, RF033, RNF004 | UC12 · W13 | CT-WEB-F004-01 | I130      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; duas marcações criadas com PC-05 em `{D}` com desvio 19 para a Dra. Ana: a primeira confirmada com PC-06 e a segunda deixada Pendente; PC-02.
- **Dados:** (a) registo sem nenhum elemento clínico; (b) prescrição sem dosagem; (c) registo válido; (d) segundo registo para a mesma consulta.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir o «Registo clínico» do paciente de teste.
  2. Para a primeira consulta, tentar gravar (a); depois (b).
  3. Gravar (c) e tentar criar (d) para a mesma consulta.
  4. Verificar se a segunda consulta (Pendente) pode receber um registo clínico.
- **Resultado esperado:**
  1. Passo 2: o registo não é gravado; aparecem a mensagem «O registo clínico tem de ter pelo menos um elemento clínico: notas clínicas, diagnóstico, um tratamento ou uma prescrição.» e depois «A dosagem da prescrição é obrigatória.», sem bloquear o formulário.
  2. Passo 3: o registo (c) é gravado; para (d) a aplicação não oferece um segundo registo da mesma consulta ou mostra a mensagem da API (409) de que a consulta já tem registo clínico.
  3. Passo 4: a consulta Pendente não é oferecida para registo clínico ou, se o pedido for feito, a mensagem da API (422) é mostrada e nada é gravado.
- **Dados criados e removidos:** cria um Cliente, um paciente, duas marcações e um registo clínico; ficam até à reposição da base.

### ST-WEB-F004-03 — Atualizar um registo clínico

| Tipo     | Perfil      | Requisitos                  | UC / ecrãs | Condição       | Automação |
| -------- | ----------- | --------------------------- | ---------- | -------------- | --------- |
| Positivo | Veterinário | RF034, RF032, RF033, RNF005 | UC12 · W13 | CT-WEB-F004-02 | I130      |

- **Pré-condições:** registo clínico criado como em ST-WEB-F004-01 (com `{D}` com desvio 20), pelo próprio cenário.
- **Dados:** novo diagnóstico `Diagnóstico alterado {sufixo}`; segundo tratamento `Tratamento 2 {sufixo}` com data `{hoje}`.
- **Passos:**
  1. Abrir o registo clínico; alterar o diagnóstico e acrescentar o segundo tratamento.
  2. Gravar; recarregar e abrir o histórico clínico do paciente.
- **Resultado esperado:** o registo mostra o novo diagnóstico, os dois tratamentos, a prescrição original e a indicação de quem alterou e quando; continua associado à mesma consulta e ao mesmo paciente; o histórico mostra o novo diagnóstico e 2 tratamentos.
- **Dados criados e removidos:** cria um Cliente, um paciente, uma marcação e um registo clínico; ficam até à reposição da base.

### ST-WEB-F004-04 — Não permitir alterar o registo clínico de outro Veterinário

| Tipo     | Perfil              | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ------------------- | ------------- | ---------- | -------------- | --------- |
| Negativo | Veterinário (outro) | RF034, RNF001 | UC12 · W13 | CT-WEB-F004-02 | I130      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** registo clínico da Dra. Ana na consulta do Bobi de 2026-10-14 (dados de base).
- **Passos:**
  1. PC-01 com `vet.bruno@`; abrir a ficha do Bobi → «Registo clínico» e abrir o registo de 2026-10-14.
  2. Tentar alterá-lo (se a aplicação oferecer a edição) e gravar.
- **Resultado esperado:** o Dr. Bruno consegue ler o registo, mas a aplicação não oferece a sua edição ou, se o pedido for feito, mostra a mensagem da API (403) e o registo mantém o diagnóstico «Otite externa».
- **Dados criados e removidos:** nenhum.

### ST-WEB-F004-05 — Consultar o histórico clínico do paciente

| Tipo     | Perfil      | Requisitos | UC / ecrãs      | Condição       | Automação |
| -------- | ----------- | ---------- | --------------- | -------------- | --------- |
| Positivo | Veterinário | RF035      | UC12 · W08, W13 | CT-WEB-F004-03 | I130      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** paciente Bobi; intervalo de 2026-10-01 a 2026-10-31.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir a ficha do Bobi → «Registo clínico».
  2. Filtrar o histórico pelo intervalo dos dados.
  3. Abrir o registo de 2026-10-14.
- **Resultado esperado:** o histórico lista os registos do Bobi do mais recente para o mais antigo, incluindo o de 14/10/2026 com a Dra. Ana e o diagnóstico «Otite externa» (1 tratamento); com o filtro continua a aparecer o de 14/10/2026; o registo aberto mostra as notas clínicas, o diagnóstico e o tratamento «Limpeza auricular com solução fisiológica».
- **Dados criados e removidos:** nenhum.

### ST-WEB-F004-06 — Recusar um intervalo de datas inválido no histórico

| Tipo     | Perfil      | Requisitos    | UC / ecrãs | Condição       | Automação |
| -------- | ----------- | ------------- | ---------- | -------------- | --------- |
| Negativo | Veterinário | RF035, RNF004 | UC12 · W13 | CT-WEB-F004-03 | I130      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** data de início 2026-10-31 e data de fim 2026-10-01.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir o histórico clínico do Bobi.
  2. Aplicar o intervalo dos dados.
- **Resultado esperado:** aparece a mensagem da API (400) «A data de fim (dataFim) não pode ser anterior à data de início (dataInicio).» ou, se o W13 validar o intervalo antes do pedido, uma mensagem local com o mesmo assunto (E-06), sem bloquear o ecrã; a lista anterior mantém-se ou fica vazia com a mensagem, sem erro técnico.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F004-07 — O Auxiliar Veterinário consulta só a informação clínica autorizada

| Tipo     | Perfil               | Requisitos    | UC / ecrãs      | Condição       | Automação |
| -------- | -------------------- | ------------- | --------------- | -------------- | --------- |
| Positivo | Auxiliar Veterinário | RF036, RNF001 | UC13 · W08, W13 | CT-WEB-F004-04 | I130      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** paciente Bobi.
- **Passos:**
  1. PC-01 com `aux.carla@`; abrir a ficha do Bobi e carregar em «Registo clínico».
  2. Abrir o registo de 2026-10-14.
- **Resultado esperado:** o Auxiliar vê o histórico e o registo em leitura (diagnóstico, notas, tratamento), sem ações para criar ou alterar notas clínicas, diagnósticos, tratamentos ou prescrições; não existe ligação para «Vacinação».
- **Dados criados e removidos:** nenhum.

### ST-WEB-F004-08 — Recusar o acesso ao registo clínico a quem não o pode ver

| Tipo     | Perfil                      | Requisitos           | UC / ecrãs | Condição                      | Automação |
| -------- | --------------------------- | -------------------- | ---------- | ----------------------------- | --------- |
| Negativo | Rececionista, Administrador | RF036, RF005, RNF001 | UC12 · W13 | CT-WEB-F004-04, CT-WEB-TRV-02 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** rota `/pacientes/{id do Bobi}/registo`.
- **Passos:**
  1. PC-01 com `rececao@`; abrir a rota.
  2. PC-02; PC-01 com `admin@`; abrir a rota.
- **Resultado esperado:** nos dois casos aparece «Acesso negado» («O seu perfil não tem permissão para ver esta página.»), sem nenhuma informação clínica.
- **Dados criados e removidos:** nenhum.

## 11. Cenários — F006 Boletim de vacinas e desparasitações

Os passos deste capítulo descrevem as ações sobre o ecrã W16 a partir do protótipo e da API v03 (secção 6). As datas de administração nunca são futuras (EP-043).

### ST-WEB-F006-01 — Registar uma vacinação com intervalo

| Tipo     | Perfil      | Requisitos           | UC / ecrãs      | Condição                       | Automação |
| -------- | ----------- | -------------------- | --------------- | ------------------------------ | --------- |
| Positivo | Veterinário | RF043, RF045, RNF005 | UC16 · W08, W16 | CT-WEB-F006-01, CT-WEB-F006-02 | I131      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; PC-02.
- **Dados:** tipo Vacina; produto `Vacina {sufixo}`; lote `L-{sufixo}`; data de administração 2026-10-01; intervalo 120 dias.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir a ficha de `Animal {sufixo}` e, na «Área clínica», abrir «Vacinação».
  2. Registar a vacinação com os dados e gravar.
  3. Recarregar a página.
- **Resultado esperado:** a vacinação aparece no boletim com o tipo, o produto, o lote, a data 01/10/2026, o intervalo de 120 dias, a Dra. Ana e a próxima administração prevista em **29/01/2027** (2026-10-01 + 120 dias, calculada pela API); a mesma data aparece nas próximas administrações; depois de recarregar, tudo se mantém.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma administração; ficam até à reposição da base.

### ST-WEB-F006-02 — Registar uma desparasitação sem próxima administração

| Tipo     | Perfil      | Requisitos   | UC / ecrãs | Condição                       | Automação |
| -------- | ----------- | ------------ | ---------- | ------------------------------ | --------- |
| Positivo | Veterinário | RF044, RF045 | UC16 · W16 | CT-WEB-F006-01, CT-WEB-F006-02 | I131      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; PC-02.
- **Dados:** tipo Desparasitação; produto `Desparasitante {sufixo}`; lote `D-{sufixo}`; data `{hoje}`; sem intervalo.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir a «Vacinação» de `Animal {sufixo}`.
  2. Registar a desparasitação sem intervalo e gravar.
- **Resultado esperado:** a desparasitação aparece no boletim com o produto, o lote e a data de hoje, sem intervalo e sem próxima administração prevista; não é acrescentada nenhuma linha às próximas administrações.
- **Dados criados e removidos:** cria um Cliente, um paciente e uma administração; ficam até à reposição da base.

### ST-WEB-F006-03 — Recusar administrações inválidas

| Tipo     | Perfil      | Requisitos                  | UC / ecrãs | Condição                       | Automação |
| -------- | ----------- | --------------------------- | ---------- | ------------------------------ | --------- |
| Negativo | Veterinário | RF043, RF044, RF045, RNF004 | UC16 · W16 | CT-WEB-F006-01, CT-WEB-F006-02 | I131      |

- **Pré-condições:** paciente de teste criado com PC-03 e PC-04; PC-02.
- **Dados:** (a) sem produto; (b) sem lote; (c) data de amanhã; (d) intervalo 0.
- **Passos:**
  1. PC-01 com `vet.ana@`; abrir a «Vacinação» de `Animal {sufixo}`.
  2. Tentar gravar uma vacinação com cada um dos casos, corrigindo o anterior antes do seguinte.
  3. Recarregar a página.
- **Resultado esperado:** em cada caso o registo é recusado com a mensagem correspondente («O produto é obrigatório.», «O lote é obrigatório.», «A data de administração não pode ser futura.», «O intervalo (intervaloDias), quando indicado, deve ser superior a 0 dias.» ou a validação local equivalente), sem bloquear o formulário; no passo 3 o boletim do paciente continua vazio.
- **Dados criados e removidos:** cria um Cliente e um paciente; ficam até à reposição da base.

### ST-WEB-F006-04 — Consultar o boletim completo do paciente

| Tipo     | Perfil      | Requisitos   | UC / ecrãs      | Condição       | Automação |
| -------- | ----------- | ------------ | --------------- | -------------- | --------- |
| Positivo | Veterinário | RF046, RF045 | UC16 · W08, W16 | CT-WEB-F006-03 | I131      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** boletim do Bobi (dados de base).
- **Passos:**
  1. PC-01 com `vet.bruno@`; abrir a ficha do Bobi → «Vacinação».
  2. Filtrar por tipo, se o ecrã o permitir (Vacina e depois Desparasitação).
- **Resultado esperado:** o boletim mostra as três administrações do Bobi (Vacina antirrábica 14/10/2025, Vacina polivalente 14/10/2026 e Desparasitante interno 14/10/2026), com produto, lote, data, intervalo e Veterinário; as próximas administrações previstas a partir de hoje incluem 12/01/2027 (desparasitante, 2026-10-14 + 90) e 14/10/2027 (polivalente, 2026-10-14 + 365) e não incluem a antirrábica de 14/10/2026, já passada; com o filtro aparecem só as do tipo escolhido.
- **Dados criados e removidos:** nenhum.

### ST-WEB-F006-05 — Recusar o boletim web a quem não é Veterinário

| Tipo     | Perfil                             | Requisitos           | UC / ecrãs      | Condição                      | Automação |
| -------- | ---------------------------------- | -------------------- | --------------- | ----------------------------- | --------- |
| Negativo | Rececionista, Auxiliar Veterinário | RF046, RF005, RNF001 | UC16 · W08, W16 | CT-WEB-F006-03, CT-WEB-TRV-02 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** rota `/pacientes/{id do Bobi}/vacinacao`.
- **Passos:**
  1. PC-01 com `rececao@`; abrir a ficha do Bobi e depois a rota.
  2. PC-02; PC-01 com `aux.carla@`; repetir.
- **Resultado esperado:** nenhuma das duas fichas tem a ligação «Vacinação»; a rota mostra «Acesso negado» nos dois perfis.
- **Dados criados e removidos:** nenhum.

## 12. Cenários transversais — perfis, sessão, rede e erros

### ST-WEB-TRV-01 — Menu de cada perfil sem entradas de outros perfis

| Tipo     | Perfil                    | Requisitos    | UC / ecrãs   | Condição      | Automação |
| -------- | ------------------------- | ------------- | ------------ | ------------- | --------- |
| Negativo | Os quatro perfis internos | RF005, RNF001 | UC01 · todos | CT-WEB-TRV-01 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** contas `admin@`, `rececao@`, `vet.ana@`, `aux.carla@`.
- **Passos:** para cada conta, PC-01, ler o menu lateral e PC-02.
- **Resultado esperado:** o menu tem exatamente as entradas da tabela de ST-WEB-F001-01 e nenhuma outra (por exemplo, o Administrador não tem Agenda nem Pacientes; o Auxiliar só tem «Preparação de consultas»).
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-02 — Recusar o acesso por endereço direto a rotas de outro perfil

| Tipo     | Perfil                    | Requisitos                  | UC / ecrãs   | Condição      | Automação |
| -------- | ------------------------- | --------------------------- | ------------ | ------------- | --------- |
| Negativo | Os quatro perfis internos | RF005, RF012, RF026, RNF001 | UC01 · todos | CT-WEB-TRV-02 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:**

  | Conta        | Rotas a abrir                                                                                     |
  | ------------ | ------------------------------------------------------------------------------------------------- |
  | `admin@`     | `/agenda`, `/agenda/nova`, `/pacientes`, `/pacientes/1`, `/preparacao`                            |
  | `rececao@`   | `/funcionarios`, `/funcionarios/novo`, `/clinica`, `/preparacao`                                  |
  | `vet.ana@`   | `/clientes`, `/clientes/novo`, `/funcionarios`, `/clinica`, `/agenda/nova`, `/preparacao`         |
  | `aux.carla@` | `/agenda`, `/agenda/1`, `/pacientes`, `/pacientes/novo`, `/clientes`, `/funcionarios`, `/clinica` |

- **Passos:** para cada conta, PC-01 e abrir cada rota da tabela escrevendo-a na barra de endereço; PC-02.
- **Resultado esperado:** cada rota mostra «Acesso negado» com «O seu perfil não tem permissão para ver esta página.» e «Voltar ao início», sem dados da página pedida e sem pedidos com sucesso à API dessa página; a sessão mantém-se e «Voltar ao início» leva à página inicial do perfil.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-03 — Rotas protegidas sem sessão levam ao início de sessão

| Tipo     | Perfil     | Requisitos    | UC / ecrãs | Condição      | Automação |
| -------- | ---------- | ------------- | ---------- | ------------- | --------- |
| Negativo | Sem sessão | RF004, RNF001 | UC01 · W01 | CT-WEB-TRV-02 | I132      |

- **Pré-condições:** navegador sem sessão (sessão anónima nova).
- **Dados:** rotas `/funcionarios`, `/clientes/1`, `/agenda`, `/preparacao`.
- **Passos:** abrir cada rota diretamente.
- **Resultado esperado:** cada rota leva a `/login` com «Iniciar sessão», sem mostrar dados da página pedida.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-04 — O Cliente não usa a aplicação web

| Tipo     | Perfil  | Requisitos    | UC / ecrãs | Condição      | Automação |
| -------- | ------- | ------------- | ---------- | ------------- | --------- |
| Negativo | Cliente | RNF003, RF005 | UC01 · W01 | CT-WEB-TRV-03 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** `cliente.joao@clinica-g08.test` com a palavra-passe correta.
- **Passos:**
  1. Abrir `/login`, preencher as credenciais do Cliente e carregar em «Entrar».
  2. Abrir `/clientes`.
- **Resultado esperado:** no passo 1 aparece «Os Clientes não têm acesso à aplicação web. Utilize a aplicação móvel.», o endereço continua em `/login` e não fica sessão guardada no navegador; no passo 2 a aplicação volta a `/login`.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-05 — Sessão revogada durante a utilização

| Tipo     | Perfil       | Requisitos   | UC / ecrãs            | Condição                      | Automação |
| -------- | ------------ | ------------ | --------------------- | ----------------------------- | --------- |
| Negativo | Rececionista | RF004, RF003 | UC01, UC02 · W01, W04 | CT-WEB-F001-04, CT-WEB-TRV-04 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** conta `rececao@`.
- **Passos:**
  1. PC-01 com `rececao@`; abrir «Clientes».
  2. Revogar o token da sessão fora do ecrã: enviar `POST /auth/logout` ao backend com o mesmo token (na automação, com o token lido do armazenamento da sessão do navegador; à mão, a partir das ferramentas de programador).
  3. Na aplicação, abrir «Pacientes».
- **Resultado esperado:** o pedido do passo 3 recebe 401 e a aplicação vai para `/login` com «A sua sessão terminou. Inicie sessão novamente.», sem mostrar a lista de pacientes; o armazenamento da sessão do navegador fica vazio.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-06 — Sessão inválida ao recarregar

| Tipo     | Perfil        | Requisitos | UC / ecrãs | Condição       | Automação |
| -------- | ------------- | ---------- | ---------- | -------------- | --------- |
| Negativo | Administrador | RF004      | UC01 · W01 | CT-WEB-F001-04 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** conta `admin@`.
- **Passos:**
  1. PC-01 com `admin@`; abrir «Funcionários».
  2. Substituir, no armazenamento da sessão do navegador (chave `clinica.sessao`), o token por um valor inválido, como acontece com um token expirado.
  3. Recarregar a página (F5).
- **Resultado esperado:** depois de «A verificar a sessão…», a confirmação da sessão no backend (EP-003) falha e a aplicação mostra «Iniciar sessão», sem a lista de funcionários; o armazenamento da sessão fica vazio.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-07 — Backend indisponível ao abrir uma página

| Tipo     | Perfil        | Requisitos | UC / ecrãs   | Condição      | Automação |
| -------- | ------------- | ---------- | ------------ | ------------- | --------- |
| Negativo | Administrador | RNF006     | — · W02, W06 | CT-WEB-TRV-05 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** conta `admin@`.
- **Passos:**
  1. PC-01 com `admin@`.
  2. Cortar a ligação ao backend (na automação, recusando os pedidos à API com `page.route`; à mão, parando o backend).
  3. Abrir pelo menu «Clínica e horários» e depois «Funcionários».
  4. Repor a ligação e carregar em «Tentar novamente» em «Clínica e horários».
- **Resultado esperado:** no passo 3 aparece «Não foi possível contactar o servidor. Verifique a ligação.», sem ficar indefinidamente em «A carregar…» e sem terminar a sessão; em «Clínica e horários» aparece «Tentar novamente»; no passo 4 os dados da clínica aparecem.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-08 — Backend indisponível ao guardar um formulário

| Tipo     | Perfil       | Requisitos     | UC / ecrãs | Condição      | Automação |
| -------- | ------------ | -------------- | ---------- | ------------- | --------- |
| Negativo | Rececionista | RNF006, RNF005 | — · W05    | CT-WEB-TRV-05 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** os de ST-WEB-F001-13.
- **Passos:**
  1. PC-01 com `rececao@`; menu «Clientes» → «Novo cliente»; preencher o formulário.
  2. Cortar a ligação ao backend e carregar em «Registar cliente».
  3. Repor a ligação e carregar de novo em «Registar cliente».
- **Resultado esperado:** no passo 2 aparece «Não foi possível contactar o servidor. Verifique a ligação.», os campos preenchidos mantêm-se (exceto a palavra-passe, que é limpa por segurança e volta a ser escrita) e o botão volta a estar ativo; no passo 3 o Cliente é registado.
- **Dados criados e removidos:** cria um Cliente; fica até à reposição da base.

### ST-WEB-TRV-09 — Mostrar os erros da API sem detalhes técnicos

| Tipo     | Perfil       | Requisitos | UC / ecrãs        | Condição      | Automação |
| -------- | ------------ | ---------- | ----------------- | ------------- | --------- |
| Negativo | Rececionista | RNF004     | — · W05, W08, W11 | CT-WEB-TRV-04 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** `/clientes/999999`, `/pacientes/999999`, `/agenda/999999`.
- **Passos:** PC-01 com `rececao@`; abrir cada rota.
- **Resultado esperado:** cada página mostra a mensagem do campo `detail` da API (404) ou «O registo pedido não foi encontrado.», com a ligação de regresso à lista; não aparecem códigos de exceção, rastos de pilha nem texto em JSON. Os casos 400, 409, 412 e 422 estão cobertos em ST-WEB-F001-15, ST-WEB-F002-03, ST-WEB-F003-06, ST-WEB-F003-08 e ST-WEB-F003-17.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-10 — Estados de carregamento e botões inativos durante os pedidos

| Tipo     | Perfil        | Requisitos                 | UC / ecrãs   | Condição      | Automação |
| -------- | ------------- | -------------------------- | ------------ | ------------- | --------- |
| Positivo | Administrador | — (qualidade de interface) | — · W02, W06 | CT-WEB-TRV-06 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** respostas da API atrasadas (na automação, com `page.route` a atrasar a resposta até o estado ser verificado; à mão, com a limitação de rede das ferramentas de programador).
- **Passos:**
  1. PC-01 com `admin@`; com as respostas atrasadas, abrir «Funcionários» e depois «Clínica e horários».
  2. Em «Clínica e horários», carregar em «Guardar» sem alterar nada.
- **Resultado esperado:** no passo 1 aparece «A carregar…» até chegar a resposta; no passo 2 o botão passa a «A guardar…» e fica inativo, tal como «Cancelar», até à resposta; no fim aparece «Alterações guardadas.» (os dados gravados são os mesmos).
- **Dados criados e removidos:** nenhum (grava a configuração sem alterações).

### ST-WEB-TRV-11 — Palavras-passe e tokens não ficam expostos

| Tipo     | Perfil       | Requisitos | UC / ecrãs            | Condição      | Automação |
| -------- | ------------ | ---------- | --------------------- | ------------- | --------- |
| Negativo | Rececionista | RNF001     | UC01, UC02 · W01, W05 | CT-WEB-TRV-07 | I132      |

- **Pré-condições:** sem sessão iniciada; consola do navegador a ser registada.
- **Dados:** conta `rececao@`.
- **Passos:**
  1. Abrir `/login`, escrever as credenciais e entrar.
  2. Abrir «Clientes» → «Novo cliente» e escrever uma palavra-passe inicial.
  3. PC-02.
- **Resultado esperado:** os campos de palavra-passe mostram os caracteres ocultos (tipo `password`); nenhum endereço visitado contém o token nem a palavra-passe; as mensagens da consola não contêm o token nem palavras-passe; depois do passo 3 o armazenamento da sessão do navegador não tem a chave `clinica.sessao`.
- **Dados criados e removidos:** nenhum.

### ST-WEB-TRV-12 — Endereço inexistente

| Tipo     | Perfil      | Requisitos | UC / ecrãs | Condição      | Automação |
| -------- | ----------- | ---------- | ---------- | ------------- | --------- |
| Negativo | Veterinário | RF005      | — · —      | CT-WEB-TRV-02 | I132      |

- **Pré-condições:** sem sessão iniciada.
- **Dados:** `/nao-existe-{sufixo}`.
- **Passos:** PC-01 com `vet.ana@`; abrir o endereço.
- **Resultado esperado:** aparece «Página não encontrada» com «O endereço que abriu não existe.» e «Voltar ao início», que leva a `/agenda`.
- **Dados criados e removidos:** nenhum.

## 13. Matriz cenários ↔ requisitos

Os identificadores da matriz omitem o prefixo `ST-WEB-` (por exemplo, `F001-07` é `ST-WEB-F001-07`).

### 13.1. Requisitos funcionais

| Requisito | Título                                            | Cenários positivos        | Cenários negativos                                        | Automação  |
| --------- | ------------------------------------------------- | ------------------------- | --------------------------------------------------------- | ---------- |
| RF001     | Autenticar utilizador                             | F001-01                   | — (o caso negativo é o RF002)                             | I129       |
| RF002     | Rejeitar autenticação inválida                    | F001-11                   | F001-02, F001-03                                          | I129       |
| RF003     | Terminar sessão                                   | F001-04                   | TRV-05                                                    | I132       |
| RF004     | Manter acesso autenticado válido                  | F001-04, F001-05          | TRV-03, TRV-05, TRV-06                                    | I132       |
| RF005     | Restringir funcionalidades por perfil             | F001-01, F001-07          | F002-10, F004-08, F006-05, TRV-01, TRV-02, TRV-04, TRV-12 | I129, I132 |
| RF006     | Criar conta de funcionário                        | F001-07                   | F001-08                                                   | I129       |
| RF007     | Atualizar conta de funcionário                    | F001-06, F001-09          | F001-10                                                   | I129       |
| RF008     | Atribuir perfil a funcionário                     | F001-07, F001-09          | F001-10                                                   | I129       |
| RF009     | Bloquear conta de funcionário                     | F001-06, F001-11          | F001-03, F001-12                                          | I129       |
| RF010     | Criar conta de Cliente                            | F001-13, F001-14          | F001-15                                                   | I129       |
| RF011     | Atualizar conta de Cliente                        | F001-16, F001-18          | F001-17                                                   | I129       |
| RF012     | Consultar dados gerais e configurações da clínica | F001-19                   | TRV-02                                                    | I129, I132 |
| RF013     | Atualizar dados gerais e configurações da clínica | F001-20                   | F001-21                                                   | I129       |
| RF014     | Registar paciente                                 | F002-01, F002-02          | F002-03                                                   | I129       |
| RF015     | Consultar ficha do paciente                       | F002-04                   | F002-05, F002-10                                          | I129, I132 |
| RF016     | Atualizar dados administrativos do paciente       | F002-06                   | F002-07                                                   | I129       |
| RF017     | Pesquisar pacientes por proprietário              | F001-18, F002-02, F002-08 | F002-09                                                   | I129       |
| RF019     | Gerir horários de funcionamento                   | F003-01                   | F003-02                                                   | I129       |
| RF020     | Consultar disponibilidade dos Veterinários        | F003-03                   | F003-04                                                   | I130       |
| RF021     | Criar marcação                                    | F003-05                   | F003-06                                                   | I130       |
| RF022     | Alterar marcação                                  | F003-07                   | F003-08                                                   | I130       |
| RF023     | Atualizar estado da marcação                      | F003-09                   | F003-04, F003-08, F003-10                                 | I130       |
| RF024     | Consultar agenda da clínica                       | F003-05, F003-11          | F003-12                                                   | I130       |
| RF025     | Registar chegada à consulta                       | F003-13                   | F003-14                                                   | I130       |
| RF026     | Consultar informação de preparação da consulta    | F003-15                   | TRV-02                                                    | I130, I132 |
| RF027     | Registar informação não clínica de apoio          | F003-16                   | F003-17                                                   | I130       |
| RF030     | Registar notas clínicas                           | F004-01                   | F004-02                                                   | I130       |
| RF031     | Registar diagnóstico                              | F004-01                   | F004-02                                                   | I130       |
| RF032     | Registar tratamento                               | F004-01, F004-03          | F004-02                                                   | I130       |
| RF033     | Registar prescrição                               | F004-01, F004-03          | F004-02                                                   | I130       |
| RF034     | Atualizar registo clínico                         | F004-03                   | F004-04                                                   | I130       |
| RF035     | Consultar histórico clínico                       | F004-05                   | F004-06                                                   | I130       |
| RF036     | Consultar informação clínica autorizada           | F004-07                   | F004-08                                                   | I130, I132 |
| RF043     | Registar vacinação                                | F006-01                   | F006-03                                                   | I131       |
| RF044     | Registar desparasitação                           | F006-02                   | F006-03                                                   | I131       |
| RF045     | Calcular próxima administração                    | F006-01, F006-02, F006-04 | F006-03                                                   | I131       |
| RF046     | Consultar boletim no sistema web                  | F006-04                   | F006-05                                                   | I131, I132 |

Os 37 requisitos funcionais web têm pelo menos um cenário positivo e todos, exceto o RF001 (cujo caso negativo é o próprio RF002), têm pelo menos um cenário negativo (critério 1 da I128 e critério de saída 2 do Plano de Testes).

### 13.2. Requisitos não funcionais

| Requisito | Cenários                                                                                                                             | Observação                                                                                                          |
| --------- | ------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------- |
| RNF001    | F002-10, F003-12, F003-17, F004-04, F004-07, F004-08, F006-05, TRV-01, TRV-02, TRV-03, TRV-11                                        | Acesso por perfil no menu, nas rotas e nas ações, também por endereço direto                                        |
| RNF003    | TRV-04                                                                                                                               | Restrição: só tem caso negativo (o Cliente é recusado)                                                              |
| RNF004    | F001-02, F001-08, F001-15, F001-17, F001-21, F002-03, F002-07, F003-02, F003-06, F004-02, F004-06, F006-03, TRV-09                   | Os casos negativos rejeitam dados inválidos; os positivos de cada requisito confirmam que dados válidos são aceites |
| RNF005    | F001-16, F001-20, F002-01, F002-04, F002-06, F003-01, F003-05, F003-07, F003-09, F003-13, F003-16, F004-01, F004-03, F006-01, TRV-08 | Dados gravados visíveis depois de recarregar ou numa consulta posterior                                             |
| RNF006    | TRV-07, TRV-08                                                                                                                       | Sem ligação ao backend, mensagem de falha e sem modo offline                                                        |

### 13.3. Condições de teste do Plano ↔ cenários

| Condição       | Cenários                                             |
| -------------- | ---------------------------------------------------- |
| CT-WEB-F001-01 | F001-01                                              |
| CT-WEB-F001-02 | F001-02, F001-03                                     |
| CT-WEB-F001-03 | F001-04                                              |
| CT-WEB-F001-04 | F001-05, TRV-05, TRV-06                              |
| CT-WEB-F001-05 | F001-06, F001-07, F001-08, F001-09                   |
| CT-WEB-F001-06 | F001-09, F001-10                                     |
| CT-WEB-F001-07 | F001-11, F001-12                                     |
| CT-WEB-F001-08 | F001-13, F001-14, F001-15, F001-16, F001-17, F001-18 |
| CT-WEB-F001-09 | F001-19, F001-20, F001-21                            |
| CT-WEB-F002-01 | F002-01, F002-02, F002-03                            |
| CT-WEB-F002-02 | F002-04, F002-05                                     |
| CT-WEB-F002-03 | F002-06, F002-07                                     |
| CT-WEB-F002-04 | F002-08, F002-09                                     |
| CT-WEB-F003-01 | F003-01, F003-02                                     |
| CT-WEB-F003-02 | F003-03, F003-04                                     |
| CT-WEB-F003-03 | F003-05, F003-06                                     |
| CT-WEB-F003-04 | F003-07, F003-08                                     |
| CT-WEB-F003-05 | F003-09, F003-10                                     |
| CT-WEB-F003-06 | F003-11, F003-12                                     |
| CT-WEB-F003-07 | F003-13, F003-14                                     |
| CT-WEB-F003-08 | F003-15, F003-16, F003-17                            |
| CT-WEB-F004-01 | F004-01, F004-02                                     |
| CT-WEB-F004-02 | F004-03, F004-04                                     |
| CT-WEB-F004-03 | F004-05, F004-06                                     |
| CT-WEB-F004-04 | F004-07, F004-08                                     |
| CT-WEB-F006-01 | F006-01, F006-02, F006-03                            |
| CT-WEB-F006-02 | F006-01, F006-02, F006-03                            |
| CT-WEB-F006-03 | F006-04, F006-05                                     |
| CT-WEB-TRV-01  | F002-10, TRV-01                                      |
| CT-WEB-TRV-02  | F004-08, F006-05, TRV-02, TRV-03, TRV-12             |
| CT-WEB-TRV-03  | TRV-04                                               |
| CT-WEB-TRV-04  | F003-08, TRV-05, TRV-09                              |
| CT-WEB-TRV-05  | TRV-07, TRV-08                                       |
| CT-WEB-TRV-06  | TRV-10                                               |
| CT-WEB-TRV-07  | TRV-11                                               |

Todas as condições de teste da secção 10 do Plano de Testes do Frontend Web têm pelo menos um cenário.

## 14. Automação e casos não automatizados

### 14.1. Distribuição pelas tarefas de automação

| Tarefa    | Âmbito                                                                        | Cenários                                                                                                                                                                                                                                                                     | N.º    |
| --------- | ----------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------ |
| I129      | Identidade, funcionários, clínica e horário, Clientes e pacientes (Sprint 07) | F001-01, F001-02, F001-03, F001-06, F001-07, F001-08, F001-09, F001-10, F001-11, F001-12, F001-13, F001-14, F001-15, F001-16, F001-17, F001-18, F001-19, F001-20, F001-21, F002-01, F002-02, F002-03, F002-04, F002-05, F002-06, F002-07, F002-08, F002-09, F003-01, F003-02 | 30     |
| I130      | Agenda, marcações, chegada, preparação e registo clínico (Sprint 08)          | F003-03, F003-04, F003-05, F003-06, F003-07, F003-08, F003-09, F003-10, F003-11, F003-12, F003-13, F003-14, F003-15, F003-16, F003-17, F004-01, F004-02, F004-03, F004-04, F004-05, F004-06, F004-07                                                                         | 22     |
| I131      | Vacinação e desparasitação (Sprint 08)                                        | F006-01, F006-02, F006-03, F006-04                                                                                                                                                                                                                                           | 4      |
| I132      | Sessão, permissões por perfil, falhas de rede e erros (Sprint 07)             | F001-04, F001-05, F002-10, F004-08, F006-05, TRV-01, TRV-02, TRV-03, TRV-04, TRV-05, TRV-06, TRV-07, TRV-08, TRV-09, TRV-10, TRV-11, TRV-12                                                                                                                                  | 17     |
| **Total** |                                                                               | 35 positivos e 38 negativos                                                                                                                                                                                                                                                  | **73** |

Cenários exclusivos (não correm em paralelo com outros, secção 5.5): F001-20, F001-21, F003-01, F003-02.

### 14.2. Casos não automatizados

Todos os cenários desta versão são automatizáveis com Playwright contra o backend real; nenhum fica só com execução manual. As situações que, à mão, exigem uma intervenção fora da aplicação têm na automação uma técnica equivalente, sem simular as respostas da API:

| Cenário                                        | Intervenção manual                                      | Técnica na automação                                                                                       |
| ---------------------------------------------- | ------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| ST-WEB-TRV-05                                  | Enviar `POST /auth/logout` com o token da sessão        | Pedido feito pelo próprio teste (`request`) com o token lido do armazenamento da sessão                    |
| ST-WEB-TRV-06                                  | Alterar o token guardado nas ferramentas de programador | Alteração do `sessionStorage` pelo teste antes de recarregar                                               |
| ST-WEB-TRV-07, ST-WEB-TRV-08                   | Parar o backend                                         | `page.route` a recusar os pedidos à API (falha de rede), só durante o passo indicado                       |
| ST-WEB-TRV-10                                  | Limitar a rede nas ferramentas de programador           | `page.route` a atrasar a resposta real até o estado ser verificado (espera por condição, sem pausas fixas) |
| ST-WEB-F003-06, ST-WEB-F003-08, ST-WEB-F003-14 | Duas janelas do navegador                               | Dois contextos do navegador no mesmo teste                                                                 |

Se, na automação, algum cenário não puder ser automatizado, a tarefa correspondente regista a justificação no Pull Request e o cenário é executado à mão e registado no relatório de resultados (I133).

### 14.3. Execução manual

Para a verificação do Revisor (verificação 2 da I128) e para repetir um cenário à mão:

1. Repor a base e carregar os dados de demonstração; arrancar o backend e o frontend (secção 5.1).
2. Escolher um `{sufixo}` (por exemplo, o identificador do cenário seguido da data e hora, `F002-01-1116-1530`) e calcular `{D}` a partir do desvio indicado.
3. Seguir os passos e comparar com o resultado esperado; um ponto que não se verifique é uma falha do cenário.

## 15. Decisões e pontos em aberto

| N.º  | Decisão                                                                                                                                                               | Razão                                                                                                                                                                                                                                                                                        |
| ---- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| E-01 | Os cenários de F004 e F006 descrevem as ações por objetivo e não pelos rótulos exatos de W13 e W16                                                                    | As I115, I116 e I118 estão em curso no Sprint 07; o comportamento esperado está fixado pelos requisitos e pela API v03 (secção 6)                                                                                                                                                            |
| E-02 | Não há cenários de eliminação de dados; a limpeza é feita pela reposição da base antes de cada execução completa                                                      | A API v03 não tem operações de eliminação de funcionários, Clientes, pacientes, marcações, registos clínicos ou administrações                                                                                                                                                               |
| E-03 | Os cenários que alteram a configuração ou o horário da clínica repõem os valores no fim e não correm em paralelo                                                      | São os únicos dados partilhados alterados pelos cenários; a disponibilidade e as marcações dependem do horário                                                                                                                                                                               |
| E-04 | Cada cenário que cria marcações usa um dia `{D}` com desvio próprio                                                                                                   | Evita que cenários em paralelo disputem a mesma vaga do mesmo Veterinário                                                                                                                                                                                                                    |
| E-05 | Os casos de erro 400, 403, 404, 409, 412 e 422 são verificados nos cenários das funcionalidades onde ocorrem, e não num cenário genérico                              | Cada erro tem significado só no contexto da operação (Plano de Testes, CT-WEB-TRV-04)                                                                                                                                                                                                        |
| E-06 | As mensagens de erro da API não confirmadas no código do frontend são indicadas pelo código HTTP e pelo assunto, e não pelo texto exato                               | O texto vem do campo `detail` do backend; a automação lê o texto do ecrã e o relatório de resultados (I133) regista-o                                                                                                                                                                        |
| E-07 | Não há cenário de sistema para a alteração de uma administração (vacinação ou desparasitação) nem para o novo cálculo da próxima administração depois dessa alteração | A alteração corresponde ao RF056 (EP-052), sem requisito aprovado na Especificação de Requisitos v02 (secção 3); os critérios da I118 sobre essa alteração são verificados na própria I118. Se o grupo aprovar o RF056, acrescenta-se um ST-WEB-F006-06 numa versão nova desta especificação |

| Ponto em aberto                                                                                    | Efeito                                                                                                                                                                                                                                                                                                                                                                                               | Seguimento                                                                        |
| -------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| Rótulos de W13 e W16                                                                               | Os passos de F004 e F006 confirmam-se quando as I115, I116 e I118 forem integradas. Na implementação das I115 e I116, o registo clínico cria-se a partir do detalhe da consulta (Agenda → detalhe → «Abrir registo clínico») e o «Registo clínico» da ficha do paciente é o histórico, só de leitura, com «Alterar registo»; a I130 segue esse caminho nos passos de ST-WEB-F004-01 e ST-WEB-F004-02 | I130 e I131; versão nova desta especificação só se o comportamento esperado mudar |
| Recarregar a página com o backend indisponível termina a sessão (observação M3 da revisão da I111) | ST-WEB-TRV-07 não recarrega a página com o backend parado; se o grupo decidir manter a sessão nesse caso, acrescenta-se um cenário                                                                                                                                                                                                                                                                   | I125 e I126 (validação dos fluxos integrados)                                     |
| Pontos «A confirmar» dos UC12 e UC13 (ponto em aberto 3 da Especificação de Requisitos v02)        | Podem alterar ST-WEB-F004-05 e ST-WEB-F004-07                                                                                                                                                                                                                                                                                                                                                        | Decisão do grupo no Backlog Refinement; versão nova desta especificação se mudar  |

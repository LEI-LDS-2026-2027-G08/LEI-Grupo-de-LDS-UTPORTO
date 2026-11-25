# Especificação dos Testes de Aceitação do Frontend Web — Gestão de Clínica Veterinária

| Campo                | Valor                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Grupo                | G08                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| Projeto              | Gestão de Clínica Veterinária                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| Artefacto            | Especificação dos Testes de Aceitação do Frontend Web (`OF-M4-014`)                                                                                                                                                                                                                                                                                                                                                                                                 |
| Ficheiro             | `m4-especificacao-testes-aceitacao-frontend-web-v01.md`                                                                                                                                                                                                                                                                                                                                                                                                             |
| Versão               | v01                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| Data                 | 2026-11-18                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| Milestone de Entrega | m4                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| Issue                | I134 — Especificar os testes de aceitação do frontend web (GitHub #122, Sprint 07)                                                                                                                                                                                                                                                                                                                                                                                  |
| Papéis               | Executor: Gonçalo Dinis Magalhães Teixeira · Revisor: Ivan Cristiano Silva Mendes · Auditor: André Filipe Moreira Mesquita                                                                                                                                                                                                                                                                                                                                          |
| Pasta de destino     | `/projeto/03-artefactos-de-qualidade-de-produto/03.02-frontend-web/03.02.03-testes-de-aceitacao`                                                                                                                                                                                                                                                                                                                                                                    |
| Fontes               | `m3-declaracao-ambito-v05.pdf` (I002); `m3-especificacao-requisitos-v02.md` (I012); `m3-especificacoes-casos-uso-v02.md` (I015); `m4-plano-testes-frontend-web-v01.md` (I043, secção 11); `m3-documentacao-api-v03.md`; `m4-especificacao-testes-sistema-frontend-web-v01.md` (I128); `m3-prototipo-baixa-fidelidade-v02.md` (I017); scripts de dados de demonstração (I061); `m4-backlog-projeto-v05.xlsx`; código do frontend web em `develop` (commit `1276a7d`) |

## Histórico de versões

| Versão | Data       | Autor                            | Alterações                                                                                                                                                   |
| ------ | ---------- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| v01    | 2026-11-18 | Gonçalo Dinis Magalhães Teixeira | Versão inicial: 19 cenários de aceitação para as funcionalidades F001, F002, F003, F004 e F006, critérios globais de aceitação e participantes da simulação. |

## Índice

1. [Introdução](#1-introdução)
2. [Documentos de referência](#2-documentos-de-referência)
3. [Âmbito](#3-âmbito)
4. [Como se lê um cenário](#4-como-se-lê-um-cenário)
5. [Preparação da simulação](#5-preparação-da-simulação)
6. [Participantes da simulação](#6-participantes-da-simulação)
7. [Critérios globais de aceitação](#7-critérios-globais-de-aceitação)
8. [Cenários — F001 Autenticação, contas e clínica](#8-cenários--f001-autenticação-contas-e-clínica)
9. [Cenários — F002 Pacientes](#9-cenários--f002-pacientes)
10. [Cenários — F003 Agendamento e consultas](#10-cenários--f003-agendamento-e-consultas)
11. [Cenários — F004 Registo clínico](#11-cenários--f004-registo-clínico)
12. [Cenários — F006 Boletim de vacinas e desparasitações](#12-cenários--f006-boletim-de-vacinas-e-desparasitações)
13. [Cenário — RNF003 O Cliente não usa a aplicação web](#13-cenário--rnf003-o-cliente-não-usa-a-aplicação-web)
14. [Matriz cenários ↔ funcionalidades e requisitos](#14-matriz-cenários--funcionalidades-e-requisitos)
15. [Execução manual e relação com os testes de sistema](#15-execução-manual-e-relação-com-os-testes-de-sistema)
16. [Decisões e pontos em aberto](#16-decisões-e-pontos-em-aberto)

## 1. Introdução

Este documento define o que os perfis internos da clínica (Administrador, Rececionista, Veterinário e Auxiliar Veterinário) teriam de conseguir fazer na aplicação web para a aceitarem. Cada cenário descreve uma situação real do dia a dia da clínica, do princípio ao fim, e diz o que tem de acontecer para o cenário ser aceite.

Os cenários estão escritos na perspetiva de quem trabalha na clínica, sem termos técnicos: quem os executa só precisa de saber usar a aplicação. A rastreabilidade aos requisitos, às funcionalidades e aos casos de uso fica nos campos de cada cenário e na matriz da secção 14.

A simulação da aceitação, com a execução destes cenários e o relatório, é feita na I135, no Sprint 08, sobre a versão candidata à entrega. A simulação não equivale a uma homologação por um cliente real (Plano de Testes do Frontend Web, secção 11).

## 2. Documentos de referência

| Documento                                                      | Uso nesta especificação                                                                                                                         |
| -------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| Declaração de Âmbito v05 (I002)                                | Funcionalidades ativas (F001, F002, F003, F004 e F006) e funcionalidades fora do âmbito (F005 e F007)                                           |
| Especificação de Requisitos v02 (I012)                         | Requisitos funcionais e não funcionais e respetivos critérios de aceitação                                                                      |
| Especificações de Casos de Uso v02 (I015)                      | Fluxos de negócio que dão origem aos cenários (UC01 a UC07, UC09, UC10, UC12, UC13 e UC16)                                                      |
| Plano de Testes do Frontend Web v01 (I043)                     | Secção 11 (participantes, cenários, versão e resultado da simulação), convenção `AT-WEB-nn` e critérios de entrada da simulação (secção 8.1)    |
| Especificação dos Testes de Sistema do Frontend Web v01 (I128) | Contas, dados de demonstração e cenários de sistema que cobrem automaticamente os mesmos fluxos                                                 |
| Dados de demonstração (I061)                                   | Contas, Clientes, pacientes, registos clínicos e boletins usados nos cenários                                                                   |
| Documentação da API v03                                        | Regras de validação e pontos em aberto sobre a informação clínica lida pelo Auxiliar Veterinário e por outros Veterinários (secção 13, ponto 4) |

## 3. Âmbito

| Incluído                                                                     | Excluído                                                                                                                          |
| ---------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| F001 — Autenticação, contas de funcionários e de Clientes e dados da clínica | F005 — Gestão de contas e pagamentos (fora do âmbito, Declaração de Âmbito v05): **não há nenhum cenário**                        |
| F002 — Pacientes                                                             | F007 — Internamento (fora do âmbito, Declaração de Âmbito v05): **não há nenhum cenário**                                         |
| F003 — Agendamento e consultas                                               | Funcionalidades do Cliente na aplicação móvel (RF018, RF028, RF029 e RF047): especificação de aceitação do frontend mobile (I140) |
| F004 — Registo clínico                                                       | RF053 a RF056: sem requisito aprovado na Especificação de Requisitos v02                                                          |
| F006 — Boletim de vacinas e desparasitações                                  | Desempenho, segurança ofensiva, acessibilidade formal e funcionamento sem rede                                                    |
| RNF003 — o Cliente não usa a aplicação web                                   |                                                                                                                                   |

## 4. Como se lê um cenário

| Campo                                    | Conteúdo                                                                                                                                    |
| ---------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| Identificador                            | `AT-WEB-nn` (Plano de Testes do Frontend Web, secção 4.1)                                                                                   |
| Funcionalidade, requisitos e caso de uso | Origem do cenário, para rastreabilidade                                                                                                     |
| Perfil                                   | Papel que quem executa o cenário assume na clínica                                                                                          |
| Situação                                 | A história do dia a dia que o cenário representa                                                                                            |
| Pré-condições                            | O que tem de existir antes de começar                                                                                                       |
| Dados                                    | Os valores a usar; `{data}` é a data da sessão de simulação no formato AAAAMMDD, para que os nomes criados nunca se repitam entre sessões   |
| Passos                                   | O que a pessoa faz, por ordem, com os nomes que aparecem no ecrã                                                                            |
| Critério de aceitação                    | O que tem de acontecer para o cenário ser **Aceite**; se algum ponto falhar, o cenário é **Não aceite** ou **Aceite com desvio** (secção 7) |

Os nomes entre «» são os textos que aparecem no ecrã. Nenhum passo pede para esperar um tempo fixo: quando um passo depende de uma resposta da aplicação, o passo seguinte só começa quando essa resposta aparece no ecrã.

## 5. Preparação da simulação

1. **Versão:** antes da sessão, o grupo identifica o commit do frontend e o do backend que vão ser usados e regista-os no relatório (I135). Os testes de sistema dessa versão têm de estar a passar (critério de entrada da secção 8.1 do Plano de Testes).
2. **Ambiente:** o ambiente local descrito no Plano de Testes (secção 6.1) ou o ambiente público de avaliação, com a base reposta e os dados de demonstração carregados no início da sessão.
3. **Contas:** as contas de demonstração da Especificação dos Testes de Sistema (secção 5.2). As palavras-passe são comunicadas aos participantes fora do repositório e não ficam escritas no relatório.
4. **Ordem:** os cenários podem ser executados por qualquer ordem. Cada cenário cria os dados de que precisa ou usa só dados de demonstração que nenhum outro cenário altera, exceto o AT-WEB-07, que repõe no fim os dados da clínica que alterar.
5. **Registo:** quem regista anota, por cenário, o executor, a hora de início e de fim, o resultado (Aceite, Aceite com desvio ou Não aceite) e, em cada desvio, o que aconteceu, o que era esperado e a decisão tomada.

## 6. Participantes da simulação

Participam os quatro elementos do grupo. Cada cenário tem:

- um **executor**, que assume o perfil indicado e segue os passos sem ajuda de quem implementou a funcionalidade;
- um **registador**, que lê os passos, confirma o critério de aceitação e anota o resultado.

Regra: o executor de um cenário **não implementou** a funcionalidade nele usada (Plano de Testes, secção 11). A proposta seguinte cumpre essa regra com as atribuições dos Sprints 06 e 07; a I135 confirma-a ou ajusta-a no início da sessão, mantendo a regra.

| Cenários                        | Funcionalidade usada e quem a implementou                                | Executor proposto                | Registador proposto              |
| ------------------------------- | ------------------------------------------------------------------------ | -------------------------------- | -------------------------------- |
| AT-WEB-01, AT-WEB-02            | Autenticação e sessão — Ivan Cristiano Silva Mendes (I107, I108)         | André Filipe Moreira Mesquita    | Gonçalo Magalhães Ribeiro        |
| AT-WEB-03, AT-WEB-04            | Funcionários — Gonçalo Dinis Magalhães Teixeira (I109)                   | Gonçalo Magalhães Ribeiro        | André Filipe Moreira Mesquita    |
| AT-WEB-05, AT-WEB-06            | Clientes — André Filipe Moreira Mesquita (I111)                          | Gonçalo Magalhães Ribeiro        | Ivan Cristiano Silva Mendes      |
| AT-WEB-07                       | Clínica e horários — Ivan Cristiano Silva Mendes (I110)                  | Gonçalo Dinis Magalhães Teixeira | André Filipe Moreira Mesquita    |
| AT-WEB-08, AT-WEB-09            | Pacientes — André Filipe Moreira Mesquita (I112)                         | Ivan Cristiano Silva Mendes      | Gonçalo Dinis Magalhães Teixeira |
| AT-WEB-10, AT-WEB-11            | Marcações — André Filipe Moreira Mesquita (I113)                         | Gonçalo Magalhães Ribeiro        | Gonçalo Dinis Magalhães Teixeira |
| AT-WEB-12, AT-WEB-13, AT-WEB-14 | Agenda, chegada e apoio — André Filipe Moreira Mesquita (I114)           | Gonçalo Dinis Magalhães Teixeira | Ivan Cristiano Silva Mendes      |
| AT-WEB-15, AT-WEB-16, AT-WEB-17 | Registo e histórico clínico — André Filipe Moreira Mesquita (I115, I116) | Ivan Cristiano Silva Mendes      | Gonçalo Magalhães Ribeiro        |
| AT-WEB-18                       | Vacinação e desparasitação — André Filipe Moreira Mesquita (I118)        | Gonçalo Magalhães Ribeiro        | Ivan Cristiano Silva Mendes      |
| AT-WEB-19                       | Autenticação (Cliente recusado) — Ivan Cristiano Silva Mendes (I108)     | André Filipe Moreira Mesquita    | Gonçalo Dinis Magalhães Teixeira |

Os cenários com mais do que um perfil (por exemplo, AT-WEB-14) são executados pelo mesmo executor, que muda de conta quando o passo o indica.

## 7. Critérios globais de aceitação

| N.º   | Critério                                                                                                                                                                  | Como se verifica                                                                                      |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| CG-01 | Todos os 19 cenários desta especificação foram executados sobre a mesma versão identificada do frontend e do backend                                                      | Relatório da simulação (I135): commits e data da sessão                                               |
| CG-02 | Nenhum cenário fica **Não aceite**                                                                                                                                        | Resultado de cada cenário no relatório                                                                |
| CG-03 | Cada cenário **Aceite com desvio** tem decisão registada: corrigido e repetido antes da entrega, ou registado como limitação conhecida nas Notas de Release finais (I056) | Coluna de decisão do relatório; Notas de Release                                                      |
| CG-04 | Nenhum desvio impede um perfil de concluir o trabalho do cenário (por exemplo, perda de dados gravados, acesso a informação de outro perfil ou impossibilidade de gravar) | Descrição dos desvios no relatório                                                                    |
| CG-05 | Em nenhum cenário um perfil vê ou faz algo que não lhe é permitido                                                                                                        | Critérios de aceitação dos cenários AT-WEB-01, AT-WEB-13, AT-WEB-14, AT-WEB-17, AT-WEB-18 e AT-WEB-19 |
| CG-06 | Cada cenário foi executado por quem não implementou a funcionalidade usada                                                                                                | Executor e registador de cada cenário no relatório (secção 6)                                         |

A aplicação web é **aceite** quando os seis critérios se verificam. Os resultados possíveis de cada cenário são os do Plano de Testes (secção 11): **Aceite** (todos os pontos do critério de aceitação se verificam), **Aceite com desvio** (o trabalho do cenário fica feito, mas algum ponto não se verifica exatamente como descrito) e **Não aceite** (o trabalho do cenário não fica feito ou a aplicação permite algo que não devia).

## 8. Cenários — F001 Autenticação, contas e clínica

### AT-WEB-01 — Começar e terminar o dia de trabalho na aplicação

| Funcionalidade | Requisitos                 | Caso de uso | Perfil                                                         |
| -------------- | -------------------------- | ----------- | -------------------------------------------------------------- |
| F001           | RF001, RF003, RF004, RF005 | UC01, UC02  | Administrador, Rececionista, Veterinário, Auxiliar Veterinário |

- **Situação:** cada funcionário entra na aplicação no início do dia, trabalha só com o que lhe compete e sai no fim.
- **Pré-condições:** contas de demonstração ativas; nenhuma sessão aberta no navegador.
- **Dados:** as contas `admin@`, `rececao@`, `vet.ana@` e `aux.carla@` da clínica de demonstração.
- **Passos:** para cada uma das quatro contas:
  1. Abrir a aplicação, escrever o e-mail e a palavra-passe e carregar em «Entrar».
  2. Ver o nome e o perfil no topo e as opções do menu.
  3. Abrir duas opções do menu e recarregar a página.
  4. Carregar em «Terminar sessão» e, a seguir, no botão de voltar atrás do navegador.
- **Critério de aceitação:**
  1. Cada pessoa entra sem erros e vê o seu nome e o seu perfil.
  2. O menu só tem o trabalho desse perfil: Administrador — Clientes, Funcionários e Clínica e horários; Rececionista — Agenda, Pacientes e Clientes; Veterinário — Agenda e Pacientes; Auxiliar — Preparação de consultas.
  3. Recarregar a página não obriga a entrar outra vez.
  4. Depois de terminar a sessão, voltar atrás não mostra nenhuma página da clínica: aparece o ecrã «Iniciar sessão».

### AT-WEB-02 — Impedir a entrada com dados errados ou com a conta bloqueada

| Funcionalidade | Requisitos   | Caso de uso | Perfil       |
| -------------- | ------------ | ----------- | ------------ |
| F001           | RF002, RF009 | UC01        | Rececionista |

- **Situação:** alguém engana-se na palavra-passe; um antigo funcionário, com a conta bloqueada, tenta entrar.
- **Pré-condições:** nenhuma sessão aberta; a conta `rececao.bloqueada@` está bloqueada nos dados de demonstração.
- **Dados:** `rececao@` com uma palavra-passe errada; `rececao.bloqueada@` com a palavra-passe correta.
- **Passos:**
  1. Tentar entrar com a conta `rececao@` e a palavra-passe errada.
  2. Tentar entrar com a conta bloqueada.
- **Critério de aceitação:** nos dois casos a pessoa não entra e a aplicação explica porquê: «Credenciais inválidas.» no primeiro e «A conta está bloqueada. Contacte a clínica.» no segundo. A mensagem do primeiro caso não revela se o e-mail existe.

### AT-WEB-03 — Admitir um novo Auxiliar Veterinário com autorização de apoio

| Funcionalidade | Requisitos                 | Caso de uso | Perfil                                |
| -------------- | -------------------------- | ----------- | ------------------------------------- |
| F001           | RF006, RF007, RF008, RF005 | UC03        | Administrador; depois o novo Auxiliar |

- **Situação:** a clínica contratou um Auxiliar Veterinário que vai poder deixar notas de preparação das consultas.
- **Pré-condições:** sessão do Administrador.
- **Dados:** nome `Auxiliar Aceitação {data}`, e-mail `aux.aceitacao.{data}@clinica-g08.test`, telefone 930000001, perfil Auxiliar Veterinário e uma palavra-passe inicial forte combinada no momento.
- **Passos:**
  1. Em «Funcionários», carregar em «Novo funcionário», preencher os dados, marcar «Pode registar informação não clínica de apoio» e guardar.
  2. Procurar o novo funcionário na lista.
  3. Abrir a ficha, mudar o telefone para 930000002 e guardar.
  4. Terminar a sessão e entrar com a conta do novo Auxiliar.
- **Critério de aceitação:**
  1. A conta é criada com o perfil Auxiliar Veterinário, estado Ativo e «Registo de apoio» = «Autorizado».
  2. A alteração do telefone fica gravada.
  3. O novo Auxiliar entra e vê apenas «Preparação de consultas».

### AT-WEB-04 — Bloquear a conta de um funcionário que saiu da clínica

| Funcionalidade | Requisitos   | Caso de uso | Perfil        |
| -------------- | ------------ | ----------- | ------------- |
| F001           | RF009, RF002 | UC03        | Administrador |

- **Situação:** um funcionário deixou a clínica e não pode voltar a entrar na aplicação.
- **Pré-condições:** sessão do Administrador; uma conta criada para o efeito, como no AT-WEB-03 (por exemplo, `Saída Aceitação {data}`, perfil Rececionista).
- **Dados:** motivo «Deixou a clínica».
- **Passos:**
  1. Em «Funcionários», procurar a conta e carregar em «Bloquear».
  2. Escrever o motivo e confirmar o bloqueio.
  3. Abrir a ficha da conta.
  4. Terminar a sessão e tentar entrar com a conta bloqueada.
- **Critério de aceitação:** a aplicação pede confirmação antes de bloquear; a conta fica Bloqueada, a ficha mostra o motivo e a conta já não consegue entrar («A conta está bloqueada. Contacte a clínica.»).

### AT-WEB-05 — Registar um novo Cliente com acesso à aplicação móvel

| Funcionalidade | Requisitos | Caso de uso | Perfil       |
| -------------- | ---------- | ----------- | ------------ |
| F001           | RF010      | UC04        | Rececionista |

- **Situação:** um tutor chega pela primeira vez à clínica e quer acompanhar os seus animais na aplicação móvel.
- **Pré-condições:** sessão da Rececionista.
- **Dados:** nome `Cliente Aceitação {data}`, telefone 931000001, e-mail `cliente.aceitacao.{data}@clinica-g08.test`, uma palavra-passe inicial forte combinada no momento; sem NIF nem morada.
- **Passos:**
  1. Em «Clientes», carregar em «Novo cliente».
  2. Carregar em «Registar cliente» sem preencher nada.
  3. Preencher os dados e carregar em «Registar cliente».
  4. Voltar à lista e procurar o Cliente pelo nome.
- **Critério de aceitação:**
  1. Com o formulário vazio, a aplicação indica os campos obrigatórios em falta e não regista nada.
  2. Com os dados preenchidos, abre a ficha do Cliente com a confirmação do registo e da conta de acesso, «Estado da conta: Ativo» e «Este cliente não tem animais.».
  3. O Cliente aparece na pesquisa.

### AT-WEB-06 — Atualizar os contactos de um Cliente

| Funcionalidade | Requisitos | Caso de uso | Perfil       |
| -------------- | ---------- | ----------- | ------------ |
| F001           | RF011      | UC04        | Rececionista |

- **Situação:** o tutor mudou de telefone e de morada.
- **Pré-condições:** sessão da Rececionista; o Cliente do AT-WEB-05 ou outro criado da mesma forma.
- **Dados:** telefone 931000002; morada «Rua da Aceitação, 1, 4470-000 Maia».
- **Passos:**
  1. Abrir a ficha do Cliente, alterar o telefone e a morada e carregar em «Guardar alterações».
  2. Recarregar a página.
- **Critério de aceitação:** aparece «Alterações guardadas.» e, depois de recarregar, a ficha mostra o novo telefone e a nova morada; a ficha não pede a palavra-passe do Cliente.

### AT-WEB-07 — Atualizar os dados e o horário da clínica

| Funcionalidade | Requisitos          | Caso de uso | Perfil        |
| -------------- | ------------------- | ----------- | ------------- |
| F001, F003     | RF012, RF013, RF019 | UC05, UC09  | Administrador |

- **Situação:** a clínica passa a abrir ao domingo de manhã e quer consultas de 45 minutos.
- **Pré-condições:** sessão do Administrador; dados e horário da clínica iguais aos de demonstração.
- **Dados:** duração da consulta 45 minutos; domingo das 10:00 às 12:00; tentativa de um segundo período ao domingo das 11:00 às 13:00.
- **Passos:**
  1. Abrir «Clínica e horários» e anotar os valores atuais.
  2. Mudar a duração da consulta, acrescentar o período de domingo e acrescentar também o segundo período de domingo; guardar.
  3. Remover o segundo período de domingo e guardar.
  4. Recarregar a página.
  5. Repor a duração e o horário anotados no passo 1 e guardar.
- **Critério de aceitação:**
  1. No passo 2 a aplicação não grava e explica que os dois períodos de domingo se sobrepõem.
  2. No passo 3 aparece «Alterações guardadas.» e, depois de recarregar, a duração é 45 e o domingo tem 10:00–12:00.
  3. No passo 5 os dados voltam aos originais.

## 9. Cenários — F002 Pacientes

### AT-WEB-08 — Registar o animal de um Cliente e encontrá-lo pelo tutor

| Funcionalidade | Requisitos          | Caso de uso | Perfil       |
| -------------- | ------------------- | ----------- | ------------ |
| F002           | RF014, RF015, RF017 | UC06, UC07  | Rececionista |

- **Situação:** um Cliente traz um cão novo e a Rececionista cria a ficha do animal; mais tarde, procura os animais desse Cliente.
- **Pré-condições:** sessão da Rececionista; um Cliente criado como no AT-WEB-05.
- **Dados:** nome do animal `Rex Aceitação {data}`, Cão, Labrador Retriever, Macho, nascido a 01/01/2023, 10,5 kg.
- **Passos:**
  1. Em «Pacientes», carregar em «Novo paciente», escolher o Cliente como proprietário e preencher os dados do animal; guardar.
  2. Voltar a «Pacientes», procurar o proprietário pelo nome e selecioná-lo.
  3. Abrir a ficha do animal a partir da lista.
- **Critério de aceitação:**
  1. A ficha do animal é criada e ligada ao Cliente, com espécie, raça, sexo, data de nascimento, peso e idade calculada.
  2. A pesquisa pelo proprietário mostra só os animais desse Cliente.
  3. A ficha vista pela Rececionista mostra apenas dados administrativos, sem informação clínica.

### AT-WEB-09 — Atualizar o peso de um paciente durante a consulta

| Funcionalidade | Requisitos   | Caso de uso | Perfil      |
| -------------- | ------------ | ----------- | ----------- |
| F002           | RF016, RF015 | UC07        | Veterinário |

- **Situação:** na consulta, o Veterinário pesa o animal e corrige o peso na ficha.
- **Pré-condições:** sessão do Veterinário (`vet.ana@`); o animal do AT-WEB-08 ou outro criado da mesma forma.
- **Dados:** novo peso 11,2 kg; um peso inválido de 0 kg.
- **Passos:**
  1. Abrir a ficha do animal e escrever 0 no peso; tentar guardar.
  2. Escrever 11,2 e guardar.
  3. Recarregar a página.
- **Critério de aceitação:** o peso 0 é recusado com uma explicação e nada é gravado; o peso 11,2 fica gravado e continua visível depois de recarregar.

## 10. Cenários — F003 Agendamento e consultas

### AT-WEB-10 — Marcar uma consulta numa vaga livre

| Funcionalidade | Requisitos   | Caso de uso | Perfil       |
| -------------- | ------------ | ----------- | ------------ |
| F003           | RF020, RF021 | UC09        | Rececionista |

- **Situação:** um tutor telefona a pedir uma consulta com a Dra. Ana na próxima semana.
- **Pré-condições:** sessão da Rececionista; o animal do AT-WEB-08 ou outro criado da mesma forma.
- **Dados:** Dra. Ana; um dia útil da semana seguinte à sessão; motivo «Consulta de rotina».
- **Passos:**
  1. Em «Agenda», carregar em «Nova marcação», procurar o animal e escolher a Dra. Ana e o dia.
  2. Carregar em «Consultar disponibilidade» e escolher um dos horários livres.
  3. Escrever o motivo e criar a marcação.
  4. Abrir a agenda desse dia.
- **Critério de aceitação:**
  1. Só aparecem horários dentro do horário de funcionamento e sem consultas já marcadas para a Dra. Ana.
  2. A marcação fica criada no estado Pendente e aparece na agenda do dia com o animal, o Cliente, a Dra. Ana, o motivo e a hora escolhida.

### AT-WEB-11 — Remarcar, confirmar e cancelar consultas

| Funcionalidade | Requisitos   | Caso de uso | Perfil       |
| -------------- | ------------ | ----------- | ------------ |
| F003           | RF022, RF023 | UC09        | Rececionista |

- **Situação:** o tutor pede para mudar a hora da consulta e depois confirma-a; outra consulta é cancelada a pedido do tutor.
- **Pré-condições:** sessão da Rececionista; duas marcações Pendentes criadas como no AT-WEB-10.
- **Dados:** novo horário livre no mesmo dia; motivo de cancelamento «Pedido do tutor».
- **Passos:**
  1. Abrir a primeira marcação, carregar em «Editar marcação», escolher outro horário livre e guardar.
  2. Confirmar a marcação.
  3. Abrir a segunda marcação e cancelá-la com o motivo.
- **Critério de aceitação:**
  1. A primeira marcação passa a ter o novo horário e fica Confirmada.
  2. A segunda fica Cancelada, com o motivo visível, e deixa de ter opções para alterar ou mudar de estado.
  3. Numa marcação Pendente, a aplicação só oferece confirmar ou cancelar (não oferece «Marcar como realizada»).

### AT-WEB-12 — Receber o animal no dia da consulta

| Funcionalidade | Requisitos   | Caso de uso | Perfil       |
| -------------- | ------------ | ----------- | ------------ |
| F003           | RF024, RF025 | UC09        | Rececionista |

- **Situação:** o tutor chega à clínica com o animal para a consulta marcada.
- **Pré-condições:** sessão da Rececionista; uma marcação Confirmada (AT-WEB-11 ou criada da mesma forma).
- **Dados:** a marcação Confirmada.
- **Passos:**
  1. Abrir a agenda do dia da marcação e filtrar pela Dra. Ana.
  2. Abrir a marcação e carregar em «Registar chegada».
  3. Voltar à agenda.
- **Critério de aceitação:** a agenda mostra as consultas do dia filtradas pela Veterinária; a chegada fica registada com a hora; a marcação continua Confirmada e a hora de chegada aparece também na agenda. Numa marcação Pendente não existe a opção «Registar chegada».

### AT-WEB-13 — Ver as próprias consultas do dia

| Funcionalidade | Requisitos   | Caso de uso | Perfil      |
| -------------- | ------------ | ----------- | ----------- |
| F003           | RF024, RF005 | UC09, UC10  | Veterinário |

- **Situação:** a Dra. Ana começa o dia e quer ver as consultas que lhe estão atribuídas.
- **Pré-condições:** no dia escolhido existem marcações da Dra. Ana e do Dr. Bruno (criadas como no AT-WEB-10).
- **Dados:** conta `vet.ana@`.
- **Passos:**
  1. Entrar como Dra. Ana e abrir «Agenda» no dia escolhido.
  2. Abrir uma das consultas.
- **Critério de aceitação:** a Dra. Ana vê só as suas consultas (as do Dr. Bruno não aparecem), sem a opção de criar marcações; o detalhe da consulta não tem ações de agenda e permite abrir a ficha do paciente.

### AT-WEB-14 — Preparar a consulta e deixar uma nota de apoio

| Funcionalidade | Requisitos          | Caso de uso | Perfil                                                   |
| -------------- | ------------------- | ----------- | -------------------------------------------------------- |
| F003           | RF026, RF027, RF005 | UC10        | Auxiliar Veterinário com e sem autorização; Rececionista |

- **Situação:** a Auxiliar Carla prepara a sala e deixa uma nota para a equipa; o Auxiliar Diogo, que não tem essa autorização, tenta fazer o mesmo.
- **Pré-condições:** uma marcação Confirmada (como no AT-WEB-11).
- **Dados:** nota «Sala 2 preparada — aceitação {data}».
- **Passos:**
  1. Entrar como Carla, abrir «Preparação de consultas», ir ao dia da marcação e selecionar a consulta.
  2. Escrever a nota e guardar.
  3. Terminar a sessão, entrar como Diogo, selecionar a mesma consulta e tentar guardar uma nota.
  4. Terminar a sessão, entrar como Rececionista e abrir a marcação.
- **Critério de aceitação:**
  1. A Carla vê a informação de preparação (consulta, Veterinário, motivo, animal e contacto do tutor) sem informação clínica e sem opções de agenda.
  2. A nota da Carla fica gravada com o nome dela, a data e a hora.
  3. O Diogo não consegue gravar e a aplicação explica que a conta dele não tem essa autorização.
  4. A Rececionista vê a nota da Carla na marcação, que continua Confirmada.

## 11. Cenários — F004 Registo clínico

Os ecrãs do registo clínico (I115 e I116) estão a ser implementados no Sprint 07. Os passos deste capítulo descrevem o que a pessoa quer fazer; os nomes exatos dos botões confirmam-se na versão candidata, antes da sessão (secção 16).

### AT-WEB-15 — Registar e corrigir o registo clínico de uma consulta

| Funcionalidade | Requisitos                        | Caso de uso | Perfil      |
| -------------- | --------------------------------- | ----------- | ----------- |
| F004           | RF030, RF031, RF032, RF033, RF034 | UC12        | Veterinário |

- **Situação:** no fim da consulta, a Dra. Ana regista o que observou, o diagnóstico, o tratamento feito e a medicação; mais tarde corrige o diagnóstico.
- **Pré-condições:** sessão da Dra. Ana; uma consulta da Dra. Ana Confirmada (como no AT-WEB-11) para um animal criado na sessão.
- **Dados:** notas «Animal ativo; ouvido esquerdo inflamado»; diagnóstico «Otite externa»; tratamento «Limpeza auricular», aplicado hoje; medicação «Gotas auriculares», 3 gotas, 2 vezes por dia, durante 7 dias; diagnóstico corrigido «Otite externa bilateral».
- **Passos:**
  1. Abrir a ficha do animal e o registo clínico; escolher a consulta.
  2. Registar as notas, o diagnóstico, o tratamento e a medicação e gravar.
  3. Tentar gravar uma segunda medicação sem dose.
  4. Abrir o registo, corrigir o diagnóstico, gravar e recarregar a página.
- **Critério de aceitação:**
  1. O registo fica ligado à consulta e ao animal, com as notas, o diagnóstico, o tratamento e a medicação introduzidos.
  2. A medicação sem dose é recusada com uma explicação, sem perder o que já estava escrito.
  3. Depois de recarregar, o registo mostra o diagnóstico corrigido e continua ligado à mesma consulta e ao mesmo animal.

### AT-WEB-16 — Consultar o histórico clínico de um paciente

| Funcionalidade | Requisitos | Caso de uso | Perfil      |
| -------------- | ---------- | ----------- | ----------- |
| F004           | RF035      | UC12        | Veterinário |

- **Situação:** antes de uma nova consulta do Bobi, a Dra. Ana revê o que já foi registado sobre ele.
- **Pré-condições:** sessão da Dra. Ana; o registo clínico do Bobi de 14/10/2026 dos dados de demonstração.
- **Dados:** paciente Bobi.
- **Passos:**
  1. Abrir a ficha do Bobi e o registo clínico.
  2. Abrir o registo de 14/10/2026.
- **Critério de aceitação:** o histórico mostra os registos do Bobi do mais recente para o mais antigo, cada um com a data e o Veterinário; o registo de 14/10/2026 mostra o diagnóstico «Otite externa» e o tratamento feito.

### AT-WEB-17 — Consultar só a informação clínica autorizada

| Funcionalidade | Requisitos   | Caso de uso | Perfil                             |
| -------------- | ------------ | ----------- | ---------------------------------- |
| F004           | RF036, RF005 | UC13        | Auxiliar Veterinário; Rececionista |

- **Situação:** a Auxiliar Carla precisa de saber que tratamento e que medicação foram indicados ao Bobi para preparar a próxima consulta; a Rececionista não deve ver informação clínica.
- **Pré-condições:** o registo clínico do Bobi de 14/10/2026 dos dados de demonstração.
- **Dados:** contas `aux.carla@` e `rececao@`; paciente Bobi.
- **Regra em vigor:** o Auxiliar Veterinário lê o registo clínico completo, só em leitura (UC13; Documentação da API v03, EP-033 e EP-034).
- **Passos:**
  1. Entrar como Carla, abrir a ficha do Bobi e o registo clínico.
  2. Abrir o registo de 14/10/2026.
  3. Terminar a sessão, entrar como Rececionista e abrir a ficha do Bobi.
- **Critério de aceitação:**
  1. **A Carla vê** o histórico do Bobi, com a data, o Veterinário e o diagnóstico de cada registo.
  2. **A Carla vê**, no registo de 14/10/2026, a Dra. Ana, as notas clínicas, o diagnóstico «Otite externa», o tratamento «Limpeza auricular com solução fisiológica» e a prescrição «Gotas auriculares (fictício)», «3 gotas no ouvido esquerdo», «2 vezes por dia», durante 7 dias.
  3. **Fica oculto para a Carla:** qualquer opção para criar um registo clínico ou para alterar notas, diagnóstico, tratamentos ou prescrições, e a ligação para «Vacinação».
  4. **Fica oculto para a Rececionista:** a ficha do Bobi não mostra a área clínica nem nenhuma ligação para o registo clínico.
- **Se a regra mudar antes da simulação:** se o grupo aprovar uma versão reduzida da informação clínica para o Auxiliar (secção 16, D-03), este cenário é atualizado numa nova versão desta especificação, revista antes da sessão: o ponto 2 passa a listar só os campos aprovados e o ponto 3 passa a incluir os campos que deixam de ser mostrados. A I135 executa sempre o cenário com a versão que corresponde à regra em vigor no dia da sessão.

## 12. Cenários — F006 Boletim de vacinas e desparasitações

Os ecrãs do boletim (I118) estão a ser implementados no Sprint 07. Os passos descrevem o que a pessoa quer fazer; os nomes exatos dos botões confirmam-se na versão candidata (secção 16).

### AT-WEB-18 — Vacinar e desparasitar um animal e consultar o boletim

| Funcionalidade | Requisitos                        | Caso de uso | Perfil                    |
| -------------- | --------------------------------- | ----------- | ------------------------- |
| F006           | RF043, RF044, RF045, RF046, RF005 | UC16        | Veterinário; Rececionista |

- **Situação:** a Dra. Ana vacina um animal e marca a próxima dose para daqui a 365 dias; aplica também um desparasitante sem próxima toma prevista.
- **Pré-condições:** sessão da Dra. Ana; um animal criado na sessão (como no AT-WEB-08).
- **Dados:** vacina «Vacina polivalente», lote «PV-ACEIT», dada hoje, intervalo de 365 dias até à próxima dose; desparasitante «Desparasitante interno», lote «DP-ACEIT», dado hoje, sem intervalo; uma tentativa de vacina com data de amanhã.
- **Passos:**
  1. Abrir a ficha do animal e a vacinação; registar a vacina e gravar.
  2. Registar o desparasitante e gravar.
  3. Tentar registar uma vacina com a data de amanhã.
  4. Ver o boletim do animal.
  5. Terminar a sessão, entrar como Rececionista e abrir a ficha do mesmo animal.
- **Critério de aceitação:**
  1. A vacina fica no boletim com a data de hoje, o lote e a próxima dose prevista para a data de administração + 365 dias.
  2. O desparasitante fica no boletim sem próxima toma.
  3. A vacina com data futura é recusada com uma explicação.
  4. O boletim mostra as duas administrações e as próximas previstas.
  5. A Rececionista não tem acesso ao boletim na ficha do animal.

## 13. Cenário — RNF003 O Cliente não usa a aplicação web

### AT-WEB-19 — O Cliente usa a aplicação móvel, não a web

| Funcionalidade | Requisitos    | Caso de uso | Perfil  |
| -------------- | ------------- | ----------- | ------- |
| F001           | RNF003, RF005 | UC01        | Cliente |

- **Situação:** um tutor com conta de acesso tenta entrar no site da clínica.
- **Pré-condições:** nenhuma sessão aberta.
- **Dados:** conta `cliente.joao@`.
- **Passos:**
  1. Abrir a aplicação web e tentar entrar com a conta do Cliente.
- **Critério de aceitação:** a pessoa não entra e a aplicação indica «Os Clientes não têm acesso à aplicação web. Utilize a aplicação móvel.».

## 14. Matriz cenários ↔ funcionalidades e requisitos

### 14.1. Por cenário

| Cenário   | Funcionalidade | Requisitos                        | Caso de uso | Perfil                                                         |
| --------- | -------------- | --------------------------------- | ----------- | -------------------------------------------------------------- |
| AT-WEB-01 | F001           | RF001, RF003, RF004, RF005        | UC01, UC02  | Administrador, Rececionista, Veterinário, Auxiliar Veterinário |
| AT-WEB-02 | F001           | RF002, RF009                      | UC01        | Rececionista                                                   |
| AT-WEB-03 | F001           | RF005, RF006, RF007, RF008        | UC03        | Administrador; depois o novo Auxiliar                          |
| AT-WEB-04 | F001           | RF002, RF009                      | UC03        | Administrador                                                  |
| AT-WEB-05 | F001           | RF010                             | UC04        | Rececionista                                                   |
| AT-WEB-06 | F001           | RF011                             | UC04        | Rececionista                                                   |
| AT-WEB-07 | F001, F003     | RF012, RF013, RF019               | UC05, UC09  | Administrador                                                  |
| AT-WEB-08 | F002           | RF014, RF015, RF017               | UC06, UC07  | Rececionista                                                   |
| AT-WEB-09 | F002           | RF015, RF016                      | UC07        | Veterinário                                                    |
| AT-WEB-10 | F003           | RF020, RF021                      | UC09        | Rececionista                                                   |
| AT-WEB-11 | F003           | RF022, RF023                      | UC09        | Rececionista                                                   |
| AT-WEB-12 | F003           | RF024, RF025                      | UC09        | Rececionista                                                   |
| AT-WEB-13 | F003           | RF005, RF024                      | UC09, UC10  | Veterinário                                                    |
| AT-WEB-14 | F003           | RF005, RF026, RF027               | UC10        | Auxiliar Veterinário com e sem autorização; Rececionista       |
| AT-WEB-15 | F004           | RF030, RF031, RF032, RF033, RF034 | UC12        | Veterinário                                                    |
| AT-WEB-16 | F004           | RF035                             | UC12        | Veterinário                                                    |
| AT-WEB-17 | F004           | RF005, RF036                      | UC13        | Auxiliar Veterinário; Rececionista                             |
| AT-WEB-18 | F006           | RF005, RF043, RF044, RF045, RF046 | UC16        | Veterinário; Rececionista                                      |
| AT-WEB-19 | F001           | RF005, RNF003                     | UC01        | Cliente                                                        |

### 14.2. Por requisito

Todos os 37 requisitos funcionais da aplicação web em âmbito e o RNF003 têm pelo menos um cenário de aceitação. Não há nenhum cenário para F005 nem para F007.

| Funcionalidade | Requisito | Cenários                                                                    |
| -------------- | --------- | --------------------------------------------------------------------------- |
| F001           | RF001     | AT-WEB-01                                                                   |
| F001           | RF002     | AT-WEB-02, AT-WEB-04                                                        |
| F001           | RF003     | AT-WEB-01                                                                   |
| F001           | RF004     | AT-WEB-01                                                                   |
| F001           | RF005     | AT-WEB-01, AT-WEB-03, AT-WEB-13, AT-WEB-14, AT-WEB-17, AT-WEB-18, AT-WEB-19 |
| F001           | RF006     | AT-WEB-03                                                                   |
| F001           | RF007     | AT-WEB-03                                                                   |
| F001           | RF008     | AT-WEB-03                                                                   |
| F001           | RF009     | AT-WEB-02, AT-WEB-04                                                        |
| F001           | RF010     | AT-WEB-05                                                                   |
| F001           | RF011     | AT-WEB-06                                                                   |
| F001           | RF012     | AT-WEB-07                                                                   |
| F001           | RF013     | AT-WEB-07                                                                   |
| F002           | RF014     | AT-WEB-08                                                                   |
| F002           | RF015     | AT-WEB-08, AT-WEB-09                                                        |
| F002           | RF016     | AT-WEB-09                                                                   |
| F002           | RF017     | AT-WEB-08                                                                   |
| F003           | RF019     | AT-WEB-07                                                                   |
| F003           | RF020     | AT-WEB-10                                                                   |
| F003           | RF021     | AT-WEB-10                                                                   |
| F003           | RF022     | AT-WEB-11                                                                   |
| F003           | RF023     | AT-WEB-11                                                                   |
| F003           | RF024     | AT-WEB-12, AT-WEB-13                                                        |
| F003           | RF025     | AT-WEB-12                                                                   |
| F003           | RF026     | AT-WEB-14                                                                   |
| F003           | RF027     | AT-WEB-14                                                                   |
| F004           | RF030     | AT-WEB-15                                                                   |
| F004           | RF031     | AT-WEB-15                                                                   |
| F004           | RF032     | AT-WEB-15                                                                   |
| F004           | RF033     | AT-WEB-15                                                                   |
| F004           | RF034     | AT-WEB-15                                                                   |
| F004           | RF035     | AT-WEB-16                                                                   |
| F004           | RF036     | AT-WEB-17                                                                   |
| F006           | RF043     | AT-WEB-18                                                                   |
| F006           | RF044     | AT-WEB-18                                                                   |
| F006           | RF045     | AT-WEB-18                                                                   |
| F006           | RF046     | AT-WEB-18                                                                   |
| Transversal    | RNF003    | AT-WEB-19                                                                   |

| Funcionalidade | Requisitos web em âmbito | Cenários                                                                                  |
| -------------- | ------------------------ | ----------------------------------------------------------------------------------------- |
| F001           | RF001–RF013 (13)         | 8: AT-WEB-01, AT-WEB-02, AT-WEB-03, AT-WEB-04, AT-WEB-05, AT-WEB-06, AT-WEB-07, AT-WEB-19 |
| F002           | RF014–RF017 (4)          | 2: AT-WEB-08, AT-WEB-09                                                                   |
| F003           | RF019–RF027 (9)          | 6: AT-WEB-07, AT-WEB-10, AT-WEB-11, AT-WEB-12, AT-WEB-13, AT-WEB-14                       |
| F004           | RF030–RF036 (7)          | 3: AT-WEB-15, AT-WEB-16, AT-WEB-17                                                        |
| F006           | RF043–RF046 (4)          | 1: AT-WEB-18                                                                              |
| F005           | Fora do âmbito           | 0                                                                                         |
| F007           | Fora do âmbito           | 0                                                                                         |

## 15. Execução manual e relação com os testes de sistema

Os cenários de aceitação são executados **à mão**, por definição, e não são automatizados. O que se quer saber é se uma pessoa que não implementou a funcionalidade consegue fazer o trabalho do dia a dia da clínica só com a aplicação, e se o resultado lhe serve. Um script verifica que um botão produz a resposta esperada, mas não verifica se quem trabalha na clínica percebe o ecrã, encontra o caminho e aceita o resultado (Plano de Testes do Frontend Web, secção 11).

Os mesmos fluxos estão especificados como cenários de sistema na Especificação dos Testes de Sistema do Frontend Web (I128, PR #271), com os resultados esperados detalhados (mensagens exatas, valores gravados e pedidos à API). A automação desses cenários com Playwright está repartida por quatro tarefas, e nenhuma estava concluída à data desta especificação:

| Tarefa | Cenários de sistema | Sprint    | Estado em 2026-11-18 |
| ------ | ------------------- | --------- | -------------------- |
| I129   | 30                  | Sprint 07 | Por concluir         |
| I132   | 17                  | Sprint 07 | Por concluir         |
| I130   | 22                  | Sprint 08 | Planeada             |
| I131   | 4                   | Sprint 08 | Planeada             |

A tabela seguinte liga cada cenário de aceitação aos cenários de sistema que partilham pelo menos um requisito com ele (sem contar o RF005, que é transversal) e indica, entre parênteses, a tarefa que automatiza cada um:

| Cenário de aceitação | Cenários de sistema relacionados (tarefa de automação)                                                                                                                                                        |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AT-WEB-01            | ST-WEB-F001-01 (I129), ST-WEB-F001-04 (I132), ST-WEB-F001-05 (I132), ST-WEB-TRV-03 (I132), ST-WEB-TRV-05 (I132), ST-WEB-TRV-06 (I132)                                                                         |
| AT-WEB-02            | ST-WEB-F001-02 (I129), ST-WEB-F001-03 (I129), ST-WEB-F001-06 (I129), ST-WEB-F001-11 (I129), ST-WEB-F001-12 (I129)                                                                                             |
| AT-WEB-03            | ST-WEB-F001-06 (I129), ST-WEB-F001-07 (I129), ST-WEB-F001-08 (I129), ST-WEB-F001-09 (I129), ST-WEB-F001-10 (I129)                                                                                             |
| AT-WEB-04            | ST-WEB-F001-02 (I129), ST-WEB-F001-03 (I129), ST-WEB-F001-06 (I129), ST-WEB-F001-11 (I129), ST-WEB-F001-12 (I129)                                                                                             |
| AT-WEB-05            | ST-WEB-F001-13 (I129), ST-WEB-F001-14 (I129), ST-WEB-F001-15 (I129)                                                                                                                                           |
| AT-WEB-06            | ST-WEB-F001-16 (I129), ST-WEB-F001-17 (I129), ST-WEB-F001-18 (I129)                                                                                                                                           |
| AT-WEB-07            | ST-WEB-F001-19 (I129), ST-WEB-F001-20 (I129), ST-WEB-F001-21 (I129), ST-WEB-F003-01 (I129), ST-WEB-F003-02 (I129), ST-WEB-TRV-02 (I132)                                                                       |
| AT-WEB-08            | ST-WEB-F001-18 (I129), ST-WEB-F002-01 (I129), ST-WEB-F002-02 (I129), ST-WEB-F002-03 (I129), ST-WEB-F002-04 (I129), ST-WEB-F002-05 (I129), ST-WEB-F002-08 (I129), ST-WEB-F002-09 (I129), ST-WEB-F002-10 (I132) |
| AT-WEB-09            | ST-WEB-F002-04 (I129), ST-WEB-F002-05 (I129), ST-WEB-F002-06 (I129), ST-WEB-F002-07 (I129), ST-WEB-F002-10 (I132)                                                                                             |
| AT-WEB-10            | ST-WEB-F003-03 (I130), ST-WEB-F003-04 (I130), ST-WEB-F003-05 (I130), ST-WEB-F003-06 (I130)                                                                                                                    |
| AT-WEB-11            | ST-WEB-F003-04 (I130), ST-WEB-F003-07 (I130), ST-WEB-F003-08 (I130), ST-WEB-F003-09 (I130), ST-WEB-F003-10 (I130)                                                                                             |
| AT-WEB-12            | ST-WEB-F003-05 (I130), ST-WEB-F003-11 (I130), ST-WEB-F003-12 (I130), ST-WEB-F003-13 (I130), ST-WEB-F003-14 (I130)                                                                                             |
| AT-WEB-13            | ST-WEB-F003-05 (I130), ST-WEB-F003-11 (I130), ST-WEB-F003-12 (I130)                                                                                                                                           |
| AT-WEB-14            | ST-WEB-F003-15 (I130), ST-WEB-F003-16 (I130), ST-WEB-F003-17 (I130), ST-WEB-TRV-02 (I132)                                                                                                                     |
| AT-WEB-15            | ST-WEB-F004-01 (I130), ST-WEB-F004-02 (I130), ST-WEB-F004-03 (I130), ST-WEB-F004-04 (I130)                                                                                                                    |
| AT-WEB-16            | ST-WEB-F004-05 (I130), ST-WEB-F004-06 (I130)                                                                                                                                                                  |
| AT-WEB-17            | ST-WEB-F004-07 (I130), ST-WEB-F004-08 (I132)                                                                                                                                                                  |
| AT-WEB-18            | ST-WEB-F006-01 (I131), ST-WEB-F006-02 (I131), ST-WEB-F006-03 (I131), ST-WEB-F006-04 (I131), ST-WEB-F006-05 (I132)                                                                                             |
| AT-WEB-19            | ST-WEB-TRV-04 (I132)                                                                                                                                                                                          |

A simulação da I135 só começa depois de as quatro tarefas de automação estarem concluídas e de os testes de sistema da versão candidata passarem (secção 5). Um cenário de aceitação que falhe num ponto que nenhum cenário de sistema cobre dá origem a um novo cenário de sistema na versão seguinte da Especificação dos Testes de Sistema.

Durante a sessão, cada cenário é executado assim:

1. O registador lê em voz alta a situação, as pré-condições e os dados e confirma que as pré-condições se verificam.
2. O executor segue os passos sozinho. Se não souber como avançar, diz o que procura; o registador anota a dificuldade como desvio e só então indica o caminho.
3. No fim, o registador confirma cada ponto do critério de aceitação e anota o resultado (secção 7).

## 16. Decisões e pontos em aberto

| N.º  | Decisão ou ponto em aberto                                                                                                                                                                                                                                                                              | Consequência nesta especificação                                                                                                                                                                                                                                     |
| ---- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| D-01 | Os cenários foram escritos por situação do dia a dia da clínica, e não por ecrã, para que cada um termine com um trabalho completo de um perfil.                                                                                                                                                        | 19 cenários cobrem os 37 requisitos funcionais da aplicação web em âmbito e o RNF003 (secção 14).                                                                                                                                                                    |
| D-02 | Os ecrãs do registo clínico (I115, I116) e do boletim (I118) estão a ser implementados no Sprint 07, ao mesmo tempo que esta especificação.                                                                                                                                                             | Os passos dos AT-WEB-15 a AT-WEB-18 dizem o que a pessoa quer fazer, sem depender dos nomes exatos dos botões. Antes da sessão, a I135 confirma esses nomes na versão candidata; se diferirem, ajustam-se os passos numa v02, sem alterar os critérios de aceitação. |
| D-03 | Informação clínica disponível para o Auxiliar Veterinário (RF036): a regra em vigor é a da API v03, em que o Auxiliar lê o registo completo, só em leitura. A Documentação da API v03 (secção 13, ponto 4) deixa em aberto se passa a haver uma versão reduzida para este perfil; a decisão é do grupo. | O AT-WEB-17 indica os campos que a Carla vê e o que fica oculto segundo a regra em vigor e diz como o cenário é atualizado se a decisão mudar antes da simulação.                                                                                                    |
| D-04 | Leitura de registos clínicos de outros Veterinários (Documentação da API v03, secção 13, ponto 4): ainda não decidida.                                                                                                                                                                                  | O AT-WEB-16 usa um registo da própria Dra. Ana. Se o grupo decidir que os Veterinários leem os registos dos colegas, acrescenta-se esse passo numa v02.                                                                                                              |
| D-05 | Participantes propostos de forma a que o executor não tenha implementado a funcionalidade usada (secção 6).                                                                                                                                                                                             | A I135 confirma ou ajusta a proposta no início da sessão, mantendo a regra (CG-06).                                                                                                                                                                                  |
| D-06 | As funcionalidades do Cliente na aplicação móvel (RF018, RF028, RF029 e RF047) não pertencem à aplicação web.                                                                                                                                                                                           | Ficam na especificação de aceitação do frontend mobile (I140). Na web só se verifica que o Cliente não entra (AT-WEB-19).                                                                                                                                            |
| D-07 | Nenhuma palavra-passe fica escrita neste documento nem no relatório.                                                                                                                                                                                                                                    | As palavras-passe de demonstração e as palavras-passe iniciais das contas criadas são combinadas entre os participantes fora do repositório (secção 5).                                                                                                              |
| D-08 | Os resultados da simulação não pertencem a esta especificação.                                                                                                                                                                                                                                          | Ficam no relatório da simulação da aceitação (I135, Sprint 08), com a versão usada, os participantes, o resultado de cada cenário e as decisões sobre os desvios.                                                                                                    |

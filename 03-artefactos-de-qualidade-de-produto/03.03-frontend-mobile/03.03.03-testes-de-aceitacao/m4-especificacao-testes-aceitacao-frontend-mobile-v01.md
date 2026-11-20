# Especificação dos Testes de Aceitação do Frontend Mobile — Gestão de Clínica Veterinária

| Campo                | Valor                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| -------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Grupo                | G08                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Projeto              | Gestão de Clínica Veterinária                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| Artefacto            | Especificação dos Testes de Aceitação do Frontend Mobile (`OF-M4-020`)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| Ficheiro             | `m4-especificacao-testes-aceitacao-frontend-mobile-v01.md`                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| Versão               | v01                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Data                 | 2026-11-20                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| Milestone de Entrega | m4                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| Issue                | I140 — Especificar os testes de aceitação do frontend mobile (GitHub #128, Sprint 07)                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| Papéis               | Executor: Gonçalo Magalhães Ribeiro · Revisor: Ivan Cristiano Silva Mendes · Auditor: André Filipe Moreira Mesquita                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Pasta de destino     | `/projeto/03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.03-testes-de-aceitacao`                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Fontes               | `m3-declaracao-ambito-v05.pdf` (I002); `m3-especificacao-requisitos-v02.md` (I012); `m3-especificacoes-casos-uso-v02.md` (I015); `m4-plano-testes-frontend-mobile-v01.md` (I047, secção 11); `m3-documentacao-api-v03.md`; `m4-especificacao-testes-sistema-frontend-mobile-v01.md` (I136); `m4-especificacao-testes-aceitacao-frontend-web-v01.md` (I134, estrutura); scripts de dados de demonstração `04.13-scripts-de-base-de-dados/m3-v02` (I061); código da aplicação Android: I121 e I122 em `develop` (`6ea37e2`) e I123 no Pull Request #274 (`7ee3a48`) |

## Histórico de versões

| Versão | Data       | Autor                     | Alterações                                                                                                                                                                                                                                                                                                  |
| ------ | ---------- | ------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| v01    | 2026-11-20 | Gonçalo Magalhães Ribeiro | Versão inicial: 9 cenários de aceitação na perspetiva do Cliente para as 4 funcionalidades da aplicação móvel (F001, F002, F003 e F006), os 9 requisitos funcionais do Cliente e os requisitos não funcionais RNF002, RNF003, RNF005 e RNF006; critérios globais de aceitação e participantes da simulação. |

## Índice

1. Introdução
2. Documentos de referência
3. Âmbito
4. Como se lê um cenário
5. Preparação da simulação
6. Participantes da simulação
7. Critérios globais de aceitação
8. Cenários — F001 Entrar e sair da aplicação
9. Cenários — F002 Os meus animais
10. Cenários — F003 As minhas Consultas
11. Cenários — F006 Boletim de vacinação e desparasitação
12. Cenários — só a minha informação, sem rede
13. Matriz cenários ↔ funcionalidades e requisitos
14. Execução manual e relação com os testes de sistema
15. Decisões e pontos em aberto

## 1. Introdução

Esta especificação define como o grupo simula a aceitação da aplicação Android pelo **Cliente** da clínica. Cada cenário conta uma situação real de um tutor de animais — entrar na aplicação, ver os seus animais, saber quando é a próxima Consulta, rever as Consultas passadas, saber quando é a próxima vacina — e diz, em linguagem do Cliente, o que tem de acontecer para essa situação ficar resolvida.

A aplicação móvel só serve para **consultar**: o Cliente não marca, não altera nem cancela Consultas e não regista nada sobre os seus animais (RNF003; Modelo de Classes do Frontend Mobile v03, secção 11). Os cenários verificam também que essas ações não existem.

A simulação é feita no Sprint 08 pela I141, que produz o relatório. Esta especificação não contém resultados (D-07). É uma simulação académica com elementos do grupo no papel de Cliente e não equivale a uma homologação por um cliente real (Plano de Testes do Frontend Mobile, secção 11).

## 2. Documentos de referência

| Documento                                                         | Uso nesta especificação                                                                                                                                |
| ----------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Declaração de Âmbito v05 (I002)                                   | Funcionalidades do Cliente em âmbito (F001, F002, F003, F006) e funcionalidades retiradas (F005 — despesas, ecrã M05; F007)                            |
| Especificação de Requisitos v02 (I012)                            | RF001–RF005, RF018, RF028, RF029, RF047; RNF002, RNF003, RNF005 e RNF006                                                                               |
| Especificações de Casos de Uso v02 (I015)                         | UC01 (Autenticar), UC02 (Terminar sessão), UC08 (Consultar os meus animais), UC11 (Consultar Consultas futuras e histórico) e UC17 (Consultar boletim) |
| Plano de Testes do Frontend Mobile v01 (I047)                     | Secção 11 (simulação), critérios de entrada (8.1), dados (6.2) e identificadores `AT-MOB-nn` (4.1)                                                     |
| Especificação dos Testes de Sistema do Frontend Mobile v01 (I136) | Contas (5.2), dados de demonstração (5.3) e preparação PD-01 a PD-04 (5.4); cenários de sistema relacionados (secção 14)                               |
| Especificação dos Testes de Aceitação do Frontend Web v01 (I134)  | Estrutura, resultados possíveis e regra dos participantes, para que as duas simulações sejam comparáveis                                               |

## 3. Âmbito

| Incluído                                                                      | Excluído                                                                                                                                                              |
| ----------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| F001 — Entrar e sair da aplicação (RF001–RF005)                               | F005 — Gestão de contas e pagamentos: despesas do Cliente (RF042, ecrã M05). Fora do âmbito (Declaração de Âmbito v05; I124 descontinuada): **não há nenhum cenário** |
| F002 — Os meus animais (RF018)                                                | F007 — Internamento (RF048–RF052). Fora do âmbito: **não há nenhum cenário**                                                                                          |
| F003 — As minhas Consultas: futuras e histórico administrativo (RF028, RF029) | Marcar, alterar ou cancelar Consultas e registar dados dos animais na aplicação móvel: não existem (RNF003); os cenários só confirmam que não aparecem                |
| F006 — Boletim de vacinação e desparasitação (RF047)                          | Funcionalidades dos perfis internos: aplicação web (I134). Aqui só se confirma que um funcionário não entra (AT-MOB-03)                                               |
| RNF002, RNF003, RNF005 e RNF006, como o Cliente os percebe                    | RF053 a RF056: sem requisito aprovado na Especificação de Requisitos v02                                                                                              |
|                                                                               | Desempenho, segurança ofensiva, acessibilidade formal, vários telemóveis e funcionamento sem rede (não exigidos)                                                      |

## 4. Como se lê um cenário

| Campo                                    | Conteúdo                                                                                                                                    |
| ---------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| Identificador                            | `AT-MOB-nn` (Plano de Testes do Frontend Mobile, secção 4.1)                                                                                |
| Funcionalidade, requisitos e caso de uso | Origem do cenário, para rastreabilidade                                                                                                     |
| Conta                                    | Cliente (ou funcionário) que quem executa o cenário representa                                                                              |
| Situação                                 | A história do dia a dia do Cliente que o cenário representa                                                                                 |
| Pré-condições                            | O que tem de existir antes de começar                                                                                                       |
| Dados                                    | Os animais, Consultas e registos usados (dados de demonstração e preparação da secção 5)                                                    |
| Passos                                   | O que o Cliente faz, por ordem, com os nomes que aparecem no ecrã                                                                           |
| Critério de aceitação                    | O que tem de acontecer para o cenário ser **Aceite**; se algum ponto falhar, o cenário é **Aceite com desvio** ou **Não aceite** (secção 7) |

Os nomes entre «» são os textos que aparecem no ecrã. Nenhum passo pede para esperar um tempo fixo: o passo seguinte só começa quando a resposta da aplicação aparece no ecrã. Datas e horas são as de Lisboa (o emulador está em `Europe/Lisbon`).

## 5. Preparação da simulação

1. **Versão:** antes da sessão, o grupo identifica o commit da aplicação Android e o do backend e regista-os no relatório (I141). Os testes de sistema dessa versão (I137 e I138) têm de estar a passar (Plano de Testes do Frontend Mobile, secção 8.1).
2. **Ambiente:** o emulador Android de referência (Pixel 8, Android 17 / API 37, fuso `Europe/Lisbon`) com a aplicação de debug ligada ao backend local (`adb reverse tcp:5159 tcp:5159` e `-PapiBaseUrl=http://localhost:5159`), ou ao ambiente público de avaliação. A base é reposta e os dados de demonstração carregados no início da sessão.
3. **Preparação dos dados:** depois da reposição, um elemento do grupo cria, no frontend web ou pela API, os dados da preparação da Especificação dos Testes de Sistema (I136, secção 5.4) de que os cenários precisam:
   - **PD-01** — uma Consulta futura para o Bobi (Rececionista, «Nova marcação»): Dra. Ana Veterinária, num dia útil a pelo menos 7 dias da sessão, às 10:00, com o motivo «Simulação {data}»;
   - **PD-02** — uma Consulta futura para o Kiko, no mesmo dia, às 11:00;
   - **PD-04** — uma vacinação nova do Bobi (Veterinário, `vet.ana`): «Vacina simulação {data} (fictícia)», administrada no dia da sessão, com 30 dias até à próxima.
     `{data}` é a data da sessão no formato AAAAMMDD. A PD-03 (Cliente sem animais) não é usada na aceitação.
4. **Contas:** as contas de demonstração da Especificação dos Testes de Sistema (secção 5.2). As palavras-passe são comunicadas aos participantes fora do repositório e não ficam escritas no relatório.
5. **Ordem:** os cenários podem ser executados por qualquer ordem. Nenhum cenário altera dados: a aplicação móvel só consulta. Antes de cada cenário apagam-se os dados da aplicação no emulador (Definições → Aplicações → Gestão de Clínica Veterinária → Armazenamento → Limpar dados), para começar sem sessão aberta.
6. **Registo:** quem regista anota, por cenário, o executor, a hora de início e de fim, o resultado (Aceite, Aceite com desvio ou Não aceite) e, em cada desvio, o que aconteceu, o que era esperado e a decisão tomada.

## 6. Participantes da simulação

Participam os quatro elementos do grupo. Cada cenário tem:

- um **executor**, que faz de Cliente (ou de funcionário, no AT-MOB-03) e segue os passos sem ajuda de quem implementou a funcionalidade;
- um **registador**, que lê os passos, confirma o critério de aceitação e anota o resultado.

Regra: o executor de um cenário **não implementou** a funcionalidade nele usada (Plano de Testes do Frontend Mobile, secção 11). A base da aplicação (I120, `ApiClient` e tratamento de falhas de rede) foi implementada por Ivan Cristiano Silva Mendes; a autenticação (I121), os animais e o boletim (I122) e as Consultas (I123) por Gonçalo Magalhães Ribeiro. Por isso Gonçalo Magalhães Ribeiro nunca é executor, e Ivan Cristiano Silva Mendes não executa o AT-MOB-09. A proposta seguinte cumpre a regra; a I141 confirma-a ou ajusta-a no início da sessão, mantendo a regra.

| Cenários             | Funcionalidade usada e quem a implementou                                                     | Executor proposto                | Registador proposto              |
| -------------------- | --------------------------------------------------------------------------------------------- | -------------------------------- | -------------------------------- |
| AT-MOB-01, AT-MOB-02 | Autenticação e sessão — Gonçalo Magalhães Ribeiro (I121)                                      | André Filipe Moreira Mesquita    | Gonçalo Magalhães Ribeiro        |
| AT-MOB-03            | Recusa dos perfis internos — Gonçalo Magalhães Ribeiro (I121)                                 | Gonçalo Dinis Magalhães Teixeira | Ivan Cristiano Silva Mendes      |
| AT-MOB-04            | Os meus animais — Gonçalo Magalhães Ribeiro (I122)                                            | Ivan Cristiano Silva Mendes      | Gonçalo Dinis Magalhães Teixeira |
| AT-MOB-05, AT-MOB-06 | Consultas — Gonçalo Magalhães Ribeiro (I123)                                                  | Gonçalo Dinis Magalhães Teixeira | Gonçalo Magalhães Ribeiro        |
| AT-MOB-07            | Boletim — Gonçalo Magalhães Ribeiro (I122)                                                    | André Filipe Moreira Mesquita    | Ivan Cristiano Silva Mendes      |
| AT-MOB-08            | Animais, Consultas e boletim de dois Clientes — Gonçalo Magalhães Ribeiro (I122, I123)        | Ivan Cristiano Silva Mendes      | André Filipe Moreira Mesquita    |
| AT-MOB-09            | Falha de ligação — Ivan Cristiano Silva Mendes (I120) e Gonçalo Magalhães Ribeiro (I121–I123) | André Filipe Moreira Mesquita    | Gonçalo Dinis Magalhães Teixeira |

## 7. Critérios globais de aceitação

| N.º   | Critério                                                                                                                                                                                                      | Como se verifica                                                                           |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------ |
| CG-01 | Os 9 cenários desta especificação foram executados sobre a mesma versão identificada da aplicação Android e do backend                                                                                        | Relatório da simulação (I141): commits e data da sessão                                    |
| CG-02 | Nenhum cenário fica **Não aceite**                                                                                                                                                                            | Resultado de cada cenário no relatório                                                     |
| CG-03 | Cada cenário **Aceite com desvio** tem decisão registada: corrigido e repetido antes da entrega, ou registado como limitação conhecida nas Notas de Release finais (I056); cada desvio tem Issue e severidade | Coluna de decisão do relatório; Issues; Notas de Release                                   |
| CG-04 | Nenhum desvio impede o Cliente de concluir o que o cenário pede (entrar, ver os seus animais, as suas Consultas e o boletim)                                                                                  | Descrição dos desvios no relatório                                                         |
| CG-05 | Em nenhum cenário o Cliente vê informação de outro Cliente, dados clínicos das Consultas ou ações de marcar, alterar, cancelar ou registar; e nenhum funcionário entra na aplicação                           | Critérios de aceitação dos cenários AT-MOB-03, AT-MOB-05, AT-MOB-06, AT-MOB-07 e AT-MOB-08 |
| CG-06 | Cada cenário foi executado por quem não implementou a funcionalidade usada                                                                                                                                    | Executor e registador de cada cenário no relatório (secção 6)                              |

A aplicação móvel é **aceite** quando os seis critérios se verificam. Os resultados possíveis de cada cenário são: **Aceite** (todos os pontos do critério de aceitação se verificam), **Aceite com desvio** (o Cliente consegue o que pretendia, mas algum ponto não se verifica exatamente como descrito) e **Não aceite** (o Cliente não consegue o que pretendia ou a aplicação mostra ou permite algo que não devia).

## 8. Cenários — F001 Entrar e sair da aplicação

### AT-MOB-01 — Entrar na aplicação, voltar mais tarde e sair

| Funcionalidade | Requisitos                 | Caso de uso | Conta                       |
| -------------- | -------------------------- | ----------- | --------------------------- |
| F001           | RF001, RF003, RF004, RF005 | UC01, UC02  | `cliente.joao` (João Costa) |

- **Situação:** o João instala a aplicação da clínica, entra com a conta que a receção lhe criou, fecha a aplicação e volta a abri-la mais tarde; no fim, como o telemóvel é partilhado em casa, sai da conta.
- **Pré-condições:** aplicação sem sessão aberta.
- **Dados:** conta `cliente.joao@clinica-g08.test`.
- **Passos:**
  1. Abrir a aplicação, escrever o e-mail e a palavra-passe em «Email» e «Palavra-passe» e carregar em «Entrar».
  2. Fechar a aplicação (tirá-la das aplicações recentes) e voltar a abri-la.
  3. Carregar em «Terminar sessão» na barra inferior.
  4. Fechar e voltar a abrir a aplicação.
- **Critério de aceitação:**
  1. Depois de entrar, o João vê logo «Os meus animais», com o Bobi e a Mia.
  2. Ao voltar a abrir a aplicação, não tem de escrever outra vez o e-mail e a palavra-passe.
  3. Depois de «Terminar sessão» aparece «Iniciar sessão» com a indicação «Terminou a sessão.».
  4. Ao abrir de novo, a aplicação pede o e-mail e a palavra-passe: ninguém em casa consegue ver os animais do João sem entrar.

### AT-MOB-02 — Enganar-se na palavra-passe

| Funcionalidade | Requisitos | Caso de uso | Conta          |
| -------------- | ---------- | ----------- | -------------- |
| F001           | RF002      | UC01        | `cliente.joao` |

- **Situação:** o João engana-se a escrever o e-mail e depois a palavra-passe, e só à terceira acerta.
- **Pré-condições:** aplicação sem sessão aberta.
- **Dados:** e-mail `cliente.joao@clinica-g08.test`; uma palavra-passe errada à escolha do executor.
- **Passos:**
  1. Carregar em «Entrar» sem escrever nada.
  2. Escrever `cliente.joao` (sem o resto do e-mail) e carregar em «Entrar».
  3. Escrever o e-mail completo com uma palavra-passe errada e carregar em «Entrar».
  4. Escrever a palavra-passe certa e carregar em «Entrar».
- **Critério de aceitação:**
  1. Nos passos 1 e 2 a aplicação diz, junto aos campos, o que falta ou está mal («Indique o email.», «Indique um email válido.», «Indique a palavra-passe.»).
  2. No passo 3 aparece «Email ou palavra-passe incorretos.», sem dizer qual dos dois está errado, e o João pode tentar outra vez sem reabrir a aplicação.
  3. No passo 4 entra normalmente e vê os seus animais.

### AT-MOB-03 — Um funcionário da clínica não usa a aplicação dos Clientes

| Funcionalidade | Requisitos    | Caso de uso | Conta                                                            |
| -------------- | ------------- | ----------- | ---------------------------------------------------------------- |
| F001           | RF005, RNF003 | UC01        | `rececao` (Rita Rececionista) e `vet.ana` (Dra. Ana Veterinária) |

- **Situação:** a Rita, da receção, e a Dra. Ana instalam a aplicação por curiosidade e tentam entrar com as contas de trabalho.
- **Pré-condições:** aplicação sem sessão aberta.
- **Dados:** contas `rececao@clinica-g08.test` e `vet.ana@clinica-g08.test`.
- **Passos:** para cada conta:
  1. Escrever o e-mail e a palavra-passe e carregar em «Entrar».
  2. Fechar e voltar a abrir a aplicação.
- **Critério de aceitação:**
  1. A aplicação não deixa entrar e explica «Esta aplicação destina-se apenas a clientes da clínica.».
  2. Não aparece nenhum animal, Consulta ou ecrã da clínica.
  3. Ao voltar a abrir, a aplicação pede de novo o e-mail e a palavra-passe (nada ficou guardado).

## 9. Cenários — F002 Os meus animais

### AT-MOB-04 — Ver os meus animais e os dados de cada um

| Funcionalidade | Requisitos | Caso de uso | Conta          |
| -------------- | ---------- | ----------- | -------------- |
| F002           | RF018      | UC08        | `cliente.joao` |

- **Situação:** o João quer confirmar que a clínica tem bem os dados do Bobi e da Mia, porque vai pedir um seguro para o Bobi e precisa da data de nascimento e do peso.
- **Pré-condições:** aplicação sem sessão aberta.
- **Dados:** Bobi (Cão, Labrador Retriever, Macho, nascido a 10/03/2022, 28,4 kg) e Mia (Gato, Europeu Comum, 4,2 kg).
- **Passos:**
  1. Entrar com a conta do João.
  2. Em «Os meus animais», carregar em «Ver detalhe» do Bobi.
  3. Carregar em «Voltar» e abrir o detalhe da Mia.
- **Critério de aceitação:**
  1. A lista tem só o Bobi e a Mia, com a espécie e a raça de cada um.
  2. O detalhe do Bobi mostra «Espécie» Cão, «Raça» Labrador Retriever, «Sexo» Macho, «Data de nascimento» 10/03/2022, a idade e «Peso» 28,4 kg, iguais aos que a clínica tem.
  3. O detalhe tem «Ver boletim» e «Ver consultas», mas nenhuma forma de alterar os dados (o João sabe que tem de pedir à clínica).
  4. «Voltar» leva de volta à lista.

## 10. Cenários — F003 As minhas Consultas

### AT-MOB-05 — Saber quando é a próxima Consulta do Bobi

| Funcionalidade | Requisitos    | Caso de uso | Conta          |
| -------------- | ------------- | ----------- | -------------- |
| F003           | RF028, RNF005 | UC11        | `cliente.joao` |

- **Situação:** o João ligou para a clínica e a receção marcou uma Consulta para o Bobi. Uns dias depois não se lembra do dia nem da hora e vê na aplicação.
- **Pré-condições:** aplicação sem sessão aberta; PD-01 e PD-02 criadas no início da sessão (secção 5).
- **Dados:** Consulta futura do Bobi (PD-01) e Consulta futura do Kiko, da Maria (PD-02).
- **Passos:**
  1. Entrar com a conta do João.
  2. Carregar em «Consultas» na barra inferior e ver o separador «Próximas».
  3. Tocar na Consulta do Bobi.
  4. Voltar a «Animais», abrir o detalhe do Bobi e carregar em «Ver consultas».
- **Critério de aceitação:**
  1. Em «Próximas» aparece a Consulta do Bobi marcada pela receção, com o dia e a hora certos (10:00), o estado, o nome do Bobi e a Dra. Ana Veterinária.
  2. Não aparece a Consulta do Kiko nem Consultas que já passaram.
  3. Não há nenhuma forma de marcar, mudar, confirmar ou cancelar a Consulta na aplicação; tocar na Consulta não abre nenhuma dessas ações.
  4. A partir do Bobi, «Ver consultas» mostra «Só as Consultas deste animal.» e a mesma Consulta.

### AT-MOB-06 — Rever as Consultas passadas sem ver informação clínica

| Funcionalidade | Requisitos | Caso de uso | Conta          |
| -------------- | ---------- | ----------- | -------------- |
| F003           | RF029      | UC11        | `cliente.joao` |

- **Situação:** o João quer saber quando o Bobi foi à última Consulta e se a Consulta da Mia de outubro chegou a ser cancelada.
- **Pré-condições:** aplicação sem sessão aberta.
- **Dados:** Consultas passadas do Bobi (14/10/2026 10:30 Realizada, 05/11/2026 11:00 Pendente) e da Mia (20/10/2026 11:00 Cancelada); a Consulta de 14/10/2026 tem registo clínico («Otite externa»).
- **Passos:**
  1. Entrar com a conta do João.
  2. Abrir «Consultas» e o separador «Histórico».
  3. Abrir o detalhe da Mia, «Ver consultas» e o separador «Histórico».
- **Critério de aceitação:**
  1. O histórico mostra as Consultas do Bobi e da Mia, da mais recente para a mais antiga, cada uma com o dia e a hora, o animal, o Veterinário e o estado (Realizada, Pendente, Cancelada).
  2. Não aparece o diagnóstico («Otite externa»), as notas, o tratamento nem a prescrição de nenhuma Consulta.
  3. A partir da Mia, o histórico só tem a Consulta cancelada de 20/10/2026, e «Próximas» diz «Este animal não tem Consultas marcadas.».

## 11. Cenários — F006 Boletim de vacinação e desparasitação

### AT-MOB-07 — Saber quando é a próxima vacina do Bobi

| Funcionalidade | Requisitos    | Caso de uso | Conta          |
| -------------- | ------------- | ----------- | -------------- |
| F006           | RF047, RNF005 | UC17        | `cliente.joao` |

- **Situação:** o Bobi foi vacinado hoje na clínica. À noite, o João quer ver no boletim a vacina de hoje e saber quando são as próximas vacinas e a próxima desparasitação. Quer também confirmar que a Mia ainda não tem nada registado.
- **Pré-condições:** aplicação sem sessão aberta; PD-04 criada no início da sessão (secção 5).
- **Dados:** boletim do Bobi (vacina antirrábica de 14/10/2025, vacina polivalente e desparasitante interno de 14/10/2026, e a vacina de PD-04); Mia sem registos.
- **Passos:**
  1. Entrar com a conta do João.
  2. Abrir o detalhe do Bobi e carregar em «Ver boletim».
  3. Carregar em «Voltar», voltar à lista e abrir o boletim da Mia.
- **Critério de aceitação:**
  1. O boletim do Bobi mostra a vacina dada hoje, com a data de hoje, o lote e o Veterinário, e as vacinações e a desparasitação anteriores.
  2. Em «Próximas administrações previstas» aparecem a próxima da vacina de hoje (daqui a 30 dias), 12/01/2027 (desparasitante interno) e 14/10/2027 (polivalente), com datas iguais às que a clínica calculou.
  3. A antirrábica, cuja data prevista já passou, não aparece como próxima.
  4. O boletim da Mia diz «Este animal ainda não tem vacinações nem desparasitações registadas.».
  5. Não há nenhuma forma de registar ou alterar vacinas ou desparasitações na aplicação.

## 12. Cenários — só a minha informação, sem rede

### AT-MOB-08 — A Maria só vê o Kiko, mesmo no telemóvel do João

| Funcionalidade   | Requisitos                         | Caso de uso      | Conta                                          |
| ---------------- | ---------------------------------- | ---------------- | ---------------------------------------------- |
| F002, F003, F006 | RF018, RF028, RF029, RF047, RNF002 | UC08, UC11, UC17 | `cliente.maria` (Maria Silva) e `cliente.joao` |

- **Situação:** a Maria, vizinha do João, usa o telemóvel dele para ver as Consultas do Kiko. O João saiu da conta antes de lhe emprestar o telemóvel.
- **Pré-condições:** aplicação sem sessão aberta; PD-01 e PD-02 criadas.
- **Dados:** Kiko (Cão, uma desparasitação a 01/08/2026, Consulta de 03/11/2026 09:00 Confirmada e a Consulta futura de PD-02); animais e Consultas do João.
- **Passos:**
  1. Entrar com a conta da Maria.
  2. Ver «Os meus animais», «Consultas» (os dois separadores) e o boletim do Kiko.
  3. Carregar em «Terminar sessão» e entrar com a conta do João.
- **Critério de aceitação:**
  1. A Maria só vê o Kiko; o Bobi e a Mia não aparecem em lado nenhum.
  2. Em «Próximas» só aparece a Consulta futura do Kiko; no «Histórico» só a de 03/11/2026 (Confirmada).
  3. O boletim do Kiko mostra a desparasitação de 01/08/2026 e «Não há administrações previstas.».
  4. Depois de o João entrar, só aparecem o Bobi e a Mia e as suas Consultas; nada da Maria fica visível.

### AT-MOB-09 — Usar a aplicação com a ligação em baixo

| Funcionalidade | Requisitos           | Caso de uso | Conta           |
| -------------- | -------------------- | ----------- | --------------- |
| F002, F003     | RNF006, RF018, RF028 | UC08, UC11  | `cliente.maria` |

- **Situação:** a Maria abre a aplicação no metro, sem rede, para ver a Consulta do Kiko; quando volta a ter rede, tenta outra vez.
- **Pré-condições:** sessão da Maria aberta.
- **Dados:** Kiko e as suas Consultas.
- **Passos:**
  1. Desligar a rede do emulador (modo de avião) — ou, no ambiente local, parar o backend.
  2. Abrir o detalhe do Kiko; depois carregar em «Consultas».
  3. Carregar em «Tentar novamente» ainda sem rede.
  4. Ligar a rede (ou o backend) e carregar em «Tentar novamente».
- **Critério de aceitação:**
  1. Sem rede, a aplicação diz «Sem ligação ao servidor. Verifique a sua ligação à rede.» e oferece «Tentar novamente», sem fechar, sem ficar presa e sem mostrar mensagens técnicas.
  2. A Maria continua a poder mudar de separador na barra inferior.
  3. Com a rede de volta, «Tentar novamente» mostra os dados do Kiko e as Consultas, sem ter de entrar outra vez.

## 13. Matriz cenários ↔ funcionalidades e requisitos

### 13.1. Por cenário

| Cenário   | Funcionalidade   | Requisitos                         | Caso de uso      | Conta                           |
| --------- | ---------------- | ---------------------------------- | ---------------- | ------------------------------- |
| AT-MOB-01 | F001             | RF001, RF003, RF004, RF005         | UC01, UC02       | `cliente.joao`                  |
| AT-MOB-02 | F001             | RF002                              | UC01             | `cliente.joao`                  |
| AT-MOB-03 | F001             | RF005, RNF003                      | UC01             | `rececao`, `vet.ana`            |
| AT-MOB-04 | F002             | RF018                              | UC08             | `cliente.joao`                  |
| AT-MOB-05 | F003             | RF028, RNF005                      | UC11             | `cliente.joao`                  |
| AT-MOB-06 | F003             | RF029                              | UC11             | `cliente.joao`                  |
| AT-MOB-07 | F006             | RF047, RNF005                      | UC17             | `cliente.joao`                  |
| AT-MOB-08 | F002, F003, F006 | RF018, RF028, RF029, RF047, RNF002 | UC08, UC11, UC17 | `cliente.maria`, `cliente.joao` |
| AT-MOB-09 | F002, F003       | RNF006, RF018, RF028               | UC08, UC11       | `cliente.maria`                 |

### 13.2. Por funcionalidade e requisito

| Funcionalidade             | Requisito                                     | Cenários                        |
| -------------------------- | --------------------------------------------- | ------------------------------- |
| F001 — Autenticação        | RF001 — Autenticar utilizador                 | AT-MOB-01, AT-MOB-02            |
| F001                       | RF002 — Rejeitar autenticação inválida        | AT-MOB-02                       |
| F001                       | RF003 — Terminar sessão                       | AT-MOB-01, AT-MOB-08            |
| F001                       | RF004 — Manter acesso autenticado válido      | AT-MOB-01, AT-MOB-09            |
| F001                       | RF005 — Restringir funcionalidades por perfil | AT-MOB-01, AT-MOB-03            |
| F002 — Os meus animais     | RF018 — Consultar animais do Cliente          | AT-MOB-04, AT-MOB-08, AT-MOB-09 |
| F003 — As minhas Consultas | RF028 — Consultar marcações futuras           | AT-MOB-05, AT-MOB-08, AT-MOB-09 |
| F003                       | RF029 — Consultar histórico administrativo    | AT-MOB-06, AT-MOB-08            |
| F006 — Boletim             | RF047 — Consultar boletim na aplicação móvel  | AT-MOB-07, AT-MOB-08            |
| —                          | RNF002 — Isolamento dos dados de Clientes     | AT-MOB-08                       |
| —                          | RNF003 — Separação dos canais                 | AT-MOB-03                       |
| —                          | RNF005 — Persistência dos dados confirmados   | AT-MOB-05, AT-MOB-07            |
| —                          | RNF006 — Dependência de ligação à rede        | AT-MOB-09                       |

As 4 funcionalidades do Cliente na aplicação móvel e os 9 requisitos funcionais do Cliente têm pelo menos um cenário de aceitação. Nenhum cenário usa F005 (despesas, M05), F007 ou funcionalidades retiradas do âmbito.

## 14. Execução manual e relação com os testes de sistema

Os cenários de aceitação são executados **à mão**, por definição, e não são automatizados: o que se quer saber é se uma pessoa que não implementou a aplicação consegue, como Cliente, resolver a situação do cenário só com a aplicação, e se o resultado lhe serve. Os mesmos fluxos estão especificados como cenários de sistema na Especificação dos Testes de Sistema do Frontend Mobile (I136), com as mensagens exatas e os elementos de cada ecrã, automatizados pelas I137 e I138 (Sprint 08).

| Cenário de aceitação | Cenários de sistema relacionados (I136)                       |
| -------------------- | ------------------------------------------------------------- |
| AT-MOB-01            | ST-MOB-F001-01, ST-MOB-F001-06, ST-MOB-F001-07                |
| AT-MOB-02            | ST-MOB-F001-02, ST-MOB-F001-04                                |
| AT-MOB-03            | ST-MOB-F001-05, ST-MOB-TRV-02                                 |
| AT-MOB-04            | ST-MOB-F002-01, ST-MOB-F002-02                                |
| AT-MOB-05            | ST-MOB-F003-01, ST-MOB-F003-03, ST-MOB-F003-04                |
| AT-MOB-06            | ST-MOB-F003-02, ST-MOB-F003-05                                |
| AT-MOB-07            | ST-MOB-F006-01, ST-MOB-F006-02, ST-MOB-F006-03                |
| AT-MOB-08            | ST-MOB-TRV-01, ST-MOB-F002-04, ST-MOB-F003-06, ST-MOB-F006-04 |
| AT-MOB-09            | ST-MOB-TRV-03, ST-MOB-TRV-04                                  |

A simulação da I141 só começa depois de os testes de sistema da versão candidata passarem (secção 5). Um cenário de aceitação que falhe num ponto que nenhum cenário de sistema cobre dá origem a um novo cenário de sistema na versão seguinte da Especificação dos Testes de Sistema.

Durante a sessão, cada cenário é executado assim:

1. O registador lê em voz alta a situação, as pré-condições e os dados e confirma que as pré-condições se verificam.
2. O executor segue os passos sozinho. Se não souber como avançar, diz o que procura; o registador anota a dificuldade como desvio e só então indica o caminho.
3. No fim, o registador confirma cada ponto do critério de aceitação e anota o resultado (secção 7).

## 15. Decisões e pontos em aberto

| N.º  | Decisão ou ponto em aberto                                                                                                                         | Consequência nesta especificação                                                                                                                                                                                                                            |
| ---- | -------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| D-01 | Os cenários foram escritos por situação do dia a dia do Cliente, e não por ecrã, para que cada um termine com uma necessidade do Cliente resolvida | 9 cenários cobrem as 4 funcionalidades e os 9 requisitos funcionais do Cliente (secção 13)                                                                                                                                                                  |
| D-02 | Os dados de demonstração não têm Consultas futuras desde 2026-11-06 nem vacinações recentes                                                        | A sessão começa com a preparação PD-01, PD-02 e PD-04 da I136 (secção 5), feita pelos perfis internos no frontend web ou pela API, como acontece na clínica                                                                                                 |
| D-03 | O ecrã M04 (Consultas) está no Pull Request #274 (I123), ainda em revisão à data desta versão                                                      | Os passos de AT-MOB-05 e AT-MOB-06 usam os nomes desse Pull Request («Próximas», «Histórico», «Ver consultas»). Antes da sessão, a I141 confirma-os na versão candidata; se diferirem, ajustam-se os passos numa v02, sem alterar os critérios de aceitação |
| D-04 | Participantes propostos de forma a que o executor não tenha implementado a funcionalidade usada (secção 6)                                         | Gonçalo Magalhães Ribeiro, que implementou I121 a I123, só é registador; a I141 confirma ou ajusta a proposta, mantendo a regra (CG-06)                                                                                                                     |
| D-05 | O AT-MOB-03 usa um funcionário e não um Cliente                                                                                                    | É a única forma de verificar, como utilizador, que a aplicação é só dos Clientes (RF005, RNF003)                                                                                                                                                            |
| D-06 | Nenhuma palavra-passe fica escrita neste documento nem no relatório                                                                                | As palavras-passe de demonstração são combinadas entre os participantes fora do repositório (secção 5)                                                                                                                                                      |
| D-07 | Os resultados da simulação não pertencem a esta especificação                                                                                      | Ficam no relatório da simulação de aceitação (I141, Sprint 08), com a versão usada, os participantes, o resultado de cada cenário e as decisões sobre os desvios                                                                                            |

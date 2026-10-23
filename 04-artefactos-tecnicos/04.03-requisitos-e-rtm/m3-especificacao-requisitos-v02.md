# Especificação de Requisitos

**Projeto:** Gestão de Clínica Veterinária  
**Grupo:** G08  
**Milestone:** m3  
**Versão:** v02  
**Issue e papéis:** por atribuir. Esta versão ainda não tem Issue própria; será planeada no Backlog Refinement e no Sprint Planning, onde se atribuem Executor, Revisor e Auditor. A Issue I012 (v01, m2) está concluída e não é reaberta.  
**Estado:** Rascunho para revisão e auditoria

---

## Histórico de versões

| Versão | Data | Descrição das alterações |
|---|---|---|
| v01 | m2 | Especificação dos requisitos RF001–RF052 e RNF001–RNF006. Mantém-se inalterada como histórico (`m2-especificacao-requisitos-v01.md`). |
| v02 | 23/10/2026 | Reflete a Declaração de Âmbito v05 e a Proposta de Sistema v05: as funcionalidades F005 (Gestão de contas e pagamentos) e F007 (Gestão de internamento) passam a fora do âmbito, por transição para o não-âmbito, autorizada pelo docente na reunião de 20/10/2026 e confirmada pelo e-mail de homologação de 21/10/2026 (Regulamento da UC, secção 7.5). Os requisitos RF037–RF042 e RF048–RF052 ficam **descontinuados**: o texto da v01 é conservado e os identificadores permanecem reservados. |

### Alterações da v02 em relação à v01

| Ponto | Alteração |
|---|---|
| Secção 2 | Fontes passam a `m3-proposta-sistema-v05.pdf` e `m3-declaracao-ambito-v05.pdf`; a tabela de funcionalidades indica F005 e F007 como fora do âmbito. |
| Secção 3 | Retiradas as referências a pagamentos, despesas e internamentos nas descrições do Rececionista, do Veterinário, do Auxiliar Veterinário e do Cliente. |
| Secções 5.5 e 5.7 | RF037–RF042 e RF048–RF052 passam a «Descontinuado». O texto e o título de cada requisito mantêm-se como na v01, para a rastreabilidade. |
| Secção 7 | Acrescentadas as exclusões da gestão de contas e pagamentos e da gestão de internamento. |
| Secção 8 | Acrescentada a nota sobre os requisitos descontinuados e a RTM v02. |
| Secção 9 | Nova: pontos em aberto para o grupo. |
| Restantes requisitos | RF001–RF036, RF043–RF047 e RNF001–RNF006 mantêm o texto da v01. |

Em âmbito ficam **41 requisitos funcionais** (RF001–RF036 e RF043–RF047) e **6 requisitos não funcionais**. Os 11 requisitos descontinuados somam-se aos 41 em âmbito, num total de 52 identificadores, como na v01.

---

## 1. Objetivo

O presente documento especifica os requisitos funcionais e não funcionais do sistema de Gestão de Clínica Veterinária, de forma individual, clara, verificável, consistente e rastreável.

A especificação concretiza o âmbito homologado na Proposta de Sistema e na Declaração de Âmbito e constitui a base para os restantes artefactos técnicos do projeto, incluindo a Checklist de Aprovação dos Requisitos, a Requirements Traceability Matrix (RTM), os casos de uso, a arquitetura, os modelos de dados e de classes, os protótipos, a implementação e os testes.

---

## 2. Fontes e âmbito

Esta especificação tem como fontes normativas principais:

- `m3-proposta-sistema-v05.pdf`;
- `m3-declaracao-ambito-v05.pdf`.

A Declaração de Âmbito v05 mantém as funcionalidades F001 a F007, mas passa F005 e F007 a **fora do âmbito**: o grupo não se compromete a executá-las, por transição para o não-âmbito, autorizada pelo docente na reunião de 20/10/2026 e confirmada pelo e-mail de homologação de 21/10/2026 (Regulamento da UC, secção 7.5). O âmbito funcional em vigor é o seguinte:

| Funcionalidade | Designação | Requisitos associados | Estado |
|---|---|---|---|
| F001 | Autenticação e Controlos de Acesso | RF001–RF013 | Em âmbito |
| F002 | Gestão de pacientes (Fichas dos animais) | RF014–RF018 | Em âmbito |
| F003 | Agendamento de consultas | RF019–RF029 | Em âmbito |
| F004 | Registo clínico e tratamentos | RF030–RF036 | Em âmbito |
| F005 | Gestão de contas e pagamentos | RF037–RF042 | **Fora do âmbito** (requisitos descontinuados) |
| F006 | Boletim de Vacinas e Desparasitações | RF043–RF047 | Em âmbito |
| F007 | Gestão de internamento (Hospitalização) | RF048–RF052 | **Fora do âmbito** (requisitos descontinuados) |

Os requisitos em âmbito não devem introduzir funcionalidades fora do âmbito aprovado. As exclusões e os limites transversais são identificados na secção 7.

---

## 3. Atores

### 3.1. Administrador

Utilizador interno com acesso à aplicação web, responsável pela gestão de utilizadores e perfis de acesso, contas de funcionários, dados gerais da clínica, configurações do sistema e horários de funcionamento da clínica. Não pode consultar, criar ou alterar registos clínicos.

### 3.2. Rececionista

Utilizador interno com acesso à aplicação web, responsável pelo registo e atualização dos dados administrativos de clientes e animais, gestão de marcações, consulta da agenda, registo de chegadas. Não pode criar ou alterar registos clínicos, diagnósticos, tratamentos ou prescrições.

### 3.3. Veterinário

Utilizador interno com acesso à aplicação web, responsável pela consulta da agenda e dos dados dos animais e pelo registo de consultas, diagnósticos, observações clínicas, tratamentos, prescrições e vacinação. Não pode gerir utilizadores, perfis de acesso ou configurações do sistema.

### 3.4. Auxiliar Veterinário

Utilizador interno com acesso à aplicação web, que pode consultar os dados administrativos e a informação clínica necessária à preparação e acompanhamento das consultas, bem como registar informação não clínica de apoio quando autorizado. Não pode criar ou alterar registos clínicos, diagnósticos, tratamentos ou prescrições.

### 3.5. Cliente (Proprietário do animal)

Utilizador com acesso exclusivamente à aplicação móvel. Pode consultar os dados dos seus animais, o boletim de vacinas e desparasitações, o histórico de consultas e as próximas consultas. Não pode solicitar, alterar ou cancelar marcações, criar ou editar fichas de animais, registar atos clínicos ou aceder a informação pertencente a outros clientes.

---

## 4. Convenções de especificação

Os requisitos funcionais são identificados pelo prefixo `RF`, seguido de um número sequencial de três algarismos.

Os requisitos não funcionais são identificados pelo prefixo `RNF`, seguido de um número sequencial de três algarismos.

Cada identificador é único e deve ser preservado nos restantes artefactos de rastreabilidade do projeto. Um identificador descontinuado não deve ser reutilizado para outro requisito.

Cada requisito funcional apresenta:

- identificador e título;
- funcionalidade de origem;
- ator ou atores envolvidos;
- descrição;
- critério de aceitação.

Cada requisito não funcional apresenta:

- identificador e título;
- descrição;
- critério de verificação.

---

# 5. Requisitos Funcionais

## 5.1. F001 — Autenticação e Controlos de Acesso

### RF001 — Autenticar utilizador

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Atores:** Administrador, Rececionista, Veterinário, Auxiliar Veterinário, Cliente

**Descrição:**  
O sistema deve permitir a autenticação de um utilizador registado através das credenciais individuais associadas à respetiva conta.

**Critério de aceitação:**  
Dada uma conta com acesso permitido e credenciais válidas, quando o utilizador solicita autenticação, o sistema autentica o utilizador e concede acesso às funcionalidades autorizadas para o respetivo perfil.

### RF002 — Rejeitar autenticação inválida

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Atores:** Administrador, Rececionista, Veterinário, Auxiliar Veterinário, Cliente

**Descrição:**  
O sistema deve rejeitar tentativas de autenticação efetuadas com credenciais inválidas ou através de uma conta sem acesso permitido.

**Critério de aceitação:**  
Perante credenciais inválidas ou uma conta sem acesso permitido, o sistema não concede acesso às funcionalidades autenticadas.

### RF003 — Terminar sessão

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Atores:** Administrador, Rececionista, Veterinário, Auxiliar Veterinário, Cliente

**Descrição:**  
O sistema deve permitir a um utilizador autenticado terminar a respetiva sessão.

**Critério de aceitação:**  
Quando o utilizador solicita o fim da sessão, a sessão ativa deixa de permitir o acesso autenticado ao sistema.

### RF004 — Manter acesso autenticado válido

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Atores:** Administrador, Rececionista, Veterinário, Auxiliar Veterinário, Cliente

**Descrição:**  
O sistema deve manter o acesso autenticado do utilizador enquanto a respetiva sessão ou credencial de acesso permanecer válida.

**Critério de aceitação:**  
Após uma autenticação válida, o utilizador consegue aceder às operações autorizadas enquanto a autenticação permanecer válida e deixa de o conseguir quando esta terminar ou for invalidada.

### RF005 — Restringir funcionalidades por perfil

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Atores:** Administrador, Rececionista, Veterinário, Auxiliar Veterinário, Cliente

**Descrição:**  
O sistema deve limitar as funcionalidades disponíveis de acordo com o perfil do utilizador autenticado.

**Critério de aceitação:**  
Cada utilizador consegue executar apenas as operações autorizadas para o respetivo perfil.

### RF006 — Criar conta de funcionário

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Ator:** Administrador

**Descrição:**  
O sistema web deve permitir ao Administrador criar contas para os funcionários da clínica.

**Critério de aceitação:**  
Quando são fornecidos os dados obrigatórios válidos, o sistema cria uma nova conta de funcionário.

### RF007 — Atualizar conta de funcionário

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Ator:** Administrador

**Descrição:**  
O sistema web deve permitir ao Administrador atualizar os dados editáveis das contas dos funcionários, incluindo, no caso do perfil Auxiliar Veterinário, a autorização para registar informação não clínica de apoio.

**Critério de aceitação:**  
Após a alteração de dados válidos e a confirmação da operação, a conta apresenta os novos valores; se a conta tiver perfil Auxiliar Veterinário, a autorização de registo de informação não clínica de apoio fica igualmente atualizada.

### RF008 — Atribuir perfil a funcionário

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Ator:** Administrador

**Descrição:**  
O sistema web deve permitir ao Administrador atribuir a uma conta de funcionário um dos perfis internos previstos no sistema.

**Critério de aceitação:**  
Após a atribuição de um perfil válido, a conta do funcionário fica associada ao perfil selecionado e sujeita às respetivas permissões.

### RF009 — Bloquear conta de funcionário

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Ator:** Administrador

**Descrição:**  
O sistema web deve permitir ao Administrador bloquear uma conta de funcionário.

**Critério de aceitação:**  
Após o bloqueio, a conta deixa de permitir autenticação enquanto permanecer bloqueada.

### RF010 — Criar conta de Cliente

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Atores:** Administrador, Rececionista

**Descrição:**  
O sistema web deve permitir ao Administrador e ao Rececionista criar uma conta para um Cliente.

**Critério de aceitação:**  
Quando são fornecidos os dados obrigatórios válidos, o sistema cria uma nova conta de Cliente.

### RF011 — Atualizar conta de Cliente

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Atores:** Administrador, Rececionista

**Descrição:**  
O sistema web deve permitir ao Administrador e ao Rececionista atualizar os dados editáveis da conta de um Cliente.

**Critério de aceitação:**  
Após a alteração de dados válidos e a confirmação da operação, a conta do Cliente apresenta os novos valores.

### RF012 — Consultar dados gerais e configurações da clínica

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Ator:** Administrador

**Descrição:**  
O sistema web deve permitir ao Administrador consultar os dados gerais da clínica e as configurações do sistema.

**Critério de aceitação:**  
Ao aceder à área correspondente, o Administrador visualiza os dados gerais da clínica e as configurações atualmente registadas no sistema.

### RF013 — Atualizar dados gerais e configurações da clínica

**Funcionalidade de origem:** F001 — Autenticação e Controlos de Acesso  
**Ator:** Administrador

**Descrição:**  
O sistema web deve permitir ao Administrador atualizar os dados gerais da clínica e as configurações do sistema.

**Critério de aceitação:**  
Após a alteração de dados válidos e a confirmação da operação, os novos dados gerais ou configurações ficam registados no sistema.

---

## 5.2. F002 — Gestão de pacientes (Fichas dos animais)

### RF014 — Registar paciente

**Funcionalidade de origem:** F002 — Gestão de pacientes (Fichas dos animais)  
**Atores:** Rececionista, Veterinário

**Descrição:**  
O sistema web deve permitir ao Rececionista e ao Veterinário criar a ficha de um paciente, registando os respetivos dados administrativos e associando-o inequivocamente ao respetivo proprietário.

**Critério de aceitação:**  
Quando são fornecidos os dados obrigatórios válidos e é selecionado um Cliente existente, o sistema cria a ficha do paciente e estabelece a associação ao respetivo proprietário.

### RF015 — Consultar ficha do paciente

**Funcionalidade de origem:** F002 — Gestão de pacientes (Fichas dos animais)  
**Atores:** Rececionista, Veterinário

**Descrição:**  
O sistema web deve permitir ao Rececionista e ao Veterinário consultar os dados administrativos da ficha de um paciente.

**Critério de aceitação:**  
Ao selecionar um paciente existente, o sistema apresenta os dados administrativos registados na respetiva ficha.

### RF016 — Atualizar dados administrativos do paciente

**Funcionalidade de origem:** F002 — Gestão de pacientes (Fichas dos animais)  
**Atores:** Rececionista, Veterinário

**Descrição:**  
O sistema web deve permitir ao Rececionista e ao Veterinário atualizar os dados administrativos da ficha de um paciente.

**Critério de aceitação:**  
Após a alteração de dados válidos e a confirmação da operação, a ficha do paciente apresenta os novos valores.

### RF017 — Pesquisar pacientes por proprietário

**Funcionalidade de origem:** F002 — Gestão de pacientes (Fichas dos animais)  
**Atores:** Rececionista, Veterinário

**Descrição:**  
O sistema web deve permitir pesquisar os pacientes associados a um determinado Cliente.

**Critério de aceitação:**  
Ao pesquisar ou selecionar um Cliente existente, o sistema apresenta os pacientes associados a esse proprietário.

### RF018 — Consultar animais do Cliente

**Funcionalidade de origem:** F002 — Gestão de pacientes (Fichas dos animais)  
**Ator:** Cliente

**Descrição:**  
A aplicação móvel deve permitir ao Cliente consultar os dados básicos dos animais associados à sua conta.

**Critério de aceitação:**  
Um Cliente autenticado consegue visualizar apenas os animais associados à sua própria conta e os respetivos dados disponibilizados para consulta.

---

## 5.3. F003 — Agendamento de consultas

### RF019 — Gerir horários de funcionamento

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Administrador

**Descrição:**  
O sistema web deve permitir ao Administrador definir os horários de funcionamento utilizados na gestão da agenda da clínica.

**Critério de aceitação:**  
Uma configuração de horário é aceite quando cada período tem hora de início anterior à hora de fim e não se sobrepõe a outro período do mesmo dia; um dia pode conter mais do que um período. Depois da confirmação, esses períodos são utilizados na determinação das disponibilidades para marcação.

### RF020 — Consultar disponibilidade dos Veterinários

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista consultar a disponibilidade dos Veterinários para um determinado período.

**Critério de aceitação:**  
Ao selecionar um Veterinário e um período, o sistema apresenta os horários livres calculados a partir do horário de funcionamento da clínica e das marcações não canceladas já existentes nesse intervalo.

### RF021 — Criar marcação

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista criar uma marcação, associando-a a um paciente, a uma data, a uma hora e a um Veterinário.

**Critério de aceitação:**  
Quando são fornecidos os dados obrigatórios válidos e existe disponibilidade, o sistema cria a marcação e associa-a ao paciente e ao Veterinário selecionados.

### RF022 — Alterar marcação

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista alterar os dados de uma marcação existente.

**Critério de aceitação:**  
Após a alteração de dados válidos e a confirmação da operação, a marcação apresenta os novos valores.

### RF023 — Atualizar estado da marcação

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista atualizar o estado de uma marcação segundo as transições previstas: de `Pendente` para `Confirmada` ou `Cancelada`, e de `Confirmada` para `Realizada` ou `Cancelada`. `Realizada` e `Cancelada` são estados finais.

**Critério de aceitação:**  
Após a seleção de uma transição permitida e a confirmação da operação, a marcação apresenta o novo estado registado; uma transição não prevista é rejeitada.

### RF024 — Consultar agenda da clínica

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Atores:** Rececionista, Veterinário

**Descrição:**  
O sistema web deve permitir ao Rececionista consultar a agenda da clínica e ao Veterinário consultar as consultas que lhe estão atribuídas.

**Critério de aceitação:**  
Ao consultar a agenda, o Rececionista visualiza as marcações do período selecionado e o Veterinário visualiza as consultas que lhe estão atribuídas.

### RF025 — Registar chegada à consulta

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista registar a chegada de um Cliente e do respetivo animal a uma consulta marcada.

**Critério de aceitação:**  
Após o registo da chegada, a consulta correspondente fica identificada com a informação de chegada registada.

### RF026 — Consultar informação de preparação da consulta

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Auxiliar Veterinário

**Descrição:**  
O sistema web deve permitir ao Auxiliar Veterinário consultar a informação necessária à preparação e acompanhamento de uma consulta, de acordo com as suas permissões.

**Critério de aceitação:**  
Ao selecionar uma consulta abrangida pelas suas permissões, o Auxiliar Veterinário consegue visualizar a informação necessária à respetiva preparação ou acompanhamento sem obter permissões de alteração clínica.

### RF027 — Registar informação não clínica de apoio

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Auxiliar Veterinário

**Descrição:**  
O sistema web deve permitir ao Auxiliar Veterinário registar informação não clínica de apoio à preparação ou acompanhamento de uma consulta quando essa autorização estiver ativa na respetiva conta de funcionário.

**Critério de aceitação:**  
Quando a autorização de registo de apoio está ativa na conta do Auxiliar Veterinário e é fornecida informação válida, o sistema regista a informação de apoio sem alterar registos clínicos ou a marcação.

### RF028 — Consultar marcações futuras

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Cliente

**Descrição:**  
A aplicação móvel deve permitir ao Cliente consultar as marcações futuras associadas aos seus animais.

**Critério de aceitação:**  
O Cliente autenticado visualiza apenas as marcações futuras associadas aos animais da sua conta e não dispõe de operações para criar, alterar ou cancelar marcações.

### RF029 — Consultar histórico administrativo de marcações e consultas

**Funcionalidade de origem:** F003 — Agendamento de consultas  
**Ator:** Cliente

**Descrição:**  
A aplicação móvel deve permitir ao Cliente consultar o histórico de marcações passadas e de consultas realizadas associadas aos seus animais, apresentando apenas informação administrativa.

**Critério de aceitação:**  
O Cliente autenticado visualiza apenas o histórico associado aos seus próprios animais, com informação administrativa como data, animal e Veterinário, sem acesso a registos clínicos.

---

## 5.4. F004 — Registo clínico e tratamentos

### RF030 — Registar notas clínicas

**Funcionalidade de origem:** F004 — Registo clínico e tratamentos  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário registar notas clínicas associadas a uma consulta e a um paciente.

**Critério de aceitação:**  
Após a confirmação do registo, as notas clínicas ficam associadas à consulta e ao paciente correspondentes.

### RF031 — Registar diagnóstico

**Funcionalidade de origem:** F004 — Registo clínico e tratamentos  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário registar um diagnóstico associado a uma consulta e a um paciente.

**Critério de aceitação:**  
Após a confirmação do registo, o diagnóstico fica associado à consulta e ao paciente correspondentes.

### RF032 — Registar tratamento

**Funcionalidade de origem:** F004 — Registo clínico e tratamentos  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário registar um tratamento associado a uma consulta e a um paciente.

**Critério de aceitação:**  
Após a confirmação do registo, o tratamento fica associado à consulta e ao paciente correspondentes.

### RF033 — Registar prescrição

**Funcionalidade de origem:** F004 — Registo clínico e tratamentos  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário registar uma prescrição de medicamentos associada a uma consulta e a um paciente.

**Critério de aceitação:**  
Após a confirmação do registo, a prescrição fica associada à consulta e ao paciente correspondentes.

### RF034 — Atualizar registo clínico

**Funcionalidade de origem:** F004 — Registo clínico e tratamentos  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir exclusivamente ao Veterinário alterar notas clínicas, diagnósticos, tratamentos e prescrições anteriormente registados.

**Critério de aceitação:**  
Após a alteração de informação clínica válida e a confirmação da operação, o registo clínico apresenta os novos valores e mantém-se associado à respetiva consulta e ao paciente.

### RF035 — Consultar histórico clínico

**Funcionalidade de origem:** F004 — Registo clínico e tratamentos  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário consultar o histórico clínico de um paciente.

**Critério de aceitação:**  
Ao consultar um paciente, o Veterinário consegue visualizar os registos clínicos anteriormente armazenados e associados a esse paciente.

### RF036 — Consultar informação clínica autorizada

**Funcionalidade de origem:** F004 — Registo clínico e tratamentos  
**Ator:** Auxiliar Veterinário

**Descrição:**  
O sistema web deve permitir ao Auxiliar Veterinário consultar a informação clínica necessária às suas funções de preparação e acompanhamento, sem permitir a criação ou alteração de registos clínicos.

**Critério de aceitação:**  
O Auxiliar Veterinário consegue consultar apenas a informação clínica disponibilizada às suas funções e não dispõe de operações para criar ou alterar notas clínicas, diagnósticos, tratamentos ou prescrições.

---

## 5.5. F005 — Gestão de contas e pagamentos

> **Fora do âmbito (Declaração de Âmbito v05).** A funcionalidade F005 passou a fora do âmbito por transição para o não-âmbito, autorizada pelo docente na reunião de 20/10/2026 e confirmada pelo e-mail de homologação de 21/10/2026 (Regulamento da UC, secção 7.5). Os requisitos RF037 a RF042 estão **descontinuados**: o texto abaixo é o da v01, conservado apenas para a rastreabilidade, e não é compromisso do grupo. Os identificadores permanecem reservados.

### RF037 — Registar custos

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F005 — Gestão de contas e pagamentos  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista registar os custos associados às consultas e tratamentos prestados pela clínica, associando os respetivos valores ao Cliente.

**Critério de aceitação:**  
Após a confirmação do registo, o custo fica associado ao Cliente, ao paciente e à consulta ou tratamento que o originou, quando essa referência exista.

### RF038 — Consultar valores em dívida

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F005 — Gestão de contas e pagamentos  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista consultar os valores em dívida associados a um Cliente.

**Critério de aceitação:**  
Ao consultar a situação administrativa de um Cliente, o sistema apresenta os valores registados como pendentes.

### RF039 — Registar pagamento ao balcão

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F005 — Gestão de contas e pagamentos  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista registar manualmente um pagamento efetuado ao balcão.

**Critério de aceitação:**  
Após o registo de um pagamento válido, o valor fica associado à conta administrativa selecionada e é refletido no montante pago e no valor em dívida.

### RF040 — Atualizar estado de pagamento

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F005 — Gestão de contas e pagamentos  
**Ator:** Rececionista

**Descrição:**  
O sistema deve permitir ao Rececionista corrigir administrativamente o estado de pagamento de um valor registado, utilizando os estados `Pendente` ou `Pago`.

**Critério de aceitação:**  
A correção só é aceite quando o estado escolhido é coerente com o montante em dívida: `Pago` quando o valor em dívida é zero e `Pendente` quando existe valor em dívida; a justificação da correção fica registada.

### RF041 — Fechar conta administrativa

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F005 — Gestão de contas e pagamentos  
**Ator:** Rececionista

**Descrição:**  
O sistema web deve permitir ao Rececionista fechar uma conta administrativa paga e sem valor em dívida, associada aos serviços prestados a um Cliente.

**Critério de aceitação:**  
Quando a conta tem estado `Pago` e valor em dívida igual a zero, após a confirmação do fecho fica identificada como encerrada e deixa de ser apresentada como conta administrativa em aberto; caso contrário, o fecho é rejeitado.

### RF042 — Consultar despesas e estados de pagamento no mobile

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F005 — Gestão de contas e pagamentos  
**Ator:** Cliente

**Descrição:**  
A aplicação móvel deve permitir ao Cliente consultar as despesas associadas aos serviços prestados aos seus animais e os respetivos estados de pagamento.

**Critério de aceitação:**  
O Cliente autenticado visualiza apenas as despesas associadas aos seus próprios animais e os respetivos estados, sem dispor de operações para alterar valores, registar pagamentos ou efetuar pagamentos pela aplicação.

---

## 5.6. F006 — Boletim de Vacinas e Desparasitações

### RF043 — Registar vacinação

**Funcionalidade de origem:** F006 — Boletim de Vacinas e Desparasitações  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário registar uma vacinação realizada a um paciente, incluindo a data de administração, o tipo de vacina ou produto utilizado, o lote e, quando pretenda planear uma próxima administração, o respetivo intervalo em dias.

**Critério de aceitação:**  
Após a confirmação do registo, a vacinação fica associada ao paciente e apresenta os dados introduzidos; quando é definido um intervalo válido, fica também disponível a data prevista para a próxima administração.

### RF044 — Registar desparasitação

**Funcionalidade de origem:** F006 — Boletim de Vacinas e Desparasitações  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário registar uma desparasitação realizada a um paciente, incluindo a data de administração, o produto utilizado, o lote e, quando pretenda planear uma próxima administração, o respetivo intervalo em dias.

**Critério de aceitação:**  
Após a confirmação do registo, a desparasitação fica associada ao paciente e apresenta os dados introduzidos; quando é definido um intervalo válido, fica também disponível a data prevista para a próxima administração.

### RF045 — Calcular próxima administração

**Funcionalidade de origem:** F006 — Boletim de Vacinas e Desparasitações  
**Ator:** Veterinário

**Descrição:**  
O sistema deve calcular automaticamente a data prevista para a próxima administração de uma vacina ou desparasitante com base na data de administração registada e no intervalo definido pelo Veterinário.

**Critério de aceitação:**  
Quando existe uma data de administração válida e um intervalo definido, o sistema apresenta a data prevista para a próxima administração resultante desses dados.

### RF046 — Consultar boletim no sistema web

**Funcionalidade de origem:** F006 — Boletim de Vacinas e Desparasitações  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário consultar o histórico completo de vacinação e desparasitação de um paciente.

**Critério de aceitação:**  
Ao selecionar um paciente, o Veterinário consegue visualizar os registos de vacinação e desparasitação associados e as próximas administrações previstas quando existam.

### RF047 — Consultar boletim na aplicação móvel

**Funcionalidade de origem:** F006 — Boletim de Vacinas e Desparasitações  
**Ator:** Cliente

**Descrição:**  
A aplicação móvel deve permitir ao Cliente consultar o histórico de vacinação e desparasitação dos seus próprios animais e as datas das próximas administrações planeadas.

**Critério de aceitação:**  
O Cliente autenticado consegue visualizar apenas o boletim dos animais associados à sua conta, incluindo o histórico e as próximas administrações previstas.

---

## 5.7. F007 — Gestão de internamento (Hospitalização)

> **Fora do âmbito (Declaração de Âmbito v05).** A funcionalidade F007 passou a fora do âmbito por transição para o não-âmbito, autorizada pelo docente na reunião de 20/10/2026 e confirmada pelo e-mail de homologação de 21/10/2026 (Regulamento da UC, secção 7.5). Os requisitos RF048 a RF052 estão **descontinuados**: o texto abaixo é o da v01, conservado apenas para a rastreabilidade, e não é compromisso do grupo. Os identificadores permanecem reservados.

### RF048 — Registar internamento

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F007 — Gestão de internamento (Hospitalização)  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário registar o internamento de um paciente.

**Critério de aceitação:**  
Quando são fornecidos os dados obrigatórios válidos, o sistema cria um internamento associado ao paciente.

### RF049 — Associar alojamento ao internamento

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F007 — Gestão de internamento (Hospitalização)  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir ao Veterinário associar ao internamento um alojamento disponível atribuído ao paciente.

**Critério de aceitação:**  
Após a confirmação da associação, o internamento apresenta o alojamento selecionado; um alojamento já ocupado por outro internamento ativo não pode ser associado.

### RF050 — Consultar painel de internamentos

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F007 — Gestão de internamento (Hospitalização)  
**Atores:** Veterinário, Auxiliar Veterinário

**Descrição:**  
O sistema web deve disponibilizar um painel com os pacientes atualmente internados e os respetivos alojamentos ao Veterinário e ao Auxiliar Veterinário.

**Critério de aceitação:**  
O painel apresenta os internamentos sem alta médica registada e permite identificar os respetivos pacientes e alojamentos quando associados.

### RF051 — Atualizar estado clínico do internamento

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F007 — Gestão de internamento (Hospitalização)  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir exclusivamente ao Veterinário atualizar o estado clínico de um paciente internado entre `EmObservacao` e `Recobro`. A passagem para `AltaMedica` é efetuada pelo registo de alta previsto no RF052.

**Critério de aceitação:**  
Após a seleção de `EmObservacao` ou `Recobro` e a confirmação da atualização, o novo estado clínico fica registado no internamento correspondente; a operação é rejeitada após a alta e não está disponível ao Auxiliar Veterinário.

### RF052 — Registar alta médica

**Estado:** Descontinuado (fora do âmbito, Declaração de Âmbito v05)  
**Funcionalidade de origem:** F007 — Gestão de internamento (Hospitalização)  
**Ator:** Veterinário

**Descrição:**  
O sistema web deve permitir exclusivamente ao Veterinário registar a alta médica de um paciente internado.

**Critério de aceitação:**  
Após o registo da alta médica, o internamento deixa de ser apresentado como ativo e a operação de alta não está disponível ao Auxiliar Veterinário.

---

# 6. Requisitos Não Funcionais

## RNF001 — Controlo de acesso às operações

**Descrição:**  
O sistema deve impedir que um utilizador execute operações para as quais o respetivo perfil não possui autorização.

**Critério de verificação:**  
Os testes de autorização demonstram que cada um dos cinco perfis consegue executar apenas as operações que lhe estão atribuídas.

## RNF002 — Isolamento dos dados de Clientes

**Descrição:**  
O sistema deve impedir que um Cliente consulte informação pertencente a animais ou dados administrativos associados a outro Cliente.

**Critério de verificação:**  
Os testes com contas de Clientes diferentes demonstram que cada Cliente apenas consegue consultar os dados associados à sua própria conta.

## RNF003 — Separação dos canais de utilização

**Descrição:**  
O sistema deve respeitar a separação de canais definida no âmbito homologado: os perfis internos utilizam a aplicação web e o perfil Cliente utiliza exclusivamente a aplicação móvel.

**Critério de verificação:**  
A verificação dos canais demonstra que os perfis internos não dispõem das suas funcionalidades na aplicação móvel e que o Cliente não dispõe das suas funcionalidades na aplicação web.

## RNF004 — Validação dos dados de entrada

**Descrição:**  
O sistema deve validar os campos obrigatórios e as restrições aplicáveis antes de aceitar operações de criação ou alteração de dados.

**Critério de verificação:**  
Os testes demonstram que dados que violem uma regra de validação definida são rejeitados e que dados válidos são aceites.

## RNF005 — Persistência dos dados confirmados

**Descrição:**  
Os dados registados e confirmados com sucesso devem permanecer disponíveis para consulta posterior.

**Critério de verificação:**  
Após um registo válido, os dados permanecem disponíveis numa consulta posterior ao sistema.

## RNF006 — Dependência de ligação à rede

**Descrição:**  
O sistema deve operar pressupondo uma ligação ativa à rede, não sendo exigido funcionamento offline.

**Critério de verificação:**  
A documentação e os testes demonstram que as funcionalidades previstas são disponibilizadas em ambiente com ligação à rede e que não existe um modo offline suportado.

---

# 7. Elementos fora do âmbito e limites transversais

Não fazem parte do âmbito desta versão do sistema:

- a gestão de contas e pagamentos (F005): registo de custos, consulta de valores em dívida, registo de pagamentos ao balcão, estado de pagamento, fecho de contas e consulta de despesas pelo Cliente na aplicação móvel (RF037–RF042, descontinuados);
- a gestão de internamento (F007): registo de internamentos, alojamentos, painel de internamentos, estado clínico do internamento e alta médica (RF048–RF052, descontinuados);
- autenticação através de fornecedores externos de identidade, como Google ou Facebook;
- autenticação de dupla camada ou multifator;
- integração com o SIAC ou outras bases de dados nacionais de registo ou saúde animal;
- integração com calendários externos, como Google Calendar ou Outlook;
- envio automático de notificações ou lembretes por SMS ou e-mail;
- geração automática de prescrições em PDF para impressão ou envio por e-mail;
- pagamentos eletrónicos através da aplicação móvel ou integração com MB Way, referências Multibanco, TPA ou outros meios de pagamento externos;
- certificação fiscal e comunicação direta com a Autoridade Tributária;
- leitura automática de códigos de barras ou QR de vacinas ou outros produtos;
- acompanhamento do internamento pelo Cliente através da aplicação móvel;
- integração do internamento com monitores de sinais vitais, câmaras ou outros equipamentos especializados;
- integração com leitores de microchips, balanças ou equipamentos de diagnóstico por imagem;
- migração de dados históricos ou clínicos provenientes de aplicações anteriores;
- funcionamento offline garantido;
- colocação efetiva do sistema em produção numa clínica veterinária real no âmbito desta unidade curricular.

O sistema será desenvolvido e demonstrado num ambiente académico controlado, utilizando dados fictícios para fins de avaliação.

---

# 8. Rastreabilidade e evolução

Cada requisito definido neste documento deve manter o mesmo identificador ao longo do projeto.

A Checklist de Aprovação dos Requisitos utiliza o ID e o título definidos nesta especificação.

A Requirements Traceability Matrix (RTM) relaciona cada requisito com os elementos aplicáveis do projeto, incluindo casos de uso, componentes de arquitetura, Issues ou tarefas, implementação, testes, evidências e eventuais defeitos ou não conformidades.

Se um requisito for alterado após a aprovação desta versão, a alteração deve manter a respetiva rastreabilidade. Se um requisito for descontinuado, o seu identificador permanece reservado e não deve ser reutilizado para outro requisito.

Nesta versão foram descontinuados os requisitos RF037–RF042 (F005) e RF048–RF052 (F007). Na Requirements Traceability Matrix v02 (`m3-requirements-traceability-matrix-v02.xlsx`) mantêm-se as respetivas linhas com o estado «7 - Descontinuado».

---

# 9. Pontos em aberto

| N.º | Ponto | Para decidir |
|---|---|---|
| 1 | **Requisitos RF053 a RF056.** O Backlog de Projeto v03 (rascunho) cita quatro requisitos que não existem nesta especificação: Cliente sem conta de acesso (RF053, RF054), horários regulares de atendimento por Veterinário (RF055) e atualização de um registo de vacinação ou desparasitação (RF056). | Se o grupo os quiser em âmbito, têm de ser escritos (descrição e critério de aceitação) e aprovados antes de aparecerem na API, nos casos de uso, na RTM ou nos testes. Esta v02 não os inclui. |
| 2 | **Checklist de Aprovação dos Requisitos.** A checklist da m2 aprovou a v01. | Decidir se a v02 exige uma nova checklist. |
| 3 | **Casos de uso UC12 e UC13.** Estão com pontos «A confirmar»: o Auxiliar Veterinário vê apenas parte do registo clínico? Os Veterinários leem registos de colegas? A resposta pode alterar RF035 e RF036. | Decisão do grupo, no Backlog Refinement. |

---

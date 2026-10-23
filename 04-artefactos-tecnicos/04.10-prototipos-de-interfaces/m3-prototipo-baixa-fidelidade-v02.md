# Protótipo de baixa fidelidade — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Unidade curricular | Laboratório de Desenvolvimento de Software — LDS |
| Grupo | G08 |
| Milestone | m3 |
| Versão | v02 |
| Substitui (sem alterar) | `m2-prototipo-baixa-fidelidade-v01.pdf` |

## 1. Objetivo

Reunir num único ficheiro Markdown o código-fonte PlantUML dos ecrãs em âmbito do protótipo de baixa fidelidade, para que o protótipo seja textual, editável e versionável (Regulamento de Funcionamento da UC v19, secção sobre diagramas). O conteúdo de cada ecrã é o mesmo dos ficheiros `.puml` conservados em `fonte-prototipo-baixa-fidelidade/` na entrega m2.

## 2. Histórico de versões

| Versão | Milestone | Descrição |
|---|---|---|
| v01 | m2 | Protótipo em PDF, com os ficheiros `.puml` soltos em `fonte-prototipo-baixa-fidelidade/`. |
| v02 | m3 | Código PlantUML dos ecrãs em âmbito incorporado neste ficheiro Markdown. Os ecrãs W14, W15, W17, W18 e M05 (F005 e F007) ficam fora do âmbito, de acordo com a Declaração de Âmbito v05: aparecem no índice e na secção 4 apenas como «fora do âmbito», sem código, e foram retirados do mapa de navegação. Os ficheiros `.puml` e o PDF da m2 mantêm-se inalterados. |

## 3. Índice dos ecrãs

| Fonte | Ecrã | Atores | Caso(s) de uso | Requisitos |
|---|---|---|---|---|
| `00-mapa-navegacao.puml` | Mapa de Navegação - Gestão de Clínica Veterinária | - | - | - |
| `W01-login-web.puml` | W01 — Login Web | Administrador, Rececionista, Veterinário, Auxiliar Veterinário | UC01 | RF001, RF002, RF004, RF005 |
| `W02-funcionarios.puml` | W02 — Lista de Funcionários | - | UC03 | RF007, RF008, RF009 |
| `W03-editar-funcionario.puml` | W03 — Criar / Editar Funcionário | - | UC03 | RF006, RF007, RF008, RF009 |
| `W04-clientes.puml` | W04 — Lista e Pesquisa de Clientes | Administrador, Rececionista | UC04 | RF010, RF011 |
| `W05-detalhe-cliente.puml` | W05 — Detalhe / Criar / Editar Cliente | Administrador, Rececionista | UC04 | RF010, RF011 |
| `W06-clinica-config-horarios.puml` | W06 — Clínica, Configurações e Horários | - | UC05 / UC09 | RF012, RF013, RF019 |
| `W07-pacientes.puml` | W07 — Lista e Pesquisa de Pacientes | Rececionista, Veterinário | UC06 / UC07 | RF014, RF017 |
| `W08-ficha-paciente.puml` | W08 — Ficha do Paciente | Rececionista, Veterinário | UC06 / UC07 / UC10 | RF014, RF015, RF016, RF026 |
| `W09-agenda.puml` | W09 — Agenda da Clínica | Rececionista, Veterinário | UC09 | RF020, RF024 |
| `W10-marcacao-form.puml` | W10 — Criar / Editar Marcação | - | UC09 | RF020, RF021, RF022, RF023 |
| `W11-detalhe-marcacao.puml` | W11 — Detalhe da Marcação | Rececionista, Veterinário | UC09 | RF023, RF024, RF025 |
| `W12-preparacao-consulta.puml` | W12 — Preparação e Apoio à Consulta | - | UC10 | RF026, RF027 |
| `W13-registo-clinico.puml` | W13 — Registo e Histórico Clínico | - | UC12 / UC13 | RF030, RF031, RF032, RF033, RF034, RF035, RF036 |
| `W14-contas-dividas.puml` | W14 — Contas, Custos e Valores em Dívida — **fora do âmbito** | - | UC14 (fora do âmbito) | RF037, RF038 (fora do âmbito) |
| `W15-conta-pagamentos.puml` | W15 — Detalhe da Conta e Pagamentos — **fora do âmbito** | - | UC14 (fora do âmbito) | RF039, RF040, RF041 (fora do âmbito) |
| `W16-vacinacao.puml` | W16 — Vacinação e Desparasitação | - | UC16 | RF043, RF044, RF045, RF046 |
| `W17-internamentos.puml` | W17 — Painel de Internamentos — **fora do âmbito** | - | UC18 (fora do âmbito) | RF048, RF049, RF050 (fora do âmbito) |
| `W18-detalhe-internamento.puml` | W18 — Detalhe do Internamento — **fora do âmbito** | - | UC18 (fora do âmbito) | RF048, RF049, RF050, RF051, RF052 (fora do âmbito) |
| `M01-login-mobile.puml` | M01 — Login Mobile | - | UC01 | RF001, RF002, RF004, RF005 |
| `M02-meus-animais.puml` | M02 — Os Meus Animais | - | UC08 | RF018 |
| `M03-detalhe-animal.puml` | M03 — Detalhe do Animal | - | UC08 | RF018 |
| `M04-consultas.puml` | M04 — Consultas | - | UC11 | RF028, RF029 |
| `M05-despesas.puml` | M05 — Despesas e Pagamentos — **fora do âmbito** | - | UC15 (fora do âmbito) | RF042 (fora do âmbito) |
| `M06-boletim.puml` | M06 — Boletim de Vacinação e Desparasitação | - | UC17 | RF047 |

## 4. Ecrãs

### 4.1. Mapa de Navegação - Gestão de Clínica Veterinária

Fonte: `00-mapa-navegacao.puml`. Nesta versão foram retirados os ecrãs W14, W15, W17, W18 e M05 (F005 e F007, fora do âmbito).

```plantuml {kroki=true}
@startuml
left to right direction
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam nodesep 35
skinparam ranksep 45
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title Mapa de Navegação - Gestão de Clínica Veterinária

rectangle "APLICAÇÃO WEB" {
  rectangle "W01\nLogin Web" as W01

  rectangle "Administrador\n----\nW02 Funcionários\nW03 Criar/Editar Funcionário\nW04 Clientes\nW05 Detalhe Cliente\nW06 Clínica/Config./Horários" as ADMIN

  rectangle "Rececionista\n----\nW04 Clientes\nW05 Detalhe Cliente\nW07 Pacientes\nW08 Ficha Paciente\nW09 Agenda\nW10 Criar/Editar Marcação\nW11 Detalhe Marcação" as REC

  rectangle "Veterinário\n----\nW07 Pacientes\nW08 Ficha Paciente\nW09 Agenda própria\nW11 Detalhe Marcação\nW13 Registo Clínico\nW16 Vacinação/Desparasitação" as VET

  rectangle "Auxiliar Veterinário\n----\nW08 Ficha Paciente (leitura)\nW12 Consultas disponíveis\n+ Preparação/Apoio\nW13 Informação clínica (leitura)" as AUX

  W01 --> ADMIN
  W01 --> REC
  W01 --> VET
  W01 --> AUX

  ADMIN -[hidden]-> REC
  REC -[hidden]-> VET
  VET -[hidden]-> AUX
}

rectangle "APLICAÇÃO MOBILE" {
  rectangle "M01\nLogin Mobile" as M01
  rectangle "Cliente" as CLI
  rectangle "M02\nOs Meus Animais" as M02
  rectangle "M03\nDetalhe do Animal" as M03
  rectangle "M04\nConsultas" as M04
  rectangle "M06\nBoletim" as M06

  M01 --> CLI
  CLI --> M02
  CLI --> M04
  M02 --> M03
  M03 --> M06
}

note bottom of ADMIN
Gestão de utilizadores, clientes,
configuração e horários.
end note

note bottom of REC
Gestão administrativa, agenda,
marcações e chegadas.
end note

note bottom of VET
W07 permite pesquisa por proprietário.\nSem acesso à gestão W04.\nAgenda própria, informação clínica\ne vacinação.
end note

note bottom of AUX
W12 inclui seleção das consultas disponíveis.\nPreparação/apoio e leitura autorizada.\nSem escrita clínica.
end note

note bottom of CLI
Acesso de consulta e ação global de terminar sessão.
Sem criar, alterar ou cancelar marcações
nem editar animais.
end note
@enduml
```

### 4.2. W01 — Login Web

Fonte: `W01-login-web.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W01 — Login Web

rectangle "CLÍNICA VETERINÁRIA\n\nINICIAR SESSÃO\n\nEmail\n[____________________________]\n\nPalavra-passe\n[____________________________]\n\n[ ENTRAR ]\n\nMensagem de erro, quando aplicável" as SCREEN

note bottom of SCREEN
Atores: Administrador, Rececionista, Veterinário, Auxiliar Veterinário
UC01
RF001, RF002, RF004, RF005
end note
@enduml
```

### 4.3. W02 — Lista de Funcionários

Fonte: `W02-funcionarios.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W02 — Lista de Funcionários

rectangle "CLÍNICA VETERINÁRIA   |   FUNCIONÁRIOS   |   [Terminar sessão]\n\n[ + Novo funcionário ]     Pesquisar: [________________]\n\nNome              Email                    Perfil             Estado\n-----------------------------------------------------------------------\nAna Silva         ana@clinica.pt           Veterinário        Ativo\nJoão Costa        joao@clinica.pt          Rececionista       Ativo\nMaria Lima        maria@clinica.pt         Auxiliar           Bloqueado\n\n[Ver / Editar]   [Alterar perfil]   [Bloquear conta]" as SCREEN

note bottom of SCREEN
Ator: Administrador
UC03
RF007, RF008, RF009
end note
@enduml
```

### 4.4. W03 — Criar / Editar Funcionário

Fonte: `W03-editar-funcionario.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W03 — Criar / Editar Funcionário

rectangle "CLÍNICA VETERINÁRIA   |   FUNCIONÁRIOS > NOVO / EDITAR\n\nNome              [____________________________]\nEmail             [____________________________]\nTelefone          [____________________________]\nPalavra-passe inicial [________________________]  (só ao criar)\nPerfil            [ Administrador v ]\nPode registar apoio [ ]  (apenas Auxiliar Veterinário)\n\n[ CANCELAR ]                         [ GUARDAR ]\n\nAo editar: opção [ BLOQUEAR CONTA ]" as SCREEN

note bottom of SCREEN
Ator: Administrador
UC03
RF006, RF007, RF008, RF009
end note
@enduml
```

### 4.5. W04 — Lista e Pesquisa de Clientes

Fonte: `W04-clientes.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W04 — Lista e Pesquisa de Clientes

rectangle "CLÍNICA VETERINÁRIA   |   CLIENTES   |   [Terminar sessão]\n\n[ + Novo cliente ]     Pesquisar: [________________________]\n\nNome                 NIF            Telefone          Email\n----------------------------------------------------------------\nCarlos Sousa         123456789      912345678         carlos@email.pt\nRita Ferreira        987654321      919876543         rita@email.pt\n\n[ VER DETALHE ]   [ EDITAR ]" as SCREEN

note bottom of SCREEN
Atores: Administrador, Rececionista
UC04
RF010, RF011
A pesquisa por proprietário do Veterinário é efetuada em W07
end note
@enduml
```

### 4.6. W05 — Detalhe / Criar / Editar Cliente

Fonte: `W05-detalhe-cliente.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W05 — Detalhe / Criar / Editar Cliente

rectangle "CLÍNICA VETERINÁRIA   |   CLIENTES > DETALHE\n\nDADOS DO CLIENTE\nNome              [____________________________]\nNIF               [____________________________]\nTelefone          [____________________________]\nEmail             [____________________________]\nMorada            [____________________________]\nPalavra-passe inicial [________________________]  (só ao criar)\n\nANIMAIS ASSOCIADOS\n- Kiko | Cão | Labrador      [Ver paciente]\n- Lua  | Gato | Europeu      [Ver paciente]\n\n[ VOLTAR ]                  [ GUARDAR ALTERAÇÕES ]" as SCREEN

note bottom of SCREEN
Atores: Administrador, Rececionista
UC04
RF010, RF011
end note
@enduml
```

### 4.7. W06 — Clínica, Configurações e Horários

Fonte: `W06-clinica-config-horarios.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W06 — Clínica, Configurações e Horários

rectangle "CLÍNICA VETERINÁRIA   |   CONFIGURAÇÃO DA CLÍNICA\n\nDADOS GERAIS\nNome da clínica     [________________________]\nMorada              [________________________]\nTelefone            [________________________]\nEmail               [________________________]\n\nHORÁRIO DE FUNCIONAMENTO\nSegunda   [09:00] - [13:00]\nSegunda   [14:00] - [19:00]\n[ + Adicionar período ]\nTerça     [09:00] - [19:00]\nQuarta    [09:00] - [19:00]\nQuinta    [09:00] - [19:00]\nSexta     [09:00] - [19:00]\nSábado    [09:00] - [13:00]\nDomingo   [ Encerrado ]\n\n[ CANCELAR ]                         [ GUARDAR ]" as SCREEN

note bottom of SCREEN
Ator: Administrador
UC05 / UC09
RF012, RF013, RF019
end note
@enduml
```

### 4.8. W07 — Lista e Pesquisa de Pacientes

Fonte: `W07-pacientes.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W07 — Lista e Pesquisa de Pacientes

rectangle "CLÍNICA VETERINÁRIA   |   PACIENTES

[ + Registar paciente ]     Pesquisar paciente: [____________________]

PESQUISA POR PROPRIETÁRIO
Proprietário: [________________________]   [ PESQUISAR ]
Resultados: Carlos Sousa | Rita Ferreira | ...
Selecionado: [ Carlos Sousa v ]

Nome        Espécie       Raça          Proprietário
------------------------------------------------------
Kiko        Cão           Labrador      Carlos Sousa
Lua         Gato          Europeu       Rita Ferreira

[ VER FICHA ]" as SCREEN

note bottom of SCREEN
Atores: Rececionista, Veterinário
UC06 / UC07
RF014, RF017
A pesquisa de proprietário ocorre aqui; o Veterinário não acede a W04
end note
@enduml
```

### 4.9. W08 — Ficha do Paciente

Fonte: `W08-ficha-paciente.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W08 — Ficha do Paciente

rectangle "CLÍNICA VETERINÁRIA   |   PACIENTES > FICHA / REGISTO\n\nDADOS ADMINISTRATIVOS\nNome              [ Kiko ______________________ ]\nProprietário      [ Carlos Sousa v ]\nEspécie           [ Cão v ]     Raça [ Labrador v ]\nSexo              [ Macho v ]\nData nascimento   [12/03/2021]\nPeso              [ 28,0 kg ]\n\nRececionista / Veterinário: [ GUARDAR DADOS ADMINISTRATIVOS ]\nAo criar a partir de W07, os mesmos campos aparecem vazios.\n\nVeterinário: [Registo clínico]   [Vacinação]\n\nAuxiliar Veterinário: modo de leitura restrita; sem edição" as SCREEN

note bottom of SCREEN
Atores: Rececionista, Veterinário
Auxiliar: leitura restrita
UC06 / UC07 / UC10
RF014, RF015, RF016, RF026
end note
@enduml
```

### 4.10. W09 — Agenda da Clínica

Fonte: `W09-agenda.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W09 — Agenda da Clínica

rectangle "CLÍNICA VETERINÁRIA   |   AGENDA\n\nRececionista: Data [ 09/10/2026 ]   Veterinário [ Todos v ]   [ + Nova marcação ]\nVeterinário: consulta apenas a própria agenda / consultas atribuídas\n\n09:00 | Dr. Silva | Kiko | Consulta geral | Confirmada\n10:00 | Dra. Costa | Lua  | Vacinação       | Pendente\n11:30 | Dr. Silva | Max  | Consulta         | Pendente\n\n[ < Dia anterior ]      [ Hoje ]      [ Dia seguinte > ]\n\nRececionista: [ Consultar disponibilidade ]" as SCREEN

note bottom of SCREEN
Atores: Rececionista, Veterinário
UC09
RF020, RF024
end note
@enduml
```

### 4.11. W10 — Criar / Editar Marcação

Fonte: `W10-marcacao-form.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W10 — Criar / Editar Marcação

rectangle "CLÍNICA VETERINÁRIA   |   AGENDA > MARCAÇÃO\n\nCliente           [________________________]\nPaciente          [________________________]\nVeterinário       [________________________ v]\nData              [__/__/____]\nHora              [__:__]\nMotivo            [________________________]\nEstado            Pendente (estado inicial)\n\nDisponibilidade do veterinário: [ CONSULTAR ]\n\n[ CANCELAR ]                         [ GUARDAR ]" as SCREEN

note bottom of SCREEN
Ator principal: Rececionista
UC09
RF020, RF021, RF022, RF023
end note
@enduml
```

### 4.12. W11 — Detalhe da Marcação

Fonte: `W11-detalhe-marcacao.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W11 — Detalhe da Marcação

rectangle "CLÍNICA VETERINÁRIA   |   AGENDA > DETALHE DA MARCAÇÃO\n\nPaciente: Kiko\nCliente: Carlos Sousa\nVeterinário: Dr. Silva\nData/Hora: 09/10/2026 09:00\nMotivo: Consulta geral\nEstado: Confirmada\n\nRececionista:\n[ REGISTAR CHEGADA ]   [ ALTERAR ESTADO v ]   [ EDITAR MARCAÇÃO ]\n\nVeterinário: consulta da marcação e [ Abrir ficha do paciente ]" as SCREEN

note bottom of SCREEN
Atores: Rececionista, Veterinário
UC09
RF023, RF024, RF025
end note
@enduml
```

### 4.13. W12 — Preparação e Apoio à Consulta

Fonte: `W12-preparacao-consulta.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W12 — Preparação e Apoio à Consulta

rectangle "CLÍNICA VETERINÁRIA   |   APOIO À CONSULTA

CONSULTAS DISPONÍVEIS
Data: [ 09/10/2026 ]   Estado: [ Todos v ]   Pesquisar: [____________]

09:00 | Kiko | Dr. Silva | Consulta geral | Confirmada
10:00 | Lua  | Dra. Costa | Vacinação      | Confirmada

[ SELECIONAR CONSULTA ]

------------------------------------------------------------

CONSULTA SELECIONADA
Paciente: Kiko      Consulta: 09/10/2026 09:00
Veterinário: Dr. Silva

INFORMAÇÃO DE PREPARAÇÃO
- Motivo da consulta
- Dados administrativos relevantes
- Informação clínica autorizada para preparação

APOIO NÃO CLÍNICO
Notas de apoio: [____________________________________]
                [____________________________________]

[ GUARDAR INFORMAÇÃO DE APOIO ]" as SCREEN

note bottom of SCREEN
Ator: Auxiliar Veterinário
UC10
RF026, RF027
W12 é o ponto de entrada autónomo: listar -> selecionar -> preparar/apoiar
end note
@enduml
```

### 4.14. W13 — Registo e Histórico Clínico

Fonte: `W13-registo-clinico.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W13 — Registo e Histórico Clínico

rectangle "CLÍNICA VETERINÁRIA   |   PACIENTE > REGISTO CLÍNICO\n\nPaciente: Kiko\n\nNOVA CONSULTA / REGISTO\nNotas clínicas      [____________________________________]\nDiagnóstico         [____________________________________]\nTratamento          [____________________________________]\nPrescrição          [____________________________________]\n\n[ GUARDAR REGISTO CLÍNICO ]\n\nHISTÓRICO CLÍNICO\n09/10/2026 | Consulta geral | [Ver detalhe]\n15/06/2026 | Vacinação      | [Ver detalhe]\n\nAuxiliar: apenas consulta de informação autorizada" as SCREEN

note bottom of SCREEN
Veterinário: escrita e consulta
Auxiliar: leitura clínica autorizada
UC12 / UC13
RF030, RF031, RF032, RF033, RF034, RF035, RF036
end note
@enduml
```

### 4.15. W14 — Contas, Custos e Valores em Dívida

**Fora do âmbito (Declaração de Âmbito v05).** O ecrã W14 pertencia a F005 — UC14 (RF037 e RF038), retirado do âmbito. O código PlantUML não é incorporado nesta versão. Mantém-se, como histórico, no ficheiro `W14-contas-dividas.puml` entregue na m2.

### 4.16. W15 — Detalhe da Conta e Pagamentos

**Fora do âmbito (Declaração de Âmbito v05).** O ecrã W15 pertencia a F005 — UC14 (RF039 a RF041), retirado do âmbito. O código PlantUML não é incorporado nesta versão. Mantém-se, como histórico, no ficheiro `W15-conta-pagamentos.puml` entregue na m2.

### 4.17. W16 — Vacinação e Desparasitação

Fonte: `W16-vacinacao.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title W16 — Vacinação e Desparasitação

rectangle "CLÍNICA VETERINÁRIA   |   PACIENTE > BOLETIM\n\nPaciente: Kiko\n\n[ + Registar vacinação ]   [ + Registar desparasitação ]\n\nTipo          Produto       Data         Próxima administração\n---------------------------------------------------------------\nVacina        V8            15/06/2026   15/06/2027\nDesparas.     Produto X     01/09/2026   01/12/2026\n\nNovo registo:\nTipo [ Vacinação v ]  Produto [____________]\nData [__/__/____]      Intervalo [________]\n\n[ CALCULAR PRÓXIMA ]   [ GUARDAR ]" as SCREEN

note bottom of SCREEN
Ator: Veterinário
UC16
RF043, RF044, RF045, RF046
end note
@enduml
```

### 4.18. W17 — Painel de Internamentos

**Fora do âmbito (Declaração de Âmbito v05).** O ecrã W17 pertencia a F007 — UC18 (RF048 a RF050), retirado do âmbito. O código PlantUML não é incorporado nesta versão. Mantém-se, como histórico, no ficheiro `W17-internamentos.puml` entregue na m2.

### 4.19. W18 — Detalhe do Internamento

**Fora do âmbito (Declaração de Âmbito v05).** O ecrã W18 pertencia a F007 — UC18 (RF048 a RF052), retirado do âmbito. O código PlantUML não é incorporado nesta versão. Mantém-se, como histórico, no ficheiro `W18-detalhe-internamento.puml` entregue na m2.

### 4.20. M01 — Login Mobile

Fonte: `M01-login-mobile.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title M01 — Login Mobile

rectangle "┌──────────────────────────┐\n│     CLÍNICA VETERINÁRIA    │\n│                            │\n│       INICIAR SESSÃO       │\n│                            │\n│ Email                      │\n│ [______________________]   │\n│                            │\n│ Palavra-passe              │\n│ [______________________]   │\n│                            │\n│        [ ENTRAR ]          │\n│                            │\n│ Mensagem de erro           │\n└──────────────────────────┘" as SCREEN

note bottom of SCREEN
Ator: Cliente
UC01
RF001, RF002, RF004, RF005
end note
@enduml
```

### 4.21. M02 — Os Meus Animais

Fonte: `M02-meus-animais.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title M02 — Os Meus Animais

rectangle "┌──────────────────────────┐\n│ OS MEUS ANIMAIS      [Sair] │\n├──────────────────────────┤\n│ Kiko                       │\n│ Cão • Labrador             │\n│ [ Ver detalhe ]            │\n├──────────────────────────┤\n│ Lua                        │\n│ Gato • Europeu             │\n│ [ Ver detalhe ]            │\n├──────────────────────────┤\n│ Animais | Consultas | €    │\n└──────────────────────────┘" as SCREEN

note bottom of SCREEN
Ator: Cliente
UC08
RF018
Consulta apenas — sem edição
end note
@enduml
```

### 4.22. M03 — Detalhe do Animal

Fonte: `M03-detalhe-animal.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title M03 — Detalhe do Animal

rectangle "┌──────────────────────────┐\n│ KIKO               [Sair]  │\n├──────────────────────────┤\n│ Espécie: Cão              │\n│ Raça: Labrador            │\n│ Sexo: Macho           │\n│ Nascimento: 12/03/2021    │\n│                           │\n│ [ Ver boletim ]           │\n│ [ Ver consultas ]         │\n│                           │\n├──────────────────────────┤\n│ Animais | Consultas | €   │\n└──────────────────────────┘" as SCREEN

note bottom of SCREEN
Ator: Cliente
UC08
RF018
Consulta apenas — sem edição
end note
@enduml
```

### 4.23. M04 — Consultas

Fonte: `M04-consultas.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title M04 — Consultas

rectangle "┌──────────────────────────┐\n│ CONSULTAS          [Sair]   │\n├──────────────────────────┤\n│ PRÓXIMAS                   │\n│ 15/10 • 10:30 • Kiko      │\n│ Dra. Costa • Confirmada    │\n├──────────────────────────┤\n│ HISTÓRICO                  │\n│ 15/06 • Kiko              │\n│ Dr. Silva • Realizada      │\n│ 02/03 • Lua               │\n│ Dra. Costa • Realizada     │\n├──────────────────────────┤\n│ Animais | Consultas | €   │\n└──────────────────────────┘" as SCREEN

note bottom of SCREEN
Ator: Cliente
UC11
RF028, RF029
Sem criar, alterar ou cancelar marcações
end note
@enduml
```

### 4.24. M05 — Despesas e Pagamentos

**Fora do âmbito (Declaração de Âmbito v05).** O ecrã M05 pertencia a F005 — UC15 (RF042), retirado do âmbito. O código PlantUML não é incorporado nesta versão. Mantém-se, como histórico, no ficheiro `M05-despesas.puml` entregue na m2.

### 4.25. M06 — Boletim de Vacinação e Desparasitação

Fonte: `M06-boletim.puml`

```plantuml {kroki=true}
@startuml
skinparam shadowing false
skinparam roundcorner 8
skinparam defaultTextAlignment center
skinparam rectangle {
  BackgroundColor white
  BorderColor black
  FontColor black
}

title M06 — Boletim de Vacinação e Desparasitação

rectangle "┌──────────────────────────┐\n│ BOLETIM — KIKO     [Sair]  │\n├──────────────────────────┤\n│ VACINAÇÕES                 │\n│ V8                         │\n│ 15/06/2026                 │\n│ Próxima: 15/06/2027        │\n├──────────────────────────┤\n│ DESPARASITAÇÃO             │\n│ Produto X                  │\n│ 01/09/2026                 │\n│ Próxima: 01/12/2026        │\n├──────────────────────────┤\n│        [ Voltar ]          │\n└──────────────────────────┘" as SCREEN

note bottom of SCREEN
Ator: Cliente
UC17
RF047
Consulta apenas
end note
@enduml
```

# Testes de sistema do frontend web

Automação com Playwright dos cenários da Especificação dos Testes de Sistema do Frontend Web (I128), nas partes atribuídas à I129 (autenticação, funcionários, Clientes, dados e horário da clínica e pacientes: 30 cenários), à I130 (disponibilidade, marcações, agenda, chegada, preparação e apoio, registo e histórico clínico: 22 cenários) e à I132 (sessão, permissões por perfil, falhas de rede e erros: 17 cenários).

## Antes de executar

1. **Backend** a correr em `http://localhost:5159`, ligado à base de teste isolada `ClinicaVeterinaria_E2E`, reposta e com os dados de demonstração (I061) carregados (secção seguinte). Os cenários contam com esses dados (por exemplo, os animais do João Costa e o horário da clínica).
2. **Palavras-passe** das contas de demonstração em variáveis de ambiente (nunca no repositório):

   ```powershell
   $env:E2E_SENHA_ADMIN = "..."
   $env:E2E_SENHA_RECECAO = "..."
   $env:E2E_SENHA_RECECAO_BLOQUEADA = "..."
   $env:E2E_SENHA_VET_ANA = "..."
   $env:E2E_SENHA_VET_BRUNO = "..."      # I130: segundo Veterinário
   $env:E2E_SENHA_AUX_CARLA = "..."
   $env:E2E_SENHA_AUX_DIOGO = "..."      # I130: Auxiliar sem autorização de apoio
   $env:E2E_SENHA_CLIENTE_JOAO = "..."   # I132: ST-WEB-TRV-04
   ```

3. **Chromium** do Playwright, na primeira vez: `npx playwright install chromium`.

Opcional: `E2E_WEB_URL` (por omissão `http://localhost:5173`) e `E2E_API_URL` (por omissão `http://localhost:5159`). Se o frontend não estiver a correr, o `npm run test:sistema` arranca o `npm run dev` uma só vez para as duas fases (com `VITE_API_URL` igual a `E2E_API_URL`) e para-o no fim.

## Base de teste isolada

Os testes criam e alteram dados, por isso correm numa base própria, `ClinicaVeterinaria_E2E`, e nunca na base de desenvolvimento configurada nos _user-secrets_ do backend. Os comandos abaixo usam o SQL Server local com autenticação do Windows, sem palavra-passe; noutra instalação muda só o `Server=`. Com login SQL, a palavra-passe nunca vai para o repositório nem para a linha de comandos: o `executar-dados.ps1` recebe `-Utilizador` e lê a palavra-passe da variável `SQLCMDPASSWORD`.

1. **Criar ou repor a base e aplicar as migrações** (backend parado), na pasta `src/backend`:

   ```powershell
   $e2e = "Server=.;Database=ClinicaVeterinaria_E2E;Trusted_Connection=True;TrustServerCertificate=True"
   .\scripts\reposicao-base-dados.ps1 -ConnectionString $e2e
   ```

2. **Carregar os dados de demonstração** (I061), na raiz do repositório:

   ```powershell
   .\04-artefactos-tecnicos\04.13-scripts-de-base-de-dados\m3-v02\dados-demonstracao\executar-dados.ps1 -Servidor "." -ConfiarCertificado -BaseDados "ClinicaVeterinaria_E2E" -PastaRegistos $env:TEMP
   ```

   O registo `dados-execucao-*.log` tem de terminar com todas as verificações aprovadas.

3. **Arrancar o backend ligado a essa base**, na pasta `src/backend` e no mesmo terminal do passo 1 (a variável `ConnectionStrings__ClinicaDb` sobrepõe-se aos _user-secrets_ só nesse terminal):

   ```powershell
   $env:ConnectionStrings__ClinicaDb = $e2e
   dotnet run --project .\ClinicaVeterinaria.Api\ClinicaVeterinaria.Api.csproj --urls http://localhost:5159
   ```

Basta repor a base uma vez antes de uma série de execuções: a suite pode correr várias vezes seguidas sem nova reposição, porque cada execução cria os seus dados com um sufixo próprio. Para voltar ao estado inicial, parar o backend e repetir os passos 1 a 3.

## Executar

```powershell
npm run test:sistema
```

Corre em duas fases: primeiro os cenários que podem correr em paralelo (projeto `sistema`) e depois, sozinhos, os que alteram a configuração ou o horário da clínica (projeto `sistema-exclusivo`, ficheiros `*.exclusivo.spec.ts`), que repõem sempre os valores originais (I128, secção 5.5). Um só cenário: `npm run test:sistema -- --grep F001-07`. Só os cenários da I132: `npm run test:sistema -- --grep "TRV-|F001-04|F001-05|F002-10|F004-08|F006-05"`. Só os da I130: `npm run test:sistema -- --grep "F003-(0[3-9]|1[0-7])|F004-0[1-7]"`.

Quando um teste falha, a captura de ecrã e o _trace_ ficam em `test-results/` e o relatório em `playwright-report/sistema` e `playwright-report/sistema-exclusivo` (`npx playwright show-report playwright-report/sistema`).

## Regras seguidas (I128, secção 5)

- A API nunca é simulada; o navegador corre sem interface.
- Cada teste inicia a sua sessão e cria os dados de que precisa, com o `{sufixo}` do cenário e da execução; nenhum depende de outro nem da ordem.
- Não há pausas fixas: cada passo espera por uma condição visível ou pela resposta da API.
- Os dados criados ficam na base de teste até à reposição seguinte; não impedem execuções seguidas.
- As marcações da I130 usam o dia `{D}` de cada cenário (secção 5.4): o N.º dia útil a seguir a hoje, com o desvio N do cenário (`diaDeMarcacao` em `apoio/dados.ts`). Ver a nota no fim deste ficheiro.

## Estrutura

| Pasta / ficheiro             | Conteúdo                                                                                                      |
| ---------------------------- | ------------------------------------------------------------------------------------------------------------- |
| `apoio/contas.ts`            | Contas de demonstração e leitura das palavras-passe                                                           |
| `apoio/sessao.ts`            | PC-01 e PC-02 e o ajudante `entrarComo` por perfil                                                            |
| `apoio/procedimentos.ts`     | PC-03 (Cliente de teste), PC-04 (paciente de teste), PC-05 (marcação de teste) e PC-06 (confirmar a marcação) |
| `apoio/dados.ts`             | `{sufixo}`, e-mails, telefones, NIF, palavras-passe gerados e `{D}`                                           |
| `apoio/pedidos.ts`           | Registo dos pedidos à API (para confirmar validações sem pedido)                                              |
| `apoio/verificarAmbiente.ts` | Verificação inicial das variáveis e do backend                                                                |
| `apoio/navegador.ts`         | Leitura e troca da sessão guardada no navegador (I132)                                                        |
| `apoio/rede.ts`              | Cortar, repor e atrasar a ligação ao backend (I132)                                                           |
| `apoio/respostas.ts`         | Registo das respostas do backend e id do Bobi (I132)                                                          |
| `paginas/`                   | Um objeto de página por ecrã (W01 a W13) e o seletor de proprietário                                          |
| `executar.mjs`               | Execução em duas fases                                                                                        |

## Cenário ↔ teste

| Cenário (I128) | Título                                                                         | Ficheiro                       |
| -------------- | ------------------------------------------------------------------------------ | ------------------------------ |
| ST-WEB-F001-01 | Iniciar sessão com cada perfil interno                                         | `f001-sessao.spec.ts`          |
| ST-WEB-F001-02 | Recusar credenciais inválidas                                                  | `f001-sessao.spec.ts`          |
| ST-WEB-F001-03 | Recusar o início de sessão de uma conta bloqueada                              | `f001-sessao.spec.ts`          |
| ST-WEB-F001-06 | Listar e pesquisar funcionários                                                | `f001-funcionarios.spec.ts`    |
| ST-WEB-F001-07 | Criar uma conta de funcionário e atribuir-lhe um perfil                        | `f001-funcionarios.spec.ts`    |
| ST-WEB-F001-08 | Recusar a criação de funcionário com dados inválidos                           | `f001-funcionarios.spec.ts`    |
| ST-WEB-F001-09 | Atualizar uma conta de funcionário e a autorização de apoio                    | `f001-funcionarios.spec.ts`    |
| ST-WEB-F001-10 | Mostrar a autorização de apoio só ao Auxiliar Veterinário                      | `f001-funcionarios.spec.ts`    |
| ST-WEB-F001-11 | Bloquear uma conta de funcionário                                              | `f001-funcionarios.spec.ts`    |
| ST-WEB-F001-12 | Recusar o bloqueio da própria conta do Administrador                           | `f001-funcionarios.spec.ts`    |
| ST-WEB-F001-13 | Registar um Cliente com conta de acesso (Rececionista)                         | `f001-clientes.spec.ts`        |
| ST-WEB-F001-14 | Registar um Cliente com NIF e morada (Administrador)                           | `f001-clientes.spec.ts`        |
| ST-WEB-F001-15 | Recusar o registo de um Cliente com dados inválidos                            | `f001-clientes.spec.ts`        |
| ST-WEB-F001-16 | Atualizar os dados de um Cliente                                               | `f001-clientes.spec.ts`        |
| ST-WEB-F001-17 | Recusar a atualização de um Cliente com dados inválidos                        | `f001-clientes.spec.ts`        |
| ST-WEB-F001-18 | Pesquisar Clientes                                                             | `f001-clientes.spec.ts`        |
| ST-WEB-F001-19 | Consultar os dados gerais e as configurações da clínica                        | `clinica.exclusivo.spec.ts`    |
| ST-WEB-F001-20 | Atualizar os dados gerais e as configurações da clínica (exclusivo)            | `clinica.exclusivo.spec.ts`    |
| ST-WEB-F001-21 | Recusar dados gerais inválidos da clínica (exclusivo)                          | `clinica.exclusivo.spec.ts`    |
| ST-WEB-F002-01 | Registar um paciente associado a um Cliente (Rececionista)                     | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-02 | Registar um paciente a partir da pesquisa por proprietário (Veterinário)       | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-03 | Recusar o registo de um paciente com dados inválidos                           | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-04 | Consultar a ficha administrativa do paciente                                   | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-05 | Abrir a ficha de um paciente inexistente                                       | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-06 | Atualizar os dados administrativos do paciente                                 | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-07 | Recusar a atualização do paciente com dados inválidos                          | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-08 | Pesquisar os pacientes de um proprietário                                      | `f002-pacientes.spec.ts`       |
| ST-WEB-F002-09 | Pesquisa por proprietário sem resultados                                       | `f002-pacientes.spec.ts`       |
| ST-WEB-F003-01 | Gerir o horário de funcionamento (exclusivo)                                   | `clinica.exclusivo.spec.ts`    |
| ST-WEB-F003-02 | Recusar períodos sobrepostos ou com fecho não posterior à abertura (exclusivo) | `clinica.exclusivo.spec.ts`    |
| ST-WEB-F003-03 | Consultar a disponibilidade de um Veterinário                                  | `f003-agenda.spec.ts`          |
| ST-WEB-F003-04 | A disponibilidade exclui as marcações não canceladas                           | `f003-agenda.spec.ts`          |
| ST-WEB-F003-05 | Criar uma marcação                                                             | `f003-agenda.spec.ts`          |
| ST-WEB-F003-06 | Recusar uma marcação incompleta ou numa vaga ocupada                           | `f003-agenda.spec.ts`          |
| ST-WEB-F003-07 | Alterar uma marcação                                                           | `f003-agenda.spec.ts`          |
| ST-WEB-F003-08 | Recusar a alteração concorrente ou de uma marcação final                       | `f003-agenda.spec.ts`          |
| ST-WEB-F003-09 | Atualizar o estado da marcação segundo as transições permitidas                | `f003-agenda.spec.ts`          |
| ST-WEB-F003-10 | Não oferecer transições de estado não previstas                                | `f003-agenda.spec.ts`          |
| ST-WEB-F003-11 | Consultar a agenda da clínica (Rececionista)                                   | `f003-agenda.spec.ts`          |
| ST-WEB-F003-12 | O Veterinário vê só as consultas que lhe estão atribuídas                      | `f003-agenda.spec.ts`          |
| ST-WEB-F003-13 | Registar a chegada à consulta                                                  | `f003-agenda.spec.ts`          |
| ST-WEB-F003-14 | Não registar a chegada fora de uma marcação Confirmada nem duas vezes          | `f003-agenda.spec.ts`          |
| ST-WEB-F003-15 | Consultar a informação de preparação da consulta                               | `f003-preparacao.spec.ts`      |
| ST-WEB-F003-16 | Registar informação não clínica de apoio com autorização                       | `f003-preparacao.spec.ts`      |
| ST-WEB-F003-17 | Recusar a nota de apoio sem autorização ou numa marcação cancelada             | `f003-preparacao.spec.ts`      |
| ST-WEB-F004-01 | Registar notas clínicas, diagnóstico, tratamento e prescrição numa consulta    | `f004-registo-clinico.spec.ts` |
| ST-WEB-F004-02 | Recusar registos clínicos inválidos                                            | `f004-registo-clinico.spec.ts` |
| ST-WEB-F004-03 | Atualizar um registo clínico                                                   | `f004-registo-clinico.spec.ts` |
| ST-WEB-F004-04 | Não permitir alterar o registo clínico de outro Veterinário                    | `f004-registo-clinico.spec.ts` |
| ST-WEB-F004-05 | Consultar o histórico clínico do paciente                                      | `f004-registo-clinico.spec.ts` |
| ST-WEB-F004-06 | Recusar um intervalo de datas inválido no histórico                            | `f004-registo-clinico.spec.ts` |
| ST-WEB-F004-07 | O Auxiliar Veterinário consulta só a informação clínica autorizada             | `f004-registo-clinico.spec.ts` |
| ST-WEB-F001-04 | Terminar sessão e não voltar a páginas protegidas                              | `trv-sessao.spec.ts`           |
| ST-WEB-F001-05 | Manter a sessão ao navegar e ao recarregar                                     | `trv-sessao.spec.ts`           |
| ST-WEB-F002-10 | Ficha do paciente só em leitura para o Auxiliar Veterinário                    | `trv-perfis.spec.ts`           |
| ST-WEB-F004-08 | Recusar o acesso ao registo clínico a quem não o pode ver                      | `trv-perfis.spec.ts`           |
| ST-WEB-F006-05 | Recusar o boletim web a quem não é Veterinário                                 | `trv-perfis.spec.ts`           |
| ST-WEB-TRV-01  | Menu de cada perfil sem entradas de outros perfis                              | `trv-perfis.spec.ts`           |
| ST-WEB-TRV-02  | Recusar o acesso por endereço direto a rotas de outro perfil                   | `trv-perfis.spec.ts`           |
| ST-WEB-TRV-03  | Rotas protegidas sem sessão levam ao início de sessão                          | `trv-perfis.spec.ts`           |
| ST-WEB-TRV-04  | O Cliente não usa a aplicação web                                              | `trv-perfis.spec.ts`           |
| ST-WEB-TRV-05  | Sessão revogada durante a utilização                                           | `trv-sessao.spec.ts`           |
| ST-WEB-TRV-06  | Sessão inválida ao recarregar                                                  | `trv-sessao.spec.ts`           |
| ST-WEB-TRV-07  | Backend indisponível ao abrir uma página                                       | `trv-rede-erros.spec.ts`       |
| ST-WEB-TRV-08  | Backend indisponível ao guardar um formulário                                  | `trv-rede-erros.spec.ts`       |
| ST-WEB-TRV-09  | Mostrar os erros da API sem detalhes técnicos                                  | `trv-rede-erros.spec.ts`       |
| ST-WEB-TRV-10  | Estados de carregamento e botões inativos durante os pedidos                   | `trv-rede-erros.spec.ts`       |
| ST-WEB-TRV-11  | Palavras-passe e tokens não ficam expostos                                     | `trv-sessao.spec.ts`           |
| ST-WEB-TRV-12  | Endereço inexistente                                                           | `trv-perfis.spec.ts`           |

## Cenário ↔ requisito não funcional (I132)

| Cenário        | Requisitos funcionais | Requisitos não funcionais  | O que o teste confirma                                                                   |
| -------------- | --------------------- | -------------------------- | ---------------------------------------------------------------------------------------- |
| ST-WEB-F001-04 | RF003, RF004          | —                          | Fim de sessão (EP-002) e páginas protegidas inacessíveis depois, também com «Retroceder» |
| ST-WEB-F001-05 | RF004                 | —                          | Sessão mantida ao navegar e ao recarregar, confirmada no backend (EP-003)                |
| ST-WEB-F002-10 | RF005, RF015          | RNF001                     | Ficha só de leitura para o Auxiliar; criar e listar pacientes recusado                   |
| ST-WEB-F004-08 | RF036, RF005          | RNF001                     | Registo clínico recusado à Rececionista e ao Administrador, sem informação clínica       |
| ST-WEB-F006-05 | RF046, RF005          | RNF001                     | Boletim recusado à Rececionista e ao Auxiliar, sem a ligação «Vacinação»                 |
| ST-WEB-TRV-01  | RF005                 | RNF001                     | Menu de cada perfil só com as suas entradas                                              |
| ST-WEB-TRV-02  | RF005, RF012, RF026   | RNF001                     | Rotas de outro perfil por endereço direto: «Acesso negado», sem pedir os dados da página |
| ST-WEB-TRV-03  | RF004                 | RNF001                     | Sem sessão, as rotas protegidas levam a `/login`                                         |
| ST-WEB-TRV-04  | RF005                 | RNF003                     | O Cliente é recusado na aplicação web e não fica sessão guardada                         |
| ST-WEB-TRV-05  | RF004, RF003          | —                          | Token revogado: 401, volta a `/login` com aviso e a sessão guardada é apagada            |
| ST-WEB-TRV-06  | RF004                 | —                          | Token inválido ao recarregar: a confirmação falha e a sessão guardada é apagada          |
| ST-WEB-TRV-07  | —                     | RNF006                     | Sem ligação: mensagem compreensível, sessão mantida e «Tentar novamente» recupera        |
| ST-WEB-TRV-08  | —                     | RNF006, RNF005             | Sem ligação ao guardar: os dados preenchidos mantêm-se e o registo funciona depois       |
| ST-WEB-TRV-09  | —                     | RNF004                     | Erros 404 da API sem detalhes técnicos e com ligação de regresso à lista                 |
| ST-WEB-TRV-10  | —                     | — (qualidade de interface) | «A carregar…» e «A guardar…» com os botões inativos até à resposta                       |
| ST-WEB-TRV-11  | —                     | RNF001                     | Palavras-passe ocultas; token e palavras-passe fora dos endereços e da consola           |
| ST-WEB-TRV-12  | RF005                 | —                          | Endereço inexistente: «Página não encontrada» e regresso à página inicial do perfil      |

Os cenários de rede (TRV-07, -08 e -10) controlam a ligação no navegador com `page.route`: os pedidos ou falham como se o backend estivesse desligado, ou seguem para o backend real, só mais tarde. A API nunca é simulada. A verificação do Revisor «desligar a API durante um cenário» faz-se à mão, parando o backend durante a execução.

## Cenário ↔ requisito (I130)

| Cenário        | Requisitos                  | O que o teste confirma                                                                                                         |
| -------------- | --------------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| ST-WEB-F003-03 | RF020                       | Horários livres só nos períodos (09:00–12:30 e 14:00–18:30), de 30 em 30 minutos; o domingo sem horários                       |
| ST-WEB-F003-04 | RF020, RF023                | A vaga das 10:00 sai da disponibilidade enquanto a marcação existe e volta quando é cancelada                                  |
| ST-WEB-F003-05 | RF021, RF024, RNF005        | Marcação criada Pendente, com os dados no detalhe e na agenda de `{D}`                                                         |
| ST-WEB-F003-06 | RF021, RNF004               | Validação local sem pedido; **vaga entretanto ocupada** noutra janela: 422, disponibilidade atualizada, uma só marcação        |
| ST-WEB-F003-07 | RF022, RNF005               | «(horário atual)», alteração do motivo e do horário, mantidas depois de recarregar, com o estado Pendente                      |
| ST-WEB-F003-08 | RF022, RF023                | Alteração concorrente: 412 e dados da outra janela; marcação Cancelada sem edição nem ações                                    |
| ST-WEB-F003-09 | RF023, RNF005               | Pendente → Confirmada (com observação) → Realizada; estado final sem ações                                                     |
| ST-WEB-F003-10 | RF023                       | Pendente não oferece «Marcar como realizada»; cancelamento com motivo, sem ações nem chegada                                   |
| ST-WEB-F003-11 | RF024                       | Agenda do dia com as colunas, filtros «Veterinário» e «Estado», lista vazia e navegação entre dias com os filtros              |
| ST-WEB-F003-12 | RF024, RNF001               | O Veterinário só vê as suas consultas, sem filtro de Veterinário, «Nova marcação» nem «Ações»; `/agenda/nova` recusado         |
| ST-WEB-F003-13 | RF025, RNF005               | Chegada à hora atual de Lisboa, sem mudar o estado; visível no detalhe e na agenda                                             |
| ST-WEB-F003-14 | RF025                       | Sem chegada numa Pendente; segunda chegada recusada (412) e a da outra janela mantida                                          |
| ST-WEB-F003-15 | RF026                       | Informação de preparação completa, sem informação clínica nem ações de agenda                                                  |
| ST-WEB-F003-16 | RF027, RNF005               | Nota vazia recusada sem pedido; nota guardada com autora e data, visível à Rececionista; estado inalterado                     |
| ST-WEB-F003-17 | RF027, RNF001               | Sem autorização: 403 e o formulário deixa de ser oferecido; marcação Cancelada: 422 sem perder o texto                         |
| ST-WEB-F004-01 | RF030–RF033, RNF005         | Registo com os quatro elementos, autoria e data; mantido depois de recarregar e no histórico; estado Confirmada                |
| ST-WEB-F004-02 | RF030–RF033, RNF004         | Registo vazio e prescrição sem dosagem recusados sem pedido; um só registo por consulta; consulta Pendente sem registo         |
| ST-WEB-F004-03 | RF034, RF032, RF033, RNF005 | Novo diagnóstico e segundo tratamento, com a última alteração; mesma consulta e paciente; histórico com 2 tratamentos          |
| ST-WEB-F004-04 | RF034, RNF001               | Outro Veterinário lê o registo, mas o W13 da consulta é recusado (403) e nada é gravado; «Otite externa» mantém-se             |
| ST-WEB-F004-05 | RF035                       | Histórico do mais recente para o mais antigo, filtro de outubro e registo de 14/10/2026 completo                               |
| ST-WEB-F004-06 | RF035, RNF004               | Fim anterior ao início: mensagem local, sem pedido, lista mantida e ecrã utilizável                                            |
| ST-WEB-F004-07 | RF036, RNF001               | Auxiliar: histórico e registo só de leitura, sem notas clínicas nem diagnóstico (também nas respostas da API), sem «Vacinação» |

### Notas da I130 sobre a I128

- **`{D}` (secção 5.4, igual na v01 e na v02).** A I128 define `{D}` como «o primeiro dia útil a pelo menos N dias de hoje» e pede que cenários diferentes nunca disputem a mesma vaga do mesmo Veterinário. Perto do fim de semana a primeira regra junta desvios diferentes no mesmo dia (a partir de uma sexta-feira, os desvios 8, 9 e 10 dão todos a mesma segunda-feira) e dois cenários do Dr. Bruno disputariam a mesma vaga. Os testes usam o N.º dia útil a seguir a hoje, que também fica a pelo menos N dias e dá um dia diferente a cada desvio.
- **ST-WEB-F004-07.** O teste segue o resultado esperado da v02 da I128 (`m4-especificacao-testes-sistema-frontend-web-v02.md`, PR #293), que o alinha com a deliberação 5 do Sprint Review do Sprint 07 (`m4-s07-20261122-sprint-review-ata-v01.pdf`), já integrada pela I125: o Auxiliar vê o histórico e o registo de 2026-10-14 em leitura, com o tratamento e a prescrição, e não vê as notas clínicas nem o diagnóstico, nem no ecrã nem nas respostas de EP-033 e EP-034. A v01 ainda indicava «diagnóstico, notas, tratamento».
- **Rótulos de F004 (secção 6).** O registo cria-se a partir da consulta, na Agenda («Abrir registo clínico»); o histórico do paciente indica isso ao Veterinário. Os testes usam os rótulos da implementação.

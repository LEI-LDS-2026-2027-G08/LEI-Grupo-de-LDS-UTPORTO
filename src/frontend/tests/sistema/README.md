# Testes de sistema do frontend web

Automação com Playwright dos cenários da Especificação dos Testes de Sistema do Frontend Web (I128), nas partes atribuídas à I129 (autenticação, funcionários, Clientes, dados e horário da clínica e pacientes: 30 cenários) e à I132 (sessão, permissões por perfil, falhas de rede e erros: 17 cenários).

## Antes de executar

1. **Backend** a correr em `http://localhost:5159`, ligado à base de teste isolada `ClinicaVeterinaria_E2E`, reposta e com os dados de demonstração (I061) carregados (secção seguinte). Os cenários contam com esses dados (por exemplo, os animais do João Costa e o horário da clínica).
2. **Palavras-passe** das contas de demonstração em variáveis de ambiente (nunca no repositório):

   ```powershell
   $env:E2E_SENHA_ADMIN = "..."
   $env:E2E_SENHA_RECECAO = "..."
   $env:E2E_SENHA_RECECAO_BLOQUEADA = "..."
   $env:E2E_SENHA_VET_ANA = "..."
   $env:E2E_SENHA_AUX_CARLA = "..."
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

Corre em duas fases: primeiro os cenários que podem correr em paralelo (projeto `sistema`) e depois, sozinhos, os que alteram a configuração ou o horário da clínica (projeto `sistema-exclusivo`, ficheiros `*.exclusivo.spec.ts`), que repõem sempre os valores originais (I128, secção 5.5). Um só cenário: `npm run test:sistema -- --grep F001-07`. Só os cenários da I132: `npm run test:sistema -- --grep "TRV-|F001-04|F001-05|F002-10|F004-08|F006-05"`.

Quando um teste falha, a captura de ecrã e o _trace_ ficam em `test-results/` e o relatório em `playwright-report/sistema` e `playwright-report/sistema-exclusivo` (`npx playwright show-report playwright-report/sistema`).

## Regras seguidas (I128, secção 5)

- A API nunca é simulada; o navegador corre sem interface.
- Cada teste inicia a sua sessão e cria os dados de que precisa, com o `{sufixo}` do cenário e da execução; nenhum depende de outro nem da ordem.
- Não há pausas fixas: cada passo espera por uma condição visível ou pela resposta da API.
- Os dados criados ficam na base de teste até à reposição seguinte; não impedem execuções seguidas.

## Estrutura

| Pasta / ficheiro             | Conteúdo                                                             |
| ---------------------------- | -------------------------------------------------------------------- |
| `apoio/contas.ts`            | Contas de demonstração e leitura das palavras-passe                  |
| `apoio/sessao.ts`            | PC-01 e PC-02 e o ajudante `entrarComo` por perfil                   |
| `apoio/procedimentos.ts`     | PC-03 (Cliente de teste) e PC-04 (paciente de teste)                 |
| `apoio/dados.ts`             | `{sufixo}`, e-mails, telefones, NIF e palavras-passe gerados         |
| `apoio/pedidos.ts`           | Registo dos pedidos à API (para confirmar validações sem pedido)     |
| `apoio/verificarAmbiente.ts` | Verificação inicial das variáveis e do backend                       |
| `apoio/navegador.ts`         | Leitura e troca da sessão guardada no navegador (I132)               |
| `apoio/rede.ts`              | Cortar, repor e atrasar a ligação ao backend (I132)                  |
| `apoio/respostas.ts`         | Registo das respostas do backend e id do Bobi (I132)                 |
| `paginas/`                   | Um objeto de página por ecrã (W01 a W08) e o seletor de proprietário |
| `executar.mjs`               | Execução em duas fases                                               |

## Cenário ↔ teste

| Cenário (I128) | Título                                                                         | Ficheiro                    |
| -------------- | ------------------------------------------------------------------------------ | --------------------------- |
| ST-WEB-F001-01 | Iniciar sessão com cada perfil interno                                         | `f001-sessao.spec.ts`       |
| ST-WEB-F001-02 | Recusar credenciais inválidas                                                  | `f001-sessao.spec.ts`       |
| ST-WEB-F001-03 | Recusar o início de sessão de uma conta bloqueada                              | `f001-sessao.spec.ts`       |
| ST-WEB-F001-06 | Listar e pesquisar funcionários                                                | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-07 | Criar uma conta de funcionário e atribuir-lhe um perfil                        | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-08 | Recusar a criação de funcionário com dados inválidos                           | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-09 | Atualizar uma conta de funcionário e a autorização de apoio                    | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-10 | Mostrar a autorização de apoio só ao Auxiliar Veterinário                      | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-11 | Bloquear uma conta de funcionário                                              | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-12 | Recusar o bloqueio da própria conta do Administrador                           | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-13 | Registar um Cliente com conta de acesso (Rececionista)                         | `f001-clientes.spec.ts`     |
| ST-WEB-F001-14 | Registar um Cliente com NIF e morada (Administrador)                           | `f001-clientes.spec.ts`     |
| ST-WEB-F001-15 | Recusar o registo de um Cliente com dados inválidos                            | `f001-clientes.spec.ts`     |
| ST-WEB-F001-16 | Atualizar os dados de um Cliente                                               | `f001-clientes.spec.ts`     |
| ST-WEB-F001-17 | Recusar a atualização de um Cliente com dados inválidos                        | `f001-clientes.spec.ts`     |
| ST-WEB-F001-18 | Pesquisar Clientes                                                             | `f001-clientes.spec.ts`     |
| ST-WEB-F001-19 | Consultar os dados gerais e as configurações da clínica                        | `clinica.exclusivo.spec.ts` |
| ST-WEB-F001-20 | Atualizar os dados gerais e as configurações da clínica (exclusivo)            | `clinica.exclusivo.spec.ts` |
| ST-WEB-F001-21 | Recusar dados gerais inválidos da clínica (exclusivo)                          | `clinica.exclusivo.spec.ts` |
| ST-WEB-F002-01 | Registar um paciente associado a um Cliente (Rececionista)                     | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-02 | Registar um paciente a partir da pesquisa por proprietário (Veterinário)       | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-03 | Recusar o registo de um paciente com dados inválidos                           | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-04 | Consultar a ficha administrativa do paciente                                   | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-05 | Abrir a ficha de um paciente inexistente                                       | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-06 | Atualizar os dados administrativos do paciente                                 | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-07 | Recusar a atualização do paciente com dados inválidos                          | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-08 | Pesquisar os pacientes de um proprietário                                      | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-09 | Pesquisa por proprietário sem resultados                                       | `f002-pacientes.spec.ts`    |
| ST-WEB-F003-01 | Gerir o horário de funcionamento (exclusivo)                                   | `clinica.exclusivo.spec.ts` |
| ST-WEB-F003-02 | Recusar períodos sobrepostos ou com fecho não posterior à abertura (exclusivo) | `clinica.exclusivo.spec.ts` |
| ST-WEB-F001-04 | Terminar sessão e não voltar a páginas protegidas                              | `trv-sessao.spec.ts`        |
| ST-WEB-F001-05 | Manter a sessão ao navegar e ao recarregar                                     | `trv-sessao.spec.ts`        |
| ST-WEB-F002-10 | Ficha do paciente só em leitura para o Auxiliar Veterinário                    | `trv-perfis.spec.ts`        |
| ST-WEB-F004-08 | Recusar o acesso ao registo clínico a quem não o pode ver                      | `trv-perfis.spec.ts`        |
| ST-WEB-F006-05 | Recusar o boletim web a quem não é Veterinário                                 | `trv-perfis.spec.ts`        |
| ST-WEB-TRV-01  | Menu de cada perfil sem entradas de outros perfis                              | `trv-perfis.spec.ts`        |
| ST-WEB-TRV-02  | Recusar o acesso por endereço direto a rotas de outro perfil                   | `trv-perfis.spec.ts`        |
| ST-WEB-TRV-03  | Rotas protegidas sem sessão levam ao início de sessão                          | `trv-perfis.spec.ts`        |
| ST-WEB-TRV-04  | O Cliente não usa a aplicação web                                              | `trv-perfis.spec.ts`        |
| ST-WEB-TRV-05  | Sessão revogada durante a utilização                                           | `trv-sessao.spec.ts`        |
| ST-WEB-TRV-06  | Sessão inválida ao recarregar                                                  | `trv-sessao.spec.ts`        |
| ST-WEB-TRV-07  | Backend indisponível ao abrir uma página                                       | `trv-rede-erros.spec.ts`    |
| ST-WEB-TRV-08  | Backend indisponível ao guardar um formulário                                  | `trv-rede-erros.spec.ts`    |
| ST-WEB-TRV-09  | Mostrar os erros da API sem detalhes técnicos                                  | `trv-rede-erros.spec.ts`    |
| ST-WEB-TRV-10  | Estados de carregamento e botões inativos durante os pedidos                   | `trv-rede-erros.spec.ts`    |
| ST-WEB-TRV-11  | Palavras-passe e tokens não ficam expostos                                     | `trv-sessao.spec.ts`        |
| ST-WEB-TRV-12  | Endereço inexistente                                                           | `trv-perfis.spec.ts`        |

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

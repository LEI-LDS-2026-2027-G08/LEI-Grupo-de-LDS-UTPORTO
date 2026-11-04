# Pipeline do backend com entrega no ambiente público de avaliação — m3-v03

| Campo | Valor |
|---|---|
| Projeto | Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027 |
| Issue | I104 — Disponibilizar o backend em ambiente público de avaliação com entrega contínua (GitHub #31) |
| Milestone / Sprint | M3 / Sprint 05 |
| Executor / Revisor / Auditor | Ivan Cristiano Silva Mendes / Gonçalo Magalhães Ribeiro / Gonçalo Dinis Magalhães Teixeira |
| Versão | m3-v03 (`/projeto/04-artefactos-tecnicos/04.16-ci-cd/m3-v03`); a m3-v01 (I102) e a m3-v02 (I103) mantêm-se sem alterações |
| Plataforma de CI/CD | GitHub Actions, runner `ubuntu-latest` (Regulamento Interno do Grupo v02, secção 11.6) |
| Plataforma do ambiente público | Microsoft Azure (subscrição Azure for Students): App Service e Azure SQL Database |
| Endereço público | https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net/swagger |
| Ficheiro em uso | `.github/workflows/backend-ci.yml` |
| Referências | Regulamento de Funcionamento da UC v19, secção 20.2; README da m3-v02 (I103); dados de demonstração da I061 (`04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao`); migrações da I062 |

## 1. Conteúdo desta pasta

A m3-v02 (I103) indica que uma alteração posterior cria uma versão nova e mantém a anterior (Regulamento da UC, secção 18.2). Esta pasta é essa versão: a pipeline da I103 com a etapa 8, de entrega. Os restantes ficheiros são iguais aos da m3-v02.

| Ficheiro nesta pasta | Caminho no repositório | Alteração na m3-v03 |
|---|---|---|
| `workflows/backend-ci.yml` | `.github/workflows/backend-ci.yml` | Etapa 8 «Entrega no ambiente público (I104)» e cabeçalho |
| `actions/preparar-dotnet/action.yml` | `.github/actions/preparar-dotnet/action.yml` | Sem alterações |
| `configuracao/quality-gates.json` | `.github/quality-gates.json` | Sem alterações |
| `configuracao/Directory.Build.props` | `Directory.Build.props` | Sem alterações |
| `scripts/*.py` | `.github/scripts/*.py` | Sem alterações |
| `m3-s05-i104-20261103-carga-dados-avaliacao-v01.log` | — | Evidência: carga dos dados da I061 na base pública (2026-11-03 22:50, 28 verificações, 28 OK) |
| `m3-s05-i104-20261103-carga-dados-avaliacao-repeticao-v01.log` | — | Evidência: segunda execução sobre a mesma base, sem duplicar dados |
| `README.md` | — | Este documento |

Alterações na API (`src/backend/ClinicaVeterinaria.Api`), necessárias para o ambiente público e sem efeito nos outros ambientes:

| Ficheiro | Alteração |
|---|---|
| `Program.cs` | O Swagger fica ativo em Development **ou** com `Swagger:Ativo = true` (critério 1). Com `BaseDados:AplicarMigracoesNoArranque = true`, a API aplica as migrações da I062 ao arrancar; se a base serverless estiver em pausa (erro transitório 40613), tenta de novo de 15 em 15 segundos, até 12 vezes, em vez de terminar (o primeiro arranque no Azure, a 2026-11-03, falhou por esse motivo). |
| `appsettings.json` | `Swagger:Ativo` e `BaseDados:AplicarMigracoesNoArranque` com o valor `false` por omissão. Só as definições do App Service os põem a `true`. |

## 2. Ambiente público de avaliação

| Recurso Azure | Nome | Região | Configuração |
|---|---|---|---|
| Grupo de recursos | `rg-clinica-g08` | — | Agrupa os recursos do ambiente público; apagá-lo remove o ambiente inteiro |
| App Service plan | `ASP-rgclinicag08-afbc` | Spain Central | Linux, **Free F1** (sem custo; até 60 minutos de CPU por dia; a aplicação adormece sem pedidos, e o primeiro pedido demora alguns segundos) |
| App Service | `clinica-g08-api` | Spain Central | .NET 10 (LTS); «HTTPS Only»; endereço `https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net` |
| Servidor Azure SQL | `sql-clinica-g08-us` | West US 2 | Autenticação SQL; acesso só para os serviços do Azure e para o IP de quem faz a reposição (secção 6) |
| Base de dados | `clinica-g08-avaliacao` | West US 2 | Oferta gratuita do Azure SQL Database (General Purpose serverless, pausa automática; ao atingir o limite gratuito, pausa até ao mês seguinte, sem custos). **Só para o ambiente público**: distinta das bases de desenvolvimento e das de defesa individual (critério 3) |

Definições do App Service (*Settings → Environment variables*). São os únicos sítios onde estão a cadeia de ligação e a chave JWT do ambiente público:

| Nome | Tipo | Valor |
|---|---|---|
| `ClinicaDb` | Connection string (SQLAzure) | Cadeia ADO.NET da base `clinica-g08-avaliacao`, com `Connection Timeout=60;ConnectRetryCount=6;ConnectRetryInterval=10` (a base em pausa demora cerca de 1 minuto a acordar e responde com o erro transitório 40613 entretanto; o SqlClient volta a tentar a abertura da ligação até 6 vezes, de 10 em 10 segundos) |
| `Jwt__Key` | App setting | Chave Base64 de 64 bytes, gerada para este ambiente (secção 5) |
| `ASPNETCORE_ENVIRONMENT` | App setting | `Production` |
| `Swagger__Ativo` | App setting | `true` |
| `BaseDados__AplicarMigracoesNoArranque` | App setting | `true` |

Nos nomes, `__` (dois underscores) corresponde ao `:` da configuração do .NET (`Jwt__Key` → `Jwt:Key`).

A base pública tem apenas os dados fictícios da I061: nomes com «(demonstração)» ou «(fictício)», e-mails do domínio reservado `.test`, NIF e telefones fictícios.

## 3. Configuração inicial (uma vez)

**No Azure:**

1. Criar a base de dados Azure SQL com a oferta gratuita (*SQL databases → Create*, «Free offer applied!»), num servidor novo com autenticação SQL. Em *Networking*: *Allow Azure services and resources to access this server* = Yes; *Add current client IPv4 address* = Yes. Comportamento no limite gratuito: *Auto-pause the database until next month*.
2. Criar o App Service (*App Services → Create → Web App*): *Publish* = Code, *Runtime stack* = .NET 10 (LTS), *Operating System* = Linux, plano Free F1, sem Application Insights.
3. Preencher as definições da secção 2. A palavra-passe do administrador SQL entra só na cadeia de ligação, nunca no repositório.
4. Em *Settings → Configuration → General settings*: *HTTPS Only* = On e *SCM Basic Auth Publishing Credentials* = On (necessário para o perfil de publicação).
5. Carregar os dados de demonstração na base pública (secção 6, passos 1 e 3). As migrações são aplicadas pela API no primeiro arranque.
6. Descarregar o perfil de publicação (*Overview → Download publish profile*).

**No GitHub** (*Settings → Environments*, com permissão de administração do repositório):

1. Criar o ambiente `avaliacao` e, em *Deployment branches and tags*, limitar a `main`.
2. Secret do ambiente `AZURE_WEBAPP_PUBLISH_PROFILE`: o conteúdo do ficheiro descarregado no passo 6, que depois se apaga do computador.
3. Variáveis do ambiente: `AZURE_WEBAPP_NAME` = `clinica-g08-api` e `AZURE_WEBAPP_URL` = `https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net`.

## 4. Entrega contínua — etapa 8

| Elemento | Valor |
|---|---|
| Job | `entrega` — «8 · Entrega no ambiente público (I104)» |
| Quando corre | Push para `main` **e** «Resultado da pipeline» com sucesso. Num Pull Request, num push para outro ramo ou com uma etapa falhada, fica *skipped* (critério 2) |
| O que faz | Confirma a configuração do ambiente `avaliacao`; publica a API (`dotnet publish`, Release) compilada nesse commit; entrega-a com `azure/webapps-deploy` v3.0.8 (fixado pelo commit); confirma que `GET /swagger/v1/swagger.json` devolve 200 e `GET /clinica` sem token devolve 401 (até 30 tentativas, 20 s entre elas, por causa do arranque e da pausa da base) |
| Concorrência | Uma entrega de cada vez (`entrega-avaliacao`), sem cancelar a que estiver a decorrer |
| Evidência | Resumo da execução com o commit entregue e o endereço; ligação «avaliacao» na página da execução e em *Environments* |

O fluxo do grupo mantém-se: as tarefas são integradas em `develop` e a promoção de `develop` para `main` (Pull Request com o «Resultado da pipeline» obrigatório) dispara a entrega.

## 5. Segredos e rotação (critério 4)

Não há segredos no repositório. Cada segredo do ambiente público só existe num sítio:

| Segredo | Onde está | Como rodar |
|---|---|---|
| Palavra-passe do administrador SQL | Servidor `sql-clinica-g08-us` e definição `ClinicaDb` do App Service | 1. Servidor SQL → *Reset password*. 2. App Service → `ClinicaDb` → substituir `Password=…`. 3. *Apply* (a aplicação reinicia). 4. Confirmar o login pelo Swagger. |
| Chave JWT (`Jwt__Key`) | Definições do App Service | 1. Gerar uma chave nova: `[Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(64))` (PowerShell). 2. Substituir `Jwt__Key` e aplicar. Os tokens emitidos antes deixam de ser válidos (é preciso iniciar sessão outra vez). |
| Perfil de publicação | Secret `AZURE_WEBAPP_PUBLISH_PROFILE` do ambiente `avaliacao` | 1. App Service → *Overview → Reset publish profile*. 2. Descarregar o novo perfil. 3. Substituir o secret no GitHub. 4. Repetir a entrega (*Re-run* da última execução de `main`) para confirmar. |
| Palavras-passe das contas de avaliação | Hashes na base pública (dados da I061); valores em claro só no ficheiro de credenciais da submissão (`m3-credenciais-avaliacao-v01.md`, I105), fora do repositório | Alterar as palavras-passe pela API (EP-007, Administrador) ou repor a base (secção 6), e atualizar o ficheiro de credenciais. Depois da avaliação, bloquear as contas (EP-008) ou apagar o grupo de recursos. |

Rodar um segredo sempre que houver suspeita de exposição, quando um elemento sai do grupo e depois da avaliação da M3.

## 6. Procedimento de reposição da base de dados pública (critério 5)

Repõe a base pública no estado inicial: esquema das migrações da I062 e dados de demonstração da I061. Feito por um elemento do grupo, no seu computador, com o .NET 10, o `dotnet-ef` e o `sqlcmd`.

1. **Acesso:** no servidor `sql-clinica-g08-us`, *Security → Networking → Add your client IPv4 address* → *Save*.
2. **Esquema:** na pasta `src/backend`, com a cadeia de ligação pública (copiada do App Service, sem a guardar em ficheiros), desfazer todas as migrações e voltar a aplicá-las:

   ```powershell
   $cadeia = Read-Host -MaskInput "Cadeia ClinicaDb do App Service"
   dotnet ef database update 0 --project ClinicaVeterinaria.Infrastructure --startup-project ClinicaVeterinaria.Api --context ClinicaDbContext --connection $cadeia
   dotnet ef database update --project ClinicaVeterinaria.Infrastructure --startup-project ClinicaVeterinaria.Api --context ClinicaDbContext --connection $cadeia
   Remove-Variable cadeia
   ```

   **Não usar** `scripts/reposicao-base-dados.ps1` (I062) neste ambiente: esse script apaga a base de dados (`database drop`), e a base recriada perdia a configuração da oferta gratuita do Azure SQL e podia passar a ser paga.
3. **Dados:** na pasta dos dados de demonstração da I061:

   ```powershell
   $env:SQLCMDPASSWORD = Read-Host -MaskInput "Palavra-passe do administrador SQL"
   .\executar-dados.ps1 -Servidor "sql-clinica-g08-us.database.windows.net" -BaseDados "clinica-g08-avaliacao" -Utilizador "adminclinica" -ConfiarCertificado
   Remove-Item Env:SQLCMDPASSWORD
   ```

   O registo tem de terminar com «28 verificações, 28 OK, 0 falhadas».
4. **Confirmação:** reiniciar o App Service e iniciar sessão pelo Swagger com uma conta de avaliação.
5. **Fecho:** retirar o IP do passo 1 em *Networking*.

## 7. Procedimento de reversão para a versão anterior (critério 5)

A reversão passa pela pipeline, para a versão entregue ter sempre passado os quality gates.

1. **Reverter o código:** em `develop`, `git revert <commit>` do que causou o problema, por Pull Request; depois promover para `main`. A etapa 8 entrega a versão revertida.
2. **Reversão imediata, sem esperar pela pipeline completa:** em *Actions → Backend CI*, abrir a última execução **com sucesso** de `main` anterior à problemática e usar *Re-run all jobs*. A execução recompila esse commit e, se os gates passarem, volta a entregá-lo. Em seguida, fazer o passo 1 para `main` ficar coerente com o que está entregue.
3. **Base de dados:** as migrações só acrescentam. Se a versão anterior não for compatível com o esquema atual, repor a base (secção 6) a partir do commit anterior.
4. **Confirmação:** o resumo da etapa 8 mostra o commit entregue; confirmar o login pelo Swagger.

## 8. Correspondência com os critérios de aceitação da I104

| Critério | Como é cumprido | Evidência |
|---|---|---|
| 1. HTTPS e Swagger com login de uma conta de avaliação | App Service com *HTTPS Only*; Swagger ativo por `Swagger__Ativo`; contas da I061 | Endereço público e capturas (secção 9) |
| 2. Entrega só quando todas as etapas passam no ramo principal | Etapa 8 depende do «Resultado da pipeline» e só corre em push para `main` | Execução da entrega (secção 9) |
| 3. Base pública distinta e só com dados fictícios | Base `clinica-g08-avaliacao` própria; dados da I061 | Secção 2 |
| 4. Segredos só na plataforma e rotação documentada | Definições do App Service e secret do ambiente `avaliacao` | Secção 5 |
| 5. Procedimentos de reposição e de reversão | Secções 6 e 7 | Este README |

## 9. Evidências (Executor)

| Evidência | Estado | Ligação ou ficheiro |
|---|---|---|
| Endereço público | Disponível | https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net/swagger |
| Base pública com os dados fictícios da I061 | Feito (2026-11-03 22:50; 28 verificações, 28 OK) | `m3-s05-i104-20261103-carga-dados-avaliacao-v01.log` e `…-repeticao-v01.log` (nesta pasta) |
| API publicada e a responder em HTTPS | Verificado a 2026-11-03, depois de uma publicação manual de teste (`az webapp deploy`, pacote Release do ramo da I104): `GET /swagger/v1/swagger.json` → 200 (título `ClinicaVeterinaria.Api`, 27 caminhos); `GET /clinica` sem token → 401 | — |
| Captura do Swagger com o login de uma conta de avaliação (`POST /auth/login` → 200, conta Administrador de demonstração) | Feito (2026-11-03 23:33 UTC; e-mail, palavra-passe e token tapados) | `m3-s05-i104-20261103-captura-swagger-login-v01.png` (nesta pasta) |
| Captura de uma consulta com o token (`GET /clinica` → 200, dados fictícios da clínica) | Feito (2026-11-03 23:38 UTC; token tapado) | `m3-s05-i104-20261103-captura-swagger-consulta-v01.png` (nesta pasta) |
| Execução da pipeline que fez a entrega (etapa 8 em `main`) | **Pendente**: só existe depois da promoção de `develop` para `main`, que o grupo ainda não marcou | *Ligação da execução a acrescentar nessa altura* |

A publicação manual de teste não substitui a evidência do critério 2: serve para confirmar a configuração do ambiente público e permitir ao Revisor a verificação 1 antes da primeira entrega pela pipeline.

## 10. Divergências

| Ref. | Divergência | Tratamento |
|---|---|---|
| DV1 | A I104 refere «Swagger (ou equivalente)»; na API o Swagger só existia em Development. | Ativado por configuração (`Swagger:Ativo`) só no ambiente público. |
| DV2 | As migrações não correm na pipeline, porque o Azure SQL só aceita IPs autorizados. | A API aplica-as no arranque, só com `BaseDados:AplicarMigracoesNoArranque = true`. |
| DV3 | A oferta gratuita do Azure SQL não está disponível em Spain Central (erro «ProvisioningDisabled»). | Base de dados em West US 2 e API em Spain Central; os pedidos à base ficam mais lentos, o que é aceitável para avaliação. |

## 11. Histórico de versões

| Versão | Data | Autor | Alteração |
|---|---|---|---|
| m3-v01 | 2026-10-27 | Gonçalo Magalhães Ribeiro | Versão da I102 (pasta `m3-v01`). |
| m3-v02 | 2026-11-03 | André Filipe Moreira Mesquita | I103: quality gates QG-03, QG-04, QG-08 e Collection Postman (pasta `m3-v02`). |
| m3-v03 | 2026-11-03 | Ivan Cristiano Silva Mendes | I104: etapa 8 de entrega no Azure App Service, Swagger e migrações por configuração, segredos, reposição e reversão. |

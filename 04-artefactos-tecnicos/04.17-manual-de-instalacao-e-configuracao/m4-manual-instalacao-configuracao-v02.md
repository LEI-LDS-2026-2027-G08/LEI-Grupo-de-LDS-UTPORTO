# Manual de Instalação e Configuração

## Sistema completo - Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Milestone / Sprint | M4 / S08 |
| Issue | I054 - GitHub #78 |
| Versão / Data | v02 / 27/11/2026 |
| Executor | Ivan Cristiano Silva Mendes |
| Revisor | Gonçalo Dinis Magalhães Teixeira |
| Auditor | Gonçalo Magalhães Ribeiro |
| Estado | Para revisão. Validação integral por outro elemento ainda não executada. |
| Código de referência | `f3feedd26de33694b23fb203d24b5d92c0fad68f` |
| Fonte editável | `m4-manual-instalacao-configuracao-v02.md` |
| Pasta de entrega | `04-artefactos-tecnicos/04.17-manual-de-instalacao-e-configuracao` |

### Histórico

| Versão | Data | Autor | Alteração |
|---|---|---|---|
| v01 | 04/11/2026 | André Filipe Moreira Mesquita | Manual do backend da M3, mantido como histórico. |
| v02 | 27/11/2026 | Ivan Cristiano Silva Mendes | Sistema completo: API, SQL Server, frontend web React e aplicação Android/Kotlin; ambientes local e público; instalação do APK; verificação e diagnóstico. Atualiza a configuração de CORS e separa verificação técnica de validação independente. |

## 1. Objetivo, ambiente e sequência

Este manual permite instalar o sistema num computador de desenvolvimento ou de defesa individual. O procedimento principal usa Windows 10/11 de 64 bits e PowerShell 5.1 ou 7. Cada aluno utiliza a sua própria base de dados, API e configuração dos frontends.

O sistema tem três componentes de aplicação: backend ASP.NET Core, frontend web React/TypeScript e aplicação Android desenvolvida em Kotlin com Jetpack Compose. A API usa SQL Server. A aplicação web destina-se aos perfis internos; a aplicação Android destina-se ao Cliente. Pagamentos e internamentos estão fora do âmbito.

A descrição antiga da Issue #78 refere Flutter. Este manual segue a implementação existente e o Backlog v06 do S08, que indicam Kotlin/Jetpack Compose. Não é necessário instalar Flutter ou Dart.

Execute as secções 2 a 7 pela ordem indicada. A secção 8 verifica o conjunto. A secção 9 explica a ligação à API pública. As secções 10 a 13 cobrem outras plataformas, atualização, diagnóstico e validação.

Use três janelas: **A - preparação**, **B - API** e **C - web**. As janelas B e C ficam abertas durante a utilização. Os comandos PowerShell não devem ser colados numa consola `cmd` ou Bash.

## 2. Ferramentas, versões e acessos

### 2.1. Versões do projeto

| Componente | Versão de referência | Verificação / origem |
|---|---|---|
| .NET SDK | 10.0.100; aceita SDK 10.0 mais recente por `latestFeature` | `dotnet --version`; `src/backend/global.json` |
| EF Core e ferramenta `dotnet-ef` | 10.0.13 | Projetos `.csproj`; `dotnet ef --version` |
| SQL Server | 2025, série 17.x | `SELECT @@VERSION;` |
| Imagem SQL para Docker | `mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04` | Mesma etiqueta dos testes do projeto |
| Node.js | 24.22.3, série 24 LTS | `node --version` |
| npm usado na verificação local | 11.19.1 | `npm --version`; ver nota abaixo |
| React / TypeScript / Vite | 19.3.1 / 6.0.3 / 8.3.4 | `src/frontend/package-lock.json` |
| JDK do Gradle | Eclipse Temurin 21; verificado 21.0.13+9 | `java -version`; `gradle-daemon-jvm.properties` |
| Gradle / Android Gradle Plugin | 9.6.0 / 9.4.1 | Gradle Wrapper e `libs.versions.toml` |
| Kotlin / Compose BOM | 2.2.10 / 2026.02.01 | `src/frontend-mobile/gradle/libs.versions.toml` |
| Android SDK de compilação / destino | API 37.0 / API 37 | `app/build.gradle.kts`; SDK Manager |
| Build-Tools / Platform-Tools de referência | 37.0.0 / 37.0.3 | SDK Manager; `adb version` |
| Android mínimo do dispositivo | API 26 - Android 8.0 | `minSdk` do projeto |
| Android Studio observado | Build `262.10968.63.2622.16975389` | Help → About; pode usar outra edição compatível com o AGP indicado |

**Nota sobre npm:** o Backlog v06 indica npm 12. O Node instalado e o README web usam npm 11; a verificação desta preparação foi feita com 11.19.1. Esta diferença está registada como D01 no registo da I054 e deve ser decidida antes da aprovação. Não se declara npm 12 testado. O procedimento usa `npm ci` e mantém o ficheiro de bloqueio de dependências do projeto.

Instale Git 2.40 ou posterior, .NET SDK, Node.js, JDK, Android Studio e `sqlcmd`. Os números das bibliotecas não são versões a instalar manualmente: o npm e o Gradle resolvem-nos a partir dos ficheiros versionados. A versão exata de cada ferramenta utilizada na validação deve ser registada.

### 2.2. O que deve ter antes de começar

- Acesso de leitura ao repositório do grupo e ligação à Internet para o primeiro restauro de dependências.
- SQL Server 2025 instalado no Windows, ou Docker Desktop com contentores Linux num computador x86-64. No Mac com Apple Silicon, consulte a secção 10 antes de escolher a base de dados.
- Palavras-passe das contas de demonstração obtidas no ficheiro de credenciais da submissão, por canal autorizado. Não estão neste manual nem no Git.
- Android SDK Platform 37.0, Build-Tools 37.0.0, Platform-Tools, Android Emulator e uma imagem de sistema instalados no SDK Manager.
- Um emulador criado no Device Manager ou um telemóvel Android com depuração USB autorizada pelo proprietário.

Transferências oficiais: `.NET` em `dotnet.microsoft.com/download`; Node.js em `nodejs.org`; JDK em `adoptium.net`; Android Studio em `developer.android.com/studio`; SQL Server e `sqlcmd` em `learn.microsoft.com/sql`. Use as versões de referência do projeto, sem atualizar automaticamente o Gradle ou as bibliotecas ao abrir o Android Studio.

## 3. Obter uma cópia identificada do projeto

### Passo 1 - Clonar e selecionar a versão

Na janela A, numa pasta à sua escolha:

```powershell
git clone https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO.git
Set-Location LEI-Grupo-de-LDS-UTPORTO
git checkout f3feedd26de33694b23fb203d24b5d92c0fad68f
$RaizLds = (Get-Location).Path
git rev-parse HEAD
```

Resultado esperado: o commit coincide com o indicado e existem `src/backend`, `src/frontend` e `src/frontend-mobile`. O estado `detached HEAD` é normal nesta seleção de uma versão identificada. Para validar outra entrega aprovada, use o commit dessa entrega e registe-o; não apresente os resultados como sendo desta referência.

### Passo 2 - Confirmar ferramentas e preparar o backend

```powershell
git --version
node --version
npm --version
java -version
Set-Location (Join-Path $RaizLds "src/backend")
dotnet --version
dotnet tool install --global dotnet-ef --version 10.0.13
dotnet ef --version
dotnet restore ClinicaVeterinaria.sln
dotnet build ClinicaVeterinaria.sln --configuration Release --no-restore
```

Se o `dotnet-ef` já existir, confirme a versão. Se for diferente, use `dotnet tool update --global dotnet-ef --version 10.0.13 --allow-downgrade`; esta opção também permite substituir uma versão mais recente. A mudança afeta a ferramenta global do utilizador. Não confunda uma ferramenta já instalada com falha da compilação.

Resultado esperado: ferramenta EF 10.0.13 e `Build succeeded`, sem erros ou avisos. Depois de instalar ferramentas, pode ser necessário abrir uma nova janela para atualizar o PATH. Nessa janela, volte à raiz do clone e defina novamente `$RaizLds`.

## 4. Preparar SQL Server, segredos e dados

### Passo 3 - Definir uma leitura protegida de palavras-passe

Mantenha os passos desta secção na janela A. Defina esta função, compatível com PowerShell 5.1 e 7:

```powershell
function Ler-SegredoLds([string]$Mensagem) {
    $segredoLds = Read-Host $Mensagem -AsSecureString
    $bstrLds = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($segredoLds)
    try {
        [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstrLds)
    } finally {
        [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstrLds)
    }
}
$BaseLds = "ClinicaVeterinaria_M4_Local"
```

Resultado esperado: ao usar a função, o texto introduzido fica oculto no ecrã. Não use `Read-Host -MaskInput` no Windows PowerShell 5.1. Os valores protegidos não devem ser mostrados na consola nem incluídos nas evidências.

### Passo 4 - Escolher uma ligação à base de dados

**Opção A - SQL Server local com autenticação Windows.** Confirme o serviço e defina a ligação:

```powershell
$ServidorLds = "localhost"
sqlcmd -S $ServidorLds -E -C -Q "SELECT @@VERSION;"
$CadeiaLds = "Server=$ServidorLds;Database=$BaseLds;" +
    "Trusted_Connection=True;TrustServerCertificate=True"
```

Resultado esperado: SQL Server 2025, versão 17.x. Numa instância Express com nome, altere apenas `$ServidorLds` para `localhost\SQLEXPRESS`.

**Opção B - SQL Server em Docker, num anfitrião x86-64.** Execute apenas se escolheu esta opção e a porta 1433 estiver livre:

```powershell
$SenhaSqlLds = Ler-SegredoLds "Palavra-passe forte para o sa local"
$env:MSSQL_SA_PASSWORD = $SenhaSqlLds
docker run --name sql-clinica-m4 `
    -e ACCEPT_EULA=Y -e MSSQL_PID=Developer -e MSSQL_SA_PASSWORD `
    -p 127.0.0.1:1433:1433 `
    -v sql-clinica-m4-dados:/var/opt/mssql `
    -d mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
Remove-Item Env:MSSQL_SA_PASSWORD
docker logs sql-clinica-m4
```

Resultado esperado: `SQL Server is now ready for client connections`. Consulte novamente os registos enquanto o arranque não terminar. Não crie um segundo contentor com o mesmo nome ou a mesma porta.

```powershell
$ServidorLds = "localhost,1433"
$env:SQLCMDPASSWORD = $SenhaSqlLds
sqlcmd -S $ServidorLds -U sa -C -Q "SELECT @@VERSION;"
$CadeiaLds = "Server=$ServidorLds;Database=$BaseLds;User Id=sa;" +
    "Password=$SenhaSqlLds;TrustServerCertificate=True;Encrypt=True"
Remove-Item Env:SQLCMDPASSWORD
```

Uma palavra-passe SQL com ponto e vírgula exige construção da cadeia com `SqlConnectionStringBuilder` ou o tratamento adequado dos caracteres especiais. Para este ambiente local de demonstração, escolha uma palavra-passe forte sem `;` e sem aspas, para que o comando acima seja inequívoco.

### Passo 5 - Configurar a API local

Na janela A, em `src/backend`, gere uma chave local e guarde os valores nos user secrets:

```powershell
Set-Location (Join-Path $RaizLds "src/backend")
$ProjetoApiLds = ".\ClinicaVeterinaria.Api\ClinicaVeterinaria.Api.csproj"
$bytesLds = New-Object byte[] 32
$rngLds = [Security.Cryptography.RandomNumberGenerator]::Create()
$rngLds.GetBytes($bytesLds)
$rngLds.Dispose()
$chaveLds = [Convert]::ToBase64String($bytesLds)
dotnet user-secrets set "Jwt:Key" $chaveLds --project $ProjetoApiLds
dotnet user-secrets set "ConnectionStrings:ClinicaDb" $CadeiaLds --project $ProjetoApiLds
dotnet user-secrets set "Cors:AllowedOrigins:0" "http://localhost:5173" --project $ProjetoApiLds
dotnet user-secrets set "Cors:AllowedOrigins:1" "http://127.0.0.1:5173" --project $ProjetoApiLds
dotnet user-secrets set "BaseDados:AplicarMigracoesNoArranque" "false" --project $ProjetoApiLds
Remove-Variable chaveLds, bytesLds, rngLds
```

Resultado esperado: confirmações `Successfully saved` com os nomes das definições. Não copie a saída de `dotnet user-secrets list` para relatórios, pois contém os valores secretos.

Os segredos são associados ao `UserSecretsId` do projeto, no perfil do utilizador. Dois clones na mesma conta usam o mesmo conjunto de user secrets. Num computador já usado, registe a configuração anterior por meio seguro antes de a substituir, ou use variáveis de ambiente próprias da sessão. Uma cópia nova do código, por si só, não isola segredos existentes.

### Passo 6 - Criar a base e aplicar migrações

```powershell
dotnet ef database update `
    --project ClinicaVeterinaria.Infrastructure `
    --startup-project ClinicaVeterinaria.Api `
    --context ClinicaDbContext --connection $CadeiaLds
dotnet ef migrations has-pending-model-changes `
    --project ClinicaVeterinaria.Infrastructure `
    --startup-project ClinicaVeterinaria.Api --context ClinicaDbContext
```

Resultado esperado: migrações aplicadas e ausência de alterações pendentes ao modelo. A base contém o esquema do projeto e dados de referência da clínica, espécies, raças e configuração. Este procedimento de instalação não apaga bases existentes.

Se precisar de repetir uma instalação limpa, use uma base local de teste nova e registe o nome. O script `src/backend/scripts/reposicao-base-dados.ps1` apaga a base indicada antes de a recriar: só deve ser usado quando se pretende essa reposição, depois de confirmar que os dados são descartáveis. Nunca o use sobre a base pública.

### Passo 7 - Carregar os dados de demonstração

```powershell
$DadosLds = Join-Path $RaizLds "04-artefactos-tecnicos/04.13-scripts-de-base-de-dados"
$DadosLds = Join-Path $DadosLds "m3-v02/dados-demonstracao"
Set-Location $DadosLds
```

Na opção A:

```powershell
powershell -ExecutionPolicy Bypass -File .\executar-dados.ps1 `
    -Servidor $ServidorLds -BaseDados $BaseLds -ConfiarCertificado
```

Na opção B:

```powershell
$env:SQLCMDPASSWORD = $SenhaSqlLds
powershell -ExecutionPolicy Bypass -File .\executar-dados.ps1 `
    -Servidor $ServidorLds -BaseDados $BaseLds `
    -Utilizador sa -ConfiarCertificado
Remove-Item Env:SQLCMDPASSWORD
Remove-Variable SenhaSqlLds
```

Resultado esperado: segunda carga sem novas inserções e `28 verificações, 28 OK, 0 falhadas`. Não use `-DadosReferenciaProvisorios` numa base criada pelas migrações. O script gera um registo local; reveja-o antes de o partilhar.

As contas incluem `admin@clinica-g08.test`, `rececao@clinica-g08.test`, `vet.ana@clinica-g08.test`, `aux.carla@clinica-g08.test` e `cliente.joao@clinica-g08.test`. A conta `rececao.bloqueada@clinica-g08.test` está bloqueada de propósito. As palavras-passe são as da submissão e os dados são fictícios.

## 5. Arrancar e verificar o backend

### Passo 8 - Arrancar a API na janela B

Abra outra janela PowerShell na raiz do clone:

```powershell
Set-Location src\backend
dotnet run --project .\ClinicaVeterinaria.Api\ClinicaVeterinaria.Api.csproj `
    --launch-profile http
```

Resultado esperado: API em `http://localhost:5159`, ambiente `Development` e Swagger em `http://localhost:5159/swagger`. Deixe a janela aberta. Para parar a API, use Ctrl+C.

Na primeira utilização com SQL em Docker, confirme que o contentor já está pronto. Não avance se a API terminar com erro de chave JWT ou de ligação SQL.

### Passo 9 - Confirmar resposta e acesso autenticado

Na janela A:

```powershell
curl.exe -s -i http://localhost:5159/auth/me
```

Resultado esperado: **401**, porque não foi enviado um token. A resposta 401 neste pedido confirma que a API responde e exige autenticação.

```powershell
$SenhaContaLds = Ler-SegredoLds "Palavra-passe da conta de Administrador de demonstração"
$CorpoLoginLds = @{
    email = "admin@clinica-g08.test"
    password = $SenhaContaLds
} | ConvertTo-Json
$LoginLds = Invoke-RestMethod -Method Post `
    -Uri "http://localhost:5159/auth/login" `
    -ContentType "application/json" -Body $CorpoLoginLds
Remove-Variable SenhaContaLds, CorpoLoginLds
$LoginLds.utilizador | Select-Object nome, perfil
Invoke-RestMethod -Uri "http://localhost:5159/clinica" `
    -Headers @{ Authorization = "Bearer $($LoginLds.token)" } |
    Select-Object nome, nif
Invoke-RestMethod -Method Post -Uri "http://localhost:5159/auth/logout" `
    -Headers @{ Authorization = "Bearer $($LoginLds.token)" }
Remove-Variable LoginLds
```

Resultado esperado: perfil `Administrador`, nome da clínica devolvido sem erro e fim de sessão concluído. Não mostre ou fotografe `$LoginLds.token`. O Swagger pode ser usado em alternativa; não se fixa uma contagem de operações, que varia com a versão do código.

## 6. Instalar e arrancar o frontend web

### Passo 10 - Instalar e configurar na janela C

Abra a janela C na raiz do clone:

```powershell
Set-Location src\frontend
npm ci
Copy-Item .env.example .env.local
Set-Content .env.local "VITE_API_URL=http://localhost:5159" -Encoding utf8
npm run build
npm run dev
```

Resultado esperado: instalação a partir do ficheiro de bloqueio de dependências, compilação concluída e Vite em `http://localhost:5173`. Abra esse endereço no navegador. A página inicial é «Iniciar sessão».

O `npm ci` destina-se a esta cópia local e substitui as dependências instaladas para coincidir com o ficheiro de bloqueio de dependências. Não use `npm install` para corrigir erros de versão sem os analisar, pois pode alterar o ficheiro de bloqueio de dependências.

### Passo 11 - Confirmar uma sessão web real

Entre com `rececao@clinica-g08.test`, usando a palavra-passe da submissão. Abra Pacientes e a ficha de Bobi. Resultado esperado: dados devolvidos pela API local. Termine sessão.

Entre como `vet.ana@clinica-g08.test` e abra Pacientes → ficha de Bobi → Vacinação. Resultado esperado: histórico e próximas administrações, sem erro de rede. Termine sessão. Uma conta Cliente é recusada na aplicação web e encaminhada para a aplicação móvel; isso é o comportamento previsto.

### Compilação para apresentação local

Pare o servidor de desenvolvimento com Ctrl+C e execute, na mesma pasta:

```powershell
npm run build
npm run preview -- --host 127.0.0.1 --port 5173
```

Resultado esperado: a versão compilada abre em `http://127.0.0.1:5173`, origem já autorizada no passo 5. Só pode existir um servidor na porta 5173. O modo de pré-visualização é uma forma de verificar a compilação local; não constitui publicação do frontend na Internet.

## 7. Instalar e configurar a aplicação Android

### Passo 12 - Preparar SDK, JDK e dispositivo

No Android Studio, abra **a pasta `src/frontend-mobile`**, e não a raiz do repositório. No SDK Manager, confirme Android SDK Platform 37.0, Build-Tools 37.0.0, Platform-Tools e Android Emulator. Em Settings → Build Tools → Gradle, selecione JDK 21. Não aceite uma atualização automática do AGP ou do Gradle.

No Device Manager, crie e inicie um AVD API 37.0. No Windows x86-64, escolha uma imagem x86-64 e confirme a virtualização do processador. Em Apple Silicon, use uma imagem ARM64 compatível. Um dispositivo físico tem de ter Android API 26 ou superior, depuração USB ativa e o pedido de autorização USB aceite pelo proprietário.

Na janela A, prepare os caminhos do SDK. Se o SDK não estiver na localização indicada pelo Android Studio, substitua o valor pela localização mostrada no SDK Manager:

```powershell
Set-Location (Join-Path $RaizLds "src/frontend-mobile")
$env:ANDROID_HOME = Join-Path $env:LOCALAPPDATA "Android\Sdk"
$AdbLds = Join-Path $env:ANDROID_HOME "platform-tools\adb.exe"
& $AdbLds version
& $AdbLds devices
```

Resultado esperado: versão do ADB e dispositivo com estado `device`. Se houver vários, escolha o identificador correspondente:

```powershell
$DispositivoLds = Read-Host "Identificador do emulador ou telemóvel mostrado pelo adb devices"
.\gradlew.bat --version
```

Resultado esperado: Gradle 9.6.0 e JVM 21. Se o JDK não for encontrado, confirme a instalação do JDK 21 e configure `JAVA_HOME` para essa pasta. O `local.properties`, quando criado pelo Android Studio, é local e não deve ser versionado.

### Passo 13 - Escolher a ligação à API local

Use **uma** das opções seguintes. O endereço é definido na compilação, através de `apiBaseUrl`; alterar o endereço exige recompilar e reinstalar a aplicação. Não é uma definição alterável no ecrã de início de sessão.

**Emulador Android:** `10.0.2.2` representa o computador que executa o emulador. A API pode continuar a ouvir em `localhost:5159`:

```powershell
.\gradlew.bat assembleDebug "-PapiBaseUrl=http://10.0.2.2:5159"
```

**Telemóvel por USB, ou emulador com o tempo de ligação esgotado em `10.0.2.2`:** crie um túnel ADB e compile para `localhost`:

```powershell
& $AdbLds -s $DispositivoLds reverse tcp:5159 tcp:5159
& $AdbLds -s $DispositivoLds reverse --list
.\gradlew.bat assembleDebug "-PapiBaseUrl=http://localhost:5159"
```

Resultado esperado: túnel listado e `BUILD SUCCESSFUL`. Repita `adb reverse` após reiniciar ou voltar a ligar o dispositivo. Neste caso, `localhost` na aplicação chega ao computador pelo túnel; sem esse túnel, representa o próprio Android.

O debug só permite HTTP sem TLS para `10.0.2.2`, `localhost` e `127.0.0.1`. Um endereço HTTP arbitrário da rede local não está autorizado na configuração atual. O caminho por USB evita alterar o manifesto ou expor a API à rede. A variante release exige HTTPS.

### Passo 14 - Instalar o APK e abrir a aplicação

Depois de `assembleDebug` terminar:

```powershell
$ApkLds = ".\app\build\outputs\apk\debug\app-debug.apk"
Test-Path $ApkLds
& $AdbLds -s $DispositivoLds install -r $ApkLds
& $AdbLds -s $DispositivoLds shell am start `
    -n "pt.ipp.estg.g08.clinica/.MainActivity"
```

Resultado esperado: `True`, `Success` e abertura da aplicação. O APK debug serve para instalação de desenvolvimento e defesa; não é uma publicação assinada para a loja. Pode também usar Run no Android Studio, mas confirme a mesma propriedade `apiBaseUrl` para o APK instalado.

### Passo 15 - Confirmar a configuração e uma sessão de Cliente

No início de sessão, abra **Abrir ecrã técnico**, disponível só em debug. Confirme o endereço da API e use **Testar ligação à API**. Sem sessão, uma resposta de autenticação recusada é normal; uma mensagem de falta de rede indica que a ligação ainda não está resolvida. Não use «Guardar sessão de teste» como prova de início de sessão real.

Volte ao início de sessão e entre com `cliente.joao@clinica-g08.test`, usando a palavra-passe da submissão. Resultado esperado: «Os meus animais», com Bobi e Mia. Abra Bobi → Ver boletim e confirme as administrações. Abra Consultas e o Histórico. Termine sessão.

As marcações de demonstração têm datas fixas. A ausência de consultas futuras pode ser correta depois dessas datas. Para demonstrar uma consulta futura, crie-a primeiro na API local através da aplicação web como Rececionista, para um animal desse Cliente. Não use a ausência de marcações futuras como prova de erro de instalação.

Uma conta interna deve ser recusada na aplicação destinada ao Cliente. A sessão não deve ser guardada como sessão de Cliente. Esta verificação não exige mudar as permissões da conta.

## 8. Verificação final do sistema completo

### Passo 16 - Percurso integrado de instalação

| Ordem | Verificação | Resultado esperado |
|---|---|---|
| 1 | API local: `GET /auth/me`, sem token | 401; sem erro de ligação ou 500. |
| 2 | Início de sessão na API e leitura autenticada da clínica | Conta e clínica corretas, sem mostrar o token. |
| 3 | Web: Rececionista consulta o paciente Bobi | Dados da base local; sem erro CORS. |
| 4 | Web: Veterinário regista uma vacinação do Bobi | Registo guardado e próxima data calculada quando existe intervalo. |
| 5 | Aplicação: Cliente João consulta o boletim do Bobi | O registo novo do passo anterior aparece depois de voltar a abrir o boletim. |
| 6 | Terminar sessão nos dois frontends | Regresso ao início de sessão e acesso autenticado encerrado. |

No passo 4, use produto e lote fictícios identificados para esta validação, a data atual e um intervalo positivo, por exemplo 30 dias. Este ensaio altera apenas a base local escolhida. A correspondência no passo 5 confirma que as aplicações web e móvel estão ligadas à mesma API e base de dados.

A instalação só está confirmada para o sistema completo quando este percurso termina com sucesso. O Revisor deve repeti-lo numa instalação de raiz; compilar os projetos ou abrir o ecrã técnico, isoladamente, não cumpre esse objetivo.

## 9. Trocar entre API local e pública

### 9.1. Mapa dos endereços

| Componente | API individual/local | API pública de avaliação |
|---|---|---|
| Web no computador | `.env.local`: `http://localhost:5159` | `.env.local`: endereço HTTPS público; origem web tem de ser autorizada no CORS público. |
| Aplicação no emulador | `http://10.0.2.2:5159` | endereço HTTPS público na propriedade Gradle. |
| Aplicação por USB com túnel | `http://localhost:5159` | endereço HTTPS público; não precisa de túnel. |
| Base de dados | SQL Server do aluno | Azure SQL gerido pelo grupo; credenciais SQL nunca entram nos frontends. |

O endereço documentado pela I104 é `https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net`. Antes do ensaio público, confirme com o responsável do ambiente que esse endereço e as contas continuam ativos. O ambiente pode estar em pausa ou indisponível; isso não invalida o teste local.

### Passo 17 - Configurar a aplicação web para a API pública

Na janela C, pare o servidor com Ctrl+C. Defina o endereço HTTPS indicado na secção anterior numa variável `$ApiPublicaLds`, depois execute em `src/frontend`:

```powershell
$ApiPublicaLds = "https://clinica-g08-api-" +
    "ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net"
Set-Content .env.local "VITE_API_URL=$ApiPublicaLds" -Encoding utf8
npm run build
npm run dev
```

Resultado esperado: a aplicação web usa a API pública. O responsável dessa API deve autorizar `http://localhost:5173` em `Cors:AllowedOrigins` antes de um ensaio web. A configuração CORS dos user secrets locais não altera o Azure. Não desative a segurança do navegador para contornar uma recusa.

### Passo 18 - Configurar a aplicação para a API pública

Na janela A, em `src/frontend-mobile`, defina a mesma variável com o endereço público e execute:

```powershell
$ApiPublicaLds = "https://clinica-g08-api-" +
    "ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net"
.\gradlew.bat assembleDebug "-PapiBaseUrl=$ApiPublicaLds"
& $AdbLds -s $DispositivoLds install -r .\app\build\outputs\apk\debug\app-debug.apk
```

Resultado esperado: o ecrã técnico mostra o endereço HTTPS público. Termine a sessão antiga e inicie uma nova no ambiente selecionado: tokens e dados locais não são automaticamente válidos no público. CORS não se aplica à aplicação Android nativa.

### Passo 19 - Voltar ao ambiente individual de defesa

Na aplicação web, volte a definir `VITE_API_URL=http://localhost:5159`, pare e reinicie o Vite; para a pré-visualização, volte também a compilar. Na aplicação, repita a compilação e instalação do passo 13, com `10.0.2.2` ou `localhost` mais `adb reverse`. Confirme o endereço no ecrã técnico e repita o passo 16. A defesa individual não deve depender da API pública para funcionar.

## 10. Utilização em macOS e Linux

O código da API e da aplicação web pode ser executado nestes sistemas. Use os mesmos manifestos e versões. O procedimento Windows de raiz não é apresentado como já validado nestas plataformas.

- `.NET`: os comandos `dotnet` são equivalentes; use `/` nos caminhos. O `sqlcmd` utiliza autenticação SQL quando a autenticação Windows não está disponível. Para os dados, defina `SQLCMD_SERVIDOR`, `SQLCMD_UTILIZADOR` e `SQLCMDPASSWORD` só na sessão e execute `bash executar-dados.sh --base-dados NOME --confiar-certificado`. O script não aceita opções `--servidor` ou `--utilizador`.
- Web: `npm ci`, configuração de `VITE_API_URL`, `npm run build` e `npm run dev` são iguais. Pode editar `.env.local` num editor de texto.
- Android: use `./gradlew` em vez de `.\gradlew.bat`. O SDK costuma estar em `~/Library/Android/sdk` no macOS e `~/Android/Sdk` no Linux. Configure `ANDROID_HOME` com a pasta real e use o ADB de `platform-tools`.
- SQL Server: as imagens oficiais de contentor têm suporte apenas em anfitriões Linux Intel/AMD x86-64; a Microsoft não dá suporte à execução por Rosetta/QEMU em ARM. Num Mac Apple Silicon, planeie SQL Server num anfitrião x86-64 acessível ou uma máquina de defesa compatível, e ajuste a cadeia de ligação da API. Uma experiência em emulação não comprova suporte oficial nem uma instalação autónoma no Mac.

Na execução local observada foi necessário impedir o recarregamento automático dos ficheiros de configuração para arrancar a API dentro do ambiente de execução restrito. Se a API ficar parada antes de começar a ouvir, pode diagnosticar com `DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false` e `DOTNET_USE_POLLING_FILE_WATCHER=1`, apenas nessa sessão. Isto não altera o contrato nem substitui a investigação de uma falha numa máquina normal.

## 11. Paragem, atualização e repetição

Para parar, termine sessões, use Ctrl+C nas janelas da aplicação web e da API e feche o emulador se já não for necessário. Na opção Docker, `docker stop sql-clinica-m4` para o serviço e `docker start sql-clinica-m4` volta a iniciá-lo; o volume mantém os dados. No Android por USB, pode remover só o túnel usado com `adb -s IDENTIFICADOR reverse --remove tcp:5159`.

Para atualizar uma instalação, registe o commit anterior e escolha o commit aprovado seguinte. Pare a API e a aplicação web, restaure as dependências e compile o backend, aplique `dotnet ef database update`, execute `npm ci` e compile novamente. Recompile e reinstale o APK com o endereço do ambiente pretendido. Repita o passo 16. Não execute a reposição destrutiva apenas para aplicar migrações novas.

O APK instalado com `-r` conserva os dados locais. Se mudar de ambiente, termine sessão e volte a entrar. Se surgir incompatibilidade de assinatura com outra instalação, a desinstalação apaga os dados locais e a sessão; confirme que isso é pretendido antes de a fazer.

## 12. Problemas frequentes

| Sintoma | Verificação e correção |
|---|---|
| .NET SDK incompatível | Execute `dotnet --version` em `src/backend`. Instale o SDK 10.0 compatível com `global.json`; não edite esse ficheiro para ocultar o erro. |
| `dotnet ef` não reconhecido | Confirme 10.0.13 e o PATH das ferramentas globais. Abra uma janela nova depois de instalar. |
| SQL não responde ou autenticação SQL falha | Confirme serviço/contentor, instância e porta. Repita o `SELECT @@VERSION` com a autenticação correta antes de arrancar a API. |
| Certificado SQL local que não é de confiança | Na instalação de demonstração, use `-C` no sqlcmd e `TrustServerCertificate=True` na cadeia. Estas opções não alteram certificados HTTPS dos frontends. |
| Chave JWT em falta ou inválida | Repita o passo 5. A chave deve ser Base64 com pelo menos 32 bytes. Não use uma palavra-passe como chave. |
| API responde 500 por ligação vazia | O `appsettings.Development.json` deixa a ligação vazia. Configure user secrets ou a variável `ConnectionStrings__ClinicaDb` do processo que arranca a API. |
| Porta 5159 ou 5173 ocupada | Pare a instância que iniciou ou escolha outra porta. Ao mudar a porta da API, atualize os endereços das aplicações web e Android e o túnel; ao mudar a porta web, atualize CORS. |
| Web abre, mas os pedidos dão CORS | Confirme a origem exata, incluindo esquema, nome e porta. `localhost` e `127.0.0.1` são origens diferentes. A lista CORS vazia não autoriza todas as origens. Reinicie a API depois de configurar. |
| Alterar `.env.local` não muda a API utilizada | Pare e reinicie o Vite. No modo de pré-visualização, volte a compilar. Confirme se uma variável de ambiente `VITE_API_URL` do terminal está a sobrepor o ficheiro. |
| `npm ci` ou Vite não é reconhecido | Confirme Node/npm e que está em `src/frontend`. Repita `npm ci`; não reutilize `node_modules` de outra máquina ou sistema. |
| SDK Android ou licença em falta | Abra o SDK Manager, instale Platform 37.0 e Build-Tools 37.0.0 e leia/aceite as licenças aplicáveis. Confirme `ANDROID_HOME` ou `local.properties`. |
| Gradle usa o JDK errado | Confirme JVM 21 com o wrapper e a definição Gradle JDK no Android Studio. Não confunda o alvo Java 11 do código com o JDK 21 usado para correr o Gradle. |
| ADB mostra `unauthorized` ou não há dispositivo | Autorize a depuração no telemóvel, confirme o cabo de dados e o controlador USB ou inicie o AVD. Se houver vários dispositivos, use `-s`. |
| Aplicação: erro de rede em `10.0.2.2` | Confirme que a API local está ativa; use `adb reverse` e APK compilado com `http://localhost:5159`, conforme o passo 13. |
| Aplicação continua na API antiga | O endereço está no APK. Recompile e reinstale com a propriedade `apiBaseUrl` correta; confirme o endereço no ecrã técnico. |
| HTTP da rede local bloqueado no Android | O debug só autoriza os três endereços locais indicados. Use o túnel USB ou um endereço HTTPS válido, sem alargar o manifesto para contornar o problema. |
| 401 ou conta bloqueada no início de sessão | Confirme a base/ambiente e as credenciais da submissão. A conta bloqueada de demonstração deve ser recusada. Não altere contas para fazer passar o ensaio. |
| Web ou aplicação recusam determinado perfil | Web: perfis internos. Aplicação: Cliente. Verifique primeiro se está a usar a conta apropriada. |
| Aplicação não mostra a vacinação criada na aplicação web | Confirme que o ambiente, o paciente e o proprietário são os mesmos. Volte à lista e reabra o boletim para obter dados recentes. |
| API pública demora ou não responde | Confirme o endereço e a disponibilidade com o responsável; o serviço/base podem estar em pausa. Prossiga a defesa com a instalação individual local. |

## 13. Validação e referências

A v01 tem um registo de validação do backend em 04/11/2026. Esse resultado não valida automaticamente as novas instruções das aplicações web e móvel desta v02.

Nesta preparação foram confrontados os comandos com o código e os manifestos. As verificações locais do Executor, os desvios e os passos ainda não executados estão em `m4-s08-i054-20261127-registo-validacao-v01.md`. O critério de aceitação 4 permanece pendente até outra pessoa seguir esta v02 de raiz, registar máquina, versões, resultados e desvios, e serem confirmadas as correções.

O Revisor indicado no Sprint Planning é Gonçalo Dinis Magalhães Teixeira; o Auditor é Gonçalo Magalhães Ribeiro. Não se regista uma validação em nome de um colega sem que a tenha executado.

Referências internas: Backlog v06 e ata de Backlog Refinement S08, deliberação 5; ata de Sprint Planning de 25/11/2026; manual e registo da I038; READMEs de `src/backend`, `src/frontend` e `src/frontend-mobile`; scripts de demonstração `m3-v02`; README da I104 em `04.16-ci-cd/m3-v03`.

Referências oficiais consultadas em 27/11/2026:

- Android Developers, acesso a servidor local e `adb reverse`: https://developer.android.com/develop/ui/views/layout/webapps/access-local-server
- Microsoft Learn, SQL Server em contentores e suporte de arquitetura: https://learn.microsoft.com/sql/linux/quickstart-install-connect-docker?view=sql-server-ver17
- Microsoft Learn, atualização de ferramentas .NET e `--allow-downgrade`: https://learn.microsoft.com/dotnet/core/tools/dotnet-tool-update

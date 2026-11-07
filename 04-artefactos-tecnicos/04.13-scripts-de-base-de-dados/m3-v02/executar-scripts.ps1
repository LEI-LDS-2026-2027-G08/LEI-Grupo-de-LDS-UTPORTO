<#
.SYNOPSIS
    Executa, por ordem numérica, os scripts de criação do esquema
    ClinicaVeterinaria e guarda o registo da execução (evidência de execução).

.DESCRIPTION
    Executa com o sqlcmd os ficheiros 00-*.sql a 06-*.sql desta pasta, por ordem
    numérica, e pára no primeiro erro. Opcionalmente executa também a verificação
    do esquema e os testes de restrições da pasta "verificacao". Tudo o que é
    escrito na consola fica também no ficheiro esquema-execucao-AAAAMMDD-HHMMSS.log.

    Projeto: Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027.

.PARAMETER Servidor
    Instância SQL Server. Por omissão: localhost.

.PARAMETER Utilizador
    Login SQL Server. Se for omitido, é usada a autenticação do Windows (-E).
    Com login SQL, a palavra-passe é lida da variável de ambiente SQLCMDPASSWORD
    e nunca é passada na linha de comandos nem guardada em ficheiros.

.PARAMETER ConfiarCertificado
    Acrescenta -C (confiar no certificado do servidor). Necessário com o sqlcmd
    das ODBC Tools 18 numa instância local com certificado autoassinado.

.PARAMETER Verificar
    Executa também verificacao\verificar-esquema.sql no fim.

.PARAMETER Testar
    Executa também verificacao\testar-restricoes.sql no fim (base sem dados).

.PARAMETER PastaRegistos
    Pasta onde é criado o ficheiro de registo. Por omissão: a pasta atual.

.EXAMPLE
    .\executar-scripts.ps1 -Verificar -Testar

.EXAMPLE
    $env:SQLCMDPASSWORD = Read-Host 'Palavra-passe'; .\executar-scripts.ps1 -Servidor 'localhost,1433' -Utilizador sa -ConfiarCertificado -Verificar
#>
[CmdletBinding()]
param(
    [string]$Servidor = 'localhost',
    [string]$Utilizador,
    [switch]$ConfiarCertificado,
    [switch]$Verificar,
    [switch]$Testar,
    [string]$PastaRegistos = (Get-Location).Path
)

$ErrorActionPreference = 'Stop'
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

if (-not (Get-Command sqlcmd -ErrorAction SilentlyContinue)) {
    throw 'O sqlcmd não foi encontrado. Instale as SQL Server Command Line Utilities (ou o go-sqlcmd) e volte a executar.'
}
if ($Utilizador -and -not $env:SQLCMDPASSWORD) {
    throw 'Com -Utilizador, defina antes a variável de ambiente SQLCMDPASSWORD (ex.: $env:SQLCMDPASSWORD = Read-Host).'
}

$pasta = $PSScriptRoot
$scripts = @(Get-ChildItem -Path $pasta -Filter '*.sql' -File |
             Where-Object { $_.Name -match '^\d{2}-' } |
             Sort-Object Name)
if ($Verificar) { $scripts += Get-Item (Join-Path $pasta 'verificacao\verificar-esquema.sql') }
if ($Testar)    { $scripts += Get-Item (Join-Path $pasta 'verificacao\testar-restricoes.sql') }

# Argumentos de ligação construídos explicitamente como lista de texto
# (evita argumentos vazios no Windows PowerShell 5.1).
$ligacao = New-Object System.Collections.Generic.List[string]
$ligacao.Add('-S'); $ligacao.Add($Servidor)
if ($Utilizador) { $ligacao.Add('-U'); $ligacao.Add($Utilizador) } else { $ligacao.Add('-E') }
if ($ConfiarCertificado) { $ligacao.Add('-C') }

# Executa o sqlcmd sem que as mensagens de erro do sqlcmd interrompam o PowerShell;
# devolve as linhas de saída e deixa o código de saída em $LASTEXITCODE.
function Invocar-Sqlcmd([string[]]$Argumentos) {
    $anterior = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try {
        $todos = @($ligacao) + $Argumentos
        & sqlcmd $todos 2>&1 | ForEach-Object { "$_" }
    }
    finally {
        $ErrorActionPreference = $anterior
    }
}

New-Item -ItemType Directory -Path $PastaRegistos -Force | Out-Null
$registo = Join-Path $PastaRegistos ("esquema-execucao-{0}.log" -f (Get-Date -Format 'yyyyMMdd-HHmmss'))

function Escrever([string]$Texto) {
    Write-Host $Texto
    Add-Content -Path $registo -Value $Texto -Encoding UTF8
}

Escrever 'Execução dos scripts do esquema da base de dados ClinicaVeterinaria'
Escrever ("Data e hora      : {0}" -f (Get-Date -Format 'yyyy-MM-dd HH:mm:ss zzz'))
Escrever ("Computador       : {0}" -f $env:COMPUTERNAME)
Escrever ("Servidor         : {0}" -f $Servidor)
Escrever ("Autenticação     : {0}" -f $(if ($Utilizador) { "SQL Server (login $Utilizador)" } else { 'Windows' }))
Escrever ("Scripts          : {0}" -f (($scripts | ForEach-Object { $_.Name }) -join ', '))

$versao = @(Invocar-Sqlcmd @('-b', '-h', '-1', '-W', '-Q', 'SET NOCOUNT ON; SELECT @@VERSION;'))
if ($LASTEXITCODE -ne 0) {
    Escrever ($versao -join [Environment]::NewLine)
    Escrever 'ERRO: não foi possível ligar ao servidor. Nenhum script foi executado.'
    exit 1
}
Escrever ("SQL Server       : {0}" -f (($versao | Select-Object -First 1) -replace '\s+', ' '))

foreach ($s in $scripts) {
    Escrever ''
    Escrever ("=== {0} — início {1}" -f $s.Name, (Get-Date -Format 'HH:mm:ss'))
    $saida = @(Invocar-Sqlcmd @('-b', '-I', '-f', '65001', '-i', $s.FullName))
    $codigo = $LASTEXITCODE
    foreach ($linha in $saida) { Escrever "$linha" }
    Escrever ("=== {0} — fim {1} — código de saída {2}" -f $s.Name, (Get-Date -Format 'HH:mm:ss'), $codigo)
    if ($codigo -ne 0) {
        Escrever ("ERRO: execução interrompida em {0}. Registo: {1}" -f $s.Name, $registo)
        exit $codigo
    }
}

Escrever ''
Escrever ("Concluído sem erros. Registo: {0}" -f $registo)

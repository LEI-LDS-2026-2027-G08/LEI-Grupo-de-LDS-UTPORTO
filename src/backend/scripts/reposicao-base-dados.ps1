param(
    [Parameter(Mandatory = $true)]
    [string]$ConnectionString
)

$ErrorActionPreference = "Stop"

$Project = "ClinicaVeterinaria.Infrastructure"
$StartupProject = "ClinicaVeterinaria.Api"
$Context = "ClinicaDbContext"

$connectionAnterior = $env:ConnectionStrings__ClinicaDb

try {
    # O EF Core 10 não permite passar --connection ao database drop.
    # A connection string é fornecida ao projeto através da configuração
    # padrão do ASP.NET Core.
    $env:ConnectionStrings__ClinicaDb = $ConnectionString

    Write-Host ""
    Write-Host "============================================"
    Write-Host " Reposicao da Base de Dados - I062"
    Write-Host "============================================"
    Write-Host ""

    Write-Host "[1/4] A verificar compilacao..."

    dotnet build ClinicaVeterinaria.sln

    if ($LASTEXITCODE -ne 0) {
        throw "Falha na compilacao da solucao."
    }

    Write-Host ""
    Write-Host "[2/4] A eliminar a base de dados existente..."

    dotnet ef database drop `
        --force `
        --project $Project `
        --startup-project $StartupProject `
        --context $Context

    if ($LASTEXITCODE -ne 0) {
        throw "Falha ao eliminar a base de dados."
    }

    Write-Host ""
    Write-Host "[3/4] A criar a base de dados e aplicar migrations..."

    dotnet ef database update `
        --project $Project `
        --startup-project $StartupProject `
        --context $Context `
        --connection $ConnectionString

    if ($LASTEXITCODE -ne 0) {
        throw "Falha ao criar a base de dados e aplicar as migrations."
    }

    Write-Host ""
    Write-Host "[4/4] A verificar migrations pendentes..."

    dotnet ef migrations has-pending-model-changes `
        --project $Project `
        --startup-project $StartupProject `
        --context $Context

    if ($LASTEXITCODE -ne 0) {
        throw "O modelo EF possui alteracoes ainda nao representadas por migrations."
    }

    Write-Host ""
    Write-Host "============================================"
    Write-Host " Reposicao concluida com sucesso."
    Write-Host "============================================"
}
finally {
    if ($null -eq $connectionAnterior) {
        Remove-Item Env:ConnectionStrings__ClinicaDb -ErrorAction SilentlyContinue
    }
    else {
        $env:ConnectionStrings__ClinicaDb = $connectionAnterior
    }
}

# Clínica Veterinária - Backend

Backend da aplicação de Gestão de Clínica Veterinária desenvolvido em .NET 10 com ASP.NET Core Web API.

## Pré-requisitos

- .NET SDK 10
- A solução inclui um ficheiro `global.json` que garante a utilização do SDK .NET 10 compatível.
- Visual Studio ou outro IDE compatível com .NET 10

Para verificar a versão instalada:

```bash
dotnet --version
```

## Estrutura da solução

A solução é composta pelos seguintes projetos:

- `ClinicaVeterinaria.Api` - API ASP.NET Core e configuração da aplicação.
- `ClinicaVeterinaria.Application` - camada de aplicação.
- `ClinicaVeterinaria.Domain` - domínio da aplicação.
- `ClinicaVeterinaria.Infrastructure` - infraestrutura e integrações.
- `ClinicaVeterinaria.UnitTests` - testes unitários xUnit.
- `ClinicaVeterinaria.IntegrationTests` - testes de integração xUnit.

O projeto `Domain` não referencia nenhum outro projeto da solução.

O projeto `Api` depende apenas de `Application` e `Infrastructure`.

## Configuração

A aplicação utiliza o sistema de configuração do ASP.NET Core.

A configuração pode ser fornecida através de:

- `appsettings.json`
- `appsettings.{Environment}.json`
- User Secrets em desenvolvimento
- Variáveis de ambiente

O ficheiro `appsettings.Example.json` apresenta a estrutura de configuração necessária sem conter segredos reais.

### Configurações utilizadas

```text
ConnectionStrings:ClinicaDb
Jwt:Key
Jwt:Issuer
Jwt:Audience
Jwt:ExpirationHours
LimpezaTokensRevogados:IntervaloMinutos
Cors:AllowedOrigins
```

A validade configurada dos tokens JWT é de 8 horas.

### Variáveis de ambiente

As configurações podem ser substituídas através de variáveis de ambiente.

Exemplos:

```text
ConnectionStrings__ClinicaDb
Jwt__Key
Jwt__Issuer
Jwt__Audience
Jwt__ExpirationHours
LimpezaTokensRevogados__IntervaloMinutos
Cors__AllowedOrigins__0
Cors__AllowedOrigins__1
```

No ASP.NET Core, `__` corresponde ao separador `:` da configuração.

## Configurar a chave JWT em desenvolvimento

A chave JWT não deve ser armazenada nos ficheiros versionados.

A chave tem de estar em Base64 e ter pelo menos 32 bytes (256 bits). Para gerar uma chave nova em PowerShell (funciona no Windows PowerShell 5.1 e no PowerShell 7):

```powershell
$bytes = New-Object byte[] 32
[Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
$chave = [Convert]::ToBase64String($bytes)
dotnet user-secrets set "Jwt:Key" $chave --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
```

Pode ser configurada através de User Secrets:

```bash
dotnet user-secrets set "Jwt:Key" "<CHAVE_JWT>" --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
```

Se a secção `Jwt` estiver incompleta ou a chave for inválida, a API não arranca e indica o problema na consola. O `dotnet ef` continua a funcionar sem a chave.

Para consultar os User Secrets configurados:

```bash
dotnet user-secrets list --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
```

Não devem ser colocadas chaves JWT, palavras-passe ou cadeias de ligação reais no repositório.

## Compilar

Na raiz da solução:

```bash
dotnet build ./ClinicaVeterinaria.sln
```

A compilação deve terminar sem erros.

## Executar os testes

Na raiz da solução:

```bash
dotnet test ./ClinicaVeterinaria.sln
```

São executados os projetos:

- `ClinicaVeterinaria.UnitTests`
- `ClinicaVeterinaria.IntegrationTests`

## Executar a API

Na raiz da solução:

```bash
dotnet run --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
```

A API apresenta no terminal os endereços HTTP/HTTPS utilizados localmente.

## Swagger / OpenAPI

Em ambiente de desenvolvimento, o Swagger fica disponível no endereço apresentado pela aplicação no terminal, por exemplo:

```text
http://localhost:<PORTA>/swagger
```

O Swagger possui suporte para autenticação Bearer JWT através do botão `Authorize`.

## Autenticação e autorização

O início de sessão é feito em `POST /auth/login` (EP-001) e devolve um JWT assinado com HS256, válido durante 8 horas, sem refresh token. `GET /auth/me` (EP-003) devolve o utilizador do token com o perfil atual na base de dados.

O token contém as claims `sub`, `email`, `role`, `jti` e, conforme o perfil, `clienteId` ou `veterinarioId`.

Em cada pedido autenticado, a API confirma ainda que a conta continua ativa, que o perfil não mudou desde a emissão do token e que o `jti` não foi revogado. Uma conta bloqueada ou com perfil alterado perde o acesso de imediato.

### Fim de sessão

`POST /auth/logout` (EP-002) termina a sessão do token usado no pedido e responde `204` sem corpo. O `jti` do token fica guardado na tabela `TokenRevogado` até à expiração original do token (claim `exp`). A partir daí, qualquer pedido com esse token recebe `401`, em qualquer endpoint protegido, porque a verificação é feita na autenticação e não em cada controlador. Como a revogação está na base de dados, continua válida depois de a API reiniciar. Os outros tokens do mesmo utilizador (por exemplo, noutro dispositivo) não são afetados.

Um token sem `jti` ou sem `exp` válido não é aceite. Repetir o fim de sessão com o mesmo token devolve `401`, porque o token já está revogado; dois pedidos simultâneos com o mesmo token não criam linhas repetidas.

### Limpeza das revogações expiradas

O serviço `LimpezaTokensRevogadosService` apaga as linhas de `TokenRevogado` cuja `DataExpiracao` já passou. Corre uma vez no arranque da API e depois a cada `LimpezaTokensRevogados:IntervaloMinutos` minutos (por omissão 60; aceita de 1 a 1440). Um valor fora desse intervalo impede a API de arrancar.

Só são apagadas as revogações de tokens que já expiraram (`DataExpiracao` anterior ao instante atual). Um token é aceite até ao próprio instante da expiração, por isso a revogação mantém-se até lá e a limpeza nunca devolve o acesso a um token terminado por logout. Uma falha da limpeza (por exemplo, base de dados indisponível) fica no log (evento 3005) e não pára a API; o número de registos apagados fica no evento 3004.

### Regras para novos endpoints

Todos os endpoints exigem autenticação por omissão. Um endpoint público tem de ser marcado com `[AllowAnonymous]`.

Um único perfil — usar a política correspondente:

```csharp
[Authorize(Policy = PoliticasAutorizacao.Administrador)]
```

Políticas disponíveis em `PoliticasAutorizacao`: `Administrador`, `Rececionista`, `Veterinario`, `AuxiliarVeterinario`, `Cliente`, `Funcionario` (qualquer um dos quatro perfis internos) e `ClienteProprietario`.

Vários perfis — usar as constantes de `Perfis` (os perfis são alternativos):

```csharp
[Authorize(Roles = Perfis.Administrador + "," + Perfis.Rececionista + "," + Perfis.Veterinario)]
```

Rotas `/me/*` da aplicação móvel — herdar de `ClienteAutenticadoControllerBase`, que aplica a política de propriedade e expõe `ClienteIdAutenticado`. O identificador do Cliente vem sempre do token e nunca do pedido (RNF002):

```csharp
[Route("me/animais")]
public sealed class MeusAnimaisController : ClienteAutenticadoControllerBase
{
    // usar ClienteIdAutenticado para filtrar os dados
}
```

Para ler o utilizador autenticado num controlador: `User.ObterUtilizadorId()`, `User.ObterPerfil()`, `User.ObterClienteId()`, `User.ObterVeterinarioId()`, `User.ObterJti()` e `User.ObterExpiracao()`.

Um pedido sem token válido recebe `401`; um perfil sem permissão recebe `403`. Ambos seguem o formato de erro descrito abaixo.

### Política de palavra-passe

As contas de funcionário criadas em `POST /utilizadores` (EP-006) exigem uma palavra-passe com 8 a 128 caracteres e com pelo menos uma letra minúscula, uma letra maiúscula, um algarismo e um símbolo (o espaço não conta como símbolo). Uma palavra-passe fraca devolve `400`.

A regra está em `PalavraPasseForteAttribute` (`ClinicaVeterinaria.Application/Validacao`) e aplica-se a um DTO com `[PalavraPasseForte]`. As palavras-passe são guardadas apenas como hash (`PasswordHasher` v3) e nenhuma resposta da API devolve o hash.

### Verificação manual

O ficheiro `ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.http` tem um pedido válido e um pedido por cada erro previsto de EP-001, EP-002 e EP-003. Os pedidos de EP-002 devem ser executados pela ordem do ficheiro. As palavras-passe das contas de demonstração ficam num ficheiro local `http-client.env.json.user` (ignorado pelo Git), como explicado no topo desse ficheiro.

## CORS

As origens permitidas pelo frontend são configuradas através de:

```text
Cors:AllowedOrigins
```

ou através de variáveis de ambiente:

```text
Cors__AllowedOrigins__0
Cors__AllowedOrigins__1
```

## Tratamento de erros

A API possui tratamento transversal de erros com formato JSON uniforme:

```json
{
  "code": 400,
  "message": "Pedido inválido.",
  "detail": "Detalhe do erro."
}
```

As respostas `400` da validação automática dos modelos e as respostas `401` e `403` da autenticação seguem o mesmo formato.

São tratados os códigos:

- `400` - Bad Request
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Not Found
- `409` - Conflict
- `422` - Unprocessable Entity

## Logging

A infraestrutura transversal regista:

- método HTTP;
- caminho do pedido;
- código HTTP da resposta;
- duração do pedido;
- erros.

Os logs não registam corpos dos pedidos, palavras-passe, tokens JWT ou o header `Authorization`.

## Segurança

Segredos não devem ser adicionados ao controlo de versões.

Em desenvolvimento devem ser utilizados User Secrets. Nos restantes ambientes devem ser utilizadas variáveis de ambiente ou outro mecanismo seguro de gestão de segredos.

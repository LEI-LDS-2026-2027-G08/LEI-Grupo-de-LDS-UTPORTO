# Clínica Veterinária - Backend

Backend da aplicação de Gestão de Clínica Veterinária desenvolvido em .NET 8 com ASP.NET Core Web API.

## Pré-requisitos

- .NET SDK 8
- A solução inclui um ficheiro `global.json` que garante a utilização do SDK .NET 8 compatível.
- Visual Studio ou outro IDE compatível com .NET 8

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
ConnectionStrings:DefaultConnection
Jwt:Key
Jwt:Issuer
Jwt:Audience
Jwt:ExpirationHours
Cors:AllowedOrigins
```

A validade configurada dos tokens JWT é de 8 horas.

### Variáveis de ambiente

As configurações podem ser substituídas através de variáveis de ambiente.

Exemplos:

```text
ConnectionStrings__DefaultConnection
Jwt__Key
Jwt__Issuer
Jwt__Audience
Jwt__ExpirationHours
Cors__AllowedOrigins__0
Cors__AllowedOrigins__1
```

No ASP.NET Core, `__` corresponde ao separador `:` da configuração.

## Configurar a chave JWT em desenvolvimento

A chave JWT não deve ser armazenada nos ficheiros versionados.

Pode ser configurada através de User Secrets:

```bash
dotnet user-secrets set "Jwt:Key" "<CHAVE_JWT>" --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
```

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

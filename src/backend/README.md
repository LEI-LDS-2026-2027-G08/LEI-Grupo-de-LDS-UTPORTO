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

O e-mail das contas de funcionário segue a regra R-EMAIL da Especificação de Testes Unitários (I028): até 256 caracteres, sem espaços, com um único `@`, parte local não vazia e domínio com pelo menos um ponto e sem partes vazias. A regra está em `EmailValidoAttribute`; um e-mail inválido devolve `400`.

A regra está em `PalavraPasseForteAttribute` (`ClinicaVeterinaria.Application/Validacao`) e aplica-se a um DTO com `[PalavraPasseForte]`. As palavras-passe são guardadas apenas como hash (`PasswordHasher` v3) e nenhuma resposta da API devolve o hash.

### Atualização e bloqueio de contas de funcionário

`PUT /utilizadores/{id}` (EP-007) atualiza o nome, o e-mail, o telefone, o perfil e a autorização `podeRegistarApoio` de uma conta de funcionário e devolve a conta no formato de EP-005. `PATCH /utilizadores/{id}/estado` (EP-008) bloqueia a conta e responde `204`. Ambos são só do Administrador, que é sempre identificado pelo token e nunca pelo pedido.

Regras verificadas em cada pedido:

- Em EP-007 o perfil `Cliente` dá `400` (em EP-006 dá `422`, como prevê o contrato de cada operação) e uma conta de Cliente dá `404`, porque não é uma conta de funcionário.
- `podeRegistarApoio = true` só é aceite no perfil `AuxiliarVeterinario` (`422`). A regra é verificada em cada atualização, por isso um Auxiliar com apoio que muda de perfil tem de enviar `podeRegistarApoio = false`.
- O Administrador não pode remover o próprio perfil de Administrador (`422`), mas pode alterar os seus outros dados e o perfil de outro Administrador.
- O e-mail é guardado em minúsculas; um e-mail usado por outra conta, de funcionário ou de Cliente, dá `409` sem alterar dados. O próprio e-mail da conta não conta como conflito.
- EP-008 só aceita o estado `Bloqueado` (sem distinguir maiúsculas); outro valor dá `400`. Bloquear uma conta já bloqueada, a própria conta do Administrador ou uma conta de Cliente dá `422`. O desbloqueio não faz parte do contrato.

**Decisão sobre os tokens já emitidos (I066, critério 4):** o bloqueio e a mudança de perfil têm efeito imediato. Uma conta bloqueada deixa de iniciar sessão (EP-001 devolve `403`) e os tokens emitidos antes do bloqueio ou da mudança de perfil são rejeitados com `401` no pedido seguinte, porque cada pedido autenticado confirma o estado e o perfil atuais da conta (ver acima). Não é preciso revogar os tokens um a um na tabela `TokenRevogado`.

**Divergência registada:** o campo `motivo` de EP-008 é aceite (até 500 caracteres), mas não é guardado, porque o Modelo de Dados (I016) não tem uma coluna para ele.

### Verificação manual

O ficheiro `ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.http` tem um pedido válido e um pedido por cada erro previsto de EP-001 a EP-008 (os de EP-007 e EP-008 criam a sua própria conta para não alterar os dados de demonstração). Os pedidos de EP-002 devem ser executados pela ordem do ficheiro. As palavras-passe das contas de demonstração ficam num ficheiro local `http-client.env.json.user` (ignorado pelo Git), como explicado no topo desse ficheiro.

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

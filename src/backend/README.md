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

A política expõe o cabeçalho `ETag` (`Access-Control-Expose-Headers`), para o frontend web poder ler a versão da marcação devolvida por EP-024 (Documentação de API v02, secção 5.1). O cabeçalho `If-Match`, que a I074 vai usar, já é aceite por `AllowAnyHeader`.

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

## Agenda e consulta de marcações (EP-023, EP-024)

`GET /consultas` (EP-023) devolve a agenda paginada (`PagedResultDto<ConsultaListItemDto>`) e `GET /consultas/{id}` (EP-024) devolve uma marcação com as notas de preparação (`ConsultaDto`). Os dois são da Rececionista, do Veterinário e do Auxiliar Veterinário; o Administrador e o Cliente recebem `403`.

**Quem vê o quê.** A Rececionista e o Auxiliar Veterinário veem as marcações de todos os Veterinários e podem filtrar por `veterinarioId`. O Veterinário só vê as que lhe estão atribuídas: o servidor ignora o `veterinarioId` do pedido e usa a claim `veterinarioId` do token (RNF002). Em EP-024, uma marcação de outro Veterinário dá `403` e uma marcação inexistente dá `404` (para qualquer perfil). A regra está em `AcessoConsultas` (`ClinicaVeterinaria.Application/Seguranca`), que lê o perfil e o Veterinário do token.

**Filtros de EP-023** (todos opcionais):

- `dataInicio` e `dataFim` são dias em hora de Lisboa (`AAAA-MM-DD`) e incluem os dois extremos. Conta o início da marcação: de `dataInicio` 00:00 a `dataFim` 24:00, hora local, convertidos para UTC (regra R-FUSO). Só com `dataInicio`, vêm as marcações dessa data em diante; só com `dataFim`, até ao fim desse dia. Ao contrário de EP-022, não há um limite de dias, porque a resposta é paginada.
- `dataFim` anterior a `dataInicio`, ou datas fora de 2000-01-01 a 9998-12-31, dão `400`. Uma data com formato inválido (por exemplo, `2026-13-40`) também dá `400`, com a mensagem do ASP.NET Core.
- `estado` aceita `Pendente`, `Confirmada`, `Realizada` ou `Cancelada`, sem distinguir maiúsculas; outro valor dá `400`. Sem `estado`, a agenda inclui todos os estados, também as marcações canceladas.
- `veterinarioId` e `animalId`, quando indicados, têm de ser superiores a 0 (`400`).
- `pagina` (1 por omissão) e `tamanho` (20 por omissão, de 1 a 100), como nas outras listas. As marcações vêm por data e hora e, em caso de empate, por identificador.

| Pedido | Marcação às 23:30 UTC de 24/10 (00:30 de 25/10 em Lisboa) | Marcação às 23:30 UTC de 25/10 (23:30 de 25/10 em Lisboa) |
|---|---|---|
| `dataInicio=dataFim=2026-10-24` | não | não |
| `dataInicio=dataFim=2026-10-25` | sim | sim |
| `dataInicio=dataFim=2026-10-26` | não | não |

**Sem dados clínicos (critério 5).** `ConsultaListItemDto`, `ConsultaDto` e `NotaPreparacaoDto` só têm dados administrativos (data e hora, duração, estado, motivo, chegada, animal, proprietário e Veterinário) e as notas de preparação. O `ConsultasService` não lê o registo clínico, os tratamentos nem as prescrições. A resposta é a mesma para os três perfis, por isso o Auxiliar Veterinário, com ou sem autorização de apoio, recebe as notas e nenhum dado clínico. O telefone do proprietário (`cliente.telefone`) vem a `null`: só é devolvido na ficha do animal (EP-016).

**Versão da marcação.** EP-024 devolve o cabeçalho `ETag` com a versão atual (`Marcacao.RowVersion` em Base64, entre aspas, por exemplo `"AAAAAAAAB9E="`), calculado em `VersaoMarcacao` (`ClinicaVeterinaria.Application/Agenda`). A comparação com `If-Match` e a resposta `412` são da I074 (EP-026 a EP-028).

As datas e horas da resposta (`dataHora`, `chegadaEm`, `criadaEm`) estão em UTC, com o sufixo `Z`.

## Segurança

Segredos não devem ser adicionados ao controlo de versões.

Em desenvolvimento devem ser utilizados User Secrets. Nos restantes ambientes devem ser utilizadas variáveis de ambiente ou outro mecanismo seguro de gestão de segredos.

## Disponibilidade dos Veterinários (EP-021, EP-022)

`GET /veterinarios` (EP-021) lista os Veterinários com conta ativa, por nome. `GET /veterinarios/{id}/disponibilidade?dataInicio=AAAA-MM-DD&dataFim=AAAA-MM-DD` (EP-022) devolve, para cada dia do período, os horários livres do Veterinário. As duas operações são só para o perfil Rececionista.

O cálculo está em `CalculadoraDisponibilidade` (`ClinicaVeterinaria.Application/Agenda`), uma função sem acesso à base de dados: recebe o horário de funcionamento, a duração das consultas, as marcações do Veterinário e o instante atual. O `VeterinariosService` só lê esses dados e prepara a resposta. `CalculadoraDisponibilidade.IntervaloDisponivel` aplica as mesmas regras a um intervalo qualquer, para ser usada na criação e alteração de marcações (EP-025, EP-026).

Regras:

- **Grelha:** os horários de cada período começam na abertura e repetem-se a cada `duracaoConsultaMinutos` (configuração de EP-010; 30 por omissão). Um horário só aparece se a consulta acabar até ao fecho do período.
- **Marcações:** cada marcação não cancelada ocupa `[início, início + a sua duração)`. Um horário que se sobreponha a uma marcação não aparece; intervalos que só se tocam não se sobrepõem. As marcações `Cancelada` não ocupam horário.
- **Hora local:** as marcações estão guardadas em UTC e o horário de funcionamento em hora de Lisboa (`Europe/Lisbon`). Os horários livres são devolvidos em hora de Lisboa (`HH:mm`), já com a hora de verão ou de inverno de cada dia. No dia da mudança para a hora de verão, uma hora que não existe (por exemplo, 01:00) não é um horário.
- **Passado:** os horários que começam antes do instante atual não são devolvidos; um dia já passado aparece com a lista vazia.
- **Período:** `dataInicio` é obrigatória; sem `dataFim`, o período é um só dia. `dataFim` não pode ser anterior a `dataInicio` e o período tem no máximo 31 dias, contando os dois extremos. As datas têm de estar entre 2000-01-01 e 9998-12-31. Caso contrário: `400`.
- **Veterinário:** um id que não é de um Veterinário com conta ativa (inexistente, de outro perfil ou bloqueado) dá `404`.

Exemplos com o horário dos dados da I061 (segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00) e consultas de 30 minutos:

| Situação | Resultado |
|---|---|
| Dia útil sem marcações | 18 horários: `09:00` a `12:30` e `14:00` a `18:30` |
| Fecho do período | `12:30` aparece (acaba às 13:00); `13:00` e `19:00` não |
| Marcação às 10:00 (30 minutos) | `10:00` não aparece; `09:30` e `10:30` aparecem |
| Marcação às 10:15 com 60 minutos | `10:00`, `10:30` e `11:00` não aparecem; `09:30` e `11:30` aparecem |
| Marcação cancelada às 10:00 | `10:00` aparece |
| Sábado | `09:00` a `12:30` |
| Domingo | dia devolvido com a lista vazia |
| Consultas de 45 minutos | manhã: `09:00`, `09:45`, `10:30`, `11:15`, `12:00` |
| Hoje às 09:10 | o primeiro horário é `09:30` |
| `dataInicio=2026-10-01&dataFim=2026-10-31` | aceite (31 dias) |
| `dataInicio=2026-10-01&dataFim=2026-11-01` | `400` (32 dias) |

Os casos estão nos testes unitários `CalculadoraDisponibilidadeTests`, `ValidadorPeriodoDisponibilidadeTests` e `VeterinariosServiceTests` (UT-F003-001 a UT-F003-018 da Especificação de Testes Unitários). Com a API e o SQL Server, `VeterinariosEndpointsTests` verifica um pedido válido e cada código de erro (200, 400, 401, 403 e 404). Os pedidos de verificação manual estão na secção I071 de `ClinicaVeterinaria.Api.http`.

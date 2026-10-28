# Collection Postman da API — m3 (I098)

Collection Postman e ambiente de variáveis para testar a API do backend, com os pedidos de EP-001 a EP-020: autenticação, contas de funcionário, clínica, clientes, pacientes e animais do Cliente.

| Ficheiro | Conteúdo |
|---|---|
| `m3-collection-postman-v01.json` | Collection (formato Postman v2.1), com 125 pedidos e os respetivos testes |
| `m3-ambiente-postman-v01.json` | Ambiente: URL base, contas de demonstração, palavras-passe vazias e variáveis preenchidas pelos scripts |

Referências: Documentação de API (I018), dados de demonstração da I061 (`04.13-scripts-de-base-de-dados/m3-v02`) e pedidos de verificação manual do backend (`src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.http`).

## Pré-requisitos

1. Base de dados com as migrações e os dados de demonstração da I061 (por exemplo, com `src/backend/scripts/reposicao-base-dados.ps1`).
2. API em execução (`dotnet run --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj --launch-profile http`), em `http://localhost:5159`. Se a porta for outra, alterar `baseUrl` no ambiente.
3. As palavras-passe das contas de demonstração. **Não estão no repositório** (ver o README dos dados de demonstração da I061): ficam apenas no ficheiro de credenciais da submissão. O ambiente versionado tem estas variáveis vazias.

## Executar no Postman

1. Importar os dois ficheiros (*Import*).
2. Selecionar o ambiente «Clínica Veterinária G08 — local (seed I061)».
3. Preencher, apenas no **Current value** (que não é exportado nem sincronizado), as variáveis `passwordAdmin`, `passwordRececionista`, `passwordVeterinario`, `passwordAuxiliar`, `passwordCliente` e `passwordBloqueada`.
4. Executar a collection inteira no *Collection Runner*, pela ordem.

## Executar com o Newman

Na pasta deste README, com o Node.js instalado:

```powershell
npx -y newman@6.2.1 run m3-collection-postman-v01.json -e m3-ambiente-postman-v01.json `
  --env-var "passwordAdmin=<...>" --env-var "passwordRececionista=<...>" `
  --env-var "passwordVeterinario=<...>" --env-var "passwordAuxiliar=<...>" `
  --env-var "passwordCliente=<...>" --env-var "passwordBloqueada=<...>"
```

Para guardar o resultado como evidência, acrescentar `--reporters cli,json --reporter-json-export newman-resultado.json`. O relatório não deve ser guardado com as palavras-passe: o Newman inclui o ambiente final no JSON, por isso é preciso rever o ficheiro antes de o arquivar, ou arquivar só a saída da consola.

A collection pode ser executada várias vezes seguidas sem repor a base de dados. Os pedidos que criam dados usam e-mails novos em cada execução, e a clínica volta aos dados iniciais no fim da pasta 03.

## Estrutura

As pastas seguem as funcionalidades e são executadas por esta ordem:

| Pasta | Operações | Pedidos |
|---|---|---|
| `00 · Preparação — sessões por perfil` | EP-001 (login de cada perfil) | 5 |
| `01 · Autenticação (EP-001 a EP-003)` | EP-001 (erros), EP-003, EP-002 | 15 |
| `02 · Contas de funcionário (EP-004 a EP-008)` | EP-004 a EP-008 | 39 |
| `03 · Clínica (EP-009, EP-010)` | EP-009, EP-010 | 10 |
| `04 · Clientes (EP-011 a EP-014)` | EP-011 a EP-014 | 22 |
| `05 · Pacientes (EP-015 a EP-018)` | EP-015 a EP-018 | 24 |
| `06 · Animais do Cliente — aplicação móvel (EP-019, EP-020)` | EP-019, EP-020 | 10 |

Cada pasta depende das anteriores: a pasta 00 guarda os tokens e as pastas seguintes guardam os identificadores que criam. Uma pasta executada sozinha só funciona depois da pasta 00.

## Convenções

**Nomes dos pedidos:** `EP-NNN · <código HTTP> — <caso>`, por exemplo `EP-007 · 422 — o Administrador remove o próprio perfil`. Os pedidos que só preparam o pedido seguinte usam `preparação` em vez do código e indicam entre parênteses a operação que chamam.

**Nomes das pastas:** `NN · <funcionalidade> (EP-xxx a EP-yyy)`. As áreas seguintes continuam a numeração (07, 08, …).

**Tokens:** cada perfil tem um pedido de login na pasta 00 que guarda o token no ambiente: `tokenAdmin`, `tokenRececionista`, `tokenVeterinario`, `tokenAuxiliar`, `tokenCliente`. Os pedidos enviam `Authorization: Bearer {{tokenPerfil}}`. Um pedido que precisa de uma sessão própria (por exemplo, o fim de sessão em EP-002) faz o seu login e guarda o token noutra variável (`tokenFimSessao`), para não invalidar os tokens partilhados.

**Identificadores:** nunca são escritos à mão. São guardados pelos scripts a partir das respostas, com `pm.environment.set`: o `id` do Administrador e o `clienteId` vêm do login, os animais do seed vêm da pesquisa em EP-015, e os recursos criados (`funcionarioNovoId`, `clienteNovoId`, `animalNovoId`) vêm da resposta `201`. A única constante é `idInexistente` (999999), usada nos casos `404`.

**Dados criados:** os pedidos de criação usam e-mails `i098.*@clinica-g08.test` gerados no pré-pedido, com a data e a hora. A palavra-passe das contas criadas pela collection é `passwordInicialTeste`, um valor de teste que só existe nessas contas. As operações que alteram dados do seed trabalham sobre os recursos criados pela collection; a clínica (EP-010) é reposta no fim.

**Testes:** cada pedido verifica o código HTTP e os campos essenciais da resposta, com `pm.test`. A collection tem ainda dois testes que correm depois de todos os pedidos:

- nenhuma resposta contém palavras-passe nem hashes;
- as respostas de erro seguem o formato único da API (I058): `code` igual ao código HTTP e `message` preenchida.

**Novas áreas:** para acrescentar uma área (por exemplo, marcações em EP-021 e seguintes), criar a pasta seguinte com os pedidos de sucesso e um pedido por cada erro previsto no contrato; reutilizar os tokens da pasta 00; guardar no ambiente os identificadores criados; e acrescentar ao ambiente as variáveis novas, com valor vazio.

## Pedidos por operação

| EP | Pedidos | Casos |
|---|---|---|
| EP-001 | 10 | 200 por perfil (5), 400 e-mail em falta, 400 palavra-passe em falta, 401 palavra-passe incorreta, 401 e-mail inexistente, 403 conta bloqueada |
| EP-002 | 5 | preparação, 204, 401 token terminado (em EP-003), 401 terminar outra vez, 401 sem token |
| EP-003 | 5 | 200 Administrador, 200 Cliente com `clienteId`, 401 sem token, 401 token adulterado, 401 assinado com outra chave |
| EP-004 | 5 | 200, 200 com filtros, 400 perfil inválido, 401, 403 |
| EP-005 | 5 | 200, 404 conta de Cliente, 404 inexistente, 401, 403 |
| EP-006 | 8 | 201 com `Location`, 400 palavra-passe fraca, 409, 422 perfil Cliente, 422 apoio fora do Auxiliar, 401, 403, login da conta criada |
| EP-007 | 10 | 200, 401 token com o perfil antigo, 400 dados inválidos, 400 perfil Cliente, 404, 409, 422 próprio perfil, 422 apoio, 401, 403 |
| EP-008 | 11 | preparação, 400, 404, 422 próprio Administrador, 422 Cliente, 401, 403, 204, 401 token anterior ao bloqueio, 403 login bloqueado, 422 já bloqueada |
| EP-009 | 3 | 200, 401, 403 |
| EP-010 | 7 | 200, 400 períodos sobrepostos, 400 fecho igual à abertura, 400 campos inválidos, 401, 403, 200 reposição |
| EP-011 | 5 | 200, 400 página inválida, 401, 403 Auxiliar, 403 Cliente |
| EP-012 | 4 | 200 com animais, 404, 401, 403 |
| EP-013 | 7 | 201 com `Location`, login do Cliente criado, 400, 409 NIF, 409 e-mail, 401, 403 |
| EP-014 | 6 | 200, 400, 404, 409 NIF, 401, 403 |
| EP-015 | 5 | 200 por Cliente, 200 por nome, 400 paginação, 401, 403 |
| EP-016 | 4 | 200 (Auxiliar, sem dados clínicos), 404, 401, 403 |
| EP-017 | 8 | 201 com `Location`, 400 peso, 400 data futura, 400 sem `clienteId`, 404 cliente, 401, 403 Auxiliar, 403 Cliente |
| EP-018 | 7 | 200, 400 peso, 400 data futura, 404 animal, 404 cliente, 401, 403 |
| EP-019 | 5 | 200, 200 do Cliente criado, 200 `clienteId` ignorado, 401, 403 |
| EP-020 | 5 | 200 sem dados do proprietário, 403 animal de outro Cliente, 404, 401, 403 |
| **Total** | **125** | |

## Validação da collection

Execução com o Newman 6.2.1 a 2026-10-26, contra a API do branch da I098 (que inclui a I066) e uma base de dados descartável criada com as migrações e os dados de demonstração da I061. Nessa base, e só nela, as palavras-passe das contas de demonstração foram substituídas por uma palavra-passe de teste, porque as verdadeiras não estão no repositório.

| | Executados | Falhados |
|---|---|---|
| Pedidos | 125 | 0 |
| Scripts de teste | 250 | 0 |
| Scripts de pré-pedido | 6 | 0 |
| Verificações (`pm.test`) | 403 | 0 |

A segunda execução seguida, sobre a mesma base, teve o mesmo resultado. A execução de referência para a evidência da I098 (critério 6) deve ser feita contra a base com o seed original, com as credenciais da submissão.

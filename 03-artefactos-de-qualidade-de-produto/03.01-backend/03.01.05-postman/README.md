# Collection Postman da API — m3 (I098, I100)

Collection Postman e ambiente de variáveis para testar a API do backend: autenticação, contas de funcionário, clínica, clientes, pacientes e animais do Cliente (EP-001 a EP-020, I098), registo clínico (EP-031 a EP-034, I100) e boletim de vacinas e desparasitações (EP-042 a EP-044 e a atualização do RF056, I100). A pasta 07 fica reservada à agenda e à área do Cliente (EP-021 a EP-030, I099).

| Ficheiro | Conteúdo |
|---|---|
| `m3-collection-postman-v01.json` | Collection (formato Postman v2.1), com 190 pedidos e os respetivos testes |
| `m3-ambiente-postman-v01.json` | Ambiente: URL base, contas de demonstração, palavras-passe vazias e variáveis preenchidas pelos scripts |
| `m3-s05-i098-20261030-resultado-newman-v01.txt` | Evidência da execução de referência com o Newman (critério 6), contra o seed original da I061: 125 pedidos e 403 verificações, sem falhas (ver [Validação da collection](#validação-da-collection)) |
| `m3-s05-i100-20261101-resultado-newman-v01.txt` | Evidência da execução de referência da I100 com o Newman, com as pastas 08 e 09: 190 pedidos e 611 verificações, sem falhas |

Referências: Documentação de API v02 (I018), dados de demonstração da I061 (`04.13-scripts-de-base-de-dados/m3-v02`) e pedidos de verificação manual do backend (`src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.http`).

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

Com o Node.js instalado:

1. Copiar `m3-ambiente-postman-v01.json` para uma pasta **fora do repositório** (por exemplo, `C:\Users\<utilizador>\Desktop\newman\`) e preencher nessa cópia o `value` das seis variáveis `password…`. A cópia com palavras-passe nunca volta para o repositório.
2. Na pasta deste README, executar no PowerShell, indicando o caminho da cópia em `-e`:

```powershell
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
npx -y newman@6.2.1 run m3-collection-postman-v01.json -e <caminho-da-copia>\m3-ambiente-postman-v01.json `
  --reporters cli --color off --disable-unicode |
  Out-File -Encoding utf8 m3-s05-<issue>-<aaaammdd>-resultado-newman-v01.txt
```

A primeira linha evita que os acentos fiquem trocados no ficheiro. A evidência é a saída da consola, gravada no `.txt`. Antes de o arquivar, confirmar que a tabela final tem 0 falhas e que o ficheiro não contém nenhuma palavra-passe (por exemplo, procurar `Demo-`).

As palavras-passe não devem ser passadas com `--env-var`: o `npx` escreve o comando completo na consola (linha `npm notice run newman run …`), e ficariam no `.txt`. Pela mesma razão, não usar o relatório `json` do Newman como evidência, porque inclui o ambiente final com as palavras-passe.

A collection pode ser executada várias vezes seguidas sem repor a base de dados. Os pedidos que criam dados usam e-mails novos em cada execução, a clínica volta aos dados iniciais no fim da pasta 03, a pasta 08 cria as suas próprias marcações nos horários livres seguintes e a pasta 09 acrescenta duas administrações em cada execução.

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
| `07` | Reservada à agenda e à área do Cliente (EP-021 a EP-030, I099) | — |
| `08 · Registo clínico (EP-031 a EP-034)` | EP-023, EP-022, EP-025 e EP-027 (preparação), EP-031 a EP-034 | 35 |
| `09 · Boletim de vacinas e desparasitações (EP-042 a EP-044)` | EP-043, EP-042, atualização (RF056), EP-044 | 30 |

Cada pasta depende das anteriores: a pasta 00 guarda os tokens e as pastas seguintes guardam os identificadores que criam. Uma pasta executada sozinha só funciona depois da pasta 00.

## Convenções

**Nomes dos pedidos:** `EP-NNN · <código HTTP> — <caso>`, por exemplo `EP-007 · 422 — o Administrador remove o próprio perfil`. Os pedidos que só preparam o pedido seguinte usam `preparação` em vez do código e indicam entre parênteses a operação que chamam.

**Nomes das pastas:** `NN · <funcionalidade> (EP-xxx a EP-yyy)`. As áreas seguintes continuam a numeração (07, 08, …).

**Tokens:** cada perfil tem um pedido de login na pasta 00 que guarda o token no ambiente: `tokenAdmin`, `tokenRececionista`, `tokenVeterinario`, `tokenAuxiliar`, `tokenCliente`. Os pedidos enviam `Authorization: Bearer {{tokenPerfil}}`. Um pedido que precisa de uma sessão própria (por exemplo, o fim de sessão em EP-002) faz o seu login e guarda o token noutra variável (`tokenFimSessao`), para não invalidar os tokens partilhados.

**Identificadores:** nunca são escritos à mão. São guardados pelos scripts a partir das respostas, com `pm.environment.set`: o `id` do Administrador e o `clienteId` vêm do login, os animais do seed vêm da pesquisa em EP-015, e os recursos criados (`funcionarioNovoId`, `clienteNovoId`, `animalNovoId`) vêm da resposta `201`. Na pasta 08, o `veterinarioId` vem da agenda do Veterinário da sessão (EP-023), a `consultaOutroVeterinarioId` da marcação M4 da I061 (de outro Veterinário, em 2026-11-04), as datas das marcações dos dois horários livres seguintes (EP-022, convertidos de hora de Lisboa para UTC) e `consultaRegistoId`, `consultaPendenteId`, `registoClinicoId`, `tratamentoId` e `prescricaoId` das respostas `201`. Na pasta 09, `administracaoVacinaId` e `administracaoDesparasitacaoId` vêm das respostas `201`. A única constante é `idInexistente` (999999), usada nos casos `404`.

**Dados criados:** os pedidos de criação usam e-mails `i098.*@clinica-g08.test` gerados no pré-pedido, com a data e a hora. A pasta 08 cria em cada execução duas marcações do Veterinário da sessão para o Bobi: uma é confirmada e recebe o registo clínico (com o texto «(I100)»), a outra fica `Pendente` para o caso `422` e é cancelada no fim da pasta. A pasta 09 regista no Bobi uma vacinação e uma desparasitação com produto e lote `I100`, com datas de administração fixas (2026-10-14), para as datas previstas serem sempre as mesmas. A palavra-passe das contas criadas pela collection é `passwordInicialTeste`, um valor de teste que só existe nessas contas. As operações que alteram dados do seed trabalham sobre os recursos criados pela collection; a clínica (EP-010) é reposta no fim.

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
| Preparação (pasta 08) | 7 | EP-023 Veterinário da sessão, EP-023 marcação de outro Veterinário, EP-022 horários livres, EP-025 marcação a confirmar, EP-027 confirmar, EP-025 marcação Pendente, EP-027 cancelar no fim |
| EP-031 | 9 | 201 com `Location`, 400 sem elemento clínico, 400 prescrição incompleta, 404, 403 consulta de outro Veterinário, 422 marcação `Pendente`, 409 consulta com registo, 401, 403 Auxiliar |
| EP-032 | 6 | 200 (mantém, altera e acrescenta tratamentos e prescrições; `atualizadoEm`/`atualizadoPor`), 400 sem elemento clínico, 400 tratamento de outro registo, 404, 401, 403 Auxiliar |
| EP-033 | 6 | 200 Veterinário, 200 Auxiliar (só leitura), 404, 401, 403 Rececionista, 403 Cliente |
| EP-034 | 7 | 200 por ordem descendente, 200 filtrado por datas, 200 animal sem registos (Auxiliar), 400 datas, 404, 401, 403 |
| EP-043 | 9 | 201 vacinação com intervalo (próxima calculada, `Location`), 201 desparasitação sem intervalo, 400 data futura, 400 `intervaloDias` = 0, 400 tipo e lote, 404, 401, 403 Auxiliar, 403 Cliente |
| EP-042 | 7 | 200 boletim (ordem das administrações e das próximas), 200 filtro `tipo`, 400 tipo inválido, 404, 401, 403 Auxiliar, 403 Cliente |
| RF056 (atualização) | 7 | 200 recálculo da próxima, 200 intervalo acrescentado, 400 data futura, 404 administração inexistente, 404 administração de outro animal, 401, 403 Auxiliar |
| EP-044 | 7 | 200 dono (sem lote nem Veterinário), 200 outro Cliente com o boletim vazio, 403 animal de outro Cliente (×2, sem dados), 404, 401, 403 Veterinário |
| **Total** | **190** | |

### Divergência — atualização de uma administração (RF056)

A I100 inclui «a atualização do registo profilático», mas a Documentação de API v02 ainda não tem essa operação (ponto em aberto 2: prevista para a v03) e o RF056 não consta da Especificação de Requisitos v02. Os pedidos seguem a implementação da I082 (`PUT /animais/{id}/vacinacoes/{administracaoId}`, só Veterinário) e usam o prefixo `RF056` em vez de um número de EP. Quando a API v03 der um número à operação, os nomes dos pedidos devem ser atualizados.

## Validação da collection

### Execução de referência da I100 — 2026-11-01

Execução com o Newman 6.2.1 a 2026-11-01, contra a API do ramo `develop` (commit `16c4bbb`, já com a I078 e a I082 integradas) e uma base de dados nova, criada com `src/backend/scripts/reposicao-base-dados.ps1` (migrações da I062) e com os dados de demonstração da I061 (`executar-dados.ps1`: 28 verificações, 28 OK), com as palavras-passe originais do seed.

Resultado completo: [`m3-s05-i100-20261101-resultado-newman-v01.txt`](m3-s05-i100-20261101-resultado-newman-v01.txt).

| | Executados | Falhados |
|---|---|---|
| Pedidos | 190 | 0 |
| Scripts de teste | 380 | 0 |
| Scripts de pré-pedido | 7 | 0 |
| Verificações (`pm.test`) | 611 | 0 |

Duração total: 21,2 s. O ficheiro de resultado não contém palavras-passe nem tokens.

### Execução de referência da I098 — 2026-10-30 (evidência do critério 6)

Execução com o Newman 6.2.1 a 2026-10-30, contra a API do ramo `develop` (commit `265cedf`) e uma base de dados nova, criada com `src/backend/scripts/reposicao-base-dados.ps1` (migrações da I062) e com os dados de demonstração da I061 (`executar-dados.ps1`: 28 verificações, 28 OK). As contas de demonstração usaram as palavras-passe originais do seed, as do ficheiro de credenciais da submissão.

Resultado completo: [`m3-s05-i098-20261030-resultado-newman-v01.txt`](m3-s05-i098-20261030-resultado-newman-v01.txt).

| | Executados | Falhados |
|---|---|---|
| Pedidos | 125 | 0 |
| Scripts de teste | 250 | 0 |
| Scripts de pré-pedido | 6 | 0 |
| Verificações (`pm.test`) | 403 | 0 |

Duração total: 12,7 s. O ficheiro de resultado não contém palavras-passe nem tokens.

### Primeira validação — 2026-10-26

Execução com o Newman 6.2.1 a 2026-10-26, contra a API do branch da I098 (que inclui a I066) e uma base de dados descartável criada com as migrações e os dados de demonstração da I061. Nessa base, e só nela, as palavras-passe das contas de demonstração foram substituídas por uma palavra-passe de teste, porque as verdadeiras não estão no repositório. O resultado foi o mesmo (125 pedidos e 403 verificações, sem falhas), também numa segunda execução seguida sobre a mesma base.

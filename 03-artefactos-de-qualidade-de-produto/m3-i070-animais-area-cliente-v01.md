# I070 — Consulta de animais na área do Cliente (EP-019, EP-020)

| Campo | Valor |
| --- | --- |
| Issue | I070 — Implementar a consulta de animais na área do Cliente (EP-019, EP-020) |
| Sprint | S04 |
| Milestone | M3 |
| Executor | André Filipe Moreira Mesquita |
| Revisor | Gonçalo Magalhães Ribeiro |
| Auditor | Gonçalo Dinis Magalhães Teixeira |
| Data | 2026-10-26 |
| Dependências | I069 (gestão de pacientes, #198) e I063 (autenticação e política de propriedade, #191), ambas integradas no `develop`. |

## Objetivo

Permitir ao Cliente, na aplicação móvel, consultar apenas os animais da sua conta:

- EP-019 — `GET /me/animais`;
- EP-020 — `GET /me/animais/{id}`.

## Implementação

- `MeusAnimaisController` herda de `ClienteAutenticadoControllerBase` (I063). A política `ClienteProprietario` só deixa entrar o perfil Cliente com `clienteId` no token, e o identificador do Cliente usado para filtrar é sempre o do token (`ClienteIdAutenticado`). Não existe nenhum parâmetro do pedido que o substitua (RNF002).
- `AnimaisService` (I069) ganha `ListarDoClienteAsync` e `ObterDoClienteAsync`, reutilizando o cálculo da idade e a leitura da raça.
- A lista vem ordenada por nome e, em caso de empate, por identificador (ordem estável). Um Cliente sem animais recebe `[]`.
- Um animal de outro Cliente devolve 403 sem nenhum dado desse animal. Um animal inexistente devolve 404, como prevê o contrato.
- DTOs `MeuAnimalListItemDto` e `MeuAnimalDto` (Modelo de Classes v02, secção 3.5): só dados administrativos, sem proprietário nem informação clínica.

## Divergências

- `IdadeAnos` é `int?` (vazio sem data de nascimento), como em `AnimalDto` da I069. O Modelo de Classes v02 tem `int`.

## Base de dados

Sem alterações ao esquema.

## Testes de integração

`ClinicaVeterinaria.IntegrationTests/Animais/MeusAnimaisEndpointsTests.cs` (9 testes, 12 execuções com o `Theory`):

| Teste | Critério |
| --- | --- |
| Lista só os animais do Cliente autenticado, por nome | 1, 3 |
| Outro Cliente vê apenas o seu animal | 1, 3 |
| `clienteId` no pedido é ignorado | 3 (isolamento pelo token) |
| Cliente sem animais recebe lista vazia | 1 |
| Consulta de um animal próprio, só com dados administrativos | 2 |
| Animal de outro Cliente devolve 403 sem dados do animal | 2, 4 |
| Animal inexistente devolve 404 | 2 |
| Perfis internos recebem 403 nos dois endpoints | 5, 6 |
| Sem token devolve 401 nos dois endpoints | 6 |

## Pedidos HTTP

`src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.http`, secção I070: um pedido válido e um pedido por cada erro previsto em cada operação, incluindo o acesso cruzado entre dois Clientes.

## Verificação manual

Os resultados da verificação manual do Executor (critério 7) estão no Pull Request da I070.

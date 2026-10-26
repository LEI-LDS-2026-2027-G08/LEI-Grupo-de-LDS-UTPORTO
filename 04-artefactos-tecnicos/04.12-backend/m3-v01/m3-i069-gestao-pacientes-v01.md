# I069 — Gestão de pacientes (EP-015 a EP-018)

| Campo | Valor |
| --- | --- |
| Issue | I069 — Implementar a gestão de pacientes (EP-015 a EP-018) |
| Sprint | S04 |
| Milestone | M3 |
| Executor | Gonçalo Dinis Magalhães Teixeira |
| Revisor | André Filipe Moreira Mesquita |
| Auditor | Ivan Cristiano Silva Mendes |
| Data | 2026-10-26 |
| Estado | Implementação do Executor concluída; aguarda pipeline, revisão e auditoria |

## Objetivo

Implementação da gestão administrativa de pacientes:

- EP-015 — GET /animais;
- EP-016 — GET /animais/{id};
- EP-017 — POST /animais;
- EP-018 — PUT /animais/{id}.

## Funcionalidades implementadas

A pesquisa de pacientes permite filtrar por proprietário, nome do animal, nome do proprietário, espécie e paginação.

A ficha do paciente devolve apenas informação administrativa e pode ser consultada por Rececionista, Veterinário e Auxiliar Veterinário.

O registo e atualização de pacientes validam:

- Cliente existente;
- peso superior a zero;
- data de nascimento não futura;
- espécie existente;
- raça compatível com a espécie.

Pedidos inválidos são rejeitados antes da persistência.

## Segurança e erros

Foram aplicados os perfis definidos no contrato.

São suportados os códigos:

- 400 — dados ou parâmetros inválidos;
- 401 — autenticação ausente ou inválida;
- 403 — perfil sem autorização;
- 404 — paciente ou Cliente inexistente.

O formato comum de erros da I058 é mantido.

## Base de dados

Não foram necessárias alterações ao esquema.

A entidade Paciente e as relações com Cliente, Especie e Raca já suportavam os requisitos da I069.

## Testes de integração

Foi criada:

`ClinicaVeterinaria.IntegrationTests/Animais/AnimaisEndpointsTests.cs`

Foram validados:

- pesquisa válida;
- permissões;
- consulta pelo Auxiliar Veterinário;
- paciente inexistente;
- criação válida;
- peso zero e negativo;
- data futura;
- Cliente inexistente;
- atualização válida;
- preservação do estado após atualização rejeitada.

Resultado específico da I069:

- total: 11
- failed: 0
- succeeded: 11
- skipped: 0

## Regressão global

Foi executado:

`dotnet test`

Resultado:

- total: 81
- failed: 0
- succeeded: 81
- skipped: 0

Build concluído com sucesso.

## Pedidos HTTP

O ficheiro:

`src/backend/ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.http`

inclui pedidos para EP-015 a EP-018 com cenários válidos e erros 400, 401, 403 e 404.

## Critérios de aceitação

| Critério | Estado |
| --- | --- |
| EP-015 | Cumprido |
| EP-016 | Cumprido |
| EP-017 | Cumprido |
| EP-018 | Cumprido |
| Apenas dados administrativos | Cumprido |
| Peso <= 0 rejeitado | Cumprido |
| Data futura rejeitada | Cumprido |
| Cliente inexistente rejeitado | Cumprido |
| Pesquisa por proprietário | Cumprido |
| Perfis e erros HTTP | Cumprido |
| Auxiliar sem dados clínicos | Cumprido |
| Testes I069 | 11/11 |
| Suite global | 81/81 |

## Estado antes da revisão

A implementação do Executor está concluída localmente.

Ficam pendentes commit, push, pipeline, revisão independente, auditoria e merge.

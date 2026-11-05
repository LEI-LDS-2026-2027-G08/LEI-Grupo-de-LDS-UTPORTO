# I062 — Comparação do esquema EF Core com os scripts da I060

## 1. Objetivo

Este documento regista a comparação entre o esquema produzido pelas migrations do Entity Framework Core e o esquema definido pelos scripts SQL da I060.

A comparação foi realizada sobre uma base de dados criada de raiz através do procedimento de reposição da I062.

Base utilizada no teste:

`ClinicaVeterinaria_I062_Test`

Tecnologia utilizada:

- .NET 10
- Entity Framework Core 10.0.13
- Microsoft SQL Server
- Collation `Latin1_General_100_CI_AS`

## 2. Resultado geral

O esquema produzido pelas migrations do EF Core está coerente com os scripts da I060 para todas as funcionalidades atualmente dentro do âmbito do projeto.

Não foram identificadas divergências funcionais ou estruturais não justificadas nas entidades atualmente implementadas.

## 3. Tabelas

O esquema EF Core contém 16 tabelas funcionais:

- Cliente
- Clinica
- ConfiguracaoSistema
- Especie
- Funcionario
- HorarioFuncionamento
- InformacaoApoio
- Marcacao
- Paciente
- Prescricao
- Raca
- RegistoClinico
- RegistoProfilaxia
- TokenRevogado
- Tratamento
- Utilizador

Os scripts da I060 contêm adicionalmente as seguintes tabelas:

- ContaCliente
- Pagamento
- HistoricoEstadoPagamento
- Alojamento
- Internamento
- HistoricoEstadoInternamento

Estas seis tabelas pertencem às funcionalidades F005 e F007, posteriormente retiradas do âmbito atual do projeto.

A sua ausência no modelo EF Core é, portanto, intencional e não constitui uma inconsistência da persistência.

## 4. Colunas e tipos

Para as 16 tabelas atualmente em âmbito foram comparados:

- nomes das colunas;
- tipos SQL;
- tamanhos máximos;
- precisão e escala;
- obrigatoriedade (`NULL` / `NOT NULL`).

Os elementos comparados encontram-se alinhados com os scripts da I060.

### 4.1. RowVersion da marcação

O modelo EF Core contém adicionalmente a coluna:

`Marcacao.RowVersion`

Esta coluna é criada como `rowversion` (`timestamp` no catálogo do SQL Server) e é utilizada para controlo de concorrência otimista.

A coluna não existe na versão `m3-v01` dos scripts da I060, constituindo uma evolução posterior da camada de persistência.

A diferença é intencional e deve ser mantida.

## 5. Chaves estrangeiras

Foram identificadas 20 chaves estrangeiras no esquema EF Core correspondente às funcionalidades em âmbito.

As relações e tabelas referenciadas coincidem com os scripts da I060.

Todas apresentam:

- `ON DELETE NO ACTION`;
- `ON UPDATE NO ACTION`.

Não foram identificadas diferenças funcionais.

## 6. Índices e restrições UNIQUE

Foram verificados 25 índices e elementos de unicidade nas tabelas atualmente em âmbito.

Os índices de pesquisa e as restrições de unicidade previstas na I060 encontram-se presentes no esquema EF Core.

Entre os principais elementos confirmados encontram-se:

- `UQ_Utilizador_Email`;
- `UQ_Cliente_NIF`;
- `UQ_Cliente_UtilizadorId`;
- `UQ_Especie_Nome`;
- `UQ_Raca_Especie_Id`;
- `UQ_Raca_Especie_Nome`;
- `UQ_RegistoClinico_Marcacao`;
- índices de pesquisa de Cliente e Paciente;
- índices associados a Marcações;
- índices associados aos Registos Clínicos e Profilaxia.

### 6.1. Representação de UNIQUE no EF Core

Alguns elementos definidos nos scripts da I060 através de:

`CONSTRAINT ... UNIQUE`

são materializados pelo Entity Framework Core como índices únicos.

Esta diferença é apenas de implementação física.

A propriedade de unicidade é preservada e não existe diferença funcional no comportamento esperado.

O índice `UQ_Cliente_NIF` mantém-se como índice único filtrado, permitindo valores `NULL` e impedindo a repetição de valores de NIF não nulos.

## 7. Restrições CHECK

Foram confirmadas 13 restrições `CHECK`, incluindo:

- linha única da Clinica;
- validade do dia e intervalo do HorarioFuncionamento;
- duração e estado da Marcacao;
- peso e sexo do Paciente;
- duração da Prescricao;
- coerência dos campos de atualização do RegistoClinico;
- tipo e intervalo da RegistoProfilaxia;
- estado e perfil do Utilizador.

As restrições apresentam comportamento equivalente ao definido nos scripts da I060.

A representação interna efetuada pelo SQL Server pode alterar a ordem das expressões ou transformar expressões `IN` em sequências de condições `OR`, sem alterar a semântica.

## 8. Valores por omissão

Foram confirmados 13 valores por omissão.

Entre os valores verificados encontram-se:

- `SYSUTCDATETIME()`;
- estado `Ativo`;
- estado de marcação `Pendente`;
- sexo `Desconhecido`;
- valores booleanos de `Ativo` e `PodeRegistarApoio`.

Alguns valores booleanos aparecem no catálogo SQL Server através de expressões como:

`CONVERT(bit, 0)`

ou:

`CONVERT(bit, 1)`

em vez da representação textual utilizada nos scripts SQL.

Esta diferença é apenas sintática e não altera o resultado.

## 9. Dados de referência

A migration `I062DadosReferencia` adiciona os dados mínimos de referência necessários ao funcionamento da aplicação.

São criados:

- 1 registo de Clinica;
- 1 configuração `DuracaoPadraoConsultaMinutos`;
- 2 espécies;
- 5 raças.

Espécies:

- Cão
- Gato

Raças:

- Labrador Retriever
- Pastor Alemão
- Sem raça definida
- Europeu Comum
- Siamês

## 10. Reposição da base de dados

O procedimento `reposicao-base-dados.ps1` foi executado sobre uma base de testes isolada.

O procedimento:

1. compila a solução;
2. elimina a base de dados existente;
3. cria novamente a base;
4. aplica todas as migrations;
5. aplica os dados de referência;
6. verifica a existência de alterações do modelo ainda sem migration.

Foi realizada uma reposição completa com sucesso na máquina do Executor.

Após a reposição foram obtidos:

- Espécies: 2
- Raças: 5
- Clínicas: 1
- Configurações: 1

Foi igualmente confirmado:

`No changes have been made to the model since the last migration.`

## 11. Conclusão

A comparação demonstra que as migrations do Entity Framework Core produzem um esquema coerente com os scripts da I060 para todas as funcionalidades atualmente em âmbito.

As diferenças identificadas são justificadas:

1. ausência das tabelas relativas a F005 e F007, funcionalidades retiradas do âmbito atual;
2. existência de `Marcacao.RowVersion` para controlo de concorrência otimista;
3. representação de algumas restrições `UNIQUE` através de índices únicos pelo Entity Framework Core;
4. diferenças puramente sintáticas na representação interna de `CHECK` e `DEFAULT` pelo SQL Server.

Não foram identificadas divergências de esquema não justificadas nas funcionalidades atualmente implementadas.

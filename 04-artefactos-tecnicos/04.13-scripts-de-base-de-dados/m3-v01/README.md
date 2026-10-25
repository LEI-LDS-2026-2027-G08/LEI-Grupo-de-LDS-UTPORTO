# Scripts de criação do esquema da base de dados `clinica-veterinaria` — m3-v01

| Campo | Valor |
|---|---|
| Projeto | Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027 |
| Issue | I060 — Escrever o script de criação do esquema da base de dados |
| Milestone / Sprint | M3 / Sprint 03 |
| Executor / Revisor / Auditor | Gonçalo Magalhães Ribeiro / Ivan Cristiano Silva Mendes / Gonçalo Dinis Magalhães Teixeira (Sprint Backlog S03 v01) |
| Versão | m3-v01 (`/projeto/04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v01`) |
| SGBD alvo | Microsoft SQL Server 2025 (Regulamento Interno G08, secção 11.2) |
| Base de dados | `ClinicaVeterinaria`, identificador físico de `clinica-veterinaria` (Modelo de Dados, secção 6.1) |
| Fonte | Modelo de Dados v01 — I016 (`04.06-modelos-de-dados/m2-modelo-de-dados-clinica-veterinaria-v01.md`) |

## 1. Âmbito

Os scripts criam, numa instância SQL Server vazia, o esquema físico completo do Modelo de Dados aprovado:

- 22 tabelas e 142 colunas, com os nomes, tipos, nulidade e ordem do dicionário de dados (secção 7);
- 22 chaves primárias e 36 chaves estrangeiras (secção 5.2), todas com `ON DELETE NO ACTION` e `ON UPDATE NO ACTION` (D13);
- 7 restrições `UNIQUE` e 3 índices únicos filtrados (secção 6.4), incluindo a chave estrangeira composta `Paciente → Raca` (D05);
- 23 restrições `CHECK` (secção 6.3);
- 21 valores por omissão (dicionário de dados e secção 6.7);
- a coluna calculada e persistida `RegistoProfilaxia.DataProximaAdministracao` (secção 6.6, RF045);
- 34 índices não únicos: os 13 da secção 6.5, 19 de chaves estrangeiras e 2 de pesquisa (ver secção 8).

Ficam **fora do âmbito** da I060, por pertencerem a outras Issues ou ao backend:

| Fora do âmbito | Onde é tratado |
|---|---|
| Dados iniciais, de referência e de demonstração (`Especie`, `Raca`, `Alojamento`, `Clinica`, `ConfiguracaoSistema`, contas de teste) | I061 |
| Migrações do EF Core e procedimento de reposição da base de dados por ambiente | I062 |
| Camada de persistência EF Core (`DbContext`, mapeamentos) | I059 |
| Regras dependentes do perfil, de somas ou de sobreposições (secção 6.8) | Backend, por decisão D14; não são implementadas com triggers |
| Logins, utilizadores e permissões do SQL Server | Configuração de cada ambiente; nenhum script cria credenciais |

## 2. Conteúdo da pasta

| Ordem | Ficheiro | Finalidade |
|:---:|---|---|
| 1 | `00-criar-base-de-dados.sql` | Cria a base de dados `ClinicaVeterinaria`. |
| 2 | `01-acesso-e-configuracao.sql` | `Utilizador`, `TokenRevogado`, `Funcionario`, `Clinica`, `ConfiguracaoSistema` (F001). |
| 3 | `02-clientes-e-pacientes.sql` | `Cliente`, `Especie`, `Raca`, `Paciente` e índice único filtrado `UQ_Cliente_NIF` (F001/F002). |
| 4 | `03-agenda.sql` | `HorarioFuncionamento`, `Marcacao`, `InformacaoApoio` (F003). |
| 5 | `04-registo-clinico.sql` | `RegistoClinico`, `Tratamento`, `Prescricao` (F004). |
| 6 | `05-vacinacao-e-desparasitacao.sql` | `RegistoProfilaxia`, com a coluna calculada persistida (F006). |
| 7 | `06-contas-e-pagamentos.sql` | `ContaCliente`, `Pagamento`, `HistoricoEstadoPagamento` (F005). |
| 8 | `07-internamento.sql` | `Alojamento`, `Internamento`, `HistoricoEstadoInternamento` e índices únicos filtrados (F007). |
| 9 | `08-indices.sql` | Índices não únicos de desempenho, de chaves estrangeiras e de pesquisa. |
| — | `executar-scripts.ps1` | Execução automática por ordem (Windows), com registo. |
| — | `executar-scripts.sh` | Execução automática por ordem (Linux, macOS e pipeline GitHub Actions), com registo. |
| — | `verificacao/verificar-esquema.sql` | Compara o catálogo da base de dados com o Modelo de Dados (só leitura). |
| — | `verificacao/testar-restricoes.sql` | Testa as restrições com inserções válidas e inválidas, numa transação anulada no fim. |

A ordem dos ficheiros respeita as dependências entre tabelas: cada tabela só é criada depois das tabelas que referencia. Os ficheiros estão em UTF-8.

## 3. Pré-requisitos

- Instância Microsoft SQL Server 2025 local ou exclusiva do aluno (Regulamento da UC, secção 20.2). Os scripts usam apenas funcionalidades disponíveis desde o SQL Server 2017.
- `sqlcmd` (SQL Server Command Line Utilities 18 ou `go-sqlcmd`) para a execução automática, ou SQL Server Management Studio para a execução manual.
- Login com permissão `CREATE DATABASE`, ou base de dados `ClinicaVeterinaria` já criada e vazia com permissão de criação de objetos.

## 4. Execução

### 4.1. Execução automática (recomendada)

Windows, com autenticação do Windows:

```powershell
.\executar-scripts.ps1 -Servidor localhost -Verificar -Testar
```

Windows, com login SQL (a palavra-passe é lida da variável de ambiente e nunca fica em ficheiros):

```powershell
$env:SQLCMDPASSWORD = Read-Host 'Palavra-passe'
.\executar-scripts.ps1 -Servidor 'localhost,1433' -Utilizador sa -ConfiarCertificado -Verificar -Testar
```

Linux, macOS ou pipeline:

```bash
export SQLCMD_SERVIDOR=localhost SQLCMD_UTILIZADOR=sa
read -rs SQLCMDPASSWORD && export SQLCMDPASSWORD
./executar-scripts.sh --confiar-certificado --verificar --testar
```

Os executores correm os scripts por ordem numérica com `sqlcmd -b -I -f 65001`, param no primeiro erro com código de saída diferente de zero e gravam tudo em `i060-execucao-AAAAMMDD-HHMMSS.log` na pasta atual (ou na pasta indicada em `-PastaRegistos` / `--pasta-registos`). A opção `-ConfiarCertificado` / `--confiar-certificado` só é necessária com o `sqlcmd` 18 numa instância com certificado autoassinado.

### 4.2. Execução manual no SSMS

Abrir e executar (F5) os ficheiros `00` a `08` por ordem, confirmando no separador *Messages* que cada um termina sem erros. A seguir, executar `verificacao/verificar-esquema.sql` e, se se pretender, `verificacao/testar-restricoes.sql`.

### 4.3. Reexecução

Antes de criar cada objeto, os scripts verificam se ele já existe (`DB_ID`, `OBJECT_ID` e `sys.indexes`). Uma segunda execução não falha nem duplica objetos e regista, para cada um, a mensagem «já existe — sem alterações». Os scripts não alteram objetos já existentes: para recriar o esquema a partir do zero, apaga-se a base de dados local do próprio aluno e voltam a executar-se os scripts. O procedimento de reposição com dados é tratado na I062.

## 5. Verificação

### 5.1. Verificação do esquema (`verificar-esquema.sql`)

Este script só lê o catálogo do SQL Server. Compara:

- as 22 tabelas;
- as 142 colunas (ordem, tipo, nulidade e coluna calculada persistida), a partir de uma lista extraída do dicionário de dados do Modelo de Dados e não dos scripts de criação;
- as 109 restrições por nome e tipo (PK, FK, UNIQUE, CHECK, DEFAULT);
- os 37 índices (tabela, unicidade, filtro e colunas por ordem);
- que todas as FKs têm `NO ACTION` e que nenhuma FK ou CHECK está desativada ou não fidedigna.

Assinala também tabelas, colunas, restrições ou índices não previstos. Termina com erro se existir alguma divergência e com «o esquema corresponde integralmente ao previsto» em caso contrário.

### 5.2. Testes de restrições (`testar-restricoes.sql`)

O script corre numa base de dados acabada de criar e recusa-se a correr se encontrar dados. Tudo decorre numa única transação, anulada no fim, e os contadores `IDENTITY` são repostos, de forma que o primeiro registo real de cada tabela continua a receber `Id = 1`. Os dados são fictícios (domínio reservado `.invalid`).

| Grupo | Testes | O que confirma |
|---|---|---|
| Inserções válidas | V01, P08–P11 | Um registo válido em cada uma das 22 tabelas; NIF opcional repetido como `NULL`; raça opcional; conta paga e encerrada; novo internamento depois da alta. |
| Valores por omissão e coluna calculada | P01–P07 | `Ativo`, `Desconhecido`, `Pendente`, datas UTC; `DataProximaAdministracao = DataAdministracao + IntervaloDias` e `NULL` sem intervalo. |
| `UNIQUE` | N01, N06, N09, N10, N12, N13, N23, N35, N39, N40 | E-mail de utilizador, `jti`, conta do Cliente, NIF, espécie, raça por espécie, um registo clínico por consulta, código de alojamento, um internamento ativo por paciente e por alojamento. |
| `CHECK` (23 de 23) | N02, N03, N08, N15–N20, N24–N30, N32–N34, N36–N38, N41 | Todas as restrições da secção 6.3, incluindo os valores positivos de peso, valores monetários, duração e intervalo em dias. |
| Chaves estrangeiras | N05, N07, N11, N14, N21, N22, N31 | Referências inexistentes, Veterinário que não é funcionário, raça de outra espécie. |
| `NOT NULL`, `NO ACTION`, coluna calculada | N04, N42, N43, N44 | Coluna obrigatória; impossibilidade de apagar dados referenciados (D13); escrita manual da coluna calculada recusada. |

O resultado lista cada teste com a mensagem obtida. O script termina com erro se algum teste falhar.

### 5.3. Evidências a anexar ao Merge Request

1. Numa instância sem a base de dados `ClinicaVeterinaria`, executar `executar-scripts.ps1 -Verificar -Testar` (ou o equivalente `.sh`). O registo resultante prova a execução sem erros, a correspondência com o modelo e o comportamento das restrições.
2. Sem apagar nada, executar novamente `executar-scripts.ps1 -Verificar`. O segundo registo prova a reexecução: todas as mensagens «já existe — sem alterações» e a verificação novamente sem divergências.
3. O quadro de correspondência da secção 7 deste README.

## 6. Convenções aplicadas

As convenções são as da secção 6.1 do Modelo de Dados. Quando as secções 6.3 a 6.5 indicam um nome explícito, é esse o nome usado.

| Elemento | Convenção | Exemplo |
|---|---|---|
| Tabelas e colunas | PascalCase, singular, sem acentos | `RegistoProfilaxia.DataAdministracao` |
| Chave primária | `PK_<Tabela>` | `PK_Marcacao` |
| Chave estrangeira | `FK_<Tabela>_<Referência>_<Coluna>` | `FK_Marcacao_Funcionario_VeterinarioId` |
| Única | `UQ_<Tabela>_<Coluna>` ou nome da secção 6.4 | `UQ_Utilizador_Email`, `UQ_Internamento_PacienteAtivo` |
| Domínio | nome da secção 6.3 | `CK_Conta_Fecho` |
| Índice | `IX_<Tabela>_<Colunas>` ou nome da secção 6.5 | `IX_Marcacao_Veterinario_Data` |
| Valor por omissão | `DF_<Tabela>_<Coluna>` (ver DI-03) | `DF_Marcacao_Estado` |
| Datas e horas | `DATETIME2(0)` em UTC com `SYSUTCDATETIME()`; `DATE`; `TIME(0)` | — |
| Texto e valores | `NVARCHAR(n)`; `DECIMAL(10,2)` em euros; enumerados como texto com `CHECK` (D16) | — |

## 7. Quadro de correspondência Modelo de Dados ↔ esquema criado

Quadro produzido por comparação automática entre o Modelo de Dados v01 (dicionário de dados, secções 5.2, 6.3, 6.4 e 6.5) e as definições dos scripts. Os nomes, tipos, nulidade, ordem das colunas, chaves, valores por omissão e regras `CHECK` coincidem em todas as tabelas. As únicas diferenças de nome e os elementos complementares estão registados na secção 8.

| Tabela do modelo (secção) | Tabela criada | Script | Colunas (modelo/criadas) | PK | FK | UNIQUE | CHECK | DEFAULT | Índices | Correspondência |
|---|---|---|---|---|---|---|---|---|---|---|
| Utilizador (7.1) | dbo.Utilizador | 01-acesso-e-configuracao.sql | 7/7 | PK_Utilizador | 0 | 1 | 2 | 2 | 0 | Coincide |
| TokenRevogado (7.2) | dbo.TokenRevogado | 01-acesso-e-configuracao.sql | 4/4 | PK_TokenRevogado | 1 | 0 | 0 | 1 | 1 | Coincide (DI-01) |
| Funcionario (7.3) | dbo.Funcionario | 01-acesso-e-configuracao.sql | 4/4 | PK_Funcionario | 1 | 0 | 0 | 1 | 0 | Coincide |
| Clinica (7.4) | dbo.Clinica | 01-acesso-e-configuracao.sql | 7/7 | PK_Clinica | 0 | 0 | 1 | 0 | 0 | Coincide |
| ConfiguracaoSistema (7.5) | dbo.ConfiguracaoSistema | 01-acesso-e-configuracao.sql | 4/4 | PK_ConfiguracaoSistema | 0 | 0 | 0 | 0 | 0 | Coincide |
| Cliente (7.6) | dbo.Cliente | 02-clientes-e-pacientes.sql | 7/7 | PK_Cliente | 1 | 2 | 0 | 1 | 1 | Coincide (DI-05, DI-06) |
| Especie (7.7) | dbo.Especie | 02-clientes-e-pacientes.sql | 2/2 | PK_Especie | 0 | 1 | 0 | 0 | 0 | Coincide |
| Raca (7.8) | dbo.Raca | 02-clientes-e-pacientes.sql | 3/3 | PK_Raca | 1 | 2 | 0 | 0 | 0 | Coincide |
| Paciente (7.9) | dbo.Paciente | 02-clientes-e-pacientes.sql | 10/10 | PK_Paciente | 3 | 0 | 2 | 2 | 3 | Coincide (DI-05) |
| HorarioFuncionamento (7.10) | dbo.HorarioFuncionamento | 03-agenda.sql | 5/5 | PK_HorarioFuncionamento | 0 | 0 | 2 | 1 | 0 | Coincide |
| Marcacao (7.11) | dbo.Marcacao | 03-agenda.sql | 11/11 | PK_Marcacao | 3 | 0 | 2 | 2 | 4 | Coincide |
| InformacaoApoio (7.12) | dbo.InformacaoApoio | 03-agenda.sql | 5/5 | PK_InformacaoApoio | 2 | 0 | 0 | 1 | 2 | Coincide |
| RegistoClinico (7.13) | dbo.RegistoClinico | 04-registo-clinico.sql | 8/8 | PK_RegistoClinico | 3 | 1 | 1 | 1 | 2 | Coincide |
| Tratamento (7.14) | dbo.Tratamento | 04-registo-clinico.sql | 4/4 | PK_Tratamento | 1 | 0 | 0 | 0 | 1 | Coincide |
| Prescricao (7.15) | dbo.Prescricao | 04-registo-clinico.sql | 6/6 | PK_Prescricao | 1 | 0 | 1 | 0 | 1 | Coincide |
| RegistoProfilaxia (7.16) | dbo.RegistoProfilaxia | 05-vacinacao-e-desparasitacao.sql | 11/11 | PK_RegistoProfilaxia | 3 | 0 | 2 | 1 | 3 | Coincide |
| ContaCliente (7.17) | dbo.ContaCliente | 06-contas-e-pagamentos.sql | 12/12 | PK_ContaCliente | 6 | 0 | 3 | 2 | 6 | Coincide |
| Pagamento (7.18) | dbo.Pagamento | 06-contas-e-pagamentos.sql | 6/6 | PK_Pagamento | 2 | 0 | 2 | 1 | 2 | Coincide |
| HistoricoEstadoPagamento (7.19) | dbo.HistoricoEstadoPagamento | 06-contas-e-pagamentos.sql | 7/7 | PK_HistoricoEstadoPagamento | 2 | 0 | 1 | 1 | 2 | Coincide |
| Alojamento (7.20) | dbo.Alojamento | 07-internamento.sql | 4/4 | PK_Alojamento | 0 | 1 | 0 | 1 | 0 | Coincide |
| Internamento (7.21) | dbo.Internamento | 07-internamento.sql | 9/9 | PK_Internamento | 4 | 2 | 3 | 2 | 4 | Coincide |
| HistoricoEstadoInternamento (7.22) | dbo.HistoricoEstadoInternamento | 07-internamento.sql | 6/6 | PK_HistoricoEstadoInternamento | 2 | 0 | 1 | 1 | 2 | Coincide (DI-02) |
| **Total** | **22** | — | **142/142** | **22** | **36** | **10** | **23** | **21** | **34** | |

Nota sobre as cardinalidades (secção 4.3):

- **1 : 0..1** — garantida pela PK = FK em `Funcionario` e pelas restrições `UQ_Cliente_UtilizadorId` e `UQ_RegistoClinico_Marcacao`.
- **1 : 0..N e 0..1 : 0..N** — garantidas pelas FKs obrigatórias (`NOT NULL`) ou opcionais (`NULL`).
- **1 : 1..N (Internamento — HistoricoEstadoInternamento)** — o «pelo menos um» não é exprimível por restrição declarativa. É garantido pelo backend, que cria o internamento e a primeira linha de histórico na mesma transação (secções 5.3 e 6.8).

## 8. Decisões de implementação e divergências face ao Modelo de Dados

Registadas nos termos da Definition of Done (divergências face ao Modelo de Dados registadas e decididas). As decisões DI-01 a DI-05 precisam de confirmação na revisão. As observações DI-06 a DI-10 não alteram o esquema.

| N.º | Situação no Modelo de Dados v01 | Decisão nos scripts | Justificação | Proposta |
|---|---|---|---|---|
| DI-01 | A secção 6.4 chama `PK_TokenRevogado_Jti` à chave primária de `TokenRevogado`; a secção 6.1 define `PK_<Tabela>`. | `PK_TokenRevogado`. | Cumpre a convenção geral, que todas as outras 21 tabelas seguem e que é também o nome por omissão do EF Core. | Corrigir a secção 6.4 no Modelo de Dados v02. |
| DI-02 | A secção 6.3 aplica `CK_Internamento_Estado` a `Internamento` e a `HistoricoEstadoInternamento`. | `CK_Internamento_Estado` em `Internamento` e `CK_HistoricoInternamento_Estado` no histórico, com a mesma regra. | O SQL Server exige nomes de restrições únicos no esquema. O nome segue a abreviatura já usada no modelo (`CK_HistoricoPagamento_Estados`, `IX_Historico_Internamento`). | Registar o nome no Modelo de Dados v02. |
| DI-03 | Não há convenção de nomes para os valores por omissão. | `DF_<Tabela>_<Coluna>` nos 21 valores por omissão. | Sem nome explícito, o SQL Server gera nomes aleatórios (`DF__Utiliza__Estad__3B75D760`), diferentes em cada ambiente, o que dificulta a verificação e a comparação com as migrações. | Acrescentar a convenção à secção 6.1. |
| DI-04 | A nota da secção 6.5 diz que as FKs sem índice próprio são indexadas automaticamente pelo EF Core. | Os scripts criam esses 19 índices explicitamente, com o nome que o EF Core usa (`IX_<Tabela>_<Coluna>`). `Paciente` recebe um único índice `(EspecieId, RacaId)` que cobre as duas FKs. `Internamento.PacienteId` e `Internamento.AlojamentoId` têm índice próprio não filtrado. | O esquema criado pelos scripts fica completo sem depender do EF Core. Os índices únicos filtrados de `Internamento` só cobrem internamentos ativos. | Confirmar na I062 que as migrações geram os mesmos índices; se o EF Core não gerar os dois de `Internamento`, configurá-los com `HasIndex` ou decidir a diferença. |
| DI-05 | A secção 6.5 não tem índices para a pesquisa de clientes e animais por nome. A descrição da I060 pede «índices de pesquisa (clientes, animais, consultas por veterinário e data, contas por cliente)». | Acrescentados `IX_Cliente_Nome` e `IX_Paciente_Nome`. | A pesquisa por NIF e por e-mail já é suportada por `UQ_Cliente_NIF` e `UQ_Utilizador_Email`, a de animais por proprietário por `IX_Paciente_ClienteId`, a de consultas por `IX_Marcacao_Veterinario_Data` e a de contas por `IX_Conta_Cliente_Estado`. Faltava a pesquisa por nome (EP-011 e EP-015). | Revisor decide: incluir no Modelo de Dados v02 ou remover. |
| DI-06 | O critério de aceitação 3 da I060 pede `UNIQUE` para o «e-mail de cliente». | Garantido por `UQ_Utilizador_Email`. | O e-mail do Cliente é `Utilizador.Email` (nota da secção 7.6 e D02); não existe coluna `Cliente.Email`. | Nenhuma alteração ao esquema. A mesma formulação existe na I059. |
| DI-07 | A secção 6.1 indica a criação do esquema por migrações do EF Core, a entregar em `04.13`. | A I060 entrega scripts T-SQL; as migrações coerentes com estes scripts são a I062. | Divisão aprovada no Backlog Refinement de 12/10/2026 (I026 dividida em I060, I061 e I062). | Atualizar a secção 6.1 no Modelo de Dados v02. |
| DI-08 | Não é definida a collation da base de dados. | Usa a collation por omissão da instância. | As instalações por omissão do SQL Server usam uma collation insensível a maiúsculas (`*_CI_AS`), pelo que `UQ_Utilizador_Email` não distingue `A@x.pt` de `a@x.pt`. | Decidir se a collation passa a ser fixada; o backend deve normalizar os e-mails em minúsculas. |
| DI-09 | O UC12 refere a associação do registo clínico à consulta «quando aplicável». | `RegistoClinico.MarcacaoId` é `NOT NULL`. | Segue o Modelo de Dados (D07) e a Documentação da API (EP-031 exige `consultaId`). A incoerência está na especificação do UC12 e já consta das pendências do README M2. | Corrigir o UC12. |
| DI-10 | — | Sem alteração ao modelo. | O modelo não liga `Internamento.AltaRegistadaPorId` a `DataAlta` por `CHECK` (ao contrário de `RegistoClinico` e `ContaCliente`). Também não tem valor por omissão para `Clinica.DataUltimaAlteracao` nem para `ConfiguracaoSistema.DataUltimaAlteracao`, que terão de ser indicados nos dados iniciais (I061). | Avaliar no Modelo de Dados v02. |

## 9. Critérios de aceitação da I060

| N.º | Critério (Backlog de Projeto m3 v01) | Como é cumprido | Evidência |
|:---:|---|---|---|
| 1 | Os scripts correm do início ao fim numa instância vazia, sem erros, por ordem numérica. | Ficheiros `00` a `08` ordenados pelas dependências; executores param no primeiro erro. | Registo da 1.ª execução (secção 5.3). |
| 2 | Tabelas, colunas, chaves, relações e cardinalidades coincidem com o Modelo de Dados em nomes e tipos. | 22/22 tabelas, 142/142 colunas, 36/36 FKs, 23/23 `CHECK`. | Quadro da secção 7; `verificar-esquema.sql` sem divergências. |
| 3 | `UNIQUE` para e-mail de utilizador, NIF e e-mail de cliente; `CHECK` para valores positivos (peso, valores monetários, intervalo em dias). | `UQ_Utilizador_Email`, `UQ_Cliente_NIF` (filtrado); e-mail do Cliente: DI-06. `CK_Paciente_Peso`, `CK_Conta_Valor`, `CK_Pagamento_Valor`, `CK_Profilaxia_Intervalo`. | Testes N01, N10, N15, N27, N28, N32 de `testar-restricoes.sql`. |
| 4 | Scripts reexecutáveis: uma segunda execução não falha nem duplica objetos. | Verificação de existência antes de cada `CREATE`. | Registo da 2.ª execução com «já existe — sem alterações» e verificação sem divergências. |
| 5 | Cabeçalho com finalidade, ordem e pré-requisitos; sem dados pessoais reais nem palavras-passe. | Cabeçalho em todos os ficheiros. Os scripts `00` a `08` não contêm `INSERT`, logins nem credenciais; os testes usam dados fictícios e anulam a transação. | Leitura dos ficheiros; pesquisa no histórico Git. |

## 10. Histórico de versões

| Versão | Data | Autor | Alteração |
|---|---|---|---|
| m3-v01 | 2026-10-16 | Gonçalo Magalhães Ribeiro | Versão inicial: criação do esquema completo do Modelo de Dados v01, execução automática, verificação do esquema e testes de restrições. |

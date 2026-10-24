# Scripts de criação do esquema da base de dados `clinica-veterinaria` — m3-v02 (rascunho)

| Campo | Valor |
|---|---|
| Projeto | Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027 |
| Issue | Por atribuir. A versão m3-v01 destes scripts é a I060 (concluída) e não é alterada por esta versão. |
| Milestone / Sprint | M3 / Sprint 04 (rascunho) |
| Executor / Revisor / Auditor | Por atribuir |
| Versão | m3-v02 (`/projeto/04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v02`) |
| Estado | **Rascunho. Executado a 24/10/2026 num SQL Server 2025, sem erros nem divergências** (ver secção 3); falta a revisão |
| SGBD alvo | Microsoft SQL Server 2025 (Regulamento Interno G08) |
| Base de dados | `ClinicaVeterinaria`, identificador físico de `clinica-veterinaria` (Modelo de Dados, secção 6.1) |
| Fonte | Modelo de Dados v02 — rascunho (`04.06-modelos-de-dados/m3-modelo-de-dados-clinica-veterinaria-v02.md`) |

## 1. O que muda em relação à m3-v01

A m3-v02 nasce de duas decisões do Líder do grupo, tomadas a 24/10/2026 (mais uma proposta ainda por confirmar, a collation, na última linha da tabela), depois da homologação do Plano de Segurança v05 e da Documentação da Arquitetura v05 (21/10/2026), que passaram F005 e F007 para o não-âmbito:

| Decisão | O que muda nos scripts |
|---|---|
| Retirar as tabelas de F005 e F007 numa versão nova (Modelo de Dados v02, ponto 9.1, opção B; DI-11) | Saem `ContaCliente`, `Pagamento`, `HistoricoEstadoPagamento`, `Alojamento`, `Internamento` e `HistoricoEstadoInternamento`. Os scripts `06-contas-e-pagamentos.sql` e `07-internamento.sql` deixam de existir e `08-indices.sql` passa a `06-indices.sql`. São 16 tabelas em vez de 22. |
| Acrescentar a coluna `RowVersion` a `Marcacao` (Modelo de Dados v02, ponto 9.2, opção B2; DI-12) | `Marcacao` passa a ter 12 colunas, a última `RowVersion ROWVERSION NOT NULL`. Nenhuma outra tabela a tem. |
| Fixar a collation da base de dados (proposta a confirmar pelo Revisor; Modelo de Dados v02, ponto 9.4; DI-08) | `00-criar-base-de-dados.sql` cria a base com `COLLATE Latin1_General_100_CI_AS` e `verificar-esquema.sql` confirma-a. Não muda tabelas, colunas, restrições nem índices. |

Os scripts `00` a `05` são os da m3-v01 com o cabeçalho atualizado; `03-agenda.sql` acrescenta a coluna e `00-criar-base-de-dados.sql` acrescenta a collation. Os ficheiros de verificação e os executores foram ajustados às 16 tabelas. A pasta `m3-v01` fica como estava.

## 2. Âmbito

Os scripts criam, numa instância SQL Server vazia, o esquema físico do Modelo de Dados v02:

- 16 tabelas e 99 colunas, com os nomes, tipos, nulidade e ordem do dicionário de dados (secção 7);
- 16 chaves primárias e 20 chaves estrangeiras (secção 5.2), todas com `ON DELETE NO ACTION` e `ON UPDATE NO ACTION` (D13);
- 6 restrições `UNIQUE` e 1 índice único filtrado (`UQ_Cliente_NIF`), incluindo a chave estrangeira composta `Paciente → Raca` (D05);
- 13 restrições `CHECK` (secção 6.3);
- 13 valores por omissão (dicionário de dados e secção 6.7);
- a coluna calculada e persistida `RegistoProfilaxia.DataProximaAdministracao` (secção 6.6, RF045);
- a coluna `Marcacao.RowVersion`, token de concorrência otimista (DI-12);
- 18 índices não únicos: os 8 da secção 6.5, 8 de chaves estrangeiras e 2 de pesquisa (ver secção 8).

Ficam **fora do âmbito**, por pertencerem a outras Issues ou ao backend:

| Fora do âmbito | Onde é tratado |
|---|---|
| Tabelas de F005 (contas e pagamentos) e F007 (internamento e alojamentos) | Fora do âmbito do projeto desde o PS v05. Só existem na m3-v01 (DI-11). |
| Dados iniciais, de referência e de demonstração (`Especie`, `Raca`, `Clinica`, `ConfiguracaoSistema`, contas de teste) | I061. Os dados de `Alojamento` ficam sem destino (DI-11). |
| Migrações do EF Core e procedimento de reposição da base de dados por ambiente | I062 |
| Camada de persistência EF Core (`DbContext`, mapeamentos) | I059 |
| Regras dependentes do perfil, de somas ou de sobreposições (secção 6.8) | Backend, por decisão D14; não são implementadas com triggers |
| Logins, utilizadores e permissões do SQL Server | Configuração de cada ambiente; nenhum script cria credenciais |

## 3. Estado da verificação

**Executada a 24/10/2026 num SQL Server 2025 (RTM, 17.0.1000.7), sem erros nem divergências.** Os dois registos estão em `esquema-execucao-20261024-232702.log` e `esquema-execucao-20261024-232732.log`:

1. **23:27:02, base de dados inexistente:** `00` a `06` criam a base com a collation `Latin1_General_100_CI_AS`, as 16 tabelas e os 19 índices; `verificar-esquema.sql` dá **237 verificações e 0 divergências** (16 tabelas, 99 colunas, 68 restrições, 20 regras de FK, 13 regras de CHECK, 19 índices, 1 filtro de índice e a collation); `testar-restricoes.sql` dá **41 testes executados, 41 OK e 0 falhados**.
2. **23:27:32, repetição sem apagar nada:** todas as mensagens são «já existe — sem alterações» e `verificar-esquema.sql` volta a dar 237 verificações e 0 divergências.

Antes da execução, esta versão foi derivada da m3-v01 e verificada também por leitura e por programa:

- as contagens deste README (16 tabelas, 99 colunas, 68 restrições, 19 índices, 41 testes) foram recalculadas a partir dos próprios ficheiros e coincidem com as do registo de execução;
- os nomes das colunas e das restrições definidas em `01` a `05` foram comparados por programa com as listas de `verificar-esquema.sql`, sem divergências nas 16 tabelas;
- os ficheiros `00` a `05` diferem dos da m3-v01 apenas no cabeçalho e, em `03-agenda.sql`, na coluna `RowVersion` e, em `00-criar-base-de-dados.sql`, na cláusula `COLLATE` (DI-08).

Falta a revisão (Revisor) e o registo das decisões na Issue I059. A execução prova o esquema e as restrições; não prova que o modelo do Entity Framework Core coincide com ele, que é o critério 4 da I059.

## 4. Conteúdo da pasta

| Ordem | Ficheiro | Finalidade |
|:---:|---|---|
| 1 | `00-criar-base-de-dados.sql` | Cria a base de dados `ClinicaVeterinaria` com a collation `Latin1_General_100_CI_AS`. |
| 2 | `01-acesso-e-configuracao.sql` | `Utilizador`, `TokenRevogado`, `Funcionario`, `Clinica`, `ConfiguracaoSistema` (F001). |
| 3 | `02-clientes-e-pacientes.sql` | `Cliente`, `Especie`, `Raca`, `Paciente` e índice único filtrado `UQ_Cliente_NIF` (F001/F002). |
| 4 | `03-agenda.sql` | `HorarioFuncionamento`, `Marcacao` (com `RowVersion`), `InformacaoApoio` (F003). |
| 5 | `04-registo-clinico.sql` | `RegistoClinico`, `Tratamento`, `Prescricao` (F004). |
| 6 | `05-vacinacao-e-desparasitacao.sql` | `RegistoProfilaxia`, com a coluna calculada persistida (F006). |
| 7 | `06-indices.sql` | Índices não únicos de desempenho, de chaves estrangeiras e de pesquisa. |
| — | `executar-scripts.ps1` | Execução automática por ordem (Windows), com registo. |
| — | `executar-scripts.sh` | Execução automática por ordem (Linux, macOS e pipeline GitHub Actions), com registo. |
| — | `verificacao/verificar-esquema.sql` | Compara o catálogo da base de dados com o Modelo de Dados (só leitura). |
| — | `verificacao/testar-restricoes.sql` | Testa as restrições com inserções válidas e inválidas, numa transação anulada no fim. |

A ordem dos ficheiros respeita as dependências entre tabelas: cada tabela só é criada depois das tabelas que referencia. Os ficheiros estão em UTF-8.

## 5. Pré-requisitos e execução

### 5.1. Pré-requisitos

- Instância Microsoft SQL Server 2025 local ou exclusiva do aluno (Regulamento da UC, secção 20.2). Os scripts usam apenas funcionalidades disponíveis desde o SQL Server 2017.
- `sqlcmd` (SQL Server Command Line Utilities 18 ou `go-sqlcmd`) para a execução automática, ou SQL Server Management Studio para a execução manual.
- Login com permissão `CREATE DATABASE`, ou base de dados `ClinicaVeterinaria` já criada e vazia, com a collation `Latin1_General_100_CI_AS` e permissão de criação de objetos (a verificação assinala outra collation).

### 5.2. Execução automática (recomendada)

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

Os executores correm os scripts por ordem numérica com `sqlcmd -b -I -f 65001`, param no primeiro erro com código de saída diferente de zero e gravam tudo em `esquema-execucao-AAAAMMDD-HHMMSS.log` na pasta atual (ou na pasta indicada em `-PastaRegistos` / `--pasta-registos`). A opção `-ConfiarCertificado` / `--confiar-certificado` só é necessária com o `sqlcmd` 18 numa instância com certificado autoassinado.

### 5.3. Execução manual no SSMS

Abrir e executar (F5) os ficheiros `00` a `06` por ordem, confirmando no separador *Messages* que cada um termina sem erros. A seguir, executar `verificacao/verificar-esquema.sql` e, se se pretender, `verificacao/testar-restricoes.sql`.

### 5.4. Reexecução

Antes de criar cada objeto, os scripts verificam se ele já existe (`DB_ID`, `OBJECT_ID` e `sys.indexes`). Uma segunda execução não falha nem duplica objetos e regista, para cada um, a mensagem «já existe — sem alterações». Os scripts não alteram objetos já existentes: **uma base de dados criada com a m3-v01 não é atualizada por estes scripts** (não ganha `RowVersion` nem perde as tabelas de F005 e F007). Para passar para a m3-v02, apaga-se a base de dados local do próprio aluno e voltam a executar-se os scripts. O procedimento de reposição com dados é tratado na I062.

## 6. Verificação

### 6.1. Verificação do esquema (`verificar-esquema.sql`)

Este script só lê o catálogo do SQL Server. Compara:

- as 16 tabelas;
- as 99 colunas (ordem, tipo, nulidade e coluna calculada persistida), a partir de uma lista extraída do dicionário de dados do Modelo de Dados e não dos scripts de criação. O SQL Server devolve o nome `timestamp` para o tipo `rowversion`, pelo que `Marcacao.RowVersion` é esperada como `TIMESTAMP NOT NULL`;
- as 68 restrições por nome e tipo (PK, FK, UNIQUE, CHECK, DEFAULT);
- os 19 índices (tabela, unicidade, filtro e colunas por ordem);
- que todas as FKs têm `NO ACTION` e que nenhuma FK ou CHECK está desativada ou não fidedigna;
- que a collation da base de dados é `Latin1_General_100_CI_AS` (DI-08).

Assinala também tabelas, colunas, restrições ou índices não previstos. Termina com erro se existir alguma divergência e com «o esquema corresponde integralmente ao previsto» em caso contrário.

### 6.2. Testes de restrições (`testar-restricoes.sql`)

O script corre numa base de dados acabada de criar e recusa-se a correr se encontrar dados. Tudo decorre numa única transação, anulada no fim, e os contadores `IDENTITY` são repostos, de forma que o primeiro registo real de cada tabela continua a receber `Id = 1`. Os dados são fictícios (domínio reservado `.invalid`). São 41 testes: V01, P01–P10 e N01–N30.

| Grupo | Testes | O que confirma |
|---|---|---|
| Inserções válidas | V01, P07, P08 | Um registo válido em cada uma das 16 tabelas; NIF opcional repetido como `NULL`; raça opcional. |
| Valores por omissão e coluna calculada | P01–P06 | `Ativo`, `Desconhecido`, `Pendente`, datas UTC; `DataProximaAdministracao = DataAdministracao + IntervaloDias` e `NULL` sem intervalo. |
| `RowVersion` (DI-12) | P09, P10 | O valor de `Marcacao.RowVersion` muda quando a marcação é alterada; um `UPDATE` com o valor antigo não altera nenhuma linha, que é o mecanismo em que assentam o 412 (`If-Match` desatualizado) e o 409 (gravações simultâneas) nas marcações. |
| `UNIQUE` | N01, N06, N09, N10, N12, N13, N23 | E-mail de utilizador, `jti`, conta do Cliente, NIF, espécie, raça por espécie, um registo clínico por consulta. |
| `CHECK` (13 de 13) | N02, N03, N08, N15–N20, N24–N27 | Todas as restrições da secção 6.3, incluindo os valores positivos de peso, duração e intervalo em dias. |
| Chaves estrangeiras | N05, N07, N11, N14, N21, N22 | Referências inexistentes, Veterinário que não é funcionário, raça de outra espécie. |
| `NOT NULL`, `NO ACTION`, coluna calculada | N04, N28, N29, N30 | Coluna obrigatória; impossibilidade de apagar dados referenciados (D13); escrita manual da coluna calculada recusada. |

Correspondência com a numeração da m3-v01, para quem a conhece: P05–P09 passam a P04–P08, N42–N44 passam a N28–N30, e saem P04, P10, P11 e N28–N41 (testes de contas, pagamentos e internamento). Os P09 e P10 da m3-v02 são novos e não têm relação com os da m3-v01.

O resultado lista cada teste com a mensagem obtida. O script termina com erro se algum teste falhar.

### 6.3. Evidências a anexar ao Merge Request

1. Numa instância sem a base de dados `ClinicaVeterinaria`, executar `executar-scripts.ps1 -Verificar -Testar` (ou o equivalente `.sh`). O registo resultante prova a execução sem erros, a correspondência com o modelo e o comportamento das restrições.
2. Sem apagar nada, executar novamente `executar-scripts.ps1 -Verificar`. O segundo registo prova a reexecução: todas as mensagens «já existe — sem alterações» e a verificação novamente sem divergências.
3. O quadro de correspondência da secção 8 deste README.

Os registos `.log` anexam-se ao Merge Request e à Issue. O Regulamento da UC (secção 18.4) admite PDF, MD, XLS e XLSX nos documentos e não quer na entrega conteúdos gerados que se possam obter de novo, pelo que os `.log` não fazem parte da pasta entregue; o resumo em Markdown da secção 3 é o que fica no README.

## 7. Convenções aplicadas

As convenções são as da secção 6.1 do Modelo de Dados. Quando as secções 6.3 a 6.5 indicam um nome explícito, é esse o nome usado.

| Elemento | Convenção | Exemplo |
|---|---|---|
| Tabelas e colunas | PascalCase, singular, sem acentos | `RegistoProfilaxia.DataAdministracao` |
| Chave primária | `PK_<Tabela>` | `PK_Marcacao` |
| Chave estrangeira | `FK_<Tabela>_<Referência>_<Coluna>` | `FK_Marcacao_Funcionario_VeterinarioId` |
| Única | `UQ_<Tabela>_<Coluna>` ou nome da secção 6.4 | `UQ_Utilizador_Email`, `UQ_Cliente_NIF` |
| Domínio | nome da secção 6.3 | `CK_Clinica_LinhaUnica` |
| Índice | `IX_<Tabela>_<Colunas>` ou nome da secção 6.5 | `IX_Marcacao_Veterinario_Data` |
| Valor por omissão | `DF_<Tabela>_<Coluna>` (ver DI-03) | `DF_Marcacao_Estado` |
| Datas e horas | `DATETIME2(0)` em UTC com `SYSUTCDATETIME()`; `DATE`; `TIME(0)` | — |
| Texto e valores | `NVARCHAR(n)`; enumerados como texto com `CHECK` (D16) | — |
| Concorrência otimista | `RowVersion ROWVERSION NOT NULL`, só em `Marcacao` (DI-12) | `Marcacao.RowVersion` |

## 8. Quadro de correspondência Modelo de Dados ↔ esquema criado

Contagens recalculadas por programa a partir dos ficheiros da pasta. Os nomes das colunas e das restrições definidas nos scripts foram comparados com as listas de `verificar-esquema.sql` e coincidem nas 16 tabelas. Os tipos, a nulidade e os valores por omissão são os dos scripts da m3-v01, que não foram alterados nas 15 tabelas que mantêm a definição, e serão confirmados por `verificar-esquema.sql` na execução. A coluna UNIQUE inclui o índice único filtrado de `Cliente`. As únicas diferenças de nome e os elementos complementares estão registados na secção 9.

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
| Marcacao (7.11) | dbo.Marcacao | 03-agenda.sql | 12/12 | PK_Marcacao | 3 | 0 | 2 | 2 | 4 | Coincide (DI-12) |
| InformacaoApoio (7.12) | dbo.InformacaoApoio | 03-agenda.sql | 5/5 | PK_InformacaoApoio | 2 | 0 | 0 | 1 | 2 | Coincide |
| RegistoClinico (7.13) | dbo.RegistoClinico | 04-registo-clinico.sql | 8/8 | PK_RegistoClinico | 3 | 1 | 1 | 1 | 2 | Coincide |
| Tratamento (7.14) | dbo.Tratamento | 04-registo-clinico.sql | 4/4 | PK_Tratamento | 1 | 0 | 0 | 0 | 1 | Coincide |
| Prescricao (7.15) | dbo.Prescricao | 04-registo-clinico.sql | 6/6 | PK_Prescricao | 1 | 0 | 1 | 0 | 1 | Coincide |
| RegistoProfilaxia (7.16) | dbo.RegistoProfilaxia | 05-vacinacao-e-desparasitacao.sql | 11/11 | PK_RegistoProfilaxia | 3 | 0 | 2 | 1 | 3 | Coincide |
| **Total** | **16** | — | **99/99** | **16** | **20** | **7** | **13** | **13** | **18** | |

Nota sobre as cardinalidades (secção 4.3):

- **1 : 0..1** — garantida pela PK = FK em `Funcionario` e pelas restrições `UQ_Cliente_UtilizadorId` e `UQ_RegistoClinico_Marcacao`.
- **1 : 0..N e 0..1 : 0..N** — garantidas pelas FKs obrigatórias (`NOT NULL`) ou opcionais (`NULL`).

## 9. Decisões de implementação e divergências face ao Modelo de Dados

Registadas nos termos da Definition of Done (divergências face ao Modelo de Dados registadas e decididas). As decisões DI-01 a DI-10 vêm da m3-v01; DI-11 e DI-12 são novas. As colunas «Proposta» indicam o que já foi incorporado no Modelo de Dados v02 e o que o Revisor ainda tem de confirmar.

| N.º | Situação no Modelo de Dados | Decisão nos scripts | Justificação | Proposta |
|---|---|---|---|---|
| DI-01 | A secção 6.4 do Modelo v01 chamava `PK_TokenRevogado_Jti` à chave primária de `TokenRevogado`; a secção 6.1 define `PK_<Tabela>`. | `PK_TokenRevogado`. | Cumpre a convenção geral, que todas as outras 15 tabelas seguem e que é também o nome por omissão do EF Core. | Incorporado no Modelo de Dados v02. |
| DI-02 | Nome de `CK_Internamento_Estado` repetido em `Internamento` e `HistoricoEstadoInternamento`. | Sem efeito na m3-v02: as duas tabelas foram retiradas (DI-11). | — | Nenhuma. |
| DI-03 | Não há convenção de nomes para os valores por omissão. | `DF_<Tabela>_<Coluna>` nos 13 valores por omissão. | Sem nome explícito, o SQL Server gera nomes aleatórios (`DF__Utiliza__Estad__3B75D760`), diferentes em cada ambiente, o que dificulta a verificação e a comparação com as migrações. | Incorporado no Modelo de Dados v02 (secção 6.1). |
| DI-04 | A nota da secção 6.5 diz que as FKs sem índice próprio são indexadas automaticamente pelo EF Core. | Os scripts criam esses 8 índices explicitamente, com o nome que o EF Core usa (`IX_<Tabela>_<Coluna>`). `Paciente` recebe um único índice `(EspecieId, RacaId)` que cobre as duas FKs. | O esquema criado pelos scripts fica completo sem depender do EF Core. | Confirmar na I062 que as migrações geram os mesmos índices. A confirmar pelo Revisor (Modelo de Dados v02, ponto 9.4). |
| DI-05 | A secção 6.5 não tem índices para a pesquisa de clientes e animais por nome. | Acrescentados `IX_Cliente_Nome` e `IX_Paciente_Nome`. | A pesquisa por NIF e por e-mail já é suportada por `UQ_Cliente_NIF` e `UQ_Utilizador_Email`, a de animais por proprietário por `IX_Paciente_ClienteId` e a de consultas por `IX_Marcacao_Veterinario_Data`. Faltava a pesquisa por nome (EP-011 e EP-015). | Proposta: manter (Modelo de Dados v02, ponto 9.4), a confirmar pelo Revisor. Ajudam a ordenação e a paginação por nome e a pesquisa pelo início do nome; uma pesquisa por parte do nome (`LIKE '%costa%'`, como no exemplo de EP-011) não consegue usar o índice para localizar linhas. |
| DI-06 | O critério de aceitação 3 da I060 pede `UNIQUE` para o «e-mail de cliente». | Garantido por `UQ_Utilizador_Email`. | O e-mail do Cliente é `Utilizador.Email` (nota da secção 7.6 e D02); não existe coluna `Cliente.Email`. | Nenhuma alteração ao esquema. A mesma formulação existe na I059. |
| DI-07 | A secção 6.1 do Modelo v01 indicava a criação do esquema por migrações do EF Core. | Os scripts T-SQL são a referência; as migrações coerentes com eles são a I062. | Divisão aprovada no Backlog Refinement de 12/10/2026 (I026 dividida em I060, I061 e I062). | Incorporado no Modelo de Dados v02 (secção 6.1). |
| DI-08 | Não é definida a collation da base de dados. | `00-criar-base-de-dados.sql` cria a base com `COLLATE Latin1_General_100_CI_AS`, e `verificar-esquema.sql` confirma-a. | Sem collation fixada, a base usa a da instância, que muda conforme a instalação (Windows, contentor, LocalDB), e a ordenação e a comparação de texto mudariam de máquina para máquina. Com `_CI_AS`, `UQ_Utilizador_Email` não distingue `A@x.pt` de `a@x.pt` em nenhum ambiente. | Proposta a confirmar pelo Revisor (Modelo de Dados v02, ponto 9.4). A I062 tem de usar a mesma collation no modelo EF (`UseCollation`). O backend continua a normalizar os e-mails em minúsculas. |
| DI-09 | O UC12 refere a associação do registo clínico à consulta «quando aplicável». | `RegistoClinico.MarcacaoId` é `NOT NULL`. | Segue o Modelo de Dados (D07) e a Documentação da API (EP-031 exige `consultaId`). A incoerência está na especificação do UC12. | Corrigir o UC12. |
| DI-10 | O modelo não define valor por omissão para `Clinica.DataUltimaAlteracao` nem para `ConfiguracaoSistema.DataUltimaAlteracao`. | Sem alteração ao modelo. | Os valores terão de ser indicados nos dados iniciais (I061). | Avaliar no Modelo de Dados v02. |
| DI-11 | O Modelo de Dados v01 e os scripts m3-v01 têm 22 tabelas, 6 delas de F005 e F007. O PS v05 e a DA v05, homologados a 21/10/2026, passam F005 e F007 para o não-âmbito. | Esta versão retira `ContaCliente`, `Pagamento`, `HistoricoEstadoPagamento`, `Alojamento`, `Internamento` e `HistoricoEstadoInternamento`, com as suas 44 colunas, 41 restrições e 16 índices não únicos (mais 2 índices únicos filtrados de `Internamento`). A m3-v01 mantém-se intacta. Os testes são renumerados (secção 6.2). | Decisão do Líder do grupo, 24/10/2026: «Se não faz sentido manter F005 e F007 então remove-se numa v nova». Um esquema com tabelas fora do âmbito levaria a EF Core, API e testes a divergir dele (critérios 1 e 4 da I059). | A registar na I059 pelo Executor/Revisor: os critérios 1 e 4 passam a medir-se contra a m3-v02 (16 tabelas). Rever os dados da I061 que preencham `Alojamento` e o `HasData` do EF (ponto 9.3 do Modelo v02). |
| DI-12 | O Modelo v01 e os scripts m3-v01 não têm coluna `rowversion`. O critério 3 da I059 pede `rowversion` na entidade da consulta (aqui, `Marcacao`). A Documentação da API v02 não documentava os códigos 412 e 409 em EP-026, EP-027 e EP-028. | `Marcacao.RowVersion ROWVERSION NOT NULL`, 12.ª e última coluna. O backend mapeia-a como token de concorrência (`byte[]`, `IsRowVersion()` / `[Timestamp]`) e devolve 412 quando o `If-Match` está desatualizado e 409 quando duas gravações da mesma marcação colidem (Documentação da API v02, secção 5.1, proposta a confirmar pelo Revisor). Mais nenhuma tabela a tem. | Decisão do Líder do grupo, 24/10/2026 (opção B2). `DataUltimaAlteracao` seria um token fraco: tem precisão de 1 segundo e depende de o backend a atualizar sempre. A `RowVersion` é atualizada pelo SQL Server em todas as alterações. | Documentar o 412 e o 409 na Documentação da API v02, no Modelo de Classes e no Plano de Testes. Registar na I059 (critério 3). |

## 10. Relação com as Issues

| Issue | Relação |
|---|---|
| I060 (m3-v01) | Concluída. A m3-v02 não a reabre nem altera os seus ficheiros. |
| I059 | Os critérios que dependem do esquema (todas as tabelas, `rowversion` na consulta, coerência entre o modelo EF e os scripts) passam a medir-se contra a m3-v02, se o Revisor confirmar DI-11 e DI-12. |
| I061 | Os scripts de dados iniciais que preencham tabelas retiradas (`Alojamento`) têm de ser revistos. |
| I062 | As migrações do EF Core devem gerar o esquema da m3-v02, incluindo `RowVersion` e os índices da secção 9 (DI-04). |

Não foi criada nenhuma Issue nova nem alterada nenhuma Issue existente.

## 11. Histórico de versões

| Versão | Data | Autor | Alteração |
|---|---|---|---|
| m3-v01 | 2026-10-16 | Gonçalo Magalhães Ribeiro | Versão inicial: criação do esquema completo do Modelo de Dados v01, execução automática, verificação do esquema e testes de restrições. |
| m3-v02 | 2026-10-24 | Por atribuir | Rascunho: retira as 6 tabelas de F005 e F007 (DI-11), acrescenta `Marcacao.RowVersion` (DI-12), junta `08-indices.sql` como `06-indices.sql`, renumera os testes e ajusta verificação e executores. Executado a 24/10/2026 num SQL Server 2025: 237 verificações sem divergências e 41 testes OK (secção 3). |

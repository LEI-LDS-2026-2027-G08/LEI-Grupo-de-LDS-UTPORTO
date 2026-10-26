# I091 — Infraestrutura dos Testes de Integração

| Campo     | Valor                                                                                 |
| --------- | ------------------------------------------------------------------------------------- |
| Issue     | I091 — Criar a infraestrutura dos testes de integração                                |
| Sprint    | S04                                                                                   |
| Milestone | M3                                                                                    |
| Executor  | Gonçalo Dinis Magalhães Teixeira                                                      |
| Revisor   | Ivan Cristiano Silva Mendes                                                           |
| Auditor   | Gonçalo Magalhães Ribeiro                                                             |
| Data      | 2026-10-26                                                                            |
| Estado    | Implementação do Executor concluída localmente; aguarda pipeline, revisão e auditoria |

## 1. Objetivo

Foi criada a infraestrutura comum para os testes de integração do backend, permitindo executar testes contra a API real em memória e uma base de dados SQL Server real e isolada.

A infraestrutura foi preparada para:

- utilização de `WebApplicationFactory`;
- execução contra SQL Server real;
- criação automática da base de dados;
- aplicação automática das migrações;
- carregamento de dados iniciais e de demonstração;
- reposição do estado entre testes;
- autenticação JWT por perfil;
- criação de dados únicos de teste;
- execução paralela entre classes sem partilha de estado;
- destruição automática dos recursos no fim da execução.

---

## 2. Tecnologias utilizadas

A infraestrutura utiliza:

- .NET 10;
- ASP.NET Core;
- xUnit 2.6;
- `WebApplicationFactory`;
- Entity Framework Core 10;
- SQL Server 2025;
- Testcontainers for .NET;
- Respawn;
- autenticação JWT da própria aplicação.

---

## 3. Componentes implementados

| Componente               | Ficheiro                             | Finalidade                                                                        |
| ------------------------ | ------------------------------------ | --------------------------------------------------------------------------------- |
| Fábrica da API           | `ClinicaWebApplicationFactory.cs`    | Executa a API real em memória com configuração própria para testes                |
| Fixture de integração    | `IntegracaoFixture.cs`               | Cria SQL Server, base de dados, aplica migrações, carrega dados e prepara limpeza |
| Classe base              | `TesteIntegracaoBase.cs`             | Garante reposição do estado antes de cada teste                                   |
| Autenticação             | `AutenticacaoTeste.cs`               | Fornece `HttpClient` autenticado para qualquer perfil                             |
| Dados de teste           | `DadosTeste.cs`                      | Gera valores únicos para evitar colisões entre testes                             |
| Testes de infraestrutura | `InfraestruturaTests.cs`             | Demonstra criação da base e isolamento entre testes                               |
| Testes de autenticação   | `AutenticacaoInfraestruturaTests.cs` | Demonstra autenticação através da infraestrutura criada                           |

Foi também adicionada ao `Program.cs` da API a declaração:

```csharp
public partial class Program { }
```

Esta declaração permite a utilização de:

```csharp
WebApplicationFactory<Program>
```

no projeto de testes de integração.

---

## 4. Base de dados de testes

Cada `IntegracaoFixture` cria automaticamente um contentor SQL Server através de Testcontainers.

É utilizada a imagem:

```text
mcr.microsoft.com/mssql/server:2025-CU9-ubuntu-22.04
```

Para cada fixture é criada uma base de dados independente com um nome no formato:

```text
ClinicaVeterinaria_I091_<guid>
```

A base utiliza a collation:

```text
Latin1_General_100_CI_AS
```

Após a criação da base:

1. são aplicadas as migrações da I062;
2. os dados de referência são criados através do `HasData`;
3. é executado o script de dados de demonstração da I061;
4. é configurado o Respawn para reposição do estado.

No final da fixture, o contentor SQL Server é destruído automaticamente.

---

## 5. Dados iniciais e de demonstração

A infraestrutura utiliza:

```text
04-artefactos-tecnicos/
04.13-scripts-de-base-de-dados/
m3-v02/
dados-demonstracao/
02-dados-demonstracao.sql
```

O script provisório:

```text
01-dados-referencia-provisorios.sql
```

não é utilizado.

Os dados de referência são obtidos através das migrações da I062.

O script `02-dados-demonstracao.sql` fornece os dados necessários aos testes, incluindo:

- contas dos vários perfis;
- Clientes;
- pacientes;
- marcações;
- registos clínicos;
- tratamentos;
- prescrições;
- registos de profilaxia.

---

## 6. Reposição do estado com Respawn

Antes de cada teste, a base de dados é reposta para um estado conhecido.

Respawn preserva as tabelas de referência:

- `__EFMigrationsHistory`;
- `Clinica`;
- `ConfiguracaoSistema`;
- `Especie`;
- `Raca`.

As restantes tabelas são limpas.

Após a limpeza, os dados de demonstração da I061 são carregados novamente.

Desta forma, cada teste começa sempre com o mesmo estado inicial e não depende das alterações realizadas por testes anteriores.

---

## 7. Execução paralela e isolamento

Os testes utilizam:

```csharp
IClassFixture<IntegracaoFixture>
```

Cada classe de testes recebe uma instância própria de `IntegracaoFixture`.

Assim, classes diferentes podem executar em paralelo utilizando:

- contentores SQL Server diferentes;
- bases de dados diferentes;
- instâncias independentes da aplicação.

Foi executado localmente um teste em que duas classes correram em paralelo.

Foram observados dois contentores SQL Server distintos a arrancar simultaneamente e ambos foram destruídos automaticamente no fim.

A execução terminou sem falhas.

---

## 8. Cliente HTTP autenticado por perfil

Foi implementado o método:

```csharp
await fixture.CriarClienteAutenticadoAsync(perfil);
```

O método:

1. procura uma conta ativa do perfil pretendido;
2. obtém os dados do utilizador através do `IAuthService`;
3. utiliza o `IJwtTokenService` real da aplicação;
4. gera um JWT válido;
5. cria um `HttpClient`;
6. adiciona automaticamente o token Bearer.

Isto permite que os restantes testes obtenham um cliente autenticado numa única chamada.

Foram testados os cinco perfis ativos:

- `Administrador`;
- `Rececionista`;
- `Veterinario`;
- `AuxiliarVeterinario`;
- `Cliente`.

Todos conseguiram aceder corretamente ao endpoint:

```http
GET /auth/me
```

---

## 9. Utilitários de construção de dados

Foi criada a classe:

```text
DadosTeste.cs
```

Esta classe fornece utilitários para gerar dados únicos durante os testes.

Exemplos:

```csharp
DadosTeste.EmailUnico();
DadosTeste.NomeUnico();
DadosTeste.TextoUnico();
```

Estes valores evitam colisões entre testes executados em paralelo e reduzem dependências de dados fixos.

---

## 10. Testes demonstrativos

### 10.1. I091-INF-001

Teste:

```text
I091-INF-001 — SQL Server isolado é criado, migrado e populado
```

Confirma que:

- é utilizado um SQL Server real;
- a base de dados possui um nome exclusivo da I091;
- as migrações foram aplicadas;
- existe uma clínica;
- existem os utilizadores de demonstração.

---

### 10.2. I091-INF-002

Teste:

```text
I091-INF-002 — alterações de um teste são removidas pelo reset
```

O teste:

1. cria um novo utilizador;
2. confirma a gravação;
3. executa a reposição da base;
4. consulta novamente a base;
5. confirma que o utilizador criado deixou de existir.

Este teste demonstra que alterações de um teste não interferem nos testes seguintes.

---

### 10.3. IT-EP003-01 / I091

Teste:

```text
IT-EP003-01 / I091 — cliente autenticado por perfil acede a /auth/me
```

Este teste corresponde ao caso `IT-EP003-01` da Especificação dos Testes de Integração.

É parametrizado para os cinco perfis da aplicação.

Para cada perfil:

1. é criado um cliente HTTP autenticado;
2. é efetuado `GET /auth/me`;
3. é esperado `200 OK`;
4. é confirmado que o perfil devolvido corresponde ao perfil utilizado.

---

## 11. Correspondência com a I030

A I091 é responsável pela infraestrutura comum e não pela implementação de todos os testes funcionais da I030.

| Referência I030                       | Implementação na I091                                                | Estado               |
| ------------------------------------- | -------------------------------------------------------------------- | -------------------- |
| Secção 5 — Ambiente previsto          | WebApplicationFactory, SQL Server, Testcontainers e Respawn          | Implementado         |
| Secção 7 — Dados de teste             | I061 + `DadosTeste`                                                  | Implementado         |
| Secção 8 — Estado inicial e reposição | Migrações I062 + I061 + Respawn                                      | Implementado         |
| IT-EP003-01                           | Teste de `/auth/me` autenticado                                      | Implementado         |
| Restantes `IT-EP*`                    | Implementação prevista nas Issues funcionais de testes de integração | Não aplicável à I091 |
| `IT-TRV-*`                            | Implementação prevista nas Issues de segurança e testes transversais | Não aplicável à I091 |
| `IT-CONC-*`                           | Implementação prevista nos testes de concorrência                    | Não aplicável à I091 |

Os testes `I091-INF-001` e `I091-INF-002` são testes próprios da infraestrutura e não substituem casos funcionais da I030.

---

## 12. Execução local

Foi executado:

```powershell
dotnet test .\ClinicaVeterinaria.IntegrationTests\ClinicaVeterinaria.IntegrationTests.csproj `
  --no-build `
  --logger "trx;LogFileName=i091-perfis.trx" `
  --results-directory .\ClinicaVeterinaria.IntegrationTests\TestResults
```

Resultado final:

```text
total: 7
failed: 0
succeeded: 7
skipped: 0
```

A execução produziu o ficheiro:

```text
ClinicaVeterinaria.IntegrationTests/TestResults/i091-perfis.trx
```

A pasta:

```text
TestResults/
```

encontra-se ignorada pelo `.gitignore`.

Os resultados deverão ser novamente produzidos pela pipeline.

---

## 13. Evidência de paralelismo

Foi realizada uma execução com duas classes de testes em paralelo.

Durante a execução foram criados dois contentores SQL Server distintos.

Ambos:

- arrancaram independentemente;
- aplicaram as próprias migrações;
- utilizaram bases isoladas;
- executaram os testes sem interferência;
- foram destruídos no final.

Resultado:

```text
total: 3
failed: 0
succeeded: 3
skipped: 0
```

---

## 14. Critérios de aceitação da I091

| Critério                                                              | Estado                                |
| --------------------------------------------------------------------- | ------------------------------------- |
| 1. Casos I030 aplicáveis implementados com identificador              | Cumprido                              |
| 2. API real em memória + SQL Server isolado + independência da ordem  | Cumprido localmente                   |
| 3. Cliente HTTP autenticado por perfil numa única linha               | Cumprido                              |
| 4. Base criada e destruída automaticamente sem instâncias partilhadas | Cumprido localmente                   |
| 5. Teste demonstrativo de autenticação e isolamento                   | Cumprido                              |
| 6. Execução sem falhas e resultado legível pela pipeline              | Cumprido localmente através de `.trx` |

A confirmação da execução na pipeline fica dependente do push da branch.

---

## 15. Evidências do Executor

Foram produzidas localmente as seguintes evidências:

- projeto compila sem erros;
- testes de integração executam contra SQL Server real;
- migrações aplicadas automaticamente;
- dados da I061 carregados automaticamente;
- Respawn repõe o estado entre testes;
- autenticação por todos os perfis validada;
- execução paralela com bases independentes validada;
- ficheiro `.trx` produzido;
- `git diff --check` sem erros;
- `TestResults` corretamente ignorado pelo `.gitignore`.

---

## 16. Estado antes da revisão

A implementação técnica local do Executor encontra-se concluída.

Ainda ficam pendentes:

1. commit das alterações;
2. push da branch;
3. execução da pipeline;
4. confirmação dos testes na pipeline;
5. revisão independente pelo Revisor;
6. auditoria pelo Auditor.

Após a pipeline ficar verde, o registo deverá ser atualizado com a evidência dessa execução.

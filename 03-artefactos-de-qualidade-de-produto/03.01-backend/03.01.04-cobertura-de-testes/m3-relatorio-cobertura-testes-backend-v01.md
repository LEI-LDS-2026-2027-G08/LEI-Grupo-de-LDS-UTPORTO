# Relatório de Cobertura dos Testes do Backend — Gestão de Clínica Veterinária

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Relatório de cobertura dos testes do backend |
| Ficheiro | `m3-relatorio-cobertura-testes-backend-v01.pdf` (fonte: `m3-relatorio-cobertura-testes-backend-v01.md`) |
| Versão | v01 |
| Milestone | m3 |
| Issue | I032 — Produzir o Relatório de Cobertura do Backend (GitHub #62) |
| Sprint | S05 |
| Executor | André Filipe Moreira Mesquita |
| Revisor | Gonçalo Magalhães Ribeiro |
| Auditor | Gonçalo Dinis Magalhães Teixeira |
| Versão medida | `develop` no commit `df5b5f2` (2026-11-04 00:41 UTC), execução #184 da pipeline |
| Pasta | `/projeto/03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes` |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-11-04 | André Filipe Moreira Mesquita | Cobertura dos testes unitários e de integração do backend sobre `df5b5f2` (execução #184): ferramentas, método, âmbito e exclusões, resultados por projeto e por área funcional, comparação com os limiares do Plano de Testes e análise das linhas não cobertas. |

## Resumo

Cobertura medida na pipeline sobre `develop` no commit `df5b5f2` (execução #184), com os testes unitários e de integração em conjunto. **No âmbito do quality gate QG-03 (Domínio e Serviços): 98,5% das linhas (2118 de 2150) e 92,8% dos ramos (596 de 642)**, para um limiar obrigatório de 70% de linhas. Nenhum projeto nem nenhuma área funcional fica abaixo do limiar; as menores coberturas de linhas por área são as do F001 e do F004 (97,8%). Os módulos críticos do Plano de Testes (LT-03, ≥ 85%) estão todos entre 96,2% e 100%. Ficam 32 linhas não cobertas, em 12 classes: 21 são caminhos defensivos, de concorrência ou dependentes do sistema que os testes não provocam de forma determinística (exceção justificada) e 11 são caminhos de validação e de alteração de marcações sem teste, para os quais se propõem testes adicionais (secção 7). Os relatórios do ReportGenerator gerados pela pipeline estão anexados.

---

## 1. Objetivo

Documentar a cobertura de código dos testes automáticos do backend entregue na m3: ferramentas e método de recolha, âmbito medido e exclusões justificadas, cobertura de linhas e de ramos por projeto e por área funcional, comparação com os limiares do Plano de Testes e tratamento das zonas não cobertas. Segue a definição da I032 aprovada no Backlog Refinement do Sprint 05 (Backlog de Projeto v03) e o Sprint Backlog do Sprint 05 v02.

## 2. Documentos de referência

| Documento | Versão | Utilização neste relatório |
|---|---|---|
| Backlog de Projeto (I032) | v03, aprovada como Ready a 2026-10-27 | Critérios de aceitação, evidências e verificações |
| Plano de Testes do Backend | `m3-plano-testes-backend-v01.md` (aprovada, I027), secção 9 | Limiares LT-01 (obrigatório), LT-02 e LT-03 (propostos) e exclusões justificadas |
| Regulamento Interno do Grupo | v02, secções 11.5 e 11.6 | Ferramentas (Coverlet 6.0 e ReportGenerator 5.5), métrica, âmbito e QG-03 |
| Configuração dos quality gates | `.github/quality-gates.json` (I103) | Âmbito, exclusões e limiar usados na pipeline |
| Pipeline do backend | `.github/workflows/backend-ci.yml` (I102, I103) | Recolha e geração dos relatórios |
| Relatórios de resultados dos testes | I090 (unitários) e I097 (integração) | Testes executados na medição |

A v02 do Plano de Testes (rascunho, em `main`) mantém o mesmo limiar de 70% e as mesmas exclusões; a avaliação deste relatório é igual com qualquer das duas versões.

## 3. Ferramentas e método de recolha

| Elemento | Valor |
|---|---|
| Recolha | Coverlet, `coverlet.collector` 6.0.0, com `--collect:"XPlat Code Coverage"` em cada `dotnet test` |
| Testes executados | Unitários (`ClinicaVeterinaria.UnitTests`): 718 testes, 717 aprovados, 1 ignorado com justificação (job «2 · Testes unitários»). Integração (`ClinicaVeterinaria.IntegrationTests`): 463 testes, 463 aprovados (job «3 · Testes de integração»), contra um SQL Server criado em cada execução |
| Junção e relatório | ReportGenerator 5.5.11 (job «4 · Cobertura (QG-03)»), sobre os ficheiros `coverage.cobertura.xml` dos dois jobs |
| Execução | Backend CI #184 — https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41748757482 (push em `develop`, commit `df5b5f2`, todas as etapas com sucesso; relatórios gerados a 2026-11-04 00:46 UTC) |
| Relatório completo | Todas as assemblies de produção (filtro `-*.UnitTests;-*.IntegrationTests`): informativo |
| Relatório do QG-03 | Âmbito e exclusões lidos de `.github/quality-gates.json` (secção 4); é este que a pipeline compara com o limiar |

O job 4 junta quatro ficheiros Cobertura: o `coverage.cobertura.xml` de cada job de testes e a cópia que o registo TRX guarda junto do resultado. A cópia contém os mesmos dados e não altera as percentagens.

Comandos equivalentes para repetir a medição localmente (na raiz do repositório, com o Docker ativo para os testes de integração):

```
dotnet test src/backend/ClinicaVeterinaria.sln --configuration Release --collect:"XPlat Code Coverage" --results-directory cobertura-local
dotnet tool install dotnet-reportgenerator-globaltool --version 5.5.11 --tool-path .ferramentas
.ferramentas/reportgenerator "-reports:cobertura-local/**/coverage.cobertura.xml" "-targetdir:cobertura-local/qg-03" "-reporttypes:Html;TextSummary" "-assemblyfilters:+ClinicaVeterinaria.Domain;+ClinicaVeterinaria.Application;+ClinicaVeterinaria.Infrastructure" "-classfilters:-ClinicaVeterinaria.Application.DTOs.*;-ClinicaVeterinaria.Infrastructure.Migrations.*;-ClinicaVeterinaria.Infrastructure.ClinicaDbContext;-ClinicaVeterinaria.*.DependencyInjection" "-filefilters:-**/obj/**"
```

## 4. Âmbito medido e exclusões

**Incluído (QG-03, LT-01):** as camadas de Domínio e Serviços — `ClinicaVeterinaria.Domain` (entidades), `ClinicaVeterinaria.Application` (agenda, validação, segurança, cálculo da próxima administração e exceções) e `ClinicaVeterinaria.Infrastructure`, onde estão os serviços da aplicação (`*Service`), que implementam as interfaces da camada de Aplicação.

**Excluído**, cada exclusão com a sua justificação:

| Exclusão | Filtro | Justificação | Fonte | Linhas (cobertura, informativa) |
|---|---|---|---|---|
| Camada API (`ClinicaVeterinaria.Api`), incluindo o `Program.cs` | Fora das assemblies incluídas | Controladores finos, sem regras de negócio, e configuração inicial; o comportamento é verificado nos testes de integração | Regulamento 11.5 (Program.cs); Plano de Testes, secção 9 | 567 (90,7%) |
| DTOs | `ClinicaVeterinaria.Application.DTOs.*` | Objetos de transporte sem lógica | Regulamento 11.5; Plano de Testes, secção 9 | 295 (99,7%) |
| Migrações do EF Core | `ClinicaVeterinaria.Infrastructure.Migrations.*` | Código gerado automaticamente | Regulamento 11.5; Plano de Testes, secção 9 | 3206 (97,8%) |
| Configuração do EF Core | `ClinicaVeterinaria.Infrastructure.ClinicaDbContext` | Mapeamento do modelo, sem regras de negócio | Plano de Testes, secção 9 (a Infraestrutura de configuração do EF Core não é medida) | 312 (100%) |
| Registo de dependências | `ClinicaVeterinaria.*.DependencyInjection` | Configuração do contentor de dependências | Plano de Testes, secção 9 | 14 (100%) |
| Código gerado na compilação | `**/obj/**` | Ficheiros gerados por geradores de origem, sem ficheiro no repositório | Plano de Testes, secção 9 (ficheiros gerados) | — |

Não há classes marcadas com `[ExcludeFromCodeCoverage]` no código. As 2150 linhas do âmbito e as 4394 excluídas somam as 6544 linhas mensuráveis do relatório completo.

## 5. Resultados

### 5.1. Global

| Medida | Âmbito do QG-03 (Domínio e Serviços) | Relatório completo (informativo) |
|---|---|---|
| Linhas cobertas / mensuráveis | 2118 / 2150 | 6388 / 6544 |
| **Cobertura de linhas** | **98,5%** | 97,6% |
| Cobertura de ramos | 92,8% (596 de 642) | 91,0% (732 de 804) |
| Cobertura de métodos | 99,3% (286 de 288) | 99,4% (676 de 680) |
| Classes | 52 | 145 |

Só com os testes unitários, a cobertura de linhas no mesmo âmbito é de 96,2% (2070 de 2150), conforme o relatório da I090 e a reexecução do seu Revisor.

### 5.2. Por projeto (âmbito do QG-03)

| Projeto | Classes | Linhas | Cobertura de linhas | Ramos | Cobertura de ramos |
|---|---|---|---|---|---|
| `ClinicaVeterinaria.Domain` | 16 | 97 / 99 | 98,0% | — (sem ramos) | — |
| `ClinicaVeterinaria.Application` | 24 | 335 / 345 | 97,1% | 237 / 252 | 94,0% |
| `ClinicaVeterinaria.Infrastructure` (serviços) | 12 | 1686 / 1706 | 98,8% | 359 / 390 | 92,1% |
| **Total** | **52** | **2118 / 2150** | **98,5%** | **596 / 642** | **92,8%** |

### 5.3. Por área funcional (âmbito do QG-03)

Cada classe do âmbito foi atribuída a uma área segundo a funcionalidade que serve (serviço, regras e entidades); todas as 52 classes ficam atribuídas.

| Área | Classes | Linhas | Cobertura de linhas | Ramos | Cobertura de ramos |
|---|---|---|---|---|---|
| F001 — Autenticação e Controlos de Acesso (autenticação, contas, clínica, Clientes) | 17 | 697 / 713 | 97,8% | 218 / 242 | 90,1% |
| F002 — Gestão de pacientes | 4 | 283 / 283 | 100% | 44 / 46 | 95,7% |
| F003 — Agendamento de consultas | 18 | 712 / 721 | 98,8% | 209 / 220 | 95,0% |
| F004 — Registo clínico e tratamentos | 5 | 267 / 273 | 97,8% | 86 / 94 | 91,5% |
| F006 — Boletim de vacinas e desparasitações | 4 | 150 / 151 | 99,3% | 39 / 40 | 97,5% |
| Transversal — exceções e validação comum | 4 | 9 / 9 | 100% | — | — |
| **Total** | **52** | **2118 / 2150** | **98,5%** | **596 / 642** | **92,8%** |

Composição das áreas: F001 — `AuthService`, `UtilizadoresService`, `ClinicaService`, `ClientesService`, `ClaimsPrincipalExtensions`, `Perfis`, validadores de e-mail, NIF, palavra-passe e horário, e as entidades `Utilizador`, `Funcionario`, `Cliente`, `Clinica`, `ConfiguracaoSistema`, `HorarioFuncionamento` e `TokenRevogado`; F002 — `AnimaisService`, `Paciente`, `Especie` e `Raca`; F003 — `ConsultasService`, `VeterinariosService`, `MinhasConsultasService`, `BloqueioAgendaVeterinario`, `DuracaoConsulta`, as classes de `Application.Agenda`, `AcessoConsultas`, os validadores de filtros e de período de disponibilidade e as entidades `Marcacao` e `InformacaoApoio`; F004 — `RegistosClinicosService`, `ValidadorRegistoClinico`, `RegistoClinico`, `Tratamento` e `Prescricao`; F006 — `VacinacoesService`, `CalculadoraProximaAdministracao`, `TiposAdministracao` e `RegistoProfilaxia`; Transversal — as três exceções da aplicação e `PeriodoValidado`. F005 e F007 estão fora do âmbito (Declaração de Âmbito v05).

## 6. Comparação com os limiares do Plano de Testes

| Limiar (Plano de Testes, secção 9) | Tipo | Valor medido | Resultado |
|---|---|---|---|
| LT-01 — cobertura de linhas ≥ 70% em Domínio e Serviços, unitários e integração em conjunto (QG-03, MQ-02) | Obrigatório | 98,5% | **Cumprido**; aplicado na pipeline pelo QG-03 (execução #184: «QG-03 cumprido: 98.51% ≥ 70%») |
| LT-02 — cobertura de ramos ≥ 60% nas mesmas camadas | Proposto, informativo | 92,8% | Acima da referência |
| LT-03 — cobertura de linhas ≥ 85% nos módulos críticos | Proposto, verificado na revisão | Ver abaixo | Todos acima de 85% |

Módulos críticos do LT-03:

| Módulo crítico | Classes | Cobertura de linhas |
|---|---|---|
| Autorização por perfil e por propriedade | `AcessoConsultas`, `ClaimsPrincipalExtensions`, `Perfis` | 100%, 100%, 100% |
| Transições de estado da marcação | `MaquinaEstadosMarcacao`; `ConsultasService` | 96,2%; 99,2% |
| Cálculo da próxima administração | `CalculadoraProximaAdministracao`; `VacinacoesService` | 100%; 100% |
| Regras financeiras e internamento | — | Fora do âmbito (F005 e F007, Declaração de Âmbito v05) |

## 7. Áreas abaixo do limite e linhas não cobertas

**Nenhum projeto nem nenhuma área funcional está abaixo do limiar de 70%**, nem dos 85% propostos para os módulos críticos. As 32 linhas não cobertas do âmbito estão em 12 classes; cada uma foi analisada no código do commit `df5b5f2`:

| Classe (cobertura de linhas) | Linhas não cobertas | Causa | Ação decidida |
|---|---|---|---|
| `FusoHorarioClinica` (87,5%) | 59, 61 | Alternativa para Windows: só corre se o fuso «Europe/Lisbon» (IANA) não existir no sistema; no runner Linux e nas versões atuais do .NET em Windows existe | Exceção justificada (depende do sistema operativo) |
| `MaquinaEstadosMarcacao` (96,2%) | 18 | Estado vazio na mudança de estado: a validação do pedido recusa-o antes de chegar a esta guarda | Teste adicional proposto (teste unitário direto da guarda) |
| `ValidadorHorario` (95,8%) | 44–45 | Período `null` dentro da lista do horário (JSON com um elemento `null`) | Teste adicional proposto |
| `ValidadorRegistoClinico` (86,8%) | 59, 80–81, 91–92 | Prescrição repetida na atualização e elementos `null` nas listas de tratamentos e de prescrições | Testes adicionais propostos |
| `HorarioFuncionamento` (80,0%) | 5 | Propriedade `Id` nunca lida pela aplicação (o EF Core preenche o campo sem passar pelo acessor) | Exceção justificada |
| `RegistoProfilaxia` (90,9%) | 8 | Propriedade `MarcacaoId` mapeada no modelo de dados mas sem uso nas operações da API v02 | Exceção justificada |
| `AuthService` (97,4%) | 191, 195, 197, 199 | Dois fins de sessão simultâneos com o mesmo token (conflito na chave de `TokenRevogado`) | Exceção justificada (concorrência não determinística) |
| `BloqueioAgendaVeterinario` (84,2%) | 47, 64, 67 | Guarda contra uso sem transação (erro de programação) e bloqueio da agenda não obtido dentro do tempo de espera (erro 51073) | Exceção justificada (os testes de concorrência da I093 não esgotam o tempo de espera) |
| `ClinicaService` (94,3%) | 62–63, 76–77 | Criação da clínica e da configuração quando não existem; as migrações da I062 criam-nas sempre | Exceção justificada (dados de referência garantidos pelas migrações) |
| `ConsultasService` (99,2%) | 337, 358, 519 | 337 e 358: na **alteração** de uma marcação (EP-026), Veterinário inativo e novo horário fora do horário da clínica (os mesmos casos estão testados na criação, EP-025); 519: data com `DateTimeKind.Local`, que a desserialização JSON não produz | 337 e 358: testes adicionais propostos; 519: exceção justificada |
| `RegistosClinicosService` (99,5%) | 176 | Atualização com uma prescrição de outro registo (o caso equivalente para tratamentos, UT-F004-019, está testado) | Teste adicional proposto |
| `UtilizadoresService` (97,3%) | 336–339, 341 | Dois pedidos simultâneos com o mesmo e-mail, travados pelo índice único | Exceção justificada (concorrência não determinística) |

**Resumo das ações:** 21 linhas em exceção justificada (`FusoHorarioClinica` 2, `HorarioFuncionamento` 1, `RegistoProfilaxia` 1, `AuthService` 4, `BloqueioAgendaVeterinario` 3, `ClinicaService` 4, `ConsultasService` 1 e `UtilizadoresService` 5); 11 linhas com testes adicionais propostos (`MaquinaEstadosMarcacao` 1, `ValidadorHorario` 2, `ValidadorRegistoClinico` 5, `ConsultasService` 2 e `RegistosClinicosService` 1). Como o limiar está cumprido com folga, os testes adicionais não bloqueiam a entrega: ficam como proposta de nova Issue de testes para o Backlog Refinement do Sprint 06 (Plano de Testes, secção 12: sugestões para lá das obrigações aprovadas seguem como novas Issues).

## 8. Correspondência com a pipeline e relatórios anexados

Os números deste relatório são os do relatório do ReportGenerator gerado na execução #184 (artefacto `relatorio-cobertura`), anexado nesta pasta sem alterações:

| Ficheiro | Conteúdo |
|---|---|
| `m3-s05-i032-20261104-relatorio-cobertura-html-v01.zip` | Artefacto `relatorio-cobertura` da execução #184, tal como publicado: relatório HTML completo (`index.htm`), `Cobertura.xml`, `Summary.txt` e `SummaryGithub.md`; na subpasta `qg-03`, o relatório HTML e os resumos do âmbito do QG-03 |
| `m3-s05-i032-20261104-resumo-cobertura-qg03-v01.txt` | `qg-03/Summary.txt`: resumo do âmbito do QG-03, por assembly e classe |
| `m3-s05-i032-20261104-resumo-cobertura-completo-v01.txt` | `Summary.txt`: resumo do relatório completo |

As tabelas das secções 5 a 7 foram calculadas a partir de `qg-03/Cobertura.xml` e `Cobertura.xml` desse artefacto; os totais coincidem com os resumos do ReportGenerator e com a linha «QG-03 cumprido: 98.51% ≥ 70%» do job 4. O artefacto fica também disponível na página da execução durante 90 dias.

## 9. Conclusão

A cobertura de linhas do backend no âmbito do QG-03 (Domínio e Serviços), com os testes unitários e de integração em conjunto, é de **98,5%** (2118 de 2150 linhas), acima do limiar obrigatório de 70% do Plano de Testes (LT-01). A cobertura de ramos é de 92,8% (LT-02: referência de 60%) e todos os módulos críticos estão acima de 85% (LT-03). Nenhum projeto nem nenhuma área funcional fica abaixo do limite. As exclusões são as do Regulamento e do Plano de Testes, cada uma justificada (secção 4). Das 32 linhas não cobertas, 21 estão justificadas como caminhos defensivos, de concorrência ou dependentes do sistema e 11 têm testes adicionais propostos para uma nova Issue (secção 7).

## 10. Critérios de aceitação da I032

| Critério (Backlog v03) | Verificação | Estado |
|---|---|---|
| 1. Ferramenta, método de recolha, âmbito medido e exclusões, cada uma justificada | Secções 3 e 4 | Cumprido |
| 2. Cobertura de linhas e de ramos por projeto e por área funcional, comparada com os limites do plano de testes | Secções 5.2, 5.3 e 6 | Cumprido |
| 3. Áreas abaixo do limite, causa e ação decidida | Secção 7: nenhuma área abaixo do limite; causa e ação de cada linha não coberta | Cumprido |
| 4. Números iguais aos dos relatórios da pipeline, anexados em HTML ou equivalente | Secção 8: relatório HTML da execução #184 anexado | Cumprido |

**Evidências da Issue:** (1) este relatório em PDF; (2) os relatórios gerados pelo ReportGenerator (secção 8); (3) a ligação à execução da pipeline (secção 3).

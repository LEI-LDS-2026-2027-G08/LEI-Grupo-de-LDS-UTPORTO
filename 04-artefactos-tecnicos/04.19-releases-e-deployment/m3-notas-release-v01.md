# Notas de Release do Backend — m3

| Campo | Valor |
|---|---|
| Grupo | G08 |
| Projeto | Gestão de Clínica Veterinária |
| Artefacto | Notas de Release do backend |
| Ficheiro | `m3-notas-release-v01.md` |
| Versão do documento | v01 |
| Milestone | m3 |
| Issue | I039 — Elaborar as Notas de Release do backend (GitHub #135) |
| Sprint | S05 |
| Executor | Ivan Cristiano Silva Mendes |
| Revisor | André Filipe Moreira Mesquita |
| Auditor | Gonçalo Magalhães Ribeiro |
| Pasta | `/projeto/04-artefactos-tecnicos/04.19-releases-e-deployment` |

## Histórico de versões

| Versão | Data | Autor | Descrição |
|---|---|---|---|
| v01 | 2026-11-04 | Ivan Cristiano Silva Mendes | Notas de release do backend da m3 sobre `develop` no commit `39040cd`: versão, operações incluídas e excluídas, limitações, resultados de testes e qualidade, compatibilidade, instalação e atualização. |

---

## 1. Identificação da versão

| Elemento | Valor |
|---|---|
| Versão | Backend **m3** — primeira versão do backend (as milestones m1 e m2 não incluíam código do backend) |
| Commit | `39040cdc1c6e6c018182c674169037670e90fdf0` em `develop` (2026-11-03 18:41 UTC) |
| Código | `src/backend/ClinicaVeterinaria.sln` — projetos `Api`, `Application`, `Domain`, `Infrastructure`, `UnitTests` e `IntegrationTests` |
| Execução da pipeline | Backend CI #170 — https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/actions/runs/41696169008 (todas as etapas com sucesso) |
| Contrato da API | Documentação da API v02 (`04.11-documentacao-de-api/m3-documentacao-api-v02.md`) |
| Âmbito | Declaração de Âmbito v05 (`04.01-proposta-de-sistema-e-declaracao-de-ambito/m3-declaracao-ambito-v05.pdf`) |

Estas notas descrevem o código em `develop` no commit indicado. A versão entregue na m3 é a que o grupo promover de `develop` para `main`, à qual se aplica a etiqueta `m3` (como `m1` e `m2`). Se, entretanto, forem integradas em `develop` alterações ao código do backend, a secção 1 é atualizada com o commit promovido.

**Estado das dependências (Backlog de Projeto v03)** à data destas notas:

| Dependência | Estado | Uso nestas notas |
|---|---|---|
| I097 — Resultados dos testes de integração | Integrada em `develop` | Secção 5 |
| I034 — Análise estática do backend | Integrada em `develop` | Secção 5 |
| I090 — Resultados dos testes unitários | Entregue (PR #241), em auditoria; ainda não integrada em `develop` | Secção 5: números da execução #170, que a I090 documenta |
| I032 — Relatório de cobertura do backend | **Ainda não iniciada** | Secção 5: a cobertura indicada é a medida pela pipeline (quality gate QG-03); o relatório formal de cobertura **é produzido na I032** |

Quando a I090 e a I032 forem integradas, a secção 5 é atualizada com as referências finais.

## 2. Funcionalidades e operações incluídas

| Funcionalidade | Operações | Requisitos |
|---|---|---|
| F001 — Autenticação e Controlos de Acesso | EP-001 a EP-014 (14) | RF001–RF013, RF019 |
| F002 — Gestão de pacientes | EP-015 a EP-020 (6) | RF014–RF018 |
| F003 — Agendamento de consultas | EP-021 a EP-030 (10) | RF019–RF029 |
| F004 — Registo clínico e tratamentos | EP-031 a EP-034 (4) | RF030–RF036 |
| F006 — Boletim de vacinas e desparasitações | EP-042 a EP-044 (3) e atualização de uma administração (RF056) | RF043–RF047, RF056 |
| **Total** | **37 operações da API v02 + 1 (RF056)** | |

Todas as operações da Documentação da API v02 estão implementadas, com o método e a rota documentados (conferido com o Swagger da API: 38 operações, as 37 documentadas e a do RF056).

| EP | Método e rota | Operação |
|---|---|---|
| EP-001 | `POST /auth/login` | Iniciar sessão |
| EP-002 | `POST /auth/logout` | Terminar sessão |
| EP-003 | `GET /auth/me` | Consultar utilizador autenticado |
| EP-004 | `GET /utilizadores` | Listar contas de funcionários |
| EP-005 | `GET /utilizadores/{id}` | Consultar conta de funcionário |
| EP-006 | `POST /utilizadores` | Criar conta de funcionário |
| EP-007 | `PUT /utilizadores/{id}` | Atualizar conta de funcionário e atribuir perfil |
| EP-008 | `PATCH /utilizadores/{id}/estado` | Bloquear conta de funcionário |
| EP-009 | `GET /clinica` | Consultar dados gerais, configurações e horário da clínica |
| EP-010 | `PUT /clinica` | Atualizar dados gerais, configurações e horário da clínica |
| EP-011 | `GET /clientes` | Pesquisar clientes |
| EP-012 | `GET /clientes/{id}` | Consultar cliente |
| EP-013 | `POST /clientes` | Criar conta de Cliente |
| EP-014 | `PUT /clientes/{id}` | Atualizar conta de Cliente |
| EP-015 | `GET /animais` | Pesquisar pacientes |
| EP-016 | `GET /animais/{id}` | Consultar ficha do paciente |
| EP-017 | `POST /animais` | Registar paciente |
| EP-018 | `PUT /animais/{id}` | Atualizar dados administrativos do paciente |
| EP-019 | `GET /me/animais` | Listar os meus animais (Cliente) |
| EP-020 | `GET /me/animais/{id}` | Consultar o meu animal (Cliente) |
| EP-021 | `GET /veterinarios` | Listar veterinários |
| EP-022 | `GET /veterinarios/{id}/disponibilidade` | Consultar disponibilidade de um veterinário |
| EP-023 | `GET /consultas` | Consultar agenda |
| EP-024 | `GET /consultas/{id}` | Consultar marcação |
| EP-025 | `POST /consultas` | Criar marcação |
| EP-026 | `PUT /consultas/{id}` | Alterar marcação |
| EP-027 | `PATCH /consultas/{id}/estado` | Atualizar estado da marcação |
| EP-028 | `PATCH /consultas/{id}/chegada` | Registar chegada |
| EP-029 | `POST /consultas/{id}/notas-preparacao` | Registar informação não clínica de apoio |
| EP-030 | `GET /me/consultas` | Consultar as minhas marcações (Cliente) |
| EP-031 | `POST /registos-clinicos` | Criar registo clínico |
| EP-032 | `PUT /registos-clinicos/{id}` | Atualizar registo clínico |
| EP-033 | `GET /registos-clinicos/{id}` | Consultar registo clínico |
| EP-034 | `GET /animais/{id}/historico-clinico` | Consultar histórico clínico do paciente |
| EP-042 | `GET /animais/{id}/vacinacoes` | Consultar boletim do paciente (web) |
| EP-043 | `POST /animais/{id}/vacinacoes` | Registar vacinação ou desparasitação |
| EP-044 | `GET /me/animais/{id}/vacinacoes` | Consultar boletim do meu animal (Cliente) |
| RF056 | `PUT /animais/{id}/vacinacoes/{administracaoId}` | Atualizar uma administração (só Veterinário); ainda sem número de EP (secção 6) |

## 3. Funcionalidades e operações não incluídas

| Não incluído | Operações | Motivo |
|---|---|---|
| F005 — Gestão de contas e pagamentos | Anteriores EP-035 a EP-041 (RF037–RF042) | Fora do âmbito desde a Declaração de Âmbito v05: transição autorizada pelo docente na reunião de 2026-10-20. Os números ficam reservados e não são reutilizados. |
| F007 — Gestão de internamento | Anteriores EP-045 a EP-051 (RF048–RF052) | Fora do âmbito desde a Declaração de Âmbito v05, pelo mesmo motivo. |

Também não fazem parte desta versão, por estarem fora do âmbito definido na Declaração de Âmbito v05 e na Documentação da API v02 (secção 11): autenticação com fornecedores externos ou multifator; marcação, alteração ou cancelamento de consultas pelo Cliente; criação ou edição de fichas de animais pelo Cliente; pagamentos eletrónicos e faturação certificada; notificações por SMS ou e-mail e sincronização com calendários; prescrições em PDF; integrações com o SIAC, bases de dados de microchips, equipamentos ou leitura de códigos; acesso do Cliente a registos clínicos; migração de dados; funcionamento offline.

## 4. Requisitos técnicos e alterações relevantes

Por ser a primeira versão do backend, não há alterações face a uma versão anterior; descrevem-se as características técnicas entregues.

| Tema | Implementação |
|---|---|
| Plataforma | ASP.NET Core em .NET 10 (`net10.0`), API REST com controladores |
| Persistência | Entity Framework Core 10.0.13 com SQL Server; esquema criado pelas migrações da I062; dados iniciais e de demonstração da I061 (`04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao`) |
| Autenticação | JWT HS256 com validade de 8 horas; fim de sessão com revogação do token (EP-002) e limpeza periódica das revogações expiradas |
| Autorização | Políticas por perfil (Administrador, Rececionista, Veterinário, Auxiliar Veterinário e Cliente) e por propriedade dos dados do Cliente; matriz de permissões da API v02 |
| Palavras-passe | Hash no formato do `PasswordHasher` do ASP.NET Core Identity (v3) e política de palavra-passe (regra R-PWD) |
| Concorrência | Versão da marcação no cabeçalho `ETag` (EP-024 a EP-028); `If-Match` opcional nas alterações de EP-026, EP-027 e EP-028, com 412 se a versão enviada estiver desatualizada; a política de CORS expõe o `ETag` |
| Erros | Formato único `code`, `message` e `detail` em todas as respostas de erro (middleware da I058) |
| Listas | Envelope de paginação comum (`itens`, `pagina`, `tamanho`, `total`) |
| Documentação | Swagger/OpenAPI (Swashbuckle 6.6.2) com autenticação Bearer, ativo em Development |
| Qualidade | Pipeline GitHub Actions «Backend CI» com os quality gates QG-01 a QG-04, QG-08 e a Collection Postman (I102, I103) |

## 5. Resultados dos testes e das verificações de qualidade

| Verificação | Resultado | Fonte |
|---|---|---|
| Testes unitários | 718 testes: 717 aprovados, 0 falhados, 1 ignorado com justificação (`UT-TRV-007`, defeito D-01); os 306 casos da Especificação dos Testes Unitários têm teste | Execução #170; relatório da I090 (PR #241, em auditoria; ainda não integrado) |
| Testes de integração | 463 testes: 463 aprovados, 0 falhados, 0 ignorados (relatório sobre `69e70c7`); job «3 · Testes de integração» com sucesso em `39040cd` | Relatório da I097 (`03.01.03-testes-de-integracao`), execução #170 |
| Collection Postman (Newman) | 269 pedidos e 862 verificações, sem falhas, cobrindo os 37 endpoints; job «7 · Collection Postman» com sucesso em `39040cd` | README de `03.01.05-postman` (execução de referência da I099), execução #170 |
| Cobertura (QG-03) | 98,5% das linhas e 92,8% dos ramos em Domínio e Serviços, com os testes unitários e de integração (limiar: 70%); só os unitários: 96,2% | Medida pela pipeline (QG-03) na revisão da I103 e no relatório da I090; o relatório formal de cobertura é produzido na I032, ainda não iniciada |
| Análise estática (QG-04) | 0 avisos e 0 erros; 54 sugestões `info` justificadas | Relatório da I034 (`03.01.06-analise-estatica`) |
| Formatação (QG-08) | 0 ficheiros por formatar | Execução #170, job 6 |
| Matriz de permissões | 228 verificações endpoint × perfil (incluindo o anónimo), todas aprovadas | I096, relatório da I097 |
| Pipeline | Execução #170 completa e com sucesso; bloqueio do merge por falha demonstrado em cada quality gate | Execução #170; I103 |

Não há defeitos de severidade Crítica ou Alta em aberto.

## 6. Limitações conhecidas

| Ref. | Limitação | Impacto | Seguimento |
|---|---|---|---|
| D-01 | Numa exceção inesperada (500), a resposta inclui a mensagem interna da exceção em `detail` | Média: pode expor detalhes internos num erro inesperado | Sem Issue de defeito; o teste `UT-TRV-007` está ignorado até à correção |
| D-02 | Validação do e-mail (detetado nos testes unitários da I086) | Média | Sem Issue de defeito |
| N1 | Palavra-passe inicial do Cliente (detetado na I086) | Média | Sem Issue de defeito |
| Relógio real | Testes de integração do boletim dependem da data real e falham a partir de 2027-01-13; não afeta a API | Média (testes) | Proposta de Issue de defeito para a infraestrutura de testes (I091) |
| D1 (formato de erro) | A API v02 indica Problem Details (RFC 9457); a implementação usa o formato único `code`/`message`/`detail` da I058 | Baixa (documentação) | Alinhar a API v03 |
| 409 em alterações simultâneas | Em EP-026 a EP-028, duas gravações simultâneas sem `If-Match` devolvem 412 e não o 409 da API v02 (secção 5.1); só uma é gravada, como previsto | Baixa (documentação) | Registado para a API v03 (D1 da I093) |
| RF056 | A atualização de uma administração do boletim está implementada mas ainda não tem número de EP na API v02 | Baixa (documentação) | Ponto em aberto 2 da API v02, previsto para a v03 |
| Swagger | O Swagger só está ativo em Development | Baixa | Ativação por configuração no ambiente público, na I104 (em revisão) |

## 7. Compatibilidade e pré-requisitos

| Elemento | Versão / requisito |
|---|---|
| SDK do .NET | .NET SDK 10 (`src/backend/global.json`: `10.0.100` com `rollForward: latestFeature`) |
| Runtime | ASP.NET Core 10 |
| Base de dados | SQL Server 2025 (usado nos testes de integração e na pipeline) ou Azure SQL Database; fornecedor `Microsoft.EntityFrameworkCore.SqlServer` 10.0.13 |
| Ferramenta de migrações | `dotnet-ef` (10.0.x) |
| Dados de demonstração | `sqlcmd` (SQL Server Command Line Utilities ou go-sqlcmd) |
| Testes de integração | Docker (Testcontainers 4.16.0 cria um SQL Server por classe de testes) |
| Collection Postman | Node.js e Newman 6.2.1 |
| Clientes da API | Frontend web e aplicação móvel segundo a Documentação da API v02 (JSON, autenticação Bearer, cabeçalhos `ETag`/`If-Match` nas marcações); a origem do frontend web é autorizada em `Cors:AllowedOrigins` |

Configuração necessária (por `appsettings`, *user secrets* ou variáveis de ambiente; os segredos nunca vão para o repositório):

| Definição | Obrigatória | Descrição |
|---|---|---|
| `ConnectionStrings:ClinicaDb` | Sim | Cadeia de ligação ao SQL Server |
| `Jwt:Key` | Sim | Chave de assinatura em Base64, com pelo menos 32 bytes; a API não arranca sem ela |
| `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpirationHours` | Já definidas em `appsettings.json` | Emissor, audiência e validade (8 h) |
| `Cors:AllowedOrigins` | Não | Origens autorizadas; vazio permite qualquer origem |
| `LimpezaTokensRevogados:IntervaloMinutos` | Já definida (60) | Intervalo da limpeza das revogações |

## 8. Instalação

A partir de um clone do repositório, na pasta `src/backend`:

1. **Compilar:**

   ```
   dotnet restore ClinicaVeterinaria.sln
   dotnet build ClinicaVeterinaria.sln --configuration Release
   ```

2. **Configurar a chave JWT** (em desenvolvimento, com *user secrets*; ver `src/backend/README.md`, secção «Configurar a chave JWT em desenvolvimento»):

   ```
   dotnet user-secrets set "Jwt:Key" "<chave Base64 de pelo menos 32 bytes>" --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj
   ```

   e, se a base de dados não for a local por omissão, a cadeia de ligação `ConnectionStrings:ClinicaDb`.

3. **Criar a base de dados** com as migrações da I062 (apaga e recria a base indicada):

   ```
   .\scripts\reposicao-base-dados.ps1 -ConnectionString "<cadeia de ligação>"
   ```

4. **Carregar os dados de demonstração** da I061 (na pasta `04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao`; ver o README dessa pasta):

   ```
   .\executar-dados.ps1 -Servidor "<servidor>" -BaseDados "<base de dados>" -ConfiarCertificado
   ```

   O registo tem de terminar com «28 verificações, 28 OK, 0 falhadas». As palavras-passe das contas de demonstração não estão no repositório: estão no ficheiro de credenciais da submissão.

5. **Executar a API:**

   ```
   dotnet run --project ./ClinicaVeterinaria.Api/ClinicaVeterinaria.Api.csproj --launch-profile http
   ```

   A API fica em `http://localhost:5159`; em Development, o Swagger está em `http://localhost:5159/swagger`.

6. **Verificar:** iniciar sessão com uma conta de demonstração (EP-001) e, opcionalmente, executar a Collection Postman com o Newman (README de `03.01.05-postman`).

## 9. Atualização

Esta é a primeira versão do backend; não há instalações anteriores a atualizar. Para versões seguintes:

1. Obter o código da nova versão (`git pull` ou checkout da etiqueta da milestone).
2. Ler as notas de release da nova versão, em especial as definições de configuração novas ou alteradas.
3. Compilar (passo 1 da secção 8).
4. Aplicar as migrações novas sem apagar os dados:

   ```
   dotnet ef database update --project ClinicaVeterinaria.Infrastructure --startup-project ClinicaVeterinaria.Api --context ClinicaDbContext
   ```

5. Reiniciar a API e repetir a verificação (passo 6 da secção 8).

## 10. Referências

- Documentação da API v02 — `04-artefactos-tecnicos/04.11-documentacao-de-api/m3-documentacao-api-v02.md`
- Declaração de Âmbito v05 — `04-artefactos-tecnicos/04.01-proposta-de-sistema-e-declaracao-de-ambito/m3-declaracao-ambito-v05.pdf`
- README do backend — `src/backend/README.md`
- Relatório de resultados dos testes unitários (I090) — `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.02-testes-unitarios` (PR #241, ainda não integrado)
- Relatório de cobertura do backend (I032) — `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.04-cobertura-de-testes` (a produzir)
- Relatório de resultados dos testes de integração (I097) — `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.03-testes-de-integracao`
- Relatório de análise estática (I034) — `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.06-analise-estatica`
- Collection Postman (I098 a I100) — `03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman`
- Pipeline do backend (I102, I103) — `04-artefactos-tecnicos/04.16-ci-cd`

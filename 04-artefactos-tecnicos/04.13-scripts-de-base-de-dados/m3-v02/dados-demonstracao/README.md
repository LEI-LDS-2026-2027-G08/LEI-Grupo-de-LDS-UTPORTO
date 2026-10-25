# Dados iniciais e de demonstração — m3-v02

| Campo | Valor |
|---|---|
| Projeto | Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027 |
| Issue | I061 — Escrever os scripts de dados iniciais e de demonstração |
| Milestone / Sprint | M3 / Sprint 04 |
| Executor / Revisor / Auditor | Gonçalo Magalhães Ribeiro / André Filipe Moreira Mesquita / Gonçalo Dinis Magalhães Teixeira |
| Pasta | `/projeto/04-artefactos-tecnicos/04.13-scripts-de-base-de-dados/m3-v02/dados-demonstracao` |
| Esquema | Scripts `m3-v02` (16 tabelas) ou base criada pelas migrações da I059, com os dados de referência do `HasData` da I062. Os dados de demonstração usam 15 dessas tabelas (todas exceto `TokenRevogado`) |
| Base de dados | Indicada na execução (`-BaseDados` / `--base-dados`, passada ao `sqlcmd` com `-d`); os scripts não fixam o nome. Por omissão: `ClinicaVeterinaria` |
| Estado | Executado pelo Executor em 2026-10-25 (SQL Server 2025 local): reexecução com 0 linhas inseridas e 28 verificações, 28 OK. Registo anexado ao Pull Request. Aguarda revisão |

## 1. Decisões que esta Issue segue

| Assunto | Decisão | Fonte |
|---|---|---|
| Pasta | `m3-v02/dados-demonstracao`, e não `m3-v01` como indica o Backlog v02. A `m3-v01` tem o esquema de 22 tabelas, que foi substituído. Os dados ficam numa subpasta para o `executar-scripts` da `m3-v02` (que só corre `00-` a `06-`) e os testes de restrições (que exigem uma base vazia) continuarem iguais | Modelo de Dados v02, 9.1; a registar no relatório de revisão |
| Dados de referência | `Clinica`, `Especie`, `Raca` e `ConfiguracaoSistema` pertencem ao `HasData` das migrações da I062. Os dados de demonstração não os inserem: procuram espécies e raças **pelo nome**. Exigem a clínica (Id 1) e as espécies «Cão» e «Gato» e param com uma mensagem clara se faltarem; uma raça que não exista deixa o animal sem raça (`RacaId` nulo). **Nomes a acordar com a I062** | Modelo de Dados v02, 9.3 (opção a) |
| Base criada pelos scripts `m3-v02` | Enquanto não houver migrações, `01-dados-referencia-provisorios.sql` cria essas linhas, só se ainda não existirem. Não se usa numa base criada pelas migrações | Idem |
| Palavras-passe | Só o hash fica na base, no formato do `PasswordHasher` do ASP.NET Core Identity (v3: PBKDF2-HMAC-SHA512, 100 000 iterações, sal de 128 bits). O `PasswordHasher` lê os parâmetros do próprio hash. **A confirmar com a I063:** a API tem de verificar as palavras-passe com o `PasswordHasher` | Modelo de Dados v02, 7.1; critério 4 |
| Credenciais em claro | Apenas em `/projeto/00-indice-e-submissao/m3-credenciais-avaliacao-v01.md`, entregue no ZIP de submissão e não publicado no repositório (Regulamento da UC, secções 18.4 e 19). Contas temporárias, revogadas depois da avaliação | Critério 4 |
| Nome da base de dados | Sem `USE` nos scripts: a base é a da ligação, para correrem em bases isoladas (testes de integração da I062, contentores, pipeline). Os scripts recusam correr numa base de sistema (`master`, `model`, `msdb`, `tempdb`) | Integração com a I062 |
| F005 e F007 | As partes dos critérios 3 e 5 sobre contas com pagamento parcial, internamento e alojamentos não se aplicam: as tabelas não existem no esquema `m3-v02` | Declaração de Âmbito v05; Modelo de Dados v02, 9.3 |

## 2. Conteúdo

| Ordem | Ficheiro | Finalidade |
|:---:|---|---|
| 1 | `01-dados-referencia-provisorios.sql` | Apoio provisório: dados de referência numa base criada pelos scripts `m3-v02` |
| 2 | `02-dados-demonstracao.sql` | Dados iniciais e de demonstração (secção 3) |
| — | `verificar-dados-demonstracao.sql` | 28 verificações (D01 a D21; D01 tem uma por perfil e D09 uma por estado) dos critérios 2 a 4 e da coerência dos dados; só leitura |
| — | `executar-dados.ps1` / `executar-dados.sh` | Executam por ordem, correm o `02` duas vezes (prova de reexecução), a verificação e guardam o registo `dados-execucao-AAAAMMDD-HHMMSS.log` |

## 3. Dados criados

| Tabela | Linhas | Conteúdo |
|---|---:|---|
| `Utilizador` | 10 | Administrador (conta inicial), Rececionista, Rececionista **bloqueado**, 2 Veterinários, Auxiliar **com** e Auxiliar **sem** autorização de apoio, 3 Clientes |
| `Funcionario` | 7 | Uma linha por conta interna |
| `HorarioFuncionamento` | 11 | Segunda a sexta 09:00–13:00 e 14:00–19:00; sábado 09:00–13:00 (como no exemplo de EP-009) |
| `Cliente` | 3 | João (dois animais), Maria e Rui (sem NIF) |
| `Paciente` | 4 | Bobi (cão), Mia (gato), Kiko (cão sem raça definida) e Luna (gato) |
| `Marcacao` | 5 | M1 **Realizada** com chegada, M2 **Cancelada**, M3 **Confirmada**, M4 e M5 **Pendentes**; sem sobreposições por Veterinário e dentro do horário |
| `InformacaoApoio` | 1 | Registada pela Auxiliar com autorização, na M1 |
| `RegistoClinico`, `Tratamento`, `Prescricao` | 1 + 1 + 1 | Consulta M1 (otite externa) |
| `RegistoProfilaxia` | 4 | Boletim do Bobi (duas vacinas e uma desparasitação, com próxima administração calculada) e desparasitação do Kiko |

As datas são fixas e em UTC (outubro e novembro de 2026), para que uma reexecução nunca duplique dados. Todos os nomes têm «(demonstração)» ou «(fictício)», os e-mails usam o domínio reservado `.test` e os NIF e telefones são fictícios.

## 4. Execução

Numa base criada pelos scripts `m3-v02` (Windows, autenticação do Windows):

```powershell
cd ...\04.13-scripts-de-base-de-dados\m3-v02
powershell -ExecutionPolicy Bypass -File .\executar-scripts.ps1 -Servidor "." -ConfiarCertificado -Verificar -Testar
cd .\dados-demonstracao
powershell -ExecutionPolicy Bypass -File .\executar-dados.ps1 -Servidor "." -ConfiarCertificado -DadosReferenciaProvisorios
```

Numa base criada pelas migrações da I062, sem `-DadosReferenciaProvisorios`. Para outra base, indicar o nome:

```powershell
powershell -ExecutionPolicy Bypass -File .\executar-dados.ps1 -Servidor "." -ConfiarCertificado -BaseDados "ClinicaVeterinaria_I062_Test"
```

Em Linux ou na pipeline: `SQLCMDPASSWORD=... ./executar-dados.sh --base-dados ClinicaVeterinaria_I062_Test --confiar-certificado`.

O registo tem de mostrar: primeira execução do `02` com linhas inseridas, **segunda execução com «0 linha(s) inserida(s)» em todas as tabelas** (critério 1) e «Verificação dos dados de demonstração: 28 verificações, 28 OK, 0 falhadas».

## 5. Critérios de aceitação e evidências

| Critério da I061 | Como é cumprido | Evidência |
|---|---|---|
| 1. Executam sem erros depois do esquema e podem ser reexecutados sem duplicar | Inserções condicionadas pela chave natural, numa transação | Registo `dados-execucao-20261025-204058.log`, anexado ao Pull Request (segunda execução com 0 linhas) |
| 2. Conta ativa por perfil, conta bloqueada, Auxiliar com e sem apoio | Secção 3 | D01 a D06 |
| 3. Clientes com mais de um animal, marcações em estados diferentes, registo clínico, boletim | Secção 3. **Não aplicável:** contas com pagamento parcial e internamento (F005/F007 fora do âmbito) | D08 a D14 |
| 4. Hash igual ao da API; credenciais no ficheiro de avaliação | `PasswordHasher` v3 (a confirmar com a I063); `m3-credenciais-avaliacao-v01.md` | D07 e login com cada conta na API (verificação 1 do Revisor) |
| 5. Alojamentos suficientes | **Não aplicável** (F007 fora do âmbito) | — |

O Revisor confirma ainda a coerência referencial (D15 a D20) e a ausência de dados pessoais reais (D21).

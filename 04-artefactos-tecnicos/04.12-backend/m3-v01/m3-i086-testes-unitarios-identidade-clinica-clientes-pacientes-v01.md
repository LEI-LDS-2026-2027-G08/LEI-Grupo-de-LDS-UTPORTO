# I086 — Testes Unitários de Identidade, Clínica, Clientes e Pacientes

| Campo     | Valor                                                                                     |
| --------- | ----------------------------------------------------------------------------------------- |
| Issue     | I086 — Implementar os testes unitários de identidade, clínica, Clientes e pacientes (#93) |
| Sprint    | S05                                                                                       |
| Milestone | M3                                                                                        |
| Executor  | Ivan Cristiano Silva Mendes                                                               |
| Revisor   | André Filipe Moreira Mesquita                                                             |
| Auditor   | Gonçalo Dinis Magalhães Teixeira                                                          |
| Data      | 2026-11-01                                                                                |
| Estado    | Implementação do Executor concluída; a submeter para revisão                              |

## 1. Objetivo

Implementar os testes unitários das regras de F001 e F002 e dos casos transversais de paginação, erros e registos, com o identificador de cada caso da Especificação de Testes Unitários (I028) no nome do teste, sem base de dados real, rede nem relógio real.

## 2. Documentos de referência

| Documento                                           | Versão                              |
| --------------------------------------------------- | ----------------------------------- |
| Especificação de Testes Unitários do Backend (I028) | v01 (secções 4, 5, 7.1, 7.2 e 7.6)  |
| Documentação da API                                 | v02                                 |
| Plano de Testes do Backend                          | v02                                 |
| Backlog de Projeto (critérios da I086)              | v03                                 |

## 3. Âmbito

A I028 atribui à I086 **165 casos**: UT-F001-001 a UT-F001-125 (autenticação, JWT, políticas, palavra-passe, NIF, e-mail, contas de funcionários, clínica e Clientes), UT-F002-001 a UT-F002-030 (pacientes e idade) e UT-TRV-001 a UT-TRV-010 (paginação, erros e registos).

Antes da I086 já existiam 38 destes casos com identificador (I064, I065, I066 e I072) e testes sem identificador para o JWT, as políticas, a palavra-passe e o NIF. A I086 acrescentou o identificador a esses testes (30 casos), escreveu os 97 casos que faltavam e removeu o `UnitTest1` gerado pelo modelo do projeto, que era um teste vazio, sem asserções.

## 4. Ficheiros

Todos em `src/backend/ClinicaVeterinaria.UnitTests`:

| Ficheiro | Casos | Origem |
| --- | --- | --- |
| `Seguranca/AuthServiceTests.cs` | UT-F001-001 a 015 e 017 a 020 | Novo |
| `Seguranca/DuplosAutenticacao.cs` | — (duplos do hasher, do emissor de tokens e do logger) | Novo |
| `Seguranca/AuthServiceFimSessaoTests.cs` | UT-F001-016, 021 e 022 | I064, sem alterações |
| `Seguranca/JwtTokenServiceTests.cs` | UT-F001-023 a 034 | I063; identificadores, e casos 027, 030, 033 e 034 novos |
| `Seguranca/JwtOptionsValidatorTests.cs` | UT-F001-035 a 039 | Novo |
| `Seguranca/PoliticasAutorizacaoTests.cs` | UT-F001-040 a 049 | I063; reorganizado (um caso por política, uma linha por perfil) |
| `Validacao/PalavraPasseForteAttributeTests.cs` | UT-F001-050 a 059 | I065; identificadores, e casos 051, 053 e 059 novos |
| `Validacao/NifValidoAttributeTests.cs` | UT-F001-060 a 066 | I067/I068; identificadores, e casos 062 e 066 novos |
| `Validacao/EmailValidoAttributeTests.cs` | UT-F001-067 a 072 | I065, sem alterações |
| `Utilizadores/UtilizadoresServiceTests.cs` | UT-F001-073 a 099 e UT-TRV-001 | I065/I066, sem alterações |
| `DadosClinica/ClinicaServiceTests.cs` | UT-F001-100 a 111 | Novo |
| `Clientes/ClientesServiceTests.cs` | UT-F001-112 a 125 e UT-TRV-005 | Novo |
| `Animais/AnimaisServiceTests.cs` | UT-F002-001 a 030 | Novo |
| `Consultas/ConsultaFiltroQueryTests.cs` | UT-TRV-001 e 002 | I072, sem alterações |
| `Validacao/PaginacaoTests.cs` | UT-TRV-003 e 004 | Novo |
| `Middleware/ErrosERegistosTests.cs` | UT-TRV-006 a 010 | Novo |
| `UnitTest1.cs` | — | Removido (teste vazio do modelo do projeto) |

Não há alterações ao código de produção, ao esquema nem aos pacotes do projeto de testes.

## 5. Decisões de implementação

1. **Duplos.** Os serviços que usam `ClinicaDbContext` são testados com o EF Core InMemory, como os testes que já existiam (I028, secção 4.2). O `IPasswordHasher<Utilizador>`, o `IJwtTokenService` e o `ILogger` são duplos escritos à mão (`DuplosAutenticacao.cs`): o projeto não usa Moq e a I028 deixa a escolha em aberto. Os duplos devolvem o resultado configurado e guardam as chamadas, para se verificar, por exemplo, que o hasher é chamado uma vez com um e-mail inexistente (UT-F001-007) ou que nenhum token é emitido para uma conta bloqueada (UT-F001-009).
2. **Hora injetada.** Todas as unidades recebem `TimeProvider`; os testes usam o `RelogioFixo` com `RELOGIO = 2026-10-14T08:00:00Z` (I028, secção 5) ou com o dia indicado no caso (idade, UT-F002-025 a 029).
3. **Regras de formato no pedido.** As regras que a API aplica na validação automática do `[ApiController]` (por exemplo, `clienteId` em falta, nome com mais de 80 caracteres, NIF ou e-mail inválidos, `duracaoConsultaMinutos = 0`) são verificadas no DTO com `Validator.TryValidateObject`, como nos testes da I065. As regras que estão no serviço são verificadas no serviço.
4. **Unidades com outro nome.** A I028 propõe uma `CalculadoraIdade` e uma `PaginacaoQuery` comum (secção 6, nomes não normativos). No código, a idade é calculada dentro do `AnimaisService`, por isso os casos UT-F002-025 a 030 verificam a idade através da ficha do animal, com o dia de hoje injetado. Cada filtro tem os seus limites de paginação, por isso UT-TRV-003 e 004 usam o `AnimalFiltroQuery` (EP-015).
5. **Um motivo de falha por teste.** Os testes que juntavam verificações de casos diferentes da I028 foram separados (por exemplo, o algoritmo HS256 passou do UT-F001-023 para o UT-F001-027, e cada política tem o seu caso).

## 6. Divergências, defeitos e notas

### 6.1. Divergências face à I028 (os testes seguem o código; decisão do grupo pendente)

- **D1 — mensagem da palavra-passe curta (UT-F001-052).** A I028 pede «A palavra-passe deve ter pelo menos 8 caracteres.». O `PalavraPasseForteAttribute` (I065) tem uma única mensagem para todas as regras («entre 8 e 128 caracteres, com…»). O teste verifica que a palavra-passe é recusada, não o texto.
- **D2 — horário vazio (UT-F001-110).** A I028 pede 400 para `horarioFuncionamento = []`. O código (I067) aceita a lista vazia e trata-a como clínica fechada; só a lista em falta (`null`) dá 400. O contrato de EP-010 na API v02 não prevê o caso. O teste verifica o comportamento atual e há um teste próprio para `null`.
- **D3 — idade sem data de nascimento (UT-F002-030).** A I028 propõe 0 (ponto em aberto PA-09). O código devolve `idadeAnos` vazio (`int?`), como já foi registado na revisão da I070 (melhoria M2). O teste verifica o comportamento atual.

### 6.2. Defeitos encontrados

- **D-01 — o 500 expõe a mensagem interna (UT-TRV-007, I028 PA-07).** O `ExceptionHandlingMiddleware` copia para `detail` a mensagem de qualquer exceção, também quando a resposta é 500: uma exceção inesperada com «segredo interno» chega ao cliente. O teste foi escrito como a I028 o descreve e está ignorado, com a justificação no atributo `Skip`; sem o `Skip` falha com `Assert.DoesNotContain() Failure: Sub-string found … "detail":"segredo interno"`. Correção proposta (fora do âmbito de uma tarefa de testes): no caso `_ => InternalServerError`, usar em `detail` uma mensagem genérica. **Ação:** abrir uma Issue de defeito e reativar o teste quando for corrigido.
- **D-02 — regra R-EMAIL só em EP-006 (fora dos casos da I028).** Na I065 o `UpdateUtilizadorDto` passou a usar `[EmailValido]`, porque o `[EmailAddress]` do .NET aceita, por exemplo, `joao@mail`. O `UpdateClienteDto` (EP-013 e EP-014) e o `UpdateClinicaDto` (EP-010) continuam com `[EmailAddress]` e aceitam esse e-mail, embora a I028 aplique a R-EMAIL também a EP-013. O caso da I028 para Clientes (UT-F001-115, `"joao"`) passa com as duas regras. **Ação:** registar para decisão; a correção é trocar o atributo.

### 6.3. Notas

- **N1 — palavra-passe inicial do Cliente.** O `CreateClienteDto` só verifica o comprimento (8 a 128) e não a regra R-PWD completa, ao contrário de EP-006. O contrato de EP-013 na API v02 não prevê erro de palavra-passe fraca, por isso nenhum caso da I028 o verifica; fica para decisão do grupo.
- **N2 — «uma única gravação» (UT-F001-112).** O `ClientesService` grava o `Utilizador` e o `Cliente` em dois `SaveChanges` dentro de uma transação. O InMemory ignora transações (I028, secção 4.2), por isso o teste verifica o resultado (conta e Cliente ligados); a atomicidade é verificada nos testes de integração (IT-EP013-01 e 02, I092).
- **N3 — sem consultas à base de dados (UT-F001-003).** Para confirmar que a validação vem antes de qualquer consulta, o serviço recebe um contexto já libertado: qualquer consulta falharia.
- **N4 — exceção inesperada (UT-TRV-007).** A I028 usa uma `NullReferenceException`; o analisador (CA2201) não deixa criá-la num teste, por isso usa-se uma `NotSupportedException`, que o middleware também não conhece e que dá igualmente 500.
- **N5 — validade do token (UT-F001-029 e 030).** A validação pela configuração da API usa a hora atual. Os tokens são emitidos com um relógio 8 h 01 min e 7 h 59 min antes da hora atual, por isso o resultado só depende da idade do token (margem de 1 minuto).

## 7. Correspondência caso da especificação ↔ teste

Legenda: **AUT** `AuthServiceTests`, **FIM** `AuthServiceFimSessaoTests`, **JWT** `JwtTokenServiceTests`, **OPT** `JwtOptionsValidatorTests`, **POL** `PoliticasAutorizacaoTests`, **PWD** `PalavraPasseForteAttributeTests`, **NIF** `NifValidoAttributeTests`, **EML** `EmailValidoAttributeTests`, **UTI** `UtilizadoresServiceTests`, **CLI** `ClinicaServiceTests`, **CLT** `ClientesServiceTests`, **ANI** `AnimaisServiceTests`, **CON** `ConsultaFiltroQueryTests`, **PAG** `PaginacaoTests`, **ERR** `ErrosERegistosTests`.

| Caso I028 | Ficheiro | Teste | Origem | Nota |
| --- | --- | --- | --- | --- |
| **7.1.1. Autenticação e sessão `AuthService`** | | | | |
| UT-F001-001 | AUT | `LoginDoAdministradorDevolveTokenERegistaUltimoAcesso` | Novo (I086) |  |
| UT-F001-002 | AUT | `LoginDeClienteDevolveClienteId` | Novo (I086) |  |
| UT-F001-003 | AUT | `EmailVazioDevolve400SemConsultas` | Novo (I086) | N3 |
| UT-F001-004 | AUT | `PalavraPasseVaziaDevolve400` | Novo (I086) |  |
| UT-F001-005 | AUT | `EmailSoComEspacosContaComoEmFalta` | Novo (I086) |  |
| UT-F001-006 | AUT | `EmailComEspacosAVoltaAutentica` | Novo (I086) |  |
| UT-F001-007 | AUT | `EmailInexistenteDevolve401ComHashDeReferencia` | Novo (I086) |  |
| UT-F001-008 | AUT | `PalavraPasseErradaDevolve401SemToken` | Novo (I086) |  |
| UT-F001-009 | AUT | `ContaBloqueadaComPalavraPasseCertaDevolve403` | Novo (I086) |  |
| UT-F001-010 | AUT | `ContaBloqueadaComPalavraPasseErradaDevolve401` | Novo (I086) |  |
| UT-F001-011 | AUT | `RehashNecessarioGravaNovoHash` | Novo (I086) |  |
| UT-F001-012 | AUT | `RegistoDaTentativaNaoTemAPalavraPasse` | Novo (I086) |  |
| UT-F001-013 | AUT | `SessaoDeContaAtivaEValida` | Novo (I086) |  |
| UT-F001-014 | AUT | `ContaBloqueadaDepoisDoTokenInvalidaASessao` | Novo (I086) |  |
| UT-F001-015 | AUT | `PerfilAlteradoInvalidaASessao` | Novo (I086) |  |
| UT-F001-016 | FIM | `TokenComJtiRevogadoNaoESessaoValida` | Já existia (I064) |  |
| UT-F001-017 | AUT | `UtilizadorInexistenteNaoTemSessao` | Novo (I086) |  |
| UT-F001-018 | AUT | `VeterinarioAutenticadoTemNomeDoFuncionario` | Novo (I086) |  |
| UT-F001-019 | AUT | `ClienteAutenticadoTemClienteIdENome` | Novo (I086) |  |
| UT-F001-020 | AUT | `ContaBloqueadaNoAuthMeDevolve401` | Novo (I086) |  |
| UT-F001-021 | FIM | `FimDeSessaoRegistaRevogacaoAteExpiracao` | Já existia (I064) |  |
| UT-F001-022 | FIM | `FimDeSessaoRepetidoEIdempotente` | Já existia (I064) |  |
| **7.1.2. Emissão e validação do token `JwtTokenService`** | | | | |
| UT-F001-023 | JWT | `TokenDeClienteTemAsClaimsDoContrato` | Já existia; ID acrescentado (I086) |  |
| UT-F001-024 | JWT | `TokenDeVeterinarioTemVeterinarioIdESemClienteId` | Já existia; ID acrescentado (I086) |  |
| UT-F001-025 | JWT | `TokenExpiraOitoHorasDepoisDaEmissao` | Já existia; ID acrescentado (I086) |  |
| UT-F001-026 | JWT | `CadaTokenTemUmJtiDiferente` | Já existia; ID acrescentado (I086) |  |
| UT-F001-027 | JWT | `TokenUsaHs256` | Novo (I086) |  |
| UT-F001-028 | JWT | `TokenAtualEAceitePelaConfiguracaoDaApi` | Já existia; ID acrescentado (I086) |  |
| UT-F001-029 | JWT | `TokenExpiradoERejeitado` | Já existia; ID acrescentado (I086) | N5 |
| UT-F001-030 | JWT | `TokenQuaseAExpirarAindaEAceite` | Novo (I086) | N5 |
| UT-F001-031 | JWT | `TokenAssinadoComOutraChaveERejeitado` | Já existia; ID acrescentado (I086) |  |
| UT-F001-032 | JWT | `TokenAdulteradoERejeitado` | Já existia; ID acrescentado (I086) |  |
| UT-F001-033 | JWT | `TokenSemAssinaturaERejeitado` | Novo (I086) |  |
| UT-F001-034 | JWT | `TokenDeOutroEmissorERejeitado` | Novo (I086) |  |
| **7.1.3. Configuração JWT `JwtOptionsValidator`** | | | | |
| UT-F001-035 | OPT | `ChaveVaziaFalhaEIndicaOsUserSecrets` | Novo (I086) |  |
| UT-F001-036 | OPT | `ChaveCom31BytesFalha` | Novo (I086) |  |
| UT-F001-037 | OPT | `ConfiguracaoCompletaEAceite` | Novo (I086) |  |
| UT-F001-038 | OPT | `ChaveForaDeBase64Falha` | Novo (I086) |  |
| UT-F001-039 | OPT | `ValidadeZeroFalha` | Novo (I086) |  |
| **7.1.4. Políticas de autorização por perfil e de propriedade ** | | | | |
| UT-F001-040 | POL | `PoliticaAdministrador` (5 casos) | Já existia; ID acrescentado (I086) |  |
| UT-F001-041 | POL | `PoliticaRececionista` (5 casos) | Já existia; ID acrescentado (I086) |  |
| UT-F001-042 | POL | `PoliticaVeterinario` (5 casos) | Já existia; ID acrescentado (I086) |  |
| UT-F001-043 | POL | `PoliticaAuxiliarVeterinario` (5 casos) | Já existia; ID acrescentado (I086) |  |
| UT-F001-044 | POL | `PoliticaCliente` (5 casos) | Já existia; ID acrescentado (I086) |  |
| UT-F001-045 | POL | `PoliticaFuncionario` (5 casos) | Já existia; ID acrescentado (I086) |  |
| UT-F001-046 | POL | `PoliticaDePropriedadeAceitaClienteComClienteId` | Já existia; ID acrescentado (I086) |  |
| UT-F001-047 | POL | `PoliticaDePropriedadeRejeitaClienteSemClienteId` | Já existia; ID acrescentado (I086) |  |
| UT-F001-048 | POL | `PoliticaDePropriedadeRejeitaFuncionariosMesmoComClienteId` (4 casos) | Já existia; ID acrescentado (I086) |  |
| UT-F001-049 | POL | `SemAutenticacaoAPoliticaPorOmissaoRecusaOAcesso` | Já existia; ID acrescentado (I086) |  |
| **7.1.5. Palavra-passe `ValidadorPalavraPasse` (regra R-PWD)** | | | | |
| UT-F001-050 | PWD | `ExemploDaApiEValido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-051 | PWD | `OitoCaracteresEValido` | Novo (I086) |  |
| UT-F001-052 | PWD | `SeteCaracteresEInvalido` | Já existia; ID acrescentado (I086) | D1 |
| UT-F001-053 | PWD | `CentoEVinteEOitoCaracteresEValido` | Novo (I086) |  |
| UT-F001-054 | PWD | `CentoEVinteENoveCaracteresEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-055 | PWD | `SemMaiusculaEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-056 | PWD | `SemMinusculaEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-057 | PWD | `SemAlgarismoEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-058 | PWD | `SemSimboloEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-059 | PWD | `PalavraPasseEmFaltaEInvalida` (2 casos) | Novo (I086) |  |
| **7.1.6. NIF `ValidadorNif` (regra R-NIF)** | | | | |
| UT-F001-060 | NIF | `RestoUmDaControloZero` | Já existia; ID acrescentado (I086) |  |
| UT-F001-061 | NIF | `RestoDoisDaControloNove` | Já existia; ID acrescentado (I086) |  |
| UT-F001-062 | NIF | `DigitoDeControloErradoEInvalido` | Novo (I086) |  |
| UT-F001-063 | NIF | `OitoAlgarismosEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-064 | NIF | `DezAlgarismosEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-065 | NIF | `CaracteresNaoNumericosEInvalido` | Já existia; ID acrescentado (I086) |  |
| UT-F001-066 | NIF | `EspacosNoMeioEInvalido` | Novo (I086) |  |
| **7.1.7. E-mail `ValidadorEmail` (regra R-EMAIL)** | | | | |
| UT-F001-067 | EML | `EmailComPontoNoDominioEValido` | Já existia (I065) |  |
| UT-F001-068 | EML | `EmailSemArrobaEInvalido` | Já existia (I065) |  |
| UT-F001-069 | EML | `EmailSemPontoNoDominioEInvalido` | Já existia (I065) |  |
| UT-F001-070 | EML | `EmailComEspacosEInvalido` | Já existia (I065) |  |
| UT-F001-071 | EML | `EmailCom256CaracteresEValido` | Já existia (I065) |  |
| UT-F001-072 | EML | `EmailCom257CaracteresEInvalido` | Já existia (I065) |  |
| **7.1.8. Contas de funcionários `UtilizadoresService`** | | | | |
| UT-F001-073 | UTI | `CriarAuxiliarComApoioCriaContaAtivaEFuncionario` | Já existia (I065/I066) |  |
| UT-F001-074 | UTI | `CriarVeterinarioSemApoio` | Já existia (I065/I066) |  |
| UT-F001-075 | UTI | `PerfilClienteERecusado` | Já existia (I065/I066) |  |
| UT-F001-076 | UTI | `RececionistaComApoioERecusado` | Já existia (I065/I066) |  |
| UT-F001-077 | UTI | `EmailJaRegistadoDaConflito` | Já existia (I065/I066) |  |
| UT-F001-078 | UTI | `NomeVazioERecusado` | Já existia (I065/I066) |  |
| UT-F001-079 | UTI | `EmailInvalidoERecusado` | Já existia (I065/I066) |  |
| UT-F001-080 | UTI | `PalavraPasseFracaERecusada` | Já existia (I065/I066) |  |
| UT-F001-081 | UTI | `NomeCom150CaracteresEAceite` | Já existia (I065/I066) |  |
| UT-F001-082 | UTI | `NomeCom151CaracteresERecusado` | Já existia (I065/I066) |  |
| UT-F001-083 | UTI | `AtualizarRececionistaParaVeterinario` | Já existia (I065/I066) |  |
| UT-F001-084 | UTI | `AtualizarParaPerfilClienteERecusado` | Já existia (I065/I066) |  |
| UT-F001-085 | UTI | `AdministradorNaoRemoveOProprioPerfil` | Já existia (I065/I066) |  |
| UT-F001-086 | UTI | `AdministradorAlteraOsPropriosDados` | Já existia (I065/I066) |  |
| UT-F001-087 | UTI | `ApoioNumVeterinarioERecusado` | Já existia (I065/I066) |  |
| UT-F001-088 | UTI | `EmailDeOutraContaDaConflito` | Já existia (I065/I066) |  |
| UT-F001-089 | UTI | `ProprioEmailNaoEConflito` | Já existia (I065/I066) |  |
| UT-F001-090 | UTI | `AtualizarContaInexistenteDaNaoEncontrado` | Já existia (I065/I066) |  |
| UT-F001-091 | UTI | `EmailInvalidoNaAtualizacaoERecusado` | Já existia (I065/I066) |  |
| UT-F001-092 | UTI | `BloquearContaAtiva` | Já existia (I065/I066) |  |
| UT-F001-093 | UTI | `EstadoDiferenteDeBloqueadoERecusado` (2 casos) | Já existia (I065/I066) |  |
| UT-F001-094 | UTI | `BloquearContaJaBloqueadaERecusado` | Já existia (I065/I066) |  |
| UT-F001-095 | UTI | `AdministradorNaoSeBloqueia` | Já existia (I065/I066) |  |
| UT-F001-096 | UTI | `BloquearClienteERecusado` | Já existia (I065/I066) |  |
| UT-F001-097 | UTI | `BloquearContaInexistenteDaNaoEncontrado` | Já existia (I065/I066) |  |
| UT-F001-098 | UTI | `FiltroPorPerfilEEstadoDevolveSoVeterinariosAtivos` | Já existia (I065/I066) |  |
| UT-F001-099 | UTI | `PerfilInvalidoNoFiltroERecusado` | Já existia (I065/I066) |  |
| **7.1.9. Dados e horário da clínica `ClinicaService`** | | | | |
| UT-F001-100 | CLI | `ObterDevolveDuracaoEHorarioPadrao` | Novo (I086) |  |
| UT-F001-101 | CLI | `DiaSeteEDomingo` | Novo (I086) |  |
| UT-F001-102 | CLI | `AtualizarSubstituiDadosDuracaoEHorario` | Novo (I086) |  |
| UT-F001-103 | CLI | `FechoIgualAAberturaDevolve400` | Novo (I086) |  |
| UT-F001-104 | CLI | `FechoAntesDaAberturaDevolve400` | Novo (I086) |  |
| UT-F001-105 | CLI | `PeriodosSobrepostosDevolvem400` | Novo (I086) |  |
| UT-F001-106 | CLI | `PeriodosContiguosSaoAceites` | Novo (I086) |  |
| UT-F001-107 | CLI | `MesmasHorasEmDiasDiferentesSaoAceites` | Novo (I086) |  |
| UT-F001-108 | CLI | `HoraForaDoFormatoDevolve400` (2 casos) | Novo (I086) |  |
| UT-F001-109 | CLI | `DuracaoZeroEInvalida` | Novo (I086) |  |
| UT-F001-110 | CLI | `HorarioVazioFechaAClinica` | Novo (I086) | D2 |
| UT-F001-111 | CLI | `NifInvalidoEInvalido` | Novo (I086) |  |
| **7.1.10. Clientes `ClientesService`** | | | | |
| UT-F001-112 | CLT | `CriarClienteCriaContaEClienteLigados` | Novo (I086) | N2 |
| UT-F001-113 | CLT | `CriarClienteSemNif` | Novo (I086) |  |
| UT-F001-114 | CLT | `NifInvalidoNaCriacaoEInvalido` | Novo (I086) |  |
| UT-F001-115 | CLT | `EmailInvalidoNaCriacaoEInvalido` | Novo (I086) |  |
| UT-F001-116 | CLT | `TelefoneEmFaltaEInvalido` | Novo (I086) |  |
| UT-F001-117 | CLT | `NifRepetidoDevolve409` | Novo (I086) |  |
| UT-F001-118 | CLT | `EmailRepetidoDevolve409` | Novo (I086) |  |
| UT-F001-119 | CLT | `NovoEmailAtualizaClienteEConta` | Novo (I086) |  |
| UT-F001-120 | CLT | `NifDeOutroClienteDevolve409` | Novo (I086) |  |
| UT-F001-121 | CLT | `ProprioNifEAceite` | Novo (I086) |  |
| UT-F001-122 | CLT | `NifCurtoNaAtualizacaoEInvalido` | Novo (I086) |  |
| UT-F001-123 | CLT | `ClienteInexistenteDevolve404` | Novo (I086) |  |
| UT-F001-124 | CLT | `PesquisaPorNomeDevolveClienteComNumeroDeAnimais` | Novo (I086) |  |
| UT-F001-125 | CLT | `ObterClienteTemContaEAnimais` | Novo (I086) |  |
| **7.2.1. Pacientes `AnimaisService`** | | | | |
| UT-F002-001 | ANI | `RegistarAnimalValido` | Novo (I086) |  |
| UT-F002-002 | ANI | `RegistarSemCamposOpcionais` | Novo (I086) |  |
| UT-F002-003 | ANI | `ClienteIdEmFaltaEInvalido` | Novo (I086) |  |
| UT-F002-004 | ANI | `ClienteInexistenteDevolve404` | Novo (I086) |  |
| UT-F002-005 | ANI | `NomeVazioEInvalido` | Novo (I086) |  |
| UT-F002-006 | ANI | `NomeCom81CaracteresEInvalido` | Novo (I086) |  |
| UT-F002-007 | ANI | `PesoZeroDevolve400` | Novo (I086) |  |
| UT-F002-008 | ANI | `PesoMinimoEAceite` | Novo (I086) |  |
| UT-F002-009 | ANI | `PesoNegativoDevolve400` | Novo (I086) |  |
| UT-F002-010 | ANI | `DataDeNascimentoFuturaDevolve400` | Novo (I086) |  |
| UT-F002-011 | ANI | `NascidoHojeTemIdadeZero` | Novo (I086) |  |
| UT-F002-012 | ANI | `EspecieInexistenteDevolve400` | Novo (I086) |  |
| UT-F002-013 | ANI | `RacaDeOutraEspecieDevolve400` | Novo (I086) |  |
| UT-F002-014 | ANI | `AtualizarPesoGravaAlteracao` | Novo (I086) |  |
| UT-F002-015 | ANI | `AtualizarComPesoZeroNaoAltera` | Novo (I086) |  |
| UT-F002-016 | ANI | `AtualizarAnimalInexistenteDevolve404` | Novo (I086) |  |
| UT-F002-017 | ANI | `AtualizarParaClienteInexistenteDevolve404` | Novo (I086) |  |
| UT-F002-018 | ANI | `FichaTemDadosAdministrativosETelefone` | Novo (I086) |  |
| UT-F002-019 | ANI | `FiltroPorClienteDevolveSoOsSeusAnimais` | Novo (I086) |  |
| UT-F002-020 | ANI | `ClienteVeSoOsSeusAnimais` | Novo (I086) |  |
| UT-F002-021 | ANI | `ClienteSemAnimaisRecebeListaVazia` | Novo (I086) |  |
| UT-F002-022 | ANI | `ClienteConsultaAnimalProprio` | Novo (I086) |  |
| UT-F002-023 | ANI | `AnimalDeOutroClienteDevolve403` | Novo (I086) |  |
| UT-F002-024 | ANI | `AnimalInexistenteNaAreaDoClienteDevolve404` | Novo (I086) |  |
| **7.2.2. Idade do animal `CalculadoraIdade`** | | | | |
| UT-F002-025 | ANI | `IdadeQuatroAnos` | Novo (I086) |  |
| UT-F002-026 | ANI | `IdadeNoDiaDeAnos` | Novo (I086) |  |
| UT-F002-027 | ANI | `IdadeNaVesperaDosAnos` | Novo (I086) |  |
| UT-F002-028 | ANI | `NascidoA29DeFevereiroAinda0AnosA28` | Novo (I086) |  |
| UT-F002-029 | ANI | `NascidoA29DeFevereiroTem1AnoA1DeMarco` | Novo (I086) |  |
| UT-F002-030 | ANI | `SemDataDeNascimentoIdadeVazia` | Novo (I086) | D3 |
| **7.6.1. Paginação `PaginacaoQuery`** | | | | |
| UT-TRV-001 | CON | `PaginacaoPorOmissao` | Já existia (I072) |  |
| UT-TRV-001 | UTI | `PaginacaoPorOmissao` | Já existia (I065/I066) |  |
| UT-TRV-002 | CON | `LimitesDaPaginacao` (6 casos) | Já existia (I072) |  |
| UT-TRV-003 | PAG | `TamanhoAcimaDoMaximoEInvalido` | Novo (I086) |  |
| UT-TRV-004 | PAG | `PaginaOuTamanhoZeroEInvalido` (2 casos) | Novo (I086) |  |
| UT-TRV-005 | CLT | `TerceiraPaginaTemOsUltimosDezassete` | Novo (I086) |  |
| **7.6.2. Tratamento de erros e registos middleware** | | | | |
| UT-TRV-006 | ERR | `ExcecaoPrevistaDaOCodigoHttp` (6 casos) | Novo (I086) |  |
| UT-TRV-007 | ERR | `ExcecaoInesperadaNaoExpoeAMensagemInterna` | Novo (I086) | D-01 (ignorado) |
| UT-TRV-008 | ERR | `ErrosDeValidacaoDaoAsDuasMensagens` | Novo (I086) |  |
| UT-TRV-009 | ERR | `CorpoInvalidoDaMensagemGenerica` (2 casos) | Novo (I086) |  |
| UT-TRV-010 | ERR | `RegistoDoPedidoNaoTemTokenNemCorpo` | Novo (I086) |  |

**Resumo:** 165 casos da I028 para a I086; 165 implementados; 0 não implementados. Um caso (UT-TRV-007) está ignorado com justificação por causa do defeito D-01.

## 8. Verificação pelo Executor

### 8.1. Execução local

```powershell
dotnet test src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj -c Release
```

| Execução      | Total | Aprovados | Falhados | Ignorados          | Duração |
| ------------- | ----- | --------- | -------- | ------------------ | ------- |
| Antes da I086 | 448   | 448       | 0        | 0                  | —       |
| 1.ª           | 575   | 574       | 0        | 1 (UT-TRV-007, D-01) | 3 s   |
| 2.ª (seguida) | 575   | 574       | 0        | 1 (UT-TRV-007, D-01) | 2 s   |
| Sobre o `develop` de 2026-11-01 (`93c5723`, já com a I076) | 598 | 597 | 0 | 1 (UT-TRV-007, D-01) | 2 s |

As duas primeiras execuções são sobre o commit `32487cd`, de onde parte o ramo da I086; a última junta as alterações da I086 ao `develop` atual, que acrescenta os 23 testes unitários da I076, e confirma que não há conflitos.

Compilação da solução em Release sem avisos nem erros (os avisos são tratados como erros). Os testes unitários não usam SQL Server, Testcontainers, `HttpClient` nem `WebApplicationFactory`: a base de dados é o EF Core InMemory e a hora é sempre injetada.

### 8.2. Alterações deliberadas (o teste certo falha)

Cada alteração foi feita só na cópia local, testada e reposta logo a seguir (`git checkout`). Sem alterações, os 574 testes voltam a passar.

| Alteração ao código de produção | Testes que falham |
| --- | --- |
| `AuthService`: o e-mail deixa de ser aparado | UT-F001-006 |
| `AuthService`: deixa de recusar contas bloqueadas no login | UT-F001-009 (e um teste da I066) |
| Política `ClienteProprietario`: deixa de exigir o perfil Cliente | UT-F001-048 (4 perfis) |
| R-NIF: controlo 0 só com resto 0 | UT-F001-060 (e os testes do telefone da clínica, que usam esse NIF) |
| Horário: períodos contíguos contam como sobrepostos | UT-F001-106 (e um teste da I067) |
| `ClientesService`: a atualização não verifica o NIF de outro Cliente | UT-F001-120 |
| `AnimaisService`: a área do Cliente não compara o dono do animal | UT-F002-023 |
| Idade: não desconta o ano antes do aniversário | UT-F002-027 e UT-F002-028 |

### 8.3. Pipeline

O job «2 · Testes unitários (QG-02)» da Backend CI executa estes testes. A ligação da execução da pipeline sobre o commit da Pull Request é acrescentada aqui quando a PR for aberta.

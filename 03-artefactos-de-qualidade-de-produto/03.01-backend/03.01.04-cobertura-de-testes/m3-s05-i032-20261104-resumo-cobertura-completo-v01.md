``````text
Summary - Backend — Gestão de Clínica Veterinária (G08)
  Generated on: 11/04/2026 - 00:46:38
  Coverage date: 11/04/2026 - 00:43:24 - 11/04/2026 - 00:46:18
  Parser: MultiReport (4x Cobertura)
  Assemblies: 4
  Classes: 145
  Files: 142
  Line coverage: 97.6%
  Covered lines: 6388
  Uncovered lines: 156
  Coverable lines: 6544
  Total lines: 11618
  Branch coverage: 91% (732 of 804)
  Covered branches: 732
  Total branches: 804
  Method coverage: 99.4% (676 of 680)
  Full method coverage: 96.1% (654 of 680)
  Covered methods: 676
  Fully covered methods: 654
  Total methods: 680

ClinicaVeterinaria.Api                                                           90.6%
  ClinicaVeterinaria.Api.Controllers.AnimaisController                            100%
  ClinicaVeterinaria.Api.Controllers.AuthController                              94.1%
  ClinicaVeterinaria.Api.Controllers.ClientesController                           100%
  ClinicaVeterinaria.Api.Controllers.ClinicaController                            100%
  ClinicaVeterinaria.Api.Controllers.ConsultasController                          100%
  ClinicaVeterinaria.Api.Controllers.MeusAnimaisController                        100%
  ClinicaVeterinaria.Api.Controllers.MinhasConsultasController                    100%
  ClinicaVeterinaria.Api.Controllers.MinhasVacinacoesController                   100%
  ClinicaVeterinaria.Api.Controllers.RegistosClinicosController                   100%
  ClinicaVeterinaria.Api.Controllers.UtilizadoresController                       100%
  ClinicaVeterinaria.Api.Controllers.VacinacoesController                         100%
  ClinicaVeterinaria.Api.Controllers.VeterinariosController                       100%
  ClinicaVeterinaria.Api.Middleware.ExceptionHandlingMiddleware                  96.2%
  ClinicaVeterinaria.Api.Middleware.RequestLoggingMiddleware                      100%
  ClinicaVeterinaria.Api.Middleware.RespostaValidacaoModelo                       100%
  ClinicaVeterinaria.Api.Seguranca.AutenticacaoExtensions                          98%
  ClinicaVeterinaria.Api.Seguranca.ClienteAutenticadoControllerBase               100%
  ClinicaVeterinaria.Api.Seguranca.JwtOptions                                      90%
  ClinicaVeterinaria.Api.Seguranca.JwtOptionsValidator                            100%
  ClinicaVeterinaria.Api.Seguranca.JwtTokenService                                100%
  ClinicaVeterinaria.Api.Seguranca.LimpezaTokensRevogadosOptions                  100%
  ClinicaVeterinaria.Api.Seguranca.LimpezaTokensRevogadosService                 92.3%
  Program                                                                        52.2%

ClinicaVeterinaria.Application                                                   98.2%
  ClinicaVeterinaria.Application.Agenda.CalculadoraDisponibilidade                100%
  ClinicaVeterinaria.Application.Agenda.DiaDisponivel                             100%
  ClinicaVeterinaria.Application.Agenda.EstadosConsulta                           100%
  ClinicaVeterinaria.Application.Agenda.FusoHorarioClinica                       87.5%
  ClinicaVeterinaria.Application.Agenda.MaquinaEstadosMarcacao                   96.1%
  ClinicaVeterinaria.Application.Agenda.MarcacaoAgendada                          100%
  ClinicaVeterinaria.Application.Agenda.TiposMinhasConsultas                      100%
  ClinicaVeterinaria.Application.Agenda.VersaoMarcacao                            100%
  ClinicaVeterinaria.Application.DependencyInjection                              100%
  ClinicaVeterinaria.Application.DTOs.Animais.AnimalDto                           100%
  ClinicaVeterinaria.Application.DTOs.Animais.AnimalFiltroQuery                   100%
  ClinicaVeterinaria.Application.DTOs.Animais.AnimalListItemDto                   100%
  ClinicaVeterinaria.Application.DTOs.Animais.ClienteDetalheDto                   100%
  ClinicaVeterinaria.Application.DTOs.Animais.ClienteResumoDto                    100%
  ClinicaVeterinaria.Application.DTOs.Animais.MeuAnimalDto                        100%
  ClinicaVeterinaria.Application.DTOs.Animais.MeuAnimalListItemDto                100%
  ClinicaVeterinaria.Application.DTOs.Animais.UpdateAnimalDto                     100%
  ClinicaVeterinaria.Application.DTOs.Auth.LoginRequestDto                        100%
  ClinicaVeterinaria.Application.DTOs.Auth.LoginResponseDto                       100%
  ClinicaVeterinaria.Application.DTOs.Auth.TokenEmitido                           100%
  ClinicaVeterinaria.Application.DTOs.Auth.UtilizadorAutenticadoDto               100%
  ClinicaVeterinaria.Application.DTOs.Auth.UtilizadorResumoDto                    100%
  ClinicaVeterinaria.Application.DTOs.Clientes.ClienteDto                         100%
  ClinicaVeterinaria.Application.DTOs.Clientes.ClienteFiltroQuery                 100%
  ClinicaVeterinaria.Application.DTOs.Clientes.ClienteListItemDto                 100%
  ClinicaVeterinaria.Application.DTOs.Clientes.CreateClienteDto                   100%
  ClinicaVeterinaria.Application.DTOs.Clientes.UpdateClienteDto                   100%
  ClinicaVeterinaria.Application.DTOs.Clinica.ClinicaDto                          100%
  ClinicaVeterinaria.Application.DTOs.Clinica.PeriodoHorarioDto                   100%
  ClinicaVeterinaria.Application.DTOs.Clinica.UpdateClinicaDto                    100%
  ClinicaVeterinaria.Application.DTOs.Comum.AnimalRefDto                          100%
  ClinicaVeterinaria.Application.DTOs.Comum.ClienteRefDto                         100%
  ClinicaVeterinaria.Application.DTOs.Comum.PagedResultDto<T>                     100%
  ClinicaVeterinaria.Application.DTOs.Comum.VeterinarioRefDto                     100%
  ClinicaVeterinaria.Application.DTOs.Consultas.ChegadaComVersao                  100%
  ClinicaVeterinaria.Application.DTOs.Consultas.ChegadaDto                        100%
  ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaComVersao                 100%
  ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaDto                       100%
  ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaFiltroQuery               100%
  ClinicaVeterinaria.Application.DTOs.Consultas.ConsultaListItemDto               100%
  ClinicaVeterinaria.Application.DTOs.Consultas.CreateConsultaDto                 100%
  ClinicaVeterinaria.Application.DTOs.Consultas.CreateNotaPreparacaoDto           100%
  ClinicaVeterinaria.Application.DTOs.Consultas.MinhaConsultaDto                  100%
  ClinicaVeterinaria.Application.DTOs.Consultas.MinhasConsultasQuery              100%
  ClinicaVeterinaria.Application.DTOs.Consultas.NotaPreparacaoDto                 100%
  ClinicaVeterinaria.Application.DTOs.Consultas.UpdateConsultaDto                 100%
  ClinicaVeterinaria.Application.DTOs.Consultas.UpdateEstadoConsultaDto           100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.CreatePrescricaoDto        100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.CreateRegistoClinicoDto    100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.CreateTratamentoDto        100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.HistoricoClinicoDto        100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.HistoricoClinicoQuery      100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.PrescricaoDto              100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.RegistoClinicoDto          100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.RegistoClinicoResumoDto    100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.TratamentoDto              100%
  ClinicaVeterinaria.Application.DTOs.RegistosClinicos.UpdateRegistoClinicoDto    100%
  ClinicaVeterinaria.Application.DTOs.Utilizadores.AlterarEstadoUtilizadorDto     100%
  ClinicaVeterinaria.Application.DTOs.Utilizadores.CreateUtilizadorDto            100%
  ClinicaVeterinaria.Application.DTOs.Utilizadores.UpdateUtilizadorDto            100%
  ClinicaVeterinaria.Application.DTOs.Utilizadores.UtilizadorDto                  100%
  ClinicaVeterinaria.Application.DTOs.Utilizadores.UtilizadorFiltroQuery          100%
  ClinicaVeterinaria.Application.DTOs.Utilizadores.UtilizadorListItemDto          100%
  ClinicaVeterinaria.Application.DTOs.Vacinacoes.AdministracaoDto                 100%
  ClinicaVeterinaria.Application.DTOs.Vacinacoes.BoletimDto                       100%
  ClinicaVeterinaria.Application.DTOs.Vacinacoes.BoletimQuery                     100%
  ClinicaVeterinaria.Application.DTOs.Vacinacoes.HistoricoAdministracaoDto        100%
  ClinicaVeterinaria.Application.DTOs.Vacinacoes.MeuBoletimDto                    100%
  ClinicaVeterinaria.Application.DTOs.Vacinacoes.ProximaAdministracaoDto          100%
  ClinicaVeterinaria.Application.DTOs.Vacinacoes.UpdateAdministracaoDto           100%
  ClinicaVeterinaria.Application.DTOs.Veterinarios.DiaDisponivelDto               100%
  ClinicaVeterinaria.Application.DTOs.Veterinarios.DisponibilidadeDto             100%
  ClinicaVeterinaria.Application.DTOs.Veterinarios.DisponibilidadeQuery          87.5%
  ClinicaVeterinaria.Application.Exceptions.ForbiddenException                    100%
  ClinicaVeterinaria.Application.Exceptions.PreconditionFailedException           100%
  ClinicaVeterinaria.Application.Exceptions.UnprocessableEntityException          100%
  ClinicaVeterinaria.Application.Seguranca.AcessoConsultas                        100%
  ClinicaVeterinaria.Application.Seguranca.ClaimsPrincipalExtensions              100%
  ClinicaVeterinaria.Application.Seguranca.Perfis                                 100%
  ClinicaVeterinaria.Application.Vacinacao.CalculadoraProximaAdministracao        100%
  ClinicaVeterinaria.Application.Vacinacao.TiposAdministracao                     100%
  ClinicaVeterinaria.Application.Validacao.EmailValidoAttribute                   100%
  ClinicaVeterinaria.Application.Validacao.NifValidoAttribute                     100%
  ClinicaVeterinaria.Application.Validacao.PalavraPasseForteAttribute             100%
  ClinicaVeterinaria.Application.Validacao.PeriodoValidado                        100%
  ClinicaVeterinaria.Application.Validacao.ValidadorFiltroConsultas               100%
  ClinicaVeterinaria.Application.Validacao.ValidadorHorario                      95.8%
  ClinicaVeterinaria.Application.Validacao.ValidadorPeriodoDisponibilidade        100%
  ClinicaVeterinaria.Application.Validacao.ValidadorRegistoClinico               86.8%

ClinicaVeterinaria.Domain                                                        97.9%
  ClinicaVeterinaria.Domain.Entities.Cliente                                      100%
  ClinicaVeterinaria.Domain.Entities.Clinica                                      100%
  ClinicaVeterinaria.Domain.Entities.ConfiguracaoSistema                          100%
  ClinicaVeterinaria.Domain.Entities.Especie                                      100%
  ClinicaVeterinaria.Domain.Entities.Funcionario                                  100%
  ClinicaVeterinaria.Domain.Entities.HorarioFuncionamento                          80%
  ClinicaVeterinaria.Domain.Entities.InformacaoApoio                              100%
  ClinicaVeterinaria.Domain.Entities.Marcacao                                     100%
  ClinicaVeterinaria.Domain.Entities.Paciente                                     100%
  ClinicaVeterinaria.Domain.Entities.Prescricao                                   100%
  ClinicaVeterinaria.Domain.Entities.Raca                                         100%
  ClinicaVeterinaria.Domain.Entities.RegistoClinico                               100%
  ClinicaVeterinaria.Domain.Entities.RegistoProfilaxia                           90.9%
  ClinicaVeterinaria.Domain.Entities.TokenRevogado                                100%
  ClinicaVeterinaria.Domain.Entities.Tratamento                                   100%
  ClinicaVeterinaria.Domain.Entities.Utilizador                                   100%

ClinicaVeterinaria.Infrastructure                                                98.2%
  ClinicaVeterinaria.Infrastructure.AnimaisService                                100%
  ClinicaVeterinaria.Infrastructure.AuthService                                  97.4%
  ClinicaVeterinaria.Infrastructure.BloqueioAgendaVeterinario                    84.2%
  ClinicaVeterinaria.Infrastructure.ClientesService                               100%
  ClinicaVeterinaria.Infrastructure.ClinicaDbContext                              100%
  ClinicaVeterinaria.Infrastructure.ClinicaService                               94.2%
  ClinicaVeterinaria.Infrastructure.ConsultasService                             99.2%
  ClinicaVeterinaria.Infrastructure.DependencyInjection                           100%
  ClinicaVeterinaria.Infrastructure.DuracaoConsulta                               100%
  ClinicaVeterinaria.Infrastructure.Migrations.ClinicaDbContextModelSnapshot      100%
  ClinicaVeterinaria.Infrastructure.Migrations.I062DadosReferencia               96.1%
  ClinicaVeterinaria.Infrastructure.Migrations.Inicial                           97.5%
  ClinicaVeterinaria.Infrastructure.MinhasConsultasService                        100%
  ClinicaVeterinaria.Infrastructure.RegistosClinicosService                      99.5%
  ClinicaVeterinaria.Infrastructure.UtilizadoresService                          97.3%
  ClinicaVeterinaria.Infrastructure.VacinacoesService                             100%
  ClinicaVeterinaria.Infrastructure.VeterinariosService                           100%
``````

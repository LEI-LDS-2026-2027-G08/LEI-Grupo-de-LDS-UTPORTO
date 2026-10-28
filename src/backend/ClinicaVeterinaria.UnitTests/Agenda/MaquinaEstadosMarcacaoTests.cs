using ClinicaVeterinaria.Application.Agenda;
using ClinicaVeterinaria.Application.Exceptions;

namespace ClinicaVeterinaria.UnitTests.Agenda;

/// <summary>
/// I074 — transições de estado das marcações (EP-027).
/// Casos UT-F003-062 a UT-F003-075.
/// </summary>
public class MaquinaEstadosMarcacaoTests
{
  [Theory(DisplayName = "UT-F003-062 a 065 — As quatro transições permitidas são aceites")]
  [InlineData(EstadosConsulta.Pendente, EstadosConsulta.Confirmada)]
  [InlineData(EstadosConsulta.Confirmada, EstadosConsulta.Realizada)]
  [InlineData(EstadosConsulta.Pendente, EstadosConsulta.Cancelada)]
  [InlineData(EstadosConsulta.Confirmada, EstadosConsulta.Cancelada)]
  public void TransicoesPermitidasSaoAceites(
      string estadoAtual,
      string novoEstado)
  {
    MaquinaEstadosMarcacao.ValidarTransicao(
        estadoAtual,
        novoEstado);
  }

  [Theory(DisplayName = "UT-F003-066 a 073 — As transições não permitidas devolvem 422")]
  [InlineData(EstadosConsulta.Pendente, EstadosConsulta.Realizada)]
  [InlineData(EstadosConsulta.Confirmada, EstadosConsulta.Confirmada)]
  [InlineData(EstadosConsulta.Realizada, EstadosConsulta.Confirmada)]
  [InlineData(EstadosConsulta.Realizada, EstadosConsulta.Realizada)]
  [InlineData(EstadosConsulta.Realizada, EstadosConsulta.Cancelada)]
  [InlineData(EstadosConsulta.Cancelada, EstadosConsulta.Confirmada)]
  [InlineData(EstadosConsulta.Cancelada, EstadosConsulta.Realizada)]
  [InlineData(EstadosConsulta.Cancelada, EstadosConsulta.Cancelada)]
  public void TransicoesInvalidasSaoRecusadas(
      string estadoAtual,
      string novoEstado)
  {
    Assert.Throws<UnprocessableEntityException>(
        () => MaquinaEstadosMarcacao.ValidarTransicao(
            estadoAtual,
            novoEstado));
  }

  [Theory(DisplayName = "UT-F003-074 — Pendente nunca pode ser escolhido manualmente")]
  [InlineData(EstadosConsulta.Pendente)]
  [InlineData(EstadosConsulta.Confirmada)]
  [InlineData(EstadosConsulta.Realizada)]
  [InlineData(EstadosConsulta.Cancelada)]
  public void PendenteNuncaEUmDestinoValido(string _)
  {
    var erro = Assert.Throws<ArgumentException>(
        () => MaquinaEstadosMarcacao.NormalizarDestino(
            EstadosConsulta.Pendente));

    Assert.Equal(
        MaquinaEstadosMarcacao.MensagemPendenteNaoPermitido,
        erro.Message);
  }

  [Fact(DisplayName = "UT-F003-075 — Um estado inexistente devolve 400")]
  public void EstadoInexistenteERecusado()
  {
    Assert.Throws<ArgumentException>(
        () => MaquinaEstadosMarcacao.NormalizarDestino("Faltou"));
  }

  [Theory(DisplayName = "I074 — O estado é normalizado sem distinguir maiúsculas ou espaços")]
  [InlineData("confirmada", EstadosConsulta.Confirmada)]
  [InlineData(" REALIZADA ", EstadosConsulta.Realizada)]
  [InlineData("cancelada", EstadosConsulta.Cancelada)]
  public void DestinoECanonicalizado(
      string recebido,
      string esperado)
  {
    Assert.Equal(
        esperado,
        MaquinaEstadosMarcacao.NormalizarDestino(recebido));
  }
}

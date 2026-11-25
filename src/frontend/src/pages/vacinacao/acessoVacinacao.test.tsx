import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { MemoryRouter } from "react-router-dom";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { ApiClient } from "../../core/api";
import { AuthProvider } from "../../core/auth/AuthContext";
import type { Perfil } from "../../models/auth";
import { AppRoutes } from "../../routes/AppRoutes";
import { servicoFalso, sessaoDe } from "../../test/auxiliares";

/**
 * I118, critério 5: só o Veterinário acede ao boletim web. Os outros perfis
 * não acedem por endereço direto (e a API também os recusa com 403).
 */
function montar(caminho: string) {
  const servico = servicoFalso();
  render(
    <MemoryRouter initialEntries={[caminho]}>
      <AuthProvider
        servico={servico}
        cliente={new ApiClient("http://api.teste", vi.fn())}
      >
        <AppRoutes />
      </AuthProvider>
    </MemoryRouter>,
  );
  return servico;
}

async function iniciarSessao(
  perfil: Perfil,
  servico: ReturnType<typeof servicoFalso>,
) {
  servico.login.mockResolvedValue(sessaoDe(perfil));
  const u = userEvent.setup();
  await u.type(screen.getByLabelText("Email"), "maria@clinica.pt");
  await u.type(screen.getByLabelText("Palavra-passe"), "segredo");
  await u.click(screen.getByRole("button", { name: "Entrar" }));
}

describe("I118 — acesso ao boletim de vacinação e desparasitação", () => {
  beforeEach(() => sessionStorage.clear());

  it.each(["Rececionista", "AuxiliarVeterinario", "Administrador"] as const)(
    "%s não acede a /pacientes/3/vacinacao por endereço direto",
    async (perfil) => {
      const servico = montar("/pacientes/3/vacinacao");
      await iniciarSessao(perfil, servico);
      expect(
        await screen.findByRole("heading", { name: "Acesso negado" }),
      ).toBeInTheDocument();
    },
  );

  it("o Veterinário abre o boletim do paciente", async () => {
    const servico = montar("/pacientes/3/vacinacao");
    await iniciarSessao("Veterinario", servico);
    expect(
      await screen.findByRole("heading", {
        name: "Vacinação e desparasitação",
      }),
    ).toBeInTheDocument();
  });
});

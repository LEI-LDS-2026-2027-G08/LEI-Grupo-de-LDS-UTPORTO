import { test, expect } from "@playwright/test";

// Teste de sistema mínimo da base (I107): não precisa do backend.
// A I128 e a I142 acrescentam os fluxos completos.

test("sem sessão, o endereço raiz leva ao ecrã de login", async ({ page }) => {
  await page.goto("/");
  await expect(page).toHaveURL(/\/login$/);
  await expect(
    page.getByRole("heading", { name: "Iniciar sessão" }),
  ).toBeVisible();
});

test("uma rota protegida sem sessão também leva ao login", async ({ page }) => {
  await page.goto("/funcionarios");
  await expect(page).toHaveURL(/\/login$/);
  await expect(page.getByLabel("Email")).toBeVisible();
  await expect(page.getByLabel("Palavra-passe")).toBeVisible();
});

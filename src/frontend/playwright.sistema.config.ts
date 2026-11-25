import { defineConfig, devices } from "@playwright/test";
import { URL_API, URL_WEB } from "./tests/sistema/apoio/ambiente";

/**
 * Testes de sistema do frontend web (I128; automação: I129).
 * Correm no Chromium sem interface, contra o backend real (a API nunca é
 * simulada) e a base com os dados de demonstração. Executar com
 * `npm run test:sistema` (ver tests/sistema/README.md).
 */
const grupo = process.env.E2E_GRUPO ?? "todos";

export default defineConfig({
  testDir: "./tests/sistema",
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: process.env.CI ? 2 : undefined,
  timeout: 60_000,
  expect: { timeout: 10_000 },
  outputDir: `test-results/${grupo}`,
  reporter: [
    ["list"],
    ["html", { outputFolder: `playwright-report/${grupo}`, open: "never" }],
  ],
  globalSetup: "./tests/sistema/apoio/verificarAmbiente.ts",
  use: {
    baseURL: URL_WEB,
    headless: true,
    locale: "pt-PT",
    timezoneId: "Europe/Lisbon",
    screenshot: "only-on-failure",
    trace: "retain-on-failure",
  },
  projects: [
    {
      name: "sistema",
      testIgnore: /\.exclusivo\.spec\.ts$/,
      use: { ...devices["Desktop Chrome"] },
    },
    {
      name: "sistema-exclusivo",
      testMatch: /\.exclusivo\.spec\.ts$/,
      fullyParallel: false,
      use: { ...devices["Desktop Chrome"] },
    },
  ],
  // Com `npm run test:sistema`, o frontend é arrancado uma só vez pelo
  // executar.mjs (E2E_WEB_EXTERNO); numa execução direta do Playwright,
  // é arrancado aqui.
  webServer: process.env.E2E_WEB_EXTERNO
    ? undefined
    : {
        command: "npm run dev",
        url: URL_WEB,
        reuseExistingServer: !process.env.CI,
        env: { VITE_API_URL: URL_API },
      },
});

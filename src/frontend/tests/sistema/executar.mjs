/**
 * Executa os testes de sistema do frontend web (I128) em duas fases:
 * 1. «sistema» — todos os cenários que podem correr em paralelo;
 * 2. «sistema-exclusivo» — os que alteram a configuração ou o horário da
 *    clínica, sozinhos e depois dos restantes (I128, secção 5.5, regra 3).
 * A segunda fase corre sempre, mesmo que a primeira tenha falhas.
 *
 * O frontend (npm run dev) é arrancado uma só vez para as duas fases, se
 * ainda não estiver a correr, e parado no fim. Assim nenhuma fase depende de
 * um servidor que a fase anterior esteja a terminar.
 * Argumentos extra são passados ao Playwright (ex.: --grep F001-07).
 */
import { spawn, spawnSync } from "node:child_process";

const URL_WEB = (process.env.E2E_WEB_URL ?? "http://localhost:5173").replace(
  /\/+$/,
  "",
);
const URL_API = (process.env.E2E_API_URL ?? "http://localhost:5159").replace(
  /\/+$/,
  "",
);
const extra = process.argv.slice(2);

async function responde(url) {
  try {
    await fetch(url);
    return true;
  } catch {
    return false;
  }
}

async function arrancarFrontend() {
  if (await responde(URL_WEB)) {
    console.log(`Frontend já a correr em ${URL_WEB}: é reutilizado.`);
    return null;
  }
  console.log(`A arrancar o frontend (npm run dev) em ${URL_WEB}…`);
  const processo = spawn("npm run dev", {
    shell: true,
    stdio: "ignore",
    detached: process.platform !== "win32",
    env: { ...process.env, VITE_API_URL: URL_API },
  });
  for (let i = 0; i < 120; i++) {
    if (await responde(URL_WEB)) return processo;
    await new Promise((r) => setTimeout(r, 500));
  }
  pararFrontend(processo);
  throw new Error(`O frontend não arrancou em ${URL_WEB}.`);
}

function pararFrontend(processo) {
  if (!processo) return;
  if (process.platform === "win32") {
    spawnSync("taskkill", ["/pid", String(processo.pid), "/T", "/F"], {
      stdio: "ignore",
    });
  } else {
    try {
      process.kill(-processo.pid);
    } catch {
      // já terminou
    }
  }
}

const servidor = await arrancarFrontend();
let falhou = false;
try {
  for (const grupo of ["sistema", "sistema-exclusivo"]) {
    console.log(`\n=== Testes de sistema — projeto ${grupo} ===\n`);
    const comando = [
      "npx playwright test",
      "--config=playwright.sistema.config.ts",
      `--project=${grupo}`,
      ...extra.map((a) => `"${a.replaceAll('"', '\\"')}"`),
    ].join(" ");
    const resultado = spawnSync(comando, {
      stdio: "inherit",
      shell: true,
      env: { ...process.env, E2E_GRUPO: grupo, E2E_WEB_EXTERNO: "1" },
    });
    if (resultado.status !== 0) falhou = true;
  }
} finally {
  pararFrontend(servidor);
}

process.exit(falhou ? 1 : 0);

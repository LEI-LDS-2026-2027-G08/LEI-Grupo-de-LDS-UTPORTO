import { spawnSync } from "node:child_process";
import { createHash } from "node:crypto";
import {
  existsSync,
  mkdirSync,
  readFileSync,
  readdirSync,
  writeFileSync,
} from "node:fs";
import { dirname, join, relative, resolve } from "node:path";
import { fileURLToPath } from "node:url";

// I046: o mesmo comando e os mesmos limites no computador e no CI.
const raiz = resolve(dirname(fileURLToPath(import.meta.url)), "..");
const argumento = process.argv.slice(2);
if (
  argumento.length > 1 ||
  (argumento[0] && !argumento[0].startsWith("--saida="))
) {
  throw new Error("Uso: npm run analise:estatica -- [--saida=PASTA]");
}
const saida = resolve(
  raiz,
  argumento[0]?.slice("--saida=".length) || "resultados-analise-estatica",
);
mkdirSync(saida, { recursive: true });
const escrever = (nome, texto) => writeFileSync(join(saida, nome), texto);
const executar = (comando, args) =>
  spawnSync(comando, args, {
    cwd: raiz,
    encoding: "utf8",
    timeout: 600_000,
    maxBuffer: 32 * 1024 * 1024,
  });
const bin = (pacote, ficheiro) => join(raiz, "node_modules", pacote, ficheiro);
const etapas = [
  [
    "QG-04",
    "eslint",
    bin("eslint", "bin/eslint.js"),
    [".", "--max-warnings", "0", "--format", "json"],
  ],
  ["QG-01", "typescript", bin("typescript", "bin/tsc"), ["-b", "--force"]],
  ["QG-08", "prettier", bin("prettier", "bin/prettier.cjs"), ["--check", "."]],
  ["QG-01", "build", bin("vite", "bin/vite.js"), ["build"]],
];
const resumo = {
  inicio: new Date().toISOString(),
  commitBase: executar("git", ["rev-parse", "HEAD"]).stdout.trim() || null,
  node: process.version,
  plataforma: `${process.platform}/${process.arch}`,
  ferramentas: {},
  resultados: [],
};
for (const pacote of [
  "eslint",
  "typescript",
  "typescript-eslint",
  "prettier",
  "vite",
]) {
  resumo.ferramentas[pacote] = JSON.parse(
    readFileSync(bin(pacote, "package.json"), "utf8"),
  ).version;
}
for (const [gate, nome, comando, args] of etapas) {
  const resultado = executar(process.execPath, [comando, ...args]);
  const codigo = resultado.status ?? 1;
  escrever(
    `${nome}.${nome === "eslint" ? "json" : "txt"}`,
    resultado.stdout || "",
  );
  escrever(
    `${nome}-stderr.txt`,
    resultado.stderr || String(resultado.error || ""),
  );
  const entrada = { gate, ferramenta: nome, codigo, passou: codigo === 0 };
  if (nome === "eslint") {
    try {
      const ficheiros = JSON.parse(resultado.stdout);
      entrada.ficheiros = ficheiros.length;
      entrada.erros = ficheiros.reduce((n, f) => n + f.errorCount, 0);
      entrada.avisos = ficheiros.reduce((n, f) => n + f.warningCount, 0);
    } catch {
      entrada.passou = false;
    }
  }
  resumo.resultados.push(entrada);
  console.log(
    `${gate} · ${nome}: ${entrada.passou ? "PASSOU" : "FALHOU"} (código ${codigo})`,
  );
}
// Identifica o conteúdo analisado mesmo antes de existir um commit das correções.
const ficheiros = [];
function listar(pasta) {
  for (const entrada of readdirSync(pasta, { withFileTypes: true })) {
    const caminho = join(pasta, entrada.name);
    if (entrada.isDirectory()) listar(caminho);
    else if (entrada.isFile()) ficheiros.push(caminho);
  }
}
for (const pasta of ["src", "tests", "scripts", "public"]) {
  if (existsSync(join(raiz, pasta))) listar(join(raiz, pasta));
}
for (const f of readdirSync(raiz)) {
  if (
    /^(package(-lock)?\.json|tsconfig.*\.json|.*config\.(ts|js)|\.prettierrc)$/.test(
      f,
    )
  ) {
    ficheiros.push(join(raiz, f));
  }
}
for (const f of [".gitignore", ".env.example", "README.md", "index.html"]) {
  ficheiros.push(join(raiz, f));
}
ficheiros.push(join(raiz, "../../.github/workflows/frontend-estatica.yml"));
const manifesto =
  ficheiros
    .sort()
    .map(
      (f) =>
        `${createHash("sha256").update(readFileSync(f)).digest("hex")}  ${relative(raiz, f).replaceAll("\\", "/")}`,
    )
    .join("\n") + "\n";
escrever("SHA256-ficheiros.txt", manifesto);
resumo.sha256Manifesto = createHash("sha256").update(manifesto).digest("hex");
resumo.fim = new Date().toISOString();
resumo.passou = resumo.resultados.every((r) => r.passou);
escrever("resumo.json", JSON.stringify(resumo, null, 2) + "\n");
console.log(`Resultado global: ${resumo.passou ? "PASSOU" : "FALHOU"}`);
console.log(`Resultados: ${saida}`);
process.exitCode = resumo.passou ? 0 : 1;

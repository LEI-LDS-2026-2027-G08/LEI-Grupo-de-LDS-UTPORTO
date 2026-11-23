#!/usr/bin/env python3
"""Arquiva fontes já guardadas em commit e compara-as com a execução da I137."""

import hashlib
import json
from pathlib import Path
import subprocess

ROOT = Path(__file__).resolve().parents[4]
DEST = ROOT / "04-artefactos-tecnicos/04.15-frontend-mobile/m4-v01"


def git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT)


def main():
    commit = git("rev-parse", "HEAD").decode().strip()
    manifest = DEST / "m4-i137-fontes-executadas-v01.json"
    if not manifest.exists():
        raise SystemExit("Falta o manifesto da execução aprovada da I137.")
    sources = json.loads(manifest.read_text())["ficheiros"]
    for name, expected in sources.items():
        result = subprocess.run(["git", "show", f"{commit}:{name}"], cwd=ROOT, capture_output=True)
        if result.returncode or hashlib.sha256(result.stdout).hexdigest() != expected:
            raise SystemExit("O commit ainda não contém a versão testada: " + name +
                             ". Guarde primeiro as fontes testadas em commit.")
    files = git("ls-tree", "-r", "--name-only", commit, "--", "src/frontend-mobile").decode().splitlines()
    files = [name for name in files if not any(part in Path(name).parts for part in
             (".idea", ".gradle", ".kotlin", "build", "resultados", "__pycache__"))
             and Path(name).name != "local.properties" and not name.endswith(".iml")]
    planned = []
    hashes = {}
    for name in sorted(files):
        relative = Path(name).relative_to("src/frontend-mobile")
        if relative == Path("README.md"):
            relative = Path("m4-frontend-mobile-instrucoes-fontes-v02.md")
        target = DEST / relative
        content = git("show", f"{commit}:{name}")
        if target.is_symlink() or any(parent.is_symlink() for parent in target.parents):
            raise SystemExit("Destino com ligação simbólica; exportação recusada: " + str(target))
        if target.exists() and target.read_bytes() != content:
            raise SystemExit("Já existe uma versão diferente; nada foi escrito: " + str(target))
        hashes[str(relative)] = hashlib.sha256(content).hexdigest()
        planned.append((target, content, name.endswith("/gradlew")))
    for target, content, executable in planned:
        target.parent.mkdir(parents=True, exist_ok=True)
        target.write_bytes(content)
        if executable:
            target.chmod(0o755)
    (DEST / "m4-i137-origem-fontes-v01.json").write_text(json.dumps({
        "commit": commit, "issue": "I137", "estado": "Fontes testadas, para revisão",
        "origem": "src/frontend-mobile", "ficheiros": hashes,
        "workflow": ".github/workflows/mobile-sistema.yml",
        "dependencias_externas": ["src/backend", "04-artefactos-tecnicos/04.13-scripts-de-base-de-dados"],
    }, ensure_ascii=False, indent=2) + "\n")
    print(f"Fontes exportadas diretamente de {commit}: {DEST}")
    print("Os documentos e arquivos anteriores foram preservados. Não foi criado nenhum commit.")


if __name__ == "__main__":
    main()

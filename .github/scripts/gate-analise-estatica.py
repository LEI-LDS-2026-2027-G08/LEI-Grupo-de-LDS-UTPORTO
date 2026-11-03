#!/usr/bin/env python3
# =============================================================================
# Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
# Issue          : I103 — Integrar análise estática, cobertura e quality gates
# Ficheiro       : .github/scripts/gate-analise-estatica.py
# Versão         : m3-v02 (cópia arquivada em
#                  /projeto/04-artefactos-tecnicos/04.16-ci-cd/m3-v02)
# Finalidade     : Quality gate QG-04 (análise estática Roslyn). Lê os relatórios
#                  SARIF 2.1 produzidos na compilação da etapa 1 (um por projeto),
#                  conta as ocorrências não suprimidas com os níveis que bloqueiam
#                  e compara com o máximo permitido. Os níveis e o máximo vêm de
#                  .github/quality-gates.json (critério 1 da I103).
# Utilização     : gate-analise-estatica.py <config> <pasta-dos-sarif> <relatorio.md>
#                  Escreve o resumo da execução e um relatório Markdown (anexado
#                  como artefacto) e termina com código 1 se o máximo for
#                  ultrapassado ou se não houver nenhum relatório SARIF.
# Dependências   : Python 3 da imagem ubuntu-latest (só biblioteca padrão).
# =============================================================================
import collections
import glob
import json
import os
import sys


def ler_config(caminho):
    with open(caminho, encoding="utf-8") as ficheiro:
        return json.load(ficheiro)["QG-04"]


def localizacao(resultado):
    for local in resultado.get("locations") or []:
        fisica = local.get("physicalLocation") or {}
        uri = (fisica.get("artifactLocation") or {}).get("uri", "")
        linha = (fisica.get("region") or {}).get("startLine")
        if uri:
            # Encurta os caminhos absolutos do runner para caminhos do repositório.
            for marcador in ("/src/backend/", "/_/src/backend/"):
                if marcador in uri:
                    uri = "src/backend/" + uri.split(marcador, 1)[1]
                    break
            uri = uri.replace("file://", "")
            return f"{uri}:{linha}" if linha else uri
    return "—"


def ler_resultados(pasta):
    ficheiros = sorted(glob.glob(os.path.join(pasta, "**", "*.sarif"), recursive=True))
    resultados = []
    for caminho in ficheiros:
        with open(caminho, encoding="utf-8-sig") as ficheiro:
            sarif = json.load(ficheiro)
        projeto = os.path.splitext(os.path.basename(caminho))[0]
        for execucao in sarif.get("runs", []):
            for resultado in execucao.get("results") or []:
                # SARIF 2.1: sem «level», o nível por omissão é «warning».
                nivel = resultado.get("level", "warning")
                suprimido = any(s.get("status", "accepted") == "accepted" for s in resultado.get("suppressions") or [])
                mensagem = resultado.get("message", {}).get("text", "")
                resultados.append({
                    "projeto": projeto,
                    "regra": resultado.get("ruleId", "?"),
                    "nivel": nivel,
                    "suprimido": suprimido,
                    "local": localizacao(resultado),
                    "mensagem": " ".join(mensagem.split())[:200],
                })
    return ficheiros, resultados


def main():
    if len(sys.argv) < 4:
        print("Utilização: gate-analise-estatica.py <config> <pasta-dos-sarif> <relatorio.md>", file=sys.stderr)
        return 2
    qg = ler_config(sys.argv[1])
    pasta, destino_relatorio = sys.argv[2], sys.argv[3]
    niveis = {n.lower() for n in qg["niveisQueBloqueiam"]}
    maximo = int(qg["maximoOcorrencias"])

    ficheiros, resultados = ler_resultados(pasta)
    bloqueiam = [r for r in resultados if r["nivel"].lower() in niveis and not r["suprimido"]]
    suprimidos = [r for r in resultados if r["suprimido"]]
    por_nivel = collections.Counter(r["nivel"] for r in resultados if not r["suprimido"])
    por_regra = collections.Counter((r["regra"], r["nivel"]) for r in bloqueiam)

    linhas = [
        "### QG-04 · Análise estática (Roslyn)",
        "",
        "| Medida | Valor |",
        "|---|---|",
        f"| Relatórios SARIF (um por projeto) | {len(ficheiros)} |",
        f"| Ocorrências por nível (não suprimidas) | {', '.join(f'{n}: {c}' for n, c in sorted(por_nivel.items())) or 'nenhuma'} |",
        f"| Ocorrências suprimidas no código | {len(suprimidos)} |",
        f"| **Ocorrências que bloqueiam** ({', '.join(sorted(niveis))}) | **{len(bloqueiam)}** |",
        f"| Máximo permitido (`.github/quality-gates.json`) | {maximo} |",
        "",
    ]
    if por_regra:
        linhas += ["| Regra | Nível | Ocorrências |", "|---|---|---|"]
        linhas += [f"| `{regra}` | {nivel} | {n} |" for (regra, nivel), n in por_regra.most_common()]
        linhas.append("")
        linhas += ["<details><summary>Ocorrências que bloqueiam (até 50)</summary>", "",
                   "| Projeto | Regra | Local | Mensagem |", "|---|---|---|---|"]
        linhas += [f"| {r['projeto']} | `{r['regra']}` | `{r['local']}` | {r['mensagem'].replace('|', '/')} |" for r in bloqueiam[:50]]
        linhas += ["", "</details>", ""]

    if not ficheiros:
        linhas.append("❌ **QG-04 não avaliado:** não foi produzido nenhum relatório SARIF na compilação (ver a etapa 1).")
        codigo = 1
    elif len(bloqueiam) > maximo:
        linhas.append(f"❌ **QG-04 não cumprido:** {len(bloqueiam)} ocorrência(s) com nível que bloqueia (máximo {maximo}). O merge fica bloqueado.")
        codigo = 1
    else:
        linhas.append(f"✅ **QG-04 cumprido:** {len(bloqueiam)} ocorrências com nível que bloqueia (máximo {maximo}).")
        codigo = 0

    texto = "\n".join(linhas) + "\n"
    with open(destino_relatorio, "w", encoding="utf-8") as relatorio:
        relatorio.write("# Relatório da análise estática — QG-04\n\n" + texto)
    resumo = os.environ.get("GITHUB_STEP_SUMMARY")
    if resumo:
        with open(resumo, "a", encoding="utf-8") as ficheiro:
            ficheiro.write(texto)
    print(texto)
    for r in bloqueiam[:20]:
        print(f"::error title=QG-04 {r['regra']}::{r['local']} — {r['mensagem']}")
    return codigo


if __name__ == "__main__":
    sys.exit(main())

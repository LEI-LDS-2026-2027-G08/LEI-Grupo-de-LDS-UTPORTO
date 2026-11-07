#!/usr/bin/env python3
# =============================================================================
# Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
# Issue          : I103 — Integrar análise estática, cobertura e quality gates
# Ficheiro       : .github/scripts/gate-cobertura.py
# Versão         : m3-v02 (cópia arquivada em
#                  /projeto/04-artefactos-tecnicos/04.16-ci-cd/m3-v02)
# Finalidade     : Quality gate QG-03 (cobertura de linhas ≥ limiar em Domínio e
#                  Serviços). O limiar e o âmbito vêm de .github/quality-gates.json
#                  (critério 1 da I103); nada está escrito no workflow.
# Utilização     : gate-cobertura.py filtros <config>
#                      escreve em GITHUB_OUTPUT os filtros do ReportGenerator
#                      (assemblyfilters, classfilters, filefilters) e o limiar.
#                  gate-cobertura.py avaliar <config> <Cobertura.xml>
#                      lê o relatório Cobertura já filtrado, escreve o resumo e
#                      termina com código 1 se a cobertura de linhas for inferior
#                      ao limiar (o que faz falhar a etapa e bloqueia o merge).
# Dependências   : Python 3 da imagem ubuntu-latest (só biblioteca padrão).
# =============================================================================
import json
import os
import sys
import xml.etree.ElementTree as ET
from fractions import Fraction


def ler_config(caminho):
    with open(caminho, encoding="utf-8") as ficheiro:
        return json.load(ficheiro)["QG-03"]


def escrever_saida(chave, valor):
    destino = os.environ.get("GITHUB_OUTPUT")
    linha = f"{chave}={valor}"
    if destino:
        with open(destino, "a", encoding="utf-8") as saida:
            saida.write(linha + "\n")
    print(linha)


def escrever_resumo(linhas):
    texto = "\n".join(linhas) + "\n"
    destino = os.environ.get("GITHUB_STEP_SUMMARY")
    if destino:
        with open(destino, "a", encoding="utf-8") as resumo:
            resumo.write(texto)
    print(texto)


def filtros(caminho_config):
    qg = ler_config(caminho_config)
    limiar = qg["limiarMinimoPercentagem"]
    if not isinstance(limiar, (int, float)) or not 0 < limiar <= 100:
        raise SystemExit(f"::error title=QG-03::Limiar inválido em {caminho_config}: {limiar!r}")
    assemblies = ";".join("+" + a["assembly"] for a in qg["assembliesIncluidas"])
    classes = ";".join("-" + c["filtro"] for c in qg.get("classesExcluidas", []))
    ficheiros = ";".join("-" + f["filtro"] for f in qg.get("ficheirosExcluidos", []))
    escrever_saida("assemblyfilters", assemblies)
    escrever_saida("classfilters", classes)
    escrever_saida("filefilters", ficheiros)
    escrever_saida("limiar", limiar)


def percentagem(cobertas, total):
    return float(Fraction(cobertas, total) * 100) if total else 0.0


def avaliar(caminho_config, caminho_cobertura):
    qg = ler_config(caminho_config)
    limiar = qg["limiarMinimoPercentagem"]
    raiz = ET.parse(caminho_cobertura).getroot()

    linhas_validas = int(raiz.get("lines-valid", "0"))
    linhas_cobertas = int(raiz.get("lines-covered", "0"))
    ramos_validos = int(raiz.get("branches-valid", "0"))
    ramos_cobertos = int(raiz.get("branches-covered", "0"))

    # Por classe (para o resumo): as classes com menos cobertura no âmbito.
    por_classe = {}
    for classe in raiz.iter("class"):
        nome = classe.get("name", "?")
        linhas = classe.findall("lines/line")
        total = len(linhas)
        cobertas = sum(1 for linha in linhas if int(linha.get("hits", "0")) > 0)
        anterior = por_classe.get(nome, (0, 0))
        por_classe[nome] = (anterior[0] + cobertas, anterior[1] + total)

    # Se o relatório não trouxer os totais na raiz, somam-se as linhas das classes.
    if raiz.get("lines-valid") is None:
        linhas_cobertas = sum(c for c, _ in por_classe.values())
        linhas_validas = sum(t for _, t in por_classe.values())

    assemblies = sorted({p.get("name", "?") for p in raiz.iter("package")})
    cobertura = percentagem(linhas_cobertas, linhas_validas)
    ramos = percentagem(ramos_cobertos, ramos_validos)
    # Comparação exata (frações): 69,99% não passa como 70%.
    cumpre = linhas_validas > 0 and Fraction(linhas_cobertas, linhas_validas) * 100 >= Fraction(str(limiar))

    resumo = [
        "### QG-03 · Cobertura de linhas (Domínio e Serviços)",
        "",
        "| Medida | Valor |",
        "|---|---|",
        f"| Assemblies medidas | {', '.join(f'`{a}`' for a in assemblies) or '—'} |",
        f"| Linhas cobertas / mensuráveis | {linhas_cobertas} / {linhas_validas} |",
        f"| **Cobertura de linhas** | **{cobertura:.2f}%** |",
        f"| Limiar (`.github/quality-gates.json`) | ≥ {limiar}% |",
        f"| Cobertura de ramos (LT-02, informativa) | {ramos:.2f}% (referência {qg.get('informativo', {}).get('referenciaPercentagem', '—')}%) |",
        "",
    ]
    exclusoes = [c["filtro"] for c in qg.get("classesExcluidas", [])] + [f["filtro"] for f in qg.get("ficheirosExcluidos", [])]
    resumo.append("Exclusões aplicadas: " + ", ".join(f"`{e}`" for e in exclusoes) + ".")
    resumo.append("")

    piores = sorted(
        ((nome, c, t) for nome, (c, t) in por_classe.items() if t > 0),
        key=lambda item: (percentagem(item[1], item[2]), -item[2]),
    )[:10]
    if piores:
        resumo += ["<details><summary>As 10 classes com menor cobertura no âmbito</summary>", "",
                   "| Classe | Linhas cobertas | Cobertura |", "|---|---|---|"]
        resumo += [f"| `{nome}` | {c} / {t} | {percentagem(c, t):.1f}% |" for nome, c, t in piores]
        resumo += ["", "</details>", ""]

    if cumpre:
        resumo.append(f"✅ **QG-03 cumprido:** {cobertura:.2f}% ≥ {limiar}%.")
    elif linhas_validas == 0:
        resumo.append("❌ **QG-03 não avaliado:** o relatório não tem linhas mensuráveis no âmbito configurado.")
    else:
        resumo.append(f"❌ **QG-03 não cumprido:** {cobertura:.2f}% < {limiar}%. O merge fica bloqueado.")
    escrever_resumo(resumo)

    if not cumpre:
        print(f"::error title=QG-03 — cobertura::Cobertura de linhas {cobertura:.2f}% abaixo do limiar de {limiar}% (.github/quality-gates.json).")
        return 1
    return 0


def main():
    if len(sys.argv) >= 3 and sys.argv[1] == "filtros":
        filtros(sys.argv[2])
        return 0
    if len(sys.argv) >= 4 and sys.argv[1] == "avaliar":
        return avaliar(sys.argv[2], sys.argv[3])
    print(__doc__ or "Utilização: gate-cobertura.py filtros <config> | avaliar <config> <Cobertura.xml>", file=sys.stderr)
    return 2


if __name__ == "__main__":
    sys.exit(main())

#!/usr/bin/env python3
# =============================================================================
# Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
# Issue          : I102 — Criar a pipeline de compilação e testes do backend
# Ficheiro       : .github/scripts/resumo-testes.py
# Versão         : m3-v01 (cópia arquivada em
#                  /projeto/04-artefactos-tecnicos/04.16-ci-cd/m3-v01)
# Finalidade     : Ler os ficheiros TRX produzidos por `dotnet test` e escrever,
#                  no resumo da execução do GitHub Actions, uma tabela com o
#                  número de testes executados, aprovados, falhados e não
#                  executados, e a lista dos testes falhados.
# Utilização     : python3 resumo-testes.py <pasta-dos-resultados> "<título>"
# Efeito         : Apenas informativo. O bloqueio da pipeline (QG-02) resulta do
#                  código de saída do `dotnet test`, e não deste script, que
#                  termina sempre com código 0 para não esconder esse resultado.
# Dependências   : Python 3 da imagem ubuntu-latest (só biblioteca padrão).
# =============================================================================
import glob
import os
import sys
import xml.etree.ElementTree as ET

NS = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def ler_trx(caminho):
    raiz = ET.parse(caminho).getroot()
    contadores = raiz.find("t:ResultSummary/t:Counters", NS)
    resumo = {
        "total": int(contadores.get("total", 0)) if contadores is not None else 0,
        "executados": int(contadores.get("executed", 0)) if contadores is not None else 0,
        "aprovados": int(contadores.get("passed", 0)) if contadores is not None else 0,
        "falhados": int(contadores.get("failed", 0)) if contadores is not None else 0,
        "nao_executados": int(contadores.get("notExecuted", 0)) if contadores is not None else 0,
    }
    falhados = []
    for resultado in raiz.findall("t:Results/t:UnitTestResult", NS):
        if resultado.get("outcome") == "Failed":
            mensagem = resultado.find("t:Output/t:ErrorInfo/t:Message", NS)
            # Uma mensagem só com espaços dá uma lista vazia: não se lê o índice 0.
            linhas_msg = (mensagem.text or "").strip().splitlines() if mensagem is not None else []
            texto = linhas_msg[0] if linhas_msg else ""
            falhados.append((resultado.get("testName", "?"), texto[:200]))
    return resumo, falhados


def main():
    pasta = sys.argv[1] if len(sys.argv) > 1 else "."
    titulo = sys.argv[2] if len(sys.argv) > 2 else "Testes"
    destino = os.environ.get("GITHUB_STEP_SUMMARY")

    ficheiros = sorted(glob.glob(os.path.join(pasta, "**", "*.trx"), recursive=True))
    linhas = [f"### {titulo}", ""]

    if not ficheiros:
        linhas.append("Não foi produzido nenhum ficheiro TRX. Ver o registo do passo de testes.")
    else:
        total = {"total": 0, "executados": 0, "aprovados": 0, "falhados": 0, "nao_executados": 0}
        todos_falhados = []
        ilegiveis = []
        for ficheiro in ficheiros:
            # Um TRX incompleto (por exemplo, testes interrompidos) não pode fazer
            # falhar este passo informativo: é indicado no resumo e ignorado.
            try:
                resumo, falhados = ler_trx(ficheiro)
            except (ET.ParseError, OSError) as erro:
                ilegiveis.append(f"`{os.path.relpath(ficheiro, pasta)}` não pôde ser lido: {erro}")
                continue
            for chave in total:
                total[chave] += resumo[chave]
            todos_falhados.extend(falhados)

        estado = "✅ Todos os testes passaram" if total["falhados"] == 0 else "❌ Há testes falhados"
        linhas += [
            f"**{estado}** (QG-02: 100% dos testes a passar)",
            "",
            "| Total | Executados | Aprovados | Falhados | Não executados |",
            "|---:|---:|---:|---:|---:|",
            f"| {total['total']} | {total['executados']} | {total['aprovados']} | {total['falhados']} | {total['nao_executados']} |",
            "",
            "Ficheiros TRX: " + ", ".join(f"`{os.path.relpath(f, pasta)}`" for f in ficheiros),
        ]
        if ilegiveis:
            linhas += ["", "⚠️ Ficheiros TRX ignorados:"] + [f"- {texto}" for texto in ilegiveis]
        if todos_falhados:
            linhas += ["", "| Teste falhado | Mensagem |", "|---|---|"]
            for nome, mensagem in todos_falhados:
                linhas.append(f"| `{nome}` | {mensagem.replace('|', '/')} |")

    texto = "\n".join(linhas) + "\n\n"
    print(texto)
    if destino:
        with open(destino, "a", encoding="utf-8") as saida:
            saida.write(texto)
    return 0


if __name__ == "__main__":
    sys.exit(main())

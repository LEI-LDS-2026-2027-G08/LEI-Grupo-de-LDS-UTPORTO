#!/usr/bin/env python3
# =============================================================================
# Projeto        : Gestão de Clínica Veterinária — Grupo G08 — LDS 2026/2027
# Issue          : I103 — Integrar análise estática, cobertura e quality gates
# Ficheiro       : .github/scripts/preparar-newman.py
# Versão         : m3-v02 (cópia arquivada em
#                  /projeto/04-artefactos-tecnicos/04.16-ci-cd/m3-v02)
# Finalidade     : Preparar a execução da Collection Postman com o Newman na
#                  pipeline sem guardar credenciais no repositório nem nos
#                  secrets do GitHub:
#                  1. gera uma palavra-passe descartável e pede ao GitHub Actions
#                     que a esconda em todos os registos (::add-mask::);
#                  2. calcula o hash no formato do PasswordHasher v3 do ASP.NET
#                     Core Identity usado pela API (PBKDF2-HMAC-SHA512, 100 000
#                     iterações, sal de 16 bytes, chave de 32 bytes);
#                  3. escreve o UPDATE que aplica esse hash às contas de
#                     demonstração da I061, só na base de dados temporária do runner;
#                  4. escreve uma cópia do ambiente Postman, fora do repositório,
#                     com as palavras-passe e o baseUrl preenchidos.
# Utilização     : preparar-newman.py <config> <ambiente.json> <copia-ambiente.json> <update.sql> <baseUrl>
# Dependências   : Python 3 da imagem ubuntu-latest (só biblioteca padrão).
# =============================================================================
import base64
import hashlib
import json
import secrets
import string
import struct
import sys

ITERACOES = 100_000
TAMANHO_SAL = 16
TAMANHO_CHAVE = 32
PRF_HMACSHA512 = 2


def hash_identity_v3(palavra_passe, sal=None):
    """Hash no formato v3 do ASP.NET Core Identity (PasswordHasher<TUser>)."""
    sal = sal if sal is not None else secrets.token_bytes(TAMANHO_SAL)
    chave = hashlib.pbkdf2_hmac("sha512", palavra_passe.encode("utf-8"), sal, ITERACOES, TAMANHO_CHAVE)
    cabecalho = b"\x01" + struct.pack(">III", PRF_HMACSHA512, ITERACOES, len(sal))
    return base64.b64encode(cabecalho + sal + chave).decode("ascii")


def gerar_palavra_passe():
    # Cumpre a política da API (maiúscula, minúscula, algarismo e símbolo) e só
    # usa caracteres seguros num literal SQL e num ficheiro JSON.
    alfabeto = string.ascii_letters + string.digits
    corpo = "".join(secrets.choice(alfabeto) for _ in range(28))
    return f"Ci-{corpo}a1A!"


def main():
    if len(sys.argv) != 6:
        print("Utilização: preparar-newman.py <config> <ambiente.json> <copia-ambiente.json> <update.sql> <baseUrl>", file=sys.stderr)
        return 2
    caminho_config, ambiente, copia, sql, base_url = sys.argv[1:]
    with open(caminho_config, encoding="utf-8") as ficheiro:
        dominio = json.load(ficheiro)["Newman"]["dominioContasDemonstracao"]
    if not dominio.startswith("@") or "'" in dominio:
        print(f"::error title=Newman::Domínio das contas de demonstração inválido: {dominio!r}")
        return 1

    palavra_passe = gerar_palavra_passe()
    # Antes de qualquer outra saída: o GitHub Actions passa a esconder o valor.
    print(f"::add-mask::{palavra_passe}", flush=True)

    with open(ambiente, encoding="utf-8-sig") as ficheiro:
        dados = json.load(ficheiro)
    preenchidas = []
    for variavel in dados.get("values", []):
        chave = variavel.get("key", "")
        if chave == "baseUrl":
            variavel["value"] = base_url
        # Só as palavras-passe das contas de demonstração (passwordAdmin, …);
        # passwordInicialTeste é das contas criadas pela própria collection.
        elif chave.startswith("password") and chave != "passwordInicialTeste":
            variavel["value"] = palavra_passe
            preenchidas.append(chave)
    if not preenchidas:
        print("::error title=Newman::O ambiente não tem variáveis password… para preencher.")
        return 1
    with open(copia, "w", encoding="utf-8") as ficheiro:
        json.dump(dados, ficheiro, ensure_ascii=False, indent=2)

    with open(sql, "w", encoding="utf-8") as ficheiro:
        ficheiro.write(
            "SET NOCOUNT ON;\n"
            f"UPDATE dbo.Utilizador SET PasswordHash = N'{hash_identity_v3(palavra_passe)}'\n"
            f"WHERE Email LIKE N'%{dominio}';\n"
            "SELECT CONCAT('Contas de demonstração com a palavra-passe descartável: ', @@ROWCOUNT);\n"
        )

    print(f"Variáveis preenchidas na cópia do ambiente: {', '.join(preenchidas)}; baseUrl = {base_url}")
    print(f"Contas atualizadas: e-mails terminados em {dominio}, só na base de dados temporária do runner.")
    return 0


if __name__ == "__main__":
    sys.exit(main())

``````text
﻿2026-11-04T18:43:56.7979145Z Current runner version: '2.338.2'
2026-11-04T18:43:56.8005100Z ##[group]Runner Image Provisioner
2026-11-04T18:43:56.8006157Z Hosted Compute Agent
2026-11-04T18:43:56.8006795Z Version: 20261028.624
2026-11-04T18:43:56.8007463Z Commit: 54898b74c23fde5556c9b3c3a2290865feef0dfe
2026-11-04T18:43:56.8008320Z Build Date: 2026-10-28T21:51:42Z
2026-11-04T18:43:56.8009068Z Worker ID: {9943f83e-331c-4cf4-86d8-76bf944f9ac7}
2026-11-04T18:43:56.8009960Z Region: eastus2
2026-11-04T18:43:56.8010577Z Cloud: Azure
2026-11-04T18:43:56.8011158Z ##[endgroup]
2026-11-04T18:43:56.8013090Z ##[group]Operating System
2026-11-04T18:43:56.8013805Z Ubuntu
2026-11-04T18:43:56.8014933Z 24.04.5
2026-11-04T18:43:56.8015552Z LTS
2026-11-04T18:43:56.8016158Z ##[endgroup]
2026-11-04T18:43:56.8016821Z ##[group]Runner Image
2026-11-04T18:43:56.8017493Z Image: ubuntu-24.04
2026-11-04T18:43:56.8018248Z Version: 20261101.360.1
2026-11-04T18:43:56.8019577Z Included Software: https://github.com/actions/runner-images/blob/ubuntu24/20261101.360/images/ubuntu/Ubuntu2404-Readme.md
2026-11-04T18:43:56.8021259Z Image Release: https://github.com/actions/runner-images/releases/tag/ubuntu24%2F20261101.360
2026-11-04T18:43:56.8022608Z ##[endgroup]
2026-11-04T18:43:56.8023908Z ##[group]GITHUB_TOKEN Permissions
2026-11-04T18:43:56.8026203Z Contents: read
2026-11-04T18:43:56.8026973Z Metadata: read
2026-11-04T18:43:56.8027626Z ##[endgroup]
2026-11-04T18:43:56.8029952Z Secret source: Actions
2026-11-04T18:43:56.8031021Z Cache mode: write
2026-11-04T18:43:56.8032206Z Prepare workflow directory
2026-11-04T18:43:56.9526592Z Prepare all required actions
2026-11-04T18:43:56.9645519Z Complete job name: Resultado da pipeline
2026-11-04T18:43:57.1229607Z ##[group]Run python3 - <<'PYTHON'
2026-11-04T18:43:57.1230337Z [36;1mpython3 - <<'PYTHON'[0m
2026-11-04T18:43:57.1230912Z [36;1mimport json, os, sys[0m
2026-11-04T18:43:57.1231451Z [36;1mnomes = {[0m
2026-11-04T18:43:57.1232261Z [36;1m    "compilacao": "1 · Restauro e compilação (QG-01)",[0m
2026-11-04T18:43:57.1233113Z [36;1m    "testes-unitarios": "2 · Testes unitários (QG-02)",[0m
2026-11-04T18:43:57.1233938Z [36;1m    "testes-integracao": "3 · Testes de integração (QG-02)",[0m
2026-11-04T18:43:57.1234704Z [36;1m    "cobertura": "4 · Cobertura (QG-03)",[0m
2026-11-04T18:43:57.1235433Z [36;1m    "analise-estatica": "5 · Análise estática (QG-04)",[0m
2026-11-04T18:43:57.1236177Z [36;1m    "formatacao": "6 · Formatação (QG-08)",[0m
2026-11-04T18:43:57.1236870Z [36;1m    "newman": "7 · Collection Postman (Newman)",[0m
2026-11-04T18:43:57.1237529Z [36;1m}[0m
2026-11-04T18:43:57.1238108Z [36;1metapas = json.loads(os.environ["RESULTADOS_ETAPAS"])[0m
2026-11-04T18:43:57.1239075Z [36;1mlinhas = ["### Resultado da pipeline", "", "| Etapa | Resultado |", "|---|---|"][0m
2026-11-04T18:43:57.1239875Z [36;1mfalhas = [][0m
2026-11-04T18:43:57.1240401Z [36;1mfor chave, nome in nomes.items():[0m
2026-11-04T18:43:57.1241174Z [36;1m    resultado = etapas.get(chave, {}).get("result", "desconhecido")[0m
2026-11-04T18:43:57.1242270Z [36;1m    linhas.append(f"| {nome} | {'✅' if resultado == 'success' else '❌'} {resultado} |")[0m
2026-11-04T18:43:57.1243137Z [36;1m    if resultado != "success":[0m
2026-11-04T18:43:57.1243746Z [36;1m        falhas.append(nome)[0m
2026-11-04T18:43:57.1244324Z [36;1mlinhas.append("")[0m
2026-11-04T18:43:57.1244840Z [36;1mif falhas:[0m
2026-11-04T18:43:57.1245742Z [36;1m    linhas.append("**❌ A pipeline falhou: o merge fica bloqueado até todas as etapas terminarem com sucesso.**")[0m
2026-11-04T18:43:57.1246722Z [36;1melse:[0m
2026-11-04T18:43:57.1247368Z [36;1m    linhas.append("**✅ Todas as etapas terminaram com sucesso.**")[0m
2026-11-04T18:43:57.1248360Z [36;1mwith open(os.environ["GITHUB_STEP_SUMMARY"], "a", encoding="utf-8") as resumo:[0m
2026-11-04T18:43:57.1249227Z [36;1m    resumo.write("\n".join(linhas) + "\n")[0m
2026-11-04T18:43:57.1249861Z [36;1mprint("\n".join(linhas))[0m
2026-11-04T18:43:57.1250401Z [36;1mif falhas:[0m
2026-11-04T18:43:57.1250886Z [36;1m    for nome in falhas:[0m
2026-11-04T18:43:57.1252202Z [36;1m        print(f"::error title=Resultado da pipeline::A etapa «{nome}» não terminou com sucesso.")[0m
2026-11-04T18:43:57.1253102Z [36;1m    sys.exit(1)[0m
2026-11-04T18:43:57.1253606Z [36;1mPYTHON[0m
2026-11-04T18:43:57.1473621Z shell: /usr/bin/bash --noprofile --norc -e -o pipefail {0}
2026-11-04T18:43:57.1474602Z env:
2026-11-04T18:43:57.1475061Z   DOTNET_NOLOGO: true
2026-11-04T18:43:57.1475623Z   DOTNET_CLI_TELEMETRY_OPTOUT: true
2026-11-04T18:43:57.1476193Z   CONFIGURACAO: Release
2026-11-04T18:43:57.1476748Z   SOLUCAO: src/backend/ClinicaVeterinaria.sln
2026-11-04T18:43:57.1477738Z   TESTES_UNITARIOS: src/backend/ClinicaVeterinaria.UnitTests/ClinicaVeterinaria.UnitTests.csproj
2026-11-04T18:43:57.1479100Z   TESTES_INTEGRACAO: src/backend/ClinicaVeterinaria.IntegrationTests/ClinicaVeterinaria.IntegrationTests.csproj
2026-11-04T18:43:57.1480145Z   RESULTADOS: resultados-ci
2026-11-04T18:43:57.1480671Z   REPORTGENERATOR_VERSAO: 5.5.11
2026-11-04T18:43:57.1481257Z   QUALITY_GATES: .github/quality-gates.json
2026-11-04T18:43:57.1483880Z   RESULTADOS_ETAPAS: {
  "compilacao": {
    "result": "success",
    "outputs": {}
  },
  "testes-unitarios": {
    "result": "success",
    "outputs": {}
  },
  "testes-integracao": {
    "result": "success",
    "outputs": {}
  },
  "cobertura": {
    "result": "success",
    "outputs": {}
  },
  "analise-estatica": {
    "result": "success",
    "outputs": {}
  },
  "formatacao": {
    "result": "success",
    "outputs": {}
  },
  "newman": {
    "result": "success",
    "outputs": {}
  }
}
2026-11-04T18:43:57.1486294Z ##[endgroup]
2026-11-04T18:43:57.1849989Z ### Resultado da pipeline
2026-11-04T18:43:57.1850566Z 
2026-11-04T18:43:57.1850906Z | Etapa | Resultado |
2026-11-04T18:43:57.1851681Z |---|---|
2026-11-04T18:43:57.1853027Z | 1 · Restauro e compilação (QG-01) | ✅ success |
2026-11-04T18:43:57.1854168Z | 2 · Testes unitários (QG-02) | ✅ success |
2026-11-04T18:43:57.1855246Z | 3 · Testes de integração (QG-02) | ✅ success |
2026-11-04T18:43:57.1856360Z | 4 · Cobertura (QG-03) | ✅ success |
2026-11-04T18:43:57.1857337Z | 5 · Análise estática (QG-04) | ✅ success |
2026-11-04T18:43:57.1858334Z | 6 · Formatação (QG-08) | ✅ success |
2026-11-04T18:43:57.1859353Z | 7 · Collection Postman (Newman) | ✅ success |
2026-11-04T18:43:57.1860040Z 
2026-11-04T18:43:57.1860517Z **✅ Todas as etapas terminaram com sucesso.**
2026-11-04T18:43:57.2044301Z Cleaning up orphan processes
``````

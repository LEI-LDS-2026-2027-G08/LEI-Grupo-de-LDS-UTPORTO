# I127 — Validação integrada da aplicação móvel

Entrega do Executor para revisão. A revisão independente, a auditoria e a integração ainda não são declaradas concluídas.

## Ordem de leitura

1. [Roteiro e resultados](m4-i127-roteiro-resultados-v01.md): fluxos, ambientes, contas, resultados e capturas.
2. [Verificações automáticas](m4-i127-verificacoes-automaticas-v01.md): compilação, testes, formatação, Android Lint e configuração dos ambientes.
3. [Ocorrências e decisões](m4-i127-ocorrencias-decisoes-v01.md): configuração local, dados de demonstração e funcionalidades fora do âmbito.
4. `m4-frontend-mobile-fontes-v01.zip`: fontes extraídas de um commit identificado, com manifesto de integridade.

Issue: [I127 — #115](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/115).

## Ambientes

| Ambiente | URL configurada |
|---|---|
| Emulador, predefinição local | `http://10.0.2.2:5159` |
| Local através de USB ou túnel do emulador | `http://localhost:5159`, com `adb reverse tcp:5159 tcp:5159` |
| Público, Azure | `https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net` |

Na pasta `src/frontend-mobile`, com o Android SDK e o JDK configurados:

```sh
# Backend público
./gradlew installDebug -PapiBaseUrl=https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net

# Backend local, predefinição do emulador
./gradlew installDebug

# Alternativa local quando 10.0.2.2 não comunica
adb reverse tcp:5159 tcp:5159
./gradlew installDebug -PapiBaseUrl=http://localhost:5159
```

A mudança de ambiente exige recompilar/reinstalar a aplicação, sem editar o código. Terminar a sessão antes da mudança: os tokens pertencem ao ambiente que os emitiu. O ecrã técnico de debug permite conferir a URL, sem mostrar o token.

Para a defesa individual, instalar a configuração local. A última compilação desta consolidação foi reposta em `http://10.0.2.2:5159`; não foi instalada num dispositivo nesta sessão.

## Contas e dados

- João: `cliente.joao@clinica-g08.test`; Bobi, ID 1, e Mia, ID 3.
- Maria: `cliente.maria@clinica-g08.test`; Kiko, ID 2.
- As palavras-passe são facultadas por meio privado e não integram esta entrega.
- As marcações futuras dos testes locais são de 30/11/2026: Bobi às 09:00 e Kiko às 09:30. Se a reprodução ocorrer depois destas datas, preparar novas marcações futuras pela Receção.
- Os dados públicos são independentes dos locais. Uma lista de próximas consultas vazia é um resultado válido quando não existem marcações futuras.

Despesas, pagamentos e internamentos estão fora do âmbito. Referência: [Plano de Testes Mobile v01, secção 3.2](../../../03-artefactos-de-qualidade-de-produto/03.03-frontend-mobile/03.03.01-plano-de-testes/m4-plano-testes-frontend-mobile-v01.md). O texto antigo da I127 que menciona despesas deve ser alinhado com esta decisão; não se implementou o RF042.

## Arquivo das fontes

Commit de origem: `4b82347dcacaf9e8b082b6675911b42d51f9efae`.

O ZIP inclui `src/frontend-mobile` e a `.editorconfig` da raiz desse commit. Exclui configurações locais, `.idea`, caches e resultados de compilação. O manifesto SHA-256 interno identifica os ficheiros incluídos. As evidências e os relatórios desta I127 estão fora do ZIP e acompanham-no nesta pasta.

O commit final desta entrega e o endereço do PR devem ser registados quando o Executor os criar. Não se atribui um commit às alterações que ainda estão por guardar no Git.

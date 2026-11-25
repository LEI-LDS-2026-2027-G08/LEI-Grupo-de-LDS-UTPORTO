# I127 — Verificações automáticas — v01

Data: 23/11/2026. Código verificado: `4b82347dcacaf9e8b082b6675911b42d51f9efae`.

Estas verificações complementam o roteiro manual confirmado pelo Executor. Os testes de componentes e serviços e os pedidos diretos à API não são apresentados como execução da interface Android contra um backend real.

## 1. Compilação e qualidade

Executado na pasta `src/frontend-mobile`, com o JDK incluído no Android Studio e o SDK Android local:

```sh
./gradlew testDebugUnitTest ktlintCheck lintDebug assembleDebug --console=plain
```

Resultado: `BUILD SUCCESSFUL in 53s`. [Registos completos das três compilações](m4-i127-registos-verificacao-v01.md); [contagens por suite](m4-i127-testes-resultados-v01.json).

| Verificação | Resultado |
|---|---|
| Testes unitários e de componentes | 166 testes, em 26 suites; zero falhas, zero erros e zero ignorados |
| Formatação — ktlintCheck | Passou |
| Android Lint — lintDebug | Passou |
| Compilação — assembleDebug | Passou |

Os testes usam dados e servidores falsos, incluindo testes de componentes com Robolectric. Não representam 166 execuções contra o Azure ou contra o backend local.

## 2. Configuração dos dois ambientes

Foi executada a compilação pública:

```sh
./gradlew assembleDebug -PapiBaseUrl=https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net --console=plain
```

Resultado: `BUILD SUCCESSFUL in 2s`. O `BuildConfig.java` gerado continha a URL pública em `API_BASE_URL`.

Depois foi executada a compilação local, sem a propriedade:

```sh
./gradlew assembleDebug --console=plain
```

Resultado: `BUILD SUCCESSFUL in 1s`. O `BuildConfig.java` gerado voltou a conter `http://10.0.2.2:5159` em `API_BASE_URL`.

Não foi necessário alterar código ou configurar o backend público como predefinição. Nenhum APK foi instalado num dispositivo nesta sessão; a aplicação já instalada no emulador ou telemóvel deve ser conferida antes da defesa.

## 3. Pedidos diretos à API pública

Resultado: **31 verificações, zero falhas**, com as contas João e Maria. Os detalhes sem tokens nem palavras-passe estão em [m4-i127-api-publica-resultados-v01.json](m4-i127-api-publica-resultados-v01.json).

Foram verificados:

- início de sessão com as duas contas (`200`);
- listagem dos animais próprios (`200`), detalhe e boletim dos três animais (`200`);
- consultas futuras e histórico, com e sem filtro de animal (`200`);
- detalhe, boletim e consultas de um animal da outra conta, em ambos os sentidos (`403`);
- terminar as duas sessões criadas para esta verificação (`204`);
- `/auth/me` sem sessão (`401`).

As palavras-passe foram lidas do ficheiro privado disponibilizado pelo Executor; os tokens permaneceram apenas em memória. O relatório guarda rotas, códigos e contagens, sem credenciais. Não foram criadas ou alteradas marcações nem outros dados clínicos ou administrativos.

Durante o contacto inicial com o serviço público houve timeout e uma resposta `503`. Sem alteração do serviço, o Swagger voltou a responder `200` e as 31 verificações seguintes passaram. A causa não foi determinada; não se declara uma correção efetuada nem um defeito persistente. Se a indisponibilidade se repetir, deve ser registada com a hora e os pedidos afetados para diagnóstico do ambiente público.

O backend local estava desligado no início desta consolidação. Os seus resultados de utilização no Android são os confirmados pelo Executor e documentados nas capturas; não são apresentados como novos pedidos automáticos locais.

## 4. Integridade do arquivo

`m4-frontend-mobile-fontes-v01.zip`: 136 ficheiros de origem, extraídos do commit identificado. A origem e os SHA-256 de cada ficheiro constam dos manifestos internos. A soma do ZIP está em `m4-frontend-mobile-fontes-v01.sha256`.

As 14 capturas foram copiadas sem alteração. A correspondência entre os nomes originais, os nomes organizados e as somas SHA-256 consta de `m4-i127-manifesto-evidencias-v01.json`.

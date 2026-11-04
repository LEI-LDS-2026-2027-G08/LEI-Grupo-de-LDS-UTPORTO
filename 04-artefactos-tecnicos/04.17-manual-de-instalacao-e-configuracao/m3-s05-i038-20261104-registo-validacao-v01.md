# Registo de validação do Manual de Instalação e Configuração do backend

| Campo | Valor |
|---|---|
| Grupo | G08 — Gestão de Clínica Veterinária |
| Issue | I038 (GitHub #67) — critério de aceitação 4 |
| Manual validado | `m3-manual-instalacao-configuracao-v01.md` / `.pdf`, commit: _(preencher)_ |
| Autor do manual | André Filipe Moreira Mesquita |
| Validado por | _(nome do elemento do grupo, diferente do autor)_ |
| Data e hora | _(início — fim)_ |
| Máquina | _(modelo ou nome, Windows 10/11, versão)_ |
| Ambiente limpo | _(conta Windows nova, máquina virtual ou computador sem o projeto; o que estava instalado antes)_ |
| Opção de base de dados | _(A — SQL Server instalado / B — Docker)_ |
| Versões usadas | .NET SDK: ___ · dotnet-ef: ___ · SQL Server: ___ · sqlcmd: ___ · PowerShell: ___ |

## Resultado por passo

| Passo | Resultado esperado obtido? | Observações e desvios |
|---|---|---|
| 1.1 Clonar | Sim / Não | |
| 1.2 Confirmar o SDK | Sim / Não | |
| 1.3 Instalar o dotnet-ef | Sim / Não | |
| 1.4 Compilar | Sim / Não | |
| 2.A ou 2.B Base de dados | Sim / Não | |
| 3.1 Criar a base e aplicar as migrações | Sim / Não | |
| 3.2 Dados de demonstração (28/28 OK) | Sim / Não | |
| 4.1 Chave JWT | Sim / Não | |
| 4.2 Cadeia de ligação | Sim / Não | |
| 5.1 Arrancar a API | Sim / Não | |
| 6.1 401 sem token | Sim / Não | |
| 6.2 Swagger | Sim / Não | |
| 6.3 Login | Sim / Não | |
| 6.4 GET /clinica autenticado | Sim / Não | |
| 8 Reposição (opcional) | Sim / Não / Não feito | |

## Desvios encontrados e correções

| N.º | Passo | Desvio | Correção no manual (commit) |
|---|---|---|---|
| 1 | | | |

## Conclusão

_(O manual permite instalar e verificar o backend num ambiente limpo: sim / sim, depois das correções indicadas / não.)_

Não registar neste ficheiro palavras-passe, chaves JWT nem cadeias de ligação com palavra-passe.

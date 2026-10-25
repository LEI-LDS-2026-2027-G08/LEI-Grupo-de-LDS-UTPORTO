# Política de Segurança

Este repositório contém o projeto **Gestão de Clínica Veterinária**, desenvolvido pelo grupo G08 no âmbito da unidade curricular de Laboratório de Desenvolvimento de Software (ESTG, Politécnico do Porto). É um projeto académico, em desenvolvimento, que **não está em produção** e **não deve conter dados reais** de clientes, animais ou funcionários.

## Versões suportadas

O projeto evolui por milestones (`m1` a `m4`), publicadas como tags e Releases. Só a versão mais recente recebe correções de segurança.

| Versão                                   | Suportada          |
| ---------------------------------------- | ------------------ |
| Branch `main` e última Release (`m<n>`)  | :white_check_mark: |
| Branch `develop` (trabalho em curso)      | :white_check_mark: |
| Releases anteriores (`m1`, `m2`, ...)     | :x:                |

## Âmbito

Estão abrangidos por esta política:

- O backend (ASP.NET Core 8, Entity Framework Core 8, Microsoft SQL Server), incluindo autenticação JWT, autorização por perfil e isolamento dos dados dos Clientes.
- Os frontends web e mobile, quando existirem neste repositório.
- Os scripts de base de dados, a configuração e os workflows do repositório.

Exemplos do que nos interessa: contornar a autenticação ou a autorização, aceder a dados de outro Cliente ou de outro perfil, injeção de SQL, exposição de segredos ou de dados pessoais, e dependências com vulnerabilidades conhecidas.

## Como reportar uma vulnerabilidade

**Não abras uma Issue pública** para reportar uma vulnerabilidade.

Usa o reporte privado do GitHub:

1. Vai ao separador **Security** do repositório.
2. Escolhe **Report a vulnerability**.
3. Descreve o problema, os passos para o reproduzir, o impacto e, se possível, uma sugestão de correção.

Se não conseguires usar o reporte privado, contacta um dos membros do grupo através do perfil de GitHub, sem incluir detalhes técnicos da vulnerabilidade na primeira mensagem, e combinamos um canal privado.

Nunca incluas segredos, palavras-passe nem dados pessoais reais no reporte.

## O que podes esperar

- **Confirmação de receção** em até **5 dias úteis**.
- **Avaliação inicial** (aceite ou recusada, com justificação) em até **10 dias úteis**.
- **Atualizações** pelo menos de 15 em 15 dias enquanto o reporte estiver aberto.
- Se for **aceite**, registamos o defeito, corrigimos conforme a severidade (Crítica e Alta têm prioridade sobre qualquer outra tarefa) e damos-te crédito, se o desejares, quando a correção for publicada.
- Se for **recusada**, explicamos porquê (por exemplo, fora do âmbito, comportamento esperado ou impossível de reproduzir).

Por se tratar de um projeto académico mantido por um grupo de estudantes, estes prazos podem alargar-se em períodos de avaliação ou férias. Dizemos-te sempre.

## Boas práticas para quem contribui

- Não submeter segredos (chaves JWT, cadeias de ligação, palavras-passe, tokens) ao repositório. Usar variáveis de ambiente ou *User Secrets*.
- Usar apenas dados fictícios nos testes e nos exemplos.
- Todas as alterações entram por Pull Request com revisão.

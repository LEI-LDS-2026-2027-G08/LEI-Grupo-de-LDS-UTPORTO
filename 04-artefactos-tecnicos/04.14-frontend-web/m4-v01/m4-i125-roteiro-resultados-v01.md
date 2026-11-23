# I125 — ROTEIRO E RESULTADOS — EXECUÇÃO DE 22/11/2026

Consolidação documental em 23/11/2026 após observações do Revisor. Estado: trabalho técnico e evidências preparados para revisão. Não é aprovação final, homologação, revisão independente ou auditoria. Issue: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/issues/39

## RESULTADO

Fluxo integrado executado com API e SQL Server reais por automação local, com os resultados e limites descritos abaixo. O Executor repetiu o roteiro manual e confirmou que passou; o registo dessa confirmação está em [m4-i125-teste-manual-executor-v01.md](m4-i125-teste-manual-executor-v01.md). As execuções automatizadas e a confirmação manual do Executor são evidências distintas. As correções de acesso clínico e disponibilidade foram publicadas nos commits 16c8513 e f54d2de e submetidas no PR #283. Defeitos registados: #281 (backend/web) e #282 (frontend). Permanecem abertos até revisão/integração ou decisão formal aplicável.

## AMBIENTE

Frontend http://localhost:5173; API http://localhost:5159; SQL Server local Docker. Backend .NET 10; Chromium/Playwright. Configuração local .env criada a partir de .env.example; CORS permite localhost:5173. Palavras-passe das contas disponíveis apenas no ficheiro privado fornecido pelo Executor/KeePass; não publicar esse ficheiro com a entrega.

## CONTAS USADAS

Admin: admin@clinica-g08.test — menus Clientes, Funcionários e Clínica. Receção: rececao@clinica-g08.test — criação administrativa e agenda. Veterinária Ana: vet.ana@clinica-g08.test — registo clínico e histórico completo. Veterinário Bruno: vet.bruno@clinica-g08.test — login/menu por perfil. Auxiliar Carla: aux.carla@clinica-g08.test — preparação com registo de apoio. Auxiliar Diogo: aux.diogo@clinica-g08.test — preparação sem registo de apoio. Cliente João: cliente.joao@clinica-g08.test — recusado na aplicação web. Conta bloqueada: rececao.bloqueada@clinica-g08.test — login recebe 403. O Revisor deve usar contas independentes para a sua verificação; não se declara aqui executada a revisão independente.

## DADOS CRIADOS NA EXECUÇÃO

Cliente 2002: Teste I125 9998170 Paciente 2002: Bobi I125 9998170 Consulta 2002: 2026-11-23T09:00:00Z (UTC), Veterinária Ana. Registo clínico 1002. Tratamento e medicamento identificados como teste. Os dados foram criados na base local usada na execução. Não foi confirmada a sua permanência após eventuais reposições posteriores. Noutra base, criar dados equivalentes com nomes e endereços de e-mail únicos, em vez de assumir estes identificadores.

## ROTEIRO PARA REPETIR

1. Entrar como Rececionista. Clientes > Novo cliente: nome de teste, telefone 912345678, e-mail único e palavra-passe de teste forte. NIF/morada opcionais. Guardar. Confirmar ficha e persistência.
2. Pacientes > Novo paciente: procurar o proprietário pelo e-mail, selecionar, indicar nome único, espécie Cão e sexo Macho. Guardar.
3. Ficha do paciente > Nova marcação. Selecionar Veterinária Ana, consultar disponibilidade e escolher um horário livre. Criar com motivo de teste.
4. No detalhe: Confirmar marcação > Confirmar; Registar chegada. A API aceitou chegada para uma consulta do dia seguinte; não se comprovou uma regra que limite a chegada ao próprio dia e não se alterou o relógio.
5. Entrar como Carla. Preparação > Dia da consulta > Selecionar a consulta do paciente. Guardar uma nota não clínica de teste. Confirmar que aparece.
6. Entrar como Ana. Abrir /agenda/ID_DA_CONSULTA/registo-clinico. Guardar notas e diagnóstico identificados como teste. Abrir histórico do paciente.
7. Voltar ao registo da consulta. Adicionar um tratamento e uma prescrição fictícios, com descrição, data, medicamento, dosagem, frequência e duração. Guardar, recarregar e confirmar persistência e restauro da sessão.
8. Entrar como Carla e consultar o histórico do paciente. Abrir o registo. Tratamento/prescrição presentes; coluna Diagnóstico e campos de notas e diagnóstico ausentes. O backend também omite as propriedades no JSON.
9. Repetir como Diogo: leitura reduzida disponível. Na Preparação, tentar guardar apoio; 403 com explicação de falta de autorização.
10. Rececionista em /funcionarios: acesso negado. Terminar sessão e tentar /agenda: regressa ao login. Cliente: aplicação web recusada após login aceite pela API. Conta bloqueada: 403 e mensagem fixa de conta bloqueada.

## EVIDÊNCIAS

Os caminhos evidencias/... abaixo são internos ao ZIP m4-i125-evidencias-v01.zip desta pasta. As capturas manuais do Executor estão anexadas ao PR #283. Não se confundem com as capturas da automação. evidencias/i125-perfis.json e capturas i125-perfil-*.png: início de sessão/menu. evidencias/i125-fluxo.json: identificadores e resultados das oito etapas principais. Capturas i125-cliente, paciente, marcacao, chegada, preparacao, registo-clinico, historico, edicao-clinica e auxiliar-tratamentos-prescricoes.png. evidencias/i125-verificacoes.json: apoio sem autorização, acesso clínico reduzido, rota recusada, registo duplicado 409 e logout. Capturas correspondentes. evidencias/i125-422-real.json: sobreposição de marcação recusada com 422 real. evidencias/i125-erros.json: 28 falhas simuladas na leitura em sete ecrãs. evidencias/i125-erros-gravacao.json: 16 falhas simuladas na gravação em quatro formulários (cliente, paciente, marcação e registo clínico). As 44 verificações simulam 401/403/422/perda de ligação. Em 401, volta ao login com aviso de sessão terminada; nos outros casos apresenta mensagem de erro. São simulações de falhas HTTP/rede no navegador, não 44 falhas reais do backend.

## TESTES LOCAIS

Backend: 751 testes unitários e 470 de integração aprovados; zero ignorados. Frontend: 316 testes aprovados; compilação e lint aprovados. Chromium: dois testes de acesso sem sessão aprovados. Backend Release: zero erros e zero avisos. Swagger: duas respostas por perfil em anyOf nos GET EP-033 e EP-034. Formatação dos ficheiros C# alterados verificada conforme .editorconfig.

## ARQUIVO — ESTADO E LIMITES

Backend: 04-artefactos-tecnicos/04.12-backend/m4-v02, sem alterar m3-v01. Frontend: 04-artefactos-tecnicos/04.14-frontend-web/m4-v01. Os ZIP submetidos foram preparados antes dos commits, sobre a base 55c4c9f, e têm manifesto SHA-256 e configuração privada excluída/limpa. O Revisor assinalou que essa base precede outras integrações do develop. Não são aqui certificados como arquivo final alinhado com a base atual: falta comparar as versões e regenerar os arquivos a partir da versão de código identificada para aceitação. O manifesto prova integridade, não atualidade ou aprovação. API v04 preparada como rascunho, preservando a v03. A proposta documental anexa identifica RF036, UC13, modelos, RTM e especificações de testes afetadas.

## ACEITAÇÃO — PENDÊNCIAS EXPLÍCITAS

CA 4 NÃO CONCLUÍDO: falta arquivar a referência formal da decisão e a autorização aplicável à alteração do contrato. O acordo do grupo comunicado pelo Executor não é apresentado como essa autorização. A API v04 continua a ser um rascunho, não uma aprovação. Fechar a revisão documental. Os DTOs foram colocados na pasta correspondente ao namespace `RegistosClinicos`, e os finais de linha do backend foram repostos em LF para evitar alterações de ficheiro completo. A base do `develop`, incluindo I118 e I123, foi integrada no commit `4b22f3f`. Falta regenerar os ZIP a partir do commit de código identificado para aceitação; os arquivos antigos não são apresentados como a versão final atualizada. Revisor repetir os fluxos com contas independentes; concluir relatório de revisão, auditoria e aceitação/merge. A tarefa não fica concluída apenas por este relatório.

## REFERÊNCIAS DA ENTREGA

PR #283: https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/pull/283 Defeitos: #281 e #282. Resultado Newman atualizado após a falha do teste antigo do Auxiliar: 03-artefactos-de-qualidade-de-produto/03.01-backend/03.01.05-postman/ m4-i125-20261122-resultado-newman-v02.txt — 270 pedidos, 865 verificações, zero falhas em execução local com base temporária. A execução do Postman é distinta dos testes manuais e não foi contada como revisão independente.

## VERIFICAÇÃO DAS CORREÇÕES DA REVISÃO EM 23/11/2026

- DTOs reduzidos na pasta `DTOs/RegistosClinicos`, alinhada com o namespace.
- Finais de linha do backend repostos em LF, conforme a base original. A pipeline normaliza a cópia de trabalho para CRLF antes da verificação de formatação, como já previsto no workflow.
- Integração do `origin/develop` em `c45180e`, incluindo I118, I123, I129, I136 e I140, concluída sem conflitos e registada no commit `4b22f3f`.
- Backend: 751 testes unitários aprovados, zero ignorados, após a reorganização dos DTOs.
- Frontend após integração da base: 338 testes aprovados; compilação e lint aprovados.
- Não foi feita uma nova execução manual dos fluxos nesta revisão documental e de organização.
- Os ZIP ainda não foram regenerados: devem identificar o commit de código já guardado, em vez de uma cópia de alterações intermédias.

A autorização aplicável ao contrato continua pendente de comprovação. Estes resultados não a substituem.

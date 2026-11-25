# I125 — PROPOSTA DOCUMENTAL PARA REVISÃO — 22/11/2026

Não substitui documentos aprovados ou homologados. Decisão funcional comunicada pelo Executor: o grupo concordou em reservar notas clínicas e diagnóstico ao Veterinário e disponibilizar tratamentos e prescrições ao Auxiliar. A decisão funcional está registada na [m4-s07-20261122-sprint-review-ata-v01.pdf](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/blob/bf68c704f2823c520e5ef030431e97c9842f0138/01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261122-sprint-review-ata-v01.pdf), deliberação 5. A revisão e a auditoria permanecem pendentes.

## API

Nova versão preparada no repositório: 04-artefactos-tecnicos/04.11-documentacao-de-api/m4-documentacao-api-v04.md A v03 foi preservada. A v04 identifica-se como rascunho para revisão/auditoria. EP-033: resposta completa ao Veterinário; resposta reduzida ao Auxiliar, com id, consultaId, animalId, veterinario, criadoEm, tratamentos, prescricoes, atualizadoEm e atualizadoPor. NotasClinicas e Diagnostico não existem no DTO. EP-034: animal e registos com id, consultaId, data, veterinario, numeroTratamentos e numeroPrescricoes. Diagnostico omitido no Auxiliar. Swagger documenta as duas formas de resposta em anyOf nos dois GET.

## RF036 — TEXTO PROPOSTO

O sistema deve permitir ao Auxiliar Veterinário consultar os tratamentos e as prescrições associados aos registos clínicos dos pacientes, exclusivamente em modo de leitura. As notas clínicas e o diagnóstico são reservados ao Veterinário e não são disponibilizados ao Auxiliar, nem no detalhe nem no histórico. Os dados de identificação e contexto necessários à consulta são mantidos. Esta alteração não decide novas restrições por Veterinário/paciente/consulta.

## UC13 — FLUXO PROPOSTO

1. Auxiliar autenticado seleciona o paciente e abre o histórico.
2. Sistema apresenta data, Veterinário e contagens de tratamentos/prescrições, sem diagnóstico.
3. Auxiliar abre um registo.
4. Sistema apresenta tratamentos e prescrições com o contexto do registo, sem notas clínicas nem diagnóstico e sem ações de escrita. Tentativa de escrita pelo Auxiliar: 403. Perfis fora do acesso clínico: 403. Sem token válido: 401.

## MODELOS DE CLASSES

API: acrescentar RegistoClinicoAuxiliarDto, HistoricoClinicoAuxiliarDto e RegistoClinicoResumoAuxiliarDto; mapeamentos próprios a partir dos modelos internos. Serviços/interfaces mantêm modelos da Application, sem DTOs HTTP. Frontend: diagnostico/notasClinicas podem estar ausentes na leitura; coluna Diagnóstico e campos de notas/diagnóstico apenas no perfil Veterinário. Não há nova tabela nem migração: a mudança limita a resposta HTTP.

## RTM — ALTERAÇÃO PROPOSTA PARA A LINHA RF036

UC13; EP-033 e EP-034; I078/I116 e correção da I125 #39, defeito #281. Implementação: RegistosClinicosController, DTOs reduzidos, mapeamento, HistoricoClinicoPage. Testes: RegistosClinicosControllerTests, RegistosClinicosLeituraEndpointsTests, historicoClinico.test.tsx; evidências evidencias/i125-auxiliar-acesso-clinico.png e evidencias/i125-auxiliar-tratamentos-prescricoes.png. Atualizar a versão atual da RTM, após confirmar a cópia aprovada do grupo. Não foi editado um ficheiro Excel antigo para evitar substituir a versão oficial.

## DECLARAÇÃO DE ÂMBITO E AUTORIZAÇÃO

A [m4-s07-20261122-sprint-review-ata-v01.pdf](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/blob/bf68c704f2823c520e5ef030431e97c9842f0138/01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261122-sprint-review-ata-v01.pdf), deliberação 5 formaliza a decisão do grupo sobre EP-033 e EP-034, no âmbito do RF036. A referência é reproduzida no campo «Estado» da API v04 e nos documentos de alinhamento. Qualquer alteração posterior ao âmbito homologado segue as secções 7.4–7.5 do regulamento.

## TESTES DOCUMENTAIS

Rever o pressuposto antigo que permitia ao Auxiliar o mesmo DTO do Veterinário (PA-08/contrato anterior) nas especificações de testes. Substituir por casos que verifiquem ausência das propriedades reservadas no JSON e ausência dos campos no ecrã, mantendo tratamentos/prescrições. A implementação dos testes afetados já foi preparada; as especificações aprovadas precisam de revisão coordenada.

## REGISTO DA DECISÃO PARA O CA 4

Referência: [m4-s07-20261122-sprint-review-ata-v01.pdf](https://github.com/LEI-LDS-2026-2027-G08/LEI-Grupo-de-LDS-UTPORTO/blob/bf68c704f2823c520e5ef030431e97c9842f0138/01-artefactos-de-gestao/01.03-reunioes-de-grupo/m4-s07-20261122-sprint-review-ata-v01.pdf), deliberação 5.

A decisão foi aprovada por unanimidade pelo grupo: o Auxiliar deixa de receber notas clínicas e diagnóstico em EP-033 e EP-034; mantém os restantes elementos autorizados. O Veterinário continua a receber o registo completo. A alteração fica registada na API v04 e é implementada e verificada na I125.

A referência consta também do PR #283. A confirmação final dos critérios, a revisão e a auditoria continuam a cargo dos respetivos responsáveis.

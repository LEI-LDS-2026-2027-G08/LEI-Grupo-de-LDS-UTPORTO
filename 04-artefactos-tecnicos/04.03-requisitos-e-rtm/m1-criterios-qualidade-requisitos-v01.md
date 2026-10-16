# Critérios de Qualidade dos Requisitos

Este documento define os critérios objetivos segundo os quais todos os requisitos funcionais e não funcionais do sistema de **Gestão de Clínica Veterinária** são avaliados. Um requisito só pode ser aprovado quando cumprir todos os critérios que lhe sejam aplicáveis e quando os critérios considerados não aplicáveis estiverem devidamente justificados.

A avaliação destes critérios é realizada de forma independente pelo Revisor e pelo Auditor da tarefa, sendo o resultado registado na **Checklist de Aprovação dos Requisitos**. Sempre que um requisito não cumpra um critério aplicável, a regra de validação aqui definida serve de base para redigir a justificação obrigatória na checklist.

### 1. Correção

O requisito descreve uma funcionalidade, regra ou restrição verdadeira e válida para o contexto do sistema de Gestão de Clínica Veterinária.

* **Regra de Validação:** Reprova se introduzir comportamentos, regras ou permissões que contradigam as necessidades factuais da clínica, a Proposta de Sistema ou a Declaração de Âmbito homologadas.

### 2. Clareza

A redação do requisito é simples, direta e utiliza vocabulário acessível, de forma a que qualquer elemento do grupo (Executor, Revisor ou Auditor) compreenda exatamente o mesmo comportamento esperado sem necessitar de explicações adicionais.

* **Regra de Validação:** Reprova se a frase for excessivamente complexa, técnica para um utilizador não-técnico, ou confusa na sua estrutura gramatical.

### 3. Ausência de Ambiguidade

O requisito não permite múltiplas interpretações. A linguagem subjetiva deve ser eliminada ou acompanhada de condições, métricas ou limites objetivos que permitam uma interpretação inequívoca.

* **Regra de Validação:** Reprova se utilizar palavras ou expressões vagas e não mensuráveis, como "rapidamente", "fácil", "normalmente", "etc." ou "de forma adequada", sem critérios objetivos que determinem o seu significado. Expressões como "quando aplicável" só são admissíveis quando a condição de aplicabilidade estiver claramente definida ou for justificável no contexto do requisito.

### 4. Completude

O requisito é autossuficiente e contém toda a informação indispensável para que o Executor consiga implementá-lo sem assumir comportamentos não documentados.

* **Regra de Validação:** Reprova se omitir informação indispensável ao seu tipo, incluindo, quando aplicável, Ator, Ação, Resultado Esperado, condições, restrições e Critérios de Aceitação.

### 5. Consistência

O requisito não entra em conflito com outros requisitos já especificados e respeita o glossário e a padronização de termos do projeto.

* **Regra de Validação:** Reprova se utilizar terminologia divergente (ex.: referir-se à mesma entidade alternando entre "Cliente", "Dono" e "Proprietário", ou alternar entre "Internamento" e "Hospitalização") ou se contradizer um limite já estabelecido.

### 6. Necessidade

O requisito responde a uma necessidade efetiva do sistema, a uma regra de negócio, a uma necessidade do utilizador, a uma obrigação da Unidade Curricular ou a uma restrição assumida no âmbito aprovado.

* **Regra de Validação:** Reprova se não existir uma origem ou justificação identificável para o requisito, ou se introduzir funcionalidade adicional sem relação com o âmbito homologado, com as necessidades do sistema ou com uma obrigação formal do projeto.

### 7. Viabilidade

A implementação e demonstração do requisito são tecnicamente possíveis dentro do calendário da Unidade Curricular, dos recursos disponíveis, da stack tecnológica definida pelo grupo e dos limites estabelecidos no âmbito homologado.

* **Regra de Validação:** Reprova se o requisito exigir tecnologia, integração, recurso, prazo ou capacidade incompatível com o âmbito aprovado, com as exclusões declaradas, com os componentes previstos ou com o calendário académico. A estimativa e eventual decomposição das tarefas necessárias à sua implementação são tratadas posteriormente no Backlog de Projeto e no Backlog Refinement.

### 8. Verificabilidade

O requisito expõe condições suficientes para que se possa comprovar, de forma inequívoca e reprodutível, se foi ou não cumprido, através da produção de uma evidência.

* **Regra de Validação:** Reprova se não for possível definir uma forma objetiva e reprodutível de verificar o cumprimento do requisito, através de teste, inspeção, análise ou outro método de verificação adequado ao seu tipo.

### 9. Rastreabilidade

O requisito possui um identificador único e estável, um título identificável e uma origem ou relação funcional reconhecível, permitindo a sua ligação aos restantes artefactos do projeto através da **Requirements Traceability Matrix** (RTM).

* **Regra de Validação:** Reprova se não possuir um identificador único e um título que permitam distingui-lo inequivocamente, ou se não for possível relacioná-lo com a respetiva origem, funcionalidade ou artefactos posteriores na RTM. O grupo pode adotar internamente um padrão de identificação, como `REQ-F001-01`, desde que seja aplicado de forma consistente.

### 10. Singularidade

O requisito foca-se numa e apenas numa ação, regra ou restrição principal. É atómico, o que facilita a atribuição de tarefas e a granularidade dos testes e commits.

* **Regra de Validação:** Reprova se agrupar várias obrigações independentes que possam ser implementadas, verificadas ou aprovadas separadamente. Nesses casos, deve ser dividido em múltiplos requisitos.

### 11. Independência da Solução

O requisito foca-se no *O QUE* o sistema deve fazer, evitando impor desnecessariamente *COMO* o sistema deve ser implementado tecnicamente.

* **Regra de Validação:** Reprova se incluir vocabulário de implementação técnica ou arquitetural que limite desnecessariamente o desenvolvimento, tal como "tabela SQL", "chave estrangeira", "componente React", "endpoint da API" ou "variável local". São admitidas exceções quando a restrição técnica resulte de uma decisão, obrigação ou requisito não funcional expressamente aprovado e relevante para o projeto.

``````text
newman

Clínica Veterinária G08 — API m3

Folder 00 · Preparação — sessões por perfil
Sub-folder EP-001 · 200 — iniciar sessão (Administrador)
  POST http://localhost:5159/auth/login [200 OK, 714B, 549ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil Administrador
  Pass  O Administrador não tem clienteId

Sub-folder EP-001 · 200 — iniciar sessão (Rececionista)
  POST http://localhost:5159/auth/login [200 OK, 719B, 154ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil Rececionista

Sub-folder EP-001 · 200 — iniciar sessão (Veterinário)
  POST http://localhost:5159/auth/login [200 OK, 747B, 73ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil Veterinario

Sub-folder EP-001 · 200 — iniciar sessão (Auxiliar Veterinário)
  POST http://localhost:5159/auth/login [200 OK, 732B, 73ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil AuxiliarVeterinario

Sub-folder EP-001 · 200 — iniciar sessão (Cliente)
  POST http://localhost:5159/auth/login [200 OK, 731B, 88ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil Cliente
  Pass  O Cliente tem clienteId

Folder 01 · Autenticação (EP-001 a EP-003)
Sub-folder EP-001 · 400 — e-mail em falta
  POST http://localhost:5159/auth/login [400 Bad Request, 236B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-001 · 400 — palavra-passe em falta
  POST http://localhost:5159/auth/login [400 Bad Request, 243B, 4ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-001 · 401 — palavra-passe incorreta
  POST http://localhost:5159/auth/login [401 Unauthorized, 246B, 97ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401
  Pass  Não devolve token

Sub-folder EP-001 · 401 — e-mail inexistente (mesma resposta)
  POST http://localhost:5159/auth/login [401 Unauthorized, 246B, 80ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-001 · 403 — conta bloqueada (palavra-passe correta)
  POST http://localhost:5159/auth/login [403 Forbidden, 296B, 74ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-003 · 200 — utilizador autenticado (Administrador)
  GET http://localhost:5159/auth/me [200 OK, 273B, 92ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve o Administrador do token

Sub-folder EP-003 · 200 — utilizador autenticado (Cliente, com clienteId)
  GET http://localhost:5159/auth/me [200 OK, 267B, 20ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve o Cliente com clienteId

Sub-folder EP-003 · 401 — sem token
  GET http://localhost:5159/auth/me [401 Unauthorized, 269B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-003 · 401 — token adulterado
  GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 6ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-003 · 401 — token assinado com outra chave
  GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 5ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-002 · preparação — iniciar a sessão a terminar (EP-001 · 200)
  POST http://localhost:5159/auth/login [200 OK, 714B, 72ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil Administrador

Sub-folder EP-002 · 204 — terminar sessão
  POST http://localhost:5159/auth/logout [204 No Content, 81B, 53ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 204
  Pass  Sem corpo

Sub-folder EP-002 · 401 — o token terminado deixa de ser aceite (EP-003)
  GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-002 · 401 — terminar sessão outra vez com o token revogado
  POST http://localhost:5159/auth/logout [401 Unauthorized, 288B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-002 · 401 — sem token
  POST http://localhost:5159/auth/logout [401 Unauthorized, 269B, 3ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Folder 02 · Contas de funcionário (EP-004 a EP-008)
Sub-folder EP-004 · 200 — listar contas de funcionário
  GET http://localhost:5159/utilizadores [200 OK, 1.3kB, 114ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Envelope paginado (itens, pagina, tamanho, total)
  Pass  Nunca lista contas de Cliente
  Pass  Cada conta tem id, nome, e-mail, perfil e estado

Sub-folder EP-004 · 200 — filtrar por perfil e estado
  GET http://localhost:5159/utilizadores?perfil=Veterinario&estado=Ativo [200 OK, 512B, 31ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Só Veterinários ativos

Sub-folder EP-004 · 400 — perfil fora da enumeração
  GET http://localhost:5159/utilizadores?perfil=Gestor [400 Bad Request, 341B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-004 · 401 — sem token
  GET http://localhost:5159/utilizadores [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-004 · 403 — Rececionista
  GET http://localhost:5159/utilizadores [403 Forbidden, 311B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-005 · 200 — consultar conta de funcionário
  GET http://localhost:5159/utilizadores/1 [200 OK, 356B, 33ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve a conta do Administrador

Sub-folder EP-005 · 404 — conta de Cliente não é conta de funcionário
  GET http://localhost:5159/utilizadores/8 [404 Not Found, 258B, 14ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-005 · 404 — conta inexistente
  GET http://localhost:5159/utilizadores/999999 [404 Not Found, 258B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-005 · 401 — sem token
  GET http://localhost:5159/utilizadores/1 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-005 · 403 — Veterinário
  GET http://localhost:5159/utilizadores/1 [403 Forbidden, 311B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-006 · 201 — criar conta de funcionário (Rececionista)
  POST http://localhost:5159/utilizadores [201 Created, 356B, 185ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201
  Pass  Conta ativa com os dados enviados
  Pass  Location indica a conta criada

Sub-folder EP-006 · 400 — palavra-passe fraca
  POST http://localhost:5159/utilizadores [400 Bad Request, 348B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-006 · 409 — e-mail já registado
  POST http://localhost:5159/utilizadores [409 Conflict, 258B, 13ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 409

Sub-folder EP-006 · 422 — perfil Cliente
  POST http://localhost:5159/utilizadores [422 Unprocessable Entity, 367B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Sub-folder EP-006 · 422 — podeRegistarApoio para um Veterinário
  POST http://localhost:5159/utilizadores [422 Unprocessable Entity, 344B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Sub-folder EP-006 · 401 — sem token
  POST http://localhost:5159/utilizadores [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-006 · 403 — Rececionista
  POST http://localhost:5159/utilizadores [403 Forbidden, 311B, 6ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-006 · 200 — a conta criada inicia sessão (EP-001)
  POST http://localhost:5159/auth/login [200 OK, 741B, 79ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil Rececionista

Sub-folder EP-007 · 200 — atualizar e atribuir o perfil Auxiliar com apoio
  PUT http://localhost:5159/utilizadores/11 [200 OK, 380B, 37ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve a conta atualizada (formato de EP-005)

Sub-folder EP-007 · 401 — o token com o perfil antigo deixa de ser aceite (EP-003)
  GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 6ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-007 · 400 — dados inválidos
  PUT http://localhost:5159/utilizadores/11 [400 Bad Request, 344B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-007 · 400 — perfil Cliente
  PUT http://localhost:5159/utilizadores/11 [400 Bad Request, 334B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-007 · 404 — conta inexistente
  PUT http://localhost:5159/utilizadores/999999 [404 Not Found, 258B, 14ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-007 · 409 — e-mail usado por outra conta
  PUT http://localhost:5159/utilizadores/11 [409 Conflict, 262B, 23ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 409

Sub-folder EP-007 · 422 — o Administrador remove o próprio perfil
  PUT http://localhost:5159/utilizadores/1 [422 Unprocessable Entity, 322B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Sub-folder EP-007 · 422 — podeRegistarApoio para um perfil diferente de Auxiliar
  PUT http://localhost:5159/utilizadores/11 [422 Unprocessable Entity, 344B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Sub-folder EP-007 · 401 — sem token
  PUT http://localhost:5159/utilizadores/11 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-007 · 403 — Rececionista
  PUT http://localhost:5159/utilizadores/11 [403 Forbidden, 311B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-008 · preparação — sessão da conta com o perfil atual (EP-001 · 200)
  POST http://localhost:5159/auth/login [200 OK, 771B, 68ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil AuxiliarVeterinario

Sub-folder EP-008 · 400 — estado diferente de Bloqueado
  PATCH http://localhost:5159/utilizadores/11/estado [400 Bad Request, 296B, 17ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-008 · 404 — conta inexistente
  PATCH http://localhost:5159/utilizadores/999999/estado [404 Not Found, 258B, 20ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-008 · 422 — o Administrador bloqueia a própria conta
  PATCH http://localhost:5159/utilizadores/1/estado [422 Unprocessable Entity, 305B, 13ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Sub-folder EP-008 · 422 — conta de Cliente
  PATCH http://localhost:5159/utilizadores/8/estado [422 Unprocessable Entity, 350B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Sub-folder EP-008 · 401 — sem token
  PATCH http://localhost:5159/utilizadores/11/estado [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-008 · 403 — Rececionista
  PATCH http://localhost:5159/utilizadores/11/estado [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-008 · 204 — bloquear a conta
  PATCH http://localhost:5159/utilizadores/11/estado [204 No Content, 81B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 204
  Pass  Sem corpo

Sub-folder EP-008 · 401 — o token emitido antes do bloqueio deixa de ser aceite (EP-003)
  GET http://localhost:5159/auth/me [401 Unauthorized, 288B, 5ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-008 · 403 — a conta bloqueada já não inicia sessão (EP-001)
  POST http://localhost:5159/auth/login [403 Forbidden, 296B, 66ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-008 · 422 — conta já bloqueada
  PATCH http://localhost:5159/utilizadores/11/estado [422 Unprocessable Entity, 281B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Folder 03 · Clínica (EP-009, EP-010)
Sub-folder EP-009 · 200 — consultar dados, configurações e horário
  GET http://localhost:5159/clinica [200 OK, 993B, 48ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Tem os dados gerais e a duração das consultas
  Pass  Cada período tem dia, abertura e fecho

Sub-folder EP-009 · 401 — sem token
  GET http://localhost:5159/clinica [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-009 · 403 — Rececionista
  GET http://localhost:5159/clinica [403 Forbidden, 311B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-010 · 200 — atualizar a duração e o horário (vários períodos, fora de ordem)
  PUT http://localhost:5159/clinica [200 OK, 590B, 130ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Guarda a nova duração
  Pass  Devolve o horário ordenado por dia e hora

Sub-folder EP-010 · 400 — períodos sobrepostos no mesmo dia
  PUT http://localhost:5159/clinica [400 Bad Request, 280B, 18ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-010 · 400 — hora de fecho igual à de abertura
  PUT http://localhost:5159/clinica [400 Bad Request, 270B, 14ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-010 · 400 — campos inválidos
  PUT http://localhost:5159/clinica [400 Bad Request, 608B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-010 · 401 — sem token
  PUT http://localhost:5159/clinica [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-010 · 403 — Rececionista
  PUT http://localhost:5159/clinica [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-010 · 200 — repor os dados originais
  PUT http://localhost:5159/clinica [200 OK, 993B, 31ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  A clínica volta ao estado inicial

Folder 04 · Clientes (EP-011 a EP-014)
Sub-folder EP-011 · 200 — pesquisar clientes
  GET http://localhost:5159/clientes?pesquisa=silva [200 OK, 340B, 54ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Envelope paginado (itens, pagina, tamanho, total)
  Pass  Encontra clientes e indica o número de animais

Sub-folder EP-011 · 400 — página inválida
  GET http://localhost:5159/clientes?pagina=0 [400 Bad Request, 252B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-011 · 401 — sem token
  GET http://localhost:5159/clientes [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-011 · 403 — Auxiliar Veterinário
  GET http://localhost:5159/clientes [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-011 · 403 — Cliente
  GET http://localhost:5159/clientes [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-012 · 200 — consultar cliente com os animais
  GET http://localhost:5159/clientes/1 [200 OK, 448B, 41ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve o Cliente do seed
  Pass  Lista os animais do Cliente

Sub-folder EP-012 · 404 — cliente inexistente
  GET http://localhost:5159/clientes/999999 [404 Not Found, 240B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-012 · 401 — sem token
  GET http://localhost:5159/clientes/1 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-012 · 403 — Auxiliar Veterinário
  GET http://localhost:5159/clientes/1 [403 Forbidden, 311B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-013 · 201 — criar conta de Cliente
  POST http://localhost:5159/clientes [201 Created, 395B, 114ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201
  Pass  Cliente criado com a conta ativa
  Pass  Location indica a ficha

Sub-folder EP-013 · 200 — o Cliente criado inicia sessão (EP-001)
  POST http://localhost:5159/auth/login [200 OK, 750B, 73ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve um token JWT e a data de expiração
  Pass  O utilizador tem o perfil Cliente
  Pass  O token é do Cliente criado

Sub-folder EP-013 · 400 — campos em falta, NIF e e-mail inválidos
  POST http://localhost:5159/clientes [400 Bad Request, 418B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-013 · 409 — NIF já registado
  POST http://localhost:5159/clientes [409 Conflict, 256B, 28ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 409

Sub-folder EP-013 · 409 — e-mail já registado (conta de funcionário)
  POST http://localhost:5159/clientes [409 Conflict, 258B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 409

Sub-folder EP-013 · 401 — sem token
  POST http://localhost:5159/clientes [401 Unauthorized, 269B, 3ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-013 · 403 — Veterinário
  POST http://localhost:5159/clientes [403 Forbidden, 311B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-014 · 200 — atualizar cliente (e o e-mail da conta)
  PUT http://localhost:5159/clientes/4 [200 OK, 364B, 50ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve o cliente atualizado

Sub-folder EP-014 · 400 — campos inválidos
  PUT http://localhost:5159/clientes/4 [400 Bad Request, 366B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-014 · 404 — cliente inexistente
  PUT http://localhost:5159/clientes/999999 [404 Not Found, 240B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-014 · 409 — NIF de outro cliente
  PUT http://localhost:5159/clientes/4 [409 Conflict, 256B, 25ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 409

Sub-folder EP-014 · 401 — sem token
  PUT http://localhost:5159/clientes/4 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-014 · 403 — Veterinário
  PUT http://localhost:5159/clientes/4 [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Folder 05 · Pacientes (EP-015 a EP-018)
Sub-folder EP-015 · 200 — pacientes de um Cliente
  GET http://localhost:5159/animais?clienteId=1 [200 OK, 490B, 51ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Envelope paginado (itens, pagina, tamanho, total)
  Pass  Só animais do Cliente indicado
  Pass  Inclui o Bobi e a Mia

Sub-folder EP-015 · 200 — pesquisar por nome do animal
  GET http://localhost:5159/animais?pesquisa=Kiko [200 OK, 345B, 35ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Encontra o Kiko, de outro Cliente

Sub-folder EP-015 · 400 — paginação inválida
  GET http://localhost:5159/animais?pagina=0&tamanho=101 [400 Bad Request, 299B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-015 · 401 — sem token
  GET http://localhost:5159/animais [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-015 · 403 — Cliente
  GET http://localhost:5159/animais [403 Forbidden, 311B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-016 · 200 — ficha administrativa (Auxiliar Veterinário)
  GET http://localhost:5159/animais/1 [200 OK, 387B, 42ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve a ficha com o proprietário
  Pass  Sem dados clínicos

Sub-folder EP-016 · 404 — animal inexistente
  GET http://localhost:5159/animais/999999 [404 Not Found, 239B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-016 · 401 — sem token
  GET http://localhost:5159/animais/1 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-016 · 403 — Cliente
  GET http://localhost:5159/animais/1 [403 Forbidden, 311B, 7ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-017 · 201 — registar paciente
  POST http://localhost:5159/animais [201 Created, 433B, 108ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201
  Pass  Ficha criada para o Cliente indicado
  Pass  Location indica a ficha

Sub-folder EP-017 · 400 — peso igual a zero
  POST http://localhost:5159/animais [400 Bad Request, 240B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-017 · 400 — data de nascimento futura
  POST http://localhost:5159/animais [400 Bad Request, 246B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-017 · 400 — clienteId em falta
  POST http://localhost:5159/animais [400 Bad Request, 245B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-017 · 404 — cliente inexistente
  POST http://localhost:5159/animais [404 Not Found, 240B, 13ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-017 · 401 — sem token
  POST http://localhost:5159/animais [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-017 · 403 — Auxiliar Veterinário
  POST http://localhost:5159/animais [403 Forbidden, 311B, 7ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-017 · 403 — Cliente
  POST http://localhost:5159/animais [403 Forbidden, 311B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-018 · 200 — atualizar dados administrativos
  PUT http://localhost:5159/animais/5 [200 OK, 413B, 40ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve a ficha atualizada

Sub-folder EP-018 · 400 — peso inválido
  PUT http://localhost:5159/animais/5 [400 Bad Request, 240B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-018 · 400 — data de nascimento futura
  PUT http://localhost:5159/animais/5 [400 Bad Request, 246B, 14ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-018 · 404 — animal inexistente
  PUT http://localhost:5159/animais/999999 [404 Not Found, 239B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-018 · 404 — novo cliente inexistente
  PUT http://localhost:5159/animais/5 [404 Not Found, 240B, 13ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-018 · 401 — sem token
  PUT http://localhost:5159/animais/5 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-018 · 403 — Cliente
  PUT http://localhost:5159/animais/5 [403 Forbidden, 311B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Folder 06 · Animais do Cliente — aplicação móvel (EP-019, EP-020)
Sub-folder EP-019 · 200 — listar os meus animais
  GET http://localhost:5159/me/animais [200 OK, 332B, 29ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve uma lista com os animais do Cliente
  Pass  Não inclui animais de outro Cliente

Sub-folder EP-019 · 200 — o Cliente criado vê só o animal registado em EP-017
  GET http://localhost:5159/me/animais [200 OK, 260B, 14ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Um único animal: o criado nesta execução

Sub-folder EP-019 · 200 — o clienteId no pedido é ignorado
  GET http://localhost:5159/me/animais?clienteId=4 [200 OK, 332B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Continua a devolver os animais do Cliente do token

Sub-folder EP-019 · 401 — sem token
  GET http://localhost:5159/me/animais [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-019 · 403 — perfil interno (Rececionista)
  GET http://localhost:5159/me/animais [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-020 · 200 — consultar o meu animal
  GET http://localhost:5159/me/animais/1 [200 OK, 287B, 25ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Devolve os dados básicos do animal
  Pass  Sem dados do proprietário nem clínicos

Sub-folder EP-020 · 403 — animal de outro Cliente
  GET http://localhost:5159/me/animais/2 [403 Forbidden, 302B, 16ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403
  Pass  Não revela dados do animal

Sub-folder EP-020 · 404 — animal inexistente
  GET http://localhost:5159/me/animais/999999 [404 Not Found, 239B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-020 · 401 — sem token
  GET http://localhost:5159/me/animais/1 [401 Unauthorized, 269B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-020 · 403 — perfil interno (Veterinário)
  GET http://localhost:5159/me/animais/1 [403 Forbidden, 311B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Folder 08 · Registo clínico (EP-031 a EP-034)
Sub-folder EP-023 · preparação — identificar o Veterinário da sessão
  GET http://localhost:5159/consultas?dataInicio=2026-10-01&dataFim=2026-12-31&tamanho=100 [200 OK, 1.2kB, 96ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  A agenda do Veterinário tem marcações (dados da I061)
  Pass  Só aparecem marcações do próprio Veterinário

Sub-folder EP-023 · preparação — marcação de outro Veterinário
  GET http://localhost:5159/consultas?dataInicio=2026-11-04&dataFim=2026-11-04&tamanho=100 [200 OK, 516B, 33ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Existe uma marcação de outro Veterinário (M4 da I061)

Sub-folder EP-022 · preparação — próximos horários livres do Veterinário
  GET http://localhost:5159/veterinarios/4/disponibilidade?dataInicio=2026-11-02&dataFim=2026-12-02 [200 OK, 5.03kB, 60ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Há pelo menos dois horários livres nos próximos 31 dias

Sub-folder EP-025 · preparação — marcação para o registo clínico
  POST http://localhost:5159/consultas [201 Created, 584B, 118ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201
  Pass  Marcação Pendente do Veterinário da sessão

Sub-folder EP-027 · preparação — confirmar a marcação
  PATCH http://localhost:5159/consultas/6/estado [204 No Content, 103B, 33ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 204

Sub-folder EP-025 · preparação — marcação que fica Pendente
  POST http://localhost:5159/consultas [201 Created, 571B, 42ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201

Sub-folder EP-031 · 201 — criar registo clínico com tratamento e prescrição
  POST http://localhost:5159/registos-clinicos [201 Created, 675B, 138ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201
  Pass  Registo associado à consulta e ao paciente
  Pass  Um tratamento e uma prescrição, com identificador
  Pass  Location indica o registo

Sub-folder EP-031 · 400 — nenhum elemento clínico
  POST http://localhost:5159/registos-clinicos [400 Bad Request, 339B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-031 · 400 — prescrição incompleta
  POST http://localhost:5159/registos-clinicos [400 Bad Request, 300B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-031 · 404 — consulta inexistente
  POST http://localhost:5159/registos-clinicos [404 Not Found, 241B, 29ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-031 · 403 — consulta de outro Veterinário
  POST http://localhost:5159/registos-clinicos [403 Forbidden, 320B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-031 · 422 — marcação Pendente
  POST http://localhost:5159/registos-clinicos [422 Unprocessable Entity, 342B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 422

Sub-folder EP-031 · 409 — a consulta já tem registo clínico
  POST http://localhost:5159/registos-clinicos [409 Conflict, 300B, 14ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 409

Sub-folder EP-031 · 401 — sem token
  POST http://localhost:5159/registos-clinicos [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-031 · 403 — Auxiliar Veterinário (só leitura)
  POST http://localhost:5159/registos-clinicos [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-032 · 200 — atualizar notas, diagnóstico, tratamentos e prescrições
  PUT http://localhost:5159/registos-clinicos/2 [200 OK, 764B, 75ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Notas e diagnóstico atualizados; consulta e paciente mantêm-se
  Pass  O tratamento indicado mantém-se e o novo é acrescentado
  Pass  A prescrição indicada é alterada
  Pass  Regista atualizadoEm e atualizadoPor

Sub-folder EP-032 · 400 — o registo ficaria sem elemento clínico
  PUT http://localhost:5159/registos-clinicos/2 [400 Bad Request, 339B, 7ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-032 · 400 — tratamento que não é deste registo
  PUT http://localhost:5159/registos-clinicos/2 [400 Bad Request, 273B, 16ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-032 · 404 — registo inexistente
  PUT http://localhost:5159/registos-clinicos/999999 [404 Not Found, 253B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-032 · 401 — sem token
  PUT http://localhost:5159/registos-clinicos/2 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-032 · 403 — Auxiliar Veterinário (só leitura)
  PUT http://localhost:5159/registos-clinicos/2 [403 Forbidden, 311B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-033 · 200 — consultar o registo (Veterinário)
  GET http://localhost:5159/registos-clinicos/2 [200 OK, 764B, 28ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Registo completo, já com a atualização

Sub-folder EP-033 · 200 — consultar o registo (Auxiliar Veterinário, só leitura)
  GET http://localhost:5159/registos-clinicos/2 [200 OK, 764B, 26ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Registo completo, já com a atualização

Sub-folder EP-033 · 404 — registo inexistente
  GET http://localhost:5159/registos-clinicos/999999 [404 Not Found, 253B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-033 · 401 — sem token
  GET http://localhost:5159/registos-clinicos/2 [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-033 · 403 — Rececionista
  GET http://localhost:5159/registos-clinicos/2 [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-033 · 403 — Cliente
  GET http://localhost:5159/registos-clinicos/2 [403 Forbidden, 311B, 7ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-034 · 200 — histórico clínico do paciente
  GET http://localhost:5159/animais/1/historico-clinico [200 OK, 569B, 59ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Histórico do animal pedido, com o registo criado
  Pass  Do registo mais recente para o mais antigo
  Pass  O resumo conta os tratamentos e as prescrições

Sub-folder EP-034 · 200 — histórico filtrado por datas
  GET http://localhost:5159/animais/1/historico-clinico?dataInicio=2026-11-02&dataFim=2026-11-02 [200 OK, 396B, 30ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Só registos do dia pedido, incluindo o registo criado

Sub-folder EP-034 · 200 — animal sem registos (Auxiliar Veterinário)
  GET http://localhost:5159/animais/5/historico-clinico [200 OK, 228B, 22ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Lista de registos vazia

Sub-folder EP-034 · 400 — dataFim anterior a dataInicio
  GET http://localhost:5159/animais/1/historico-clinico?dataInicio=2026-10-15&dataFim=2026-10-14 [400 Bad Request, 290B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-034 · 404 — animal inexistente
  GET http://localhost:5159/animais/999999/historico-clinico [404 Not Found, 239B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-034 · 401 — sem token
  GET http://localhost:5159/animais/1/historico-clinico [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-034 · 403 — Rececionista
  GET http://localhost:5159/animais/1/historico-clinico [403 Forbidden, 311B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-027 · preparação — cancelar a marcação Pendente (limpeza)
  PATCH http://localhost:5159/consultas/7/estado [204 No Content, 103B, 15ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 204

Folder 09 · Boletim de vacinas e desparasitações (EP-042 a EP-044)
Sub-folder EP-043 · 201 — registar vacinação com intervalo
  POST http://localhost:5159/animais/1/vacinacoes [201 Created, 415B, 68ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201
  Pass  Vacinação registada com os dados enviados
  Pass  Próxima administração = data + intervalo (2026-10-14 + 365 dias)
  Pass  Location indica a administração do animal

Sub-folder EP-043 · 201 — registar desparasitação sem intervalo
  POST http://localhost:5159/animais/1/vacinacoes [201 Created, 421B, 22ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 201
  Pass  Sem intervalo, não há próxima administração

Sub-folder EP-043 · 400 — data de administração futura
  POST http://localhost:5159/animais/1/vacinacoes [400 Bad Request, 259B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-043 · 400 — intervaloDias igual a zero
  POST http://localhost:5159/animais/1/vacinacoes [400 Bad Request, 284B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-043 · 400 — tipo inválido e lote em falta
  POST http://localhost:5159/animais/1/vacinacoes [400 Bad Request, 276B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-043 · 404 — animal inexistente
  POST http://localhost:5159/animais/999999/vacinacoes [404 Not Found, 239B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-043 · 401 — sem token
  POST http://localhost:5159/animais/1/vacinacoes [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-043 · 403 — Auxiliar Veterinário
  POST http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 6ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-043 · 403 — Cliente
  POST http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 6ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-042 · 200 — boletim completo do paciente
  GET http://localhost:5159/animais/1/vacinacoes [200 OK, 1.68kB, 35ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Boletim do animal pedido, com as duas administrações registadas
  Pass  Cada administração tem lote, intervalo, próxima e Veterinário
  Pass  Administrações da mais recente para a mais antiga
  Pass  Próximas da mais próxima para a mais distante

Sub-folder EP-042 · 200 — filtrar por tipo (Desparasitacao)
  GET http://localhost:5159/animais/1/vacinacoes?tipo=Desparasitacao [200 OK, 811B, 22ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Só desparasitações, nas administrações e nas próximas

Sub-folder EP-042 · 400 — tipo inválido
  GET http://localhost:5159/animais/1/vacinacoes?tipo=Antibiotico [400 Bad Request, 252B, 7ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder EP-042 · 404 — animal inexistente
  GET http://localhost:5159/animais/999999/vacinacoes [404 Not Found, 239B, 12ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-042 · 401 — sem token
  GET http://localhost:5159/animais/1/vacinacoes [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-042 · 403 — Auxiliar Veterinário
  GET http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 5ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-042 · 403 — Cliente
  GET http://localhost:5159/animais/1/vacinacoes [403 Forbidden, 311B, 7ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder RF056 · 200 — atualizar a data e o intervalo e recalcular a próxima
  PUT http://localhost:5159/animais/1/vacinacoes/5 [200 OK, 374B, 37ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Próxima recalculada (2026-10-01 + 90 dias)

Sub-folder RF056 · 200 — acrescentar um intervalo a uma desparasitação
  PUT http://localhost:5159/animais/1/vacinacoes/6 [200 OK, 387B, 23ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Passa a ter próxima administração (2026-10-14 + 30 dias)

Sub-folder RF056 · 400 — data de administração futura
  PUT http://localhost:5159/animais/1/vacinacoes/5 [400 Bad Request, 259B, 9ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 400

Sub-folder RF056 · 404 — administração inexistente
  PUT http://localhost:5159/animais/1/vacinacoes/999999 [404 Not Found, 293B, 13ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder RF056 · 404 — administração de outro animal
  PUT http://localhost:5159/animais/5/vacinacoes/5 [404 Not Found, 293B, 14ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder RF056 · 401 — sem token
  PUT http://localhost:5159/animais/1/vacinacoes/5 [401 Unauthorized, 269B, 1ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder RF056 · 403 — Auxiliar Veterinário
  PUT http://localhost:5159/animais/1/vacinacoes/5 [403 Forbidden, 311B, 8ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

Sub-folder EP-044 · 200 — boletim do meu animal
  GET http://localhost:5159/me/animais/1/vacinacoes [200 OK, 1.07kB, 36ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Histórico com o registo atualizado e sem lote nem Veterinário
  Pass  Próximas da mais próxima para a mais distante

Sub-folder EP-044 · 200 — outro Cliente vê o boletim do seu animal (vazio)
  GET http://localhost:5159/me/animais/5/vacinacoes [200 OK, 178B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Devolve 200
  Pass  Sem registos, as duas listas vêm vazias

Sub-folder EP-044 · 403 — animal de outro Cliente
  GET http://localhost:5159/me/animais/5/vacinacoes [403 Forbidden, 302B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403
  Pass  A resposta não tem dados do animal

Sub-folder EP-044 · 403 — o outro Cliente pede um animal que não é seu
  GET http://localhost:5159/me/animais/1/vacinacoes [403 Forbidden, 302B, 10ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403
  Pass  A resposta não tem dados do animal

Sub-folder EP-044 · 404 — animal inexistente
  GET http://localhost:5159/me/animais/999999/vacinacoes [404 Not Found, 239B, 11ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 404

Sub-folder EP-044 · 401 — sem token
  GET http://localhost:5159/me/animais/1/vacinacoes [401 Unauthorized, 269B, 2ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 401

Sub-folder EP-044 · 403 — perfil interno (Veterinário)
  GET http://localhost:5159/me/animais/1/vacinacoes [403 Forbidden, 311B, 6ms]
  Pass  Não expõe palavras-passe nem hashes
  Pass  Erro no formato único da API (code, message, detail)
  Pass  Devolve 403

------------------------------------------------------------------
|                         |          executed |           failed |
--------------------------+-------------------+-------------------
|              iterations |                 1 |                0 |
--------------------------+-------------------+-------------------
|                requests |               190 |                0 |
--------------------------+-------------------+-------------------
|            test-scripts |               380 |                0 |
--------------------------+-------------------+-------------------
|      prerequest-scripts |                 7 |                0 |
--------------------------+-------------------+-------------------
|              assertions |               611 |                0 |
------------------------------------------------------------------
| total run duration: 21.2s                                      |
------------------------------------------------------------------
| total data received: 45.11kB (approx)                          |
------------------------------------------------------------------
| average response time: 27ms [min: 1ms, max: 549ms, s.d.: 49ms] |
------------------------------------------------------------------
``````

# Diagrama de Sequência — Autenticar Utilizador

**Projeto:** Gestão de Clínica Veterinária  
**Grupo:** G08  
**Milestone:** m2  
**Versão:** v01  
**Issue:** I023 — Diagrama de sequência — autenticar utilizador  
**Ficheiro:** `m2-diagrama-sequencia-autenticar-utilizador-v01.md`

---

## 1. Objetivo

Este documento apresenta o diagrama de sequência do caso de uso **UC01 — Autenticar utilizador**.
O diagrama representa a interação prevista entre o utilizador, o frontend do canal utilizado, a API, os serviços de autenticação e a persistência durante a validação das credenciais e a emissão do token JWT.

O fluxo segue a arquitetura definida para o projeto: Frontend Web ou Mobile → ARC-API → ARC-F001/ARC-SEC → ARC-DATA, utilizando ASP.NET Core 8, Entity Framework Core e JWT.

## 2. Diagrama de Sequência

O diagrama inclui o fluxo de sucesso e as alternativas relevantes previstas nos requisitos e na Documentação da API.

```plantuml {kroki=true}
@startuml
skinparam maxMessageSize 150
autonumber

actor "Utilizador\n(Web/Mobile)" as User
participant "ARC-WEB / ARC-MOB\n(Frontend)" as UI
participant "AuthController\n(API Layer)" as Controller
participant "AuthService\n(ARC-F001 / Application)" as AuthSvc
participant "JwtTokenService\n(ARC-SEC)" as JwtSvc
participant "ClinicaDbContext\n(ARC-DATA)" as DB

User -> UI : Introduz email e palavra-passe
activate UI
UI -> Controller : POST /auth/login { email, password }
activate Controller
Controller -> AuthSvc : AutenticarAsync(email, password)
activate AuthSvc

AuthSvc -> DB : Procurar Utilizador por email
activate DB
DB --> AuthSvc : Utilizador ou null
deactivate DB

alt Utilizador inexistente
    AuthSvc --> Controller : Falha de autenticação
    Controller --> UI : 401 Unauthorized
    UI --> User : Credenciais incorretas
else Utilizador existente
    AuthSvc -> AuthSvc : Verificar password com PasswordHash

    alt Password inválida
        AuthSvc --> Controller : Falha de autenticação
        Controller --> UI : 401 Unauthorized
        UI --> User : Credenciais incorretas
    else Conta bloqueada
        AuthSvc --> Controller : Acesso recusado
        Controller --> UI : 403 Forbidden
        UI --> User : Conta bloqueada
    else Credenciais válidas e conta ativa
        AuthSvc -> JwtSvc : Gerar token (Utilizador)
        activate JwtSvc
        JwtSvc --> AuthSvc : JWT + data de expiração
        deactivate JwtSvc

        AuthSvc --> Controller : AuthResult(token, expiraEm, utilizador)
        Controller --> UI : 200 OK (AuthResult)

        UI -> UI : Verificar se o perfil pertence ao canal atual
        alt Perfil permitido no canal
            UI -> UI : Guardar sessão\nWeb: sessionStorage\nMobile: secure storage
            UI --> User : Abrir área autorizada do respetivo perfil
        else Perfil não permitido no canal
            UI -> UI : Definir token apenas para revogação
            UI -> Controller : POST /auth/logout\nAuthorization: Bearer <token>
            Controller -> AuthSvc : TerminarSessaoAsync(jti)
            AuthSvc -> DB : Registar TokenRevogado até à expiração
            DB --> AuthSvc : Confirma gravação
            AuthSvc --> Controller : Concluído
            Controller --> UI : 204 No Content
            UI -> UI : Limpar token / não criar sessão
            UI --> User : Acesso recusado neste canal
        end
    end
end

deactivate AuthSvc
deactivate Controller
deactivate UI
@enduml
```

## 3. Regras representadas

- `401 Unauthorized` é utilizado quando o utilizador não existe ou a palavra-passe está incorreta (RF002).
- `403 Forbidden` é utilizado quando a conta está bloqueada (RF002, RF009).
- O JWT é emitido apenas depois da validação das credenciais e do estado da conta; a validade prevista é de 8 horas e não existe *refresh token* (RF004; DA-ARQ-06).
- O Frontend Web aceita apenas Administrador, Rececionista, Veterinário e Auxiliar Veterinário; o Frontend Mobile aceita apenas Cliente (RNF003).
- Se EP-001 devolver um perfil que não pertence ao canal atual, o token é revogado por EP-002 e não é mantida uma sessão nesse frontend.
- O armazenamento local do token depende do canal: `sessionStorage` no Frontend Web e armazenamento seguro no Frontend Mobile, conforme os respetivos modelos de classes.

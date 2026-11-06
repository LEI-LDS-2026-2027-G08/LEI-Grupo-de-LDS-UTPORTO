# Credenciais de Avaliação — M3

| Campo | Valor |
|---|---|
| Grupo | G08 — Gestão de Clínica Veterinária — LDS 2026/2027 |
| Ficheiro | `m3-credenciais-avaliacao-v01.md` |
| Versão | v01 (2026-11-05) |
| Issue | I105 — Preparar o README cumulativo e as credenciais de avaliação da M3 |
| Executor | Gonçalo Magalhães Ribeiro |
| Referência | Regulamento de Funcionamento da UC, secção 18.4 (credenciais de avaliação) |

> **Ficheiro só para o ZIP da submissão.** Não é versionado no repositório Git do grupo nem enviado por outros canais.

## 1. Acesso

| Elemento | Valor |
|---|---|
| Ambiente público de avaliação | https://clinica-g08-api-ehf2bfe5fweuaner.spaincentral-01.azurewebsites.net/swagger |
| Iniciar sessão | No Swagger: `POST /auth/login` → *Try it out* → corpo `{ "email": "…", "password": "…" }` → *Execute*. Copiar o `token` da resposta, carregar em **Authorize** e escrever `Bearer <token>`. O token é válido durante 8 horas. |
| Primeiro acesso | O ambiente usa um plano gratuito: a aplicação e a base de dados adormecem sem utilização, e o primeiro pedido pode demorar 1 a 2 minutos. Se der erro de tempo esgotado, repetir. |
| Instalação local | As mesmas contas existem numa instalação local depois de carregar os dados de demonstração (Manual de Instalação e Configuração, passo 3.2). |

## 2. Contas temporárias

Todas as contas são **fictícias e temporárias**, criadas só para a avaliação do projeto (dados de demonstração da I061: nomes com «(demonstração)», e-mails do domínio reservado `.test`, NIF e telefones fictícios). Nenhuma é uma conta pessoal de um elemento do grupo, nenhuma palavra-passe é usada noutro sistema e não dão acesso a dados reais.

| E-mail | Palavra-passe | Perfil | Estado | Para avaliar |
|---|---|---|---|---|
| `admin@clinica-g08.test` | `Demo-z8AJrx6ETK#26` | Administrador | Ativo | Gestão de contas de funcionários, perfis e bloqueio (EP-005 a EP-008), dados da clínica e horários (EP-009, EP-010), Clientes. É a única conta com permissões de administração do sistema, necessárias para avaliar a F001. |
| `rececao@clinica-g08.test` | `Demo-YnYJvXuj6q#26` | Rececionista | Ativo | Clientes, pacientes, agenda e marcação de Consultas, chegada (F001 a F003). |
| `vet.ana@clinica-g08.test` | `Demo-PQ2sbPUIoo#26` | Veterinário | Ativo | Agenda própria, registo clínico, tratamentos e prescrições, vacinação e desparasitação (F003, F004, F006). |
| `vet.bruno@clinica-g08.test` | `Demo-9MCt6U5zNE#26` | Veterinário | Ativo | Segundo Veterinário, para verificar a disponibilidade e a separação das agendas. |
| `aux.carla@clinica-g08.test` | `Demo-jRBIiqdZJA#26` | Auxiliar Veterinário (com apoio) | Ativo | Preparação da Consulta e registo de apoio não clínico (EP-029); leitura da informação clínica autorizada. |
| `aux.diogo@clinica-g08.test` | `Demo-uieFJHWIen#26` | Auxiliar Veterinário (sem apoio) | Ativo | Mesmo perfil sem autorização para registar apoio: EP-029 devolve 403. |
| `cliente.joao@clinica-g08.test` | `Demo-4dP06SlCni#26` | Cliente | Ativo | Área do Cliente: os seus animais (Bobi, Mia), Consultas futuras e histórico, boletim (RF018, RF028, RF029 e boletim). |
| `cliente.maria@clinica-g08.test` | `Demo-Xc5FB7IPwz#26` | Cliente | Ativo | Segundo Cliente (animal Kiko), para verificar que um Cliente não acede aos animais de outro Cliente. |
| `cliente.rui@clinica-g08.test` | `Demo-uZlN9Pvksl#26` | Cliente | Ativo | Cliente com dados mínimos (sem NIF nem morada; animal Luna sem data de nascimento nem peso). |
| `rececao.bloqueada@clinica-g08.test` | `Demo-hF2a6QZkJ0#26` | Rececionista | Bloqueado | Conta bloqueada de propósito: o login devolve 403 «A conta está bloqueada». |

## 3. Condições

- **Âmbito:** as contas só existem na base de dados do ambiente público de avaliação (Azure SQL `clinica-g08-avaliacao`) e nas instalações locais com os dados de demonstração; não dão acesso ao portal do Azure, ao GitHub nem a qualquer outro serviço.
- **Alterações:** a avaliação pode criar, alterar e bloquear dados livremente. O grupo pode repor a base pública no estado inicial (README da pipeline `04.16-ci-cd/m3-v03`, secção 6); a reposição repõe também estas palavras-passe.
- **Conta Administrador:** é a única com permissões de administração da aplicação; é necessária para avaliar a gestão de contas e da clínica (F001). Não tem qualquer permissão fora da aplicação.
- **Validade e revogação:** as contas são válidas até ao fim da avaliação da M3 (incluindo a defesa). Depois, o grupo bloqueia-as (EP-008) ou apaga o ambiente público, e roda os segredos do ambiente (README da pipeline `m3-v03`, secção 5).
- **Contacto:** em caso de problema de acesso, contactar o grupo pelo canal da UC; o grupo pode repor a base pública sem alterar as credenciais deste ficheiro.

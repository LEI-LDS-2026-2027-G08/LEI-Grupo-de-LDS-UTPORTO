# Testes de sistema do frontend web

Automação com Playwright dos cenários da Especificação dos Testes de Sistema do Frontend Web (I128), na parte atribuída à I129: autenticação, funcionários, Clientes, dados e horário da clínica e pacientes (30 cenários).

## Antes de executar

1. **Backend** a correr em `http://localhost:5159`, com a base reposta e os dados de demonstração (I061) carregados. Os cenários contam com esses dados (por exemplo, os animais do João Costa e o horário da clínica).
2. **Palavras-passe** das contas de demonstração em variáveis de ambiente (nunca no repositório):

   ```powershell
   $env:E2E_SENHA_ADMIN = "..."
   $env:E2E_SENHA_RECECAO = "..."
   $env:E2E_SENHA_RECECAO_BLOQUEADA = "..."
   $env:E2E_SENHA_VET_ANA = "..."
   $env:E2E_SENHA_AUX_CARLA = "..."
   ```

3. **Chromium** do Playwright, na primeira vez: `npx playwright install chromium`.

Opcional: `E2E_WEB_URL` (por omissão `http://localhost:5173`) e `E2E_API_URL` (por omissão `http://localhost:5159`). Se o frontend não estiver a correr, o `npm run test:sistema` arranca o `npm run dev` uma só vez para as duas fases (com `VITE_API_URL` igual a `E2E_API_URL`) e para-o no fim.

## Executar

```powershell
npm run test:sistema
```

Corre em duas fases: primeiro os cenários que podem correr em paralelo (projeto `sistema`) e depois, sozinhos, os que alteram a configuração ou o horário da clínica (projeto `sistema-exclusivo`, ficheiros `*.exclusivo.spec.ts`), que repõem sempre os valores originais (I128, secção 5.5). Um só cenário: `npm run test:sistema -- --grep F001-07`.

Quando um teste falha, a captura de ecrã e o _trace_ ficam em `test-results/` e o relatório em `playwright-report/sistema` e `playwright-report/sistema-exclusivo` (`npx playwright show-report playwright-report/sistema`).

## Regras seguidas (I128, secção 5)

- A API nunca é simulada; o navegador corre sem interface.
- Cada teste inicia a sua sessão e cria os dados de que precisa, com o `{sufixo}` do cenário e da execução; nenhum depende de outro nem da ordem.
- Não há pausas fixas: cada passo espera por uma condição visível ou pela resposta da API.
- Os dados criados ficam até à reposição da base, feita antes de cada execução completa.

## Estrutura

| Pasta / ficheiro             | Conteúdo                                                             |
| ---------------------------- | -------------------------------------------------------------------- |
| `apoio/contas.ts`            | Contas de demonstração e leitura das palavras-passe                  |
| `apoio/sessao.ts`            | PC-01 e PC-02 e o ajudante `entrarComo` por perfil                   |
| `apoio/procedimentos.ts`     | PC-03 (Cliente de teste) e PC-04 (paciente de teste)                 |
| `apoio/dados.ts`             | `{sufixo}`, e-mails, telefones, NIF e palavras-passe gerados         |
| `apoio/pedidos.ts`           | Registo dos pedidos à API (para confirmar validações sem pedido)     |
| `apoio/verificarAmbiente.ts` | Verificação inicial das variáveis e do backend                       |
| `paginas/`                   | Um objeto de página por ecrã (W01 a W08) e o seletor de proprietário |
| `executar.mjs`               | Execução em duas fases                                               |

## Cenário ↔ teste

| Cenário (I128) | Título                                                                         | Ficheiro                    |
| -------------- | ------------------------------------------------------------------------------ | --------------------------- |
| ST-WEB-F001-01 | Iniciar sessão com cada perfil interno                                         | `f001-sessao.spec.ts`       |
| ST-WEB-F001-02 | Recusar credenciais inválidas                                                  | `f001-sessao.spec.ts`       |
| ST-WEB-F001-03 | Recusar o início de sessão de uma conta bloqueada                              | `f001-sessao.spec.ts`       |
| ST-WEB-F001-06 | Listar e pesquisar funcionários                                                | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-07 | Criar uma conta de funcionário e atribuir-lhe um perfil                        | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-08 | Recusar a criação de funcionário com dados inválidos                           | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-09 | Atualizar uma conta de funcionário e a autorização de apoio                    | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-10 | Mostrar a autorização de apoio só ao Auxiliar Veterinário                      | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-11 | Bloquear uma conta de funcionário                                              | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-12 | Recusar o bloqueio da própria conta do Administrador                           | `f001-funcionarios.spec.ts` |
| ST-WEB-F001-13 | Registar um Cliente com conta de acesso (Rececionista)                         | `f001-clientes.spec.ts`     |
| ST-WEB-F001-14 | Registar um Cliente com NIF e morada (Administrador)                           | `f001-clientes.spec.ts`     |
| ST-WEB-F001-15 | Recusar o registo de um Cliente com dados inválidos                            | `f001-clientes.spec.ts`     |
| ST-WEB-F001-16 | Atualizar os dados de um Cliente                                               | `f001-clientes.spec.ts`     |
| ST-WEB-F001-17 | Recusar a atualização de um Cliente com dados inválidos                        | `f001-clientes.spec.ts`     |
| ST-WEB-F001-18 | Pesquisar Clientes                                                             | `f001-clientes.spec.ts`     |
| ST-WEB-F001-19 | Consultar os dados gerais e as configurações da clínica                        | `clinica.exclusivo.spec.ts` |
| ST-WEB-F001-20 | Atualizar os dados gerais e as configurações da clínica (exclusivo)            | `clinica.exclusivo.spec.ts` |
| ST-WEB-F001-21 | Recusar dados gerais inválidos da clínica (exclusivo)                          | `clinica.exclusivo.spec.ts` |
| ST-WEB-F002-01 | Registar um paciente associado a um Cliente (Rececionista)                     | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-02 | Registar um paciente a partir da pesquisa por proprietário (Veterinário)       | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-03 | Recusar o registo de um paciente com dados inválidos                           | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-04 | Consultar a ficha administrativa do paciente                                   | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-05 | Abrir a ficha de um paciente inexistente                                       | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-06 | Atualizar os dados administrativos do paciente                                 | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-07 | Recusar a atualização do paciente com dados inválidos                          | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-08 | Pesquisar os pacientes de um proprietário                                      | `f002-pacientes.spec.ts`    |
| ST-WEB-F002-09 | Pesquisa por proprietário sem resultados                                       | `f002-pacientes.spec.ts`    |
| ST-WEB-F003-01 | Gerir o horário de funcionamento (exclusivo)                                   | `clinica.exclusivo.spec.ts` |
| ST-WEB-F003-02 | Recusar períodos sobrepostos ou com fecho não posterior à abertura (exclusivo) | `clinica.exclusivo.spec.ts` |

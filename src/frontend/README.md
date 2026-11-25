# Frontend Web — Clínica Veterinária

Aplicação web (React 19 + TypeScript + Vite) usada por Administradores, Rececionistas, Veterinários e Auxiliares veterinários. Os Clientes usam apenas a aplicação móvel.

## Pré-requisitos

- [Node.js 24 LTS](https://nodejs.org/), que inclui o `npm` 11. O Backlog e o regulamento indicam npm 12: o projeto foi verificado com o npm 11 que vem com o Node 24 e não exige atualizar. (Confirme com `node -v` e `npm -v`.)
- O backend a correr (por omissão em `http://localhost:5159`) com a origem `http://localhost:5173` incluída em `Cors:AllowedOrigins`

## Instalação

```bash
cd src/frontend
npm ci
```

## Configuração

A aplicação lê o endereço do backend da variável `VITE_API_URL`.

```bash
cp .env.example .env      # em Windows (PowerShell): Copy-Item .env.example .env
```

Edite o `.env` se o backend não estiver em `http://localhost:5159`. O ficheiro `.env` não é versionado; o `.env.example` só tem valores de exemplo.

## Arranque

```bash
npm run dev
```

Abra <http://localhost:5173>. Sem sessão iniciada, qualquer endereço leva ao ecrã de login.

## Comandos

| Comando                | O que faz                                                                    |
| ---------------------- | ---------------------------------------------------------------------------- |
| `npm run dev`          | Servidor de desenvolvimento com recarga automática                           |
| `npm run build`        | Verifica os tipos (TypeScript strict) e gera `dist/`                         |
| `npm run lint`         | ESLint, sem avisos permitidos                                                |
| `npm run format`       | Formata o código com Prettier                                                |
| `npm run format:check` | Verifica a formatação (usado na CI)                                          |
| `npm test`             | Testes unitários e de integração (Vitest)                                    |
| `npm run test:e2e`     | Testes ponta-a-ponta (Playwright). Na primeira vez: `npx playwright install` |

## Estrutura

```
src/
  core/
    config.ts            URL da API (VITE_API_URL)
    api/                 ApiClient (HTTP) e ApiRequestError (erro único)
    auth/                AuthProvider, useAuth, ProtectedRoute, TokenStorage, permissoes (ações por perfil)
  layout/                AppLayout (cabeçalho + menu) e navegacao.ts (menu por perfil)
  routes/                AppRoutes (tabela de rotas) e rotas.ts (rota -> perfis)
  pages/                 Uma pasta por área (auth, funcionarios, clientes, ...)
  services/              Um Service por área; só eles falam com o ApiClient
  models/                Tipos que espelham os DTOs da API
  components/            FormField, FormErro, DataTable, SoPara
```

Regra de ouro: **as páginas nunca chamam `fetch`**. Uma página usa um Service, e o Service usa o `apiClient`.

## Como a sessão funciona

- `POST /auth/login` devolve o token; fica em `sessionStorage` (desaparece ao fechar o separador) e é enviado em todos os pedidos como `Authorization: Bearer ...`. Não há refresh token e o token nunca é escrito na consola.
- Ao recarregar a página, a sessão guardada é confirmada com `GET /auth/me` antes de se mostrar a aplicação.
- **Credenciais inválidas** (401) e **conta bloqueada** (403) mostram mensagens fixas que não revelam qual campo está errado: "Email ou palavra-passe incorretos." e "Esta conta está bloqueada. Contacte o administrador da clínica.".
- **Terminar sessão** chama `POST /auth/logout`, limpa a sessão local e deixa de enviar o token (a sessão terminada não se reutiliza, nem com o botão Voltar do browser).
- **Sessão expirada ou revogada:** se o backend responder **401** a um pedido que levava token, a sessão é limpa e o utilizador volta ao login com o aviso "A sua sessão terminou. Inicie sessão novamente.".
- **Perfis:** se o backend aceitar o login de um **Cliente**, a sessão é terminada de imediato e é mostrada uma mensagem a indicar a aplicação móvel (RNF003).
- **Rotas e menus** respeitam o perfil, também quando se escreve o endereço à mão: um perfil sem permissão é enviado para `/acesso-negado`.
- **Falhas de ligação:** cada pedido tem um tempo limite de 30 s; se o servidor não responder, aparece uma mensagem e o ecrã não fica preso.

## Permissões por ação (RF005)

Além das rotas e do menu, a interface esconde botões e campos que o perfil não pode usar. As ações e os perfis que as podem executar estão em `src/core/auth/permissoes.ts` (matriz de acesso do Modelo de Classes, §12). Quem implementa um ecrã usa-as assim:

```tsx
import { SoPara } from "../../components/SoPara";
import { useAuth } from "../../core/auth/useAuth";

// Esconde o botão a quem não pode
<SoPara acao="gerirMarcacoes">
  <button onClick={novaMarcacao}>Nova marcação</button>
</SoPara>;

// Ou, em lógica:
const { pode } = useAuth();
if (pode("registarClinico")) {
  /* campos editáveis */
}
```

Isto só melhora a interface: a API continua a ser quem autoriza e rejeita o que o perfil não pode fazer.

## Erros da API

O `ApiClient` só lança um tipo de erro, `ApiRequestError`, com `status`, `categoria` e uma `message` pronta a mostrar:

| Estado HTTP  | `categoria`                                           |
| ------------ | ----------------------------------------------------- |
| 400          | `validacao`                                           |
| 401          | `naoAutenticado`                                      |
| 403          | `semPermissao`                                        |
| 404          | `naoEncontrado`                                       |
| 409          | `conflito`                                            |
| 412          | `versaoDesatualizada`                                 |
| 422          | `regraNegocio`                                        |
| 5xx          | `servidor` (mensagem genérica, sem detalhes técnicos) |
| sem resposta | `rede`                                                |

## Como acrescentar um ecrã

1. Crie a página em `src/pages/<area>/` e o Service em `src/services/` (copie o padrão de `AuthService.ts`).
2. Em `src/routes/rotas.ts`, preencha o campo `pagina` da rota correspondente (`{ ecra: "W04", ..., pagina: ClientesPage }`). Os perfis já estão definidos segundo a matriz de acessos.
3. Se o ecrã deve aparecer no menu, confirme o item em `src/layout/navegacao.ts`.

## Componentes comuns

Exemplo de uma listagem com formulário de pesquisa:

```tsx
import { useEffect, useState } from "react";
import { DataTable } from "../../components/DataTable";
import { FormErro } from "../../components/FormErro";
import { FormField } from "../../components/FormField";
import { clienteService } from "../../services/ClienteService";

export function ClientesPage() {
  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [aCarregar, setACarregar] = useState(true);
  const [erro, setErro] = useState<unknown>(null);

  useEffect(() => {
    clienteService
      .listar()
      .then((r) => setClientes(r.itens))
      .catch(setErro)
      .finally(() => setACarregar(false));
  }, []);

  return (
    <DataTable
      colunas={[
        { cabecalho: "Nome", celula: (c) => c.nome },
        { cabecalho: "Email", celula: (c) => c.email },
      ]}
      linhas={clientes}
      chaveLinha={(c) => c.id}
      aCarregar={aCarregar}
      erro={erro}
      mensagemVazia="Ainda não há clientes."
    />
  );
}
```

- `DataTable` mostra "A carregar…", a mensagem de erro (de um `ApiRequestError`) ou a mensagem de lista vazia, conforme o caso.
- `FormField` junta etiqueta, campo e mensagem de erro (`erro={...}`), com acessibilidade.
- `FormErro` mostra o erro de um formulário a partir do que apanhou no `catch`: `<FormErro erro={erro} />`.

## Testes

`npm test` corre o Vitest com React Testing Library. Os testes não precisam do backend: simulam as respostas HTTP. Há testes do `ApiClient` (tradução de erros, token, 401, tempo limite), do `AuthService` (mensagens do login), do `TokenStorage`, das permissões por perfil, do menu e das rotas protegidas (login, sessão expirada, fim de sessão, acesso por endereço direto).

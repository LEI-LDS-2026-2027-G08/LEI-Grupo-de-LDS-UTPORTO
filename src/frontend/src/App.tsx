import { useState } from "react";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { AuthProvider } from "./core/auth/AuthContext";
import { AppRoutes } from "./routes/AppRoutes";

/**
 * Raiz da aplicação. Usa um data router (createBrowserRouter) para que os
 * ecrãs possam bloquear a navegação com alterações por guardar (useBlocker),
 * incluindo o botão «Retroceder» do navegador (I115, critério 6). As rotas
 * continuam definidas em AppRoutes, dentro de uma rota «*».
 */
function criarRouter() {
  return createBrowserRouter([
    {
      path: "*",
      element: (
        <AuthProvider>
          <AppRoutes />
        </AuthProvider>
      ),
    },
  ]);
}

export default function App() {
  // Criado na primeira renderização, para ler o endereço atual nesse momento.
  const [router] = useState(criarRouter);
  return <RouterProvider router={router} />;
}

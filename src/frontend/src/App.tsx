import { BrowserRouter } from "react-router-dom";
import { AuthProvider } from "./core/auth/AuthContext";
import { AppRoutes } from "./routes/AppRoutes";

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}

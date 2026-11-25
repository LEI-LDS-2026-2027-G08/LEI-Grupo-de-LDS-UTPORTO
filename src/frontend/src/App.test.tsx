import { render, screen } from "@testing-library/react";
import { expect, test } from "vitest";
import App from "./App";

test("sem sessão a aplicação abre no ecrã de login", () => {
  window.history.pushState({}, "", "/");
  render(<App />);
  expect(
    screen.getByRole("heading", { name: "Iniciar sessão" }),
  ).toBeInTheDocument();
});

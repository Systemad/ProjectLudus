import "./styles.css";
import "./fonts.css";

import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { RouterProvider } from "@tanstack/react-router";
import { QueryClientProvider } from "@tanstack/react-query";

import { router, queryClient } from "./router";
import { UIProvider } from "ui";
import { config, theme } from "./theme/themes";

// Register the router instance for type safety
declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <UIProvider config={config} theme={theme}>
            <QueryClientProvider client={queryClient}>
                <RouterProvider router={router} />
            </QueryClientProvider>
        </UIProvider>
    </StrictMode>,
);

import "./styles.css";
import "./fonts.css";

import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { RouterProvider, createRouter } from "@tanstack/react-router";

// Import the generated route tree
import { routeTree } from "./routeTree.gen";
import { UIProvider } from "ui";
import { config, theme } from "./theme";

const router = createRouter({
    routeTree,

    scrollRestoration: true,
    defaultPreload: "intent",
    defaultPreloadStaleTime: 0,
});

// Register the router instance for type safety
declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}
createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <UIProvider config={config} theme={theme}>
            <RouterProvider router={router} />
        </UIProvider>
    </StrictMode>,
);

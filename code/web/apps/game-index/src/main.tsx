import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import { UIProvider, defineConfig } from "@packages/ui";
import { RouterProvider, createRouter } from "@tanstack/react-router";

// Import the generated route tree
import { routeTree } from "./routeTree.gen";

// Create a new router instance

/*
createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <UIProvider>
            <App />
        </UIProvider>
    </StrictMode>
);
*/

export const config = defineConfig({
    css: { varPrefix: "ui" },
    breakpoint: { direction: "up", identifier: "@media screen" },
    defaultColorMode: "light",
    defaultThemeScheme: "base",
    //notice: { duration: 5000 },
    theme: { responsive: true },
});

// Register the router instance for type safety
declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}

const router = createRouter({ routeTree, defaultPreload: "intent" });
const rootElement = document.getElementById("root")!;
if (!rootElement.innerHTML) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <StrictMode>
            <UIProvider config={config}>
                <RouterProvider router={router} />
            </UIProvider>
        </StrictMode>
    );
}

import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import { QueryClientProvider } from "@tanstack/react-query";
import { Loading, UIProvider } from "@yamada-ui/react";
import { AuthProvider } from "./features/auth/AuthProvider";
import { useAuth } from "./features/auth/useAuth";
import { queryClient } from "./queryClient";
import { config, theme } from "../theme/index.ts";

const router = createRouter({
    routeTree,
    context: { queryClient, auth: undefined! },
    defaultPreload: "intent",
    defaultPreloadStaleTime: 0,
    defaultStructuralSharing: true,
    defaultPendingMinMs: 0,
    defaultPendingMs: 100,
    scrollRestoration: true,
    defaultPendingComponent: () => (
        <Loading variant="puff" colorScheme="emerald" />
    ),
});

export function AppInitializer() {
    const auth = useAuth();
    return <RouterProvider router={router} context={{ auth: auth }} />;
}

declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <UIProvider config={config} theme={theme}>
            <QueryClientProvider client={queryClient}>
                <AuthProvider>
                    <AppInitializer />
                </AuthProvider>
            </QueryClientProvider>
        </UIProvider>
    </StrictMode>
);

import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import { QueryClientProvider } from "@tanstack/react-query";
import { Loading, UIProvider, extendConfig } from "@yamada-ui/react";
import { AuthProvider } from "./features/auth/AuthProvider";
import { useAuth } from "./features/auth/useAuth";
import { queryClient } from "./queryClient";

// bg={["blackAlpha.50", "whiteAlpha.100"]}
const router = createRouter({
    routeTree,
    context: { queryClient, auth: undefined! },
    defaultPreload: "intent",
    defaultPreloadStaleTime: 0,
    //defaultPreloadStaleTime: 0,
    scrollRestoration: true,
    defaultPendingComponent: () => (
        <Loading variant="puff" colorScheme="emerald" />
    ),
    defaultPendingMinMs: 1500,
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

const customConfig = extendConfig({ breakpoint: { direction: "up" } });

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <UIProvider config={customConfig}>
            <QueryClientProvider client={queryClient}>
                <AuthProvider>
                    <AppInitializer />
                </AuthProvider>
            </QueryClientProvider>
        </UIProvider>
    </StrictMode>
);

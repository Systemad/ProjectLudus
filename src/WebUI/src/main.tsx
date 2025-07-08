import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Loading, UIProvider, extendConfig } from "@yamada-ui/react";

export const queryClient = new QueryClient();
// bg={["blackAlpha.50", "whiteAlpha.100"]}
const router = createRouter({
    routeTree,
    context: { queryClient },
    defaultPreload: "intent",
    defaultPreloadStaleTime: 0,
    //defaultPreloadStaleTime: 0,
    scrollRestoration: true,
    defaultPendingComponent: () => (
        <Loading variant="puff" colorScheme="emerald" />
    ),
    defaultPendingMinMs: 1500,
});

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
                <RouterProvider router={router} />
            </QueryClientProvider>
        </UIProvider>
    </StrictMode>
);

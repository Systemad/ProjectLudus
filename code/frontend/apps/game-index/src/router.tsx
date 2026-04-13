import { QueryClient } from "@tanstack/react-query";
import { createRouter } from "@tanstack/react-router";
import { setupRouterSsrQueryIntegration } from "@tanstack/react-router-ssr-query";
import { routeTree } from "./routeTree.gen";

export const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            staleTime: 1000 * 10,
        },
    },
});

export const router = createRouter({
    routeTree,
    context: { queryClient },
    scrollRestoration: true,
    defaultPreload: "intent",
    defaultPreloadStaleTime: 0,
    defaultViewTransition: {
        types: ({ fromLocation, toLocation }) => {
            let direction = "none";

            if (fromLocation) {
                const fromIndex = fromLocation.state.__TSR_index;
                const toIndex = toLocation.state.__TSR_index;
                if (fromIndex !== toIndex) {
                    direction = fromIndex > toIndex ? "right" : "left";
                }
            }

            if (fromLocation?.state.__TSR_index === toLocation?.state.__TSR_index) {
                direction = "none";
            }

            return [`slide-${direction}`];
        },
    },
});

setupRouterSsrQueryIntegration({
    router,
    queryClient,
    // optional:
    // handleRedirects: true,
    // wrapQueryClient: true,
});

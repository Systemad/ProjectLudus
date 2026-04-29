import { AppShell } from "@src/components/AppShell/AppShell";
import { createRootRouteWithContext, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import type { QueryClient } from "@tanstack/react-query";

const RootLayout = () => (
    <AppShell>
        <Outlet />
        <TanStackRouterDevtools />
    </AppShell>
);

export const Route = createRootRouteWithContext<{
    queryClient: QueryClient;
}>()({
    component: RootLayout,
});

//

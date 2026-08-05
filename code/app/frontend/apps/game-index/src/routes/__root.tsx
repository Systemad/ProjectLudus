import { AppShell } from "@src/app/app-shell";
import { createRootRouteWithContext, Outlet } from "@tanstack/react-router";
import type { QueryClient } from "@tanstack/react-query";

const RootLayout = () => {
    return (
        <AppShell>
            <Outlet />
        </AppShell>
    );
};

export const Route = createRootRouteWithContext<{
    queryClient: QueryClient;
}>()({
    component: RootLayout,
});

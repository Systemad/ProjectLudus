import AppShell from "@src/components/AppShell";
import { createRootRoute, Link, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";

const RootLayout = () => (
    <AppShell>
        <Outlet />
        <TanStackRouterDevtools />
    </AppShell>
);

export const Route = createRootRoute({ component: RootLayout });
/*
export const Route = createRootRouteWithContext<{
    queryClient: QueryClient;
}>()({
    component: RootLayout,
    notFoundComponent: () => {
        return (
            <div>
                <p>This is the notFoundComponent configured on root route</p>
                <Link to="/">Start Over</Link>
            </div>
        );
    },
});

*/

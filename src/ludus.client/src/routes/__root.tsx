import { Outlet, createRootRouteWithContext } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import type { QueryClient } from "@tanstack/react-query";

import { AppShell, Burger, Button, Container } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";

interface MyRouterContext {
    // The ReturnType of your useAuth hook or the value of your AuthContext
    //auth: AuthState
}
export const Route = createRootRouteWithContext<{
    queryClient: QueryClient;
}>()({
    component: RootComponent,
});

function RootComponent() {
    const [opened, { toggle }] = useDisclosure();
    return (
        <>
            <AppShell
                navbar={{
                    width: 200,
                    breakpoint: "sm",
                    collapsed: { mobile: !opened },
                }}
            >
                <AppShell.Navbar p="md">Navbar</AppShell.Navbar>

                <AppShell.Main>
                    <Container fluid mt={"xs"}>
                        <Outlet />
                    </Container>
                </AppShell.Main>
            </AppShell>

            <ReactQueryDevtools buttonPosition="top-right" />
            <TanStackRouterDevtools position="bottom-right" />
        </>
    );
}

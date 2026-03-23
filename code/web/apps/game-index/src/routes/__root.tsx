import {
    createRootRouteWithContext,
    Link,
    Outlet,
} from "@tanstack/react-router";
import { Flex } from "@packages/ui";
import { NavBar } from "../layouts/Navigation/Navbar";
import type { QueryClient } from "@tanstack/react-query";

const RootLayout = () => (
    <Flex w="full" alignItems="center" flexDirection="column" minH="100dvh">
        <NavBar />
        <Flex
            alignItems="center"
            flex="1"
            flexDirection="column"
            maxW="8xl"
            w="full"
        >
            <Flex
                as="main"
                flex="1"
                flexDirection="column"
                px="xs"
                w="full"
                pb="md"
            >
                <Outlet />
            </Flex>
        </Flex>
    </Flex>
);

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
/*
<TanStackRouterDevtools />
*/

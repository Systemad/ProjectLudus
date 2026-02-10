import { createRootRoute, Outlet } from "@tanstack/react-router";
import { Flex } from "@packages/ui";
import { NavBar } from "../layouts/Navigation/Navbar";

const RootLayout = () => (
    <Flex w="full" alignItems="center" flexDirection="column" minH="100dvh">
        <Flex
            alignItems="center"
            flex="1"
            flexDirection="column"
            maxW="8xl"
            w="full"
        >
            <NavBar />
            <Flex
                as="main"
                flex="1"
                flexDirection="column"
                px="xs"
                w="full"
                pb="md"
                scrollbarGutter={"stable"}
            >
                <Outlet />
            </Flex>
        </Flex>
    </Flex>
);

export const Route = createRootRoute({ component: RootLayout });
/*
<TanStackRouterDevtools />
*/

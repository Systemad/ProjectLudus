import { createRootRoute, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Box, Flex } from "@packages/ui";
import { NavBar } from "../layouts/Navigation/Navbar";

const RootLayout = () => (
    <>
        <NavBar />
        <Flex as="main" justify="center">
            <Box flex="1" maxW="9xl" w="full" minW="0">
                <Outlet />
            </Box>
        </Flex>
        <TanStackRouterDevtools />
    </>
);

export const Route = createRootRoute({ component: RootLayout });

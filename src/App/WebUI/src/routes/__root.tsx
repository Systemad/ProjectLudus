import type { QueryClient } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import {
    createRootRouteWithContext,
    Link,
    Outlet,
} from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Container, VStack } from "@yamada-ui/react";
import type { AuthContext } from "~/features/auth/AuthContext";
import { Header } from "~/layouts/Header/header";

export const Route = createRootRouteWithContext<{
    queryClient: QueryClient;
    auth: AuthContext;
}>()({
    component: RootComponent,
    notFoundComponent: () => {
        return (
            <div>
                <p>This is the notFoundComponent configured on root route</p>
                <Link to="/">Start Over</Link>
            </div>
        );
    },
});

function RootComponent() {
    return (
        <VStack as="main">
            <Header />

            <Container centerContent>
                <Outlet />
            </Container>

            <ReactQueryDevtools buttonPosition="bottom-left" />
            <TanStackRouterDevtools position="bottom-right" />
        </VStack>
    );
}

/*
            <Center>
                <HStack
                    alignItems="flex-start"
                    gap="0"
                    maxW="9xl"
                    px={{ base: "lg", md: "md" }}
                    py={{ base: "lg", sm: "normal" }}
                    w="full"
                >
                    <VStack as="main" flex="1" gap="0" minW="0">
                        <Outlet />
                    </VStack>
                </HStack>
            </Center>
*/

/*
            <Box
                bg={{
                    base: "red.500",
                    xl: "blue.500",
                    lg: "green.500",
                    md: "yellow.500",
                    sm: "purple.500",
                }}
                p="md"
                rounded="md"
                color="white"
                transitionProperty="all"
                transitionDuration="slower"
            >
                The current breakpoint is "{breakpoint}"
            </Box>
*/

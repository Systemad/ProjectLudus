import type { QueryClient } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import {
    createRootRouteWithContext,
    Link,
    Outlet,
} from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";
import { Container } from "@yamada-ui/react";
import { Header } from "~/layouts/header";

export const Route = createRootRouteWithContext<{
    queryClient: QueryClient;
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
        <div>
            <Header />

            <Container
                as="main"
                maxW={{ base: "92rem" }}
                w="full"
                px={{ base: "lg", md: "md" }}
                mx="auto"
            >
                <Outlet />
            </Container>

            <ReactQueryDevtools buttonPosition="top-right" />
            <TanStackRouterDevtools position="bottom-right" />
        </div>
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

import { createFileRoute } from "@tanstack/react-router";
import {
    SimpleGrid,
    GridItem,
    Pagination,
    Flex,
    Heading,
    Container,
} from "@yamada-ui/react";
import { useState } from "react";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
import { usePublicGamesGetTopRatedGamesEndpointSuspenseHook } from "~/gen";
export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    const [page, onChange] = useState<number>(1);
    const { data } = usePublicGamesGetTopRatedGamesEndpointSuspenseHook(
        {
            params: {
                pageNumber: page,
                pageSize: 40,
            },
        },
        {
            query: {
                queryKey: ["games", page],
            },
        }
    );

    return (
        <Container
            centerContent
            layerStyle="container"
            zIndex="nappa"
            maxW={"9xl"}
        >
            <Flex direction={"column"} alignItems={"center"} gap={"xl"}>
                <Flex justifyContent="flex-start" w="full">
                    <Heading>Most popular games</Heading>
                </Flex>
                <SimpleGrid
                    columns={{ base: 1, sm: 2, md: 2, lg: 3, xl: 5 }}
                    gap="lg"
                >
                    {data.items.map((item) => (
                        <GridItem key={item.id}>
                            <HoverGameCard item={item} />
                        </GridItem>
                    ))}
                </SimpleGrid>
                <Pagination
                    size="md"
                    alignSelf={"center"}
                    siblings={2}
                    page={data.pageNumber}
                    total={data.pageCount}
                    onChange={onChange}
                />
            </Flex>
        </Container>
    );
}

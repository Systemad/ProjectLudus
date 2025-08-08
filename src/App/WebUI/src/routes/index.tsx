import { keepPreviousData } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import {
    SimpleGrid,
    GridItem,
    Pagination,
    Flex,
    Heading,
} from "@yamada-ui/react";
import { useState } from "react";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
import { usePublicGamesGetTopRatedGamesEndpointHook } from "~/gen";
export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    const [page, onChange] = useState<number>(1);
    const { data, isPending, isPlaceholderData, isFetching } =
        usePublicGamesGetTopRatedGamesEndpointHook(
            {
                params: {
                    pageNumber: page,
                    pageSize: 40,
                },
            },
            {
                query: {
                    queryKey: ["games", page],
                    placeholderData: keepPreviousData,
                },
            }
        );

    if (isPending) {
        return "loading";
    }
    return (
        <>
            {data && (
                <Flex direction={"column"} alignItems={"center"} gap={"xl"}>
                    <Flex justifyContent="flex-start" w="full">
                        <Heading>Most popular games</Heading>
                    </Flex>
                    <SimpleGrid
                        columns={{ base: 1, sm: 2, md: 2, lg: 3, xl: 5 }}
                        gap="lg"
                    >
                        {data?.items.map((item) => (
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
                        disabled={isPlaceholderData}
                    />
                </Flex>
            )}

            {isFetching ? <span> Loading...</span> : null}
        </>
    );
}

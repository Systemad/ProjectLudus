import { createFileRoute } from "@tanstack/react-router";
import { keepPreviousData } from "@tanstack/react-query";
import { SimpleGrid } from "@mantine/core";
import { GameCard } from "~/features/components/GameCard/GameCard";
import { usePublicGamesGetTopRatedGamesEndpoint } from "~/api";
export const Route = createFileRoute("/")({
    component: Index,
});

function Index() {
    const { isPending, isError, data, error } =
        usePublicGamesGetTopRatedGamesEndpoint(
            {
                pageNumber: 1,
                pageSize: 40,
            },
            {
                query: {
                    queryKey: ["games"],
                    placeholderData: keepPreviousData,
                },
            }
        );

    if (isPending) {
        return <span>Loading...</span>;
    }

    if (isError) {
        return <span>Error: {error.message}</span>;
    }

    return (
        <SimpleGrid cols={5}>
            {data?.data.items?.map((item, index) => (
                <GameCard key={index} Game={item}></GameCard>
            ))}
        </SimpleGrid>
    );
}

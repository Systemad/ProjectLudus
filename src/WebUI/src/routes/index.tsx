import { createFileRoute } from "@tanstack/react-router";
import { SimpleGrid, GridItem } from "@yamada-ui/react";
import { usePublicGamesGetTopRatedGamesEndpoint } from "~/api";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    const { data, isPending } = usePublicGamesGetTopRatedGamesEndpoint({
        pageNumber: 1,
        pageSize: 20,
    });

    if (isPending) {
        return "loading";
    }
    return (
        <SimpleGrid columns={{ base: 1, sm: 2, md: 2, lg: 3, xl: 4 }} gap="lg">
            {data?.items.map((item) => (
                <GridItem key={item.id}>
                    <HoverGameCard id={item.id} />
                </GridItem>
            ))}
        </SimpleGrid>
    );
}

/*
        <SimpleGrid columns={{ base: 1, sm: 2, md: 2, lg: 3, xl: 4 }} gap="lg">
            {Array.from({ length: 25 }, (_, i) => i).map((i) => (
                <GridItem key={i}>
                    <HoverGameCard id={i} />
                </GridItem>
            ))}
        </SimpleGrid>
*/

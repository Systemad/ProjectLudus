import { createFileRoute } from "@tanstack/react-router";
import { SimpleGrid, GridItem } from "@yamada-ui/react";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
import { usePublicGamesGetTopRatedGamesEndpointHook } from "~/gen";
export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    const { data, isPending } = usePublicGamesGetTopRatedGamesEndpointHook(
        {
            pageNumber: 1,
            pageSize: 40,
        },
        { query: { queryKey: ["games"] } }
    );

    if (isPending) {
        return "loading";
    }
    return (
        <SimpleGrid columns={{ base: 1, sm: 2, md: 2, lg: 3, xl: 5 }} gap="lg">
            {data?.items.map((item) => (
                <GridItem key={item.id}>
                    <HoverGameCard item={item} />
                </GridItem>
            ))}
        </SimpleGrid>
    );
}

import { createFileRoute } from "@tanstack/react-router";
import { SimpleGrid, GridItem } from "@yamada-ui/react";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    return (
        <SimpleGrid w="full" columns={{ base: 5, md: 1 }} gap="md">
            {Array.from({ length: 25 }).map((_, i) => (
                <GridItem key={i} w="full">
                    <HoverGameCard />
                </GridItem>
            ))}
        </SimpleGrid>
    );
}

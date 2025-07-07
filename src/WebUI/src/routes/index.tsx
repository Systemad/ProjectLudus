import { createFileRoute } from "@tanstack/react-router";
import { SimpleGrid, GridItem } from "@yamada-ui/react";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    return (
        <SimpleGrid columns={{ base: 5, md: 2, lg: 3, xl: 4 }} gap="lg">
            {Array.from({ length: 25 }, (_, i) => i).map((i) => (
                <GridItem key={i}>
                    <HoverGameCard id={i} />
                </GridItem>
            ))}
        </SimpleGrid>
    );
}

import { createFileRoute } from "@tanstack/react-router";
import { SimpleGrid } from "@mantine/core";
import { GameCard } from "~/features/components/GameCard/GameCard";
export const Route = createFileRoute("/")({
    component: Index,
});

function Index() {
    return (
        <SimpleGrid cols={5}>
            {Array.from({ length: 18 }, (_, i) => (
                <GameCard key={i} Id={i.toString()}></GameCard>
            ))}
        </SimpleGrid>
    );
}

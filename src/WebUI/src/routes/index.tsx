import { createFileRoute } from "@tanstack/react-router";
import {
    SimpleGrid,
    GridItem,
    useBreakpoint,
    Box,
    useBreakpointValue,
} from "@yamada-ui/react";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    const breakpoint = useBreakpoint();

    const bg = useBreakpointValue({
        base: "red.500",
        "2xl": "pink.500",
        xl: "blue.500",
        lg: "green.500",
        md: "yellow.500",
        sm: "purple.500",
    });
    return (
        <>
            <Box bg={bg} p="md">
                The current breakpoint is "{breakpoint}"
            </Box>

            <SimpleGrid columns={{ base: 5, md: 2, lg: 3, xl: 4 }} gap="lg">
                {Array.from({ length: 25 }).map((index, i) => (
                    <GridItem key={i}>
                        <HoverGameCard key={i} />
                    </GridItem>
                ))}
            </SimpleGrid>
        </>
    );
}

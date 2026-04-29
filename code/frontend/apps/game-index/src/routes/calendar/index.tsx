import { createFileRoute } from "@tanstack/react-router";
import { Suspense } from "react";
import { useCalendarGetGamesSuspenseHook } from "@src/gen/catalogApi";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import { getYear } from "date-fns";
import { isTbaReleaseDate } from "@src/utils/dateUtils";
import { groupGamesByMonth } from "@src/features/calendar/utils/groupGamesByMonth";
import { GameGroupCard } from "@src/features/calendar/components/GameGroupCard";
import { Box, Grid, GridItem, Heading, Loading, Text, VStack } from "ui";

export const Route = createFileRoute("/calendar/")({
    component: RouteComponent,
});

function GamesCalendarPage() {
    const year = getYear(new Date());
    const { data } = useCalendarGetGamesSuspenseHook({ year });
    const tbaGames = data.games.filter((game) => isTbaReleaseDate(game.firstReleaseDate));
    const datedGames = data.games.filter((game) => !isTbaReleaseDate(game.firstReleaseDate));
    const groups = groupGamesByMonth(datedGames);

    return (
        <VStack align="stretch" gap="6">
            <Box>
                <Heading fontSize="2xl" fontWeight="bold">
                    {year}
                </Heading>
                <Text fontSize="sm" color="fg.muted" mt="1">
                    Most anticipated releases this year
                </Text>
            </Box>

            {groups.length === 0 ? (
                <Box rounded="xl" bg="bg.panel" p={{ base: "6", md: "8" }} textAlign="center">
                    <Text fontSize="lg" color="fg.base" fontWeight="semibold">
                        No scheduled games
                    </Text>
                    <Text fontSize="sm" color="fg.muted" mt="1">
                        Games with placeholder dates are listed separately.
                    </Text>
                </Box>
            ) : (
                <Grid
                    templateColumns={{ base: "1fr", md: "repeat(2, 1fr)", xl: "repeat(4, 1fr)" }}
                    gap="4"
                >
                    {groups.map((group) => (
                        <GridItem key={group.month}>
                            <GameGroupCard
                                title={group.month}
                                games={group.games}
                                emptyLabel="No games scheduled"
                            />
                        </GridItem>
                    ))}
                </Grid>
            )}

            <GameGroupCard
                title="Games expected to release this year"
                games={tbaGames}
                emptyLabel="No TBD games"
                isPlaceholder
            />
        </VStack>
    );
}

function LoadingFallback() {
    return (
        <Box display="grid" placeItems="center" minH="64">
            <Loading.Rings color="primary.500" fontSize="5xl" />
        </Box>
    );
}

function RouteComponent() {
    return (
        <PageWrapper maxW="9xl" py={{ base: "4", md: "6" }}>
            <Suspense fallback={<LoadingFallback />}>
                <GamesCalendarPage />
            </Suspense>
        </PageWrapper>
    );
}

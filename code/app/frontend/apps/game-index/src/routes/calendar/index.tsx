import { createFileRoute } from "@tanstack/react-router";
import { Suspense } from "react";
import { useCalendarGetGamesSuspenseHook } from "@src/gen/catalogApi";
import { getYear } from "date-fns";
import { isTbaReleaseDate } from "@src/utils/dateUtils";
import { groupGamesByMonth } from "@src/features/calendar/utils/group-games-by-month";
import { GameGroupCard } from "@src/features/calendar/components/calendar-game-card";
import { Card } from "@astryxdesign/core/Card";
import { Grid } from "@astryxdesign/core/Grid";
import { Text, Heading } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";
import { Spinner } from "@astryxdesign/core/Spinner";
import * as stylex from "@stylexjs/stylex";
import { presentationStyles } from "@src/app/presentation-styles";

export const Route = createFileRoute("/calendar/")({
    component: RouteComponent,
});

function GamesCalendarPage() {
    const year = getYear(new Date());
    const { data } = useCalendarGetGamesSuspenseHook({
        year,
        params: { Page: 1, PageSize: 50 },
    });
    const tbaGames = data.games.filter((game) => isTbaReleaseDate(game.firstReleaseDate));
    const datedGames = data.games.filter((game) => !isTbaReleaseDate(game.firstReleaseDate));
    const groups = groupGamesByMonth(datedGames);

    return (
        <VStack hAlign="stretch" gap={6}>
            <div {...stylex.props(presentationStyles.pageHeader)}>
                <div>
                <Heading level={2}>
                    {year}
                </Heading>
                <Text color="secondary" style={{fontSize: "0.875rem", marginTop: "0.25rem"}}>
                    Most anticipated releases this year
                </Text>
                </div>
            </div>

            {groups.length === 0 ? (
                <Card padding={6} style={{textAlign: "center"}}>
                    <Text weight="semibold" style={{fontSize: "1.125rem"}}>
                        No scheduled games
                    </Text>
                    <Text color="secondary" style={{fontSize: "0.875rem", marginTop: "0.25rem"}}>
                        Games with placeholder dates are listed separately.
                    </Text>
                </Card>
            ) : (
                <Grid columns={{minWidth: 280}} gap={4}>
                    {groups.map((group) => (
                        <div key={group.month}>
                            <GameGroupCard
                                title={group.month}
                                games={group.games}
                                emptyLabel="No games scheduled"
                            />
                    </div>
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
        <div {...stylex.props(presentationStyles.loadingCenter)}>
            <Spinner size="xl" />
        </div>
    );
}

function RouteComponent() {
    return (
        <Suspense fallback={<LoadingFallback />}>
            <GamesCalendarPage />
        </Suspense>
    );
}

import { createFileRoute } from "@tanstack/react-router";
import { Spinner } from "@astryxdesign/core/Spinner";
import { Grid } from "@astryxdesign/core/Grid";
import { Heading, Text } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";
import { Suspense } from "react";
import { useSuspenseQueries } from "@tanstack/react-query";
import * as stylex from "@stylexjs/stylex";

import { MostPlayedTable } from "@src/features/homepage/components/most-played-table";
import { PopularReleasesTable } from "@src/features/homepage/components/popular-releases-table";
import { HotReleasesTable } from "@src/features/homepage/components/hot-releases-table";
import { EventCarousel } from "@src/features/homepage/components/event-carousel";
import {
    eventsGetListSuspenseQueryOptionsHook,
    steamChartSuspenseQueryOptionsHook,
} from "@src/gen/catalogApi";
import { presentationStyles } from "@src/app/presentation-styles";

export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function RouteComponent() {
    const [
        { data: mostPlayed },
        { data: popularReleases },
        { data: hotReleases },
        { data: upcomingEvents },
    ] = useSuspenseQueries({
        queries: [
            steamChartSuspenseQueryOptionsHook({
                params: { Type: "most-played", Page: 1, PageSize: 15 },
            }),
            steamChartSuspenseQueryOptionsHook({
                params: { Type: "popular-releases", Page: 1, PageSize: 15 },
            }),
            steamChartSuspenseQueryOptionsHook({
                params: { Type: "hot-releases", Page: 1, PageSize: 15 },
            }),
            eventsGetListSuspenseQueryOptionsHook({
                params: { year: new Date().getFullYear(), limit: 12, status: "notstarted" },
            }),
        ],
    });

    return (
        <Suspense
            fallback={
                <VStack hAlign="center" vAlign="center" xstyle={presentationStyles.loadingCenter}>
                    <Spinner size="xl" />
                </VStack>
            }
        >
            <VStack hAlign="stretch" gap={5}>
                <div {...stylex.props(presentationStyles.pageHeader)}>
                    <div>
                        <Heading level={1}>Game index</Heading>
                        <Text color="secondary">Live activity, popular releases, and upcoming events.</Text>
                    </div>
                    <Text color="secondary" xstyle={presentationStyles.metric}>Updated from current catalog data</Text>
                </div>
                <Grid columns={{ minWidth: 420 }} gap={4}>
                    <MostPlayedTable games={mostPlayed.games} />
                    <PopularReleasesTable games={popularReleases.games} />
                </Grid>
                <HotReleasesTable games={hotReleases.games} />
                <EventCarousel events={upcomingEvents?.events} />
            </VStack>
        </Suspense>
    );
}

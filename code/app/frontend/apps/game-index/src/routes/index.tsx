import { createFileRoute } from "@tanstack/react-router";
import { Spinner } from "@astryxdesign/core/Spinner";
import { Grid } from "@astryxdesign/core/Grid";
import { VStack } from "@astryxdesign/core/VStack";
import { Suspense } from "react";
import { useSuspenseQueries } from "@tanstack/react-query";

import { PageWrapper } from "@src/app/page-wrapper";
import { MostPlayedTable } from "@src/features/homepage/components/most-played-table";
import { PopularReleasesTable } from "@src/features/homepage/components/popular-releases-table";
import { HotReleasesTable } from "@src/features/homepage/components/hot-releases-table";
import { EventCarousel } from "@src/features/homepage/components/event-carousel";
import {
    eventsGetListSuspenseQueryOptionsHook,
    steamChartSuspenseQueryOptionsHook,
} from "@src/gen/catalogApi";

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
            steamChartSuspenseQueryOptionsHook({ params: { Type: "most-played", Limit: 15 } }),
            steamChartSuspenseQueryOptionsHook({ params: { Type: "popular-releases", Limit: 15 } }),
            steamChartSuspenseQueryOptionsHook({ params: { Type: "hot-releases", Limit: 15 } }),
            eventsGetListSuspenseQueryOptionsHook({
                params: { year: new Date().getFullYear(), limit: 12, status: "notstarted" },
            }),
        ],
    });

    return (
        <Suspense
            fallback={
                <PageWrapper paddingBlock="clamp(1rem, 3vw, 1.5rem)">
                    <VStack hAlign="center" vAlign="center" style={{minHeight: "60vh"}}>
                        <Spinner size="xl" />
                    </VStack>
                </PageWrapper>
            }
        >
            <PageWrapper paddingBlock="clamp(0.5rem, 2vw, 0.5rem)">
                <VStack hAlign="stretch" gap={4}>
                    <Grid columns={{minWidth: 320}} gap={4}>
                        <MostPlayedTable games={mostPlayed.games} />
                        <PopularReleasesTable games={popularReleases.games} />
                        <HotReleasesTable games={hotReleases.games} />
                    </Grid>
                    <EventCarousel events={upcomingEvents?.events} />
                </VStack>
            </PageWrapper>
        </Suspense>
    );
}

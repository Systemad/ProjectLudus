import { createFileRoute } from "@tanstack/react-router";
import { Loading, SimpleGrid, VStack } from "ui";
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
                <PageWrapper py={{ base: "4", md: "6" }}>
                    <VStack align="center" justify="center" minH="60vh">
                        <Loading.Rings color="blue.500" fontSize="5xl" />
                    </VStack>
                </PageWrapper>
            }
        >
            <PageWrapper py={{ base: "2", md: "2" }}>
                <VStack align="stretch" gap="md">
                    <SimpleGrid columns={{ base: 1, md: 2 }} gap="md">
                        <MostPlayedTable games={mostPlayed.games} />
                        <PopularReleasesTable games={popularReleases.games} />
                        <HotReleasesTable games={hotReleases.games} />
                    </SimpleGrid>
                    <EventCarousel events={upcomingEvents?.events} />
                </VStack>
            </PageWrapper>
        </Suspense>
    );
}

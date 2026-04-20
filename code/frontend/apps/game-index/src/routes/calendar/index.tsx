import { createFileRoute } from "@tanstack/react-router";
import { Suspense } from "react";
import { Box, Loading } from "ui";
import { VerticalCalendar } from "@src/features/calendar/components/VerticalCalendar";
import PageWrapper from "@src/components/AppShell/PageWrapper";

export const Route = createFileRoute("/calendar/")({
    component: RouteComponent,
});

function LoadingFallback() {
    return (
        <Box display="grid" placeItems="center" minH="screen">
            <Loading.Rings color="primary.500" fontSize="5xl" />
        </Box>
    );
}

function RouteComponent() {
    return (
        <PageWrapper>
            <Suspense fallback={<LoadingFallback />}>
                <VerticalCalendar />
            </Suspense>
        </PageWrapper>
    );
}

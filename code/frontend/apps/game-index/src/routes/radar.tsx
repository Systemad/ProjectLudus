import { createFileRoute } from "@tanstack/react-router";
import { PageWrapper } from "@src/components/layout/PageWrapper";
import { ReleaseRadarSearchLayout } from "@src/Typesense/Search/ReleaseRadarSearchLayout";

export const Route = createFileRoute("/radar")({
    component: RouteComponent,
});

function RouteComponent() {
    return (
        <PageWrapper px={{ base: "4", md: "6", xl: "8" }} py={{ base: "3", md: "5" }}>
            <ReleaseRadarSearchLayout />
        </PageWrapper>
    );
}

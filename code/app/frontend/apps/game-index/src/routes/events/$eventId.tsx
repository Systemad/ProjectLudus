import { createFileRoute, Link } from "@tanstack/react-router";
import { useState } from "react";
import { Card } from "@astryxdesign/core/Card";
import { Text, Heading } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { Grid } from "@astryxdesign/core/Grid";
import { SegmentedControl, SegmentedControlItem } from "@astryxdesign/core/SegmentedControl";
import { Spinner } from "@astryxdesign/core/Spinner";
import { EmptyState } from "@astryxdesign/core/EmptyState";
import { useEventsGetByIdHook } from "@src/gen/catalogApi";
import { PageWrapper } from "@src/app/page-wrapper";
import { GameCard } from "@src/features/game/components/game-card";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { formatIsoDateTime } from "@src/utils/dateUtils";
import { CountdownClock } from "@src/features/event/components/event-countdown";

const createAnyFileRoute = createFileRoute as any;

export const Route = createAnyFileRoute("/events/$eventId")({
    component: EventDetailPage,
});

function EventDetailPage() {
    const { eventId } = Route.useParams();
    const { data, isLoading } = useEventsGetByIdHook({ id: Number(eventId) });
    const [showLocalTime, setShowLocalTime] = useState(true);

    if (isLoading || !data) {
        return (
            <PageWrapper paddingBlock="clamp(1rem, 3vw, 1.5rem)">
                <div style={{ display: "grid", placeItems: "center", minHeight: "16rem" }}>
                    <Spinner size="xl" />
                </div>
            </PageWrapper>
        );
    }

    const { event } = data;
    const localTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    const coverUrl = event.games[0]?.coverUrl
        ? getIGDBImageUrl(event.games[0].coverUrl, "1080p")
        : null;
    const logoUrl = event.logoImageId ? getIGDBImageUrl(event.logoImageId, "logo_med") : null;

    const timeZone = showLocalTime ? undefined : (event.timeZone ?? "UTC");
    const formatEventDate = (value?: string | null) =>
        formatIsoDateTime(value, { timeZone }) ?? "TBD";

    const startTimeBox = formatEventDate(event.startTimeUtc);
    const endTimeBox = formatEventDate(event.endTimeUtc);
    const activeTimeZoneLabel = showLocalTime ? localTimeZone : (event.timeZone ?? "UTC");

    return (
        <PageWrapper maxWidth="var(--spacing-9xl, 1128px)" paddingBlock="clamp(0.75rem, 3vw, 1.5rem)">
            <VStack hAlign="stretch" gap={6}>
                <VStack hAlign="stretch" gap={4}>
                    <Link
                        to="/events"
                        style={{ color: "inherit", textDecoration: "none", width: "fit-content" }}
                    >
                        <Text color="secondary" style={{fontSize: "0.875rem"}}>
                            Back to events
                        </Text>
                    </Link>
                    <Grid columns={{minWidth: 300}} gap={4}>
                        <div
                            style={{
                                borderRadius: "var(--radius-2xl)",
                                overflow: "hidden",
                                background: "var(--bg-panel)",
                                minHeight: "16rem",
                                position: "relative",
                            }}
                        >
                            {coverUrl ? (
                                <img
                                    src={coverUrl}
                                    alt={event.name}
                                    style={{ width: "100%", height: "100%", objectFit: "cover" }}
                                />
                            ) : (
                                <div style={{ width: "100%", height: "100%", background: "var(--bg-subtle)" }} />
                            )}
                            {logoUrl ? (
                                <div
                                    style={{
                                        position: "absolute",
                                        inset: 0,
                                        display: "grid",
                                        placeItems: "center",
                                    }}
                                >
                                    <img
                                        src={logoUrl}
                                        alt={event.name}
                                        style={{
                                            maxWidth: "10rem",
                                            maxHeight: "7rem",
                                            objectFit: "contain",
                                        }}
                                    />
                                </div>
                            ) : null}
                        </div>

                        <VStack hAlign="stretch" gap={4}>
                            <VStack hAlign="stretch" gap={2}>
                                <HStack hAlign="between" gap={3} style={{alignItems: "baseline"}}>
                                    <Text color="secondary" style={{fontSize: "0.875rem"}}>
                                        Event
                                    </Text>
                                    <SegmentedControl
                                        value={showLocalTime ? "my" : "event"}
                                        onChange={(next) => setShowLocalTime(next === "my")}
                                        label="Time display"
                                        size="sm"
                                    >
                                        <SegmentedControlItem value="my" label="My time" />
                                        <SegmentedControlItem value="event" label="Event time" />
                                    </SegmentedControl>
                                </HStack>
                            <Heading level={1} style={{fontSize: "clamp(1.875rem, 4vw, 2.5rem)", lineHeight: "1.1"}}>
                                {event.name}
                            </Heading>
                                {event.description ? (
                                    <Text color="secondary">{event.description}</Text>
                                ) : null}
                            </VStack>

                            <Grid columns={2} gap={3}>
                                <Card>
                                    <CountdownClock
                                        startUtc={event.startTimeUtc}
                                        endUtc={event.endTimeUtc}
                                    />
                                </Card>
                                <Card>
                                <VStack hAlign="start" gap={1}>
                                        <Text
                                            color="secondary"
                                            style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                                        >
                                            {showLocalTime ? "Your Time" : "Event Time"}
                                        </Text>
                                        <Text weight="semibold">{startTimeBox}</Text>
                                        <Text color="secondary" style={{fontSize: "0.875rem"}}>
                                            {activeTimeZoneLabel}
                                        </Text>
                                    </VStack>
                                </Card>
                                <Card>
                                    <VStack hAlign="start" gap={1}>
                                        <Text
                                            color="secondary"
                                            style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                                        >
                                            Ends
                                        </Text>
                                        <Text weight="semibold">{endTimeBox}</Text>
                                        <Text color="secondary" style={{fontSize: "0.875rem"}}>
                                            {activeTimeZoneLabel}
                                        </Text>
                                    </VStack>
                                </Card>
                            </Grid>
                        </VStack>
                    </Grid>
                </VStack>

                <VStack hAlign="stretch" gap={4}>
                    <Heading level={3}>Related Games</Heading>
                    <HStack
                        gap={3}
                        vAlign="stretch"
                        style={{overflowX: "auto", overflowY: "hidden", paddingBottom: "0.5rem"}}
                    >
                        {event.games.length > 0 ? (
                            event.games.map((game) => <GameCard key={game.id} game={game} />)
                        ) : (
                            <EmptyState title="No games yet" description="No related games linked to this event yet." />
                        )}
                    </HStack>
                </VStack>
            </VStack>
        </PageWrapper>
    );
}

import { createFileRoute, Link } from "@tanstack/react-router";
import { useState } from "react";
import { Card } from "@astryxdesign/core/Card";
import { Text, Heading } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { Grid } from "@astryxdesign/core/Grid";
import { SegmentedControl } from "@astryxdesign/core/SegmentedControl";
import { Spinner } from "@astryxdesign/core/Spinner";
import { EmptyState } from "@astryxdesign/core/EmptyState";
import { Gamepad2Icon } from "react-icons/hi2";
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
            <PageWrapper py={{ base: "4", md: "6" }}>
                <div style={{ display: "grid", placeItems: "center", minHeight: "16rem" }}>
                    <Spinner color="primary.500" fontSize="5xl" />
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
        <PageWrapper maxW="9xl" py={{ base: "3", md: "6" }}>
            <VStack hAlign="stretch" gap={{ base: "6", md: "8" }}>
                <VStack hAlign="stretch" gap={4}>
                    <Link
                        to="/events"
                        style={{ color: "inherit", textDecoration: "none", width: "fit-content" }}
                    >
                        <Text fontSize="sm" color="secondary">
                            Back to events
                        </Text>
                    </Link>
                    <Grid
                        templateColumns={{ base: "1fr", lg: "1.4fr 1fr" }}
                        gap={{ base: "4", lg: "8" }}
                    >
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

                        <VStack hAlign="stretch" gap={{ base: "4", md: "5" }}>
                            <VStack hAlign="stretch" gap={2}>
                                <HStack hAlign="between" vAlign="baseline" gap={3}>
                                    <Text fontSize="sm" color="secondary">
                                        Event
                                    </Text>
                                    <SegmentedControl.Root
                                        value={showLocalTime ? "my" : "event"}
                                        onChange={(next) => setShowLocalTime(next === "my")}
                                        size="sm"
                                        colorScheme="neutral"
                                        fullRounded
                                        maxW={{ base: "full", md: "auto" }}
                                    >
                                        <SegmentedControl.Item value="my">
                                            My time
                                        </SegmentedControl.Item>
                                        <SegmentedControl.Item value="event">
                                            Event time
                                        </SegmentedControl.Item>
                                    </SegmentedControl.Root>
                                </HStack>
                                <Heading fontSize={{ base: "3xl", md: "4xl" }} lineHeight="shorter">
                                    {event.name}
                                </Heading>
                                {event.description ? (
                                    <Text color="secondary">{event.description}</Text>
                                ) : null}
                            </VStack>

                            <Grid columns={{ base: 1, md: 2 }} gap={{ base: "3", md: "4" }}>
                            <Card>
                                    <CountdownClock
                                        startUtc={event.startTimeUtc}
                                        endUtc={event.endTimeUtc}
                                    />
                                </Card>
                                <Card>
                                <VStack hAlign="start" gap={1}>
                                        <Text
                                            fontSize="xs"
                                            color="secondary"
                                            textTransform="uppercase"
                                            letterSpacing="widest"
                                        >
                                            {showLocalTime ? "Your Time" : "Event Time"}
                                        </Text>
                                        <Text fontWeight="semibold">{startTimeBox}</Text>
                                        <Text fontSize="sm" color="secondary">
                                            {activeTimeZoneLabel}
                                        </Text>
                                    </VStack>
                                </Card>
                                <Card>
                                    <VStack hAlign="start" gap={1}>
                                        <Text
                                            fontSize="xs"
                                            color="secondary"
                                            textTransform="uppercase"
                                            letterSpacing="widest"
                                        >
                                            Ends
                                        </Text>
                                        <Text fontWeight="semibold">{endTimeBox}</Text>
                                        <Text fontSize="sm" color="secondary">
                                            {activeTimeZoneLabel}
                                        </Text>
                                    </VStack>
                                </Card>
                            </Grid>
                        </VStack>
                    </Grid>
                </VStack>

                <VStack hAlign="stretch" gap={4}>
                    <Heading fontSize="xl">Related Games</Heading>
                    <HStack
                        gap={{ base: "3", md: "4" }}
                        overflowX="auto"
                        overflowY="hidden"
                        vAlign="stretch"
                        pb="2"
                    >
                        {event.games.length > 0 ? (
                            event.games.map((game) => <GameCard key={game.id} game={game} />)
                        ) : (
                            <EmptyState.Root
                                description="No related games linked to this event yet."
                                indicator={<Gamepad2Icon />}
                            />
                        )}
                    </HStack>
                </VStack>
            </VStack>
        </PageWrapper>
    );
}

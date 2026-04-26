import { createFileRoute, Link } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import {
    Box,
    EmptyState,
    For,
    Gamepad2Icon,
    Grid,
    Heading,
    HStack,
    Image,
    Loading,
    SegmentedControl,
    SimpleGrid,
    Text,
    VStack,
} from "ui";
import { useEventsGetByIdHook } from "@src/gen/catalogApi";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import { GameCard } from "@src/components/game/GameCard";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { formatIsoDateTime } from "@src/utils/dateUtils";
import { parseISO } from "date-fns";

const createAnyFileRoute = createFileRoute as any;

export const Route = createAnyFileRoute("/events/$eventId")({
    component: EventDetailPage,
});

function CountdownClock({
    startUtc,
    endUtc,
}: {
    startUtc: string | null | undefined;
    endUtc: string | null | undefined;
}) {
    const [now, setNow] = useState(() => new Date());

    useEffect(() => {
        const timer = window.setInterval(() => setNow(new Date()), 1000);
        return () => window.clearInterval(timer);
    }, []);

    if (!startUtc) {
        return (
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="2xl" fontWeight="bold">
                    TBD
                </Text>
            </VStack>
        );
    }

    const target = parseISO(startUtc);
    const endTarget = endUtc ? parseISO(endUtc) : null;

    const formatHms = (ms: number) => {
        const totalSeconds = Math.max(Math.floor(ms / 1000), 0);
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = totalSeconds % 60;

        return `${String(hours).padStart(2, "0")}:${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`;
    };

    if (endTarget && endTarget <= now) {
        return (
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="lg" fontWeight="bold">
                    Event has been concluded
                </Text>
            </VStack>
        );
    }

    if (target <= now) {
        return (
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="2xl" fontWeight="bold">
                    Started
                </Text>
            </VStack>
        );
    }

    return (
        <VStack align="start" gap="1">
            <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                Countdown
            </Text>
            <Text fontSize="2xl" fontWeight="bold">
                {formatHms(target.getTime() - now.getTime())}
            </Text>
        </VStack>
    );
}

function EventDetailPage() {
    const { eventId } = Route.useParams();
    const { data, isLoading } = useEventsGetByIdHook({ id: Number(eventId) });
    const [showLocalTime, setShowLocalTime] = useState(true);

    if (isLoading || !data) {
        return (
            <PageWrapper py={{ base: "4", md: "6" }}>
                <Box display="grid" placeItems="center" minH="64">
                    <Loading.Rings color="primary.500" fontSize="5xl" />
                </Box>
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
            <VStack align="stretch" gap={{ base: "6", md: "8" }}>
                <VStack align="stretch" gap="4">
                    <Link
                        to="/events"
                        style={{ color: "inherit", textDecoration: "none", width: "fit-content" }}
                    >
                        <Text fontSize="sm" color="fg.muted">
                            Back to events
                        </Text>
                    </Link>
                    <Grid
                        templateColumns={{ base: "1fr", lg: "1.4fr 1fr" }}
                        gap={{ base: "4", lg: "8" }}
                    >
                        <Box
                            rounded="2xl"
                            overflow="hidden"
                            bg="bg.panel"
                            minH={{ base: "64", md: "80" }}
                            position="relative"
                        >
                            {coverUrl ? (
                                <Image
                                    src={coverUrl}
                                    alt={event.name}
                                    w="full"
                                    h="full"
                                    objectFit="cover"
                                />
                            ) : (
                                <Box w="full" h="full" bg="bg.subtle" />
                            )}
                            {logoUrl ? (
                                <Box
                                    position="absolute"
                                    inset="0"
                                    display="grid"
                                    placeItems="center"
                                >
                                    <Image
                                        src={logoUrl}
                                        alt={event.name}
                                        maxW={{ base: "40", md: "56" }}
                                        maxH={{ base: "28", md: "40" }}
                                        objectFit="contain"
                                    />
                                </Box>
                            ) : null}
                        </Box>

                        <VStack align="stretch" gap={{ base: "4", md: "5" }}>
                            <VStack align="stretch" gap="2">
                                <HStack justify="space-between" align="baseline" gap="3">
                                    <Text fontSize="sm" color="fg.muted">
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
                                    <Text color="fg.muted">{event.description}</Text>
                                ) : null}
                            </VStack>

                            <SimpleGrid columns={{ base: 1, md: 2 }} gap={{ base: "3", md: "4" }}>
                                <Box rounded="xl" bg="bg.panel" p="4">
                                    <CountdownClock
                                        startUtc={event.startTimeUtc}
                                        endUtc={event.endTimeUtc}
                                    />
                                </Box>
                                <Box rounded="xl" bg="bg.panel" p="4">
                                    <VStack align="start" gap="1">
                                        <Text
                                            fontSize="xs"
                                            color="fg.muted"
                                            textTransform="uppercase"
                                            letterSpacing="widest"
                                        >
                                            {showLocalTime ? "Your Time" : "Event Time"}
                                        </Text>
                                        <Text fontWeight="semibold">{startTimeBox}</Text>
                                        <Text fontSize="sm" color="fg.muted">
                                            {activeTimeZoneLabel}
                                        </Text>
                                    </VStack>
                                </Box>
                                <Box rounded="xl" bg="bg.panel" p="4">
                                    <VStack align="start" gap="1">
                                        <Text
                                            fontSize="xs"
                                            color="fg.muted"
                                            textTransform="uppercase"
                                            letterSpacing="widest"
                                        >
                                            Ends
                                        </Text>
                                        <Text fontWeight="semibold">{endTimeBox}</Text>
                                        <Text fontSize="sm" color="fg.muted">
                                            {activeTimeZoneLabel}
                                        </Text>
                                    </VStack>
                                </Box>
                            </SimpleGrid>
                        </VStack>
                    </Grid>
                </VStack>

                <VStack align="stretch" gap="4">
                    <Heading fontSize="xl">Related Games</Heading>
                    <HStack
                        gap={{ base: "3", md: "4" }}
                        overflowX="auto"
                        overflowY="hidden"
                        align="stretch"
                        pb="2"
                    >
                        <For
                            each={event.games}
                            fallback={
                                <EmptyState.Root
                                    description="No related games linked to this event yet."
                                    indicator={<Gamepad2Icon />}
                                />
                            }
                        >
                            {(game) => <GameCard key={game.id} game={game} />}
                        </For>
                    </HStack>
                </VStack>
            </VStack>
        </PageWrapper>
    );
}

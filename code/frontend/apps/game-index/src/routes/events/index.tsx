import { createFileRoute } from "@tanstack/react-router";
import { Suspense, useState } from "react";
import { Box, Collapse, Grid, GridItem, Heading, HStack, Image, Loading, Text, VStack } from "ui";
import { useEventsGetByYearSuspenseHook } from "@src/gen/catalogApi";
import type { EventDto } from "@src/gen/catalogApi";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { formatEventDayLabel, getEventMonthLabel } from "@src/utils/eventDateTime";
import { getYear } from "date-fns";

export const Route = createFileRoute("/events/")({
    component: RouteComponent,
});

function EventRow({ event }: { event: EventDto }) {
    const start = formatEventDayLabel(event.startTimeUtc, event.timeZone);
    const end = formatEventDayLabel(event.endTimeUtc, event.timeZone);
    const dateRange = start && end ? `${start}–${end}` : (start ?? null);
    const imageUrl = getIGDBImageUrl(event.logoImageId, "logo_med");

    return (
        <a
            href={`/events/${event.id}`}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <HStack
                justify="space-between"
                align="start"
                py="3"
                gap="3"
                borderBottomWidth="1px"
                borderColor="border.subtle"
            >
                <HStack gap="3" minW="0" flex="1">
                    <Box
                        flexShrink={0}
                        w="10"
                        h="12"
                        rounded="md"
                        overflow="hidden"
                        bg="bg.subtle"
                        borderWidth="1px"
                        borderColor="border.subtle"
                    >
                        {imageUrl ? (
                            <Image
                                src={imageUrl}
                                alt={event.name}
                                w="full"
                                h="full"
                                objectFit="cover"
                            />
                        ) : (
                            <Box display="grid" placeItems="center" w="full" h="full">
                                <Text fontSize="xs" color="fg.muted" fontWeight="semibold">
                                    {event.name.slice(0, 1)}
                                </Text>
                            </Box>
                        )}
                    </Box>
                    <Text fontWeight="medium" fontSize="sm" lineClamp={2} minW="0">
                        {event.name}
                    </Text>
                </HStack>
                {dateRange && (
                    <Text fontSize="xs" color="fg.muted" flexShrink={0}>
                        {dateRange}
                    </Text>
                )}
            </HStack>
        </a>
    );
}

type MonthGroup = { month: string; events: EventDto[] };

function groupByMonth(events: EventDto[]): MonthGroup[] {
    const userTz = Intl.DateTimeFormat().resolvedOptions().timeZone;
    const map = new Map<string, EventDto[]>();

    for (const event of events) {
        const key = getEventMonthLabel(event.startTimeUtc, event.timeZone ?? userTz);
        const group = map.get(key) ?? [];
        group.push(event);
        map.set(key, group);
    }

    return Array.from(map.entries()).map(([month, evs]) => ({ month, events: evs }));
}

function MonthCard({ group }: { group: MonthGroup }) {
    const [expanded, setExpanded] = useState(false);
    const hasMore = group.events.length > 4;

    return (
        <Box borderWidth="1px" borderColor="border.subtle" rounded="xl" p="4" bg="bg.panel">
            <Text fontWeight="semibold" fontSize="lg" mb="2">
                {group.month}
            </Text>
            {group.events.length === 0 ? (
                <VStack py="6" color="fg.muted" gap="1">
                    <Text fontSize="2xl">:(</Text>
                    <Text fontSize="sm">No events yet</Text>
                </VStack>
            ) : (
                <VStack gap="0" align="stretch">
                    {group.events.slice(0, 4).map((e) => (
                        <EventRow key={e.id} event={e} />
                    ))}
                    <Collapse open={expanded}>
                        {group.events.slice(4).map((e) => (
                            <EventRow key={e.id} event={e} />
                        ))}
                    </Collapse>
                    {hasMore && (
                        <Box
                            as="button"
                            onClick={() => setExpanded((v) => !v)}
                            mt="2"
                            py="2"
                            w="full"
                            textAlign="center"
                            fontSize="sm"
                            color="fg.muted"
                            borderWidth="1px"
                            borderColor="border.subtle"
                            rounded="lg"
                            bg="bg.subtle"
                        >
                            {expanded ? "Show less" : `Expand all`}
                        </Box>
                    )}
                </VStack>
            )}
        </Box>
    );
}

function EventsPage() {
    const year = getYear(new Date());
    const { data } = useEventsGetByYearSuspenseHook({ year });
    const groups = groupByMonth(data.events);

    return (
        <VStack align="stretch" gap="6">
            <HStack justify="space-between" align="baseline">
                <Heading fontSize="2xl" fontWeight="bold">
                    {year}
                </Heading>
                <Text fontSize="sm" color="fg.muted">
                    {data.events.length} events
                </Text>
            </HStack>
            <Grid
                templateColumns={{ base: "1fr", sm: "repeat(2, 1fr)", lg: "repeat(4, 1fr)" }}
                gap="4"
            >
                {groups.map((g) => (
                    <GridItem key={g.month}>
                        <MonthCard group={g} />
                    </GridItem>
                ))}
            </Grid>
        </VStack>
    );
}

function RouteComponent() {
    return (
        <PageWrapper py={{ base: "4", md: "6" }}>
            <Suspense
                fallback={
                    <Box display="grid" placeItems="center" minH="64">
                        <Loading.Rings color="primary.500" fontSize="5xl" />
                    </Box>
                }
            >
                <EventsPage />
            </Suspense>
        </PageWrapper>
    );
}

import { createFileRoute } from "@tanstack/react-router";
import { Suspense, useMemo, useRef, useState } from "react";
import { Grid, GridItem, Heading, HStack, SegmentedControl, Text, VStack } from "ui";
import { useEventsGetByYearSuspenseHook } from "@src/gen/catalogApi";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import { EventsPageLoadingState } from "@src/features/events/components/EventsPageLoadingState";
import { MonthCard } from "@src/features/events/components/MonthCard";
import {
    EVENT_FILTERS,
    type EventFilter,
    groupByMonth,
    isEventEnded,
} from "@src/features/events/utils/eventsList";
import { getYear } from "date-fns";

export const Route = createFileRoute("/events/")({
    component: EventsPage,
});

function EventsPage() {
    const year = getYear(new Date());
    const { data } = useEventsGetByYearSuspenseHook({ year });
    const [eventFilter, setEventFilter] = useState<EventFilter>("all");

    const nowRef = useRef(new Date());
    const now = nowRef.current;

    const processed = useMemo(() => {
        const endedMap = new Map<number, boolean>();
        for (const event of data.events) {
            endedMap.set(event.id, isEventEnded(event, now));
        }

        const finishedCount = data.events.filter((e) => endedMap.get(e.id)).length;
        const upcomingCount = data.events.length - finishedCount;

        const filteredEvents = data.events.filter((event) => {
            if (eventFilter === "all") return true;
            const ended = endedMap.get(event.id);
            return eventFilter === "finished" ? ended : !ended;
        });

        const groups = groupByMonth(filteredEvents, year);

        return { endedMap, finishedCount, upcomingCount, groups, filteredEvents };
    }, [data, eventFilter, year, now]);

    return (
        <PageWrapper maxW="9xl" py={{ base: "4", md: "6" }}>
            <VStack align="stretch" gap="6">
                <HStack justify="space-between" align="baseline" wrap="wrap" gap="3">
                    <Heading fontSize="2xl" fontWeight="bold">
                        {year}
                    </Heading>
                    <Text fontSize="sm" color="fg.muted">
                        {processed.filteredEvents.length} events
                    </Text>
                </HStack>

                <SegmentedControl.Root
                    value={eventFilter}
                    onChange={(next) => setEventFilter(next as EventFilter)}
                    size="sm"
                    colorScheme="neutral"
                    fullRounded
                    maxW={{ base: "full", md: "sm" }}
                >
                    {EVENT_FILTERS.map((filter) => {
                        const count =
                            filter.value === "all"
                                ? data.events.length
                                : filter.value === "finished"
                                  ? processed.finishedCount
                                  : processed.upcomingCount;

                        return (
                            <SegmentedControl.Item
                                key={filter.value}
                                value={filter.value}
                                colorScheme="neutral"
                            >
                                {filter.label}
                                <Text as="span" display={{ base: "none", md: "inline" }}>
                                    {` (${count})`}
                                </Text>
                            </SegmentedControl.Item>
                        );
                    })}
                </SegmentedControl.Root>

                <Grid
                    templateColumns={{ base: "1fr", md: "repeat(2, 1fr)", xl: "repeat(4, 1fr)" }}
                    gap="4"
                >
                    {processed.groups.map((g) => (
                        <GridItem key={g.month}>
                            <MonthCard group={g} now={now} />
                        </GridItem>
                    ))}
                </Grid>
            </VStack>
        </PageWrapper>
    );
}

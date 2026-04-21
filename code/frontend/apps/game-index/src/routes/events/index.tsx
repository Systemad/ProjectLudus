import { createFileRoute } from "@tanstack/react-router";
import { Suspense, useState } from "react";
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
    component: RouteComponent,
});

function EventsPage() {
    const year = getYear(new Date());
    const { data } = useEventsGetByYearSuspenseHook({ year });
    const [eventFilter, setEventFilter] = useState<EventFilter>("all");
    const now = new Date();

    const finishedCount = data.events.filter((event) => isEventEnded(event, now)).length;
    const upcomingCount = data.events.length - finishedCount;

    const filteredEvents = data.events.filter((event) => {
        if (eventFilter === "all") {
            return true;
        }

        const ended = isEventEnded(event, now);
        return eventFilter === "finished" ? ended : !ended;
    });

    const groups = groupByMonth(filteredEvents, year);

    return (
        <VStack align="stretch" gap="6">
            <HStack justify="space-between" align="baseline" wrap="wrap" gap="3">
                <Heading fontSize="2xl" fontWeight="bold">
                    {year}
                </Heading>
                <Text fontSize="sm" color="fg.muted">
                    {filteredEvents.length} events
                </Text>
            </HStack>

            <SegmentedControl.Root
                value={eventFilter}
                onChange={(next) => setEventFilter(next as EventFilter)}
                size="sm"
                colorScheme="gray"
                fullRounded
                maxW={{ base: "full", md: "sm" }}
            >
                {EVENT_FILTERS.map((filter) => {
                    const count =
                        filter.value === "all"
                            ? data.events.length
                            : filter.value === "finished"
                              ? finishedCount
                              : upcomingCount;

                    return (
                        <SegmentedControl.Item
                            key={filter.value}
                            value={filter.value}
                            colorScheme="gray"
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
                {groups.map((g) => (
                    <GridItem key={g.month}>
                        <MonthCard group={g} now={now} />
                    </GridItem>
                ))}
            </Grid>
        </VStack>
    );
}

function RouteComponent() {
    return (
        <PageWrapper py={{ base: "4", md: "6" }}>
            <Suspense fallback={<EventsPageLoadingState />}>
                <EventsPage />
            </Suspense>
        </PageWrapper>
    );
}

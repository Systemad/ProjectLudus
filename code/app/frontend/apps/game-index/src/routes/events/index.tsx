import { createFileRoute } from "@tanstack/react-router";
import { useMemo, useRef, useState } from "react";
import { Grid } from "@astryxdesign/core/Grid";
import { Text, Heading } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { SegmentedControl, SegmentedControlItem } from "@astryxdesign/core/SegmentedControl";
import { useEventsGetListSuspenseHook } from "@src/gen/catalogApi";
import { MonthCard } from "@src/features/event/components/month-card";
import {
    EVENT_FILTERS,
    type EventFilter,
    groupByMonth,
    isEventEnded,
} from "@src/features/event/utils/events-list";
import { getYear } from "date-fns";
import { presentationStyles } from "@src/app/presentation-styles";

export const Route = createFileRoute("/events/")({
    component: EventsPage,
});

function EventsPage() {
    const year = getYear(new Date());
    const { data } = useEventsGetListSuspenseHook({ params: { year } });
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
        <VStack hAlign="stretch" gap={6}>
                <HStack hAlign="between" gap={3} xstyle={presentationStyles.pageHeader}>
                    <Heading level={2}>
                        {year}
                    </Heading>
                    <Text color="secondary" xstyle={presentationStyles.metric}>
                        {processed.filteredEvents.length} events
                    </Text>
                </HStack>

                <SegmentedControl
                    value={eventFilter}
                    onChange={(next) => setEventFilter(next as EventFilter)}
                    label="View"
                    size="sm"
                >
                    {EVENT_FILTERS.map((filter) => {
                        const count =
                            filter.value === "all"
                                ? data.events.length
                                : filter.value === "finished"
                                  ? processed.finishedCount
                                  : processed.upcomingCount;

                        return (
                            <SegmentedControlItem
                                key={filter.value}
                                value={filter.value}
                                label={`${filter.label} (${count})`}
                            />
                        );
                    })}
                </SegmentedControl>

                <Grid columns={{minWidth: 280}} gap={4}>
                    {processed.groups.map((g) => (
                        <div key={g.month}>
                            <MonthCard group={g} now={now} />
                        </div>
                    ))}
                </Grid>
            </VStack>
    );
}

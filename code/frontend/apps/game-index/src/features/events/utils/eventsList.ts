import type { EventDto } from "@src/gen/catalogApi";
import { getEventMonthLabel } from "@src/utils/eventDateTime";
import { parseISO } from "date-fns";

export type EventFilter = "all" | "finished" | "upcoming";

export const EVENT_FILTERS: Array<{ value: EventFilter; label: string }> = [
    { value: "all", label: "All" },
    { value: "finished", label: "Finished" },
    { value: "upcoming", label: "Upcoming" },
];

export type MonthGroup = { month: string; events: EventDto[] };

export function isEventEnded(event: EventDto, now: Date) {
    const comparisonUtc = event.endTimeUtc ?? event.startTimeUtc;

    if (!comparisonUtc) {
        return false;
    }

    return parseISO(comparisonUtc) <= now;
}

export function groupByMonth(events: EventDto[], year: number): MonthGroup[] {
    const userTz = Intl.DateTimeFormat().resolvedOptions().timeZone;
    const monthFormatter = new Intl.DateTimeFormat("en-US", {
        month: "long",
        timeZone: userTz,
    });
    const allMonths = Array.from({ length: 12 }, (_, monthIndex) => {
        const monthDate = new Date(Date.UTC(year, monthIndex, 1));
        return monthFormatter.format(monthDate);
    });

    const map = new Map<string, EventDto[]>();

    for (const month of allMonths) {
        map.set(month, []);
    }

    for (const event of events) {
        const key = getEventMonthLabel(event.startTimeUtc, event.timeZone ?? userTz);
        const group = map.get(key) ?? [];
        group.push(event);
        map.set(key, group);
    }

    return allMonths.map((month) => ({ month, events: map.get(month) ?? [] }));
}

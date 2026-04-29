import type { EventDto } from "@src/gen/catalogApi";

export type EventFilter = "all" | "finished" | "upcoming";

export const EVENT_FILTERS: Array<{ value: EventFilter; label: string }> = [
    { value: "all", label: "All" },
    { value: "finished", label: "Finished" },
    { value: "upcoming", label: "Upcoming" },
];

export type MonthGroup = { month: string; events: EventDto[] };

const monthFormatter = new Intl.DateTimeFormat("en-US", { month: "long" });

export function isEventEnded(event: EventDto, now: Date) {
    const comparisonUtc = event.endTimeUtc ?? event.startTimeUtc;
    if (!comparisonUtc) return false;
    return comparisonUtc <= now.toISOString();
}

export function groupByMonth(events: EventDto[], year: number): MonthGroup[] {
    const userTz = Intl.DateTimeFormat().resolvedOptions().timeZone;
    const allMonths = Array.from({ length: 12 }, (_, monthIndex) => {
        const monthDate = new Date(Date.UTC(year, monthIndex, 1));
        return monthFormatter.format(monthDate);
    });

    const map = new Map<string, EventDto[]>();
    for (const month of allMonths) {
        map.set(month, []);
    }

    for (const event of events) {
        const date = event.startTimeUtc ? new Date(event.startTimeUtc) : null;
        let key = "Unknown";
        if (date) {
            const tz = event.timeZone ?? userTz;
            key = tz === userTz
                ? monthFormatter.format(date)
                : new Intl.DateTimeFormat("en-US", { month: "long", timeZone: tz }).format(date);
        }
        map.get(key)?.push(event);
    }

    return allMonths.map((month) => ({ month, events: map.get(month) ?? [] }));
}

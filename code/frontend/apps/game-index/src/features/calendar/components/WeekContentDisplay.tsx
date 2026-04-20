"use client";
import { VStack, For } from "ui";
import type { DayBucket } from "@src/features/calendar/utils/calendarWeekGrouping";
import { DayGroup } from "@src/features/calendar/components/DayGroup";

type WeekContentDisplayProps = {
    dayBuckets: DayBucket[];
};

export function WeekContentDisplay({ dayBuckets }: WeekContentDisplayProps) {
    return (
        <VStack align="stretch" gap={{ base: "lg", md: "2xl" }} py={{ base: "lg", md: "2xl" }}>
            <For each={dayBuckets}>
                {(bucket) => <DayGroup key={bucket.dayKey} bucket={bucket} />}
            </For>
        </VStack>
    );
}

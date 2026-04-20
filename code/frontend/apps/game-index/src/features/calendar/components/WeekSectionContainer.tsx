"use client";
import { VStack } from "ui";
import { useCalendarGetGamesSuspenseHook } from "@src/gen/catalogApi";
import { buildDayBuckets } from "@src/features/calendar/utils/calendarWeekGrouping";
import { WeekContentDisplay } from "@src/features/calendar/components/WeekContentDisplay";

type WeekSectionContainerProps = {
    weekStartDate: string;
};

export function WeekSectionContainer({ weekStartDate }: WeekSectionContainerProps) {
    const query = useCalendarGetGamesSuspenseHook({
        startDate: weekStartDate,
    });

    const dayBuckets = buildDayBuckets(query.data, weekStartDate);

    return (
        <VStack align="stretch" gap="0">
            <WeekContentDisplay dayBuckets={dayBuckets} />
        </VStack>
    );
}

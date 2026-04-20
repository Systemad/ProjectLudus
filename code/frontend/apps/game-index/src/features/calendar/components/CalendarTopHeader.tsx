"use client";
import { useCalendarGetGamesSuspenseHook } from "@src/gen/catalogApi";
import {
    buildDayBuckets,
    getWeekDayCounts,
} from "@src/features/calendar/utils/calendarWeekGrouping";
import { WeekOverviewHeader } from "@src/features/calendar/components/WeekOverviewHeader";
import { useCalendarContext } from "@src/features/calendar/hooks/useCalendarContext";
import { startOfWeek, format } from "date-fns";

export function CalendarTopHeader() {
    const { activeDayKey } = useCalendarContext();

    const weekStartDate = format(
        startOfWeek(new Date(activeDayKey), { weekStartsOn: 1 }),
        "yyyy-MM-dd",
    );

    const query = useCalendarGetGamesSuspenseHook({
        startDate: weekStartDate,
    });

    const dayBuckets = buildDayBuckets(query.data, weekStartDate);
    const dayCounts = getWeekDayCounts(dayBuckets);

    return <WeekOverviewHeader weekStartDate={weekStartDate} dayCounts={dayCounts} />;
}

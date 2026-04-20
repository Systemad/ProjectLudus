"use client";

import { useCallback, useEffect, useRef, useState } from "react";
import { addWeeks, format, parseISO, startOfWeek } from "date-fns";
import { dayjs } from "@src/utils/dayjs";
import { useCalendarMonthObserver } from "@src/features/calendar/hooks/useCalendarMonthObserver";
import type { CalendarContextValue } from "@src/features/calendar/hooks/useCalendarContext";

const WEEK_STARTS_ON = 1;
const SCROLL_GAP_PX = 12;

function formatWeekStart(date: Date) {
    return format(startOfWeek(date, { weekStartsOn: WEEK_STARTS_ON }), "yyyy-MM-dd");
}

function expandWeekRange(startDate: Date, endDate: Date) {
    const weeks: string[] = [];
    let cursor = startOfWeek(startDate, { weekStartsOn: WEEK_STARTS_ON });
    const boundary = startOfWeek(endDate, { weekStartsOn: WEEK_STARTS_ON });

    while (cursor.getTime() <= boundary.getTime()) {
        weeks.push(format(cursor, "yyyy-MM-dd"));
        cursor = addWeeks(cursor, 1);
    }

    return weeks;
}

type PendingScrollTarget = {
    dayKey: string;
    behavior: ScrollBehavior;
};

export function useVerticalCalendarController() {
    const [weeks, setWeeks] = useState<string[]>(() => [formatWeekStart(new Date())]);
    const stickyHeaderRef = useRef<HTMLDivElement | null>(null);
    const pendingScrollTargetRef = useRef<PendingScrollTarget | null>(null);

    const {
        activeMonthKey,
        activeDayKey,
        getSectionNode,
        registerSection: observeSection,
    } = useCalendarMonthObserver();

    const tryScrollToPendingTarget = useCallback(() => {
        const pendingTarget = pendingScrollTargetRef.current;
        if (!pendingTarget) return false;

        const targetNode = getSectionNode(pendingTarget.dayKey);
        if (!targetNode) return false;

        const stickyBottom = stickyHeaderRef.current?.getBoundingClientRect().bottom ?? 0;
        const targetTop =
            targetNode.getBoundingClientRect().top + window.scrollY - stickyBottom - SCROLL_GAP_PX;

        window.scrollTo({
            top: Math.max(targetTop, 0),
            behavior: pendingTarget.behavior,
        });

        pendingScrollTargetRef.current = null;
        return true;
    }, [getSectionNode]);

    const ensureWeekLoaded = useCallback((dayKey: string) => {
        const targetWeekStart = formatWeekStart(parseISO(dayKey));

        setWeeks((previousWeeks) => {
            if (previousWeeks.includes(targetWeekStart)) {
                return previousWeeks;
            }

            const firstWeek = parseISO(previousWeeks[0]);
            const lastWeek = parseISO(previousWeeks[previousWeeks.length - 1]);
            const targetWeek = parseISO(targetWeekStart);

            if (targetWeek.getTime() < firstWeek.getTime()) {
                return [...expandWeekRange(targetWeek, addWeeks(firstWeek, -1)), ...previousWeeks];
            }

            if (targetWeek.getTime() > lastWeek.getTime()) {
                return [...previousWeeks, ...expandWeekRange(addWeeks(lastWeek, 1), targetWeek)];
            }

            return previousWeeks;
        });
    }, []);

    const onJumpToDay = useCallback(
        (dayKey: string, behavior: ScrollBehavior = "smooth") => {
            pendingScrollTargetRef.current = { dayKey, behavior };
            ensureWeekLoaded(dayKey);

            window.requestAnimationFrame(() => {
                tryScrollToPendingTarget();
            });
        },
        [ensureWeekLoaded, tryScrollToPendingTarget],
    );

    const registerSection = useCallback(
        (dayKey: string, node: HTMLElement | null) => {
            observeSection(dayKey, node);

            if (node && pendingScrollTargetRef.current?.dayKey === dayKey) {
                window.requestAnimationFrame(() => {
                    tryScrollToPendingTarget();
                });
            }
        },
        [observeSection, tryScrollToPendingTarget],
    );

    const onAddNextWeek = useCallback(() => {
        setWeeks((previousWeeks) => {
            const nextWeek = format(
                addWeeks(parseISO(previousWeeks[previousWeeks.length - 1]), 1),
                "yyyy-MM-dd",
            );

            return previousWeeks.includes(nextWeek) ? previousWeeks : [...previousWeeks, nextWeek];
        });
    }, []);

    const onJumpToToday = useCallback(() => {
        onJumpToDay(dayjs.utc().format("YYYY-MM-DD"));
    }, [onJumpToDay]);

    const onChangeWeek = useCallback(
        (direction: -1 | 1) => {
            const targetWeek = addWeeks(
                startOfWeek(parseISO(activeDayKey), { weekStartsOn: WEEK_STARTS_ON }),
                direction,
            );
            onJumpToDay(format(targetWeek, "yyyy-MM-dd"));
        },
        [activeDayKey, onJumpToDay],
    );

    useEffect(() => {
        tryScrollToPendingTarget();
    }, [tryScrollToPendingTarget, weeks]);

    const contextValue: CalendarContextValue = {
        activeMonthKey,
        activeDayKey,
        onJumpToDay,
        onJumpToToday,
        onChangeWeek,
        registerSection,
    };

    return {
        contextValue,
        onAddNextWeek,
        stickyHeaderRef,
        weeks,
    };
}

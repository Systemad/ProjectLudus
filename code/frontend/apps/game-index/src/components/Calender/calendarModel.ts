import { dayjs } from "@src/utils/dayjs";
import type { ReleaseDatePreset } from "@src/utils/dateUtils";
import type { CalendarDayCell, CalendarModel } from "./types";

function buildDisplayCells(preset: ReleaseDatePreset, anchorStr: string): CalendarDayCell[] {
    const anchor = dayjs.utc(anchorStr);
    if (preset === "week") {
        const today = anchor.startOf("day");
        return Array.from({ length: 7 }, (_, i) => {
            const day = today.add(i, "day");
            return { key: day.format("YYYY-MM-DD"), dayLabel: day.format("ddd D") };
        });
    }

    const start = anchor.startOf("month");
    const end = anchor.endOf("month");
    const cells: CalendarDayCell[] = [];

    for (let day = start; !day.isAfter(end, "day"); day = day.add(1, "day")) {
        cells.push({ key: day.format("YYYY-MM-DD"), dayLabel: day.format("ddd D") });
    }

    return cells;
}

export function getCalendarModel(preset: ReleaseDatePreset, anchorStr: string): CalendarModel {
    const anchor = dayjs.utc(anchorStr);
    const dayCells = buildDisplayCells(preset, anchorStr);

    if (preset === "week") {
        const start = anchor.startOf("day");
        const end = start.add(6, "day").endOf("day");
        return {
            from: start.unix(),
            to: end.unix(),
            periodLabel: `${start.format("MMM D")} - ${end.format("MMM D")}`,
            dayCells,
        };
    }

    if (preset === "month") {
        const start = anchor.startOf("month");
        const end = anchor.endOf("month");
        return {
            from: start.unix(),
            to: end.unix(),
            periodLabel: anchor.format("MMMM YYYY"),
            dayCells,
        };
    }

    const start = anchor.startOf("day");
    const end = start.add(6, "day").endOf("day");
    return {
        from: start.unix(),
        to: end.unix(),
        periodLabel: `${start.format("MMM D")} - ${end.format("MMM D")}`,
        dayCells,
    };
}

export function alignAnchorToPreset(anchorStr: string, preset: ReleaseDatePreset): string {
    const anchor = dayjs.utc(anchorStr);
    if (preset === "month") return anchor.startOf("month").format("YYYY-MM-DD");
    return anchor.startOf("day").format("YYYY-MM-DD");
}

export function shiftAnchor(
    anchorStr: string,
    preset: ReleaseDatePreset,
    direction: -1 | 1,
): string {
    const anchor = dayjs.utc(anchorStr);
    if (preset === "month")
        return anchor.add(direction, "month").startOf("month").format("YYYY-MM-DD");
    return anchor.add(direction, "week").startOf("day").format("YYYY-MM-DD");
}

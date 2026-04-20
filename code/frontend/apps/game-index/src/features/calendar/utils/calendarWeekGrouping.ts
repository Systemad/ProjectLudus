import type { CalendarGetGamesQueryResponse, GamesSearchDto } from "@src/gen/catalogApi";
import { dayjs } from "@src/utils/dayjs";
import { formatHeadingLabel } from "@src/features/calendar/utils/date.utils";
import { addDays, startOfWeek } from "date-fns";

export type DayBucket = {
    dayKey: string;
    headingLabel: string;
    games: GamesSearchDto[];
};

export function buildDayBuckets(
    weekData: CalendarGetGamesQueryResponse,
    weekStartDate?: string,
): DayBucket[] {
    const map = new Map<string, DayBucket>();
    const seenGameIdsByDay = new Map<string, Set<string>>();

    if (weekData.games && weekData.games.length > 0) {
        for (const game of weekData.games) {
            if (!game.firstReleaseDate) continue;

            const dayKey = game.firstReleaseDate;
            const gameId = String(game.id);
            const seenGameIds = seenGameIdsByDay.get(dayKey) ?? new Set<string>();

            if (seenGameIds.has(gameId)) {
                continue;
            }

            seenGameIds.add(gameId);
            seenGameIdsByDay.set(dayKey, seenGameIds);

            const bucket = map.get(dayKey);

            if (bucket) {
                bucket.games.push(game);
                continue;
            }

            map.set(dayKey, {
                dayKey,
                headingLabel: formatHeadingLabel(dayjs.utc(dayKey).unix()),
                games: [game],
            });
        }
    }

    const weekStart = weekStartDate
        ? new Date(weekStartDate)
        : startOfWeek(new Date(), { weekStartsOn: 1 });
    const allDayBuckets: DayBucket[] = [];

    for (let i = 0; i < 7; i++) {
        const date = addDays(weekStart, i);
        const dayKey = dayjs.utc(date).format("YYYY-MM-DD");
        const existingBucket = map.get(dayKey);

        if (existingBucket) {
            allDayBuckets.push(existingBucket);
        } else {
            allDayBuckets.push({
                dayKey,
                headingLabel: formatHeadingLabel(dayjs.utc(dayKey).unix()),
                games: [],
            });
        }
    }

    return allDayBuckets;
}

export function getWeekDayCounts(dayBuckets: DayBucket[]): Record<string, number> {
    return dayBuckets.reduce(
        (acc, bucket) => {
            acc[bucket.dayKey] = bucket.games.length;
            return acc;
        },
        {} as Record<string, number>,
    );
}

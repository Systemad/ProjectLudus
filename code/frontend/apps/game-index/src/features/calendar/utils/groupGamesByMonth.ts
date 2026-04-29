import type { GameDto } from "@src/gen/catalogApi";
import { getReleaseMonth, getReleaseMonthKey } from "@src/utils/dateUtils";

export function groupGamesByMonth(games: GameDto[]) {
    const map = new Map<string, { month: string; games: GameDto[] }>();

    for (const game of games) {
        const monthKey = getReleaseMonthKey(game.firstReleaseDate);
        if (!monthKey) continue;

        const group = map.get(monthKey) ?? {
            month: getReleaseMonth(game.firstReleaseDate)!,
            games: [],
        };

        group.games.push(game);
        map.set(monthKey, group);
    }

    return Array.from(map.entries())
        .sort(([leftKey], [rightKey]) => leftKey.localeCompare(rightKey))
        .map(([, group]) => ({
            month: group.month,
            games: group.games.sort((left, right) =>
                left.firstReleaseDate!.localeCompare(right.firstReleaseDate!),
            ),
        }));
}

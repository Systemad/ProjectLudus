import { dayjs } from "@src/utils/dayjs";
import type { GameSearchHit } from "@src/Typesense/utils/hits";

export function bucketHitsByDay(items: GameSearchHit[]) {
    const map = new Map<string, GameSearchHit[]>();

    for (const hit of items) {
        const epoch = hit.first_release_date;
        if (typeof epoch !== "number") continue;
        const key = dayjs.unix(epoch).utc().format("YYYY-MM-DD");
        const existing = map.get(key);
        if (existing) existing.push(hit);
        else map.set(key, [hit]);
    }

    return map;
}

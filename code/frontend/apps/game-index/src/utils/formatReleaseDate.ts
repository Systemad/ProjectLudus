import { formatEpochDate } from "./dateUtils";

export function formatReleaseDate(firstReleaseDate?: number | null) {
    return formatEpochDate(firstReleaseDate);
}

import type { GameSearchHit } from "./hits";
import { formatEpochDate, parseEpochSeconds } from "@src/utils/dateUtils";

export function getReleaseYear(hit: GameSearchHit) {
    if (typeof hit.release_year === "number") return String(hit.release_year);

    const epoch = parseEpochSeconds(hit.first_release_date);
    if (typeof epoch === "number") {
        return String(new Date(epoch * 1000).getUTCFullYear());
    }

    return "Unknown year";
}

export function getDevelopersLabel(developers?: string[] | string) {
    if (!developers) return "Unknown developer";
    if (Array.isArray(developers)) {
        if (developers.length === 0) return "Unknown developer";
        return developers.slice(0, 2).join(", ");
    }
    return developers;
}

export function getReleaseEpoch(hit: GameSearchHit): number | null {
    return parseEpochSeconds(hit.first_release_date);
}

export function getReleaseDateLabel(hit: GameSearchHit): string {
    return formatEpochDate(getReleaseEpoch(hit), {
        year: "numeric",
        month: "short",
        day: "2-digit",
    });
}

export function getLimitedPlatforms(platforms?: string[] | string, limit = 3): string[] {
    if (!platforms) return [];
    if (Array.isArray(platforms)) {
        return platforms.slice(0, limit);
    }

    return [platforms];
}

export function getCompanyStatusLabel(status?: string) {
    return status && status.trim().length > 0 ? status : "Unknown";
}

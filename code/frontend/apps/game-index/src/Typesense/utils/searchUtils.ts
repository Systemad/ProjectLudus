import type { GameSearchHit } from "./hits";

export function getReleaseYear(hit: GameSearchHit) {
    if (typeof hit.release_year === "number") return String(hit.release_year);

    if (typeof hit.first_release_date === "number") {
        return String(new Date(hit.first_release_date * 1000).getUTCFullYear());
    }

    if (typeof hit.first_release_date === "string") {
        const parsed = new Date(hit.first_release_date);
        if (!Number.isNaN(parsed.getTime())) {
            return String(parsed.getUTCFullYear());
        }
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

export function getCompanyStatusLabel(status?: string) {
    return status && status.trim().length > 0 ? status : "Unknown";
}

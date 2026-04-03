export type GameSearchHit = {
    id: string;
    name?: string;
    cover_url?: string;
    aggregated_rating?: number;
    aggregated_rating_count?: number;
    developers?: string[] | string;
    release_year?: number;
    first_release_date?: number | string;
};

export function getCoverImageUrl(coverUrl?: string) {
    if (!coverUrl) return "";
    if (coverUrl.startsWith("http")) return coverUrl;
    return `https://images.igdb.com/igdb/image/upload/t_cover_big/${coverUrl}.jpg`;
}

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

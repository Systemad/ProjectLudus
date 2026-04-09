export function formatReleaseDate(firstReleaseDate?: number | null) {
    if (typeof firstReleaseDate !== "number") {
        return "Release date TBA";
    }

    return new Intl.DateTimeFormat("en-US", {
        month: "short",
        day: "numeric",
        year: "numeric",
        timeZone: "UTC",
    }).format(new Date(firstReleaseDate * 1000));
}

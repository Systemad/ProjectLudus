import { format, isValid, parseISO } from "date-fns";

export function isTbaReleaseDate(firstReleaseDate?: string | null) {
    return firstReleaseDate == null;
}

export function formatReleaseDateLabel(firstReleaseDate?: string | null) {
    if (!firstReleaseDate) {
        return "TBA";
    }

    const date = parseISO(firstReleaseDate);
    if (!isValid(date)) {
        return "TBA";
    }

    return format(date, "yyyy-MM-dd");
}

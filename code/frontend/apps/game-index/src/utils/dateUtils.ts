export type ReleaseDatePreset = "week" | "month" | "year";

const DAY_SECONDS = 24 * 60 * 60;

export function getCurrentEpochSeconds() {
    return Math.floor(Date.now() / 1000);
}

export function parseEpochSeconds(value?: number | string | null): number | null {
    if (typeof value === "number") {
        return value;
    }

    if (typeof value === "string") {
        const parsed = new Date(value);
        if (!Number.isNaN(parsed.getTime())) {
            return Math.floor(parsed.getTime() / 1000);
        }
    }

    return null;
}

export function formatEpochDate(
    epochSeconds?: number | null,
    options?: Intl.DateTimeFormatOptions,
) {
    if (typeof epochSeconds !== "number") {
        return "Release date TBA";
    }

    return new Intl.DateTimeFormat("en-US", {
        month: "short",
        day: "numeric",
        year: "numeric",
        timeZone: "UTC",
        ...options,
    }).format(new Date(epochSeconds * 1000));
}

export function getPresetDurationSeconds(preset: ReleaseDatePreset) {
    if (preset === "week") return 7 * DAY_SECONDS;
    if (preset === "month") return 30 * DAY_SECONDS;
    return 365 * DAY_SECONDS;
}

export function getUpcomingEpochRange(
    preset: ReleaseDatePreset,
    fromEpoch = getCurrentEpochSeconds(),
) {
    return {
        from: fromEpoch,
        to: fromEpoch + getPresetDurationSeconds(preset),
    };
}

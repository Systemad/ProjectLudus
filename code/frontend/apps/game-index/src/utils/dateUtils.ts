const parseValidIsoDate = (value?: string | null) => {
    if (!value) return null;
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
};

const isPlaceholderDate = (value?: string | null) => {
    const date = parseValidIsoDate(value);
    return !date || (date.getMonth() === 11 && date.getDate() === 31);
};

export const isTbaReleaseDate = (value?: string | null) => isPlaceholderDate(value);
export const formatIsoDateTime = (value?: string | null, options?: Intl.DateTimeFormatOptions) => {
    const date = parseValidIsoDate(value);
    return date
        ? new Intl.DateTimeFormat(undefined, {
              weekday: "long",
              month: "short",
              day: "numeric",
              year: "numeric",
              hour: "2-digit",
              minute: "2-digit",
              ...options,
          }).format(date)
        : null;
};
export const formatReleaseDateLabel = (value?: string | null) => {
    const date = parseValidIsoDate(value);
    return date
        ? new Intl.DateTimeFormat("en-US", {
              year: "numeric",
              month: "2-digit",
              day: "2-digit",
          }).format(date)
        : "TBA";
};
export const getReleaseMonth = (value?: string | null) => {
    const date = parseValidIsoDate(value);
    return date ? new Intl.DateTimeFormat("en-US", { month: "long" }).format(date) : null;
};
export const getReleaseMonthKey = (value?: string | null) => {
    const date = parseValidIsoDate(value);
    return date ? date.toISOString().slice(0, 7) : null;
};
export const parseEpochSeconds = (value?: number | string | null): number | null => {
    if (typeof value === "number") return value;
    if (typeof value === "string") {
        const parsed = new Date(value);
        if (!Number.isNaN(parsed.getTime())) return Math.floor(parsed.getTime() / 1000);
    }
    return null;
};

export const formatEpochDate = (
    epochSeconds?: number | null,
    options?: Intl.DateTimeFormatOptions,
) =>
    typeof epochSeconds === "number"
        ? new Intl.DateTimeFormat("en-US", {
              month: "short",
              day: "numeric",
              year: "numeric",
              timeZone: "UTC",
              ...options,
          }).format(new Date(epochSeconds * 1000))
        : "Release date TBA";

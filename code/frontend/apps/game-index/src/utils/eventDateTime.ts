import { format, parseISO } from "date-fns";

type DateTimeOptions = Intl.DateTimeFormatOptions;

function resolveTimeZone(timeZone?: string | null) {
    return timeZone ?? Intl.DateTimeFormat().resolvedOptions().timeZone;
}

function formatInTimeZone(
    utc: string | null | undefined,
    timeZone: string | null | undefined,
    options: DateTimeOptions,
) {
    if (!utc) return null;

    return new Intl.DateTimeFormat("en-US", {
        ...options,
        timeZone: resolveTimeZone(timeZone),
    }).format(new Date(utc));
}

export function getClientTimeZone() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}

export function formatClientLocalDateTime(utc: string | null | undefined) {
    if (!utc) return "TBD";
    return format(parseISO(utc), "EEEE, MMM d, yyyy 'at' HH:mm");
}

export function formatEventDayLabel(
    utc: string | null | undefined,
    timeZone: string | null | undefined,
) {
    return formatInTimeZone(utc, timeZone, {
        month: "short",
        day: "numeric",
    });
}

export function formatEventDateTime(
    utc: string | null | undefined,
    timeZone: string | null | undefined,
) {
    return (
        formatInTimeZone(utc, timeZone, {
            weekday: "long",
            month: "short",
            day: "numeric",
            year: "numeric",
            hour: "2-digit",
            minute: "2-digit",
            timeZoneName: "short",
        }) ?? "TBD"
    );
}

export function getEventMonthLabel(
    utc: string | null | undefined,
    timeZone: string | null | undefined,
) {
    return (
        formatInTimeZone(utc, timeZone, {
            month: "long",
        }) ?? "Unknown"
    );
}

export type SteamChartRange = "24h" | "7d" | "30d";

export function formatPlayerCount(value: number | null | undefined) {
  if (value === null || value === undefined) return "—";
  if (value >= 1_000_000) return `${(value / 1_000_000).toFixed(1)}M`;
  if (value >= 1_000) return `${(value / 1_000).toFixed(1)}K`;
  return Math.round(value).toLocaleString();
}

export function formatTimestamp(value: number, range: SteamChartRange) {
  const date = new Date(value);
  const dateLabel = date
    .toLocaleDateString("en-GB", { day: "numeric", month: "short", weekday: "short" })
    .replace(",", "");

  if (range === "30d") {
    return dateLabel;
  }

  const timeLabel = date.toLocaleTimeString(undefined, {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false,
  });

  return `${dateLabel} · ${timeLabel}`;
}

type TimestampParts = {
  day: string;
  hour: string;
  minute: string;
  month: string;
  weekday: string;
};

function getTimestampParts(value: number, timeZone: string): TimestampParts {
  const parts = new Intl.DateTimeFormat("en-GB", {
    day: "numeric",
    hour: "2-digit",
    hour12: false,
    minute: "2-digit",
    month: "long",
    timeZone,
    weekday: "long",
  }).formatToParts(new Date(value));
  const get = (type: keyof TimestampParts) => parts.find((part) => part.type === type)?.value ?? "";

  return {
    day: get("day"),
    hour: get("hour"),
    minute: get("minute"),
    month: get("month"),
    weekday: get("weekday"),
  };
}

function formatDatePart(parts: TimestampParts) {
  return `${parts.weekday} ${parts.day} ${parts.month}`;
}

export function formatTooltipTimestamp(value: number) {
  const utc = getTimestampParts(value, "UTC");
  const localTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
  const local = getTimestampParts(value, localTimeZone);
  const utcDate = formatDatePart(utc);
  const localDate = formatDatePart(local);
  const utcTime = `${utc.hour}:${utc.minute}`;
  const localTime = `${local.hour}:${local.minute}`;

  if (localTimeZone === "UTC" || localTimeZone === "Etc/UTC") {
    return `${utcDate}, ${utcTime} UTC`;
  }

  const localLabel =
    localDate === utcDate ? `${localTime} local` : `${localDate}, ${localTime} local`;
  return `${utcDate}, ${utcTime} UTC (${localLabel})`;
}

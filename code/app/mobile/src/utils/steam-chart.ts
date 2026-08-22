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

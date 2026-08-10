export type SteamChartRange = "24h" | "7d" | "30d";

export function formatPlayerCount(value: number) {
  if (value >= 1_000_000) return `${(value / 1_000_000).toFixed(1)}M`;
  if (value >= 1_000) return `${(value / 1_000).toFixed(1)}K`;
  return Math.round(value).toLocaleString();
}

export function formatTimestamp(value: number, range: SteamChartRange) {
  const date = new Date(value);
  return range === "30d"
    ? date.toLocaleDateString(undefined, { day: "numeric", month: "short" })
    : date.toLocaleDateString(undefined, {
        day: "numeric",
        hour: "numeric",
        month: "short",
      });
}

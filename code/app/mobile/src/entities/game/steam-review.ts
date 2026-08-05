import type { SteamReviewData } from "@/gen/types/SteamReviewData";

export type SteamReviewTone = "muted" | "positive" | "mixed" | "negative";

export function getSteamReviewPercentage(
  review: SteamReviewData | null | undefined,
): number | undefined {
  const total = review?.totalReviews ?? 0;
  const positive = review?.positive ?? 0;

  if (total === 0) {
    return undefined;
  }

  return Math.round((positive / total) * 100);
}

export function formatSteamReviewRating(review: SteamReviewData | null | undefined): string {
  const percentage = getSteamReviewPercentage(review);
  return percentage === undefined ? "N/A" : `${percentage}%`;
}

export function getSteamReviewTone(review: SteamReviewData | null | undefined): SteamReviewTone {
  const percentage = getSteamReviewPercentage(review);

  if (percentage === undefined) return "muted";
  if (percentage >= 80) return "positive";
  if (percentage >= 60) return "mixed";
  return "negative";
}

export function getSteamReviewEmoji(review: SteamReviewData | null | undefined): string {
  const tone = getSteamReviewTone(review);

  if (tone === "positive") return "😎";
  if (tone === "mixed") return "👍";
  if (tone === "negative") return "😡";
  return "—";
}

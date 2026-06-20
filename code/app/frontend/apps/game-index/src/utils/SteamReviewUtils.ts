import type { SteamReviewData } from "@src/gen/catalogApi";

export function steamReviewColor(review: SteamReviewData | null | undefined): string {
    const total = review?.totalReviews ?? 0;
    const positive = review?.positive ?? 0;
    if (total === 0) return "fg.muted";
    const pct = (positive / total) * 100;
    if (pct >= 80) return "emerald.200";
    if (pct >= 60) return "yellow.500";
    return "red.500";
}

export function steamReviewRating(review: SteamReviewData | null | undefined): string {
    const total = review?.totalReviews ?? 0;
    const positive = review?.positive ?? 0;
    if (total === 0) return "N/A";
    return `${Math.round((positive / total) * 100)}%`;
}

export function steamReviewEmoji(review: SteamReviewData | null | undefined): string {
    const total = review?.totalReviews ?? 0;
    const positive = review?.positive ?? 0;
    if (total === 0) return "\u{2014}";
    const pct = (positive / total) * 100;
    if (pct >= 80) return "\u{1F60E}";
    if (pct >= 60) return "\u{1F44D}";
    return "\u{1F621}";
}

import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";

import { getGameCardImage } from "./game-image";
import { formatSteamReviewRating, getSteamReviewEmoji } from "./steam-review";

export type GameCardData = {
  title: string;
  metadata?: string;
  imageUrl?: string;
};

export function getGameCardData(game: GameBrowseDto): GameCardData {
  const primaryGenre = game.gameFeatures.genres[0]?.name ?? "Game";
  const reviewRating = formatSteamReviewRating(game.steam?.review);
  const review =
    reviewRating === "N/A"
      ? undefined
      : `${getSteamReviewEmoji(game.steam?.review)} ${reviewRating}`;
  const metadata = [game.firstReleaseDate?.slice(0, 4) ?? "TBA", primaryGenre, review]
    .filter((value): value is string => value !== undefined)
    .join(" · ");

  return {
    title: game.name,
    metadata,
    imageUrl: getGameCardImage(game),
  };
}

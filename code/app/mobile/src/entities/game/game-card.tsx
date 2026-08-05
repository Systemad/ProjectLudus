import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";
import { getGameCardImage } from "@/entities/game/game-image";
import { formatSteamReviewRating, getSteamReviewEmoji } from "@/entities/game/steam-review";
import { GameCoverCard, type GameCoverCardVariant } from "./game-cover-card";

export type GameCardVariant = GameCoverCardVariant;

type GameCardProps = {
  game: GameBrowseDto;
  variant: GameCardVariant;
  href: Href;
};

export function GameCard({ game, variant, href }: GameCardProps) {
  const image = getGameCardImage(game);
  const primaryGenre = game.gameFeatures.genres[0]?.name ?? "Game";
  const reviewRating = formatSteamReviewRating(game.review);
  const review =
    reviewRating === "N/A" ? undefined : `${getSteamReviewEmoji(game.review)} ${reviewRating}`;
  const metadata = [game.firstReleaseDate?.slice(0, 4) ?? "TBA", primaryGenre, review]
    .filter((value): value is string => value !== undefined)
    .join(" · ");
  return (
    <GameCoverCard
      title={game.name}
      metadata={metadata}
      imageUrl={image}
      href={href}
      variant={variant}
    />
  );
}

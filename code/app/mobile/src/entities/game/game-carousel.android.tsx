import { Host } from "@expo/ui";
import { LazyRow } from "@expo/ui/jetpack-compose";
import { fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";
import { useWindowDimensions } from "react-native";

import { GAME_RAIL_GAP, GAME_RAIL_VISIBLE_CARD_COUNT, getGameRailCardWidth } from "@/config/layout";
import { GameCard, getGameCardData, type GameCardVariant } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";

type GameCarouselProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
  variant?: Extract<GameCardVariant, "rail" | "cover">;
  visibleCardCount?: number;
};

export function GameCarousel({
  games,
  getHref,
  variant = "rail",
  visibleCardCount = GAME_RAIL_VISIBLE_CARD_COUNT,
}: GameCarouselProps) {
  const { width } = useWindowDimensions();
  const cardWidth = getGameRailCardWidth(width, visibleCardCount);

  return (
    <Host matchContents={{ vertical: true }} style={{ width: "100%" }}>
      <LazyRow
        modifiers={[fillMaxWidth()]}
        horizontalArrangement={{ spacedBy: GAME_RAIL_GAP }}
        verticalAlignment="top"
      >
        {games.map((game) => (
          <GameCard
            key={String(game.id)}
            {...getGameCardData(game)}
            cardWidth={cardWidth}
            variant={variant}
            href={getHref(game)}
          />
        ))}
      </LazyRow>
    </Host>
  );
}

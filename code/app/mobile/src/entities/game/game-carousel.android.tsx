import { Host, LazyRow } from "@expo/ui/jetpack-compose";
import { fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";
import { useWindowDimensions } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { GameCard, getGameCardData, type GameCardVariant } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";

const CAROUSEL_GAP = 10;
const DEFAULT_VISIBLE_CARD_COUNT = 3.2;

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
  visibleCardCount = DEFAULT_VISIBLE_CARD_COUNT,
}: GameCarouselProps) {
  const { width } = useWindowDimensions();
  const availableWidth = width - PAGE_GUTTER * 2;
  const cardWidth = Math.floor(
    (availableWidth - CAROUSEL_GAP * (visibleCardCount - 1)) / visibleCardCount,
  );

  return (
    <Host style={{ width: "100%" }}>
      <LazyRow
        modifiers={[fillMaxWidth()]}
        horizontalArrangement={{ spacedBy: CAROUSEL_GAP }}
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

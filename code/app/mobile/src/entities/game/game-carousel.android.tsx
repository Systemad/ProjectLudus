import { Host, Row, ScrollView } from "@expo/ui";

import { GAME_RAIL_CARD_WIDTH, GAME_RAIL_GAP } from "@/config/layout";
import { GameCard, getGameCardData, type GameCardVariant } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";

type GameCarouselProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
  variant?: Extract<GameCardVariant, "rail" | "cover">;
};

export function GameCarousel({ games, getHref, variant = "rail" }: GameCarouselProps) {
  return (
    <Host matchContents={{ vertical: true }} style={{ width: "100%" }}>
      <ScrollView direction="horizontal" showsIndicators={false} style={{ width: "100%" }}>
        <Row spacing={GAME_RAIL_GAP}>
          {games.map((game) => (
            <GameCard
              key={String(game.id)}
              {...getGameCardData(game)}
              cardWidth={GAME_RAIL_CARD_WIDTH}
              variant={variant}
              href={getHref(game)}
            />
          ))}
        </Row>
      </ScrollView>
    </Host>
  );
}

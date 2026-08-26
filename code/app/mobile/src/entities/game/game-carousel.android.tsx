import { Host } from "@expo/ui";
import { ScrollView, StyleSheet, View } from "react-native";

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
    <ScrollView
      horizontal
      showsHorizontalScrollIndicator={false}
      contentContainerStyle={styles.content}
    >
      {games.map((game) => (
        <View key={String(game.id)} style={styles.card}>
          <Host matchContents={{ vertical: true }} style={styles.host}>
            <GameCard
              {...getGameCardData(game)}
              cardWidth={GAME_RAIL_CARD_WIDTH}
              variant={variant}
              href={getHref(game)}
            />
          </Host>
        </View>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  content: {
    gap: GAME_RAIL_GAP,
  },
  card: {
    width: GAME_RAIL_CARD_WIDTH,
  },
  host: {
    width: GAME_RAIL_CARD_WIDTH,
  },
});

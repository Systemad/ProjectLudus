import { FlatList, StyleSheet, useWindowDimensions, View } from "react-native";

import { GameCard, type GameCardVariant } from "@/entities/game/game-card";
import { PAGE_GUTTER } from "@/config/layout";
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
    <FlatList
      horizontal
      data={games}
      keyExtractor={(game) => String(game.id)}
      renderItem={({ item }) => (
        <View style={{ width: cardWidth }}>
          <GameCard game={item} variant={variant} href={getHref(item)} />
        </View>
      )}
      showsHorizontalScrollIndicator={false}
      contentContainerStyle={styles.content}
    />
  );
}

const styles = StyleSheet.create({
  content: {
    gap: CAROUSEL_GAP,
  },
});

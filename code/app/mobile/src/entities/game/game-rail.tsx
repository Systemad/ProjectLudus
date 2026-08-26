import { FlatList, StyleSheet, View } from "react-native";

import { GAME_RAIL_CARD_WIDTH, GAME_RAIL_GAP } from "@/config/layout";

import { GameCard, type GameCardItem } from "./game-card";

export function GameRail({ items }: { items: GameCardItem[] }) {
  return (
    <FlatList
      horizontal
      data={items}
      keyExtractor={(item) => item.id}
      showsHorizontalScrollIndicator={false}
      contentContainerStyle={styles.content}
      renderItem={({ item }) => (
        <View style={styles.card}>
          <GameCard {...item} />
        </View>
      )}
    />
  );
}

const styles = StyleSheet.create({
  content: {
    gap: GAME_RAIL_GAP,
  },
  card: {
    width: GAME_RAIL_CARD_WIDTH,
  },
});

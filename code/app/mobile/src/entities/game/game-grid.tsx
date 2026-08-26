import { FlatList, StyleSheet, useWindowDimensions, View } from "react-native";

import { GRID_COLUMN_GAP, GRID_PAGE_PADDING, GRID_ROW_GAP } from "@/config/layout";

import { GameCard, type GameCardItem } from "./game-card";

type GameGridProps = {
  items: GameCardItem[];
  bottomInset: number;
  pagePadding?: number;
  topPadding?: number;
  bottomPadding?: number;
};

export function GameGrid({
  items,
  bottomInset,
  pagePadding = GRID_PAGE_PADDING,
  topPadding = GRID_PAGE_PADDING,
  bottomPadding = bottomInset,
}: GameGridProps) {
  const { width } = useWindowDimensions();
  const cardWidth = Math.floor((width - pagePadding * 2 - GRID_COLUMN_GAP) / 2);

  return (
    <FlatList
      data={items}
      numColumns={2}
      keyExtractor={(item) => item.id}
      style={styles.list}
      contentInsetAdjustmentBehavior="automatic"
      columnWrapperStyle={styles.row}
      contentContainerStyle={[
        styles.content,
        {
          paddingHorizontal: pagePadding,
          paddingTop: topPadding,
          paddingBottom: bottomPadding,
        },
      ]}
      renderItem={({ item }) => (
        <View style={{ width: cardWidth }}>
          <GameCard {...item} />
        </View>
      )}
    />
  );
}

const styles = StyleSheet.create({
  list: {
    flex: 1,
  },
  content: {
    gap: GRID_ROW_GAP,
  },
  row: {
    gap: GRID_COLUMN_GAP,
  },
});

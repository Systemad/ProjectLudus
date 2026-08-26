import { FlatList, StyleSheet, View } from "react-native";

import { GRID_COLUMN_GAP, GRID_PAGE_PADDING, GRID_ROW_GAP } from "@/config/layout";
import { GameCard, getGameCardData } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { useContentBottomInset } from "@/shared/ui/insets";
import type { Href } from "expo-router";

type GameGridProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
};

export function GameGrid({ games, getHref, isLoading, isError, onRetry }: GameGridProps) {
  const bottomInset = useContentBottomInset(120);

  return (
    <ContentState
      fullScreen
      status={getContentStateStatus(isLoading, isError, games.length === 0)}
      loading={{ label: "Loading games…" }}
      error={{ onRetry, title: "Couldn’t load this collection." }}
      empty={{ title: "No games found", message: "There are no games in this collection yet." }}
    >
      <FlatList
        data={games}
        numColumns={2}
        keyExtractor={(game) => String(game.id)}
        contentInsetAdjustmentBehavior="automatic"
        columnWrapperStyle={styles.row}
        contentContainerStyle={[styles.content, { paddingBottom: bottomInset }]}
        renderItem={({ item }) => (
          <View style={styles.cell}>
            <GameCard {...getGameCardData(item)} variant="grid" href={getHref(item)} />
          </View>
        )}
      />
    </ContentState>
  );
}

const styles = StyleSheet.create({
  content: {
    gap: GRID_ROW_GAP,
    paddingHorizontal: GRID_PAGE_PADDING,
    paddingTop: GRID_PAGE_PADDING,
  },
  row: {
    gap: GRID_COLUMN_GAP,
  },
  cell: {
    flex: 1,
    minWidth: 0,
  },
});

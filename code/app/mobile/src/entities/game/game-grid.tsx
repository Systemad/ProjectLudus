import { FlatList, StyleSheet, View } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";
import { InlineState } from "@/shared/ui/inline-state";
import { EmptyState, ErrorState, LoadingState } from "@/shared/ui/screen-state";

type GameGridProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
  isLoading: boolean;
  isError: boolean;
  isFetchingNextPage: boolean;
  isFetchNextPageError: boolean;
  hasNextPage: boolean;
  onLoadMore: () => void;
  onRetry: () => void;
  onRetryNextPage: () => void;
};

export function GameGrid({
  games,
  getHref,
  isLoading,
  isError,
  isFetchingNextPage,
  isFetchNextPageError,
  hasNextPage,
  onLoadMore,
  onRetry,
  onRetryNextPage,
}: GameGridProps) {
  if (isLoading) return <LoadingState label="Loading games…" />;
  if (isError && games.length === 0) return <ErrorState onRetry={onRetry} />;
  if (games.length === 0) {
    return (
      <EmptyState title="No games found" message="There are no games in this collection yet." />
    );
  }

  return (
    <FlatList
      data={games}
      numColumns={2}
      keyExtractor={(game) => String(game.id)}
      renderItem={({ item }) => (
        <View style={styles.cell}>
          <GameCard game={item} variant="grid" href={getHref(item)} />
        </View>
      )}
      contentContainerStyle={styles.content}
      columnWrapperStyle={styles.columns}
      onEndReached={() => {
        if (hasNextPage && !isFetchingNextPage) onLoadMore();
      }}
      onEndReachedThreshold={0.6}
      ListFooterComponent={
        isFetchingNextPage ? (
          <InlineState loading minHeight={72} />
        ) : isFetchNextPageError ? (
          <InlineState
            minHeight={72}
            message="More games could not be loaded."
            onRetry={onRetryNextPage}
          />
        ) : null
      }
    />
  );
}

const styles = StyleSheet.create({
  content: {
    paddingHorizontal: PAGE_GUTTER,
    paddingTop: 20,
    paddingBottom: 120,
  },
  columns: {
    gap: 14,
  },
  cell: {
    flex: 1,
    minWidth: 0,
    marginBottom: 16,
  },
});

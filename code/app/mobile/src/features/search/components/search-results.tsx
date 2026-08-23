import { useHits, useInstantSearch } from "react-instantsearch-core";
import { Host } from "@expo/ui";
import { FlatList, StyleSheet } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { ContentState } from "@/shared/ui/content-state";
import { getGameDetailHref } from "@/utils/game-routes";

import { getSearchContentStatus, SEARCH_STATE_COPY } from "../search-state";
import type { GameSearchHit } from "../search-types";

export function SearchResults({ bottomInset }: { bottomInset: number }) {
  const { items } = useHits<GameSearchHit>();
  const { status, error, refresh } = useInstantSearch({ catchError: true });
  const contentStatus = getSearchContentStatus({
    status,
    error,
    hasResults: items.length > 0,
  });

  if (contentStatus === "loading") {
    return <ContentState status="loading" minHeight={240} />;
  }

  if (contentStatus === "error") {
    return (
      <ContentState
        status="error"
        error={{
          title: SEARCH_STATE_COPY.errorTitle,
          message: SEARCH_STATE_COPY.errorMessage,
          onRetry: refresh,
        }}
      />
    );
  }

  if (contentStatus === "empty") {
    return (
      <ContentState
        status="empty"
        empty={{
          title: SEARCH_STATE_COPY.emptyTitle,
          message: SEARCH_STATE_COPY.emptyMessage,
        }}
        minHeight={240}
      />
    );
  }

  return (
    <FlatList
      data={items}
      numColumns={2}
      keyExtractor={(item) => item.objectID}
      style={styles.list}
      contentInsetAdjustmentBehavior="automatic"
      columnWrapperStyle={styles.row}
      contentContainerStyle={{
        gap: 12,
        paddingHorizontal: PAGE_GUTTER,
        paddingTop: 4,
        paddingBottom: bottomInset + 20,
      }}
      keyboardShouldPersistTaps="handled"
      renderItem={({ item }) => (
        <Host style={{ flex: 1 }}>
          <GameCard
            title={item.name ?? "Untitled"}
            metadata={`Game · ${String(item.release_year ?? "Release date unknown")}`}
            imageUrl={item.cover_url ? getIgdbImageUrl(item.cover_url, "cover_big") : undefined}
            href={getGameDetailHref(item.id)}
            variant="grid"
          />
        </Host>
      )}
    />
  );
}

const styles = StyleSheet.create({
  list: {
    flex: 1,
  },
  row: {
    gap: 12,
  },
});

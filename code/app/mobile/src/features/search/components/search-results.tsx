import { useHits, useInstantSearch } from "react-instantsearch-core";
import { Host } from "@expo/ui";
import { type Href } from "expo-router";
import { FlatList, StyleSheet } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { ContentState } from "@/shared/ui/content-state";
import type { GameSearchHit } from "../search-types";

const getSearchGameHref = (id: string | number) =>
  ({
    pathname: "/(search)/games/[slug]",
    params: { slug: String(id) },
  }) satisfies Href;

export function SearchResults({ bottomInset }: { bottomInset: number }) {
  const { items } = useHits<GameSearchHit>();
  const { status, error, refresh } = useInstantSearch({ catchError: true });

  if ((status === "loading" || status === "stalled") && items.length === 0) {
    return <ContentState status="loading" minHeight={240} />;
  }

  if (error) {
    return (
      <ContentState
        status="error"
        error={{
          title: "Search failed",
          message: "The search service could not be reached.",
          onRetry: refresh,
        }}
      />
    );
  }

  return (
    <FlatList
      data={items}
      numColumns={2}
      keyExtractor={(item) => item.objectID}
      style={styles.list}
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
            href={getSearchGameHref(item.id)}
            variant="grid"
            fillFraction={1}
          />
        </Host>
      )}
      ListEmptyComponent={
        <ContentState
          status="empty"
          empty={{
            title: "No results found",
            message: "Try another title or adjust your filters.",
          }}
          minHeight={240}
        />
      }
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

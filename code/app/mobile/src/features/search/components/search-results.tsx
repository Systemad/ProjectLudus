import { useHits, useInstantSearch } from "react-instantsearch-core";
import { Image } from "expo-image";
import { useRouter } from "expo-router";
import { FlatList, Pressable, StyleSheet, Text, View } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";
import { InlineState } from "@/shared/ui/inline-state";
import type { GameSearchHit } from "../search-types";

export function SearchResults({ bottomInset }: { bottomInset: number }) {
  const { items } = useHits<GameSearchHit>();
  const { status, error, refresh } = useInstantSearch({ catchError: true });

  if ((status === "loading" || status === "stalled") && items.length === 0) {
    return <InlineState loading minHeight={240} />;
  }

  if (error) {
    return (
      <InlineState
        title="Search failed"
        message="The search service could not be reached."
        onRetry={refresh}
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
      renderItem={({ item }) => <SearchResultCard hit={item} />}
      ListEmptyComponent={
        <InlineState
          title="No results found"
          message="Try another title or adjust your filters."
          minHeight={240}
        />
      }
    />
  );
}

function SearchResultCard({ hit }: { hit: GameSearchHit }) {
  const colors = useAppTheme();
  const router = useRouter();
  const title = hit.name ?? "Untitled";
  const metadata = `Game · ${String(hit.release_year ?? "Release date unknown")}`;
  const imageUrl = hit.cover_url ? getIgdbImageUrl(hit.cover_url, "cover_big") : null;

  return (
    <Pressable
      accessibilityRole="button"
      onPress={() => {
        router.push({
          pathname: "/(search)/games/[slug]",
          params: { slug: String(hit.id) },
        });
      }}
      style={({ pressed }) => [
        styles.card,
        { backgroundColor: colors.surface, opacity: pressed ? 0.72 : 1 },
      ]}
    >
      {imageUrl ? (
        <Image
          source={imageUrl}
          contentFit="cover"
          style={[styles.image, { backgroundColor: colors.surfaceHigh }]}
        />
      ) : (
        <View style={[styles.image, { backgroundColor: colors.surfaceHigh }]} />
      )}
      <View style={styles.copy}>
        <Text numberOfLines={2} style={[styles.title, { color: colors.text }]}>
          {title}
        </Text>
        <Text numberOfLines={1} style={[styles.metadata, { color: colors.textMuted }]}>
          {metadata}
        </Text>
      </View>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  list: {
    flex: 1,
  },
  card: {
    flex: 1,
    borderRadius: 18,
    overflow: "hidden",
  },
  image: {
    width: "100%",
    aspectRatio: 0.72,
  },
  copy: {
    padding: 10,
    gap: 5,
  },
  title: {
    fontSize: 16,
    lineHeight: 20,
    fontWeight: "700",
  },
  metadata: {
    fontSize: 13,
    lineHeight: 17,
  },
  row: {
    gap: 12,
  },
});

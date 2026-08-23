import { FlatList, Pressable, ScrollView, StyleSheet, Text, View } from "react-native";

import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { useContentBottomInset } from "@/shared/ui/insets";
import { getGameDetailHref } from "@/utils/game-routes";

import { BrowseListCard } from "./browse-list-card";

export type BrowseCollection = "mostPlayed" | "popularReleases" | "hotReleases" | "trending";

type BrowseListProps = {
  collection: BrowseCollection;
  games: GameBrowseDto[];
  onCollectionChange: (collection: BrowseCollection) => void;
};

const collections: readonly { value: BrowseCollection; label: string }[] = [
  { value: "mostPlayed", label: "Played" },
  { value: "popularReleases", label: "Popular" },
  { value: "hotReleases", label: "Hot" },
  { value: "trending", label: "Trending" },
];

function CollectionChip({
  collection,
  selectedCollection,
  label,
  onSelect,
}: {
  collection: BrowseCollection;
  selectedCollection: BrowseCollection;
  label: string;
  onSelect: (collection: BrowseCollection) => void;
}) {
  const colors = useAppTheme();
  const selected = collection === selectedCollection;

  return (
    <Pressable
      accessibilityRole="button"
      accessibilityState={{ selected }}
      onPress={() => onSelect(collection)}
      style={[
        styles.chip,
        {
          backgroundColor: selected ? colors.primaryContainer : colors.surfaceHigh,
          borderColor: colors.outline,
        },
      ]}
    >
      <Text style={{ color: colors.text, fontSize: 13, fontWeight: "700" }}>{label}</Text>
    </Pressable>
  );
}

export function BrowseList({ collection, games, onCollectionChange }: BrowseListProps) {
  const colors = useAppTheme();
  const bottomInset = useContentBottomInset(28);

  return (
    <FlatList
      data={games}
      keyExtractor={(game) => String(game.id)}
      contentInsetAdjustmentBehavior="automatic"
      contentContainerStyle={[styles.content, { paddingBottom: bottomInset }]}
      ItemSeparatorComponent={() => <View style={styles.separator} />}
      ListHeaderComponent={
        <View style={styles.header}>
          <ScrollView horizontal showsHorizontalScrollIndicator={false}>
            <View style={styles.chips}>
              {collections.map((item) => (
                <CollectionChip
                  key={item.value}
                  collection={item.value}
                  selectedCollection={collection}
                  label={item.label}
                  onSelect={onCollectionChange}
                />
              ))}
            </View>
          </ScrollView>
          <Text style={[styles.eyebrow, { color: colors.text }]}>LIVE PLAYER RANKINGS</Text>
        </View>
      }
      renderItem={({ item, index }) => (
        <BrowseListCard game={item} rank={index + 1} href={getGameDetailHref(item.id)} />
      )}
    />
  );
}

const styles = StyleSheet.create({
  content: {
    paddingHorizontal: 16,
    paddingTop: 12,
  },
  header: {
    gap: 10,
    paddingBottom: 2,
  },
  chips: {
    flexDirection: "row",
    gap: 8,
  },
  chip: {
    borderRadius: 18,
    borderWidth: StyleSheet.hairlineWidth,
    paddingHorizontal: 14,
    paddingVertical: 9,
  },
  eyebrow: {
    fontSize: 12,
    fontWeight: "700",
  },
  separator: {
    height: 10,
  },
});

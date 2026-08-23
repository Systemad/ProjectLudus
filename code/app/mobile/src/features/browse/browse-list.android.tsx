import { Host } from "@expo/ui";
import { FilterChip, LazyColumn, Row, Text } from "@expo/ui/jetpack-compose";
import { fillMaxSize, fillMaxWidth, horizontalScroll } from "@expo/ui/jetpack-compose/modifiers";

import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
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
  return (
    <FilterChip selected={collection === selectedCollection} onClick={() => onSelect(collection)}>
      <FilterChip.Label>
        <Text>{label}</Text>
      </FilterChip.Label>
    </FilterChip>
  );
}

export function BrowseList({ collection, games, onCollectionChange }: BrowseListProps) {
  const bottomInset = useContentBottomInset(28);

  return (
    <Host style={{ flex: 1 }}>
      <LazyColumn
        modifiers={[fillMaxSize()]}
        contentPadding={{ bottom: bottomInset, end: 16, start: 16, top: 12 }}
        verticalArrangement={{ spacedBy: 10 }}
      >
        <Row
          modifiers={[fillMaxWidth(), horizontalScroll()]}
          horizontalArrangement={{ spacedBy: 8 }}
        >
          {collections.map((item) => (
            <CollectionChip
              key={item.value}
              collection={item.value}
              selectedCollection={collection}
              label={item.label}
              onSelect={onCollectionChange}
            />
          ))}
        </Row>

        <Text style={{ typography: "labelMedium", fontWeight: "700" }}>LIVE PLAYER RANKINGS</Text>

        {games.map((game, index) => (
          <BrowseListCard
            key={game.id}
            game={game}
            rank={index + 1}
            href={getGameDetailHref(game.id)}
          />
        ))}
      </LazyColumn>
    </Host>
  );
}

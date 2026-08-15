import { FilterChip, LazyColumn, Row, Text } from "@expo/ui/jetpack-compose";
import { fillMaxSize, fillMaxWidth, horizontalScroll } from "@expo/ui/jetpack-compose/modifiers";

import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";

import { BrowseListCard } from "./browse-list-card.android";

export type BrowseCollection = "mostPlayed" | "popularReleases" | "hotReleases" | "trending";

type BrowseListProps = {
  collection: BrowseCollection;
  games: GameBrowseDto[];
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
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

export function BrowseList({
  collection,
  games,
  isLoading,
  isError,
  onRetry,
  onCollectionChange,
}: BrowseListProps) {
  const colors = useAppTheme();

  return (
    <LazyColumn
      modifiers={[fillMaxSize()]}
      contentPadding={{ bottom: 28, end: 16, start: 16, top: 12 }}
      verticalArrangement={{ spacedBy: 10 }}
    >
      <Row modifiers={[fillMaxWidth(), horizontalScroll()]} horizontalArrangement={{ spacedBy: 8 }}>
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

      <Text
        color={colors.textMuted as string}
        style={{ typography: "labelMedium", fontWeight: "700" }}
      >
        LIVE PLAYER RANKINGS
      </Text>

      <ContentState
        status={getContentStateStatus(isLoading, isError, games.length === 0)}
        loading={{ label: "Loading games…" }}
        error={{
          onRetry,
          title: "This list could not be loaded.",
          retryLabel: "Retry",
        }}
        empty={{ title: "No games found", message: "There are no games in this collection yet." }}
      >
        {games.map((game, index) => (
          <BrowseListCard
            key={game.id}
            game={game}
            rank={index + 1}
            href={{ pathname: "/(browse)/games/[slug]", params: { slug: game.id } }}
          />
        ))}
      </ContentState>
    </LazyColumn>
  );
}

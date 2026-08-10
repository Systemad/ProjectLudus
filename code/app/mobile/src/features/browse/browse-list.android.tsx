import {
  Box,
  Column,
  LazyColumn,
  SegmentedButton,
  Shape,
  SingleChoiceSegmentedButtonRow,
  Surface,
  Text,
} from "@expo/ui/jetpack-compose";
import {
  align,
  fillMaxSize,
  fillMaxWidth,
  padding,
  weight,
} from "@expo/ui/jetpack-compose/modifiers";

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

function CollectionButton({
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
    <SegmentedButton
      selected={collection === selectedCollection}
      onClick={() => onSelect(collection)}
      modifiers={[weight(1)]}
    >
      <SegmentedButton.Label>
        <Text>{label}</Text>
      </SegmentedButton.Label>
    </SegmentedButton>
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
    <Box modifiers={[fillMaxSize()]}>
      <LazyColumn
        modifiers={[fillMaxSize()]}
        contentPadding={{ bottom: 104, end: 16, start: 16, top: 12 }}
        verticalArrangement={{ spacedBy: 8 }}
      >
        <Column
          modifiers={[fillMaxWidth(), padding(0, 0, 0, 4)]}
          verticalArrangement={{ spacedBy: 2 }}
        >
          <Text
            color={colors.text as string}
            style={{ typography: "headlineMedium", fontWeight: "800" }}
          >
            Browse
          </Text>
          <Text
            color={colors.textMuted as string}
            style={{ typography: "labelMedium", fontWeight: "700" }}
          >
            STEAM DATA
          </Text>
        </Column>

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

      <Surface
        color={colors.surfaceHigh}
        contentColor={colors.text}
        tonalElevation={2}
        shape={Shape.RoundedCorner({
          cornerRadii: { topStart: 20, topEnd: 20, bottomStart: 20, bottomEnd: 20 },
        })}
        modifiers={[align("bottomCenter"), fillMaxWidth(), padding(12, 8, 12, 12)]}
      >
        <SingleChoiceSegmentedButtonRow modifiers={[fillMaxWidth()]}>
          <CollectionButton
            collection="mostPlayed"
            selectedCollection={collection}
            label="Played"
            onSelect={onCollectionChange}
          />
          <CollectionButton
            collection="popularReleases"
            selectedCollection={collection}
            label="Popular"
            onSelect={onCollectionChange}
          />
          <CollectionButton
            collection="hotReleases"
            selectedCollection={collection}
            label="Hot"
            onSelect={onCollectionChange}
          />
          <CollectionButton
            collection="trending"
            selectedCollection={collection}
            label="Trending"
            onSelect={onCollectionChange}
          />
        </SingleChoiceSegmentedButtonRow>
      </Surface>
    </Box>
  );
}

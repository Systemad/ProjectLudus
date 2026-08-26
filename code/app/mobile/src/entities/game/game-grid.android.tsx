import { Host } from "@expo/ui";
import { Column, FlowRow } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, padding } from "@expo/ui/jetpack-compose/modifiers";
import { useWindowDimensions } from "react-native";

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
  const { width } = useWindowDimensions();
  const bottomInset = useContentBottomInset(120);
  const cardWidth = Math.floor((width - GRID_PAGE_PADDING * 2 - GRID_COLUMN_GAP) / 2);

  return (
    <ContentState
      fullScreen
      status={getContentStateStatus(isLoading, isError, games.length === 0)}
      loading={{ label: "Loading games…" }}
      error={{ onRetry, title: "Couldn’t load this collection." }}
      empty={{ title: "No games found", message: "There are no games in this collection yet." }}
    >
      <Host style={{ flex: 1 }}>
        <Column
          modifiers={[
            fillMaxWidth(),
            padding(GRID_PAGE_PADDING, GRID_PAGE_PADDING, GRID_PAGE_PADDING, bottomInset),
          ]}
          verticalArrangement={{ spacedBy: GRID_COLUMN_GAP }}
        >
          <FlowRow
            horizontalArrangement={{ spacedBy: GRID_COLUMN_GAP }}
            verticalArrangement={{ spacedBy: GRID_ROW_GAP }}
            modifiers={[fillMaxWidth()]}
          >
            {games.map((game) => (
              <GameCard
                key={String(game.id)}
                {...getGameCardData(game)}
                variant="grid"
                href={getHref(game)}
                cardWidth={cardWidth}
              />
            ))}
          </FlowRow>
        </Column>
      </Host>
    </ContentState>
  );
}

import { Host } from "@expo/ui";
import { Column, FlowRow } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, padding } from "@expo/ui/jetpack-compose/modifiers";

import { GameCard, getGameCardData } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { CollectionState } from "@/shared/ui/collection-state";
import type { Href } from "expo-router";

type GameGridProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
};

export function GameGrid({ games, getHref, isLoading, isError, onRetry }: GameGridProps) {
  return (
    <CollectionState
      fullScreen
      isLoading={isLoading}
      isError={isError}
      isEmpty={games.length === 0}
      onRetry={onRetry}
      loadingLabel="Loading games…"
      errorTitle="Couldn’t load this collection."
      emptyTitle="No games found"
      emptyMessage="There are no games in this collection yet."
    >
      <Host style={{ flex: 1 }}>
        <Column
          modifiers={[fillMaxWidth(), padding(20, 20, 20, 120)]}
          verticalArrangement={{ spacedBy: 14 }}
        >
          <FlowRow
            horizontalArrangement={{ spacedBy: 14 }}
            verticalArrangement={{ spacedBy: 16 }}
            modifiers={[fillMaxWidth()]}
          >
            {games.map((game) => (
              <GameCard
                key={String(game.id)}
                {...getGameCardData(game)}
                variant="grid"
                href={getHref(game)}
              />
            ))}
          </FlowRow>
        </Column>
      </Host>
    </CollectionState>
  );
}

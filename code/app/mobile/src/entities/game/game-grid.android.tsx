import { Host } from "@expo/ui";
import { Column, FlowRow } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, padding } from "@expo/ui/jetpack-compose/modifiers";
import { useWindowDimensions } from "react-native";

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
  const cardWidth = Math.floor((width - 40 - 14) / 2);

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
          modifiers={[fillMaxWidth(), padding(20, 20, 20, bottomInset)]}
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
                cardWidth={cardWidth}
              />
            ))}
          </FlowRow>
        </Column>
      </Host>
    </ContentState>
  );
}

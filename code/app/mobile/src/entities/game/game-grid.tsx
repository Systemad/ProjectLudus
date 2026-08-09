import { Host } from "@expo/ui";
import {
  Button,
  Column,
  FlowRow,
  LoadingIndicator,
  Text,
} from "@expo/ui/jetpack-compose";
import { fillMaxWidth, padding, paddingAll } from "@expo/ui/jetpack-compose/modifiers";

import { BrowseGameCard } from "@/entities/game/browse-game-card.android";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";

type GameGridProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
};

export function GameGrid({ games, getHref, isLoading, isError, onRetry }: GameGridProps) {
  if (isLoading) {
    return (
      <Host style={{ flex: 1 }}>
        <Column
          horizontalAlignment="center"
          verticalArrangement={{ spacedBy: 12 }}
          modifiers={[fillMaxWidth(), paddingAll(24)]}
        >
          <LoadingIndicator />
          <Text>Loading games…</Text>
        </Column>
      </Host>
    );
  }

  if (isError) {
    return (
      <Host style={{ flex: 1 }}>
        <Column
          horizontalAlignment="center"
          verticalArrangement={{ spacedBy: 12 }}
          modifiers={[fillMaxWidth(), paddingAll(24)]}
        >
          <Text>Couldn’t load this collection.</Text>
          <Button onClick={onRetry}>
            <Text>Try again</Text>
          </Button>
        </Column>
      </Host>
    );
  }

  if (games.length === 0) {
    return (
      <Host style={{ flex: 1 }}>
        <Column
          horizontalAlignment="center"
          verticalArrangement={{ spacedBy: 8 }}
          modifiers={[fillMaxWidth(), paddingAll(24)]}
        >
          <Text>No games found</Text>
          <Text>There are no games in this collection yet.</Text>
        </Column>
      </Host>
    );
  }

  return (
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
            <BrowseGameCard key={String(game.id)} game={game} href={getHref(game)} />
          ))}
        </FlowRow>
      </Column>
    </Host>
  );
}

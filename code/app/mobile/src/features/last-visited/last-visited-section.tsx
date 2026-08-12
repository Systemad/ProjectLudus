import { Host } from "@expo/ui";
import type { Href } from "expo-router";
import { StyleSheet, Text, View, useWindowDimensions } from "react-native";

import { getGameRailCardWidth } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";

import { useLastVisited } from "./last-visited-context";
import type { GameId } from "./last-visited-storage";

const gameHref = (gameId: GameId) =>
  ({
    pathname: "/(discover)/games/[slug]",
    params: { slug: gameId },
  }) satisfies Href;

export function LastVisitedSection() {
  const colors = useAppTheme();
  const { width } = useWindowDimensions();
  const { gameId, game, isHydrating, isLoading, isError, retry } = useLastVisited();

  if (isHydrating || gameId === null) {
    return null;
  }

  const status = getContentStateStatus(isLoading, isError, !game);
  const metadata = [
    game?.firstReleaseDate?.slice(0, 4) ?? "TBA",
    game?.gameTypeName ?? "Game",
  ].join(" · ");
  const imageUrl = game
    ? (getIgdbImageUrl(game.cover, "cover_big", true) ?? game.coverUrl ?? undefined)
    : undefined;

  return (
    <View style={styles.section}>
      <View style={styles.header}>
        <Text style={[styles.title, { color: colors.text }]}>Last visited</Text>
        <Text style={[styles.subtitle, { color: colors.textMuted }]}>
          Pick up where you left off
        </Text>
      </View>

      <ContentState
        status={status}
        minHeight={190}
        loading={{ label: "Loading last visited game…" }}
        error={{
          onRetry: retry,
          message: "Your last visited game could not be loaded.",
          retryLabel: "Retry",
        }}
        empty={{ message: "Your last visited game is no longer available." }}
      >
        {game ? (
          <Host matchContents>
            <GameCard
              title={game.name}
              metadata={metadata}
              imageUrl={imageUrl}
              variant="rail"
              href={gameHref(gameId)}
              cardWidth={getGameRailCardWidth(width)}
            />
          </Host>
        ) : null}
      </ContentState>
    </View>
  );
}

const styles = StyleSheet.create({
  section: {
    gap: 8,
  },
  header: {
    minHeight: 56,
    justifyContent: "center",
    gap: 3,
  },
  title: {
    fontSize: 22,
    lineHeight: 28,
    fontWeight: "800",
  },
  subtitle: {
    fontSize: 14,
  },
});

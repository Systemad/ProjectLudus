import { Host } from "@expo/ui";
import { ScrollView, StyleSheet, Text, View, useWindowDimensions } from "react-native";

import { getGameRailCardWidth } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { getGameDetailHref } from "@/utils/game-routes";

import { useLastVisited } from "./last-visited-context";

export function LastVisitedSection() {
  const colors = useAppTheme();
  const { width } = useWindowDimensions();
  const { games, isHydrating, isLoading, isError, retry } = useLastVisited();

  if (isHydrating || (games.length === 0 && !isLoading && !isError)) {
    return null;
  }

  const status = getContentStateStatus(isLoading, isError, games.length === 0);
  const cardWidth = getGameRailCardWidth(width);

  return (
    <View style={styles.section}>
      <View style={styles.header}>
        <Text style={[styles.title, { color: colors.text }]}>Recently visited</Text>
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
        <ScrollView
          horizontal
          showsHorizontalScrollIndicator={false}
          contentContainerStyle={styles.cards}
        >
          {games.map(({ id, game }) => {
            const metadata = [
              game.firstReleaseDate?.slice(0, 4) ?? "TBA",
              game.gameTypeName ?? "Game",
            ].join(" · ");
            const imageUrl =
              getIgdbImageUrl(game.cover, "cover_big", true) ?? game.coverUrl ?? undefined;

            return (
              <Host key={id} matchContents>
                <GameCard
                  title={game.name}
                  metadata={metadata}
                  imageUrl={imageUrl}
                  variant="rail"
                  href={getGameDetailHref(id)}
                  cardWidth={cardWidth}
                />
              </Host>
            );
          })}
        </ScrollView>
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
  cards: {
    gap: 10,
  },
});

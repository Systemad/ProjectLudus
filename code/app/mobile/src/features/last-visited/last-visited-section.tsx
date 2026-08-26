import { StyleSheet, Text, View } from "react-native";

import { GAME_RAIL_CARD_WIDTH } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { spacing, typography } from "@/theme";
import { getGameDetailHref } from "@/utils/game-routes";

import { useLastVisited } from "./last-visited-context";
import { LastVisitedCarousel } from "./last-visited-carousel";

export function LastVisitedSection() {
  const colors = useAppTheme();
  const { games, isHydrating, isLoading, isError, retry } = useLastVisited();

  if (isHydrating || (games.length === 0 && !isLoading && !isError)) {
    return null;
  }

  const status = getContentStateStatus(isLoading, isError, games.length === 0);
  const cards = games.map(({ id, game }) => {
    const metadata = [
      game.firstReleaseDate?.slice(0, 4) ?? "TBA",
      game.gameTypeName ?? "Game",
    ].join(" · ");
    const imageUrl = getIgdbImageUrl(game.cover, "cover_big", true) ?? game.coverUrl ?? undefined;

    return (
      <GameCard
        key={id}
        title={game.name}
        metadata={metadata}
        imageUrl={imageUrl}
        variant="rail"
        href={getGameDetailHref(id)}
        cardWidth={GAME_RAIL_CARD_WIDTH}
      />
    );
  });

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
        <LastVisitedCarousel>{cards}</LastVisitedCarousel>
      </ContentState>
    </View>
  );
}

const styles = StyleSheet.create({
  section: {
    gap: spacing.xs,
  },
  header: {
    minHeight: 56,
    justifyContent: "center",
    gap: spacing.xxs - 1,
  },
  title: {
    ...typography.sectionTitle,
  },
  subtitle: {
    ...typography.body,
  },
});

import { Host, Row, ScrollView as ExpoScrollView } from "@expo/ui";
import { Platform, ScrollView, StyleSheet, Text, View } from "react-native";

import { GAME_RAIL_CARD_WIDTH, GAME_RAIL_GAP } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { getGameDetailHref } from "@/utils/game-routes";

import { useLastVisited } from "./last-visited-context";

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
        {Platform.OS === "android" ? (
          <Host matchContents={{ vertical: true }} style={{ width: "100%" }}>
            <ExpoScrollView
              direction="horizontal"
              showsIndicators={false}
              style={{ width: "100%" }}
            >
              <Row spacing={GAME_RAIL_GAP}>{cards}</Row>
            </ExpoScrollView>
          </Host>
        ) : (
          <ScrollView
            horizontal
            showsHorizontalScrollIndicator={false}
            contentContainerStyle={styles.cards}
          >
            {cards}
          </ScrollView>
        )}
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
    gap: GAME_RAIL_GAP,
  },
});

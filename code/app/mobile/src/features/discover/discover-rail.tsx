import { Link, type Href } from "expo-router";
import { ChevronRight } from "lucide-react-native";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { GameCarousel } from "@/entities/game/game-carousel";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { spacing, typography } from "@/theme";

type DiscoverRailProps = {
  title: string;
  subtitle: string;
  href: Href;
  games: GameBrowseDto[];
  getGameHref: (game: GameBrowseDto) => Href;
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
  featured?: boolean;
};

export function DiscoverRail({
  title,
  subtitle,
  href,
  games,
  getGameHref,
  isLoading,
  isError,
  onRetry,
  featured = false,
}: DiscoverRailProps) {
  const colors = useAppTheme();

  return (
    <View style={styles.section}>
      <Link href={href} asChild>
        <Pressable
          accessibilityRole="button"
          accessibilityLabel={`View all ${title} games`}
          style={({ pressed }) => [styles.header, { opacity: pressed ? 0.68 : 1 }]}
        >
          <View style={styles.titleRow}>
            <Text style={[styles.title, { color: colors.text }]}>{title}</Text>
            <ChevronRight color={colors.primary} size={18} strokeWidth={2.4} />
          </View>
          <Text style={[styles.subtitle, { color: colors.textMuted }]}>{subtitle}</Text>
        </Pressable>
      </Link>

      <ContentState
        status={getContentStateStatus(isLoading, isError, games.length === 0)}
        minHeight={170}
        loading={{ label: "Loading games…" }}
        error={{
          onRetry,
          message: "This collection could not be loaded.",
          retryLabel: "Retry",
        }}
        empty={{ message: "No games are available in this collection yet." }}
      >
        <GameCarousel games={games} getHref={getGameHref} variant={featured ? "cover" : "rail"} />
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
  titleRow: {
    width: "100%",
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },
  title: {
    ...typography.sectionTitle,
  },
  subtitle: {
    ...typography.body,
  },
});

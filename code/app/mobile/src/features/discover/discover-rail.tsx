import { Link, type Href } from "expo-router";
import { ChevronRight } from "lucide-react-native";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { GameCarousel } from "@/entities/game/game-carousel";
import { FeaturedCarousel } from "@/features/discover/featured-carousel";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { CollectionState } from "@/shared/ui/collection-state";

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

      <CollectionState
        isLoading={isLoading}
        isError={isError}
        isEmpty={games.length === 0}
        onRetry={onRetry}
        minHeight={170}
        loadingLabel="Loading games…"
        errorMessage="This collection could not be loaded."
        emptyMessage="No games are available in this collection yet."
        retryLabel="Retry"
      >
        {featured ? (
          <FeaturedCarousel games={games} getHref={getGameHref} />
        ) : (
          <GameCarousel games={games} getHref={getGameHref} />
        )}
      </CollectionState>
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
  titleRow: {
    width: "100%",
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
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

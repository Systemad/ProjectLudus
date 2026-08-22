import { Image } from "expo-image";
import { Link, type Href } from "expo-router";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { getGameCardImage } from "@/entities/game/game-image";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { formatPlayerCount } from "@/utils/steam-chart";

type BrowseListCardProps = {
  game: GameBrowseDto;
  rank: number;
  href: Href;
};

export function BrowseListCard({ game, rank, href }: BrowseListCardProps) {
  const colors = useAppTheme();
  const imageUrl = getGameCardImage(game);
  const genre = game.gameFeatures.genres[0]?.name ?? game.gameFeatures.themes[0]?.name ?? "Game";

  return (
    <Link href={href} asChild>
      <Pressable style={[styles.card, { backgroundColor: colors.surfaceHigh }]}>
        {imageUrl ? (
          <Image source={imageUrl} style={styles.image} contentFit="cover" />
        ) : (
          <View style={[styles.image, styles.placeholder, { backgroundColor: colors.primaryContainer }]}>
            <Text style={[styles.initial, { color: colors.onPrimaryContainer }]}>
              {game.name.slice(0, 1)}
            </Text>
          </View>
        )}
        <View style={styles.copy}>
          <View style={styles.titleRow}>
            <Text numberOfLines={2} style={[styles.title, { color: colors.text }]}>
              {game.name}
            </Text>
            <Text style={[styles.rank, { color: colors.text }]}>#{rank}</Text>
          </View>
          <Text numberOfLines={1} style={[styles.genre, { color: colors.textMuted }]}>
            {genre}
          </Text>
          <View style={styles.stats}>
            <PlayerStat
              label="PLAYING NOW"
              value={formatPlayerCount(game.steam?.currentPlayers)}
              colors={colors}
            />
            <PlayerStat
              label="PEAK TODAY"
              value={formatPlayerCount(game.steam?.peak24h)}
              colors={colors}
            />
          </View>
        </View>
      </Pressable>
    </Link>
  );
}

function PlayerStat({
  label,
  value,
  colors,
}: {
  label: string;
  value: string;
  colors: ReturnType<typeof useAppTheme>;
}) {
  return (
    <View style={styles.stat}>
      <Text style={[styles.statLabel, { color: colors.textMuted }]}>{label}</Text>
      <Text style={[styles.statValue, { color: colors.text }]}>{value}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    borderRadius: 16,
    flexDirection: "row",
    gap: 12,
    overflow: "hidden",
    padding: 8,
  },
  image: {
    borderRadius: 10,
    height: 104,
    width: 76,
  },
  placeholder: {
    alignItems: "center",
    justifyContent: "center",
  },
  initial: {
    fontSize: 28,
    fontWeight: "800",
  },
  copy: {
    flex: 1,
    gap: 6,
    justifyContent: "space-between",
    minWidth: 0,
    paddingVertical: 1,
  },
  titleRow: {
    alignItems: "flex-start",
    flexDirection: "row",
    gap: 8,
  },
  title: {
    flex: 1,
    fontSize: 16,
    fontWeight: "700",
  },
  rank: {
    fontSize: 14,
    fontWeight: "800",
  },
  genre: {
    fontSize: 14,
  },
  stats: {
    flexDirection: "row",
    gap: 16,
  },
  stat: {
    flex: 1,
    gap: 2,
  },
  statLabel: {
    fontSize: 10,
    fontWeight: "700",
  },
  statValue: {
    fontSize: 16,
    fontWeight: "700",
  },
});

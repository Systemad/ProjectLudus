import { Image } from "expo-image";
import { Link, type Href } from "expo-router";
import { Pressable, StyleSheet, Text, View } from "react-native";

import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { radius, spacing, typography } from "@/theme";

import { getGameCardImage } from "./game-image";
import { formatSteamReviewRating, getSteamReviewEmoji } from "./steam-review";

const GAME_CARD_ASPECT_RATIO = 0.72;

export type GameCardItem = {
  id: string;
  title: string;
  metadata: string;
  imageUrl?: string;
  href: Href;
};

export function getGameCardItem(game: GameBrowseDto, href: Href): GameCardItem {
  const primaryGenre = game.gameFeatures.genres[0]?.name ?? "Game";
  const reviewRating = formatSteamReviewRating(game.steam?.review);
  const review =
    reviewRating === "N/A"
      ? undefined
      : `${getSteamReviewEmoji(game.steam?.review)} ${reviewRating}`;

  return {
    id: String(game.id),
    title: game.name,
    metadata: [game.firstReleaseDate?.slice(0, 4) ?? "TBA", primaryGenre, review]
      .filter((value): value is string => value !== undefined)
      .join(" · "),
    imageUrl: getGameCardImage(game),
    href,
  };
}

export function GameCard({ title, metadata, imageUrl, href }: GameCardItem) {
  const colors = useAppTheme();

  return (
    <Link href={href} asChild>
      <Pressable
        accessibilityRole="button"
        accessibilityLabel={`View ${title}`}
        style={({ pressed }) => [
          styles.card,
          { backgroundColor: colors.surfaceHigh, opacity: pressed ? 0.82 : 1 },
        ]}
      >
        {imageUrl ? (
          <Image source={imageUrl} style={styles.image} contentFit="cover" transition={180} />
        ) : (
          <View style={[styles.image, styles.placeholder, { backgroundColor: colors.surfaceHigh }]}>
            <Text style={[styles.placeholderText, { color: colors.textMuted }]}>
              {title.slice(0, 1)}
            </Text>
          </View>
        )}
        <View style={styles.body}>
          <Text
            style={[styles.title, { color: colors.text }]}
            numberOfLines={1}
            ellipsizeMode="tail"
          >
            {title}
          </Text>
          <Text style={[styles.metadata, { color: colors.textMuted }]} numberOfLines={1}>
            {metadata}
          </Text>
        </View>
      </Pressable>
    </Link>
  );
}

const styles = StyleSheet.create({
  card: {
    width: "100%",
    overflow: "hidden",
    borderRadius: radius.md,
    borderCurve: "continuous",
  },
  image: {
    width: "100%",
    aspectRatio: GAME_CARD_ASPECT_RATIO,
    borderRadius: radius.md,
    borderCurve: "continuous",
  },
  placeholder: {
    alignItems: "center",
    justifyContent: "center",
  },
  placeholderText: {
    ...typography.placeholder,
  },
  body: {
    padding: spacing.md,
    gap: spacing.xxs,
  },
  title: {
    ...typography.cardTitle,
  },
  metadata: {
    ...typography.cardMetadata,
  },
});

import { Image } from "expo-image";
import { Link } from "expo-router";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";
import { radius, spacing, typography } from "@/theme";
import type { GameCardProps } from "./game-card-types";

export { getGameCardData } from "./game-card-data";
export type { GameCardData } from "./game-card-data";
export type { GameCardProps, GameCardVariant } from "./game-card-types";

export function GameCard({ title, metadata, imageUrl, variant, href, cardWidth }: GameCardProps) {
  const colors = useAppTheme();
  const showCopy = variant !== "cover";

  return (
    <Link href={href} asChild>
      <Pressable
        accessibilityRole="button"
        accessibilityLabel={`View ${title}`}
        style={({ pressed }) => [
          styles.card,
          cardWidth ? { width: cardWidth } : undefined,
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
        {showCopy && metadata ? (
          <View style={[styles.copy, variant === "rail" && styles.railCopy]}>
            <Text
              style={[styles.name, variant === "rail" && styles.railName, { color: colors.text }]}
              numberOfLines={2}
            >
              {title}
            </Text>
            <Text
              style={[
                styles.meta,
                variant === "rail" && styles.railMeta,
                { color: colors.textMuted },
              ]}
              numberOfLines={1}
            >
              {metadata}
            </Text>
          </View>
        ) : null}
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
    aspectRatio: 0.72,
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
  copy: {
    padding: spacing.md,
    gap: spacing.xxs,
  },
  railCopy: {
    paddingHorizontal: spacing.xs,
    paddingVertical: spacing.sm - 1,
    gap: spacing.xxs - 1,
  },
  name: {
    ...typography.cardTitle,
  },
  railName: {
    ...typography.railTitle,
  },
  meta: {
    ...typography.cardMetadata,
  },
  railMeta: {
    fontSize: 11,
    lineHeight: 14,
  },
});

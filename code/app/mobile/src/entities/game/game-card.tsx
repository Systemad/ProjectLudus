import type { Href } from "expo-router";
import { Image } from "expo-image";
import { Link } from "expo-router";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";

import type { GameCardData } from "./game-card-data";

export { getGameCardData } from "./game-card-data";
export type { GameCardData } from "./game-card-data";

export type GameCardVariant = "grid" | "rail" | "cover";

export type GameCardProps = GameCardData & {
  variant: GameCardVariant;
  href: Href;
  cardWidth?: number;
  fillFraction?: number;
};

export function GameCard({ title, metadata, imageUrl, variant, href }: GameCardProps) {
  const colors = useAppTheme();
  const showCopy = variant !== "cover";

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
    borderRadius: 12,
    borderCurve: "continuous",
  },
  image: {
    width: "100%",
    aspectRatio: 0.72,
    borderRadius: 12,
    borderCurve: "continuous",
  },
  placeholder: {
    alignItems: "center",
    justifyContent: "center",
  },
  placeholderText: {
    fontSize: 32,
    fontWeight: "800",
  },
  copy: {
    padding: 12,
    gap: 4,
  },
  railCopy: {
    paddingHorizontal: 8,
    paddingVertical: 9,
    gap: 3,
  },
  name: {
    fontSize: 15,
    lineHeight: 19,
    fontWeight: "700",
  },
  railName: {
    fontSize: 13,
    lineHeight: 16,
  },
  meta: {
    fontSize: 12,
  },
  railMeta: {
    fontSize: 11,
    lineHeight: 14,
  },
});

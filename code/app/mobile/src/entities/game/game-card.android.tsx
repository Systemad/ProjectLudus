import { Box, Card, Column, RNHostView, Text } from "@expo/ui/jetpack-compose";
import {
  clickable,
  fillMaxWidth,
  height,
  paddingAll,
  width,
} from "@expo/ui/jetpack-compose/modifiers";
import { Image } from "expo-image";
import { type Href, useRouter } from "expo-router";

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

export function GameCard({
  title,
  metadata,
  imageUrl,
  variant,
  href,
  cardWidth,
  fillFraction = 0.5,
}: GameCardProps) {
  const colors = useAppTheme();
  const router = useRouter();
  const showCopy = variant !== "cover";
  const cardModifiers = cardWidth
    ? [width(cardWidth)]
    : [variant === "grid" ? fillMaxWidth(fillFraction) : fillMaxWidth()];

  return (
    <Card
      colors={{ containerColor: colors.surfaceHigh, contentColor: colors.text }}
      elevation={0}
      modifiers={[...cardModifiers, clickable(() => router.push(href))]}
    >
      <Column modifiers={[fillMaxWidth()]}>
        {imageUrl ? (
          <RNHostView modifiers={[fillMaxWidth()]}>
            <Image
              source={imageUrl}
              style={{ width: "100%", aspectRatio: 0.72 }}
              contentFit="cover"
            />
          </RNHostView>
        ) : (
          <Box
            contentAlignment="center"
            modifiers={[fillMaxWidth(), height(cardWidth ? cardWidth / 0.72 : 180)]}
          >
            <Text color={colors.textMuted as string} style={{ typography: "titleLarge" }}>
              {title.slice(0, 1)}
            </Text>
          </Box>
        )}
        {showCopy && metadata ? (
          <Column
            modifiers={[paddingAll(variant === "rail" ? 8 : 12)]}
            verticalArrangement={{ spacedBy: 4 }}
          >
            <Text
              color={colors.text as string}
              maxLines={2}
              style={{
                typography: variant === "rail" ? "titleSmall" : "titleMedium",
                fontWeight: "700",
              }}
            >
              {title}
            </Text>
            <Text
              color={colors.textMuted as string}
              maxLines={1}
              style={{ typography: "bodySmall" }}
            >
              {metadata}
            </Text>
          </Column>
        ) : null}
      </Column>
    </Card>
  );
}

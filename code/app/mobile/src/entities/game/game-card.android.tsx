import { Card, Column, Text } from "@expo/ui/jetpack-compose";
import { clickable, fillMaxWidth, paddingAll, width } from "@expo/ui/jetpack-compose/modifiers";
import { useRouter } from "expo-router";

import { GameArtwork } from "./game-artwork.android";
import { useAppTheme } from "@/hooks/use-app-theme";
import type { GameCardProps } from "./game-card-types";

export { getGameCardData } from "./game-card-data";
export type { GameCardData } from "./game-card-data";
export type { GameCardProps, GameCardVariant } from "./game-card-types";

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
        <GameArtwork
          imageUrl={imageUrl}
          fallbackLabel={title.slice(0, 1)}
          modifiers={[fillMaxWidth()]}
          fallbackHeight={cardWidth ? cardWidth / 0.72 : 180}
          aspectRatio={0.72}
        />
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

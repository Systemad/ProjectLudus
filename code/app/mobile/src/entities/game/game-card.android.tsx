import { Card, Column, Text } from "@expo/ui/jetpack-compose";
import {
  clickable,
  fillMaxWidth,
  height,
  paddingAll,
  width,
} from "@expo/ui/jetpack-compose/modifiers";
import { useRouter } from "expo-router";

import { GAME_RAIL_COPY_HEIGHT } from "@/config/layout";
import { spacing } from "@/theme";
import { GameArtwork } from "./game-artwork.android";
import type { GameCardProps } from "./game-card-types";

export { getGameCardData } from "./game-card-data";
export type { GameCardData } from "./game-card-data";

export type { GameCardProps, GameCardVariant } from "./game-card-types";

export function GameCard({ title, metadata, imageUrl, variant, href, cardWidth }: GameCardProps) {
  const router = useRouter();
  const showCopy = variant !== "cover";
  const copyHeight = variant === "grid" ? 92 : GAME_RAIL_COPY_HEIGHT;
  const cardModifiers = cardWidth ? [width(cardWidth)] : [fillMaxWidth()];

  return (
    <Card elevation={0} modifiers={[...cardModifiers, clickable(() => router.push(href))]}>
      <Column modifiers={[fillMaxWidth()]}>
        <GameArtwork
          imageUrl={imageUrl}
          fallbackLabel={title.slice(0, 1)}
          modifiers={[fillMaxWidth()]}
          fallbackHeight={cardWidth ? cardWidth / 0.72 : 180}
          imageStyle={{ width: cardWidth, height: cardWidth ? cardWidth / 0.72 : 180 }}
        />
        {showCopy && metadata ? (
          <Column
            modifiers={[
              height(copyHeight),
              paddingAll(variant === "rail" ? spacing.xs : spacing.md),
            ]}
            verticalArrangement={{ spacedBy: spacing.xxs }}
          >
            <Text
              maxLines={2}
              style={{
                typography: variant === "rail" ? "titleSmall" : "titleMedium",
                fontWeight: "700",
              }}
            >
              {title}
            </Text>
            <Text maxLines={1} style={{ typography: "bodySmall" }}>
              {metadata}
            </Text>
          </Column>
        ) : null}
      </Column>
    </Card>
  );
}

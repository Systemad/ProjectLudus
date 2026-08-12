import { Card, Column, Row, Text } from "@expo/ui/jetpack-compose";
import {
  clickable,
  fillMaxWidth,
  height,
  paddingAll,
  weight,
  width,
} from "@expo/ui/jetpack-compose/modifiers";
import { type Href, useRouter } from "expo-router";

import { GameArtwork } from "@/entities/game/game-artwork.android";
import { getGameCardImage } from "@/entities/game/game-image";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";

type BrowseListCardProps = {
  game: GameBrowseDto;
  rank: number;
  href: Href;
};

function formatPlayerCount(value: string | null | undefined) {
  if (value === null || value === undefined) {
    return "—";
  }

  return Number(value).toLocaleString();
}

export function BrowseListCard({ game, rank, href }: BrowseListCardProps) {
  const colors = useAppTheme();
  const router = useRouter();
  const imageUrl = getGameCardImage(game);
  const genre = game.gameFeatures.genres[0]?.name ?? game.gameFeatures.themes[0]?.name ?? "Game";

  return (
    <Card
      colors={{ containerColor: colors.surfaceHigh, contentColor: colors.text }}
      elevation={0}
      modifiers={[fillMaxWidth(), clickable(() => router.push(href))]}
    >
      <Row
        modifiers={[fillMaxWidth(), paddingAll(8)]}
        verticalAlignment="top"
        horizontalArrangement={{ spacedBy: 12 }}
      >
        <GameArtwork
          imageUrl={imageUrl}
          modifiers={[width(76), height(104)]}
          fallbackAlpha={0.7}
          imageStyle={{ width: 76, height: 104 }}
        />

        <Column modifiers={[weight(1), height(104)]} verticalArrangement="spaceBetween">
          <Column verticalArrangement={{ spacedBy: 3 }}>
            <Row
              modifiers={[fillMaxWidth()]}
              verticalAlignment="top"
              horizontalArrangement="spaceBetween"
            >
              <Text
                color={colors.text as string}
                maxLines={2}
                overflow="ellipsis"
                modifiers={[weight(1)]}
                style={{ typography: "titleMedium", fontWeight: "700" }}
              >
                {game.name}
              </Text>
              <Text
                color={colors.textMuted as string}
                style={{ typography: "labelLarge", fontWeight: "800" }}
              >
                #{rank}
              </Text>
            </Row>
            <Text
              color={colors.textMuted as string}
              maxLines={1}
              overflow="ellipsis"
              style={{ typography: "bodyMedium" }}
            >
              {genre}
            </Text>
          </Column>

          <Row modifiers={[fillMaxWidth()]} horizontalArrangement={{ spacedBy: 16 }}>
            <Column modifiers={[weight(1)]} verticalArrangement={{ spacedBy: 1 }}>
              <Text
                color={colors.textMuted as string}
                style={{ typography: "labelSmall", fontWeight: "700" }}
              >
                PLAYING NOW
              </Text>
              <Text
                color={colors.text as string}
                style={{ typography: "titleMedium", fontWeight: "700" }}
              >
                {formatPlayerCount(game.steam?.currentPlayers)}
              </Text>
            </Column>
            <Column modifiers={[weight(1)]} verticalArrangement={{ spacedBy: 1 }}>
              <Text
                color={colors.textMuted as string}
                style={{ typography: "labelSmall", fontWeight: "700" }}
              >
                PEAK TODAY
              </Text>
              <Text
                color={colors.text as string}
                style={{ typography: "titleMedium", fontWeight: "700" }}
              >
                {formatPlayerCount(game.steam?.peak24h)}
              </Text>
            </Column>
          </Row>
        </Column>
      </Row>
    </Card>
  );
}

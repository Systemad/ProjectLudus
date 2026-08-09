import { Card, Column, RNHostView, Text } from "@expo/ui/jetpack-compose";
import { clickable, fillMaxWidth, paddingAll } from "@expo/ui/jetpack-compose/modifiers";
import { Image } from "expo-image";
import { type Href, useRouter } from "expo-router";

import { getGameCardImage } from "@/entities/game/game-image";
import { formatSteamReviewRating, getSteamReviewEmoji } from "@/entities/game/steam-review";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";

type BrowseGameCardProps = {
  game: GameBrowseDto;
  href: Href;
};

export function BrowseGameCard({ game, href }: BrowseGameCardProps) {
  const colors = useAppTheme();
  const router = useRouter();
  const imageUrl = getGameCardImage(game);
  const genre = game.gameFeatures.genres[0]?.name ?? "Game";
  const year = game.firstReleaseDate?.slice(0, 4) ?? "TBA";
  const rating = formatSteamReviewRating(game.review);
  const metadata = [
    year,
    genre,
    rating === "N/A" ? undefined : `${getSteamReviewEmoji(game.review)} ${rating}`,
  ]
    .filter((value): value is string => value !== undefined)
    .join(" · ");

  return (
    <Card
      colors={{ containerColor: colors.surfaceHigh, contentColor: colors.text }}
      elevation={0}
      modifiers={[fillMaxWidth(0.5), clickable(() => router.push(href))]}
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
        ) : null}
        <Column modifiers={[paddingAll(12)]} verticalArrangement={{ spacedBy: 4 }}>
          <Text
            color={colors.text}
            maxLines={2}
            style={{ typography: "titleSmall", fontWeight: "700" }}
          >
            {game.name}
          </Text>
          <Text color={colors.textMuted} maxLines={1} style={{ typography: "bodySmall" }}>
            {metadata}
          </Text>
        </Column>
      </Column>
    </Card>
  );
}

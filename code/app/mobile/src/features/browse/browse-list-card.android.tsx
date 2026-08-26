import { Box, Card, Column, Icon, Image, RNHostView, Row, Text } from "@expo/ui/jetpack-compose";
import {
  clickable,
  clip,
  fillMaxHeight,
  fillMaxSize,
  fillMaxWidth,
  height,
  padding,
  weight,
  width,
} from "@expo/ui/jetpack-compose/modifiers";
import { Image as NativeImage } from "expo-image";
import { type Href, useRouter } from "expo-router";
import { Defs, RadialGradient, Rect, Stop, Svg } from "react-native-svg";
import { StyleSheet, View } from "react-native";

import { getGameCardImage } from "@/entities/game/game-image";
import { formatSteamReviewRating } from "@/entities/game/steam-review";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { radius, spacing } from "@/theme";

type BrowseListCardProps = {
  game: GameBrowseDto;
  rank: number;
  href: Href;
};

export function BrowseListCard({ game, rank, href }: BrowseListCardProps) {
  const colors = useAppTheme();
  const router = useRouter();
  const imageUrl = getGameCardImage(game);
  const backdropUrl = game.steam?.headerUrl;
  const genres =
    [...game.gameFeatures.genres, ...game.gameFeatures.themes]
      .slice(0, 2)
      .map((feature) => feature.name)
      .join(", ") || "Game";
  const details = [game.firstReleaseDate?.slice(0, 4) ?? "TBA"].join(" · ");
  const rating = formatSteamReviewRating(game.steam?.review);

  return (
    <Card
      colors={{ containerColor: colors.surfaceHigh, contentColor: colors.text }}
      elevation={0}
      modifiers={[
        fillMaxWidth(),
        height(126),
        clip({ type: "roundedCorner", radius: radius.md }),
        clickable(() => router.push(href)),
      ]}
    >
      <Box modifiers={[fillMaxSize()]}>
        {backdropUrl ? (
          <RNHostView modifiers={[fillMaxSize()]} style={StyleSheet.absoluteFill}>
            <View style={StyleSheet.absoluteFill}>
              <NativeImage
                source={backdropUrl}
                contentFit="cover"
                contentPosition="right center"
                style={[StyleSheet.absoluteFill, { opacity: 0.46, zIndex: 0 }]}
              />
              <View
                style={[
                  StyleSheet.absoluteFill,
                  { backgroundColor: "rgba(20, 18, 24, 0.08)", zIndex: 1 },
                ]}
              />
              <Svg style={[StyleSheet.absoluteFill, { zIndex: 2 }]}>
                <Defs>
                  <RadialGradient
                    id={`browse-card-fade-${game.id}`}
                    cx="100%"
                    cy="0%"
                    r={280}
                    gradientUnits="userSpaceOnUse"
                  >
                    <Stop offset="0%" stopColor="#141218" stopOpacity={0.26} />
                    <Stop offset="46%" stopColor="#141218" stopOpacity={0.58} />
                    <Stop offset="100%" stopColor="#141218" stopOpacity={1} />
                  </RadialGradient>
                </Defs>
                <Rect width="100%" height="100%" fill={`url(#browse-card-fade-${game.id})`} />
              </Svg>
            </View>
          </RNHostView>
        ) : null}

        <Row
          modifiers={[fillMaxSize(), padding(spacing.xxs, spacing.xxs, spacing.xxs, spacing.xxs)]}
          verticalAlignment="top"
          horizontalArrangement={{ spacedBy: spacing.sm }}
        >
          <Card
            colors={{ containerColor: colors.surface, contentColor: colors.text }}
            elevation={0}
            modifiers={[width(76), height(118), clip({ type: "roundedCorner", radius: radius.md })]}
          >
            {imageUrl ? (
              <Image
                source={{ uri: imageUrl }}
                contentScale="crop"
                contentDescription={`${game.name} poster`}
                modifiers={[fillMaxSize()]}
              />
            ) : (
              <Box contentAlignment="center" modifiers={[fillMaxSize()]}>
                <Text style={{ typography: "titleLarge" }}>{game.name.slice(0, 1)}</Text>
              </Box>
            )}
          </Card>

          <Column modifiers={[weight(1), fillMaxHeight()]} verticalArrangement="spaceBetween">
            <Row modifiers={[fillMaxWidth()]} verticalAlignment="top">
              <Text
                color={colors.text}
                maxLines={1}
                overflow="ellipsis"
                modifiers={[weight(1)]}
                style={{ typography: "titleMedium", fontWeight: "700" }}
              >
                {game.name}
              </Text>
              <Row verticalAlignment="top" horizontalArrangement={{ spacedBy: 4 }}>
                <Text color={colors.text} style={{ typography: "labelLarge", fontWeight: "800" }}>
                  #{rank}
                </Text>
                <Icon
                  source={require("@/assets/icons/more_vert.xml")}
                  tint={colors.text}
                  size={24}
                  contentDescription={`More options for ${game.name}`}
                />
              </Row>
            </Row>

            <Column verticalArrangement={{ spacedBy: 2 }}>
              <Text color={colors.textMuted} maxLines={1} overflow="ellipsis">
                {genres}
              </Text>
              <Row modifiers={[fillMaxWidth()]} verticalAlignment="bottom">
                <Text
                  color={colors.text}
                  maxLines={1}
                  overflow="ellipsis"
                  modifiers={[weight(1)]}
                  style={{ typography: "bodyMedium", fontWeight: "600" }}
                >
                  {details}
                </Text>
                {rating !== "N/A" ? (
                  <Text
                    color={colors.primary}
                    style={{ typography: "titleSmall", fontWeight: "800" }}
                  >
                    ★ {rating}
                  </Text>
                ) : null}
              </Row>
            </Column>
          </Column>
        </Row>
      </Box>
    </Card>
  );
}

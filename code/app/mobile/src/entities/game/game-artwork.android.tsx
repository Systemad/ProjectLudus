import { Box, Card, RNHostView, Text } from "@expo/ui/jetpack-compose";
import { alpha, background, type ModifierConfig, height } from "@expo/ui/jetpack-compose/modifiers";
import { Image } from "expo-image";
import type { ImageStyle } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";

export type GameArtworkProps = {
  imageUrl?: string;
  fallbackLabel?: string;
  modifiers: ModifierConfig[];
  fallbackHeight?: number;
  fallbackAlpha?: number;
  contentFit?: "cover" | "contain";
  imageStyle: ImageStyle;
};

export function GameArtwork({
  imageUrl,
  fallbackLabel = "?",
  modifiers,
  fallbackHeight,
  fallbackAlpha = 1,
  contentFit = "cover",
  imageStyle,
}: GameArtworkProps) {
  const colors = useAppTheme();

  if (imageUrl) {
    return (
      <Card colors={{ containerColor: "transparent", contentColor: colors.text }} elevation={0}>
        <RNHostView matchContents modifiers={modifiers}>
          <Image source={imageUrl} style={imageStyle} contentFit={contentFit} />
        </RNHostView>
      </Card>
    );
  }

  return (
    <Card colors={{ containerColor: colors.surfaceHigh, contentColor: colors.text }} elevation={0}>
      <Box
        contentAlignment="center"
        modifiers={[
          ...modifiers,
          background(colors.surfaceHigh),
          ...(fallbackHeight === undefined ? [] : [height(fallbackHeight)]),
          ...(fallbackAlpha === 1 ? [] : [alpha(fallbackAlpha)]),
        ]}
      >
        <Text color={colors.textMuted as string} style={{ typography: "titleLarge" }}>
          {fallbackLabel}
        </Text>
      </Box>
    </Card>
  );
}

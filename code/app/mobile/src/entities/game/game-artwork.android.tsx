import { Box, RNHostView, Text } from "@expo/ui/jetpack-compose";
import {
  alpha,
  background,
  clip,
  type ModifierConfig,
  height,
  Shapes,
} from "@expo/ui/jetpack-compose/modifiers";
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
  cornerRadius?: number;
  imageStyle: ImageStyle;
};

export function GameArtwork({
  imageUrl,
  fallbackLabel = "?",
  modifiers,
  fallbackHeight,
  fallbackAlpha = 1,
  contentFit = "cover",
  cornerRadius = 12,
  imageStyle,
}: GameArtworkProps) {
  const colors = useAppTheme();
  const artworkModifiers = [...modifiers, clip(Shapes.RoundedCorner(cornerRadius))];

  if (imageUrl) {
    return (
      <RNHostView matchContents modifiers={artworkModifiers}>
        <Image source={imageUrl} style={imageStyle} contentFit={contentFit} />
      </RNHostView>
    );
  }

  return (
    <Box
      contentAlignment="center"
      modifiers={[
        ...artworkModifiers,
        background(colors.surfaceHigh),
        ...(fallbackHeight === undefined ? [] : [height(fallbackHeight)]),
        ...(fallbackAlpha === 1 ? [] : [alpha(fallbackAlpha)]),
      ]}
    >
      <Text color={colors.textMuted as string} style={{ typography: "titleLarge" }}>
        {fallbackLabel}
      </Text>
    </Box>
  );
}

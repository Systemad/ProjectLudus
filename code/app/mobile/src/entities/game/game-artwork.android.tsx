import { Box, Card, RNHostView, Text } from "@expo/ui/jetpack-compose";
import { alpha, type ModifierConfig, height } from "@expo/ui/jetpack-compose/modifiers";
import { Image } from "expo-image";
import type { ImageStyle } from "react-native";

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
  if (imageUrl) {
    return (
      <Card colors={{ containerColor: "transparent" }} elevation={0}>
        <RNHostView matchContents modifiers={modifiers}>
          <Image source={imageUrl} style={imageStyle} contentFit={contentFit} />
        </RNHostView>
      </Card>
    );
  }

  return (
    <Card elevation={0}>
      <Box
        contentAlignment="center"
        modifiers={[
          ...modifiers,
          ...(fallbackHeight === undefined ? [] : [height(fallbackHeight)]),
          ...(fallbackAlpha === 1 ? [] : [alpha(fallbackAlpha)]),
        ]}
      >
        <Text style={{ typography: "titleLarge" }}>{fallbackLabel}</Text>
      </Box>
    </Card>
  );
}

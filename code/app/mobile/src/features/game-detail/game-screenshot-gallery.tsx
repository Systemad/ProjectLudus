import { Image } from "expo-image";
import { useState } from "react";
import { ActivityIndicator, ScrollView, StyleSheet, Text, View } from "react-native";

import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";

export function GameScreenshotGallery({ screenshotIds }: { screenshotIds: string[] }) {
  const colors = useAppTheme();
  const screenshots = screenshotIds.flatMap((imageId, index) => {
    const thumbnailUrl = getIgdbImageUrl(imageId, "screenshot_med");
    if (!thumbnailUrl) return [];
    return [{ key: `${imageId}-${index}`, thumbnailUrl }];
  });

  if (screenshots.length === 0) return null;

  return (
    <ScrollView
      horizontal
      showsHorizontalScrollIndicator={false}
      contentContainerStyle={styles.content}
    >
      {screenshots.map((screenshot, index) => (
        <Screenshot
          key={screenshot.key}
          colors={colors}
          index={index}
          source={screenshot.thumbnailUrl}
          total={screenshots.length}
        />
      ))}
    </ScrollView>
  );
}

function Screenshot({
  colors,
  index,
  source,
  total,
}: {
  colors: ReturnType<typeof useAppTheme>;
  index: number;
  source: string;
  total: number;
}) {
  const [state, setState] = useState<"loading" | "loaded" | "error">("loading");

  return (
    <View style={[styles.frame, { backgroundColor: colors.surfaceHigh }]}>
      {state !== "error" ? (
        <Image
          accessible
          accessibilityLabel={`Screenshot ${index + 1} of ${total}`}
          source={source}
          style={styles.image}
          contentFit="cover"
          transition={180}
          onLoad={() => setState("loaded")}
          onError={() => setState("error")}
        />
      ) : null}
      {state === "loading" ? (
        <View style={styles.overlay}>
          <ActivityIndicator color={colors.primary} />
        </View>
      ) : null}
      {state === "error" ? (
        <View style={styles.overlay}>
          <Text style={[styles.fallback, { color: colors.textMuted }]}>Image unavailable</Text>
        </View>
      ) : null}
    </View>
  );
}

const styles = StyleSheet.create({
  content: {
    gap: 12,
    paddingRight: 32,
  },
  frame: {
    aspectRatio: 16 / 9,
    borderRadius: 16,
    overflow: "hidden",
    width: 300,
  },
  image: {
    height: "100%",
    width: "100%",
  },
  overlay: {
    alignItems: "center",
    ...StyleSheet.absoluteFill,
    justifyContent: "center",
  },
  fallback: {
    fontSize: 13,
    fontWeight: "700",
  },
});

import { Galeria } from "@nandorojo/galeria";
import { Image } from "expo-image";
import { useState } from "react";
import { ActivityIndicator, StyleSheet, Text, View } from "react-native";

import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";

export function GameScreenshotGallery({ screenshotIds }: { screenshotIds: string[] }) {
  const colors = useAppTheme();
  const screenshots = screenshotIds.flatMap((imageId, index) => {
    const thumbnailUrl = getIgdbImageUrl(imageId, "screenshot_med");
    const imageUrl = getIgdbImageUrl(imageId, "screenshot_huge");
    if (!thumbnailUrl || !imageUrl) return [];
    return [{ key: `${imageId}-${index}`, imageUrl, thumbnailUrl }];
  });

  if (screenshots.length === 0) return null;

  return (
    <Galeria urls={screenshots.map((screenshot) => screenshot.imageUrl)} theme="dark">
      <View style={styles.grid}>
        {screenshots.map((screenshot, index) => (
          <Galeria.Image key={screenshot.key} index={index} style={styles.item}>
            <Screenshot
              colors={colors}
              index={index}
              source={screenshot.thumbnailUrl}
              total={screenshots.length}
            />
          </Galeria.Image>
        ))}
      </View>
    </Galeria>
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
  grid: {
    flexDirection: "row",
    flexWrap: "wrap",
    gap: 12,
  },
  item: {
    aspectRatio: 16 / 9,
    borderRadius: 16,
    overflow: "hidden",
    width: "48%",
  },
  frame: {
    height: "100%",
    width: "100%",
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

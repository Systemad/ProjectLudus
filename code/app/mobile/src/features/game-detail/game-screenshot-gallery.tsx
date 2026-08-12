import { Galeria } from "@nandorojo/galeria";
import { Image } from "expo-image";
import { useWindowDimensions, View } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { useAppTheme } from "@/hooks/use-app-theme";

const GRID_GAP = 10;

type GameScreenshotGalleryProps = {
  screenshotIds: string[];
};

export function GameScreenshotGallery({ screenshotIds }: GameScreenshotGalleryProps) {
  const colors = useAppTheme();
  const { width } = useWindowDimensions();
  const cardWidth = (width - PAGE_GUTTER * 2 - GRID_GAP) / 2;
  const screenshots = screenshotIds.flatMap((imageId, index) => {
    const thumbnailUrl = getIgdbImageUrl(imageId, "screenshot_med");
    const viewerUrl = getIgdbImageUrl(imageId, "screenshot_huge");

    if (!thumbnailUrl || !viewerUrl) return [];

    return [
      {
        key: `${imageId}-${index}`,
        thumbnailUrl,
        viewerUrl,
      },
    ];
  });
  const viewerUrls = screenshots.map((screenshot) => screenshot.viewerUrl);

  if (screenshots.length === 0) return null;

  return (
    <Galeria urls={viewerUrls} theme="dark">
      <View style={{ flexDirection: "row", flexWrap: "wrap", gap: GRID_GAP }}>
        {screenshots.map((screenshot, index) => (
          <Galeria.Image
            index={index}
            key={screenshot.key}
            style={{ width: cardWidth, aspectRatio: 16 / 9 }}
          >
            <Image
              accessible
              accessibilityRole="button"
              accessibilityLabel={`Open screenshot ${index + 1} of ${screenshots.length}`}
              accessibilityHint="Opens the screenshot viewer"
              source={screenshot.thumbnailUrl}
              style={{
                width: cardWidth,
                aspectRatio: 16 / 9,
                backgroundColor: colors.surfaceHigh,
                borderRadius: 16,
              }}
              contentFit="cover"
              transition={180}
            />
          </Galeria.Image>
        ))}
      </View>
    </Galeria>
  );
}

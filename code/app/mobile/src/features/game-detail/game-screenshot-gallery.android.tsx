import { Host } from "@expo/ui";
import { Card, FlowRow, RNHostView } from "@expo/ui/jetpack-compose";
import {
  clip,
  fillMaxWidth,
  Shapes,
  width as widthModifier,
} from "@expo/ui/jetpack-compose/modifiers";
import { Galeria } from "@nandorojo/galeria";
import { Image } from "expo-image";
import { useWindowDimensions } from "react-native";

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
      <Host matchContents={{ vertical: true }} seedColor={colors.primary} style={{ width: "100%" }}>
        <FlowRow
          modifiers={[fillMaxWidth()]}
          horizontalArrangement={{ spacedBy: GRID_GAP }}
          verticalArrangement={{ spacedBy: GRID_GAP }}
        >
          {screenshots.map((screenshot, index) => (
            <Card
              colors={{ containerColor: colors.surfaceHigh, contentColor: colors.text }}
              elevation={0}
              key={screenshot.key}
              modifiers={[widthModifier(cardWidth)]}
            >
              <RNHostView
                matchContents
                modifiers={[fillMaxWidth(), clip(Shapes.RoundedCorner(16))]}
              >
                <Galeria.Image index={index} style={{ width: "100%", aspectRatio: 16 / 9 }}>
                  <Image
                    accessible
                    accessibilityLabel={`Open screenshot ${index + 1} of ${screenshots.length}`}
                    accessibilityHint="Opens the screenshot viewer"
                    source={screenshot.thumbnailUrl}
                    style={{ width: "100%", aspectRatio: 16 / 9 }}
                    contentFit="cover"
                    transition={180}
                  />
                </Galeria.Image>
              </RNHostView>
            </Card>
          ))}
        </FlowRow>
      </Host>
    </Galeria>
  );
}

import { Card, HorizontalPager, RNHostView } from "@expo/ui/jetpack-compose";
import { clip, fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";
import { Galeria } from "@nandorojo/galeria";
import { Image } from "expo-image";

import { getIgdbImageUrl } from "@/entities/game/game-image";

export function GameScreenshotGallery({ screenshotIds }: { screenshotIds: string[] }) {
  const screenshots = screenshotIds.flatMap((imageId, index) => {
    const thumbnailUrl = getIgdbImageUrl(imageId, "screenshot_med");
    const viewerUrl = getIgdbImageUrl(imageId, "screenshot_huge");
    if (!thumbnailUrl || !viewerUrl) return [];
    return [{ key: `${imageId}-${index}`, thumbnailUrl, viewerUrl }];
  });
  const viewerUrls = screenshots.map((screenshot) => screenshot.viewerUrl);

  if (screenshots.length === 0) return null;

  return (
    <Galeria urls={viewerUrls} theme="dark">
      <HorizontalPager modifiers={[fillMaxWidth()]} pageSpacing={12}>
        {screenshots.map((screenshot, index) => (
          <Card elevation={0} key={screenshot.key} modifiers={[fillMaxWidth()]}>
            <RNHostView
              matchContents
              modifiers={[fillMaxWidth(), clip({ type: "roundedCorner", radius: 16 })]}
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
      </HorizontalPager>
    </Galeria>
  );
}

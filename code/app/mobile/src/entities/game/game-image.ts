import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";

export type IgdbImageSize =
  | "cover_small"
  | "screenshot_med"
  | "cover_big"
  | "logo_med"
  | "screenshot_big"
  | "screenshot_huge"
  | "thumb"
  | "micro"
  | "720p"
  | "1080p";

export function getIgdbImageUrl(
  imageId: string | null | undefined,
  size: IgdbImageSize = "thumb",
  retina = false,
): string | undefined {
  if (!imageId) {
    return undefined;
  }

  if (imageId.startsWith("http://") || imageId.startsWith("https://")) {
    return imageId;
  }

  const rendition = retina ? `${size}_2x` : size;

  return `https://images.igdb.com/igdb/image/upload/t_${rendition}/${imageId}.jpg`;
}

export function getGameCardImage(game: GameBrowseDto) {
  return getIgdbImageUrl(game.coverUrl, "cover_big") ?? game.steam?.capsuleUrl ?? undefined;
}

export type ImageSize =
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

export function getIGDBImageUrl(
    imageId: string,
    size: ImageSize = "thumb",
    retina: boolean = false
): string {
    const finalSize = retina ? `${size}_2x` : size;
    return `https://images.igdb.com/igdb/image/upload/t_${finalSize}/${imageId}.jpg`;
}

import { Image } from "@mantine/core";
import type { ImageProps } from "@mantine/core";

type Props = {
    imageId: string;
    size?: ImageSize;
    height?: number;
    retina?: boolean;
};
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

export function IGDBImage({
    imageId,
    size = "thumb",
    height = 200,
    retina = false,
    ...rest
}: Props & ImageProps) {
    const { className, ...other } = rest;

    const finalSize = retina ? `${size}_2x` : size;
    const url = `https://images.igdb.com/igdb/image/upload/t_${finalSize}/${imageId}.jpg`;
    return <Image src={url} radius={"lg"} height={height} {...other}></Image>;
}

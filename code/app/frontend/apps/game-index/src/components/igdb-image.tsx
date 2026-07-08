import { getIGDBImageUrl, type ImageSize } from "@src/utils/ImageHelper";
import type { ImgHTMLAttributes } from "react";

type Props = {
    imageId: string;
    imageSize?: ImageSize;
    retina?: boolean;
};

export function IGDBImage({
    imageId,
    imageSize = "thumb",
    retina = false,
    ...rest
}: Props & ImgHTMLAttributes<HTMLImageElement>) {
    const { ...other } = rest;

    const url = getIGDBImageUrl(imageId, imageSize, retina);

    return <img src={url} style={{ borderRadius: "0.5rem" }} {...other} />;
}

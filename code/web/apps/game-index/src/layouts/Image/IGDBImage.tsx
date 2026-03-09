import type { ImageProps } from "@packages/ui";
import { Image } from "@packages/ui";
import { getIGDBImageUrl, type ImageSize } from "../../utils/ImageHelper";

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
}: Props & ImageProps) {
    const { ...other } = rest;

    const url = getIGDBImageUrl(imageId, imageSize, retina);

    return <Image src={url} borderRadius={"lg"} {...other}></Image>;
}

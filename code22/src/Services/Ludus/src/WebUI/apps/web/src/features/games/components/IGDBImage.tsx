import type { ImageProps } from "@yamada-ui/react";
import { Image } from "@yamada-ui/react";
import {
    getIGDBImageUrl,
    type ImageSize,
} from "~/features/utilities/ImageHelper";

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

    return <Image src={url} radius={"lg"} {...other}></Image>;
}

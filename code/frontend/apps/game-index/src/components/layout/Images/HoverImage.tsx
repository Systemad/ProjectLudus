"use client";
import { getIGDBImageUrl, type ImageSize } from "@src/utils/ImageHelper";
import { Image, Box, useHover } from "ui";

type Props = {
    src: string;
    size?: ImageSize;
    alt: string;
    fallback?: string;
};

export function HoverImage({ src, size = "thumb", alt }: Props) {
    const { hovered, ref } = useHover();
    const imageUrl = getIGDBImageUrl(src, size);
    return (
        <Box
            ref={ref}
            position="relative"
            borderRadius="lg"
            overflow="hidden"
            boxShadow={hovered ? "lg" : "md"}
            transition="all 0.3s ease-in-out"
            w="full"
            h="full"
        >
            <Image
                src={imageUrl}
                alt={alt}
                w="full"
                objectFit="cover"
                display="block"
                transition="transform 0.3s ease-in-out"
                _hover={{ transform: "scale(1.1)" }}
            />
        </Box>
    );
}

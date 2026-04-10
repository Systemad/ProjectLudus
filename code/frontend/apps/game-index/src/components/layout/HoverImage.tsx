"use client";
import { Image, Box, useHover } from "ui";

type Props = {
    src: string;
    alt: string;
    fallback?: string;
};

export default function HoverImage({ src, alt }: Props) {
    const { hovered, ref } = useHover();

    return (
        <Box
            ref={ref}
            position="relative"
            borderRadius="md"
            overflow="hidden"
            boxShadow={hovered ? "lg" : "md"}
            transition="all 0.3s ease-in-out"
            w="full"
        >
            <Image
                src={src}
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

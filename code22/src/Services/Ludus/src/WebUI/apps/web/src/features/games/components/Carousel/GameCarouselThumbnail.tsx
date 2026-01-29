import type { MediaItem } from "./GameCarousel";
import { IGDBImage } from "../IGDBImage";
import { Image, Box } from "@yamada-ui/react";

type Props = {
    media: MediaItem;
    selected: boolean;
    onClick: () => void;
};

export function GameCarouselThumbnail({ media, onClick }: Props) {
    if (media.type === "screenshot" || media.type === "artwork") {
        return (
            <IGDBImage
                imageId={media.imageId}
                rounded="xl"
                imageSize="screenshot_med"
                borderRadius="lg"
                onClick={onClick}
            />
        );
    }

    if (media.type === "video") {
        return (
            <Box
                onClick={onClick}
                position="relative"
                w="full"
                h="200px"
                bg="gray.200"
            >
                <Image h="2xs" src={media.thumbnail} />
                <Box
                    position="absolute"
                    top="50%"
                    left="50%"
                    transform="translate(-50%, -50%)"
                    zIndex={10}
                    w="25px"
                    h="25px"
                    bg="red.500"
                    borderRadius="md"
                >
                    ▶
                </Box>
            </Box>
        );
    }

    return null;
}

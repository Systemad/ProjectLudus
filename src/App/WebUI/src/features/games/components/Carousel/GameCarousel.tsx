import { IGDBImage } from "../IGDBImage";
import { Box } from "@yamada-ui/react";
import { Carousel, CarouselSlide } from "@yamada-ui/carousel";
import { useState } from "react";
export type MediaItem =
    | { type: "screenshot"; id: number; imageId: string }
    | { type: "artwork"; id: number; imageId: string }
    | { type: "video"; id: number; videoId: string; thumbnail: string };

type CarouselProps = {
    items: MediaItem[];
};

export function GameCarousel({ items }: CarouselProps) {
    const [index, setIndex] = useState<number>(0);

    const handleThumbnailClick = (clickedIndex: number) => {
        setIndex(clickedIndex);
    };

    return (
        <>
            {/* Main carousel */}
            <Carousel
                rounded="xl"
                gap={4}
                height={"xl"}
                withControls
                withIndicators={false}
                index={index}
                onChange={setIndex}
            >
                {items.map((item) => {
                    if (item.type === "screenshot" || item.type === "artwork") {
                        return (
                            <CarouselSlide
                                key={item.id}
                                as={IGDBImage}
                                imageId={item.imageId}
                                bg="primary"
                                rounded="xl"
                                imageSize="1080p"
                            />
                        );
                    } else if (item.type === "video") {
                        return (
                            <CarouselSlide key={item.id}>
                                <div
                                    className="relative w-full h-full cursor-pointer flex items-center justify-center bg-black"
                                    onClick={() =>
                                        window.open(
                                            `https://youtu.be/${item.videoId}`,
                                            "_blank"
                                        )
                                    }
                                >
                                    <img
                                        src={item.thumbnail}
                                        alt="Video thumbnail"
                                        className="object-cover w-full h-full rounded-xl"
                                    />
                                    <div className="absolute flex items-center justify-center">
                                        <div className="rounded bg-black bg-opacity-50 p-2">
                                            ▶
                                        </div>
                                    </div>
                                </div>
                            </CarouselSlide>
                        );
                    }
                })}
            </Carousel>

            {/* Thumbnail carousel */}
            <Box
                w="auto"
                rounded="xl"
                mt="sm"
                overflow="hidden"
                bg={["blackAlpha.50", "whiteAlpha.100"]}
            >
                <Carousel
                    index={index}
                    containScroll="trimSnaps"
                    slideSize="15%"
                    align="center"
                    dragFree
                    h="4xs"
                    loop={false}
                    gap={2}
                    withIndicators={false}
                    withControls={false}
                >
                    {items.map((item, idx) => {
                        if (
                            item.type === "screenshot" ||
                            item.type === "artwork"
                        ) {
                            return (
                                <CarouselSlide
                                    key={item.id}
                                    as={IGDBImage}
                                    imageId={item.imageId}
                                    rounded="xl"
                                    imageSize="screenshot_med"
                                    borderRadius="lg"
                                    onClick={() => handleThumbnailClick(idx)}
                                />
                            );
                        } else if (item.type === "video") {
                            return (
                                <CarouselSlide
                                    key={item.id}
                                    onClick={() => handleThumbnailClick(idx)}
                                >
                                    <Box
                                        position="relative"
                                        w="full"
                                        h="200px"
                                        bg="gray.200"
                                    >
                                        {/* This could be the "background" content */}
                                        <Box w="full" h="full" bg="blue.300" />

                                        {/* Small box in the center on top */}
                                        <Box
                                            position="absolute"
                                            top="50%"
                                            left="50%"
                                            transform="translate(-50%, -50%)"
                                            zIndex={10} // ensures it's on top
                                            w="100px"
                                            h="100px"
                                            bg="red.500"
                                            borderRadius="md"
                                        >
                                            Center Box
                                        </Box>
                                    </Box>
                                </CarouselSlide>
                            );
                        }
                    })}
                </Carousel>
            </Box>
        </>
    );
}

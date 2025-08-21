import { IGDBImage } from "../IGDBImage";
import { Box } from "@yamada-ui/react";
import { Carousel, CarouselSlide } from "@yamada-ui/carousel";
import { useState } from "react";
import { VideCarouselSlide } from "./VideCarouselSlide";

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
            <Carousel
                rounded="xl"
                gap={4}
                height={"xl"}
                containScroll={"keepSnaps"}
                dragFree={true}
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
                                <VideCarouselSlide
                                    iconSize={"5xs"}
                                    url={item.thumbnail}
                                />
                            </CarouselSlide>
                        );
                    }
                })}
            </Carousel>

            <Box w="auto" rounded="xl" mt="sm" overflow="hidden">
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
                                    <VideCarouselSlide url={item.thumbnail} />
                                </CarouselSlide>
                            );
                        }
                    })}
                </Carousel>
            </Box>
        </>
    );
}

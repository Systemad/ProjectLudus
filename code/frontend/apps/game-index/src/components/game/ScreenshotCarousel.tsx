"use client";

import {
    AspectRatio,
    Box,
    Carousel,
    Center,
    ChevronLeftIcon,
    ChevronRightIcon,
    CircleIcon,
    dataAttr,
    Image,
    VStack,
} from "ui";
import { useEffect, useRef, useState } from "react";

export interface ScreenshotCarouselProps {
    sources: string[];
    autoplay?: boolean;
    autoplayDelay?: number;
    loop?: boolean;
}

export default function ScreenshotCarousel({
    sources,
    autoplay = true,
    autoplayDelay = 3500,
    loop = true,
}: ScreenshotCarouselProps) {
    return (
        <VStack align="stretch" gap={0} py={0}>
            <Box rounded="2xl" overflow="hidden" p={0} position="relative">
                <Carousel.Root
                    colorScheme={"white"}
                    defaultIndex={0}
                    autoplay={autoplay}
                    delay={autoplayDelay}
                    loop={loop}
                >
                    <Carousel.List>
                        {sources.map((src, idx) => (
                            <Carousel.Item key={`${src}-${idx}`} index={idx}>
                                <Image
                                    src={src}
                                    alt={`screenshot ${idx + 1}`}
                                    objectFit="cover"
                                    boxSize="full"
                                    display="block"
                                />
                            </Carousel.Item>
                        ))}
                    </Carousel.List>

                    {sources.length > 1 && (
                        <>
                            <Carousel.PrevTrigger
                                variant="ghost"
                                position="absolute"
                                left="3"
                                top="50%"
                                transform="translateY(-50%)"
                                rounded="full"
                                p="2"
                                minW="10"
                                aria-label="Previous"
                            >
                                <ChevronLeftIcon boxSize="5" />
                            </Carousel.PrevTrigger>

                            <Carousel.NextTrigger
                                variant="ghost"
                                position="absolute"
                                right="3"
                                top="50%"
                                transform="translateY(-50%)"
                                rounded="full"
                                p="2"
                                minW="10"
                                aria-label="Next"
                            >
                                <ChevronRightIcon boxSize="5" />
                            </Carousel.NextTrigger>
                        </>
                    )}
                    <Carousel.Indicators />
                </Carousel.Root>
            </Box>
        </VStack>
    );
}

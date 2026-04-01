"use client";
import { Box, Button, Carousel, Heading, HStack, Image, Text, VStack } from "ui";
import { useState } from "react";
import type { Game } from "../../data/games";
import { games } from "../../data/games";

function CarouselCard({ game, index, isActive }: { game: Game; index: number; isActive: boolean }) {
    return (
        <Carousel.Item index={index} slideSize={isActive ? "100%" : undefined}>
            <Box position="relative" rounded="xl" overflow="hidden">
                <Image
                    src={game.heroImage || game.image}
                    alt={game.title}
                    boxSize="full"
                    objectFit="cover"
                />

                {isActive && (
                    <Box position="absolute" inset={0} zIndex="kurillin" pointerEvents="none">
                        <Box
                            position="absolute"
                            inset="0"
                            bgGradient="linear(to-t, rgba(2,6,23,0.72) 8%, transparent 55%)"
                        />
                        <Box
                            position="absolute"
                            inset="0"
                            bgGradient="linear(to-r, rgba(2,6,23,0.8), transparent 65%)"
                        />

                        <VStack
                            align="stretch"
                            gap="4"
                            maxW="4xl"
                            position="absolute"
                            bottom={{ base: "6", md: "10" }}
                            left={{ base: "6", md: "12" }}
                            pointerEvents="auto"
                        >
                            <Text color="yellow.300" fontSize="sm" fontWeight="bold">
                                {game.category}
                            </Text>

                            <Heading
                                fontFamily="heading"
                                fontWeight="black"
                                lineHeight="1"
                                letterSpacing="tight"
                                fontSize={{ base: "3xl", md: "5xl", xl: "6xl" }}
                                color="white"
                            >
                                {game.title}
                            </Heading>

                            {game.subtitle && (
                                <Text color="whiteAlpha.800" fontSize={{ base: "sm", md: "md" }}>
                                    {game.subtitle}
                                </Text>
                            )}

                            <HStack gap="3" wrap="wrap">
                                {game.genres.slice(0, 3).map((g) => (
                                    <Text
                                        key={g}
                                        fontSize="sm"
                                        color="white"
                                        bg="rgba(0,0,0,0.36)"
                                        px="3"
                                        py="1"
                                        rounded="full"
                                    >
                                        {g}
                                    </Text>
                                ))}

                                {game.themes.slice(0, 2).map((t) => (
                                    <Text
                                        key={t}
                                        fontSize="sm"
                                        color="white"
                                        bg="rgba(0,0,0,0.36)"
                                        px="3"
                                        py="1"
                                        rounded="full"
                                    >
                                        {t}
                                    </Text>
                                ))}

                                {game.platforms.slice(0, 4).map((p) => (
                                    <Text
                                        key={p}
                                        fontSize="sm"
                                        color="white"
                                        bg="rgba(0,0,0,0.36)"
                                        px="3"
                                        py="1"
                                        rounded="full"
                                    >
                                        {p}
                                    </Text>
                                ))}

                                <Text
                                    fontSize="sm"
                                    color="black"
                                    bg="yellow.300"
                                    px="3"
                                    py="1"
                                    rounded="full"
                                >
                                    ★ {game.rating}
                                </Text>
                            </HStack>

                            <HStack gap="4">
                                <Button
                                    size="lg"
                                    bg="yellow.400"
                                    color="black"
                                    px="6"
                                    rounded="2xl"
                                >
                                    View
                                </Button>
                                <Button
                                    size="lg"
                                    variant="outline"
                                    rounded="2xl"
                                    px="6"
                                    borderColor="rgba(255,255,255,0.16)"
                                    color="white"
                                >
                                    View
                                </Button>
                            </HStack>
                        </VStack>
                    </Box>
                )}
            </Box>
        </Carousel.Item>
    );
}

export function SpotlightHero() {
    const [activeIndex, setActiveIndex] = useState(0);
    const slides = games;

    return (
        <Box pt={{ base: "6", md: "8" }}>
            <Box rounded="xl" overflow="hidden" boxShadow="lg" position="relative">
                <Carousel.Root
                    slideSize="50%"
                    align="center"
                    defaultIndex={0}
                    onChange={(i) => setActiveIndex(i)}
                    autoplay
                    delay={10000}
                    loop
                    stopMouseEnterAutoplay={true}
                    aria-label="Spotlight carousel"
                >
                    <Carousel.List>
                        {slides.map((game: Game, index) => (
                            <CarouselCard
                                key={game.id}
                                game={game}
                                index={index}
                                isActive={index === activeIndex}
                            />
                        ))}
                    </Carousel.List>

                    <Carousel.Indicators />
                </Carousel.Root>

                {/* overlay moved into CarouselCard */}
            </Box>
        </Box>
    );
}

export default SpotlightHero;

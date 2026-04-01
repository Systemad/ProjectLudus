"use client";
import { Link } from "@tanstack/react-router";
import { Box, Flex, Grid, Heading, Image, SimpleGrid, Text, VStack } from "ui";
import { highestRated, spotlightIndies } from "../../data/games";
import { GameCard } from "../GameCard";

const linkStyle = { color: "inherit", textDecoration: "none" };
const ratedCardLinkStyle = {
    color: "inherit",
    textDecoration: "none",
    display: "block",
    width: "100%",
} as const;

export function SpotlightRatedSection() {
    return (
        <Grid
            templateColumns={{
                base: "1fr",
                xl: "minmax(0, 3fr) minmax(360px, 2fr)",
            }}
            gap={{ base: "12", lg: "14" }}
            alignItems="start"
        >
            <VStack align="stretch" gap="6">
                <Flex justify="space-between" align="center">
                    <Heading fontFamily="heading" fontSize={{ base: "2xl", md: "3xl" }}>
                        Spotlight Indies
                    </Heading>
                    <Link to="/search" style={linkStyle}>
                        <Text
                            color="colorScheme.solid"
                            fontWeight="bold"
                            fontSize="sm"
                            textTransform="uppercase"
                            letterSpacing="widest"
                            _hover={{ opacity: 0.8 }}
                        >
                            See more
                        </Text>
                    </Link>
                </Flex>

                <SimpleGrid columns={{ base: 2, sm: 2, xl: 4 }} gap={{ base: 4, md: 5 }}>
                    {spotlightIndies.map((game) => (
                        <GameCard key={game.id} game={game} showSubtitle={false} />
                    ))}
                </SimpleGrid>
            </VStack>

            <VStack align="stretch" gap="6">
                <Heading fontFamily="heading" fontSize={{ base: "2xl", md: "3xl" }}>
                    Highest Rated
                </Heading>

                <VStack align="stretch" gap="3">
                    {highestRated.map((game) => (
                        <Link
                            key={game.id}
                            to="/games/$gameId"
                            params={{ gameId: game.id }}
                            style={ratedCardLinkStyle}
                        >
                            <Grid
                                templateColumns="48px minmax(0, 1fr) auto"
                                gap="3"
                                alignItems="center"
                                px="4"
                                py="4"
                                bg="bg.panel"
                                rounded="2xl"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                _hover={{ bg: "bg.subtle" }}
                            >
                                <Box
                                    w="12"
                                    h="12"
                                    minW="12"
                                    rounded="xl"
                                    overflow="hidden"
                                    flexShrink={0}
                                    bg="bg.subtle"
                                >
                                    <Image
                                        src={game.image}
                                        alt=""
                                        w="full"
                                        h="full"
                                        objectFit="cover"
                                    />
                                </Box>

                                <VStack align="stretch" gap="0.5" minW="0">
                                    <Text
                                        fontWeight="bold"
                                        fontSize="sm"
                                        color="fg.base"
                                        whiteSpace="nowrap"
                                        overflow="hidden"
                                        textOverflow="ellipsis"
                                    >
                                        {game.title}
                                    </Text>
                                    {game.subtitle ? (
                                        <Text
                                            color="fg.subtle"
                                            fontSize="xs"
                                            whiteSpace="nowrap"
                                            overflow="hidden"
                                            textOverflow="ellipsis"
                                        >
                                            {game.subtitle}
                                        </Text>
                                    ) : null}
                                </VStack>

                                <VStack align="end" gap="0" minW="12">
                                    <Text
                                        fontFamily="heading"
                                        fontWeight="black"
                                        fontSize="xl"
                                        color="colorScheme.solid"
                                        lineHeight="1"
                                    >
                                        {game.score}
                                    </Text>
                                    <Text
                                        fontSize="xs"
                                        color="fg.subtle"
                                        textTransform="uppercase"
                                        letterSpacing="wider"
                                    >
                                        Meta
                                    </Text>
                                </VStack>
                            </Grid>
                        </Link>
                    ))}
                </VStack>
            </VStack>
        </Grid>
    );
}

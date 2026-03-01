"use client";

import { createFileRoute } from "@tanstack/react-router";
import {
    Box,
    Image,
    Flex,
    Grid,
    Tag,
    Text,
    Heading,
    Button,
    GridItem,
    useBoolean,
    DataList,
} from "@packages/ui";
import { useMemo } from "react";
export const Route = createFileRoute("/games/$gameId")({
    component: RouteComponent,
});

function RouteComponent() {
    return <Banner />;
}
const Banner = () => {
    const [open, { toggle }] = useBoolean();

    const longText = `Call of Duty features both singleplayer and multiplayer modes. During the singleplayer campaign, a briefing is held or a slide-show with commentary before every mission. In order to complete a level, the player has to complete several objectives in a chronological order. There are four difficulty levels in Call of Duty, they differ in the amount of damage the player can sustain. There is no health regeneration system present in the game. A health pack system is used instead.
The players can carry two main weapons, one pistol, and 10 grenades. The players can melee any enemy they encounter in the game.`;

    const items = useMemo<DataList.Item[]>(
        () => [
            { description: "Valve", term: "Main Developer" },
            { description: "Valve", term: "Publisher" },
            { description: "Source 2", term: "Game Engine" },

            { description: "21 August", term: "Release date" },
            { description: "Shooter, Action", term: "Genre" },
            { description: "Open-world", term: "Themes" },
            { description: "Yes", term: "Multiplayer" },
        ],
        []
    );

    return (
        <Grid templateColumns={{ base: "1fr", md: "2fr 1fr" }} gap="md">
            <GridItem>
                <Box
                    position="relative"
                    w="full"
                    rounded="lg"
                    overflow="hidden"
                >
                    {/* Background Image */}
                    <Image
                        src="https://images.igdb.com/igdb/image/upload/t_1080p/ar1481.webp"
                        objectFit="cover"
                        w="full"
                        h="full"
                        position="absolute"
                        inset="0"
                        zIndex={0}
                    />

                    {/* Gradient Overlay */}
                    <Box
                        position="absolute"
                        inset="0"
                        bgGradient="linear(to-r, blackAlpha.900 0%, blackAlpha.700 45%, transparent 100%)"
                        zIndex={1}
                    />

                    {/* Content (THIS defines height) */}
                    <Flex
                        position="relative"
                        zIndex={2}
                        p={{ base: "md", md: "lg" }}
                        gap={{ base: "md", md: "lg" }}
                        align="center"
                    >
                        {/* Cover */}
                        <Box
                            flexShrink={0}
                            w={{ base: "90px", md: "140px" }}
                            aspectRatio="2 / 3"
                            rounded="md"
                            overflow="hidden"
                            boxShadow="lg"
                        >
                            <Image
                                src="https://images.igdb.com/igdb/image/upload/t_cover_big/co5r8l.webp"
                                objectFit="cover"
                                w="full"
                                h="full"
                                alt="Game Cover"
                            />
                        </Box>

                        {/* Right Side */}
                        <Flex
                            direction="column"
                            justify="center"
                            gap="sm"
                            color="white"
                            minW="0"
                        >
                            <Flex gap="xs" wrap="wrap">
                                <Tag
                                    size="sm"
                                    bg="whiteAlpha.200"
                                    backdropFilter="blur(8px)"
                                    rounded="md"
                                    textShadow={"0 2px 6px rgba(0,0,0,0.8)"}
                                >
                                    PC
                                </Tag>
                                <Tag
                                    size="sm"
                                    bg="whiteAlpha.200"
                                    backdropFilter="blur(8px)"
                                    rounded="md"
                                    textShadow={"0 2px 6px rgba(0,0,0,0.8)"}
                                >
                                    PlayStation 5
                                </Tag>
                            </Flex>

                            <Heading
                                size={{ base: "md", md: "lg" }}
                                textShadow="0 2px 8px rgba(0,0,0,0.6)"
                            >
                                Clair Obscur: Expedition 33
                            </Heading>

                            <Flex gap="sm">
                                <Button size="xs">Add to library</Button>
                                <Button size="xs" variant="surface">
                                    Something
                                </Button>
                            </Flex>
                        </Flex>
                    </Flex>
                </Box>
                <Box
                    transition={"all 0.3s ease-in-out"}
                    mt="md"
                    p="md"
                    bg={"gray.800/40"}
                    rounded="xl"
                >
                    <Heading as="h3" size="2xl">
                        About
                    </Heading>

                    <Box borderWidth={0} p="0" rounded="l2" w="full">
                        <Text fontSize="sm" lineClamp={open ? 0 : 4} mb="sm">
                            {longText}
                        </Text>
                    </Box>

                    <Button size="xs" variant="ghost" mt="xs" onClick={toggle}>
                        {open ? "Read less" : "Read more"}
                    </Button>
                </Box>
            </GridItem>

            <GridItem>
                <Box bg={"gray.800/40"} rounded="xl" p="md">
                    <DataList.Root
                        h="full"
                        w="full"
                        variant="bold"
                        items={items}
                    />
                </Box>
            </GridItem>
        </Grid>
    );
};

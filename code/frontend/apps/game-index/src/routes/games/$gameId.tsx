"use client";
import { createFileRoute, Link } from "@tanstack/react-router";
import { useExtractColors } from "react-extract-colors";
import {
    Box,
    Button,
    Gamepad2Icon,
    GithubIcon,
    GlobeIcon,
    Grid,
    Heading,
    HStack,
    Icon,
    SimpleGrid,
    Tag,
    Text,
    UserIcon,
    VStack,
    Wrap,
} from "ui";
import AmbientBackground from "../../components/game/AmbientBackground";
import Hero from "../../components/game/Hero";
import ScreenshotCarousel from "../../components/game/ScreenshotCarousel";
import SimilarGamesSection from "../../components/game/SimilarGamesSection";
import Card from "../../components/ui/Card";
import { getGameById, getGamesByIds } from "../../data/games";
import { buildAmbientGradient } from "../../utils/ambientGradient";

export const Route = createFileRoute("/games/$gameId")({
    component: GameDetailPage,
});

const linkStyle = { color: "inherit", textDecoration: "none" };
const sectionLabelStyle = {
    fontSize: "sm",
    fontWeight: "semibold",
    color: "fg.emphasized",
    textTransform: "uppercase" as const,
    letterSpacing: "wide",
};
const sectionMetaStyle = {
    fontSize: "xs",
    fontWeight: "semibold",
    color: "fg.emphasized",
    textTransform: "uppercase" as const,
    letterSpacing: "wide",
};

function GameDetailPage() {
    const { gameId } = Route.useParams();
    const game = getGameById(gameId);

    const sourceImage = game?.heroImage ?? game?.coverImage ?? "";
    const { dominantColor, darkerColor, lighterColor } = useExtractColors(sourceImage, {
        format: "hex",
        maxColors: 4,
        maxSize: 120,
        colorSimilarityThreshold: 45,
        sortBy: "vibrance",
    });
    const pageAmbientGradient = buildAmbientGradient({
        dominantColor,
        darkerColor,
        lighterColor,
    });

    const screenshotSources = game?.screenshots ?? [];
    const similarGames = getGamesByIds(game?.similarGameIds || []);

    if (!game) {
        return (
            <Box maxW="7xl" mx="auto" px={{ base: "4", md: "6", xl: "8" }} py="20">
                <VStack align="center" gap="6">
                    <Heading fontFamily="heading">Game not found</Heading>
                    <Link to="/" style={linkStyle}>
                        <Button>Back to Home</Button>
                    </Link>
                </VStack>
            </Box>
        );
    }

    return (
        <Box w="full" color="fg.base" position="relative" isolation="isolate">
            <AmbientBackground gradient={pageAmbientGradient} />

            <Box position="relative" zIndex={1}>
                <Hero game={game} />

                <Box maxW="6xl" mx="auto" px={{ base: 4, md: 6 }} pb="20">
                    <Grid templateColumns={{ base: "1fr", md: "1fr 320px" }} gap={10}>
                        <VStack align="stretch" gap={6}>
                            <Card p={8}>
                                <Heading color="fg.base" size="md" mb={4}>
                                    The Ancient Kingdom Awaits
                                </Heading>
                                <Text color="fg.subtle" lineHeight="normal" fontSize="lg">
                                    {game.summary}
                                </Text>
                            </Card>

                            {screenshotSources.length > 0 && (
                                <ScreenshotCarousel sources={screenshotSources} />
                            )}

                            <SimpleGrid columns={{ base: 1, md: 2 }} gap={6}>
                                <Card variant="translucent" p={6}>
                                    <Text {...sectionLabelStyle} mb={4}>
                                        Development
                                    </Text>
                                    <VStack align="stretch" gap={4}>
                                        <HStack justify="space-between">
                                            <Text color="fg.subtle">Developer</Text>
                                            <Text
                                                fontSize="sm"
                                                fontWeight="medium"
                                                color="fg.emphasized"
                                            >
                                                Team Cherry
                                            </Text>
                                        </HStack>
                                        <HStack justify="space-between">
                                            <Text color="fg.subtle">Publisher</Text>
                                            <Text
                                                fontSize="sm"
                                                fontWeight="medium"
                                                color="fg.emphasized"
                                            >
                                                Team Cherry
                                            </Text>
                                        </HStack>
                                        <HStack justify="space-between">
                                            <Text color="fg.subtle">Release Date</Text>
                                            <Text
                                                fontSize="sm"
                                                fontWeight="medium"
                                                color="fg.emphasized"
                                            >
                                                Feb 24, 2017
                                            </Text>
                                        </HStack>
                                    </VStack>
                                </Card>

                                <Card variant="translucent" p={6}>
                                    <Text {...sectionLabelStyle} mb={4}>
                                        Genres & Themes
                                    </Text>
                                    <Text {...sectionMetaStyle} mb={2}>
                                        GENRES
                                    </Text>
                                    <Wrap mb={4} gap="xs">
                                        {["Action", "Adventure", "Indie"].map((item) => (
                                            <Tag
                                                key={item}
                                                variant="surface"
                                                colorScheme="gray"
                                                color="fg.base"
                                                textTransform="none"
                                                px="sm"
                                                py="2xs"
                                            >
                                                {item}
                                            </Tag>
                                        ))}
                                    </Wrap>
                                    <Text {...sectionMetaStyle} mb={2}>
                                        THEMES
                                    </Text>
                                    <Wrap gap="xs">
                                        {["Metroidvania", "Atmospheric", "Difficult"].map(
                                            (item) => (
                                                <Tag
                                                    key={item}
                                                    variant="surface"
                                                    colorScheme="gray"
                                                    color="fg.base"
                                                    textTransform="none"
                                                    px="sm"
                                                    py="2xs"
                                                >
                                                    {item}
                                                </Tag>
                                            ),
                                        )}
                                    </Wrap>
                                </Card>
                            </SimpleGrid>
                        </VStack>

                        {/* Sidebar Column */}
                        <VStack align="stretch" gap={6}>
                            <Card p={6}>
                                <VStack align="stretch" gap={6}>
                                    <Box>
                                        <Text {...sectionLabelStyle} mb={3}>
                                            Consoles
                                        </Text>
                                        <Wrap gap="md">
                                            <Tag
                                                variant="surface"
                                                colorScheme="gray"
                                                size="md"
                                                rounded="lg"
                                                startIcon={<GithubIcon />}
                                                px="sm"
                                                py="2xs"
                                                color="fg.base"
                                            >
                                                Github
                                            </Tag>
                                        </Wrap>
                                    </Box>

                                    <Box>
                                        <Text {...sectionLabelStyle} mb={3}>
                                            Multiplayer Modes
                                        </Text>
                                        <HStack bg="bg.panel" p={3} rounded="xl" justify="center">
                                            <Icon as={UserIcon} color="yellow.500" />
                                            <Text fontWeight="bold" fontSize="sm">
                                                Single-player
                                            </Text>
                                        </HStack>
                                    </Box>

                                    <Box>
                                        <Text {...sectionLabelStyle} mb={3}>
                                            Official Links
                                        </Text>
                                        <VStack align="stretch" gap={2}>
                                            <Button
                                                variant="ghost"
                                                colorScheme="gray"
                                                color="fg.base"
                                                justifyContent="space-between"
                                                endIcon={<span>›</span>}
                                                size="sm"
                                                _hover={{ bg: "bg.subtle", color: "fg.base" }}
                                            >
                                                <HStack>
                                                    <Icon as={Gamepad2Icon} color="fg.muted" />
                                                    <Text>Steam Store</Text>
                                                </HStack>
                                            </Button>
                                            <Button
                                                variant="ghost"
                                                colorScheme="gray"
                                                color="fg.base"
                                                justifyContent="space-between"
                                                endIcon={<span>›</span>}
                                                size="sm"
                                                _hover={{ bg: "bg.subtle", color: "fg.base" }}
                                            >
                                                <HStack>
                                                    <Icon as={GlobeIcon} color="fg.muted" />{" "}
                                                    <Text>Official Website</Text>
                                                </HStack>
                                            </Button>
                                            <Button
                                                variant="ghost"
                                                colorScheme="gray"
                                                color="fg.base"
                                                justifyContent="space-between"
                                                endIcon={<span>›</span>}
                                                size="sm"
                                                _hover={{ bg: "bg.subtle", color: "fg.base" }}
                                            >
                                                <HStack>
                                                    <Icon as={Gamepad2Icon} color="fg.muted" />{" "}
                                                    <Text>Epic Games Store</Text>
                                                </HStack>
                                            </Button>
                                        </VStack>
                                    </Box>

                                    <Box
                                        bg="bg.info"
                                        p={4}
                                        rounded="xl"
                                        border="1px solid"
                                        borderColor="border.info"
                                    >
                                        <HStack>
                                            <Icon as={Gamepad2Icon} color="fg.info" />
                                            <VStack align="start" gap={0}>
                                                <Text fontSize="xs" fontWeight="bold">
                                                    PC Master Race
                                                </Text>
                                                <Text fontSize="xs" color="fg.subtle">
                                                    OPTIMIZATION GOLD
                                                </Text>
                                            </VStack>
                                        </HStack>
                                    </Box>
                                </VStack>
                            </Card>

                            <Card p={6}>
                                <HStack gap={2} mb={4} align="center">
                                    <Icon as={Gamepad2Icon} />
                                    <Text {...sectionLabelStyle}>System Requirements</Text>
                                </HStack>
                                <VStack align="stretch" gap={4} fontSize="xs">
                                    <Box>
                                        <Text {...sectionMetaStyle} mb={1}>
                                            MINIMUM SPECS
                                        </Text>
                                        <Text color="fg.subtle">• OS: Windows 7</Text>
                                        <Text color="fg.subtle">
                                            • Processor: Intel Core 2 Duo E5200
                                        </Text>
                                        <Text color="fg.subtle">• Memory: 4 GB RAM</Text>
                                    </Box>
                                    <Box>
                                        <Text {...sectionMetaStyle} mb={1}>
                                            RECOMMENDED SPECS
                                        </Text>
                                        <Text color="fg.subtle">• OS: Windows 10</Text>
                                        <Text color="fg.subtle">• Processor: Intel Core i5</Text>
                                        <Text color="fg.subtle">• Memory: 8 GB RAM</Text>
                                    </Box>
                                </VStack>
                            </Card>
                        </VStack>
                    </Grid>

                    <SimilarGamesSection games={similarGames} />
                </Box>
            </Box>
        </Box>
    );
}

export default GameDetailPage;

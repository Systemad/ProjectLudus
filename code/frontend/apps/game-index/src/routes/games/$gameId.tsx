"use client";
import { createFileRoute, Link } from "@tanstack/react-router";
import { useExtractColors } from "react-extract-colors";
import { useMemo } from "react";
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
import Card from "../../components/ui/Card";
import { buildAmbientGradient } from "../../utils/ambientGradient";
import { type GameDto, useGetApiGamesGameidSuspenseHook } from "@src/gen/catalogApi";
import { formatReleaseDate } from "@src/utils/formatReleaseDate";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

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
    const numericGameId = Number(gameId);
    const { data } = useGetApiGamesGameidSuspenseHook({ gameId: numericGameId });
    const game = data?.game as GameDto | undefined;

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

    const coverImage = game.coverUrl ? getIGDBImageUrl(game.coverUrl, "cover_big") : "";
    const sourceImage = coverImage;
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

    const screenshotSources = useMemo(
        () =>
            (game.screenshots ?? [])
                .map((screenshot) =>
                    screenshot.url ? getIGDBImageUrl(screenshot.url, "screenshot_big") : "",
                )
                .filter((src) => src.length > 0),
        [game.screenshots],
    );

    const genres = game.genres ?? [];
    const themes = game.themes ?? [];
    const platforms = game.platforms ?? [];
    const modes = game.gameModes ?? game.playerPerspectives ?? [];
    const rating = game.aggregatedRating ?? game.rating;
    const websites = game.websites?.filter((website) => website.url) ?? [];

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
                                    {game.storyline ? "Storyline" : "Overview"}
                                </Heading>
                                <Text color="fg.subtle" lineHeight="normal" fontSize="lg">
                                    {game.storyline ?? game.summary ?? "No description available."}
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
                                                Unknown
                                            </Text>
                                        </HStack>
                                        <HStack justify="space-between">
                                            <Text color="fg.subtle">Publisher</Text>
                                            <Text
                                                fontSize="sm"
                                                fontWeight="medium"
                                                color="fg.emphasized"
                                            >
                                                Unknown
                                            </Text>
                                        </HStack>
                                        <HStack justify="space-between">
                                            <Text color="fg.subtle">Release Date</Text>
                                            <Text
                                                fontSize="sm"
                                                fontWeight="medium"
                                                color="fg.emphasized"
                                            >
                                                {formatReleaseDate(game.firstReleaseDate)}
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
                                        {genres.map((item) => (
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
                                        {themes.map((item) => (
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
                                </Card>
                            </SimpleGrid>
                        </VStack>

                        <VStack align="stretch" gap={6}>
                            <Card p={6}>
                                <VStack align="stretch" gap={6}>
                                    <Box>
                                        <Text {...sectionLabelStyle} mb={3}>
                                            Consoles
                                        </Text>
                                        <Wrap gap="md">
                                            {platforms.map((platform) => (
                                                <Tag
                                                    key={platform}
                                                    variant="surface"
                                                    colorScheme="gray"
                                                    size="md"
                                                    rounded="lg"
                                                    startIcon={<GithubIcon />}
                                                    px="sm"
                                                    py="2xs"
                                                    color="fg.base"
                                                >
                                                    {platform}
                                                </Tag>
                                            ))}
                                        </Wrap>
                                    </Box>

                                    <Box>
                                        <Text {...sectionLabelStyle} mb={3}>
                                            Multiplayer Modes
                                        </Text>
                                        <HStack bg="bg.panel" p={3} rounded="xl" justify="center">
                                            <Icon as={UserIcon} color="yellow.500" />
                                            <Text fontWeight="bold" fontSize="sm">
                                                {modes.join(", ") || "Not specified"}
                                            </Text>
                                        </HStack>
                                    </Box>

                                    <Box>
                                        <Text {...sectionLabelStyle} mb={3}>
                                            Official Links
                                        </Text>
                                        <VStack align="stretch" gap={2}>
                                            {websites.length > 0 ? (
                                                websites.map((website) => (
                                                    <Button
                                                        key={website.id}
                                                        as="a"
                                                        href={website.url ?? undefined}
                                                        target="_blank"
                                                        rel="noreferrer"
                                                        variant="ghost"
                                                        colorScheme="gray"
                                                        color="fg.base"
                                                        justifyContent="space-between"
                                                        endIcon={<span>›</span>}
                                                        size="sm"
                                                        _hover={{
                                                            bg: "bg.subtle",
                                                            color: "fg.base",
                                                        }}
                                                    >
                                                        <HStack>
                                                            <Icon
                                                                as={
                                                                    website.typeName
                                                                        ?.toLowerCase()
                                                                        .includes("official")
                                                                        ? GlobeIcon
                                                                        : Gamepad2Icon
                                                                }
                                                                color="fg.muted"
                                                            />
                                                            <Text>
                                                                {website.typeName ??
                                                                    "Official Link"}
                                                            </Text>
                                                        </HStack>
                                                    </Button>
                                                ))
                                            ) : (
                                                <Text color="fg.subtle" fontSize="sm">
                                                    No official links available.
                                                </Text>
                                            )}
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
                                                    {game.gameStatusName ?? "Game status"}
                                                </Text>
                                                <Text fontSize="xs" color="fg.subtle">
                                                    {typeof rating === "number"
                                                        ? `${rating.toFixed(1)} aggregated rating`
                                                        : "Rating unavailable"}
                                                </Text>
                                            </VStack>
                                        </HStack>
                                    </Box>
                                </VStack>
                            </Card>

                            <Card p={6}>
                                <HStack gap={2} mb={4} align="center">
                                    <Icon as={Gamepad2Icon} />
                                    <Text {...sectionLabelStyle}>Release Details</Text>
                                </HStack>
                                <VStack align="stretch" gap={4} fontSize="xs">
                                    <Box>
                                        <Text {...sectionMetaStyle} mb={1}>
                                            TYPE
                                        </Text>
                                        <Text color="fg.subtle">
                                            {game.gameTypeName ?? "Unspecified"}
                                        </Text>
                                    </Box>
                                    <Box>
                                        <Text {...sectionMetaStyle} mb={1}>
                                            RELEASE DATE
                                        </Text>
                                        <Text color="fg.subtle">
                                            {formatReleaseDate(game.firstReleaseDate)}
                                        </Text>
                                    </Box>
                                </VStack>
                            </Card>
                        </VStack>
                    </Grid>
                </Box>
            </Box>
        </Box>
    );
}

export default GameDetailPage;

"use client";
import { createFileRoute, Link } from "@tanstack/react-router";
import { useState } from "react";
import { Box, Button, Grid, Heading, HStack, Image, Tag, Text, VStack, Wrap } from "ui";
import MediaGrid from "@src/components/game/MediaGrid";
import AlternativeNames from "@src/components/game/AlternativeNames";
import { OfficialLinks } from "@src/components/game/OfficialLinks";
import OverviewPanel from "@src/components/game/OverviewPanel";
import RelatedGamesSection from "@src/components/game/RelatedGamesSection";
import { ScreenshotPreview } from "@src/components/game/ScreenshotPreview";
import { linkStyle, sectionLabelStyle, sectionMetaStyle } from "@src/utils/sectionTextStyles";
import { type GameDto, useGetApiGamesGameidSuspenseHook } from "@src/gen/catalogApi";
import { formatReleaseDate } from "@src/utils/formatReleaseDate";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Tab = "overview" | "official-links" | "media" | "details";

const TABS: { value: Tab; label: string }[] = [
    { value: "overview", label: "Overview" },
    { value: "official-links", label: "Official links" },
    { value: "media", label: "Media" },
    { value: "details", label: "Details" },
];

export const Route = createFileRoute("/games/$gameId")({
    component: GameDetailPage,
});

function GameDetailPage() {
    const { gameId } = Route.useParams();
    const { data } = useGetApiGamesGameidSuspenseHook({ gameId: Number(gameId) });
    const game = data?.game as GameDto | undefined;
    const [activeTab, setActiveTab] = useState<Tab>("overview");

    if (!game) {
        return (
            <Box maxW="7xl" mx="auto" px={{ base: "4", md: "6" }} py="20">
                <VStack align="center" gap="6">
                    <Heading>Game not found</Heading>
                    <Link to="/" style={linkStyle}>
                        <Button>Back to Home</Button>
                    </Link>
                </VStack>
            </Box>
        );
    }

    const coverImageId = game.cover != null ? String(game.cover) : (game.coverUrl ?? "");
    const coverImage = coverImageId ? getIGDBImageUrl(coverImageId, "cover_big") : "";

    const screenshotSources = (game.screenshots ?? [])
        .map((s) => getIGDBImageUrl(s.imageId, "screenshot_med"))
        .filter((src) => src.length > 0);

    const genres = game.genres ?? [];
    const themes = game.themes ?? [];
    const platforms = game.platforms ?? [];
    const modes = game.gameModes ?? [];
    const playerPerspectives = game.playerPerspectives ?? [];
    const websites = game.websites?.filter((w) => w.url) ?? [];
    const videos = game.videos?.filter((v) => v.videoId) ?? [];
    const alternativeNames = (game.alternativeNames ?? [])
        .map((item) => item.name)
        .filter((item): item is string => Boolean(item));
    const relatedGames = (game.similarGames ?? [])
        .map((item) => {
            const coverUrl = item?.coverUrl ?? "";
            const imageUrl = coverUrl.startsWith("http")
                ? coverUrl
                : item?.cover != null
                  ? getIGDBImageUrl(String(item.cover), "cover_big")
                  : coverUrl
                    ? getIGDBImageUrl(coverUrl, "cover_big")
                    : undefined;
            return {
                id: item?.id ?? -1,
                name: item?.name ?? "",
                imageUrl,
            };
        })
        .filter((item) => item.id > 0 && item.name.length > 0);
    const developers = (game.involvedCompanies ?? []).filter((c) => c.developed);
    const publishers = (game.involvedCompanies ?? []).filter((c) => c.published);

    return (
        <Box w="full" color="fg.base">
            <Box position="relative" overflow="hidden">
                {coverImage && (
                    <Image
                        src={coverImage}
                        alt=""
                        position="absolute"
                        inset={0}
                        w="full"
                        h="full"
                        objectFit="cover"
                        filter="blur(28px) brightness(0.3)"
                        transform="scale(1.1)"
                        pointerEvents="none"
                    />
                )}
                <Box position="relative" maxW="6xl" mx="auto" px={{ base: 4, md: 6 }} py={8}>
                    <HStack align="flex-start" gap={{ base: 4, md: 6 }}>
                        <Box
                            flexShrink={0}
                            w={{ base: "90px", md: "120px" }}
                            rounded="xl"
                            overflow="hidden"
                            boxShadow="lg"
                        >
                            <Image
                                src={coverImage || undefined}
                                alt={game.name ?? "Cover"}
                                objectFit="cover"
                                w="full"
                                display="block"
                            />
                        </Box>

                        <VStack align="start" gap={2} flex={1} minW={0}>
                            <Text color="fg.muted" fontSize="sm">
                                {formatReleaseDate(game.firstReleaseDate)}
                            </Text>
                            <Heading
                                size={{ base: "2xl", md: "4xl" }}
                                color="fg.base"
                                lineHeight="1.1"
                            >
                                {game.name ?? "Untitled game"}
                            </Heading>
                            <Wrap gap="xs">
                                {genres.slice(0, 3).map((g) => (
                                    <Tag
                                        key={g}
                                        variant="subtle"
                                        colorScheme="gray"
                                        size="sm"
                                        textTransform="none"
                                    >
                                        {g}
                                    </Tag>
                                ))}
                            </Wrap>
                            <HStack gap={6} mt={1} flexWrap="wrap">
                                {developers.length > 0 && (
                                    <Box>
                                        <Text {...sectionMetaStyle} fontSize="xs" mb="2xs">
                                            DEVELOPER
                                        </Text>
                                        <Text color="fg.subtle" fontSize="sm">
                                            {developers.map((c) => c.name).join(", ")}
                                        </Text>
                                    </Box>
                                )}
                                {publishers.length > 0 && (
                                    <Box>
                                        <Text {...sectionMetaStyle} fontSize="xs" mb="2xs">
                                            PUBLISHER
                                        </Text>
                                        <Text color="fg.subtle" fontSize="sm">
                                            {publishers.map((c) => c.name).join(", ")}
                                        </Text>
                                    </Box>
                                )}
                            </HStack>
                            <Text color="fg.subtle" fontSize="sm" maxW="2xl" lineClamp={3} mt={1}>
                                {game.summary ?? game.storyline ?? ""}
                            </Text>
                        </VStack>
                    </HStack>
                </Box>
            </Box>

            <Box borderBottomWidth="1px" borderColor="border.subtle">
                <Box maxW="6xl" mx="auto" px={{ base: 4, md: 6 }}>
                    <HStack gap={0}>
                        {TABS.map((tab) => (
                            <Button
                                key={tab.value}
                                variant="ghost"
                                colorScheme="gray"
                                color={activeTab === tab.value ? "fg.base" : "fg.muted"}
                                borderBottomWidth="2px"
                                borderColor={activeTab === tab.value ? "fg.base" : "transparent"}
                                borderRadius="0"
                                px={4}
                                py={3}
                                h="auto"
                                fontSize="sm"
                                fontWeight={activeTab === tab.value ? "semibold" : "normal"}
                                onClick={() => setActiveTab(tab.value)}
                                _hover={{ color: "fg.base", bg: "transparent" }}
                            >
                                {tab.label}
                            </Button>
                        ))}
                    </HStack>
                </Box>
            </Box>

            <Box maxW="6xl" mx="auto" px={{ base: 4, md: 6 }} py={8} pb={20}>
                <Box display={activeTab === "overview" ? "block" : "none"}>
                    <Grid
                        templateColumns={{ base: "1fr", lg: "1fr 300px" }}
                        gap={{ base: 8, lg: 10 }}
                        alignItems="start"
                    >
                        <VStack align="stretch" gap={8}>
                            <Box>
                                <Text {...sectionLabelStyle} mb={3}>
                                    Story
                                </Text>
                                <Text color="fg.subtle" lineHeight="tall">
                                    {game.storyline ?? game.summary ?? "No description available."}
                                </Text>
                            </Box>
                            <ScreenshotPreview
                                sources={screenshotSources}
                                visible
                                onViewAll={() => setActiveTab("media")}
                            />
                            <RelatedGamesSection games={relatedGames} />
                        </VStack>

                        <OverviewPanel
                            gameTypeName={game.gameTypeName}
                            modes={modes}
                            playerPerspectives={playerPerspectives}
                            platforms={platforms}
                            genres={genres}
                            themes={themes}
                        />
                    </Grid>
                </Box>

                <Box display={activeTab === "official-links" ? "block" : "none"}>
                    <OfficialLinks websites={websites} />
                </Box>

                <Box display={activeTab === "media" ? "block" : "none"}>
                    <MediaGrid sources={screenshotSources} videos={videos} />
                </Box>

                <Box display={activeTab === "details" ? "block" : "none"}>
                    <VStack align="stretch" gap={8}>
                        <OfficialLinks websites={websites} />
                        <AlternativeNames names={alternativeNames} />
                    </VStack>
                </Box>
            </Box>
        </Box>
    );
}

export default GameDetailPage;

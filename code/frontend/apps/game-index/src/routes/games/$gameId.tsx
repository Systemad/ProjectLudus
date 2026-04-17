"use client";
import { createFileRoute, Link } from "@tanstack/react-router";
import { Suspense, useState } from "react";
import {
    Box,
    Button,
    Grid,
    Heading,
    HStack,
    Image,
    Loading,
    SegmentedControl,
    Tag,
    Text,
    VStack,
    Wrap,
    For,
} from "ui";
import MediaGrid from "@src/components/game/MediaGrid";
import AlternativeNames from "@src/components/game/AlternativeNames";
import { GameReleaseDates } from "@src/components/game/GameReleaseDates";
import { GameStory } from "@src/components/game/GameStory";
import { OfficialLinks } from "@src/components/game/OfficialLinks";
import OverviewPanel from "@src/components/game/OverviewPanel";
import { RelatedGamesSection } from "@src/components/game/Sections/RelatedGamesSection";
import { ScreenshotPreview } from "@src/components/game/ScreenshotPreview";
import { linkStyle, sectionMetaStyle } from "@src/utils/sectionTextStyles";
import {
    getApiGamesGameidDetailsSuspenseQueryOptionsHook,
    getApiGamesGameidMediaSuspenseQueryOptionsHook,
    getApiGamesGameidPageReleaseDataSuspenseQueryOptionsHook,
    getApiGamesGameidSimilarGamesSuspenseQueryOptionsHook,
    getApiGamesGameidSuspenseQueryOptionsHook,
    useGetApiGamesGameidDetailsSuspenseHook,
    useGetApiGamesGameidMediaSuspenseHook,
    useGetApiGamesGameidPageReleaseDataSuspenseHook,
    useGetApiGamesGameidSimilarGamesSuspenseHook,
    useGetApiGamesGameidSuspenseHook,
} from "@src/gen/catalogApi";
import { formatReleaseDate } from "@src/utils/formatReleaseDate";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { PageWrapper } from "@src/components/layout/PageWrapper";

type Tab = "overview" | "official-links" | "media" | "details" | "release-dates";

const TABS: { value: Tab; label: string }[] = [
    { value: "overview", label: "Overview" },
    { value: "official-links", label: "Official links" },
    { value: "media", label: "Media" },
    { value: "details", label: "Details" },
    { value: "release-dates", label: "Release Dates" },
];

export const Route = createFileRoute("/games/$gameId")({
    component: GameDetailPage,
    loader: async ({ params: { gameId }, context: { queryClient } }) => {
        const id = Number(gameId);

        await Promise.all([
            queryClient.ensureQueryData(getApiGamesGameidSuspenseQueryOptionsHook({ gameId: id })),
            queryClient.ensureQueryData(
                getApiGamesGameidDetailsSuspenseQueryOptionsHook({ gameId: id }),
            ),
            queryClient.ensureQueryData(
                getApiGamesGameidMediaSuspenseQueryOptionsHook({ gameId: id }),
            ),
            queryClient.ensureQueryData(
                getApiGamesGameidPageReleaseDataSuspenseQueryOptionsHook({ gameId: id }),
            ),
            queryClient.ensureQueryData(
                getApiGamesGameidSimilarGamesSuspenseQueryOptionsHook({ gameId: id }),
            ),
        ]);
    },
});

function GameDetailPage() {
    const { gameId } = Route.useParams();
    const gameIdNumber = Number(gameId);
    const { data: overviewData } = useGetApiGamesGameidSuspenseHook({ gameId: gameIdNumber });
    const { data: detailsData } = useGetApiGamesGameidDetailsSuspenseHook({
        gameId: gameIdNumber,
    });
    const { data: mediaData } = useGetApiGamesGameidMediaSuspenseHook({ gameId: gameIdNumber });
    const { data: releaseData } = useGetApiGamesGameidPageReleaseDataSuspenseHook({
        gameId: gameIdNumber,
    });
    const { data: similarData } = useGetApiGamesGameidSimilarGamesSuspenseHook({
        gameId: gameIdNumber,
    });
    const overview = overviewData?.game;
    const details = detailsData?.game;
    const media = mediaData?.game;
    const releasePageData = releaseData?.data;
    const similarGames = similarData?.games ?? [];
    const [activeTab, setActiveTab] = useState<Tab>("overview");

    if (!overview) {
        return (
            <PageWrapper py="20">
                <VStack align="center" gap="6">
                    <Heading>Game not found</Heading>
                    <Link to="/" style={linkStyle}>
                        <Button>Back to Home</Button>
                    </Link>
                </VStack>
            </PageWrapper>
        );
    }

    const coverImage = getIGDBImageUrl(overview.cover, "cover_big");

    const storyText = overview.storyline ?? overview.summary ?? "No storyline available.";

    const firstReleaseDate =
        overview.releaseDates
            ?.map((release) => release.releaseDate ?? 0)
            .filter(Boolean)
            .sort((a, b) => a - b)[0] ?? null;

    const platforms = (overview.platforms ?? [])
        .map((platform) => platform?.name)
        .filter((name): name is string => Boolean(name));

    const websites = details?.websites?.filter((w) => w.url) ?? [];
    const alternativeNames = (details?.alternativeNames ?? [])
        .map((item) => item.name)
        .filter((item): item is string => Boolean(item));

    const developers = (details?.involvedCompanies ?? []).filter((c) => c.developed);
    const publishers = (details?.involvedCompanies ?? []).filter((c) => c.published);

    return (
        <Suspense fallback={<Loading.Rings color="blue.500" fontSize="5xl" />}>
            <Box w="full" color="fg.base">
                <Box position="relative" overflow="hidden">
                    {coverImage && (
                        <Image
                            src={coverImage}
                            alt=""
                            position="absolute"
                            inset={0}
                            zIndex={0}
                            w="full"
                            h="full"
                            objectFit="cover"
                            filter="blur(28px) brightness(0.45)"
                            transform="scale(1.1)"
                            pointerEvents="none"
                        />
                    )}
                    <Box
                        position="absolute"
                        inset={0}
                        zIndex={1}
                        bgGradient="linear(to-b, blackAlpha.450, blackAlpha.650)"
                        pointerEvents="none"
                    />
                    <PageWrapper py="8" position="relative" zIndex={2}>
                        <HStack align="flex-start" gap={{ base: 4, md: 6 }}>
                            <Box
                                flexShrink={0}
                                w={{ base: "120px", md: "160px" }}
                                h={{ base: "160px", md: "210px" }}
                                rounded="xl"
                                overflow="hidden"
                                boxShadow="lg"
                            >
                                <Image
                                    src={coverImage || undefined}
                                    alt={overview.name ?? "Cover"}
                                    objectFit="cover"
                                    w="full"
                                    h="full"
                                    display="block"
                                />
                            </Box>

                            <VStack align="start" gap={2} flex={1} minW={0} color="white">
                                <Text color="white" fontSize="sm" opacity={0.95}>
                                    {formatReleaseDate(firstReleaseDate)}
                                </Text>
                                <Heading
                                    size={{ base: "2xl", md: "4xl" }}
                                    color="white"
                                    lineHeight="1.1"
                                    textShadow="0 1px 2px rgba(0, 0, 0, 0.45)"
                                >
                                    {overview.name ?? "Untitled game"}
                                </Heading>
                                <Wrap gap="xs">
                                    <For each={overview.genres}>
                                        {(genre) => (
                                            <Tag
                                                key={genre}
                                                variant="outline"
                                                colorScheme="gray"
                                                color="whiteAlpha.900"
                                                borderColor="whiteAlpha.500"
                                                bg="whiteAlpha.200"
                                                size="sm"
                                                textTransform="none"
                                            >
                                                {genre}
                                            </Tag>
                                        )}
                                    </For>
                                </Wrap>
                                <HStack gap={6} mt={1} flexWrap="wrap">
                                    {developers.length > 0 && (
                                        <Box>
                                            <Text
                                                {...sectionMetaStyle}
                                                fontSize="xs"
                                                color="white"
                                                opacity={0.9}
                                                mb="2xs"
                                            >
                                                DEVELOPER
                                            </Text>
                                            <Text
                                                color="white"
                                                fontSize="sm"
                                                textShadow="0 1px 1px rgba(0, 0, 0, 0.4)"
                                            >
                                                {developers.map((c) => c.name).join(", ")}
                                            </Text>
                                        </Box>
                                    )}
                                    {publishers.length > 0 && (
                                        <Box>
                                            <Text
                                                {...sectionMetaStyle}
                                                fontSize="xs"
                                                color="white"
                                                opacity={0.9}
                                                mb="2xs"
                                            >
                                                PUBLISHER
                                            </Text>
                                            <Text
                                                color="white"
                                                fontSize="sm"
                                                textShadow="0 1px 1px rgba(0, 0, 0, 0.4)"
                                            >
                                                {publishers.map((c) => c.name).join(", ")}
                                            </Text>
                                        </Box>
                                    )}
                                </HStack>
                                <Text
                                    color="white"
                                    fontSize="sm"
                                    maxW="2xl"
                                    lineClamp={3}
                                    mt={1}
                                    opacity={0.95}
                                    textShadow="0 1px 2px rgba(0, 0, 0, 0.45)"
                                >
                                    {overview.summary ??
                                        overview.storyline ??
                                        "No summary available."}
                                </Text>
                            </VStack>
                        </HStack>
                    </PageWrapper>
                </Box>

                <Box borderBottomWidth="1px" borderColor="border.subtle">
                    <PageWrapper py="3">
                        <SegmentedControl.Root
                            value={activeTab}
                            onChange={(value) => setActiveTab(value as Tab)}
                            size="sm"
                            w="full"
                            colorScheme="emerald"
                            fullRounded
                            css={{
                                overflowX: "auto",
                                WebkitOverflowScrolling: "touch",
                                scrollbarWidth: "none",
                                "&::-webkit-scrollbar": {
                                    display: "none",
                                },
                                display: "flex",
                                flexWrap: "nowrap",
                            }}
                        >
                            {TABS.map((tab) => (
                                <SegmentedControl.Item
                                    key={tab.value}
                                    value={tab.value}
                                    colorScheme="emerald"
                                    fontSize="sm"
                                    h="full"
                                    fontWeight={activeTab === tab.value ? "semibold" : "normal"}
                                >
                                    {tab.label}
                                </SegmentedControl.Item>
                            ))}
                        </SegmentedControl.Root>
                    </PageWrapper>
                </Box>

                <PageWrapper py={8} pb={20}>
                    <Suspense
                        fallback={
                            <Box py="20" textAlign="center">
                                <Loading.Rings color="blue.500" fontSize="5xl" />
                            </Box>
                        }
                    >
                        <Box display={activeTab === "overview" ? "block" : "none"}>
                            <Grid
                                templateColumns={{ base: "1fr", lg: "1fr 300px" }}
                                gap={{ base: 8, lg: 10 }}
                                alignItems="start"
                            >
                                <VStack align="stretch" gap={8}>
                                    <GameStory storyText={storyText} />
                                    <ScreenshotPreview
                                        screenshots={media.screenshots}
                                        visible
                                        onViewAll={() => setActiveTab("media")}
                                    />
                                    <RelatedGamesSection games={similarGames} />
                                </VStack>

                                <OverviewPanel
                                    gameTypeName={overview.gameTypeName}
                                    modes={overview.genres}
                                    playerPerspectives={details?.playerPerspectives}
                                    platforms={platforms}
                                    genres={overview.genres}
                                    themes={overview.themes}
                                />
                            </Grid>
                        </Box>

                        <Box display={activeTab === "official-links" ? "block" : "none"}>
                            <OfficialLinks websites={websites} />
                        </Box>

                        <Box display={activeTab === "media" ? "block" : "none"}>
                            <MediaGrid screenshots={media.screenshots} videos={media.videos} />
                        </Box>

                        <Box display={activeTab === "details" ? "block" : "none"}>
                            <VStack align="stretch" gap={8}>
                                <AlternativeNames names={alternativeNames} />
                            </VStack>
                        </Box>

                        <Box display={activeTab === "release-dates" ? "block" : "none"}>
                            <GameReleaseDates releaseDates={releasePageData?.releases} />
                        </Box>
                    </Suspense>
                </PageWrapper>
            </Box>
        </Suspense>
    );
}

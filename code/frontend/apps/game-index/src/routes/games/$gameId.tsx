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
    Motion,
    VStack,
    Wrap,
    For,
} from "ui";
import MediaGrid from "@src/features/games/components/MediaGrid";
import AlternativeNames from "@src/features/games/components/AlternativeNames";
import { GameReleaseDates } from "@src/features/games/components/GameReleaseDates";
import { GameStory } from "@src/features/games/components/GameStory";
import { OfficialLinks } from "@src/features/games/components/OfficialLinks";
import OverviewPanel from "@src/features/games/components/OverviewPanel";
import { RelatedGamesSection } from "@src/features/games/components/Sections/RelatedGamesSection";
import { ScreenshotPreview } from "@src/features/games/components/ScreenshotPreview";
import { linkStyle, sectionMetaStyle } from "@src/utils/sectionTextStyles";
import {
    gamesGetMediaSuspenseQueryOptionsHook,
    gamesGetReleaseDataSuspenseQueryOptionsHook,
    gamesGetOverviewSuspenseQueryOptionsHook,
    gamesGetSimilarSuspenseQueryOptionsHook,
    gamesGetSuspenseQueryOptionsHook,
    useGamesGetMediaSuspenseHook,
    useGamesGetReleaseDataSuspenseHook,
    useGamesGetOverviewSuspenseHook,
    useGamesGetSimilarSuspenseHook,
    useGamesGetSuspenseHook,
} from "@src/gen/catalogApi";
import { formatReleaseDate } from "@src/utils/formatReleaseDate";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";

type Tab = "overview" | "official-links" | "media" | "details" | "release-dates";

const TABS: { value: Tab; label: string }[] = [
    { value: "overview", label: "Overview" },
    { value: "official-links", label: "Official links" },
    { value: "media", label: "Media" },
    { value: "details", label: "Details" },
    { value: "release-dates", label: "Release Dates" },
];

const TAB_INDEX: Record<Tab, number> = {
    overview: 0,
    "official-links": 1,
    media: 2,
    details: 3,
    "release-dates": 4,
};

export const Route = createFileRoute("/games/$gameId")({
    component: GameDetailPage,
    loader: async ({ params: { gameId }, context: { queryClient } }) => {
        const id = Number(gameId);

        await Promise.all([
            queryClient.ensureQueryData(gamesGetOverviewSuspenseQueryOptionsHook({ gameId: id })),
            queryClient.ensureQueryData(gamesGetSuspenseQueryOptionsHook({ gameId: id })),
            queryClient.ensureQueryData(gamesGetMediaSuspenseQueryOptionsHook({ gameId: id })),
            queryClient.ensureQueryData(
                gamesGetReleaseDataSuspenseQueryOptionsHook({ gameId: id }),
            ),
            queryClient.ensureQueryData(gamesGetSimilarSuspenseQueryOptionsHook({ gameId: id })),
        ]);
    },
});

function GameDetailPage() {
    const { gameId } = Route.useParams();
    const gameIdNumber = Number(gameId);
    const { data: overviewData } = useGamesGetOverviewSuspenseHook({ gameId: gameIdNumber });
    const { data: detailsData } = useGamesGetSuspenseHook({
        gameId: gameIdNumber,
    });
    const { data: mediaData } = useGamesGetMediaSuspenseHook({ gameId: gameIdNumber });
    const { data: releaseData } = useGamesGetReleaseDataSuspenseHook({
        gameId: gameIdNumber,
    });
    const { data: similarData } = useGamesGetSimilarSuspenseHook({
        gameId: gameIdNumber,
    });
    const overview = overviewData?.game;
    const details = detailsData?.game;
    const media = mediaData?.game;
    const releasePageData = releaseData?.data;
    const similarGames = similarData?.games ?? [];
    const [activeTab, setActiveTab] = useState<Tab>("overview");
    const [tabDirection, setTabDirection] = useState(1);

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

    const handleTabChange = (value: string) => {
        const nextTab = value as Tab;
        const currentIndex = TAB_INDEX[activeTab];
        const nextIndex = TAB_INDEX[nextTab];

        if (nextIndex !== currentIndex) {
            setTabDirection(nextIndex > currentIndex ? 1 : -1);
            setActiveTab(nextTab);
        }
    };

    return (
        <Suspense fallback={<Loading.Rings color="primary.500" fontSize="5xl" />}>
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
                        <Motion
                            initial={{ y: 18 }}
                            animate={{ y: 0 }}
                            transition={{ duration: 0.22, ease: "easeOut" }}
                        >
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
                        </Motion>
                    </PageWrapper>
                </Box>

                <Box borderBottomWidth="1px" borderColor="border.subtle">
                    <PageWrapper py="3">
                        <SegmentedControl.Root
                            value={activeTab}
                            onChange={handleTabChange}
                            size="sm"
                            w="full"
                            colorScheme="emerald"
                            fullRounded
                            indicatorProps={{
                                transition: {
                                    type: "tween",
                                    duration: 0.16,
                                    ease: [0.22, 1, 0.36, 1],
                                },
                            }}
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
                                    transitionProperty="color"
                                    transitionDuration="0.18s"
                                    transitionTimingFunction="ease-out"
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
                                <Loading.Rings color="primary.500" fontSize="5xl" />
                            </Box>
                        }
                    >
                        <Box overflow="hidden">
                            <Motion
                                key={activeTab}
                                initial={{ x: tabDirection > 0 ? 40 : -40 }}
                                animate={{ x: 0 }}
                                transition={{ duration: 0.16, ease: [0.16, 1, 0.3, 1] }}
                            >
                                {activeTab === "overview" ? (
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
                                ) : null}

                                {activeTab === "official-links" ? (
                                    <OfficialLinks websites={websites} />
                                ) : null}

                                {activeTab === "media" ? (
                                    <MediaGrid
                                        screenshots={media.screenshots}
                                        videos={media.videos}
                                    />
                                ) : null}

                                {activeTab === "details" ? (
                                    <VStack align="stretch" gap={8}>
                                        <AlternativeNames names={alternativeNames} />
                                    </VStack>
                                ) : null}

                                {activeTab === "release-dates" ? (
                                    <GameReleaseDates releaseDates={releasePageData?.releases} />
                                ) : null}
                            </Motion>
                        </Box>
                    </Suspense>
                </PageWrapper>
            </Box>
        </Suspense>
    );
}

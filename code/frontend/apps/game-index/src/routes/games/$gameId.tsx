"use client";
import { createFileRoute, Link } from "@tanstack/react-router";
import { Suspense, useState } from "react";
import {
    Box,
    Button,
    ChevronRightIcon,
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
} from "ui";
import { CardSurface } from "@src/components/layout/Card";
import MediaGrid from "@src/components/game/MediaGrid";
import AlternativeNames from "@src/components/game/AlternativeNames";
import { OfficialLinks } from "@src/components/game/OfficialLinks";
import OverviewPanel from "@src/components/game/OverviewPanel";
import { RelatedGamesSection } from "@src/components/game/RelatedGamesSection";
import { ScreenshotPreview } from "@src/components/game/ScreenshotPreview";
import { linkStyle, sectionLabelStyle, sectionMetaStyle } from "@src/utils/sectionTextStyles";
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
    const { data: overviewData } = useGetApiGamesGameidSuspenseHook({ gameId: Number(gameId) });
    const { data: detailsData } = useGetApiGamesGameidDetailsSuspenseHook({
        gameId: Number(gameId),
    });
    const { data: mediaData } = useGetApiGamesGameidMediaSuspenseHook({ gameId: Number(gameId) });
    const { data: releaseData } = useGetApiGamesGameidPageReleaseDataSuspenseHook({
        gameId: Number(gameId),
    });
    const { data: similarData } = useGetApiGamesGameidSimilarGamesSuspenseHook({
        gameId: Number(gameId),
    });
    const overview = overviewData?.game;
    const details = detailsData?.game;
    const media = mediaData?.game;
    const releasePageData = releaseData?.data;
    const similarGames = similarData?.games ?? [];
    const [activeTab, setActiveTab] = useState<Tab>("overview");
    const [storyExpanded, setStoryExpanded] = useState(false);

    if (!overview) {
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

    const coverImage = getIGDBImageUrl(overview.cover, "cover_big");

    const storyText = overview.storyline ?? overview.summary ?? "No storyline available.";
    const isStoryLong = storyText.length > 280;

    const screenshotSources = (media?.screenshots ?? [])
        .map((s) => getIGDBImageUrl(s, "screenshot_med"))
        .filter((src) => src.length > 0);

    const firstReleaseDate =
        overview.releaseDates
            ?.map((release) => release.releaseDate ?? 0)
            .filter(Boolean)
            .sort((a, b) => a - b)[0] ?? null;

    const genres = overview.genres ?? [];
    const themes = overview.themes ?? [];
    const platforms = (overview.platforms ?? [])
        .map((platform) => platform?.name)
        .filter((name): name is string => Boolean(name));
    const modes = details?.gameModes ?? [];
    const playerPerspectives = details?.playerPerspectives ?? [];
    const websites = details?.websites?.filter((w) => w.url) ?? [];
    const videos = media?.videos ?? [];
    const alternativeNames = (details?.alternativeNames ?? [])
        .map((item) => item.name)
        .filter((item): item is string => Boolean(item));
    const releaseDates = releasePageData?.releases ?? [];
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

                            <VStack align="start" gap={2} flex={1} minW={0}>
                                <Text color="fg.muted" fontSize="sm">
                                    {formatReleaseDate(firstReleaseDate)}
                                </Text>
                                <Heading
                                    size={{ base: "2xl", md: "4xl" }}
                                    color="fg.base"
                                    lineHeight="1.1"
                                >
                                    {overview.name ?? "Untitled game"}
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
                                <Text
                                    color="fg.subtle"
                                    fontSize="sm"
                                    maxW="2xl"
                                    lineClamp={3}
                                    mt={1}
                                >
                                    {overview.summary ?? ""}
                                </Text>
                            </VStack>
                        </HStack>
                    </Box>
                </Box>

                <Box borderBottomWidth="1px" borderColor="border.subtle">
                    <Box maxW="6xl" mx="auto" px={{ base: 4, md: 6 }} py="3">
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
                    </Box>
                </Box>

                <Box maxW="6xl" mx="auto" px={{ base: 4, md: 6 }} py={8} pb={20}>
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
                                    <Box>
                                        <Text {...sectionLabelStyle} mb={3}>
                                            Story
                                        </Text>
                                        <Text
                                            color="fg.subtle"
                                            lineHeight="tall"
                                            lineClamp={storyExpanded ? undefined : 5}
                                        >
                                            {storyText}
                                        </Text>
                                        {isStoryLong ? (
                                            <Box textAlign="right" mt="2">
                                                <Button
                                                    variant="ghost"
                                                    size="sm"
                                                    disableRipple
                                                    onClick={() =>
                                                        setStoryExpanded((prev) => !prev)
                                                    }
                                                    endIcon={
                                                        <ChevronRightIcon
                                                            boxSize="4"
                                                            transitionProperty="transform"
                                                            transitionDuration="moderate"
                                                            transform={
                                                                storyExpanded
                                                                    ? "rotate(90deg)"
                                                                    : "rotate(0deg)"
                                                            }
                                                        />
                                                    }
                                                >
                                                    {storyExpanded ? "Show less" : "Read more"}
                                                </Button>
                                            </Box>
                                        ) : null}
                                    </Box>
                                    <ScreenshotPreview
                                        sources={screenshotSources}
                                        visible
                                        onViewAll={() => setActiveTab("media")}
                                    />
                                    <RelatedGamesSection games={similarGames} />
                                </VStack>

                                <OverviewPanel
                                    gameTypeName={overview.gameTypeName}
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

                        <Box display={activeTab === "release-dates" ? "block" : "none"}>
                            <VStack align="stretch" gap={8}>
                                {releaseDates.length > 0 ? (
                                    <VStack align="stretch" gap={4}>
                                        {releaseDates.map((release) => (
                                            <CardSurface
                                                key={`${release.platformSlug ?? "unknown"}-${release.region ?? "any"}-${release.releaseDate ?? ""}`}
                                                variant="translucent"
                                                rounded="lg"
                                                p="md"
                                            >
                                                <HStack
                                                    align="start"
                                                    justify="space-between"
                                                    wrap="wrap"
                                                    gap="4"
                                                >
                                                    <VStack align="start" gap={1}>
                                                        <Text fontSize="sm" color="fg.muted">
                                                            Platform
                                                        </Text>
                                                        <Text fontWeight="semibold" color="fg.base">
                                                            {release.platformName ??
                                                                "Unknown platform"}
                                                        </Text>
                                                    </VStack>

                                                    <VStack align="start" gap={1}>
                                                        <Text fontSize="sm" color="fg.muted">
                                                            Release date
                                                        </Text>
                                                        <Text fontWeight="semibold" color="fg.base">
                                                            {release.human ??
                                                                (release.releaseDate
                                                                    ? new Date(
                                                                          release.releaseDate *
                                                                              1000,
                                                                      ).toLocaleDateString()
                                                                    : "Unknown")}
                                                        </Text>
                                                    </VStack>
                                                </HStack>

                                                {release.region ? (
                                                    <Text color="fg.subtle" fontSize="sm" mt="3">
                                                        Region: {release.region}
                                                    </Text>
                                                ) : null}

                                                <Wrap gap="2" mt="4">
                                                    {release.developers?.map((developer) => (
                                                        <Tag
                                                            key={`dev-${developer.name ?? "unknown"}`}
                                                            variant="surface"
                                                            colorScheme="gray"
                                                            size="md"
                                                        >
                                                            Developer: {developer.name ?? "Unknown"}
                                                        </Tag>
                                                    ))}
                                                    {release.publishers?.map((publisher) => (
                                                        <Tag
                                                            key={`pub-${publisher.name ?? "unknown"}`}
                                                            variant="surface"
                                                            colorScheme="gray"
                                                            size="sm"
                                                        >
                                                            Publisher: {publisher.name ?? "Unknown"}
                                                        </Tag>
                                                    ))}
                                                </Wrap>
                                            </CardSurface>
                                        ))}
                                    </VStack>
                                ) : (
                                    <Text color="fg.subtle">
                                        Release date data is not available.
                                    </Text>
                                )}
                            </VStack>
                        </Box>
                    </Suspense>
                </Box>
            </Box>{" "}
        </Suspense>
    );
}

export default GameDetailPage;

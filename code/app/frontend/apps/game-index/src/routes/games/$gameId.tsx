"use client";
import { Suspense } from "react";
import { createFileRoute, Link } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";
import {
    Box,
    Button,
    DataList,
    Heading,
    Image,
    Loading,
    Tabs,
    Tag,
    For,
    Text,
    HStack,
    VStack,
    Wrap,
} from "ui";
import { MediaGrid } from "@src/features/game/components/media-grid";
import { GameReleaseDates } from "@src/features/game/components/game-release-dates";
import { GameStory } from "@src/features/game/components/game-story";
import { OfficialLinks } from "@src/features/game/components/official-links";
import { RelatedGamesSection } from "@src/features/game/components/related-games-section";
import { PlayerStats } from "@src/features/game/components/player-stats";
import { PricingInfo } from "@src/features/game/components/pricing-info";
import { IGDBInfo } from "@src/features/game/components/game-info-panel";
import { linkStyle } from "@src/features/game/utils/section-text-styles";
import {
    gamesGetHeroSuspenseQueryOptionsHook,
    gamesGetLinksSuspenseQueryOptionsHook,
    gamesGetMediaSuspenseQueryOptionsHook,
    gamesGetReleaseDataSuspenseQueryOptionsHook,
    gamesGetSimilarSuspenseQueryOptionsHook,
} from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { PageWrapper } from "@src/app/page-wrapper";

export const Route = createFileRoute("/games/$gameId")({
    component: GameDetailPage,
});

function GameDetailPage() {
    const { gameId } = Route.useParams();
    const gameIdNumber = Number(gameId);

    const heroQuery = useSuspenseQuery(
        gamesGetHeroSuspenseQueryOptionsHook({ gameId: gameIdNumber }),
    );

    const mediaQuery = useSuspenseQuery(
        gamesGetMediaSuspenseQueryOptionsHook({ gameId: gameIdNumber }),
    );

    const releaseQuery = useSuspenseQuery(
        gamesGetReleaseDataSuspenseQueryOptionsHook({ gameId: gameIdNumber }),
    );

    const similarQuery = useSuspenseQuery(
        gamesGetSimilarSuspenseQueryOptionsHook({ gameId: gameIdNumber }),
    );

    const hero = heroQuery.data?.game;
    const media = mediaQuery.data?.game;
    const releasePageData = releaseQuery.data?.data;
    const similarGames = similarQuery.data?.games ?? [];

    if (!hero) {
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

    const coverImage = getIGDBImageUrl(hero.cover, "720p");

    return (
        <Suspense fallback={<Loading.Rings color="primary.500" fontSize="5xl" />}>
            <PageWrapper pt="0" maxW="8xl">
                <Box position="relative" overflow="hidden" mb={{ base: 6, md: 10 }}>
                    <Box position="absolute" inset={0} zIndex={-1}>
                        {coverImage && (
                            <Image
                                src={coverImage}
                                alt=""
                                w="full"
                                h="full"
                                objectFit="cover"
                                filter="blur(80px) brightness(0.3)"
                                transform="scale(1.2)"
                                pointerEvents="none"
                            />
                        )}
                        <Box
                            position="absolute"
                            inset={0}
                            bgGradient="linear(to-b, blackAlpha.400 0%, blackAlpha.200 35%, oklch(21.35% .0146 225deg) 100%)"
                        />
                    </Box>

                    <Box w="full" color="fg.base" position="relative" zIndex={1}>
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
                                    src={coverImage}
                                    alt={hero.name ?? "Cover"}
                                    objectFit="cover"
                                    w="full"
                                    h="full"
                                    display="block"
                                />
                            </Box>

                            <VStack align="start" gap={2} flex={1} minW={0} color="white">
                                <Heading
                                    size={{ base: "2xl", md: "4xl" }}
                                    color="white"
                                    lineHeight="1.1"
                                    textShadow="0 1px 2px rgba(0, 0, 0, 0.45)"
                                >
                                    {hero.name ?? "Untitled game"}
                                </Heading>

                                <Wrap gap="xs">
                                    <For each={hero.genres}>
                                        {(genre) => (
                                            <Tag
                                                key={genre.name}
                                                variant="subtle"
                                                size="sm"
                                                textTransform="none"
                                            >
                                                {genre.name}
                                            </Tag>
                                        )}
                                    </For>
                                </Wrap>

                                <DataList.Root
                                    variant="grid"
                                    items={[
                                        {
                                            term: "Release Date",
                                            description: hero.firstReleaseDate ?? "TBA",
                                        },
                                        {
                                            term: "Developer",
                                            description:
                                                hero.companies
                                                    .filter((c) => c.developer)
                                                    .map((c) => c.companyName)
                                                    .join(", ") || "—",
                                        },
                                        {
                                            term: "Publisher",
                                            description:
                                                hero.companies
                                                    .filter((c) => c.publisher)
                                                    .map((c) => c.companyName)
                                                    .join(", ") || "—",
                                        },
                                        {
                                            term: "Game Mode",
                                            description:
                                                hero.gameModes.map((m) => m.name).join(", ") || "—",
                                        },
                                        {
                                            term: "Perspective",
                                            description:
                                                hero.playerPerspectives
                                                    .map((p) => p.name)
                                                    .join(", ") || "—",
                                        },
                                        {
                                            term: "Platforms",
                                            description:
                                                hero.platforms.map((p) => p.name).join(", ") || "—",
                                        },
                                    ]}
                                />

                                <GameStory storyText={hero.summary ?? "No summary available."} />
                            </VStack>
                        </HStack>
                    </Box>
                </Box>

                <Tabs.Root orientation="horizontal" lazy mt={{ base: 6, md: 10 }}>
                    <Tabs.List>
                        <Tabs.Tab index={0}>Players</Tabs.Tab>
                        <Tabs.Tab index={1}>Steam Store</Tabs.Tab>
                        <Tabs.Tab index={2}>IGDB</Tabs.Tab>
                        <Tabs.Tab index={3}>Release Dates</Tabs.Tab>
                        <Tabs.Tab index={4}>Screenshots</Tabs.Tab>
                        <Tabs.Tab index={5}>Links</Tabs.Tab>
                        <Tabs.Tab index={6}>Related Games</Tabs.Tab>
                    </Tabs.List>

                    <Tabs.Panels>
                        <Tabs.Panel index={0}>
                            <PlayerStats gameId={gameIdNumber} />
                        </Tabs.Panel>

                        <Tabs.Panel index={1}>
                            <Suspense
                                fallback={<Loading.Rings color="primary.500" fontSize="xl" />}
                            >
                                <PricingInfo gameId={gameIdNumber} />
                            </Suspense>
                        </Tabs.Panel>

                        <Tabs.Panel index={2}>
                            <IGDBInfo />
                        </Tabs.Panel>

                        <Tabs.Panel index={3}>
                            <GameReleaseDates releaseDates={releasePageData?.releases} />
                        </Tabs.Panel>
                        <Tabs.Panel index={4}>
                            {media ? (
                                <MediaGrid screenshots={media.screenshots} videos={media.videos} />
                            ) : (
                                <Text color="fg.muted">No media available.</Text>
                            )}
                        </Tabs.Panel>
                        <Tabs.Panel index={5}>
                            <Suspense
                                fallback={<Loading.Rings color="primary.500" fontSize="xl" />}
                            >
                                <LinksTabContent gameId={gameIdNumber} />
                            </Suspense>
                        </Tabs.Panel>

                        <Tabs.Panel index={6}>
                            <RelatedGamesSection games={similarGames} />
                        </Tabs.Panel>
                    </Tabs.Panels>
                </Tabs.Root>
            </PageWrapper>
        </Suspense>
    );
}

function LinksTabContent({ gameId }: { gameId: number }) {
    const { data } = useSuspenseQuery(gamesGetLinksSuspenseQueryOptionsHook({ gameId }));

    return <OfficialLinks websites={data?.websites ?? []} />;
}

"use client";
import { Suspense, useState } from "react";
import { createFileRoute, Link } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";
import { Text, Heading } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { Button } from "@astryxdesign/core/Button";
import { Badge } from "@astryxdesign/core/Badge";
import { Spinner } from "@astryxdesign/core/Spinner";
import { TabList, Tab } from "@astryxdesign/core/TabList";
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

const TAB_VALUES = ["players", "steam", "igdb", "releases", "screenshots", "links", "related"] as const;
type TabValue = (typeof TAB_VALUES)[number];

function GameDetailPage() {
    const { gameId } = Route.useParams();
    const gameIdNumber = Number(gameId);
    const [selectedTab, setSelectedTab] = useState<TabValue>("players");

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
            <PageWrapper paddingBlock="5rem">
                <VStack hAlign="center" gap={6}>
                    <Heading level={1}>Game not found</Heading>
                    <Link to="/" style={linkStyle}>
                        <Button label="Back to Home" />
                    </Link>
                </VStack>
            </PageWrapper>
        );
    }

    const coverImage = getIGDBImageUrl(hero.cover, "720p");

    return (
        <Suspense fallback={<Spinner size="xl" />}>
            <PageWrapper paddingTop="0" maxWidth="var(--spacing-8xl, 896px)">
                <div style={{ position: "relative", overflow: "hidden", marginBottom: "1.5rem" }}>
                    <div style={{ position: "absolute", inset: 0, zIndex: -1 }}>
                        {coverImage && (
                            <img
                                src={coverImage}
                                alt=""
                                style={{
                                    width: "100%",
                                    height: "100%",
                                    objectFit: "cover",
                                    filter: "blur(80px) brightness(0.3)",
                                    transform: "scale(1.2)",
                                    pointerEvents: "none",
                                }}
                            />
                        )}
                        <div
                            style={{
                                position: "absolute",
                                inset: 0,
                                background: "linear-gradient(to bottom, rgba(0,0,0,0.4) 0%, rgba(0,0,0,0.2) 35%, oklch(21.35% .0146 225deg) 100%)",
                            }}
                        />
                    </div>

                    <div style={{ width: "100%", color: "white", position: "relative", zIndex: 1 }}>
                        <HStack vAlign="start" gap={4}>
                            <div
                                style={{
                                    flexShrink: 0,
                                    width: "120px",
                                    height: "160px",
                                    borderRadius: "var(--radius-xl)",
                                    overflow: "hidden",
                                    boxShadow: "0 10px 15px -3px rgba(0, 0, 0, 0.3)",
                                }}
                            >
                                <img
                                    src={coverImage}
                                    alt={hero.name ?? "Cover"}
                                    style={{ width: "100%", height: "100%", objectFit: "cover", display: "block" }}
                                />
                            </div>

                            <VStack hAlign="start" gap={2} style={{flex: 1, minWidth: 0, color: "white"}}>
                                <Heading
                                    level={1}
                                    style={{fontSize: "clamp(1.5rem, 3vw, 2.5rem)", color: "white", lineHeight: 1.1, textShadow: "0 1px 2px rgba(0, 0, 0, 0.45)"}}
                                >
                                    {hero.name ?? "Untitled game"}
                                </Heading>

                                <div style={{ display: "flex", flexWrap: "wrap", gap: "0.25rem" }}>
                                    {hero.genres.map((genre: { name: string }) => (
                                        <Badge
                                            key={genre.name}
                                            variant="info"
                                            label={genre.name}
                                            style={{ textTransform: "none" }}
                                        />
                                    ))}
                                </div>

                                <dl style={{ display: "grid", gridTemplateColumns: "auto 1fr", gap: "0.25rem 0.75rem", fontSize: "0.875rem" }}>
                                    <dt style={{ color: "rgba(255,255,255,0.6)" }}>Release Date</dt>
                                    <dd style={{ margin: 0, color: "white" }}>{hero.firstReleaseDate ?? "TBA"}</dd>
                                    <dt style={{ color: "rgba(255,255,255,0.6)" }}>Developer</dt>
                                    <dd style={{ margin: 0, color: "white" }}>{hero.companies.filter((c: { developer: boolean }) => c.developer).map((c: { companyName: string }) => c.companyName).join(", ") || "—"}</dd>
                                    <dt style={{ color: "rgba(255,255,255,0.6)" }}>Publisher</dt>
                                    <dd style={{ margin: 0, color: "white" }}>{hero.companies.filter((c: { publisher: boolean }) => c.publisher).map((c: { companyName: string }) => c.companyName).join(", ") || "—"}</dd>
                                    <dt style={{ color: "rgba(255,255,255,0.6)" }}>Game Mode</dt>
                                    <dd style={{ margin: 0, color: "white" }}>{hero.gameModes.map((m: { name: string }) => m.name).join(", ") || "—"}</dd>
                                    <dt style={{ color: "rgba(255,255,255,0.6)" }}>Perspective</dt>
                                    <dd style={{ margin: 0, color: "white" }}>{hero.playerPerspectives.map((p: { name: string }) => p.name).join(", ") || "—"}</dd>
                                    <dt style={{ color: "rgba(255,255,255,0.6)" }}>Platforms</dt>
                                    <dd style={{ margin: 0, color: "white" }}>{hero.platforms.map((p: { name: string }) => p.name).join(", ") || "—"}</dd>
                                </dl>

                                <GameStory storyText={hero.summary ?? "No summary available."} />
                            </VStack>
                        </HStack>
                    </div>
                </div>

                <TabList value={selectedTab} onChange={(val) => setSelectedTab(val as TabValue)}>
                    <Tab value="players" label="Players" />
                    <Tab value="steam" label="Steam Store" />
                    <Tab value="igdb" label="IGDB" />
                    <Tab value="releases" label="Release Dates" />
                    <Tab value="screenshots" label="Screenshots" />
                    <Tab value="links" label="Links" />
                    <Tab value="related" label="Related Games" />
                </TabList>

                <div style={{ marginTop: "1.5rem" }}>
                    {selectedTab === "players" && <PlayerStats gameId={gameIdNumber} />}
                    {selectedTab === "steam" && (
                        <Suspense fallback={<Spinner size="lg" />}>
                            <PricingInfo gameId={gameIdNumber} />
                        </Suspense>
                    )}
                    {selectedTab === "igdb" && <IGDBInfo />}
                    {selectedTab === "releases" && (
                        <GameReleaseDates releaseDates={releasePageData?.releases} />
                    )}
                    {selectedTab === "screenshots" && (
                        media ? (
                            <MediaGrid screenshots={media.screenshots} videos={media.videos} />
                        ) : (
                            <Text color="secondary">No media available.</Text>
                        )
                    )}
                    {selectedTab === "links" && (
                        <Suspense fallback={<Spinner size="lg" />}>
                            <LinksTabContent gameId={gameIdNumber} />
                        </Suspense>
                    )}
                    {selectedTab === "related" && <RelatedGamesSection games={similarGames} />}
                </div>
            </PageWrapper>
        </Suspense>
    );
}

function LinksTabContent({ gameId }: { gameId: number }) {
    const { data } = useSuspenseQuery(gamesGetLinksSuspenseQueryOptionsHook({ gameId }));

    return <OfficialLinks websites={data?.websites ?? []} />;
}

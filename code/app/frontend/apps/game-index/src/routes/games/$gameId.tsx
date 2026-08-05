"use client";
import { Suspense } from "react";
import { createFileRoute, Link, useNavigate } from "@tanstack/react-router";
import { z } from "zod";
import { useSuspenseQuery } from "@tanstack/react-query";
import { Text, Heading } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";
import { Button } from "@astryxdesign/core/Button";
import { Badge } from "@astryxdesign/core/Badge";
import { Spinner } from "@astryxdesign/core/Spinner";
import { TabList, Tab } from "@astryxdesign/core/TabList";
import { MediaTheme } from "@astryxdesign/core/theme";
import * as stylex from "@stylexjs/stylex";
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

const styles = stylex.create({
    missingGame: {
        paddingBlock: "var(--spacing-10)",
    },
    page: {
        paddingTop: "var(--spacing-4)",
    },
    record: {
        position: "relative",
        overflow: "hidden",
        marginBottom: "var(--spacing-6)",
        border: "1px solid var(--color-border)",
        borderRadius: "var(--radius-container)",
        padding: "var(--spacing-4)",
    },
    backdrop: {
        position: "absolute",
        inset: 0,
        zIndex: -1,
    },
    backdropImage: {
        width: "100%",
        height: "100%",
        objectFit: "cover",
        filter: "blur(80px) brightness(0.3)",
        transform: "scale(1.2)",
        pointerEvents: "none",
    },
    backdropWash: {
        position: "absolute",
        inset: 0,
        backgroundColor: "color-mix(in srgb, var(--color-background-body) 78%, transparent)",
    },
    recordGrid: {
        display: "grid",
        gridTemplateColumns: "minmax(0, 1fr) minmax(12rem, 18rem)",
        alignItems: "start",
        gap: "var(--spacing-5)",
        width: "100%",
        "@media (max-width: 640px)": {
            gridTemplateColumns: "1fr",
        },
    },
    recordDetails: {
        minWidth: 0,
        padding: "var(--spacing-4)",
        backgroundColor: "color-mix(in srgb, var(--color-background-surface) 82%, transparent)",
        border: "1px solid var(--color-border)",
        borderRadius: "var(--radius-container)",
        backdropFilter: "blur(12px)",
    },
    title: {
        fontSize: "clamp(1.5rem, 3vw, 2.5rem)",
        lineHeight: 1.1,
    },
    genres: {
        display: "flex",
        flexWrap: "wrap",
        gap: "var(--spacing-1)",
    },
    genre: {
        textTransform: "none",
    },
    facts: {
        display: "grid",
        gridTemplateColumns: "repeat(2, minmax(0, 1fr))",
        gap: "var(--spacing-3) var(--spacing-5)",
        "@media (max-width: 640px)": {
            gridTemplateColumns: "1fr",
        },
    },
    fact: {
        display: "grid",
        gap: "var(--spacing-1)",
        minWidth: 0,
    },
    factLabel: {
        color: "var(--color-text-secondary)",
        fontSize: "0.75rem",
    },
    factValue: {
        fontSize: "0.875rem",
        overflowWrap: "anywhere",
    },
    cover: {
        width: "100%",
        maxWidth: "18rem",
        justifySelf: "end",
        border: "1px solid var(--color-border)",
        borderRadius: "var(--radius-container)",
        overflow: "hidden",
        "@media (max-width: 640px)": {
            maxWidth: "12rem",
            justifySelf: "start",
            order: -1,
        },
    },
    coverImage: {
        display: "block",
        width: "100%",
        height: "100%",
        objectFit: "cover",
    },
    tabContent: {
        marginTop: "var(--spacing-6)",
    },
});

const TAB_VALUES = ["players", "steam", "igdb", "releases", "screenshots", "links", "related"] as const;
type TabValue = (typeof TAB_VALUES)[number];

export const Route = createFileRoute("/games/$gameId")({
    validateSearch: z.object({
        tab: z.enum(TAB_VALUES).optional(),
    }),
    component: GameDetailPage,
});

function GameDetailPage() {
    const { gameId } = Route.useParams();
    const { tab } = Route.useSearch();
    const navigate = useNavigate({ from: Route.fullPath });
    const gameIdNumber = Number(gameId);
    const selectedTab = tab ?? "players";

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
            <VStack hAlign="center" gap={6} xstyle={styles.missingGame}>
                <Heading level={1}>Game not found</Heading>
                <Link to="/" style={linkStyle}>
                    <Button label="Back to Home" />
                </Link>
            </VStack>
        );
    }

    const coverImage = getIGDBImageUrl(hero.cover, "720p");

    return (
        <Suspense fallback={<Spinner size="xl" />}>
            <div {...stylex.props(styles.page)}>
                <MediaTheme mode="dark">
                <div {...stylex.props(styles.record)}>
                    <div {...stylex.props(styles.backdrop)}>
                        {coverImage && (
                            <img
                                src={coverImage}
                                alt=""
                                {...stylex.props(styles.backdropImage)}
                            />
                        )}
                        <div {...stylex.props(styles.backdropWash)} />
                    </div>

                    <div {...stylex.props(styles.recordGrid)}>
                            <VStack hAlign="start" gap={3} xstyle={styles.recordDetails}>
                                <Heading level={1} xstyle={styles.title}>
                                    {hero.name ?? "Untitled game"}
                                </Heading>

                                <div {...stylex.props(styles.genres)}>
                                    {hero.genres.map((genre: { name: string }) => (
                                        <Badge
                                            key={genre.name}
                                            variant="info"
                                            label={genre.name}
                                            xstyle={styles.genre}
                                        />
                                    ))}
                                </div>

                                <div {...stylex.props(styles.facts)}>
                                    <Fact label="Release Date" value={hero.firstReleaseDate ?? "Not announced"} />
                                    <Fact label="Developer" value={hero.companies.filter((c: { developer: boolean }) => c.developer).map((c: { companyName: string }) => c.companyName).join(", ") || "Not listed"} />
                                    <Fact label="Publisher" value={hero.companies.filter((c: { publisher: boolean }) => c.publisher).map((c: { companyName: string }) => c.companyName).join(", ") || "Not listed"} />
                                    <Fact label="Game Mode" value={hero.gameModes.map((m: { name: string }) => m.name).join(", ") || "Not listed"} />
                                    <Fact label="Perspective" value={hero.playerPerspectives.map((p: { name: string }) => p.name).join(", ") || "Not listed"} />
                                    <Fact label="Platforms" value={hero.platforms.map((p: { name: string }) => p.name).join(", ") || "Not listed"} />
                                </div>

                                <GameStory storyText={hero.summary ?? "No summary available."} />
                            </VStack>

                            <div {...stylex.props(styles.cover)}>
                                <img
                                    src={coverImage}
                                    alt={hero.name ?? "Cover"}
                                    {...stylex.props(styles.coverImage)}
                                />
                            </div>
                    </div>
                </div>
                </MediaTheme>

                <TabList
                    value={selectedTab}
                    onChange={(value) =>
                        navigate({
                            to: "/games/$gameId",
                            params: { gameId },
                            search: { tab: value as TabValue },
                        })
                    }
                >
                    <Tab value="players" label="Players" />
                    <Tab value="steam" label="Steam Store" />
                    <Tab value="igdb" label="IGDB" />
                    <Tab value="releases" label="Release Dates" />
                    <Tab value="screenshots" label="Screenshots" />
                    <Tab value="links" label="Links" />
                    <Tab value="related" label="Related Games" />
                </TabList>

                <div {...stylex.props(styles.tabContent)}>
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
            </div>
        </Suspense>
    );
}

function Fact({ label, value }: { label: string; value: string }) {
    return (
        <div {...stylex.props(styles.fact)}>
            <Text xstyle={styles.factLabel}>{label}</Text>
            <Text xstyle={styles.factValue}>{value}</Text>
        </div>
    );
}

function LinksTabContent({ gameId }: { gameId: number }) {
    const { data } = useSuspenseQuery(gamesGetLinksSuspenseQueryOptionsHook({ gameId }));

    return <OfficialLinks websites={data?.websites ?? []} />;
}

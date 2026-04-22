import { Link, createFileRoute } from "@tanstack/react-router";
import { Suspense, useState } from "react";
import type { GameDto } from "@src/gen/catalogApi";
import { useCalendarGetGamesSuspenseHook } from "@src/gen/catalogApi";
import PageWrapper from "@src/components/AppShell/PageWrapper";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import { format, getYear, isValid, parseISO } from "date-fns";
import { formatReleaseDateLabel, isTbaReleaseDate } from "@src/utils/releaseDateUtils";
import { Box, Collapse, Grid, GridItem, Heading, HStack, Image, Loading, Text, VStack } from "ui";

export const Route = createFileRoute("/calendar/")({
    component: RouteComponent,
});

function getReleaseMonth(firstReleaseDate?: string | null) {
    if (!firstReleaseDate) {
        return null;
    }

    const date = parseISO(firstReleaseDate);
    if (!isValid(date)) {
        return null;
    }

    return format(date, "MMMM");
}

function groupGamesByMonth(games: GameDto[]) {
    const map = new Map<string, GameDto[]>();

    for (const game of games) {
        const key = getReleaseMonth(game.firstReleaseDate);
        if (!key) {
            continue;
        }

        const group = map.get(key) ?? [];
        group.push(game);
        map.set(key, group);
    }

    return Array.from(map.entries())
        .map(([month, groupedGames]) => ({
            month,
            games: groupedGames.sort((left, right) => {
                if (!left.firstReleaseDate && !right.firstReleaseDate) return 0;
                if (!left.firstReleaseDate) return 1;
                if (!right.firstReleaseDate) return -1;
                return left.firstReleaseDate.localeCompare(right.firstReleaseDate);
            }),
        }))
        .sort((left, right) => {
            const leftDate = left.games[0]?.firstReleaseDate
                ? parseISO(left.games[0].firstReleaseDate)
                : parseISO("9999-12-31");
            const rightDate = right.games[0]?.firstReleaseDate
                ? parseISO(right.games[0].firstReleaseDate)
                : parseISO("9999-12-31");
            return leftDate.getTime() - rightDate.getTime();
        });
}

function GameRow({ game, isPlaceholder = false }: { game: GameDto; isPlaceholder?: boolean }) {
    const dayLabel = isPlaceholder ? "Date TBA" : formatReleaseDateLabel(game.firstReleaseDate);
    const imageUrl = game.coverUrl ? getIGDBImageUrl(game.coverUrl, "cover_small") : null;
    const gameId = String(game.id);
    const studioLabel = game.developers?.[0]?.name ?? "Upcoming release";
    const platformIcons =
        game.platforms?.slice(0, 4).map((platform) => platform.slug ?? platform.name) ?? [];

    return (
        <Link
            to="/games/$gameId"
            params={{ gameId }}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <HStack
                align="start"
                px="2"
                py="3"
                gap="3"
                rounded="md"
                bg="bg.surface"
                borderWidth="1px"
                borderColor="border.subtle"
            >
                <Box
                    flexShrink={0}
                    w="10"
                    h="12"
                    rounded="md"
                    overflow="hidden"
                    bg="bg.subtle"
                    borderWidth="1px"
                    borderColor="border.subtle"
                >
                    {imageUrl ? (
                        <Image src={imageUrl} alt={game.name} w="full" h="full" objectFit="cover" />
                    ) : (
                        <Box display="grid" placeItems="center" w="full" h="full">
                            <Text fontSize="xs" color="fg.muted" fontWeight="semibold">
                                {game.name.slice(0, 1)}
                            </Text>
                        </Box>
                    )}
                </Box>

                <VStack align="stretch" gap="1" flex="1" minW="0">
                    <HStack justify="space-between" align="flex-start" gap="3" minW="0">
                        <Text
                            fontWeight="medium"
                            fontSize="sm"
                            lineClamp={2}
                            minW="0"
                            color="fg.base"
                        >
                            {game.name}
                        </Text>
                        {dayLabel && (
                            <Text fontSize="xs" color="fg.subtle" textAlign="end" flexShrink={0}>
                                {dayLabel}
                            </Text>
                        )}
                    </HStack>

                    <HStack justify="space-between" align="center" gap="3" minW="0">
                        <Text fontSize="xs" color="fg.subtle" minW="0">
                            {studioLabel}
                        </Text>
                        <HStack gap="1" wrap="wrap" justify="flex-end" flexShrink={0}>
                            {platformIcons.map((platform) => (
                                <Box
                                    key={platform}
                                    boxSize="4"
                                    display="grid"
                                    placeItems="center"
                                    fontSize="xs"
                                >
                                    <PlatformIcon type={platform} />
                                </Box>
                            ))}
                        </HStack>
                    </HStack>
                </VStack>
            </HStack>
        </Link>
    );
}

function GameGroupCard({
    title,
    games,
    emptyLabel,
    isPlaceholder = false,
}: {
    title: string;
    games: GameDto[];
    emptyLabel: string;
    isPlaceholder?: boolean;
}) {
    const [expanded, setExpanded] = useState(false);
    const hasMore = games.length > 4;

    return (
        <Box
            borderWidth="1px"
            borderColor="border.subtle"
            rounded="xl"
            p={{ base: "3", md: "4" }}
            bg="bg.panel"
        >
            <Text fontWeight="semibold" fontSize="lg" mb="2">
                {title}
            </Text>
            {games.length === 0 ? (
                <VStack py="6" color="fg.muted" gap="1">
                    <Text fontSize="2xl">:(</Text>
                    <Text fontSize="sm">{emptyLabel}</Text>
                </VStack>
            ) : (
                <VStack gap="2" align="stretch">
                    {games.slice(0, 4).map((game) => (
                        <GameRow key={game.id} game={game} isPlaceholder={isPlaceholder} />
                    ))}
                    <Collapse open={expanded}>
                        <VStack gap="2" align="stretch">
                            {games.slice(4).map((game) => (
                                <GameRow key={game.id} game={game} isPlaceholder={isPlaceholder} />
                            ))}
                        </VStack>
                    </Collapse>
                    {hasMore && (
                        <Box
                            as="button"
                            onClick={() => setExpanded((value) => !value)}
                            mt="2"
                            py="2"
                            w="full"
                            textAlign="center"
                            fontSize="sm"
                            color="fg.muted"
                            borderWidth="1px"
                            borderColor="border.subtle"
                            rounded="lg"
                            bg="bg.subtle"
                        >
                            {expanded ? "Show less" : "Expand all"}
                        </Box>
                    )}
                </VStack>
            )}
        </Box>
    );
}

function GamesCalendarPage() {
    const year = getYear(new Date());
    const { data } = useCalendarGetGamesSuspenseHook({ year });
    const tbaGames = data.games.filter((game) => isTbaReleaseDate(game.firstReleaseDate));
    const datedGames = data.games.filter((game) => !isTbaReleaseDate(game.firstReleaseDate));
    const groups = groupGamesByMonth(datedGames);

    return (
        <VStack align="stretch" gap="6">
            <HStack justify="space-between" align="baseline" wrap="wrap" gap="3">
                <Heading fontSize="2xl" fontWeight="bold">
                    {year}
                </Heading>
                <Text fontSize="sm" color="fg.muted">
                    {data.games.length} games
                </Text>
            </HStack>

            {groups.length === 0 ? (
                <Box
                    borderWidth="1px"
                    borderColor="border.subtle"
                    rounded="xl"
                    bg="bg.panel"
                    p={{ base: "6", md: "8" }}
                    textAlign="center"
                >
                    <Text fontSize="lg" color="fg.base" fontWeight="semibold">
                        No scheduled games
                    </Text>
                    <Text fontSize="sm" color="fg.muted" mt="1">
                        Games with placeholder dates are listed separately.
                    </Text>
                </Box>
            ) : (
                <Grid
                    templateColumns={{
                        base: "1fr",
                        md: "repeat(2, 1fr)",
                        xl: "repeat(4, 1fr)",
                    }}
                    gap="4"
                >
                    {groups.map((group) => (
                        <GridItem key={group.month}>
                            <GameGroupCard
                                title={group.month}
                                games={group.games}
                                emptyLabel="No games scheduled"
                            />
                        </GridItem>
                    ))}
                </Grid>
            )}

            <GameGroupCard
                title="Date TBA"
                games={tbaGames}
                emptyLabel="No TBD games"
                isPlaceholder
            />
        </VStack>
    );
}

function LoadingFallback() {
    return (
        <Box display="grid" placeItems="center" minH="64">
            <Loading.Rings color="primary.500" fontSize="5xl" />
        </Box>
    );
}

function RouteComponent() {
    return (
        <PageWrapper maxW="9xl" py={{ base: "4", md: "6" }}>
            <Suspense fallback={<LoadingFallback />}>
                <GamesCalendarPage />
            </Suspense>
        </PageWrapper>
    );
}

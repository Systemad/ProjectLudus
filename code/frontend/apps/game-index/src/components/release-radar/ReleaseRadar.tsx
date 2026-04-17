"use client";
import { useEffect, useState } from "react";
import { useInfiniteQuery } from "@tanstack/react-query";
import { Box, Flex, For, Gamepad2Icon, Image, InfiniteScrollArea, Loading, Text, VStack } from "ui";
import { RouterLink } from "@src/components/YamadaLink";
import {
    getApiGamesReleaseDateRangeHook,
    type GamesReleaseDateRangeResponse,
    type GamesSearchDto,
} from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { dayjs } from "@src/utils/dayjs";

const DAYS_PER_WEEK = 7;
const MAX_GAMES_PER_DAY = 5;
const PAGE_LIMIT = 120;
const MAX_WEEKS = 52;

type DayBucket = {
    dayKey: string;
    headingLabel: string;
    games: GamesSearchDto[];
};

function getOrdinal(value: number) {
    const mod10 = value % 10;
    const mod100 = value % 100;

    if (mod10 === 1 && mod100 !== 11) return "st";
    if (mod10 === 2 && mod100 !== 12) return "nd";
    if (mod10 === 3 && mod100 !== 13) return "rd";

    return "th";
}

function formatHeadingLabel(epoch: number) {
    const date = dayjs.unix(epoch).utc();
    const day = date.date();

    return `${date.format("MMMM D")}${getOrdinal(day)}`;
}

function sortGames(games: GamesSearchDto[]) {
    return [...games].sort((left, right) => {
        const leftDate = left.firstReleaseDate ?? Number.MAX_SAFE_INTEGER;
        const rightDate = right.firstReleaseDate ?? Number.MAX_SAFE_INTEGER;

        if (leftDate !== rightDate) {
            return leftDate - rightDate;
        }

        const leftWishlist = left.steamMostWishlistedUpcoming ?? 0;
        const rightWishlist = right.steamMostWishlistedUpcoming ?? 0;

        if (leftWishlist !== rightWishlist) {
            return rightWishlist - leftWishlist;
        }

        return left.name.localeCompare(right.name);
    });
}

function buildDayBuckets(pages: GamesReleaseDateRangeResponse[]) {
    const orderedGames = sortGames(pages.flatMap((page) => page.games));
    const map = new Map<string, GamesSearchDto[]>();

    for (const game of orderedGames) {
        if (!game.firstReleaseDate) continue;

        const dayKey = dayjs.unix(game.firstReleaseDate).utc().format("YYYY-MM-DD");
        const existing = map.get(dayKey);

        if (existing) {
            existing.push(game);
            continue;
        }

        map.set(dayKey, [game]);
    }

    return Array.from(map.entries())
        .sort(([left], [right]) => left.localeCompare(right))
        .map(
            ([dayKey, games]) =>
                ({
                    dayKey,
                    headingLabel: formatHeadingLabel(dayjs.utc(dayKey).unix()),
                    games: sortGames(games),
                }) satisfies DayBucket,
        );
}

function getWeekRange(startEpoch: number) {
    const start = dayjs.unix(startEpoch).utc().startOf("day");
    const end = start.add(DAYS_PER_WEEK - 1, "day").endOf("day");

    return {
        From: start.unix(),
        To: end.unix(),
        Limit: PAGE_LIMIT,
    };
}

export function ReleaseRadar() {
    const initialWeekStart = dayjs.utc().startOf("day").unix();
    const [activeMonthKey, setActiveMonthKey] = useState(() => dayjs.utc().format("YYYY-MM"));

    const query = useInfiniteQuery({
        queryKey: ["release-radar", initialWeekStart],
        initialPageParam: initialWeekStart,
        queryFn: ({ pageParam }: { pageParam: number }) =>
            getApiGamesReleaseDateRangeHook({ params: getWeekRange(pageParam) }),
        getNextPageParam: (lastPage, allPages) => {
            if (allPages.length >= MAX_WEEKS) {
                return undefined;
            }

            return dayjs.unix(lastPage.to).utc().add(1, "day").startOf("day").unix();
        },
    });

    const { fetchNextPage, hasNextPage, isFetchingNextPage } = query;

    const dayBuckets = buildDayBuckets(query.data?.pages ?? []);

    useEffect(() => {
        const updateActiveMonth = () => {
            const sections = document.querySelectorAll("[data-day-key]");
            if (sections.length === 0) {
                return;
            }

            const viewportCenter = window.innerHeight / 2;
            let closestSection: Element | null = null;
            let smallestDistance = Number.POSITIVE_INFINITY;

            for (const section of sections) {
                const rect = section.getBoundingClientRect();
                const sectionCenter = rect.top + rect.height / 2;
                const distance = Math.abs(sectionCenter - viewportCenter);

                if (distance < smallestDistance) {
                    smallestDistance = distance;
                    closestSection = section;
                }
            }

            const dayKey = closestSection?.getAttribute("data-day-key");
            if (!dayKey) {
                return;
            }

            const monthKey = dayjs.utc(dayKey).format("YYYY-MM");
            setActiveMonthKey((current) => (current === monthKey ? current : monthKey));
        };

        updateActiveMonth();
        window.addEventListener("scroll", updateActiveMonth, { passive: true });

        return () => {
            window.removeEventListener("scroll", updateActiveMonth);
        };
    }, [dayBuckets]);

    const activeMonth = dayjs.utc(`${activeMonthKey}-01`);
    const previousMonthLabel = activeMonth.add(-1, "month").format("MMMM");
    const currentMonthLabel = activeMonth.format("MMMM");
    const nextMonthLabel = activeMonth.add(1, "month").format("MMMM");

    return (
        <VStack align="stretch" gap={{ base: "lg", md: "2xl" }}>
            <Box
                position="sticky"
                top="20"
                zIndex="nappa"
                bg="bg.panel"
                pt={{ base: "2", md: "3" }}
                pb={{ base: "4", md: "5" }}
                borderBottomWidth="1px"
                borderColor="border.subtle"
                shadow="sm"
            >
                <Flex align="start" justify="space-between" gap={{ base: "md", md: "2xl" }}>
                    <VStack align="start" gap="2">
                        <Text
                            fontSize="sm"
                            color="fg.muted"
                            textTransform="uppercase"
                            letterSpacing="widest"
                            lineHeight="1"
                        >
                            Featured Upcoming Games
                        </Text>
                        <Text
                            fontSize={{ base: "4xl", md: "6xl" }}
                            fontWeight="medium"
                            lineHeight="0.95"
                        >
                            Release Radar
                        </Text>
                    </VStack>

                    <VStack align="end" gap="0">
                        <Text fontSize={{ base: "sm", md: "md" }} color="fg.muted">
                            {previousMonthLabel}
                        </Text>
                        <Text
                            key={activeMonthKey}
                            fontSize={{ base: "3xl", md: "5xl" }}
                            color="fg.base"
                            fontWeight="medium"
                            lineHeight="1"
                            transitionProperty="color, transform"
                            transitionDuration="moderate"
                            transform="scale(1.03)"
                        >
                            {currentMonthLabel}
                        </Text>
                        <Text fontSize={{ base: "sm", md: "md" }} color="fg.muted">
                            {nextMonthLabel}
                        </Text>
                    </VStack>
                </Flex>
            </Box>

            {query.isLoading ? (
                <Box minH={{ base: "64", md: "80" }} display="grid" placeItems="center">
                    <Loading.Rings color="blue.500" fontSize="5xl" />
                </Box>
            ) : null}

            {query.isError ? (
                <Box
                    minH={{ base: "64", md: "80" }}
                    rounded="xl"
                    borderWidth="1px"
                    borderColor="border.subtle"
                    bg="bg.panel"
                    display="grid"
                    placeItems="center"
                    textAlign="center"
                    gap="sm"
                >
                    <Text fontSize="sm" color="fg.subtle">
                        Radar data failed to load.
                    </Text>
                </Box>
            ) : null}

            {!query.isLoading && !query.isError && dayBuckets.length === 0 ? (
                <Box
                    minH={{ base: "64", md: "80" }}
                    rounded="xl"
                    borderWidth="1px"
                    borderColor="border.subtle"
                    bg="bg.panel"
                    display="grid"
                    placeItems="center"
                    textAlign="center"
                    gap="sm"
                >
                    <Gamepad2Icon boxSize="6" color="fg.muted" />
                    <Text fontSize="sm" color="fg.subtle">
                        No upcoming releases found.
                    </Text>
                </Box>
            ) : null}

            <InfiniteScrollArea
                pt={{ base: "1", md: "2" }}
                rootMargin="0px 0px 900px 0px"
                loading={<Loading.Rings color="blue.500" fontSize="3xl" />}
                onLoad={({ finish }) => {
                    if (!hasNextPage || isFetchingNextPage) {
                        finish();
                        return;
                    }

                    void fetchNextPage();
                }}
            >
                <VStack align="stretch" gap={{ base: "lg", md: "2xl" }}>
                    <For each={dayBuckets}>
                        {(bucket) => (
                            <Box
                                key={bucket.dayKey}
                                data-day-key={bucket.dayKey}
                                display="grid"
                                gap="md"
                            >
                                <Flex align="center" gap="4">
                                    <Text
                                        flexShrink={0}
                                        fontSize={{ base: "2xl", md: "4xl" }}
                                        fontWeight="medium"
                                    >
                                        {bucket.headingLabel}
                                    </Text>
                                    <Box
                                        flex="1"
                                        borderTopWidth="1px"
                                        borderTopStyle="dashed"
                                        borderColor="border.subtle"
                                    />
                                </Flex>

                                <Flex gap="4" overflowX="auto" pb="2">
                                    <For each={bucket.games.slice(0, MAX_GAMES_PER_DAY)}>
                                        {(game) => {
                                            const imageUrl = game.coverUrl
                                                ? getIGDBImageUrl(game.coverUrl, "cover_big")
                                                : undefined;

                                            return (
                                                <RouterLink
                                                    key={String(game.id)}
                                                    to="/games/$gameId"
                                                    params={{ gameId: String(game.id) }}
                                                    style={{ textDecoration: "none" }}
                                                >
                                                    <Box
                                                        position="relative"
                                                        minW={{ base: "28", md: "32" }}
                                                        w={{ base: "28", md: "32" }}
                                                        h={{ base: "40", md: "48" }}
                                                        rounded="2xl"
                                                        overflow="hidden"
                                                        borderWidth="1px"
                                                        borderColor="border.base"
                                                        bg="bg.panel"
                                                    >
                                                        {imageUrl ? (
                                                            <Image
                                                                src={imageUrl}
                                                                alt={game.name}
                                                                w="full"
                                                                h="full"
                                                                objectFit="cover"
                                                                loading="lazy"
                                                            />
                                                        ) : (
                                                            <Flex
                                                                w="full"
                                                                h="full"
                                                                align="center"
                                                                justify="center"
                                                                p="4"
                                                            >
                                                                <Text
                                                                    textAlign="center"
                                                                    fontSize="sm"
                                                                    color="fg.muted"
                                                                >
                                                                    {game.name}
                                                                </Text>
                                                            </Flex>
                                                        )}

                                                        <Box
                                                            position="absolute"
                                                            insetX="0"
                                                            bottom="0"
                                                            bgGradient="linear(to-t, blackAlpha.900, blackAlpha.500, transparent)"
                                                            px="3"
                                                            py="3"
                                                        >
                                                            <Text
                                                                fontSize="sm"
                                                                fontWeight="medium"
                                                                color="white"
                                                                lineClamp={2}
                                                            >
                                                                {game.name}
                                                            </Text>
                                                        </Box>
                                                    </Box>
                                                </RouterLink>
                                            );
                                        }}
                                    </For>

                                    {bucket.games.length > MAX_GAMES_PER_DAY ? (
                                        <Box
                                            minW={{ base: "20", md: "24" }}
                                            w={{ base: "20", md: "24" }}
                                            h={{ base: "40", md: "48" }}
                                            rounded="2xl"
                                            borderWidth="1px"
                                            borderColor="border.base"
                                            bg="bg.panel"
                                            display="grid"
                                            placeItems="center"
                                            textAlign="center"
                                            px="3"
                                        >
                                            <Text fontSize="lg" fontWeight="medium">
                                                +{bucket.games.length - MAX_GAMES_PER_DAY} more
                                            </Text>
                                            <Text fontSize="sm" color="fg.subtle">
                                                View all
                                            </Text>
                                        </Box>
                                    ) : null}
                                </Flex>
                            </Box>
                        )}
                    </For>
                </VStack>
            </InfiniteScrollArea>
        </VStack>
    );
}

import { createFileRoute, Link } from "@tanstack/react-router";
import { Box, Flex, HStack, Text, VStack, Heading, Loading, Container, SimpleGrid, Format } from "ui";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import {
    useMostAnticipatedGetSuspenseHook,
    usePopularityTypesGetByIdSuspenseHook,
    useStatsGetStatsSuspenseHook,
} from "@src/gen/catalogApi";
import { Suspense } from "react";
import { differenceInDays, differenceInHours, differenceInMinutes } from "date-fns";
import { GameTableGrid } from "@src/features/home/components/GameTableGrid";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function StatsPanel({ stats }: { stats: { totalGames: number; totalCompanies: number } }) {
    return (
        <Container.Root p={{ base: "sm", md: "md" }} rounded="2xl" variant={"panel"} border="none">
            <VStack align="stretch" gap="sm">
                <HStack justify="center" align="stretch" gap="4" wrap="wrap">
                    <Box p="sm" rounded="2xl" flex="1" minW="10rem" maxW="20rem">
                        <Text color="fg.muted" fontSize="sm" textAlign="center">
                            Games Indexed
                        </Text>
                        <Text color="fg.base" fontSize="2xl" fontWeight="bold" textAlign="center">
                            {stats?.totalGames ?? 0}
                        </Text>
                    </Box>
                    <Box p="sm" rounded="2xl" flex="1" minW="10rem" maxW="20rem">
                        <Text color="fg.muted" fontSize="sm" textAlign="center">
                            Companies Indexed
                        </Text>
                        <Text color="fg.base" fontSize="2xl" fontWeight="bold" textAlign="center">
                            {stats?.totalCompanies ?? 0}
                        </Text>
                    </Box>
                </HStack>
            </VStack>
        </Container.Root>
    );
}

function Hero() {
    return (
        <Container.Root
            p={{ base: "sm", md: "md" }}
            rounded="2xl"
            textAlign="center"
            variant={"panel"}
            colorScheme={"fuchsia"}
            border="none"
        >
            <VStack gap="xs" align="center">
                <Heading fontSize={{ base: "2xl", md: "3xl" }}>game-index.app</Heading>
                <Text fontSize="sm" color="fg.muted">
                    Discover, search, and explore games directly from IGDB
                </Text>
            </VStack>
        </Container.Root>
    );
}

function RouteComponent() {
    const { data: steamMostWishlisted } = usePopularityTypesGetByIdSuspenseHook({
        popularityTypeId: 10,
        params: { Limit: 25 },
    });

    const { data: steamMostPlayed } = usePopularityTypesGetByIdSuspenseHook({
        popularityTypeId: 9,
        params: { Limit: 25 },
    });

    const { data: steamPeakHours } = usePopularityTypesGetByIdSuspenseHook({
        popularityTypeId: 5,
        params: { Limit: 25 },
    });

    const { data: stats } = useStatsGetStatsSuspenseHook();
    const { data: mostAnticipated } = useMostAnticipatedGetSuspenseHook();

    return (
        <Suspense
            fallback={
                <PageWrapper py={{ base: "4", md: "6" }}>
                    <VStack align="center" justify="center" minH="60vh">
                        <Loading.Rings color="blue.500" fontSize="5xl" />
                    </VStack>
                </PageWrapper>
            }
        >
            <PageWrapper py={{ base: "2", md: "2" }}>
                <VStack align="stretch" gap="xl">
                    <Hero />

                    <Box>
                        <Flex justify="space-between" align="baseline" mb="3">
                            <Text fontWeight="semibold" color="fg.base">
                                Most Anticipated
                            </Text>
                            <Link to="/events" style={{ color: "inherit", textDecoration: "none" }}>
                                <Text fontSize="sm" color="fg.muted">
                                    View all
                                </Text>
                            </Link>
                        </Flex>
                        <SimpleGrid columns={{ base: 2 }} gap="4">
                            {mostAnticipated.games.slice(0, 4).map((game) => {
                                const releaseDate = new Date(game.firstReleaseDateUtc);
                                const now = new Date();
                                const days = Math.max(0, differenceInDays(releaseDate, now));
                                const hours = Math.max(0, differenceInHours(releaseDate, now) % 24);
                                const minutes = Math.max(
                                    0,
                                    differenceInMinutes(releaseDate, now) % 60,
                                );

                                return (
                                    <Link
                                        key={game.id}
                                        to="/games/$gameId"
                                        params={{ gameId: String(game.id) }}
                                        style={{ color: "inherit", textDecoration: "none" }}
                                    >
                                        <Box
                                            h="56"
                                            rounded="xl"
                                            bgImage={getIGDBImageUrl(game.coverUrl, "1080p")}
                                            bgSize="cover"
                                            bgPosition="center"
                                            position="relative"
                                            overflow="hidden"
                                        >
                                            <Box
                                                position="absolute"
                                                inset={0}
                                                bg="linear-gradient(135deg, rgba(0,0,0,0.85) 0%, rgba(0,0,0,0.25) 60%)"
                                            />
                                            <VStack
                                                position="absolute"
                                                inset={0}
                                                p="4"
                                                justify="space-between"
                                                align="stretch"
                                            >
                                                <VStack gap="0" align="flex-start">
                                                    <Text
                                                        fontSize="xl"
                                                        fontWeight="bold"
                                                        color="white"
                                                    >
                                                        {game.name}
                                                    </Text>
                                                    <Text fontSize="md" color="whiteAlpha.800">
                                                        <Format.DateTime
                                                            value={
                                                                new Date(game.firstReleaseDateUtc)
                                                            }
                                                            month="short"
                                                            day="2-digit"
                                                            year="numeric"
                                                        />
                                                    </Text>
                                                </VStack>

                                                <HStack gap="3" justify="center" w="full">
                                                    {[
                                                        {
                                                            value: String(days).padStart(2, "0"),
                                                            label: "Days",
                                                        },
                                                        {
                                                            value: String(hours).padStart(2, "0"),
                                                            label: "Hours",
                                                        },
                                                        {
                                                            value: String(minutes).padStart(2, "0"),
                                                            label: "Mins",
                                                        },
                                                    ].map((unit) => (
                                                        <VStack
                                                            key={unit.label}
                                                            gap="1"
                                                            align="center"
                                                            bg="whiteAlpha.200"
                                                            backdropFilter="blur(4px)"
                                                            rounded="xl"
                                                            px="4"
                                                            py="3"
                                                            minW="16"
                                                        >
                                                            <Text
                                                                fontSize="3xl"
                                                                fontWeight="bold"
                                                                color="white"
                                                                lineHeight="1.1"
                                                            >
                                                                {unit.value}
                                                            </Text>
                                                            <Text
                                                                fontSize="sm"
                                                                color="whiteAlpha.700"
                                                                lineHeight="1"
                                                            >
                                                                {unit.label}
                                                            </Text>
                                                        </VStack>
                                                    ))}
                                                </HStack>
                                            </VStack>
                                        </Box>
                                    </Link>
                                );
                            })}
                        </SimpleGrid>
                    </Box>

                    <StatsPanel stats={stats} />

                    <Suspense fallback={<Loading.Rings color="blue.500" fontSize="5xl" />}>
                        <GameTableGrid
                            tables={[
                                {
                                    title: "Most Wishlisted Upcoming",
                                    games: steamMostWishlisted.games,
                                    showReleaseDate: true,
                                },
                                { title: "Steam Global Top Sellers", games: steamMostPlayed.games },
                                { title: "24hr Peak Players", games: steamPeakHours.games },
                            ]}
                        />
                    </Suspense>
                </VStack>
            </PageWrapper>
        </Suspense>
    );
}

import { createFileRoute, Link } from "@tanstack/react-router";
import {
    Box,
    Flex,
    HStack,
    Text,
    VStack,
    Heading,
    Loading,
    Container,
    SimpleGrid,
    Format,
    Card,
    Image,
} from "ui";
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
import type { MostAnticipatedGame } from "@src/gen/catalogApi";

export const Route = createFileRoute("/")({
    component: RouteComponent,
});

function Hero() {
    return (
        <Box
            rounded="2xl"
            position="relative"
            overflow="hidden"
            px={{ base: "6", md: "10" }}
            py={{ base: "10", md: "14" }}
        >
            <Box
                position="absolute"
                inset={0}
                bgGradient="linear(to-r, #C6426E, #642B73)"
                opacity="0.15"
            />
            <VStack gap="3" align="center" position="relative">
                <Heading
                    fontSize={{ base: "3xl", md: "5xl" }}
                    fontWeight="black"
                    letterSpacing="tight"
                    bgClip="text"
                    bgGradient="linear(to-l, #C6426E, #642B73)"
                    textAlign="center"
                >
                    game-index.app
                </Heading>
                <Text fontSize={{ base: "md", md: "lg" }} color="fg.muted" textAlign="center" maxW="lg">
                    Discover, search, and explore games directly from IGDB
                </Text>
            </VStack>
        </Box>
    );
}

function MostAnticipatedCard({ game }: { game: MostAnticipatedGame }) {
    const releaseDate = new Date(game.firstReleaseDateUtc);
    const now = new Date();
    const days = Math.max(0, differenceInDays(releaseDate, now));
    const hours = Math.max(0, differenceInHours(releaseDate, now) % 24);
    const minutes = Math.max(0, differenceInMinutes(releaseDate, now) % 60);

    return (
        <Link
            to="/games/$gameId"
            params={{ gameId: String(game.id) }}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <Card.Root rounded="xl" overflow="hidden" border="none" bg="bg.panel">
                <Box position="relative">
                    <Image
                        src={getIGDBImageUrl(game.coverUrl, "1080p")}
                        alt={game.name}
                        w="full"
                        h="48"
                        objectFit="cover"
                    />
                    <Box position="absolute" inset={0} bg="linear-gradient(180deg, rgba(0,0,0,0.05) 0%, rgba(0,0,0,0.85) 100%)" />
                    <Box position="absolute" bottom="0" left="0" right="0" p="3">
                        <Text fontSize="md" fontWeight="bold" color="white" lineClamp={2}>
                            {game.name}
                        </Text>
                        <Text fontSize="xs" color="whiteAlpha.800">
                            <Format.DateTime
                                value={releaseDate}
                                month="short"
                                day="2-digit"
                                year="numeric"
                            />
                        </Text>
                    </Box>
                    <HStack
                        position="absolute"
                        top="3"
                        right="3"
                        gap="1.5"
                    >
                        {[
                            { value: String(days).padStart(2, "0"), label: "D" },
                            { value: String(hours).padStart(2, "0"), label: "H" },
                            { value: String(minutes).padStart(2, "0"), label: "M" },
                        ].map((unit) => (
                            <VStack
                                key={unit.label}
                                gap="0"
                                align="center"
                                bg="blackAlpha.600"
                                backdropFilter="blur(4px)"
                                rounded="md"
                                px="2.5"
                                py="1.5"
                                minW="10"
                            >
                                <Text fontSize="lg" fontWeight="bold" color="white" lineHeight="1.2">
                                    {unit.value}
                                </Text>
                                <Text fontSize="xs" color="whiteAlpha.700" lineHeight="1">
                                    {unit.label}
                                </Text>
                            </VStack>
                        ))}
                    </HStack>
                </Box>
            </Card.Root>
        </Link>
    );
}

function StatsPanel({ stats }: { stats: { totalGames: number; totalCompanies: number } }) {
    const statItems = [
        { label: "Games Indexed", value: stats?.totalGames ?? 0 },
        { label: "Companies Indexed", value: stats?.totalCompanies ?? 0 },
    ];

    return (
        <HStack gap="4" justify="center" wrap="wrap">
            {statItems.map((item) => (
                <Box
                    key={item.label}
                    flex="1"
                    minW="12rem"
                    maxW="16rem"
                    p="4"
                    rounded="xl"
                    textAlign="center"
                    bg="bg.panel"
                >
                    <Text fontSize="3xl" fontWeight="bold" color="fg.base">
                        {item.value.toLocaleString()}
                    </Text>
                    <Text fontSize="sm" color="fg.muted">
                        {item.label}
                    </Text>
                </Box>
            ))}
        </HStack>
    );
}

function PlaceholderTrendingCard({ index }: { index: number }) {
    const placeholders = [
        { name: "Elden Ring Nightreign", cover: "//images.igdb.com/igdb/image/upload/t_1080p/co8ni3.jpg" },
        { name: "Grand Theft Auto VI", cover: "//images.igdb.com/igdb/image/upload/t_1080p/co8dff.jpg" },
        { name: "Metroid Prime 4", cover: "//images.igdb.com/igdb/image/upload/t_1080p/co8jj4.jpg" },
        { name: "Hades II", cover: "//images.igdb.com/igdb/image/upload/t_1080p/co8bnj.jpg" },
    ];
    const game = placeholders[index];

    return (
        <Card.Root rounded="lg" bg="bg.panel" overflow="hidden" border="none">
            <Box aspectRatio={4 / 3} bg="bg.subtle" position="relative" overflow="hidden">
                <Box
                    w="full"
                    h="full"
                    bgGradient="linear(135deg, #C6426E, #642B73)"
                    opacity="0.3"
                />
                <VStack position="absolute" inset="0" justify="center" align="center" p="2">
                    <Text fontSize="xl" color="white" fontWeight="black">
                        ?
                    </Text>
                </VStack>
            </Box>
            <Card.Body p="2">
                <Text fontSize="xs" fontWeight="semibold" color="fg.base" lineClamp={1}>
                    Coming soon
                </Text>
            </Card.Body>
        </Card.Root>
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
                <VStack align="stretch" gap="10">
                    <Hero />

                    <StatsPanel stats={stats} />

                    <Box>
                        <Flex justify="space-between" align="baseline" mb="4">
                            <Heading fontSize="xl" fontWeight="bold" color="fg.base">
                                Most Anticipated
                            </Heading>
                            <Link to="/events" style={{ color: "inherit", textDecoration: "none" }}>
                                <Text fontSize="sm" color="fg.muted" _hover={{ color: "fg.base" }}>
                                    View all
                                </Text>
                            </Link>
                        </Flex>
                        <SimpleGrid columns={{ base: 2, md: 4 }} gap="4">
                            {mostAnticipated.games.slice(0, 4).map((game) => (
                                <MostAnticipatedCard key={game.id} game={game} />
                            ))}
                        </SimpleGrid>
                    </Box>

                    <Box>
                        <Heading fontSize="xl" fontWeight="bold" color="fg.base" mb="4">
                            Trending Now
                        </Heading>
                        <SimpleGrid columns={{ base: 2, md: 4 }} gap="4">
                            {[0, 1, 2, 3].map((i) => (
                                <PlaceholderTrendingCard key={i} index={i} />
                            ))}
                        </SimpleGrid>
                    </Box>

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

"use client";

import {
    Card,
    SimpleGrid,
    Image,
    Text,
    Heading,
    Box,
    Flex,
    Badge,
    Icon,
    VStack,
    HStack,
    TrendingUpIcon,
} from "@packages/ui";
import { createFileRoute, Link } from "@tanstack/react-router";
// react-instantsearch removed for layout-only version

export const Route = createFileRoute("/")({
    component: Index,
});

type GameSearchHit = {
    id?: string;
    objectID?: string;
    name?: string;
    cover_url?: string;
    aggregated_rating?: number;
    aggregated_rating_count?: number;
    first_release_date?: number | string;
};

const DEFAULT_IMAGE =
    "https://lh3.googleusercontent.com/aida-public/AB6AXuDrhV31xIPyEEpV8Z2HtaH3D-BFSIZtssVM_rsyySeLmC4VVBTkGke6dNVl2fTizEodmC0_xlXbECLefHIe2faK3a6KdevAB-f6JZPnxYg8HaOigYVT-zUp4HB1RVwp4CxPczrSX2Vz-4lxIMPrpYD8-3m1BNxrjWgJiJAFxHJ45zjsnC9VzA1lydtQOr5JpHjwigiCDywbpcL0rsFf7TbDw2rRiK5qvuQgJDB8tdgiAYx-peNLDEEMfOpyUWbnevRMWoFjDVU-nS5C";

const CONSOLES = [
    { name: "PlayStation 5", color: "blue.600", logo: "/logos/ps5.svg" },
    { name: "Xbox Series X", color: "emerald.500", logo: "/logos/xbox.svg" },
    { name: "Nintendo Switch", color: "red.500", logo: "/logos/switch.svg" },
    { name: "PC", color: "amber.200", logo: "/logos/pc.svg" },
];

const SAMPLE_GAMES: GameSearchHit[] = [
    {
        id: "g1",
        name: "Hollow Knight",
        cover_url: DEFAULT_IMAGE,
        aggregated_rating: 92,
        first_release_date: "2017-02-24",
    },
    {
        id: "g2",
        name: "Stardew Valley",
        cover_url: DEFAULT_IMAGE,
        aggregated_rating: 88,
        first_release_date: "2016-02-26",
    },
    {
        id: "g3",
        name: "Terraria",
        cover_url: DEFAULT_IMAGE,
        aggregated_rating: 85,
        first_release_date: "2011-05-16",
    },
    {
        id: "g4",
        name: "The Witcher 3",
        cover_url: DEFAULT_IMAGE,
        aggregated_rating: 94,
        first_release_date: "2015-05-19",
    },
    {
        id: "g5",
        name: "Hades II",
        cover_url: DEFAULT_IMAGE,
        aggregated_rating: 90,
        first_release_date: "2026-10-01",
    },
    {
        id: "g6",
        name: "Sea of Stars",
        cover_url: DEFAULT_IMAGE,
        aggregated_rating: 80,
        first_release_date: "2023-10-12",
    },
];

function getCoverImageUrl(coverUrl?: string) {
    if (!coverUrl) return DEFAULT_IMAGE;
    if (coverUrl.startsWith("http")) return coverUrl;
    return `https://images.igdb.com/igdb/image/upload/t_cover_big/${coverUrl}.jpg`;
}

function getReleaseLabel(firstReleaseDate?: number | string) {
    if (typeof firstReleaseDate === "number") {
        const year = new Date(firstReleaseDate * 1000).getUTCFullYear();
        return Number.isNaN(year) ? "Unknown release" : String(year);
    }

    if (typeof firstReleaseDate === "string") {
        const parsed = new Date(firstReleaseDate);
        if (!Number.isNaN(parsed.getTime())) {
            return String(parsed.getUTCFullYear());
        }
    }

    return "Unknown release";
}

function Index() {
    return (
        <VStack
            gap="xl"
            align="stretch"
            py="xl"
            px={{ base: "md", xl: "lg" }}
            minH="100vh"
        >
            <Hero />

            <TrendingNow />

            <ConsoleSection />

            <ReleasingThisMonth />
        </VStack>
    );
}

function Hero() {
    const heroImage =
        "https://images.igdb.com/igdb/image/upload/t_screenshot_huge/sc7e1o.jpg";

    return (
        <Box position="relative" w="full">
            <Box
                borderRadius="xl"
                overflow="hidden"
                h={{ base: "60", md: "96" }}
                position="relative"
                bg="black"
            >
                <Image
                    src={heroImage}
                    alt="Spotlight"
                    w="full"
                    h="full"
                    fit="cover"
                />

                <Box position="absolute" top="0" left="0" right="0" bottom="0">
                    <Box
                        position="absolute"
                        top="0"
                        left="0"
                        right="0"
                        bottom="0"
                        bgGradient="linear(to-r, blackAlpha.700, transparent)"
                        display="flex"
                        alignItems="center"
                    >
                        <VStack
                            align="start"
                            gap="md"
                            px={{ base: "md", md: "lg" }}
                        >
                            <Text
                                fontSize="xs"
                                fontWeight="900"
                                color="fg.muted"
                            >
                                SPOTLIGHT
                            </Text>

                            <Heading
                                fontSize={{ base: "2xl", md: "5xl" }}
                                fontWeight="900"
                                color="white"
                                lineHeight="shorter"
                            >
                                Starfield: Premium Edition
                            </Heading>

                            <HStack gap="sm">
                                <Badge
                                    colorScheme="emerald"
                                    fontWeight="bold"
                                    px="3"
                                    py="2"
                                    borderRadius="md"
                                >
                                    -40%
                                </Badge>

                                <Text fontWeight="bold" color="white">
                                    $59.99
                                </Text>
                            </HStack>
                        </VStack>
                    </Box>
                </Box>
            </Box>
        </Box>
    );
}

const ConsoleSection = () => {
    return (
        <Box>
            <Heading
                fontSize="sm"
                fontWeight="800"
                textTransform="uppercase"
                mb="4"
            >
                Browse by Console
            </Heading>

            <SimpleGrid columns={{ base: 2, md: 4 }} gap="4">
                {CONSOLES.map((console) => (
                    <Card.Root
                        key={console.name}
                        bg={console.color}
                        p="4"
                        borderRadius="lg"
                        transition="all 0.3s cubic-bezier(0.4, 0, 0.2, 1)"
                        _hover={{
                            transform: "translateY(-4px)",
                            boxShadow: "shadows.xl",
                        }}
                    >
                        <VStack align="center" gap="2">
                            {console.logo ? (
                                <Image
                                    src={console.logo}
                                    alt={console.name}
                                    w="16"
                                    h="16"
                                />
                            ) : (
                                <Box w="16" h="16" />
                            )}
                            <Text fontWeight="bold" color="white">
                                {console.name}
                            </Text>
                        </VStack>
                    </Card.Root>
                ))}
            </SimpleGrid>
        </Box>
    );
};

const TrendingNow = () => {
    const items = SAMPLE_GAMES;

    return (
        <Box>
            <Flex justify="space-between" align="center" mb="8">
                <Heading
                    fontSize="sm"
                    fontWeight="800"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    <Icon as={TrendingUpIcon} mr="xs" /> Trending Now
                </Heading>
                <Text
                    as={Link}
                    to="/search"
                    fontSize="xs"
                    fontWeight="900"
                    color="emerald.600"
                    letterSpacing="widest"
                >
                    VIEW ALL
                </Text>
            </Flex>

            <SimpleGrid columns={{ base: 2, md: 4, xl: 6 }} gap="6">
                {items.map((safeHit, idx) => {
                    const rating =
                        typeof safeHit.aggregated_rating === "number"
                            ? Math.round(safeHit.aggregated_rating)
                            : null;

                    return (
                        <Card.Root
                            key={safeHit.id ?? `trending-${idx}`}
                            bg="bg.panel"
                            borderRadius="xl"
                            overflow="hidden"
                            transition="all 0.3s cubic-bezier(0.4, 0, 0.2, 1)"
                            boxShadow="shadows.lg"
                            _hover={{
                                transform: "translateY(-6px)",
                                boxShadow: "shadows.xl",
                            }}
                        >
                            <Box aspectRatio={3 / 4} position="relative">
                                <Image
                                    src={getCoverImageUrl(safeHit.cover_url)}
                                    alt={safeHit.name ?? "Game"}
                                    w="full"
                                    h="full"
                                    fit="cover"
                                />

                                {rating !== null ? (
                                    <Badge
                                        colorScheme="emerald"
                                        position="absolute"
                                        bottom="2"
                                        right="2"
                                        px="2"
                                        py="1"
                                        borderRadius="md"
                                        fontWeight="bold"
                                        bg="emerald.500/90"
                                        color="white"
                                        boxShadow="0 6px 12px rgba(0, 0, 0, 0.4)"
                                    >
                                        {(rating / 10).toFixed(1)}
                                    </Badge>
                                ) : null}
                            </Box>

                            <Card.Body p="4">
                                <Text
                                    fontSize="sm"
                                    fontWeight="bold"
                                    mb="xs"
                                    color="white"
                                >
                                    {safeHit.name ?? "Untitled game"}
                                </Text>
                                <Badge
                                    bg="bg.base"
                                    fontSize="xs"
                                    borderRadius="lg"
                                    color="fg.muted"
                                >
                                    {getReleaseLabel(
                                        safeHit.first_release_date
                                    )}
                                </Badge>
                            </Card.Body>
                        </Card.Root>
                    );
                })}
            </SimpleGrid>
        </Box>
    );
};

function ReleasingThisMonth() {
    const sample = [
        {
            title: "Hollow Knight",
            subtitle: "Release Date",
            img: DEFAULT_IMAGE,
        },
        { title: "Tenaris", subtitle: "Release Date", img: DEFAULT_IMAGE },
        { title: "Hades II", subtitle: "Release Date", img: DEFAULT_IMAGE },
        {
            title: "Stardew Valley",
            subtitle: "Release Date",
            img: DEFAULT_IMAGE,
        },
        { title: "Sea of Stars", subtitle: "Release Date", img: DEFAULT_IMAGE },
        {
            title: "The Witcher 3",
            subtitle: "Release Date",
            img: DEFAULT_IMAGE,
        },
    ];

    return (
        <Box mt="lg">
            <Heading fontSize="sm" fontWeight="800" mb="4">
                Releasing This Month
            </Heading>

            <SimpleGrid columns={{ base: 1, md: 4 }} gap="6">
                <Card.Root
                    bg="bg.panel"
                    borderRadius="xl"
                    p="4"
                    boxShadow="shadows.lg"
                >
                    <Box
                        aspectRatio={1}
                        w="full"
                        bg="bg.base"
                        borderRadius="md"
                    />
                </Card.Root>

                <SimpleGrid
                    columns={{ base: 2, md: 3 }}
                    gap="4"
                    as={Box}
                    gridColumn="span 3"
                >
                    {sample.map((g, i) => (
                        <Card.Root
                            key={i}
                            bg="bg.panel"
                            borderRadius="lg"
                            overflow="hidden"
                            transition="all 0.3s cubic-bezier(0.4, 0, 0.2, 1)"
                            boxShadow="shadows.lg"
                            _hover={{
                                transform: "translateY(-4px)",
                                boxShadow: "shadows.xl",
                            }}
                        >
                            <Image
                                src={g.img}
                                alt={g.title}
                                w="full"
                                h="40"
                                fit="cover"
                            />
                            <Card.Body p="3">
                                <Text
                                    fontSize="sm"
                                    fontWeight="bold"
                                    color="white"
                                >
                                    {g.title}
                                </Text>
                                <Text fontSize="xs" color="fg.muted">
                                    {g.subtitle}
                                </Text>
                            </Card.Body>
                        </Card.Root>
                    ))}
                </SimpleGrid>
            </SimpleGrid>
        </Box>
    );
}

"use client";

import {
    Card,
    Skeleton,
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
    List,
    TrendingUpIcon,
    DatabaseIcon,
    ClockIcon,
    MoveRightIcon,
} from "@packages/ui";
import { createFileRoute, Link } from "@tanstack/react-router";
import {
    Configure,
    Hits,
    Index as AlgoliaIndex,
    InstantSearch,
    useHits,
    useInstantSearch,
} from "react-instantsearch";
import { searchClient } from "../instantsearch";

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
    { name: "PlayStation 5", color: "#003791", logo: "/logos/ps5.svg" },
    { name: "Xbox Series X", color: "#107C10", logo: "/logos/xbox.svg" },
    { name: "Nintendo Switch", color: "#E60012", logo: "/logos/switch.svg" },
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

function getHitKey(hit: GameSearchHit, fallback: string) {
    return String(hit.id ?? hit.objectID ?? fallback);
}

function SectionLoadingList() {
    return (
        <List.Root>
            {[0, 1].map((slot) => (
                <List.Item key={slot}>
                    <Flex
                        w="full"
                        p="md"
                        align="center"
                        gap="md"
                        borderRadius="xl"
                        bgColor="bg.panel"
                    >
                        <Skeleton boxSize="6" rounded="md" />
                        <Skeleton boxSize="14" rounded="lg" />
                        <VStack align="start" flex="1" gap="xs">
                            <Skeleton h="4" w="70%" />
                            <Skeleton h="3" w="40%" />
                        </VStack>
                    </Flex>
                </List.Item>
            ))}
        </List.Root>
    );
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
            <InstantSearch
                searchClient={searchClient}
                indexName="games_search/sort/aggregated_rating:desc"
            >
                <Configure query="" />

                <SimpleGrid columns={{ base: 1, md: 2 }} gap="lg">
                    <AlgoliaIndex indexName="games_search/sort/aggregated_rating:desc">
                        <Configure hitsPerPage={2} />
                        <PopularSection />
                    </AlgoliaIndex>

                    <AlgoliaIndex indexName="games_search/sort/first_release_date:asc">
                        <Configure hitsPerPage={2} />
                        <UpcomingSection />
                    </AlgoliaIndex>
                </SimpleGrid>

                <ConsoleSection />

                <AlgoliaIndex indexName="games_search/sort/aggregated_rating:desc">
                    <Configure hitsPerPage={6} />
                    <TrendingGames />
                </AlgoliaIndex>
            </InstantSearch>
        </VStack>
    );
}

const StatItem = ({
    rank,
    name,
    imageUrl,
    meta,
}: {
    rank: string;
    name: string;
    imageUrl: string;
    meta: string;
}) => (
    <Flex
        w="full"
        p="md"
        align="center"
        gap="md"
        borderRadius="xl"
        bgColor={"bg.panel"}
    >
        <Text fontSize="sm" fontWeight="900" w="6">
            {rank}
        </Text>
        <Image
            src={imageUrl}
            w="14"
            h="14"
            borderRadius="lg"
            fit="cover"
            alt={name}
        />
        <Box flex="1">
            <Text fontSize="sm" fontWeight="bold">
                {name}
            </Text>
            <Text
                fontSize="xs"
                color="fg.emphasized"
                fontWeight="bold"
                textTransform="uppercase"
            >
                {meta}
            </Text>
        </Box>
        <Icon as={MoveRightIcon} color="fg" fontSize="md" />
    </Flex>
);

function PopularSection() {
    const { items } = useHits<GameSearchHit>();
    const { status } = useInstantSearch();

    return (
        <Box
            css={{
                ".index-trending-grid": {
                    display: "grid",
                    gridTemplateColumns: "repeat(2, minmax(0, 1fr))",
                    gap: "1.5rem",
                    listStyle: "none",
                    margin: 0,
                    padding: 0,
                },
                "@media (min-width: 48em)": {
                    ".index-trending-grid": {
                        gridTemplateColumns: "repeat(4, minmax(0, 1fr))",
                    },
                },
                "@media (min-width: 80em)": {
                    ".index-trending-grid": {
                        gridTemplateColumns: "repeat(6, minmax(0, 1fr))",
                    },
                },
                ".index-trending-grid-item": {
                    margin: 0,
                },
            }}
        >
            <Heading
                fontSize="xs"
                fontWeight="800"
                textTransform="uppercase"
                letterSpacing="widest"
                mb="lg"
                display="flex"
                alignItems="center"
                gap="md"
            >
                <Icon as={TrendingUpIcon} /> Most Popular This Month
            </Heading>

            {status === "loading" || status === "stalled" ? (
                <SectionLoadingList />
            ) : (
                <List.Root>
                    {items.map((item, index) => (
                        <List.Item key={getHitKey(item, `popular-${index}`)}>
                            <StatItem
                                rank={String(index + 1).padStart(2, "0")}
                                name={item.name ?? "Untitled game"}
                                imageUrl={getCoverImageUrl(item.cover_url)}
                                meta={`Rating ${Math.round(
                                    item.aggregated_rating ?? 0
                                )}/100`}
                            />
                        </List.Item>
                    ))}
                </List.Root>
            )}
        </Box>
    );
}

function UpcomingSection() {
    const { items } = useHits<GameSearchHit>();
    const { status } = useInstantSearch();

    return (
        <Box>
            <Heading
                fontSize="xs"
                fontWeight="800"
                textTransform="uppercase"
                letterSpacing="widest"
                mb="lg"
                display="flex"
                alignItems="center"
                gap="md"
            >
                <Icon as={ClockIcon} /> Upcoming Games
            </Heading>

            {status === "loading" || status === "stalled" ? (
                <SectionLoadingList />
            ) : (
                <List.Root>
                    {items.map((item, index) => (
                        <List.Item key={getHitKey(item, `upcoming-${index}`)}>
                            <StatItem
                                rank={String(index + 1).padStart(2, "0")}
                                name={item.name ?? "Untitled game"}
                                imageUrl={getCoverImageUrl(item.cover_url)}
                                meta={getReleaseLabel(item.first_release_date)}
                            />
                        </List.Item>
                    ))}
                </List.Root>
            )}
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
            <HStack gap="4">
                {CONSOLES.map((console) => (
                    <Card.Root
                        key={console.name}
                        bg={`${console.color}/20`}
                        borderRadius="lg"
                        p="4"
                        _hover={{ transform: "translateY(-2px)" }}
                    >
                        <VStack align="center" gap="2">
                            <Image
                                src={console.logo}
                                alt={console.name}
                                w="16"
                                h="16"
                            />
                            <Text fontWeight="bold" color="white">
                                {console.name}
                            </Text>
                        </VStack>
                    </Card.Root>
                ))}
            </HStack>
        </Box>
    );
};

const TrendingGames = () => {
    const { status } = useInstantSearch();

    if (status === "loading" || status === "stalled") {
        return (
            <Box>
                <Flex justify="space-between" align="center" mb="8">
                    <Heading
                        fontSize="sm"
                        fontWeight="800"
                        textTransform="uppercase"
                        letterSpacing="widest"
                    >
                        <Icon as={DatabaseIcon} mr="xs" /> Trending
                    </Heading>
                    <Text
                        fontSize="xs"
                        fontWeight="900"
                        color="info.600"
                        letterSpacing="widest"
                    >
                        FULL DATABASE INDEX
                    </Text>
                </Flex>

                <SimpleGrid columns={{ base: 2, md: 4, xl: 6 }} gap="6">
                    {[0, 1, 2, 3, 4, 5].map((slot) => (
                        <Card.Root
                            key={slot}
                            bg="bg.subtle"
                            borderRadius="xl"
                            overflow="hidden"
                            border="none"
                        >
                            <Skeleton h="16rem" rounded="none" />
                            <Card.Body p="4">
                                <Skeleton h="4" mb="2" />
                                <Skeleton h="5" w="40%" rounded="full" />
                            </Card.Body>
                        </Card.Root>
                    ))}
                </SimpleGrid>
            </Box>
        );
    }

    return (
        <Box>
            <Flex justify="space-between" align="center" mb="8">
                <Heading
                    fontSize="sm"
                    fontWeight="800"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    <Icon as={DatabaseIcon} mr="xs" /> Trending
                </Heading>
                <Text
                    as={Link}
                    to="/search"
                    fontSize="xs"
                    fontWeight="900"
                    color="info.600"
                    letterSpacing="widest"
                >
                    FULL DATABASE INDEX
                </Text>
            </Flex>

            <Hits
                hitComponent={({ hit }) => {
                    const safeHit = hit as GameSearchHit;
                    const rating =
                        typeof safeHit.aggregated_rating === "number"
                            ? Math.round(safeHit.aggregated_rating)
                            : null;

                    return (
                        <Card.Root
                            bg="bg.subtle"
                            borderRadius="xl"
                            overflow="hidden"
                            border="none"
                            _hover={{ transform: "translateY(-2px)" }}
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
                                        position="absolute"
                                        bottom="2"
                                        right="2"
                                        px="2"
                                        py="1"
                                        borderRadius="md"
                                        fontWeight="bold"
                                        bg="whiteAlpha.600"
                                        backdropFilter="blur(10px)"
                                        color="black"
                                        borderColor="whiteAlpha.400"
                                        boxShadow="0 4px 6px rgba(0, 0, 0, 0.1)"
                                    >
                                        {(rating / 10).toFixed(1)}
                                    </Badge>
                                ) : null}
                            </Box>

                            <Card.Body p="4">
                                <Text fontSize="sm" fontWeight="bold" mb="xs">
                                    {safeHit.name ?? "Untitled game"}
                                </Text>
                                <Badge bg="bg" fontSize="xs" borderRadius="lg">
                                    {getReleaseLabel(
                                        safeHit.first_release_date
                                    )}
                                </Badge>
                            </Card.Body>
                        </Card.Root>
                    );
                }}
                classNames={{
                    list: "index-trending-grid",
                    item: "index-trending-grid-item",
                }}
            />
        </Box>
    );
};

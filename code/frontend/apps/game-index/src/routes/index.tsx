import { SimpleTable } from "@src/components/layout/SimpleTable";
import { createFileRoute, Link } from "@tanstack/react-router";
import { Box, Image, SimpleGrid, Text, VStack, Heading, GridItem, Loading } from "ui";
import {
    getApiPopularityPopularitytypeidSuspenseQueryOptionsHook,
    useGetApiPopularityPopularitytypeidSuspenseHook,
} from "@src/gen/catalogApi";
import { formatReleaseDate } from "@src/utils/formatReleaseDate";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { Suspense } from "react";

export const Route = createFileRoute("/")({
    component: RouteComponent,
    loader: async ({ context: { queryClient } }) => {
        await Promise.all([
            queryClient.ensureQueryData(
                getApiPopularityPopularitytypeidSuspenseQueryOptionsHook({
                    popularityTypeId: 10,
                    params: { Limit: 25 },
                }),
            ),
            queryClient.ensureQueryData(
                getApiPopularityPopularitytypeidSuspenseQueryOptionsHook({
                    popularityTypeId: 9,
                    params: { Limit: 25 },
                }),
            ),
            queryClient.ensureQueryData(
                getApiPopularityPopularitytypeidSuspenseQueryOptionsHook({
                    popularityTypeId: 5,
                    params: { Limit: 25 },
                }),
            ),
        ]);
    },
});

function RouteComponent() {
    const { data: steamMostWishlisted } = useGetApiPopularityPopularitytypeidSuspenseHook({
        popularityTypeId: 10,
        params: { Limit: 25 },
    });

    const { data: steamMostPlayed } = useGetApiPopularityPopularitytypeidSuspenseHook({
        popularityTypeId: 9,
        params: { Limit: 25 },
    });

    const { data: steamPeakHours } = useGetApiPopularityPopularitytypeidSuspenseHook({
        popularityTypeId: 5,
        params: { Limit: 25 },
    });

    const totalIndexed = 360202;

    const latestUpdatedAt = Math.max(
        ...[...steamMostWishlisted.games, ...steamMostPlayed.games].map(
            (game) => game.updatedAt ?? 0,
        ),
    );
    const lastIndexedLabel =
        latestUpdatedAt > 0 ? new Date(latestUpdatedAt * 1000).toLocaleDateString() : "Unknown";

    return (
        <Box maxW="6xl" mx="auto" w="full" px={{ base: "4", md: "6" }} py={{ base: "2", md: "2" }}>
            <VStack align="stretch" gap="md">
                <Box
                    rounded="md"
                    bg="bg.surface"
                    borderWidth="0px"
                    borderColor="border.subtle"
                    p="md"
                    textAlign="center"
                >
                    <VStack gap="xs" align="center">
                        <Heading>game-index.app</Heading>
                        <Text fontSize="sm" color="fg.muted">
                            Discover, search, and explore games directly from IGDB
                        </Text>
                    </VStack>
                </Box>
                <Box
                    rounded="lg"
                    bg="bg.panel"
                    borderWidth="1px"
                    borderColor="border.subtle"
                    p="md"
                >
                    <VStack align="stretch" gap="sm">
                        <Text fontSize="lg" fontWeight="semibold" color="fg.base">
                            Database Stats
                        </Text>

                        <SimpleGrid columns={{ base: 1, md: 2 }} gap="sm">
                            <Box
                                rounded="md"
                                bg="bg.surface"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                p="sm"
                            >
                                <Text color="fg.muted" fontSize="sm">
                                    Games Indexed
                                </Text>
                                <Text color="fg.base" fontSize="2xl" fontWeight="bold">
                                    {Number(totalIndexed).toLocaleString()}
                                </Text>
                            </Box>

                            <Box
                                rounded="md"
                                bg="bg.surface"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                p="sm"
                            >
                                <Text color="fg.muted" fontSize="sm">
                                    Last Indexed
                                </Text>
                                <Text color="fg.base" fontSize="2xl" fontWeight="bold">
                                    {lastIndexedLabel}
                                </Text>
                            </Box>
                        </SimpleGrid>
                    </VStack>
                </Box>
                <Suspense fallback={<Loading.Rings color="blue.500" fontSize="5xl" />}>
                    <SimpleGrid columns={{ base: 1, lg: 2 }} gap="lg">
                        <GridItem>
                            <Box
                                key={"1"}
                                bg="bg.panel"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                rounded="lg"
                                p="md"
                                minW={0}
                            >
                                <Text fontWeight="semibold" color="fg.base" mb="sm">
                                    Most Wishlisted Upcoming
                                </Text>

                                <SimpleTable
                                    headers={["#", "Title", "Release Date"]}
                                    rows={steamMostWishlisted.games.slice(0, 10).map((b, index) => [
                                        b.id ?? index + 1,
                                        <Image
                                            key={`thumb-${b.id ?? index + 1}`}
                                            src={getIGDBImageUrl(b.coverUrl, "cover_big")}
                                            alt={b?.name ?? ""}
                                            w="12"
                                            h="12"
                                            rounded="md"
                                            objectFit="cover"
                                        />,
                                        <Link to="/games/$gameId" params={{ gameId: String(b.id) }}>
                                            {b.name ?? "Unknown"}
                                        </Link>,
                                        formatReleaseDate(b.firstReleaseDate ?? null),
                                    ])}
                                    hoverCardGames={steamMostWishlisted.games.slice(0, 10)}
                                />
                            </Box>
                        </GridItem>
                        <GridItem>
                            <Box
                                key={"2"}
                                bg="bg.panel"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                rounded="lg"
                                p="md"
                                minW={0}
                            >
                                <Text fontWeight="semibold" color="fg.base" mb="sm">
                                    Steam Global Top Sellers
                                </Text>

                                <SimpleTable
                                    headers={["#", "Title"]}
                                    rows={steamMostPlayed.games.slice(0, 10).map((b, index) => [
                                        b.id ?? index + 1,
                                        <Image
                                            key={`thumb-${b.id ?? index + 1}`}
                                            src={getIGDBImageUrl(b.coverUrl, "cover_big")}
                                            alt={b?.name ?? ""}
                                            w="12"
                                            h="12"
                                            rounded="md"
                                            objectFit="cover"
                                        />,
                                        <Link to="/games/$gameId" params={{ gameId: String(b.id) }}>
                                            {b.name ?? "Unknown"}
                                        </Link>,
                                    ])}
                                    hoverCardGames={steamMostPlayed.games.slice(0, 10)}
                                />
                            </Box>
                        </GridItem>
                        <GridItem>
                            <Box
                                key={"2"}
                                bg="bg.panel"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                rounded="lg"
                                p="md"
                                minW={0}
                            >
                                <Text fontWeight="semibold" color="fg.base" mb="sm">
                                    24hr Peak Players
                                </Text>

                                <SimpleTable
                                    headers={["#", "Title"]}
                                    rows={steamPeakHours.games.slice(0, 10).map((b, index) => [
                                        b.id ?? index + 1,
                                        <Image
                                            key={`thumb-${b.id ?? index + 1}`}
                                            src={getIGDBImageUrl(b.coverUrl, "cover_big")}
                                            alt={b?.name ?? ""}
                                            w="12"
                                            h="12"
                                            rounded="md"
                                            objectFit="cover"
                                        />,
                                        <Link to="/games/$gameId" params={{ gameId: String(b.id) }}>
                                            {b.name ?? "Unknown"}
                                        </Link>,
                                    ])}
                                    hoverCardGames={steamPeakHours.games.slice(0, 10)}
                                />
                            </Box>
                        </GridItem>
                    </SimpleGrid>
                </Suspense>
            </VStack>
        </Box>
    );
}

/*
                            <HoverCard key={id as string} openDelay={300} closeDelay={200}>
                                <HoverCardTrigger asChild>{TableRowContent}</HoverCardTrigger>
                                <HoverCardContent
                                    side="left"
                                    align="start"
                                    sideOffset={5}
                                    className="w-80 bg-transparent p-0 border-0"
                                >
                                    {customRenderHoverCard
                                        ? customRenderHoverCard(id as string)
                                        : renderHoverCard(hoverCardType, id as string)}
                                </HoverCardContent>
                            </HoverCard>

*/

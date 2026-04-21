import { SimpleTable } from "@src/components/layout/SimpleTable";
import { createFileRoute, Link } from "@tanstack/react-router";
import { Box, Image, SimpleGrid, Text, VStack, Heading, GridItem, Loading, Container } from "ui";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import {
    popularityTypesGetByIdSuspenseQueryOptionsHook,
    usePopularityTypesGetByIdSuspenseHook,
} from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { Suspense } from "react";

export const Route = createFileRoute("/")({
    component: RouteComponent,
    loader: async ({ context: { queryClient } }) => {
        await Promise.all([
            queryClient.ensureQueryData(
                popularityTypesGetByIdSuspenseQueryOptionsHook({
                    popularityTypeId: 10,
                    params: { Limit: 25 },
                }),
            ),
            queryClient.ensureQueryData(
                popularityTypesGetByIdSuspenseQueryOptionsHook({
                    popularityTypeId: 9,
                    params: { Limit: 25 },
                }),
            ),
            queryClient.ensureQueryData(
                popularityTypesGetByIdSuspenseQueryOptionsHook({
                    popularityTypeId: 5,
                    params: { Limit: 25 },
                }),
            ),
        ]);
    },
});

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

    return (
        <PageWrapper py={{ base: "2", md: "2" }}>
            <VStack align="stretch" gap="md">
                <Container.Root
                    p={{ base: "sm", md: "md" }}
                    rounded="2xl"
                    textAlign="center"
                    variant={"panel"}
                    colorScheme={"fuchsia"}
                >
                    <VStack gap="xs" align="center">
                        <Heading fontSize={{ base: "2xl", md: "3xl" }}>game-index.app</Heading>
                        <Text fontSize="sm" color="fg.muted">
                            Discover, search, and explore games directly from IGDB
                        </Text>
                    </VStack>
                </Container.Root>
                <Container.Root p={{ base: "sm", md: "md" }} rounded="2xl" variant={"panel"}>
                    <VStack align="stretch" gap="sm">
                        <Text fontSize="lg" fontWeight="semibold" color="fg.base">
                            Database Stats
                        </Text>

                        <SimpleGrid columns={{ base: 1, md: 2 }} gap="sm">
                            <Box p="sm" rounded="2xl">
                                <Text color="fg.muted" fontSize="sm">
                                    Games Indexed
                                </Text>
                                <Text color="fg.base" fontSize="2xl" fontWeight="bold">
                                    aaa
                                </Text>
                            </Box>

                            <Box p="sm" rounded="2xl">
                                <Text color="fg.muted" fontSize="sm">
                                    Last Indexed
                                </Text>
                                <Text color="fg.base" fontSize="2xl" fontWeight="bold">
                                    aaaaaaa
                                </Text>
                            </Box>
                        </SimpleGrid>
                    </VStack>
                </Container.Root>
                <Suspense fallback={<Loading.Rings color="blue.500" fontSize="5xl" />}>
                    <SimpleGrid columns={{ base: 1, lg: 2 }} gap="lg">
                        <GridItem>
                            <Container.Root rounded="2xl">
                                <Container.Header>
                                    <Text fontWeight="semibold" color="fg.base" mb="sm">
                                        Most Wishlisted Upcoming
                                    </Text>
                                </Container.Header>
                                <Container.Body>
                                    <SimpleTable
                                        headers={["#", "Title", "Release Date"]}
                                        rows={steamMostWishlisted.games
                                            .slice(0, 10)
                                            .map((b, index) => [
                                                b.id ?? index + 1,
                                                <Image
                                                    key={`thumb-${b.id ?? index + 1}`}
                                                    src={getIGDBImageUrl(b.coverUrl, "cover_big")}
                                                    alt={b?.name ?? ""}
                                                    w={{ base: "10", md: "12" }}
                                                    h={{ base: "10", md: "12" }}
                                                    rounded="md"
                                                    objectFit="cover"
                                                />,
                                                <Link
                                                    to="/games/$gameId"
                                                    params={{ gameId: String(b.id) }}
                                                >
                                                    {b.name ?? "Unknown"}
                                                </Link>,
                                                b.firstReleaseDate,
                                            ])}
                                        hoverCardGames={steamMostWishlisted.games.slice(0, 10)}
                                    />
                                </Container.Body>
                            </Container.Root>
                        </GridItem>
                        <GridItem>
                            <Container.Root rounded="2xl">
                                <Container.Header>
                                    <Text fontWeight="semibold" color="fg.base" mb="sm">
                                        Steam Global Top Sellers
                                    </Text>
                                </Container.Header>
                                <Container.Body>
                                    <SimpleTable
                                        headers={["#", "Title"]}
                                        rows={steamMostPlayed.games.slice(0, 10).map((b, index) => [
                                            b.id ?? index + 1,
                                            <Image
                                                key={`thumb-${b.id ?? index + 1}`}
                                                src={getIGDBImageUrl(b.coverUrl, "cover_big")}
                                                alt={b?.name ?? ""}
                                                w={{ base: "10", md: "12" }}
                                                h={{ base: "10", md: "12" }}
                                                rounded="md"
                                                objectFit="cover"
                                            />,
                                            <Link
                                                to="/games/$gameId"
                                                params={{ gameId: String(b.id) }}
                                            >
                                                {b.name ?? "Unknown"}
                                            </Link>,
                                        ])}
                                        hoverCardGames={steamMostPlayed.games.slice(0, 10)}
                                    />
                                </Container.Body>
                            </Container.Root>
                        </GridItem>
                        <GridItem>
                            <Container.Root rounded="2xl">
                                <Container.Header>
                                    <Text fontWeight="semibold" color="fg.base" mb="sm">
                                        24hr Peak Players
                                    </Text>
                                </Container.Header>

                                <Container.Body>
                                    <SimpleTable
                                        headers={["#", "Title"]}
                                        rows={steamPeakHours.games.slice(0, 10).map((b, index) => [
                                            b.id ?? index + 1,
                                            <Image
                                                key={`thumb-${b.id ?? index + 1}`}
                                                src={getIGDBImageUrl(b.coverUrl, "cover_big")}
                                                alt={b?.name ?? ""}
                                                w={{ base: "10", md: "12" }}
                                                h={{ base: "10", md: "12" }}
                                                rounded="md"
                                                objectFit="cover"
                                            />,
                                            <Link
                                                to="/games/$gameId"
                                                params={{ gameId: String(b.id) }}
                                            >
                                                {b.name ?? "Unknown"}
                                            </Link>,
                                        ])}
                                        hoverCardGames={steamPeakHours.games.slice(0, 10)}
                                    />
                                </Container.Body>
                            </Container.Root>
                        </GridItem>
                    </SimpleGrid>
                </Suspense>
            </VStack>
        </PageWrapper>
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

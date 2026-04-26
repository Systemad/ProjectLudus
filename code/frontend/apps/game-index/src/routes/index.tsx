import { SimpleTable } from "@src/components/layout/SimpleTable";
import { createFileRoute, Link } from "@tanstack/react-router";
import {
    Box,
    Format,
    HStack,
    Image,
    SimpleGrid,
    Text,
    VStack,
    Heading,
    GridItem,
    Loading,
    Container,
} from "ui";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import {
    usePopularityTypesGetByIdSuspenseHook,
    useStatsGetStatsSuspenseHook,
} from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { isTbaReleaseDate } from "@src/utils/dateUtils";
import { Suspense } from "react";

export const Route = createFileRoute("/")({
    component: RouteComponent,
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

    const { data: stats } = useStatsGetStatsSuspenseHook();

    return (
        <Suspense
            fallback={
                <PageWrapper py={{ base: "2", md: "2" }}>
                    <VStack align="center" justify="center" minH="60vh">
                        <Loading.Rings color="blue.500" fontSize="5xl" />
                    </VStack>
                </PageWrapper>
            }
        >
            <PageWrapper py={{ base: "2", md: "2" }}>
                <VStack align="stretch" gap="md">
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
                    <Container.Root
                        p={{ base: "sm", md: "md" }}
                        rounded="2xl"
                        variant={"panel"}
                        border="none"
                    >
                        <VStack align="stretch" gap="sm">
                            <HStack justify="center" align="stretch" gap="4" wrap="wrap">
                                <Box p="sm" rounded="2xl" flex="1" minW="10rem" maxW="20rem">
                                    <Text color="fg.muted" fontSize="sm" textAlign="center">
                                        Games Indexed
                                    </Text>
                                    <Text
                                        color="fg.base"
                                        fontSize="2xl"
                                        fontWeight="bold"
                                        textAlign="center"
                                    >
                                        {stats?.totalGames ?? 0}
                                    </Text>
                                </Box>

                                <Box p="sm" rounded="2xl" flex="1" minW="10rem" maxW="20rem">
                                    <Text color="fg.muted" fontSize="sm" textAlign="center">
                                        Companies Indexed
                                    </Text>
                                    <Text
                                        color="fg.base"
                                        fontSize="2xl"
                                        fontWeight="bold"
                                        textAlign="center"
                                    >
                                        {stats?.totalCompanies ?? 0}
                                    </Text>
                                </Box>
                            </HStack>
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
                                                        src={getIGDBImageUrl(
                                                            b.coverUrl,
                                                            "cover_big",
                                                        )}
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
                                                    !isTbaReleaseDate(b.firstReleaseDate) &&
                                                    b.firstReleaseDate ? (
                                                        <Format.DateTime
                                                            value={new Date(b.firstReleaseDate)}
                                                            month="short"
                                                            day="2-digit"
                                                            year="numeric"
                                                        />
                                                    ) : (
                                                        "TBA"
                                                    ),
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
                                            rows={steamMostPlayed.games
                                                .slice(0, 10)
                                                .map((b, index) => [
                                                    b.id ?? index + 1,
                                                    <Image
                                                        key={`thumb-${b.id ?? index + 1}`}
                                                        src={getIGDBImageUrl(
                                                            b.coverUrl,
                                                            "cover_big",
                                                        )}
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
                                            rows={steamPeakHours.games
                                                .slice(0, 10)
                                                .map((b, index) => [
                                                    b.id ?? index + 1,
                                                    <Image
                                                        key={`thumb-${b.id ?? index + 1}`}
                                                        src={getIGDBImageUrl(
                                                            b.coverUrl,
                                                            "cover_big",
                                                        )}
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
        </Suspense>
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

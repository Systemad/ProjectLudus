import { createFileRoute, Link } from "@tanstack/react-router";
import {
    Grid,
    GridItem,
    Accordion,
    AccordionItem,
    SimpleGrid,
    Card,
    Box,
    Text,
    VStack,
    Flex,
    Tag,
    Button,
    Heading,
    ButtonGroup,
    Wrap,
    Rating,
    useDisclosure,
    Spacer,
    IconButton,
} from "@yamada-ui/react";

import Steam2 from "~/icons/Launchers/SteamIcon2";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
import { ManageGameListsDialog } from "~/features/games/components/Dialogs/ManageGameListsDialog";
import { IGDBImage } from "~/features/games/components/IGDBImage";
import { PlatformAccordionCardItem } from "~/features/games/components/Accordion/PlatformAccordionCardItem";
import {
    publicGamesGetGameByIdEndpointHook,
    useMeGameMetadataGetEndpointSuspenseHook,
    usePublicGamesGetSimilarGamesEndpointSuspenseHook,
} from "~/gen";
import { useHype } from "~/features/games/components/Hype/hooks/useHype";
import { useWishlist } from "~/features/games/components/Wishlist/hooks/useWishlist";
import { BookmarkSimpleIcon, FlameIcon } from "@phosphor-icons/react";
import {
    GameCarousel,
    type MediaItem,
} from "~/features/games/components/Carousel/GameCarousel";
import { ReleaseDateAccordionItem } from "~/features/games/components/Accordion/ReleaseDateAccordionItem";

export const Route = createFileRoute("/games/$gameId")({
    component: RouteComponent,
    loader: ({ context: { queryClient }, params: { gameId } }) => {
        const id = parseInt(gameId);
        return queryClient.ensureQueryData({
            queryKey: ["games", id],
            queryFn: () => publicGamesGetGameByIdEndpointHook({ gameId: id }),
        });
    },
});

function RouteComponent() {
    const { gameId } = Route.useParams();
    const { game: game } = Route.useLoaderData();

    const id = parseInt(gameId);
    const { open, onOpen, onClose } = useDisclosure();

    const { data: metadata } = useMeGameMetadataGetEndpointSuspenseHook(
        { gameId: id },
        { query: { queryKey: ["games", "metadata", id] } }
    );
    const { toggleHype } = useHype();
    const { toggleWishlist } = useWishlist();
    const {
        isPending: simGamesPending,
        isError: isSimGamesError,
        data: simGamesData,
    } = usePublicGamesGetSimilarGamesEndpointSuspenseHook(
        { gameId: id },
        { query: { queryKey: ["games", "similar", id] } }
    );

    const items: MediaItem[] = [
        ...game.screenshots.slice(0, 2).map((s) => ({
            type: "screenshot" as const,
            id: s.id,
            imageId: s.image_id,
        })),
        ...game.artworks.slice(0, 2).map((a) => ({
            type: "artwork" as const,
            id: a.id,
            imageId: a.image_id,
        })),
        ...game.videos.slice(0, 2).map((v) => ({
            type: "video" as const,
            id: v.id,
            videoId: v.video_id,
            thumbnail: `https://i.ytimg.com/vi/${v.video_id}/hqdefault.jpg`,
        })),
    ];

    return (
        <VStack>
            <ManageGameListsDialog
                gameId={gameId}
                open={open}
                onClose={onClose}
            />
            <Flex
                flex={1}
                w="full"
                gap="md"
                direction={{ base: "column", md: "row" }}
                borderRadius="xl"
                shadow="none"
                h={{ base: "auto", md: "md" }}
            >
                <Flex h="full">
                    <IGDBImage
                        imageId={game?.cover?.image_id ?? ""}
                        shadow="none"
                        imageSize="cover_big"
                        borderRadius={"lg"}
                    ></IGDBImage>
                </Flex>

                <Flex
                    gap="md"
                    p="md"
                    justify="space-between"
                    direction={"column"}
                    w="full"
                    bg={["blackAlpha.50", "whiteAlpha.100"]}
                    borderRadius={"lg"}
                    shadow="none"
                >
                    <Flex direction={"column"} gap="md">
                        <Wrap gap={6} alignItems={"center"}>
                            <Heading>{game?.name}</Heading>
                            <Tag w="fit-content" size="lg" rounded={"xl"}>
                                Rank #1
                            </Tag>
                            <Rating
                                readOnly
                                fractions={4}
                                items={10}
                                size={"lg"}
                                value={game.totalRating ?? 0}
                                defaultValue={3}
                            />
                            <Spacer />

                            {metadata && (
                                <>
                                    <IconButton
                                        onClick={() =>
                                            toggleWishlist(
                                                game.id,
                                                metadata.isWishlisted,
                                                game.name
                                            )
                                        }
                                        colorScheme="primary"
                                        variant="primary"
                                        size="lg"
                                        icon={
                                            <BookmarkSimpleIcon
                                                size="full"
                                                weight={
                                                    metadata.isWishlisted
                                                        ? "fill"
                                                        : "regular"
                                                }
                                                color="var(--ui-colors-yellow-500)"
                                            />
                                        }
                                    />
                                    <IconButton
                                        onClick={() =>
                                            toggleHype(
                                                game.id,
                                                metadata.isHyped,
                                                game.name
                                            )
                                        }
                                        colorScheme="primary"
                                        variant="primary"
                                        size="lg"
                                        icon={
                                            <FlameIcon
                                                size="full"
                                                weight={
                                                    metadata.isHyped
                                                        ? "fill"
                                                        : "regular"
                                                }
                                                color="var(--ui-colors-red-500)"
                                            />
                                        }
                                    />
                                </>
                            )}
                        </Wrap>
                    </Flex>
                    <VStack gap="xs">
                        <Text fontSize="lg" as="div">
                            <Text fontWeight="bold" as="span">
                                Developers
                            </Text>
                            :{" "}
                            <Wrap gap="xs" direction="row" as="span">
                                {game?.involvedCompanies
                                    .filter((x) => x.developer)
                                    .map((company, i, arr) => (
                                        <Link
                                            key={company.company.id}
                                            to="/company/$companyId"
                                            params={{
                                                companyId:
                                                    company.company.id.toString(),
                                            }}
                                        >
                                            {company.company.name}
                                            {i < arr.length - 1 ? ", " : ""}
                                        </Link>
                                    ))}
                            </Wrap>
                        </Text>

                        <Text fontSize="lg" as="div">
                            <Text fontWeight="bold" as="span">
                                Developers
                            </Text>
                            :
                            <Wrap gap="xs" direction="row" as="span">
                                {game?.involvedCompanies
                                    .filter((x) => x.developer)
                                    .map((company, i, arr) => (
                                        <Link
                                            key={company.company.id}
                                            to="/company/$companyId"
                                            params={{
                                                companyId:
                                                    company.company.id.toString(),
                                            }}
                                        >
                                            {company.company.name}
                                            {i < arr.length - 1 ? ", " : ""}
                                        </Link>
                                    ))}
                            </Wrap>
                        </Text>
                    </VStack>
                    <Flex
                        justify="space-between"
                        wrap={{ base: "wrap", md: "nowrap" }}
                        gap="sm"
                    >
                        <Wrap gap="sm">
                            <ButtonGroup size={"lg"} gap="sm">
                                <Button
                                    onClick={onOpen}
                                    rounded="xl"
                                    alignSelf={"start"}
                                    variant="subtle"
                                >
                                    Add to list
                                </Button>
                                <Button
                                    rounded="xl"
                                    alignSelf={"start"}
                                    variant="subtle"
                                    startIcon={<Steam2 />}
                                >
                                    Steam
                                </Button>
                                <Button
                                    rounded="xl"
                                    alignSelf={"start"}
                                    variant="subtle"
                                >
                                    IGDB
                                </Button>
                            </ButtonGroup>
                        </Wrap>
                        <Flex>
                            <Button
                                size={"lg"}
                                rounded="xl"
                                alignSelf={"start"}
                                variant="solid"
                                colorScheme={"emerald"}
                            >
                                Share
                            </Button>
                        </Flex>
                    </Flex>
                </Flex>
            </Flex>

            <Grid
                w="full"
                templateColumns={{
                    sm: "repeat(1, 1fr)",
                    md: "repeat(6, 1fr)",
                    xl: "repeat(6, 1fr)",
                    "2xl": "repeat(6, 1fr)",
                }}
                gap="md"
            >
                <GridItem
                    colSpan={{ sm: 1, md: 4, lg: 4, xl: 4, "2xl": 4 }}
                    w="full"
                    rounded="xl"
                    overflow="hidden"
                >
                    <GameCarousel items={items} />
                    <Box
                        mt="md"
                        w="auto"
                        borderRadius={"lg"}
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        shadow="none"
                        p="md"
                    >
                        <Text>{game?.summary}</Text>
                        <br></br>
                        <Text>{game?.storyline}</Text>
                    </Box>

                    <Box
                        mt="md"
                        w="auto"
                        borderRadius={"lg"}
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        shadow="none"
                        p="md"
                    >
                        <Heading as={"h3"}>Recommended Games</Heading>
                        {isSimGamesError ? (
                            <>Could not load games</>
                        ) : simGamesPending ? (
                            <>Loading</>
                        ) : (
                            <SimpleGrid
                                mt="sm"
                                columns={{ base: 5, md: 2, lg: 3, xl: 4 }}
                                gap="lg"
                            >
                                {simGamesData?.similarGames.map((i) => (
                                    <GridItem key={i.id}>
                                        <HoverGameCard height="xs" item={i} />
                                    </GridItem>
                                ))}
                            </SimpleGrid>
                        )}
                    </Box>

                    <Box
                        mt="md"
                        w="auto"
                        borderRadius={"lg"}
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        shadow="none"
                        p="md"
                    >
                        <Heading as={"h3"}>Genres</Heading>
                        <SimpleGrid columns={{ base: 3 }} gap="md">
                            {Array.from({ length: 6 }, (_, i) => i).map((i) => (
                                <GridItem key={i}>
                                    <Card
                                        borderRadius={"lg"}
                                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                                        shadow="none"
                                        h="5xs"
                                    ></Card>
                                </GridItem>
                            ))}
                        </SimpleGrid>
                    </Box>
                </GridItem>

                <GridItem
                    colSpan={{ sm: 1, md: 2, lg: 2, xl: 2, "2xl": 2 }}
                    rowSpan={3}
                    w="full"
                    minH="4xs"
                    rounded="md"
                >
                    <Accordion
                        defaultIndex={[0, 1, 2]}
                        multiple={true}
                        variant="card"
                    >
                        <AccordionItem rounded="xl" label="Platforms">
                            <SimpleGrid columns={{ base: 2 }} gap="md" p="2">
                                {game?.platforms.map((platform) => (
                                    <GridItem key={platform.id}>
                                        <PlatformAccordionCardItem
                                            text={platform.name}
                                        />
                                    </GridItem>
                                ))}
                            </SimpleGrid>
                        </AccordionItem>

                        <AccordionItem rounded="xl" label="Release dates">
                            <SimpleGrid columns={{ base: 2 }} gap="md" p="2">
                                {game?.releaseDates.map((release) => (
                                    <GridItem key={release.id}>
                                        <ReleaseDateAccordionItem
                                            releaseDate={release}
                                        />
                                    </GridItem>
                                ))}
                            </SimpleGrid>
                        </AccordionItem>
                    </Accordion>
                </GridItem>
            </Grid>
        </VStack>
    );
}

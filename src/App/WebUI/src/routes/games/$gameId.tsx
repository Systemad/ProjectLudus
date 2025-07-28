import { createFileRoute } from "@tanstack/react-router";
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
} from "@yamada-ui/react";
import { Carousel, CarouselSlide } from "@yamada-ui/carousel";
import { useState } from "react";
import Steam2 from "~/icons/Launchers/SteamIcon2";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
import { ManageGameListsDialog } from "~/features/games/components/Dialogs/ManageGameListsDialog";
import { IGDBImage } from "~/features/games/components/IGDBImage";
import { PlatformAccordionCardItem } from "~/features/games/components/Accordion/PlatformAccordionCardItem";
import {
    publicGamesGetGameByIdEndpointHook,
    usePublicGamesGetSimilarGamesEndpointHook,
} from "~/gen";

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
    const [index, onChange] = useState<number>(0);
    const { open, onOpen, onClose } = useDisclosure();

    const {
        isPending: simGamesPending,
        isError: isSimGamesError,
        data: simGamesData,
    } = usePublicGamesGetSimilarGamesEndpointHook(
        { gameId: id },
        { query: { queryKey: ["games", "similar"] } }
    );

    /*
    if (isPending) {
        return <span>Loading...</span>;
    }

    if (isError) {
        return <span>Error: {error.message}</span>;
    }
*/
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
                direction={{ sm: "row", md: "row", xl: "row", "2xl": "row" }}
                borderRadius={"xl"}
                shadow="none"
                h={{ sm: "xs", md: "2xs", xl: "xs", "2xl": "md" }}
            >
                <Flex h="full">
                    <IGDBImage
                        imageId={game?.cover.image_id}
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
                                defaultValue={3}
                            />
                        </Wrap>
                    </Flex>
                    <VStack gap="xs">
                        <Text fontSize={"lg"}>
                            <Text fontWeight={"bold"} as={"span"}>
                                Developer
                            </Text>
                            :
                            {
                                game?.involved_companies.find(
                                    (x) => x.developer == true
                                )?.company.name
                            }
                        </Text>
                        <Text fontSize={"lg"}>
                            <Text fontWeight={"bold"} as={"span"}>
                                Publisher
                            </Text>
                            :
                            {
                                game?.involved_companies.find(
                                    (x) => x.publisher == true
                                )?.company.name
                            }
                        </Text>
                    </VStack>
                    <Flex justifyContent={"space-between"}>
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
                    <Carousel
                        rounded="xl"
                        gap={4}
                        height={"xl"}
                        withControls={true}
                        withIndicators={false}
                        index={index}
                        onChange={onChange}
                    >
                        {game?.artworks.map((i) => (
                            <CarouselSlide
                                key={i.id}
                                as={IGDBImage}
                                imageId={i.image_id}
                                bg="primary"
                                rounded="xl"
                                imageSize="1080p"
                            ></CarouselSlide>
                        ))}
                    </Carousel>
                    <Box
                        w="auto"
                        rounded="xl"
                        mt="sm"
                        overflow="hidden"
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                    >
                        <Carousel
                            index={index}
                            containScroll="trimSnaps"
                            slideSize="15%"
                            align="center"
                            dragFree
                            h="4xs"
                            loop={false}
                            gap={2}
                            withIndicators={false}
                            withControls={false}
                        >
                            {game?.artworks.map((i, idx) => (
                                <CarouselSlide
                                    key={i.id}
                                    as={IGDBImage}
                                    onClick={() => onChange(idx)}
                                    imageId={i.image_id}
                                    rounded="xl"
                                    imageSize="cover_big"
                                    borderRadius={"lg"}
                                ></CarouselSlide>
                            ))}
                        </Carousel>
                    </Box>
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

                        <AccordionItem rounded="xl" label="Game Launchers">
                            <SimpleGrid columns={{ base: 3 }} gap="md" p="2">
                                {Array.from({ length: 6 }, (_, i) => i).map(
                                    (i) => (
                                        <GridItem key={i}>
                                            <Card
                                                borderRadius={"lg"}
                                                bg={[
                                                    "blackAlpha.50",
                                                    "whiteAlpha.100",
                                                ]}
                                                shadow="none"
                                                h="5xs"
                                            ></Card>
                                        </GridItem>
                                    )
                                )}
                            </SimpleGrid>
                        </AccordionItem>

                        <AccordionItem rounded="xl" label="Release dates">
                            <SimpleGrid columns={{ base: 3 }} gap="md" p="2">
                                {Array.from({ length: 6 }, (_, i) => i).map(
                                    (i) => (
                                        <GridItem key={i}>
                                            <Card
                                                borderRadius={"lg"}
                                                bg={[
                                                    "blackAlpha.50",
                                                    "whiteAlpha.100",
                                                ]}
                                                shadow="none"
                                                h="5xs"
                                            ></Card>
                                        </GridItem>
                                    )
                                )}
                            </SimpleGrid>
                        </AccordionItem>
                    </Accordion>
                </GridItem>
            </Grid>
        </VStack>
    );
}

import { createFileRoute } from "@tanstack/react-router";
import {
    Grid,
    GridItem,
    Center,
    Accordion,
    AccordionItem,
    SimpleGrid,
    Card,
    Box,
    Image,
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
import XboxIcon from "~/icons/Consoles/XboxIcon";
import PlaystationIcon from "~/icons/Consoles/PlaystationIcon";
import UbisoftIcon from "~/icons/Launchers/UbisoftIcon";
import EpicGamesIcon from "~/icons/Launchers/EpicGamesIcon";
import UnrealEngineIcon from "~/icons/GameEngines/UnrealEngineIcon";
import EAIcon from "~/icons/Launchers/EAIcon";
import GodotEngineIcon from "~/icons/GameEngines/GodotEngineIcon";
export const Route = createFileRoute("/games/$gameId")({
    component: RouteComponent,
});

function RouteComponent() {
    const { gameId } = Route.useParams();

    const [index, onChange] = useState<number>(0);
    const { open, onOpen, onClose } = useDisclosure();
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
                    <Image
                        borderRadius={"lg"}
                        bg={"red"}
                        shadow="none"
                        src="https://images.igdb.com/igdb/image/upload/t_cover_big/co4jni.webp"
                    ></Image>
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
                            <Heading>Elden Ring</Heading>
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
                            <XboxIcon />
                            <PlaystationIcon />
                            <UbisoftIcon />
                            <EpicGamesIcon />
                            <UnrealEngineIcon />
                            <EAIcon />
                            <GodotEngineIcon />
                        </Wrap>
                    </Flex>
                    <VStack gap="xs">
                        <Text fontSize={"lg"}>
                            <Text fontWeight={"bold"} as={"span"}>
                                Developer
                            </Text>
                            : Namco
                        </Text>
                        <Text fontSize={"lg"}>
                            <Text fontWeight={"bold"} as={"span"}>
                                Publisher
                            </Text>
                            : Namco
                        </Text>
                        <Text fontSize={"lg"}>
                            <Text fontWeight={"bold"} as={"span"}>
                                Age rating
                            </Text>
                            : Namco
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
                        {Array.from({ length: 10 }, (_, i) => i).map((i) => (
                            <CarouselSlide
                                key={i}
                                as={Center}
                                bg="primary"
                                rounded="xl"
                            >
                                {i}
                            </CarouselSlide>
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
                            {Array.from({ length: 10 }, (_, i) => i).map(
                                (i) => (
                                    <CarouselSlide
                                        key={i}
                                        bg={index === i ? "red" : "primary"}
                                        onClick={() => onChange(i)}
                                        as={Center}
                                        rounded="xl"
                                    >
                                        {i}
                                    </CarouselSlide>
                                )
                            )}
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
                        <Text>
                            Elden Ring is an action RPG developed by
                            FromSoftware and published by Bandai Namco
                            Entertainment, released in February 2022. Directed
                            by Hidetaka Miyazaki, with world-building
                            contributions from novelist George R. R. Martin, the
                            game features an expansive open world called the
                            Lands Between. Players assume the role of a
                            customisable character known as the Tarnished, who
                            must explore this world, battle formidable enemies,
                            and seek to restore the Elden Ring to become the
                            Elden Lord.
                        </Text>
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
                        <SimpleGrid
                            mt="sm"
                            columns={{ base: 5, md: 2, lg: 3, xl: 4 }}
                            gap="lg"
                        >
                            {Array.from({ length: 8 }, (_, i) => i).map((i) => (
                                <GridItem key={i}>
                                    <HoverGameCard height="xs" id={i} />
                                </GridItem>
                            ))}
                        </SimpleGrid>
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

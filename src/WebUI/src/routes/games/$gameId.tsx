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
    HStack,
    VStack,
    Flex,
    Tag,
    Button,
    Heading,
    ButtonGroup,
    Wrap,
} from "@yamada-ui/react";
import {
    Carousel,
    CarouselSlide,
    CarouselIndicators,
} from "@yamada-ui/carousel";
import { useState } from "react";
import Steam2 from "~/icons/SteamIcon2";
export const Route = createFileRoute("/games/$gameId")({
    component: RouteComponent,
});

/*

                <Carousel rounded="xl" gap={4} autoplay withControls={false}>
                    <CarouselSlide as={Center} bg="primary" rounded="xl">
                        1
                    </CarouselSlide>
                    <CarouselSlide as={Center} bg="secondary" rounded="xl">
                        2
                    </CarouselSlide>
                    <CarouselSlide as={Center} bg="warning" rounded="xl">
                        3
                    </CarouselSlide>
                    <CarouselSlide as={Center} bg="danger" rounded="xl">
                        4
                    </CarouselSlide>

                    <CarouselIndicators
                        sx={{
                            "& > *": {
                                w: "4",
                                _selected: { w: "12" },
                                transitionProperty: "width",
                                transitionDuration: "slower",
                            },
                        }}
                    />
                </Carousel>

*/
function RouteComponent() {
    const [index, onChange] = useState<number>(0);

    return (
        <VStack>
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
                    justify="flex-end"
                    direction={"column"}
                    w="full"
                    bg={["blackAlpha.50", "whiteAlpha.100"]}
                    borderRadius={"lg"}
                    shadow="none"
                >
                    <Flex direction={"column"} gap="md">
                        <Heading>Elden Ring</Heading>
                        <Tag w="fit-content" size="lg" rounded={"xl"}>
                            Rank #1
                        </Tag>
                    </Flex>

                    <Flex justifyContent={"space-between"}>
                        <Wrap gap="sm">
                            <ButtonGroup gap="sm">
                                <Button
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
                    minH="4xs"
                    rounded="xl"
                >
                    <Carousel
                        rounded="xl"
                        gap={4}
                        autoplay
                        withControls={true}
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

                        <CarouselIndicators
                            sx={{
                                "& > *": {
                                    w: "4",
                                    _selected: { w: "12" },
                                    transitionProperty: "width",
                                    transitionDuration: "slower",
                                },
                            }}
                        />
                    </Carousel>
                    <Box
                        w="auto"
                        rounded="xl"
                        mt="2"
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                    >
                        <Carousel
                            index={index}
                            containScroll="trimSnaps"
                            slideSize="18%"
                            align="start"
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
                                        borderWidth={5}
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
                        mt="2"
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

                        <AccordionItem rounded="xl" label="Genres">
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

/*
                        <Card
                            key={i}
                            flexShrink={0}
                            w="3xs"
                            borderRadius={"lg"}
                            bg={["blackAlpha.50", "whiteAlpha.100"]}
                            shadow="none"
                            h="4xs"
                        ></Card>








                                        <HStack
                    wrap="nowrap"
                    w={{ base: "auto", sm: "full" }}
                    gap={{ base: "md", sm: "sm" }}
                    overflowX="auto"
                    bg="blue.200"
                >
                    {Array.from({ length: 25 }, (_, i) => i).map((i) => (
                        <Box
                            key={i}
                            h="4xs"
                            w="3xs"
                            overflow="hidden"
                            position="relative"
                            rounded="md"
                            flexShrink={0}
                        >
                            <Image
                                objectFit="cover"
                                sizes="100%"
                                src="https://images.igdb.com/igdb/image/upload/t_720p/scsodz.webp"
                            />
                        </Box>
                    ))}
                </HStack>
*/

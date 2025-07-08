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
} from "@yamada-ui/react";
import {
    Carousel,
    CarouselSlide,
    CarouselIndicators,
} from "@yamada-ui/carousel";
import { useState } from "react";
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
        <Grid w="full" templateColumns="repeat(6, 1fr)" gap="md">
            <GridItem colSpan={4} w="full" minH="4xs" rounded="xl">
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
                        {Array.from({ length: 10 }, (_, i) => i).map((i) => (
                            <CarouselSlide
                                key={i}
                                border={index === i ? "red" : "transparent"}
                                borderWidth={5}
                                onClick={() => onChange(i)}
                                as={Center}
                                bg="primary"
                                rounded="xl"
                            >
                                {i}
                            </CarouselSlide>
                        ))}
                    </Carousel>
                </Box>
            </GridItem>

            <GridItem colSpan={2} rowSpan={3} w="full" minH="4xs" rounded="md">
                <Accordion
                    defaultIndex={[0, 1, 2]}
                    multiple={true}
                    variant="card"
                >
                    <AccordionItem rounded="xl" label="Platforms">
                        <SimpleGrid columns={{ base: 2 }} gap="md" p="2">
                            {Array.from({ length: 6 }, (_, i) => i).map((i) => (
                                <GridItem key={i}>
                                    <Card
                                        borderRadius={"lg"}
                                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                                        shadow="none"
                                        h="4xs"
                                    ></Card>
                                </GridItem>
                            ))}
                        </SimpleGrid>
                    </AccordionItem>

                    <AccordionItem rounded="xl" label="Genres">
                        <SimpleGrid columns={{ base: 2 }} gap="md" p="2">
                            {Array.from({ length: 6 }, (_, i) => i).map((i) => (
                                <GridItem key={i}>
                                    <Card
                                        borderRadius={"lg"}
                                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                                        shadow="none"
                                        h="4xs"
                                    ></Card>
                                </GridItem>
                            ))}
                        </SimpleGrid>
                    </AccordionItem>

                    <AccordionItem rounded="xl" label="Release dates">
                        <SimpleGrid columns={{ base: 2 }} gap="md" p="2">
                            {Array.from({ length: 6 }, (_, i) => i).map((i) => (
                                <GridItem key={i}>
                                    <Card
                                        borderRadius={"lg"}
                                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                                        shadow="none"
                                        h="4xs"
                                    ></Card>
                                </GridItem>
                            ))}
                        </SimpleGrid>
                    </AccordionItem>
                </Accordion>
            </GridItem>
        </Grid>
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

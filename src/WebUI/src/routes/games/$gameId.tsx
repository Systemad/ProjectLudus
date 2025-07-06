import { createFileRoute } from "@tanstack/react-router";
import {
    Box,
    Grid,
    GridItem,
    Center,
    Accordion,
    AccordionItem,
    AccordionLabel,
    AccordionPanel,
    SimpleGrid,
    Card,
} from "@yamada-ui/react";
import {
    Carousel,
    CarouselSlide,
    CarouselControlNext,
    CarouselControlPrev,
    CarouselIndicators,
} from "@yamada-ui/carousel";
export const Route = createFileRoute("/games/$gameId")({
    component: RouteComponent,
});

function RouteComponent() {
    return (
        <Grid w="full" templateColumns="repeat(6, 1fr)" gap="md">
            <GridItem colSpan={4} w="full" minH="4xs" rounded="md" bg="blue">
                <Carousel gap={0} autoplay withControls={false}>
                    <CarouselSlide as={Center} bg="primary">
                        1
                    </CarouselSlide>
                    <CarouselSlide as={Center} bg="secondary">
                        2
                    </CarouselSlide>
                    <CarouselSlide as={Center} bg="warning">
                        3
                    </CarouselSlide>
                    <CarouselSlide as={Center} bg="danger">
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
            </GridItem>

            <GridItem colSpan={2} rowSpan={3} w="full" minH="4xs" rounded="md">
                <Accordion
                    defaultIndex={[0, 1, 2]}
                    multiple={true}
                    variant="card"
                >
                    <AccordionItem label="Platforms">
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

                    <AccordionItem label="Genres">
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

                    <AccordionItem label="Release dates">
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
                        </SimpleGrid>{" "}
                    </AccordionItem>
                </Accordion>
            </GridItem>
        </Grid>
    );
}

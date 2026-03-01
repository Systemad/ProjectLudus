"use client";

import { createFileRoute } from "@tanstack/react-router";
import {
    HStack,
    Box,
    Button,
    For,
    Card,
    Image,
    CheckboxGroup,
    useDisclosure,
    Pagination,
    Stack,
    Text,
    Heading,
    Drawer,
    Accordion,
    Grid,
    Center,
    GridItem,
    Flex,
    VStack,
} from "@packages/ui";

export const Route = createFileRoute("/search")({
    component: Index,
});

function Index() {
    return (
        <>
            <FacetedSearch />
        </>
    );
}

export const FacetedSearch = () => {
    const { open, onOpen, onClose } = useDisclosure();

    return (
        <VStack gap={"xs"}>
            <Flex
                direction={{ base: "column", md: "row" }}
                flexDirection={{ base: "column", md: "row" }}
                py="4"
                px="2"
                gap="3"
                left="0"
                bg="bg"
                align="center"
            >
                <HStack
                    w={{ base: "full", md: "auto" }}
                    display={{ base: "flex", md: "none" }}
                    justify="space-between"
                >
                    <Button variant="solid" size="sm" onClick={onOpen}>
                        Mobile Filters
                    </Button>
                    <Text>25 products</Text>
                </HStack>
            </Flex>
            <HStack align="flex-start" gap="lg">
                <Box
                    display={{ base: "none", md: "block" }}
                    w="35%"
                    p="md"
                    bg="bg.panel"
                    borderWidth={0}
                    rounded="lg"
                >
                    <Heading size="md" mb="md">
                        Filters
                    </Heading>
                    <FilterFields />
                </Box>

                <Box w="full">
                    <Grid
                        css={{
                            "--min-grid-col-size": {
                                base: "200px",
                                md: "220px",
                            },
                        }}
                        w="full"
                        gap="md"
                        templateColumns="repeat(auto-fill, minmax(min({--min-grid-col-size}, 100%), 1fr))"
                    >
                        <GridItem>
                            <Box
                                position="relative"
                                overflow="hidden"
                                borderRadius="lg"
                            >
                                <Image
                                    src="https://slamdunk-movie.jp/files/images/p_gallery_03.jpg"
                                    alt="Card image"
                                    w="100%"
                                    objectFit="cover"
                                />

                                <Box
                                    position="absolute"
                                    bottom="0"
                                    left="0"
                                    right="0"
                                    p="4"
                                    background="linear-gradient(to top, rgba(0,0,0,0.7), rgba(0,0,0,0))"
                                >
                                    <Text
                                        color="white"
                                        fontWeight="bold"
                                        fontSize="lg"
                                    >
                                        Your Title
                                    </Text>
                                    <Text color="gray.200" fontSize="sm">
                                        Optional subtitle
                                    </Text>
                                </Box>
                            </Box>
                        </GridItem>
                        <For each={Array.from({ length: 20 })}>
                            {(_, index) => (
                                <GridItem key={index}>
                                    <Card.Root
                                        border="none"
                                        height={"auto"}
                                        rounded={"2xl"}
                                        variant={"solid"}
                                    >
                                        <Card.Header
                                            justifyContent="center"
                                            p="0"
                                            m="0"
                                        >
                                            <Image
                                                src="https://slamdunk-movie.jp/files/images/p_gallery_03.jpg"
                                                w="full"
                                                roundedTop="2xl"
                                            />
                                        </Card.Header>

                                        <Card.Body>
                                            <Heading
                                                size="md"
                                                color={"fg.solid"}
                                            >
                                                Grand Theft Auto
                                            </Heading>

                                            <Text>New game in 2026</Text>
                                        </Card.Body>

                                        <Card.Footer>
                                            <Button
                                                rounded="xl"
                                                size={"sm"}
                                                variant={"subtle"}
                                                bg={"colorScheme.subtle/60"}
                                            >
                                                View
                                            </Button>
                                        </Card.Footer>
                                    </Card.Root>
                                </GridItem>
                            )}
                        </For>
                    </Grid>

                    <Center mt="lg">
                        <Pagination.Root total={5} variant={"subtle"} />
                    </Center>
                </Box>
            </HStack>

            <Drawer.Root
                duration={0.3}
                placement={"block-end"}
                open={open}
                onClose={onClose}
                size="xl"
                closeOnDrag
                withDragBar={true}
            >
                <Drawer.Content
                    transition={
                        {
                            enter: {
                                type: "spring",
                                stiffness: 400,
                                damping: 25,
                                mass: 0.9,
                            },
                        } as any
                    }
                >
                    <Drawer.Header>Filters</Drawer.Header>

                    <Drawer.Body>
                        <FilterFields onApply={onClose} />
                    </Drawer.Body>
                </Drawer.Content>
            </Drawer.Root>
        </VStack>
    );
};

interface FilterFieldsProps {
    onApply?: () => void;
}
const FilterFields = ({ onApply }: FilterFieldsProps) => (
    <Stack w="full" h="full">
        <Accordion.Root toggle multiple>
            <Accordion.Item button="Theme" index={0}>
                <Accordion.Panel
                    borderRadius={"lg"}
                    duration={0.25}
                    transition={{
                        enter: {
                            type: "spring",
                            stiffness: 400,
                            damping: 25,
                            mass: 0.7,
                        },
                        exit: {
                            type: "tween",
                            duration: 0.05,
                            ease: [0.4, 0, 1, 1],
                        },
                    }}
                >
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>Action</CheckboxGroup.Item>
                        <CheckboxGroup.Item>RPG</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Open-world</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Genre" index={1}>
                <Accordion.Panel
                    borderRadius={"lg"}
                    duration={0.25}
                    transition={{
                        enter: {
                            type: "spring",
                            stiffness: 400,
                            damping: 20,
                            mass: 0.8,
                        },
                    }}
                >
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>Action</CheckboxGroup.Item>
                        <CheckboxGroup.Item>RPG</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Open-world</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>
        </Accordion.Root>

        {onApply && (
            <Box pt="4" pb="calc(env(safe-area-inset-bottom) + 16px)">
                <Button w="full" size="lg" onClick={onApply}>
                    Show Results
                </Button>
            </Box>
        )}
    </Stack>
);

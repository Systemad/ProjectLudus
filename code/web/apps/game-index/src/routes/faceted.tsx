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
    GridItem,
    Flex,
} from "@packages/ui";

export const Route = createFileRoute("/faceted")({
    component: Index,
});

function Index() {
    return <FacetedSearchTwo />;
}

export const FacetedSearchTwo = () => {
    const { open, onOpen, onClose } = useDisclosure();

    return (
        <Flex
            direction="column"
            bg="bg.surface"
            minH="100vh"
            px={{ base: "md", xl: "lg" }}
            py="xl"
            gap="xl"
        >
            {/* MOBILE FILTER BUTTON */}
            <Flex
                display={{ base: "flex", md: "none" }}
                justify="space-between"
            >
                <Button
                    size="sm"
                    variant="outline"
                    rounded="xl"
                    onClick={onOpen}
                >
                    Filters
                </Button>

                <Text color="fg.muted" fontSize="sm">
                    3,842 entries
                </Text>
            </Flex>

            <Flex align="flex-start" gap="xl">
                {/* SIDEBAR */}
                <Box
                    display={{ base: "none", md: "block" }}
                    w="280px"
                    flexShrink={0}
                    position="sticky"
                    top="xl"
                >
                    <Stack
                        bg="bg.panel"
                        rounded="2xl"
                        p="lg"
                        borderWidth="1px"
                        borderColor="border.subtle"
                        gap="lg"
                    >
                        <Flex justify="space-between" align="center">
                            <Heading size="sm">Refine</Heading>

                            <Button variant="ghost" size="xs">
                                Reset
                            </Button>
                        </Flex>

                        <FilterFields />
                    </Stack>
                </Box>

                {/* CONTENT */}
                <Stack flex="1" gap="xl">
                    {/* ACTIVE FILTERS */}
                    <HStack flexWrap="wrap" gap="sm">
                        <Text fontSize="xs" color="fg.muted">
                            Filters:
                        </Text>

                        <Button size="xs" variant="subtle" rounded="full">
                            Action
                        </Button>

                        <Button size="xs" variant="subtle" rounded="full">
                            PC
                        </Button>

                        <Button size="xs" variant="subtle" rounded="full">
                            2023
                        </Button>

                        <Button size="xs" variant="ghost">
                            Clear
                        </Button>
                    </HStack>

                    {/* GRID */}
                    <Grid
                        gap="lg"
                        templateColumns={{
                            base: "repeat(2, 1fr)",
                            sm: "repeat(auto-fill, minmax(160px, 1fr))",
                            md: "repeat(auto-fill, minmax(200px, 1fr))",
                            xl: "repeat(auto-fill, minmax(220px, 1fr))",
                        }}
                    >
                        <For each={Array.from({ length: 20 })}>
                            {(_, i) => (
                                <GridItem key={i}>
                                    <ExplorerCard />
                                </GridItem>
                            )}
                        </For>
                    </Grid>

                    {/* PAGINATION */}
                    <Flex justify="space-between" align="center">
                        <Button>Load more</Button>
                    </Flex>
                </Stack>
            </Flex>

            {/* MOBILE FILTERS */}
            <Drawer.Root
                placement="block-end"
                open={open}
                onClose={onClose}
                size="xl"
                closeOnDrag
                withDragBar
            >
                <Drawer.Content>
                    <Drawer.Header>Filters</Drawer.Header>

                    <Drawer.Body>
                        <FilterFields onApply={onClose} />
                    </Drawer.Body>
                </Drawer.Content>
            </Drawer.Root>
        </Flex>
    );
};

/* -------------------------------------------------- */
/* CARD */
/* -------------------------------------------------- */

const ExplorerCard = () => {
    return (
        <Card.Root
            bg="bg.panel"
            borderWidth="1px"
            borderColor="border.subtle"
            rounded="2xl"
            overflow="hidden"
            transition="all .2s"
            _hover={{
                transform: "translateY(-4px)",
                bg: "bg.muted",
            }}
        >
            <Box aspectRatio="3/4" overflow="hidden">
                <Image
                    src="https://slamdunk-movie.jp/files/images/p_gallery_03.jpg"
                    w="full"
                    h="full"
                    objectFit="cover"
                    transition="transform .4s"
                    _groupHover={{ transform: "scale(1.1)" }}
                />
            </Box>

            <Card.Body>
                <Stack gap="xs">
                    <Heading size="sm" lineClamp={2}>
                        Neon Protocol: Revelations
                    </Heading>

                    <Text fontSize="xs" color="fg.muted">
                        Cyberpunk • 2023
                    </Text>
                </Stack>
            </Card.Body>

            <Card.Footer>
                <Flex justify="space-between" w="full">
                    <Text fontSize="sm" fontWeight="bold">
                        ★ 9.2
                    </Text>

                    <Button size="xs" variant="ghost">
                        Save
                    </Button>
                </Flex>
            </Card.Footer>
        </Card.Root>
    );
};

/* -------------------------------------------------- */
/* FILTERS */
/* -------------------------------------------------- */

interface FilterFieldsProps {
    onApply?: () => void;
}

const FilterFields = ({ onApply }: FilterFieldsProps) => (
    <Stack gap="lg" w="full" h="full">
        <Accordion.Root toggle multiple>
            <Accordion.Item button="Genre" index={0}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>Action</CheckboxGroup.Item>
                        <CheckboxGroup.Item>RPG</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Strategy</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Simulation</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Platform" index={1}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>PC</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Console</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Release Year" index={2}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>2024</CheckboxGroup.Item>
                        <CheckboxGroup.Item>2023</CheckboxGroup.Item>
                        <CheckboxGroup.Item>2022</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>
        </Accordion.Root>

        {onApply && (
            <Box pt="4" pb="calc(env(safe-area-inset-bottom) + 16px)">
                <Button w="full" size="lg" rounded="2xl" onClick={onApply}>
                    Show Results
                </Button>
            </Box>
        )}
    </Stack>
);

"use client";

import { createFileRoute } from "@tanstack/react-router";
import {
    HStack,
    Box,
    Button,
    DrawerHeader,
    DrawerBody,
    Checkbox,
    For,
    Card,
    Image,
    CheckboxGroup,
    useDisclosure,
    Stack,
    Text,
    Heading,
    Drawer,
    Accordion,
    AccordionPanel,
    Grid,
    GridItem,
} from "@packages/ui";

export const Route = createFileRoute("/")({
    component: Index,
});

function Index() {
    return (
        <div>
            <FacetedSearch />
        </div>
    );
}

export const FacetedSearch = () => {
    const { open, onOpen, onClose } = useDisclosure();

    return (
        <Box p="md">
            <Button
                display={{ base: "block", md: "none" }}
                onClick={onOpen}
                w="full"
                mb="md"
            >
                Filters
            </Button>

            <HStack align="flex-start" gap="lg">
                <Box
                    display={{ base: "none", md: "block" }}
                    w="30%"
                    p="md"
                    border="1px solid"
                    borderColor="border"
                    rounded="md"
                    position="sticky"
                    top="1rem"
                >
                    <Heading size="md" mb="md">
                        Filters
                    </Heading>
                    <FilterFields />
                </Box>

                <Grid
                    w="full"
                    gap="md"
                    justifyContent={"center"}
                    templateColumns="repeat(auto-fill, minmax(min(250px, 100%), 1fr))"
                >
                    <For each={Array.from({ length: 8 })}>
                        {(_, index) => (
                            <GridItem key={index}>
                                <Card.Root maxW="md" rounded={"2xl"}>
                                    <Card.Header
                                        justifyContent="center"
                                        p="0"
                                        m="0"
                                    >
                                        <Image
                                            src="https://slamdunk-movie.jp/files/images/p_gallery_03.jpg"
                                            w="full"
                                            rounded="2xl"
                                        />
                                    </Card.Header>

                                    <Card.Body>
                                        <Heading size="md">
                                            Grand Theft Auto
                                        </Heading>

                                        <Text>New game in 2026</Text>
                                    </Card.Body>

                                    <Card.Footer>
                                        <Button rounded="xl" size={"sm"}>
                                            Wikipedia
                                        </Button>
                                    </Card.Footer>
                                </Card.Root>
                            </GridItem>
                        )}
                    </For>
                </Grid>
            </HStack>

            <Drawer.Root
                placement={"block-start"}
                open={open}
                onClose={onClose}
                size="full"
            >
                <Drawer.Content>
                    <Drawer.Body>
                        <Drawer.Header>Filters</Drawer.Header>
                        <FilterFields onApply={onClose} />
                    </Drawer.Body>
                </Drawer.Content>
            </Drawer.Root>
        </Box>
    );
};

interface FilterFieldsProps {
    onApply?: () => void;
}
const FilterFields = ({ onApply }: FilterFieldsProps) => (
    <Stack w="full">
        <Accordion.Root toggle>
            <Accordion.Item button="Theme" index={0}>
                <Accordion.Panel bg="bg.panel" borderRadius={"lg"} pt={"sm"}>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>Action</CheckboxGroup.Item>
                        <CheckboxGroup.Item>RPG</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Open-world</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>
        </Accordion.Root>

        {onApply && (
            <Button mt="lg" colorScheme="primary" onClick={onApply}>
                Apply Filters
            </Button>
        )}
    </Stack>
);

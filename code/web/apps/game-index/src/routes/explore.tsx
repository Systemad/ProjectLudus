import { createFileRoute } from "@tanstack/react-router";
import {
    Box,
    Card,
    Container,
    Heading,
    Text,
    Image,
    Input,
    Button,
    VStack,
    HStack,
    Badge,
    List,
    Center,
    SimpleGrid,
    Icon,
    MailIcon,
} from "@packages/ui";

export const Route = createFileRoute("/explore")({
    component: Page,
});

function Page() {
    return (
        <Box bg="black" color="white" minH="100vh">
            {/* Replaced generic Box with Container.Root as requested */}
            <Container.Root size="xl" py="xl" px="lg" gap="lg">
                {/* Featured Hero Section */}
                <SimpleGrid columns={{ base: 1, lg: 2 }} gap="md">
                    <Card.Root
                        variant="outline"
                        overflow="hidden"
                        position="relative"
                        minH="md"
                    >
                        <Image
                            src="https://lh3.googleusercontent.com/aida-public/AB6AXuDrhV31xIPyEEpV8Z2HtaH3D-BFSIZtssVM_rsyySeLmC4VVBTkGke6dNVl2fTizEodmC0_xlXbECLefHIe2faK3a6KdevAB-f6JZPnxYg8HaOigYVT-zUp4HB1RVwp4CxPczrSX2Vz-4lxIMPrpYD8-3m1BNxrjWgJiJAFxHJ45zjsnC9VzA1lydtQOr5JpHjwigiCDywbpcL0rsFf7TbDw2rRiK5qvuQgJDB8tdgiAYx-peNLDEEMfOpyUWbnevRMWoFjDVU-nS5C"
                            position="absolute"
                            inset="0"
                            w="full"
                            h="full"
                            objectFit="cover"
                            _hover={{ transform: "scale(1.05)" }}
                            transitionProperty="transform"
                            transitionDuration="slower"
                        />
                        <Box
                            position="absolute"
                            inset="0"
                            bgGradient="linear(to-t, black, transparent)"
                            opacity="0.9"
                        />
                        <Card.Body
                            position="relative"
                            p="lg"
                            justifyContent="flex-end"
                        >
                            <HStack mb="sm">
                                <Badge colorScheme="indigo" variant="solid">
                                    Trending Now
                                </Badge>
                                <Text
                                    fontSize="xs"
                                    fontWeight="black"
                                    color="whiteAlpha.600"
                                    letterSpacing="widest"
                                >
                                    DIGITAL DELUXE
                                </Text>
                            </HStack>
                            <Heading
                                size="2xl"
                                color="white"
                                mb="md"
                                fontWeight="black"
                            >
                                Neon Protocol: Revelations
                            </Heading>
                            <Card.Footer p="0">
                                <HStack gap="md">
                                    <HStack gap="xs">
                                        {["08", "14", "22"].map((val, i) => (
                                            <VStack
                                                key={i}
                                                bg="blackAlpha.500"
                                                backdropFilter="blur(md)"
                                                rounded="md"
                                                px="sm"
                                                py="xs"
                                                minW="12"
                                            >
                                                <Text
                                                    color="white"
                                                    fontWeight="black"
                                                    fontSize="lg"
                                                >
                                                    {val}
                                                </Text>
                                                <Text
                                                    color="whiteAlpha.500"
                                                    fontSize="xs"
                                                    fontWeight="bold"
                                                >
                                                    UNIT
                                                </Text>
                                            </VStack>
                                        ))}
                                    </HStack>
                                    <Box w="px" h="10" bg="whiteAlpha.200" />
                                    <Box>
                                        <Text
                                            fontSize="xl"
                                            fontWeight="black"
                                            color="white"
                                        >
                                            $59.99
                                        </Text>
                                        <Text
                                            fontSize="xs"
                                            fontWeight="bold"
                                            color="emerald.400"
                                        >
                                            EARLY ACCESS
                                        </Text>
                                    </Box>
                                </HStack>
                            </Card.Footer>
                        </Card.Body>
                    </Card.Root>

                    <Card.Root
                        variant="outline"
                        overflow="hidden"
                        position="relative"
                        minH="md"
                    >
                        <Image
                            src="https://lh3.googleusercontent.com/aida-public/AB6AXuCHL_FDYUsD-boOxzYl9s5SBBN2OOVV5cB9gEpoXr9d0qdzQWwo6rvCgGZBpBN4PQtX0Pp1mRE8Ni9eFlMD0gEKRNq_cCgFxBLpoYxSaLJ16JS9QHHnvjyZk3NEtvl2-wpBnk7_-Bfjbm6RZWMMS77H7UKpHG16KHq3Lulw8bnauZP1XXS3LFJmXcZbhIM-lclkx2dKw0emlP1eQbLa60OYNPwL7IydV0WgSHnyJQbT05gE6NfVJH4ekWURnvT-j9wCmO17QEqVj9yS"
                            position="absolute"
                            inset="0"
                            w="full"
                            h="full"
                            objectFit="cover"
                            _hover={{ transform: "scale(1.05)" }}
                            transitionProperty="transform"
                            transitionDuration="slower"
                        />
                        <Box
                            position="absolute"
                            inset="0"
                            bgGradient="linear(to-t, black, transparent)"
                            opacity="0.9"
                        />
                        <Card.Body
                            position="relative"
                            p="lg"
                            justifyContent="flex-end"
                        >
                            <HStack mb="sm">
                                <Badge
                                    colorScheme="emerald"
                                    variant="solid"
                                    color="black"
                                >
                                    Must Play
                                </Badge>
                                <Text
                                    fontSize="xs"
                                    fontWeight="black"
                                    color="whiteAlpha.600"
                                    letterSpacing="widest"
                                >
                                    OPEN WORLD
                                </Text>
                            </HStack>
                            <Heading
                                size="2xl"
                                color="white"
                                mb="md"
                                fontWeight="black"
                            >
                                Verdant Odyssey
                            </Heading>
                            <Card.Footer p="0">
                                <HStack gap="lg">
                                    <VStack align="start" gap="0">
                                        <Text
                                            color="whiteAlpha.500"
                                            fontSize="xs"
                                            fontWeight="bold"
                                        >
                                            METASCORE
                                        </Text>
                                        <Text
                                            fontSize="2xl"
                                            fontWeight="black"
                                            color="emerald.400"
                                        >
                                            96
                                        </Text>
                                    </VStack>
                                    <Box w="px" h="10" bg="whiteAlpha.200" />
                                    <VStack align="start" gap="0">
                                        <Text
                                            color="whiteAlpha.500"
                                            fontSize="xs"
                                            fontWeight="bold"
                                        >
                                            COMMUNITY
                                        </Text>
                                        <Text
                                            fontSize="2xl"
                                            fontWeight="black"
                                            color="white"
                                        >
                                            1.2M
                                        </Text>
                                    </VStack>
                                </HStack>
                            </Card.Footer>
                        </Card.Body>
                    </Card.Root>
                </SimpleGrid>

                {/* Info Grid */}
                <SimpleGrid columns={{ base: 1, md: 2, lg: 3 }} gap="lg">
                    <VStack align="stretch" gap="md">
                        <Heading
                            size="xs"
                            color="whiteAlpha.600"
                            letterSpacing="widest"
                        >
                            TOP SELLERS
                        </Heading>
                        <List.Root gap="xs">
                            {[0, 2, 3].map((img, i) => (
                                <HStack
                                    key={i}
                                    p="sm"
                                    rounded="md"
                                    _hover={{ bg: "whiteAlpha.100" }}
                                    cursor="pointer"
                                >
                                    <Text
                                        w="4"
                                        fontSize="xs"
                                        fontWeight="black"
                                        color="whiteAlpha.500"
                                    >
                                        0{i + 1}
                                    </Text>
                                    <Image
                                        src={`http://googleusercontent.com/profile/picture/${img}`}
                                        boxSize="12"
                                        rounded="md"
                                        objectFit="cover"
                                    />
                                    <VStack flex="1" gap="0">
                                        <Text fontSize="sm" fontWeight="bold">
                                            Midnight Gate
                                        </Text>
                                        <Text
                                            fontSize="xs"
                                            color="whiteAlpha.500"
                                        >
                                            PC • PS5
                                        </Text>
                                    </VStack>
                                    <Text fontSize="sm" fontWeight="bold">
                                        $49.99
                                    </Text>
                                </HStack>
                            ))}
                        </List.Root>
                    </VStack>

                    <Container.Root variant="subtle" size="md">
                        <Container.Header gap="xs">
                            <Heading
                                size="xs"
                                color="indigo.400"
                                letterSpacing="widest"
                            >
                                NETWORK PULSE
                            </Heading>
                        </Container.Header>
                        <Container.Body>
                            <Text fontSize="md" fontWeight="medium" mb="md">
                                Database reached{" "}
                                <Text as="span" color="emerald.400">
                                    250k verified
                                </Text>{" "}
                                entries.
                            </Text>
                            <SimpleGrid columns={2} gap="sm">
                                <Box p="md" bg="whiteAlpha.100" rounded="md">
                                    <Text
                                        fontSize="xs"
                                        fontWeight="black"
                                        color="whiteAlpha.500"
                                    >
                                        APIS
                                    </Text>
                                    <Text fontSize="xl" fontWeight="black">
                                        1.4M
                                    </Text>
                                </Box>
                                <Box p="md" bg="whiteAlpha.100" rounded="md">
                                    <Text
                                        fontSize="xs"
                                        fontWeight="black"
                                        color="whiteAlpha.500"
                                    >
                                        NEW
                                    </Text>
                                    <Text
                                        fontSize="xl"
                                        fontWeight="black"
                                        color="emerald.400"
                                    >
                                        +412
                                    </Text>
                                </Box>
                            </SimpleGrid>
                        </Container.Body>
                    </Container.Root>

                    <Container.Root variant="subtle" size="md" centerContent>
                        <Container.Header flexDirection="column">
                            <Center
                                boxSize="12"
                                bg="whiteAlpha.100"
                                rounded="md"
                                mb="md"
                            >
                                <Icon
                                    as={MailIcon}
                                    fontSize="xl"
                                    color="indigo.400"
                                />
                            </Center>
                            <Heading size="sm">Stay Updated</Heading>
                        </Container.Header>
                        <Container.Body textAlign="center">
                            <Text fontSize="xs" color="whiteAlpha.600" mb="md">
                                Weekly curated deals.
                            </Text>
                            <HStack w="full" gap="xs">
                                <Input
                                    variant="filled"
                                    size="sm"
                                    placeholder="Email"
                                    rounded="md"
                                />
                                <Button
                                    size="sm"
                                    bg="white"
                                    color="black"
                                    rounded="md"
                                >
                                    JOIN
                                </Button>
                            </HStack>
                        </Container.Body>
                    </Container.Root>
                </SimpleGrid>
            </Container.Root>
        </Box>
    );
}

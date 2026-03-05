import {
    Card,
    SimpleGrid,
    Image,
    Text,
    Heading,
    Box,
    Flex,
    Badge,
    Icon,
    VStack,
    HStack,
    List,
    TrendingUpIcon,
    DatabaseIcon,
    BadgeCheckIcon,
    ClockIcon,
    MoveRightIcon,
} from "@packages/ui";
import { createFileRoute } from "@tanstack/react-router";
import { useMemo } from "react";

export const Route = createFileRoute("/discover")({
    component: Discover,
});

function Discover() {
    return <ObjectiveDiscovery />;
}

const ObjectiveDiscovery = () => {
    const popularItems = useMemo(
        () => [
            { id: "01", name: "Midnight Gate", stat: "482k Weekly Active" },
            { id: "02", name: "Neon Protocol", stat: "310k Weekly Active" },
        ],
        []
    );

    const anticipatedItems = useMemo(
        () => [
            { id: "01", name: "Aether Drift", stat: "Feb 2025" },
            { id: "02", name: "Void Runner", stat: "Q3 2025" },
        ],
        []
    );

    const consoles = useMemo(
        () => [
            { name: "PlayStation 5", color: "#003791", logo: "/logos/ps5.svg" },
            {
                name: "Xbox Series X",
                color: "#107C10",
                logo: "/logos/xbox.svg",
            },
            {
                name: "Nintendo Switch",
                color: "#E60012",
                logo: "/logos/switch.svg",
            },
        ],
        []
    );

    return (
        <VStack gap="20" align="stretch">
            {/* HERO SECTION */}
            <SimpleGrid columns={{ base: 1, lg: 2 }} gap="6">
                {/* Neon Protocol */}
                <Card.Root
                    h="520px"
                    overflow="hidden"
                    position="relative"
                    cursor="pointer"
                    borderRadius="24px"
                    border="none"
                >
                    <Image
                        src="https://lh3.googleusercontent.com/aida-public/AB6AXuDrhV31xIPyEEpV8Z2HtaH3D-BFSIZtssVM_rsyySeLmC4VVBTkGke6dNVl2fTizEodmC0_xlXbECLefHIe2faK3a6KdevAB-f6JZPnxYg8HaOigYVT-zUp4HB1RVwp4CxPczrSX2Vz-4lxIMPrpYD8-3m1BNxrjWgJiJAFxHJ45zjsnC9VzA1lydtQOr5JpHjwigiCDywbpcL0rsFf7TbDw2rRiK5qvuQgJDB8tdgiAYx-peNLDEEMfOpyUWbnevRMWoFjDVU-nS5C"
                        alt="Neon"
                        position="absolute"
                        inset="0"
                        w="full"
                        h="full"
                        fit="cover"
                    />
                    <Box
                        position="absolute"
                        inset="0"
                        bgGradient="linear(to-t, #111111, transparent)"
                        opacity="0.95"
                    />
                    <Card.Body
                        position="absolute"
                        bottom="0"
                        p="10"
                        w="full"
                        zIndex="1"
                    >
                        <HStack mb="4">
                            <Badge
                                bg="#5e6ad2"
                                color="white"
                                fontSize="9px"
                                fontWeight="900"
                                px="2"
                                py="1"
                                borderRadius="4px"
                            >
                                HIGHEST CONCURRENCY
                            </Badge>
                            <Text
                                color="whiteAlpha.600"
                                fontSize="10px"
                                fontWeight="bold"
                                letterSpacing="widest"
                                textTransform="uppercase"
                            >
                                Technical Masterpiece
                            </Text>
                        </HStack>
                        <Heading
                            size="3xl"
                            color="white"
                            fontWeight="900"
                            mb="6"
                            lineHeight="1.1"
                        >
                            Neon Protocol: Revelations
                        </Heading>
                        <Flex align="center" gap="6">
                            <VStack
                                bg="blackAlpha.300"
                                backdropBlur="md"
                                borderRadius="12px"
                                px="3"
                                py="2"
                                minW="60px"
                                align="center"
                                gap="0"
                            >
                                <Text
                                    color="white"
                                    fontSize="xl"
                                    fontWeight="900"
                                >
                                    94%
                                </Text>
                                <Text
                                    color="whiteAlpha.500"
                                    fontSize="8px"
                                    fontWeight="bold"
                                    textTransform="uppercase"
                                >
                                    Stability
                                </Text>
                            </VStack>
                            <Box h="10" w="1px" bg="whiteAlpha.200" mx="2" />
                            <VStack align="start" gap="0">
                                <Text
                                    fontSize="10px"
                                    fontWeight="bold"
                                    color="#86ead4"
                                    textTransform="uppercase"
                                    letterSpacing="widest"
                                >
                                    Global Peak Reach
                                </Text>
                                <Text
                                    fontSize="2xl"
                                    fontWeight="900"
                                    color="white"
                                >
                                    2.4M Active
                                </Text>
                            </VStack>
                        </Flex>
                    </Card.Body>
                </Card.Root>

                {/* Verdant Odyssey */}
                <Card.Root
                    h="520px"
                    overflow="hidden"
                    position="relative"
                    cursor="pointer"
                    borderRadius="24px"
                    border="none"
                >
                    <Image
                        src="https://lh3.googleusercontent.com/aida-public/AB6AXuCHL_FDYUsD-boOxzYl9s5SBBN2OOVV5cB9gEpoXr9d0qdzQWwo6rvCgGZBpBN4PQtX0Pp1mRE8Ni9eFlMD0gEKRNq_cCgFxBLpoYxSaLJ16JS9QHHnvjyZk3NEtvl2-wpBnk7_-Bfjbm6RZWMMS77H7UKpHG16KHq3Lulw8bnauZP1XXS3LFJmXcZbhIM-lclkx2dKw0emlP1eQbLa60OYNPwL7IydV0WgSHnyJQbT05gE6NfVJH4ekWURnvT-j9wCmO17QEqVj9yS"
                        alt="Verdant"
                        position="absolute"
                        inset="0"
                        w="full"
                        h="full"
                        fit="cover"
                    />
                    <Box
                        position="absolute"
                        inset="0"
                        bgGradient="linear(to-t, #111111, transparent)"
                        opacity="0.95"
                    />
                    <Card.Body
                        position="absolute"
                        bottom="0"
                        p="10"
                        w="full"
                        zIndex="1"
                    >
                        <HStack mb="4">
                            <Badge
                                bg="#86ead4"
                                color="black"
                                fontSize="9px"
                                fontWeight="900"
                                px="2"
                                py="1"
                                borderRadius="4px"
                            >
                                CRITICAL ACCLAIM
                            </Badge>
                        </HStack>
                        <Heading
                            size="3xl"
                            color="white"
                            fontWeight="900"
                            mb="6"
                            lineHeight="1.1"
                        >
                            Verdant Odyssey
                        </Heading>
                        <VStack align="start" gap="1">
                            <Text
                                color="whiteAlpha.500"
                                fontSize="10px"
                                fontWeight="bold"
                            >
                                DATABASE SCORE
                            </Text>
                            <HStack>
                                <Text
                                    fontSize="4xl"
                                    fontWeight="900"
                                    color="#86ead4"
                                >
                                    9.6
                                </Text>
                                <Icon
                                    as={BadgeCheckIcon}
                                    color="#86ead4"
                                    fontSize="2xl"
                                />
                            </HStack>
                        </VStack>
                    </Card.Body>
                </Card.Root>
            </SimpleGrid>

            {/* STATS SECTION */}
            <SimpleGrid columns={{ base: 1, md: 2 }} gap="12">
                <Box>
                    <Heading
                        fontSize="11px"
                        fontWeight="800"
                        color="#6e6e6e"
                        textTransform="uppercase"
                        letterSpacing="0.2em"
                        mb="6"
                        display="flex"
                        alignItems="center"
                        gap="3"
                    >
                        <Icon as={TrendingUpIcon} /> Most Popular This Month
                    </Heading>
                    <List.Root styleType="none">
                        {popularItems.map((item) => (
                            <List.Item key={item.id}>
                                <StatItem
                                    rank={item.id}
                                    name={item.name}
                                    stat={item.stat}
                                />
                            </List.Item>
                        ))}
                    </List.Root>
                </Box>

                <Box>
                    <Heading
                        fontSize="11px"
                        fontWeight="800"
                        color="#6e6e6e"
                        textTransform="uppercase"
                        letterSpacing="0.2em"
                        mb="6"
                        display="flex"
                        alignItems="center"
                        gap="3"
                    >
                        <Icon as={ClockIcon} /> Most Anticipated
                    </Heading>
                    <List.Root styleType="none">
                        {anticipatedItems.map((item) => (
                            <List.Item key={item.id}>
                                <StatItem
                                    rank={item.id}
                                    name={item.name}
                                    stat={item.stat}
                                />
                            </List.Item>
                        ))}
                    </List.Root>
                </Box>
            </SimpleGrid>

            {/* BROWSE BY CONSOLE */}
            <Box>
                <Heading
                    fontSize="11px"
                    fontWeight="800"
                    color="#6e6e6e"
                    textTransform="uppercase"
                    mb="4"
                >
                    Browse by Console
                </Heading>
                <HStack gap="4">
                    {consoles.map((console) => (
                        <Card.Root
                            key={console.name}
                            cursor="pointer"
                            bg={console.color + "20"}
                            borderRadius="20px"
                            p="4"
                            _hover={{ transform: "translateY(-2px)" }}
                        >
                            <VStack align="center" gap="2">
                                <Image
                                    src={console.logo}
                                    alt={console.name}
                                    w="16"
                                    h="16"
                                />
                                <Text fontWeight="bold" color="white">
                                    {console.name}
                                </Text>
                            </VStack>
                        </Card.Root>
                    ))}
                </HStack>
            </Box>

            {/* CATALOG GRID */}
            <Box>
                <Flex justify="space-between" align="center" mb="8">
                    <Heading
                        fontSize="11px"
                        fontWeight="800"
                        color="#6e6e6e"
                        textTransform="uppercase"
                        letterSpacing="0.2em"
                    >
                        <Icon as={DatabaseIcon} mr="2" /> Latest Catalog
                        Additions
                    </Heading>
                    <Text
                        fontSize="11px"
                        fontWeight="900"
                        color="#5e6ad2"
                        cursor="pointer"
                        letterSpacing="widest"
                    >
                        FULL DATABASE INDEX
                    </Text>
                </Flex>
                <SimpleGrid columns={{ base: 2, md: 4, xl: 6 }} gap="6">
                    {[...Array(6)].map((_, i) => (
                        <Card.Root
                            key={i}
                            bg="#191919"
                            borderRadius="24px"
                            overflow="hidden"
                            cursor="pointer"
                            border="none"
                            _hover={{ transform: "translateY(-2px)" }}
                        >
                            <Box aspectRatio={3 / 4} position="relative">
                                <Image
                                    src={`http://googleusercontent.com/profile/picture/${i}`}
                                    w="full"
                                    h="full"
                                    fit="cover"
                                />
                                <Badge
                                    position="absolute"
                                    bottom="3"
                                    right="3"
                                    bg="#111111E6"
                                    color="#eeeeee"
                                    backdropBlur="sm"
                                    px="2"
                                    py="1"
                                    borderRadius="8px"
                                    zIndex="1"
                                >
                                    8.4
                                </Badge>
                            </Box>
                            <Card.Body p="4">
                                <Text fontSize="13px" fontWeight="bold" mb="2">
                                    Shadow Realm
                                </Text>
                                <Badge
                                    bg="#222222"
                                    color="#b0b0b0"
                                    fontSize="9px"
                                    borderRadius="4px"
                                >
                                    SOULSLIKE
                                </Badge>
                            </Card.Body>
                        </Card.Root>
                    ))}
                </SimpleGrid>
            </Box>
        </VStack>
    );
};

const StatItem = ({
    rank,
    name,
    stat,
}: {
    rank: string;
    name: string;
    stat: string;
}) => (
    <Flex
        w="full"
        p="4"
        align="center"
        gap="4"
        borderRadius="18px"
        _hover={{ bg: "#222222" }}
        cursor="pointer"
    >
        <Text fontSize="sm" fontWeight="900" color="#6e6e6e" w="6">
            {rank}
        </Text>
        <Image
            src="https://lh3.googleusercontent.com/aida-public/AB6AXuDrhV31xIPyEEpV8Z2HtaH3D-BFSIZtssVM_rsyySeLmC4VVBTkGke6dNVl2fTizEodmC0_xlXbECLefHIe2faK3a6KdevAB-f6JZPnxYg8HaOigYVT-zUp4HB1RVwp4CxPczrSX2Vz-4lxIMPrpYD8-3m1BNxrjWgJiJAFxHJ45zjsnC9VzA1lydtQOr5JpHjwigiCDywbpcL0rsFf7TbDw2rRiK5qvuQgJDB8tdgiAYx-peNLDEEMfOpyUWbnevRMWoFjDVU-nS5C"
            w="14"
            h="14"
            borderRadius="12px"
            fit="cover"
        />
        <Box flex="1">
            <Text fontSize="14px" fontWeight="bold">
                {name}
            </Text>
            <Text
                fontSize="10px"
                color="#6e6e6e"
                fontWeight="bold"
                textTransform="uppercase"
            >
                {stat}
            </Text>
        </Box>
        <Icon as={MoveRightIcon} color="#6e6e6e" fontSize="xs" />
    </Flex>
);

"use client";

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

export const Route = createFileRoute("/")({
    component: Index,
});

function Index() {
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

    return (
        <VStack
            gap="xl"
            align="stretch"
            py="xl"
            px={{ base: "md", xl: "lg" }}
            minH="100vh"
        >
            <SimpleGrid columns={{ base: 1, md: 2 }} gap="lg">
                <Box>
                    <Heading
                        fontSize="xs"
                        fontWeight="800"
                        textTransform="uppercase"
                        letterSpacing="widest"
                        mb="lg"
                        display="flex"
                        alignItems="center"
                        gap="md"
                    >
                        <Icon as={TrendingUpIcon} /> Most Popular This Month
                    </Heading>
                    <List.Root>
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
                        fontSize="xs"
                        fontWeight="800"
                        textTransform="uppercase"
                        letterSpacing="widest"
                        mb="lg"
                        display="flex"
                        alignItems="center"
                        gap="md"
                    >
                        <Icon as={ClockIcon} /> Upcoming Games
                    </Heading>
                    <List.Root>
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

            <ConsoleSection />

            <TrendingGames />
        </VStack>
    );
}

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
        p="md"
        align="center"
        gap="md"
        borderRadius="xl"
        bgColor={"bg.panel"}
    >
        <Text fontSize="sm" fontWeight="900" w="6">
            {rank}
        </Text>
        <Image
            src="https://lh3.googleusercontent.com/aida-public/AB6AXuDrhV31xIPyEEpV8Z2HtaH3D-BFSIZtssVM_rsyySeLmC4VVBTkGke6dNVl2fTizEodmC0_xlXbECLefHIe2faK3a6KdevAB-f6JZPnxYg8HaOigYVT-zUp4HB1RVwp4CxPczrSX2Vz-4lxIMPrpYD8-3m1BNxrjWgJiJAFxHJ45zjsnC9VzA1lydtQOr5JpHjwigiCDywbpcL0rsFf7TbDw2rRiK5qvuQgJDB8tdgiAYx-peNLDEEMfOpyUWbnevRMWoFjDVU-nS5C"
            w="14"
            h="14"
            borderRadius="lg"
            fit="cover"
        />
        <Box flex="1">
            <Text fontSize="sm" fontWeight="bold">
                {name}
            </Text>
            <Text
                fontSize="xs"
                color="fg.emphasized"
                fontWeight="bold"
                textTransform="uppercase"
            >
                {stat}
            </Text>
        </Box>
        <Icon as={MoveRightIcon} color="fg" fontSize="md" />
    </Flex>
);

const ConsoleSection = () => {
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
        <Box>
            <Heading
                fontSize="sm"
                fontWeight="800"
                textTransform="uppercase"
                mb="4"
            >
                Browse by Console
            </Heading>
            <HStack gap="4">
                {consoles.map((console) => (
                    <Card.Root
                        key={console.name}
                        bg={`${console.color}/20`}
                        borderRadius="lg"
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
    );
};
const DiscoverSection = () => {
    return (
        <>
            <SimpleGrid columns={{ base: 1, lg: 2 }} gap="6">
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
                    <Card.Body position="absolute" bottom="0" p="10" w="full">
                        <HStack mb="4">
                            <Badge
                                variant="solid"
                                fontSize="xs"
                                colorScheme={"blue"}
                                fontWeight="900"
                                px="2"
                                py="1"
                                borderRadius="lg"
                            >
                                CRITICAL ACCLAIM
                            </Badge>
                            <Text
                                color="white"
                                textShadow={"0px 0px 4px rgba(0, 0, 0, 0.8)"}
                                fontSize="xs"
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
                                    color="white"
                                    textShadow={
                                        "0px 0px 4px rgba(0, 0, 0, 0.8)"
                                    }
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
                    <Card.Body position="absolute" bottom="0" p="10" w="full">
                        <HStack mb="4">
                            <Badge
                                variant="solid"
                                fontSize="xs"
                                fontWeight="900"
                                px="2"
                                py="1"
                                borderRadius="lg"
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
                                color="white.700"
                                fontSize="10px"
                                fontWeight="bold"
                            >
                                DATABASE SCORE
                            </Text>
                            <HStack>
                                <Text
                                    fontSize="4xl"
                                    fontWeight="900"
                                    color="teal"
                                >
                                    9.6
                                </Text>
                                <Icon
                                    as={BadgeCheckIcon}
                                    color="teal"
                                    fontSize="2xl"
                                />
                            </HStack>
                        </VStack>
                    </Card.Body>
                </Card.Root>
            </SimpleGrid>
        </>
    );
};
const TrendingGames = () => {
    return (
        <Box>
            <Flex justify="space-between" align="center" mb="8">
                <Heading
                    fontSize="sm"
                    fontWeight="800"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    <Icon as={DatabaseIcon} mr="xs" /> Trending
                </Heading>
                <Text
                    fontSize="xs"
                    fontWeight="900"
                    color="info.600"
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
                        bg="bg.subtle"
                        borderRadius="xl"
                        overflow="hidden"
                        border="none"
                        _hover={{ transform: "translateY(-2px)" }}
                    >
                        <Box aspectRatio={3 / 4} position="relative">
                            <Image
                                src={`https://lh3.googleusercontent.com/aida-public/AB6AXuDhm_jCOrQxWHh5S-RxoaBJ0GznEVNVFarWlHhfW9vfFwfiyy2mR8nJMD4dgZIvnukpwU3bBzXifclMi7SFuQVRBPfeFBcd7fgAZ3pqLvPWFFTPQwZCkqdFoaODgbjSJ5WVwhW-dzxBzzD5sNKuN6M1IEmB7uti6ObnEm2wRbXcoF1GWvv-WMZ8pjqH4bWCjMiqFvv2kp-kg-qMh10bHmrY7LoXiyTZMmFO2sAcoAcerLAKLJUodvSNTFUy3JS7KPAQfuTXQ_8wsHc_`}
                                w="full"
                                h="full"
                                fit="cover"
                            />
                            <Badge
                                position="absolute"
                                bottom="2"
                                right="2"
                                px="2"
                                py="1"
                                borderRadius="md"
                                fontWeight="bold"
                                // The "Glass" Effect
                                bg="whiteAlpha.600"
                                backdropFilter="blur(10px)"
                                color="black"
                                borderColor="whiteAlpha.400"
                                boxShadow="0 4px 6px rgba(0, 0, 0, 0.1)"
                            >
                                8.4
                            </Badge>
                        </Box>
                        <Card.Body p="4">
                            <Text fontSize="sm" fontWeight="bold" mb="x">
                                Shadow Realm
                            </Text>
                            <Badge bg="bg" fontSize="xs" borderRadius="lg">
                                SOULSLIKE
                            </Badge>
                        </Card.Body>
                    </Card.Root>
                ))}
            </SimpleGrid>
        </Box>
    );
};

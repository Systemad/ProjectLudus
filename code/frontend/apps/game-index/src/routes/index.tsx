import { createFileRoute } from "@tanstack/react-router";
import { Box, Heading, SimpleGrid, Text, VStack } from "ui";
import { AnticipatedSection } from "../components/home/AnticipatedSection";
import { ConsolesSection } from "../components/home/ConsolesSection";
import { ReleasingSection } from "../components/home/ReleasingSection";
import { SpotlightHero } from "../components/home/SpotlightHero";
import { SpotlightRatedSection } from "../components/home/SpotlightRatedSection";

import { TrendingSection } from "../components/home/TrendingSection";
import { games } from "../data/games";
import { SteamPopularitySection } from "@src/components/home/SteamPopularitySection";

export const Route = createFileRoute("/")({ component: GamingHubPage });

function GamingHubPage() {
    return (
        <Box maxW="8xl" mx="auto" w="full">
            <SpotlightHero />
            <Box py={{ base: "14", md: "20" }}>
                <VStack align="stretch" gap={{ base: "14", md: "20" }}>
                    <AnticipatedSection />
                    <TrendingSection />
                    <ConsolesSection />
                    <SpotlightRatedSection />
                    <ReleasingSection />
                    <SteamPopularitySection title="Steam 24h Peak Players" popularityTypeId={5} />
                    <SteamPopularitySection
                        title="Steam Most Wishlisted Upcoming"
                        popularityTypeId={10}
                    />

                    {/* Stats banner */}
                    <Box
                        rounded="3xl"
                        bg="bg.panel"
                        borderWidth="1px"
                        borderColor="border.subtle"
                        p={{ base: "6", md: "8" }}
                    >
                        <SimpleGrid columns={{ base: 1, md: 3 }} gap="6">
                            <VStack align="stretch" gap="3">
                                <Text
                                    color="colorScheme.solid"
                                    textTransform="uppercase"
                                    letterSpacing="widest"
                                    fontSize="xs"
                                    fontWeight="bold"
                                >
                                    Signal stack
                                </Text>
                                <Heading fontFamily="heading" size="lg">
                                    Data-Centric Gaming Hub
                                </Heading>
                            </VStack>
                            {[
                                { label: "Titles tracked", value: `${games.length}+` },
                                { label: "Genres mapped", value: "24" },
                                { label: "Daily updates", value: "6 feeds" },
                            ].map((stat) => (
                                <VStack key={stat.label} align="stretch" gap="1">
                                    <Text
                                        fontSize="xs"
                                        textTransform="uppercase"
                                        letterSpacing="widest"
                                        color="fg.subtle"
                                        fontWeight="bold"
                                    >
                                        {stat.label}
                                    </Text>
                                    <Text
                                        fontFamily="heading"
                                        fontSize={{ base: "3xl", md: "4xl" }}
                                        fontWeight="black"
                                    >
                                        {stat.value}
                                    </Text>
                                </VStack>
                            ))}
                        </SimpleGrid>
                    </Box>
                </VStack>
            </Box>
        </Box>
    );
}

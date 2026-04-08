import {
    Box,
    Button,
    ChevronLeftIcon,
    ChevronRightIcon,
    Flex,
    Grid,
    Heading,
    HStack,
    Text,
    VStack,
} from "ui";
import { Configure, Hits, InstantSearch } from "react-instantsearch";
import { SEARCH_INDEX_NAME, releasingSearchClient } from "@src/Typesense/instantsearch";
import { HomeRailHitCard, homeRailHitClassNames, homeRailHitListCss } from "./HomeRailHitCard";

const CALENDAR_HIGHLIGHTS = new Set([18, 20, 22, 24, 26, 28]);

export function ReleasingSection() {
    const now = new Date();
    const monthStart = Math.floor(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), 1) / 1000);
    const nextMonthStart = Math.floor(
        Date.UTC(now.getUTCFullYear(), now.getUTCMonth() + 1, 1) / 1000,
    );
    const releaseRailParams = {
        sort_by: "aggregated_rating:desc",
        filter_by: `aggregated_rating:!=null && first_release_date:>=${monthStart} && first_release_date:<${nextMonthStart}`,
    } as const;

    return (
        <VStack align="stretch" gap="8">
            <Heading fontFamily="heading" fontSize={{ base: "3xl", md: "4xl" }}>
                Releasing This Month
            </Heading>
            <Grid
                templateColumns={{ base: "1fr", lg: "280px 1fr" }}
                gap={{ base: "8", lg: "10" }}
                alignItems="start"
            >
                {/* Calendar */}
                <Box
                    rounded="2xl"
                    bg="bg.panel"
                    borderWidth="1px"
                    borderColor="border.subtle"
                    p="5"
                >
                    <VStack align="stretch" gap="4">
                        <Flex justify="space-between" align="center">
                            <Text fontFamily="heading" fontWeight="bold">
                                January 2022
                            </Text>
                            <HStack gap="1">
                                <Button
                                    variant="ghost"
                                    size="xs"
                                    rounded="full"
                                    p="1"
                                    h="6"
                                    minW="6"
                                >
                                    <ChevronLeftIcon boxSize="3" />
                                </Button>
                                <Button
                                    variant="ghost"
                                    size="xs"
                                    rounded="full"
                                    p="1"
                                    h="6"
                                    minW="6"
                                >
                                    <ChevronRightIcon boxSize="3" />
                                </Button>
                            </HStack>
                        </Flex>
                        <Grid templateColumns="repeat(7, 1fr)" gap="0.5" textAlign="center">
                            {["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"].map((d) => (
                                <Text
                                    key={d}
                                    fontSize="xs"
                                    color="fg.subtle"
                                    fontWeight="bold"
                                    py="1"
                                >
                                    {d}
                                </Text>
                            ))}
                            {/* January 2022 starts on Saturday — 6 padding cells */}
                            {Array.from({ length: 6 }).map((_, i) => (
                                <Box key={`pad-${i}`} />
                            ))}
                            {Array.from({ length: 31 }).map((_, i) => {
                                const day = i + 1;
                                const highlighted = CALENDAR_HIGHLIGHTS.has(day);
                                return (
                                    <Flex
                                        key={day}
                                        align="center"
                                        justify="center"
                                        h="7"
                                        rounded="full"
                                        bg={highlighted ? "colorScheme.solid" : "transparent"}
                                        color={highlighted ? "fg.contrast" : "fg.base"}
                                        fontSize="xs"
                                        fontWeight={highlighted ? "bold" : "normal"}
                                        cursor={highlighted ? "pointer" : "default"}
                                    >
                                        {day}
                                    </Flex>
                                );
                            })}
                        </Grid>
                    </VStack>
                </Box>

                <InstantSearch
                    searchClient={releasingSearchClient}
                    indexName={SEARCH_INDEX_NAME}
                    future={{ preserveSharedStateOnUnmount: true }}
                >
                    <Configure query="*" hitsPerPage={10} {...(releaseRailParams as object)} />

                    <Box css={homeRailHitListCss}>
                        <Hits hitComponent={HomeRailHitCard} classNames={homeRailHitClassNames} />
                    </Box>
                </InstantSearch>
            </Grid>
        </VStack>
    );
}

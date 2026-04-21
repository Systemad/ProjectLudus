import {
    Box,
    For,
    Grid,
    GridItem,
    Heading,
    HStack,
    Loading,
    Skeleton,
    SkeletonText,
    Text,
    VStack,
} from "ui";

function MonthCardSkeleton() {
    return (
        <Box
            borderWidth="1px"
            borderColor="border.subtle"
            rounded="xl"
            p={{ base: "3", md: "4" }}
            bg="bg.panel"
        >
            <Skeleton>
                <Heading fontSize="lg" fontWeight="semibold" mb="2">
                    September
                </Heading>
            </Skeleton>

            <VStack gap="2" align="stretch">
                <For each={[0, 1, 2]}>
                    {(index) => (
                        <HStack
                            key={index}
                            justify="space-between"
                            align={{ base: "stretch", md: "start" }}
                            direction={{ base: "column", md: "row" }}
                            px="2"
                            py={{ base: "2", md: "3" }}
                            gap={{ base: "2", md: "3" }}
                            rounded="md"
                            bg="bg.surface"
                            borderWidth="1px"
                            borderColor="border.subtle"
                        >
                            <HStack gap="3" minW="0" flex="1" w="full">
                                <Skeleton>
                                    <Box
                                        w={{ base: "9", md: "10" }}
                                        h={{ base: "11", md: "12" }}
                                        rounded="md"
                                    />
                                </Skeleton>

                                <VStack align="start" gap="1" minW="0" flex="1">
                                    <SkeletonText lineClamp={1} w="full" />
                                    <Skeleton>
                                        <Text fontSize="xs">Upcoming</Text>
                                    </Skeleton>
                                </VStack>
                            </HStack>

                            <Skeleton>
                                <Text fontSize="xs">Sep 14-Sep 18</Text>
                            </Skeleton>
                        </HStack>
                    )}
                </For>
            </VStack>
        </Box>
    );
}

export function EventsPageLoadingState() {
    return (
        <VStack align="stretch" gap="6">
            <VStack align="stretch" gap="3">
                <HStack justify="space-between" align="baseline" wrap="wrap" gap="3">
                    <Skeleton>
                        <Heading fontSize="2xl" fontWeight="bold">
                            2026
                        </Heading>
                    </Skeleton>
                    <Skeleton>
                        <Text fontSize="sm">12 events</Text>
                    </Skeleton>
                </HStack>

                <HStack gap="2" maxW={{ base: "full", md: "sm" }}>
                    <For each={[0, 1, 2]}>
                        {(index) => (
                            <Skeleton key={index} flex="1">
                                <Box h="8" rounded="full" />
                            </Skeleton>
                        )}
                    </For>
                </HStack>

                <HStack gap="2" align="center" color="fg.subtle">
                    <Loading.Dots color="fg.subtle" fontSize="lg" />
                    <Text fontSize="sm">Loading events...</Text>
                </HStack>
            </VStack>

            <Grid
                templateColumns={{ base: "1fr", md: "repeat(2, 1fr)", xl: "repeat(4, 1fr)" }}
                gap="4"
            >
                <For each={[0, 1, 2, 3, 4, 5, 6, 7]}>
                    {(index) => (
                        <GridItem key={index}>
                            <MonthCardSkeleton />
                        </GridItem>
                    )}
                </For>
            </Grid>
        </VStack>
    );
}

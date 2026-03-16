"use client";
import type { GameItem, SearchQueryResponse } from "../../gen";
import {
    SkeletonCircle,
    VStack,
    SkeletonText,
    Skeleton,
    For,
    Grid,
    EmptyState,
    BoxIcon,
    GridItem,
    Card,
    Image,
    Stack,
    Text,
    Button,
    Box,
    Heading,
    Flex,
} from "@packages/ui";
import { getIGDBImageUrl } from "../../utils/ImageHelper";
import { useExtractColors } from "react-extract-colors";
import { useInView } from "react-intersection-observer";
import { useEffect, useRef } from "react";
import type { UseInfiniteQueryResult } from "@tanstack/react-query";

export interface SearchResultsProps {
    loading: boolean;
    fetching: boolean;
    hasNextPage: boolean | undefined;
    isFetchingNextPage: boolean;
    fetchNextPage: UseInfiniteQueryResult["fetchNextPage"];
    results: SearchQueryResponse | null | undefined;
}

export function SearchResults({
    loading,
    fetching,
    hasNextPage,
    isFetchingNextPage,
    fetchNextPage,
    results,
}: SearchResultsProps) {
    const { ref, inView } = useInView();
    const hasTriggeredInViewRef = useRef(false);

    useEffect(() => {
        if (!inView) {
            hasTriggeredInViewRef.current = false;
            return;
        }

        if (
            !loading &&
            !fetching &&
            !isFetchingNextPage &&
            hasNextPage &&
            !hasTriggeredInViewRef.current
        ) {
            hasTriggeredInViewRef.current = true;
            void fetchNextPage();
        }
    }, [
        fetchNextPage,
        fetching,
        hasNextPage,
        inView,
        isFetchingNextPage,
        loading,
    ]);

    if (loading && !results) {
        return (
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
                    {(_, index) => (
                        <VStack key={index}>
                            <SkeletonCircle variant={"pulse"} />
                            <SkeletonText variant={"pulse"} lineClamp={2} />
                            <Skeleton variant={"pulse"} h="4xs" />
                        </VStack>
                    )}
                </For>
            </Grid>
        );
    }

    return (
        <Grid
            gap="lg"
            templateColumns={{
                base: "repeat(2, 1fr)",
                sm: "repeat(auto-fill, minmax(160px, 1fr))",
                md: "repeat(auto-fill, minmax(200px, 1fr))",
                xl: "repeat(auto-fill, minmax(220px, 1fr))",
            }}
        >
            <For
                each={results?.data}
                fallback={
                    <EmptyState.Root
                        description="There are no items to show"
                        indicator={<BoxIcon />}
                    />
                }
            >
                {(pagedItem) => {
                    const game = pagedItem.item;
                    if (!game) return null;

                    return (
                        <GridItem key={game.id}>
                            <ExplorerCard item={game} />
                        </GridItem>
                    );
                }}
            </For>

            {/* Infinite scroll trigger */}
            {hasNextPage && (
                <GridItem
                    ref={ref}
                    display="flex"
                    justifyContent="center"
                    py="md"
                >
                    <Text fontSize="xs" color="fg.muted">
                        Loading more...
                    </Text>
                </GridItem>
            )}
        </Grid>
    );
}

type CardProps = {
    item: GameItem | null;
};
const ExplorerCard = ({ item }: CardProps) => {
    const imageUrl = getIGDBImageUrl(item?.coverUrl, "1080p", false);
    const { dominantColor, lighterColor } = useExtractColors(imageUrl, {
        format: "rgba",
        maxColors: 3,
        sortBy: "vibrance",
    });

    return (
        <Card.Root
            style={{
                background: `
            radial-gradient(circle at 50% 20%, ${dominantColor} 0%, ${dominantColor} 35%, transparent 65%),
            radial-gradient(circle at 80% 80%, ${dominantColor} 0%, transparent 70%),
            linear-gradient(180deg, ${dominantColor}, rgba(12,12,16,0.95))
        `,
            }}
            border={"none"}
            rounded="2xl"
            overflow="hidden"
            transition="all .25s"
            _hover={{
                transform: "translateY(-4px)",
                boxShadow: `0 18px 40px ${lighterColor}`,
            }}
        >
            <Box aspectRatio="3/4" overflow="hidden">
                <Image
                    src={imageUrl}
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
                        {item?.name}
                    </Heading>

                    <Text fontSize="xs" color="fg.muted">
                        {item?.releaseYear}
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

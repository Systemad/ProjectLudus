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
import { getCachedCardStyles } from "../../utils/CardStyleCache";
import { useInView } from "react-intersection-observer";
import { useEffect, useRef } from "react";
import type { UseInfiniteQueryResult } from "@tanstack/react-query";

export interface SearchResultsProps {
    loading: boolean;
    hasNextPage?: boolean;
    isFetchingNextPage?: boolean;
    fetchNextPage?: UseInfiniteQueryResult["fetchNextPage"];
    results: SearchQueryResponse | null | undefined;
}

export function SearchResults({
    loading,
    hasNextPage = false,
    isFetchingNextPage = false,
    fetchNextPage,
    results,
}: SearchResultsProps) {
    const { ref, inView } = useInView({ rootMargin: "200px" });
    const hasTriggeredInViewRef = useRef(false);

    useEffect(() => {
        if (!inView) {
            hasTriggeredInViewRef.current = false;
            return;
        }

        if (
            hasNextPage &&
            fetchNextPage &&
            !isFetchingNextPage &&
            !hasTriggeredInViewRef.current
        ) {
            hasTriggeredInViewRef.current = true;
            fetchNextPage();
        }
    }, [inView, hasNextPage, isFetchingNextPage, fetchNextPage]);

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
    const { gradient, shadow } = getCachedCardStyles(imageUrl);

    return (
        <Card.Root
            style={{
                background: gradient,
            }}
            border={"none"}
            rounded="2xl"
            overflow="hidden"
            transition="all .25s"
            _hover={{
                transform: "translateY(-4px)",
                boxShadow: shadow,
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

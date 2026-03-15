import type { TypeOf } from "zod/v4";
import type {
    GameItem,
    searchQueryParamsSchema,
    SearchQueryResponse,
} from "../../gen";
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
import { useEffect } from "react";

export interface SearchResultsProps {
    query: TypeOf<typeof searchQueryParamsSchema>;
    setField: <K extends keyof TypeOf<typeof searchQueryParamsSchema>>(
        field: K,
        value: TypeOf<typeof searchQueryParamsSchema>[K]
    ) => void;
    loading: boolean;
    results: SearchQueryResponse | null | undefined;
}

export function SearchResults({
    query,
    setField,
    loading,
    results,
}: SearchResultsProps) {
    const { ref, inView } = useInView();

    useEffect(() => {
        if (inView && results?.pageMetadata?.nextPageCursor) {
            setField("AfterCursor", results.pageMetadata.nextPageCursor);
        }
    }, [inView, results?.pageMetadata?.nextPageCursor, setField]);

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
                {(pagedItem, index) => {
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
            {results?.pageMetadata?.nextPageCursor && (
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

const withAlpha = (rgb: string | null, alpha: number) => {
    if (!rgb) return `rgba(0,0,0,0)`;
    return rgb.replace("rgb(", "rgba(").replace(")", `, ${alpha})`);
};

type CardProps = {
    item: GameItem | null;
};
const ExplorerCard = ({ item }: CardProps) => {
    const imageUrl = getIGDBImageUrl(item?.coverUrl, "1080p", false);
    const { dominantColor, darkerColor, lighterColor } = useExtractColors(
        imageUrl,
        {
            format: "rgba",
            maxColors: 3,
            orderBy: "vibrance",
        }
    );

    const dom66 = withAlpha(dominantColor, 0.6);
    const dom33 = withAlpha(dominantColor, 0.3);
    const dark44 = withAlpha(darkerColor, 0.4);
    const dark55 = withAlpha(darkerColor, 0.55);

    return (
        <Card.Root
            style={{
                background: `
            radial-gradient(circle at 50% 20%, ${withAlpha(
                lighterColor,
                0.95
            )} 0%, ${dominantColor} 35%, transparent 65%),
            radial-gradient(circle at 80% 80%, ${withAlpha(
                dominantColor,
                0.4
            )} 0%, transparent 70%),
            linear-gradient(180deg, ${dominantColor}, rgba(12,12,16,0.95))
        `,
            }}
            border={"none"}
            rounded="2xl"
            overflow="hidden"
            transition="all .25s"
            _hover={{
                transform: "translateY(-4px)",
                boxShadow: `0 18px 40px ${dark55}`,
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

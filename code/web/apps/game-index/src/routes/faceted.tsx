"use client";
import { Suspense, useEffect } from "react";
import { createFileRoute, stripSearchParams } from "@tanstack/react-router";
import {
    HStack,
    Box,
    Button,
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
    Grid,
    GridItem,
    Flex,
    Checkbox,
    BoxIcon,
    EmptyState,
} from "@packages/ui";
import {
    searchQueryParamsSchema,
    useSearchSuspenseInfiniteHook,
    type Bucket,
    type Facets,
    type GameItem,
    type SearchQueryParams,
    type SearchQueryResponse,
} from "../gen";

import { useInView } from "react-intersection-observer";
import React from "react";
import { getIGDBImageUrl } from "../utils/ImageHelper";
import type {
    InfiniteData,
    InfiniteQueryObserverResult,
    UseSuspenseInfiniteQueryOptions,
    UseSuspenseInfiniteQueryResult,
} from "@tanstack/react-query";
import z from "zod/v4";

/*
export const searchQueryParamsSchema = z
    .object({
        Name: z.optional(z.string()),
        Genres: z.optional(z.array(z.string())),
        Themes: z.optional(z.array(z.string())),
        Modes: z.optional(z.array(z.string())),
        AfterCursor: z.optional(z.string()),
        PageSize: z.optional(z.coerce.number().int()),
    })
    .optional();

    */

export const searchQueryParamsSchema2 = z.object({
    Name: z.string().optional(),
    Genres: z.array(z.string()).optional(),
    Themes: z.array(z.string()).optional(),
    Modes: z.array(z.string()).optional(),
    AfterCursor: z.string().optional(),
    PageSize: z.number().int().catch(20).optional(),
});

type SearchParams = z.infer<typeof searchQueryParamsSchema2>;

const defaultValues: Partial<SearchParams> = {
    Name: undefined,
    Genres: [],
    Themes: [],
    Modes: [],
    AfterCursor: undefined,
    PageSize: undefined,
};

export const Route = createFileRoute("/faceted")({
    component: Index,
    validateSearch: searchQueryParamsSchema2,
    search: {
        middlewares: [
            stripSearchParams<SearchParams>({
                Name: "",
                Genres: [],
                Themes: [],
                Modes: [],
                AfterCursor: "",
            }),
        ],
    },
});

function Index() {
    const { open, onOpen, onClose } = useDisclosure();

    const searchParams = Route.useSearch() ?? {};
    const navigate = Route.useNavigate();

    // Standard TanStack update pattern
    const onUpdate = (next: SearchQueryParams) =>
        navigate({ search: next, replace: true });

    const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isFetching } =
        useSearchSuspenseInfiniteHook({ params: searchParams });
    const facets = data?.pages[0]?.itemFacets?.facets ?? [];
    return (
        <Flex
            direction="column"
            bg="bg.surface"
            minH="100vh"
            px={{ base: "md", xl: "lg" }}
            py="xl"
            gap="xl"
        >
            {/* MOBILE FILTER BUTTON */}
            <Flex
                display={{ base: "flex", md: "none" }}
                justify="space-between"
            >
                <Button
                    size="sm"
                    variant="outline"
                    rounded="xl"
                    onClick={onOpen}
                >
                    Filters
                </Button>

                <Text color="fg.muted" fontSize="sm">
                    3,842 entries
                </Text>
            </Flex>

            <Flex align="flex-start" gap="xl">
                {/* SIDEBAR */}
                <Box
                    display={{ base: "none", md: "block" }}
                    w="280px"
                    flexShrink={0}
                    position="sticky"
                    top="xl"
                >
                    <Stack
                        bg="bg.panel"
                        rounded="2xl"
                        p="lg"
                        borderWidth="1px"
                        borderColor="border.subtle"
                        gap="lg"
                    >
                        <Flex justify="space-between" align="center">
                            <Heading size="sm">Refine</Heading>

                            <Button variant="ghost" size="xs">
                                Reset
                            </Button>
                        </Flex>
                        <FilterSidebar
                            facets={facets}
                            searchParams={searchParams}
                            onUpdate={onUpdate}
                        />
                    </Stack>
                </Box>

                {/* CONTENT */}
                <Stack flex="1" gap="xl">
                    {/* ACTIVE FILTERS */}
                    <HStack flexWrap="wrap" gap="sm">
                        <Text fontSize="xs" color="fg.muted">
                            Filters:
                        </Text>

                        <Button size="xs" variant="subtle" rounded="full">
                            Action
                        </Button>

                        <Button size="xs" variant="subtle" rounded="full">
                            PC
                        </Button>

                        <Button size="xs" variant="subtle" rounded="full">
                            2023
                        </Button>

                        <Button size="xs" variant="ghost">
                            Clear
                        </Button>
                    </HStack>

                    {/* GRID */}
                    <ItemScrolls
                        data={data}
                        fetchNextPage={fetchNextPage}
                        hasNextPage={hasNextPage}
                        isFetchingNextPage={isFetchingNextPage}
                        isFetching={isFetching}
                    />

                    {/* PAGINATION */}
                    <Flex justify="space-between" align="center">
                        <Button>Load more</Button>
                    </Flex>
                </Stack>
            </Flex>

            {/* MOBILE FILTERS */}
            <Drawer.Root
                placement="block-end"
                open={open}
                onClose={onClose}
                size="xl"
                closeOnDrag
                withDragBar
            >
                <Drawer.Content>
                    <Drawer.Header>Filters</Drawer.Header>

                    <Drawer.Body>
                        <FilterFields onApply={onClose} />
                    </Drawer.Body>
                </Drawer.Content>
            </Drawer.Root>
        </Flex>
    );
}

interface ItemScrollsProps {
    data: InfiniteData<SearchQueryResponse> | undefined;
    fetchNextPage: () => Promise<
        InfiniteQueryObserverResult<InfiniteData<SearchQueryResponse>, Error>
    >;
    hasNextPage: boolean | undefined;
    isFetchingNextPage: boolean;
    isFetching: boolean;
}

export const ItemScrolls = ({
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    isFetching,
}: ItemScrollsProps) => {
    const { ref, inView } = useInView();

    useEffect(() => {
        if (inView && hasNextPage && !isFetchingNextPage) {
            fetchNextPage();
        }
    }, [inView, hasNextPage, isFetchingNextPage]);

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
            <Suspense>
                {/* 1. Loop through pages */}
                <For
                    each={data?.pages}
                    fallback={
                        <EmptyState.Root
                            description="There are no items to show"
                            indicator={<BoxIcon />}
                        />
                    }
                >
                    {(axiosResponse, pageIndex) => {
                        // axiosResponse.data is your PaginatedResponseOfGameItem JSON
                        const data = axiosResponse.data;
                        const pageInfo = axiosResponse.pageInfo;

                        return (
                            <React.Fragment
                                key={
                                    pageInfo?.nextPageCursor ??
                                    `page-${pageIndex}`
                                }
                            >
                                <For each={data}>
                                    {(project) => (
                                        <GridItem key={project.item?.id}>
                                            <ExplorerCard item={project.item} />
                                        </GridItem>
                                    )}
                                </For>
                            </React.Fragment>
                        );
                    }}
                </For>
                {/* 3. The Sentinel / Load More Trigger */}
                <GridItem display="flex" justifyContent="center" py="md">
                    <button
                        ref={ref}
                        onClick={() => fetchNextPage()}
                        disabled={!hasNextPage || isFetchingNextPage}
                        style={{
                            padding: "10px",
                            cursor: hasNextPage ? "pointer" : "default",
                        }}
                    >
                        {isFetchingNextPage
                            ? "Loading more..."
                            : hasNextPage
                            ? "Load More"
                            : "Nothing more to load"}
                    </button>
                </GridItem>
                {/* Background Sync Indicator */}
                {isFetching && !isFetchingNextPage && (
                    <GridItem textAlign="center" fontSize="xs" color="fg.muted">
                        Background Updating...
                    </GridItem>
                )}
            </Suspense>
        </Grid>
    );
};

/*

*/
/* -------------------------------------------------- */
/* CARD */
/* -------------------------------------------------- */

type CardProps = {
    item: GameItem | null;
};
const ExplorerCard = ({ item }: CardProps) => {
    const imageUrl = getIGDBImageUrl(item?.coverUrl, "1080p", false);

    return (
        <Card.Root
            bg="bg.panel"
            borderWidth="1px"
            borderColor="border.subtle"
            rounded="2xl"
            overflow="hidden"
            transition="all .2s"
            _hover={{
                transform: "translateY(-4px)",
                bg: "bg.muted",
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
                        Cyberpunk • 2023
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

/* -------------------------------------------------- */
/* FILTERS */
/* -------------------------------------------------- */

interface FilterFieldsProps {
    onApply?: () => void;
}

const FilterFields = ({ onApply }: FilterFieldsProps) => (
    <Stack gap="lg" w="full" h="full">
        <Accordion.Root toggle multiple>
            <Accordion.Item button="Genre" index={0}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>Action</CheckboxGroup.Item>
                        <CheckboxGroup.Item>RPG</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Strategy</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Simulation</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Platform" index={1}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>PC</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Console</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Release Year" index={2}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>2024</CheckboxGroup.Item>
                        <CheckboxGroup.Item>2023</CheckboxGroup.Item>
                        <CheckboxGroup.Item>2022</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>
        </Accordion.Root>

        {onApply && (
            <Box pt="4" pb="calc(env(safe-area-inset-bottom) + 16px)">
                <Button w="full" size="lg" rounded="2xl" onClick={onApply}>
                    Show Results
                </Button>
            </Box>
        )}
    </Stack>
);

// FIX LOGiC?
// TODO: ON BACKEND: FILTER ALWAYS ALPHABETICAL!!
// INSTEAD OF DYNAIMCALLY GENERATING, DO MANUALLY, AND HAVE MORE CONTROL, AND GRAY OUT, IF A BUCKET HAS ZERO DOC_COUNT
type FacetKey = "Themes" | "Modes" | "Genres";

type BucketWithKey = Bucket & { key: string };

function hasKey(bucket: Bucket): bucket is BucketWithKey {
    return typeof bucket.key === "string";
}
const facetKeys: FacetKey[] = ["Themes", "Modes", "Genres"];

const FilterSidebar = ({
    searchParams,
    facets,
    onUpdate,
}: {
    searchParams: SearchQueryParams;
    facets: Facets[];
    onUpdate: (s: SearchQueryParams) => void;
}) => {
    const toggle = (key: FacetKey, val: string) => {
        const current = searchParams[key] ?? [];

        const next = current.includes(val)
            ? current.filter((v) => v !== val)
            : [...current, val];

        onUpdate({
            ...searchParams,
            [key]: next,
        });
    };

    return (
        <Accordion.Root toggle multiple>
            {facets.map((facet, i) => {
                const key = facetKeys[i];
                if (!key) return null;

                const buckets = facet.buckets?.filter(hasKey) ?? [];

                return (
                    <Accordion.Item key={key} button={key} index={i}>
                        <Accordion.Panel>
                            {buckets.map((bucket) => {
                                const selected = (
                                    searchParams[key] ?? []
                                ).includes(bucket.key);

                                return (
                                    <Checkbox
                                        key={bucket.key}
                                        checked={selected}
                                        onChange={() => toggle(key, bucket.key)}
                                    >
                                        {bucket.key} ({bucket.doc_count ?? 0})
                                    </Checkbox>
                                );
                            })}
                        </Accordion.Panel>
                    </Accordion.Item>
                );
            })}
        </Accordion.Root>
    );
};

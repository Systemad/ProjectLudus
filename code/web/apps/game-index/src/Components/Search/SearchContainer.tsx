"use client";

import { useStore } from "@tanstack/react-store";
import {
    getSearchStore,
    searchStoreManager,
    type SearchState,
} from "./SearchStore";
import type { TypeOf } from "zod";
import { useEffect, useMemo, useRef } from "react";
import { useDebouncedState, useDebouncedValue } from "@mantine/hooks";
import {
    searchQueryParamsSchema,
    useGetApiTagsAllTagsHook,
    useSearchHook,
    type AggregationBuckets,
} from "../../gen";
import { keepPreviousData } from "@tanstack/react-query";
import { SearchResults } from "./SearchResults";
import {
    Flex,
    Box,
    Input,
    Button,
    Drawer,
    Accordion,
    Checkbox,
    Text,
    Stack,
    Heading,
    Select,
    useDisclosure,
} from "@packages/ui"; // Adjust imports as needed

export interface SearchContainerProps {
    contextId?: string;
    initialSearch?: Partial<TypeOf<typeof searchQueryParamsSchema>>;
    fixedParams?: Partial<TypeOf<typeof searchQueryParamsSchema>>;
    onSearchChange?: (search: TypeOf<typeof searchQueryParamsSchema>) => void;
    title?: string;
}

const mergeSearchStates = (
    ...states: Partial<SearchState>[]
): Partial<SearchState> => {
    const result: Partial<SearchState> = {};
    for (const state of states) {
        for (const key in state) {
            const k = key as keyof SearchState;
            const resultValue = result[k];
            const stateValue = state[k];
            if (Array.isArray(resultValue) && Array.isArray(stateValue)) {
                (result as Record<keyof SearchState, unknown>)[k] = [
                    ...new Set([...resultValue, ...stateValue]),
                ];
            } else if (stateValue !== undefined) {
                (result as Record<keyof SearchState, unknown>)[k] = stateValue;
            }
        }
    }
    return result;
};

interface FilterPanelProps {
    query: TypeOf<typeof searchQueryParamsSchema>;
    setField: <K extends keyof TypeOf<typeof searchQueryParamsSchema>>(
        field: K,
        value: TypeOf<typeof searchQueryParamsSchema>[K]
    ) => void;
    tagCounts: Record<string, number>;
    allTags: any; // Adjust type as needed
    tagTypes: any[]; // Adjust type as needed
    onReset: () => void;
}

const FilterPanel = ({
    query,
    setField,
    tagCounts,
    allTags,
    tagTypes,
    onReset,
}: FilterPanelProps) => (
    <Stack gap="lg" w="full" h="full">
        <Flex justify="space-between" align="center">
            <Heading size="sm">Refine</Heading>
            <Button variant="ghost" size="xs" onClick={onReset}>
                Reset
            </Button>
        </Flex>

        <Accordion.Root toggle multiple>
            <Accordion.Item button="Genres" index={0}>
                <Accordion.Panel>
                    <Stack>
                        {tagTypes.find((t) => t.name === "genres") &&
                            Object.entries(tagCounts)
                                .filter(
                                    ([key]) =>
                                        allTags?.tags?.find(
                                            (t: { id: string }) => t.id === key
                                        )?.groupName === "genres"
                                )
                                .map(([key, count]) => (
                                    <Checkbox
                                        key={key}
                                        checked={(query.Genres || []).includes(
                                            key
                                        )}
                                        onChange={() => {
                                            const current = query.Genres || [];
                                            setField(
                                                "Genres",
                                                current.includes(key)
                                                    ? current.filter(
                                                          (g) => g !== key
                                                      )
                                                    : [...current, key]
                                            );
                                        }}
                                    >
                                        {
                                            allTags?.tags?.find(
                                                (t: { id: string }) =>
                                                    t.id === key
                                            )?.name
                                        }{" "}
                                        ({count})
                                    </Checkbox>
                                ))}
                    </Stack>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Themes" index={1}>
                <Accordion.Panel>
                    <Stack>
                        {tagTypes.find((t) => t.name === "themes") &&
                            Object.entries(tagCounts)
                                .filter(
                                    ([key]) =>
                                        allTags?.tags?.find(
                                            (t: { id: string }) => t.id === key
                                        )?.groupName === "themes"
                                )
                                .map(([key, count]) => (
                                    <Checkbox
                                        key={key}
                                        checked={(query.Themes || []).includes(
                                            key
                                        )}
                                        onChange={() => {
                                            const current = query.Themes || [];
                                            setField(
                                                "Themes",
                                                current.includes(key)
                                                    ? current.filter(
                                                          (t) => t !== key
                                                      )
                                                    : [...current, key]
                                            );
                                        }}
                                    >
                                        {
                                            allTags?.tags?.find(
                                                (t: { id: string }) =>
                                                    t.id === key
                                            )?.name
                                        }{" "}
                                        ({count})
                                    </Checkbox>
                                ))}
                    </Stack>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Game Modes" index={2}>
                <Accordion.Panel>
                    <Stack>
                        {tagTypes.find((t) => t.name === "modes") &&
                            Object.entries(tagCounts)
                                .filter(
                                    ([key]) =>
                                        allTags?.tags?.find(
                                            (t: { id: string }) => t.id === key
                                        )?.groupName === "modes"
                                )
                                .map(([key, count]) => (
                                    <Checkbox
                                        key={key}
                                        checked={(query.Modes || []).includes(
                                            key
                                        )}
                                        onChange={() => {
                                            const current = query.Modes || [];
                                            setField(
                                                "Modes",
                                                current.includes(key)
                                                    ? current.filter(
                                                          (m) => m !== key
                                                      )
                                                    : [...current, key]
                                            );
                                        }}
                                    >
                                        {
                                            allTags?.tags?.find(
                                                (t: { id: string }) =>
                                                    t.id === key
                                            )?.name
                                        }{" "}
                                        ({count})
                                    </Checkbox>
                                ))}
                    </Stack>
                </Accordion.Panel>
            </Accordion.Item>
        </Accordion.Root>
    </Stack>
);

// Extract SearchFilters component
const SearchFilters = ({
    query,
    setField,
    tagCounts,
    allTags,
    tagTypes,
    onReset,
}: FilterPanelProps) => (
    <aside>
        <Box
            bg="bg.panel"
            rounded="2xl"
            p="lg"
            borderWidth="1px"
            borderColor="border.subtle"
            w="280px"
            flexShrink={0}
        >
            <Flex align="center" justify="space-between" mb="lg">
                <Text fontWeight="bold">Filters</Text>
                <Box
                    bg="accent"
                    color="white"
                    px="2"
                    py="1"
                    rounded="lg"
                    fontSize="xs"
                >
                    {Object.values(tagCounts).reduce((sum, n) => sum + n, 0)}
                </Box>
            </Flex>

            <FilterPanel
                query={query}
                setField={setField}
                tagCounts={tagCounts}
                allTags={allTags}
                tagTypes={tagTypes}
                onReset={onReset}
            />
        </Box>
    </aside>
);

// Extract SearchHeader component
const SearchHeader = ({
    query,
}: {
    query: TypeOf<typeof searchQueryParamsSchema>;
}) => (
    <Flex gap="md" flexWrap="wrap" align="center" justify="space-between">
        <Box minW="200px">
            <Input
                placeholder="Search games..."
                value={query.Name || ""}
                size="lg"
                rounded="xl"
            />
        </Box>

        <Flex gap="sm" align="center" flexShrink={0}>
            <Select.Root value="popularity" size="lg" rounded="xl" minW="180px">
                <Select.Option value="popularity">Popularity</Select.Option>
                <Select.Option value="newest">Newest</Select.Option>
                <Select.Option value="upcoming">Upcoming</Select.Option>
            </Select.Root>

            <Button variant="outline" size="lg" rounded="xl">
                ☐
            </Button>
            <Button variant="outline" size="lg" rounded="xl">
                ☰
            </Button>
        </Flex>
    </Flex>
);

export function SearchContainer({
    contextId,
    initialSearch = {},
    fixedParams = {},
    onSearchChange,
    title = "Search",
}: SearchContainerProps) {
    const store = getSearchStore(
        contextId,
        mergeSearchStates(initialSearch, fixedParams)
    );

    const query = useStore(store, (state) => state) as TypeOf<
        typeof searchQueryParamsSchema
    >;

    const setField: <K extends keyof TypeOf<typeof searchQueryParamsSchema>>(
        field: K,
        value: TypeOf<typeof searchQueryParamsSchema>[K]
    ) => void = (field, value) => {
        store.setState((prev) => {
            if (field !== "AfterCursor") {
                return {
                    ...prev,
                    [field]: value,
                    AfterCursor: undefined,
                };
            }
            return {
                ...prev,
                [field]: value,
            };
        });
    };

    const prevSearchRef = useRef<
        TypeOf<typeof searchQueryParamsSchema> | undefined
    >(undefined);
    // Track if component has been initialized to avoid overriding user changes
    const isInitializedRef = useRef(false);

    const blockOnSearchChangeRef = useRef(true);

    useEffect(() => {
        if (!isInitializedRef.current) {
            isInitializedRef.current = true;

            setTimeout(() => {
                blockOnSearchChangeRef.current = false;
            }, 0);
        }
    }, []);

    // SHOLD NOT BE NEEDED WITH REACT COMPILER
    const mergedQuery = useMemo(() => {
        const merged = mergeSearchStates(query, fixedParams);
        return {
            ...merged,
            PageSize: merged.PageSize ?? 40,
        } as TypeOf<typeof searchQueryParamsSchema>;
    }, [query, fixedParams]);
    const [debouncedQuery, setDebouncedQuery] = useDebouncedValue(
        mergedQuery,
        300
    );

    const { data: results, isLoading } = useSearchHook(
        { params: debouncedQuery },
        {
            query: {
                queryKey: ["search", contextId, JSON.stringify(debouncedQuery)],
                placeholderData: keepPreviousData,
            },
        }
    );
    const { data: allTags } = useGetApiTagsAllTagsHook({
        query: {
            // queryKey: ["all-tags"],
            placeholderData: keepPreviousData,
            refetchInterval: 1000 * 60 * 5,
        },
    });

    const tagTypesDictionary: { [key: string]: string } = {
        genres: "genres",
        modes: "modes",
        themes: "themes",
    };

    const tagTypes = Array.from(
        new Map(
            allTags?.tags
                ?.filter((tag) => tag.groupName)
                .map((tag) => [
                    tag.groupName,
                    {
                        name: tag.groupName,
                        label:
                            tagTypesDictionary[tag.groupName] ?? tag.groupName,
                    },
                ])
        ).values()
    );

    const parseAggregationBuckets = (
        aggregation: AggregationBuckets,
        transformKey?: (key: string) => string
    ): Record<string, number> => {
        const counts: Record<string, number> = {};
        for (const bucket of aggregation?.buckets ?? []) {
            const key = bucket.key;
            const count = bucket.doc_count;

            if (!key || count === undefined) continue;

            const normalizedKey = transformKey ? transformKey(key) : key;
            counts[normalizedKey] = count;
        }
        return counts;
    };

    const tagCounts: Record<string, number> = parseAggregationBuckets(
        results?.aggregationBuckets as AggregationBuckets,
        (key) => allTags?.tags?.find((t) => t.name === key)?.id ?? key
    );

    useEffect(() => {
        if (
            !onSearchChange ||
            !isInitializedRef.current ||
            blockOnSearchChangeRef.current
        )
            return;

        const currentSearch = { ...query, ...fixedParams };
        const prevSearch = prevSearchRef.current;

        // Only call onSearchChange if the search has actually changed
        // might not be needed?
        if (
            !prevSearch ||
            JSON.stringify(currentSearch) !== JSON.stringify(prevSearch)
        ) {
            prevSearchRef.current = currentSearch;
            onSearchChange(currentSearch);
        }
    }, [query, fixedParams, onSearchChange]);

    useEffect(() => {
        return () => {
            if (contextId) {
                searchStoreManager.removeStore(contextId);
            }
        };
    }, [contextId]);

    const { open, onOpen, onClose } = useDisclosure(); // For drawer

    const handleReset = () => {
        setField("Genres", []);
        setField("Themes", []);
        setField("Modes", []);
    };

    return (
        <Flex direction="column" gap="4" minH="screen" w="full">
            <Flex
                as="main"
                direction="row"
                flexWrap="nowrap"
                align="start"
                justify="between"
                gap="4"
            >
                <SearchFilters
                    query={query}
                    setField={setField}
                    tagCounts={tagCounts}
                    allTags={allTags}
                    tagTypes={tagTypes}
                    onReset={handleReset}
                />

                <Flex
                    direction="column"
                    gap="4"
                    w="full"
                    justify="start"
                    align="start"
                    position="relative"
                >
                    <SearchHeader query={query} />

                    <SearchResults
                        query={query as TypeOf<typeof searchQueryParamsSchema>}
                        setField={setField}
                        loading={isLoading}
                        results={results}
                    />
                </Flex>
            </Flex>

            {/* Mobile filter button */}
            <Flex display={{ base: "flex", md: "none" }} justify="center">
                <Button
                    size="sm"
                    variant="outline"
                    rounded="xl"
                    onClick={onOpen}
                >
                    Filters
                </Button>
            </Flex>

            {/* Mobile drawer (filters) */}
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
                        <FilterPanel
                            query={query}
                            setField={setField}
                            tagCounts={tagCounts}
                            allTags={allTags}
                            tagTypes={tagTypes}
                            onReset={handleReset}
                        />

                        <Box
                            pt="4"
                            pb="calc(env(safe-area-inset-bottom) + 16px)"
                        >
                            <Button
                                w="full"
                                size="lg"
                                rounded="2xl"
                                onClick={onClose}
                            >
                                Show Results
                            </Button>
                        </Box>
                    </Drawer.Body>
                </Drawer.Content>
            </Drawer.Root>
        </Flex>
    );
}

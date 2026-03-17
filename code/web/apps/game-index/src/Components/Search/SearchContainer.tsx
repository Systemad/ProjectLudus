"use client";

import { useStore } from "@tanstack/react-store";
import { getSearchStore, type SearchState } from "./SearchStore";
import type { TypeOf } from "zod";
import { useEffect, useRef } from "react";
import { useDebouncedValue } from "@mantine/hooks";
import {
    searchQueryParamsSchema,
    type FullTag,
    useGetApiTagsAllTagsHook,
    useSearchHook,
} from "../../gen";
import { keepPreviousData } from "@tanstack/react-query";
import { SearchFilters } from "./SearchFilters";
import { SearchHeader } from "./SearchHeader";
import { SearchResults } from "./SearchResults";
import { Flex, Box, Button, Drawer, useDisclosure } from "@packages/ui";

export interface SearchContainerProps {
    contextId?: string;
    initialSearch?: Partial<TypeOf<typeof searchQueryParamsSchema>>;
    onSearchChange?: (search: TypeOf<typeof searchQueryParamsSchema>) => void;
    fixedParams?: Partial<TypeOf<typeof searchQueryParamsSchema>>;
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

export function SearchContainer({
    contextId,
    initialSearch = {},
    fixedParams = {},
    onSearchChange,
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
            if (Object.is(prev[field], value)) {
                return prev;
            }

            if (field !== "Page" && field !== "Limit") {
                return {
                    ...prev,
                    [field]: value,
                    Page: 1,
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

    const [debouncedName] = useDebouncedValue(query.Name ?? "", 300);

    const mergedQuery = {
        ...mergeSearchStates(query, fixedParams),
        Name: debouncedName || undefined,
        Page: query.Page ?? fixedParams.Page ?? 1,
        Limit: query.Limit ?? fixedParams.Limit ?? 40,
    } as TypeOf<typeof searchQueryParamsSchema>;
    const queryKey = JSON.stringify(mergedQuery);

    const { data: results, isLoading: isResultsLoading } = useSearchHook(
        { params: mergedQuery },
        {
            query: {
                queryKey: ["search", contextId, queryKey],
                placeholderData: keepPreviousData,
                staleTime: 30_000,
                refetchOnWindowFocus: false,
                retry: false,
            },
        }
    );

    const { data: allTags } = useGetApiTagsAllTagsHook({
        query: {
            placeholderData: keepPreviousData,
            refetchInterval: 1000 * 60 * 5,
        },
    });

    const tagCounts: Record<string, number> = (() => {
        const aggregations = results?.aggregationBuckets;
        const tags = (allTags?.tags ?? []) as FullTag[];

        if (!aggregations || tags.length === 0) {
            return {};
        }

        const counts: Record<string, number> = {};
        const tagLookupByGroup = tags.reduce<
            Record<string, Record<string, string>>
        >((acc, tag) => {
            if (!acc[tag.groupName]) {
                acc[tag.groupName] = {};
            }

            acc[tag.groupName][tag.id] = tag.id;
            acc[tag.groupName][tag.name] = tag.id;

            if (tag.slug) {
                acc[tag.groupName][tag.slug] = tag.id;
            }

            return acc;
        }, {});

        for (const [aggregationKey, aggregation] of Object.entries(
            aggregations
        )) {
            const groupLookup = tagLookupByGroup[aggregationKey];
            if (!groupLookup) {
                continue;
            }

            for (const bucket of aggregation.buckets ?? []) {
                const matchedTagId = groupLookup[bucket.key];
                if (!matchedTagId) {
                    continue;
                }

                counts[matchedTagId] = bucket.doc_count;
            }
        }

        return counts;
    })();

    const sanitizedQueryForUrl = {
        ...query,
        ...fixedParams,
    } as TypeOf<typeof searchQueryParamsSchema>;
    const sanitizedQueryForUrlKey = JSON.stringify(sanitizedQueryForUrl);
    const [debouncedUrlSearchKey] = useDebouncedValue(
        sanitizedQueryForUrlKey,
        300
    );

    useEffect(() => {
        if (
            !onSearchChange ||
            !isInitializedRef.current ||
            blockOnSearchChangeRef.current
        ) {
            return;
        }

        const currentSearch = JSON.parse(debouncedUrlSearchKey) as TypeOf<
            typeof searchQueryParamsSchema
        >;
        const prevSearch = prevSearchRef.current;

        if (
            !prevSearch ||
            JSON.stringify(currentSearch) !== JSON.stringify(prevSearch)
        ) {
            prevSearchRef.current = currentSearch;
            onSearchChange(currentSearch);
        }
    }, [onSearchChange, debouncedUrlSearchKey]);

    const { open, onOpen, onClose } = useDisclosure();

    const handleReset = () => {
        setField("Genres", []);
        setField("Themes", []);
        setField("GameModes", []);
        setField("Multiplayer", []);
        setField("Perspectives", []);
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
                    <SearchHeader
                        query={query}
                        onNameChange={(value) => {
                            setField("Name", value || undefined);
                        }}
                    />

                    <SearchResults
                        loading={isResultsLoading}
                        results={results}
                    />
                </Flex>
            </Flex>

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
                        <SearchFilters
                            query={query}
                            setField={setField}
                            tagCounts={tagCounts}
                            allTags={allTags}
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

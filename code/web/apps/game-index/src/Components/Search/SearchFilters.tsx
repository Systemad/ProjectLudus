"use client";

import type { TypeOf } from "zod";
import {
    searchQueryParamsSchema,
    type FullTag,
    type TagResponse,
} from "../../gen";
import {
    FILTER_GROUP_ORDER,
    FILTER_GROUP_TO_QUERY_KEY,
    type FilterQueryKey,
    toFilterGroupLabel,
} from "./filterGroups";
import {
    Flex,
    Box,
    Button,
    Accordion,
    CheckboxCardGroup,
    ScrollArea,
    Text,
    Stack,
    Heading,
} from "@packages/ui";

export interface SearchFiltersProps {
    query: TypeOf<typeof searchQueryParamsSchema>;
    setField: <K extends keyof TypeOf<typeof searchQueryParamsSchema>>(
        field: K,
        value: TypeOf<typeof searchQueryParamsSchema>[K]
    ) => void;
    tagCounts: Record<string, number>;
    allTags?: TagResponse;
    onReset: () => void;
}

function FilterPanel({
    query,
    setField,
    tagCounts,
    allTags,
    onReset,
}: SearchFiltersProps) {
    const tags = (allTags?.tags ?? []) as FullTag[];
    const hasCounts = Object.keys(tagCounts).length > 0;

    const tagsByGroup = tags.reduce<Record<string, FullTag[]>>((acc, tag) => {
        if (!FILTER_GROUP_TO_QUERY_KEY[tag.groupName]) {
            return acc;
        }

        if (!acc[tag.groupName]) {
            acc[tag.groupName] = [];
        }

        acc[tag.groupName].push(tag);
        return acc;
    }, {});

    const sectionGroupNames = FILTER_GROUP_ORDER.filter(
        (groupName) => (tagsByGroup[groupName]?.length ?? 0) > 0
    );

    const getSelectedValues = (field: FilterQueryKey): string[] => {
        return (query[field] ?? []) as string[];
    };

    return (
        <Stack gap="lg" w="full" h="full">
            <Flex justify="space-between" align="center">
                <Heading size="sm">Refine</Heading>
                <Button variant="ghost" size="xs" onClick={onReset}>
                    Reset
                </Button>
            </Flex>

            <Accordion.Root toggle multiple>
                {sectionGroupNames.map((groupName, index) => {
                    const queryKey = FILTER_GROUP_TO_QUERY_KEY[groupName];
                    const groupTags = tagsByGroup[groupName] ?? [];
                    const visibleTags = groupTags.filter((tag) => {
                        if (!hasCounts) {
                            return true;
                        }

                        return (tagCounts[tag.id] ?? 0) > 0;
                    });

                    return (
                        <Accordion.Item
                            button={toFilterGroupLabel(groupName)}
                            index={index}
                            key={groupName}
                        >
                            <Accordion.Panel>
                                <ScrollArea maxH="md" pr="xs">
                                    <Stack gap="sm">
                                        <CheckboxCardGroup.Root
                                            orientation="vertical"
                                            size="sm"
                                            value={getSelectedValues(queryKey)}
                                            onChange={(values: string[]) => {
                                                setField(
                                                    queryKey,
                                                    values as TypeOf<
                                                        typeof searchQueryParamsSchema
                                                    >[FilterQueryKey]
                                                );
                                            }}
                                        >
                                            {visibleTags.map((tag) => {
                                                const value =
                                                    tag.slug ?? tag.name;
                                                const count =
                                                    tagCounts[tag.id] ?? 0;

                                                return (
                                                    <CheckboxCardGroup.Item.Root
                                                        key={tag.id}
                                                        value={value}
                                                        flexDirection="row"
                                                        alignItems="center"
                                                        gap="sm"
                                                        w="full"
                                                    >
                                                        <Text
                                                            as="span"
                                                            fontSize="sm"
                                                            lineClamp={1}
                                                            minW={0}
                                                            flex="1"
                                                        >
                                                            {tag.name}
                                                        </Text>
                                                        <Text
                                                            as="span"
                                                            fontSize="xs"
                                                            color="fg.muted"
                                                            whiteSpace="nowrap"
                                                            ml="auto"
                                                        >
                                                            {count}
                                                        </Text>
                                                    </CheckboxCardGroup.Item.Root>
                                                );
                                            })}
                                        </CheckboxCardGroup.Root>
                                        {visibleTags.length === 0 && (
                                            <Text
                                                color="fg.muted"
                                                fontSize="sm"
                                            >
                                                No tags found
                                            </Text>
                                        )}
                                    </Stack>
                                </ScrollArea>
                            </Accordion.Panel>
                        </Accordion.Item>
                    );
                })}
            </Accordion.Root>
        </Stack>
    );
}

export function SearchFilters({
    query,
    setField,
    tagCounts,
    allTags,
    onReset,
}: SearchFiltersProps) {
    return (
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
                        {Object.values(tagCounts).reduce(
                            (sum, n) => sum + n,
                            0
                        )}
                    </Box>
                </Flex>

                <FilterPanel
                    query={query}
                    setField={setField}
                    tagCounts={tagCounts}
                    allTags={allTags}
                    onReset={onReset}
                />
            </Box>
        </aside>
    );
}

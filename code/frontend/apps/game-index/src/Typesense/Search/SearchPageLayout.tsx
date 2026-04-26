import { useState } from "react";
import { appShellStickyTopOffset } from "@src/components/AppShell/layout.constants";
import { Accordion, Box, Button, Drawer, Grid, Heading, Pagination, Text } from "ui";
import { Hits, Stats, usePagination } from "react-instantsearch";
import { SearchFacetFilterGroup } from "./SearchFacetFilterGroup";
import { SearchHeader } from "./SearchHeader";
import type { SortFieldOption } from "./SearchControl";
import type { HitsProps } from "react-instantsearch";

type SearchFacetConfig = {
    title: string;
    attribute: string;
};

type SearchPageLayoutProps<THit extends Record<string, unknown>> = {
    searchPlaceholder: string;
    indexName: string;
    sortFieldOptions: SortFieldOption[];
    defaultSort: string;
    facets: SearchFacetConfig[];
    hitComponent: NonNullable<HitsProps<THit>["hitComponent"]>;
};

function TypesensePagination() {
    const { currentRefinement, nbPages, refine } = usePagination();

    if (nbPages <= 1) {
        return null;
    }

    return (
        <Box mt="md" w="full" display="flex" justifyContent="center">
            <Pagination.Root
                page={currentRefinement + 1}
                total={nbPages}
                onChange={(page) => refine(page - 1)}
                size="sm"
                variant="outline"
                colorScheme="neutral"
                justify="center"
                wrap="wrap"
                withEdges
                siblings={1}
                boundaries={1}
            />
        </Box>
    );
}

export function SearchPageLayout<THit extends Record<string, unknown>>({
    searchPlaceholder,
    indexName,
    sortFieldOptions,
    defaultSort,
    facets,
    hitComponent,
}: SearchPageLayoutProps<THit>) {
    const [isMobileFiltersOpen, setIsMobileFiltersOpen] = useState(false);

    return (
        <Box
            p={{ base: "xs", md: "sm" }}
            display="grid"
            gap={{ base: "sm", md: "md" }}
            bg="bg.surface"
            rounded="xl"
            css={{
                ".typesense-layout": {
                    display: "grid",
                    gridTemplateColumns: "1fr",
                    gap: "0.875rem",
                },
                "@media (min-width: 48em)": {
                    ".typesense-layout": {
                        gridTemplateColumns: "280px 1fr",
                        alignItems: "start",
                    },
                    ".typesense-facet-desktop": {
                        display: "block",
                    },
                    ".typesense-mobile-filter-trigger": {
                        display: "none",
                    },
                },
                "@media (max-width: calc(48em - 1px))": {
                    ".typesense-facet-desktop": {
                        display: "none",
                    },
                    ".typesense-mobile-filter-trigger": {
                        display: "inline-flex",
                    },
                    ".typesense-hit-list": {
                        gridTemplateColumns: "1fr",
                    },
                },
                "@media (min-width: 30em) and (max-width: calc(48em - 1px))": {
                    ".typesense-hit-list": {
                        gridTemplateColumns: "repeat(2, minmax(0, 1fr))",
                    },
                },
                ".typesense-facet-panel": {
                    position: "sticky",
                },
                ".typesense-hit-list": {
                    display: "grid",
                    gridTemplateColumns: "repeat(auto-fill, minmax(180px, 1fr))",
                    gap: "0.75rem",
                    listStyle: "none",
                    margin: 0,
                    padding: 0,
                },
                ".typesense-hit-item": {
                    margin: 0,
                },
            }}
        >
            <Grid className="typesense-layout">
                <Box
                    as="aside"
                    layerStyle="panel"
                    className="typesense-facet-panel typesense-facet-desktop"
                    top={appShellStickyTopOffset}
                    p="sm"
                    rounded="lg"
                >
                    <Heading size="sm" mb="sm">
                        Filters
                    </Heading>

                    <Accordion.Root
                        multiple
                        defaultIndex={facets.slice(0, 2).map((_, index) => index)}
                    >
                        {facets.map((facet, index) => (
                            <SearchFacetFilterGroup
                                key={facet.attribute}
                                title={facet.title}
                                attribute={facet.attribute}
                                index={index}
                            />
                        ))}
                    </Accordion.Root>
                </Box>

                <Box minW={0}>
                    <SearchHeader
                        searchPlaceholder={searchPlaceholder}
                        indexName={indexName}
                        sortFieldOptions={sortFieldOptions}
                        defaultSort={defaultSort}
                    />

                    <Button
                        className="typesense-mobile-filter-trigger"
                        size="sm"
                        variant="outline"
                        mb="sm"
                        w="full"
                        justifyContent="center"
                        colorScheme="neutral"
                        onClick={() => setIsMobileFiltersOpen(true)}
                    >
                        Filters
                    </Button>

                    <Drawer.Root
                        open={isMobileFiltersOpen}
                        onClose={() => setIsMobileFiltersOpen(false)}
                        placement="block-end"
                        withCloseButton={false}
                    >
                        <Drawer.Content bg="bg.surface" borderTopRadius="xl">
                            <Drawer.Header>
                                <Heading size="sm">Filters</Heading>
                            </Drawer.Header>

                            <Drawer.Body>
                                <Accordion.Root
                                    multiple
                                    defaultIndex={facets.slice(0, 2).map((_, index) => index)}
                                >
                                    {facets.map((facet, index) => (
                                        <SearchFacetFilterGroup
                                            key={`mobile-${facet.attribute}`}
                                            title={facet.title}
                                            attribute={facet.attribute}
                                            index={index}
                                        />
                                    ))}
                                </Accordion.Root>
                            </Drawer.Body>

                            <Drawer.Footer>
                                <Box w="full" display="flex" justifyContent="end">
                                    <Button
                                        size="sm"
                                        variant="outline"
                                        colorScheme="neutral"
                                        onClick={() => setIsMobileFiltersOpen(false)}
                                    >
                                        Close
                                    </Button>
                                </Box>
                            </Drawer.Footer>
                        </Drawer.Content>
                    </Drawer.Root>

                    <Hits<THit>
                        hitComponent={hitComponent}
                        classNames={{
                            list: "typesense-hit-list",
                            item: "typesense-hit-item",
                        }}
                    />

                    <TypesensePagination />

                    <Box mt="sm" textAlign={{ base: "start", md: "end" }}>
                        <Text fontSize="sm" color="fg.subtle">
                            <Stats />
                        </Text>
                    </Box>
                </Box>
            </Grid>
        </Box>
    );
}

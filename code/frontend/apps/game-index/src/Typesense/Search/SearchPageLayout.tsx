import { useState } from "react";
import { appShellStickyTopOffset } from "@src/components/AppShell/layout.constants";
import { Accordion, Box, Button, Drawer, Grid, Heading, Pagination } from "ui";
import { Hits, usePagination } from "react-instantsearch";
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
        <Box mt="lg" w="full" display="flex" justifyContent="center">
            <Pagination.Root
                page={currentRefinement + 1}
                total={nbPages}
                onChange={(page) => refine(page - 1)}
                size="sm"
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
        <Box p={{ base: "xs", md: "sm" }} bg="bg.surface" rounded="xl">
            <Grid
                templateColumns={{ base: "1fr", md: "280px 1fr" }}
                gap={{ base: "sm", md: "md" }}
                alignItems="start"
                css={{
                    ".typesense-hit-list": {
                        display: "grid",
                        gridTemplateColumns:
                            "repeat(auto-fill, minmax(clamp(140px, 25vw, 200px), 1fr))",
                        gap: "1rem",
                        listStyle: "none",
                        margin: 0,
                        padding: 0,
                    },
                    ".typesense-hit-item": {
                        margin: 0,
                    },
                }}
            >
                <Box
                    as="aside"
                    display={{ base: "none", md: "block" }}
                    position="sticky"
                    top={appShellStickyTopOffset}
                    p="sm"
                    rounded="lg"
                >
                    <Heading size="md" mb="sm">
                        Filters
                    </Heading>

                    <Accordion.Root
                        multiple
                        variant="panel"
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
                        display={{ base: "inline-flex", md: "none" }}
                        size="sm"
                        mb="sm"
                        w="full"
                        colorScheme={"gray"}
                        justifyContent="center"
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
                                {isMobileFiltersOpen && (
                                    <Accordion.Root multiple defaultIndex={[0, 1]}>
                                        {facets.map((facet, index) => (
                                            <SearchFacetFilterGroup
                                                key={`mobile-${facet.attribute}`}
                                                title={facet.title}
                                                attribute={facet.attribute}
                                                index={index}
                                            />
                                        ))}
                                    </Accordion.Root>
                                )}
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
                </Box>
            </Grid>
        </Box>
    );
}

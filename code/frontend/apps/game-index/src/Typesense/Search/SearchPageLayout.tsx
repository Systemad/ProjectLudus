import { useState } from "react";
import { Accordion, Box, Button, Drawer, Grid, Heading } from "ui";
import { Hits, Pagination, Stats } from "react-instantsearch";
import { CardSurface } from "@src/components/layout/Card";
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
    facets: SearchFacetConfig[];
    hitComponent: NonNullable<HitsProps<THit>["hitComponent"]>;
};

export function SearchPageLayout<THit extends Record<string, unknown>>({
    searchPlaceholder,
    indexName,
    sortFieldOptions,
    facets,
    hitComponent,
}: SearchPageLayoutProps<THit>) {
    const [isMobileFiltersOpen, setIsMobileFiltersOpen] = useState(false);

    return (
        <Box
            p={{ base: "sm", md: "md" }}
            display="grid"
            gap="md"
            css={{
                ".typesense-layout": {
                    display: "grid",
                    gridTemplateColumns: "1fr",
                    gap: "1rem",
                },
                "@media (min-width: 900px)": {
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
                "@media (max-width: 899px)": {
                    ".typesense-facet-desktop": {
                        display: "none",
                    },
                    ".typesense-mobile-filter-trigger": {
                        display: "inline-flex",
                    },
                    ".typesense-hit-list": {
                        gridTemplateColumns: "repeat(2, minmax(0, 1fr))",
                    },
                },
                ".typesense-facet-panel": {
                    position: "sticky",
                    top: "1rem",
                },
                ".typesense-searchbox": {
                    marginBottom: "1rem",
                },
                ".typesense-hit-list": {
                    display: "grid",
                    gridTemplateColumns: "repeat(auto-fill, minmax(180px, 1fr))",
                    gap: "0.875rem",
                    listStyle: "none",
                    margin: 0,
                    padding: 0,
                },
                ".typesense-hit-item": {
                    margin: 0,
                },
                ".typesense-pagination": {
                    marginTop: "1rem",
                },
                ".typesense-pagination-list": {
                    listStyle: "none",
                    margin: 0,
                    padding: 0,
                    display: "flex",
                    gap: "0.35rem",
                    flexWrap: "wrap",
                    justifyContent: "center",
                },
                ".typesense-pagination-item": {
                    margin: 0,
                },
                ".typesense-pagination-link": {
                    display: "inline-flex",
                    alignItems: "center",
                    justifyContent: "center",
                    minWidth: "2rem",
                    height: "2rem",
                    borderRadius: "0.5rem",
                    border: "1px solid rgba(255,255,255,0.12)",
                    textDecoration: "none",
                    color: "inherit",
                    background: "rgba(18,18,20,0.55)",
                    backdropFilter: "blur(10px) saturate(110%)",
                },
                ".typesense-pagination-item--selected .typesense-pagination-link": {
                    background: "rgba(93, 163, 255, 0.18)",
                    borderColor: "rgba(163, 210, 255, 0.35)",
                },
            }}
        >
            <Grid className="typesense-layout">
                <CardSurface
                    as="aside"
                    variant="translucent"
                    className="typesense-facet-panel typesense-facet-desktop"
                    p="md"
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
                </CardSurface>

                <Box minW={0}>
                    <SearchHeader
                        searchPlaceholder={searchPlaceholder}
                        indexName={indexName}
                        sortFieldOptions={sortFieldOptions}
                    />

                    <Button
                        className="typesense-mobile-filter-trigger"
                        size="sm"
                        variant="outline"
                        mb="sm"
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
                        <Drawer.Content>
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
                                    <Button size="sm" onClick={() => setIsMobileFiltersOpen(false)}>
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

                    <Pagination
                        classNames={{
                            root: "typesense-pagination",
                            list: "typesense-pagination-list",
                            item: "typesense-pagination-item",
                            selectedItem: "typesense-pagination-item--selected",
                            link: "typesense-pagination-link",
                        }}
                    />

                    <Box mt="sm" textAlign="end">
                        <Stats />
                    </Box>
                </Box>
            </Grid>
        </Box>
    );
}

import { useState } from "react";
import { Button } from "@astryxdesign/core/Button";
import { Grid } from "@astryxdesign/core/Grid";
import { Text } from "@astryxdesign/core/Text";
import { Pagination } from "@astryxdesign/core/Pagination";
import { Dialog, DialogHeader } from "@astryxdesign/core/Dialog";
import { Layout, LayoutContent } from "@astryxdesign/core/Layout";
import * as stylex from "@stylexjs/stylex";
import { Hits, usePagination } from "react-instantsearch";
import type { HitsProps } from "react-instantsearch";
import type { SortFieldOption } from "./search-control";
import { SearchFacetFilterGroup } from "./search-facets";
import { SearchHeader } from "./search-header";

type SearchFacetConfig = {
    title: string;
    attribute: string;
};

const styles = stylex.create({
    page: {
        padding: "var(--spacing-2)",
        backgroundColor: "var(--color-background-surface)",
        borderRadius: "var(--radius-container)",
    },
    pagination: {
        display: "flex",
        justifyContent: "center",
        marginTop: "var(--spacing-4)",
        width: "100%",
    },
    desktopFacets: {
        display: "none",
        position: "sticky",
        top: "var(--spacing-10)",
        padding: "var(--spacing-2)",
        "@media (min-width: 641px)": {
            display: "block",
        },
    },
    filterHeading: {
        fontSize: "1.125rem",
        fontWeight: 600,
        marginBottom: "var(--spacing-2)",
    },
    results: {
        minWidth: 0,
    },
    mobileFilters: {
        display: "inline-flex",
        "@media (min-width: 641px)": {
            display: "none",
        },
    },
});

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
        <div {...stylex.props(styles.pagination)}>
            <Pagination
                page={currentRefinement + 1}
                totalPages={nbPages}
                onChange={(page) => refine(page - 1)}
                size="sm"
                siblingCount={1}
            />
        </div>
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
        <div {...stylex.props(styles.page)}>
            <Grid columns={{minWidth: 280}} gap={4}>
                <div {...stylex.props(styles.desktopFacets)}>
                    <Text as="h2" xstyle={styles.filterHeading}>
                        Filters
                    </Text>

                    {facets.map((facet, index) => (
                        <SearchFacetFilterGroup
                            key={facet.attribute}
                            title={facet.title}
                            attribute={facet.attribute}
                            index={index}
                        />
                    ))}
                </div>

                <div {...stylex.props(styles.results)}>
                    <SearchHeader
                        searchPlaceholder={searchPlaceholder}
                        indexName={indexName}
                        sortFieldOptions={sortFieldOptions}
                        defaultSort={defaultSort}
                    />

                    <Button
                        size="sm"
                        label="Filters"
                        onClick={() => setIsMobileFiltersOpen(true)}
                        xstyle={styles.mobileFilters}
                    />

                    <Dialog isOpen={isMobileFiltersOpen} onOpenChange={setIsMobileFiltersOpen} variant="fullscreen">
                        <Layout
                            header={<DialogHeader title="Filters" onOpenChange={() => setIsMobileFiltersOpen(false)} />}
                            content={
                                <LayoutContent>
                                {facets.map((facet, index) => (
                                    <SearchFacetFilterGroup
                                        key={`mobile-${facet.attribute}`}
                                        title={facet.title}
                                        attribute={facet.attribute}
                                        index={index}
                                    />
                                ))}
                                </LayoutContent>
                            }
                        />
                    </Dialog>

                    <Hits<THit>
                        hitComponent={hitComponent}
                        classNames={{
                            list: "typesense-hit-list",
                            item: "typesense-hit-item",
                        }}
                    />

                    <TypesensePagination />
                </div>
            </Grid>
        </div>
    );
}

import { useState } from "react";
import { Button } from "@astryxdesign/core/Button";
import { Grid } from "@astryxdesign/core/Grid";
import { Text } from "@astryxdesign/core/Text";
import { Pagination } from "@astryxdesign/core/Pagination";
import { Hits, usePagination } from "react-instantsearch";
import type { HitsProps } from "react-instantsearch";
import type { SortFieldOption } from "./search-control";
import { SearchFacetFilterGroup } from "./search-facets";
import { SearchHeader } from "./search-header";

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
        <div style={{ marginTop: "1rem", width: "100%", display: "flex", justifyContent: "center" }}>
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
        <div style={{ padding: "0.25rem", background: "var(--bg-surface)", borderRadius: "var(--radius-xl)" }}>
            <Grid columns={{minWidth: 280}} gap={4}
                style={{
                    "--typesense-hit-grid": "repeat(auto-fill, minmax(clamp(140px, 25vw, 200px), 1fr))",
                } as React.CSSProperties}
            >
                <div
                    style={{
                        display: "none",
                        position: "sticky",
                        top: "6rem",
                        padding: "0.5rem",
                        borderRadius: "var(--radius-lg)",
                    }}
                    className="desktop-aside"
                >
                    <Text as="h2" style={{fontSize: "1.125rem", fontWeight: 600, marginBottom: "0.5rem"}}>
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

                <div style={{ minWidth: 0 }}>
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
                        style={{display: "inline-flex"}}
                    />

                    {isMobileFiltersOpen && (
                        <div
                            style={{
                                position: "fixed",
                                inset: 0,
                                zIndex: 1000,
                                background: "rgba(0,0,0,0.5)",
                                display: "flex",
                                alignItems: "flex-end",
                            }}
                            onClick={() => setIsMobileFiltersOpen(false)}
                        >
                            <div
                                style={{
                                    width: "100%",
                                    maxHeight: "80vh",
                                    overflowY: "auto",
                                    background: "var(--bg-surface)",
                                    borderRadius: "var(--radius-xl) var(--radius-xl) 0 0",
                                    padding: "1rem",
                                }}
                                onClick={(e) => e.stopPropagation()}
                            >
                                <Text as="h2" style={{fontSize: "0.875rem", fontWeight: 600, marginBottom: "0.75rem"}}>Filters</Text>
                                {facets.map((facet, index) => (
                                    <SearchFacetFilterGroup
                                        key={`mobile-${facet.attribute}`}
                                        title={facet.title}
                                        attribute={facet.attribute}
                                        index={index}
                                    />
                                ))}
                                <div style={{ width: "100%", display: "flex", justifyContent: "end", marginTop: "0.75rem" }}>
                                    <Button
                                        size="sm"
                                        variant="secondary"
                                        label="Close"
                                        onClick={() => setIsMobileFiltersOpen(false)}
                                    />
                                </div>
                            </div>
                        </div>
                    )}

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

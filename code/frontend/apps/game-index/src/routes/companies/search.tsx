import { createFileRoute } from "@tanstack/react-router";
import { COMPANIES_SEARCH_INDEX_NAME } from "@src/Typesense/instantsearch";
import { CompanyHitCard } from "@src/Typesense/Search/CompanyHitCard";
import { SearchPageLayout } from "@src/Typesense/Search/SearchPageLayout";
import { useSearchController } from "@src/Typesense/Search/useSearchController";
import { createSearchRouting } from "@src/Typesense/Search/createSearchRouting";
import { Configure, InstantSearch } from "react-instantsearch";
import type { ComponentProps } from "react";

const DEFAULT_SORT = "games_published_count:desc";
const FACET_ATTRIBUTES = ["status"] as const;

const routing = createSearchRouting(COMPANIES_SEARCH_INDEX_NAME, DEFAULT_SORT, FACET_ATTRIBUTES);
const routingConfig = routing as NonNullable<ComponentProps<typeof InstantSearch>["routing"]>;

export const Route = createFileRoute("/companies/search")({
    component: RouteComponent,
});

function RouteComponent() {
    const { searchClient } = useSearchController(COMPANIES_SEARCH_INDEX_NAME);

    return (
        <InstantSearch
            searchClient={searchClient}
            indexName={COMPANIES_SEARCH_INDEX_NAME}
            routing={routingConfig}
            future={{ preserveSharedStateOnUnmount: true }}
        >
            <Configure hitsPerPage={24} />

            <SearchPageLayout
                searchPlaceholder="Search companies..."
                indexName={COMPANIES_SEARCH_INDEX_NAME}
                sortFieldOptions={[
                    { label: "Relevancy", value: "relevancy" },
                    { label: "Games Published", value: "games_published_count" },
                    { label: "Games Developed", value: "games_developed_count" },
                ]}
                facets={[{ title: "Status", attribute: "status" }]}
                hitComponent={CompanyHitCard}
            />
        </InstantSearch>
    );
}

import { createFileRoute } from "@tanstack/react-router";
import {
    COMPANIES_SEARCH_INDEX_NAME,
    companiesSearchClient,
} from "@src/features/search/instantsearch";
import { CompanyHitCard } from "@src/features/search/hits/company-hit-card";
import { SearchPageLayout } from "@src/features/search/components/search-page-layout";
import { PageWrapper } from "@src/app/page-wrapper";
import { Configure, InstantSearch } from "react-instantsearch";

export const Route = createFileRoute("/companies/search")({
    component: RouteComponent,
});

function RouteComponent() {
    return (
        <InstantSearch
            searchClient={companiesSearchClient}
            indexName={COMPANIES_SEARCH_INDEX_NAME}
            future={{ preserveSharedStateOnUnmount: true }}
        >
            <Configure hitsPerPage={24} />

            <PageWrapper py={{ base: "2", md: "4" }}>
                <SearchPageLayout
                    searchPlaceholder="Search companies..."
                    indexName={COMPANIES_SEARCH_INDEX_NAME}
                    defaultSort="games_published_count:desc"
                    sortFieldOptions={[
                        { label: "Games Published", value: "games_published_count" },
                        { label: "Games Developed", value: "games_developed_count" },
                    ]}
                    facets={[{ title: "Status", attribute: "status" }]}
                    hitComponent={CompanyHitCard}
                />
            </PageWrapper>
        </InstantSearch>
    );
}

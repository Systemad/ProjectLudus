import type { SearchClient } from "typesense-instantsearch-adapter";
import {
    companiesSearchClient,
    gamesSearchClient,
    COMPANIES_SEARCH_INDEX_NAME,
} from "@src/Typesense/instantsearch";

export type SearchControllerResult = {
    searchClient: SearchClient;
};

export function useSearchController(indexName: string): SearchControllerResult {
    const isCompaniesIndex = indexName === COMPANIES_SEARCH_INDEX_NAME;
    const searchClient = isCompaniesIndex ? companiesSearchClient : gamesSearchClient;

    return {
        searchClient,
    };
}

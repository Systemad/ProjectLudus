import TypesenseInstantSearchAdapter from "typesense-instantsearch-adapter";

export const GAMES_SEARCH_INDEX_NAME = "search___games_search";
export const COMPANIES_SEARCH_INDEX_NAME = "company_search___company_search";
// process.env.NEXT_PUBLIC_TYPESENSE_API_KEY
const baseServerConfig = {
    apiKey: "qH3qAqXoEkRxKZcJDJeGe4bIm5g5ObnT",
    nodes: [
        {
            host: "localhost",
            port: 8108,
            path: "",
            protocol: "http",
        },
    ],
    cacheSearchResultsForSeconds: 0,
};

export const gamesSearchClient = new TypesenseInstantSearchAdapter({
    server: baseServerConfig,
    additionalSearchParameters: {
        query_by: "name,genres,themes,game_modes,multiplayer_modes,player_perspectives",
        query_by_weights: "12,3,2,2,1,1",
        sort_by: "aggregated_rating:desc",
        text_match_type: "max_score",
    },
}).searchClient;

export const companiesSearchClient = new TypesenseInstantSearchAdapter({
    server: baseServerConfig,
    additionalSearchParameters: {
        query_by: "name,status",
        query_by_weights: "12,4",
        text_match_type: "max_score",
    },
}).searchClient;

export const SEARCH_INDEX_NAME = GAMES_SEARCH_INDEX_NAME;
export const COMPANY_SEARCH_INDEX_NAME = COMPANIES_SEARCH_INDEX_NAME;

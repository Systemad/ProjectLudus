import TypesenseInstantSearchAdapter from "typesense-instantsearch-adapter";

export const SEARCH_INDEX_NAME = "search___games_search";
// process.env.NEXT_PUBLIC_TYPESENSE_API_KEY
const baseServerConfig = {
    apiKey: "PeONwmjZnjhprfb8qM539fgWFyknThSR",
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
const searchInstantsearchAdapter = new TypesenseInstantSearchAdapter({
    server: baseServerConfig,
    additionalSearchParameters: {
        query_by: "name,genres,themes,game_modes,multiplayer_modes,player_perspectives",
        query_by_weights: "12,3,2,2,1,1",
        sort_by: "aggregated_rating:desc",
        text_match_type: "max_score",
    },
});

export const searchClient = searchInstantsearchAdapter.searchClient;

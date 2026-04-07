import TypesenseInstantSearchAdapter from "typesense-instantsearch-adapter";

export const SEARCH_INDEX_NAME = "search___games_search";

const typesenseInstantsearchAdapter = new TypesenseInstantSearchAdapter({
    server: {
        apiKey: "GqGGZsi5vpsNXOGY74tuNfUnQrc1Bf8U", // Be sure to use an API key that only allows search operations
        nodes: [
            {
                host: "localhost",
                port: 8108,
                path: "", // Optional. Example: If you have your typesense mounted in localhost:8108/typesense, path should be equal to '/typesense'
                protocol: "http",
            },
        ],
        cacheSearchResultsForSeconds: 6 * 60, // Cache search results from server. Defaults to 2 minutes. Set to 0 to disable caching.
    },
    // The following parameters are directly passed to Typesense's search API endpoint.
    //  So you can pass any parameters supported by the search endpoint below.
    //  query_by is required.
    additionalSearchParameters: {
        query_by: "name,genres,themes,game_modes,multiplayer_modes,player_perspectives",
        query_by_weights: "12,3,2,2,1,1",
        text_match_type: "max_score",
    },
});

export const searchClient = typesenseInstantsearchAdapter.searchClient;

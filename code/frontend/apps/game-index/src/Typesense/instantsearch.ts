import TypesenseInstantSearchAdapter from "typesense-instantsearch-adapter";

export const SEARCH_INDEX_NAME = "search___games_search";

// Keep search and homepage rails on separate adapter instances.
// Sharing adapter-level additionalSearchParameters can cause rails to render identical lists.

const baseServerConfig = {
    apiKey: "PeONwmjZnjhprfb8qM539fgWFyknThSR", // Be sure to use an API key that only allows search operations
    nodes: [
        {
            host: "localhost",
            port: 8108,
            path: "", // Optional. Example: If you have your typesense mounted in localhost:8108/typesense, path should be equal to '/typesense'
            protocol: "http",
        },
    ],
    cacheSearchResultsForSeconds: 0,
};
const searchInstantsearchAdapter = new TypesenseInstantSearchAdapter({
    server: baseServerConfig,
    // The following parameters are directly passed to Typesense's search API endpoint.
    //  So you can pass any parameters supported by the search endpoint below.
    //  query_by is required.
    additionalSearchParameters: {
        query_by: "name,genres,themes,game_modes,multiplayer_modes,player_perspectives",
        //query_by_weights: "12,3,2,2,1,1",
        //text_match_type: "max_score",
    },
});

const releasingInstantsearchAdapter = new TypesenseInstantSearchAdapter({
    server: baseServerConfig,
    additionalSearchParameters: {
        query_by: "name",
        sort_by: "aggregated_rating:desc",
    },
});

const steamPeakPlayersInstantsearchAdapter = new TypesenseInstantSearchAdapter({
    server: baseServerConfig,
    additionalSearchParameters: {
        query_by: "name",
        sort_by: "steam_24hr_peak_players:desc",
    },
});

const steamWishlistedUpcomingInstantsearchAdapter = new TypesenseInstantSearchAdapter({
    server: baseServerConfig,
    additionalSearchParameters: {
        query_by: "name",
        sort_by: "steam_most_wishlisted_upcoming:desc",
    },
});

export const searchClient = searchInstantsearchAdapter.searchClient;
export const releasingSearchClient = releasingInstantsearchAdapter.searchClient;
export const steamPeakPlayersSearchClient = steamPeakPlayersInstantsearchAdapter.searchClient;
export const steamWishlistedUpcomingSearchClient =
    steamWishlistedUpcomingInstantsearchAdapter.searchClient;

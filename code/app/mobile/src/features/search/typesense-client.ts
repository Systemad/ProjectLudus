import TypesenseInstantSearchAdapter from "typesense-instantsearch-adapter";

export const GAMES_SEARCH_INDEX_NAME = "games___games_search";

const server = {
  apiKey: process.env.EXPO_PUBLIC_TYPESENSE_SEARCH_API_KEY!,
  nodes: [
    {
      host: process.env.EXPO_PUBLIC_TYPESENSE_HOST!,
      port: Number(process.env.EXPO_PUBLIC_TYPESENSE_PORT),
      protocol: process.env.EXPO_PUBLIC_TYPESENSE_PROTOCOL === "https" ? "https" : "http",
    },
  ],
  cacheSearchResultsForSeconds: 120,
};

export const searchClient = new TypesenseInstantSearchAdapter({
  server,
  additionalSearchParameters: {
    query_by: "name",
    query_by_weights: "12",
    text_match_type: "max_score",
  },
  collectionSpecificSearchParameters: {
    [GAMES_SEARCH_INDEX_NAME]: {
      query_by: "name,genres,themes,game_modes,multiplayer_modes,player_perspectives",
      query_by_weights: "12,3,2,2,1,1",
      facet_by: "game_type,genres,themes,game_modes,multiplayer_modes,player_perspectives",
      sort_by: "aggregated_rating:desc",
    },
  },
}).searchClient;

import { createFileRoute } from "@tanstack/react-router";
import { GAMES_SEARCH_INDEX_NAME } from "@src/Typesense/instantsearch";
import { SearchPageLayout } from "@src/Typesense/Search/SearchPageLayout";
import { GameHitCard } from "@src/Typesense/Search/GameHitCard";
import { useSearchController } from "@src/Typesense/Search/useSearchController";
import { createSearchRouting } from "@src/Typesense/Search/createSearchRouting";
import { Configure, InstantSearch } from "react-instantsearch";
import type { ComponentProps } from "react";

const DEFAULT_SORT = "aggregated_rating:desc";
const FACET_ATTRIBUTES = [
    "game_type",
    "genres",
    "themes",
    "game_modes",
    "multiplayer_modes",
    "player_perspectives",
] as const;

const routing = createSearchRouting(GAMES_SEARCH_INDEX_NAME, DEFAULT_SORT, FACET_ATTRIBUTES);
const routingConfig = routing as NonNullable<ComponentProps<typeof InstantSearch>["routing"]>;

export const Route = createFileRoute("/games/search")({
    component: RouteComponent,
});

function RouteComponent() {
    const { searchClient } = useSearchController(GAMES_SEARCH_INDEX_NAME);

    return (
        <InstantSearch
            searchClient={searchClient}
            indexName={GAMES_SEARCH_INDEX_NAME}
            routing={routingConfig}
            future={{ preserveSharedStateOnUnmount: true }}
        >
            <Configure hitsPerPage={24} />

            <SearchPageLayout
                searchPlaceholder="Search games..."
                indexName={GAMES_SEARCH_INDEX_NAME}
                sortFieldOptions={[
                    { label: "Relevancy", value: "relevancy" },
                    { label: "Aggregated Rating", value: "aggregated_rating" },
                ]}
                facets={[
                    { title: "Game Type", attribute: "game_type" },
                    { title: "Genres", attribute: "genres" },
                    { title: "Themes", attribute: "themes" },
                    { title: "Game Modes", attribute: "game_modes" },
                    { title: "Multiplayer Modes", attribute: "multiplayer_modes" },
                    { title: "Player Perspectives", attribute: "player_perspectives" },
                ]}
                hitComponent={GameHitCard}
            />
        </InstantSearch>
    );
}

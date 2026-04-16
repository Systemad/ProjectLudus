import { createFileRoute } from "@tanstack/react-router";
import { GAMES_SEARCH_INDEX_NAME, gamesSearchClient } from "@src/Typesense/instantsearch";
import { SearchPageLayout } from "@src/Typesense/Search/SearchPageLayout";
import { GameHitCard } from "@src/Typesense/Search/GameHitCard";
import { PageWrapper } from "@src/components/layout/PageWrapper";
import { Configure, InstantSearch } from "react-instantsearch";

export const Route = createFileRoute("/games/search")({
    component: RouteComponent,
});

function RouteComponent() {
    return (
        <InstantSearch
            searchClient={gamesSearchClient}
            indexName={GAMES_SEARCH_INDEX_NAME}
            future={{ preserveSharedStateOnUnmount: true }}
        >
            <Configure hitsPerPage={24} />

            <PageWrapper py={{ base: "2", md: "4" }}>
                <SearchPageLayout
                    searchPlaceholder="Search games..."
                    indexName={GAMES_SEARCH_INDEX_NAME}
                    defaultSort="aggregated_rating:desc"
                    sortFieldOptions={[{ label: "Aggregated Rating", value: "aggregated_rating" }]}
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
            </PageWrapper>
        </InstantSearch>
    );
}

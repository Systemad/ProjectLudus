import { createFileRoute } from "@tanstack/react-router";
import { GAMES_SEARCH_INDEX_NAME, gamesSearchClient } from "@src/features/search/instantsearch";
import { SearchPageLayout } from "@src/features/search/components/search-page-layout";
import type { GameSearchHit } from "@src/features/search/utils/hits";
import { PageWrapper } from "@src/app/page-wrapper";
import { GameCard } from "@src/features/game/components/game-card";
import { Configure, InstantSearch } from "react-instantsearch";

export const Route = createFileRoute("/games/search")({
    component: RouteComponent,
});

function GameSearchHitAsCard({ hit }: { hit: GameSearchHit }) {
    return (
        <GameCard
            game={{
                id: hit.id,
                name: hit.name ?? "Untitled game",
                coverUrl: hit.cover_url ?? null,
            }}
        />
    );
}

function RouteComponent() {
    return (
        <InstantSearch
            searchClient={gamesSearchClient}
            indexName={GAMES_SEARCH_INDEX_NAME}
            future={{ preserveSharedStateOnUnmount: true }}
        >
            <Configure hitsPerPage={24} />

            <PageWrapper paddingBlock="clamp(0.5rem, 2vw, 1rem)">
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
                    hitComponent={GameSearchHitAsCard}
                />
            </PageWrapper>
        </InstantSearch>
    );
}

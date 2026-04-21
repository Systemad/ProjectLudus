import { createFileRoute } from "@tanstack/react-router";
import { GAMES_SEARCH_INDEX_NAME, gamesSearchClient } from "@src/Typesense/instantsearch";
import { SearchPageLayout } from "@src/Typesense/Search/SearchPageLayout";
import type { GameSearchHit } from "@src/Typesense/utils/hits";
import { PageWrapper } from "@src/components/AppShell/PageWrapper";
import { GameCard } from "@src/components/game/GameCard";
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
                    hitComponent={GameSearchHitAsCard}
                />
            </PageWrapper>
        </InstantSearch>
    );
}

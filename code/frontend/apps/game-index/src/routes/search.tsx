import { createFileRoute, useSearch, stripSearchParams } from "@tanstack/react-router";
import { Accordion, Box, Flex, Grid, Heading } from "ui";
import { Configure, Pagination, InstantSearch, Stats, Hits } from "react-instantsearch";
import { SEARCH_INDEX_NAME, searchClient } from "@src/Typesense/instantsearch";
import { SearchFacetFilterGroup } from "@src/Typesense/Search/SearchFacetFilterGroup";
import { SearchHitCard } from "@src/Typesense/Search/SearchHitCard";
import { SearchHeader } from "@src/Typesense/Search/SearchHeader";
import { CardSurface } from "@src/components/layout/Card";
import z from "zod";

const parseArray = z.preprocess((val) => {
    if (typeof val === "string") {
        return val.split(",").filter(Boolean);
    }
    if (Array.isArray(val)) {
        return val;
    }
    return [];
}, z.array(z.string()));

const DEFAULT_SORT = "aggregated_rating:desc";

export const Route = createFileRoute("/search")({
    component: RouteComponent,
    validateSearch: z.object({
        q: z.string().default(""),
        page: z.coerce.number().min(1).default(1),
        game_type: parseArray.default([]),
        genres: parseArray.default([]),
        themes: parseArray.default([]),
        game_modes: parseArray.default([]),
        multiplayer_modes: parseArray.default([]),
        player_perspectives: parseArray.default([]),
        sort: z.string().optional().default(DEFAULT_SORT),
    }),
    search: {
        middlewares: [
            stripSearchParams({
                game_type: [],
                genres: [],
                themes: [],
                game_modes: [],
                multiplayer_modes: [],
                player_perspectives: [],
                q: "",
                page: 1,
                sort: DEFAULT_SORT,
            }),
        ],
    },
});

function RouteComponent() {
    const search = useSearch({ from: Route.fullPath });

    const uiState = {
        [SEARCH_INDEX_NAME]: {
            query: search.q,
            page: search.page,
            refinementList: {
                game_type: search.game_type,
                genres: search.genres,
                themes: search.themes,
                game_modes: search.game_modes,
                multiplayer_modes: search.multiplayer_modes,
                player_perspectives: search.player_perspectives,
            },
            sortBy: search.sort ? `${SEARCH_INDEX_NAME}/sort/${search.sort}` : SEARCH_INDEX_NAME,
        },
    };

    return (
        <Box
            p={{ base: "sm", md: "md" }}
            display="grid"
            gap="md"
            css={{
                ".typesense-layout": {
                    display: "grid",
                    gridTemplateColumns: "1fr",
                    gap: "1rem",
                },
                "@media (min-width: 900px)": {
                    ".typesense-layout": {
                        gridTemplateColumns: "280px 1fr",
                        alignItems: "start",
                    },
                },
                ".typesense-facet-panel": {
                    position: "sticky",
                    top: "1rem",
                },
                ".typesense-searchbox": {
                    marginBottom: "1rem",
                },
                ".typesense-hit-list": {
                    display: "grid",
                    gridTemplateColumns: "repeat(auto-fill, minmax(220px, 1fr))",
                    gap: "0.875rem",
                    listStyle: "none",
                    margin: 0,
                    padding: 0,
                },
                ".typesense-hit-item": {
                    margin: 0,
                },
                ".typesense-pagination": {
                    marginTop: "1rem",
                },
                ".typesense-pagination-list": {
                    listStyle: "none",
                    margin: 0,
                    padding: 0,
                    display: "flex",
                    gap: "0.35rem",
                    flexWrap: "wrap",
                    justifyContent: "center",
                },
                ".typesense-pagination-item": {
                    margin: 0,
                },
                ".typesense-pagination-link": {
                    display: "inline-flex",
                    alignItems: "center",
                    justifyContent: "center",
                    minWidth: "2rem",
                    height: "2rem",
                    borderRadius: "0.5rem",
                    border: "1px solid rgba(255,255,255,0.12)",
                    textDecoration: "none",
                    color: "inherit",
                    background: "rgba(18,18,20,0.55)",
                    backdropFilter: "blur(10px) saturate(110%)",
                },
                ".typesense-pagination-item--selected .typesense-pagination-link": {
                    background: "rgba(93, 163, 255, 0.18)",
                    borderColor: "rgba(163, 210, 255, 0.35)",
                },
            }}
        >
            <InstantSearch
                searchClient={searchClient}
                indexName={SEARCH_INDEX_NAME}
                initialUiState={uiState}
                future={{ preserveSharedStateOnUnmount: true }}
            >
                <Configure hitsPerPage={24} />

                <Grid className="typesense-layout">
                    <CardSurface
                        as="aside"
                        variant="translucent"
                        className="typesense-facet-panel"
                        p="md"
                    >
                        <Heading size="sm" mb="sm">
                            Filters
                        </Heading>

                        <Accordion.Root multiple defaultIndex={[0, 1]}>
                            <SearchFacetFilterGroup
                                title="Game Type"
                                attribute="game_type"
                                index={0}
                                currentValues={search.game_type}
                            />

                            <SearchFacetFilterGroup
                                title="Genres"
                                attribute="genres"
                                index={1}
                                currentValues={search.genres}
                            />
                            <SearchFacetFilterGroup
                                title="Themes"
                                attribute="themes"
                                index={2}
                                currentValues={search.themes}
                            />
                            <SearchFacetFilterGroup
                                title="Game Modes"
                                attribute="game_modes"
                                index={3}
                                currentValues={search.game_modes}
                            />
                            <SearchFacetFilterGroup
                                title="Multiplayer Modes"
                                attribute="multiplayer_modes"
                                index={4}
                                currentValues={search.multiplayer_modes}
                            />
                            <SearchFacetFilterGroup
                                title="Player Perspectives"
                                attribute="player_perspectives"
                                index={5}
                                currentValues={search.player_perspectives}
                            />
                        </Accordion.Root>
                    </CardSurface>

                    <Box minW={0}>
                        <SearchHeader />

                        <Flex align="center" justify="space-between" mb="sm" gap="sm" wrap="wrap">
                            <Stats />
                        </Flex>

                        <Hits
                            hitComponent={SearchHitCard}
                            classNames={{
                                list: "typesense-hit-list",
                                item: "typesense-hit-item",
                            }}
                        />

                        <Pagination
                            classNames={{
                                root: "typesense-pagination",
                                list: "typesense-pagination-list",
                                item: "typesense-pagination-item",
                                selectedItem: "typesense-pagination-item--selected",
                                link: "typesense-pagination-link",
                            }}
                        />
                    </Box>
                </Grid>
            </InstantSearch>
        </Box>
    );
}

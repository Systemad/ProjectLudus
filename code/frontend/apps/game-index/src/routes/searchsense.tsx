import { createFileRoute } from "@tanstack/react-router";
import { Accordion, Box, Flex, Grid, Heading } from "ui";
import { Configure, Pagination, InstantSearch, Stats, Hits } from "react-instantsearch";
import Card from "@src/components/ui/Card";
import { SEARCH_INDEX_NAME, searchClient } from "@src/Typesense/instantsearch";
import { SearchFacetFilterGroup } from "@src/Typesense/Search/SearchFacetFilterGroup";
import { SearchHeader } from "@src/Typesense/Search/SearchHeader";
import { SearchHitCard } from "@src/Typesense/Search/SearchHitCard";

export const Route = createFileRoute("/searchsense")({
    component: RouteComponent,
});

const FILTER_GROUPS = [
    { title: "Game Type", attribute: "game_type" },
    { title: "Genres", attribute: "genres" },
    { title: "Themes", attribute: "themes" },
    { title: "Game Modes", attribute: "game_modes" },
    { title: "Multiplayer Modes", attribute: "multiplayer_modes" },
    { title: "Player Perspectives", attribute: "player_perspectives" },
] as const;

function RouteComponent() {
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
                initialUiState={{
                    [SEARCH_INDEX_NAME]: {
                        refinementList: {
                            game_type: ["Main Game"],
                        },
                    },
                }}
                future={{ preserveSharedStateOnUnmount: true }}
            >
                <Configure
                    hitsPerPage={24}
                    sortBy="_text_match:desc,aggregated_rating_count:desc,aggregated_rating:desc,first_release_date:desc"
                />

                <Grid className="typesense-layout">
                    <Card as="aside" variant="translucent" className="typesense-facet-panel" p="md">
                        <Heading size="sm" mb="sm">
                            Filters
                        </Heading>

                        <Accordion.Root multiple defaultIndex={[0, 1]}>
                            {FILTER_GROUPS.map((group, i) => (
                                <SearchFacetFilterGroup
                                    key={group.attribute}
                                    title={group.title}
                                    attribute={group.attribute}
                                    index={i}
                                />
                            ))}
                        </Accordion.Root>
                    </Card>

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

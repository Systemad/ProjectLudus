import { createFileRoute } from "@tanstack/react-router";
import {
    Accordion,
    Box,
    Button,
    CheckboxCardGroup,
    Flex,
    Grid,
    Heading,
    Image,
    Input,
    ScrollArea,
    Text,
} from "ui";
import {
    Configure,
    Pagination,
    InstantSearch,
    Stats,
    Hits,
    useSortBy,
    useRefinementList,
    useSearchBox,
} from "react-instantsearch";
import { useState } from "react";
import { useDebouncedCallback } from "@mantine/hooks";
import { searchClient } from "@src/Typesense/instantsearch";
import { SearchHeader } from "@src/Typesense/Search/SearchHeader";

export const Route = createFileRoute("/searchsense")({
    component: RouteComponent,
});

type GameSearchHit = {
    id: string;
    name?: string;
    cover_url?: string;
    aggregated_rating?: number;
    aggregated_rating_count?: number;
    developers?: string[] | string;
    release_year?: number;
    first_release_date?: number | string;
};

const FILTER_GROUPS = [
    { title: "Game Type", attribute: "game_type" },
    { title: "Genres", attribute: "genres" },
    { title: "Themes", attribute: "themes" },
    { title: "Game Modes", attribute: "game_modes" },
    { title: "Multiplayer Modes", attribute: "multiplayer_modes" },
    { title: "Player Perspectives", attribute: "player_perspectives" },
] as const;

const DEFAULT_SORT_INDEX = "games_to_typesense_dataset___games_search";
// All combinations pre-listed so useSortBy accepts every possible value.

function FacetFilterGroup({
    title,
    attribute,
    index,
}: {
    title: string;
    attribute: string;
    index: number;
}) {
    const { items, refine, canRefine, canToggleShowMore, isShowingMore, toggleShowMore } =
        useRefinementList({
            attribute,
            limit: 12,
            showMore: true,
            showMoreLimit: 30,
            sortBy: ["isRefined:desc", "count:desc", "name:asc"],
        });

    const selectedValues = items.filter((item) => item.isRefined).map((item) => item.value);

    return (
        <Accordion.Item button={title} index={index}>
            <Accordion.Panel px="xs" pb="sm">
                {canRefine ? (
                    <>
                        <ScrollArea maxH="sm" pr="xs">
                            <CheckboxCardGroup.Root
                                orientation="vertical"
                                size="sm"
                                value={selectedValues}
                                onChange={(values: string[]) => {
                                    const nextSet = new Set(values);

                                    items.forEach((item) => {
                                        const shouldBeRefined = nextSet.has(item.value);

                                        if (shouldBeRefined !== item.isRefined) {
                                            refine(item.value);
                                        }
                                    });
                                }}
                            >
                                {items.map((item) => (
                                    <CheckboxCardGroup.Item.Root
                                        key={`${attribute}:${item.label}`}
                                        value={item.value}
                                        flexDirection="row"
                                        alignItems="center"
                                        gap="xs"
                                        w="full"
                                    >
                                        <Text
                                            as="span"
                                            fontSize="sm"
                                            lineClamp={1}
                                            minW={0}
                                            flex="1"
                                        >
                                            {item.label}
                                        </Text>
                                        <Text
                                            as="span"
                                            fontSize="xs"
                                            color="fg.muted"
                                            whiteSpace="nowrap"
                                            ml="auto"
                                        >
                                            {item.count}
                                        </Text>
                                    </CheckboxCardGroup.Item.Root>
                                ))}
                            </CheckboxCardGroup.Root>
                        </ScrollArea>

                        {canToggleShowMore && (
                            <Button
                                size="xs"
                                variant="ghost"
                                mt="xs"
                                onClick={() => toggleShowMore()}
                            >
                                {isShowingMore ? "Show less" : "Show more"}
                            </Button>
                        )}
                    </>
                ) : (
                    <Text color="fg.muted" fontSize="sm">
                        No options found
                    </Text>
                )}
            </Accordion.Panel>
        </Accordion.Item>
    );
}

function getCoverImageUrl(coverUrl?: string) {
    if (!coverUrl) return "";
    if (coverUrl.startsWith("http")) return coverUrl;
    return `https://images.igdb.com/igdb/image/upload/t_cover_big/${coverUrl}.jpg`;
}

function getReleaseYear(hit: GameSearchHit) {
    if (typeof hit.release_year === "number") return String(hit.release_year);

    if (typeof hit.first_release_date === "number") {
        return String(new Date(hit.first_release_date * 1000).getUTCFullYear());
    }

    if (typeof hit.first_release_date === "string") {
        const parsed = new Date(hit.first_release_date);
        if (!Number.isNaN(parsed.getTime())) {
            return String(parsed.getUTCFullYear());
        }
    }

    return "Unknown year";
}

function getDevelopersLabel(developers?: string[] | string) {
    if (!developers) return "Unknown developer";
    if (Array.isArray(developers)) {
        if (developers.length === 0) return "Unknown developer";
        return developers.slice(0, 2).join(", ");
    }
    return developers;
}

function HitCard({ hit }: { hit: GameSearchHit }) {
    const imageUrl = getCoverImageUrl(hit.cover_url);
    const rating =
        typeof hit.aggregated_rating === "number" ? Math.round(hit.aggregated_rating) : null;

    return (
        <Box
            as="article"
            h="full"
            display="grid"
            gridTemplateRows="auto 1fr"
            gap="sm"
            p="sm"
            rounded="xl"
            borderWidth="1px"
            borderColor="whiteAlpha.300"
            bg="blackAlpha.500"
        >
            <Box aspectRatio="3/4" overflow="hidden" rounded="lg" bg="blackAlpha.400">
                {imageUrl ? (
                    <Image
                        src={imageUrl}
                        alt={hit.name ? `${hit.name} cover` : `Game cover`}
                        w="full"
                        h="full"
                        objectFit="cover"
                    />
                ) : (
                    <Flex w="full" h="full" align="center" justify="center">
                        <Text fontSize="sm" color="fg.muted">
                            No cover image
                        </Text>
                    </Flex>
                )}
            </Box>

            <Flex direction="column" gap="xs">
                <Heading size="sm" lineClamp={2} minH="2.75rem">
                    {hit.name ?? "Untitled game"}
                </Heading>

                <Text fontSize="sm" color="fg.muted">
                    {getDevelopersLabel(hit.developers)} - {getReleaseYear(hit)}
                </Text>

                <Text fontSize="sm" fontWeight="semibold">
                    {rating !== null
                        ? `Rating: ${rating}/100 (${hit.aggregated_rating_count ?? 0} votes)`
                        : "No rating yet"}
                </Text>
            </Flex>
        </Box>
    );
}

function RouteComponent() {
    return (
        <Box
            p="md"
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
                    marginBottom: "0.75rem",
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
                    border: "1px solid rgba(255,255,255,0.2)",
                    textDecoration: "none",
                    color: "inherit",
                    background: "rgba(255,255,255,0.03)",
                },
                ".typesense-pagination-item--selected .typesense-pagination-link": {
                    background: "rgba(93, 163, 255, 0.25)",
                    borderColor: "rgba(93, 163, 255, 0.6)",
                },
            }}
        >
            <InstantSearch
                searchClient={searchClient}
                indexName={DEFAULT_SORT_INDEX}
                initialUiState={{
                    [DEFAULT_SORT_INDEX]: {
                        refinementList: {
                            game_type: ["Main Game"],
                        },
                    },
                }}
            >
                <Configure
                    hitsPerPage={24}
                    sortBy="_text_match:desc,aggregated_rating_count:desc,aggregated_rating:desc,first_release_date:desc"
                />

                <Grid className="typesense-layout">
                    <Box
                        as="aside"
                        className="typesense-facet-panel"
                        rounded="xl"
                        borderWidth="1px"
                        borderColor="whiteAlpha.300"
                        bg="blackAlpha.500"
                        p="md"
                    >
                        <Heading size="sm" mb="sm">
                            Filters
                        </Heading>

                        <Accordion.Root multiple defaultIndex={[0, 1]}>
                            {FILTER_GROUPS.map((group, i) => (
                                <FacetFilterGroup
                                    key={group.attribute}
                                    title={group.title}
                                    attribute={group.attribute}
                                    index={i}
                                />
                            ))}
                        </Accordion.Root>
                    </Box>

                    <Box minW={0}>
                        <SearchHeader />

                        <Flex align="center" justify="space-between" mb="sm" gap="sm" wrap="wrap">
                            <Stats />
                        </Flex>

                        <Hits
                            hitComponent={HitCard}
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

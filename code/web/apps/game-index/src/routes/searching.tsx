"use client";

import { createFileRoute, stripSearchParams } from "@tanstack/react-router";
import { getApiTagsAllTagsQueryOptionsHook } from "../gen";
import z from "zod/v4";
import { SearchContainer } from "../Components/Search/SearchContainer";

const searchQueryParamsSchema2 = z.object({
    Name: z.string().optional(),
    Genres: z.array(z.string()).optional(),
    Themes: z.array(z.string()).optional(),
    GameModes: z.array(z.string()).optional(),
    Multiplayer: z.array(z.string()).optional(),
    Perspectives: z.array(z.string()).optional(),
    Page: z.number().int().catch(1).optional(),
    Limit: z.number().int().catch(40).optional(),
});

type SearchParams = z.infer<typeof searchQueryParamsSchema2>;

export const Route = createFileRoute("/searching")({
    component: RouteComponent,

    loader: ({ context }) => {
        const { queryClient } = context;

        // Prefetch without blocking route render; network issues should not blank the page.
        void queryClient.prefetchQuery(getApiTagsAllTagsQueryOptionsHook());
    },
    validateSearch: searchQueryParamsSchema2,
    search: {
        middlewares: [
            stripSearchParams<SearchParams>({
                Name: "",
                Genres: [],
                Themes: [],
                GameModes: [],
                Multiplayer: [],
                Perspectives: [],
                Page: 1,
                Limit: 40,
            }),
        ],
    },
});

function RouteComponent() {
    const search = Route.useSearch() ?? {};
    const navigate = Route.useNavigate();

    return (
        <SearchContainer
            contextId="search"
            initialSearch={search}
            onSearchChange={(s) => {
                navigate({
                    search: s,
                    resetScroll: false,
                });
            }}
        />
    );
}

import type { TypeOf } from "zod";
import type { searchQueryParamsSchema } from "../../gen";

export type FilterQueryKey = Extract<
    keyof TypeOf<typeof searchQueryParamsSchema>,
    "Genres" | "Themes" | "GameModes" | "Multiplayer" | "Perspectives"
>;

export const FILTER_GROUP_TO_QUERY_KEY: Record<string, FilterQueryKey> = {
    genres: "Genres",
    themes: "Themes",
    game_modes: "GameModes",
    multiplayer_modes: "Multiplayer",
    player_perspectives: "Perspectives",
};

export const FILTER_GROUP_ORDER = Object.keys(FILTER_GROUP_TO_QUERY_KEY);

export function toFilterGroupLabel(groupName: string): string {
    return groupName
        .split("_")
        .map((part) => part.charAt(0).toUpperCase() + part.slice(1))
        .join(" ");
}
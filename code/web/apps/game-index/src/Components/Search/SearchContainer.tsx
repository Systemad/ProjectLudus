import { useStore } from "@tanstack/react-store";
import {
    getSearchStore,
    type formSchema,
    type SearchState,
} from "./SearchStore";
import type { TypeOf } from "zod";
import { useEffect, useMemo, useRef } from "react";
import { useDebouncedState } from "@mantine/hooks";
import { useGetTagsAllTagsHook, useSearchHook } from "../../gen";
import { keepPreviousData } from "@tanstack/react-query";

export interface SearchContainerProps {
    contextId?: string;
    initialSearch?: Partial<TypeOf<typeof formSchema>>;
    fixedParams?: Partial<TypeOf<typeof formSchema>>;
    onSearchChange?: (search: TypeOf<typeof formSchema>) => void;
    title?: string;
}

const mergeSearchStates = (
    ...states: Partial<SearchState>[]
): Partial<SearchState> => {
    const result: Partial<SearchState> = {};
    for (const state of states) {
        for (const key in state) {
            const k = key as keyof SearchState;
            const resultValue = result[k];
            const stateValue = state[k];
            if (Array.isArray(resultValue) && Array.isArray(stateValue)) {
                (result as Record<keyof SearchState, unknown>)[k] = [
                    ...new Set([...resultValue, ...stateValue]),
                ];
            } else if (stateValue !== undefined) {
                (result as Record<keyof SearchState, unknown>)[k] = stateValue;
            }
        }
    }
    return result;
};

export function SearchContainer({
    contextId,
    initialSearch = {},
    fixedParams = {},
    onSearchChange,
    title = "Search",
}: SearchContainerProps) {
    const store = getSearchStore(
        contextId,
        mergeSearchStates(initialSearch, fixedParams)
    );

    const query = useStore(store, (state) => state) as TypeOf<
        typeof formSchema
    >;

    const setField: <K extends keyof TypeOf<typeof formSchema>>(
        field: K,
        value: TypeOf<typeof formSchema>[K]
    ) => void = (field, value) => {
        store.setState((prev) => {
            // If a filter changes (not the page itself), reset pagination to 1
            // TODO: EXPLORE WITH INFINTE QUERY? MAYBE NOT EVEN NEEDED?
            if (field !== "PageSize") {
                return {
                    ...prev,
                    [field]: value,
                    pageSize: 40,
                };
            }
            return {
                ...prev,
                [field]: value,
            };
        });
    };

    const prevSearchRef = useRef<TypeOf<typeof formSchema> | undefined>(
        undefined
    );

    // Track if component has been initialized to avoid overriding user changes
    const isInitializedRef = useRef(false);

    // Track if we should block onSearchChange during initial setup
    const blockOnSearchChangeRef = useRef(true);

    useEffect(() => {
        if (!isInitializedRef.current) {
            isInitializedRef.current = true;

            // Allow onSearchChange to run after initialization is complete
            setTimeout(() => {
                blockOnSearchChangeRef.current = false;
            }, 0);
        }
    }, []); // Empty dependency array - only run once on mount

    // SHOLD NOT BE NEEDED WITH REACT COMPILER
    const mergedQuery = useMemo(
        () => mergeSearchStates(query, fixedParams),
        [query, fixedParams]
    );
    const [debouncedQuery, setDebouncedQuery] = useDebouncedState(
        mergedQuery,
        300
    );

    const { data: results } = useSearchHook({
        params: {
            PageSize: debouncedQuery.PageSize ?? 40,
            Genres: debouncedQuery.genres,
            Themes: debouncedQuery.themes,
        },
    });
    const { data: tags } = useGetTagsAllTagsHook({
        query: {
            queryKey: ["all-tags"],
            placeholderData: keepPreviousData,
            refetchInterval: 1000 * 60 * 5,
        },
    });
}

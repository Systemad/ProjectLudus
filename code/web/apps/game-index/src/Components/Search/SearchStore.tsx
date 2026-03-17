import { Store } from "@tanstack/react-store";
import { z } from "zod";
import type { searchQueryParamsSchema } from "../../gen";
/*
export const formSchema = z.object({
    name: z.string().optional(),
    genres: z.array(z.string()).optional(),
    themes: z.array(z.string()).optional(),
    modes: z.array(z.string()).optional(),
    PageSize: z.number(),
});
*/
export type SearchState = z.infer<typeof searchQueryParamsSchema>;

const DEFAULT_PAGE_SIZE = 40;
const defaultState: SearchState = {
    Page: 1,
    Limit: DEFAULT_PAGE_SIZE,
};

export const searchStore = new Store<SearchState>(defaultState);

export class SearchStoreManager {
    private stores = new Map<string, Store<SearchState>>();

    getStore(contextId: string, initialState?: Partial<SearchState>) {
        if (!this.stores.has(contextId)) {
            this.stores.set(
                contextId,
                new Store<SearchState>({
                    ...defaultState,
                    ...initialState,
                })
            );
        }
        return this.stores.get(contextId)!;
    }

    removeStore(contextId: string) {
        this.stores.delete(contextId);
    }

    getActiveStoreIds(): string[] {
        return Array.from(this.stores.keys());
    }

    clearAll(): void {
        this.stores.clear();
    }
}

export const searchStoreManager = new SearchStoreManager();

export function getSearchStore(
    contextId?: string,
    initialState?: Partial<SearchState>
) {
    if (!contextId) {
        return searchStore;
    }
    return searchStoreManager.getStore(contextId, initialState);
}

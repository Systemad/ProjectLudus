type RouteState = Record<string, string | string[] | number | undefined>;

function serializeParams(routeState: RouteState, facetAttributes: readonly string[]): string {
    const params = new URLSearchParams();

    if (typeof routeState.q === "string" && routeState.q) {
        params.set("q", routeState.q);
    }
    if (typeof routeState.page === "number" && routeState.page > 1) {
        params.set("page", String(routeState.page));
    }
    if (typeof routeState.sort === "string" && routeState.sort) {
        params.set("sort", routeState.sort);
    }
    for (const attr of facetAttributes) {
        const vals = routeState[attr];
        if (Array.isArray(vals)) {
            for (const v of vals) params.append(attr, v);
        }
    }

    return params.toString();
}

function parseParams(search: string, facetAttributes: readonly string[]): RouteState {
    const params = new URLSearchParams(search);
    const state: RouteState = {};

    const q = params.get("q");
    if (q) state.q = q;

    const page = params.get("page");
    if (page) state.page = parseInt(page, 10);

    const sort = params.get("sort");
    if (sort) state.sort = sort;

    for (const attr of facetAttributes) {
        const vals = params.getAll(attr);
        if (vals.length) state[attr] = vals;
    }

    return state;
}

function strArraysEqual(a: string[] = [], b: string[] = []): boolean {
    return a.length === b.length && a.every((v, i) => v === b[i]);
}

function isPaginationOnly(
    prev: RouteState,
    next: RouteState,
    facetAttributes: readonly string[],
): boolean {
    const qSame = (prev.q ?? "") === (next.q ?? "");
    const sortSame = (prev.sort ?? "") === (next.sort ?? "");
    const facetsSame = facetAttributes.every((attr) =>
        strArraysEqual(
            (prev[attr] as string[] | undefined) ?? [],
            (next[attr] as string[] | undefined) ?? [],
        ),
    );
    return qSame && sortSame && facetsSame;
}

export function createSearchRouting(
    indexName: string,
    defaultSort: string,
    facetAttributes: readonly string[],
) {
    let _cb: ((route: RouteState) => void) | null = null;
    let _popstateHandler: (() => void) | null = null;

    const router = {
        read(): RouteState {
            return parseParams(window.location.search, facetAttributes);
        },
        write(routeState: RouteState) {
            const prev = parseParams(window.location.search, facetAttributes);
            const qs = serializeParams(routeState, facetAttributes);
            const url = `${window.location.pathname}${qs ? `?${qs}` : ""}`;
            const state = window.history.state ?? {};
            if (isPaginationOnly(prev, routeState, facetAttributes)) {
                window.history.pushState(state, "", url);
            } else {
                window.history.replaceState(state, "", url);
            }
        },
        createURL(routeState: RouteState): string {
            const qs = serializeParams(routeState, facetAttributes);
            return `${window.location.pathname}${qs ? `?${qs}` : ""}`;
        },
        onUpdate(cb: (route: RouteState) => void) {
            if (_popstateHandler) {
                window.removeEventListener("popstate", _popstateHandler);
            }
            _cb = cb;
            _popstateHandler = () => {
                cb(parseParams(window.location.search, facetAttributes));
            };
            window.addEventListener("popstate", _popstateHandler);
        },
        dispose() {
            if (_popstateHandler) {
                window.removeEventListener("popstate", _popstateHandler);
                _popstateHandler = null;
            }
            if (_cb) {
                _cb = null;
            }
        },
    };

    const stateMapping = {
        stateToRoute(uiState: any): RouteState {
            const indexState = (uiState[indexName] ?? {}) as Record<string, unknown>;
            const sortBy = indexState.sortBy as string | undefined;
            const refinementList = (indexState.refinementList ?? {}) as Record<string, string[]>;
            const routeState: RouteState = {};

            if (indexState.query) routeState.q = indexState.query as string;
            if (typeof indexState.page === "number" && indexState.page > 1) {
                routeState.page = indexState.page;
            }

            if (sortBy === indexName) {
                routeState.sort = "relevancy";
            } else if (sortBy) {
                const s = sortBy.replace(`${indexName}/sort/`, "");
                if (s !== defaultSort) routeState.sort = s;
            }

            for (const attr of facetAttributes) {
                if (refinementList[attr]?.length) {
                    routeState[attr] = refinementList[attr];
                }
            }

            return routeState;
        },
        routeToState(routeState: RouteState): any {
            const sort = routeState.sort as string | undefined;
            const effectiveSort = sort ?? defaultSort;
            const sortBy =
                effectiveSort === "relevancy" ? indexName : `${indexName}/sort/${effectiveSort}`;

            const refinementList: Record<string, string[]> = {};
            for (const attr of facetAttributes) {
                const vals = routeState[attr] as string[] | undefined;
                if (vals?.length) refinementList[attr] = vals;
            }

            return {
                [indexName]: {
                    query: (routeState.q as string) || "",
                    page: (routeState.page as number) || 1,
                    sortBy,
                    refinementList,
                },
            };
        },
    };

    return { router, stateMapping };
}

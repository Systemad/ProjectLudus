# Mobile catalog lists

## Goal

Replace the removed hardcoded category browser with a data-first Lists screen. It should bring the web index tables to mobile without adding a new API layer or duplicating catalog queries.

The first version contains three global lists:

- Trending: IGDB popscore, popularity type `9`.
- Most played: Steam chart type `most-played`.
- Hot releases: Steam chart type `hot-releases`.

`popular-releases` already exists in the web app and can be added as a fourth rail later without changing the structure.

## Existing data to reuse

No Catalog or Backend.API changes are required. Use the generated hooks that already back the web tables and the mobile Discover page:

```ts
const trending = useIgdbGetPopscore({
  query: { PopularityTypeId: "9", Page: 1, PageSize: 12 },
});

const mostPlayed = useSteamChart({
  query: { Type: "most-played", Page: 1, PageSize: 12 },
});

const hotReleases = useSteamChart({
  query: { Type: "hot-releases", Page: 1, PageSize: 12 },
});
```

The source endpoints are:

- `GET /catalog/igdb/popscore`
- `GET /catalog/steam/chart?type=most-played`
- `GET /catalog/steam/chart?type=hot-releases`

Do not add a mobile-specific endpoint, DTO, service, or query client. Kubb-generated hooks remain the only client boundary.

## Mobile structure

Keep the existing `/(browse)` route and tab label. Replace the temporary Browse placeholder with a feature slice:

```text
src/
├── app/(browse)/index.tsx
└── features/catalog-lists/
    ├── index.ts
    ├── catalog-lists-screen.tsx
    └── catalog-list-rail.tsx
```

`catalog-lists-screen.tsx` owns the three generated queries and renders one vertical section per list. `catalog-list-rail.tsx` owns the shared section header, retry/empty/loading state, and horizontal game carousel. Existing `entities/game/game-card.tsx` and `entities/game/game-carousel.tsx` remain the visual primitives.

Each game navigates through the existing Browse stack route:

```ts
const getGameHref = (game: GameBrowseDto) =>
  ({
    pathname: "/(browse)/games/[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;
```

The screen should use the existing safe-area/page-gutter conventions. The rail header keeps the current title-plus-chevron affordance, but its “view all” action should navigate to an existing collection route only when there is a real collection destination. If no route exists for a list yet, keep the header non-actionable instead of creating a fake route.

## Presentation

- Use a single vertical `ScrollView` for the Lists screen.
- Render each list as a horizontal, freeform `GameCarousel` with the current compact card treatment.
- Keep three visible cards on the standard device width; let the carousel scroll freely.
- Preserve image radius and game-card navigation already used by Discover and detail pages.
- Keep section spacing and page gutters consistent with Discover.
- Use inline XDS/Expo UI loading, empty, and retry states; do not hide a failed rail silently.
- No category icons, category slugs, filter sheet, or hardcoded category metadata remain.

## Query and state behavior

- Queries stay in TanStack Query through generated hooks.
- The Browse stack remains mounted while switching tabs, so list data and horizontal scroll state stay in memory.
- Query keys are produced by the generated hooks; do not construct or serialize keys manually.
- Use the existing finite first page for the rails. A future “view all” screen can reuse the generated infinite hooks and `GameGrid` without changing this screen.
- Keep query failures independent: one failed rail must not blank the other two.

## Cleanup already associated with this change

The removed dynamic-filter/category feature must stay absent:

- No `/catalog/games/filters` endpoint or `GetFiltersAsync` method.
- No `Catalog.Features.Games.Filters` DTOs or endpoint.
- No `/(browse)/categories/[slug]` route.
- No mobile category configuration, category cards, or category screen.
- No generated client regeneration for the removed endpoint.

## Implementation sequence

1. Add `features/catalog-lists` and move the temporary Browse screen to that feature.
2. Extract the shared rail rendering from the existing Discover rail only if it avoids duplication; otherwise reuse the existing rail and carousel directly.
3. Wire the three generated hooks and per-rail states.
4. Keep Browse navigation and game detail links on the existing Expo Router stack.
5. Add `popular-releases` only as a separate, explicitly approved fourth rail.
6. Run `pnpm run fmt` after the mobile edits, then `pnpm exec tsc --noEmit` and `pnpm run lint`.

## Verification

- Browse opens on the Lists screen, not a category grid.
- Trending, Most played, and Hot releases render from live API data.
- A failure in one request leaves the other rails usable and offers retry.
- Game cards open the existing detail route and return to Browse without creating a duplicate stack.
- Discover remains unchanged.
- No `/filters`, category route, or hardcoded category symbols are present in source.

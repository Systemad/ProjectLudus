# Mobile Typesense search

## Goal

Replace the mobile search placeholder with a native React Native search experience backed directly by the two existing Typesense collections:

- `games___games_search`
- `companies___company_search`

The CatalogAPI, Kubb generation, indexing pipeline, and event search remain out of scope.

## Current state

- `typesense-instantsearch-adapter@3.0.2`, `react-instantsearch-core@7.39.0`, and `@babel/runtime` are installed.
- `src/features/search/typesense-client.ts` exports separate game and company search clients.
- A client-safe Typesense key exists with only the `documents:search` action and only those two collections.
- The generated key is stored in the ignored mobile `.env`; the administrator key is never used by the app.
- `src/features/search/search-screen.tsx` still shows the placeholder and is the implementation target.

## Configuration

Keep the adapter configuration intentionally direct:

- Read `EXPO_PUBLIC_TYPESENSE_HOST`, `EXPO_PUBLIC_TYPESENSE_PORT`, `EXPO_PUBLIC_TYPESENSE_PROTOCOL`, `EXPO_PUBLIC_TYPESENSE_PATH`, and `EXPO_PUBLIC_TYPESENSE_SEARCH_API_KEY` directly from `process.env`.
- Keep collection names as constants in the search feature.
- Keep the two-minute adapter cache.
- Use the existing web query configuration for each index:
  - Games: `name,genres,themes,game_modes,multiplayer_modes,player_perspectives`, weighted toward `name`, sorted by `aggregated_rating:desc`.
  - Companies: `name,status`, weighted toward `name`, sorted by `games_published_count:desc`.
- Configure faceting for the indexed fields:
  - Games: `game_type`, `genres`, `themes`, `game_modes`, `multiplayer_modes`, and `player_perspectives`.
  - Companies: `status`.
- Never add the administrator key to `.env.example`, source files, or the mobile bundle.

## Search screen

### Search scope

Provide a native tab or segmented control with two options: Games and Companies. The selected scope determines which mounted InstantSearch tree and collection are visible. Events are not presented as a false search option until an indexed event collection exists.

Keep the two scopes independently stateful. Switching Games → Companies → Games must restore the previous query, selected facets, loaded pages, and scroll position for each scope. Do not use a global store; keep state in the mounted search feature and the InstantSearch cache.

### Search provider

For each scope, compose `InstantSearch` and `Configure` from `react-instantsearch-core` using the corresponding adapter client and index name. Enable shared-state preservation when a scope is temporarily unmounted. Configure 20 hits per page.

Use the core hooks only:

- `useSearchBox` for the controlled search field.
- `useInfiniteHits` for paginated results.
- `useInstantSearch` for loading, error, and stalled-search states.

Do not use DOM widgets from `react-instantsearch` or add a TanStack Query wrapper around Typesense.

### Facets

Filtering is sheet-based. Render one `Filters` action for the active scope with an active-refinement count; do not place facet controls inline in the result list.

Implement the filter surface with Expo UI’s drop-in BottomSheet:

- Import `BottomSheetModal` and `BottomSheetView` from `@expo/ui/community/bottom-sheet`.
- Keep the sheet closed by default and call `present()` from the Filters action.
- Use `snapPoints={['50%', '90%']}` and `enablePanDownToClose`.
- Put the facet controls in a scrollable sheet body and close it with the native dismiss/back behavior.
- Keep this as the cross-platform Expo UI component; do not add `@gorhom/bottom-sheet`.

- Games expose refinements for game type, genres, themes, game modes, multiplayer modes, and player perspectives.
- Companies expose a status refinement.
- Use `useRefinementList` and `useClearRefinements` from `react-instantsearch-core`.
- Refinements must be represented by Typesense `facet_by` and `facetFilters` through InstantSearch; do not manually filter returned hits in React.
- Show active refinement count and provide a one-action clear-all control.
- Preserve refinements when switching scopes and when navigating into a detail route and back.

### Native UI

- Use the Expo UI native text input for the search field and the Expo UI native scope control available in the installed SDK.
- Keep the large result set in a React Native `FlatList`; do not use Expo UI `List` for this collection.
- Keep the screen inside the existing page gutter and theme system.
- Preserve the existing Expo Router search tab so the mounted tab stack retains search state when navigating away and back.

### Results

Define app-owned hit types for only fields rendered by the UI.

Game hits:

- `id`
- `name`
- `cover_url`
- `release_year`
- `genres`
- `developers`
- `aggregated_rating`

Company hits:

- `id`
- `name`
- `logo_url`
- `start_year`
- `status`
- `games_developed_count`
- `games_published_count`

Render games using the shared game-card/entity conventions and IGDB image URL utility. Render companies using the existing company presentation, including `logo_url` when available. Use a compact two-column result grid for games and a dense single-column list or card grid for companies, based on the existing mobile entity patterns.

Selecting a game or company must use the existing Expo Router detail routes and pass the indexed identifier in the route’s established parameter format. Do not invent a new route or duplicate generated API models.

## States

- Initial loading: use the existing mobile loading treatment.
- Pagination: show a footer activity indicator while `showMore` is pending.
- Empty query: show the default ranked collection, not an empty state.
- Non-empty query with zero hits: show `EmptyState` with a clear no-results message.
- Network or Typesense failure: show a retryable error state without logging the API key.
- Missing configuration: fail with a concise configuration error at the search feature boundary; never print key values.

## Interaction and performance

- Debounce text input through InstantSearch’s normal search behavior; do not create a second debounce or request layer.
- Call `showMore` from `onEndReached` only when more pages exist and no request is active.
- De-duplicate rendered hits by stable indexed id.
- Preserve keyboard dismissal and touch targets on both Android and iOS.
- Respect reduced-motion preferences for result transitions.

## Validation

Static:

- `pnpm exec tsc --noEmit`
- `pnpm run lint`

Functional on a requested device:

- Search partial game names, genres, and themes.
- Search company names and statuses.
- Switch Games → Companies → Games and confirm each scope restores its query and scroll position.
- Clear a query and confirm ranked results return.
- Scroll through multiple pages without duplicates.
- Verify no-results, offline, and invalid-configuration states.
- Open a game and company result and confirm the existing detail routes.

Do not run Aspire or launch the mobile app during implementation unless explicitly requested. Device verification is a separate step using `agent-device`.

## Non-goals

- No new CatalogAPI search endpoint.
- No changes to Kubb-generated files.
- No new Typesense collections or indexing changes.
- No analytics, URL query synchronization, offline persistence, or event search in this iteration.
- No administrator credentials in the mobile bundle.

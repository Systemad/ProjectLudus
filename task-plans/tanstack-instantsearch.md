# TanStack Router × react-instantsearch URL sync

## Goal

Persist search state (query, facets, pagination, sort) to TanStack Router URL search params so that:
- Browser back/forward restores search state
- URLs can be shared with specific results
- Page refresh preserves state

## Approach

Create a custom react-instantsearch `routing.router` that bridges `InstantSearch`'s internal state with TanStack Router's `validateSearch` + `navigate()`.

## Implementation

### 1. Route definition (`routes/games/search.tsx`)

```tsx
import { z } from "zod";

const searchSchema = z.object({
  q: z.string().catch(""),
  page: z.coerce.number().catch(1),
  sort: z.string().catch("aggregated_rating:desc"),
  genres: z.array(z.string()).catch([]),
  themes: z.array(z.string()).catch([]),
  gameModes: z.array(z.string()).catch([]),
  multiplayerModes: z.array(z.string()).catch([]),
  playerPerspectives: z.array(z.string()).catch([]),
  gameType: z.array(z.string()).catch([]),
});

export const Route = createFileRoute("/games/search")({
  validateSearch: searchSchema,
  component: RouteComponent,
});
```

### 2. Custom router bridge (`Typesense/tanstackRouter.ts`)

react-instantsearch's `routing.router` interface:

```ts
interface Router {
  read(): RouteState;
  write(routeState: RouteState): void;
  onUpdate(cb: (routeState: RouteState) => void): void;
  dispose(): void;
}
```

- `read()` — converts TanStack Router search params to InstantSearch route state
- `write(routeState)` — calls `navigate({ search: ..., replace: true })`
- `onUpdate()` / `dispose()` — lifecycle management

### 3. Infinite loop prevention

Use a `ref` for the current search params so the router object is stable (not recreated on every URL change):

```tsx
const searchRef = useRef(search);
searchRef.current = search;

const router = useMemo(() => createRouter(searchRef, navigate), [navigate]);
```

### 4. Replace, not push

All URL writes use `replace: true` to avoid bloating browser history (InstantSearch debounces writes internally).

### 5. URL param to InstantSearch mapping

| URL param | InstantSearch route state key |
|---|---|
| `q` | `state.query` |
| `page` | `state.page` |
| `sort` | `state.sort` |
| `genres` | `state.refinementList.genres` |
| `themes` | `state.refinementList.themes` |
| `gameModes` | `state.refinementList.game_modes` |
| `multiplayerModes` | `state.refinementList.multiplayer_modes` |
| `playerPerspectives` | `state.refinementList.player_perspectives` |
| `gameType` | `state.refinementList.game_type` |

## Files to create/modify

- `src/routes/games/search.tsx` — add `validateSearch`, wire router
- `src/Typesense/tanstackRouter.ts` (new) — `createTanStackRouter(searchRef, navigate)` factory

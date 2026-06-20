# Reactive Steam Data Pipeline

## Goal
Capped `steam.tracked_games` from popscores works for popular games, but we need on-demand fetching for less-popular games users actually browse.

## Three Layers

### Layer 1 — IGDB popscores (baseline)
- `steam-game-index` runs weekly
- Keeps top ~1500 games in `steam.tracked_games` via popscores
- Sets the floor — always have data for popular titles

### Layer 2 — Frontend triggers (on-demand)
When a user visits a game page, CatalogAPI checks if Steam data exists/is fresh. If not:

**Option A — Request table (simpler)**
- New table `steam.pending_refreshes(game_id, steam_app_id, requested_at)`
- CatalogAPI inserts on page view if game not tracked or data stale
- A periodic flow picks up pending games, fetches pricing/details/CCU in batch
- After fetch, removes from pending

**Option B — Direct Prefect API call**
- CatalogAPI calls Prefect's REST API to trigger a single-game flow
- Requires Prefect API key/auth from CatalogAPI
- More complex but reactive in real-time

**Recommendation:** Option A — simpler, batched, no auth complexity.

### Layer 3 — Umami analytics (reactive)
- New flow that periodically queries Umami for top game pages by pageviews
- Cross-references with `steam.tracked_games`
- Adds games with significant traffic that aren't tracked yet
- Also flags tracked games with recent traffic spikes for priority refresh

## New Flows Needed

| Flow | Schedule | Source | Destination |
|---|---|---|---|
| `steam-request-queue` | Every 15 min | `steam.pending_refreshes` | Fetches pricing + details + CCU |
| `umami-game-discovery` | Daily | Umami API | `steam.tracked_games` (inserts new) |

## Questions
1. Option A or B for frontend triggers?
2. For `steam-request-queue`: fetch each game individually (slow but gentle on Steam API) or batch all pending at once?
3. Umami flow — should it also trigger immediate Steam fetches for high-traffic games, or just add to tracked_games and let the regular schedule handle it?

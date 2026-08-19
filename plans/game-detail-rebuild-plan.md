# Game detail rebuild plan

Status: planning only. No production implementation is included in this plan.

Source material:

- \`mobile-layout-explorations.html\`, Design 1 — CS2 detail.
- Uploaded game-detail GIF: 96 frames, 6.4 seconds, reviewed at multiple scroll positions.
- Current Android game-detail implementation and its Compose/React Native layout boundaries.
- Expo safe-area guidance: <https://docs.expo.dev/develop/user-interface/safe-areas/>

## Goal

Replace the current game-detail page with one coherent Android-native screen composition using Expo UI Jetpack Compose components. React Native should only bridge content that genuinely requires it, such as expo-image and the chart.

The page must match the reference hierarchy, preserve the existing game data, keep routes thin, and avoid per-section layout hacks, duplicated color props, and independently measured native islands.

## What is wrong with the current architecture

The current page combines a React Native ScrollView with several independent Compose Host trees:

~~~text
React Native ScrollView
├── Compose header Host
├── React Native hero and record
├── Compose action Host
├── Compose summary Host
├── Compose Steam Host
├── Compose metadata Host
├── React Native chart
└── Compose related-games Host
~~~

This makes every section measure itself independently. The resulting problems are structural:

- vertical gaps appear between unrelated native hosts;
- page insets are applied by multiple parents;
- card content padding is confused with card surface modifiers;
- dividers can measure at zero size;
- hero, cover, and title do not share one layout boundary;
- the page has more than one styling system;
- chart state and page navigation can interfere with one another;
- Android and universal Steam summary implementations duplicate presentation logic.

The rebuild will replace this with one vertical native composition tree.

## Target hierarchy

~~~text
SafeAreaView (top boundary for the custom header)
└── Host
    └── Column (single vertical scroll container)
        ├── GameDetailTopBar
        ├── GameDetailTabs
        ├── GameDetailMedia
        │   ├── hero image
        │   ├── hero scrim
        │   ├── metadata chips
        │   ├── overlapping cover
        │   └── title metadata
        ├── GameDetailActions
        ├── SummaryCard
        ├── SteamNowSection
        ├── RatingsAndPriceSection
        ├── GameInformationSection
        ├── SteamPlayersSection
        └── RelatedGamesSection
~~~

There will be one authoritative Android layout for the detail page. The route remains a thin entry point.

## Visual wireframe

These are target relationships and measurements, not arbitrary per-component style values:

~~~text
┌──────────────────────────────────────┐
│              top safe area            │
├──────────────────────────────────────┤
│  ‹        Counter-Strike 2        ⋮   │  57dp top bar
├──────────────────────────────────────┤
│       Overview      Media      Links  │
│       ━━━━━━━                         │  compact active indicator
├──────────────────────────────────────┤
│        ┌──────────────────────┐      │
│        │      HERO IMAGE       │      │  222dp
│        │                 2012 │      │
│        │             Steam FPS│      │
│        └──────────────────────┘      │
│              ┌─────────┐             │
│              │  COVER  │ Counter-Strike 2
│              │         │ Valve · multiplayer shooter
│              └─────────┘             │
├──────────────────────────────────────┤
│  Wishlist  Save  Track players  Share │
├──────────────────────────────────────┤
│  ┌────────────────────────────────┐  │
│  │ Summary                        │  │
│  │ compact body text              │  │
│  │             Read more          │  │
│  └────────────────────────────────┘  │
├──────────────────────────────────────┤
│  Steam now                    Open Steam
│  ┌────────────┬────────────┬────────────┐
│  │ Playing    │ 24h peak   │ Steam ID   │
│  └────────────┴────────────┴────────────┘
├──────────────────────────────────────┤
│  Ratings and price                   │
│  ┌──────────────────┬──────────────────┐
│  │ Steam reviews    │ Total reviews    │
│  ├──────────────────┼──────────────────┤
│  │ Price            │ IGDB rating      │
│  └──────────────────┴──────────────────┘
├──────────────────────────────────────┤
│  Game information             All details
│  ┌──────────────────┬──────────────────┐
│  │ Platforms        │ Genres           │
│  ├──────────────────┼──────────────────┤
│  │ Game modes       │ Perspective      │
│  ├──────────────────┼──────────────────┤
│  │ Themes           │ Companies         │
│  └──────────────────┴──────────────────┘
├──────────────────────────────────────┤
│  Steam players                       │
│  ┌──────────────────────────────────┐ │
│  │ 24H       7D       30D            │ │
│  │              chart               │ │
│  └──────────────────────────────────┘ │
├──────────────────────────────────────┤
│  Related games                       │
│  [Dota 2] [PUBG] [Apex Legends]       │
└──────────────────────────────────────┘
~~~

## Layout rules

### Global

- One vertical scroll container owns the whole detail page.
- One shared horizontal content inset is used by headings, cards, and rails.
- Sections use a small, consistent vertical rhythm.
- No component adds an unrelated bottom spacer.
- The bottom safe area is handled by the screen/container boundary.
- The native tab bar must not appear as an extra detail-page control.

### Header and tabs

- Detail routes explicitly hide the stack header.
- The custom top bar is exactly one row: back, centered title, overflow.
- Settings is not rendered on the detail page.
- Tabs appear directly below the top bar: Overview, Media, Links.
- Tab state stays local to the detail screen and must not alter route navigation.

### Media and record

- Hero image is an inset banner with a fixed target height of 222dp.
- Hero and cover use one media layout boundary.
- Cover overlaps the lower edge of the hero by approximately 30dp.
- Chips sit at the hero lower-right edge.
- Record metadata is a fixed two-column layout: approximately 96dp cover plus title content.
- Title and publisher/genre metadata align with the bottom of the cover record.

### Actions

- Four equal-width action surfaces are always rendered.
- Labels remain short enough to fit without truncation.
- Authentication-required actions route to profile when signed out.
- The signed-out state must not replace the action row with one full-width button.

### Summary

- Summary is a Material card surface, not loose screen text.
- Card content uses an inner full-width padded column.
- Body text is collapsed to a compact preview.
- Read more is centered inside the card.
- Expansion stays inline and does not create an overlay or route.

### Metadata grids

MetadataGrid is the only reusable grid primitive for these sections:

~~~text
MetadataGrid(columns: 3)  -> Steam now
MetadataGrid(columns: 2)  -> Ratings and price
MetadataGrid(columns: 2)  -> Game information
~~~

Rules:

- equal-width columns;
- row-major item order;
- odd final rows leave the remaining side empty;
- vertical divider only exists between two real cells;
- horizontal divider exists between every pair of real rows;
- cell padding belongs to cell content, not the card surface modifier;
- card inset is applied once by the grid boundary;
- no section-specific duplicate grid implementation.

### Steam players

- The range control belongs to the chart section.
- Selecting 24H, 7D, or 30D only changes chart state.
- It must never navigate to Most Played or another route.
- Peak and average have visibly distinct legend entries.
- Tooltip state is owned by the chart and can be dismissed by tapping outside it.
- Date labels use the selected interval’s actual data bounds and readable 24-hour formatting.

### Related games

- Related games are a horizontal rail of compact pills.
- The rail has one shared page inset.
- It does not create a second vertical list or grid.

## Data and view-model boundary

Create one screen model hook responsible for logic and derived presentation data:

~~~text
useGameDetailScreen(gameId)
├── core game queries
├── Steam/reviews/pricing queries
├── screenshots and links
├── related games
├── active tab state
├── summary expansion state
├── chart range state
├── action handlers
└── section-ready/error/empty states
~~~

The Android renderer receives already-shaped values:

~~~text
GameDetailScreen
├── header model
├── media model
├── action model
├── summary model
├── Steam-now items
├── ratings items
├── game-information items
├── chart model
└── related games
~~~

The renderer should not contain query selection, authentication branching, API nullability decisions, or navigation decisions mixed into JSX.

## Component ownership

Proposed boundaries:

~~~text
src/app/(discover)/games/[slug].tsx       thin route
src/app/(browse)/games/[slug].tsx         thin route
src/app/(search)/games/[slug].tsx         thin route

src/features/game-detail/
├── use-game-detail-screen.ts             logic and view model
├── game-detail-screen.android.tsx        one Compose screen tree
├── game-detail-media.android.tsx         hero/cover/image bridge
├── game-detail-header.android.tsx        top bar and tabs
├── game-detail-actions.android.tsx       four action surfaces
├── metadata-grid.android.tsx             shared configurable grid
├── steam-players-section.android.tsx     chart section boundary
└── related-games-section.android.tsx     horizontal rail
~~~

Existing components that duplicate these responsibilities should be replaced or removed after the new tree works. The goal is fewer authoritative layout boundaries, not more wrappers.

## Material and color policy

- Compose text uses default Material content color.
- Compose cards use default Material surface and shape tokens.
- Compose dividers use component default tokens.
- No raw hex colors in the new detail implementation.
- No color props passed through every component.
- React Native bridges may receive dynamic Material tokens only when they cannot inherit Compose theme values, such as chart strokes or image fallback surfaces.
- No manual radius values unless the native component cannot provide the required shape.

## Safe-area policy

- Add SafeAreaProvider once at the app root.
- Let React Navigation handle safe areas for normal stack/tab screens.
- The custom detail screen owns only the top inset because it replaces the stack header.
- Do not call useSafeAreaInsets in every section.
- Do not add arbitrary bottom padding to compensate for multiple navigation layers.

## Implementation sequence

1. Freeze the current detail-page changes as the baseline; do not add more local patches.
2. Define the screen model hook and remove presentation logic from the route component.
3. Build one Android Compose scroll tree with placeholder section blocks.
4. Implement the top bar, tabs, and safe-area boundary.
5. Implement the combined media/record block.
6. Implement the four equal action surfaces.
7. Implement the summary card with real internal padding and centered action.
8. Implement MetadataGrid and use it for all three data sections.
9. Reattach Steam data, chart, media, links, and related-game behavior.
10. Remove superseded detail layout components.
11. Run focused formatting, typechecking, linting, and diff checks.
12. Verify top, middle, and bottom scroll states on the Android device.

## Verification checklist

- [ ] No stack header is visible on game detail.
- [ ] No settings gear appears on game detail.
- [ ] Top bar and tabs appear exactly once.
- [ ] Hero, cover, and record metadata share one layout boundary.
- [ ] Hero is 222dp target height and inset.
- [ ] Chips sit over the hero at bottom-right.
- [ ] Cover overlaps the hero.
- [ ] Four action surfaces have equal widths.
- [ ] Summary card has internal padding.
- [ ] Read more is centered.
- [ ] Steam now is a three-column grid.
- [ ] Ratings and price is a two-column grid with one horizontal divider.
- [ ] Game information is a two-column grid with all row dividers.
- [ ] No unexplained gap appears before or after Platforms.
- [ ] Chart range buttons do not navigate.
- [ ] Chart tooltip dismisses correctly.
- [ ] Related games are horizontal pills.
- [ ] No raw color values are introduced in the new Compose implementation.
- [ ] No generated files or OpenAPI schemas are modified.
- [ ] Normal Metro reload is sufficient after the initial runtime is healthy.

## Explicit non-goals

- No backend/API redesign in this screen rewrite.
- No generated-file edits.
- No OpenAPI schema edits.
- No new design-system package.
- No speculative abstractions beyond the screen model, layout boundary, and shared metadata grid.
- No requirement to clear Metro after every source edit.


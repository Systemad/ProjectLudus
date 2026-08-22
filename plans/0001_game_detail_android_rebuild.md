# 0001 — Universal Expo game-detail rebuild, Android-first

Status: revised planning only. The implementation direction is now universal React Native/Expo for iOS and Android, aligned with the Expo SDK 57 documentation, with Android as the primary design, build, and device-validation target.

## Objective

Rebuild the game-detail route as one compact, vertically scrollable React Native/Expo screen that works on both iOS and Android. Android receives priority for visual tuning, native behavior, and physical-device validation because it is the available test platform.

The implementation should use ordinary React Native and Expo primitives for the page shell, media, scrolling, and composition. Use universal `@expo/ui` components for native controls and grouped UI where they fit. Android-only Compose code is not the foundation of this screen and must not force a separate renderer.

The result must:

- match the reference game-detail hierarchy;
- keep Overview/Media/Links as local screen state;
- keep one vertical scroll owner;
- prevent vertical detail scrolling from changing Browse or Trending state;
- provide safe-area-correct content below one stable native navigation header;
- render useful loading, empty, and error states instead of blank reserved space;
- preserve a code path that can run on iOS even though no iOS device is available for validation.

## Scope and implementation stance

### In scope

- Expo Router route ownership and canonical `/games/[slug]` navigation;
- React Native/Expo page layout using `View`, `Text`, `Pressable`, `ScrollView`, `Animated.ScrollView`, and `expo-image`;
- universal `@expo/ui` controls where they provide a native equivalent;
- Android-first Material-oriented spacing, surfaces, controls, typography, and interaction;
- Android-first native navigation and content behavior;
- focused Android build and physical-device validation;
- Expo Router native navigation, Native Tabs, safe-area APIs, semantic colors, and Material 3 dynamic-color handling;
- existing game-detail data, actions, Steam sections, media, links, and related games.

### Out of scope

- backend or OpenAPI changes;
- generated Kubb changes;
- a separate iOS implementation or a separate Android Compose renderer;
- requiring an iOS device or simulator to validate the feature;
- replacing the app-wide design system;
- speculative abstraction of unrelated screens.

Universal components are an implementation choice, not a platform target. Android-first means Android is tuned and tested first; it does not mean the page should import Android-only UI APIs or abandon the iOS-compatible React Native path.

## Reference material

Primary design evidence:

- `mobile-layout-explorations.html`, Design 1 — CS2 detail;
- uploaded game-detail GIF and annotated reference image;
- existing Android captures under `code/app/mobile/agent-device-output/`.

Animated-header reference (evaluated, then intentionally rejected for this screen):

- [Stump `useAnimatedHeader.ts`](https://github.com/stumpapp/stump/blob/main/apps/expo/components/header/useAnimatedHeader.ts).

Expo SDK 57 documentation used for the navigation/UI decisions:

- [Expo Router introduction](https://docs.expo.dev/router/introduction/);
- [Native Tabs](https://docs.expo.dev/router/advanced/native-tabs/);
- [Stack](https://docs.expo.dev/router/advanced/stack/);
- [Safe areas](https://docs.expo.dev/develop/user-interface/safe-areas/);
- [Expo Router `Color`](https://docs.expo.dev/versions/v57.0.0/sdk/router/color/);
- [Universal `@expo/ui`](https://docs.expo.dev/versions/v57.0.0/sdk/ui/universal/);
- [Jetpack Compose `@expo/ui`](https://docs.expo.dev/versions/v57.0.0/sdk/ui/jetpack-compose/);
- [Experimental Stack](https://docs.expo.dev/versions/v57.0.0/sdk/router/experimental-stack/).

The useful parts of that pattern are:

- `useSafeAreaInsets` remains useful for bottom gesture-area spacing;
- the animated gradient added visual weight without improving hierarchy or readability;
- the stable native Stack header is now the sole detail-header owner;
- no header-only animation or gradient dependency is required by the detail route.

## Expo architecture decisions

### Expo Router owns navigation

Expo Router remains the app’s file-based navigation layer. Route files stay thin, layouts own navigators, and links use canonical route paths. The game-detail screen must not create a second navigation system inside its content.

Use the standard `Stack` as the production baseline for the root navigator and detail screen. It supports the header configuration needed by this plan and keeps the implementation inside the stable SDK 57 path.

### Native Tabs own only the tab shell

Use `NativeTabs` from `expo-router/unstable-native-tabs` only inside `src/app/(tabs)/_layout.tsx`. It should own Discover, Browse, Search, and Profile on native platforms. The root `src/app/_layout.tsx` owns the parent stack, and `games/[slug]` is pushed outside the Native Tabs navigator.

Configure native tab labels and icons using the platform-aware Native Tabs API, including Android Material icon names and iOS SF Symbol names where supported. Do not render a custom React Native tab bar for the native app path. Validate Android tab selection, reselect/scroll-to-top behavior, and the absence of the tab bar on detail.

### Safe areas are explicit, not guessed

Use `useSafeAreaInsets` from `react-native-safe-area-context` for bottom gesture-area spacing and measured content placement. The detail route uses an opaque native Stack header, so it must not add a second status-bar/header spacer. Do not use React Native’s deprecated `SafeAreaView` or add arbitrary top spacers.

The detail screen must avoid double-applying the top inset: the native Stack header owns the top navigation area and the screen owns only content/bottom spacing. Verify the final result on Android with status-bar, gesture-navigation, and cutout configurations when validation resumes.

### Universal `@expo/ui` is the shared native layer

Use the universal package root for shared native controls:

```text
@expo/ui
├── Host
├── Button / Switch / Checkbox / Slider / Picker
├── BottomSheet / Collapsible
├── List / ListItem / FieldGroup
└── Row / Column / Text / Icon where a native subtree is useful
```

Every universal subtree is wrapped in the root `Host` imported from `@expo/ui`. The same tree then delegates to Jetpack Compose on Android and SwiftUI on iOS. Do not import `@expo/ui/jetpack-compose` or `@expo/ui/swift-ui` for the core screen merely to obtain layout containers.

The page shell remains React Native/Expo because it needs predictable scrolling, image measurement, Reanimated integration, and bespoke game-detail geometry. Before introducing a community or React Native control, check whether the universal `@expo/ui` API provides the required native control. Keep `Pressable` for bespoke action cells and local detail tabs when their layout cannot be expressed as a standard native control.

### Compose UI is an intentional escape hatch

Android-only `@expo/ui/jetpack-compose` is allowed only for a specific Android capability that the universal layer does not expose and that materially improves the Android experience. Such a subtree must be isolated behind a component boundary and must have a React Native/iOS-safe fallback or alternate implementation.

It is not allowed as the renderer for the entire detail page. Any Compose-specific addition must document:

1. the missing universal API;
2. why the Android behavior is materially required;
3. the iOS-compatible fallback;
4. the additional build/runtime validation it requires.

### Material 3 dynamic colors are theme inputs

Centralize colors in a theme module using `Color` from `expo-router`:

- Android primary palette: `Color.android.dynamic.*` for Android 12+ wallpaper-derived Material 3 colors;
- Android fallback: `Color.android.material.*` for static Material 3 roles on older Android versions or when a stable fallback is needed;
- iOS fallback: `Color.ios.*` semantic system colors;
- web/default fallback only where the shared code path requires it.

Components that render these colors must call `useColorScheme()` so light/dark changes cause a render update. Use Material 3 roles such as `surface`, `surfaceContainer`, `onSurface`, `onSurfaceVariant`, `primary`, `primaryContainer`, `outline`, and `outlineVariant` instead of scattered hex values. Reanimated styles must receive resolved static colors where required by the animation API; do not pass opaque platform color objects into worklet styles without verifying support.

For universal `@expo/ui` subtrees, use the `Host` color-scheme support where needed and keep the surrounding React Native theme tokens aligned with the same light/dark intent. The Android dynamic palette is a styling priority, not permission to create an Android-only page renderer.

### Experimental Stack is a gated evaluation, not the default

`ExperimentalStack` is available in SDK 57 and includes Android predictive-back support, but Expo documents it as alpha/testing-only. It currently supports only `title`, `headerShown`, `headerTransparent`, and `headerBackVisible`; it does not support the full header, modal, sheet, custom-header, tint, or status-bar options this plan may need. It also cannot coexist with standard `Stack` on Android.

Therefore:

- Phase 0 may run a small Android-only compatibility spike against the actual app config;
- the spike may evaluate predictive back and transparent-header behavior;
- the production plan remains standard `Stack` unless the entire native stack can migrate safely and the required options are confirmed;
- do not mix `ExperimentalStack` and `Stack` in the app’s Android navigator tree;
- if adopted later, enable `android.predictiveBackGestureEnabled` and update the acceptance/validation matrix explicitly.

The feature should not be delayed for Experimental Stack. Android back behavior must work correctly with the stable Stack path first.

## Current defects to preserve as acceptance targets

### Media failure creates a void

Hero and cover images currently use a Compose/RN bridge arrangement that can leave a large black region when an image is missing or fails. The new screen uses `expo-image` directly in React Native layout and makes image state explicit:

- missing URL → compact placeholder;
- loading → compact loading treatment inside the measured media boundary;
- load error → compact error/fallback treatment;
- loaded → artwork with the intended aspect ratio and shape;
- no image state may reserve an unexplained hero-sized blank region.

### Media and title are detached

Hero, chips, cover, title, studio, and genre must be one React Native composition. The cover overlaps the hero boundary without relying on a failed-image spacer or unmeasured native child.

### Actions are loose icons instead of surfaces

Keep four equal action cells visible for both signed-in and signed-out users:

- Wishlist;
- Save to list;
- Track players;
- Share.

Authentication changes the action behavior, not the existence or layout of the row.

### Detail inherits the tab shell

The canonical detail route must be a root-stack sibling of the NativeTabs group. The bottom tab bar must not be visible while detail is open, and the detail scroll owner must not mutate Browse/Trending selection.

### Header ownership is duplicated

The detail route must have exactly one navigation header. Settings must not appear behind or beside the detail header. Profile remains the owner of settings access.

### Spacing and typography are oversized

Use a compact page rhythm, explicit content padding, semantic typography, and measured media boundaries. Do not add spacer values to compensate for headers, missing images, nested hosts, or unmeasured children.

## Target route hierarchy

The route tree must have one tab group and one canonical detail route:

```text
src/app/_layout.tsx
└── Root Stack
    ├── (tabs)
    │   ├── (discover)
    │   ├── (browse)
    │   ├── (search)
    │   └── profile
    └── games/[slug]
        └── GameDetailRoute
```

Rules:

- `NativeTabs` is mounted only in `src/app/(tabs)/_layout.tsx`;
- `src/app/_layout.tsx` owns the root `Stack`;
- `src/app/games/[slug].tsx` is the only game-detail route;
- the three old group-local `games/[slug].tsx` files are removed after links are migrated;
- the detail route has no inherited header action or bottom tab bar;
- the detail route keeps its own header configuration and does not navigate Overview/Media/Links through Expo Router;
- all game links, including Browse, Discover, Search, history, lists, company pages, event pages, and related games, resolve to `/games/[slug]`.

The interrupted implementation pass already moved part of the route tree. Before production work continues, compare the current worktree to this hierarchy and finish or safely reconcile that move without overwriting unrelated user changes.

## Header and scroll architecture

Use the root Stack header as the sole navigation owner for game detail:

- opaque native header with the screen background color;
- no default header shadow;
- native back affordance;
- title from the loaded game name;
- native share action where it is safe;
- no settings action;
- no custom or animated second header rendered inside the page.

The detail body uses one ordinary React Native `ScrollView`. Its content starts with compact padding below the native Stack header and adds only bottom safe-area spacing. Reanimated remains available for other app interactions, but the detail header does not depend on a scroll worklet or gradient package.

## Universal UI strategy

Use normal React Native/Expo components for the page body:

- `Animated.ScrollView` for the single vertical scroll owner;
- `View` for layout, cards, overlays, and measured media boundaries;
- `Text` for copy and metadata;
- `Pressable` for bespoke action surfaces and local tabs;
- `expo-image` for hero, cover, screenshots, and artwork;
- `StyleSheet` or shared inline styles for reusable styling;
- `react-native-safe-area-context` for inset-aware placement.

Consult `@expo/ui` before choosing a native control. Prefer universal components from the package root when the need matches:

- `Button` for standard action buttons;
- `List` and `ListItem` for short, fixed native rows;
- `BottomSheet` for list selection/action sheets;
- `Switch`, `Checkbox`, `Slider`, `Picker`, and `Collapsible` where applicable;
- `Host` around each universal `@expo/ui` subtree.

Use React Native `Pressable` for the four bespoke action surfaces and local Overview/Media/Links tabs when the universal component cannot express the required layout. Do not import Android-only `@expo/ui/jetpack-compose` primitives into the core screen merely to obtain layout containers.

Android priority means:

- tune spacing, touch targets, typography, and surfaces against Material 3 Android behavior first;
- prefer Android-native semantic colors and interaction feedback without hardcoding an Android-only renderer;
- validate scroll, back behavior, tab bar separation, image sizing, and gestures on Android first;
- keep the shared React Native implementation free of iOS-breaking imports.

## Data and state boundaries

### Route

The route only:

- reads and validates `slug`;
- renders `GameDetail`;
- declares Stack screen options.

It does not own API queries, chart range state, local tabs, auth policy, or layout calculations.

### Server data

Keep `useGameDetailData(gameId)` focused on typed server state:

- core hero and overview;
- Steam/review/pricing data;
- media and links;
- related games;
- loading/error/empty status and retry callbacks.

Derive image URLs, title metadata, chips, and metadata items once at the screen/view-model boundary. Do not rebuild Steam CDN and IGDB URLs in multiple child components.

### Actions

Keep `useGameDetailActions(gameId)` focused on auth-aware mutations and share/list behavior. It may navigate to Profile for signed-out actions, but child visual components receive callbacks and do not access Router, auth context, or query clients directly.

### Local screen state

The screen owns:

- active Overview/Media/Links tab;
- summary expanded/collapsed state;
- selected chart interval;
- chart tooltip visibility if required by the chart API.

None of these values may call `router.push` or change the root tab selection.

## Page composition contract

```text
Root Stack detail screen
└── GameDetail
    ├── Stack.Screen configuration
    └── ScrollView
        └── DetailContent
            ├── MediaRecord
            ├── LocalDetailTabs content
            ├── ActionSurfaceRow
            ├── SummaryCard
            ├── SteamNowSection
            ├── RatingsAndPriceSection
            ├── GameInformationSection
            ├── SteamPlayersSection
            └── RelatedGameRail
```

There is one vertical scroll owner. Horizontal scrolling is allowed only for screenshots and related games. No `Link`, route-level `Pressable`, or parent gesture wrapper may surround the full detail content.

### MediaRecord

- hero banner has a stable aspect ratio and compact fallback;
- chips overlay the hero without affecting measured height;
- cover overlaps the lower hero edge using one named layout token;
- title, studio/publisher, and genre remain in the same record composition;
- artwork uses `expo-image` directly;
- image callbacks update explicit loading/error state;
- no `RNHostView` is needed for the primary page artwork.

### ActionSurfaceRow

- four equal-width surfaces;
- consistent touch target and gap;
- icon plus short label;
- all four remain present signed out;
- no full-width sign-in replacement;
- callbacks are supplied by the action hook.

### SummaryCard

- compact preview with a deliberate line limit;
- centered Read more/Show less action;
- expansion remains in the same card;
- no navigation or route changes.

### MetadataGrid

Implement one reusable React Native grid with a `columns` parameter:

```text
MetadataGrid(columns=3, items=steamNowItems)
MetadataGrid(columns=2, items=ratingsAndPriceItems)
MetadataGrid(columns=2, items=gameInformationItems)
```

Rules:

1. Chunk populated items into rows.
2. Give real cells equal width.
3. Draw vertical dividers only between real adjacent cells.
4. Draw horizontal dividers only between rows.
5. Keep an odd final item in the first column without a trailing divider.
6. Keep padding inside the card boundary.
7. Use semantic theme colors and Android-first surface styling with iOS-safe fallbacks.
8. Keep cards inset from the page edges.

### Steam chart

- selected range remains local state;
- peak and average are separate series and legend entries;
- range controls never navigate;
- chart uses the existing TanStack React Native chart path unless a focused bug requires change;
- tooltip formatting remains compact and dismissible;
- no interval mixing or artificial x-axis padding.

### Media and Links tabs

Media is a horizontal screenshot rail with explicit loading/empty/error states. Links are a compact vertical list with explicit external-link actions. Neither tab changes the root tab selection.

## Implementation phases

### Phase 0 — reconcile the interrupted worktree

1. Inspect the current route tree and identify which changes were applied by the interrupted implementation pass.
2. Preserve unrelated user changes.
3. Ensure the route tree matches the root Stack + `(tabs)` + canonical `/games/[slug]` structure.
4. Remove only obsolete duplicate game-detail route files after all links are migrated.
5. Confirm the project still typechecks before the renderer rewrite.
6. Run an Android-only Experimental Stack spike only if predictive-back behavior is worth evaluating; discard it unless its limited options and no-mixing restriction are acceptable.

### Phase 1 — establish universal route and header foundation

1. Finish root Stack and `(tabs)` ownership.
2. Migrate every game link to `/games/[slug]`.
3. Configure a transparent standard Stack header with one title/back/share ownership path.
4. Configure one opaque native Stack header with safe content spacing.
5. Centralize semantic/iOS and Material 3 Android color tokens, including dynamic-color roles and static fallbacks.
6. Make the first scrollable content safe-area-correct on Android without double-applying insets.
7. Verify the detail route has no NativeTabs bar, no settings gear, and no duplicate header.

### Phase 2 — replace the page shell with React Native/Expo

1. Replace Compose layout containers in the core detail screen with React Native/Expo layout.
2. Use one `Animated.ScrollView` and no nested vertical scroll owner.
3. Build the MediaRecord and explicit image states.
4. Build local tabs, action surfaces, and SummaryCard.
5. Keep universal `@expo/ui` controls only where they provide a real native benefit.

### Phase 3 — migrate data sections

1. Keep generated hooks/types untouched.
2. Shape typed display data once at the screen/view-model boundary.
3. Implement the shared React Native MetadataGrid.
4. Migrate Steam Now, Ratings and price, and Game information.
5. Attach chart, media rail, links, and related games after core geometry is stable.
6. Ensure nullable Steam/media/links data produces compact states rather than blank regions.

### Phase 4 — Android-first interaction and visual validation

Validate on the Android device first:

1. detail top at rest;
2. short swipe beginning in the hero;
3. swipe beginning in the action row;
4. swipe beginning in Summary;
5. metadata grids;
6. chart range changes and tooltip dismissal;
7. Media horizontal scrolling;
8. Links actions;
9. signed-out action behavior;
10. back navigation to the originating catalog screen;
11. Android build/relaunch from the active Expo bundle.
12. Material 3 dynamic colors on Android 12+ and static Material 3 fallback behavior on older Android versions or emulator configurations.
13. Native Tabs reselect/scroll-to-top behavior and preservation of the originating tab after returning from detail.

For every vertical swipe:

- route remains `/games/[slug]`;
- local detail tab remains unchanged;
- Browse/Trending selection does not change;
- no action callback fires;
- NativeTabs is absent from the detail screen;
- the native Stack header does not intercept the gesture.

iOS validation is static/code-path validation only unless an iOS device becomes available. Do not introduce an iOS-specific implementation just to satisfy an unavailable device test.

## Validation commands

Run from `code/app/mobile` without starting Expo, Aspire, or device automation during the code pass:

```powershell
bun run fmt:check
bun run lint
bunx tsc --noEmit
npx expo-doctor
git diff --check
```

If the package-manager migration in the worktree is not finalized, use the repository’s selected package manager consistently and report the mismatch rather than regenerating lockfiles opportunistically.

Android runtime/build validation is separate and may be run when requested:

```text
npx expo run:android
agent-device open com.lossyxp.gameindex --platform android --relaunch
agent-device snapshot -i
agent-device screenshot <detail-top.png>
agent-device scroll down --pixels 800 --settle
agent-device snapshot -i
agent-device screenshot <detail-scrolled.png>
```

If the feature remains compatible with Expo Go, try the Expo Go path before creating or refreshing a custom development build. Use the Android development build when the project’s native dependencies or current app configuration require it. Do not create an iOS build as part of this task.

## Acceptance criteria

- [ ] One canonical `/games/[slug]` route exists outside the NativeTabs group.
- [ ] Native Tabs are mounted only in the `(tabs)` layout and preserve native Android tab behavior.
- [ ] The detail route is a shared React Native/Expo implementation usable on iOS and Android.
- [ ] Android is the primary validated target and matches the reference hierarchy.
- [ ] No bottom NativeTabs bar is visible on detail.
- [ ] Vertical scrolling never selects Browse or Trending.
- [ ] Overview/Media/Links are local state and never navigate through Expo Router.
- [ ] Native stack header appears once with correct safe-area/header-height behavior.
- [ ] The opaque native Stack header owns the detail navigation area on both platforms.
- [ ] Detail content uses one vertical React Native scroll owner with bottom safe-area spacing.
- [ ] Hero and cover render or show compact fallbacks without a blank void.
- [ ] MediaRecord keeps hero, chips, cover, title, and metadata together.
- [ ] Four equal action surfaces remain visible signed out.
- [ ] Summary expands in place.
- [ ] MetadataGrid is shared and supports 3-column and 2-column layouts.
- [ ] Media scrolls horizontally without changing vertical scroll state.
- [ ] Chart controls do not navigate and tooltip dismissal works.
- [ ] Dynamic/semantic colors are used with Android-first styling and iOS-safe fallbacks.
- [ ] Android Material 3 dynamic colors are used where available, with static Material 3 fallbacks and `useColorScheme()`-driven updates.
- [ ] Universal `@expo/ui` controls are wrapped in the package-root `Host` and platform-specific Compose imports are isolated or avoided.
- [ ] Experimental Stack is either rejected for documented SDK 57 limitations or adopted only after a complete Android navigator compatibility check.
- [ ] No Android-only Compose tree is required for the core page.
- [ ] No generated Kubb files or OpenAPI schemas are modified.
- [ ] Formatting, lint, typecheck, and diff checks pass.
- [ ] Android build/runtime validation passes when run.

## Explicit non-goals

- Do not redesign the backend API.
- Do not edit generated Kubb/OpenAPI output.
- Do not create separate iOS and Android page renderers.
- Do not require an iOS device to complete the feature.
- Do not make Experimental Stack the default without proving its limited options and Android-wide navigator compatibility.
- Do not replace the whole app with Android-only Compose.
- Do not keep patching offset/spacer defects after the React Native page shell is established.
- Do not add speculative state hooks for simple local presentation state.

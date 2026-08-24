# Mobile dogfood report

- Date: 2026-08-24
- Platform: Android, physical Pixel 7 Pro
- Target: `com.lossyxp.gameindex`
- Session: `physical-verify` (`Pixel 7 Pro`, physical device; device target explicitly selected)
- Scope: Discover, game detail, summary expansion, media grid/viewer, Steam chart controls, Search, Browse navigation, Profile, Settings, and bottom-of-page states.
- Findings: 0 active

## Issues

### DF-001 — Resolved — Profile Settings button navigates

- Category: Functional/navigation
- Affected flow: Profile → Settings
- Repro:
  1. Tap `Profile` in bottom navigation.
  2. Tap the `Settings` button in the upper-right corner.
- Expected: The Settings screen opens.
- Actual: The Settings screen opens successfully after the Metro bundle is refreshed.
- Evidence:
  - `screenshots/df-001-settings-verified.png`
  - Physical-device verification: Settings opened, showed the Settings header and GitHub row, and the back button returned to Profile.

### DF-002 — Resolved — Recently visited renders persisted games

- Category: Content/data
- Affected flow: Discover → bottom of page → Recently visited
- Repro:
  1. Open Counter-Strike 2 from Discover.
  2. Return to Discover.
  3. Scroll to the bottom.
  4. Inspect `Recently visited`.
- Expected: The recently visited Counter-Strike 2 entry appears.
- Actual: After the fix, the section renders the visited games without the error state. A newly opened game appears first, and the list remains present after relaunching the app.
- Evidence:
  - `screenshots/issue-003-recently-visited-error.png`
  - `screenshots/df-002-recently-visited-fixed.png`
  - `screenshots/df-002-recently-visited-persisted.png`

## Verified flows

- Counter-Strike 2 detail opened without crashing.
- Summary text expanded and collapsed by tapping the text.
- Media loaded as a two-column image grid; tapping an image opened the viewer and Back returned to the grid.
- Chart range controls switched between 24H, 7D, and 30D without replacing the chart with a loading state.
- Chart popup displayed UTC and local time and dismissed when tapping outside the chart.
- Discover collection pages for Trending, Most played on Steam, and Coming up opened successfully.
- Search opened and returned results for `counter`.
- Recently visited recorded a successfully loaded game, rendered the persisted rail, and survived a physical-device app relaunch.

## Test notes

- The automation daemon briefly lost its connection during Search exploration; the session was closed and reopened on the Pixel 7 Pro before continuing.
- Steam sign-in was not completed. On this device, the login URL was intercepted by the installed URL Checker app, so that external-handler behavior was not classified as an app defect.
- An initial exploratory session was accidentally opened on the Pixel 10 Pro emulator. Its emulator-only Browse evidence was discarded and is not counted above.
- Browse was rechecked on the physical Pixel 7 Pro and opened the Browse rankings screen successfully: `screenshots/browse-physical-verified.png`.

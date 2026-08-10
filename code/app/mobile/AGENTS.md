# Mobile

Expo SDK 57 mobile client. Android is the active target; iOS is out of scope unless requested.

## Architecture

- Use Expo Router for file-based navigation. Keep routes thin and preserve the existing tab stacks and root-tab behavior.
- Organize code under `src/`: routes in `app`, feature behavior in `features`, reusable game presentation in `entities`, shared UI in `shared`, and app-owned utilities/hooks/config in their respective folders.
- Reuse `GameCard` and `getGameCardData` for standard grid, rail, and cover catalog presentations. Reuse the Android `GameArtwork` primitive for shared cover/placeholder rendering. Keep a specialized card only when it adds genuinely distinct information or actions, such as browse ranking/player metrics or list removal actions.
- Use `ContentState` for loading, error, empty, and ready branches throughout the app. Add a new state component only when the interaction or layout is genuinely different.
- Keep platform-specific files adjacent to their shared implementation. Avoid generic catch-all component folders.

## Data and authentication

- Use generated Kubb hooks and types for Backend.API. Never edit `src/gen`; run `pnpm run generate` after API changes.
- Keep server state in the shared TanStack Query client. Keep local preferences in the existing stores; do not create clients in screens.
- Authentication is owned by the shared auth context/provider. SecureStore access goes through the profile auth-storage wrapper.
- Treat generated numeric IDs as strings at the client boundary and never put `BigInt` values in query keys.
- Use the current `EXPO_PUBLIC_API_URL` mobile-api tunnel for physical-device API access. Never put API keys or write credentials in the bundle.

## Android UI

- Prefer `@expo/ui` and Jetpack Compose components through `Host` for Android UI. Use semantic theme values and native controls when they fit; bridge to React Native only when Expo UI has no suitable primitive (for example, `expo-image` hosted through `RNHostView`).
- Read the Expo UI skill before changing Android UI. Keep Android Compose trees in `.android.tsx` files outside `app/`, and verify the installed `@expo/ui` component and modifier types before introducing a new primitive.
- Avoid custom control replacements, Tailwind, raw interface colors, and manual layout math when native Compose behavior is available.

## Workflow

- From `code/app/mobile/`, run `pnpm run fmt`, `pnpm exec tsc --noEmit`, and `pnpm run lint` for focused changes.
- Do not run `pnpm install`, start Expo, start Aspire, or use device automation unless explicitly requested.
- Keep changes focused, uncommitted, and local. Do not edit generated files or unrelated work.

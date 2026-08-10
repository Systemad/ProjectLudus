# Mobile

Expo SDK 57 mobile client. Android is the active target; iOS is out of scope unless requested.

## Architecture

- Use Expo Router for file-based navigation. Keep routes thin and preserve the existing tab stacks and root-tab behavior.
- Organize code under `src/`: routes in `app`, feature behavior in `features`, reusable game presentation in `entities`, shared UI in `shared`, and app-owned utilities/hooks/config in their respective folders.
- Reuse existing game-card primitives and `getGameCardData` when adapting catalog responses. Add specialized presentation only for genuinely distinct behavior or information.
- Keep platform-specific files adjacent to their shared implementation. Avoid generic catch-all component folders.

## Data and authentication

- Use generated Kubb hooks and types for Backend.API. Never edit `src/gen`; run `pnpm run generate` after API changes.
- Keep server state in the shared TanStack Query client. Keep local preferences in the existing stores; do not create clients in screens.
- Authentication is owned by the shared auth context/provider. SecureStore access goes through the profile auth-storage wrapper.
- Treat generated numeric IDs as strings at the client boundary and never put `BigInt` values in query keys.
- Use the current `EXPO_PUBLIC_API_URL` mobile-api tunnel for physical-device API access. Never put API keys or write credentials in the bundle.

## Android UI

- Prefer `@expo/ui` and Jetpack Compose components through `Host` for Android UI. Use semantic theme values and native controls when they fit; bridge to React Native only when Expo UI has no suitable primitive.
- Read the Expo UI skill before changing Android UI. Prefer the existing shared state components for loading, error, and empty states.
- Avoid custom control replacements, Tailwind, raw interface colors, and manual layout math when native Compose behavior is available.

## Workflow

- From `code/app/mobile/`, run `pnpm run fmt`, `pnpm exec tsc --noEmit`, and `pnpm run lint` for focused changes.
- Do not run `pnpm install`, start Expo, start Aspire, or use device automation unless explicitly requested.
- Keep changes focused, uncommitted, and local. Do not edit generated files or unrelated work.

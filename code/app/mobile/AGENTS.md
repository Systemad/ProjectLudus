# Mobile

Expo SDK 57 client. Android is the active target; support iOS only when requested.

## Structure

- Use Expo Router; keep routes thin.
- Under `src/`, use `app` for routes, `features` for feature behavior, `entities` for reusable domain UI, and `shared` for common UI/utilities.
- Keep platform-specific files beside their shared implementation. Android Compose belongs in `.android.tsx` files outside `app/`.
- Reuse existing game cards, artwork, and `ContentState` before adding new equivalents.
- `src/gen/` is generated: never edit or format it manually.

## React

- React Compiler is enabled. Do not add `useMemo`, `useCallback`, or `React.memo`; use direct calculations and inline callbacks.
- Use `useEffect` only to synchronize with an external system. Prefer render-time derivation, event handlers, or TanStack Query. Clean up unavoidable effects.
- Do not use `"use no memo"` unless explicitly requested.

## Data and types

- Use generated Kubb hooks/types and run `pnpm run generate` after API changes.
- Keep server state in TanStack Query and authentication in the existing auth provider.
- Never use `any`. Treat client IDs as strings; never put `BigInt` in query keys.
- Use environment variables for API configuration. Never bundle secrets.

## Android UI

- Prefer `@expo/ui` Jetpack Compose through `Host`; use semantic theme values and native controls.
- Use `RNHostView` only to bridge React Native content such as `expo-image`.
- Read the Expo UI skill and verify installed component/modifier types before adding Compose primitives.
- Avoid Tailwind, raw interface colors, custom control replacements, and manual layout math when native Compose provides the behavior.

## Workflow

- For focused changes, run from this directory: `pnpm run fmt`, `pnpm exec tsc --noEmit`, and `pnpm run lint`.
- Do not install packages, start Expo/Aspire, or use device automation unless requested.
- Keep changes focused, uncommitted, and local.

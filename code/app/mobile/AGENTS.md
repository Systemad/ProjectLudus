# Android mobile

This is the Expo SDK 57 mobile client for Game-Index. Android is the active target. iOS is out of scope unless explicitly requested.

## Stack

- Expo Router for file-based navigation and nested tab stacks
- Expo UI Jetpack Compose components for Android UI
- TanStack Query for server state
- Kubb-generated hooks and types for Backend.API
- Expo SecureStore for Steam session tokens
- Expo MCP and agent-device for local device verification

## Structure

```text
src/
├── app/          # thin Expo Router routes and layouts
├── api/          # API configuration
├── config/       # app constants
├── entities/     # reusable game cards, images and rails
├── features/     # vertical feature slices
├── gen/          # Kubb output; never edit manually
├── hooks/        # shared hooks
├── navigation/   # Router/navigation components
├── shared/       # small shared presentation primitives
├── stores/       # local preferences and small cross-screen state
├── types/        # app-owned types
└── utils/        # app-owned utilities
```

Keep behavior in feature folders and reusable game presentation in entities. Keep platform variants adjacent only when behavior genuinely differs. Do not create generic flat component folders.

## Data and auth

- Set EXPO_PUBLIC_API_URL to the current AppHost mobile-api tunnel URL for physical devices.
- Run pnpm run generate after API changes; use only generated Kubb hooks and types under src/gen/.
- Generated numeric IDs are strings at the client boundary: use String(id).
- Never place BigInt values in query keys.
- Keep server state in generated TanStack Query hooks; keep preferences in the existing stores.
- Do not create a QueryClient inside a screen or duplicate generated clients.
- Steam tokens use the profile auth-storage wrapper backed by expo-secure-store.
- Do not put API keys or write credentials in the mobile bundle.

## Expo UI and Android presentation

Read the applicable Expo UI skill before implementing Android UI. Prefer native Jetpack Compose components from @expo/ui/jetpack-compose through Host, including Column, Row, FlowRow, LazyColumn, LazyRow, NavigationBar, BottomSheet, DockedSearchBar, and native controls when they fit the interaction.

Use Expo UI semantic theme values and Compose modifiers. Avoid React Native View, FlatList, StyleSheet.create, custom control replacements, Tailwind, raw interface colors, unsafe casts, manual row slicing, and custom layout math when Compose provides the behavior. Keep touch targets, keyboard behavior, safe areas, and reduced motion accessible.

Preserve Expo Router tab stacks and root-tab reset behavior. Do not replace file-based routes with a manual tab registry.

## Commands

Run from code/app/mobile/:

```powershell
pnpm run generate
pnpm run fmt
pnpm exec tsc --noEmit
pnpm run lint

$env:EXPO_UNSTABLE_MCP_SERVER = "1"; pnpm expo start --dev-client
```

Run pnpm run fmt after every mobile code change. Do not run pnpm install, start Aspire, launch Expo/Android, or run device automation unless requested.

## Device automation

Enable the local agent-device MCP in Codex /mcp. For CLI fallback, first run:

```powershell
agent-device --version
agent-device help workflow
```

Use open → snapshot -i → act → re-snapshot → verify → close. Read agent-device help react-native for Metro/React Native issues and agent-device help debugging for logs or runtime failures. Use Expo MCP when the local Expo MCP server is available.

# Game-Index Mobile

Expo SDK 57 app using Expo Router, Expo UI, TanStack Query, and Kubb hooks against `Backend.API`.

## Structure

```text
src/
├── app/          # thin Expo Router routes/layouts
├── api/          # API configuration
├── config/       # app constants/layout
├── entities/     # reusable game cards, images, rails
├── features/     # vertical slices
├── gen/          # Kubb output; never edit
├── hooks/ lib/   # shared hooks/integrations
├── navigation/   # Expo Router/native navigation
├── shared/       # small presentation primitives
├── stores/       # cross-screen state/preferences
├── types/ utils/ # app-owned types/utilities
```

Keep behavior in features, reusable game presentation in `entities`, and platform variants adjacent (`.android.tsx`/`.tsx`). Do not create generic flat component folders.

## Data and auth

- `EXPO_PUBLIC_API_URL` is the AppHost `mobile-api` tunnel URL for physical devices.
- Generate with `pnpm run generate`; use only Kubb hooks/types under `src/gen/`.
- Generated IDs are strings: use `String(id)`. Never put BigInt values in query keys.
- Server state belongs in TanStack Query; app preferences use the existing settings store.
- Steam tokens use `features/profile/auth-storage.ts` and `expo-secure-store`.

## UI

- Use Expo UI native controls first (`Host`, `Column`, `Row`, `Text`, `Button`, `ListItem`, `Checkbox`, `Switch`, `BottomSheet`, Compose components where needed).
- Use Expo UI semantic theme values; no Tailwind, raw interface colors, unsafe casts, or custom replacements for native controls.
- Preserve current UX: Discover rails, Browse two-column grid, Search state/filter sheet, Expo Router tab stacks, and root-tab reset behavior.

## Commands

```powershell
# regenerate/format/validate from this directory
pnpm run generate
pnpm run fmt
pnpm exec tsc --noEmit
pnpm run lint

# start Metro/dev client with Expo MCP
$env:EXPO_UNSTABLE_MCP_SERVER = "1"; pnpm expo start --dev-client
```

Run `pnpm run fmt` after every mobile code change. Do not run Android/iOS/web unless requested.

## Device automation

Enable `agent-device` in Codex `/mcp` and use its MCP tools when available. CLI fallback requires:

```powershell
agent-device --version
agent-device help workflow
```

Use the serial loop `open → snapshot -i → act → verify → close`; read `help debugging` for logs/runtime failures and `help react-native` for Metro or React Native issues.

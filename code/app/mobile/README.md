# Game-Index Mobile

Android-first Expo SDK 57 client.

## Local setup

From this directory, set EXPO_PUBLIC_API_URL to the current AppHost mobile-api tunnel URL for a physical device, then run:

```powershell
pnpm run generate
pnpm run fmt
pnpm exec tsc --noEmit
pnpm run lint

$env:EXPO_UNSTABLE_MCP_SERVER = "1"; pnpm expo start --dev-client
```

The implementation rules are in [AGENTS.md](AGENTS.md): Expo UI Jetpack Compose components are preferred, iOS is currently out of scope, and Kubb output is generated rather than hand-edited.

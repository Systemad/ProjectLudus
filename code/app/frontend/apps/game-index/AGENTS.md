# Game-Index web app

This is the React/Vite web client at apps/game-index.

## Stack

- React with the React Compiler
- TanStack Router for file-based routing
- TanStack Query for server state
- Kubb-generated hooks and types
- Astryx XDS components and semantic tokens
- StyleX only where XDS does not express layout or interaction
- Typesense for game search

Tailwind is prohibited. Do not introduce raw interface colors, legacy bg/fg variables, or custom component replacements for XDS.

## Structure

~~~text
src/
├── routes/       # TanStack Router route files
├── features/     # vertical feature slices
├── components/   # shared presentation components
├── gen/          # Kubb-generated hooks/types; never edit
├── api/          # API/client configuration
├── styles/       # document-level reset/font styles
└── lib/          # small app integrations
~~~

Keep route files thin and place behavior in feature slices. Use XDS Table, Card, ClickableCard, Overlay, MediaTheme, SegmentedControl, TabList, Dialog, EmptyState, and related primitives before adding custom UI.

## Data and navigation

- Generate API clients with pnpm exec kubb generate from the Backend.API OpenAPI document.
- Use generated TanStack hooks; do not handwrite fetch clients or edit src/gen/.
- Keep URL-addressable game tabs in TanStack Router search state.
- Preserve existing routes, primary navigation labels, legal copy, and game tab query values.
- Keep catalog queries data-first and avoid duplicating backend aggregation in the client.

## Styling and validation

- Use Astryx XDS semantic tokens for color, surface, border, spacing, radius, and state.
- Use StyleX for responsive layout and interaction only when XDS props are insufficient.
- Do not add Tailwind classes, hard-coded color values, shadow-based elevation, or gradient text.
- Keep tables compact and data-dense; preserve loading, empty, error, hover, and focus states.

Run from code/app/frontend/:

~~~powershell
pnpm exec kubb generate
vp lint
vp build
~~~

The root AGENTS.md supplies the shared workflow and approval rules.


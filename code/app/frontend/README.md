# Game-Index Web

Vite+ workspace for the React Game-Index client.

## Stack

React Compiler, TanStack Router, TanStack Query, Kubb-generated hooks, Astryx XDS, StyleX, and Typesense.

## Commands

Run from this directory:

~~~powershell
pnpm exec kubb generate
vp lint
vp build
~~~

The app in apps/game-index owns the routes and feature slices. Use XDS components and semantic tokens before adding StyleX; Tailwind is not used. Generated output under src/gen/ is never edited manually.

See the root AGENTS.md for workflow and approval rules.


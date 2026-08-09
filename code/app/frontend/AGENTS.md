# Frontend workspace

This directory is the Vite+ frontend workspace. The Game-Index application has its own instructions in apps/game-index/AGENTS.md.

## Workspace rules

- Use pnpm and Vite+ commands; do not invoke node_modules binaries directly.
- Keep generated Kubb output under each app's src/gen/ untouched.
- Run focused lint/build checks for the changed workspace.
- Follow the root AGENTS.md for branch, approval, commit, push, pull-request, and Linear rules.

## Commands

Run from code/app/frontend/:

~~~powershell
pnpm exec kubb generate
vp lint
vp build
~~~

Read apps/game-index/AGENTS.md before changing the Game-Index app.


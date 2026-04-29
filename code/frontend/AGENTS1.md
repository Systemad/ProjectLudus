<!-- intent-skills:start -->
## Skill Loading

Before substantial work:
- Skill check: run `npx @tanstack/intent@latest list`, or use skills already listed in context.
- Skill guidance: if one local skill clearly matches the task, run `npx @tanstack/intent@latest load <package>#<skill>` and follow the returned `SKILL.md`.
- Monorepos: when working across packages, run the skill check from the workspace root and prefer the local skill for the package being changed.
- Multiple matches: prefer the most specific local skill for the package or concern you are changing; load additional skills only when the task spans multiple packages or concerns.
<!-- intent-skills:end -->

<!--VITE PLUS START-->

# Using Vite+, the Unified Toolchain for the Web

This project is using Vite+, a unified toolchain built on top of Vite, Rolldown, Vitest, tsdown, Oxlint, Oxfmt, and Vite Task. Vite+ wraps runtime management, package management, and frontend tooling in a single global CLI called `vp`. Vite+ is distinct from Vite, and it invokes Vite through `vp dev` and `vp build`. Run `vp help` to print a list of commands and `vp <command> --help` for information about a specific command.

Docs are local at `node_modules/vite-plus/docs` or online at https://viteplus.dev/guide/.

## Review Checklist

- [ ] Run `vp install` after pulling remote changes and before getting started.
- [ ] Run `vp check` and `vp test` to format, lint, type check and test changes.
- [ ] Check if there are `vite.config.ts` tasks or `package.json` scripts necessary for validation, run via `vp run <script>`.

<!--VITE PLUS END-->

<!--GAME-INDEX START-->

## React Frontend for game-index (apps/game-index)

## Root path for `game-index`

**game-index**: apps/game-index. `game-index` refers to `apps/game-index`

### Routing (generouted)

File-based routing in `src/routes/`. **`src/routeTree.gen.ts` is auto-generated — never edit it manually.**

### API and State

- **Read and Write API**: Tanstack Query is used, combined with Auto-Generated hooks from from `src/gen`

### UI Stack

- **Components**: Refer to `doc/yamada-ui-digest.md` for specifications on how to use YamadaUI and which rules to follow
- **Search**: Typesense via `react-instantsearch` + `typesense-instantsearch-adapter`

<!--GAME-INDEX END-->

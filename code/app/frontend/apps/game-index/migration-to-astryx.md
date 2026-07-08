# Migration Guide: YamadaUI + BaseUI → Astryx + Mantine Hooks

## Goal

Remove all dependencies on:
- `@yamada-ui/react` (consumed indirectly through the `ui` workspace package)
- `@base-ui/react` (used for NavigationMenu and PreviewCard wrappers)
- Custom theme at `src/theme/` (YamadaUI-specific tokens)

Replace with:
- `@astryxdesign/core` + `@astryxdesign/theme-neutral` for all UI components
- `@astryxdesign/theme-neutral` provides the design token set (colors, spacing, typography, radii)
- `@mantine/hooks` for state management hooks
- `@stylexjs/stylex` for custom styling with design tokens
- Native HTML/CSS for simple layout containers (replacing `Box`, `Image`, etc.)

## Pre-requisites

All already installed in `apps/game-index/package.json`:
- `@astryxdesign/core` (catalog:)
- `@astryxdesign/theme-neutral` (catalog:)
- `@astryxdesign/cli` (catalog:)
- `@mantine/hooks` (catalog:)
- `@stylexjs/stylex` (transitive dep of core)
- `@fontsource-variable/inter` (catalog:)

## Files to modify

| # | File | Action |
|---|------|--------|
| 1 | `package.json` | Remove 3 dependencies |
| 2 | `src/main.tsx` | Replace UIProvider with Theme |
| 3 | `src/app/app-shell.tsx` | Replace Box layout with AppShell |
| 4 | `src/app/navigation-bar.tsx` | Full rewrite with TopNav + TopNavMegaMenu |
| 5 | `src/app/footer.tsx` | Full rewrite with Section, Grid, Divider, Text, Link |
| 6 | `src/app/page-wrapper.tsx` | Replace Box with native div |
| 7 | `src/components/hover-image.tsx` | Replace useHover + Box with HoverCard |
| 8 | `src/components/router-link.tsx` | Replace ui imports with Astryx |
| 9 | `src/components/game-preview-card.tsx` | Replace ui imports with Astryx |
| 10 | `src/components/igdb-image.tsx` | Replace Image with native img |
| 11 | All other `.tsx`/`.ts` files | Bulk import replacement "ui" → Astryx |

## Files to delete

| # | File | Reason |
|---|------|--------|
| 1 | `src/theme/` (entire directory) | YamadaUI-specific theme |
| 2 | `src/components/navigation-menu.tsx` | BaseUI wrapper — replaced by TopNavMegaMenu |
| 3 | `src/components/preview-card.tsx` | BaseUI wrapper — replaced by HoverCard |

## Files to create

| # | File | Contents |
|---|------|----------|
| 1 | `src/components/astryx-box.tsx` | Custom Box component using stylex.create + tokens |

---

## Step-by-step

### Step 1: `package.json` — remove dependencies

```diff
"dependencies": {
-  "ui": "workspace:*",
-  "@base-ui/react": "catalog:",
},
"devDependencies": {
-  "@yamada-ui/cli": "catalog:",
}
```

Run `pnpm install` after making this change.

---

### Step 2: `src/main.tsx` — replace providers

**Remove these imports:**
```tsx
import { UIProvider } from "ui";
import { config } from "./theme/config";
import { theme } from "./theme";
```

**Add these imports:**
```tsx
import { Theme } from "@astryxdesign/core/theme";
import { neutralTheme } from "@astryxdesign/theme-neutral/built";
import "@astryxdesign/theme-neutral/theme.css";
```

**Replace the provider wrapper:**

Before:
```tsx
<UIProvider config={config} theme={theme}>
  <QueryClientProvider client={queryClient}>
    <RouterProvider router={router} />
  </QueryClientProvider>
</UIProvider>
```

After:
```tsx
<Theme theme={neutralTheme} mode="dark">
  <QueryClientProvider client={queryClient}>
    <RouterProvider router={router} />
  </QueryClientProvider>
</Theme>
```

**Keep the existing imports for `router` and `queryClient`:**
```tsx
import { router, queryClient } from "./router";
```

---

### Step 3: `src/app/app-shell.tsx` — AppShell with top-nav only

Before:
```tsx
import { Box } from "ui";
import type { ReactNode } from "react";
import { Footer } from "@src/app/footer";
import { NavigationBar } from "@src/app/navigation-bar";

export type AppShellProps = {
    active?: string;
    children: ReactNode;
};

export function AppShell({ active = "home", children }: AppShellProps) {
    return (
        <Box minH="dvh" color="fg.base" display="flex" flexDirection="column">
            <NavigationBar active={active} />
            <Box as="main" flex="1">
                {children}
            </Box>
            <Footer />
        </Box>
    );
}
```

After:
```tsx
import { AppShell } from "@astryxdesign/core/AppShell";
import type { ReactNode } from "react";
import { NavigationBar } from "@src/app/navigation-bar";
import { Footer } from "@src/app/footer";

export type AppShellProps = {
    active?: string;
    children: ReactNode;
};

export function AppShell({ active = "home", children }: AppShellProps) {
    return (
        <>
            <AppShell
                contentPadding={4}
                height="auto"
                topNav={<NavigationBar active={active} />}
            >
                {children}
            </AppShell>
            <Footer />
        </>
    );
}
```

**Key changes:**
- `Box` from `"ui"` → `AppShell` from `@astryxdesign/core/AppShell`
- No `sideNav` slot → top-nav only layout (no sidebar)
- `contentPadding={4}` = 16px padding (per Astryx best practices)
- `height="auto"` lets page grow with content, footer sits at bottom
- Footer rendered **outside** AppShell, after children (matches Astryx docsite pattern)
- `<Box as="main">` removed — AppShell's children already render in `<main>`

---

### Step 4: `src/app/navigation-bar.tsx` — full rewrite

**Delete the entire existing file** and replace with:

```tsx
import { useCallback } from "react";
import { Link } from "@tanstack/react-router";
import { TopNav, TopNavHeading, TopNavItem } from "@astryxdesign/core/TopNav";
import { TopNavMegaMenu } from "@astryxdesign/core/TopNavMegaMenu";
import { TopNavMegaMenuItem } from "@astryxdesign/core/TopNavMegaMenuItem";
import { NavIcon } from "@astryxdesign/core/NavIcon";
import { Button } from "@astryxdesign/core/Button";
import { Icon } from "@astryxdesign/core/Icon";
import { HStack } from "@astryxdesign/core/HStack";
import {
    CubeIcon,
    MagnifyingGlassIcon,
    UserCircleIcon,
    BellIcon,
    CalendarDaysIcon,
    BuildingOfficeIcon,
    RocketLaunchIcon,
} from "@heroicons/react/24/outline";

type NavigationBarProps = {
    active?: string;
};

const NAV_ITEMS = [
    {
        id: "search",
        label: "Search",
        isMegaMenu: true,
        items: [
            {
                title: "Games",
                description: "Search by name, genre, or keyword",
                href: "/games/search",
                icon: MagnifyingGlassIcon,
            },
            {
                title: "Companies",
                description: "Search for publishers and developers",
                href: "/companies/search",
                icon: BuildingOfficeIcon,
            },
        ],
    },
    {
        id: "events",
        label: "Events",
        isMegaMenu: true,
        items: [
            {
                title: "Upcoming Events",
                description: "Browse gaming events and conferences",
                href: "/events",
                icon: RocketLaunchIcon,
            },
        ],
    },
    {
        id: "calendar",
        label: "Calendar",
        href: "/calendar",
    },
    {
        id: "companies",
        label: "Companies",
        href: "/companies/search",
    },
];

export function NavigationBar({ active: _active }: NavigationBarProps) {
    return (
        <TopNav
            label="Main navigation"
            heading={
                <TopNavHeading
                    heading="Game-Index"
                    href="/"
                    logo={<NavIcon icon={<Icon icon={CubeIcon} size="sm" />} />}
                />
            }
            centerContent={
                <>
                    {NAV_ITEMS.map((item) =>
                        "items" in item && item.isMegaMenu ? (
                            <TopNavMegaMenu
                                key={item.id}
                                label={item.label}
                                items={item.items.map((sub) => (
                                    <TopNavMegaMenuItem
                                        key={sub.href}
                                        title={sub.title}
                                        description={sub.description}
                                        href={sub.href}
                                        icon={
                                            <Icon icon={sub.icon} size="sm" />
                                        }
                                    />
                                ))}
                            />
                        ) : "href" in item ? (
                            <TopNavItem
                                key={item.id}
                                label={item.label}
                                href={item.href}
                                isSelected={_active === item.id}
                            />
                        ) : null
                    )}
                </>
            }
            endContent={
                <HStack gap={2} vAlign="center">
                    <Button
                        label="Search"
                        variant="ghost"
                        icon={<Icon icon={MagnifyingGlassIcon} size="sm" />}
                        isIconOnly
                    />
                    <Button
                        label="Notifications"
                        variant="ghost"
                        icon={<Icon icon={BellIcon} size="sm" />}
                        isIconOnly
                    />
                    <Button
                        label="Profile"
                        variant="ghost"
                        icon={<Icon icon={UserCircleIcon} size="sm" />}
                        isIconOnly
                    />
                </HStack>
            }
        />
    );
}
```

**Key differences from old code:**
- BaseUI `@base-ui/react/navigation-menu` wrappers are gone
- `TopNavMegaMenu` handles positioning, hover behavior, and mobile drawer natively
- No manual `Portal`, `Positioner`, `Popup`, `Arrow`, `Viewport` management
- No YamadaUI `Drawer` for mobile — AppShell handles responsive behavior
- All icons use `@heroicons/react/24/outline`

---

### Step 5: Delete `src/components/navigation-menu.tsx`

Entire file is replaced by `TopNavMegaMenu` inline. No migration needed — just delete.

Reference to these are removed from `navigation-bar.tsx` in Step 4.

---

### Step 6: Delete `src/components/preview-card.tsx` → use `HoverCard`

The file wraps `@base-ui/react/preview-card` with YamadaUI `styled()`. Replace all usages with Astryx `HoverCard`.

**After (Astryx HoverCard):**

```tsx
import { HoverCard } from "@astryxdesign/core/HoverCard";
import { VStack } from "@astryxdesign/core/VStack";
import { Heading, Text } from "@astryxdesign/core/Text";

function GamePreview({ game }: { game: any }) {
    return (
        <HoverCard
            placement="below"
            alignment="start"
            delay={300}
            content={
                <VStack gap={2} style={{ width: 300 }}>
                    <Heading level={5}>{game.name}</Heading>
                    <Text type="body" color="secondary">
                        {game.summary}
                    </Text>
                </VStack>
            }
        >
            <img src={game.cover} alt={game.name} style={{ width: 100 }} />
        </HoverCard>
    );
}
```

For imperative control, use `useHoverCard` from `@astryxdesign/core/useHoverCard`.

**HoverCard props:**

| Prop | Type | Default | Description |
|---|---|---|---|
| `children` | ReactNode | — | Trigger element (must accept a ref) |
| `content` | ReactNode | — | Hover card content |
| `placement` | `'above' \| 'below' \| 'start' \| 'end'` | `'above'` | Position relative to anchor |
| `alignment` | `'start' \| 'center' \| 'end'` | `'center'` | Alignment along placement axis |
| `delay` | number | `300` | Show delay in ms |
| `hideDelay` | number | `200` | Hide delay in ms |
| `isEnabled` | boolean | `true` | Enable/disable triggers |
| `onOpenChange` | `(isOpen: boolean) => void` | — | Visibility callback |

---

### Step 7: `src/app/footer.tsx` — full rewrite

**Replace the entire file** using Astryx components (based on Astryx docsite `SiteFooter.tsx`):

```tsx
import * as stylex from "@stylexjs/stylex";
import { Section } from "@astryxdesign/core/Section";
import { Divider } from "@astryxdesign/core/Divider";
import { HStack, VStack } from "@astryxdesign/core/Layout";
import { Heading, Text } from "@astryxdesign/core/Text";
import { Button } from "@astryxdesign/core/Button";
import { EU } from "country-flag-icons/react/3x2";

const styles = stylex.create({
    footer: {
        paddingTop: "var(--spacing-10)",
    },
    gradientText: {
        background: "linear-gradient(to left, #C6426E, #642B73)",
        WebkitBackgroundClip: "text",
        WebkitTextFillColor: "transparent",
    },
});

export function Footer() {
    return (
        <Section role="contentinfo" padding={6} xstyle={styles.footer}>
            <VStack gap={4} hAlign="center">
                <Heading level={4} xstyle={styles.gradientText}>
                    GAME-INDEX
                </Heading>
                <Text
                    type="supporting"
                    color="secondary"
                    hAlign="center"
                    style={{ maxWidth: 448 }}
                >
                    game-index.app is a fan-made website and is not affiliated
                    with IGDB. All the logos, images, trademarks and creatives
                    are property of their respective owners.
                </Text>
                <Button variant="secondary" size="sm" href="#">
                    <EU style={{ width: "1em", height: "auto" }} />
                    Made in EU
                </Button>
                <Divider />
                <Text type="supporting" color="secondary">
                    &copy;{new Date().getFullYear()} Game-Index
                </Text>
            </VStack>
        </Section>
    );
}
```

**Key changes:**
- `Box` → native `<div>` with `stylex.create()` or `Section`
- `Tag` → native `<span>` or removed
- `bgGradient` → `stylex.create()` with `background` property
- Responsive `paddingX={{ base: 4, md: 6 }}` → `padding={6}` on `Section` (Astryx handles responsive)
- `Text` props: `color="fg.muted"` → `color="secondary"`, `fontSize="xs"` → `type="supporting"`

---

### Step 8: `src/app/page-wrapper.tsx` — replace Box with native div

Before:
```tsx
import type { ComponentProps, ReactNode } from "react";
import { Box } from "ui";

type BoxWidthProps = ComponentProps<typeof Box>;
type Props = { children: ReactNode } & Omit<BoxWidthProps, "children">;

export function PageWrapper({
    children,
    maxW = "9xl",
    px = { base: "3", md: "6", xl: "8" },
    py,
    pt,
    pb,
    ...rest
}: Props) {
    return (
        <Box
            w="full"
            maxW={maxW}
            mx="auto"
            px={px}
            py={py}
            pt={pt}
            pb={pb}
            animation="all 0.5s ease-in-out"
            {...rest}
        >
            {children}
        </Box>
    );
}
```

After:
```tsx
import type { CSSProperties, ReactNode } from "react";

type Props = {
    children: ReactNode;
    maxWidth?: CSSProperties["maxWidth"];
    paddingInline?: CSSProperties["paddingInline"];
    paddingBlock?: CSSProperties["paddingBlock"];
    paddingTop?: CSSProperties["paddingTop"];
    paddingBottom?: CSSProperties["paddingBottom"];
};

export function PageWrapper({
    children,
    maxWidth = "var(--spacing-9xl, 1128px)",
    paddingInline = "clamp(0.75rem, 3vw, 2rem)",
    paddingBlock,
    paddingTop,
    paddingBottom,
}: Props) {
    return (
        <div
            style={{
                width: "100%",
                maxWidth,
                marginInline: "auto",
                paddingInline,
                paddingBlock,
                paddingTop,
                paddingBottom,
            }}
        >
            {children}
        </div>
    );
}
```

The old responsive `{ base: "3", md: "6", xl: "8" }` is replaced with `clamp()` for a single responsive value. Each usage site can pass specific values as needed.

---

### Step 9: `src/components/hover-image.tsx` — use HoverCard

Before:
```tsx
import { Box, Image, useHover } from "ui";
```

After:
```tsx
import { HoverCard } from "@astryxdesign/core/HoverCard";
```

Replace `useHover` + `Box` overlay pattern with `HoverCard` wrapping the image trigger.

---

### Step 10: `src/components/router-link.tsx` — replace ui imports

Before:
```tsx
import { Button, IconButton, Link } from "ui";
```

After:
```tsx
import { Button } from "@astryxdesign/core/Button";
import { IconButton } from "@astryxdesign/core/IconButton";
import { Link } from "@astryxdesign/core/Link";
```

`Link` from Astryx supports `href`, `type`, `color`, `isStandalone` props.

---

### Step 11: Delete `src/theme/` directory

Delete recursively: `src/theme/` containing:
- `src/theme/index.ts`, `src/theme/config.ts`
- `src/theme/tokens/` (20 files)
- `src/theme/semantic-tokens/` (8 files)
- `src/theme/styles/` (4 files)
- `src/theme/registry.json`

All tokens are replaced by Astryx `neutralTheme` from `@astryxdesign/theme-neutral`.

---

### Step 12: Bulk import replacements in ALL component files

The following table maps every import from `"ui"` to its Astryx or Mantine equivalent. Apply these changes across ALL `.tsx` and `.ts` files in `src/`.

#### Component imports

| `from "ui"` | Replace with |
|---|---|
| `import { Text } from "ui"` | `import { Text } from "@astryxdesign/core/Text"` |
| `import { Heading } from "ui"` | `import { Heading } from "@astryxdesign/core/Text"` |
| `import { HStack } from "ui"` | `import { HStack } from "@astryxdesign/core/HStack"` |
| `import { VStack } from "ui"` | `import { VStack } from "@astryxdesign/core/VStack"` |
| `import { Flex } from "ui"` | `import { HStack } from "@astryxdesign/core/HStack"` (horizontal) or `VStack` (vertical) |
| `import { Box } from "ui"` | Native `<div>` or custom Box component |
| `import { Card } from "ui"` | `import { Card } from "@astryxdesign/core/Card"` |
| `import { Button } from "ui"` | `import { Button } from "@astryxdesign/core/Button"` |
| `import { IconButton } from "ui"` | `import { IconButton } from "@astryxdesign/core/IconButton"` |
| `import { Badge } from "ui"` | `import { Badge } from "@astryxdesign/core/Badge"` |
| `import { Tag } from "ui"` | `import { Badge } from "@astryxdesign/core/Badge"` or `import { Token } from "@astryxdesign/core/Token"` |
| `import { TextInput } from "ui"` | `import { TextInput } from "@astryxdesign/core/TextInput"` |
| `import { NumberInput } from "ui"` | `import { NumberInput } from "@astryxdesign/core/NumberInput"` |
| `import { Selector } from "ui"` | `import { Selector } from "@astryxdesign/core/Selector"` |
| `import { Select } from "ui"` | `import { Selector } from "@astryxdesign/core/Selector"` |
| `import { Pagination } from "ui"` | `import { Pagination } from "@astryxdesign/core/Pagination"` |
| `import { Dialog } from "ui"` | `import { Dialog } from "@astryxdesign/core/Dialog"` |
| `import { Drawer } from "ui"` | `import { Dialog } from "@astryxdesign/core/Dialog"` |
| `import { Modal } from "ui"` | `import { Dialog } from "@astryxdesign/core/Dialog"` |
| `import { Accordion } from "ui"` | `import { Accordion } from "@astryxdesign/core/Accordion"` |
| `import { Grid } from "ui"` | `import { Grid } from "@astryxdesign/core/Grid"` |
| `import { GridItem } from "ui"` | `import { GridSpan } from "@astryxdesign/core/GridSpan"` |
| `import { SimpleGrid } from "ui"` | `import { Grid } from "@astryxdesign/core/Grid"` with `columns={{minWidth: ..., repeat: 'fit'}}` |
| `import { Divider } from "ui"` | `import { Divider } from "@astryxdesign/core/Divider"` |
| `import { Separator } from "ui"` | `import { Divider } from "@astryxdesign/core/Divider"` |
| `import { Tooltip } from "ui"` | `import { Tooltip } from "@astryxdesign/core/Tooltip"` |
| `import { Spinner } from "ui"` | `import { Spinner } from "@astryxdesign/core/Spinner"` |
| `import { Skeleton } from "ui"` | `import { Skeleton } from "@astryxdesign/core/Skeleton"` |
| `import { Table } from "ui"` | `import { Table } from "@astryxdesign/core/Table"` |
| `import { NativeTable } from "ui"` | `import { Table } from "@astryxdesign/core/Table"` |
| `import { SegmentedControl } from "ui"` | `import { SegmentedControl, SegmentedControlItem } from "@astryxdesign/core/SegmentedControl"` |
| `import { Switch } from "ui"` | `import { Switch } from "@astryxdesign/core/Switch"` |
| `import { Container } from "ui"` | `import { Section } from "@astryxdesign/core/Section"` |
| `import { Center } from "ui"` | `import { Center } from "@astryxdesign/core/Center"` |
| `import { EmptyState } from "ui"` | Use `Center` + `VStack` + `Text` |
| `import { Menu } from "ui"` | `import { DropdownMenu } from "@astryxdesign/core/DropdownMenu"` |
| `import { Wrap } from "ui"` | `import { HStack } from "@astryxdesign/core/HStack"` + `wrap="wrap"` |
| `import { Image } from "ui"` | Native `<img>` element |
| `import type { ImageProps } from "ui"` | `type ImgProps = React.ImgHTMLAttributes<HTMLImageElement>` |
| `import { For } from "ui"` | Native JavaScript `.map()` |
| `import { Format } from "ui"` | `date-fns` functions (already in deps) |
| `import { AspectRatio } from "ui"` | `import { AspectRatio } from "@astryxdesign/core/AspectRatio"` |
| `import { Collapse } from "ui"` | CSS animation or `motion` (already in deps) |
| `import { CheckboxInput } from "ui"` | `import { CheckboxInput } from "@astryxdesign/core/CheckboxInput"` |
| `import { CheckboxCardGroup } from "ui"` | `import { SelectableCard } from "@astryxdesign/core/SelectableCard"` or `CheckboxInput` + `Card` |
| `import { Radio } from "ui"` | `import { RadioList, RadioListItem } from "@astryxdesign/core/RadioList"` |
| `import { Rating } from "ui"` | Native `<span>` with stars |
| `import { Stat } from "ui"` | `Text` + `Heading` composition |
| `import { Progress } from "ui"` | `import { ProgressBar } from "@astryxdesign/core/ProgressBar"` |
| `import { Link } from "ui"` | `import { Link } from "@astryxdesign/core/Link"` |
| `import { Icon } from "ui"` | `import { Icon } from "@astryxdesign/core/Icon"` with Heroicons |
| `import { Loading } from "ui"` | `import { Spinner } from "@astryxdesign/core/Spinner"` |
| `import { StatusDot } from "ui"` | Use `Badge` or native `<span>` |
| `import { List } from "ui"` | `import { List, ListItem } from "@astryxdesign/core/List"` |
| `import { Breadcrumb } from "ui"` | `import { Breadcrumbs, BreadcrumbItem } from "@astryxdesign/core/Breadcrumbs"` |

#### Hooks imports

| `from "ui"` | Replace with |
|---|---|
| `import { useDisclosure } from "ui"` | `import { useDisclosure } from "@mantine/hooks"` |
| `import { useHover } from "ui"` | `import { useHover } from "@mantine/hooks"` — OR replace with `HoverCard` component where applicable |
| `import { useMediaQuery } from "ui"` | `import { useMediaQuery } from "@mantine/hooks"` |
| `import { useClipboard } from "ui"` | `import { useClipboard } from "@mantine/hooks"` |
| `import { useLocalStorage } from "ui"` | `import { useLocalStorage } from "@mantine/hooks"` |
| `import { useClickOutside } from "ui"` | `import { useClickOutside } from "@mantine/hooks"` |
| `import { useDebouncedValue } from "ui"` | `import { useDebouncedValue } from "@mantine/hooks"` |
| `import { useInterval } from "ui"` | `import { useInterval } from "@mantine/hooks"` |
| `import { useFocusTrap } from "ui"` | `import { useFocusTrap } from "@mantine/hooks"` |
| `import { usePagination } from "ui"` | `import { usePagination } from "@mantine/hooks"` |
| `import { useResizeObserver } from "ui"` | `import { useResizeObserver } from "@mantine/hooks"` |
| `import { useBoolean } from "ui"` | `import { useDisclosure } from "@mantine/hooks"` |
| `import { useCounter } from "ui"` | `useState` + manual handlers |
| `import { useEventListener } from "ui"` | Native `addEventListener` in `useEffect` |
| `import { useMounted } from "ui"` | `useEffect(() => {}, [])` |
| `import { usePrevious } from "ui"` | Store in `useRef` |
| `import { useTimeout } from "ui"` | `setTimeout` in `useEffect` |
| `import { useAsync } from "ui"` | `@tanstack/react-query` (already in deps) |

#### Astryx-specific hooks

```tsx
import { useAppShellMobile } from "@astryxdesign/core/AppShell"; // returns { isMobile }
import { useHoverCard } from "@astryxdesign/core/useHoverCard";   // for custom HoverCard implementations
```

---

### Step 13: Mantine hook usage reference

```tsx
// useDisclosure — manages boolean state
const [opened, { open, close, toggle }] = useDisclosure(false);

// useHover — detects hover state
const { hovered, ref } = useHover<HTMLDivElement>();

// useMediaQuery — matches CSS media query
const isMobile = useMediaQuery("(max-width: 768px)");

// useClipboard — copy to clipboard
const clipboard = useClipboard({ timeout: 500 });
clipboard.copy("text");

// useLocalStorage — localStorage-backed state
const [value, setValue, removeValue] = useLocalStorage({ key: "key", defaultValue: "" });

// useClickOutside — detect click outside element
const ref = useClickOutside(() => handleClose());

// useDebouncedValue — debounce a value
const [debounced, cancel] = useDebouncedValue(value, 300);

// useInterval — setInterval as hook
const interval = useInterval(() => tick(), 1000);
interval.start();

// useFocusTrap — trap focus in element
const focusTrapRef = useFocusTrap(true);

// usePagination — pagination state
const pagination = usePagination({ total: 10 });
// pagination.range, pagination.active, pagination.setPage(n), pagination.next(), pagination.previous()

// useResizeObserver — observe element size
const [ref, rect] = useResizeObserver<HTMLDivElement>();
```

---

### Step 14: Prop mapping reference

| YamadaUI prop | Astryx equivalent | Notes |
|---|---|---|
| `gap="4"` (string) | `gap={4}` (number) | Astryx uses number literals from spacing scale |
| `gap="md"` | `gap={3}` | Map: xs=1, sm=2, md=3, lg=4, xl=6 |
| `isDisabled` | `isDisabled` | Same |
| `isLoading` | `isLoading` | Same |
| `variant="solid"` | `variant="primary"` | Semantic variant names |
| `variant="outline"` | `variant="secondary"` | |
| `variant="ghost"` | `variant="ghost"` | Same |
| `size="sm"`/`md`/`lg` | `size="sm"`/`md"`/`lg"` | Same values |
| `onChange={handler}` | `onChange={handler}` | Same |
| `defaultValue` | `value` | Controlled by default |
| `isRequired` | `isRequired` | Same |
| `isReadOnly` | `isReadOnly` | Same |
| `rounded="md"` | `borderRadius: "var(--radius-element)"` | Use CSS variable |
| `px="4"`, `py="2"` | `paddingInline: "var(--spacing-4)"`, `paddingBlock: "var(--spacing-2)"` | Use CSS variables |
| `_hover={{bg: ...}}` | `:hover` pseudo class in `stylex.create()` | StyleX pattern |
| `colorScheme="blue"` | `variant="blue"` on supported components | |
| `bg="bg.panel"` | `backgroundColor: "var(--color-background-surface)"` | CSS variable |
| `color="fg.base"` | `color: "var(--color-text-primary)"` | CSS variable |
| `color="fg.muted"` | `color="secondary"` on `Text` component | Astryx Text prop |
| `borderColor="border"` | `borderColor: "var(--color-border)"` | CSS variable |
| `fontFamily="heading"` | `Text` with appropriate `type` prop | |
| `fontWeight="bold"` | Set by `type` prop on `Text` | |
| `display={{base: "none", md: "flex"}}` | CSS media queries or `useMediaQuery` hook | |
| `mx="auto"` | `marginInline: "auto"` | CSS |
| `maxW="7xl"` | `maxWidth` CSS property | |
| `textTransform="uppercase"` | `textTransform: "uppercase"` | CSS |
| `bgGradient="linear(...)"` | `background: "linear-gradient(...)"` in `stylex.create()` | CSS |

---

### Step 15: Create custom Box component

Create `src/components/astryx-box.tsx`:

```tsx
import * as stylex from "@stylexjs/stylex";
import type { ReactNode } from "react";

type BoxProps = {
    children: ReactNode;
    as?: "div" | "section" | "main" | "article" | "span";
    className?: string;
    style?: React.CSSProperties;
    onClick?: () => void;
};

const styles = stylex.create({
    base: {
        backgroundColor: "var(--color-background-surface)",
        color: "var(--color-text-primary)",
        padding: "var(--spacing-4)",
        borderRadius: "var(--radius-container)",
    },
});

export const Box = ({
    children,
    as: Tag = "div",
    className,
    style,
    onClick,
}: BoxProps) => {
    return (
        <Tag
            {...stylex.props(styles.base)}
            className={className}
            style={style}
            onClick={onClick}
        >
            {children}
        </Tag>
    );
};
```

**When to use Box:**
- Only when needing a generic container with Astryx-themed background/text colors
- For layout grouping, prefer `HStack`, `VStack`, or `Section` from Astryx
- For unstyled wrappers, use native `<div>`

---

### Step 16: Component migration examples

#### `SegmentedControl`

Before:
```tsx
import { SegmentedControl } from "ui";
<SegmentedControl value={value} onChange={setValue}
    items={[{ label: "Grid", value: "grid" }, { label: "List", value: "list" }]}
/>
```

After:
```tsx
import { SegmentedControl, SegmentedControlItem } from "@astryxdesign/core/SegmentedControl";
<SegmentedControl value={value} onChange={setValue} label="View mode">
    <SegmentedControlItem value="grid" label="Grid" />
    <SegmentedControlItem value="list" label="List" />
</SegmentedControl>
```

#### `Dialog`

Before:
```tsx
import { Dialog } from "ui";
<Dialog isOpen={open} onClose={onClose}>
    <Dialog.Header>Title</Dialog.Header>
    <Dialog.Body>Content</Dialog.Body>
    <Dialog.Footer>Actions</Dialog.Footer>
</Dialog>
```

After:
```tsx
import { Dialog, DialogHeader } from "@astryxdesign/core/Dialog";
import { Layout, LayoutContent, LayoutFooter } from "@astryxdesign/core/Layout";
import { Button } from "@astryxdesign/core/Button";
<Dialog isOpen={open} onOpenChange={setOpen} purpose="form">
    <Layout
        header={<DialogHeader title="Title" onOpenChange={setOpen} />}
        content={<LayoutContent>Content</LayoutContent>}
        footer={
            <LayoutFooter>
                <Button label="Save" variant="primary" onClick={handleSave} />
            </LayoutFooter>
        }
    />
</Dialog>
```

#### `Selector`

Before:
```tsx
import { Select } from "ui";
<Select value={value} onChange={setValue} placeholder="Select..."
    items={[{ label: "Option 1", value: "1" }]}
/>
```

After:
```tsx
import { Selector } from "@astryxdesign/core/Selector";
<Selector value={value} onChange={setValue} label="Field" placeholder="Select..."
    options={["Option 1", "Option 2"]}
/>
```

#### `Grid`

Before:
```tsx
import { SimpleGrid } from "ui";
<SimpleGrid columns={{ base: 1, md: 2 }} gap={4}>
    <Item />
    <Item />
</SimpleGrid>
```

After:
```tsx
import { Grid } from "@astryxdesign/core/Grid";
<Grid columns={{ minWidth: 300, repeat: "fit" }} gap={4}>
    <Item />
    <Item />
</Grid>
```

---

### Step 17: StyleX and tokens reference

**`stylex` import:**
```tsx
import * as stylex from "@stylexjs/stylex";
```

**Design token imports:**
```tsx
import { spacingVars, colorVars, radiusVars } from "@astryxdesign/core";
```

Or use CSS variables directly in StyleX:
```tsx
const styles = stylex.create({
    container: {
        padding: "var(--spacing-4)",
        backgroundColor: "var(--color-background-surface)",
        borderRadius: "var(--radius-container)",
        color: "var(--color-text-primary)",
        borderWidth: 1,
        borderStyle: "solid",
        borderColor: "var(--color-border)",
        ":hover": {
            backgroundColor: "var(--color-background-hover)",
        },
    },
});
```

**Available token categories (from `@astryxdesign/core`):**

| Import | Token prefix | Examples |
|---|---|---|
| `spacingVars` | `--spacing-*` | `--spacing-0`, `--spacing-4`, `--spacing-6` |
| `colorVars` | `--color-*` | `--color-background-surface`, `--color-text-primary` |
| `radiusVars` | `--radius-*` | `--radius-element`, `--radius-container` |
| `fontSizeVars` | `--font-size-*` | `--font-size-sm`, `--font-size-base` |
| `fontWeightVars` | `--font-weight-*` | `--font-weight-normal`, `--font-weight-semibold` |

**Spacing scale:**
```
--spacing-0: 0px      --spacing-3: 12px     --spacing-8: 32px
--spacing-0-5: 2px    --spacing-4: 16px     --spacing-10: 40px
--spacing-1: 4px      --spacing-5: 20px     --spacing-12: 48px
--spacing-1-5: 6px    --spacing-6: 24px
--spacing-2: 8px
```

---

## Summary of changes

| Category | Files |
|---|---|
| `package.json` | 1 |
| `main.tsx` | 1 (providers) |
| `app/app-shell.tsx` | 1 |
| `app/navigation-bar.tsx` | 1 (full rewrite) |
| `app/footer.tsx` | 1 (full rewrite) |
| `app/page-wrapper.tsx` | 1 |
| `components/hover-image.tsx` | 1 |
| `components/router-link.tsx` | 1 |
| `components/game-preview-card.tsx` | 1 |
| `components/igdb-image.tsx` | 1 |
| All other `.tsx`/`.ts` files | Import replacements (est. 40+ files) |
| `src/components/astryx-box.tsx` | 1 (new) |
| Files to delete | 3 |

**The core migration pattern for each file:**
1. Replace `from "ui"` imports with `from "@astryxdesign/core/*"` or `from "@mantine/hooks"`
2. Replace YamadaUI props with Astryx props (see prop mapping table)
3. Replace styling props (`_hover`, `bg`, `colorScheme`, `rounded`) with StyleX tokens or CSS variables

No business logic changes. No route changes. No data fetching changes. Only the UI layer.

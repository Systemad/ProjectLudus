import "./styles.css";
import "./fonts.css";
import { QueryClientProvider } from "@tanstack/react-query";

import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import { UIProvider, defineConfig, extendTheme } from "@packages/ui";
// import { my_theme } from "@packages/theme";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import { queryClient } from "./QueryClient";
// Create a new router instance

/*
createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <UIProvider>
            <App />
        </UIProvider>
    </StrictMode>
);
*/
//base: "bg.float",
//
/*

black: {
    base: "#202123",        // main background
    bg: "#2a2b32",          // surfaces like menu
    subtle: "#2f3037",      // inputs
    muted: "#35363d",       // hover states
    contrast: "white",
    emphasized: "black.200",
    fg: "white",
    ghost: "white/10",
    outline: "#3a3b42",
    solid: "#3a3b42",
},
            black: {
                base: "#0a0b0b",
                bg: "#f8f8f8",
                contrast: "white",
                emphasized: "black.200",
                fg: "black.800",
                ghost: "black.100/50",
                muted: "black.100",
                outline: "black.900",
                solid: "black",
                subtle: "black.50",
            },
*/
const extendedTheme = extendTheme({
    fonts: {
        body: '"Montserrat Variable", "sans-serif"',
        heading: '"Montserrat Variable", "sans-serif"',
        //heading: '"Inter", "Inter Fallback"',
        mono: '"Geist Mono", "Geist Mono Fallback"',
    },
    semanticTokens: {
        colors: {},
    },
    colors: {},
    styles: {
        globalStyle: {
            body: {
                "--root-header-height": "sizes.14",
                "--space": { base: "spaces.lg", md: "spaces.md" },
                //scrollbarGutter: "stable",
                colorScheme: "emerald",
                overflowY: "scroll",
            },
            html: {
                scrollbarWidth: "thin",
                //overflowY: "scroll",
                scrollBehavior: "smooth",
                //scrollbarGutter: "stable",
            },
        },
    },
});
export const config = defineConfig({
    css: { varPrefix: "ui" },
    breakpoint: { direction: "up", identifier: "@media screen" },
    defaultColorMode: "dark",
    defaultThemeScheme: "base",
    //notice: { duration: 5000 },
    theme: { responsive: true },
});

// Register the router instance for type safety
declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}
const router = createRouter({
    routeTree,
    context: { queryClient },
    defaultPreload: "intent",
    //defaultPreloadStaleTime: 0,
    // defaultStructuralSharing: true,
    //defaultPendingMinMs: 0,
    //defaultPendingMs: 100,
    scrollRestoration: false,
    defaultViewTransition: {
        types: ({ fromLocation, toLocation }) => {
            let direction = "none";

            if (fromLocation) {
                const fromIndex = fromLocation.state.__TSR_index;
                const toIndex = toLocation.state.__TSR_index;
                if (fromIndex !== toIndex) {
                    direction = fromIndex > toIndex ? "right" : "left";
                }
            }

            if (
                fromLocation?.state.__TSR_index ===
                toLocation?.state.__TSR_index
            ) {
                direction = "none";
            }

            return [`slide-${direction}`];
        },
    },
});
// TODO: KEEP SELECTED FILTETRS ALWAYS INTOP!
const rootElement = document.getElementById("root")!;
if (!rootElement.innerHTML) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <StrictMode>
            <UIProvider config={config} theme={extendedTheme}>
                <QueryClientProvider client={queryClient}>
                    <RouterProvider router={router} />
                </QueryClientProvider>
            </UIProvider>
        </StrictMode>
    );
}

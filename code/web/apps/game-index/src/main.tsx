import "./styles.css";
import "./fonts.css";
import { QueryClientProvider } from "@tanstack/react-query";

import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import {
    UIProvider,
    defineConfig,
    extendTheme,
    defineStyles,
} from "@packages/ui";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import { queryClient } from "./QueryClient";
// Create a new router instance

const customLayerStyles = defineStyles.layerStyle({
    glass: {
        bg: "colorScheme.200/40",
        backdropFilter: "blur(10px)",
        borderRadius: "xl",
        transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
    },
    "glass.hover": {
        bg: "colorScheme.200/60",
        transform: "translateY(-4px)",
        boxShadow: "shadows.cardHover",
    },
});

const extendedTheme = extendTheme({
    fonts: {
        body: '"Montserrat Variable", "sans-serif"',
        heading: '"Montserrat Variable", "sans-serif"',
        //heading: '"Inter", "Inter Fallback"',
        mono: '"Geist Mono", "Geist Mono Fallback"',
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
                bg: "bg.base",
            },
            html: {
                scrollbarWidth: "thin",
                //overflowY: "scroll",
                scrollBehavior: "smooth",
                //scrollbarGutter: "stable",
            },
        },
        layerStyles: customLayerStyles,
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

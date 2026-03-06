import "./styles.css";
import "./fonts.css";
import {
    useQuery,
    useMutation,
    useQueryClient,
    QueryClient,
    QueryClientProvider,
} from "@tanstack/react-query";

import { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import { UIProvider, defineConfig, extendTheme } from "@packages/ui";
// import { my_theme } from "@packages/theme";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";

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
        colors: {
            /*
            black: {
                base: "gray.90",
                bg: "gray.800", // menu/panels
                subtle: "gray.700", // inputs
                muted: "gray.600", // hover
                fg: "whiteAlpha.900",
                contrast: "white",
                ghost: "whiteAlpha.100",
                outline: "gray.700",
                solid: "black.700",
            },
            white: {
                base: "#ffffff",
                //base: "bg.float",
                bg: "#161616",
                contrast: "black",
                emphasized: "white.400/50",
                fg: "white.900",
                ghost: "white.200/50",
                muted: "white.300/50",
                outline: "white.800",
                solid: "white",
                subtle: "white.200/50",
            },
        },
        */
        },
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

const queryClient = new QueryClient();

// Register the router instance for type safety
declare module "@tanstack/react-router" {
    interface Register {
        router: typeof router;
    }
}

const router = createRouter({
    routeTree,
    defaultPreload: "intent",
    scrollRestoration: true,
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

const rootElement = document.getElementById("root")!;
if (!rootElement.innerHTML) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <StrictMode>
            <UIProvider config={config} theme={extendedTheme}>
                <RouterProvider router={router} />
            </UIProvider>
        </StrictMode>
    );
}
